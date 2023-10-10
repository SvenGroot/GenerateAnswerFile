namespace Ookii.AnswerFile;

static class EnumerableExtensions
{
    public static int FindIndex<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        int index = 0;
        foreach (var item in source)
        {
            if (predicate(item))
            {
                return index;
            }

            ++index;
        }

        return -1;
    }
}
