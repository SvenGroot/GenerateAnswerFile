using Ookii.CommandLine;

namespace GenerateAnswerFile;

class ResourceValueDescriptionAttribute : ValueDescriptionAttribute
{
    public ResourceValueDescriptionAttribute(string resourceName)
        : base(Properties.Resources.ResourceManager.GetString(resourceName)!)
    {
    }
}
