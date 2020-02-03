using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    // Type created for JSON at <<root>>
    [DataContract]
    public partial class StructConfig
    {
        [DataMember(Name = "doxyBrief")]
        public string DoxyBrief;
        [DataMember(Name = "definitionType", IsRequired = true)]
        public string DefinitionType;
        [DataMember(Name = "definitionName", IsRequired = true)]
        public string DefinitionName;
        [DataMember(Name = "memberVariables")]
        public VariableConfig[] MemberVariables;
    }
}
