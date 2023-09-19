using System.Diagnostics.CodeAnalysis;

namespace Ookii.AnswerFile;

/// <summary>
/// Specifies a partition to be created when using the <see cref="CleanBiosOptions"/> or
/// <see cref="CleanEfiOptions"/> class.
/// </summary>
public class Partition : ISpanParsable<Partition>
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

    public static Partition Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        ParseHelper(s, provider, true, out var result);
        return result!;
    }

    public static Partition Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Partition result)
        => ParseHelper(s, provider, false, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Partition result)
    {
        if (s == null)
        {
            result = null;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    private static bool ParseHelper(ReadOnlySpan<char> s, IFormatProvider? provider, bool throwOnError, [MaybeNullWhen(false)] out Partition result)
    {
        var index = s.LastIndexOf(':');
        var label = index < 0 ? null : s[0..index].ToString();
        var size = index < 0 ? s : s[(index + 1)..];
        string? fileSystem = null;
        if (size.Length > 0 && size[size.Length - 1] == ']')
        {
            index = size.LastIndexOf('[');
            if (index >= 0)
            {
                fileSystem = size[(index + 1)..(size.Length - 1)].ToString();
                size = size[0..index];
            }
        }

        BinarySize? partitionSize = null;
        if (size.Length != 0 && size != "*")
        {
            if (throwOnError)
            {
                partitionSize = BinarySize.Parse(size, provider);
            }
            else
            {
                if (!BinarySize.TryParse(size, provider, out var value))
                {
                    result = null;
                    return false;
                }

                partitionSize = value;
            }
        }

        var type = label?.ToUpperInvariant() switch
        {
            "SYSTEM" => PartitionType.System,
            "MSR" => PartitionType.Msr,
            "WINRE" or "RECOVERY" => PartitionType.Utility,
            _ => PartitionType.Normal
        };

        if (type == PartitionType.Msr)
        {
            label = null;
        }

        result = new() { Type = type, Label = label, FileSystem = fileSystem, Size = partitionSize };
        return true;
    }
}
