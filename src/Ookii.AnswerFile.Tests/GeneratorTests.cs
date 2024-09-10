using System.Drawing;
using System.Runtime.CompilerServices;

namespace Ookii.AnswerFile.Tests;

[TestClass]
public class GeneratorTests : FileTestsBase
{
    [TestMethod]
    public void TestGeneratePreInstalled()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            JoinDomain = new DomainOptions("somedomain",
                new DomainCredential(new DomainUser("somedomain", "domainuser"), "DomainPassword"))
            {
                OUPath = "OU=SomeOU,DC=somedomain",
                DomainAccounts = { new(new("domainuser2")), new(new("somedomain2", "domainuser4")),
                    new(new("somedomain", "domainuser3")), new(new("somedomain2", "domainuser5"), "Users") }
            },
            AutoLogon = new AutoLogonOptions(new DomainUser("somedomain", "domainuser2"), "DomainPassword2")
            {
                Count = 9999,
            },
            LocalAccounts =
            { 
                new LocalCredential("MyAccount", "Password"),
                new LocalCredential("MyAccount2", "Password2", "Users")
            },
            FirstLogonCommands = { "command1.exe", "command2.exe foo" },
            SetupScripts = { "\\\\machine\\shared\\script.ps1 -Arg" },
            ComputerName = "test-machine",
            EnableCloud = false,
            DisplayResolution = new(1280, 1024)
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateCleanEfi()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanEfiOptions()
            {
                OptionalFeatures = new OptionalFeatures(new Version(10, 0, 22621, 1))
                {
                    Features = { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }
                }
            },
            JoinDomain = new ProvisionedDomainOptions("base64-data-goes-here"),
            EnableRemoteDesktop = true,
            EnableServerManager = false,
            LocalAccounts = { new LocalCredential("MyUser", "Password") },
            AutoLogon = new AutoLogonOptions(new DomainUser(null, "MyUser"), "Password"),
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE"
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateCleanEfiCustomPartitions()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanEfiOptions()
            {
                Partitions =
                {
                    new Partition() { Type = PartitionType.Utility, Label = "WinRE", Size = BinarySize.FromMebi(350) },
                    new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromMebi(100) },
                    new Partition() { Type = PartitionType.Msr, Size = BinarySize.FromMebi(128) },
                    new Partition() { Type = PartitionType.Normal, Label = "Windows", Size = BinarySize.FromGibi(64) },
                    new Partition() { Type = PartitionType.Normal, Label = "Data", FileSystem = "FAT32" },
                }
            },
            LocalAccounts = { new LocalCredential("MyUser", "Password") },
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE"
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateCleanEfiCustomTargetPartition()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanEfiOptions()
            {
                Partitions =
                {
                    new Partition() { Type = PartitionType.Utility, Label = "WinRE", Size = BinarySize.FromMebi(350) },
                    new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromMebi(100) },
                    new Partition() { Type = PartitionType.Msr, Size = BinarySize.FromMebi(128) },
                    new Partition() { Type = PartitionType.Normal, Label = "Windows", Size = BinarySize.FromGibi(64) },
                    new Partition() { Type = PartitionType.Normal, Label = "Data", FileSystem = "FAT32" },
                },
                CustomTargetPartitionId = 5
            },
            LocalAccounts = { new LocalCredential("MyUser", "Password") },
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE"
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateCleanBios()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanBiosOptions()
            {
                DiskId = 1,
                ImageIndex = 2,
            },
            EnableDefender = false,
            LocalAccounts = { new LocalCredential("MyUser", "Password") },
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE",
            ProcessorArchitecture = "x86"
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateCleanBiosCustomPartitions()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanBiosOptions()
            {
                Partitions =
                {
                    new Partition() { Type = PartitionType.Utility, Label = "WinRE", Size = BinarySize.FromMebi(350) },
                    new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromMebi(100) },
                    new Partition() { Type = PartitionType.Normal, Label = "Windows", Size = BinarySize.FromGibi(64) },
                    new Partition() { Type = PartitionType.Normal, Label = "Data", FileSystem = "FAT32" },
                },
            },
            LocalAccounts = { new LocalCredential("MyUser", "Password") },
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE",
            ProcessorArchitecture = "x86"
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateCleanBiosExtendedPartition()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanBiosOptions()
            {
                Partitions =
                {
                    new Partition() { Type = PartitionType.Utility, Label = "WinRE", Size = BinarySize.FromMebi(350) },
                    new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromMebi(100) },
                    new Partition() { Type = PartitionType.Normal, Label = "Windows", Size = BinarySize.FromGibi(64) },
                    new Partition() { Type = PartitionType.Normal, Label = "Data", FileSystem = "FAT32", Size = BinarySize.FromGibi(10) },
                    new Partition() { Type = PartitionType.Normal, Label = "Data 2", FileSystem = "NTFS" },
                },
            },
            LocalAccounts = { new LocalCredential("MyUser", "Password") },
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE",
            ProcessorArchitecture = "x86"
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateExistingPartition()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new ExistingPartitionOptions()
            {
                DiskId = 1,
                PartitionId = 5,
            },
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE",
            TimeZone = "UTC"
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateManual()
    {
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            InstallOptions = new ManualInstallOptions(),
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }

    [TestMethod]
    public void TestGenerateDomainOnly()
    {
        // Make sure account screens are hidden in OOBE if joining a domain while not creating
        // local accounts.
        var (actualPath, expectedPath) = GetPaths();
        var options = new GeneratorOptions()
        {
            JoinDomain = new DomainOptions("somedomain",
                new DomainCredential(new DomainUser("somedomain", "domainuser"), "DomainPassword"))
            {
                OUPath = "OU=SomeOU,DC=somedomain",
            },
            ComputerName = "test-machine",
        };

        Generator.Generate(actualPath, options);
        CheckFilesEqual(expectedPath, actualPath);
    }
}
