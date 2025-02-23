using System.Xml;

namespace Ookii.AnswerFile;

class AutoCloseElement : IDisposable
{
    private readonly XmlWriter _writer;

    public AutoCloseElement(XmlWriter writer)
    {
        _writer = writer;
    }

    public void Dispose()
    {
        try
        {
            _writer.WriteEndElement();
        }
        catch
        {
            // Ignore exceptions, as this can hide other exceptions.
        }
    }
}
