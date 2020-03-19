using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Common;
using System.Text;
using System.Security.Cryptography;
using ResSharingPlatform.Lib;
using System.IO;
using System.Data;

namespace ResSharingPlatform.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateInput(false)]
        public string Add(string from, string username, string password, string realname, string sex, string email)
        {
            try
            {
                //string content = "from:" + from + "username:" + username + "password:" + password + "realname:" + realname + "sex:" + sex + "email:" + email;
                //clsLog.ErrorLog("add", "", content);
                //sex=true, username=t0324, email=1111@aa.com, realname=真实姓名, password=1
                string RecordId = Guid.NewGuid().ToString();
                DateTime CreateTime = DateTime.Now;

                byte[] result = Encoding.Default.GetBytes(password);    //Password为输入密码的文本
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                password = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的
                string roleid = "";
                string type = "5";//其他人员
                if (from == "cms")
                {
                    type = "1";
                }
                else if (from == "bbs")
                {
                    type = "3";
                }

                if(string.IsNullOrEmpty(realname)){
                    realname = username;
                }

                string sql = "insert into Z_User (ID,Type,Name,Login_Name,Password,PageRole_ID,EditDate,DataRole) values ('"
                           + username + "','" + type + "','" + realname + "','" + username + "','" + password + "','" + roleid + "','" + CreateTime + "','1')";
                DBOperation db = new DBOperation();
                //clsLog.ErrorLog("", "", sql);
                db.ExecuteSql(sql);

                return "{\"result\":\"success\"}";
            }
            catch (Exception ex)
            {
                return "{\"result\":\"error\"}";
            }
        }

         [HttpPost]
        [ValidateInput(false)]
        public string Update(string from, string username, string password, string realname, string sex, string email)
        {
            try
            {
                //string content = "from:" + from + "username:" + username + "password:" + password + "realname:" + realname + "sex:" + sex + "email:" + email;
                //clsLog.ErrorLog("update", "", content);
                DateTime CreateTime = DateTime.Now; 

                if (password != "")
                {
                    byte[] result = Encoding.Default.GetBytes(password);    //Password为输入密码的文本
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] output = md5.ComputeHash(result);
                    password = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的
                }

                if (string.IsNullOrEmpty(realname))
                {
                    realname = username;
                }

                string sql = "update Z_User set Name='" + realname + "'";

                if (password != "")
                {
                    sql = sql + " ,Password='" + password + "'";
                } 
                 
                sql = sql + " ,EditDate='" + CreateTime + "'";
                sql = sql + " where ID='" + username + "'";
                //clsLog.ErrorLog("", "", sql);
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);

                return "{\"result\":\"success\"}";
            }
            catch (Exception ex)
            {
                return "{\"result\":\"error\"}";
            }
        }

         [HttpGet]
         [ValidateInput(false)]
         public string UpdateUser(string from, string username, string password, string realname, string sex, string email)
         {
             try
             {
                 //string content = "from:" + from + "username:" + username + "password:" + password + "realname:" + realname + "sex:" + sex + "email:" + email;
                 //clsLog.ErrorLog("update", "", content);
                 DateTime CreateTime = DateTime.Now;

                 if (password != "")
                 {
                     byte[] result = Encoding.Default.GetBytes(password);    //Password为输入密码的文本
                     MD5 md5 = new MD5CryptoServiceProvider();
                     byte[] output = md5.ComputeHash(result);
                     password = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的
                 }

                 if (string.IsNullOrEmpty(realname))
                 {
                     realname = username;
                 }
                 else
                 {
                     realname = Server.UrlDecode(realname);
                 }
                 
                 string sql = "update Z_User set Name='" + realname + "'";

                 if (password != "")
                 {
                     sql = sql + " ,Password='" + password + "'";
                 }

                 sql = sql + " ,EditDate='" + CreateTime + "'";
                 sql = sql + " where ID='" + username + "'";
                 //clsLog.ErrorLog("", "", sql);
                 DBOperation db = new DBOperation();
                 db.ExecuteSql(sql);

                 return "{\"result\":\"success\"}";
             }
             catch (Exception ex)
             {
                 return "{\"result\":\"error\"}";
             }
         }

        [HttpPost]
        [ValidateInput(false)]
        public string Delete(string from, string username)
        {
            try
            {
                //clsLog.ErrorLog("delete", "", username);
                string sql = "delete from Z_User where ID='" + username + "'";
                DBOperation db = new DBOperation();
                db.ExecuteSql(sql);
                return "{\"result\":\"success\"}";
            }
            catch (Exception ex)
            {
                return "{\"result\":\"error\"}";
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="autoLogin"></param>
        /// <returns></returns>
        [HttpPost]
        public void userInfor(string userid, string password)
        {
           
            string account = userid;
            try
            {
                string sql = "select * from  Z_User where ID='" + userid + "'";

                if (!string.IsNullOrEmpty(password))
                {
                    byte[] result = Encoding.Default.GetBytes(password);    //Password为输入密码的文本
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] output = md5.ComputeHash(result);
                    password = BitConverter.ToString(output).Replace("-", "").ToLower(); 
                    sql +=" and Password ='"+password+"'";
                }
                //clsLog.ErrorLog("login", "", sql);
                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                string userinfor = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    userinfor = "{\"Result\":true,\"Name\":\"" + ds.Tables[0].Rows[0]["Name"] + "\"}";
                }
                else
                {
                    userinfor = "{\"Result\":false}";
                }
                Response.Write(userinfor); 
            }
            catch (Exception ex) { string s = ex.Message; }
        }

    }
}
