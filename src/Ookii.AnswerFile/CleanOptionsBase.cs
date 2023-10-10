using System.CodeDom.Compiler;
using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

/// <summary>
/// Base class for all install options that perform a clean installation on a specific disk.
/// </summary>
/// <threadsafety instance="false" static="true"/>
public abstract class CleanOptionsBase : TargetedInstallOptionsBase
{
    private Collection<Partition>? _partitions;

    /// <summary>
    /// Gets or sets a list of partitions to create.
    /// </summary>
    /// <value>
    /// A collection of <see cref="Partition"/> objects.
    /// </value>
    /// <remarks>
    /// <para>
    ///   If this property is an empty list, the default layout returned by the
    ///   <see cref="GetDefaultPartitions"/> method will be used, which depends on the system type.
    /// </para>
    /// </remarks>
    public Collection<Partition> Partitions
    {
        get => _partitions ??= new();
        set => _partitions = value;
    }

    /// <summary>
    /// Gets or sets the ID of the partition to install to.
    /// </summary>
    /// <value>
    /// The one-based ID of the partition to install to, or <see langword="null"/> to install to
    /// the first primary non-utility partition. The default value is <see langword="null"/>.
    /// </value>
    public int? CustomTargetPartitionId { get; set; }

    /// <summary>
    /// Gets the ID of the partition to install to.
    /// </summary>
    /// <value>
    /// The value of the <see cref="CustomTargetPartitionId"/> property, or if that property is
    /// <see langword="null"/>, the one-based ID of the first partition with
    /// <see cref="PartitionType.Normal" qualifyHint="true"/>.
    /// </value>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="CustomTargetPartitionId"/> property is <see langword="null"/>, and the value
    /// of the <see cref="Partitions"/> property does not contain a partition with
    /// <see cref="PartitionType.Normal" qualifyHint="true"/>.
    /// </exception>
    protected override int TargetPartitionId
    {
        get
        {
            if (CustomTargetPartitionId is int id)
            {
                return id;
            }

            var partitions = _partitions?.Count > 0 ? _partitions : GetDefaultPartitions();
            var index = partitions.FindIndex(p => p.Type == PartitionType.Normal);
            if (index < 0)
            {
                throw new InvalidOperationException(Properties.Resources.UnknownTargetPartition);
            }

            return index + 1;
        }
    }

    /// <summary>
    /// When implemented in a derived class, gets the partition type to use for the system
    /// partition.
    /// </summary>
    /// <value>
    /// The partition type, either "Primary" or "EFI".
    /// </value>
    protected abstract string SystemPartitionType { get; }

    /// <summary>
    /// When implemented in a derived class, gets the file system to use for the system partition.
    /// </summary>
    /// <value>
    /// The file system type, either "FAT32" or "NTFS".
    /// </value>
    protected abstract string SystemPartitionFileSystem { get; }

    /// <summary>
    /// When implemented in a derived class, gets a value which indicates whether an extended
    /// partition should be used if there are more than 4 partitions.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if an extended partition should be used; otherwise,
    /// <see langword="false"/>.
    /// </value>
    protected abstract bool UseExtendedPartition { get; }

    /// <summary>
    /// When implemented in a derived class, gets the type ID that marks a partition as a utility 
    /// partition.
    /// </summary>
    /// <value>
    /// The partition type ID.
    /// </value>
    protected abstract string UtilityTypeId { get; }

    /// <summary>
    /// Writes the disk configuration for this installation method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
    protected override void WriteDiskConfiguration(Generator generator)
    {
        var partitions = _partitions?.Count > 0 ? _partitions : GetDefaultPartitions();

        generator.Writer.WriteElementString("WillWipeDisk", "true");
        bool useLogical = false;
        int order = 1;
        using (var createPartitions = generator.Writer.WriteAutoCloseElement("CreatePartitions"))
        {
            foreach (var partition in partitions)
            {
                if (order == 4 && partitions.Count > 4 && UseExtendedPartition)
                {
                    generator.WriteCreatePartition(order, "Extended");
                    useLogical = true;
                    order++;
                }

                var partitionType = partition.Type switch
                {
                    PartitionType.System => SystemPartitionType,
                    PartitionType.Msr => "MSR",
                    _ => useLogical ? "Logical" : "Primary",
                };

                var size = (int?)partition.Size?.AsMebi;
                if (size == null && useLogical)
                {
                    // Must use extend on modify for logical partition.
                    size = 100;
                }

                generator.WriteCreatePartition(order, partitionType, size);
                order++;
            }
        }

        using var modifyPartitions = generator.Writer.WriteAutoCloseElement("ModifyPartitions");
        order = 1;
        char nextLetter = 'C';
        foreach (var partition in partitions)
        {
            var format = partition.Type switch
            {
                PartitionType.System => SystemPartitionFileSystem,
                PartitionType.Msr => null,
                _ => partition.FileSystem ?? "NTFS"
            };

            var label = partition.Type == PartitionType.Msr ? null : partition.Label;
            var active = (partition.Type == PartitionType.System && SystemPartitionType == "Primary") ? true : (bool?)null;
            var extend = (useLogical && partition.Size == null) ? true : (bool?)null;
            char? letter = null;
            if (partition.Type == PartitionType.Normal && nextLetter <= 'Z')
            {
                letter = nextLetter;
                ++nextLetter;
            }

            var typeId = partition.Type == PartitionType.Utility ? UtilityTypeId : null;

            generator.WriteModifyPartition(order, order, format, label, letter, extend, active, typeId);
            ++order;
        }
    }

    /// <summary>
    /// When implemented in a derived class, gets the partition layout to use if the
    /// <see cref="Partitions"/> property is an empty list.
    /// </summary>
    /// <returns>A list containing the default partition layout.</returns>
    protected abstract IList<Partition> GetDefaultPartitions();
}
