using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    class Program
    {
        static void Main(string[] args)
        {
#if false
            List<int> intArray = new List<int>
            {
                1
            };
            intArray[0] = 4;
            intArray.Add(2);
            System.Console.WriteLine("arraySize: {0}\n", intArray.Count);

            int val1 = 1;
            int val2 = 2;
            System.Console.WriteLine("val1: {0} val2: {1}\n", val1, val2);
#endif
            string path = @"D:\prj\software\codegeneration\CommaSeparatedResource\Resource\TextureDescriptor.txt";

            CommaSeparatedStructureResourceReader reader = new CommaSeparatedStructureResourceReader();
            List<string> files = new List<string>();
            files.Add(path);
            reader.Initialize(files);
        }
    }
}
