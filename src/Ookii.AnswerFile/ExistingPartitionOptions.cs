using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for installing to an existing partition.
/// </summary>
/// <remarks>
/// <para>
///   When using this installation method, the partition specified by
///   <see cref="TargetedInstallOptionsBase.DiskId"/> and <see cref="PartitionId"/> will be
///   wiped and re-formatted, and Windows will be installed on this partition. Other partitions
///   are not affected.
/// </para>
/// <para>
///   Depending on the system type, a system or EFI partition must already exist.
/// </para>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public class ExistingPartitionOptions : TargetedInstallOptionsBase
{
    /// <summary>
    /// Gets or sets the ID of the partition to install to.
    /// </summary>
    /// <value>
    /// The one-based partition ID. The default value is zero.
    /// </value>
    /// <remarks>
    /// <para>
    ///   If the disk uses a default UEFI partition layout, Windows should be installed to
    ///   partition 3. For the default BIOS partition layout, Windows should be installed to
    ///   partition 2.
    /// </para>
    /// <para>
    ///   Since the default value of this property is zero, you must change it in order to generate
    ///   a valid answer file.
    /// </para>
    /// </remarks>
    [JsonPropertyName("TargetPartitionId")]
    public int PartitionId { get; set; }

    /// <summary>
    /// Gets the ID of the partition to install to.
    /// </summary>
    /// <value>
    /// The value of the <see cref="PartitionId"/> property.
    /// </value>
    protected override int TargetPartitionId => PartitionId;

    /// <summary>
    /// Writes the disk configuration for this installation method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
    protected override void WriteDiskConfiguration(AnswerFileGenerator generator)
    {
        using var modifyPartitions = generator.Writer.WriteAutoCloseElement("ModifyPartitions");
        generator.WriteModifyPartition(1, PartitionId, "NTFS", "Windows", 'C');
    }
}
