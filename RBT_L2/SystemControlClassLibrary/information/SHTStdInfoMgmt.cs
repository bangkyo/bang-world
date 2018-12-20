using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Data;
using System.Diagnostics;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Globalization;

namespace SystemControlClassLibrary.information
{
    public partial class SHTStdInfoMgmt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;

        clsStyle cs = new clsStyle();

        // 셀의 수정전 값
        private string strBefValue = "";

        private string ownerNM = "";
        private string titleNM = "";
        private string bndWgt = "";
        private int editRow = 0;
        private string txtitem = "";

        TextBox tbRollerRPM;
        TextBox tbFrequency;

        #endregion 변수 설정

        #region 생성자, 로드 설정

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public SHTStdInfoMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void SHTStdInfoMgmt_Load(object sender, EventArgs e)
        {
            InitControl();

            btnDisplay_Click(null, null);
        }
        #endregion 생성자, 로드 설정

        #region init 컨트롤, 그리드 설정
        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);

            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitPanel(panel1);

            clsStyle.Style.InitLabel(item_size_lb);

            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnSave);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnClose);

            clsStyle.Style.InitTextBox(item_size_tb);

            InitGrd_Main();
        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["TRANS_ROLLER_RPM"].Index);//, 0, grdMain.Cols["ILLUM"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["IMPELLER_FREQUENCY"].Index);//, 0, grdMain.Cols["ILLUM"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            int nwidth = (grdMain.Size.Width - cs.L_No_Width - 5) / 4;

            grdMain.Cols["NUM"].Width = cs.L_No_Width;
            grdMain.Cols["ITEM_SIZE_MIN"].Width = nwidth;
            grdMain.Cols["ITEM_SIZE_MAX"].Width = nwidth;
            grdMain.Cols["TRANS_ROLLER_RPM"].Width = nwidth;
            grdMain.Cols["IMPELLER_FREQUENCY"].Width = nwidth;

            grdMain.Cols["GUBUN"].Width = 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["NUM"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["ITEM_SIZE_MIN"].TextAlign = cs.SIZE_MIN_TextAlign;
            grdMain.Cols["ITEM_SIZE_MAX"].TextAlign = cs.SIZE_MAX_TextAlign;
            grdMain.Cols["TRANS_ROLLER_RPM"].TextAlign = cs.MLFT_RPM_TextAlign;
            grdMain.Cols["IMPELLER_FREQUENCY"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            tbRollerRPM = new TextBox();
            cs.InitTextBox(tbRollerRPM);

            // 소수점자리하나를 포함한 5자리수의 숫자 입력
            // oracle number(5,1)
            tbRollerRPM.MaxLength = 6;
            tbRollerRPM.KeyPress += Number_KeyPress;
            tbRollerRPM.TextAlign = HorizontalAlignment.Right;
            tbRollerRPM.Text = vf.Format(vf.CDbl(tbRollerRPM.Text), "#,##0.0");
            grdMain.Cols["TRANS_ROLLER_RPM"].Editor = tbRollerRPM;

            tbFrequency = new TextBox();
            cs.InitTextBox(tbFrequency);

            // 소수점자리하나를 포함한 5자리수의 숫자 입력
            // oracle number(5,1)
            tbFrequency.MaxLength = 6;
            tbFrequency.KeyPress += Number_KeyPress;
            tbFrequency.TextAlign = HorizontalAlignment.Right;
            tbFrequency.Text = vf.Format(vf.CDbl(tbFrequency.Text), "#,##0.0");
            grdMain.Cols["IMPELLER_FREQUENCY"].Editor = tbFrequency;
        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 소수점을 포함한 숫자만 입력 가능
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != '.' && e.KeyChar != 8)

            {
                e.Handled = true;
            }
        }

        #endregion init 컨트롤, 그리드 설정

        #region 컨트롤 이벤트 설정

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            SetDataBinding();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        private void item_size_tb_TextChanged(object sender, EventArgs e)
        {
            txtitem = item_size_tb.Text;
        }

        private void btnRowAdd_Click(object sender, EventArgs e)
        {

            // 수정가능 하도록 열추가
            grdMain.Rows.Add();

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, "추가");
            grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");
            // Insert 배경색 지정
            grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];

            //// 커서위치 결정
            grdMain.Row = 0;
            grdMain.Col = 0;
        }

        private void btnDelRow_Click(object sender, EventArgs e)
        {

            if (grdMain.Rows.Count < 2 || grdMain.Row < 1)
            {
                return;
            }

            //mj 추가되었지만 행삭제로 지울때
            if (grdMain.Rows[grdMain.Row][0].ToString() == "추가")
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
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void item_size_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

        #endregion 컨트롤 이벤트 설정

        #region 그리드 BeforeEdit, AfterEdit 설정

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            if (grdMain.Row < 1 || grdMain.GetData(grdMain.Row, grdMain.Col) == null)
            {
                return;
            }

            //추가된 행이 아니면 메뉴 ITEM_SIZE_MIN 열은  수정되지않게 한다
            if (e.Col == grdMain.Cols["ITEM_SIZE_MIN"].Index && grdMain.GetData(e.Row, "NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 메뉴 ITEM_SIZE_MAX 열은  수정되지않게 한다
            if (e.Col == grdMain.Cols["ITEM_SIZE_MAX"].Index && grdMain.GetData(e.Row, "NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grdMain.GetData(grdMain.Row, grdMain.Col).ToString();
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            string set_value = "";

            // No,구분은 수정 불가
            if (grdMain.Col == 0 || grdMain.Col == grdMain.Cols.Count - 1)
            {
                grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdMain.GetData(grdMain.Row, grdMain.Col).ToString().Trim())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdMain.GetData(grdMain.Row, 0).ToString() != "추가")
            {
                // 규격 수정 불가
                if (e.Col == grdMain.Cols["ITEM_SIZE_MIN"].Index || e.Col == grdMain.Cols["ITEM_SIZE_MAX"].Index)
                {
                    grdMain.SetData(e.Row, e.Col, strBefValue);
                    return;
                }

                // 저장시 UPDATE로 처리하기 위해 flag set
                grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                grdMain.SetData(grdMain.Row, 0, "수정");
                // Update 배경색 지정
                grdMain.Rows[grdMain.Row].Style = grdMain.Styles["UpColor"];
            }
            else
            {
                if (grdMain.Col == grdMain.Cols["ITEM_SIZE_MIN"].Index)
                {
                    set_value = vf.Format(vf.CInt(grdMain.GetData(grdMain.Row, "ITEM_SIZE_MIN").ToString()).ToString().Trim(), "0000");
                    grdMain.SetData(grdMain.Row, "ITEM_SIZE_MIN", set_value);
                }

                if (grdMain.Col == grdMain.Cols["ITEM_SIZE_MAX"].Index)
                {
                    set_value = vf.Format(vf.CInt(grdMain.GetData(grdMain.Row, "ITEM_SIZE_MAX").ToString()).ToString().Trim(), "0000");
                    grdMain.SetData(grdMain.Row, "ITEM_SIZE_MAX", set_value);
                }
            }
        }

        #endregion 그리드 BeforeEdit, AfterEdit 설정

        #region 저장 설정

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql1 = string.Empty;
            string strMsg = string.Empty;

            #region 삭제항목체크
            string check_value1 = string.Empty;
            string check_Cols_NM1 = string.Empty;
            string check_field_NM1 = string.Empty;
            string check_table_NM1 = string.Empty;

            string check_value2 = string.Empty;
            string check_Cols_NM2 = string.Empty;
            string check_field_NM2 = string.Empty;
            string check_table_NM2 = string.Empty;

            string check_value3 = string.Empty;
            string check_Cols_NM3 = string.Empty;
            string check_field_NM3 = string.Empty;
            string check_table_NM3 = string.Empty;

            string check_value4 = string.Empty;
            string check_Cols_NM4 = string.Empty;
            string check_field_NM4 = string.Empty;
            string check_table_NM4 = string.Empty;

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
                gubun_value = grdMain.GetData(checkrow, 0).ToString();

                if (gubun_value == "삭제")
                {
                    isChange = true;
                }

                if (gubun_value == "수정" || gubun_value == "추가")
                {
                    if (gubun_value == "추가")
                    {
                        #region ITEM_SIZE_MIN 체크
                        check_field_NM1 = "ITEM_SIZE_MIN";
                        check_table_NM1 = "TB_SHT_STDINFO";
                        check_value1 = grdMain.GetData(checkrow, check_field_NM1).ToString();
                        check_Cols_NM1 = grdMain.Cols[check_field_NM1].Caption;

                        if (string.IsNullOrEmpty(check_value1))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                            MessageBox.Show(show_msg);
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }
                        else
                        {
                            if (vf.compareMinMax(check_value1, grdMain, checkrow))
                            {
                                grdMain.Select(checkrow, 1, true);
                                return;
                            }
                        }

                        if (vf.isContainHangul(check_value1))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value1) > 4)
                        {
                            MessageBox.Show("숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }

                        // 명이입력되지 않은경우 체크
                        check_field_NM1 = "ITEM_SIZE_MIN";
                        check_NM = grdMain.GetData(checkrow, check_field_NM1).ToString();
                        check_Cols_NM1 = grdMain.Cols[check_field_NM1].Caption;

                        if (string.IsNullOrEmpty(check_NM))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                            MessageBox.Show(show_msg);
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }
                        isChange = true;
                        #endregion ITEM_SIZE_MIN 체크

                        #region ITEM_SIZE_MAX 체크
                        check_field_NM2 = "ITEM_SIZE_MAX";
                        check_table_NM2 = "TB_SHT_STDINFO";
                        check_value2 = grdMain.GetData(checkrow, check_field_NM2).ToString();
                        check_Cols_NM2 = grdMain.Cols[check_field_NM2].Caption;

                        if (string.IsNullOrEmpty(check_value2))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                            MessageBox.Show(show_msg);
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }
                        else
                        {
                            if (vf.compareMinMax(check_value2, grdMain, checkrow))
                            {
                                grdMain.Select(checkrow, 1, true);
                                return;
                            }
                        }

                        if (vf.isContainHangul(check_value2))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value2) > 4)
                        {
                            MessageBox.Show("숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.Has_Item(check_table_NM2, check_field_NM2, check_value2) && vf.Has_Item(check_table_NM1, check_field_NM1, check_value1))
                        {
                            show_msg = string.Format("필수요소가 중복되었습니다.", check_Cols_NM1, check_Cols_NM2);
                            MessageBox.Show(show_msg);
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }

                        isChange = true;
                        #endregion ITEM_SIZE_MAX 체크

                        // 크기비교
                        if (vf.CInt(check_value1) >= vf.CInt(check_value2))
                        {
                            string message = string.Format("{0}의 값이 {1}값보다 크거나 같습니다.", check_Cols_NM1, check_Cols_NM2);
                            MessageBox.Show(message);
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }
                        if (vf.IsSameRangeMinMax(check_table_NM1, check_value1, check_value2))
                        {
                            string message = string.Format("중복하는 범위가 이미 존재합니다.");
                            MessageBox.Show(message);
                            grdMain.Select(checkrow, 1, true);
                            return;
                        }
                    }

                    #region TRANS_ROLLER_RPM 체크
                    check_field_NM3 = "TRANS_ROLLER_RPM";
                    check_table_NM3 = "TB_SHT_STDINFO";
                    check_value3 = vf.Format(grdMain.GetData(checkrow, check_field_NM3).ToString(), "####.0");
                    check_Cols_NM3 = grdMain.Cols[check_field_NM3].Caption;

                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM3);
                        MessageBox.Show(show_msg);
                        grdMain.Select(checkrow, 1, true);
                        return;
                    }

                    if (vf.isContainHangul(check_value3))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        grdMain.Select(checkrow, 1, true);
                        return;
                    }

                    if (vf.getStrLen(check_value3) > 6)
                    {
                        MessageBox.Show("소수점이하 자리수를 포함한 숫자 5자 이하로 입력하시기 바랍니다.");
                        grdMain.Select(checkrow, 1, true);
                        return;
                    }
                    #endregion TRANS_ROLLER_RPM 체크

                    #region IMPELLER_FREQUENCY 체크
                    check_field_NM4 = "IMPELLER_FREQUENCY";
                    check_table_NM4 = "TB_SHT_STDINFO";
                    check_value4 = vf.Format(grdMain.GetData(checkrow, check_field_NM4).ToString(), "####.0");
                    check_Cols_NM4 = grdMain.Cols[check_field_NM4].Caption;

                    if (string.IsNullOrEmpty(check_value4))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM4);
                        MessageBox.Show(show_msg);
                        grdMain.Select(checkrow, 1, true);
                        return;
                    }

                    if (vf.isContainHangul(check_value4))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                        grdMain.Select(checkrow, 1, true);
                        return;
                    }

                    if (vf.getStrLen(check_value4) > 5)
                    {
                        MessageBox.Show("소수점이하 자리수를 포함한 숫자 5자 이하로 입력하시기 바랍니다.");
                        grdMain.Select(checkrow, 1, true);
                        return;
                    }
                    #endregion IMPELLER_FREQUENCY 체크

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

            DataTable dt = null;

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
                    if (grdMain.GetData(row, 0).ToString() == "추가")
                    {

                        sql1 = string.Format("INSERT ");
                        sql1 += string.Format("INTO TB_SHT_STDINFO( ");
                        sql1 += string.Format("                     ITEM_SIZE_MIN ");
                        sql1 += string.Format("                     ,ITEM_SIZE_MAX ");
                        sql1 += string.Format("                     ,TRANS_ROLLER_RPM ");
                        sql1 += string.Format("                     ,IMPELLER_FREQUENCY ");
                        sql1 += string.Format("                     ,REGISTER  ");
                        sql1 += string.Format("                     ,REG_DDTT  ");
                        sql1 += string.Format("                    ) ");
                        sql1 += string.Format("                    VALUES( ");
                        sql1 += string.Format("                             '{0}' ", grdMain.GetData(row, "ITEM_SIZE_MIN").ToString());
                        sql1 += string.Format("                            ,'{0}' ", grdMain.GetData(row, "ITEM_SIZE_MAX").ToString());
                        sql1 += string.Format("                            ,'{0}' ", grdMain.GetData(row, "TRANS_ROLLER_RPM").ToString());
                        sql1 += string.Format("                            ,'{0}'  ", grdMain.GetData(row, "IMPELLER_FREQUENCY").ToString());
                        sql1 += string.Format("                            ,'{0}' ", ck.UserID);
                        sql1 += string.Format("                            ,SYSDATE ");
                        sql1 += string.Format("                           ) ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString() == "수정")
                    {
                        sql1 += string.Format("UPDATE TB_SHT_STDINFO ");
                        sql1 += string.Format("SET TRANS_ROLLER_RPM   = '{0}' ", grdMain.GetData(row, "TRANS_ROLLER_RPM").ToString());
                        sql1 += string.Format("   ,IMPELLER_FREQUENCY = '{0}' ", grdMain.GetData(row, "IMPELLER_FREQUENCY").ToString());
                        sql1 += string.Format("   ,MODIFIER       = '{0}'   ", ck.UserID);
                        sql1 += string.Format("   ,MOD_DDTT       = SYSDATE ");
                        sql1 += string.Format("WHERE ITEM_SIZE_MIN    = '{0}' ", grdMain.GetData(row, "ITEM_SIZE_MIN"));
                        sql1 += string.Format("  AND ITEM_SIZE_MAX    = '{0}' ", grdMain.GetData(row, "ITEM_SIZE_MAX"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString() == "삭제")
                    {
                        sql1 += string.Format("  DELETE ");
                        sql1 += string.Format("FROM TB_SHT_STDINFO ");
                        sql1 += string.Format("WHERE ITEM_SIZE_MIN = '{0}' ", grdMain.GetData(row, "ITEM_SIZE_MIN"));
                        sql1 += string.Format("  AND ITEM_SIZE_MAX = '{0}' ", grdMain.GetData(row, "ITEM_SIZE_MAX"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        DelCnt++;
                    }
                }
                #endregion grdMain1 
                //실행후 성공
                transaction.Commit();

                btnDisplay_Click(null, null);
               
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
                MessageBox.Show(ex.Message);
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

        #endregion 저장 설정

        #region 조회 설정

        private void SetDataBinding()
        {
            string sql1 = string.Format("SELECT TO_CHAR(ROWNUM) AS NUM ");
            sql1 += string.Format("            ,X.* ");
            sql1 += string.Format("            ,'' AS GUBUN ");
            sql1 += string.Format("      FROM  ( ");
            sql1 += string.Format("             SELECT ITEM_SIZE_MIN ");  //--규격MIN 
            sql1 += string.Format("                   ,ITEM_SIZE_MAX ");  //--규격MAX 
            sql1 += string.Format("                   ,TRANS_ROLLER_RPM ");  //--이송 롤러 회전수;
            sql1 += string.Format("                   ,IMPELLER_FREQUENCY ");  // --임펠라 주파수 
            sql1 += string.Format("             FROM  TB_SHT_STDINFO ");
            sql1 += string.Format("             WHERE TO_NUMBER(ITEM_SIZE_MIN) <= NVL(TO_NUMBER('{0}'),9999)", txtitem);
            sql1 += string.Format("             AND   TO_NUMBER(ITEM_SIZE_MAX) >= NVL(TO_NUMBER('{0}'),0000)", txtitem);
            sql1 += string.Format("             ORDER BY ITEM_SIZE_MIN, ITEM_SIZE_MAX ");
            sql1 += string.Format("             ) X ");

            olddt = cd.FindDataTable(sql1);

            moddt = olddt.Copy();
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;
        }

        #endregion 조회 설정

        private void grdMain_StartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (grd.GetData(grd.Row, "GUBUN").ToString() != "추가")
            {
                if (grd.ColSel == grd.Cols["NUM"].Index || grd.ColSel == grd.Cols["ITEM_SIZE_MIN"].Index || grd.ColSel == grd.Cols["ITEM_SIZE_MAX"].Index)
                {

                    e.Cancel = true;
                }
            }
        }
    }
}
