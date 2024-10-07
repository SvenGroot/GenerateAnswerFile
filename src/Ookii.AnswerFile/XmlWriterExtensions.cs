using System.Xml;

namespace Ookii.AnswerFile;

static class XmlWriterExtensions
{
    public static AutoCloseElement WriteAutoCloseElement(this XmlWriter writer, string name, KeyValueList? attributes = null)
    {
        writer.WriteStartElement(name);
        if (attributes != null)
        {
            writer.WriteAttributes(attributes);
        }

        return new AutoCloseElement(writer);
    }

    public static void WriteAttributes(this XmlWriter writer, KeyValueList attributes)
    {
        foreach (var item in attributes)
        {
            var value = item.Value;
            if (value != null)
            {
                var (prefix, name) = item.Key.SplitOnce(':');
                writer.WriteAttributeString(prefix, name, null, GetValueString(value));
            }
        }
    }

    public static void WriteEmptyElement(this XmlWriter writer, string name, KeyValueList? attributes = null)
    {
        writer.WriteStartElement(name);
        if (attributes != null)
        {
            writer.WriteAttributes(attributes);
        }

        writer.WriteEndElement();
    }

    public static void WriteElements(this XmlWriter writer, KeyValueList elements)
    {
        foreach (var item in elements)
        {
            var value = item.Value;
            if (value != null)
            {
                if (item.Value is KeyValueList childElements)
                {
                    if (item.Key == "_attributes")
                    {
                        writer.WriteAttributes(childElements);
                    }
                    else
                    {
                        writer.WriteStartElement(item.Key);
                        writer.WriteElements(childElements);
                        writer.WriteEndElement();
                    }
                }
                else
                {
                    writer.WriteElementString(item.Key, GetValueString(value));
                }
            }
        }
    }

    private static string? GetValueString(object value)
    {
        if (value.GetType() == typeof(bool) || value.GetType() == typeof(bool?))
        {
            return ((bool)value ? "true" : "false");
        }

        return value.ToString();
    }
}
