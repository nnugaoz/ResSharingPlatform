using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Models
{
    public class AppendixModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public string Res_Id { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 附件保存名称
        /// </summary>
        public string SaveName { get; set; }

        /// <summary>
        /// 附件后缀名
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        private DateTime _uploadTime = DateTime.Now;
        public DateTime GetUploadTime()
        {
            return this._uploadTime;
        }
        public void SetUploadTime(string value)
        {
            if (value != null)
            {
                try
                {
                    _uploadTime = Convert.ToDateTime(value);
                }
                catch
                {
                    _uploadTime = DateTime.Now;
                }
            }
            else
            {
                _uploadTime = DateTime.Now;
            }
        }

        public string Author { get; set; }
        /// <summary>
        /// 有效开始时间
        /// </summary>
        private Nullable<DateTime> _startTime = null;
        public Nullable<DateTime> GetStartTime()
        {
            return this._startTime;
        }
        public void SetStartTime(string value)
        {
            value = string.IsNullOrEmpty(value) == true ? DateTime.Now.ToString("yyyy/MM/dd") : value;
            value = value + " 00:00:00";
            try
            {
                _startTime = Convert.ToDateTime(value);
            }
            catch
            {
                _startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
            }
        }
        
        /// <summary>
        /// 有效结束时间
        /// </summary>
        private Nullable<DateTime> _endTime = null;
        public Nullable<DateTime> GetEndTime()
        {
            return _endTime;
        }
        public void SetEndTime(string value)
        {
            value = string.IsNullOrEmpty(value) == true ? DateTime.Now.ToString("yyyy/MM/dd") : value;
            value = value + " 23:59:59";
            try
            {
                _endTime = Convert.ToDateTime(value);
            }
            catch
            {
                _endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59");
            }
        }
        
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// swf文件路径
        /// </summary>
        public string SwfUrl { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public string DelFlg { get; set; }

        /// <summary>
        ///  文件类型
        /// </summary>
        public string TypeFlg { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateId { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        public string ModifyId { get; set; }

        /// <summary>
        /// 是否永久有效
        /// </summary>
        private Boolean isForever = false;
        public Boolean getIsForever()
        {
            return isForever;
        }
        public void setIsForever(string forever)
        {
            if ("1".Equals(forever))
            {
                isForever = true;
            }
            else
            {
                isForever = false;
            }
        }

    }
}