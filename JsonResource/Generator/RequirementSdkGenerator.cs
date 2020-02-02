using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class RequirementSdkGenerator
    {
        private string[] MacroNames = new string[(int)RequirementInfo.RequirementType.Count]
        {
            "",
            "SDK_NOT_NULL",
            "SDK_LESS",
            "SDK_LESS_EQUAL",
            "SDK_MIN_MAX",
        };
        private RequirementInfo Info;
        public RequirementSdkGenerator(RequirementInfo info)
        {
            this.Info = info;
        }
        private string GetMacroName()
        {
            return MacroNames[(int)this.Info.Type];
        }
        public string GetString(string variableName)
        {
            switch (this.Info.Type)
            {
                case RequirementInfo.RequirementType.Less:
                case RequirementInfo.RequirementType.LessEqual:
                case RequirementInfo.RequirementType.MinMax:
                    string ret = GetMacroName() + "(" + variableName + ", ";
                    foreach (var value in this.Info.Values)
                    {
                        ret += value + ", ";
                    }
                    return ret.Remove(ret.Length - ", ".Length, ", ".Length) + ");";
                case RequirementInfo.RequirementType.NotNull:
                    return GetMacroName() + "(" + variableName + ");";
                case RequirementInfo.RequirementType.NoRequirement:
                    return GetMacroName();
            }
            return "Invalid variableName";
        }
    }
}
