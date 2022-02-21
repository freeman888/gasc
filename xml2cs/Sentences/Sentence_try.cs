using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Sentences
{
    internal class Sentence_try : ISentence
    {
        public string GasStr { get; set; }
        List<ISentence> then = new List<ISentence>(),_catch = new List<ISentence>();
        bool varnew = false;
        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            varnew = Convert.ToBoolean(element.GetAttribute("var_new"));
            foreach(var i in element.FirstChild.ChildNodes)
            {
                then.Add(Sentence.LoadSentencesFromXml(i as XmlElement));
            }
            foreach(var i in  element.ChildNodes[1].ChildNodes)
            {
                _catch.Add(Sentence.LoadSentencesFromXml(i as XmlElement));
            }

        }

        public string ToCsharp(string enviname)
        {
            var bc = @"#region try_s {0}
try
                {{
                    Dictionary<string,Variable> {1} = Variable.GetOwnVariables({2});
                    try
                    {{
                        {3}
                    }}
                    catch (Exception ex)
                    {{
                        if (ex is Exceptions.ISysException)
                        {{
                            throw ex;
                        }}
                        if ({4})
                            {1}.Add({4}, new Variable(ex.Message));
                        else
                            {1}[{4}] = new Variable(ex.Message);
                        {5}
                    }}
                }}
                catch (Exception ex)
                {{
                    if (ex is Exceptions.ISysException)
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{6}"" );
                }}
            }}
#endregion";
            var _0 = GasStr;
            var _1 = Xml2cs.GetvarName();
            var _2 = enviname;
            var _3 = "";
            foreach(var i in then)
            {
                _3+=i.ToCsharp(_1)+Environment.NewLine;
            }
            var _4 = varnew.ToString().ToLower();
            var _5 = "";
            foreach(var i in _catch)
            {
                _5 += i.ToCsharp(_1) + Environment.NewLine;
            }
            var ret = string.Format(bc, _0, _1, _2, _3, _4, _5, GasStr.Replace("\"", "\"\""));
            return ret;
        }
    }
}
