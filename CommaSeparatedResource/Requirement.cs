using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public abstract class Requirement
    {
        public Requirement()
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
            Count,
        };

        public string[] RequirementTags = new string[(int)RequirementType.Count]
        {
            "",
            "NOTNULL",
            "L",
            "LE",
            "MINMAX",
        };
        public char RequirementValueSeparater = ':';
        public RequirementType Type { get; set; }
        public List<string> Values { get; set; }
        public abstract string GetString(string variableName);
    }
}
