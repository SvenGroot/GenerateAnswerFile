namespace Ookii.AnswerFile;

/// <summary>
/// Specifies a partition to be created when using the <see cref="CleanBiosOptions"/> or
/// <see cref="CleanEfiOptions"/> class.
/// </summary>
public class Partition
{
    /// <summary>
    /// Gets the type of the partition.
    /// </summary>
    /// <value>
    /// One of the values of the <see cref="PartitionType"/> enumeration.
    /// </value>
    public PartitionType Type { get; set; }

    /// <summary>
    /// Gets the label of the partition.
    /// </summary>
    /// <value>
    /// The label, or <see langword="null"/> to not use a label.
    /// </value>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the size of the partition.
    /// </summary>
    /// <value>
    /// The size of the partition, or <see langword="null"/> to extend the partition to fill the
    /// remaining space. The default value is <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// Only the last partition to create should use <see langword="null"/> as the value.
    /// </remarks>
    public BinarySize? Size { get; set; }

    /// <summary>
    /// Gets or sets the file system to format the partition with.
    /// </summary>
    /// <value>
    /// The name of a supported file system, such as "FAT32" or "NTFS", or <see langword="null"/> to
    /// use the default format for the <see cref="Type"/>.
    /// </value>
    public string? FileSystem { get; set; }
}
