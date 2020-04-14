using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class GpuCommandPublicGenerator
    {
        public StructGenerationInfo Info { get; }
        public GpuCommandPublicGenerator(StructGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
