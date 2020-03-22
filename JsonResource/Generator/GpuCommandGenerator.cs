using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class GpuCommandGenerator
    {
        public StructGenerationInfo Info { get; }
        public GpuCommandGenerator(StructGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
