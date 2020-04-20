using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class FunctionInfo
    {
        public FunctionInfo()
        {
            ArgumentInfos = new List<VariableInfo>();
            DoxyDetails = new List<string>();
        }

        public string FunctionName;
        public bool IsInline;
        public string DoxyBrief;
        public List<string> DoxyDetails;
        public string ReturnType;
        public string StringAfterArgument;
        public List<VariableInfo> ArgumentInfos;
    }
}
