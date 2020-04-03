using System;
using System.Collections.Generic;
using System.IO;
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
            // RootConfig.FileContext
            foreach (var fileContext in RootContext.FileContexts)
            {
                var fileInfo = new FileInfo();
                var fileInfoCpp = new FileInfo();

                fileInfo.Copyright = RootContext.Copyright;
                fileInfoCpp.Copyright = RootContext.Copyright;

                // RootConfig.CommonFileConfig
                {
                    var commonFileConfig = RootContext.RootConfig.CommonFileConfig;
                    fileInfo.Includes = new List<string>(commonFileConfig.IncludeFiles);
                    string[] namespaces = commonFileConfig.Namespace.Replace("::", ":").Split(':');
                    fileInfo.NameSpaces = namespaces;
                }

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
                        variableInfo.TypePrefix = variable.TypePrefix;
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

                    if (config.Declaration.DefinitionType == "union")
                    {
                        structGenInfo.StructureType = "union";
                    }
                    else
                    {
                        structGenInfo.StructureType = "struct";
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

                        // json の場合はポインタ渡しとする。
                        bool isJson = memberVariable.Type.IndexOf(".json") != -1;
                        if (isJson)
                        {
                            variableInfo.TypeSuffix = "*";
                        }

                        if ((memberVariable.WordOffset != null) && (memberVariable.WordOffset != ""))
                        {
                            variableInfo.HasBitWidthDeclaration = true;
                            var bitRanges = memberVariable.BitRange.Split(':');
                            if (bitRanges.Length == 1)
                            {
                                variableInfo.BitBegin = int.Parse(bitRanges[0]);
                                variableInfo.BitEnd = int.Parse(bitRanges[0]);
                            }
                            else
                            {
                                variableInfo.BitBegin = int.Parse(bitRanges[1]);
                                variableInfo.BitEnd = int.Parse(bitRanges[0]);
                            }

                            var wordRanges = memberVariable.WordOffset.Split(':');
                            if (wordRanges.Length == 1)
                            {
                                variableInfo.OffsetIn4ByteUnit = int.Parse(wordRanges[0]);
                            }
                            else
                            {
                                if (int.Parse(wordRanges[0]) > int.Parse(wordRanges[1]))
                                {
                                    throw new System.InvalidOperationException("WordOffsetRange must be wordRanges[0] <= wordRanges[1].");
                                }
                                variableInfo.OffsetIn4ByteUnit = int.Parse(wordRanges[0]);
                            }
                                
                            variableInfo.Modifier = memberVariable.Modifier;
                        }

                        structGenInfo.MemberVariableInfos.Add(variableInfo);
                    }

                    var structGenTuple =
                        new Tuple<Generator.StructGenerationInfo, Type>(structGenInfo, descConfig.Item2);
                    fileInfo.StructGenerationInfos.Add(structGenTuple);
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
                            this.SetCommonVariableInfo(argInfo, argConfig.VariableConfig);
                            argInfo.TypePrefix = argConfig.TypePrefix;
                            argInfo.TypeSuffix = argConfig.TypeSuffix;
                            memberFunctionInfo.ArgumentInfos.Add(argInfo);
                        }

                        classGenInfo.AddMemberFunctionInfo(memberFunctionInfo);

                        if(!memberFunctionInfo.IsInline)
                        {
                            var funcGen = new Generator.FunctionGenerationInfo();
                            funcGen.IsDeclaration = false;
                            var skeltonDefinition = funcGen.FunctionInfo;
                            skeltonDefinition.FunctionName =
                                classGenInfo.Name + "::" + memberFunctionInfo.FunctionName;
                            skeltonDefinition.StringAfterArgument = memberFunctionInfo.StringAfterArgument;
                            skeltonDefinition.ReturnType = memberFunctionInfo.ReturnType;

                            foreach (var argConfig in config.FunctionConfig.ArgumentConfigs)
                            {
                                var argInfo = new Generator.VariableInfo();
                                this.SetCommonVariableInfo(argInfo, argConfig.VariableConfig);
                                argInfo.TypePrefix = argConfig.TypePrefix;
                                argInfo.TypeSuffix = argConfig.TypeSuffix;
                                skeltonDefinition.ArgumentInfos.Add(argInfo);
                            }

                            if (fileInfoCpp.FunctionGenerationInfos == null)
                            {
                                fileInfoCpp.FunctionGenerationInfos = new List<Generator.FunctionGenerationInfo>();
                            }

                            fileInfoCpp.FunctionGenerationInfos.Add(funcGen);
                        }
                    }

                    foreach (var config in classConfig.MemberVariableConfigs)
                    {
                        var memberVariableInfo = new Generator.MemberVariableInfo();
                        var variableConfig = config.VariableDeclarationConfig.VariableConfig;
                        this.SetCommonVariableInfo(memberVariableInfo, variableConfig);
                        memberVariableInfo.AccessModifier = config.AccessModifier;
                        memberVariableInfo.IsDefineAccessor = config.IsDefineAccessor;
                        memberVariableInfo.IsInlineAccessor = config.IsInlineAccessor;
                        memberVariableInfo.IsAccessorReturnThis = config.IsAccessorReturnThis;

                        int idxOfFirstUpperCase = 0;
                        foreach (Char c in variableConfig.VariableName)
                        {
                            if (Char.IsUpper(c)) break;
                            ++idxOfFirstUpperCase;
                        }
                        memberVariableInfo.AccessorName = variableConfig.VariableName.Substring(idxOfFirstUpperCase);
                        classGenInfo.AddMemberVariableInfo(memberVariableInfo);

                        if(memberVariableInfo.IsDefineAccessor)
                        {
                            if (fileInfoCpp.FunctionGenerationInfos == null)
                            {
                                fileInfoCpp.FunctionGenerationInfos = new List<Generator.FunctionGenerationInfo>();
                            }

                            // アクセサ定義を必要としているがインラインじゃない場合は cpp に出力
                            if(!memberVariableInfo.IsInlineAccessor)
                            {
                                var funcGen = new Generator.FunctionGenerationInfo();
                                funcGen.IsDeclaration = false;
                                var skeltonDefinition = funcGen.FunctionInfo;
                                skeltonDefinition.FunctionName =
                                    classGenInfo.Name + "::" + "Get" + memberVariableInfo.AccessorName;
                                skeltonDefinition.StringAfterArgument = "const";
                                skeltonDefinition.ReturnType = memberVariableInfo.Type;
                                // TODO: struct 型はポインタにする
                                var arg = new Generator.VariableInfo();
                                SetCommonVariableInfo(arg, variableConfig);
                                arg.VariableName = char.ToLower(memberVariableInfo.AccessorName[0]) + memberVariableInfo.AccessorName.Substring(1);
                                skeltonDefinition.ArgumentInfos.Add(arg);
                                fileInfoCpp.FunctionGenerationInfos.Add(funcGen);
                            }

                            // setter
                            if (!memberVariableInfo.IsInlineAccessor)
                            {
                                // アクセサについても隠している時点で自分で実装したいのでスケルトンを cpp に出力する。
                                var funcGen = new Generator.FunctionGenerationInfo();
                                funcGen.IsDeclaration = false;
                                var skeltonDefinition = funcGen.FunctionInfo;
                                skeltonDefinition.FunctionName =
                                    classGenInfo.Name + "::" + "Set" + memberVariableInfo.AccessorName;
                                skeltonDefinition.StringAfterArgument = "";
                                skeltonDefinition.ReturnType = "void";
                                // TODO: struct 型はポインタにする
                                var arg = new Generator.VariableInfo();
                                SetCommonVariableInfo(arg, variableConfig);
                                arg.VariableName = char.ToLower(memberVariableInfo.AccessorName[0]) + memberVariableInfo.AccessorName.Substring(1);
                                skeltonDefinition.ArgumentInfos.Add(arg);
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
        private void SetCommonVariableInfo(Generator.VariableInfo variableInfo, VariableConfig config)
        {
            variableInfo.VariableName = config.VariableName;

            variableInfo.Type = config.Type.Split('[')[0];
            if (config.Type.Split('[').Length > 1)
            {
                variableInfo.ArrayLength = int.Parse(config.Type.Split('[')[1].Split(']')[0]);
            }
            else if(config.DefaultValues != null && (config.DefaultValues[0].IndexOf(".csv") != -1))
            {
                var file = new StreamReader(config.DefaultValues[0]).ReadToEnd();
                var lines = file.Split(new char[] { '\n' });
                if (lines[lines.Length - 1] == "")
                {
                    variableInfo.ArrayLength = lines.Length - 1;
                }
                else
                {
                    variableInfo.ArrayLength = lines.Length;
                }
            }
            else
            {
                variableInfo.ArrayLength = 0;
            }
            variableInfo.NameAlias = config.NameAlias;
            variableInfo.DoxyBrief = config.DoxyBrief;
            variableInfo.DefaultValue = GetDefaultValue(config);

            // override Type and NameAlias if it have dependency.
            if (config.Type.IndexOf(".json") != -1)
            {
                DeclarationCommonConfig declarationCommonConfig = new DeclarationCommonConfig();
                RootContext.GetType(declarationCommonConfig, config.Type);
                variableInfo.Type = declarationCommonConfig.DefinitionName;
                variableInfo.NameAlias = declarationCommonConfig.NameAlias;
            }

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
                List<string> requirements = new List<string>(requirement.Split(requirementInfo.RequirementValueSeparater));
                requirementInfo.Type = Generator.RequirementInfo.GetRequirementType(requirements[0]);
                // remove first element which shows requirement type.
                requirements.RemoveAt(0);
                requirementInfo.Values.AddRange(requirements);
                variableInfo.RequirementInfos.Add(requirementInfo);
            }
        }

        private string GetDefaultValue(VariableConfig config)
        {
            if (config.DefaultValues != null)
            {
                if ((config.DefaultValues[0].Length - config.DefaultValues[0].LastIndexOf(".csv")) == 4)
                {
                    // TODO: csv を read していい感じに {} を挿入する。
                    //      -> でもこの時に結局 struct の情報が必要になる。
                    //      -> ここから辿っても辿った先が分からない。
                    //          -> VariableConfig は所詮宣言だけをするためのものであって初期値の代入まではできない。
                    //              -> 自身からの相対パスなので辿れるか。
                    ReadDefaultValue readDefaultValue = new ReadDefaultValue();
                    string ret = "";
                    var defaultValues = readDefaultValue.GetCsvDefaultValue(config);
                    foreach(var value in defaultValues)
                    {
                        ret += value;
                    }
                    return ret;
                }
                else
                {
                    string ret = "";
                    for (int i = 0; i < config.DefaultValues.Length; ++i)
                    {
                        ret += config.DefaultValues[i];
                        if (i != config.DefaultValues.Length - 1)
                        {
                            ret += ", ";
                        }
                    }
                    return ret;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
