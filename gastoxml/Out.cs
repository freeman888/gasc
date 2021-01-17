using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace gastoxml
{
    public static class Out
    {
        /// <summary>
        /// 从 gas 文件到 IL 文件，如果xml文件存在就覆盖
        /// </summary>
        /// <param name="str_gas">gas(string)</param>
        /// <param name="outpath">IL文件地址</param>
        /// <returns>成功与否</returns>
        public static bool Gas2IL(string str_gas, string outpath)
        {
            try
            {
                
                App.AyalyseCode(str_gas, new System.Xml.XmlDocument(), outpath);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }
        public enum Mode { Function, Sentence }
        public static string IModeGas2IL(string gas, Mode mode)
        {
            if (mode == Mode.Sentence)
            {
                var sentences = gas.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var dones = new List<string>();
                dones.Add("fun IMode():");
                foreach (var item in sentences)
                {
                    dones.Add("    " + item);
                }
                string done = "";
                foreach (var item in dones)
                {
                    done += item + "\r\n";
                }
                return App.Getfunandvar(done, new System.Xml.XmlDocument(), null);
            }
            return null;
        }
    }
}
