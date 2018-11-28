using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGsystemLibrary.RsltMgmt
{
    public partial class WorkDayMgmt : Form
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
        private int main_GridRowsCount = 3;
        private int main_GridColsCount = 14;
        private int main_RowsFixed = 3;
        private int main_RowsFrozen = 0;
        private int main_ColsFixed = 0;
        private int main_ColsFrozen = 0;



        private int sub_GridRowsCount = 2;
        private int sub_GridColsCount = 6;
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
        private string lastSearchHeatNo = "";
        C1FlexGrid selectedGrd;
        List<C1FlexGrid> gridList;
        #endregion 공통 생성자

        public WorkDayMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void MWorkDayMgmt_Load(object sender, EventArgs e)
        {

            gridList = new List<C1FlexGrid>();
            gridList.Add(grdMain);
            gridList.Add(grdSub);

            //그리드 초기화
            DrawMainGrid(grdMain);
            DrawSubGrid(grdSub);
            
            //초기화
            InitControl();
            //조회버튼 클릭
            //btnDisplay_Click(null, null);

            selectedGrd = grdMain;
        }

        private void InitControl()
        {
            //조회조건 판넬 색상 세팅
            clsStyle.Style.InitPanel(panel1);
            start_dt.Value = DateTime.Now.Date;

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

        private void FlexMainGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
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
            grdItem.Cols[13].Width = 120;

            //컬럼 명 세팅
            grdItem[1, 0] =  "No";
            grdItem[1, 1] =  "HEAT";
            grdItem[1, 2] =  "강종";
            grdItem[1, 3] =  "규격";
            grdItem[1, 4] =  "그라인딩";
            grdItem[1, 5] =  "본수";
            grdItem[1, 6] =  "재작업본수";
            grdItem[1, 7] =  "PASS 수";
            grdItem[1, 8] =  "PASS 수";
            grdItem[1, 9] =  "압력";
            grdItem[1, 10] = "압력";
            grdItem[1, 11] =  "압력";
            grdItem[1, 12] =  "휠각도";
            grdItem[1, 13] =  "대차속도";


            grdItem[2, 0] = "No";
            grdItem[2, 1] =  "HEAT";
            grdItem[2, 2] =  "강종";
            grdItem[2, 3] =  "규격";
            grdItem[2, 4] =  "그라인딩";
            grdItem[2, 5] =  "본수";
            grdItem[2, 6] =  "재작업본수";
            grdItem[2, 7] =  "면";
            grdItem[2, 8] =  "코너";
            grdItem[2, 9] =  "면";
            grdItem[2, 10] = "코너";
            grdItem[2, 11] =  "끝";
            grdItem[2, 12] =  "휠각도";
            grdItem[2, 13] = "대차속도";
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
            clsFlexGrid.DataGridCenterStyle(grdItem, 0);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 1);
            clsFlexGrid.DataGridCenterStyle(grdItem, 2, 4);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 5, 6);
            clsFlexGrid.DataGridCenterStyle(grdItem, 7, grdItem.Cols.Count - 1);
        }

        private void FlexSubGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 250;
            grdItem.Cols[2].Width = 250;
            grdItem.Cols[3].Width = 150;
            grdItem.Cols[4].Width = 300;
            grdItem.Cols[5].Width = 300;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "시작일시";
            grdItem[1, 2] = "종료일시";
            grdItem[1, 3] = "정지시간(분)";
            grdItem[1, 4] = "정지사유";
            grdItem[1, 5] = "DATA_OCC_GP";

            grdItem.Cols[5].Visible = false;
            grdItem.AllowEditing = false;


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 3);
            clsFlexGrid.DataGridLeftStyle(grdItem, 4, 4);

            //clsFlexGrid.DataGridLeftStyle(grdItem, 3, 5);

        }
        private void DrawSubGrid(C1FlexGrid grdItem)
        {
            grdItem.BeginInit();

            try
            {
                int _GridRowsCount = sub_GridRowsCount;
                int _GridColsCount = sub_GridColsCount;
                int _RowsFixed = sub_RowsFixed;
                int _RowsFrozen = sub_RowsFrozen;
                int _ColsFixed = sub_ColsFixed;
                int _ColsFrozen = sub_ColsFrozen;

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexSubGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

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


                case "btnExcel":
                    SaveExcel();
                    break;

            }
        }
        
        private void SaveExcel()
        {
            grdMain.Tag = "작업일보1";
            grdSub.Tag = "작업일보2";
            vf.SaveExcel(titleNM, gridList);
            //SaveExcel(string fileName, string sheetName, FileFlags flags, PrinterSettings ps);
            //grdMain.SaveExcel("", FileFlags.IncludeMergedRanges);

        }

        private void SetDataBinding()
        {
            SetDataBinding_Grd_byinitData();

            SetDataBinding_MainGrd();

            SetDataBinding_SubGrd();
        }

        private void SetDataBinding_SubGrd()
        {
            try
            {
                DateTime startDt = start_dt.Value.AddHours(6);//2018-08-28 06:00:00
                DateTime endDt = startDt.AddDays(1);          //2018-08-29 06:00:00
                string _start_dt = vf.Format(startDt, "yyyyMMddHHmmss");
                string _end_dt = vf.Format(endDt, "yyyyMMddHHmmss");

                string sql = string.Format(@"SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY START_DDTT ASC)) AS L_NUM    
                                                  , CONVERT(VARCHAR, START_DDTT, 120) AS START_DDTT                              
                                                  , CONVERT(VARCHAR, END_DDTT, 120) AS END_DDTT    
	                                              , DATEDIFF(MINUTE, START_DDTT, END_DDTT) as  DURATION_MIN                          
	                                              , STOP_RSN                                                                 
                                                  , DATA_OCC_GP                                                              
                                               FROM TB_STOP_HR     
                                              WHERE FORMAT(START_DDTT,'yyyyMMddHHmmss')  >= '{0}'
                                                AND FORMAT(START_DDTT,'yyyyMMddHHmmss')  < '{1}'
                                                    ", _start_dt, _end_dt);


                olddtSub = cd.FindDataTable(sql);
                moddtSub = olddtSub.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdSub, moddtSub);
                this.Cursor = Cursors.Default;

                //SetGridRowSelect(grdMain, -1);

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} 작업실적: {moddtMain.Rows.Count} 건, 정지실적: {moddtSub.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void SetDataBinding_MainGrd()
        {
            try
            {
                DateTime startDt = start_dt.Value.AddHours(6);//2018-08-28 06:00:00
                DateTime endDt = startDt.AddDays(1);          //2018-08-29 06:00:00
                string _start_dt = vf.Format(startDt, "yyyyMMddHHmmss");
                string _end_dt = vf.Format(endDt, "yyyyMMddHHmmss");

                string sql = "";
                sql += string.Format(@"  SELECT  CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY A.HEAT_NO)) AS L_NUM
                                                ,HEAT_NO
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
                                                     FROM    TB_GRD_WR
                                                     WHERE   FORMAT(GRD_START_DDTT,'yyyyMMddHHmmss')  >= '{0}'
                                                         AND FORMAT(GRD_START_DDTT,'yyyyMMddHHmmss')  < '{1}'
                                                     GROUP BY HEAT_NO
                                                ) A;                 ", _start_dt, _end_dt);


                olddtMain = cd.FindDataTable(sql);
                moddtMain = olddtMain.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddtMain);
                this.Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} : {moddtMain.Rows.Count} 건 조회 되었습니다.");
                //SetGridRowSelect(grdMain, -1);
            }
            catch (Exception ex)
            {

                //throw;
            }
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

                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

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
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndUpdate();
                grdItem.Invalidate();
            }

        }

        private void SetGridRowSelect(C1FlexGrid grdMain, int v)
        {
            
        }

        private void SetDataBinding_Grd_byinitData()
        {
            InitGridData();
        }

        private void InitGridData()
        {
            clsFlexGrid.grdDataClear(grdMain, main_GridRowsCount);
            clsFlexGrid.grdDataClear(grdSub, sub_GridRowsCount);
        }
    }
}
