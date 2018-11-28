using ComLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using ComLib.clsMgr;
using System.Collections;
using BGsystemLibrary.Common;
using System.Data.SqlClient;

namespace BGsystemLibrary.MatMgmt
{
    public partial class MatTakeOverMgmt : Form
    {

        #region 공통 생성자
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        //데이터테이블
        DataTable olddtMain;
        DataTable moddtMain;
        DataTable olddtSub;
        DataTable moddtSub;

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private C1FlexGrid _selectedGrd = new C1FlexGrid();
        private int main_GridRowsCount = 2;
        private int main_GridColsCount = 13;
        private int main_RowsFixed = 2;
        private int main_RowsFrozen = 0;
        private int main_ColsFixed = 0;
        private int main_ColsFrozen = 0;

        private int sub_GridRowsCount = 2;
        private int sub_GridColsCount = 12;
        private int sub_RowsFixed = 2;
        private int sub_RowsFrozen = 0;
        private int sub_ColsFixed = 0;
        private int sub_ColsFrozen = 0;

        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        public string heatNo = "";     //HEAT NO

        // 프로그램 명 변수
        private static string ownerNM = "";
        private static string titleNM = "";

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한

        private string strBefValue_Main;
        private string strBefValue_Sub;
        private string lastSearchHeatNo="";
        C1FlexGrid selectedGrd;
        #endregion 공통 생성자

        public MatTakeOverMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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
        private void MatTakeOverMgmt_Load(object sender, EventArgs e)
        {
            //그리드 초기화
            DrawMainGrid(grdMain);
            DrawSubGrid(grdSub);
            //초기화
            InitControl();
            //조회버튼 클릭
            //btnDisplay_Click(null, null);

            selectedGrd = grdMain;
        }

        private void DrawSubGrid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();

            try
            {
                int _GridRowsCount  = sub_GridRowsCount;
                int _GridColsCount  = sub_GridColsCount;
                int _RowsFixed      = sub_RowsFixed;
                int _RowsFrozen     = sub_RowsFrozen;
                int _ColsFixed      = sub_ColsFixed;
                int _ColsFrozen     = sub_ColsFrozen;


                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexSubGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                MakeAllSelect();

                //var crCellRange = grdItem.GetCellRange(1, 1);//, 0, grdMain.Cols["MFG_DATE"].Index);
                //crCellRange.Style = grdItem.Styles["ModifyStyle"];
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

        

        ArrayList _al = new ArrayList();

        /// <summary>
        /// 그리드 컬럼 스타일 적용
        /// </summary>
        /// <param name="grdItem"></param>
        private void FlexMainGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 80;
            grdItem.Cols[2].Width = 80;
            grdItem.Cols[3].Width = 80;
            grdItem.Cols[4].Width = 80;
            grdItem.Cols[5].Width = 80;
            grdItem.Cols[6].Width = 80;
            grdItem.Cols[7].Width = 80;
            grdItem.Cols[8].Width = 120;
            grdItem.Cols[9].Width = 80;
            grdItem.Cols[10].Width = 120;
            grdItem.Cols[11].Width = 80;
            grdItem.Cols[12].Width = 80;


            
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "HEAT";
            grdItem[1, 2] = "강종";
            grdItem[1, 3] = "강종";
            grdItem[1, 4] = "품목";
            grdItem[1, 5] = "품목규격";
            grdItem[1, 6] = "규격";
            grdItem[1, 7] = "강종규격";
            grdItem[1, 8] = "생산일자";
            grdItem[1, 9] = "생산본수";
            grdItem[1, 10] = "인수일자";
            grdItem[1, 11] = "인수본수";
            grdItem[1, 12] = "표면상태";


            grdItem.Cols[2].Visible = false;
            grdItem.Cols[4].Visible = false;
            grdItem.Cols[5].Visible = false;
            grdItem.Cols[7].Visible = false;
            grdItem.Cols[10].Visible = false;


            for (int col = 0; col < grdItem.Cols.Count ; col++)
            {
                if (col == 12)
                {
                    grdItem.Cols[col].AllowEditing = true;
                }
                else
                    grdItem.Cols[col].AllowEditing = false;

                //grdItem.Cols[col].AllowEditing = false;

            }
            


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 1, 1);
            clsFlexGrid.DataGridCenterStyle(grdItem, 2, 10);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 11);
            clsFlexGrid.DataGridCenterStyle(grdItem, 12);

            //Label lbSel = new Label();

            //lbSel.BackColor = Color.Transparent;
            //lbSel.Cursor = Cursors.Hand;

            //_al.Add(new HostedControl(grdMain, lbSel, 0, 1));
        }

        private void SetGridAllowEdit(C1FlexGrid grdItem, int editAllowcol)
        {
            for (int col = 0; col < grdItem.Cols.Count; col++)
            {
                if (col == editAllowcol)
                {
                    grdItem.Cols[col].AllowEditing = true;
                }
                else
                    grdItem.Cols[col].AllowEditing = false;
            }
        }

        private void FlexSubGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 80;
            grdItem.Cols[2].Width = 80;
            grdItem.Cols[3].Width = 80;
            grdItem.Cols[4].Width = 80;
            grdItem.Cols[5].Width = 80;
            grdItem.Cols[6].Width = 80;
            grdItem.Cols[7].Width = 80;
            grdItem.Cols[8].Width = 80;
            grdItem.Cols[9].Width = 80;
            grdItem.Cols[10].Width = 120;
            grdItem.Cols[11].Width = 80;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "전체선택";
            grdItem[1, 2] = "HEAT";
            grdItem[1, 3] = "HEAT 순번";
            grdItem[1, 4] = "STRAND NO";
            grdItem[1, 5] =  "강종";
            grdItem[1, 6] =  "강종";
            grdItem[1, 7] =  "품목";
            grdItem[1, 8] =  "규격";
            grdItem[1, 9] =  "규격";
            grdItem[1, 10] = "생산일자";
            grdItem[1, 11] = "MarkingCode";

            //grdItem.Cols[3].Visible = false;
            grdItem.Cols[5].Visible = false;
            grdItem.Cols[7].Visible = false;
            grdItem.Cols[8].Visible = false;

            for (int col = 0; col < grdItem.Cols.Count; col++)
            {
                if (col == 1)
                {
                    grdItem.Cols[col].AllowEditing = true;
                }
                else
                    grdItem.Cols[col].AllowEditing = false;

                //grdItem.Cols[col].AllowEditing = false;

            }


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellColumnStyle(grdItem, 1, 1, 1, 1);

            // 컬럼 스타일 설정
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 2);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3, grdItem.Cols.Count - 1);

        }

        private void DrawMainGrid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                int _GridRowsCount = main_GridRowsCount;
                int _GridColsCount = main_GridColsCount;
                int _RowsFixed     = main_RowsFixed    ;
                int _RowsFrozen    = main_RowsFrozen   ;
                int _ColsFixed     = main_ColsFixed    ;
                int _ColsFrozen    = main_ColsFrozen   ;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexMainGridCol(grdItem);
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

        private void MakeAllSelect()
        {
            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;
            
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSel.Click += AllRowSelectedEvent;

            _al.Add(new HostedControl(grdSub, lbSel, 1, 1));
        }

        private void InitControl()
        {
            //조회조건 판넬 색상 세팅
            clsStyle.Style.InitPanel(panel1);

            //cs.InitCombo(cbx_TakeOver_Gp, StringAlignment.Center);

            cd.SetCombo(cbx_TakeOver_Gp, "TAKEOVER_CD", "", false);

            start_dt.Value = DateTime.Now.Date.AddDays(-1);
            end_dt.Value = DateTime.Now.Date;

        }


        private void pbx_Search_Click(object sender, EventArgs e)
        {
            //if (this.scrAuthMod != "Y")
            //{
            //    MessageBox.Show("수정 권한이 없습니다");
            //    return;
            //}
            ck.StrKey1 = txt_Heat.Text;
            ck.StrKey2 = ((ComLib.DictionaryList)this.cbx_TakeOver_Gp.SelectedItem).fnValue;
            //HEAT 선택 팝업
            HeatPopup();
        }

        private void HeatPopup()
        {
            HeatPopup dia = new HeatPopup();
            dia.Owner = this; //A폼을 지정하게 된다.
            dia.StartPosition = FormStartPosition.CenterScreen;
            dia.ShowDialog();
            if (ck.StrKey1 != "" || ck.StrKey2 != "")
            {
                txt_Heat.Text = ck.StrKey1;
                ck.StrKey1 = "";
                ck.StrKey2 = "";
            }
        }

        private void callback(object sender)
        {
            txt_Heat.Text = heatNo;
        }



        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    if (this.scrAuthInq != "Y")
                    {
                        //MessageBox.Show("조회 권한이 없습니다");
                        return;
                    }
                    //InitGridData(true);
                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");
                    SetDataBinding();  // 조회 버튼을 통한 데이터입력
                    break;


                case "btnSave":
                    SaveData();
                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
                case "btnTakeOver":
                    TakeOver();
                    break;
                case "btnTakOverCancel":
                    TakOverCancel();
                    break;
                case "btnForceInsert":
                    ForceInsert();
                    break;
            }
        }

        private void ForceInsert()
        {
            string titleNM = "인수강제등록";
            string menuNM = "";
            string categoryNM = "";
            TakeOverRegist dia = new TakeOverRegist(titleNM, menuNM, categoryNM);
            dia.StartPosition = FormStartPosition.CenterScreen;
            //dia.FormSendEvent += new ScheduleRegist.FormSendDataHandler(callback);
            dia.ShowDialog();

            btnDisplay.PerformClick();
        }

        private void TakOverCancel()
        {
            if (grdSub.Rows.Count <= sub_GridRowsCount) return;

            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            if (!IsCheckedItem())
            {
                MessageBox.Show(" 인수 취소할 BLOOM 항목을 선택하세요.");
                return;
            }

            //string infomsg = string.Format("HEAT NO :{0}, BLOOM : {1}인수 진행 하시겠습니까?", grdSub.GetData(3, 2).ToString().Trim(), GetSelectedBloomData());

            if (MessageBox.Show("인수 취소 진행 하시겠습니까? ", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }


            


            SqlCommand cmd = new SqlCommand();

            string spName = "SP_MatTakeOverMgmt_CAN";

            string take_Over_gp = ((ComLib.DictionaryList)cbx_TakeOver_Gp.SelectedItem).fnValue;
            string _heat_no = grdSub.GetData(2, 2).ToString().Trim();
            string _surface_state = GetModifiedSurfaceState();
            string takeOverBloomData = GetSelectedBloomData();
            string proc_stat = "";
            string proc_msg = "";
            try
            {

                param.Clear();
                param.Add(SqlDbType.VarChar, "@P_INSU_GP", take_Over_gp, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_HEAT_NO", _heat_no, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_SURFACE_STAT", _surface_state, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_HEAT_SEQ", takeOverBloomData, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_USER_ID", ck.UserID, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_PROC_STAT", proc_stat, ParameterDirection.Output, 4000);
                param.Add(SqlDbType.VarChar, "@P_PROC_MSG", proc_msg, ParameterDirection.Output, 4000);


                this.Cursor = Cursors.AppStarting;
                cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                this.Cursor = Cursors.Default;

                //lastSearchHeatNo = _heat_no;
                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);

                if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                {
                    MessageBox.Show("@P_PROC_MSG:" + cmd.Parameters["@P_PROC_MSG"].Value.ToString());
                    string _warning_msg = "HEAT_NO: " + _heat_no + ", HEAT_SEQ LIST:" + takeOverBloomData + "가 인수 취소 실패하였습니다.";
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                    return;
                }

                Button_Click(btnDisplay, null);

                string _info_msg = "HEAT_NO: " + _heat_no +", HEAT_SEQ LIST:"+ takeOverBloomData + "가 인수 취소 되었습니다.";
                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _info_msg);

            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {

                
                //SelectFirstRow();
            }
        }



        private void TakeOver()
        {

            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            if (grdSub.Rows.Count <= sub_GridRowsCount) return;


            if (!IsCheckedItem())
            {
                MessageBox.Show(" 인수 할  BLOOM을 선택하세요.");
                return;
            }

            string infomsg = string.Format("HEAT NO :{0}, BLOOM 갯수: {1}인수 진행 하시겠습니까?", grdSub.GetData(3, 2).ToString().Trim(),"");

            if (MessageBox.Show("인수 진행 하시겠습니까? ", Text, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }


            SqlCommand cmd = new SqlCommand();

            string spName = "SP_MatTakeOverMgmt_UPD";

            string take_Over_gp = ((ComLib.DictionaryList)cbx_TakeOver_Gp.SelectedItem).fnValue;
            string _heat_no = grdSub.GetData(3, 2).ToString().Trim();
            string _surface_state = GetModifiedSurfaceState();
            string takeOverBloomData = GetSelectedBloomData();
            string proc_stat = "";
            string proc_msg = "";
            try
            {

                param.Clear();
                param.Add(SqlDbType.VarChar, "@P_INSU_GP"       , take_Over_gp, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_HEAT_NO"       , _heat_no, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_SURFACE_STAT"  , _surface_state, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_HEAT_SEQ"      , takeOverBloomData, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_USER_ID"       , ck.UserID, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_PROC_STAT"     , proc_stat, ParameterDirection.Output, 4000);
                param.Add(SqlDbType.VarChar, "@P_PROC_MSG"      , proc_msg, ParameterDirection.Output, 4000);


                this.Cursor = Cursors.AppStarting;
                cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                this.Cursor = Cursors.Default;

                //lastSearchHeatNo = _heat_no;
                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);

                if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                {
                    string _warning_msg = "HEAT_NO: " + _heat_no + ", HEAT_SEQ LIST:" + takeOverBloomData + "가 인수 실패하였습니다.";
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                    return;
                }

                Button_Click(btnDisplay, null);

                string _info_msg = "HEAT_NO: " + _heat_no + ", HEAT_SEQ LIST:" + takeOverBloomData + "가 인수 되었습니다.";
                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _info_msg);

            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {
                
                //Button_Click(btnDisplay, null);
                //SelectFirstRow();
            }

        }

        private void TakeOver_old()
        {


            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();


            string spName = "SP_MatTakeOverMgmt_UPD";  // 프로시저명

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                //무제한 으로 설정
                cmd.CommandTimeout = 0;
                


            }
            catch (Exception)
            {

                throw;
            }
        }
        private void SetDataBinding_MainGrd()
        {
            try
            {

                string take_Over_gp = ((ComLib.DictionaryList)cbx_TakeOver_Gp.SelectedItem).fnValue;
                string _start_dt = vf.Format(start_dt.Value, "yyyyMMdd");
                string _end_dt = vf.Format(end_dt.Value, "yyyyMMdd");

                param.Clear();
                param.Add(SqlDbType.VarChar, "@P_INSU_GP", take_Over_gp, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_FR_DATE", _start_dt, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_TO_DATE", _end_dt, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_HEAT_NO", txt_Heat.Text.Trim(), ParameterDirection.Input);


                //SQL 쿼리 조회 후 데이터셋 return
                olddtMain = cd.ExecuteStoreProcedureDataTable("SP_MatTakeOverMgmt_GetH", param);
                moddtMain = olddtMain.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawMainGrid(grdMain, moddtMain);
                this.Cursor = Cursors.Default;

                if (moddtMain.Rows.Count > 0)
                {
                    grdMain.RowSel = main_GridRowsCount;
                }

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), Text + ":" + moddtMain.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //grdMain_Click(null, null);
                SelectRow();
            }
        }

        private void SetDataBinding_SubGrd(C1FlexGrid _grdItem, int rowSel)
        {

            try
            {

                string take_Over_gp = ((ComLib.DictionaryList)cbx_TakeOver_Gp.SelectedItem).fnValue;
                string _start_dt = vf.Format(start_dt.Value, "yyyyMMdd");
                string _end_dt = vf.Format(end_dt.Value, "yyyyMMdd");
                string _heatNo = _grdItem.GetData(rowSel, "HEAT_NO").ToString().Trim();

                param.Clear();
                param.Add(SqlDbType.VarChar, "@P_INSU_GP", take_Over_gp, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_FR_DATE", _start_dt, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_TO_DATE", _end_dt, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_HEAT_NO", _heatNo, ParameterDirection.Input);


                //SQL 쿼리 조회 후 데이터셋 return

                this.Cursor = Cursors.AppStarting;
                olddtSub = cd.ExecuteStoreProcedureDataTable("SP_MatTakeOverMgmt_GetD", param);
                moddtSub = olddtSub.Copy();

                //조회된 데이터 그리드에 세팅
                DrawSubGrid(grdSub, moddtSub);
                this.Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), Text + ":" + moddtSub.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }

        private string GetModifiedSurfaceState()
        {
            string rtn = "";

            rtn = grdMain.GetData(grdMain.RowSel, 12).ToString();

            return rtn;
        }

        private bool IsCheckedItem()
        {
            bool rtn = false;

            DataTable dt = (DataTable)grdSub.DataSource;

            if (dt == null ) return false;

            foreach (DataRow  row in dt.Rows)
            {
                if (row[1].ToString() == "True")
                {
                    return rtn = true;
                }
            }

            return rtn;
        }

        private int GetBloomCnt()
        {
            int rtn = 0;

            DataTable dt = (DataTable)grdSub.DataSource;

            if (dt == null) return 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row[1].ToString() == "True")
                {
                    rtn ++;
                }
            }
            return rtn;
        }

        private string GetSelectedBloomData()
        {
            // format HEAT_NO, 표면상태(입력된것), HEAT_SEQ,USER_ID, 처리결과, 처리결과 MSG
            string rtData = "";


            DataTable dt = (DataTable)grdSub.DataSource;
            int counts = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row[1].ToString() == "True")
                    {
                        rtData += row[3].ToString() + ",";
                        counts++;
                    }

                }
                if (counts >= 1)
                {
                    rtData = rtData.Substring(0, rtData.Length - 1);
                }
                
            }

            return rtData;
        }

        private void InitGridData()
        {
            clsFlexGrid.grdDataClearForBind(grdMain);
            clsFlexGrid.grdDataClearForBind(grdSub);
        }


        List<C1FlexGrid> gridList;
        private void SaveExcel()
        {
            gridList = new List<C1FlexGrid>();
            gridList.Add(grdMain);
            gridList.Add(grdSub);
            grdMain.Tag = "소재인수개요";
            grdSub.Tag = "소재인수상세";
            vf.SaveExcel(titleNM, gridList);
            //vf.SaveExcel(titleNM + _selectedGrd.Tag, _selectedGrd);
        }

        private void SaveData()
        {
            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            if (!IsSaveItem())
            {
                MessageBox.Show("수정된 표면상태 항목이 없습니다.");
                return;
            }



            string _heat_no = "";// grdMain.GetData(grdMain.RowSel, 2).ToString().Trim();
            string _surface_state = "";// GetModifiedSurfaceState();
            string sql = "";
            int UpCnt = 0;

            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                this.Cursor = Cursors.AppStarting;
                for (int row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    if (grdMain.GetData(row, "L_NUM").ToString() == "수정")
                    {
                        _heat_no = grdMain.GetData(row, "HEAT_NO").ToString().Trim();
                        _surface_state = grdMain.GetData(row, "SURFACE_STAT").ToString().Trim();

                        sql = string.Format(" UPDATE TB_MAT_TAKE_OVER_HEAT                                 ");
                        sql += string.Format(" SET                                                         ");
                        sql += string.Format("        SURFACE_STAT = '{0}'                                 ", _surface_state);
                        sql += string.Format("       ,MODIFIER = '{0}'                                     ", ck.UserID);
                        sql += string.Format("       ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        sql += string.Format(" WHERE HEAT_NO = '{0}'                                       ", _heat_no);

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        UpCnt++;
                    }
                        
                }
 
                this.Cursor = Cursors.Default;

                //lastSearchHeatNo = _heat_no;

                //실행후 성공
                transaction.Commit();

                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);

                Button_Click(btnDisplay, null);

            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {

                
                //SelectFirstRow();
            }


        }

        private bool IsSaveItem()
        {
            bool rtn = false;

            DataTable dt = (DataTable)grdMain.DataSource;

            if (dt == null) return false;

            foreach (DataRow row in dt.Rows)
            {
                if (row[0].ToString() == "수정")
                {
                    return rtn = true;
                }
            }

            return rtn;
        }

        private void SetDataBinding()
        {
            SetDataBinding_Grd_byinitData();

            SetDataBinding_MainGrd();
        }
        private void SetDataBinding_Grd_byinitData()
        {
            InitGridData();

        }

        bool allChecked = false;
        private void AllRowSelectedEvent(object sender, EventArgs e)
        {

            if (allChecked)
            {
                for (int rowCnt = sub_GridRowsCount; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, "SEL", false);
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = sub_GridRowsCount; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, "SEL", true);
                }
                allChecked = true;

            }
        }
        clsParameterMember param = new clsParameterMember();


        private void SelectRow()
        {
            if (clsFlexGrid.CanRowSelectGrid(grdMain, main_GridRowsCount))
            {
                //int selectRow = GetRowBykey(lastSearchHeatNo, main_GridRowsCount);
                SetDataBinding_SubGrd(grdMain, grdMain.RowSel);
            }
                
        }

        private int GetRowBykey(string lastSearchHeatNo, int _main_GridRowsCount)
        {
            int rtn = _main_GridRowsCount;

            for (int row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
            {
                if (grdMain.Rows[row][1].ToString().Equals(lastSearchHeatNo))
                {
                    rtn = row;
                }
            }

            return rtn;
        }

        private void DrawMainGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {

                //그리드 콤보박스 지정
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("MFG_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now.Date);

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);

                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //마지막행 사이즈조절, 로우공백흰색
                grdItem.ExtendLastCol = true;
                grdItem.Styles.EmptyArea.BackColor = Color.White;

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


        private void DrawSubGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {

                //그리드 콤보박스 지정
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("SEL", FlexGridCellTypeEnum.CheckBox, "true");
                FlexGridColumns.Add("MFG_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now.Date);

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);

                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //마지막행 사이즈조절, 로우공백흰색
                grdItem.ExtendLastCol = true;
                grdItem.Styles.EmptyArea.BackColor = Color.White;


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

        private void cbx_TakeOver_Gp_DropDown(object sender, EventArgs e)
        {


        }

        private void SetNotTakeOverSearchMode()
        {
            lb_TakeOverDay.Text = "생산일자";
            //grdMain.Cols[12].Visible = false;//표면상태
            grdMain[1, 12] = "비고";
            grdMain.Cols[10].Visible = false; //인수일자
            //SetGridAllowEdit(grdMain, 12);
            //if (this.scrAuthMod == "Y")
            //{
            //    btnSave.Enabled = false;
            //    btnTakeOver.Enabled = true;
            //    btnTakOverCancel.Enabled = false;
            //    grdMain.Cols[12].AllowEditing = false;
            //}else
            //{
            //    btnSave.Enabled = false;
            //    btnTakeOver.Enabled = false;
            //    btnTakOverCancel.Enabled = false;
            //    grdMain.Cols[12].AllowEditing = false;
            //}

            if (this.scrAuthMod == "Y")
            {
                btnSave.Enabled = false;
                grdMain.Cols[12].AllowEditing = false;
            }
            else
            {
                btnSave.Enabled = false;
                grdMain.Cols[12].AllowEditing = false;
            }

            if (this.scrAuthReg == "Y")
            {
                btnTakeOver.Enabled = true;
                btnForceInsert.Enabled = true;
            }
            else
            {
                btnTakeOver.Enabled = false;
                btnForceInsert.Enabled = false;
            }

            if (this.scrAuthDel == "Y")
            {
                btnTakOverCancel.Enabled = false;
            }
            else
            {
                btnTakOverCancel.Enabled = false;
            }




        }

        private void SetTakeOverSearchMode()
        {
            lb_TakeOverDay.Text = "인수일자";
            //SetGridAllowEditAllFalse(grdMain);
            //grdMain.Cols[12].Visible = true; //표면상태
            grdMain[1, 12] = "표면상태";
            grdMain.Cols[10].Visible = true; //인수일자
            //if (this.scrAuthMod == "Y")
            //{
            //    btnSave.Enabled = true;
            //    btnTakeOver.Enabled = false;
            //    btnTakOverCancel.Enabled = true;
            //    grdMain.Cols[12].AllowEditing = true;
            //}
            //else
            //{
            //    btnSave.Enabled = false;
            //    btnTakeOver.Enabled = false;
            //    btnTakOverCancel.Enabled = false;
            //    grdMain.Cols[12].AllowEditing = false;
            //}

            if (this.scrAuthMod == "Y")
            {
                btnSave.Enabled = true;
                grdMain.Cols[12].AllowEditing = true;
            }
            else
            {
                btnSave.Enabled = false;
                grdMain.Cols[12].AllowEditing = false;
            }



            if (this.scrAuthReg == "Y")
            {
                btnTakeOver.Enabled = false;
                btnForceInsert.Enabled = true;
            }
            else
            {
                btnTakeOver.Enabled = false;
                btnForceInsert.Enabled = false;
            }

            if (this.scrAuthDel == "Y")
            {
                btnTakOverCancel.Enabled = true;
            }
            else
            {
                btnTakOverCancel.Enabled = false;
            }


        }

        private void SetGridAllowEditAllFalse(C1FlexGrid grdItem)
        {
            for (int col = 0; col < grdItem.Cols.Count; col++)
            {
                grdItem.Cols[col].AllowEditing = false;
            }
        }

        private void cbx_TakeOver_Gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModeSetupForTakeOrNot();

            Button_Click(btnDisplay, null);
            
        }

        private void ModeSetupForTakeOrNot()
        {
            string takeoverYN = ((ComLib.DictionaryList)cbx_TakeOver_Gp.SelectedItem).fnValue;
            if (takeoverYN == "Y")
            {
                SetTakeOverSearchMode();
            }
            else
            {
                SetNotTakeOverSearchMode();
            }
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            C1FlexGrid grid = sender as C1FlexGrid;
            if (!IsAailableRowSelect(grid, main_GridRowsCount)) return;

            SetDataBinding_SubGrd(grid, grdMain.RowSel);

            selectedGrd = grid;
        }




        private bool IsAailableRowSelect(C1FlexGrid _grid, int _minRowCount)
        {
            if (_grid.Rows.Count < _minRowCount || _grid.RowSel < 0) return false;
            return true;
        }



        private void grdSub_Paint(object sender, PaintEventArgs e)
        {
            foreach (HostedControl hosted in _al)
                hosted.UpdatePosition();
        }

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            // 수정여부 확인을 위해 저장
            strBefValue_Main = grd.GetData(grd.Row, grd.Col).ToString();
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            string strAftValue = editedGrd.GetData(editedRow, editedCol).ToString().Trim();

            if (string.Equals(strBefValue_Main, strAftValue, StringComparison.CurrentCulture))
            {
                return;
            }else
            {
                SetModifiedMode(editedGrd, editedRow);
            }

            strBefValue_Main = "";

        }

        private void SetModifiedMode(C1FlexGrid _editedGrd, int _editedRow)
        {
            _editedGrd.SetData(_editedRow, 0, "수정");

            // Update 배경색 지정
            //editedGrd.Rows[editedRow].Style = editedGrd.Styles["UpColor"];
            clsFlexGrid.GridCellRangeStyleColor(_editedGrd, _editedRow, 0, _editedRow, _editedGrd.Cols.Count - 1, Color.Green, Color.Black);
        }

        private void grdSub_BeforeEdit(object sender, RowColEventArgs e)
        {
            //C1FlexGrid grd = sender as C1FlexGrid;

            //// 수정여부 확인을 위해 저장
            //strBefValue_Sub = grd.GetData(grd.Row, grd.Col).ToString();
        }

        private void grdSub_AfterEdit(object sender, RowColEventArgs e)
        {
            //C1FlexGrid editedGrd = sender as C1FlexGrid;

            //int editedRow = e.Row;
            //int editedCol = e.Col;

            //string strAftValue = editedGrd.GetData(editedRow, editedCol).ToString().Trim();

            //if (string.Equals(strBefValue_Sub, strAftValue, StringComparison.CurrentCulture))
            //{
            //    return;
            //}
            //else
            //{
            //    SetModifiedMode(editedGrd, editedRow);
            //}
            
        }

        private void grdSub_CellChecked(object sender, RowColEventArgs e)
        {
            //IntializeTempStr();

            if (e.Row >= sub_GridRowsCount)
            {
                CheckEnum checkState = grdSub.GetCellCheck(e.Row, e.Col);
                for (int r = grdSub.Selection.r1; r < grdSub.Selection.r2 + 1; r++)
                {
                    grdSub.SetCellCheck(r, e.Col, checkState);
                }
            }

        }

        private void IntializeTempStr()
        {
            //strBefValue_Sub = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void grdSub_Click(object sender, EventArgs e)
        {
            C1FlexGrid grid = sender as C1FlexGrid;

            selectedGrd = grid;
        }

        private void btnForceInsert_Click(object sender, EventArgs e)
        {

        }

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

    /// <summary>
    /// HostedControl
    /// helper class that contains a control hosted within a C1FlexGrid
    /// </summary>
    public class HostedControl
    {
        internal C1FlexGrid _flex;
        internal Control _ctl;
        internal Row _row;
        internal Column _col;

        internal HostedControl(C1FlexGrid flex, Control hosted, int row, int col)
        {
            // save info
            _flex = flex;
            _ctl = hosted;
            _row = flex.Rows[row];
            _col = flex.Cols[col];


            // insert hosted control into grid
            _flex.Controls.Add(_ctl);
        }

        internal bool UpdatePosition()
        {
            // get row/col indices
            int r = _row.Index;
            int c = _col.Index;
            if (r < 0 || c < 0) return false;

            // get cell rect
            Rectangle rc = _flex.GetCellRect(r, c, false);

            // hide control if out of range
            if (rc.Width <= 0 || rc.Height <= 0 || !rc.IntersectsWith(_flex.ClientRectangle))
            {
                _ctl.Visible = false;
                return true;
            }

            // move the control and show it
            _ctl.Bounds = rc;
            _ctl.Size   =new Size(rc.Width, rc.Height);

            _ctl.Visible = true;

            // done
            return true;
        }
    }
}
