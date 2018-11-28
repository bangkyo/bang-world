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
    public partial class ScheduleMgmt : Form
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
        DataTable olddtMain_drag;
        DataTable moddtMain_drag;

        

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private int main_GridRowsCount = 2;
        private int main_GridColsCount = 12;
        private int main_RowsFixed = 2;
        private int main_RowsFrozen = 0;
        private int main_ColsFixed = 1;
        private int main_ColsFrozen = 0;



        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        // 프로그램 명 변수
        private static string ownerNM = "";
        private static string titleNM = "";

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        private object strBefValue;

        #endregion 공통 생성자
        public ScheduleMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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


                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

                    SetDataBinding();  // 조회 버튼을 통한 데이터입력

                    break;



                case "btnSave":
                    if (this.scrAuthMod != "Y")
                    {
                        MessageBox.Show("수정 권한이 없습니다");
                        return;
                    }

                    SaveData();

                    break;

                case "btnTakOverCancel":
                    TakOverCancel();
                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

        private void TakOverCancel()
        {
            // 체크 유무
            // 계획 가부
            // BLOOM LIST 확보

            if (grdMain.Rows.Count <= main_GridRowsCount) return;

            if (!IsCheckedItem()) return;


            if (MessageBox.Show("인수 취소 진행 하시겠습니까? ", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Heats cancelHeats = new Heats();

            cancelHeats = GetCancelHeats();

            SqlCommand cmd;
            clsParameterMember param = new clsParameterMember();
            foreach (Heat heat in cancelHeats)
            {
                 cmd = new SqlCommand();

                string spName = "SP_MatTakeOverMgmt_CAN";

                string take_Over_gp = "Y";
                string _heat_no = heat.No;
                string _surface_state = "";
                string takeOverBloomData = heat.GetBloomList();
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

                    string _info_msg = "HEAT_NO: " + _heat_no + ", HEAT_SEQ LIST:" + takeOverBloomData + "가 인수 취소 되었습니다.";
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

            return;


        }

        private Heats GetCancelHeats()
        {
            Heats _cancelHeats = new Heats();

            DataTable dt = (DataTable)grdMain.DataSource;
            DataTable heatDT;
            Heat _heat;
            string _heatno = string.Empty;
            BloomIDs bloomids;

            if (dt == null) return _cancelHeats;

            foreach (DataRow row in dt.Rows)
            {
                if (row["SEL"].ToString() == "True")
                {
                    _heatno = row["HEAT_NO"].ToString();
                    
                    heatDT = new DataTable();
                    heatDT = GetBloomDataFromSchedule(_heatno);
                    _heat = new Heat(_heatno);
                    foreach (DataRow heatrow in heatDT.Rows)
                    {
                        _heat.AddBloomid(new BloomID(heatrow["HEAT_NO"].ToString(), heatrow["HEAT_SEQ"].ToString(), "0"));

                    }
                    _cancelHeats.Add(_heat);
                }
            }

            return _cancelHeats;
        }

        private DataTable GetBloomDataFromSchedule(string _heat)
        {

            DataTable heatDT = new DataTable();
            try
            {


                //SQL
                string sql = "";
                sql = string.Format(@"SELECT HEAT_NO
                                           , HEAT_SEQ
                                        FROM TB_MAT_TAKE_OVER_BLOOM
                                       WHERE HEAT_NO = '{0}'", _heat);

                //SQL 쿼리 조회 후 데이터셋 return
                heatDT = cd.FindDataTable(sql);
               
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //grdMain_Click(null, null);
                //SelectFirstRow();
            }

            return heatDT;
        }

        private string GetSelectedBloomData(string _HeatNo)
        {
            // format HEAT_NO, 표면상태(입력된것), HEAT_SEQ,USER_ID, 처리결과, 처리결과 MSG
            string rtData = "";


            DataTable dt = GetBloomLIstByHeat(_HeatNo);
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

        private DataTable GetBloomLIstByHeat(string _HeatNo)
        {
            throw new NotImplementedException();
        }

        private bool IsCheckedItem()
        {
            bool rtn = false;

            DataTable dt = (DataTable)grdMain.DataSource;

            if (dt == null) return false;

            foreach (DataRow row in dt.Rows)
            {
                if (row["SEL"].ToString() == "True")
                {
                    return rtn = true;
                }
            }

            return rtn;
        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        private void SaveData()
        {

            if (IsChanged())
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }

            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;
            string sql = "";
            int UpCnt = 0;
            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                //정호준
                #region grdMain1
                for (int row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    if (grdMain.GetData(row, "L_NUM").ToString() == "수정")
                    {
                        sql = string.Format(" UPDATE TB_SCHEDULE                                           ");
                        sql += string.Format(" SET                                                         ");
                        sql += string.Format("        WORK_PLN_DATE = '{0}'                                ", vf.Format(grdMain.GetData(row, "WORK_PLN_DATE"), "yyyyMMdd"));
                        sql += string.Format("       ,WORK_SEQ  = '{0}'                                    ", grdMain.GetData(row, "WORK_SEQ"));
                        sql += string.Format("       ,MODIFIER = '{0}'                                     ", ck.UserID);
                        sql += string.Format("       ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        sql += string.Format(" WHERE HEAT_NO = '{0}'                                       ", grdMain.GetData(row, "HEAT_NO"));

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        UpCnt++;
                    }

                }
                #endregion grdMain1 


                //실행후 성공
                transaction.Commit();

                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);



                Button_Click(btnDisplay, null);

                //커서유지 add ]]
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다. Error: " + ex.Message);
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

        private bool IsChanged()
        {
            bool ischanged = false;

            for (int row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
            {
                if(grdMain.GetData(row, "L_NUM").ToString() =="수정")
                {
                    ischanged = true;
                }
            }

            return ischanged;
        }

        private void SetDataBinding()
        {
            InitGridData();
            SetDataBinding_MainGrd();
        }

        private void SetDataBinding_MainGrd()
        {
            try
            {

                //string take_Over_gp = ((ComLib.DictionaryList)cbx_TakeOver_Gp.SelectedItem).fnValue;
                string _start_dt = vf.Format(start_dt.Value, "yyyyMMdd");
                string _end_dt = vf.Format(end_dt.Value, "yyyyMMdd");

                //SQL
                string sql = "";
                sql = string.Format(@"SELECT '' AS HOLD   
                                           , 'False' AS SEL                                                                                             
                                           , CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY WORK_PLN_DATE, WORK_SEQ ASC)) AS L_NUM 
                                           , WORK_SEQ 
                                           , HEAT_NO                       
                                           , CONVERT(DATE, WORK_PLN_DATE) AS WORK_PLN_DATE                                                                                                                                                       
                                           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = STEEL) AS STEEL_NM                    
                                           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = ITEM + ITEM_SIZE) AS ITEM_SIZE_NM 
                                           , CONVERT(DATE, TAKE_OVER_DATE) AS TAKE_OVER_DATE                                                            
                                           , TAKE_OVER_PCS                                                                                              
                                           , WORK_PCS                                                                                                   
                                           , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'PROG_STAT' AND CD_ID = WORK_STAT) AS WORK_STAT           
                                        FROM TB_SCHEDULE   
                                       WHERE CONVERT(varchar(10), WORK_PLN_DATE, 120) BETWEEN '{0}' AND '{1}'", _start_dt, _end_dt);

                //SQL 쿼리 조회 후 데이터셋 return
                olddtMain = cd.FindDataTable(sql);
                moddtMain = olddtMain.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGridWithDT(grdMain, moddtMain);
                this.Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), Text + ":" + moddtMain.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //grdMain_Click(null, null);
                //SelectFirstRow();
            }
        }

        private void DrawGridWithDT(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("WORK_PLN_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now.Date );
                FlexGridColumns.Add("TAKE_OVER_DATE", FlexGridCellTypeEnum.TimePicker, DateTime.Now.Date);
                FlexGridColumns.Add("SEL", FlexGridCellTypeEnum.CheckBox, false);

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);

                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //마지막행 사이즈조절, 로우공백흰색
                grdMain.ExtendLastCol = true;
                grdMain.Styles.EmptyArea.BackColor = Color.White;

                grdMain.SelectionMode = SelectionModeEnum.Row;
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
                //clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
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

        private void InitGridData()
        {
            clsFlexGrid.grdDataClear(grdMain, main_GridRowsCount);
        }

        private void ScheduleMgmt_Load(object sender, EventArgs e)
        {
            //그리드 초기화
            DrawMainGrid(grdMain);
            //초기화
            InitControl();
        }

        private void InitControl()
        {
            start_dt.Value = DateTime.Now.Date;
            end_dt.Value = DateTime.Now.Date;
        }

        private void DrawMainGrid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                int _GridRowsCount = main_GridRowsCount;
                int _GridColsCount = main_GridColsCount;
                int _RowsFixed = main_RowsFixed;
                int _RowsFrozen = main_RowsFrozen;
                int _ColsFixed = main_ColsFixed;
                int _ColsFrozen = main_ColsFrozen;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexMainGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdItem.SelectionMode = SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                grdItem.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.Rows;
                grdItem.BeforeDragRow += grdItem_BeforeDragRow;
                grdItem.AfterDragRow += grdMain_AfterDragRow;

                grdItem.AllowSorting = AllowSortingEnum.MultiColumn;
                grdItem.Cols[2].Sort = SortFlags.Ascending;
                grdItem.Cols[3].Sort = SortFlags.Ascending;
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

        private void grdItem_BeforeDragRow(object sender, DragRowColEventArgs e)
        {
            int dragitemRow = e.Row - main_GridRowsCount;
            int dropitemRow = e.Position - main_GridRowsCount;

            if (!clsFlexGrid.CanMoveRow(grdMain, e.Row, e.Position, grdMain.Cols["WORK_PLN_DATE"].Index))
            {
                MessageBox.Show(" 같은 작업계획일자에서만 이동가능.");
                e.Cancel = true;
                return;
            }

            MoveRow(e.Row, e.Position, dragitemRow, dropitemRow);
            e.Cancel = true;                                        // To Suppress default Dragging Behaviour
        }

        private void MoveRow(int _Row, int _Position, int dragitemRow, int dropitemRow)
        {

            try
            {
                DataTable dt = (DataTable)grdMain.DataSource;
                dt.DefaultView.Sort = "WORK_PLN_DATE, WORK_SEQ";
                dt = dt.DefaultView.ToTable();

                olddtMain_drag = dt.Copy();
                olddtMain_drag.DefaultView.Sort = "WORK_PLN_DATE, WORK_SEQ";
                olddtMain_drag = olddtMain_drag.DefaultView.ToTable();

                System.Data.DataRow drDragged = dt.Rows[dragitemRow];
                System.Data.DataRow drNewPosition = dt.Rows[dropitemRow];

                object[] arr1 = drDragged.ItemArray;
                object[] arr2 = drNewPosition.ItemArray;

                dt.Rows.Remove(drDragged);
                //dt.Rows.Remove(drNewPosition);

                DataRow dr1 = dt.NewRow();
                dr1.ItemArray = arr1;                                   //Temporary Row to store data of row being dragged
                                                                        //DataRow dr2 = dt.NewRow();
                                                                        //dr2.ItemArray = arr2;                                   //Temporary Row to store data of row at which it is dropped

                dt.Rows.InsertAt(dr1, dropitemRow);                    //Inserting Rows at Swapped Postions
                                                                       //dt.Rows.InsertAt(dr2, dragitemRow);

                //dragrow < droprow 아래로 옮겼을 경우 해당 일자의 seq 를 리스트로 가져와서. 드랍된이수 리스트 순서대로 seq 재조정한다.
                //droprow < dragrow
                //List<string> seqList = new List<string>();
                //seqList.Add()

                foreach (DataRow datarow in olddtMain_drag.Rows)
                {
                    //dt.Rows[int.Parse(row["L_NUM"].ToString()) - 1]["WORK_SEQ"] = row["WORK_SEQ"].ToString();
                    dt.Rows[olddtMain_drag.Rows.IndexOf(datarow)]["WORK_SEQ"] = datarow["WORK_SEQ"].ToString();
                }

                //grdMain.DataSource = dt;
                DrawGridWithDT(grdMain, dt);

                int dragRow = dragitemRow + 2;
                int dropRow = dropitemRow + 2;
                int modifiedStartRow = (dragRow > dropRow) ? dropRow : dragRow;
                int modifiedEndRow = (dragRow < dropRow) ? dropRow : dragRow;
                //var diffRows = dt.AsEnumerable().Intersect(olddt.AsEnumerable(), DataRowComparer.Default);

                // 이동이 크게 변한경우 사이의 row 들의 순번이 변경됨으로 적용필요..
                for (int rowNo = modifiedStartRow; rowNo <= modifiedEndRow; rowNo++)
                {
                    grdMain.SetData(rowNo, "L_NUM", "수정");
                    // Update 배경색 지정
                    //clsFlexGrid.GridCellRangeStyleColor(grdMain, row, 1, row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
                SetUpDateFlag();

                
            }
            catch (Exception ex)
            {

                MessageBox.Show("{0}",ex.Message);
                return;
            }
            
        }









        private void grdMain_AfterDragRow(object sender, DragRowColEventArgs e)
        {
            //SetUpDateFlag();

            //DataTable dt = (DataTable)grdMain.DataSource;

            //int dropitemRow = e.Position - 2;

            //foreach (DataRow oldrow in olddtMain_drag.Rows)
            //{
            //    foreach (DataRow setRow in dt.Rows)
            //    {
            //        if (IsChangedTable(olddtMain_drag, oldrow, dt, setRow, "HEAT_NO"))
            //        {
            //            //dt.["L_NUM"] = "수정";
            //            grdMain.SetData(dropitemRow, "L_NUM", "수정");
            //            // Update 배경색 지정
            //            //grdMain.Rows[grdMain.Row].Style = grdMain.Styles["UpColor"];
            //            clsFlexGrid.GridCellRangeStyleColor(grdMain, dropitemRow, 1, dropitemRow, grdMain.Cols.Count - 1, Color.Green, Color.Black);
            //        }

            //    }
            //}
        }

        private void SetUpDateFlag()
        {
            for (int row = main_GridRowsCount; row < grdMain.Rows.Count; row++)
            {
                if (grdMain.GetData(row, "L_NUM").ToString().Equals("수정"))
                {
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, row, 1, row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }
        }

        private bool IsChangedTable(DataTable olddt, DataRow oldrow, DataTable dt, DataRow setRow, string key)
        {

            if (oldrow[key].ToString() == setRow[key].ToString())
            {
                if (olddt.Rows.IndexOf(oldrow) == dt.Rows.IndexOf(setRow)) return false;
                else return true;

            }
            else return false;

        }


        private void FlexMainGridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 60;
            grdItem.Cols[2].Width = 100;
            grdItem.Cols[3].Width = 150;
            grdItem.Cols[4].Width = 150;
            grdItem.Cols[5].Width = 200;
            grdItem.Cols[6].Width = 150;
            grdItem.Cols[7].Width = 150;
            grdItem.Cols[8].Width = 150;
            grdItem.Cols[9].Width = 150;
            grdItem.Cols[10].Width = 150;
            grdItem.Cols[11].Width = 150;
            //컬럼 명 세팅
            grdItem[1, 0] = "";
            grdItem[1, 1] = "선택";
            grdItem[1, 2] =  "NO";
            grdItem[1, 3] =  "순서";
            grdItem[1, 4] =  "HEAT";
            grdItem[1, 5] =  "작업계획일자";
            grdItem[1, 6] =  "강종";
            grdItem[1, 7] =  "규격";
            grdItem[1, 8] =  "인수일자";
            grdItem[1, 9] =  "인수본수";
            grdItem[1, 10] = "작업본수";
            grdItem[1, 11] = "진행상태";

            //grdItem.Cols[1].Visible = false;
            //grdItem.AllowEditing = true;

            grdItem.Cols[0].AllowEditing = false;
            grdItem.Cols[1].AllowEditing = true;
            grdItem.Cols[2].AllowEditing = false;
            grdItem.Cols[3].AllowEditing = true;
            grdItem.Cols[4].AllowEditing = false;
            grdItem.Cols[5].AllowEditing = true;
            grdItem.Cols[6].AllowEditing = false;
            grdItem.Cols[7].AllowEditing = false;
            grdItem.Cols[8].AllowEditing = false;
            grdItem.Cols[9].AllowEditing = false;
            grdItem.Cols[10].AllowEditing =false;
            grdItem.Cols[11].AllowEditing = false; 
            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);

           
            //데이터로우
            clsFlexGrid.DataGridFixedCenterStyle(grdItem, 0, 0);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 4);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 5);
            clsFlexGrid.DataGridCenterStyle(grdItem, 6, 8);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 9);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 10);
            clsFlexGrid.DataGridCenterStyle(grdItem, 11);


        }
        DataTable beforeEditDT;
        private object callback;

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            //// NO COLUMN 수정불가하게..
            //if (e.Col == grd.Cols["L_NUM"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            //{
            //    e.Cancel = true;
            //    return;
            //}

            // 수정여부 확인을 위해 저장
            strBefValue = grd.GetData(e.Row, e.Col).ToString();

            //if (e.Col == grd.Cols["WORK_PLN_DATE"].Index)
            //{
            //    DateTime modifiedDT = (DateTime)grd.GetData(e.Row, e.Col);
            //    if (IsOutOfRange(modifiedDT, start_dt.Value, end_dt.Value))
            //    {

            //        MessageBox.Show("검색기간을 벗어난 수정입니다.");

            //        grd.SetData(e.Row, e.Col, strBefValue);
            //        e.Cancel = true;
            //        return;
            //    }
            //}

            //Console.WriteLine("grdMain_BeforeEdit:" + strBefValue);


        }

        private bool IsOutOfRange(DateTime modifiedDT, DateTime start_dt, DateTime end_dt)
        {
            bool rtn = true;

            if ((DateTime.Compare(modifiedDT,start_dt) >=0) && (DateTime.Compare(end_dt,modifiedDT) >= 0))
            {
                rtn = false;
            }

            return rtn;
        }

        public void Search(DateTime _dt)
        {
            start_dt.Value = _dt;
            end_dt.Value = _dt;
            Button_Click(btnDisplay, null);
        }




        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            int maxSeq = 0;
            string modifiedHeatNo = "";

            if (e.Col == grd.Cols["WORK_PLN_DATE"].Index)
            {

                //Console.WriteLine("grdMain_AfterEdit:" + strBefValue);
                DateTime modifiedDT = (DateTime)grd.GetData(e.Row, e.Col);
                if (IsOutOfRange(modifiedDT, start_dt.Value, end_dt.Value))
                {
                    grd.SetData(e.Row, e.Col, strBefValue);
                    return;
                }
                //Console.WriteLine("grdMain_AfterEdit_modified:" + grd.GetData(e.Row, e.Col).ToString());

                modifiedHeatNo = (string)grd.GetData(e.Row, "HEAT_NO");
                // 작업계획일자가 수정된경우 수정된 작업계획일자에 해당하는 seq 를 max+1로 수정한다.
                // 이때 수정된 작업의 SEQ 는 무시해야한다.
                //maxSeq = GetMaxSeq(grd, vf.Format(grd.GetData(e.Row, "WORK_PLN_DATE"), "yyyyMMdd"));
                maxSeq = GetMaxSeq(grd, (DateTime)grd.GetData(e.Row, "WORK_PLN_DATE"), modifiedHeatNo) + 1;

                grd.SetData(e.Row, "WORK_SEQ", maxSeq.ToString());

                SetUpRowModified(grd, e.Row);

                SetSorting(grd);

            }

            

            //grd.Refresh();
            //e.Cancel = false;

        }

        private void SetSorting(C1FlexGrid _grd)
        {
            //_grd.AllowSorting = AllowSortingEnum.MultiColumn;
            //_grd.Cols["WORK_PLN_DATE"].Sort = SortFlags.Ascending;
            //_grd.Cols["WORK_SEQ"].Sort = SortFlags.Ascending;
            _grd.Sort(SortFlags.UseColSort, _grd.Cols["WORK_PLN_DATE"].Index, _grd.Cols["WORK_SEQ"].Index);
        }

        private void SetUpRowModified(C1FlexGrid _grd, int modifiedRow)
        {
            // 저장시 UPDATE로 처리하기 위해 flag set
            _grd.SetData(modifiedRow, "L_NUM", "수정");
            // Update 배경색 지정
            clsFlexGrid.GridCellRangeStyleColor(_grd, modifiedRow, 1, modifiedRow, _grd.Cols.Count - 1, Color.Green, Color.Black);

        }

        private int GetMaxSeq(C1FlexGrid _grd, DateTime _work_pln_date, string _modifiedHeatNo)
        {
            int rtn = 0;

            DataTable dt = (DataTable)_grd.DataSource;

            try
            {
                var results = (from DataRow row in dt.Rows
                               where (DateTime)row["WORK_PLN_DATE"] == _work_pln_date && !((string)row["HEAT_NO"] == _modifiedHeatNo)

                               orderby row["WORK_SEQ"] descending
                               select row["WORK_SEQ"]
                          ).Take(1)
                          ;
                rtn = (int)results.First();
            }
            catch (Exception)
            {

                rtn = 0;
            }

            return rtn;
        }

        private void grdMain_SelChange(object sender, EventArgs e)
        {
            
        }

        private void grdMain_ValidateEdit(object sender, ValidateEditEventArgs e)
        {

        }

        private void grdMain_StartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            strBefValue = grd.GetData(e.Row, e.Col).ToString();
        }

        private void grdMain_GridChanged(object sender, GridChangedEventArgs e)
        {
            //C1FlexGrid grd = sender as C1FlexGrid;
            //grd.AllowSorting = AllowSortingEnum.MultiColumn;
            //grd.Cols["WORK_PLN_DATE"].Sort = SortFlags.Ascending;
            //grd.Cols["WORK_SEQ"].Sort = SortFlags.Ascending;
            //grd.Sort(SortFlags.UseColSort, grd.Cols["WORK_PLN_DATE"].Index, grd.Cols["WORK_SEQ"].Index);
        }

        private void grdMain_ChangeEdit(object sender, EventArgs e)
        {

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            //선택된 row가 맨위에 있는경우 
            if (grdMain.Rows.Count <= main_GridRowsCount || grdMain.RowSel <= main_GridRowsCount )
            {
                return;
            }

            int BeforeRowIngrd = grdMain.RowSel;
            int AfterRowIngrd = BeforeRowIngrd - 1;
            int BeforRowInDT  = grdMain.RowSel- main_GridRowsCount;
            int AfterRowInDt = BeforRowInDT - 1;

            if (!clsFlexGrid.CanMoveRow(grdMain, BeforeRowIngrd, AfterRowIngrd, grdMain.Cols["WORK_PLN_DATE"].Index))
            {
                MessageBox.Show(" 같은 작업계획일자에서만 이동가능.");

                return;
            }

            MoveRow(BeforeRowIngrd, AfterRowIngrd, BeforRowInDT, AfterRowInDt);

            grdMain.Row = AfterRowIngrd;

        }





        private void btnDown_Click(object sender, EventArgs e)
        {
            //선택된 row가 맨아래에 있는경우 
            if (grdMain.Rows.Count <= main_GridRowsCount || grdMain.RowSel >= grdMain.Rows.Count -1)
            {
                return;
            }

            int BeforeRowIngrd = grdMain.RowSel;
            int AfterRowIngrd = BeforeRowIngrd +1;
            int BeforRowInDT = grdMain.RowSel - main_GridRowsCount;
            int AfterRowInDt = BeforRowInDT + 1;

            if (!clsFlexGrid.CanMoveRow(grdMain, BeforeRowIngrd, AfterRowIngrd, grdMain.Cols["WORK_PLN_DATE"].Index))
            {
                MessageBox.Show(" 같은 작업계획일자에서만 이동가능.");

                return;
            }

            MoveRow(BeforeRowIngrd, AfterRowIngrd, BeforRowInDT, AfterRowInDt);

            grdMain.Row = AfterRowIngrd;
        }

        private void MoveRow(DataTable dt, int _beforRowDt, int _afterRowDt)
        {
            int grd_sourcRow = _beforRowDt + 1;
            int grd_destRow = _afterRowDt + 1;

            //DataRow newRow = moddt.NewRow();
            DataRow sourceRow = dt.Rows[_beforRowDt];
            DataRow destRow = dt.Rows[_afterRowDt];

            List<string> list = new List<string>();
            foreach (var item in dt.Rows[_afterRowDt].ItemArray)
            {
                list.Add(item.ToString());
            }

            //foreach (DataColumn col in dt.Rows)
            //{

            //}

            //이동시에 NO에 수정을 표시
            //         수정된 작업순위는 저장시에 순서대로 마킹해서 테이블에 저장하고 SP 호출하고 마무리.



            //foreach (var item in sourceRow.ItemArray)
            //{
            //    grdMain.SetData(grd_destRow, item, col);
            //}

            for (int itemindex = 0; itemindex < sourceRow.ItemArray.Count(); itemindex++)
            {
                //if (itemindex == grdMain.Cols["L_NO"].Index || itemindex == grdMain.Cols["LINE_GP"].Index || itemindex == grdMain.Cols["WORK_ORD_DATE"].Index || itemindex == grdMain.Cols["WORK_RANK"].Index)
                //{
                //    continue;
                //}

                grdMain.SetData(grd_destRow, itemindex, sourceRow[itemindex]);
            }

            for (int itemindex = 0; itemindex < list.Count(); itemindex++)
            {
                //if (itemindex == grdMain.Cols["L_NO"].Index || itemindex == grdMain.Cols["LINE_GP"].Index || itemindex == grdMain.Cols["WORK_ORD_DATE"].Index || itemindex == grdMain.Cols["WORK_RANK"].Index)
                //{
                //    continue;
                //}

                grdMain.SetData(grd_sourcRow, itemindex, list[itemindex]);
            }
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
}
