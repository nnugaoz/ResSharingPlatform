using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using ResSharingPlatform.Common;
using System.Data;

namespace ResSharingPlatform.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceList
    {
        private string clsName = "ResourceList";

        #region 资源记录总数
        /// <summary>
        /// 资源记录总数
        /// </summary>
        /// <param name="res_type"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int GetResourceListCount(string res_type,JSZX_ResourceEntities db) {
            int  listcount = 0;

            var query = from t in db.T_Resource 
                        select t;
            if (!string.IsNullOrEmpty(res_type))
            {
                query = query.Where(t => t.TYPE_ID.Contains(res_type));
            }
            //审核状态：通过（1）
            query = query.Where(t => t.STATUS.Contains("1"));
            //删除状态:未删除（0）
            query = query.Where(t => t.DEL_FLG.Contains("0"));
            //公开状态：公开（1）
            query = query.Where(t => t.SHARE_FLG.Contains("1"));

            listcount = query.Count();
           
            return listcount;
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="res_type"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<T_Resource> GetResourceList(string res_type, JSZX_ResourceEntities db)
        {
            List<T_Resource> rlt = null;
            var query = from t in db.T_Resource
                        select t;
            if (!string.IsNullOrEmpty(res_type))
            {
                query = query.Where(t => t.TYPE_ID.Contains(res_type));
            }
            //审核状态：通过（1）
            query = query.Where(t => t.STATUS.Contains("1"));
            //删除状态:未删除（0）
            query = query.Where(t => t.DEL_FLG.Contains("0"));
            //公开状态：公开（1）
            query = query.Where(t => t.SHARE_FLG.Contains("1"));
            int a = query.Count();

            query = query.OrderByDescending((t => t.CREATETIME));
            rlt = query.ToList();
            DateTime dt = DateTime.Now.AddHours(0); //缓存6小时
            System.Web.HttpRuntime.Cache.Insert("T_Resource", rlt, null, dt, System.Web.Caching.Cache.NoSlidingExpiration);
           
            return rlt;
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="res_type"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<View_Resource> GetResourceListByView(string res_type, int pagesize, int pagecurrent, string orderBy, JSZX_ResourceEntities db)
        {
            List<View_Resource> rlt = null;
            return rlt;
        }
        #endregion

        #region 获取权限
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public AthorityModels GetAthority(string usrId,string pageId)
        {
            List<SPT_Get_Role_Button_Result> list = new List<SPT_Get_Role_Button_Result>();
            MenuOperate me = new MenuOperate();

            if (!string.IsNullOrEmpty(usrId))
            {
                list = me.GetButton(usrId, pageId);
            }

            AthorityModels am = new AthorityModels();

            am.AddAthority = "0";          //添加权限
            am.EditAthority = "0";         //编辑权限
            am.DelAthority = "0";         //删除权限
            am.ExamineAthority = "0";      //审核权限
            am.DownLoadAthority = "0";      //下载权限

            if (list != null && list.Count > 0)
            {
                foreach (SPT_Get_Role_Button_Result button in list)
                {
                    string buttonId = "";
                    if (button.Menu_Code != null)
                    {
                        buttonId = button.Menu_Code.Substring(6, 2);
                    }

                    switch (buttonId)
                    {
                        case "01":
                            am.ExamineAthority = "1";
                            break;
                        case "02":
                            am.EditAthority = "1";
                            break;
                        case "03":
                            am.DelAthority = "1";
                            break;
                        case "04":
                            am.AddAthority = "1";
                            break;
                        case "05":
                            am.DownLoadAthority = "1";
                            break;
                        default:
                            break;
                    }
                }
            }

            return am;
        }
        #endregion

        #region 获取资源分类
        /// <summary>
        /// 获取资源分类
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<ComboTreeModels> GetResTypeList(string type, JSZX_ResourceEntities db)
        {
            List<T_Res_Type> list = new List<T_Res_Type>();

            var query = from t in db.T_Res_Type orderby t.BELONG_ID, t.ID select t;
            query = query.OrderBy(t => t.CREATETIME);
            list = query.ToList();

            List<ComboTreeModels> treeList = new List<ComboTreeModels>();

            if (list != null && list.Count > 0)
            {
                ReturnTreeList(list, null, 1, ref treeList);
                
                if (type == "combox")
                {
                    ComboTreeModels tree1 = new ComboTreeModels();
                    tree1.id = "";
                    tree1.text = "";
                    tree1.belong = "";
                    treeList.Insert(0, tree1);
                }
            }

            return treeList;
        }

        private void ReturnTreeList(List<T_Res_Type> list, string parentID,int num, ref List<ComboTreeModels> treeList)
        {
            List<T_Res_Type> t = FindById(list, parentID);
            if (t.Count > 0)
            {
                foreach (T_Res_Type resType in t)
                {
                    ComboTreeModels tree = new ComboTreeModels();
                    tree.id = resType.ID;
                    tree.text = resType.NAME;
                    tree.belong = num.ToString();
                    List<ComboTreeModels> newTreeList = new List<ComboTreeModels>();
                    tree.children = newTreeList;
                    treeList.Add(tree);
                    ReturnTreeList(list, resType.ID, num + 1, ref newTreeList);
                }
            }
            else
            {
                return;
            }
        }

        private List<T_Res_Type> FindById(List<T_Res_Type> list,string parentID)
        {
            List<T_Res_Type> retlist = new List<T_Res_Type>();
            if (list != null && list.Count > 0)
            {
                foreach (T_Res_Type t in list)
                {
                    if (parentID == null || parentID == "")
                    {
                        if (t.BELONG_ID == null || t.BELONG_ID == "")
                        {
                            retlist.Add(t);
                        }
                    }
                    else
                    {
                        if (t.BELONG_ID == parentID)
                        {
                            retlist.Add(t);
                        }
                    }
                }
            }
            return retlist;
        }
        #endregion

        #region 后台管理 资源总数
        /// <summary>
        /// 后台管理 资源总数
        /// </summary>
        /// <param name="resource">资源名称</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <param name="orderBy">排序</param>
        /// <param name="delFlg">删除标志</param>
        /// <param name="origin">来源标志</param>
        /// <param name="db"></param>
        /// <returns>资源总数</returns>
        public int GetResourceListSize(string SchoolId,string userId,string dataRole,string resource, string type, string uploadTime, string label, string orderBy,string delFlg, string origin ,string topage,JSZX_ResourceEntities db)
        {
            var query = from t in db.View_Resource select t;

            query = getLinkQ(query,SchoolId, userId, dataRole, resource, type, uploadTime, label, delFlg, origin, topage);

            return query.Count();
        }
        #endregion

        #region 获取LinkQ语句 (检索)

        /// <summary>
        /// 获取LinkQ语句
        /// </summary>
        /// <param name="query">LinkQ</param>
        /// <param name="resource">资源名称</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <param name="delFlg">删除标志</param>
        /// <param name="origin">来源标志</param>
        /// <returns>LinkQ</returns>
        private IQueryable<View_Resource> getLinkQ(IQueryable<View_Resource> query,string SchoolId, string userId,string dataRole, string resource, string type, string uploadTime, string label, string delFlg, string origin, string topage)
        {
            query = query.Where(t => t.SchoolId.Equals(SchoolId));
            if (dataRole == "1")
            {
                query = query.Where(t => t.CREATEID.Equals(userId));
            }

            //删除状态:未删除（0）
            if (!string.IsNullOrEmpty(delFlg))
            {
                query = query.Where(t => t.DEL_FLG.Equals(delFlg));
            }

            // 资源名称
            if (!string.IsNullOrEmpty(resource))
            {
                query = query.Where(t => t.NAME.Contains(resource));
            }

            // 资源分类
            if (!string.IsNullOrEmpty(type))
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    string typeStr = GetTypeCondition(db, type);
                    query = query.Where(t => typeStr.Contains(t.TYPE_ID));
                }
            }

            // 上传时间
            if (!string.IsNullOrEmpty(uploadTime))
            {
                DateTime dtStart = Convert.ToDateTime(uploadTime);
                query = query.Where(t => t.UPLOAD_TIME >= dtStart);

                DateTime dtEnd = Convert.ToDateTime(uploadTime + " 23:59:59");
                query = query.Where(t => t.UPLOAD_TIME <= dtEnd);
            }

            // 标签
            if (!string.IsNullOrEmpty(label))
            {
                label = label.Replace(" ", ",");
                query = query.Where(t => t.LABEL.Contains(label));
            }

            //来源
            if (!string.IsNullOrEmpty(origin))
            {
                query = query.Where(t => t.ORIGIN_FLG.Contains(origin));
            }

            if (!string.IsNullOrEmpty(topage))
            {
                if (topage == "pending")
                {
                    query = query.Where(t => t.STATUS == "0");
                }
                else if (topage == "checked")
                {
                    query = query.Where(t => t.STATUS == "1");
                }
                else if (topage == "unqualified")
                {
                    query = query.Where(t => t.STATUS == "2");
                }
            }
            return query;
        }
        #endregion

        #region 获取所有资源信息
        /// <summary>
        /// 获取所有资源信息
        /// </summary>
        /// <param name="resource">资源名称</param>
        /// <param name="type">资源分类</param>
        /// <param name="uploadTime">上传时间</param>
        /// <param name="label">标签</param>
        /// <param name="orderBy">排序</param>
        /// <param name="delFlg">删除标志</param>
        /// <param name="origin">来源标志</param>
        /// <param name="pagesize">每页显示的条目数</param>
        /// <param name="pagecurrent">当前被选中的页</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<View_Resource> GetResourceList(string SchoolId,string userId,string dataRole,string resource, string type, string uploadTime, string label, string orderBy,string delFlg, string origin, int pagesize, int pagecurrent,string topage, JSZX_ResourceEntities db)
        {
            List<View_Resource> list = new List<View_Resource>();

            var query = from t in db.View_Resource select t;

            query = getLinkQ(query,SchoolId, userId, dataRole, resource, type, uploadTime, label, delFlg, origin, topage);

            //排序
            if (orderBy == "1")
            {
                //评价次数
                query = query.OrderByDescending(t => t.REVIEW_NUM).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "2")
            {
                //最受好评
                query = query.OrderByDescending(t => t.PRAISE_PRE).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "3")
            {
                //最多浏览
                query = query.OrderByDescending(t => t.PAGE_VIEW_NUM).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "4")
            {
                //最多下载
                query = query.OrderByDescending(t => t.DOWNLOAD_NUM).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "5")
            {
                //最新上传
                query = query.OrderByDescending(t => t.UPLOAD_TIME).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else
            {
                query = query.OrderByDescending(t => t.UPLOAD_TIME).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            
            list = query.ToList();

            return list;
        }
        #endregion

        #region 获取资源详细信息
        /// <summary>
        /// 获取资源详细信息
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>资源详细信息</returns>
        public View_Resource GetResourceDetail(string id, JSZX_ResourceEntities db)
        {
            View_Resource detail = new View_Resource();
            List<View_Resource> list = null;

            var query = from t in db.View_Resource where t.DEL_FLG == "0" && t.ID == id select t;

            list = query.ToList();
            if (list != null && list.Count > 0)
            {
                detail = list[0];
            }
            
            return detail;
        }
        #endregion

        #region 获取附件信息
        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>附件信息</returns>
        public List<T_Res_Appendix> GetAppendixList(string id, JSZX_ResourceEntities db)
        {
            List<T_Res_Appendix> list = new List<T_Res_Appendix>();

            var query = from t in db.T_Res_Appendix where t.DEL_FLG == "0" && t.RES_ID == id select t;

            list = query.ToList();

            return list;
        }
        #endregion

        #region 获取资源评分信息
        /// <summary>
        /// 获取资源评分信息总数
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>资源评分信息总数</returns>
        public int GetResourceScoreSize(string id, JSZX_ResourceEntities db)
        {
            var query = from t in db.View_Score where t.RES_ID == id select t;

            return query.Count();
        }

        /// <summary>
        /// 获取资源评分信息
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>资源评分信息</returns>
        public List<View_Score> GetResourceScore(string id, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<View_Score> list = null;

            var query = from t in db.View_Score where t.RES_ID == id orderby t.CREATETIME descending select t;

            list = query.Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize).ToList();

            return list;
        }

        /// <summary>
        /// 获取资源评分
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>资源评分信息</returns>
        public List<View_Score> GetResourceScoreGrade(string id, JSZX_ResourceEntities db)
        {
            List<View_Score> list = null;

            var query = from t in db.View_Score where t.RES_ID == id select t;

            list = query.ToList();

            return list;
        }

        /// <summary>
        /// 获取资源评分信息总条数
        /// </summary>
        /// <param name="appId">附件ID</param>
        /// <param name="db"></param>
        /// <returns>资源评分信息</returns>
        public Int32 GetResourceScoreSizeByAppId(string appId, JSZX_ResourceEntities db)
        {
            var query = from t in db.View_Score where t.APPEND_ID == appId orderby t.CREATETIME descending select t;

            return query.Count();
        }

        /// <summary>
        /// 获取资源评分信息
        /// </summary>
        /// <param name="appId">附件ID</param>
        /// <param name="db"></param>
        /// <returns>资源评分信息</returns>
        public List<View_Score> GetResourceScoreByAppId(string appId, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<View_Score> list = null;

            var query = from t in db.View_Score where t.APPEND_ID == appId orderby t.CREATETIME descending select t;

            list = query.Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize).ToList();

            return list;
        }
        #endregion

        #region 获取资源提问信息
        /// <summary>
        /// 获取资源提问信息
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>提问信息</returns>
        public int GetResourceQaSize(string id, JSZX_ResourceEntities db)
        {
            var query = from t in db.View_Qa where t.RES_ID == id orderby t.CREATETIME descending select t;

            return query.Count();
        }

        /// <summary>
        /// 获取资源提问信息
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>提问信息</returns>
        public List<View_Qa> GetResourceQa(string id, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<View_Qa> list = null;

            var query = from t in db.View_Qa where t.RES_ID == id orderby t.CREATETIME descending select t;

            list = query.Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize).ToList();

            return list;
        }

        /// <summary>
        /// 获取资源提问信息条数
        /// </summary>
        /// <param name="appId">附件ID</param>
        /// <param name="db"></param>
        /// <returns>提问信息总数</returns>
        public int GetResourceQaSizeByAppId(string appId, JSZX_ResourceEntities db)
        {
            var query = from t in db.View_Qa where t.APPEND_ID == appId orderby t.CREATETIME descending select t;

            return query.Count();
        }

        /// <summary>
        /// 获取资源提问信息
        /// </summary>
        /// <param name="appId">附件ID</param>
        /// <param name="db"></param>
        /// <returns>提问信息</returns>
        public List<View_Qa> GetResourceQaByAppId(string appId, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<View_Qa> list = null;

            var query = from t in db.View_Qa where t.APPEND_ID == appId orderby t.CREATETIME descending select t;

            list = query.Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize).ToList();

            return list;
        }
        #endregion

        #region 获取资源提问信息
        /// <summary>
        /// 获取资源提问信息
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="db"></param>
        /// <returns>提问信息</returns>
        public List<View_Qa> GetResQa(string id, JSZX_ResourceEntities db)
        {
            List<View_Qa> list = null;

            var query = from t in db.View_Qa where t.ID == id select t;

            list = query.ToList();

            return list;
        }
        #endregion

        #region 前台管理 资源列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFlg"></param>
        /// <param name="userId"></param>
        /// <param name="orderBy"></param>
        /// <param name="pagesize"></param>
        /// <param name="pagecurrent"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<AppendixByLike_Result> GetAppendList(string typeFlg, string like, string userId, string orderBy, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<AppendixByLike_Result> list = new List<AppendixByLike_Result>();

            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            var query = from t in db.AppendixByLike(like, dt) select t;
            // 资源类型
            if (!string.IsNullOrEmpty(typeFlg))
            {
                query = query.Where(t => typeFlg.Contains(t.TYPE_FLG));
            }
            //登录用户的文库
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserID.Contains(userId));
            }
            #region 排序
            if (orderBy == "1")
            {
                //评价次数
                query = query.OrderByDescending(t => t.REVIEW_NUM).OrderByDescending(t => t.num).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "2")
            {
                //最受好评
                query = query.OrderByDescending(t => t.GRADE).OrderByDescending(t => t.num).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "3")
            {
                //最多浏览
                query = query.OrderByDescending(t => t.PAGE_VIEW_NUM).OrderByDescending(t => t.num).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "4")
            {
                //最多下载
                query = query.OrderByDescending(t => t.DOWNLOAD_NUM).OrderByDescending(t => t.num).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else if (orderBy == "5")
            {
                //最新上传
                query = query.OrderByDescending(t => t.UPLOAD_TIME).OrderByDescending(t => t.num).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            else
            {
                query = query.OrderByDescending(t => t.num).Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize);
            }
            #endregion 
            list = query.ToList();
            return list;
        }
        #endregion

        #region 前台管理 资源列表总数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFlg"></param>
        /// <param name="userId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int GetAppendListCount(string typeFlg,string like, string userId, JSZX_ResourceEntities db)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            var query = from t in db.AppendixByLike(like, dt) select t;
            
            // 资源类型（0：文档 1：视频, 2:图片）
            if (!string.IsNullOrEmpty(typeFlg))
            {
                query = query.Where(t => typeFlg.Contains(t.TYPE_FLG));
            }
            //登录用户的文库
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserID.Contains(userId));
            }
            return query.Count();
        }
        #endregion

        #region 前台管理 获取精品资源
        /// <summary>
        /// 前台管理 获取精品资源
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<View_Res_Appendix> GetExcellentAppendix(string typeFlag, JSZX_ResourceEntities db)
        {
            List<View_Res_Appendix> list = new List<View_Res_Appendix>();

            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            var query = from t in db.View_Res_Appendix
                        where t.EXCELLENT_FLG == "1"
                        && t.STATUS == "1"
                        && t.DEL_FLG == "0"
                        && t.APP_DEl_FLG == "0"
                        && typeFlag.Contains(t.TYPE_FLG)
                        && (t.IS_FOREVER == true || (t.IS_FOREVER == false && t.ACTIVE_TIME_START <= dt && t.ACTIVE_TIME_END >= dt))
                        select t;
            query = query.OrderByDescending(t => t.UPLOAD_TIME);

            if (typeFlag == "1")
            {
                list = query.Take(Constant.HOME_VIDEO_NUMBER).ToList();
            }
            else
            {
                list = query.Take(Constant.HOME_DOC_NUMBER).ToList();
            }
            
            return list;
        }
        #endregion

        #region 根据id获取分类
        /// <summary>
        /// 根据id获取分类
        /// </summary>
        /// <param name="id">分类id</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T_Res_Type GetTypeById(string id, JSZX_ResourceEntities db)
        {
            T_Res_Type type = new T_Res_Type();

            var query = from t in db.T_Res_Type where t.ID == id select t;

            List<T_Res_Type> typeList = query.ToList();
            if (typeList != null)
            {
                type = typeList[0];
            }

            return type;
        }
        #endregion

        #region 根据分类获取附件
        /// <summary>
        /// 获取分类和其子分类id
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private string[] GetAllType(string typeId, JSZX_ResourceEntities db)
        {
            string[] type = { };

            var query = from t in db.T_Res_Type
                        where t.ID == typeId || t.BELONG_ID == typeId
                        select t.ID;
            type = query.ToArray();

            return type;
        }

        /// <summary>
        /// 根据分类获取附件总件数
        /// </summary>
        /// <param name="typeId">分类id</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int GetFileSizeByTypeId(string typeId, string like, JSZX_ResourceEntities db)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            string typeStr = GetTypeCondition(db, typeId);
            var query = from t in db.AppendixByLike(like, dt) where typeStr.Contains(t.TYPE_ID) select t;
            return query.Count();
        }

        /// <summary>
        /// 根据类型获取所有子类型
        /// </summary>
        /// <param name="db"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTypeCondition(JSZX_ResourceEntities db, string type)
        {
            List<T_Res_Type> list = new List<T_Res_Type>();
            var query = from t in db.T_Res_Type orderby t.BELONG_ID, t.ID select t;
            list = query.ToList();
            string ret = type + ",";
            ReturnTypeStr(list, type, ref ret);
            return ret;
        }
        /**递归**/
        private void ReturnTypeStr(List<T_Res_Type> list, string parentID, ref string retStr)
        {
            List<T_Res_Type> t = FindById(list, parentID);
            if (t.Count > 0)
            {
                foreach (T_Res_Type resType in t)
                {
                    string tID = resType.ID;
                    retStr = retStr + tID + ",";
                    ReturnTypeStr(list, tID, ref retStr);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 根据分类获取附件
        /// </summary>
        /// <param name="typeId">分类id</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<AppendixByLike_Result> GetFileListByTypeId(string typeId, string like, string sequence, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<AppendixByLike_Result> appList = new List<AppendixByLike_Result>();
            string typeStr = GetTypeCondition(db, typeId);

            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            var query = from t in db.AppendixByLike(like, dt) where typeStr.Contains(t.TYPE_ID) select t;
            #region 排序
            if (sequence == "1")
            {
                query = query.OrderByDescending(t => t.REVIEW_NUM).OrderByDescending(t => t.num);
            }
            else if (sequence == "2")
            {
                query = query.OrderByDescending(t => t.PRAISE_PRE).OrderByDescending(t => t.num);
            }
            else if (sequence == "3")
            {
                query = query.OrderByDescending(t => t.PAGE_VIEW_NUM).OrderByDescending(t => t.num);
            }
            else if (sequence == "4")
            {
                query = query.OrderByDescending(t => t.DOWNLOAD_NUM).OrderByDescending(t => t.num);
            }
            else
            {
                query = query.OrderByDescending(t => t.UPLOAD_TIME).OrderByDescending(t => t.num);
            }
            #endregion
            appList = query.Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize).ToList();
            return appList;
        }
        #endregion

        #region 根据关键字检索
        /// <summary>
        /// 拼接sql
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="db"></param>
        /// <returns></returns>
        private IEnumerable<AppendixByLike_Result> getSql(string keyword, string filetype, JSZX_ResourceEntities db)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            var query = from t in db.AppendixByLike(keyword, dt) select t;

            if (!string.IsNullOrEmpty(filetype))
            {
                if (filetype == "video")
                {
                    query = query.Where(t => t.TYPE_FLG == "1");
                }
                else
                {
                    query = query.Where(t=>t.TYPE_FLG == "0" && t.FILE_NAME.Contains(filetype));
                }
            }
            return query;
        }

        /// <summary>
        /// 根据关键字检索出的总数
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int GetAppendixSizeByKeyword(string keyword,string fileType, JSZX_ResourceEntities db)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            var query = from t in db.AppendixByLike(keyword, dt) select t;
            if (!string.IsNullOrEmpty(fileType))
            {
                if (fileType == "video")
                {
                    query = query.Where(t => t.TYPE_FLG == "1");
                }
                else
                {
                    query = query.Where(t => t.TYPE_FLG == "0" && t.FILE_NAME.Contains(fileType));
                }
            }
            return query.Count();
        }

        /// <summary>
        /// 根据关键字检索
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pagesize">总页数</param>
        /// <param name="pagecurrent">跳转页</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<AppendixByLike_Result> GetAppendixListByKeyword(string keyword,string fileType, string orderBy, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<AppendixByLike_Result> list = new List<AppendixByLike_Result>();
            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

            var query = from t in db.AppendixByLike(keyword, dt) select t;
            if (!string.IsNullOrEmpty(fileType))
            {
                if (fileType == "video")
                {
                    query = query.Where(t => t.TYPE_FLG == "1");
                }
                else
                {
                    query = query.Where(t => t.TYPE_FLG == "0" && t.FILE_NAME.Contains(fileType));
                }
            }
            #region 排序
            if (orderBy == "1")
            {
                query = query.OrderByDescending(t => t.REVIEW_NUM).OrderByDescending(t => t.num);
            }
            else if (orderBy == "2")
            {
                query = query.OrderByDescending(t => t.GRADE).OrderByDescending(t => t.num);
            }
            else if (orderBy == "3")
            {
                query = query.OrderByDescending(t => t.PAGE_VIEW_NUM).OrderByDescending(t => t.num);
            }
            else if (orderBy == "4")
            {
                query = query.OrderByDescending(t => t.DOWNLOAD_NUM).OrderByDescending(t => t.num);
            }
            else
            {
                query = query.OrderByDescending(t => t.UPLOAD_TIME).OrderByDescending(t => t.num);
            }
            #endregion
            list = query.Select(t => t).Skip((pagecurrent - 1) * pagesize).Take(pagesize).ToList();
            return list;
        }
        #endregion

        #region 根据id获取附件信息
        /// <summary>
        /// 根据id获取附件信息
        /// </summary>
        /// <param name="id">附件id</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T_Res_Appendix GetAppendixInfoById(string id, JSZX_ResourceEntities db)
        {
            T_Res_Appendix app = null;

            var query = from t in db.T_Res_Appendix where t.ID == id select t;

            List<T_Res_Appendix> list = query.ToList();

            if (list != null && list.Count > 0)
            {
                app = list[0];
            }

            return app;
        }
        #endregion

        #region 获取标签
        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<T_Res_Tag> GetLabelByKeyword(string id, string keyword, JSZX_ResourceEntities db)
        {
            List<T_Res_Tag> list = new List<T_Res_Tag>();

            var query = from t in db.T_Res_Tag select t;

            if (id != null)
            {
                query = query.Where(t => t.PARENTID == id);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.NAME.Contains(keyword) || t.PY.Contains(keyword));
            }

            query = query.OrderBy(t => t.CREATETIME);

            list = query.ToList();
            
            return list;
        }
        #endregion

        #region 添加标签
        public Boolean AddTag(T_Res_Tag newTag, JSZX_ResourceEntities db)
        {
            try
            {
                db.T_Res_Tag.Add(newTag);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog(clsName, "AddTag", ex.Message);
                return false;
            }
        }
        #endregion

        #region 删除标签
        public Boolean DelTag(string tagID, JSZX_ResourceEntities db)
        {
            try
            {
                DelTagList(tagID, db);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog(clsName, "DelTag", ex.Message);
                return false;
            }
        }

        private void DelTagList(string id, JSZX_ResourceEntities db)
        {
            T_Res_Tag model = db.T_Res_Tag.FirstOrDefault(m => m.ID == id);
            if (model != null)
            {
                List<T_Res_Tag> list = new List<T_Res_Tag>();
                var query = from t in db.T_Res_Tag where t.PARENTID == id select t;
                list = query.ToList();
                foreach (T_Res_Tag tag in list)
                {
                    DelTagList(tag.ID, db);
                }
                db.T_Res_Tag.Remove(model);
            }
        }
        #endregion

        #region 添加资源分类
        public Boolean AddType(T_Res_Type newType, JSZX_ResourceEntities db)
        {
            try
            {
                db.T_Res_Type.Add(newType);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog(clsName, "AddType", ex.Message);
                return false;
            }
        }
        #endregion

        #region 删除资源分类
        public Boolean DelType(string typeID, JSZX_ResourceEntities db)
        {
            try
            {
                DelTypeList(typeID, db);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog(clsName, "DelType", ex.Message);
                return false;
            }
        }

        private void DelTypeList(string id, JSZX_ResourceEntities db)
        {
            T_Res_Type model = db.T_Res_Type.FirstOrDefault(m => m.ID == id);
            if (model != null)
            {
                List<T_Res_Type> list = new List<T_Res_Type>();
                var query = from t in db.T_Res_Type where t.BELONG_ID == id select t;
                list = query.ToList();
                foreach (T_Res_Type type in list)
                {
                    DelTypeList(type.ID, db);
                }
                db.T_Res_Type.Remove(model);
            }
        }
        #endregion

        #region 我的收藏
        /// <summary>
        /// 获取我的收藏总数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="like">模糊条件</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int GetMyCollectCount(string userId, string like, JSZX_ResourceEntities db)
        {
            int ret = 0;
            try
            {
                DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                var query = from t in db.MyCollectionByLike(like, dt) select t;
                //此用户的收藏资源
                query = query.Where(t => t.MyUserID.Equals(userId));
                ret = query.Count();
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("ResourceList", "GetMyCollectCount", "Error! " + ex.Message);
            }
            return ret;
        }
        /// <summary>
        /// 获取我的收藏列表（含分页）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="like">模糊条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="pagecurrent">当前页</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<MyCollectionByLike_Result> GetMyCollectList(string userId, string like, string orderBy, int pagesize, int pagecurrent, JSZX_ResourceEntities db)
        {
            List<MyCollectionByLike_Result> models = new List<MyCollectionByLike_Result>();
            try
            {
                DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));

                var query = from t in db.MyCollectionByLike(like, dt) select t;
                //此用户的收藏资源
                query = query.Where(t => t.MyUserID.Equals(userId));
                //按收藏时间倒序
                if (orderBy == "0")
                {
                    query = query.OrderByDescending(t => t.CreateTime).OrderByDescending(t => t.num);
                }
                if (orderBy == "1")
                {
                    //评价次数
                    query = query.OrderByDescending(t => t.REVIEW_NUM).OrderByDescending(t => t.num);
                }
                else if (orderBy == "2")
                {
                    //最受好评
                    query = query.OrderByDescending(t => t.GRADE).OrderByDescending(t => t.num);
                }
                else if (orderBy == "3")
                {
                    //最多浏览
                    query = query.OrderByDescending(t => t.PAGE_VIEW_NUM).OrderByDescending(t => t.num);
                }
                else if (orderBy == "4")
                {
                    //最多下载
                    query = query.OrderByDescending(t => t.DOWNLOAD_NUM).OrderByDescending(t => t.num);
                }
                //默认按收藏时间倒序
                else 
                {
                    query = query.OrderByDescending(t => t.CreateTime).OrderByDescending(t => t.num);
                }
                //获取当前页的数据
                models = query.Skip((pagecurrent - 1) * pagesize).Take(pagesize).ToList();
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("ResourceList", "GetMyCollectList", "Error! " + ex.Message);
            }
            return models;
        }
        #endregion

        #region 递归判断分类是否被使用
        /// <summary>
        /// 查询分类有无被使用
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Boolean TypeIsUsed(string typeId)
        {
            bool ret = false;
            try
            {
                if (!string.IsNullOrEmpty(typeId))
                {
                    using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                    {
                        string typeStr = GetTypeCondition(db, typeId);
                        var query = from t in db.T_Resource select t;
                        query = query.Where(t => typeStr.Contains(t.TYPE_ID));
                        query = query.Where(t => t.DEL_FLG == "0");//只需要判断没有被删除的资源当中，有没有使用该分类
                        if (query.Count() > 0)
                        {
                            ret = true;
                        }
                    }
                }
                else
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Resource", "TypeIsUsed", "Error! " + ex.Message);
            }
            return ret;
        }
        #endregion

        #region 统计报表
        /// <summary>
        /// 统计报表
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<ResStatistics_Result> statisticsInfo(string orderBy, JSZX_ResourceEntities db)
        {
            List<ResStatistics_Result> vs = new List<ResStatistics_Result>();

            var query = from t in db.ResStatistics() select t;

            //按上传最多
            if (orderBy == "1")
            {
                //上传最多
                query = query.OrderByDescending(t => t.allFile);
            }
            else if (orderBy == "2")
            {
                //最受好评
                query = query.OrderByDescending(t => t.PRAISE_PRE);
            }
            else if (orderBy == "3")
            {
                //最多浏览
                query = query.OrderByDescending(t => t.VIEW_NUM);
            }
            else if (orderBy == "4")
            {
                //最多下载
                query = query.OrderByDescending(t => t.DOWNLOAD_NUM);
            }
            //默认按老师排序
            else
            {
                query = query.OrderBy(t => t.CREATEID);
            }

            vs = query.ToList();

            return vs;
        }
        #endregion


        /// <summary>
        /// 根据当前登陆用户ID取得所在的学校ID
        /// </summary>
        /// <param name="UserId">当前登陆用户ID</param>
        /// <returns></returns>
        public string GetLoginUserSchoolId(string UserId)
        {
            try
            {
                string sql = " ";
                sql = sql + " select SchoolId from Z_User ";
                sql = sql + " where Login_Name = '" + UserId + "'";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["SchoolId"].ToString();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}