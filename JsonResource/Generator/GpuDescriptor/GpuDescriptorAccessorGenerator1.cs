﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン: 16.0.0.0
//  
//     このファイルへの変更は、正しくない動作の原因になる可能性があり、
//     コードが再生成されると失われます。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace JsonResource.Generator.GpuDescriptor
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class GpuDescriptorAccessorGenerator : GpuDescriptorAccessorGeneratorBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
 // セッター・ゲッター定義 
foreach(var member in Info.MemberVariableInfos) { 
        string upperCamelVariableName = char.ToUpper(member.VariableName[0]) + member.VariableName.Substring(1);
        string functionNameSuffix = Info.Name + upperCamelVariableName;
        string inputType = member.Type;
        string inputVariableName = member.VariableName;
        string memcpySize = "";
        string memcpySrcDstVariableName = "";
        string pointerGetterReturnType = "";
        bool isInputTypePointer = false;
        bool isArray = member.ArrayLength > 0;
        if(member.TypeSuffix != null && member.TypeSuffix != "")
        {
            inputType += " " + member.TypeSuffix;
            isInputTypePointer = member.TypeSuffix == "*";
        }

        if(isInputTypePointer)
        {
            inputVariableName = "p" + upperCamelVariableName;
            memcpySize = "sizeof(" + member.Type + ")";
            memcpySrcDstVariableName = inputVariableName;
            pointerGetterReturnType = inputType;
        }
        else if(isArray)
        {
            inputVariableName = member.VariableName + "[" + member.ArrayLength + "]";
            memcpySize = "sizeof(" + member.Type  + ")" + " * " + member.ArrayLength;
            memcpySrcDstVariableName = member.VariableName;
            pointerGetterReturnType = inputType + "*";
        }

        int bitLength = member.BitEnd - member.BitBegin + 1;

        // 配列は常に memcpy とする
        bool isNeedsMemcpy = (isArray || isInputTypePointer) && ((bitLength % 32) == 0);
        if(isNeedsMemcpy)
        {
            if(member.HasModifier())
            {
                throw new System.InvalidOperationException("Cannot handle modifier for memcpy.\n");
            }
            if(member.BitBegin != 0)
            {
                throw new System.InvalidOperationException("Invalid bit begin, cannot perform memcpy.\n");
            }
        }

        bool isExact32bitVariable = (member.BitEnd - member.BitBegin) == 31;
        bool isExact64bitVariable = (member.BitEnd - member.BitBegin) == 63;
        bool isGreaterThan32bit = (member.BitEnd - member.BitBegin) >= 32;
        bool isZeroStartVariable = (member.BitBegin == 0);
        // 現状は 33bit 以上でしか存在しないので条件を決め打ち
        bool isCrossingVariable =  isGreaterThan32bit && !isZeroStartVariable;
        
        string inputTempValueString = "";
        string inputTempValueType = "";
        string returnValueString = "";
        string outputTempValueType = "";
        // for general case
        string outputValueString = "";

        if(isGreaterThan32bit)
        {
            inputTempValueType = "uint64_t";
        }
        else
        {
            inputTempValueType = "uint32_t";
        }

        if(isInputTypePointer)
        {
            inputTempValueString = "*reinterpret_cast<" + inputTempValueType + "*>(" + inputVariableName + ")";
            returnValueString = "*reinterpret_cast<" + inputType + ">(&outputVal)";
            outputTempValueType = inputTempValueType;
        }
        else
        {
            inputTempValueString = "static_cast<" + inputTempValueType + ">(" + member.VariableName + ")";
            returnValueString = "outputVal";
            outputTempValueType = member.Type;
            if(inputType == "bool")
            {
                outputValueString = "((pDesc->data[uint32ArrayIndex] & mask) >> bitOffset) != 0";
            }
            else
            {
                outputValueString = "static_cast<" + outputTempValueType + ">((pDesc->data[uint32ArrayIndex] & mask) >> bitOffset)";
            }
        }


        if(isNeedsMemcpy) { 
            this.Write("inline\r\nvoid Set");
            this.Write(this.ToStringHelper.ToStringWithCulture(functionNameSuffix));
            this.Write("(");
            this.Write(this.ToStringHelper.ToStringWithCulture(Info.Name));
            this.Write("* pDesc, const ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputType));
            this.Write(" ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputVariableName));
            this.Write(")\r\n{\r\n    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    memcpy(&pDesc->data[uint32ArrayIndex], ");
            this.Write(this.ToStringHelper.ToStringWithCulture(memcpySrcDstVariableName));
            this.Write(", ");
            this.Write(this.ToStringHelper.ToStringWithCulture(memcpySize));
            this.Write(");\r\n}\r\n");
        }else {
            this.Write("inline\r\nvoid Set");
            this.Write(this.ToStringHelper.ToStringWithCulture(functionNameSuffix));
            this.Write("(");
            this.Write(this.ToStringHelper.ToStringWithCulture(Info.Name));
            this.Write("* pDesc, ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputType));
            this.Write(" ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputVariableName));
            this.Write(")\r\n{\r\n");
            if(member.RequirementInfos != null) { 
                foreach(var info in member.RequirementInfos) { 
                    if(info.Type != RequirementInfo.RequirementType.NoRequirement) { 
            this.Write("    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(info.GetString(member.VariableName)));
            this.Write("\r\n");
                    } 
                } 
            } 
            if((isExact64bitVariable || isExact32bitVariable) && isZeroStartVariable) { 
            this.Write("    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    auto pOut = reinterpret_cast<");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.Type));
            this.Write("*>(&pDesc->data[uint32ArrayIndex]);\r\n    *pOut = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.VariableName));
            this.Write(";\r\n");
                if(member.HasModifier()) { 
            this.Write("    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.GetModifierString("*pOut")));
            this.Write(";\r\n");
                } 
            } else if(isCrossingVariable) { 
            this.Write("    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    constexpr int bitOffset = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitBegin));
            this.Write(";\r\n    constexpr int lowBitLength = 32 - bitOffset;\r\n    constexpr uint32_t lowBi" +
                    "tMask = static_cast<uint32_t>(~(-1LL << lowBitLength ));\r\n    constexpr int high" +
                    "BitLength = (");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitEnd));
            this.Write(" - ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitBegin));
            this.Write(" + 1) - lowBitLength;\r\n    constexpr uint32_t highBitMask = static_cast<uint32_t>" +
                    "(~(-1LL << highBitLength));\r\n\r\n    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputTempValueType));
            this.Write(" inputVal = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputTempValueString));
            this.Write(";\r\n");
                if(member.HasModifier()) { 
            this.Write("    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.GetModifierString()));
            this.Write(";\r\n");
                } 
            this.Write(@"    int lowVal = inputVal & lowBitMask;
    int highVal = (inputVal >> lowBitLength) & highBitMask;

    // low side
    pDesc->data[uint32ArrayIndex] &= ~(lowBitMask << bitOffset);
    pDesc->data[uint32ArrayIndex] |= lowVal << bitOffset;
    // high side
    pDesc->data[uint32ArrayIndex + 1] &= ~highBitMask;
    pDesc->data[uint32ArrayIndex + 1] |= highVal;
    
");
            } else { 
            this.Write("    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    constexpr int bitOffset = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitBegin));
            this.Write(";\r\n    constexpr int bitLength = (");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitEnd));
            this.Write(" - bitOffset) + 1;\r\n    constexpr uint32_t mask = static_cast<uint32_t>(~(-1LL <<" +
                    " bitLength ));\r\n    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputTempValueType));
            this.Write(" inputVal = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputTempValueString));
            this.Write(";\r\n");
                if(member.HasModifier()) { 
            this.Write("    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.GetModifierString()));
            this.Write(";\r\n");
                } 
            this.Write("    pDesc->data[uint32ArrayIndex] |= (inputVal & mask) << bitOffset;\r\n");
            } 
            this.Write("}\r\n");
        } 
            this.Write("\r\n");
        if(isNeedsMemcpy) { 
            this.Write("inline\r\nvoid Get");
            this.Write(this.ToStringHelper.ToStringWithCulture(functionNameSuffix));
            this.Write("(");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputType));
            this.Write(" ");
            this.Write(this.ToStringHelper.ToStringWithCulture(inputVariableName));
            this.Write(", const ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Info.Name));
            this.Write("* pDesc)\r\n{\r\n    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    memcpy(");
            this.Write(this.ToStringHelper.ToStringWithCulture(memcpySrcDstVariableName));
            this.Write(", &pDesc->data[uint32ArrayIndex], ");
            this.Write(this.ToStringHelper.ToStringWithCulture(memcpySize));
            this.Write(");\r\n}\r\n\r\ninline\r\n");
            this.Write(this.ToStringHelper.ToStringWithCulture(pointerGetterReturnType));
            this.Write(" Get");
            this.Write(this.ToStringHelper.ToStringWithCulture(functionNameSuffix));
            this.Write("Pointer(");
            this.Write(this.ToStringHelper.ToStringWithCulture(Info.Name));
            this.Write("* pDesc)\r\n{\r\n    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    return reinterpret_cast<");
            this.Write(this.ToStringHelper.ToStringWithCulture(pointerGetterReturnType));
            this.Write(">(&pDesc->data[uint32ArrayIndex]);\r\n}\r\n");
        }else {
            this.Write("inline\r\n");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.Type));
            this.Write(" Get");
            this.Write(this.ToStringHelper.ToStringWithCulture(functionNameSuffix));
            this.Write("(const ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Info.Name));
            this.Write("* pDesc)\r\n{\r\n");
            if((member.BitEnd - member.BitBegin) == 63) { 
            this.Write("    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    auto pOut = reinterpret_cast<const ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.Type));
            this.Write("*>(&pDesc->data[uint32ArrayIndex]);\r\n    return *pOut;\r\n");
            } else if((member.BitEnd - member.BitBegin) >= 32 && (member.BitBegin != 0)) { 
            this.Write("    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    constexpr int bitOffset = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitBegin));
            this.Write(";\r\n    constexpr int lowBitLength = 32 - bitOffset;\r\n    constexpr uint32_t lowBi" +
                    "tMask = static_cast<uint32_t>(~(-1LL << lowBitLength));\r\n    constexpr int highB" +
                    "itLength = (");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitEnd));
            this.Write(" - ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitBegin));
            this.Write(" + 1) - lowBitLength;\r\n    constexpr uint32_t highBitMask = static_cast<uint32_t>" +
                    "(~(-1LL << highBitLength));\r\n\r\n    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.Type));
            this.Write(" outputVal = 0;\r\n    outputVal = (pDesc->data[uint32ArrayIndex] & (lowBitMask << " +
                    "bitOffset)) >> bitOffset;\r\n    outputVal |= static_cast<uint64_t>((pDesc->data[u" +
                    "int32ArrayIndex + 1] & highBitMask)) << lowBitLength;\r\n    return ");
            this.Write(this.ToStringHelper.ToStringWithCulture(returnValueString));
            this.Write(";\r\n");
            } else { 
            this.Write("    constexpr int uint32ArrayIndex = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.OffsetIn4ByteUnit));
            this.Write(";\r\n    constexpr int bitOffset = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitBegin));
            this.Write(";\r\n    constexpr int bitLength = (");
            this.Write(this.ToStringHelper.ToStringWithCulture(member.BitEnd));
            this.Write(" - bitOffset) + 1;\r\n    constexpr uint32_t mask = static_cast<uint32_t>(~(-1LL <<" +
                    " bitLength )) << bitOffset;\r\n    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(outputTempValueType));
            this.Write(" outputVal = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(outputValueString));
            this.Write(";\r\n    return ");
            this.Write(this.ToStringHelper.ToStringWithCulture(returnValueString));
            this.Write(";\r\n");
            } 
            this.Write("}\r\n");
        } 
            this.Write("\r\n");
    } 
            return this.GenerationEnvironment.ToString();
        }
    }
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class GpuDescriptorAccessorGeneratorBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
