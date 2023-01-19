namespace GenerateAnswerFile;

record class UserAndPassword(string UserName, string Password)
{
    public static UserAndPassword Parse(string value)
    {
        var (userName, password) = value.SplitOnce(',');
        if (userName == null)
        {
            throw new FormatException("Format is not 'user,password'.");
        }

        return new UserAndPassword(userName, password);
    }
}
