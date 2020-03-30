using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class VariableDeclarationsConfig : DeclarationConfig
    {
        [DataMember(Name = "variableDeclarations", IsRequired = true)]
        public VariableDeclarationConfig[] VariableDeclarationConfigs;
    }
    [DataContract]
    public partial class VariableDeclarationConfig
    {
        [DataMember(Name = "isAssignDefaultValue")]
        public bool IsAssignDefaultValue;

        [DataMember(Name = "typePrefix")]
        public string TypePrefix;
        [DataMember(Name = "typeSuffix")]
        public string TypeSuffix;
        [DataMember(Name = "variable")]
        public VariableConfig VariableConfig;
    }
}
