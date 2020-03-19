using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;

namespace ResSharingPlatform.Controllers
{
    public class streamController : Controller
    {
        //
        // GET: /stream/

        public ActionResult Index()
        {
            return View();
        }

        public void tk(string name,string size,string prefix)
        {
            string url = ConfigurationManager.AppSettings["javaUploadUrl"];

            WebRequest request = WebRequest.Create(url+"?name="+name+"&size="+size+"&prefix="+prefix);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            reader.Dispose(); 
            response.Close();

            HttpContext.Response.Write(str); 
        }

    }
}
