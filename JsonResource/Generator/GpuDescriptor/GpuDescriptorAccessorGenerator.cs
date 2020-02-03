using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator.GpuDescriptor
{
    public partial class GpuDescriptorAccessorGenerator
    {
        public StructGenerationInfo Info { get; }
        public GpuDescriptorAccessorGenerator(StructGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
