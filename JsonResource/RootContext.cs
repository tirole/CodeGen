using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    class FileContext
    {
        public FileContext()
        {
            FileConfig = new FileConfig();
            DescriptorConfigs = new List<DescriptorConfig>();
        }
        public FileConfig FileConfig;
        public List<DescriptorConfig> DescriptorConfigs;
    }
    class RootContext
    {
        public string RootConfigDirectoryName;
        public string Copyright;
        public RootConfig RootConfig;
        public List<FileContext> FileContexts;
        public RootContext(string rootConfigFilePath)
        {
            // RootConfig
            {
                RootConfigDirectoryName = Path.GetDirectoryName(rootConfigFilePath);
                var rootConfigStream = new StreamReader(rootConfigFilePath);
                var rootConfigSerializer = new DataContractJsonSerializer(typeof(RootConfig));
                RootConfig = (RootConfig)rootConfigSerializer.ReadObject(rootConfigStream.BaseStream);
                rootConfigStream.Close();

                string copyrightFilePath = RootConfigDirectoryName + "/" + RootConfig.CommonFileConfig.Copyright;
                var copyrightStream = new StreamReader(copyrightFilePath);
                Copyright = copyrightStream.ReadToEnd();
            }

            // FileContext
            {
                FileContexts = new List<FileContext>();

                foreach (var fileConfig in RootConfig.FileConfigs)
                {
                    FileContext fileContext = new FileContext();
                    fileContext.FileConfig = fileConfig;
                    foreach (var definitionConfig in fileConfig.DefinitionConfigs)
                    {
                        string definitionConfigFilePath = RootConfigDirectoryName + "/" + definitionConfig;
                        var sr = new StreamReader(definitionConfigFilePath);
                        var descriptorSerializer = new DataContractJsonSerializer(typeof(DescriptorConfig));
                        var descriptorConfig = (DescriptorConfig)descriptorSerializer.ReadObject(sr.BaseStream);
                        fileContext.DescriptorConfigs.Add(descriptorConfig);
                    }
                    this.FileContexts.Add(fileContext);
                }
            }
        }
        public Type GetGenericDeserializerType(string definitionType)
        {
            Tuple<string, Type>[] deserializerTypes =    
            { 
                new Tuple<string, Type>("enum", typeof(EnumConfig)) ,
                new Tuple<string, Type>("struct", typeof(StructConfig)),
            };
            foreach (var deserializerType in deserializerTypes)
            {
                if (deserializerType.Item1 == definitionType)
                {
                    return deserializerType.Item2;
                }
            }
            throw new System.InvalidOperationException("Couldn't find custom deserializer type.");
        }
        public Type GetCustomDeserializerType(string deserializerName)
        {
            Tuple<string, Type>[] deserializerTypes =
            { 
                new Tuple<string, Type>("DescriptorConfig", typeof(DescriptorConfig)) 
            };
            foreach (var deserializerType in deserializerTypes)
            {
                if (deserializerType.Item1 == deserializerName)
                {
                    return deserializerType.Item2;
                }
            }
            throw new System.InvalidOperationException("Couldn't find custom deserializer type.");
        }
   
    }
}
