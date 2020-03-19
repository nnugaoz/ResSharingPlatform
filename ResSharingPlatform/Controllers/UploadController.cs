using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResSharingPlatform.Models;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using ResSharingPlatform.Lib;
using System.Web.Script.Serialization;
using System.Collections;
using ResSharingPlatform.Common;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;

namespace ResSharingPlatform.Controllers
{
    public class UploadController : Controller
    {
        #region 上传文件一览页面初始化
        /// <summary>
        /// 初始化session
        /// </summary>
        /// <returns></returns>
        public ActionResult ToIndex()
        {
            Session.Remove("condition");
            Hashtable ht = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(ht);
            Session.Add("condition", json);

            return RedirectToAction("Index");           
        }
        /// <summary>
        /// 上传文件一览页面初始化
        /// </summary>
        /// <param name="resource">资源名称</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <param name="orderBy">排序</param>
        /// <param name="delFlg">删除与否</param>
        /// <param name="origin">来源于</param>
        /// <param name="pagecurrent">当前页</param>
        /// <returns>页面初始化</returns>
        public ActionResult Index(string resource, string type, string uploadTime, string label, string orderBy, string delFlg, string origin, string pagecurrent,string topage)
        {

            if (HttpContext.Request.Cookies["gm_userinfo"] == null)
            {
                return RedirectToAction("../Login/Login");
            }

            string pType = Request.Cookies["gm_userinfo"].Values["Type"].ToString();
            if (pType != "1")
            {
                return View("Error");
            }    

            ViewData["resource"] = resource;        //资源名称
            ViewData["type"] = type;                //资源分类
            ViewData["uploadTime"] = uploadTime;    //上传时间
            ViewData["label"] = "";// label;              //标签
            ViewData["orderBy"] = orderBy;          //排序
            ViewData["delFlg"] = delFlg;            //删除与否
            ViewData["origin"] = origin;            //来源于
            ViewData["pagecurrent"] = pagecurrent;  // 当前页
            ViewData["topage"] = topage;

            string pageId = "020101";
            if (topage == "pending")
            {
                pageId = "020103";
            }
            else if (topage == "checked")
            {
                pageId = "020104";
            }
            else if (topage == "unqualified")
            {
                pageId = "020105";
            }

            string id = CommonUtil.GetSession(Session, "id");

            ResourceList rslist = new ResourceList();
            AthorityModels am = rslist.GetAthority(id, pageId);

            ViewData["AddAthority"] = am.AddAthority;
            ViewData["EditAthority"] = am.EditAthority;
            ViewData["DelAthority"] = am.DelAthority;
            ViewData["ExamineAthority"] = am.ExamineAthority;

            return View();
        }
        #endregion

        #region 获取分类
        /// <summary>
        /// 获取分类
        /// </summary>
        public void GetResType(string type)
        {
            List<ComboTreeModels> rtlist = null;

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList rslist = new ResourceList();
                rtlist = rslist.GetResTypeList(type, db);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();

            string json = js.Serialize(rtlist);

            HttpContext.Response.ContentType = "text/json;charset=UTF-8;";
            HttpContext.Response.Write(json);
        }
        #endregion

        #region 上传编辑资源信息页面
        /// <summary>
        /// 上传编辑资源信息页面
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="resource"></param>
        /// <param name="type"></param>
        /// <param name="uploadTime"></param>
        /// <param name="label"></param>
        /// <param name="orderBy"></param>
        /// <param name="delFlg"></param>
        /// <param name="origin"></param>
        /// <param name="pagecurrent"></param>
        /// <returns></returns>
        public ActionResult Upload(string resId, string resource, string type, string uploadTime, string label, string orderBy, string delFlg, string origin, string pagecurrent,string topage)
        {
            View_Resource detail = null;
            List<T_Res_Appendix> appendixList = null;

            if (!string.IsNullOrEmpty(resId))
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    ResourceList rslist = new ResourceList();
                    detail = rslist.GetResourceDetail(resId, db);

                    appendixList = rslist.GetAppendixList(resId, db);
                }

                ViewData["ID"] = detail.ID;
                ViewData["NAME"] = detail.NAME;
                ViewData["UPLOAD_TIME"] = detail.UPLOAD_TIME;
                ViewData["CREATE_NAME"] = detail.CREATE_NAME;
                ViewData["TYPE_ID"] = detail.TYPE_ID;
                ViewData["INTRODUCTION"] = detail.INTRODUCTION;
                if (detail.LABEL != null)
                {
                    ViewData["LABEL"] = detail.LABEL.Split(',');
                }

                ViewData["NOTE"] = detail.NOTE;
                ViewData["ORIGIN_FLG"] = detail.ORIGIN_FLG;
                ViewData["appendixList"] = appendixList;
            }
            else
            {
                ViewData["ORIGIN_FLG"] = "1";
            }

            ViewData["fileTypeName"] = Constant.FILE_TYPE_NAME;
            ViewData["fileType"] = Constant.FILE_TYPE;
            ViewData["fileSize"] = Constant.FILE_SIZE;

            ViewData["topage"] = topage;

            Session.Remove("condition");

            Hashtable ht = new Hashtable();
            ht.Add("resource", StringUtil.ObjToString(resource));
            ht.Add("type", StringUtil.ObjToString(type));
            ht.Add("uploadTime", StringUtil.ObjToString(uploadTime));
            ht.Add("label", StringUtil.ObjToString(label));
            ht.Add("orderBy", StringUtil.ObjToString(orderBy));
            ht.Add("origin", StringUtil.ObjToString(origin));
            ht.Add("pagecurrent", StringUtil.ObjToString(pagecurrent));

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(ht);

            Session.Add("condition", json);

            return View();
        }
        #endregion

        #region 资源详细页面
        /// <summary>
        /// 资源详细页面
        /// </summary>
        /// <param name="resId">资源ID</param>
        /// <returns></returns>
        public ActionResult Detail(string resId, string resource, string type, string uploadTime, string label, string orderBy, string delFlg, string origin, string pagecurrent,string topage)
        {
            View_Resource detail = new View_Resource();
            List<T_Res_Appendix> appendixList = new List<T_Res_Appendix>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList rslist = new ResourceList();
                detail = rslist.GetResourceDetail(resId, db);

                appendixList = rslist.GetAppendixList(resId, db);
            }

            ViewData["ID"] = detail.ID;
            ViewData["NAME"] = detail.NAME;
            ViewData["UPLOAD_TIME"] = detail.UPLOAD_TIME;
            ViewData["INTRODUCTION"] = detail.INTRODUCTION;
            ViewData["TYPE_NAME"] = detail.TYPE_NAME;
            if (detail.LABEL != null)
            {
                ViewData["LABEL"] = detail.LABEL.Split(',');
            }
            ViewData["ORIGIN_FLG"] = detail.ORIGIN_FLG;
            ViewData["appendixList"] = appendixList;

            ViewData["topage"] = topage;

            Session.Remove("condition");

            Hashtable ht = new Hashtable();
            ht.Add("resource", StringUtil.ObjToString(resource));
            ht.Add("type", StringUtil.ObjToString(type));
            ht.Add("uploadTime", StringUtil.ObjToString(uploadTime));
            ht.Add("label", StringUtil.ObjToString(label));
            ht.Add("orderBy", StringUtil.ObjToString(orderBy));
            ht.Add("origin", StringUtil.ObjToString(origin));
            ht.Add("pagecurrent", StringUtil.ObjToString(pagecurrent));

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(ht);

            Session.Add("condition", json);

            return View();
        }
        #endregion

        #region 保存资源信息
        /// <summary>
        /// 保存资源信息
        /// </summary>
        /// <param name="form">资源信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(FormCollection form)
        {
            ResourceModel rm = new ResourceModel();
            rm.Id = form["resId"];
            rm.Name = form["resourceName"];
            rm.TypeId = form["typeId"];
            rm.Introduction = form["introduction"];
            rm.Note = form["note"];
            rm.DataType = form.GetValues("dataType");
            rm.FileName = form.GetValues("fileName");
            rm.SaveName = form.GetValues("saveName");
            rm.Author = form.GetValues("author");
            rm.isForever = form.GetValues("isForever");
            rm.ActiveTimeStart = form.GetValues("activeStart");
            rm.ActiveTimeEnd = form.GetValues("activeEnd");
            rm.UploadTime = form.GetValues("uploadTime");
            rm.FileType = form.GetValues("fileType");
            rm.FileUrl = form.GetValues("fileUrl");
            rm.UserId = CommonUtil.GetSession(Session, "id");

            //标签
            string label = "";
            string[] labels = form.GetValues("savelabel");

            if(labels != null)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    label = label + "," + labels[i];
                }
            }

            if (!string.IsNullOrEmpty(label))
            {
                label = label.Substring(1, label.Length - 1);
            }
            rm.Label = label;

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                if (string.IsNullOrEmpty(rm.Id.ToString()))
                {
                    rm.Id = Guid.NewGuid().ToString();
                    if (res.insertResource(rm, db))
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //插入失败
                    }
                }
                else
                {
                    if (res.updateResource(rm, db))
                    {
                        return RedirectToAction("Index", new { topage = form["topage"] });
                    }
                    else
                    {
                        //保存成功
                    }
                }
            }
            return RedirectToAction("Index", new { topage = form["topage"] });
        }
        #endregion

        #region 检索按钮
        /// <summary>
        /// 检索按钮
        /// </summary>
        /// <param name="resource">资源名称</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <param name="orderBy">排序</param>
        /// <param name="delFlg">删除标志</param>
        /// <param name="origin">来源标志</param>
        /// <param name="pagecurrent">当前被选中的页</param>
        /// <returns>检索出的资源信息</returns>
        public ActionResult SearchList(string resource,string type,string uploadTime,string label,string orderBy,
                                    string delFlg, string origin, string pagecurrent, string pagecurrent2, string topage,
                                    string AddAthority,string EditAthority,string DelAthority,string ExamineAthority)
        {
            resource = string.IsNullOrEmpty(resource) ? "" : resource.Trim();
            label = string.IsNullOrEmpty(label) ? "" : label.Trim();
            string addAthority = AddAthority;
            string editAthority = EditAthority;
            string delAthority = DelAthority;
            string examineAthority = ExamineAthority;

            string dataRole = CommonUtil.GetSession(Session, "DataRole");
            string userId = CommonUtil.GetSession(Session, "id");

            List<View_Resource> resourcelist = null;
            ResourceList rslist = new ResourceList();
            string linkpage = "";// 分页标签
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {

                // 初期化时当前页是否为空、若为空默认第一页
                if (string.IsNullOrEmpty(pagecurrent))
                {
                    if (!string.IsNullOrEmpty(pagecurrent2))
                    {
                        pagecurrent = pagecurrent2;
                    }
                    else
                    {
                        pagecurrent = "1";
                    }
                }

                //取得当前登陆用户所属的学校ID
                string SchoolId = rslist.GetLoginUserSchoolId(userId);

                // 取得记录集数
                int listcount = rslist.GetResourceListSize(SchoolId,userId, dataRole, resource, type, uploadTime, label, orderBy, delFlg, origin, topage, db);

                // 分页
                Pages page = new Pages();
                page.subeachnums = Constant.BACKGROUND_RESOURCE_NUM;// 每页显示的条目数
                page.subnums = listcount;// 总条目数
                page.subcurrentpage = int.Parse(pagecurrent);// 当前被选中的页  
                page.subeachpages = 10;// 每次显示的页数
                page.subpagetype = 2;// 分页样式1:普通模式、2：经典模式
                page.subformname = "ActionForm";// 表单名
                linkpage = page.SubPages();// 生成分页标签

                // 删除时的设置 start 
                // 删除操作时如果当前页大于总页数，当前页设为最后一页
                if (int.Parse(pagecurrent) > page.subpagenums && page.subpagenums != 0)
                {
                    pagecurrent = page.subpagenums.ToString();
                }
                // 删除时的设置 end

                resourcelist = rslist.GetResourceList(SchoolId,userId, dataRole, resource, type, uploadTime, label, orderBy, delFlg, origin, page.subeachnums, int.Parse(pagecurrent), topage, db);
            }

            ViewData["resource"] = resource;        //资源名称
            ViewData["type"] = type;                //资源分类
            ViewData["uploadTime"] = uploadTime;    //上传时间
            ViewData["label"] = label;              //标签
            ViewData["orderBy"] = orderBy;          //排序
            ViewData["delFlg"] = delFlg;
            ViewData["origin"] = origin;
            ViewData["pagecurrent"] = pagecurrent;// 当前页
            ViewData["topage"] = topage;

            ViewData["resources"] = resourcelist;   //列表信息
            ViewData["linkpage"] = linkpage;// 分页标签

            ViewData["AddAthority"] = addAthority;
            ViewData["EditAthority"] = editAthority;
            ViewData["DelAthority"] = delAthority;
            ViewData["ExamineAthority"] = examineAthority;

            return View();
        }
        #endregion

        #region 删除资源
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="resource">资源名称</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <param name="orderBy">排序</param>
        /// <param name="delFlg">删除标志</param>
        /// <param name="origin">来源标志</param>
        /// <param name="pagecurrent">当前被选中的页</param>
        /// <returns></returns>
        public ActionResult Delete(string id, string resource, string type, string uploadTime, string label, string orderBy, string delFlg, string origin, string pagecurrent, string pagecurrent2, string topage,
                                    string AddAthority, string EditAthority, string DelAthority, string ExamineAthority)
        {
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                if (!string.IsNullOrEmpty(id))
                {
                    string userId = CommonUtil.GetSession(Session, "id");
                    if (res.deleteResource(id, userId, db))
                    {
                        //return RedirectToAction("Index");
                    }
                }
            }
            ViewData["topage"] = topage;
            return RedirectToAction("SearchList", new
            {
                resource = resource,
                type = type,
                uploadTime = uploadTime,
                label = label,
                orderBy = orderBy,
                delFlg = delFlg,
                origin = origin,
                pagecurrent = pagecurrent,
                pagecurrent2 = pagecurrent2,
                topage = topage,
                AddAthority = AddAthority,
                EditAthority = EditAthority,
                DelAthority = DelAthority,
                ExamineAthority = ExamineAthority
            });
        }
        #endregion
        
        #region 上传服务器
        /// <summary>
        /// 上传服务器
        /// </summary>
        /// <param name="fileData">文件</param>
        public void UploadPdf(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                string filetype = fileData.ContentType.ToLower();//文件类型

                //上传根目录
                string uploadDir = Constant.DISK_ADDRESS + Constant.UPLOADDIRECTORY;

                //扩展名
                string extName = Path.GetExtension(fileData.FileName).ToLower();
                string saveName = Guid.NewGuid() + extName;

                UploadFileToFtp upload = new UploadFileToFtp();
                try
                {
                    string fileType = Constant.GetFileType(extName);
                    //上传子目录
                    string childDir = "";
                    if (fileType == "0")
                    {
                        childDir = Constant.DOCDDIRECTORY;
                    }
                    else if (fileType == "1")
                    {
                        childDir = Constant.VFDDIRECTORY;
                    }
                    else if (fileType == "2")
                    {
                        childDir = Constant.IMGDIRECTORY;
                    }
                    else
                    {
                        childDir = Constant.OTHERDIRECTORY;
                    }

                    string dirPath = uploadDir + "\\" + childDir;

                    //上传后文件路径
                    string filePath = upload.fileUpload(fileData, dirPath, saveName);

                    HttpContext.Response.Write("true," + saveName + "," + fileType + "," + filePath);
                }
                catch (Exception ex)
                {
                    clsLog.ErrorLog("UploadController", "UploadPdf", ex.Message);
                    HttpContext.Response.Write("false," + ex.Message);
                }
            }
            else
            {
                HttpContext.Response.Write("false,请选择要上传的文件");
            }
        }
        #endregion

        #region 文件格式转换
        /// <summary>
        /// 文件格式转换
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="saveName">文件名</param>
        /// <param name="extName">文件后缀名</param>
        /// <returns></returns>
        private string ConvertFormat(string filePath, string saveName, string extName)
        {
            extName = Constant.GetDocType(extName);

            ////转格式后的文件存储路径
            //string targetPath = Constant.DISK_ADDRESS + Constant.UPLOADDIRECTORY + "\\" + Constant.SWFDIRECTORY;
            string swfName = "";
            //if (filePath != null)
            //{
            //    FileToFlash ftf = new FileToFlash();
            //    //pdf转成swf文件
            //    if (extName == "3")
            //    {
            //        swfName = ftf.PdfToSwf(filePath, targetPath, saveName);
            //    }
            //    else
            //    {
            //        //office转pdf
            //        string newFilePath = null;
            //        if (extName == "0")
            //        {
            //            newFilePath = ftf.WordToPDF(filePath, targetPath, saveName);
            //        }
            //        else if (extName == "1")
            //        {
            //            newFilePath = ftf.ExcelToPDF(filePath, targetPath, saveName);
            //        }
            //        else if (extName == "2")
            //        {
            //            newFilePath = ftf.PowerPointToPDF(filePath, targetPath, saveName);
            //        }
            //        //pdf转swf
            //        if (newFilePath != null)
            //        {
            //            swfName = ftf.PdfToSwf(newFilePath, targetPath, saveName);
            //        }
            //    }
            //}

            return swfName;
        }
        #endregion

        #region 在线预览
        /// <summary>
        /// 在线预览
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public ActionResult ViewOnline(string fileName)
        {
            ViewData["fileName"] = fileName;
            ViewData["extName"] = "other";

            string ext = Path.GetExtension(fileName).ToLower();
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

            ViewData["DownLoadAthority"] = am.DownLoadAthority;

            return View();
        }

        /// <summary>
        /// 在线预览
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void ToView(string fileName)
        {
            //文件扩展名
            string ext = Path.GetExtension(fileName).ToLower();
            //文件类型 0：文档，1：视频，2:图片，3：其他
            string typeFlg = Constant.GetFileType(ext);

            //文件id
            string fileId = Path.GetFileNameWithoutExtension(fileName);

            //文件转成swf文件后的文件名
            string fileName2 = fileName;
            if (ext != null && ext != "")
            {
                fileName2 = fileId + ".swf";
            }

            //根据附件id获取附件信息
            T_Res_Appendix app = null;
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                app = relist.GetAppendixInfoById(fileId, db);
            }

            //文件存储路径
            string filePath = "";
            //文件转格式后的存储路径
            string readPath = "";
            int pageCount = 0;

            //如果数据库中有数据则获取数据库中的文件存储路径
            if (app != null)
            {
                filePath = app.FILE_URL;
                readPath = app.READ_URL;
                pageCount = app.PAGECOUNT;
            }
            Hashtable ht = null;
            try
            {
                //文件存储路径
                string sourcePath = filePath;
                if (!System.IO.File.Exists(sourcePath))
                {
                    throw new Exception("文件不存在");
                }

                if (!System.IO.File.Exists(readPath) && !Directory.Exists(readPath))
                {
                    throw new Exception("文件未处理好，请等待");
                }

                //返回在线阅读的路径
                string savePath = StringUtil.ObjToString(readPath).Replace("\\", "/");
                string[] path = savePath.Split('/');

                savePath = Constant.SWF_ADDRESS + path[path.Length - 2] + "/" + path[path.Length - 1];
                
                //返回信息
                ht = new Hashtable();
                ht.Add("success", "true");
                ht.Add("savePath", savePath);
                ht.Add("filePath", filePath);
                ht.Add("pageCount", pageCount);
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("UploadController", "ToView", ex.Message);
                ht = new Hashtable();
                readPath = ex.Message;
                ht.Add("success", "false");
                ht.Add("savePath", readPath);
                ht.Add("filePath", filePath);
                ht.Add("pageCount", pageCount);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(ht);

            HttpContext.Response.Write(json);
        }
        #endregion

        #region 下载时判断文件是否存在
        /// <summary>
        /// 下载时判断文件是否存在
        /// </summary>
        /// <param name="fileName">文件</param>
        public void CheckFileIsExist(string fileName)
        {
            //文件存储路径
            string loccalFile = fileName;// Constant.DISK_ADDRESS + fileName;

            Hashtable ht = new Hashtable();
            ht.Add("isExist", System.IO.File.Exists(loccalFile));

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(ht);

            HttpContext.Response.Write(json);
        }
        #endregion

        #region 将文件从服务器下载到服务器上
        /// <summary>
        /// 将文件从服务器下载到服务器上
        /// </summary>
        /// <param name="fileName">文件路劲</param>
        public void DownLoad(string fileName)
        {
            //文件存储路径
            string loccalFile = fileName;// Constant.DISK_ADDRESS + fileName;

            try
            {
                if (!System.IO.File.Exists(loccalFile))
                {
                    throw new Exception("文件不存在");
                }

                // //100K 每次读取文件，只读取100K
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
            }
            catch (Exception ex)
            {
                HttpContext.Response.Write("<script type='text/javascript'>layer.alert(" + ex.Message + ")</script>");
            }
        }
        #endregion

        #region 新建分类
        /// <summary>
        /// 新建分类
        /// </summary>
        /// <param name="resType">分类名</param>
        /// <param name="father">上级id</param>
        public void SaveResType(string resType, string father)
        {
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                string userId = CommonUtil.GetSession(Session, "id");
                if (res.insertResType(resType, father,userId, db))
                {
                    HttpContext.Response.Write("true");
                }
            }
        }
        #endregion

        #region 回复提问
        /// <summary>
        /// 回复提问
        /// </summary>
        /// <param name="qaId">提问id</param>
        /// <param name="answer">回复</param>
        public void SaveAnswer(string qaId, string answer)
        {
            Hashtable ht = new Hashtable();
            ht.Add("id", qaId);
            ht.Add("answer", "");

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                string userId = CommonUtil.GetSession(Session, "id");
                if (res.updateResQa(qaId, answer, userId, db))
                {
                    ht.Remove("answer");
                    ht.Add("answer", answer);
                }
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(ht);

            HttpContext.Response.Write(json);
        }
        #endregion

        #region 实时刷新
        /// <summary>
        /// 实时刷新
        /// </summary>
        /// <param name="resId">资源id</param>
        public void Refresh(string resId)
        {
            List<View_Score> scoreList = new List<View_Score>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList rslist = new ResourceList();

                scoreList = rslist.GetResourceScoreGrade(resId, db);
            }

            double score = 0;
            foreach (View_Score resC in scoreList)
            {
                score = score + (double)resC.GRADE;
            }

            string grade = "0";
            if (scoreList.Count > 0)
            {
                grade = Math.Round(score / scoreList.Count, 1).ToString();
            }

            Hashtable ht = new Hashtable();
            ht.Add("grade", grade);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(ht);

            HttpContext.Response.Write(json);
        }
        #endregion

        #region 资源审核画面
        /// <summary>
        /// 资源审核画面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <param name="type"></param>
        /// <param name="uploadTime"></param>
        /// <param name="label"></param>
        /// <param name="orderBy"></param>
        /// <param name="delFlg"></param>
        /// <param name="origin"></param>
        /// <param name="pagecurrent"></param>
        /// <returns></returns>
        public ActionResult Examine(string id, string resource, string type, string uploadTime, string label, string orderBy, string delFlg, string origin, string pagecurrent,string topage) {

            Upload(id, resource, type, uploadTime, label, orderBy, delFlg, origin, pagecurrent, topage);

            return View();
        }
        #endregion

        #region 资源审核
        /// <summary>
        /// 资源审核
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult DoExamine(FormCollection form)
        {
            ResourceModel rm = new ResourceModel();
            rm.Id = form["resId"];
            rm.Name = form["resourceName"];
            rm.TypeId = form["typeId"];
            rm.Introduction = form["introduction"];
            rm.Status = form["status"];
            rm.Excellent_Flg = form["excellentFlg"];
            rm.UserId = CommonUtil.GetSession(Session, "id");

            //标签
            string label = "";
            string[] labels = form.GetValues("savelabel");

            if (labels != null)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    label = label + "," + labels[i];
                }
            }

            if (!string.IsNullOrEmpty(label))
            {
                label = label.Substring(1, label.Length - 1);
            }
            rm.Label = label;

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                if (!string.IsNullOrEmpty(rm.Id))
                {
                    if (res.examineResource(rm, db))
                    {

                    }
                }
            }

            return RedirectToAction("Index", new { resource = form["resource"], type = form["type"], uploadTime = form["upload_Time"], label = form["labels"], orderBy = form["orderBy"], delFlg = form["delFlg"], origin = form["origin"], pagecurrent = form["pagecurrent"],topage=form["topage"] });
        
        }
        #endregion

        #region 批量资源审核
        /// <summary>
        /// 批量资源审核
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult BatchExamine(FormCollection form)
        {
            ResourceModel rm = new ResourceModel();
            rm.Ids = form.GetValues("resIds");
            rm.isSelect = form.GetValues("hidIsSelect");
            rm.Status = form["Status"];
            rm.Excellent_Flg = form["ExcellentFlg"];
            rm.UserId = CommonUtil.GetSession(Session, "id");

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                res.examineResource2(rm, db);
            }

            string pagecurrent = form["pagecurrent"];
            return RedirectToAction("Index", new { resource = form["resource"], type = form["type"], uploadTime = form["upload_Time"], label = form["labels"], orderBy = form["orderBy"], delFlg = form["delFlg"], origin = form["origin"], pagecurrent = pagecurrent, topage = form["topage"] });

        }
        #endregion

        #region 批量资源删除
        /// <summary>
        /// 批量资源删除
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult BatchDelete(FormCollection form)
        {
            ResourceModel rm = new ResourceModel();
            rm.Ids = form.GetValues("resIds");
            rm.isSelect = form.GetValues("hidIsSelect");
            rm.UserId = CommonUtil.GetSession(Session, "id");

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                res.deleteResource2(rm, db);
            }

            return RedirectToAction("SearchList", new
            {
                resource = form["resource"],
                type = form["type"],
                uploadTime = form["upload_Time"],
                label = form["labels"],
                orderBy = form["orderBy"],
                delFlg = form["delFlg"],
                origin = form["origin"],
                pagecurrent = form["pagecurrent"],
                pagecurrent2 = form["pagecurrent2"],
                topage = form["topage"],
                AddAthority = form["AddAthority"],
                EditAthority = form["EditAthority"],
                DelAthority = form["DelAthority"],
                ExamineAthority = form["ExamineAthority"]
            });
        }
        #endregion

        #region 评价一览
        /// <summary>
        /// 评价一览
        /// </summary>
        /// <param name="sco_res_Id">资源id</param>
        /// <param name="scoPagecurrent">当前页</param>
        /// <returns></returns>
        public ActionResult ScoreList(string sco_res_Id, string scoPagecurrent)
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
                int scoListcount = reList.GetResourceScoreSize(sco_res_Id, db);

                // 分页
                Pages2 scoPage = new Pages2();
                scoPage.subeachnums = Constant.BACKGROUND_COMMENTARY_NUM;// 每页显示的条目数
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

                scoList = reList.GetResourceScore(sco_res_Id, scoPage.subeachnums, Convert.ToInt32(scoPagecurrent), db);
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
        /// <param name="qa_res_Id">资源id</param>
        /// <param name="qaPagecurrent">当前页</param>
        /// <returns></returns>
        public ActionResult QaList(string qa_res_Id, string qaPagecurrent)
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
                int qaListcount = reList.GetResourceQaSize(qa_res_Id, db);

                // 分页
                Pages2 qaPage = new Pages2();
                qaPage.subeachnums = Constant.BACKGROUND_QUESTION_NUM;// 每页显示的条目数
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

                qaList = reList.GetResourceQa(qa_res_Id, qaPage.subeachnums, Convert.ToInt32(qaPagecurrent), db);
            }

            ViewData["qaPagecurrent"] = qaPagecurrent;// 当前页
            ViewData["qaList"] = qaList;
            ViewData["qaLinkpage"] = qaLinkpage;// 分页标签

            return View();
        }
        #endregion

        #region 获取标签
        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="keyword"></param>
        public void GetLabel(string id,string keyword)
        {
            if (id == null)
            {
                id = "";
            }
            List<T_Res_Tag> list = new List<T_Res_Tag>();
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                list = relist.GetLabelByKeyword(id, keyword, db);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(list);

            HttpContext.Response.Write(json);

        }
        #endregion

        #region 分类管理页面
        /// <summary>
        /// 分类管理页面初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassificationManage()
        {
            List<ComboTreeModels> rtlist = new List<ComboTreeModels>();
            T_Res_Type type = new T_Res_Type();
            T_Res_Type fatherType = new T_Res_Type();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                rtlist = relist.GetResTypeList("tree", db);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(rtlist);
            ViewData["typeTree"] = json;

            return View();
        }
        /***添加资源分类***/
        /*父类ID*/
        /*新类名*/
        public ActionResult AddType(string selTypeID, string addTypeName)
        {
            selTypeID = (selTypeID == null) ? "" : selTypeID;
            addTypeName = addTypeName.Trim();
            T_Res_Type type = new T_Res_Type();
            type.ID = Guid.NewGuid().ToString();
            type.NAME = addTypeName;
            type.BELONG_ID = selTypeID;
            type.CREATEID = CommonUtil.GetSession(Session, "id");
            type.CREATETIME = DateTime.Now;

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                //查询对应节点下是否存在同名的分类
                var query = from t in db.T_Res_Type select t;
                query = query.Where(t=>t.BELONG_ID.Equals(selTypeID));
                query = query.Where(t => t.NAME.Equals(addTypeName));
                if (query.Count() <= 0)
                {
                    ResourceList relist = new ResourceList();
                    relist.AddType(type, db);
                }
            }
            return RedirectToAction("ClassificationManage");
        }
        /***删除资源分类以及子类***/
        public ActionResult DelType(string selTypeID)
        {
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                relist.DelType(selTypeID, db);
            }
            return RedirectToAction("ClassificationManage");
        }
        /**此分类是否被使用**/
        [HttpGet]
        public void IsUsedType(string typeId)
        {
            bool ret = false;
            ResourceList res = new ResourceList();
            ret = res.TypeIsUsed(typeId);
            HttpContext.Response.Write(ret.ToString().ToLower());
        }
        /**此分类名称是否在同节点存在**/
        [HttpGet]
        public void IsExsit(string selTypeID, string addTypeName)
        {
            bool ret = false;
            try
            {
                selTypeID = (selTypeID == null) ? "" : selTypeID;
                addTypeName = addTypeName.Trim();
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var query = from t in db.T_Res_Type select t;
                    query = query.Where(t => t.BELONG_ID.Equals(selTypeID));
                    query = query.Where(t => t.NAME.Equals(addTypeName));
                    if (query.Count() > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Upload", "IsExsit", "Error! " + ex.Message);
            }
            finally
            {
                HttpContext.Response.Write(ret.ToString().ToLower());
            }
        }
        #endregion
        
        #region 标签管理页面
        /// <summary>
        /// 标签管理页面初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult LabelManage(string selParent, string selParentName)
        {
            List<T_Res_Tag> list = new List<T_Res_Tag>();
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                list = relist.GetLabelByKeyword("", "", db);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            String json = ser.Serialize(list);
            ViewData["typeTree"] = json;
            ViewData["ParentID"] = string.IsNullOrEmpty(selParent) == true ? "" : selParent;
            ViewData["ParentName"] = string.IsNullOrEmpty(selParentName) == true ? "" : selParentName;
            return View();
        }
        /***添加标签***/
        public ActionResult AddTag(string addTagName, string tagTypeID, string parentID, string parentName)
        {
            tagTypeID = (tagTypeID == null) ? "" : tagTypeID;
            T_Res_Tag tag = new T_Res_Tag();
            tag.ID = Guid.NewGuid().ToString();
            tag.NAME = addTagName;
            tag.PARENTID = tagTypeID;
            tag.CREATEID = CommonUtil.GetSession(Session, "id");
            tag.CREATETIME = DateTime.Now;
            tag.PY = Hz2Py.Convert(addTagName);
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                relist.AddTag(tag, db);
            }
            return RedirectToAction("LabelManage", new { selParent = parentID, selParentName = parentName });
        }
        /***删除标签***/
        public ActionResult DelTag(string delTagID, string parentID, string parentName)
        {
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList relist = new ResourceList();
                relist.DelTag(delTagID, db);
            }
            return RedirectToAction("LabelManage", new { selParent = parentID, selParentName = parentName });
        }
        #endregion

        #region 页面菜单列表
        /// <summary>
        /// 页面菜单列表 add by wuxh 2014/8/6
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public String Getqx()
        {
            try
            {
                var menus = "";

                MenuOperate op = new MenuOperate();
                List<SPT_Menus_User_Result> list = new List<SPT_Menus_User_Result>();
                list = op.Get_Menus(Convert.ToString(Session["id"]));

                List<SPT_Menus_User_Result> it = new List<SPT_Menus_User_Result>();
                string str = "";

                // 最大层级数
                int max_level = 0;
                if (list != null && list.Count() > 0)
                {
                    max_level = list.Max(p => p.Code.Length) / 2;
                }
                
                // 取第2层数据
                it = list.FindAll(p => p.Code.Length == 4);
                for (int i = 0; i < it.Count; i++)
                {
                    menus += ""
                        + "<li class=\"frist hassub\"><a href=\"" + it[i].Url + "\" class=\"one\">" + it[i].Title + "</a>"
                        + "%%%" + it[i].Code + "%%%"
                        + "</li>";
                }

                // 取第三层以后的数据
                for (int l = 3; l <= max_level; l++)
                {
                    it = list.FindAll(p => p.Code.Length == l * 2);
                    for (int i = 0; i < it.Count; i++)
                    {
                        str = ""
                            + "<ul class=\"subnav mynav\">"
                            + "<li><a href=\"" + it[i].Url + "\">" + it[i].Title + "</a></li>"
                            + "</ul>"
                            + "%%%" + it[i].Code.Substring(0, (l - 1) * 2) + "%%%";

                        menus = menus.Replace("%%%" + it[i].Code.Substring(0, (l - 1) * 2) + "%%%", str);
                    }
                }

                // 移除所有的记号
                for (int i = 0; i < list.Count; i++)
                {
                    menus = menus.Replace("%%%" + list[i].Code + "%%%", "");
                }

                //HttpCookie cookies_ = new HttpCookie("Meun");
                //cookies_.Values.Add("value", menus);
                //this.Response.Cookies.Add(cookies_);
                Session["menus"] = menus;
                return menus;
            }
            catch (Exception ex) { string s = ex.Message; }

            return "";
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="form"></param>
        public ActionResult DeleteFile(FormCollection form)
        {
            string topage = form["topage"];
            string resId = form["resId"];
            string[] fileUrl = form.GetValues("fileUrl");

            if (string.IsNullOrEmpty(resId))
            {
                if (fileUrl != null)
                {
                    for (int i = 0; i < fileUrl.Length; i++)
                    {
                        try
                        {
                            if (System.IO.File.Exists(fileUrl[i]))
                            {
                                System.IO.File.Delete(fileUrl[i]);
                            }
                        }
                        catch (Exception ex)
                        {
                            clsLog.ErrorLog("UploadController", "DeleteFile", ex.Message);
                        }
                    }
                }
            }

            return RedirectToAction("Index", new { topage = topage });
        }
        #endregion

        #region 大文件断点续传
        public ActionResult JavaUpLaod()
        {
            ViewData["fileTypeName"] = Constant.FILE_TYPE_NAME;
            ViewData["fileExt"] = Constant.JAVA_UPLOAD_EXT;
            ViewData["javaUploadUrl"] = Constant.JAVA_UPLOAD_URL;
            ViewData["fileSize"] = Constant.JAVA_UPLOAD_SIZE;
            ViewData["javaUploadSize"] = Constant.JAVA_UPLOAD_SIZE * 1024 * 1024 * 1024;
            ViewData["UserID"] = CommonUtil.GetSession(Session, "id");
            return View();
        }

        /**保存上传数据**/
        [HttpPost]
        public void JavaUpLoadSubmit(FormCollection form)
        {
            bool isSuccess = false;
            try
            {
                JavaUploadModel model = new JavaUploadModel();
                model.ID = Guid.NewGuid().ToString();
                model.Name = form["resourceName"];
                model.TypeID = form["typeId"];
                model.Introduction = form["introduction"];
                #region 标签拼接
                string label = "";
                string[] labels = form.GetValues("savelabel");
                if (labels != null)
                {
                    for (int i = 0; i < labels.Length; i++)
                    {
                        label = label + "," + labels[i];
                    }
                }
                if (!string.IsNullOrEmpty(label))
                {
                    label = label.Substring(1, label.Length - 1);
                }
                #endregion
                model.Label = label;
                model.Note = form["note"];
                model.CreateID = CommonUtil.GetSession(Session, "id");
                model.FileName = form.GetValues("uploadFileName");
                model.Author = form["author"];
                model.ActiveTimeStart = form["startDate"];
                model.ActiveTimeEnd = form["endDate"];
                model.FileUrl = form.GetValues("uploadFilePath");
                model.isForever = form["isForever"];
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    Resource res = new Resource();
                    isSuccess = res.JavaUploadSaveData(model, db);
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Upload", "JavaUpLoadSubmit", "Error! " + ex.ToString());
            }
            finally
            {
                HttpContext.Response.Write(isSuccess.ToString().ToLower());
            }
        }
        #endregion

        #region 版本对比
        /**根据最新的版本ID对比之前的版本**/
        public ActionResult ContrastFile(string Aid)
        {
            T_Res_Appendix model;
            List<VersionModel> VersionList = new List<VersionModel>();
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                model = res.GetVersionContrast(db, Aid, ref VersionList);
            }

            if (model != null)
            {
                string sourcePath = model.FILE_URL;
                string readPath = model.READ_URL;
                int pageCount = model.PAGECOUNT;
                if (!System.IO.File.Exists(sourcePath))
                {
                    ViewData["MSG"] = "文件不存在！";
                }
                else if (!System.IO.File.Exists(readPath) && !Directory.Exists(readPath))
                {
                    ViewData["MSG"] = "文件未处理好，请等待！";
                }
                else
                {
                    //返回在线阅读的路径
                    string readFile = StringUtil.ObjToString(readPath).Replace("\\", "/");
                    string[] path = readFile.Split('/');
                    readFile = Constant.SWF_ADDRESS + path[path.Length - 2] + "/" + path[path.Length - 1];

                    ViewData["MSG"] = "OK";
                    ViewData["fileName"] = Path.GetFileNameWithoutExtension(model.FILE_NAME);
                    if (model.TYPE_FLG == "0")
                    {
                        ViewData["extName"] = ".swf";
                    }
                    else if (model.TYPE_FLG == "1")
                    {
                        ViewData["extName"] = ".flv";
                    }
                    else if (model.TYPE_FLG == "2")
                    {
                        ViewData["extName"] = ".jpg";
                    }
                    else
                    {
                        ViewData["extName"] = "";
                    }
                    ViewData["fileUrl"] = sourcePath;           //源文件地址
                    ViewData["readUrl"] = readFile;             //转换文件地址
                    ViewData["pageCount"] = pageCount;          //文档页数
                    ViewData["VersionList"] = VersionList;      //旧版本
                }
            }
            else
            {
                ViewData["MSG"] = "文件不存在！";
            }
            return View();
        }

        [HttpPost]
        public void VersionInfo(string Oid)
        {
            Hashtable ht = new Hashtable();
            T_Res_Appendix model = null;
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                model = db.T_Res_Appendix.First(t => t.ID == Oid);
            }

            if (model != null)
            {
                string ext = "";
                if (model.TYPE_FLG == "0")
                {
                    ext = ".swf";
                }
                else if (model.TYPE_FLG == "1")
                {
                    ext = ".flv";
                }
                else if (model.TYPE_FLG == "2")
                {
                    ext = ".jpg";
                }
                else
                {
                    ext = "";
                }
                string sourcePath = model.FILE_URL;
                string readPath = model.READ_URL;
                int pageCount = model.PAGECOUNT;
                if (!System.IO.File.Exists(sourcePath))
                {
                    ht.Add("success", "false");
                    ht.Add("MSG", "文件不存在！");
                }
                else if (!System.IO.File.Exists(readPath) && !Directory.Exists(readPath))
                {
                    ht.Add("success", "false");
                    ht.Add("MSG", "文件未处理好，请等待！");
                }
                else
                {
                    string readFile = StringUtil.ObjToString(readPath).Replace("\\", "/");
                    string[] path = readFile.Split('/');
                    readFile = Constant.SWF_ADDRESS + path[path.Length - 2] + "/" + path[path.Length - 1];
                    ht.Add("success", "true");
                    ht.Add("ext", ext);
                    ht.Add("fileUrl", sourcePath);
                    ht.Add("readUrl", readFile);
                    ht.Add("pageCount", pageCount);
                }
            }
            else
            {
                ht.Add("success", "false");
                ht.Add("MSG", "文件不存在！");
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = Int32.MaxValue;
            String json = ser.Serialize(ht);
            HttpContext.Response.Write(json);
        }
        #endregion

        #region 删除评论
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id"></param>
        public void deleteScore(string id)
        {
            bool success = false;
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                string userId = CommonUtil.GetSession(Session, "id");
                success = res.deleteScore(id, db);
            }

            HttpContext.Response.Write(success.ToString().ToLower());
        }
        #endregion

        #region 删除提问
        /// <summary>
        /// 删除提问
        /// </summary>
        /// <param name="id"></param>
        public void deleteQuestion(string id)
        {
            bool success = false;
            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                Resource res = new Resource();
                string userId = CommonUtil.GetSession(Session, "id");
                success = res.deleteQuestion(id, db);
            }

            HttpContext.Response.Write(success.ToString().ToLower());
        }
        #endregion

        #region 统计页面初始化
        /// <summary>
        /// 统计页面初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            return View();
        }
        #endregion

        #region 统计报表
        /// <summary>
        /// 统计报表
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public ContentResult StatisticsList(string orderBy)
        {
            List<ResStatistics_Result> list = new List<ResStatistics_Result>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList rlist = new ResourceList();
                list = rlist.statisticsInfo(orderBy, db);
            }


            Hashtable ht = new Hashtable();
            if (list != null && list.Count > 0)
            {
                ht.Add("HaveData", "true");
                ht.Add("list", list);
            }
            else
            {
                ht.Add("HaveData", "false");
            }

            String json = JsonConvert.SerializeObject(ht);
            return Content(json);
        }
        #endregion

        #region 图表页面初始化
        /// <summary>
        /// 图表页面初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Chart()
        {
            return View();
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void ChartData(string type)
        {
            List<ResStatistics_Result> list = new List<ResStatistics_Result>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                ResourceList rlist = new ResourceList();
                list = rlist.statisticsInfo("", db);
            }

            Hashtable ht = new Hashtable();
            if (list != null && list.Count > 0)
            {
                string[] name = new string[list.Count];
                int[] data = new int[list.Count];

                for (int i = 0; i < list.Count; i++)
                {
                    name[i] = list[i].CREATE_NAME;

                    if (type == "total")
                    {
                        data[i] = Convert.ToInt32(list[i].allFile);
                    }
                    else if (type == "praisePre")
                    {
                        data[i] = Convert.ToInt32(list[i].PRAISE_PRE * 100);
                    }
                    else if (type == "viewNum")
                    {
                        data[i] = Convert.ToInt32(list[i].VIEW_NUM);
                    }
                    else if (type == "downloadNum")
                    {
                        data[i] = Convert.ToInt32(list[i].DOWNLOAD_NUM);
                    }
                }
                ht.Add("name", name);
                ht.Add("data", data);
            }

            String json = JsonConvert.SerializeObject(ht);
            HttpContext.Response.Write(json);
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void checkFileIsReady(string resId)
        {
            Boolean isReady = true;

            List<T_Res_Appendix> list = new List<T_Res_Appendix>();

            using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
            {
                var query = from t in db.T_Res_Appendix where t.RES_ID == resId select t;
                list = query.ToList();
            }

            Hashtable ht = new Hashtable();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T_Res_Appendix a = list[i];
                    if (a.TYPE_FLG != "3")
                    {
                        if (a.READ_URL == null && a.IMAGE == null)
                        {
                            isReady = false;
                            break;
                        }
                    }
                }
            }

            ht.Add("isReady", isReady.ToString().ToLower());

            String json = JsonConvert.SerializeObject(ht);
            HttpContext.Response.Write(json);
        }
        #endregion

        #region "无权限页面跳转"
        public ActionResult Error()
        {
            return View();
        }
        #endregion


        #region "CMS精品资源取得接口"
        /// <summary>
        /// CMS端取得精品资源
        /// </summary>
        /// <param name="top">前top条精品资源，默认5条</param>
        /// <returns>精品资源html</returns>
        public string CompetitiveResourceForCms(string top, string callback)
        {
            if (string.IsNullOrEmpty(top)) top = "5";

            string sql = "  SELECT top " + top + " a.*, CONVERT(CHAR(8), a.CREATETIME, 11) AS CreateDate  ";
            sql = sql + "     FROM T_Resource r  ";
            sql = sql + "    INNER JOIN T_Res_Appendix a  ";
            sql = sql + "       ON a.DEL_FLG = '0'  ";
            sql = sql + "      AND a.RES_ID = r.ID  ";
            sql = sql + "    WHERE r.EXCELLENT_FLG = '1'  ";
            sql = sql + "      AND r.DEL_FLG = '0'  ";
            sql = sql + "      AND r.SHARE_FLG = '1'  ";
            sql = sql + "      AND r.STATUS = '1'  ";
            sql = sql + "    ORDER BY r.CREATETIME DESC  ";

            DBOperation db = new DBOperation();
            DataSet ds = new DataSet();
            ds = db.GetDataSet(sql);

            string html = "";
            html = html + "<ul class=\"showlist tab01\">";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string ResoureUrl = ConfigurationManager.AppSettings["ResoureUrl"] + "/Mine/ViewOnline?";
                string appId = "";//附件ID
                string[] ArrFileName;//后缀
                string resId = "";//资源ID

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    appId = ds.Tables[0].Rows[i]["ID"].ToString();//附件ID
                    ArrFileName = ds.Tables[0].Rows[i]["FILE_NAME"].ToString().Split('.');//后缀
                    resId = ds.Tables[0].Rows[i]["RES_ID"].ToString();//资源ID

                    html = html + "    <li>";
                    html = html + "        <span class=\"s1\">" + ds.Tables[0].Rows[i]["CreateDate"].ToString() + "</span>";
                    html = html + "        <a target=\"_blank\" href=\"" + ResoureUrl + "appId=" + appId + "&ext=." + ArrFileName[1] + "&resId=" + resId + "\">";
                    html = html + "            <span class=\"iconfont s2\"></span>";
                    html = html + "" + ArrFileName[0];
                    html = html + "        </a>";
                    html = html + "    </li>";
                }
            }
            html = html + "</ul>";


         //   return "{\"result\":\"success\",\"html\":" + html + "\"}";
            return callback + "({\"result\":\"success\",\"html\":\"" + JsonHelper.String2Json(html) + "\"})";
        }
        #endregion
    }
}
