using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using ResSharingPlatform.Lib;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Controllers
{
    public class InterfaceController : Controller
    {
        //
        // GET: /Interface/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        } 
        
        [HttpPost]
        public string GetInterfaceList(string pagecurrent)
        {
            string interfaceList = "";
            Interface m = new Interface();

            string linkpage = "";// 分页标签

            // 初期化时当前页是否为空、若为空默认第一页
            if (string.IsNullOrEmpty(pagecurrent))
            {
                pagecurrent = "1";
            }

            // 取得记录集数
            int listcount = m.GetInterfaceListCount();

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
            Ret = m.GetInterfaceList(page.subeachnums, int.Parse(pagecurrent));

            interfaceList = "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) 
                                              + ",\"linkpage\":\"" + JsonHelper.String2Json(linkpage) + "\"}";


            return interfaceList;
        }

        [HttpPost]
        public ContentResult SaveDelete(string id)
        {
            try
            {
                string sql = "delete from T_Interface where Interface_ID='" + id + "'";
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return Content("[{\"Save\":\"true\"}]");
            }
            catch (Exception ex)
            {
                return Content("[{\"Save\":\"false\"}]");
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="title">角色名称</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ContentResult Save(FormCollection form)
        {
            try
            {
                DateTime CreateDate = DateTime.Now;
                string sql = "";
                if (form["id"] == "")
                {
                    string Interface_ID = Guid.NewGuid().ToString();
                    sql = "insert into T_Interface (Interface_ID,Name,Url,CreateDate) values ('"
                               + Interface_ID + "','" + form["Name"] + "','" + form["Url"] + "','" + CreateDate + "')";
                }
                else
                {
                  
                    sql = "update T_Interface set Name='" + form["Name"] + "'";
                    sql = sql + " ,Url='" + form["Url"] + "'";
                    sql = sql + " where Interface_ID='" + form["id"] + "'";
                   
                }
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }


        public ActionResult Add(string id)
        {
            ViewData["id"] = id;
            if (id != "")
            {
                string sql = "select * from  T_Interface where Interface_ID='" + id + "'";
                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewData["Name"] = ds.Tables[0].Rows[0]["Name"];
                    ViewData["Url"] = ds.Tables[0].Rows[0]["Url"];
                }
            }
            return View();
        } 

    }
}
