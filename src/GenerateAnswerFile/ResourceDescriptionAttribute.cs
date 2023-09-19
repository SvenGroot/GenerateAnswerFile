using System.ComponentModel;

namespace GenerateAnswerFile;

class ResourceDescriptionAttribute : DescriptionAttribute
{
    public ResourceDescriptionAttribute(string resourceName)
        : base(Properties.Resources.ResourceManager.GetString(resourceName)!)
    {
    }
}
