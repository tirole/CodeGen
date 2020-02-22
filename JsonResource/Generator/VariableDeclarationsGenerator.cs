using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public partial class VariableDeclarationsGenerator
    {
        public VariableDeclarationsGenerationInfo Info { get; }
        public VariableDeclarationsGenerator(VariableDeclarationsGenerationInfo info)
        {
            this.Info = info;
        }
    }
}
