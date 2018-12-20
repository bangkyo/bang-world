﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using SystemControlClassLibrary.UC.sub_UC;
using ChartFX.WinForms;
using System.Drawing;
using System.Collections;

namespace SystemControlClassLibrary.Results
{
    public partial class LineProcWrkGrf : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        clsStyle cs = new clsStyle();

        private string ownerNM = "";
        private string titleNM = "";

        string sql1 = string.Empty;
        C1FlexGrid selectedGrd;

        private UC_Line_gp_s uC_Line_gp_s1;
        private UC_Routing_cd uC_Routing_cd1;
        private UC_Work_Date_Fr_To uC_Work_Date_Fr_To1;

        DataTable moddt_grdSTR1;
        DataTable moddt_grdSTR2;
        DataTable moddt_grdSTR3;
        DataTable moddt_grdCHF1;
        DataTable moddt_grdCHF2;
        DataTable moddt_grdCHF3;

        DataTable checker_dt;

        DataTable chart_dt;

        bool IsChangeLineOrRoutingOrCheckerInGrid;
        ArrayList _al = new ArrayList();
        private bool allChecked = true;
        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public LineProcWrkGrf(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void LineProcWrkGrf_Load(object sender, EventArgs e)
        {
            InitControl();

            btnDisplay_Click(null, null);
        }
        #endregion 로딩, 생성자 설정

        #region Init Control 설정
        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitButton(btnExcel);

            cs.InitButton(btnDisplay);

            cs.InitButton(btnClose);
            
            #region 유저컨트롤 설정
            int location_x = 0;
            int location_y = 0;
            //
            // uC_Line_gp_s1
            // 
            uC_Line_gp_s1 = new UC_Line_gp_s();
            uC_Line_gp_s1.BackColor = System.Drawing.Color.Transparent;
            uC_Line_gp_s1.cb_Enable = true;
            uC_Line_gp_s1.Line_GP = "#1";
            uC_Line_gp_s1.Location = new System.Drawing.Point(26 + location_x, 8 + location_y);
            uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            uC_Line_gp_s1.TabIndex = 1;
            uC_Line_gp_s1.LineChangedEvent += UC_Line_gp_s1_LineChangedEvent;

            // 
            // uC_Routing_cd1
            // 
            uC_Routing_cd1 = new UC_Routing_cd();
            uC_Routing_cd1.BackColor = System.Drawing.Color.Transparent;
            uC_Routing_cd1.cb_Enable = true;
            uC_Routing_cd1.Location = new System.Drawing.Point(329 + location_x, 8 + location_y);
            uC_Routing_cd1.Name = "uC_Routing_cd1";
            uC_Routing_cd1.Routing_cd = "A1";
            uC_Routing_cd1.Size = new System.Drawing.Size(203, 27);
            uC_Routing_cd1.TabIndex = 20;
            uC_Routing_cd1.RoutingChangedEvent += UC_Routing_cd1_RoutingChangedEvent;


            // 
            // uC_Work_Date_Fr_To1
            // 
            uC_Work_Date_Fr_To1 = new UC_Work_Date_Fr_To();
            uC_Work_Date_Fr_To1.BackColor = System.Drawing.Color.Transparent;
            uC_Work_Date_Fr_To1.Location = new System.Drawing.Point(532 + 100 + location_x, 8 + location_y);
            uC_Work_Date_Fr_To1.Name = "uC_Work_Date_Fr_To1";
            uC_Work_Date_Fr_To1.TabIndex = 20;

            uC_Work_Date_Fr_To1.Work_From_Date = DateTime.Now.AddHours(-1);
            uC_Work_Date_Fr_To1.Work_To_Date = DateTime.Now;

            panel1.Controls.Add(this.uC_Work_Date_Fr_To1);
            panel1.Controls.Add(this.uC_Routing_cd1);
            panel1.Controls.Add(this.uC_Line_gp_s1);
            #endregion

            InitGrd();
            InitChart();
        }

        private void UC_Routing_cd1_RoutingChangedEvent(object sender, EventArgs e)
        {
            if (!IsChangeLineOrRoutingOrCheckerInGrid)
            {
                IsChangeLineOrRoutingOrCheckerInGrid = true;
            }

            //btnDisplay_Click(null, null);
        }

        private void UC_Line_gp_s1_LineChangedEvent(object sender, EventArgs e)
        {
            if (!IsChangeLineOrRoutingOrCheckerInGrid)
            {
                IsChangeLineOrRoutingOrCheckerInGrid = true;
            }
            //btnDisplay_Click(null, null);
        }
        #endregion Init Control 설정

        #region InitChart 설정
        private void InitChart()
        {
            chart1.Reset();
            chart1.LegendBox.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chart1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

            
            chart1.Dock = DockStyle.Fill;
            chart1.ToolBar.Visible = false;
            chart1.LegendBox.Visible = false;
            chart1.ContextMenus = false; //팝업메뉴 죽임
            chart1.AxisY.AutoScale = true;
            chart1.AxisY.ForceZero = false; // 0 을 최소값으로 두지 않는 옵션.
            chart1.AxisY.DataFormat.Decimals = 1;
        }


        #endregion InitChart 설정

        #region Init Grid 설정
        private void InitGrd()
        {
            // 선택 항목
            InitGrid_Checker(grdCheck);

            // #1 교정
            InitGrd_STR1(grdSTR1);

            // #2 교정
            InitGrd_STR_com(grdSTR2);

            // #3 교정
            InitGrd_STR_com(grdSTR3);

            // #1 면취
            InitGrd_CHF1(grdCHF1);

            // #2 면취
            InitGrd_CHF_com(grdCHF2);

            // #3 면취
            InitGrd_CHF_com(grdCHF3);
        }

        private void InitGrid_Checker(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search(grd);

            var crCellRange = grd.GetCellRange(0, grd.Cols["CHECKER"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grd.Styles["ModifyStyle"];

            grd.Cols["CHECKER"].AllowEditing = true;
            grd.Cols["ITEM"].AllowEditing = false;
            grd.Cols["ITEM_GUBUN"].AllowEditing = false;

            grd.Cols["CHECKER"].Caption = "선택";
            grd.Cols["ITEM"].Caption = "항목";
            grd.Cols["ITEM_GUBUN"].Caption = "구분";

            grd.Cols["CHECKER"].Width = cs.Sel_Width;
            grd.Cols["ITEM"].Width = cs.Item_L_Width;
            grd.Cols["ITEM_GUBUN"].Width = 0;

            grd.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
            grd.Cols["CHECKER"].TextAlign = cs.SEL_TextAlign;
            grd.Cols["ITEM"].TextAlign = cs.Item_List_TextAlign;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grd, lbSel, 0, grd.Cols["CHECKER"].Index));

        }

        private void SEL_Click(object sender, EventArgs e)
        {
            if (allChecked)
            {
                for (int rowCnt = 1; rowCnt < grdCheck.Rows.Count; rowCnt++)
                {
                    grdCheck.SetData(rowCnt, "CHECKER", false);
                    
                }
                allChecked = false;
            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdCheck.Rows.Count; rowCnt++)
                {
                    grdCheck.SetData(rowCnt, "CHECKER", true);

                }
                allChecked = true;
            }


            UpdateChart(checker_dt, chart1);

            if (IsChangeLineOrRoutingOrCheckerInGrid)
            {
                IsChangeLineOrRoutingOrCheckerInGrid = true;
            }
        }


        private void InitGrd_CHF_com(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search(grd);
            grd.AllowEditing = false;

            grd.Cols["L_NO"].Caption = "NO";
            grd.Cols["WORK_DDTT"].Caption = "작업시각";
            grd.Cols["SCREW_FEED_CURRENT_ACTV_1"].Caption = "#1SCREW전류";
            grd.Cols["GRIND_CURRENT_ACTV_1"].Caption = "#1그라인드모터전류";
            grd.Cols["STOPPER_ACTV_1"].Caption = "#1STOPPER";
            grd.Cols["GUIDE_ACTV_1"].Caption = "#1GUIDE";
            grd.Cols["SERVO_ACTV_1"].Caption = "#1SERVO";
            grd.Cols["SCREW_FEED_CURRENT_ACTV_2"].Caption = "#2SCREW전류";
            grd.Cols["GRIND_CURRENT_ACTV_2"].Caption = "#2그라인드모터전류";
            grd.Cols["STOPPER_ACTV_2"].Caption = "#2STOPPER";
            grd.Cols["GUIDE_ACTV_2"].Caption = "#2GUIDE";
            grd.Cols["SERVO_ACTV_2"].Caption = "#2SERVO";

            grd.Cols["L_NO"].Width = cs.L_No_Width;
            grd.Cols["WORK_DDTT"].Width=cs.Date_14_width;
            grd.Cols["SCREW_FEED_CURRENT_ACTV_1"].Width = cs.Current_Value_Width;
            grd.Cols["GRIND_CURRENT_ACTV_1"].Width = cs.Current_Value_Width;
            grd.Cols["STOPPER_ACTV_1"].Width = cs.Stopper_Value_Width;
            grd.Cols["GUIDE_ACTV_1"].Width = cs.Guide_Value_Width;
            grd.Cols["SERVO_ACTV_1"].Width = cs.Servo_Value_Width;
            grd.Cols["SCREW_FEED_CURRENT_ACTV_2"].Width = cs.Current_Value_Width;
            grd.Cols["GRIND_CURRENT_ACTV_2"].Width = cs.Current_Value_Width;
            grd.Cols["STOPPER_ACTV_2"].Width = cs.Stopper_Value_Width;
            grd.Cols["GUIDE_ACTV_2"].Width = cs.Guide_Value_Width;
            grd.Cols["SERVO_ACTV_2"].Width = cs.Servo_Value_Width;


            grd.Rows[0].TextAlign=TextAlignEnum.CenterCenter;

            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["WORK_DDTT"].TextAlign=cs.DATE_TextAlign;
            grd.Cols["SCREW_FEED_CURRENT_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["GRIND_CURRENT_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["STOPPER_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["GUIDE_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["SERVO_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["SCREW_FEED_CURRENT_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["GRIND_CURRENT_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["STOPPER_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["GUIDE_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["SERVO_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;

            grd.Cols.Frozen = grd.Cols["WORK_DDTT"].Index + 1;

        }

        private void InitGrd_CHF1(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search(grd);
            grd.AllowEditing = false;

            grd.Cols["L_NO"].Caption = "NO";
            grd.Cols["WORK_DDTT"].Caption = "작업시각";
            grd.Cols["SERVO_ACTV_1"].Caption = "#1SERVO";
            grd.Cols["SERVO_ACTV_2"].Caption = "#2SERVO";
            grd.Cols["SCREW_ACTV_1"].Caption = "#1SCREW";
            grd.Cols["SCREW_ACTV_2"].Caption = "#2SCREW";
            grd.Cols["RT_ACTV_1"].Caption = "#1R/T";
            grd.Cols["RT_ACTV_2"].Caption = "#2R/T";
            grd.Cols["RT_ACTV_EXIT"].Caption = "ExitR/T";

            grd.Cols["L_NO"].Width = cs.L_No_Width;
            grd.Cols["WORK_DDTT"].Width = cs.Date_14_width;
            int nwidth = (1000 - cs.Date_14_width) / 5;
            
            grd.Cols["SERVO_ACTV_1"].Width = nwidth;
            grd.Cols["SERVO_ACTV_2"].Width = nwidth;
            grd.Cols["SCREW_ACTV_1"].Width = nwidth;
            grd.Cols["SCREW_ACTV_2"].Width = nwidth;
            grd.Cols["RT_ACTV_1"].Width = nwidth;
            grd.Cols["RT_ACTV_2"].Width = nwidth;
            grd.Cols["RT_ACTV_EXIT"].Width = nwidth;

            grd.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["WORK_DDTT"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["SERVO_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["SERVO_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["SCREW_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["SCREW_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["RT_ACTV_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["RT_ACTV_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["RT_ACTV_EXIT"].TextAlign = cs.Angle_Value_TextAlign;


            grd.Cols.Frozen = grd.Cols["WORK_DDTT"].Index +1;

        }

        private void InitGrd_STR1(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search(grd);

            grd.AllowEditing = false;
           
            #region 테스트..
           
            grd.Cols["L_NO"].Caption = "NO";
            grd.Cols["WORK_DDTT"].Caption = "작업시각";
            grd.Cols["TOP_ROLL_GAP_1"].Caption = "상롤GAP1";
            grd.Cols["TOP_ROLL_GAP_2"].Caption = "상롤GAP2";
            grd.Cols["TOP_ROLL_GAP_3"].Caption = "상롤GAP3";
            grd.Cols["TOP_ROLL_GAP_4"].Caption = "상롤GAP4";
            grd.Cols["TOP_ROLL_GAP_5"].Caption = "상롤GAP5";
            grd.Cols["TOP_ROLL_GAP_6"].Caption = "상롤GAP6";
            grd.Cols["TOP_ROLL_ANGLE_1"].Caption = "상롤각도1";
            grd.Cols["TOP_ROLL_ANGLE_2"].Caption = "상롤각도2";
            grd.Cols["TOP_ROLL_ANGLE_3"].Caption = "상롤각도3";
            grd.Cols["TOP_ROLL_ANGLE_4"].Caption = "상롤각도4";
            grd.Cols["TOP_ROLL_ANGLE_5"].Caption = "상롤각도5";
            grd.Cols["TOP_ROLL_ANGLE_6"].Caption = "상롤각도6";
            grd.Cols["BOT_ROLL_ANGLE_1"].Caption = "하롤각도1";
            grd.Cols["BOT_ROLL_ANGLE_2"].Caption = "하롤각도2";
            grd.Cols["BOT_ROLL_ANGLE_3"].Caption = "하롤각도3";
            grd.Cols["THREAD_SPEED"].Caption = "스래드스피드";
            grd.Cols["MACHINE_SPEED"].Caption = "머신스피드";
            grd.Cols["INLET_ROLL_CURRENT"].Caption = "Inlet롤전류";
            grd.Cols["MID_ROLL_CURRENT"].Caption = "Mid롤전류";
            grd.Cols["OUTLET_ROLL_CURRENT"].Caption = "Outlet롤전류";

            grd.Cols["L_NO"].Width = cs.L_No_Width;
            grd.Cols["WORK_DDTT"].Width = cs.Date_14_width;

            grd.Cols["TOP_ROLL_GAP_1"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_GAP_2"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_GAP_3"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_GAP_4"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_GAP_5"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_GAP_6"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_ANGLE_1"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_ANGLE_2"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_ANGLE_3"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_ANGLE_4"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_ANGLE_5"].Width = cs.Angle_Value_Width;
            grd.Cols["TOP_ROLL_ANGLE_6"].Width = cs.Angle_Value_Width;
            grd.Cols["BOT_ROLL_ANGLE_1"].Width = cs.Angle_Value_Width;
            grd.Cols["BOT_ROLL_ANGLE_2"].Width = cs.Angle_Value_Width;
            grd.Cols["BOT_ROLL_ANGLE_3"].Width = cs.Angle_Value_Width;
            grd.Cols["THREAD_SPEED"].Width = cs.Speed_Value_Width;
            grd.Cols["MACHINE_SPEED"].Width = cs.Speed_Value_Width;
            grd.Cols["INLET_ROLL_CURRENT"].Width = cs.Current_Value_Width;
            grd.Cols["MID_ROLL_CURRENT"].Width = cs.Current_Value_Width;
            grd.Cols["OUTLET_ROLL_CURRENT"].Width = cs.Current_Value_Width;

            grd.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["WORK_DDTT"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["TOP_ROLL_GAP_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_GAP_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_GAP_3"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_GAP_4"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_GAP_5"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_GAP_6"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_ANGLE_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_ANGLE_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_ANGLE_3"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_ANGLE_4"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_ANGLE_5"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_ANGLE_6"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["BOT_ROLL_ANGLE_1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["BOT_ROLL_ANGLE_2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["BOT_ROLL_ANGLE_3"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["THREAD_SPEED"].TextAlign = cs.Speed_Value_TextAlign;
            grd.Cols["MACHINE_SPEED"].TextAlign = cs.Speed_Value_TextAlign;
            grd.Cols["INLET_ROLL_CURRENT"].TextAlign = cs.Current_Value_TextAlign;
            grd.Cols["MID_ROLL_CURRENT"].TextAlign = cs.Current_Value_TextAlign;
            grd.Cols["OUTLET_ROLL_CURRENT"].TextAlign = cs.Current_Value_TextAlign;
            
            #endregion

            grd.Cols.Frozen = grd.Cols["WORK_DDTT"].Index + 1;
        }

        private void InitGrd_STR_com(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search(grd);

            grd.AllowEditing = false;
            grd.Cols["L_NO"].Caption = "NO";
            grd.Cols["WORK_DDTT"].Caption = "작업시각";
            grd.Cols["TOP_ROLL_ANGLE_ACTV"].Caption = "상롤각도";
            grd.Cols["BOT_ROLL_ANGLE_ACTV"].Caption = "하롤각도";
            grd.Cols["TOP_ROLL_MTR_RPM_ACTV"].Caption = "상롤모터RPM ";
            grd.Cols["TOP_ROLL_MTR_CURRENT_ACTV"].Caption = "상롤모터전류";
            grd.Cols["BOT_ROLL_MTR_RPM_ACTV"].Caption = "하롤모터RPM ";
            grd.Cols["BOT_ROLL_MTR_CURRENT_ACTV"].Caption = "하롤모터전류";

            grd.Cols["L_NO"].Width = cs.L_No_Width;
            grd.Cols["WORK_DDTT"].Width = cs.Date_14_width;
            int nwidth = (1000 - cs.Date_14_width)/5;

            grd.Cols["TOP_ROLL_ANGLE_ACTV"].Width = nwidth;
            grd.Cols["BOT_ROLL_ANGLE_ACTV"].Width = nwidth;
            grd.Cols["TOP_ROLL_MTR_RPM_ACTV"].Width = nwidth;
            grd.Cols["TOP_ROLL_MTR_CURRENT_ACTV"].Width = nwidth;
            grd.Cols["BOT_ROLL_MTR_RPM_ACTV"].Width = nwidth;
            grd.Cols["BOT_ROLL_MTR_CURRENT_ACTV"].Width = nwidth;

            grd.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["WORK_DDTT"].TextAlign = cs.DATE_TextAlign;

            grd.Cols["TOP_ROLL_ANGLE_ACTV"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["BOT_ROLL_ANGLE_ACTV"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["TOP_ROLL_MTR_RPM_ACTV"].TextAlign = cs.Speed_Value_TextAlign;
            grd.Cols["TOP_ROLL_MTR_CURRENT_ACTV"].TextAlign = cs.Current_Value_TextAlign;
            grd.Cols["BOT_ROLL_MTR_RPM_ACTV"].TextAlign = cs.Current_Value_TextAlign;
            grd.Cols["BOT_ROLL_MTR_CURRENT_ACTV"].TextAlign = cs.Current_Value_TextAlign;

            grd.Cols.Frozen = grd.Cols["WORK_DDTT"].Index + 1;
        }
        #endregion Init Grid 설정

        #region 조회 설정
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            cd.InsertLogForSearch(ck.UserID, btnDisplay);
            //검색조건에 따라 데이터 기준을 만들고 그에 따른 표시 기준을 만들어 표시한다.
            //선택조건에 따른 분류해야함
            string selectedTableNM = string.Empty;
            selectedGrd = new C1FlexGrid();

            if (uC_Line_gp_s1.Line_GP == "#1")
            {
                // 1라인
                switch (uC_Routing_cd1.Routing_cd)
                {
                    case "A1"://교정
                        selectedTableNM = "TB_STR_OPERINFO_NO1";
                        selectedGrd = grdSTR1;
                        moddt_grdSTR1 = new DataTable();
                        ShowDataSTR1(selectedTableNM, selectedGrd, moddt_grdSTR1);
                        break;

                    case "B1"://면취
                        selectedTableNM = "TB_CHF_OPERINFO_NO1";
                        selectedGrd = grdCHF1;
                        moddt_grdCHF1 = new DataTable();
                        ShowDataCHF1(selectedTableNM, selectedGrd, moddt_grdCHF1);
                        break;

                    default:
                        break;
                }
            }
            else if (uC_Line_gp_s1.Line_GP == "#2")
            {
                // 2라인
                switch (uC_Routing_cd1.Routing_cd)
                {
                    case "A1"://교정
                        selectedTableNM = "TB_STR_OPERINFO_NO2";
                        selectedGrd = grdSTR2;
                        moddt_grdSTR2 = new DataTable();
                        ShowDataSTR_com(selectedTableNM, selectedGrd, moddt_grdSTR2);
                        break;

                    case "B1"://면취
                        selectedTableNM = "TB_CHF_OPERINFO_NO2";
                        selectedGrd = grdCHF2;
                        moddt_grdCHF2 = new DataTable();
                        ShowDataCHF_com(selectedTableNM, selectedGrd, moddt_grdCHF2);
                        break;

                    default:
                        break;
                }
            }
            else if (uC_Line_gp_s1.Line_GP == "#3")
            {
                // 3라인
                switch (uC_Routing_cd1.Routing_cd)
                {
                    case "A1"://교정
                        selectedTableNM = "TB_STR_OPERINFO_NO3";
                        selectedGrd = grdSTR3;
                        moddt_grdSTR3 = new DataTable();
                        ShowDataSTR_com(selectedTableNM, selectedGrd, moddt_grdSTR3);
                        break;

                    case "B1"://면취
                        selectedTableNM = "TB_CHF_OPERINFO_NO3";
                        selectedGrd = grdCHF3;
                        moddt_grdCHF3 = new DataTable();
                        ShowDataCHF_com(selectedTableNM, selectedGrd, moddt_grdCHF3);
                        break;

                    default:
                        break;
                }
            }
            // 모든 그리드를 숨기고 해당 그리드만 docking 시킴
            selectedGrd.Dock = DockStyle.Fill;
            selectedGrd.BringToFront();

            IsChangeLineOrRoutingOrCheckerInGrid = false;
        }

        private void ShowDataCHF_com(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {
            sql1 = string.Empty;
            sql1 += string.Format("SELECT ");
            sql1 += string.Format("    ROWNUM AS L_NO ");
            sql1 += string.Format("    ,TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 14), 'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH:MI:SS') AS WORK_DDTT ");
            //sql1 += string.Format("   ,MILL_NO ");
            //sql1 += string.Format("   ,WORK_TYPE ");
            //sql1 += string.Format("   ,WORK_TEAM ");
            //sql1 += string.Format("   ,KICKER_SIZE_ACTV_1 ");
            //sql1 += string.Format("   ,KICKER_SIZE_SETV_1 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_ACTV_1 ");
            //sql1 += string.Format("   ,ROLLER_CV_CURRENT_ACTV_1 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_H_SETV_1 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_S_T_SETV_1 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_S_SETV_1 ");
            //sql1 += string.Format("   ,SCREW_FEED_HZ_ACTV_1 ");
            sql1 += string.Format("   ,SCREW_FEED_CURRENT_ACTV_1 ");
            //sql1 += string.Format("   ,SCREW_FEED_HZ_SETV_1 ");
            //sql1 += string.Format("   ,GRIND_HZ_ACTV_1 ");
            sql1 += string.Format("   ,GRIND_CURRENT_ACTV_1 ");
            //sql1 += string.Format("   ,GRIND_HZ_SETV_1 ");
            sql1 += string.Format("   ,STOPPER_ACTV_1 ");
            //sql1 += string.Format("   ,STOPPER_SETV_1 ");
            sql1 += string.Format("   ,GUIDE_ACTV_1 ");
            //sql1 += string.Format("   ,GUIDE_SETV_1 ");
            sql1 += string.Format("   ,SERVO_ACTV_1 ");
            //sql1 += string.Format("   ,SERVO_JOG_UP_SP_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_JOG_DOWN_SP_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_UP_SP_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_UP_POS_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_CH_POS_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_SP_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_POS_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_POS_IN_SP_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_POS_IN_POS_SETV_1 ");
            //sql1 += string.Format("   ,KICKER_SIZE_ACTV_2 ");
            //sql1 += string.Format("   ,KICKER_SIZE_SETV_2 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_ACTV_2 ");
            //sql1 += string.Format("   ,ROLLER_CV_CURRENT_ACTV_2 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_H_SETV_2 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_S_T_SETV_2 ");
            //sql1 += string.Format("   ,ROLLER_CV_HZ_S_SETV_2 ");
            //sql1 += string.Format("   ,SCREW_FEED_HZ_ACTV_2 ");
            sql1 += string.Format("   ,SCREW_FEED_CURRENT_ACTV_2 ");
            //sql1 += string.Format("   ,SCREW_FEED_HZ_SETV_2 ");
            //sql1 += string.Format("   ,GRIND_HZ_ACTV_2 ");
            sql1 += string.Format("   ,GRIND_CURRENT_ACTV_2 ");
            //sql1 += string.Format("   ,GRIND_HZ_SETV_2 ");
            sql1 += string.Format("   ,STOPPER_ACTV_2 ");
            //sql1 += string.Format("   ,STOPPER_SETV_2 ");
            sql1 += string.Format("   ,GUIDE_ACTV_2 ");
            //sql1 += string.Format("   ,GUIDE_SETV_2 ");
            sql1 += string.Format("   ,SERVO_ACTV_2 ");
            //sql1 += string.Format("   ,SERVO_JOG_UP_SP_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_JOG_DOWN_SP_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_UP_SP_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_UP_POS_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_CH_POS_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_SP_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_POS_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_POS_IN_SP_SETV_2 ");
            //sql1 += string.Format("   ,SERVO_POS_IN_POS_SETV_2 ");
            //sql1 += string.Format("   ,EXIT_KICKER_SIZE_ACTV ");
            //sql1 += string.Format("   ,EXIT_KICKER_SIZE_SETV ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("WHERE ");
            sql1 += string.Format("  WORK_DDTT BETWEEN :P_FROM_DATE AND :P_TO_DATE ");

            string[] parm = new string[2];
            parm[0] = ":P_FROM_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_From_Date, "yyyyMMddHH0000");
            parm[1] = ":P_TO_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_To_Date, "yyyyMMddHH0000");


            dt = cd.FindDataTable(sql1, parm);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grd.SetDataBinding(dt, null, true);

            chart_dt = dt.Copy();
            chart_dt.Columns.Remove("WORK_DDTT");
            chart_dt.Columns.Remove("L_NO");

            SetChart(chart_dt, grd, chart1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + dt.Rows.Count.ToString() + " 건 조회 되었습니다.");
        }

        private void ShowDataCHF1(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {
            sql1 = string.Empty;
            sql1 += string.Format("SELECT ");
            sql1 += string.Format("    ROWNUM AS L_NO ");
            sql1 += string.Format("    ,TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 14), 'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH:MI:SS') AS WORK_DDTT ");
            //sql1 += string.Format("   ,WORK_TYPE ");
            //sql1 += string.Format("   ,WORK_TEAM ");
            sql1 += string.Format("   ,SERVO_ACTV_1 ");
            //sql1 += string.Format("   ,SERVO_SETV_1 ");
            //sql1 += string.Format("   ,SERVO_SETV_2 ");
            sql1 += string.Format("   ,SERVO_ACTV_2 ");
            sql1 += string.Format("   ,SCREW_ACTV_1 ");
            //sql1 += string.Format("   ,SCREW_SETV_1 ");
            sql1 += string.Format("   ,SCREW_ACTV_2 ");
            //sql1 += string.Format("   ,SCREW_SETV_2 ");
            sql1 += string.Format("   ,RT_ACTV_1 ");
            //sql1 += string.Format("   ,RT_SETV_1 ");
            sql1 += string.Format("   ,RT_ACTV_2 ");
            //sql1 += string.Format("   ,RT_SETV_2 ");
            sql1 += string.Format("   ,RT_ACTV_EXIT ");
            //sql1 += string.Format("   ,RT_SETV_EXIT ");
            //sql1 += string.Format("   ,STOPPER_1 ");
            //sql1 += string.Format("   ,PROP_1 ");
            //sql1 += string.Format("   ,MATERIAL_GAP_1 ");
            //sql1 += string.Format("   ,STOPPER_2 ");
            //sql1 += string.Format("   ,PROP_2 ");
            //sql1 += string.Format("   ,MATERIAL_GAP_2 ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("WHERE ");
            sql1 += string.Format("  WORK_DDTT BETWEEN :P_FROM_DATE AND :P_TO_DATE ");

            string[] parm = new string[2];
            parm[0] = ":P_FROM_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_From_Date, "yyyyMMddHH0000");
            parm[1] = ":P_TO_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_To_Date, "yyyyMMddHH0000");


            dt = cd.FindDataTable(sql1, parm);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grd.SetDataBinding(dt, null, true);

            chart_dt = dt.Copy();
            chart_dt.Columns.Remove("WORK_DDTT");
            chart_dt.Columns.Remove("L_NO");

            SetChart(chart_dt, grd, chart1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + dt.Rows.Count.ToString() + " 건 조회 되었습니다.");
        }

        private void ShowDataSTR_com(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format("SELECT ");
            sql1 += string.Format("    ROWNUM AS L_NO ");
            sql1 += string.Format("    ,TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 14), 'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH:MI:SS') AS WORK_DDTT ");
            //sql1 += string.Format("   ,WORK_TYPE ");
            //sql1 += string.Format("   ,WORK_TEAM ");
            //sql1 += string.Format("   ,MILL_NO ");
            //sql1 += string.Format("   ,TOP_ROLL_CUSH_ACTV ");
            //sql1 += string.Format("   ,TOP_ROLL_POS_ACTV ");
            //sql1 += string.Format("   ,TOP_ROLL_POS_SETV ");
            sql1 += string.Format("   ,TOP_ROLL_ANGLE_ACTV ");
            //sql1 += string.Format("   ,TOP_ROLL_ANGLE_SETV ");
            sql1 += string.Format("   ,BOT_ROLL_ANGLE_ACTV ");
            //sql1 += string.Format("   ,BOT_ROLL_ANGLE_SETV ");
            //sql1 += string.Format("   ,UP_DOWN_LINEAR_POS_ACTV ");
            //sql1 += string.Format("   ,UP_DOWN_LINEAR_POS_SETV ");
            //sql1 += string.Format("   ,FRONT_LINEAR_POS_ACTV ");
            //sql1 += string.Format("   ,FRONT_LINEAR_POS_SETV ");
            //sql1 += string.Format("   ,REAL_LINEAR_POS_ACTV ");
            //sql1 += string.Format("   ,REAL_LINEAR_POS_SETV ");
            sql1 += string.Format("   ,TOP_ROLL_MTR_RPM_ACTV ");
            //sql1 += string.Format("   ,TOP_ROLL_MTR_HZ_SETV ");
            sql1 += string.Format("   ,TOP_ROLL_MTR_CURRENT_ACTV ");
            sql1 += string.Format("   ,BOT_ROLL_MTR_RPM_ACTV ");
            //sql1 += string.Format("   ,BOT_ROLL_MTR_HZ_SETV ");
            sql1 += string.Format("   ,BOT_ROLL_MTR_CURRENT_ACTV ");
            //sql1 += string.Format("   ,IN_ROLLDR_HZ_ACTV ");
            //sql1 += string.Format("   ,IN_ROLLDR_HZ_SETV ");
            //sql1 += string.Format("   ,IN_ROLLDR_CURRENT_ACTV ");
            //sql1 += string.Format("   ,ENTRY_ROLLDR_HZ_ACTV ");
            //sql1 += string.Format("   ,ENTRY_ROLLDR_HZ_SETV ");
            //sql1 += string.Format("   ,ENTRY_ROLLDR_CURRENT_ACTV ");
            //sql1 += string.Format("   ,EXIT_ROLLDR_HZ_ACTV ");
            //sql1 += string.Format("   ,EXIT_ROLLDR_HZ_SETV ");
            //sql1 += string.Format("   ,EXIT_ROLLDR_CURRENT_ACTV ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("WHERE ");
            sql1 += string.Format("  WORK_DDTT BETWEEN :P_FROM_DATE AND :P_TO_DATE ");
            //sql1 += string.Format("   WORK_DDTT BETWEEN TO_CHAR(:P_FROM_DATE, 'yyyyMMddHH24') AND TO_CHAR(:P_TO_DATE , 'yyyyMMddHH24')");
           

            string[] parm = new string[2];
            parm[0] = ":P_FROM_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_From_Date, "yyyyMMddHH0000");
            parm[1] = ":P_TO_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_To_Date, "yyyyMMddHH0000");


            dt = cd.FindDataTable(sql1, parm);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grd.SetDataBinding(dt, null, true);

            chart_dt = dt.Copy();
            chart_dt.Columns.Remove("WORK_DDTT");
            chart_dt.Columns.Remove("L_NO");

            SetChart(chart_dt, grd, chart1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + dt.Rows.Count.ToString() + " 건 조회 되었습니다.");
        }

        private void ShowDataSTR1(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {
            sql1 = string.Empty;
            sql1 += string.Format("SELECT ");
            sql1 += string.Format("    ROWNUM AS L_NO ");
            //sql1 += string.Format("    TO_DATE(WORK_DDTT) AS WORK_DDTT ");
            sql1 += string.Format("    ,TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 14), 'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH:MI:SS') AS WORK_DDTT ");
            //sql1 += string.Format("   ,MILL_NO ");
            //sql1 += string.Format("   ,WORK_TYPE ");
            //sql1 += string.Format("   ,WORK_TEAM ");
            sql1 += string.Format("   ,TOP_ROLL_GAP_1 ");
            sql1 += string.Format("   ,TOP_ROLL_GAP_2 ");
            sql1 += string.Format("   ,TOP_ROLL_GAP_3 ");
            sql1 += string.Format("   ,TOP_ROLL_GAP_4 ");
            sql1 += string.Format("   ,TOP_ROLL_GAP_5 ");
            sql1 += string.Format("   ,TOP_ROLL_GAP_6 ");
            sql1 += string.Format("   ,TOP_ROLL_ANGLE_1 ");
            sql1 += string.Format("   ,TOP_ROLL_ANGLE_2 ");
            sql1 += string.Format("   ,TOP_ROLL_ANGLE_3 ");
            sql1 += string.Format("   ,TOP_ROLL_ANGLE_4 ");
            sql1 += string.Format("   ,TOP_ROLL_ANGLE_5 ");
            sql1 += string.Format("   ,TOP_ROLL_ANGLE_6 ");
            sql1 += string.Format("   ,BOT_ROLL_ANGLE_1 ");
            sql1 += string.Format("   ,BOT_ROLL_ANGLE_2 ");
            sql1 += string.Format("   ,BOT_ROLL_ANGLE_3 ");
            sql1 += string.Format("   ,THREAD_SPEED ");
            sql1 += string.Format("   ,MACHINE_SPEED ");
            sql1 += string.Format("   ,INLET_ROLL_CURRENT ");
            sql1 += string.Format("   ,MID_ROLL_CURRENT ");
            sql1 += string.Format("   ,OUTLET_ROLL_CURRENT ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("WHERE ");
            sql1 += string.Format("  WORK_DDTT BETWEEN :P_FROM_DATE AND :P_TO_DATE ");

            string[] parm = new string[2];
            parm[0] = ":P_FROM_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_From_Date, "yyyyMMddHH0000");
            parm[1] = ":P_TO_DATE|" + vf.Format(uC_Work_Date_Fr_To1.Work_To_Date, "yyyyMMddHH0000");

            dt = cd.FindDataTable(sql1, parm);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grd.SetDataBinding(dt, null, true);

            //L_NO 추가 하고 WORK_DDTT column 제거후 chart1에 databinding

            chart_dt = dt.Copy();
            chart_dt.Columns.Remove("WORK_DDTT");
            chart_dt.Columns.Remove("L_NO");

            SetChart(chart_dt, grd, chart1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + dt.Rows.Count.ToString() + " 건 조회 되었습니다.");
        }
        #endregion 조회 설정

        #region chart 설정
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Chartdt"> 차트 정보를 가지고 있는 테이블 grd 의 일부 컬럼을 제외한 정보를 가지고 있슴</param>
        /// <param name="grd"> 데이터를 가지고 있는 메인 그리드</param>
        /// <param name="chart">메인 차트</param>
        private void SetChart(DataTable Chartdt, C1FlexGrid grd, Chart chart)
        {
            //차트 초기화
            InitChart();
            //차트 데이터 바인딩
            ChartBindData(Chartdt, chart);

            //선택 수정 혹은 조회 조건 수정이 있는경우 checkgrid 초기화
            if (IsChangeLineOrRoutingOrCheckerInGrid)
            {
                InitCheckerGrid(Chartdt, chart, grd);
            }

            // 조회된 정보를 가진 주 그리드의 데이터로 부터 check grid 설정 및 chart label 설정을한다.

            //chart label 설정을한다.
            SetUpChartSeriesText(Chartdt, chart, grd);

            grdCheck.SetDataBinding(checker_dt, null, true);

            #region 임시 스타일을 저장
            var tempStyle = new CellStyle[30];
            string[] tempStyle_name = new string[30];

            for (int itemCnt = 0; itemCnt < 30; itemCnt++)
            {
                tempStyle_name[itemCnt] = itemCnt.ToString();
                tempStyle[itemCnt] = grdCheck.Styles.Add(tempStyle_name[itemCnt]);
            }
            #endregion

            // series color 를 읽어서 grid forcolor를 정의함
            var tempStyle11 = grdCheck.Styles.Add("TempStyle");
            var crCellRange = new CellRange();
            //bool visiable;
            for (int row = 0; row < checker_dt.Rows.Count; row++)
            {

                //visiable = (checker_dt.Rows[row]["CHECKER"].ToString() == "True");

                foreach (var series in chart.Series)
                {
                    //마커 크기
                    ((PointAttributes)series).MarkerShape = MarkerShape.None;
                    //선 굵기
                    ((PointAttributes)series).Line.Width = 2;
                    ((PointAttributes)series).Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;

                    if (((PointAttributes)series).Text == grdCheck.GetData(row + 1, "ITEM").ToString())
                    {
                        tempStyle[row].ForeColor = ((PointAttributes)series).Color;

                        crCellRange = grdCheck.GetCellRange(row + 1, grdCheck.Cols["ITEM"].Index);
                        crCellRange.Style = grdCheck.Styles[row.ToString()];
                    }
                }

            }

            //chart1.RecalculateScale();// .RecalculateAxesScale();

            UpdateChart(checker_dt, chart1);
            
            //RealizeMaxMin();
            

        }

        private void SetDataTableMinMax(DataTable _checker_dt, C1FlexGrid _selectedGrd)
        {
            //선택된 colsName 리스트를 가지고
            //선택된 그리드에서 해당 colsName의 min, max를 계산해서 
            //최종 min,max를 적용한다.
            List<string> selecteditem = new List<string>();
            foreach (DataRow row in _checker_dt.Rows)
            {
                if (row["CHECKER"].ToString() == "True")
                {
                    selecteditem.Add(row["ITEM_GUBUN"].ToString());
                }
                
            }

            double minValue = vf.CDbl(int.MaxValue);
            double maxValue = vf.CDbl(int.MinValue);

            DataTable dt = (DataTable)_selectedGrd.DataSource;
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            if (selecteditem.Count == 0)
            {
                chart1.AxisY.Min = -1.0;
                chart1.AxisY.Max = 1.0;
                return;
            }

            foreach (var item in selecteditem)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    double value = vf.CDbl(dr.Field<object>(item));
                    minValue = Math.Min(minValue, value);
                    maxValue = Math.Max(maxValue, value);

                }
            }

            double meanValue = Math.Abs((maxValue - minValue)) / 2;
            double marginValue = meanValue / 5;

            if (minValue.Equals(maxValue))
            {

                if (minValue.Equals(0.0))
                {
                    chart1.AxisY.Min = -1.0;
                    chart1.AxisY.Max = 1.0;
                    return;
                }

                chart1.AxisY.Min = minValue * 0.8;
                chart1.AxisY.Max = maxValue * 1.2;
                return;
            }
 

            chart1.AxisY.Min = minValue - marginValue;
            chart1.AxisY.Max = maxValue + marginValue;

        }

        /// <summary>
        /// checker_dt 새로 생성 및 초기화
        /// </summary>
        /// <param name="_chartdt"> 차트 정보를 가지고 있는 테이블 grd 의 일부 컬럼을 제외한 정보를 가지고 있슴</param>
        /// <param name="_grd"> 데이터를 가지고 있는 메인 그리드</param>
        /// <param name="_chart">메인 차트</param>
        private void InitCheckerGrid(DataTable _chartdt, Chart _chart, C1FlexGrid _grd)
        {
            checker_dt = new DataTable();
            checker_dt.Columns.Add("CHECKER", typeof(Boolean));
            checker_dt.Columns.Add("ITEM", typeof(string));
            checker_dt.Columns.Add("ITEM_GUBUN", typeof(string));

            foreach (DataColumn col in _chartdt.Columns)
            {

                if (_grd.Cols[col.ColumnName].Width > 0) // 화면에 보이는 항목
                {
                    // 작업시각은 제외
                    // dt의 column 21
                    // chart.series.count 20 + 1(L_NO)
                    if (col.ColumnName == "L_NO" || col.ColumnName == "WORK_DDTT") // L_NO + WORK_DDTT
                    {
                        continue;
                    }


                    // 선택항목 테이블 만들기
                    checker_dt.Rows.Add(true, _grd.Cols[col.ColumnName].Caption, col.ColumnName);
                }
            }
        }

        /// <summary>
        /// chart1 의 시리즈 각 Text를 메인그리드의 컬럼명으로 지정
        /// </summary>
        /// <param name="_chartdt"> 차트 정보를 가지고 있는 테이블 grd 의 일부 컬럼을 제외한 정보를 가지고 있슴</param>
        /// <param name="_grd"> 데이터를 가지고 있는 메인 그리드</param>
        /// <param name="_chart">메인 차트</param>
        private void SetUpChartSeriesText(DataTable _chartdt, Chart _chart, C1FlexGrid _grd)
        {
            foreach (DataColumn col in _chartdt.Columns)
            {

                if (_grd.Cols[col.ColumnName].Width > 0) // 화면에 보이는 항목
                {
                    // 작업시각은 제외
                    // dt의 column 21
                    // chart.series.count 20 + 1(L_NO)
                    if (col.ColumnName == "L_NO" || col.ColumnName == "WORK_DDTT") // L_NO + WORK_DDTT
                    {
                        continue;
                    }

                    // grd.cols[].index 1 이면 chart.series  순번은 0   여기서 2는 L_NO와 WORK_DDTT둘을 뺀것.
                    _chart.Series[_grd.Cols[col.ColumnName].Index - 2].Text = _grd.Cols[col.ColumnName].Caption;

                }
            }
        }

        private static void ChartBindData(DataTable Chartdt, Chart chart)
        {
            chart.Data.Series = Chartdt.Columns.Count;
            chart.DataSource = Chartdt;
        }
        #endregion chart 설정

        #region 이벤트 설정
        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, selectedGrd);
        }

        private void grdCheck_CellChecked(object sender, RowColEventArgs e)
        {

            UpdateChart(checker_dt, chart1);

            if (IsChangeLineOrRoutingOrCheckerInGrid)
            {
                IsChangeLineOrRoutingOrCheckerInGrid = true;
            }
        }

        private void UpdateChart(DataTable _checker_dt, Chart _chart1)
        {

            for (int row = 0; row < _checker_dt.Rows.Count; row++)
            {
                //var line = from series in chart1.Series.Contains()

                foreach (var series in _chart1.Series)
                {
                    if (((PointAttributes)series).Text == _checker_dt.Rows[row]["ITEM"].ToString())// .Columns["ITEM"].ToString())
                    {
                        ((SeriesAttributes)series).Visible = (_checker_dt.Rows[row]["CHECKER"].ToString() == "True");
                        //continue;
                    }

                }
            }
            SetDataTableMinMax(checker_dt, selectedGrd);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion 이벤트 설정

        private void chart1_Click(object sender, EventArgs e)
        {
            


        }

        private void chart1_MouseClick(object sender, HitTestEventArgs e)
        {
            Int32 nXPos;
            //Int32 nYPos;

            if (chart1.Series.Count == 0)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                //chart_line_point = this.Size.Width - ChartPanel.Size.Width + 1;
                nXPos = decimal.ToInt32(decimal.Round(decimal.Parse(chart1.Series[0].AxisX.PixelToValue(e.X).ToString()), 0));

                //MessageBox.Show(nXPos.ToString());
                if (nXPos > 0 && nXPos < selectedGrd.Rows.Count)
                {
                    selectedGrd.Row = nXPos;
                }
                
            }
        }

        private void grdCheck_Paint(object sender, PaintEventArgs e)
        {
            foreach (Order.CrtInOrdCre.HostedControl hosted in _al)
                hosted.UpdatePosition();
        }
    }
}
