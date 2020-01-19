using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public class CppMember : Member
    {
        override public string GetString()
        {
            string ret = Type + " " + Name + ";";
            return ret;
        }
    }
}
