namespace Ookii.AnswerFile;

/// <summary>
/// Provides options for an installation where the disk and partition to install to must be manually
/// selected by the user.
/// </summary>
/// <remarks>
/// When using this installation method, the installation is not fully unattended.
/// </remarks>
/// <threadsafety instance="false" static="true"/>
public class ManualInstallOptions : InstallOptionsBase
{
    /// <summary>
    /// This method does nothing for this installation method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
    protected override void WriteInstallElements(Generator generator)
    {
        // Intentionally blank.
    }
}
