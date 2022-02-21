using System;

namespace gasctest
{
    class Program
    {
        static void Main(string[] args)
        {
            //gastoxml.Out.Gas2IL("f:\\1.gas", "f:\\code.xml");
            xml2cs.Xml2cs.Xml2CS(@"E:\projects\freestudio\App1\App1\source\code.xml", "C:\\Users\\Freeman\\Desktop");
        }
    }
}
