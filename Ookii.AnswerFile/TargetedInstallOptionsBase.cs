namespace Ookii.AnswerFile;

public abstract class TargetedInstallOptionsBase : InstallOptionsBase
{
    public int DiskId { get; set; }

    public int ImageIndex { get; set; }

    protected abstract int TargetPartitionId { get; }

    protected override void WriteInstallElements(Generator generator)
    {
        using (var imageInstall = generator.Writer.WriteAutoCloseElement("ImageInstall"))
        using (var osImage = generator.Writer.WriteAutoCloseElement("OSImage"))
        {
            if (ImageIndex != 0)
            {
                generator.Writer.WriteElements(new
                {
                    InstallFrom = new
                    {
                        MetaData = new
                        {
                            _attributes = new { wcm_action = "add" },
                            Key = "/IMAGE/INDEX",
                            Value = ImageIndex
                        }
                    }
                });
            }

            generator.Writer.WriteElements(new
            {
                InstallTo = new
                {
                    DiskID = DiskId,
                    PartitionID = TargetPartitionId
                }
            });
        }

        using var diskConfiguration = generator.Writer.WriteAutoCloseElement("DiskConfiguration");
        using var disk = generator.Writer.WriteAutoCloseElement("Disk", new { wcm_action = "add" });
        generator.Writer.WriteElementString("DiskID", DiskId.ToString());
        WriteDiskConfiguration(generator);
    }

    protected abstract void WriteDiskConfiguration(Generator generator);
}
