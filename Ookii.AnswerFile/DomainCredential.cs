namespace Ookii.AnswerFile;

public class DomainCredential
{
    public DomainCredential(DomainUser userAccount, string password)
    {
        ArgumentNullException.ThrowIfNull(userAccount);
        ArgumentNullException.ThrowIfNull(password);
        UserAccount = userAccount;
        Password = password;
    }

    public DomainUser UserAccount { get; }

    public string Password { get; }
}
