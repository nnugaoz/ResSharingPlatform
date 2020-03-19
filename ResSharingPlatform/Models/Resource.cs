using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.IO;
using ResSharingPlatform.Common;

namespace ResSharingPlatform.Models
{
    public class Resource
    {
        #region 保存资源信息
        /// <summary>
        /// 保存资源信息
        /// </summary>
        /// <param name="rm">资源信息</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool insertResource(ResourceModel rm, JSZX_ResourceEntities db)
        {
            try
            {
                T_Resource tr = new T_Resource();
                tr.ID = rm.Id;
                tr.NAME = rm.Name;
                tr.TYPE_ID = rm.TypeId;
                tr.INTRODUCTION = rm.Introduction;
                tr.LABEL = rm.Label;
                tr.NOTE = rm.Note;
                tr.STATUS = "0";
                tr.DEL_FLG = "0";
                tr.ORIGIN_FLG = "1";
                tr.SHARE_FLG = "1";
                tr.EXCELLENT_FLG = "0";
                tr.CREATEID = rm.UserId;
                tr.CREATETIME = DateTime.Now;
                tr.MODIFYID = rm.UserId;
                tr.MODIFYTIME = DateTime.Now;

                db.T_Resource.Add(tr);

                doAppendix(rm, db);

                db.SaveChanges();

                //上传资源时，可以获得积分
                PointRule pr = new PointRule();
                pr.AddPoint(tr.ID, tr.MODIFYID, "0");
                //上传资源时，可以获得积分

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "insertResource", ex.Message);
                return false;
            }
        }
        #endregion

        #region 保存附件信息
        /// <summary>
        /// 保存附件信息
        /// </summary>
        /// <param name="rm">附件信息</param>
        /// <param name="db"></param>
        private void doAppendix(ResourceModel rm, JSZX_ResourceEntities db)
        {
            string[] fileNames = rm.FileName;
            string[] saveNames = rm.SaveName;
            string[] authors = rm.Author;
            string[] activeStart = rm.ActiveTimeStart;
            string[] activeEnd = rm.ActiveTimeEnd;
            string[] uploadTime = rm.UploadTime;
            string[] fileType = rm.FileType;
            string[] fileUrl = rm.FileUrl;
            string[] dataType = rm.DataType;
            string[] isForever = rm.isForever;

            if (fileNames != null)
            {
                for (int i = 0; i < fileNames.Length; i++)
                {
                    string saveName = saveNames[i];

                    string ext = "";
                    string id = saveName;
                    if (fileNames[i] != null && fileNames[i].LastIndexOf('.') > 0)
                    {
                        id = saveName.Substring(0, saveName.LastIndexOf('.'));
                        ext = fileNames[i].Substring(fileNames[i].LastIndexOf('.'), fileNames[i].Length - fileNames[i].LastIndexOf('.')).ToLower();
                    }

                    AppendixModel am = null;
                    if (dataType[i] == "add")
                    {
                        am = new AppendixModel();

                        am.Id = id;
                        am.Res_Id = rm.Id;
                        am.FileName = fileNames[i];
                        am.SaveName = saveName;
                        am.Ext = ext;
                        am.Author = authors[i];
                        am.SetUploadTime(uploadTime[i]);
                        am.SetStartTime(activeStart[i]);
                        am.SetEndTime(activeEnd[i]);
                        am.FileUrl = fileUrl[i];
                        am.TypeFlg = fileType[i];
                        am.CreateId = rm.UserId;
                        am.setIsForever(isForever[i]);

                        insertAppendix(am, db);
                    }
                    else if (dataType[i] == "update")
                    {
                        am = new AppendixModel();
                        am.Id = id;
                        am.Author = authors[i];
                        am.SetStartTime(activeStart[i]);
                        am.SetEndTime(activeEnd[i]);
                        am.ModifyId = rm.UserId;
                        am.setIsForever(isForever[i]);

                        updateAppendix(am, db);
                    }
                    else if (dataType[i] == "delete")
                    {
                        am = new AppendixModel();
                        am.Id = id;
                        am.ModifyId = rm.UserId;
                        DeleteAppendix(am, db);
                    }
                    /**更新资源附件 2014-12-19 5920 **/
                    else //前台更新了附件 此时的dataType是旧的附件ID
                    {
                        string OldFileID = dataType[i];
                        /**逻辑删除旧的附件**/
                        T_Res_Appendix app = db.T_Res_Appendix.FirstOrDefault(t => t.ID == OldFileID);
                        app.DEL_FLG = "1";
                        app.MODIFYID = rm.UserId;
                        app.MODIFYTIME = DateTime.Now;
                        /**添加新的附件**/
                        am = new AppendixModel();
                        am.Id = id;
                        am.Res_Id = rm.Id;
                        am.FileName = fileNames[i];
                        am.SaveName = saveName;
                        am.Ext = ext;
                        am.SetUploadTime(uploadTime[i]);
                        am.Author = authors[i];
                        am.SetStartTime(activeStart[i]);
                        am.SetEndTime(activeEnd[i]);
                        am.FileUrl = fileUrl[i];
                        am.TypeFlg = fileType[i];
                        am.CreateId = rm.UserId;
                        am.setIsForever(isForever[i]);

                        insertAppendix(am, db);

                        /**记录下版本更新**/
                        T_Version model = new T_Version();
                        model.ID = Guid.NewGuid().ToString();
                        model.NID = id;
                        model.OID = OldFileID;
                        model.VERSION_NUM = GetVersionNum(db, OldFileID); //这边需要算出之前的所有版本
                        model.USERID = rm.UserId;
                        model.CreateTime = DateTime.Now;
                        db.T_Version.Add(model);
                    }
                }
            }

        }
        #endregion

        #region 更新资源信息
        /// <summary>
        /// 更新资源信息
        /// </summary>
        /// <param name="rm">资源信息</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool updateResource(ResourceModel rm, JSZX_ResourceEntities db)
        {
            try
            {
                T_Resource tr = db.T_Resource.First(t => t.ID == rm.Id);

                tr.NAME = rm.Name;
                tr.TYPE_ID = rm.TypeId;
                tr.INTRODUCTION = rm.Introduction;
                tr.STATUS = "0";
                tr.LABEL = rm.Label;
                tr.NOTE = rm.Note;
                tr.MODIFYID = rm.UserId;
                tr.MODIFYTIME = DateTime.Now;

                doAppendix(rm, db);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "updateResource", ex.Message);
                return false;
            }
        }
        #endregion

        #region 插入附件信息
        /// <summary>
        /// 插入附件信息
        /// </summary>
        /// <param name="rm">附件id</param>
        /// <param name="db"></param>
        private void insertAppendix(AppendixModel am, JSZX_ResourceEntities db)
        {
            T_Res_Appendix tra = new T_Res_Appendix();

            tra.ID = am.Id;
            tra.RES_ID = am.Res_Id;
            tra.FILE_NAME = am.FileName;
            tra.UPLOAD_TIME = am.GetUploadTime();
            tra.AUTHOR = am.Author;
            tra.ACTIVE_TIME_START = am.GetStartTime();
            tra.ACTIVE_TIME_END = am.GetEndTime();
            tra.IS_FOREVER = am.getIsForever();

            tra.CREATEID = am.CreateId;
            tra.CREATETIME = DateTime.Now;

            tra.DEL_FLG = "0";
            tra.FILE_URL = am.FileUrl;
            tra.TYPE_FLG = am.TypeFlg;

            db.T_Res_Appendix.Add(tra);

            T_ToChange change = new T_ToChange();

            change.ID = Guid.NewGuid().ToString();
            change.FileName = am.Id;
            change.SourceFilePath = am.FileUrl;
            change.TargetFilePath = Constant.DISK_ADDRESS + Constant.UPLOADDIRECTORY + "\\" + Constant.SWFDIRECTORY;

            string sql = "update JSZX_Resource.dbo.T_Res_Appendix set READ_URL=@TargetFilePath,[IMAGE]=@FileFirstImg,[PAGECOUNT]=@PageCount where ID='" + am.Id + "'";
            change.RetSql = sql;
            change.TaskTime = DateTime.Now;

            db.T_ToChange.Add(change);
        }
        #endregion

        #region 更新附件信息
        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="rm">附件id</param>
        /// <param name="db"></param>
        private void updateAppendix(AppendixModel am, JSZX_ResourceEntities db)
        {
            T_Res_Appendix app = db.T_Res_Appendix.FirstOrDefault(t => t.ID == am.Id);
            app.AUTHOR = am.Author;
            app.ACTIVE_TIME_START = am.GetStartTime();
            app.ACTIVE_TIME_END = am.GetEndTime();
            app.MODIFYID = am.ModifyId;
            app.MODIFYTIME = DateTime.Now;
            app.IS_FOREVER = am.getIsForever();
        }
        #endregion

        #region 删除附件信息
        /// <summary>
        /// 删除附件信息
        /// </summary>
        /// <param name="rm">附件id</param>
        /// <param name="db"></param>
        private void DeleteAppendix(AppendixModel am, JSZX_ResourceEntities db)
        {
            T_Res_Appendix app = db.T_Res_Appendix.FirstOrDefault(t => t.ID == am.Id);

            app.DEL_FLG = "1";
            app.MODIFYID = am.ModifyId;
            app.MODIFYTIME = DateTime.Now;

            //删除评价信息
            var query = from t in db.T_Res_Score where t.APPEND_ID == am.Id select t;
            List<T_Res_Score> scoList = query.ToList();
            if (scoList != null)
            {
                foreach (T_Res_Score item in scoList)
                {
                    db.T_Res_Score.Remove(item);
                    db.SaveChanges();
                }
            }

            //删除问题信息
            var query2 = from t in db.T_Res_Qa where t.APPEND_ID == am.Id select t;
            List<T_Res_Qa> qaList = query2.ToList();
            if (qaList != null)
            {
                foreach (T_Res_Qa item in qaList)
                {
                    db.T_Res_Qa.Remove(item);
                    db.SaveChanges();
                }
            }

            //删除评价结果表
            var query3 = from t in db.T_Res_Result where t.APPEND_ID == am.Id select t;
            List<T_Res_Result> resList = query3.ToList();
            if (resList != null)
            {
                foreach (T_Res_Result item in resList)
                {
                    db.T_Res_Result.Remove(item);
                    db.SaveChanges();
                }
            }
        }
        #endregion

        #region 新建分类
        /// <summary>
        /// 新建分类
        /// </summary>
        /// <param name="type">分类名</param>
        /// <param name="father">分类名</param>
        /// <param name="userId">用户</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool insertResType(string type, string father, string userId, JSZX_ResourceEntities db)
        {
            try
            {
                T_Res_Type rt = new T_Res_Type();
                rt.ID = Guid.NewGuid().ToString();
                if (father != "-1")
                {
                    rt.BELONG_ID = father;
                }

                rt.NAME = type;
                rt.CREATEID = userId;
                rt.CREATETIME = DateTime.Now;

                db.T_Res_Type.Add(rt);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "insertResType", ex.Message);
                return false;
            }
        }
        #endregion

        #region 删除资源
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool deleteResource(string id, string userId, JSZX_ResourceEntities db)
        {
            try
            {
                T_Resource tr = db.T_Resource.First(t => t.ID == id);

                tr.DEL_FLG = "1";
                tr.MODIFYID = userId;
                tr.MODIFYTIME = DateTime.Now;

                List<T_Res_Appendix> list = db.T_Res_Appendix.Where(t => t.RES_ID == id).ToList();

                if (list != null)
                {
                    foreach (T_Res_Appendix item in list)
                    {
                        item.DEL_FLG = "1";
                        item.MODIFYID = userId;
                        item.MODIFYTIME = DateTime.Now;
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "deleteResource", ex.Message);
                return false;
            }
        }
        #endregion

        #region 批量删除资源
        /// <summary>
        /// 批量删除资源
        /// </summary>
        /// <param name="rm">资源ID</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool deleteResource2(ResourceModel rm, JSZX_ResourceEntities db)
        {
            string[] ids = rm.Ids;
            string[] isSelect = rm.isSelect;
            string userId = rm.UserId;

            try
            {
                if (ids != null)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (isSelect[i] == "1")
                        {
                            string id = ids[i];
                            T_Resource tr = db.T_Resource.First(t => t.ID == id);

                            tr.DEL_FLG = "1";
                            tr.MODIFYID = userId;
                            tr.MODIFYTIME = DateTime.Now;

                            List<T_Res_Appendix> list = db.T_Res_Appendix.Where(t => t.RES_ID == id).ToList();

                            if (list != null)
                            {
                                foreach (T_Res_Appendix item in list)
                                {
                                    item.DEL_FLG = "1";
                                    item.MODIFYID = userId;
                                    item.MODIFYTIME = DateTime.Now;
                                }
                            }
                        }
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "deleteResource2", ex.Message);
                return false;
            }
        }
        #endregion

        #region 回复问题
        /// <summary>
        /// 回复问题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="answer"></param>
        /// <param name="userId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool updateResQa(string id, string answer, string userId, JSZX_ResourceEntities db)
        {
            try
            {
                T_Res_Qa qa = db.T_Res_Qa.First(t => t.ID == id);

                qa.ANSWER = answer;
                qa.MODIFYID = userId;
                qa.MODIFYTIME = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "updateResQa", ex.Message);
                return false;
            }

        }
        #endregion

        #region 审核资源
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rm"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool examineResource(ResourceModel rm, JSZX_ResourceEntities db)
        {
            try
            {
                T_Resource tr = db.T_Resource.First(t => t.ID == rm.Id);

                tr.TYPE_ID = rm.TypeId;
                tr.INTRODUCTION = rm.Introduction;
                tr.LABEL = rm.Label;
                tr.STATUS = rm.Status;
                tr.EXCELLENT_FLG = rm.Excellent_Flg;
                tr.MODIFYID = rm.UserId;
                tr.MODIFYTIME = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "examineResource", ex.Message);
                return false;
            }
        }
        #endregion

        #region 批量审核资源
        /// <summary>
        /// 批量审核资源
        /// </summary>
        /// <param name="rm"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool examineResource2(ResourceModel rm, JSZX_ResourceEntities db)
        {
            string[] ids = rm.Ids;
            string[] isSelect = rm.isSelect;
            string status = rm.Status;
            string excellentFlg = rm.Excellent_Flg;
            string userId = rm.UserId;

            try
            {
                if (ids != null)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (isSelect[i] == "1")
                        {
                            string id = ids[i];
                            T_Resource tr = db.T_Resource.First(t => t.ID == id);

                            tr.STATUS = status;
                            tr.EXCELLENT_FLG = excellentFlg;
                            tr.MODIFYID = userId;
                            tr.MODIFYTIME = DateTime.Now;
                        }
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "examineResource2", ex.Message);
                return false;
            }
        }
        #endregion

        #region 记录阅读数
        /// <summary>
        /// 记录阅读数
        /// </summary>
        /// <param name="id">资源id</param>
        /// <param name="appId">附件id</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool AddViewNum(string id, string appId, string userId, JSZX_ResourceEntities db)
        {
            T_Res_Result result = null;
            try
            {
                //var query = from t in db.T_Res_Result
                //            where t.RES_ID == id && t.APPEND_ID == appId
                //            select t;
                //资源ID不作为条件检索数据，附件ID唯一，才是关键字
                var query = from t in db.T_Res_Result
                            where t.APPEND_ID == appId
                            select t;

                List<T_Res_Result> list = query.ToList();

                if (list == null || list.Count == 0)
                {
                    result = new T_Res_Result();
                    result.ID = Guid.NewGuid().ToString();
                    result.RES_ID = id;
                    result.APPEND_ID = appId;
                    result.PAGE_VIEW_NUM = 1;
                    result.CREATEID = userId;
                    result.CREATETIME = DateTime.Now;

                    db.T_Res_Result.Add(result);
                    db.SaveChanges();
                }
                else
                {
                    //result = db.T_Res_Result.First(t => t.APPEND_ID == appId && t.RES_ID == id);
                    //资源ID不作为条件检索数据，附件ID唯一，才是关键字
                    result = db.T_Res_Result.First(t => t.APPEND_ID == appId);

                    int viewNum = Convert.ToInt32(result.PAGE_VIEW_NUM);
                    viewNum = viewNum + 1;

                    result.PAGE_VIEW_NUM = viewNum;
                    result.MODIFYID = userId;
                    result.MODIFYTIME = DateTime.Now;

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "AddViewNum", ex.Message);
                return false;
            }
        }
        #endregion

        #region 记录下载数
        /// <summary>
        /// 记录下载数
        /// </summary>
        /// <param name="id">资源id</param>
        /// <param name="appId">附件id</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool AddDownLoadNum(string id, string appId, string userId, JSZX_ResourceEntities db)
        {
            T_Res_Result result = null;
            try
            {
                //var query = from t in db.T_Res_Result
                //            where t.RES_ID == id && t.APPEND_ID == appId
                //            select t;
                //资源ID不作为条件检索数据，附件ID唯一，才是关键字
                var query = from t in db.T_Res_Result
                            where t.APPEND_ID == appId
                            select t;

                List<T_Res_Result> list = query.ToList();

                if (list == null || list.Count == 0)
                {
                    result = new T_Res_Result();
                    result.ID = Guid.NewGuid().ToString();
                    result.RES_ID = id;
                    result.APPEND_ID = appId;
                    result.DOWNLOAD_NUM = 1;
                    result.CREATEID = userId;
                    result.CREATETIME = DateTime.Now;

                    db.T_Res_Result.Add(result);
                    db.SaveChanges();
                }
                else
                {
                    //result = db.T_Res_Result.First(t => t.APPEND_ID == appId && t.RES_ID == id);
                    //资源ID不作为条件检索数据，附件ID唯一，才是关键字
                    result = db.T_Res_Result.First(t => t.APPEND_ID == appId);

                    int downloadNum = Convert.ToInt32(result.DOWNLOAD_NUM);
                    downloadNum = downloadNum + 1;

                    result.DOWNLOAD_NUM = downloadNum;
                    result.MODIFYID = userId;
                    result.MODIFYTIME = DateTime.Now;

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "AddDownLoadNum", ex.Message);
                return false;
            }
        }
        #endregion

        #region 保存评价
        /// <summary>
        /// 保存评价
        /// </summary>
        /// <param name="rm">评价信息</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool insertScore(ResourceModel rm, JSZX_ResourceEntities db)
        {
            try
            {
                T_Res_Score score = new T_Res_Score();
                score.ID = Guid.NewGuid().ToString();
                score.RES_ID = rm.Id;
                score.APPEND_ID = rm.AppId;
                score.GRADE = Convert.ToDecimal(rm.Grade);
                score.REVIEW = rm.Review;
                score.CREATEID = rm.UserId;
                score.CREATETIME = DateTime.Now;

                db.T_Res_Score.Add(score);

                AddReviewNum(rm.Id, rm.AppId, Convert.ToDouble(rm.Grade), rm.UserId, db);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "insertScore", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 是否已评价
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="appId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean IsExistScore(string resId, string appId, string userId)
        {
            try
            {
                int count = 0;
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var query = from t in db.T_Res_Score select t;
                    //query = query.Where(t => t.RES_ID.Equals(resId));//资源ID不作为条件检索数据
                    query = query.Where(t => t.APPEND_ID.Equals(appId));
                    query = query.Where(t => t.CREATEID.Equals(userId));
                    count = query.Count();
                }
                return count == 0 ? false : true;
            }
            catch (Exception e)
            {
                clsLog.ErrorLog("Resource", "IsExistScore", "Error! " + e.Message);
                return false;
            }
        }
        #endregion

        #region 记录评价数
        /// <summary>
        /// 记录评价数
        /// </summary>
        /// <param name="id">资源id</param>
        /// <param name="appId">附件id</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool AddReviewNum(string id, string appId, double grade, string userId, JSZX_ResourceEntities db)
        {
            T_Res_Result result = null;
            try
            {
                var query = from t in db.T_Res_Result
                            where t.RES_ID == id && t.APPEND_ID == appId
                            select t;

                List<T_Res_Result> list = query.ToList();

                if (list == null || list.Count == 0)
                {
                    result = new T_Res_Result();
                    result.ID = Guid.NewGuid().ToString();
                    result.RES_ID = id;
                    result.APPEND_ID = appId;
                    result.REVIEW_NUM = 1;
                    result.CREATEID = userId;
                    result.CREATETIME = DateTime.Now;

                    if (grade >= 4)
                    {
                        result.PRAISE_PRE = 1;
                    }
                    else
                    {
                        result.PRAISE_PRE = 0;
                    }

                    db.T_Res_Result.Add(result);
                    //db.SaveChanges();
                }
                else
                {
                    result = db.T_Res_Result.First(t => t.APPEND_ID == appId && t.RES_ID == id);

                    int reviewNum = 0;
                    if (result.REVIEW_NUM != null)
                    {
                        reviewNum = Convert.ToInt32(result.REVIEW_NUM);
                    }
                    reviewNum = reviewNum + 1;

                    int goodGrade = db.T_Res_Score.Count(t => t.RES_ID == id && t.APPEND_ID == appId && t.GRADE >= 4);
                    if (grade >= 4)
                    {
                        goodGrade = goodGrade + 1;
                    }

                    result.REVIEW_NUM = reviewNum;
                    result.PRAISE_PRE = Convert.ToDecimal(Math.Round(Convert.ToDouble(goodGrade) / Convert.ToDouble(reviewNum), 2));
                    result.MODIFYID = userId;
                    result.MODIFYTIME = DateTime.Now;

                    //db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "AddReviewNum", ex.Message);
                return false;
            }
        }
        #endregion

        #region 保存提问
        /// <summary>
        /// 保存提问
        /// </summary>
        /// <param name="rm">提问信息</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool insertQa(ResourceModel rm, JSZX_ResourceEntities db)
        {
            try
            {
                T_Res_Qa qa = new T_Res_Qa();
                qa.ID = Guid.NewGuid().ToString();
                qa.RES_ID = rm.Id;
                qa.APPEND_ID = rm.AppId;
                qa.QUESTION = rm.Question;
                qa.CREATEID = rm.UserId;
                qa.CREATETIME = DateTime.Now;

                db.T_Res_Qa.Add(qa);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "insertQa", ex.Message);
                return false;
            }
        }
        #endregion

        #region 大文件上传数据保存
        /// <summary>
        /// 大文件上传数据保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Boolean JavaUploadSaveData(JavaUploadModel model, JSZX_ResourceEntities db)
        {
            try
            {
                #region 资源
                T_Resource rModel = new T_Resource();
                rModel.ID = model.ID;
                rModel.NAME = model.Name;
                rModel.TYPE_ID = model.TypeID;
                rModel.INTRODUCTION = model.Introduction;
                rModel.LABEL = model.Label;
                rModel.NOTE = model.Note;
                rModel.STATUS = "0";
                rModel.DEL_FLG = "0";
                rModel.ORIGIN_FLG = "1";
                rModel.SHARE_FLG = "1";
                rModel.EXCELLENT_FLG = "0";
                rModel.CREATEID = model.CreateID;
                rModel.CREATETIME = DateTime.Now;
                db.T_Resource.Add(rModel);
                #endregion

                #region 附件
                if (model.FileUrl != null && model.FileUrl.Length > 0)
                {
                    #region 循环插入附件
                    for (int i = 0; i < model.FileUrl.Length; i++)
                    {
                        string path = model.FileUrl[i];
                        string fileType = Constant.GetFileType(System.IO.Path.GetExtension(path));
                        string fileID = System.IO.Path.GetFileNameWithoutExtension(path);

                        T_Res_Appendix aModel = new T_Res_Appendix();
                        aModel.ID = fileID;
                        aModel.RES_ID = model.ID;
                        aModel.FILE_NAME = model.FileName[i];
                        aModel.UPLOAD_TIME = DateTime.Now;
                        aModel.AUTHOR = model.Author;
                        aModel.ACTIVE_TIME_START = StringToDateTime(model.ActiveTimeStart);
                        aModel.ACTIVE_TIME_END = StringToDateTime(model.ActiveTimeEnd);
                        aModel.CREATEID = model.CreateID;
                        aModel.CREATETIME = DateTime.Now;
                        aModel.DEL_FLG = "0";
                        aModel.FILE_URL = path;
                        aModel.TYPE_FLG = fileType;
                        aModel.IS_FOREVER = model.isForever == "1" ? true : false;
                        db.T_Res_Appendix.Add(aModel);

                        #region 转换任务
                        T_ToChange change = new T_ToChange();
                        change.ID = Guid.NewGuid().ToString();
                        change.FileName = fileID;
                        change.SourceFilePath = path;
                        change.TargetFilePath = Constant.DISK_ADDRESS + Constant.UPLOADDIRECTORY + "\\" + Constant.SWFDIRECTORY;
                        string sql = "update JSZX_Resource.dbo.T_Res_Appendix set READ_URL=@TargetFilePath,[IMAGE]=@FileFirstImg,[PAGECOUNT]=@PageCount where ID='" + fileID + "'";
                        change.RetSql = sql;
                        change.TaskTime = DateTime.Now;
                        db.T_ToChange.Add(change);
                        #endregion
                    }
                    #endregion
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    clsLog.ErrorLog("Resource", "ShareResource", "Error! 缺少必要的附件！");
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "JavaUploadSaveData", "Error! " + ex.Message);
                return false;
            }
        }
        /**字符串转换成时间格式**/
        private DateTime StringToDateTime(string value)
        {
            DateTime dt = DateTime.Now;
            try
            {
                dt = Convert.ToDateTime(value);
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "StringToDateTime", "Error! DateStr= " + value + "; " + ex.ToString());
            }
            return dt;
        }
        #endregion

        #region 版本更新
        /**获取新的版本号**/
        public int GetVersionNum(JSZX_ResourceEntities db, string AId)
        {
            List<string> OidArr = new List<string>();
            GetVersion(db, AId, ref OidArr);
            return OidArr.Count + 1;
        }

        /**根据资源附件ID获取之前所有的版本ID**/
        private void GetVersion(JSZX_ResourceEntities db, string Aid, ref List<string> OidArr)
        {
            try
            {
                var query = from t in db.T_Version select t;
                query = query.Where(t => t.NID == Aid);
                query = query.OrderByDescending(t => t.CreateTime);
                List<T_Version> models = query.ToList();
                if (models != null && models.Count > 0)
                {
                    foreach (T_Version model in models)
                    {
                        OidArr.Add(model.OID);
                        GetVersion(db, model.OID, ref OidArr);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /**获取最新版本信息和之前版本号**/
        public T_Res_Appendix GetVersionContrast(JSZX_ResourceEntities db, string Aid, ref List<VersionModel> VersionList)
        {
            try
            {
                T_Res_Appendix model = db.T_Res_Appendix.First(t => t.ID == Aid);
                GetVersion(db, Aid, ref VersionList);
                return model;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "GetVersionContrast", "Error! " + ex.Message);
                return null;
            }
        }
        /**根据资源附件ID获取之前所有的版本ID**/
        private void GetVersion(JSZX_ResourceEntities db, string Aid, ref List<VersionModel> VersionList)
        {
            try
            {
                var query = from t in db.V_Version select t;
                query = query.Where(t => t.NID == Aid);
                query = query.OrderByDescending(t => t.CreateTime);
                List<V_Version> models = query.ToList();
                if (models != null && models.Count > 0)
                {
                    foreach (V_Version model in models)
                    {
                        VersionModel vModel = new VersionModel();
                        vModel.ID = model.OID;
                        vModel.Name = model.FileName;
                        vModel.Version = model.VERSION_NUM;
                        vModel.DtTime = model.CreateTime;
                        VersionList.Add(vModel);
                        GetVersion(db, model.OID, ref VersionList);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        #region 获取附件原名称
        public string GetFileOriginalName(string fileName)
        {
            try
            {
                string fID = Path.GetFileNameWithoutExtension(fileName);
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    T_Res_Appendix model = db.T_Res_Appendix.First(t => t.ID.Equals(fID));
                    fileName = model.FILE_NAME;
                }
            }
            catch (Exception e)
            {
                clsLog.ErrorLog("Resource", "GetFileOriginalName", "Error! " + e.Message);
            }
            return fileName;
        }
        #endregion

        #region 删除评论
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Boolean deleteScore(string id, JSZX_ResourceEntities db)
        {
            try
            {
                T_Res_Score qa = db.T_Res_Score.First(t => t.ID == id);

                db.T_Res_Score.Remove(qa);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "deleteScore", ex.Message);
                return false;
            }
        }
        #endregion

        #region 删除提问
        /// <summary>
        /// 删除提问
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Boolean deleteQuestion(string id, JSZX_ResourceEntities db)
        {
            try
            {
                T_Res_Qa qa = db.T_Res_Qa.First(t => t.ID == id);

                db.T_Res_Qa.Remove(qa);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "deleteQuestion", ex.Message);
                return false;
            }
        }
        #endregion
    }
}