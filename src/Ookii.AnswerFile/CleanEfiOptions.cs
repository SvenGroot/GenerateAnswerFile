using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for a clean installation on UEFI-based systems.
/// </summary>
/// <remarks>
/// <para>
///   When using this installation method, the disk specified by <see cref="TargetedInstallOptionsBase.DiskId"/>
///   will be wiped, and repartitioned as a GPT disk with partitions created according to the
///   <see cref="CleanOptionsBase.Partitions"/> property.
/// </para>
/// <para>
///   If the <see cref="CleanOptionsBase.Partitions"/> property is an empty list, the default layout
///   will be used: a 100MB EFI partition, a 128MB MSR partition, and an OS partition with the
///   remaining size of the disk. Windows will be installed on the third partition.
/// </para>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public class CleanEfiOptions : CleanOptionsBase
{
    /// <summary>
    /// Gets the partition type to use for the system partition.
    /// </summary>
    /// <value>
    /// The value "EFI".
    /// </value>
    /// <remarks>
    /// <para>
    ///   The system partition for UEFI systems is an EFI System Partition (ESP), which must be
    ///   formatted as FAT32 because it is accessed by the system firmware.
    /// </para>
    /// </remarks>
    protected override string SystemPartitionType => "EFI";

    /// <summary>
    /// Gets the file system to use for the system partition.
    /// </summary>
    /// <value>
    /// The value "FAT32".
    /// </value>
    /// <remarks>
    /// <para>
    ///   The system partition for UEFI systems is an EFI System Partition (ESP), which must be
    ///   formatted as FAT32 because it is accessed by the system firmware.
    /// </para>
    /// </remarks>
    protected override string SystemPartitionFileSystem => "FAT32";

    /// <summary>
    /// Gets a value which indicates whether an extended partition should be used if there are more
    /// than 4 partitions.
    /// </summary>
    /// <value>
    /// <see langword="false"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   UEFI systems use GPT, which has a 128 partition limit, so an extended partition with
    ///   logical volumes is not required for more than four partitions. Creating more than 128
    ///   partitions is not supported.
    /// </para>
    /// </remarks>
    protected override bool UseExtendedPartition => false;

    /// <summary>
    /// Gets the type ID that marks a partition as a utility partition.
    /// </summary>
    /// <value>
    /// The GPT partition type ID for utility partitions, which is "de94bba4-06d1-4d40-a16a-bfd50179d6ac".
    /// </value>
    protected override string UtilityTypeId => "de94bba4-06d1-4d40-a16a-bfd50179d6ac";

    /// <summary>
    /// Gets the partition layout to use if the <see cref="CleanOptionsBase.Partitions"/> property
    /// is an empty list.
    /// </summary>
    /// <returns>
    /// A list containing the default EFI partition layout: a 100MB EFI system partition, a 128MB
    /// MSR partition, and an OS partition with the remaining size of the disk.
    /// </returns>
    protected override IList<Partition> GetDefaultPartitions()
    {
        return new[]
        {
            new Partition() { Type = PartitionType.System, Size = BinarySize.FromMebi(100), Label = "System" },
            new Partition() { Type = PartitionType.Msr, Size = BinarySize.FromMebi(128) },
            new Partition() { Label = "Windows" }
        };
    }
}
