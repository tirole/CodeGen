using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class DescriptorConfig : DeclarationConfig
    {
        [DataMember(Name = "memberVariables")]
        public DescriptorVariableConfig[] memberVariables;
    }

    [DataContract]
    public partial class DescriptorVariableConfig : VariableConfig
    {
        [DataMember(Name = "wordOffset", IsRequired = true)]
        public string WordOffset;

        [DataMember(Name = "bitRange", IsRequired = true)]
        public string BitRange;

        [DataMember(Name = "modifier", IsRequired = true)]
        public string Modifier;
    }
}
