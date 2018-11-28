using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BGsystemLibrary.EQPCheckMgmt
{
    public partial class EQPCheckItem : Form
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
        string selected_eqCD = "";

        // 셀의 수정전 값
        string strBefValue = "";
        string strBefValue2 = "";

        string ownerNM = "";
        string titleNM = "";

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        static C1FlexGrid selectedGrd;
        private int main_GridRowsCount = 2;
        private int main_GridColsCount = 5;
        private int main_RowsFixed = 2;
        private int main_RowsFrozen = 0;
        private int main_ColsFixed = 0;
        private int main_ColsFrozen = 0;
        private int main_TopRowsHeight = 2;
        private int main_DataRowsHeight = 35;

        private int sub_GridRowsCount = 2;
        private int sub_GridColsCount = 9;
        private int sub_RowsFixed = 2;
        private int sub_RowsFrozen = 0;
        private int sub_ColsFixed = 0;
        private int sub_ColsFrozen = 0;
        private int sub_TopRowsHeight = 2;
        private int sub_DataRowsHeight = 35;

        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        //권한관련 add ]]

        public EQPCheckItem(string titleNm, string scrAuth, string factCode, string ownerNm)
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

            Load += EqpChkItemMgmt_Load;


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
        TextBox tbEditor;
        private void DrawMainGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            grdItem.BeginInit();

            try
            {
                int _GridRowsCount  = main_GridRowsCount;
                int _GridColsCount  = main_GridColsCount;
                int _RowsFixed      = main_RowsFixed;
                int _RowsFrozen     = main_RowsFrozen;
                int _ColsFixed      = main_ColsFixed;
                int _ColsFrozen     = main_ColsFrozen;
                int _TopRowsHeight  = main_TopRowsHeight;
                int _DataRowsHeight = main_DataRowsHeight;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexMainGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, _TopRowsHeight, _DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                //MakeAllSelect();

                //var crCellRange = grdItem.GetCellRange(1, 1);//, 0, grdMain.Cols["MFG_DATE"].Index);
                //crCellRange.Style = grdItem.Styles["ModifyStyle"];
                
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

        //그리드 초기화
        private void DrawSubGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            grdItem.BeginInit();

            try
            {
                int _GridRowsCount  = sub_GridRowsCount;
                int _GridColsCount  = sub_GridColsCount;
                int _RowsFixed      = sub_RowsFixed;
                int _RowsFrozen     = sub_RowsFrozen;
                int _ColsFixed      = sub_ColsFixed;
                int _ColsFrozen     = sub_ColsFrozen;
                int _TopRowsHeight  = sub_TopRowsHeight;
                int _DataRowsHeight = sub_DataRowsHeight;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexSubGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, _TopRowsHeight, _DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                //MakeAllSelect();

                //var crCellRange = grdItem.GetCellRange(1, 1);//, 0, grdMain.Cols["MFG_DATE"].Index);
                //crCellRange.Style = grdItem.Styles["ModifyStyle"];
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
        private void DrawMainGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("INST_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now);
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

                grdMain.SelectionMode = SelectionModeEnum.Row;
                //clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
                //clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
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
        //조회 그리드 세팅
        private void DrawSubGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                DataTable dt = GetCheckCycle();
                ListDictionary dataMap = new ListDictionary();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataMap.Add(dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString());
                }


                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("CHECK_START_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now);
                FlexGridColumns.Add("CHECK_END_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now);
                FlexGridColumns.Add("CHECK_CYCLE", FlexGridCellTypeEnum.ComboBox, dataMap);
                FlexGridColumns.Add("USE_YN", FlexGridCellTypeEnum.CheckBox,"Y");
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

                grdMain.SelectionMode = SelectionModeEnum.Row;
                //clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
                //clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
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


        private DataTable GetCheckCycle()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "";
                sql += string.Format(@" SELECT CD_ID
                                             , CD_NM
                                          FROM TB_CM_COM_CD
                                         WHERE CATEGORY = 'CHECK_CYCLE'
                                           AND USE_YN = 'Y'");

                dt = cd.FindDataTable(sql);
            }
            catch (Exception ex)
            {

            }


            return dt;
        }

        //그리드 스타일 세팅
        private void FlexMainGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {

            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 100;
            grdItem.Cols[3].Width = 100;

            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "설비코드";
            grdItem[1, 2] = "설비명";
            grdItem[1, 3] = "공정";
            grdItem[1, 4] = "도입일자";

            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0,1);
            clsFlexGrid.DataGridLeftStyle(grdItem, 2, 2);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3, grdItem.Cols.Count - 1);


            tbEditor = new TextBox();

            tbEditor.MaxLength = 10;
            tbEditor.CharacterCasing = CharacterCasing.Upper;

            grdItem.Cols[1].Editor = tbEditor;
        }

        private void FlexSubGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {

            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 150;
            grdItem.Cols[2].Width = 150;
            grdItem.Cols[3].Width = 150;
            grdItem.Cols[4].Width = 150;
            grdItem.Cols[5].Width = 150;
            grdItem.Cols[6].Width = 100;
            grdItem.Cols[7].Width = 150;
            grdItem.Cols[8].Width = 100;

            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "설비코드";
            grdItem[1, 2] = "점검코드";
            grdItem[1, 3] = "정검항목";
            grdItem[1, 4] = "점검시작일자";
            grdItem[1, 5] = "점검종료일자";
            grdItem[1, 6] = "점검간격";
            grdItem[1, 7] = "점검주기";
            grdItem[1, 8] = "사용여부";

            grdItem.Cols[1].Visible = false;
            //grdItem.Cols[6].Visible = false;

            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 0, 6);
            //clsFlexGrid.DataGridLeftStyle(grdItem, 2, 5);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0,2);
            clsFlexGrid.DataGridLeftStyle(grdItem, 3);
            clsFlexGrid.DataGridCenterStyle(grdItem, 4, grdItem.Cols.Count - 1);

            tbEditor = new TextBox();

            tbEditor.MaxLength = 10;
            tbEditor.CharacterCasing = CharacterCasing.Upper;

            grdItem.Cols[2].Editor = tbEditor;

            TextBox tbEditorNum = new TextBox();

            tbEditorNum.MaxLength = 4;
            tbEditorNum.KeyPress += TbEditor_KeyPress;
            

            grdItem.Cols[6].Editor = tbEditorNum;

        }

        private void TbEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

            }
        }

        private void GrdSub_RowColChange(object sender, EventArgs e)
        {

        }


        private void InitControl()
        {

            clsStyle.Style.InitPanel(panel1);

            
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
            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedRow < main_GridRowsCount )
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (editedCol == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 EQP_CD ID 열은  수정되지않게 한다
            if (editedCol == grd.Cols["EQP_CD"].Index && grd.GetData(editedRow, "L_NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(editedRow, editedCol).ToString();
        }

        private void GrdMain_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {

            C1FlexGrid grd = sender as C1FlexGrid;
            int editedRow = e.Row;
            int editedCol = e.Col;

            // No,구분은 수정 불가
            if (editedCol == 0)
            {
                grdMain.SetData(editedRow, editedCol, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdMain.GetData(editedRow, editedCol).ToString())
                return;

            //   
            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdMain.GetData(editedRow, 0).ToString() != "추가")
            {
                // 화면ID 수정 불가
                if (editedCol == 1)
                {
                    grdMain.SetData(editedRow, editedCol, strBefValue);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdMain.SetData(editedRow, 0, "수정");

                // Update 배경색 지정
                //editedRows[editedRow].Style = grdMain.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(grd, editedRow, 0, editedRow, grd.Cols.Count -1, Color.Green, Color.Black);
            }


        }

        private void GrdSub_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            int editedRow = e.Row;
            int editedCol = e.Col;

            if (grd.Row < sub_GridRowsCount || grd.GetData(grd.Row, grd.Col) == null)
            {
                return;
            }

            // NO COLUMN 수정불가하게..strBefValue
            if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 CD ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["ITEM_CD"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")
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

            // No,구분은 수정 불가
            if (grdSub.Col == 0)
            {
                grdSub.SetData(grdSub.Row, grdSub.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdSub.GetData(grdSub.Row, grdSub.Col).ToString())
                return;

            //   
            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdSub.GetData(grdSub.Row, 0).ToString() != "추가")
            {
                // 코드 수정 불가
                if (grdSub.Col == 1)
                {
                    grdSub.SetData(grdSub.Row, grdSub.Col, strBefValue);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdSub.SetData(grdSub.Row, 0, "수정");

                // Update 배경색 지정
                //grdSub.Rows[grdSub.Row].Style = grdSub.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(grdSub, grdSub.Row, 0, grdSub.Row, grdSub.Cols.Count - 1, Color.Green, Color.Black);
            }

        }
        private void GrdMain_RowColChange(object sender, EventArgs e)
        {

        }
        List<C1FlexGrid> gridList;
        private void SaveExcel()
        {
            gridList = new List<C1FlexGrid>();
            gridList.Add(grdMain);
            gridList.Add(grdSub);
            grdMain.Tag = "개요";
            grdSub.Tag = "상세";
            vf.SaveExcel(titleNM, gridList);
            //vf.SaveExcel(titleNM, selectedGrd);
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

            #region grdMain Check
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "L_NUM").ToString();

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;

                    // 수정일 경우 명 확인...""
                }


                if (gubun_value == "추가")
                {
                    check_field_NM = "EQP_CD";
                    check_table_NM = "TB_EQP_INFO";
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

                    if (vf.getStrLen(check_value) > 10)
                    {
                        MessageBox.Show("영문 및 숫자 10자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    {
                        show_msg = string.Format("그룹코드가 중복되었습니다. 다시 입력해주세요.");
                        MessageBox.Show(show_msg);
                        return;
                    }

                    isChange = true;
                }

            }
            #endregion
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            #region grdSub Check
            for (checkrow = 1; checkrow < grdSub.Rows.Count; checkrow++)
            {

                

                gubun_value = grdSub.GetData(checkrow, "L_NUM").ToString();


                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }

                if (gubun_value == "추가")
                {


                    check_keyColNM = "EQP_CD";
                    check_keyValue = selected_eqCD;

                    check_field_NM = "ITEM_CD";
                    check_table_NM = "TB_EQP_CHECK_ITEM";
                    check_value = grdSub.GetData(checkrow, check_field_NM).ToString();
                    string check_Cols_NM1 = grdSub.Cols[check_keyColNM].Caption;
                    string check_Cols_NM2 = grdSub.Cols[check_field_NM].Caption;

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
                        show_msg = string.Format("{0}:{1}, {2}:{3} 코드가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM1, check_keyValue, check_Cols_NM2, check_value);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    isChange = true;
                }
            }
            #endregion


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
                for (row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    // Insert 처리
                    if (grdMain.GetData(row, 0).ToString() == "추가")
                    {
                        Sql1 = string.Format(" INSERT INTO TB_EQP_INFO ");
                        Sql1 += string.Format("             ( ");
                        Sql1 += string.Format("                EQP_CD ");
                        Sql1 += string.Format("               ,EQP_NM ");
                        Sql1 += string.Format("               ,ROUTING_NM ");
                        Sql1 += string.Format("               ,INST_DATE ");
                        Sql1 += string.Format("               ,REGISTER ");
                        Sql1 += string.Format("               ,REG_DDTT ");
                        Sql1 += string.Format("             ) ");
                        Sql1 += string.Format(" VALUES      ( ");
                        Sql1 += string.Format("               '{0}' ", grdMain.GetData(row, "EQP_CD"));
                        Sql1 += string.Format("              ,'{0}' ", grdMain.GetData(row, "EQP_NM"));
                        Sql1 += string.Format("              ,'{0}' ", grdMain.GetData(row, "ROUTING_NM"));
                        Sql1 += string.Format("              ,'{0}' ", vf.Format(grdMain.GetData(row, "INST_DATE"), "yyyyMMdd"));
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
                        Sql1 = string.Format(" UPDATE TB_EQP_INFO ");
                        Sql1 += string.Format(" SET    EQP_NM         = '{0}' ", grdMain.GetData(row, "EQP_NM"));
                        Sql1 += string.Format("       ,ROUTING_NM     = '{0}' ", grdMain.GetData(row, "ROUTING_NM"));
                        Sql1 += string.Format("       ,INST_DATE      = '{0}' ", vf.Format(grdMain.GetData(row, "INST_DATE"), "yyyyMMdd"));
                        Sql1 += string.Format("       ,MODIFIER       = '{0}' ", ck.UserID);
                        Sql1 += string.Format("       ,MOD_DDTT       = (select convert(varchar, getdate(), 120)) ");
                        Sql1 += string.Format(" WHERE EQP_CD          = '{0}' ", grdMain.GetData(row, "EQP_CD"));

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        //strMsg = " 설비코드: " + grdMain.GetData(row, "EQP_CD").ToString() + " 설비 명칭: " + olddt.Rows[row - main]["EQP_NM"].ToString() + " To " + grdMain.GetData(row, "EQP_NM").ToString() + "로 수정 ";

                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString() == "삭제")
                    {
                        // category 항목에 따른 데이터가 있는경우 같이 없애준다.

                        delSublist = new List<string>();
                        Sql1 = string.Format("SELECT EQP_CD FROM TB_EQP_CHECK_ITEM WHERE EQP_CD = '{0}'", grdMain.GetData(row, "EQP_CD"));
                        dt = cd.FindDataTable(Sql1);

                        foreach (DataRow row_data in dt.Rows) // Loop over the rows.
                        {
                            delSublist.Add(row_data.ItemArray[0].ToString());
                            Sql1 = string.Format("DELETE FROM TB_EQP_CHECK_ITEM WHERE EQP_CD = '{0}' ", row_data.ItemArray[0], grdMain.GetData(row, "EQP_CD"));
                            cmd.CommandText = Sql1;
                            cmd.ExecuteNonQuery();
                        }

                        Sql1 = string.Format("DELETE FROM TB_EQP_INFO WHERE EQP_CD = '{0}'", grdMain.GetData(row, "EQP_CD"));

                        //strMsg = " 설비코드: " + grdMain.GetData(row, "EQP_CD").ToString() + "를 삭제 ";

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        DelCnt++;

                    }

                }
                #endregion

                #region gridSub 추가 수정 삭제 처리
                for (row = sub_GridRowsCount; row < grdSub.Rows.Count; row++)
                {
                    // Insert 처리 

                    if (grdSub.GetData(row, 0).ToString() == "추가")
                    {
                        Sql1 = string.Format(" INSERT INTO TB_EQP_CHECK_ITEM  ");
                        Sql1 += string.Format("             ( ");
                        Sql1 += string.Format("               EQP_CD                ");
                        Sql1 += string.Format("              ,ITEM_CD               ");
                        Sql1 += string.Format("              ,CHECK_ITEM            ");
                        Sql1 += string.Format("              ,CHECK_START_DATE      ");
                        Sql1 += string.Format("              ,CHECK_END_DATE        ");
                        Sql1 += string.Format("              ,CHECK_GAP             ");
                        Sql1 += string.Format("              ,CHECK_CYCLE           ");
                        Sql1 += string.Format("              ,USE_YN                ");
                        Sql1 += string.Format("              ,REGISTER              ");
                        Sql1 += string.Format("              ,REG_DDTT              ");
                        Sql1 += string.Format("             )            ");
                        Sql1 += string.Format(" VALUES( ");
                        Sql1 += string.Format("         '{0}'   ", selected_eqCD);
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "ITEM_CD"));    
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "CHECK_ITEM"));     
                        Sql1 += string.Format("        ,'{0}'   ", vf.Format(grdSub.GetData(row, "CHECK_START_DATE"), "yyyyMMdd")); 
                        Sql1 += string.Format("        ,'{0}'   ", vf.Format(grdSub.GetData(row, "CHECK_END_DATE"), "yyyyMMdd"));
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "CHECK_GAP"));
                        Sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "CHECK_CYCLE")); 
                        Sql1 += string.Format("        ,'{0}'   ", vf.BoolStringToString(grdSub.GetData(row, "USE_YN").ToString()));
                        Sql1 += string.Format("        ,'{0}'   ", ck.UserID);
                        Sql1 += string.Format("        ,(select convert(varchar, getdate(), 120)) ");
                        Sql1 += string.Format("       ) ");


                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();

                        InsCnt++;
                    }
                    // Update 처리
                    else if (grdSub.GetData(row, 0).ToString() == "수정")
                    {
                        

                        Sql1 = string.Format(" UPDATE TB_EQP_CHECK_ITEM SET ");
                        Sql1 += string.Format("         CHECK_ITEM = '{0}'       ", grdSub.GetData(row, "CHECK_ITEM"));
                        Sql1 += string.Format("        ,CHECK_START_DATE = '{0}' ", vf.Format(grdSub.GetData(row, "CHECK_START_DATE"), "yyyyMMdd"));
                        Sql1 += string.Format("        ,CHECK_END_DATE = '{0}'   ", vf.Format(grdSub.GetData(row, "CHECK_END_DATE"), "yyyyMMdd"));
                        Sql1 += string.Format("        ,CHECK_GAP = '{0}'        ", grdSub.GetData(row, "CHECK_GAP"));
                        Sql1 += string.Format("        ,CHECK_CYCLE = '{0}'      ", grdSub.GetData(row, "CHECK_CYCLE"));
                        Sql1 += string.Format("        ,USE_YN = '{0}'           ", vf.BoolStringToString(grdSub.GetData(row, "USE_YN").ToString()));
                        Sql1 += string.Format("        ,MODIFIER = '{0}'         ", ck.UserID);
                        Sql1 += string.Format("        ,MOD_DDTT = (select convert(varchar, getdate(), 120))  ");
                        Sql1 += string.Format(" WHERE   EQP_CD     = '{0}' ", selected_eqCD);
                        Sql1 += string.Format("   AND   ITEM_CD    = '{0}' ", grdSub.GetData(row, "ITEM_CD"));

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();


                        UpCnt++;

                    }
                    else if (grdSub.GetData(row, 0).ToString() == "삭제")
                    {

                        Sql1 = string.Format("DELETE FROM TB_EQP_CHECK_ITEM WHERE EQP_CD = '{0}' AND ITEM_CD = '{1}'", selected_eqCD, grdSub.GetData(row, "ITEM_CD"));

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
            int addRow = grdMain.Rows.Count - 1;
            grdMain.Rows[addRow].Height = DataRowsHeight;

            // grdMain.Rows[addRow]["CD_NM"] = "";

            // 추가열 데이타 초기화
            //for (int i = 1; i < grdMain.Cols.Count; i++)
            //    grdMain.SetData(addRow, i, "");

            grdMain.SetData(addRow, "INST_DATE", DateTime.Now.Date);

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(addRow, 0, "추가");

            // Insert 배경색 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, addRow, 0, addRow, grdMain.Cols.Count - 1, Color.Yellow, Color.Black);

            grdMain.Row = addRow;
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

            int addRow = grdSub.Rows.Count - 1;

            grdSub.Rows[grdSub.Rows.Count - 1].Height = DataRowsHeight;

            //// 추가열 데이타 초기화
            //for (int col = 1; col < grdSub.Cols.Count-1; col++)
            //    grdSub.SetData(addRow, col, "");


            // 저장시 Insert로 처리하기 위해 flag set
            grdSub.SetData(addRow, 0, "추가");
            grdSub.SetData(addRow, "CHECK_START_DATE", DateTime.Now.Date);
            grdSub.SetData(addRow, "CHECK_END_DATE", DateTime.Now.Date.AddYears(1));
            // 1행에 Count 자동 입력
            grdSub.SetData(addRow, "USE_YN", "Y");
            grdSub.SetData(addRow, "CHECK_CYCLE", "W");
            grdSub.SetData(addRow, "CHECK_GAP", "1");

            //grdSub.SetCellCheck(addRow, 7, CheckEnum.Checked);
            // Insert 배경색 지정
            //grdSub.Rows[addRow].Style = grdSub.Styles["InsColor"];
            clsFlexGrid.GridCellRangeStyleColor(grdSub, addRow, 0, addRow, grdSub.Cols.Count - 1, Color.Yellow, Color.Black);

            //grdMain.Row = grdMain.Rows.Count - 1;
            //grdMain.Col = 0;
        }

        private void DataRowDel2(int deleteRow)
        {
            if (grdSub.Rows.Count > 2)
            {

                if (grdSub.Rows[grdSub.Row][0].ToString() == "추가")
                {
                    grdSub.RemoveItem(grdSub.Row);

                    return;
                }else
                {
                    // 저장시 Delete로 처리하기 위해 flag set
                    grdSub.Rows[deleteRow][0] = "삭제";

                    // Delete 배경색 지정
                    //grdSub.Rows[deleteRow].Style = grdSub.Styles["DelColor"];
                    clsFlexGrid.GridCellRangeStyleColor(grdSub, deleteRow, 0, deleteRow, grdSub.Cols.Count - 1, Color.Red, Color.Black);
                }
            }
        }


        private void EqpChkItemMgmt_Load(object sender, System.EventArgs e)
        {
            msg = new List<string>();

            this.BackColor = Color.White;

            //tableLayoutPanel1.RowStyles[0].Height = 38F;
            //tableLayoutPanel1.RowStyles[1].Height = 50F;
            //tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;

            InitControl();

            DrawMainGrid(grdMain);
            DrawSubGrid(grdSub);

            //MakeInitGridData();
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
            clsFlexGrid.grdDataClearForBind(grdMain);
            clsFlexGrid.grdDataClearForBind(grdSub);
        }


        private void SetDataBinding_grdMain()
        {
            string Sql1 = string.Empty;
            try
            {

                Sql1 += string.Format(@"SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY EQP_CD ASC)) AS L_NUM
                                             , EQP_CD
                                             , EQP_NM
                                             , ROUTING_NM
                                             , CONVERT(DATE, INST_DATE) AS INST_DATE 
                                          FROM TB_EQP_INFO
                                         WHERE EQP_NM LIKE '{0}%'", txtEqp.Text);


                olddt = cd.FindDataTable(Sql1);

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                // grdMain.SetDataBinding(moddt, null, true);
                DrawMainGrid(grdMain, moddt);
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

            //if (grdMain.GetData(grdMain.RowSel, "L_NUM").ToString() == "삭제")
            //{
            //    DataRowDel();
            //}
        }

        private void grdMain_Row_Selected(int selectedRow)
        {
            if (selectedRow < main_GridRowsCount)
            {
                return;
            }

            selected_eqCD = grdMain.GetData(selectedRow, "EQP_CD").ToString();

            try
            {

                string sql = "";
                sql += string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY EQP_CD, ITEM_CD )) AS L_NUM
                                             , EQP_CD
                                             , ITEM_CD
                                             , CHECK_ITEM
                                             , CONVERT(DATE, ISNULL(CHECK_START_DATE,(SELECT CONVERT(VARCHAR(8), DATEADD(YEAR, +10, GETDATE()), 112)))) AS CHECK_START_DATE 
                                             , CONVERT(DATE, ISNULL(CHECK_END_DATE,(SELECT CONVERT(VARCHAR(8), DATEADD(YEAR, +10, GETDATE()), 112)))) AS CHECK_END_DATE  
                                             , CHECK_GAP
                                             , CHECK_CYCLE
                                             , (CASE WHEN USE_YN = 'Y' THEN 'True' ELSE 'False' END) AS USE_YN
                                         FROM TB_EQP_CHECK_ITEM   
                                        WHERE EQP_CD = '{0}'
                                     ORDER BY EQP_CD, ITEM_CD", selected_eqCD);


                //SQL 쿼리 조회 후 데이터셋 return
                olddt_sub = cd.FindDataTable(sql);

                moddt_sub = olddt_sub.Copy();

                Cursor = Cursors.AppStarting;
                DrawSubGrid(grdSub, moddt_sub);
                Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt_sub.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }

        }




        private void grdSub_CellChecked(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedGrd.GetCellCheck(editedRow, editedCol).ToString() == "Checked")
            {
                editedGrd.Rows[editedRow][editedCol] = "True";
            }
            else
            {
                editedGrd.Rows[editedRow][editedCol] = "False";
            }
            //// 저장시 추가, 수정 flag 구분해서 INSERT, UPDATE로 처리하기 위해 flag set
            ////editedGrd.SetData(editedRow, editedGrd.Cols.Count - 1, "수정");
            //editedGrd.SetData(editedRow, 0, "수정");

            //// Update 배경색 지정
            ////editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            //clsFlexGrid.GridCellRangeStyleColor(editedGrd, editedGrd.Row, 0, editedGrd.Row, editedGrd.Cols.Count - 1, Color.Green, Color.Black);
        }

        private void btnRowAdd1_Click(object sender, EventArgs e)
        {
            if (this.scrAuthReg != "Y")
            {
                MessageBox.Show("등록 권한이 없습니다");
                return;
            }

            DataRowAdd();
        }

        private void btnDelRow1_Click(object sender, EventArgs e)
        {
            if (this.scrAuthDel != "Y")
            {
                MessageBox.Show("삭제 권한이 없습니다");
                return;
            }

            DataRowDel();
        }

        private void btnRowAdd2_Click(object sender, EventArgs e)
        {
            if (this.scrAuthReg != "Y")
            {
                MessageBox.Show("등록 권한이 없습니다");
                return;
            }

            DataRowAdd2();
        }

        private void btnDelRow2_Click(object sender, EventArgs e)
        {
            if (this.scrAuthDel != "Y")
            {
                MessageBox.Show("삭제 권한이 없습니다");
                return;
            }

            DataRowDel2(grdSub.Row);
        }
    }
}
