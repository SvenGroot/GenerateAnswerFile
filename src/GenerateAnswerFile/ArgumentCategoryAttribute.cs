using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateAnswerFile;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
class ArgumentCategoryAttribute : Attribute
{
    public ArgumentCategoryAttribute(ArgumentCategory category)
    {
        Category = category;
    }

    public ArgumentCategory Category { get; }
}
