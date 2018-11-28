using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Text;

namespace BGsystemLibrary.StdMgmt
{
    public partial class UnitWgtMgmt : Form
    {
        //공통
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();

        //데이터테이블
        DataTable olddt;
        DataTable moddt;
        string set_value = "";

        // 셀의 수정전 값
        private string strBefValue = "";

        //타이틀 세팅
        private static string ownerNM = "";
        private static string titleNM = "";



        //그리드 세팅
        private int GridRowsCount = 2;
        private int GridColsCount = 7;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;
        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        //텍스트뷰
        private string txtSCR_ID = "";
        private string txtSCR_NM = "";

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
                                        //권한관련 add ]]
        public UnitWgtMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

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
        }

        private void UnitWgtMgmt_Load(object sender, EventArgs e)
        {


            DrawGrid(grdMain);
            //초기화
            InitControl();

            //조회버튼 클릭
            //btnDisplay_Click(null, null);
        }



        private void DrawGrid(object grdMain)
        {
        }

        /// <summary>
        /// 프로그램 초기화
        /// </summary>
        private void InitControl()
        {
            //조회조건 판넬 그리기
            clsStyle.Style.InitPanel(panel1);
            //조회조건 시스템구분 가져오기
            //setCbxSysGP();
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
                int _GridRowsCount = GridRowsCount;
                int _GridColsCount = GridColsCount;
                int _RowsFixed = RowsFixed;
                int _RowsFrozen = RowsFrozen;
                int _ColsFixed = ColsFixed;
                int _ColsFrozen = ColsFrozen;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
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

        clsFlexGridColumns FlexGridColumns;
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                //그리드 콤보박스 지정

                string selected_item = "";
                DataTable dt1 = GetItem();
                ListDictionary item_dataMap = new ListDictionary();

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (i ==0)
                    {
                        selected_item = dt1.Rows[i].ItemArray[0].ToString();
                    }
                    item_dataMap.Add(dt1.Rows[i].ItemArray[0].ToString(), dt1.Rows[i].ItemArray[0].ToString());
                }

                DataTable dt2 = GetItem_size(selected_item);
                ListDictionary item_size_dataMap = new ListDictionary();

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    item_size_dataMap.Add(dt2.Rows[i].ItemArray[0].ToString(), dt2.Rows[i].ItemArray[0].ToString());
                }

                DataTable dt3 = Getsteel_type();
                ListDictionary steel_type_dataMap = new ListDictionary();

                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    steel_type_dataMap.Add(dt3.Rows[i].ItemArray[0].ToString(), dt3.Rows[i].ItemArray[0].ToString());
                }

                FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("ITEM", FlexGridCellTypeEnum.ComboBox, item_dataMap);
                FlexGridColumns.Add("ITEM_SIZE", FlexGridCellTypeEnum.ComboBox, item_size_dataMap);
                FlexGridColumns.Add("STEEL_TYPE", FlexGridCellTypeEnum.ComboBox, steel_type_dataMap);
                //그리드 수정가능 컬럼 세팅
                //grdItem.AllowEditing = true;
                //grdItem.Cols[1].AllowEditing = true;
                //grdItem.Cols[2].AllowEditing = true;
                //grdItem.Cols[3].AllowEditing = true;
                //grdItem.Cols[4].AllowEditing = true;

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

                //상태창 검색 로우수 출력
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

        private DataTable Getsteel_type()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "";

                sql += string.Format(@" SELECT DISTINCT COLUMNA
                                          FROM TB_CM_COM_CD
                                         WHERE CATEGORY = 'STEEL'
                                           AND COLUMNA IS NOT NULL
                                           AND DATALENGTH(COLUMNA) >0
                                           AND USE_YN = 'Y'");

                dt = cd.FindDataTable(sql);
            }
            catch (Exception ex)
            {

                return null;
            }


            return dt;
        }

        private DataTable GetItem_size(string selected_item)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "";

                sql += string.Format(@" SELECT DISTINCT COLUMNB
                                          FROM TB_CM_COM_CD
                                         WHERE CATEGORY = 'ITEM_SIZE'
                                           AND COLUMNA = '{0}'
                                           AND USE_YN = 'Y' ", selected_item);

                dt = cd.FindDataTable(sql);
            }
            catch (Exception ex)
            {

                return null;
            }


            return dt;
        }



        private DataTable GetItem()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "";
                
                sql += string.Format(@" SELECT DISTINCT COLUMNA
                                          FROM TB_CM_COM_CD
                                         WHERE CATEGORY = 'ITEM_SIZE'
                                           AND COLUMNA IS NOT NULL
                                           AND USE_YN = 'Y' ");

                dt = cd.FindDataTable(sql);
            }
            catch (Exception ex)
            {

                return null;
            }

            return dt;
        }

        /// <summary>
        /// 그리드 컬럼 스타일 적용
        /// </summary>
        /// <param name="grdItem"></param>
        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60 ;
            grdItem.Cols[1].Width = 230;
            grdItem.Cols[2].Width = 230;
            grdItem.Cols[3].Width = 230;
            grdItem.Cols[4].Width = 230;
            grdItem.Cols[5].Width = 230;
            grdItem.Cols[6].Width = 230;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "품목코드";
            grdItem[1, 2] = "규격코드";
            grdItem[1, 3] = "강종분류";
            grdItem[1, 4] = "단위중량";
            grdItem[1, 5] = "품목명칭";
            grdItem[1, 6] = "규격명칭";

            grdItem.Cols[0].AllowEditing = true;
            grdItem.Cols[1].AllowEditing = true;
            grdItem.Cols[2].AllowEditing = true;
            grdItem.Cols[3].AllowEditing = true;
            grdItem.Cols[4].AllowEditing = true;
            grdItem.Cols[5].AllowEditing = true;
            grdItem.Cols[6].AllowEditing = false;
            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 6);
            //clsFlexGrid.DataGridLeftStyle(grdItem, 3, 4);

            //TextBox tbEditor = new TextBox();

            //tbEditor.MaxLength = 2;
            //tbEditor.CharacterCasing = CharacterCasing.Upper;

            //grdItem.Cols[1].Editor = tbEditor;

            //tbEditor = new TextBox();

            //tbEditor.MaxLength = 4;
            //tbEditor.KeyPress += TbEditor_KeyPress;

            //grdItem.Cols[2].Editor = tbEditor;

            //tbEditor.MaxLength = 3;
            //tbEditor.KeyPress += TbEditor_KeyPress;

            //grdItem.Cols[3].Editor = tbEditor;

            TextBox tbNumEditor = new TextBox();

            //tbNumEditor.MaxLength = 10;
            tbNumEditor.KeyPress += TbEditor_Num12_5_KeyPress;
            tbNumEditor.Leave += TbNumEditor_Leave;

            grdItem.Cols[4].Editor = tbNumEditor;

            //grdItem.Cols[4].EditMask = "000000.00000";

        }

        private void TbNumEditor_Leave(object sender, EventArgs e)
        {
            //string input = (sender as TextBox).Text;

            //bool isMach = CheckNum(input);
        }

        private bool IsSaveNum(string input)
        {
            bool isMach = true;
            Char delimiter = '.';


            // 정수인가?
            int value;
            if (int.TryParse(input, out value))
            {
                if (input.Length > 7)
                {
                    return isMach = false;
                }
                return isMach = true;
            }

            // 정수 및 소수점으로 이루어졌으면...
            string[] substrings = input.Split(delimiter);

            if (substrings[0].Length > 7)//  substrings[1].Length > 5)
            {
                //MessageBox.Show("정수부문을 최대 7자리 입력가능합니다.");
                return isMach = false;
            }

            if (substrings[1].Length > 5)
            {
                //MessageBox.Show("수수점이하는 최대 5자리 입력가능합니다.");
                return isMach = false;
            }

            return isMach;
        }

        private void TbEditor_Num12_5_KeyPress(object sender, KeyPressEventArgs e)
        {

            string input = (sender as TextBox).Text;
            string result = string.Empty;

            // 7wk
            Regex reg = new Regex(@"\d{0,7}\.\d{0,5}");

            Match match = reg.Match(input);

            string number = "";
            //We only want to allow numeric style chars
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private string GetNumber(string input)
        {
            string rtn = "";

            string[] substrings = Regex.Split(input, ".");


            return rtn = substrings[0];
        }

        private void TbEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            if (this.scrAuthReg != "Y")
            {
                MessageBox.Show("등록 권한이 없습니다");
                return;
            }

            // 수정가능 하도록 열추가
            //grdMain.AllowEditing = true;
            grdMain.Cols["ITEM"].AllowEditing = true;
            grdMain.Cols["ITEM_SIZE"].AllowEditing = true;
            grdMain.Cols["STEEL_TYPE"].AllowEditing = true;
            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();
            int addRow = grdMain.Rows.Count - 1;
            grdMain.Rows[addRow].Height = DataRowsHeight;
            grdMain.SetData(addRow, "L_NUM", "추가");
            //grdMain.SetData(addRow, "ITEM", "ZZ");
            //grdMain.SetData(addRow, "ITEM_SIZE", "9999");
            //grdMain.SetData(addRow, "STEEL_TYPE", "999");
            grdMain.SetData(addRow, "UOM_WGT", "0.00001" );
            grdMain.SetData(addRow, "ITEM_NM", "" );
            grdMain.SetData(addRow, "ITEM_SIZE_NM", "" );

            //행추가 색상 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, addRow, 0, addRow, grdMain.Cols.Count - 1, Color.Yellow, Color.Black);
            //커서 제일마지막행 세팅
            grdMain.Row = addRow;
            grdMain.Col = 0;
        }


        private void btnDelRow_Click(object sender, EventArgs e)
        {
            if (this.scrAuthDel != "Y")
            {
                MessageBox.Show("삭제 권한이 없습니다");
                return;
            }

            if (grdMain.Rows.Count < 2 || grdMain.Row < 1)
            {
                return;
            }

            //행 추가 후 삭제 클릭시 바로 삭제
            if (grdMain.Rows[grdMain.Row][0].ToString() == "추가")
            {
                grdMain.RemoveItem(grdMain.Row);

                return;
            }

            // 저장시 Delete로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Row, "L_NUM", "삭제");

            // Delete 배경색 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Red, Color.Black);

        }


        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {

            C1FlexGrid grd = sender as C1FlexGrid;


            int editedRow = e.Row;
            int editedCol = e.Col;


            //// NO COLUMN 수정불가하게..
            //if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            //{
            //    e.Cancel = true;
            //    return;
            //}

            ////추가된 행이 아니면 화면 ID 열은  수정되지않게 한다
            if (grd.GetData(editedRow, "L_NUM").ToString() != "추가")
            {
                if (editedCol == grd.Cols["ITEM"].Index
                 || editedCol == grd.Cols["ITEM_SIZE"].Index
                 || editedCol == grd.Cols["STEEL_TYPE"].Index)
                {
                    e.Cancel = true;
                    return;
                }
            }


            strBefValue = grd.GetData(editedRow, editedCol).ToString();


        }



        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (strBefValue == editedGrd.GetData(editedRow, editedCol).ToString())return;


            // 숫자형식이 안맞는 경우 원래값으로 설정되게.
            if (editedCol == editedGrd.Cols["UOM_WGT"].Index)
            {
                if (!IsSaveNum(editedGrd.GetData(editedRow, editedCol).ToString()))
                {
                    editedGrd.SetData(editedRow, editedCol, strBefValue);

                    MessageBox.Show("정수자리 최대 7자리,소수점이하 5자리 입력바랍니다.");
                    return;
                }

            }

            // ITEM 선택시 ITEM SIZE list 수정하게함.
            if (editedCol == editedGrd.Cols["ITEM"].Index)
            {
                // ITEM_SIZE COMBO LIST ITEM에 맞게 수정.
                string _editeditem = editedGrd.GetData(editedRow, editedCol).ToString();
                SetComboItemSize(_editeditem);
                // ITEM + ITEM_SIZE 에 맞게 ITEM NM 을 수정.
                string _editedItem_size = editedGrd.GetData(editedRow, "ITEM_SIZE").ToString();
            }

            // ITEM, ITEM_SIZE 둘다 유효한 선택이 이루어지면.
            // ITEM + ITEM_SIZE 에 맞게 ITEM NM 을 수정.
            if (editedCol == editedGrd.Cols["ITEM"].Index || editedCol == editedGrd.Cols["ITEM_SIZE"].Index)
            {

                string _editeditem = editedGrd.GetData(editedRow, "ITEM").ToString();
                string _editedItem_size = editedGrd.GetData(editedRow, "ITEM_SIZE").ToString();
                if (!string.IsNullOrEmpty(_editeditem) && !string.IsNullOrEmpty(_editedItem_size) )
                {
                    string itemSizeNm = GetITEM_SIZE_NM(_editeditem, _editedItem_size);

                    editedGrd.SetData(editedRow, "ITEM_SIZE_NM", itemSizeNm);
                }
            }

            if (editedGrd.GetData(editedRow, 0).ToString() != "추가")
            {
                editedGrd.SetData(editedRow, "L_NUM", "수정");
                clsFlexGrid.GridCellRangeStyleColor(editedGrd, editedRow, 0, editedRow, editedGrd.Cols.Count - 1, Color.Green, Color.Black);
            }

        }

        private string GetITEM_SIZE_NM(string _editeditem, string _editedItem_size)
        {
            string rtn = string.Empty;

            DataTable dt = new DataTable();

            try
            {
                string sql = "";



                sql += string.Format(@" SELECT TOP 1 
                                           ISNULL(CD_NM, '') AS ITEM_NM
                                      FROM TB_CM_COM_CD
                                     WHERE CATEGORY = 'ITEM_SIZE'
                                       AND CD_ID = '{0}'+'{1}' ", _editeditem, _editedItem_size);

                dt = cd.FindDataTable(sql);

                if (dt.Rows.Count >0)
                {
                    rtn = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

                return rtn = "";
            }
            return rtn;
        }





        private void SetComboItemSize(string _editeditem)
        {
            C1FlexGrid grdItem = grdMain;

            DataTable dt2 = GetItem_size(_editeditem);
            ListDictionary item_size_dataMap = new ListDictionary();

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                item_size_dataMap.Add(dt2.Rows[i].ItemArray[0].ToString(), dt2.Rows[i].ItemArray[0].ToString());
            }

            FlexGridColumns.Remove("ITEM_SIZE");
            FlexGridColumns.Add("ITEM_SIZE", FlexGridCellTypeEnum.ComboBox, item_size_dataMap);

            clsFlexGrid.FlexGridBinding(grdItem, FlexGridColumns);

        }

        private void SetDataBinding()
        {
            //SQL
            string sql = string.Empty;

            sql += string.Format(@"SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY ITEM,ITEM_SIZE, STEEL_TYPE ASC)) AS L_NUM
                                        , ITEM
                                        , ITEM_SIZE
                                        , STEEL_TYPE
                                        , UOM_WGT
                                        , ITEM_NM
                                        , ITEM_SIZE_NM
                                     FROM TB_CM_UOM_WGT ");


            //SQL 쿼리 조회 후 데이터셋 return
            olddt = cd.FindDataTable(sql);
            moddt = olddt.Copy();


            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            //조회된 데이터 그리드에 세팅
            DrawGrid(grdMain, moddt);
            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt.Rows.Count} 건 조회 되었습니다.");
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":

                    if (this.scrAuthInq != "Y")
                    {
                        MessageBox.Show("조회 권한이 없습니다");
                        return;
                    }

                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

                    SetDataBinding();

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

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        private void SaveData()
        {
            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            string Sql1 = string.Empty;
            string strMsg = string.Empty;

            #region 항목체크
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

                if (gubun_value == "삭제")
                {
                    isChange = true;
                }

                if (gubun_value == "수정")
                {
                    // 명이입력되지 않은경우 체크
                    check_field_NM = "ITEM";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = grdMain.GetData(1, check_field_NM).ToString();

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "ITEM_SIZE";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = grdMain.GetData(1, check_field_NM).ToString();

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    check_field_NM = "STEEL_TYPE";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = grdMain.GetData(1, check_field_NM).ToString();

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    isChange = true;

                }

                if (gubun_value == "추가")
                {
                    string check_field_NM1 = "ITEM";
                    string check_field_NM2 = "ITEM_SIZE";
                    string check_field_NM3 = "STEEL_TYPE";
                    check_table_NM = "TB_CM_UOM_WGT";
                    string check_value1 = grdMain.GetData(checkrow, check_field_NM1).ToString().Trim();
                    string check_value2 = grdMain.GetData(checkrow, check_field_NM2).ToString().Trim();
                    string check_value3 = grdMain.GetData(checkrow, check_field_NM3).ToString().Trim();
                    string check_Cols_NM1 = grdMain.GetData(1, check_field_NM1).ToString();
                    string check_Cols_NM2 = grdMain.GetData(1, check_field_NM2).ToString();
                    string check_Cols_NM3 = grdMain.GetData(1, check_field_NM3).ToString();

                    if (string.IsNullOrEmpty(check_value1))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    if (string.IsNullOrEmpty(check_value2))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM3);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.isContainHangul(check_value1))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        return;
                    }
                    if (vf.isContainHangul(check_value2))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        return;
                    }
                    if (vf.isContainHangul(check_value3))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM1, check_field_NM2, check_field_NM3, check_value1, check_value2, check_value3))
                    {
                        show_msg = string.Format("{0}:{3}, {1}:{4}, {2}:{5}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM1, check_Cols_NM2, check_Cols_NM3, check_value1, check_value2, check_value3);
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
            #endregion 삭제항목체크

            int row = 0;
            int InsCnt = 0;
            int UpCnt = 0;
            int DelCnt = 0;

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

                #region grdMain1
                for (row = GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    // Update 처리
                    if (grdMain.GetData(row, "L_NUM").ToString().Equals("추가"))
                    // if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "추가")
                    {
                        Sql1 += string.Format(" INSERT INTO TB_CM_UOM_WGT                                 ");
                        Sql1 += string.Format("             (  ITEM                                       ");
                        Sql1 += string.Format("               ,ITEM_SIZE                                  ");
                        Sql1 += string.Format("               ,STEEL_TYPE                                 ");
                        Sql1 += string.Format("               ,UOM_WGT                                    ");
                        Sql1 += string.Format("               ,ITEM_NM                                    ");
                        Sql1 += string.Format("               ,ITEM_SIZE_NM                               ");
                        Sql1 += string.Format("               ,REGISTER                                   ");
                        Sql1 += string.Format("               ,REG_DDTT                                   ");
                        Sql1 += string.Format("             )                                             ");
                        Sql1 += string.Format(" VALUES(                                                   ");
                        Sql1 += string.Format("         '" + grdMain.GetData(row, "ITEM"      ) + "'      "); 
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "ITEM_SIZE" ) + "'      "); 
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "STEEL_TYPE") + "'      "); 
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "UOM_WGT"   ) + "'      ");
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "ITEM_NM") + "'         ");
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "ITEM_SIZE_NM") + "'    ");
                        Sql1 += string.Format("        ,'" + ck.UserID + "'                               "); 
                        Sql1 += string.Format("        ,(SELECT CONVERT(VARCHAR, GETDATE(), 120))         ");  
                        Sql1 += string.Format("       )                                                   ");


                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, "L_NUM").ToString().Equals("수정"))
                    {

                        Sql1 += string.Format(" UPDATE TB_CM_UOM_WGT                                              ");
                        Sql1 += string.Format("    SET                                                            ");
                        Sql1 += string.Format("        UOM_WGT     = '" + grdMain.GetData(row, "UOM_WGT") + "'    ");
                        Sql1 += string.Format("      , ITEM_NM     = '" + grdMain.GetData(row, "ITEM_NM") + "'    ");
                        Sql1 += string.Format("      , MODIFIER    = '" + ck.UserID + "'                          ");
                        Sql1 += string.Format("      , MOD_DDTT    = (SELECT CONVERT(VARCHAR, GETDATE(), 120))    ");
                        Sql1 += string.Format("  WHERE ITEM        = '" + grdMain.GetData(row, "ITEM") + "'       ");
                        Sql1 += string.Format("    AND ITEM_SIZE   = '" + grdMain.GetData(row, "ITEM_SIZE") + "'  ");
                        Sql1 += string.Format("    AND STEEL_TYPE  = '" + grdMain.GetData(row, "STEEL_TYPE") + "' ");
                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    //else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    else if (grdMain.GetData(row, "L_NUM").ToString().Equals("삭제"))
                    {
                        Sql1 += string.Format("DELETE FROM TB_CM_UOM_WGT                                          ");
                        Sql1 += string.Format("  WHERE ITEM        = '" + grdMain.GetData(row, "ITEM") + "'       ");
                        Sql1 += string.Format("    AND ITEM_SIZE   = '" + grdMain.GetData(row, "ITEM_SIZE") + "'  ");
                        Sql1 += string.Format("    AND STEEL_TYPE  = '" + grdMain.GetData(row, "STEEL_TYPE") + "' ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        DelCnt++;
                    }
                }
                #endregion grdMain1 
                //실행후 성공
                transaction.Commit();

                //DrawGrid(grdMain);
                Button_Click(btnDisplay, null);
                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴
                //InitGrd_Main();

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);

                //add [[
                //MDMS_INFO.MainForm mainFrm = this.MdiParent as MDMS_INFO.MainForm;

                //mainFrm.MenuItemsRemove();
                //mainFrm.InitMenu();

                //mainFrm.Update();
                //mainFrm.Invalidate();
                //add ]]
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
    }
}
