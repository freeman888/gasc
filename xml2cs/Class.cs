using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using xml2cs.Functions;

namespace xml2cs
{
    internal class Class : IXml2cs
    {
        internal string name = "", parent = "";
        bool iscvf = false;
        List<string> members = new List<string>();
        
        Function_Initfun Function_Initfun = null;
        
        List<Function_Memfun> Function_Memfuns = new List<Function_Memfun>();
        internal string poslib = "";
        public void LoadFromXml(XmlElement element)
        {
            name = element.GetAttribute("name");
            parent = element.GetAttribute("parent");
            iscvf = Convert.ToBoolean(element.GetAttribute("cvf"));
            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.Name)
                {
                    case "member":
                        members.Add(node.GetAttribute("value"));
                        break;
                    case "memfun":
                        var fun = new Function_Memfun();
                        
                        fun.LoadFromXml(node as XmlElement);
                        Function_Memfuns.Add(fun);
                        break;
                    case "initfun":
                        var initfun = new Function_Initfun();
                        initfun.poslib = poslib;
                        initfun.LoadFromXml(node as XmlElement);
                        Function_Initfun = initfun;
                        break;
                }
            }
        }

        public string ToCsharp()
        {
            var basecode =
@"            public class {0} : GClassTemplate
            {{
                public {0}() : base({1}, {2})
                {{
                    iscvf = {3};
                    parentclassname = {4};
                    ctor = {5};
                    Istr_xcname = {6};
                    CVFFunction = {7};
                    //8是成员
                    {8}
                    //9是函数
                    {9}

                }}

                //函数的类的位置
                {10}
            }}";
            var _0 = "Class_" + name;
            var _1 = $"\"{name}\"";
            var _2 = $"\"{poslib}\"";
            var _3 = iscvf.ToString().ToLower();
            var _4 = $"\"{parent}\"";
            var _5 = Function_Initfun!= null? $"new InitFun_{name}()":"null";
            var _6 = Function_Initfun!= null?$"\"{Function_Initfun.param}\"":"\"\"";
            var _7 = iscvf?$"new Function_cvf()":"null";
            var _8 = "";
            foreach (var i in members)
                _8 += $"membernames.Add(\"{i}\");{Environment.NewLine}";
            var _9 = "";
            foreach(var i in Function_Memfuns)
            {
                if(i.funnname != "cvf")
                    _9 += $"memberfuncs.Add(\"{i.funnname}\",new Function_{i.funnname});{Environment.NewLine}";
            }
            var _10 = "";
            _10 += Function_Initfun.ToCsharp()+Environment.NewLine;
            foreach(var i in Function_Memfuns)
            {
                _10 += i.ToCsharp() + Environment.NewLine;
            }
            var ret = string.Format(basecode, _0, _1, _2, _3, _4,_5,_6,_7,_8,_9,_10);
            return ret;
        }
    }
}
