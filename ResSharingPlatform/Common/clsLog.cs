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

        private const string CHARACTOR_CODE = "UTF-8";           //д����־�ı���

        private static string LOG_FILEPATH = "D:\\Log\\";            //��־���·��(Ĭ�ς�)
        private const string LOG_FILENAME = "log.txt";               //��־�ļ��ļ���(Ĭ��ֵ)
        private const string PREFIX_ERROR = "E";                     //������־ǰ׺

        private byte mbytLogLevel = Convert.ToByte(LOG_LEVEL.LV_DEBUG);

        private string mstrLogFilePath = LOG_FILEPATH;
        private string mstrLogFileName = LOG_FILENAME;

        //------------------------------------------------------------------------------
        //��  Ҫ�������������־
        //��  ������strCls                   I       string����
        //        ��strMethodName            I       string����
        //        ��strEx                    I       string����
        //        ��Optional strExtention����I������ Optional string����
        //����ֵ������
        //˵  ���������������־�ļ�
        //��  ע������־����Ϊ1ʱִ�С�
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         �½�
        //2007/05/02 1.01 S.Hirooka       ����������Object���ͱ��string
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
        //��  Ҫ�������һ����־
        //��  ������strCls                   I       string����
        //        ��strMethodName            I       string����
        //        ��strEx                    I       string����
        //        ��Optional strExtention����I������ Optional string����
        //����ֵ������
        //˵  ���������������־�ļ�
        //��  ע������־����Ϊ1ʱִ�С�
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         �½�
        //2007/05/02 1.01 S.Hirooka       ����������Object���ͱ��string
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
        //��  Ҫ�����趨��־����·������
        //��  ������strFilePath      I       string���� ��
        //����ֵ������
        //˵  �������趨��־����·������
        //��  ע����
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         �½�
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
        //��  Ҫ���������־�ļ����ļ����趨
        //��  ������strFileName      I       string�� ��
        //����ֵ������
        //˵  ����������������־���ļ���
        //��  ע����
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         �½�
        //------------------------------------------------------------------------------
        private static void SetFileName(string strFileName)
        {
            lock (myLogWrite)
            {
                myLogWrite.mstrLogFileName = strFileName;
            }
        }

        //------------------------------------------------------------------------------
        //��  Ҫ���������־����
        //��  ������strPrefix  ��            I       string�ͣ�E:������־ D:Debug��־ I:������ʼ���� O:����������־��
        //   ���� ��strCls                   I       string��
        //        ��strMethodName            I       string��
        //        ��strLogText������         I������ string��
        //����ֵ������
        //˵  ����������־д����־�ļ�
        //��  ע����
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka         �½�
        //------------------------------------------------------------------------------
        private void LogOut(string strPrefix, string strCls, string strMethodName, string strLogText)
        {
            try
            {
                //Output file path acquisition   ��־�ļ�·��ȡ�á�
                string strFilePath = mstrLogFilePath + GetFileName(mstrLogFileName);
                //MessageBox.Show(strFilePath);
                //create the log file    ��־�ļ����ɡ�
                FileStream OutStream = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write);

                //create a Char writer   
                StreamWriter CharWriter = new StreamWriter(OutStream, Encoding.GetEncoding(CHARACTOR_CODE));
                CharWriter.BaseStream.Seek(0, SeekOrigin.End); // set the file pointer to the end

                //log the new message    д��־�ļ�
                WriteLog(strPrefix, strCls, strMethodName, strLogText, CharWriter);

                //close the writer and underlying file   �ر��ļ�
                CharWriter.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show("Error!");
            }

        }

        //------------------------------------------------------------------------------
        //��  Ҫ������־�ļ������ɺ���
        //��  ������strBeFileName          I       string��
        //����ֵ����string��
        //˵  ������������־�ļ���
        //��  ע�������ݲ���������־�ļ���
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/05/02 1.00 S.Hirooka      �½�
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
        //��  Ҫ������ָ����������������־
        //��  ������strPrefix  ��            I       string�ͣ�E:������־ D:Debug��־ I:������ʼ���� O:����������־��
        //   ���� ��strCls                 I       string��
        //        ��strMethodName            I       string��
        //        ��strLogText������         I������ string��
        //����������Writer                   I������ StreamWriter���� 
        //����ֵ�����ʤ�
        //˵  ��������ָ����������������־
        //��  ע����
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka      �½�
        //------------------------------------------------------------------------------
        private void WriteLog(string strPrefix, string strCls, string strMethodName, string strLogText, StreamWriter Writer)
        {

            string strClsName = strCls;
            strLogText = strLogText.Replace(System.Environment.NewLine, "(CRLF)");
            //strLogText = strLogText.Replace(Keys.Control, "(CR)");
            //strLogText = strLogText.Replace(Keys.Return, "(LF)");

            // The contents of a log output are edited.  �༭������ݡ�
            strLogText = strClsName + "\t" + strMethodName + "\t" + strLogText;

            // The line output of the contents of a log is carried out.  ������ļ���
            Writer.WriteLine(strPrefix + "\t" + GetDateTime() + "\t" + GetThreadId() + "\t" + strLogText);
            Writer.Flush();

        }

        //------------------------------------------------------------------------------
        //��  Ҫ������ȡϵͳʱ�亯��
        //��  ��������
        //����ֵ����string��
        //˵  ����������ϵͳʱ�䴮
        //��  ע������yyyy/MM/dd_HH:mm:ss:fff��ʽ����ϵͳʱ��
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka      �½�
        //------------------------------------------------------------------------------
        private string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss:fff");
        }

        //------------------------------------------------------------------------------
        //��  Ҫ����ȡ�õ�ǰ�߳�ID
        //��  ��������
        //����ֵ����Integer��
        //˵  ������ȡ�õ�ǰ�߳�ID
        //��  ע����
        //�������ݣ�
        //�����ա��� Ver���������������� ����
        //---------- ---- -------------- -----------------------------------------------
        //2007/04/23 1.00 S.Hirooka      �½�
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