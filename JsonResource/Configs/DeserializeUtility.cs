using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    class DeserializeUtility
    {
        public static string GetDoxyBrief(string doxyBrief, string nameAlias)
        {
            if (doxyBrief == "" || doxyBrief == null)
            {
                return nameAlias + "です。";
            }
            else
            {
                return doxyBrief;
            }
        }

        public static void GetDoxyDetails(List<string> outStr, string[] srcStr)
        {
            if (outStr != null && srcStr != null)
            {
                foreach (string str in srcStr)
                {
                    outStr.Add(str);
                }
            }
        }
    }
}
