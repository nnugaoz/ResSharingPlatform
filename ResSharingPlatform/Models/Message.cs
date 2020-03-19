using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Models
{
    public class Message
    {
        public int GetSendListCount(string userid)
        {
             

            List<object> Ret = new List<object>();
            try
            {
                
                string sql = " ";
                sql = sql + " with cte as(select id0=ROW_NUMBER()over(order by Send_Time desc),* from T_Message) ";
                sql = sql + " select * from cte ";
                sql = sql + " where Send_Id = '" + userid + "'  ";

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

        public DataSet GetSendList(string userid, int pagesize, int pagecurrent)
        {
            int iFrom = (pagecurrent - 1) * pagesize + 1;
            int iTo = pagecurrent * pagesize;
            
            try
            {
                
                string sql = " ";
                sql = sql + "with cte as(";
                sql = sql + "select id0=ROW_NUMBER()over(order by m.Send_Time desc), m.* ";
                sql = sql + " from T_Message m";
                sql = sql + " where  m.Send_Id = '" + userid + "'";
                sql = sql + "     )";
                sql = sql + "SELECT * FROM cte";
                sql = sql + " where  id0 between " + iFrom + " and " + iTo + " ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql); 
                return ds;
                
            }
            catch (Exception ex)
            { 
                return null;
            }
        }


        public int GetReceiveListCount(string userid)
        {
            List<object> Ret = new List<object>();
            try
            {

                string sql = " ";
                sql = sql + " select * from T_Receive_Message ";
                sql = sql + " where Receiver_Id = '" + userid + "'  ";

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

        public DataSet GetReceiveList(string userid, int pagesize, int pagecurrent)
        {
            int iFrom = (pagecurrent - 1) * pagesize + 1;
            int iTo = pagecurrent * pagesize; 
           
            try
            {
                string sql = " ";
                sql = sql + "with cte as(";
                sql = sql + "select id0=ROW_NUMBER()over(order by m.Send_Time desc), m.* ,Z_User.ID,Z_User.Name";
                sql = sql + " from T_Receive_Message m";
                sql = sql + " left JOIN Z_User";
                sql = sql + " on Z_User.ID=m.Send_Id";
                sql = sql + " where  m.Receiver_Id = '" + userid + "'"; 
                sql = sql + "     )";
                sql = sql +  "SELECT * FROM cte";
                sql = sql + " where  id0 between " + iFrom + " and " + iTo + " ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}