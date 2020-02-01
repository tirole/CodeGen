using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class DescriptorConfig
    {
        [DataMember(Name = "doxyBrief")]
        public string DoxyBrief;

        [DataMember(Name = "definitionType")]
        public string DefinitionType;
        [DataMember(Name = "memberVariables")]
        public DescriptorVariableConfig[] memberVariables;
    }

    [DataContract]
    public partial class DescriptorVariableConfig : VariableConfig
    {
        [DataMember(Name = "wordOffset")]
        public string WordOffset;

        [DataMember(Name = "bitRange")]
        public string BitRange;
    }
}
