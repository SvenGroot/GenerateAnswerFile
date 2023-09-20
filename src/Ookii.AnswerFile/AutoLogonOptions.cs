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
    /// <remarks>
    /// <para>
    ///   A workaround is applied for an issue where Windows adds 1 to the count specified in the
    ///   answer file. If the count is 1, a first-logon command is used to clear the count to ensure
    ///   exactly one logon. If the count is larger than 1, the count written to the answer file
    ///   will be one less than the value specified here.
    /// </para>
    /// </remarks>
    public int Count { get; set; } = 1;
}
