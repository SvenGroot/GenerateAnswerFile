using Ookii.AnswerFile;
using Ookii.CommandLine;
using Ookii.CommandLine.Terminal;
using System.Text;
using System.Text.Json;

namespace GenerateAnswerFile;

static class Program
{
    public static int Main()
    {
        var debug = Environment.GetEnvironmentVariable("OOKII_DEBUG") == "1";
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            var options = GetOptions();
            if (options == null)
            {
                return 1;
            }

            var (arguments, answerFileOptions) = options.Value;
            if (arguments.OutputFile != null)
            {
                AnswerFileGenerator.Generate(arguments.OutputFile.FullName, answerFileOptions);
            }
            else
            {
                AnswerFileGenerator.Generate(Console.Out, answerFileOptions);
            }
        }
        catch (Exception ex) when (debug)
        {
            VirtualTerminal.WriteLineErrorFormatted(ex.ToString());
            return 1;
        }
        catch (JsonException ex)
        {
            var message = ex.Message ?? "";

            // For some reason this isn't always present, so we'll add it manually if it's missing.
            if (!message.Contains("BytePositionInLine") && ex.LineNumber != null)
            {
                message += $" Path: {ex.Path} | LineNumber: {ex.LineNumber} | BytePositionInLine: {ex.BytePositionInLine}";
            }

            VirtualTerminal.WriteLineErrorFormatted(message);
            return 1;
        }
        catch (Exception ex)
        {
            VirtualTerminal.WriteLineErrorFormatted(ex.Message);
            return 1;
        }

        return 0;
    }

    private static (BaseArguments Arguments, AnswerFileOptions Options)? GetOptions()
    {
        var jsonOptions = GetJsonOptions();
        if (jsonOptions != null)
        {
            if (jsonOptions.Value.Arguments == null)
            {
                return null;
            }

            return jsonOptions!;
        }

        var options = new ParseOptions
        {
            UsageWriter = new CustomUsageWriter(),
            DefaultValueDescriptions = new Dictionary<Type, string>()
            {
                { typeof(int), Properties.Resources.NumberValueDescription }
            }
        };

        var arguments = Arguments.Parse(options);
        if (arguments == null)
        {
            return null;
        }

        return (arguments, arguments.ToOptions());
    }

    private static (BaseArguments? Arguments, AnswerFileOptions Options)? GetJsonOptions()
    {
        if (!Console.IsInputRedirected)
        {
            return null;
        }

        var json = Console.In.ReadToEnd();
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        var options = AnswerFileOptions.FromJson(json);
        if (options == null)
        {
            return null;
        }

        var arguments = JsonArguments.Parse();
        return (arguments, options);
    }
}
