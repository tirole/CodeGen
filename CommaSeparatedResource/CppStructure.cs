using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public class CppStructure : Structure
    {
        public override string GetString()
        {
            string ret =
                "struct " + Name + "\n" +
                "{\n";
            foreach (var member in Members)
            {
                ret += "\t" + member.GetString() + "\n";
            }
            ret += "}\n";
            return ret;
        }
    }
}
