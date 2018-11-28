using ComLib;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_L2_120
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        string iniFileFullPath = "C:\\BG_L2_120\\INI\\" + "UserId.ini";
        bool bEditChange = false;
        ComponentResourceManager resources = new ComponentResourceManager(typeof(LoginForm));
        
        


        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (ReadUserIdFile())
            {
                if (ck.UserCk.Trim() == "1")
                {
                    idchk.Checked = true;
                    txtLoginID.Text = ck.UserID.Trim();
                }
            }
            else
            {
                txtLoginID.Text = "";
            }

        }

        

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Login();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();

            // 작업표시줄을 제외한  작업 사이즈 {Width = 1280 Height = 984} 1280,984
            //Size WorkingArea = Screen.PrimaryScreen.WorkingArea.Size;
            //MessageBox.Show(WorkingArea.ToString());

            DeleteUserIdFile();
            WriteUserIdFile();
        }



        private void TxtLoginID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                txtPasswd.Focus();
        }

        private void lblChangePassword_Click(object sender, EventArgs e)
        {
            if (txtLoginID.Text == "")
            {
                MessageBox.Show("ID를 입력하세요");
                return;
            }

            Password Pwd = new Password();
            Pwd.StartPosition = FormStartPosition.CenterParent;

            this.Hide();

            DialogResult dResult = Pwd.ShowDialog(this);

            if (DialogResult.OK == dResult || DialogResult.Cancel == dResult)
            {
                this.Show();
            }
        }



        private void txtLoginID_TextChanged(object sender, EventArgs e)
        {
            bEditChange = true;
        }

        private void TxtPasswd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Login();
        }

        private void Login()
        {


            if (txtLoginID.Text.Trim() == "")
            {
                MessageBox.Show("아이디를 입력하세요.", "확인");
                txtLoginID.Focus();
                return;
            }

            if (txtPasswd.Text.Trim() == "")
            {
                MessageBox.Show("비밀번호를 입력하세요.", "확인");
                txtPasswd.Focus();
                return;
            }

            try
            {
                //사용자성명/권한그룹ID/비밀번호 구하기
                string Sql = "";
                Sql += string.Format(" SELECT TOP 1           ");
                Sql += string.Format("        A.USER_ID       ");
                Sql += string.Format("      , A.NM            ");
                Sql += string.Format("      , A.PASSWD        ");
                Sql += string.Format("      , B.ACL_GRP_ID    ");
                Sql += string.Format("      , B.ACL_GRP_NM    ");
                Sql += string.Format("   FROM TB_CM_USER    A ");
                Sql += string.Format("      , TB_CM_ACL_GRP B ");
                Sql += string.Format("  WHERE A.USER_ID =  '{0}'", txtLoginID.Text.Trim());
                Sql += string.Format("    AND A.PASSWD  =  '{0}'", txtPasswd.Text.Trim());
                Sql += string.Format("    AND A.ACL_GRP_ID IS NOT NULL    ");
                Sql += string.Format("    AND A.ACL_GRP_ID = B.ACL_GRP_ID ");

                DataTable dt = cd.FindDataTable(Sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("등록된 사용자가 아닙니다." + "\r\n" + "\r\n" + "아이디를 확인하세요!!", "확인");
                    txtLoginID.Focus();
                    return;
                }

                if (txtPasswd.Text.Trim() != dt.Rows[0]["PASSWD"].ToString().Trim())
                {
                    MessageBox.Show("비밀번호를 확인하세요!!", "확인");
                    txtPasswd.Focus();
                    return;
                }

                ck.UserID = txtLoginID.Text.Trim();
                ck.UserNm = dt.Rows[0]["NM"].ToString();
                ck.UserGrp = dt.Rows[0]["ACL_GRP_ID"].ToString();

                this.DialogResult = DialogResult.OK;

                // 로그인 이력 기록 
                InsertLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show("연결실패!!!!_"+ ex.Message, ex.Message);
                return;
            }
            finally
            {
                
            }

            

            
        }

        private void InsertLogin()
        {
            string localComputerName = Dns.GetHostName();

            IPAddress[] localIPs = Dns.GetHostAddresses(localComputerName);

            string ipNum = localIPs[1].ToString();

            // insert login history

            //디비선언
            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                //DateTime rdate = Convert.ToDateTime(cd.GetDBDate("1"));
                //DateTime rdate = DateTime.ParseExact(cd.GetDBDate("1"), "yyyy.MM.dd HH:mm:ss", null);
                //string  rdate = cd.GetDBDate("1");

                string Sql1 = string.Empty;
                Sql1 = string.Format(" INSERT INTO TB_CM_LOGINHIS ");
                Sql1 += string.Format(" ( ");
                Sql1 += string.Format("    LOGIN_DDTT ");
                //Sql1 += string.Format("   ,SYSTEM_GP ");
                Sql1 += string.Format("   ,USER_ID ");
                Sql1 += string.Format("   ,PC_IP ");
                Sql1 += string.Format("   ,REGISTER ");
                Sql1 += string.Format("   ,REG_DDTT ");
                Sql1 += string.Format("   ) ");
                Sql1 += string.Format(" VALUES ( ");
                Sql1 += string.Format("    (select convert(varchar, getdate(), 120)) ");
                //Sql1 += string.Format("   ,'MDMS'");
                Sql1 += string.Format("   ,'{0}' ", ck.UserID);
                Sql1 += string.Format("   ,'{0}' ", ipNum);
                Sql1 += string.Format("   ,'{0}' ", ck.UserID);
                Sql1 += string.Format("   ,(select convert(varchar, getdate(), 120)) ");
                Sql1 += string.Format("   ) ");


                cmd.CommandText = Sql1;
                cmd.ExecuteNonQuery();

                //실행후 성공
                transaction.Commit();

            }
            catch (System.Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        #region "사용자정의 이벤트"
        //---이벤트 생성
        private void EventCreate()
        {
            this.txtLoginID.GotFocus += txtLoginID_GotFocus;      //아이디(GotFocus)
            this.txtLoginID.LostFocus += txtLoginID_LostFocus;    //아이디(LostFocus)
        }

        private void txtLoginID_GotFocus(object sender, EventArgs e)
        {
            bEditChange = false;
        }

        //아이디 변경 -> UserId.ini파일을 삭제하고, 다시 만든다.
        private void txtLoginID_LostFocus(object sender, EventArgs e)
        {
            if (bEditChange == true)
            {
                if (FileSystem.GetFileInfo(iniFileFullPath).Exists)
                {
                    DeleteUserIdFile();
                    WriteUserIdFile();
                }
                else
                {
                    // 파일 생성후 
                    CreateUserIdFile(iniFileFullPath);
                    WriteUserIdFile();
                }
            }
        }

        private void WriteUserIdFile()
        {
            if (FileSystem.GetFileInfo(iniFileFullPath).Exists) FileSystem.GetFileInfo(iniFileFullPath).Delete();  //파일 지움

            FileSystem.GetFileInfo(iniFileFullPath).OpenWrite().Close();
            StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("# User Id");
            sb.AppendLine("UserId = " + txtLoginID.Text.Trim());

            if (idchk.Checked == true)
            {
                sb.AppendLine("UserCk = " + "1");
            }
            else
            {
                sb.AppendLine("UserCk = " + "0");
            }

            sb.AppendLine("");

            FileSystem.WriteAllText(iniFileFullPath, sb.ToString(), true);
        }

        private void DeleteUserIdFile()
        {
            if (FileSystem.GetFileInfo(iniFileFullPath).Exists) FileSystem.GetFileInfo(iniFileFullPath).Delete();  //파일 지움
        }

        private void CreateUserIdFile(string _filefullpath)
        {
            FileInfo fileInfo = new FileInfo(_filefullpath);

            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

        }

        #endregion

        private void LoginForm_Shown(object sender, System.EventArgs e)
        {
            //사용자정의 이벤트
            EventCreate();

            //Caps Lock 키
            if (IsKeyLocked(Keys.CapsLock)) lblCapsLock.Visible = true;
            else lblCapsLock.Visible = false;

            //if (this.AutoAdminLogin == 1)
            //    btnLogin_Click(null, null);
        }

        private void c1Button1_Click(object sender, EventArgs e)
        {
            txtLoginID.Text = "TEST";
            txtPasswd.Text = "TEST";
            Login();

        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            lblCapsLock.Visible = IsKeyLocked(Keys.CapsLock);
        }

        /// <summary>
        /// UserId.ini 파일 읽기
        /// </summary>
        public bool ReadUserIdFile()
        {
            int nIndex = 0;

            if (FileSystem.GetFileInfo(iniFileFullPath).Exists == false) return false;                        //파일이 없음

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
                        ck.UserID = arrText[i].Substring(nIndex + 1, arrText[i].Length - (nIndex + 1)).Trim().ToString();
                    }
                    if (arrText[i].Replace(" ", "").Substring(0, nIndex).ToUpper().Trim().ToString() == "USERCK")
                    {
                        nIndex = arrText[i].IndexOf("=");
                        ck.UserCk = arrText[i].Substring(nIndex + 1, arrText[i].Length - (nIndex + 1)).Trim().ToString();
                    }
                }
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtLoginID.Text = "TEST";
            txtPasswd.Text = "TEST";
            Login();
        }
    }
}
