using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class FileConfig
    {
        [DataMember(Name = "fileName")]
        public string FileName;

        [DataMember(Name = "copyright")]
        public string Copyright;

        [DataMember(Name = "includeFiles")]
        public string[] IncludeFiles;

        [DataMember(Name = "namespace")]
        public string Namespace;

        [DataMember(Name = "definitions")]
        public string[] Definitions;
    }
}
