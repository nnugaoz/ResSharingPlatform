using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ResSharingPlatform.Models;

namespace ResSharingPlatform.Controllers
{
    public class VoucherController : Controller
    {
        //
        // GET: /Voucher/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 获得凭证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult getVoucher()
        {
            try
            {
                if (Request.Cookies["gm_userinfo"] != null)
                {
                    string Account = Request.Cookies["gm_userinfo"].Values["Account"].ToString();
                    string guid = Request.Cookies["gm_userinfo"].Values["guid"].ToString();

                    VoucherModels pVoucherModels = new VoucherModels();
                    LoginReturn pLoginReturn = new LoginReturn();
                    if (pVoucherModels.Verification(Account, guid, out pLoginReturn))
                    {
                        //session 赋值
                        Session["id"] = pLoginReturn.ID;
                        Session["name"] = pLoginReturn.Name;
                        Session["rid"] = pLoginReturn.Role_ID;
                        Session["Type"] = pLoginReturn.Type;
                        Session["DataRole"] = pLoginReturn.DataRole;
                        Session["guid"] = guid;
                        Session["Account"] = Account;
                        string xinfo = pLoginReturn.Type == "1" ? " 老师" : (pLoginReturn.Type == "2" ? " 学生" : (pLoginReturn.Type == "3" ? " 企业" : ""));
                        return Content("[{\"Login\":\"True\",\"Type\":\"" + pLoginReturn.Type + "\",\"STR\":\"" + Session["name"] + xinfo + "\"}]");
                    }
                    return Content("out");

                    
                }
                return Content("no");
            }
            catch (Exception ex)
            {
                ResSharingPlatform.Common.clsLog.ErrorLog("Voucher", "getVoucher", ex.Message);
                return Content("err");
            }
        }
    }
}
