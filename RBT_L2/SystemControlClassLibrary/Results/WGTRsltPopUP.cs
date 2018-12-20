using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControlClassLibrary.UC.sub_UC;
using SystemControlClassLibrary.Results;

namespace SystemControlClassLibrary.Popup
{
    public partial class WGTRsltPopUP : Form
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
        private int THEORY_WGT = 0;
        private int NET_WGT = 0;

        private string sMeasNo = "";
        private string p_proc_gp = "";       //입력(REG), 수정(MOD)
        private string line_ID = "";
        private string line_NM = "";
        private string work_type_ID = "";
        private string work_type_NM = "";
        private string work_team_ID = "";
        private string work_team_NM = "";
        private string del_ID = "";
        private string del_NM = "";
        private string scrAuth;
        #endregion

        #region 화면
        public WGTRsltPopUP(int LineGp, string MeasNo, string _scrAuth)
        {
            scrAuth = _scrAuth;

            InitializeComponent();

            cboLine_GP_index = LineGp;
            sMeasNo = MeasNo;

            p_proc_gp = "MOD";                     //수정

            //scrAuth button 권한 적용
            ApplyAuth();

        }

        private void ApplyAuth()
        {
            string[] arrText = System.Text.RegularExpressions.Regex.Split(scrAuth, ",");
            string sProgramId = arrText[0];
            string InqAcl = arrText[1];
            string RegAcl = arrText[2];
            string ModAcl = arrText[3];
            string DelAcl = arrText[4];


            if (ModAcl == "Y")
            {
                btnReg.Enabled = true;
            }
            else
            {
                btnReg.Enabled = false;
            }
        }

        public WGTRsltPopUP(int LineGp)
        {
            InitializeComponent();

            cboLine_GP_index = LineGp;

            p_proc_gp = "REG";       //입력

        }

        private void WGTRsltPopUP_Load(object sender, EventArgs e)
        {
            txtPoc_No.Text = sMeasNo;

            SetComboBox();
            ClearForm();
            InitControl();

            txtPoc_No.Focus();
            if (p_proc_gp == "REG") mskWork_Meas_Ddtt.Text = DateTime.Now.ToString("HH:mm:ss");//vf.Format(dateTime,"hhmmss");


        }

        private void WGTRsltPopUP_Shown(object sender, EventArgs e)
        {
            //사용자정의 이벤트
            EventCreate();
        }
        private void InitControl()
        {
            lblWorkType.ForeColor = Color.Blue;
            lblWorkTeam.ForeColor = Color.Blue;
            lblMeas_Date.ForeColor = Color.Blue;
            lblMeasTime.ForeColor = Color.Blue;
            lblNet_Wgt.ForeColor = Color.Blue;
            c1Label10.ForeColor = Color.Blue;
            c1Label11.ForeColor = Color.Blue;

            if (p_proc_gp == "MOD")
            {
                SetDataBinding();
                lblDel.Visible = true;
                cboDel.Visible = true;
                btnConfirm.Visible = false;
                txtPoc_No.ReadOnly = true;


                //txtBundle_No.ReadOnly = true;

                /*
                txtBundle_No.ReadOnly = false;
                txtPcs.ReadOnly = false;
                txtMeas_No.ReadOnly = false;
                */
                txtPcs.ReadOnly = false;
                txtBundle_No.ReadOnly = false;
                //txtLength.ReadOnly = false;
            }
            else
            {
                lblDel.Visible = false;
                cboDel.Visible = false;
                btnConfirm.Visible = true;

                /*
                txtPoc_No.ReadOnly = true;
                txtBundle_No.ReadOnly = true;
                */
                //*
                txtPoc_No.ReadOnly = false;
                txtBundle_No.ReadOnly = false;
                txtPcs.ReadOnly = false;
                txtPoc_No.ReadOnly = false;
                //*/
                
            }




        }
        private void ClearForm()
        {
            //cboLine_GP.SelectedIndex = 0;
            //cboWork_Type.SelectedIndex = 0;
            //cboWork_Team.SelectedIndex = 0;
            dtpMeas_Date.Value = DateTime.Now;

            txtBundle_No.Text = "";
            txtMeas_No.Text = sMeasNo;
            txtPoc_No.Text = "";
            txtSize.Text = "";
            txtLength.Text = "";
            txtHeat.Text = "";
            txtSteel.Text = "";
            txtSteel_Nm.Text = "";
            mskWork_Meas_Ddtt.Text = "";
            txtPcs.Text = "";
            txtTheory_Wgt.Text = "";
            txtNet_Wgt.Text = "";

            //btnConfirm.Enabled = true;
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

                    //===========================================
                    List<string> list = new List<string>();
                    //list.Add("전체");

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        list.Add(dt.Rows[row].ItemArray[1].ToString());
                    }
                    foreach (var item in list)
                    {
                        cboLine_GP.Items.Add(item);
                    }

                    //============================================
                    //중앙정렬
                    cs.InitCombo(cboLine_GP, StringAlignment.Center);
                    //아이템 선택
                    cboLine_GP.SelectedIndex = cboLine_GP_index;
                }

                //근--------------------------------
                //cboWork_Type.Items.Clear();

                DataTable dt1 = cd.Find_CD("WORK_TYPE");

                if (dt1.Rows.Count > 0)
                {
                    //콤보박스 설정
                    //cboWork_Type.DataSource = dt1;

                    //cboWork_Type.ValueMember = dt1.Columns["CD_ID"].ToString();
                    //cboWork_Type.DisplayMember = dt1.Columns["CD_NM"].ToString();
                    //===========================================
                    List<string> list1 = new List<string>();
                    //list.Add("전체");

                    for (int row = 0; row < dt1.Rows.Count; row++)
                    {
                        list1.Add(dt1.Rows[row].ItemArray[1].ToString());
                    }
                    foreach (var item in list1)
                    {
                        cboWork_Type.Items.Add(item);
                    }

                    //============================================
                    //중앙정렬
                    cs.InitCombo(cboWork_Type, StringAlignment.Center);
                    //첫번째 아이템 선택

                    //work_type = int.Parse(olddt.Rows[0]["WORK_TYPE"].ToString());

                    cboWork_Type.SelectedIndex = work_type;
                }

                //조--------------------------------
                cboWork_Team.Items.Clear();

                DataTable dt2 = cd.Find_CD("WORK_TEAM");

                if (dt2.Rows.Count > 0)
                {
                    //콤보박스 설정
                    //cboWork_Team.DataSource = dt2;

                    //cboWork_Team.ValueMember = dt2.Columns["CD_ID"].ToString();
                    //cboWork_Team.DisplayMember = dt2.Columns["CD_NM"].ToString();
                    //===========================================
                    List<string> list2 = new List<string>();
                    //list.Add("전체");

                    for (int row = 0; row < dt2.Rows.Count; row++)
                    {
                        list2.Add(dt2.Rows[row].ItemArray[1].ToString());
                    }
                    foreach (var item in list2)
                    {
                        cboWork_Team.Items.Add(item);
                    }

                    //============================================

                    //중앙정렬
                    cs.InitCombo(cboWork_Team, StringAlignment.Center);
                    //첫번째 아이템 선택


                    //work_team = int.Parse(olddt.Rows[0]["WORK_TEAM"].ToString());

                    cboWork_Team.SelectedIndex = work_team;
                }
                // 삭제유무Y/N --------------------------------
                cboDel.Items.Clear();

                DataTable dt3 = cd.Find_CD("DEL_YN");

                if (dt3.Rows.Count > 0)
                {
                    //콤보박스 설정
                    //cboDel.DataSource = dt3;

                    //cboDel.ValueMember = dt3.Columns["CD_ID"].ToString();
                    //cboDel.DisplayMember = dt3.Columns["CD_NM"].ToString();
                    //===========================================
                    List<string> list3 = new List<string>();
                    //list.Add("전체");

                    for (int row = 0; row < dt3.Rows.Count; row++)
                    {
                        list3.Add(dt3.Rows[row].ItemArray[1].ToString());
                    }
                    foreach (var item in list3)
                    {
                        cboDel.Items.Add(item);
                    }

                    //============================================
                    //중앙정렬
                    cs.InitCombo(cboDel, StringAlignment.Center);
                    //첫번째 아이템 선택
                    cboDel.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }
        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cboLine_GP.DataSource = dt;

            //cboLine_GP.ValueMember = dt.Columns["CD_ID"].ToString();
            //cboLine_GP.DisplayMember = dt.Columns["CD_NM"].ToString();
            line_NM = string.Empty;
            if (cboLine_GP.SelectedIndex >= 0)
            {
                line_NM = cboLine_GP.SelectedItem as string;
                line_ID = cd.Find_CD_ID("LINE_GP", line_NM);
            }
        }

        private void cboWork_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_type_NM = string.Empty;
            if (cboWork_Type.SelectedIndex >= 0)
            {
                work_type_NM = cboWork_Type.SelectedItem as string;
                work_type_ID = cd.Find_CD_ID("WORK_TYPE", work_type_NM);
            }
        }

        private void cboWork_Team_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_NM = string.Empty;
            if (cboWork_Team.SelectedIndex >= 0)
            {
                work_team_NM = cboWork_Team.SelectedItem as string;
                work_team_ID = cd.Find_CD_ID("WORK_TEAM", work_team_NM);
            }
        }

        private void cboDel_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_NM = string.Empty;
            if (cboDel.SelectedIndex >= 0)
            {
                del_NM = cboDel.SelectedItem as string;
                del_ID = cd.Find_CD_ID("DEL_YN", del_NM);
            }
        }
        #endregion

        #region 버튼
        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnConfirm":
                    btnReg_Click(sender, e);
                    break;

                case "btnReg":
                    SaveReg();
                    break;
                case "btnRegs":
                    SaveRegs();
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

            THEORY_WGT = vf.CInt(txtTheory_Wgt.Text);
            NET_WGT = vf.CInt(txtNet_Wgt.Text);

            if ((cboLine_GP.SelectedIndex == -1) || (line_ID == ""))
            {
                MessageBox.Show("라인을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLine_GP.Focus();
                return false;
            }
            //계량번호
            if (p_proc_gp == "MOD" && txtMeas_No.Text == "")
            {
                MessageBox.Show("계량번호를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            //제품번들번호
            if (txtBundle_No.Text == "")
            {
                MessageBox.Show("제품번들번호를 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBundle_No.Focus();
                return false;
            }
            //계근일자
            if (dtpMeas_Date.Value.ToString().Length < 8)
            {
                MessageBox.Show("계근일자를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpMeas_Date.Focus();
                return false;
            }
            //근
            if ((cboWork_Type.SelectedIndex == -1) || (work_type_ID == ""))
            {
                MessageBox.Show("근을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboWork_Type.Focus();
                return false;
            }

            if ((THEORY_WGT - NET_WGT > 100) || (NET_WGT - THEORY_WGT > 100))
            {
                MessageBox.Show("이론중량과 차이가 100이상입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNet_Wgt.Focus();
                return false;
            }

            //조
            //if ((cboWork_Team.SelectedIndex == -1) || (cboWork_Team.SelectedValue.ToString() == ""))
            //{
            //    MessageBox.Show("조를 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    cboWork_Team.Focus();
            //    return false;
            //}
            //실중량
            if (vf.IsNumeric(txtNet_Wgt.Text) == false)
            {
                MessageBox.Show("(실중량) 숫자로 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNet_Wgt.Focus();
                return false;
            }
            else if (txtNet_Wgt.Text == "" || double.Parse(txtNet_Wgt.Text) == 0)
            {
                MessageBox.Show("실중량을 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNet_Wgt.Focus();
                return false;
            }
            //-----------------------------------------------------------------------------------------------------------

            //string ConnString = "Data Source = L2PRB; Persist Security Info = True; User ID = l2user_dev; Password = l2user_dev; Unicode = true";

            string spName = "SP_WGT_WR_REG";  // 조회용 저장프로시저명

            //OracleConnection oConn = new OracleConnection(ConnString);
            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();


            try
            {
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;
                string sDelGp = "";
                string sLineGp = "";
                string sBundleNo = "";
                string sWorkType = "";
                string sWorkTeam = "";

                if (cboLine_GP.SelectedIndex != -1) sLineGp = line_ID;
                if (txtBundle_No.Text != "") sBundleNo = txtBundle_No.Text;
                if (cboWork_Type.SelectedIndex != -1) sWorkType = work_type_ID;
                if (cboWork_Team.SelectedIndex != -1) sWorkTeam = work_team_ID;
                if (cboDel.SelectedIndex != -1) sDelGp = del_ID;
                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sLineGp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = txtPoc_No.Text;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MEAS_NO", OracleType.Number);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.CInt2(txtMeas_No.Text); //int.Parse(txtMeas_No.Text);
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_BUNDLE_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sBundleNo;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MEAS_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = dtpMeas_Date.Value.ToString("yyyyMMdd");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MEAS_TIME", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.Format(mskWork_Meas_Ddtt.Value, "HHmmss");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_WORK_TYPE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sWorkType;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_WORK_TEAM", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sWorkTeam;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_NET_WGT", OracleType.Number);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.CInt(txtNet_Wgt.Text);
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_DEL_YN", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sDelGp;    // 삭제유무 Y/N
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = cS.UserID;    // 사용자 id 가져와서 입력
                cmd.Parameters.Add(op);

                //계중본수수정테스트 신귀현(20170110)
                op = new OracleParameter("P_PCS", OracleType.Number);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.CInt(txtPcs.Text);
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

                if (result_stat == "ERR")
                {
                    string error_msg = "LINE_GP:" + sLineGp
                                 + " " + "POC_NO:" + txtPoc_No.Text
                                 + " " + "MEAS_NO:" + vf.CInt2(txtMeas_No.Text)
                                 + " " + "BUNDLE_NO:" + sBundleNo
                                 + " " + "MEAS_DATE:" + dtpMeas_Date.Value.ToString("yyyyMMdd")
                                 + " " + "MEAS_TIME:" + vf.Format(mskWork_Meas_Ddtt.Value, "HHmmss")
                                 + " " + "WORK_TYPE:" + sWorkType
                                 + " " + "WORK_TEAM:" + sWorkTeam
                                 + " " + "NET_WGT:" + vf.CInt(txtNet_Wgt.Text)
                                 + " " + "DEL_YN:" + sDelGp
                                 + " " + "User_id:" + ck.UserID
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), error_msg);

                    MessageBox.Show(result_msg);
                }
                else
                {
                    string success_msg = "LINE_GP:" + sLineGp
                                 + " " + "POC_NO:" + txtPoc_No.Text
                                 + " " + "MEAS_NO:" + vf.CInt2(txtMeas_No.Text)
                                 + " " + "BUNDLE_NO:" + sBundleNo
                                 + " " + "MEAS_DATE:" + dtpMeas_Date.Value.ToString("yyyyMMdd")
                                 + " " + "MEAS_TIME:" + vf.Format(mskWork_Meas_Ddtt.Value, "HHmmss")
                                 + " " + "WORK_TYPE:" + sWorkType
                                 + " " + "WORK_TEAM:" + sWorkTeam
                                 + " " + "NET_WGT:" + vf.CInt(txtNet_Wgt.Text)
                                 + " " + "DEL_YN:" + sDelGp
                                 + " " + "User_id:" + ck.UserID
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.InSerted, Text, clsMsg.Log.__Line(), success_msg);
                    Close();
                }
                //MessageBox.Show(result_msg);
                
            }

            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show(ex.Message);
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

                //this.Close();
            }

            return true;
        }
        #endregion

        #region 저장
        private bool SaveRegs()
        {
            //입력 체크--------------------------------------------------------------------------------------------
            //라인

            THEORY_WGT = vf.CInt(txtTheory_Wgt.Text);
            NET_WGT = vf.CInt(txtNet_Wgt.Text);

            if ((cboLine_GP.SelectedIndex == -1) || (line_ID == ""))
            {
                MessageBox.Show("라인을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLine_GP.Focus();
                return false;
            }
            //계량번호
            if (p_proc_gp == "MOD" && txtMeas_No.Text == "")
            {
                MessageBox.Show("계량번호를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            //제품번들번호
            if (txtBundle_No.Text == "")
            {
                MessageBox.Show("제품번들번호를 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBundle_No.Focus();
                return false;
            }
            //계근일자
            if (dtpMeas_Date.Value.ToString().Length < 8)
            {
                MessageBox.Show("계근일자를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpMeas_Date.Focus();
                return false;
            }
            //근
            if ((cboWork_Type.SelectedIndex == -1) || (work_type_ID == ""))
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
            //실중량
            if (vf.IsNumeric(txtNet_Wgt.Text) == false)
            {
                MessageBox.Show("(실중량) 숫자로 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNet_Wgt.Focus();
                return false;
            }
            else if (txtNet_Wgt.Text == "" || double.Parse(txtNet_Wgt.Text) == 0)
            {
                MessageBox.Show("실중량을 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNet_Wgt.Focus();
                return false;
            }
            //-----------------------------------------------------------------------------------------------------------

            //string ConnString = "Data Source = L2PRB; Persist Security Info = True; User ID = l2user_dev; Password = l2user_dev; Unicode = true";

            string spName = "SP_WGT_WR_REGS";  // 조회용 저장프로시저명

            //OracleConnection oConn = new OracleConnection(ConnString);
            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();


            try
            {
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;
                string sDelGp = "";
                string NET_MINUS = "Y";
                string sLineGp = "";
                string sBundleNo = "";
                string sWorkType = "";
                string sWorkTeam = "";

                if (cboLine_GP.SelectedIndex != -1) sLineGp = line_ID;
                if (txtBundle_No.Text != "") sBundleNo = txtBundle_No.Text;
                if (cboWork_Type.SelectedIndex != -1) sWorkType = work_type_ID;
                if (cboWork_Team.SelectedIndex != -1) sWorkTeam = work_team_ID;
                if (cboDel.SelectedIndex != -1) sDelGp = del_ID;
                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sLineGp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = txtPoc_No.Text;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MEAS_NO", OracleType.Number);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.CInt2(txtMeas_No.Text); //int.Parse(txtMeas_No.Text);
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_BUNDLE_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sBundleNo;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MEAS_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = dtpMeas_Date.Value.ToString("yyyyMMdd");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MEAS_TIME", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.Format(mskWork_Meas_Ddtt.Value, "HHmmss");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_WORK_TYPE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sWorkType;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_WORK_TEAM", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sWorkTeam;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_NET_WGT", OracleType.Number);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.CInt(txtNet_Wgt.Text);
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_DEL_YN", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = sDelGp;    // 삭제유무 Y/N
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = cS.UserID;    // 사용자 id 가져와서 입력
                cmd.Parameters.Add(op);

                //계중본수수정테스트 신귀현(20170110)
                op = new OracleParameter("P_PCS", OracleType.Number);
                op.Direction = ParameterDirection.Input;
                op.Value = vf.CInt(txtPcs.Text);
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

                if (result_stat == "ERR")
                {
                    string error_msg = "LINE_GP:" + sLineGp
                                 + " " + "POC_NO:" + txtPoc_No.Text
                                 + " " + "MEAS_NO:" + vf.CInt2(txtMeas_No.Text)
                                 + " " + "BUNDLE_NO:" + sBundleNo
                                 + " " + "MEAS_DATE:" + dtpMeas_Date.Value.ToString("yyyyMMdd")
                                 + " " + "MEAS_TIME:" + vf.Format(mskWork_Meas_Ddtt.Value, "HHmmss")
                                 + " " + "WORK_TYPE:" + sWorkType
                                 + " " + "WORK_TEAM:" + sWorkTeam
                                 + " " + "NET_WGT:" + vf.CInt(txtNet_Wgt.Text)
                                 + " " + "DEL_YN:" + sDelGp
                                 + " " + "User_id:" + ck.UserID
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), error_msg);

                    MessageBox.Show(result_msg);
                }
                else
                {
                    string success_msg = "LINE_GP:" + sLineGp
                                 + " " + "POC_NO:" + txtPoc_No.Text
                                 + " " + "MEAS_NO:" + vf.CInt2(txtMeas_No.Text)
                                 + " " + "BUNDLE_NO:" + sBundleNo
                                 + " " + "MEAS_DATE:" + dtpMeas_Date.Value.ToString("yyyyMMdd")
                                 + " " + "MEAS_TIME:" + vf.Format(mskWork_Meas_Ddtt.Value, "HHmmss")
                                 + " " + "WORK_TYPE:" + sWorkType
                                 + " " + "WORK_TEAM:" + sWorkTeam
                                 + " " + "NET_WGT:" + vf.CInt(txtNet_Wgt.Text)
                                 + " " + "DEL_YN:" + sDelGp
                                 + " " + "User_id:" + ck.UserID
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.InSerted, Text, clsMsg.Log.__Line(), success_msg);
                    Close();
                }
                //MessageBox.Show(result_msg);

            }

            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show(ex.Message);
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

                //this.Close();
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

            if (p_proc_gp == "MOD" && txtMeas_No.Text == "")
            {
                txtPoc_No.Focus();
                return false;
            }

            olddt = new DataTable();
            string sql1 = "";

            try
            {
                if (p_proc_gp == "REG")                   //입력
                {
                    sql1 = string.Format("SELECT A.POC_NO      AS POC_NO   ");
                    sql1 += string.Format("      ,A.HEAT       AS HEAT ");
                    sql1 += string.Format("      ,A.STEEL      AS STEEL ");
                    sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("      ,A.ITEM  AS ITEM ");
                    sql1 += string.Format("      ,A.ITEM_SIZE  AS ITEM_SIZE ");
                    sql1 += string.Format("      ,A.LENGTH     AS ITEM_LENGTH    ");
                    
                    //if (line_ID == "#3")
                    //{
                    //    sql1 += string.Format("      ,A.PCS        AS MILL_PCS  ");
                    //    sql1 += string.Format("      ,FN_GET_WGT(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.PCS) AS THEORY_WGT "); //A.THEORY_WGT AS THEORY_WGT
                    //    sql1 += string.Format("FROM   TB_BND_WR A ");
                    //    sql1 += string.Format("WHERE  A.POC_NO  = '{0}' ", txtPoc_No.Text);      //POC_NO
                    //    sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
                    //    sql1 += string.Format("AND    NVL(A.DEL_YN,'N') <> 'Y' ");
                    //}
                    if (line_ID == "#3")
                    {
                        sql1 += string.Format("      ,A.BND_ORD_PCS        AS MILL_PCS  ");
                        sql1 += string.Format("      ,FN_GET_WGT(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.BND_ORD_PCS) AS THEORY_WGT "); //A.THEORY_WGT AS THEORY_WGT
                        sql1 += string.Format("FROM   TB_BND_PLC_ORD A ");
                        sql1 += string.Format("WHERE  A.POC_NO  = '{0}' ", txtPoc_No.Text);      //POC_NO
                        sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
                        
                    }
                    else if (line_ID == "#1")
                    {
                        sql1 += string.Format("      ,A.BND_ORD_PCS        AS MILL_PCS  ");
                        sql1 += string.Format("      ,FN_GET_WGT(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.BND_ORD_PCS) AS THEORY_WGT "); //A.THEORY_WGT AS THEORY_WGT
                        sql1 += string.Format("FROM   TB_BND_PLC_ORD_NO1 A ");
                        sql1 += string.Format("WHERE  A.POC_NO  = '{0}' ", txtPoc_No.Text);      //POC_NO
                        sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
                        
                    }
                    else if (line_ID == "#2")
                    {
                        sql1 += string.Format("      ,A.BND_ORD_PCS        AS MILL_PCS  ");
                        sql1 += string.Format("      ,FN_GET_WGT(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.BND_ORD_PCS) AS THEORY_WGT "); //A.THEORY_WGT AS THEORY_WGT
                        sql1 += string.Format("FROM   TB_BND_PLC_ORD_NO2 A ");
                        sql1 += string.Format("WHERE  A.POC_NO  = '{0}' ", txtPoc_No.Text);      //POC_NO
                        sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
                        
                    }
                    olddt = cd.FindDataTable(sql1);

                    if (olddt == null || olddt.Rows.Count == 0)
                    {

                        return false;
                    }
                    else
                    {
                        txtItem.Text = olddt.Rows[0]["ITEM"].ToString();
                        txtSize.Text = olddt.Rows[0]["ITEM_SIZE"].ToString();
                        txtLength.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["ITEM_LENGTH"].ToString()));
                        txtHeat.Text = olddt.Rows[0]["HEAT"].ToString();
                        txtSteel.Text = olddt.Rows[0]["STEEL"].ToString();
                        txtSteel_Nm.Text = olddt.Rows[0]["STEEL_NM"].ToString();
                        txtPcs.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MILL_PCS"].ToString()));
                        txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));

                        btnConfirm.Enabled = true;
                    }
                }
                else if (p_proc_gp == "MOD")            //수정
                {
                    sql1 = string.Format("SELECT B.POC_NO      AS POC_NO  ");
                    sql1 += string.Format("      ,A.BUNDLE_NO  AS BUNDLE_NO ");
                    sql1 += string.Format("      ,B.HEAT       AS HEAT ");
                    sql1 += string.Format("      ,B.STEEL      AS STEEL ");
                    sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("      ,B.ITEM  AS ITEM ");
                    sql1 += string.Format("      ,B.ITEM_SIZE  AS ITEM_SIZE ");
                    sql1 += string.Format("      ,B.LENGTH     AS ITEM_LENGTH ");
                    sql1 += string.Format("      ,A.MEAS_DATE   AS MEAS_DATE  ");
                    sql1 += string.Format("      ,SUBSTR(A.MEAS_DDTT, 9, 6) AS MEAS_DDTT  ");
                    sql1 += string.Format("      ,A.WORK_TYPE  AS WORK_TYPE  ");
                    sql1 += string.Format("      ,A.WORK_TEAM  AS WORK_TEAM  ");
                    sql1 += string.Format("      ,B.PCS        AS MILL_PCS  ");
                    sql1 += string.Format("      ,A.NET_WGT    AS NET_WGT  ");
                    sql1 += string.Format("      ,FN_GET_WGT(B.ITEM, B.ITEM_SIZE, B.LENGTH, B.PCS) AS THEORY_WGT "); //B.THEORY_WGT AS THEORY_WGT
                    sql1 += string.Format("FROM   TB_WGT_WR A, ");
                    sql1 += string.Format("       TB_BND_WR B ");
                    sql1 += string.Format("WHERE  A.MEAS_NO  = '{0}' ", txtMeas_No.Text);                                //계량번호
                    //sql1 += string.Format("AND    A.LINE_GP = '{0}' ", cboLine_GP.SelectedValue.ToString());             //LINE_GP
                    //sql1 += string.Format("AND    A.LINE_GP = B.LINE_GP ");                 //LINE_GP
                    sql1 += string.Format("AND    A.BUNDLE_NO = B.BUNDLE_NO ");             //번들번호
                    sql1 += string.Format("AND    NVL(A.DEL_YN,'N') <> 'Y' ");
                    sql1 += string.Format("AND    NVL(B.DEL_YN,'N') <> 'Y' ");

                    olddt = cd.FindDataTable(sql1);

                    if (olddt == null || olddt.Rows.Count == 0)
                    {

                        return false;
                    }
                    else
                    {
                        //날짜 포맷
                        string sDate = olddt.Rows[0]["MEAS_DATE"].ToString().Substring(0, 4) + "-" +
                                       olddt.Rows[0]["MEAS_DATE"].ToString().Substring(4, 2) + "-" +
                                       olddt.Rows[0]["MEAS_DATE"].ToString().Substring(6, 2);
                        //시간 포맷
                        string sTime = olddt.Rows[0]["MEAS_DDTT"].ToString().Substring(0, 2) + ":" +
                                       olddt.Rows[0]["MEAS_DDTT"].ToString().Substring(2, 2) + ":" +
                                       olddt.Rows[0]["MEAS_DDTT"].ToString().Substring(4, 2);



                        int work_team_num = 0;
                        if (olddt.Rows[0]["WORK_TEAM"].ToString() == "A")
                        {
                            work_team_num = 0;
                        }
                        else if (olddt.Rows[0]["WORK_TEAM"].ToString() == "B")
                        {
                            work_team_num = 1;
                        }
                        else
                        {
                            work_team_num = 2;
                        }

                        txtPoc_No.Text = olddt.Rows[0]["POC_NO"].ToString();
                        txtItem.Text = olddt.Rows[0]["ITEM"].ToString();
                        txtSize.Text = olddt.Rows[0]["ITEM_SIZE"].ToString();
                        txtLength.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["ITEM_LENGTH"].ToString()));
                        txtHeat.Text = olddt.Rows[0]["HEAT"].ToString();
                        txtSteel.Text = olddt.Rows[0]["STEEL"].ToString();
                        txtSteel_Nm.Text = olddt.Rows[0]["STEEL_NM"].ToString();
                        dtpMeas_Date.Value = DateTime.Parse(sDate);
                        mskWork_Meas_Ddtt.Value = DateTime.Parse(sTime);// vf.CDate((olddt.Rows[0]["MEAS_DDTT"].ToString()));
                        cboWork_Type.SelectedIndex = int.Parse(olddt.Rows[0]["WORK_TYPE"].ToString()) - 1;
                        cboWork_Team.SelectedIndex = work_team_num;
                        txtPcs.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MILL_PCS"].ToString()));
                        txtNet_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["NET_WGT"].ToString()));
                        txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));

                        //ComboBox_Bundle(olddt.Rows[0]["BUNDLE_NO"].ToString());       //번들번호: 수정시 콤보박스에 설정
                        txtBundle_No.Text = olddt.Rows[0]["BUNDLE_NO"].ToString();

                        btnConfirm.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            dtpMeas_Date.Focus();


            return true;
        }
        #endregion

        #region "사용자정의 이벤트"
        //---이벤트 생성
        private void EventCreate()
        {
            // this.txtPoc_No.LostFocus += new System.EventHandler(txtPoc_No_LostFocus);            //POC NO
        }

        //POC NO(LostFocus)
        private void txtPoc_No_LostFocus(object sender, EventArgs e)
        {
            //if (p_proc_gp == "REG") ComboBox_Bundle("");       //번들번호: 입력시 콤보박스에 설정
        }

        private void txtNet_Wgt_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }
        #endregion

        #region 이벤트
        private void txtPoc_No_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetDataBinding1(); SetDataBinding();
            }
        }

        private void txtPcs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                olddt = new DataTable();
                string sql1 = "";
                sql1 = string.Format("SELECT  ");
                sql1 += string.Format("FN_GET_WGT('{0}', '{1}', '{2}', '{3}') AS THEORY_WGT FROM DUAL", txtItem.Text, txtSize.Text, txtLength.Text, txtPcs.Text);   //B.THEORY_WGT AS THEORY_WGT

                olddt = cd.FindDataTable(sql1);
                txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            //C1FlexGrid grd = new C1FlexGrid();
            WGTRsltSecPopUP popup2 = new WGTRsltSecPopUP(line_ID);
            popup2.Owner = this; //A폼을 지정하게 된다.
            popup2.StartPosition = FormStartPosition.CenterScreen;
            popup2.ShowDialog();

            try
            {
                string sql1 = string.Empty;

                if (ck.Temptable == null)
                {
                    return;
                }
                if (ck.Temptable.Rows.Count > 0)
                {
                    ck.Temptable.Rows[0]["L_NO"].ToString();

                    SetupParameter();
                    //ck.setDateTime = sDate;
                    // ck.line_Selected_Index = cboLine_GP.SelectedIndex;
                    ck.Temptable = null;

                }
            }
            catch (Exception ex)
            {

            }

        }
        private void SetupParameter()
        {
            C1FlexGrid grd = new C1FlexGrid();
            if (grd.Rows.Count > 1)
            {
                string mfg_Date = ck.Temptable.Rows[0]["MFG_DATE"].ToString().Substring(0, 4) + "-" +
                                  ck.Temptable.Rows[0]["MFG_DATE"].ToString().Substring(5, 2) + "-" +
                                  ck.Temptable.Rows[0]["MFG_DATE"].ToString().Substring(8, 2);


                txtBundle_No.Text = ck.Temptable.Rows[0]["BUNDLE_NO"].ToString();
                txtPoc_No.Text = ck.Temptable.Rows[0]["POC_NO"].ToString();
                txtHeat.Text = ck.Temptable.Rows[0]["HEAT"].ToString();
                txtSteel.Text = ck.Temptable.Rows[0]["STEEL"].ToString();
                txtSteel_Nm.Text = ck.Temptable.Rows[0]["STEEL_NM"].ToString();
                txtItem.Text = ck.Temptable.Rows[0]["ITEM"].ToString();
                txtSize.Text = ck.Temptable.Rows[0]["ITEM_SIZE"].ToString();
                txtLength.Text = string.Format("{0:#,0.00}", double.Parse(ck.Temptable.Rows[0]["LENGTH"].ToString()));  //ck.Temptable.Rows[0]["LENGTH"].ToString()
                txtPcs.Text = ck.Temptable.Rows[0]["PCS"].ToString();
                dtpMeas_Date.Value = vf.CDate(mfg_Date);
                mskWork_Meas_Ddtt.Text = ck.Temptable.Rows[0]["END_DDTT"].ToString();
                txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(ck.Temptable.Rows[0]["THEORY_WGT"].ToString())); //ck.Temptable.Rows[0]["THEORY_WGT"].ToString();

            }
            else
            {
                txtBundle_No.Text = "";
                txtPoc_No.Text = "";
                txtHeat.Text = "";
                txtSteel.Text = "";
                txtSteel_Nm.Text = "";
                txtSize.Text = "";
                txtLength.Text = "";
                txtPcs.Text = "";
                txtTheory_Wgt.Text = "";
            }
        }



        #endregion

        private void btnConfirm1_Click(object sender, EventArgs e)
        {
            olddt = new DataTable();
            string sql1 = "";
            sql1 = string.Format("SELECT  ");
            sql1 += string.Format("FN_GET_WGT('{0}', '{1}', '{2}', '{3}') AS THEORY_WGT FROM DUAL", txtItem.Text, txtSize.Text, txtLength.Text, txtPcs.Text);   //B.THEORY_WGT AS THEORY_WGT

            olddt = cd.FindDataTable(sql1);
            txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));
        }

        private void btnConfirm2_Click(object sender, EventArgs e)
        {
            olddt = new DataTable();
            string sql1 = "";
            sql1 = string.Format("SELECT NVL(MAX(MEAS_NO),0) + 1 AS MEAS_NO  ");
            sql1 += string.Format("FROM  TB_WGT_WR");   //B.THEORY_WGT AS THEORY_WGT

            olddt = cd.FindDataTable(sql1);
            txtMeas_No.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MEAS_NO"].ToString()));
        }
        private bool SetDataBinding1()
        {
            olddt = new DataTable();
            string sql1 = "";
            //sql1 = string.Format("SELECT MIN(BUNDLE_NO) AS BUNDLE_NO ");
            //sql1 += string.Format("FROM   TB_BND_PLC_ORD A ");
            //sql1 += string.Format("WHERE  BUNDLE_NO  LIKE '{0}' || '%'", txtPoc_No.Text);
            //sql1 += string.Format("AND    NOT EXISTS ( SELECT BUNDLE_NO FROM TB_BND_WR");
            //sql1 += string.Format(" WHERE  BUNDLE_NO = A.BUNDLE_NO ) ");
            //sql1 += string.Format("AND    ROWNUM     = 1");

            sql1 = string.Format("SELECT  ");
            sql1 += string.Format("FN_BUNDLE_NO_NEW('{0}') AS BUNDLE_NO FROM DUAL", txtPoc_No.Text);   //B.THEORY_WGT AS THEORY_WGT
            olddt = cd.FindDataTable(sql1);
            txtBundle_No.Text = olddt.Rows[0]["BUNDLE_NO"].ToString();
            return true;
        }
    }
}
