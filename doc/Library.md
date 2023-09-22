# Ookii.AnswerFile library [![NuGet](https://img.shields.io/nuget/v/Ookii.AnswerFile)](https://nuget.org/packages/Ookii.AnswerFile)

The core functionality of the Answer File Generator is implemented in the Ookii.AnswerFile library,
which you can integrate into your own applications. The library requires .Net 7.0 or later, and is
available on [NuGet](https://nuget.org/packages/Ookii.AnswerFile).

To generate an answer file, you invoke the `Generator.Generate` method, passing in an instance of
the `GeneratorOptions` class, which describes the various settings you want to use in the answer
file.

To specify the install method, you set the `GeneratorOptions.InstallOptions` property to an instance
of the `CleanEfiOptions`, `CleanBiosOptions`, `ExistingPartitionOptions`, or `ManualOptions` class.
Additional options that are specific to an install method, such as partition layout or optional
features, are set in those classes.

Leave the `GeneratorOptions.InstallOptions` set to null to generate an answer file for a
pre-installed image, such as one created by sysprep or DISM tools.

The below sample sets various options to perform a clean installation on a UEFI-based system,
enabling some optional features and remote desktop. It creates a user account, and sets it be logged
on automatically at first boot.

```csharp
var options = new GeneratorOptions()
{
    InstallOptions = new CleanEfiOptions()
    {
        OptionalFeatures = new OptionalFeatures(new Version(10, 0, 22621, 1))
        {
            Features = { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }
        }
    },
    EnableRemoteDesktop = true,
    LocalAccounts = { new LocalCredential("MyUser", "Password") },
    AutoLogon = new AutoLogonOptions(new DomainUser(null, "MyUser"), "Password"),
    ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE"
    DisplayResolution = new Size(1280, 1024)
};

Generator.Generate("unattend.xml", options);
```

For more information, check out the
[class library documentation](https://www.ookii.org/Link/GenerateAnswerFileDoc).
