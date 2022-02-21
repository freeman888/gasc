using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace xml2cs
{
    internal interface IXml2cs
    {
        /// <summary>
        /// 从xml反序列化
        /// </summary>
        /// <param name="element">外层的xmlelement</param>
        void LoadFromXml(XmlElement element);
        /// <summary>
        /// 输出cs代码
        /// </summary>
        /// <returns>csharp代码</returns>
        string ToCsharp();
    }
}
