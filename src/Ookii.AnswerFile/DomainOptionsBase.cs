using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

/// <summary>
/// Base class for types that provide options to join a domain.
/// </summary>
/// <threadsafety instance="false" static="true"/>
[JsonDerivedType(typeof(DomainOptions), typeDiscriminator: "Credential")]
[JsonDerivedType(typeof(ProvisionedDomainOptions), typeDiscriminator: "Provisioning")]
public abstract class DomainOptionsBase
{
    private Collection<DomainUserGroup>? _domainAccounts;

    /// <summary>
    /// Gets a list of domain accounts that should be added to a local group.
    /// </summary>
    /// <value>
    /// A collection of <see cref="DomainUserGroup"/> instances for the accounts.
    /// </value>
    /// <remarks>
    /// <para>
    ///   If the <see cref="DomainUser.Domain" qualifyHint="true"/> property of an element in this
    ///   collection is <see langword="null"/>, the account is assumed to belong to the domain
    ///   specified in the <see cref="DomainOptions.Domain" qualifyHint="true"/> property, rather
    ///   than a local account. If this object is not an instance of the <see cref="DomainOptions"/>
    ///   class, then the <see cref="DomainUser.Domain" qualifyHint="true"/> property may not be
    ///   <see langword="null"/>.
    /// </para>
    /// </remarks>
    public Collection<DomainUserGroup> DomainAccounts
    {
        get => _domainAccounts ??= [];
        set => _domainAccounts = value;
    }

    /// <summary>
    /// Gets the name of the domain to use for an account in the <see cref="DomainAccounts"/>
    /// property where the <see cref="DomainUser.Domain" qualifyHint="true"/> property is
    /// <see langword="null"/>.
    /// </summary>
    /// <value>
    /// A domain name.
    /// </value>
    /// <remarks>
    /// <para>
    ///   The base class implementation always throw a <see cref="NotSupportedException"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// Using a default domain name is not supported.
    /// </exception>
    [JsonIgnore]
    public virtual string DefaultDomainAccountDomain
        => throw new NotSupportedException(Properties.Resources.DomainAccountWithoutDomain);

    /// <summary>
    /// When implemented in a derived class, writes options to join the domain.
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
    /// The derived class does not support generating options for the specified pass.
    /// </exception>
    public abstract void WriteDomainElements(AnswerFileGenerator generator, bool offlineServicing);
}
