﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using static ComLib.clsUtil;
using System.Collections.Specialized;
using SystemControlClassLibrary.Popup;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SystemControlClassLibrary
{

    public partial class STRRslt : Form
    {
        #region 변수선언
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        DataTable olddt = new DataTable();

        private string cd_id = "";
        private string cd_id2 = "";
        private string cd_id3 = "";
        private string current_Tab = "";
        private string sql = "";
        private string ownerNM = "";
        private string titleNM = "";
        bool _CanSaveSearchLog = false;
        private C1FlexGrid selectedgrid;
        private int subtotalNo;
        #endregion

        #region 화면
        public STRRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void STRRslt_Load(object sender, System.EventArgs e)
        {
            TabOpt.SelectedIndex = 0;

            InitControl();

            EventCreate();      //사용자정의 이벤트
            _CanSaveSearchLog = true;
            btnDisplay_Click(null, null);
        }

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            //Label
            cs.InitLabel(lblHeat);
            cs.InitLabel(lblSteel);
            cs.InitLabel(lblLine);
            cs.InitLabel(lblPoc);
            cs.InitLabel(lblWorkType);
            cs.InitLabel(lblMfgDate);
            cs.InitLabel(lblItemSize);
            cs.InitLabel(lblTEAM);

            //Button
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            

            //TextBox
            cs.InitTextBox(txtPoc);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(txtHeat);
            cs.InitTextBox(txtItemSize);
            cs.InitTextBox(gangjong_Nm_tb);

            //DateTime
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            //Tab
            cs.InitTab(TabOpt);

            SetComboBox();

            InitGrd_Main1();
            InitGrd_Main2();
            InitGrd_Main3();

            

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


        #endregion

        #region "콤보박스 설정"
        private void SetComboBox()
        {

            //Combo
            cs.InitCombo(cbo_Work_Type, StringAlignment.Near);
            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            cs.InitCombo(cboTEAM, StringAlignment.Near);

            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);

            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);

            cd.SetCombo(cboTEAM, "WORK_TEAM", "", true);
        }

        #endregion

        #region 조회
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (_CanSaveSearchLog)
            {
                cd.InsertLogForSearch(ck.UserID, btnDisplay);
            }            

            SetDataBinding();
        }

        private void SetDataBinding()
        {


            SetSelectedGrid();

            try
            {
                //쿼리 가져욤
                sql = QuerySetup();

                olddt = cd.FindDataTable(sql);
                Cursor.Current = Cursors.WaitCursor;
                selectedgrid.SetDataBinding(olddt, null, true);
                Cursor.Current = Cursors.Default;

                if (current_Tab == "TabP2" && (cboLine_GP.Text == "#2라인" || cboLine_GP.Text == "#3라인"))
                {
                    selectedgrid.AutoSizeCols();
                }
                else if(current_Tab == "TabP1")
                {
                    UpdateTotals();
                }



                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), olddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }

            
        }
        #endregion

        private void UpdateTotals()
        {

            subtotalNo = 0;

            // clear existing totals
            grdMain1.Subtotal(AggregateEnum.Clear);

            grdMain1.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain1.Cols["MILL_PCS"].Index, "합계");
            grdMain1.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain1.Cols["MILL_WGT"].Index, "합계");
            grdMain1.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain1.Cols["STR_PCS"].Index, "합계");

            //sql1 += string.Format("        ,MILL_PCS");
            //sql1 += string.Format("        ,MILL_WGT");
            //sql1 += string.Format("        ,STR_PCS");
            AddSubtotalNo();
            grdMain1.Rows.Frozen = GetAvailMinRow(grdMain1) -1;
            //grdMain.Subtotal(AggregateEnum.Average, 1, -1, grdMain.Cols["THEORY_WGT"].Index, "평균");

            //grdMain.AutoSizeCols();

            //grdMain.Rows.Fixed = GetAvailMinRow();
        }

        private void AddSubtotalNo()
        {
            ++subtotalNo;
        }

        private int GetAvailMinRow(C1FlexGrid grid)
        {
            return (grid.Rows.Fixed + subtotalNo);
        }


        #region 닫기
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "엑셀파일 생성"
        private void btnExcel_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = "xlsx";

            C1.Win.C1FlexGrid.C1FlexGrid grid = new C1.Win.C1FlexGrid.C1FlexGrid();

            if (current_Tab == "TabP1")
            {
                grid = grdMain1;
                dlg.FileName = "교정_실적_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP2" && (cboLine_GP.Text == "#2라인" || cboLine_GP.Text == "#3라인"))
            {
                grid = grdMain2_2;
                dlg.FileName = "교정_조업_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }

            if (current_Tab == "TabP2" && cboLine_GP.Text == "#1라인")
            {
                grid = grdMain2_1;
                dlg.FileName = "교정_조업_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                grid.SaveGrid(dlg.FileName, C1.Win.C1FlexGrid.FileFormatEnum.Excel, C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells);

                if (MessageBox.Show("저장한 Excel File을 여시겠습니까?", "Excel File Open", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("excel.exe", "\"" + dlg.FileName + "\"");
                }
            }
        }
        #endregion

        #region 이벤트
        private void TabOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabOpt.SelectedIndex == 0) current_Tab = "TabP1";
            if (TabOpt.SelectedIndex == 1) current_Tab = "TabP2";

            btnDisplay_Click(null, null);
            //SetDataBinding();
        }

        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            C1.Win.C1FlexGrid.C1FlexGrid grid = new C1.Win.C1FlexGrid.C1FlexGrid();

            cd_id = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;

            ck.Line_gp = cd_id;

            //sql = "";
            //if(olddt!=null) olddt.Clear();
            SetSelectedGrid();

            //btnDisplay_Click(null, null);
        }

        private void SetSelectedGrid()
        {

            if (current_Tab == "TabP1") selectedgrid = grdMain1;
            if (current_Tab == "TabP2" && (cboLine_GP.Text == "#2라인" || cboLine_GP.Text == "#3라인"))
            {
                selectedgrid = grdMain2_2;
                selectedgrid.Dock = System.Windows.Forms.DockStyle.Fill;
                selectedgrid.BringToFront();
            }
            if (current_Tab == "TabP2" && cboLine_GP.Text == "#1라인")
            {
                selectedgrid = grdMain2_1;
                selectedgrid.Dock = System.Windows.Forms.DockStyle.Fill;
                selectedgrid.BringToFront();
            }

        }


        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id2 = ((ComLib.DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }
        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id3 = ((ComLib.DictionaryList)cboTEAM.SelectedItem).fnValue;
        }

        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjong_Nm_tb.Text = "";
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        #endregion

        #region "사용자정의 이벤트"
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
        }

        //강종ID(LostFocus)
        private void gangjong_id_tb_LostFocus(object sender, EventArgs e)
        {
            if (gangjong_id_tb.Text == "") gangjong_Nm_tb.Text = "";
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
            }
        }
        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }
        #endregion

        #region "팝업화면-강종 찾기"
        private void btnSteel_Click(object sender, EventArgs e)
        {
            SearchSteelNm popup = new SearchSteelNm();
            popup.Owner = this;      //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }
        #endregion

        #region 그리드 설정
        
        private void InitGrd_Main1()
        {
            clsStyle.Style.InitGrid_search(grdMain1);

            grdMain1.AllowEditing = false;
            grdMain1.AutoResize = true;
            //생산실적-----------------------------------------------------------------------

            grdMain1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain1.Cols["MFG_DATE"].Width = cs.Date_8_Width + 20;
            grdMain1.Cols["WORK_TYPE"].Width = 0;
            grdMain1.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width -10;
            grdMain1.Cols["WORK_TEAM"].Width = 0;
            grdMain1.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width - 10;
            grdMain1.Cols["POC_NO"].Width = cs.POC_NO_Width -30;                     //변경전 90
            grdMain1.Cols["MILL_NO"].Width = cs.Mill_No_Width + 10;              //변경전 130;
            grdMain1.Cols["HEAT"].Width = cs.HEAT_Width -10;                         //변경전 90;
            grdMain1.Cols["STEEL"].Width = cs.STEEL_Width + 10;                  //변경전 50;
            grdMain1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 30;            //변경전 100;
            grdMain1.Cols["ITEM"].Width = cs.ITEM_Width + 15;                    //변경전 60;
            grdMain1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 25;          //변경전 70;
            grdMain1.Cols["LENGTH"].Width = cs.LENGTH_Width ;                //변경전 70;
            grdMain1.Cols["MILL_PCS"].Width = cs.MILL_PCS_Width;                 //변경전 100;
            grdMain1.Cols["MILL_WGT"].Width = cs.MILL_WGT_Width;                 //변경전 100;
            grdMain1.Cols["STR_PCS"].Width = cs.STR_PCS_Width;                   //변경전 100;

            grdMain1.Cols["STR_WGT"].Width = 0;

            grdMain1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain1.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain1.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain1.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain1.Cols["WORK_TEAM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grdMain1.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TEAM_NM_TextAlign;
            grdMain1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain1.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain1.Cols["STR_PCS"].TextAlign = cs.STR_WGT_TextAlign + 5;
            grdMain1.Cols["STR_WGT"].TextAlign = cs.STR_WGT_TextAlign + 5;
            grdMain1.Cols["MILL_PCS"].TextAlign = cs.STR_WGT_TextAlign + 5;
            grdMain1.Cols["MILL_WGT"].TextAlign = cs.STR_WGT_TextAlign + 5;

            grdMain1.Rows[0].TextAlign = TextAlignEnum.CenterCenter; // 캡션 정렬
            grdMain1.ExtendLastCol = true;

            grdMain1.Tree.Column = 1;

        }
        private void InitGrd_Main2()
        {
            clsStyle.Style.InitGrid_search(grdMain2_1);

            grdMain2_1.AllowEditing = false;
            grdMain2_1.AutoResize = true;
            
            //조업정보(라인별로 그리드 캡션 변경)-------------------------------------------------
            grdMain2_1.Cols["L_NO"].Caption = "NO";
            grdMain2_1.Cols["WORK_DATE"].Caption = "작업일자";
            grdMain2_1.Cols["WORK_TIME"].Caption = "작업시각";
            grdMain2_1.Cols["POC_NO"].Caption = "POC";
            grdMain2_1.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain2_1.Cols["HEAT"].Caption = "HEAT";
            grdMain2_1.Cols["STEEL"].Caption = "강종";
            grdMain2_1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain2_1.Cols["ITEM"].Caption = "품목";
            grdMain2_1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain2_1.Cols["LENGTH"].Caption = "길이(m)";
            grdMain2_1.Cols["COL_01"].Caption = "상롤Gap1";
            grdMain2_1.Cols["COL_02"].Caption = "상롤Gap2";
            grdMain2_1.Cols["COL_03"].Caption = "상롤Gap3";
            grdMain2_1.Cols["COL_04"].Caption = "상롤Gap4";
            grdMain2_1.Cols["COL_05"].Caption = "상롤Gap5";
            grdMain2_1.Cols["COL_06"].Caption = "상롤Gap6";
            grdMain2_1.Cols["COL_07"].Caption = "상롤각도1";
            grdMain2_1.Cols["COL_08"].Caption = "상롤각도2";
            grdMain2_1.Cols["COL_09"].Caption = "상롤각도3";
            grdMain2_1.Cols["COL_10"].Caption = "상롤각도4";
            grdMain2_1.Cols["COL_11"].Caption = "상롤각도5";
            grdMain2_1.Cols["COL_12"].Caption = "상롤각도6";
            grdMain2_1.Cols["COL_13"].Caption = "하롤각도1";
            grdMain2_1.Cols["COL_14"].Caption = "하롤각도2";
            grdMain2_1.Cols["COL_15"].Caption = "하롤각도3";
            grdMain2_1.Cols["COL_16"].Caption = "Thread Speed";
            grdMain2_1.Cols["COL_17"].Caption = "Machine Speed";
            grdMain2_1.Cols["COL_18"].Caption = "Inlet Roll전류";
            grdMain2_1.Cols["COL_19"].Caption = "Mid Roll전류";
            grdMain2_1.Cols["COL_20"].Caption = "Outlet Roll전류";
            grdMain2_1.Cols["COL_21"].Visible = false;
            grdMain2_1.Cols["COL_22"].Visible = false;
            grdMain2_1.Cols["COL_23"].Visible = false;
            grdMain2_1.Cols["COL_24"].Visible = false;
            grdMain2_1.Cols["COL_25"].Visible = false;
            grdMain2_1.Cols["COL_26"].Visible = false;
            grdMain2_1.Cols["COL_27"].Visible = false;
            grdMain2_1.Cols["COL_28"].Visible = false;

            grdMain2_1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain2_1.Cols["WORK_DATE"].Width = cs.Date_8_Width;           //변경전 130;
            grdMain2_1.Cols["WORK_TIME"].Width = cs.Date_8_Width;           //변경전 100;
            grdMain2_1.Cols["POC_NO"].Width = cs.POC_NO_Width;              //변경전 80;
            grdMain2_1.Cols["MILL_NO"].Width = cs.Mill_No_Width + 20;       //변경전 130;
            grdMain2_1.Cols["HEAT"].Width = cs.HEAT_Width - 20;             //변경전 80;
            grdMain2_1.Cols["STEEL"].Width = cs.STEEL_Width;                //변경전 80;
            grdMain2_1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;          //변경전 80;
            grdMain2_1.Cols["ITEM"].Width = cs.ITEM_Width;                  //변경전 80;
            grdMain2_1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35;   //변경전 80;
            grdMain2_1.Cols["LENGTH"].Width = cs.LENGTH_Width;              //변경전 80;
            grdMain2_1.Cols["COL_01"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_02"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_03"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_04"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_05"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_06"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_07"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_08"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_09"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_10"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_11"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_12"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_13"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_14"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_15"].Width = cs.Short_Value_Width;
            grdMain2_1.Cols["COL_16"].Width = cs.Middle_Value_Width;
            grdMain2_1.Cols["COL_17"].Width = cs.Middle_Value_Width;
            grdMain2_1.Cols["COL_18"].Width = cs.Middle_Value_Width;
            grdMain2_1.Cols["COL_19"].Width = cs.Middle_Value_Width;
            grdMain2_1.Cols["COL_20"].Width = cs.Middle_Value_Width;
            grdMain2_1.Cols["COL_21"].Width = cs.Shortest_Value_Width;
            grdMain2_1.Cols["COL_22"].Width = cs.Shortest_Value_Width;
            grdMain2_1.Cols["COL_23"].Width = cs.Shortest_Value_Width;
            grdMain2_1.Cols["COL_24"].Width = cs.Shortest_Value_Width;
            grdMain2_1.Cols["COL_25"].Width = cs.Shortest_Value_Width;
            grdMain2_1.Cols["COL_26"].Width = cs.Shortest_Value_Width;
            grdMain2_1.Cols["COL_27"].Width = cs.Shortest_Value_Width;
            grdMain2_1.Cols["COL_28"].Width = cs.Shortest_Value_Width;

            grdMain2_1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain2_1.Cols["WORK_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain2_1.Cols["WORK_TIME"].TextAlign = cs.WORK_TIME_TextAlign;
            grdMain2_1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain2_1.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain2_1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain2_1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain2_1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain2_1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain2_1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain2_1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain2_1.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_16"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_17"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_18"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_19"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_20"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_21"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_22"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_23"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_24"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_25"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_26"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_27"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_28"].TextAlign = cs.VALUE_RIGHT_TextAlign;

            grdMain2_1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter; // 캡션 정렬
            grdMain2_1.ExtendLastCol = true;

           
        }
        private void InitGrd_Main3()
        {
            clsStyle.Style.InitGrid_search(grdMain2_2);

            grdMain2_2.AllowEditing = false;
            grdMain2_2.AutoResize = true;
            
            //조업정보(라인별로 그리드 캡션 변경)-------------------------------------------------
            grdMain2_2.Cols["L_NO"].Caption = "NO";
            grdMain2_2.Cols["WORK_DATE"].Caption = "작업일자";
            grdMain2_2.Cols["WORK_TIME"].Caption = "작업시각";
            grdMain2_2.Cols["POC_NO"].Caption = "POC";
            grdMain2_2.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain2_2.Cols["HEAT"].Caption = "HEAT";
            grdMain2_2.Cols["STEEL"].Caption = "강종";
            grdMain2_2.Cols["STEEL_NM"].Caption = "강종명";
            grdMain2_2.Cols["ITEM"].Caption = "품목";
            grdMain2_2.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain2_2.Cols["LENGTH"].Caption = "길이(m)";
            grdMain2_2.Cols["COL_01"].Caption = "상롤 압력 현재값";
            grdMain2_2.Cols["COL_02"].Caption = "상롤 위치 현재값";
            grdMain2_2.Cols["COL_03"].Caption = "상롤 위치 설정값";
            grdMain2_2.Cols["COL_04"].Caption = "상롤 각도 현재값";
            grdMain2_2.Cols["COL_05"].Caption = "상롤 각도 설정값";
            grdMain2_2.Cols["COL_06"].Caption = "하롤 각도 현재값";
            grdMain2_2.Cols["COL_07"].Caption = "하롤 각도 설정값";
            grdMain2_2.Cols["COL_08"].Caption = "상하가이드의위치현재값";
            grdMain2_2.Cols["COL_09"].Caption = "상하가이드의위치설정값";
            grdMain2_2.Cols["COL_10"].Caption = "전후면의가이드위치설정값";
            grdMain2_2.Cols["COL_11"].Caption = "전면의가이드위치현재값";
            grdMain2_2.Cols["COL_12"].Caption = "후면의가이드위치현재값";
            grdMain2_2.Cols["COL_13"].Caption = "상롤 모터 속도 현재값";
            grdMain2_2.Cols["COL_14"].Caption = "상롤 모터 주파수 현재값";
            grdMain2_2.Cols["COL_15"].Caption = "상롤 모터 전류 현재값";
            grdMain2_2.Cols["COL_16"].Caption = "상하 롤러 속도 설정값"; //추가
            grdMain2_2.Cols["COL_17"].Caption = "하롤 모터 속도 현재값";
            grdMain2_2.Cols["COL_18"].Caption = "하롤 모터 주파수 현재값";
            grdMain2_2.Cols["COL_19"].Caption = "하롤 모터 전류 현재값";
            grdMain2_2.Cols["COL_20"].Caption = "인피딩 롤러구동주파수현재값";
            grdMain2_2.Cols["COL_21"].Caption = "인피딩 롤러구동주파수설정값";
            grdMain2_2.Cols["COL_22"].Caption = "인피딩 롤러구동전류현재값"; ///////////
            grdMain2_2.Cols["COL_23"].Caption = "ENTRY롤러구동주파수현재값";
            grdMain2_2.Cols["COL_24"].Caption = "ENTRY롤러구동주파수설정값";
            grdMain2_2.Cols["COL_25"].Caption = "ENTRY롤러구동전류 현재값";
            grdMain2_2.Cols["COL_26"].Caption = "EXIT롤러구동주파수 현재값";
            grdMain2_2.Cols["COL_27"].Caption = "EXIT롤러구동주파수 설정값";
            grdMain2_2.Cols["COL_28"].Caption = "EXIT롤러구동전류 현재값";

            grdMain2_2.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain2_2.Cols["WORK_DATE"].Width = cs.Date_8_Width;           //변경전 130;
            grdMain2_2.Cols["WORK_TIME"].Width = cs.Date_8_Width;           //변경전 100;
            grdMain2_2.Cols["POC_NO"].Width = cs.POC_NO_Width;              //변경전 80;
            grdMain2_2.Cols["MILL_NO"].Width = cs.Mill_No_Width + 20;       //변경전 130;
            grdMain2_2.Cols["HEAT"].Width = cs.HEAT_Width - 20;             //변경전 80;
            grdMain2_2.Cols["STEEL"].Width = cs.STEEL_Width;                //변경전 80;
            grdMain2_2.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;          //변경전 80;
            grdMain2_2.Cols["ITEM"].Width = cs.ITEM_Width;                  //변경전 80;
            grdMain2_2.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35;   //변경전 80;
            grdMain2_2.Cols["LENGTH"].Width = cs.LENGTH_Width;              //변경전 80;
            grdMain2_2.Cols["COL_01"].Width = cs.Middle_Value_Width + 10;
            grdMain2_2.Cols["COL_02"].Width = cs.Middle_Value_Width + 10;
            grdMain2_2.Cols["COL_03"].Width = cs.Middle_Value_Width + 10;
            grdMain2_2.Cols["COL_04"].Width = cs.Middle_Value_Width + 10;
            grdMain2_2.Cols["COL_05"].Width = cs.Middle_Value_Width + 10;
            grdMain2_2.Cols["COL_06"].Width = cs.Middle_Value_Width + 10;
            grdMain2_2.Cols["COL_07"].Width = cs.Middle_Value_Width + 10;
            grdMain2_2.Cols["COL_08"].Width = cs.Longest_Value_Width - 20;
            grdMain2_2.Cols["COL_09"].Width = cs.Longest_Value_Width - 20;
            grdMain2_2.Cols["COL_10"].Width = cs.Longest_Value_Width - 20;
            grdMain2_2.Cols["COL_11"].Width = cs.Longest_Value_Width - 20;
            grdMain2_2.Cols["COL_12"].Width = cs.Longest_Value_Width - 20;
            grdMain2_2.Cols["COL_13"].Width = cs.Longest_Value_Width - 40;
            grdMain2_2.Cols["COL_14"].Width = cs.Longer_Value_Width + 40;
            grdMain2_2.Cols["COL_15"].Width = cs.Longer_Value_Width + 10;
            grdMain2_2.Cols["COL_16"].Width = cs.Longer_Value_Width + 10;
            grdMain2_2.Cols["COL_17"].Width = cs.Longer_Value_Width + 10;
            grdMain2_2.Cols["COL_18"].Width = cs.Longer_Value_Width + 40;
            grdMain2_2.Cols["COL_19"].Width = cs.Longer_Value_Width + 10;
            grdMain2_2.Cols["COL_20"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_21"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_22"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_23"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_24"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_25"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_26"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_27"].Width = cs.Longest_Value_Width;
            grdMain2_2.Cols["COL_28"].Width = cs.Longest_Value_Width;

            grdMain2_2.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain2_2.Cols["WORK_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain2_2.Cols["WORK_TIME"].TextAlign = cs.WORK_TIME_TextAlign;
            grdMain2_2.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain2_2.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain2_2.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain2_2.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain2_2.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain2_2.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain2_2.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain2_2.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain2_2.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_16"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_17"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_18"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_19"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_20"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_21"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_22"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_23"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_24"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_25"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_26"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_27"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_28"].TextAlign = cs.VALUE_RIGHT_TextAlign;

            grdMain2_2.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter; // 캡션 정렬
            grdMain2_2.ExtendLastCol = true;
        }
        #endregion

        #region 쿼리 설정
        private string QuerySetup()
        {
            string sql1 = "";

            string start_date = start_dt.Value.ToString("yyyyMMdd");
            string end_date = end_dt.Value.ToString("yyyyMMdd");

            //생산실적---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP1")
            {
                sql1 = string.Format("  SELECT  ROWNUM AS L_NO ");
                sql1 += string.Format("        ,MFG_DATE");
                sql1 += string.Format("        ,WORK_TYPE");
                sql1 += string.Format("        ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM");
                sql1 += string.Format("        ,WORK_TEAM");
                sql1 += string.Format("        ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                sql1 += string.Format("        ,POC_NO");
                sql1 += string.Format("        ,MILL_NO ");
                sql1 += string.Format("        ,HEAT");
                sql1 += string.Format("        ,STEEL ");
                sql1 += string.Format("        ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM");
                sql1 += string.Format("        ,ITEM");
                sql1 += string.Format("        ,ITEM_SIZE");
                sql1 += string.Format("        ,LENGTH");
                sql1 += string.Format("        ,MILL_PCS");
                sql1 += string.Format("        ,MILL_WGT");
                sql1 += string.Format("        ,STR_PCS");
                sql1 += string.Format("        ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,STR_PCS),0)  AS STR_WGT "); //--중량계산 FUNC CALL 
                sql1 += string.Format("  FROM   (");
                sql1 += string.Format("          SELECT TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD')  AS MFG_DATE"); //작업일자 형식 변경
                sql1 += string.Format("                ,A.WORK_TYPE      AS WORK_TYPE");
                sql1 += string.Format("                ,NVL(A.WORK_TEAM,'O')  AS WORK_TEAM");
                sql1 += string.Format("                ,A.POC_NO         AS POC_NO");
                sql1 += string.Format("                ,A.MILL_NO        AS MILL_NO");
                sql1 += string.Format("                ,A.HEAT           AS HEAT");
                sql1 += string.Format("                ,MAX(A.ITEM)      AS ITEM");
                sql1 += string.Format("                ,MAX(A.ITEM_SIZE) AS ITEM_SIZE");
                sql1 += string.Format("                ,MAX(A.STEEL)     AS STEEL");
                sql1 += string.Format("                ,NVL(MAX(A.LENGTH),'0.00')    AS LENGTH");
                sql1 += string.Format("                ,MAX(B.PCS)       AS MILL_PCS");
                sql1 += string.Format("                ,MAX(B.NET_WGT)   AS MILL_WGT");
                sql1 += string.Format("                ,COUNT(*)         AS STR_PCS");
                sql1 += string.Format("          FROM   TB_CR_PIECE_WR       A ");
                sql1 += string.Format("                ,TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("          WHERE  A.MILL_NO  = B.MILL_NO");
                sql1 += string.Format("          AND    A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                sql1 += string.Format("          AND    A.LINE_GP = '{0}' ", cd_id);
                sql1 += string.Format("          AND    A.ROUTING_CD = 'A1'");// --교정
                if (cd_id2 != "%")
                {
                    sql1 += string.Format("          AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);
                }
                if (cd_id3 != "%")
                {
                    sql1 += string.Format("          AND NVL(A.WORK_TEAM,'A')  LIKE '%{0}%' ", cd_id3);
                }
                if (txtHeat.Text != "")
                {
                    sql1 += string.Format("          AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);
                }
                if (gangjong_id_tb.Text != "")
                {
                    sql1 += string.Format("          AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                }
                if (txtItemSize.Text != "")
                {
                    sql1 += string.Format("          AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                }
                if (txtPoc.Text != "")
                {
                    sql1 += string.Format("          AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
                }
                //sql1 += string.Format("          AND A.REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("                               WHERE MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                //sql1 += string.Format("                               AND  MILL_NO = A.MILL_NO ");
                //sql1 += string.Format("                               AND PIECE_NO = A.PIECE_NO ");
                //sql1 += string.Format("                               AND LINE_GP = A.LINE_GP ");
                //sql1 += string.Format("                               AND ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("          GROUP BY A.MFG_DATE");
                sql1 += string.Format("          ,A.WORK_TYPE");
                sql1 += string.Format("          ,NVL(A.WORK_TEAM,'O')");
                sql1 += string.Format("          ,A.POC_NO");
                sql1 += string.Format("          ,A.MILL_NO");
                sql1 += string.Format("          ,A.HEAT");
                sql1 += string.Format("          ORDER BY  MFG_DATE DESC, WORK_TYPE, MILL_NO ");
                sql1 += string.Format("        ) X ");


                //sql1 = string.Format("  SELECT  ROWNUM AS L_NO ");
                //sql1 += string.Format("        ,MFG_DATE");
                //sql1 += string.Format("        ,WORK_TYPE");
                //sql1 += string.Format("        ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM");
                //sql1 += string.Format("        ,WORK_TEAM");
                //sql1 += string.Format("        ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                //sql1 += string.Format("        ,POC_NO");
                //sql1 += string.Format("        ,MILL_NO ");
                //sql1 += string.Format("        ,HEAT");
                //sql1 += string.Format("        ,STEEL ");
                //sql1 += string.Format("        ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM");
                //sql1 += string.Format("        ,ITEM");
                //sql1 += string.Format("        ,ITEM_SIZE");
                //sql1 += string.Format("        ,LENGTH");
                //sql1 += string.Format("        ,MILL_PCS");
                //sql1 += string.Format("        ,MILL_WGT");
                //sql1 += string.Format("        ,STR_PCS");
                //sql1 += string.Format("        ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,STR_PCS),0)  AS STR_WGT "); //--중량계산 FUNC CALL 
                //sql1 += string.Format("  FROM   (");
                //sql1 += string.Format("          SELECT TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD')  AS MFG_DATE"); //작업일자 형식 변경
                //sql1 += string.Format("                ,A.WORK_TYPE      AS WORK_TYPE");
                //sql1 += string.Format("                ,NVL(A.WORK_TEAM,'A')  AS WORK_TEAM");
                //sql1 += string.Format("                ,A.POC_NO         AS POC_NO");
                //sql1 += string.Format("                ,A.MILL_NO        AS MILL_NO");
                //sql1 += string.Format("                ,A.HEAT           AS HEAT");
                //sql1 += string.Format("                ,MAX(A.ITEM)      AS ITEM");
                //sql1 += string.Format("                ,MAX(A.ITEM_SIZE) AS ITEM_SIZE");
                //sql1 += string.Format("                ,MAX(A.STEEL)     AS STEEL");
                //sql1 += string.Format("                ,NVL(MAX(A.LENGTH),'0.00')    AS LENGTH");
                //sql1 += string.Format("                ,MAX(B.PCS)       AS MILL_PCS");
                //sql1 += string.Format("                ,MAX(B.NET_WGT)   AS MILL_WGT");
                //sql1 += string.Format("                ,COUNT(*)         AS STR_PCS");
                //sql1 += string.Format("          FROM   TB_CR_PIECE_WR       A ");
                //sql1 += string.Format("                ,TB_CR_ORD_BUNDLEINFO B ");
                //sql1 += string.Format("          WHERE  A.MILL_NO  = B.MILL_NO");
                //sql1 += string.Format("          AND    A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                //sql1 += string.Format("          AND    A.LINE_GP = '{0}' ", cd_id);
                //sql1 += string.Format("          AND    A.ROUTING_CD = 'A1'");// --교정
                //sql1 += string.Format("          AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);
                //sql1 += string.Format("          AND NVL(A.WORK_TEAM,'A')  LIKE '%{0}%' ", cd_id3);
                //sql1 += string.Format("          AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);
                //sql1 += string.Format("          AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                //sql1 += string.Format("          AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                //sql1 += string.Format("          AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
                //sql1 += string.Format("          AND A.REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("                               WHERE  MILL_NO = A.MILL_NO ");
                //sql1 += string.Format("                               AND PIECE_NO = A.PIECE_NO ");
                //sql1 += string.Format("                               AND LINE_GP = A.LINE_GP ");
                //sql1 += string.Format("                               AND ROUTING_CD = A.ROUTING_CD ) ");
                //sql1 += string.Format("          GROUP BY A.MFG_DATE");
                //sql1 += string.Format("          ,A.WORK_TYPE");
                //sql1 += string.Format("          ,NVL(A.WORK_TEAM,'A')");
                //sql1 += string.Format("          ,A.POC_NO");
                //sql1 += string.Format("          ,A.MILL_NO");
                //sql1 += string.Format("          ,A.HEAT");
                //sql1 += string.Format("          ORDER BY  MFG_DATE DESC, WORK_TYPE, MILL_NO ");
                //sql1 += string.Format("        ) X ");
            }

            //조업정보---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP2")
            {
                sql1 = string.Format("  SELECT ROWNUM AS L_NO, ");
                sql1 += string.Format("   X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("     SELECT /*+ rule */ TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 8), 'YYYYMMDD'), 'YYYY-MM-DD') AS WORK_DATE , ");
                sql1 += string.Format("       TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 9, 6), 'HH24MISS'), 'HH24:MI:SS' )AS WORK_TIME , ");
                sql1 += string.Format("       B.POC_NO AS POC_NO , ");
                sql1 += string.Format("       A.MILL_NO AS MILL_NO , ");
                sql1 += string.Format("       B.HEAT AS HEAT , ");
                sql1 += string.Format("       B.STEEL AS STEEL , ");
                sql1 += string.Format("       ( ");
                sql1 += string.Format("         SELECT CD_NM ");
                sql1 += string.Format("         FROM TB_CM_COM_CD ");
                sql1 += string.Format("         WHERE CATEGORY = 'STEEL' ");
                sql1 += string.Format("           AND CD_ID = B.STEEL) AS STEEL_NM , ");
                sql1 += string.Format("       B.ITEM AS ITEM , ");
                sql1 += string.Format("       B.ITEM_SIZE AS ITEM_SIZE , ");
                sql1 += string.Format("       NVL(B.LENGTH,'0.0') AS LENGTH , ");
                sql1 += string.Format("       NVL(TOP_ROLL_CUSH_ACTV,'0.0') AS COL_01 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_POS_ACTV,'0.0') AS COL_02 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_POS_SETV,'0.0') AS COL_03 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_ACTV,'0.0') AS COL_04 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_SETV,'0.0') AS COL_05 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_ANGLE_ACTV,'0.0') AS COL_06 ,");
                sql1 += string.Format("       NVL(BOT_ROLL_ANGLE_SETV,'0.0') AS COL_07 ,");
                sql1 += string.Format("       NVL(UP_DOWN_LINEAR_POS_ACTV,'0.0') AS COL_08 ,");
                sql1 += string.Format("       NVL(UP_DOWN_LINEAR_POS_SETV,'0.0') AS COL_09 , ");
                sql1 += string.Format("       NVL(FRONT_LINEAR_POS_SETV,'0.0') AS COL_10 , ");
                sql1 += string.Format("       NVL(FRONT_LINEAR_POS_ACTV,'0.0') AS COL_11 , ");
                sql1 += string.Format("       NVL(REAL_LINEAR_POS_ACTV,'0.0') AS COL_12 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_MTR_RPM_ACTV,'0.0') AS COL_13 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_MTR_HZ_SETV,'0.0') AS COL_14 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_MTR_CURRENT_ACTV,'0.0') AS COL_15 , ");
                sql1 += string.Format("       NVL(TOP_BOT_ROLLER_SPEED_SETV,'0.0') AS COL_16 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_MTR_RPM_ACTV,'0.0') AS COL_17 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_MTR_HZ_SETV,'0.0') AS COL_18 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_MTR_CURRENT_ACTV,'0.0') AS COL_19 , ");
                sql1 += string.Format("       NVL(IN_ROLLDR_HZ_ACTV,'0.0') AS COL_20 , ");
                sql1 += string.Format("       NVL(IN_ROLLDR_HZ_SETV,'0.0') AS COL_21 , ");
                sql1 += string.Format("       NVL(IN_ROLLDR_CURRENT_ACTV,'0.0') AS COL_22 , ");
                sql1 += string.Format("       NVL(ENTRY_ROLLDR_HZ_ACTV,'0.0') AS COL_23 , ");
                sql1 += string.Format("       NVL(ENTRY_ROLLDR_HZ_SETV,'0.0') AS COL_24 , ");
                sql1 += string.Format("       NVL(ENTRY_ROLLDR_CURRENT_ACTV,'0.0') AS COL_25 , ");
                sql1 += string.Format("       NVL(EXIT_ROLLDR_HZ_ACTV,'0.0') AS COL_26 , ");
                sql1 += string.Format("       NVL(EXIT_ROLLDR_HZ_SETV,'0.0') AS COL_27 , ");
                sql1 += string.Format("       NVL(EXIT_ROLLDR_CURRENT_ACTV,'0.0') AS COL_28 ");
                sql1 += string.Format("      FROM TB_STR_OPERINFO_NO3 A , ");
                sql1 += string.Format("       TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("      WHERE A.MILL_NO = B.MILL_NO ");
                sql1 += string.Format("          AND A.WORK_DDTT BETWEEN '{0}' AND '{1}' ", start_date + "000000", end_date + "245959");    //:P_FR_DATE AND :P_TO_DATE
                sql1 += string.Format("          AND    '{0}' = '#3' ", cd_id);
                sql1 += string.Format("          AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);
                sql1 += string.Format("          AND B.HEAT      LIKE '%{0}%' ", txtHeat.Text);
                sql1 += string.Format("          AND B.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("          AND B.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("          AND B.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
                sql1 += string.Format("     UNION ");
                sql1 += string.Format("     SELECT /*+ rule */TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 8), 'YYYYMMDD'), 'YYYY-MM-DD') AS WORK_DATE , ");
                sql1 += string.Format("       TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 9, 6), 'HH24MISS'), 'HH24:MI:SS' )AS WORK_TIME , ");
                sql1 += string.Format("       B.POC_NO , ");
                sql1 += string.Format("       A.MILL_NO , ");
                sql1 += string.Format("       B.HEAT , ");
                sql1 += string.Format("       B.STEEL , ");
                sql1 += string.Format("       ( ");
                sql1 += string.Format("         SELECT CD_NM ");
                sql1 += string.Format("         FROM TB_CM_COM_CD ");
                sql1 += string.Format("         WHERE CATEGORY = 'STEEL' ");
                sql1 += string.Format("           AND CD_ID = B.STEEL) AS STEEL_NM , ");
                sql1 += string.Format("       B.ITEM , ");
                sql1 += string.Format("       B.ITEM_SIZE , ");
                sql1 += string.Format("       NVL(B.LENGTH,'0.0') AS LENGTH , ");
                sql1 += string.Format("       NVL(TOP_ROLL_CUSH_ACTV,'0.0') AS COL_01 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_POS_ACTV,'0.0') AS COL_02 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_POS_SETV,'0.0') AS COL_03 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_ACTV,'0.0') AS COL_04 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_SETV,'0.0') AS COL_05 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_ANGLE_ACTV,'0.0') AS COL_06 ,");
                sql1 += string.Format("       NVL(BOT_ROLL_ANGLE_SETV,'0.0') AS COL_07 ,");
                sql1 += string.Format("       NVL(UP_DOWN_LINEAR_POS_ACTV,'0.0') AS COL_08 ,");
                sql1 += string.Format("       NVL(UP_DOWN_LINEAR_POS_SETV,'0.0') AS COL_09 , ");
                sql1 += string.Format("       NVL(FRONT_LINEAR_POS_SETV,'0.0') AS COL_10 , ");
                sql1 += string.Format("       NVL(FRONT_LINEAR_POS_ACTV,'0.0') AS COL_11 , ");
                sql1 += string.Format("       NVL(REAL_LINEAR_POS_ACTV,'0.0') AS COL_12 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_MTR_RPM_ACTV,'0.0') AS COL_13 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_MTR_HZ_SETV,'0.0') AS COL_14 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_MTR_CURRENT_ACTV,'0.0') AS COL_15 , ");
                sql1 += string.Format("       NVL(TOP_BOT_ROLLER_SPEED_SETV,'0.0') AS COL_16 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_MTR_RPM_ACTV,'0.0') AS COL_17 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_MTR_HZ_SETV,'0.0') AS COL_18 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_MTR_CURRENT_ACTV,'0.0') AS COL_19 , ");
                sql1 += string.Format("       NVL(IN_ROLLDR_HZ_ACTV,'0.0') AS COL_20 , ");
                sql1 += string.Format("       NVL(IN_ROLLDR_HZ_SETV,'0.0') AS COL_21 , ");
                sql1 += string.Format("       NVL(IN_ROLLDR_CURRENT_ACTV,'0.0') AS COL_22 , ");
                sql1 += string.Format("       NVL(ENTRY_ROLLDR_HZ_ACTV,'0.0') AS COL_23 , ");
                sql1 += string.Format("       NVL(ENTRY_ROLLDR_HZ_SETV,'0.0') AS COL_24 , ");
                sql1 += string.Format("       NVL(ENTRY_ROLLDR_CURRENT_ACTV,'0.0') AS COL_25 , ");
                sql1 += string.Format("       NVL(EXIT_ROLLDR_HZ_ACTV,'0.0') AS COL_26 , ");
                sql1 += string.Format("       NVL(EXIT_ROLLDR_HZ_SETV,'0.0') AS COL_27 , ");
                sql1 += string.Format("       NVL(EXIT_ROLLDR_CURRENT_ACTV,'0.0') AS COL_28 ");
                sql1 += string.Format("     FROM TB_STR_OPERINFO_NO2 A , ");
                sql1 += string.Format("       TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("     WHERE    A.MILL_NO = B.MILL_NO ");
                sql1 += string.Format("       AND    '{0}' = '#2' ", cd_id);
                sql1 += string.Format("       AND A.WORK_DDTT BETWEEN '{0}' AND '{1}' ", start_date + "000000", end_date + "245959");    //:P_FR_DATE AND :P_TO_DATE
                sql1 += string.Format("       AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);
                sql1 += string.Format("       AND B.HEAT      LIKE '%{0}%' ", txtHeat.Text);
                sql1 += string.Format("       AND B.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("       AND B.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("       AND B.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
                sql1 += string.Format("     UNION ");
                sql1 += string.Format("     SELECT /*+ rule */TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 8), 'YYYYMMDD'), 'YYYY-MM-DD') AS WORK_DATE , ");
                sql1 += string.Format("       TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 9, 6), 'HH24MISS'), 'HH24:MI:SS' )AS WORK_TIME , ");
                sql1 += string.Format("       B.POC_NO , ");
                sql1 += string.Format("       A.MILL_NO , ");
                sql1 += string.Format("       B.HEAT , ");
                sql1 += string.Format("       B.STEEL , ");
                sql1 += string.Format("       ( ");
                sql1 += string.Format("         SELECT CD_NM ");
                sql1 += string.Format("         FROM TB_CM_COM_CD ");
                sql1 += string.Format("         WHERE CATEGORY = 'STEEL' ");
                sql1 += string.Format("           AND CD_ID = B.STEEL) AS STEEL_NM , ");
                sql1 += string.Format("       B.ITEM , ");
                sql1 += string.Format("       B.ITEM_SIZE , ");
                sql1 += string.Format("       NVL(B.LENGTH,'0.0') AS LENGTH , ");
                sql1 += string.Format("       NVL(TOP_ROLL_GAP_1,'0.0') AS COL_01 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_GAP_2,'0.0') AS COL_02 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_GAP_3,'0.0') AS COL_03 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_GAP_4,'0.0') AS COL_04 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_GAP_5,'0.0') AS COL_05 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_GAP_6,'0.0') AS COL_06 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_1,'0.0') AS COL_07 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_2,'0.0') AS COL_08 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_3,'0.0') AS COL_09 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_4,'0.0') AS COL_10 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_5,'0.0') AS COL_11 , ");
                sql1 += string.Format("       NVL(TOP_ROLL_ANGLE_6,'0.0') AS COL_12 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_ANGLE_1,'0.0') AS COL_13 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_ANGLE_2,'0.0') AS COL_14 , ");
                sql1 += string.Format("       NVL(BOT_ROLL_ANGLE_3,'0.0') AS COL_15 , ");
                sql1 += string.Format("       NVL(THREAD_SPEED,'0.0') AS COL_16 , ");
                sql1 += string.Format("       NVL(MACHINE_SPEED,'0.0') AS COL_17 , ");
                sql1 += string.Format("       NVL(INLET_ROLL_CURRENT,'0.0') AS COL_18 , ");
                sql1 += string.Format("       NVL(MID_ROLL_CURRENT,'0.0') AS COL_19 , ");
                sql1 += string.Format("       NVL(OUTLET_ROLL_CURRENT,'0.0') AS COL_20 , ");
                sql1 += string.Format("           NULL AS COL_21 , ");
                sql1 += string.Format("           NULL AS COL_22 , ");
                sql1 += string.Format("           NULL AS COL_23 , ");
                sql1 += string.Format("           NULL AS COL_24 , ");
                sql1 += string.Format("           NULL AS COL_25 , ");
                sql1 += string.Format("           NULL AS COL_26 , ");
                sql1 += string.Format("           NULL AS COL_27 , ");
                sql1 += string.Format("           NULL AS COL_28 ");
                sql1 += string.Format("     FROM TB_STR_OPERINFO_NO1 A , ");
                sql1 += string.Format("       TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("     WHERE    A.MILL_NO = B.MILL_NO ");
                sql1 += string.Format("       AND    '{0}' = '#1' ", cd_id);
                sql1 += string.Format("       AND A.WORK_DDTT BETWEEN '{0}' AND '{1}' ", start_date + "000000", end_date + "245959");    //:P_FR_DATE AND :P_TO_DATE
                sql1 += string.Format("       AND A.WORK_TYPE   LIKE '%{0}%' ", cd_id2);
                sql1 += string.Format("       AND B.HEAT        LIKE '%{0}%' ", txtHeat.Text);
                sql1 += string.Format("       AND B.STEEL       LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("       AND B.ITEM_SIZE   LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("       AND B.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
                sql1 += string.Format("     ORDER BY 1 DESC, 2 DESC) X  ");
            }
                return sql1;
        }
        #endregion
    }
}
