using Ookii.CommandLine;
using Ookii.CommandLine.Validation;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GenerateAnswerFile;

class CustomUsageWriter : UsageWriter
{
    public bool Markdown { get; set; }

    protected override void WriteParserUsageCore(UsageHelpRequest request)
    {
        base.WriteParserUsageCore(request);
        if (request == UsageHelpRequest.Full && !Markdown)
        {
            Write(string.Format(CultureInfo.CurrentCulture, Properties.Resources.UsageHelpFooterFormat, ExecutableName));
            WriteLine();
            WriteLine();
        }
    }

    protected override void WriteUsageSyntaxPrefix()
    {
        if (Markdown)
        {
            SetIndent(0);
            WriteLine("## Usage syntax");
            WriteLine();
            WriteLine("```text");
            SetIndent(3);
            Write(ExecutableName);
        }
        else
        {
            base.WriteUsageSyntaxPrefix();
        }
    }

    protected override void WriteUsageSyntaxSuffix()
    {
        if (Markdown)
        {
            WriteLine();
            Writer.ResetIndent();
            Write("```");
        }
        else 
        {
            base.WriteUsageSyntaxSuffix(); 
        }
    }

    protected override void WriteArgumentDescriptions()
    {
        var groups = Parser.Arguments.GroupBy(a => GetCategory(a)).OrderBy(c => c.Key);
        foreach (var group in groups)
        {
            if (group.Key is ArgumentCategory category)
            {
                if (Markdown)
                {
                    WriteLine($"## {GetCategoryDescription(category).TrimEnd(':')}");
                    WriteLine();
                }
                else
                {
                    WriteColor(UsagePrefixColor);
                    Write(GetCategoryDescription(category));
                    ResetColor();
                    WriteLine();
                    WriteLine();
                }
            }
            else if (Markdown)
            {
                WriteLine("## General options");
                WriteLine();
            }

            if (ShouldIndent && !Markdown)
            {
                Writer.Indent = ArgumentDescriptionIndent;
            }

            foreach (var argument in group)
            {
                if (!argument.IsHidden)
                {
                    if (Markdown)
                    {
                        WriteArgumentMarkdown(argument);
                    }
                    else
                    {
                        WriteArgumentDescription(argument);
                    }
                }
            }
        }
    }

    private void WriteArgumentMarkdown(CommandLineArgument argument)
    {
#if DEBUG

        WriteLine($"### `-{argument.ArgumentName}`");
        WriteLine();

        // Put backticks around install method values.
        var values = string.Join("|", typeof(InstallMethod).GetEnumNames());
        var description = Regex.Replace(argument.Description, $@"(?<!')\b({values})\b(?!')", "`$1`").ReplaceLineEndings();

        // Replace single quotes terms with backticks (but avoid changing apostrophes).
        description = Regex.Replace(description, @"(^|\s)'([^']*)'(\s|$|\.|,|\))", "$1`$2`$3");

        // Make links for arguments.
        description = Regex.Replace(description, @"(?<=^|\s)-([A-Z][a-zA-Z]*)(?=\s|$|\.|,|\))",
            m => $"[`-{m.Groups[1]}`](#{m.Groups[1].Value.ToLowerInvariant()})");

        description = description.Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine);
        WriteLine(description);
        WriteLine();
        foreach (var validator in argument.Validators)
        {
            if (validator is not (RequiresAttribute or ValidateInstallMethodAttribute or ValidateEnumValueAttribute))
            {
                var help = validator.GetUsageHelp(argument);
                if (!string.IsNullOrEmpty(help))
                {
                    WriteLine(help);
                    WriteLine();
                }
            }
        }

        // Since GitHub markdown doesn't support tables without header rows, we take a page out of
        // platyps's book and use yaml to get some table-like highlighting.
        WriteLine("```yaml");
        Write("Value: ");
        if (argument.ArgumentType.IsEnum)
        {
            Write(string.Join(", ", Enum.GetNames(argument.ArgumentType)));
        }
        else if (argument.IsSwitch)
        {
            WriteSwitchValueDescription(argument.ValueDescription);
        }
        else
        {
            WriteValueDescriptionForDescription(argument.ValueDescription);
        }

        if (argument.MultiValueInfo != null)
        {
            Write(" (multiple allowed)");
        }

        WriteLine();
        var prefix = Parser.ArgumentNamePrefixes[0];
        if (argument.Aliases.Length > 0)
        {
            Write($"Aliases: {string.Join(", ", argument.Aliases.Select(a => prefix + a))}");
            WriteLine();
        }

        if (argument.IsRequired)
        {
            WriteLine("Required: True");
        }

        if (argument.Position != null)
        {
            WriteLine("Positional: True");
        }

        if (argument.DefaultValue != null)
        {
            WriteLine($"Default value: {argument.DefaultValue}");
        }

        var requiresAttribute = argument.Validators.OfType<RequiresAttribute>().FirstOrDefault();
        if (requiresAttribute != null)
        {
            WriteLine($"Required arguments: {string.Join(", ", requiresAttribute.Arguments.Select(a => prefix + a))}");
        }

        var installMethodAttribute = argument.Validators.OfType<ValidateInstallMethodAttribute>().FirstOrDefault();
        if (installMethodAttribute != null)
        {
            WriteLine($"Allowed -Install values: {string.Join(", ", installMethodAttribute.Methods)}");
        }

        WriteLine("```");
        WriteLine();

#endif
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

    private void WriteLine(string text)
    {
        Write(text);
        WriteLine();
    }
}
