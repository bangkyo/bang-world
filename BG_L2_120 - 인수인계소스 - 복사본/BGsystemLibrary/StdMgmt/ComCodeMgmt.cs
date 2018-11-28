using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BGsystemLibrary.SystemMgmt
{

    public partial class ComCodeMgmt : Form
    {
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        clsUtil cu = new clsUtil();

        //데이터테이블
        DataTable olddt;
        DataTable moddt;
        DataTable olddt_sub;
        DataTable moddt_sub;
        DataTable grdMainDT;
        DataTable grdSubDT;

        TextBox tbCategory;
        TextBox tbCD_ID;


        List<string> msg;
        bool _CanSaveSearchLog = false;
        string selected_Category = "";

        // 셀의 수정전 값
        string strBefValue = "";
        string strBefValue2 = "";

        string ownerNM = "";
        string titleNM = "";

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        static C1FlexGrid selectedGrd;
        private int GridRowsCount = 2;
        private int GridColsCount = 3;
        private int GridColsCount2 = 8;
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

        public ComCodeMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            //권한관련 add [[
            string[] scrAuthParams = scrAuth.Split(',');

            this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장
            //권한관련 add ]]

            InitializeComponent();

            Load += ComCodeMgmt_Load;


            grdMain.RowColChange += GrdMain_RowColChange;
            grdMain.AfterEdit += GrdMain_AfterEdit;
            grdMain.BeforeEdit += GrdMain_BeforeEdit;
            grdMain.DoubleClick += GrdMain_DoubleClick;
            grdMain.Click += grdMain_Click;



            grdSub.RowColChange += GrdSub_RowColChange;
            grdSub.AfterEdit += GrdSub_AfterEdit;
            grdSub.BeforeEdit += GrdSub_BeforeEdit;
            grdSub.DoubleClick += GrdSub_DoubleClick;

        }
        //그리드 초기화
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            try
            {
                if (grdItem.Rows.Count == 1)
                {
                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                }
                else
                {
                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, grdItem.Rows.Count, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, grdItem.Rows.Count, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                }

                FlexGridCol(grdItem);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdMain.SelectionMode = SelectionModeEnum.Row;
                grdSub.SelectionMode = SelectionModeEnum.Row;

                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
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
        //조회 그리드 세팅
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginInit();
            grdItem.Rows.RemoveRange(1, grdItem.Rows.Count - 1);
            try
            {
                if (dataTable.Rows.Count == 0)
                {
                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        for (int intCol = 0; intCol < dataTable.Columns.Count; intCol++)
                        {
                            grdItem.Cols[intCol].Name = dataTable.Columns[intCol].ColumnName;
                        }
                    }
                }
                else
                {
                    if (grdItem.Name.ToString() == "grdMain")
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, dataTable.Rows.Count + GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, dataTable.Rows.Count + GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else
                    {
                        //clsFlexGrid.FlexGridMain(grdItem, dataTable.Rows.Count + GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                        clsFlexGrid.FlexGridMainSystem(grdItem, dataTable.Rows.Count + GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                }

                FlexGridCol(grdItem);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("USE_YN", FlexGridCellTypeEnum.CheckBox, "Y");

                grdItem.AllowEditing = true;
                grdItem.Cols[1].AllowEditing = true;
                grdItem.Cols[2].AllowEditing = true;

                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns);

                //마지막행 사이즈조절, 로우공백흰색
                grdMain.ExtendLastCol = true;
                grdMain.Styles.EmptyArea.BackColor = Color.White;
                grdSub.ExtendLastCol = true;
                grdSub.Styles.EmptyArea.BackColor = Color.White;
                //RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable);

                grdMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdSub.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
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
        TextBox tbEditor;
        //그리드 스타일 세팅
        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {

            if (grdItem.Name.ToString() == "grdMain")
            {
                grdItem.Cols[0].Width = 60;
                grdItem.Cols[1].Width = 200;
                grdItem.Cols[2].Width = 200;

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "그룹코드";
                grdItem[1, 2] = "그룹명";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0);
                clsFlexGrid.DataGridLeftStyle(grdItem, 1, 2);

                tbEditor = new TextBox();

                tbEditor.MaxLength = 10;
                tbEditor.CharacterCasing = CharacterCasing.Upper;

                grdItem.Cols[1].Editor = tbEditor;
            }
            else
            {
                grdItem.Cols[0].Width = 60;
                grdItem.Cols[1].Width = 150;
                grdItem.Cols[2].Width = 150;
                grdItem.Cols[3].Width = 150;
                grdItem.Cols[4].Width = 150;
                grdItem.Cols[5].Width = 150;
                grdItem.Cols[6].Width = 80;
                grdItem.Cols[7].Width = 80;

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "코드";
                grdItem[1, 2] = "코드명";
                grdItem[1, 3] = "코드명2";
                grdItem[1, 4] = "코드명3";
                grdItem[1, 5] = "코드명4";
                grdItem[1, 6] = "정렬순서";
                grdItem[1, 7] = "사용여부";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 7);
                clsFlexGrid.DataGridLeftStyle(grdItem, 2, 5);

                tbEditor = new TextBox();

                tbEditor.MaxLength = 10;
                tbEditor.CharacterCasing = CharacterCasing.Upper;

                grdItem.Cols[1].Editor = tbEditor;

            }
        }
        //버튼 클릭 이벤트
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

                    SetDataBinding();  // 조회 버튼을 통한 데이터입력

                    break;

                case "btnRowAdd1":
                    if (this.scrAuthReg != "Y")
                    {
                        MessageBox.Show("등록 권한이 없습니다");
                        return;
                    }

                    DataRowAdd();
                    break;

                case "btnDelRow1":
                    if (this.scrAuthDel != "Y")
                    {
                        MessageBox.Show("삭제 권한이 없습니다");
                        return;
                    }

                    DataRowDel();
                    break;

                case "btnRowAdd2":
                    if (this.scrAuthReg != "Y")
                    {
                        MessageBox.Show("등록 권한이 없습니다");
                        return;
                    }

                    DataRowAdd2();
                    break;

                case "btnDelRow2":
                    if (this.scrAuthDel != "Y")
                    {
                        MessageBox.Show("삭제 권한이 없습니다");
                        return;
                    }

                    DataRowDel2(grdSub.Row);
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

        private void GrdSub_RowColChange(object sender, EventArgs e)
        {

        }


        private void InitControl()
        {

            clsStyle.Style.InitPanel(panel1);

            DrawGrid(grdMain);
            DrawGrid(grdSub);
        }




        private void GrdMain_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain.Row <= 0)
            {
                return;
            }

            grdMain.AllowEditing = true;
        }

        private void GrdSub_DoubleClick(object sender, EventArgs e)
        {
            if (grdSub.Row <= 0)
            {
                return;
            }

            grdSub.AllowEditing = true;
        }

        private void GrdMain_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (e.Row <= 0)
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 CATEGORY ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["CATEGORY"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(e.Row, e.Col).ToString();
        }

        private void GrdMain_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {

            // No,구분은 수정 불가
            if (grdMain.Col == 0)
            {
                grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdMain.GetData(grdMain.Row, grdMain.Col).ToString())
                return;

            //   
            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdMain.GetData(grdMain.Row, 0).ToString() != "추가")
            {
                // 화면ID 수정 불가
                if (grdMain.Col == 1)
                {
                    grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdMain.SetData(grdMain.Row, 0, "수정");

                // Update 배경색 지정
                //grdMain.Rows[grdMain.Row].Style = grdMain.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
            }


        }

        private void GrdSub_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            if (grd.Row < 1 || grd.GetData(grd.Row, grd.Col) == null)
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 CD ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["CD_ID"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            if (grd.Rows[e.Row]["L_NUM"].ToString() != "추가")
            {
                // 수정여부 확인을 위해 저장
                strBefValue = grd.GetData(e.Row, e.Col).ToString();
            }
        }

        private void GrdSub_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            int editedRow = e.Row;
            int editedCol = e.Col;
            // No,구분은 수정 불가
            if (editedRow == 0)
            {
                grdSub.SetData(editedRow, editedCol, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdSub.GetData(editedRow, editedCol).ToString())
                return;

            //   
            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdSub.GetData(editedRow, 0).ToString() != "추가")
            {
                // 코드 수정 불가
                if (grdSub.Col == 1)
                {
                    grdSub.SetData(editedRow, editedCol, strBefValue);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdSub.SetData(editedRow, 0, "수정");

                // Update 배경색 지정
                //grdSub.Rows[grdSub.Row].Style = grdSub.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(grdSub, editedRow, 0, editedRow, grdSub.Cols.Count - 1, Color.Green, Color.Black);
            }

        }
        private void GrdMain_RowColChange(object sender, EventArgs e)
        {

        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, selectedGrd);
        }

        private void SaveData()
        {
            grdMain.Row = 0;
            grdMain.Col = 0;

            grdSub.Row = 0;
            grdSub.Col = 0;

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
            for (checkrow = GridRowsCount; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "L_NUM").ToString();

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;

                    // 수정일 경우 명 확인...""
                }


                if (gubun_value == "추가")
                {
                    check_field_NM = "CATEGORY";
                    check_table_NM = "TB_CM_COM_CD_GRP";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("그룹코드가 입력하세요.");
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.isContainHangul(check_value))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        return;
                    }

                    if (vf.getStrLen(check_value) > 30)
                    {
                        MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    {
                        show_msg = string.Format("그룹코드가 중복되었습니다. 다시 입력해주세요.");
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "CATEGORY_NM";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("그룹명을 입력하세요.");
                        MessageBox.Show(show_msg);
                        return;
                    }

                    check_field_NM = "CATEGORY";
                    check_table_NM = "TB_CM_COM_CD_GRP";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);


                    isChange = true;
                }

            }

            for (checkrow = GridRowsCount; checkrow < grdSub.Rows.Count; checkrow++)
            {

                gubun_value = grdSub.GetData(checkrow, "L_NUM").ToString();


                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }

                if (gubun_value == "추가")
                {
                    check_keyColNM = "CATEGORY";
                    check_keyValue = selected_Category;

                    check_field_NM = "CD_ID";
                    check_table_NM = "TB_CM_COM_CD";
                    check_value = grdSub.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdSub.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("코드를 입력하세요.");
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.isContainHangul(check_value))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        return;
                    }

                    if (vf.getStrLen(check_value) > 10)
                    {
                        MessageBox.Show("영문 및 숫자 10자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value, check_keyColNM, check_keyValue))
                    {
                        show_msg = string.Format("코드가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "CD_NM";
                    if (grdSub.GetData(checkrow, check_field_NM) == null)
                    {
                        show_msg = string.Format("코드명을 입력하세요.");
                        MessageBox.Show(show_msg);
                        return;
                    }
                    check_NM = grdSub.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdSub.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("코드명을 입력하세요.");
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
            #endregion 데이터 체크

            string Sql1 = string.Empty;
            string strMsg = string.Empty;

            int row = 0;
            int InsCnt = 0;
            int UpCnt = 0;
            int DelCnt = 0;
            List<string> delSublist = null;

            DataTable dt = null;

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

                #region grdMain 추가,수정,삭제 처리
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    // Insert 처리
                    if (grdMain.GetData(row, 0).ToString() == "추가")
                    {
                        Sql1 = string.Format(" INSERT INTO TB_CM_COM_CD_GRP ");
                        Sql1 += string.Format("             ( ");
                        Sql1 += string.Format("                CATEGORY ");
                        Sql1 += string.Format("               ,CATEGORY_NM ");
                        Sql1 += string.Format("               ,USE_YN ");
                        Sql1 += string.Format("               ,REGISTER ");
                        Sql1 += string.Format("               ,REG_DDTT ");
                        Sql1 += string.Format("             ) ");
                        Sql1 += string.Format(" VALUES      ( ");
                        Sql1 += string.Format("               '{0}' ", grdMain.GetData(row, "CATEGORY"));
                        Sql1 += string.Format("              ,'{0}' ", grdMain.GetData(row, "CATEGORY_NM"));
                        Sql1 += string.Format("              ,'{0}' ", "Y");
                        Sql1 += string.Format("              ,'{0}' ", ck.UserID);
                        Sql1 += string.Format("              ,(select convert(varchar, getdate(), 120)) ");
                        Sql1 += string.Format("             ) ");


                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        InsCnt++;

                    }
                    // Update 처리
                    else if (grdMain.GetData(row, 0).ToString() == "수정")
                    {
                        Sql1 = string.Format(" UPDATE TB_CM_COM_CD_GRP ");
                        Sql1 += string.Format(" SET    CATEGORY_NM  = '{0}' ", grdMain.GetData(row, "CATEGORY_NM"));
                        Sql1 += string.Format("       ,MODIFIER     = '{0}' ", ck.UserID);
                        Sql1 += string.Format("       ,MOD_DDTT     = (select convert(varchar, getdate(), 120)) ");
                        Sql1 += string.Format(" WHERE CATEGORY      = '{0}' ", grdMain.GetData(row, "CATEGORY"));

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        strMsg = " 그룹: " + grdMain.GetData(row, "CATEGORY").ToString() + " 그룹명: " + olddt.Rows[row - 1]["CATEGORY_NM"].ToString() + " To " + grdMain.GetData(row, "CATEGORY_NM").ToString() + "로 수정 ";

                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString() == "삭제")
                    {
                        // category 항목에 따른 데이터가 있는경우 같이 없애준다.

                        delSublist = new List<string>();
                        Sql1 = string.Format("SELECT CD_ID FROM TB_CM_COM_CD WHERE CATEGORY = '{0}'", grdMain.GetData(row, "CATEGORY"));
                        dt = cd.FindDataTable(Sql1);

                        foreach (DataRow row_data in dt.Rows) // Loop over the rows.
                        {
                            delSublist.Add(row_data.ItemArray[0].ToString());
                            Sql1 = string.Format("DELETE FROM TB_CM_COM_CD WHERE CD_ID = '{0}' AND CATEGORY = '{1}'", row_data.ItemArray[0], grdMain.GetData(row, "CATEGORY"));
                            cmd.CommandText = Sql1;
                            cmd.ExecuteNonQuery();
                        }

                        Sql1 = string.Format("DELETE FROM TB_CM_COM_CD_GRP WHERE CATEGORY = '{0}'", grdMain.GetData(row, "CATEGORY"));

                        strMsg = " 그룹: " + grdMain.GetData(row, "CATEGORY").ToString() + "를 삭제 ";

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        DelCnt++;

                    }

                }
                #endregion

                #region gridSub 추가 수정 삭제 처리
                for (row = 1; row < grdSub.Rows.Count; row++)
                {
                    // Insert 처리 

                    if (grdSub.GetData(row, 0).ToString() == "추가")
                    {


                        Sql1 = string.Format(" INSERT INTO TB_CM_COM_CD  ");
                        Sql1 += string.Format("             ( ");
                        Sql1 += string.Format("               CATEGORY   ");
                        Sql1 += string.Format("              ,CD_ID      ");
                        Sql1 += string.Format("              ,CD_NM      ");
                        Sql1 += string.Format("              ,COLUMNA    ");
                        Sql1 += string.Format("              ,COLUMNB    ");
                        Sql1 += string.Format("              ,COLUMNC    ");
                        //Sql1 += string.Format("              ,COLUMNE    ");
                        if (grdSub.GetData(row, "SORT_SEQ") != null )
                        {
                            if (grdSub.GetData(row, "SORT_SEQ").ToString().Length > 0)
                            {
                                Sql1 += string.Format("              ,SORT_SEQ   ");
                            }
                        }
                        Sql1 += string.Format("              ,USE_YN     ");
                        Sql1 += string.Format("              ,REGISTER   ");
                        Sql1 += string.Format("              ,REG_DDTT   ");
                        Sql1 += string.Format("             )            ");
                        Sql1 += string.Format(" VALUES( ");
                        Sql1 += string.Format("         '{0}'   ", selected_Category);    //"CATEGORY,  " 0
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "CD_ID")); //"CD_ID,     " 1
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "CD_NM")); //"CD_NM,     " 2
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMNA"));               //"COLUMNA,   " 3
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMNB"));  //"COLUMNB,   " 4
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMNC"));  //"COLUMNC,   " 5
                        //Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMNE"));  //"COLUMNC,   " 5
                        if (grdSub.GetData(row, "SORT_SEQ") != null)
                        {
                            if (grdSub.GetData(row, "SORT_SEQ").ToString().Length > 0)
                            {
                                Sql1 += string.Format("        ,'{0}'   ", Int32.Parse(grdSub.GetData(row, "SORT_SEQ").ToString()));  //"SORT_SEQ,  " 6
                            }
                        }

                        Sql1 += string.Format("        ,'{0}'   ", vf.BoolToString((Boolean)grdSub.GetData(row, "USE_YN")));// grdSub.GetData(row, "USE_YN").ToString());  //"USE_YN,    " 7
                        Sql1 += string.Format("        ,'{0}'   ", ck.UserID);  //"REGISTER,  " 15
                        Sql1 += string.Format("        ,(select convert(varchar, getdate(), 120)) ");  //"REG_DDTT,  " 16
                        Sql1 += string.Format("       ) ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        InsCnt++;
                    }
                    // Update 처리
                    else if (grdSub.GetData(row, 0).ToString() == "수정")
                    {

                        Sql1 = string.Format(" UPDATE TB_CM_COM_CD SET  ");
                        Sql1 += string.Format("         CD_NM = '{0}'       ", grdSub.GetData(row, "CD_NM"));
                        Sql1 += string.Format("        ,COLUMNA = '{0}'     ", grdSub.GetData(row, "COLUMNA"));
                        Sql1 += string.Format("        ,COLUMNB = '{0}'     ", grdSub.GetData(row, "COLUMNB"));
                        Sql1 += string.Format("        ,COLUMNC = '{0}'     ", grdSub.GetData(row, "COLUMNC"));
                        //Sql1 += string.Format("        ,COLUMNE = '{0}'     ", grdSub.GetData(row, "COLUMNE"));
                        if (grdSub.GetData(row, "SORT_SEQ").ToString().Length > 0 & grdSub.GetData(row, "SORT_SEQ") != null)
                        {
                            Sql1 += string.Format("        ,SORT_SEQ = '{0}'    ", grdSub.GetData(row, "SORT_SEQ"));
                        }
                        Sql1 += string.Format("        ,USE_YN = '{0}'      ", vf.BoolToString((Boolean)grdSub.GetData(row, "USE_YN")));// grdSub.GetData(row, "USE_YN").ToString());//vf.BoolToString((Boolean)grdSub.GetData(row, "USE_YN")));
                        Sql1 += string.Format("        ,MODIFIER = '{0}'    ", ck.UserID);
                        Sql1 += string.Format("        ,MOD_DDTT = (select convert(varchar, getdate(), 120))  ");
                        Sql1 += string.Format(" WHERE CATEGORY = '{0}' ", selected_Category);
                        Sql1 += string.Format(" and   CD_ID    = '{0}' ", grdSub.GetData(row, "CD_ID"));

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();


                        UpCnt++;

                    }
                    else if (grdSub.GetData(row, 0).ToString() == "삭제")
                    {

                        Sql1 = string.Format("DELETE FROM TB_CM_COM_CD WHERE CD_ID = '{0}' AND CATEGORY = '{1}'", grdSub.GetData(row, "CD_ID"), selected_Category);

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        DelCnt++;

                    }

                }// end of for(GridSub) 
                #endregion

                //실행후 성공
                transaction.Commit();


                Button_Click(btnDisplay, null);

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
                MessageBox.Show("저장에 실패하였습니다. Error:" + ex.Message);
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
            grdMain.AllowEditing = true;
            grdMain.Rows.Add();
            grdMain.Rows[grdMain.Rows.Count - 1].Height = DataRowsHeight;

            // grdMain.Rows[grdMain.Rows.Count - 1]["CD_NM"] = "";

            // 추가열 데이타 초기화
            for (int i = 1; i < grdMain.Cols.Count; i++)
                grdMain.SetData(grdMain.Rows.Count - 1, i, "");

            // 1행에 Count 자동 입력
            grdMain.SetData(grdMain.Rows.Count - 1, 0, (grdMain.Rows.Count - 1).ToString());

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");

            // Insert 배경색 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Rows.Count - 1, 0, grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, Color.Yellow, Color.Black);

            grdMain.Row = grdMain.Rows.Count - 1;
            grdMain.Col = 0;
        }

        private void DataRowDel()
        {

            //mj 추가되었지만 행삭제로 지울때
            if (grdMain.Rows[grdMain.Row][0].ToString() == "추가")
            {
                grdMain.RemoveItem(grdMain.Row);

                return;
            }

            grdMain.Rows[grdMain.Row][0] = "삭제";

            // 메인 의 삭제 flag를 서브 그리드에 적용
            grdSub_update();

            // Delete 배경색 지정
            //grdMain.Rows[grdMain.Row].Style = grdMain.Styles["DelColor"];
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Red, Color.Black);
            // 커서위치 결정
            //grdMain.Row = 0;
            //grdMain.Col = 0;
        }

        private void grdSub_update()
        {

            if (grdMain.Rows[grdMain.Row][0].ToString() == "삭제")
            {
                //main category에 따른 하부 sub grid 항목들도 삭제할수있게 설정한다.
                // 다른 항목을 선택했을시에도 이하의 프로세스를 거쳐야한다.
                //start
                for (int sub_row = 2; sub_row < grdSub.Rows.Count; sub_row++)
                {
                    DataRowDel2(sub_row);
                }
                //end
            }

        }

        private void DataRowAdd2()
        {

            // 수정가능 하도록 열추가
            grdSub.AllowEditing = true;
            grdSub.Rows.Add();
            grdSub.Rows[grdSub.Rows.Count - 1].Height = DataRowsHeight;

            //// 추가열 데이타 초기화
            //for (int col = 1; col < grdSub.Cols.Count-1; col++)
            //    grdSub.SetData(grdSub.Rows.Count - 1, col, "");


            // 저장시 Insert로 처리하기 위해 flag set
            grdSub.SetData(grdSub.Rows.Count - 1, 0, "추가");
            // 1행에 Count 자동 입력
            grdSub.SetData(grdSub.Rows.Count - 1, "USE_YN", "Y");

            grdSub.SetCellCheck(grdSub.Rows.Count - 1, 7, CheckEnum.Checked);
            // Insert 배경색 지정
            //grdSub.Rows[grdSub.Rows.Count - 1].Style = grdSub.Styles["InsColor"];
            clsFlexGrid.GridCellRangeStyleColor(grdSub, grdSub.Rows.Count - 1, 0, grdSub.Rows.Count - 1, grdSub.Cols.Count - 1, Color.Yellow, Color.Black);

            //grdMain.Row = grdMain.Rows.Count - 1;
            //grdMain.Col = 0;
        }

        private void DataRowDel2(int deleteRow)
        {
            if (grdSub.Rows.Count > 2)
            {
                // 저장시 Delete로 처리하기 위해 flag set
                grdSub.Rows[deleteRow][0] = "삭제";

                // Delete 배경색 지정
                //grdSub.Rows[deleteRow].Style = grdSub.Styles["DelColor"];
                clsFlexGrid.GridCellRangeStyleColor(grdSub, deleteRow, 0, deleteRow, grdSub.Cols.Count - 1, Color.Red, Color.Black);
            }
        }


        private void ComCodeMgmt_Load(object sender, System.EventArgs e)
        {
            msg = new List<string>();

            this.BackColor = Color.White;

            //tableLayoutPanel1.RowStyles[0].Height = 38F;
            //tableLayoutPanel1.RowStyles[1].Height = 50F;
            //tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;

            InitControl();


            MakeInitGridData();
            _CanSaveSearchLog = true;
           
            Button_Click(btnDisplay, null);

        }

        private void MakeInitGridData()
        {
            MakeInitgrdMainData();

            MakeInitgrdSubData();
        }

        private void MakeInitgrdSubData()
        {
            grdSubDT = vf.CreateDataTable(grdSub);
        }

        private void MakeInitgrdMainData()
        {
            grdMainDT = vf.CreateDataTable(grdMain);
        }

        private void SetDataBinding()
        {
            SetDataBinding_Grd_byinitData();

            SetDataBinding_grdMain();
        }
        //그리드 초기화
        private void SetDataBinding_Grd_byinitData()
        {
            DrawGrid(grdMain);
            DrawGrid(grdSub);
        }


        private void SetDataBinding_grdMain()
        {
            string Sql1 = string.Empty;
            try
            {

                Sql1 += string.Format("/*2017.06.09 공통코드 Main 조회          */                                ");
                Sql1 += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY CATEGORY ASC)) AS L_NUM");
                Sql1 += string.Format("      ,CATEGORY                                                            ");
                Sql1 += string.Format("      ,CATEGORY_NM                                                         ");
                Sql1 += string.Format(" FROM TB_CM_COM_CD_GRP                                                     ");
                Sql1 += string.Format("WHERE CATEGORY    LIKE '%" + txtCategory.Text + "%'                        ");
                Sql1 += string.Format("  AND CATEGORY_NM like '%" + txtCategory_nm.Text + "%'                     ");
                Sql1 += string.Format("  AND USE_YN       = 'Y'                                                   ");
                Sql1 += string.Format("ORDER BY CATEGORY ASC                                                      ");


                olddt = cd.FindDataTable(Sql1);

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                // grdMain.SetDataBinding(moddt, null, true);
                DrawGrid(grdMain, moddt);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString() + "건이 조회 되었습니다.");

                if (moddt.Rows.Count > 0)
                {
                    grdMain.Row = 2;
                    grdMain_Row_Selected(grdMain.Row);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            selectedGrd = grdMain;

            if (grdMain.Rows.Count < 2) { return; }

            grdMain_Row_Selected(grdMain.RowSel);

            if (grdMain.GetData(grdMain.RowSel, "L_NUM").ToString() == "삭제")
            {
                DataRowDel();
            }
        }

        private void grdMain_Row_Selected(int selectedRow)
        {
            if (selectedRow < GridRowsCount)
            {
                return;
            }

            selected_Category = grdMain.GetData(selectedRow, "CATEGORY").ToString();

            string Sql1 = string.Empty;

            Sql1 += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY SORT_SEQ ASC)) AS L_NUM ");
            Sql1 += string.Format("	     , N.* FROM( SELECT CD_ID     ");
            Sql1 += string.Format(",CD_NM     ");
            Sql1 += string.Format(",COLUMNA     ");
            Sql1 += string.Format(",COLUMNB     ");
            Sql1 += string.Format(",COLUMNC     ");
            //Sql1 += string.Format(",COLUMNE     ");
            Sql1 += string.Format(",SORT_SEQ     ");
            Sql1 += string.Format(",(CASE WHEN USE_YN = 'Y' THEN 'True' ELSE 'False' END) AS USE_YN       ");
            Sql1 += string.Format("FROM TB_CM_COM_CD     ");
            Sql1 += string.Format("WHERE CATEGORY = '{0}') N     ", selected_Category);
            Sql1 += string.Format("ORDER BY SORT_SEQ ");

            olddt_sub = cd.FindDataTable(Sql1);

            moddt_sub = olddt_sub.Copy();

            Cursor = Cursors.AppStarting;
            DrawGrid(grdSub, moddt_sub);
            // grdSub.SetDataBinding(moddt_sub, null, true);
            Cursor = Cursors.Default;

            //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), olddt_sub.Rows.Count.ToString() + "건이 조회 되었습니다.");
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt_sub.Rows.Count} 건 조회 되었습니다.");

        }



        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                Button_Click(btnDisplay, null);
                //SetDataBinding();  // 조회 버튼을 통한 데이터입력
            }
        }

        private void txtCategory_nm_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                Button_Click(btnDisplay, null);
                //SetDataBinding();  // 조회 버튼을 통한 데이터입력
            }
        }

        private void grdSub_CellChecked(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedGrd.GetData(editedRow, 0).ToString() != "추가")
            {

                if (editedGrd.GetCellCheck(editedRow, editedCol).ToString() == "Checked")
                {
                    editedGrd.Rows[editedRow][editedCol] = "Y";
                }
                else
                {
                    editedGrd.Rows[editedRow][editedCol] = "N";
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                //editedGrd.SetData(editedRow, editedGrd.Cols.Count - 1, "수정");
                editedGrd.SetData(editedRow, 0, "수정");

                // Update 배경색 지정
                //editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(editedGrd, editedGrd.Row, 0, editedGrd.Row, editedGrd.Cols.Count - 1, Color.Green, Color.Black);
            }
        }

        private void pbx_Click(object sender, EventArgs e)
        {
            switch (((PictureBox)sender).Name)
            {
                

                case "btnRowAdd1":
                    if (this.scrAuthReg != "Y")
                    {
                        MessageBox.Show("등록 권한이 없습니다");
                        return;
                    }

                    DataRowAdd();
                    break;

                case "btnDelRow1":
                    if (this.scrAuthDel != "Y")
                    {
                        MessageBox.Show("삭제 권한이 없습니다");
                        return;
                    }

                    DataRowDel();
                    break;

                case "btnRowAdd2":
                    if (this.scrAuthReg != "Y")
                    {
                        MessageBox.Show("등록 권한이 없습니다");
                        return;
                    }

                    DataRowAdd2();
                    break;

                case "btnDelRow2":
                    if (this.scrAuthDel != "Y")
                    {
                        MessageBox.Show("삭제 권한이 없습니다");
                        return;
                    }
                    DataRowDel2();
                    break;

            }
        }

        private void DataRowDel2()
        {
            //mj 추가되었지만 행삭제로 지울때
            if (grdSub.Rows[grdSub.Row][0].ToString() == "추가")
            {
                grdSub.RemoveItem(grdSub.Row);

                return;
            }

            grdSub.Rows[grdSub.Row][0] = "삭제";

            // 메인 의 삭제 flag를 서브 그리드에 적용
            grdSub_update();

            // Delete 배경색 지정
            //grdSub.Rows[grdSub.Row].Style = grdSub.Styles["DelColor"];
            clsFlexGrid.GridCellRangeStyleColor(grdSub, grdSub.Row, 0, grdSub.Row, grdSub.Cols.Count - 1, Color.Red, Color.Black);
        }
    }
}
