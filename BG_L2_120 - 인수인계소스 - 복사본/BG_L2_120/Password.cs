using ComLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_L2_120
{
    public partial class Password : Form
    {
        public Password()
        {
            InitializeComponent();
        }

        private void Password_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblCaps.Visible = IsKeyLocked(Keys.CapsLock);
        }

        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();

        private void txtPwdNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPwdCfm.Focus();
            }
        }

        private void txtPwdCfm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender, e);
            }
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            lblCaps.Visible = IsKeyLocked(Keys.CapsLock);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPwdOld.Text == "")
            {
                MessageBox.Show("『기존 비밀번호』를 입력하여 주십시오.");
                txtPwdOld.Focus();
                return;
            }

            if (txtPwdNew.Text == "")
            {
                MessageBox.Show("『신규 비밀번호』를 입력하여 주십시오.");
                txtPwdNew.Focus();
                return;
            }

            if (txtPwdCfm.Text == "")
            {
                MessageBox.Show("『비밀번호 확인』을 입력하여 주십시오.");
                txtPwdCfm.Focus();
                return;
            }

            if (txtPwdOld.Text == txtPwdNew.Text)
            {
                MessageBox.Show("『기존 비밀번호』와 『신규 비밀번호』가 같습니다.");
                txtPwdNew.Text = "";
                txtPwdCfm.Text = "";
                txtPwdNew.Focus();
                return;
            }

            if (txtPwdNew.Text != txtPwdCfm.Text)
            {
                MessageBox.Show("『신규 비밀번호』와 『비밀번호 확인』이 다릅니다.");
                txtPwdNew.Text = "";
                txtPwdCfm.Text = "";
                txtPwdNew.Focus();
                return;
            }

            try
            {
                string Sql = string.Empty;

                Sql += string.Format("/*2017.07.05 비밀번호 체크 by 정호준*/");
                Sql += string.Format("SELECT PASSWD                         ");
                Sql += string.Format("  FROM TB_CM_USER                     ");
                Sql += string.Format(" WHERE USER_ID = '" + ck.UserID + "'   ");

                //string Sql = "SELECT PASSWD "
                //           + "  FROM MDMS.dbo.TB_CM_SCR"
                //           + " WHERE USER_ID = :P_LOGIN_ID";
                //string[] param = new string[1];
                //param[0] = ":P_LOGIN_ID|" + ck.UserID;

                DataTable dt = cd.FindDataTable(Sql);

                //string Sql = "SELECT PASSWD "
                //           + "  FROM MDMS.dbo.TB_CM_SCR"
                //           + " WHERE USER_ID = :P_LOGIN_ID";


                //string[] param = new string[1];
                //param[0] = ":P_LOGIN_ID|" + ck.UserID;

                //DataTable dt = cd.FindDataTable(Sql, param);

                if (dt == null || dt.Rows.Count < 1)
                {
                    MessageBox.Show("등록되어 있지 않은 아이디 입니다.");
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }

                if (txtPwdOld.Text != dt.Rows[0]["PASSWD"].ToString())
                {
                    MessageBox.Show("기존 비밀번호를 바르게 입력하여 주십시오.");
                    txtPwdOld.Text = "";
                    txtPwdNew.Text = "";
                    txtPwdCfm.Text = "";
                    txtPwdOld.Focus();
                    return;
                }

                //변경된 비밀번호 등록
                //Sql  = string.Format(" UPDATE MDMS.dbo.TB_CM_SCR ");
                //Sql += string.Format(" SET PASSWD = '{0}' ", txtPwdNew.Text);
                //Sql += string.Format("    ,MODIFIER = '{0}' ", ck.UserID);
                //Sql += string.Format("    ,MOD_DDTT =  (select convert(varchar, getdate(), 120)) ");
                //Sql += string.Format(" WHERE USER_ID = '{0}'", ck.UserID);
                Sql = string.Format(" ");
                Sql += string.Format("UPDATE TB_CM_USER                                          ");
                Sql += string.Format("   SET PASSWD = '" + txtPwdNew.Text + "'                       ");
                Sql += string.Format("      ,MODIFIER = '" + ck.UserID + "'                       ");
                Sql += string.Format("	    ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120))");
                Sql += string.Format(" WHERE USER_ID = '" + ck.UserID + "'                        ");

                cd.ExecuteQuery(Sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("비밀번호를 변경하던 중 오류가 발생하였습니다.\n" + ex.Message);
                return;
            }

            MessageBox.Show("비밀번호가 정상적으로 변경되었습니다.");
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtPwdOld_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPwdNew.Focus();
            }
        }
    }
}
