using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public abstract class Structure
    {
        public Structure()
        {
            Members = new List<Member>();
        }
        public string Name { get; set; }
        public List<Member> Members { get; set; }
        public abstract string GetString();
    }
}
