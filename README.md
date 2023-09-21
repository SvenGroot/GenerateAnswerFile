# Windows Answer File Generator

The Answer File Generator is a command line application and [library](#ookiianswerfile-library) for
generating answer files for unattended Windows installations. These files are commonly called
unattend.xml or autounattend.xml (depending on the installation method used).

This tool can be used to generate answer files as part of an automated workflow for installing
Windows, or just as a convenient way to generate answer files for personal use, without the need to
install the Windows System Image Manager, or manually edit XML files.

Answer files can customize many aspects of the Windows installation, only some of which are
available through this tool. Customizations supported by the Answer File Generator include:

- The installation method, partition layout, and target disk and partition.
- Enabling optional features during installation.
- Creation of local user accounts.
- Joining a domain and adding domain accounts to the local Administrators group.
- Configuring automatic log-on.
- The product key, computer name, language/culture, and time zone.
- Display resolution.
- Disabling Windows Defender.
- Enabling remote desktop access.
- Running PowerShell scripts and other commands on first log-on.

All of these items can be customized using command line arguments. In addition, the answer files
generated here will always skip the entire OOBE experience unless no local account was created and
no domain was joined.

For a full list of command line arguments, run `./GenerateAnswerFile -Help`.

If you need additional customization, you will need to edit the generated answer file. If you'd like
any other options to be available through the tool, you can
[file an issue](https://github.com/SvenGroot/GenerateAnswerFile/issues) or
[submit a pull request](https://github.com/SvenGroot/GenerateAnswerFile/pulls).

## Installation method

The Answer File Generator supports several methods of installing Windows, which are specified using
the `-Install` argument. The following values are supported.

Method                | Description
----------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
**PreInstalled**      | This is the default method, which is used to customize an already installed Windows image, such as one created using sysprep, or an image that was manually expanded using DISM tools such as [`Expand-WindowsImage`](https://learn.microsoft.com/powershell/module/dism/expand-windowsimage). Some features are unavailable using this method, including installation of optional features.
**CleanEfi**          | Performs a clean installation for systems using UEFI on the disk indicated by the `-InstallToDisk` argument. This disk will be repartitioned according to the `-Partition` argument, or using a default layout of a 100MB EFI partition, a 128MB MSR partition, and the remainder of the disk used for the OS installation.
**CleanBios**         | Performs a clean installation for systems using legacy BIOS on the disk indicated by the `-InstallToDisk` argument. This disk will be repartitioned according to the `-Partition` argument, or using a default of a 100MB system partition, and the remainder of the disk used for the OS installation.
**ExistingPartition** | Installs Windows to an existing partition, specified using the `-InstallToDisk` and `-InstallToPartition` arguments. This partition will be formatted before installation, but other partitions are not touched. The system must already have a suitable system or EFI partition.
**Manual**            | Allows the user to specify the target disk/partition during setup. When using this method, installation is not fully unattended and will require user intervention during the first stage.

The `-InstallToDisk` argument takes the zero-based disk index, in the order that the Windows
installation (when executed manually) would list them. The `-InstallToPartition` argument is
one-based instead (don't look at me; that's how the IDs work in the answer file).

## Selecting the edition to install

Usually, Windows installation media contains multiple editions, such as Professional or Home, and
you must select which edition to install. The most common way to do this is using the product key,
which can be set using the `-ProductKey` argument. Setting the product key will select the correct
edition to install, and activate Windows using that key. Setting a product key in the answer file is
required for most installations of Windows unless the installation method is `PreInstalled`.

Versions of Windows that use alternative activation methods, such as volume licensing, do not
require a product key in the answer file. In this case, if the installation media holds multiple
editions, you can select the desired one using the `-ImageIndex` argument. You can use the
[`Get-WindowsImage`](https://learn.microsoft.com/powershell/module/dism/get-windowsimage) PowerShell
command to list the images in an install.wim or install.esd file.

## Passwords in answer files

Passwords are needed for several actions taken by the answer file. When creating local accounts,
their initial password must be set. To join a domain, the password of a domain account with
the appropriate permissions must be specified. For automatic log-on, you must specify the password
of the account that will be logged on.

:warning: **Passwords in answer files are not securely stored** :warning:

They are at best base64-encoded (which is easily reversible), and at worst just stored in plain
text. Do not store answer files with sensitive passwords in unsecure locations, and delete such
files when you are done with them.

The answer file generator also does not treat these passwords securely (it can't, to be able to
write them out to the answer file in this fashion), so copies of the passwords may remain in system
memory after the application is terminated.

## Time zone

The Answer File Generator defaults to Pacific Standard Time. To change this, use the `-TimeZone`
argument. Run `tzutil /l` for a list of accepted time zone names.

## Examples

To make them easier to read, the examples below are split over multiple lines, using the PowerShell
syntax to continue a command on the next line.

### Clean installation using UEFI

```text
./GenerateAnswerFile autounattend.xml `
    -Install CleanEfi `
    -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

The resulting answer file will install a 64 bit version of Windows on the first disk of a UEFI
system, and activates it using the specified product key.

### Installing a 32 bit OS

```text
./GenerateAnswerFile autounattend.xml `
    -Install CleanBios `
    -ProcessorArchitecture x86 `
    -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

By default, the generated answer files are for 64 bit editions of Windows. Use the
`-ProcessorArchitecture` argument to specify a different CPU architecture.

### Creating a user during installation

```text
./GenerateAnswerFile autounattend.xml `
    -Install CleanEfi `
    -LocalAccount "John,Password" `
    -AutoLogonUser John `
    -AutoLogonPassword Password `
    -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

This example creates a user named "John" with the password "Password" (don't do this, obviously),
and logs in with that user automatically on first boot[^1]. To log in automatically more often, use
the `-AutoLogonCount` argument.

The `-LocalAccount` argument takes multiple values, if you want to create more than one account.

### Joining a domain and automatic log-on

```text
./GenerateAnswerFile unattend.xml `
    -ComputerName my-pc `
    -JoinDomain mydomain `
    -JoinDomainUser domainuser `
    -JoinDomainPassword Password `
    -DomainAccount domainuser `
    -AutoLogonUser mydomain\domainuser `
    -AutoLogonPassword Password
```

The answer file created by this command sets the computer name to "my-pc" and joins it to the domain
"mydomain", using the supplied credentials. It also adds the account "domainuser" to the local
administrators group and logs in using that account automatically on first boot[^1].

This sample does not use the `-Install` argument, so it creates an answer file suitable for
pre-installed Windows images, such as those created using sysprep or DISM tools. The `-JoinDomain`
argument can be used with any install method, however.

### Custom partition layout

If you use the `CleanEfi` or `CleanBios`, you can choose to customize the partition layout for the
disk specified by `-InstallToDisk`, by using the `-Partition` argument. This argument accepts
multiple values, each creating a partition on that disk in the order specified. If this argument is
not specified, a default partition layout is used.

Each partition uses the format `label:size`, where label is the volume label, and size is the size
of the partition. The size can use multiple-byte units, such as GB or MB, and will be rounded down
to a whole number of megabytes. If the size is `*`, it indicates the partition will fill the
remainder of the disk.


```text
./GenerateAnswerFile autounattend.xml `
    -Install CleanEFI
    -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
    -Partition System:100MB MSR:128MB Windows:256GB Data:*
```

This example creates four partitions: a 100 MB EFI partition, a 128 MB Microsoft Reserved partition,
a 256 GB partition that the OS will be installed to, and an additional partition that fills the
remainder of the disk.

Several values for the volume label are used to create special partition types.

Label                     | Meaning
--------------------------|-----------------------------------------------------------------------------------------------------------------------------------
**System**                | For `CleanEfi`, creates the special EFI partition. For `CleanBios`, creates the system partition holding the Windows boot manager.
**MSR**                   | Creates a partition with the Microsoft Reserved partition type. For use with `CleanEfi` only.
**WinRE** or **Recovery** | Marks the partition as a utility partition, and does not assign it a drive letter.

All other volume labels create regular data partitions with that label. These will be assigned
drive letters in the order they were specified, starting with `C:`.

You can use the `-InstallToPartition` argument to specify which partition should hold the OS. If you
don't supply this argument, Windows will be installed on the first regular data partition.

If you use `CleanBios` and specify more than four partitions, the Answer File Generator will create
an answer file that creates an extended partition, and creates the remaining partitions as logical
volumes in that partition.

### Optional features

Answer files can be used to enable optional features during installation. To do this, use the
`-Feature` argument. This argument can take multiple values to enable multiple features.

```text
./GenerateAnswerFile autounattend.xml `
    -Install CleanEfi `
    -Feature Microsoft-Windows-Subsystem-Linux VirtualMachinePlatform `
    -WindowsVersion 10.0.22621.1
```

This example enables the Windows Subsystem for Linux feature, and the Virtual Machine Platform
feature.

When using optional features, the answer file must contain the exact version number of the Windows
version being installed, such as "10.0.22621.1" (this is the version for Windows 11 22h2). You must
specify this version using the `-WindowsVersion` argument. To find out the exact version number, the
easiest way is to look at the file properties of the setup.exe file on your Windows installation
media. The Windows version is not needed if you don't enable any optional features.

To determine the names of available optional features, you can use the
[`Get-WindowsOptionalFeature`](https://learn.microsoft.com/powershell/module/dism/get-windowsoptionalfeature)
PowerShell command.

You cannot specify optional features when using the `PreInstalled` method, as this is not supported
by unattended installations. Optional features for a pre-installed image must be enabled prior to
using sysprep, or by using DISM tools.

### First log-on commands and scripts

```text
./GenerateAnswerFile autounattend.xml `
    -Install CleanEfi `
    -FirstLogonCommand "reg add "HKCU\Software\MyCompany\" /v ImportantRegistryKey /t REG_DWORD /d 1 /f" `
    -SetupScript "\\server\share\script.ps1 -Argument" `
    -LocalAccount "John,Password" `
    -AutoLogonUser John `
    -AutoLogonPassword Password `
    -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

The `-FirstLogonCommand` argument can be used to execute a command when a user first logs on to the
system after installation (either manually, or automatically as in the above example). For
convenience, there is also a `-SetupScript` argument which executes the specified Windows PowerShell
script, including any arguments.

Either argument accepts multiple values to run multiple commands or scripts. Both are executed in
the order they are supplied, but all commands will be executed before any scripts.

If you execute any scripts, they must be stored in a location that is accessible to the system after
installation, such as a network share like the example above.

## Using an answer file

Please refer to the [official Microsoft documentation](https://learn.microsoft.com/windows-hardware/manufacture/desktop/windows-setup-automation-overview)
to see how to use an answer file during Windows setup.

## Ookii.AnswerFile library

The core functionality for generating answer files is implemented in the Ookii.AnswerFile library,
which you can use in your own applications targeting .Net 7.0 or later.

The library is available on [NuGet ![NuGet](https://img.shields.io/nuget/v/Ookii.AnswerFile)](https://nuget.org/packages/Ookii.AnswerFile).
For more information, check out the [class library documentation](https://www.ookii.org/Link/GenerateAnswerFileDoc).

## Disclaimer

The Answer File Generator will generate files that, when used to install Windows, can erase a disk
or partition on your system without user intervention. I am not responsible for any loss of data or
any other adverse effects caused by the use of answer files generated by this tool.

[^1]: Windows has a [known issue with the `LogonCount` element](https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-shell-setup-autologon-logoncount)
    which causes it to be inaccurate. The Answer File Generator adjusts the count and uses a first
    log-on command if needed to ensure the `-AutoLogonCount` argument is accurate.
