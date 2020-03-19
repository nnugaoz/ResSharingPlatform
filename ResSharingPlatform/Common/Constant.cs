using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Common
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class Constant
    {
        public const string FTPSERVERIP = "ftpServerIP";
        public const string FTPUSERID = "ftpUserID";
        public const string FTPPASSWORD = "ftpPassword";
        public const string FTPHEAD = "ftp://";


        /// <summary>
        /// 文件上传的ip地址：废弃
        /// </summary>
        public static string IP_ADDRESS = "http://swf.ntgzjy.com/";

        /// <summary>
        /// Swf文件的阅读地址
        /// </summary>
        public static string SWF_ADDRESS = "http://swf.ntgzjy.com/";

        /// <summary>
        /// 文件上传的硬盘
        /// </summary>
        public static string DISK_ADDRESS = "H:\\";

        /// <summary>
        /// 上传文件的文件夹
        /// </summary>
        public static string UPLOADDIRECTORY = "resourcetoswf";

        /// <summary>
        /// 文档存储文件夹
        /// </summary>
        public static string DOCDDIRECTORY = "Document";

        /// <summary>
        /// 视频存储文件夹
        /// </summary>
        public static string VFDDIRECTORY = "Video";

        /// <summary>
        /// 图片存储文件夹
        /// </summary>
        public static string IMGDIRECTORY = "Image";

        /// <summary>
        /// 视频存储文件夹
        /// </summary>
        public static string OTHERDIRECTORY = "Other";

        /// <summary>
        /// 格式转换文件夹
        /// </summary>
        public static string SWFDIRECTORY = "Swf";

        /// <summary>
        /// 上传的文件格式
        /// </summary>
        public static string FILE_TYPE = "*.doc;*.docx;*.xls;*.ppt;*.txt;*.pdf;*.flv;*.mp4;*.wmv;*.jpg;*jpeg";

        /// <summary>
        /// 文件类型
        /// </summary>
        public static string FILE_TYPE_NAME = "文档、视频和图片";

        /// <summary>
        /// 上传文件大小限制
        /// </summary>
        public static string FILE_SIZE = "300MB";

        /// <summary>
        /// 首页精品文档显示的数量
        /// </summary>
        public static int HOME_DOC_NUMBER = 8;

        /// <summary>
        /// 首页精品视频显示的数量
        /// </summary>
        public static int HOME_VIDEO_NUMBER = 8;

        /// <summary>
        /// 前台文档每页显示条数
        /// </summary>
        public static int FOREGROUND_DOC_NUM = 20;

        /// <summary>
        /// 前台视频每页显示的条数
        /// </summary>
        public static int FOREGROUND_VIDEO_NUM = 15;

        /// <summary>
        /// 前台我的文档每页显示的条数
        /// </summary>
        public static int FOREGROUND_MY_DOC_NUM = 20;

        /// <summary>
        /// 前台检索画面每页显示的条数
        /// </summary>
        public static int FOREGROUND_SEARCH_NUM = 20;

        /// <summary>
        /// 在线预览评论每页显示的条数
        /// </summary>
        public static int VIEWONLINE_COMMENTARY_NUM = 5;

        /// <summary>
        /// 在线预览提问每页显示的条数
        /// </summary>
        public static int VIEWONLINE_QUESTION_NUM = 5;

        /// <summary>
        /// 按分类查询文档每页显示的条数
        /// </summary>
        public static int ALLAPPENDIX_NUM = 10;

        /// <summary>
        /// 后台资源一览每页显示的条数
        /// </summary>
        public static int BACKGROUND_RESOURCE_NUM = 15;

        /// <summary>
        /// 后台详细画面评论每页显示的条数
        /// </summary>
        public static int BACKGROUND_COMMENTARY_NUM = 5;

        /// <summary>
        /// 后台详细画面提问每页显示的条数
        /// </summary>
        public static int BACKGROUND_QUESTION_NUM = 5;

        /// <summary>
        /// 大文件断点续传
        /// </summary>
        public static string JAVA_UPLOAD_URL = "";
        public static Int64 JAVA_UPLOAD_SIZE = 0;
        public static string JAVA_UPLOAD_EXT = "";

        /// <summary>
        /// 配置上传路径
        /// </summary>
        public static void SetFileConfig()
        {
            SYSBLL sys = new SYSBLL();
            try
            {
                IP_ADDRESS = sys.readoptions("fileConfig", "IP_ADDRESS");
                SWF_ADDRESS = sys.readoptions("fileConfig", "SWF_ADDRESS");
                DISK_ADDRESS = sys.readoptions("fileConfig", "DISK_ADDRESS");
                UPLOADDIRECTORY = sys.readoptions("fileConfig", "UPLOADDIRECTORY");
                DOCDDIRECTORY = sys.readoptions("fileConfig", "DOCDDIRECTORY");
                VFDDIRECTORY = sys.readoptions("fileConfig", "VFDDIRECTORY");
                IMGDIRECTORY = sys.readoptions("fileConfig", "IMGDIRECTORY");
                OTHERDIRECTORY = sys.readoptions("fileConfig", "OTHERDIRECTORY");
                SWFDIRECTORY = sys.readoptions("fileConfig", "SWFDIRECTORY");
                FILE_TYPE = sys.readoptions("fileConfig", "FILE_TYPE");
                FILE_TYPE_NAME = sys.readoptions("fileConfig", "FILE_TYPE_NAME");
                FILE_SIZE = sys.readoptions("fileConfig", "FILE_SIZE");
                /**大文件断点续传**/
                JAVA_UPLOAD_URL = sys.readoptions("fileConfig", "JAVA_UPLOAD_URL");
                JAVA_UPLOAD_SIZE = Convert.ToInt64(sys.readoptions("fileConfig", "JAVA_UPLOAD_SIZE"));
                JAVA_UPLOAD_EXT = sys.readoptions("fileConfig", "JAVA_UPLOAD_EXT");
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Constant", "SetFileConfig", ex.Message);
            }
        }

        /// <summary>
        /// 配置分页参数
        /// </summary>
        public static void SetPageConfig()
        {
            SYSBLL sys = new SYSBLL();
            try
            {
                HOME_DOC_NUMBER = Convert.ToInt32(sys.readoptions("pageConfig", "HOME_DOC_NUMBER"));
                HOME_VIDEO_NUMBER = Convert.ToInt32(sys.readoptions("pageConfig", "HOME_VIDEO_NUMBER"));
                FOREGROUND_DOC_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "FOREGROUND_DOC_NUM"));
                FOREGROUND_VIDEO_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "FOREGROUND_VIDEO_NUM"));
                FOREGROUND_MY_DOC_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "FOREGROUND_MY_DOC_NUM"));
                FOREGROUND_SEARCH_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "FOREGROUND_SEARCH_NUM"));
                VIEWONLINE_COMMENTARY_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "VIEWONLINE_COMMENTARY_NUM"));
                VIEWONLINE_QUESTION_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "VIEWONLINE_QUESTION_NUM"));
                ALLAPPENDIX_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "ALLAPPENDIX_NUM"));
                BACKGROUND_RESOURCE_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "BACKGROUND_RESOURCE_NUM"));
                BACKGROUND_COMMENTARY_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "BACKGROUND_COMMENTARY_NUM"));
                BACKGROUND_QUESTION_NUM = Convert.ToInt32(sys.readoptions("pageConfig", "BACKGROUND_QUESTION_NUM"));
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("Constant", "SetPageConfig", ex.Message);
            }
        }

        /// <summary>
        /// word
        /// </summary>
        private const string Doc = ".doc";
        /// <summary>
        /// word 2007
        /// </summary>
        private const string Docx = ".docx";
        /// <summary>
        /// excel
        /// </summary>
        private const string Xls = ".xls";
        /// <summary>
        /// excel 2007
        /// </summary>
        private const string Xlsx = ".xlsx";
        /// <summary>
        /// PowerPoint
        /// </summary>
        private const string Ppt = ".ppt";
        /// <summary>
        /// PowerPoint 2007
        /// </summary>
        private const string Pptx = ".pptx";
        /// <summary>
        /// pdf
        /// </summary>
        private const string Pdf = ".pdf";
        /// <summary>
        /// text
        /// </summary>
        private const string Txt = ".txt";
        /// <summary>
        /// mp4
        /// </summary>
        private const string Mp4 = ".mp4";
        /// <summary>
        /// flv
        /// </summary>
        private const string Flv = ".flv";
        /// <summary>
        /// jsp
        /// </summary>
        private const string Jpg = ".jpg";
        /// <summary>
        /// jpeg
        /// </summary>
        private const string Jpeg = ".jpeg";
        /// <summary>
        /// wmv
        /// </summary>
        private const string Wmv = ".wmv";
        private const string Rmvb = ".rmvb";
        private const string Avi = ".avi";

        /// <summary>
        /// 获取文件类型 0：文档，1：视频，2:图片，3：其他
        /// </summary>
        /// <param name="extension">后缀名</param>
        /// <returns></returns>
        public static string GetFileType(string extension)
        {
            extension = extension.ToLower();

            //文件类型：0：文档，1：视频，2:图片，3：其他
            string fileType = "2";
            switch (extension)
            {
                case Doc:
                    fileType = "0";
                    break;
                case Docx:
                    fileType = "0";
                    break;
                case Xls:
                    fileType = "0";
                    break;
                case Xlsx:
                    fileType = "0";
                    break;
                case Ppt:
                    fileType = "0";
                    break;
                case Pptx:
                    fileType = "0";
                    break;
                case Pdf:
                    fileType = "0";
                    break;
                case Txt:
                    fileType = "0";
                    break;
                case Mp4:
                    fileType = "1";
                    break;
                case Flv:
                    fileType = "1";
                    break;
                case Wmv:
                    fileType = "1";
                    break;
                case Rmvb:
                    fileType = "1";
                    break;
                case Avi:
                    fileType = "1";
                    break;
                case Jpg:
                    fileType = "2";
                    break;
                case Jpeg:
                    fileType = "2";
                    break;
                default:
                    fileType = "3";
                    break;
            }

            return fileType;
        }

        /// <summary>
        /// 获取文档文件类型 0：word/text，1：excel，2：ppt，3：pdf,4:其他
        /// </summary>
        /// <param name="extension">后缀名</param>
        /// <returns></returns>
        public static string GetDocType(string extension)
        {
            extension = extension.ToLower();

            //文件类型：0：word/text，1：excel，2：ppt，3：pdf,4:其他
            string fileType = "2";
            switch (extension)
            {
                case Doc:
                    fileType = "0";
                    break;
                case Docx:
                    fileType = "0";
                    break;
                case Xls:
                    fileType = "1";
                    break;
                case Xlsx:
                    fileType = "1";
                    break;
                case Ppt:
                    fileType = "2";
                    break;
                case Pptx:
                    fileType = "2";
                    break;
                case Pdf:
                    fileType = "3";
                    break;
                case Txt:
                    fileType = "4";
                    break;
                default:
                    fileType = "5";
                    break;
            }

            return fileType;
        }

        /**是否需要注册**/
        public static Boolean IsRegister()
        {
            try
            {
                SYSBLL sys = new SYSBLL();
                string Register = sys.readoptions("system", "REGISTER");
                bool ret = Convert.ToBoolean(Register);
                return ret;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}