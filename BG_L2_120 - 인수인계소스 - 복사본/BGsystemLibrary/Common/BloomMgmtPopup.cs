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
    public partial class BloomMgmtPopup : Form
    {
        //공통
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();


        DataTable olddtMain;
        DataTable moddtMain;
        DataTable olddtMain_drag;
        DataTable moddtMain_drag;
        string txtdeptNm = "";

        public ScheduleMgmt frm;

        public delegate void FormSendDataHandler(object obj);
        public event FormSendDataHandler FormSendEvent;



        private int GridRowsCount = 2;
        private int GridColsCount = 7;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 1;
        private int ColsFrozen = 0;
        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        private string titleNM = "";
        private string menuNM = "";
        private string categoryNM = "";
        private string heatNo = "";

        private string itemSizeHead = "";
        private string itemSizeIDTail = "";

        private object strBefValue;

        public BloomMgmtPopup(string _titleNM, string _menuNM, string _categoryNM, string _HeatNo)
        {
            titleNM = _titleNM;
            menuNM = _menuNM;
            categoryNM = _categoryNM;

            heatNo = _HeatNo;

            InitializeComponent();

            Text = titleNM;
            categoryNM = _categoryNM;
        }
        clsParameterMember param = new clsParameterMember();
        int lastSelectRow ;
        private void btnSave_Click(object sender, EventArgs e)
        {
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
                for (int row = GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    if (grdMain.GetData(row, "L_NUM").ToString() == "수정")
                    {
                        sql = string.Format(" UPDATE TB_RL_TM_TRACKING                                     ");
                        sql += string.Format(" SET                                                         ");
                        sql += string.Format("        WORK_SEQ  = '{0}'                                    ", grdMain.GetData(row, "WORK_SEQ"));
                        sql += string.Format("       ,MODIFIER = '{0}'                                     ", ck.UserID);
                        sql += string.Format("       ,MOD_DDTT = (SELECT CONVERT(VARCHAR, GETDATE(), 120)) ");
                        sql += string.Format(" WHERE HEAT_NO = '{0}'                                       ", grdMain.GetData(row, "HEAT_NO"));
                        sql += string.Format("   AND HEAT_SEQ = '{0}'                                      ", grdMain.GetData(row, "HEAT_SEQ"));

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


                btnDisplay_Click(btnDisplay, null);
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

        private void BloomMgmtPopup_Load(object sender, EventArgs e)
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

                grdItem.SelectionMode = SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                grdItem.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.Rows;
                grdItem.BeforeDragRow += grdItem_BeforeDragRow;
                grdItem.AfterDragRow += grdMain_AfterDragRow;

                grdItem.AllowSorting = AllowSortingEnum.MultiColumn;
                grdItem.Cols[2].Sort = SortFlags.Ascending;
                //grdItem.Cols[3].Sort = SortFlags.Ascending;
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

        private void FlexGridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 60;
            grdItem.Cols[2].Width = 120;
            grdItem.Cols[3].Width = 120;
            grdItem.Cols[4].Width = 120;
            grdItem.Cols[5].Width = 120;

            //컬럼 명 세팅
            grdItem[1, 0] = "선택";
            grdItem[1, 1] = "NO";
            grdItem[1, 2] = "작업순서";
            grdItem[1, 3] = "HEAT";
            grdItem[1, 4] = "순번";
            grdItem[1, 5] = "강종";
            grdItem[1, 6] = "규격";

            //grdItem.Cols[0].AllowEditing = false;
            //grdItem.Cols[3].Visible = false;
            //grdItem.Cols[4].Visible = false;
            //grdItem.Cols[6].Visible = false;
            //grdItem.Cols[7].Visible = false;

            //for (int col = 0; col < grdItem.Cols.Count; col++)
            //{
            //    //if (col == 1)
            //    //{
            //    //    grdItem.Cols[col].AllowEditing = true;
            //    //}
            //    //else
            //    //    grdItem.Cols[col].AllowEditing = false;

            //    grdItem.Cols[col].AllowEditing = false;

            //}


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //clsFlexGrid.CaptionCellColumnStyle(grdItem, 1, 1, 1, 1);
            //데이터로우
            clsFlexGrid.DataGridFixedCenterStyle(grdItem, 0, 0);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 2);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 3);
            clsFlexGrid.DataGridCenterStyle(grdItem, 4, grdItem.Cols.Count - 1);

        }

        private void InitControl()
        {
            SetCombo(cbx_HeatNo, GetHeatNoSql(), "", false);
        }


        private string GetHeatNoSql()
        {
            string sql = string.Empty;

            sql = string.Format(@"  SELECT DISTINCT A.HEAT_NO
                                      FROM (                                             
                                    	     SELECT 
                                    	            ROW_NUMBER()  OVER(ORDER BY B.WORK_PLN_DATE, B.WORK_SEQ, A.HEAT_NO, A.WORK_SEQ)      AS L_NUM
                                    	          , A.HEAT_NO 
                                    	          , A.ZONE_CD 
                                    	       FROM TB_RL_TM_TRACKING A
                                    	          , TB_SCHEDULE B
												  , (
														SELECT TOP 1
												                CD_NM
												          FROM TB_CM_COM_CD
												        WHERE CATEGORY = 'TRK_IN_ZON'
												          AND CD_NM IN ('Z00', 'Z60')
												          AND USE_YN = 'Y'
													) C
                                    	      WHERE A.PROG_STAT  NOT IN ('END')
                                    	        AND A.HEAT_NO = B.HEAT_NO 
                                    	        AND  (C.CD_NM IS NOT NULL AND A.ZONE_CD = C.CD_NM ) OR ( C.CD_NM IS NULL AND A.ZONE_CD ='Z00')
                                    	        AND B.WORK_STAT    <> 'END' 
                                    	   --ORDER BY B.WORK_PLN_DATE, B.WORK_SEQ, A.HEAT_NO, A.WORK_SEQ  
                                    	   ) A
                                    ORDER BY A.HEAT_NO ");

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
                        arrType1.Add(new DictionaryList(row["HEAT_NO"].ToString(), row["HEAT_NO"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
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
                sql += string.Format(@" SELECT '' AS HOLD
                                             , CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY A.HEAT_NO, A.WORK_SEQ ASC)) AS L_NUM
                                        	 , A.WORK_SEQ
                                             , A.HEAT_NO
                                             , A.HEAT_SEQ
                                             , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM
                                             , (SELECT CD_NM         FROM TB_CM_COM_CD           WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = A.ITEM + A.ITEM_SIZE) AS ITEM_SIZE_NM 
                                          FROM TB_RL_TM_TRACKING A
                                             , TB_SCHEDULE B
                                             , (
														SELECT TOP 1
												                CD_NM
												          FROM TB_CM_COM_CD
												        WHERE CATEGORY = 'TRK_IN_ZON'
												          AND CD_NM IN ('Z00', 'Z60')
												          AND USE_YN = 'Y'
												) C
                                         WHERE A.HEAT_NO = '{0}'
                                           AND  ((C.CD_NM IS NOT  NULL AND A.ZONE_CD = C.CD_NM  ) OR ( C.CD_NM IS  NULL AND A.ZONE_CD ='Z00' ))
                                           AND B.WORK_STAT    <> 'END' 
                                           AND A.HEAT_NO = B.HEAT_NO 
                                      ORDER BY A.HEAT_NO, A.WORK_SEQ", ((ComLib.DictionaryList)cbx_HeatNo.SelectedItem).fnValue);


                //SQL 쿼리 조회 후 데이터셋 return
                olddtMain = cd.FindDataTable(sql);
                moddtMain = olddtMain.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGridWithDT(grdMain, moddtMain);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void DrawGridWithDT(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
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

        private void grdMain_AfterDragRow(object sender, DragRowColEventArgs e)
        {
            SetUpDateFlag();
        }

        private void grdItem_BeforeDragRow(object sender, DragRowColEventArgs e)
        {
            int dragitemRow = e.Row - GridRowsCount;
            int dropitemRow = e.Position - GridRowsCount;

            MoveRow(e.Row, e.Position, dragitemRow, dropitemRow);
            e.Cancel = true;                                        // To Suppress default Dragging Behaviour

        }



        private void MoveRow(int row, int position, int dragitemRow, int dropitemRow)
        {
            try
            {
                DataTable dt = (DataTable)grdMain.DataSource;
                dt.DefaultView.Sort = "WORK_SEQ";
                dt = dt.DefaultView.ToTable();

                olddtMain_drag = dt.Copy();
                olddtMain_drag.DefaultView.Sort = "WORK_SEQ";
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

                MessageBox.Show("{0}", ex.Message);
                return;
            }
        }

        private void SetUpDateFlag()
        {
            for (int row = GridRowsCount; row < grdMain.Rows.Count; row++)
            {
                if (grdMain.GetData(row, "L_NUM").ToString().Equals("수정"))
                {
                    clsFlexGrid.GridCellRangeStyleColor(grdMain, row, 1, row, grdMain.Cols.Count - 1, Color.Green, Color.Black);
                }
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            //if (this.scrAuthInq != "Y")
            //{
            //    //MessageBox.Show("조회 권한이 없습니다");
            //    return;
            //}
            if (true)
            {

            }
            //cbx_HeatNo.SelectedItem.ToString

            cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

            SetBindingGrd();
        }

        private void cbx_HeatNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetBindingGrd();
        }

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            //C1FlexGrid grd = sender as C1FlexGrid;


            //// 수정여부 확인을 위해 저장
            //strBefValue = grd.GetData(e.Row, e.Col).ToString();



            //Console.WriteLine("grdMain_BeforeEdit:" + strBefValue);
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            //C1FlexGrid grd = sender as C1FlexGrid;
            //int maxSeq = 0;
            //string modifiedHeatNo = "";

            //if (e.Col == grd.Cols["WORK_PLN_DATE"].Index)
            //{

            //    Console.WriteLine("grdMain_AfterEdit:" + strBefValue);
            //    DateTime modifiedDT = (DateTime)grd.GetData(e.Row, e.Col);
            //    if (IsOutOfRange(modifiedDT, start_dt.Value, end_dt.Value))
            //    {
            //        grd.SetData(e.Row, e.Col, strBefValue);
            //        return;
            //    }
            //    Console.WriteLine("grdMain_AfterEdit_modified:" + grd.GetData(e.Row, e.Col).ToString());

            //    modifiedHeatNo = (string)grd.GetData(e.Row, "HEAT_NO");
            //    // 작업계획일자가 수정된경우 수정된 작업계획일자에 해당하는 seq 를 max+1로 수정한다.
            //    // 이때 수정된 작업의 SEQ 는 무시해야한다.
            //    //maxSeq = GetMaxSeq(grd, vf.Format(grd.GetData(e.Row, "WORK_PLN_DATE"), "yyyyMMdd"));
            //    maxSeq = GetMaxSeq(grd, (DateTime)grd.GetData(e.Row, "WORK_PLN_DATE"), modifiedHeatNo) + 1;

            //    grd.SetData(e.Row, "WORK_SEQ", maxSeq.ToString());

            //    SetUpRowModified(grd, e.Row);

            //    SetSorting(grd);

            //}
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            //선택된 row가 맨위에 있는경우 
            if (grdMain.Rows.Count <= GridRowsCount || grdMain.RowSel <= GridRowsCount)
            {
                return;
            }

            int BeforeRowIngrd = grdMain.RowSel;
            int AfterRowIngrd = BeforeRowIngrd - 1;
            int BeforRowInDT = grdMain.RowSel - GridRowsCount;
            int AfterRowInDt = BeforRowInDT - 1;

            MoveRow(BeforeRowIngrd, AfterRowIngrd, BeforRowInDT, AfterRowInDt);

            grdMain.Row = AfterRowIngrd;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            //선택된 row가 맨아래에 있는경우 
            if (grdMain.Rows.Count <= GridRowsCount || grdMain.RowSel >= grdMain.Rows.Count - 1)
            {
                return;
            }

            int BeforeRowIngrd = grdMain.RowSel;
            int AfterRowIngrd = BeforeRowIngrd + 1;
            int BeforRowInDT = grdMain.RowSel - GridRowsCount;
            int AfterRowInDt = BeforRowInDT + 1;

            MoveRow(BeforeRowIngrd, AfterRowIngrd, BeforRowInDT, AfterRowInDt);

            grdMain.Row = AfterRowIngrd;
        }
    }
}
