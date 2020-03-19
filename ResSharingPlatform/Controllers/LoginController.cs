using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using ResSharingPlatform.Common;
using System.Data;
using System.Configuration;
using System.Text;

namespace ResSharingPlatform.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录获取上一页面地址或者系统类别
        /// </summary>
        /// <param name="from"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Login(string from,string type)
        {
            
            //获取上一页的网页地址
            Uri Url = HttpContext.Request.UrlReferrer;
            if (Url != null)
            {
                ViewData["url"] = Url.AbsoluteUri.ToString();
            }
            else
            {
                ViewData["url"] = "../../Admin/Admin";
            }
            if (!string.IsNullOrEmpty(from))
            {
                ViewData["url"] = from;
            }
 
             
            //string url = "";
            //string urlapi = ConfigurationManager.AppSettings["LoginUrl"];
            //string[] urls = urlapi.Split(new char[] { ';' });
            //string currentsys = "";
            //for (int i = 0; i < urls.Length - 1; i++)
            //{
            //    string[] temp = urls[i].ToString().Split(new char[] { '?' });
            //    string flag = "";

            //    //如果上个页面和接口页面标识一致，则设定当前这个页面地址为需要单点登录后才跳转的地址
            //    if (type == temp[1].Split(new char[] { '=' })[1])
            //    {
            //        flag = "true";
            //    }
            //    else//否则不需要
            //    {
            //        flag = "false";
            //    }

            //    if (i == urls.Length-2)
            //    {
            //        url += temp[0].ToString() + "," + flag;
            //    }
            //    else
            //    {
            //        url += temp[0].ToString() + "," + flag + ";";
            //    }
            //} 

            ////排除当前中心用户系统接口
            //for (int i = 0; i < urls.Length - 1; i++)
            //{
            //    string[] temp = urls[i].ToString().Split(new char[] { '?' });
            //    currentsys= temp[1].Split(new char[] { '=' })[1];

            //    //如果上个页面和接口页面标识一致，则设定当前这个页面地址为需要单点登录后才跳转的地址
            //    if (currentsys != "user")
            //    {
            //        if (i == urls.Length - 2)
            //        {
            //            url += temp[0].ToString();
            //        }
            //        else
            //        {
            //            url += temp[0].ToString()+ ";";
            //        }
            //    }
            //} 

            //ViewData["urls"] = url;
            //ViewData["type"] = type;
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="autoLogin"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult LoginOn(string account, string password, bool autoLogin)
        {
            try
            {

                HttpCookie myCookie = new HttpCookie("gm_userinfo", null);
                myCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(myCookie);
                Session.RemoveAll();

                Basice pLoginModels = new Basice();
                LoginReturn pLoginReturn;
                // 登录唯一凭证
                string tokenValue = Guid.NewGuid().ToString().ToUpper();
                #region /*获得IP*/
                string userIp = "";
                if (Request.ServerVariables["HTTP_VIA"] != null)
                {
                    userIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    userIp = Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                #endregion
                // 登录
                if (pLoginModels.Login(account, password, userIp, tokenValue,autoLogin, out pLoginReturn))
                {
                    //session 赋值
                    Session["id"] = pLoginReturn.ID;
                    Session["name"] = pLoginReturn.Name;
                    Session["rid"] = pLoginReturn.Role_ID;
                    Session["guid"] = tokenValue;
                    Session["Type"] = pLoginReturn.Type;
                    Session["DataRole"] = pLoginReturn.DataRole;
                    Session["Account"] = account;

                    //HttpCookie myCookie = new HttpCookie("gm_userinfo", null);
                    //myCookie.Expires = DateTime.Now.AddDays(-1d);
                    //Response.Cookies.Add(myCookie);
                    
                    HttpCookie cookies_ = new HttpCookie("gm_userinfo");
                    cookies_.Values.Add("id", pLoginReturn.ID);
                    cookies_.Values.Add("name", pLoginReturn.Name);
                    cookies_.Values.Add("rid", pLoginReturn.Role_ID);
                    cookies_.Values.Add("guid", tokenValue);
                    cookies_.Values.Add("Type", pLoginReturn.Type);
                    cookies_.Values.Add("DataRole", pLoginReturn.DataRole);
                    cookies_.Values.Add("Account", account);
                    //域
                    if (!autoLogin)
                    {
                        cookies_.Expires = DateTime.Now.AddDays(2);
                    }
                    else
                    {
                        cookies_.Expires = DateTime.Now.AddDays(7);
                    }
                    this.Response.Cookies.Add(cookies_);
                    this.Response.AppendCookie(cookies_);

                    string xinfo = Session["Type"].ToString() == "1" ? " 老师" : (Session["Type"].ToString() == "2" ? " 学生" : (Session["Type"].ToString() == "3" ? " 企业" : ""));
                    return Content("[{\"Login\":\"True\",\"Type\":\"" + pLoginReturn.Type + "\",\"STR\":\"" + Session["name"] + xinfo + "\"}]");
                }
                return Content("no");
            }
            catch (Exception ex) { string s = ex.Message; }
            return null;
        }

        /// <summary>
        /// 登录：大后台登录本中心用户系统用
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="autoLogin"></param>
        /// <returns></returns>
        [HttpPost]
        public string loginAPI(string userid, string password)
        {
            bool autoLogin = false;
            string account=userid;
            try
            {
                HttpCookie myCookie = new HttpCookie("gm_userinfo", null);
                myCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(myCookie);
                Session.RemoveAll();

                Basice pLoginModels = new Basice();
                LoginReturn pLoginReturn;
                // 登录唯一凭证
                string tokenValue = Guid.NewGuid().ToString().ToUpper();
                #region /*获得IP*/
                string userIp = "";
                if (Request.ServerVariables["HTTP_VIA"] != null)
                {
                    userIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    userIp = Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                #endregion
                // 登录
                if (pLoginModels.Login(account, password, userIp, tokenValue, autoLogin, out pLoginReturn))
                {
                    //session 赋值
                    Session["id"] = pLoginReturn.ID;
                    Session["name"] = pLoginReturn.Name;
                    Session["rid"] = pLoginReturn.Role_ID;
                    Session["guid"] = tokenValue;
                    Session["Type"] = pLoginReturn.Type;
                    Session["DataRole"] = pLoginReturn.DataRole;
                    Session["Account"] = account;

                    //HttpCookie myCookie = new HttpCookie("gm_userinfo", null);
                    //myCookie.Expires = DateTime.Now.AddDays(-1d);
                    //Response.Cookies.Add(myCookie);

                    HttpCookie cookies_ = new HttpCookie("gm_userinfo");
                    cookies_.Values.Add("id", pLoginReturn.ID);
                    cookies_.Values.Add("name", pLoginReturn.Name);
                    cookies_.Values.Add("rid", pLoginReturn.Role_ID);
                    cookies_.Values.Add("guid", tokenValue);
                    cookies_.Values.Add("Type", pLoginReturn.Type);
                    cookies_.Values.Add("DataRole", pLoginReturn.DataRole);
                    cookies_.Values.Add("Account", account);
                    //域
                    if (!autoLogin)
                    {
                        cookies_.Expires = DateTime.Now.AddDays(2);
                    }
                    else
                    {
                        cookies_.Expires = DateTime.Now.AddDays(7);
                    }
                    this.Response.Cookies.Add(cookies_);
                    this.Response.AppendCookie(cookies_);
                    string schoolname = GetSchoolName(pLoginReturn.SchoolId);

                    //string xinfo = Session["Type"].ToString() == "1" ? " 老师" : (Session["Type"].ToString() == "2" ? " 学生" : (Session["Type"].ToString() == "3" ? " 企业" : ""));
                    string url = GetLoingApi(pLoginReturn.Type,pLoginReturn.IsCmsAdmin,pLoginReturn.IsMonitorAdmin);
                    string userinfor = "{\"Login\":true,\"Type\":\"" + pLoginReturn.Type + "\",\"Name\":\"" + Session["name"] + "\",\"Email\":\"\",\"url\":\"" + url + "\",\"schoolid\":\"" + pLoginReturn.SchoolId + "\",\"schoolname\":\"" + schoolname + "\"}";


                    return userinfor;
                }
                
            }
            catch (Exception ex) { string s = ex.Message; }
            return "{\"Login\":false}";
        }

        public string GetSchoolName(string schoolid)
        {
            string sql = "select * from  Z_School where RecordId='" + schoolid + "'"; 

            DBOperation db = new DBOperation();
            DataSet ds = new DataSet();
            ds = db.GetDataSet(sql);

            string SchoolName = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                SchoolName = ds.Tables[0].Rows[0]["SchoolName"].ToString();
            }
             
            return SchoolName;
        }

        public string GetLoingApi(string type, string iscmsadmin, string IsMonitorAdmin)
        {
            string url = "";
            string urlapi = ConfigurationManager.AppSettings["LoginUrl"];
            string[] urls = urlapi.Split(new char[] { ';' });
            string currentsys = ""; 

            //排除当前中心用户系统接口
            for (int i = 0; i < urls.Length - 1; i++)
            {
                string[] temp = urls[i].ToString().Split(new char[] { '?' });
                currentsys = temp[1].Split(new char[] { '=' })[1];

                //如果上个页面和接口页面标识一致，则设定当前这个页面地址为需要单点登录后才跳转的地址
                //monitor:监控
                //user:资源共享平台、作为中心系统
                //cms:cms系统
                //bbs:bbs系统
                //zyhd:作业互动平台
                //type:1 教师、需登录接口:cms、bbs、资源共享平台、作业平台、监控
                //type:2:学生、3:企业 需登录接口: bbs、资源共享平台(后台作控制)、作业平台(后台作控制)
                //教师
                if (type == "1")
                {
                    //如果是cms接口或者监控并且已经激活，则添加cms登录接口
                    //if ((currentsys == "cms" || currentsys == "monitor") && iscmsadmin == "1" && IsMonitorAdmin == "1")
                    //{
                    //    url += setUrl(currentsys, i, urls.Length, temp);
                    //}
                    //else if(currentsys!="cms")//如果不是cms接口
                    //{
                    //    url += setUrl(currentsys, i, urls.Length, temp); 
                    //}

                    if (currentsys == "cms" || currentsys == "monitor")
                    {
                        if (currentsys == "cms" && iscmsadmin == "1")
                        {
                            url += setUrl(currentsys, i, urls.Length, temp);
                        }
                        else if (currentsys == "monitor" && IsMonitorAdmin == "1")
                        {
                            url += setUrl(currentsys, i, urls.Length, temp);
                        }
                    }
                    else
                    {
                        url += setUrl(currentsys, i, urls.Length, temp);
                    }

                }
                else//学生
                {
                    if (currentsys == "bbs" || currentsys == "zyhd")
                    {
                        url += setUrl(currentsys, i, urls.Length, temp);
                    }
                }
            }
            url = url.Substring(0, url.Length - 1);
            return url; 
        }

        public string setUrl(string type, int step, int length, string[] urls)
        {
            string url="";
            if (type != "user")
            {
                url = urls[0].ToString() + ";";
            }
            return url;
        }

        /// <summary>
        /// 其他系统登录单点登录用
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="autoLogin"></param>
        /// <returns></returns>
        public string LoginSSO(string callback,string userid, string password,string flag)
        {
            bool autoLogin = false;
            string account = userid;
            try
            {
                HttpCookie myCookie = new HttpCookie("gm_userinfo", null);
                myCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(myCookie);
                Session.RemoveAll();

                Basice pLoginModels = new Basice();
                LoginReturn pLoginReturn;
                // 登录唯一凭证
                string tokenValue = Guid.NewGuid().ToString().ToUpper();
                #region /*获得IP*/
                string userIp = "";
                if (Request.ServerVariables["HTTP_VIA"] != null)
                {
                    userIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    userIp = Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                #endregion
                // 登录
                if (pLoginModels.Login(account, password, userIp, tokenValue, autoLogin, out pLoginReturn))
                {
                    //session 赋值
                    Session["id"] = pLoginReturn.ID;
                    Session["name"] = pLoginReturn.Name;
                    Session["rid"] = pLoginReturn.Role_ID;
                    Session["guid"] = tokenValue;
                    Session["Type"] = pLoginReturn.Type;
                    Session["DataRole"] = pLoginReturn.DataRole;
                    Session["Account"] = account;

                    //HttpCookie myCookie = new HttpCookie("gm_userinfo", null);
                    //myCookie.Expires = DateTime.Now.AddDays(-1d);
                    //Response.Cookies.Add(myCookie);

                    HttpCookie cookies_ = new HttpCookie("gm_userinfo");
                    cookies_.Values.Add("id", pLoginReturn.ID);
                    cookies_.Values.Add("name", pLoginReturn.Name);
                    cookies_.Values.Add("rid", pLoginReturn.Role_ID);
                    cookies_.Values.Add("guid", tokenValue);
                    cookies_.Values.Add("Type", pLoginReturn.Type);
                    cookies_.Values.Add("DataRole", pLoginReturn.DataRole);
                    cookies_.Values.Add("Account", account);
                    //域
                    if (!autoLogin)
                    {
                        cookies_.Expires = DateTime.Now.AddDays(2);
                    }
                    else
                    {
                        cookies_.Expires = DateTime.Now.AddDays(7);
                    }
                    this.Response.Cookies.Add(cookies_);
                    this.Response.AppendCookie(cookies_);

                    //string xinfo = Session["Type"].ToString() == "1" ? " 老师" : (Session["Type"].ToString() == "2" ? " 学生" : (Session["Type"].ToString() == "3" ? " 企业" : ""));

                    string userinfor = "{\"Login\":true,\"Type\":\"" + pLoginReturn.Type + "\",\"Name\":\"" + Session["name"] + "\",\"Email\":\"\",\"flag\":\"" + flag + "\"}";
                    Response.AddHeader("P3P", "CP='IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT'");
                    return callback+"("+userinfor+")";
                }

            }
            catch (Exception ex) { string s = ex.Message; }
            Response.AddHeader("P3P", "CP='IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT'");
            return callback + "({\"Login\":false})";
        }


        /// <summary>
        /// 安全退出
        /// </summary>
        /// <returns></returns>
        public void OutLoginAPI()
        {

            string url = ConfigurationManager.AppSettings["LoginOutUrl"];
            string[] urls = url.Split(new char[] { ';' });
            StringBuilder s = new StringBuilder();
            s.Append("function sysLoginOut(){");
            for (int i = 0; i < urls.Length-1; i++)
            { 
                s.Append("jQuery.ajax({");
                s.Append("url: \""+urls[i]+"\", ");
                s.Append("dataType: \"jsonp\",");
                s.Append("jsonp: \"callback\",");
                s.Append("timeout: 5000,");
                s.Append("success: function (data) {");
                s.Append("},");
                s.Append("error: function () {");
                s.Append(" }});");
            }
            s.Append("}");

            Response.Write(s.ToString());
        }

        /// <summary>
        /// 生成单点登录名单调用接口
        /// </summary>
        /// <returns></returns>
        public void LoginSSOAPI()
        {
             
            string url = ConfigurationManager.AppSettings["LoginUrl"];
            string[] urls = url.Split(new char[] { ';' });
            StringBuilder s = new StringBuilder();
            s.Append("function sysLogin(userid,password){");
            for (int i = 0; i < urls.Length - 1; i++)
            {
                s.Append("jQuery.ajax({");
                s.Append("url: \"" + urls[i] + "\", ");
                s.Append("data: {");
                s.Append("\"userid\": userid,");
                s.Append("\"password\": password");
                s.Append("},");
                s.Append("dataType: \"jsonp\",");
                s.Append("jsonp: \"callback\",");
                s.Append("timeout: 5000,");
                s.Append("success: function (data) {");
                s.Append("},");
                s.Append("error: function () {");
                s.Append(" }});");
            }
            s.Append("}");

            Response.Write(s.ToString());
        }


        /// <summary>
        /// 生成单点登录名单调用接口
        /// </summary>
        /// <returns></returns>
        public void LoginSSOAPI1(string sysname)
        {

            string strurls = ConfigurationManager.AppSettings["LoginUrl"];
            string[] urls = strurls.Split(new char[] { ';' });
            StringBuilder s = new StringBuilder();
            string currentsys="";
            string url="";
            //排除当前中心用户系统接口
            for (int i = 0; i < urls.Length - 1; i++)
            {
                string[] temp = urls[i].ToString().Split(new char[] { '?' });
                currentsys= temp[1].Split(new char[] { '=' })[1];

                //如果上个页面和接口页面标识一致，则设定当前这个页面地址为需要单点登录后才跳转的地址
                if (currentsys != sysname)
                {
                    if (i == urls.Length - 2)
                    {
                        url += temp[0].ToString();
                    }
                    else
                    {
                        url += temp[0].ToString()+ ";";
                    }
                }
            }

            s.Append("var queue = new Array();");
            s.Append("function sysLogin(userid,password,pageurl){");
            s.Append("var strursl="+url+";");
            s.Append("queue = strursl.split(\";\"); ");
            s.Append("getAjaxFun(queue[0],0,userid,password,\"\",resultCallBack)");
            s.Append(" }");

            s.Append("function resultCallBack(data){ ");
            s.Append("var queuecount=parseInt(data.flag)+1; ");
            s.Append("if(parseInt(queuecount)<queue.length){");
            s.Append("getAjaxFun(queue[queuecount],queuecount,userid,password,\"\",resultCallBack)");
            s.Append("}else{");
            s.Append("layer.closeAll();");
            s.Append("window.location.href =pageurl;");
            s.Append("}}");
            s.Append(getAjaxFun()); 

             Response.Write(s.ToString());
        }

        public string getAjaxFun(){
            StringBuilder s = new StringBuilder();
            s.Append("function getAjaxFun(urls,queuecount, userid, password, name, callback) {");
            s.Append("callback = callback || function () { }; ");
            s.Append("jQuery.ajax({");
            s.Append("url: urls,");
            s.Append("dataType: \"jsonp\",");
            s.Append("jsonp: \"callback\",");
            s.Append("crossDomain: true,"); 
            s.Append("data: {");
            s.Append("\"userid\": userid,");
            s.Append("\"password\": password");
            s.Append("\"password\": password");
            s.Append("\"name\": name");
            s.Append("\"flag\": queuecount");
            s.Append("\"password\": password");
            s.Append("\"password\": password");
            s.Append("},");
            s.Append("timeout: 5000,");
            s.Append("success: function (data) {");
            s.Append("callback(data);");
            s.Append("},");
            s.Append("error: function () {");
            s.Append(" }});");
            return s.ToString();
        }

        /// <summary>
        /// 安全退出
        /// </summary>
        /// <returns></returns>
        public ContentResult OutLogin()
        {
            try
            {
                HttpCookie myCookie = new HttpCookie("gm_userinfo", null);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
                string guid = Session["guid"].ToString();
                Session.RemoveAll();
                Basice pLoginModels = new Basice();
                pLoginModels.outLogin(guid);
            }
            catch (Exception ex) { string s = ex.Message; Session.RemoveAll(); }
            return null;
        }


        /// <summary>
        /// login by heqq 2014/07/08
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult getCookie()
        {
            try
            {
                string a = Request.Cookies["gm_userinfo"].Values["Account"].ToString();
                string b = Request.Cookies["gm_userinfo"].Values["guid"].ToString();
                return Content("[\"Account\":\"" + a + "\",\"guid\":\"" + b + "\"]");
            }
            catch (Exception ex) { string s = ex.Message; return Content("[\"Account\":\"\",\"guid\":\"\"]"); }
        }

    }
}
