namespace Ookii.AnswerFile;

static class StringExtensions
{
    public ref struct SplitResult
    {
        public bool HasValue { get; set; }
        public ReadOnlySpan<char> Left { get; set; }
        public ReadOnlySpan<char> Right { get; set; }

        public readonly void Deconstruct(out bool hasValue, out ReadOnlySpan<char> left, out ReadOnlySpan<char> right)
        {
            hasValue = HasValue;
            left = Left;
            right = Right;
        }
    }

    public static (string?, string) SplitOnce(this string value, char separator)
    {
        var index = value.IndexOf(separator);
        if (index < 0)
        {
            return (null, value);
        }

        return (value[0..index], value[(index + 1)..]);
    }

    public static SplitResult SplitOnce(this ReadOnlySpan<char> value, char separator)
    {
        var index = value.IndexOf(separator);
        if (index < 0)
        {
            return default;
        }

        return new()
        {
            HasValue = true,
            Left = value[0..index],
            Right = value[(index + 1)..]
        };
    }

    public static SplitResult SplitOnceLast(this ReadOnlySpan<char> value, char separator)
    {
        var index = value.LastIndexOf(separator);
        if (index < 0)
        {
            return default;
        }

        return new()
        {
            HasValue = true,
            Left = value[0..index],
            Right = value[(index + 1)..]
        };
    }

    public static char? Last(this ReadOnlySpan<char> value)
        => value.Length == 0 ? null : value[value.Length - 1];
}
