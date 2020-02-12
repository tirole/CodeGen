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
        [DataMember(Name = "returnType")]
        public string ReturnType;
        [DataMember(Name = "arguments", IsRequired = true)]
        public VariableConfig[] ArgumentConfigs;
    }
}
