namespace Ookii.AnswerFile;

public class CleanBiosOptions : TargetedInstallOptionsBase
{
    protected override int TargetPartitionId => 2;

    protected override void WriteDiskConfiguration(Generator generator)
    {
        generator.Writer.WriteElementString("WillWipeDisk", "true");
        using (var createPartitions = generator.Writer.WriteAutoCloseElement("CreatePartitions"))
        {
            generator.WriteCreatePartition(1, "Primary", 100);
            generator.WriteCreatePartition(2, "Primary");
        }

        using var modifyPartitions = generator.Writer.WriteAutoCloseElement("ModifyPartitions");
        generator.WriteModifyPartition(1, 1, "NTFS", "System");
        generator.WriteModifyPartition(2, 2, "NTFS", "Windows", 'C');
    }
}
