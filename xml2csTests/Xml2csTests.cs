using Microsoft.VisualStudio.TestTools.UnitTesting;
using xml2cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace xml2cs.Tests
{
    [TestClass()]
    public class Xml2csTests
    {
        [TestMethod()]
        public void Xml2CSTest()
        {
            Xml2cs.Xml2CS(@"E:\projects\freestudio\App1\App1\source\code.xml", "");
            Assert.Fail();
        }
    }
}