# Command line arguments

This page describes the command line arguments supported by the Answer File Generator, which are
used to customize the generated answer file. All argument names are case insensitive.

Argument values can be specified as either e.g. `-JoinDomain mydomain`, `-JoinDomain:mydomain` or
`-JoinDomain=mydomain`.

Some arguments can be specified multiple times. This can be done by listing several values after
the argument:

```text
-LocalAccount "John,Password" "Dave,OtherPassword" -EnableRemoteDesktop
```

Or, by repeating the argument multiple times, potentially interleaving other arguments:

```text
-LocalAccount "John,Password" -EnableRemoteDesktop -LocalAccount "Dave,OtherPassword"
```

This syntax makes Answer File Generator compatible with
[PowerShell hash table splatting](https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/about_splatting),
which can be a convenient way to handle invocations with many arguments.

```powershell
$arguments = @{
    "OutputPath" = "unattend.xml"
    "InstallMethod" = "CleanEfi"
    "Feature" = "Microsoft-Windows-Subsystem-Linux","VirtualMachinePlatform"
    "WindowsVersion" = "10.0.22621.1"
    "EnableRemoteDesktop" = $true
}

./GenerateAnswerFile @arguments
```

You can also use [JSON files](Json.md) to specify options for generating an answer file. In this
case, only the `-OutputFile` argument can be used.

The arguments are split into several categories:

- [General options](#general-options)
- [Installation options](#installation-options)
- [User account options](#user-account-options)
- [Domain options](#domain-options)
- [Other setup options](#other-setup-options)

## Usage syntax

<!-- markdownlint-disable MD033 -->
<pre>GenerateAnswerFile
    [[<a href="#-outputfile">-OutputFile</a>] &lt;Path&gt;]
    [<a href="#-autologoncount">-AutoLogonCount</a> &lt;Number&gt;]
    [<a href="#-autologonpassword">-AutoLogonPassword</a> &lt;String&gt;]
    [<a href="#-autologonuser">-AutoLogonUser</a> &lt;[Domain\]User&gt;]
    [<a href="#-computername">-ComputerName</a> &lt;String&gt;]
    [<a href="#-disablecloud">-DisableCloud</a>]
    [<a href="#-disabledefender">-DisableDefender</a>]
    [<a href="#-disableservermanager">-DisableServerManager</a>]
    [<a href="#-displayresolution">-DisplayResolution</a> &lt;Resolution&gt;]
    [<a href="#-domainaccount">-DomainAccount</a> &lt;[Group:][Domain\]User&gt;...]
    [<a href="#-enableremotedesktop">-EnableRemoteDesktop</a>]
    [<a href="#-feature">-Feature</a> &lt;String&gt;...]
    [<a href="#-firstlogoncommand">-FirstLogonCommand</a> &lt;String&gt;...]
    [<a href="#-firstlogonscript">-FirstLogonScript</a> &lt;String&gt;...]
    [<a href="#-help">-Help</a>]
    [<a href="#-imageindex">-ImageIndex</a> &lt;Number&gt;]
    [<a href="#-install">-Install</a> &lt;InstallMethod&gt;]
    [<a href="#-installtodisk">-InstallToDisk</a> &lt;Number&gt;]
    [<a href="#-installtopartition">-InstallToPartition</a> &lt;Number&gt;]
    [<a href="#-joindomain">-JoinDomain</a> &lt;String&gt;]
    [<a href="#-joindomainoffline">-JoinDomainOffline</a>]
    [<a href="#-joindomainpassword">-JoinDomainPassword</a> &lt;String&gt;]
    [<a href="#-joindomainprovisioningfile">-JoinDomainProvisioningFile</a> &lt;Path&gt;]
    [<a href="#-joindomainuser">-JoinDomainUser</a> &lt;[Domain\]User&gt;]
    [<a href="#-language">-Language</a> &lt;String&gt;]
    [<a href="#-localaccount">-LocalAccount</a> &lt;[Group:]Name,Password&gt;...]
    [<a href="#-onlinehelp">-OnlineHelp</a>]
    [<a href="#-oupath">-OUPath</a> &lt;String&gt;]
    [<a href="#-partition">-Partition</a> &lt;Label:Size&gt;...]
    [<a href="#-processorarchitecture">-ProcessorArchitecture</a> &lt;String&gt;]
    [<a href="#-productkey">-ProductKey</a> &lt;String&gt;]
    [<a href="#-timezone">-TimeZone</a> &lt;String&gt;]
    [<a href="#-version">-Version</a>]
    [<a href="#-windowsversion">-WindowsVersion</a> &lt;Version&gt;]</pre>
<!-- markdownlint-enable MD033 -->

## General options

### `-OutputFile`

The path and file name to write the answer file to. If not specified, the generated answer file is
written to the console.

```yaml
Value: <Path>
Alias: -o
Positional: True
```

### `-Help`

Displays a help message.

```yaml
Value: [<Boolean>]
Aliases: -?, -h
```

### `-OnlineHelp`

Shows additional help in your web browser, including example usage.

```yaml
Value: [<Boolean>]
Aliases: -oh, -??
```

### `-Version`

Displays version information.

```yaml
Value: [<Boolean>]
```

## Installation options

### `-Feature`

The name of an optional feature to install. Use the PowerShell `Get-WindowsOptionalFeature` command
to get a list of valid feature names. Can have multiple values.

Must not be blank.

See [optional features](../README.md#optional-features).

```yaml
Value: <String> (multiple allowed)
Alias: -c
Required argument: -WindowsVersion
Allowed -Install values: ExistingPartition, CleanBios, CleanEfi, Manual
```

### `-ImageIndex`

The index of the image in the WIM file to install. Use this for Windows editions not installed using
a product key, such as those that use volume licensing. Use the PowerShell `Get-WindowsImage`
command to list all images in a .wim or .esd file.

See [selecting the edition to install](../README.md#selecting-the-edition-to-install).

```yaml
Value: <Number>
Alias: -wim
Allowed -Install values: ExistingPartition, CleanEfi, CleanBios
```

### `-Install`

The installation method to use.

See [installation method](../README.md#installation-method).

```yaml
Value: PreInstalled, ExistingPartition, CleanEfi, CleanBios, Manual
Alias: -i
Default value: PreInstalled
```

### `-InstallToDisk`

The zero-based ID of the disk to install to. This disk will be wiped and repartitioned according to
[`-Partition`](#-partition), or using the default layout if [`-Partition`](#-partition) is not
specified.

Must be at least 0.

```yaml
Value: <Number>
Alias: -disk
Default value: 0
Allowed -Install values: ExistingPartition, CleanEfi, CleanBios
```

### `-InstallToPartition`

The one-based ID of the partition to install to, on the disk specified by
[`-InstallToDisk`](#-installtodisk). If not specified and [`-Install`](#-install) is `CleanEfi` or
`CleanBios`, Windows will be installed on the first regular data partition. If
[`-Install`](#-install) is `ExistingPartition`, the default value is 3, which is appropriate for
UEFI systems with the default partition layout.

Must be at least 1.

```yaml
Value: <Number>
Alias: -part
Allowed -Install values: CleanEfi, CleanBios, ExistingPartition
```

### `-Partition`

A partition to create on the disk specified by [`-InstallToDisk`](#-installtodisk). Can have
multiple values.

Use the format `label:size` or `label:size[fs]`, where label is the volume label, size is the size
of the partition, and fs is an optional file system like FAT32 or NTFS. Sizes can use multiple-byte
units such as GB, and will be truncated to whole megabytes. For example `System:100MB`,
`Windows:128GB`, or `Data:16GB[FAT32]`.

Use `*` as the size to extend the partition to fill the remainder of the disk (e.g. `Windows:*`).

Use the following labels to create special partitions: `System`, `MSR`, `WinRE`, and `Recovery`.

If not specified, the default partition layout for the method specified by [`-Install`](#-install)
is used.

See [custom partition layout](../README.md#custom-partition-layout).

```yaml
Value: <Label:Size> (multiple allowed)
Alias: -p
Allowed -Install values: CleanEfi, CleanBios
```

### `-ProductKey`

The product key used to select what edition to install, and to activate Windows.

Must not be blank.

See [selecting the edition to install](../README.md#selecting-the-edition-to-install).

```yaml
Value: <String>
Alias: -key
```

### `-WindowsVersion`

The exact version and build number (e.g. `10.0.22621.1`) of the OS being installed. This argument is
only used when [`-Feature`](#-feature) is specified.

```yaml
Value: <Version>
Alias: -v
```

## User account options

### `-AutoLogonCount`

The number of times the user specified by [`-AutoLogonUser`](#-autologonuser) will be automatically
logged on.

Must be at least 1.

```yaml
Value: <Number>
Alias: -alc
Default value: 1
Required argument: -AutoLogonUser
```

### `-AutoLogonPassword`

The password of the user specified by [`-AutoLogonUser`](#-autologonuser).

Must not be blank.

```yaml
Value: <String>
Alias: -alp
Required argument: -AutoLogonUser
```

### `-AutoLogonUser`

The name of a user to automatically log on, in the format `domain\user`, or just `user` for local
users. If not specified, automatic log-on will not be used.

Must not be blank.

See [joining a domain and automatic logon](../README.md#joining-a-domain-and-automatic-log-on).

```yaml
Value: <[Domain\]User>
Alias: -alu
Required argument: -AutoLogonPassword
```

### `-LocalAccount`

A local account to create, using the format `group:name,password` or `name,password`. Can have
multiple values. If no group is specified, the user will be added to the Administrators group. You
can specify multiple groups by separating them with semicolons.

If no local accounts are created, the user will be asked to create one during OOBE, making setup not
fully unattended.

See [creating a user during installation](../README.md#creating-a-user-during-installation).

```yaml
Value: <[Group:]Name,Password> (multiple allowed)
Alias: -a
```

## Domain options

### `-DomainAccount`

The name of a domain account to add to a local group, using the format `group:domain\user`,
`domain\user`, `group:user` or `user`. If no group is specified, the user is added to the local
Administrators group. You can specify multiple groups by separating them with semicolons. If no
domain is specified, the user must be in the domain you're joining. Can have multiple values.

Must not be blank.

```yaml
Value: <[Group:][Domain\]User> (multiple allowed)
Alias: -da
Requires one of: -JoinDomain, -JoinDomainProvisioningFile
```

### `-JoinDomain`

The name of a domain to join. If not specified, the system will not be joined to a domain.

Must not be blank.

See [joining a domain and automatic logon](../README.md#joining-a-domain-and-automatic-log-on).

```yaml
Value: <String>
Alias: -jd
Required arguments: -JoinDomainUser, -JoinDomainPassword
```

### `-JoinDomainOffline`

Join the domain during the offlineServicing pass of Windows setup, rather than the specialize pass.

```yaml
Value: [<Boolean>]
Alias: -jdo
Required argument: -JoinDomainProvisioningFile
Allowed -Install values: ExistingPartition, CleanBios, CleanEfi, Manual
```

### `-JoinDomainPassword`

The password of the user specified by [`-JoinDomainUser`](#-joindomainuser). This will be stored in
plain text in the answer file.

Must not be blank.

```yaml
Value: <String>
Alias: -jdp
Required argument: -JoinDomain
```

### `-JoinDomainProvisioningFile`

The path to a file containing provisioned account data to join the domain. This file can be created
using the command `djoin.exe /provision /domain domainname /machine machinename /savefile filename`.

See [joining a domain using provisioning](../README.md#joining-a-domain-using-provisioning).

```yaml
Value: <Path>
Alias: -jdpf
Prohibited argument: -JoinDomain
```

### `-JoinDomainUser`

The name of a user with permission to join the domain specified by [`-JoinDomain`](#-joindomain).
Use the format `domain\user`, or just `user` if the user is a member of the domain you are joining.

Must not be blank.

```yaml
Value: <[Domain\]User>
Alias: -jdu
Required argument: -JoinDomain
```

### `-OUPath`

The organizational unit to use when joining the domain specified by [`-JoinDomain`](#-joindomain).

Must not be blank.

```yaml
Value: <String>
Alias: -ou
Required argument: -JoinDomain
```

## Other setup options

### `-ComputerName`

The network name for the computer. If not specified, Windows will generate a default name.

Must not be blank.

```yaml
Value: <String>
Alias: -n
```

### `-DisableCloud`

Disable Windows cloud consumer features. This prevents auto-installation of recommended store apps.

```yaml
Value: [<Boolean>]
Alias: -dc
```

### `-DisableDefender`

Disable Windows Defender virus and threat protection.

```yaml
Value: [<Boolean>]
Alias: -d
```

### `-DisableServerManager`

Do not automatically start Server Manager when logging on (Windows Server only).

```yaml
Value: [<Boolean>]
Alias: -dsm
```

### `-DisplayResolution`

The display resolution, in the format `width,height`. For example, `1920,1080`. If not specified,
the default resolution is determined by Windows.

```yaml
Value: <Resolution>
Alias: -res
```

### `-EnableRemoteDesktop`

Turn on remote desktop, and create a Windows Defender Firewall rule to allow incoming connections.

```yaml
Value: [<Boolean>]
Alias: -rdp
```

### `-FirstLogonCommand`

A command to run during first logon. Can have multiple values. All commands are executed before the
scripts specified by [`-FirstLogonScript`](#-firstlogonscript), in the order specified.

Must not be blank.

See [first log-on commands and scripts](../README.md#first-log-on-commands-and-scripts).

```yaml
Value: <String> (multiple allowed)
Alias: -cmd
```

### `-FirstLogonScript`

The full path of a Windows PowerShell script to run during first log-on, plus arguments. Can have
multiple values. Scripts are executed after the commands specified by
[`-FirstLogonCommand`](#-firstlogoncommand), in the order specified.

Must not be blank.

See [first log-on commands and scripts](../README.md#first-log-on-commands-and-scripts).

```yaml
Value: <String> (multiple allowed)
Aliases: -SetupScript, -s
```

### `-Language`

The language used for the UI language, and the input, system and user locales.

Must not be blank.

```yaml
Value: <String>
Alias: -lang
Default value: en-US
```

### `-ProcessorArchitecture`

The processor architecture of the Windows edition you're installing. Use `amd64` for 64 bit Intel
and AMD processors, `x86` for 32 bit, and `arm64` for ARM-based devices.

Must not be blank.

See [installing a 32 bit OS](../README.md#installing-a-32-bit-os).

```yaml
Value: <String>
Alias: -arch
Default value: amd64
```

### `-TimeZone`

The time zone that Windows will use. Run `tzutil /l` for a list of valid values.

Must not be blank.

```yaml
Value: <String>
Default value: Pacific Standard Time
```
