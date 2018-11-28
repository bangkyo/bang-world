using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Collections;

namespace ComLib.clsMgr
{
    public enum Alarms
    {
        Info,
        Modified,
        InSerted,
        Deleted,
        Error
    }


    public class clsMsg
    {
        public ListBox Lstbox = new ListBox();

        //LABEL에서 StatusStrip으로 변경
        //public Label Msglabel = new Label();
        public Label Msglabel = new Label();
        public StatusStrip MsgStrip = new StatusStrip();
        //public string StrFilePath;
        //string StrFilePath = Directory.GetParent(Application.StartupPath) + @"\LOG\";
        //private Font msgFont = new Font("현대 하모니 B", 12, FontStyle.Bold);

        public static clsMsg Log = new clsMsg();

        private string prefixfileName = "L2_LOG";


        public static clsMsg GetInstance()
        {
            return Log;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinfo"> 1: information( 화면에 msg label로 알려주기만함), 
        ///                      2: modified or delete 시 ( 화면에 표시 및 로그에 기록을 남김 )
        ///                      3: Error message (로그 기록을 남김)
        /// </param>
        /// <param name="pModuleName"></param>
        /// <param name="line"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string Alarm(Alarms alarmEnum, string pModuleName, int line, params object[] msg)
        {


            var rtn = string.Empty;

            if (msg.Length <= 0) return rtn;
            var strMsg = $"[{clsUtil.Utl.GetCurrTime(1)}]";

            strMsg = msg.Aggregate(strMsg, (current, m) => current + m);


            switch (alarmEnum)
            {
                case Alarms.Info:
                    // 단순 조회
                    //Msglabel.Text = strMsg;
                    SetText(Msglabel, strMsg);
                    break;

                case Alarms.Modified:
                    // 수정 
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Modified", strMsg, prefixfileName);
                    break;

                case Alarms.InSerted:
                    // 추가
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "InSerted", strMsg, prefixfileName);
                    break;

                case Alarms.Deleted:
                    // 삭제
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Deleted", strMsg, prefixfileName);
                    break;

                case Alarms.Error:
                    // 오류
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Error", strMsg, prefixfileName);
                    break;

            }

            

            return strMsg;
        }
		
        //ALARM 함수 추가 및 변경
        //파라메터 추가 및 Label -> StatusBar로 변경
        //2017.06.05 OCJ
        public string Alarm(Alarms alarmEnum, string pModuleName, int line, int RowCount, params object[] msg)
        {

            var rtn = string.Empty;

            if (msg.Length <= 0) return rtn;
            var strMsg = "";//$"[{clsUtil.Utl.GetCurrTime(1)}]";

            strMsg = msg.Aggregate(strMsg, (current, m) => current + m);


            switch (alarmEnum)
            {
                case Alarms.Info:
                    // 단순 조회
                    //Msglabel.Text = strMsg;
                    //SetText(Msglabel, strMsg);
                    //SetStausLabelText(MsgStrip, strMsg);


                    SetStausLabelText(MsgStrip, strMsg, RowCount);
                    break;

                case Alarms.Modified:
                    // 수정 
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Modified", strMsg, prefixfileName);
                    break;

                case Alarms.InSerted:
                    // 추가
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "InSerted", strMsg, prefixfileName);
                    break;

                case Alarms.Deleted:
                    // 삭제
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Deleted", strMsg, prefixfileName);
                    break;

                case Alarms.Error:
                    // 오류
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Error", strMsg, prefixfileName);
                    break;

            }

            return strMsg;
        }

        public string Alarm(Alarms alarmEnum, string pModuleName, int line, int RowCount, TimeSpan time, params object[] msg)
        {

            var rtn = string.Empty;

            if (msg.Length <= 0) return rtn;
            var strMsg = "";//$"[{clsUtil.Utl.GetCurrTime(1)}]";

            strMsg = msg.Aggregate(strMsg, (current, m) => current + m);


            switch (alarmEnum)
            {
                case Alarms.Info:
                    // 단순 조회
                    //Msglabel.Text = strMsg;
                    //SetText(Msglabel, strMsg);
                    //SetStausLabelText(MsgStrip, strMsg);


                    SetStausLabelText(MsgStrip, strMsg, RowCount, time);
                    break;

                case Alarms.Modified:
                    // 수정 
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Modified", strMsg, prefixfileName);
                    break;

                case Alarms.InSerted:
                    // 추가
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "InSerted", strMsg, prefixfileName);
                    break;

                case Alarms.Deleted:
                    // 삭제
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Deleted", strMsg, prefixfileName);
                    break;

                case Alarms.Error:
                    // 오류
                    //Msglabel.Text = strMsg;
                    clsFile.Run.LogWrite(pModuleName, line, "Error", strMsg, prefixfileName);
                    break;

            }

            return strMsg;
        }

        delegate void SetTextCallback(Label lb, string text);

        private void SetText(Label lb, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lb.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                lb.Invoke(d, new object[] { lb, text });
            }
            else
            {
                lb.Text = text;
            }
        }
        //2017.06.05 OCJ 추가----
        //메인 프로그램 상태 바 변경 후 
        //관련 함수 추가
        //StatusStrip 관련함수 추가
        delegate void SetStatusLabelTextCallback(StatusStrip statusStrip, string text);

        //StatusStrip에 Message추가
        private void SetStausLabelText(StatusStrip statusStrip, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (statusStrip.InvokeRequired)
            {
                SetStatusLabelTextCallback txtCallBack = new SetStatusLabelTextCallback(SetStausLabelText);
                statusStrip.Invoke(txtCallBack, new object[] { statusStrip, text });
            }
            else
            {
                for (int i = 0; i < statusStrip.Items.Count; i++)
                {
                    if (statusStrip.Items[i].Tag.Equals("Message"))
                    {
                        statusStrip.Items[i].Text = text;
                    }
                    else if (statusStrip.Items[i].Tag.Equals("RowCount"))
                    {
                        statusStrip.Items[i].Text = "Rows : 0";
                    }
                    else if (statusStrip.Items[i].Tag.Equals("SearchTime"))
                    {
                        statusStrip.Items[i].Text = new TimeSpan(0, 0, 0).ToString("c");
                    }
                }
            }
        }

        //StatusStrip에 Message, RowCount 추가
        delegate void SetStatusLabelTextRowCountCallback(StatusStrip statusStrip, string text, int RowCount);


        //StatusStrip에 Message, RowCount 추가
        private void SetStausLabelText(StatusStrip statusStrip, string text, int RowCount)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (statusStrip.InvokeRequired)
            {
                SetStatusLabelTextRowCountCallback txtCallBack = new SetStatusLabelTextRowCountCallback(SetStausLabelText);
                statusStrip.Invoke(txtCallBack, new object[] { statusStrip, text, RowCount });
            }
            else
            {
                for (int i = 0; i < statusStrip.Items.Count; i++)
                {
                    if (statusStrip.Items[i].Tag.Equals("Message"))
                    {
                        statusStrip.Items[i].Text = text;
                    }
                    else if (statusStrip.Items[i].Tag.Equals("RowCount"))
                    {
                        statusStrip.Items[i].Text = "Rows : " + RowCount.ToString();
                    }
                    else if (statusStrip.Items[i].Tag.Equals("SearchTime"))
                    {
                        statusStrip.Items[i].Text = new TimeSpan(0,0,0).ToString("c");
                    }
                }
            }
        }
        delegate void SetStatusLabelTextRowCountTimeCallback(StatusStrip statusStrip, string text, int RowCount, TimeSpan time);

        private void SetStausLabelText(StatusStrip statusStrip, string text, int RowCount, TimeSpan time)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (statusStrip.InvokeRequired)
            {
                SetStatusLabelTextRowCountTimeCallback txtCallBack = new SetStatusLabelTextRowCountTimeCallback(SetStausLabelText);
                statusStrip.Invoke(txtCallBack, new object[] { statusStrip, text, RowCount, time });
            }
            else
            {
                for (int i = 0; i < statusStrip.Items.Count; i++)
                {
                    if (statusStrip.Items[i].Tag.Equals("Message"))
                    {
                        statusStrip.Items[i].Text = text;
                    }
                    else if (statusStrip.Items[i].Tag.Equals("RowCount"))
                    {
                        statusStrip.Items[i].Text = "Rows : " + RowCount.ToString();
                    }
                    else if (statusStrip.Items[i].Tag.Equals("SearchTime"))
                    {
                        statusStrip.Items[i].Text = time.ToString("c");
                    }
                }
            }
        }
        //---


        public string __Function()
        {
            var stackTrace = new StackTrace();
            return stackTrace.GetFrame(1).GetMethod().Name;
        }

        public int __Line()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileLineNumber();
        }

        public string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public int __Column()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileColumnNumber();
        }


        //
        public string __ReadUserId()
        {
            int nIndex = 0;
            var rtname = "";
            string iniFileFullPath = "C:\\MDMS\\INI\\" + "UserId.ini";
            if (FileSystem.GetFileInfo(iniFileFullPath).Exists == false) return "";                        //파일이 없음

            string sData = FileSystem.ReadAllText(iniFileFullPath, Encoding.Unicode).Replace("\r\n", ",");    //Enter(\n\r)표시를 콤마(,)로 대체
            string[] arrText = System.Text.RegularExpressions.Regex.Split(sData, ",");

            for (int i = 0; i < arrText.Length; i++)
            {
                if ((arrText[i].Replace(" ", "").Length > 0) && (arrText[i].Replace(" ", "").Substring(0, 1) != "#"))
                {
                    nIndex = arrText[i].Replace(" ", "").IndexOf("=");

                    //# UserId(사용자ID)
                    if (arrText[i].Replace(" ", "").Substring(0, nIndex).ToUpper().Trim().ToString() == "USERID")
                    {
                        nIndex = arrText[i].IndexOf("=");
                        rtname = arrText[i].Substring(nIndex + 1, arrText[i].Length - (nIndex + 1)).Trim().ToString();
                    }
                }
            }
            return rtname;
        }
        //
        public string[] __ReadUserIdIniFile()
        {
            int nIndex = 0;
            string iniFileFullPath = "C:\\MDMS\\INI\\" + "UserId.ini";
            if (FileSystem.GetFileInfo(iniFileFullPath).Exists == false) return null;                        //파일이 없음

            int nCount = 0;

            string sData = FileSystem.ReadAllText(iniFileFullPath, Encoding.Unicode).Replace("\r\n", ",");    //Enter(\n\r)표시를 콤마(,)로 대체
            string[] arrText = System.Text.RegularExpressions.Regex.Split(sData, ",");
            string[] UserInfo = new string[2];

            for (int i = 0; i < arrText.Length; i++)
            {
                if ((arrText[i].Replace(" ", "").Length > 0) && (arrText[i].Replace(" ", "").Substring(0, 1) != "#"))
                {
                    nIndex = arrText[i].Replace(" ", "").IndexOf("=");

                    //# UserId(사용자ID)
                    if (arrText[i].Replace(" ", "").Substring(0, nIndex).ToUpper().Trim().ToString() == "USERID" || arrText[i].Replace(" ", "").Substring(0, nIndex).ToUpper().Trim().ToString() == "USERCK")
                    {
                        nIndex = arrText[i].IndexOf("=");
                        UserInfo[nCount++] = arrText[i].Substring(nIndex + 1, arrText[i].Length - (nIndex + 1)).Trim().ToString();
                    }
                }
            }

            return UserInfo;
        }


    }
}
