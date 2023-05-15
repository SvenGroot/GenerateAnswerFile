using System.Data.Common;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Ookii.AnswerFile;

/// <summary>
/// A generator for Windows unattended installation files.
/// </summary>
/// <seealso cref="GeneratorOptions"/>
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
    public static void Generate(TextWriter writer, GeneratorOptions options)
    {
        using var xmlWriter = XmlWriter.Create(writer, XmlSettings);
        Generate(xmlWriter, options);
    }

    private void Generate()
    {
        Writer.WriteStartElement("unattend", "urn:schemas-microsoft-com:unattend");
        Writer.WriteAttributes(new
        {
            xmlns_wcm = "http://schemas.microsoft.com/WMIConfig/2002/State",
            xmlns_xsi = "http://www.w3.org/2001/XMLSchema-instance"
        });
        GenerateServicing();
        GenerateWindowsPePass();
        GenerateSpecializePass();
        GenerateOobePass();
        Writer.WriteEndElement();
    }

    private void GenerateServicing()
    {
        Options.InstallOptions?.GenerateServicingPass(this);
    }

    private void GenerateWindowsPePass()
    {
        Options.InstallOptions?.GenerateWindowsPePass(this);
    }

    private void GenerateSpecializePass()
    {
        using var pass = WritePassStart("specialize");
        WriteInternationalCore();
        if (Options.JoinDomain is DomainOptions domainOptions)
        {
            using var join = WriteComponentStart("Microsoft-Windows-UnattendedJoin");
            Writer.WriteElements(new
            {
                Identification = new
                {
                    UnsecureJoin = "false",
                    Credentials = new
                    {
                        domainOptions.Credential.UserAccount.Domain,
                        domainOptions.Credential.Password,
                        Username = domainOptions.Credential.UserAccount.UserName
                    },
                    JoinDomain = domainOptions.Domain,
                    MachineObjectOU = domainOptions.OUPath,
                }
            });
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
            Writer.WriteElements(new
            {
                FirewallGroups = new
                {
                    FirewallGroup = new
                    {
                        _attributes = new
                        {
                            wcm_action = "add",
                            wcm_keyValue = "rd1"
                        },
                        Active = "true",
                        Group = "Remote Desktop",
                        Profile = "all"
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
                    Writer.WriteElements(new
                    {
                        LocalAccount = new
                        {
                            _attributes = new { wcm_action = "add" },
                            Password = new
                            {
                                Value = Convert.ToBase64String(Encoding.Unicode.GetBytes(account.Password + "Password")),
                                PlainText = "false",
                            },
                            Description = account.UserName,
                            DisplayName = account.UserName,
                            Group = "Administrators",
                            Name = account.UserName,
                        }
                    });
                }
            }

            if (Options.JoinDomain is DomainOptions domainOptions && domainOptions.DomainAccounts.Count > 0)
            {
                using var domainAccounts = Writer.WriteAutoCloseElement("DomainAccounts");
                using var domainAccountList = Writer.WriteAutoCloseElement("DomainAccountList", new { wcm_action = "add" });
                foreach (var account in domainOptions.DomainAccounts)
                {
                    Writer.WriteElements(new
                    {
                        DomainAccount = new
                        {
                            _attributes = new { wcm_action = "add" },
                            Name = account,
                            Group = "Administrators",
                        }
                    });
                }

                Writer.WriteElementString("Domain", domainOptions.Domain);
            }
        }

        Writer.WriteElements(new
        {
            OOBE = new
            {
                ProtectYourPC = 1,
                HideEULAPage = "true",
                HideLocalAccountScreen = "true",
                HideOEMRegistrationScreen = "true",
                HideOnlineAccountScreens = "true",
                HideWirelessSetupInOOBE = "true"
            }
        });

        Writer.WriteElementString("TimeZone", Options.TimeZone);
        if (Options.AutoLogon is AutoLogonOptions autoLogonOptions)
        {
            Writer.WriteElements(new
            {
                AutoLogon = new
                {
                    Enabled = "true",
                    LogonCount = autoLogonOptions.Count,
                    autoLogonOptions.Credential.UserAccount.Domain,
                    Username = autoLogonOptions.Credential.UserAccount.UserName,
                    Password = new
                    {
                        Value = Convert.ToBase64String(Encoding.Unicode.GetBytes(autoLogonOptions.Credential.Password + "Password")),
                        PlainText = "false"
                    }
                }
            });
        }

        using (var firstLogon = Writer.WriteAutoCloseElement("FirstLogonCommands"))
        {
            int order = 1;
            if (Options.CmdKeyAccount is DomainCredential cmdKeyOptions)
            {
                WriteSynchronousCommand($"cmdkey.exe /add:* /user:{cmdKeyOptions.UserAccount} /pass:{cmdKeyOptions.Password}", "CmdKey", order);
                ++order;
            }

            if (Options.SetupScripts.Count > 0)
            {
                WriteSynchronousCommand("PowerShell.exe Set-ExecutionPolicy unrestricted -Force", "PowerShell unrestricted", order);
                ++order;
                WriteSynchronousCommand("PowerShell.exe Set-ExecutionPolicy bypass -Force", "PowerShell bypass", order);
                ++order;
                foreach (var script in Options.SetupScripts)
                {
                    WriteSynchronousCommand($"PowerShell.exe {script}", "Setup script", order);
                    ++order;
                }
            }
        }

        if (Options.DisplayResolution is Size resolution)
        {
            Writer.WriteElements(new
            {
                Display = new
                {
                    HorizontalResolution = resolution.Width,
                    VerticalResolution = resolution.Height,
                }
            });
        }
    }

    internal AutoCloseElement WriteComponentStart(string name)
    {
        return Writer.WriteAutoCloseElement("component", new
        {
            name,
            processorArchitecture = Options.ProcessorArchitecture,
            publicKeyToken = PublicKeyToken,
            language = "neutral",
            versionScope = "nonSxS",
        });
    }

    internal AutoCloseElement WritePassStart(string pass)
    {
        return Writer.WriteAutoCloseElement("settings", new { pass });
    }

    internal void WriteInternationalCore(bool winPe = false)
    {
        var name = "Microsoft-Windows-International-Core";
        if (winPe)
        {
            name += "-WinPE";
        }

        using var component = WriteComponentStart(name);
        Writer.WriteElements(new
        {
            InputLocale = Options.Language,
            SystemLocale = Options.Language,
            UILanguage = Options.Language,
            UserLocale = Options.Language
        });
    }

    internal void WriteCreatePartition(int order, string type, int? size = null)
    {
        Writer.WriteElements(new
        {
            CreatePartition = new
            {
                _attributes = new { wcm_action = "add" },
                Order = order,
                Size = size,
                Type = type,
                Extend = size.HasValue ? null : "true",
            }
        });
    }

    internal void WriteModifyPartition(int order, int partitionId, string? format = null, string? label = null, char? letter = null)
    {
        Writer.WriteElements(new
        {
            ModifyPartition = new
            {
                _attributes = new { wcm_action = "add" },
                Order = order,
                PartitionId = partitionId,
                Format = format,
                Label = label,
                Letter = letter
            }
        });
    }

    private void WriteRunSynchronousCommand(string path, string description, int order)
    {
        Writer.WriteElements(new
        {
            RunSynchronousCommand = new
            {
                _attributes = new { wcm_action = "add" },
                Order = order,
                Description = description,
                Path = path,
            }
        });
    }

    private void WriteSynchronousCommand(string commandLine, string description, int order)
    {
        Writer.WriteElements(new
        {
            SynchronousCommand = new
            {
                _attributes = new { wcm_action = "add" },
                Order = order,
                Description = description,
                CommandLine = commandLine,
            }
        });
    }
}
