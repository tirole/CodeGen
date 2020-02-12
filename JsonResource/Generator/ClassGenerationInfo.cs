using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource.Generator
{
    public class ClassGenerationInfo
    {
        public ClassGenerationInfo()
        {
            PublicMemberVariableInfos = new List<MemberVariableInfo>();
            PrivateMemberVariableInfos = new List<MemberVariableInfo>();
            PublicMemberFunctionInfos = new List<MemberFunctionInfo>();
            PrivateMemberFunctionInfos = new List<MemberFunctionInfo>();
        }
        public string Name { get; set; }
        public string DoxyBrief { get; set; }
        public List<MemberVariableInfo> PublicMemberVariableInfos { get; set; }
        public List<MemberVariableInfo> PrivateMemberVariableInfos { get; set; }
        public List<MemberFunctionInfo> PublicMemberFunctionInfos{ get; set; }
        public List<MemberFunctionInfo> PrivateMemberFunctionInfos { get; set; }
        public void AddMemberVariableInfo(MemberVariableInfo info)
        {
            switch (info.AccessModifier)
            {
                case "private":
                case "":
                    PrivateMemberVariableInfos.Add(info);
                    break;
                case "public":
                    PublicMemberVariableInfos.Add(info);
                    break;

                default:
                    throw new System.InvalidOperationException("Not implemented access modifier.");
            }
        }
        public void AddMemberFunctionInfo(MemberFunctionInfo info)
        {
            switch (info.AccessModifier)
            {
                case "private":
                case "":
                    PrivateMemberFunctionInfos.Add(info);
                    break;
                case "public":
                    PublicMemberFunctionInfos.Add(info);
                    break;

                default:
                    throw new System.InvalidOperationException("Not implemented access modifier.");
            }
        }
    }
}
