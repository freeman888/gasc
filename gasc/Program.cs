using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace gasc
{
    class Program
    {
        /// <summary>
        /// build gas and files to gaa eg: --name hello --targetpath f:\\chat(projectname)\\hello(gaaname) --version 1.0 --id 200002 --code f:\\chat\\background.gas f:\\chat\\mainpage.gas
        /// </summary>
        /// <param name="name">the name of gaa</param>
        /// <param name="targetpath">the path where file exists</param>
        /// <param name="version">the version of gaa</param>
        /// <param name="id">the id of gaa</param>
        /// <param name="code">the source code(gas)</param>
        /// <param name="dep">the dependence of this gaa</param>
        /// <param name="sup">which platform does this gaa support</param>
        /// <param name="writer">the writer of this gaa</param>
        /// <param name="helplink">the help link of this gaa</param>
        public static void Main(string name,
            string targetpath,
            string version,
            string id,
            string[] code,
            string[] dep = null,
            string[] sup = null,
            string writer = "personalwriter",
            string helplink = "gasoline@freeman")
        {
            if(name == null || targetpath == null||
                version == null||
                id == null||
                code == null
                )
            {
                Console.WriteLine("Please check the command params.\nUsing --help to get the usage");
            }
            //设置基本信息
            XmlDocument xmlDocument = new();
            XmlElement rootgaa = xmlDocument.CreateElement("gaa");
            rootgaa.SetAttribute("name", name);
            rootgaa.SetAttribute("source", "gas");
            rootgaa.SetAttribute("version", version);
            rootgaa.SetAttribute("writer", writer);
            rootgaa.SetAttribute("id", id);
            rootgaa.SetAttribute("helplink", helplink);

            XmlElement xml_supportplatforms = xmlDocument.CreateElement("supportplatforms");
            XmlElement xml_dependences = xmlDocument.CreateElement("dependences");

            string str_codes = "";
            foreach(var i in code)
            {
                using StreamReader sr = new(i);
                str_codes += sr.ReadToEnd() + "\r\n";
            }
            if (!Directory.Exists( Path.Combine( targetpath , "source"))) Directory.CreateDirectory( Path.Combine( targetpath, "source"));
            if (!Directory.Exists(Path.Combine(targetpath, "file"))) Directory.CreateDirectory(Path.Combine(targetpath, "file"));
            gastoxml.Out.Gas2IL(str_codes,Path.Combine( targetpath , "source","code.xml"));


            if (dep != null)
            {
                foreach (var i in dep)
                {
                    XmlElement xmlElement = xmlDocument.CreateElement("dependence");
                    xmlElement.SetAttribute("name", i);
                    xml_dependences.AppendChild(xmlElement);
                }
            }

            if (sup != null)
            {
                foreach (var i in sup)
                {
                    XmlElement xmlElement = xmlDocument.CreateElement("platform");
                    xmlElement.InnerText = i;
                    xml_supportplatforms.AppendChild(xmlElement);
                }
            }
            rootgaa.AppendChild(xml_dependences);
            rootgaa.AppendChild(xml_supportplatforms);
            xmlDocument.AppendChild(rootgaa);
            using (Stream sr = new FileStream(Path.Combine( targetpath , "information.xml"), FileMode.Create))
            {
                xmlDocument.Save(sr);
            }
            //最后一步，压缩文件



            string gaapath = Path.Combine( targetpath , "..","debug" , name + ".gaa");
            if (File.Exists(gaapath))
                File.Delete(gaapath);
            ZipFile.CreateFromDirectory(targetpath, gaapath, CompressionLevel.NoCompression, true);

            Console.WriteLine("maybe everything done.");

        }
    }
}
