using Ookii.AnswerFile;
using Ookii.CommandLine;
using Ookii.CommandLine.Validation;
using System.ComponentModel;
using System.Drawing;

namespace GenerateAnswerFile;

[ApplicationFriendlyName("Windows Answer File Generator")]
[Description("Generates answer files (unattend.xml and autounattend.xml) for unattended Windows installation.")]
class Arguments
{
    [CommandLineArgument(IsRequired = true, Position = 0, ValueDescription = "Path")]
    [Description("The path and file name to write the answer file to.")]
    [Alias("o")]
    public FileInfo OutputFile { get; set; } = default!;

    [CommandLineArgument]
    [Description("Version and build number (e.g. 10.0.22000.1) of the OS being installed. This argument is only used when -Feature is specified.")]
    [Alias("v")]
    public Version? WindowsVersion { get; set; }

    [CommandLineArgument]
    [Description("Name of the domain to join.")]
    [Requires(nameof(JoinDomainUser), nameof(JoinDomainPassword))]
    [Alias("jd")]
    [ValidateNotWhiteSpace]
    public string? JoinDomain { get; set; }

    [CommandLineArgument]
    [Description("Name of a user with permission to join the domain. Must be a member of the domain specified with -JoinDomain.")]
    [Alias("jdu")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? JoinDomainUser { get; set; }

    [CommandLineArgument]
    [Description("Password of the user used to join the domain. Will be stored in plain text.")]
    [Alias("jdp")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? JoinDomainPassword { get; set; }

    [CommandLineArgument]
    [Description("The organizational unit to use when joining the domain.")]
    [Alias("ou")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? OUPath { get; set; }

    [CommandLineArgument]
    [Description("The network name for the computer.")]
    [Alias("n")]
    [ValidateNotWhiteSpace]
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
    [Description("The name of a domain account to add to the local administrators group. Must be in the domain you're joining. Can have multiple values.")]
    [Alias("da")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? DomainAccounts { get; set; }

    [CommandLineArgument("LocalAccount", ValueDescription = "Name,Password")]
    [Description("A local account to add, using the format 'name,password'. Can have multiple values.")]
    [Alias("a")]
    [MultiValueSeparator]
    public LocalCredential[]? LocalAccounts { get; set; }

    [CommandLineArgument]
    [Description("The name of the user (in the format 'domain\\user', or just 'user' for local users) to automatically log on.")]
    [Alias("alu")]
    [Requires(nameof(AutoLogonPassword))]
    [ValidateNotWhiteSpace]
    public DomainUser? AutoLogonUser { get; set; }

    [CommandLineArgument]
    [Description("The password of the user to automatically log on.")]
    [Alias("alp")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateNotWhiteSpace]
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
    [ValidateNotWhiteSpace]
    public DomainUser? CmdKeyUser { get; set; }

    [CommandLineArgument]
    [Description("The password of the user used to access the network.")]
    [Alias("ckp")]
    [Requires(nameof(CmdKeyUser))]
    [ValidateNotWhiteSpace]
    public string? CmdKeyPassword { get; set; }

    [CommandLineArgument("SetupScript")]
    [Alias("s")]
    [Description("The full path of a Windows PowerShell script to run during first logon. Can have multiple values.")]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? SetupScripts { get; set; }

    [CommandLineArgument("Feature")]
    [Description("The feature name of an optional feature to install. Use the PowerShell 'Get-WindowsOptionalFeature' command to get a list of valid feature names. Can have multiple values.")]
    [Alias("c")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanBios, InstallMethod.CleanEfi, InstallMethod.Manual)]
    [Requires(nameof(WindowsVersion))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? Features { get; set; }

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
    [Description("The WIM image index to install. Use this for editions not installed using a product key such as volume license editions. Use the PowerShell 'Get-WindowsImage' command to list all images in a .wim or .esd file.")]
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
    public string Language { get; set; } = default!;

    [CommandLineArgument]
    [Description("The product key used to select what edition to install and to activate Windows.")]
    [Alias("key")]
    [ValidateNotWhiteSpace]
    public string? ProductKey { get; set; }

    [CommandLineArgument(DefaultValue = "amd64")]
    [Description("The processor architecture of the Windows edition you're installing. Use amd64 for 64 bit, and x86 for 32 bit.")]
    [Alias("arch")]
    [ValidateNotWhiteSpace]
    public string ProcessorArchitecture { get; set; } = default!;

    [CommandLineArgument(DefaultValue = "Pacific Standard Time")]
    [Description("The time zone that Windows will use. Run 'tzutil /l' for a list of valid values.")]
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

    public GeneratorOptions ToOptions()
    {
        var options = new GeneratorOptions()
        {
            InstallOptions = ToInstallOptions(),
            JoinDomain = ToDomainOptions(),
            ComputerName = ComputerName,
            EnableDefender = !DisableDefender,
            EnableCloud = !DisableCloud,
            EnableRemoteDesktop = EnableRemoteDesktop,
            AutoLogon = ToAutoLogonOptions(),
            CmdKeyAccount = ToCmdKeyOptions(),
            DisplayResolution = DisplayResolution,
            Language = Language,
            ProductKey = ProductKey,
            ProcessorArchitecture = ProcessorArchitecture,
            TimeZone = TimeZone,
        };

        if (LocalAccounts != null)
        {
            options.LocalAccounts.AddRange(LocalAccounts);
        }

        if (SetupScripts != null)
        {
            options.SetupScripts.AddRange(SetupScripts);
        }

        return options;
    }

    private InstallOptionsBase? ToInstallOptions()
    {
        InstallOptionsBase? options = Install switch
        {
            InstallMethod.ExistingPartition => new ExistingPartitionOptions()
            {
                DiskId = InstallToDisk,
                PartitionId = InstallToPartition,
                ImageIndex = ImageIndex,
            },
            InstallMethod.CleanEfi => new CleanEfiOptions()
            {
                DiskId = InstallToDisk,
                ImageIndex = ImageIndex,
            },
            InstallMethod.CleanBios => new CleanBiosOptions()
            {
                DiskId = InstallToDisk,
                ImageIndex = ImageIndex,
            },
            InstallMethod.Manual => new ManualInstallOptions(),
            _ => null,
        };

        if (options != null && Features?.Length > 0)
        {
            options.OptionalFeatures = new OptionalFeatures(WindowsVersion!);
            options.OptionalFeatures.Features.AddRange(Features);
        }

        return options;
    }

    private DomainOptions? ToDomainOptions()
    {
        if (JoinDomain == null)
        {
            return null;
        }

        var options = new DomainOptions(JoinDomain, new DomainCredential(new DomainUser(JoinDomain, JoinDomainUser!), JoinDomainPassword!))
        {
            OUPath = OUPath,
        };

        if (DomainAccounts != null)
        {
            options.DomainAccounts.AddRange(DomainAccounts);
        }

        return options;
    }

    private AutoLogonOptions? ToAutoLogonOptions()
    {
        if (AutoLogonUser == null)
        {
            return null;
        }

        return new AutoLogonOptions(AutoLogonUser, AutoLogonPassword!)
        {
            Count = AutoLogonCount,
        };
    }

    private DomainCredential? ToCmdKeyOptions()
    {
        if (CmdKeyUser == null)
        {
            return null;
        }

        return new DomainCredential(CmdKeyUser, CmdKeyPassword!);
    }
}
