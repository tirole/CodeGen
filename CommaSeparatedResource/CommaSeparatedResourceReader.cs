using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommaSeparatedResource
{
    public class CommaSeparatedStructureResourceReader : ResourceReader
    {
        enum ColumnIndex
        {
            DefinitionType = 0,
            DefinitionName = 1,
            VariableName = 2,
            VariableType = 3,
            Requirement = 4,
            VariableOffsetIn4ByteUnit = 5,
            VariableBitRange = 6,
            Count,
        };
        public override void Initialize(List<string> fileNames)
        {
            FileNames = fileNames;
            foreach(var fileName in FileNames)
            {
                StreamReader sr = new StreamReader(fileName);

                List<string[]> lines = new List<string[]>();

                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine();
                    lines.Add(line.Split(','));
                }

                sr.Close();

                if(lines.Count == 0)
                {
                    throw new System.InvalidOperationException("No line in File.");
                }

                Structures.Add(new CppStructure());
                var structure = Structures[Structures.Count - 1];

                foreach (var line in lines)
                {
                    structure.Members.Add(new CppMember());
                    var member = structure.Members[structure.Members.Count - 1];
                    for (ColumnIndex i = 0; i < ColumnIndex.Count; ++i)
                    {
                        switch (i)
                        {
                            case ColumnIndex.DefinitionName:
                                structure.Name = line[(int)i];
                                break;
                            case ColumnIndex.DefinitionType:
                                // strcut の定義と決め打ちしているので何もしない
                                break;
                            case ColumnIndex.Requirement:
                                SdkRequirement sdkReq = new SdkRequirement(line[(int)i]);
                                member.Requirement = sdkReq;
                                break;
                            case ColumnIndex.VariableBitRange:
                                var bitDeclarations = line[(int)i].Split(member.BitWidthDeclarationSeparater);
                                if (bitDeclarations.Length == 0)
                                {
                                    member.HasBitWidthDeclaration = false;
                                }
                                else 
                                {
                                    member.HasBitWidthDeclaration = true;
                                    member.BitBegin = int.Parse(bitDeclarations[1]);
                                    member.BitEnd = int.Parse(bitDeclarations[0]);
                                }
                                break;
                            case ColumnIndex.VariableName:
                                member.Name = line[(int)i];
                                break;
                            case ColumnIndex.VariableOffsetIn4ByteUnit:
                                member.OffsetIn4ByteUnit = int.Parse(line[(int)i]);
                                break;
                            case ColumnIndex.VariableType:
                                member.Type = line[(int)i];
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
