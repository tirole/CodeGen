using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class VariableDeclarationsGenerationInfo
    {
        public VariableDeclarationsGenerationInfo()
        {
            VariableInfos = new List<VariableInfo>();
        }
        public List<VariableInfo> VariableInfos { get; set; }
    }
}
