using Ookii.CommandLine;
using System.Globalization;

namespace GenerateAnswerFile;

class CustomUsageWriter : UsageWriter
{
    protected override void WriteParserUsageCore(UsageHelpRequest request)
    {
        base.WriteParserUsageCore(request);
        if (request == UsageHelpRequest.Full)
        {
            Write(string.Format(CultureInfo.CurrentCulture, Properties.Resources.UsageHelpFooterFormat, ExecutableName));
            WriteLine();
            WriteLine();
        }
    }
}
