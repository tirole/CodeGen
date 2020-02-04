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
            StructConfigs = new List<StructConfig>();
            EnumConfigs = new List<EnumConfig>();
        }
        public FileConfig FileConfig;

        public List<StructConfig> StructConfigs;
        public List<EnumConfig> EnumConfigs;
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
                        string jsonFilePath = RootConfigDirectoryName + "/" + definitionConfig;
                        var descriptorConfig = Deserialize<DeclarationConfig>(jsonFilePath);

                        Type configType = null;

                        // search custom deserializer
                        foreach(var customDeserializerConfig in RootConfig.CommonFileConfig.CustomDeserializerConfigs)
                        {
                            if(customDeserializerConfig.DefinitionName == definitionConfig.DefinitionName)
                            {
                                configType = GetCustomDeserializerType(customDeserializerConfig.DeserializerType);
                                break;
                            }
                        }

                        // search general deserializer
                        if(configType == null)
                        {
                            var declarationConfig = Deserialize<DeclarationConfig>(jsonFilePath);
                            configType = GetGenericDeserializerType(declarationConfig.Declaration.DefinitionType);
                        }
                        

                        if (configType == typeof(EnumConfig))
                        {
                            var config = Deserialize<EnumConfig>(jsonFilePath);
                            fileContext.EnumConfigs.Add(config);
                        }
                        else if (configType == typeof(StructConfig))
                        {
                            var config = Deserialize<StructConfig>(jsonFilePath);
                            fileContext.StructConfigs.Add(config);
                        }
                        else if(configType == typeof(DescriptorConfig))
                        {
                            var config = Deserialize<DescriptorConfig>(jsonFilePath);
                            fileContext.DescriptorConfigs.Add(config);
                        }
                        else
                        {
                            throw new System.InvalidOperationException("Couldn't find proper deserializer type.");
                        }
                    }
                    this.FileContexts.Add(fileContext);
                }
            }
        }
        public static T Deserialize<T>(string jsonFIiePath)
        {
            var deserializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new StreamReader(jsonFIiePath))
            {
                T result = (T)deserializer.ReadObject(stream.BaseStream);
                return result;
            }
        }

        public Type GetGenericDeserializerType(string definitionType)
        {
            Tuple<string, Type>[] deserializerTypes =    
            { 
                new Tuple<string, Type>("enum class", typeof(EnumConfig)) ,
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
