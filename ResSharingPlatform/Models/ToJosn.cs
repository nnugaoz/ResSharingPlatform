using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ResSharingPlatform.Models
{
    public class ToJosn
    {
        public ToJosn()
        {

        }

        static public string StrToJosn(string str)
        {
            return "{" + StrFormat(str) + "}";
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        static private string StrFormat(string str)
        {
            str = str.Replace("\"", "");
            str = str.Replace("\'", "");
            str = str.Replace(" ", "");

            string[] l1 = str.Split(',');
            for (int i = 0; i < l1.Length; i++)
            {
                string[] l2 = l1[i].Split(':');

                l1[i] = "\"";
                for (int j = 0; j < l2.Length; j++)
                {
                    if (j == 0)
                    {
                        l1[i] += l2[j];
                    }
                    else if (j == 1)
                    {
                        l1[i] += "\":\"" + l2[j];
                    }
                    else
                    {
                        l1[i] += ":" + l2[j];
                    }
                }
                l1[i] += "\"";
            }

            str = "";
            for (int i = 0; i < l1.Length; i++)
            {
                if (i == 0)
                {
                    str += l1[i];
                }
                else
                {
                    str += "," + l1[i];
                }
            }

            return str;
        }

        /// <summary>
        /// 将参数转化成Josn格式
        /// </summary>
        /// <param name="l">需要转换的参数</param>
        /// <returns></returns>
        static public string ListToJson(List<object> l)
        {
            string json = "{\"para\":{%p1%},\"tables\":[%p2%]}";
            string p1 = "";
            string p2 = "";

            if (l.Count > 0)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    switch (l[i].GetType().Name)
                    {
                        case "String":
                            if (p1 != "")
                            {
                                p1 += ",";
                            }
                            p1 += StrFormat(l[i].ToString());
                            break;
                        case "DataSet":
                            DataSet ds = l[i] as DataSet;
                            if (ds.Tables.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables.Count; j++)
                                {
                                    if (p2 != "")
                                    {
                                        p2 += ",";
                                    }
                                    p2 += DataTableToJson(ds.Tables[j]);
                                }
                            }
                            break;
                    }
                }
                json = json.Replace("%p1%", p1);
                json = json.Replace("%p2%", p2);
            }

            return json;
        }

        /// <summary>
        /// 将参数转化成Josn格式
        /// </summary>
        /// <param name="l">需要转换的参数</param>
        /// <returns></returns>
        static public string wxListToJson(List<object> l)
        {
            string json = "%p2%";
            string p1 = "";
            string p2 = "";

            if (l.Count > 0)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    switch (l[i].GetType().Name)
                    {
                        case "String":
                            if (p1 != "")
                            {
                                p1 += ",";
                            }
                            p1 += StrFormat(l[i].ToString());
                            break;
                        case "DataSet":
                            DataSet ds = l[i] as DataSet;
                            if (ds.Tables.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables.Count; j++)
                                {
                                    if (p2 != "")
                                    {
                                        p2 += ",";
                                    }
                                    p2 += DataTableToJson(ds.Tables[j]);
                                }
                            }
                            break;
                    }
                }
                //    json = json.Replace("%p1%", p1);
                json = json.Replace("%p2%", p2);
            }

            return json;
        }


        /// <summary>
        /// 将DataTable转化成Josn格式
        /// </summary>
        /// <param name="dt">需要转换的DataTable</param>
        /// <returns></returns>
        static private string DataTableToJson(DataTable dt)
        {
            string json = "[%data%]";

            if (dt.Rows.Count <= 0)
            {
                return json.Replace("%data%", "");
            }

            List<string> col = new List<string>();
            if (dt.Columns.Count <= 0)
            {
                return json.Replace("%data%", "");
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                col.Add(dt.Columns[i].ColumnName.ToLower());
            }

            string row = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                {
                    row = ",{";
                }
                else
                {
                    row = "{";
                }
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j > 0)
                    {
                        row += ",";
                    }
                    row += "\"" + col[j] + "\"";
                    row += ":\"" + dt.Rows[i][j].ToString() + "\"";
                }
                row += "}%data%";

                json = json.Replace("%data%", row);
            }

            return json.Replace("%data%", "");
        }
    }
}