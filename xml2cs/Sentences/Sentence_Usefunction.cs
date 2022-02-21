using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Resulters;
namespace xml2cs.Sentences
{
    internal class Sentence_Usefunction : ISentence
    {
        public string GasStr { get; set; }
        IResulter torun;


        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            torun = Resulter.LoadResulterFromXml(element.FirstChild as XmlElement);

        }

        public string ToCsharp(string enviname)
        {
            var bc = @"#region usefun_s {0}
try
                {{
                    {1}
                }}
                catch (Exception ex)
                {{
                    if (ex is Exceptions.ISysException)
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{2}"" );
                }}
#endregion";
            var _0 = GasStr;
            var _1 = torun.ToCsharp(Xml2cs.GetvarName(),enviname);
            var _2 = GasStr.Replace("\"", "\"\"");
            var ret = string.Format(bc, _0, _1,_2);
            return ret;
        }
    }
}
