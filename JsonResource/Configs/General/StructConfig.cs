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
    public partial class StructConfig : DeclarationConfig
    {
        [DataMember(Name = "memberVariables")]
        public VariableConfig[] MemberVariables;
    }
}
