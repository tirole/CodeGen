﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    public class FileContext
    {
        public FileContext()
        {
            FileConfig = new FileConfig();
            DescriptorConfigs = new List<Tuple<DescriptorConfig, string, Type>>();
            StructConfigs = new List<Tuple<StructConfig, string>>();
            EnumConfigs = new List<Tuple<EnumConfig, string>>();
        }
        public FileConfig FileConfig;
        public List<Tuple<StructConfig, string>> StructConfigs;
        public List<Tuple<EnumConfig, string>> EnumConfigs;
        public List<Tuple<DescriptorConfig,string,Type>> DescriptorConfigs;
    }
    public class RootContext
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
                        string jsonFilePath = RootConfigDirectoryName + "/" + definitionConfig.Path;
                        var descriptorConfig = Deserialize<DeclarationConfig>(jsonFilePath);

                        Type configType = null;
                        Type generatorType = null;

                        // search custom deserializer
                        foreach (var customDeserializerConfig in RootConfig.CommonFileConfig.CustomDeserializerConfigs)
                        {
                            if(customDeserializerConfig.DefinitionName == definitionConfig.DefinitionName)
                            {
                                configType = GetCustomDeserializerType(customDeserializerConfig.DeserializerType);
                                generatorType = GetCustomGeneratorType(customDeserializerConfig.GeneratorType);
                                break;
                            }
                        }

                        // search general deserializer
                        if(configType == null)
                        {
                            var declarationConfig = Deserialize<DeclarationConfig>(jsonFilePath);
                            configType = GetGenericDeserializerType(declarationConfig.Declaration.DefinitionType);
                        }

                        // generator は desrializer から決まるので変更が無い場合は何もしない。
                        

                        if (configType == typeof(EnumConfig))
                        {
                            var config = Deserialize<EnumConfig>(jsonFilePath);
                            var tuple = new Tuple<EnumConfig, string>(config, jsonFilePath);
                            fileContext.EnumConfigs.Add(tuple);
                        }
                        else if (configType == typeof(StructConfig))
                        {
                            var config = Deserialize<StructConfig>(jsonFilePath);
                            var tuple = new Tuple<StructConfig, string>(config, jsonFilePath);
                            fileContext.StructConfigs.Add(tuple);
                        }
                        else if(configType == typeof(DescriptorConfig))
                        {
                            var config = Deserialize<DescriptorConfig>(jsonFilePath);
                            var tuple = new Tuple<DescriptorConfig, string, Type>(config, jsonFilePath, generatorType);
                            fileContext.DescriptorConfigs.Add(tuple);
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

            // early out
            if (deserializerName == "")
            {
                return null;
            }

            foreach (var deserializerType in deserializerTypes)
            {
                if (deserializerType.Item1 == deserializerName)
                {
                    return deserializerType.Item2;
                }
            }
            throw new System.InvalidOperationException("Couldn't find custom deserializer type.");
        }
        public Type GetCustomGeneratorType(string generatorName)
        {
            Tuple<string, Type>[] generatorTypes =
            {
                new Tuple<string, Type>("GpuDescriptorAccessorGenerator", typeof(Generator.GpuDescriptor.GpuDescriptorAccessorGenerator)),
                new Tuple<string, Type>("GpuDescriptorStructGenerator", typeof(Generator.GpuDescriptor.GpuDescriptorStructGenerator))
            };

            // early out
            if (generatorName == "")
            {
                return null;
            }

            foreach (var type in generatorTypes)
            {
                if (type.Item1 == generatorName)
                {
                    return type.Item2;
                }
            }
            throw new System.InvalidOperationException("Couldn't find custom generator type.");
        }

    }
}
