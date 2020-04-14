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
            DoxyDetails = new List<string>();
        }
        public string DoxyBrief { get; set; }
        public List<string> DoxyDetails { get; set; }
        public string Name { get; set; }
        public List<VariableInfo> MemberVariableInfos { get; set; }
        public string StructureType { get; set; }
    }
}
