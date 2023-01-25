using GenerateAnswerFile;
using Ookii.AnswerFile;
using Ookii.CommandLine;
using Ookii.CommandLine.Terminal;

var arguments = Arguments.Parse();
if (arguments == null)
{
    return 1;
}

try
{
    Generator.Generate(arguments.OutputFile.FullName, arguments.ToOptions());
}
catch (Exception ex)
{
    using var support = VirtualTerminal.EnableColor(StandardStream.Error);
    using var writer = LineWrappingTextWriter.ForConsoleError();
    if (support.IsSupported)
    {
        writer.Write(TextFormat.ForegroundRed);
    }

    writer.Write(ex.Message);
    if (support.IsSupported)
    {
        writer.Write(TextFormat.Default);
    }

    writer.WriteLine();
    return 1;
}

return 0;
