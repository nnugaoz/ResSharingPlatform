using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResSharingPlatform.Common;

namespace ResSharingPlatform.Models
{
    public class MenuOperate
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="User_ID">用户ID</param>
        /// <returns></returns>
        public List<SPT_Menus_User_Result> Get_Menus(string User_ID)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var linq =
                        from t in db.SPT_Menus_User(User_ID, "02")
                        select t;

                    return linq.ToList();
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("MenuOperate", "Get_Menus", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User_Id"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public List<SPT_Get_Role_Button_Result> GetButton(string User_Id, string pageId)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var linq =
                        from t in db.SPT_Get_Role_Button(User_Id, pageId)
                        select t;

                    return linq.ToList();
                }
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("MenuOperate", "GetButton", ex.Message);
                return null;
            }
        }
    }
}