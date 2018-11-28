using BGsystemLibrary.MatMgmt;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;

namespace BGsystemLibrary.Common
{
    public partial class ZoneMovePopup : Form
    {
        //공통
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();

        DataTable olddt;
        string txtdeptNm = "";

        public ScheduleMgmt frm;

        public delegate void FormSendDataHandler(object obj);
        public event FormSendDataHandler FormSendEvent;

        private int GridRowsCount = 2;
        private int GridColsCount = 7;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;
        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        private string titleNM = "";
        private string menuNM = "";
        private string categoryNM = "";
        private string zoneCD = "";

        private string itemSizeHead = "";
        private string itemSizeIDTail = "";
        ArrayList _al = new ArrayList();

        public ZoneMovePopup(string _titleNM, string _menuNM, string _categoryNM, string _zoneCD)
        {
            titleNM = _titleNM;
            menuNM = _menuNM;
            categoryNM = _categoryNM;

            zoneCD = _zoneCD;

            InitializeComponent();

            Text = titleNM;
            categoryNM = _categoryNM;
        }
        clsParameterMember param = new clsParameterMember();
        private void btnSave_Click(object sender, EventArgs e)
        {
            //
            if (!IsCheckedItem())
            {
                MessageBox.Show(" 이동할 소재를 선택하세요.");
                return;
            }

            SqlCommand cmd = new SqlCommand();

            string spName = "SP_ZONE_MOVE_PROC";

            string proc_stat = "";
            string proc_msg = "";
            string _heat_no = "";
            int _heat_seq = 999;
            string _toZone = "";
            try
            {
                for (int row = GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    if (grdMain.GetData(row, "SEL").ToString() == "True")
                    {
                        this.Cursor = Cursors.AppStarting;
                        _heat_no = grdMain.GetData(row, "HEAT_NO").ToString();
                        _heat_seq = int.Parse(grdMain.GetData(row, "HEAT_SEQ").ToString());
                        _toZone = ((ComLib.DictionaryList)this.cbx_TargetZone.SelectedItem).fnValue;

                        param.Clear();
                        param.Add(SqlDbType.VarChar, "@P_HEAT_NO", _heat_no, ParameterDirection.Input);
                        param.Add(SqlDbType.VarChar, "@P_HEAT_SEQ", _heat_seq, ParameterDirection.Input);
                        param.Add(SqlDbType.VarChar, "@P_TO_ZONE_CD", _toZone, ParameterDirection.Input);
                        param.Add(SqlDbType.VarChar, "@P_USER_ID", ck.UserID, ParameterDirection.Input);
                        param.Add(SqlDbType.VarChar, "@P_PROC_STAT", proc_stat, ParameterDirection.Output, 4000);
                        param.Add(SqlDbType.VarChar, "@P_PROC_MSG", proc_msg, ParameterDirection.Output, 4000);

                        cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                        this.Cursor = Cursors.Default;

                        Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                        Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);

                        if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                        {
                            MessageBox.Show("@P_PROC_MSG:{0}"+ cmd.Parameters["@P_PROC_MSG"].Value.ToString());
                            string _warning_msg = "HEAT_NO: " + _heat_no + "가 ZONE ID:" + _toZone + "이동 실패하였습니다.";
                            clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                            return;
                        }

                        string _info_msg = "HEAT_NO: " + _heat_no + "가 ZONE ID:"+ _toZone+"로 이동하였습니다.";

                        clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _info_msg);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {

                //Button_Click(btnDisplay, null);
                //SelectFirstRow();
                Close();
            }

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

        private void ZoneMovePopup_Load(object sender, EventArgs e)
        {
            DrawGrid(grdMain);

            InitControl();

        }

        private void DrawGrid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            
            try
            {
                int _GridRowsCount = GridRowsCount;
                int _GridColsCount = GridColsCount;
                int _RowsFixed = RowsFixed;
                int _RowsFrozen = RowsFrozen;
                int _ColsFixed = ColsFixed;
                int _ColsFrozen = ColsFrozen;
                int _TopRowsHeight = TopRowsHeight;
                int _DataRowsHeight = DataRowsHeight;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, _TopRowsHeight, _DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
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

            _al.Add(new HostedControl(grdMain, lbSel, 1, 1));
        }
        bool allChecked = false;
        private void AllRowSelectedEvent(object sender, EventArgs e)
        {
            if (allChecked)
            {
                for (int rowCnt = GridRowsCount; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "SEL", false);
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = GridRowsCount; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "SEL", true);
                }
                allChecked = true;

            }
        }

        private void FlexGridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 60;
            grdItem.Cols[2].Width = 120;
            grdItem.Cols[3].Width = 120;
            grdItem.Cols[4].Width = 120;
            grdItem.Cols[5].Width = 120;
            grdItem.Cols[6].Width = 120;

            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "선택";
            grdItem[1, 2] = "HEAT";
            grdItem[1, 3] = "HEAT 순번";
            grdItem[1, 4] = "강종";
            grdItem[1, 5] = "규격";
            grdItem[1, 6] = "MarkingCode";


            //grdItem.Cols[3].Visible = false;
            //grdItem.Cols[4].Visible = false;
            //grdItem.Cols[6].Visible = false;
            //grdItem.Cols[7].Visible = false;

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

            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 2);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3, grdItem.Cols.Count - 1);
        }

        private void InitControl()
        {

            SetCombo(cbx_SourceZone, GetSourceZoneSql(), zoneCD, false);

            SetCombo(cbx_TargetZone, GetTargetZoneSql(), "", false);

        }

        private void SetCombo(ComboBox cbx_SourceZone, string v1, object _zoneCD, bool v2)
        {
            
        }

        private string GetTargetZoneSql()
        {
            string sql = string.Empty;
            string sourceZone = ((ComLib.DictionaryList)this.cbx_SourceZone.SelectedItem).fnValue;
            sql += string.Format(@"   SELECT DISTINCT COLUMNB AS ZONE
                                            FROM TB_CM_COM_CD
                                           WHERE CATEGORY = 'ZONE_CD'
                                             AND USE_YN = 'Y' 
                                             AND COLUMNA = '{0}'", sourceZone);
            return sql;
        }

        private string GetSourceZoneSql()
        {
            string sql = string.Empty;

            sql = string.Format(@"SELECT DISTINCT COLUMNA AS ZONE
                                         FROM TB_CM_COM_CD
                                        WHERE CATEGORY = 'ZONE_CD'
                                          AND USE_YN = 'Y' ");

            return sql;
        }


        private void SetCombo(ComboBox cb, string _sql, string selected_id, bool isTotalAdd)
        {

            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = cd.FindDataTable(_sql);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row["ZONE"].ToString(), row["ZONE"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndex = 0;

                    if (!string.IsNullOrEmpty(selected_id))
                    {
                        foreach (var item in cb.Items)
                        {
                            if (((DictionaryList)item).fnValue == selected_id)
                            {
                                cb.SelectedIndex = cb.Items.IndexOf(item);
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbx_SourceZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCombo(cbx_TargetZone, GetTargetZoneSql(), "", false);

            SetBindingGrd();
        }

        private void SetBindingGrd()
        {
            clsFlexGrid.grdDataClearForBind(grdMain);

            SetDataBinding();
        }

        private void SetDataBinding()
        {
            try
            {

                string sql = "";
                sql += string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY A.HEAT_NO, A.HEAT_SEQ ASC)) AS L_NUM
                                             , 'False' AS SEL
                                             , A.HEAT_NO
  	                                         , A.HEAT_SEQ
  	                                         , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
                                             , (SELECT CD_NM         FROM TB_CM_COM_CD           WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = A.ITEM + A.ITEM_SIZE) AS ITEM_SIZE_NM 
  	                                         , (SELECT Marking_Code  FROM TB_MAT_TAKE_OVER_BLOOM WHERE HEAT_NO = A.HEAT_NO      AND HEAT_SEQ = A.HEAT_SEQ )     AS MARKING_CODE                                                                      
                                          FROM TB_RL_TM_TRACKING A
                                         WHERE A.PROG_STAT  IN( 'RUN', 'WAT')
                                           AND ZONE_CD = '{0}'   ", ((ComLib.DictionaryList)this.cbx_SourceZone.SelectedItem).fnValue);





                //SQL 쿼리 조회 후 데이터셋 return
                olddt = cd.FindDataTable(sql);



                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, olddt);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                //그리드 콤보박스 지정
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("SEL", FlexGridCellTypeEnum.CheckBox, "true");

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;

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

        private void grdMain_Paint(object sender, PaintEventArgs e)
        {
            foreach (HostedControl hosted in _al)
                hosted.UpdatePosition();
        }

        private void grdMain_CellChecked(object sender, RowColEventArgs e)
        {

            if (e.Row >= GridRowsCount)
            {
                CheckEnum checkState = grdMain.GetCellCheck(e.Row, e.Col);
                for (int r = grdMain.Selection.r1; r < grdMain.Selection.r2 + 1; r++)
                {
                    grdMain.SetCellCheck(r, e.Col, checkState);
                }
            }
            
        }
    }
}
