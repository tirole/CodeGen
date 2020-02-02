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
        }
    }
}
