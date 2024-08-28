using Ookii.CommandLine;

namespace GenerateAnswerFile;

[ResourceDescription(nameof(Properties.Resources.ApplicationDescription))]
abstract class BaseArguments
{
    [CommandLineArgument(IsPositional = true)]
    [ResourceDescription(nameof(Properties.Resources.OutputFileDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.PathValueDescription))]
    [Alias("o")]
    public FileInfo? OutputFile { get; set; }
}
