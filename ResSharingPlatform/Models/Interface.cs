using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Models
{
    public class Interface
    {
        public int GetInterfaceListCount()
        {
            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + "with cte as( ";
                sql = sql + "             select id0=ROW_NUMBER()over(order by CreateDate desc),*  ";
                sql = sql + "               from T_Interface "; 
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

        public List<object> GetInterfaceList(int pagesize, int pagecurrent)
        {
            int iFrom = (pagecurrent - 1) * pagesize + 1;
            int iTo = pagecurrent * pagesize;
            
            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + "with cte as( ";
                sql = sql + "             select id0=ROW_NUMBER()over(order by CreateDate desc),*  ";
                sql = sql + "               from T_Interface ";
                sql = sql + "           ) ";
                sql = sql + " select * from cte ";
                sql = sql + " where id0 between " + iFrom + " and " + iTo + " ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                //保存数据表并返回
                Ret.Add(ds);

                return Ret; 
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return Ret;
            }
        }
    }
}