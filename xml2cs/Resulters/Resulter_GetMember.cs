using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Resulters
{
    internal class Resulter_GetMember : IResulter
    {
        IResulter from = null;
        string memname = "";
        public void LoadFromXml(XmlElement element)
        {
            from = Resulter.LoadResulterFromXml(element.FirstChild as XmlElement);
            memname = element.GetAttribute("value");
        }

        public string ToCsharp(string varname,string enviname)
        {
            string bc = @"#region arg getmember
               {0}
               var {3} =  {1}.value.IGetMember({2});
#endregion";
            var _1 = Xml2cs.GetvarName();
            var _0 = from.ToCsharp(_1,enviname);
            var _3 = varname;
            var _2 = $"\"{memname}\"";
            var ret = string.Format(bc,_0,_1,_2,_3);
            return ret;
        }
    }
}
