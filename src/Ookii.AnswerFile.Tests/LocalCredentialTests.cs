using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ookii.AnswerFile.Tests;

[TestClass]
public class LocalCredentialTests
{
    [TestMethod]
    public void TestConstructor()
    {
        var target = new LocalCredential("foo", "bar", "baz");
        Assert.AreEqual("foo", target.UserName);
        Assert.AreEqual("bar", target.Password);
        Assert.AreEqual("baz", target.Group);

        target = new LocalCredential("foo", "bar");
        Assert.AreEqual("foo", target.UserName);
        Assert.AreEqual("bar", target.Password);
        Assert.AreEqual("Administrators", target.Group);
    }

    [TestMethod]
    public void TestParse()
    {
        var target = LocalCredential.Parse("baz:foo,bar");
        Assert.AreEqual("foo", target.UserName);
        Assert.AreEqual("bar", target.Password);
        Assert.AreEqual("baz", target.Group);

        target = LocalCredential.Parse("foo,bar");
        Assert.AreEqual("foo", target.UserName);
        Assert.AreEqual("bar", target.Password);
        Assert.AreEqual("Administrators", target.Group);
    }
}
