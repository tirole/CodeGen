using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class ClassGenerator
    {
        public ClassGenerationInfo Info { get; }
        public ClassGenerator(ClassGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
