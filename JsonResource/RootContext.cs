using System;
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
            DescriptorConfigs = new List<Tuple<DescriptorConfig, Type>>();
            StructConfigs = new List<StructConfig>();
            EnumConfigs = new List<EnumConfig>();
            ClassConfigs = new List<ClassConfig>();
            VariableDeclarationsConfigs = new List<VariableDeclarationsConfig>();
        }
        public FileConfig FileConfig;
        public List<StructConfig> StructConfigs;
        public List<EnumConfig> EnumConfigs;
        public List<Tuple<DescriptorConfig,Type>> DescriptorConfigs;
        public List<ClassConfig> ClassConfigs;
        public List<VariableDeclarationsConfig> VariableDeclarationsConfigs;
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
                        if (RootConfig.CommonFileConfig.CustomDeserializerConfigs != null)
                        {
                            foreach (var customDeserializerConfig in RootConfig.CommonFileConfig.CustomDeserializerConfigs)
                            {
                                if (customDeserializerConfig.DefinitionName == definitionConfig.DefinitionName)
                                {
                                    configType = GetCustomDeserializerType(customDeserializerConfig.DeserializerType);
                                    generatorType = GetCustomGeneratorType(customDeserializerConfig.GeneratorType);
                                    break;
                                }
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
                            fileContext.DescriptorConfigs.Add(new Tuple<DescriptorConfig, Type>(config, generatorType));
                        }
                        else if (configType == typeof(ClassConfig))
                        {
                            var config = Deserialize<ClassConfig>(jsonFilePath);
                            fileContext.ClassConfigs.Add(config);
                        }
                        else if (configType == typeof(VariableDeclarationsConfig))
                        {
                            var config = Deserialize<VariableDeclarationsConfig>(jsonFilePath);
                            fileContext.VariableDeclarationsConfigs.Add(config);
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
        public static T Deserialize<T>(string jsonFilePath)
        {
            var deserializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new StreamReader(jsonFilePath))
            {
                T result = (T)deserializer.ReadObject(stream.BaseStream);
                Type type = result.GetType();
                if (type == typeof(DescriptorConfig))
                {
                    DescriptorConfig descConfig = (DescriptorConfig)(object)result;

                    foreach(var member in descConfig.memberVariables)
                    {
                        if(member.Type.Split('.').Length > 1)
                        {
                            member.Type = Path.GetDirectoryName(jsonFilePath) + "/" + member.Type;
                        }
                        if (member.DefaultValues != null && member.DefaultValues[0].Split('.').Length > 1)
                        {
                            member.DefaultValues[0] = Path.GetDirectoryName(jsonFilePath) + "/" + member.DefaultValues[0];
                        }
                    }
                }
                else if (type == typeof(ClassConfig))
                {
                    var config = (ClassConfig)(object)result;

                    foreach (var member in config.MemberVariableConfigs)
                    {
                        var variableConfig = member.VariableDeclarationConfig.VariableConfig;
                        if (variableConfig.Type.Split('.').Length > 1)
                        {
                            variableConfig.Type = Path.GetDirectoryName(jsonFilePath) + "/" + variableConfig.Type;
                        }
                        if (variableConfig.DefaultValues != null && variableConfig.DefaultValues[0].Split('.').Length > 1)
                        {
                            variableConfig.DefaultValues[0] = Path.GetDirectoryName(jsonFilePath) + "/" + variableConfig.DefaultValues[0];
                        }
                    }
                    foreach (var member in config.MemberFunctionConfigs)
                    {
                        foreach (var arg in member.FunctionConfig.ArgumentConfigs)
                        {
                            var variableConfig = arg.VariableConfig;
                            if (variableConfig.Type.Split('.').Length > 1)
                            {
                                variableConfig.Type = Path.GetDirectoryName(jsonFilePath) + "/" + variableConfig.Type;
                            }
                            if (variableConfig.DefaultValues != null && variableConfig.DefaultValues[0].Split('.').Length > 1)
                            {
                                variableConfig.DefaultValues[0] = Path.GetDirectoryName(jsonFilePath) + "/" + variableConfig.DefaultValues[0];
                            }
                        }
                    }
                }
                else if (type == typeof(StructConfig))
                {
                    var config = (StructConfig)(object)result;

                    foreach (var member in config.MemberVariables)
                    {
                        if (member.Type.Split('.').Length > 1)
                        {
                            member.Type = Path.GetDirectoryName(jsonFilePath) + "/" + member.Type;
                        }
                        if (member.DefaultValues != null && member.DefaultValues[0].Split('.').Length > 1)
                        {
                            member.DefaultValues[0] = Path.GetDirectoryName(jsonFilePath) + "/" + member.DefaultValues[0];
                        }
                    }
                }
                else if (type == typeof(VariableDeclarationsConfig))
                {
                    var config = (VariableDeclarationsConfig)(object)result;

                    foreach (var variable in config.VariableDeclarationConfigs)
                    {
                        if (variable.VariableConfig.Type.Split('.').Length > 1)
                        {
                            var dependedDescPath = Path.GetDirectoryName(jsonFilePath) + "/" + variable.VariableConfig.Type;
                            variable.VariableConfig.Type = dependedDescPath;
                        }
                        if (variable.VariableConfig.IsCsvDefaultValue())
                        {
                            variable.VariableConfig.DefaultValues[0] = Path.GetDirectoryName(jsonFilePath) + "/" + variable.VariableConfig.DefaultValues[0];
                        }
                    }
                }
                return result;
            }
        }
        public static void GetType(DeclarationCommonConfig commonDeclConfig, string jsonFilePath)
        {
            using (var stream = new StreamReader(jsonFilePath))
            {
                DeclarationConfig config = Deserialize<DeclarationConfig>(jsonFilePath);
                commonDeclConfig.DefinitionName = config.Declaration.DefinitionName;
                commonDeclConfig.NameAlias = config.Declaration.NameAlias;
            }
        }
        public static Type GetGenericDeserializerType(string definitionType)
        {
            Tuple<string, Type>[] deserializerTypes =    
            { 
                new Tuple<string, Type>("enum class", typeof(EnumConfig)) ,
                new Tuple<string, Type>("struct", typeof(StructConfig)),
                new Tuple<string, Type>("union", typeof(StructConfig)),
                new Tuple<string, Type>("class", typeof(ClassConfig)),
                new Tuple<string, Type>("variable", typeof(VariableDeclarationsConfig)),
            };
            foreach (var deserializerType in deserializerTypes)
            {
                if (deserializerType.Item1 == definitionType)
                {
                    return deserializerType.Item2;
                }
            }
            throw new System.InvalidOperationException("Couldn't find default deserializer type.");
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
                new Tuple<string, Type>("GpuDescriptorStructGenerator", typeof(Generator.GpuDescriptor.GpuDescriptorStructGenerator)),
                new Tuple<string, Type>("GpuCommandGenerator", typeof(Generator.GpuCommandGenerator)),
                new Tuple<string, Type>("GpuCommandPublicGenerator", typeof(Generator.GpuCommandPublicGenerator)),
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
