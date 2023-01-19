using GenerateAnswerFile;

var arguments = Arguments.Parse();
if (arguments == null)
{
    return 1;
}

// TODO: Handle exceptions.
Generator.Generate(arguments);
return 0;
