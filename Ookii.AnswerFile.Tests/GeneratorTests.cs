using Ookii.AnswerFile;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace GenerateAnswerFileTests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Directory.CreateDirectory(Path.Join(Path.GetDirectoryName(typeof(GeneratorTests).Assembly.Location), "actual"));
        }

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
                    DomainAccounts = { "domainuser2" }
                },
                AutoLogon = new AutoLogonOptions(new DomainUser("somedomain", "domainuser2"), "DomainPassword2")
                {
                    Count = 9999,
                },
                LocalAccounts = { new LocalCredential("MyAccount", "Password") },
                SetupScripts = { "\\\\machine\\shared\\script.ps1 -Arg" },
                ComputerName = "test-machine",
                EnableCloud = false,
                DisplayResolution = new Size(1280, 1024)
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
                        Components = { "Microsoft-Windows-Subsystem-Linux", "VirtualMachinePlatform" }
                    }
                },
                EnableRemoteDesktop = true,
                LocalAccounts = { new LocalCredential("MyUser", "Password") },
                AutoLogon = new AutoLogonOptions(new DomainUser(null, "MyUser"), "Password"),
                ProductKey = "ABCDE-12345-ABCDE-12345-ABCDE"
            };

            Generator.Generate(actualPath, options);
            CheckFilesEqual(expectedPath, actualPath);
        }

        private static void CheckFilesEqual(string expectedPath, string actualPath)
        {
            var expected = File.ReadAllText(expectedPath);
            var actual = File.ReadAllText(actualPath);
            Assert.AreEqual(expected, actual);
        }

        private static (string, string) GetPaths([CallerMemberName] string name = null!)
        {
            var basePath = Path.GetDirectoryName(typeof(GeneratorTests).Assembly.Location);
            var file = name + ".xml";
            return (Path.Join(basePath, "actual", file), Path.Join(basePath, "expected", file));
        }
    }
}