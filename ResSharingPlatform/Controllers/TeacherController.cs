using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using ResSharingPlatform.Common;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using NPOI.SS.UserModel;
using ResSharingPlatform.Lib;
using System.Data.SqlClient;

namespace ResSharingPlatform.Controllers
{
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string userid)
        {

            ViewData["userid"] = userid;

            string sql = "select * from  Z_User where ID='" + userid + "'";

            DBOperation db = new DBOperation();
            DataSet ds = new DataSet();
            ds = db.GetDataSet(sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewData["Name"] = ds.Tables[0].Rows[0]["Name"];
                ViewData["Password"] = ds.Tables[0].Rows[0]["Password"];
                ViewData["PageRole_ID"] = ds.Tables[0].Rows[0]["PageRole_ID"];
                ViewData["SchoolId"] = ds.Tables[0].Rows[0]["SchoolId"];
                ViewData["DataRole"] = ds.Tables[0].Rows[0]["DataRole"];
            }

            return View();
        }

        /// <summary>
        /// 更改用户信息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ContentResult EditSave(FormCollection form)
        {
            try
            {

                DateTime CreateTime = DateTime.Now;
                string password = "";

                if (form["Password"].ToString() != "")
                {
                    byte[] result = Encoding.Default.GetBytes(form["Password"]);    //Password为输入密码的文本
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] output = md5.ComputeHash(result);
                    password = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的
                }
                string sql = "update Z_User set Name='" + form["Name"] + "'";

                if (password != "")
                {
                    sql = sql + " ,Password='" + password + "'";
                }

                sql = sql + " ,PageRole_ID='" + form["roleId"] + "'";
                sql = sql + " ,SchoolId='" + form["schoolId"] + "'";
                sql = sql + " ,EditDate='" + CreateTime + "'";
                sql = sql + " ,DataRole='" + form["DataRole"] + "'";
                sql = sql + " where ID='" + form["userid"] + "'";
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }


        /// <summary>
        /// 获取学校Json数据
        /// </summary>
        /// <param name="title">角色名称</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult getSchoolList()
        {

            Teacher op = new Teacher();
            string json = op.GetSchoolList();
            return Content(json);
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
                string RecordId = Guid.NewGuid().ToString();
                DateTime CreateTime = DateTime.Now;

                byte[] result = Encoding.Default.GetBytes(form["Password"]);    //Password为输入密码的文本
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string password = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的

                string sql = "insert into Z_User (ID,Type,Name,Login_Name,Password,PageRole_ID,SchoolId,EditDate,DataRole) values ('"
                           + form["Login_Name"] + "','1','" + form["Name"] + "','" + form["Login_Name"] + "','" + password + "','" + form["roleId"] + "','" + form["schoolId"] + "','" + CreateTime + "','" + form["DataRole"] + "')";
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }

        /// <summary>
        /// 检测重复用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ContentResult checkUser(string username)
        {
            if (isExit(username) == true)
            {
                return Content("{\"result\":true}");
            }
            else
            {
                return Content("{\"result\":false}");
            }

        }

        public bool isExit(string username)
        {
            DataSet ds = new DataSet();
            string sql = "select * from Z_User  where Login_Name='" + username + "'";
            DBOperation db = new DBOperation();
            ds = db.GetDataSet(sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 导入xls源数据
        /// </summary>
        /// <returns></returns>
        public ContentResult addXls(string roleId, string schoolId, string filepath)
        {
            try
            {

                byte[] result = Encoding.Default.GetBytes("000000");    //"000000"为输入密码的文本
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string password = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的
                string uid = "";
                string uname = "";

                DateTime CreateTime = DateTime.Now;
                DataTable dt = new DataTable();
                dt = NPOIHelper.ImportExceltoDt(filepath, 0, 0);

                if (dt != null)
                {
                    DBOperation db = new DBOperation();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        uid = dt.Rows[i][0].ToString();
                        uname = dt.Rows[i][1].ToString();

                        if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(uname))
                        {
                            if (!isExit(uid) == true)
                            {
                                string sql = "insert into Z_User (ID,Type,Name,Login_Name,Password,PageRole_ID,SchoolId,EditDate,DataRole) values ('"
                                + uid + "','1','" + uname + "','" + uid + "','" + password + "','" + roleId + "','" + schoolId + "','" + CreateTime + "','1')";
                                db.ExecuteSql(sql);
                            }
                        }
                    }
                }

                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }

        public ActionResult List()
        {
            return View();
        }
        public ActionResult TeacherList()
        {
            return View();
        }
        [HttpPost]
        public string GetTeacherList(string TeacherName, string schoolId, string pagecurrent)
        {
            string TeacherList = "";
            Teacher teacher = new Teacher();

            string linkpage = "";// 分页标签

            // 初期化时当前页是否为空、若为空默认第一页
            if (string.IsNullOrEmpty(pagecurrent))
            {
                pagecurrent = "1";
            }

            // 取得记录集数
            int listcount = teacher.GetTeacherListCount(TeacherName, schoolId, "1");

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
            Ret = teacher.GetTeacherList(TeacherName, schoolId, page.subeachnums, int.Parse(pagecurrent), "1");
             
            TeacherList = "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) 
                                              + ",\"linkpage\":\"" + JsonHelper.String2Json(linkpage) + "\"}"; 
           

            return TeacherList;
        }
        [HttpPost]
        public ContentResult SaveDelete(string id)
        {
            try
            {
                string sql = "delete from Z_User where ID='" + id + "'";
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
        /// 激活CMS账户
        /// </summary>
        /// <param name="iscmsadmin"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddCmsList(string[] isactivate)
        {
            try
            {
                string userid = ""; 
                DBOperation db = new DBOperation();
                Dictionary<string, object> SQLStringList = new Dictionary<string, object>();

                SqlParameter[] paraimgdel = new SqlParameter[1];
                paraimgdel[0] = new SqlParameter("@ID", userid); 
                string updatesql = "";

                if (isactivate != null)
                {
                    for (int i = 0; i < isactivate.Length; i++)
                    {

                        userid = isactivate[i].ToString();
                        updatesql += " update Z_User set  IsCmsAdmin='1' where ID='" + userid + "';";

                    }
                }
                SQLStringList.Add(updatesql, paraimgdel);

                db.ExecuteSqlTran_sort(SQLStringList);
                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }
        
        /// <summary>
        /// 激活监控账户
        /// </summary>
        /// <param name="iscmsadmin"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddMonitorList(string[] isactivate)
        {
            try
            {
                string userid = ""; 
                DBOperation db = new DBOperation();
                Dictionary<string, object> SQLStringList = new Dictionary<string, object>();

                SqlParameter[] paraimgdel = new SqlParameter[1];
                paraimgdel[0] = new SqlParameter("@ID", userid); 
                string updatesql = "";

                if (isactivate != null)
                {
                    for (int i = 0; i < isactivate.Length; i++)
                    {

                        userid = isactivate[i].ToString();
                        updatesql += " update Z_User set  IsMonitorAdmin='1' where ID='" + userid + "';";

                    }
                }
                SQLStringList.Add(updatesql, paraimgdel);

                db.ExecuteSqlTran_sort(SQLStringList);
                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }
        #region 教师资源与文章

        public ActionResult ArticleAndResource()
        {
            return View();
        }

        #endregion

        #region 用户资源

        /// <summary>
        /// 用户资源
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceList(string userid)
        {
            ViewData["userid"] = userid;
            return View();
        } 

        /// <summary>
        /// 教师资源
        /// </summary>
        /// <param name="userid">教师ID</param>
        /// <param name="resource">资源名称</param>
        /// <param name="pagecurrent">当前被选中的页</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <returns></returns>
        [HttpPost]
        public string SearchList(string userid, string resource, string pagecurrent, string type, string uploadTime, string label)
        {
            string ResourceList = "";
            Teacher tr = new Teacher();

            string linkpage = "";// 分页标签

            // 初期化时当前页是否为空、若为空默认第一页
            if (string.IsNullOrEmpty(pagecurrent))
            {
                pagecurrent = "1";
            }

            // 取得记录集数
            int listcount = tr.GetTeacherResourceListCount(userid,resource, type, uploadTime, label);

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
            DataSet ds = new DataSet();
            ds = tr.GetTeacherResourceList(userid,resource, type, uploadTime, label, page.subeachnums, int.Parse(pagecurrent));
            ResourceList = "{\"result\":\"success\",\"list\":" + JsonHelper.ToJson(ds)
                                              + ",\"linkpage\":\"" + JsonHelper.String2Json(linkpage) + "\"}";

            return ResourceList;
        }

        #endregion

        #region 用户文章

        /// <summary>
        /// 用户来自CMS的文章
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleList(string userid)
        {
            ViewData["userid"] = userid;
            return View();
        }

        /// <summary>
        /// 教师资源
        /// </summary>
        /// <param name="userid">教师ID</param>
        /// <param name="resource">资源名称</param>
        /// <param name="pagecurrent">当前被选中的页</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <returns></returns>
        [HttpPost]
        public string SearchArticle(string userid, string pagecurrent)
        {
            string ArticleList = "";
            Teacher tr = new Teacher();

            string linkpage = "";// 分页标签

            // 初期化时当前页是否为空、若为空默认第一页
            if (string.IsNullOrEmpty(pagecurrent))
            {
                pagecurrent = "1";
            }

            // 取得记录集数
            int listcount = tr.GetArticleListCount(userid);

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
            Ret = tr.GetArticleList(userid, page.subeachnums, int.Parse(pagecurrent));
            ArticleList = "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret)
                                              + ",\"linkpage\":\"" + JsonHelper.String2Json(linkpage) + "\"}";

            return ArticleList;
        }

        #endregion

        [HttpPost]
        public string GetTeacherPoint(string userid)
        {
            string ResourcePoint = "";
            string BbsPoint = "0";
            Teacher tr = new Teacher();

            ResourcePoint = tr.GetResourcePoint(userid);
            BbsPoint = tr.GetBbsPoint(userid);


            return "{\"ResourcePoint\":" + ResourcePoint + ",\"BbsPoint\":" + BbsPoint + "}";
        }
 
    }
}
