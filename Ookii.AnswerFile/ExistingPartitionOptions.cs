namespace Ookii.AnswerFile;

public class ExistingPartitionOptions : TargetedInstallOptionsBase
{
    public int PartitionId { get; set; }

    protected override int TargetPartitionId => PartitionId;

    protected override void WriteDiskConfiguration(Generator generator)
    {
        using var modifyPartitions = generator.Writer.WriteAutoCloseElement("ModifyPartitions");
        generator.WriteModifyPartition(1, PartitionId, "NTFS", "Windows", 'C');
    }
}
