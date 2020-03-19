using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using ResSharingPlatform.Lib;
using ResSharingPlatform.Common;
using System.Web.Script.Serialization;
using System.Collections;


namespace ResSharingPlatform.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        #region 资源平台初始化
        /// <summary>
        /// 资源平台初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            #region 判断是否在有效期内
            //if (!Constant.IsRegister())
            //{
            //    try
            //    {
            //        return RedirectToAction("Index", "Register");
            //        //int res = MicrosoftOffice.TimeClass.InitRegedit();//判断是否在有效期内
            //        //if (res != 0)
            //        //{
            //        //    return RedirectToAction("Index", "Register");
            //        //}
            //    }
            //    catch (Exception ex)
            //    {
            //        string s = ex.Message;
            //        return RedirectToAction("Index", "Register");
            //    }
            //}
            #endregion

            SetConfig();
            List<View_Res_Appendix> docList = new List<View_Res_Appendix>();
            List<View_Res_Appendix> videoList = new List<View_Res_Appendix>();
            List<ComboTreeModels> rtlist = new List<ComboTreeModels>();
            List<V_Public_Type> models = new List<V_Public_Type>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                docList = relist.GetExcellentAppendix("0,2,3", db);
                videoList = relist.GetExcellentAppendix("1", db);
                //rtlist = relist.GetResTypeList("tree", db);
                models = db.V_Public_Type.OrderBy(t => t.CREATETIME).ToList();
            }

            int docCount = docList.Count / 4 + (docList.Count % 4 > 0 ? 1 : 0);
            int videoCount = videoList.Count / 4 + (videoList.Count % 4 > 0 ? 1 : 0);

            ViewData["docList"] = docList;
            ViewData["docCount"] = docCount;
            ViewData["videoList"] = videoList;
            ViewData["videoCount"] = videoCount;
            ViewData["typeList"] = rtlist;

            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = Int32.MaxValue;
            string json = js.Serialize(models); //js.Serialize(rtlist);
            ViewData["typeTree"] = json;

            return View();
        }
        #endregion

        #region 根据分类查询资源
        /// <summary>
        /// 根据分类查询资源
        /// </summary>
        /// <returns></returns>
        public ActionResult ListByType(string typeId, string belong)
        {
            T_Res_Type type = new T_Res_Type();
            T_Res_Type fatherType = new T_Res_Type();
            //
            List<V_Public_Type> models = new List<V_Public_Type>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                models = db.V_Public_Type.OrderBy(t=>t.CREATETIME).ToList();

                ResourceList relist = new ResourceList();
                string retStr="";
                GetTypeListString(relist,db,typeId,ref retStr);
                ViewData["retStr"] = retStr;
                if (typeId != null && typeId != "")
                {
                    type = relist.GetTypeById(typeId, db);
                    if (type.BELONG_ID != null && type.BELONG_ID != "")
                    {
                        fatherType = relist.GetTypeById(type.BELONG_ID, db);
                    }
                    if (belong == "2")
                    {
                        ViewData["TypeName_1"] = fatherType.NAME;
                        ViewData["TypeId_1"] = fatherType.ID;
                        ViewData["belong_1"] = "1";
                        ViewData["TypeName_2"] = type.NAME;
                        ViewData["TypeId_2"] = type.ID;
                        ViewData["belong_2"] = "2";
                    }
                    else
                    {
                        ViewData["TypeName_2"] = type.NAME;
                        ViewData["TypeId_2"] = type.ID;
                        ViewData["belong_2"] = "1";
                    }
                    ViewData["typeId"] = typeId;
                }
                else
                {
                    ViewData["typeId"] = null;
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = Int32.MaxValue;
            string json = js.Serialize(models);
            ViewData["typeTree"] = json;
            return View();
        }

        private void GetTypeListString(ResourceList relist, JSZX_ResourceEntities db, string typeId, ref string retString)
        {
            if (typeId != null && typeId != "")
            {
                T_Res_Type type = relist.GetTypeById(typeId, db);
                string str = "<span>></span> ";
                str += "<a href=\"javascript:doSearch2(\'" + type.ID + "\',\'" + type.BELONG_ID + "\')\" class=\"ml5 mr5\">" + type.NAME + "</a>";
                retString = str + retString;
                GetTypeListString(relist, db, type.BELONG_ID, ref retString);
            }
            else
            {
                return;
            }
        }

        /**获取link**/
        [HttpGet]
        public void GetLink(string typeId)
        {
            string retStr = "";
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                GetTypeListString(relist, db, typeId, ref retStr);
            }
            HttpContext.Response.Write(retStr);
        }
        #endregion

        #region 根据分类检索
        /// <summary>
        /// 根据分类检索
        /// </summary>
        /// <param name="typeId">分类id</param>
        /// <param name="belong">级别</param>
        public void Search(string typeId, string belong)
        {
            T_Res_Type type = new T_Res_Type();
            T_Res_Type fatherType = new T_Res_Type();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                type = relist.GetTypeById(typeId, db);
                if (type.BELONG_ID != null && type.BELONG_ID != "")
                {
                    fatherType = relist.GetTypeById(type.BELONG_ID, db);
                }
            }

            string ahtml = "";

            if (belong == "2")
            {
                ahtml = "<span>></span> <a href='javascript:doSearch2(\"" + fatherType.ID + "\",\"1\")' class='ml5 mr5'>" + fatherType.NAME + "</a> <span>></span> <a href='javascript:doSearch2(\"" + type.ID + "\",\"2\")' class='ml5 mr5'>" + type.NAME + "</a>";
            }
            else
            {
                ahtml = "<span>></span> <a href='javascript:doSearch2(\"" + type.ID + "\",\"1\")' class='ml5 mr5'>" + type.NAME + "</a>";
            }

            HttpContext.Response.Write(ahtml);
        }
        #endregion

        #region 根据分类获得文件列表
        /// <summary>
        /// 根据分类获得文件列表
        /// </summary>
        /// <param name="typeId">分类id</param>
        /// <param name="pagecurrent">当前页</param>
        /// <returns></returns>
        public ActionResult FileList(string typeId, string sequence, string pagecurrent, string like)
        {
            List<AppendixByLike_Result> applist = new List<AppendixByLike_Result>();

            string linkpage = "";// 分页标签

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList reList = new ResourceList();

                // 初期化时当前页是否为空、若为空默认第一页
                if (string.IsNullOrEmpty(pagecurrent))
                {
                    pagecurrent = "1";
                }

                int appSize = reList.GetFileSizeByTypeId(typeId, like, db);

                // 分页
                Pages page = new Pages();
                page.subeachnums = Constant.ALLAPPENDIX_NUM;// 每页显示的条目数
                page.subnums = appSize;// 总条目数
                page.subcurrentpage = int.Parse(pagecurrent);// 当前被选中的页  
                page.subeachpages = 10;// 每次显示的页数
                page.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
                page.subformname = "ActionForm";// 表单名
                linkpage = page.SubPages();

                applist = reList.GetFileListByTypeId(typeId, like, sequence, page.subeachnums, Convert.ToInt32(pagecurrent), db);
            }

            ViewData["pagecurrent"] = pagecurrent;// 当前页
            ViewData["appList"] = applist;
            ViewData["linkpage"] = linkpage;// 分页标签

            return View();
        }
        #endregion

        #region 配置参数
        private void SetConfig()
        {
            Constant.SetFileConfig();
            Constant.SetPageConfig();
        }
        #endregion

        #region 检索标签
        /// <summary>
        /// 检索标签
        /// </summary>
        /// <param name="keyword">关键字</param>
        public void SearchLabel()
        {
            List<T_Res_Tag> list = new List<T_Res_Tag>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                list = relist.GetLabelByKeyword(null, "", db);
            }

            List<Hashtable> res = new List<Hashtable>();
            Hashtable ht = null;
            if (list != null)
            {
                foreach (T_Res_Tag tag in list)
                {
                    ht = new Hashtable();
                    ht.Add("name", StringUtil.ObjToString(tag.NAME));
                    ht.Add("py", StringUtil.ObjToString(tag.PY));
                    res.Add(ht);
                }
            }
            
            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(res);

            HttpContext.Response.Write(json);
        }
        #endregion
    }
}
