using System.Data.Common;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Xml;

namespace GenerateAnswerFile;

public class Generator : IDisposable
{
    private readonly Arguments _arguments;
    private readonly XmlWriter _writer;

    private const string PublicKeyToken = "31bf3856ad364e35";

    public Generator(Arguments arguments)
    {
        _arguments = arguments;
        var settings = new XmlWriterSettings()
        {
            Indent = true,
            NamespaceHandling = NamespaceHandling.OmitDuplicates,
            Encoding = new UTF8Encoding(false),
        };

        _writer = XmlWriter.Create(_arguments.OutputFile.FullName, settings);
    }

    public static void Generate(Arguments arguments)
    {
        using var generator = new Generator(arguments);
        generator.Generate();
    }

    public void Generate()
    {
        _writer.WriteStartElement("unattend", "urn:schemas-microsoft-com:unattend");
        _writer.WriteAttributes(new
        {
            xmlns_wcm = "http://schemas.microsoft.com/WMIConfig/2002/State",
            xmlns_xsi = "http://www.w3.org/2001/XMLSchema-instance"
        });
        GenerateServicing();
        GenerateWindowsPePass();
        GenerateSpecializePass();
        GenerateOobePass();
        _writer.WriteEndElement();
    }

    public void Dispose()
    {
        _writer.Dispose();
    }

    private void GenerateServicing()
    {
        if (_arguments.Components == null)
        {
            return;
        }

        using var servicing = _writer.WriteAutoCloseElement("servicing");
        using var package = _writer.WriteAutoCloseElement("package", new { action = "configure" });
        _writer.WriteEmptyElement("assemblyIdentity", new
        {
            name = "Microsoft-Windows-Foundation-Package",
            version = _arguments.WindowsVersion,
            processorArchitecture = _arguments.ProcessorArchitecture,
            publicKeyToken = PublicKeyToken,
            language = ""
        });

        foreach (var component in _arguments.Components)
        {
            _writer.WriteEmptyElement("selection", new { name = component, state = "true" });
        }
    }

    private void GenerateWindowsPePass()
    {
        if (_arguments.Install == InstallMethod.PreInstalled)
        {
            return;
        }

        using var pass = WritePassStart("windowsPE");
        WriteInternationalCore(true);
        using var setup = WriteComponentStart("Microsoft-Windows-Setup");
        if (_arguments.Install != InstallMethod.Manual)
        {
            WriteInstallElements();
        }

        _writer.WriteElements(new
        {
            UserData = new
            {
                AcceptEula = "true",
                FullName = "",
                Organization = "",
                // This one chooses the edition
                ProductKey = _arguments.ProductKey == null ? null : new
                {
                    Key = _arguments.ProductKey,
                }
            }
        });
    }

    private void GenerateSpecializePass()
    {
        using var pass = WritePassStart("specialize");
        WriteInternationalCore();
        if (_arguments.JoinDomain != null)
        {
            using var join = WriteComponentStart("Microsoft-Windows-UnattendedJoin");
            _writer.WriteElements(new
            {
                Identification = new
                {
                    UnsecureJoin = "false",
                    Credentials = new
                    {
                        Domain = _arguments.JoinDomain,
                        Password = _arguments.JoinDomainPassword,
                        Username = _arguments.JoinDomainUser
                    },
                    _arguments.JoinDomain,
                    MachineObjectOU = _arguments.OUPath,
                }
            });
        }

        if (_arguments.ComputerName != null || _arguments.ProductKey != null)
        {
            using var shellSetup = WriteComponentStart("Microsoft-Windows-Shell-Setup");
            if (_arguments.ComputerName != null)
            {
                _writer.WriteElementString("ComputerName", _arguments.ComputerName);
            }

            if (_arguments.ProductKey != null)
            {
                // This one activates Windows.
                _writer.WriteElementString("ProduceKey", _arguments.ProductKey);
            }
        }

        if (_arguments.DisableDefender)
        {
            using var defender = WriteComponentStart("Security-Malware-Windows-Defender");
            _writer.WriteElementString("DisableAntiSpyware", "true");
        }

        if (_arguments.EnableRemoteDesktop)
        {
            using (var ts = WriteComponentStart("Microsoft-Windows-TerminalServices-LocalSessionManager"))
            {
                _writer.WriteElementString("fDenyTSConnections", "false");
            }

            using var firewall = WriteComponentStart("Networking-MPSSVC-Svc");
            _writer.WriteElements(new
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

        if (_arguments.DisableCloud)
        {
            using var deployment = WriteComponentStart("Microsoft-Windows-Deployment");
            using var runSynchronous = _writer.WriteAutoCloseElement("RunSynchronous");
            WriteRunSynchronousCommand(@"reg add HKLM\Software\Policies\Microsoft\Windows\CloudContent /v DisableWindowsConsumerFeatures /t REG_DWORD /d 1", "Disable cloud consumer features", 1);
        }
    }

    private void GenerateOobePass()
    {
        using var pass = WritePassStart("oobeSystem");
        WriteInternationalCore();
        using var shellSetup = WriteComponentStart("Microsoft-Windows-Shell-Setup");
        if (_arguments.LocalAccounts?.Length > 0 || (_arguments.JoinDomain != null && _arguments.DomainAccounts?.Length > 0))
        {
            using var userAccounts = _writer.WriteAutoCloseElement("UserAccounts");
            if (_arguments.LocalAccounts?.Length > 0)
            {
                using var localAccounts = _writer.WriteAutoCloseElement("LocalAccounts");
                foreach (var account in _arguments.LocalAccounts)
                {
                    _writer.WriteElements(new
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

            if (_arguments.JoinDomain != null && _arguments.DomainAccounts?.Length > 0)
            {
                using var domainAccounts = _writer.WriteAutoCloseElement("DomainAccounts");
                using var domainAccountList = _writer.WriteAutoCloseElement("DomainAccountList", new { wcm_action = "add" });
                foreach (var account in _arguments.DomainAccounts)
                {
                    _writer.WriteElements(new
                    {
                        DomainAccount = new
                        {
                            _attributes = new { wcm_action = "add" },
                            Name = account,
                            Group = "Administrators",
                        }
                    });
                }

                _writer.WriteElementString("Domain", _arguments.JoinDomain);
            }
        }

        _writer.WriteElements(new
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

        _writer.WriteElementString("TimeZone", _arguments.TimeZone);
        if (_arguments.AutoLogonUser != null)
        {
            _writer.WriteElements(new
            {
                AutoLogon = new
                {
                    Enabled = "true",
                    LogonCount = _arguments.AutoLogonCount,
                    _arguments.AutoLogonUser.Domain,
                    Username = _arguments.AutoLogonUser.UserName,
                    Password = new
                    {
                        Value = Convert.ToBase64String(Encoding.Unicode.GetBytes(_arguments.AutoLogonPassword + "Password")),
                        PlainText = "false"
                    }
                }
            });
        }

        using (var firstLogon = _writer.WriteAutoCloseElement("FirstLogonCommands"))
        {
            int order = 1;
            if (_arguments.CmdKeyUser != null)
            {
                WriteSynchronousCommand($"cmdkey.exe /add:* /user:{_arguments.CmdKeyUser.ToString()} /pass:{_arguments.CmdKeyPassword}", "CmdKey", order);
                ++order;
            }

            if (_arguments.SetupScripts?.Length > 0)
            {
                WriteSynchronousCommand("PowerShell.exe Set-ExecutionPolicy unrestricted -Force", "PowerShell unrestricted", order);
                ++order;
                WriteSynchronousCommand("PowerShell.exe Set-ExecutionPolicy bypass -Force", "PowerShell bypass", order);
                ++order;
                foreach (var script in _arguments.SetupScripts)
                {
                    WriteSynchronousCommand($"PowerShell.exe {script}", "Setup script", order);
                    ++order;
                }
            }
        }

        if (_arguments.DisplayResolution is Size resolution)
        {
            _writer.WriteElements(new
            {
                Display = new
                {
                    HorizontalResolution = resolution.Width,
                    VerticalResolution = resolution.Height,
                }
            });
        }
    }

    private void WriteInstallElements()
    {
        var installToPartition = _arguments.Install switch
        {
            InstallMethod.CleanEfi => 3,
            InstallMethod.CleanBios => 2,
            _ => _arguments.InstallToPartition
        };

        using (var imageInstall = _writer.WriteAutoCloseElement("ImageInstall"))
        using (var osImage = _writer.WriteAutoCloseElement("OSImage"))
        {
            if (_arguments.ImageIndex != 0)
            {
                _writer.WriteElements(new
                {
                    InstallFrom = new
                    {
                        MetaData = new
                        {
                            _attributes = new { wcm_action = "add" },
                            Key = "/IMAGE/INDEX",
                            Value = _arguments.ImageIndex
                        }
                    }
                });
            }

            _writer.WriteElements(new
            {
                InstallTo = new
                {
                    DiskID = _arguments.InstallToDisk,
                    PartitionID = installToPartition
                }
            });
        }

        using var diskConfiguration = _writer.WriteAutoCloseElement("DiskConfiguration");
        using var disk = _writer.WriteAutoCloseElement("Disk", new { wcm_action = "add" });
        _writer.WriteElementString("DiskID", _arguments.InstallToDisk.ToString());
        if (_arguments.Install != InstallMethod.ExistingPartition)
        {
            _writer.WriteElementString("WillWipeDisk", "true");
            using var createPartitions = _writer.WriteAutoCloseElement("CreatePartitions");
            switch (_arguments.Install)
            {
            case InstallMethod.CleanEfi:
                WriteCreatePartition(1, "EFI", 100);
                WriteCreatePartition(2, "MSR", 128);
                WriteCreatePartition(3, "Primary");
                break;

            case InstallMethod.CleanBios:
                WriteCreatePartition(1, "Primary", 100);
                WriteCreatePartition(2, "Primary");
                break;
            }
        }

        using var modifyPartitions = _writer.WriteAutoCloseElement("ModifyPartitions");
        switch (_arguments.Install)
        {
        case InstallMethod.CleanEfi:
            WriteModifyPartition(1, 1, "FAT32", "System");
            WriteModifyPartition(2, 2);
            WriteModifyPartition(3, 3, "NTFS", "Windows", 'C');
            break;

        case InstallMethod.CleanBios:
            WriteModifyPartition(1, 1, "NTFS", "System");
            WriteModifyPartition(2, 2, "NTFS", "Windows", 'C');
            break;

        case InstallMethod.ExistingPartition:
            WriteModifyPartition(1, _arguments.InstallToPartition, "NTFS", "Windows", 'C');
            break;
        }
    }

    private AutoCloseElement WriteComponentStart(string name)
    {
        return _writer.WriteAutoCloseElement("component", new
        {
            name,
            processorArchitecture = _arguments.ProcessorArchitecture,
            publicKeyToken = PublicKeyToken,
            language = "neutral",
            versionScope = "nonSxS",
        });
    }

    private AutoCloseElement WritePassStart(string pass)
    {
        return _writer.WriteAutoCloseElement("settings", new { pass });
    }

    private void WriteInternationalCore(bool winPe = false)
    {
        var name = "Microsoft-Windows-International-Core";
        if (winPe)
        {
            name += "-WinPE";
        }

        using var component = WriteComponentStart(name);
        _writer.WriteElements(new
        {
            InputLocale = _arguments.Language,
            SystemLocale = _arguments.Language,
            UILanguage = _arguments.Language,
            UserLocale = _arguments.Language
        });
    }

    private void WriteCreatePartition(int order, string type, int? size = null)
    {
        _writer.WriteElements(new
        {
            CreatePartition = new
            {
                _attributes = new { wcm_action = "add" },
                Order = order,
                Size = size,
                Type = type,
                extend = size.HasValue ? null : "true",
            }
        });
    }

    private void WriteModifyPartition(int order, int partitionId, string? format = null, string? label = null, char? letter = null)
    {
        _writer.WriteElements(new
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
        _writer.WriteElements(new
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
        _writer.WriteElements(new
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
