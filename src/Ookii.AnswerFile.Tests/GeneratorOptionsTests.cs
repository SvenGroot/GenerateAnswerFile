using System.Drawing;
using System.Text.Json;

namespace Ookii.AnswerFile.Tests;

[TestClass]
public class GeneratorOptionsTests : FileTestsBase
{
    [TestMethod]
    public void TestJsonSerializationPreInstalled()
    {
        var (actualPath, expectedPath) = GetPaths(".json");
        var options = new GeneratorOptions()
        {
            AutoLogon = new AutoLogonOptions(new DomainUser("domain", "user"), "password"),
            JoinDomain = new DomainOptions("domain", new DomainCredential(new DomainUser("domain", "user"), "password"))
            {
                OUPath = "OU=Computers",
                DomainAccounts =
                {
                    new(new("user")),
                    new(new("domain", "user2"), "Users"),
                }
            },
            CmdKeyAccount = new DomainCredential(new DomainUser("domain", "user"), "password"),
            ComputerName = "TestComputer",
            DisplayResolution = new(1280, 1024),
            EnableCloud = true,
            EnableDefender = false,
            LocalAccounts =
            {
                new LocalCredential("localuser", "password"),
                new LocalCredential("localuser2", "password2"),
                new LocalCredential("localuser3", "password3", "Users"),
            },
            FirstLogonCommands = { "Hello", "Bye" },
            SetupScripts = { "Foo", "Bar" },
        };

        var json = options.ToJson();
        File.WriteAllText(actualPath, json);
        CheckFilesEqual(expectedPath, actualPath);
        var deserialized = GeneratorOptions.FromJson(json);
        Assert.IsNotNull(deserialized);
        Assert.IsNull(deserialized.InstallOptions);
        Assert.IsNotNull(deserialized.AutoLogon);
        Assert.AreEqual(options.AutoLogon.Credential, deserialized.AutoLogon.Credential);
        Assert.IsNotNull(deserialized.JoinDomain);
        Assert.AreEqual(((DomainOptions)options.JoinDomain).Credential, ((DomainOptions)deserialized.JoinDomain).Credential);
        Assert.AreEqual(((DomainOptions)options.JoinDomain).Domain, ((DomainOptions)deserialized.JoinDomain).Domain);
        Assert.AreEqual(((DomainOptions)options.JoinDomain).OUPath, ((DomainOptions)deserialized.JoinDomain).OUPath);
        CollectionAssert.AreEqual(((DomainOptions)options.JoinDomain).DomainAccounts, ((DomainOptions)deserialized.JoinDomain).DomainAccounts);
        CollectionAssert.AreEqual(options.LocalAccounts, deserialized.LocalAccounts);
        Assert.AreEqual(options.DisplayResolution, deserialized.DisplayResolution);
        Assert.IsTrue(deserialized.EnableCloud);
        Assert.IsFalse(deserialized.EnableDefender);
        CollectionAssert.AreEqual(options.FirstLogonCommands, deserialized.FirstLogonCommands);
        CollectionAssert.AreEqual(options.SetupScripts, deserialized.SetupScripts);
    }

    [TestMethod]
    public void TestJsonSerializationEfi()
    {
        var (actualPath, expectedPath) = GetPaths(".json");
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanEfiOptions()
            {
                CustomTargetPartitionId = 5,
                DiskId = 3,
                OptionalFeatures = new OptionalFeatures(new Version(10, 0, 22000, 1))
                {
                    Features = { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }
                },
                Partitions =
                {
                    new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromGibi(128) },
                    new Partition() { Label = "Windows" }
                },
            },
            JoinDomain = new ProvisionedDomainOptions("base64-data-goes-here"),
            ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE",
        };

        var json = options.ToJson();
        File.WriteAllText(actualPath, json);
        CheckFilesEqual(expectedPath, actualPath);
        var deserialized = GeneratorOptions.FromJson(json);
        Assert.IsNotNull(deserialized);
        Assert.IsNotNull(deserialized.InstallOptions);
        var install = (CleanEfiOptions)deserialized.InstallOptions;
        Assert.AreEqual(5, install.CustomTargetPartitionId);
        Assert.AreEqual(3, install.DiskId);
        Assert.AreEqual("ABCDE-12345-ABCDE-12345-ABCDE", options.ProductKey);
        Assert.IsNotNull(install.OptionalFeatures);
        Assert.AreEqual(new Version(10, 0, 22000, 1), install.OptionalFeatures.WindowsVersion);
        CollectionAssert.AreEqual(new[] { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }, install.OptionalFeatures.Features);
    }

    [TestMethod]
    public void TestJsonSerializationBios()
    {
        var (actualPath, expectedPath) = GetPaths(".json");
        var options = new GeneratorOptions()
        {
            InstallOptions = new CleanBiosOptions()
            {
                CustomTargetPartitionId = 5,
                DiskId = 3,
                ImageIndex = 2,
                OptionalFeatures = new OptionalFeatures(new Version(10, 0, 22000, 1))
                {
                    Features = { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }
                },
                Partitions =
                {
                    new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromGibi(128) },
                    new Partition() { Label = "Windows" }
                }
            },
        };

        var json = options.ToJson();
        File.WriteAllText(actualPath, json);
        CheckFilesEqual(expectedPath, actualPath);
        var deserialized = GeneratorOptions.FromJson(json);
        Assert.IsNotNull(deserialized);
        Assert.IsNotNull(deserialized.InstallOptions);
        var install = (CleanBiosOptions)deserialized.InstallOptions;
        Assert.AreEqual(5, install.CustomTargetPartitionId);
        Assert.AreEqual(3, install.DiskId);
        Assert.AreEqual(2, install.ImageIndex);
        Assert.IsNotNull(install.OptionalFeatures);
        Assert.AreEqual(new Version(10, 0, 22000, 1), install.OptionalFeatures.WindowsVersion);
        CollectionAssert.AreEqual(new[] { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }, install.OptionalFeatures.Features);
    }

    [TestMethod]
    public void TestJsonSerializationExisting()
    {
        var (actualPath, expectedPath) = GetPaths(".json");
        var options = new GeneratorOptions()
        {
            InstallOptions = new ExistingPartitionOptions()
            {
                PartitionId = 5,
                DiskId = 3,
                ImageIndex = 2,
                OptionalFeatures = new OptionalFeatures(new Version(10, 0, 22000, 1))
                {
                    Features = { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }
                },
            },
        };

        var json = options.ToJson();
        File.WriteAllText(actualPath, json);
        CheckFilesEqual(expectedPath, actualPath);
        var deserialized = GeneratorOptions.FromJson(json);
        Assert.IsNotNull(deserialized);
        Assert.IsNotNull(deserialized.InstallOptions);
        var install = (ExistingPartitionOptions)deserialized.InstallOptions;
        Assert.AreEqual(5, install.PartitionId);
        Assert.AreEqual(3, install.DiskId);
        Assert.AreEqual(2, install.ImageIndex);
        Assert.IsNotNull(install.OptionalFeatures);
        Assert.AreEqual(new Version(10, 0, 22000, 1), install.OptionalFeatures.WindowsVersion);
        CollectionAssert.AreEqual(new[] { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }, install.OptionalFeatures.Features);
    }

    [TestMethod]
    public void TestJsonSerializationManual()
    {
        var (actualPath, expectedPath) = GetPaths(".json");
        var options = new GeneratorOptions()
        {
            InstallOptions = new ManualInstallOptions()
            {
                OptionalFeatures = new OptionalFeatures(new Version(10, 0, 22000, 1))
                {
                    Features = { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }
                },
            },
        };

        var json = options.ToJson();
        File.WriteAllText(actualPath, json);
        CheckFilesEqual(expectedPath, actualPath);
        var deserialized = GeneratorOptions.FromJson(json);
        Assert.IsNotNull(deserialized);
        Assert.IsNotNull(deserialized.InstallOptions);
        var install = (ManualInstallOptions)deserialized.InstallOptions;
        Assert.IsNotNull(install.OptionalFeatures);
        Assert.AreEqual(new Version(10, 0, 22000, 1), install.OptionalFeatures.WindowsVersion);
        CollectionAssert.AreEqual(new[] { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }, install.OptionalFeatures.Features);
    }
}
