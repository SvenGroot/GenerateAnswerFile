namespace GenerateAnswerFile;

record class DomainUser(string? Domain, string UserName)
{
    public static DomainUser Parse(string value)
    {
        var (domain, userName) = value.SplitOnce('\\');
        return new DomainUser(domain, userName);
    }
}
