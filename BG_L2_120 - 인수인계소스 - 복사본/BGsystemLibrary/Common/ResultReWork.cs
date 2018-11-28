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
    public partial class ResultReWork : Form
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
        private string heatNo = "";

        private string itemSizeHead = "";
        private string itemSizeIDTail = "";

        public ResultReWork(string _titleNM, string _menuNM, string _categoryNM, string _heatNo)
        {
            titleNM = _titleNM;
            menuNM = _menuNM;
            categoryNM = _categoryNM;

            heatNo = _heatNo;

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

            string spName = "SP_REWORK_PROC";

            string proc_stat = "";
            string proc_msg = "";
            string _heat_no = "";
            int _heat_seq = 999;
            string _toZone = "";
            string _heat_seq_List = GetHeatSeqList(grdMain);
            try
            {
                for (int row = GridRowsCount; row < grdMain.Rows.Count; row++)
                {
                    if (grdMain.GetData(row, "SEL").ToString() == "True")
                    {
                        this.Cursor = Cursors.AppStarting;
                        _heat_no = grdMain.GetData(row, "HEAT_NO").ToString();
                        _heat_seq = int.Parse(grdMain.GetData(row, "HEAT_SEQ").ToString());
                        //_toZone = ((ComLib.DictionaryList)this.cbx_TargetZone.SelectedItem).fnValue;

                        param.Clear();
                        param.Add(SqlDbType.VarChar, "@P_HEAT_NO", _heat_no, ParameterDirection.Input);
                        param.Add(SqlDbType.VarChar, "@P_HEAT_SEQ", _heat_seq, ParameterDirection.Input);
                        param.Add(SqlDbType.VarChar, "@P_USER_ID", ck.UserID, ParameterDirection.Input);
                        param.Add(SqlDbType.VarChar, "@P_PROC_STAT", proc_stat, ParameterDirection.Output, 4000);
                        param.Add(SqlDbType.VarChar, "@P_PROC_MSG", proc_msg, ParameterDirection.Output, 4000);
                        
                        cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                        this.Cursor = Cursors.Default;

                        //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                        //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);

                        if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                        {
                            MessageBox.Show("@P_PROC_MSG:{0}"+ cmd.Parameters["@P_PROC_MSG"].Value.ToString());
                            string _warning_msg = "HEAT_NO: " + _heat_no + "가 HEAT_SEQ:" + _heat_seq_List + "가 실적 재작업처리 실패하였습니다.";
                            clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                            return;
                        }
                    }
                }


                string _info_msg = "HEAT_NO: " + _heat_no + "가 HEAT_SEQ:" + _heat_seq_List + "가 실적 재작업처리 되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _info_msg);

                Close();

                //RTMonitoring 화면 리플레시
                //RefreshRTMonitoring();
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

        private void RefreshRTMonitoring()
        {
            var temp_form = GetForm("RTMonitoring");

            
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

        private string GetHeatSeqList(C1FlexGrid grd)
        {
            // format HEAT_NO, 표면상태(입력된것), HEAT_SEQ,USER_ID, 처리결과, 처리결과 MSG
            string rtData = "";


            DataTable dt = (DataTable)grd.DataSource;
            int counts = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["SEL"].ToString() == "True")
                    {
                        rtData += row["HEAT_SEQ"].ToString() + ",";
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

        private void ResultReWork_Load(object sender, EventArgs e)
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

                //MakeAllSelect();

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

        private void FlexGridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 120;
            grdItem.Cols[2].Width = 120;
            grdItem.Cols[3].Width = 120;
            grdItem.Cols[4].Width = 120;
            grdItem.Cols[5].Width = 120;

            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "선택";
            grdItem[1, 2] = "HEAT";
            grdItem[1, 3] = "HEAT 순번";
            grdItem[1, 4] = "강종";
            grdItem[1, 5] = "규격";
            grdItem[1, 6] = "작업일자";


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
            //SetCombo(cbx_HeatNo, GetHeatNoSql(), zoneCD, false);
            tb_HeatNo.Text = heatNo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                                             , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = A.ITEM + A.ITEM_SIZE) AS ITEM_SIZE_NM 
                                             , CONVERT(DATE, A.WORK_DATE) AS WORK_DATE 
									     FROM (
												SELECT HEAT_NO
													 , HEAT_SEQ 
													 , MAX(STEEL     ) AS STEEL    
													 , MAX(ITEM		 ) AS ITEM		
													 , MAX(ITEM_SIZE ) AS ITEM_SIZE
													 , MAX(WORK_DATE ) AS WORK_DATE
													 , MAX(REWORK_SNO) AS MAX_REWORK_SNO
												  FROM TB_GRD_WR
												 WHERE HEAT_NO LIKE '{0}%'
											  GROUP BY HEAT_NO
													 , HEAT_SEQ 
										      ) A    ", tb_HeatNo.Text);


                //SQL 쿼리 조회 후 데이터셋 return
                olddt = cd.FindDataTable(sql);



                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, olddt);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
        }

        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("SEL", FlexGridCellTypeEnum.CheckBox, "true");
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
                //clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
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

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

            SetBindingGrd();
        }

        private void pbx_Search_Click(object sender, EventArgs e)
        {
            ck.StrKey1 = tb_HeatNo.Text;
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
                tb_HeatNo.Text = ck.StrKey1;
                ck.StrKey1 = "";
                ck.StrKey2 = "";
            }
        }
    }
}
