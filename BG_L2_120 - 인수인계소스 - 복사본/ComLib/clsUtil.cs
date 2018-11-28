using C1.Win.C1FlexGrid;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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

            switch(strDate.Trim().Length)
            {
                case 4:
                    strDate = strDate + "0101000000000";
                    break;
                case 6:
                    strDate = strDate + "01000000000";
                    break;
                case 8:
                    strDate = strDate + "000000000";
                    break;
                case 10:
                    strDate = strDate + "0000000";
                    break;
                case 12:
                    strDate = strDate + "00000";
                    break;
                case 14:
                    strDate = strDate + "000";
                    break;
            }

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


		//2017.07.07 OCJ 추가
        #region 둘중에 작은수 비교하여 더작은수를 반환하는 함수
        public float GetMinNumber(float fSourceNumber, float fDestNumber)
        {
            float fNumber = 0.0f;
            if(fSourceNumber < fDestNumber)
            {
                fNumber = fSourceNumber;
            }
            else
            {
                fNumber = fDestNumber;
            }

            return fNumber;
        }

        public double GetMinNumber(double fSourceNumber, double fDestNumber)
        {
            double fNumber = 0.0d;
            if (fSourceNumber < fDestNumber)
            {
                fNumber = fSourceNumber;
            }
            else
            {
                fNumber = fDestNumber;
            }

            return fNumber;
        }

        public int GetMinNumber(int fSourceNumber, int fDestNumber)
        {
            int fNumber = 0;
            if (fSourceNumber < fDestNumber)
            {
                fNumber = fSourceNumber;
            }
            else
            {
                fNumber = fDestNumber;
            }

            return fNumber;
        }
        #endregion

        #region 둘중에 작은수 비교하여 더 큰 수를 반환하는 함수
        public float GetMaxNumber(float fSourceNumber, float fDestNumber)
        {
            float fNumber = 0.0f;
            if (fSourceNumber > fDestNumber)
            {
                fNumber = fSourceNumber;
            }
            else
            {
                fNumber = fDestNumber;
            }

            return fNumber;
        }



        public double GetMaxNumber(double fSourceNumber, double fDestNumber)
        {
            double fNumber = 0.0d;
            if (fSourceNumber > fDestNumber)
            {
                fNumber = fSourceNumber;
            }
            else
            {
                fNumber = fDestNumber;
            }

            return fNumber;
        }



        public int GetMaxNumber(int fSourceNumber, int fDestNumber)
        {
            int fNumber = 0;
            if (fSourceNumber > fDestNumber)
            {
                fNumber = fSourceNumber;
            }
            else
            {
                fNumber = fDestNumber;
            }

            return fNumber;
        }
        #endregion


        public int GetCeiling(int nNumber, int nLength)
        {
            int Number = 0;

            double result = Convert.ToDouble(nNumber) / (Math.Pow(10, nLength));
            Number = Convert.ToInt32(Math.Ceiling(result) * (Math.Pow(10, nLength)));
            return Number;
        }


        public int GetTruncate(double floatNumber)
        {
            return Convert.ToInt32(Math.Truncate(floatNumber));
        }

        public decimal GetTruncate(decimal decimalNumber)
        {
            return Convert.ToDecimal(Math.Truncate(decimalNumber));
        }

        public double GetTruncate(double floatNumber, int floatPoint)
        {
            string strNumber = "";
            string strFormat = "N" + floatPoint;

            strNumber = floatNumber.ToString(strFormat);
            return Convert.ToDouble(strNumber);
        }

        public int GetTruncate(int nNumber, int nLength)
        {
            int Number = 0;

            double result = Convert.ToDouble(nNumber) / (Math.Pow(10, nLength));
            Number = Convert.ToInt32(Math.Truncate(result) * (Math.Pow(10, nLength)));
            return Number;
        }

        public int GetRound(int nNumber, int nPoint)
        {
            int Number = 0;

            double result = Convert.ToDouble(nNumber) / (Math.Pow(10, nPoint));
            Number = Convert.ToInt32(Math.Round(result, 0) * (Math.Pow(10, nPoint)));
            return Number;
        }

        public double GetRound(double floatNumber, int floatPoint = 0)
        {
            double Number = 0.00d;

            Number = Math.Round(floatNumber, floatPoint);
            return Number;
        }

        public object GetRoundFormat(double floatNumber, int floatPoint = 0, bool bPercent = false)
        {
            string strNumber = "";
            string strFormat = "N" + floatPoint;
            object objNumber = DBNull.Value;

            floatNumber = GetRound(floatNumber, floatPoint);
            strNumber = floatNumber.ToString(strFormat);

            if (bPercent)
            {
                if (!strNumber.Equals(""))
                    objNumber = strNumber + "%";
            }
            else
            {
                objNumber = ZeroPointToNull(strNumber);
            }

            return objNumber;
        }



        public void GridExcel(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            GridExcel(grdItem, "");
        }
        public void GridExcel(C1.Win.C1FlexGrid.C1FlexGrid grdItem, string strFileName)
        {
            DialogResult result;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            dlg.Filter = "Excel Files (*.xls)|*.xls|All Files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            dlg.FileName = strFileName.Replace("/", " ");

            result = dlg.ShowDialog();
            if (result != DialogResult.OK) return;

            grdItem.SaveExcel(dlg.FileName, "", FileFlags.SaveMergedRanges);

            result = MessageBox.Show("  저장된 Excel File을 열겠습니까? ", "Excel File Open 확인",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (!File.Exists(dlg.FileName))
                {
                    MessageBox.Show("File not found!!!", "Error");
                    return;
                }
                System.Diagnostics.Process.Start(dlg.FileName);
            }
        }


        public int StringToInt(string Value, string[] Token)
        {
            int nValue = 0;
            string strValue = "";
            
            for(int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (strValue.Equals(""))
            {
                nValue = 0;
            }
            else
            {
                nValue = Convert.ToInt32(strValue);
            }

            return nValue;
        }

        public double StringToDouble(string Value, string[] Token)
        {
            double nValue = 0;
            string strValue = "";

            for (int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (strValue.Equals(""))
            {
                nValue = 0;
            }
            else
            {
                nValue = Convert.ToDouble(strValue);
            }

            return nValue;
        }

        

        public int StringToInt(string Value)
        {
            int nValue = 0;
            string strValue = "";
            string[] Token = { ",", "%" };

            for (int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (strValue.Equals(""))
            {
                nValue = 0;
            }
            else
            {
                nValue = Convert.ToInt32(strValue);
            }

            return nValue;
        }

        public double StringToDouble(string Value)
        {
            double nValue = 0;
            string strValue = "";
            string[] Token = { ",", "%" };

            for (int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (strValue.Equals(""))
            {
                nValue = 0;
            }
            else
            {
                nValue = Convert.ToDouble(strValue);
            }

            return nValue;
        }

        public object StringToInt(string Value, string[] Token, bool bNull)
        {
            object nValue = 0;
            string strValue = "";

            for (int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (bNull)
            {
                if (strValue.Equals(""))
                {
                    nValue = null;
                }
                else
                {
                    nValue = Convert.ToInt32(strValue);
                }
            }
            else
            {
                if (strValue.Equals(""))
                {
                    nValue = 0;
                }
                else
                {
                    nValue = Convert.ToInt32(strValue);
                }
            }

            return nValue;
        }

        public object StringToDouble(string Value, string[] Token, bool bNull)
        {
            object nValue = 0;
            string strValue = "";

            for (int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (bNull)
            {
                if (strValue.Equals(""))
                {
                    nValue = null;
                }
                else
                {
                    nValue = Convert.ToDouble(strValue);
                }
            }
            else
            {
                if (strValue.Equals(""))
                {
                    nValue = 0.00f;
                }
                else
                {
                    nValue = Convert.ToInt32(strValue);
                }
            }

            return nValue;
        }

        public object StringToInt(string Value, bool bNull)
        {
            object nValue = 0;
            string strValue = "";
            string[] Token = { ",", "%" };

            for (int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (bNull)
            {
                if (strValue.Equals(""))
                {
                    nValue = null;
                }
                else
                {
                    nValue = Convert.ToInt32(strValue);
                }
            }
            else
            {
                if (strValue.Equals(""))
                {
                    nValue = 0;
                }
                else
                {
                    nValue = Convert.ToInt32(strValue);
                }
            }

            return nValue;
        }

        public object StringToDouble(string Value, bool bNull)
        {
            object nValue = 0;
            string strValue = "";
            string[] Token = { ",", "%" };

            for (int i = 0; i < Token.Length; i++)
            {
                Value = Value.Replace(Token[i], "").Trim();
            }

            strValue = Value;

            if (bNull)
            {
                if (strValue.Equals(""))
                {
                    nValue = null;
                }
                else
                {
                    nValue = Convert.ToDouble(strValue);
                }
            }
            else
            {
                if (strValue.Equals(""))
                {
                    nValue = 0.00f;
                }
                else
                {
                    nValue = Convert.ToDouble(strValue);
                }
            }

            return nValue;
        }

        public string InitDateTimeLabelX(int StartYear, int EndYear, ref int Month, ref bool bSum)
        {
            DateTime startDateTime = new DateTime(StartYear, Month, 1);
            DateTime endDateTime = new DateTime(EndYear, 12, 1);

            string strXLabel = startDateTime.ToString("yyyy");

            if (startDateTime.Year == endDateTime.Year)
            {
                if(bSum)
                {
                    strXLabel = startDateTime.ToString("yyyy");
                    bSum = false;
                }
                else
                {
                    if (startDateTime.Month == 1)
                    {
                        strXLabel = startDateTime.ToString("yyyy.MM");
                    }
                    else
                    {
                        strXLabel = startDateTime.ToString("MM");
                    }
                    startDateTime = startDateTime.AddMonths(1);
                    Month = startDateTime.Month;
                }
            }
            else
            {
                strXLabel = startDateTime.ToString("yyyy");
            }

            return strXLabel.Trim();
        }

        public string DateTimeToString(int Year, int Month, int Day, string Format)
        {
            DateTime startDateTime = new DateTime(Year, Month, 1);

            string strXLabel = startDateTime.ToString(Format);

            return strXLabel.Trim();
        }

        public object ZeroPointToNull(object Value)
        {
            object obj = null;

            if (Value == null)
            {
                return obj;
            }

            if (Convert.ToBoolean(Value) == false)
            {
                obj = DBNull.Value;
            }
            else
            {
                obj = Value;
            }
            

            return obj;
        }

        public object ZeroPointToNull(string Value)
        {
            object obj = null;

            if (Value == null)
            {
                return obj;
            }

            if (Convert.ToDouble(Value) == 0)
            {
                obj = DBNull.Value;
            }
            else
            {
                obj = Value;
            }


            return obj;
        }

        public int GetAxisMaxByChart(int nMax, int Step)
        {
            int nRtn = 0;

            if (nMax % Step == 0)
            {
                nRtn = nMax ;
            }
            else
            {
                double dRate = (nMax % Step) / Convert.ToDouble(Step.ToString("#.##"));

                if (dRate > 0.6f)
                {
                    double nRate_ = Math.Round(Convert.ToDouble(nMax / Convert.ToDouble(Step.ToString("#.##"))), 0) + 1;
                    nRtn = (int) nRate_ * Step;
                }
                else
                {
                    while(nMax % Step != 0)
                    {
                        nMax++;
                    }

                    nRtn = nMax;
                }
            }

            return nRtn;
        }
        
        #region #. CheckNumber
        /// <summary>
        /// 숫자체크
        /// </summary>
        /// <param name="letter">문자
        /// 
        public static bool IsNumber(string letter)
        {
            bool IsCheck = true;

            int numChk = 0;
            bool isNum = int.TryParse(letter, out numChk);
            if (!isNum)
            {
                IsCheck = false;
            }

            return IsCheck;
        }
        #endregion
        //------------------
    }
}