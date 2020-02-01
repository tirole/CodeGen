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
                new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuEnum\FileConfig.json");

            var serializer = new DataContractJsonSerializer(typeof(FileConfig));

            var deserialized = (FileConfig)serializer.ReadObject(sr.BaseStream);

            sr.Close();

            sr =
                new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuEnum\Definitions\CompressionMode.json");

            var enumSerializer = new DataContractJsonSerializer(typeof(EnumConfig));

            var enumDeserialized = (EnumConfig)enumSerializer.ReadObject(sr.BaseStream);

            sr.Close();

            {
                sr =
                     new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuRenderTargetDescriptor\SimpleStruct.json");

                var simpleStructSerializer = new DataContractJsonSerializer(typeof(StructConfig));

                var simpleStructDeserialized = (StructConfig)simpleStructSerializer.ReadObject(sr.BaseStream);

                sr.Close();
            }

            {
                sr =
                     new StreamReader(@"D:\prj\software\codegeneration\JsonResource\Resources\GpuRenderTargetDescriptor\RenderTargetDescriptor.json");

                var simpleStructSerializer = new DataContractJsonSerializer(typeof(DescriptorConfig));

                var simpleStructDeserialized = (DescriptorConfig)simpleStructSerializer.ReadObject(sr.BaseStream);

                sr.Close();
            }
        }
    }
}
