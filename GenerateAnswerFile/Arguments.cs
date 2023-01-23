﻿using Ookii.CommandLine;
using Ookii.CommandLine.Validation;
using System.ComponentModel;
using System.Drawing;

namespace GenerateAnswerFile;

[ApplicationFriendlyName("Answer File Generator")]
public class Arguments
{
    [CommandLineArgument(IsRequired = true, Position = 0, ValueDescription = "Path")]
    [Description("The path and file name to write the answer file to.")]
    [Alias("o")]
    public FileInfo OutputFile { get; set; } = default!;

    [CommandLineArgument]
    [Description("Version and build number (e.g. 10.0.22000.1) of the OS being installed. This argument is only used when -Component is specified.")]
    [Alias("v")]
    public Version? WindowsVersion { get; set; }

    [CommandLineArgument]
    [Description("Name of the domain to join.")]
    [Requires(nameof(JoinDomainUser), nameof(JoinDomainPassword))]
    [Alias("jd")]
    public string? JoinDomain { get; set; }

    [CommandLineArgument]
    [Description("Name of a user with permission to join the domain. Must be a member of the domain specified with -JoinDomain.")]
    [Alias("jdu")]
    [Requires(nameof(JoinDomain))]
    public string? JoinDomainUser { get; set; }

    [CommandLineArgument]
    [Description("Password of the user used to join the domain. Will be stored in plain text.")]
    [Alias("jdp")]
    [Requires(nameof(JoinDomain))]
    public string? JoinDomainPassword { get; set; }

    [CommandLineArgument]
    [Description("The organizational unit to use when joining the domain.")]
    [Alias("ou")]
    [Requires(nameof(JoinDomain))]
    public string? OUPath { get; set; }

    [CommandLineArgument]
    [Description("The network name for the computer.")]
    [Alias("n")]
    public string? ComputerName { get; set; }

    [CommandLineArgument]
    [Description("Disable Windows Defender after installation.")]
    [Alias("d")]
    public bool DisableDefender { get; set; }

    [CommandLineArgument]
    [Description("Disable Windows cloud consumer features. This prevents auto-installation of recommended store apps.")]
    [Alias("dc")]
    public bool DisableCloud { get; set; }

    [CommandLineArgument("DomainAccount")]
    [Description("The name of a domain account to add to the local administrators group. Must be in the domain you're joining. Can be specified more than once.")]
    [Alias("da")]
    public string[]? DomainAccounts { get; set; }

    [CommandLineArgument("LocalAccount", ValueDescription = "Name,Password")]
    [Description("A local account to add, using the format 'name,password'. Can be specified more than once.")]
    [Alias("a")]
    public UserAndPassword[]? LocalAccounts { get; set; }

    [CommandLineArgument]
    [Description("The name of the user (in the format 'domain\\user', or just 'user' for local users) to automatically log on.")]
    [Alias("alu")]
    [Requires(nameof(AutoLogonPassword))]
    public DomainUser? AutoLogonUser { get; set; }

    [CommandLineArgument]
    [Description("The password of the user to automatically log on.")]
    [Alias("alp")]
    [Requires(nameof(AutoLogonUser))]
    public string? AutoLogonPassword { get; set; }

    [CommandLineArgument(DefaultValue = 1)]
    [Description("The number of times the user will be automatically logged in.")]
    [Alias("alc")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateRange(1, null)]
    public int AutoLogonCount { get; set; }

    [CommandLineArgument]
    [Description("The default user used to access the network, in 'domain\\user' format.")]
    [Alias("cku")]
    [Requires(nameof(CmdKeyPassword))]
    public DomainUser? CmdKeyUser { get; set; }

    [CommandLineArgument]
    [Description("The password of the user used to access the network.")]
    [Alias("ckp")]
    [Requires(nameof(CmdKeyUser))]
    public string? CmdKeyPassword { get; set; }

    [CommandLineArgument("SetupScript")]
    [Alias("s")]
    [Description("The full path of a Windows PowerShell script to run during first logon. Can be specified more than once.")]
    public string[]? SetupScripts { get; set; }

    [CommandLineArgument("Component")]
    [Description("The feature name of an optional component to install. Can be specified more than once.")]
    [Alias("c")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanBios, InstallMethod.CleanEfi, InstallMethod.Manual)]
    [Requires(nameof(WindowsVersion))]
    public string[]? Components { get; set; }

    [CommandLineArgument(DefaultValue = InstallMethod.PreInstalled)]
    [Description("The install method used.")]
    [Alias("i")]
    [ValidateEnumValue]
    public InstallMethod Install { get; set; }

    [CommandLineArgument(DefaultValue = 0)]
    [Description("The zero-based ID of the disk to install to.")]
    [Alias("disk")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    [ValidateRange(0, null)]
    public int InstallToDisk { get; set; }

    [CommandLineArgument]
    [Description("The partition to install to. The default value is 3, which is appropriate for UEFI systems with the default partition layout.")]
    [Alias("part")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition)]
    [ValidateRange(1, null)]
    public int InstallToPartition { get; set; } = 3;

    [CommandLineArgument]
    [Description("The WIM image index to install.")]
    [Alias("wim")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    public int ImageIndex { get; set; }

    [CommandLineArgument]
    [Description("Turn on remote desktop and allow it through the firewall.")]
    [Alias("rdp")]
    public bool EnableRemoteDesktop { get; set; }

    [CommandLineArgument]
    [Description("The display resolution, in the format 'width,height'. For example, '1280,1024'. If not specified, the default resolution is determined by Windows.")]
    [Alias("res")]
    public Size? DisplayResolution { get; set; }

    [CommandLineArgument(DefaultValue = "en-US")]
    [Description("The language used for the UI language and the input, system and user locales.")]
    [Alias("lang")]
    [ValidateNotWhiteSpace]
    public string? Language { get; set; }

    [CommandLineArgument]
    [Description("The product key used to select what edition to install and to activate Windows.")]
    [Alias("key")]
    [ValidateNotWhiteSpace]
    public string? ProductKey { get; set; }

    [CommandLineArgument(DefaultValue = "amd64")]
    [Description("The processor architecture of the Windows edition you're installing. Use amd64 for 64 bit, and x86 for 32 bit.")]
    [Alias("arch")]
    [ValidateNotWhiteSpace]
    public string? ProcessorArchitecture { get; set; }

    [CommandLineArgument(DefaultValue = "Pacific Standard Time")]
    [Description("The time zone that Windows will use.")]
    [ValidateNotWhiteSpace]
    public string TimeZone { get; set; } = default!;

    public static Arguments? Parse()
    {
        var options = new ParseOptions
        {
            ShowUsageOnError = UsageHelpRequest.SyntaxOnly,
            UsageWriter = new UsageWriter()
            {
                UseAbbreviatedSyntax = true,
            }
        };

        return CommandLineParser.Parse<Arguments>(options);
    }
}