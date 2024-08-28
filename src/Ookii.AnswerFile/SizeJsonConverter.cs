using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

// A custom converter that serializes Size as a string in the format "width,height".
// The default JSON converter for Size serializes it as an object with Width and Height properties,
// and likes to also include the "IsEmpty" property which is not needed.
internal class SizeJsonConverter : JsonConverter<Size>
{
    public override Size Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var (width, height) = reader.GetString()!.SplitOnce(',');
        if (width == null)
        {
            throw new JsonException(Properties.Resources.InvalidSize);
        }

        try
        {
            return new Size(int.Parse(width), int.Parse(height));
        }
        catch (FormatException ex)
        {
            throw new JsonException(Properties.Resources.InvalidSize, ex);
        }
    }

    public override void Write(Utf8JsonWriter writer, Size value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStringValue(FormattableString.Invariant($"{value.Width},{value.Height}"));
    }
}
