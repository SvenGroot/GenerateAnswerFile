using GenerateAnswerFile;
using Ookii.AnswerFile;
using Ookii.CommandLine;
using Ookii.CommandLine.Terminal;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var options = new ParseOptions
{
    UsageWriter = new CustomUsageWriter()
    {
        UseAbbreviatedSyntax = true,
        IndentAfterEmptyLine = true,
    },
    DefaultValueDescriptions = new Dictionary<Type, string>() 
    {
        { typeof(int), GenerateAnswerFile.Properties.Resources.NumberValueDescription } 
    }
};

var arguments = Arguments.Parse(options);
if (arguments == null)
{
    return 1;
}

try
{
    if (arguments.OutputFile != null)
    {
        Generator.Generate(arguments.OutputFile.FullName, arguments.ToOptions());
    }
    else
    {
        Generator.Generate(Console.Out, arguments.ToOptions());
    }
}
catch (Exception ex)
{
    if (arguments.Debug)
    {
        VirtualTerminal.WriteLineErrorFormatted(ex.ToString());
    }
    else
    {
        VirtualTerminal.WriteLineErrorFormatted(ex.Message);
    }

    return 1;
}

return 0;
