using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using ResSharingPlatform.Models;
using ResSharingPlatform.Lib;
using System.Collections;
using System.Web.Script.Serialization;
using System.IO;
using ResSharingPlatform.Common;

namespace ResSharingPlatform.Controllers
{
    public class MineController : Controller
    {
        #region 文档列表画面
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Library()
        {
            return View();
        }
        #endregion

        #region 文档列表展现
        /// <summary>
        /// 文档列表展现
        /// </summary>
        /// <param name="res_type"></param>
        /// <param name="pagecurrent"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public ActionResult LibraryList(string res_type, string like, string pagecurrent, string orderBy)
        {
            List<AppendixByLike_Result> resourcelist = getList(res_type, like, "", pagecurrent, orderBy, Constant.FOREGROUND_DOC_NUM);

            ViewData["reslist"] = resourcelist;
            ViewData["res_type"] = res_type;
            ViewData["orderBy"] = orderBy;

            return View();
        }
        #endregion

        #region 获取文档列表
        /// <summary>
        /// 获取文档列表
        /// </summary>
        /// <param name="res_type"></param>
        /// <param name="userId"></param>
        /// <param name="pagecurrent"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<AppendixByLike_Result> getList(string res_type, string like, string userId, string pagecurrent, string orderBy, int pageSize)
        {
            string linkpage = "";// 分页标签
            int listcount = 0;
            List<AppendixByLike_Result> resourcelist = null;

            ResourceList rslist = new ResourceList();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                listcount = rslist.GetAppendListCount(res_type, like, userId, db);

                // 初期化时当前页是否为空、若为空默认第一页
                if (string.IsNullOrEmpty(pagecurrent))
                {
                    pagecurrent = "1";
                }
                // 分页
                Pages page = new Pages();
                page.subeachnums = pageSize;// 每页显示的条目数
                page.subnums = listcount;// 总条目数
                page.subcurrentpage = int.Parse(pagecurrent);// 当前被选中的页  
                page.subeachpages = 10;// 每次显示的页数
                page.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
                page.subformname = "ActionForm";// 表单名
                linkpage = page.SubPages();// 生成分页标签

                resourcelist = rslist.GetAppendList(res_type, like, userId, orderBy, page.subeachnums, int.Parse(pagecurrent), db);
            }

            ViewData["pagecurrent"] = pagecurrent;// 当前页
            ViewData["linkpage"] = linkpage;// 分页标签

            return resourcelist;
        }
        #endregion

        #region 视频列表画面
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Video()
        {
            return View();
        }
        #endregion

        #region 视频列表展示
        /// <summary>
        /// 视频列表展示
        /// </summary>
        /// <param name="res_type">附件类型</param>
        /// <param name="pagecurrent">跳转页</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public ActionResult VideoList(string res_type, string like, string pagecurrent, string orderBy)
        {
            List<AppendixByLike_Result> resourcelist = getList(res_type, like, "", pagecurrent, orderBy, Constant.FOREGROUND_VIDEO_NUM);

            int videoCount = resourcelist.Count / 5 + (resourcelist.Count % 5 > 0 ? 1 : 0);

            ViewData["reslist"] = resourcelist;
            ViewData["videoCount"] = videoCount;
            ViewData["res_type"] = res_type;
            ViewData["orderBy"] = orderBy;

            return View();
        }
        #endregion

        #region 我的文库列表画面

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MyResource()
        {
            return View();
        }

        #endregion

        #region 我的文库列表展现
        /// <summary>
        /// 我的文库列表展现
        /// </summary>
        /// <param name="pagecurrent"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public ActionResult MyResList(string userId, string like, string pagecurrent, string orderBy)
        {
            userId = CommonUtil.GetSession(Session, "id");
            List<AppendixByLike_Result> resourcelist = getList("", like, userId, pagecurrent, orderBy, Constant.FOREGROUND_MY_DOC_NUM);

            ViewData["reslist"] = resourcelist;
            ViewData["userId"] = userId;
            ViewData["orderBy"] = orderBy;

            return View();
        }
        #endregion

        #region 在线预览
        /// <summary>
        /// 在线预览
        /// </summary>
        /// <param name="appId">附件id</param>
        /// <param name="ext">文件扩展名</param>
        /// <returns></returns>
        public ActionResult ViewOnline(string appId, string ext, string resId)
        {
            string AnnexName = ""; //附件名称
            string Author = ""; //作者
            string UserName = ""; //上传者
            string Tag = ""; //标签
            string UploadTime = ""; //上传时间
            decimal Review_num = 0; //评价数
            decimal Page_view_num = 0; //浏览数
            decimal Download_num =0 ; //下载数
            decimal Grage = 0; //评分
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                string userId = CommonUtil.GetSession(Session, "id");
                res.AddViewNum(resId, appId, userId, db);
                View_Appendix model = db.View_Appendix.First(t => t.ID.Equals(appId));
                AnnexName = model.FILE_NAME;
                Author = model.AUTHOR;
                UserName = model.USER_NAME;
                Tag = model.LABEL;
                UploadTime = Convert.ToDateTime(model.UPLOAD_TIME == null ? "1900/01/01 00:00:00" : model.UPLOAD_TIME.ToString()).ToString("yyyy/MM/dd HH:mm:ss");
                Review_num = model.REVIEW_NUM == null ? 0 : Convert.ToDecimal(model.REVIEW_NUM);
                Page_view_num = model.PAGE_VIEW_NUM == null ? 0 : Convert.ToDecimal(model.PAGE_VIEW_NUM);
                Download_num = model.DOWNLOAD_NUM == null ? 0 : Convert.ToDecimal(model.DOWNLOAD_NUM);
                Grage = model.GRADE == null ? 0 : Convert.ToDecimal(model.GRADE);
                ResourceList relist = new ResourceList();
                string retStr = "";
                GetTypeListString(relist, db, model.TYPE_ID, ref retStr);
                ViewData["retStr"] = retStr;
            }

            ViewData["appId"] = appId;
            ViewData["AnnexName"] = string.IsNullOrEmpty(AnnexName) ? "" : Path.GetFileNameWithoutExtension(AnnexName);
            ViewData["AnnexType"] = Constant.GetDocType(string.IsNullOrEmpty(AnnexName) ? "" : Path.GetExtension(AnnexName));
            ViewData["Author"] = string.IsNullOrEmpty(Author) ? "" : Author;
            ViewData["UserName"] = string.IsNullOrEmpty(UserName) ? "" : UserName;
            ViewData["Tag"] = string.IsNullOrEmpty(Tag) ? "" : Tag.Replace(",", " ");
            ViewData["UploadTime"] = UploadTime;
            ViewData["Review_num"] = Review_num.ToString();
            ViewData["Page_view_num"] = Page_view_num.ToString();
            ViewData["Download_num"] = Download_num.ToString();
            ViewData["Grage"] = Grage.ToString();

            string fileName = appId + ext;
            ViewData["fileName"] = fileName;
            ViewData["extName"] = "other";
            ViewData["resId"] = resId;

            ext = ext.ToLower();

            string fileType = Constant.GetFileType(ext);

            if (fileType == "0")
            {
                ViewData["extName"] = ".swf";
            }
            else if (fileType == "1")
            {
                ViewData["extName"] = ".flv";
            }
            else if (fileType == "2")
            {
                ViewData["extName"] = ".jpg";
            }

            string pageId = "020101";
            string id = CommonUtil.GetSession(Session, "id");

            ResourceList rslist = new ResourceList();
            AthorityModels am = rslist.GetAthority(id, pageId);

            /**根据用户是否登录，显示收藏按钮。 2014-12-18 5920 start**/
            if (string.IsNullOrEmpty(id))
                ViewData["IsShowCollection"] = "0";
            else
                ViewData["IsShowCollection"] = "1";
            /**根据用户是否登录，显示收藏按钮。 2014-12-18 5920 end**/

            ViewData["DownLoadAthority"] = am.DownLoadAthority;

            return View();
        }

        /**递归资源分类导航**/
        private void GetTypeListString(ResourceList relist, JSZX_ResourceEntities db, string typeId, ref string retString)
        {
            if (!string.IsNullOrEmpty(typeId))
            {
                T_Res_Type type = relist.GetTypeById(typeId, db);
                if (type != null && string.IsNullOrEmpty(type.ID) == false)
                {
                    string str = "<span>></span> ";
                    str += "<a href=\"javascript:doSearch2(\'" + type.ID + "\',\'" + type.BELONG_ID + "\')\" class=\"ml5 mr5\">" + type.NAME + "</a>";
                    retString = str + retString;
                    GetTypeListString(relist, db, type.BELONG_ID, ref retString);
                }
            }
            else
            {
                return;
            }
        }

        /**收藏资源附件 2014-12-18 5920 **/
        /// <summary>
        /// 收藏资源附件
        /// </summary>
        /// <param name="UserID">用户名</param>
        /// <param name="AID">资源附件ID</param>
        [HttpPost]
        public void CollectFile(string AID)
        {
            int ret = 0;
            string UserID = CommonUtil.GetSession(Session, "id");
            try
            {
                if (string.IsNullOrEmpty(UserID)) //用户未登录
                {
                    ret = 3;
                }
                else
                {
                    using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                    {
                        int i = db.T_MyCollection.Count(t => t.USERID.Equals(UserID) && t.AID.Equals(AID) && t.IS_DEL == false);
                        if (i > 0) //已收藏了的，不能再收藏
                        {
                            ret = 2;
                        }
                        else
                        {
                            T_MyCollection model = new T_MyCollection();
                            model.ID = Guid.NewGuid().ToString();
                            model.USERID = UserID;
                            model.AID = AID;
                            model.CreateTime = DateTime.Now;
                            model.IS_DEL = false;
                            db.T_MyCollection.Add(model);
                            db.SaveChanges();
                            ret = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("MineController", "CollectFile", "Error! " + ex.Message);
            }
            finally
            {
                /**返回js (0：收藏失败；1：收藏成功；2：已收藏；3：用户未登录。)**/
                HttpContext.Response.Write(ret.ToString());
            }
        }
        /**取消收藏**/
        [HttpPost]
        public void CannelMyCollect(string AID)
        {
            bool ret = false;
            string UserID = CommonUtil.GetSession(Session, "id");
            try
            {
                if (!string.IsNullOrEmpty(UserID))
                {
                    using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                    {
                        var linq = from t in db.T_MyCollection select t;
                        linq = linq.Where(t => t.AID.Equals(AID));
                        linq = linq.Where(t => t.USERID.Equals(UserID));
                        List<T_MyCollection> models = linq.ToList();
                        foreach (T_MyCollection model in models)
                        {
                            model.IS_DEL = true;
                        }
                        db.SaveChanges();
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("MineController", "CannelMyCollect", "Error! " + ex.Message);
            }
            finally
            {
                HttpContext.Response.Write(ret.ToString().ToLower());
            }
        }
        #endregion

        #region 评价一览
        /// <summary>
        /// 评价一览
        /// </summary>
        /// <param name="sco_app_Id">附件id</param>
        /// <param name="scoPagecurrent">当前页</param>
        /// <returns></returns>
        public ActionResult ScoreList(string sco_app_Id, string scoPagecurrent)
        {
            List<View_Score> scoList = new List<View_Score>();
            string scoLinkpage = "";// 分页标签
            ResourceList reList = new ResourceList();
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                // 初期化时当前页是否为空、若为空默认第一页
                if (string.IsNullOrEmpty(scoPagecurrent))
                {
                    scoPagecurrent = "1";
                }
                // 取得记录集数
                int scoListcount = reList.GetResourceScoreSizeByAppId(sco_app_Id, db);
                // 分页
                Pages2 scoPage = new Pages2();
                scoPage.subeachnums = Constant.VIEWONLINE_COMMENTARY_NUM;// 每页显示的条目数
                scoPage.subnums = scoListcount;// 总条目数
                scoPage.subcurrentpage = int.Parse(scoPagecurrent);// 当前被选中的页  
                scoPage.subeachpages = 10;// 每次显示的页数
                scoPage.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
                scoPage.subformname = "Sco_ActionForm";// 表单名
                scoPage.hidIdName = "scoPagecurrent";//存放当前页的隐藏控件的id
                scoPage.subPageId = "scoPagehow";//跳转页数输入框控件的id
                scoPage.functionName = "GetScoreList();";//获取列表信息的方法名
                scoPage.functionPageName = "scoPage";//翻页的方法名
                scoPage.functionTopageName = "scoTopage";//跳转页面的方法名
                scoLinkpage = scoPage.SubPages();// 生成分页标签
                scoList = reList.GetResourceScoreByAppId(sco_app_Id, scoPage.subeachnums, Convert.ToInt32(scoPagecurrent), db);
            }

            ViewData["scoPagecurrent"] = scoPagecurrent;// 当前页
            ViewData["scoList"] = scoList;
            ViewData["scoLinkpage"] = scoLinkpage;// 分页标签

            return View();
        }
        #endregion

        #region 问题一览
        /// <summary>
        /// 问题一览
        /// </summary>
        /// <param name="sco_app_Id">附件id</param>
        /// <param name="scoPagecurrent">当前页</param>
        /// <returns></returns>
        public ActionResult QaList(string qa_app_Id, string qaPagecurrent)
        {
            List<View_Qa> qaList = new List<View_Qa>();
            string qaLinkpage = "";// 分页标签
            ResourceList reList = new ResourceList();
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                // 初期化时当前页是否为空、若为空默认第一页
                if (string.IsNullOrEmpty(qaPagecurrent))
                {
                    qaPagecurrent = "1";
                }

                // 取得记录集数
                int qaListcount = reList.GetResourceQaSizeByAppId(qa_app_Id, db);

                // 分页
                Pages2 qaPage = new Pages2();
                qaPage.subeachnums = Constant.VIEWONLINE_QUESTION_NUM;// 每页显示的条目数
                qaPage.subnums = qaListcount;// 总条目数
                qaPage.subcurrentpage = int.Parse(qaPagecurrent);// 当前被选中的页  
                qaPage.subeachpages = 10;// 每次显示的页数
                qaPage.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
                qaPage.subformname = "Qa_ActionForm";// 表单名
                qaPage.hidIdName = "qaPagecurrent";//存放当前页的隐藏控件的id
                qaPage.subPageId = "qaPagehow";//跳转页数输入框控件的id
                qaPage.functionName = "GetQaList();";//获取列表信息的方法名
                qaPage.functionPageName = "qaPage";//翻页的方法名
                qaPage.functionTopageName = "qaTopage";//跳转页面的方法名
                qaLinkpage = qaPage.SubPages();// 生成分页标签
                qaList = reList.GetResourceQaByAppId(qa_app_Id, qaPage.subeachnums, Convert.ToInt32(qaPagecurrent), db);
            }

            ViewData["qaPagecurrent"] = qaPagecurrent;// 当前页
            ViewData["qaList"] = qaList;
            ViewData["qaLinkpage"] = qaLinkpage;// 分页标签


            return View();
        }
        #endregion

        #region 将文件从服务器下载到本地
        /// <summary>
        /// 将文件从服务器下载到本地
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="resId">资源id</param>
        /// <param name="appId">附件id</param>
        public void DownLoad(string fileName, string resId, string appId)
        {
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                string userId = CommonUtil.GetSession(Session, "id");
                res.AddDownLoadNum(resId, appId,userId, db);
            }

            string loccalFile = fileName;// Constant.DISK_ADDRESS + fileName;

            try
            {
                if (!System.IO.File.Exists(loccalFile))
                {
                    throw new Exception("文件不存在");
                }

                //100K 每次读取文件，只读取100K
                //const long ChunkSize = 102400;

                //byte[] buffer = new byte[ChunkSize];

                //HttpContext.Response.Clear();

                //FileStream fs = new FileStream(loccalFile, FileMode.Open);

                //long dataLengthToRead = fs.Length;//获取下载的文件总大小 

                #region 20150228 5920 Add 文件ID改成文件原名
                fileName = Path.GetFileName(fileName);
                Resource res = new Resource();
                fileName = res.GetFileOriginalName(fileName);
                #endregion

                HttpContext.Response.ContentType = "application/octet-stream";
                HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName));

                Response.TransmitFile(loccalFile);

                //while (dataLengthToRead > 0 && HttpContext.Response.IsClientConnected)
                //{
                //    int lengthRead = fs.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小 
                //    HttpContext.Response.OutputStream.Write(buffer, 0, lengthRead);
                //    HttpContext.Response.Flush();
                //    dataLengthToRead = dataLengthToRead - lengthRead;
                //}

                //HttpContext.Response.Close();

                ////下载完成
                //if (dataLengthToRead == 0)
                //{
                //    if (fs != null)
                //    {
                //        fs.Close();
                //    }
                //}

                //下载资源时，可以获得积分
                PointRule pr = new PointRule();
                pr.AddPoint(resId, CommonUtil.GetSession(Session, "id"), "1");
                //下载资源时，可以获得积分

            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("MineController", "DownLoad", ex.Message);

                HttpContext.Response.Write("<script type='text/javascript'>layer.alert(" + ex.Message + ")</script>");
            }
        }
        #endregion

        #region 保存评价
        /// <summary>
        /// 保存评价
        /// </summary>
        /// <param name="resId">资源id</param>
        /// <param name="appId">附件id</param>
        /// <param name="grade">评分</param>
        /// <param name="review">评价内容</param>
        public void SaveScore(string resId, string appId, string grade, string review)
        {
            ResourceModel model = new ResourceModel();
            model.Id = resId;
            model.AppId = appId;
            model.Grade = grade;
            model.Review = review;
            model.UserId = CommonUtil.GetSession(Session, "id");
            bool success = false;
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                success = res.insertScore(model, db);
            }
            HttpContext.Response.Write(success.ToString().ToLower());
        }

        /**是否已经评价**/
        [HttpPost]
        public void IsScore(string resId, string appId)
        {
            bool success = false;
            try
            {
                string userId = CommonUtil.GetSession(Session, "id");
                Resource res = new Resource();
                success = res.IsExistScore(resId, appId, userId);
            }
            catch (Exception e)
            {
                clsLog.ErrorLog("Mine", "IsScore", "Error! " + e.Message);
            }
            finally
            {
                HttpContext.Response.Write(success.ToString().ToLower());
            }
        }
        #endregion

        #region 提问
        /// <summary>
        /// 提问
        /// </summary>
        /// <param name="resId">资源id</param>
        /// <param name="appId">附件id</param>
        /// <param name="question">问题</param>
        public void SaveQa(string resId, string appId, string question)
        {
            ResourceModel model = new ResourceModel();
            model.Id = resId;
            model.AppId = appId;
            model.Question = question;
            model.UserId = CommonUtil.GetSession(Session, "id");

            bool success = false;

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                success = res.insertQa(model, db);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(success);

            HttpContext.Response.Write(json);
        }
        #endregion

        #region 检索
        /// <summary>
        /// 关键字检索
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public ActionResult Search(string kw, string radioValue)
        {
            ViewData["keyword"] = kw;
            ViewData["radioValue"] = string.IsNullOrEmpty(radioValue) == true ? "" : radioValue;
            return View();
        }

        /// <summary>
        /// 关键字检索
        /// </summary>
        /// <param name="af_keyword">关键字</param>
        /// <param name="pagecurrent">跳转页</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public ActionResult SearchList(string af_keyword,string fileType, string orderBy, string pagecurrent)
        {
            List<AppendixByLike_Result> list = new List<AppendixByLike_Result>();

            string linkpage = "";// 分页标签

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();

                // 初期化时当前页是否为空、若为空默认第一页
                if (string.IsNullOrEmpty(pagecurrent))
                {
                    pagecurrent = "1";
                }

                int appSize = relist.GetAppendixSizeByKeyword(af_keyword,fileType, db);

                // 分页
                Pages page = new Pages();
                page.subeachnums = Constant.FOREGROUND_SEARCH_NUM;// 每页显示的条目数
                page.subnums = appSize;// 总条目数
                page.subcurrentpage = int.Parse(pagecurrent);// 当前被选中的页  
                page.subeachpages = 10;// 每次显示的页数
                page.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
                page.subformname = "ActionForm";// 表单名
                linkpage = page.SubPages();

                list = relist.GetAppendixListByKeyword(af_keyword,fileType, orderBy, page.subeachnums, Convert.ToInt32(pagecurrent), db);

            }

            ViewData["pagecurrent"] = pagecurrent;// 当前页
            ViewData["list"] = list;
            ViewData["linkpage"] = linkpage;// 分页标签

            return View();
        }
        #endregion

        #region 显示图片
        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="id">附件id</param>
        public void ShowImg(string id)
        {
            T_Res_Appendix app = new T_Res_Appendix();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                app = relist.GetAppendixInfoById(id, db);

                if (app != null)
                {
                    if (app.IMAGE != null)
                    {
                        Response.ContentType = "image/BMP";
                        Response.BinaryWrite(app.IMAGE);
                    }
                    else
                    {
                        FileStream fs = new FileStream(Server.MapPath("../Images/unknow.png"), FileMode.Open);//可以是其他重载方法 
                        byte[] byData = new byte[fs.Length];
                        fs.Read(byData, 0, byData.Length);
                        if (fs != null)
                        {
                            fs.Close();
                        }

                        Response.ContentType = "image/BMP";
                        Response.BinaryWrite(byData);
                    
                    }
                }
            }
        }
        #endregion

        #region 我的收藏
        /**我的收藏**/
        public ActionResult MyCollection()
        {
            return View();
        }
        /**我的收藏列表展示**/
        public ActionResult MyCollectList(string like, string pagecurrent, string orderBy)
        {
            string userId = CommonUtil.GetSession(Session, "id");
            List<MyCollectionByLike_Result> resourcelist = GetMyList(userId, like, orderBy, pagecurrent, Constant.FOREGROUND_MY_DOC_NUM);
            ViewData["reslist"] = resourcelist;
            ViewData["userId"] = userId;
            ViewData["orderBy"] = orderBy;
            return View();
        }
        /**获取收藏列表**/
        private List<MyCollectionByLike_Result> GetMyList(string userId, string like, string orderBy, string pagecurrent, int pageSize)
        {
            string linkpage = "";// 分页标签
            int listcount = 0;
            List<MyCollectionByLike_Result> resourcelist = null;

            ResourceList rslist = new ResourceList();
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                listcount = rslist.GetMyCollectCount(userId, like, db);

                // 初期化时当前页是否为空、若为空默认第一页
                if (string.IsNullOrEmpty(pagecurrent))
                {
                    pagecurrent = "1";
                }
                // 分页
                Pages page = new Pages();
                page.subeachnums = pageSize;// 每页显示的条目数
                page.subnums = listcount;// 总条目数
                page.subcurrentpage = int.Parse(pagecurrent);// 当前被选中的页  
                page.subeachpages = 10;// 每次显示的页数
                page.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
                page.subformname = "ActionForm";// 表单名
                linkpage = page.SubPages();// 生成分页标签

                resourcelist = rslist.GetMyCollectList(userId, like, orderBy, page.subeachnums, page.subcurrentpage, db);
            }

            ViewData["pagecurrent"] = pagecurrent;// 当前页
            ViewData["linkpage"] = linkpage;// 分页标签

            return resourcelist;
        }
        /**删除收藏附件**/
        [HttpPost]
        public void DeleteMyCollect(string myId)
        {
            bool ret = false;
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    T_MyCollection model = db.T_MyCollection.First(t => t.ID == myId);
                    model.IS_DEL = true;
                    db.SaveChanges();
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("MineController", "DeleteMyCollect", "Error! " + ex.Message);
            }
            finally
            {
                HttpContext.Response.Write(ret.ToString().ToLower());
            }
        }
        #endregion

        #region 相关资源
        /**搜索相关资源**/
        public ActionResult RelatedAnnex(string app_Id)
        {
            string AId = app_Id;
            List<View_Appendix> resourcelist;
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                View_Appendix model = db.View_Appendix.First(t => t.ID.Equals(AId));
                string relatedStr = (string.IsNullOrEmpty(model.FILE_NAME) ? "" : model.FILE_NAME)
                                  + ","
                                  + (string.IsNullOrEmpty(model.LABEL) ? "" : model.LABEL);
                var query = from t in db.relatedAnnex(relatedStr) select t;
                query = query.Where(t => !t.ID.Equals(AId));
                resourcelist = query.Take(8).ToList();
            }
            ViewData["reslist"] = resourcelist;
            return View();
        }
        #endregion
    }
}
