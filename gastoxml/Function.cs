﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace gastoxml
{
    public interface IFunction
    {
        
        string Istr_xcname { get; set; }
        bool Iisreffunction { get; set; }

    }

    public partial class Function : IFunction
    {
        //实现IFunction
        
        string IFunction.Istr_xcname
        {
            get
            {
                return str_xcname;
            }
            set
            {
                str_xcname = value;
            }
        }
        bool IFunction.Iisreffunction
        {
            get
            {
                return isreffunction;
            }
            set
            {
                isreffunction = value;
            }
        }


        public virtual object Run(Hashtable xc)//父方法
        {

            return new object();
        }
        public string str_xcname;
        public bool isreffunction = false;

        //以下为用户自定义函数
        public class New_User_Function : Function
        {
            public Sentence[] sentences;
            public string name;
            public New_User_Function(string fname, string fxc)
            {
                str_xcname = fxc; name = fname;
            }
            public void ToXml(XmlDocument xmlDocument, XmlElement xmlElement)
            {
                XmlElement myfun = xmlDocument.CreateElement("deffun");
                myfun.SetAttribute("funname", name);
                myfun.SetAttribute("params", str_xcname);
                myfun.SetAttribute("isref", isreffunction.ToString());
                foreach (var i in sentences)
                {
                    i.ToXml(xmlDocument, myfun);
                }
                xmlElement.AppendChild(myfun);
            }
        }

        public class New_Member_Function : Function
        {
            public Sentence[] sentences;
            public string name;
            public bool isstatic = false;
            public New_Member_Function(string fname, string fxc, bool _static)
            {
                str_xcname = fxc; name = fname; this.isstatic = _static;
            }
            public void ToXml(XmlDocument xmlDocument, XmlElement xmlElement)
            {
                XmlElement myfun = xmlDocument.CreateElement("memfun");
                myfun.SetAttribute("funname", name);
                myfun.SetAttribute("params", str_xcname);
                myfun.SetAttribute("isref", isreffunction.ToString());
                myfun.SetAttribute("isstatic", isstatic.ToString());
                foreach (var i in sentences)
                {
                    i.ToXml(xmlDocument, myfun);
                }
                xmlElement.AppendChild(myfun);
            }
        }

        public class New_Init_Function : Function
        {
            public Sentence[] sentences;
            public Variable.Resulter resulter = null;
            public New_Init_Function(string fxc, string basstr)
            {
                if(!string.IsNullOrEmpty(basstr))
                    resulter = new Variable.Resulter(basstr);
                str_xcname = fxc;
            }
            public void ToXml(XmlDocument xmlDocument, XmlElement xmlElement)
            {
                XmlElement myfun = xmlDocument.CreateElement("initfun");
                var basefun = xmlDocument.CreateElement("base");
                if(resulter != null)
                    resulter.ToXml(xmlDocument, basefun);

                var xmlsentroot = xmlDocument.CreateElement("sentences");
                myfun.SetAttribute("params", str_xcname);
                myfun.SetAttribute("isref", isreffunction.ToString());
                foreach (var i in sentences)
                {
                    i.ToXml(xmlDocument, xmlsentroot);
                }
                myfun.AppendChild(basefun);
                myfun.AppendChild(xmlsentroot);
                xmlElement.AppendChild(myfun);
            }
        }


    }
}
