# Ookii.AnswerFile library [![NuGet](https://img.shields.io/nuget/v/Ookii.AnswerFile)](https://nuget.org/packages/Ookii.AnswerFile)

The core functionality of the Answer File Generator is implemented in the Ookii.AnswerFile library,
which you can integrate into your own applications. The library requires .Net 8.0 or later, and is
available on [NuGet](https://nuget.org/packages/Ookii.AnswerFile).

To generate an answer file, you invoke the [`Generator.Generate`][] method, passing in an instance
of the [`GeneratorOptions`][] class, which describes the various settings you want to use in the
answer file.

To specify the install method, you set the [`GeneratorOptions.InstallOptions`][] property to an
instance of the [`CleanEfiOptions`][], [`CleanBiosOptions`][], [`ExistingPartitionOptions`][], or
[`ManualInstallOptions`][] class. Additional options that are specific to an install method, such as
partition layout or optional features, are set in those classes.

Leave the [`GeneratorOptions.InstallOptions`][] property set to null to generate an answer file for
a pre-installed image, such as one created by sysprep or DISM tools.

The below example sets various options to perform a clean installation on a UEFI-based system,
enabling some optional features and remote desktop. It creates a user account, and sets it to be
logged on automatically at first boot.

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
    ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE",
    DisplayResolution = new Resolution(1920, 1080)
};

Generator.Generate("unattend.xml", options);
```

For more information, check out the
[class library documentation](https://www.ookii.org/Link/GenerateAnswerFileDoc).

## Breaking changes

Version 2.0 of the library has a few breaking changes from version 1.x:

- The `DomainOptions.DomainAccounts` property has a different type.
- The `GeneratorOptions.DisplayResolution` property has a different type.
- The `GeneratorOptions.JoinDomain` property has a different type.
- The `GeneratorOptions.CmdKeyAccount` property has been removed.

[`CleanBiosOptions`]: https://www.ookii.org/docs/answerfile-1.1/html/T_Ookii_AnswerFile_CleanBiosOptions.htm
[`CleanEfiOptions`]: https://www.ookii.org/docs/answerfile-1.1/html/T_Ookii_AnswerFile_CleanEfiOptions.htm
[`ExistingPartitionOptions`]: https://www.ookii.org/docs/answerfile-1.1/html/T_Ookii_AnswerFile_ExistingPartitionOptions.htm
[`Generator.Generate`]: https://www.ookii.org/docs/answerfile-1.1/html/Overload_Ookii_AnswerFile_Generator_Generate.htm
[`GeneratorOptions.InstallOptions`]: https://www.ookii.org/docs/answerfile-1.1/html/P_Ookii_AnswerFile_GeneratorOptions_InstallOptions.htm
[`GeneratorOptions`]: https://www.ookii.org/docs/answerfile-1.1/html/T_Ookii_AnswerFile_GeneratorOptions.htm
[`ManualInstallOptions`]: https://www.ookii.org/docs/answerfile-1.1/html/T_Ookii_AnswerFile_ManualInstallOptions.htm
