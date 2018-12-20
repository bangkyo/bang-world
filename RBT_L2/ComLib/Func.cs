using C1.Win.C1FlexGrid;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;

namespace ComLib
{
    public class VbFunc
    {
        string strRet = "";
        int intRet;

        ConnectDB cd = new ConnectDB();

        public string UCase(string str)
        {
            strRet = "";
            if (str != null && str != "")
            {
                strRet = str.Trim().ToUpper();
            }
            return strRet;
        }

        public string UCase(object obj)
        {
            strRet = "";
            if (obj != null && obj.ToString() != "")
            {
                strRet = obj.ToString().Trim().ToUpper();
            }
            return strRet;
        }

        public string LCase(string str)
        {
            strRet = "";
            if (str != null && str != "")
            {
                strRet = str.Trim().ToLower();
            }
            return strRet;
        }

        public string LCase(object obj)
        {
            strRet = "";
            if (obj != null && obj.ToString() != "")
            {
                strRet = obj.ToString().Trim().ToLower();
            }
            return strRet;
        }

        public int Len(string str)
        {
            intRet = 0;
            if (str != null && str != "")
            {
                intRet = str.Trim().Length;
            }
            return intRet;
        }

        public int Len(object obj)
        {
            intRet = 0;
            if (obj != null && obj.ToString() != "")
            {
                intRet = obj.ToString().Trim().Length;
            }
            return intRet;
        }

        /// <summary>
        /// 지정한 배열의 상한선을 int로 반환
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int UBound(string[] str)
        {
            intRet = 0;
            if (str != null && str.Length > 0)
            {
                intRet = str.GetUpperBound(0);
            }
            return intRet;
        }

        public int UBound(byte[,] str)
        {
            intRet = 0;
            if (str != null && str.Length > 0)
            {
                intRet = str.GetUpperBound(0);
            }
            return intRet;
        }

        public int UBound(float[] str)
        {
            intRet = 0;
            if (str != null && str.Length > 0)
            {
                intRet = str.GetUpperBound(0);
            }
            return intRet;
        }

        public int LBound(string[] str)
        {
            intRet = 0;
            if (str != null && str.Length > 0)
            {
                intRet = str.GetLowerBound(0);
            }
            return intRet;
        }

        public string RTrim(string str)
        {
            strRet = "";
            if (str != null && str != "")
            {
                strRet = str.TrimEnd();
            }
            return strRet;
        }

        public string RTrim(object obj)
        {
            strRet = "";
            if (obj != null && obj.ToString() != "")
            {
                strRet = obj.ToString().TrimEnd();
            }
            return strRet;
        }

        public string LTrim(string str)
        {
            strRet = "";
            if (str != null && str != "")
            {
                strRet = str.TrimStart();
            }
            return strRet;
        }

        public string LTrim(object obj)
        {
            strRet = "";
            if (obj != null && obj.ToString() != "")
            {
                strRet = obj.ToString().TrimStart();
            }
            return strRet;
        }

        public string Trim(string str)
        {
            strRet = "";
            if (str != null && str != "")
            {
                strRet = str.Trim();
            }
            return strRet;
        }

        public string Trim(object obj)
        {
            strRet = "";
            if (obj != null && obj.ToString() != "")
            {
                strRet = obj.ToString().Trim();
            }
            return strRet;
        }

        public int CLng(float value)
        {
            int iRet = 0;

            if (IsNumeric(value))
            {
                iRet = Convert.ToInt16(Math.Round(value));
            }
            return iRet;
        }

        public int CLng(double value)
        {
            int iRet = 0;

            if (IsNumeric(value))
            {
                iRet = Convert.ToInt16(Math.Round(value));
            }
            return iRet;
        }

        public int CLng(object value)
        {
            int iRet = 0;

            if (value != null && IsNumeric(value))
            {
                iRet = Convert.ToInt32(Math.Round(double.Parse(value.ToString().Trim())));
            }
            return iRet;
        }

        /// <summary>
        /// double타입으로 변환
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double CDbl(string value)
        {
            double dbRet = 0;
            if (value != null && IsNumeric(value))
            {
                dbRet = Convert.ToDouble(value.Trim());
            }
            return dbRet;
        }

        public double CDbl(object value)
        {
            double dbRet = 0;
            if (value != null && IsNumeric(value))
            {
                dbRet = Convert.ToDouble(value.ToString().Trim());
            }
            return dbRet;
        }

        /// <summary>
        /// int타입으로 변환
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int CInt(string value)
        {
            int iRet = 0;
            if (value != null && IsNumeric(value))
            {
                iRet = Convert.ToInt16(Math.Floor(double.Parse(value.Trim())));
            }
            return iRet;
        }

        public int CInt2(string value)
        {
            int iRet = 0;
            if (value != null && IsNumeric(value))
            {
                iRet = Convert.ToInt32(Math.Floor(double.Parse(value.Trim())));
            }
            return iRet;
        }

        public int CInt(object value)
        {
            int iRet = 0;
            if (value != null && IsNumeric(value))
            {
                iRet = Convert.ToInt16(value.ToString().Trim());
            }
            return iRet;
        }

        /// <summary>
        /// float타입으로 변환
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float CSng(string value)
        {
            float fRet = 0.0f;
            if (value != null && IsNumeric(value))
            {
                fRet = Convert.ToSingle(value.Trim());
            }
            return fRet;
        }

        public float CSng(object value)
        {
            float fRet = 0.0f;
            if (value != null && IsNumeric(value))
            {
                fRet = Convert.ToSingle(value.ToString().Trim());
            }
            return fRet;
        }

        public float CSng(double value)
        {
            float fRet = 0.0f;
            if (IsNumeric(value))
            {
                fRet = Convert.ToSingle(value);
            }
            return fRet;
        }

        //Mid    
        /// <summary>    
        /// 문자열 원본의 지정한 위치에서 부터 추출할 갯수 만큼 문자열을 가져옵니다.    
        /// </summary>    
        /// <param name="sString">문자열 원본</param>   
        /// <param name="nStart">추출을 시작할 위치</param>   
        /// <returns>추출된 문자열</returns>    
        public string Mid(string sString, int nStart)
        {
            int nLength = sString.Length;
            string sReturn;

            if (nStart == 1) nStart = 0;

            //VB에서 문자열의 시작은 0이 아니므로 같은 처리를 하려면         
            //스타트 위치를 인덱스로 바꿔야 하므로 -1을 하여        
            //1부터 시작하면 0부터 시작하도록 변경하여 준다.        --nStart;         
            //시작위치가 데이터의 범위를 안넘겼는지?        
            if (nStart <= sString.Length)
            {
                //안넘겼다.             
                //필요한 부분이 데이터를 넘겼는지?            
                if ((nStart + nLength) <= sString.Length)
                {
                    //안넘겼다.                
                    sReturn = sString.Substring(nStart, nLength);
                }
                else
                {
                    //넘겼다.                                     
                    //데이터 끝까지 출력                
                    sReturn = sString.Substring(nStart);
                }
            }
            else
            {
                //넘겼다.             
                //그렇다는 것은 데이터가 없음을 의미한다.            
                sReturn = string.Empty;
            }
            return sReturn;
        }

        //Mid    
        /// <summary>    
        /// 문자열 원본의 지정한 위치에서 부터 추출할 갯수 만큼 문자열을 가져옵니다.    
        /// </summary>    
        /// <param name="sString">문자열 원본</param>   
        /// <param name="nStart">추출을 시작할 위치</param>   
        /// <param name="nLength">추출할 갯수</param>    
        /// <returns>추출된 문자열</returns>    
        public string Mid(string sString, int nStart, int nLength)
        {
            string sReturn;
            //VB에서 문자열의 시작은 0이 아니므로 같은 처리를 하려면         
            //스타트 위치를 인덱스로 바꿔야 하므로 -1을 하여        
            nStart = nStart - 1;
            //1부터 시작하면 0부터 시작하도록 변경하여 준다.        --nStart;         
            //시작위치가 데이터의 범위를 안넘겼는지?    
            if (nStart <= sString.Length)
            {
                //안넘겼다.             
                //필요한 부분이 데이터를 넘겼는지?            
                if ((nStart + nLength) <= sString.Length)
                {
                    //안넘겼다.                
                    sReturn = sString.Substring(nStart, nLength);
                }
                else
                {
                    //넘겼다.                                     
                    //데이터 끝까지 출력                
                    sReturn = sString.Substring(nStart);
                }
            }
            else
            {
                //넘겼다.             
                //그렇다는 것은 데이터가 없음을 의미한다.            
                sReturn = string.Empty;
            }
            return sReturn;
        }

        //Mid    
        /// <summary>    
        /// (사용안함) 문자열 원본의 지정한 위치에서 부터 추출할 갯수 만큼 문자열을 가져옵니다.    
        /// </summary>    
        /// <param name="sString">문자열 원본</param>   
        /// <param name="nStart">추출을 시작할 위치</param>   
        /// <param name="nLength">추출할 갯수</param>    
        /// <returns>추출된 문자열</returns>    
        public string MidB(byte[] btValue, int nStart, int nLength)
        {
            string sReturn = "";
            //VB에서 문자열의 시작은 0이 아니므로 같은 처리를 하려면         
            //스타트 위치를 인덱스로 바꿔야 하므로 -1을 하여        
            //1부터 시작하면 0부터 시작하도록 변경하여 준다.        --nStart;         
            //시작위치가 데이터의 범위를 안넘겼는지?    

            //if (nStart <= btValue.Length)
            //{
            //    //안넘겼다.             
            //    //필요한 부분이 데이터를 넘겼는지?            
            //    if ((nStart + nLength) <= btValue.Length)
            //    {
            //        //안넘겼다.                
            //        sReturn = btValue.Substring(nStart, nLength);
            //    }
            //    else
            //    {
            //        //넘겼다.                                     
            //        //데이터 끝까지 출력                
            //        sReturn = btValue.Substring(nStart);
            //    }
            //}
            //else
            //{
            //    //넘겼다.             
            //    //그렇다는 것은 데이터가 없음을 의미한다.            
            //    sReturn = string.Empty;
            //}
            return sReturn;
        }

        //Left    
        /// <summary>    
        /// 문자열 원본에서 왼쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.    
        /// </summary>    /// <param name="sString">문자열 원본</param>    
        /// <param name="nLength">추출할 갯수</param>    
        /// <returns>추출된 문자열</returns>    
        public string Left(string sString, int nLength)
        {
            string sReturn = "";
            if (sString != null && sString != "")
            {
                //추출할 갯수가 문자열 길이보다 긴지?        
                if (nLength > sString.Length)
                {
                    //길다!             
                    //길다면 원본의 길이만큼 리턴해 준다.            
                    nLength = sString.Length;
                }

                //문자열 추출        
                sReturn = sString.Substring(0, nLength);
            }
            return sReturn;
        }

        //Right    
        /// <summary>    
        /// 문자열 원본에서 오른쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.    
        /// </summary>    
        /// <param name="sString">문자열 원본</param>    
        /// <param name="nLength">추출할 갯수</param>    
        /// <returns>추출된 문자열</returns>    
        public string Right(string sString, int nLength)
        {
            string sReturn = "";
            if (sString != null && sString != "")
            {
                //추출할 갯수가 문자열 길이보다 긴지?        
                if (nLength > sString.Length)
                {
                    //길다!             
                    //길다면 원본의 길이만큼 리턴해 준다.            
                    nLength = sString.Length;
                }

                //문자열 추출        
                sReturn = sString.Substring(sString.Length - nLength, nLength);
            }
            return sReturn;
        }

        /// <summary>
        /// 숫자여부를 체크한다.(숫자이면 true 아니면 false)
        /// </summary>
        /// <param name="stringToTest">확인할 문자</param>
        /// <returns>bool</returns>
        public Boolean IsNumeric(string stringToTest)
        {
            double result;

            try
            {
                if (double.TryParse(stringToTest, out result))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 숫자여부를 체크한다.(숫자이면 true 아니면 false)
        /// </summary>
        /// <param name="stringToTest">확인할 object</param>
        /// <returns>bool</returns>
        public Boolean IsNumeric(object oToTest)
        {
            double result;
            try
            {
                if (double.TryParse(oToTest.ToString(), out result))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int IIf(bool bValue, int a, int b)
        {
            int iRet = 0;
            iRet = (bValue ? a : b);
            return iRet;
        }

        public float IIf(bool bValue, float a, float b)
        {
            float fRet = 0f;
            fRet = (bValue ? a : b);
            return fRet;
        }

        public double IIf(bool bValue, double a, double b)
        {
            double dbRet = 0;
            dbRet = (bValue ? a : b);
            return dbRet;
        }

        public string IIf(bool bValue, string a, string b)
        {
            string strRet = "";
            strRet = (bValue ? a : b);
            return strRet;
        }

        public bool IIf(bool bValue, bool a, bool b)
        {
            bool strRet = false;
            strRet = (bValue ? a : b);
            return strRet;
        }

        /// <summary>
        /// 날짜 타입이 맞는지 확인 맞으면 True
        /// </summary>
        /// <param name="sdate">날짜형태 문자</param>
        /// <returns></returns>
        public bool IsDate(string sdate)
        {
            DateTime dt;
            bool isDate = true;

            try
            {
                dt = DateTime.Parse(sdate);
            }
            catch
            {
                isDate = false;
            }
            return isDate;
        }

        /// <summary>
        /// 시간 타입이 맞는지 확인 맞으면 True
        /// </summary>
        /// <param name="stime"></param>
        /// <returns></returns>
        public bool ISTIME(string stime)
        {
            DateTime dt;
            bool isTime = true;

            try
            {
                dt = DateTime.Parse("2014-04-07 " + stime);
            }
            catch
            {
                isTime = false;
            }
            return isTime;
        }

        /// <summary>
        /// 포멧설정
        /// </summary>
        /// <param name="strValue">변환 값</param>
        /// <param name="setFormat">Format</param>
        /// <returns></returns>
        public string Format(string strValue, string setFormat)
        {
            string strRet = "";
            if (!IsNull(strValue))
            {
                if (IsNumeric(strValue))
                {
                    strRet = String.Format("{0:" + setFormat + "}", double.Parse(strValue));
                }
                else
                    strRet = String.Format(setFormat, strValue);
            }
            return strRet;
        }

        public string Format(DateTime dtValue, string setFormat)
        {
            return dtValue.ToString(setFormat);
        }

        public string Format(int iValue, string setFormat)
        {
            return String.Format("{0:" + setFormat + "}", iValue);
        }

        public string Format(double dbValue, string setFormat)
        {
            string strRet = String.Format("{0:" + setFormat + "}", dbValue);
            return strRet;
        }

        public string Format(object obValue, string setFormat)
        {
            string strRet = "";

            if (!IsNull(obValue))
            {
                Type tp = obValue.GetType();
                if (tp == typeof(string))
                {
                    if (IsNumeric(obValue))
                    {
                        strRet = String.Format("{0:" + setFormat + "}", double.Parse(obValue.ToString()));
                    }
                    else
                        strRet = String.Format(setFormat, obValue);
                }
                else if (tp == typeof(decimal)) strRet = String.Format("{0:" + setFormat + "}", obValue);
                else if (tp == typeof(int)) strRet = String.Format("{0:" + setFormat + "}", obValue);
                else if (tp == typeof(float)) strRet = String.Format("{0:" + setFormat + "}", obValue);
                else if (tp == typeof(double)) strRet = String.Format("{0:" + setFormat + "}", obValue);
                else if (tp == typeof(DateTime))
                {
                    DateTime dTime = DateTime.Parse(obValue.ToString());
                    strRet = dTime.ToString(setFormat);
                }
            }

            return strRet;
        }

        /// <summary>
        /// DateTime 으로 변환
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public DateTime CDate(string strDate)
        {
            return DateTime.Parse(strDate);
        }

        public DateTime CDate(string strDate, string format)
        {
            return DateTime.ParseExact(strDate, format, null);
        }

        /// <summary>
        /// DateTime 으로 변환
        /// </summary>
        /// <param name="objDate"></param>
        /// <returns></returns>
        public DateTime CDate(object objDate)
        {
            return DateTime.Parse(objDate.ToString());
        }

        /// <summary>
        /// 널체크
        /// </summary>
        /// <param name="strValu"></param>
        /// <returns></returns>
        public bool IsNull(string strValu)
        {
            if (strValu == null || strValu == "")
                return true;
            else
                return false;
        }

        /// <summary>
        ///  널체크
        /// </summary>
        /// <param name="objValu"></param>
        /// <returns></returns>
        public bool IsNull(object objValu)
        {
            if (objValu == null || objValu.ToString() == "")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 공백만들기
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string Space(int i)
        {
            return new String(' ', i);
        }

        /// <summary>
        /// 같은 문자를 갯수만큼 생성한다.
        /// </summary>
        /// <param name="i">변환문자갯수</param>
        /// <param name="setChr">변환문자</param>
        /// <returns></returns>
        public string vfString(int i, char setChr)
        {
            return new String(setChr, i);
        }

        /// <summary>
        /// StrConv(TmpStr, vbFromUnicode) ==> StrConv(TmpStr)
        /// </summary>
        /// <param name="strValue">변환할값</param>
        /// <returns>char[]</returns>
        public byte[] StrConv(string strValue)
        {
            System.Text.UnicodeEncoding unicode = new System.Text.UnicodeEncoding();
            byte[] bytRet = unicode.GetBytes(strValue);

            return bytRet;
        }

        /// <summary>
        /// StrConv(TmpStr, vbUnicode) ==> StrConv2(TmpStr)
        /// </summary>
        /// <param name="strValue">변환할값</param>
        /// <returns>char[]</returns>
        public string StrConv2(byte[] bytValue)
        {
            System.Text.UnicodeEncoding unicode = new System.Text.UnicodeEncoding();
            strRet = unicode.GetString(bytValue);

            return strRet;
        }

        public int LenB(byte[] chValue)
        {
            int iRet = 0;
            iRet = chValue.Length;

            return iRet;
        }

        public int InStr(int iStart, string strValue, string chkStr)
        {
            //VB에서 문자열의 시작은 0이 아니므로 같은 처리를 하려면         
            //스타트 위치를 인덱스로 바꿔야 하므로 -1을 하여        
            //1부터 시작하면 0부터 시작하도록 변경하여 준다.
            if (iStart == 0) iStart = 1;
            iStart = iStart - 1;
            int iRet = strValue.IndexOf(chkStr, iStart);
            return iRet + 1;
        }

        public double VAL(string p_Val)
        {
            if (p_Val.Trim() == "")
                return 0;

            if (IsNumeric(p_Val)) return Convert.ToDouble(p_Val.ToString());

            char[] cRec = p_Val.ToCharArray();
            string sNum = string.Empty;
            for (int i = 0; i < cRec.Length; i++)
            {
                if (char.IsNumber(cRec[i]))
                    sNum += cRec[i];
                else
                {
                    if (sNum != "")
                    {
                        try
                        {
                            return Convert.ToDouble(sNum);
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                    return 0;
                }
            }
            return 0;
        }

        public double VAL(object o_Val)
        {
            if (o_Val == null || o_Val.ToString().Trim() == "")
                return 0.0f;

            string p_Val = o_Val.ToString();

            if (IsNumeric(p_Val)) return Convert.ToDouble(p_Val.ToString());

            char[] cRec = p_Val.ToCharArray();
            string sNum = string.Empty;
            for (int i = 0; i < cRec.Length; i++)
            {
                if (char.IsNumber(cRec[i]))
                    sNum += cRec[i];
                else
                {
                    if (sNum != "")
                    {
                        try
                        {
                            return Convert.ToDouble(sNum);
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                    return 0;
                }
            }
            return 0;
        }

        public string BoolToString(bool bl)
        {
            //string rtn = string.Empty;
            if (bl)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
        }

        public string ChangeString(string str, string fromStr, string toStr)
        {
            string rtn = string.Empty;


            rtn = str.Replace(fromStr, toStr);

            return rtn;
        }

        public bool StringToBool(string str)
        {
            if (str == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string StringToString(string str)
        {
            string rtn = string.Empty;
            if (str == "True")
            {
                rtn = "Y";
            }
            else if (str == "False")
            {
                rtn = "N";
            }
            return rtn;
        }

        public bool IsCapsLock()
        {
            bool rtn = false;
            rtn = Control.IsKeyLocked(Keys.CapsLock);

            return rtn;
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public void On_CapsLock()
        {
            if (!Control.IsKeyLocked(Keys.CapsLock)) // Checks Capslock is off -> on
            {
                const int KEYEVENTF_EXTENDEDKEY = 0x1;
                const int KEYEVENTF_KEYUP = 0x2;
                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
                (UIntPtr)0);
            }
        }
        /// <summary>
        /// 전체, 점검, 미점검 설정을 위한 리스트 생성
        /// </summary>
        /// <returns></returns>
        public ArrayList GetCheckList()
        {
            ArrayList arrType1 = new ArrayList();

            arrType1.Add(new DictionaryList("전체", "%"));
            arrType1.Add(new DictionaryList("점검", "Y"));
            arrType1.Add(new DictionaryList("미점검", "N"));

            return arrType1;
        }

        public ArrayList GetReWorkList()
        {
            ArrayList arrType1 = new ArrayList();

            arrType1.Add(new DictionaryList("Y", "Y"));
            arrType1.Add(new DictionaryList("N", "N"));

            return arrType1;
        }

        public  IEnumerable<Control> GetAllChildrens(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(c => GetAllChildrens(c))
              .Concat(controls);
        }

        public void SaveExcel(string titleNM, C1.Win.C1FlexGrid.C1FlexGrid selectedGrd)
        {
            if (selectedGrd == null)
            {
                return;
            }

            var dlg = new SaveFileDialog();

            dlg.DefaultExt = "xlsx";
            dlg.FileName = titleNM + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                //selectedGrd.SaveGrid(dlg.FileName, FileFormatEnum.Excel, FileFlags.IncludeFixedCells);
                selectedGrd.SaveGrid(dlg.FileName, FileFormatEnum.Excel, FileFlags.OpenXml| FileFlags.SaveMergedRanges|FileFlags.IncludeFixedCells);
                //selectedGrd.SaveGrid(dlg.FileName, FileFormatEnum.Excel, FileFlags.SaveMergedRanges | FileFlags.IncludeFixedCells);

                if (MessageBox.Show("저장한 Excel File을 여시겠습니까?", "Excel File Open", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("excel.exe", "\"" + dlg.FileName + "\"");
                }
            }
        }

        public void MakeExcelByTemplete()
        {
            Microsoft.Office.Interop.Excel.Application oAppln;
            Excel.Workbook oWorkBook;
            Excel.Worksheet oWorkSheet;
            Excel.Range oRange;

            //templete에서 파일을 이름을 받아 생성한다.
            //datatable을 가져와서 위치에 따라 content를 뿌린다.

            oAppln = new Excel.Application();
            oWorkBook = (oAppln.Workbooks.Add(true));
            oWorkSheet = (Excel.Worksheet)oWorkBook.ActiveSheet;


            oRange = oWorkSheet.get_Range("A1", "IV1");
            oRange.EntireColumn.AutoFit();
            oAppln.UserControl = false;

            string Path = System.Windows.Forms.Application.StartupPath.ToString();//이미지 파일 저장위치
            String Time = System.DateTime.Now.ToString("yyyyMMddHHmmss");

            string Fromfile = "D:\\작업방\\Boso3.0\\Report\\" + Time + ".xls";

            // Excel에 Chart 그리기

            int a = 1; //홀수 Chart 위치 변수
            int b = 2; //짝수 Chart 위치 변수

            //for (int i = 0; i < Make_ChartCount; i++)
            //{
            //    string filename = "D:\\작업방\\Boso3.0\\Image\\" + Time + " .bmp";

            //    System.IO.MemoryStream stream = new System.IO.MemoryStream();

            //    //Chart_Select[i].ExportImageSize = new System.Drawing.Size(550, 150);
            //    //Chart_Select[i].Export(FileFormat.Bitmap, stream);

            //    Image _Image = Image.FromStream(stream);
            //    _Image.Save(filename);

            //    //oWorkSheet.Cells[1, 4].Font.Size = 20;
            //    oWorkSheet.Cells[1, 4] = "검색보고서";
            //    //oWorkSheet.Cells[2, 3] = "(" + Start_time + "~" + End_time + ") ";
            //    oWorkSheet.get_Range("A1", "IV1");

            //}
            oWorkBook.SaveAs(Fromfile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);

            oAppln.Visible = true;
        }

        Excel.Application oAppln;
        Excel.Workbook oWorkBook;
        Excel.Worksheet oWorkSheet;
        Excel.Range oRange;

        string sourcepath = Application.StartupPath + @"\excel_Temp\";
        string targetpath = Application.StartupPath + @"\report\";
        private DataTable temp_min_max;

        public void MakeExcelByTemplete1MPI(string line_gp,string titleNM, DateTime search_date, C1FlexGrid grd)
        {
            DataRow[] result = null;
            DataTable dt = (DataTable)grd.DataSource;
            string templateName = "MPI조업일보";
            //..\..\excel\

            string filetype = ".xlsx";

            string TargetFileFullName = targetpath + titleNM + "_" + Format(search_date, "yyyyMMdd") + filetype;

            string sourceFileFullName = sourcepath + templateName + filetype;

            // report 폴더 없으면생성
            DirectoryInfo di = new DirectoryInfo(targetpath);

            if (di.Exists == false)

            {

                di.Create();

            }

            CopyFile(sourceFileFullName, TargetFileFullName);

            // Get the Excel application object.
            oAppln = new Excel.Application();

            // Make Excel visible (optional).
            oAppln.Visible = true;

            oWorkBook = (oAppln.Workbooks.Add(TargetFileFullName));
            oWorkSheet = (Excel.Worksheet)oWorkBook.ActiveSheet;

            //oRange = oWorkSheet.get_Range("A1", "O31");
            //oRange.EntireColumn.AutoFit();
            //oAppln.UserControl = false;

            //Excel.Shape shp = oWorkSheet.Activate.re

            int work_date_rowIndex = 4;

            int work_type_1_rowIndex = 7;
            int work_type_1_sum_rowIndex = 14;
            int work_type_2_rowIndex = 15;
            int work_type_2_sum_rowIndex = 22;
            int work_type_3_rowIndex = 23;
            int work_type_3_sum_rowIndex = 30;
            int work_type_total_sum_rowIndex = 31;

            int data_start_cols_index = 2;
            int dt_start_data_index = 2;
            // datarow에서 5번째부터 계정보가들어있슴.
            int dt_start_sum_data_index = 5;
            int sum_col_index = 5;

            string title = "No. " + line_gp.Substring(1, 1) + "MPI조업일보";
            oWorkSheet.Cells[2, 3] = title;

            string work_date = string.Format("작업일자 : {0} 년 {1} 월 {2} 일", search_date.Year, search_date.Month, search_date.Day);
            oWorkSheet.Cells[work_date_rowIndex, 1] = work_date;

            // 근에 따른 구분
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '1근'  AND ISNULL(ITEM_SIZE, ' ') <> '소계' AND ISNULL(ITEM_SIZE, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //1근좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_data_index, work_type_1_rowIndex, data_start_cols_index);
                //귀현수정
                work_type_1_rowIndex++;

            }
            // 1근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '1근'  AND ISNULL(ITEM_SIZE, ' ') = '소계' AND ISNULL(ITEM_SIZE, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //1근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_sum_data_index, work_type_1_sum_rowIndex, sum_col_index);
            }

            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '2근'  AND ISNULL(ITEM_SIZE, ' ') <> '소계' AND ISNULL(ITEM_SIZE, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //2근좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_data_index, work_type_2_rowIndex, data_start_cols_index);
                work_type_2_rowIndex++;
            }

            // 2근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '2근'  AND ISNULL(ITEM_SIZE, ' ') = '소계' AND ISNULL(ITEM_SIZE, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //2근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_sum_data_index, work_type_2_rowIndex, sum_col_index);
            }

            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '3근'  AND ISNULL(ITEM_SIZE, ' ') <> '소계' AND ISNULL(ITEM_SIZE, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //3근 데이터 좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_data_index, work_type_3_rowIndex, data_start_cols_index);
                work_type_3_rowIndex++;
            }

            //3근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '3근'  AND ISNULL(ITEM_SIZE, ' ') = '소계' AND ISNULL(ITEM_SIZE, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //3근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_sum_data_index, work_type_3_sum_rowIndex, sum_col_index);

            }

            //총근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '계'");//  AND ISNULL(ITEM_SIZE, ' ') <> '소계' AND ISNULL(ITEM_SIZE, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //총근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_sum_data_index, work_type_total_sum_rowIndex, sum_col_index);

            }

            //oWorkBook.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);

            oAppln.Visible = true;
        }

        public void MakeExcelByTempleteRun(string line_gp, string titleNM, DateTime search_date, C1FlexGrid grd)
        {
            DataRow[] result = null;
            DataTable dt = ((DataTable)grd.DataSource).Copy();
            string templateName = "가동속보";
            //..\..\excel\

            dt.Columns.Remove("STOP_RSN_CD");
            dt.Columns.Remove("STOP_GP");
            dt.Columns.Remove("GUBUN");

            string filetype = ".xlsx";

            string TargetFileFullName = targetpath + titleNM + "_" + Format(search_date, "yyyyMMdd") + filetype;

            string sourceFileFullName = sourcepath + templateName + filetype;

            // report 폴더 없으면생성
            DirectoryInfo di = new DirectoryInfo(targetpath);

            if (di.Exists == false)

            {

                di.Create();

            }

            CopyFile(sourceFileFullName, TargetFileFullName);

            // Get the Excel application object.
            oAppln = new Excel.Application();

            // Make Excel visible (optional).
            oAppln.Visible = true;

            oWorkBook = (oAppln.Workbooks.Add(TargetFileFullName));
            oWorkSheet = (Excel.Worksheet)oWorkBook.ActiveSheet;

            //oRange = oWorkSheet.get_Range("A1", "I38");
            //oRange.EntireColumn.AutoFit();
            //oAppln.UserControl = false;

            //Excel.Shape shp = oWorkSheet.Activate.re

            int work_date_rowIndex = 4;

            int work_type_1_rowIndex = 8;
            int work_type_1_sum_rowIndex = 17;
            int work_type_2_rowIndex = 18;
            int work_type_2_sum_rowIndex = 27;
            int work_type_3_rowIndex = 28;
            int work_type_3_sum_rowIndex = 37;
            int work_type_total_sum_rowIndex = 38;

            int excel_start_cols_index = 2;
            int excel_start_sum_cols_index = 4;
            int dt_start_col_data_index = 6;

            //int dt_start_sum_data_index = 5;
            int sum_col_index = 8;

            //string title = "No. " + line_gp.Substring(1, 1) + "MPI 가동속보";
            //oWorkSheet.Cells[2, 3] = title;

            string work_date = string.Format("작업일자 : {0} 년 {1} 월 {2} 일", search_date.Year, search_date.Month, search_date.Day);
            oWorkSheet.Cells[work_date_rowIndex, 1] = work_date;

            // 라인 정보
            string _line_gp = string.Format("LINE NO.( {0} )", line_gp.Substring(1,1));
            oWorkSheet.Cells[work_date_rowIndex + 1, 1] = _line_gp;

            // 근에 따른 구분
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '1근'  AND ISNULL(START_DDTT, ' ') <> '소계' AND ISNULL(END_DDTT, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //1근좌표에서 부터 뿌려줌
                // item: 한 datarow 형식으로 적힐 데이터를 소유함
                // dt_start_col_data_index:  dt의 적힐 col 의 인덱스
                // work_type_1_rowIndex: 근 관련 행의 시작점으로 증가함
                // excel_start_cols_index: 엑셀에 적힐 시작점의 인덱스..
                SetupExcelData(item, dt_start_col_data_index, work_type_1_rowIndex, excel_start_cols_index);
                work_type_1_rowIndex++;
            }
            // 1근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '1근'  AND ISNULL(START_DDTT, ' ') = '소계'");//  AND ISNULL(END_DDTT, ' ') =' '");
            foreach (DataRow item in result)
            {
                //1근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, sum_col_index, work_type_1_sum_rowIndex, excel_start_sum_cols_index);
            }

            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '2근'  AND ISNULL(START_DDTT, ' ') <> '소계' AND ISNULL(END_DDTT, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //2근좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_col_data_index, work_type_2_rowIndex, excel_start_cols_index);
                work_type_2_rowIndex++;
            }

            // 2근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '2근'  AND ISNULL(START_DDTT, ' ') = '소계' ");//  AND ISNULL(END_DDTT, ' ') = ' '");
            foreach (DataRow item in result)
            {
                //2근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, sum_col_index, work_type_2_sum_rowIndex, excel_start_sum_cols_index);
            }

            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '3근'  AND ISNULL(START_DDTT, ' ') <> '소계' AND ISNULL(END_DDTT, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //3근 데이터 좌표에서 부터 뿌려줌
                SetupExcelData(item, dt_start_col_data_index, work_type_3_rowIndex, excel_start_cols_index);
                work_type_3_rowIndex++;
            }

            //3근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '3근'  AND ISNULL(START_DDTT, ' ') = '소계'");// AND ISNULL(END_DDTT, ' ') = ' '");
            foreach (DataRow item in result)
            {
                //3근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, sum_col_index, work_type_3_sum_rowIndex, excel_start_sum_cols_index);

            }

            //총근에 대한 계를 뿌려줌
            result = dt.Select("ISNULL(WORK_TYPE_NM, ' ') = '계'");//  AND ISNULL(START_DDTT, ' ') <> '소계' AND ISNULL(START_DDTT, ' ') <> ' '");
            foreach (DataRow item in result)
            {
                //총근 계 좌표에서 부터 뿌려줌
                SetupExcelData(item, sum_col_index, work_type_total_sum_rowIndex, excel_start_sum_cols_index);

            }

            //oWorkBook.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);

            oAppln.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"> datarow </param>
        /// <param name="dt_start_col_data_index"> 원하는 column의 시작 인덱스 </param>
        /// <param name="work_type_rowIndex"> 엑셀에 적힐 근에 대한 행의 인덱스</param>
        /// <param name="excel_start_cols_index"> 엑셀에 적힐 column의 인덱스로 적힐 값이 늘어날때마다 증가되어야함.</param>
        private void SetupExcelData(DataRow item, int dt_start_col_data_index, int work_type_rowIndex, int excel_start_cols_index)
        {
            for (int col = dt_start_col_data_index; col < item.ItemArray.Count(); col++)
            {
                //3근좌표에서 부터 뿌려줌
                oWorkSheet.Cells[work_type_rowIndex, excel_start_cols_index] = item[col].ToString();
                excel_start_cols_index++;
            }
        }



        private string CopyFile(string source, string dest)
        {
            string result = "Copied file";
            try
            {
                // Overwrites existing files
                File.Copy(source, dest, true);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private Excel.Worksheet FindSheet(Excel.Workbook workbook, string sheet_name)
        {
            foreach (Excel.Worksheet sheet in workbook.Sheets)
            {
                if (sheet.Name == sheet_name) return sheet;
            }

            return null;

        }
        /// <summary>
        /// 그리드를 테이블로 만듬.
        /// </summary>
        /// <param name="flex"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(C1FlexGrid flex)
        {
            // create data table
            var dt = new System.Data.DataTable();

            // add columns
            for (int c = flex.Cols.Fixed; c < flex.Cols.Count; c++)
            {
                var col = flex.Cols[c];
                var colName = string.IsNullOrEmpty(col.Name) ? string.Format("col{0}", c) : col.Name;
                var colType = col.DataType == null ? typeof(object) : col.DataType;
                dt.Columns.Add(colName, colType);
            }

            // add rows
            for (int r = flex.Rows.Fixed; r < flex.Rows.Count; r++)
            {
                var row = dt.NewRow();
                for (int c = flex.Cols.Fixed; c < flex.Cols.Count; c++)
                {
                    var value = flex[r, c];
                    if (value != null)
                    {
                        row[c - flex.Cols.Fixed] = value;
                    }
                }
                dt.Rows.Add(row);
            }

            // done
            return dt;
        }

        /// <summary>
        /// 데이터 테이블에 해당 컬럼의 항목이 이미 존재하는지 유무를 파악
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colsNM"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool IsCheck_Item(System.Data.DataTable dt, string colsNM, string parameter)
        {
            bool rtn = false;
            int item_count = 0;

            string sql = string.Format("{0} = '{1}'", colsNM, parameter);

            DataRow[] result = dt.Select(sql);
            if (result.Count() > 0)
            {
                rtn = true;
            }
            return rtn;
        }

        /// <summary>
        /// 데이터 테이블에 해당 컬럼의 항목이 이미 존재하는지 유무를 파악
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colsNM"></param>
        /// <param name="parameter"></param>
        /// <param name="isAddRow"></param> 추가된 행인지 여부
        /// <returns></returns>
        public bool IsCheck_Item(System.Data.DataTable dt, string colsNM, string parameter, bool isAddRow)
        {
            bool rtn = false;
            int item_count = 0;

            if (isAddRow)
            {
                item_count = 0;
            }
            else
            {
                item_count = 1;
            }

            string sql = string.Format("{0} = '{1}'", colsNM, parameter);

            DataRow[] result = dt.Select(sql);
            if (result.Count() > item_count)
            {
                rtn = true;
            }
            return rtn;
        }

        public bool Has_Item(string table_NM, string colsNM, DateTime parameter)
        {
            bool rtn = false;

            // 해당 테이블에 해당 컬럼에 해당 문자열이 있는지 확인한다...
            string sql1 = "";
            sql1 += string.Format("select {0} ", colsNM);
            sql1 += string.Format("from {0} ", table_NM);
            sql1 += string.Format("WHERE {0} = '{1}' ", colsNM, parameter);

            DataTable dt = cd.FindDataTable(sql1);

            if (dt.Rows.Count > 0)
            {
                rtn = true;
            }

            return rtn;
        }
        public bool Has_Item(string table_NM, string colsNM, string parameter)
        {
            bool rtn = false;

            // 해당 테이블에 해당 컬럼에 해당 문자열이 있는지 확인한다...
            string sql1 = "";
            sql1 += string.Format("select {0} ", colsNM);
            sql1 += string.Format("from {0} ", table_NM);
            sql1 += string.Format("WHERE {0} = '{1}' ", colsNM, parameter);

            System.Data.DataTable dt = cd.FindDataTable(sql1);

            if (dt.Rows.Count > 0)
            {
                rtn = true;
            }

            return rtn;
        }
        public bool Has_Item(string table_NM, string colsNM1, string colsNM2, string colsNM3, string parameter1, string parameter2, string parameter3)
        {
            bool rtn = false;

            // 해당 테이블에 해당 컬럼에 해당 문자열이 있는지 확인한다...
            string sql1 = "";
            sql1 += string.Format("select {0},{1},{2} ", colsNM1, colsNM2, colsNM3);
            sql1 += string.Format("from {0} ", table_NM);
            sql1 += string.Format("WHERE {0} = '{1}' ", colsNM1, parameter1);
            sql1 += string.Format("AND {0} = '{1}' ", colsNM2, parameter2);
            sql1 += string.Format("AND {0} = '{1}' ", colsNM3, parameter3);
            System.Data.DataTable dt = cd.FindDataTable(sql1);

            if (dt.Rows.Count > 0)
            {
                rtn = true;
            }

            return rtn;
        }
        public bool Has_Item(string table_NM, string colsNM1, string colsNM2, string colsNM3, string colsNM4, string parameter1, string parameter2, string parameter3, string parameter4)
        {
            bool rtn = false;

            // 해당 테이블에 해당 컬럼에 해당 문자열이 있는지 확인한다...
            string sql1 = "";
            sql1 += string.Format(" select {0} ", colsNM1);
            sql1 += string.Format(" from {0} ", table_NM);
            sql1 += string.Format(" WHERE {0} = TO_DATE( '{1}'||'{2}'||'00' , 'YYYY-MM-DD HH24:MI:SS') ", colsNM1, parameter3, parameter1);
            sql1 += string.Format(" AND {0} = '{1}' ", colsNM2, parameter2);
            sql1 += string.Format(" AND {0} = '{1}' ", colsNM3, parameter3);
            sql1 += string.Format(" AND {0} = '{1}' ", colsNM4, parameter4);
            System.Data.DataTable dt = cd.FindDataTable(sql1);

            if (dt.Rows.Count > 0)
            {
                rtn = true;
            }

            return rtn;
        }
        public bool Has_Item(string table_NM, string colsNM, string parameter, string keyColNM, string keyValue)
        {
            bool rtn = false;

            // 해당 테이블에 해당 컬럼에 해당 문자열이 있는지 확인한다...
            string sql1 = "";
            sql1 += string.Format("select {0} ", colsNM);
            sql1 += string.Format("from {0} ", table_NM);
            sql1 += string.Format("WHERE {0} = '{1}' ", keyColNM, keyValue);
            sql1 += string.Format("AND   {0} = '{1}' ", colsNM, parameter);

            System.Data.DataTable dt = cd.FindDataTable(sql1);

            if (dt.Rows.Count > 0)
            {
                rtn = true;
            }

            return rtn;
        }

        public bool IsNumberChk(string value)
        {
            try
            {
                double i = double.Parse(value);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetGridValue(C1.Win.C1FlexGrid.C1FlexGrid grd, int row, string colNm)
        {
            string sRet = "";

            if (grd.Rows[row][colNm] != null)
            {
                sRet = grd.Rows[row][colNm].ToString();
            }

            return sRet;
        }

        public string GetGridValue(C1.Win.C1FlexGrid.C1FlexGrid grd, int row, int col)
        {
            string sRet = "";

            if (grd.Rows[row][col] != null)
            {
                sRet = grd.Rows[row][col].ToString();
            }

            return sRet;
        }

        public string GetGridValue(C1.Win.C1FlexGrid.C1FlexGrid grd, int row, string colNm, string def)
        {
            string sRet = def;

            if (grd.Rows[row][colNm] != null)
            {
                sRet = grd.Rows[row][colNm].ToString();
            }

            return sRet;
        }

        public string GetGridValue(C1.Win.C1FlexGrid.C1FlexGrid grd, int row, int col, string def)
        {
            string sRet = def;

            if (grd.Rows[row][col] != null)
            {
                sRet = grd.Rows[row][col].ToString();
            }

            return sRet;
        }

        /// <summary>
        /// 문자의 바이트 단위 갯수를 가져온다.
        /// </summary>
        /// <param name="str">확인문자</param>
        /// <returns>바이트 갯수</returns>
        public int getStrLen(string str)
        {
            byte[] tmp = System.Text.Encoding.Default.GetBytes(str);
            return tmp.Length;
        }

        /// <summary>
        /// 문자중 한글 포함여부 체크(한글이 존재시 true)
        /// </summary>
        /// <param name="s">확인 문자</param>
        /// <returns>true, false</returns>
        public bool isContainHangul(string s)
        {
            char[] charArr = s.ToCharArray();

            foreach (char c in charArr)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter) //OtherLetter
                {
                    return true;
                }
            }

            return false;
        }
        public bool compareMinMax(string value, string grp_nm, C1.Win.C1FlexGrid.C1FlexGrid selectedGrd)
        {
             int comNum= int.Parse(value);
                for (int row = 1; row < selectedGrd.Rows.Count - 1; row++)
                {
                    if (grp_nm == selectedGrd.Rows[row]["STEEL_GRP"].ToString())
                    {
                        if (comNum >= int.Parse(selectedGrd.Rows[row]["ITEM_SIZE_MIN"].ToString()) && comNum <= int.Parse(selectedGrd.Rows[row]["ITEM_SIZE_MAX"].ToString()))
                        {
                            string strMsg =  string.Format("{0}라인의 MIN ~ MAX 범위와 중복이 됩니다. ", row.ToString());
                            MessageBox.Show(strMsg);

                            return true;
                        }
                    }
                }
            
            return false;
        }


        /// <summary>
        /// 그리드 내에서 범위 중복 유무 찾기
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="grup_id"></param>
        /// <param name="minvalue"></param>
        /// <param name="maxvalue"></param>
        /// <param name="myRowIndex"></param>
        /// <returns></returns>
        public bool IsSameRangeInGrd(C1FlexGrid grd, string grup_id, string minvalue, string maxvalue, int myRowIndex)
        {
            bool isSameRange = false;

            int minValuegrd = 0;
            int maxValuegrd = 0;

            int minValue = 0;
            int maxValue = 0;

            for (int row = 1; row < grd.Rows.Count - 1; row++)
            {
                if (grd.GetData(row, "STEEL_GRP").ToString() != grup_id || row == myRowIndex)
                {
                    continue;
                }
                minValuegrd = CInt(grd.GetData(row, "ITEM_SIZE_MIN").ToString());
                maxValuegrd = CInt(grd.GetData(row, "ITEM_SIZE_MAX").ToString());


                minValue = CInt(minvalue);
                maxValue = CInt(maxvalue);

                // 기준범위 밖에서 곂치는 경우    입력범위 min 값 < 기준 min 값<  < 기준 max 값 < 입력범위 max 값    + // 기분범위 안에서 겹치는 경우
                if ((minValuegrd > minValue && maxValuegrd < maxValue)||(minValuegrd < minValue && maxValuegrd > maxValue) || ((minValuegrd > minValue && minValuegrd < maxValue) || maxValuegrd > minValue && maxValuegrd < maxValue))
                {

                    string strMsg = string.Format("{0}라인의 MIN ~ MAX 범위와 중복이 됩니다. ", row.ToString());
                    MessageBox.Show(strMsg);
                    grd.Select(myRowIndex, 1, true);
                    return true;
                }
            }
            return isSameRange;
        }



        /// <summary>
        /// 입력된 min,max 값이 db의 min,max 값과 범위가 겹치는지 여부
        /// </summary>
        /// <param name="check_table_NM"> table name</param>
        /// <param name="check_NM"> GRP NAME</param>
        /// <param name="check_Minvalue"> MIX VALUE</param>
        /// <param name="check_Maxvalue"> MAX VALUE</param>
        /// <returns></returns>
        public bool IsSameRange(string check_table_NM, string check_NM, string check_Minvalue, string check_Maxvalue)
        {
            bool IsSameRange = false;
            //서버에서 해당 테이블의 그룹명으로 검색한 테이블을 가져와서 
            // 범위가 중복하는것이 있는지 검색한다.

            List<int> listvalue;
            string strQry = string.Empty;
            int minLimitDuplicate = 1;

            strQry += string.Format("SELECT  ");
            strQry += string.Format("    STEEL_GRP ");
            strQry += string.Format("    ,ITEM_SIZE_MIN ");
            strQry += string.Format("    ,ITEM_SIZE_MAX ");
            strQry += string.Format("FROM ");
            strQry += string.Format("    {0} ", check_table_NM);
            strQry += string.Format(" WHERE STEEL_GRP LIKE '{0}' || '%'  ", check_NM);
            strQry += string.Format(" ORDER BY STEEL_GRP,ITEM_SIZE_MIN,ITEM_SIZE_MAX ");
            temp_min_max = cd.FindDataTable(strQry);

            int minValuedt = 0;
            int maxValuedt = 0;

            int minValue = 0;
            int maxValue = 0;

            for (int row = 0; row < temp_min_max.Rows.Count; row++)
            {
                listvalue = new List<int>();

                minValuedt = CInt(temp_min_max.Rows[row]["ITEM_SIZE_MIN"].ToString());
                maxValuedt = CInt(temp_min_max.Rows[row]["ITEM_SIZE_MAX"].ToString());

                for (int i = minValuedt; i <= maxValuedt; i++)
                {
                    listvalue.Add(i);
                }

                minValue = CInt(check_Minvalue);
                maxValue = CInt(check_Maxvalue);

                for (int i = minValue; i <= maxValue; i++)
                {
                    listvalue.Add(i);
                }

                var numberOfTestcasesWithDuplicates =
                        listvalue.GroupBy(x => x).Count(x => x.Count() > 1);

                if (numberOfTestcasesWithDuplicates > minLimitDuplicate)
                {
                    IsSameRange = true;
                }


                //// 기준범위 밖에서 곂치는 경우    입력범위 min 값 < 기준 min 값<  < 기준 max 값 < 입력범위 max 값
                //if (minValuedt > minValue && maxValuedt < maxValue)
                //{
                //    IsSameRange = true;
                //    break;
                //}

                //if (minValuedt < minValue && maxValuedt > maxValue)
                //{
                //    IsSameRange =  true;
                //    break;
                //}

                //// 기분범위 안에서 겹치는 경우
                //if ((minValuedt > minValue && minValuedt < maxValue) || maxValuedt > minValue && maxValuedt < maxValue)
                //{
                //    IsSameRange = true;
                //    break;
                //}
            }
            return IsSameRange;
        }


        public bool IsSameRangeMinMax(string check_table_NM, string check_Minvalue, string check_Maxvalue)
        {
            bool IsSameRange = false;
            //서버에서 해당 테이블의 그룹명으로 검색한 테이블을 가져와서 
            // 범위가 중복하는것이 있는지 검색한다.

            string strQry = string.Empty;

            strQry += string.Format("SELECT  ");
            strQry += string.Format("    ITEM_SIZE_MIN ");
            strQry += string.Format("    ,ITEM_SIZE_MAX ");
            strQry += string.Format("FROM ");
            strQry += string.Format("    {0} ", check_table_NM);
            strQry += string.Format("    ORDER BY ITEM_SIZE_MIN,ITEM_SIZE_MAX ");
            temp_min_max = cd.FindDataTable(strQry);

            int minValuedt = 0;
            int maxValuedt = 0;

            int minValue = 0;
            int maxValue = 0;

            for (int row = 0; row < temp_min_max.Rows.Count; row++)
            {
                minValuedt = CInt(temp_min_max.Rows[row]["ITEM_SIZE_MIN"].ToString());
                maxValuedt = CInt(temp_min_max.Rows[row]["ITEM_SIZE_MAX"].ToString());

                minValue = CInt(check_Minvalue);
                maxValue = CInt(check_Maxvalue);

                // 기준범위 밖에서 곂치는 경우    입력범위 min 값 < 기준 min 값<  < 기준 max 값 < 입력범위 max 값
                if (minValuedt >= minValue && maxValuedt <= maxValue)
                {
                    IsSameRange = true;
                    break;
                }

                if (minValuedt <= minValue && maxValuedt >= maxValue)
                {
                    IsSameRange = true;
                    break;
                }

                // 기분범위 안에서 겹치는 경우
                if ((minValuedt >= minValue && minValuedt <= maxValue) || maxValuedt >= minValue && maxValuedt <= maxValue)
                {
                    IsSameRange = true;
                    break;
                }


            }

            return IsSameRange;
        }
        public bool IsITEMInGrd(string value, string col_nm, C1FlexGrid grd)
        {
            for (int row = 1; row < grd.Rows.Count - 1; row++)
            {
                if (value == grd.GetData(row, col_nm).ToString())
                {
                    MessageBox.Show(string.Format("{0}라인의 {1} 값이 중복됩니다. ", row.ToString(), grd.Cols[col_nm].Caption));

                    return true;
                }
            }
            return false;
        }

        public bool compareMinMax(string value, C1.Win.C1FlexGrid.C1FlexGrid selectedGrd, int myRowIndex)
        {
            int comNum = int.Parse(value);
            for (int row = 1; row < selectedGrd.Rows.Count - 1; row++)
            {
                if (row == myRowIndex)
                { continue; }
                else
                {
                    if (comNum >= int.Parse(selectedGrd.Rows[row]["ITEM_SIZE_MIN"].ToString()) && comNum <= int.Parse(selectedGrd.Rows[row]["ITEM_SIZE_MAX"].ToString()))
                    {
                        string strMsg = string.Format("{0}라인의 MIN ~ MAX 범위와 중복이 됩니다. ", row.ToString());
                        MessageBox.Show(strMsg);

                        return true;
                    }
                }
            }

            return false;
        }
        //
        //문자 체크
        //
        public bool isContainLetter(string s)
        {
            char[] charArr = s.ToCharArray();

            foreach (char c in charArr)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.UppercaseLetter) //
                {
                    return true;
                }else if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.LowercaseLetter) //
                {
                    return true;
                }
                else if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter) //
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// substring B : 한글의substring
        /// 한글이중간에서잘릴경우?가나오기때문에하나지워준다.
        /// </summary>
        /// <param name="inputString">대상문자</param>
        /// <param name="stringLength">문자열갯수</param>
        /// <returns></returns>
        public string SubstrB(string inputString, int stringLength)
        {
            string outputString = "";
            Encoding ec = Encoding.Default;
            byte[] temp = ec.GetBytes(inputString);

            if (temp.Length > stringLength)
            {
                outputString = ec.GetString(temp, 0, stringLength);
                if (outputString.Substring(outputString.Length - 1, 1) == "?")
                {
                    outputString = outputString.Substring(0, outputString.Length - 1);
                }
            }
            else
            {
                outputString = ec.GetString(temp, 0, temp.Length);
            }

            return outputString;
        }

        public void KeyPressEvent(object sender, KeyPressEditEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8)// && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
            {
                e.Handled = true;
            }
        }


        public void KeyPressEvent_number(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8)// && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
            {
                e.Handled = true;
            }
            //tbItemSize.NumericInput = true;
        }

        public void KeyPressEvent_decimal(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 46)// && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
            {
                e.Handled = true;
            }
        }

        public void KeyPressEvent_None(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    }
}
