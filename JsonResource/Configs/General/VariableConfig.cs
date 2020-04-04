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

        [DataMember(Name = "defaultValues")]
        public string[] DefaultValues;

        [DataMember(Name = "doxyBrief")]
        public string DoxyBrief;

        [DataMember(Name = "nameAlias")]
        public string NameAlias;

        [DataMember(Name = "requirements")]
        public string[] Requirements;

        public bool IsCsvDefaultValue()
        {
            if (DefaultValues != null && DefaultValues.Length > 0)
            {
                if (DefaultValues[0].LastIndexOf(".csv") != -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
