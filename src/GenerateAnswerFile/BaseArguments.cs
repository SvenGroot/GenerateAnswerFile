using Ookii.CommandLine;
using System.Diagnostics;
using System.Globalization;

namespace GenerateAnswerFile;

[ResourceDescription(nameof(Properties.Resources.ApplicationDescription))]
abstract class BaseArguments
{
    [CommandLineArgument(IsPositional = true)]
    [ResourceDescription(nameof(Properties.Resources.OutputFileDescription))]
    [ResourceValueDescription(nameof(Properties.Resources.PathValueDescription))]
    [Alias("o")]
    public FileInfo? OutputFile { get; set; }

    [CommandLineArgument]
    [Alias("oh")]
    [Alias("??")]
    [ResourceDescription(nameof(Properties.Resources.OnlineHelpDescription))]
    public static CancelMode OnlineHelp(CommandLineParser parser)
    {
        try
        {
            var info = new ProcessStartInfo(Properties.Resources.OnlineHelpUrl)
            {
                UseShellExecute = true,
            };

            Process.Start(info);
        }
        catch (Exception ex)
        {
            if ((bool?)parser.GetArgument(nameof(Debug))!.Value ?? false)
            {
                Console.Error.WriteLine(ex.ToString());
            }

            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Properties.Resources.UsageHelpFooterFormat,
                CommandLineParser.GetExecutableName()));
        }

        return CancelMode.Abort;
    }
}
