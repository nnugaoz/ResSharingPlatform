using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResSharingPlatform.Common;
using System.Data;
using ResSharingPlatform.Lib;

namespace ResSharingPlatform.Models
{
    public class RoleOperate
    {
        #region select
        /// <summary>
        /// 分页数据
        /// 页面权限信息
        /// </summary>
        /// <param name="title">页面权限名称</param>
        /// <returns></returns>
        public string Select_Role_Page(string title)
        {
            List<object> Ret = new List<object>();
            try
            {
                string sql = "SELECT row_number() over (order by EditDate DESC) as rowid, * FROM Z_Role WHERE Del = '0' AND Title LIKE '%" + title + "%'";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                //保存数据表并返回
                Ret.Add(ds);

                return "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) + "}";
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return ToJosn.wxListToJson(Ret);
            }
        }

        /// <summary>
        /// 分页数据
        /// 页面权限信息
        /// </summary>
        /// <param name="title">页面权限名称</param>
        /// <returns></returns>
        public string get_role_user()
        {
            List<object> Ret = new List<object>();
            try
            {
                string sql = "select Z_Role.* ,(select count(*) from Z_User ";
                sql = sql+" where Z_Role.ID=Z_User.PageRole_ID) as usercount ";
                sql = sql+" FROM Z_Role WHERE Del = '0'";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                //保存数据表并返回
                Ret.Add(ds);

                return "{\"result\":\"success\",\"list\":" + ToJosn.wxListToJson(Ret) + "}";
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return ToJosn.wxListToJson(Ret);
            }
        }

        /// <summary> 
        /// 获取角色下的用户
        /// </summary>
        /// <param name="title">页面权限名称</param>
        /// <returns></returns>
        public string get_role_user_list(string id)
        {
            List<object> Ret = new List<object>();
            try
            {
                string sql = "select * from Z_User ";
                sql = sql+" where PageRole_ID='"+id+"' ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);
                //保存数据表并返回
               // Ret.Add(ds);

                return "{\"result\":\"success\",\"id\":\"" + id + "\",\"list\":" + JsonHelper.ToJson(ds) + "}";
            }
            catch (Exception ex)
            {
                Ret.Add("state:0,msg:获取数据错误！");
                return ToJosn.wxListToJson(Ret);
            }
        }

       /// <summary>
       /// 检索用户
       /// </summary>
       /// <param name="username">用户名</param>
       /// <returns></returns>
        public string search_user(string username)
        {
            
            try
            {
                string sql = "select ID,Name from Z_User ";
                sql = sql + " where Name like'%" + username + "%' ";

                DBOperation db = new DBOperation();
                DataSet ds = new DataSet();
                ds = db.GetDataSet(sql);

                return "{\"result\":\"success\",\"list\":" + JsonHelper.ToJson(ds) + "}";
            }
            catch (Exception ex)
            { 
                return "{\"result\":\"error\"}";
            }
        }
        

        

        /// <summary>
        /// 单条数据
        /// 页面权限信息
        /// </summary>
        /// <param name="id">页面权限ID</param>
        /// <returns></returns>
        public string Select_Role_One(string id)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var linq =
                        from t in db.SPT_Role_One(id)
                        select t;
                
                    List<SPT_Role_One_Result> it = new List<SPT_Role_One_Result>();
                    it = linq.ToList();
                
                    string ret = "";
                    // 判断是否取到数据
                    if (it != null && it.Count > 0)// 取到数据
                    {
                        ret += "{\"Table\":[";
                        for (int i = 0; i < it.Count; i++)
                        {
                            // 不是第一行 补逗号
                            if (i != 0)
                            {
                                ret += ",";
                            }
                
                            ret += "{"
                                + "\"ID\":\"" + it[i].ID + "\""
                                + ",\"Title\":\"" + it[i].Title + "\""
                                + ",\"Remark\":\"" + it[i].Remark + "\""
                                + "}";
                        }
                        ret += "]}";
                    }
                
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 列表数据
        /// 菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Select_Menus(string id)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var linq =
                        from t in db.SPT_Menus(id)
                        select t;
                
                    List<SPT_Menus_Result> it = new List<SPT_Menus_Result>();
                    it = linq.ToList();
                
                    string ret = "";
                    // 判断是否取到数据
                    if (it != null && it.Count > 0)// 取到数据
                    {
                        ret += "{\"Table\":[";
                        for (int i = 0; i < it.Count; i++)
                        {
                            // 不是第一行 补逗号
                            if (i != 0)
                            {
                                ret += ",";
                            }
                
                            ret += "{"
                                + "\"id\":\"" + it[i].id + "\""
                                + ",\"pId\":\"" + it[i].pId + "\""
                                + ",\"name\":\"" + it[i].name + "\""
                                + ",\"checked\":\"" + it[i].is_checked + "\""
                                + "}";
                        }
                        ret += "]}";
                    }
                
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region insert
        /// <summary>
        /// 插入Z_Role
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="title">角色名称</param>
        /// <param name="remark">描述</param>
        /// <returns></returns>
        public bool Insert_Role(ref string id, string title, string remark)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    id = Guid.NewGuid().ToString();
                
                    Z_Role it = new Z_Role();
                    it.ID = id;
                    it.Title = title;
                    it.Remark = remark;
                    it.Del = false;
                    it.EditMan = "";
                    it.EditDate = DateTime.Now;
                    it.Version = Parameters.Version;
                    db.Z_Role.Add(it);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 插入Z_Role_Menu
        /// </summary>
        /// <param name="role_id">角色ID</param>
        /// <param name="menus">菜单code串</param>
        /// <returns></returns>
        public bool Insert_Menus(string role_id, string menus)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    string[] codes = menus.Split(',');
                    for (int i = 0; i < codes.Length; i++)
                    {
                        if (codes[i] != "")
                        {
                            Z_Role_Menu it = new Z_Role_Menu();
                            it.Role_ID = role_id;
                            it.Menu_Code = codes[i];
                            it.Del = false;
                            it.EditMan = "";
                            it.EditDate = DateTime.Now;
                            it.Version = Parameters.Version;
                            db.Z_Role_Menu.Add(it);
                        }
                    }
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region update
        /// <summary>
        /// 更新Z_Role
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="title">角色名称</param>
        /// <param name="remark">描述</param>
        /// <returns></returns>
        public bool Update_Role(string id, string title, string remark)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var list = (
                            from t in db.Z_Role
                            where t.ID == id
                            select t
                        ).Take(1).ToList();
                
                    if (list != null && list.Count > 0)
                    {
                        var it = list.First();
                        it.Title = title;
                        it.Remark = remark;
                        it.EditMan = "";
                        it.EditDate = DateTime.Now;
                        it.Version = Parameters.Version;
                        db.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 插入Z_Role_Menu
        /// </summary>
        /// <param name="role_id">角色ID</param>
        /// <param name="menus">菜单code串</param>
        /// <returns></returns>
        public bool Update_Menus(string role_id, string menus)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var list = (
                            from t in db.Z_Role_Menu
                            where t.Role_ID == role_id
                            select t
                        ).ToList();
                
                    for (int i = 0; i < list.Count; i++)
                    {
                        db.Z_Role_Menu.Remove(list[i]);
                    }
                    db.SaveChanges();
                }
                
                return Insert_Menus(role_id, menus);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region delete
        /// <summary>
        /// 删除Z_Role
        /// 删除Z_Role_Menu
        /// </summary>
        /// <param name="role_id">角色ID</param>
        /// <returns></returns>
        public bool Delete_Role(string id)
        {
            try
            {
                //using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                //{
                //    var linq =
                //        from t in db.SP_Delete("Z_Role", id, "", "", "", "")
                //        select t;

                //    string ret = linq.FirstOrDefault();
                
                //    if (ret == "")
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}