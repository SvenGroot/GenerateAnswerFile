using Ookii.CommandLine;

namespace GenerateAnswerFile;

class ResourceUsageFooterAttribute : UsageFooterAttribute
{
    public ResourceUsageFooterAttribute(string resourceName)
        : base(Properties.Resources.ResourceManager.GetString(resourceName)!)
    {
    }
}
