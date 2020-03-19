using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Models
{
    public class VoucherModels
    {
        /// <summary>
        /// GUID是否有效
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool GUID(string guid)
        {
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var rls = (from t in db.Z_User where t.GUID == guid select t).Take(1).ToList();
                    if (rls.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex) { string s = ex.Message; }
            return false;
        }

        /// <summary>
        /// 获得凭证
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="guid"></param>
        /// <param name="pLoginReturn"></param>
        /// <returns></returns>
        public bool Verification(string Account, string guid, out LoginReturn pLoginReturn)
        {
            pLoginReturn = new LoginReturn();
            try
            {
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    var rls = (from t in db.Z_User where t.ID == Account select t).Take(1).ToList();

                    if (rls.Count > 0)
                    {
                        var pl = rls.First();

                        #region /*给返回类 赋值 start*/
                        //ID
                        pLoginReturn.ID = pl.ID;
                        //姓名
                        pLoginReturn.Name = pl.Name;
                        //权限ID
                        pLoginReturn.Role_ID = pl.PageRole_ID;
                        //类型
                        pLoginReturn.Type = pl.Type;

                        pLoginReturn.DataRole = pl.DataRole;
                        #endregion

                        return true;
                    }
                }
            }
            catch (Exception ex) { string s = ex.Message; }
            return false;
        }

    }
}