namespace Ookii.AnswerFile;

/// <summary>
/// Base class for all install options that target a specific disk and partition.
/// </summary>
/// <threadsafety instance="false" static="true"/>
public abstract class TargetedInstallOptionsBase : InstallOptionsBase
{
    /// <summary>
    /// Gets or sets the disk to which Windows will be installed.
    /// </summary>
    /// <value>
    /// The zero-based disk ID. The default value is zero.
    /// </value>
    public int DiskId { get; set; }

    /// <summary>
    /// Gets or sets the index of the Windows image to install.
    /// </summary>
    /// <value>
    /// The one-based image index, or zero to decide based on the product key.
    /// </value>
    /// <remarks>
    /// <para>
    ///   A WIM or ESD image file can contain multiple images, typically used for different SKUs
    ///   such as Home or Professional. Normally, the <see cref="GeneratorOptions.ProductKey" qualifyHint="true"/>
    ///   property is used to determine which image to install. However, for editions that are not
    ///   necessarily activated using a product key (such as those using volume licensing), you can
    ///   use the image index to select which edition to install.
    /// </para>
    /// <para>
    ///   To list the images in a WIM or ESD file, use the PowerShell <c>Get-WindowsImage</c>
    ///   command.
    /// </para>
    /// </remarks>
    public int ImageIndex { get; set; }

    /// <summary>
    /// When implemented in a derived class, gets the ID of the partition to install to.
    /// </summary>
    /// <value>
    /// The one-based partition ID.
    /// </value>
    protected abstract int TargetPartitionId { get; }

    /// <summary>
    /// Writes elements specific to this installation method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
    /// <remarks>
    /// <inheritdoc/>
    /// </remarks>
    protected override void WriteInstallElements(Generator generator)
    {
        using (var imageInstall = generator.Writer.WriteAutoCloseElement("ImageInstall"))
        using (var osImage = generator.Writer.WriteAutoCloseElement("OSImage"))
        {
            if (ImageIndex != 0)
            {
                generator.Writer.WriteElements(new KeyValueList
                {
                    { "InstallFrom", new KeyValueList
                    {
                        { "MetaData", new KeyValueList
                        {
                            { "_attributes", new KeyValueList { { "wcm:action", "add" } } },
                            { "Key", "/IMAGE/INDEX" },
                            { "Value", ImageIndex },
                        }
                        }
                    }
                    }
                });
            }

            generator.Writer.WriteElements(new KeyValueList
            {
                { "InstallTo", new KeyValueList
                {
                    { "DiskID", DiskId },
                    { "PartitionID", TargetPartitionId },
                }
                }
            });
        }

        using var diskConfiguration = generator.Writer.WriteAutoCloseElement("DiskConfiguration");
        using var disk = generator.Writer.WriteAutoCloseElement("Disk", new KeyValueList { { "wcm:action", "add" } });
        generator.Writer.WriteElementString("DiskID", DiskId.ToString());
        WriteDiskConfiguration(generator);
    }

    /// <summary>
    /// When implemented in a derived class, writes the disk configuration for this installation
    /// method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
    protected abstract void WriteDiskConfiguration(Generator generator);
}
