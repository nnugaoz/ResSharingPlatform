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
    
    public partial class Z_Homework_Student
    {
        public Z_Homework_Student()
        {
            this.Z_Homework_Student_Comment = new HashSet<Z_Homework_Student_Comment>();
        }
    
        public string RecordId { get; set; }
        public string StudentId { get; set; }
        public string HomeworkId { get; set; }
        public string IsRecommend { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string DeleteFlag { get; set; }
        public Nullable<int> Score { get; set; }
        public string Contents { get; set; }
    
        public virtual Z_Homework Z_Homework { get; set; }
        public virtual ICollection<Z_Homework_Student_Comment> Z_Homework_Student_Comment { get; set; }
    }
}
