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
    
    public partial class T_Receive_Message
    {
        public string Msg_Id { get; set; }
        public string Receiver_Id { get; set; }
        public Nullable<System.DateTime> Receiver_Time { get; set; }
        public string Msg_Status { get; set; }
        public string Record_Id { get; set; }
        public string Msg_Title { get; set; }
        public string Msg_Content { get; set; }
        public string Type { get; set; }
        public string Send_Id { get; set; }
        public Nullable<System.DateTime> Send_Time { get; set; }
    }
}
