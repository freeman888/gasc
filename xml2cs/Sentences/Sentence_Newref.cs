using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Sentences
{
    internal class Sentence_Newref : ISentence
    {
        public string GasStr { get; set; }
        string varname = "";
        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            varname = element.GetAttribute("varname");
        }

        public string ToCsharp(string enviname)
        {
            var bc = @"#region var_s {0}
try
                {{
                    {1}.Add({2}, new Variable(0));
                }}
                catch (Exception ex)
                {{
                    if (ex is Exceptions.ISysException)
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{3}"" );
                }}
#endregion";
            var _0 = GasStr;
            var _1 = enviname;
            var _2 = $"\"{varname}\"";
            var ret = string.Format(bc, _0, _1, _2, GasStr.Replace("\"", "\"\""));
            return ret;
        }
    }
}
