namespace Ookii.AnswerFile;

public class AutoLogonOptions
{
    public AutoLogonOptions(DomainUser userAccount, string password)
    {
        Credential = new DomainCredential(userAccount, password);
    }

    public DomainCredential Credential { get; }

    public int Count { get; set; } = 1;
}
