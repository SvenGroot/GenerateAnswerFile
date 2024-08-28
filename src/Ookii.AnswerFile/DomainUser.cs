using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

/// <summary>
/// Represents a domain or local user.
/// </summary>
/// <remarks>
/// <para>
///   This class represents a local user account if the <see cref="Domain"/> property is
///   <see langword="null"/>.
/// </para>
/// </remarks>
/// <threadsafety instance="false" static="true"/>
[JsonConverter(typeof(DomainUserJsonConverter))]
public record class DomainUser
{
    #region Nested types

    internal class DomainUserJsonConverter : JsonConverter<DomainUser>
    {
        public override DomainUser? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value == null)
            {
                return null;
            }

            return Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DomainUser value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(value);

            writer.WriteStringValue(value.ToString());
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainUser"/> class.
    /// </summary>
    /// <param name="domain">
    ///   The domain of the account, or <see langword="null"/> if this is a local account.
    /// </param>
    /// <param name="userName">The account user name.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="userName"/> is <see langword="null"/>.
    /// </exception>
    public DomainUser(string? domain, string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);
        Domain = domain;
        UserName = userName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainUser"/> class for a local user account.
    /// </summary>
    /// <param name="userName">The account user name.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="userName"/> is <see langword="null"/>.
    /// </exception>
    public DomainUser(string userName)
        : this(null, userName)
    {
    }

    /// <summary>
    /// Gets the domain for the user account.
    /// </summary>
    /// <value>
    /// The domain name, or <see langword="null"/> if this is a local account.
    /// </value>
    public string? Domain { get; }

    /// <summary>
    /// Gets the account user name.
    /// </summary>
    /// <value>
    /// The user name.
    /// </value>
    public string UserName { get; }

    /// <summary>
    /// Gets a string representation of the current <see cref="DomainUser"/>.
    /// </summary>
    /// <returns>
    /// If the <see cref="Domain"/> property is not <see langword="null"/>, a string in the format
    /// "domain\user"; otherwise, the value of the <see cref="UserName"/> property.
    /// </returns>
    public override string ToString()
        => Domain == null ? UserName : $"{Domain}\\{UserName}";

    /// <summary>
    /// Parses the domain and user name from a string in the form 'domain\user' or just 'user'.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <returns>An instance of the <see cref="DomainUser"/> class.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static DomainUser Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var (domain, userName) = value.SplitOnce('\\');
        return new DomainUser(domain, userName);
    }
}
