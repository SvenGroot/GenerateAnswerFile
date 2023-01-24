using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

public class DomainOptions
{
    public DomainOptions(string domain, DomainCredential credential)
    {
        ArgumentNullException.ThrowIfNull(domain);
        ArgumentNullException.ThrowIfNull(credential);
        Domain = domain;
        Credential = credential;
    }

    public string Domain { get; }

    public DomainCredential Credential { get; }

    public string? OUPath { get; set; }

    public Collection<string> DomainAccounts { get; } = new();
}
