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
    public partial class DownTmMgmt : Form
    {

        #region 공통 생성자
        public enum Search
        {
            Reault,   // 실적조회
            ProdInfo  // 조업정보조회
        }

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
        private int GridColsCount = 6;
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
                FlexGridColumns.Add("START_DDTT", FlexGridCellTypeEnum.TimePicker_All, DateTime.Now.Date);
                FlexGridColumns.Add("END_DDTT", FlexGridCellTypeEnum.TimePicker_All, DateTime.Now.Date);

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;


                


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

        
        public DownTmMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void DownTmMgmt_Load(object sender, EventArgs e)
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
            grdItem.Cols[1].Width = 300;
            grdItem.Cols[2].Width = 300;
            grdItem.Cols[3].Width = 300;
            grdItem.Cols[4].Width = 300;
            grdItem.Cols[5].Width = 300;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "시작일시";
            grdItem[1, 2] = "종료일시";
            grdItem[1, 3] = "정지시간(분)";
            grdItem[1, 4] = "정지사유";
            grdItem[1, 5] = "구분";

            grdItem.Cols[5].Visible = false;

            //grdItem.AllowEditing = true;
            grdItem.Cols[0].AllowEditing = false;
            grdItem.Cols[1].AllowEditing = true;
            grdItem.Cols[2].AllowEditing = true;
            grdItem.Cols[3].AllowEditing = false;
            grdItem.Cols[4].AllowEditing = true;

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 3);
            clsFlexGrid.DataGridLeftStyle(grdItem, 4);
            //clsFlexGrid.DataGridLeftStyle(grdItem, 3, 5);

            //TextBox tb_time = new TextBox();

            //tb_time.MaxLength = 10;
            //tb_time.KeyPress += Tb_KeyPress;

            //grdMain.Cols[3].Editor = tb_time;

            TextBox tbEditor = new TextBox();

            tbEditor.MaxLength = 100;
            tbEditor.Height = DataRowsHeight;
            //tbEditor.CharacterCasing = CharacterCasing.Upper;

            grdItem.Cols[4].Editor = tbEditor;
        }

        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            if (this.scrAuthReg != "Y")
            {
                MessageBox.Show("등록 권한이 없습니다");
                return;
            }

            // 수정가능 하도록 열추가
            //grdMain.Cols["START_DDTT"].AllowEditing = true;
            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();
            grdMain.SetData(grdMain.Rows.Count - 1, "START_DDTT", DateTime.Now);
            grdMain.SetData(grdMain.Rows.Count - 1, "END_DDTT", DateTime.Now.AddHours(1));
            grdMain.SetData(grdMain.Rows.Count - 1, "DURATION_MIN", "60");
            grdMain.Rows[grdMain.Rows.Count - 1].Height = DataRowsHeight;
            grdMain.SetData(grdMain.Rows.Count - 1, "L_NUM", "추가");
            //사용여부 체크 설정
            //grdMain.SetCellCheck(grdMain.Rows.Count - 1, 5, CheckEnum.Checked);
            //행추가 색상 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Rows.Count - 1, 0, grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, Color.Yellow, Color.Black);
            //커서 제일마지막행 세팅
            grdMain.Row = grdMain.Rows.Count - 1;
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
            if (grd.GetData(editedRow, "L_NUM").ToString() != "추가" && editedCol == grd.Cols["START_DDTT"].Index)
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
            else //변경된 경우
            {
                if (editedGrd.GetData(editedRow, "L_NUM").ToString() == "추가")
                {
                    SetDurationTime(editedGrd, editedRow,editedCol);
                    return;
                }
                else // 수정된경우
                {
                    SetDurationTime(editedGrd, editedRow,editedCol);

                    editedGrd.SetData(editedRow, "L_NUM", "수정");
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }


        }

        private void SetDurationTime(C1FlexGrid editedGrd, int editedRow, int editedCol)
        {
            DateTime starttime = (DateTime)editedGrd.GetData(editedRow, "START_DDTT");
            DateTime endtime = (DateTime)editedGrd.GetData(editedRow, "END_DDTT");

            if (editedCol == editedGrd.Cols["START_DDTT"].Index || editedCol == editedGrd.Cols["END_DDTT"].Index)
            {
                // 시간 변경으로 인한 정기시간 계산 입력.
                editedGrd.SetData(editedRow, "DURATION_MIN", GetDurationTime(starttime, endtime).ToString());
            }
        }



        private int GetDurationTime(DateTime starttime, DateTime endtime)
        {
            int timespan = 0;
            TimeSpan ts = endtime - starttime;
            return timespan = Convert.ToInt32(ts.TotalMinutes);
        }

        private void SetDataBinding()
        {

            try
            {
                DateTime startDt = start_dt.Value.AddHours(6);//2018-08-28 06:00:00
                DateTime endDt = end_dt.Value.AddDays(1).AddHours(6);         //2018-08-29 06:00:00
                string _start_dt = vf.Format(startDt, "yyyyMMddHHmmss");
                string _end_dt = vf.Format(endDt, "yyyyMMddHHmmss");

                string sql = string.Format(@"SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY START_DDTT ASC)) AS L_NUM    
                                                  , CONVERT(DATETIME, START_DDTT) AS START_DDTT                              
                                                  , CONVERT(DATETIME, END_DDTT) AS END_DDTT    
	                                              , DATEDIFF(MINUTE, START_DDTT, END_DDTT) as  DURATION_MIN                          
	                                              , STOP_RSN                                                                 
                                                  , DATA_OCC_GP                                                              
                                               FROM TB_STOP_HR     
                                              WHERE FORMAT(START_DDTT,'yyyyMMddHHmmss')  >= '{0}'
                                                AND FORMAT(START_DDTT,'yyyyMMddHHmmss')  < '{1}'
                                                   ", _start_dt, _end_dt);

                //SQL 쿼리 조회 후 데이터셋 return
                olddt = cd.FindDataTable(sql);
                moddt = olddt.Copy();


                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddt);

                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), Text+":"+ moddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception)
            {

                throw;
            }
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
            for (checkrow = GridRowsCount; checkrow < grdMain.Rows.Count; checkrow++)
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

                    check_field_NM = "START_DDTT";
                    check_table_NM = "TB_STOP_HR";
                    check_date = (DateTime)grdMain.GetData(checkrow, check_field_NM);
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_date))
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
                string start_ddtt = "";
                string end_ddtt = "";
                #region grdMain1
                for (row = GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    start_ddtt = vf.Format((DateTime)grdMain.GetData(row, "START_DDTT"), "yyyy-MM-dd HH:mm:ss");
                    end_ddtt = vf.Format((DateTime)grdMain.GetData(row, "END_DDTT"), "yyyy-MM-dd HH:mm:ss");

                    // Update 처리
                    if (grdMain.GetData(row, 0).ToString().Equals("추가"))
                    {


                        Sql1 += string.Format(" INSERT INTO TB_STOP_HR                                                    ");
                        Sql1 += string.Format("             (  START_DDTT                                                 ");
                        Sql1 += string.Format("               ,END_DDTT                                                   ");
                        Sql1 += string.Format("               ,STOP_RSN                                                   ");
                        Sql1 += string.Format("               ,DATA_OCC_GP                                                ");
                        Sql1 += string.Format("               ,REGISTER                                                   ");
                        Sql1 += string.Format("               ,REG_DDTT                                                   ");
                        Sql1 += string.Format("             )                                                             ");
                        Sql1 += string.Format(" VALUES(                                                                   ");
                        Sql1 += string.Format("         '" + start_ddtt + "'                                              ");
                        Sql1 += string.Format("        ,'" + end_ddtt + "'                                                ");
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "STOP_RSN") + "'                        ");
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "DATA_OCC_GP") + "'                     ");
                        Sql1 += string.Format("        ,'" + ck.UserID + "'                                               ");
                        Sql1 += string.Format("        ,(SELECT CONVERT(VARCHAR, GETDATE(), 120))                         ");
                        Sql1 += string.Format("       )                                                                   ");


                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString().Equals("수정"))
                    {

                        Sql1 += string.Format(" UPDATE TB_STOP_HR                                                          ");
                        Sql1 += string.Format("    SET                                                                     ");
                        Sql1 += string.Format("        END_DDTT   = '" + end_ddtt + "'                                     ");
                        Sql1 += string.Format("       ,STOP_RSN   = '" + grdMain.GetData(row, "STOP_RSN") + "'             ");
                        Sql1 += string.Format("       ,DATA_OCC_GP = '" + grdMain.GetData(row, "DATA_OCC_GP") + "'         ");
                        Sql1 += string.Format("       ,MODIFIER = '" + ck.UserID + "'                                      ");
                        Sql1 += string.Format("       ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120))                ");
                        Sql1 += string.Format("  WHERE CONVERT(VARCHAR, START_DDTT, 120)   = '" + start_ddtt + "'                                 ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    //else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    else if (grdMain.GetData(row, 0).ToString().Equals("삭제"))
                    {
                        Sql1 = string.Format("DELETE FROM TB_STOP_HR WHERE CONVERT(VARCHAR, START_DDTT, 120) = '" + start_ddtt + "'");

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
