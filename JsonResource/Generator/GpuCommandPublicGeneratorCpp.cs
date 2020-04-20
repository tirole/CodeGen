using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class GpuCommandPublicGeneratorCpp
    {
        public StructGenerationInfo Info { get; }
        public GpuCommandPublicGeneratorCpp(StructGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
