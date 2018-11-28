using System;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace ComLib
{
    public class clsLog
    {
        public ListBox LstBox;
        public string StrFileName;

        public static clsLog Log = new clsLog();
        public static clsLog GetInstance()
        {
            return Log;
        }

        private readonly clsFile _file = new clsFile(clsLog.Log.StrFileName);

        // state_flag : 1(info), 2(warning), 3(Error)
        // save_flag  : 0(Dialog Only), 1(Dialog & File), 2(Dialog & File & DB), 3(File Only), 4(DB Only), 5(File & DB Only)
        public void SetMessage(string stateFlag, short saveFlag, string pModulname, int line, string msg)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            var now = DateTime.Now;

            var strMsg = $"[{clsUtil.Utl.GetCurrTime(12)}] ";

            strMsg += msg;

            if(saveFlag == 0 || saveFlag == 1 || saveFlag == 2)
                LstBox.Items.Insert(0, strMsg);

            if (saveFlag == 1 || saveFlag == 2 || saveFlag == 3)
                _file.LogWrite(pModulname, line, stateFlag, strMsg, StrFileName);

            if (LstBox.Items.Count > 1000)
                LstBox.Items.RemoveAt(1000);
        }

        public string Alarm(string pinfo, string pModuleName, int line, params object[] msg)
        {
            var rtn = string.Empty;

            if (msg.Length <= 0) return rtn;
            var strMsg = $"[{clsUtil.Utl.GetCurrTime(1)}]";

            strMsg = msg.Aggregate(strMsg, (current, m) => current + m);

            if (LstBox != null)
            {
                LstBox.Items.Insert(0, strMsg);

                if (LstBox.Items.Count > 1000)
                    LstBox.Items.RemoveAt(1000);
            }

            clsFile.Run.LogWrite(pModuleName, line, pinfo, strMsg, StrFileName);

            return strMsg;
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
