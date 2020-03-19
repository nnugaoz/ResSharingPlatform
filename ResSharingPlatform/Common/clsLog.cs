using System.IO;
using System.Text;
using System;

namespace ResSharingPlatform.Common
{
    public class clsLog
    {
        enum LOG_LEVEL
        {
            LV_ERROR = 0,
            LV_TRACE = 1,
            LV_DEBUG = 2,
        };

        private static clsLog myLogWrite = new clsLog();

        private const string CHARACTOR_CODE = "UTF-8";           //写入日志的编码

        private static string LOG_FILEPATH = "D:\\Log\\";            //日志输出路径(默认値)
        private const string LOG_FILENAME = "log.txt";               //日志文件文件名(默认值)
        private const string PREFIX_ERROR = "E";                     //错误日志前缀

        private byte mbytLogLevel = Convert.ToByte(LOG_LEVEL.LV_DEBUG);

        private string mstrLogFilePath = LOG_FILEPATH;
        private string mstrLogFileName = LOG_FILENAME;

        //------------------------------------------------------------------------------
        //概  要　：输出错误日志
        //参  数　：strCls                   I       string类型
        //        ：strMethodName            I       string类型
        //        ：strEx                    I       string类型
        //        ：Optional strExtention　　I　　　 Optional string类型
        //返回值　：无
        //说  明　：输入错误日志文件
        //备  注　：日志类型为1时执行。
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         新建
        //2007/05/02 1.01 S.Hirooka       　将参数由Object类型变成string
        //------------------------------------------------------------------------------
        public static void ErrorLog(string strCls, string strMethodName, string strEx)
        {

            string strExtention = "";

            SetFilePath(LOG_FILEPATH);
            //MessageBox.Show(Application.StartupPath + LOG_FILEPATH);
            SetFileName(LOG_FILENAME);

            if (Convert.ToByte(myLogWrite.mbytLogLevel) >= Convert.ToByte(LOG_LEVEL.LV_TRACE))
            {
                lock (myLogWrite)
                {
                    myLogWrite.LogOut(PREFIX_ERROR, strCls, strMethodName, strEx + " " + strExtention);
                }
            }
        }

        //------------------------------------------------------------------------------
        //概  要　：输出一般日志
        //参  数　：strCls                   I       string类型
        //        ：strMethodName            I       string类型
        //        ：strEx                    I       string类型
        //        ：Optional strExtention　　I　　　 Optional string类型
        //返回值　：无
        //说  明　：输入错误日志文件
        //备  注　：日志类型为1时执行。
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         新建
        //2007/05/02 1.01 S.Hirooka       　将参数由Object类型变成string
        //------------------------------------------------------------------------------
        public static void OutLog(string strCls, string strMethodName, string strEx)
        {
            //string strExtention = "";

            //SetFilePath(Application.StartupPath + LOG_FILEPATH);
            //SetFileName(LOG_FILENAME);

            //if (Convert.ToByte(myLogWrite.mbytLogLevel) >= Convert.ToByte(LOG_LEVEL.LV_TRACE))
            //{
            //    lock (myLogWrite)
            //    {
            //        myLogWrite.LogOut("", strCls, strMethodName, strEx + " " + strExtention);
            //    }
            //}
        }

        //------------------------------------------------------------------------------
        //概  要　：设定日志输入路径函数
        //参  数　：strFilePath      I       string类型 　
        //返回值　：无
        //说  明　：设定日志输入路径函数
        //备  注　：
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         新建
        //------------------------------------------------------------------------------
        public static void SetFilePath(string strFilePath)
        {
            lock (myLogWrite)
            {
                if (Directory.Exists(strFilePath) == false)
                {
                    Directory.CreateDirectory(strFilePath);
                }
                myLogWrite.mstrLogFilePath = strFilePath;
            }
        }

        //------------------------------------------------------------------------------
        //概  要　：输出日志文件，文件名设定
        //参  数　：strFileName      I       string型 　
        //返回值　：无
        //说  明　：设置输入日志的文件名
        //备  注　：
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         新建
        //------------------------------------------------------------------------------
        private static void SetFileName(string strFileName)
        {
            lock (myLogWrite)
            {
                myLogWrite.mstrLogFileName = strFileName;
            }
        }

        //------------------------------------------------------------------------------
        //概  要　：输出日志处理
        //参  数　：strPrefix  　            I       string型（E:错误日志 D:Debug日志 I:函数开始日期 O:函数结束日志）
        //   　　 ：strCls                   I       string型
        //        ：strMethodName            I       string型
        //        ：strLogText　　　         I　　　 string型
        //返回值　：无
        //说  明　：将日志写入日志文件
        //备  注　：
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         新建
        //------------------------------------------------------------------------------
        private void LogOut(string strPrefix, string strCls, string strMethodName, string strLogText)
        {
            try
            {
                //Output file path acquisition   日志文件路径取得。
                string strFilePath = mstrLogFilePath + GetFileName(mstrLogFileName);
                //MessageBox.Show(strFilePath);
                //create the log file    日志文件作成。
                FileStream OutStream = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write);

                //create a Char writer   
                StreamWriter CharWriter = new StreamWriter(OutStream, Encoding.GetEncoding(CHARACTOR_CODE));
                CharWriter.BaseStream.Seek(0, SeekOrigin.End); // set the file pointer to the end

                //log the new message    写日志文件
                WriteLog(strPrefix, strCls, strMethodName, strLogText, CharWriter);

                //close the writer and underlying file   关闭文件
                CharWriter.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show("Error!");
            }

        }

        //------------------------------------------------------------------------------
        //概  要　：日志文件名生成函数
        //参  数　：strBeFileName          I       string型
        //返回值　：string型
        //说  明　：生成日志文件名
        //备  注　：根据参数产生日志文件名
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/05/02 1.00 S.Hirooka      新建
        //------------------------------------------------------------------------------
        private static string GetFileName(string strBeFileName)
        {
            string strFileName = "";
            string strDate = DateTime.Now.ToString("_yyyyMMdd");
            int intIndex = strBeFileName.LastIndexOf(".");
            if (intIndex >= 0)
                strFileName = strBeFileName.Substring(0, intIndex) + strDate + strBeFileName.Substring(intIndex);
            else
                strFileName = strBeFileName + strDate;
            return strFileName;
        }

        //------------------------------------------------------------------------------
        //概  要　：在指定的输出流上输出日志
        //参  数　：strPrefix  　            I       string型（E:错误日志 D:Debug日志 I:函数开始日期 O:函数结束日志）
        //   　　 ：strCls                 I       string型
        //        ：strMethodName            I       string型
        //        ：strLogText　　　         I　　　 string型
        //　　　　：Writer                   I　　　 StreamWriter　　 
        //返回值　：なし
        //说  明　：在指定的输出流上输出日志
        //备  注　：
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka      新建
        //------------------------------------------------------------------------------
        private void WriteLog(string strPrefix, string strCls, string strMethodName, string strLogText, StreamWriter Writer)
        {

            string strClsName = strCls;
            strLogText = strLogText.Replace(System.Environment.NewLine, "(CRLF)");
            //strLogText = strLogText.Replace(Keys.Control, "(CR)");
            //strLogText = strLogText.Replace(Keys.Return, "(LF)");

            // The contents of a log output are edited.  编辑输出内容。
            strLogText = strClsName + "\t" + strMethodName + "\t" + strLogText;

            // The line output of the contents of a log is carried out.  输出到文件。
            Writer.WriteLine(strPrefix + "\t" + GetDateTime() + "\t" + GetThreadId() + "\t" + strLogText);
            Writer.Flush();

        }

        //------------------------------------------------------------------------------
        //概  要　：获取系统时间函数
        //参  数　：无
        //返回值　：string型
        //说  明　：返回系统时间串
        //备  注　：以yyyy/MM/dd_HH:mm:ss:fff形式返回系统时间
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka      新建
        //------------------------------------------------------------------------------
        private string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss:fff");
        }

        //------------------------------------------------------------------------------
        //概  要　：取得当前线程ID
        //参  数　：无
        //返回值　：Integer型
        //说  明　：取得当前线程ID
        //备  注　：
        //更新内容：
        //年月日　　 Ver　姓名　　　　　 内容
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka      新建
        //------------------------------------------------------------------------------
        private int GetThreadId()
        {

            int intThreadId = -1;
            try
            {
                intThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            }
            catch (Exception)
            {
            }

            return intThreadId;
        }

    }

}