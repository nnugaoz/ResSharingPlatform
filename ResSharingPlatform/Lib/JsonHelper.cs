using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Data;

namespace ResSharingPlatform.Lib
{
    public  class JsonHelper
    {

        #region  DataSet转换为Json

        /// <summary>           
        /// DataSet转换为Json     
        /// </summary>       
        /// <param name="dataSet">DataSet对象</param>
        /// <returns>Json字符串</returns>  
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString;
        }
        #endregion

        #region Datatable转换为Json

        /// <summary>   
        /// Datatable转换为Json 
        /// </summary>      
        /// <param name="table">Datatable对象</param>
        /// <returns>Json字符串</returns>    
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName.ToLower();
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            if (jsonString.ToString() == "]")
            {
                jsonString.Remove(0, jsonString.Length);
                jsonString.Append("[]");
            }
            return jsonString.ToString();
        }

        /// <summary>  
        /// DataTable转换为Json 
        /// </summary>    
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        #endregion

        /// <summary>     
        /// 格式化字符型、日期型、布尔型 
        /// </summary>     
        /// <param name="str"></param>   
        /// <param name="type"></param> 
        /// <returns></returns>     
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        public static string String2Json(String s)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {

                char c = s.ToCharArray()[i];

                switch (c)
                {

                    case '\"':

                        sb.Append("\\\""); break;

                    case '\\':

                        sb.Append("\\\\"); break;

                    case '/':

                        sb.Append("\\/"); break;

                    case '\b':

                        sb.Append("\\b"); break;

                    case '\f':

                        sb.Append("\\f"); break;

                    case '\n':

                        sb.Append("\\n"); break;

                    case '\r':

                        sb.Append("\\r"); break;

                    case '\t':

                        sb.Append("\\t"); break;

                    default:

                        sb.Append(c); break;

                }

            }

            return sb.ToString();

        }
    }
}