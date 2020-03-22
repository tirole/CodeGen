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
        public const string MemberType = "uint32_t"; // C# 向けの出力はどのみちテンプレートを変えざる負えないのでベタ書きにする。
        public GpuDescriptorStructGenerator(StructGenerationInfo info)
        {
            var lastMember = info.MemberVariableInfos[info.MemberVariableInfos.Count - 1];
            int wordOffset = lastMember.OffsetIn4ByteUnit;
            int dataArrayLength = wordOffset;
            int lastMemberBitCount = lastMember.BitEnd - lastMember.BitBegin + 1;

            // alignup to 32bit
            int align = 32;
            var rem = lastMemberBitCount % align;
            lastMemberBitCount = (rem != 0) ? lastMemberBitCount + (align - rem) : lastMemberBitCount;
            int lastMemberSizeIn4Byte = lastMemberBitCount / 32;
            dataArrayLength += lastMemberSizeIn4Byte;

            DefinitionName = info.Name;
            MemberName =
                "data[" +
                dataArrayLength + "]";
        }
    }
}
