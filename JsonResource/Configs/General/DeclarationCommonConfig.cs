using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class DeclarationConfig
    {
        [DataMember(Name = "declaration")]
        public DeclarationCommonConfig Declaration;
    }
    [DataContract]
    public partial class DeclarationCommonConfig
    {
        [DataMember(Name = "definitionType", IsRequired = true)]
        public string DefinitionType;
        [DataMember(Name = "definitionName", IsRequired = true)]
        public string DefinitionName;
        [DataMember(Name = "doxyBrief")]
        public string DoxyBrief;
        [DataMember(Name = "nameAlias")]
        public string NameAlias;
    }
}
