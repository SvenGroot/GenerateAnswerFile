using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

/// <summary>
/// Indicates the type of a partition.
/// </summary>
/// <seealso href="https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-setup-diskconfiguration-disk-createpartitions-createpartition-type">Partition Type in answer files.</seealso>
/// <seealse href="https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-setup-diskconfiguration-disk-modifypartitions-modifypartition-typeid">Partition TypeId in answer files.</seealse>
[JsonConverter(typeof(JsonStringEnumConverter<PartitionType>))]
public enum PartitionType
{
    /// <summary>
    /// The partition is a normal data partition. For EFI/GPT, these are always primary partitions.
    /// For BIOS/MBR, an extended partition with logical volumes is created automatically based on
    /// the partition count.
    /// </summary>
    Normal,
    /// <summary>
    /// The system partition. For EFI/GPT, this is the special EFI System Partition (ESP), and will
    /// be formatted as FAT32. For BIOS/MBR, this is a regular NTFS partition that should precede
    /// the Windows partition. No drive letter will be assigned.
    /// </summary>
    System,
    /// <summary>
    /// A Microsoft reserved (MSR) partition. This partition type is only used for EFI/GPT.
    /// Partitions of this type will not be formatted using a file system.
    /// </summary>
    Msr,
    /// <summary>
    /// Indicates the partition is a utility partition, such as a recovery partition or Windows RE
    /// tools partition. It will be marked as a utility partition using the appropriate TypeId,
    /// and will not be assigned a drive letter.
    /// </summary>
    Utility
}
