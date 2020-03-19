using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace ResSharingPlatform.Models
{
    public class Basice
    {
        //#region 获取ftp地址，上传文件大小信息
        ///// <summary>
        ///// 获取ftp地址，上传文件大小信息
        ///// </summary>
        ///// <param name="db"></param>
        ///// <returns></returns>
        //public Z_FTP_Parameters GetFtpInfo(GM_BasiceEntities db)
        //{
        //    Z_FTP_Parameters ftp = null;

        //    var query = from t in db.Z_FTP_Parameters select t;
        //    query = query.OrderByDescending(t => t.EditDate);
        //    ftp = query.FirstOrDefault();

        //    return ftp;
        //}
        //#endregion

        /// <summary>
        /// 安全退出
        /// </summary>
        /// <param name="guid"></param>
        public void outLogin(string guid)
        {
            #region /*更改数据库信息 start*/
            JSZX_ResourceEntities db = new JSZX_ResourceEntities();
            var rlt = (from t in db.Z_User
                       where t.GUID == guid
                       select t).Take(1).ToList();
            var up = rlt.First();
            up.GUID = "";
            //保存数据
            db.SaveChanges();
            #endregion
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="uid">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="ip">IP</param>
        /// <param name="guid">GUID</param>
        /// <param name="pLoginReturn">返回类</param>
        /// <returns>bool是否登录成功</returns>
        public bool Login(string uid, string pwd, string ip, string guid, bool autoLogin, out LoginReturn pLoginReturn)
        {
            //实例化返回类
            pLoginReturn = new LoginReturn();
            try
            {
                //实例化数据库实体
                using (JSZX_ResourceEntities db = new JSZX_ResourceEntities())
                {
                    //用户登录查询
                    //密码改成MD5,和BBS及CMS一致
                    byte[] result = Encoding.Default.GetBytes(pwd);    //Password为输入密码的文本
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] output = md5.ComputeHash(result);
                    pwd = BitConverter.ToString(output).Replace("-", "").ToLower();  //tbMd5pass为输出加密文本的


                    var rls = (from t in db.Z_User where t.ID == uid && t.Password == pwd && t.Del == false select t).Take(1).ToList();
                    if (rls.Count > 0)
                    {
                        #region /*更改数据库信息 start*/
                        var pl = rls.First();
                        //登录次数
                        pl.Link_Count++;
                        //登录时间
                        pl.Login_Time = DateTime.Now;
                        //IP
                        pl.IP = ip;
                        //GUID
                        //pl.GUID = guid;
                        if (autoLogin)
                        {
                            pl.isSeven = true;
                        }
                        else
                        {
                            pl.isSeven = false;
                        }
                        #endregion

                        #region /*给返回类 赋值 start*/
                        //ID
                        pLoginReturn.ID = pl.ID;
                        //姓名
                        pLoginReturn.Name = pl.Name;
                        //权限ID
                        pLoginReturn.Role_ID = pl.PageRole_ID;
                        //类型
                        pLoginReturn.Type = pl.Type;
                        //数据权限
                        pLoginReturn.DataRole = pl.DataRole;
                        pLoginReturn.IsCmsAdmin = pl.IsCmsAdmin;
                        pLoginReturn.IsMonitorAdmin = pl.IsMonitorAdmin;
                        pLoginReturn.SchoolId = pl.SchoolId;
                        #endregion

                        //保存数据
                        db.SaveChanges();

                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex) { string s = ex.Message; return false; }
        }
    }
}