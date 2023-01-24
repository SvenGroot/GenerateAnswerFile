namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for a clean installation on BIOS-based systems.
/// </summary>
/// <remarks>
/// When using this installation method, the disk specified by <see cref="TargetedInstallOptionsBase.DiskId"/>
/// will be wiped, and two partitions will be created: a 100MB system partition, and an OS
/// partition with the remaining size of the disk. Windows will be installed on the second
/// partition.
/// </remarks>
public class CleanBiosOptions : TargetedInstallOptionsBase
{
    /// <summary>
    /// Gets the ID of the partition to install to.
    /// </summary>
    /// <value>
    /// This property returns 2 for clean BIOS-based installations.
    /// </value>
    protected override int TargetPartitionId => 2;

    /// <summary>
    /// Writes the disk configuration for this installation method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
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
