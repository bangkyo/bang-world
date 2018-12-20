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
using static SystemControlClassLibrary.Order.CrtInOrdCre;


namespace SystemControlClassLibrary.information
{
    public partial class NDTRsltSend : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable checkdt;

        ListDictionary ngTest_Value = null;

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
        private  string line_gp_id = "";
        private  string work_type_id = "";
        private string gp = "";
        private string poc = "";
        private  bool detector = true;
        private string work_TEAM_id = "";
        private string start_dt_str = "";
        private string end_dt_str = "";
        private string gangjung_id = "";
        private DateTimePicker dtpTime;
        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public NDTRsltSend(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void NDTRsltSendInq_Load(object sender, EventArgs e)
        {
            InitControl();

            EventCreate();      //사용자정의 이벤트

            btnDisplay_Click(null, null);
        }
        #endregion 로딩, 생성자 설정

        #region Init Control 설정
        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);

            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitPanel(panel1);

            //InitLabel
            clsStyle.Style.InitLabel(lblHeat);
            clsStyle.Style.InitLabel(lblSteel);
            clsStyle.Style.InitLabel(lblLine);
            clsStyle.Style.InitLabel(lblGubun);
            clsStyle.Style.InitLabel(lblPoc);
            clsStyle.Style.InitLabel(lblWorkType);
            clsStyle.Style.InitLabel(lblMfgDate);
            clsStyle.Style.InitLabel(lblTEAM);

            //InitButton
            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnSave);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnClose);

            //InitCombo
            clsStyle.Style.InitCombo(cbo_Work_Type, StringAlignment.Near);
            clsStyle.Style.InitCombo(cboLine_GP, StringAlignment.Near);
            clsStyle.Style.InitCombo(cboGubun_GP, StringAlignment.Near);
            clsStyle.Style.InitCombo(cboTEAM, StringAlignment.Near);

            //InitTextBox
            clsStyle.Style.InitTextBox(txtPoc);
            clsStyle.Style.InitTextBox(gangjong_id_tb);
            clsStyle.Style.InitTextBox(txtHeat);
            clsStyle.Style.InitTextBox(gangjong_Nm_tb);

            //DateTime
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            //ImeMode
            txtHeat.ImeMode = ImeMode.Disable;
            txtPoc.ImeMode = ImeMode.Disable;

            //SetComboBox
            SetComboBox1();
            SetComboBox2();
            SetComboBox3();
            SetComboBox4();

            //InitGrd
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
        #endregion Init Control 설정

        #region Init Grid 설정
        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.Rows[0].Height = cs.Head_Height;

            #region Grid Column Head Caption 색깔 설정
            //var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["GOOD_YN"].Index, 0, grdMain.Cols["MFG_DATE"].Index);
            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["GOOD_YN"].Index, 0, grdMain.Cols["WORK_TIME"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];
            #endregion Grid Column Head Caption 색깔 설정

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;


            grdMain.Cols["MAT_GOOD_NG"].Caption = "  MAT\n합부";   
            grdMain.Cols["MLFT_GOOD_NG"].Caption = "  MLFT\n합부";
            grdMain.Cols["UT_GOOD_NG"].Caption = "  UT\n합부";
            grdMain.Cols["POC_END_YN"].Caption = "  POC\n종료여부";
            grdMain.Cols["LENGTH"].Caption = "  길이\n(m)";       //width 90

            grdMain.Cols["WORK_TYPE"].Visible = false;
            grdMain.Cols["WORK_TEAM"].Visible = false;
            grdMain.Cols["GUBUN"].Visible = false;

            #region Width 설정
            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["CHECKER"].Width = cs.Checker_Width;           //width 50
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width + 20;      //width 130
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;    //width 60
            grdMain.Cols["GOOD_YN"].Width = cs.Good_NG_S_Width;           //width 80
            grdMain.Cols["MAT_GOOD_NG"].Width = cs.Good_NG_S_Width;   //width 90
            grdMain.Cols["MLFT_GOOD_NG"].Width = cs.Good_NG_S_Width; //width 100
            grdMain.Cols["UT_GOOD_NG"].Width = cs.Good_NG_S_Width;     //width 80
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width +20;         //width 140
            grdMain.Cols["WORK_TIME"].Width = cs.TIME_8_Width +10;          //width 100
            grdMain.Cols["WORK_TYPE"].Width = 0;                        //width 0
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width; //width 50
            grdMain.Cols["WORK_TEAM"].Width = 0;                        //width 0
            grdMain.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width; //width 50
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width - 10;        //width 110
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width - 20;            //width 80
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width + 20;          //width 60
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 10;    //width 100
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width + 15;            //width 60
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 15;  //width 60
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width + 10;        //width 90
            grdMain.Cols["POC_END_YN"].Width = cs.POC_END_YN_Width;     //width 100
            grdMain.Cols["GUBUN"].Width = 0;                            //width 0
            #endregion Width 설정

            #region grdMain head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["CHECKER"].TextAlign = cs.SEL_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["PIECE_NO"].TextAlign = cs.PIECE_NO_TextAlign;
            grdMain.Cols["GOOD_YN"].TextAlign = cs.GOOD_YN_TextAlign;
            grdMain.Cols["MAT_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["MLFT_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["UT_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["WORK_TEAM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TEAM_NM_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["POC_END_YN"].TextAlign = cs.POC_END_YN_TextAlign;

            #endregion

            #region 그리드 수정가능 및 컬럼 콤보박스 설정 
            grdMain.AllowEditing = true;

            grdMain.Cols["MILL_NO"].AllowEditing = false;
            grdMain.Cols["PIECE_NO"].AllowEditing = false;
            grdMain.Cols["WORK_TYPE"].AllowEditing    = false;
            grdMain.Cols["WORK_TYPE_NM"].AllowEditing = false;
            grdMain.Cols["WORK_TEAM"].AllowEditing    = false;
            grdMain.Cols["WORK_TEAM_NM"].AllowEditing = false;
            grdMain.Cols["POC_NO"].AllowEditing       = false;
            grdMain.Cols["HEAT"].AllowEditing         = false;
            grdMain.Cols["STEEL"].AllowEditing        = false;
            grdMain.Cols["STEEL_NM"].AllowEditing     = false;
            grdMain.Cols["ITEM"].AllowEditing         = false;
            grdMain.Cols["ITEM_SIZE"].AllowEditing    = false;
            grdMain.Cols["LENGTH"].AllowEditing       = false;
            grdMain.Cols["POC_END_YN"].AllowEditing   = false;
            grdMain.Cols["GUBUN"].AllowEditing        = false;
            #endregion 그리드 수정가능 및 컬럼 콤보박스 설정 

            #region 그리드 내의 ComboBox 설정
            ngTest_Value = new ListDictionary();
            DataTable dt1 = cd.Find_CD_GOOD_NG("GOOD_NG");

            foreach (DataRow row in dt1.Rows)
            {
                ngTest_Value.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
            }

            grdMain.Cols["GOOD_YN"].DataMap   = ngTest_Value;
            grdMain.Cols["GOOD_YN"].TextAlign = TextAlignEnum.CenterCenter;

            grdMain.Cols["MAT_GOOD_NG"].DataMap   = ngTest_Value;
            grdMain.Cols["MAT_GOOD_NG"].TextAlign = TextAlignEnum.CenterCenter;

            grdMain.Cols["MLFT_GOOD_NG"].DataMap = ngTest_Value;
            grdMain.Cols["MLFT_GOOD_NG"].TextAlign = TextAlignEnum.CenterCenter;

            grdMain.Cols["UT_GOOD_NG"].DataMap   = ngTest_Value;
            grdMain.Cols["UT_GOOD_NG"].TextAlign = TextAlignEnum.CenterCenter;
            #endregion 그리드 내의 ComboBox 설정

            dtpTime = new DateTimePicker();
            cs.InitDateTimePicker(dtpTime);


            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "HH:mm:ss";
            dtpTime.Value = DateTime.Now;
            dtpTime.ShowUpDown = true;

            grdMain.Cols["WORK_TIME"].Editor = dtpTime;
        }
        #endregion Init Grid 설정
        
        #region ComboBox 설정
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
                //list3.Add("전체");
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
        #endregion ComboBox 설정

        #region 저장 설정
        private void btnSave_Click(object sender, EventArgs e)
        {
            string strQry = string.Empty;
            string strMsg = string.Empty;
            string wTime = "";

            #region 삭제항목체크

            string mill_no = string.Empty;
            string piece_no = string.Empty;
            bool IsRUN = false;

            #region 실시간 트레킹에서 작업중인지 확인하기위한 변수
            checkdt = new DataTable();
            checkdt = cd.GetRTDATA(line_gp_id);
            string checksql = string.Empty;
            string _mill_no = string.Empty;
            string _piece_no = string.Empty;
            string _poc_no = string.Empty;
            #endregion

            string check_value1 = null;
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
            bool isChecked = false;
            bool isChange = false;
            
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "GUBUN").ToString();

                isChecked = (grdMain.GetData(checkrow, "CHECKER").ToString() == "True") ? true : false;

                if (!isChecked)
                {
                    continue;
                }

                if (gubun_value == "수정")
                {
                    check_field_NM1 = "MFG_DATE";
                    check_table_NM1 = "TB_CR_PIECE_WR";
                    check_value1 = grdMain.GetData(checkrow, check_field_NM1).ToString();
                    check_Cols_NM1 = grdMain.Cols[check_field_NM1].Caption;

                    if (string.IsNullOrEmpty(check_value1))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    check_field_NM2 = "WORK_TIME";
                    check_table_NM2 = "TB_CR_PIECE_WR";
                    check_value2 = grdMain.GetData(checkrow, check_field_NM2).ToString();
                    check_Cols_NM2 = grdMain.Cols[check_field_NM2].Caption;

                    if (string.IsNullOrEmpty(check_value2))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    int left_num = Int32.Parse(vf.Left(check_value2, 2));
                    int mid_num = Int32.Parse(vf.Mid(check_value2, 4, 2));
                    int right_num = Int32.Parse(vf.Right(check_value2, 2));

                    if (left_num > 23)
                    {
                        show_msg = string.Format("{0}의 시가 24를 넘었습니다. 다시 입력하세요", check_Cols_NM2);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (mid_num > 59)
                    {
                        show_msg = string.Format("{0}의 분이 60을 넘었습니다. 다시 입력하세요", check_Cols_NM2);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    if (right_num > 59)
                    {
                        show_msg = string.Format("{0}의 초가 60을 넘었습니다. 다시 입력하세요", check_Cols_NM2);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    //if (left_num == 24 && mid_num >= 0 && right_num >= 0)
                    //{
                    //    show_msg = string.Format("{0}의 시간이 정확하지 않습니다(24시의 경우 00시로 변경). 다시 입력하세요", check_Cols_NM2);
                    //    MessageBox.Show(show_msg);
                    //    return;
                    //}

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
            #endregion 삭제항목체크     

            int row = 0;
            int UpCnt = 0;
            string sql1 = string.Empty;
            DateTime time = new DateTime();

            string strLog = string.Empty;
            var itemList = new List<DictionaryList>();
            var logList = new List<LogDataList>();
            

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
                    gubun_value = grdMain.GetData(row, "GUBUN").ToString();

                    isChecked = (grdMain.GetData(row, "CHECKER").ToString() == "True");

                    // Update 처리
                    if (isChecked & gubun_value == "수정")
                    {
                        time = Convert.ToDateTime(grdMain.GetData(row, "WORK_TIME").ToString());
                        wTime = clsUtil.Utl.GetDateFormat(8, time);

                        sql1  = string.Format(" MERGE INTO TB_CR_PIECE_WR  A ");
                        sql1 += string.Format(" USING ( SELECT  '{0}'    AS MILL_NO ", grdMain.GetData(row, "MILL_NO").ToString());
                        sql1 += string.Format("               , '{0}'   AS PIECE_NO ", grdMain.GetData(row, "PIECE_NO").ToString());
                        sql1 += string.Format("               , '{0}'   AS LINE_GP ", line_gp_id);
                        sql1 += string.Format("               ,'F2'          AS ROUTING_CD ");
                        sql1 += string.Format("               ,'{0}'             AS REWORK_SEQ ", olddt.Rows[row - 1]["REWORK_SEQ"].ToString());
                        sql1 += string.Format("         FROM  DUAL ) B ");
                        sql1 += string.Format(" ON    ( A.MILL_NO    = B.MILL_NO  AND ");
                        sql1 += string.Format("         A.PIECE_NO   = B.PIECE_NO AND ");
                        sql1 += string.Format("         A.LINE_GP    = B.LINE_GP  AND ");
                        sql1 += string.Format("         A.ROUTING_CD = B.ROUTING_CD AND ");
                        sql1 += string.Format("         A.REWORK_SEQ = B.REWORK_SEQ )   ");
                        sql1 += string.Format(" WHEN  MATCHED THEN   ");
                        sql1 += string.Format("       UPDATE  SET  ");
                        sql1 += string.Format("               GOOD_YN       = '{0}'  ", grdMain.GetData(row, "GOOD_YN").ToString());  //변경되는값 이벤트로 받을것
                        sql1 += string.Format("               ,MAT_GOOD_NG  = '{0}' ", grdMain.GetData(row, "MAT_GOOD_NG").ToString());
                        sql1 += string.Format("               ,MLFT_GOOD_NG  = '{0}' ", grdMain.GetData(row, "MLFT_GOOD_NG").ToString());
                        sql1 += string.Format("               ,UT_GOOD_NG    = '{0}' ", grdMain.GetData(row, "UT_GOOD_NG").ToString());
                        sql1 += string.Format("               ,MFG_DATE    = '{0}' ", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd")); //vf.Format(grdMain.GetData(row, "MFG_DATE"),"yyyyMMdd"))
                        sql1 += string.Format("               ,EXIT_DDTT    =  TO_DATE('{0}' || '{1}','YYYYMMDDHH24MISS' )", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"), wTime); //vf.Format(vf.CDate(grdMain.GetData(row, "WORK_TIME").ToString()), "yyyyMMdd"))
                        sql1 += string.Format("               ,MODIFIER      = '{0}'  ", ck.UserID);
                        sql1 += string.Format("               ,MOD_DDTT      = SYSDATE  ");
                        sql1 += string.Format(" WHEN NOT MATCHED THEN  ");
                        sql1 += string.Format("       INSERT (MILL_NO  ");
                        sql1 += string.Format("              ,PIECE_NO  ");
                        sql1 += string.Format("              ,LINE_GP  ");
                        sql1 += string.Format("              ,ROUTING_CD  ");
                        sql1 += string.Format("              ,REWORK_SEQ  ");
                        sql1 += string.Format("              ,POC_NO  ");
                        sql1 += string.Format("              ,HEAT  ");
                        sql1 += string.Format("              ,STEEL ");
                        sql1 += string.Format("              ,ITEM  ");
                        sql1 += string.Format("              ,ITEM_SIZE ");
                        sql1 += string.Format("              ,LENGTH  ");
                        sql1 += string.Format("              ,ENTRY_DDTT ");
                        sql1 += string.Format("              ,EXIT_DDTT ");
                        sql1 += string.Format("              ,GOOD_YN  ");     // 합부
                        sql1 += string.Format("              ,MAT_GOOD_NG  "); // MAT합부
                        sql1 += string.Format("              ,MLFT_GOOD_NG "); // MLFT합부
                        sql1 += string.Format("              ,UT_GOOD_NG ");   // UT합부
                        sql1 += string.Format("              ,MFG_DATE   ");   
                        sql1 += string.Format("              ,WORK_TYPE  ");
                        sql1 += string.Format("              ,WORK_TEAM ");
                        sql1 += string.Format("              ,REGISTER  ");
                        sql1 += string.Format("              ,REG_DDTT  ");
                        sql1 += string.Format("              ) VALUES (  ");
                        sql1 += string.Format("               B.MILL_NO  ");
                        sql1 += string.Format("              ,B.PIECE_NO ");
                        sql1 += string.Format("              ,B.LINE_GP  ");
                        sql1 += string.Format("              ,B.ROUTING_CD ");
                        sql1 += string.Format("              ,B.REWORK_SEQ");
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "POC_NO").ToString());
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "HEAT").ToString());
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "STEEL").ToString());
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "ITEM").ToString());
                        sql1 += string.Format("              , '{0}'" , grdMain.GetData(row, "ITEM_SIZE").ToString());
                        sql1 += string.Format("              ,  {0}" ,  vf.CDbl(grdMain.GetData(row, "LENGTH").ToString()));
                        sql1 += string.Format("              , TO_DATE('{0}' ||  '{1}','YYYYMMDDHH24MISS'  )", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"), wTime);
                        sql1 += string.Format("              , TO_DATE('{0}' ||  '{1}','YYYYMMDDHH24MISS'  )", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"), wTime);
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "GOOD_YN").ToString()); //olddt.Rows[row - 1]["GOOD_YN"].ToString()); //OLDDT 에서 변경전 값 대입  그리고 파라메터 변경할것
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "MAT_GOOD_NG").ToString()); //olddt.Rows[row - 1]["MAT_GOOD_NG"].ToString());
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "MLFT_GOOD_NG").ToString()); //olddt.Rows[row - 1]["MLFT_GOOD_NG"].ToString());
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "UT_GOOD_NG").ToString()); //olddt.Rows[row - 1]["UT_GOOD_NG"].ToString());
                        sql1 += string.Format("              , '{0}' ", vf.Format(olddt.Rows[row - 1]["MFG_DATE"], "yyyyMMdd"));//vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"));
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "WORK_TYPE").ToString()); //work_type_id
                        sql1 += string.Format("              , '{0}' ", grdMain.GetData(row, "WORK_TEAM").ToString());
                        sql1 += string.Format("              , '{0}' ", ck.UserID);
                        sql1 += string.Format("              ,SYSDATE  ");
                        sql1 += string.Format("              )        ");
                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        itemList.Add(new DictionaryList("MILL_NO", grdMain.GetData(row, "MILL_NO").ToString()));
                        itemList.Add(new DictionaryList("PIECE_NO", grdMain.GetData(row, "PIECE_NO").ToString()));
                        itemList.Add(new DictionaryList("LINE_GP", line_gp_id));
                        itemList.Add(new DictionaryList("ROUTING_CD", "F2"));
                        itemList.Add(new DictionaryList("REWORK_SEQ", olddt.Rows[row - 1]["REWORK_SEQ"].ToString()));

                        itemList.Add(new DictionaryList("GOOD_YN", grdMain.GetData(row, "GOOD_YN").ToString()));
                        itemList.Add(new DictionaryList("MAT_GOOD_NG", grdMain.GetData(row, "MAT_GOOD_NG").ToString()));
                        itemList.Add(new DictionaryList("MLFT_GOOD_NG", grdMain.GetData(row, "MLFT_GOOD_NG").ToString()));
                        itemList.Add(new DictionaryList("UT_GOOD_NG", grdMain.GetData(row, "UT_GOOD_NG").ToString()));
                        itemList.Add(new DictionaryList("MFG_DATE", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd")));
                        itemList.Add(new DictionaryList("EXIT_DDTT", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd") + wTime));
                        itemList.Add(new DictionaryList("MODIFIER", ck.UserID));

                        LogStrCreate(itemList, ref strLog);

                        logList.Add(new LogDataList(Alarms.Modified, Text, strLog));

                        UpCnt++;
                    }
                }
                #endregion grdMain1 

                //실행후 성공
                transaction.Commit();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴
                btnDisplay_Click(null, null);

                foreach (var log in logList)
                {
                    clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                }

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

                foreach (var log in logList)
                {
                    clsMsg.Log.Alarm(Alarms.Error, log.PageName, clsMsg.Log.__Line(), log.Contents);
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다. Error:"+ ex.Message);
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

        #region SetDataBinding 설정
        private void SetDataBinding()
        {

            string sql1 = "";
            start_dt_str = Utl.GetDateFormat(3, start_dt.Value);
            end_dt_str = Utl.GetDateFormat(3, end_dt.Value);

            sql1 = string.Format("SELECT TO_CHAR(ROWNUM) AS L_NO ");
            sql1 += string.Format("      ,'False' AS CHECKER  ");
            sql1 += string.Format("      ,MILL_NO   ");   //--압연번들번호                                                                           
            sql1 += string.Format("      ,PIECE_NO ");    //--PIECE NO                                                                               
            sql1 += string.Format("      ,LINE_GP  ");
            sql1 += string.Format("      ,ROUTING_CD ");
            sql1 += string.Format("      ,REWORK_SEQ ");
            sql1 += string.Format("      , MFG_DATE ");    //--작업일자                                                                               
            sql1 += string.Format("      ,WORK_TIME ");    //--작업시각                                                                                 
            sql1 += string.Format("      ,WORK_TYPE ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
            sql1 += string.Format("      ,WORK_TEAM  ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
            sql1 += string.Format("      ,POC_NO    ");
            sql1 += string.Format("      ,HEAT   ");
            sql1 += string.Format("      ,STEEL  ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM    ");
            sql1 += string.Format("      ,ITEM   ");
            sql1 += string.Format("      ,ITEM_SIZE ");
            sql1 += string.Format("      ,LENGTH  ");
            sql1 += string.Format("      ,GOOD_YN   ");    //--합부                                                                                  
            sql1 += string.Format("      ,MAT_GOOD_NG  ");
            sql1 += string.Format("      ,MLFT_GOOD_NG  ");
            sql1 += string.Format("      ,UT_GOOD_NG   ");
            sql1 += string.Format("      ,POC_END_YN  ");//--POC종료여부   
            sql1 += string.Format("      ,'' AS GUBUN  ");
            sql1 += string.Format("FROM  (SELECT  MILL_NO  ");
            sql1 += string.Format("              ,PIECE_NO  ");
            sql1 += string.Format("              ,LINE_GP  ");
            sql1 += string.Format("              ,ROUTING_CD ");
            sql1 += string.Format("              ,REWORK_SEQ ");
            sql1 += string.Format("              ,TO_DATE(MFG_DATE, 'YYYY-MM-DD' ) AS MFG_DATE  ");        //--작업일자                                                                  
            sql1 += string.Format("              ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME  ");//--작업시각                                         
            sql1 += string.Format("              ,WORK_TYPE   ");      //--근                                                                        
            sql1 += string.Format("              ,NVL(WORK_TEAM,'A') AS WORK_TEAM ");
            sql1 += string.Format("              ,POC_NO   ");
            sql1 += string.Format("              ,HEAT    ");
            sql1 += string.Format("              ,STEEL  ");
            sql1 += string.Format("              ,ITEM   ");
            sql1 += string.Format("              ,ITEM_SIZE  ");
            sql1 += string.Format("              ,LENGTH   ");
            sql1 += string.Format("              ,GOOD_YN    ");   //--합부                                                                          
            sql1 += string.Format("              ,MAT_GOOD_NG ");
            sql1 += string.Format("              ,MLFT_GOOD_NG  ");
            sql1 += string.Format("              ,UT_GOOD_NG   ");
            sql1 += string.Format("              ,(SELECT FINISH_YN FROM TB_CR_ORD WHERE POC_NO = A.POC_NO)  AS POC_END_YN "); //--POC종료여부       
            sql1 += string.Format("              ,REG_DDTT  ");
            sql1 += string.Format("        FROM  TB_CR_PIECE_WR A  ");
            sql1 += string.Format("        WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", line_gp_id);
            sql1 += string.Format("        AND   A.ROUTING_CD =  'C1'    "); //--쇼트
            if (work_type_id != "%")
            {
                sql1 += string.Format("          AND A.WORK_TYPE  LIKE '%{0}%' ", work_type_id);
            }
            if (work_TEAM_id != "%")
            {
                sql1 += string.Format("          AND NVL(A.WORK_TEAM,'A')  LIKE '%{0}%' ", work_TEAM_id);
            }
            if (txtHeat.Text != "")
            {
                sql1 += string.Format("          AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);
            }
            if (gangjong_id_tb.Text != "")
            {
                sql1 += string.Format("          AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
            }
            if (txtPoc.Text != "")
            {
                sql1 += string.Format("          AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
            }
            sql1 += string.Format("        AND    '{0}' = 'A'", gp);
            sql1 += string.Format("        AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR   ");
            sql1 += string.Format("                                 WHERE  MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("                                 AND  MILL_NO    = A.MILL_NO   ");
            sql1 += string.Format("                                 AND    PIECE_NO   = A.PIECE_NO   ");
            sql1 += string.Format("                                 AND    LINE_GP    = A.LINE_GP    ");
            sql1 += string.Format("                                 AND    ROUTING_CD = A.ROUTING_CD )  ");
            sql1 += string.Format("        AND   NOT EXISTS (SELECT ROUTING_CD FROM TB_CR_PIECE_WR   ");
            sql1 += string.Format("                          WHERE  MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("                          AND  MILL_NO  = A.MILL_NO   ");
            sql1 += string.Format("                          AND    PIECE_NO = A.PIECE_NO  ");
            sql1 += string.Format("                          AND    LINE_GP  = A.LINE_GP   ");
            sql1 += string.Format("                          AND    ROUTING_CD = 'F2'    ");
            sql1 += string.Format("                          AND    REWORK_SEQ = A.REWORK_SEQ )   ");
            sql1 += string.Format("       UNION       ");
            sql1 += string.Format("         SELECT MILL_NO "); // --압연번들번호
            sql1 += string.Format("              ,PIECE_NO  ");
            sql1 += string.Format("              ,LINE_GP   ");
            sql1 += string.Format("              ,ROUTING_CD  ");
            sql1 += string.Format("              ,REWORK_SEQ    ");
            sql1 += string.Format("              ,TO_DATE(MFG_DATE, 'YYYY-MM-DD' ) AS MFG_DATE     ");     //--작업일자 
            sql1 += string.Format("              ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME "); //--작업시각                                         
            sql1 += string.Format("              ,WORK_TYPE    ");     //--근                                                                        
            sql1 += string.Format("              ,NVL(WORK_TEAM, 'A') AS WORK_TEAM  ");
            sql1 += string.Format("              ,POC_NO   ");
            sql1 += string.Format("              ,HEAT    ");
            sql1 += string.Format("              ,STEEL  ");
            sql1 += string.Format("              ,ITEM   ");
            sql1 += string.Format("              ,ITEM_SIZE ");
            sql1 += string.Format("              ,LENGTH   ");
            sql1 += string.Format("              ,GOOD_YN   ");    //--합부                                                                          
            sql1 += string.Format("              ,MAT_GOOD_NG  ");
            sql1 += string.Format("              ,MLFT_GOOD_NG  ");
            sql1 += string.Format("              ,UT_GOOD_NG  ");
            sql1 += string.Format("              ,(SELECT FINISH_YN FROM TB_CR_ORD WHERE POC_NO = A.POC_NO)  AS POC_END_YN "); //--POC종료여부       
            sql1 += string.Format("              ,REG_DDTT   ");
            sql1 += string.Format("         FROM  TB_CR_PIECE_WR A  ");
            sql1 += string.Format("         WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("         AND   A.LINE_GP    = '{0}' ", line_gp_id);
            sql1 += string.Format("         AND   A.ROUTING_CD =  'F2'    "); //--NDT
            if (work_type_id != "%")
            {
                sql1 += string.Format("          AND A.WORK_TYPE  LIKE '%{0}%' ", work_type_id);
            }
            if (work_TEAM_id != "%")
            {
                sql1 += string.Format("          AND NVL(A.WORK_TEAM,'A')  LIKE '%{0}%' ", work_TEAM_id);
            }
            if (txtHeat.Text != "")
            {
                sql1 += string.Format("          AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);
            }
            if (gangjong_id_tb.Text != "")
            {
                sql1 += string.Format("          AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
            }
            if (txtPoc.Text != "")
            {
                sql1 += string.Format("          AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
            }
            sql1 += string.Format("         AND    '{0}' = 'B'", gp);
            sql1 += string.Format("         AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR    ");
            sql1 += string.Format("                                                        WHERE  MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("                                                        AND  MILL_NO    = A.MILL_NO   ");
            sql1 += string.Format("                                                        AND    PIECE_NO   = A.PIECE_NO   ");
            sql1 += string.Format("                                                        AND    LINE_GP    = A.LINE_GP   ");
            sql1 += string.Format("                                                        AND    ROUTING_CD = A.ROUTING_CD )   ");
            sql1 += string.Format("       ORDER BY MFG_DATE DESC, WORK_TIME DESC, MILL_NO, PIECE_NO  ");
            sql1 += string.Format("       ) X  ");

            //string sql1 = string.Format("SELECT TO_CHAR(ROWNUM) AS L_NO ");
            //sql1 += string.Format("      ,'False' AS CHECKER  ");
            //sql1 += string.Format("      ,MILL_NO   ");   //--압연번들번호                                                                           
            //sql1 += string.Format("      ,PIECE_NO ");    //--PIECE NO                                                                               
            //sql1 += string.Format("      ,LINE_GP  ");
            //sql1 += string.Format("      ,ROUTING_CD ");
            //sql1 += string.Format("      ,REWORK_SEQ ");
            //sql1 += string.Format("      , MFG_DATE ");    //--작업일자                                                                               
            //sql1 += string.Format("      ,WORK_TIME ");    //--작업시각                                                                                 
            //sql1 += string.Format("      ,WORK_TYPE ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
            //sql1 += string.Format("      ,WORK_TEAM  ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
            //sql1 += string.Format("      ,POC_NO    ");
            //sql1 += string.Format("      ,HEAT   ");
            //sql1 += string.Format("      ,STEEL  ");
            //sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM    ");
            //sql1 += string.Format("      ,ITEM   ");
            //sql1 += string.Format("      ,ITEM_SIZE ");
            //sql1 += string.Format("      ,LENGTH  ");
            //sql1 += string.Format("      ,GOOD_YN   ");    //--합부                                                                                  
            //sql1 += string.Format("      ,MAT_GOOD_NG  ");
            //sql1 += string.Format("      ,MLFT_GOOD_NG  ");
            //sql1 += string.Format("      ,UT_GOOD_NG   ");
            //sql1 += string.Format("      ,POC_END_YN  ");//--POC종료여부   
            //sql1 += string.Format("      ,'' AS GUBUN  ");
            //sql1 += string.Format("FROM  (SELECT  MILL_NO  ");
            //sql1 += string.Format("              ,PIECE_NO  ");
            //sql1 += string.Format("              ,LINE_GP  ");
            //sql1 += string.Format("              ,ROUTING_CD ");
            //sql1 += string.Format("              ,REWORK_SEQ ");
            //sql1 += string.Format("              ,TO_DATE(MFG_DATE, 'YYYY-MM-DD' ) AS MFG_DATE  ");        //--작업일자                                                                  
            //sql1 += string.Format("              ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME  ");//--작업시각                                         
            //sql1 += string.Format("              ,WORK_TYPE   ");      //--근                                                                        
            //sql1 += string.Format("              ,NVL(WORK_TEAM,'A') AS WORK_TEAM ");
            //sql1 += string.Format("              ,POC_NO   ");
            //sql1 += string.Format("              ,HEAT    ");
            //sql1 += string.Format("              ,STEEL  ");
            //sql1 += string.Format("              ,ITEM   ");
            //sql1 += string.Format("              ,ITEM_SIZE  ");
            //sql1 += string.Format("              ,LENGTH   ");
            //sql1 += string.Format("              ,GOOD_YN    ");   //--합부                                                                          
            //sql1 += string.Format("              ,MAT_GOOD_NG ");
            //sql1 += string.Format("              ,MLFT_GOOD_NG  ");
            //sql1 += string.Format("              ,UT_GOOD_NG   ");
            //sql1 += string.Format("              ,(SELECT FINISH_YN FROM TB_CR_ORD WHERE POC_NO = A.POC_NO)  AS POC_END_YN "); //--POC종료여부       
            //sql1 += string.Format("              ,REG_DDTT  ");
            //sql1 += string.Format("        FROM  TB_CR_PIECE_WR A  ");
            //sql1 += string.Format("        WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            //sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", line_gp_id);
            //sql1 += string.Format("        AND   A.ROUTING_CD =  'C1'    "); //--쇼트
            //sql1 += string.Format("        AND   A.POC_NO      LIKE '%' || '{0}' || '%' ", poc);
            //sql1 += string.Format("        AND   A.HEAT      LIKE '%' || '{0}' || '%' ", heat);
            //sql1 += string.Format("        AND   A.STEEL      LIKE  '%' || '{0}'|| '%'  ", gangjung_id);
            //sql1 += string.Format("        AND   A.WORK_TYPE  LIKE '{0}' || '%'  ", work_type_id);
            //sql1 += string.Format("        AND   A.WORK_TEAM  LIKE '{0}' || '%'  ", work_TEAM_id);
            //sql1 += string.Format("        AND    '{0}' = 'A'", gp);
            //sql1 += string.Format("        AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR   ");
            //sql1 += string.Format("                                 WHERE  MILL_NO    = A.MILL_NO   ");
            //sql1 += string.Format("                                 AND    PIECE_NO   = A.PIECE_NO   ");
            //sql1 += string.Format("                                 AND    LINE_GP    = A.LINE_GP    ");
            //sql1 += string.Format("                                 AND    ROUTING_CD = A.ROUTING_CD )  ");
            //sql1 += string.Format("        AND   NOT EXISTS (SELECT ROUTING_CD FROM TB_CR_PIECE_WR   ");
            //sql1 += string.Format("                          WHERE  MILL_NO  = A.MILL_NO   ");
            //sql1 += string.Format("                          AND    PIECE_NO = A.PIECE_NO  ");
            //sql1 += string.Format("                          AND    LINE_GP  = A.LINE_GP   ");
            //sql1 += string.Format("                          AND    ROUTING_CD = 'F2'    ");
            //sql1 += string.Format("                          AND    REWORK_SEQ = A.REWORK_SEQ )   ");
            //sql1 += string.Format("       UNION       ");
            //sql1 += string.Format("         SELECT MILL_NO "); // --압연번들번호
            //sql1 += string.Format("              ,PIECE_NO  ");
            //sql1 += string.Format("              ,LINE_GP   ");
            //sql1 += string.Format("              ,ROUTING_CD  ");
            //sql1 += string.Format("              ,REWORK_SEQ    ");
            //sql1 += string.Format("              ,TO_DATE(MFG_DATE, 'YYYY-MM-DD' ) AS MFG_DATE     ");     //--작업일자 
            //sql1 += string.Format("              ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME "); //--작업시각                                         
            //sql1 += string.Format("              ,WORK_TYPE    ");     //--근                                                                        
            //sql1 += string.Format("              ,NVL(WORK_TEAM, 'A') AS WORK_TEAM  ");
            //sql1 += string.Format("              ,POC_NO   ");
            //sql1 += string.Format("              ,HEAT    ");
            //sql1 += string.Format("              ,STEEL  ");
            //sql1 += string.Format("              ,ITEM   ");
            //sql1 += string.Format("              ,ITEM_SIZE ");
            //sql1 += string.Format("              ,LENGTH   ");
            //sql1 += string.Format("              ,GOOD_YN   ");    //--합부                                                                          
            //sql1 += string.Format("              ,MAT_GOOD_NG  ");
            //sql1 += string.Format("              ,MLFT_GOOD_NG  ");
            //sql1 += string.Format("              ,UT_GOOD_NG  ");
            //sql1 += string.Format("              ,(SELECT FINISH_YN FROM TB_CR_ORD WHERE POC_NO = A.POC_NO)  AS POC_END_YN "); //--POC종료여부       
            //sql1 += string.Format("              ,REG_DDTT   ");
            //sql1 += string.Format("         FROM  TB_CR_PIECE_WR A  ");
            //sql1 += string.Format("         WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            //sql1 += string.Format("         AND   A.LINE_GP    = '{0}' ", line_gp_id);
            //sql1 += string.Format("         AND   A.ROUTING_CD =  'F2'    "); //--NDT
            //sql1 += string.Format("         AND   A.POC_NO      LIKE '%' ||'{0}' || '%' ", poc);
            //sql1 += string.Format("         AND   A.HEAT      LIKE '%' || '{0}' || '%' ", heat);
            //sql1 += string.Format("         AND   A.STEEL      LIKE '%' ||'{0}'|| '%'  ", gangjung_id);
            //sql1 += string.Format("         AND   A.WORK_TYPE  LIKE '{0}' || '%'  ", work_type_id);
            //sql1 += string.Format("         AND   A.WORK_TEAM  LIKE '{0}' || '%'  ", work_TEAM_id);
            //sql1 += string.Format("         AND    '{0}' = 'B'", gp);
            //sql1 += string.Format("         AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR    ");
            //sql1 += string.Format("                                                        WHERE  MILL_NO    = A.MILL_NO   ");
            //sql1 += string.Format("                                                        AND    PIECE_NO   = A.PIECE_NO   ");
            //sql1 += string.Format("                                                        AND    LINE_GP    = A.LINE_GP   ");
            //sql1 += string.Format("                                                        AND    ROUTING_CD = A.ROUTING_CD )   ");
            //sql1 += string.Format("       ORDER BY MFG_DATE DESC, WORK_TIME DESC, MILL_NO, PIECE_NO  ");
            //sql1 += string.Format("       ) X  ");

            olddt = cd.FindDataTable(sql1);
            moddt = olddt.Copy();
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            grdMain.AutoSizeCols(grdMain.Cols["L_NO"].Index, grdMain.Cols["UT_GOOD_NG"].Index, 1);
            grdMain.AutoSizeCols(grdMain.Cols["WORK_TYPE_NM"].Index, grdMain.Cols["POC_END_YN"].Index, 1);
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;
        }
        #endregion SetDataBinding 설정

        #region 이벤트 설정
        #region Selected IndexChanged 이벤트 설정
        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp_id = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;
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
            work_type_id = ((ComLib.DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }
        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_TEAM_id = ((ComLib.DictionaryList)cboTEAM.SelectedItem).fnValue;
        }
        #endregion Selected IndexChanged 이벤트 설정

        #region Value Changed 이벤트 설정
        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_dt_str = clsUtil.Utl.GetDateFormat(3, start_dt.Value);
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            end_dt_str = clsUtil.Utl.GetDateFormat(3, end_dt.Value);
        }
        #endregion Value Changed 이벤트 설정

        #region TextChanged 이벤트 설정
        private void txtSteel_id_TextChanged(object sender, EventArgs e)
        {
            gangjung_id = gangjong_id_tb.Text;
        }

        private void txtPoc_TextChanged(object sender, EventArgs e)
        {
            poc = txtPoc.Text;
        }

        private void txtHeat_TextChanged(object sender, EventArgs e)
        {
            heat = txtHeat.Text;
        }
        #endregion TextChanged 이벤트 설정

        #region Grid Before Edit & After Edit 이벤트 설정
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid edited_grd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            // 수정여부 확인을 위해 저장
            strBefValue = edited_grd.GetData(editedRow, editedCol).ToString();// .ToString() 삭제 
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            // No,구분은 수정 불가
            if (editedRow == 0 || editedCol == editedGrd.Cols.Count - 1)
            {
                editedGrd.SetData(editedRow, editedCol, strBefValue);
                return;
            }

            if (editedGrd.Row > 0)
            {
                // 수정된 내용이 없으면 Update 처리하지 않는다.
                if (strBefValue == editedGrd.GetData(editedRow, editedCol).ToString().Trim())
                    return;

                // 선택이 변했을때는 그대로 리턴...
                if (editedCol == editedGrd.Cols["CHECKER"].Index)
                {
                    return;
                }

                editedGrd.SetData(editedGrd.Row, editedGrd.Cols.Count - 1, "수정");
                editedGrd.SetData(editedGrd.Row, 0, "수정");
                //Update 배경색 지정
                editedGrd.Rows[editedGrd.Row].Style = editedGrd.Styles["UpColor"];
            }
        }
        #endregion Grid Before Edit & After Edit 이벤트 설정

        #region Click 이벤트 설정
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

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            SetDataBinding();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, grdMain);
        }
        #endregion Click 이벤트 설정

        #region 기타 이벤트 설정
        private void txtCheckNumeric_KeyPress(object sender, KeyPressEventArgs e)

        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))

                e.Handled = true;
        }

        private void grdMain_CellChanged(object sender, RowColEventArgs e)
        {
            int num = e.Row;

            if (detector == true && grdMain.GetData(num, "GOOD_YN").ToString() == "OK")
            {
                grdMain.SetData(num, "MAT_GOOD_NG", "OK");
                grdMain.SetData(num, "MLFT_GOOD_NG", "OK");
                grdMain.SetData(num, "UT_GOOD_NG", "OK");
                detector = false;
            }
            else
            {
                if (grdMain.GetData(num, "MAT_GOOD_NG").ToString() == "NG")
                {
                    grdMain.SetData(num, "GOOD_YN", "NG");
                }
                if (grdMain.GetData(num, "MLFT_GOOD_NG").ToString() == "NG")
                {
                    grdMain.SetData(num, "GOOD_YN", "NG");
                }
                if (grdMain.GetData(num, "UT_GOOD_NG").ToString() == "NG")
                {
                    grdMain.SetData(num, "GOOD_YN", "NG");
                }
                if (grdMain.GetData(num, "MAT_GOOD_NG").ToString() == "OK" && grdMain.GetData(num, "MLFT_GOOD_NG").ToString() == "OK" && grdMain.GetData(num, "UT_GOOD_NG").ToString() == "OK")
                {
                    grdMain.SetData(num, "GOOD_YN", "OK");
                }
                detector = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        //사용자이벤트 생성
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
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
        #endregion 기타 이벤트 설정
        #endregion 이벤트 설정
    }
}