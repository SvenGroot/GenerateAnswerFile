using Ookii.CommandLine;
using Ookii.CommandLine.Validation;
using System.Text;

namespace GenerateAnswerFile;
class ValidateInstallMethodAttribute : ArgumentValidationWithHelpAttribute
{
    private readonly InstallMethod[] _methods;

    public ValidateInstallMethodAttribute(params InstallMethod[] methods)
    { 
        if (methods.Length == 0)
        {
            throw new ArgumentException("Need at least one method.", nameof(methods));
        }

        _methods = methods;
    }

    public override ValidationMode Mode => ValidationMode.AfterParsing;

    public override bool IsValid(CommandLineArgument argument, object? value)
    {
        var installArg = argument.Parser.GetArgument(nameof(Arguments.Install))!;
        var method = ((InstallMethod?)installArg.Value) ?? InstallMethod.PreInstalled;
        return _methods.Contains(method);
    }

    public override string GetErrorMessage(CommandLineArgument argument, object? value)
        => $"The '{argument.ArgumentName}' argument may only be used if -Install is set to {GetMethodList()}.";

    protected override string GetUsageHelpCore(CommandLineArgument argument)
        => $"May only be used if -Install is set to {GetMethodList()}.";

    private string GetMethodList()
    {
        if (_methods.Length == 1)
        {
            return _methods[0].ToString();
        }

        var result = new StringBuilder();
        result.AppendJoin(", ", _methods.Take(_methods.Length - 1));
        result.Append($", or {_methods.Last()}");
        return result.ToString();
    }
}
