using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for generating an unattended Windows installation answer file using the
/// <see cref="Generator"/> class.
/// </summary>
/// <threadsafety instance="false" static="true"/>
[JsonSourceGenerationOptions()]
public class GeneratorOptions
{
    private Collection<LocalCredential>? _localAccounts;
    private Collection<string>? _firstLogonCommands;
    private Collection<string>? _setupScripts;

    /// <summary>
    /// Gets or sets the installation method to use, along with the options for that method.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="ManualInstallOptions"/>, <see
    /// cref="ExistingPartitionOptions"/>, <see cref="CleanBiosOptions"/> or <see
    /// cref="CleanEfiOptions"/> class, or <see langword="null"/> to generate an answer file for a
    /// pre-installed Windows image, such as one generated with sysprep. The default value is
    /// <see langword="null"/>.
    /// </value>
    public InstallOptionsBase? InstallOptions { get; set; }

    /// <summary>
    /// Gets or sets options for joining a domain.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="DomainOptions"/> class, or <see langword="null"/> to not join
    /// a domain. The default value is <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// <note type="security">
    ///   The password of the account used to join the domain is stored in plain text in the answer
    ///   file. Do not store answer files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    public DomainOptions? JoinDomain { get; set; }

    /// <summary>
    /// Gets or sets the computer name of the system.
    /// </summary>
    /// <value>
    /// The computer name, or <see langword="null"/> to let Windows pick a computer name.
    /// </value>
    public string? ComputerName { get; set; }

    /// <summary>
    /// Gets or sets a value which indicates whether Windows Defender is enabled after installation.
    /// </summary>
    /// <value>
    /// <see langword="true"/> to enable Windows Defender; otherwise, <see langword="false"/>. The
    /// default value is <see langword="true"/>.
    /// </value>
    public bool EnableDefender { get; set; } = true;

    /// <summary>
    /// Gets or sets a value which indicates whether cloud consumer features are enabled after
    /// installation.
    /// </summary>
    /// <value>
    /// <see langword="true"/> to enable cloud consumer features; otherwise, <see langword="false"/>.
    /// The default value is <see langword="true"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   If this value is <see langword="false"/>, the value DisableWindowsConsumerFeatures under
    ///   the registry key HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\CloudContent
    ///   will be set to 1. On certain versions of Windows, this may prevent auto-installation of
    ///   additional recommended store apps for new user accounts.
    /// </para>
    /// </remarks>
    public bool EnableCloud { get; set; } = true;

    /// <summary>
    /// Gets or sets a value which indicates whether remote desktop accepts incoming connections
    /// after installation.
    /// </summary>
    /// <value>
    /// <see langword="true"/> to enable remote desktop; otherwise, <see langword="false"/>. The
    /// default value is <see langword="false"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   If this property is <see langword="true"/>, remote desktop will be enabled, and a
    ///   firewall rule will be created to allow remote desktop connections. This will automatically
    ///   give all members of the Administrators group remote access to the machine.
    /// </para>
    /// </remarks>
    public bool EnableRemoteDesktop { get; set; }

    /// <summary>
    /// Gets or sets a value which indicates whether server manager will be launched on logon on
    /// Windows Server.
    /// </summary>
    /// <value>
    /// <see langword="true"/> to enable server manager; otherwise, <see langword="false"/>. the
    /// default value is <see langword="true"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   This property has no effect on Windows installations that are not a Server SKU.
    /// </para>
    /// </remarks>
    public bool EnableServerManager { get; set; } = true;

    /// <summary>
    /// Gets or sets a collection of local administrator accounts to create.
    /// </summary>
    /// <value>
    /// A collection of local user accounts.
    /// </value>
    /// <remarks>
    /// <para>
    ///   All accounts specified by this property will be created as members of the local
    ///   Administrators group.
    /// </para>
    /// <note type="security">
    ///   The passwords for the local accounts are stored using base64 encoding in the answer file;
    ///   they are not encrypted. Do not store answer files with sensitive passwords in public
    ///   locations.
    /// </note>
    /// </remarks>
    public Collection<LocalCredential> LocalAccounts
    {
        get => _localAccounts ??= new();
        set => _localAccounts = value;
    }

    /// <summary>
    /// Gets or sets options for logging on automatically.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="AutoLogonOptions"/> class, or <see langword="null"/> to not
    /// use automatic log-on. The default value is <see langword="null"/>.
    /// </value>
    public AutoLogonOptions? AutoLogon { get; set; }

    /// <summary>
    /// Gets or sets the credentials of an account that will be used to access all network
    /// locations.
    /// </summary>
    /// <value>
    /// A instance of the <see cref="DomainCredential"/> class, or <see langword="null"/> to not
    /// set up a cmdkey user. The default value is <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   Setting this property to a value other than <see langword="null"/> will add a first log-on
    ///   command to the answer file that uses 'cmdkey.exe' to use the specified account for all
    ///   network destinations. Using this is not very secure and should only be done in test
    ///   environments.
    /// </para>
    /// <note type="security">
    ///   The password of this account is stored in plain text in the answer file. Do not store
    ///   answer files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    public DomainCredential? CmdKeyAccount { get; set; }

    /// <summary>
    /// Gets or sets the display resolution.
    /// </summary>
    /// <value>
    /// A <see cref="Size"/> value with the resolution, or <see langword="null"/> to let Windows
    /// determine the resolution. The default value is <see langword="null"/>.
    /// </value>
    [JsonConverter(typeof(SizeJsonConverter))]
    public Size? DisplayResolution { get; set; }

    /// <summary>
    /// Gets or sets the language used for the UI and culture settings.
    /// </summary>
    /// <value>
    /// A language code. The default value is "en-US".
    /// </value>
    public string Language { get; set; } = "en-US";

    /// <summary>
    /// Gets or sets the Windows product key.
    /// </summary>
    /// <value>
    /// The product key, or <see langword="null"/> to install without a key.
    /// </value>
    /// <remarks>
    /// <para>
    ///   The product key is used to select a Windows edition, and to activate Windows.
    /// </para>
    /// <para>
    ///   A product key is required in the answer file for most editions of Windows, but may not
    ///   be required if the edition uses alternate activation methods such as volume licensing.
    /// </para>
    /// </remarks>
    public string? ProductKey { get; set; }

    /// <summary>
    /// Gets or sets the processor architecture of the Windows version being installed.
    /// </summary>
    /// <value>
    /// The processor architecture. The default value is "amd64".
    /// </value>
    /// <remarks>
    /// <para>
    ///   Use "amd64" for 64 bit Intel and AMD systems, and "x86" for 32 bit systems. Other values
    ///   may also be valid, such as "arm64".
    /// </para>
    /// </remarks>
    public string ProcessorArchitecture { get; set; } = "amd64";

    /// <summary>
    /// Gets or sets the time zone.
    /// </summary>
    /// <value>
    /// The standard name of a time zone. The default value is "Pacific Standard Time".
    /// </value>
    /// <remarks>
    /// Use the <c>tzutil /l</c> command to list valid time zone names.
    /// </remarks>
    public string TimeZone { get; set; } = "Pacific Standard Time";

    /// <summary>
    /// Gets or sets a collection of commands to run during first log-on.
    /// </summary>
    /// <value>
    /// A collection of commands.
    /// </value>
    /// <remarks>
    /// <para>
    ///   These commands will run before any scripts specified by the <see cref="SetupScripts"/>
    ///   property.
    /// </para>
    /// </remarks>
    public Collection<string> FirstLogonCommands
    {
        get => _firstLogonCommands ??= new();
        set => _firstLogonCommands = value;
    }


    /// <summary>
    /// Gets or sets a collection of PowerShell scripts to run during first log-on.
    /// </summary>
    /// <value>
    /// A collection of scripts, with their path and arguments.
    /// </value>
    /// <remarks>
    /// <para>
    ///   These scripts will run after any commands specified by the <see cref="FirstLogonCommands"/>
    ///   property.
    /// </para>
    /// </remarks>
    public Collection<string> SetupScripts
    {
        get => _setupScripts ??= new();
        set => _setupScripts = value;
    }
}
