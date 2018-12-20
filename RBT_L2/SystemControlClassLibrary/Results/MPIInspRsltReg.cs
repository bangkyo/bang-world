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
using System.Collections;
using static SystemControlClassLibrary.Order.CrtInOrdCre;


namespace SystemControlClassLibrary.information
{
    public partial class MPIInspRsltReg : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();


        OracleTransaction transaction = null;

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
        private string steel_nm = "";
        private string line_gp_id = "";
        private string line_gp_nm = "";
        private string work_type_id = "";
        private string work_type_nm = "";
        private string work_team_id = "";
        private string rework_YN_id = "";
        private string auto_YN_id = "";
        private string tested_gp = "";
        private string poc = "";
        private string gangjung_id = "";

        private string line_id = "";
        private string line_nm = "";
        private string worktype_id = "";
        private string worktype_nm = "";

        private string fault_cd_id = "";
        private string fault_cd_nm = "";
        private string mpi_good_ng_id = "";
        private string mpi_good_ng_nm = "";
        private string workteam_id = "";
        private string workteam_nm = "";

        private string start_dt_str = "";
        private string end_dt_str = "";

        ArrayList _al = new ArrayList();
        private bool allChecked;
        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public MPIInspRsltReg(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        ~MPIInspRsltReg()
        {

        }
        private void MPIInspRsltReg_Load(object sender, EventArgs e)
        {
            InitControl();

            EventCreate();      //사용자정의 이벤트

            btnDisplay_Click(null, null);
        }
        #endregion 로딩, 생성자 설정

        #region Init Control 설정
        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            //InitLabel
            cs.InitLabel(lblHeat);
            cs.InitLabel(lblSteel);
            cs.InitLabel(lblLine);
            cs.InitLabel(lblGubun);
            cs.InitLabel(lblPoc);
            cs.InitLabel(lblWorkType);
            cs.InitLabel(lblMfgDate);
            cs.InitLabel(lblTEAM);

            //InitButton
            cs.InitButton(btnExcel);
            cs.InitButton(btnSave);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            //InitCombo
            cs.InitCombo(cbo_Work_Type, StringAlignment.Near);
            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            cs.InitCombo(cboGubun_GP, StringAlignment.Near);
            cs.InitCombo(cboTEAM, StringAlignment.Near);
            cs.InitCombo(cboRework_YN, StringAlignment.Near);
            cs.InitCombo(cboAuto_YN, StringAlignment.Near);

            //InitTextBox
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
            SetComboBox5();
            SetComboBox6();

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

        #region Init Grid Main 설정
        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            #region Column Head Caption 색깔 설정
            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["MPI_INSP_GOOD_NG"].Index, 0, grdMain.Cols["MPI_FAULT_CD"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];
            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["CHECKER"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];
            #endregion Column Head Caption 색깔 설정

            #region Width 설정
            //grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            grdMain.AllowSorting = AllowSortingEnum.SingleColumn;

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["CHECKER"].Width = cs.Sel_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;
            grdMain.Cols["REWORK_SEQ"].Width = cs.REWORK_SEQ_Width;
            grdMain.Cols["AUTO_YN"].Width = cs.REWORK_SEQ_Width;
            grdMain.Cols["MPI_INSP_GOOD_NG"].Width = cs.Good_NG_L_Width;
            grdMain.Cols["MPI_FAULT_CD"].Width = cs.MPI_FAULT_CD_Width +30;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TIME"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width -5;
            grdMain.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width -5;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width -35;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width -20;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width +10;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain.Cols["GUBUN"].Width = 0;
            #endregion Width 설정

            #region grdMain head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            
            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["CHECKER"].TextAlign = cs.SEL_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["PIECE_NO"].TextAlign = cs.PIECE_NO_TextAlign;
            grdMain.Cols["REWORK_SEQ"].TextAlign = cs.REWORK_SEQ_TextAlign;
            grdMain.Cols["AUTO_YN"].TextAlign = cs.REWORK_SEQ_TextAlign;
            grdMain.Cols["MPI_INSP_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["MPI_FAULT_CD"].TextAlign = cs.MPI_FAULT_CD_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TEAM_NM_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;

            #endregion

            #region 그리드 수정가능 및 컬럼 콤보박스 설정 
            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].AllowEditing = false;
            grdMain.Cols["MILL_NO"].AllowEditing = false;
            grdMain.Cols["PIECE_NO"].AllowEditing = false;
            grdMain.Cols["REWORK_SEQ"].AllowEditing = false;
            grdMain.Cols["AUTO_YN"].AllowEditing = false;
            grdMain.Cols["MFG_DATE"].AllowEditing = false;
            grdMain.Cols["WORK_TIME"].AllowEditing = false;
            grdMain.Cols["WORK_TYPE_NM"].AllowEditing = false;
            grdMain.Cols["WORK_TEAM_NM"].AllowEditing = false;
            grdMain.Cols["POC_NO"].AllowEditing = false;
            grdMain.Cols["HEAT"].AllowEditing = false;
            grdMain.Cols["STEEL"].AllowEditing = false;
            grdMain.Cols["STEEL_NM"].AllowEditing = false;
            grdMain.Cols["ITEM"].AllowEditing = false;
            grdMain.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["GUBUN"].AllowEditing = false;
            #endregion 그리드 수정가능 및 컬럼 콤보박스 설정 

            #region 콤보박스 설정
            SetComboinGrd();
            grdMain.Cols["MPI_INSP_GOOD_NG"].DataMap = ngTest_Value;
            grdMain.Cols["MPI_INSP_GOOD_NG"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["MPI_FAULT_CD"].DataMap = fault_cd;
            grdMain.Cols["MPI_FAULT_CD"].TextAlign = TextAlignEnum.LeftCenter;
            #endregion 콤보박스 설정

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new HostedControl(grdMain, lbSel, 0, grdMain.Cols["CHECKER"].Index));

            grdMain.ExtendLastCol = true;
        }



        private void SEL_Click(object sender, EventArgs e)
        {
            if (allChecked)
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "CHECKER", false);

                }
                allChecked = false;
            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "CHECKER", true);

                }
                allChecked = true;
            }
        }
        #endregion Init Grid Main 설정

        #region ComboBox 설정
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
                    fault_cd.Add(row["CD_ID"].ToString(), row["CD_ID"].ToString() + "  " + row["CD_NM"].ToString());
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
                list3.Add("검사완료"); // B
                list3.Add("미검사");   // A

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

        private void SetComboBox5()
        {
            cd.SetCombo(cboRework_YN, "REWORK_YN", "", true);
        }

        private void SetComboBox6()
        {
            cd.SetCombo(cboAuto_YN, "AUTO_YN", "", true);
        }
        #endregion ComboBox 설정

        #region SetDateBinding 설정
        private void SetDataBinding()
        {
            string sql1 = string.Format("SELECT TO_CHAR(ROWNUM) AS L_NO ");
            sql1 += string.Format("      ,'False' AS CHECKER  ");
            sql1 += string.Format("      ,MILL_NO  ");// --KEY  압연번들번호
            sql1 += string.Format("      ,PIECE_NO "); //--KEY  PIECE NO
            sql1 += string.Format("      ,MPI_INSP_GOOD_NG "); //--합부  
            sql1 += string.Format("      ,MPI_FAULT_CD "); //--결함코드
            sql1 += string.Format("      ,LINE_GP "); //--KEY
            sql1 += string.Format("      ,ROUTING_CD");// --KEY
            sql1 += string.Format("      ,REWORK_SEQ ");// --KEY
            sql1 += string.Format("      ,AUTO_YN ");// --KEY
            sql1 += string.Format("      ,TO_DATE(MFG_DATE, 'YYYY-MM-DD' ) AS MFG_DATE "); //--작업일자
            sql1 += string.Format("      ,WORK_TIME "); //--작업시각
            sql1 += string.Format("      ,WORK_TYPE ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
            sql1 += string.Format("      ,WORK_TEAM ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
            sql1 += string.Format("      ,POC_NO");
            sql1 += string.Format("      ,HEAT  ");
            sql1 += string.Format("      ,STEEL ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
            sql1 += string.Format("      ,ITEM");
            sql1 += string.Format("      ,ITEM_SIZE ");
            sql1 += string.Format("      ,LENGTH  ");
            sql1 += string.Format("      ,'' AS GUBUN  ");
            sql1 += string.Format("FROM  (SELECT  MILL_NO ");
            sql1 += string.Format("              ,PIECE_NO");
            sql1 += string.Format("              ,MPI_INSP_GOOD_NG  ");
            sql1 += string.Format("              ,MPI_FAULT_CD  ");
            sql1 += string.Format("              ,LINE_GP ");
            sql1 += string.Format("              ,ROUTING_CD");
            sql1 += string.Format("              ,REWORK_SEQ");
            sql1 += string.Format("              ,NVL(AUTO_YN, 'N') AS AUTO_YN");
            sql1 += string.Format("              ,MFG_DATE");
            sql1 += string.Format("              ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME"); //--작업시각
            sql1 += string.Format("              ,WORK_TYPE ");
            sql1 += string.Format("              ,NVL(WORK_TEAM, 'A') AS WORK_TEAM ");
            sql1 += string.Format("              ,POC_NO ");
            sql1 += string.Format("              ,HEAT ");
            sql1 += string.Format("              ,STEEL ");
            sql1 += string.Format("              ,ITEM ");
            sql1 += string.Format("              ,ITEM_SIZE ");
            sql1 += string.Format("              ,LENGTH ");
            sql1 += string.Format("              ,REG_DDTT ");
            sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ");
            sql1 += string.Format("        WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", line_gp_id);
            sql1 += string.Format("        AND   A.ROUTING_CD =  'F2'"); //--NDT
            sql1 += string.Format("        AND   A.POC_NO      LIKE '{0}' || '%' ", poc);
            sql1 += string.Format("        AND   A.HEAT      LIKE '%' || '{0}' || '%' ", heat);
            sql1 += string.Format("        AND   A.STEEL      LIKE '{0}'|| '%'  ", gangjung_id);
            sql1 += string.Format("        AND   A.WORK_TYPE  LIKE '{0}' || '%'  ", work_type_id);
            sql1 += string.Format("        AND   A.WORK_TEAM  LIKE '{0}' || '%'  ", work_team_id);
            sql1 += string.Format("        AND    '{0}' = 'A'", tested_gp);
            sql1 += string.Format("        AND   MLFT_GOOD_NG = 'NG' ");
            sql1 += string.Format("        AND   A.GOOD_YN IS NOT NULL");
            sql1 += string.Format("        AND   NVL(A.REWORK_YN, 'N')   LIKE '%{0}%' ", rework_YN_id);
            sql1 += string.Format("        AND   NOT EXISTS(SELECT ROUTING_CD FROM TB_CR_PIECE_WR ");
            sql1 += string.Format("                         WHERE  MILL_NO  = A.MILL_NO ");
            sql1 += string.Format("                         AND    PIECE_NO = A.PIECE_NO ");
            sql1 += string.Format("                         AND    LINE_GP  = A.LINE_GP ");
            sql1 += string.Format("                         AND    ROUTING_CD = 'H2'  ");
            sql1 += string.Format("                          AND    REWORK_SEQ = A.REWORK_SEQ ) ");
            sql1 += string.Format("        UNION                                        "); 
            sql1 += string.Format("        SELECT MILL_NO ");
            sql1 += string.Format("              ,PIECE_NO");
            sql1 += string.Format("              ,MPI_INSP_GOOD_NG  ");
            sql1 += string.Format("              ,MPI_FAULT_CD  ");
            sql1 += string.Format("              ,LINE_GP ");
            sql1 += string.Format("              ,ROUTING_CD");
            sql1 += string.Format("              ,REWORK_SEQ");
            sql1 += string.Format("              ,NVL(AUTO_YN, 'N') AS AUTO_YN");
            sql1 += string.Format("              ,MFG_DATE");
            sql1 += string.Format("              ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME"); //--작업시각
            sql1 += string.Format("              ,WORK_TYPE ");
            sql1 += string.Format("              ,NVL(WORK_TEAM, 'A') AS WORK_TEAM ");
            sql1 += string.Format("              ,POC_NO ");
            sql1 += string.Format("              ,HEAT ");
            sql1 += string.Format("              ,STEEL ");
            sql1 += string.Format("              ,ITEM ");
            sql1 += string.Format("              ,ITEM_SIZE ");
            sql1 += string.Format("              ,LENGTH ");
            sql1 += string.Format("              ,REG_DDTT ");
            sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ");
            sql1 += string.Format("        WHERE A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", line_gp_id);
            sql1 += string.Format("        AND   A.ROUTING_CD =  'H2'"); //--MPI
            sql1 += string.Format("        AND   A.POC_NO       LIKE  '%' || '{0}' || '%' ", poc);
            sql1 += string.Format("        AND   A.HEAT         LIKE  '%' || '{0}' || '%' ", heat);
            sql1 += string.Format("        AND   A.STEEL        LIKE  '%' || '{0}' || '%'  ", gangjung_id);
            sql1 += string.Format("        AND   A.WORK_TYPE    LIKE         '{0}' || '%'  ", work_type_id);
            sql1 += string.Format("        AND   A.WORK_TEAM    LIKE         '{0}' || '%'  ", work_team_id);
            sql1 += string.Format("        AND   '{0}' = 'B'", tested_gp); // 
            sql1 += string.Format("        AND   NVL(A.REWORK_YN, 'N')   LIKE '%{0}%' ", rework_YN_id);
            sql1 += string.Format("        AND   NVL(A.AUTO_YN, 'N')   LIKE '%{0}%' ", auto_YN_id);
            sql1 += string.Format("        AND   A.GOOD_YN IS NOT NULL"); 
            sql1 += string.Format("        ORDER BY MFG_DATE DESC, WORK_TIME DESC , MILL_NO, PIECE_NO ");
            sql1 += string.Format("        ) X  ");

            olddt = cd.FindDataTable(sql1);
            moddt = olddt.Copy();
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            grdMain.AutoSizeCols(grdMain.Cols["L_NO"].Index, grdMain.Cols["MPI_INSP_GOOD_NG"].Index, 4);
            grdMain.AutoSizeCols(grdMain.Cols["MFG_DATE"].Index, grdMain.Cols["LENGTH"].Index, 4);

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;
        }
        #endregion SetDateBinding 설정

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
                gubun_value = grdMain.GetData(checkrow, "GUBUN").ToString();

                isChecked = (grdMain.GetData(checkrow, "CHECKER").ToString() == "True") ? true : false;

                if (!isChecked)
                {
                    continue;
                }

                if (gubun_value == "수정")
                {
                    if (grdMain.GetData(checkrow, "MPI_INSP_GOOD_NG").ToString() == "NG" && grdMain.GetData(checkrow, "MPI_FAULT_CD").ToString() == "")
                    {
                        show_msg = string.Format("{0}를 입력하세요.", "결함코드");
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 3라인일경우만 체크
                    if (line_gp_id == "#3")
                    {
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
                    isChecked = (grdMain.GetData(row, "CHECKER").ToString() == "True") ? true : false;

                    // Update 처리
                    if (isChecked & gubun_value == "수정")
                    {
                        time = Convert.ToDateTime(grdMain.GetData(row, "WORK_TIME").ToString());
                        wTime = clsUtil.Utl.GetDateFormat(8, time);

                        strLog = string.Empty;

                        string sql1 = string.Format(" MERGE INTO TB_CR_PIECE_WR  A ");
                        sql1 += string.Format("USING ( SELECT '{0}'    AS MILL_NO ", grdMain.GetData(row, "MILL_NO").ToString());
                        sql1 += string.Format("              ,'{0}'   AS PIECE_NO ", grdMain.GetData(row, "PIECE_NO").ToString());
                        sql1 += string.Format("              ,'{0}'   AS LINE_GP ", line_gp_id);
                        sql1 += string.Format("              ,'H2'          AS ROUTING_CD");
                        sql1 += string.Format("              ,'{0}'             AS REWORK_SEQ ", olddt.Rows[row - 1]["REWORK_SEQ"].ToString());//,0 AS REWORK_SEQ");
                        sql1 += string.Format("        FROM  DUAL ) B ");
                        sql1 += string.Format("ON    ( A.MILL_NO    = B.MILL_NO  AND");
                        sql1 += string.Format("        A.PIECE_NO   = B.PIECE_NO AND");
                        sql1 += string.Format("        A.LINE_GP    = B.LINE_GP  AND");
                        sql1 += string.Format("        A.ROUTING_CD = B.ROUTING_CD AND");
                        sql1 += string.Format("        A.REWORK_SEQ = B.REWORK_SEQ )");
                        sql1 += string.Format("WHEN  MATCHED THEN");
                        sql1 += string.Format("      UPDATE  SET");
                        sql1 += string.Format("              GOOD_YN          = '{0}' ", grdMain.GetData(row, "MPI_INSP_GOOD_NG").ToString());
                        sql1 += string.Format("              ,MPI_INSP_GOOD_NG = '{0}'", grdMain.GetData(row, "MPI_INSP_GOOD_NG").ToString());  
                        sql1 += string.Format("              ,MPI_FAULT_CD     = '{0}' ", grdMain.GetData(row, "MPI_FAULT_CD").ToString());
                        sql1 += string.Format("              ,MODIFIER      = '{0}' ", ck.UserID);
                        sql1 += string.Format("              ,MOD_DDTT      = SYSDATE ");
                        sql1 += string.Format("WHEN NOT MATCHED THEN ");
                        sql1 += string.Format("      INSERT (MILL_NO");
                        sql1 += string.Format("             ,PIECE_NO");
                        sql1 += string.Format("             ,GOOD_YN");
                        sql1 += string.Format("             ,MPI_INSP_GOOD_NG");
                        sql1 += string.Format("             ,MPI_FAULT_CD");
                        sql1 += string.Format("             ,LINE_GP");
                        sql1 += string.Format("             ,ROUTING_CD");
                        sql1 += string.Format("             ,REWORK_SEQ");
                        sql1 += string.Format("             ,POC_NO");
                        sql1 += string.Format("             ,HEAT");
                        sql1 += string.Format("             ,STEEL");
                        sql1 += string.Format("             ,ITEM");
                        sql1 += string.Format("             ,ITEM_SIZE");
                        sql1 += string.Format("             ,LENGTH");
                        sql1 += string.Format("             ,ENTRY_DDTT");
                        sql1 += string.Format("             ,EXIT_DDTT");
                        sql1 += string.Format("             ,MFG_DATE");
                        sql1 += string.Format("             ,WORK_TYPE");
                        sql1 += string.Format("             ,WORK_TEAM");
                        sql1 += string.Format("             ,REGISTER");
                        sql1 += string.Format("             ,REG_DDTT");
                        sql1 += string.Format("             ) VALUES (");
                        sql1 += string.Format("              B.MILL_NO");
                        sql1 += string.Format("             ,B.PIECE_NO");
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "MPI_INSP_GOOD_NG").ToString()); 
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "MPI_INSP_GOOD_NG").ToString());
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "MPI_FAULT_CD").ToString());
                        sql1 += string.Format("             ,B.LINE_GP");
                        sql1 += string.Format("             ,B.ROUTING_CD");
                        sql1 += string.Format("             ,B.REWORK_SEQ ");
                        sql1 += string.Format("             , '{0}'   ", grdMain.GetData(row, "POC_NO").ToString());
                        sql1 += string.Format("             , '{0}' ", grdMain.GetData(row, "HEAT").ToString());
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "STEEL").ToString());
                        sql1 += string.Format("             ,'{0}' ", grdMain.GetData(row, "ITEM").ToString());
                        sql1 += string.Format("             ,'{0}'", grdMain.GetData(row, "ITEM_SIZE").ToString());
                        sql1 += string.Format("             ,'{0}'", grdMain.GetData(row, "LENGTH").ToString());
                        sql1 += string.Format("             , TO_DATE('{0}' ||  '{1}','YYYYMMDDHH24MISS'  )", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"), wTime); //grdMain.GetData(row, "WORK_TIME").ToString()
                        sql1 += string.Format("             , TO_DATE('{0}' ||  '{1}','YYYYMMDDHH24MISS'  )" , vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"), wTime);
                        sql1 += string.Format("             ,'{0}' ", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"));
                        sql1 += string.Format("             ,'{0}'  ", olddt.Rows[row - 1]["WORK_TYPE"].ToString());
                        sql1 += string.Format("             ,'{0}' ", olddt.Rows[row - 1]["WORK_TEAM"].ToString());
                        sql1 += string.Format("             ,'{0}' ", ck.UserID);
                        sql1 += string.Format("             ,SYSDATE");
                        sql1 += string.Format("             )");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        itemList.Add(new DictionaryList("MILL_NO", grdMain.GetData(row, "MILL_NO").ToString()));
                        itemList.Add(new DictionaryList("PIECE_NO", grdMain.GetData(row, "PIECE_NO").ToString()));
                        itemList.Add(new DictionaryList("LINE_GP", line_gp_id));
                        itemList.Add(new DictionaryList("ROUTING_CD", "H2"));
                        itemList.Add(new DictionaryList("REWORK_SEQ", olddt.Rows[row - 1]["REWORK_SEQ"].ToString()));

                        itemList.Add(new DictionaryList("GOOD_YN", grdMain.GetData(row, "MPI_INSP_GOOD_NG").ToString()));
                        itemList.Add(new DictionaryList("MPI_INSP_GOOD_NG", grdMain.GetData(row, "MPI_INSP_GOOD_NG").ToString()));
                        itemList.Add(new DictionaryList("MPI_FAULT_CD", grdMain.GetData(row, "MPI_FAULT_CD").ToString()));
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

                AddProcedure();
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
                MessageBox.Show(ex.Message);
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
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Click 이벤트 설정

        #region Selected Index Changed 이벤트 설정
        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp_id = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = line_gp_id;
        }

        private void cboGubun_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGubun_GP.SelectedIndex == 1)
            {
                tested_gp = "A";
            }
            else 
            {
                tested_gp = "B";
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

        private void cboRework_YN_SelectedIndexChanged(object sender, EventArgs e)
        {
            rework_YN_id =((DictionaryList)cboRework_YN.SelectedItem).fnValue;
        }

        private void cboAuto_YN_SelectedIndexChanged(object sender, EventArgs e)
        {
            auto_YN_id = ((DictionaryList)cboAuto_YN.SelectedItem).fnValue;
        }
        #endregion Selected Index Changed 이벤트 설정

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

        #region Text Changed 이벤트 설정
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

        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjung_id = gangjong_id_tb.Text;
        }
        #endregion Text Changed 이벤트 설정
        
        #region Grid Main Before Edit & After Edit 설정
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
                        editedGrd.SetData(editedRow, "MPI_INSP_GOOD_NG", "NG");
                    }
                }

                if (editedCol == editedGrd.Cols["MPI_INSP_GOOD_NG"].Index)
                {
                    if (editedGrd.GetData(editedRow, "MPI_INSP_GOOD_NG").ToString() == "OK")
                    {
                        editedGrd.SetData(editedRow, "MPI_FAULT_CD", "");
                    }
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                editedGrd.SetData(editedRow, gubun_index, "수정");
                editedGrd.SetData(editedRow, 0, "수정");

                // Update 배경색 지정
                editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            }
        }
        #endregion Grid Main Before Edit & After Edit 설정

        #region 기타 이벤트 설정
        private void grdMain_CellChanged(object sender, RowColEventArgs e)
        {
            workteam_nm = cboLine_GP.SelectedItem as string; 
            workteam_id = cd.Find_CD_ID("LINE_GP", workteam_nm);
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
        private void AddProcedure()
        {
            string spName = "SP_MPIGR_WR_SUM";  // 조회용 저장프로시저명
            //string mfg_date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();
            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();


            try
            {
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;



                op = new OracleParameter("P_FR_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = start_dt_str;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_TO_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = end_dt_str;
                cmd.Parameters.Add(op);

                oConn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.ExecuteOracleScalar();


                transaction.Commit();


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

   
    }
}
