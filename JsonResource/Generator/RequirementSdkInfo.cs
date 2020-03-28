﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class RequirementSdkInfo : RequirementInfo
    {
        private string[] MacroNames = new string[(int)RequirementInfo.RequirementType.Count]
        {
            "",
            "SDK_NOT_NULL",
            "SDK_LESS",
            "SDK_LESS_EQUAL",
            "SDK_MIN_MAX",
            "SDK_NOT_EQUAL",
            "// ",
            "SDK_EQUAL",
            "SDK_ALIGN",
        };
        public RequirementSdkInfo()
        {
        }
        private string GetMacroName()
        {
            return MacroNames[(int)this.Type];
        }
        public override string GetString(string variableName)
        {
            switch (this.Type)
            {
                case RequirementInfo.RequirementType.Less:
                case RequirementInfo.RequirementType.LessEqual:
                case RequirementInfo.RequirementType.MinMax:
                case RequirementInfo.RequirementType.NotEqual:
                case RequirementInfo.RequirementType.Equal:
                case RequirementInfo.RequirementType.Align:
                    string ret = GetMacroName() + "(" + variableName + ", ";
                    foreach (var value in this.Values)
                    {
                        ret += value + ", ";
                    }
                    return ret.Remove(ret.Length - ", ".Length, ", ".Length) + ");";
                case RequirementInfo.RequirementType.NotNull:
                    return GetMacroName() + "(" + variableName + ");";
                case RequirementInfo.RequirementType.NoRequirement:
                    return GetMacroName();
                case RequirementInfo.RequirementType.Comment:
                    // NOTE: 1 行コメントのみに対応
                    return GetMacroName() + this.Values[0];
            }
            return "Invalid variableName";
        }
    }
}
