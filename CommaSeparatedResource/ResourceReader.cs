using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public abstract class ResourceReader
    {
        public ResourceReader()
        {
            this.Structures = new List<Structure>();
            IsInitialized = false;
        }
        public abstract void Initialize(List<string> fileNames);
        protected bool IsInitialized;
        protected List<string> FileNames;
        public List<Structure> Structures;
    }
}
