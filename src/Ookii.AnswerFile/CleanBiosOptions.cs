namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for a clean installation on BIOS-based systems.
/// </summary>
/// <remarks>
/// <para>
///   When using this installation method, the disk specified by <see cref="TargetedInstallOptionsBase.DiskId"/>
///   will be wiped, and repartitioned as an MBR disk with partitions created according to the
///   <see cref="CleanOptionsBase.Partitions"/> property.
/// </para>
/// <para>
///   If the <see cref="CleanOptionsBase.Partitions"/> property is an empty list, the default layout
///   will be used: a 100MB system partition, and an OS partition with the remaining size of the
///   disk. Windows will be installed on the second partition.
/// </para>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public class CleanBiosOptions : CleanOptionsBase
{
    /// <summary>
    /// Gets the partition type to use for the system partition.
    /// </summary>
    /// <value>
    /// The value "Primary".
    /// </value>
    /// <remarks>
    /// <para>
    ///   The system partition for BIOS systems is a regular primary partition that holds the
    ///   Windows boot manager. This partition should be formatted using the NTFS file system.
    /// </para>
    /// </remarks>
    protected override string SystemPartitionType => "Primary";

    /// <summary>
    /// Gets the file system to use for the system partition.
    /// </summary>
    /// <value>
    /// The value "NTFS".
    /// </value>
    /// <remarks>
    /// <para>
    ///   The system partition for BIOS systems is a regular primary partition that holds the
    ///   Windows boot manager. This partition should be formatted using the NTFS file system.
    /// </para>
    /// </remarks>
    protected override string SystemPartitionFileSystem => "NTFS";

    /// <summary>
    /// Gets a value which indicates whether an extended partition should be used if there are more
    /// than 4 partitions.
    /// </summary>
    /// <value>
    /// <see langword="true"/>.
    /// </value>
    /// <remarks>
    /// <para>
    ///   BIOS systems use MBR, which has a four partition limit, requiring the use of an extended
    ///   partition containing logical volumes if more volumes are desired.
    /// </para>
    /// </remarks>
    protected override bool UseExtendedPartition => true;

    /// <summary>
    /// Gets the type ID that marks a partition as a utility partition.
    /// </summary>
    /// <value>
    /// The MBR partition type ID for utility partitions, which is "0x27".
    /// </value>
    protected override string UtilityTypeId => "0x27";

    /// <summary>
    /// Gets the partition layout to use if the <see cref="CleanOptionsBase.Partitions"/> property
    /// is an empty list.
    /// </summary>
    /// <returns>
    /// A list containing the default BIOS partition layout: a 100MB system partition, and an OS
    /// partition with the remaining size of the disk.
    /// </returns>
    protected override IList<Partition> GetDefaultPartitions()
    {
        return new[]
        {
            new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromMebi(100) },
            new Partition() { Label = "Windows" },
        };
    }
}
