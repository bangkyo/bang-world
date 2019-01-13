using ComLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using ComLib.clsMgr;
using System.Data.OracleClient;
using System.Collections.Specialized;
using System.Collections;

namespace SystemControlClassLibrary.ResultsMgmt
{
    public partial class EqpChkItemMgmt : Form
    {
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsCom ck = new clsCom();

        DataTable olddt;
        DataTable moddt;

        DataTable olddt_sub;
        DataTable moddt_sub;

        DataTable grdMain_dt;
        DataTable grdSub_dt;

        List<string> msg;
        List<string> modifList;

        clsStyle cs = new clsStyle();

        private static string cd_id = "";
        private static string eq_nm = "";

        // 셀의 수정전 값
        private string strBefValue = string.Empty;
        private string changed_Value = string.Empty;

        private static string selected_Eq_cd = "";

        ListDictionary RouTing_id_nm = null;
        ListDictionary RouTing_nm_id = null;
        ListDictionary cycle_cb = null;

        private static string ownerNM = "";
        private static string titleNM = "";

        string head = "";
        string modified_item = "";
        string beforeModfy = "";
        string afterModfy = "";

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public EqpChkItemMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

            Load += FrmComControl_Load;

            grdMain.AfterEdit += GrdMain_AfterEdit;
            grdMain.DoubleClick += GrdMain_DoubleClick;
            grdMain.Click += grdMain_Click;

            grdSub.AfterEdit += GrdSub_AfterEdit;
            grdSub.BeforeEdit += GrdSub_BeforeEdit;
            grdSub.DoubleClick += GrdSub_DoubleClick;
            
            cboLine_GP.SelectedValueChanged += cboLine_GP_SelectedValueChanged;
        }

        private void GrdSub_AfterAddRow(object sender, RowColEventArgs e)
        {
            grdSub[e.Row, "CHECK_START_DATE"] = DateTime.Now;
            grdSub[e.Row, "CHECK_END_DATE"] = DateTime.Now;
            grdSub[e.Row, "USE_YN"] = true;
        }

        private void GrdMain_AfterAddRow(object sender, RowColEventArgs e)
        {
            grdMain[e.Row, "INST_DATE"] = DateTime.Now;
        }

        private void cboLine_GP_SelectedValueChanged(object sender, EventArgs e)
        {
            cd_id = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = cd_id;
        }

        private void GrdSub_DoubleClick(object sender, EventArgs e)
        {
            if (grdSub.Row <= 0)
            {
                return;
            }

            grdSub.AllowEditing = true;
        }

        private void GrdSub_BeforeEdit(object sender, RowColEventArgs e)
        {
            if (grdSub.Row <= 0)
            {
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grdSub.GetData(grdSub.Row, grdSub.Col).ToString();
        }

        private void GrdSub_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            DataTable dt = olddt_sub;

            changed_Value = grd.GetData(e.Row, e.Col).ToString().Trim();

            // No,구분은 수정 불가
            if (e.Col == 0 || e.Col == grd.Cols.Count - 1)
            {
                grd.SetData(e.Row, e.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == changed_Value)
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grd.GetData(e.Row, grd.Cols.Count - 1).ToString() != "추가")
            {
                // 항목코드 수정불가
                if (e.Col == 1)
                {
                    grd.SetData(e.Row, e.Col, strBefValue);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grd.SetData(e.Row, grd.Cols.Count - 1, "수정");
                grd.SetData(e.Row, 0, "수정");

                // Update 배경색 지정
                grd.Rows[e.Row].Style = grd.Styles["UpColor"];
            }
            else
            {
                if (e.Col == grd.Cols["ITEM_CD"].Index)
                {
                    grd.SetData(e.Row, e.Col, vf.UCase(grd.GetData(e.Row, "ITEM_CD").ToString()));
                }
            }
        }

        private void GrdMain_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain.Row <= 0)
            {
                return;
            }

            grdMain.AllowEditing = true;
        }

        private void GrdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (e.Row < 1 || grd.GetData(e.Row, e.Col) == null)
            {
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(e.Row, e.Col).ToString();
        }

        private void GrdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            DataTable dt = olddt;
            changed_Value = grd.GetData(e.Row, e.Col).ToString().Trim();

            // No,구분은 수정 불가
            if (e.Col == 0 || e.Col == grd.Cols.Count - 1)
            {
                grd.SetData(e.Row, e.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue.ToString() == changed_Value.ToString())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grd.GetData(e.Row, grd.Cols.Count - 1).ToString() != "추가")
            {
                // 설비코드 수정 불가
                if (e.Col == 1)
                {
                    grd.SetData(e.Row, e.Col, strBefValue);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grd.SetData(e.Row, grd.Cols.Count - 1, "수정");
                grd.SetData(e.Row, 0, "수정");

                // Update 배경색 지정
                grd.Rows[e.Row].Style = grd.Styles["UpColor"];
            }
            else
            {
                if (e.Col == grd.Cols["EQP_CD"].Index)
                {
                    grd.SetData(e.Row, e.Col, vf.UCase(grd.GetData(e.Row, "EQP_CD").ToString()));
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    Search();
                    break;

                case "btnRowAdd1":
                    DataRowAdd();
                    break;

                case "btnDelRow1":
                    DataRowDel();
                    break;

                case "btnRowAdd2":
                    DataRowAdd2();
                    break;

                case "btnDelRow2":
                    DataRowDel2(grdSub.Row);
                    break;

                case "btnSave":
                    SaveData();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

        private void Search()
        {
            InitGridData();
            SetDataBinding();  // 조회 버튼을 통한 데이터입력
        }

        private void InitGridData()
        {
            grdMain.SetDataBinding(grdMain_dt, null, true);

            grdSub.SetDataBinding(grdSub_dt, null, true);
        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }

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

            bool is_delrow = false;
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "GUBUN").ToString();

                if (gubun_value == "삭제")
                {
                    is_delrow = true;
                }
                if (gubun_value == "추가")
                {
                    is_delrow = true;

                    check_table_NM = "TB_EQP_INFO";
                    check_field_NM = "EQP_CD";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString();

                    if (string.IsNullOrEmpty(check_value))
                    {
                        MessageBox.Show("설비코드를 입력하세요.");
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
                        MessageBox.Show("설비코드가 중복되었습니다. 다시 입력해주세요.");
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "EQP_NM";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }
                }

                if (gubun_value == "수정")
                {
                    is_delrow = true;
                    // 명이입력되지 않은경우 체크
                    check_field_NM = "EQP_NM";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }
                }
            }

            for (checkrow = 1; checkrow < grdSub.Rows.Count; checkrow++)
            {
                gubun_value = grdSub.GetData(checkrow, "GUBUN").ToString().Trim();

                if (gubun_value == "삭제")
                {
                    is_delrow = true;
                }
                if (gubun_value == "추가")
                {
                    is_delrow = true;

                    check_table_NM = "TB_EQP_CHECK_ITEM";
                    check_field_NM = "ITEM_CD";

                    check_value = grdSub.GetData(checkrow, check_field_NM).ToString().Trim();

                    if (string.IsNullOrEmpty(check_value))
                    {
                        MessageBox.Show("코드를 입력하세요.");
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
                        MessageBox.Show("코드가 중복되었습니다. 다시 입력해주세요.");
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "CHECK_ITEM";
                    check_NM = grdSub.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdSub.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }
                }
                if (gubun_value == "수정")
                {
                    is_delrow = true;

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "CHECK_ITEM";
                    check_NM = grdSub.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdSub.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }
                }
            }
            if (is_delrow)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            #endregion

            string strQry = string.Empty;
            string strMsg = string.Empty;

            int row = 0;
            int InsCnt = 0;
            int UpCnt = 0;
            int DelCnt = 0;
            List<string> delSublist = null;

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

                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    // Insert 처리
                    if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "추가")
                    {
                        strQry = string.Format("INSERT INTO TB_EQP_INFO  ");
                        strQry += string.Format("( ");
                        strQry += string.Format("LINE_GP,  ");
                        strQry += string.Format("EQP_CD,  ");
                        strQry += string.Format("EQP_NM,     ");
                        strQry += string.Format("ROUTING_CD,     ");
                        strQry += string.Format("INST_DATE,   ");
                        strQry += string.Format("REGISTER,   ");
                        strQry += string.Format("REG_DDTT   ");
                        strQry += string.Format(") ");
                        strQry += string.Format("VALUES( ");
                        strQry += string.Format("'{0}' ,", cd_id);    //"selected_Eq_cd,  " 0
                        strQry += string.Format("'{0}' ,", grdMain.GetData(row, "EQP_CD"));                      //"EQP_CD,     " 1
                        strQry += string.Format("'{0}' ,", grdMain.GetData(row, "EQP_NM"));                      //"EQP_NM,     " 2
                        strQry += string.Format("'{0}' ,", grdMain.GetData(row, "ROUTING_CD").ToString());               //"ROUTING_CD,   " 3
                        strQry += string.Format("'{0}' ,", vf.Format(grdMain.GetData(row, "INST_DATE"), "yyyyMMdd"));  //"INST_DATE,   " 4
                        strQry += string.Format("'{0}' ,", ck.UserID);  //"REGISTER,   " 5
                        strQry += string.Format("SYSDATE "); //REG_DDTT
                        strQry += string.Format(") ");

                        cmd.CommandText = strQry;
                        cmd.ExecuteNonQuery();

                        InsCnt++;

                        // Insert Log 저장
                        msg.Add(" 라인: " + cd_id + "/ 설비명: " + grdMain.GetData(row, "EQP_NM").ToString() + " 추가 ");// + grdMain.GetData(row, "L3_CATEGORY") + " 추가 ");
                    }
                    // Update 처리
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "수정")
                    {
                        strQry = string.Format("UPDATE TB_EQP_INFO SET  ");
                        strQry += string.Format("EQP_NM = '{0}',   ", grdMain.GetData(row, "EQP_NM"));
                        strQry += string.Format("ROUTING_CD = '{0}',   ", grdMain.GetData(row, "ROUTING_CD").ToString());
                        strQry += string.Format("INST_DATE = '{0}',   ", vf.Format(vf.CDate(grdMain.GetData(row, "INST_DATE")), "yyyyMMdd"));
                        strQry += string.Format("MODIFIER = '{0}',  ", ck.UserID);
                        strQry += string.Format("MOD_DDTT = SYSDATE    ");//vf.BoolToString((Boolean)grdSub.GetData(row, "USE_YN")));
                        strQry += string.Format("WHERE LINE_GP = '{0}' ", cd_id);
                        strQry += string.Format("and  EQP_CD = '{0}' ", grdMain.GetData(row, "EQP_CD"));

                        cmd.CommandText = strQry;
                        cmd.ExecuteNonQuery();

                        modifList = new List<string>();

                        modifList.Add("EQP_NM");
                        modifList.Add("ROUTING_CD");
                        modifList.Add("INST_DATE");

                        foreach (string item in modifList)
                        {
                            if (olddt.Rows[row - 1][item].ToString() != grdMain.GetData(row, item).ToString())
                            {
                                head = "라인: " + cd_id + " /  "  + grdMain.GetData(row, "EQP_CD") +" / ";
                                modified_item = grdMain.Cols[item].Caption;
                                beforeModfy = olddt.Rows[row - 1][item].ToString();
                                afterModfy = grdMain.GetData(row, item).ToString();

                                msg.Add(head + modified_item + " : " + beforeModfy + " To " + afterModfy + "수정됨" );
                            }
                        }

                        msg.Add(strMsg);
                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    {
                        strQry = string.Format("DELETE FROM TB_EQP_INFO WHERE EQP_CD = '{0}'", grdMain.GetData(row, "EQP_CD"));

                        strMsg = " 설비: " + grdMain.GetData(row, "EQP_CD").ToString() + "를 삭제 ";
                        msg.Add(strMsg);

                        cmd.CommandText = strQry;
                        cmd.ExecuteNonQuery();

                        DelCnt++;
                    }
                }

                //gridSub 추가 수정 삭제 처리
                for (row = 1; row < grdSub.Rows.Count; row++)
                {
                    // Insert 처리 

                    if (grdSub.GetData(row, grdSub.Cols.Count - 1).ToString() == "추가")
                    {
                        strQry = string.Format("INSERT INTO TB_EQP_CHECK_ITEM  ");
                        strQry += string.Format("( ");
                        strQry += string.Format("LINE_GP,  ");
                        strQry += string.Format("EQP_CD,  ");
                        strQry += string.Format("ITEM_CD,  ");
                        strQry += string.Format("CHECK_ITEM,     ");
                        strQry += string.Format("CHECK_START_DATE,     ");
                        strQry += string.Format("CHECK_END_DATE,   ");
                        strQry += string.Format("CHECK_GAP,   ");
                        strQry += string.Format("CHECK_CYCLE,   ");
                        strQry += string.Format("USE_YN,   ");
                        strQry += string.Format("REGISTER,  ");
                        strQry += string.Format("REG_DDTT    ");
                        strQry += string.Format(") ");
                        strQry += string.Format("VALUES( ");
                        strQry += string.Format("'{0}' ,", cd_id);    
                        strQry += string.Format("'{0}' ,", selected_Eq_cd);    
                        strQry += string.Format("'{0}' ,", grdSub.GetData(row, "ITEM_CD"));                    
                        strQry += string.Format("nvl('{0}','check_item') ,", grdSub.GetData(row, "CHECK_ITEM"));                 
                        strQry += string.Format("'{0}' ,", vf.Format(grdSub.GetData(row, "CHECK_START_DATE"), "yyyyMMdd"));           
                        strQry += string.Format("'{0}' ,", vf.Format(grdSub.GetData(row, "CHECK_END_DATE"), "yyyyMMdd"));
                        strQry += string.Format("nvl({0}, 1) ,", grdSub.GetData(row, "CHECK_GAP"));
                        strQry += string.Format("'{0}' ,", grdSub.GetData(row, "CHECK_CYCLE"));
                        strQry += string.Format("'{0}' ,", vf.StringToString(grdSub.GetData(row, "USE_YN").ToString()));  
                        strQry += string.Format("'{0}' ,", ck.UserID);  //"COLUMND,   " 8
                        strQry += string.Format("SYSDATE ", "");  //"COLUMNE,   " 9
                        strQry += string.Format(") ");

                        cmd.CommandText = strQry;
                        cmd.ExecuteNonQuery();

                        InsCnt++;
                    }
                    // Update 처리
                    else if (grdSub.GetData(row, grdSub.Cols.Count - 1).ToString() == "수정")
                    {
                        strQry = string.Format ("UPDATE TB_EQP_CHECK_ITEM SET  ");
                        strQry += string.Format("CHECK_ITEM = '{0}' ",       grdSub.GetData(row, "CHECK_ITEM"));
                        strQry += string.Format(",CHECK_START_DATE = '{0}' ", vf.Format(grdSub.GetData(row, "CHECK_START_DATE"), "yyyyMMdd"));
                        strQry += string.Format(",CHECK_END_DATE = '{0}' ",   vf.Format(grdSub.GetData(row, "CHECK_END_DATE"), "yyyyMMdd"));
                        strQry += string.Format(",CHECK_GAP = {0} ", grdSub.GetData(row, "CHECK_GAP"));
                        strQry += string.Format(",CHECK_CYCLE = '{0}' ",       grdSub.GetData(row, "CHECK_CYCLE"));
                        strQry += string.Format(",USE_YN = '{0}'  ",          vf.StringToString(grdSub.GetData(row, "USE_YN").ToString()));//vf.BoolToString((Boolean)grdSub.GetData(row, "USE_YN")));
                        strQry += string.Format(",MODIFIER = '{0}'  ", ck.UserID);
                        strQry += string.Format(",MOD_DDTT = SYSDATE  " );
                        strQry += string.Format("WHERE LINE_GP = '{0}' ",       cd_id);
                        strQry += string.Format("and  EQP_CD = '{0}' ",         selected_Eq_cd);
                        strQry += string.Format("and  ITEM_CD = '{0}' ",        grdSub.GetData(row, "ITEM_CD"));

                        cmd.CommandText = strQry;
                        cmd.ExecuteNonQuery();

                        UpCnt++;
                    }
                    else if (grdSub.GetData(row, grdSub.Cols.Count - 1).ToString() == "삭제")
                    {
                        strQry = string.Format("DELETE FROM TB_EQP_CHECK_ITEM WHERE LINE_GP = '{0}' AND EQP_CD = '{1}' AND ITEM_CD ='{2}' ", cd_id, selected_Eq_cd, grdSub.GetData(row, "ITEM_CD").ToString());

                        cmd.CommandText = strQry;
                        cmd.ExecuteNonQuery();

                        msg.Add("라인코드 :" + cd_id + "/ 설비코드: " + selected_Eq_cd + " 점검항목 : " + grdSub.GetData(row, "ITEM_CD") + "  삭제");

                        DelCnt++;
                    }
                }// end of for(GridSub)

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

        private void DataRowDel2(int row)
        {
            if (row < 1)
            {
                return;
            }

            if (grdSub.Rows[grdSub.Row][grdSub.Cols.Count - 1].ToString() == "추가")
            {
                grdSub.RemoveItem(grdSub.Row);
                return;
            }

            // 저장시 Delete로 처리하기 위해 flag set
            grdSub.Rows[row][grdSub.Cols.Count - 1] = "삭제";
            grdSub.Rows[row][0] = "삭제";

            // Delete 배경색 지정
            grdSub.Rows[row].Style = grdSub.Styles["DelColor"];

            grdSub.Row = -1;
            grdSub.Col = -1;
        }

        private void DataRowAdd2()
        {
            // 수정가능 하도록 열추가
            grdSub.AllowEditing = true;
            grdSub.Rows.Add();

            // 추가열 데이타 초기화
            for (int col = 1; col < grdSub.Cols.Count - 1; col++)
                grdSub.SetData(grdSub.Rows.Count - 1, col, "");

            // 1행에 Count 자동 입력
            grdSub.SetData(grdSub.Rows.Count - 1, 0, (grdSub.Rows.Count - 1).ToString());

            // 저장시 Insert로 처리하기 위해 flag set
            grdSub.SetData(grdSub.Rows.Count - 1, grdSub.Cols.Count - 1, "추가");
            grdSub.SetData(grdSub.Rows.Count - 1, 0, "추가");
            grdSub.SetData(grdSub.Rows.Count - 1, "CHECK_START_DATE", DateTime.Now);
            grdSub.SetData(grdSub.Rows.Count - 1, "CHECK_END_DATE", DateTime.Now.AddYears(1));

            IEnumerator enumerator = cycle_cb.Keys.GetEnumerator();
            enumerator.MoveNext();
            object first = enumerator.Current;

            grdSub.SetData(grdSub.Rows.Count - 1, "CHECK_CYCLE", first);
            grdSub.SetData(grdSub.Rows.Count - 1, "CHECK_GAP", 1);
            grdSub.SetData(grdSub.Rows.Count - 1, "USE_YN", true);

            // Insert 배경색 지정
            grdSub.Rows[grdSub.Rows.Count - 1].Style = grdSub.Styles["InsColor"];
        }

        private void DataRowDel()
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
            grdMain.Rows[grdMain.Row][grdMain.Cols.Count - 1] = "삭제";
            grdMain.Rows[grdMain.Row][0] = "삭제";

            // Delete 배경색 지정
            grdMain.Rows[grdMain.Row].Style = grdMain.Styles["DelColor"];

            // 커서위치 결정
            grdMain.Row = 0;
            grdMain.Col = 0;
        }

        private void DataRowAdd()
        {
            // 수정가능 하도록 열추가
            grdMain.AllowEditing = true;
            grdMain.Rows.Add();

            // 1행에 Count 자동 입력
            grdMain.SetData(grdMain.Rows.Count - 1, 0, (grdMain.Rows.Count - 1).ToString());

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, "추가");
            grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");

            grdMain.SetData(grdMain.Rows.Count - 1, "EQP_CD", "");
            grdMain.SetData(grdMain.Rows.Count - 1, "EQP_NM", "");

            grdMain.SetData(grdMain.Rows.Count - 1, "INST_DATE", DateTime.Now);

            IEnumerator enumerator = RouTing_id_nm.Keys.GetEnumerator();
            enumerator.MoveNext();
            object first = enumerator.Current;

            grdMain.SetData(grdMain.Rows.Count - 1, "ROUTING_CD", first);
            // Insert 배경색 지정
            grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];

        }

        private bool SetDataBinding()
        {
            string strQry = string.Empty;
            bool isAnd = false;

            try
            {
                strQry += string.Format("select ");
                strQry += string.Format("       TO_CHAR(ROWNUM) AS L_NO ");
                strQry += string.Format("      ,EQP_CD ");
                strQry += string.Format("      ,EQP_NM ");
                strQry += string.Format("      ,ROUTING_CD ");
                strQry += string.Format("      ,TO_DATE(INST_DATE, 'YYYYMMDD') AS INST_DATE ");
                strQry += string.Format("      ,'' AS GUBUN ");
                strQry += string.Format("FROM ");
                strQry += string.Format("    TB_EQP_INFO ");
                strQry += string.Format("WHERE ");
                strQry += string.Format("       LINE_GP  LiKE '%{0}%'  ", cd_id);
                strQry += string.Format("AND    EQP_NM   LIKE '%{0}%'  ", eq_nm);
                strQry += string.Format("ORDER BY L_NO asc ");

                olddt = cd.FindDataTable(strQry);

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString() + "건이 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            if (grdMain.Rows.Count > 1)
            {
                grdMain_Row_Selected(1);
            }
            
            return true;
        }

        private void FrmComControl_Load(object sender, EventArgs e)
        {
            msg = new List<string>();

            this.BackColor = Color.White;

            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;

            InitControl();
   
            MakeInitgrdData();

            Button_Click(btnDisplay, null);
            //Search();
        }

        private void MakeInitgrdData()
        {
            grdMain_dt = vf.CreateDataTable(grdMain);
            grdSub_dt = vf.CreateDataTable(grdSub);
        }

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);
            cs.InitTitle(title_lb,  ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitButton(btnDisplay);
            cs.InitButton(btnSave);
            cs.InitButton(btnExcel);
            cs.InitButton(btnClose);

            cs.InitCombo(cboLine_GP, StringAlignment.Near);
           
            // 처음 버튼 disable
            btnRowAdd1.Enabled = false;
            btnDelRow1.Enabled = false;
            btnRowAdd2.Enabled = false;
            btnDelRow2.Enabled = false;

            SetComboBox();

            InitGrd_Main();
            
            InitGrdSub();
        }

        private void SetComboBox()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", false, ck.Line_gp);
        }

        private void InitGrdSub()
        {
            clsStyle.Style.InitGrid_search(grdSub);

            var crCellRange = grdSub.GetCellRange(0, grdSub.Cols["CHECK_ITEM"].Index, 0, grdSub.Cols["USE_YN"].Index);
            crCellRange.Style = grdSub.Styles["ModifyStyle"];

            grdSub.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            grdSub.AllowEditing = false;

            grdSub.Cols["L_NO"].Width = cs.L_No_Width;
            grdSub.Cols["ITEM_CD"].Width = 150;       // No
            grdSub.Cols["CHECK_ITEM"].Width = 150;
            grdSub.Cols["CHECK_START_DATE"].Width = 100;
            grdSub.Cols["CHECK_END_DATE"].Width = 100;
            grdSub.Cols["CHECK_GAP"].Width = 70;
            grdSub.Cols["CHECK_CYCLE"].Width = 70;
            grdSub.Cols["USE_YN"].Width = 80;
            grdSub.Cols["GUBUN"].Width = 0;

            #region 2. grdSub head 및 row  align 설정

            grdSub.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdSub.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdSub.Cols["ITEM_CD"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;       // No
            grdSub.Cols["CHECK_ITEM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
            grdSub.Cols["CHECK_START_DATE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdSub.Cols["CHECK_END_DATE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdSub.Cols["CHECK_GAP"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdSub.Cols["CHECK_CYCLE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
            grdSub.Cols["USE_YN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdSub.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            #endregion

            // CHECK_CYCLE 컬럼 - 점검주기
            cycle_cb = new ListDictionary();

            DataTable dt =  cd.Find_CD("CHECK_CYCLE");

            foreach (DataRow datarow in dt.Rows)
            {
                cycle_cb.Add(datarow["CD_ID"], datarow["CD_NM"]);
            }
            grdSub.Cols["CHECK_CYCLE"].DataMap = cycle_cb;
            grdSub.Cols["CHECK_CYCLE"].TextAlign = TextAlignEnum.LeftCenter;

            grdSub.Rows[0].AllowMerging = true;

            grdSub.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Custom;

            grdSub.MergedRanges.Add(grdSub.GetCellRange(0, grdSub.Cols["CHECK_GAP"].Index, 0, grdSub.Cols["CHECK_CYCLE"].Index));

            grdSub.ExtendLastCol = true;
        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["EQP_NM"].Index, 0, grdMain.Cols["INST_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            grdMain.AllowEditing = false;
            
            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["EQP_CD"].Width = 140;
            grdMain.Cols["EQP_NM"].Width = 140;
            grdMain.Cols["ROUTING_CD"].Width = 80;
            grdMain.Cols["INST_DATE"].Width = 100;
            grdMain.Cols["GUBUN"].Width = 0;

            #region 1. grdMain head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // No col은 1 이고 왼쪽 가운데 정렬로 한다.
            grdMain.Cols["EQP_CD"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["EQP_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
            grdMain.Cols["INST_DATE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            #endregion

            // 공정 코드 명칭을 리스트로 콤보박스에 입력
            string sql = string.Format("SELECT ROWNUM AS L_NO, N.* FROM( SELECT CD_ID , CD_NM   FROM TB_CM_COM_CD WHERE CATEGORY = 'ROUTING_CD' ORDER BY CD_ID) N ");
            var dt2 = cd.FindDataTable(sql);

            //컬럼1 - 공장
            RouTing_id_nm = new ListDictionary();
            RouTing_nm_id = new ListDictionary();
            
            foreach (DataRow row in dt2.Rows)
            {
                RouTing_id_nm.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                RouTing_nm_id.Add(row["CD_NM"].ToString(), row["CD_ID"].ToString());
            }
            
            grdMain.Cols["ROUTING_CD"].DataMap = RouTing_id_nm;
            grdMain.Cols["ROUTING_CD"].TextAlign = TextAlignEnum.CenterCenter;

            grdMain.ExtendLastCol = true;
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count <= 1)
            { return; }

            grdMain_Row_Selected(grdMain.RowSel);
        }

        private void grdMain_Row_Selected(int rowSel)
        {
            if (grdMain.GetData(rowSel, "EQP_CD") == null)
            {
                return;
            }

            selected_Eq_cd = grdMain.GetData(rowSel, "EQP_CD").ToString();

            string sql = string.Format("SELECT TO_CHAR(ROWNUM) AS L_NO, N.* FROM( SELECT ITEM_CD , CHECK_ITEM ,  TO_DATE(CHECK_START_DATE, 'YYYYMMDD') AS CHECK_START_DATE  , TO_DATE(CHECK_END_DATE, 'YYYYMMDD') AS CHECK_END_DATE  , CHECK_GAP, CHECK_CYCLE , (CASE WHEN USE_YN = 'Y' THEN 'True' ELSE 'False' END) AS USE_YN , '' AS GUBUN   FROM TB_EQP_CHECK_ITEM WHERE LINE_GP = '{0}' AND EQP_CD = '{1}' ORDER BY ITEM_CD) N " ,cd_id, selected_Eq_cd);

            var dt2 = cd.FindDataTable(sql);

            olddt_sub = dt2.Copy();

            moddt_sub = olddt_sub.Copy();

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdSub.SetDataBinding(moddt_sub, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt_sub.Rows.Count.ToString() + "건이 조회 되었습니다.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void eq_nm_tb_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                SendKeys.Send("{TAB}");
                Button_Click(btnDisplay, null);
            }
        }

        private void eq_nm_tb_TextChanged(object sender, EventArgs e)
        {
            eq_nm = eq_nm_tb.Text;
        }

        private void grdMain_StartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (grd.GetData(grd.Row, "GUBUN").ToString() != "추가")
            {
                if (grd.ColSel == grd.Cols["L_NO"].Index || grd.ColSel == grd.Cols["EQP_CD"].Index)
                {
                    e.Cancel = true;
                }

            }
        }

        private void grdSub_StartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (grd.GetData(grd.Row, "GUBUN").ToString() != "추가")
            {
                if (grd.ColSel == grd.Cols["L_NO"].Index || grd.ColSel == grd.Cols["ITEM_CD"].Index)
                {

                    e.Cancel = true;
                }

            }
        }
    }
}
