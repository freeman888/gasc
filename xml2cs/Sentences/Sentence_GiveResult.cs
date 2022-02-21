using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Resulters;

namespace xml2cs.Sentences
{
    internal class Sentence_GiveResult : ISentence
    {
        public string GasStr { get; set; }
        IResulter from, to;
        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            to = Resulter.LoadResulterFromXml(element.FirstChild as XmlElement);
            from = Resulter.LoadResulterFromXml(element.ChildNodes[1] as XmlElement);

        }

        public string ToCsharp(string enviname)
        {
            var bc = @"#region getres_s {0}
try
                {{
                    {1}
                    {2}
                    {4}.value = {3}.value;
                }}
                catch (Exception ex)
                {{
                    if (ex is Exceptions.ISysException)
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{5}"" );
                }}
#endregion";
            var _0 = GasStr;
            var _3 = Xml2cs.GetvarName();
            var _4 = Xml2cs.GetvarName();
            var _1 = from.ToCsharp(_3,enviname);
            var _2 = to.ToCsharp(_4,enviname);
            var ret = string.Format(bc,_0,_1,_2,_3,_4, GasStr.Replace("\"", "\"\""));
            return ret;
        }
    }
}
