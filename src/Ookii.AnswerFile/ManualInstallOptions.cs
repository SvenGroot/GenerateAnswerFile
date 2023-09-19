namespace Ookii.AnswerFile;

/// <summary>
/// Indicates the user must manually pick an installation target.
/// </summary>
/// <remarks>
/// When using this installation method, the installation is not fully unattended.
/// </remarks>
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
