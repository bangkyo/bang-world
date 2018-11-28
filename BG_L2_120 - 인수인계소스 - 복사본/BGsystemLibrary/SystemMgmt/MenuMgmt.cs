using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using C1.Win.C1FlexGrid;
using System.Text.RegularExpressions;

namespace BGsystemLibrary.SystemMgmt
{
    public partial class MenuMgmt : Form
    {
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        //데이터테이블
        DataTable olddt;
        DataTable moddt;

        // 셀의 수정전 값
        private string strBefValue = "";
        // 프로그램 명 변수
        private static string ownerNM = "";
        private static string titleNM = "";



        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private int GridRowsCount = 2;
        private int GridColsCount = 6;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;

        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        //변수
        private string txtmenuID = "";  //조회조건메뉴ID
        private string txtmenuNM = "";  //조회조건메뉴명
        string set_value = "";          //그리드 화면코드 세팅변수

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        //권한관련 add ]]


        public MenuMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            string[] scrAuthParams = scrAuth.Split(',');
            this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장


            InitializeComponent();

        }

        /// <summary>
        /// 화면 초기화
        /// </summary>
        private void MenuMgmt_Load(object sender, EventArgs e)
        {
            //그리드 초기화
            DrawGrid(grdMain);
            //초기화
            InitControl();
            //조회버튼 클릭
            Button_Click(btnDisplay, null);
        }
        /// <summary>
        /// 프로그램 초기화
        /// </summary>
        private void InitControl()
        {
            //조회조건 판넬 색상 세팅
            clsStyle.Style.InitPanel(panel1);
          
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
                //그리드 수정가능 컬럼 세팅
                grdItem.AllowEditing = true;
                grdItem.Cols[1].AllowEditing = true;
                grdItem.Cols[2].AllowEditing = true;
                grdItem.Cols[3].AllowEditing = true;
                grdItem.Cols[4].AllowEditing = true;
                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;
                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, true);
                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);
                //마지막행 사이즈조절, 로우공백흰색
                grdMain.ExtendLastCol = true;
                grdMain.Styles.EmptyArea.BackColor = Color.White;

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

        /// <summary>
        /// 그리드 컬럼 스타일 적용
        /// </summary>
        /// <param name="grdItem"></param>
        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 400;
            grdItem.Cols[3].Width = 200;
            grdItem.Cols[4].Width = 400;
            grdItem.Cols[5].Width = 70;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "메뉴ID";
            grdItem[1, 2] = "메뉴명칭";
            grdItem[1, 3] = "상위메뉴ID";
            grdItem[1, 4] = "화면ID";
            grdItem[1, 5] = "메뉴순서";

            grdItem.Cols[0].Caption = "NO";
            grdItem.Cols[1].Caption ="메뉴ID";
            grdItem.Cols[2].Caption ="메뉴명칭";
            grdItem.Cols[3].Caption ="상위메뉴ID";
            grdItem.Cols[4].Caption ="화면ID";
            grdItem.Cols[5].Caption ="메뉴순서";

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            clsFlexGrid.DataGridLeftStyle(grdItem, 2);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3, grdItem.Cols.Count - 1);
            
            //clsFlexGrid.DataGridLeftStyle(grdItem, 4);


            grdItem.Cols[1].EditMask = @"S\L-MN-000";
            grdItem.Cols[3].EditMask = @"S\L-MN-000";
            grdItem.Cols[4].EditMask = @"S\L-UI-0000"; 


        }


        /// <summary>
        /// 메뉴명 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMenuNM_TextChanged(object sender, EventArgs e)
        {
            //txt_MenuNM.Text = vf.UCase(txt_MenuNM.Text);
            txtmenuNM = txt_MenuNM.Text;
        }

        /// <summary>
        /// 메뉴ID 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMenuID_TextChanged(object sender, EventArgs e)
        {
            //txt_MenuID.Text = vf.UCase(txt_MenuID.Text);
            txtmenuID = txt_MenuID.Text;
        }

        /// <summary>
        /// 행추가 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowadd_btn_Click(object sender, EventArgs e)
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
            grdMain.SetData(grdMain.Rows.Count - 1, 0, (grdMain.Rows.Count - 1).ToString());

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");

            // Insert 배경색 지정
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
        private void rowdel_btn_Click(object sender, EventArgs e)
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
            grdMain.Rows[grdMain.Row][0] = "삭제";

            // Delete 배경색 지정
            //grdMain.Rows[grdMain.Row].Style = grdMain.Styles["DelColor"];
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

            if (grd.Row < GridRowsCount || grd.GetData(grd.Row, grd.Col) == null)
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 메뉴 ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["MENU_ID"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(grd.Row, grd.Col).ToString();
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
            if (editedCol == 0)
            {
                editedGrd.SetData(editedRow, editedCol, strBefValue);
                return;
            }

            if (editedGrd.GetData(editedRow, editedCol).ToString() == strBefValue)
            {
                e.Cancel = true;
                return;
            }


            if (editedGrd.GetData(editedRow, 0).ToString() != "추가")
            {
                // 저장시 UPDATE로 처리하기 위해 flag set
                editedGrd.SetData(editedRow, 0, "수정");

                // Update 배경색 지정
                //editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
                clsFlexGrid.GridCellRangeStyleColor(editedGrd, grdMain.Row, 0, grdMain.Row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
            }

        }



        /// <summary>
        /// 저장클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql1 = string.Empty;
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

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }

                if (gubun_value == "추가")
                {
                    check_field_NM = "MENU_ID";
                    check_table_NM = "TB_CM_MENU";
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
                    check_field_NM = "MENU_NM";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    {
                        show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "UP_MENU_ID";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (!vf.Has_Item(check_table_NM, "MENU_ID", check_value))
                    {
                        show_msg = string.Format("{0}가 존재하지 않습니다.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "SCR_ID";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);
                    string check_table_NM_scr = "TB_CM_SCR";

                    //check_field_NM = "PAGE_ID";

                    if (!string.IsNullOrEmpty(check_value))
                    {
                        if (!vf.Has_Item(check_table_NM_scr, check_field_NM, check_value))
                        {
                            show_msg = string.Format("{0}의 화면이 존재하지 않습니다. 화면을 등록해주세요.", check_Cols_NM);
                            MessageBox.Show(show_msg);
                            return;
                        }
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "MENU_SEQ";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

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

                #region grdMain1
                for (row = 1; row < grdMain.Rows.Count; row++)
                {

                    if (grdMain.GetData(row, "L_NUM").ToString() == "추가")
                    {
                        sql1 = string.Format(" INSERT INTO TB_CM_MENU ");
                        sql1 += string.Format("           ( ");
                        sql1 += string.Format("             MENU_ID ");
                        sql1 += string.Format("            ,MENU_NM ");
                        sql1 += string.Format("            ,UP_MENU_ID ");
                        sql1 += string.Format("            ,SCR_ID ");
                        sql1 += string.Format("            ,MENU_SEQ ");
                        sql1 += string.Format("            ,REGISTER ");
                        sql1 += string.Format("            ,REG_DDTT ");
                        sql1 += string.Format("            ) ");
                        sql1 += string.Format(" VALUES ( ");
                        sql1 += string.Format("         '{0}' ", grdMain.GetData(row, "MENU_ID"));
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "MENU_NM"));
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "UP_MENU_ID"));
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "SCR_ID"));
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "MENU_SEQ"));
                        sql1 += string.Format("        ,'{0}' ", ck.UserID);
                        sql1 += string.Format("        ,(SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        sql1 += string.Format("        ) ");

                        //sql1 = string.Format("INSERT INTO TB_CM_MENU (MENU_ID, MENU_NM, UP_MENU_ID, SCR_ID, MENU_SEQ,REGISTER) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                        //grdMain.GetData(row, 1), grdMain.GetData(row, 2), grdMain.GetData(row, 3), grdMain.GetData(row, 4), grdMain.GetData(row, 5), grdMain.GetData(row, 6));
                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, "L_NUM").ToString() == "수정")
                    {
                        sql1 = string.Format(" UPDATE TB_CM_MENU ");
                        sql1 += string.Format(" SET   ");
                        sql1 += string.Format("      MENU_NM    = '{0}' ", grdMain.GetData(row, "MENU_NM"));
                        sql1 += string.Format("     ,UP_MENU_ID = '{0}' ", grdMain.GetData(row, "UP_MENU_ID"));
                        sql1 += string.Format("     ,SCR_ID     = '{0}' ", grdMain.GetData(row, "SCR_ID"));
                        sql1 += string.Format("     ,MENU_SEQ   = '{0}' ", grdMain.GetData(row, "MENU_SEQ"));
                        sql1 += string.Format("     ,MODIFIER   = '{0}' ", ck.UserID);
                        sql1 += string.Format("     ,MOD_DDTT   = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        sql1 += string.Format(" WHERE MENU_ID   = '{0}' ", grdMain.GetData(row, 1));

                        //sql1 = string.Format("UPDATE TB_CM_MENU SET MENU_ID = '{0}', MENU_NM = '{1}', UP_MENU_ID = '{2}', SCR_ID = '{3}', MENU_SEQ = '{4}', REGISTER = '{5}' WHERE MENU_ID = '{6}'",
                        //grdMain.GetData(row, 1), grdMain.GetData(row, 2), grdMain.GetData(row, 3), grdMain.GetData(row, 4), grdMain.GetData(row, 5), grdMain.GetData(row, 6), grdMain.GetData(row, 1));
                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, "L_NUM").ToString() == "삭제")
                    {
                        sql1 = string.Format("DELETE FROM TB_CM_MENU WHERE MENU_ID = '{0}'", grdMain.GetData(row, 1));
                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        DelCnt++;
                    }
                }
                #endregion grdMain1 
                //실행후 성공
                transaction.Commit();

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

        /// <summary>
        /// 조회
        /// </summary>
        private void SetDataBinding()
        {
            try
            {
                //SQL
                string Sql = string.Empty;

                Sql += string.Format("/* 2017.06.02 메뉴관리 조회 */                                      ");
                Sql += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY MENU_SEQ ASC)) AS L_NUM  ");
                Sql += string.Format("      ,X.*                                                                   ");
                Sql += string.Format("  FROM                                                                       ");
                Sql += string.Format("     (                                                                       ");
                Sql += string.Format("      SELECT MENU_ID                                                         ");
                Sql += string.Format("            ,MENU_NM                                                         ");
                Sql += string.Format("            ,UP_MENU_ID                                                      ");
                Sql += string.Format("            ,SCR_ID                                                          ");
                Sql += string.Format("            ,MENU_SEQ                                                        ");
                Sql += string.Format("        FROM TB_CM_MENU                                                      ");
                Sql += string.Format("       WHERE MENU_ID LIKE '%" + txtmenuID + "%'                              ");
                Sql += string.Format("         AND MENU_NM LIKE '%" + txtmenuNM + "%'                              ");
                Sql += string.Format("     ) X                                                                     ");
                //SQL 쿼리 조회 후 데이터셋 return
                olddt = cd.FindDataTable(Sql);
                moddt = olddt.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddt);
                this.Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddt.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

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
            int checkrow = 0;

            bool isChange = false;
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "L_NUM").ToString();

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }

                if (gubun_value == "추가")
                {
                    check_field_NM = "MENU_ID";
                    check_table_NM = "TB_CM_MENU";
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
                    check_field_NM = "MENU_NM";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    //if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    //{
                    //    show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                    //    MessageBox.Show(show_msg);
                    //    return;
                    //}
                    // 정호준 주석
                    // 명이입력되지 않은경우 체크
                    //check_field_NM = "UP_MENU_ID";
                    //check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    //check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    //if (!vf.Has_Item(check_table_NM, "MENU_ID", check_value))
                    //{
                    //    show_msg = string.Format("{0}가 존재하지 않습니다.", check_Cols_NM);
                    //    MessageBox.Show(show_msg);
                    //    return;
                    //}

                    // 명이입력되지 않은경우 체크
                    //check_field_NM = "SCR_ID";
                    //check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    //check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);
                    //string check_table_NM_scr = "TB_CM_SCR";

                    //check_field_NM = "PAGE_ID";

                    //if (!string.IsNullOrEmpty(check_value))
                    //{
                    //    if (!vf.Has_Item(check_table_NM_scr, check_field_NM, check_value))
                    //    {
                    //        show_msg = string.Format("{0}의 화면이 존재하지 않습니다. 화면을 등록해주세요.", check_Cols_NM);
                    //        MessageBox.Show(show_msg);
                    //        return;
                    //    }
                    //}

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "MENU_SEQ";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = (string)grdMain.GetData(1, check_field_NM);

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

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

                #region grdMain1
                for (row = 1; row < grdMain.Rows.Count; row++)
                {

                    if (grdMain.GetData(row, "L_NUM").ToString().Equals("추가"))
                    {

                        Sql1 = string.Format(" /* 2017.06.02 메뉴관리 저장  */           ");
                        Sql1 += string.Format(" INSERT INTO TB_CM_MENU                             ");
                        Sql1 += string.Format("        ( MENU_ID                                   ");
                        Sql1 += string.Format("         ,MENU_NM                                   ");
                        Sql1 += string.Format("         ,UP_MENU_ID                                ");
                        Sql1 += string.Format("         ,SCR_ID                                   ");
                        Sql1 += string.Format("         ,MENU_SEQ                                  ");
                        Sql1 += string.Format("         ,REGISTER                                  ");
                        Sql1 += string.Format("         ,REG_DDTT                                  ");
                        Sql1 += string.Format("        )                                           ");
                        Sql1 += string.Format(" VALUES                                             ");
                        Sql1 += string.Format("        (                                           ");
                        Sql1 += string.Format("          '" + grdMain.GetData(row, "MENU_ID") + "'     ");
                        Sql1 += string.Format("         ,'" + grdMain.GetData(row, "MENU_NM") + "'     ");
                        Sql1 += string.Format("         ,'" + grdMain.GetData(row, "UP_MENU_ID") + "'  ");
                        Sql1 += string.Format("         ,'" + grdMain.GetData(row, "SCR_ID") + "'    ");
                        Sql1 += string.Format("         ,'" + grdMain.GetData(row, "MENU_SEQ") + "'    ");
                        Sql1 += string.Format("         ,'" + ck.UserID + "'                           ");
                        Sql1 += string.Format("         ,(SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        Sql1 += string.Format("        )                                           ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, "L_NUM").ToString().Equals("수정"))
                    {
                        Sql1 = string.Format(" /* 2017.06.02 메뉴화면 수정 */                      ");
                        Sql1 += string.Format(" UPDATE TB_CM_MENU                                            ");
                        Sql1 += string.Format("    SET                                                       ");
                        Sql1 += string.Format("       MENU_NM    = '" + grdMain.GetData(row, "MENU_NM") + "'     ");
                        Sql1 += string.Format("      ,UP_MENU_ID = '" + grdMain.GetData(row, "UP_MENU_ID") + "'  ");
                        Sql1 += string.Format("      ,SCR_ID     = '" + grdMain.GetData(row, "SCR_ID") + "'    ");
                        Sql1 += string.Format("      ,MENU_SEQ   = '" + grdMain.GetData(row, "MENU_SEQ") + "'    ");
                        Sql1 += string.Format("      ,MODIFIER   = '" + ck.UserID + "'                          ");
                        Sql1 += string.Format("      ,MOD_DDTT   = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        Sql1 += string.Format(" WHERE MENU_ID   = '" + grdMain.GetData(row, 1) + "'              ");

                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, "L_NUM").ToString().Equals("삭제"))
                    {

                        Sql1 = string.Format("/* 2017.06.02 메뉴관리 삭제  */        ");
                        Sql1 += string.Format("DELETE FROM TB_CM_MENU WHERE MENU_ID = '{0}'", grdMain.GetData(row, 1));
                        cmd.CommandText = Sql1;
                        cmd.ExecuteNonQuery();
                        DelCnt++;
                    }
                }
                #endregion grdMain1 
                //실행후 성공
                transaction.Commit();

                //재조회
                Button_Click(btnDisplay, null);

                //string message = "정상적으로 저장되었습니다.";

                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);

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
