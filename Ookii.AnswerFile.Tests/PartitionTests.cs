using System.Globalization;

namespace Ookii.AnswerFile.Tests;

[TestClass]
public class PartitionTests
{
    [TestMethod]
    public void TestParse()
    {
        var partition = Partition.Parse("128GB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Normal, partition.Type);
        Assert.IsNull(partition.Label);
        Assert.AreEqual(BinarySize.FromGibi(128), partition.Size);
        Assert.IsNull(partition.FileSystem);

        partition = Partition.Parse(":128GB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Normal, partition.Type);
        Assert.AreEqual("", partition.Label);
        Assert.AreEqual(BinarySize.FromGibi(128), partition.Size);
        Assert.IsNull(partition.FileSystem);

        partition = Partition.Parse("Foo:Bar:128GB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Normal, partition.Type);
        Assert.AreEqual("Foo:Bar", partition.Label);
        Assert.AreEqual(BinarySize.FromGibi(128), partition.Size);
        Assert.IsNull(partition.FileSystem);

        partition = Partition.Parse("Foo[Bar:64GB[FAT32]", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Normal, partition.Type);
        Assert.AreEqual("Foo[Bar", partition.Label);
        Assert.AreEqual(BinarySize.FromGibi(64), partition.Size);
        Assert.AreEqual("FAT32", partition.FileSystem);
    }

    [TestMethod]
    public void TestParseSpecialPartition()
    {
        var partition = Partition.Parse("System:100MB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.System, partition.Type);
        Assert.AreEqual("System", partition.Label);
        Assert.AreEqual(BinarySize.FromMebi(100), partition.Size);
        Assert.IsNull(partition.FileSystem);

        partition = Partition.Parse("winre:128MB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Utility, partition.Type);
        Assert.AreEqual("winre", partition.Label);
        Assert.AreEqual(BinarySize.FromMebi(128), partition.Size);
        Assert.IsNull(partition.FileSystem);

        partition = Partition.Parse("WINRE:128MB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Utility, partition.Type);
        Assert.AreEqual("WINRE", partition.Label);
        Assert.AreEqual(BinarySize.FromMebi(128), partition.Size);
        Assert.IsNull(partition.FileSystem);

        partition = Partition.Parse("recovery:128MB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Utility, partition.Type);
        Assert.AreEqual("recovery", partition.Label);
        Assert.AreEqual(BinarySize.FromMebi(128), partition.Size);
        Assert.IsNull(partition.FileSystem);

        partition = Partition.Parse("MSR:128MB", CultureInfo.InvariantCulture);
        Assert.AreEqual(PartitionType.Msr, partition.Type);
        Assert.IsNull(partition.Label);
        Assert.AreEqual(BinarySize.FromMebi(128), partition.Size);
        Assert.IsNull(partition.FileSystem);
    }
}
