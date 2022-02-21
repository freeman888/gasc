using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Sentences
{
    internal class Sentence_BreakPoint : ISentence
    {
        public string GasStr { get; set; }
        public void LoadFromXml(XmlElement element)
        {
            GasStr = "breakpoint;";
        }

        public string ToCsharp(string s)
        {
            var basecode = @"#region bpt_s {0}
try
                {{

                    Debugger.Chatwithclient(htxc);

                }}
                catch (Exception ex)
                {{
                    if (ex is Exceptions.ISysException)
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{1}"");
                }}
#endregion";
            var ret = string.Format(basecode, GasStr, GasStr.Replace("\"", "\"\""));
            return ret;
        }
    }
}
