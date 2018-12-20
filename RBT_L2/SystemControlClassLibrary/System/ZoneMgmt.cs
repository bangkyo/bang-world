﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using static ComLib.clsUtil;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using SystemControlClassLibrary.Popup;
using System.Data.OracleClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace SystemControlClassLibrary.information
{
    public partial class ZoneMgmt : Form
    {
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;

        ListDictionary ngTest_Value = null;
        List<string> msg;
        List<string> modifList;
        List<string> checkList;
        List<string> good_ng_List;

        string set_value = "";

        clsStyle cs = new clsStyle();

        // 셀의 수정전 값
        private  string strBefValue = "";

        ListDictionary Work_cd = null;

        private bool _CanSaveSearchLog = false;
        private static string ownerNM = "";
        private static string titleNM = "";

        private static string cboBiz_id = "";
        private static string cboBiz_nm = "";
        private string txtSCR_ID = "";
        private string txtSCR_NM = "";



        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public ZoneMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();


        }

        private void ZoneMgmt_Load(object sender, EventArgs e)
        {

            InitControl();

            _CanSaveSearchLog = true;
            btnDisplay_Click(null, null);

        }

        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);
            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);
            clsStyle.Style.InitPanel(panel1);

            //clsStyle.Style.InitLabel(label1);
            clsStyle.Style.InitLabel(label2);
            clsStyle.Style.InitLabel(label3);

            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnSave);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnClose);

            //clsStyle.Style.InitCombo(cboBIZ_GP, StringAlignment.Near);

            clsStyle.Style.InitTextBox(txtSCRID);
            clsStyle.Style.InitTextBox(txtSCRNM);

            //SetComboBox1();

            InitGrd_Main();
        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["ZONE_CD"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["ZONE_CD"].Index, 0, grdMain.Cols["ORD_YN"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            int level1 = 50; // 2자리
            int level2 = 60; // 4자리
            int level3 = 100; // 6자리
            int level4 = 140; // 8자리이상


            grdMain.Cols["L_NUM"].Width = cs.L_No_Width;
            grdMain.Cols["ZONE_CD"].Width = 300;
            grdMain.Cols["MOVE_ZONE_CD"].Width = 300;
            grdMain.Cols["ORD_YN"].Width = 150;
            grdMain.Cols["PROC_REMARKS"].Width = 400;
            //grdMain.Cols["USE_YN"].Width = 160;
            grdMain.Cols["GUBUN"].Width = 0;
           
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NUM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ZONE_CD"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["MOVE_ZONE_CD"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ORD_YN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
            grdMain.Cols["PROC_REMARKS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            //grdMain.Cols["USE_YN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            
            //grdMain.Cols["BIZ_GP"].DataMap = Work_cd;
            //grdMain.Cols["BIZ_GP"].TextAlign = TextAlignEnum.CenterCenter;

            grdMain.AllowEditing = true;
        }
        
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (_CanSaveSearchLog)
            {
                cd.InsertLogForSearch(ck.UserID, btnDisplay);
            }
            

            SetDataBinding();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, grdMain);
        }
        
      
       

        private void txtSCRID_TextChanged(object sender, EventArgs e)
        {
            txtSCRID.Text = vf.UCase(txtSCRID.Text);
            txtSCR_ID = txtSCRID.Text;
        }

        private void txtSCRNM_TextChanged(object sender, EventArgs e)
        {
            txtSCRNM.Text = vf.UCase(txtSCRNM.Text);
            txtSCR_NM = txtSCRNM.Text;
        }
        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            // 수정가능 하도록 열추가
            grdMain.AllowEditing = true;

            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, "추가");
            grdMain.SetData(grdMain.Rows.Count - 1, "L_NUM", "추가");
            //grdMain.SetData(grdMain.Rows.Count - 1, "ORD_YN", false);

            //IEnumerator enumerator = Work_cd.Keys.GetEnumerator();
            //enumerator.MoveNext();
            //object first = enumerator.Current;

            //grdMain.SetData(grdMain.Rows.Count - 1, "ZONE_CD", first);

            // Insert 배경색 지정
            grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];

        }
        private void btnDelRow_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count < 2)
            {
                return;
            }

            //mj 추가되었지만 행삭제로 지울때
            if (grdMain.Rows[grdMain.Row][grdMain.Cols.Count - 1].ToString() == "추가")
            {
                grdMain.RemoveItem(grdMain.Row);

                return;
            }

            // 저장시 Delete로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Row, "GUBUN", "삭제");
            grdMain.SetData(grdMain.Row, "L_NUM", "삭제");

            // Delete 배경색 지정
            grdMain.Rows[grdMain.Row].Style = grdMain.Styles["DelColor"];

            // 커서위치 결정
            grdMain.Row = 0;
            grdMain.Col = 0;
        }
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            
            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 화면 ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["PROC_REMARKS"].Index && grd.GetData(e.Row, "L_NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(editedRow, editedCol).ToString();
           
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            // No,구분은 수정 불가
            if (editedRow == 0 || editedCol == editedGrd.Cols.Count - 1)
            {
                editedGrd.SetData(editedRow, editedCol, strBefValue);
                return;
            }
            
            //// 상위메뉴 ID 인경우 입력수치를 SL-UI-9999 형식으로 변환해서 기존값과 비교한다.
            //if (editedCol == editedGrd.Cols["SCR_ID"].Index)
            //{
            //    //기존값이 널ㅇ거나 빈 값이고 입력된값도역시 널이거나 빈값이면.
            //    if (string.IsNullOrEmpty(strBefValue) && string.IsNullOrEmpty(editedGrd.GetData(editedRow, editedCol).ToString().Trim()))
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        // 입력된값이 정상적인 값이라면. 기존값과 비교해서 같으면 그냥 빠져나감.
            //        if (!string.IsNullOrEmpty(editedGrd.GetData(editedRow, editedCol).ToString().Trim()))
            //        {
            //            string str = "SL-UI-" + editedGrd.GetData(editedRow, editedCol).ToString().Trim();
            //            // 수정된 내용이 없으면 Update 처리하지 않는다.
            //            if (strBefValue == str)
            //            {
            //                editedGrd.SetData(editedRow, editedCol, str);
            //                return;
            //            }
            //        }
                    
            //    }
            //}
            //else
            //{
            //    // 수정된 내용이 없으면 Update 처리하지 않는다.
            //    if (strBefValue == editedGrd.GetData(editedRow, editedCol).ToString())
            //        return;
            //}

            // 추가된 열에 대한 수정은 INSERT 처리
            if (editedGrd.GetData(editedRow, editedGrd.Cols.Count - 1).ToString() != "추가")
            {
                // 저장시 UPDATE로 처리하기 위해 flag set
                editedGrd.SetData(editedGrd.Row, editedGrd.Cols.Count - 1, "수정");
                editedGrd.SetData(editedGrd.Row, 0, "수정");

                // Update 배경색 지정
                editedGrd.Rows[editedGrd.Row].Style = editedGrd.Styles["UpColor"];
            }
            else
            {

                //if (editedCol == editedGrd.Cols["SCR_ID"].Index)
                //{
                //    if (!string.IsNullOrEmpty(editedGrd.GetData(editedRow, "SCR_ID").ToString().Trim()))
                //    {
                //        //정상 입력시에 는 SL-MN-을 붙여서 입력됨
                //        set_value = "SL-UI-" + editedGrd.GetData(editedRow, "SCR_ID").ToString();
                //        editedGrd.SetData(e.Row, e.Col, set_value);
                //    }
                //    else
                //    {
                //        //공백이거나 NULL 인경우 그대로 입력되게..
                //        editedGrd.SetData(e.Row, e.Col, editedGrd.GetData(editedRow, "SCR_ID").ToString());
                //    }
                //}
            }

        }

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
                gubun_value = grdMain.GetData(checkrow, "GUBUN").ToString();

                if (gubun_value == "삭제" )
                {
                    isChange = true;
                }

                if (gubun_value == "수정")
                {
                    // 명이입력되지 않은경우 체크
                    check_field_NM = "ZONE_CD";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "MOVE_ZONE_CD";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

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
                    check_field_NM = "ZONE_CD";
                    //check_table_NM = "MOVE_ZONE_CD";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    //if (vf.isContainHangul(check_value))
                    //{
                    //    MessageBox.Show("한글이 포함되어서는 안됩니다.");
                    //    return;
                    //}

                    //if (vf.getStrLen(check_value) > 30)
                    //{
                    //    MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                    //    return;
                    //}

                    //if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    //{
                    //    show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                    //    MessageBox.Show(show_msg);
                    //    return;
                    //}

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "MOVE_ZONE_CD";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString().Trim();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_value))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    //check_field_NM = "PAGE_ID";
                    //check_value = grdMain.GetData(checkrow, check_field_NM).ToString();
                    //check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    //if (string.IsNullOrEmpty(check_value))
                    //{
                    //    show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                    //    MessageBox.Show(show_msg);
                    //    return;
                    //}

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
            int SEQ = 0;

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

                #region grdMain1
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    // Update 처리
                    if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "추가")
                    {
                        //sql1 = string.Format("INSERT INTO TB_CM_SCR (BIZ_GP, SCR_ID, SCR_NM, PAGE_ID, USE_YN) VALUES('{0}','{1}','{2}','{3}','{4}')",
                        //    grdMain.GetData(row, 1), grdMain.GetData(row, 2), grdMain.GetData(row, 3), grdMain.GetData(row, 4), vf.StringToString(grdMain.GetData(row, 5).ToString()));
                        
                        sql1 = string.Format(" INSERT INTO TB_ZONE_MOVE_ROUTE ");
                        sql1 += string.Format("             ( ");
                        sql1 += string.Format("                ZONE_CD ");
                        sql1 += string.Format("               ,MOVE_ZONE_CD ");
                        sql1 += string.Format("               ,PROC_REMARKS ");
                        sql1 += string.Format("               ,ORD_YN ");
                        sql1 += string.Format("               ,REGISTER ");
                        sql1 += string.Format("               ,REG_DDTT ");
                        //sql1 += string.Format("               ,SEQ ");
                        sql1 += string.Format("             ) ");
                        sql1 += string.Format(" VALUES( ");
                        sql1 += string.Format("         '{0}' ", grdMain.GetData(row, "ZONE_CD"));             /* ZONE_CD ");*/
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "MOVE_ZONE_CD"));                  /* MOVE_ZONE_CD ");*/
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "PROC_REMARKS"));              /* PROC_REMARKS ");*/
                        sql1 += string.Format("        ,'{0}' ", vf.StringToString(grdMain.GetData(row, "ORD_YN").ToString()));             /* ORD_YN ");*/
                        sql1 += string.Format("        ,'{0}' ", ck.UserID);                                   /* REGISTER ");*/
                        sql1 += string.Format("        ,SYSDATE ");                                            /* REG_DDTT ");*/
                        //sql1 += string.Format("        , SELECT MAX(SEQ)+1 FROM TB_ZONE_MOVE_ROUTE ");             /* SEQ ");*/
                        sql1 += string.Format("       ) ");


                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "수정")
                    {
                        //sql1 = string.Format("UPDATE TB_CM_SCR SET BIZ_GP = '{0}', SCR_NM = '{2}', PAGE_ID = '{3}', USE_YN = '{4}' WHERE SCR_ID = '{5}'",
                        //    grdMain.GetData(row, 1), grdMain.GetData(row, 2), grdMain.GetData(row, 3), grdMain.GetData(row, 4), vf.StringToString(grdMain.GetData(row, 5).ToString()), grdMain.GetData(row, 2));

                        sql1 = string.Format(" UPDATE TB_ZONE_MOVE_ROUTE ");
                        sql1 += string.Format(" SET  ");

                        sql1 += string.Format("        ZONE_CD = '{0}' ", grdMain.GetData(row, "ZONE_CD"));
                        sql1 += string.Format("       ,MOVE_ZONE_CD = '{0}' ", grdMain.GetData(row, "MOVE_ZONE_CD"));
                        sql1 += string.Format("       ,PROC_REMARKS = '{0}' ", grdMain.GetData(row, "PROC_REMARKS"));
                        sql1 += string.Format("       ,ORD_YN = '{0}' ", vf.StringToString(grdMain.GetData(row, "ORD_YN").ToString()));
                       
                        sql1 += string.Format("       ,REGISTER = '{0}' ", ck.UserID);
                        sql1 += string.Format("       ,REG_DDTT = SYSDATE ");
                        sql1 += string.Format(" WHERE ZONE_CD = '{0}' AND PROC_REMARKS = '{1}'", grdMain.GetData(row, "ZONE_CD"), grdMain.GetData(row, "PROC_REMARKS"));
                        //sql1 += string.Format(" WHERE ZONE_CD = '{0}' AND SEQ = '{1}'", grdMain.GetData(row, "ZONE_CD"), grdMain.GetData(row, "L_NUM"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    {
                        sql1 = string.Format("DELETE FROM TB_ZONE_MOVE_ROUTE WHERE ZONE_CD = '{0}' AND MOVE_ZONE_CD = '{1}'", grdMain.GetData(row, 1), grdMain.GetData(row, 2));
                        //sql1 = string.Format("DELETE FROM TB_ZONE_MOVE_ROUTE WHERE ZONE_CD = '{0}' AND SEQ = '{1}'", grdMain.GetData(row, 1), grdMain.GetData(row, "L_NUM"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        DelCnt++;
                    }
                }
                #endregion grdMain1 
                //실행후 성공
                transaction.Commit();
                btnDisplay_Click(null, null);
                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴
                InitGrd_Main();
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
        private void SetDataBinding()
        {
        

            string sql1 = string.Format("SELECT TO_CHAR(rownum) as L_NUM, ");
            sql1 += string.Format("X.*  ");
            sql1 += string.Format("from  ");
            sql1 += string.Format("(SELECT  ");
            sql1 += string.Format("  ZONE_CD, ");
            sql1 += string.Format(" MOVE_ZONE_CD, ");
            sql1 += string.Format("  (CASE WHEN ORD_YN = 'Y' THEN 'True' ELSE 'False' END) AS ORD_YN,");
            sql1 += string.Format("  PROC_REMARKS, ");
            sql1 += string.Format("  '' AS GUBUN ");
            sql1 += string.Format(" from TB_ZONE_MOVE_ROUTE ");
            sql1 += string.Format(" where ZONE_CD LIKE '{0}' || '%' ", txtSCR_ID);
            sql1 += string.Format(" AND MOVE_ZONE_CD  LIKE '%{0}%' || '%' ", txtSCR_NM); //:SCR_ID
            sql1 += string.Format(" ORDER BY ZONE_CD, MOVE_ZONE_CD ASC ) X ");

            olddt = cd.FindDataTable(sql1);

            moddt = olddt.Copy();
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSCRID_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {

                btnDisplay_Click(null, null);
            }
        }

        private void txtSCRNM_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {

                btnDisplay_Click(null, null);
            }
        }

        private void grdMain_CellChecked(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedGrd.GetData(editedRow, editedGrd.Cols.Count - 1).ToString() != "추가")
            {
                // 저장시 UPDATE로 처리하기 위해 flag set
                editedGrd.SetData(editedRow, editedGrd.Cols.Count - 1, "수정");
                editedGrd.SetData(editedRow, 0, "수정");

                // Update 배경색 지정
                editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            }

        }
    }
}
