using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class RequirementSdkInfo : RequirementInfo
    {
        private static string MacroPrefix = "SDK_";
        private string[] MacroNames = new string[(int)RequirementInfo.RequirementType.Count]
        {
            "",
            "NOT_NULL",
            "LESS",
            "LESS_EQUAL",
            "MIN_MAX",
            "NOT_EQUAL",
            "// ",
            "EQUAL",
            "ALIGN",
        };
        public RequirementSdkInfo()
        {
        }
        private string GetMacroName(bool isRequirement)
        {
            string assertTypeName = "";
            if (isRequirement)
            {
                assertTypeName = "REQUIRES_";
            }
            return MacroPrefix + assertTypeName + MacroNames[(int)this.Type];
        }
        public override string GetString(string variableName, bool isRequirement)
        {
            switch (this.Type)
            {
                case RequirementInfo.RequirementType.Less:
                case RequirementInfo.RequirementType.LessEqual:
                case RequirementInfo.RequirementType.MinMax:
                case RequirementInfo.RequirementType.NotEqual:
                case RequirementInfo.RequirementType.Equal:
                case RequirementInfo.RequirementType.Align:
                    string ret = GetMacroName(isRequirement) + "(" + variableName + ", ";
                    foreach (var value in this.Values)
                    {
                        ret += value + ", ";
                    }
                    return ret.Remove(ret.Length - ", ".Length, ", ".Length) + ");";
                case RequirementInfo.RequirementType.NotNull:
                    return GetMacroName(isRequirement) + "(" + variableName + ");";
                case RequirementInfo.RequirementType.NoRequirement:
                    return GetMacroName(isRequirement);
                case RequirementInfo.RequirementType.Comment:
                    // NOTE: 1 行コメントのみに対応
                    return GetMacroName(isRequirement) + this.Values[0];
            }
            return "Invalid variableName";
        }
    }
}
