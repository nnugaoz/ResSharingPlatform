using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Models
{
    public class School
    {
        public int GetSchoolListCount(string SchoolName)
        {

            if (string.IsNullOrEmpty(SchoolName)) SchoolName = "";


            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                //sql = sql + " with cte as(select id0=ROW_NUMBER()over(order by CreateTime desc),* from Z_School) ";
                //sql = sql + " select * from cte ";
                //sql = sql + " where SchoolName like '%" + SchoolName + "%'  ";

                sql = sql + "with cte as( ";
                sql = sql + "             select id0=ROW_NUMBER()over(order by CreateTime desc),*  ";
                sql = sql + "               from Z_School ";
                sql = sql + "              where SchoolName like '%" + SchoolName + "%'  ";
                sql = sql + "           ) ";
                sql = sql + " select * from cte ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                
                //返回
                return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return 0;
            }
        }

        public List<object> GetSchoolList(string SchoolName, int pagesize, int pagecurrent)
        {
            int iFrom = (pagecurrent - 1) * pagesize+1;
            int iTo = pagecurrent * pagesize;
            if (string.IsNullOrEmpty(SchoolName)) SchoolName = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                //sql = sql + " with cte as(select id0=ROW_NUMBER()over(order by CreateTime desc),* from Z_School) ";
                //sql = sql + " select * from cte ";
                //sql = sql + " where SchoolName like '%" + SchoolName + "%' and id0 between " + iFrom + " and " + iTo + " ";

                sql = sql + "with cte as( ";
                sql = sql + "             select id0=ROW_NUMBER()over(order by CreateTime desc),*  ";
                sql = sql + "               from Z_School ";
                sql = sql + "              where SchoolName like '%" + SchoolName + "%'  ";
                sql = sql + "           ) ";
                sql = sql + " select * from cte ";
                sql = sql + " where id0 between " + iFrom + " and " + iTo + " ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                //保存数据表并返回
                Ret.Add(ds);

                return Ret;
                //return "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) + "}";
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return Ret;
            }
        }

        public string GetSchoolBySchoolId(string SchoolId)
        {
            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";
                sql = sql + " select * from Z_School ";
                sql = sql + " where RecordId = '" + SchoolId + "'";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                //保存数据表并返回
                Ret.Add(ds);

                return "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) + "}";
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return ToJosn.wxListToJson(Ret);
            }
        }
    }
}