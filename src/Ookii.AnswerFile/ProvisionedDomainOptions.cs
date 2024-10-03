using System.IO;
using System.Net;

namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for joining a domain using a provisioned computer account.
/// </summary>
/// <remarks>
/// <para>
///   The account data needed for provisioning can be created using the following command:
///   <c>djoin.exe /provision /domain domainname /machine machinename /savefile filename</c>. The
///   contents of the created file should be passed to the constructor of this class.
/// </para>
/// <para>
///   Joining a domain using provisioning is supported both during the specialize and
///   offlineServicing passes.
/// </para>
/// </remarks>
public class ProvisionedDomainOptions : DomainOptionsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProvisionedDomainOptions"/> class.
    /// </summary>
    /// <param name="accountData">The account data used to join the domain.</param>
    public ProvisionedDomainOptions(string accountData)
    {
        ArgumentNullException.ThrowIfNull(accountData);
        AccountData = accountData;
    }

    /// <summary>
    /// Gets the account data used to join the domain.
    /// </summary>
    /// <value>
    /// The account data.
    /// </value>
    public string AccountData { get; }

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
    public override void WriteDomainElements(Generator generator, bool offlineServicing)
    {
        ArgumentNullException.ThrowIfNull(generator);
        string elementName = offlineServicing ? "OfflineIdentification" : "Identification";
        generator.Writer.WriteElements(new KeyValueList
            {
                { elementName, new KeyValueList
                {
                    { "Provisioning", new KeyValueList
                    {
                        { "AccountData", AccountData },
                    }
                    },
                }
                }
            });
    }
}
