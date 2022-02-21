using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Resulters;

namespace xml2cs.Sentences
{
    internal class Sentence_Return : ISentence

    {
        public string GasStr { get; set; }
        IResulter toret;
        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            toret = Resulter.LoadResulterFromXml(element.FirstChild as XmlElement);
        }

        public string ToCsharp(string enviname)
        {
            var bc = @"#region return_s {0}
try
                {{
                    {1}
                    throw new Exceptions.ReturnException() {{ toreturn = {2} }};
                }}
                catch (Exception ex)
                {{
                    if (ex.GetType() == typeof(Exceptions.ReturnException))
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{3}"" );
                }}
#endregion";
            var _0 = GasStr;
            var _2 = Xml2cs.GetvarName();
            var _1 = toret.ToCsharp(_2,enviname);
            var ret = string.Format(bc, _0, _1, _2, GasStr.Replace("\"", "\"\""));
            return ret;
        }
    }
}
