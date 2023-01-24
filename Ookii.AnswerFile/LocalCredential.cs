namespace Ookii.AnswerFile;

public record class LocalCredential(string UserName, string Password)
{
    public static LocalCredential Parse(string value)
    {
        var (userName, password) = value.SplitOnce(',');
        if (userName == null)
        {
            throw new FormatException("Format is not 'user,password'.");
        }

        return new LocalCredential(userName, password);
    }
}
