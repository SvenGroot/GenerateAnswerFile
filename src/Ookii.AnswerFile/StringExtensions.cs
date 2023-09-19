namespace Ookii.AnswerFile;

static class StringExtensions
{
    public static (string?, string) SplitOnce(this string value, char separator)
    {
        var index = value.IndexOf(separator);
        if (index < 0)
        {
            return (null, value);
        }

        return (value[0..index], value[(index + 1)..]);
    }
}
