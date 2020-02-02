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

        [DataMember(Name = "customDeserializerConfigs")]
        public CustomDeserializerConfig[] CustomDeserializerConfigs;
    }

    [DataContract]
    public partial class CustomDeserializerConfig
    {
        [DataMember(Name = "definitionName", IsRequired = true)]
        public string DefinitionName;
        [DataMember(Name = "deserializerType", IsRequired = true)]
        public string DeserializerType;
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
        public DefinitionConfig[] DefinitionConfigs;
    }

    [DataContract]
    public partial class DefinitionConfig
    {
        [DataMember(Name = "definitionName", IsRequired = true)]
        public string DefinitionName;
        [DataMember(Name = "path", IsRequired = true)]
        public string Path;
    }
}
