using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class FunctionGenerator
    {
        public FunctionGenerationInfo Info { get; }
        public FunctionGenerator(FunctionGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
