using GenerateAnswerFile;

var arguments = Arguments.Parse();
if (arguments == null)
{
    return 1;
}

// TODO: Handle exceptions.
Ookii.AnswerFile.Generator.Generate(arguments.OutputFile.FullName, arguments.ToOptions());
return 0;
