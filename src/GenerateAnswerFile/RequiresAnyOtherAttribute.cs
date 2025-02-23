using Ookii.CommandLine;
using Ookii.CommandLine.Validation;

namespace GenerateAnswerFile;

// Basically RequiresAny, but applied to an argument rather than a class.
internal class RequiresAnyOtherAttribute : ArgumentValidationWithHelpAttribute
{
    public RequiresAnyOtherAttribute(params string[] arguments)
    {
        Arguments = arguments;
    }

    public string[] Arguments { get; }

    public override CommandLineArgumentErrorCategory ErrorCategory => CommandLineArgumentErrorCategory.DependencyFailed;

    public override bool IsValidPostParsing(CommandLineArgument argument)
        => !argument.HasValue || Arguments.Any(a => argument.Parser.GetArgument(a)!.HasValue);

    public override string GetErrorMessage(CommandLineArgument argument, object? value)
        => string.Format(Properties.Resources.RequiresAnyOtherErrorFormat, argument.ArgumentName, string.Join(", ", Arguments.Select(a => $"-{a}")));

    protected override string GetUsageHelpCore(CommandLineArgument argument)
        => string.Format(Properties.Resources.RequiresAnyOtherUsageHelpFormat, string.Join(", ", Arguments.Select(a => $"-{a}")));
}
