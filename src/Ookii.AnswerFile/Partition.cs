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

    /// <summary>
    /// Parses a span of characters into a <see cref="Partition"/> class.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> contains a partition size that is not representable as a <see cref="BinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    /// </remarks>
    public static Partition Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        ParseHelper(s, provider, true, out var result);
        return result!;
    }

    /// <summary>
    /// Parses a string into a <see cref="Partition"/> class.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> contains a partition size that is not representable as a <see cref="BinarySize"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   The input must use the format "size", "label:size", "size[fs]" or "label:size[fs]", where
    ///   "label" is the volume label, "size" is a value using multiple-byte units, and "fs" is the
    ///   name of a supported file system such as "FAT32" or "NTFS". An example value is
    ///   "Windows:128GB" or "Data:16GB[FAT32]"
    /// </para>
    /// <para>
    ///   Instead of an explicit size, you can use "*" for the size to indicate the partition
    ///   should be extended to fill the remaining space on the disk.
    /// </para>
    /// <para>
    ///   Certain volume labels are treated specially, and can be used to create special partition
    ///   types. "System" creates a partition with <see cref="PartitionType.System" qualifyHint="true"/>,
    ///   "MSR" with <see cref="PartitionType.Msr" qualifyHint="true"/>, and "WinRE" and "Recovery"
    ///   with <see cref="PartitionType.Utility" qualifyHint="true"/>. This check is case
    ///   insensitive, and all other volume labels create a partition with
    ///   <see cref="PartitionType.Normal" qualifyHint="true"/>.
    /// </para>
    /// </remarks>
    public static Partition Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>
    /// Tries to parse a span of characters into a <see cref="Partition"/> class.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the result of successfully parsing <paramref name="s"/>,
    /// or an undefined value on failure.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Partition result)
        => ParseHelper(s, provider, false, out result);

    /// <summary>
    /// Tries to parse a string into a <see cref="Partition"/> class.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the result of successfully parsing <paramref name="s"/>,
    /// or an undefined value on failure.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    /// </remarks>
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
        var (hasSeparator, left, right) = s.SplitOnceLast(':');
        ReadOnlySpan<char> size = s;
        string? label = null;
        if (hasSeparator)
        {
            label = left.ToString();
            size = right;
        }

        string? fileSystem = null;
        if (size.Last() == ']')
        {
            (hasSeparator, left, right) = size.SplitOnceLast('[');
            if (hasSeparator)
            {
                fileSystem = right[..(right.Length - 1)].ToString();
                size = left;
            }
        }

        BinarySize? partitionSize = null;
        if (size.Length != 0 && !size.Equals("*", StringComparison.Ordinal))
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
