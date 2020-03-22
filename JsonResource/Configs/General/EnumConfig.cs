using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class EnumConfig : DeclarationConfig
    {
        [DataMember(Name = "enumBase", IsRequired = true)]
        public string EnumBase;

        [DataMember(Name = "enumerators", IsRequired = true),]
        public Enumerator[] Enumerators;
    }

    [DataContract]
    public partial class Enumerator
    {

        [DataMember(Name = "enumeratorName", IsRequired = true)]
        public string Name;

        [DataMember(Name = "value")]
        public string Value;

        [DataMember(Name = "doxyBrief")]
        public string DoxyBrief;
    }

}
