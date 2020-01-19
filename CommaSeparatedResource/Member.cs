using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public abstract class Member
    {
        public char BitWidthDeclarationSeparater = ':';
        public Member()
        {
            HasBitWidthDeclaration = false;
        }
        public string Type { get; set; }
        public string Name { get; set; }
        public Requirement Requirement { get; set; }
        public bool HasBitWidthDeclaration;
        public int BitBegin { get; set; }
        public int BitEnd { get; set; }
        public int OffsetIn4ByteUnit { get; set; }
        abstract public string GetString();
    }
}
