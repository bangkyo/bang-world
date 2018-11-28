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

namespace BGsystemLibrary.MatMgmt
{
    public partial class ElecUseMgmt : Form
    {
        #region 공통 생성자
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
        private int GridColsCount = 10;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;
        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;
        private int GrandTotalHeight = 35;

        //텍스트뷰
        private string txtSCR_ID = "";
        private string txtSCR_NM = "";

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        

        //권한관련 add ]]
        #endregion

        #region 공통메서드
        private void InitControl()
        {
            //조회조건 판넬 그리기
            clsStyle.Style.InitPanel(panel1);
            //조회조건 시스템구분 가져오기
            //setCbxSysGP();

            start_dt.Value = DateTime.Now.Date;
            end_dt.Value = DateTime.Now.Date;
        }


        private void DrawGrid(C1FlexGrid grdItem)
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


        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                //그리드 콤보박스 지정
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("WORK_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now.Date);

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
        #endregion

        #region 화면별 메서드


        public ElecUseMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void ElecUseMgmt_Load(object sender, EventArgs e)
        {

            DrawGrid(grdMain);
            //초기화
            InitControl();

            //조회버튼 클릭
            Button_Click(btnDisplay, null);
        }

        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 200;
            grdItem.Cols[2].Width = 200;
            grdItem.Cols[3].Width = 200;
            grdItem.Cols[4].Width = 200;
            grdItem.Cols[5].Width = 200;
            grdItem.Cols[6].Width = 200;
            grdItem.Cols[7].Width = 200;
            grdItem.Cols[8].Width = 200;
            grdItem.Cols[9].Width = 200;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "작업일자";
            grdItem[1, 2] = "쇼트 집진기";
            grdItem[1, 3] = "그라인딩 집진기";
            grdItem[1, 4] = "그라인딩";
            grdItem[1, 5] = "공장전체";
            grdItem[1, 6] = "쇼트집진 누계 사용량";
            grdItem[1, 7] = "그라인드 집진기 누계 사용량";
            grdItem[1, 8] = "그라인드 누계 사용량";
            grdItem[1, 9] = "공장 전체 누계 사용량";

            grdItem.Cols[6].Visible = false;
            grdItem.Cols[7].Visible = false;
            grdItem.Cols[8].Visible = false;
            grdItem.Cols[9].Visible = false;



            //grdItem.AllowEditing = true;
            grdItem.Cols[0].AllowEditing = false;
            grdItem.Cols[1].AllowEditing = false;
            grdItem.Cols[2].AllowEditing = true;
            grdItem.Cols[3].AllowEditing = true;
            grdItem.Cols[4].AllowEditing = true;
            grdItem.Cols[5].AllowEditing = true;


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            clsFlexGrid.DataGridFormat(grdItem, 2, 9, "N0");

            TextBox tb_time = new TextBox();

            tb_time.MaxLength = 10;
            tb_time.KeyPress += Tb_KeyPress;

            grdMain.Cols[2].Editor = tb_time;
            grdMain.Cols[3].Editor = tb_time;
            grdMain.Cols[4].Editor = tb_time;
            grdMain.Cols[5].Editor = tb_time;


        }

        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            if (this.scrAuthReg != "Y")
            {
                MessageBox.Show("등록 권한이 없습니다");
                return;
            }

            // 수정가능 하도록 열추가
            grdMain.Cols["WORK_DATE"].AllowEditing = true;
            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();
            int addRow = grdMain.Rows.Count - 1;
            grdMain.SetData(addRow, "WORK_DATE"              , DateTime.Now);
            grdMain.SetData(addRow, "SHT_DUST_DAY_USE_QTY"   , "0");
            grdMain.SetData(addRow, "GRD_DUST_DAY_USE_QTY"   , "0");
            grdMain.SetData(addRow, "GRD_DAY_USE_QTY"        , "0");
            grdMain.SetData(addRow, "FACTORY_ALL_DAY_USE_QTY", "0");

            grdMain.Rows[addRow].Height = DataRowsHeight;
            grdMain.SetData(addRow, "L_NUM", "추가");
            //사용여부 체크 설정
            //grdMain.SetCellCheck(addRow, 5, CheckEnum.Checked);
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

            //if (grd.Cols.Count == grd.Cols.Fixed + grd.Cols.Frozen)
            //{
            //    return;
            //}

            // NO COLUMN 수정불가하게..
            if (editedCol == grd.Cols["L_NUM"].Index)
            {
                e.Cancel = true;
                return;
            }

            //추가인경우만 KEY 수정가능.
            if (grd.GetData(editedRow, "L_NUM").ToString() != "추가" && editedCol == grd.Cols["WORK_DATE"].Index)
            {
                e.Cancel = true;
                return;
            }

            // Console.WriteLine(grd.Rows[editedRow]["L_NUM"].ToString());
            if (grd.Rows[editedRow]["L_NUM"].ToString() != "추가")
            {
                // 수정여부 확인을 위해 저장
                strBefValue = grd.GetData(editedRow, editedCol).ToString();
            }


        }


        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            // No,구분은 수정 불가
            // if (editedRow == 0 || editedCol == editedGrd.Cols.Count - 1)
            if (editedRow == 0)
            {
                editedGrd.SetData(editedRow, editedCol, strBefValue);
                return;
            }

            if (strBefValue == editedGrd.GetData(editedRow, editedCol).ToString().Trim())
            {
                return;
            }
            else
            {
                if (editedGrd.GetData(editedRow, "L_NUM").ToString() == "추가")
                {
                    return;
                }
                else
                {
                    editedGrd.SetData(editedRow, "L_NUM", "수정");
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }


        }

        private void SetDataBinding()
        {

            try
            {
                string _start_dt = vf.Format(start_dt.Value, "yyyyMMdd");
                string _end_dt = vf.Format(end_dt.Value, "yyyyMMdd");

                string sql = "";
                sql += string.Format(@"SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY WORK_DATE ASC)) AS L_NUM 
                                            , CONVERT(DATE, WORK_DATE) AS WORK_DATE                                
                                            , SHT_DUST_DAY_USE_QTY                                              
                                            , GRD_DUST_DAY_USE_QTY                                              
                                            , GRD_DAY_USE_QTY                                                   
                                            , FACTORY_ALL_DAY_USE_QTY                                           
                                            , SHT_DUST_TOT_USE_QTY                                              
                                            , GRD_DUST_TOT_USE_QTY                                              
                                            , GRD_TOT_USE_QTY                                                   
                                            , FACTORY_ALL_TOT_USE_QTY                                           
                                        FROM TB_ELEC_USE_WR                                                       
                                      WHERE WORK_DATE BETWEEN '{0}' AND '{1}'", _start_dt, _end_dt);

                //SQL 쿼리 조회 후 데이터셋 return
                olddt = cd.FindDataTable(sql);
                moddt = olddt.Copy();


                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddt);

                if (moddt.Rows.Count > 1 && clsFlexGrid.IsSubTotal)
                {
                    UpdateTotals();
                }
                else
                {
                    GridRowsCount = 2;
                }
                //grdMain.AutoSizeCols();


                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception)
            {

            }
        }

        private void UpdateTotals()
        {

            // Show OutlineBar on column 0.
            grdMain.Tree.Column = 1;
            grdMain.Tree.Style = TreeStyleFlags.Simple;
            //grdMain.Rows[3].Style = grdMain.Styles[CellStyleEnum.GrandTotal];// = grdMain.Styles[CellStyleEnum.GrandTotal];

            //CellStyle cs;
            //cs = grdMain.Styles[CellStyleEnum.GrandTotal];
            //cs.BackColor = Color.Black;
            //cs.ForeColor = Color.White;
            //cs.Font = new Font("돋움체", clsFlexGrid.CommonFontSize, FontStyle.Bold);

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["SHT_DUST_DAY_USE_QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["GRD_DUST_DAY_USE_QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["GRD_DAY_USE_QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["FACTORY_ALL_DAY_USE_QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["SHT_DUST_TOT_USE_QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["GRD_DUST_TOT_USE_QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["GRD_TOT_USE_QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, -1, -1, grdMain.Cols["FACTORY_ALL_TOT_USE_QTY"].Index, "합계");


            //grdMain.Rows.Fixed = 1;
            grdMain.Rows.Frozen = 1;
            GridRowsCount = 3;

            //grdMain.Cols[grdMain.Cols["ROLLER_CONVR_SPEED"].Index].Format = "N0";
            //grdMain.Cols[grdMain.Cols["IMPELLER_NO1_SPEED"].Index].Format = "N0";

            grdMain.Rows[2].Height = GrandTotalHeight;

            //grdMain.Rows[3].Style.BackColor = Color.Blue;
            //grdMain.Rows[3].Style.ForeColor = Color.White;

            clsFlexGrid.GridCellRangeStyleColor(grdMain, 2, 0, 2, grdMain.Cols.Count - 1, Color.Blue, Color.White);



            //grdMain.Invalidate();
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

            #region 삭제항목체크
            string check_value = string.Empty;
            string check_Cols_NM = string.Empty;
            string check_field_NM = string.Empty;
            string check_table_NM = string.Empty;

            string check_keyColNM = string.Empty;
            string check_keyValue = string.Empty;

            string check_NM = string.Empty;

            string gubun_value = string.Empty;
            string show_msg = string.Empty;

            DateTime check_date = DateTime.Now;
            int checkrow = 0;

            bool isChange = false;
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "L_NUM").ToString();

                if (gubun_value == "삭제")
                {
                    isChange = true;
                }

                if (gubun_value == "수정")
                {

                    isChange = true;

                }

                if (gubun_value == "추가")
                {

                    check_field_NM = "WORK_DATE";
                    check_table_NM = "TB_ELEC_USE_WR";
                    check_value = vf.Format((DateTime)grdMain.GetData(checkrow, check_field_NM),"yyyyMMdd").ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    {
                        show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
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
                string work_date = "";
                string sht_dust_use_qty = "";
                string grd_dust_use_qty = "";
                string grd_day_use_qty = "";
                string factory_allDay_use_qty = "";
                #region grdMain1
                for (row = GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    work_date = vf.Format((DateTime)grdMain.GetData(row, "WORK_DATE"), "yyyyMMdd");
                    sht_dust_use_qty = grdMain.GetData(row, "SHT_DUST_DAY_USE_QTY").ToString();
                    grd_dust_use_qty = grdMain.GetData(row, "GRD_DUST_DAY_USE_QTY").ToString();
                    grd_day_use_qty = grdMain.GetData(row, "GRD_DAY_USE_QTY").ToString();
                    factory_allDay_use_qty = grdMain.GetData(row, "FACTORY_ALL_DAY_USE_QTY").ToString();



                    // Update 처리
                    if (grdMain.GetData(row, 0).ToString().Equals("추가"))
                    {


                        Sql1 += string.Format(" INSERT INTO TB_ELEC_USE_WR                                                ");
                        Sql1 += string.Format("             (  WORK_DATE                                                  ");
                        Sql1 += string.Format("               ,SHT_DUST_DAY_USE_QTY                                       ");
                        Sql1 += string.Format("               ,GRD_DUST_DAY_USE_QTY                                       ");
                        Sql1 += string.Format("               ,GRD_DAY_USE_QTY                                            ");
                        Sql1 += string.Format("               ,FACTORY_ALL_DAY_USE_QTY                                    ");
                        Sql1 += string.Format("               ,REGISTER                                                   ");
                        Sql1 += string.Format("               ,REG_DDTT                                                   ");
                        Sql1 += string.Format("             )                                                             ");
                        Sql1 += string.Format(" VALUES(                                                                   ");
                        Sql1 += string.Format("         '" + work_date + "'                                               ");
                        Sql1 += string.Format("        ,'" + sht_dust_use_qty + "'                                        ");
                        Sql1 += string.Format("        ,'" + grd_dust_use_qty + "'                                        ");
                        Sql1 += string.Format("        ,'" + grd_day_use_qty + "'                                         ");
                        Sql1 += string.Format("        ,'" + factory_allDay_use_qty + "'                                  ");
                        Sql1 += string.Format("        ,'" + ck.UserID + "'                                               ");
                        Sql1 += string.Format("        ,(SELECT CONVERT(VARCHAR, GETDATE(), 120))                         ");
                        Sql1 += string.Format("       )                                                                   ");


                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString().Equals("수정"))
                    {

                        Sql1 += string.Format(" UPDATE TB_ELEC_USE_WR                                                          ");
                        Sql1 += string.Format("    SET                                                                         ");
                        Sql1 += string.Format("        SHT_DUST_DAY_USE_QTY    = '" + sht_dust_use_qty + "'                    ");
                        Sql1 += string.Format("       ,GRD_DUST_DAY_USE_QTY    = '" + grd_dust_use_qty + "'                    ");
                        Sql1 += string.Format("       ,GRD_DAY_USE_QTY         = '" + grd_day_use_qty + "'                     ");
                        Sql1 += string.Format("       ,FACTORY_ALL_DAY_USE_QTY = '" + factory_allDay_use_qty + "'              ");
                        Sql1 += string.Format("       ,MODIFIER                = '" + ck.UserID + "'                           ");
                        Sql1 += string.Format("       ,MOD_DDTT                = (SELECT CONVERT(VARCHAR, GETDATE(), 120))     ");
                        Sql1 += string.Format("  WHERE WORK_DATE               = '" + work_date + "'                           ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    //else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    else if (grdMain.GetData(row, 0).ToString().Equals("삭제"))
                    {
                        Sql1 = string.Format("DELETE FROM TB_ELEC_USE_WR WHERE WORK_DATE = '" + work_date + "'");

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

        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        #endregion

        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            if (DateTime.Compare(start_dt.Value, end_dt.Value) > 0)
            {
                start_dt.Value = end_dt.Value;
            }
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            if (DateTime.Compare(start_dt.Value, end_dt.Value) > 0)
            {
                end_dt.Value = start_dt.Value;
            }
        }
    }
}
