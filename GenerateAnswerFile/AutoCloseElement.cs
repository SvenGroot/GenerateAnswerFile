using System.Xml;

namespace GenerateAnswerFile;

class AutoCloseElement : IDisposable
{
    private readonly XmlWriter _writer;
    
    public AutoCloseElement(XmlWriter writer)
    {
        _writer = writer;
    }

    public void Dispose()
    {
        _writer.WriteEndElement();
    }
}
