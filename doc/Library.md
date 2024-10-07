# Ookii.AnswerFile library [![NuGet](https://img.shields.io/nuget/v/Ookii.AnswerFile)](https://nuget.org/packages/Ookii.AnswerFile)

The core functionality of the Answer File Generator is implemented in the Ookii.AnswerFile library,
which you can integrate into your own applications. The library requires .Net 8.0 or later, and is
available on [NuGet](https://nuget.org/packages/Ookii.AnswerFile).

To generate an answer file, you invoke the [`AnswerFileGenerator.Generate`][] method, passing in an
instance of the [`AnswerFileOptions`][] class, which describes the various settings you want to use
in the answer file.

To specify the install method, you set the [`AnswerFileOptions.InstallOptions`][] property to an
instance of the [`CleanEfiOptions`][], [`CleanBiosOptions`][], [`ExistingPartitionOptions`][], or
[`ManualInstallOptions`][] class. Additional options that are specific to an install method, such as
partition layout or optional features, are set in those classes.

Leave the [`AnswerFileOptions.InstallOptions`][] property set to null to generate an answer file for
a pre-installed image, such as one created by sysprep or DISM tools.

The below example sets various options to perform a clean installation on a UEFI-based system,
enabling some optional features and remote desktop. It creates a user account, and sets it to be
logged on automatically at first boot.

```csharp
var options = new AnswerFileOptions()
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

AnswerFileGenerator.Generate("unattend.xml", options);
```

For more information, check out the
[class library documentation](https://www.ookii.org/Link/GenerateAnswerFileDoc).

## Breaking changes

Version 2.0 of the library has a few breaking changes from version 1.x:

- The `Generator` class was renamed to [`AnswerFileGenerator`][].
- The `GeneratorOptions` class was renamed to [`AnswerFileOptions`][].
- The [`AnswerFileOptions.DisplayResolution`][] property has a different type.
- The [`AnswerFileOptions.JoinDomain`][] property has a different type.
- The `AnswerFileOptions.CmdKeyAccount` property has been removed.
- The [`DomainOptions`][] class now derives from the [`DomainOptionsBase`][] class.
- The [`DomainOptionsBase.DomainAccounts`][] property has a different type.
- The `AnswerFileOptions.SetupScripts` property was renamed to [`FirstLogonScripts`][].

[`AnswerFileGenerator.Generate`]: https://www.ookii.org/docs/answerfile-2.0/html/Overload_Ookii_AnswerFile_AnswerFileGenerator_Generate.htm
[`AnswerFileGenerator`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_AnswerFileGenerator.htm
[`AnswerFileOptions.DisplayResolution`]: https://www.ookii.org/docs/answerfile-2.0/html/P_Ookii_AnswerFile_AnswerFileOptions_DisplayResolution.htm
[`AnswerFileOptions.InstallOptions`]: https://www.ookii.org/docs/answerfile-2.0/html/P_Ookii_AnswerFile_AnswerFileOptions_InstallOptions.htm
[`AnswerFileOptions.JoinDomain`]: https://www.ookii.org/docs/answerfile-2.0/html/P_Ookii_AnswerFile_AnswerFileOptions_JoinDomain.htm
[`AnswerFileOptions`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_AnswerFileOptions.htm
[`CleanBiosOptions`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_CleanBiosOptions.htm
[`CleanEfiOptions`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_CleanEfiOptions.htm
[`DomainOptions`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_DomainOptions.htm
[`DomainOptionsBase.DomainAccounts`]: https://www.ookii.org/docs/answerfile-2.0/html/P_Ookii_AnswerFile_DomainOptionsBase_DomainAccounts.htm
[`DomainOptionsBase`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_DomainOptionsBase.htm
[`ExistingPartitionOptions`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_ExistingPartitionOptions.htm
[`FirstLogonScripts`]: https://www.ookii.org/docs/answerfile-2.0/html/P_Ookii_AnswerFile_AnswerFileOptions_FirstLogonScripts.htm
[`ManualInstallOptions`]: https://www.ookii.org/docs/answerfile-2.0/html/T_Ookii_AnswerFile_ManualInstallOptions.htm
