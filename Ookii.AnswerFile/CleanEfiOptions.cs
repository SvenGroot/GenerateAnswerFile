namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for a clean installation on EFI or UEFI-based systems.
/// </summary>
/// <remarks>
/// When using this installation method, the disk specified by <see
/// cref="TargetedInstallOptionsBase.DiskId"/> will be wiped, and two partitions will be created: a
/// 100MB EFI partition, a 128MB MSR partition, and an OS partition with the remaining size of the
/// disk. Windows will be installed on the third partition.
/// </remarks>
public class CleanEfiOptions : TargetedInstallOptionsBase
{
    /// <summary>
    /// Gets the ID of the partition to install to.
    /// </summary>
    /// <value>
    /// This property returns 3 for clean EFI-based installations.
    /// </value>
    protected override int TargetPartitionId => 3;

    /// <summary>
    /// Writes the disk configuration for this installation method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
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
