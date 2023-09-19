namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for logging on automatically.
/// </summary>
public class AutoLogonOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoLogonOptions"/> class.
    /// </summary>
    /// <param name="userAccount">The domain or local account used to log on automatically.</param>
    /// <param name="password">The password of the account used to log on automatically.</param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="userAccount"/> or <paramref name="password"/> is <see langword="null"/>.
    /// </exception>
    public AutoLogonOptions(DomainUser userAccount, string password)
    {
        Credential = new DomainCredential(userAccount, password);
    }

    /// <summary>
    /// Gets the credentials used to log on automatically.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="DomainCredential"/> class.
    /// </value>
    public DomainCredential Credential { get; }

    /// <summary>
    /// Gets the number of times automatic log-on will be used.
    /// </summary>
    /// <value>
    /// The automatic log-on count. The default value is 1.
    /// </value>
    public int Count { get; set; } = 1;
}
