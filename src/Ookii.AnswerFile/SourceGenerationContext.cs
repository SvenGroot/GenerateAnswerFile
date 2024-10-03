using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

[JsonSerializable(typeof(AnswerFileOptions))]
[JsonSourceGenerationOptions(
    AllowTrailingCommas = true,
    PropertyNameCaseInsensitive = true,
    // Do not use WhenWritingDefault, as the default value of some properties is not the default
    // value of the type.
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    ReadCommentHandling = JsonCommentHandling.Skip,
    WriteIndented = true)]
partial class SourceGenerationContext : JsonSerializerContext
{
}
