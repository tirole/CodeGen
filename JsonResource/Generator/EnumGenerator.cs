using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class EnumGenerator
    {
        public EnumGenerationInfo Info { get; }
        public EnumGenerator(EnumGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
