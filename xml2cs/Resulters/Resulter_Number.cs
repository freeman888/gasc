using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Resulters
{
    internal class Resulter_Number : IResulter
    {
        string value = "";
        public void LoadFromXml(XmlElement element)
        {
            value = element.GetAttribute("value");
        }

        public string ToCsharp(string varname,string enviname)
        {
            var bc = @"#region arg num
               var {0} = new Variable({1});
#endregion";
            var _0 = varname;
            var _1 = value;
            var ret = string.Format(bc,_0,_1);
            return ret;
        }
    }
}
