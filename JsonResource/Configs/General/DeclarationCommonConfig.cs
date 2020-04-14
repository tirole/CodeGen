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
        [DataMember(Name = "doxyDetails")]
        public string[] DoxyDetails;
        [DataMember(Name = "nameAlias")]
        public string NameAlias;

        public string GetDoxyBrief()
        {
            if (DoxyBrief == "")
            {
                return NameAlias + "です。";
            }
            else
            {
                return DoxyBrief;
            }
        }

        public void GetDoxyDetails(List<string> outStr)
        {
            if(outStr != null)
            {
                foreach(string str in DoxyDetails)
                {
                    outStr.Add(str);
                }
            }
        }
    }
}
