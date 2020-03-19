using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Models
{
    public class ComboTreeModels
    {
        /// <summary>
        /// ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 分类名
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 所属id
        /// </summary>
        public string belong { get; set; }

        /// <summary>
        /// 子分类
        /// </summary>
        public List<ComboTreeModels> children { get; set; }

    }
}