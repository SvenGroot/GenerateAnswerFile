# Command line arguments

This page describes the command line arguments supported by Answer File Generator, which are used
to customize the generated answer file. All argument names are case insensitive.

Argument values can be specified as either `-JoinDomain mydomain`, `-JoinDomain:mydomain` or
`-JoinDomain=mydomain`.

Some arguments can be specified multiple times. This can be done by listing several values after
the argument:

```text
-LocalAccount "John,Password" "Dave,OtherPassword" -EnableRemoteDesktop
```

Or, by repeating the argument multiple times:

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
    "EnableRemoteDesktop" = $true
}

./GenerateAnswerFile @arguments
```

The arguments are split into several categories:

- [General options](#general-options)
- [Installation options](#installation-options)
- [User account options](#user-account-options)
- [Domain options](#domain-options)
- [Other setup options](#other-setup-options)

## Usage syntax

<!-- markdownlint-disable MD033 -->
<pre>GenerateAnswerFile
    [<a href="#-outputfile">-OutputFile</a>] &lt;Path&gt;
    [<a href="#-autologoncount">-AutoLogonCount</a> &lt;Number&gt;]
    [<a href="#-autologonpassword">-AutoLogonPassword</a> &lt;String&gt;]
    [<a href="#-autologonuser">-AutoLogonUser</a> &lt;[Domain\]User&gt;]
    [<a href="#-cmdkeypassword">-CmdKeyPassword</a> &lt;String&gt;]
    [<a href="#-cmdkeyuser">-CmdKeyUser</a> &lt;Domain\User&gt;]
    [<a href="#-computername">-ComputerName</a> &lt;String&gt;]
    [<a href="#-disablecloud">-DisableCloud</a>]
    [<a href="#-disabledefender">-DisableDefender</a>]
    [<a href="#-disableservermanager">-DisableServerManager</a>]
    [<a href="#-displayresolution">-DisplayResolution</a> &lt;Size&gt;]
    [<a href="#-domainaccount">-DomainAccount</a> &lt;String&gt;...]
    [<a href="#-enableremotedesktop">-EnableRemoteDesktop</a>]
    [<a href="#-feature">-Feature</a> &lt;String&gt;...]
    [<a href="#-firstlogoncommand">-FirstLogonCommand</a> &lt;String&gt;...]
    [<a href="#-help">-Help</a>]
    [<a href="#-imageindex">-ImageIndex</a> &lt;Number&gt;]
    [<a href="#-install">-Install</a> &lt;InstallMethod&gt;]
    [<a href="#-installtodisk">-InstallToDisk</a> &lt;Number&gt;]
    [<a href="#-installtopartition">-InstallToPartition</a> &lt;Number&gt;]
    [<a href="#-joindomain">-JoinDomain</a> &lt;String&gt;]
    [<a href="#-joindomainpassword">-JoinDomainPassword</a> &lt;String&gt;]
    [<a href="#-joindomainuser">-JoinDomainUser</a> &lt;[Domain\]User&gt;]
    [<a href="#-language">-Language</a> &lt;String&gt;]
    [<a href="#-localaccount">-LocalAccount</a> &lt;Name,Password&gt;...]
    [<a href="#-onlinehelp">-OnlineHelp</a>]
    [<a href="#-oupath">-OUPath</a> &lt;String&gt;]
    [<a href="#-partition">-Partition</a> &lt;Label:Size&gt;...]
    [<a href="#-processorarchitecture">-ProcessorArchitecture</a> &lt;String&gt;]
    [<a href="#-productkey">-ProductKey</a> &lt;String&gt;]
    [<a href="#-setupscript">-SetupScript</a> &lt;String&gt;...]
    [<a href="#-timezone">-TimeZone</a> &lt;String&gt;]
    [<a href="#-version">-Version</a>]
    [<a href="#-windowsversion">-WindowsVersion</a> &lt;Version&gt;]</pre>
<!-- markdownlint-enable MD033 -->

## General options

### `-OutputFile`

The path and file name to write the answer file to.

```yaml
Value: <Path>
Aliases: -o
Required: True
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
Aliases: -c
Required arguments: -WindowsVersion
Allowed -Install values: ExistingPartition, CleanBios, CleanEfi, Manual
```

### `-ImageIndex`

The index of the image in the WIM file to install. Use this for Windows editions not installed using
a product key, such as those that use volume licensing. Use the PowerShell `Get-WindowsImage`
command to list all images in a .wim or .esd file.

See [selecting the edition to install](../README.md#selecting-the-edition-to-install).

```yaml
Value: <Number>
Aliases: -wim
Allowed -Install values: ExistingPartition, CleanEfi, CleanBios
```

### `-Install`

The installation method to use.

See [installation method](../README.md#installation-method).

```yaml
Value: PreInstalled, ExistingPartition, CleanEfi, CleanBios, Manual
Aliases: -i
Default value: PreInstalled
```

### `-InstallToDisk`

The zero-based ID of the disk to install to. This disk will be wiped and repartitioned according to
[`-Partition`](#-partition), or using the default layout if [`-Partition`](#-partition) is not
specified.

Must be at least 0.

```yaml
Value: <Number>
Aliases: -disk
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
Aliases: -part
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
Aliases: -p
Allowed -Install values: CleanEfi, CleanBios
```

### `-ProductKey`

The product key used to select what edition to install, and to activate Windows.

Must not be blank.

See [selecting the edition to install](../README.md#selecting-the-edition-to-install).

```yaml
Value: <String>
Aliases: -key
```

### `-WindowsVersion`

The exact version and build number (e.g. `10.0.22621.1`) of the OS being installed. This argument is
only used when [`-Feature`](#-feature) is specified.

```yaml
Value: <Version>
Aliases: -v
```

## User account options

### `-CmdKeyPassword`

The password of the user specified by [`-CmdKeyUser`](#-cmdkeyuser).

Must not be blank.

```yaml
Value: <String>
Aliases: -ckp
Required arguments: -CmdKeyUser
```

### `-CmdKeyUser`

The name of a user used to access all network resources, in `domain\user` format. If present, the
cmdkey.exe application will be used at first logon to save this user's credentials for all network
paths

Must not be blank.

```yaml
Value: <Domain\User>
Aliases: -cku
Required arguments: -CmdKeyPassword
```

### `-LocalAccount`

A local account to create, using the format `name,password`. Can have multiple values. If no local
accounts are created, the user will be asked to create one during OOBE, making setup not fully
unattended.

See [creating a user during installation](../README.md#creating-a-user-during-installation).

```yaml
Value: <Name,Password> (multiple allowed)
Aliases: -a
```

## Automatic logon options

### `-AutoLogonCount`

The number of times the user specified by [`-AutoLogonCount`](#-autologoncount) will be
automatically logged on.

Must be at least 1.

```yaml
Value: <Number>
Aliases: -alc
Default value: 1
Required arguments: -AutoLogonUser
```

### `-AutoLogonPassword`

The password of the user specified by [`-AutoLogonCount`](#-autologoncount).

Must not be blank.

```yaml
Value: <String>
Aliases: -alp
Required arguments: -AutoLogonUser
```

### `-AutoLogonUser`

The name of a user to automatically log on, in the format `domain\user`, or just `user` for local
users. If not specified, automatic logon will not be used.

Must not be blank.

See [joining a domain and automatic logon](../README.md#joining-a-domain-and-automatic-log-on).

```yaml
Value: <[Domain\]User>
Aliases: -alu
Required arguments: -AutoLogonPassword
```

## Domain options

### `-DomainAccount`

The name of a domain account to add to the local administrators group. Must be in the domain you're
joining. Can have multiple values.

Must not be blank.

```yaml
Value: <String> (multiple allowed)
Aliases: -da
Required arguments: -JoinDomain
```

### `-JoinDomain`

The name of a domain to join. If not specified, the system will not be joined to a domain.

Must not be blank.

See [joining a domain and automatic logon](../README.md#joining-a-domain-and-automatic-log-on).

```yaml
Value: <String>
Aliases: -jd
Required arguments: -JoinDomainUser, -JoinDomainPassword
```

### `-JoinDomainPassword`

The password of the user specified by [`-JoinDomainUser`](#-joindomainuser). Will be stored in plain
text.

Must not be blank.

```yaml
Value: <String>
Aliases: -jdp
Required arguments: -JoinDomain
```

### `-JoinDomainUser`

The name of a user with permission to join the domain specified by [`-JoinDomain`](#-joindomain).
Use the format `domain\user`, or just `user` if the user is a member of the domain you are joining.

Must not be blank.

```yaml
Value: <[Domain\]User>
Aliases: -jdu
Required arguments: -JoinDomain
```

### `-OUPath`

The organizational unit to use when joining the domain specified by [`-JoinDomain`](#-joindomain).

Must not be blank.

```yaml
Value: <String>
Aliases: -ou
Required arguments: -JoinDomain
```

## Other setup options

### `-ComputerName`

The network name for the computer.

Must not be blank.

```yaml
Value: <String>
Aliases: -n
```

### `-DisableCloud`

Disable Windows cloud consumer features. This prevents auto-installation of recommended store apps.

```yaml
Value: [<Boolean>]
Aliases: -dc
```

### `-DisableDefender`

Disable Windows Defender virus and threat protection.

```yaml
Value: [<Boolean>]
Aliases: -d
```

### `-DisableServerManager`

Do not automatically start Server Manager when logging on (Windows Server only).

```yaml
Value: [<Boolean>]
Aliases: -dsm
```

### `-DisplayResolution`

The display resolution, in the format `width,height`. For example, `1920,1080`. If not specified,
the default resolution is determined by Windows.

```yaml
Value: <Size>
Aliases: -res
```

### `-EnableRemoteDesktop`

Turn on remote desktop, and create a Windows Defender Firewall rule to allow incoming connections.

```yaml
Value: [<Boolean>]
Aliases: -rdp
```

### `-FirstLogonCommand`

A command to run during first logon. Can have multiple values. Commands are executed before the
scripts specified by [`-SetupScript`](#-setupscript).

Must not be blank.

See [first log-on commands and scripts](../README.md#first-log-on-commands-and-scripts).

```yaml
Value: <String> (multiple allowed)
Aliases: -cmd
```

### `-Language`

The language used for the UI language, and the input, system and user locales.

Must not be blank.

```yaml
Value: <String>
Aliases: -lang
Default value: en-US
```

### `-ProcessorArchitecture`

The processor architecture of the Windows edition you're installing. Use `amd64` for 64 bit Intel
and AMD processors, `x86` for 32 bit, and `arm64` for ARM-based devices.

Must not be blank.

See [installing a 32 bit OS](../README.md#installing-a-32-bit-os).

```yaml
Value: <String>
Aliases: -arch
Default value: amd64
```

### `-SetupScript`

The full path of a Windows PowerShell script to run during first logon. Can have multiple values.
Scripts are executed after the commands specified by [`-FirstLogonCommand`](#-firstlogoncommand).

Must not be blank.

See [first log-on commands and scripts](../README.md#first-log-on-commands-and-scripts).

```yaml
Value: <String> (multiple allowed)
Aliases: -s
```

### `-TimeZone`

The time zone that Windows will use. Run `tzutil /l` for a list of valid values.

Must not be blank.

```yaml
Value: <String>
Default value: Pacific Standard Time
```
