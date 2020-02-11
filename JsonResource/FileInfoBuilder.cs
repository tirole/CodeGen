using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    public class FileInfoBuilder
    {
        public RootContext RootContext;
        public FileInfoBuilder(RootContext context)
        {
            RootContext = context;
        }
        public void BuildFileInfo(List<FileInfo> fileInfos)
        {
            var fileInfo = new FileInfo();
            {
                fileInfo.Copyright = RootContext.Copyright;

                // RootConfig.CommonFileConfig
                {
                    var commonFileConfig = RootContext.RootConfig.CommonFileConfig;
                    fileInfo.Includes = new List<string>(commonFileConfig.IncludeFiles);
                    string[] namespaces = commonFileConfig.Namespace.Replace("::", ":").Split(':');
                    fileInfo.NameSpaces = namespaces;
                }

                // RootConfig.FileContext
                foreach (var fileContext in RootContext.FileContexts)
                {
                    var fileConfig = fileContext.FileConfig;
                    // TODO: .cpp 対応
                    fileInfo.OutputFileName = fileConfig.OutputFileName.Split('.')[0] + ".h";

                    if (fileConfig.IncludeFiles != null)
                    {
                        fileInfo.Includes.AddRange(new List<string>(fileConfig.IncludeFiles));
                    }

                    if ((fileConfig.Namespace != null) && (fileConfig.Namespace != ""))
                    {
                        string[] namespaces = fileConfig.Namespace.Replace("::", ":").Split(':');
                        fileInfo.NameSpaces = namespaces;
                    }

                    if (fileContext.DescriptorConfigs.Count != 0)
                    {
                        fileInfo.StructGenerationInfos = new List<Tuple<Generator.StructGenerationInfo, Type>>();
                    }

                    if (fileContext.EnumConfigs.Count != 0)
                    {
                        fileInfo.EnumGenerationInfos = new List<Generator.EnumGenerationInfo>();
                    }

                    // fileInfo.StructGenerationInfos
                    for (int i = 0; i < fileContext.DescriptorConfigs.Count; ++i)
                    {
                        var descConfig = fileContext.DescriptorConfigs[i];

                        var structGenInfo = new Generator.StructGenerationInfo();
                        structGenInfo.Name = descConfig.Item1.Declaration.DefinitionName;
                        foreach (var memberVariable in descConfig.Item1.memberVariables)
                        {
                            var variableInfo = new Generator.VariableInfo();
                            variableInfo.VariableName = memberVariable.VariableName;
                            variableInfo.Type = memberVariable.Type.Split('[')[0];
                            if (memberVariable.Type.Split('[').Length > 1)
                            {
                                variableInfo.ArrayLength = memberVariable.Type.Split('[')[1].Split(']')[0];
                            }
                            else
                            {
                                variableInfo.ArrayLength = "0";
                            }
                            variableInfo.NameAlias = memberVariable.NameAlias;
                            variableInfo.DoxyBrief = memberVariable.DoxyBrief;
                            variableInfo.DefaultValue = memberVariable.DefaultValue;

                            if (memberVariable.Requirements != null)
                            {
                                variableInfo.RequirementInfos = new List<Generator.RequirementInfo>();
                            }

                            foreach (var requirement in memberVariable.Requirements)
                            {
                                if (requirement == null && requirement == "")
                                {
                                    continue;
                                }
                                var requirementInfo = new Generator.RequirementSdkInfo();
                                List<string> requirements = new List<string>(requirement.Split(':'));
                                requirementInfo.Type = Generator.RequirementInfo.GetRequirementType(requirements[0]);
                                // remove first element which shows requirement type.
                                requirements.RemoveAt(0);
                                requirementInfo.Values.AddRange(requirements);
                                variableInfo.RequirementInfos.Add(requirementInfo);
                            }

                            if ((memberVariable.WordOffset != null) && (memberVariable.WordOffset != ""))
                            {
                                variableInfo.HasBitWidthDeclaration = true;
                                var bitRanges = memberVariable.BitRange.Split(':');
                                variableInfo.BitBegin = int.Parse(bitRanges[1]);
                                variableInfo.BitEnd = int.Parse(bitRanges[0]);
                                variableInfo.OffsetIn4ByteUnit = int.Parse(memberVariable.WordOffset);
                            }

                            structGenInfo.MemberVariableInfos.Add(variableInfo);
                        }

                        var structGenTuple =
                            new Tuple<Generator.StructGenerationInfo, Type>(structGenInfo, descConfig.Item2);
                        fileInfo.StructGenerationInfos.Add(structGenTuple);
                    }

                    // fileInfo.EnumGenerationInfos
                    for (int i = 0; i < fileContext.EnumConfigs.Count; ++i)
                    {
                        var enumConfig = fileContext.EnumConfigs[i];

                        var enumGenInfo = new Generator.EnumGenerationInfo();
                        enumGenInfo.EnumName = enumConfig.Declaration.DefinitionName;
                        enumGenInfo.DoxyBrief = enumConfig.Declaration.DoxyBrief;
                        enumGenInfo.EnumBase = enumConfig.EnumBase;
                        enumGenInfo.EnumKey = enumConfig.Declaration.DefinitionType;

                        foreach (var enumerator in enumConfig.Enumerators)
                        {
                            var enumeratorInfo = new Generator.EnumeratorInfo();
                            enumeratorInfo.Name = enumerator.Name;
                            enumeratorInfo.Value = enumerator.Value;
                            enumeratorInfo.DoxyBrief = enumerator.DoxyBrief;
                            enumGenInfo.EnumeratorInfos.Add(enumeratorInfo);
                        }
                        fileInfo.EnumGenerationInfos.Add(enumGenInfo);
                    }

                    fileInfos.Add(fileInfo);
                }
            }
        }
    }
}
