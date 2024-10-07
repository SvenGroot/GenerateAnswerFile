using System.Runtime.CompilerServices;

namespace Ookii.AnswerFile.Tests;

public abstract class FileTestsBase
{
    [TestInitialize]
    public void Initialize()
    {
        Directory.CreateDirectory(Path.Join(Path.GetDirectoryName(typeof(AnswerFileGeneratorTests).Assembly.Location), "actual"));
    }

    protected static void CheckFilesEqual(string expectedPath, string actualPath)
    {
        var expected = File.ReadAllText(expectedPath);
        var actual = File.ReadAllText(actualPath);
        Assert.AreEqual(expected, actual);
    }

    protected static (string, string) GetPaths(string extension = ".xml", [CallerMemberName] string name = null!)
    {
        var basePath = Path.GetDirectoryName(typeof(AnswerFileGeneratorTests).Assembly.Location);
        var file = name + extension;
        return (Path.Join(basePath, "actual", file), Path.Join(basePath, "expected", file));
    }
}
