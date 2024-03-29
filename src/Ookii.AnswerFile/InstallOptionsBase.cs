﻿using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

/// <summary>
/// Base class for types that provide options for a specific installation method.
/// </summary>
/// <threadsafety instance="false" static="true"/>
[JsonDerivedType(typeof(CleanEfiOptions), typeDiscriminator: "CleanEfi")]
[JsonDerivedType(typeof(CleanBiosOptions), typeDiscriminator: "CleanBios")]
[JsonDerivedType(typeof(ExistingPartitionOptions), typeDiscriminator: "ExistingPartition")]
[JsonDerivedType(typeof(ManualInstallOptions), typeDiscriminator: "Manual")]
public abstract class InstallOptionsBase
{
    /// <summary>
    /// Gets or sets the optional features to install.
    /// </summary>
    /// <value>
    /// An instance of the <see cref="OptionalFeatures"/> class, or <see langword="null"/> if no
    /// optional features should be installed.
    /// </value>
    public OptionalFeatures? OptionalFeatures { get; set; }

    /// <summary>
    /// When implemented in a derived class, writes options specific to the install method.
    /// </summary>
    /// <param name="generator">The generator creating the answer file.</param>
    /// <remarks>
    /// <para>
    ///   This method is called when generating the Microsoft-Windows-Setup component of the
    ///   windowsPE pass.
    /// </para>
    /// </remarks>
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
