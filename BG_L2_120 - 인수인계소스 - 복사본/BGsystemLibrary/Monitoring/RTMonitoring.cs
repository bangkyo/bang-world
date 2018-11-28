using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGsystemLibrary.Monitoring
{
    public partial class RTMonitoring : Form
    {
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        clsUtil cu = new clsUtil();

        //데이터테이블
        DataTable olddt;
        DataTable moddt;
        DataTable olddt_sub;
        DataTable moddt_sub;
        DataTable grdMainDT;
        DataTable grdSubDT;

        DataTable olddtMain3 = null;
        DataTable moddtMain3 = null;

        DataTable olddtMain1 = null;
        DataTable moddtMain1 = null;

        TextBox tbCategory;
        TextBox tbCD_ID;


        List<string> msg;
        bool _CanSaveSearchLog = false;
        string selected_Category = "";

        // 셀의 수정전 값
        string strBefValue = "";
        string strBefValue2 = "";

        string ownerNM = "";
        string titleNM = "";

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        static C1FlexGrid selectedGrd;

        public delegate void ReFreshDelegate(string content);

        public delegate void SetLabelVisibleDelegate(Label lb, bool isvisible);

        public delegate void event_handler(string contents);

        public event event_handler Child_handler;



        private int main1_GridRowsCount = 6;
        private int main1_GridColsCount = 2;
        private int main1_RowsFixed = 1;
        private int main1_RowsFrozen = 0;
        private int main1_ColsFixed = 1;
        private int main1_ColsFrozen = 0;
        private int main1_TopRowsHeight = 2;
        private int main1_DataRowsHeight = 36;

        private int main2_GridRowsCount = 6;
        private int main2_GridColsCount = 2;
        private int main2_RowsFixed = 1;
        private int main2_RowsFrozen = 0;
        private int main2_ColsFixed = 1;
        private int main2_ColsFrozen = 0;
        private int main2_TopRowsHeight = 2;
        private int main2_DataRowsHeight = 36;

        private int main3_GridRowsCount = 3;
        private int main3_GridColsCount = 13;
        private int main3_RowsFixed = 3;
        private int main3_RowsFrozen = 0;
        private int main3_ColsFixed = 0;
        private int main3_ColsFrozen = 0;
        private int main3_TopRowsHeight = 2;
        private int main3_DataRowsHeight = 42;

        private int TopRowsHeight = 2;
        private int DataRowsHeight = 36;

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        //권한관련 add ]]


        private Task t;
        private Stopwatch sw;
        private TextBox IsConnected = new TextBox();
        private string scrAuth = "";
        private bool isChipWaring;
        private bool isRockWaring;

        public RTMonitoring(string titleNm, string _scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            scrAuth = _scrAuth;

            string[] scrAuthParams = _scrAuth.Split(',');

            this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장

            InitializeComponent();
        }

        private void RTMonitoring_Click(object sender, EventArgs e)
        {

        }

        private void RTMonitoring_Load(object sender, EventArgs e)
        {

            SetAcl();


            UC_Zone_Setup();

            Setup_UCZoneMoveBtn();

            InitGrid();

            SetPanel();

            lbChipChange.Visible = false;
            lbRockChange.Visible = false;


            //tmWarning.Start();

            IsConnected.TextChanged += IsConnected_TextChanged;

            sw = new Stopwatch();
            t = Task.Factory.StartNew(AsyncUpdateUIData);
        }

        private void SetAcl()
        {
            if (this.scrAuthMod != "Y")
            {
                foreach (var item in vf.GetAllChildrens(this))
                {
                    if (item.GetType().ToString() == "ComLib.UC_ZoneMoveBtn")
                    {
                        var uc_zone = item as ComLib.UC_ZoneMoveBtn;

                        //uc_zone.PopupEvent += ZoneMoveEvent;//ZoneMoveEvent(uc_zone.Before_Zone, uc_zone.After_Zone);
                        uc_zone.Enabled = false;
                    }
                }
            }
        }

        private void SetPanel()
        {

        }

        private void Setup_UCZoneMoveBtn()
        {
            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "ComLib.UC_ZoneMoveBtn")
                {
                    var uc_zone = item as ComLib.UC_ZoneMoveBtn;

                    uc_zone.PopupEvent += ZoneMoveEvent;//ZoneMoveEvent(uc_zone.Before_Zone, uc_zone.After_Zone);
                }
            }
        }


        private void ZoneMoveEvent(object sender, EventArgs e)
        {
            if (this.scrAuthMod != "Y")
            {
                MessageBox.Show("수정 권한이 없습니다");
                return;
            }

            // 이전 존의 bloom 정보와 이후 존 정보를 보여주고 이동할것인지 확인 절차 후 sp 호출.
            if (!string.IsNullOrEmpty(((Control)sender).Parent.Name))
            {

                var uc_zone = ((Control)sender).Parent as UC_ZoneMoveBtn;

                string movingBloomID = GetBloomID(uc_zone.Before_Zone);
                //if (string.IsNullOrEmpty(movingBloomID))
                //{
                //    MessageBox.Show("이전 존 ("+uc_zone.Before_Zone+") 에 이동할 BLOOM 이 없습니다. ");
                //    return;
                //}

                string msg = "Bloom ID: " + movingBloomID + " \nFrom:" + uc_zone.Before_Zone + " \nTo:" + uc_zone.After_Zone + " 이동하려고 합니다.\n진행하시겠습니까?";
                string info_msg = "Bloom ID: " + movingBloomID + "  이전 존(" + uc_zone.Before_Zone + ") 에서 이후 존(" + uc_zone.After_Zone + ")으로 이동하였습니다.";
                if (MessageBox.Show(msg, Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                SetMoveZone(uc_zone.TC_CD, info_msg);

            }

        }

        private void SetMoveZone(string tcID, string _info_msg, string data_01= "1")
        {
            SqlCommand cmd = new SqlCommand();
            //int rtn = 0;
            string spName = "SP_SCR_TO_L1_RECEIVE_CRE";


            string proc_stat = "";
            string proc_msg = "";
            clsParameterMember param = new clsParameterMember();
            try
            {

                param.Clear();

                param.Add(SqlDbType.VarChar, "@P_TC_CD", tcID, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_DATA_01", data_01, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_PROC_STAT", proc_stat, ParameterDirection.Output, 4000);
                param.Add(SqlDbType.VarChar, "@P_PROC_MSG", proc_msg, ParameterDirection.Output, 4000);

                cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                this.Cursor = Cursors.Default;

                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);


                if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                {
                    MessageBox.Show("@P_PROC_MSG:{0}"+ cmd.Parameters["@P_PROC_MSG"].Value.ToString());
                    string _warning_msg = cmd.Parameters["@P_PROC_MSG"].Value.ToString();
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                    return;
                }
                //this.Cursor = Cursors./*Default*/;

                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);
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

        private void CallSp(string tcID, string _spName,string _info_msg, string data_01 = "1")
        {
            SqlCommand cmd = new SqlCommand();
            //int rtn = 0;
            string spName = _spName;


            string proc_stat = "";
            string proc_msg = "";
            clsParameterMember param = new clsParameterMember();
            try
            {

                param.Clear();

                param.Add(SqlDbType.VarChar, "@P_TC_CD", tcID, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_DATA_01", data_01, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_PROC_STAT", proc_stat, ParameterDirection.Output, 4000);
                param.Add(SqlDbType.VarChar, "@P_PROC_MSG", proc_msg, ParameterDirection.Output, 4000);

                cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                this.Cursor = Cursors.Default;

                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);


                if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                {
                    MessageBox.Show("@P_PROC_MSG:{0}" + cmd.Parameters["@P_PROC_MSG"].Value.ToString());
                    string _warning_msg = cmd.Parameters["@P_PROC_MSG"].Value.ToString();
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                    return;
                }
                //this.Cursor = Cursors./*Default*/;

                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);
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

        private string GetBloomID(string before_Zone)
        {
            string rtn = "";
            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "ComLib.UC_Zone_B")
                {
                    var uc_zone = item as ComLib.UC_Zone_B;

                    if (uc_zone.ZoneCD == before_Zone)
                    {
                        return rtn = uc_zone.BloomNo;
                    }
                }
                if (item.GetType().ToString() == "ComLib.UC_Zone_BS")
                {
                    var uc_zone = item as ComLib.UC_Zone_BS;

                    if (uc_zone.ZoneCD == before_Zone)
                    {
                        return rtn = uc_zone.BloomNo;
                    }
                }

                if (item.GetType().ToString() == "ComLib.UC_Zone_BS_BL")
                {
                    var uc_zone = item as ComLib.UC_Zone_BS_BL;

                    if (uc_zone.ZoneCD == before_Zone)
                    {
                        return rtn = uc_zone.BloomNo;
                    }
                }

                if (item.GetType().ToString() == "ComLib.UC_Zone_BC")
                {
                    var uc_zone = item as ComLib.UC_Zone_BC;

                    if (uc_zone.ZoneCD == before_Zone)
                    {
                        return rtn = uc_zone.BloomNo;
                    }
                }
            }
            return rtn;
        }

        private void UC_Zone_Setup()
        {
            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "ComLib.UC_Zone_B")
                {
                    var uc_zone = item as ComLib.UC_Zone_B;

                    uc_zone.PopupEvent += PopupEvent;

                }
                if (item.GetType().ToString() == "ComLib.UC_Zone_BC")
                {
                    var uc_zone = item as ComLib.UC_Zone_BC;

                    uc_zone.PopupEvent += PopupEvent;

                }

                if (item.GetType().ToString() == "ComLib.UC_Zone_BS")
                {
                    var uc_zone = item as ComLib.UC_Zone_BS;

                    uc_zone.PopupEvent += PopupEvent;
                }

                if (item.GetType().ToString() == "ComLib.UC_Zone_BS_R")
                {
                    var uc_zone = item as ComLib.UC_Zone_BS_R;

                    uc_zone.PopupEvent += PopupEvent;
                }

                if (item.GetType().ToString() == "ComLib.UC_Zone_BS_BL")
                {
                    var uc_zone = item as ComLib.UC_Zone_BS_BL;

                    uc_zone.PopupEvent += PopupEvent;
                }

                if (item.GetType().ToString() == "ComLib.UC_Zone_BSO")
                {
                    var uc_zone = item as ComLib.UC_Zone_BSO;

                    uc_zone.PopupEvent += PopupEvent;
                }
            }
        }

        private void PopupEvent(object sender, EventArgs e)
        {
            switch ((((Control)sender).Parent).GetType().FullName)
            {
                case "ComLib.UC_Zone_B":
                    ShowZoneMovePopup(((ComLib.UC_Zone_B)(((Control)sender).Parent)).ZoneCD);
                    break;
                case "ComLib.UC_Zone_BC":
                    ShowZoneMovePopup(((ComLib.UC_Zone_BC)(((Control)sender).Parent)).ZoneCD);
                    break;
                case "ComLib.UC_Zone_BS":
                    ShowZoneMovePopup(((ComLib.UC_Zone_BS)(((Control)sender).Parent)).ZoneCD);
                    break;
                case "ComLib.UC_Zone_BS_R":
                    ShowZoneMovePopup(((ComLib.UC_Zone_BS_R)(((Control)sender).Parent)).ZoneCD);
                    break;
                case "ComLib.UC_Zone_BS_BL":
                    ShowZoneMovePopup(((ComLib.UC_Zone_BS_BL)(((Control)sender).Parent)).ZoneCD);
                    break;
                case "ComLib.UC_Zone_BSO":
                    ShowZoneMovePopup(((ComLib.UC_Zone_BSO)(((Control)sender).Parent)).ZoneCD);
                    break;
                default:
                    break;
            }
            
        }

        private void InitGrid()
        {
            DrawMain1Grid(grdMain1);

            DrawMain2Grid(grdMain2);

            GrdDataClear1_2();

            DrawMain3Grid(grdMain3);

        }

        delegate void SetGridRowSelectCallback(C1FlexGrid grid, int selectrow);
        private void SetGridRowSelect(C1FlexGrid grid, int selectrow)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (grid.InvokeRequired)
            {
                SetGridRowSelectCallback d = new SetGridRowSelectCallback(SetGridRowSelect);
                grid.Invoke(d, new object[] { grid, selectrow });
            }
            else
            {
                grid.Row = selectrow;
            }
        }

        delegate void AsyncSetGridBindingCallback(C1FlexGrid grid, DataTable dt);
        private void AsyncSetGridBinding(C1FlexGrid grid, DataTable dt)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (grid.InvokeRequired)
            {
                AsyncSetGridBindingCallback d = new AsyncSetGridBindingCallback(AsyncSetGridBinding);
                grid.Invoke(d, new object[] { grid, dt });
            }
            else
            {
                DrawGrid(grid, dt);
            }
        }

        public void ReFresh(string _content)
        {
            if (this.InvokeRequired)
            {
                ReFreshDelegate r = new ReFreshDelegate(ReFresh);
                this.Invoke(r, new object[] { "" });
            }
            else UpdateUIData();
        }

        public void SetLabelVisible(Label lb, bool isvisible)
        {
            if (this.InvokeRequired)
            {
                SetLabelVisibleDelegate r = new SetLabelVisibleDelegate(SetLabelVisible);
                this.Invoke(r, new object[] { lb, isvisible });
            }
            else SetWarningVisible(lb, isvisible);
        }



        private void SetWarningVisible(Label lb, bool isvisible)
        {
            lb.Visible = isvisible;
        }

        private void SetWarningVisible()
        {
            if (!isChipWaring)
            {
                lbChipChange.Visible = false;

            }

            if (!isRockWaring)
            {
                lbRockChange.Visible = false;
            }
        }

        private void DrawMain2Grid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                int _GridRowsCount = main2_GridRowsCount;
                int _GridColsCount = main2_GridColsCount;
                int _RowsFixed = main2_RowsFixed;
                int _RowsFrozen = main2_RowsFrozen;
                int _ColsFixed = main2_ColsFixed;
                int _ColsFrozen = main2_ColsFrozen;
                int _TopRowsHeight = main2_TopRowsHeight;
                int _DataRowsHeight = main2_DataRowsHeight;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                Flexmain2GridCol(grdItem);
                //grdItem.AllowMerging = AllowMergingEnum.Free;


                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, _TopRowsHeight, _DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;


            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                //MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndInit();
                grdItem.Invalidate();
            }
        }

        private void Flexmain2GridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 240;
            grdItem.Cols[1].Width = 100;


            //컬럼 명 세팅
            grdItem[1, 0] = "그라인딩 모터 전력( kW )";
            grdItem[2, 0] = "연마지석 직경( mm )";
            grdItem[3, 0] = "연마지석 속도( m/s )";
            grdItem[4, 0] = "휠 각도( ° )";
            grdItem[5, 0] = "대차 속도( m/min )";


            grdItem.AllowEditing = false;

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 5, 0);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 1, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 2, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 3, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 4, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 5, 0);
        }

        private void DrawMain1Grid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                int _GridRowsCount = main1_GridRowsCount;
                int _GridColsCount = main1_GridColsCount;
                int _RowsFixed = main1_RowsFixed;
                int _RowsFrozen = main1_RowsFrozen;
                int _ColsFixed = main1_ColsFixed;
                int _ColsFrozen = main1_ColsFrozen;
                int _TopRowsHeight = main1_TopRowsHeight;
                int _DataRowsHeight = main1_DataRowsHeight;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                Flexmain1GridCol(grdItem);
                //grdItem.AllowMerging = AllowMergingEnum.Free;


                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, _TopRowsHeight, _DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                //SetGridRowSelect(grdItem, -1);
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                //MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndInit();
                grdItem.Invalidate();
            }
        }

        private void Flexmain1GridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 240;
            grdItem.Cols[1].Width = 100;


            //컬럼 명 세팅
            grdItem[1, 0] = "설정 그라인딩 Force( N )";
            grdItem[2, 0] = "그라인딩 Force 측정값( N )";
            grdItem[3, 0] = "그라인딩 압력 면( N )";
            grdItem[4, 0] = "그라인딩 압력 코너( N )";
            grdItem[5, 0] = "그라인딩 압력 끝( N )";


            grdItem.AllowEditing = false;

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 5, 0);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 1, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 2, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 3, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 4, 0);
            //clsFlexGrid.DataGridCenterStyle(grdItem, 5, 0);
 
        }

        private void DrawMain3Grid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                int _GridRowsCount = main3_GridRowsCount;
                int _GridColsCount = main3_GridColsCount;
                int _RowsFixed = main3_RowsFixed;
                int _RowsFrozen = main3_RowsFrozen;
                int _ColsFixed = main3_ColsFixed;
                int _ColsFrozen = main3_ColsFrozen;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                Flexmain3GridCol(grdItem);
                //grdItem.AllowMerging = AllowMergingEnum.Free;


                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;


            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                //MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndInit();
                grdItem.Invalidate();
            }
        }

        private void Flexmain3GridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 120;
            grdItem.Cols[1].Width = 120;
            grdItem.Cols[2].Width = 120;
            grdItem.Cols[3].Width = 120;
            grdItem.Cols[4].Width = 120;
            grdItem.Cols[5].Width = 120;
            grdItem.Cols[6].Width = 120;
            grdItem.Cols[7].Width = 120;
            grdItem.Cols[8].Width = 120;
            grdItem.Cols[9].Width = 120;
            grdItem.Cols[10].Width = 120;
            grdItem.Cols[11].Width = 120;
            grdItem.Cols[12].Width = 120;
            
            //컬럼 명 세팅
            grdItem[1, 0] = "HEAT";
            grdItem[1, 1] = "강종";
            grdItem[1, 2] = "규격";
            grdItem[1, 3] = "그라인딩";
            grdItem[1, 4] = "본수";
            grdItem[1, 5] = "재작업본수";
            grdItem[1, 6] = "PASS 수";
            grdItem[1, 7] = "PASS 수";
            grdItem[1, 8] = "압력( N )";
            grdItem[1, 9] = "압력( N )";
            grdItem[1, 10] = "압력( N )";
            grdItem[1, 11] = "휠각도( ° )";
            grdItem[1, 12] = "대차속도( m/min )";



            grdItem[2, 0] = "HEAT";
            grdItem[2, 1] = "강종";
            grdItem[2, 2] = "규격";
            grdItem[2, 3] = "그라인딩";
            grdItem[2, 4] = "본수";
            grdItem[2, 5] = "재작업본수";
            grdItem[2, 6] = "면";
            grdItem[2, 7] = "코너";
            grdItem[2, 8] = "면";
            grdItem[2, 9] = "코너";
            grdItem[2, 10] = "끝";
            grdItem[2, 11] = "휠각도( ° )";
            grdItem[2, 12] = "대차속도( m/min )";
            //grdItem.Cols[36].Visible = false;
            //grdItem.Cols[37].Visible = false;

            grdItem.AllowMerging = AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdItem.Cols.Count; i++)
            {
                grdItem.Cols[i].AllowMerging = true;

            }

            grdItem.Rows[1].AllowMerging = true;

            grdItem.AllowEditing = false;

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 2, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 0);
            clsFlexGrid.DataGridCenterStyle(grdItem, 1, 3);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 4, 5);
            clsFlexGrid.DataGridCenterStyle(grdItem, 6, grdItem.Cols.Count - 1);




        }



        private void IsConnected_TextChanged(object sender, EventArgs e)
        {
            if (IsConnected.Text == "Connected")
            {
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + " 데이터베이스 접속되었습니다.");
            }
            else
            {
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + " 데이터베이스 접속이 해제되었습니다.");
            }
        }

        private void AsyncUpdateUIData()
        {
            int test = 0;
            while (!IsDisposed)
            {
                // do something
                //Text = DateTime.Now.ToString();
                sw.Reset();
                sw.Start();
                //Console.WriteLine("update start"+DateTime.Now.ToString());
                UpdateUIData();
                //Text = DateTime.Now.ToString();
                sw.Stop();
                //Console.WriteLine("update Elapsed:" + sw.Elapsed.TotalMilliseconds);
                //Console.WriteLine("update Time:" + DateTime.Now);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        private void UpdateUIData()
        {
            if (!cd.CheckDbConnection())
            {
                IsConnected.Text = "DisConnected";
                this.Enable(false);
                return;
            }


            //InitGrd();
            this.Enable(true);
            IsConnected.Text = "Connected";


            //cd.CheckDbConnection();
            //현재의 근조 표시
            //SetWorkBinding(line_gp, uC_WorkShow1);

            SetDataBinding();

            SetDataBinding_Zone();

            SetWarningChange();

            Invalidate();
        }

        private void SetWarningChange()
        {

            Tuple<bool, bool, int> warningData = new Tuple<bool, bool, int>(false, false, 0);
            warningData = GetWarningData();

            // 교체중으로 변경되면.
            if (!isChipWaring && warningData.Item1)
            {
                isChipWaring = warningData.Item1;
                LbChipBlink(lbChipChange);
            }

            if (!isRockWaring && warningData.Item2)
            {
                isRockWaring = warningData.Item2;
                LbRockBlink(lbRockChange);
            }

            uC_KeyValue_S01.PLC_ITEM_VALUE = warningData.Item3.ToString();


            isChipWaring = warningData.Item1;
            isRockWaring = warningData.Item2;

        }

        private Tuple<bool, bool, int> GetWarningData()
        {
            Tuple<bool, bool, int> returnData = new Tuple<bool, bool, int>(false, false, 0);
            try
            {


                string sql = string.Format(@"SELECT TOP 1 
                                                    ISNULL(GRD_CHIP_REMV_YN, 'N') AS GRD_CHIP_REMV_YN
                                                  , ISNULL(GRD_RBGW_CHG_YN, 'N') AS GRD_RBGW_CHG_YN
												  , ISNULL(GRD_RBGW_USE_CNT,0) AS GRD_RBGW_USE_CNT
                                               FROM TB_GRD_RBGW_MGMT");

                olddt_sub = cd.FindDataTable(sql);
                moddt_sub = olddt_sub.Copy();
                if (moddt_sub.Rows.Count > 0)
                {
                    returnData = new Tuple<bool, bool, int>( vf.StringToBool(moddt_sub.Rows[0]["GRD_CHIP_REMV_YN"].ToString())
                                                      , vf.StringToBool(moddt_sub.Rows[0]["GRD_RBGW_CHG_YN"].ToString())
                                                      , int.Parse(moddt_sub.Rows[0]["GRD_RBGW_USE_CNT"].ToString())
                                                      );
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            
            return returnData;
        }

        private void SetDataBinding()
        {
            SetDataBinding_grdMain1_2();
            SetDataBinding_grdMain3();
        }

        private void GrdDataClear1_2()
        {
            grdMain1.SetData(1, 1, "");
            grdMain1.SetData(2, 1, "");
            grdMain1.SetData(3, 1, "");
            grdMain1.SetData(4, 1, "");
            grdMain1.SetData(5, 1, "");

            grdMain2.SetData(1, 1, "");
            grdMain2.SetData(2, 1, "");
            grdMain2.SetData(3, 1, "");
            grdMain2.SetData(4, 1, "");
            grdMain2.SetData(5, 1, "");
        }

        

        private void DrawGrid(C1FlexGrid grdItem, DataTable dataTable)
        {

            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);
                //grdItem.SetDataBinding(dataTable, null, true);
                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, main3_DataRowsHeight);



                //마지막행 사이즈조절, 로우공백흰색
                grdItem.ExtendLastCol = true;
                grdItem.Styles.EmptyArea.BackColor = Color.White;

                grdItem.SelectionMode = SelectionModeEnum.Row;
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
                //clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                //MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndUpdate();
                grdItem.Invalidate();
            }

        }

        private void SetDataBinding_Zone()
        {

            //깜빡임 방지.
            //InitZ00();

            //InitZ91();

            //InitZ99();

            SetZ00();

            SetZ60();

            SetZ91();

            SetZ99();



        }

        private void SetZ60()
        {
            olddt = new DataTable();
            string rowNo = "";
            string blData = "";
            string blCntData = "";
            try
            {


                string sql = "";
                sql += string.Format(@"WITH Q AS ( SELECT 1 AS L_NUM UNION ALL SELECT L_NUM + 1 FROM q WHERE L_NUM < 8 ) 

                                        SELECT Q.L_NUM
                                             , ISNULL(B.BLOOM_NO, '') AS BLOOM_NO
                                        	 , ISNULL(B.ZONE_CD, '')  AS ZONE_CD
											 , ISNULL(B.BL_CNT, '')   AS BL_CNT
                                          FROM Q 
                                               LEFT OUTER JOIN 
                                        	  (
                                        			SELECT TOP 8
                                        					   ROW_NUMBER()  OVER(ORDER BY A.ZONE_CD_UPD_DDTT, B.WORK_PLN_DATE, A.HEAT_NO, A.WORK_SEQ)       AS L_NUM
                                        					 , A.HEAT_NO +'  '+ CONVERT(VARCHAR(3), A.HEAT_SEQ)            AS BLOOM_NO
                                        					 , A.ZONE_CD
															 , COUNT(*) OVER (PARTITION BY A.ZONE_CD)  AS BL_CNT
                                        				  FROM TB_RL_TM_TRACKING A
                                        					 , TB_SCHEDULE B
                                        				 WHERE A.PROG_STAT  IN ('WAT', 'RUN')
                                        				   AND A.HEAT_NO = B.HEAT_NO 
                                        				   AND A.ZONE_CD = 'Z60'
                                        				   AND B.WORK_STAT    <> 'END' 
                                        			  ORDER BY B.WORK_PLN_DATE, B.WORK_SEQ, A.HEAT_NO, A.WORK_SEQ 
                                        	   ) B
                                        	   ON Q.L_NUM = B.L_NUM ;  ");


                olddt = cd.FindDataTable(sql);


                for (int row = 0; row < olddt.Rows.Count; row++)
                {
                    //uC_Zone_BS_R2.BloomNo0 = "test";
                    rowNo = olddt.Rows[row]["L_NUM"].ToString();
                    blData = olddt.Rows[row]["BLOOM_NO"].ToString();
                    var lastblData = uC_Zone_BS_BL1.GetType().GetProperty("BL_DATA" + rowNo).GetValue(uC_Zone_BS_BL1).ToString();

                    if (!lastblData.Equals(blData))
                    {
                        uC_Zone_BS_BL1.GetType().GetProperty("BL_DATA" + rowNo).SetValue(uC_Zone_BS_BL1, blData, null);
                    }

                    if (row == 0)
                    {
                        blCntData = olddt.Rows[row]["BL_CNT"].ToString();
                        var lastblCntData = uC_Zone_BS_BL1.GetType().GetProperty("TOTAL_CNT").GetValue(uC_Zone_BS_BL1).ToString();

                        if (!lastblCntData.Equals(blData))
                        {
                            uC_Zone_BS_BL1.GetType().GetProperty("TOTAL_CNT").SetValue(uC_Zone_BS_BL1, blCntData, null);
                        }
                    }
                    
                    //getPropertyValueFromProperty(uC_Zone_BS_R2, olddt.Rows[row]["L_NUM"].ToString());
                }

                //this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                //DrawGrid(grdMain3, moddtMain3);
                //this.Cursor = Cursors.Default;

                //SetGridRowSelect(grdMain3, -1);
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void SetZ91()
        {
            olddt = new DataTable();
            string rowNo = "";
            string HeatData = "";
            string lastHeatData = "";
            string bloomCnt = "";
            string lastBloomCnt = "";
            try
            {


                string sql = "";
            //    sql += string.Format(@"   WITH Q AS ( SELECT 0 AS L_NUM UNION ALL SELECT L_NUM + 1 FROM q WHERE L_NUM < 7 ) 

            //                            SELECT Q.L_NUM
            //                                 , ISNULL(B.BLOOM_NO, '') AS BLOOM_NO
            //                            	 , ISNULL(B.ZONE_CD, '')  AS ZONE_CD
											 //, MAX(ISNULL(B.CNT, 0   )) OVER ()      AS CNT
            //                              FROM Q 
            //                                   LEFT OUTER JOIN 
            //                            	  (
												//	 SELECT  TOP 8
												//		  ROW_NUMBER()  OVER(ORDER BY A.HEAT_NO, A.WORK_SEQ DESC) -1       AS L_NUM
												//		, A.HEAT_NO +'  '+ CONVERT(VARCHAR(3), A.HEAT_SEQ)            AS BLOOM_NO
												//		, A.ZONE_CD 
												//		, COUNT(*) OVER () AS CNT
												//	FROM  TB_RL_TM_TRACKING A
												//   WHERE  A.PROG_STAT   = 'RUN' 
												//	 AND  A.ZONE_CD = 'Z91' 
            //                            	   ) B
            //                            	   ON Q.L_NUM = B.L_NUM
            //                            	   ORDER BY Q.L_NUM ");

                sql += string.Format(@" WITH Q AS ( SELECT 1 AS L_NUM UNION ALL SELECT L_NUM + 1 FROM q WHERE L_NUM < 5 ) 

									     SELECT Q.L_NUM
									          , ISNULL(E.DATA, '') AS DATA
									       FROM Q
									            LEFT OUTER JOIN
									     		(
									     			SELECT  ROW_NUMBER()  OVER(ORDER BY D.SCH_WORK_PLN_DATE DESC , D.SCH_WORK_SEQ DESC )  AS L_NUM
									     			     ,  D.HEAT_NO
									     				 ,  D.SCH_WORK_PLN_DATE
									     				 ,  D.SCH_WORK_SEQ
									     				 ,  D.RUN_CNT
									     				 ,  D.TOTAL_CNT
														 ,  D.HEAT_NO +' '+ D.RUN_CNT + '/'+D.TOTAL_CNT AS DATA
									     			  FROM  (
									     						SELECT TOP 5 C.HEAT_NO
									     						           , C.SCH_WORK_PLN_DATE
									     								   , C.SCH_WORK_SEQ  
									     								   , CONVERT(VARCHAR,MAX(C.TOTAL_CNT)) AS TOTAL_CNT
									     								   , MAX(C.RUN_CNT) AS RUN_CNT_INT
									     								   , CASE WHEN LEN(MAX(C.RUN_CNT)) = 1 THEN ' ' +CONVERT(VARCHAR,MAX(C.RUN_CNT)) ELSE CONVERT(VARCHAR,MAX(C.RUN_CNT)) END AS RUN_CNT
									     								FROM (
									     										  SELECT B.HEAT_NO
									     											   , A.PROG_STAT
									     											   , B.WORK_PLN_DATE AS SCH_WORK_PLN_DATE
									     											   , B.WORK_SEQ      AS SCH_WORK_SEQ
									     											   , A.HEAT_NO       AS TRK_HEAT_NO
									     											   , A.WORK_SEQ      AS TRK_WORK_SEQ
									     											   , COUNT(*) OVER (PARTITION BY B.HEAT_NO)     AS TOTAL_CNT
									     											   , COUNT(CASE WHEN A.PROG_STAT = 'RUN' AND A.ZONE_CD  = 'Z91' THEN 1 END) OVER (PARTITION BY B.HEAT_NO,A.PROG_STAT) AS RUN_CNT
									     											FROM TB_RL_TM_TRACKING A
									     											   , TB_SCHEDULE B
									     										   WHERE A.HEAT_NO = B.HEAT_NO
									     											 AND B.WORK_STAT    <> 'END'
																					 --AND A.ZONE_CD  = 'Z91'
									     											 --AND A.HEAT_NO = '4D8235'
									     								 ) C
									     							  --WHERE C.PROG_STAT = 'RUN'
									     							GROUP BY C.HEAT_NO
									     							       , C.SCH_WORK_PLN_DATE
									     								   , C.SCH_WORK_SEQ 
									     					   ) D
									     		) E
									     	   ON Q.L_NUM = E.L_NUM   ");


                olddt = cd.FindDataTable(sql);

                for (int row = 0; row < olddt.Rows.Count; row++)
                {
                    //uC_Zone_BS2.BloomNo0 = "test";
                    rowNo = olddt.Rows[row]["L_NUM"].ToString();
                    HeatData = olddt.Rows[row]["DATA"].ToString();
                    lastHeatData = uC_Zone_BS2.GetType().GetProperty("HEAT_DATA" + rowNo).GetValue(uC_Zone_BS2).ToString();

                    //uC_Zone_BSO1.GetType().GetProperty("BloomNo" + rowNo).SetValue(uC_Zone_BSO1, bloomNo, null);
                    //uC_Zone_BSO1.GetType().GetProperty("BloomCnt" ).SetValue(uC_Zone_BSO1, bloomCnt, null);
                    //getPropertyValueFromProperty(uC_Zone_BS2, olddt.Rows[row]["L_NUM"].ToString());

                    if (!lastHeatData.Equals(HeatData))
                    {
                        uC_Zone_BS2.GetType().GetProperty("HEAT_DATA" + rowNo).SetValue(uC_Zone_BS2, HeatData, null);
                    }

                }

                //this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                //DrawGrid(grdMain3, moddtMain3);
                //this.Cursor = Cursors.Default;

                //SetGridRowSelect(grdMain3, -1);
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void InitZ91()
        {
            
            uC_Zone_BS1.HEAT_DATA1 = "";
            uC_Zone_BS1.HEAT_DATA2 = "";
            uC_Zone_BS1.HEAT_DATA3 = "";
            uC_Zone_BS1.HEAT_DATA4 = "";
            uC_Zone_BS1.HEAT_DATA5 = "";

        }
        List<Zone> ZoneList;
        private void SetZ99()
        {
            olddt = new DataTable();

            MakeZoneList();

            try
            {


                string sql = "";
                sql += string.Format(@"    WITH BASE AS
                                          (
                                          	 SELECT A.ZONE_CD
                                          	      , A.ZONE_RANK
                                          		  , A.BLOOM_NO
                                          	   FROM (
                                          				 SELECT A.ZONE_CD
                                          					  , A.HEAT_NO +'  '+ CONVERT(VARCHAR(3), A.HEAT_SEQ) AS BLOOM_NO
                                          					  , RANK()  OVER(PARTITION BY A.ZONE_CD ORDER BY A.ZONE_CD_UPD_DDTT ASC, A.HEAT_NO ASC, A.HEAT_SEQ ASC ) AS ZONE_RANK
                                          					FROM  TB_RL_TM_TRACKING A
                                          				   WHERE  A.PROG_STAT   = 'RUN'
                                          			) A
                                          )
                                          SELECT B.ZONE_CD
                                               , C.BLOOM_NO
                                          	 , B.CNT
                                          	 , C.ZONE_RANK
                                            FROM
                                               (
                                          		SELECT ZONE_CD 
                                          		     --, CASE ZONE_CD WHEN 'Z90' THEN MAX(ZONE_RANK) 
                                          			 --               ELSE MIN(ZONE_RANK) END AS GUBUN_RANK
                                                     , MIN(ZONE_RANK) AS GUBUN_RANK
                                          			 , COUNT(*) AS CNT
                                          		  FROM BASE
                                          		GROUP BY ZONE_CD
                                               ) B
                                          	 ,BASE C
                                           WHERE B.ZONE_CD = C.ZONE_CD
                                             AND B.GUBUN_RANK = C.ZONE_RANK  ");


                //sql += string.Format(@"   SELECT  A.ZONE_CD
                //                               ,  MIN(A.HEAT_NO +'  '+ CONVERT(VARCHAR(3), A.HEAT_SEQ)) AS BLOOM_NO
                //                          	   ,  COUNT(*) AS CNT
                //                            FROM  TB_RL_TM_TRACKING A
                //                           WHERE  A.PROG_STAT   = 'RUN'
                //                          GROUP BY ZONE_CD  ");

                olddt = cd.FindDataTable(sql);
                moddt = olddt.Copy();

                if (moddt != null)
                {

                    SetNewDataToZoneData(moddt, ZoneList);

                    SetZoneDataToZone(ZoneList);

                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void SetZoneDataToZone(List<Zone> _zoneList)
        {
            var lastBloomNo = string.Empty;
            var lastBloomCnt = string.Empty;
            var NewBloomNo = string.Empty;
            var NewBloomCnt = string.Empty;

            foreach (var zoneNewData in _zoneList)
            {
                foreach (var item in vf.GetAllChildrens(this))
                {
                    if (item.GetType().ToString() == zoneNewData.UC_Name)
                    {
                        if (zoneNewData.UC_Name.Equals("ComLib.UC_Zone_B"))
                        {
                            var uc_zone = item as UC_Zone_B;

                            if (uc_zone.ZoneCD.Equals(zoneNewData.ZoneCD))
                            {
                                lastBloomNo = uc_zone.BloomNo.ToString();
                                lastBloomCnt = uc_zone.BloomCnt.ToString();

                                NewBloomNo = zoneNewData.BloomNo;
                                NewBloomCnt = zoneNewData.BloomCnt;

                                if (!lastBloomNo.Equals(NewBloomNo))
                                {
                                    uc_zone.BloomNo = NewBloomNo;
                                }

                                if (!lastBloomCnt.Equals(NewBloomCnt))
                                {
                                    uc_zone.BloomCnt = NewBloomCnt;
                                }
                            }
                            
                        }

                        if (zoneNewData.UC_Name.Equals("ComLib.UC_Zone_BC"))
                        {
                            var uc_zone = item as UC_Zone_BC;

                            if (uc_zone.ZoneCD.Equals(zoneNewData.ZoneCD))
                            {
                                lastBloomNo = uc_zone.BloomNo.ToString();
                                lastBloomCnt = uc_zone.BloomCnt.ToString();

                                NewBloomNo = zoneNewData.BloomNo;
                                NewBloomCnt = zoneNewData.BloomCnt;

                                if (!lastBloomNo.Equals(NewBloomNo))
                                {
                                    uc_zone.BloomNo = NewBloomNo;
                                }

                                if (!lastBloomCnt.Equals(NewBloomCnt))
                                {
                                    uc_zone.BloomCnt = NewBloomCnt;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MakeZoneList()
        {
            ZoneList = new List<Zone>();
            Zone zone = null;

            foreach (var item1 in vf.GetAllChildrens(this))
            {
                zone = new Zone();

                if (item1.GetType().ToString() == "ComLib.UC_Zone_B")
                {
                    var uc_zone = item1 as UC_Zone_B;
                    //ListZoneUC_Zone_B.Add(uc_zone.ZoneCD);

                    zone.UC_Name = uc_zone.GetType().ToString();
                    zone.ZoneCD = uc_zone.ZoneCD;
                    zone.BloomNo = string.Empty;
                    zone.BloomCnt = string.Empty;
                    ZoneList.Add(zone);
                }

                if (item1.GetType().ToString() == "ComLib.UC_Zone_BC")
                {
                    var uc_zone = item1 as UC_Zone_BC;
                    //ListZoneUC_Zone_BC.Add(uc_zone.ZoneCD);
                    zone.UC_Name = uc_zone.GetType().ToString();
                    zone.ZoneCD = uc_zone.ZoneCD;
                    zone.BloomNo = string.Empty;
                    zone.BloomCnt = string.Empty;
                    ZoneList.Add(zone);

                    //}
                }
            }
        }

        private void SetNewDataToZoneData(DataTable _moddt, List<Zone> _zoneList)
        {
            foreach (DataRow row in _moddt.Rows)
            {
                foreach (var zoneItem in _zoneList)
                {
                    if (zoneItem.ZoneCD.Equals(row["ZONE_CD"].ToString()))
                    {
                        zoneItem.BloomNo = row["BLOOM_NO"].ToString();
                        zoneItem.BloomCnt = row["CNT"].ToString();
                    }
                }
            }
        }

        private void SetZ00()
        {

            olddt = new DataTable();
            string rowNo = "";
            string heatData = "";

            var _uc = uC_Zone_BS1 as UC_Zone_BS;
            try
            {


                string sql = "";
                sql += string.Format(@"  WITH Q AS ( SELECT 1 AS L_NUM UNION ALL SELECT L_NUM + 1 FROM q WHERE L_NUM < 5 ) 

                                        SELECT AA.L_NUM
                                                , AA.DATA
	                                            , BB.BLOOM_NO
	                                            , BB.ZONE_CD
                                            FROM (
		                                        SELECT Q.L_NUM
			                                        , ISNULL(E.DATA, '') AS DATA
		                                        FROM Q
			                                        LEFT OUTER JOIN
			                                        (
				                                        SELECT  ROW_NUMBER()  OVER(ORDER BY D.SCH_WORK_PLN_DATE  , D.SCH_WORK_SEQ  )  AS L_NUM
						                                        ,  D.HEAT_NO
						                                        ,  D.SCH_WORK_PLN_DATE
						                                        ,  D.SCH_WORK_SEQ
						                                        ,  D.RUN_CNT
						                                        ,  D.TOTAL_CNT
						                                        ,  D.HEAT_NO +' '+ D.RUN_CNT + '/'+D.TOTAL_CNT AS DATA
					                                        FROM  (
							                                        SELECT TOP 5 C.HEAT_NO
									                                            , C.SCH_WORK_PLN_DATE
									                                            , C.SCH_WORK_SEQ  
									                                            , CONVERT(VARCHAR,MAX(C.TOTAL_CNT)) AS TOTAL_CNT
									                                            , MAX(C.RUN_CNT) AS RUN_CNT_INT
										                                        , CASE WHEN LEN(MAX(C.RUN_CNT)) = 1 THEN ' ' +CONVERT(VARCHAR,MAX(C.RUN_CNT)) ELSE CONVERT(VARCHAR,MAX(C.RUN_CNT)) END AS RUN_CNT
									                                            --, C.HEAT_NO +' ' +  CONVERT(VARCHAR,MAX(C.RUN_CNT))  +'/'+ CONVERT(VARCHAR,MAX(C.TOTAL_CNT))  AS DATA
									                                        FROM (
									     		                                        SELECT B.HEAT_NO
									     			                                        , A.PROG_STAT
									     			                                        , B.WORK_PLN_DATE AS SCH_WORK_PLN_DATE
									     			                                        , B.WORK_SEQ      AS SCH_WORK_SEQ
									     			                                        , A.HEAT_NO       AS TRK_HEAT_NO
									     			                                        , A.WORK_SEQ      AS TRK_WORK_SEQ
									     			                                        , COUNT(*) OVER (PARTITION BY B.HEAT_NO)     AS TOTAL_CNT
									     			                                        , COUNT(CASE WHEN  A.ZONE_CD  = 'Z00' THEN 1 END) OVER (PARTITION BY B.HEAT_NO,A.PROG_STAT) AS RUN_CNT
									     		                                        FROM TB_RL_TM_TRACKING A
									     			                                        , TB_SCHEDULE B
									     		                                        WHERE A.HEAT_NO = B.HEAT_NO
									     			                                        AND B.WORK_STAT    <> 'END'
													                                        --AND A.ZONE_CD  = 'Z91'
									     			                                        --AND A.HEAT_NO = '4D8235'
									                                            ) C
									                                        --WHERE C.PROG_STAT = 'RUN'
								                                        GROUP BY C.HEAT_NO
									                                            , C.SCH_WORK_PLN_DATE
									                                            , C.SCH_WORK_SEQ 
							                                        ) D
			                                        ) E
			                                        ON Q.L_NUM = E.L_NUM 
	                                            ) AA
                                                ,
	                                            (
		                                            SELECT Q.L_NUM
                                                                                        , ISNULL(B.BLOOM_NO, '') AS BLOOM_NO
                                        	                                            , ISNULL(B.ZONE_CD, '')  AS ZONE_CD
                                                                                    FROM Q 
                                                                                        LEFT OUTER JOIN 
                                        	                                            (
                                        			                                        SELECT TOP 8
                                        					                                            ROW_NUMBER()  OVER(ORDER BY B.WORK_PLN_DATE, B.WORK_SEQ, A.HEAT_NO, A.WORK_SEQ) -1       AS L_NUM
                                        					                                            , A.HEAT_NO +'  '+ CONVERT(VARCHAR(3), A.HEAT_SEQ)            AS BLOOM_NO
                                        					                                            , A.ZONE_CD 
                                        				                                            FROM TB_RL_TM_TRACKING A
                                        					                                            , TB_SCHEDULE B
                                        				                                            WHERE A.PROG_STAT  IN ('WAT')
                                        				                                            AND A.HEAT_NO = B.HEAT_NO 
                                        				                                            AND A.ZONE_CD = 'Z00'
                                        				                                            AND B.WORK_STAT    <> 'END' 
                                        			                                            ORDER BY B.WORK_PLN_DATE, B.WORK_SEQ, A.HEAT_NO, A.WORK_SEQ 
                                        	                                            ) B
                                        	                                            ON Q.L_NUM = B.L_NUM
	                                            ) BB
                                        WHERE  AA.L_NUM = BB.L_NUM  ");


                olddt = cd.FindDataTable(sql);


                for (int row = 0; row < olddt.Rows.Count; row++)
                {
                    //uC_Zone_BS1.BloomNo0 = "test";
                    rowNo = olddt.Rows[row]["L_NUM"].ToString();
                    heatData = olddt.Rows[row]["DATA"].ToString();
                    var lastHeatData = _uc.GetType().GetProperty("HEAT_DATA" + rowNo).GetValue(_uc).ToString();

                    if (!lastHeatData.Equals(heatData))
                    {
                        _uc.GetType().GetProperty("HEAT_DATA" + rowNo).SetValue(_uc, heatData, null);
                    }

                    if (row == 0)
                    {
                        var lastBloomData = _uc.GetType().GetProperty("BloomNo").GetValue(_uc).ToString();
                        var newBloomData = olddt.Rows[row]["BLOOM_NO"].ToString();
                        if (!lastBloomData.Equals(newBloomData))
                        {
                            _uc.GetType().GetProperty("BloomNo").SetValue(_uc, newBloomData, null);
                        }
                    }

                    //getPropertyValueFromProperty(uC_Zone_BS1, olddt.Rows[row]["L_NUM"].ToString());
                }

                //this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                //DrawGrid(grdMain3, moddtMain3);
                //this.Cursor = Cursors.Default;

                //SetGridRowSelect(grdMain3, -1);
            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        private void SetDataBinding_grdMain3()
        {
            //clsFlexGrid.grdDataClear(grdMain3, main3_GridRowsCount);

            string sql = "";
            sql += string.Format(@"  SELECT     TOP 4 A.HEAT_NO
                                                ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
                                                ,(SELECT CD_NM         FROM TB_CM_COM_CD           WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = A.ITEM_SIZE_A) AS ITEM_SIZE_NM
                                                ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GRD_DRV_MODE' AND CD_ID = A.GRD_DRV_MODE) AS GRD_NM
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.CNT                    AS MONEY), 1) , '.00', '')   AS CNT                    
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.RE_CNT					AS MONEY), 1) , '.00', '')   AS RE_CNT					
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.FACE_PASS_CNT			AS MONEY), 1) , '.00', '')   AS FACE_PASS_CNT			
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.CORN_PASS_CNT			AS MONEY), 1) , '.00', '')   AS CORN_PASS_CNT			
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.GRD_PRES_SURFACE_SETV	AS MONEY), 1) , '.00', '')   AS GRD_PRES_SURFACE_SETV	
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.GRD_PRES_CORN_SETV		AS MONEY), 1) , '.00', '')   AS GRD_PRES_CORN_SETV		
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.GRD_PRES_ENDP_SETV		AS MONEY), 1) , '.00', '')   AS GRD_PRES_ENDP_SETV		
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.ROTANGLE_MV			AS MONEY), 1) , '.00', '')   AS ROTANGLE_MV			
                                                ,REPLACE(CONVERT(VARCHAR(50), CAST(A.TRUCKSPEED_MV			AS MONEY), 1) , '.00', '')   AS TRUCKSPEED_MV			
                                         FROM    (
                                                     SELECT  HEAT_NO
                                                            ,MAX(STEEL)                                      AS STEEL  
                                                            ,MAX(ITEM + ITEM_SIZE)                           AS ITEM_SIZE_A
                                                            ,MAX(GRD_DRV_MODE)                               AS GRD_DRV_MODE
                                                            ,SUM(case when REWORK_SNO = 0 then 1 else 0 end) AS CNT    --본수
                                                            ,SUM(case when REWORK_SNO > 0 then 1 else 0 end) AS RE_CNT --재작업본수
                                                            ,ISNULL(MAX(FACE_PASS_CNT), 0)                   AS FACE_PASS_CNT  --면 (PASS수)
                                                            ,ISNULL(MAX(CORN_PASS_CNT), 0)                   AS CORN_PASS_CNT  --코너 (PASS수)
                                                            ,ISNULL(MAX(GRD_PRES_SURFACE_SETV),0)            AS GRD_PRES_SURFACE_SETV --면 (압력)
                                                            ,ISNULL(MAX(GRD_PRES_CORN_SETV)   ,0)            AS GRD_PRES_CORN_SETV    --코너 (압역)
                                                            ,ISNULL(MAX(GRD_PRES_ENDP_SETV)   ,0)            AS GRD_PRES_ENDP_SETV    --끝 (압력)
                                                            ,ISNULL(MAX(ROTANGLE_MV  )        ,0)            AS ROTANGLE_MV           --휠각도
                                                            ,ISNULL(MAX(TRUCKSPEED_MV)        ,0)            AS TRUCKSPEED_MV         --대차속도
                                                            ,MAX(GRD_START_DDTT)                             AS GRD_START_DDTT
                                                     FROM    TB_GRD_WR
                                                     WHERE   WORK_DATE  = CONVERT(VARCHAR(8), GETDATE(), 112)
                                                     GROUP BY HEAT_NO
                                                ) A  
                                        ORDER BY A.GRD_START_DDTT DESC
                                          ");


            olddtMain3 = cd.FindDataTable(sql);
            moddtMain3 = olddtMain3.Copy();

            //this.Cursor = Cursors.AppStarting;
            //조회된 데이터 그리드에 세팅
            //DrawGrid(grdMain3, moddtMain3);
            //AsyncSetGridBinding(grdMain3, moddtMain3);

            //grdMain3.SetDataBinding(moddtMain3, null, true);
            //this.Cursor = Cursors.Default;

            //grdMain3.Invalidate();
            //SetGridRowSelect(grdMain3, -1);

            if (moddtMain3 != null)
            {
                try
                {
                    //grdMain3.SetDataBinding(moddtMain3, null, true);

                    AsyncSetGridBinding(grdMain3, moddtMain3);
                }
                catch (Exception)
                {

                    return;
                }
                //grdMain9.SetDataBinding(moddt, null, true);

                // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
                //grdMain_Row_Selected(grdMain.Row);
                //grdMain9.Row = -1;
                //grdMain3.Invalidate();
            }
            SetGridRowSelect(grdMain3, -1);
        }
        private void SetDataBinding_grdMain1_2()
        {
            //GrdDataClear1_2();

            try
            {


                string sql = string.Format(@"SELECT REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(SET_GRD_Force        ,'')AS MONEY), 1) , '.00', '')   AS SET_GRD_Force        --설정그라인딩Force
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_MV          ,'') AS MONEY), 1) , '.00', '')   AS GRD_PRES_MV           --그라인딩 Force측정값
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_SURFACE_SETV,'') AS MONEY), 1) , '.00', '')   AS GRD_PRES_SURFACE_SETV --압력 면
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_CORN_SETV   ,'') AS MONEY), 1) , '.00', '')   AS GRD_PRES_CORN_SETV    --압력 코너
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_PRES_ENDP_SETV   ,'') AS MONEY), 1) , '.00', '')   AS GRD_PRES_ENDP_SETV    --압력 끝
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(GRD_MTR_ELEC         ,'') AS MONEY), 1) , '.00', '')   AS GRD_MTR_ELEC          --그라인딩 모터 전력
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(RBGW_DIA_MV          ,'') AS MONEY), 1) , '.00', '')   AS RBGW_DIA_MV           --직경
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(RBGW_SPEED_SETV      ,'') AS MONEY), 1) , '.00', '')   AS RBGW_SPEED_SETV       --속도
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(ROTANGLE_MV          ,'') AS MONEY), 1) , '.00', '')   AS ROTANGLE_MV           --휠각도
                                                    ,REPLACE(CONVERT(VARCHAR(50), CAST(ISNULL(TRUCKSPEED_MV        ,'') AS MONEY), 1) , '.00', '')   AS TRUCKSPEED_MV         --대차속도
                                              FROM   TB_GRD_OPER_INFO
                                              WHERE  WORK_DDTT  = ( SELECT MAX(WORK_DDTT) FROM TB_GRD_OPER_INFO )");

                olddtMain1 = cd.FindDataTable(sql);
                moddtMain1 = olddtMain1.Copy();

                //this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                //DrawGrid(grdMain1, moddtMain1);
                //bool IsNewData = IsNewData("", "");


                var listForGrdMain1 = new List<KeyValuePair<int, string>>() {
                                      new KeyValuePair<int, string>(1, "SET_GRD_Force"),
                                      new KeyValuePair<int, string>(2, "GRD_PRES_MV"),
                                      new KeyValuePair<int, string>(3, "GRD_PRES_SURFACE_SETV"),
                                      new KeyValuePair<int, string>(4, "GRD_PRES_CORN_SETV"),
                                      new KeyValuePair<int, string>(5, "GRD_PRES_ENDP_SETV"),
                                  };

                foreach (var item in listForGrdMain1)
                {
                    if (!grdMain1.GetData(item.Key, 1).ToString().Equals(moddtMain1.Rows[0][item.Value].ToString()))
                    {
                        grdMain1.SetData(item.Key, 1, moddtMain1.Rows[0][item.Value].ToString());
                    }
                }

                var listForGrdMain2 = new List<KeyValuePair<int, string>>() {
                                      new KeyValuePair<int, string>(1, "GRD_MTR_ELEC"),
                                      new KeyValuePair<int, string>(2, "RBGW_DIA_MV"),
                                      new KeyValuePair<int, string>(3, "RBGW_SPEED_SETV"),
                                      new KeyValuePair<int, string>(4, "ROTANGLE_MV"),
                                      new KeyValuePair<int, string>(5, "TRUCKSPEED_MV"),
                                  };

                foreach (var item in listForGrdMain2)
                {
                    if (!grdMain2.GetData(item.Key, 1).ToString().Equals(moddtMain1.Rows[0][item.Value].ToString()))
                    {
                        grdMain2.SetData(item.Key, 1, moddtMain1.Rows[0][item.Value].ToString());
                    }
                }

                //grdMain1.SetData(1, 1, moddtMain1.Rows[0]["SET_GRD_Force"].ToString());
                //grdMain1.SetData(2, 1, moddtMain1.Rows[0]["GRD_PRES_MV"].ToString());
                //grdMain1.SetData(3, 1, moddtMain1.Rows[0]["GRD_PRES_SURFACE_SETV"].ToString());
                //grdMain1.SetData(4, 1, moddtMain1.Rows[0]["GRD_PRES_CORN_SETV"].ToString());
                //grdMain1.SetData(5, 1, moddtMain1.Rows[0]["GRD_PRES_ENDP_SETV"].ToString());

                //grdMain2.SetData(1, 1, moddtMain1.Rows[0]["GRD_MTR_ELEC"].ToString());
                //grdMain2.SetData(2, 1, moddtMain1.Rows[0]["RBGW_DIA_MV"].ToString());
                //grdMain2.SetData(3, 1, moddtMain1.Rows[0]["RBGW_SPEED_SETV"].ToString());
                //grdMain2.SetData(4, 1, moddtMain1.Rows[0]["ROTANGLE_MV"].ToString());
                //grdMain2.SetData(5, 1, moddtMain1.Rows[0]["TRUCKSPEED_MV"].ToString());
                //this.Cursor = Cursors.Default;

                grdMain1.Invalidate();
                SetGridRowSelect(grdMain1, -1);
                grdMain2.Invalidate();
                SetGridRowSelect(grdMain2, -1);
                //SetGridRowSelect(grdMain1, -1);
                //SetGridRowSelect(grdMain2, -1);
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        static string getPropertyValueFromProperty(object control, string propertyName)
        {
            var controlType = control.GetType();
            var property = controlType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property == null)
                throw new InvalidOperationException(string.Format("Property “{0}” does not exist in type “{1}”.", propertyName, controlType.FullName));
            if (property.PropertyType != typeof(string))
                throw new InvalidOperationException(string.Format("Property “{0}” in type “{1}” does not have the type “string”.", propertyName, controlType.FullName));
            return (string)property.GetValue(control, null);
        }



        private void InitZ00()
        {
            uC_Zone_BS1.HEAT_DATA1 = "";
            uC_Zone_BS1.HEAT_DATA2 = "";
            uC_Zone_BS1.HEAT_DATA3 = "";
            uC_Zone_BS1.HEAT_DATA4 = "";
            uC_Zone_BS1.HEAT_DATA5 = "";
        }

        private void InitZ99()
        {
            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "ComLib.UC_Zone_B")
                {
                    var uc_zone = item as ComLib.UC_Zone_B;
                    //zone 초기화
                    uc_zone.BloomNo = "";
                }

                if (item.GetType().ToString() == "ComLib.UC_Zone_BC")
                {
                    var uc_zone = item as ComLib.UC_Zone_BC;
                    //zone 초기화
                    uc_zone.BloomNo = "";
                    uc_zone.BloomCnt = "";
                }
            }
        }

        private void btnZoneMove_Click(object sender, EventArgs e)
        {
            string selectedZoneCD = "";
            ShowZoneMovePopup(selectedZoneCD);
            
        }

        private void ShowZoneMovePopup(string _selectedZoneCD)
        {
            var temp_form = GetForm("ZoneMovePopup");
            string selectedZoneCD = _selectedZoneCD;

            if (temp_form == null)
            {
                var sub = new Common.ZoneMovePopup("ZONE 이동 처리", "", "", selectedZoneCD);

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;

                sub.FormBorderStyle = FormBorderStyle.FixedDialog;
                // Set the MaximizeBox to false to remove the maximize box.
                sub.MaximizeBox = false;
                // Set the MinimizeBox to false to remove the minimize box.
                sub.MinimizeBox = false;
                //sub.ShowDialog();
                sub.Show();
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void btHeatFin_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("HeatFinPopup");

            if (temp_form == null)
            {
                var sub = new Common.HeatFinPopup("HEAT 종료 처리", "", "");

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;

                sub.FormBorderStyle = FormBorderStyle.FixedDialog;
                // Set the MaximizeBox to false to remove the maximize box.
                sub.MaximizeBox = false;
                // Set the MinimizeBox to false to remove the minimize box.
                sub.MinimizeBox = false;
                //sub.ShowDialog();
                sub.Show();
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }


        public static Form GetForm(string formName)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == formName)
                {
                    return frm;
                }
            }
            return null;

        }

        private void uC_ZoneMoveBtn2_Load(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReFresh("");
        }



        private void bt_BloomMgmt_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("BloomMgmtPopup");

            if (temp_form == null)
            {
                var sub = new Common.BloomMgmtPopup("BLOOM 스케줄 관리", "", "","Z00");

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;

                sub.FormBorderStyle = FormBorderStyle.FixedDialog;
                // Set the MaximizeBox to false to remove the maximize box.
                sub.MaximizeBox = false;
                // Set the MinimizeBox to false to remove the minimize box.
                sub.MinimizeBox = false;
                //sub.ShowDialog();
                sub.Show();
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }



        private void btResultReWork_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RsultReWork");

            if (temp_form == null)
            {
                var sub = new Common.ResultReWork("실적 재작업 등록", "", "", "");

                //sub.MdiParent = this;
                sub.StartPosition = FormStartPosition.CenterScreen;

                sub.FormBorderStyle = FormBorderStyle.FixedDialog;
                // Set the MaximizeBox to false to remove the maximize box.
                sub.MaximizeBox = false;
                // Set the MinimizeBox to false to remove the minimize box.
                sub.MinimizeBox = false;
                //sub.ShowDialog();
                sub.Show();
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTSHT");

            if (temp_form == null)
            {
                LoadPage("RTSHT");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void LoadPage(string _formName)
        {
            string formName = _formName;
            string itemName = "";
            string tempTag = "";
            Form parent = (Form)this.MdiParent;
            ToolStripMenuItem formItem = null;

            foreach (ToolStripMenuItem topItem in parent.MainMenuStrip.Items)
            {
                formItem = topItem as ToolStripMenuItem;
                //Console.WriteLine(formItem.Tag);
                //if (formItem.Tag == null || string.IsNullOrEmpty(formItem.Tag.ToString())) continue;
                itemName = GetItemName(formItem.Tag);
                if (itemName.Length > 0 & itemName.Equals(formName))
                {
                    formItem.PerformClick();
                }

                foreach (ToolStripMenuItem  subItem in topItem.DropDownItems)
                {
                    formItem = subItem as ToolStripMenuItem;
                    //Console.WriteLine(formItem.Tag);
                    itemName = GetItemName(formItem.Tag);
                    if (itemName.Length > 0 & itemName.Equals(formName))
                    {
                        formItem.PerformClick();
                    }

                    foreach (ToolStripMenuItem ssubItem in subItem.DropDownItems)
                    {
                        formItem = ssubItem as ToolStripMenuItem;
                        //Console.WriteLine(formItem.Tag);
                        itemName = GetItemName(formItem.Tag);
                        if (itemName.Length > 0 & itemName.Equals(formName))
                        {
                            formItem.PerformClick();
                        }

                        foreach (ToolStripMenuItem sssubItem in ssubItem.DropDownItems)
                        {
                            formItem = sssubItem as ToolStripMenuItem;
                            //Console.WriteLine(formItem.Tag);
                            itemName = GetItemName(formItem.Tag);
                            if (itemName.Length > 0 & itemName.Equals(formName))
                            {
                                formItem.PerformClick();
                            }

                        }
                    }
                }

            }
            //parent.ChildClick(null, null);
        }

        private string GetItemName(object tag)
        {
            if (tag == null) return string.Empty;

            string[] arrText = System.Text.RegularExpressions.Regex.Split(tag.ToString(), ",");
            string sProgramId = arrText[0];

            return  sProgramId;
        }

        private string GetItemName()
        {
            throw new NotImplementedException();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTMPI");

            if (temp_form == null)
            {
                LoadPage("RTMPI");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTGrinder");

            if (temp_form == null)
            {
                LoadPage("RTGrinder");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void c1Button1_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTMPI");

            if (temp_form == null)
            {
                LoadPage("RTMPI");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void c1Button2_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTSHT_DUST");

            if (temp_form == null)
            {
                LoadPage("RTSHT_DUST");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void c1Button3_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTSHT_DUST");

            if (temp_form == null)
            {
                LoadPage("RTSHT_DUST");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void c1Button5_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTGrinder");

            if (temp_form == null)
            {
                LoadPage("RTGrinder");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void c1Button4_Click(object sender, EventArgs e)
        {
            var temp_form = GetForm("RTGrinder_DUST");

            if (temp_form == null)
            {
                LoadPage("RTGrinder_DUST");
            }
            else
            {
                //temp_form.Activate();
                temp_form.Show();
                temp_form.BringToFront();
            }
        }

        private void uC_ZoneMoveBtn1_Load(object sender, EventArgs e)
        {

        }

        private void uC_ZoneMoveBtn6_Load(object sender, EventArgs e)
        {

        }

        private void btnChipRemoveStart_Click(object sender, EventArgs e)
        {
            CallSp("F080", "SP_SCR_TO_L1_RECEIVE_CRE", "칩제거 시작");
        }

        private void btnChipRemoveEnd_Click(object sender, EventArgs e)
        {
            CallSp("F090", "SP_SCR_TO_L1_RECEIVE_CRE", "칩제거 종료");
        }

        private void btnRockReaStart_Click(object sender, EventArgs e)
        {
            CallSp("F100", "SP_SCR_TO_L1_RECEIVE_CRE", "연마지석교체 시작");
        }

        private void btnRockReaEnd_Click(object sender, EventArgs e)
        {
            CallSp("F110", "SP_SCR_TO_L1_RECEIVE_CRE", "연마지석교체 종료");
        }

        private void uC_ZoneMoveBtn13_Load(object sender, EventArgs e)
        {

        }

        

        private async void LbChipBlink(Label lb)
        {

            while (isChipWaring)
            {
                await Task.Delay(500);
                if (!lb.Visible)
                {
                    SetLabelVisible(lb, isChipWaring);
                    //lb.BackColor = Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
                }
                //lb.ForeColor = lb.ForeColor == Color.Red ? Color.Green : Color.Red;
                lb.BackColor = lb.BackColor == Color.Red ? Color.Green : Color.Red;
            }

            if (!isChipWaring)
            {
                SetLabelVisible(lb, isChipWaring);
                lb.BackColor = Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            }
        }

        private async void LbRockBlink(Label lb)
        {

            while (isRockWaring)
            {
                await Task.Delay(500);

                if (!lb.Visible)
                {
                    SetLabelVisible(lb, isRockWaring);
                    lb.BackColor = Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
                }

                //lb.ForeColor = lb.ForeColor == Color.Red ? Color.Green : Color.Red;
                lb.BackColor = lb.BackColor == Color.Red ? Color.Green : Color.Red;
            }


            if (!isRockWaring)
            {
                SetLabelVisible(lb, isRockWaring);
                lb.BackColor = Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            }
        }

        private void uC_ZoneMoveBtn2_Load_1(object sender, EventArgs e)
        {

        }

        private void btnRockReaStart1_Click(object sender, EventArgs e)
        {
            CallSp("F110", "SP_SCR_TO_L1_RECEIVE_CRE", "연마지석교체 시작", "0");
        }

        private void btnRockReaEnd1_Click(object sender, EventArgs e)
        {
            CallSp("F110", "SP_SCR_TO_L1_RECEIVE_CRE", "연마지석교체 종료", "1");
        }

        private void btnRockUseInit_Click(object sender, EventArgs e)
        {
            CallSp("F110", "SP_SCR_TO_L1_RECEIVE_CRE", "연마지석교체 시작", "0");
            
            CallSp("F110", "SP_SCR_TO_L1_RECEIVE_CRE", "연마지석교체 종료", "1");
        }
    }

    public static class GuiExtensionMethods
    {
        public static void Enable(this Control con, bool enable)
        {
            if (con != null)
            {
                try
                {
                    con.Invoke((MethodInvoker)(() => con.Enabled = enable));
                }
                catch
                {
                }
            }
        }




    }
}
