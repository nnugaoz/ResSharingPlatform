//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResSharingPlatform.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_Public_Annex
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public Nullable<int> FileType { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public byte[] FileFirstImg { get; set; }
        public string TypeID { get; set; }
        public string TypeName { get; set; }
        public string Tag { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public int PAGECOUNT { get; set; }
    }
}
