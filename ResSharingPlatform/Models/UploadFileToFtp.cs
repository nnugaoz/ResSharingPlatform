using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;
using ResSharingPlatform.Common;

namespace ResSharingPlatform.Models
{
    public class UploadFileToFtp
    {
        #region 上传到服务器
        /// <summary>
        /// 上传到服务器
        /// </summary>
        /// <param name="fileData">文件信息</param>
        /// <param name="dirPath">保存到服务器上的文件路径</param>
        /// <param name="saveName">保存到服务器上的文件名</param>
        public string fileUpload(HttpPostedFileBase fileData, string dirPath, string saveName)
        {
            Stream localFileStream = null;
            FileStream fstream = null;

            try
            {
                int buffLength = 1024;
                byte[] buff = new byte[buffLength];
                localFileStream = fileData.InputStream;

                int contentLen = localFileStream.Read(buff, 0, buffLength);

                string fileUrl = dirPath;

                if (!Directory.Exists(fileUrl))
                {
                    Directory.CreateDirectory(fileUrl);
                }

                fileUrl = fileUrl + "\\" + saveName;

                fstream = new FileStream(fileUrl, FileMode.Create);

                while (contentLen != 0)
                {
                    fstream.Write(buff, 0, contentLen);

                    contentLen = localFileStream.Read(buff, 0, buffLength);
                }

                return fileUrl;
            }
            catch (Exception ex)
            {
                clsLog.ErrorLog("UploadFileToFtp", "fileUpload", ex.Message);
                return null;
            }
            finally
            {
                if (localFileStream != null)
                {
                    localFileStream.Close();
                }
                if (fstream != null)
                {
                    fstream.Close();
                }
            }
        }
        #endregion
    }
}