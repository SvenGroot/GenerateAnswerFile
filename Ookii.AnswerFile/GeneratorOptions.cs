using System.Collections.ObjectModel;
using System.Drawing;

namespace Ookii.AnswerFile;

public class GeneratorOptions
{
    public InstallOptionsBase? InstallOptions { get; set; }

    public DomainOptions? JoinDomain { get; set; }

    public string? ComputerName { get; set; }

    public bool EnableDefender { get; set; } = true;

    public bool EnableCloud { get; set; } = true;

    public bool EnableRemoteDesktop { get; set; }

    public Collection<LocalCredential> LocalAccounts { get; } = new();

    public AutoLogonOptions? AutoLogon { get; set; }

    public DomainCredential? CmdKeyAccount { get; set; }

    public Size? DisplayResolution { get; set; }

    public string Language { get; set; } = "en-US";

    public string? ProductKey { get; set; }

    public string ProcessorArchitecture { get; set; } = "amd64";

    public string TimeZone { get; set; } = "Pacific Standard Time";

    public Collection<string> SetupScripts { get; } = new();
}