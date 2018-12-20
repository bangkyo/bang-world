using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ComLib
{
    public class clsUtil
    {
        public static clsUtil Utl = new clsUtil();

        public static clsUtil GetInstance()
        {
            return Utl;
        }

        //public string INIPATH = Application.StartupPath + @"\HRB.ini";

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public string GetCurrTime(DateTime tm, short sw)
        {
            string rtn;
            //var dt = new DateTime();

            rtn = GetDateFormat(sw, tm);

            return rtn;
        }

        public string GetCurrTime(short sw)
        {
            var rtn = string.Empty;
            var dt = new DateTime();

            dt = DateTime.Now;

            rtn = GetDateFormat(sw, dt);

            return rtn;
        }

        public string GetCurrTime(short sw, string strDate)
        {
            string rtn = string.Empty;

            rtn = GetDateFormat(sw, strDate);

            return rtn;
        }

        public string GetDateFormat(short sw, DateTime tm)
        {
            string rtn = string.Empty;

            switch (sw)
            {
                case 1:
                    //YYYY-MM-DD hh:mm:ss
                    rtn = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", tm.Year, tm.Month, tm.Day,
                                        tm.Hour, tm.Minute, tm.Second);

                    break;
                case 2:
                    //MMDDhhmm
                    rtn = string.Format("{0:00}{1:00}{2:00}{3:00}", tm.Month, tm.Day, tm.Hour, tm.Minute);

                    break;
                case 3:
                    //YYMMDD
                    rtn = string.Format("{0:0000}{1:00}{2:00}", tm.Year, tm.Month, tm.Day);
                //case 3:
                //    //YYMMDD
                //    rtn = string.Format("{0:0000}{1:00}{2:00}", tm.Year % 100, tm.Month, tm.Day);

                    break;
                case 4:
                    //hhmmss
                    rtn = string.Format("{0:00}:{1:00}:{2:00}", tm.Hour, tm.Minute, tm.Second);

                    break;
                case 5:
                    //YYYY/MM/DD hh:mm:ss
                    rtn = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", tm.Year, tm.Month, tm.Day,
                                        tm.Hour, tm.Minute, tm.Second);

                    break;
                case 6:
                    //YYMMDDhhmmss
                    rtn = string.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", tm.Year%100, tm.Month, tm.Day, tm.Hour,
                                        tm.Minute, tm.Second);

                    break;
                case 7:
                    //MMDD
                    rtn = string.Format("{0:00}{1:00}", tm.Month, tm.Day);

                    break;
                case 8:
                    //hhmmss
                    rtn = string.Format("{0:00}{1:00}{2:00}", tm.Hour, tm.Minute, tm.Second);

                    break;
                case 9:
                    //YYYY/MM/DD hh:mm:ss.mms
                    rtn = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}.{6:00}", tm.Year, tm.Month, tm.Day,
                                        tm.Hour, tm.Minute, tm.Second, tm.Millisecond);

                    break;
                case 10:
                    //YYYYMMDDhhmmss
                    rtn = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", tm.Year, tm.Month, tm.Day, tm.Hour,
                                        tm.Minute, tm.Second);

                    break;
                case 11:
                    //YYYY-MM-DD
                    rtn = string.Format("{0:0000}-{1:00}-{2:00}", tm.Year, tm.Month, tm.Day);

                    break;
                case 12:
                    //YY-MM-DD hh:mm:ss.mms
                    rtn = string.Format("{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}", tm.Year%100, tm.Month,
                                        tm.Day, tm.Hour, tm.Minute, tm.Second, tm.Millisecond);

                    break;
                case 13:
                    //ss
                    rtn = string.Format("{0}", tm.Second);

                    break;
                case 14:
                    //YYYYMMDDhhmmssmss
                    rtn = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}", tm.Year, tm.Month, tm.Day,
                                        tm.Hour, tm.Minute, tm.Second, tm.Millisecond);

                    break;
                case 15:
                    //mm:ss
                    rtn = string.Format("{0:00}:{1:00}", tm.Minute, tm.Second);

                    break;
                default:
                    //MessageBox.Show(string.Format("정의되지 않은 인자[{0}] 입니다.", sw));

                    break;
            }

            return rtn;
        }

        // 01234567890123456
        // YYYYMMDDHHMMSSmms형식의 날짜 문자열을 입력받아 여러가지 형태로 변환하여 반환한다.
        public string GetDateFormat(short sw, string strDate)
        {
            string rtn = string.Empty;
            string strYear = null;
            string strMonth = null;
            string strDay = null;
            string strHour = null;
            string strMinute = null;
            string strSecond = null;
            string strMillisecond = null;

            strYear = strDate.Substring(0, 4);
            strMonth = strDate.Substring(4, 2);
            strDay = strDate.Substring(6, 2);
            strHour = strDate.Substring(8, 2);
            strMinute = strDate.Substring(10, 2);
            strSecond = strDate.Substring(12, 2);
            strMillisecond = strDate.Substring(14, 3);

            switch (sw)
            {
                case 1:
                    //YYYY-MM-DD hh:mm:ss
                    rtn = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", strYear, strMonth, strDay,
                                        strHour, strMinute, strSecond);

                    break;
                case 2:
                    //MMDDhhmm
                    rtn = string.Format("{0:00}{1:00}{2:00}{3:00}", strMonth, strDay, strHour, strMinute);

                    break;
                case 3:
                    //YYMMDD
                    rtn = string.Format("{0:00}/{1:00}/{2:00}", strYear.Substring(2, 2), strMonth, strDay);

                    break;
                case 4:
                    //hhmmss
                    rtn = string.Format("{0:00}:{1:00}:{2:00}", strHour, strMinute, strSecond);

                    break;
                case 5:
                    //YYYY/MM/DD hh:mm:ss
                    rtn = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", strYear, strMonth, strDay,
                                        strHour, strMinute, strSecond);

                    break;
                case 6:
                    //YYMMDDhhmmss
                    rtn = string.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", strYear.Substring(2, 2), strMonth,
                                        strDay, strHour, strMinute, strSecond);

                    break;
                case 7:
                    //MMDD
                    rtn = string.Format("{0:00}{1:00}", strMonth, strDay);

                    break;
                case 8:
                    //hhmmss
                    rtn = string.Format("{0:00}{1:00}{2:00}", strHour, strMinute, strSecond);

                    break;
                case 9:
                    //YYYY/MM/DD hh:mm:ss.mms
                    rtn = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}.{6:00}", strYear, strMonth, strDay,
                                        strHour, strMinute, strSecond, strMillisecond);

                    break;
                case 10:
                    //YYYYMMDDhhmmss
                    rtn = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", strYear, strMonth, strDay, strHour,
                                        strMinute, strSecond);

                    break;
                case 11:
                    //YYYY-MM-DD
                    rtn = string.Format("{0:0000}-{1:00}-{2:00}", strYear, strMonth, strDay);

                    break;
                case 12:
                    //YY-MM-DD hh:mm:ss.mms
                    rtn = string.Format("{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:00}", strYear.Substring(2, 2),
                                        strMonth, strDay, strHour, strMinute, strSecond, strMillisecond);

                    break;
                case 13:
                    //ss
                    rtn = string.Format("{0}", strSecond);

                    break;
                case 14:
                    //YYYYMMDDhhmmssmss
                    rtn = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}", strYear, strMonth, strDay,
                                        strHour, strMinute, strSecond, strMillisecond);

                    break;
                case 15:
                    //mm:ss
                    rtn = string.Format("{0:00}:{1:00}", strMinute, strSecond);

                    break;
                default:
                    //Interaction.MsgBox(string.Format("정의되지 않은 인자[{0}] 입니다.", sw));

                    break;
            }

            return rtn;
        }

        public string __Function()
        {
            StackTrace stackTrace = new StackTrace();
            return stackTrace.GetFrame(1).GetMethod().Name;
        }

        public int __Line()
        {
            StackTrace st = new StackTrace(new StackFrame(true));
            StackFrame sf = st.GetFrame(0);
            return sf.GetFileLineNumber();
        }

        public string __File()
        {
            StackTrace st = new StackTrace(new StackFrame(true));
            StackFrame sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public int __Column()
        {
            StackTrace st = new StackTrace(new StackFrame(true));
            StackFrame sf = st.GetFrame(0);
            return sf.GetFileColumnNumber();
        }

        public void DeleteLog(string path, int val)
        {
            // 폴더 존재유무 체크
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Exists == false) return;

            TimeSpan tGap = new TimeSpan();

            // 파일비교
            foreach (FileInfo fi in di.GetFiles())
            {
                // 확장자 비교
                if (fi.Extension != ".csv") continue;

                // 지정날짜이전생성(생성일자기준)로그를삭제합니다.
                tGap = DateTime.Now.Subtract(fi.CreationTime);

                if (tGap.Days >= val)
                {
                    fi.Delete();
                }
            }
        }

        //public string GetiniValue(string section, string key)
        //{
        //    string rtn = string.Empty;

        //    var temp = new StringBuilder(255);
        //    GetPrivateProfileString(section, key, "", temp, 255, INIPATH);
        //    rtn = temp.ToString();
        //    return rtn;
        //}

        //public bool SetiniValue(string section, string key, string val)
        //{
        //    bool rtn = false;

        //    rtn = WritePrivateProfileString(section, key, val, INIPATH);

        //    return rtn;
        //}

        public string GetLastString(string pStr, char pSpa, int cnt)
        {
            string rtn = string.Empty;

            string[] list = pStr.Split(pSpa);

            rtn = list[list.Length-1];

            return rtn;
        }

        public string test__File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }


    }
}