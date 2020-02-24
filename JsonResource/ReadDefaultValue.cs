using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonResource
{
    public class ReadDefaultValue
    {
        public class ReadValueContext
        {
            public void AddIndent()
            {
                for (int lineIndex = 0; lineIndex < Outputs.Length; ++lineIndex)
                {
                    for (int depth = 0; depth < Depth; ++depth)
                    {
                        Outputs[lineIndex] += "    ";
                    }
                }
            }
            public void AddString(string val)
            {
                for (int lineIndex = 0; lineIndex < Outputs.Length; ++lineIndex)
                {
                    Outputs[lineIndex] += val;
                }
            }
            public int Depth { get; set; }
            public int VariableIndex;
            public string[] Outputs { get; set; }
            public List<string> CsvLines { get; set; }
        }

        private ReadValueContext Context;

        private string GetPrimitiveTypeDefaultValue(VariableConfig config)
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

        private void ReadCsv(VariableConfig config)
        {
            if (config.DefaultValues[0].IndexOf(".csv") != -1)
            {
                Context.CsvLines = new List<string>();
                using (StreamReader sr = new StreamReader(config.DefaultValues[0]))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        Context.CsvLines.Add(line);
                    }
                }
            }
            else
            {
                throw new System.InvalidOperationException("Default value isn't from csv.");
            }
        }
        public string[] GetCsvDefaultValue(VariableConfig config)
        {
            Context = new ReadValueContext();
            ReadCsv(config);
            Context.Outputs = new string[Context.CsvLines.Count];
            
            if (config.Type.IndexOf(".json") != -1)
            {
                string jsonPath = config.Type;
                var typeDeclConfig = RootContext.Deserialize<DeclarationConfig>(jsonPath);
                var configType = RootContext.GetGenericDeserializerType(typeDeclConfig.Declaration.DefinitionType);
                if (configType == typeof(StructConfig))
                {
                    var typeConfig = RootContext.Deserialize<StructConfig>(jsonPath);
                    ++Context.Depth;
                    ReadRecursive(typeConfig);
                    --Context.Depth;
                }
            }
            return Context.Outputs;
        }

        private void ReadRecursive(StructConfig config)
        {
            Context.AddString("\n");
            Context.AddIndent();
            Context.AddString("{\n");
            ++Context.Depth;
            Context.AddIndent();
            foreach (var member in config.MemberVariables)
            {
                if (member.Type.IndexOf(".json") != -1)
                {
                    string jsonPath = member.Type;
                    var typeDeclConfig = RootContext.Deserialize<DeclarationConfig>(jsonPath);
                    var configType = RootContext.GetGenericDeserializerType(typeDeclConfig.Declaration.DefinitionType);
                    if (configType == typeof(StructConfig))
                    {
                        var typeConfig = RootContext.Deserialize<StructConfig>(jsonPath);
                        ReadRecursive(typeConfig);
                    }
                }
                else
                {
                    for (int i = 0; i < Context.CsvLines.Count; ++i)
                    {
                        if (Context.VariableIndex < Context.CsvLines[i].Length)
                        {
                            Context.Outputs[i] += Context.CsvLines[i].Split(',')[Context.VariableIndex];
                            Context.Outputs[i] += ", ";
                        }
                    }
                    ++Context.VariableIndex;
                }
            }
            --Context.Depth;
            Context.AddString("\n");
            Context.AddIndent();
            Context.AddString("},");
        }
    }
}
