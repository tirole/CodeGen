using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public class SdkRequirement : Requirement
    {
        private string[] MacroNames = new string[(int)RequirementType.Count]
        {
            "",
            "SDK_NOT_NULL",
            "SDK_LESS",
            "SDK_LESS_EQUAL",
            "SDK_MIN_MAX",
        };
        public SdkRequirement(string str)
        {
            var values = str.Split(this.RequirementValueSeparater);

            bool isFoundRequirementTag = false;
            for (var reqType = RequirementType.NoRequirement; reqType < RequirementType.Count; ++reqType)
            {
                int reqTypeIndex = (int)reqType;
                if (values[0] == RequirementTags[reqTypeIndex])
                {
                    this.Type = reqType;
                    isFoundRequirementTag = true;
                    break;
                }
            }
            if (!isFoundRequirementTag)
            {
                throw new System.InvalidOperationException("Cannot find proper requirement tag.");
            }

            for (int i = 1; i < values.Length; ++i)
            {
                this.Values.Add(values[i]);
            }
        }
        private string GetString()
        {
            return MacroNames[(int)Type];
        }
        override public string GetString(string variableName)
        {
            switch (Type)
            {
                case RequirementType.Less:
                case RequirementType.LessEqual:
                case RequirementType.MinMax:
                    string ret = GetString() + "(" + variableName + ", ";
                    foreach(var value in Values)
                    {
                        ret += value + ", ";
                    }
                    return ret.Remove(ret.Length - ", ".Length, ", ".Length) + ");";
                case RequirementType.NotNull:
                    return GetString() + "(" + variableName + ");";
                case RequirementType.NoRequirement:
                    return GetString();
            }
            return "Invalid variableName";
        }
    }
}
