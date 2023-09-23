using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for installing optional features.
/// </summary>
/// <threadsafety instance="false" static="true"/>
public class OptionalFeatures
{
    private Collection<string>? _features;

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionalFeatures"/> class.
    /// </summary>
    /// <param name="windowsVersion">The exact Windows version being installed (e.g. "10.0.22000.1").</param>
    public OptionalFeatures(Version windowsVersion)
    {
        ArgumentNullException.ThrowIfNull(windowsVersion);
        WindowsVersion = windowsVersion;
    }

    /// <summary>
    /// Gets the Windows version being installed.
    /// </summary>
    /// <value>
    /// The Windows version.
    /// </value>
    public Version WindowsVersion { get; }

    /// <summary>
    /// Gets or sets the optional features to install.
    /// </summary>
    /// <value>
    /// A collection of optional features.
    /// </value>
    /// <remarks>
    /// Use the PowerShell 'Get-WindowsOptionalFeature' command to get a list of valid feature
    /// names.
    /// </remarks>
    public Collection<string> Features
    {
        get => _features ??= new();
        set => _features = value;
    }

    internal void GenerateServicingPass(Generator generator)
    {
        using var servicing = generator.Writer.WriteAutoCloseElement("servicing");
        using var package = generator.Writer.WriteAutoCloseElement("package", new { action = "configure" });
        generator.Writer.WriteEmptyElement("assemblyIdentity", new
        {
            name = "Microsoft-Windows-Foundation-Package",
            version = WindowsVersion,
            processorArchitecture = generator.Options.ProcessorArchitecture,
            publicKeyToken = Generator.PublicKeyToken,
            language = ""
        });

        foreach (var component in Features)
        {
            generator.Writer.WriteEmptyElement("selection", new { name = component, state = "true" });
        }
    }
}
