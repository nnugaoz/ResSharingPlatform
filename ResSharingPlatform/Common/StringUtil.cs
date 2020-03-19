using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Common
{
    public class StringUtil
    {
        #region object转换成string
        /// <summary>
        /// 转换成string
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static string ObjToString(Object ob)
        {
            if (ob == null)
            {
                return "";
            }
            else
            {
                return ob.ToString();
            }
        }
        #endregion
    }
}