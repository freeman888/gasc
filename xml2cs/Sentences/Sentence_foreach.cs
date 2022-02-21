using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Resulters;

namespace xml2cs.Sentences
{
    internal class Sentence_foreach : ISentence
    {
        public string GasStr { get; set; }
        string var_togive = "";
        bool var_new = false;
        IResulter from;
        List<ISentence> sentences = new List<ISentence>();
        
        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            var_togive = element.GetAttribute("var_togive");
            var_new = Convert.ToBoolean(element.GetAttribute("var_new"));
            from = Resulter.LoadResulterFromXml(element.FirstChild.FirstChild as XmlElement);
            foreach(var i in element.ChildNodes[1].ChildNodes)
            {
                sentences.Add(Sentence.LoadSentencesFromXml(i as XmlElement));
            }
        }

        public string ToCsharp(string enviname)
        {
            var basecode =
                @"#region foreach_s {0}
try
                {{
                    Dictionary<string,Variable> {1} = Variable.GetOwnVariables({2});
                    {9}
                    foreach (var {4} in ({3}.value as Glist))
                    {{
                        try
                        {{
                            Dictionary<string,Variable> {5} = Variable.GetOwnVariables({2});
                            if ({6})
                                {5}.Add({7}, {4});
                            else
                                {5}[{7}] ={4};
                            {8}
                        }}
                        catch (Exceptions.BreakException)
                        {{
                            break;
                        }}
                        catch (Exceptions.ContinueException)
                        {{
                            continue;
                        }}
                        catch (Exception ex)
                        {{
                            throw ex;
                        }}
                    }}
                }}
                catch (Exception ex)
                {{
                    if (ex is Exceptions.ISysException)
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{10}"");
                }}
#endregion";
            var _0 = GasStr;
            var _1 = Xml2cs.GetxcName();
            var _2 = enviname;
            var _3 = Xml2cs.GetvarName();
            var _4 = Xml2cs.GetvarName();
            var _5 = Xml2cs.GetxcName();
            var _6 = var_new.ToString().ToLower();
            var _7 = $"\"{var_togive}\"";
            var _9 = from.ToCsharp(_3,_1);
            var _8 = "";
            foreach (var i in sentences)
                _8 += i.ToCsharp(_5);
            var ret = string.Format(basecode, _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, GasStr.Replace("\"", "\"\""));
            return ret;

        }
    }
}
