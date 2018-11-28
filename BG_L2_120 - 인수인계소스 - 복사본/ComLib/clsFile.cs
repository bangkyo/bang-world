using System;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;

namespace ComLib
{
    public class clsFile
    {

        private const int logSavePeriod = 30;
        public static clsFile Run = new clsFile();
        public static clsFile GetInstance()
        {
            return Run;
        }

        //private readonly string gFilePath = Directory.GetParent(Application.StartupPath) + @"\LOG\";
        //private readonly string gFilePath = Directory.GetParent(Application.ExecutablePath).ToString() + @"\LOG\";
        private readonly string gFilePath = "C:" + @"\L2\LOG\";
        private string gFileName;
        
        public clsFile()
        {

        }

        public clsFile(string pFileName)
        {
            gFileName = pFileName;
        }

        public clsFile(string pFilePath, string pFileName)
        {
            gFileName = pFileName;
            gFilePath = pFilePath;
        }

        private string FilePathEdit(string _gFilePath, string _gFileName)
        {
            DateTime dt = DateTime.Now;
            return _gFilePath + _gFileName + "_" + dt.ToString("yyyyMMdd") + ".csv";
        }

        private string FileCreate(string _gFilePath, string _gFileName)
        {
            StreamWriter sw;

            var _gLogFilePath = FilePathEdit(_gFilePath, _gFileName);

            if (!Directory.Exists(_gFilePath))
                Directory.CreateDirectory(_gFilePath);

            if (!File.Exists(_gLogFilePath))
            {
                sw = File.CreateText(_gLogFilePath);
                //sw = File.SetAttributes(_gLogFilePath)
                sw.Close();
            }
            
            //File.SetAttributes(_gLogFilePath, FileAttributes.ReadOnly);

            return _gLogFilePath;
        }

        public void LogWrite(string pModuleName, int line, string pinfo, string pMessage, string pPreFixFileName)
        {
            //log disable
            return;

            StreamWriter sw;
            string cm = ",";
            DateTime dt = DateTime.Now;

            if (gFileName == null)
                gFileName = pPreFixFileName;

            var fileFullPath = FileCreate(gFilePath, pPreFixFileName);

            clsUtil.Utl.DeleteLog(fileFullPath, logSavePeriod);
            sw = new StreamWriter(fileFullPath, true, System.Text.Encoding.GetEncoding(949));
            sw.WriteLine(dt.ToString("yyyy-MM-dd hh:mm:ss") + cm + pinfo + cm + pModuleName + cm + line + cm + pMessage);

            sw.Flush();
            sw.Close();
        }

    }
}
