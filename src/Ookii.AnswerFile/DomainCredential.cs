namespace Ookii.AnswerFile;

/// <summary>
/// Represents a credential for a domain or local user.
/// </summary>
/// <remarks>
/// <note type="security">
///   Passwords in answer files are not encrypted. They are plain text at worst, and base64 encoded
///   at best. Do not store generated files with sensitive passwords in public locations.
/// </note>
/// </remarks>
public class DomainCredential
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainCredential"/> class.
    /// </summary>
    /// <param name="userAccount">The domain or local user account.</param>
    /// <param name="password">The password.</param>
    /// <remarks>
    /// <note type="security">
    ///   Passwords in answer files are not encrypted. They are plain text at worst, and base64 encoded
    ///   at best. Do not store generated files with sensitive passwords in public locations.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="userAccount"/> or <paramref name="password"/> is <see langword="null"/>.
    /// </exception>
    public DomainCredential(DomainUser userAccount, string password)
    {
        ArgumentNullException.ThrowIfNull(userAccount);
        ArgumentNullException.ThrowIfNull(password);
        UserAccount = userAccount;
        Password = password;
    }

    /// <summary>
    /// Gets the domain or local user account.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="DomainUser"/> class.
    /// </value>
    public DomainUser UserAccount { get; }

    /// <summary>
    /// Gets the password for the account.
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
}
