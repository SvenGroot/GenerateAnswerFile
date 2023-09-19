using Ookii.AnswerFile;
using Ookii.CommandLine;
using Ookii.CommandLine.Conversion;
using Ookii.CommandLine.Validation;
using System.ComponentModel;
using System.Drawing;

namespace GenerateAnswerFile;

[GeneratedParser]
[ResourceDescription(nameof(Properties.Resources.ApplicationDescription))]
partial class Arguments
{
    [CommandLineArgument(IsPositional = true)]
    [ResourceDescription(nameof(Properties.Resources.OutputFileDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.PathValueDescription))]
    [Alias("o")]
    public required FileInfo OutputFile { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.WindowsVersionDescription))]
    [Alias("v")]
    public Version? WindowsVersion { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainDescription))]
    [Requires(nameof(JoinDomainUser), nameof(JoinDomainPassword))]
    [Alias("jd")]
    [ValidateNotWhiteSpace]
    public string? JoinDomain { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainUserDescription))]
    [Alias("jdu")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? JoinDomainUser { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainPasswordDescription))]
    [Alias("jdp")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? JoinDomainPassword { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.OUPathDescription))]
    [Alias("ou")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? OUPath { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ComputerNameDescription))]
    [Alias("n")]
    [ValidateNotWhiteSpace]
    public string? ComputerName { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisableDefenderDesciption))]
    [Alias("d")]
    public bool DisableDefender { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisableCloudDescription))]
    [Alias("dc")]
    public bool DisableCloud { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisableServerManagerDescription))]
    [Alias("dsm")]
    public bool DisableServerManager { get; set; }

    [CommandLineArgument("DomainAccount")]
    [ResourceDescription(nameof(Properties.Resources.DomainAccountsDescription))]
    [Alias("da")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? DomainAccounts { get; set; }

    [CommandLineArgument("LocalAccount")]
    [ResourceDescription(nameof(Properties.Resources.LocalAccountsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.LocalCredentialValueDescription))]
    [Alias("a")]
    [MultiValueSeparator]
    public LocalCredential[]? LocalAccounts { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonUserDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.OptionalDomainUserValueDescription))]
    [Alias("alu")]
    [Requires(nameof(AutoLogonPassword))]
    [ValidateNotWhiteSpace]
    public DomainUser? AutoLogonUser { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonPasswordDescription))]
    [Alias("alp")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateNotWhiteSpace]
    public string? AutoLogonPassword { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonCountDescription))]
    [Alias("alc")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateRange(1, null)]
    public int AutoLogonCount { get; set; } = 1;

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.CmdKeyUserDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.DomainUserValueDescription))]
    [Alias("cku")]
    [Requires(nameof(CmdKeyPassword))]
    [ValidateNotWhiteSpace]
    public DomainUser? CmdKeyUser { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.CmdKeyPasswordDescription))]
    [Alias("ckp")]
    [Requires(nameof(CmdKeyUser))]
    [ValidateNotWhiteSpace]
    public string? CmdKeyPassword { get; set; }

    [CommandLineArgument("SetupScript")]
    [ResourceDescription(nameof(Properties.Resources.SetupScriptsDescription))]
    [Alias("s")]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? SetupScripts { get; set; }

    [CommandLineArgument("Feature")]
    [ResourceDescription(nameof(Properties.Resources.FeaturesDescription))]
    [Alias("c")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanBios, InstallMethod.CleanEfi, InstallMethod.Manual)]
    [Requires(nameof(WindowsVersion))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? Features { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.InstallDescription))]
    [Alias("i")]
    [ValidateEnumValue]
    public InstallMethod Install { get; set; } = InstallMethod.PreInstalled;

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.InstallToDiskDescription))]
    [Alias("disk")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    [ValidateRange(0, null)]
    public int InstallToDisk { get; set; } = 0;

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.InstallToPartitionDescription))]
    [Alias("part")]
    [ValidateInstallMethod(InstallMethod.CleanEfi, InstallMethod.CleanBios, InstallMethod.ExistingPartition)]
    [ValidateRange(1, null)]
    public int? InstallToPartition { get; set; }

    [CommandLineArgument("Partition")]
    [ResourceDescription(nameof(Properties.Resources.PartitionsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.PartitionsValueDescription))]
    [Alias("p")]
    [ValidateInstallMethod(InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    [MultiValueSeparator]
    public Partition[]? Partitions { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ImageIndexDescription))]
    [Alias("wim")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    public int ImageIndex { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.EnableRemoteDesktopDescriptoin))]
    [Alias("rdp")]
    public bool EnableRemoteDesktop { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisplayResolutionDescription))]
    [Alias("res")]
    [ArgumentConverter(typeof(WrappedDefaultTypeConverter<Size>))]
    public Size? DisplayResolution { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.LanguageDescription))]
    [Alias("lang")]
    [ValidateNotWhiteSpace]
    public string Language { get; set; } = "en-US";

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ProductKeyDescription))]
    [Alias("key")]
    [ValidateNotWhiteSpace]
    public string? ProductKey { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ProcessorArchitectureDescription))]
    [Alias("arch")]
    [ValidateNotWhiteSpace]
    public string ProcessorArchitecture { get; set; } = "amd64";

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.TimeZoneDescription))]
    [ValidateNotWhiteSpace]
    public string TimeZone { get; set; } = "Pacific Standard Time";

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
            EnableServerManager = !DisableServerManager,
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
                PartitionId = InstallToPartition ?? 3,
                ImageIndex = ImageIndex,
            },
            InstallMethod.CleanEfi => new CleanEfiOptions()
            {
                DiskId = InstallToDisk,
                ImageIndex = ImageIndex,
                CustomTargetPartitionId = InstallToPartition,
            },
            InstallMethod.CleanBios => new CleanBiosOptions()
            {
                DiskId = InstallToDisk,
                ImageIndex = ImageIndex,
                CustomTargetPartitionId = InstallToPartition,
            },
            InstallMethod.Manual => new ManualInstallOptions(),
            _ => null,
        };

        if (options != null && Features?.Length > 0)
        {
            options.OptionalFeatures = new OptionalFeatures(WindowsVersion!);
            options.OptionalFeatures.Features.AddRange(Features);
        }

        if (options is CleanOptionsBase cleanOptions && Partitions?.Length > 0)
        {
            cleanOptions.Partitions.AddRange(Partitions);
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
