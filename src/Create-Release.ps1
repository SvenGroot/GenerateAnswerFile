param(
    [Parameter()][Switch]$PackageOnly,
    [Parameter()][string]$LocalNugetPath
)

# This script is used to create a distribution folder that can be packaged into a zip file for release.
$projects = @("Ookii.AnswerFile","GenerateAnswerFile")
$publishDir = Join-Path $projects[1] "bin" "publish"
$zipDir = Join-Path $publishDir "zip"
New-Item $publishDir -ItemType Directory -Force | Out-Null
Remove-Item "$publishDir/*" -Recurse -Force
New-Item $zipDir -ItemType Directory -Force | Out-Null

[xml]$projectFile = Get-Content (Join-Path $PSScriptRoot "$($projects[1])/$($projects[1]).csproj")
$framework = ($projectFile.Project.PropertyGroup.TargetFramework | Where-Object { $null -ne $_ })
[xml]$props = Get-Content (Join-Path $PSScriptRoot "Directory.Build.Props")
$versionPrefix = $props.Project.PropertyGroup.VersionPrefix
$versionSuffix = $props.Project.PropertyGroup.VersionSuffix
if ($versionSuffix) {
    $version = "$versionPrefix-$versionSuffix"
} else {
    $version = $versionPrefix
}


# Clean and build deterministic.
dotnet clean "$PSScriptRoot" --configuration Release

# Build each project and framework separately. Without this, the nuget package almost always
# contains at least one pdb that isn't actually deterministic, even though we build with the
# ContinuousIntegrationBuild property. I don't really understand why, but this seems to work around
# it.
foreach ($project in $projects) {
    "Processing $project"
    dotnet build "$PSScriptRoot/$project" --configuration Release /p:ContinuousIntegrationBuild=true
    # Uncomment the below to use mdv (https://github.com/dotnet/metadata-tools) to verify the
    # build was actually deterministic.
    # $sources = mdv "$PSScriptRoot/$project/bin/Release/$framework/$project.pdb" | Where-Object { $_.Contains("/_/src" ) }
    # if ($sources.Length -eq 0) {
    #     throw "$project, $framework is not deterministic"
    # }

    "Creating package..."
    if ($project -ne "GenerateAnswerFile") {
        dotnet pack "$PSScriptRoot/$project" --no-build --configuration Release --output "$publishDir"
    }
}

if (-not $PackageOnly) {
    # Publish the executable.
    $project = $projects[1]
    dotnet publish --no-build "$PSScriptRoot/$project" --configuration Release --framework $framework --output "$zipDir"

    # Copy global items.
    Copy-Item "$PSScriptRoot/../LICENSE.txt" $zipDir

    # Create readme.txt files.
    $url = "https://github.com/SvenGroot/GenerateAnswerFile"
    "GenerateAnswerFile $version","For documentation and other information, see:",$url | Set-Content "$zipDir/readme.txt"

    Compress-Archive "$zipDir/*" "$publishDir/GenerateAnswerFile-$version.zip"

    # Generate AOT binary
    foreach ($arch in "x64", "arm64") {
        $runtime = "win-$arch"
        $outputDir = Join-Path $publishDir $runtime
        dotnet publish "$PSScriptRoot/$project" --configuration Release --framework $framework --runtime $runtime --output "$outputDir" /p:PublishAot=true
        Copy-Item (Join-Path $outputDir "GenerateAnswerFile.exe") (Join-Path $publishDIr "GenerateAnswerFile-$version-$arch.exe")
    }
}

if ($LocalNugetPath) {
    Copy-Item "$publishDir\*.nupkg" $LocalNugetPath
    Copy-Item "$publishDir\*.snupkg" $LocalNugetPath
    Remove-Item -Recurse "~/.nuget/packages/ookii.answerfile/*"
}
