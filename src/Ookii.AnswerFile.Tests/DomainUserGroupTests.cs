namespace Ookii.AnswerFile.Tests;

[TestClass]
public class DomainUserGroupTests
{
    [TestMethod]
    public void TestConstructor()
    {
        var target = new DomainUserGroup(new DomainUser("foo", "bar"), "baz");
        Assert.AreEqual("foo", target.DomainUser.Domain);
        Assert.AreEqual("bar", target.DomainUser.UserName);
        Assert.AreEqual("baz", target.Group);

        target = new DomainUserGroup(new DomainUser("foo", "bar"));
        Assert.AreEqual("foo", target.DomainUser.Domain);
        Assert.AreEqual("bar", target.DomainUser.UserName);
        Assert.AreEqual("Administrators", target.Group);
    }

    [TestMethod]
    public void TestToString()
    {
        var target = new DomainUserGroup(new DomainUser("foo", "bar"), "baz");
        Assert.AreEqual("baz:foo\\bar", target.ToString());
        target = new DomainUserGroup(new DomainUser("bar"), "baz");
        Assert.AreEqual("baz:bar", target.ToString());
    }

    [TestMethod]
    public void TestParse()
    {
        var target = DomainUserGroup.Parse("baz:foo\\bar");
        Assert.AreEqual("foo", target.DomainUser.Domain);
        Assert.AreEqual("bar", target.DomainUser.UserName);
        Assert.AreEqual("baz", target.Group);

        target = DomainUserGroup.Parse("baz:bar");
        Assert.IsNull(target.DomainUser.Domain);
        Assert.AreEqual("bar", target.DomainUser.UserName);
        Assert.AreEqual("baz", target.Group);

        target = DomainUserGroup.Parse("bar\\foo");
        Assert.AreEqual("bar", target.DomainUser.Domain);
        Assert.AreEqual("foo", target.DomainUser.UserName);
        Assert.AreEqual("Administrators", target.Group);

        target = DomainUserGroup.Parse("foo");
        Assert.IsNull(target.DomainUser.Domain);
        Assert.AreEqual("foo", target.DomainUser.UserName);
        Assert.AreEqual("Administrators", target.Group);
    }
}
