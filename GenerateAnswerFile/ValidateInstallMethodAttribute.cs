using Ookii.CommandLine;
using Ookii.CommandLine.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateAnswerFile;
class ValidateInstallMethodAttribute : ArgumentValidationWithHelpAttribute
{
    private readonly InstallMethod[] _methods;

    public ValidateInstallMethodAttribute(params InstallMethod[] methods)
    { 
        _methods = methods;
    }

    public override ValidationMode Mode => ValidationMode.AfterParsing;

    public override bool IsValid(CommandLineArgument argument, object? value)
    {
        var installArg = argument.Parser.GetArgument(nameof(Arguments.Install))!;
        var method = ((InstallMethod?)installArg.Value) ?? InstallMethod.PreInstalled;
        return _methods.Contains(method);
    }

    protected override string GetUsageHelpCore(CommandLineArgument argument)
        => $"May only be used if -Install is set to {string.Join(", ", _methods)}.";

    public override string GetErrorMessage(CommandLineArgument argument, object? value)
        => $"The '{argument.ArgumentName}' argument may only be used if -Install is set to {string.Join(", ", _methods)}.";
}
