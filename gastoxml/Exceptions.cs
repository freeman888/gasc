using System;
using System.Collections.Generic;
using System.Text;

namespace gastoxml
{
    internal class Exceptions:Exception
    {
        public enum ID
        {
            类内部出现未知代号 = 0,
            库内部出现位置代号 = 1,
            构造函数
        }
        public ID id;
        public string message = "";
        public Exceptions(ID id)
        {
            this.id = id;
        }
        public override string Message => $"[{id}]:{message}";
    }
}
