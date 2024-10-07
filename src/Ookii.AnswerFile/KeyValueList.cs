namespace Ookii.AnswerFile;

class KeyValueList : List<KeyValuePair<string, object?>>
{
    public void Add(string key, object? value) => Add(new(key, value));
}
