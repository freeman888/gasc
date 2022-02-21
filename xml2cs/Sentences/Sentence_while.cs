using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Resulters;

namespace xml2cs.Sentences
{
    internal class Sentence_while : ISentence
    {

        public string GasStr { get; set; }
        IResulter express;
        List<ISentence> sentences = new List<ISentence> ();
        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            express = Resulter.LoadResulterFromXml(element.FirstChild.FirstChild as XmlElement);
            foreach(var i in element.ChildNodes[1].ChildNodes)
            {
                sentences.Add(Sentence.LoadSentencesFromXml( i as XmlElement));            }
        }

        public string ToCsharp(string enviname)
        {
            var bc = @"#region while_s {0}
try
                {{
                    {2}
                    bool {1}= Convert.ToBoolean(({3}).value);
                    while ({1})
                    {{
                        Dictionary<string,Variable> {4}= Variable.GetOwnVariables({5});
                        
                            try
                            {{
                                 {6}
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
                        {7}
                        {1}= Convert.ToBoolean(({8}).value);
                    }}

                }}

                catch (Exception ex)
                {{
                    if (ex is Exceptions.ISysException)
                    {{
                        throw ex;
                    }}
                    throw new Exception(ex.Message + Environment.NewLine + @""位置:{9}"" );
                }}
#endregion";
            var _0 = GasStr;
            var _1 = Xml2cs.GetvarName();
            var _3 = Xml2cs.GetvarName();
            var _2 = express.ToCsharp(_3, enviname);
            var _4 = Xml2cs.GetxcName();
            var _5 = enviname;
            var _6 = "";
            foreach(var i in sentences)
            {
                _6 += i.ToCsharp(_4) + Environment.NewLine;
            }
            var _8 = Xml2cs.GetvarName();
            var _7 = express.ToCsharp(_8,enviname);
            var ret = string.Format(bc,_0,_1,_2,_3,_4,_5,_6,_7,_8, GasStr.Replace("\"", "\"\""));
            return ret;
        }
    }
}
