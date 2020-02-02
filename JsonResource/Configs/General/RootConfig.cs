using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class RootConfig
    {
        [DataMember(Name = "commonFileConfig")]
        public CommonFileConfig CommonFileConfig;

        [DataMember(Name = "fileConfigs")]
        public FileConfig[] FileConfigs;
    }

    [DataContract]
    public partial class CommonFileConfig
    {
        [DataMember(Name = "copyright")]
        public string Copyright;

        [DataMember(Name = "includeFiles")]
        public string[] IncludeFiles;

        [DataMember(Name = "namespace")]
        public string Namespace;
    }
    [DataContract]
    public partial class FileConfig
    {
        [DataMember(Name = "outputFileName")]
        public string OutputFileName;

        [DataMember(Name = "includeFiles")]
        public string[] IncludeFiles;

        [DataMember(Name = "namespace")]
        public string Namespace;

        [DataMember(Name = "definitions")]
        public string[] Definitions;
    }
}
