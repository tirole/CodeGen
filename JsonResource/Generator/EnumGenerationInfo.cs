using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class EnumGenerationInfo
    {
        public EnumGenerationInfo()
        {
            EnumeratorInfos = new List<EnumeratorInfo>();
        }
        public string DoxyBrief { get; set; }
        public string EnumKey { get; set; }
        public string EnumBase { get; set; }
        public string EnumName { get; set; }
        public List<EnumeratorInfo> EnumeratorInfos { get; set; }
    }

    public class EnumeratorInfo
    {
        public string Name;
        public string Value;
        public string DoxyBrief;
    }
}
