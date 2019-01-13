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
    public partial class WPIRsltPopUP : Form
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
        public WPIRsltPopUP(int LineGp, string MeasNo, string _scrAuth)
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

        public WPIRsltPopUP(int LineGp)
        {
            InitializeComponent();

            cboLine_GP_index = LineGp;

            p_proc_gp = "REG";       //입력

        }

        private void WPIRsltPopUP_Load(object sender, EventArgs e)
        {
            //txtPoc_No.Text = sMeasNo;
            //SetDataBinding1();
            SetComboBox();
            ClearForm();
            InitControl();

            txtPoc_No.Focus();
            //if (p_proc_gp == "REG") mskWork_Meas_Ddtt.Text = DateTime.Now.ToString("HH:mm:ss");//vf.Format(dateTime,"hhmmss");


        }

        private void WPIRsltPopUP_Shown(object sender, EventArgs e)
        {
            //사용자정의 이벤트
            EventCreate();
        }
        private void InitControl()
        {
            lblMeas_Date.ForeColor = Color.Blue;
            c1Label10.ForeColor = Color.Blue;
            c1Label11.ForeColor = Color.Blue;

            if (p_proc_gp == "MOD")
            {
                SetDataBinding();
                lblDel.Visible = true;
                cboDel.Visible = true;
                //btnConfirm.Visible = false;
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
                //btnConfirm.Visible = true;

                /*
                txtPoc_No.ReadOnly = true;
                txtBundle_No.ReadOnly = true;
                */
                //*
                txtPoc_No.ReadOnly = false;
                txtBundle_No.ReadOnly = false;
                txtPcs.ReadOnly = false;
                txtPoc_No.ReadOnly = false;
                SetDataBinding1();
                SetDataBinding2();
                SetDataBinding();
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
            txtPoc_No.Text = "";
            txtSize.Text = "";
            txtLength.Text = "";
            txtHeat.Text = "";
            txtSteel.Text = "";
            txtSteel_Nm.Text = "";
            txtPcs.Text = "";
            txtTheory_Wgt.Text = "";
            

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


                    //============================================
                    //중앙정렬

                    //첫번째 아이템 선택

                    //work_type = int.Parse(olddt.Rows[0]["WORK_TYPE"].ToString());


                }

                //조--------------------------------


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


                    //============================================

                    //중앙정렬

                    //첫번째 아이템 선택


                    //work_team = int.Parse(olddt.Rows[0]["WORK_TEAM"].ToString());


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
            if ((cboLine_GP.SelectedIndex == -1) || (line_ID == ""))
            {
                MessageBox.Show("라인을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLine_GP.Focus();
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

            //if ((cboWork_Team.SelectedIndex == -1) || (cboWork_Team.SelectedValue.ToString() == ""))
            //{
            //    MessageBox.Show("조를 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    cboWork_Team.Focus();
            //    return false;
            //}
            //실중량
 
            //-----------------------------------------------------------------------------------------------------------

            //string ConnString = "Data Source = L2PRB; Persist Security Info = True; User ID = l2user_dev; Password = l2user_dev; Unicode = true";

            string spName = "SP_WGT_WIR_REG";  // 조회용 저장프로시저명

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
                if (cboDel.SelectedIndex != -1) sDelGp = del_ID;
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
                op.Value = sBundleNo;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MFG_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = dtpMeas_Date.Value.ToString("yyyyMMdd");
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
                                 + " " + "BUNDLE_NO:" + sBundleNo
                                 + " " + "MEAS_DATE:" + dtpMeas_Date.Value.ToString("yyyyMMdd")
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
                                 + " " + "BUNDLE_NO:" + sBundleNo
                                 + " " + "MEAS_DATE:" + dtpMeas_Date.Value.ToString("yyyyMMdd")
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
                    //sql1 += string.Format("      ,A.BND_ORD_PCS        AS MILL_PCS  ");
                    //sql1 += string.Format("      ,FN_GET_WGT(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.BND_ORD_PCS) AS THEORY_WGT "); //A.THEORY_WGT AS THEORY_WGT
                    sql1 += string.Format("FROM   TB_CR_ORD A ");
                    sql1 += string.Format("WHERE  A.POC_NO  = '{0}' ", txtPoc_No.Text);      //POC_NO
                    //sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
                        

                    
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
                        //txtPcs.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MILL_PCS"].ToString()));
                        //txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));

                        //btnConfirm.Enabled = true;
                    }
                }
                else if (p_proc_gp == "MOD")            //수정
                {
                    sql1 = string.Format("SELECT POC_NO      AS POC_NO  ");
                    sql1 += string.Format("      ,BUNDLE_NO  AS BUNDLE_NO ");
                    sql1 += string.Format("      ,HEAT       AS HEAT ");
                    sql1 += string.Format("      ,STEEL      AS STEEL ");
                    sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = STEEL) AS STEEL_NM ");
                    sql1 += string.Format("      ,ITEM  AS ITEM ");
                    sql1 += string.Format("      ,ITEM_SIZE  AS ITEM_SIZE ");
                    sql1 += string.Format("      ,LENGTH     AS ITEM_LENGTH ");
                    sql1 += string.Format("      ,MFG_DATE   AS MFG_DATE  ");
                    sql1 += string.Format("      ,PCS        AS MILL_PCS  ");
                    sql1 += string.Format("      ,FN_GET_WGT(ITEM, ITEM_SIZE, LENGTH, PCS) AS THEORY_WGT  "); //B.THEORY_WGT AS THEORY_WGT
                    sql1 += string.Format("FROM   TB_WIP_WR ");
                    sql1 += string.Format("WHERE    BUNDLE_NO = '{0}' ", sMeasNo);            //번들번호


                    olddt = cd.FindDataTable(sql1);

                    if (olddt == null || olddt.Rows.Count == 0)
                    {

                        return false;
                    }
                    else
                    {
                        //날짜 포맷
                        string sDate = olddt.Rows[0]["MFG_DATE"].ToString().Substring(0, 4) + "-" +
                                       olddt.Rows[0]["MFG_DATE"].ToString().Substring(4, 2) + "-" +
                                       olddt.Rows[0]["MFG_DATE"].ToString().Substring(6, 2);

                        txtPoc_No.Text = olddt.Rows[0]["POC_NO"].ToString();
                        txtItem.Text = olddt.Rows[0]["ITEM"].ToString();
                        txtSize.Text = olddt.Rows[0]["ITEM_SIZE"].ToString();
                        txtLength.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["ITEM_LENGTH"].ToString()));
                        txtHeat.Text = olddt.Rows[0]["HEAT"].ToString();
                        txtSteel.Text = olddt.Rows[0]["STEEL"].ToString();
                        txtSteel_Nm.Text = olddt.Rows[0]["STEEL_NM"].ToString();
                        dtpMeas_Date.Value = DateTime.Parse(sDate);
                        //mskWork_Meas_Ddtt.Value = DateTime.Parse(sTime);// vf.CDate((olddt.Rows[0]["MEAS_DDTT"].ToString()));
                        //cboWork_Type.SelectedIndex = int.Parse(olddt.Rows[0]["WORK_TYPE"].ToString()) - 1;
                        //cboWork_Team.SelectedIndex = work_team_num;
                        txtPcs.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MILL_PCS"].ToString()));
                        //txtNet_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["NET_WGT"].ToString()));
                        txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));

                        //ComboBox_Bundle(olddt.Rows[0]["BUNDLE_NO"].ToString());       //번들번호: 수정시 콤보박스에 설정
                        txtBundle_No.Text = olddt.Rows[0]["BUNDLE_NO"].ToString();

                        //btnConfirm.Enabled = false;
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
                SetDataBinding2();
                SetDataBinding();
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
                //mskWork_Meas_Ddtt.Text = ck.Temptable.Rows[0]["END_DDTT"].ToString();
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
            //txtMeas_No.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MEAS_NO"].ToString()));
        }
        private bool SetDataBinding1()
        {
            string sLineGp = "";
            if (cboLine_GP_index == 0) sLineGp = "#1";
            if (cboLine_GP_index == 1) sLineGp = "#2";
            if (cboLine_GP_index == 2) sLineGp = "#3";

            olddt = new DataTable();
            string sql1 = "";
            //sql1 = string.Format("SELECT MIN(BUNDLE_NO) AS BUNDLE_NO ");
            //sql1 += string.Format("FROM   TB_BND_PLC_ORD A ");
            //sql1 += string.Format("WHERE  BUNDLE_NO  LIKE '{0}' || '%'", txtPoc_No.Text);
            //sql1 += string.Format("AND    NOT EXISTS ( SELECT BUNDLE_NO FROM TB_BND_WR");
            //sql1 += string.Format(" WHERE  BUNDLE_NO = A.BUNDLE_NO ) ");
            //sql1 += string.Format("AND    ROWNUM     = 1");

            sql1 = string.Format("SELECT ");
            sql1 += string.Format("POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = '{0}'", sLineGp);   //B.THEORY_WGT AS THEORY_WGT
            olddt = cd.FindDataTable(sql1);
            txtPoc_No.Text = olddt.Rows[0]["POC_NO"].ToString();
            return true;
        }

        private bool SetDataBinding2()
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
            sql1 += string.Format("FN_BUNDLE_NO_JAGONG('{0}') AS BUNDLE_NO FROM DUAL", txtPoc_No.Text);   //B.THEORY_WGT AS THEORY_WGT
            olddt = cd.FindDataTable(sql1);
            txtBundle_No.Text = olddt.Rows[0]["BUNDLE_NO"].ToString();
            return true;
        }
    }
}
