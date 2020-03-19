using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Controllers
{
    public class PointRuleController : Controller
    {
        #region 编辑
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 获取页面角色详细Json数据
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult GetRule(string id)
        {
            List<object> Ret = new List<object>();
            try
            {
                string sql = "SELECT top 1 * FROM Z_PointRule ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                //保存数据表并返回
                Ret.Add(ds);

                string json = "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) + "}";
                return Content(json);
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return Content("");
            }
        }

        /// <summary>
        /// 编辑页面提交操作
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="title">角色名称</param>
        /// <param name="remark">角色描述</param>
        /// <param name="menus">菜单code串</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult EditRule(string RecordId, string PointRuleName, int Point)
        {
            try
            {
                DBOperation db = new DBOperation();
                string sql = "";

                sql = "update Z_PointRule set PointRuleName = '" + PointRuleName + "', Point = " + Point + " where RecordId = '" + RecordId + "'";
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }
        [HttpPost]
        public ContentResult AddRule(string PointRuleName, int Point)
        {
            try
            {
                string RecordId = Guid.NewGuid().ToString();
                DateTime CreateTime = DateTime.Now;

                string sql = "insert into Z_PointRule (RecordId,PointRuleName,Point,CreateTime) values ('"
                           + RecordId + "','" + PointRuleName + "'," + Point + ",'" + CreateTime + "')";
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }
        #endregion


        public ContentResult AddPoint()
        {
            try
            {
                string sql = "SELECT top 1 * FROM Z_PointRule ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                int point = 0;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    point = int.Parse(ds.Tables[0].Rows[0]["Point"].ToString());
                }



                sql = "insert into Z_UserPoint (RecordId, UserId, ResourceId, Type, Point, CreateTime) values ('"
                           + Guid.NewGuid().ToString() + "','" + "kkk" + "'," + "ssss" + ",'" + "dddd" + "'," + point + ",'" + DateTime.Now + "')";
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }

    }
}
