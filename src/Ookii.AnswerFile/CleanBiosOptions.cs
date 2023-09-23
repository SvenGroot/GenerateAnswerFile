namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for a clean installation on BIOS-based systems.
/// </summary>
/// <remarks>
/// <para>
///   When using this installation method, the disk specified by <see cref="TargetedInstallOptionsBase.DiskId"/>
///   will be wiped, with partitions created according to the <see cref="CleanOptionsBase.Partitions"/>
///   property.
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
    protected override string SystemPartitionType => "Primary";

    /// <summary>
    /// Gets the file system to use for the system partition.
    /// </summary>
    /// <value>
    /// The file system type "NTFS".
    /// </value>
    protected override string SystemPartitionFileSystem => "NTFS";

    /// <summary>
    /// Gets a value which indicates whether an extended partition should be used if there are more
    /// than 4 partitions.
    /// </summary>
    /// <value>
    /// <see langword="true"/>.
    /// </value>
    protected override bool UseExtendedPartition => true;

    /// <summary>
    /// Gets the type ID that marks a partition as a utility partition.
    /// </summary>
    /// <value>
    /// The partition type ID.
    /// </value>
    protected override string UtilityTypeId => "0x27";

    /// <summary>
    /// Gets the partition layout to use if the <see cref="CleanOptionsBase.Partitions"/> property
    /// is an empty list.
    /// </summary>
    /// <returns>A list containing the default BIOS partition layout.</returns>
    protected override IList<Partition> GetDefaultPartitions()
    {
        return new[]
        {
            new Partition() { Type = PartitionType.System, Label = "System", Size = BinarySize.FromMebi(100) },
            new Partition() { Label = "Windows" },
        };
    }
}
