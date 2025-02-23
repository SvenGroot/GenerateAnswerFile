namespace Ookii.AnswerFile;

/// <summary>
/// Provides credentials for a local user account.
/// </summary>
/// <remarks>
/// <para>
///   While the <see cref="DomainCredential"/> class can represent either a domain or local user
///   account, this class can only represent a local user account.
/// </para>
/// <note type="security">
///   Passwords in answer files are not encrypted. They are plain text at worst, and base64 encoded
///   at best. Do not store answer files with sensitive passwords in public locations.
/// </note>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public record class LocalCredential
{
    /// <summary>
    /// The group that users will be added to if no group is explicitly specified.
    /// </summary>
    /// <value>
    /// The "Administrators" group.
    /// </value>
    public const string DefaultGroup = "Administrators";

    /// <summary>
    /// Creates a new instance of the <see cref="LocalCredential"/> class.
    /// </summary>
    /// <param name="userName">The user name of the account.</param>
    /// <param name="password">The password of the account.</param>
    /// <param name="group">
    /// The group to add the user to. Multiple groups may be separated by semicolons.
    /// </param>
    /// <remarks>
    /// <note type="security">
    ///   Passwords in answer files are not encrypted. They are plain text at worst, and base64 encoded
    ///   at best. Do not store answer files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="userName"/> or <paramref name="password"/> or <paramref name="group"/> is
    ///   <see langword="null"/>.
    /// </exception>
    public LocalCredential(string userName, string password, string group = DefaultGroup)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(group);
        UserName = userName;
        Password = password;
        Group = group;
    }

    /// <summary>
    /// Gets the user name of the account.
    /// </summary>
    /// <value>
    /// The user name.
    /// </value>
    public string UserName { get; }

    /// <summary>
    /// Gets the password of the account.
    /// </summary>
    /// <value>
    /// The password.
    /// </value>
    /// <remarks>
    /// <note type="security">
    ///   Passwords in answer files are not encrypted. They are plain text at worst, and base64 encoded
    ///   at best. Do not store answer files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    public string Password { get; }

    /// <summary>
    /// Gets the group to which the user will be added.
    /// </summary>
    /// <value>
    /// The name of a local group on the target computer, or multiple group names separated by
    /// semicolons.
    /// </value>
    public string Group { get; }

    /// <summary>
    /// Parses the domain and user name from a string in the form 'user,password' or 'group:user,password'.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <returns>An instance of the <see cref="LocalCredential"/> class.</returns>
    /// <remarks>
    /// <para>
    ///   If the string does not contain a group, the value of the <see cref="DefaultGroup"/>
    ///   constant is used.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="FormatException"><paramref name="value"/> is not in a valid format.</exception>
    public static LocalCredential Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var (group, credential) = value.SplitOnce(':');
        var (userName, password) = credential.SplitOnce(',');
        if (userName == null)
        {
            throw new FormatException(Properties.Resources.InvalidLocalCredential);
        }

        return new LocalCredential(userName, password, group ?? DefaultGroup);
    }
}
