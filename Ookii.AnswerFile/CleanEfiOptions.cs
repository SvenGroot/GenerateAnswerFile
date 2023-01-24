namespace Ookii.AnswerFile;

public class CleanEfiOptions : TargetedInstallOptionsBase
{
    protected override int TargetPartitionId => 3;

    protected override void WriteDiskConfiguration(Generator generator)
    {
        generator.Writer.WriteElementString("WillWipeDisk", "true");
        using (var createPartitions = generator.Writer.WriteAutoCloseElement("CreatePartitions"))
        {
            generator.WriteCreatePartition(1, "EFI", 100);
            generator.WriteCreatePartition(2, "MSR", 128);
            generator.WriteCreatePartition(3, "Primary");
        }

        using var modifyPartitions = generator.Writer.WriteAutoCloseElement("ModifyPartitions");
        generator.WriteModifyPartition(1, 1, "FAT32", "System");
        generator.WriteModifyPartition(2, 2);
        generator.WriteModifyPartition(3, 3, "NTFS", "Windows", 'C');
    }
}
