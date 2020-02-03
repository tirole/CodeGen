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
                memberVariable.Name = "baseAddress";
                memberVariable.Type = "ptrdiff_t";
                info.MemberVariableInfos.Add(memberVariable);
                var memberVariable2 = new Generator.VariableInfo();
                memberVariable2.Name = "mipCount";
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
                memberVariable.Name = "baseAddress";
                memberVariable.Type = "ptrdiff_t";
                info.MemberVariableInfos.Add(memberVariable);
                var memberVariable2 = new Generator.VariableInfo();
                memberVariable2.Name = "mipCount";
                memberVariable2.Type = "int";
                info.MemberVariableInfos.Add(memberVariable2);
                var structGenerator = new Generator.GpuDescriptor.GpuDescriptorAccessorGenerator(info);
                var line = structGenerator.TransformText();
                Console.WriteLine(line);
            }
        }
    }
}
