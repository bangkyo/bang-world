using C1.Win.C1FlexGrid;
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
    public partial class LineMlftGrfNew : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();

        DataTable olddt;

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        clsStyle cs = new clsStyle();

        private PictureScreen PictureScreen = new PictureScreen();

        private string start_dt_str = "";
        private string end_dt_str = "";

        private int start_dt_str1 = 0;
        private int end_dt_str1 = 0;

        private string HEAT_id = "";

        private string POC_id = "";

        private string count_id = "";

        private string PIECE_id = "";
        private string WORK_id = "";
        private string date_id = "";

        UC_OKNG1 uC_OKNG1;

        private string ownerNM = "";
        private string titleNM = "";

        private DateTimePicker dtpTime;

        string sql1 = string.Empty;
        C1FlexGrid selectedGrd;
        C1FlexGrid selectedGrd1;

        private UC_Line_gp_s uC_Line_gp_s1;
        private UC_Routing_cd uC_Routing_cd1;
        //private UC_Work_Date_Fr_To uC_Work_Date_Fr_To1;

        DataTable moddt_grdSTR1;
        DataTable moddt_grdSTR2;
        DataTable moddt_grdSTR3;
        DataTable moddt_grdCHF1;
        DataTable moddt_grdCHF2;
        DataTable moddt_grdCHF3;

        DataTable checker_dt;

        DataTable chart_dt1;
        DataTable chart_dt2;

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

        public LineMlftGrfNew(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void LineMlftGrfNew_Load(object sender, EventArgs e)
        {
            InitControl();

            //SetComboBox2();
            


            //grdCheck1_BeforeEdit(null, null);
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

            cs.InitCombo(cboHEAT, StringAlignment.Near);

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
            uC_Line_gp_s1.Location = new System.Drawing.Point(26 + location_x, 7 + location_y);
            uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            uC_Line_gp_s1.TabIndex = 1;
            uC_Line_gp_s1.LineChangedEvent += UC_Line_gp_s1_LineChangedEvent;

            // 
            // uC_Routing_cd1
            // 
            //uC_Routing_cd1 = new UC_Routing_cd();
            //uC_Routing_cd1.BackColor = System.Drawing.Color.Transparent;
            //uC_Routing_cd1.cb_Enable = true;
            //uC_Routing_cd1.Location = new System.Drawing.Point(329 + location_x, 8 + location_y);
            //uC_Routing_cd1.Name = "uC_Routing_cd1";
            //uC_Routing_cd1.Routing_cd = "A1";
            //uC_Routing_cd1.Size = new System.Drawing.Size(203, 27);
            //uC_Routing_cd1.TabIndex = 20;
            //uC_Routing_cd1.RoutingChangedEvent += UC_Routing_cd1_RoutingChangedEvent;

            // 
            // uC_Okng
            // 
            uC_OKNG1 = new UC_OKNG1();
            uC_OKNG1.BackColor = System.Drawing.Color.Transparent;
            uC_OKNG1.cb_Enable = true;
            uC_OKNG1.Location = new System.Drawing.Point(329 + location_x, 7 + location_y);
            uC_OKNG1.Name = "uC_OKNG1";
            uC_OKNG1.OKNG = "OK";
            uC_OKNG1.Size = new System.Drawing.Size(202, 27);
            uC_OKNG1.TabIndex = 2;
            //uC_OKNG1.RoutingChangedEvent += UC_Routing_cd1_RoutingChangedEvent;


            //start_dt.ValueChanged += Start_dt_ValueChanged;
            //end_dt.ValueChanged += End_dt_ValueChanged;


            // 
            // uC_Work_Date_Fr_To1
            // 
            //uC_Work_Date_Fr_To1 = new UC_Work_Date_Fr_To();
            //uC_Work_Date_Fr_To1.BackColor = System.Drawing.Color.Transparent;
            //uC_Work_Date_Fr_To1.Location = new System.Drawing.Point(532 + 100 + location_x, 8 + location_y);
            //uC_Work_Date_Fr_To1.Name = "uC_Work_Date_Fr_To1";
            //uC_Work_Date_Fr_To1.TabIndex = 20;

            //uC_Work_Date_Fr_To1.Work_From_Date = DateTime.Now.AddHours(-1);
            //uC_Work_Date_Fr_To1.Work_To_Date = DateTime.Now;

            //panel1.Controls.Add(this.uC_Work_Date_Fr_To1);
            //panel1.Controls.Add(this.uC_Routing_cd1);
            panel1.Controls.Add(this.uC_Line_gp_s1);
            panel1.Controls.Add(this.uC_OKNG1);
            #endregion

            InitGrd();
            InitChart();
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            UpdateChart1();
        }

        private void UC_Routing_cd1_RoutingChangedEvent(object sender, EventArgs e)
        {
            if (!IsChangeLineOrRoutingOrCheckerInGrid)
            {
                IsChangeLineOrRoutingOrCheckerInGrid = true;
            }

            //btnDisplay_Click(null, null);
        }

        private void SetComboBox2()
        {
            string selectedTableNM = string.Empty;
            selectedGrd = new C1FlexGrid();

            if (uC_Line_gp_s1.Line_GP == "#1")
            {
                selectedTableNM = "TB_MLFT_OPERINFO_NO1@MLFT_DBLINK";
            }
            else if (uC_Line_gp_s1.Line_GP == "#2")
            {
                selectedTableNM = "TB_MLFT_OPERINFO_NO2@MLFT_DBLINK";
            }
            else if (uC_Line_gp_s1.Line_GP == "#3")
            {
                selectedTableNM = "TB_MLFT_OPERINFO_NO3@MLFT_DBLINK";
            }

            //sql1 = string.Empty;
            //sql1 = string.Format("SELECT COUNT(*) COUNT   ");
            //sql1 += string.Format("FROM ");
            //sql1 += string.Format("  {0} ", selectedTableNM);
            //sql1 += string.Format(" WHERE SUBSTR(WORK_DDTT, 1, 8)  BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);      //POC_NO
            //                                                                //sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
            //olddt = cd.FindDataTable(sql1);

            //count_id = olddt.Rows[0]["COUNT"].ToString();

            //SetCombo_H(cboHEAT, selectedTableNM, start_dt_str, end_dt_str, false);

            //if (count_id != "0")
            //{
                SetCombo_H(cboHEAT, selectedTableNM, start_dt_str, end_dt_str, false);
           // }
            //else
            //{
            //    //cboHEAT.Items.Clear();
            //    cboHEAT.DataSource = null;
            //    cboHEAT.Items.Clear();
            //}
        }

        

        private void UC_Line_gp_s1_LineChangedEvent(object sender, EventArgs e)
        {
            if (!IsChangeLineOrRoutingOrCheckerInGrid)
            {
                IsChangeLineOrRoutingOrCheckerInGrid = true;
            }

            //SetComboBox2();
            //btnDisplay_Click(null, null);
        }


        private void SetCombo_H(ComboBox cb, string tableNM, string start_dt_str, string end_dt_str, bool isTotalAdd)
        {
            string sql1 = string.Empty;
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                //cb.DataSource = null;
                //cb.Items.Clear();

                sql1 = string.Format("SELECT DISTINCT HEAT FROM {0} WHERE WORK_DDTT BETWEEN '{1}' AND '{2}'", tableNM, start_dt_str+"000000", end_dt_str+"235959");
                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = cd.FindDataTable(sql1);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row["HEAT"].ToString(), row["HEAT"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    //foreach (var item in cb.Items)
                    //{
                    //    if (((DictionaryList)item).fnText == _Init_zone_id)
                    //    {
                    //        cb.SelectedIndex = cb.Items.IndexOf(item);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }
        private void cboHEAT_SelectedIndexChanged(object sender, EventArgs e)
        {
            HEAT_id = ((DictionaryList)cboHEAT.SelectedItem).fnValue;
            string selectedTableNM = string.Empty;
            selectedGrd = new C1FlexGrid();

            ShowDataSTR3();

            if (uC_Line_gp_s1.Line_GP == "#1")
            {
                selectedTableNM = "TB_MLFT_OPERINFO_NO1@MLFT_DBLINK";
                selectedGrd = grdCheck1;
                moddt_grdSTR2 = new DataTable();
                ShowDataSTR2(selectedTableNM, selectedGrd, moddt_grdSTR1);
                
            }
            else if (uC_Line_gp_s1.Line_GP == "#2")
            {
                selectedTableNM = "TB_MLFT_OPERINFO_NO2@MLFT_DBLINK";
                selectedGrd = grdCheck1;
                moddt_grdSTR1 = new DataTable();
                ShowDataSTR2(selectedTableNM, selectedGrd, moddt_grdSTR1);
            }
            else if (uC_Line_gp_s1.Line_GP == "#3")
            {
                selectedTableNM = "TB_MLFT_OPERINFO_NO3@MLFT_DBLINK";
                selectedGrd = grdCheck1;
                moddt_grdSTR1 = new DataTable();
                ShowDataSTR2(selectedTableNM, selectedGrd, moddt_grdSTR1);
            }
        }
        #endregion Init Control 설정

        #region InitChart 설정
        private void InitChart()
        {
            //chart1.Reset();
            chart1.LegendBox.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chart1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

            
            chart1.Dock = DockStyle.Fill;
            chart1.ToolBar.Visible = true;
            chart1.LegendBox.Visible = false;

            chart1.ContextMenus = false; //팝업메뉴 죽임
            chart1.AxisY.AutoScale = true;
            chart1.AxisY.ForceZero = false; // 0 을 최소값으로 두지 않는 옵션.
            chart1.AxisY.DataFormat.Decimals = 1;
            chart1.AxisX.Title.Text = "탐상길이(mm)";
            chart1.AxisY.Title.Text = "전압(V)";
            //chart1.AxisX.Step = 40;
            chart1.AxisX.LabelValue = 13;
            //chart1.AxisX.Grids.Major.Style = so;
            //chart1.AxisX.ScaleUnit = 3;
            //chart1.AxisY.Line.Width = 3;


            //chart1.AxisX.Visible = false;

        }


        #endregion InitChart 설정

        #region Init Grid 설정
        private void InitGrd()
        {
            // 선택 항목
            InitGrid_Checker(grdCheck);

            InitGrid_Checker1(grdCheck1);

            // #1 교정
            InitGrd_STR1(grdSTR1);

            InitGrd_STR2(grdSTR2);


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

        private void InitGrid_Checker1(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search(grd);

            var crCellRange = grd.GetCellRange(0, grd.Cols["WORK_TIME"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grd.Styles["ModifyStyle"];

            grd.Cols["WORK_DDTT"].AllowEditing = true;
            grd.Cols["WORK_DAY"].AllowEditing = true;
            grd.Cols["PIECE_NO"].AllowEditing = true;
            grd.Cols["GOOD_YN"].AllowEditing = true;
            grd.Cols["WORK_TIME"].AllowEditing = true;


            grd.Cols["WORK_DDTT"].Caption = "작업일자";
            grd.Cols["WORK_DAY"].Caption = "탐상시간";
            grd.Cols["PIECE_NO"].Caption = "No";
            grd.Cols["GOOD_YN"].Caption = "합부";
            grd.Cols["WORK_TIME"].Caption = "시간";


            grd.Cols["WORK_DDTT"].Width = 0;
            grd.Cols["WORK_DAY"].Width = 160;
            grd.Cols["PIECE_NO"].Width = 50;
            grd.Cols["GOOD_YN"].Width = 20;
            grd.Cols["WORK_TIME"].Width = 0;


            grd.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
            grd.Cols["WORK_DDTT"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["WORK_DAY"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["PIECE_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["GOOD_YN"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["WORK_TIME"].TextAlign = cs.DATE_TextAlign;


            Txt6.Text = "5.0~(E)";
            Txt7.Text = "4.0~4.9(D)";
            Txt8.Text = "3.0~3.9(D)";
            Txt9.Text = "2.0~2.9(D)";
            Txt10.Text = "1.5~1.9(D)";

            //Label lbSel = new Label();

            //lbSel.BackColor = Color.Transparent;
            //lbSel.Cursor = Cursors.Hand;


            //lbSel.Click += SEL_Click;

            //_al.Add(new Order.CrtInOrdCre.HostedControl(grd, lbSel, 0, grd.Cols["WORK_DDTT"].Index));

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


        

        

        private void InitGrd_STR1(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search(grd);

            grd.AllowEditing = false;

            grd.Rows[0].Height = 40;

            #region 테스트..

            grd.Cols["L_NO"].Caption = "탐상길이";
            grd.Cols["WORK_DDTT"].Caption = "탐상시각";
            grd.Cols["A_OR"].Caption = "채널A";
            grd.Cols["B_OR"].Caption = "채널B";
            grd.Cols["CH1"].Caption = "A채널 P1";
            grd.Cols["CH2"].Caption = "A채널 P2";
            grd.Cols["CH3"].Caption = "A채널 P3";
            grd.Cols["CH4"].Caption = "A채널 P4";
            grd.Cols["CH5"].Caption = "A채널 P5";
            grd.Cols["CH6"].Caption = "B채널 P1";
            grd.Cols["CH7"].Caption = "B채널 P2";
            grd.Cols["CH8"].Caption = "B채널 P3";
            grd.Cols["CH9"].Caption = "B채널 P4";
            grd.Cols["CH10"].Caption = "B채널 P5";
            //grd.Cols["POINT"].Caption = "POINT";


            grd.Cols["L_NO"].Width = 70;
            grd.Cols["WORK_DDTT"].Width = 0;

            grd.Cols["A_OR"].Width = 50;
            grd.Cols["B_OR"].Width = 50;
            grd.Cols["CH1"].Width = 70;
            grd.Cols["CH2"].Width = 70;
            grd.Cols["CH3"].Width = 70;
            grd.Cols["CH4"].Width = 70;
            grd.Cols["CH5"].Width = 70;
            grd.Cols["CH6"].Width = 70;
            grd.Cols["CH7"].Width = 70;
            grd.Cols["CH8"].Width = 70;
            grd.Cols["CH9"].Width = 70;
            grd.Cols["CH10"].Width = 70;
            grd.Cols["LIMIT"].Width = 70;


            grd.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["WORK_DDTT"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["A_OR"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["B_OR"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH1"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH2"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH3"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH4"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH5"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH6"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH7"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH8"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH9"].TextAlign = cs.Angle_Value_TextAlign;
            grd.Cols["CH10"].TextAlign = cs.Angle_Value_TextAlign;
            //grd.Cols["POINT"].TextAlign = cs.Angle_Value_TextAlign;


            #endregion

            grd.Cols.Frozen = grd.Cols["WORK_DDTT"].Index + 1;
        }

        private void InitGrd_STR2(C1FlexGrid grd)
        {
            try
            {
                grd.Clear();
                // Grid Init
                grd.Rows.Count = 5;
                grd.Cols.Count = 100 * 7;
                grd.Rows.Fixed = 0;
                grd.Cols.Fixed = 0;
                grd.Rows.DefaultSize = 23;
                //grd.Rows.DefaultSize = 42;
                grd.Cols.DefaultSize = 2;
                grd.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
                grd.Rows[grd.Rows.Count - 1].TextAlign = TextAlignEnum.CenterCenter;
                //grdList.ForeColor.ToArgb()
                // Grid Line Color 설정
                grd.Styles.Normal.Border.Color = Color.Black;

                // Grid Highlight 해제 및 선택 기능 해제
                grd.HighLight = HighLightEnum.Never;
                //grdList.Enabled = false;

                // Grid Default Backcolor Style 설정
                CellStyle gridStyle = grd.Styles.Add("A");
                gridStyle.BackColor = Color.Yellow;

                gridStyle = grd.Styles.Add("B");
                gridStyle.BackColor = Color.Red;

                gridStyle = grd.Styles.Add("C");
                gridStyle.BackColor = Color.Lime;

                gridStyle = grd.Styles.Add("D");
                gridStyle.BackColor = Color.LightGray;

                gridStyle = grd.Styles.Add("E");
                gridStyle.BackColor = Color.Blue;

                gridStyle = grd.Styles.Add("F");
                gridStyle.BackColor = Color.Black;

                gridStyle = grd.Styles.Add("LINE");
                gridStyle.BackColor = Color.White;

                gridStyle = grd.Styles.Add("Point");
                gridStyle.BackColor = Color.Black;

                for (int j = 0; j < grd.Rows.Count; j++)
                {
                    for (int i = 0; i < grd.Cols.Count; i++)
                        grd.SetCellStyle(j, i, "Point");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                //selectedTableNM = "TB_MLFT_OPERINFO_NO1";
                selectedTableNM = "TB_MLFT_OPERINFO_NO1@MLFT_DBLINK";
                selectedGrd = grdSTR1;
                moddt_grdSTR1 = new DataTable();
                ShowDataSTR1(selectedTableNM, selectedGrd, moddt_grdSTR1);
                InitGrd_STR2(grdSTR2);
                ShowDataSTR4(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR7(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR8(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR9(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR10(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR11(selectedTableNM, selectedGrd, moddt_grdSTR1);
                mlft_defect_position_display();
            }
            else if (uC_Line_gp_s1.Line_GP == "#2")
            {
                // 2라인
                selectedTableNM = "TB_MLFT_OPERINFO_NO2@MLFT_DBLINK";
                selectedGrd = grdSTR1;
                moddt_grdSTR1 = new DataTable();
                ShowDataSTR1(selectedTableNM, selectedGrd, moddt_grdSTR1);
                InitGrd_STR2(grdSTR2);
                ShowDataSTR4(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR7(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR8(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR9(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR10(selectedTableNM, selectedGrd, moddt_grdSTR1);
                ShowDataSTR11(selectedTableNM, selectedGrd, moddt_grdSTR1);
                mlft_defect_position_display();
            }
            else if (uC_Line_gp_s1.Line_GP == "#3")
            {
                // 3라인
                selectedTableNM = "TB_MLFT_OPERINFO_NO3@MLFT_DBLINK";
                selectedGrd = grdSTR1;
                moddt_grdSTR1 = new DataTable();
                ShowDataSTR1(selectedTableNM, selectedGrd, moddt_grdSTR1);
            }
            // 모든 그리드를 숨기고 해당 그리드만 docking 시킴
            //selectedGrd1.Dock = DockStyle.Fill;
            //selectedGrd1.BringToFront();

            IsChangeLineOrRoutingOrCheckerInGrid = false;
        }

        

        

        

        private void ShowDataSTR1(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format("SELECT ");
            sql1 += string.Format("    POINT AS L_NO");
            sql1 += string.Format("    ,TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 14), 'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH:MI:SS') AS WORK_DDTT ");
            sql1 += string.Format("   ,A_OR ");
            sql1 += string.Format("   ,B_OR ");
            sql1 += string.Format("   ,CH1 ");
            sql1 += string.Format("   ,CH2 ");
            sql1 += string.Format("   ,CH3 ");
            sql1 += string.Format("   ,CH4 ");
            sql1 += string.Format("   ,CH5 ");
            sql1 += string.Format("   ,CH6 ");
            sql1 += string.Format("   ,CH7 ");
            sql1 += string.Format("   ,CH8 ");
            sql1 += string.Format("   ,CH9 ");
            sql1 += string.Format("   ,CH10 ");
            sql1 += string.Format("   ,LIMIT ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND PIECE_NO = '{0}' ", PIECE_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 9, 4) = '{0}' ", WORK_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 1, 8) = '{0}' ", date_id);
            sql1 += string.Format("  ORDER BY POINT ");





            dt = cd.FindDataTable(sql1);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grd.SetDataBinding(dt, null, true);

            //L_NO 추가 하고 WORK_DDTT column 제거후 chart1에 databinding

            chart_dt1 = dt.Copy();

            chart_dt1.Columns.Remove("WORK_DDTT");
            chart_dt1.Columns.Remove("L_NO");

            SetChart(chart_dt1, grd, chart1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + dt.Rows.Count.ToString() + " 건 조회 되었습니다.");
        }


        private void ShowDataSTR2(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format(" SELECT TO_CHAR(TO_DATE(WORK_DDTT, 'YYYYMMDD HH24:MI:SS'), 'YYYYMMDD HH24:MI:SS') AS WORK_DAY , PIECE_NO, GOOD_YN, SUBSTR(WORK_DDTT,1,8) AS WORK_DDTT, SUBSTR(WORK_DDTT, 9, 4) AS WORK_TIME");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND GOOD_YN LIKE '%{0}%' ", uC_OKNG1.OKNG);
            sql1 += string.Format("  GROUP BY WORK_DDTT, PIECE_NO, GOOD_YN");
            sql1 += string.Format("  ORDER BY WORK_DDTT, PIECE_NO ");



            dt = cd.FindDataTable(sql1);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grd.SetDataBinding(dt, null, true);

            this.Cursor = System.Windows.Forms.Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + dt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            
        }




        private void ShowDataSTR4(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format("SELECT FN_GET_INFO_MLFT_GRADE(A_OR, B_OR) AS GRADE ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND PIECE_NO = '{0}' ", PIECE_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 9, 4) = '{0}' ", WORK_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 1, 8) = '{0}' ", date_id);
            sql1 += string.Format("  ORDER BY POINT ");

            dt = cd.FindDataTable(sql1);
            int pos = 0;
            foreach (DataRow row in dt.Rows)
            {

                if (row["GRADE"].ToString() == "A")
                    grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 1, pos, "A");
                else if (row["GRADE"].ToString() == "B")
                    grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 2, pos, "B");
                else if (row["GRADE"].ToString() == "C")
                    grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 3, pos, "C");
                else if (row["GRADE"].ToString() == "D")
                    grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 4, pos, "D");
                else if (row["GRADE"].ToString() == "E")
                    grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 5, pos, "E");

                pos++;

            }

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            //grd.SetDataBinding(dt, null, true);

            this.Cursor = System.Windows.Forms.Cursors.Default;
            //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + dt.Rows.Count.ToString() + " 건 조회 되었습니다.");
        }

        private void ShowDataSTR7(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format(" SELECT COUNT(GRADE) AS COUNT ");
            sql1 += string.Format(" FROM( ");
            sql1 += string.Format("SELECT FN_GET_INFO_MLFT_GRADE(A_OR, B_OR) AS GRADE ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND PIECE_NO = '{0}' ", PIECE_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 9, 4) = '{0}' ", WORK_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 1, 8) = '{0}') ", date_id);
            sql1 += string.Format("  WHERE GRADE = 'A' ");
            sql1 += string.Format("  ORDER BY GRADE ");

            dt = cd.FindDataTable(sql1);
            if (dt != null)
            {
                try
                {

                    Txt5.Text = dt.Rows[0]["COUNT"].ToString();
                    

                }
                catch (Exception)
                {
                    return;
                }
                //grdMain8.SetDataBinding(moddt, null, true);
            }
        }
        private void ShowDataSTR8(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format(" SELECT COUNT(GRADE) AS COUNT ");
            sql1 += string.Format(" FROM( ");
            sql1 += string.Format("SELECT FN_GET_INFO_MLFT_GRADE(A_OR, B_OR) AS GRADE ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND PIECE_NO = '{0}' ", PIECE_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 9, 4) = '{0}' ", WORK_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 1, 8) = '{0}') ", date_id);
            sql1 += string.Format("  WHERE GRADE = 'B' ");
            sql1 += string.Format("  ORDER BY GRADE ");

            dt = cd.FindDataTable(sql1);
            if (dt != null)
            {
                try
                {

                    Txt4.Text = dt.Rows[0]["COUNT"].ToString();


                }
                catch (Exception)
                {
                    return;
                }
                //grdMain8.SetDataBinding(moddt, null, true);
            }
        }
        private void ShowDataSTR9(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format(" SELECT COUNT(GRADE) AS COUNT ");
            sql1 += string.Format(" FROM( ");
            sql1 += string.Format("SELECT FN_GET_INFO_MLFT_GRADE(A_OR, B_OR) AS GRADE ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND PIECE_NO = '{0}' ", PIECE_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 9, 4) = '{0}' ", WORK_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 1, 8) = '{0}') ", date_id);
            sql1 += string.Format("  WHERE GRADE = 'C' ");
            sql1 += string.Format("  ORDER BY GRADE ");

            dt = cd.FindDataTable(sql1);
            if (dt != null)
            {
                try
                {

                    Txt3.Text = dt.Rows[0]["COUNT"].ToString();


                }
                catch (Exception)
                {
                    return;
                }
                //grdMain8.SetDataBinding(moddt, null, true);
            }
        }
        private void ShowDataSTR10(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format(" SELECT COUNT(GRADE) AS COUNT ");
            sql1 += string.Format(" FROM( ");
            sql1 += string.Format("SELECT FN_GET_INFO_MLFT_GRADE(A_OR, B_OR) AS GRADE ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND PIECE_NO = '{0}' ", PIECE_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 9, 4) = '{0}' ", WORK_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 1, 8) = '{0}') ", date_id);
            sql1 += string.Format("  WHERE GRADE = 'D' ");
            sql1 += string.Format("  ORDER BY GRADE ");

            dt = cd.FindDataTable(sql1);
            if (dt != null)
            {
                try
                {

                    Txt2.Text = dt.Rows[0]["COUNT"].ToString();


                }
                catch (Exception)
                {
                    return;
                }
                //grdMain8.SetDataBinding(moddt, null, true);
            }
        }
        private void ShowDataSTR11(string selectedTableNM, C1FlexGrid grd, DataTable dt)
        {

            sql1 = string.Empty;
            sql1 += string.Format(" SELECT COUNT(GRADE) AS COUNT ");
            sql1 += string.Format(" FROM( ");
            sql1 += string.Format("SELECT FN_GET_INFO_MLFT_GRADE(A_OR, B_OR) AS GRADE ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("  {0} ", selectedTableNM);
            sql1 += string.Format("  WHERE HEAT = '{0}' ", HEAT_id);
            sql1 += string.Format("  AND PIECE_NO = '{0}' ", PIECE_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 9, 4) = '{0}' ", WORK_id);
            sql1 += string.Format("  AND SUBSTR(WORK_DDTT, 1, 8) = '{0}') ", date_id);
            sql1 += string.Format("  WHERE GRADE = 'E' ");
            sql1 += string.Format("  ORDER BY GRADE ");

            dt = cd.FindDataTable(sql1);
            if (dt != null)
            {
                try
                {

                    Txt1.Text = dt.Rows[0]["COUNT"].ToString();


                }
                catch (Exception)
                {
                    return;
                }
                //grdMain8.SetDataBinding(moddt, null, true);
            }
        }

        private bool ShowDataSTR3()
        {
            sql1 = string.Empty;
            sql1 = string.Format("SELECT MAX(A.POC_NO)      AS POC_NO   ");
            sql1 += string.Format("FROM   TB_CR_ORD A ");
            sql1 += string.Format("WHERE  A.HEAT  = '{0}' ", HEAT_id);      //POC_NO
                                                                            //sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
            olddt = cd.FindDataTable(sql1);

            POC_id = olddt.Rows[0]["POC_NO"].ToString();
               



            sql1 = string.Empty;
            sql1 = string.Format("SELECT A.POC_NO      AS POC_NO   ");
            sql1 += string.Format("      ,A.HEAT       AS HEAT ");
            sql1 += string.Format("      ,A.STEEL      AS STEEL ");
            sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
            sql1 += string.Format("      ,A.ITEM  AS ITEM ");
            sql1 += string.Format("      ,A.ITEM_SIZE  AS ITEM_SIZE ");
            sql1 += string.Format("      ,A.LENGTH     AS ITEM_LENGTH    ");
            sql1 += string.Format("      ,A.SURFACE_LEVEL AS SURFACE_LEVEL    ");
            sql1 += string.Format("FROM   TB_CR_ORD A ");
            sql1 += string.Format("WHERE  A.POC_NO  = '{0}' ", POC_id);      //POC_NO
                                                                                     //sql1 += string.Format("AND    A.LINE_GP = '{0}' ", line_ID);             //LINE_GP
            olddt = cd.FindDataTable(sql1);

            if (olddt == null || olddt.Rows.Count == 0)
            {

                return false;
            }
            else
            {
                //txtItem.Text = olddt.Rows[0]["ITEM"].ToString();
                txtSize.Text = olddt.Rows[0]["ITEM_SIZE"].ToString();
                txtLength.Text = string.Format("{0:#,0.00}", double.Parse(olddt.Rows[0]["ITEM_LENGTH"].ToString()));
                txtSurface.Text = olddt.Rows[0]["SURFACE_LEVEL"].ToString();
                txtSteel.Text = olddt.Rows[0]["STEEL"].ToString();
                txtSteel_Nm.Text = olddt.Rows[0]["STEEL_NM"].ToString();
                //txtPcs.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["MILL_PCS"].ToString()));
                //txtTheory_Wgt.Text = string.Format("{0:#,0}", double.Parse(olddt.Rows[0]["THEORY_WGT"].ToString()));

                //btnConfirm.Enabled = true;
            }

            return true;

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


        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_dt_str = clsUtil.Utl.GetDateFormat(3, start_dt.Value);
            start_dt_str1 = Convert.ToInt32(start_dt_str);

            SetComboBox2();
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            end_dt_str = clsUtil.Utl.GetDateFormat(3, end_dt.Value);
            end_dt_str1 = Convert.ToInt32(end_dt_str);

            SetComboBox2();
            
        }

        private void Start_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }


        private void End_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        public void mlft_defect_position_display()
        {
            grdSTR2.Redraw = false;
            int pos = 0;
            //foreach (var temp in map)
            //{
       
            //        if (temp.Value.ToString() == "A")
            //            grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 1, pos, "A");
            //        else if (temp.Value.ToString() == "B")
            //            grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 2, pos, "B");
            //        else if (temp.Value.ToString() == "C")
            //            grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 3, pos, "C");
            //        else if (temp.Value.ToString() == "D")
            //            grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 4, pos, "D");
            //        else if (temp.Value.ToString() == "E")
            //            grdSTR2.SetCellStyle(grdSTR2.Rows.Count - 5, pos, "E");
                
            //    pos++;
            //}

            //라인구분 추가
            int iColPos = 0;


            for (int i = 0; i < 7; i++)
            {
                grdSTR2.Cols.Insert((i + 1) * 100 + 1 * i);
                grdSTR2.Cols[(i + 1) * 100 + 1 * i].Width = 2;

                for (int j = 0; j < grdSTR2.Rows.Count; j++)
                    grdSTR2.SetCellStyle(j, (i + 1) * 100 + 1 * i, "LINE");

                iColPos++;
            }

            int iRowPos = 0;
            for (int i = 0; i < grdSTR2.Rows.Count; i++)
            {
                if (i == 0 || i == 2 || i == 4 || i == 6)
                {
                    grdSTR2.Rows.Insert(i + 1);
                    grdSTR2.Rows[i + 1].Height = 2;

                    for (int j = 0; j < grdSTR2.Cols.Count; j++)
                        grdSTR2.SetCellStyle(i + 1, j, "LINE");

                    iRowPos++;
                }
            }
            grdSTR2.Redraw = true;
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


            //chart1.AxisY.Min = minValue - marginValue;
            //chart1.AxisY.Max = maxValue + marginValue;

            chart1.AxisY.Min = 0;
            chart1.AxisY.Max = 7;


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
                    //if (col.ColumnName == "L_NO" || col.ColumnName == "WORK_DDTT") // L_NO + WORK_DDTT
                    if (col.ColumnName == "L_NO" || col.ColumnName == "WORK_DDTT" || col.ColumnName == "POINT") // L_NO + WORK_DDTT
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
                    //if (col.ColumnName == "L_NO" || col.ColumnName == "WORK_DDTT") // L_NO + WORK_DDTT
                    if (col.ColumnName == "L_NO" || col.ColumnName == "WORK_DDTT" || col.ColumnName == "POINT") // L_NO + WORK_DDTT
                                                                                       //if (col.ColumnName == "WORK_DDTT") // L_NO + WORK_DDTT
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
            //chart.DataSourceSettings.Fields.Add(new FieldMap("POINT", FieldUsage.Label));
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

        /// <summary>
        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCheck_Paint(object sender, PaintEventArgs e)
        {
            foreach (Order.CrtInOrdCre.HostedControl hosted in _al)
                hosted.UpdatePosition();
        }

        private void grdCheck1_Click(object sender, RowColEventArgs e)
        {
            int editedRow = e.Row;
            PIECE_id = grdCheck1.GetData(editedRow, "PIECE_NO").ToString();
            btnDisplay_Click(null, null);
        }


        private void grdCheck1_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedRow <= 0)
            {
                return;
            }

            // 수정여부 확인을 위해 저장
            PIECE_id = editedGrd.GetData(editedRow, "PIECE_NO").ToString();
            WORK_id = editedGrd.GetData(editedRow, "WORK_TIME").ToString();
            date_id = editedGrd.GetData(editedRow, "WORK_DDTT").ToString();
            btnDisplay_Click(null, null);
        }

        private void UpdateChart1()
        {

            //수정여부 확인을 위해 저장
            PIECE_id = grdCheck1.GetData(1, "PIECE_NO").ToString();
            WORK_id = grdCheck1.GetData(1, "WORK_TIME").ToString();
            date_id = grdCheck1.GetData(1, "WORK_DDTT").ToString();
            btnDisplay_Click(null, null);
        }

        private void c1Button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("현재 화면을 인쇄 하시겠습니까?", "화면 인쇄", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                PictureScreen.PrintScreen(this);
                //PictureScreen.ScreenSave(titleNM, this);
            }
        }


    }
}
