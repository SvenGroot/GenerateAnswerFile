using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for a clean installation on EFI or UEFI-based systems.
/// </summary>
/// <remarks>
/// <para>
///   When using this installation method, the disk specified by <see cref="TargetedInstallOptionsBase.DiskId"/>
///   will be wiped, with partitions created according to the <see cref="CleanOptionsBase.Partitions"/>
///   property.
/// </para>
/// <para>
///   If the <see cref="CleanOptionsBase.Partitions"/> property is an empty list, the default layout
///   will be used: a 100MB EFI partition, a 128MB MSR partition, and an OS partition with the
///   remaining size of the disk. Windows will be installed on the third partition.
/// </para>
/// </remarks>
public class CleanEfiOptions : CleanOptionsBase
{
    /// <summary>
    /// Gets the partition type to use for the system partition.
    /// </summary>
    /// <value>
    /// The value "EFI".
    /// </value>
    protected override string SystemPartitionType => "EFI";

    /// <summary>
    /// Gets the file system to use for the system partition.
    /// </summary>
    /// <value>
    /// The file system type "FAT32".
    /// </value>
    protected override string SystemPartitionFileSystem => "FAT32";

    /// <summary>
    /// Gets a value which indicates whether an extended partition should be used if there are more
    /// than 4 partitions.
    /// </summary>
    /// <value>
    /// <see langword="false"/>.
    /// </value>
    protected override bool UseExtendedPartition => false;

    /// <summary>
    /// Gets the type ID that marks a partition as a utility partition.
    /// </summary>
    /// <value>
    /// The partition type ID.
    /// </value>
    protected override string UtilityTypeId => "de94bba4-06d1-4d40-a16a-bfd50179d6ac";

    /// <summary>
    /// Gets the partition layout to use if the <see cref="CleanOptionsBase.Partitions"/> property is an empty list.
    /// </summary>
    /// <returns>A list containing the default EFI partition layout.</returns>
    protected override List<Partition> GetDefaultPartitions()
    {
        return new List<Partition>
        {
            new Partition() { Type = PartitionType.System, Size = BinarySize.FromMebi(100), Label = "System" },
            new Partition() { Type = PartitionType.Msr, Size = BinarySize.FromMebi(128) },
            new Partition() { Label = "Windows" }
        };
    }
}
