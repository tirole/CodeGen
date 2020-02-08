using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class StructConfig : DeclarationConfig
    {
        [DataMember(Name = "memberVariables")]
        public VariableConfig[] MemberVariables;
    }
}
