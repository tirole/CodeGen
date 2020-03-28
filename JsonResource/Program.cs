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
            {
                var variableInfo = new Generator.VariableInfo();
                variableInfo.OffsetIn4ByteUnit = 1;
                variableInfo.BitBegin = 6;
                variableInfo.BitEnd = 63;
                int bitWidth = variableInfo.BitEnd - variableInfo.BitBegin + 1;
                if (variableInfo.BitBegin != 0 && bitWidth > 63)
                {
                    int wordOffset = 0;
                    while (bitWidth >= 0)
                    {
                        if (bitWidth < 32)
                        {
                            // ビットマスクで書き込む
                        }
                        else
                        {
                            // 32bit 代入
                        }
                        wordOffset += 1;
                        bitWidth -= 32;
                    }
                }
            }
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
            List<string> RootConfigPaths = new List<string>
            {
                //@"D:\prj\software\codegeneration\JsonResource\Resources\SimpleStruct\RootConfig.json",
                //@"D:\prj\software\codegeneration\JsonResource\Resources\GpuCommand\RootConfig.json",
                //@"D:\prj\software\codegeneration\JsonResource\Resources\Variable\RootConfig.json",
                //@"D:\prj\software\codegeneration\JsonResource\Resources\Class\RootConfig.json",
                @"D:\prj\software\codegeneration\JsonResource\Resources\GpuDescriptors\RootConfig.json",
                //@"D:\prj\software\codegeneration\JsonResource\Resources\GpuEnum\RootConfig.json"
            };

            foreach(var rootConfigPath in RootConfigPaths)
            {
                string path = rootConfigPath;
                var rootCtx = new RootContext(path);
                var fileInfos = new List<FileInfo>();
                var builder = new FileInfoBuilder(rootCtx);
                builder.BuildFileInfo(fileInfos);

                foreach(var fileInfo in fileInfos)
                {
                    string output;
                    output = fileInfo.Copyright;
                    output += "\n";

                    if(fileInfo.OutputFileName.LastIndexOf(".h") != -1)
                    {
                        output += "#pragma once\n\n";
                    }

                    foreach (var include in fileInfo.Includes)
                    {
                        output += "#include " + include;
                        output += "\n";
                    }

                    foreach(var namespaceName in fileInfo.NameSpaces)
                    {
                        output += "namespace " + namespaceName;
                        output += " { ";
                    }
                    output += "\n\n";

                    // fileInfo.EnumeratorInfos
                    if (fileInfo.EnumGenerationInfos != null)
                    {
                        foreach (var info in fileInfo.EnumGenerationInfos)
                        {
                            var generator = new Generator.EnumGenerator(info);
                            output += generator.TransformText(); output += "\n\n";
                        }
                    }

                    // fileInfo.StructGenerationInfos
                    if (fileInfo.StructGenerationInfos != null)
                    {
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
                            else if (serializerType == typeof(Generator.StructGenerator) || (serializerType == null))
                            {
                                var generator = new Generator.StructGenerator(structInfo);
                                output += generator.TransformText();
                            }
                            else if (serializerType == typeof(Generator.GpuCommandGenerator))
                            {
                                var generator = new Generator.GpuCommandGenerator(structInfo);
                                output += generator.TransformText();
                            }
                            output += "\n\n";
                        }
                    }

                    // fileInfo.ClassGenerationInfos
                    if (fileInfo.ClassGenerationInfos != null)
                    {
                        foreach (var info in fileInfo.ClassGenerationInfos)
                        {
                            var generator = new Generator.ClassGenerator(info);
                            output += generator.TransformText(); output += "\n\n";
                        }
                    }

                    // fileInfo.FunctionGenerationInfos
                    if (fileInfo.FunctionGenerationInfos != null)
                    {
                        foreach (var info in fileInfo.FunctionGenerationInfos)
                        {
                            var generator = new Generator.FunctionGenerator(info);
                            output += generator.TransformText(); output += "\n";
                        }
                    }

                    // fileInfo.FunctionGenerationInfos
                    if (fileInfo.VariableDeclarationsGenerationInfos != null)
                    {
                        foreach (var info in fileInfo.VariableDeclarationsGenerationInfos)
                        {
                            var generator = new Generator.VariableDeclarationsGenerator(info);
                            output += generator.TransformText(); output += "\n";
                        }
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
        public List<Generator.VariableDeclarationsGenerationInfo> VariableDeclarationsGenerationInfos;
        public List<Generator.EnumGenerationInfo> EnumGenerationInfos;
        public List<Generator.ClassGenerationInfo> ClassGenerationInfos;
        public List<Generator.FunctionGenerationInfo> FunctionGenerationInfos;
        public bool IsEmpty()
        {
            return 
                (StructGenerationInfos == null) && (VariableDeclarationsGenerationInfos == null) && 
                (EnumGenerationInfos == null) && (ClassGenerationInfos == null) && 
                (FunctionGenerationInfos == null);
    }
    }
}
