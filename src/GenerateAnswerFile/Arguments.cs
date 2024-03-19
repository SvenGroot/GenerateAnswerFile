using Ookii.AnswerFile;
using Ookii.CommandLine;
using Ookii.CommandLine.Conversion;
using Ookii.CommandLine.Validation;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

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

    #region Installation options

    [CommandLineArgument("Feature")]
    [ResourceDescription(nameof(Properties.Resources.FeaturesDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("c")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanBios, InstallMethod.CleanEfi, InstallMethod.Manual)]
    [Requires(nameof(WindowsVersion))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? Features { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.InstallDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("i")]
    [ValidateEnumValue]
    public InstallMethod Install { get; set; } = InstallMethod.PreInstalled;

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.InstallToDiskDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("disk")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    [ValidateRange(0, null)]
    public int InstallToDisk { get; set; } = 0;

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.InstallToPartitionDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("part")]
    [ValidateInstallMethod(InstallMethod.CleanEfi, InstallMethod.CleanBios, InstallMethod.ExistingPartition)]
    [ValidateRange(1, null)]
    public int? InstallToPartition { get; set; }

    [CommandLineArgument("Partition")]
    [ResourceDescription(nameof(Properties.Resources.PartitionsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.PartitionsValueDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("p")]
    [ValidateInstallMethod(InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    [MultiValueSeparator]
    public Partition[]? Partitions { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ImageIndexDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("wim")]
    [ValidateInstallMethod(InstallMethod.ExistingPartition, InstallMethod.CleanEfi, InstallMethod.CleanBios)]
    public int ImageIndex { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ProductKeyDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("key")]
    [ValidateNotWhiteSpace]
    public string? ProductKey { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.WindowsVersionDescription))]
    [ArgumentCategory(ArgumentCategory.Install)]
    [Alias("v")]
    public Version? WindowsVersion { get; set; }


    #endregion

    #region User account options

    [CommandLineArgument("LocalAccount")]
    [ResourceDescription(nameof(Properties.Resources.LocalAccountsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.LocalCredentialValueDescription))]
    [ArgumentCategory(ArgumentCategory.UserAccounts)]
    [Alias("a")]
    [MultiValueSeparator]
    public LocalCredential[]? LocalAccounts { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.CmdKeyUserDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.DomainUserValueDescription))]
    [ArgumentCategory(ArgumentCategory.UserAccounts)]
    [Alias("cku")]
    [Requires(nameof(CmdKeyPassword))]
    [ValidateNotWhiteSpace]
    public DomainUser? CmdKeyUser { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.CmdKeyPasswordDescription))]
    [ArgumentCategory(ArgumentCategory.UserAccounts)]
    [Alias("ckp")]
    [Requires(nameof(CmdKeyUser))]
    [ValidateNotWhiteSpace]
    public string? CmdKeyPassword { get; set; }

    #endregion

    #region AutoLogon options

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonUserDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.OptionalDomainUserValueDescription))]
    [ArgumentCategory(ArgumentCategory.AutoLogon)]
    [Alias("alu")]
    [Requires(nameof(AutoLogonPassword))]
    [ValidateNotWhiteSpace]
    public DomainUser? AutoLogonUser { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonPasswordDescription))]
    [ArgumentCategory(ArgumentCategory.AutoLogon)]
    [Alias("alp")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateNotWhiteSpace]
    public string? AutoLogonPassword { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.AutoLogonCountDescription))]
    [ArgumentCategory(ArgumentCategory.AutoLogon)]
    [Alias("alc")]
    [Requires(nameof(AutoLogonUser))]
    [ValidateRange(1, null)]
    public int AutoLogonCount { get; set; } = 1;

    #endregion

    #region Domain options

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainDescription))]
    [ArgumentCategory(ArgumentCategory.Domain)]
    [Requires(nameof(JoinDomainUser), nameof(JoinDomainPassword))]
    [Alias("jd")]
    [ValidateNotWhiteSpace]
    public string? JoinDomain { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainUserDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.OptionalDomainUserValueDescription))]
    [ArgumentCategory(ArgumentCategory.Domain)]
    [Alias("jdu")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public DomainUser? JoinDomainUser { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.JoinDomainPasswordDescription))]
    [ArgumentCategory(ArgumentCategory.Domain)]
    [Alias("jdp")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? JoinDomainPassword { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.OUPathDescription))]
    [ArgumentCategory(ArgumentCategory.Domain)]
    [Alias("ou")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    public string? OUPath { get; set; }

    [CommandLineArgument("DomainAccount")]
    [ResourceDescription(nameof(Properties.Resources.DomainAccountsDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.DomainUserGroupValueDescription))]
    [ArgumentCategory(ArgumentCategory.Domain)]
    [Alias("da")]
    [Requires(nameof(JoinDomain))]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public DomainUserGroup[]? DomainAccounts { get; set; }

    #endregion

    #region Other options

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ComputerNameDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("n")]
    [ValidateNotWhiteSpace]
    public string? ComputerName { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisableDefenderDesciption))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("d")]
    public bool DisableDefender { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisableCloudDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("dc")]
    public bool DisableCloud { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisableServerManagerDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("dsm")]
    public bool DisableServerManager { get; set; }

    [CommandLineArgument("FirstLogonCommand")]
    [ResourceDescription(nameof(Properties.Resources.FirstLogonCommandsDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("cmd")]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? FirstLogonCommands { get; set; }

    [CommandLineArgument("SetupScript")]
    [ResourceDescription(nameof(Properties.Resources.SetupScriptsDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("s")]
    [ValidateNotWhiteSpace]
    [MultiValueSeparator]
    public string[]? SetupScripts { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.EnableRemoteDesktopDescriptoin))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("rdp")]
    public bool EnableRemoteDesktop { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.DisplayResolutionDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("res")]
    [ArgumentConverter(typeof(WrappedDefaultTypeConverter<Size>))]
    public Size? DisplayResolution { get; set; }

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.LanguageDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("lang")]
    [ValidateNotWhiteSpace]
    public string Language { get; set; } = "en-US";

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.ProcessorArchitectureDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [Alias("arch")]
    [ValidateNotWhiteSpace]
    public string ProcessorArchitecture { get; set; } = "amd64";

    [CommandLineArgument]
    [ResourceDescription(nameof(Properties.Resources.TimeZoneDescription))]
    [ArgumentCategory(ArgumentCategory.Other)]
    [ValidateNotWhiteSpace]
    public string TimeZone { get; set; } = "Pacific Standard Time";

    // Shows detailed information if an exception occurs.
    [CommandLineArgument(IsHidden = true)]
    public bool Debug { get; set; }

    [CommandLineArgument]
    [Alias("oh")]
    [Alias("??")]
    [ResourceDescription(nameof(Properties.Resources.OnlineHelpDescription))]
    public static CancelMode OnlineHelp(CommandLineParser parser)
    {
        try
        {
            var info = new ProcessStartInfo(Properties.Resources.OnlineHelpUrl)
            {
                UseShellExecute = true,
            };

            Process.Start(info);
        }
        catch (Exception ex)
        {
            if ((bool?)parser.GetArgument(nameof(Debug))!.Value ?? false)
            {
                Console.Error.WriteLine(ex.ToString());
            }

            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Properties.Resources.UsageHelpFooterFormat,
                CommandLineParser.GetExecutableName()));
        }

        return CancelMode.Abort;
    }

#if DEBUG

    // This argument is used to generate the bulk of CommandLine.md. It has no real value otherwise,
    // so is excluded from the release.
    // N.B. Some modifications are made manually in CommandLine.md, so when updating do so
    // selectively.
    [CommandLineArgument(IsHidden = true)]
    public static CancelMode MarkdownHelp(CommandLineParser parser)
    {
        var writer = ((CustomUsageWriter)parser.Options.UsageWriter);
        writer.Markdown = true;
        writer.IncludeApplicationDescription = false;
        writer.UseAbbreviatedSyntax = false;
        parser.HelpRequested = true;
        return CancelMode.Abort;
    }

#endif

#endregion

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

        if (FirstLogonCommands != null)
        {
            options.FirstLogonCommands.AddRange(FirstLogonCommands);
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

        var user = JoinDomainUser!;
        if (user.Domain ==  null)
        {
            user = new DomainUser(JoinDomain, user.UserName);
        }

        var options = new DomainOptions(JoinDomain, new DomainCredential(user, JoinDomainPassword!))
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
