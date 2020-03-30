using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class ClassConfig : DeclarationConfig
    {
        [DataMember(Name = "memberVariables")]
        public MemberVariableConfig[] MemberVariableConfigs;
        [DataMember(Name = "memberFunctions")]
        public MemberFunctionConfig[] MemberFunctionConfigs;
    }
    [DataContract]
    public partial class MemberVariableConfig
    {
        [DataMember(Name = "accessModifier", IsRequired = true)]
        public string AccessModifier;
        [DataMember(Name = "isDefineAccessor", IsRequired = true)]
        public bool IsDefineAccessor;
        [DataMember(Name = "isInlineAccessor", IsRequired = true)]
        public bool IsInlineAccessor;
        [DataMember(Name = "isAccessorReturnThis", IsRequired = true)]
        public bool IsAccessorReturnThis;
        [DataMember(Name = "variableDeclaration", IsRequired = true)]
        public VariableDeclarationConfig VariableDeclarationConfig;
    }
    [DataContract]
    public partial class MemberFunctionConfig
    {
        [DataMember(Name = "accessModifier", IsRequired = true)]
        public string AccessModifier;
        [DataMember(Name = "function", IsRequired = true)]
        public FunctionConfig FunctionConfig;
    }

}
