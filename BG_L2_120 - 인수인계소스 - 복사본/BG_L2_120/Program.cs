using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_L2_120
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {

            RemoveRemainProgram();

            LoginProgramStart();

        }

        private static void LoginProgramStart()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForm());
            }
        }

        private static void RemoveRemainProgram()
        {
            Process[] processes = Process.GetProcessesByName(Application.ProductName);
            Process currentProcess = Process.GetCurrentProcess();

            foreach (Process p in processes)
            {
                if (currentProcess.Id != p.Id)
                {
                    p.Kill();
                }
            }
        }
    }
}
