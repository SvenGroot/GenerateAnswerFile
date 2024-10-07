using Ookii.CommandLine;
using System.ComponentModel;

namespace GenerateAnswerFile;

class ResourceUsageFooterAttribute : UsageFooterAttribute
{
    public ResourceUsageFooterAttribute(string resourceName)
        : base(Properties.Resources.ResourceManager.GetString(resourceName)!)
    {
    }
}
