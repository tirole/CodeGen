using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public abstract class RequirementInfo
    {
        public RequirementInfo()
        {
            Values = new List<string>();
        }
        public enum RequirementType
        {
            NoRequirement,
            NotNull,
            Less,
            LessEqual,
            MinMax,
            NotEqual,
            Comment,
            Equal,
            Align,
            Count,
        };

        public static string[] RequirementTags = new string[(int)RequirementType.Count]
        {
            "",
            "NOTNULL",
            "L",
            "LE",
            "MINMAX",
            "NE",
            "COM",
            "EQ",
            "ALIGN",
        };
        public char RequirementValueSeparater = ',';
        public RequirementType Type { get; set; }
        public List<string> Values { get; set; }
        public abstract string GetString(string variableName, bool isRequirement);

        public static RequirementType GetRequirementType(string str)
        {
            for(int i = 0; i < RequirementTags.Length; ++i)
            {
                if(RequirementTags[i] == str)
                {
                    return (RequirementType)i;
                }
            }
            throw new System.InvalidOperationException("Couldn't find RequirementType.");
        }
    }
}
