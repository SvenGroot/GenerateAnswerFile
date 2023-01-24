using System.Collections.ObjectModel;

namespace Ookii.AnswerFile;

public class OptionalFeatures
{
    public OptionalFeatures(Version version)
    {
        ArgumentNullException.ThrowIfNull(version);
        WindowsVersion = version;
    }

    public Version WindowsVersion { get; }

    public Collection<string> Components { get; } = new();

    internal void GenerateServicingPass(Generator generator)
    {
        using var servicing = generator.Writer.WriteAutoCloseElement("servicing");
        using var package = generator.Writer.WriteAutoCloseElement("package", new { action = "configure" });
        generator.Writer.WriteEmptyElement("assemblyIdentity", new
        {
            name = "Microsoft-Windows-Foundation-Package",
            version = WindowsVersion,
            processorArchitecture = generator.Options.ProcessorArchitecture,
            publicKeyToken = Generator.PublicKeyToken,
            language = ""
        });

        foreach (var component in Components)
        {
            generator.Writer.WriteEmptyElement("selection", new { name = component, state = "true" });
        }
    }
}
