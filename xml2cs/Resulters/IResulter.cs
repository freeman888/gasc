using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Resulters
{
    interface IResulter  {
        void LoadFromXml(XmlElement xmlElement);
        string ToCsharp(string varname,string enviname);
    }

    internal class Resulter
    {
        internal static IResulter LoadResulterFromXml(XmlElement element)
        {
            IResulter toret = null;
            var con = element.GetAttribute("con");
            switch (con)
            {
                case "fun":
                    toret = new Resulter_Function();
                    toret.LoadFromXml(element);
                    break;
                case "mem":
                    toret = new Resulter_GetMember();
                    toret.LoadFromXml(element);
                    break;
                case "num":
                    toret = new Resulter_Number();
                    toret.LoadFromXml(element);
                    break;
                case "str":
                    toret = new Resulter_String();
                    toret.LoadFromXml(element);
                    break;
                case "var":
                    toret = new Resulter_Variable();
                    toret.LoadFromXml(element);
                    break;

                default:
                    throw new Exception();

            }
            return toret;
        }
    }
}
