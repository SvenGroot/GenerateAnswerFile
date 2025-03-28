using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for generating an unattended Windows installation answer file using the
/// <see cref="AnswerFileGenerator"/> class.
/// </summary>
/// <threadsafety instance="false" static="true"/>
public partial class AnswerFileOptions
{
    private string? _computerName;
    private Collection<LocalCredential>? _localAccounts;
    private Collection<string>? _firstLogonCommands;
    private Collection<string>? _firstLogonScripts;

    /// <summary>
    /// Gets the schema that can be used for validation of the JSON representation of this object.
    /// </summary>
    /// <value>
    /// The JSON schema URL.
    /// </value>
    // This must be the first property and must not be static, so ToJson will insert the $schema
    // property into the output.
    [JsonPropertyName("$schema")]
    public string JsonSchema => "https://www.ookii.org/Link/AnswerFileJsonSchema-2.2";

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
    /// An instance of the <see cref="DomainOptions"/> class, the <see cref="ProvisionedDomainOptions"/>
    /// class, or <see langword="null"/> to not join a domain. The default value is
    /// <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// <note type="security">
    ///   The password of the account used to join the domain is stored in plain text in the answer
    ///   file. Do not store answer files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    public DomainOptionsBase? JoinDomain { get; set; }

    /// <summary>
    /// Gets or sets the computer name of the system.
    /// </summary>
    /// <value>
    /// The computer name, or <see langword="null"/> to let Windows pick a computer name. The
    /// default value is <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   If this property is set to a value containing the # character, each # will be replaced
    ///   with a random digit between 0 and 9. For example, the value "PC-###" will be replaced with
    ///   "PC-123" (or any other random number).
    /// </para>
    /// <note type="note">
    ///   While random numbers can be used to generate a distinct computer name, it is not
    ///   necessarily guaranteed to be a unique name on the network.
    /// </note>
    /// </remarks>
    public string? ComputerName
    {
        get => _computerName;
        set
        {
            if (value != null)
            {
                // Replace # with random digits.
                value = RandomNumberRegex().Replace(value,
                    m => Random.Shared.Next().ToString(new string('0', m.Length)).Substring(0, m.Length));
            }

            _computerName = value;
        }
    }

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
    /// Gets or sets a value which indicates whether server manager will be launched when logging
    /// in on Windows Server.
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
    /// Gets or sets a collection of local accounts to create.
    /// </summary>
    /// <value>
    /// A collection of local user accounts.
    /// </value>
    /// <remarks>
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
    /// Gets or sets the password for the local Administrator account.
    /// </summary>
    /// <value>
    /// The password for the administrator account, or <see langword="null"/> to leave the account
    /// disabled. The default value is <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   This password will be applied to the built-in local Administrator account. If this
    ///   property is <see langword="null"/>, this account is disabled by default. If this property
    ///   is set to an empty string, the account will be enabled with no password.
    /// </para>
    /// <note type="security">
    ///   The password is stored using base64 encoding in the answer file; it is not encrypted.
    ///   Ensure that answer files containing sensitive information are stored securely and are
    ///   not exposed to unauthorized parties.
    /// </note>
    /// </remarks>
    public string? AdministratorPassword { get; set; }

    /// <summary>
    /// Gets or sets options for logging on automatically.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="AutoLogonOptions"/> class, or <see langword="null"/> to not
    /// An instance of the <see cref="AutoLogonOptions"/> class, or <see langword="null"/> to not
    /// use automatic log-on. The default value is <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// <note type="security">
    ///   The password of the account used to log on automatically is stored using base64 encoding
    ///   in the answer file; it is not encrypted. Do not store answer files with sensitive
    ///   passwords in public locations.
    /// </note>
    /// </remarks>
    public AutoLogonOptions? AutoLogon { get; set; }

    /// <summary>
    /// Gets or sets the display resolution.
    /// </summary>
    /// <value>
    /// A <see cref="Resolution"/> value, or <see langword="null"/> to let Windows determine the
    /// resolution. The default value is <see langword="null"/>.
    /// </value>
    public Resolution? DisplayResolution { get; set; }

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
    /// Gets or sets the system time zone.
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
    ///   These commands will run before any scripts specified by the <see cref="FirstLogonScripts"/>
    ///   property.
    /// </para>
    /// </remarks>
    public Collection<string> FirstLogonCommands
    {
        get => _firstLogonCommands ??= [];
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
    ///   The scripts specified by this property will be executed by invoking Windows PowerShell
    ///   using <c>PowerShell.exe -ExecutionPolicy Bypass</c>. This is provided for convenience,
    ///   and there is no difference between this property and using the <see cref="FirstLogonCommands"/>
    ///   property to explicitly invoke PowerShell.
    /// </para>
    /// <para>
    ///   The scripts will run after any commands specified by the <see cref="FirstLogonCommands"/>
    ///   property.
    /// </para>
    /// </remarks>
    public Collection<string> FirstLogonScripts
    {
        get => _firstLogonScripts ??= [];
        set => _firstLogonScripts = value;
    }

    /// <summary>
    /// Creates an instance of the <see cref="AnswerFileOptions"/> class based on the specified JSON
    /// value.
    /// </summary>
    /// <param name="json">The JSON value.</param>
    /// <returns>
    /// An instance of the <see cref="AnswerFileOptions"/> class, or <see langword="null"/> if
    /// <paramref name="json"/> is a single <see langword="null"/> value.
    /// </returns>
    /// <exception cref="JsonException">
    /// <paramref name="json"/> is not a valid JSON representation of the
    /// <see cref="AnswerFileOptions"/> class.
    /// </exception>
    public static AnswerFileOptions? FromJson(ReadOnlySpan<char> json)
        => JsonSerializer.Deserialize(json, SourceGenerationContext.Default.AnswerFileOptions);

    /// <summary>
    /// Serializes the current instance to a JSON string.
    /// </summary>
    /// <returns>A JSON representation of the current instance.</returns>
    public string ToJson()
        => JsonSerializer.Serialize(this, typeof(AnswerFileOptions), SourceGenerationContext.Default);

    // This regex is used to replace the # characters in the computer name with random digits.
    // It processes groups of # in batches of 9, to avoid exceeding the maximum value of an int
    // for a single random number.
    [GeneratedRegex("#{1,9}")]
    private static partial Regex RandomNumberRegex();
}
