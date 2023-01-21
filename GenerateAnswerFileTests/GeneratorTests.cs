using GenerateAnswerFile;
using Ookii.CommandLine;
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
            string[] args = new[] { actualPath, "-ComputerName", "test-machine", "-WindowsVersion", "10.0.22000.1", "-LocalAccount", "MyAccount,Password", "-AutoLogonCount", "9999", "-DisableCloud", "-SetupScript", "\\\\machine\\shared\\script.ps1 -Arg", "-JoinDomain", "somedomain", "-JoinDomainUser", "domainuser", "-JoinDomainPassword", "DomainPassword", "-DomainAccount", "domainuser2", "-AutoLogonUser", "somedomain\\domainuser2", "-AutoLogonPassword", "DomainPassword2", "-OUPath", "OU=SomeOU,DC=somedomain", "-DisplayResolution", "1280,1024" };
            var arguments = CommandLineParser.Parse<Arguments>(args);
            Assert.IsNotNull(arguments);
            Generator.Generate(arguments);
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