using System.Collections.ObjectModel;
using System.Drawing;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for generating an unattended Windows installation answer file.
/// </summary>
/// <seealso cref="Generator"/>
public class GeneratorOptions
{
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
    /// <warning>
    ///   The password of the account used to join the domain is stored in plain text.
    /// </warning>
    /// </remarks>
    public DomainOptions? JoinDomain { get; set; }

    /// <summary>
    /// Gets or sets the computer name of the system after installation.
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
    public bool EnableCloud { get; set; } = true;

    /// <summary>
    /// Gets or sets a value which indicates whether remote desktop accepts incomding connections
    /// after installation.
    /// </summary>
    /// <value>
    /// <see langword="true"/> to enable remote desktop; otherwise, <see langword="false"/>. The
    /// default value is <see langword="false"/>.
    /// </value>
    public bool EnableRemoteDesktop { get; set; }

    /// <summary>
    /// Gets a collection of local administrator accounts to create.
    /// </summary>
    /// <value>
    /// A collection with the local user accounts.
    /// </value>
    /// <remarks>
    /// <warning>
    ///   The passwords for the local accounts are stored using base64 encoding. They are not
    ///   encrypted.
    /// </warning>
    /// </remarks>
    public Collection<LocalCredential> LocalAccounts { get; } = new();

    /// <summary>
    /// Gets or sets options for automatic logging on to Windows.
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
    ///   This adds a command to the answer file that uses 'cmdkey.exe' to use the specified
    ///   account for all network destinations. Using this is not very secure and should only be
    ///   done in test environments.
    /// </para>
    /// <warning>
    ///   The password of the cmdkey account is stored in plain text.
    /// </warning>
    /// </remarks>
    public DomainCredential? CmdKeyAccount { get; set; }

    /// <summary>
    /// Gets or sets the display resolution.
    /// </summary>
    /// <value>
    /// A <see cref="Size"/> value with the resolution, or <see langword="null"/> to let Windows
    /// determine the resolution. The default value is <see langword="null"/>.
    /// </value>
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
    ///   Depending on the edition being installed, a product key may or may not be required.
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
    ///   Use "amd64" for 64 bit systems, and "x86" for 32 bit systems. Other values may also be
    ///   valid, such as "arm64".
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
    /// Gets a collection of PowerShell scripts to run during first log-on.
    /// </summary>
    /// <value>
    /// A collection of scripts, with their path and arguments.
    /// </value>
    public Collection<string> SetupScripts { get; } = new();
}