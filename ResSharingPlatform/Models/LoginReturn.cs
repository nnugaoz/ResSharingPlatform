using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Models
{
    public class LoginReturn
    {
        /// <summary> 
        /// ID，老师工号/学生学号/企业自定义 
        /// </summary> 
        private String _ID;
        public String ID
        {
            set { _ID = value; }
            get { return _ID; }
        }

        /// <summary> 
        /// 用户类型，0 管理员/1 老师/2 学生/3 企业 
        /// </summary> 
        private String _Type;
        public String Type
        {
            set { _Type = value; }
            get { return _Type; }
        }

        /// <summary> 
        /// 真实姓名 
        /// </summary> 
        private String _Name;
        public String Name
        {
            set { _Name = value; }
            get { return _Name; }
        }

        /// <summary> 
        /// 角色ID，Z_Role表主键 
        /// </summary> 
        private String _Role_ID;
        public String Role_ID
        {
            set { _Role_ID = value; }
            get { return _Role_ID; }
        }

        /// <summary>
        /// 数据权限
        /// </summary>
        public string DataRole { set; get; }

        /// <summary> 
        /// CMS权限 
        /// </summary> 
        private String _IsCmsAdmin;
        public String IsCmsAdmin
        {
            set { _IsCmsAdmin = value; }
            get { return _IsCmsAdmin; }
        }

        /// <summary> 
        /// 监控权限 
        /// </summary> 
        private String _IsMonitorAdmin;
        public String IsMonitorAdmin
        {
            set { _IsMonitorAdmin = value; }
            get { return _IsMonitorAdmin; }
        }

        /// <summary> 
        /// 学校id 
        /// </summary> 
        private String _SchoolId;
        public String SchoolId
        {
            set { _SchoolId = value; }
            get { return _SchoolId; }
        }
        
    }
}