using Ookii.AnswerFile;
using Ookii.CommandLine;
using Ookii.CommandLine.Validation;

namespace GenerateAnswerFile;

[GeneratedParser]
partial class Arguments : BaseArguments
{
    #region Installation options

    [CommandLineArgument("Feature", Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.FeaturesDescription))]
    [Alias("c")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanBios, InstallMethod.CleanEfi, InstallMethod.Manual)]
    [Requires(nameof(WindowsVersion))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? Features { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.InstallDescription))]
    [Alias("i")]
    [ValidateEnumValue]
    public InstallMethod Install { get; set; } = InstallMethod.PreInstalled;

    [CommandLineArgument(Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.InstallToDiskDescription))]
    [Alias("disk")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    [ValidateRange(0, null)]
    public int InstallToDisk { get; set; } = 0;

    [CommandLineArgument(Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.InstallToPartitionDescription))]
    [Alias("part")]
    [ValidateInstallMethod(InstallMethod.CleanEfi, InstallMethod.CleanBios, InstallMethod.ExistingPartition)]
    [ValidateRange(1, null)]
    public int? InstallToPartition { get; set; }

    [CommandLineArgument("Partition", Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.PartitionsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.PartitionsValueDescription))]
    [Alias("p")]
    [ValidateInstallMethod(InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    [MultiValueSeparator]
    public Partition[]? Partitions { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.ImageIndexDescription))]
    [Alias("wim")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    public int ImageIndex { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.ProductKeyDescription))]
    [Alias("key")]
    [ValidateNotWhiteSpace]
    public string? ProductKey { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Install)]
    [ResourceDescription(nameof(Properties.Resources.WindowsVersionDescription))]
    [Alias("v")]
    public Version? WindowsVersion { get; set; }

    #endregion

    #region User account options

    [CommandLineArgument("LocalAccount", Category = ArgumentCategory.UserAccounts)]
    [ResourceDescription(nameof(Properties.Resources.LocalAccountsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.LocalCredentialValueDescription))]
    [Alias("a")]
    [MultiValueSeparator]
    public LocalCredential[]? LocalAccounts { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.UserAccounts)]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonUserDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.OptionalDomainUserValueDescription))]
    [Alias("alu")]
    [Requires(nameof(AutoLogonPassword))]
    [ValidateNotWhiteSpace]
    public DomainUser? AutoLogonUser { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.UserAccounts)]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonPasswordDescription))]
    [Alias("alp")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateNotWhiteSpace]
    public string? AutoLogonPassword { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.UserAccounts)]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonCountDescription))]
    [Alias("alc")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateRange(1, null)]
    public int AutoLogonCount { get; set; } = 1;

    #endregion

    #region Domain options

    [CommandLineArgument(Category = ArgumentCategory.Domain)]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainDescription))]
    [Requires(nameof(JoinDomainUser), nameof(JoinDomainPassword))]
    [Alias("jd")]
    [ValidateNotWhiteSpace]
    public string? JoinDomain { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Domain)]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainUserDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.OptionalDomainUserValueDescription))]
    [Alias("jdu")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public DomainUser? JoinDomainUser { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Domain)]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainPasswordDescription))]
    [Alias("jdp")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? JoinDomainPassword { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Domain)]
    [ResourceDescription(nameof(Properties.Resources.OUPathDescription))]
    [Alias("ou")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? OUPath { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Domain)]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainProvisioningFileDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.PathValueDescription))]
    [Prohibits(nameof(JoinDomain))]
    [Alias("jdpf")]
    public FileInfo? JoinDomainProvisioningFile { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Domain)]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainOfflineDescription))]
    [Requires(nameof(JoinDomainProvisioningFile))]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanBios, InstallMethod.CleanEfi, InstallMethod.Manual)]
    [Alias("jdo")]
    public bool JoinDomainOffline { get; set; }

    [CommandLineArgument("DomainAccount", Category = ArgumentCategory.Domain)]
    [ResourceDescription(nameof(Properties.Resources.DomainAccountsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.DomainUserGroupValueDescription))]
    [Alias("da")]
    [RequiresAnyOther(nameof(JoinDomain), nameof(JoinDomainProvisioningFile))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public DomainUserGroup[]? DomainAccounts { get; set; }

    #endregion

    #region Other options

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.ComputerNameDescription))]
    [Alias("n")]
    [ValidateNotWhiteSpace]
    public string? ComputerName { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.DisableDefenderDesciption))]
    [Alias("d")]
    public bool DisableDefender { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.DisableCloudDescription))]
    [Alias("dc")]
    public bool DisableCloud { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.DisableServerManagerDescription))]
    [Alias("dsm")]
    public bool DisableServerManager { get; set; }

    [CommandLineArgument("FirstLogonCommand", Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.FirstLogonCommandsDescription))]
    [Alias("cmd")]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? FirstLogonCommands { get; set; }

    [CommandLineArgument("FirstLogonScript", Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.FirstLogonScriptsDescription))]
    [Alias("SetupScript", IsHidden = true)]
    [Alias("s")]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? FirstLogonScripts { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.EnableRemoteDesktopDescriptoin))]
    [Alias("rdp")]
    public bool EnableRemoteDesktop { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.DisplayResolutionDescription))]
    [Alias("res")]
    public Resolution? DisplayResolution { get; set; }

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.LanguageDescription))]
    [Alias("lang")]
    [ValidateNotWhiteSpace]
    public string Language { get; set; } = "en-US";

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.ProcessorArchitectureDescription))]
    [Alias("arch")]
    [ValidateNotWhiteSpace]
    public string ProcessorArchitecture { get; set; } = "amd64";

    [CommandLineArgument(Category = ArgumentCategory.Other)]
    [ResourceDescription(nameof(Properties.Resources.TimeZoneDescription))]
    [ValidateNotWhiteSpace]
    public string TimeZone { get; set; } = "Pacific Standard Time";

#if DEBUG

    // This argument is used to generate the bulk of CommandLine.md. It has no real value otherwise,
    // so is excluded from the release.
    // N.B. Some modifications are made manually in CommandLine.md, so when updating do so
    // selectively.
    [CommandLineArgument(IsHidden = true)]
    public static CancelMode MarkdownHelp(CommandLineParser parser)
    {
        using var writer = new LineWrappingTextWriter(Console.Out, 100, false);
        var usageWriter = new CustomUsageWriter(writer)
        {
            Markdown = true,
            IncludeApplicationDescription = false,
            UseAbbreviatedSyntax = false,
        };

        usageWriter.WriteParserUsage(parser);
        return CancelMode.Abort;
    }

#endif

    #endregion

    public AnswerFileOptions ToOptions()
    {
        var options = new AnswerFileOptions()
        {
            InstallOptions = ToInstallOptions(),
            JoinDomain = ToDomainOptions(),
            ComputerName = ComputerName,
            EnableDefender = !DisableDefender,
            EnableCloud = !DisableCloud,
            EnableRemoteDesktop = EnableRemoteDesktop,
            EnableServerManager = !DisableServerManager,
            AutoLogon = ToAutoLogonOptions(),
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

        if (FirstLogonCommands != null)
        {
            options.FirstLogonCommands.AddRange(FirstLogonCommands);
        }

        if (FirstLogonScripts != null)
        {
            options.FirstLogonScripts.AddRange(FirstLogonScripts);
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

        if (options != null)
        {
            options.JoinDomainOffline = JoinDomainOffline;
            if (Features?.Length > 0)
            {
                options.OptionalFeatures = new OptionalFeatures(WindowsVersion!);
                options.OptionalFeatures.Features.AddRange(Features);
            }
        }

        if (options is CleanOptionsBase cleanOptions && Partitions?.Length > 0)
        {
            cleanOptions.Partitions.AddRange(Partitions);
        }

        return options;
    }

    private DomainOptionsBase? ToDomainOptions()
    {
        DomainOptionsBase? options;
        if (JoinDomainProvisioningFile != null)
        {
            // Files created by djoin.exe have a null character at the end for some reason, which
            // must be removed because XmlWriter doesn't like it.
            options = new ProvisionedDomainOptions(File.ReadAllText(JoinDomainProvisioningFile.FullName).Trim('\0'));
        }
        else if (JoinDomain != null)
        {
            options = new DomainOptions(JoinDomain, new DomainCredential(JoinDomainUser!, JoinDomainPassword!))
            {
                OUPath = OUPath,
            };
        }
        else
        {
            return null;
        }

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
}
