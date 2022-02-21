using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Resulters
{
    internal class Resulter_Variable : IResulter
    {
        string value = "";
        public void LoadFromXml(XmlElement element)
        {
            value = element.GetAttribute("value");
        }

        public string ToCsharp(string varname, string enviname)
        {
            var bc = @"#region arg var
               var {0} = {1}[{2}];
#endregion";
            var _0 = varname;
            var _1 = enviname;
            var _2 = $"\"{value}\"";
            var ret = string.Format(bc, _0, _1, _2);
            return ret;
        }
    }
}
