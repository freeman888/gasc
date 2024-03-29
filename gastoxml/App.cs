﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace gastoxml
{
    public partial class App 
    {
        public static string gas = "";
        [STAThread]
        public static void GMain(string s)
        {
            try
            {
                gas = s;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        [STAThread]
        public static void Main(string[] ar)
        {

            gas = @"get Array;
get IO;
get Math;
fun Main(arg):
    IO.Tip(123456);
fun Test(t1,t2):
    IO.Write(""hello"");
";


            if (ar.Length == 2)
            {
                try
                {
                    Out.Gas2IL(ar[0], ar[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    Console.Read();
                }
            }
            else if (ar.Length == 1)
            {
                try
                {
                    Out.Gas2IL("f:\\1.gas", "f:\\code.xml");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    Console.Read();
                }
            }
            else
            {
                Console.WriteLine("input error");
                Console.WriteLine("params: [gas path] [xml path]");
                Console.Read();
            }


        }


        public static string Getfunandvar(string sourcecode, XmlDocument xmlDocument, string outpath)
        {

            XmlElement xmlElement = xmlDocument.CreateElement("code");
            xmlElement.SetAttribute("minversion", "1902");

            {

                string[] codes = sourcecode.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                Hashtable functions = new Hashtable();
                XmlElement x_lib = null, x_dflib = xmlDocument.CreateElement("lib");
                for (int index = 0; index < codes.Length;)
                {

                    if (codes[index] == "" || codes[index].Length < 6)
                    { index++; continue; }
                    string sign, content, end, code;
                    code = codes[index];
                    sign = code.Substring(0, 4);
                    end = code.Substring(code.Length - 1, 1);
                    if (end != ":" && end != ";")
                    {
                        index++; continue;
                    }
                    content = code.Substring(4, code.Length - 5);

                    if (sign == "lib " && end == ":")
                    {
                        x_lib = xmlDocument.CreateElement("lib");
                        x_lib.SetAttribute("name", content);
                        index++;


                        xmlElement.AppendChild(x_lib);
                    }
                    else
                    {
                        if (x_lib == null)
                            x_lib = x_dflib;
                        code = code.Substring(4);
                        sign = code.Substring(0, 4);
                        end = code.Substring(code.Length - 1, 1);

                        //2、判断为get 方法
                        if (sign == "get " && end == ";")
                        {
                            content = content.Replace(" ", "");
                            foreach (string item in content.Split(','))
                            {
                                XmlElement getter = xmlDocument.CreateElement("get");
                                getter.SetAttribute("value", item);
                                x_lib.AppendChild(getter);
                            }
                            index++; continue;
                        }
                        else if (sign == "var " && end == ";")
                        { //variables.Add(content, new Variable(0)); index++ ;
                            content = content.Replace(" ", "");
                            foreach (string item in content.Split(','))
                            {
                                XmlElement varer = xmlDocument.CreateElement("var");
                                varer.SetAttribute("value", item);
                                x_lib.AppendChild(varer);
                            }
                            index++; continue;
                        }
                        else if (sign == "fun " && end == ":")
                        {
                            bool isref = false;

                            string adj, functionname, xcname;
                            adj = content.Substring(0, content.IndexOf("("));
                            List<string> list = new List<string>(Regex.Split(adj, " +"));
                            functionname = list[list.Count - 1];
                            if (list.IndexOf("ref") != -1)
                                isref = true;

                            xcname = content.Substring(content.IndexOf("(") + 1, content.LastIndexOf(")") - content.IndexOf("(") - 1);

                            index++;
                            ArrayList array_str_sentences = new ArrayList();
                            for (; index < codes.Length && codes[index].Length >= 4 && codes[index].Substring(0, 4) == "    "; index++)
                            {
                                array_str_sentences.Add(codes[index].Substring(4, codes[index].Length - 4));
                            }
                            Function.New_User_Function new_User_Function = new Function.New_User_Function(functionname, xcname)
                            {
                                isreffunction = isref,
                                sentences = Sentence.GetSentencesfromArrayList(array_str_sentences)
                            };
                            functions.Add(functionname, new_User_Function);


                            new_User_Function.ToXml(xmlDocument, x_lib);

                            //432432432fafa


                        }

                        else if (sign == "cls ")
                        {
                            content = content.Replace(" ", "");
                            content = content.Substring(3);
                            index++;
                            ArrayList cls_content = new ArrayList();
                            for (; index < codes.Length && codes[index].Length >= 8 && codes[index].Substring(0, 8) == "        "; index++)
                            {
                                cls_content.Add(codes[index].Substring(8));
                            }

                        }
                        else
                        {
                            throw new Exception();
                        }
                    }




                }
                xmlElement.AppendChild(x_dflib);
                xmlDocument.AppendChild(xmlElement);
                if (outpath == null)
                {
                    return xmlElement.InnerXml;
                }
                StreamWriter streamWriter = new StreamWriter(outpath);
                xmlDocument.Save(streamWriter);
                streamWriter.Close();
                return "ok";
            }
        }



        public static void AyalyseCode(string sourcecode, XmlDocument xmlDocument, string outpath)
        {
            XmlElement xmlElement = xmlDocument.CreateElement("code");
            xmlElement.SetAttribute("minversion", "2202");
            string[] codes = sourcecode.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (var index = 0; index < codes.Length;)
            {
                var code = codes[index];
                if (code.Length < 6 || string.IsNullOrWhiteSpace(code))
                { index++; continue; }
                var start = code.Substring(0, 4);
                var end = code.Substring(code.Length - 1);
                if (end != ";" && end != ":")
                { index++; continue; }
                if (start == "lib ")
                {
                    index++;
                    var libname = code.Substring(4, code.Length - 5);
                    List<string> libcontents = new List<string>();
                    for (; index < codes.Length && codes[index].Length >= 4 && codes[index].Substring(0, 4) == "    "; index++)
                        libcontents.Add(codes[index].Substring(4));
                    LibCreat(libname, libcontents, xmlDocument, xmlElement);


                }
                else
                { Console.WriteLine("语法错误，没有使用lib 开头"); return; }
                xmlDocument.AppendChild(xmlElement);
            }

            xmlDocument.AppendChild(xmlElement);
            if(File.Exists(outpath))
            {
                File.Delete(outpath);
            }
            Stream stream = new FileStream(outpath, FileMode.OpenOrCreate);
            xmlDocument.Save(stream);
            stream.Close();

        }

        private static void LibCreat(string libname, List<string> libcontents, XmlDocument xmlDocument, XmlElement xmlElement)
        {
            XmlElement x_lib = xmlDocument.CreateElement("lib");
            x_lib.SetAttribute("name", libname);
            for (var index = 0; index < libcontents.Count;)
            {
                var code = libcontents[index];
                if (code.Length < 6 || string.IsNullOrWhiteSpace(code))
                { index++; continue; }
                var start = code.Substring(0, 4);
                var end = code.Substring(code.Length - 1);
                if (end != ";" && end != ":")
                { index++; continue; }
                var content = code.Substring(4, code.Length - 5);
                if (start == "get " && end == ";")
                {
                    content = content.Replace(" ", "");
                    foreach (string item in content.Split(','))
                    {
                        XmlElement getter = xmlDocument.CreateElement("get");
                        getter.SetAttribute("value", item);
                        x_lib.AppendChild(getter);
                    }
                    index++; continue;
                }
                else if (start == "var " && end == ";")
                { //variables.Add(content, new Variable(0)); index++ ;
                    content = content.Replace(" ", "");
                    foreach (string item in content.Split(','))
                    {
                        XmlElement varer = xmlDocument.CreateElement("var");
                        varer.SetAttribute("value", item);
                        x_lib.AppendChild(varer);
                    }
                    index++; continue;
                }
                else if (start == "fun " && end == ":")
                {
                    bool isref = false;

                    string adj, functionname, xcname;
                    adj = content.Substring(0, content.IndexOf("("));
                    List<string> list = new List<string>(Regex.Split(adj, " +"));
                    functionname = list[list.Count - 1];
                    if (list.IndexOf("ref") != -1)
                        isref = true;

                    xcname = content.Substring(content.IndexOf("(") + 1, content.LastIndexOf(")") - content.IndexOf("(") - 1);

                    index++;
                    ArrayList array_str_sentences = new ArrayList();
                    for (; index < libcontents.Count && libcontents[index].Length >= 4 && libcontents[index].Substring(0, 4) == "    "; index++)
                    {
                        array_str_sentences.Add(libcontents[index].Substring(4, libcontents[index].Length - 4));
                    }
                    Function.New_User_Function new_User_Function = new Function.New_User_Function(functionname, xcname)
                    {
                        isreffunction = isref,
                        sentences = Sentence.GetSentencesfromArrayList(array_str_sentences)
                    };
                    //functions.Add(functionname, new_User_Function);


                    new_User_Function.ToXml(xmlDocument, x_lib);

                    //432432432fafa
                }
                else if (start == "cls " && end == ":")
                {
                    index++;
                    List<string> clscontents = new List<string>();
                    for (; index < libcontents.Count && libcontents[index].Length >= 4 && libcontents[index].Substring(0, 4) == "    "; index++)
                    {
                        clscontents.Add(libcontents[index].Substring(4));
                    }
                    ClsCreat(content, clscontents, xmlDocument, x_lib);

                }
                else { Console.WriteLine("Fuckkkkk"); return; }

            }
            xmlElement.AppendChild(x_lib);
        }

        private static void ClsCreat(string cls, List<string> clscontents, XmlDocument xmlDocument, XmlElement x_lib)
        {

            XmlElement x_cls = xmlDocument.CreateElement("cls");
            string cls_name = "", cls_parent = "";
            string[] atts = null;
            var i = cls.IndexOf("(");
            if (i != -1)
            {
                

                atts = Regex.Split( cls.Substring(0, i)," +");
                cls_name = atts[atts.Length - 1];
                
                cls_parent = cls.Substring(i + 1, cls.IndexOf(")") - i - 1);

            }
            else
            {
                atts = Regex.Split(cls, " +");
                cls_name = atts[atts.Length - 1];
            }

            x_cls.SetAttribute("name", cls_name);
            x_cls.SetAttribute("parent", cls_parent);
            if(new ArrayList(atts).IndexOf("cvf") != -1)
            {
                x_cls.SetAttribute("cvf", "True");
            }
            else
            {

                x_cls.SetAttribute("cvf", "False");
            }

            for (var index = 0; index < clscontents.Count;)
            {
                var code = clscontents[index];
                if (code.Length < 6 || string.IsNullOrWhiteSpace(code))
                { index++; continue; }
                var start = code.Substring(0, 4);
                var end = code.Substring(code.Length - 1);
                if (end != ";" && end != ":")
                { index++; continue; }
                var content = code.Substring(4, code.Length - 5);
                if (start == "var " && end == ";")
                { //variables.Add(content, new Variable(0)); index++ ;
                    content = content.Replace(" ", "");
                    foreach (string item in content.Split(','))
                    {
                        XmlElement varer = xmlDocument.CreateElement("member");
                        varer.SetAttribute("value", item);
                        x_cls.AppendChild(varer);
                    }
                    index++; continue;
                }
                else if (start == "fun " && end == ":")
                {
                    bool isref = false;
                    bool _static = false;
                    string adj, functionname, xcname;
                    adj = content.Substring(0, content.IndexOf("("));
                    List<string> list = new List<string>(Regex.Split(adj, " +"));
                    list.RemoveAll(str => str == "");
                    functionname = list[list.Count - 1];
                    if (functionname == "init")
                    {
                        string basestr = "";
                        if(Regex.IsMatch(content, @"^\s{0,}init\s{0,}\(.{0,}\)\s{0,}:\s{0,}base\s{0,}\(.{0,}\)\s{0,}$"))
                        {
                            basestr = Regex.Match(content, @"base\s{0,}\(.{0,}\)\s{0,}$").Value;
                        }
                        if (list.IndexOf("ref") != -1)
                            isref = true;
                        xcname = content.Substring(content.IndexOf("(") + 1, content.IndexOf(")") - content.IndexOf("(") - 1);
                        index++;
                        ArrayList array_str_sentences = new ArrayList();
                        for (; index < clscontents.Count && clscontents[index].Length >= 4 && clscontents[index].Substring(0, 4) == "    "; index++)
                        {
                            array_str_sentences.Add(clscontents[index].Substring(4, clscontents[index].Length - 4));
                        }
                        var new_init_Function = new Function.New_Init_Function(xcname, basestr)
                        {
                            isreffunction = isref,
                            sentences = Sentence.GetSentencesfromArrayList(array_str_sentences)
                        };
                        //functions.Add(functionname, new_User_Function);


                        new_init_Function.ToXml(xmlDocument, x_cls);
                    }
                    else
                    {
                        if (list.IndexOf("ref") != -1)
                            isref = true;
                        if (list.IndexOf("static") != -1)
                            _static = true;
                        xcname = content.Substring(content.IndexOf("(") + 1, content.LastIndexOf(")") - content.IndexOf("(") - 1);

                        index++;
                        ArrayList array_str_sentences = new ArrayList();
                        for (; index < clscontents.Count && clscontents[index].Length >= 4 && clscontents[index].Substring(0, 4) == "    "; index++)
                        {
                            array_str_sentences.Add(clscontents[index].Substring(4, clscontents[index].Length - 4));
                        }
                        Function.New_Member_Function new_User_Function = new Function.New_Member_Function(functionname, xcname, _static)
                        {
                            isreffunction = isref,
                            sentences = Sentence.GetSentencesfromArrayList(array_str_sentences)
                        };
                        //functions.Add(functionname, new_User_Function);


                        new_User_Function.ToXml(xmlDocument, x_cls);
                    }
                    //432432432fafa
                }
                else {
                    throw new Exceptions(Exceptions.ID.类内部出现未知代号) { message="类名："+cls_name};
                }

            }
            x_lib.AppendChild(x_cls);
        }
    }
}
