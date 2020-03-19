using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;

namespace ResSharingPlatform.Controllers
{
    public class RoleController : Controller
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
        /// 获取页面角色Json数据
        /// </summary>
        /// <param name="title">角色名称</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult GetList(string title)
        {
            
            RoleOperate op = new RoleOperate();
            string json = op.Select_Role_Page(title.Trim());

            return Content(json);
        }

        /// <summary>
        /// 获取页面角色和人员数量Json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult get_role_user()
        {
            
            RoleOperate op = new RoleOperate();
            string json = op.get_role_user();

            return Content(json);
        }

         /// <summary>
        /// 获取页面角色和人员数量Json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult get_role_user_list(string id)
        {
            
            RoleOperate op = new RoleOperate();
            string json = op.get_role_user_list(id);

            return Content(json);
        }

        /// <summary>
        /// 检索用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult search_user(string username)
        {
            
            RoleOperate op = new RoleOperate();
            string json = op.search_user(username);

            return Content(json);
        }
        
        

        

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult SaveDelete(string id)
        {
            RoleOperate op = new RoleOperate();

            if (op.Delete_Role(id) == true)
            {
                return Content("[{\"Save\":\"true\"}]");
            }

            return Content("[{\"Save\":\"false\"}]");
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
        public ActionResult Add()
        {
            return View();
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
        public ContentResult SaveAdd(string title, string remark, string menus)
        {
            RoleOperate op = new RoleOperate();

            string id = "";
            if (op.Insert_Role(ref id, title, remark) == true)
            {
                if (op.Insert_Menus(id, menus) == true)
                {
                    return Content("[{\"Save\":\"true\"}]");
                }
            }

            return Content("[{\"Save\":\"false\"}]");
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
