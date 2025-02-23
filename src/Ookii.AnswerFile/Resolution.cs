using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ookii.AnswerFile;

/// <summary>
/// Represents a display resolution.
/// </summary>
/// <threadsafety static="true" instance="false"/>
[JsonConverter(typeof(ResolutionJsonConverter))]
public struct Resolution : ISpanParsable<Resolution>
{
    internal class ResolutionJsonConverter : JsonConverter<Resolution>
    {
        public override Resolution Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return Parse(reader.GetString()!, CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                throw new JsonException(ex.Message, ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, Resolution value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.WriteStringValue(value.ToString());
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Resolution"/> structure.
    /// </summary>
    /// <param name="width">The horizontal display resolution.</param>
    /// <param name="height">The vertical display resolution.</param>
    public Resolution(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets or sets the horizontal display resolution.
    /// </summary>
    /// <value>
    /// The horizontal display resolution, in pixels.
    /// </value>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the vertical display resolution.
    /// </summary>
    /// <value>
    /// The vertical display resolution, in pixels.
    /// </value>
    public int Height { get; set; }

    /// <summary>
    /// Parses a span of characters into a <see cref="Resolution"/> structure.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <remarks>
    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    /// </remarks>
    public static Resolution Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new FormatException(Properties.Resources.InvalidResolution);
        }

        return result;
    }

    /// <summary>
    /// Parses a string into a <see cref="Resolution"/> structure.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    /// <remarks>
    /// <para>
    ///   Resolutions are represented as a string using the format "width,height". For example,
    ///   "1920,1080" represents a resolution of 1920 by 1080 pixels.
    /// </para>
    /// </remarks>
    public static Resolution Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new FormatException(Properties.Resources.InvalidResolution);
        }

        return result;
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="Resolution"/> structure.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">Receives the result of parsing <paramref name="s"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Resolution result)
    {
        var split = s.SplitOnce(',');
        if (!split.HasValue)
        {
            result = default;
            return false;
        }

        if (!int.TryParse(split.Left, NumberStyles.Integer, provider, out var width) ||
            !int.TryParse(split.Right, NumberStyles.Integer, provider, out var height))
        {
            result = default;
            return false;
        }

        result = new Resolution
        {
            Width = width,
            Height = height
        };

        return true;
    }

    /// <summary>
    /// Parses a string into a <see cref="Resolution"/> structure.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">Receives the result of parsing <paramref name="s"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    /// </remarks>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Resolution result)
    {
        if (s == null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Returns a string representation of this instance.
    /// </summary>
    /// <returns>A string representation of this instance.</returns>
    /// <remarks>
    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    /// </remarks>
    public override readonly string ToString() => FormattableString.Invariant($"{Width},{Height}");
}
