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
        public string DeclarationPrefix { get; set; }
        //abstract public string GetString();
    }
}
