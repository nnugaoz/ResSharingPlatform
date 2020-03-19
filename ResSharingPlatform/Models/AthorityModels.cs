using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Models
{
    public class AthorityModels
    {
        /// <summary>
        /// 上传权限
        /// </summary>
        public string AddAthority { get; set; }

        /// <summary>
        /// 删除权限
        /// </summary>
        public string DelAthority { get; set; }

        /// <summary>
        /// 编辑权限
        /// </summary>
        public string EditAthority { get; set; }

        /// <summary>
        /// 显示权限
        /// </summary>
        public string ShowPageAthority { get; set; }

        /// <summary>
        /// 审核权限
        /// </summary>
        public string ExamineAthority { get; set; }

        /// <summary>
        /// 下载权限
        /// </summary>
        public string DownLoadAthority { get; set; }
    }
}