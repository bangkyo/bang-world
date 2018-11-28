using BGsystemLibrary.MatMgmt;
using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace BGsystemLibrary
{
    public partial class AuthMgmt : Form
    {

        //공통
        ConnectDB cd = new ConnectDB();
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        private string ownerNM = "";
        private string titleNM = "";
        bool _CanSaveSearchLog = false;

        //데이터테이블
        DataTable grdMain_dt;
        DataTable grdSub_dt;
        DataTable olddt;
        DataTable moddt;
        DataTable olddt_sub;
        DataTable moddt_sub;
        TextBox tbACL_ID;

        //그리드 변경값 체크 변수
        private string strBefValue_Main = "";
        private string strBefValue_Sub = "";
        private string selected_grp_id = "";

        //콤보박스 변수
        Hashtable Sys_cd = null;
        private static string cbxSys_id = "";


        //그리드 세팅
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private static C1FlexGrid selectedGrd;
        private int GridRowsCount = 2;
        private int GridColsCount = 4;
        private int GridColsCount2 = 9;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;
        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        //권한관련 add ]]

        public AuthMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            //ownerNM = ownerNm;
            //titleNM = titleNm;

            ////권한관련 add [[
            string[] scrAuthParams = scrAuth.Split(',');

            this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장
            //권한관련 add ]]

            InitializeComponent();
        }

        /// <summary>
        /// 화면 초기화
        /// </summary>
        private void AuthMgmt_Load(object sender, System.EventArgs e)
        {
            this.BackColor = Color.White;

            // 각족 컬트롤 설정
            InitControl();

            _CanSaveSearchLog = true;
            //버튼 클릭 이벤트
            Button_Click(btnDisplay, null);

        }

        /// <summary>
        /// 그리드 초기화
        /// </summary>
        /// <param name="grdItem"></param>
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                //그리드 그리기
                if (grdItem.Rows.Count == 1)
                {
                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                }
                else
                {
                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                }

                //수정가능 컬럼 세팅
                if (grdItem.Name.ToString() == "grdMain")
                {
                    FlexGridCol(grdItem);

                    //grdItem.AllowEditing = true;
                    grdItem.Cols[1].AllowEditing = true;
                    grdItem.Cols[2].AllowEditing = true;
                    grdItem.Cols[3].AllowEditing = true;
                }
                else
                {
                    FlexGridCol2(grdItem);

                    //grdItem.AllowEditing = true;
                    grdItem.Cols[0].AllowEditing = false;
                    grdItem.Cols[1].AllowEditing = false;
                    grdItem.Cols[2].AllowEditing = false;
                    grdItem.Cols[3].AllowEditing = true;
                    grdItem.Cols[4].AllowEditing = true;
                    grdItem.Cols[5].AllowEditing = true;
                    grdItem.Cols[6].AllowEditing = true;
                    grdItem.Cols[7].AllowEditing = true;

                    MakeAllSelect();
                }
                grdMain.SelectionMode = SelectionModeEnum.Row;
                grdSub.SelectionMode = SelectionModeEnum.Row;
                grdMain.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
                grdSub.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndInit();
                grdItem.Invalidate();
            }
        }

        ArrayList _al = new ArrayList();
        private void MakeAllSelect()
        {

            /*
             * TOTAL
             * INQ_ACL
             * REG_ACL
             * MOD_ACL
             * DEL_ACL
             */
       
            Label lbSelTotal = new Label();

            lbSelTotal.BackColor = Color.Transparent;
            lbSelTotal.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSelTotal.Click += AllRowSelectedEvent;
            lbSelTotal.Tag = "TOTAL";

            _al.Add(new HostedControl(grdSub, lbSelTotal, 1, 3));

            Label lbSelINQ_ACL = new Label();

            lbSelINQ_ACL.BackColor = Color.Transparent;
            lbSelINQ_ACL.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSelINQ_ACL.Click += AllRowSelectedEvent;
            lbSelINQ_ACL.Tag = "INQ_ACL";

            _al.Add(new HostedControl(grdSub, lbSelINQ_ACL, 1, 4));

            Label lbSelREG_ACL = new Label();

            lbSelREG_ACL.BackColor = Color.Transparent;
            lbSelREG_ACL.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSelREG_ACL.Click += AllRowSelectedEvent;
            lbSelREG_ACL.Tag = "REG_ACL";

            _al.Add(new HostedControl(grdSub, lbSelREG_ACL, 1, 5));

            Label lbSelMOD_ACL = new Label();

            lbSelMOD_ACL.BackColor = Color.Transparent;
            lbSelMOD_ACL.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSelMOD_ACL.Click += AllRowSelectedEvent;
            lbSelMOD_ACL.Tag = "MOD_ACL";

            _al.Add(new HostedControl(grdSub, lbSelMOD_ACL, 1, 6));

            Label lbSelDEL_ACL = new Label();

            lbSelDEL_ACL.BackColor = Color.Transparent;
            lbSelDEL_ACL.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSelDEL_ACL.Click += AllRowSelectedEvent;
            lbSelDEL_ACL.Tag = "DEL_ACL";

            _al.Add(new HostedControl(grdSub, lbSelDEL_ACL, 1, 7));
        }
        bool allChecked = false;
        private void AllRowSelectedEvent(object sender, EventArgs e)
        {
            string tagValue = ((Label)sender).Tag.ToString();

            if (allChecked)
            {
                for (int row = GridRowsCount; row < grdSub.Rows.Count; row++)
                {
                    grdSub.SetData(row, tagValue, false);
                    //GrdSub_CellChecked(row, grdSub.Cols["TOTAL"].Index);
                    SetupCheckedEvent(row, grdSub.Cols[tagValue].Index);
                }
                allChecked = false;

            }
            else
            {
                for (int row = GridRowsCount; row < grdSub.Rows.Count; row++)
                {
                    grdSub.SetData(row, tagValue, true);
                    SetupCheckedEvent(row, grdSub.Cols[tagValue].Index);
                }
                allChecked = true;

            }
        }

        /// <summary>
        /// 조회된 데이터 그리드에 세팅 
        /// </summary>
        /// <param name="grdItem"></param>
        /// <param name="dataTable"></param>
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {

                //그리드 콤보박스 지정
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("INQ_ACL", FlexGridCellTypeEnum.CheckBox, "true");
                FlexGridColumns.Add("REG_ACL", FlexGridCellTypeEnum.CheckBox, "true");
                FlexGridColumns.Add("MOD_ACL", FlexGridCellTypeEnum.CheckBox, "true");
                FlexGridColumns.Add("DEL_ACL", FlexGridCellTypeEnum.CheckBox, "true");
                FlexGridColumns.Add("TOTAL", FlexGridCellTypeEnum.CheckBox, "true");

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);

                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //마지막행 사이즈조절, 로우공백흰색
                grdMain.ExtendLastCol = true;
                grdMain.Styles.EmptyArea.BackColor = Color.White;
                grdSub.ExtendLastCol = true;
                grdSub.Styles.EmptyArea.BackColor = Color.White;

                grdMain.SelectionMode = SelectionModeEnum.Row;
                grdSub.SelectionMode = SelectionModeEnum.Row;

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndUpdate();
                grdItem.Invalidate();
            }
        }

        /// <summary>
        /// grdMain그리드 컬럼 스타일 적용
        /// </summary>
        /// <param name="grdItem"></param>
        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 250;
            grdItem.Cols[3].Width = 250;

            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "권한그룹ID";
            grdItem[1, 2] = "권한그룹명";
            grdItem[1, 3] = "비고";

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0);
            clsFlexGrid.DataGridLeftStyle(grdItem, 1, 3);

        }

        /// <summary>
        /// grdSub그리드 컬럼 스타일 적용
        /// </summary>
        /// <param name="grdItem"></param>
        private void FlexGridCol2(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;    //no
            grdItem.Cols[1].Width = 100;   //업무구분
            grdItem.Cols[2].Width = 300;   //화면명
            grdItem.Cols[3].Width = 85;    //전체
            grdItem.Cols[4].Width = 85;    //조회
            grdItem.Cols[5].Width = 85;    //등록
            grdItem.Cols[6].Width = 85;    //수정
            grdItem.Cols[7].Width = 85;    //삭제
            grdItem.Cols[8].Width = 50;    //시스템구분

            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "업무구분";
            grdItem[1, 2] = "화면명";
            grdItem[1, 3] = "전체";
            grdItem[1, 4] = "조회";
            grdItem[1, 5] = "등록";
            grdItem[1, 6] = "수정";
            grdItem[1, 7] = "삭제";
            grdItem[1, 8] = "시스템구분";

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);

            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            clsFlexGrid.DataGridLeftStyle(grdItem, 2);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3, 4);
            clsFlexGrid.DataGridCenterStyle(grdItem, 5, 6);
            clsFlexGrid.DataGridCenterStyle(grdItem, 7);
            clsFlexGrid.DataGridCenterStyle(grdItem, 8);

        }


        /// <summary>
        /// grdMain 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdMain_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count <= 1 || grdMain.RowSel < 0) { return; }
            //grdMain 로우 선택 함수
            grdMain_Row_Selected(grdMain.Row);
        }

        /// <summary>
        /// grdMain BeforeEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdMain_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            //수정여부 확인을 위해 저장
            if (e.Row < 1 || grd.GetData(e.Row, e.Col) == null)
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index )  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 메뉴 ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["ACL_GRP_ID"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            //수정여부 확인을 위해 저장
            strBefValue_Main = grd.GetData(e.Row, e.Col).ToString();

        }

        /// <summary>
        /// grdMain AfterEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdMain_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {

            C1FlexGrid grd = sender as C1FlexGrid;
            if(grdMain.Col == 0)
            {
                grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue_Main);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue_Main == grdMain.GetData(grdMain.Row, grdMain.Col).ToString())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdMain.GetData(grdMain.Row, "L_NUM").ToString() != "추가")
            {
                // ACL_GRP_ID 수정 불가
                if (grdMain.Col == grdMain.Cols["ACL_GRP_ID"].Index)
                {
                    grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue_Main);
                    return;
                }

                // 저장시 UPDATE로 처리하기 위해 flag set
                grdMain.SetData(grdMain.Row, 0, "수정");
                // Update 배경색 지정
                //grdMain.Rows[grdMain.Row].Style = grdMain.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
            }

        }

        /// <summary>
        /// GrdSub BeforeEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdSub_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue_Sub = grd.GetData(e.Row, e.Col).ToString();
        }

        /// <summary>
        /// GrdSub AfterEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdSub_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            int modifiedRow = e.Row;

            SetEditView(modifiedRow);
            // 추가된 열에 대한 수정은 INSERT 처리
            

        }

        private void SetEditView(int modifiedRow)
        {
            if (grdSub.GetData(modifiedRow, "L_NUM").ToString() != "추가")
            {
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdSub.SetData(modifiedRow, "L_NUM", "수정");

                // Update 배경색 지정
                clsFlexGrid.GridCellRangeStyleColor(grdSub, modifiedRow, 0, modifiedRow, grdSub.Cols.Count - 1, Color.Green, Color.Black);
            }
        }


        /// <summary>
        /// GrdSub_CellChecked 체크박스 클릭 이벤트 전체 선택 및 체크 ON/FF기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdSub_CellChecked(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            int checkedCol = e.Col;
            int checkedRow = e.Row;

            SetupCheckedEvent(checkedRow, checkedCol);

            
        }

        private void SetupCheckedEvent(int checkedRow, int checkedCol)
        {
            //전체클릭
            if (checkedCol == grdSub.Cols["TOTAL"].Index)
            {
                if (grdSub.GetCellCheck(checkedRow, checkedCol).ToString() == "Checked")
                {
                    grdSub.Rows[checkedRow]["INQ_ACL"] = "Y";
                    grdSub.Rows[checkedRow]["REG_ACL"] = "Y";
                    grdSub.Rows[checkedRow]["MOD_ACL"] = "Y";
                    grdSub.Rows[checkedRow]["DEL_ACL"] = "Y";

                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["INQ_ACL"].Index, CheckEnum.Checked);
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["REG_ACL"].Index, CheckEnum.Checked);
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["MOD_ACL"].Index, CheckEnum.Checked);
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["DEL_ACL"].Index, CheckEnum.Checked);

                    SetEditView(checkedRow);
                }
                else
                {
                    grdSub.Rows[checkedRow]["INQ_ACL"] = "N";
                    grdSub.Rows[checkedRow]["REG_ACL"] = "N";
                    grdSub.Rows[checkedRow]["MOD_ACL"] = "N";
                    grdSub.Rows[checkedRow]["DEL_ACL"] = "N";

                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["INQ_ACL"].Index, CheckEnum.Unchecked);
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["REG_ACL"].Index, CheckEnum.Unchecked);
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["MOD_ACL"].Index, CheckEnum.Unchecked);
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["DEL_ACL"].Index, CheckEnum.Unchecked);

                    SetEditView(checkedRow);
                }
            }
            //조회
            else if (checkedCol == grdSub.Cols["INQ_ACL"].Index)
            {
                if (grdSub.GetCellCheck(checkedRow, checkedCol).ToString() == "Checked")
                {
                    grdSub.Rows[checkedRow]["INQ_ACL"] = "Y";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["INQ_ACL"].Index, CheckEnum.Checked);

                    SetEditView(checkedRow);
                }
                else
                {
                    grdSub.Rows[checkedRow]["INQ_ACL"] = "N";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["INQ_ACL"].Index, CheckEnum.Unchecked);

                    SetEditView(checkedRow);
                }
            }
            //저장
            else if (checkedCol == grdSub.Cols["REG_ACL"].Index)
            {
                if (grdSub.GetCellCheck(checkedRow, checkedCol).ToString() == "Checked")
                {
                    grdSub.Rows[checkedRow]["REG_ACL"] = "Y";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["REG_ACL"].Index, CheckEnum.Checked);

                    SetEditView(checkedRow);

                }
                else
                {
                    grdSub.Rows[checkedRow]["REG_ACL"] = "N";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["REG_ACL"].Index, CheckEnum.Unchecked);

                    SetEditView(checkedRow);

                }
            }
            //수정
            else if (checkedCol == grdSub.Cols["MOD_ACL"].Index)
            {
                if (grdSub.GetCellCheck(checkedRow, checkedCol).ToString() == "Checked")
                {
                    grdSub.Rows[checkedRow]["MOD_ACL"] = "Y";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["MOD_ACL"].Index, CheckEnum.Checked);

                    SetEditView(checkedRow);
                }
                else
                {
                    grdSub.Rows[checkedRow]["MOD_ACL"] = "N";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["MOD_ACL"].Index, CheckEnum.Unchecked);

                    SetEditView(checkedRow);
                }
            }
            //삭제
            else if (checkedCol == grdSub.Cols["DEL_ACL"].Index)
            {
                if (grdSub.GetCellCheck(checkedRow, checkedCol).ToString() == "Checked")
                {
                    grdSub.Rows[checkedRow]["DEL_ACL"] = "Y";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["DEL_ACL"].Index, CheckEnum.Checked);

                    SetEditView(checkedRow);
                }
                else
                {
                    grdSub.Rows[checkedRow]["DEL_ACL"] = "N";
                    grdSub.SetCellCheck(checkedRow, grdSub.Cols["DEL_ACL"].Index, CheckEnum.Unchecked);

                    SetEditView(checkedRow);
                }
            }
        }

        /// <summary>
        /// 프로그램 초기화
        /// </summary>
        private void InitControl()
        {
            clsStyle.Style.InitPanel(panel1);

            // 그리드 초기화
            InitGrid();

        }

        /// <summary>
        /// 그리드 초기화
        /// </summary>
        private void InitGrid()
        {
            DrawGrid(grdMain);
            DrawGrid(grdSub);
        }


        /// <summary>
        /// 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    if (this.scrAuthInq != "Y")
                    {
                        //MessageBox.Show("조회 권한이 없습니다");
                        return;
                    }

                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");
                    
                    SetDataBinding();   // 조회버튼
                    break;

                case "btnRowAdd":
                    if (this.scrAuthReg != "Y")
                    {
                        MessageBox.Show("등록 권한이 없습니다");
                        return;
                    }

                    DataRowAdd();
                    break;

                case "btnDelRow":
                    if (this.scrAuthDel != "Y")
                    {
                        MessageBox.Show("삭제 권한이 없습니다");
                        return;
                    }

                    DataRowDel();
                    break;

                case "btnSave":
                    if (this.scrAuthMod != "Y")
                    {
                        MessageBox.Show("수정 권한이 없습니다");
                        return;
                    }

                    SaveData();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

        private void Pbx_Click(object sender, EventArgs e)
        {
            switch (((PictureBox)sender).Name)
            {

                case "btnRowAdd":
                    if (this.scrAuthReg != "Y")
                    {
                        MessageBox.Show("등록 권한이 없습니다");
                        return;
                    }

                    DataRowAdd();
                    break;

                case "btnDelRow":
                    if (this.scrAuthDel != "Y")
                    {
                        MessageBox.Show("삭제 권한이 없습니다");
                        return;
                    }

                    DataRowDel();
                    break;


            }
        }

        /// <summary>
        /// 조회
        /// </summary>
        private bool SetDataBinding()
        {     
            try
            {
                //SQL
                string Sql1 = string.Empty;

                Sql1 += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY ACL_GRP_ID ASC)) AS L_NUM ");
                Sql1 += string.Format("      ,ACL_GRP_ID                                                             ");
                Sql1 += string.Format("      ,ACL_GRP_NM                                                             ");
                Sql1 += string.Format("      ,REMARKS                                                                ");
                Sql1 += string.Format(" FROM TB_CM_ACL_GRP                                                           ");
                Sql1 += string.Format("ORDER BY ACL_GRP_ID                                                           ");

                //SQL 쿼리 조회 후 데이터셋 return
                olddt = cd.FindDataTable(Sql1);
                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddt);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            
            if (grdMain.Rows.Count > 2)
            {
                //grdMain 선택 함수
                grdMain_Row_Selected(2);
            }

            return true;
        }

        /// <summary>
        /// 행추가 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataRowAdd()
        {

            for (int i = 1; i < grdMain.Rows.Count; i++)
            {
                if (grdMain.Rows[i][0].ToString() == "추가")
                {
                    MessageBox.Show("한개씩만 추가할 수 있습니다.");
                    return;
                }

            }

            // 수정가능 하도록 열추가
            grdMain.AllowEditing = false;
            grdMain.Rows.Add();
            grdMain.Rows[grdMain.Rows.Count - 1].Height = DataRowsHeight;
            grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");
            //grdMain.SetData(grdMain.Rows.Count - 1, 6, "");
            for (int col = 1; col < grdMain.Cols.Count; col++)
                grdMain.SetData(grdMain.Rows.Count - 1, col, "");

            // Insert 배경색 지정
            //grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Rows.Count - 1, 0, grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, Color.Yellow, Color.Black);
            //// 커서위치 결정
            grdMain.Row = grdMain.Rows.Count - 1;
            grdMain.Col = 0;
        }

        /// <summary>
        /// 행삭제 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataRowDel()
        {
            if (grdMain.Rows.Count < 2 || grdMain.Row < 1)
            {
                return;
            }
            //구분 col 8
            //mj 추가되었지만 행삭제로 지울때
            if (grdMain.Rows[grdMain.RowSel]["L_NUM"].ToString() == "추가")
            {
                grdMain.RemoveItem(grdMain.RowSel);
                return;
            }

            // 저장시 Delete로 처리하기 위해 flag set
            // grdMain.Rows[grdMain.RowSel]["GUBUN"] = "삭제";
            grdMain.Rows[grdMain.RowSel]["L_NUM"] = "삭제";

            // Delete 배경색 지정
            //grdMain.Rows[grdMain.RowSel].Style = grdMain.Styles["DelColor"];
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Red, Color.Black);

            grdMain.Row = -1;
        }

        List<C1FlexGrid> gridList;
        /// <summary>
        /// 엑셀버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveExcel()
        {
            gridList = new List<C1FlexGrid>();
            gridList.Add(grdMain);
            gridList.Add(grdSub);

            //vf.SaveExcel(titleNM, selectedGrd);
            titleNM = "화면권한";
            grdMain.Tag = "권한 그룹";
            grdSub.Tag = "화면 권한 그룹";
            vf.SaveExcel(titleNM, gridList);
        }


        /// <summary>
        /// 저장클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void SaveData()
        {
            #region 데이터 체크
            string check_value = string.Empty;
            string check_Cols_NM = string.Empty;
            string check_field_NM = string.Empty;
            string check_table_NM = string.Empty;

            string check_keyColNM = string.Empty;
            string check_keyValue = string.Empty;

            string check_NM = string.Empty;

            string gubun_value = string.Empty;
            string show_msg = string.Empty;
            int checkrow = 0;

            bool isChange = false;
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "L_NUM").ToString();

                if (gubun_value == "삭제")
                {
                    isChange = true;

                    // 수정일 경우 명 확인...""
                }

                if (gubun_value == "수정")
                {
                    isChange = true;

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "ACL_GRP_NM";
                    //check_table_NM = "TB_CM_COM_CD_GRP";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                }
                if (gubun_value == "추가")
                {
                    check_field_NM = "ACL_GRP_ID";
                    check_table_NM = "TB_CM_ACL_GRP";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.isContainHangul(check_value))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        return;
                    }

                    if (vf.getStrLen(check_value) > 20)
                    {
                        MessageBox.Show("영문 및 숫자 20자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    {
                        show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "ACL_GRP_NM";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    isChange = true;
                }
            }
            for (checkrow = 1; checkrow < grdSub.Rows.Count; checkrow++)
            {

                gubun_value = grdSub.GetData(checkrow, "L_NUM").ToString();


                if (gubun_value == "수정")
                {
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
            #endregion 데이터 체크


            //grdMain 관련 추가 수정 함수
            string Sql1 = string.Empty;
            string strMsg = string.Empty;

            string index = "";
            string grp_mn = "";
            string screen_name = "";
            string can_search = "N";
            string can_modify = "N";
            string can_regist = "N";
            string can_remove = "N";
            string screen_id = "";

            int row = 0;
            int InsCnt = 0;
            int UpCnt = 0;
            int DelCnt = 0;

            List<string> delSublist = null;
            //디비선언
            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                // grdMain 관련부분
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    #region Insert 처리
                    if (grdMain.GetData(row, "L_NUM").ToString() == "추가")
                    {
                        Sql1 = string.Format(" INSERT INTO TB_CM_ACL_GRP ");
                        Sql1 += string.Format("             ( ");
                        Sql1 += string.Format("              ACL_GRP_ID ");
                        Sql1 += string.Format("             ,ACL_GRP_NM ");
                        Sql1 += string.Format("             ,REMARKS ");
                        Sql1 += string.Format("             ,REGISTER ");
                        Sql1 += string.Format("             ,REG_DDTT ");
                        Sql1 += string.Format("             ) ");
                        Sql1 += string.Format(" VALUES( ");
                        Sql1 += string.Format("          '{0}' ", grdMain.GetData(row, "ACL_GRP_ID"));
                        Sql1 += string.Format("         ,'{0}' ", grdMain.GetData(row, "ACL_GRP_NM"));
                        Sql1 += string.Format("         ,'{0}' ", grdMain.GetData(row, "REMARKS"));
                        Sql1 += string.Format("         ,'{0}' ", ck.UserID);
                        Sql1 += string.Format("         ,(select convert(varchar, getdate(), 120)) ");
                        Sql1 += string.Format("       ) ");


                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        InsCnt++;
                    }
                    #endregion

                    #region Update 처리
                    else if (grdMain.GetData(row, "L_NUM").ToString() == "수정")
                    {
                        Sql1 += string.Format(" UPDATE TB_CM_ACL_GRP ");
                        Sql1 += string.Format(" SET    ACL_GRP_NM = '{0}' ", grdMain.GetData(row, "ACL_GRP_NM"));
                        Sql1 += string.Format("       ,REMARKS    = '{0}' ", grdMain.GetData(row, "REMARKS"));
                        Sql1 += string.Format("       ,MODIFIER   = '{0}' ", ck.UserID);
                        Sql1 += string.Format("       ,MOD_DDTT   = (select convert(varchar, getdate(), 120)) ");
                        Sql1 += string.Format(" WHERE ACL_GRP_ID  = '{0}' ", grdMain.GetData(row, "ACL_GRP_ID"));

                        //Sql1 = string.Format("UPDATE TB_CM_ACL_GRP SET  ACL_GRP_NM = '{1}', REMARKS = '{2}' WHERE ACL_GRP_ID = '{0}'",
                        //    grdMain.GetData(row, "ACL_GRP_ID"), grdMain.GetData(row, "ACL_GRP_NM"), grdMain.GetData(row, "REMARKS"));

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        UpCnt++;
                    }
                    #endregion

                    #region delete 처리
                    else if (grdMain.GetData(row, "L_NUM").ToString() == "삭제")
                    {
                        // category 항목에 따른 데이터가 있는경우 같이 없애준다.

                        delSublist = new List<string>();
                        Sql1 = string.Format(" SELECT SCR_ID FROM    TB_CM_ACL  WHERE ACL_GRP_ID = '{0}'", grdMain.GetData(row, "ACL_GRP_ID"));
                       // DataTable dt = cd.FindDataTable(Sql1);
                        DataSet dset = new DataSet();
                        DataTable dt = null;
                        SqlDataAdapter olda = null;

                        cmd.CommandText = Sql1;
                        olda = new SqlDataAdapter();
                        olda.SelectCommand = cmd;
                        olda.Fill(dset, "DataTable");
                        dt = dset.Tables[0];


                        foreach (DataRow row_data in dt.Rows) // Loop over the rows.
                        {
                            delSublist.Add(row_data["SCR_ID"].ToString());
                            Sql1 = string.Format(" DELETE FROM TB_CM_ACL WHERE  SCR_ID = '{0}' AND ACL_GRP_ID = '{1}'", row_data["SCR_ID"].ToString(), grdMain.GetData(row, "ACL_GRP_ID").ToString());
                            cmd.CommandText = Sql1;
                            cmd.ExecuteNonQuery();
                        }

                        //권한그룹 삭제
                        Sql1 += string.Format(" DELETE FROM TB_CM_ACL_GRP                                   ");
                        Sql1 += string.Format("  WHERE ACL_GRP_ID = '"+grdMain.GetData(row, "ACL_GRP_ID")+"'");
                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        ////유저권한 삭제
                        //Sql1 += string.Format("/*2017.06.16 유저권한 삭제 by 정호준*/
                        //Sql1 += string.Format("DELETE FROM TB_CM_USER_ACL                                      ");
                        //Sql1 += string.Format(" WHERE ACL_GRP_ID = '" + grdMain.GetData(row, "ACL_GRP_ID") + "'");

            cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();


                        DelCnt++;

                    }
                    #endregion
                }

                for (row = 1; row < grdSub.Rows.Count; row++)
                {
                    index = grdSub.GetData(row, "L_NUM").ToString();             // 구분

                    if (index != "")
                    {
                        grp_mn = grdSub.GetData(row, "BIZ_GP").ToString();                 // 업무구분
                        screen_name = grdSub.GetData(row, "SCR_NM").ToString();            // 화면 명
                        can_search = grdSub.GetData(row, "INQ_ACL").ToString();    // 조회 가능
                        can_regist = grdSub.GetData(row, "REG_ACL").ToString();    // 등록 가능
                        can_modify = grdSub.GetData(row, "MOD_ACL").ToString();    // 수정 가능
                        can_remove = grdSub.GetData(row, "DEL_ACL").ToString();    // 삭제 가능

                        //Insert/Update
                        if (index == "수정")
                        {
                            //화면명으로 화면ID 구하기
                            screen_id = ScreenId(grp_mn, screen_name, cbxSys_id);

                            //자료존재여부 Check (존재:Update   미존재:Insert)
                            //Sql1 = "";
                            Sql1 = "SELECT INQ_ACL";
                            Sql1 += "  FROM TB_CM_ACL";
                            Sql1 += " WHERE ACL_GRP_ID = " + "'" + selected_grp_id + "'";
                            Sql1 += "   AND SCR_ID = (SELECT SCR_ID FROM TB_CM_SCR WHERE BIZ_GP = '" + grp_mn + "'";
                            Sql1 += "                                                AND SCR_NM = '" + screen_name + "')";

                            

                            DataSet dset = new DataSet();
                            DataTable dt = null;
                            SqlDataAdapter olda = null;

                            cmd.CommandText = Sql1;
                            olda = new SqlDataAdapter();
                            olda.SelectCommand = cmd;
                            olda.Fill(dset, "DataTable");
                            dt = dset.Tables[0];
                            


                            //Insert
                            if (dt == null || dt.Rows.Count == 0)
                            {
                                
                                Sql1 = string.Format(" INSERT INTO TB_CM_ACL ");
                                Sql1 += string.Format("             ( ");
                                Sql1 += string.Format("               ACL_GRP_ID ");
                                Sql1 += string.Format("              ,SCR_ID ");
                                Sql1 += string.Format("              ,INQ_ACL ");
                                Sql1 += string.Format("              ,REG_ACL ");
                                Sql1 += string.Format("              ,MOD_ACL ");
                                Sql1 += string.Format("              ,DEL_ACL ");
                                Sql1 += string.Format("              ,REGISTER ");
                                Sql1 += string.Format("              ,REG_DDTT ");
                                Sql1 += string.Format("             ) ");
                                Sql1 += string.Format(" VALUES ");
                                Sql1 += string.Format("      ( ");
                                Sql1 += string.Format("        '{0}' ", selected_grp_id);      //ACL_GRP_ID 
                                Sql1 += string.Format("       ,'{0}' ", screen_id);            //SCR_ID        
                                Sql1 += string.Format("       ,'{0}' ", vf.StringToString(can_search).ToString());           //INQ_ACL
                                Sql1 += string.Format("       ,'{0}' ", vf.StringToString(can_regist).ToString());           //REG_ACL
                                Sql1 += string.Format("       ,'{0}' ", vf.StringToString(can_modify).ToString());           //MOD_ACL
                                Sql1 += string.Format("       ,'{0}' ", vf.StringToString(can_remove).ToString());           //DEL_ACL 
                                Sql1 += string.Format("       ,'{0}' ", ck.UserID);            //REGISTER 
                                Sql1 += string.Format("       ,(select convert(varchar, getdate(), 120)) ");                     //REG_DDTT   
                                Sql1 += string.Format("      ) ");

                                //Console.WriteLine(Sql1);
                                //Sql1 = string.Format("INSERT INTO TB_CM_ACL (ACL_GRP_ID, SCR_ID, INQ_ACL, REG_ACL, MOD_ACL, DEL_ACL) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                //                        selected_grp_id, screen_id, can_search, can_regist, can_modify, can_remove);

                                cmd.CommandText = Sql1;
                                cmd.ExecuteNonQuery();

                                InsCnt++;
                            }
                            //Delete, Update
                            else
                            {
                                //Delete
                                if (can_search == "N" && can_regist == "N" && can_modify == "N" && can_remove == "N")
                                {
                                    Sql1 = string.Format(" DELETE FROM TB_CM_ACL WHERE ACL_GRP_ID = '{0}' AND SCR_ID = '{1}' ", selected_grp_id, screen_id);

                                    cmd.CommandText = Sql1;
                                    cmd.ExecuteNonQuery();
                                    DelCnt++;
                                }
                                //Update
                                else
                                {
                                    Sql1 = string.Format(" UPDATE TB_CM_ACL ");
                                    Sql1 += string.Format(" SET    INQ_ACL  = '{0}'   ", vf.StringToString(can_search).ToString());
                                    Sql1 += string.Format("       ,REG_ACL  = '{0}'   ", vf.StringToString(can_regist).ToString());
                                    Sql1 += string.Format("       ,MOD_ACL  = '{0}'   ", vf.StringToString(can_modify).ToString());
                                    Sql1 += string.Format("       ,DEL_ACL  = '{0}'   ", vf.StringToString(can_remove).ToString());
                                    Sql1 += string.Format("       ,MODIFIER = '{0}'   ", ck.UserID);
                                    Sql1 += string.Format("       ,MOD_DDTT = (select convert(varchar, getdate(), 120)) ");
                                    Sql1 += string.Format(" WHERE ACL_GRP_ID = '{0}'  ", selected_grp_id);
                                    Sql1 += string.Format(" AND   SCR_ID = '{0}'      ", screen_id);
                                    
                                    //Sql1 = string.Format("UPDATE TB_CM_ACL SET INQ_ACL = '{0}', REG_ACL = '{1}', MOD_ACL = '{2}', DEL_ACL = '{3}' WHERE ACL_GRP_ID = '{4}' AND SCR_ID = '{5}'",
                                    //                        can_search, can_regist, can_modify, can_remove, selected_grp_id, screen_id);

                                    cmd.CommandText = Sql1;
                                    cmd.ExecuteNonQuery();
                                    
                                    UpCnt++;
                                }
                            }
 
                        }
                    }
                }

                //실행후 성공
                transaction.Commit();
                //SetDataBinding();
                Button_Click(btnDisplay, null);

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), message);

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
            return;

        }

        /// <summary>
        /// grdMain 로우 선택 함수
        /// </summary>
        /// <param name="selectedRow"></param>
        private void grdMain_Row_Selected(int selectedRow)
        {
            //InitGridData_Sub();

            selectedGrd = grdMain;
            
            selected_grp_id = grdMain.Rows[selectedRow]["ACL_GRP_ID"].ToString().Trim();    //그룹ID

            string Sql1 = "";

            try
            {
                Sql1 += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY SORT_SEQ, BIZ_GP, SCR_NM ASC)) AS L_NUM ");
                Sql1 += string.Format("      ,BIZ_GP                                                                               ");
                //Sql1 += string.Format("      ,SORT_SEQ                                                                             ");
                //Sql1 += string.Format("      ,ACL_GRP_ID                                                                           ");
                Sql1 += string.Format("      ,SCR_NM                                                                               ");
                //Sql1 += string.Format("      ,'N' AS TOTAL                                                                         ");
                //Sql1 += string.Format("      ,INQ_ACL                                                                              ");
                //Sql1 += string.Format("      ,REG_ACL                                                                              ");
                //Sql1 += string.Format("      ,MOD_ACL                                                                              ");
                //Sql1 += string.Format("      ,DEL_ACL                                                                              ");
                Sql1 += string.Format("      ,'False' AS TOTAL                                                                         ");
                Sql1 += string.Format("      ,(CASE WHEN INQ_ACL = 'Y' THEN 'True' ELSE 'False' END) INQ_ACL                       ");
                Sql1 += string.Format("      ,(CASE WHEN REG_ACL = 'Y' THEN 'True' ELSE 'False' END) REG_ACL                       ");
                Sql1 += string.Format("      ,(CASE WHEN MOD_ACL = 'Y' THEN 'True' ELSE 'False' END) MOD_ACL                       ");
                Sql1 += string.Format("      ,(CASE WHEN DEL_ACL = 'Y' THEN 'True' ELSE 'False' END) DEL_ACL                       ");
                Sql1 += string.Format("  FROM (SELECT '1' AS SORT_SEQ                                                              ");
                Sql1 += string.Format("              ,A.ACL_GRP_ID AS ACL_GRP_ID                                                   ");
                Sql1 += string.Format("              ,B.BIZ_GP     AS BIZ_GP                                                       ");
                Sql1 += string.Format("              ,B.SCR_NM     AS SCR_NM                                                       ");
                Sql1 += string.Format("              ,A.INQ_ACL    AS INQ_ACL                                                      ");
                Sql1 += string.Format("              ,A.REG_ACL    AS REG_ACL                                                      ");
                Sql1 += string.Format("              ,A.MOD_ACL    AS MOD_ACL                                                      ");
                Sql1 += string.Format("              ,A.DEL_ACL    AS DEL_ACL                                                      ");
                Sql1 += string.Format("              ,A.REGISTER   AS REGISTER                                                     ");
                Sql1 += string.Format("              ,A.REG_DDTT   AS REG_DDTT                                                     ");
                Sql1 += string.Format("              ,A.MODIFIER   AS MODIFIER                                                     ");
                Sql1 += string.Format("              ,A.MOD_DDTT   AS MOD_DDTT                                                     ");
                Sql1 += string.Format("          FROM TB_CM_ACL A                                                                  ");
                Sql1 += string.Format("              ,TB_CM_SCR B                                                                  ");
                Sql1 += string.Format("         WHERE A.SCR_ID = B.SCR_ID                                                          ");
                Sql1 += string.Format("           AND A.ACL_GRP_ID = '"+ selected_grp_id + "'                                      ");
                Sql1 += string.Format("         UNION                                                                              ");
                Sql1 += string.Format("        SELECT '2'  AS SORT_SEQ                                                             ");
                Sql1 += string.Format("              ,''          AS ACL_GRP_ID                                                    ");
                Sql1 += string.Format("              ,A.BIZ_GP AS BIZ_GP                                                           ");
                Sql1 += string.Format("              ,A.SCR_NM AS SCR_NM                                                           ");
                Sql1 += string.Format("              ,'N'  AS INQ_ACL                                                              ");
                Sql1 += string.Format("              ,'N'  AS REG_ACL                                                              ");
                Sql1 += string.Format("              ,'N'  AS MOD_ACL                                                              ");
                Sql1 += string.Format("              ,'N'  AS DEL_ACL                                                              ");
                Sql1 += string.Format("              ,NULL AS REGISTER                                                             ");
                Sql1 += string.Format("              ,NULL AS REG_DDTT                                                             ");
                Sql1 += string.Format("              ,NULL AS MODIFIER                                                             ");
                Sql1 += string.Format("              ,NULL AS MOD_DDTT                                                             ");
                Sql1 += string.Format("          FROM TB_CM_SCR A                                                                  ");
                Sql1 += string.Format("         WHERE NOT EXISTS (SELECT X.SCR_ID                                     ");
                Sql1 += string.Format("                             FROM TB_CM_ACL X                                               ");
                Sql1 += string.Format("                            WHERE X.SCR_ID = A.SCR_ID                                       ");
                Sql1 += string.Format("                              AND X.ACL_GRP_ID = '"+ selected_grp_id + "') ) Z              ");

                olddt_sub = cd.FindDataTable(Sql1);

                moddt_sub = olddt_sub.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드 세팅
                DrawGrid(grdSub, moddt_sub);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt_sub.Rows.Count.ToString() + "건이 조회 되었습니다.");
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt_sub.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }

        //상세그리드 저장 -> CM_권한


        //화면명으로 화면ID 구하기
        // sBizGp  : 업무구분
        // ScrName : 화면명
        private string ScreenId(string sBizGp, string ScrName, string cbxSys_id)
        {
            string Sql = "";
            DataTable dt = new DataTable();

            //화면명으로 화면ID 구하기

            Sql = " SELECT SCR_ID";
            Sql += "  FROM TB_CM_SCR";
            Sql += " WHERE BIZ_GP = " + "'" + sBizGp + "'";
            Sql += "   AND SCR_NM = " + "'" + ScrName + "'";
            dt = cd.FindDataTable(Sql);

            return dt.Rows[0]["SCR_ID"].ToString();
        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            grdMain.AllowEditing = true;
        }

        private void grdSub_Paint(object sender, PaintEventArgs e)
        {
            foreach (HostedControl hosted in _al)
                hosted.UpdatePosition();
        }
    }

    
}
