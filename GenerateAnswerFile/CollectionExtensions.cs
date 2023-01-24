namespace GenerateAnswerFile;

static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            self.Add(item);
        }
    }
}
