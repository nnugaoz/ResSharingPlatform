using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace ResSharingPlatform.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            ViewData["ResoureUrl"] = ConfigurationManager.AppSettings["ResoureUrl"];
            ViewData["HomeWorkUrl"] = ConfigurationManager.AppSettings["HomeWorkUrl"];
            ViewData["SafeMonitorUrl"] = ConfigurationManager.AppSettings["SafeMonitorUrl"];
            ViewData["CmsUrl"] = ConfigurationManager.AppSettings["CmsUrl"];
            ViewData["BbsUrl"] = ConfigurationManager.AppSettings["BbsUrl"];
            ViewData["ResoureNoLoginUrl"] = "../../Login/Login?from=" + Server.UrlEncode(ConfigurationManager.AppSettings["ResoureUrl"] + "/Upload");
            ViewData["ResoureUrl"] = ConfigurationManager.AppSettings["ResoureUrl"] + "/Upload";
            // if (HttpContext.Request.Cookies["gm_userinfo"] == null)
            //{
            //    ViewData["ResoureUrl"]="../../Login/Login?from="+ Server.UrlEncode(ConfigurationManager.AppSettings["ResoureUrl"]+"/Upload");
            //}else{
            //    ViewData["ResoureUrl"]=ConfigurationManager.AppSettings["ResoureUrl"]+"/Upload";
            // }

            return View();
        }
    }
}
