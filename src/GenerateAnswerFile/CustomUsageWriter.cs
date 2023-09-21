using Ookii.CommandLine;
using System.Globalization;
using System.Reflection;

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

    protected override void WriteArgumentDescriptions()
    {
        var groups = Parser.Arguments.GroupBy(a => GetCategory(a)).OrderBy(c => c.Key);
        foreach (var group in groups)
        {
            if (group.Key is ArgumentCategory category)
            {
                WriteColor(UsagePrefixColor);
                Write(GetCategoryDescription(category));
                ResetColor();
                WriteLine();
                WriteLine();
            }

            if (ShouldIndent)
            {
                Writer.Indent = ArgumentDescriptionIndent;
            }

            foreach (var argument in group)
            {
                if (!argument.IsHidden)
                {
                    WriteArgumentDescription(argument);
                }
            }
        }
    }

    private static ArgumentCategory? GetCategory(CommandLineArgument argument)
        => argument.Parser.ArgumentsType.GetMember(argument.MemberName).FirstOrDefault()
                ?.GetCustomAttribute<ArgumentCategoryAttribute>()?.Category;

    private static string GetCategoryDescription(ArgumentCategory category)
    {
        return category switch
        {
            ArgumentCategory.Install => Properties.Resources.CategoryInstall,
            ArgumentCategory.UserAccounts => Properties.Resources.CategoryUserAccounts,
            ArgumentCategory.AutoLogon => Properties.Resources.CategoryAutoLogon,
            ArgumentCategory.Domain => Properties.Resources.CategoryDomain,
            _ => Properties.Resources.CategoryOther,
        };
    }
}
