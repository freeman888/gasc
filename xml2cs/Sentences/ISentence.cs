using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Sentences
{
    internal interface ISentence
    {
        string GasStr { get; set; }
        string ToCsharp(string enviname);
    }

    internal class Sentence
    {
        internal static ISentence LoadSentencesFromXml(XmlElement element)
        {
            ISentence toret = null ;
            switch (element.Name)
            {
                case "if_s":
                    Sentence_if ifs = new Sentence_if();
                    ifs.LoadFromXml(element as XmlElement);
                    toret = (ifs);
                    break;
                case "while_s":
                    Sentence_while sentence_While = new Sentence_while();
                    sentence_While.LoadFromXml(element as XmlElement);
                    toret = (sentence_While);
                    break;
                case "foreach_s":
                    Sentence_foreach sentence_Foreach = new Sentence_foreach();
                    sentence_Foreach.LoadFromXml(element as XmlElement);
                    toret = (sentence_Foreach);
                    break;
                case "try_s":
                    Sentence_try sentence_Try = new Sentence_try();
                    sentence_Try.LoadFromXml(element as XmlElement);
                    toret = (sentence_Try);
                    break;
                case "var_s":
                    Sentence_Newref sentence_Newref = new Sentence_Newref();
                    sentence_Newref.LoadFromXml(element as XmlElement);
                    toret = (sentence_Newref);
                    break;
                case "usefun_s":
                    Sentence_Usefunction sentence_Usefunction = new Sentence_Usefunction();
                    sentence_Usefunction.LoadFromXml(element as XmlElement);
                    toret = (sentence_Usefunction);
                    break;
                case "getres_s":
                    Sentence_GiveResult sentence_GiveResult = new Sentence_GiveResult();
                    sentence_GiveResult.LoadFromXml(element as XmlElement);
                    toret = (sentence_GiveResult);
                    break;
                case "return_s":
                    Sentence_Return sentence_Return = new Sentence_Return();
                    sentence_Return.LoadFromXml(element as XmlElement);
                    toret = (sentence_Return);
                    break;
                case "breakpoint":
                    Sentence_BreakPoint sentence_BreakPoint = new Sentence_BreakPoint();
                    sentence_BreakPoint.LoadFromXml(element as XmlElement);
                    toret = (sentence_BreakPoint);
                    break;
                case "break_s":
                    Sentence_Break sentence_Break = new Sentence_Break();
                    sentence_Break.LoadFromXml(element as XmlElement);
                    toret = (sentence_Break);
                    break;
                case "continue_s":
                    Sentence_Continue sentence_Continue = new Sentence_Continue();
                    sentence_Continue.LoadFromXml(element as XmlElement);
                    toret = (sentence_Continue);
                    break;
                default:
                    throw new Exception();
            }
            return toret;
        }
    }
}
