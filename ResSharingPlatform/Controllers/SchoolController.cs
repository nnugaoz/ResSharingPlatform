using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using ResSharingPlatform.Common;
using System.Data;
using ResSharingPlatform.Lib;
using System.Text;

namespace ResSharingPlatform.Controllers
{
    public class SchoolController : Controller
    {
        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SchoolList(string SchoolName, string pagecurrent)
        {
            return View();
        }

        /// <summary>
        /// 获取页面角色Json数据
        /// </summary>
        /// <param name="title">角色名称</param>
        /// <returns></returns>
        [HttpPost]
        public string GetSchoolList(string SchoolName, string pagecurrent)
        {
            string SchoolList = "";
            School school = new School();

            string linkpage = "";// 分页标签

            // 初期化时当前页是否为空、若为空默认第一页
            if (string.IsNullOrEmpty(pagecurrent))
            {
                pagecurrent = "1";
            }

            // 取得记录集数
            int listcount = school.GetSchoolListCount(SchoolName);

            // 分页
            MyPages page = new MyPages();
            page.subeachnums = Constant.BACKGROUND_RESOURCE_NUM;// 每页显示的条目数
            page.subnums = listcount;// 总条目数
            page.subcurrentpage = int.Parse(pagecurrent);// 当前被选中的页  
            page.subeachpages = 10;// 每次显示的页数
            page.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
            page.subformname = "ActionForm";// 表单名
            linkpage = page.SubPages();// 生成分页标签

            // 删除时的设置 start 
            // 删除操作时如果当前页大于总页数，当前页设为最后一页
            if (int.Parse(pagecurrent) > page.subpagenums && page.subpagenums != 0)
            {
                pagecurrent = page.subpagenums.ToString();
            }
           
            
            List<object> Ret = new List<object>();
            Ret = school.GetSchoolList(SchoolName, page.subeachnums, int.Parse(pagecurrent));
            SchoolList = "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret)
                                              + ",\"linkpage\":\"" + JsonHelper.String2Json(linkpage) + "\"}"; 
       
            return SchoolList;
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult SaveDelete(string id)
        {
            try
            {
                string sql = "delete from Z_School where RecordId='" + id + "'";
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return Content("[{\"Save\":\"true\"}]");
            }
            catch (Exception ex)
            {
                return Content("[{\"Save\":\"false\"}]");
            }
        }
        #endregion

        #region 详细记录
        /// <summary>
        /// 详细记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// 获取页面角色详细Json数据
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpGet]
        public ContentResult GetDetail(string id)
        {
            RoleOperate op = new RoleOperate();
            string json = "";
            string ret1 = op.Select_Role_One(id);
            string ret2 = op.Select_Menus(id);

            json += "[{\"HaveData\":";
            if (ret1 == "")
            {
                json += "\"false\"},{\"Table\":\"\"}";
            }
            else
            {
                json += "\"true\"}," + ret1;
            }
            json += ",{\"HaveData\":";
            if (ret2 == "")
            {
                json += "\"false\"},{\"Table\":\"\"}";
            }
            else
            {
                json += "\"true\"}," + ret2;
            }
            json += "]";

            return Content(json);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult AddSchool(string SchoolId)
        {
            ViewData["SchoolId"] = SchoolId;
            return View();
        }

        [HttpPost]
        public ContentResult GetSchoolInfo(string SchoolId)
        {
            string json = "{\"result\":\"Nodata\"}";
            if (!string.IsNullOrEmpty(SchoolId))
            {
                School school = new School();
                List<object> Ret = new List<object>();
                json = school.GetSchoolBySchoolId(SchoolId);
            }
            return Content(json);
        }

        /// <summary>
        /// 获取页面角色详细Json数据
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpGet]
        public ContentResult GetAdd()
        {
            RoleOperate op = new RoleOperate();
            string json = "";
            string ret1 = op.Select_Menus("");

            json += "[{\"HaveData\":";
            if (ret1 == "")
            {
                json += "\"false\"},{\"Table\":\"\"}";
            }
            else
            {
                json += "\"true\"}," + ret1;
            }
            json += "]";

            return Content(json);
        }

        /// <summary>
        /// 添加页面提交操作
        /// </summary>
        /// <param name="title">角色名称</param>
        /// <param name="remark">角色描述</param>
        /// <param name="menus">菜单code串</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult DoAddSchool(string SchoolName)
        {
            try
            {
                string RecordId = Guid.NewGuid().ToString();
                DateTime CreateTime = DateTime.Now;

                string sql = "insert into Z_School (RecordId,SchoolName,CreateTime) values ('"
                           + RecordId + "','" + SchoolName + "','" + CreateTime + "')";
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }
        [HttpPost]
        public ContentResult DoEditSchool(string RecordId,string SchoolName)
        {
            try
            {
                DBOperation db = new DBOperation();
                string sql = "";

                sql = "update Z_School set SchoolName = '" + SchoolName + "' where RecordId = '" + RecordId + "'";
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }


        #endregion

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
        [HttpGet]
        public ContentResult GetEdit(string id)
        {
            RoleOperate op = new RoleOperate();
            string json = "";
            string ret1 = op.Select_Role_One(id);
            string ret2 = op.Select_Menus(id);

            json += "[{\"HaveData\":";
            if (ret1 == "")
            {
                json += "\"false\"},{\"Table\":\"\"}";
            }
            else
            {
                json += "\"true\"}," + ret1;
            }
            json += ",{\"HaveData\":";
            if (ret2 == "")
            {
                json += "\"false\"},{\"Table\":\"\"}";
            }
            else
            {
                json += "\"true\"}," + ret2;
            }
            json += "]";

            return Content(json);
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
        public ContentResult SaveEdit(string id, string title, string remark, string menus)
        {
            RoleOperate op = new RoleOperate();

            if (op.Update_Role(id, title, remark) == true)
            {
                if (op.Update_Menus(id, menus) == true)
                {
                    return Content("[{\"Save\":\"true\"}]");
                }
            }

            return Content("[{\"Save\":\"false\"}]");
        }
        #endregion

    }
}
