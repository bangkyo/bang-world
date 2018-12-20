﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using static ComLib.clsUtil;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using SystemControlClassLibrary.Popup;
using System.Data.OracleClient;
using System.Drawing;
using System.Windows.Forms;

namespace SystemControlClassLibrary.information
{
    public partial class ExpRsltReg : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable checkdt;

        ListDictionary ngTest_Value = null;
        ListDictionary fault_cd = null;

        List<string> msg;
        List<string> modifList;
        List<string> checkList;
        List<string> good_ng_List;

        clsStyle cs = new clsStyle();

        // 셀의 수정전 값
        private  string strBefValue = "";

        private  string ownerNM = "";
        private  string titleNM = "";

        private string heat = "";
        private string steel_id = "";
        private  string line_gp_id = "";
        private  string work_type_id = "";
        private  string work_team_id = "";
        private string gp = "";
        private string poc = "";
        private  string gangjung_id = "";

        private string workteam_id = "";
        private string workteam_nm = "";

        private string start_dt_str = "";
        private string end_dt_str = "";

        #endregion 변수 설정

        #region 생성자, 로딩 설정
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public ExpRsltReg(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }
        ~ExpRsltReg()
        {

        }
        private void ExpRsltReg_Load(object sender, EventArgs e)
        {
            InitControl();

            EventCreate();      //사용자정의 이벤트

            btnDisplay_Click(null, null);
        }
        #endregion 생성자, 로딩 설정

        #region Init control,grid 설정

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(lblHeat);
            cs.InitLabel(lblSteel);
            cs.InitLabel(lblLine);
            cs.InitLabel(lblGubun);
            cs.InitLabel(lblPoc);
            cs.InitLabel(lblWorkType);
            cs.InitLabel(lblMfgDate);
            cs.InitLabel(lblTEAM);

            cs.InitButton(btnExcel);
            cs.InitButton(btnSave);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            cs.InitCombo(cbo_Work_Type, StringAlignment.Near);
            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            cs.InitCombo(cboGubun_GP, StringAlignment.Near);
            cs.InitCombo(cboTEAM, StringAlignment.Near);

            cs.InitTextBox(txtPoc);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(txtHeat);
            cs.InitTextBox(gangjong_Nm_tb);

            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            SetComboBox1();
            SetComboBox2();
            SetComboBox3();
            SetComboBox4();

            InitGrd_Main();
        }
        private void End_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void Start_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["MPI_FAULT_CD"].Index, 0, grdMain.Cols["BAD_LENGTH"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];
            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["CHECKER"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];
            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["GOOD_YN"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            grdMain.AllowSorting = AllowSortingEnum.SingleColumn;
            
            int level1 = 50; // 2자리
            int level2 = 60; // 4자리
            int level3 = 100; // 6자리
            int level4 = 140; // 8자리이상

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["CHECKER"].Width = cs.Checker_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;
            grdMain.Cols["LINE_GP"].Width = 0;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TIME"].Width = cs.TIME_8_Width;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width -5;
            grdMain.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width -5;
            grdMain.Cols["ROUTING_CD_NM"].Width = level2;
            grdMain.Cols["ZONE_CD"].Width = level2 + 20;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width - 35;
            grdMain.Cols["HEAT"].Width = 110;
            grdMain.Cols["STEEL"].Width = level2 + 10;
            grdMain.Cols["STEEL_NM"].Width = level3 + 30;
            grdMain.Cols["ITEM"].Width = level2;
            grdMain.Cols["ITEM_SIZE"].Width = level2;
            grdMain.Cols["LENGTH"].Width = level2 + 30;
            grdMain.Cols["GOOD_YN"].Width = level3;
            grdMain.Cols["MPI_FAULT_CD"].Width = level3+100;
            grdMain.Cols["BAD_LENGTH"].Width = level2 + 30;
            grdMain.Cols["GUBUN"].Width = 0;

            #region 1. grdMain head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["CHECKER"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["MILL_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["PIECE_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["LINE_GP"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ROUTING_CD_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["MFG_DATE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TIME"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ROUTING_CD_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ZONE_CD"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["POC_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["HEAT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["STEEL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["STEEL_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ITEM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ITEM_SIZE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["LENGTH"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["GOOD_YN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["MPI_FAULT_CD"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            #endregion

            // 그리드 수정가능 및 컬럼 콤보박스 설정 
            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].AllowEditing = false;
            grdMain.Cols["MILL_NO"].AllowEditing = false;
            grdMain.Cols["PIECE_NO"].AllowEditing = false;
            grdMain.Cols["LINE_GP"].AllowEditing = false;
            grdMain.Cols["MFG_DATE"].AllowEditing = false;
            grdMain.Cols["WORK_TIME"].AllowEditing = false;
            grdMain.Cols["WORK_TYPE_NM"].AllowEditing = false;
            grdMain.Cols["WORK_TEAM_NM"].AllowEditing = false;
            grdMain.Cols["ROUTING_CD_NM"].AllowEditing = false;
            grdMain.Cols["ZONE_CD"].AllowEditing = false;
            grdMain.Cols["POC_NO"].AllowEditing = false;
            grdMain.Cols["HEAT"].AllowEditing = false;
            grdMain.Cols["STEEL"].AllowEditing = false;
            grdMain.Cols["STEEL_NM"].AllowEditing = false;
            grdMain.Cols["ITEM"].AllowEditing = false;
            grdMain.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["BAD_LENGTH"].AllowEditing = true;
            grdMain.Cols["GUBUN"].AllowEditing = false;
            
            SetComboinGrd();

            grdMain.Cols["GOOD_YN"].DataMap = ngTest_Value;
            grdMain.Cols["GOOD_YN"].TextAlign = TextAlignEnum.CenterCenter;

            grdMain.Cols["MPI_FAULT_CD"].DataMap = fault_cd; //ngTest_Value
            grdMain.Cols["MPI_FAULT_CD"].TextAlign = TextAlignEnum.LeftCenter;

            grdMain.ExtendLastCol = true;
        }
        #endregion Init control,grid 설정

        #region Combo Box 설정
        private bool SetComboinGrd()
        {
            try
            {
                ngTest_Value = new ListDictionary();
                DataTable dt1 = cd.Find_CD_GOOD_NG("GOOD_NG");
                foreach (DataRow row in dt1.Rows)
                {
                    ngTest_Value.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                fault_cd = new ListDictionary();
                DataTable dt2 = cd.Find_CD("FAULT_CD");

                foreach (DataRow row in dt2.Rows)
                {
                    fault_cd.Add(row["CD_ID"].ToString(), row["CD_ID"].ToString() + "  "+ row["CD_NM"].ToString());
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private void SetComboBox1()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
        }
        private void SetComboBox2()
        {
            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);
        }
        private bool SetComboBox3()
        {
            try
            {
                cboGubun_GP.Items.Clear();
                List<string> list3 = new List<string>();
                list3.Add("등록");
                list3.Add("미등록");

                foreach (var item in list3)
                {
                    cboGubun_GP.Items.Add(item);
                }
                //첫번째 아이템 선택
                cboGubun_GP.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private void SetComboBox4()
        {
            cd.SetCombo(cboTEAM, "WORK_TEAM", "", true);
        }
        #endregion Combo Box 설정

        #region 이벤트 설정

        #region 버튼 관련 이벤트 설정

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            SetDataBinding();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, grdMain);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion 버튼 관련 이벤트 설정

        #region 컨트롤 관련 이벤트 설정
        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp_id = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = line_gp_id;
        }

        private void cboGubun_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGubun_GP.SelectedIndex == 1)
            {
                gp = "A";
            }
            else 
            {
                gp = "B";
            }
        }
        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_type_id = ((DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }

        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_team_id = ((DictionaryList)cboTEAM.SelectedItem).fnValue;
        }

        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_dt_str = clsUtil.Utl.GetDateFormat(3, start_dt.Value);
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            end_dt_str = clsUtil.Utl.GetDateFormat(3, end_dt.Value);
        }

        private void txtSteel_id_TextChanged(object sender, EventArgs e)
        {
            steel_id = gangjong_id_tb.Text;
        }

        private void txtPoc_TextChanged(object sender, EventArgs e)
        {
            poc = txtPoc.Text;
        }

        private void txtHeat_TextChanged(object sender, EventArgs e)
        {
            heat = txtHeat.Text;
        }
        #endregion 컨트롤 관련 이벤트 설정

        #region 강종 관련이벤트 설정
        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjung_id = gangjong_id_tb.Text;
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        //강종ID(LostFocus)
        private void gangjong_id_tb_LostFocus(object sender, EventArgs e)
        {
            if (gangjong_id_tb.Text == "")
            {
                gangjong_Nm_tb.Text = "";
                gangjung_id = "";
            }
            else
            {
                gangjong_Nm_tb.Text = cd.Find_Steel_NM_By_ID(gangjong_id_tb.Text);

                if (gangjong_Nm_tb.Text.Length == 0)
                {
                    if (MessageBox.Show(" 해당 강종에 따른 강종명을 찾을 수 없습니다.", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        gangjong_Nm_tb.Text = "";
                        gangjong_id_tb.Focus();
                        return;
                    }
                }
                else
                    gangjung_id = gangjong_id_tb.Text;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SearchSteelNm popup = new SearchSteelNm();
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }
        #endregion 강종 관련이벤트 설정

        #region 사용자 정의 이벤트 설정

        //사용자이벤트 생성
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
        }

        #endregion 사용자 정의 이벤트 설정

        #region 그리드 관련 이벤트 설정

        private void grdMain_CellChanged(object sender, RowColEventArgs e)
        {
            workteam_nm = cboLine_GP.SelectedItem as string;
            workteam_id = cd.Find_CD_ID("LINE_GP", workteam_nm);
        }
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedRow <= 0)
            {
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = editedGrd.GetData(editedRow, editedCol).ToString();
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            int gubun_index = editedGrd.Cols["GUBUN"].Index;

            if (editedRow <= 0)
            {
                return;
            }

            // No,구분은 수정 불가
            if (editedCol == 0 || editedCol == gubun_index)
            {
                editedGrd.SetData(editedRow, editedCol, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue.ToString() == editedGrd.GetData(editedRow, editedCol).ToString().Trim())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (editedGrd.GetData(editedRow, gubun_index).ToString() != "추가")
            {
                if (editedCol == 0)
                {
                    editedGrd.SetData(editedRow, editedCol, strBefValue);
                    return;
                }
                
                // 선택이 변했을때는 그대로 리턴...
                if (editedCol == editedGrd.Cols["CHECKER"].Index)
                {
                    return;
                }

                if (editedCol == editedGrd.Cols["MPI_FAULT_CD"].Index)
                {
                    if (!string.IsNullOrEmpty(editedGrd.GetData(editedRow, "MPI_FAULT_CD").ToString()))
                    {
                        editedGrd.SetData(editedRow, "GOOD_YN", "NG");
                    }
                }

                if (string.IsNullOrEmpty(editedGrd.GetData(editedRow, "BAD_LENGTH").ToString()))
                {
                    string set_value = "0.0";
                    editedGrd.SetData(editedRow, "BAD_LENGTH", set_value);
                }

                // 저장시 UPDATE로 처리하기 위해 flag set
                editedGrd.SetData(editedRow, gubun_index, "수정");
                editedGrd.SetData(editedRow, 0, "수정");

                // Update 배경색 지정
                editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            }
        }

        #endregion 그리드 관련 이벤트 설정

        #endregion 이벤트 설정

        #region 저장 설정

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 항목체크 

            #region 실시간 트레킹에서 작업중인지 확인하기위한 변수
            checkdt = new DataTable();
            checkdt = cd.GetRTDATA(line_gp_id);
            string checksql = string.Empty;
            string _mill_no = string.Empty;
            string _piece_no = string.Empty;
            string _poc_no = string.Empty;
            #endregion

            string check_value1 = string.Empty;
            string check_Cols_NM1 = string.Empty;
            string check_field_NM1 = string.Empty;
            string check_table_NM1 = string.Empty;

            string check_value2 = string.Empty;
            string check_Cols_NM2 = string.Empty;
            string check_field_NM2 = string.Empty;
            string check_table_NM2 = string.Empty;

            string check_keyColNM = string.Empty;
            string check_keyValue = string.Empty;

            string check_NM = string.Empty;

            string gubun_value = string.Empty;
            string show_msg = string.Empty;
            int checkrow = 0;

            bool isChange = false;
            bool isChecked = false;
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "L_NO").ToString();

                isChecked = (grdMain.GetData(checkrow, "CHECKER").ToString() == "True") ? true : false;

                if (!isChecked)
                {
                    continue;
                }

                if (gubun_value == "수정")
                {
                    if (grdMain.GetData(checkrow, "GOOD_YN").ToString() == "NG" && grdMain.GetData(checkrow, "MPI_FAULT_CD").ToString() == "")
                    {
                        show_msg = string.Format("{0}를 입력하세요.", "결함코드");
                        MessageBox.Show(show_msg);
                        isChange = false;
                        return;
                    }

                    // 실시간 트랙킹 테이블에서 해당 압연번호, PIECE_NO를 키로 검색된 항목일 경우 수정을 중단한다.
                    _mill_no = grdMain.GetData(checkrow, "MILL_NO").ToString();
                    _piece_no = grdMain.GetData(checkrow, "PIECE_NO").ToString();
                    _poc_no = grdMain.GetData(checkrow, "POC_NO").ToString();
                    checksql = string.Format("MILL_NO = '{0}' AND PIECE_NO = '{1}' AND  POC_NO = '{2}'", _mill_no, _piece_no, _poc_no);
                    DataRow[] rows = checkdt.Select(checksql);

                    if (rows.Length > 0)
                    {
                        show_msg = string.Format("작업중입니다. (수정 불가)");
                        MessageBox.Show(show_msg);
                        return;
                    }
                    isChange = true;
                }
            }

            if (isChange)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            #endregion 항목체크     

            string strQry = string.Empty;
            string strMsg = string.Empty;
            string wTime = "";
            DateTime time = new DateTime();

            int row = 0;
            int UpCnt = 0;
            List<string> delSublist = null;

            DataTable dt = null;

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

                #region grdMain1
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    gubun_value = grdMain.GetData(row, "L_NO").ToString();
                    isChecked = (grdMain.GetData(row, "CHECKER").ToString() == "True") ? true : false;

                    // Update 처리
                    if (isChecked & gubun_value == "수정")
                    {
                        time = Convert.ToDateTime(grdMain.GetData(row, "WORK_TIME").ToString());
                        wTime = clsUtil.Utl.GetDateFormat(8, time);

                        string sql1 = string.Format(" MERGE INTO TB_CR_PIECE_WR  A ");
                        sql1 += string.Format("USING ( SELECT '{0}'    AS MILL_NO ", grdMain.GetData(row, "MILL_NO").ToString());
                        sql1 += string.Format("              ,'{0}'   AS PIECE_NO ", grdMain.GetData(row, "PIECE_NO").ToString());
                        sql1 += string.Format("              ,'{0}'   AS LINE_GP ", line_gp_id);
                        sql1 += string.Format("              ,'W2'          AS ROUTING_CD ");
                        sql1 += string.Format("              ,'{0}'             AS REWORK_SEQ ", olddt.Rows[row - 1]["REWORK_SEQ"].ToString());//,0             AS REWORK_SEQ");
                        sql1 += string.Format("        FROM  DUAL ) B ");
                        sql1 += string.Format("ON    ( A.MILL_NO    = B.MILL_NO  AND ");
                        sql1 += string.Format("        A.PIECE_NO   = B.PIECE_NO AND ");
                        sql1 += string.Format("        A.LINE_GP    = B.LINE_GP  AND ");
                        sql1 += string.Format("        A.ROUTING_CD = B.ROUTING_CD AND ");
                        sql1 += string.Format("        A.REWORK_SEQ = B.REWORK_SEQ ) ");
                        sql1 += string.Format("WHEN  MATCHED THEN ");
                        sql1 += string.Format("      UPDATE  SET ");
                        sql1 += string.Format("               GOOD_YN       = '{0}' ", grdMain.GetData(row, "GOOD_YN").ToString());
                        sql1 += string.Format("              ,MPI_FAULT_CD  = '{0}' ", grdMain.GetData(row, "MPI_FAULT_CD").ToString());
                        sql1 += string.Format("              ,BAD_LENGTH    = '{0}' ", grdMain.GetData(row, "BAD_LENGTH").ToString());
                        sql1 += string.Format("              ,IF_YN         = DECODE( IF_YN, 'Y', DECODE(GOOD_YN, 'OK', 'D', 'U')) ");// INTERFACE 가 Y 일때만 GOOD_YN가 OK이면, D를 아니면 U 를 한다.
                        sql1 += string.Format("              ,MODIFIER      = '{0}' ", ck.UserID);
                        sql1 += string.Format("              ,MOD_DDTT      = SYSDATE ");
                        sql1 += string.Format("WHEN NOT MATCHED THEN  ");
                        sql1 += string.Format("      INSERT (MILL_NO ");
                        sql1 += string.Format("             ,PIECE_NO ");
                        sql1 += string.Format("             ,LINE_GP ");
                        sql1 += string.Format("             ,ROUTING_CD ");
                        sql1 += string.Format("             ,REWORK_SEQ ");
                        sql1 += string.Format("             ,POC_NO ");
                        sql1 += string.Format("             ,HEAT ");
                        sql1 += string.Format("             ,STEEL ");
                        sql1 += string.Format("             ,ITEM ");
                        sql1 += string.Format("             ,ITEM_SIZE ");
                        sql1 += string.Format("             ,BAD_LENGTH ");
                        sql1 += string.Format("             ,LENGTH ");
                        sql1 += string.Format("             ,ENTRY_DDTT ");
                        sql1 += string.Format("             ,EXIT_DDTT ");
                        sql1 += string.Format("             ,GOOD_YN ");
                        sql1 += string.Format("             ,MPI_FAULT_CD ");
                        sql1 += string.Format("             ,MFG_DATE ");
                        sql1 += string.Format("             ,WORK_TYPE ");
                        sql1 += string.Format("             ,WORK_TEAM ");
                        sql1 += string.Format("             ,REGISTER ");
                        sql1 += string.Format("             ,REG_DDTT ");
                        sql1 += string.Format("             ) VALUES ( ");
                        sql1 += string.Format("              B.MILL_NO ");
                        sql1 += string.Format("             ,B.PIECE_NO ");
                        sql1 += string.Format("             ,B.LINE_GP ");
                        sql1 += string.Format("             ,B.ROUTING_CD ");
                        sql1 += string.Format("             ,B.REWORK_SEQ ");
                        sql1 += string.Format("             ,'{0}'   ", grdMain.GetData(row, "POC_NO").ToString());
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "HEAT").ToString());
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "STEEL").ToString());
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "ITEM").ToString());
                        sql1 += string.Format("             ,'{0}'", grdMain.GetData(row, "ITEM_SIZE").ToString());
                        sql1 += string.Format("             ,'{0}'", grdMain.GetData(row, "BAD_LENGTH").ToString());
                        sql1 += string.Format("             ,'{0}'", grdMain.GetData(row, "LENGTH").ToString());
                        sql1 += string.Format("             ,SYSDATE ");
                        sql1 += string.Format("             ,SYSDATE ");
                        sql1 += string.Format("             ,'{0}'", grdMain.GetData(row, "GOOD_YN").ToString());   //GOOD_YN
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "MPI_FAULT_CD").ToString());
                        sql1 += string.Format("             ,FN_GET_MFG_DATE('') ");
                        sql1 += string.Format("             ,'{0}'  ", olddt.Rows[row - 1]["WORK_TYPE"].ToString());
                        sql1 += string.Format("             ,'{0}' ", olddt.Rows[row - 1]["WORK_TEAM"].ToString());
                        sql1 += string.Format("             ,'{0}' ", ck.UserID);
                        sql1 += string.Format("             ,SYSDATE ");
                        sql1 += string.Format("             ) ");
                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        UpCnt++;
                    }
                }
                #endregion grdMain1 

                //실행후 성공
                transaction.Commit();
                btnDisplay_Click(null, null);
                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴
                InitGrd_Main();

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);
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

        #endregion 저장 설정

        #region 조회 설정

        private void SetDataBinding()
        {
            string sql1 = string.Format(" SELECT TO_CHAR(ROWNUM) AS L_NO ");
            sql1 += string.Format("       ,Y.*        ");
            sql1 += string.Format("       FROM(        ");
            sql1 += string.Format("       SELECT        ");
            sql1 += string.Format("             'False' AS CHECKER  ");
            sql1 += string.Format("             ,MILL_NO  "); //--KEY  압연번들번호
            sql1 += string.Format("             ,PIECE_NO "); //--KEY  PIECE NO
            sql1 += string.Format("             ,LINE_GP "); //--KEY
            sql1 += string.Format("             ,ROUTING_CD "); //--KEY
            sql1 += string.Format("             ,REWORK_SEQ "); //--KEY
            sql1 += string.Format("             ,MFG_DATE  "); //--작업일자
            sql1 += string.Format("             ,WORK_TIME ");// --작업시각
            sql1 += string.Format("             ,WORK_TYPE ");
            sql1 += string.Format("             ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM  ");
            sql1 += string.Format("             ,WORK_TEAM ");
            sql1 += string.Format("             ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM  ");
            sql1 += string.Format("             ,ROUTING_CD_NM ");// --공정
            sql1 += string.Format("             ,ZONE_CD  ");
            sql1 += string.Format("             ,POC_NO  ");
            sql1 += string.Format("             ,HEAT  ");
            sql1 += string.Format("             ,STEEL ");
            sql1 += string.Format("             ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM   ");
            sql1 += string.Format("             ,ITEM ");
            sql1 += string.Format("             ,ITEM_SIZE  ");
            sql1 += string.Format("             ,LENGTH  ");
            sql1 += string.Format("             ,GOOD_YN  "); //--합부
            sql1 += string.Format("             ,MPI_FAULT_CD "); // --결함코드
            sql1 += string.Format("             ,NVL(BAD_LENGTH, 0) AS BAD_LENGTH "); // --불량길이
            sql1 += string.Format("             ,'' AS GUBUN  ");
            sql1 += string.Format("      FROM  (                   ");
            sql1 += string.Format("                     SELECT  *  ");
            sql1 += string.Format("                     FROM    (  ");
            sql1 += string.Format("                               SELECT  MILL_NO  ");
            sql1 += string.Format("                                      ,PIECE_NO  ");
            sql1 += string.Format("                                      ,LINE_GP  ");
            sql1 += string.Format("                                      ,ROUTING_CD ");
            sql1 += string.Format("                                      ,REWORK_SEQ  ");
            sql1 += string.Format("                                      ,TO_DATE(MFG_DATE, 'YYYY-MM-DD' ) AS MFG_DATE    ");
            sql1 += string.Format("                                      ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME  ");
            sql1 += string.Format("                                      ,WORK_TYPE   ");
            sql1 += string.Format("                                      ,NVL(WORK_TEAM, 'A') AS WORK_TEAM   ");
            sql1 += string.Format("                                      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'ROUTING_CD' AND CD_ID = A.ROUTING_CD) AS ROUTING_CD_NM ");
            sql1 += string.Format("                                      ,ZONE_CD   ");
            sql1 += string.Format("                                      ,POC_NO  ");
            sql1 += string.Format("                                      ,HEAT ");
            sql1 += string.Format("                                      ,STEEL  ");
            sql1 += string.Format("                                      ,ITEM ");
            sql1 += string.Format("                                      ,ITEM_SIZE   ");
            sql1 += string.Format("                                      ,LENGTH  ");
            sql1 += string.Format("                                      ,GOOD_YN  ");
            sql1 += string.Format("                                      ,MPI_FAULT_CD ");
            sql1 += string.Format("                                      ,BAD_LENGTH ");
            sql1 += string.Format("                                      ,REG_DDTT  ");
            sql1 += string.Format("                                FROM  TB_CR_PIECE_WR A  ");
            sql1 += string.Format("                                WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("                                AND   A.LINE_GP    = '{0}' ", line_gp_id);
            sql1 += string.Format("                                AND   A.ROUTING_CD IN ('F2','H2','K2')  "); //--NDT,MPI,GR
            sql1 += string.Format("                                AND   A.POC_NO      LIKE '{0}' || '%' ", poc);
            sql1 += string.Format("                                AND   A.HEAT      LIKE '%' || '{0}' || '%' ", heat);
            sql1 += string.Format("                                AND   A.STEEL      LIKE '{0}'|| '%'  ", gangjung_id);
            sql1 += string.Format("                                AND   A.WORK_TYPE  LIKE '{0}' || '%'  ", work_type_id);
            sql1 += string.Format("                                AND   A.WORK_TEAM  LIKE '{0}' || '%'  ", work_team_id);
            sql1 += string.Format("                                AND    '{0}' = 'A'", gp); //--미등록
            sql1 += string.Format("                                AND   A.GOOD_YN    = 'NG'  ");
            sql1 += string.Format("                                AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR  ");
            sql1 += string.Format("                                                         WHERE  MILL_NO    = A.MILL_NO   ");
            sql1 += string.Format("                                                         AND    PIECE_NO   = A.PIECE_NO   ");
            sql1 += string.Format("                                                         AND    LINE_GP    = A.LINE_GP    ");
            sql1 += string.Format("                                                         AND    ROUTING_CD = A.ROUTING_CD )   ");
            sql1 += string.Format("                                AND   NOT EXISTS (SELECT ROUTING_CD FROM TB_CR_PIECE_WR   ");
            sql1 += string.Format("                                                  WHERE  MILL_NO  = A.MILL_NO   ");
            sql1 += string.Format("                                                  AND    PIECE_NO = A.PIECE_NO  ");
            sql1 += string.Format("                                                  AND    LINE_GP  = A.LINE_GP   ");
            sql1 += string.Format("                                                  AND    ROUTING_CD = 'W2'   ");
            sql1 += string.Format("                                                  AND    REWORK_SEQ = A.REWORK_SEQ )  ");
            sql1 += string.Format("                              ) B    ");
            sql1 += string.Format("                     WHERE  ROUTING_CD  = ( SELECT MAX(ROUTING_CD) FROM TB_CR_PIECE_WR  ");
            sql1 += string.Format("                                       WHERE  MILL_NO    = B.MILL_NO  ");
            sql1 += string.Format("                                       AND    PIECE_NO   = B.PIECE_NO  ");
            sql1 += string.Format("                                       AND    LINE_GP    = B.LINE_GP   ");
            sql1 += string.Format("                                       AND    REWORK_SEQ = B.REWORK_SEQ ");
            sql1 += string.Format("                                       AND    ROUTING_CD IN ('F2','H2','K2')  ");
            sql1 += string.Format("                                       AND    GOOD_YN    = 'NG' ) ");
            sql1 += string.Format("                UNION   ");
            sql1 += string.Format("                     SELECT  MILL_NO ");
            sql1 += string.Format("                            ,PIECE_NO ");
            sql1 += string.Format("                            ,LINE_GP  ");
            sql1 += string.Format("                            ,ROUTING_CD  ");
            sql1 += string.Format("                            ,REWORK_SEQ  ");
            sql1 += string.Format("                            ,TO_DATE(MFG_DATE, 'YYYY-MM-DD' ) AS MFG_DATE    ");
            sql1 += string.Format("                            ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME   ");
            sql1 += string.Format("                            ,WORK_TYPE   ");
            sql1 += string.Format("                            ,NVL(WORK_TEAM, 'A') AS WORK_TEAM   ");
            sql1 += string.Format("                            ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'ROUTING_CD' AND CD_ID = A.ROUTING_CD) AS ROUTING_CD_NM  ");
            sql1 += string.Format("                            ,ZONE_CD   ");
            sql1 += string.Format("                            ,POC_NO  ");
            sql1 += string.Format("                            ,HEAT  ");
            sql1 += string.Format("                            ,STEEL  ");
            sql1 += string.Format("                            ,ITEM  ");
            sql1 += string.Format("                            ,ITEM_SIZE   ");
            sql1 += string.Format("                            ,LENGTH      ");
            sql1 += string.Format("                            ,GOOD_YN     ");
            sql1 += string.Format("                            ,MPI_FAULT_CD  ");
            sql1 += string.Format("                            ,BAD_LENGTH ");
            sql1 += string.Format("                            ,REG_DDTT    ");
            sql1 += string.Format("                     FROM  TB_CR_PIECE_WR A  ");
            sql1 += string.Format("                     WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("                     AND   A.LINE_GP    = '{0}' ", line_gp_id);
            sql1 += string.Format("                     AND   A.ROUTING_CD =  'W2'   "); //--격외
            sql1 += string.Format("                     AND   A.POC_NO       LIKE  '%' || '{0}' || '%' ", poc);
            sql1 += string.Format("                     AND   A.HEAT         LIKE  '%' || '{0}' || '%' ", heat);
            sql1 += string.Format("                     AND   A.STEEL        LIKE  '%' || '{0}' || '%'  ", gangjung_id);
            sql1 += string.Format("                     AND   A.WORK_TYPE    LIKE         '{0}' || '%'  ", work_type_id);
            sql1 += string.Format("                     AND   A.WORK_TEAM    LIKE         '{0}' || '%'  ", work_team_id);
            sql1 += string.Format("                     AND   '{0}'        = 'B' ",gp); //--등록
            sql1 += string.Format("                     AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR   ");
            sql1 += string.Format("                                              WHERE  MILL_NO    = A.MILL_NO  ");
            sql1 += string.Format("                                              AND    PIECE_NO   = A.PIECE_NO  ");
            sql1 += string.Format("                                              AND    LINE_GP    = A.LINE_GP  ");
            sql1 += string.Format("                                              AND    ROUTING_CD = A.ROUTING_CD )  ");
            sql1 += string.Format("                ) X    ");
            sql1 += string.Format("      ORDER BY MFG_DATE DESC, WORK_TIME DESC, MILL_NO ");
            sql1 += string.Format("      ) Y    ");

            olddt = cd.FindDataTable(sql1);
            moddt = olddt.Copy();
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            grdMain.AutoSizeCols(grdMain.Cols["BAD_LENGTH"].Index, grdMain.Cols["LENGTH"].Index, 1);
            grdMain.AutoSizeCols(grdMain.Cols["L_NO"].Index, grdMain.Cols["GOOD_YN"].Index, 1);
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;
        }

        #endregion 조회 설정
    }
}

