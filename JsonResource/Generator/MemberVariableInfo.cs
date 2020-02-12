using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class MemberVariableInfo : VariableInfo
    {
        public string AccessModifier;
        public bool IsDefineAccessor;
        public bool IsInlineAccessor;
        public bool IsAccessorReturnThis;
        public string AccessorName;
    }
}
