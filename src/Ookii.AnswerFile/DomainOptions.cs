using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for joining a domain.
/// </summary>
/// <remarks>
/// <note type="security">
///   The password of the account used to join the domain is stored in plain text.
/// </note>
/// </remarks>
public class DomainOptions
{
    private Collection<string>? _domainAccounts;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainOptions"/> class.
    /// </summary>
    /// <param name="domain">The domain to join.</param>
    /// <param name="credential">The credentials of a domain account that has permission to join the domain.</param>
    /// <remarks>
    /// <note type="security">
    ///   The password of the account used to join the domain is stored in plain text.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="domain"/> or <paramref name="credential"/> is <see langword="null"/>.
    /// </exception>
    public DomainOptions(string domain, DomainCredential credential)
    {
        ArgumentNullException.ThrowIfNull(domain);
        ArgumentNullException.ThrowIfNull(credential);
        Domain = domain;
        Credential = credential;
    }

    /// <summary>
    /// Gets the name of the domain to join.
    /// </summary>
    /// <value>
    /// The domain.
    /// </value>
    public string Domain { get; }

    /// <summary>
    /// Gets the credentials of a domain account that has permission to join the domain.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="DomainCredential"/> class.
    /// </value>
    public DomainCredential Credential { get; }

    /// <summary>
    /// Gets the path of the Organizational Unit that the computer account should be added to.
    /// </summary>
    /// <value>
    /// The organizational unit path, or <see langword="null"/> to not join a specific OU.
    /// </value>
    public string? OUPath { get; set; }

    /// <summary>
    /// Gets a list of domain accounts that should be added to the local administrators group.
    /// </summary>
    /// <value>
    /// A collection of the accounts.
    /// </value>
    /// <remarks>
    /// <para>
    ///   These accounts must be in the domain specified by the <see cref="Domain"/> property.
    /// </para>
    /// </remarks>
    public Collection<string> DomainAccounts
    {
        get => _domainAccounts ??= new();
        set => _domainAccounts = value;
    }
}
