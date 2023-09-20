namespace Ookii.AnswerFile;

/// <summary>
/// Provides credentials for a local user account.
/// </summary>
/// <remarks>
/// <note type="security">
///   Passwords in answer files are not encrypted. They are plain text at worst, and base64 encoded
///   at best. Do not store generated files with sensitive passwords in public locations.
/// </note>
/// </remarks>
public record class LocalCredential
{
    /// <summary>
    /// Creates a new instance of the <see cref="LocalCredential"/> class.
    /// </summary>
    /// <param name="userName">The user name of the account.</param>
    /// <param name="password">The password of the account.</param>
    /// <remarks>
    /// <note type="security">
    ///   Passwords in answer files are not encrypted. They are plain text at worst, and base64 encoded
    ///   at best. Do not store generated files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="userName"/> or <paramref name="password"/> is <see langword="null"/>.
    /// </exception>
    public LocalCredential(string userName, string password)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);
        UserName = userName;
        Password = password;
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
    ///   at best. Do not store generated files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    public string Password { get; }

    /// <summary>
    /// Parses the domain and user name from a string in the form 'user,password'.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <returns>An instance of the <see cref="LocalCredential"/> class.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="FormatException"><paramref name="value"/> is not in a valid format.</exception>
    public static LocalCredential Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var (userName, password) = value.SplitOnce(',');
        if (userName == null)
        {
            throw new FormatException(Properties.Resources.InvalidLocalCredential);
        }

        return new LocalCredential(userName, password);
    }
}
