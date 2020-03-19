using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResSharingPlatform.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        public ActionResult Index()
        {
            //try
            //{
            //    string cpuid = MicrosoftOffice.TimeClass.GetDiskId();//获得 DiskId
            //    ViewData["Message"] = cpuid;
            //}
            //catch (Exception ex)
            //{
            //    string s = ex.Message;
            //    return Content("err");
            //}
            return View();
        }

        /// <summary>
        /// 判断是否在有效期
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult Valid()
        {
            try
            {
                return Content("ok");
                //int res = MicrosoftOffice.TimeClass.InitRegedit();//判断是否在有效期内
                //if (res == 0)
                //{
                //    return Content("ok");
                //}
                //else if (res == 1)
                //{
                //    return Content("软件尚未注册，请注册软件！");//软件尚未注册，请注册软件！
                //}
                //else if (res == 2)
                //{
                //    return Content("注册机器与本机不一致,请联系管理员！");//注册机器与本机不一致,请联系管理员！
                //}
                //else if (res == 3)
                //{
                //    return Content("软件试用已到期！");//软件试用已到期！
                //}
                //else
                //{
                //    return Content("软件运行出错，请重新启动或请联系管理员！");//软件运行出错，请重新启动！
                //}
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return Content("err");
            }
        }

        /// <summary>
        /// 获得 CPU ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult CpuId()
        {
            try
            {
                return Content("");
                //string cpuid = MicrosoftOffice.TimeClass.GetDiskId();//获得 CPU ID
                //return Content(cpuid);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return Content("err");
            }
        }

        /// <summary>
        /// 激活软件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public ContentResult Register(string key)
        {
            try
            {
                return Content("ok");
                //int res = MicrosoftOffice.TimeClass.InitRegedit();
                //if (res != 0)
                //{
                //    MicrosoftOffice.TimeClass.WriteSetting("", key);//激活软件
                //    res = MicrosoftOffice.TimeClass.InitRegedit();//判断是否在有效期内
                //    if (res == 0)
                //    {
                //        return Content("ok");
                //    }
                //    else
                //    {
                //        return Content("no");
                //    }
                //}
                //else
                //{
                //    return Content("no");
                //}
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return Content(s);
            }
        }
    }
}
