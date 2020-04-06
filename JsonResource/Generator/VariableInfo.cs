using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class VariableInfo
    {
        public char BitWidthDeclarationSeparater = ':';
        public VariableInfo()
        {
            HasBitWidthDeclaration = false;
        }
        public string Type { get; set; }
        public int ArrayLength { get; set; }
        public string VariableName { get; set; }
        public string NameAlias { get; set; }
        public string DoxyBrief { get; set; }
        public string DefaultValue { get; set; }
        public List<RequirementInfo> RequirementInfos { get; set; }
        public bool HasBitWidthDeclaration;
        public int BitBegin { get; set; }
        public int BitEnd { get; set; }
        public int OffsetIn4ByteUnit { get; set; }
        // Get/Set 時に値に Modifier を加えます。今の所 DescriptorConfig でしか利用していない。
        public string Modifier { get; set; }
        public bool IsAssignDefaultValue { get; set; }
        public string TypePrefix { get; set; }
        public string TypeSuffix { get; set; }
        //abstract public string GetString();

        public enum ModifierType
        {
            Add,
            Minus,
            Mul,
            Func,
            RightShift,
            Count
        };
        public string[] Modifiers = new string[(int)ModifierType.Count]
        {
            "add",
            "minus",
            "mul",
            "func",
            "rshift",
        };

        public string InputTempVariableName = "inputVal";
        public string OutputTempVariableName = "outputVal";

        public bool HasModifier()
        {
            if (Modifier != null && Modifier != "")
            {
                return true;
            }
            return false;
        }

        public string GetModifierString(string tempVariableName)
        {
            var modifierStrings = Modifier.Split(',');
            string modifierName = modifierStrings[0];
            string modifierVal = modifierStrings[1];
            string retString = "";
            switch (modifierName)
            {
                case "add":
                    retString = tempVariableName + " += " + modifierVal;
                    break;
                case "minus":
                    retString = tempVariableName + " -= " + modifierVal;
                    break;
                case "mul":
                    retString = tempVariableName + " *= " + modifierVal;
                    break;
                case "func":
                    retString = tempVariableName + " = " + modifierVal + "(" + tempVariableName + ")";
                    break;
                case "rshift":
                    retString = tempVariableName + " >>= " + modifierVal;
                    break;
                default:
                    throw new System.InvalidOperationException("Illegal modifier type.");
            }

            return retString;
        }

        public string GetModifierString()
        {
            return GetModifierString(InputTempVariableName);
        }
    }
}
