using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class StructGenerationInfo
    {
        public StructGenerationInfo()
        {
            MemberVariableInfos = new List<VariableInfo>();
            StructureType = "struct";
        }
        public string Name { get; set; }
        public List<VariableInfo> MemberVariableInfos { get; set; }
        public string StructureType { get; set; }
    }
}
