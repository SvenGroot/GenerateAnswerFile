namespace Ookii.AnswerFile;

/// <summary>
/// Represents a domain user and the local group to which the user should be added.
/// </summary>
/// <remarks>
/// <para>
///   This class is used to add domain users to local groups when the target system is joined to a
///   domain as part of the unattended installation.
/// </para>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public record class DomainUserGroup
{
    /// <summary>
    /// The group that users will be added to if no group is explicitly specified.
    /// </summary>
    /// <value>
    /// The "Administrators" group.
    /// </value>
    public const string DefaultGroup = "Administrators";

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainUserGroup"/> class.
    /// </summary>
    /// <param name="domainUser">
    /// The <see cref="AnswerFile.DomainUser"/> to add to the group.
    /// </param>
    /// <param name="group">
    /// The group to add the user to. Multiple groups may be separated by semicolons.
    /// </param>
    /// <remarks>
    /// <para>
    ///   If the <see cref="DomainUser.Domain" qualifyHint="true"/> property of
    ///   <paramref name="domainUser"/> is <see langword="null"/>, this refers to a user that is
    ///   a member of the domain that is being joined, not a local account. When using the
    ///   <see cref="ProvisionedDomainOptions"/> class, the <see cref="DomainUser.Domain" qualifyHint="true"/>
    ///   property may not be <see langword="null"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="domainUser"/> or <paramref name="group"/> is <see langword="null"/>.
    /// </exception>
    public DomainUserGroup(DomainUser domainUser, string group = DefaultGroup)
    {
        ArgumentNullException.ThrowIfNull(domainUser);
        ArgumentNullException.ThrowIfNull(group);
        DomainUser = domainUser;
        Group = group;
    }

    /// <summary>
    /// Gets the domain user that is being added to a group.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="AnswerFile.DomainUser"/> class.
    /// </value>
    /// <remarks>
    /// <para>
    ///   If the <see cref="DomainUser.Domain" qualifyHint="true"/> property is
    ///   <see langword="null"/>, this refers to a user that is a member of the domain that is being
    ///   joined, not a local account. When using the <see cref="ProvisionedDomainOptions"/> class,
    ///   the <see cref="DomainUser.Domain" qualifyHint="true"/> property may not be
    ///   <see langword="null"/>.
    /// </para>
    /// </remarks>
    public DomainUser DomainUser { get; }

    /// <summary>
    /// The group to which the user will be added.
    /// </summary>
    /// <value>
    /// The name of a local group on the target computer, or multiple group names separated by
    /// semicolons.
    /// </value>
    public string Group { get; }

    /// <summary>
    /// Gets a string representation of the current <see cref="DomainUserGroup"/>.
    /// </summary>
    /// <returns>
    /// A string in the format "group:domain\user", or "group:user" if the
    /// <see cref="DomainUser.Domain" qualifyHint="true"/> property is <see langword="null"/>.
    /// </returns>
    public override string ToString() => $"{Group}:{DomainUser}";

    /// <summary>
    /// Parses the group, domain and user name from a string in the form '[group:][domain\]user'.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <returns>An instance of the <see cref="DomainUserGroup"/> class.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   If the string does not contain a group, the value of the <see cref="DefaultGroup"/>
    ///   constant is used. If the string does not contain a domain, this indicates the user is a
    ///   member of the domain that the target computer is joining. When using the
    ///   <see cref="ProvisionedDomainOptions"/> class, the value must contain a domain.
    /// </para>
    /// </remarks>
    public static DomainUserGroup Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var (group, domainUser) = value.SplitOnce(':');
        return new DomainUserGroup(DomainUser.Parse(domainUser), group ?? DefaultGroup);
    }
}
