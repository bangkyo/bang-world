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
    public partial class AuthMgmt : Form
    {
        ConnectDB cd = new ConnectDB();
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        DataTable grdMain_dt;
        DataTable grdSub_dt;

        DataTable olddt;
        DataTable moddt;


        DataTable olddt_sub;
        DataTable moddt_sub;

        TextBox tbACL_ID;

        private  string strBefValue_Main = "";

        private static C1FlexGrid selectedGrd;

        clsStyle cs = new clsStyle();

        private  string selected_grp_id = "";

        private  string ownerNM = "";
        private  string titleNM = "";
        bool _CanSaveSearchLog = false;

        #region 변수선언
        #endregion

        #region 화면
        public AuthMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();            
        }

        //폼 로드
        private void AuthMgmt_Load(object sender, System.EventArgs e)
        {
            this.BackColor = Color.White;

            // 각족 컬트롤 설정
            InitControl();

            MakeGrdData();
            _CanSaveSearchLog = true;
            Button_Click(btnDisplay, null);

        }

        private void InitGridData()
        {
            InitGridData_Main();

            InitGridData_Sub();
        }

        private void InitGridData_Main()
        {
            grdMain.SetDataBinding(grdMain_dt, null, true);
        }

        private void InitGridData_Sub()
        {
            grdSub.SetDataBinding(grdSub_dt, null, true);
        }

        private void MakeGrdData()
        {
            grdMain_dt = vf.CreateDataTable(grdMain);

            grdSub_dt = vf.CreateDataTable(grdSub);
        }
        #endregion

        #region 메인 그리드
        //메인그리드 클릭 -> 상세그리드 자료 표시
        private void GrdMain_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count <= 1 || grdMain.RowSel < 0) { return; }

            grdMain_Row_Selected(grdMain.Row);
        }

        private void GrdAuth_DoubleClick(object sender, EventArgs e)
        {
            grdMain.AllowEditing = true;
        }

        private void GrdAuth_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            // NO COLUMN 수정불가하게..
            if (e.Col == grd.Cols["NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
                return;
            }

            //추가된 행이 아니면 메뉴 ID 열은  수정되지않게 한다
            if (e.Col == grd.Cols["ACL_GRP_ID"].Index && grd.GetData(e.Row, "NUM").ToString() != "추가")
            {
                e.Cancel = true;
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue_Main = grd.GetData(e.Row, e.Col).ToString();
        }

        private void GrdAuth_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            // No,구분은 수정 불가
            if (e.Col == grd.Cols["NUM"].Index || e.Col == grd.Cols["GUBUN"].Index)
            {
                grd.SetData(e.Row, e.Col, strBefValue_Main);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue_Main == grd.GetData(e.Row, e.Col).ToString())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grd.GetData(e.Row, grd.Cols["GUBUN"].Index).ToString() != "추가")
            {
                // 권한그룹ID 수정 불가
                if (e.Col == grd.Cols["ACL_GRP_ID"].Index)
                {
                    grd.SetData(e.Row, e.Col, strBefValue_Main);
                    return;
                }

                // 저장시 UPDATE로 처리하기 위해 flag set
                grd.SetData(e.Row, grd.Cols["GUBUN"].Index, "수정");
                grd.SetData(e.Row, grd.Cols["NUM"].Index, "수정");

                //

                // Update 배경색 지정
                grd.Rows[grd.RowSel].Style = grd.Styles["UpColor"];
            }
            else
            {
                if (e.Col == grd.Cols["ACL_GRP_ID"].Index)
                {
                    grd.SetData(e.Row, e.Col, vf.UCase(grd.GetData(e.Row, "ACL_GRP_ID").ToString()));
                }
            }

        }
        #endregion

        #region 상세 그리드
        private void GrdSub_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdSub.GetData(e.Row, "GUBUN").ToString() != "추가")
            {
                // 저장시 UPDATE로 처리하기 위해 flag set
                grdSub.SetData(e.Row, "GUBUN", "수정");
                grdSub.SetData(e.Row, "NUM", "수정");

                // Update 배경색 지정
                grdSub.Rows[grdSub.RowSel].Style = grdSub.Styles["UpColor"];
            }

        }

        //셀 체크박스 클릭(전체 체크 -> 조회/등록/수정/삭제 자동 체크)
        private void GrdSub_CellChecked(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col != grdSub.Cols["TOTAL"].Index) return;

            for (int iCol = grdSub.Cols["INQ_ACL"].Index; iCol <= grdSub.Cols["DEL_ACL"].Index; iCol++)
            {
                grdSub.Rows[e.Row][iCol] = grdSub.Rows[e.Row]["TOTAL"];
            }
        }
        #endregion

        #region 초기화
        private void InitControl()
        {
            tableLayoutPanel1.RowStyles[0].Height = 35F;
            tableLayoutPanel1.RowStyles[1].Height = 50F;

            clsStyle.Style.InitPicture(pictureBox1);
            clsStyle.Style.InitTitle(lblAuthTitle,  ownerNM, titleNM);
            clsStyle.Style.InitPanel(panel1);

            // Button Color Set
            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnSave);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnRowAdd);
            clsStyle.Style.InitButton(btnDelRow);
            clsStyle.Style.InitButton(btnClose);

            // 그리드 초기화
            InitGrid();
        }

        private void InitGrid()
        {
            InitgrdMain();

            InitgrdSub();
        }

        private void InitgrdSub()
        {
            clsStyle.Style.InitGrid_search(grdSub);

            var crCellRange = grdSub.GetCellRange(0, grdSub.Cols["TOTAL"].Index, 0, grdSub.Cols["DEL_ACL"].Index);
            crCellRange.Style = grdSub.Styles["ModifyStyle"];
            #region 4. grdSub cols 수정가능 설정
            grdSub.AllowEditing = true; // grid 설정에서 수정 불가능하게 해놓아서 전체를 수정가능하게 해야 수정가능으로 설정가능...
            grdSub.Rows[0].AllowEditing = false;   // header 부분 수정불가

            grdSub.Cols["NUM"].AllowEditing = false;   // No col 은 수정불가
            grdSub.Cols["BIZ_GP"].AllowEditing = false;   // 업무구분 col 은 수정불가
            grdSub.Cols["SCR_NM"].AllowEditing = false;   // 화면명 col 은 수정불가

            grdSub.Cols["TOTAL"].AllowEditing = true;    // 전체 col 은 수정가능
            grdSub.Cols["INQ_ACL"].AllowEditing = true;    // 조회 col 은 수정가능
            grdSub.Cols["REG_ACL"].AllowEditing = true;    // 등록 col 은 수정가능
            grdSub.Cols["MOD_ACL"].AllowEditing = true;    // 수정 col 은 수정가능
            grdSub.Cols["DEL_ACL"].AllowEditing = true;    // 삭제 col 은 수정가능

            grdSub.Cols["GUBUN"].AllowEditing = false;   // 구분 col 은 수정불가
            #endregion

            #region 5. grdSub cols size 설정
            grdSub.Cols["NUM"].Width = cs.L_No_Width;   // 보이지않는 No..

            grdSub.Cols["BIZ_GP"].Width = 80; // 업무구분
            grdSub.Cols["SCR_NM"].Width = 200; // 화면명
            grdSub.Cols["TOTAL"].Width = 50;  // 전체
            grdSub.Cols["INQ_ACL"].Width = 50;  // 조회
            grdSub.Cols["REG_ACL"].Width = 50;  // 등록
            grdSub.Cols["MOD_ACL"].Width = 50;  // 수정
            grdSub.Cols["DEL_ACL"].Width = 50;  // 삭제
            grdSub.Cols["GUBUN"].Width = 0;   // 구분
            #endregion

            #region 6. grdSub head 및 row align 설정
            grdSub.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdSub.Cols["NUM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // No col은 1 이고 왼쪽 가운데 정렬로 한다.
            grdSub.Cols["BIZ_GP"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 업무구분 ID 왼족 가운데 정렬
            grdSub.Cols["SCR_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 화면명 왼쪽 가운데 정렬

            grdSub.Cols["TOTAL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 전체 왼쪽 가운데 정렬
            grdSub.Cols["INQ_ACL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 조회 왼쪽 가운데 정렬
            grdSub.Cols["REG_ACL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 등록 왼쪽 가운데 정렬
            grdSub.Cols["MOD_ACL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 수정 왼쪽 가운데 정렬
            grdSub.Cols["DEL_ACL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // 삭제 왼쪽 가운데 정렬
            #endregion

        }

        private void InitgrdMain()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["ACL_GRP_NM"].Index,0, grdMain.Cols["REMARKS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            #region 1. grdMain  cols 수정가능 설정
            grdMain.AllowEditing = true; // grid 설정에서 수정 불가능하게 해놓아서 전체를 수정가능하게 해야 수정가능으로 설정가능...
            grdMain.Rows[0].AllowEditing = false;   // header 부분 수정불가

            grdMain.Cols["NUM"].AllowEditing = false;   // No col 은 수정불가
            grdMain.Cols["GUBUN"].AllowEditing = false;   // 구분 col 은 수정불가

            #endregion

            #region 2. grdMain cols size 설정
            grdMain.Cols["NUM"].Width = cs.L_No_Width;   // 보이지않는 No..
            grdMain.Cols["ACL_GRP_ID"].Width = 200;  // No.
            grdMain.Cols["ACL_GRP_NM"].Width = 200;
            grdMain.Cols["REMARKS"].Width = 100;
            grdMain.Cols["GUBUN"].Width = 0;
            #endregion

            #region 3. grdMain head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["NUM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;     // No col은 1 이고 왼쪽 가운데 정렬로 한다.
            grdMain.Cols["ACL_GRP_ID"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 권한 그룹 ID 왼족 가운데 정렬
            grdMain.Cols["ACL_GRP_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 비고 왼쪽 가운데 정렬
            grdMain.Cols["REMARKS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;     // 비고 왼쪽 가운데 정렬
            #endregion

            #region Editor 설정
            tbACL_ID = new TextBox();
            cs.InitTextBox(tbACL_ID);
            
            tbACL_ID.MaxLength = 20;
            tbACL_ID.CharacterCasing = CharacterCasing.Upper;
            
            grdMain.Cols["ACL_GRP_ID"].Editor = tbACL_ID;

            #endregion

        }

        #endregion

        #region 버튼 클릭
        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    if (_CanSaveSearchLog)
                    {
                        cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    }

                    SetDataBinding();   // 조회버튼
                    break;

                case "btnRowAdd":
                    DataRowAdd();
                    break;

                case "btnDelRow":
                    DataRowDel();
                    break;

                case "btnSave":
                    SaveData();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }
        #endregion

        #region 버튼 처리(조회/추가/삭제/엑셀)
        //메인그리드 자료 선택
        private bool SetDataBinding()
        {


            InitGridData();

            try
            {
                string strQry = string.Empty;
                strQry += string.Format("SELECT TO_CHAR(rownum) as NUM ");
                strQry += string.Format("      ,ACL_GRP_ID ");
                strQry += string.Format("      ,ACL_GRP_NM ");
                strQry += string.Format("      ,REMARKS ");
                strQry += string.Format("      ,'' AS GUBUN ");
                strQry += string.Format("FROM  TB_CM_ACL_GRP       ");
                strQry += string.Format("WHERE  ");
                strQry += string.Format("    ACL_GRP_NM LIKE nvl2('{0}','%{0}%', '%')  ", txtAcl_Grp_NM.Text);
                strQry += string.Format("ORDER BY ACL_GRP_ID DESC ");

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

            if (grdMain.Rows.Count > 1)
            {
                grdMain_Row_Selected(1);
            }

            return true;
        }

        //행추가
        private void DataRowAdd()
        {
            // 수정가능 하도록 열추가
            grdMain.AllowEditing = true;
            grdMain.Rows.Add();

            // 추가열 데이타 초기화
            for (int i = 0; i < grdMain.Cols["GUBUN"].Index; i++)
                grdMain.SetData(grdMain.Rows.Count - 1, i, "");

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, "GUBUN", "추가");
            grdMain.SetData(grdMain.Rows.Count - 1, "NUM", "추가");

            // Insert 배경색 지정
            grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];

            grdMain.Row = -1;
        }

        //행삭제
        private void DataRowDel()
        {
            if (grdMain.Rows.Count < 2 || grdMain.Row < 1)
            {
                return;
            }
            //구분 col 8
            //mj 추가되었지만 행삭제로 지울때
            if (grdMain.Rows[grdMain.RowSel]["GUBUN"].ToString() == "추가")
            {
                grdMain.RemoveItem(grdMain.RowSel);
                return;
            }

            // 저장시 Delete로 처리하기 위해 flag set
            grdMain.Rows[grdMain.RowSel]["GUBUN"] = "삭제";
            grdMain.Rows[grdMain.RowSel]["NUM"] = "삭제";

            // Delete 배경색 지정
            grdMain.Rows[grdMain.RowSel].Style = grdMain.Styles["DelColor"];

            grdMain.Row = -1;
        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, selectedGrd);
        }
        #endregion

        #region 저장

        //메인그리드 저장 -> CM_권한그룹 
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

                if (gubun_value == "삭제" )
                {
                    isChange = true;

                    // 수정일 경우 명 확인...""
                }

                if (gubun_value == "수정")
                {
                    isChange = true;

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "ACL_GRP_NM";
                    //check_table_NM = "TB_CM_COM_CD_GRP";
                    check_NM = grdMain.GetData(checkrow, check_field_NM).ToString();
                    check_Cols_NM = grdMain.Cols[check_field_NM].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                }
                if (gubun_value == "추가")
                {
                    check_field_NM = "ACL_GRP_ID";
                    check_table_NM = "TB_CM_ACL_GRP";
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

                    if (vf.getStrLen(check_value) > 20)
                    {
                        MessageBox.Show("영문 및 숫자 20자 이하로 입력하시기 바랍니다..");
                        return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM, check_value))
                    {
                        show_msg = string.Format("{0}가 중복되었습니다. 다시 입력해주세요.", check_Cols_NM);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM = "ACL_GRP_NM";
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


                if (gubun_value == "수정")
                {
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


            //grdMain 관련 추가 수정 함수
            string sql1 = string.Empty;
            string strMsg = string.Empty;
            
            string index = "";
            string grp_mn = "";
            string screen_name = "";
            string can_search = "N";
            string can_modify = "N";
            string can_regist = "N";
            string can_remove = "N";
            string screen_id = "";

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

                // grdMain 관련부분
                for (row = 1; row < grdMain.Rows.Count; row++)
                {
                    #region Insert 처리
                    if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "추가")
                    {
                        sql1 = string.Format(" INSERT INTO TB_CM_ACL_GRP ");
                        sql1 += string.Format("             ( ");
                        sql1 += string.Format("              ACL_GRP_ID ");
                        sql1 += string.Format("             ,ACL_GRP_NM ");
                        sql1 += string.Format("             ,REMARKS ");
                        sql1 += string.Format("             ,REGISTER ");
                        sql1 += string.Format("             ,REG_DDTT ");
                        sql1 += string.Format("             ) ");
                        sql1 += string.Format(" VALUES( ");
                        sql1 += string.Format("          '{0}' ", grdMain.GetData(row, "ACL_GRP_ID"));
                        sql1 += string.Format("         ,'{0}' ", grdMain.GetData(row, "ACL_GRP_NM"));
                        sql1 += string.Format("         ,'{0}' ", grdMain.GetData(row, "REMARKS"));
                        sql1 += string.Format("         ,'{0}' ", ck.UserID);
                        sql1 += string.Format("         ,SYSDATE ");
                        sql1 += string.Format("       ) ");


                        //sql1 = string.Format("INSERT INTO TB_CM_ACL_GRP (ACL_GRP_ID, ACL_GRP_NM,  REMARKS) VALUES('{0}','{1}','{2}')",
                        //    grdMain.GetData(row, "ACL_GRP_ID"), grdMain.GetData(row, "ACL_GRP_NM"), grdMain.GetData(row, "REMARKS"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        InsCnt++;
                    }
                    #endregion

                    #region Update 처리
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "수정")
                    {
                        sql1 += string.Format(" UPDATE TB_CM_ACL_GRP ");
                        sql1 += string.Format(" SET    ACL_GRP_NM = '{0}' ", grdMain.GetData(row, "ACL_GRP_NM"));
                        sql1 += string.Format("       ,REMARKS    = '{0}' ", grdMain.GetData(row, "REMARKS"));
                        sql1 += string.Format("       ,MODIFIER   = '{0}' ", ck.UserID);
                        sql1 += string.Format("       ,MOD_DDTT   = SYSDATE ");
                        sql1 += string.Format(" WHERE ACL_GRP_ID  = '{0}' ", grdMain.GetData(row, "ACL_GRP_ID"));

                        //sql1 = string.Format("UPDATE TB_CM_ACL_GRP SET  ACL_GRP_NM = '{1}', REMARKS = '{2}' WHERE ACL_GRP_ID = '{0}'",
                        //    grdMain.GetData(row, "ACL_GRP_ID"), grdMain.GetData(row, "ACL_GRP_NM"), grdMain.GetData(row, "REMARKS"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        UpCnt++;
                    }
                    #endregion

                    #region delete 처리
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    {
                        // category 항목에 따른 데이터가 있는경우 같이 없애준다.

                        delSublist = new List<string>();
                        sql1 = string.Format(" SELECT SCR_ID FROM    TB_CM_ACL  WHERE ACL_GRP_ID = '{0}'", grdMain.GetData(row, "ACL_GRP_ID"));
                        DataTable dt = cd.FindDataTable(sql1);

                        foreach (DataRow row_data in dt.Rows) // Loop over the rows.
                        {
                            delSublist.Add(row_data["SCR_ID"].ToString());
                            sql1 = string.Format(" DELETE FROM TB_CM_ACL WHERE  SCR_ID = '{0}' AND ACL_GRP_ID = '{1}'", row_data["SCR_ID"].ToString(), grdMain.GetData(row, "ACL_GRP_ID").ToString());
                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();
                        }

                        //권한그룹 삭제
                        sql1 = string.Format(" DELETE FROM TB_CM_ACL_GRP WHERE ACL_GRP_ID = '{0}'", grdMain.GetData(row, "ACL_GRP_ID"));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        DelCnt++;

                    } 
                    #endregion
                }

                for (row = 1; row < grdSub.Rows.Count; row++)
                {
                    index = grdSub.GetData(row, "GUBUN").ToString();             // 구분

                    if (index != "")
                    {
                        grp_mn = grdSub.GetData(row, "BIZ_GP").ToString();                 // 업무구분
                        screen_name = grdSub.GetData(row, "SCR_NM").ToString();            // 화면 명
                        can_search = vf.StringToString(grdSub.GetData(row, "INQ_ACL").ToString());    // 조회 가능
                        can_regist = vf.StringToString(grdSub.GetData(row, "REG_ACL").ToString());    // 등록 가능
                        can_modify = vf.StringToString(grdSub.GetData(row, "MOD_ACL").ToString());    // 수정 가능
                        can_remove = vf.StringToString(grdSub.GetData(row, "DEL_ACL").ToString());    // 삭제 가능

                        //Insert/Update
                        if (index == "수정")
                        {
                            //화면명으로 화면ID 구하기
                            screen_id = ScreenId(grp_mn, screen_name);

                            //자료존재여부 Check (존재:Update   미존재:Insert)
                            //sql1 = "";
                            sql1  = "SELECT INQ_ACL";
                            sql1 += "  FROM TB_CM_ACL";
                            sql1 += " WHERE ACL_GRP_ID = " + "'" + selected_grp_id + "'";
                            sql1 += "   AND SCR_ID = (SELECT SCR_ID FROM TB_CM_SCR WHERE BIZ_GP = " + "'" + grp_mn + "'";
                            sql1 += "                                                AND SCR_NM = " + "'" + screen_name + "')";

                            DataTable dt = cd.FindDataTable(sql1);

                            //Insert
                            if (dt == null || dt.Rows.Count == 0)
                            {
                                sql1  = string.Format(" INSERT INTO TB_CM_ACL ");
                                sql1 += string.Format("             ( ");
                                sql1 += string.Format("               ACL_GRP_ID ");
                                sql1 += string.Format("              ,SCR_ID ");
                                sql1 += string.Format("              ,INQ_ACL ");
                                sql1 += string.Format("              ,REG_ACL ");
                                sql1 += string.Format("              ,MOD_ACL ");
                                sql1 += string.Format("              ,DEL_ACL ");
                                sql1 += string.Format("              ,REGISTER ");
                                sql1 += string.Format("              ,REG_DDTT ");
                                sql1 += string.Format("             ) ");
                                sql1 += string.Format(" VALUES ");
                                sql1 += string.Format("      ( ");
                                sql1 += string.Format("        '{0}' ", selected_grp_id);      //ACL_GRP_ID 
                                sql1 += string.Format("       ,'{0}' ", screen_id);            //SCR_ID        
                                sql1 += string.Format("       ,'{0}' ", can_search);           //INQ_ACL
                                sql1 += string.Format("       ,'{0}' ", can_regist);           //REG_ACL
                                sql1 += string.Format("       ,'{0}' ", can_modify);           //MOD_ACL
                                sql1 += string.Format("       ,'{0}' ", can_remove);           //DEL_ACL 
                                sql1 += string.Format("       ,'{0}' ", ck.UserID);            //REGISTER 
                                sql1 += string.Format("       ,SYSDATE ");                     //REG_DDTT   
                                sql1 += string.Format("      ) ");

                                //sql1 = string.Format("INSERT INTO TB_CM_ACL (ACL_GRP_ID, SCR_ID, INQ_ACL, REG_ACL, MOD_ACL, DEL_ACL) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                //                        selected_grp_id, screen_id, can_search, can_regist, can_modify, can_remove);

                                cmd.CommandText = sql1;
                                cmd.ExecuteNonQuery();

                                InsCnt++;
                            }
                            //Delete, Update
                            else
                            {
                                //Delete
                                if (can_search == "N" && can_regist == "N" && can_modify == "N" && can_remove == "N")
                                {
                                    sql1 = string.Format(" DELETE FROM TB_CM_ACL WHERE ACL_GRP_ID = '{0}' AND SCR_ID = '{1}'", selected_grp_id, screen_id);

                                    cmd.CommandText = sql1;
                                    cmd.ExecuteNonQuery();

                                    DelCnt++;
                                }
                                //Update
                                else
                                {
                                    sql1  = string.Format(" UPDATE TB_CM_ACL ");
                                    sql1 += string.Format(" SET    INQ_ACL  = '{0}'   ", can_search);
                                    sql1 += string.Format("       ,REG_ACL  = '{0}'   ", can_regist);
                                    sql1 += string.Format("       ,MOD_ACL  = '{0}'   ", can_modify);
                                    sql1 += string.Format("       ,DEL_ACL  = '{0}'   ", can_remove);
                                    sql1 += string.Format("       ,MODIFIER = '{0}'   ", ck.UserID);
                                    sql1 += string.Format("       ,MOD_DDTT = SYSDATE ");
                                    sql1 += string.Format(" WHERE ACL_GRP_ID = '{0}'  ", selected_grp_id);
                                    sql1 += string.Format(" AND   SCR_ID = '{0}'      ", screen_id);

                                    //sql1 = string.Format("UPDATE TB_CM_ACL SET INQ_ACL = '{0}', REG_ACL = '{1}', MOD_ACL = '{2}', DEL_ACL = '{3}' WHERE ACL_GRP_ID = '{4}' AND SCR_ID = '{5}'",
                                    //                        can_search, can_regist, can_modify, can_remove, selected_grp_id, screen_id);

                                    cmd.CommandText = sql1;
                                    cmd.ExecuteNonQuery();

                                    UpCnt++;
                                }
                            }
                        }
                    }
                }

                //실행후 성공
                transaction.Commit();
                //SetDataBinding();
                Button_Click(btnDisplay, null);

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), message);

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

        private void grdMain_Row_Selected(int selectedRow)
        {
            InitGridData_Sub();

            selectedGrd = grdMain;

            selected_grp_id = grdMain.Rows[selectedRow]["ACL_GRP_ID"].ToString().Trim();

            string sql1 = "";

            try
            {
                sql1  = string.Format(" SELECT ");
                sql1 += string.Format("       TO_CHAR(rownum) as NUM ");
                sql1 += string.Format("       ,SORT_SEQ ");
                sql1 += string.Format("       ,ACL_GRP_ID ");
                sql1 += string.Format("       ,BIZ_GP ");
                sql1 += string.Format("       ,SCR_NM ");
                sql1 += string.Format("       ,'False' AS TOTAL ");
                sql1 += string.Format("       ,INQ_ACL, REG_ACL ");
                sql1 += string.Format("       ,MOD_ACL, DEL_ACL ");
                sql1 += string.Format("       ,'' AS GUBUN ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("           SELECT rownum as NUM ");
                sql1 += string.Format("                 ,'1'          AS SORT_SEQ ");
                sql1 += string.Format("                 ,A.ACL_GRP_ID AS ACL_GRP_ID ");
                sql1 += string.Format("                 ,B.BIZ_GP     AS BIZ_GP ");
                sql1 += string.Format("                 ,B.SCR_NM     AS SCR_NM ");
                sql1 += string.Format("                 ,(CASE WHEN A.INQ_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS INQ_ACL ");
                sql1 += string.Format("                 ,(CASE WHEN A.REG_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS REG_ACL ");
                sql1 += string.Format("                 ,(CASE WHEN A.MOD_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS MOD_ACL ");
                sql1 += string.Format("                 ,(CASE WHEN A.DEL_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS DEL_ACL ");
                sql1 += string.Format("                 ,A.REGISTER   AS REGISTER ");
                sql1 += string.Format("                 ,A.REG_DDTT   AS REG_DDTT ");
                sql1 += string.Format("                 ,A.MODIFIER   AS MODIFIER ");
                sql1 += string.Format("                 ,A.MOD_DDTT   AS MOD_DDTT ");
                sql1 += string.Format("           FROM   TB_CM_ACL A ");
                sql1 += string.Format("                 ,TB_CM_SCR B ");
                sql1 += string.Format("           WHERE A.SCR_ID     = B.SCR_ID ");
                sql1 += string.Format("           AND   A.ACL_GRP_ID = '{0}' ",selected_grp_id);
                sql1 += string.Format("      UNION ");
                sql1 += string.Format("           SELECT rownum as NUM, '2'  AS SORT_SEQ ");
                sql1 += string.Format("                 ,''        AS ACL_GRP_ID ");
                sql1 += string.Format("                 ,A.BIZ_GP         AS BIZ_GP ");
                sql1 += string.Format("                 ,A.SCR_NM         AS SCR_NM ");
                sql1 += string.Format("                 ,'N' AS INQ_ACL ");
                sql1 += string.Format("                 ,'N' AS REG_ACL ");
                sql1 += string.Format("                 ,'N' AS MOD_ACL ");
                sql1 += string.Format("                 ,'N' AS DEL_ACL ");
                sql1 += string.Format("                 ,NULL AS REGISTER ");
                sql1 += string.Format("                 ,NULL AS REG_DDTT ");
                sql1 += string.Format("                 ,NULL AS MODIFIER ");
                sql1 += string.Format("                 ,NULL AS MOD_DDTT ");
                sql1 += string.Format("           FROM   TB_CM_SCR A ");
                sql1 += string.Format("           WHERE A.SCR_ID NOT IN ");
                sql1 += string.Format("                               ( ");
                sql1 += string.Format("                                SELECT X.SCR_ID ");
                sql1 += string.Format("                                FROM TB_CM_ACL X ");
                sql1 += string.Format("                                WHERE X.ACL_GRP_ID = '{0}' ",selected_grp_id);
                sql1 += string.Format("                               ) ");
                sql1 += string.Format("           ORDER BY  BIZ_GP, SCR_NM ");
                sql1 += string.Format("     ) ");



                //sql1 = "";
                //sql1 += "SELECT  TO_CHAR(rownum) as NUM, SORT_SEQ, ACL_GRP_ID, BIZ_GP, SCR_NM, 'False' AS TOTAL,  INQ_ACL, REG_ACL, MOD_ACL, DEL_ACL, '' AS GUBUN  ";
                //sql1 += " FROM(                ";
                //sql1 += " SELECT rownum as NUM                 ";
                //sql1 += "       ,'1'          AS SORT_SEQ      ";
                //sql1 += "       ,A.ACL_GRP_ID          AS ACL_GRP_ID ";
                //sql1 += "       ,B.BIZ_GP     AS BIZ_GP        ";
                //sql1 += "       ,B.SCR_NM     AS SCR_NM        ";
                //sql1 += "       ,(CASE WHEN A.INQ_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS INQ_ACL       ";
                //sql1 += "       ,(CASE WHEN A.REG_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS REG_ACL       ";
                //sql1 += "       ,(CASE WHEN A.MOD_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS MOD_ACL       ";
                //sql1 += "       ,(CASE WHEN A.DEL_ACL  = 'Y' THEN 'True' ELSE 'False' END)   AS DEL_ACL       ";
                //sql1 += "       ,A.REGISTER   AS REGISTER      ";
                //sql1 += "       ,A.REG_DDTT   AS REG_DDTT      ";
                //sql1 += "       ,A.MODIFIER   AS MODIFIER      ";
                //sql1 += "       ,A.MOD_DDTT   AS MOD_DDTT      ";
                //sql1 += " FROM   TB_CM_ACL A                   ";
                //sql1 += "       ,TB_CM_SCR B                   ";
                //sql1 += " WHERE A.SCR_ID     = B.SCR_ID        ";
                //sql1 += " AND   A.ACL_GRP_ID = " + "'" + selected_grp_id + "'";
                   
                //sql1 += " UNION                                    ";
                //sql1 += " SELECT rownum as NUM, '2'  AS SORT_SEQ   ";
                //sql1 += "       ,''        AS ACL_GRP_ID ";
                //sql1 += "       ,A.BIZ_GP         AS BIZ_GP        ";
                //sql1 += "       ,A.SCR_NM         AS SCR_NM        ";
                //sql1 += "       ,'N' AS INQ_ACL                    ";
                //sql1 += "       ,'N' AS REG_ACL                    ";
                //sql1 += "       ,'N' AS MOD_ACL                    ";
                //sql1 += "       ,'N' AS DEL_ACL                    ";
                //sql1 += "       ,NULL AS REGISTER                  ";
                //sql1 += "       ,NULL AS REG_DDTT                  ";
                //sql1 += "       ,NULL AS MODIFIER                  ";
                //sql1 += "       ,NULL AS MOD_DDTT                  ";
                //sql1 += " FROM   TB_CM_SCR A                       ";
                //sql1 += " WHERE A.SCR_ID NOT IN (SELECT X.SCR_ID FROM TB_CM_ACL X WHERE X.ACL_GRP_ID = " + "'" + selected_grp_id + "')";
                //sql1 += "       ORDER BY  BIZ_GP, SCR_NM      ";
                //sql1 += " )                                        ";

                olddt_sub = cd.FindDataTable(sql1);

                moddt_sub = olddt_sub.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdSub.SetDataBinding(moddt_sub, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt_sub.Rows.Count.ToString() + "건이 조회 되었습니다.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }

        //상세그리드 저장 -> CM_권한

        #endregion

        //화면명으로 화면ID 구하기
        // sBizGp  : 업무구분
        // ScrName : 화면명
        private string ScreenId(string sBizGp, string ScrName)
        {
            string sql = "";
            DataTable dt = new DataTable();

            //화면명으로 화면ID 구하기
            
            sql  = " SELECT SCR_ID";
            sql += "  FROM TB_CM_SCR";
            sql += " WHERE BIZ_GP = " + "'" + sBizGp + "'";
            sql += "   AND SCR_NM = " + "'" + ScrName + "'";

            dt = cd.FindDataTable(sql);

            return dt.Rows[0]["SCR_ID"].ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdSub_Click(object sender, EventArgs e)
        {
            selectedGrd = grdSub;
        }

        private void txtAcl_Grp_NM_KeyDown(object sender, KeyEventArgs e)
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
