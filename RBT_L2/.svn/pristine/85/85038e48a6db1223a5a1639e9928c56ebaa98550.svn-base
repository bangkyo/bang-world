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

namespace RBT_L2
{
    public partial class Password : Form
    {

        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();

        public Password()
        {
            InitializeComponent();

            KeyPreview = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblCaps.Visible = IsKeyLocked(Keys.CapsLock);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
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
                string sql = "SELECT PASSWD "
                           + "  FROM TB_CM_USER"
                           + " WHERE USER_ID = :P_LOGIN_ID";


                string[] param = new string[1];
                param[0] = ":P_LOGIN_ID|" + ck.UserID;

                DataTable dt = cd.FindDataTable(sql, param);

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
                sql  = string.Format(" UPDATE TB_CM_USER ");
                sql += string.Format(" SET PASSWD = '{0}' ", txtPwdNew.Text);
                sql += string.Format("    ,MODIFIER = '{0}' ", ck.UserID);
                sql += string.Format("    ,MOD_DDTT =  SYSDATE ");
                sql += string.Format(" WHERE USER_ID = '{0}'", ck.UserID);

                cd.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("비밀번호를 변경하던 중 오류가 발생하였습니다.\n" + ex.Message);
                return;
            }

            MessageBox.Show("비밀번호가 정상적으로 변경되었습니다.");
            this.DialogResult = DialogResult.OK;
        }

        private void txtPwdOld_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPwdNew.Focus();
            }
        }

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
    }
}
