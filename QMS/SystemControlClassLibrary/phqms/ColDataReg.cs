using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComLib;
using ComLib.clsMgr;
using System.Data.OracleClient;

namespace SystemControlClassLibrary.phqms
{
    public partial class ColDataReg : Form
    {
        #region 변수선언
        OracleTransaction transaction = null;
        DataTable olddt;
        DataTable moddt;

        clsCom ck = new clsCom();
        clsCom cS = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        DateTime dateTime = new DateTime();

        private int cboLine_GP_index = 0;
        private int work_type = 0;
        private int work_team = 0;

        string biz_gp, use_yn;

        #endregion

        public ColDataReg()
        {
            InitializeComponent();
        }

        private void ColDataReg_Load(object sender, EventArgs e)
        {
            InitControl();
        }

        private void InitControl()
        {
            cs.InitCombo(cbBiz_gp, StringAlignment.Near);
            cs.InitCombo(cbUse_yn, StringAlignment.Near);

            cd.SetCombo(cbBiz_gp, "BIZ_GP", false, ck.Biz_gp);
            cd.SetCombo(cbUse_yn, "USE_YN", false, ck.Use_yn);

            ClearForm();
        }

        private void ClearForm()
        {
            txtTableId.Text = "";
            txtTableNm.Text = "";
            txtOperTableId.Text = "";
            txtDbLinkId.Text = "";
        }

        private void cbBiz_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            biz_gp = string.Empty;

            if (cbBiz_gp.SelectedIndex >= 0)
            {
                biz_gp = cbBiz_gp.SelectedValue as string;
                //biz_ID = cd.Find_CD_ID("BIZ_GP", biz_gp);
                //MessageBox.Show(biz_gp);
            }
        }

        private void cbUse_yn_SelectedIndexChanged(object sender, EventArgs e)
        {
            use_yn = string.Empty;

            if (cbUse_yn.SelectedIndex >= 0)
            {
                use_yn = cbUse_yn.SelectedValue as string;
                //biz_ID = cd.Find_CD_ID("BIZ_GP", biz_gp);
                //MessageBox.Show(use_yn);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnReg":
                    SaveReg();
                    break;

                case "btnClose":
                    Close();
                    break;
            }
        }

        #region 저장
        private bool SaveReg()
        {


            #region 입력 체크--------------------------------------------------------------------------------------------
            biz_gp = cbBiz_gp.SelectedValue.ToString();
            use_yn = cbUse_yn.SelectedValue.ToString();

            //업무구분
            if (biz_gp == "")   //|| (biz_gp == "")
            {
                MessageBox.Show("업무구분을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbBiz_gp.Focus();
                return false;
            }
            
            //MV테이블ID
            if (txtTableId.Text == "")
            {
                MessageBox.Show("MV테이블ID를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTableId.Focus();
                return false;
            }

            //MV테이블명
            if (txtTableNm.Text == "")
            {
                MessageBox.Show("MV테이블명을 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTableNm.Focus();
                return false;
            }

            //운영테이블ID
            if (txtOperTableId.Text == "")
            {
                MessageBox.Show("운영테이블ID를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtOperTableId.Focus();
                return false;
            }

            //DB Link ID
            if (txtDbLinkId.Text == "")
            {
                MessageBox.Show("DB Link ID를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDbLinkId.Focus();
                return false;
            }

            //사용여부
            if (use_yn =="")
            {
                MessageBox.Show("사용여부를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbUse_yn.Focus();
                return false;
            }
            #endregion

            string sql1 = string.Empty;
            string sql2 = string.Empty;

            //디비선언
            OracleConnection conn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                sql2 += string.Format(@" SELECT BIZ_GP, TABLE_ID FROM TB_MV_TABLE 
                         WHERE BIZ_GP = '{0}' AND TABLE_ID = '{1}'", biz_gp, txtTableId.Text);

                DataTable dt = cd.FindDataTable(sql2);

                if(dt.Rows.Count > 0)
                {
                    MessageBox.Show("중복된 데이터가 존재합니다.\n 저장 실패!");
                    return false;
                }

                sql1 += string.Format(@" INSERT INTO TB_MV_TABLE (
                                BIZ_GP
                              , TABLE_ID
                              , TABLE_NM
                              , OPER_TABLE_ID
                              , DB_LINK_ID
                              , USE_YN )
                          VALUES (
                                '{0}'
                              , '{1}'
                              , '{2}'
                              , '{3}'
                              , '{4}'
                              , '{5}' )", biz_gp, txtTableId.Text, txtTableNm.Text, txtOperTableId.Text, txtDbLinkId.Text, use_yn);

                int cnt = 0;
                cmd.CommandText = sql1;
                cnt = cmd.ExecuteNonQuery();
                transaction.Commit();

                if (cnt > 0)
                {
                    string message = "정상적으로 저장되었습니다.";
                    clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), message);
                    MessageBox.Show("저장 완료.");
                    ClearForm();
                }

            }
            catch(Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다.");
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

            return true;
        }
        #endregion
    }
}
