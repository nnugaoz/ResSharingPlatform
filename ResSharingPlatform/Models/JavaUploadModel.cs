using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Models
{
    public class JavaUploadModel
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 资源分类ID
        /// </summary>
        public string TypeID { get; set; }
        /// <summary>
        /// 资源介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 资源标签
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 创建者ID
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string[] FileName { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public string ActiveTimeStart { get; set; }
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public string ActiveTimeEnd { get; set; }
        /// <summary>
        /// 附件地址
        /// </summary>
        public string[] FileUrl { get; set; }

        /// <summary>
        /// 是否永久有效
        /// </summary>
        public string isForever { get; set; }
    }
}