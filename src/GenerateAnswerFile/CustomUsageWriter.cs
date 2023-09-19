using Ookii.CommandLine;

namespace GenerateAnswerFile;

class CustomUsageWriter : UsageWriter
{
    protected override void WriteParserUsageCore(UsageHelpRequest request)
    {
        base.WriteParserUsageCore(request);
        if (request == UsageHelpRequest.Full)
        {
            Write(Properties.Resources.UsageHelpFooter);
            WriteLine();
            WriteLine();
        }
    }
}
