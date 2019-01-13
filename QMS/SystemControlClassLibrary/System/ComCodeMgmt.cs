using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SystemControlClassLibrary
{

    public partial class ComCodeMgmt : Form
    {
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        clsUtil cu = new clsUtil();


        DataTable olddt;
        DataTable moddt;

        DataTable olddt_sub;
        DataTable moddt_sub;

        DataTable grdMainDT;
        DataTable grdSubDT;

        TextBox tbCategory;
        TextBox tbCD_ID;

        TextBox tbCOLUMN1;


        List<string> msg;
        List<string> modifList;
        bool _CanSaveSearchLog = false;


        clsStyle cs = new clsStyle();

        string selected_Category = "";
        
        // 셀의 수정전 값
        string strBefValue = "";
        string strBefValue2 = "";

        string ownerNM = "";
        string titleNM = "";

        static C1FlexGrid selectedGrd;


        public ComCodeMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {


            ownerNM = ownerNm;
            titleNM = titleNm;
            InitializeComponent();

            Load += ComCodeMgmt_Load;


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

        private void Button_Click(object sender, EventArgs e)
        {

            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    if (_CanSaveSearchLog)
                    {
                        cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    }
                    SetDataBinding();  // 조회 버튼을 통한 데이터입력

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

        private void GrdSub_RowColChange(object sender, EventArgs e)
        {

        }


        private void InitControl()
        {

            tableLayoutPanel1.RowStyles[0].Height = 35F;

            clsStyle.Style.InitPicture(pictureBox1);
            clsStyle.Style.InitTitle(title_lb ,  ownerNM, titleNM);
            clsStyle.Style.InitPanel(panel1);


            // Button Color Set
            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnSave);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnRowAdd1);
            clsStyle.Style.InitButton(btnRowAdd2);
            clsStyle.Style.InitButton(btnDelRow1);
            clsStyle.Style.InitButton(btnDelRow2);
            clsStyle.Style.InitButton(btnClose);


            InitGrd_Main();
            InitGrdSub();
        }

        
        private void InitGrd_Main()
        {


            clsStyle.Style.InitGrid_search(grdMain);
            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["CATEGORY_NM"].Index, 0, grdMain.Cols["L3_CATEGORY"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            
            grdMain.Cols["NUM"].Width = cs.L_No_Width;
            grdMain.Cols["CATEGORY"].Width = cs.CATEGORY_Width;
            grdMain.Cols["CATEGORY_NM"].Width = 160;
            grdMain.Cols["L3_CATEGORY"].Width = cs.L3_Grp_cd_Width;
            grdMain.Cols["GUBUN"].Width = 0;

            #region 1. grdMain head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.


            grdMain.Cols[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // No col은 1 이고 왼쪽 가운데 정렬로 한다.
            grdMain.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 권한 코드  왼족 가운데 정렬
            grdMain.Cols[2].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 그룹명 왼쪽 가운데 정렬
            grdMain.Cols[3].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // L3 그룹코드 왼쪽 가운데 정렬
            #endregion

            #region 카테고리 입력설정

            tbCategory = new TextBox();
            cs.InitTextBox(tbCategory);

            tbCategory.MaxLength = 30;
            tbCategory.CharacterCasing = CharacterCasing.Upper;

            grdMain.Cols["CATEGORY"].Editor = tbCategory;

            #endregion


        }

        private void InitGrdSub()
        {
            clsStyle.Style.InitGrid_search(grdSub);

            var crCellRange = grdSub.GetCellRange(0, grdSub.Cols["CD_NM"].Index, 0, grdSub.Cols["L3_CD_ID"].Index);
            crCellRange.Style = grdSub.Styles["ModifyStyle"];

            grdSub.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            grdSub.Cols["NUM"].Width = cs.L_No_Width;
            grdSub.Cols["CD_ID"].Width = 100;       // No
            grdSub.Cols["CD_NM"].Width = 200;
            grdSub.Cols["COLUMNA"].Width = 100;
            grdSub.Cols["COLUMNB"].Width = 100;
            grdSub.Cols["COLUMNC"].Width = 100;
            grdSub.Cols["COLUMNE"].Width = 100;
            grdSub.Cols["COLUMN1"].Width = 100;
            grdSub.Cols["SORT_SEQ"].Width = 80;
            grdSub.Cols["USE_YN"].Width = 80;
            grdSub.Cols["L3_CD_ID"].Width = 100;
            grdSub.Cols["GUBUN"].Width = 0;

            #region 2. grdSub head 및 row  align 설정
            grdSub.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdSub.Cols["NUM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // No col은 1 이고 왼쪽 가운데 정렬로 한다.
            grdSub.Cols["CD_ID"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 코드명  
            grdSub.Cols["CD_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 그룹명 
            grdSub.Cols["COLUMNA"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 그룹명2
            grdSub.Cols["COLUMNB"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 그룹명3
            grdSub.Cols["COLUMNC"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 그룹명4
            grdSub.Cols["COLUMNE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 그룹명4
            grdSub.Cols["COLUMN1"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;     // 1
            grdSub.Cols["SORT_SEQ"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 정렬순서 
            grdSub.Cols["USE_YN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;       // 사용여부
            grdSub.Cols["L3_CD_ID"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;       // L3 코드
            grdSub.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;        // 구분
            #endregion

            #region CD_ID 입력설정

            tbCD_ID = new TextBox();
            cs.InitTextBox(tbCD_ID);

            tbCD_ID.MaxLength = 20;
            tbCD_ID.CharacterCasing = CharacterCasing.Upper;

            grdSub.Cols["CD_ID"].Editor = tbCD_ID;

            #endregion

            tbCOLUMN1 = new TextBox();
            cs.InitTextBox(tbCOLUMN1);

            tbCOLUMN1.MaxLength = 8;
            tbCOLUMN1.KeyPress += vf.KeyPressEvent_decimal;
            tbCOLUMN1.TextAlign = HorizontalAlignment.Right;
            grdSub.Cols["COLUMN1"].Editor = tbCOLUMN1;

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

            if (e.Row <= 0)
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 CATEGORY ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["CATEGORY"].Index && grd.GetData(e.Row, "NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(e.Row, e.Col).ToString();
        }

        private void GrdMain_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {

            // No,구분은 수정 불가
            if (grdMain.Col == 0 || grdMain.Col == grdMain.Cols.Count-1)
            {
                grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdMain.GetData(grdMain.Row, grdMain.Col).ToString())
                return;

            //   
            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdMain.GetData(grdMain.Row, grdMain.Cols.Count - 1).ToString() != "추가")
            {
                // 화면ID 수정 불가
                if (grdMain.Col == 1)
                {
                    grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                grdMain.SetData(grdMain.Row, 0, "수정");

                // Update 배경색 지정
                grdMain.Rows[grdMain.Row].Style = grdMain.Styles["UpColor"];
            }

            
        }

        private void GrdSub_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (e.Row <= 0)
            {
                return;
            }

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 CD ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["CD_ID"].Index && grd.GetData(e.Row, "NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            strBefValue2 = grd.GetData(e.Row, e.Col).ToString();
        }

        private void GrdSub_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {

            // No,구분은 수정 불가
            if (grdSub.Col == 0 || grdSub.Col == grdSub.Cols.Count-1)
            {
                grdSub.SetData(grdSub.Row, grdSub.Col, strBefValue2);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue2 == grdSub.GetData(grdSub.Row, grdSub.Col).ToString())
                return;

            //   
            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdSub.GetData(grdSub.Row, grdSub.Cols.Count-1).ToString() != "추가")
            {
                // 코드 수정 불가
                if (grdSub.Col == 1)
                {
                    grdSub.SetData(grdSub.Row, grdSub.Col, strBefValue2);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdSub.SetData(grdSub.Row, grdSub.Cols.Count-1, "수정");
                grdSub.SetData(grdSub.Row, 0, "수정");

                // Update 배경색 지정
                grdSub.Rows[grdSub.Row].Style = grdSub.Styles["UpColor"];
            }

        }
        private void GrdMain_RowColChange(object sender, EventArgs e)
        {

        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, selectedGrd);
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

            bool isChange = false;
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, "GUBUN").ToString();

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;

                    // 수정일 경우 명 확인...""
                }


                if (gubun_value == "추가")
                {
                    check_field_NM = "CATEGORY";
                    check_table_NM = "TB_CM_COM_CD_GRP";
                    check_value = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

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
                    check_field_NM = "CATEGORY_NM";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }
                    isChange = true;
                }
                
            }

            for (checkrow = 1; checkrow < grdSub.Rows.Count; checkrow++)
            {

                gubun_value = grdSub.GetData(checkrow, "GUBUN").ToString();


                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }

                if (gubun_value == "추가")
                {
                    check_keyColNM = "CATEGORY";
                    check_keyValue = selected_Category;

                    check_field_NM = "CD_ID";
                    check_table_NM = "TB_CM_COM_CD";
                    check_value = grdSub.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdSub.Cols[check_field_NM].Caption;

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

                    if (vf.getStrLen(check_value) > 22)
                    {
                        MessageBox.Show("영문 및 숫자 22자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value, check_keyColNM, check_keyValue))
                    {
                        show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "CD_NM";
                    check_NM = grdSub.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdSub.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
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
            #endregion 데이터 체크

            string sql1 = string.Empty;
            string strMsg = string.Empty;

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

                #region grdMain 추가,수정,삭제 처리
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    // Insert 처리
                    if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "추가")
                    {
                        sql1  = string.Format(" INSERT INTO TB_CM_COM_CD_GRP ");
                        sql1 += string.Format("             ( ");
                        sql1 += string.Format("                CATEGORY ");
                        sql1 += string.Format("               ,CATEGORY_NM ");
                        sql1 += string.Format("               ,L3_CATEGORY ");
                        sql1 += string.Format("               ,REGISTER ");
                        sql1 += string.Format("               ,REG_DDTT ");
                        sql1 += string.Format("             ) ");
                        sql1 += string.Format(" VALUES      ( ");
                        sql1 += string.Format("               '{0}' ", grdMain.GetData(row, "CATEGORY"));
                        sql1 += string.Format("              ,'{0}' ", grdMain.GetData(row, "CATEGORY_NM"));
                        sql1 += string.Format("              ,'{0}' ", grdMain.GetData(row, "L3_CATEGORY"));
                        sql1 += string.Format("              ,'{0}' ", ck.UserID);
                        sql1 += string.Format("              ,SYSDATE ");
                        sql1 += string.Format("             ) ");


                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        
                        InsCnt++;

                    }
                    // Update 처리
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "수정")
                    {
                        sql1  = string.Format(" UPDATE TB_CM_COM_CD_GRP ");
                        sql1 += string.Format(" SET    CATEGORY_NM  = '{0}' ", grdMain.GetData(row, "CATEGORY_NM"));
                        sql1 += string.Format("       ,L3_CATEGORY  = '{0}' ", grdMain.GetData(row, "L3_CATEGORY"));
                        sql1 += string.Format("       ,MODIFIER     = '{0}' ", ck.UserID);
                        sql1 += string.Format("       ,MOD_DDTT     = SYSDATE ");
                        sql1 += string.Format(" WHERE CATEGORY      = '{0}' ", grdMain.GetData(row, "CATEGORY"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        strMsg = " 그룹: " + grdMain.GetData(row, "CATEGORY").ToString() + " 그룹명: " + olddt.Rows[row - 1]["CATEGORY_NM"].ToString() + " To " + grdMain.GetData(row, "CATEGORY_NM").ToString() + "로 수정 ";

                        UpCnt++;
                    }
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    {
                        // category 항목에 따른 데이터가 있는경우 같이 없애준다.

                        delSublist = new List<string>();
                        sql1 = string.Format("SELECT CD_ID FROM TB_CM_COM_CD WHERE CATEGORY = '{0}'", grdMain.GetData(row, "CATEGORY"));
                        dt = cd.FindDataTable(sql1);

                        foreach (DataRow row_data in dt.Rows) // Loop over the rows.
                        {
                            delSublist.Add(row_data.ItemArray[0].ToString());
                            sql1 = string.Format("DELETE FROM TB_CM_COM_CD WHERE CD_ID = '{0}' AND CATEGORY = '{1}'", row_data.ItemArray[0], grdMain.GetData(row, "CATEGORY"));
                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();
                        }

                        sql1 = string.Format("DELETE FROM TB_CM_COM_CD_GRP WHERE CATEGORY = '{0}'", grdMain.GetData(row, "CATEGORY"));

                        strMsg = " 그룹: " + grdMain.GetData(row, "CATEGORY").ToString() + "를 삭제 ";

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        DelCnt++;

                    }

                }
                #endregion

                #region gridSub 추가 수정 삭제 처리
                for (row = 1; row < grdSub.Rows.Count; row++)
                {
                    // Insert 처리 

                    if (grdSub.GetData(row, grdSub.Cols.Count - 1).ToString() == "추가")
                    {


                        sql1 =  string.Format(" INSERT INTO TB_CM_COM_CD  ");
                        sql1 += string.Format("             ( ");
                        sql1 += string.Format("               CATEGORY   ");
                        sql1 += string.Format("              ,CD_ID      ");
                        sql1 += string.Format("              ,CD_NM      ");
                        sql1 += string.Format("              ,COLUMNA    ");
                        sql1 += string.Format("              ,COLUMNB    ");
                        sql1 += string.Format("              ,COLUMNC    ");
                        sql1 += string.Format("              ,COLUMN1    ");
                        sql1 += string.Format("              ,SORT_SEQ   ");
                        sql1 += string.Format("              ,USE_YN     ");
                        sql1 += string.Format("              ,REGISTER   ");
                        sql1 += string.Format("              ,REG_DDTT   ");
                        sql1 += string.Format("              ,L3_CD_ID   ");
                        sql1 += string.Format("             )            ");
                        sql1 += string.Format(" VALUES( ");
                        sql1 += string.Format("         '{0}'   ", selected_Category);    //"CATEGORY,  " 0
                        sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "CD_ID")); //"CD_ID,     " 1
                        sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "CD_NM")); //"CD_NM,     " 2
                        sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMNA"));               //"COLUMNA,   " 3
                        sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMNB"));  //"COLUMNB,   " 4
                        sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMNC"));  //"COLUMNC,   " 5
                        sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "COLUMN1"));  //"COLUMNC,   " 5
                        sql1 += string.Format("        ,'{0}'   ", grdSub.GetData(row, "SORT_SEQ"));  //"SORT_SEQ,  " 6
                        sql1 += string.Format("        ,'{0}'   ", vf.StringToString(grdSub.GetData(row, "USE_YN").ToString()));  //"USE_YN,    " 7
                        sql1 += string.Format("        ,'{0}'   ", ck.UserID);  //"REGISTER,  " 15
                        sql1 += string.Format("        ,SYSDATE "    );  //"REG_DDTT,  " 16
                        sql1 += string.Format("        ,'{0}' ", grdSub.GetData(row, "L3_CD_ID"));  //"L3_CD_ID   " 20
                        sql1 += string.Format("       ) ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        InsCnt++;
                    }
                    // Update 처리
                    else if (grdSub.GetData(row, grdSub.Cols.Count - 1).ToString() == "수정")
                    {

                        sql1 =  string.Format(" UPDATE TB_CM_COM_CD SET  ");
                        sql1 += string.Format("         CD_NM = '{0}'       ", grdSub.GetData(row, "CD_NM"));
                        sql1 += string.Format("        ,COLUMNA = '{0}'     ", grdSub.GetData(row, "COLUMNA"));
                        sql1 += string.Format("        ,COLUMNB = '{0}'     ", grdSub.GetData(row, "COLUMNB"));
                        sql1 += string.Format("        ,COLUMNC = '{0}'     ", grdSub.GetData(row, "COLUMNC"));
                        sql1 += string.Format("        ,COLUMN1 = '{0}'     ", grdSub.GetData(row, "COLUMN1"));
                        sql1 += string.Format("        ,SORT_SEQ = '{0}'    ", grdSub.GetData(row, "SORT_SEQ"));
                        sql1 += string.Format("        ,USE_YN = '{0}'      ", vf.StringToString(grdSub.GetData(row, "USE_YN").ToString()));//vf.BoolToString((Boolean)grdSub.GetData(row, "USE_YN")));
                        sql1 += string.Format("        ,MODIFIER = '{0}'    ", ck.UserID);
                        sql1 += string.Format("        ,MOD_DDTT = SYSDATE  ");
                        sql1 += string.Format("        ,L3_CD_ID = '{0}'  ", grdSub.GetData(row, "L3_CD_ID"));
                        sql1 += string.Format(" WHERE CATEGORY = '{0}' ", selected_Category);
                        sql1 += string.Format(" and   CD_ID    = '{0}' ", grdSub.GetData(row, "CD_ID"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        modifList = new List<string>();

                        modifList.Add("CD_NM");
                        modifList.Add("COLUMNA");
                        modifList.Add("COLUMNB");
                        modifList.Add("COLUMNC");
                        modifList.Add("SORT_SEQ");
                        modifList.Add("USE_YN");
                        modifList.Add("L3_CD_ID");

                        foreach (string item in modifList)
                        {
                            if (olddt_sub.Rows[row - 1][item].ToString() != grdSub.GetData(row, item).ToString())
                            {

                            }
                        }


                        UpCnt++;

                    }
                    else if (grdSub.GetData(row, grdSub.Cols.Count - 1).ToString() == "삭제")
                    {

                        sql1 = string.Format("DELETE FROM TB_CM_COM_CD WHERE CD_ID = '{0}' AND CATEGORY = '{1}'", grdSub.GetData(row, "CD_ID"), selected_Category);

                        cmd.CommandText = sql1;
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

            // 수정가능 하도록 열추가
            grdMain.AllowEditing = true;
            grdMain.Rows.Add();

            // 추가열 데이타 초기화
            for (int i = 1; i < grdMain.Cols.Count; i++)
                grdMain.SetData(grdMain.Rows.Count - 1, i, "");

            // 1행에 Count 자동 입력
            grdMain.SetData(grdMain.Rows.Count - 1, 0, (grdMain.Rows.Count - 1).ToString());

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, grdMain.Cols.Count - 1, "추가");
            grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");

            // Insert 배경색 지정
            grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];

        }

        private void DataRowDel()
        {

            //mj 추가되었지만 행삭제로 지울때
            if (grdMain.Rows[grdMain.Row][grdMain.Cols.Count-1].ToString() == "추가")
            {
                grdMain.RemoveItem(grdMain.Row);

                return;
            }

            grdMain.Rows[grdMain.Row][grdMain.Cols.Count - 1] = "삭제";

            grdMain.Rows[grdMain.Row][0] = "삭제";

            // 메인 의 삭제 flag를 서브 그리드에 적용
            grdSub_update();

            // Delete 배경색 지정
            grdMain.Rows[grdMain.Row].Style = grdMain.Styles["DelColor"];

            // 커서위치 결정
            grdMain.Row = 0;
            grdMain.Col = 0;
        }

        private void grdSub_update()
        {

            if (grdMain.Rows[grdMain.Row][grdMain.Cols.Count-1].ToString() == "삭제")
            {
                //main category에 따른 하부 sub grid 항목들도 삭제할수있게 설정한다.
                // 다른 항목을 선택했을시에도 이하의 프로세스를 거쳐야한다.
                //start
                for (int sub_row = 1; sub_row < grdSub.Rows.Count; sub_row++)
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

            // 추가열 데이타 초기화
            for (int col = 1; col < grdSub.Cols.Count-1; col++)
                grdSub.SetData(grdSub.Rows.Count - 1, col, "");

            // 1행에 Count 자동 입력
            grdSub.SetData(grdSub.Rows.Count - 1, 0, (grdSub.Rows.Count - 1).ToString());
            grdSub.SetData(grdSub.Rows.Count - 1, "USE_YN", true);


            // 저장시 Insert로 처리하기 위해 flag set
            grdSub.SetData(grdSub.Rows.Count - 1, grdSub.Cols.Count-1, "추가");
            grdSub.SetData(grdSub.Rows.Count - 1, 0, "추가");

            // Insert 배경색 지정
            grdSub.Rows[grdSub.Rows.Count - 1].Style = grdSub.Styles["InsColor"];
 
        }

        private void DataRowDel2(int deleteRow)
        {


            // 저장시 Delete로 처리하기 위해 flag set
            grdSub.Rows[deleteRow][grdSub.Cols.Count-1] = "삭제";
            grdSub.Rows[deleteRow][0] = "삭제";

            // Delete 배경색 지정
            grdSub.Rows[deleteRow].Style = grdSub.Styles["DelColor"];

            grdSub.Row = 0;
            grdSub.Col = 0;
        }


        private void ComCodeMgmt_Load(object sender, System.EventArgs e)
        {
            msg = new List<string>();

            this.BackColor = Color.White;

            tableLayoutPanel1.RowStyles[0].Height = 38F;
            tableLayoutPanel1.RowStyles[1].Height = 50F;
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;

            InitControl();


            MakeInitGridData();
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

        //public static string __File()
        //{
        //    var st = new StackTrace(new StackFrame(true));
        //    var sf = st.GetFrame(0);
        //    return sf.GetFileName();
        //}

        private void SetDataBinding()
        {
            SetDataBinding_Grd_byinitData();

            SetDataBinding_grdMain();
        }

        private void SetDataBinding_Grd_byinitData()
        {
            InitgrdMainData();

            InitgrdSubData();
        }

        private void InitgrdSubData()
        {
            grdSub.SetDataBinding(grdSubDT, null, true);

            grdSub.Row = -1;
        }

        private void InitgrdMainData()
        {
            grdMain.SetDataBinding(grdMainDT, null, true);

            grdMain.Row = -1;
        }

        private void SetDataBinding_grdMain()
        {
            string sql1 = string.Empty;
            try
            {
                sql1 += string.Format("select ");
                sql1 += string.Format("       TO_CHAR(rownum) as NUM ");
                sql1 += string.Format("      ,CATEGORY, CATEGORY_NM ");
                sql1 += string.Format("      ,L3_CATEGORY ");
                sql1 += string.Format("      ,'' AS GUBUN ");
                sql1 += string.Format("FROM  TB_CM_COM_CD_GRP ");
                sql1 += string.Format("where   CATEGORY    like '%{0}%' ", txtCategory.Text);
                sql1 += string.Format("and     CATEGORY_NM like '%{0}%' ", txtCategory_nm.Text);
                sql1 += string.Format("ORDER BY CATEGORY ASC ");

                olddt = cd.FindDataTable(sql1);
                
                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString() + "건이 조회 되었습니다.");

                if (moddt.Rows.Count > 0)
                {
                    grdMain.Row = 1;
                    grdMain_Row_Selected(grdMain.Row);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return ;
            }

            return ;
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            selectedGrd = grdMain;

            if (grdMain.Rows.Count <= 1)
            { return; }

            grdMain_Row_Selected(grdMain.RowSel);

            if (grdMain.GetData(grdMain.RowSel, "GUBUN").ToString() == "삭제")
            {
                DataRowDel();
            }

        }

        private void grdMain_Row_Selected(int selectedRow)
        {
            if (selectedRow < 1 )
            {
                return;
            }

            selected_Category = grdMain.GetData(selectedRow, "CATEGORY").ToString();

                string sql1 = string.Empty;
                sql1 += string.Format("SELECT TO_CHAR(ROWNUM) AS NUM, ");
                sql1 += string.Format("  N.* ");
                sql1 += string.Format("FROM( ");
                sql1 += string.Format("    SELECT CD_ID ");
                sql1 += string.Format("    ,CD_NM ");
                sql1 += string.Format("    ,COLUMNA ");
                sql1 += string.Format("    ,COLUMNB ");
                sql1 += string.Format("    ,COLUMNC ");
                sql1 += string.Format("    ,COLUMNE ");
                sql1 += string.Format("    ,COLUMN1 ");
                sql1 += string.Format("    ,SORT_SEQ ");
                sql1 += string.Format("    ,(CASE WHEN USE_YN = 'Y' THEN 'True' ELSE 'False' END) AS USE_YN ");
                sql1 += string.Format("    ,L3_CD_ID ");
                sql1 += string.Format("    ,'' AS GUBUN ");
                sql1 += string.Format("    FROM TB_CM_COM_CD ");
                sql1 += string.Format("    WHERE CATEGORY = '{0}' ", selected_Category);
                sql1 += string.Format("    ORDER BY SORT_SEQ) N ");

                olddt_sub = cd.FindDataTable(sql1);

                moddt_sub = olddt_sub.Copy();

                Cursor = Cursors.AppStarting;
                grdSub.SetDataBinding(moddt_sub, null, true);
                Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), olddt_sub.Rows.Count.ToString() + "건이 조회 되었습니다.");
            

        }

        private void btnRowAdd2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitGrd_Main();
        }

        private void grdMain_Click_1(object sender, EventArgs e)
        {

        }

        private void grpCode_lb_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRowAdd1_Click(object sender, EventArgs e)
        {

        }

        private void grdSub_Click(object sender, EventArgs e)
        {
            selectedGrd = grdSub;
        }

        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                Button_Click(btnDisplay, null);
                //SetDataBinding();  // 조회 버튼을 통한 데이터입력
            }
        }

        private void txtCategory_nm_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                Button_Click(btnDisplay, null);
                //SetDataBinding();  // 조회 버튼을 통한 데이터입력
            }
        }
    }
}
