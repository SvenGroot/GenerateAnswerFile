﻿using Ookii.CommandLine;
using Ookii.CommandLine.Validation;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GenerateAnswerFile;

class CustomUsageWriter : UsageWriter
{
    public CustomUsageWriter(LineWrappingTextWriter? writer = null)
        : base(writer)
    {
        UseAbbreviatedSyntax = true;
        IndentAfterEmptyLine = true;
    }

    public bool Markdown { get; set; }

    protected override void WriteParserUsageFooter()
    {
        if (!Markdown)
        {
            // This done here and not with the UsageFooterAttribute because we need to add the
            // executable name.
            WriteLine(string.Format(CultureInfo.CurrentCulture, Properties.Resources.UsageHelpFooterFormat, ExecutableName));
            WriteLine();
        }
    }

    protected override void WriteMoreInfoMessage()
    {
        WriteLine(string.Format(CultureInfo.CurrentCulture, Properties.Resources.UsageHelpMoreInfoFormat, ExecutableName));
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
                    Writer.ResetIndent();
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

#if DEBUG

    protected override void WriteParserUsageSyntax()
    {
        if (!Markdown)
        {
            base.WriteParserUsageSyntax();
            return;
        }

        SetIndent(0);
        WriteLine("## Usage syntax");
        WriteLine();
        WriteLine("<!-- markdownlint-disable MD033 -->");
        Write($"<pre>{ExecutableName}");
        SetIndent(4);
        foreach (var arg in GetArgumentsInUsageOrder())
        {
            WriteLine();
            if (arg.IsRequired)
            {
                WriteArgumentSyntax(arg);
            }
            else
            {
                WriteOptionalArgumentSyntax(arg);
            }
        }

        WriteLine("</pre>");
        Writer.ResetIndent();
        WriteLine("<!-- markdownlint-enable MD033 -->");
        WriteLine();
    }

    protected override void WriteArgumentName(string argumentName, string prefix)
    {
        if (!Markdown)
        { 
            base.WriteArgumentName(argumentName, prefix);
            return; 
        }

        Write($"<a href=\"#-{argumentName.ToLowerInvariant()}\">{prefix}{argumentName}</a>");
    }

    protected override void WriteValueDescription(string valueDescription)
    {
        if (!Markdown)
        {
            base.WriteValueDescription(valueDescription);
            return;
        }

        Write($"&lt;{valueDescription}&gt;");
    }

#endif


    private void WriteArgumentMarkdown(CommandLineArgument argument)
    {
#if DEBUG

        WriteLine($"### `-{argument.ArgumentName}`");
        WriteLine();

        var json = JsonDocument.Parse(File.OpenRead("mdhelp.json"));
        string? description = null;
        string? additionalDescription = null;
        if (json.RootElement.TryGetProperty(argument.ArgumentName, out var argumentInfo))
        {
            if (argumentInfo.ValueKind == JsonValueKind.Object)
            {
                if (argumentInfo.GetProperty("Override").GetBoolean())
                {
                    description = argumentInfo.GetProperty("Text").GetString();
                }
                else
                {
                    additionalDescription = argumentInfo.GetProperty("Text").GetString();
                }
            }
            else
            {
                additionalDescription = argumentInfo.GetString();
            }
        }

        if (description == null)
        {
            // Put backticks around install method values.
            var values = string.Join("|", typeof(InstallMethod).GetEnumNames());
            description = Regex.Replace(argument.Description, $@"(?<!')\b({values})\b(?!')", "`$1`").ReplaceLineEndings();

            // Replace single quoted terms with backticks (but avoid changing apostrophes).
            description = Regex.Replace(description, @"(^|\s)'([^']*)'(\s|$|\.|,|\))", "$1`$2`$3");

            // Make links for arguments.
            description = Regex.Replace(description, @"(?<=^|\s)-([A-Z][a-zA-Z]*)(?=\s|$|\.|,|\))",
                m => $"[`-{m.Groups[1]}`](#-{m.Groups[1].Value.ToLowerInvariant()})");
        }

        WriteLine(description);
        WriteLine();
        foreach (var validator in argument.Validators)
        {
            if (validator is not (RequiresAttribute or ProhibitsAttribute or RequiresAnyOtherAttribute or 
                    ValidateInstallMethodAttribute or ValidateEnumValueAttribute))
            {
                var help = validator.GetUsageHelp(argument);
                if (!string.IsNullOrEmpty(help))
                {
                    WriteLine(help);
                    WriteLine();
                }
            }
        }

        if (additionalDescription != null)
        {
            WriteLine(additionalDescription);
            WriteLine();
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
            var plural = argument.Aliases.Length > 1 ? "es" : "";
            WriteLine($"Alias{plural}: {string.Join(", ", argument.Aliases.Select(a => prefix + a))}");
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
            var plural = requiresAttribute.Arguments.Length > 1 ? "s" : "";
            WriteLine($"Required argument{plural}: {string.Join(", ", requiresAttribute.Arguments.Select(a => prefix + a))}");
        }

        var requiresAnyOtherAttribute = argument.Validators.OfType<RequiresAnyOtherAttribute>().FirstOrDefault();
        if (requiresAnyOtherAttribute != null)
        {
            WriteLine($"Requires one of: {string.Join(", ", requiresAnyOtherAttribute.Arguments.Select(a => prefix + a))}");
        }

        var prohibitsAttribute = argument.Validators.OfType<ProhibitsAttribute>().FirstOrDefault();
        if (prohibitsAttribute != null)
        {
            var plural = prohibitsAttribute.Arguments.Length > 1 ? "s" : "";
            WriteLine($"Prohibited argument{plural}: {string.Join(", ", prohibitsAttribute.Arguments.Select(a => prefix + a))}");
        }

        var installMethodAttribute = argument.Validators.OfType<ValidateInstallMethodAttribute>().FirstOrDefault();
        if (installMethodAttribute != null)
        {
            var plural = installMethodAttribute.Methods.Length > 1 ? "s" : "";
            WriteLine($"Allowed -Install value{plural}: {string.Join(", ", installMethodAttribute.Methods)}");
        }

        WriteLine("```");
        WriteLine();

#endif
    }

    private static ArgumentCategory? GetCategory(CommandLineArgument argument)
        => argument.Member?.GetCustomAttribute<ArgumentCategoryAttribute>()?.Category;

    private static string GetCategoryDescription(ArgumentCategory category)
    {
        return category switch
        {
            ArgumentCategory.Install => Properties.Resources.CategoryInstall,
            ArgumentCategory.UserAccounts => Properties.Resources.CategoryUserAccounts,
            ArgumentCategory.Domain => Properties.Resources.CategoryDomain,
            _ => Properties.Resources.CategoryOther,
        };
    }
}
