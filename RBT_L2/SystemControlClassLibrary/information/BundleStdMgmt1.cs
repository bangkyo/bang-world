using C1.Win.C1FlexGrid;
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

namespace SystemControlClassLibrary.information
{
    public partial class BundleStdMgmt1 : Form
    {
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;


        clsStyle cs = new clsStyle();

        // 셀의 수정전 값
        private  string strBefValue = "";

        private string ownerNM = "";
        private string titleNM = "";
        private string compareItem = "";
        private string compareItemSize = "";
        private string compareLength = "";
        private string comparePcs = "";
        private string txtitem = "";

        string set_value = "";

        int _i;
        TextBox tbNoPoint;
        TextBox tbBND_PCS;
        TextBox tbLength;

        private DataTable dtWgt;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public BundleStdMgmt1(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();


        }

        private void BundleStdMgmt1_Load(object sender, EventArgs e)
        {

            InitControl();
            
            btnDisplay_Click(null, null);
        }

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(label1);

            cs.InitButton(btnExcel);
            cs.InitButton(btnSave);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            ////////////////////////////////////////////
            //ColinComboBox
            cs.InitTextBox(item_size_tb);
            //SetComboBox1();
            InitGrd_Main();
        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["BND_PCS"].Index);//, 0, grdSub.Cols["USE_YN"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["REMARKS"].Index);//, 0, grdSub.Cols["USE_YN"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["NO1_BND_POINT"].Index);//, 0, grdSub.Cols["USE_YN"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["NO2_BND_POINT"].Index);//, 0, grdSub.Cols["USE_YN"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["NO3_BND_POINT"].Index);//, 0, grdSub.Cols["USE_YN"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            int level1 = 50; // 2자리
            int level2 = 60; // 4자리
            int level3 = 100; // 6자리
            int level4 = 140; // 8자리이상

            grdMain.Cols["NUM"].Width = cs.L_No_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width +40 ;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 40;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width + 50;
            grdMain.Cols["BND_PCS"].Width = cs.BND_PCS_Width + 50;
            grdMain.Cols["BND_WGT"].Width = cs.Wgt_Width + 20;
            grdMain.Cols["REMARKS"].Width = 150 ;
            grdMain.Cols["NO1_BND_POINT"].Width = 150 ;
            grdMain.Cols["NO2_BND_POINT"].Width = 150 ;
            grdMain.Cols["NO3_BND_POINT"].Width = 150 ;

            grdMain.Cols["GUBUN"].Width = 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["NUM"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["BND_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["BND_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["REMARKS"].TextAlign = cs.REMARKS_TextAlign;
            grdMain.Cols["NO1_BND_POINT"].TextAlign = cs.BND_POINT_TextAlign;
            grdMain.Cols["NO2_BND_POINT"].TextAlign = cs.BND_POINT_TextAlign;
            grdMain.Cols["NO3_BND_POINT"].TextAlign = cs.BND_POINT_TextAlign;

            grdMain.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.AllowEditing = true;
            grdMain.Cols["ITEM"].AllowEditing = false;
            grdMain.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["BND_WGT"].AllowEditing = false;

            grdMain.ExtendLastCol = true;


            tbLength = new TextBox();
            tbLength.MaxLength = 5;
            tbLength.KeyPress += vf.KeyPressEvent_decimal;
            tbLength.TextAlign = HorizontalAlignment.Right;
            grdMain.Cols["LENGTH"].Editor = tbLength;

            // 5자리수의 정수 입력
            // oracle number(5)
            tbBND_PCS = new TextBox();
            tbBND_PCS.MaxLength = 5;
            tbBND_PCS.KeyPress += vf.KeyPressEvent_number;
            tbBND_PCS.TextAlign = HorizontalAlignment.Right;
            grdMain.Cols["BND_PCS"].Editor = tbBND_PCS;

            // 4자리수의 정수 입력
            // oracle number(4)
            tbNoPoint = new TextBox();
            tbNoPoint.MaxLength = 4;
            tbNoPoint.KeyPress += vf.KeyPressEvent_number;
            tbNoPoint.TextAlign = HorizontalAlignment.Right;
            //tbNoPoint.Text = vf.Format(vf.CDbl(tbNoPoint.Text), "#,##0.0");
            grdMain.Cols["NO1_BND_POINT"].Editor = tbNoPoint;
            grdMain.Cols["NO2_BND_POINT"].Editor = tbNoPoint;
            grdMain.Cols["NO3_BND_POINT"].Editor = tbNoPoint;


        }

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
            item_size_tb.Text = vf.UCase(item_size_tb.Text);
            txtitem = item_size_tb.Text;
        }

        private void rowadd_btn_Click(object sender, EventArgs e)
        {

            // 수정가능 하도록 열추가
            grdMain.AllowEditing = true;
            grdMain.Cols[5].AllowEditing = false;

            grdMain.Cols[1].AllowEditing = true;
            grdMain.Cols[2].AllowEditing = true;
            grdMain.Cols[3].AllowEditing = true;
            grdMain.Cols[4].AllowEditing = true;

            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();
            grdMain.SetData(grdMain.Rows.Count - 1, 1, "RB");
            grdMain.SetData(grdMain.Rows.Count - 1, 0, (grdMain.Rows.Count - 1).ToString());
            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, "추가");
            grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");
            // Insert 배경색 지정
            grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];

            //// 커서위치 결정
            grdMain.Row = 0;
            grdMain.Col = 0;

        }
        private void rowdel_btn_Click(object sender, EventArgs e)
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

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            if (e.Row < 1 || grd.GetData(e.Row, e.Col) == null)
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //항목에서 품목은 수정되지 않는다.
            if (e.Col == grd.Cols["ITEM"].Index )
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(e.Row, e.Col).ToString();
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
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
                
                if (e.Col == grdMain.Cols["ITEM_SIZE"].Index)
                {
                    grdMain.SetData(e.Row, e.Col, strBefValue);
                    return;
                }

                if (e.Col == grdMain.Cols["LENGTH"].Index)
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
                if (e.Col == grdMain.Cols["ITEM_SIZE"].Index)
                {
                    set_value = vf.Format(vf.CInt(grdMain.GetData(grdMain.Row, "ITEM_SIZE").ToString()).ToString().Trim(), "0000");
                    grdMain.SetData(e.Row, e.Col, set_value);
   
                }

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql1 = string.Empty;
            string strMsg = string.Empty;

            #region 삭제항목체크
            string check_value1 = string.Empty;
            string check_Cols_NM1 = string.Empty;
            string check_field_NM1 = string.Empty;
            string check_table_NM1 = string.Empty;

            string check_item = string.Empty;
            string check_item_size = string.Empty;
            string check_Category = string.Empty;

            string check_value2 = string.Empty;
            string check_Cols_NM2 = string.Empty;
            string check_field_NM2 = string.Empty;
            string check_table_NM2 = string.Empty;

            string check_value3 = string.Empty;
            string check_Cols_NM3 = string.Empty;
            string check_field_NM3 = string.Empty;
            string check_table_NM3 = string.Empty;
            
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

                if (gubun_value == "삭제" )
                {
                    isChange = true;
                }


                if (gubun_value == "추가" || gubun_value == "수정")
                {

                    if (gubun_value == "추가")
                    {
                        #region ITEM 체크
                        check_field_NM1 = "ITEM";
                        check_table_NM1 = "TB_BUNDLE_PIECESTD1";
                        check_value1 = grdMain.GetData(checkrow, check_field_NM1).ToString();
                        check_Cols_NM1 = grdMain.Cols[check_field_NM1].Caption;

                        if (string.IsNullOrEmpty(check_value1))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                            MessageBox.Show(show_msg);
                            return;
                        }

                        if (vf.isContainHangul(check_value1))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            return;
                        }

                        if (vf.getStrLen(check_value1) > 30)
                        {
                            MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                            return;
                        }


                        #endregion 

                        #region ITEM_SIZE 체크
                        check_field_NM2 = "ITEM_SIZE";
                        check_table_NM2 = "TB_BUNDLE_PIECESTD1";
                        check_value2 = grdMain.GetData(checkrow, check_field_NM2).ToString();
                        check_Cols_NM2 = grdMain.Cols[check_field_NM2].Caption;

                        if (string.IsNullOrEmpty(check_value2))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                            MessageBox.Show(show_msg);
                            return;
                        }

                        if (vf.isContainHangul(check_value2))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            return;
                        }

                        if (vf.getStrLen(check_value2) > 30)
                        {
                            MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                            return;
                        }

                        #region # 추가될 품목에 따른 규격의 존재 유무파악 (없을경우 이론중량을 계산못함을 알림)
                        check_item = "RB";
                        check_item_size = grdMain.GetData(checkrow, "ITEM_SIZE").ToString();

                        if (!vf.Has_Item("TB_CM_COM_CD", "CATEGORY", check_item, "CD_ID", check_item_size))
                        {
                            show_msg = string.Format("{0} 의 {1} 규격의 값이 존재하지않습니다.", check_item, check_item_size);
                            MessageBox.Show(show_msg);
                            return;
                        }

                        #endregion #


                        #endregion ITEM_SIZE 체크

                        #region LENGTH 체크
                        check_field_NM3 = "LENGTH";
                        check_table_NM3 = "TB_BUNDLE_PIECESTD1";
                        check_value3 = grdMain.GetData(checkrow, check_field_NM3).ToString();
                        check_Cols_NM3 = grdMain.Cols[check_field_NM3].Caption;

                        if (string.IsNullOrEmpty(check_value3))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM3);
                            MessageBox.Show(show_msg);
                            return;
                        }

                        if (vf.isContainHangul(check_value3))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            return;
                        }
                        if (vf.isContainLetter(check_value3))
                        {
                            MessageBox.Show("문자가 포함되어서는 안됩니다.");
                            return;
                        }
                        if (vf.getStrLen(check_value3) > 8)
                        {
                            MessageBox.Show("숫자 {0}자 이하로 입력하시기 바랍니다..",8.ToString());
                            return;
                        }

                        //if (vf.Has_Item(check_table_NM3, check_field_NM3, check_value3) && vf.Has_Item(check_table_NM2, check_field_NM2, check_value2) && vf.Has_Item(check_table_NM1, check_field_NM1, check_value1))
                        if(vf.Has_Item(check_table_NM1, check_field_NM1, check_field_NM2, check_field_NM3, check_value1, check_value2, check_value3))
                        {
                            show_msg = string.Format("필수요소가 중복되었습니다. 다시 입력해주세요.");
                            MessageBox.Show(show_msg);
                            return;
                        }
                        #endregion ITEM_SIZE 체크
                    }

                    #region 본수 체크
                    check_field_NM3 = "BND_PCS";
                    check_value3 = grdMain.GetData(checkrow, check_field_NM3).ToString();
                    check_Cols_NM3 = grdMain.Cols[check_field_NM3].Caption;

                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를(을) 입력하세요.", check_Cols_NM3);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    #endregion

                    #region #1결속Point 체크
                    check_field_NM3 = "NO1_BND_POINT";
                    check_value3 = grdMain.GetData(checkrow, check_field_NM3).ToString();
                    check_Cols_NM3 = grdMain.Cols[check_field_NM3].Caption;

                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를(을) 입력하세요.", check_Cols_NM3);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    #endregion

                    #region #2결속Point 체크
                    check_field_NM3 = "NO2_BND_POINT";
                    check_value3 = grdMain.GetData(checkrow, check_field_NM3).ToString();
                    check_Cols_NM3 = grdMain.Cols[check_field_NM3].Caption;

                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를(을) 입력하세요.", check_Cols_NM3);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    #endregion

                    #region #3결속Point 체크
                    check_field_NM3 = "NO3_BND_POINT";
                    check_value3 = grdMain.GetData(checkrow, check_field_NM3).ToString();
                    check_Cols_NM3 = grdMain.Cols[check_field_NM3].Caption;

                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를(을) 입력하세요.", check_Cols_NM3);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    #endregion


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
                       
                        sql1 = string.Empty;
                        sql1 += string.Format("INSERT INTO TB_BUNDLE_PIECESTD1 ");
                        sql1 += string.Format("            ( ");
                        sql1 += string.Format("              ITEM ");
                        sql1 += string.Format("             ,ITEM_SIZE ");
                        sql1 += string.Format("             ,LENGTH ");
                        sql1 += string.Format("             ,BND_PCS ");
                        sql1 += string.Format("             ,NO1_BND_POINT ");
                        sql1 += string.Format("             ,NO2_BND_POINT ");
                        sql1 += string.Format("             ,NO3_BND_POINT ");
                        sql1 += string.Format("             ,REMARKS ");
                        sql1 += string.Format("             ,REGISTER ");
                        sql1 += string.Format("             ,REG_DDTT ");
                        sql1 += string.Format("            ) ");
                        sql1 += string.Format("VALUES( ");
                        sql1 += string.Format("       '{0}' ", grdMain.GetData(row, "ITEM"));
                        sql1 += string.Format("      ,'{0}' ", grdMain.GetData(row, "ITEM_SIZE"));
                        sql1 += string.Format("      ,'{0}' ", grdMain.GetData(row, "LENGTH"));
                        sql1 += string.Format("      ,'{0}' ", grdMain.GetData(row, "BND_PCS"));
                        sql1 += string.Format("      ,'{0}' ", grdMain.GetData(row, "NO1_BND_POINT"));
                        sql1 += string.Format("      ,'{0}' ", grdMain.GetData(row, "NO2_BND_POINT"));
                        sql1 += string.Format("      ,'{0}' ", grdMain.GetData(row, "NO3_BND_POINT"));
                        sql1 += string.Format("      ,'{0}' ", grdMain.GetData(row, "REMARKS"));
                        sql1 += string.Format("      ,'{0}' ", ck.UserID);
                        sql1 += string.Format("      ,SYSDATE ");
                        sql1 += string.Format("      ) ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        InsCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString() == "수정")
                    {
                        sql1 = string.Empty;
                        sql1 += string.Format("UPDATE TB_BUNDLE_PIECESTD1 ");
                        sql1 += string.Format("SET  BND_PCS = '{0}'       ", grdMain.GetData(row, "BND_PCS"));
                        sql1 += string.Format("    ,REMARKS = '{0}'       ", grdMain.GetData(row, "REMARKS"));
                        sql1 += string.Format("    ,NO1_BND_POINT = '{0}' ", grdMain.GetData(row, "NO1_BND_POINT"));
                        sql1 += string.Format("    ,NO2_BND_POINT = '{0}' ", grdMain.GetData(row, "NO2_BND_POINT"));
                        sql1 += string.Format("    ,NO3_BND_POINT = '{0}' ", grdMain.GetData(row, "NO3_BND_POINT"));
                        sql1 += string.Format("    ,MODIFIER = '{0}'      ", ck.UserID);
                        sql1 += string.Format("    ,MOD_DDTT = SYSDATE    ");
                        sql1 += string.Format("WHERE ITEM      = '{0}'    ", grdMain.GetData(row, "ITEM"));
                        sql1 += string.Format("AND   ITEM_SIZE = '{0}'    ", grdMain.GetData(row, "ITEM_SIZE"));
                        sql1 += string.Format("AND   LENGTH    = '{0}'    ", grdMain.GetData(row, "LENGTH"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, 0).ToString() == "삭제")
                    {
                        sql1 = string.Format("DELETE FROM TB_BUNDLE_PIECESTD1 WHERE ITEM = '{0}' AND ITEM_SIZE = '{1}' AND LENGTH ='{2}' ", grdMain.GetData(row, "ITEM"), grdMain.GetData(row, "ITEM_SIZE"), grdMain.GetData(row, "LENGTH") );
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
            string sql1 = string.Format(" SELECT TO_CHAR(rownum) as NUM ");
            sql1 += string.Format("              ,X.* ");
            sql1 += string.Format("              from ");
            sql1 += string.Format("                  (  ");
            sql1 += string.Format("                    SELECT  ");
            sql1 += string.Format("                    ITEM, ");
            sql1 += string.Format("                    ITEM_SIZE, ");
            sql1 += string.Format("                    LENGTH, ");
            sql1 += string.Format("                    BND_PCS, ");
            sql1 += string.Format("                    FN_GET_WGT (ITEM, ITEM_SIZE, LENGTH, BND_PCS) AS BND_WGT, ");
            sql1 += string.Format("                    NO1_BND_POINT, ");
            sql1 += string.Format("                    NO2_BND_POINT, ");
            sql1 += string.Format("                    NO3_BND_POINT, ");
            sql1 += string.Format("                    REMARKS ");
            sql1 += string.Format("                    from TB_BUNDLE_PIECESTD1  ");
            sql1 += string.Format("                    WHERE ITEM_SIZE LIKE '%' || '{0}' || '%' ", txtitem);
            sql1 += string.Format("                    ORDER BY ITEM_SIZE ASC   ");
            sql1 += string.Format("                  ) X  ");

            olddt = cd.FindDataTable(sql1);

            moddt = olddt.Copy();
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;
        }

        private void grdMain_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 숫자만 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_decimal(sender, e);
        }

        private void flexTextEditor_SelectedValueChanged(object sender, EventArgs e)
        {
            //_i++;
            grdMain.SetData(grdMain.Row, grdMain.Col, sender.ToString());
            
        }

        private void grdMain_RowColChange(object sender, EventArgs e)
        {

        }

        private void grdMain_CellChanged(object sender, RowColEventArgs e)
        {

            C1FlexGrid grd = sender as C1FlexGrid;


            if (string.IsNullOrEmpty(grd.GetData(e.Row, "ITEM").ToString()) ||
                string.IsNullOrEmpty(grd.GetData(e.Row, "ITEM_SIZE").ToString()) ||
                string.IsNullOrEmpty(grd.GetData(e.Row, "LENGTH").ToString()) ||
                string.IsNullOrEmpty(grd.GetData(e.Row, "BND_PCS").ToString()) 
                )
            {
                return;
            }

            string msg = string.Empty;
            string _check_item = string.Empty;

            if (e.Col == grd.Cols["ITEM"].Index ||
                e.Col == grd.Cols["ITEM_SIZE"].Index ||
                e.Col == grd.Cols["LENGTH"].Index ||
                e.Col == grd.Cols["BND_PCS"].Index 
                )
            {

                #region ITEM 체크
                _check_item = "ITEM";
                if (string.IsNullOrEmpty(grd.GetData(e.Row, _check_item).ToString()))
                {
                    return;
                }

                #endregion

                #region ITEM_SIZE 체크
                _check_item = "ITEM_SIZE";
                if (string.IsNullOrEmpty(grd.GetData(e.Row, _check_item).ToString()))
                {
                    return;
                }
                #endregion

                #region LENGTH 체크
                _check_item = "LENGTH";
                if (string.IsNullOrEmpty(grd.GetData(e.Row, _check_item).ToString()))
                {
                    return;
                }
                #endregion

                #region BND_PCS 체크
                _check_item = "BND_PCS";
                if (string.IsNullOrEmpty(grd.GetData(e.Row, _check_item).ToString()))
                {
                    return;
                }
                #endregion

                //결속 본수가 수정되면 해당본수에 따른 이론중량을 계산해 입력한다.
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

                    string sql1 = string.Format("SELECT DISTINCT ");
                    sql1 += string.Format("FN_GET_WGT ('{0}','{1}','{2}','{3}') AS BND_WGT ", grd.GetData(e.Row, "ITEM"), grd.GetData(e.Row, "ITEM_SIZE"), grd.GetData(e.Row, "LENGTH"), grd.GetData(e.Row, "BND_PCS"));
                    sql1 += string.Format("from TB_BUNDLE_PIECESTD1 ");

                    dtWgt = cd.FindDataTable(sql1);
                    grd.SetData(e.Row, "BND_WGT", dtWgt.Rows[0]["BND_WGT"]);
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

            }
        }
    }
}
