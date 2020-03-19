using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Models
{
    public class PointRule
    {
        /// <summary>
        /// //上传资源时，可以获得积分
        /// </summary>
        /// <param name="ResourceId">资源ID</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="strType">0：上传 1：下载</param>
        /// <returns></returns>
        public string AddPoint(string ResourceId, string UserId, string strType)
        {
            try
            {
                //取得上传资源可以获得的积分数
                string sql = "SELECT top 1 * FROM Z_PointRule ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                int point = 0;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    point = int.Parse(ds.Tables[0].Rows[0]["Point"].ToString());
                }

                //插入用户积分表
                sql = "insert into Z_UserPoint (RecordId, UserId, ResourceId, Type, Point, CreateTime) values ('"
                           + Guid.NewGuid().ToString() + "','" + UserId + "','" + ResourceId + "','" + strType + "'," + point + ",'" + DateTime.Now + "')";
                db.ExecuteSql(sql);

                return "{\"result\":\"success\"}";
            }
            catch (Exception ex)
            {
                return "{\"result\":\"error\"}";
            }
        }
    }
}