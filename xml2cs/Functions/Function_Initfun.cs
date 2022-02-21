using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Resulters;
using xml2cs.Sentences;

namespace xml2cs.Functions
{
    internal class Function_Initfun : IXml2cs
    {
        public string poslib = "";
        internal string param = "";
        bool isref = false;
        IResulter baseresulter = null;
        List<ISentence> sentences = new List<ISentence>();
        public void LoadFromXml(XmlElement element)
        {
            param = element.GetAttribute("params");
            isref = Convert.ToBoolean(element.GetAttribute("isref"));
            if(element.FirstChild.HasChildNodes)
                baseresulter = Resulter.LoadResulterFromXml(element.FirstChild.FirstChild as XmlElement);
            if (element.ChildNodes[1].HasChildNodes)
            {
                foreach(var node in element.ChildNodes[1].ChildNodes)
                {
                    sentences.Add(Sentence.LoadSentencesFromXml(node as XmlElement));
                }
            }
        }

        public string ToCsharp()
        {
            string basecode =
                @"public class {0}:Fucntion
{{
    public Variable.BaseResulter baseresulter = null;
    public {0}()
    {{
        poslib = {1};
        str_xcname = {2};
        isreffunction = {3};
        Iisasync = false;
        
    }}

    public override object Run(Dictionary<string,Variable> {5})
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
            var _0 = $"Function_Init";
            var _1 = $"\"{poslib}\"";
            var _2 = $"\"{param}\"";
            var _3 = isref.ToString().ToLower();
            var _4 = "";
            var _5 =Xml2cs.GetxcName();
            foreach (var i in sentences)
            {
                _4 += i.ToCsharp(_5) + Environment.NewLine;
            }
            var ret = string.Format(basecode, _0, _1, _2, _3, _4,_5);
            return ret;
        }
    }
}
