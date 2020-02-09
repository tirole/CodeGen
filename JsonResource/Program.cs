using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr =
                new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuEnum\RootConfig.json");

            var serializer = new DataContractJsonSerializer(typeof(RootConfig));

            var deserialized = (RootConfig)serializer.ReadObject(sr.BaseStream);

            sr.Close();

            sr =
                new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuEnum\Definitions\CompressionMode.json");

            var enumSerializer = new DataContractJsonSerializer(typeof(EnumConfig));

            var enumDeserialized = (EnumConfig)enumSerializer.ReadObject(sr.BaseStream);

            sr.Close();

            {
                sr =
                     new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\SimpleStruct\SimpleStruct.json");

                var tempSerializer = new DataContractJsonSerializer(typeof(StructConfig));

                var tempDeserialized = (StructConfig)tempSerializer.ReadObject(sr.BaseStream);

                sr.Close();
            }

            {
                sr =
                     new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuDescriptors\Definitions\RenderTargetDescriptor.json");

                var tempSerializer = new DataContractJsonSerializer(typeof(DescriptorConfig));

                var tempDeserialized = (DescriptorConfig)tempSerializer.ReadObject(sr.BaseStream);

                sr.Close();
            }

            {
                sr =
                     new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuDescriptors\RootConfig.json");

                var tempSerializer = new DataContractJsonSerializer(typeof(RootConfig));

                var tempDeserialized = (RootConfig)tempSerializer.ReadObject(sr.BaseStream);

                sr.Close();
            }

            {
                var info = new Generator.StructGenerationInfo();
                info.Name = "Test";
                var memberVariable = new Generator.VariableInfo();
                memberVariable.VariableName = "baseAddress";
                memberVariable.Type = "ptrdiff_t";
                info.MemberVariableInfos.Add(memberVariable);
                var memberVariable2 = new Generator.VariableInfo();
                memberVariable2.VariableName = "mipCount";
                memberVariable2.Type = "int";
                info.MemberVariableInfos.Add(memberVariable2);
                var structGenerator = new Generator.StructGenerator(info);
                var line = structGenerator.TransformText();
                Console.WriteLine(line);
            }

            {
                var info = new Generator.StructGenerationInfo();
                info.Name = "Test";
                var memberVariable = new Generator.VariableInfo();
                memberVariable.VariableName = "baseAddress";
                memberVariable.Type = "ptrdiff_t";
                info.MemberVariableInfos.Add(memberVariable);
                var memberVariable2 = new Generator.VariableInfo();
                memberVariable2.VariableName = "mipCount";
                memberVariable2.Type = "int";
                info.MemberVariableInfos.Add(memberVariable2);
                var structGenerator = new Generator.GpuDescriptor.GpuDescriptorAccessorGenerator(info);
                var line = structGenerator.TransformText();
                Console.WriteLine(line);
            }

            {
                var info = new Generator.EnumGenerationInfo();
                info.DoxyBrief = "This is simple enum.";
                info.EnumBase = "int8_t";
                info.EnumKey = "enum class";
                info.EnumName = "Simple";
                var info1 = new Generator.EnumeratorInfo();
                info1.Name = "R8G8B8_Unorm";
                info1.Value = "3";
                info1.DoxyBrief = "3 component R8G8B8.";
                info.EnumeratorInfos.Add(info1);
                var info2 = new Generator.EnumeratorInfo();
                info2.Name = "R8G8_Unorm";
                info2.Value = "12";
                info2.DoxyBrief = "2 component R8G8.";
                info.EnumeratorInfos.Add(info2);

                var generator = new Generator.EnumGenerator(info);
                var line = generator.TransformText();
                Console.WriteLine(line);
            }

            // RootContext and FileInfoBuilder Test
            {
                string path = @"D:\prj\software\codegeneration\JsonResource\Resources\GpuDescriptors\RootConfig.json";
                var rootCtx = new RootContext(path);
                var fileInfos = new List<FileInfo>();
                var builder = new FileInfoBuilder(rootCtx);
                builder.BuildFileInfo(fileInfos);

                foreach(var fileInfo in fileInfos)
                {
                    string output;
                    output = fileInfo.Copyright;
                    output += "\n";

                    foreach (var include in fileInfo.Includes)
                    {
                        output += include;
                        output += "\n";
                    }

                    foreach(var namespaceName in fileInfo.NameSpaces)
                    {
                        output += "namespace " + namespaceName;
                        output += " { ";
                    }
                    output += "\n\n";

                    // fileInfo.StructGenerationInfos
                    foreach (var info in fileInfo.StructGenerationInfos)
                    {
                        var structInfo = info.Item1;
                        var serializerType = info.Item2;

                        if (serializerType == typeof(Generator.GpuDescriptor.GpuDescriptorAccessorGenerator))
                        {
                            var generator = new Generator.GpuDescriptor.GpuDescriptorAccessorGenerator(structInfo);
                            output += generator.TransformText();
                        }
                        else if (serializerType == typeof(Generator.GpuDescriptor.GpuDescriptorStructGenerator))
                        {
                            var generator = new Generator.GpuDescriptor.GpuDescriptorStructGenerator(structInfo);
                            output += generator.TransformText();
                        }
                        else if(serializerType == typeof(Generator.StructGenerator) || (serializerType == null))
                        {
                            var generator = new Generator.StructGenerator(structInfo);
                            output += generator.TransformText();
                        }
                        output += "\n\n";
                    }

                    foreach (var namespaceName in fileInfo.NameSpaces)
                    {
                        output += "}";
                    }

                    string outputPath = @"D:\prj\software\codegeneration\JsonResource\Outputs";
                    outputPath += "/";
                    outputPath += fileInfo.OutputFileName;

                    File.WriteAllText(outputPath, output);
                }
            }
        }
    }

    public class FileInfo
    {
        public string Copyright;
        public string OutputFileName;
        public List<string> Includes;
        public string[] NameSpaces;
        public List<Tuple<Generator.StructGenerationInfo, Type>> StructGenerationInfos;
        public List<Generator.VariableInfo> VariableInfo;
    }

    public class FileInfoBuilder
    {
        public RootContext RootContext;
        public FileInfoBuilder(RootContext context)
        {
            RootContext = context;
        }
        public void BuildFileInfo(List<FileInfo> fileInfos)
        {
            var fileInfo = new FileInfo();
            {
                fileInfo.Copyright = RootContext.Copyright;

                // RootConfig.CommonFileConfig
                {
                    var commonFileConfig = RootContext.RootConfig.CommonFileConfig;
                    fileInfo.Includes = new List<string>(commonFileConfig.IncludeFiles);
                    string[] namespaces = commonFileConfig.Namespace.Replace("::", ":").Split(':');
                    fileInfo.NameSpaces = namespaces;
                }

                // RootConfig.FileContext
                foreach(var fileContext in RootContext.FileContexts)
                {
                    var fileConfig = fileContext.FileConfig;
                    // TODO: .cpp 対応
                    fileInfo.OutputFileName = fileConfig.OutputFileName.Split('.')[0] + ".h";

                    if(fileConfig.IncludeFiles != null)
                    {
                        fileInfo.Includes.AddRange(new List<string>(fileConfig.IncludeFiles));
                    }

                    if((fileConfig.Namespace != null) && (fileConfig.Namespace != ""))
                    {
                        string[] namespaces = fileConfig.Namespace.Replace("::", ":").Split(':');
                        fileInfo.NameSpaces = namespaces;
                    }

                    if(fileContext.DescriptorConfigs.Count != 0)
                    {
                        fileInfo.StructGenerationInfos = new List<Tuple<Generator.StructGenerationInfo, Type>>();
                    }

                    // fileInfo.StructGenerationInfos
                    for(int i = 0; i < fileContext.DescriptorConfigs.Count; ++i)
                    {
                        var descConfig = fileContext.DescriptorConfigs[i];

                        var structGenInfo = new Generator.StructGenerationInfo();
                        structGenInfo.Name = descConfig.Item1.Declaration.DefinitionName;
                        foreach(var memberVariable in descConfig.Item1.memberVariables)
                        {
                            var variableInfo = new Generator.VariableInfo();
                            variableInfo.VariableName = memberVariable.VariableName;
                            variableInfo.Type = memberVariable.Type.Split('[')[0];
                            if(memberVariable.Type.Split('[').Length > 1)
                            {
                                variableInfo.ArrayLength = memberVariable.Type.Split('[')[1].Split(']')[0];
                            }
                            else
                            {
                                variableInfo.ArrayLength = "0";
                            }
                            variableInfo.NameAlias = memberVariable.NameAlias;
                            variableInfo.DoxyBrief = memberVariable.DoxyBrief;
                            variableInfo.DefaultValue = memberVariable.DefaultValue;

                            if(memberVariable.Requirements != null)
                            {
                                variableInfo.RequirementInfos = new List<Generator.RequirementInfo>();
                            }

                            foreach (var requirement in memberVariable.Requirements)
                            {
                                if(requirement == null && requirement == "")
                                {
                                    continue;
                                }
                                var requirementInfo = new Generator.RequirementSdkInfo();
                                List<string> requirements = new List<string>(requirement.Split(':'));
                                requirementInfo.Type = Generator.RequirementInfo.GetRequirementType(requirements[0]);
                                // remove first element which shows requirement type.
                                requirements.RemoveAt(0);
                                requirementInfo.Values.AddRange(requirements);
                                variableInfo.RequirementInfos.Add(requirementInfo);
                            }

                            if((memberVariable.WordOffset != null) && (memberVariable.WordOffset != ""))
                            {
                                variableInfo.HasBitWidthDeclaration = true;
                                var bitRanges = memberVariable.BitRange.Split(':');
                                variableInfo.BitBegin = int.Parse(bitRanges[1]);
                                variableInfo.BitEnd = int.Parse(bitRanges[0]);
                                variableInfo.OffsetIn4ByteUnit = int.Parse(memberVariable.WordOffset);
                            }

                            structGenInfo.MemberVariableInfos.Add(variableInfo);
                        }

                        var structGenTuple =
                            new Tuple<Generator.StructGenerationInfo, Type>(structGenInfo, descConfig.Item2);
                        fileInfo.StructGenerationInfos.Add(structGenTuple);
                    }
                    fileInfos.Add(fileInfo);
                }
            }
        }
    }

}
