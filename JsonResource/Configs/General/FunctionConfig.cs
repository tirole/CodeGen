using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class FunctionConfig
    {
        [DataMember(Name = "functionName", IsRequired = true)]
        public string FunctionName;
        [DataMember(Name = "isInline", IsRequired = true)]
        public bool IsInline;
        [DataMember(Name = "doxyBrief", IsRequired = true)]
        public string DoxyBrief;
        [DataMember(Name = "doxyDetails")]
        public string[] DoxyDetails;
        [DataMember(Name = "returnType")]
        public string ReturnType;
        [DataMember(Name = "arguments", IsRequired = true)]
        public VariableDeclarationConfig[] ArgumentConfigs;

        public string GetDoxyBrief()
        {
            return DeserializeUtility.GetDoxyBrief(DoxyBrief, FunctionName);
        }

        public void GetDoxyDetails(List<string> outStr)
        {
            DeserializeUtility.GetDoxyDetails(outStr, DoxyDetails);
        }
    }
}
