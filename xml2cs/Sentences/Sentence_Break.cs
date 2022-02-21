using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Sentences
{
    internal class Sentence_Break : ISentence
    {
        public string GasStr { get; set; }

        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
        }

        public string ToCsharp(string s)
        {
            var basecode = @"
#region break_s {0}
try
                {{
                    throw new Exceptions.BreakException();
                }}
                catch (Exception ex)
                {{
                    if (ex.GetType() == typeof(Exceptions.BreakException))
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{1}"");
                }}
#endregion";
            var ret = string.Format(basecode, GasStr,GasStr.Replace("\"","\"\""));
            return ret;
        }
    }
}
