using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using ResSharingPlatform.Common;

namespace HomeworkInteraction.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Password()
        {
            ViewData["UserId"] = CommonUtil.GetSession(Session, "id");
            return View();
        }
        [HttpPost]
        public ContentResult SavePassword(string Login_Name, string Password)
        {
            try
            {
                byte[] result = Encoding.Default.GetBytes(Password);    //Password为输入密码的文本
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                Password = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的

                string sql = "update Z_User set Password='" + Password + "'";
                sql = sql + " where ID='" + Login_Name + "'";

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
