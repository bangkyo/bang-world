using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace SystemControlClassLibrary.Popup
{
    public partial class BNDRsltPopUP : Form
    {
        #region 변수선언
        OracleTransaction transaction = null;

        clsCom cS = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();


        private int cboLine_GP_index = 0;
        private string sBundleNo = "";
        private string p_proc_gp = "";       //입력(REG), 수정(MOD)
        #endregion

        #region 화면
        public BNDRsltPopUP(int LineGp, string BundleNo)
        {
            InitializeComponent();

            cboLine_GP_index = LineGp;
            sBundleNo = BundleNo;

            if (sBundleNo == "") p_proc_gp = "REG";     //입력
            else p_proc_gp = "MOD";                     //수정

        }

        private void BNDRsltPopUP_Load(object sender, EventArgs e)
        {
            SetComboBox();

            ClearForm();

            if (p_proc_gp == "MOD") SetDataBinding();
            txtPoc_No.Focus();
        }

        private void ClearForm()
        {
            cboLine_GP.SelectedIndex = cboLine_GP_index;
            cboWork_Type.SelectedIndex = 0;
            cboWork_Team.SelectedIndex = 0;
            dtpMfg_Date.Value = DateTime.Now;

            txtBundle_No.Text = sBundleNo;
            txtPoc_No.Text = "";
            txtSize.Text = "";
            txtLength.Text = "";
            txtHeat.Text = "";
            txtSteel.Text = "";
            txtSteel_Nm.Text = "";
            mskWork_Start_Ddtt.Text = "";
            mskWork_End_Ddtt.Text = "";
            txtPcs.Text = "";
            txtTheory_Wgt.Text = "";

            btnConfirm.Enabled = true;
        }

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }
        #endregion

        #region "콤보박스 설정"
        private void SetComboBox()
        {
            try
            {
                //라인--------------------------------
                cboLine_GP.Items.Clear();

                DataTable dt = cd.Find_CD("LINE_GP");

                if (dt.Rows.Count > 0)
                {
                    //콤보박스 설정
                    cboLine_GP.DataSource = dt;

                    cboLine_GP.ValueMember = dt.Columns["CD_ID"].ToString();
                    cboLine_GP.DisplayMember = dt.Columns["CD_NM"].ToString();

                    //아이템 선택
                    cboLine_GP.SelectedIndex = cboLine_GP_index;
                }

                //근--------------------------------
                cboWork_Type.Items.Clear();

                DataTable dt1 = cd.Find_CD("WORK_TYPE");

                if (dt1.Rows.Count > 0)
                {
                    //콤보박스 설정
                    cboWork_Type.DataSource = dt1;

                    cboWork_Type.ValueMember = dt1.Columns["CD_ID"].ToString();
                    cboWork_Type.DisplayMember = dt1.Columns["CD_NM"].ToString();

                    //첫번째 아이템 선택
                    cboWork_Type.SelectedIndex = 0;
                }

                //조--------------------------------
                cboWork_Team.Items.Clear();

                DataTable dt2 = cd.Find_CD("WORK_TEAM");

                if (dt2.Rows.Count > 0)
                {
                    //콤보박스 설정
                    cboWork_Team.DataSource = dt2;

                    cboWork_Team.ValueMember = dt2.Columns["CD_ID"].ToString();
                    cboWork_Team.DisplayMember = dt2.Columns["CD_NM"].ToString();

                    //첫번째 아이템 선택
                    cboWork_Team.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }
        #endregion

        #region 버튼
        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnConfirm":
                    SetDataBinding();
                    break;

                case "btnReg":
                    SaveReg();
                    break;

                case "btnClose":
                    Close();
                    break;
            }
        }
        #endregion

        #region 저장
        private bool SaveReg()
        {
            //입력 체크--------------------------------------------------------------------------------------------
            //라인
            if ((cboLine_GP.SelectedIndex == -1) || (cboLine_GP.SelectedValue.ToString() == ""))
            {
                MessageBox.Show("라인을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLine_GP.Focus();
                return false;
            }
            //제품번들번호
            if (p_proc_gp == "MOD" && txtBundle_No.Text == "")
            {
                MessageBox.Show("제품번들번호를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPoc_No.Focus();
                return false;
            }
            //POC NO
            if (txtPoc_No.Text == "")
            {
                MessageBox.Show("POC를 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPoc_No.Focus();
                return false;
            }
            else if (txtSize.Text == "" && txtSteel.Text == "")
            {
                MessageBox.Show("등록된 POC가  아닙니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPoc_No.Focus();
                return false;
            }
            //작업일자
            if (dtpMfg_Date.Value.ToString().Length < 8)
            {
                MessageBox.Show("작업일자를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpMfg_Date.Focus();
                return false;
            }
            //근
            if ((cboWork_Type.SelectedIndex == -1) || (cboWork_Type.SelectedValue.ToString() == ""))
            {
                MessageBox.Show("근을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboWork_Type.Focus();
                return false;
            }
            //조
            //if ((cboWork_Team.SelectedIndex == -1) || (cboWork_Team.SelectedValue.ToString() == ""))
            //{
            //    MessageBox.Show("조를 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    cboWork_Team.Focus();
            //    return false;
            //}
            //결속본수
            if (vf.IsNumeric(txtPcs.Text) == false)
            {
                MessageBox.Show("(결속본수) 숫자로 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPcs.Focus();
                return false;
            }
            else if (txtPcs.Text == "" || int.Parse(txtPcs.Text) == 0)
            {
                MessageBox.Show("결속본수를 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPcs.Focus();
                return false;
            }
            //-----------------------------------------------------------------------------------------------------------

            //string ConnString = "Data Source = L2PRB; Persist Security Info = True; User ID = l2user_dev; Password = l2user_dev; Unicode = true";

            string spName = "SP_BND_WR_REG";  // 조회용 저장프로시저명

            //OracleConnection oConn = new OracleConnection(ConnString);
            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();

            try
            {
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;

                string sLineGp = "";
                string sWorkType = "";
                string sWorkTeam = "";

                if (cboLine_GP.SelectedIndex != -1) sLineGp = cboLine_GP.SelectedValue.ToString();
                if (cboWork_Type.SelectedIndex != -1) sWorkType = cboWork_Type.SelectedValue.ToString();
                if (cboWork_Team.SelectedIndex != -1) sWorkTeam = cboWork_Team.SelectedValue.ToString();

                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sLineGp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = txtPoc_No.Text;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_BUNDLE_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = txtBundle_No.Text;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MFG_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = dtpMfg_Date.Value.ToString("yyyyMMdd");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_START_TIME", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = mskWork_Start_Ddtt.Text.Replace(":", "");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_END_TIME", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = mskWork_End_Ddtt.Text.Replace(":", "");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_WORK_TYPE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sWorkType;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_WORK_TEAM", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sWorkTeam;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PCS", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = int.Parse(txtPcs.Text);
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = cS.UserID;    // 사용자 id 가져와서 입력
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                oConn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.ExecuteOracleScalar();

                string result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                string result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                transaction.Commit();

                MessageBox.Show(result_msg);
            }

            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다.");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();

                this.Close();
            }

            return true;
        }
        #endregion

        #region 조회
        private bool SetDataBinding()
        {
            if (p_proc_gp == "REG" && txtPoc_No.Text == "")
            {
                txtPoc_No.Focus();
                return false;
            }

            if (p_proc_gp == "MOD" && txtBundle_No.Text == "")
            {
                txtPoc_No.Focus();
                return false;
            }

            DataTable olddt = new DataTable();
            string sql1 = "";

            try
            {
                if (p_proc_gp == "REG")                   //입력
                {
                    sql1 = string.Format("SELECT A.MILL_NO     AS MILL_NO  ");
                    sql1 += string.Format("      ,A.POC_NO     AS POC_NO  ");
                    sql1 += string.Format("      ,A.HEAT       AS HEAT ");
                    sql1 += string.Format("      ,A.STEEL      AS STEEL ");
                    sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("      ,A.ITEM_SIZE  AS ITEM_SIZE ");
                    sql1 += string.Format("      ,A.LENGTH     AS ITEM_LENGTH    ");
                    sql1 += string.Format("      ,A.PCS        AS MILL_PCS  ");
                    sql1 += string.Format("      ,A.THEORY_WGT AS THEORY_WGT ");
                    sql1 += string.Format("      ,B.LINE_GP    AS LINE_GP ");
                    sql1 += string.Format("FROM   TB_CR_ORD_BUNDLEINFO A ");
                    sql1 += string.Format("      ,TB_CR_INPUT_WR       B ");
                    sql1 += string.Format("WHERE  A.POC_NO  = '{0}' ", txtPoc_No.Text);    //:POC_NO
                    sql1 += string.Format("AND    B.LINE_GP = '{0}' ", cboLine_GP.SelectedValue.ToString());             //:P_LINE_GP
                    sql1 += string.Format("AND    A.MILL_NO = B.MILL_NO(+) ");

                    olddt = cd.FindDataTable(sql1);

                    if (olddt == null || olddt.Rows.Count == 0)
                    {
                        MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtPoc_No.Focus();
                        return false;
                    }
                    else
                    {
                        txtSize.Text = olddt.Rows[0]["ITEM_SIZE"].ToString();
                        txtLength.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["ITEM_LENGTH"].ToString()));
                        txtHeat.Text = olddt.Rows[0]["HEAT"].ToString();
                        txtSteel.Text = olddt.Rows[0]["STEEL"].ToString();
                        txtSteel_Nm.Text = olddt.Rows[0]["STEEL_NM"].ToString();
                        txtTheory_Wgt.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));

                        btnConfirm.Enabled = true;
                    }
                }
                else if (p_proc_gp == "MOD")            //수정
                {
                    sql1 = string.Format("SELECT A.POC_NO      AS POC_NO  ");
                    sql1 += string.Format("      ,A.HEAT       AS HEAT ");
                    sql1 += string.Format("      ,A.STEEL      AS STEEL ");
                    sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("      ,A.ITEM_SIZE  AS ITEM_SIZE ");
                    sql1 += string.Format("      ,A.LENGTH     AS ITEM_LENGTH ");
                    sql1 += string.Format("      ,A.MFG_DATE   AS MFG_DATE  ");
                    sql1 += string.Format("      ,SUBSTR(A.WORK_START_DDTT, 9, 6) AS WORK_START_DDTT  ");
                    sql1 += string.Format("      ,SUBSTR(A.WORK_END_DDTT, 9, 6)   AS WORK_END_DDTT  ");
                    //sql1 += string.Format("      ,TO_CHAR(A.MFG_DATE,'YYYY-MM-DD') AS MFG_DATE ");
                    sql1 += string.Format("      ,A.WORK_TYPE  AS WORK_TYPE  ");
                    sql1 += string.Format("      ,A.WORK_TEAM  AS WORK_TEAM  ");
                    sql1 += string.Format("      ,A.PCS        AS MILL_PCS  ");
                    sql1 += string.Format("      ,A.THEORY_WGT AS THEORY_WGT ");
                    sql1 += string.Format("FROM   TB_BND_WR A ");
                    sql1 += string.Format("WHERE  A.BUNDLE_NO  = '{0}' ", txtBundle_No.Text);    //:번들번호
                    sql1 += string.Format("AND    A.LINE_GP = '{0}' ", cboLine_GP.SelectedValue.ToString());             //:P_LINE_GP
                    sql1 += string.Format("AND    NVL(A.DEL_YN,'N') <> 'Y' ");             //

                    olddt = cd.FindDataTable(sql1);

                    if (olddt == null || olddt.Rows.Count == 0)
                    {
                        MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtPoc_No.Focus();
                        return false;
                    }
                    else
                    {
                        //날짜 포맷: 쿼리문의 주석처리한 부분이 안되므로, 다음과 같이 처리
                        string sDate = olddt.Rows[0]["MFG_DATE"].ToString().Substring(0, 4) + "-" +
                                       olddt.Rows[0]["MFG_DATE"].ToString().Substring(4, 2) + "-" +
                                       olddt.Rows[0]["MFG_DATE"].ToString().Substring(6, 2);

                        txtPoc_No.Text = olddt.Rows[0]["POC_NO"].ToString();
                        txtSize.Text = olddt.Rows[0]["ITEM_SIZE"].ToString();
                        txtLength.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["ITEM_LENGTH"].ToString()));
                        txtHeat.Text = olddt.Rows[0]["HEAT"].ToString();
                        txtSteel.Text = olddt.Rows[0]["STEEL"].ToString();
                        txtSteel_Nm.Text = olddt.Rows[0]["STEEL_NM"].ToString();
                        dtpMfg_Date.Value = DateTime.Parse(sDate);
                        mskWork_Start_Ddtt.Text = olddt.Rows[0]["WORK_START_DDTT"].ToString();
                        mskWork_End_Ddtt.Text = olddt.Rows[0]["WORK_END_DDTT"].ToString();
                        cboWork_Type.SelectedValue = olddt.Rows[0]["WORK_TYPE"].ToString();
                        cboWork_Team.SelectedValue = olddt.Rows[0]["WORK_TEAM"].ToString();
                        txtPcs.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MILL_PCS"].ToString()));
                        txtTheory_Wgt.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));

                        btnConfirm.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            dtpMfg_Date.Focus();


            return true;
        }
        #endregion

        #region 이벤트
        private void txtPoc_No_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SetDataBinding();
        }
        #endregion
    }
}
