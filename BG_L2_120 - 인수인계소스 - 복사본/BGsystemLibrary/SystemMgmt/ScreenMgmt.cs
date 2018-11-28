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
using System.Collections.Specialized;

namespace BGsystemLibrary.SystemMgmt
{
    public partial class ScreenMgmt : Form
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

        //콤보박스
        Hashtable Sys_cd = null;

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

        public ScreenMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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
        /// <summary>
        /// 화면 초기화
        /// </summary>
        private void ScreenMgmt_Load(object sender, EventArgs e)
        {


            DrawGrid(grdMain);
            //초기화
            InitControl();

            grdMain.AllowEditing = true;
            grdMain.Cols[0].AllowEditing = false;
            grdMain.Cols[1].AllowEditing = false;
            grdMain.Cols[2].AllowEditing = false;
            grdMain.Cols[3].AllowEditing = false;
            grdMain.Cols[4].AllowEditing = false;

            //조회버튼 클릭
            //btnDisplay_Click(null, null);
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
                //그리드 그리기
                if (grdItem.Rows.Count == 1)
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                }
                else
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                }
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

                DataTable dt = GetBizGroup();
                ListDictionary dataMap = new ListDictionary();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataMap.Add(dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString());
                }

                //그리드 콤보박스 지정
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("BIZ_GP", FlexGridCellTypeEnum.ComboBox, dataMap);
                FlexGridColumns.Add("USE_YN", FlexGridCellTypeEnum.CheckBox, "true");
                //그리드 수정가능 컬럼 세팅
                grdItem.AllowEditing = true;
                grdItem.Cols[1].AllowEditing = true;
                grdItem.Cols[2].AllowEditing = true;
                grdItem.Cols[3].AllowEditing = true;
                grdItem.Cols[4].AllowEditing = true;
                grdItem.Cols[5].AllowEditing = true;

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

        private DataTable GetBizGroup()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "";
                sql += string.Format(@" SELECT DISTINCT BIZ_GP AS CD_ID, BIZ_GP AS CD_NM   FROM TB_CM_SCR  ORDER BY BIZ_GP");

                dt = cd.FindDataTable(sql);
            }
            catch (Exception ex)
            {

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
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 220;
            grdItem.Cols[3].Width = 350;
            grdItem.Cols[4].Width = 350;
            grdItem.Cols[5].Width = 70;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "업무구분";
            grdItem[1, 2] = "화면ID";
            grdItem[1, 3] = "화면명칭";
            grdItem[1, 4] = "PageID";
            grdItem[1, 5] = "사용여부";
            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 5);
            clsFlexGrid.DataGridLeftStyle(grdItem, 3, 4);

            grdItem.Cols[2].EditMask = @"S\L-UI-0000";
        }



        /// <summary>
        /// 조회조건 화면코드 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSCRID_TextChanged(object sender, EventArgs e)
        {
            txtSCR_ID = txtSCRID.Text;
        }

        /// <summary>
        /// 조회조건 화면명 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSCRNM_TextChanged(object sender, EventArgs e)
        {
            txtSCR_NM = txtSCRNM.Text;
        }

        /// <summary>
        /// 행추가 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            if (this.scrAuthReg != "Y")
            {
                MessageBox.Show("등록 권한이 없습니다");
                return;
            }

            // 수정가능 하도록 열추가
            grdMain.AllowEditing = true;
            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();
            grdMain.Rows[grdMain.Rows.Count - 1].Height = DataRowsHeight;
            grdMain.SetData(grdMain.Rows.Count - 1, "L_NUM", "추가");
            grdMain.SetData(grdMain.Rows.Count - 1, "USE_YN", true);
            //사용여부 체크 설정
            grdMain.SetCellCheck(grdMain.Rows.Count - 1, 5, CheckEnum.Checked);
            //행추가 색상 지정
            clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Rows.Count - 1, 0, grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, Color.Yellow, Color.Black);
            //커서 제일마지막행 세팅
            grdMain.Row = grdMain.Rows.Count - 1;
            grdMain.Col = 0;
        }

        /// <summary>
        /// 행삭제 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// grdMain BeforeEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)

        {

            C1FlexGrid grd = sender as C1FlexGrid;


            int editedRow = e.Row;
            int editedCol = e.Col;

            if (grd.Cols.Count == grd.Cols.Fixed + grd.Cols.Frozen)
            {
                return;
            }


            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 화면 ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["SCR_ID"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")
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


        /// <summary>
        /// grdMain AfterEdit 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (editedGrd.GetData(editedRow, editedCol).ToString() == strBefValue)
            {
                e.Cancel = true;
                return;
            }
            

            // 추가된 열에 대한 수정은 INSERT 처리
            if (editedGrd.GetData(editedRow, 0).ToString() != "추가")
            {
                // 저장시 UPDATE로 처리하기 위해 flag set
                // editedGrd.SetData(editedGrd.Row, editedGrd.Cols.Count - 1, "수정");
                editedGrd.SetData(editedGrd.Row, 0, "수정");

                // Update 배경색 지정
                clsFlexGrid.GridCellRangeStyleColor(grdMain, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                //grdMain.Rows[editedGrd.Row].Style = grdMain.Styles["UpColor"];
            }
            

        }

        /// <summary>
        /// 저장클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    // 명이입력되지 않은경우 체크
                    check_field_NM = "SCR_NM";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "PAGE_ID";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

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
                    check_field_NM = "SCR_ID";
                    check_table_NM = "TB_CM_SCR";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
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

                    if (vf.getStrLen(check_value) > 30)
                    {
                        MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    {
                        show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "SCR_NM";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "PAGE_ID";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
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
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    // Update 처리
                    if (grdMain.GetData(row, 0).ToString().Equals("추가"))
                    // if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "추가")
                    {
                        //Sql1 = string.Format("INSERT INTO TB_CM_SCR (BIZ_GP, SCR_ID, SCR_NM, PAGE_ID, USE_YN) VALUES('{0}','{1}','{2}','{3}','{4}')",
                        //    grdMain.GetData(row, 1), grdMain.GetData(row, 2), grdMain.GetData(row, 3), grdMain.GetData(row, 4), vf.StringToString(grdMain.GetData(row, 5).ToString()));
                        Sql1 = string.Format("                                                                            ");
                        Sql1 += string.Format(" INSERT INTO TB_CM_SCR                                                     ");
                        Sql1 += string.Format("             (  BIZ_GP                                                     ");
                        Sql1 += string.Format("               ,SCR_ID                                                     ");
                        Sql1 += string.Format("               ,SCR_NM                                                     ");
                        Sql1 += string.Format("               ,PAGE_ID                                                    ");
                        Sql1 += string.Format("               ,USE_YN                                                     ");
                        Sql1 += string.Format("               ,REGISTER                                                   ");
                        Sql1 += string.Format("               ,REG_DDTT                                                   ");
                        Sql1 += string.Format("             )                                                             ");
                        Sql1 += string.Format(" VALUES(                                                                   ");
                        Sql1 += string.Format("         '" + grdMain.GetData(row, "BIZ_GP") + "'                              "); /* BIZ_GP   */
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "SCR_ID") + "'                              "); /* SCR_ID   */
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "SCR_NM") + "'                              "); /* SCR_NM   */
                        Sql1 += string.Format("        ,'" + grdMain.GetData(row, "PAGE_ID") + "'                             "); /* PAGE_ID  */
                        Sql1 += string.Format("        ,'{0}' ", vf.StringToString(grdMain.GetData(row, "USE_YN").ToString()));
                        //Sql1 += string.Format("        ,'"+grdMain.GetData(row, "USE_YN")+"'                              "); /* USE_YN   */
                        Sql1 += string.Format("        ,'" + ck.UserID + "'                                                   "); /* REGISTER */
                        Sql1 += string.Format("        ,(SELECT CONVERT(VARCHAR, GETDATE(), 120))                         "); /* REG_DDTT */
                        Sql1 += string.Format("       )                                                                   ");


                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString().Equals("수정"))
                    {

                        Sql1 = string.Format("                                                                             ");
                        Sql1 += string.Format(" UPDATE TB_CM_SCR                                                                     ");
                        Sql1 += string.Format("    SET                                                                               ");
                        Sql1 += string.Format("        BIZ_GP   = '" + grdMain.GetData(row, "BIZ_GP") + "'                               ");
                        Sql1 += string.Format("       ,SCR_NM   = '" + grdMain.GetData(row, "SCR_NM") + "'                               ");
                        Sql1 += string.Format("       ,PAGE_ID  = '" + grdMain.GetData(row, "PAGE_ID") + "'                              ");
                        Sql1 += string.Format("       ,USE_YN = '{0}' ", vf.StringToString(grdMain.GetData(row, "USE_YN").ToString()));
                        //Sql1 += string.Format("       ,USE_YN   = '"+grdMain.GetData(row, "USE_YN")+"'                               ");
                        Sql1 += string.Format("       ,MODIFIER = '" + ck.UserID + "'                                                    ");
                        Sql1 += string.Format("       ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120))                          ");
                        Sql1 += string.Format("  WHERE SCR_ID   = '" + grdMain.GetData(row, "SCR_ID") + "'                               ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    //else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    else if (grdMain.GetData(row, 0).ToString().Equals("삭제"))
                    {
                        Sql1 = string.Format("                                                    ");
                        Sql1 += string.Format("DELETE FROM TB_CM_SCR WHERE SCR_ID = '" + grdMain.GetData(row, "SCR_ID") + "'");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        DelCnt++;
                    }
                }
                #endregion grdMain1 
                //실행후 성공
                transaction.Commit();

                DrawGrid(grdMain);
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

        /// <summary>
        /// 조회
        /// </summary>
        private void SetDataBinding()
        {
            //SQL
            string Sql = string.Empty;

            Sql += string.Format("/*2017.06.02 화면관리 조회 by 정호준*/                                         ");
            Sql += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY SCR_ID ASC)) AS L_NUM      ");
            Sql += string.Format("      ,X.*                                                                     ");
            Sql += string.Format("  FROM(SELECT BIZ_GP                                                           ");
            Sql += string.Format("             ,SCR_ID                                                           ");
            Sql += string.Format("             ,SCR_NM                                                           ");
            Sql += string.Format("             ,PAGE_ID                                                          ");
            //Sql += string.Format("             ,USE_YN                                                           ");
            Sql += string.Format("             ,(CASE WHEN USE_YN = 'Y' THEN 'True' ELSE 'False' END) AS USE_YN  ");
            Sql += string.Format("         FROM TB_CM_SCR                                                        ");
            Sql += string.Format("        WHERE SCR_ID LIKE '%" + txtSCR_ID + "%'                                 ");
            Sql += string.Format("          AND SCR_NM LIKE '%" + txtSCR_NM + "%') X                              ");

            //SQL 쿼리 조회 후 데이터셋 return
            olddt = cd.FindDataTable(Sql);
            moddt = olddt.Copy();


            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            //조회된 데이터 그리드에 세팅
            DrawGrid(grdMain, moddt);
            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt.Rows.Count} 건 조회 되었습니다.");
        }


        /// <summary>
        /// 조회조건 시스템구분 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_SysGP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string key = ((KeyValuePair<string, string>)cbx_SysGP.SelectedItem).Key;
            //string value = ((KeyValuePair<string, string>)cbx_SysGP.SelectedItem).Value;
            //cbxSys_id = key;

            ////선택항목 변경시 조회
            ////SetDataBinding();
            //btnDisplay_Click(null, null);
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


    }
}
