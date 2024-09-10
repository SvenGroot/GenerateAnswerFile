using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for joining a domain.
/// </summary>
/// <remarks>
/// <note type="security">
///   The password of the account used to join the domain is stored in plain text in the answer
///   file. Do not store answer files with sensitive passwords in public locations.
/// </note>
/// <para>
///   This class cannot be used if the <see cref="InstallOptionsBase.DomainJoinOffline" qualifyHint="true"/>
///   property is <see langword="true"/>. To join a domain during the offlineServicing pass, you
///   must use the <see cref="ProvisionedDomainOptions"/> class instead.
/// </para>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public class DomainOptions : DomainOptionsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainOptions"/> class.
    /// </summary>
    /// <param name="domain">The domain to join.</param>
    /// <param name="credential">
    /// The credentials of a domain account that has permission to join the domain.
    /// </param>
    /// <remarks>
    /// <note type="security">
    ///   The password of the account used to join the domain is stored in plain text in the answer
    ///   file. Do not store answer files with sensitive passwords in public locations.
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
    /// The organizational unit path, or <see langword="null"/> to not join a specific OU. The
    /// default value is <see langword="null"/>.
    /// </value>
    public string? OUPath { get; set; }

    /// <summary>
    /// Gets the name of the domain to use for an account in the
    /// <see cref="DomainOptionsBase.DomainAccounts" qualifyHInt="true"/> property where the
    /// <see cref="DomainUser.Domain" qualifyHint="true"/> is <see langword="null"/>.
    /// </summary>
    /// <value>
    /// The value of the <see cref="Domain"/> property.
    /// </value>
    public override string DefaultDomainAccountDomain => Domain;

    /// <summary>
    /// Writes options to join the domain.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
    /// <param name="offlineServicing">
    /// <see langword="true"/> if the options are for the offlineServicing pass;
    /// <see langword="false"/> if they are for the specialize pass.
    /// </param>
    /// <remarks>
    /// <para>
    ///   This method is called when generating the Microsoft-Windows-JoinDomain component of the
    ///   specialize or offlineServicing pass.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="generator"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="offlineServicing"/> is <see langword="true"/>.
    /// </exception>
    public override void WriteDomainElements(Generator generator, bool offlineServicing)
    {
        ArgumentNullException.ThrowIfNull(generator);
        if (offlineServicing)
        {
            throw new NotSupportedException(Properties.Resources.UnsupportedDomainOptionsPass);
        }

        generator.Writer.WriteElements(new KeyValueList
            {
                { "Identification", new KeyValueList
                {
                    { "UnsecureJoin", false },
                    { "Credentials", new KeyValueList
                    {
                        { "Domain", Credential.UserAccount.Domain ?? Domain },
                        { "Password", Credential.Password },
                        { "Username", Credential.UserAccount.UserName }
                    }
                    },
                    { "JoinDomain", Domain },
                    { "MachineObjectOU", OUPath },
                }
                }
            });
    }
}
