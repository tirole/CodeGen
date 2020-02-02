using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class StructGenerator
    {
        public StructGenerationInfo Info { get; }
        public StructGenerator(StructGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
