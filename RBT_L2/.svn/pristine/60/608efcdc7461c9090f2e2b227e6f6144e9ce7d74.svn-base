using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public Label Msglabel = new Label();
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
    }
}
