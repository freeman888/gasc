using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs.Resulters
{
    internal class Resulter_Function : IResulter
    {
        IResulter func = null;
        List<IResulter> param = new List<IResulter>();
        public void LoadFromXml(XmlElement element)
        {
            func = Resulter.LoadResulterFromXml(element.FirstChild.FirstChild as XmlElement);
            foreach(var i in element.ChildNodes[1].ChildNodes)
            {
                param.Add(Resulter.LoadResulterFromXml(i as XmlElement));
            }
        }

        public string ToCsharp(string varname, string enviname)
        {
            var bc = @"#region arg fun
                //funname
                {0}
                //params
                {1}
                var {3} = {2}.value as IFunction;
                Variable {5} ;
                if({3}.Iisasync)
                    {5} = await {3}.IAsyncRun(Resulter.Setvariablesname({3}.Istr_xcname, new ArrayList {{ {4} }}, {3}.poslib)) as Variable;
                else
                    {5} = {3}.IRun(Resulter.Setvariablesname({3}.Istr_xcname, new ArrayList {{ {4} }}, {3}.poslib)) as Variable;
#endregion";
            var _2 = Xml2cs.GetvarName();
            var _0 = func.ToCsharp(_2,enviname);
            var _1 = "";
            var _4 = "";
            foreach(var i in param)
            {
                var temp = Xml2cs.GetvarName();
                _1 += i.ToCsharp(temp, enviname)+Environment.NewLine;
                _4 += temp + ",";
            }
            var _3 = Xml2cs.GetvarName();
            var _5 = varname;
            var ret = string.Format(bc,_0,_1,_2,_3,_4,_5);
            return ret;
        }
    }
}
