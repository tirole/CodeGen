using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class FunctionGenerationInfo : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public FunctionGenerationInfo()
        {
            FunctionInfo = new FunctionInfo();
            IsDeclaration = false;
        }
        public bool IsDeclaration;
        public FunctionInfo FunctionInfo;
    }
}
