using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class VariableConfig
    {

        [DataMember(Name = "variableName", IsRequired = true)]
        public string VariableName;

        [DataMember(Name = "type", IsRequired = true)]
        public string Type;

        [DataMember(Name = "defaultValue")]
        public string DefaultValue;

        [DataMember(Name = "doxyBrief")]
        public string DoxyBrief;

        [DataMember(Name = "nameAlias")]
        public string NameAlias;

        [DataMember(Name = "requirements")]
        public string[] Requirements;
    }
}
