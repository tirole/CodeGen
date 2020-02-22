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
            var fileInfoCpp = new FileInfo();

            {
                fileInfo.Copyright = RootContext.Copyright;
                fileInfoCpp.Copyright = RootContext.Copyright;

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
                    fileInfo.OutputFileName = fileConfig.OutputFileName.Split('.')[0] + ".h";
                    fileInfoCpp.OutputFileName = fileConfig.OutputFileName.Split('.')[0] + ".cpp";

                    if (fileConfig.IncludeFiles != null)
                    {
                        fileInfo.Includes.AddRange(new List<string>(fileConfig.IncludeFiles));
                    }

                    if ((fileConfig.Namespace != null) && (fileConfig.Namespace != ""))
                    {
                        string[] namespaces = fileConfig.Namespace.Replace("::", ":").Split(':');
                        fileInfo.NameSpaces = namespaces;
                    }

                    if ((fileContext.DescriptorConfigs.Count != 0) || (fileContext.StructConfigs.Count != 0))
                    {
                        fileInfo.StructGenerationInfos = new List<Tuple<Generator.StructGenerationInfo, Type>>();
                    }

                    if (fileContext.EnumConfigs.Count != 0)
                    {
                        fileInfo.EnumGenerationInfos = new List<Generator.EnumGenerationInfo>();
                    }

                    if (fileContext.ClassConfigs.Count != 0)
                    {
                        fileInfo.ClassGenerationInfos = new List<Generator.ClassGenerationInfo>();
                    }

                    if (fileContext.VariableDeclarationsConfigs.Count != 0)
                    {
                        fileInfo.VariableDeclarationsGenerationInfos = new List<Generator.VariableDeclarationsGenerationInfo>();
                    }

                    {
                        string include = "<";
                        foreach (var namespaceName in fileInfo.NameSpaces)
                        {
                            include += namespaceName + "/";
                        }
                        include += fileInfo.OutputFileName + ">";
                        fileInfoCpp.Includes = new List<string>();
                        fileInfoCpp.Includes.Add(include);
                        fileInfoCpp.NameSpaces = fileInfo.NameSpaces;
                    }

                    // fileInfo.VariableDeclarationConfigs
                    for (int i = 0; i < fileContext.VariableDeclarationsConfigs.Count; ++i)
                    {
                        var variableConfig = fileContext.VariableDeclarationsConfigs[i];
                        var variableGenInfo = new Generator.VariableDeclarationsGenerationInfo();

                        foreach (var variable in variableConfig.VariableDeclarationConfigs)
                        {
                            var variableInfo = new Generator.VariableInfo();
                            SetCommonVariableInfo(variableInfo, variable.VariableConfig);
                            variableInfo.IsAssignDefaultValue = variable.IsAssignDefaultValue;
                            variableInfo.DeclarationPrefix = variable.DeclarationPrefix;
                            variableGenInfo.VariableInfos.Add(variableInfo);
                        }
                        fileInfo.VariableDeclarationsGenerationInfos.Add(variableGenInfo);
                    }

                    // fileInfo.StructGenerationInfos for StructConfigs
                    for (int i = 0; i < fileContext.StructConfigs.Count; ++i)
                    {
                        var config = fileContext.StructConfigs[i];

                        var structGenInfo = new Generator.StructGenerationInfo();
                        structGenInfo.Name = config.Declaration.DefinitionName;
                        foreach (var memberVariable in config.MemberVariables)
                        {
                            var variableInfo = new Generator.VariableInfo();
                            this.SetCommonVariableInfo(variableInfo, memberVariable);
                            structGenInfo.MemberVariableInfos.Add(variableInfo);
                        }

                        var structGenTuple =
                            new Tuple<Generator.StructGenerationInfo, Type>(structGenInfo, null);
                        fileInfo.StructGenerationInfos.Add(structGenTuple);
                    }

                    // fileInfo.StructGenerationInfos for DescriptorConfigs
                    for (int i = 0; i < fileContext.DescriptorConfigs.Count; ++i)
                    {
                        var descConfig = fileContext.DescriptorConfigs[i];

                        var structGenInfo = new Generator.StructGenerationInfo();
                        structGenInfo.Name = descConfig.Item1.Declaration.DefinitionName;
                        foreach (var memberVariable in descConfig.Item1.memberVariables)
                        {
                            var variableInfo = new Generator.VariableInfo();

                            this.SetCommonVariableInfo(variableInfo, memberVariable);

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

                    // fileInfo.ClassGenerationInfos
                    for (int i = 0; i < fileContext.ClassConfigs.Count; ++i)
                    {
                        var classConfig = fileContext.ClassConfigs[i];

                        var classGenInfo = new Generator.ClassGenerationInfo();
                        classGenInfo.Name = classConfig.Declaration.DefinitionName;
                        classGenInfo.DoxyBrief = classConfig.Declaration.DoxyBrief;

                        foreach (var config in classConfig.MemberFunctionConfigs)
                        {
                            var memberFunctionInfo = new Generator.MemberFunctionInfo();
                            memberFunctionInfo.AccessModifier = config.AccessModifier;
                            memberFunctionInfo.DoxyBrief = config.FunctionConfig.DoxyBrief;
                            memberFunctionInfo.FunctionName = config.FunctionConfig.FunctionName;
                            memberFunctionInfo.IsInline = config.FunctionConfig.IsInline;
                            memberFunctionInfo.ReturnType = config.FunctionConfig.ReturnType;

                            foreach (var argConfig in config.FunctionConfig.ArgumentConfigs)
                            {
                                var argInfo = new Generator.VariableInfo();
                                this.SetCommonVariableInfo(argInfo, argConfig);
                                memberFunctionInfo.ArgumentInfos.Add(argInfo);
                            }

                            classGenInfo.AddMemberFunctionInfo(memberFunctionInfo);
                        }

                        foreach (var config in classConfig.MemberVariableConfigs)
                        {
                            var memberVariableInfo = new Generator.MemberVariableInfo();
                            this.SetCommonVariableInfo(memberVariableInfo, config.VariableConfig);
                            memberVariableInfo.AccessModifier = config.AccessModifier;
                            memberVariableInfo.IsDefineAccessor = config.IsDefineAccessor;
                            memberVariableInfo.IsInlineAccessor = config.IsInlineAccessor;
                            memberVariableInfo.IsAccessorReturnThis = config.IsAccessorReturnThis;

                            int idxOfFirstUpperCase = 0;
                            foreach (Char c in config.VariableConfig.VariableName)
                            {
                                if (Char.IsUpper(c)) break;
                                ++idxOfFirstUpperCase;
                            }
                            memberVariableInfo.AccessorName = config.VariableConfig.VariableName.Substring(idxOfFirstUpperCase);
                            classGenInfo.AddMemberVariableInfo(memberVariableInfo);

                            if(memberVariableInfo.IsDefineAccessor && memberVariableInfo.IsInlineAccessor)
                            {
                                if (fileInfoCpp.FunctionGenerationInfos == null)
                                {
                                    fileInfoCpp.FunctionGenerationInfos = new List<Generator.FunctionGenerationInfo>();
                                }

                                {
                                    // アクセサについても隠している時点で自分で実装したいのでスケルトンを cpp に出力する。
                                    var funcGen = new Generator.FunctionGenerationInfo();
                                    funcGen.IsDeclaration = false;
                                    var skeltonDefinition = funcGen.FunctionInfo;
                                    skeltonDefinition.FunctionName =
                                        classGenInfo.Name + "::" + "Get" + memberVariableInfo.AccessorName;
                                    skeltonDefinition.StringAfterArgument = "const";
                                    if (memberVariableInfo.IsAccessorReturnThis)
                                    {
                                        skeltonDefinition.ReturnType = classGenInfo.Name + "&";
                                    }
                                    else
                                    {
                                        skeltonDefinition.ReturnType = "void";
                                    }
                                    fileInfoCpp.FunctionGenerationInfos.Add(funcGen);
                                }

                                // setter
                                {
                                    // アクセサについても隠している時点で自分で実装したいのでスケルトンを cpp に出力する。
                                    var funcGen = new Generator.FunctionGenerationInfo();
                                    funcGen.IsDeclaration = false;
                                    var skeltonDefinition = funcGen.FunctionInfo;
                                    skeltonDefinition.FunctionName =
                                        classGenInfo.Name + "::" + "Set" + memberVariableInfo.AccessorName;
                                    skeltonDefinition.StringAfterArgument = "";
                                    skeltonDefinition.ReturnType = "void";
                                    skeltonDefinition.ArgumentInfos.Add((Generator.VariableInfo)memberVariableInfo);
                                    fileInfoCpp.FunctionGenerationInfos.Add(funcGen);
                                }
                            }
                        }

                        fileInfo.ClassGenerationInfos.Add(classGenInfo);
                    }

                    fileInfos.Add(fileInfo);
                    if (!fileInfoCpp.IsEmpty())
                    {
                        fileInfos.Add(fileInfoCpp);
                    }
                }
            }
        }
        private void SetCommonVariableInfo(Generator.VariableInfo variableInfo, VariableConfig config)
        {
            variableInfo.VariableName = config.VariableName;
            variableInfo.Type = config.Type.Split('[')[0];
            if (config.Type.Split('[').Length > 1)
            {
                variableInfo.ArrayLength = int.Parse(config.Type.Split('[')[1].Split(']')[0]);
            }
            else
            {
                variableInfo.ArrayLength = 0;
            }
            variableInfo.NameAlias = config.NameAlias;
            variableInfo.DoxyBrief = config.DoxyBrief;
            variableInfo.DefaultValue = GetDefaultValue(config.DefaultValues);

            if (config.Requirements != null)
            {
                variableInfo.RequirementInfos = new List<Generator.RequirementInfo>();
            }

            foreach (var requirement in config.Requirements)
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
        }

        private string GetDefaultValue(string[] defaultValues)
        {
            string ret = "";
            //if((defaultValue.Length - defaultValue.LastIndexOf(".csv")) == 4)
            //{
            //    // TODO: csv を read していい感じに {} を挿入する。
            //    //      -> でもこの時に結局 struct の情報が必要になる。
            //    //      -> ここから辿っても辿った先が分からない。
            //    //          -> VariableConfig は所詮宣言だけをするためのものであって初期値の代入まではできない。
            //    //              -> 自身からの相対パスなので辿れるか。
            //}
            //else
            for(int i = 0; i < defaultValues.Length; ++i)
            {
                ret += defaultValues[i];
                if(i != defaultValues.Length - 1)
                {
                    ret += ", ";
                }
            }
            return ret;
        }
    }
}
