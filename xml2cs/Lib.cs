using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using xml2cs.Functions;

namespace xml2cs
{
    public class Lib : IXml2cs
    {
        string name = "";
        List<string> gets = new List<string>();
        List<Function_Deffun> functions = new List<Function_Deffun>();
        List<Class> classes = new List<Class>();

        /// <summary>
        /// 通过xmlelement加载类，要求可以重复调用方法来加载不同文件中相同类名的文件
        /// </summary>
        /// <param name="element">lib的xmlelement</param>
        public void LoadFromXml(XmlElement element)
        {
            name = element.GetAttribute("name");
            foreach(XmlNode node in element.ChildNodes)
            {
                if(node.Name == "get")
                {
                    var value = node.GetAttribute("value");
                    if(!gets.Contains(value))
                    {
                        gets.Add(value);
                    }
                }
                else if(node.Name == "deffun")
                {
                    Function_Deffun item = new Function_Deffun();
                    item.poslib = name;
                    item.LoadFromXml(node as XmlElement);
                    functions.Add(item);
                }
                else if(node.Name == "cls")
                {
                    Class item = new Class();
                    item.poslib = name;
                    item.LoadFromXml(node as XmlElement);
                    classes.Add(item);
                }
                    
            }
        }

        public string ToCsharp()
        {
            var basecode =
@"using GI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GI.Function;

namespace {0}
{{
    internal class {1} : ILib
    {{
        public Dictionary<string, Variable> myThing {{ get; set; }} = new Dictionary<string, Variable>
        {{
            {2}
        }};

        public Dictionary<string, Variable> otherThing {{ get; set; }} = new Dictionary<string, Variable>();
        public List<string> waittoadd {{ get; set; }} = new List<string>
        {{
            {3}
        }};


        {4}

    }}
}}
";
            //{0}命名空间名称
            var nsname_0 = "NS_" + name;
            //1
            var csname_1 = "Lib_" + name;
            //2 mything
            var mything_2 = "";
            functions.ForEach((functions) =>
            {
                mything_2 += $"{{ \"{functions.funnname}\" , new Variable(new Func_{functions.funnname}() )}}, {Environment.NewLine}";
            });
            classes.ForEach((_class) =>
            {
                mything_2 += $"{{ \"{_class.name}\" , new Variable(new Class_{_class.name}() )}}, {Environment.NewLine}";
            });
            Debug.Write(mything_2);

            //{3}写get
            var getstr_3 = "";
            gets.ForEach((str) =>
            {
                getstr_3 += $"\"{str}\",{Environment.NewLine}";
            });

            //4
            var templatesstr_4 = "";
            functions.ForEach((func) =>
            {
                templatesstr_4 += func.ToCsharp() + Environment.NewLine;
            });
            classes.ForEach((_class) =>
            {
                templatesstr_4 += _class.ToCsharp() + Environment.NewLine;
            });


            var ret = string.Format( basecode,nsname_0,csname_1,mything_2,getstr_3,templatesstr_4);
            Debug.WriteLine(ret);
            return ret;
        }
    }
}
