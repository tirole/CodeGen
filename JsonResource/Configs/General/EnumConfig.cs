﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    [DataContract]
    public partial class EnumConfig : DeclarationConfig
    {
        [DataMember(Name = "underlyingType", IsRequired = true)]
        public string UnderlyingType;

        [DataMember(Name = "enums")]
        public Enum[] enums;
    }

    [DataContract]
    public partial class Enum
    {

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "value")]
        public string Value;

        [DataMember(Name = "doxyBrief")]
        public string DoxyBrief;
    }

}
