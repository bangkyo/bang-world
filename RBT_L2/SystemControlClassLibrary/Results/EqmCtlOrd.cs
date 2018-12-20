using ComLib;
using ComLib.clsMgr;
using SystemControlClassLibrary.Popup;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using static SystemControlClassLibrary.CHMRslt;
using System.Drawing;
using C1.Win.C1FlexGrid;
using System.Data.OracleClient;
using System.Collections;
using C1.Win.C1Command;

namespace SystemControlClassLibrary.Results
{
    public partial class EqmCtlOrd : Form
    {
        #region 변수선언
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        DataTable olddt = new DataTable();
        C1FlexGrid selectedgrid = new C1FlexGrid();
        private string sql = "";
        private string line_gp = "";
        private string current_Tab = "";

        private string ownerNM = "";
        private string titleNM = "";

        private List<CHM_item> ChmList;

        private List<EqmCtlGrid> GrdList;
        ArrayList _al = new ArrayList();
        private bool allChecked;
        bool _CanSaveSearchLog = false;
        private string strBefValue;
        private int subtotalNo;
        #endregion

        #region 화면
        public EqmCtlOrd(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

            this.Icon = Properties.Resources.H_child_form_icon;

        }

        private void EqmCtlOrd_Load(object sender, System.EventArgs e)
        {
            TabOpt.SelectedIndex = 0;

            InitControl();

            SetComboBox();

            //ck.line_gp 에 따른 Tab Setup
            SetupTab(ck.Line_gp);

            EventCreate();      //사용자정의 이벤트
            _CanSaveSearchLog = true;

            btnDisplay_Click(null, null);

            //TabP1.TabVisible = false;
        }

        private void SetupTab(string line_gp)
        {
            List<string> tablist = new List<string>();

            switch (line_gp)
            {
                case "#1":
                    tablist.Add("교정정보");
                    tablist.Add("면취정보");
                    //tablist.Add("쇼트정보");
                    //tablist.Add("PRII정보");
                    tablist.Add("NDT정보");
                    //tablist.Add("MPI정보");
                    //tablist.Add("성분분석");
                    //tablist.Add("라벨발행");
                    tablist.Add("바인딩");
                    break;
                case "#2":
                    tablist.Add("교정정보");
                    tablist.Add("면취정보");
                    //tablist.Add("쇼트정보");
                    tablist.Add("PRII정보");
                    tablist.Add("NDT정보");
                    //tablist.Add("MPI정보");
                    //tablist.Add("성분분석");
                    //tablist.Add("라벨발행");
                    tablist.Add("바인딩");
                    break;
                case "#3":
                    tablist.Add("교정정보");
                    tablist.Add("면취정보");
                    tablist.Add("쇼트정보");
                    tablist.Add("PRII정보");
                    tablist.Add("NDT정보");
                    tablist.Add("MPI정보");
                    tablist.Add("성분분석");
                    tablist.Add("라벨발행");
                    tablist.Add("바인딩");
                    break;
            }

            foreach (C1DockingTabPage item in TabOpt.Controls)
            {
                if (tablist.Contains(item.Text.Trim()))
                {
                    item.TabVisible = true;
                }
                else
                { 
                    item.TabVisible = false;
                }
            }

            TabOpt.Indent = 0;
        }

        public class EqmCtlGrid
        {
            string _line_gp = string.Empty;
            string _grd_Name = string.Empty;
            string _tab_Name = string.Empty;
            string _tableName = string.Empty;
            string _send_YN_COL_Name = string.Empty;

            string _keyName = string.Empty;
            string _keyColName = string.Empty;

            public EqmCtlGrid(string line_gp, string grd_Name, string tab_Name, string tableName, string send_YN_Col_Name, string key_Name)
            {
                _line_gp = line_gp;
                _grd_Name = grd_Name;
                _tab_Name = tab_Name;
                _tableName = tableName;
                _send_YN_COL_Name = send_YN_Col_Name;
                _keyName = key_Name;
                _keyColName = key_Name;
            }

            public EqmCtlGrid(string line_gp, string grd_Name, string tab_Name, string tableName, string send_YN_Col_Name, string key_Name, string keyColName)
            {
                _line_gp = line_gp;
                _grd_Name = grd_Name;
                _tab_Name = tab_Name;
                _tableName = tableName;
                _send_YN_COL_Name = send_YN_Col_Name;
                _keyName = key_Name;
                _keyColName = keyColName;
            }


            public string Line_gp { get { return _line_gp; } set { _line_gp = value; } }
            public string Grd_Name { get { return _grd_Name; } set { _grd_Name = value; } }
            public string Tab_Name { get { return _tab_Name; } set { _tab_Name = value; } }
            public string Table_Name { get { return _tableName; } set { _tableName = value; } }
            public string SEND_YN_COL_Name { get { return _send_YN_COL_Name; } set { _send_YN_COL_Name = value; } }
            public string KeyName { get { return _keyName; } set { _keyName = value; } }
            public string KeyColName { get { return _keyColName; } set { _keyColName = value; } }


        }

        private void InitControl()
        {
            GrdList = new List<EqmCtlGrid>();

            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(label1);
            cs.InitLabel(label2);
            cs.InitLabel(label3);
            cs.InitLabel(label5);
            cs.InitLabel(label6);
            cs.InitLabel(label7);
            cs.InitLabel(label8);

            cs.InitCombo(cboLine_GP, StringAlignment.Center);

            cs.InitTextBox(txtPOC);
            cs.InitTextBox(txtHEAT);
            cs.InitTextBox(txtItemSize);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(gangjong_Nm_tb);

            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;
            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            cs.InitButton(btnPLCSend);
            cs.InitButton(btnBNDSend);
            cs.InitButton(btnSave);
            



            cs.InitTab(TabOpt);

            //일자 데이터 default 값 적용 
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;

            Init_grdMain1();
            Init_grdMain1_1();
            Init_grdMain2();
            Init_grdMain2_1();
            Init_grdMain3();
            Init_grdMain4();
            Init_grdMain5();
            Init_grdMain5_1();
            Init_grdMain6();
            Init_grdMain7();
            Init_grdMain8();
            Init_grdMain9();
            Init_grdMain9_1();

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
            cd.SetCombo(cboLine_GP, "LINE_GP", false, ck.Line_gp);
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
            selectedgrid = Check_Line_Matching_Grid(selectedgrid);
            subtotalNo = 0;

            try
            {
                //쿼리 가져욤
                sql = QuerySetup();

                olddt = cd.FindDataTable(sql);
                Cursor.Current = Cursors.WaitCursor;
                selectedgrid.SetDataBinding(olddt, null, true);
                Cursor.Current = Cursors.Default;
                if (selectedgrid == grdMain7)
                {
                    if (olddt.Rows.Count > 0)
                    {
                        CheckMinMax(selectedgrid);
                    }
                }

                if (selectedgrid == grdMain9_1)
                {
                    UpdateTotals(selectedgrid);
                }

                ////교정 면취만 AUTOSIZE
                //if (current_Tab == "TabP1" || current_Tab == "TabP2")
                //{
                //    grid.AutoSizeCols();
                //}
                selectedgrid.AutoSizeCols();
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), olddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }

            
        }

        private void UpdateTotals(C1FlexGrid selectedgrid)
        {
            int subtotalrowindex = 0;

            // clear existing totals
            selectedgrid.Subtotal(AggregateEnum.Clear);

            selectedgrid.Subtotal(AggregateEnum.Sum, subtotalrowindex, -1, selectedgrid.Cols["PDA_SCAN_PCS"].Index, "합계");
            selectedgrid.Subtotal(AggregateEnum.Sum, subtotalrowindex, -1, selectedgrid.Cols["BND_ORD_PCS"].Index, "합계");
            //selectedgrid.Subtotal(AggregateEnum.Sum, subtotalrowindex, -1, selectedgrid.Cols["PCS"].Index, "합계");

            AddSubtotalNo();
            selectedgrid.Rows.Frozen = selectedgrid.Rows.Fixed ;
            //grdMain.Subtotal(AggregateEnum.Average, 1, -1, grdMain.Cols["THEORY_WGT"].Index, "평균");

            //grdMain.AutoSizeCols();

            //grdMain.Rows.Fixed = GetAvailMinRow();
        }

        private int GetAvailMinRow(C1FlexGrid grid)
        {
            return (grid.Rows.Fixed + subtotalNo);
        }

        private void AddSubtotalNo()
        {
            ++subtotalNo;
        }




        private void CheckMinMax(C1FlexGrid grd)
        {
            CellRange rg;

            for (int row = 2; row < grd.Rows.Count; row++)
            {
                foreach (Column col in grd.Cols)
                {

                    foreach (CHM_item item in ChmList)
                    {
                        if (col.Name == item.Value_Cols_NM)
                        {

                            //NG COLUMN이 숨겨져있음. 디자인모드에서
                            rg = grd.GetCellRange(row, grd.Cols[item.Value_Cols_NM].Index);
                            //if (grd.GetData(row, item.Value_NG_NM).ToString() != "OK")
                            //{
                            //    rg.StyleNew.ForeColor = Color.Red;
                            //}

                            rg = grd.GetCellRange(row, grd.Cols[item.Min_Cols_NM].Index);
                            rg.StyleNew.BackColor = Color.LightGray;

                            rg = grd.GetCellRange(row, grd.Cols[item.Max_Cols_NM].Index);
                            rg.StyleNew.BackColor = Color.LightGray;



                        }
                    }
                }

            }

        }
        #endregion
        
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

            if (current_Tab == "TabP1")
            {
                selectedgrid = grdMain1;
                dlg.FileName = "설비제어지시_교정" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP2")
            {
                selectedgrid = grdMain2;
                dlg.FileName = "설비제어지시_면취" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP3")
            {
                selectedgrid = grdMain3;
                dlg.FileName = "설비제어지시_쇼트" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP4")
            {
                selectedgrid = grdMain4;
                dlg.FileName = "설비제어지시_PRII" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP5")
            {
                selectedgrid = grdMain5_1;
                dlg.FileName = "설비제어지시_NDT" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP6")
            {
                selectedgrid = grdMain6;
                dlg.FileName = "설비제어지시_MPI" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP7")
            {
                selectedgrid = grdMain7;
                dlg.FileName = "설비제어지시_성분분석" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP8")
            {
                selectedgrid = grdMain8;
                dlg.FileName = "설비제어지시_라벨발행" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP9")
            {
                selectedgrid = grdMain9_1;
                dlg.FileName = "설비제어지시_바인딩" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedgrid.SaveGrid(dlg.FileName, C1.Win.C1FlexGrid.FileFormatEnum.Excel, C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells);

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
            // default line combobox enable
            //LinsSetAndEnable(ck.Line_gp, true);

            switch (TabOpt.SelectedIndex)
            {
                case 0:
                    current_Tab = "TabP1";
                    break;
                case 1:
                    current_Tab = "TabP2";
                    break;
                case 2:
                    current_Tab = "TabP3";
                    break;
                case 3:
                    current_Tab = "TabP4";
                    break;
                case 4:
                    current_Tab = "TabP5";
                    break;
                case 5:
                    current_Tab = "TabP6";
                    break;
                case 6:
                    current_Tab = "TabP7";
                    break;
                case 7:
                    current_Tab = "TabP8";
                    break;
                case 8:
                    current_Tab = "TabP9";
                    break;
            }

            btnDisplay_Click(null, null);
            allChecked = false;
        }

        private void LinsSetAndEnable(string _line_gp, bool _enable)
        {
            foreach (var item in cboLine_GP.Items)
            {
                if (((DictionaryList)item).fnValue == _line_gp)
                {
                    cboLine_GP.SelectedIndex = cboLine_GP.Items.IndexOf(item);
                }
            }
            cboLine_GP.Enabled = _enable;
        }


        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;

            ck.Line_gp = line_gp;

            //Check_Line_Matching_Grid(selectedgrid);

            SetupTab(line_gp);

            btnDisplay_Click(null,null);
        }


        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
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
            this.gangjong_id_tb.LostFocus += new EventHandler(gangjong_id_tb_LostFocus);            //강종ID
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

        //탭에 따른 라인 구분↓
        private C1FlexGrid Check_Line_Matching_Grid(C1FlexGrid grd)
        {
            //교정
            if (current_Tab == "TabP1" && cboLine_GP.Text == "#1라인")
            {
                selectedgrid = grdMain1;
                grdMain1.BringToFront();
            }
            if (current_Tab == "TabP1" && (cboLine_GP.Text == "#2라인" || cboLine_GP.Text == "#3라인"))
            {
                selectedgrid = grdMain1_1;
                grdMain1_1.BringToFront();
            }
            //면취
            if (current_Tab == "TabP2" && cboLine_GP.Text == "#1라인")
            {
                selectedgrid = grdMain2;
                grdMain2.BringToFront();
            }
            if (current_Tab == "TabP2" && (cboLine_GP.Text == "#2라인" || cboLine_GP.Text == "#3라인"))
            {
                selectedgrid = grdMain2_1;
                grdMain2_1.BringToFront();
            }
            //쇼트
            if (current_Tab == "TabP3") selectedgrid = grdMain3;
            //PRII
            if (current_Tab == "TabP4") selectedgrid = grdMain4;
            //NDT
            //if (current_Tab == "TabP5") selectedgrid = grdMain5_1;
            if (current_Tab == "TabP5" && (cboLine_GP.Text == "#1라인" || cboLine_GP.Text == "#2라인"))
            {
                selectedgrid = grdMain5;
                grdMain5.BringToFront();
            }
            if (current_Tab == "TabP5" && cboLine_GP.Text == "#3라인")
            {
                selectedgrid = grdMain5_1;
                grdMain5_1.BringToFront();
            }

            //MPI정보
            if (current_Tab == "TabP6") selectedgrid = grdMain6;
            //성분분석
            if (current_Tab == "TabP7") selectedgrid = grdMain7;
            //라벨발행
            if (current_Tab == "TabP8") selectedgrid = grdMain8;
            //바인딩
            if (current_Tab == "TabP9" && (cboLine_GP.Text == "#1라인" || cboLine_GP.Text == "#2라인"))
            {
                selectedgrid = grdMain9;
                grdMain9.BringToFront();
            }
            if (current_Tab == "TabP9" && cboLine_GP.Text == "#3라인" )
            {
                selectedgrid = grdMain9_1;
                grdMain9_1.BringToFront();
            }

            return selectedgrid;
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

        private void SettingStyleAndCheckEnableSelCols(C1FlexGrid grd)
        {

            grd.AllowEditing = true;
            for (int col = 0; col < grd.Cols.Count; col++)
            {
                grd.Cols[col].AllowEditing = false;
            }

            if (grd.Cols["SEL"] != null)
            {
                grd.Cols["SEL"].AllowEditing = true;

                var crCellRange = grd.GetCellRange(0, grd.Cols["SEL"].Index, grd.Rows.Fixed - 1, grd.Cols["SEL"].Index);
                crCellRange.Style = grd.Styles["ModifyStyle"];

            }
        }

        /// <summary>
        /// 쿼리의 컬럼이 1,2,3라인이 동일해야함으로 그리드 컬럼 또한 동일해야함. 
        /// </summary>
        #region Init_grdMain1 교정
        private void Init_grdMain1()
        {
            grdMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            clsStyle.Style.InitGrid_search(grdMain1);

            grdMain1.Paint += grd_Paint;

            SettingStyleAndCheckEnableSelCols(grdMain1);


            grdMain1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain1.AutoResize = true;
            //grdMain1.AllowEditing = true;

            #region Caption
            grdMain1.Cols["L_NO"].Caption = "NO";
            grdMain1.Cols["SEL"].Caption = "선택";
            grdMain1.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain1.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain1.Cols["KEY_NO"].Visible = false;
            grdMain1.Cols["MILL_NO"].Visible = false;
            grdMain1.Cols["POC_NO"].Caption = "POC";
            grdMain1.Cols["HEAT"].Caption = "HEAT";
            grdMain1.Cols["STEEL"].Caption = "강종";
            grdMain1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain1.Cols["ITEM"].Caption = "품목";
            grdMain1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain1.Cols["LENGTH"].Visible = false;
            grdMain1.Cols["COL_00"].Visible = false;
            grdMain1.Cols["COL_01"].Caption = "상롤GAP1";
            grdMain1.Cols["COL_02"].Caption = "상롤GAP2";
            grdMain1.Cols["COL_03"].Caption = "상롤GAP3";
            grdMain1.Cols["COL_04"].Caption = "상롤GAP4";
            grdMain1.Cols["COL_05"].Caption = "상롤GAP5";
            grdMain1.Cols["COL_06"].Caption = "상롤GAP6";
            grdMain1.Cols["COL_07"].Caption = "상롤각도1";
            grdMain1.Cols["COL_08"].Caption = "상롤각도2";
            grdMain1.Cols["COL_09"].Caption = "상롤각도3";
            grdMain1.Cols["COL_10"].Caption = "상롤각도4";
            grdMain1.Cols["COL_11"].Caption = "상롤각도5";
            grdMain1.Cols["COL_12"].Caption = "상롤각도6";
            grdMain1.Cols["COL_13"].Caption = "하롤각도1";
            grdMain1.Cols["COL_14"].Caption = "하롤각도2";
            grdMain1.Cols["COL_15"].Caption = "하롤각도3";
            grdMain1.Cols["COL_16"].Caption = "THREAD SPEED";
            grdMain1.Cols["COL_17"].Caption = "MACHINE SPEED";
            grdMain1.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain1.Cols["SEL"].Width = cs.Sel_Width;
            grdMain1.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;         //변경전 110;
            grdMain1.Cols["ORD_TIME"].Width = cs.Date_8_Width;              //변경전 100;
            grdMain1.Cols["POC_NO"].Width = cs.POC_NO_Width;                //변경전 100;
            grdMain1.Cols["HEAT"].Width = cs.HEAT_Width;                    //변경전 100;
            grdMain1.Cols["STEEL"].Width = cs.STEEL_Width + 20;             //변경전 60;
            grdMain1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;       //변경전 100;
            grdMain1.Cols["ITEM"].Width = cs.ITEM_Width + 15;               //변경전 60;
            grdMain1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35;     //변경전 80;
            grdMain1.Cols["COL_01"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_02"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_03"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_04"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_05"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_06"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_07"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_08"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_09"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_10"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_11"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_12"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_13"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_14"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_15"].Width = cs.Short_Value_Width;
            grdMain1.Cols["COL_16"].Width = cs.Longer_Value_Width;
            grdMain1.Cols["COL_17"].Width = cs.Longer_Value_Width;
            grdMain1.Cols["SEND_YN"].Width = cs.SEND_YN_Width;

            grdMain1.Cols["COL_00"].Visible = false;
            #endregion Width

            #region TextAlign
            grdMain1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain1.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain1.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain1.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain1.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_16"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["COL_17"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#1", grdMain1.Name, grdMain1.Parent.Name, "TB_STR_PLC_ORD_NO1", "SEND_YN", "POC_NO"));

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain1, lbSel, 0, grdMain1.Cols["SEL"].Index));

        }

        private void grd_Paint(object sender, PaintEventArgs e)
        {
            foreach (Order.CrtInOrdCre.HostedControl hosted in _al)
                hosted.UpdatePosition();
        }


        private void SEL_Click(object sender, EventArgs e)
        {

            var grd = selectedgrid as C1FlexGrid;

            int Rowindex = grd.Rows.Fixed;

            if (allChecked)
            {
                for (int rowCnt = Rowindex; rowCnt < grd.Rows.Count; rowCnt++)
                {
                    grd.SetData(rowCnt, "SEL", false);
                    //SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = Rowindex; rowCnt < grd.Rows.Count; rowCnt++)
                {
                    grd.SetData(rowCnt, "SEL", true);
                    //SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = true;

            }
        }

        #endregion Init_grdMain1

        #region Init_grdMain1_1 교정
        private void Init_grdMain1_1()
        {
            clsStyle.Style.InitGrid_search(grdMain1_1);

            SettingStyleAndCheckEnableSelCols(grdMain1_1);

            grdMain1_1.AutoResize = true;
            //grdMain1_1.AllowEditing = true;
            grdMain1_1.Paint += grd_Paint;

            #region Caption
            grdMain1_1.Cols["L_NO"].Caption = "NO";
            grdMain1_1.Cols["SEL"].Caption = "선택";
            grdMain1_1.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain1_1.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain1_1.Cols["KEY_NO"].Visible = false;
            grdMain1_1.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain1_1.Cols["POC_NO"].Caption = "POC";
            grdMain1_1.Cols["HEAT"].Caption = "HEAT";
            grdMain1_1.Cols["STEEL"].Caption = "강종";
            grdMain1_1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain1_1.Cols["ITEM"].Caption = "품목";
            grdMain1_1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain1_1.Cols["LENGTH"].Caption = "길이(m)";
            grdMain1_1.Cols["COL_00"].Caption = "본수";
            grdMain1_1.Cols["COL_01"].Caption = "상롤위치";
            grdMain1_1.Cols["COL_02"].Caption = "상롤각도";
            grdMain1_1.Cols["COL_03"].Caption = "하롤각도";
            grdMain1_1.Cols["COL_04"].Caption = "상하가이드의위치";
            grdMain1_1.Cols["COL_05"].Caption = "전후면의가이드위치";
            grdMain1_1.Cols["COL_06"].Caption = "진입롤모터주파수(Hz)";
            grdMain1_1.Cols["COL_07"].Caption = "상롤모터주파수(Hz)";
            grdMain1_1.Cols["COL_08"].Caption = "출구롤모터주파수(Hz)";
            grdMain1_1.Cols["COL_09"].Visible = false;
            grdMain1_1.Cols["COL_10"].Visible = false;
            grdMain1_1.Cols["COL_11"].Visible = false;
            grdMain1_1.Cols["COL_12"].Visible = false;
            grdMain1_1.Cols["COL_13"].Visible = false;
            grdMain1_1.Cols["COL_14"].Visible = false;
            grdMain1_1.Cols["COL_15"].Visible = false;
            grdMain1_1.Cols["COL_16"].Visible = false;
            grdMain1_1.Cols["COL_17"].Visible = false;
            grdMain1_1.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region width
            grdMain1_1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain1_1.Cols["SEL"].Width = cs.Sel_Width;
            grdMain1_1.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;           //변경전 110;
            grdMain1_1.Cols["ORD_TIME"].Width = cs.Date_8_Width;                //변경전 100;
            grdMain1_1.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;           //변경전 140;
            grdMain1_1.Cols["POC_NO"].Width = cs.POC_NO_Width;                  //변경전 100;
            grdMain1_1.Cols["HEAT"].Width = cs.HEAT_Width;                      //변경전 100;
            grdMain1_1.Cols["STEEL"].Width = cs.STEEL_Width + 20;               //변경전 60;
            grdMain1_1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;         //변경전 100;
            grdMain1_1.Cols["ITEM"].Width = cs.ITEM_Width + 15;                 //변경전 60;
            grdMain1_1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35;       //변경전 80;
            grdMain1_1.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain1_1.Cols["COL_00"].Width = cs.PCS_Width + 30;                   // 변경전 80;
            grdMain1_1.Cols["COL_01"].Width = cs.Short_Value_Width;             // 변경전 90;
            grdMain1_1.Cols["COL_02"].Width = cs.Short_Value_Width;             // 변경전 90;
            grdMain1_1.Cols["COL_03"].Width = cs.Short_Value_Width;             // 변경전 90;
            grdMain1_1.Cols["COL_04"].Width = cs.Longer_Value_Width ;           // 변경전 150;
            grdMain1_1.Cols["COL_05"].Width = cs.Longer_Value_Width ;           // 변경전 150;
            grdMain1_1.Cols["COL_06"].Width = cs.Longer_Value_Width + 30;       // 변경전 180;
            grdMain1_1.Cols["COL_07"].Width = cs.Longer_Value_Width + 30;       // 변경전 180;
            grdMain1_1.Cols["COL_08"].Width = cs.Longer_Value_Width + 30;       // 변경전 180;
            grdMain1_1.Cols["COL_09"].Width = 0;
            grdMain1_1.Cols["COL_10"].Width = 0;
            grdMain1_1.Cols["COL_11"].Width = 0;
            grdMain1_1.Cols["COL_12"].Width = 0;
            grdMain1_1.Cols["COL_13"].Width = 0;
            grdMain1_1.Cols["COL_14"].Width = 0;
            grdMain1_1.Cols["COL_15"].Width = 0;
            grdMain1_1.Cols["SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion width

            #region TextAlign

            grdMain1_1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain1_1.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain1_1.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain1_1.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;

            grdMain1_1.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain1_1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain1_1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain1_1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain1_1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain1_1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain1_1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain1_1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain1_1.Cols["COL_00"].TextAlign = cs.PCS_TextAlign;
            grdMain1_1.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain1_1.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;

            #endregion TextAlign

            grdMain1_1.Dock = System.Windows.Forms.DockStyle.Fill;
            grdMain1_1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            GrdList.Add(new EqmCtlGrid("#2", grdMain1_1.Name, grdMain1_1.Parent.Name, "TB_STR_PLC_ORD_NO2", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#3", grdMain1_1.Name, grdMain1_1.Parent.Name, "TB_STR_PLC_ORD_NO3", "SEND_YN", "MILL_NO"));

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain1_1, lbSel, 0, grdMain1_1.Cols["SEL"].Index));
        }
        #endregion Init_grdMain1_1

        #region Init_grdMain2 면취정보
        private void Init_grdMain2()
        {
            clsStyle.Style.InitGrid_search(grdMain2);
            
            grdMain2.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain2);

            #region Caption
            grdMain2.Cols["L_NO"].Caption = "NO";
            grdMain2.Cols["SEL"].Caption = "선택";
            grdMain2.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain2.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain2.Cols["KEY_NO"].Visible = false;
            grdMain2.Cols["MILL_NO"].Visible = false;
            grdMain2.Cols["POC_NO"].Caption = "POC";
            grdMain2.Cols["HEAT"].Caption = "HEAT";
            grdMain2.Cols["STEEL"].Caption = "강종";
            grdMain2.Cols["STEEL_NM"].Caption = "강종명";
            grdMain2.Cols["ITEM"].Caption = "품목";
            grdMain2.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain2.Cols["LENGTH"].Visible = false;
            grdMain2.Cols["PCS"].Visible = false;
            grdMain2.Cols["COL_01"].Visible = false;
            grdMain2.Cols["COL_02"].Visible = false;
            grdMain2.Cols["COL_03"].Caption = "스크류1";
            grdMain2.Cols["COL_04"].Caption = "스크류2";
            grdMain2.Cols["COL_05"].Caption = "롤러테이블1";
            grdMain2.Cols["COL_06"].Caption = "롤러테이블2";
            grdMain2.Cols["COL_07"].Caption = "롤러테이블EXIT";
            grdMain2.Cols["COL_08"].Visible = false;
            grdMain2.Cols["COL_09"].Visible = false;
            grdMain2.Cols["COL_10"].Visible = false;
            grdMain2.Cols["COL_11"].Visible = false;
            grdMain2.Cols["COL_12"].Visible = false;
            grdMain2.Cols["COL_13"].Visible = false;
            grdMain2.Cols["COL_14"].Visible = false;
            grdMain2.Cols["COL_15"].Visible = false;
            grdMain2.Cols["COL_16"].Visible = false;
            grdMain2.Cols["SEND_YN"].Caption = "송신여부";
            grdMain2.Cols["SERVO_POS_SP_1"].Visible = false;
            grdMain2.Cols["SERVO_POS_SP_2"].Visible = false;
            #endregion Caption

            #region Width
            grdMain2.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain2.Cols["SEL"].Width = cs.Sel_Width;
            grdMain2.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain2.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain2.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain2.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain2.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain2.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain2.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain2.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain2.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain2.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain2.Cols["PCS"].Width = cs.PCS_Width + 30;             //변경전 80;
            grdMain2.Cols["COL_01"].Width = cs.Short_Value_Width;       //변경전 90;
            grdMain2.Cols["COL_02"].Width = cs.Short_Value_Width;       //변경전 90;
            grdMain2.Cols["COL_03"].Width = cs.Short_Value_Width;       //변경전 90;
            grdMain2.Cols["COL_04"].Width = cs.Short_Value_Width;       //변경전 90;
            grdMain2.Cols["COL_05"].Width = cs.Middle_Value_Width;      //변경전 120;
            grdMain2.Cols["COL_06"].Width = cs.Middle_Value_Width;      //변경전 120;
            grdMain2.Cols["COL_07"].Width = cs.Middle_Value_Width;      //변경전 120;
            grdMain2.Cols["COL_08"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_09"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_10"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_11"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_12"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_13"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_14"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_15"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["COL_16"].Width = cs.Short_Value_Width + 10;  //변경전 100;
            grdMain2.Cols["SEND_YN"].Width = cs.SEND_YN_Width;  
            #endregion Width

            #region TextAlign
            grdMain2.Rows[0].TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain2.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain2.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain2.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain2.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain2.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain2.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain2.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain2.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain2.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain2.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain2.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain2.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain2.Cols["PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["COL_16"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            grdMain2.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain2.Dock = System.Windows.Forms.DockStyle.Fill;

            GrdList.Add(new EqmCtlGrid("#1", grdMain2.Name, grdMain2.Parent.Name, "TB_CHF_PLC_ORD_NO1", "SEND_YN", "POC_NO"));

            grdMain2.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain2, lbSel, 0, grdMain2.Cols["SEL"].Index));
        }
        #endregion Init_grdMain2

        #region Init_grdMain2_1 면취정보 2
        private void Init_grdMain2_1()
        {
            grdMain2_1.Dock = System.Windows.Forms.DockStyle.Fill;
            clsStyle.Style.InitGrid_search(grdMain2_1);
            grdMain2_1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain2_1.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain2_1);

            #region Caption
            grdMain2_1.Cols["L_NO"].Caption = "NO";
            grdMain2_1.Cols["SEL"].Caption = "선택";
            grdMain2_1.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain2_1.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain2_1.Cols["KEY_NO"].Visible = false;
            grdMain2_1.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain2_1.Cols["POC_NO"].Caption = "POC";
            grdMain2_1.Cols["HEAT"].Caption = "HEAT";
            grdMain2_1.Cols["STEEL"].Caption = "강종";
            grdMain2_1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain2_1.Cols["ITEM"].Caption = "품목";
            grdMain2_1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain2_1.Cols["LENGTH"].Caption = "길이(m)";
            grdMain2_1.Cols["PCS"].Caption = "본수";
            grdMain2_1.Cols["COL_01"].Caption = "키커사이즈조정1";
            grdMain2_1.Cols["COL_02"].Caption = "롤러컨베이어주파수고속1(Hz)";
            grdMain2_1.Cols["COL_03"].Caption = "롤러컨베이어주파수저속1(Hz)";
            grdMain2_1.Cols["COL_04"].Caption = "스크류구동주파수1(Hz)";
            grdMain2_1.Cols["COL_05"].Caption = "그라인딩구동주파수1(Hz)";
            grdMain2_1.Cols["COL_06"].Caption = "스토퍼1";
            grdMain2_1.Cols["COL_07"].Caption = "가이드1";
            grdMain2_1.Cols["SERVO_POS_SP_1"].Caption = "서보위치이동속도1"; 
            grdMain2_1.Cols["COL_08"].Caption = "서보위치1";
            grdMain2_1.Cols["COL_09"].Caption = "키커사이즈조정2";
            grdMain2_1.Cols["COL_10"].Caption = "롤러컨베이어주파수고속2(Hz)";
            grdMain2_1.Cols["COL_11"].Caption = "롤러컨베이어주파수저속2(Hz)";
            grdMain2_1.Cols["COL_12"].Caption = "스크류구동주파수2(Hz)";
            grdMain2_1.Cols["COL_13"].Caption = "그라인딩구동주파수2(Hz)";
            grdMain2_1.Cols["COL_14"].Caption = "스토퍼2";
            grdMain2_1.Cols["COL_15"].Caption = "가이드2";
            grdMain2_1.Cols["COL_16"].Caption = "서보위치2";
            grdMain2_1.Cols["SERVO_POS_SP_2"].Caption = "서보위치이동속도2";
            grdMain2_1.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain2_1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain2_1.Cols["SEL"].Width = cs.Sel_Width;
            grdMain2_1.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain2_1.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain2_1.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain2_1.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain2_1.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain2_1.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain2_1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain2_1.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain2_1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain2_1.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain2_1.Cols["PCS"].Width = cs.PCS_Width + 30;             //변경전 80;
            grdMain2_1.Cols["COL_01"].Width = cs.Longer_Value_Width;         // 변경전 150;
            grdMain2_1.Cols["COL_02"].Width = cs.Longest_Value_Width + 20;   // 변경전 240;
            grdMain2_1.Cols["COL_03"].Width = cs.Longest_Value_Width + 20;   // 변경전 240;
            grdMain2_1.Cols["COL_04"].Width = cs.Longer_Value_Width + 50;    // 변경전 200;
            grdMain2_1.Cols["COL_05"].Width = cs.Longer_Value_Width + 50;    // 변경전 200;
            grdMain2_1.Cols["COL_06"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["COL_07"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["SERVO_POS_SP_1"].Width = cs.Longer_Value_Width;
            grdMain2_1.Cols["COL_08"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["COL_09"].Width = cs.Longer_Value_Width;         // 변경전 150;
            grdMain2_1.Cols["COL_10"].Width = cs.Longest_Value_Width + 20;   // 변경전 240;
            grdMain2_1.Cols["COL_11"].Width = cs.Longest_Value_Width + 20;   // 변경전 240;
            grdMain2_1.Cols["COL_12"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["COL_13"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["COL_14"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["COL_15"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["COL_16"].Width = cs.Short_Value_Width;          // 변경전 90;
            grdMain2_1.Cols["SERVO_POS_SP_2"].Width = cs.Longer_Value_Width;
            grdMain2_1.Cols["SEND_YN"].Width = cs.SEND_YN_Width;

            #endregion Width

            #region TextAlign

            grdMain2_1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain2_1.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain2_1.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain2_1.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain2_1.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain2_1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain2_1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain2_1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain2_1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain2_1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain2_1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain2_1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain2_1.Cols["PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain2_1.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["SERVO_POS_SP_1"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_16"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["SERVO_POS_SP_2"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#2", grdMain2_1.Name, grdMain2_1.Parent.Name, "TB_CHF_PLC_ORD_NO2", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#3", grdMain2_1.Name, grdMain2_1.Parent.Name, "TB_CHF_PLC_ORD_NO3", "SEND_YN", "MILL_NO"));

            grdMain2_1.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain2_1, lbSel, 0, grdMain2_1.Cols["SEL"].Index));

        }
        #endregion 

        #region Init_grdMain3 쇼트
        private void Init_grdMain3()
        {
            clsStyle.Style.InitGrid_search(grdMain3);
            grdMain3.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain3.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain3);

            #region Caption
            grdMain3.Cols["L_NO"].Caption = "NO";
            grdMain3.Cols["SEL"].Caption = "선택";
            grdMain3.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain3.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain3.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain3.Cols["POC_NO"].Caption = "POC";
            grdMain3.Cols["HEAT"].Caption = "HEAT";
            grdMain3.Cols["STEEL"].Caption = "강종";
            grdMain3.Cols["STEEL_NM"].Caption = "강종명";
            grdMain3.Cols["ITEM"].Caption = "품목";
            grdMain3.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain3.Cols["LENGTH"].Caption = "길이(m)";
            grdMain3.Cols["PCS"].Caption = "본수";
            grdMain3.Cols["TRANS_ROLLER_RPM"].Caption = "이송롤러회전수";
            grdMain3.Cols["IMPELLER_FREQUENCY"].Caption = "임펠라주파수";
            grdMain3.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain3.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain3.Cols["SEL"].Width = cs.Sel_Width;
            grdMain3.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain3.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain3.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain3.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain3.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain3.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain3.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain3.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain3.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain3.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain3.Cols["PCS"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["TRANS_ROLLER_RPM"].Width = cs.Longer_Value_Width;
            grdMain3.Cols["IMPELLER_FREQUENCY"].Width = cs.Longer_Value_Width;
            grdMain3.Cols["SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign

            grdMain3.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain3.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain3.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain3.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain3.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain3.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain3.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain3.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain3.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain3.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain3.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain3.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain3.Cols["PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain3.Cols["TRANS_ROLLER_RPM"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain3.Cols["IMPELLER_FREQUENCY"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain3.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#1", grdMain3.Name, grdMain3.Parent.Name, "TB_SHT_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#2", grdMain3.Name, grdMain3.Parent.Name, "TB_SHT_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#3", grdMain3.Name, grdMain3.Parent.Name, "TB_SHT_PLC_ORD", "SEND_YN", "MILL_NO"));

            grdMain3.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain3, lbSel, 0, grdMain3.Cols["SEL"].Index));

        }
        #endregion 

        #region Init_grdMain4 PRII
        private void Init_grdMain4()
        {
            clsStyle.Style.InitGrid_search(grdMain4);
            grdMain4.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain4.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain4);

            #region Caption
            grdMain4.Cols["L_NO"].Caption = "NO";
            grdMain4.Cols["SEL"].Caption = "선택";
            grdMain4.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain4.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain4.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain4.Cols["POC_NO"].Caption = "POC";
            grdMain4.Cols["HEAT"].Caption = "HEAT";
            grdMain4.Cols["STEEL"].Caption = "강종";
            grdMain4.Cols["STEEL_NM"].Caption = "강종명";
            grdMain4.Cols["ITEM"].Caption = "품목";
            grdMain4.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain4.Cols["LENGTH"].Caption = "길이(m)";
            grdMain4.Cols["PCS"].Caption = "본수";
            grdMain4.Cols["SURFACE_LEVEL_NM"].Caption = "표면등급";
            grdMain4.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain4.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain4.Cols["SEL"].Width = cs.Sel_Width;
            grdMain4.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain4.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain4.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain4.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain4.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain4.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain4.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain4.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain4.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain4.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain4.Cols["PCS"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["SURFACE_LEVEL_NM"].Width = cs.Middle_Value_Width;
            grdMain4.Cols["SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign
            grdMain4.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain4.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain4.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain4.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain4.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain4.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain4.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain4.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain4.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain4.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain4.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain4.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain4.Cols["PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain4.Cols["SURFACE_LEVEL_NM"].TextAlign = cs.VALUE_LEFT_TextAlign;
            grdMain4.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#1", grdMain4.Name, grdMain4.Parent.Name, "TB_PRII_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#2", grdMain4.Name, grdMain4.Parent.Name, "TB_PRII_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#3", grdMain4.Name, grdMain4.Parent.Name, "TB_PRII_PLC_ORD", "SEND_YN", "MILL_NO"));

            grdMain4.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain4, lbSel, 0, grdMain4.Cols["SEL"].Index));

        }
        #endregion Init_grdMain4

        #region Init_grdMain5 NDT정보
        private void Init_grdMain5()
        {
            clsStyle.Style.InitGrid_search(grdMain5);
            grdMain5.Dock = DockStyle.Fill;
            grdMain5.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain5.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain5);

            #region Caption
            grdMain5.Cols["L_NO"].Caption = "NO";
            grdMain5.Cols["SEL"].Caption = "선택";
            grdMain5.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain5.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain5.Cols["KEY_NO"].Visible = false;
            grdMain5.Cols["MILL_NO"].Visible = false;
            grdMain5.Cols["POC_NO"].Caption = "POC";
            grdMain5.Cols["HEAT"].Caption = "HEAT";
            grdMain5.Cols["STEEL"].Caption = "강종";
            grdMain5.Cols["STEEL_NM"].Caption = "강종명";
            grdMain5.Cols["ITEM"].Caption = "품목";
            grdMain5.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain5.Cols["LENGTH"].Visible = false;
            grdMain5.Cols["PCS"].Visible = false;
            grdMain5.Cols["SURFACE_LEVEL_NM"].Caption = "표면등급";
            grdMain5.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain5.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain5.Cols["SEL"].Width = cs.Sel_Width;
            grdMain5.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain5.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain5.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain5.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain5.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain5.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain5.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain5.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain5.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain5.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain5.Cols["PCS"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["SURFACE_LEVEL_NM"].Width = cs.Middle_Value_Width;
            grdMain5.Cols["SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign
            grdMain5.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain5.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain5.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain5.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain5.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain5.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain5.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain5.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain5.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain5.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain5.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain5.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain5.Cols["PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain5.Cols["SURFACE_LEVEL_NM"].TextAlign = cs.VALUE_LEFT_TextAlign;
            grdMain5.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#1", grdMain5.Name, grdMain5.Parent.Name, "TB_NDT_PLC_ORD_NO1", "SEND_YN", "POC_NO"));
            GrdList.Add(new EqmCtlGrid("#2", grdMain5.Name, grdMain5.Parent.Name, "TB_NDT_PLC_ORD_NO2", "SEND_YN", "POC_NO"));
            

            grdMain5.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain5, lbSel, 0, grdMain5.Cols["SEL"].Index));

        }
        #endregion Init_grdMain5

        #region Init_grdMain5_1 NDT정보 2
        private void Init_grdMain5_1()
        {
            clsStyle.Style.InitGrid_search(grdMain5_1);
            grdMain5_1.Dock = DockStyle.Fill;
            grdMain5_1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain5_1.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain5_1);

            #region Caption
            grdMain5_1.Cols["L_NO"].Caption = "NO";
            grdMain5_1.Cols["SEL"].Caption = "선택";
            grdMain5_1.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain5_1.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain5_1.Cols["KEY_NO"].Visible = false;
            grdMain5_1.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain5_1.Cols["POC_NO"].Caption = "POC";
            grdMain5_1.Cols["HEAT"].Caption = "HEAT";
            grdMain5_1.Cols["STEEL"].Caption = "강종";
            grdMain5_1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain5_1.Cols["ITEM"].Caption = "품목";
            grdMain5_1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain5_1.Cols["LENGTH"].Caption = "길이(m)";
            grdMain5_1.Cols["PCS"].Caption = "본수";
            grdMain5_1.Cols["SURFACE_LEVEL_NM"].Caption = "표면등급";
            grdMain5_1.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain5_1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain5_1.Cols["SEL"].Width = cs.Sel_Width;
            grdMain5_1.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain5_1.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain5_1.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain5_1.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain5_1.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain5_1.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain5_1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain5_1.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain5_1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain5_1.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain5_1.Cols["PCS"].Width = cs.Short_Value_Width - 10;
            grdMain5_1.Cols["SURFACE_LEVEL_NM"].Width = cs.Middle_Value_Width;
            grdMain5_1.Cols["SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign
            grdMain5_1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain5_1.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain5_1.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain5_1.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain5_1.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain5_1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain5_1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain5_1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain5_1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain5_1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain5_1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain5_1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain5_1.Cols["PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain5_1.Cols["SURFACE_LEVEL_NM"].TextAlign = cs.VALUE_LEFT_TextAlign;
            grdMain5_1.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#3", grdMain5_1.Name, grdMain5_1.Parent.Name, "TB_NDT_PLC_ORD", "SEND_YN", "MILL_NO"));

            grdMain5_1.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain5_1, lbSel, 0, grdMain5_1.Cols["SEL"].Index));

        }
        #endregion Init_grdgrdMain5_1

        #region Init_grdMain6 MPI정보
        private void Init_grdMain6()
        {
            clsStyle.Style.InitGrid_search(grdMain6);
            grdMain6.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain6.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain6);

            #region Caption
            grdMain6.Cols["L_NO"].Caption = "NO";
            grdMain6.Cols["SEL"].Caption = "선택";
            grdMain6.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain6.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain6.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain6.Cols["POC_NO"].Caption = "POC";
            grdMain6.Cols["HEAT"].Caption = "HEAT";
            grdMain6.Cols["STEEL"].Caption = "강종";
            grdMain6.Cols["STEEL_NM"].Caption = "강종명";
            grdMain6.Cols["ITEM"].Caption = "품목";
            grdMain6.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain6.Cols["LENGTH"].Caption = "길이(m)";
            grdMain6.Cols["PCS"].Caption = "본수";
            grdMain6.Cols["MAGNETP_CNCNT"].Caption = "자분농도";
            grdMain6.Cols["MAGNET_CURRENT"].Caption = "자화전류";
            grdMain6.Cols["ILLUM"].Caption = "조도";
            grdMain6.Cols["SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain6.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain6.Cols["SEL"].Width = cs.Sel_Width;
            grdMain6.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain6.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain6.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain6.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain6.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain6.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain6.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain6.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain6.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain6.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain6.Cols["PCS"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["MAGNETP_CNCNT"].Width = cs.Short_Value_Width + 10;
            grdMain6.Cols["MAGNET_CURRENT"].Width = cs.Short_Value_Width + 10;
            grdMain6.Cols["ILLUM"].Width = cs.Short_Value_Width + 10;
            grdMain6.Cols["SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign
            grdMain6.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain6.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain6.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain6.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain6.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain6.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain6.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain6.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain6.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain6.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain6.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain6.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain6.Cols["PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["MAGNETP_CNCNT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["MAGNET_CURRENT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["ILLUM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#1", grdMain6.Name, grdMain6.Parent.Name, "TB_MPI_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#2", grdMain6.Name, grdMain6.Parent.Name, "TB_MPI_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#3", grdMain6.Name, grdMain6.Parent.Name, "TB_MPI_PLC_ORD", "SEND_YN", "MILL_NO"));

            grdMain6.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain6, lbSel, 0, grdMain6.Cols["SEL"].Index));
        }
        #endregion Init_grdMain6

        #region Init_grdMain7 성분분석
        private void Init_grdMain7()
        {
            #region grid caption 설정
            cs.InitGrid_search(grdMain7, false, 2, 1);

            SettingStyleAndCheckEnableSelCols(grdMain7);

            grdMain7.Cols["L_NO"].Caption = "NO";
            grdMain7.Cols["SEL"].Caption = "선택";
            grdMain7.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain7.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain7.Cols["MILL_NO"].Caption = "제품번들번호";
            grdMain7.Cols["POC_NO"].Caption = "POC";
            grdMain7.Cols["HEAT"].Caption = "HEAT";
            grdMain7.Cols["STEEL"].Caption = "강종";
            grdMain7.Cols["STEEL_NM"].Caption = "강종명";
            grdMain7.Cols["ITEM"].Caption = "품목";
            grdMain7.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain7.Cols["LENGTH"].Caption = "길이(m)";
            grdMain7.Cols["PCS"].Caption = "본수";
            grdMain7.Cols["CHM_MIN_C"].Caption = "C";
            grdMain7.Cols["CHM_VAL_C"].Caption = "C";
            grdMain7.Cols["CHM_MAX_C"].Caption = "C";
            grdMain7.Cols["CHM_MIN_SI"].Caption = "SI";
            grdMain7.Cols["CHM_VAL_SI"].Caption = "SI";
            grdMain7.Cols["CHM_MAX_SI"].Caption = "SI";
            grdMain7.Cols["CHM_MIN_MN"].Caption = "MN";
            grdMain7.Cols["CHM_VAL_MN"].Caption = "MN";
            grdMain7.Cols["CHM_MAX_MN"].Caption = "MN";
            grdMain7.Cols["CHM_MIN_NI"].Caption = "NI";
            grdMain7.Cols["CHM_VAL_NI"].Caption = "NI";
            grdMain7.Cols["CHM_MAX_NI"].Caption = "NI";
            grdMain7.Cols["CHM_MIN_CR"].Caption = "CR";
            grdMain7.Cols["CHM_VAL_CR"].Caption = "CR";
            grdMain7.Cols["CHM_MAX_CR"].Caption = "CR";
            grdMain7.Cols["CHM_MIN_MO"].Caption = "MO";
            grdMain7.Cols["CHM_VAL_MO"].Caption = "MO";
            grdMain7.Cols["CHM_MAX_MO"].Caption = "MO";
            grdMain7.Cols["CHM_MIN_V"].Caption = "V";
            grdMain7.Cols["CHM_VAL_V"].Caption = "V";
            grdMain7.Cols["CHM_MAX_V"].Caption = "V";
            grdMain7.Cols["CHM_MIN_TI"].Caption = "TI";
            grdMain7.Cols["CHM_VAL_TI"].Caption = "TI";
            grdMain7.Cols["CHM_MAX_TI"].Caption = "TI";
            grdMain7.Cols["CHM_MIN_NB"].Caption = "NB";
            grdMain7.Cols["CHM_VAL_NB"].Caption = "NB";
            grdMain7.Cols["CHM_MAX_NB"].Caption = "NB";
            grdMain7.Cols["CHM_MIN_CU"].Caption = "CU";
            grdMain7.Cols["CHM_VAL_CU"].Caption = "CU";
            grdMain7.Cols["CHM_MAX_CU"].Caption = "CU";
            grdMain7.Cols["CHM_MIN_ZR"].Caption = "ZR";
            grdMain7.Cols["CHM_VAL_ZR"].Caption = "ZR";
            grdMain7.Cols["CHM_MAX_ZR"].Caption = "ZR";
            grdMain7.Cols["CHM_MIN_P"].Caption = "P";
            grdMain7.Cols["CHM_VAL_P"].Caption = "P";
            grdMain7.Cols["CHM_MAX_P"].Caption = "P";
            grdMain7.Cols["CHM_MIN_S"].Caption = "S";
            grdMain7.Cols["CHM_VAL_S"].Caption = "S";
            grdMain7.Cols["CHM_MAX_S"].Caption = "S";
            grdMain7.Cols["SEND_YN"].Caption = "송신여부";
            #endregion

            #region ChmList 설정
            ChmList = new List<CHM_item>();

            ChmList.Add(new CHM_item("C"));
            ChmList.Add(new CHM_item("SI"));
            ChmList.Add(new CHM_item("MN"));
            ChmList.Add(new CHM_item("NI"));
            ChmList.Add(new CHM_item("CR"));
            ChmList.Add(new CHM_item("MO"));
            ChmList.Add(new CHM_item("V"));
            ChmList.Add(new CHM_item("TI"));
            ChmList.Add(new CHM_item("NB"));
            ChmList.Add(new CHM_item("CU"));
            ChmList.Add(new CHM_item("ZR"));
            ChmList.Add(new CHM_item("P"));
            ChmList.Add(new CHM_item("S"));
            #endregion ChmList 설정

            #region    grid 1번째 로우 캡션 설정
            grdMain7[1, "L_NO"] = grdMain7.Cols["L_NO"].Caption;
            grdMain7[1, "SEL"] = grdMain7.Cols["SEL"].Caption;
            grdMain7[1, "ORD_DATE"] = grdMain7.Cols["ORD_DATE"].Caption;
            grdMain7[1, "ORD_TIME"] = grdMain7.Cols["ORD_TIME"].Caption;
            grdMain7[1, "MILL_NO"] = grdMain7.Cols["MILL_NO"].Caption;
            grdMain7[1, "POC_NO"] = grdMain7.Cols["POC_NO"].Caption;
            grdMain7[1, "HEAT"] = grdMain7.Cols["HEAT"].Caption;
            grdMain7[1, "STEEL"] = grdMain7.Cols["STEEL"].Caption;
            grdMain7[1, "STEEL_NM"] = grdMain7.Cols["STEEL_NM"].Caption;
            grdMain7[1, "ITEM"] = grdMain7.Cols["ITEM"].Caption;
            grdMain7[1, "ITEM_SIZE"] = grdMain7.Cols["ITEM_SIZE"].Caption;
            grdMain7[1, "LENGTH"] = grdMain7.Cols["LENGTH"].Caption;
            grdMain7[1, "PCS"] = grdMain7.Cols["PCS"].Caption;


            grdMain7[1, "CHM_MIN_C"] = "Min";
            grdMain7[1, "CHM_VAL_C"] = "값";
            grdMain7[1, "CHM_MAX_C"] = "Max";
            grdMain7[1, "CHM_MIN_SI"] = "Min";
            grdMain7[1, "CHM_VAL_SI"] = "값";
            grdMain7[1, "CHM_MAX_SI"] = "Max";
            grdMain7[1, "CHM_MIN_MN"] = "Min";
            grdMain7[1, "CHM_VAL_MN"] = "값";
            grdMain7[1, "CHM_MAX_MN"] = "Max";
            grdMain7[1, "CHM_MIN_NI"] = "Min";
            grdMain7[1, "CHM_VAL_NI"] = "값";
            grdMain7[1, "CHM_MAX_NI"] = "Max";
            grdMain7[1, "CHM_MIN_CR"] = "Min";
            grdMain7[1, "CHM_VAL_CR"] = "값";
            grdMain7[1, "CHM_MAX_CR"] = "Max";
            grdMain7[1, "CHM_MIN_MO"] = "Min";
            grdMain7[1, "CHM_VAL_MO"] = "값";
            grdMain7[1, "CHM_MAX_MO"] = "Max";
            grdMain7[1, "CHM_MIN_V"] = "Min";
            grdMain7[1, "CHM_VAL_V"] = "값";
            grdMain7[1, "CHM_MAX_V"] = "Max";
            grdMain7[1, "CHM_MIN_TI"] = "Min";
            grdMain7[1, "CHM_VAL_TI"] = "값";
            grdMain7[1, "CHM_MAX_TI"] = "Max";
            grdMain7[1, "CHM_MIN_NB"] = "Min";
            grdMain7[1, "CHM_VAL_NB"] = "값";
            grdMain7[1, "CHM_MAX_NB"] = "Max";
            grdMain7[1, "CHM_MIN_CU"] = "Min";
            grdMain7[1, "CHM_VAL_CU"] = "값";
            grdMain7[1, "CHM_MAX_CU"] = "Max";
            grdMain7[1, "CHM_MIN_ZR"] = "Min";
            grdMain7[1, "CHM_VAL_ZR"] = "값";
            grdMain7[1, "CHM_MAX_ZR"] = "Max";
            grdMain7[1, "CHM_MIN_P"] = "Min";
            grdMain7[1, "CHM_VAL_P"] = "값";
            grdMain7[1, "CHM_MAX_P"] = "Max";
            grdMain7[1, "CHM_MIN_S"] = "Min";
            grdMain7[1, "CHM_VAL_S"] = "값";
            grdMain7[1, "CHM_MAX_S"] = "Max";

            grdMain7[1, "SEND_YN"] = "송신여부";

            #endregion    grid 1번째 로우 캡션 설정

            #region 성분분석 컬럼 사이즈 설정

            grdMain7.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain7.Cols["SEL"].Width = cs.Sel_Width;
            grdMain7.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain7.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain7.Cols["MILL_NO"].Width = cs.Mill_No_Width + 30;     //변경전 140;
            grdMain7.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain7.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain7.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain7.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain7.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain7.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain7.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain7.Cols["PCS"].Width = cs.Short_Value_Width - 10;

            grdMain7.Cols["CHM_MIN_C"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_C"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_C"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_SI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_SI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_SI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_MN"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_MN"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_MN"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_NI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_NI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_NI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_CR"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_CR"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_CR"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_MO"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_MO"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_MO"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_V"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_V"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_V"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_TI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_TI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_TI"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_NB"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_NB"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_NB"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_CU"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_CU"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_CU"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_ZR"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_ZR"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_ZR"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_P"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_P"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_P"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MIN_S"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_VAL_S"].Width = cs.Short_Value_Width - 30;
            grdMain7.Cols["CHM_MAX_S"].Width = cs.Short_Value_Width - 30;

            grdMain7.Cols["SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion

            #region 정렬 설정
            grdMain7.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain7.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain7.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain7.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain7.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain7.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain7.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain7.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain7.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain7.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain7.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain7.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain7.Cols["CHM_MIN_C"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_C"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_C"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_SI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_SI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_SI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_MN"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_MN"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_MN"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_NI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_NI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_NI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_CR"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_CR"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_CR"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_MO"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_MO"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_MO"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_V"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_V"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_V"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_TI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_TI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_TI"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_NB"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_NB"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_NB"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_CU"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_CU"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_CU"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_ZR"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_ZR"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_ZR"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_P"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_P"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_P"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MIN_S"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_VAL_S"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain7.Cols["CHM_MAX_S"].TextAlign = cs.VALUE_RIGHT_TextAlign;

            grdMain7.Cols["SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion 정렬 설정

            #region Merging 설정

            grdMain7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;

            grdMain7.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain7.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            for (int i = 0; i < grdMain7.Cols.Count; i++)
            {
                grdMain7.Cols[i].AllowMerging = true;
            }
            grdMain7.Rows[0].AllowMerging = true;

            //var crCellRange = grdMain7.GetCellRange(0, grdMain7.Cols["PCS"].Index, grdMain7.Rows.Fixed - 1, grdMain7.Cols.Count-1);


            #endregion Merging 설정

            GrdList.Add(new EqmCtlGrid("#1", grdMain7.Name, grdMain7.Parent.Name, "TB_CHM_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#2", grdMain7.Name, grdMain7.Parent.Name, "TB_CHM_PLC_ORD", "SEND_YN", "MILL_NO"));
            GrdList.Add(new EqmCtlGrid("#3", grdMain7.Name, grdMain7.Parent.Name, "TB_CHM_PLC_ORD", "SEND_YN", "MILL_NO"));


            grdMain7.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain7, lbSel, 0, grdMain7.Cols["SEL"].Index));
        }
        #endregion Init_grdMain7

        #region Init_grdMain8 라벨발행
        private void Init_grdMain8()
        {
            clsStyle.Style.InitGrid_search(grdMain8);
            grdMain8.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain8);

            #region Caption
            grdMain8.Cols["L_NO"].Caption = "NO";
            grdMain8.Cols["SEL"].Caption = "선택";
            grdMain8.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain8.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain8.Cols["BUNDLE_NO"].Caption = "제품번들번호"; // 압연번들번호 삭제
            grdMain8.Cols["POC_NO"].Caption = "POC";
            grdMain8.Cols["HEAT"].Caption = "HEAT";
            grdMain8.Cols["STEEL"].Caption = "강종";
            grdMain8.Cols["STEEL_NM"].Caption = "강종명";
            grdMain8.Cols["ITEM"].Caption = "품목";
            grdMain8.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain8.Cols["LENGTH"].Caption = "길이(m)";
            grdMain8.Cols["BND_ORD_PCS"].Caption = "결속본수";
            grdMain8.Cols["PDA_SCAN_PCS"].Caption = "스캔본수";
            grdMain8.Cols["CERTI_MARK"].Caption = "인증마크";
            grdMain8.Cols["CERTI_NO"].Caption = "인증번호";
            grdMain8.Cols["LABEL_SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain8.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain8.Cols["SEL"].Width = cs.Sel_Width;
            grdMain8.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;             //변경전 110;
            grdMain8.Cols["ORD_TIME"].Width = cs.Date_8_Width;                  //변경전 100;
            grdMain8.Cols["BUNDLE_NO"].Width = cs.BUNDLE_NO_Width;
            grdMain8.Cols["POC_NO"].Width = cs.POC_NO_Width;                    //변경전 100;
            grdMain8.Cols["HEAT"].Width = cs.HEAT_Width;                        //변경전 100;
            grdMain8.Cols["STEEL"].Width = cs.STEEL_Width + 20;                 //변경전 60;
            grdMain8.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;           //변경전 100;
            grdMain8.Cols["ITEM"].Width = cs.ITEM_Width + 15;                   //변경전 60;
            grdMain8.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35;         //변경전 80;
            grdMain8.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain8.Cols["BND_ORD_PCS"].Width = cs.Short_Value_Width - 10;     // 변경전 80;
            grdMain8.Cols["PDA_SCAN_PCS"].Width = cs.Short_Value_Width - 10;    // 변경전 80;
            grdMain8.Cols["CERTI_MARK"].Width = cs.Short_Value_Width + 10;      // 변경전 100;
            grdMain8.Cols["CERTI_NO"].Width = cs.Short_Value_Width + 10;        // 변경전 100;
            grdMain8.Cols["LABEL_SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign
            grdMain8.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain8.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain8.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain8.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain8.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_NO_TextAlign;
            grdMain8.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain8.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain8.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain8.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain8.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain8.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain8.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain8.Cols["BND_ORD_PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain8.Cols["PDA_SCAN_PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain8.Cols["CERTI_MARK"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain8.Cols["CERTI_NO"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain8.Cols["LABEL_SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#1", grdMain8.Name, grdMain8.Parent.Name, "TB_BND_PLC_ORD", "LABEL_SEND_YN", "BUNDLE_NO"));
            GrdList.Add(new EqmCtlGrid("#2", grdMain8.Name, grdMain8.Parent.Name, "TB_BND_PLC_ORD", "LABEL_SEND_YN", "BUNDLE_NO"));
            GrdList.Add(new EqmCtlGrid("#3", grdMain8.Name, grdMain8.Parent.Name, "TB_BND_PLC_ORD", "LABEL_SEND_YN", "BUNDLE_NO"));

            grdMain8.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain8, lbSel, 0, grdMain8.Cols["SEL"].Index));

        }
        #endregion Init_grdMain8

        #region Init_grdMain9 바인딩
        private void Init_grdMain9()
        {
            grdMain9.Dock = DockStyle.Fill;
            clsStyle.Style.InitGrid_search(grdMain9);
            grdMain9.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain9.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain9);

            #region Caption
            grdMain9.Cols["L_NO"].Caption = "NO";
            grdMain9.Cols["SEL"].Caption = "선택";
            grdMain9.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain9.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain9.Cols["KEY_NO"].Visible = false;
            grdMain9.Cols["BUNDLE_NO"].Visible = false;
            grdMain9.Cols["POC_NO"].Caption = "POC";
            grdMain9.Cols["HEAT"].Caption = "HEAT";
            grdMain9.Cols["STEEL"].Caption = "강종";
            grdMain9.Cols["STEEL_NM"].Caption = "강종명";
            grdMain9.Cols["ITEM"].Caption = "품목";
            grdMain9.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain9.Cols["LENGTH"].Visible = false;
            grdMain9.Cols["BND_ORD_PCS"].Caption = "결속본수";
            grdMain9.Cols["PDA_SCAN_PCS"].Visible = false;
            grdMain9.Cols["BND_POINT"].Caption = "결속POINT";
            grdMain9.Cols["BND_SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain9.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain9.Cols["SEL"].Width = cs.Sel_Width;
            grdMain9.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain9.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain9.Cols["BUNDLE_NO"].Width = cs.BUNDLE_NO_Width;
            grdMain9.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain9.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain9.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain9.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain9.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain9.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain9.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain9.Cols["BND_ORD_PCS"].Width = cs.Short_Value_Width - 10; // 변경전 80;
            grdMain9.Cols["PDA_SCAN_PCS"].Width = cs.Short_Value_Width - 10;// 변경전 80;
            grdMain9.Cols["BND_POINT"].Width = cs.BND_POINT_Width - 10;
            grdMain9.Cols["BND_SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign
            grdMain9.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain9.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain9.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain9.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain9.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_NO_TextAlign;
            grdMain9.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain9.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain9.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain9.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain9.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain9.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain9.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain9.Cols["BND_ORD_PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain9.Cols["PDA_SCAN_PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain9.Cols["BND_POINT"].TextAlign = cs.BND_POINT_TextAlign;
            grdMain9.Cols["BND_SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign

            GrdList.Add(new EqmCtlGrid("#1", grdMain9.Name, grdMain9.Parent.Name, "TB_BND_PLC_ORD", "BND_SEND_YN", "POC_NO"));
            GrdList.Add(new EqmCtlGrid("#2", grdMain9.Name, grdMain9.Parent.Name, "TB_BND_PLC_ORD", "BND_SEND_YN", "POC_NO"));
            

            grdMain9.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain9, lbSel, 0, grdMain9.Cols["SEL"].Index));

        }
        #endregion Init_grdMain9

        #region Init_grdMain9_1 바인딩2
        private void Init_grdMain9_1()
        {
            grdMain9_1.Dock = DockStyle.Fill;
            clsStyle.Style.InitGrid_search(grdMain9_1);

            //var crCellRange = grdMain9_1.GetCellRange(0, grdMain9_1.Cols["BND_ORD_PCS"].Index, 0, grdMain9_1.Cols["PDA_SCAN_PCS"].Index);
            var crCellRange = grdMain9_1.GetCellRange(0, grdMain9_1.Cols["BND_ORD_PCS"].Index, 0, grdMain9_1.Cols["BND_POINT"].Index);
            crCellRange.Style = grdMain9_1.Styles["ModifyStyle"];

            
            

            grdMain9_1.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
            grdMain9_1.AutoResize = true;
            SettingStyleAndCheckEnableSelCols(grdMain9_1);
            grdMain9_1.AllowEditing = true;

            grdMain9_1.Cols["L_NO"].AllowEditing = false;
            grdMain9_1.Cols["ORD_DATE"].AllowEditing = false;
            grdMain9_1.Cols["ORD_TIME"].AllowEditing = false;
            grdMain9_1.Cols["BUNDLE_NO"].AllowEditing = false;
            grdMain9_1.Cols["POC_NO"].AllowEditing = false;
            grdMain9_1.Cols["HEAT"].AllowEditing = false;
            grdMain9_1.Cols["STEEL"].AllowEditing = false;
            grdMain9_1.Cols["STEEL_NM"].AllowEditing = false;
            grdMain9_1.Cols["ITEM"].AllowEditing = false;
            grdMain9_1.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain9_1.Cols["LENGTH"].AllowEditing = false;
            grdMain9_1.Cols["BND_ORD_PCS"].AllowEditing = true;
            grdMain9_1.Cols["PDA_SCAN_PCS"].AllowEditing = true;
            grdMain9_1.Cols["BND_POINT"].AllowEditing = true;
            grdMain9_1.Cols["BND_SEND_YN"].AllowEditing = false;

            #region Caption
            grdMain9_1.Cols["L_NO"].Caption = "NO";
            grdMain9_1.Cols["SEL"].Caption = "선택";
            grdMain9_1.Cols["ORD_DATE"].Caption = "지시일자";
            grdMain9_1.Cols["ORD_TIME"].Caption = "지시시각";
            grdMain9_1.Cols["KEY_NO"].Visible = false;
            grdMain9_1.Cols["BUNDLE_NO"].Caption = "제품번들번호";  // 압연번들번호 삭제
            grdMain9_1.Cols["POC_NO"].Caption = "POC";
            grdMain9_1.Cols["HEAT"].Caption = "HEAT";
            grdMain9_1.Cols["STEEL"].Caption = "강종";
            grdMain9_1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain9_1.Cols["ITEM"].Caption = "품목";
            grdMain9_1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain9_1.Cols["LENGTH"].Caption = "길이(m)";
            grdMain9_1.Cols["BND_ORD_PCS"].Caption = "결속본수";
            grdMain9_1.Cols["PDA_SCAN_PCS"].Caption = "스캔본수";
            grdMain9_1.Cols["BND_POINT"].Caption = "결속POINT";
            grdMain9_1.Cols["BND_SEND_YN"].Caption = "송신여부";
            #endregion Caption

            #region Width
            grdMain9_1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain9_1.Cols["SEL"].Width = cs.Sel_Width;
            grdMain9_1.Cols["ORD_DATE"].Width = cs.Date_8_Width + 10;     //변경전 110;
            grdMain9_1.Cols["ORD_TIME"].Width = cs.Date_8_Width;          //변경전 100;
            grdMain9_1.Cols["BUNDLE_NO"].Width = cs.BUNDLE_NO_Width;
            grdMain9_1.Cols["POC_NO"].Width = cs.POC_NO_Width;            //변경전 100;
            grdMain9_1.Cols["HEAT"].Width = cs.HEAT_Width;                //변경전 100;
            grdMain9_1.Cols["STEEL"].Width = cs.STEEL_Width + 20;         //변경전 60;
            grdMain9_1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;   //변경전 100;
            grdMain9_1.Cols["ITEM"].Width = cs.ITEM_Width + 15;           //변경전 60;
            grdMain9_1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35; //변경전 80;
            grdMain9_1.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain9_1.Cols["BND_ORD_PCS"].Width = cs.Short_Value_Width - 10; // 변경전 80;
            grdMain9_1.Cols["PDA_SCAN_PCS"].Width = cs.Short_Value_Width - 10;// 변경전 80;
            grdMain9_1.Cols["BND_POINT"].Width = cs.BND_POINT_Width - 10;
            grdMain9_1.Cols["BND_SEND_YN"].Width = cs.SEND_YN_Width;
            #endregion Width

            #region TextAlign
            grdMain9_1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain9_1.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain9_1.Cols["ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain9_1.Cols["ORD_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain9_1.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_NO_TextAlign;
            grdMain9_1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain9_1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain9_1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain9_1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain9_1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain9_1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain9_1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain9_1.Cols["BND_ORD_PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain9_1.Cols["PDA_SCAN_PCS"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain9_1.Cols["BND_POINT"].TextAlign = cs.BND_POINT_TextAlign;
            grdMain9_1.Cols["BND_SEND_YN"].TextAlign = cs.SEND_YN_TextAlign;
            #endregion TextAlign


            GrdList.Add(new EqmCtlGrid("#3", grdMain9_1.Name, grdMain9_1.Parent.Name, "TB_BND_PLC_ORD", "BND_SEND_YN", "BUNDLE_NO"));

            grdMain9_1.Paint += grd_Paint;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain9_1, lbSel, 0, grdMain9_1.Cols["SEL"].Index));

            grdMain9_1.Tree.Column = grdMain9_1.Cols["BND_SEND_YN"].Index;

        }
        #endregion Init_grdMain9_1

        #endregion
        
        #region 쿼리 설정
        private string QuerySetup()
        {
            string sql1 = "";

            string start_date = start_dt.Value.ToString("yyyyMMdd");
            string end_date = end_dt.Value.ToString("yyyyMMdd");

            #region 교정
            //교정---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP1")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM  ( ");
                sql1 += string.Format("     SELECT  ");
                sql1 += string.Format("             NVL(SEND_YN,'N') AS SEND_YN "); //송신여부
                sql1 += string.Format("           , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("           , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("           , POC_NO AS KEY_NO ");// POC_NO
                sql1 += string.Format("           , NULL   AS MILL_NO "); //
                sql1 += string.Format("           , POC_NO AS POC_NO ");// POC_NO
                sql1 += string.Format("           , HEAT ");
                sql1 += string.Format("           , STEEL ");
                sql1 += string.Format("           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("           , ITEM ");
                sql1 += string.Format("           , ITEM_SIZE ");
                sql1 += string.Format("           , NULL AS LENGTH ");
                sql1 += string.Format("           , NULL AS COL_00 ");
                sql1 += string.Format("           , NVL(TOP_ROLL_GAP_1,'0.0') AS COL_01 ");   //상롤GAP TOP_ROLL_GAP_1
                sql1 += string.Format("           , NVL(TOP_ROLL_GAP_2,'0.0') AS COL_02 ");  //상롤GAP2  TOP_ROLL_GAP_2
                sql1 += string.Format("           , NVL(TOP_ROLL_GAP_3,'0.0') AS COL_03 ");  //상롤GAP3 TOP_ROLL_GAP_3
                sql1 += string.Format("           , NVL(TOP_ROLL_GAP_4,'0.0') AS COL_04 ");  //상롤GAP4 TOP_ROLL_GAP_4
                sql1 += string.Format("           , NVL(TOP_ROLL_GAP_5,'0.0') AS COL_05 ");  //상롤GAP5 TOP_ROLL_GAP_5
                sql1 += string.Format("           , NVL(TOP_ROLL_GAP_6,'0.0') AS COL_06 ");  //상롤GAP6 TOP_ROLL_GAP_6
                sql1 += string.Format("           , NVL(TOP_ROLL_ANGLE_1,'0.0') AS COL_07 "); //상롤각도1 TOP_ROLL_ANGLE_1
                sql1 += string.Format("           , NVL(TOP_ROLL_ANGLE_2,'0.0') AS COL_08 "); //상롤각도2 TOP_ROLL_ANGLE_2
                sql1 += string.Format("           , NVL(TOP_ROLL_ANGLE_3,'0.0') AS COL_09 "); //상롤각도3 TOP_ROLL_ANGLE_3
                sql1 += string.Format("           , NVL(TOP_ROLL_ANGLE_4,'0.0') AS COL_10 "); //상롤각도4 TOP_ROLL_ANGLE_4
                sql1 += string.Format("           , NVL(TOP_ROLL_ANGLE_5,'0.0') AS COL_11 "); //상롤각도5 TOP_ROLL_ANGLE_5
                sql1 += string.Format("           , NVL(TOP_ROLL_ANGLE_6,'0.0') AS COL_12 "); //상롤각도6 TOP_ROLL_ANGLE_6
                sql1 += string.Format("           , NVL(BOT_ROLL_ANGLE_1,'0.0') AS COL_13 "); //하롤각도1 BOT_ROLL_ANGLE_1
                sql1 += string.Format("           , NVL(BOT_ROLL_ANGLE_2,'0.0') AS COL_14 "); //하롤각도2 BOT_ROLL_ANGLE_2
                sql1 += string.Format("           , NVL(BOT_ROLL_ANGLE_3,'0.0') AS COL_15 "); //하롤각도3 BOT_ROLL_ANGLE_3
                sql1 += string.Format("           , NVL(THREAD_SPEED,'0.0')  AS COL_16 "); //THREAD SPEED THREAD_SPEED
                sql1 += string.Format("           , NVL(MACHINE_SPEED,'0.0') AS COL_17 ");  //MACHINE SPEED MACHINE_SPEED
                sql1 += string.Format("    FROM   TB_STR_PLC_ORD_NO1  A ");
                sql1 += string.Format("    WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND   '{0}' = '#1' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("   UNION ");
                sql1 += string.Format("      SELECT  "); //지시일자
                sql1 += string.Format("              NVL(SEND_YN,'N') AS SEND_YN "); //송신여부
                sql1 += string.Format("            , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("            , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("            , MILL_NO AS KEY_NO ");  //압연번들번호 MILL_NO
                sql1 += string.Format("            , MILL_NO AS MILL_NO "); //압연번들번호 MILL_NO
                sql1 += string.Format("            , POC_NO ");
                sql1 += string.Format("            , HEAT ");
                sql1 += string.Format("            , STEEL ");
                sql1 += string.Format("            , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("            , ITEM ");
                sql1 += string.Format("            , ITEM_SIZE ");
                sql1 += string.Format("            , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("            , NVL(PCS,'0') AS COL_00 "); //본수 
                sql1 += string.Format("            , NVL(TOP_ROLL_POS,'0.0') AS COL_01 "); //상롤위치 TOP_ROLL_POS
                sql1 += string.Format("            , NVL(TOP_ROLL_ANGLE,'0.0') AS COL_02 "); //상롤각도
                sql1 += string.Format("            , NVL(BOT_ROLL_ANGLE,'0.0') AS COL_03 "); //하롤각도
                sql1 += string.Format("            , NVL(UP_DOWN_LINEAR_POS,'0.0') AS COL_04 "); //상하가이드의위치
                sql1 += string.Format("            , NVL(FRONT_LINEAR_GUIDE_POS,'0.0')    AS COL_05 "); //전후면의가이드위치
                sql1 += string.Format("            , NVL(REAL_LINEAR_GUIDE_POS,'0.0') AS COL_06 "); //진입롤모터주파수(Hz)
                sql1 += string.Format("            , NVL(TOP_ROLL_MTR_HZ,'0.0') AS COL_07 "); //상롤모터주파수(Hz)
                sql1 += string.Format("            , NVL(BOT_ROLL_MTR_HZ,'0.0') AS COL_08 "); //출구롤모터주파수(Hz)
                sql1 += string.Format("            , NULL AS COL_09 ");
                sql1 += string.Format("            , NULL AS COL_10 ");
                sql1 += string.Format("            , NULL AS COL_11 ");
                sql1 += string.Format("            , NULL AS COL_12 ");
                sql1 += string.Format("            , NULL AS COL_13 ");
                sql1 += string.Format("            , NULL AS COL_14 ");
                sql1 += string.Format("            , NULL AS COL_15 ");
                sql1 += string.Format("            , NULL AS COL_16 ");
                sql1 += string.Format("            , NULL AS COL_17 ");

                sql1 += string.Format("     FROM   TB_STR_PLC_ORD_NO2 A ");
                sql1 += string.Format("     WHERE  SUBSTR(ORD_DDTT,1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND    '{0}' = '#2' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("    UNION ");
                sql1 += string.Format("     SELECT  ");
                sql1 += string.Format("             NVL(SEND_YN,'N') AS SEND_YN "); //송신여부
                sql1 += string.Format("           , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("           , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("           , MILL_NO AS KEY_NO "); //압연번들번호 MILL_NO
                sql1 += string.Format("           , MILL_NO AS MILL_NO "); //압연번들번호 MILL_NO
                sql1 += string.Format("           , POC_NO ");
                sql1 += string.Format("           , HEAT ");
                sql1 += string.Format("           , STEEL ");
                sql1 += string.Format("           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("           , ITEM ");
                sql1 += string.Format("           , ITEM_SIZE ");
                sql1 += string.Format("           , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("           , NVL(PCS,'0') AS COL_00 "); //본수 
                sql1 += string.Format("           , NVL(TOP_ROLL_POS,'0.0') AS COL_01 "); //상롤위치
                sql1 += string.Format("           , NVL(TOP_ROLL_ANGLE,'0.0') AS COL_02 "); //상롤각도
                sql1 += string.Format("           , NVL(BOT_ROLL_ANGLE,'0.0') AS COL_03 "); //하롤각도
                sql1 += string.Format("           , NVL(UP_DOWN_LINEAR_POS,'0.0') AS COL_04 "); //UP DOWN리니어위치
                sql1 += string.Format("           , NVL(FRONT_LINEAR_GUIDE_POS,'0.0')    AS COL_05 "); //FRONT리니어가이드위치
                sql1 += string.Format("           , NVL(REAL_LINEAR_GUIDE_POS,'0.0') AS COL_06 "); //REAL리니어가이드위치
                sql1 += string.Format("           , NVL(TOP_ROLL_MTR_HZ,'0.0') AS COL_07 "); //상롤모터Hz
                sql1 += string.Format("           , NVL(BOT_ROLL_MTR_HZ,'0.0') AS COL_08 "); //하롤모터Hz
                sql1 += string.Format("           , NULL AS COL_09 ");
                sql1 += string.Format("           , NULL AS COL_10 ");
                sql1 += string.Format("           , NULL AS COL_11 ");
                sql1 += string.Format("           , NULL AS COL_12 ");
                sql1 += string.Format("           , NULL AS COL_13 ");
                sql1 += string.Format("           , NULL AS COL_14 ");
                sql1 += string.Format("           , NULL AS COL_15 ");
                sql1 += string.Format("           , NULL AS COL_16 ");
                sql1 += string.Format("           , NULL AS COL_17 ");

                sql1 += string.Format("     FROM  TB_STR_PLC_ORD_NO3 A ");
                sql1 += string.Format("     WHERE SUBSTR(ORD_DDTT,1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND     '{0}' = '#3' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("     ORDER BY ORD_DATE DESC, ORD_TIME DESC, KEY_NO DESC ");
                sql1 += string.Format("     ) X ");
            }
            #endregion

            #region 면취
            //면취---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP2")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("     SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("           , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("           , POC_NO  AS KEY_NO");
                sql1 += string.Format("           , NULL AS MILL_NO "); //압연번들번호
                sql1 += string.Format("           , POC_NO  ");
                sql1 += string.Format("           , HEAT ");
                sql1 += string.Format("           , STEEL ");
                sql1 += string.Format("           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("           , ITEM ");
                sql1 += string.Format("           , ITEM_SIZE ");
                sql1 += string.Format("           , NULL AS LENGTH ");
                sql1 += string.Format("           , NULL AS PCS "); //본수
                sql1 += string.Format("           , NULL AS COL_01 "); //서보1
                sql1 += string.Format("           , NULL AS COL_02 "); //서보2
                sql1 += string.Format("           , NVL(SCREW_1,'0.0') AS COL_03 "); //스크류1
                sql1 += string.Format("           , NVL(SCREW_2,'0.0') AS COL_04 "); //스크류2
                sql1 += string.Format("           , NVL(RT_1,'0.0') AS COL_05 "); //롤러테이블1
                sql1 += string.Format("           , NVL(RT_2,'0.0') AS COL_06 "); //롤러테이블2
                sql1 += string.Format("           , NVL(RT_EXIT,'0.0') AS COL_07 "); //롤러테이블EXIT
                sql1 += string.Format("           , NULL AS COL_08 "); //STOPPER1
                sql1 += string.Format("           , NULL AS COL_09 "); //받침대1
                sql1 += string.Format("           , NULL AS COL_10 "); //소재간격1
                sql1 += string.Format("           , NULL AS COL_11 "); //STOPPER2
                sql1 += string.Format("           , NULL AS COL_12 "); //받침대2
                sql1 += string.Format("           , NULL AS COL_13 "); //소재간격2
                sql1 += string.Format("           , NULL AS COL_14 "); //
                sql1 += string.Format("           , NULL AS COL_15 "); //
                sql1 += string.Format("           , NULL AS COL_16 "); //
                sql1 += string.Format("           , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("           , NULL             AS SERVO_POS_SP_1 "); //
                sql1 += string.Format("           , NULL             AS SERVO_POS_SP_2 "); //

                sql1 += string.Format("    FROM TB_CHF_PLC_ORD_NO1  A ");
                sql1 += string.Format("    WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("     AND    '{0}' = '#1' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("   UNION ");
                sql1 += string.Format("     SELECT TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("           ,TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("           ,MILL_NO AS KEY_NO "); //압연번들번호
                sql1 += string.Format("           ,MILL_NO  "); //압연번들번호
                sql1 += string.Format("           ,POC_NO  ");
                sql1 += string.Format("           ,HEAT ");
                sql1 += string.Format("           ,STEEL ");
                sql1 += string.Format("           ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("           ,ITEM ");
                sql1 += string.Format("           ,ITEM_SIZE ");
                sql1 += string.Format("           ,NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("           ,NVL(PCS,'0') AS PCS"); //본수
                sql1 += string.Format("           ,NVL(KICKER_SIZE_1,'0.0') AS COL_01 "); //키커사이즈조정1
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_H_1,'0.0') AS COL_02 "); //롤러컨베어HZ고속1
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_S_1,'0.0') AS COL_03 "); //롤러컨베어HZ저속1
                sql1 += string.Format("           ,NVL(SCREW_FEED_HZ_1,'0.0') AS COL_04 "); //스크류구동HZ1
                sql1 += string.Format("           ,NVL(GRIND_HZ_1,'0.0') AS COL_05 "); //그라인딩구동HZ1
                sql1 += string.Format("           ,NVL(STOPPER_1,'0.0') AS COL_06 "); //스토퍼1
                sql1 += string.Format("           ,NVL(GUIDE_1,'0.0') AS COL_07 "); //가이드1
                sql1 += string.Format("           ,NVL(SERVO_POS_1,'0.0') AS COL_08 "); //서보위치1
                sql1 += string.Format("           ,NVL(KICKER_SIZE_2,'0.0') AS COL_09 "); //키커사이즈조정2
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_H_2,'0.0') AS COL_10 "); //롤러컨베어HZ고속2
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_S_2,'0.0') AS COL_11 "); //롤러컨베어HZ저속2
                sql1 += string.Format("           ,NVL(SCREW_FEED_HZ_2,'0.0') AS COL_12 "); //스크류구동HZ2
                sql1 += string.Format("           ,NVL(GRIND_HZ_2,'0.0') AS COL_13 "); //그라인딩구동HZ2
                sql1 += string.Format("           ,NVL(STOPPER_2,'0.0') AS COL_14 "); //스토퍼2
                sql1 += string.Format("           ,NVL(GUIDE_2,'0.0') AS COL_15 "); //GUIDE2
                sql1 += string.Format("           ,NVL(SERVO_POS_2,'0.0') AS COL_16 "); //서보위치2
                sql1 += string.Format("           ,NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("           ,NVL(SERVO_POS_SP_1,'0.0')   AS SERVO_POS_SP_1 "); //
                sql1 += string.Format("           ,NVL(SERVO_POS_SP_2,'0.0')   AS SERVO_POS_SP_2 "); //
                sql1 += string.Format("     FROM   TB_CHF_PLC_ORD_NO2 A ");
                sql1 += string.Format("     WHERE SUBSTR(ORD_DDTT,1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("     AND    '{0}' = '#2' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("    UNION ");
                sql1 += string.Format("     SELECT TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("           ,TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("           ,MILL_NO AS KEY_NO "); //압연번들번호
                sql1 += string.Format("           ,MILL_NO  "); //압연번들번호
                sql1 += string.Format("           ,POC_NO ");
                sql1 += string.Format("           ,HEAT ");
                sql1 += string.Format("           ,STEEL ");
                sql1 += string.Format("           ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("           ,ITEM ");
                sql1 += string.Format("           ,ITEM_SIZE ");
                sql1 += string.Format("           ,NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("           ,NVL(PCS,'0') AS PCS "); //본수
                sql1 += string.Format("           ,NVL(KICKER_SIZE_1,'0.0') AS COL_01 "); //키커사이즈조정1
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_H_1,'0.0') AS COL_02 "); //롤러컨베어HZ고속1
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_S_1,'0.0') AS COL_03 "); //롤러컨베어HZ저속1
                sql1 += string.Format("           ,NVL(SCREW_FEED_HZ_1,'0.0') AS COL_04 "); //스크류구동HZ1
                sql1 += string.Format("           ,NVL(GRIND_HZ_1,'0.0') AS COL_05 "); //그라인딩구동HZ1
                sql1 += string.Format("           ,NVL(STOPPER_1,'0.0') AS COL_06 "); //스토퍼1
                sql1 += string.Format("           ,NVL(GUIDE_1,'0.0') AS COL_07 "); //가이드1
                sql1 += string.Format("           ,NVL(SERVO_POS_1,'0.0') AS COL_08 "); //서보위치1
                sql1 += string.Format("           ,NVL(KICKER_SIZE_2,'0.0') AS COL_09 "); //키커사이즈조정2
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_H_2,'0.0') AS COL_10 "); //롤러컨베어HZ고속2
                sql1 += string.Format("           ,NVL(ROLLER_CV_HZ_S_2,'0.0') AS COL_11 "); //롤러컨베어HZ저속2
                sql1 += string.Format("           ,NVL(SCREW_FEED_HZ_2,'0.0') AS COL_12 "); //스크류구동HZ2
                sql1 += string.Format("           ,NVL(GRIND_HZ_2,'0.0') AS COL_13 "); //그라인딩구동HZ2
                sql1 += string.Format("           ,NVL(STOPPER_2,'0.0') AS COL_14 "); //스토퍼2
                sql1 += string.Format("           ,NVL(GUIDE_2,'0.0')AS COL_15 "); //GUIDE2
                sql1 += string.Format("           ,NVL(SERVO_POS_2,'0.0') AS COL_16 "); //서보위치2
                sql1 += string.Format("           ,NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("           ,NVL(SERVO_POS_SP_1,'0.0')   AS SERVO_POS_SP_1 "); //
                sql1 += string.Format("           ,NVL(SERVO_POS_SP_2,'0.0')   AS SERVO_POS_SP_2 "); //
                sql1 += string.Format("     FROM   TB_CHF_PLC_ORD_NO3 A ");
                sql1 += string.Format("     WHERE SUBSTR(ORD_DDTT,1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("     AND    '{0}' = '#3' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("     ORDER BY ORD_DATE DESC, ORD_TIME DESC, KEY_NO DESC ");
                sql1 += string.Format("     ) X ");
            } 
            #endregion

            #region 쇼트
            //쇼트---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP3")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("      SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("            , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("            , MILL_NO "); //압연번들번호
                sql1 += string.Format("            , POC_NO ");
                sql1 += string.Format("            , HEAT ");
                sql1 += string.Format("            , STEEL ");
                sql1 += string.Format("            , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("            , ITEM ");
                sql1 += string.Format("            , ITEM_SIZE ");
                sql1 += string.Format("            , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("            , NVL(PCS,'0') AS PCS "); //본수
                sql1 += string.Format("            , NVL(TRANS_ROLLER_RPM,'0') AS TRANS_ROLLER_RPM "); //이송 롤러 회전수
                sql1 += string.Format("            , NVL(IMPELLER_FREQUENCY,'0') AS IMPELLER_FREQUENCY "); //임펠라 주파수
                sql1 += string.Format("            , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("      FROM TB_SHT_PLC_ORD  A ");
                sql1 += string.Format("      WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND    LINE_GP = '{0}' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("      ORDER BY ORD_DATE DESC, ORD_TIME DESC, MILL_NO DESC ");
                sql1 += string.Format("     ) X ");
            } 
            #endregion

            #region PRII
            //PRII---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP4")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("      SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("           , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("           , MILL_NO "); //압연번들번호
                sql1 += string.Format("           , POC_NO ");
                sql1 += string.Format("           , HEAT ");
                sql1 += string.Format("           , STEEL ");
                sql1 += string.Format("           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("           , ITEM ");
                sql1 += string.Format("           , ITEM_SIZE ");
                sql1 += string.Format("           , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("           , NVL(PCS,'0') AS PCS "); //본수
                sql1 += string.Format("           , SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = A.SURFACE_LEVEL) AS SURFACE_LEVEL_NM "); //표면등급
                sql1 += string.Format("           , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("      FROM TB_PRII_PLC_ORD  A ");
                sql1 += string.Format("      WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND    LINE_GP = '{0}' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("      ORDER BY ORD_DATE DESC, ORD_TIME DESC, MILL_NO DESC ");
                sql1 += string.Format("     ) X ");
            } 
            #endregion

            #region NDT
            //NDT---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP5")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("            SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("                 , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("                 , POC_NO AS KEY_NO ");
                sql1 += string.Format("                 , NULL AS MILL_NO "); //압연번들번호
                sql1 += string.Format("                 , POC_NO  ");
                sql1 += string.Format("                 , HEAT ");
                sql1 += string.Format("                 , STEEL ");
                sql1 += string.Format("                 , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("                 , ITEM ");
                sql1 += string.Format("                 , ITEM_SIZE ");
                sql1 += string.Format("                 , NULL AS LENGTH ");
                sql1 += string.Format("                 , NULL AS PCS " ); //본수
                sql1 += string.Format("                 , SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = A.SURFACE_LEVEL) AS SURFACE_LEVEL_NM "); //결함등급
                sql1 += string.Format("                 , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("            FROM TB_NDT_PLC_ORD_NO1  A ");
                sql1 += string.Format("            WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("            AND    '{0}' = '#1' ", line_gp);
                sql1 += string.Format("            AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("            AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("            AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("            AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);

                sql1 += string.Format("      UNION ");
                sql1 += string.Format("            SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("                 , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("                 , POC_NO AS KEY_NO ");
                sql1 += string.Format("                 , NULL AS MILL_NO "); //압연번들번호
                sql1 += string.Format("                 , POC_NO  ");
                sql1 += string.Format("                 , HEAT ");
                sql1 += string.Format("                 , STEEL ");
                sql1 += string.Format("                 , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("                 , ITEM ");
                sql1 += string.Format("                 , ITEM_SIZE ");
                sql1 += string.Format("                 , NULL AS LENGTH ");
                sql1 += string.Format("                 , NULL AS PCS "); //본수
                sql1 += string.Format("                 , SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = A.SURFACE_LEVEL) AS SURFACE_LEVEL_NM "); //결함등급
                sql1 += string.Format("                 , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("            FROM TB_NDT_PLC_ORD_NO2  A ");
                sql1 += string.Format("            WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("            AND           '{0}' = '#2' ", line_gp);
                sql1 += string.Format("            AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("            AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("            AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("            AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);

                sql1 += string.Format("      UNION ");
                sql1 += string.Format("            SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("                 , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("                 , MILL_NO AS KEY_NO"); //압연번들번호
                sql1 += string.Format("                 , MILL_NO "); //압연번들번호
                sql1 += string.Format("                 , POC_NO ");
                sql1 += string.Format("                 , HEAT ");
                sql1 += string.Format("                 , STEEL ");
                sql1 += string.Format("                 , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("                 , ITEM ");
                sql1 += string.Format("                 , ITEM_SIZE ");
                sql1 += string.Format("                 , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("                 , NVL(PCS,'0') AS PCS "); //본수
                sql1 += string.Format("                 , SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = A.SURFACE_LEVEL) AS SURFACE_LEVEL_NM "); //결함등급
                sql1 += string.Format("                 , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("            FROM TB_NDT_PLC_ORD  A ");
                sql1 += string.Format("            WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("            AND          '{0}' = '#3' ", line_gp);
                sql1 += string.Format("            AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("            AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("            AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("            AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("            ORDER BY ORD_DATE DESC, ORD_TIME DESC, KEY_NO DESC ");

                sql1 += string.Format("     ) X ");
            } 
            #endregion

            #region MPI
            //MPI---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP6")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("      SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("            , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("            , MILL_NO "); //압연번들번호
                sql1 += string.Format("            , POC_NO ");
                sql1 += string.Format("            , HEAT ");
                sql1 += string.Format("            , STEEL ");
                sql1 += string.Format("            , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("            , ITEM ");
                sql1 += string.Format("            , ITEM_SIZE ");
                sql1 += string.Format("            , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("            , NVL(PCS,'0') AS PCS "); //본수
                sql1 += string.Format("            , NVL(MAGNETP_CNCNT,'0') AS MAGNETP_CNCNT "); //자분농도
                sql1 += string.Format("            , NVL(MAGNET_CURRENT,'0') AS MAGNET_CURRENT "); //자화전류
                sql1 += string.Format("            , NVL(ILLUM,'0') AS ILLUM "); //조도
                sql1 += string.Format("            , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("      FROM   TB_MPI_PLC_ORD A ");
                sql1 += string.Format("      WHERE SUBSTR(ORD_DDTT,1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND    LINE_GP = '{0}' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("      ORDER BY ORD_DATE DESC, ORD_TIME DESC, MILL_NO DESC ");
                sql1 += string.Format("     ) X ");
            } 
            #endregion

            #region  성분분석
            //성분분석---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP7")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("       SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("           , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("           , MILL_NO "); //압연번들번호
                sql1 += string.Format("           , POC_NO ");
                sql1 += string.Format("           , HEAT ");
                sql1 += string.Format("           , STEEL ");
                sql1 += string.Format("           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("           , ITEM ");
                sql1 += string.Format("           , ITEM_SIZE ");
                sql1 += string.Format("           , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("           , NVL(PCS,'0') AS PCS "); //본수
                sql1 += string.Format("           , NVL(CHM_MIN_C,'0') AS CHM_MIN_C ");
                sql1 += string.Format("           , NVL(CHM_VAL_C,'0') AS CHM_VAL_C");
                sql1 += string.Format("           , NVL(CHM_MAX_C,'0') AS CHM_MAX_C");
                sql1 += string.Format("           , NVL(CHM_MIN_SI,'0') AS CHM_MIN_SI");
                sql1 += string.Format("           , NVL(CHM_VAL_SI,'0') AS CHM_VAL_SI");
                sql1 += string.Format("           , NVL(CHM_MAX_SI,'0') AS CHM_MAX_SI");
                sql1 += string.Format("           , NVL(CHM_MIN_MN,'0') AS CHM_MIN_MN");
                sql1 += string.Format("           , NVL(CHM_VAL_MN,'0') AS CHM_VAL_MN");
                sql1 += string.Format("           , NVL(CHM_MAX_MN,'0') AS CHM_MAX_MN");
                sql1 += string.Format("           , NVL(CHM_MIN_NI,'0') AS CHM_MIN_NI");
                sql1 += string.Format("           , NVL(CHM_VAL_NI,'0') AS CHM_VAL_NI");
                sql1 += string.Format("           , NVL(CHM_MAX_NI,'0') AS CHM_MAX_NI");
                sql1 += string.Format("           , NVL(CHM_MIN_CR,'0') AS CHM_MIN_CR");
                sql1 += string.Format("           , NVL(CHM_VAL_CR,'0') AS CHM_VAL_CR");
                sql1 += string.Format("           , NVL(CHM_MAX_CR,'0') AS CHM_MAX_CR");
                sql1 += string.Format("           , NVL(CHM_MIN_MO,'0') AS CHM_MIN_MO");
                sql1 += string.Format("           , NVL(CHM_VAL_MO,'0') AS CHM_VAL_MO");
                sql1 += string.Format("           , NVL(CHM_MAX_MO,'0') AS CHM_MAX_MO");
                sql1 += string.Format("           , NVL(CHM_MIN_V,'0') AS CHM_MIN_V");
                sql1 += string.Format("           , NVL(CHM_VAL_V,'0') AS CHM_VAL_V");
                sql1 += string.Format("           , NVL(CHM_MAX_V,'0') AS CHM_MAX_V");
                sql1 += string.Format("           , NVL(CHM_MIN_TI,'0') AS CHM_MIN_TI");
                sql1 += string.Format("           , NVL(CHM_VAL_TI,'0') AS CHM_VAL_TI");
                sql1 += string.Format("           , NVL(CHM_MAX_TI,'0') AS CHM_MAX_TI");
                sql1 += string.Format("           , NVL(CHM_MIN_NB,'0') AS CHM_MIN_NB");
                sql1 += string.Format("           , NVL(CHM_VAL_NB,'0') AS CHM_VAL_NB");
                sql1 += string.Format("           , NVL(CHM_MAX_NB,'0') AS CHM_MAX_NB");
                sql1 += string.Format("           , NVL(CHM_MIN_CU,'0') AS CHM_MIN_CU");
                sql1 += string.Format("           , NVL(CHM_VAL_CU,'0') AS CHM_VAL_CU");
                sql1 += string.Format("           , NVL(CHM_MAX_CU,'0') AS CHM_MAX_CU");
                sql1 += string.Format("           , NVL(CHM_MIN_ZR,'0') AS CHM_MIN_ZR");
                sql1 += string.Format("           , NVL(CHM_VAL_ZR,'0') AS CHM_VAL_ZR");
                sql1 += string.Format("           , NVL(CHM_MAX_ZR,'0') AS CHM_MAX_ZR");
                sql1 += string.Format("           , NVL(CHM_MIN_P,'0') AS CHM_MIN_P");
                sql1 += string.Format("           , NVL(CHM_VAL_P,'0') AS CHM_VAL_P");
                sql1 += string.Format("           , NVL(CHM_MAX_P,'0') AS CHM_MAX_P");
                sql1 += string.Format("           , NVL(CHM_MIN_S,'0') AS CHM_MIN_S");
                sql1 += string.Format("           , NVL(CHM_VAL_S,'0') AS CHM_VAL_S");
                sql1 += string.Format("           , NVL(CHM_MAX_S,'0') AS CHM_MAX_S");
                sql1 += string.Format("           , NVL(SEND_YN,'N') AS SEND_YN "); //
                sql1 += string.Format("       FROM   TB_CHM_PLC_ORD A ");
                sql1 += string.Format("       WHERE SUBSTR(ORD_DDTT,1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("       AND    LINE_GP = '{0}' ", line_gp);
                sql1 += string.Format("       AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("       AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("       AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("       AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                if (line_gp != "#3")
                {
                    sql1 += string.Format("       AND ROWNUM = 0    ");
                }
                sql1 += string.Format("       ORDER BY ORD_DATE DESC, ORD_TIME DESC, MILL_NO DESC ");
                sql1 += string.Format("     ) X ");
            }
            #endregion

            #region 라벨발행
            //라벨발행---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP8")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       ROWNUM AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("       SELECT  TO_CHAR(TO_DATE(SUBSTR(LABEL_ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("             , TO_CHAR(TO_DATE(SUBSTR(LABEL_ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("             , BUNDLE_NO "); //제품번들번호
                sql1 += string.Format("             , POC_NO ");
                sql1 += string.Format("             , HEAT ");
                sql1 += string.Format("             , STEEL ");
                sql1 += string.Format("             , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("             , ITEM ");
                sql1 += string.Format("             , ITEM_SIZE ");
                sql1 += string.Format("             , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("             , NVL(BND_ORD_PCS,'0') AS BND_ORD_PCS "); //결속본수
                sql1 += string.Format("             , NVL(PDA_SCAN_PCS,'0') AS PDA_SCAN_PCS "); //PDA SCAN 본수
                sql1 += string.Format("             , CERTI_MARK "); //인증마크
                sql1 += string.Format("             , CERTI_NO "); //인증번호
                sql1 += string.Format("             , NVL(LABEL_SEND_YN,'N') AS LABEL_SEND_YN "); //
                sql1 += string.Format("       FROM   TB_BND_PLC_ORD A ");
                sql1 += string.Format("       WHERE SUBSTR(LABEL_ORD_DDTT,1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("       AND    LINE_GP = '{0}' ", line_gp);
                sql1 += string.Format("       AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("       AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("       AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("       AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                if (line_gp != "#3")
                {
                    sql1 += string.Format("       AND ROWNUM = 0    ");
                }
                sql1 += string.Format("       ORDER BY ORD_DATE DESC, ORD_TIME DESC, BUNDLE_NO DESC ");
                sql1 += string.Format("     ) X ");
            }
            #endregion

            #region 바인딩
            //바인딩---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP9")
            {
                sql1 = string.Format(" SELECT ");
                sql1 += string.Format("       TO_CHAR(ROWNUM) AS L_NO ");
                sql1 += string.Format("       , 'False' AS SEL ");
                sql1 += string.Format("       , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("      SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("            , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("            , POC_NO AS KEY_NO ");
                sql1 += string.Format("            , NULL AS BUNDLE_NO "); //
                sql1 += string.Format("            , POC_NO ");
                sql1 += string.Format("            , HEAT ");
                sql1 += string.Format("            , STEEL ");
                sql1 += string.Format("            , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("            , ITEM ");
                sql1 += string.Format("            , ITEM_SIZE ");
                sql1 += string.Format("            , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("            , NVL(BND_ORD_PCS, '0') AS BND_ORD_PCS "); //결속본수
                sql1 += string.Format("            , NULL AS PDA_SCAN_PCS "); //PDA SCAN 본수
                sql1 += string.Format("            , NVL(BND_POINT, '0') AS BND_POINT "); //PDA SCAN 본수
                sql1 += string.Format("            , NVL(BND_SEND_YN,'N') AS BND_SEND_YN "); //
                sql1 += string.Format("      FROM TB_BND_PLC_ORD_NO1  A ");
                sql1 += string.Format("      WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND    '{0}' = '#1' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);

                sql1 += string.Format("     UNION ");
                sql1 += string.Format("      SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("            , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("            , POC_NO AS KEY_NO ");
                sql1 += string.Format("            , NULL AS BUNDLE_NO "); //
                sql1 += string.Format("            , POC_NO ");
                sql1 += string.Format("            , HEAT ");
                sql1 += string.Format("            , STEEL ");
                sql1 += string.Format("            , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("            , ITEM ");
                sql1 += string.Format("            , ITEM_SIZE ");
                sql1 += string.Format("            , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("            , NVL(BND_ORD_PCS, '0') AS BND_ORD_PCS "); //결속본수
                sql1 += string.Format("            , NULL AS PDA_SCAN_PCS "); //PDA SCAN 본수
                sql1 += string.Format("            , NVL(BND_POINT, '0') AS BND_POINT "); //PDA SCAN 본수
                sql1 += string.Format("            , NVL(BND_SEND_YN,'N') AS BND_SEND_YN "); //
                sql1 += string.Format("      FROM TB_BND_PLC_ORD_NO2  A ");
                sql1 += string.Format("      WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND    '{0}' = '#2' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);

                sql1 += string.Format("     UNION ");
                sql1 += string.Format("      SELECT  TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD') AS ORD_DATE"); //지시일자
                sql1 += string.Format("            , TO_CHAR(TO_DATE(SUBSTR(ORD_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS ORD_TIME "); //지시시간
                sql1 += string.Format("            , BUNDLE_NO AS KEY_NO ");
                sql1 += string.Format("            , BUNDLE_NO  ");
                sql1 += string.Format("            , POC_NO ");
                sql1 += string.Format("            , HEAT ");
                sql1 += string.Format("            , STEEL ");
                sql1 += string.Format("            , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("            , ITEM ");
                sql1 += string.Format("            , ITEM_SIZE ");
                sql1 += string.Format("            , NVL(LENGTH,'0.00') AS LENGTH ");
                sql1 += string.Format("            , NVL(BND_ORD_PCS, '0') AS BND_ORD_PCS "); //결속본수
                sql1 += string.Format("            , NVL(PDA_SCAN_PCS, '0') AS PDA_SCAN_PCS "); //PDA SCAN 본수
                sql1 += string.Format("            , NVL(BND_POINT, '0') AS BND_POINT "); //PDA SCAN 본수
                sql1 += string.Format("            , NVL(BND_SEND_YN,'N') AS BND_SEND_YN "); //
                sql1 += string.Format("      FROM TB_BND_PLC_ORD  A ");
                sql1 += string.Format("      WHERE  SUBSTR(ORD_DDTT, 1, 8) BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND    '{0}' = '#3' ", line_gp);
                sql1 += string.Format("      AND HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND POC_NO    LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("      ORDER BY ORD_DATE DESC, ORD_TIME DESC, KEY_NO DESC ");
                sql1 += string.Format("     ) X ");
            } 
            #endregion

            return sql1;
        }

        #endregion

        private void btnPLCSend_Click(object sender, EventArgs e)
        {


            // 1.현재 선택된 TAB에서 활성화된 그리드를 읽어옴
            // 2.선택된 그리드에서 선택 체크된 행의 데이터를 읽어옴
            // 3.현재 선택된 그리드의 db table의 선택된 행의 SEND_YN 값을 "N"으로 업데이트함.
            // 4.데이터를 읽어와서 SP를 실행하고 성공하면 조회 실행 후 정상처리 메시지를, 실패하면 메시지를 뿌려줌
            var selectedGrd = (C1FlexGrid)TabOpt.SelectedTab.Controls[0];

            //  Checked 행이 있으면 진행
            if (!IsCheckedItem(selectedGrd))
            {
                return;
            }

            // 한번에 한개의 poc 만 재송신가능하게 poc 갯수 체크
            int duplicateIndex = GetMultiePocRow(selectedGrd);

            if (duplicateIndex != 0)
            {
                MessageBox.Show(" POC가 두가지 이상입니다. \n  "+ duplicateIndex + " 행을 선택해제하세요.");
                return;
            }

            //Cursor = Cursors.WaitCursor;
            foreach (var grddata in GrdList)
            {
                if (grddata.Grd_Name == selectedGrd.Name && grddata.Line_gp == line_gp)
                {
                    if (UpdateDB(selectedGrd, grddata))
                    {
                        //SP SP_PLC_ORD_IF1_CALL, SP_PLC_ORD_IF2_CALL  실행함.
                        if (   CALL_PLC_ORD_IF1(selectedGrd, line_gp) 
                            && CALL_PLC_ORD_IF2(selectedGrd, line_gp))
                        {
                            //btnDisplay_Click(null, null);
                        }

                    }
                    break;
                }
            }
            //Cursor = Cursors.Default;
        }

        private int GetMultiePocRow(C1FlexGrid grd)
        {
            string pocNo = string.Empty;

            int key_index = GetPOCIndex(grd);

            for (int row = GetAvailMinRow(grd); row < grd.Rows.Count; row++)
            {
                if (grd.GetData(row, "SEL").ToString() == "False")
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(pocNo) && pocNo != grd.GetData(row, key_index).ToString())
                {
                    return row;
                }

                pocNo = grd.GetData(row, key_index).ToString();

            }

            return 0;
        }

        private string GetPocRow(C1FlexGrid grd)
        {
            string pocNo = string.Empty;

            int key_index = GetPOCIndex(grd);

            for (int row = GetAvailMinRow(grd); row < grd.Rows.Count; row++)
            {
                if (grd.GetData(row, "SEL").ToString() == "TRUE")
                
                {
                    pocNo = grd.GetData(row, key_index).ToString();
                    return pocNo;
                }


            }

            return pocNo;
        }

        private int GetPOCIndex(C1FlexGrid grd)
        {
            foreach (Column item in grd.Cols)
            {
                if (item.Caption == "POC")
                {
                    return item.Index;
                }
            }
            return 5;
        }

        private void IsManyPoc(C1FlexGrid grd)
        {
            //DataTable dt = (DataTable)grd.DataSource;

            //DataRow[] result = dt.Select("SEL = 'True'");

            //int index;

            //string pocNo = (result[0].ItemArray)[grd.Cols["POC_NO"].Index].ToString();

            //foreach (DataRow item in result)
            //{
            //    if (pocNo != item["POC_NO"].ToString())
            //    {

            //    }
            //}

            string pocNo = string.Empty;

            for (int row = grd.Rows.Fixed; row < grd.Rows.Count; row++)
            {
                if (grd.GetData(row, "SEL").ToString() == "False")
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(pocNo) && pocNo != grd.GetData(row, "POC_NO").ToString())
                {

                }

            }

            

        }

        private bool CALL_PLC_ORD_IF1(C1FlexGrid grd, string _line_gp)
        {
            bool success = false;

            OracleConnection conn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string spName = "SP_PLC_ORD_IF1_CALL";
            string result_stat = string.Empty;
            string result_msg = string.Empty;
            string poc_no = string.Empty;

            //List<string> millList = new List<string>();

            ////라벨이후의 공정에는 BUNDLE_NO 이전에는 MILL_NO  MILL_NO 컬럼이 존재하면 MILL_NO 아니면 BUNDLE_NO
            //string IndexColName = (grd.Cols.Contains("MILL_NO")) ? "MILL_NO":"BUNDLE_NO";

            int key_index = GetPOCIndex(grd);

            try
            {

                cmd.Connection = conn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                OracleParameter op;

                for (int row = GetAvailMinRow(grd); row < grd.Rows.Count; row++)
                {
                    if (grd.GetData(row, "SEL").ToString() == "False")
                    {
                        continue;
                    }

                    //동일한 poc 인경울 pass
                    if (poc_no == grd.GetData(row, key_index).ToString())
                    {
                        continue;
                    }

                    //op = null;
                    cmd.Parameters.Clear();

                    op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = _line_gp;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = grd.GetData(row, key_index).ToString();    // poc
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_GP", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = "REG";
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_USER", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ck.UserID;    // 사용자 id 가져와서 입력
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_MILL_NO", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = "%";//grd.GetData(row, IndexColName).ToString();

                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = 4000;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = 4000;
                    cmd.Parameters.Add(op);

                    cmd.ExecuteOracleScalar();

                    result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                    result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                    if (result_stat == "ERR")
                    {
                        MessageBox.Show(result_msg);
                        //return false;
                        success = false;
                        break;
                    }

                    poc_no = grd.GetData(row, key_index).ToString();
                }

                transaction.Commit();

                success = true;
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                success = false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();

            }
            return success;
        }


        private bool CALL_PLC_ORD_IF2(C1FlexGrid grd, string _line_gp)
        {
            bool success = false;

            OracleConnection conn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string spName = "SP_PLC_ORD_IF2_CALL";
            string result_stat = string.Empty;
            string result_msg = string.Empty;
            string poc_no = string.Empty;

            ////라벨이후의 공정에는 BUNDLE_NO 이전에는 MILL_NO  MILL_NO 컬럼이 존재하면 MILL_NO 아니면 BUNDLE_NO
            //string IndexColName = (grd.Cols.Contains("MILL_NO")) ? "MILL_NO" : "BUNDLE_NO";
            int key_index = GetPOCIndex(grd);
            try
            {

                cmd.Connection = conn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                OracleParameter op;

                for (int row = GetAvailMinRow(grd); row < grd.Rows.Count; row++)
                {
                    if (grd.GetData(row, "SEL").ToString() == "False")
                    {
                        continue;
                    }

                    //동일한 poc 인경울 pass
                    if (poc_no == grd.GetData(row, key_index).ToString())
                    {
                        continue;
                    }

                    //op = null;
                    cmd.Parameters.Clear();



                    op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = _line_gp;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = grd.GetData(row, key_index).ToString();    // poc
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_GP", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = "REG";
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_USER", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ck.UserID;    // 사용자 id 가져와서 입력
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_MILL_NO", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = "%";// grd.GetData(row, IndexColName).ToString();
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = 4000;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = 4000;
                    cmd.Parameters.Add(op);

                    cmd.ExecuteOracleScalar();

                    result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                    result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                    if (result_stat == "ERR")
                    {
                        MessageBox.Show(result_msg);
                        //return false;
                        success = false;
                        break;
                    }

                    poc_no = grd.GetData(row, key_index).ToString();
                }

                transaction.Commit();

                btnDisplay_Click(null, null);

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), result_msg);

                success = true;

            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                success = false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();

            }
            return success;
        }
        /// <summary>
        /// 선택된 항목을 해당테이블의 전송유무를  N으로 수정
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="_grdData"></param>
        /// <returns></returns>
        private bool UpdateDB(C1FlexGrid grd, EqmCtlGrid _grdData)
        {
            string sql1 = string.Empty;
            
            bool success;

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


                #region grd
                for (int row = GetAvailMinRow(grd); row < grd.Rows.Count; row++)
                {
                    
                    // Update 처리
                    if (grd.GetData(row, "SEL").ToString() == "True")
                    {

                        sql1 = string.Format("UPDATE {0} "            , _grdData.Table_Name);
                        sql1 += string.Format("SET {0} = 'N' "         , _grdData.SEND_YN_COL_Name);
                        sql1 += string.Format("WHERE {0} =  '{1}' ", _grdData.KeyName, grd.GetData(row, _grdData.KeyName));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                    }
                }
                #endregion  
                //실행후 성공
                transaction.Commit();

                //btnDisplay_Click(null, null);

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);
                success = true;
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
                success = false;
                return success;
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
            return success;
        }


        private bool IsCheckedItem(C1FlexGrid selectedGrd)
        {
            // 실제 데이터 부분의 시작점이 1 or 2 인경우가 있슴.
            for (int row = GetAvailMinRow(selectedGrd); row < selectedGrd.Rows.Count; row++)
            {
                if (selectedGrd.GetData(row, "SEL").ToString() == "True")
                {
                    return true;
                }
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //sanity
            //바인딩 그리드 이면서 3라인일경우 진행
            if (selectedgrid.Name != "grdMain9_1") return;

            #region 항목 체크
            //항목 체크
            string contents = string.Empty;
            string show_msg = string.Empty;
            string contenstsCol = string.Empty;
            bool IsModified = false;

            for (int row = GetAvailMinRow(selectedgrid); row < selectedgrid.Rows.Count; row++)
            {
                //수정된것
                if (selectedgrid.GetData(row, "L_NO").ToString() == "수정")
                {
                    // 스캔본수
                    contents = selectedgrid.GetData(row, "PDA_SCAN_PCS").ToString();
                    contenstsCol = selectedgrid.Cols["PDA_SCAN_PCS"].Caption;
                    if (string.IsNullOrEmpty(contents))
                    {
                        show_msg = string.Format("{1}째 행의 {0}를 입력하세요.", contenstsCol, row-1);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 결속본수
                    contents = selectedgrid.GetData(row, "BND_ORD_PCS").ToString();
                    contenstsCol = selectedgrid.Cols["BND_ORD_PCS"].Caption;
                    if (string.IsNullOrEmpty(contents))
                    {
                        show_msg = string.Format("{1}째 행의 {0}를 입력하세요.", contenstsCol, row-1);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }
            #endregion

            //수정진행

            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string sql1 = string.Empty;
            string sql2 = string.Empty;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                #region selectedgrid1
                for (int row = GetAvailMinRow(selectedgrid); row < selectedgrid.Rows.Count; row++)
                {
                    if (selectedgrid.GetData(row, "L_NO").ToString() == "수정")
                    {
                        sql1  = string.Format(" UPDATE TB_BND_PLC_ORD ");
                        sql1 += string.Format(" SET   ");
                        sql1 += string.Format("      PDA_SCAN_PCS    = '{0}' ", selectedgrid.GetData(row, "PDA_SCAN_PCS"));
                        sql1 += string.Format("     ,BND_ORD_PCS = '{0}' ", selectedgrid.GetData(row, "BND_ORD_PCS"));
                        sql1 += string.Format("     ,BND_POINT = '{0}' ", selectedgrid.GetData(row, "BND_POINT"));
                        sql1 += string.Format("     ,MODIFIER   = '{0}' ", ck.UserID);
                        sql1 += string.Format("     ,MOD_DDTT   = SYSDATE ");
                        sql1 += string.Format("     ,LABEL_SEND_YN   = 'N' ");
                        sql1 += string.Format("     ,BND_SEND_YN   = 'N' ");
                        sql1 += string.Format(" WHERE BUNDLE_NO  = '{0}' ", selectedgrid.GetData(row, "BUNDLE_NO"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                    }

                }
                #endregion selectedgrid1 

                #region selectedgrid2
                for (int row = GetAvailMinRow(selectedgrid); row < selectedgrid.Rows.Count; row++)
                {
                    if (selectedgrid.GetData(row, "L_NO").ToString() == "수정")
                    {
                        sql2 = string.Format(" UPDATE TB_CHM_PLC_ORD ");
                        sql2 += string.Format(" SET   ");
                        sql2 += string.Format("      PCS    = '{0}' ", selectedgrid.GetData(row, "PDA_SCAN_PCS"));
                        sql2 += string.Format("     ,REGISTER   = '{0}' ", ck.UserID);
                        sql2 += string.Format("     ,REG_DDTT   = SYSDATE ");
                        sql2 += string.Format("     ,SEND_YN   = 'N' ");
                        sql2 += string.Format(" WHERE MILL_NO  = '{0}' ", selectedgrid.GetData(row, "BUNDLE_NO"));

                        cmd.CommandText = sql2;
                        cmd.ExecuteNonQuery();

                    }

                }
                #endregion selectedgrid2
                //실행후 성공
                transaction.Commit();

                btnDisplay_Click(null, null);
                //SetDataBinding();


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

        private void grdMain9_1_BeforeEdit(object sender, RowColEventArgs e)
        {
            //sanity
            C1FlexGrid grd = sender as C1FlexGrid;

            //subtotal 선택 못하게함.
            if (e.Row == grd.Rows[1].Index) e.Cancel = true;

            //check 선택시 수정표시 안되게함.
            if (e.Col == grd.Cols["SEL"].Index ) return;

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(e.Row, e.Col).ToString();
        }

        private void grdMain9_1_AfterEdit(object sender, RowColEventArgs e)
        {
            //sanity
            C1FlexGrid grd = sender as C1FlexGrid;

           

            // 수정된 내용이 없으면 Update 처리하지 않는다., 선택시에도 ..
            if (strBefValue == grd.GetData(e.Row, e.Col).ToString() || e.Col == grd.Cols["SEL"].Index)
                return;

            //grd.SetData(e.Row, e.Col, vf.UCase(grd.GetData(e.Row, "ACL_GRP_ID").ToString()));

            // 저장시 UPDATE로 처리하기 위해 flag set
            //grd.SetData(e.Row, grd.Cols["GUBUN"].Index, "수정");
            grd.SetData(e.Row, grd.Cols["L_NO"].Index, "수정");

            // Update 배경색 지정
            grd.Rows[grd.RowSel].Style = grd.Styles["UpColor"];

        }

        private void btnBNDSend_Click(object sender, EventArgs e)
        {
            var selectedGrd = (C1FlexGrid)TabOpt.SelectedTab.Controls[0];
            //int key_index = GetPOCIndex(selectedGrd);
            var pocNo = GetPocRow(selectedGrd);
  
            BNDFinREG(line_gp);
        }

        public static Form GetForm(string formName)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == formName)
                {
                    return frm;
                }
            }
            return null;

        }
        public static void BNDFinREG(string _line_gp)
        {
            var temp_form = GetForm("BNDOrdUpd");

            if (temp_form == null)
            {
                var sub = new BNDOrdUpd(_line_gp, "");

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;

                sub.FormBorderStyle = FormBorderStyle.FixedDialog;
                // Set the MaximizeBox to false to remove the maximize box.
                sub.MaximizeBox = false;
                // Set the MinimizeBox to false to remove the minimize box.
                sub.MinimizeBox = false;
                //sub.ShowDialog();
                sub.Show();
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }
    }
}