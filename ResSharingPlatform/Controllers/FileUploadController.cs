using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ResSharingPlatform.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /fileUpload/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult File()
        {
            ViewData["format"] = "*.xls;*.xlsx;";
            return View();
        }

        #region "保存文件至本地服务器"
        /// <summary>
        /// 保存文件至本地服务器 by maoh 2014/07/18
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public void Upload(HttpPostedFileBase fileData)
        {
            
            // 如果有文件保存
            if (fileData != null)
            {
                //扩展名
                string extName = Path.GetExtension(fileData.FileName).ToLower();//文件类型;
                string saveName = Guid.NewGuid().ToString() + extName;

                // 判断目录是否存在
                if (!Directory.Exists(Server.MapPath("../xls")))
                {
                    // 创建文件夹
                    DirectoryInfo folder = Directory.CreateDirectory(Server.MapPath("../xls"));
                    folder.Create();
                }

                string filePath = Server.MapPath("../xls/" + saveName);

                try
                {
                    fileData.SaveAs(filePath);
                    HttpContext.Response.Write("true," + saveName + "," + extName + "," + filePath);
                }
                catch (Exception ex)
                {
                    HttpContext.Response.Write("false," + fileData.FileName + "上传失败");
                }
            }

        }

        #endregion

    }
}
