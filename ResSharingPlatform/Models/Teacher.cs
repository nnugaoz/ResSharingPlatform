using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Models
{
    public class Teacher
    {
        /// <summary>
        /// 获取学校数据
        /// </summary>
        /// <returns></returns>
        public string GetSchoolList()
        {
            List<object> Ret = new List<object>();
            try
            {
                string sql = "SELECT row_number() over (order by CreateTime DESC) as rowid, * FROM Z_School ";

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

        public int GetTeacherListCount(string TeacherName, string schoolId, string Type)
        {
            if (string.IsNullOrEmpty(TeacherName)) TeacherName = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + "with cte as( ";
                sql = sql + "             select id0=ROW_NUMBER()over(order by EditDate desc),*  ";
                sql = sql + "               from Z_User ";
                sql = sql + "              left join Z_School s on s.RecordId = Z_User.SchoolId ";
                sql = sql + "              where Z_User.Name like '%" + TeacherName + "%' and Z_User.Type = '" + Type + "' ";
                if (schoolId != "")
                {
                    sql = sql + "   and Z_User.SchoolId = '" + schoolId + "'";
                }
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

        public List<object> GetTeacherList(string TeacherName, string schoolId, int pagesize, int pagecurrent, string Type)
        {
            int iFrom = (pagecurrent - 1) * pagesize + 1;
            int iTo = pagecurrent * pagesize;
            if (string.IsNullOrEmpty(TeacherName)) TeacherName = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + "with cte as( ";
                sql = sql + "             select id0=ROW_NUMBER()over(order by EditDate desc),*  ";
                sql = sql + "               from Z_User ";
                sql = sql + "              left join Z_School s on s.RecordId = Z_User.SchoolId ";
                sql = sql + "              where Z_User.Name like '%" + TeacherName + "%' and Z_User.Type = '" + Type + "' ";
                if (schoolId != "")
                {
                    sql = sql + "   and Z_User.SchoolId = '" + schoolId + "'";
                }
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


        /// <summary>
        /// 取得教师的资源总数
        /// </summary>
        /// <param name="userid">教师ID</param>
        /// <param name="resource">资源名</param>
        /// <param name="type">资源类型</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <returns></returns>
        public int GetTeacherResourceListCount(string userid, string resource, string type, string uploadTime, string label)
        {
            if (string.IsNullOrEmpty(userid)) userid = "";
            if (string.IsNullOrEmpty(resource)) resource = "";
            if (string.IsNullOrEmpty(type)) type = "";
            if (string.IsNullOrEmpty(uploadTime)) uploadTime = "";
            if (string.IsNullOrEmpty(label)) label = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + "  with cte as(  ";
                sql = sql + "                select id0=ROW_NUMBER()over(order by T_Res_Appendix.CREATETIME desc),T_Res_Appendix.*   ";
                sql = sql + "                  from T_Res_Appendix ";
                sql = sql + "                 INNER JOIN T_Resource r  ";
                sql = sql + "                 ON r.ID = T_Res_Appendix.RES_ID ";
                sql = sql + "                 AND r.DEL_FLG = '0' ";
                sql = sql + "                 AND r.CREATEID = '" + userid + "' ";
                sql = sql + "                 AND r.LABEL LIKE '%" + label + "%' ";
                sql = sql + "                 AND r.TYPE_ID LIKE '%" + type + "%' ";
                sql = sql + "                 AND r.CREATETIME LIKE '%" + uploadTime + "%' ";
                sql = sql + "                 WHERE T_Res_Appendix.DEL_FLG = '0' ";
                sql = sql + "                 AND T_Res_Appendix.FILE_NAME LIKE '%" + resource + "%' ";
                sql = sql + "             )";
                sql = sql + "  select cte.*  ";
                sql = sql + "    from cte  ";




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

        /// <summary>
        /// 取得教师的资源
        /// </summary>
        /// <param name="userid">教师ID</param>
        /// <param name="resource">资源名</param>
        /// <param name="type">资源类型</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <param name="pagesize">每页显示的数目</param>
        /// <param name="pagecurrent">当前页</param>
        /// <returns></returns>
        public DataSet GetTeacherResourceList(string userid, string resource, string type, string uploadTime, string label, int pagesize, int pagecurrent)
        {
            int iFrom = (pagecurrent - 1) * pagesize + 1;
            int iTo = pagecurrent * pagesize;

            if (string.IsNullOrEmpty(userid)) userid = "";
            if (string.IsNullOrEmpty(resource)) resource = "";
            if (string.IsNullOrEmpty(type)) type = "";
            if (string.IsNullOrEmpty(uploadTime)) uploadTime = "";
            if (string.IsNullOrEmpty(label)) label = "";

        
            try
            {
                string sql = " ";

                sql = sql + "  with cte as(  ";
                sql = sql + "                select id0=ROW_NUMBER()over(order by T_Res_Appendix.CREATETIME desc)   ";

                sql = sql + "  ,T_Res_Appendix.ID,T_Res_Appendix.RES_ID,T_Res_Appendix.FILE_NAME,T_Res_Appendix.UPLOAD_TIME  ";
                sql = sql + "  ,T_Res_Appendix.ACTIVE_TIME_START, T_Res_Appendix.ACTIVE_TIME_END  ";
                sql = sql + "  ,T_Res_Appendix.NOTE,T_Res_Appendix.CREATEID,T_Res_Appendix.CREATETIME,T_Res_Appendix.MODIFYID  ";
                sql = sql + "  ,T_Res_Appendix.MODIFYTIME,T_Res_Appendix.DEL_FLG,T_Res_Appendix.TYPE_FLG,T_Res_Appendix.FILE_URL  ";
                sql = sql + "  ,T_Res_Appendix.READ_URL,T_Res_Appendix.PAGECOUNT,T_Res_Appendix.AUTHOR,T_Res_Appendix.IS_FOREVER";                  

                sql = sql + "                  from T_Res_Appendix ";
                sql = sql + "                 INNER JOIN T_Resource r  ";
                sql = sql + "                 ON r.ID = T_Res_Appendix.RES_ID ";
                sql = sql + "                 AND r.DEL_FLG = '0' ";
                sql = sql + "                 AND r.CREATEID = '" + userid + "' ";
                sql = sql + "                 AND r.LABEL LIKE '%" + label + "%' ";
                sql = sql + "                 AND r.TYPE_ID LIKE '%" + type + "%' ";
                sql = sql + "                 AND r.CREATETIME LIKE '%" + uploadTime + "%' ";
                sql = sql + "                 WHERE T_Res_Appendix.DEL_FLG = '0' ";
                sql = sql + "                 AND T_Res_Appendix.FILE_NAME LIKE '%" + resource + "%' ";
                sql = sql + "             )";
                sql = sql + "  select cte.*  ";
                sql = sql + "    from cte  ";
                sql = sql + "   where (cte.id0 BETWEEN " + iFrom + " AND " + iTo + ") ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                return ds;
                //return "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) + "}";
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public int GetArticleListCount(string userid)
        {
            //userid = "admin";//TODO
            if (string.IsNullOrEmpty(userid)) userid = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + " with cte as(  ";
                sql = sql + "              select id0=ROW_NUMBER()over(order by jc_content.sort_date desc) ";
                sql = sql + "                ,s.protocol,s.domain ,ch.channel_path,jc_content.content_id,s.dynamic_suffix,e.title,jc_content.sort_date, config.context_path  ";
                sql = sql + "                , case when config.port=null or config.port = '' or config.port = '80' then '' ";
                sql = sql + "                  else config.port ";
                sql = sql + "                  end as port ";
                sql = sql + "                from jc_content ";
                sql = sql + "                left join jc_config config on 1=1 ";
                sql = sql + "                LEFT JOIN jc_user u ";
                sql = sql + "                on u.user_id=jc_content.user_id ";
                sql = sql + "                LEFT JOIN jc_channel ch ";
                sql = sql + "                on ch.channel_id=jc_content.channel_id ";
                sql = sql + "                LEFT JOIN jc_site s ";
                sql = sql + "                ON s.site_id=jc_content.site_id ";
                sql = sql + "                LEFT JOIN jc_content_ext e ";
                sql = sql + "                ON e.content_id=jc_content.content_id ";
                sql = sql + "                where u.username='" + userid + "' ";
                sql = sql + "            )  ";
                sql = sql + "  select cte.*  ";
                sql = sql + "    from cte  ";




                //调用CMS的数据库
                CmsDbOperation db = new CmsDbOperation();
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

        public List<object> GetArticleList(string userid,int pagesize, int pagecurrent)
        {
            //userid = "admin";//TODO

            int iFrom = (pagecurrent - 1) * pagesize + 1;
            int iTo = pagecurrent * pagesize;
            if (string.IsNullOrEmpty(userid)) userid = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + " with cte as(  ";
                sql = sql + "              select id0=ROW_NUMBER()over(order by jc_content.sort_date desc) ";
                sql = sql + "                ,s.protocol,s.domain ,ch.channel_path,jc_content.content_id,s.dynamic_suffix,e.title,jc_content.sort_date, config.context_path  ";
                sql = sql + "                , case when config.port=null or config.port = '' or config.port = '80' then '' ";
                sql = sql + "                 else ':'+cast(config.port as varchar(10)) ";
                sql = sql + "                  end as port ";
                sql = sql + "                from jc_content ";
                sql = sql + "                left join jc_config config on 1=1 ";
                sql = sql + "                LEFT JOIN jc_user u ";
                sql = sql + "                on u.user_id=jc_content.user_id ";
                sql = sql + "                LEFT JOIN jc_channel ch ";
                sql = sql + "                on ch.channel_id=jc_content.channel_id ";
                sql = sql + "                LEFT JOIN jc_site s ";
                sql = sql + "                ON s.site_id=jc_content.site_id ";
                sql = sql + "                LEFT JOIN jc_content_ext e ";
                sql = sql + "                ON e.content_id=jc_content.content_id ";
                sql = sql + "                where u.username='" + userid + "' ";
                sql = sql + "            )  ";
                sql = sql + "  select cte.*  ";
                sql = sql + "    from cte  ";
                sql = sql + "   where (cte.id0 BETWEEN " + iFrom + " AND " + iTo + ") ";

                //调用CMS的数据库
                CmsDbOperation db = new CmsDbOperation();
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

        /// <summary>
        /// 取得用户资源平台积分
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public string GetResourcePoint(string userid)
        {
            if (string.IsNullOrEmpty(userid)) userid = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + " SELECT UserId, SUM(Point) AS ResourcePoint FROM Z_UserPoint ";
                sql = sql + " WHERE UserId = '" + userid + "' ";
                sql = sql + " GROUP BY UserId ";

                //调用CMS的数据库
                //CmsDbOperation db = new CmsDbOperation();
                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                //返回
                return ds.Tables[0].Rows[0]["ResourcePoint"].ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public string GetBbsPoint(string userid)
        {
            if (string.IsNullOrEmpty(userid)) userid = "";

            List<object> Ret = new List<object>();
            try
            {
                string sql = " ";

                sql = sql + " SELECT POINT as BbsPoint FROM jb_user ";
                sql = sql + " WHERE username = '" + userid + "' ";


                //调用CMS的数据库
                BbsDbOperation db = new BbsDbOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                //返回
                return ds.Tables[0].Rows[0]["BbsPoint"].ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
    }
}