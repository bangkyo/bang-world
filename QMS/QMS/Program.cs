using QMS;
using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace QMS
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool IsNewApp;
            Mutex mutex = new Mutex(true, Application.ProductName, out IsNewApp);
            if (IsNewApp)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //////Application.Run(new MainForm());
                ////clsMsg.Log.Alarm(Alarms.Info, "test" , clsMsg.Log.__Line(), "TEST");
                //bool Isupdated = CheckUpdateSetup();
                ////InstallUpdateSyncWithInfo();

                //if (Isupdated)
                //{
                //    Application.Restart();
                //}
                

                var loginForm = new LoginForm();
                //loginForm.ShowDialog();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
               
                    Application.Run(new MainForm());
                }
                //loginForm = null;
                mutex.ReleaseMutex();
            }
            else
            {
                ActivateProcessInExist(Application.ProductName);
            }
        }

        private static void ActivateProcessInExist(string ProductNameOfApplication)
        {
            // 이미 실행되고 있는 인스턴스를 활성화시킨다.
            Process[] viewProcesses = Process.GetProcessesByName(ProductNameOfApplication);
            if (null != viewProcesses && 2 == viewProcesses.Length)
            {
                Process view = (viewProcesses[0].Id == Process.GetCurrentProcess().Id) ?
                    viewProcesses[1] : viewProcesses[0];

                IntPtr hWndOfPrevInstance = view.MainWindowHandle;
                if (Win32.IsIconic(hWndOfPrevInstance))
                    Win32.ShowWindowAsync(hWndOfPrevInstance, Win32.SW_RESTORE);
                Win32.SetForegroundWindow(hWndOfPrevInstance);
            }
        }

        static bool CheckUpdateSetup()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                if (ad.CheckForUpdate(false))
                {
                    ad.UpdateAsync();

                    try
                    {
                        ProgressBar pb = new ProgressBar(ad);
                        //pb.MaximizeBox = false;
                        //pb.MinimizeBox = false;
                        pb.ControlBox = false;
                        pb.StartPosition = FormStartPosition.CenterParent;
                        pb.ShowDialog();
                        //ad.Update();
                        //ad.UpdateProgressChanged += Ad_UpdateProgressChanged;

                        //MessageBox.Show("The application has been upgraded, and will now restart.");
                        //Application.Restart();
                    }
                    catch (DeploymentDownloadException dde)
                    {
                        MessageBox.Show("최신버전으로 설치할 수 없습니다. 네트워크 상태를 확인하세요. Error: " + dde);
                        return false;
                    }
                    finally
                    {
                        //Application.Restart();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;  
        }
        static void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                    //info = ad.CheckForUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("네트워크 상태를 확인바랍니다. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("새 버전을 확인할수 없습니다. 다시 시도하세요.. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("업데이트를 실패하였습니다. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (doUpdate)
                    {

                        try
                        {
                            ProgressBar pb = new ProgressBar(ad);
                            //pb.MaximizeBox = false;
                            //pb.MinimizeBox = false;
                            pb.ControlBox = false;
                            pb.StartPosition = FormStartPosition.CenterParent;
                            pb.ShowDialog();
                            //ad.Update();
                            //ad.UpdateProgressChanged += Ad_UpdateProgressChanged;

                            //MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("최신버전으로 설치할 수 없습니다. 네트워크 상태를 확인하세요. Error: " + dde);
                            return;
                        }
                        finally
                        {
                            //Application.Restart();
                        }
                    }
                }
            }
        }
    }

    public class Win32
    {
        /// <summary>
        /// The SetForegroundWindow function puts the thread that created the specified window
        /// into the foreground and activates the window. Keyboard input is directed to the window,
        /// and various visual cues are changed for the user. The system assigns a slightly higher
        /// priority to the thread that created the foreground window than it does to other threads.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// The ShowWindowAsync function sets the show state of a window created by a different thread.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// The IsIconic function determines whether the specified window is minimized (iconic).
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        // Activates and displays the window. If the window is minimized or maximized, the system
        // restores it to its original size and position. An application should specify this flag
        // when restoring a minimized window.
        public static int SW_RESTORE = 9;
    }
}
