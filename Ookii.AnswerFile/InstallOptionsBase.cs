namespace Ookii.AnswerFile;

public abstract class InstallOptionsBase
{
    public OptionalFeatures? OptionalFeatures { get; set; }

    protected abstract void WriteInstallElements(Generator generator);

    internal void GenerateWindowsPePass(Generator generator)
    {
        using var pass = generator.WritePassStart("windowsPE");
        generator.WriteInternationalCore(true);
        using var setup = generator.WriteComponentStart("Microsoft-Windows-Setup");
        WriteInstallElements(generator);

        generator.Writer.WriteElements(new
        {
            UserData = new
            {
                AcceptEula = "true",
                FullName = "",
                Organization = "",
                // This one chooses the edition
                ProductKey = generator.Options.ProductKey == null ? null : new
                {
                    Key = generator.Options.ProductKey,
                }
            }
        });
    }

    internal void GenerateServicingPass(Generator generator)
    {
        OptionalFeatures?.GenerateServicingPass(generator);
    }
}
