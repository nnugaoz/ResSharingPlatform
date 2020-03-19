using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace ResSharingPlatform.Common
{
    public class SYSBLL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpDefault"></param>
        /// <param name="lpReturnedString"></param>
        /// <param name="nSize"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpappName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpstring"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern uint WritePrivateProfileString(string lpappName, string lpKeyName, string lpstring, string lpFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public string readoptions(string a1, string a2)
        {
            StringBuilder lpReturnedString = new StringBuilder(0x100);
            GetPrivateProfileString(a1, a2, "", lpReturnedString, 0x100, HttpContext.Current.Server.MapPath("~/Lib/dll/Config.dll"));
            return lpReturnedString.ToString().Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <param name="w3"></param>
        /// <returns></returns>
        public string writeoptions(string w1, string w2, string w3)
        {
            return ((long)WritePrivateProfileString(w1, w2, w3, HttpContext.Current.Server.MapPath("~/Lib/dll/Config.dll"))).ToString();
        }
    }
}

