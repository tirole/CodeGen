using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator.GpuDescriptor
{
    public partial class GpuDescriptorStructGenerator
    {
        public string DefinitionName;
        public string MemberName;
        public const string MemberType = "int32_t"; // C# 向けの出力はどのみちテンプレートを変えざる負えないのでベタ書きにする。
        public GpuDescriptorStructGenerator(StructGenerationInfo info)
        {
            DefinitionName = info.Name;
            MemberName =
                "data[" + 
                info.MemberVariableInfos[info.MemberVariableInfos.Count - 1].OffsetIn4ByteUnit.ToString() + "]";
        }
    }
}
