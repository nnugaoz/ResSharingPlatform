using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Models
{
    public class ResourceModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string[] Ids { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public string[] isSelect { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源分类ID
        /// </summary>
        public string TypeId { get; set; }

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
        /// 审核状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 精品状态
        /// </summary>
        public string Excellent_Flg { get; set; }

        /// <summary>
        /// 附件状态：add增加、update更新、delete删除
        /// </summary>
        public string[] DataType { get; set; }

        /// <summary>
        ///  附件名
        /// </summary>
        public string[] FileName { get; set; }

        /// <summary>
        /// 保存的附件名
        /// </summary>
        public string[] SaveName { get; set; }
        /// <summary>
        /// 附件作者
        /// </summary>
        public string[] Author { get; set; }
        /// <summary>
        /// 有效时限_开始
        /// </summary>
        public string[] ActiveTimeStart { get; set; }

        /// <summary>
        /// 有效时限_结束
        /// </summary>
        public string[] ActiveTimeEnd { get; set; }

        /// <summary>
        /// 是否永久有效
        /// </summary>
        public string[] isForever { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public string[] UploadTime { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string[] FileType { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string[] FileUrl { get; set; }

        /// <summary>
        /// swf文件路径
        /// </summary>
        public string[] SwfUrl { get; set; }

        /// <summary>
        /// 附件id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        public string Review { get; set; }

        /// <summary>
        /// 提问
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 回答
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
    }
}