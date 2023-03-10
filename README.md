# Windows Answer File Generator

The Answer File Generator is a command line application and library for generating answer files for
unattended Windows installations. These files are commonly called unattend.xml or autounattend.xml
(depending on the installation method used).

This tool can be used to generate answer files as part of an automated workflow for installing
Windows, or just as a convenient way to generate answer files for personal use, without the need to
install the Windows System Image Manager, or manually edit XML files.

Answer files can customize many aspects of the Windows installation, only some of which are
available through this tool. Customizations supported by the Answer File Generator include:

- The installation method and target disk and partition.
- Enabling optional features during installation.
- Creation of local user accounts.
- Joining a domain and adding domain accounts to the local Administrators group.
- Configuring automatic log-on.
- The product key, computer name, language/culture, and time zone.
- Display resolution.
- Disabling Windows Defender.
- Enabling remote desktop access.
- Running PowerShell scripts on first log-on.

All of these items can be customized using command line arguments. In addition, the answer files
generated here will always skip the entire OOBE experience unless no local account was created.

For a full list of command line arguments, run `./GenerateAnswerFile -Help`.

If you need additional customization, you will need to edit the generated unattend.xml file. If
you'd like any other options to be available through the tool, you can
[file an issue](https://github.com/SvenGroot/GenerateAnswerFile/issues) or
[submit a pull request](https://github.com/SvenGroot/GenerateAnswerFile/pulls).

## Installation method

The Answer File Generator supports several methods of installing Windows, which are specified using
the `-Install` argument. The following values are supported.

Method                | Description
----------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
**PreInstalled**      | This is the default method, which is used to customize an already installed Windows image, such as one created using sysprep, or an image that was manually expanded using DISM tools such as [`Expand-WindowsImage`](https://learn.microsoft.com/powershell/module/dism/expand-windowsimage). Some features are unavailable using this method, including installation of optional features.
**CleanEfi**          | Performs a clean installation for systems using UEFI on the disk indicated by the `-InstallToDisk` argument. This disk will be repartitioned using a partition scheme that's appropriate for UEFI systems, with a 100MB EFI partition, a 128MB MSR partition, and the remainder of the disk used for the OS installation.
**CleanBios**         | Performs a clean installation for systems using legacy BIOS on the disk indicated by the `-InstallToDisk` argument. This disk will be repartitioned using a partition scheme that's appropriate for BIOS systems, with a 100MB system partition, and the remainder of the disk used for the OS installation.
**ExistingPartition** | Installs Windows to an existing partition, specified using the `-InstallToDisk` and `-InstallToPartition` arguments. This partition will be formatted before installation, but other partitions are not touched. The system must already have a suitable system or EFI partition.
**Manual**            | Allows the user to specify the target disk/partition during setup. When using this method, installation is not fully unattended and will require user intervention during the first stage.

The `-InstallToDisk` argument takes the zero-based disk index, in the order that the Windows
installation (when executed manually) would list them. The `-InstallToPartition` argument is
one-based instead (don't look at me; that's how the IDs work in the answer file).

## Selecting the edition to install

Usually, Windows installation media contains multiple editions, such as Professional or Home, and
you must select which edition to install. The most common way to do this is using the product key,
which can be set using the `-ProductKey` argument. Setting the product key will select the correct
edition to install, and activate Windows using that key. Setting a product key in an answer file is
required for most installations of Windows.

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

### Clean installation using UEFI

```text
./GenerateAnswerFile autounattend.xml -Install CleanEfi -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

The resulting answer file will install a 64 bit version of Windows on the first disk of a UEFI
system, and activates it using the specified product key.

### Installing a 32 bit OS

```text
./GenerateAnswerFile autounattend.xml -Install CleanBios -ProcessorArchitecture x86 -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

By default, the generated answer files are for 64 bit (amd64) editions of Windows. Use the
`-ProcessorArchitecture` argument to specify a different CPU architecture.

### Creating a user during installation

```text
./GenerateAnswerFile autounattend.xml -Install CleanEfi -LocalAccount John,Password -AutoLogonUser John -AutoLogonPassword Password -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

This creates a user named "John" with the password "Password" (don't do this, obviously), and logs
in with that user automatically on first boot[^1]. To log in automatically more often, use the
`-AutoLogonCount` argument.

The `-LocalAccount` argument takes multiple values, if you want to create more than one account.

### Joining a domain

```text
./GenerateAnswerFile unattend.xml -ComputerName my-pc -JoinDomain mydomain -JoinDomainUser domainuser -JoinDomainPassword Password -DomainAccount domainuser -AutoLogonUser mydomain\domainuser -AutoLogonPassword Password
```

This answer file sets the computer name to "my-pc" and joins it to the domain "mydomain" using the
supplied credentials. It also adds the account "domainuser" to the local administrators group and
logs in using that account automatically on first boot[^1].

This sample does not use the `-Install` argument, so it creates an answer file suitable for
pre-installed Windows images, such as those created using sysprep. The `-JoinDomain` argument can
be used with any install method, however.

### Optional features

Answer files can be used to enable optional features during installation. To do this, use the
`-Feature` argument. This argument can take multiple values to enable multiple features.

```text
./GenerateAnswerFile autounattend.xml -Install CleanEfi -Feature Microsoft-Windows-Subsystem-Linux VirtualMachinePlatform -WindowsVersion 10.0.22621.1
```

This example enables the Windows Subsystem for Linux feature, and the Virtual Machine Platform feature.

When using optional features, the answer file must contain the exact version number of the Windows
version being installed, such as "10.0.22621.1" (this is the version for Windows 11 22h2). You must
specify this version using the `-WindowsVersion` argument. To find out the exact version number, the
easiest way is to look at the file properties of the setup.exe file on your Windows installation
media. The Windows version is not needed if you don't enable any optional components.

To determine the names of available optional features, you can use the
[`Get-WindowsOptionalFeature`](https://learn.microsoft.com/powershell/module/dism/get-windowsoptionalfeature)
PowerShell command.

### First log-on scripts

```text
./GenerateAnswerFile autounattend.xml -Install CleanEfi -SetupScript "\\server\share\script.ps1 -Argument" -LocalAccount John,Password -AutoLogonUser John -AutoLogonPassword Password -ProductKey ABCDE-12345-ABCDE-12345-ABCDE
```

The `-SetupScript` argument can be used to specify one or more PowerShell scripts to execute during
first log-on. This argument can take multiple values to specify multiple scripts, which will be
executed in the order they are supplied. The argument value may include script arguments.

Scripts specified with this argument are executed with PowerShell.exe, so must always be
Windows PowerShell scripts. If at least one setup script is used, the answer file will also contain
commands to set the PowerShell execution policy to enable script execution.

The scripts must be stored in a location that is accessible to the system after installation, such
as a network share like the example above.

## Using an answer file

Please refer to the [official Microsoft documentation](https://learn.microsoft.com/windows-hardware/manufacture/desktop/windows-setup-automation-overview) to see how to use an answer file during Windows setup.

## Ookii.AnswerFile library

The core functionality for generating answer files is implemented in the Ookii.AnswerFile library,
which you can use in your own applications targeting .Net 7.0 or later.

The library is available on [NuGet ![NuGet](https://img.shields.io/nuget/v/Ookii.AnswerFile)](https://nuget.org/packages/Ookii.AnswerFile).
For more information, check out the [class library documentation](https://www.ookii.org/Link/GenerateAnswerFileDoc).

## Disclaimer

The Answer File Generator will generate files that, when used to install Windows, can erase a disk
or partition on your system without user intervention. I am not responsible for any loss of data or
any other adverse effects caused by the use of answer files generated by this tool.

[^1]: Automatic log-on will actually occur twice, due to a [known issue with the `LogonCount` element](https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-shell-setup-autologon-logoncount).
