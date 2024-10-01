using System.Data.Common;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Ookii.AnswerFile;

/// <summary>
/// A generator for Windows unattended installation files.
/// </summary>
/// <remarks>
/// <para>
///   Create an instance of the <see cref="GeneratorOptions"/> class with the answer file options
///   you wish to use, and call the <see cref="Generate(string, GeneratorOptions)"/> method or
///   one of its overloads to generate an answer file with those options.
/// </para>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public class Generator
{
    internal const string PublicKeyToken = "31bf3856ad364e35";

    private static readonly XmlWriterSettings XmlSettings = new()
    {
        Indent = true,
        NamespaceHandling = NamespaceHandling.OmitDuplicates,
        Encoding = new UTF8Encoding(false),
    };

    private Generator(XmlWriter writer, GeneratorOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(options);
        Writer = writer;
        Options = options;
    }

    /// <summary>
    /// Gets the options used for this generator.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="GeneratorOptions"/> class.
    /// </value>
    public GeneratorOptions Options { get; }

    /// <summary>
    /// Gets the <see cref="XmlWriter"/> that output is written to.
    /// </summary>
    /// <value>
    /// An instance of a class derived from the <see cref="XmlWriter"/> class.
    /// </value>
    public XmlWriter Writer { get; }

    /// <summary>
    /// Generates an unattended installation answer file and writes it to the specified
    /// <see cref="XmlWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> to write to.</param>
    /// <param name="options">The options for the unattended installation.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="writer"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public static void Generate(XmlWriter writer, GeneratorOptions options)
    {
        var generator = new Generator(writer, options);
        generator.Generate();
    }

    /// <summary>
    /// Generates an unattended installation answer file and writes it to the specified file.
    /// </summary>
    /// <param name="outputPath">The path of the file to write to.</param>
    /// <param name="options">The options for the unattended installation.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="outputPath"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public static void Generate(string outputPath, GeneratorOptions options)
    {
        using var writer = XmlWriter.Create(outputPath, XmlSettings);
        Generate(writer, options);
    }

    /// <summary>
    /// Generates an unattended installation answer file and writes it to the specified
    /// <see cref="TextWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
    /// <param name="options">The options for the unattended installation.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="writer"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public static void Generate(TextWriter writer, GeneratorOptions options)
    {
        using var xmlWriter = XmlWriter.Create(writer, XmlSettings);
        Generate(xmlWriter, options);
    }

    private void Generate()
    {
        Writer.WriteStartElement("unattend", "urn:schemas-microsoft-com:unattend");
        Writer.WriteAttributes(new KeyValueList
        {
            { "xmlns:wcm", "http://schemas.microsoft.com/WMIConfig/2002/State" },
            { "xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance" }
        });
        GenerateServicing();
        GenerateWindowsPePass();
        GenerateSpecializePass();
        GenerateOobePass();
        Writer.WriteEndElement();
    }

    private void GenerateServicing()
    {
        if (Options.InstallOptions != null)
        {
            Options.InstallOptions.GenerateServicingPass(this);
            if (Options.InstallOptions.JoinDomainOffline && Options.JoinDomain != null)
            {
                using var pass = Writer.WriteAutoCloseElement("offlineServicing");
                using var join = WriteComponentStart("Microsoft-Windows-UnattendedJoin");
                Options.JoinDomain.WriteDomainElements(this, true);
            }
        }
    }

    private void GenerateWindowsPePass()
    {
        Options.InstallOptions?.GenerateWindowsPePass(this);
    }

    private void GenerateSpecializePass()
    {
        using var pass = WritePassStart("specialize");
        WriteInternationalCore();
        if (Options.JoinDomain != null && Options.InstallOptions?.JoinDomainOffline != true)
        {
            using var join = WriteComponentStart("Microsoft-Windows-UnattendedJoin");
            Options.JoinDomain.WriteDomainElements(this, false);
        }

        if (Options.ComputerName != null || Options.ProductKey != null)
        {
            using var shellSetup = WriteComponentStart("Microsoft-Windows-Shell-Setup");
            if (Options.ComputerName != null)
            {
                Writer.WriteElementString("ComputerName", Options.ComputerName);
            }

            if (Options.ProductKey != null)
            {
                // This one activates Windows.
                Writer.WriteElementString("ProductKey", Options.ProductKey);
            }
        }

        if (!Options.EnableDefender)
        {
            using var defender = WriteComponentStart("Security-Malware-Windows-Defender");
            Writer.WriteElementString("DisableAntiSpyware", "true");
        }

        if (Options.EnableRemoteDesktop)
        {
            using (var ts = WriteComponentStart("Microsoft-Windows-TerminalServices-LocalSessionManager"))
            {
                Writer.WriteElementString("fDenyTSConnections", "false");
            }

            using var firewall = WriteComponentStart("Networking-MPSSVC-Svc");
            Writer.WriteElements(new KeyValueList
            {
                { "FirewallGroups", new KeyValueList
                {
                    { "FirewallGroup", new KeyValueList
                    {
                        { "_attributes", new KeyValueList
                        {
                            { "wcm:action", "add" },
                            { "wcm:keyValue", "rd1" }
                        }
                        },
                        { "Active", true },
                        { "Group", "Remote Desktop" },
                        { "Profile", "all" }
                    }
                    }
                }
                }
            });
        }

        if (!Options.EnableCloud)
        {
            using var deployment = WriteComponentStart("Microsoft-Windows-Deployment");
            using var runSynchronous = Writer.WriteAutoCloseElement("RunSynchronous");
            WriteRunSynchronousCommand(@"reg add HKLM\Software\Policies\Microsoft\Windows\CloudContent /v DisableWindowsConsumerFeatures /t REG_DWORD /d 1", "Disable cloud consumer features", 1);
        }

        if (!Options.EnableServerManager)
        {
            using var serverManager = WriteComponentStart("Microsoft-Windows-ServerManager-SvrMgrNc");
            Writer.WriteElementString("DoNotOpenServerManagerAtLogon", "true");
        }
    }

    private void GenerateOobePass()
    {
        using var pass = WritePassStart("oobeSystem");
        WriteInternationalCore();
        using var shellSetup = WriteComponentStart("Microsoft-Windows-Shell-Setup");
        if (Options.LocalAccounts.Count > 0 || (Options.JoinDomain?.DomainAccounts.Count > 0))
        {
            using var userAccounts = Writer.WriteAutoCloseElement("UserAccounts");
            if (Options.LocalAccounts.Count > 0)
            {
                using var localAccounts = Writer.WriteAutoCloseElement("LocalAccounts");
                foreach (var account in Options.LocalAccounts)
                {
                    Writer.WriteElements(new KeyValueList
                    {
                        { "LocalAccount", new KeyValueList
                        {
                            { "_attributes", new KeyValueList { { "wcm:action", "add" } } },
                            { "Password", new KeyValueList
                            {
                                { "Value", Convert.ToBase64String(Encoding.Unicode.GetBytes(account.Password + "Password")) },
                                { "PlainText", false },
                            }
                            },
                            { "Description", account.UserName },
                            { "DisplayName", account.UserName },
                            { "Group", account.Group },
                            { "Name", account.UserName },
                        }
                        }
                    });
                }
            }

            if (Options.JoinDomain?.DomainAccounts.Count > 0)
            {
                using var domainAccounts = Writer.WriteAutoCloseElement("DomainAccounts");
                string? lastDomain = null;
                var accounts = Options.JoinDomain.DomainAccounts
                    .Select(u => u.DomainUser.Domain == null
                        ? new DomainUserGroup(
                            new DomainUser(Options.JoinDomain.DefaultDomainAccountDomain, u.DomainUser.UserName), u.Group)
                        : u)
                    .OrderBy(u => u.DomainUser.Domain);

                foreach (var account in accounts)
                {
                    if (account.DomainUser.Domain != lastDomain)
                    {
                        if (lastDomain != null)
                        {
                            Writer.WriteElementString("Domain", lastDomain);
                            Writer.WriteEndElement(); // DomainAccountList
                        }

                        Writer.WriteStartElement("DomainAccountList");
                        Writer.WriteAttributeString("wcm", "action", null, "add");
                        lastDomain = account.DomainUser.Domain;
                    }

                    Writer.WriteElements(new KeyValueList
                    {
                        { "DomainAccount", new KeyValueList
                        {
                            { "_attributes", new KeyValueList { { "wcm:action", "add" } } },
                            { "Name",account.DomainUser.UserName},
                            { "Group", account.Group},
                        }
                        }
                    });
                }

                if (lastDomain != null)
                {
                    Writer.WriteElementString("Domain", lastDomain);
                    Writer.WriteEndElement(); // DomainAccountList
                }
            }
        }

        bool hideAccountScreens = Options.JoinDomain != null || Options.LocalAccounts.Count != 0;
        Writer.WriteElements(new KeyValueList
        {
            { "OOBE", new KeyValueList
            {
                { "ProtectYourPC", 1 },
                { "HideEULAPage", true },
                { "HideLocalAccountScreen", hideAccountScreens },
                { "HideOEMRegistrationScreen", true },
                { "HideOnlineAccountScreens", hideAccountScreens },
                { "HideWirelessSetupInOOBE", hideAccountScreens }
            }
            }
        });

        Writer.WriteElementString("TimeZone", Options.TimeZone);
        if (Options.AutoLogon?.Count > 0)
        {
            // Windows adds 1 to the LogonCount value, so subtract one to get the correct number
            // of logons. If the count is 1, we use a first logon command to work around that.
            // See https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-shell-setup-autologon-logoncount
            var count = Options.AutoLogon.Count;
            if (count > 1)
            {
                --count;
            }

            Writer.WriteElements(new KeyValueList
            {
                { "AutoLogon", new KeyValueList
                {
                    { "Enabled", true },
                    { "LogonCount", count },
                    { "Domain", Options.AutoLogon.Credential.UserAccount.Domain },
                    { "Username", Options.AutoLogon.Credential.UserAccount.UserName },
                    { "Password", new KeyValueList
                    {
                        { "Value", Convert.ToBase64String(Encoding.Unicode.GetBytes(Options.AutoLogon.Credential.Password + "Password")) },
                        { "PlainText", false }
                    }
                    }
                }
                }
            });
        }

        using (var firstLogon = Writer.WriteAutoCloseElement("FirstLogonCommands"))
        {
            int order = 1;
            
            // Work around the LogonCount issue if the count is 1 (see above).
            if (Options.AutoLogon?.Count == 1)
            {
                WriteSynchronousCommand(@"reg add ""HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"" /v AutoLogonCount /t REG_DWORD /d 0 /f", "LogonCount", order);
                ++order;
            }

            foreach (var command in Options.FirstLogonCommands)
            {
                WriteSynchronousCommand(command, "Custom command", order);
                ++order;
            }

            foreach (var script in Options.FirstLogonScripts)
            {
                WriteSynchronousCommand($"PowerShell.exe -ExecutionPolicy Bypass {script}", "Setup script", order);
                ++order;
            }
        }

        if (Options.DisplayResolution is Resolution resolution)
        {
            Writer.WriteElements(new KeyValueList
            {
                { "Display", new KeyValueList
                {
                    { "HorizontalResolution", resolution.Width },
                    { "VerticalResolution", resolution.Height },
                }
                }
            });
        }
    }

    internal AutoCloseElement WriteComponentStart(string name)
    {
        return Writer.WriteAutoCloseElement("component", new KeyValueList
        {
            { "name", name },
            { "processorArchitecture", Options.ProcessorArchitecture },
            { "publicKeyToken", PublicKeyToken },
            { "language", "neutral" },
            { "versionScope", "nonSxS" },
        });
    }

    internal AutoCloseElement WritePassStart(string pass)
    {
        return Writer.WriteAutoCloseElement("settings", new KeyValueList { { "pass", pass } });
    }

    internal void WriteInternationalCore(bool winPe = false)
    {
        var name = "Microsoft-Windows-International-Core";
        if (winPe)
        {
            name += "-WinPE";
        }

        using var component = WriteComponentStart(name);
        Writer.WriteElements(new KeyValueList
        {
            { "InputLocale", Options.Language },
            { "SystemLocale", Options.Language },
            { "UILanguage", Options.Language },
            { "UserLocale", Options.Language },
        });
    }

    internal void WriteCreatePartition(int order, string type, int? size = null)
    {
        Writer.WriteElements(new KeyValueList
        {
            { "CreatePartition", new KeyValueList
            {
                { "_attributes", new KeyValueList { { "wcm:action", "add" } } },
                { "Order", order },
                { "Size", size },
                { "Type", type },
                { "Extend", size.HasValue ? (bool?)null : true },
            }
            }
        });
    }

    internal void WriteModifyPartition(int order, int partitionId, string? format = null, string? label = null, char? letter = null, bool? extend = null, bool? active = null, string? typeId = null)
    {
        Writer.WriteElements(new KeyValueList
        {
            { "ModifyPartition", new KeyValueList
            {
                { "_attributes", new KeyValueList { { "wcm:action", "add" } } },
                { "Order", order },
                { "PartitionId", partitionId },
                { "Format", format },
                { "Label", label },
                { "Letter", letter },
                { "Extend", extend },
                { "Active", active },
                { "TypeID", typeId },
            }
            }
        });
    }

    private void WriteRunSynchronousCommand(string path, string description, int order)
    {
        Writer.WriteElements(new KeyValueList
        {
            { "RunSynchronousCommand", new KeyValueList
            {
                { "_attributes", new KeyValueList { { "wcm:action", "add" } } },
                { "Order", order },
                { "Description", description },
                { "Path", path },
            }
            }
        });
    }

    private void WriteSynchronousCommand(string commandLine, string description, int order)
    {
        Writer.WriteElements(new KeyValueList
        {
            { "SynchronousCommand", new KeyValueList
            {
                { "_attributes", new KeyValueList { { "wcm:action", "add" } } },
                { "Order", order },
                { "Description", description },
                { "CommandLine", commandLine },
            }
            }
        });
    }
}
