using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace xml2cs
{
    internal static class ExFunctions
    {
        public static string GetAttribute(this XmlNode xmlNode, string name)
        {
            if (xmlNode.Attributes[name] == null)
                return "";
            return xmlNode.Attributes[name].InnerText;
        }
    }
}
