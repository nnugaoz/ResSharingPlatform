using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using ResSharingPlatform.Lib;
using ResSharingPlatform.Common;
using System.Data;
using System.Data.SqlClient;

namespace ResSharingPlatform.Controllers
{
    public class MessageController : Controller
    {
        //
        // GET: /Message/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendList()
        {
            return View();
        }

        public ActionResult ReceiveList()
        {
            return View();
        }

        public ActionResult SendBox(string groupid,string userid,string username)
        {
            ViewData["groupid"] = groupid;
            ViewData["userid"] = userid;
            ViewData["username"] = username;
            return View();
        }

        public ActionResult selectUser()
        {
            return View();
        }


        /// <summary>
        /// 读取公文
        /// </summary>
        /// <returns></returns>
        public ActionResult readMsg(string rid,string mid,string from)
        {
            DBOperation db = new DBOperation();
            string sql = "";
            string type="";
            if (from == "send")
            {
                sql = "select u.ID,u.Name,u.PageRole_ID,t.Msg_Title,t.Msg_Content,t.Type from T_Message t";
                sql += " left join Z_User u";
                sql += " on u.ID=t.Send_Id";
                sql += " where t.Msg_Id='" + mid + "'";
                
            }
            else
            {
                sql = "select u.ID,u.Name,u.PageRole_ID,t.Msg_Title,t.Msg_Content,t.Type,t.Record_Id from T_Receive_Message t";
                sql += " LEFT JOIN Z_User u";
                sql += " on u.ID=t.Send_Id";
                sql += " where t.Record_Id='" + rid + "'"; 
            }
            ViewData["from"] = from;
            DataSet data = new DataSet();
            data = db.GetDataSet(sql);
            if (data.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = data.Tables[0];
                if (from != "send")
                {
                    ViewData["Record_Id"] = dt.Rows[0]["Record_Id"].ToString();
                }
                ViewData["fromuserid"] = dt.Rows[0]["ID"].ToString();
                ViewData["fromusername"] = dt.Rows[0]["Name"].ToString();
                ViewData["roleid"] = dt.Rows[0]["PageRole_ID"].ToString();
                ViewData["Msg_Title"] = dt.Rows[0]["Msg_Title"].ToString();
                ViewData["Msg_Content"] = dt.Rows[0]["Msg_Content"].ToString();
                ViewData["type"] = dt.Rows[0]["Type"].ToString();
                type = dt.Rows[0]["Type"].ToString();
            }
            ViewData["mid"] = mid;

            //一般公文和发件箱阅读页面
            if (type == "0" && from != "send")
            {
                DateTime Receiver_Time = DateTime.Now;
                string updatesql = "update T_Receive_Message set Msg_Status='1' , Receiver_Time='" + Receiver_Time + "'";
                updatesql += " where Record_Id='" + rid + "'";
                db.ExecuteSql(updatesql);
            }
           
            return View();
        }

        

        /// <summary>
        /// 获取发件箱Json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSendList(string pagecurrent)
        {
            string List = "";
            Message m = new Message();
            string userid = CommonUtil.GetSession(Session, "id");
            string linkpage = "";// 分页标签

            // 初期化时当前页是否为空、若为空默认第一页
            if (string.IsNullOrEmpty(pagecurrent))
            {
                pagecurrent = "1";
            }

            // 取得记录集数
            int listcount = m.GetSendListCount(userid);

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


            DataSet Ret = new DataSet();
            Ret = m.GetSendList(userid,page.subeachnums, int.Parse(pagecurrent));
            List = "{\"result\":\"success\",\"list\":" + JsonHelper.ToJson(Ret)
                                              + ",\"linkpage\":\"" + JsonHelper.String2Json(linkpage) + "\"}";

            return List;
        }

        

        /// <summary>
        /// 获取收件箱Json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetReceiveList(string pagecurrent)
        {
            string List = "";
            Message m = new Message();
            string userid = CommonUtil.GetSession(Session, "id");
            string linkpage = "";// 分页标签

            // 初期化时当前页是否为空、若为空默认第一页
            if (string.IsNullOrEmpty(pagecurrent))
            {
                pagecurrent = "1";
            }

            // 取得记录集数
            int listcount = m.GetReceiveListCount(userid);

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


            DataSet Ret = new DataSet();
            Ret = m.GetReceiveList(userid, page.subeachnums, int.Parse(pagecurrent));
            List = "{\"result\":\"success\",\"list\":" + JsonHelper.ToJson(Ret)
                                              + ",\"linkpage\":\"" + JsonHelper.String2Json(linkpage) + "\"}";

            return List;
        }

        /// <summary>
        /// 发件
        /// </summary>
        /// <param name="form"></param>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ContentResult SaveSendMsg(FormCollection form, string[] tousergroup)
        {
            try
            {
                string Msg_Id = Guid.NewGuid().ToString();
                string Msg_Title = form["Msg_Title"].ToString();
                string Msg_Content = form["Msg_Content"].ToString();
                string Send_Id = CommonUtil.GetSession(Session, "id");
                string Type = form["Type"].ToString();
                DateTime Send_Time = DateTime.Now;

                DBOperation db = new DBOperation();
                Dictionary<string, object> SQLStringList = new Dictionary<string, object>();

                SqlParameter[] paraimgdel = new SqlParameter[1];
                paraimgdel[0] = new SqlParameter("@Msg_Id", Msg_Id);

                string sql = "insert into T_Message (Msg_Id,Msg_Title,Msg_Content,Send_Id,Send_Time,Type) values ('"
                          + Msg_Id + "','" + Msg_Title + "','" + Msg_Content + "','" + Send_Id + "','" + Send_Time + "','" + Type + "')";
                SQLStringList.Add(sql, paraimgdel);
                string sqlselect ="";
                for (int i = 0; i < tousergroup.Length; i++)
                {
                    string Receiver_Id = tousergroup[i]; 
                    string Msg_Status = "0";
                    string Record_Id = Guid.NewGuid().ToString();
                    if(sqlselect!="")
                    {
                        sqlselect = sqlselect + "  UNION ALL  ";  
                    }
                     sqlselect += " select '"
                          + Msg_Id + "','" + Receiver_Id + "','" + Msg_Status + "','" + Record_Id + "','" + Msg_Title + "','" + Msg_Content + "','" + Type + "','" + Send_Id + "','" + Send_Time + "'";
                }
                string sqlInsert = "insert into T_Receive_Message (Msg_Id,Receiver_Id,Msg_Status,Record_Id,Msg_Title,Msg_Content,Type,Send_Id,Send_Time)  ";
                      sqlInsert+=sqlselect  ;
                SQLStringList.Add(sqlInsert, paraimgdel);
                db.ExecuteSqlTran_sort(SQLStringList);
                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }

        /// <summary>
        /// 获取以签收用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string get_read_user_list(string mid)
        {

            string sql = "select u.ID,u.Name,m.Receiver_Time from T_Receive_Message m";
                    sql +=" LEFT JOIN Z_User u";
                    sql += " on u.ID=m.Receiver_Id";
                    sql +=" where m.Msg_Status='1' ";
                    sql += " and m.Msg_Id='" + mid + "' "; 

            DBOperation db = new DBOperation();
            DataSet ds = new DataSet();
            ds = db.GetDataSet(sql);

            return "{\"result\":\"success\",\"list\":" + JsonHelper.ToJson(ds) + "}"; 
        }

        /// <summary>
        /// 获取未签收用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string get_unread_user_list(string mid)
        {

            string sql = "select u.ID,u.Name from T_Receive_Message m";
            sql += " LEFT JOIN Z_User u";
            sql += " on u.ID=m.Receiver_Id";
            sql += " where m.Msg_Status='0' ";
            sql += " and m.Msg_Id='" + mid + "' "; 

            DBOperation db = new DBOperation();
            DataSet ds = new DataSet();
            ds = db.GetDataSet(sql);

            return "{\"result\":\"success\",\"list\":" + JsonHelper.ToJson(ds) + "}";
        }

        /// <summary>
        /// 获取自定义签收用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string get_table_list(string mid)
        {
            //string sql = "select * from T_Custom_Msg m"; 
            //sql += " where m.Msg_Id='"+mid+"' ";
            string sql = " select m.*, u.Name from T_Custom_Msg m left join Z_User u on u.Login_Name=m.Receiver_Id";
            sql += " where m.Msg_Id='" + mid + "' ";

            DBOperation db = new DBOperation();
            DataSet ds = new DataSet();
            ds = db.GetDataSet(sql);

            return "{\"result\":\"success\",\"list\":" + JsonHelper.ToJson(ds) + "}";
        }
         
        [HttpPost]
        [ValidateInput(false)]
        public ContentResult savetable(FormCollection form)
        {
            try
            {
                string Msg_Id = form["Msg_Id"].ToString(); 
                string Receiver_Id = CommonUtil.GetSession(Session, "id");
                string Table_Title = form["Table_Title"].ToString();
                string Table_Project = form["Table_Project"].ToString();
                string Table_Id = Guid.NewGuid().ToString();
                string Msg_Status = "1";//签收
                DateTime Receiver_Time = DateTime.Now;

                DBOperation db = new DBOperation();
                Dictionary<string, object> SQLStringList = new Dictionary<string, object>();

                SqlParameter[] paraimgdel = new SqlParameter[1];
                paraimgdel[0] = new SqlParameter("@Msg_Id", Msg_Id);

                string sql = "insert into T_Custom_Msg (Table_Id,Msg_Id,Receiver_Id,Table_Title,Table_Project) values ('"
                          + Table_Id + "','" + Msg_Id + "','" + Receiver_Id + "','" + Table_Title + "','" + Table_Project+"')";
                SQLStringList.Add(sql, paraimgdel); 

                string sqlInsert = "update T_Receive_Message set Msg_Status='1' , Receiver_Time='" + Receiver_Time.ToString() + "'";
                sqlInsert += " where Record_Id='" + form["Record_Id"].ToString() + "'";
                    
                SQLStringList.Add(sqlInsert, paraimgdel);
                db.ExecuteSqlTran_sort(SQLStringList);
                return Content("{\"result\":\"success\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\"}");
            }
        }

        

         /// <summary>
        /// 删除发件箱
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost] 
        public ContentResult DeleteSendMsg(string  mid)
        {
            try
            {
                string sql = "delete from T_Message where Msg_Id='" + mid + "'";
 
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
        /// 删除发件箱
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult DeleteReceiveMsg(string rid)
        {
            try
            {
                string sql = "delete from T_Receive_Message where Record_Id='" + rid + "'";

                DBOperation db = new DBOperation();
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
