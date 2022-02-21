using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace xml2cs
{
    public class Xml2cs
    {
        public static void Xml2CS(string xmlfilepath,string outputfolderpath)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(xmlfilepath);
            var element = xmldoc.DocumentElement;
            var minversion = Convert.ToInt32( element.GetAttribute("minversion"));
            if(minversion < 2202)
            {
                throw new Exception();

            }
            var libs = new Dictionary<string, Lib>();
            foreach(XmlNode i in element.ChildNodes)
            {
                if(!libs.ContainsKey(i.Name))
                    libs.Add(i.Name,new Lib());
                var lib = libs[i.Name];
                lib.LoadFromXml(i as XmlElement);
            }
            foreach(var i in libs)
            {
                var cscode = i.Value.ToCsharp();

                string path = Path.Combine(outputfolderpath, i.Key + ".cs");
                if(File.Exists(path))
                    File.Delete(path);
                File.Create(path).Close();
                File.WriteAllText(path, cscode);
            }

        }

        static int xcindex = 0;
        public static string GetxcName()
        {
            return $"xc_{++xcindex}";
        }

        static int varindex=0;
        public static string GetvarName()
        {
            return $"var_{++varindex}";
        }
    }
}
