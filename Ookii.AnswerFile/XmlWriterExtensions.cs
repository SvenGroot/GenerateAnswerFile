using System.Xml;

namespace Ookii.AnswerFile;

static class XmlWriterExtensions
{
    public static AutoCloseElement WriteAutoCloseElement(this XmlWriter writer, string name, object? attributes = null)
    {
        writer.WriteStartElement(name);
        if (attributes != null)
        {
            writer.WriteAttributes(attributes);
        }

        return new AutoCloseElement(writer);
    }

    public static void WriteAttributes(this XmlWriter writer, object attributes)
    {
        foreach (var property in attributes.GetType().GetProperties())
        {
            var value = property.GetValue(attributes);
            if (value != null)
            {
                var (prefix, name) = property.Name.SplitOnce('_');
                string? valueString;
                if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    valueString = ((bool)value ? "true" : "false");
                }
                else
                {
                    valueString = value.ToString();
                }

                writer.WriteAttributeString(prefix, name, null, valueString);
            }
        }
    }

    public static void WriteEmptyElement(this XmlWriter writer, string name, object? attributes = null)
    {
        writer.WriteStartElement(name);
        if (attributes != null)
        {
            writer.WriteAttributes(attributes);
        }

        writer.WriteEndElement();
    }

    public static void WriteElements(this XmlWriter writer, object elements)
    {
        foreach (var property in elements.GetType().GetProperties())
        {
            var value = property.GetValue(elements);
            if (value != null)
            {
                if (property.Name == "_attributes")
                {
                    writer.WriteAttributes(value);
                }
                else if (property.PropertyType.Name.StartsWith("<>"))
                {
                    writer.WriteStartElement(property.Name);
                    writer.WriteElements(value);
                    writer.WriteEndElement();
                }
                else
                {
                    string? valueString;
                    if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                    {
                        valueString = ((bool)value ? "true" : "false");
                    }
                    else
                    {
                        valueString = value.ToString();
                    }

                    writer.WriteElementString(property.Name, valueString);
                }
            }
        }
    }
}
