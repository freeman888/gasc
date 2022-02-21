using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Sentences;

namespace xml2cs.Functions
{
    internal class Function_Deffun : IXml2cs
    {
        internal string funnname = "";
        string param = "";
        bool isref = false;
        List<ISentence> sentences = new List<ISentence>();
        internal string poslib = "";
        public void LoadFromXml(XmlElement element)
        {
            funnname = element.GetAttribute("funname");
            param = element.GetAttribute("params");
            isref = Convert.ToBoolean(element.GetAttribute("isref"));
            foreach (XmlNode node in element.ChildNodes)
            {
                sentences.Add(Sentence.LoadSentencesFromXml(node as XmlElement));
            }
        }

        public string ToCsharp()
        {
            string basecode =
@"private class {0}: AFunction
{{
    public {0}()
    {{
        this.Iisreffunction = {1};
        this.Istr_xcname = {2};
        this.poslib = {3};
    }}

    public async override Task<object> Run(Dictionary<string,Variable> {5})
    {{
       try
                {{
                    {4}
                }}
                catch (Exceptions.ReturnException ex)
                {{
                    return ex.toreturn;
                }}
                catch (Exception ex)
                {{
                    throw ex;
                }}
                return new Variable(0);
    }}
}}";
            //0
            var _0 = "Func_" + funnname;
            //1
            var _1 = isref.ToString().ToLower();
            //2
            var _2 = $"\"{param}\"";
            //3
            var _3 = $"\"{poslib}\"";
            //4
            var _4 = "";
            //5
            var _5 = Xml2cs.GetxcName();
            sentences.ForEach((sentence) =>
            {
                _4 += sentence.ToCsharp(_5) + Environment.NewLine;
            });
            var ret = string.Format(basecode,_0,_1,_2,_3,_4,_5);
            return ret;
        }
    }
}
