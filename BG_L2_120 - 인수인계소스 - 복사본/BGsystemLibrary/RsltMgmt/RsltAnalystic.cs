using BGsystemLibrary.Common;
using BGsystemLibrary.MatMgmt;
using C1.Win.C1FlexGrid;
using ChartFX.WinForms;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
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
    public partial class RsltAnalystic : Form
    {
        #region 공통 생성자
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        //데이터테이블

        DataTable olddtData;
        DataTable moddtData;

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private C1FlexGrid _selectedGrd = new C1FlexGrid();


        public string heatNo = "";     //HEAT NO

        // 프로그램 명 변수
        private static string ownerNM = "";
        private static string titleNM = "";

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한


        //C1FlexGrid selectedGrd;

        List<flexgrid> flexgridList;
        flexgrid SEL;

        flexgrid selectedFlexgridData;
        flexgrid lastSelectedFlexgrid;
        List<string> removeColListForChart;

        public DataTable chart_dt;
        private DataTable checker_dt;

        ArrayList _al = new ArrayList();
        private bool allChecked = true;

        public bool IsChangeLineOrRoutingOrCheckerInGrid { get; private set; } = false;

        #endregion 공통 생성자

        public RsltAnalystic(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void MRsltAnalystic_Load(object sender, EventArgs e)
        {

            //gridList = new List<C1FlexGrid>();
            //gridList.Add(grdSel);
            //gridList.Add(grdData);


            lastSelectedFlexgrid = new flexgrid("last");
            flexgridList = new List<flexgrid>();
            //flexgridList.Add(new flexgrid("쇼트실적"));

            SEL = new flexgrid("SEL");
            SEL.GridRowsCount = 2;
            SEL.GridColsCount = 3;
            SEL.RowsFixed = 2;
            SEL.RowsFrozen = 0;
            SEL.ColsFixed = 0;
            SEL.ColsFrozen = 0;
            SEL.TopRowsHeight = 2;
            SEL.DataRowsHeight = 35;

            flexgrid SHT_OPER_INFO = new flexgrid("SHT_OPER_INFO");
            SHT_OPER_INFO.GridRowsCount = 2;
            SHT_OPER_INFO.GridColsCount = 22;
            SHT_OPER_INFO.RowsFixed = 2;
            SHT_OPER_INFO.RowsFrozen = 0;
            SHT_OPER_INFO.ColsFixed = 0;
            SHT_OPER_INFO.ColsFrozen = 2;
            SHT_OPER_INFO.TopRowsHeight = 2;
            SHT_OPER_INFO.DataRowsHeight = 35;
            SHT_OPER_INFO.SearchOption = SearchOption.SHT_OPER_INFO;
            SHT_OPER_INFO.grd = grd_SHT_OPER_INFO;
            SHT_OPER_INFO.chart = chart1;



            removeColListForChart = new List<string>();
            //removeColList.Add("L_NUM");
            removeColListForChart.Add("WORK_DDTT");
            SHT_OPER_INFO.RemoveColList = removeColListForChart;

            flexgridList.Add(SHT_OPER_INFO);


            flexgrid SHT_DUST_INFO = new flexgrid("SHT_DUST_INFO");
            SHT_DUST_INFO.GridRowsCount = 2;
            SHT_DUST_INFO.GridColsCount = 31;
            SHT_DUST_INFO.RowsFixed = 2;
            SHT_DUST_INFO.RowsFrozen = 0;
            SHT_DUST_INFO.ColsFixed = 0;
            SHT_DUST_INFO.ColsFrozen = 2;
            SHT_DUST_INFO.TopRowsHeight = 2;
            SHT_DUST_INFO.DataRowsHeight = 35;
            SHT_DUST_INFO.SearchOption = SearchOption.SHT_DUST_INFO;
            SHT_DUST_INFO.grd = grd_SHT_DUST_INFO;
            SHT_DUST_INFO.chart = chart1;

            removeColListForChart = new List<string>();
            //removeColList.Add("L_NUM");
            removeColListForChart.Add("WORK_DDTT");


            SHT_DUST_INFO.RemoveColList = removeColListForChart;

            flexgridList.Add(SHT_DUST_INFO);


            flexgrid MPI_OPER_INFO = new flexgrid("MPI_OPER_INFO");
            MPI_OPER_INFO.GridRowsCount = 2;
            MPI_OPER_INFO.GridColsCount = 28;
            MPI_OPER_INFO.RowsFixed = 2;
            MPI_OPER_INFO.RowsFrozen = 0;
            MPI_OPER_INFO.ColsFixed = 0;
            MPI_OPER_INFO.ColsFrozen = 2;
            MPI_OPER_INFO.TopRowsHeight = 2;
            MPI_OPER_INFO.DataRowsHeight = 35;
            MPI_OPER_INFO.SearchOption = SearchOption.MPI_OPER_INFO;
            MPI_OPER_INFO.grd = grd_MPI_OPER_INFO;
            MPI_OPER_INFO.chart = chart1;

            removeColListForChart = new List<string>();
            //removeColList.Add("L_NUM");
            removeColListForChart.Add("WORK_DDTT");
            MPI_OPER_INFO.RemoveColList = removeColListForChart;

            flexgridList.Add(MPI_OPER_INFO);

            flexgrid GRD_OPER_INFO1 = new flexgrid("GRD_OPER_INFO1");
            GRD_OPER_INFO1.GridRowsCount = 2;
            GRD_OPER_INFO1.GridColsCount = 18;
            GRD_OPER_INFO1.RowsFixed = 2;
            GRD_OPER_INFO1.RowsFrozen = 0;
            GRD_OPER_INFO1.ColsFixed = 0;
            GRD_OPER_INFO1.ColsFrozen = 2;
            GRD_OPER_INFO1.TopRowsHeight = 2;
            GRD_OPER_INFO1.DataRowsHeight = 35;
            GRD_OPER_INFO1.SearchOption = SearchOption.GRD_OPER_INFO1;
            GRD_OPER_INFO1.grd = grd_GRD_OPER_INFO1;
            GRD_OPER_INFO1.chart = chart1;

            removeColListForChart = new List<string>();
            //removeColList.Add("L_NUM");
            removeColListForChart.Add("WORK_DDTT");

            GRD_OPER_INFO1.RemoveColList = removeColListForChart;

            flexgridList.Add(GRD_OPER_INFO1);

            flexgrid GRD_OPER_INFO2 = new flexgrid("GRD_OPER_INFO2");
            GRD_OPER_INFO2.GridRowsCount = 2;
            GRD_OPER_INFO2.GridColsCount = 27;
            GRD_OPER_INFO2.RowsFixed = 2;
            GRD_OPER_INFO2.RowsFrozen = 0;
            GRD_OPER_INFO2.ColsFixed = 0;
            GRD_OPER_INFO2.ColsFrozen = 2;
            GRD_OPER_INFO2.TopRowsHeight = 2;
            GRD_OPER_INFO2.DataRowsHeight = 35;
            GRD_OPER_INFO2.SearchOption = SearchOption.GRD_OPER_INFO2;
            GRD_OPER_INFO2.grd = grd_GRD_OPER_INFO2;
            GRD_OPER_INFO2.chart = chart1;

            removeColListForChart = new List<string>();
            //removeColList.Add("L_NUM");
            removeColListForChart.Add("WORK_DDTT");

            GRD_OPER_INFO2.RemoveColList = removeColListForChart;

            flexgridList.Add(GRD_OPER_INFO2);


            flexgrid GRD_DUST_INFO1 = new flexgrid("GRD_DUST_INFO1");
            GRD_DUST_INFO1.GridRowsCount = 2;
            GRD_DUST_INFO1.GridColsCount = 27;
            GRD_DUST_INFO1.RowsFixed = 2;
            GRD_DUST_INFO1.RowsFrozen = 0;
            GRD_DUST_INFO1.ColsFixed = 0;
            GRD_DUST_INFO1.ColsFrozen = 2;
            GRD_DUST_INFO1.TopRowsHeight = 2;
            GRD_DUST_INFO1.DataRowsHeight = 35;
            GRD_DUST_INFO1.SearchOption = SearchOption.GRD_DUST_INFO1;
            GRD_DUST_INFO1.grd = grd_GRD_DUST_INFO1;
            GRD_DUST_INFO1.chart = chart1;

            removeColListForChart = new List<string>();
            //removeColList.Add("L_NUM");
            removeColListForChart.Add("WORK_DDTT");

            GRD_DUST_INFO1.RemoveColList = removeColListForChart;

            flexgridList.Add(GRD_DUST_INFO1);

          


            flexgrid GRD_DUST_INFO2 = new flexgrid("GRD_DUST_INFO2");
            GRD_DUST_INFO2.GridRowsCount = 2;
            GRD_DUST_INFO2.GridColsCount = 25;
            GRD_DUST_INFO2.RowsFixed = 2;
            GRD_DUST_INFO2.RowsFrozen = 0;
            GRD_DUST_INFO2.ColsFixed = 0;
            GRD_DUST_INFO2.ColsFrozen = 2;
            GRD_DUST_INFO2.TopRowsHeight = 2;
            GRD_DUST_INFO2.DataRowsHeight = 35;
            GRD_DUST_INFO2.SearchOption = SearchOption.GRD_DUST_INFO2;
            GRD_DUST_INFO2.grd = grd_GRD_DUST_INFO2;
            GRD_DUST_INFO2.chart = chart1;

            removeColListForChart = new List<string>();
            //removeColList.Add("L_NUM");
            removeColListForChart.Add("WORK_DDTT");
            GRD_DUST_INFO2.RemoveColList = removeColListForChart;

            flexgridList.Add(GRD_DUST_INFO2);

            //flexgrid temp = flexgridList.Find(x => x.Name == "쇼트실적");
            //그리드 초기화



            //DrawMainGrid(grdSel);
            //DrawSubGrid(grdData);

            //초기화
            InitControl();
            //조회버튼 클릭
            //btnDisplay_Click(null, null);

            //selectedGrd = grdCheck;

            DrawSelGrid();

            DrawDataGrid();


            flexgrid griddata = flexgridList.Find(x => x.SearchOption == GetselSearchOption());
            

            selectedFlexgridData = griddata;

            ShowActivateGrid();

            //btnDisplay.PerformClick();
        }

        private void DrawDataGrid()
        {

            foreach (flexgrid griddata in flexgridList)
            {
                C1FlexGrid grdItem = griddata.grd as C1FlexGrid;

                grdItem.BeginInit();
                try
                {

                    clsFlexGrid.FlexGridMainSystem(grdItem, griddata.GridRowsCount, griddata.GridColsCount, griddata.RowsFixed, griddata.RowsFrozen, griddata.ColsFixed, griddata.ColsFrozen);

                    //컬럼 스타일 세팅
                    FlexDataGridCol(grdItem, griddata);
                    //컬럼 높이 세팅
                    clsFlexGrid.FlexGridRow(grdItem, griddata.TopRowsHeight, griddata.DataRowsHeight);

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
            

            
        }

        private void FlexDataGridCol(C1FlexGrid grdItem, flexgrid datagrid)
        {
            switch (datagrid.SearchOption)
            {
                case SearchOption.SHT_OPER_INFO:
                    Set_Cols_SHT_OPER_INFO(grdItem);
                    break;
                case SearchOption.SHT_DUST_INFO:
                    Set_Cols_SHT_DUST_INFO(grdItem);
                    break;
                case SearchOption.MPI_OPER_INFO:
                    Set_Cols_MPI_OPER_INFO(grdItem);
                    break;
                case SearchOption.GRD_OPER_INFO1:
                    Set_Cols_GRD_OPER_INFO1(grdItem);
                    break;
                case SearchOption.GRD_OPER_INFO2:
                    Set_Cols_GRD_OPER_INFO2(grdItem);
                    break;
                case SearchOption.GRD_DUST_INFO1:
                    Set_Cols_GRD_DUST_INFO1(grdItem);
                    break;
                case SearchOption.GRD_DUST_INFO2:
                    Set_Cols_GRD_DUST_INFO2(grdItem);
                    break;
                default:
                    break;
            }
        }

        private void FlexDataGridSql(flexgrid datagrid)
        {
            switch (datagrid.SearchOption)
            {
                case SearchOption.SHT_OPER_INFO:
                    Set_Sql_SHT_OPER_INFO(datagrid);
                    break;
                case SearchOption.SHT_DUST_INFO:
                    Set_Sql_SHT_DUST_INFO(datagrid);
                    break;
                case SearchOption.MPI_OPER_INFO:
                    Set_Sql_MPI_OPER_INFO(datagrid);
                    break;
                case SearchOption.GRD_OPER_INFO1:
                    Set_Sql_GRD_OPER_INFO1(datagrid);
                    break;
                case SearchOption.GRD_OPER_INFO2:
                    Set_Sql_GRD_OPER_INFO2(datagrid);
                    break;
                case SearchOption.GRD_DUST_INFO1:
                    Set_Sql_GRD_DUST_INFO1(datagrid);
                    break;
                case SearchOption.GRD_DUST_INFO2:
                    Set_Sql_GRD_DUST_INFO2(datagrid);
                    break;
            }
        }
        private void Set_Sql_GRD_OPER_INFO1(flexgrid datagrid)
        {
            string heat_no = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue1;
            string heat_seq = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue2;
            string rework_sno = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue3;

            datagrid.Sql = string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY WORK_DDTT ASC)) AS L_NUM    
                                               ,CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                       
                                               ,ISNULL(HEIGHT_SETV          ,0) AS  HEIGHT_SETV                  --   높이 설정값                                        
                                               ,ISNULL(WIDTH_SETV           ,0) AS  WIDTH_SETV                   --   폭 설정값                                        
                                               ,ISNULL(HEIGHT_MV            ,0) AS  HEIGHT_MV                    --   높이 측정값                                        
                                               ,ISNULL(WIDTH_MV             ,0) AS  WIDTH_MV                     --   폭 측정값                                        
                                               ,ISNULL(LENGTH_MV            ,0) AS  LENGTH_MV                    --   길이 측정값                                        
                                               ,ISNULL(FACE_PASS_CNT        ,0) AS  FACE_PASS_CNT                --   면 Pass수                                        
                                               ,ISNULL(CORN_PASS_CNT        ,0) AS  CORN_PASS_CNT                --   코너 Pass수                                        
                                               ,ISNULL(WORK_SEQ             ,0) AS  WORK_SEQ                     --   작업 순서                                        
                                               ,ISNULL(RBGW_SPEED_SETV      ,0) AS  RBGW_SPEED_SETV              --   연마지석 속도 설정값                                        
                                               ,ISNULL(RBGW_ADV_SETV        ,0) AS  RBGW_ADV_SETV                --   연마지석 전진 설정값                                        
                                               ,ISNULL(RBGW_DIA_MV          ,0) AS  RBGW_DIA_MV                  --   연마지석 직경 측정값                                        
                                               ,ISNULL(RBGW_ENGY_MV         ,0) AS  RBGW_ENGY_MV                 --   연마지석 에너지 측정값                                        
                                               ,ISNULL(RBGW_VIBR_MV         ,0) AS  RBGW_VIBR_MV                 --   연마지석 진동 측정값                                        
                                               ,ISNULL(ROTANGLE_SETV        ,0) AS  ROTANGLE_SETV                --   회전각도 설정값                                        
                                               ,ISNULL(ROTANGLE_MV          ,0) AS  ROTANGLE_MV                  --   회전각도 측정값                                        
                                               ,ISNULL(GRD_PRES_SURFACE_SETV,0) AS  GRD_PRES_SURFACE_SETV        --   그라인딩 압력 표면 설정값                                        
									          FROM  TB_GRD_OPER_INFO A
									     	    ,  (
									     				SELECT  TOP 1 
									     			            GRD_START_DDTT
									     			          , GRD_END_DDTT                                                                                     
									     			       FROM TB_GRD_WR                                                                                        
									     			      WHERE HEAT_NO  = '{0}'
									     			    	AND HEAT_SEQ = '{1}'
									     			    	AND REWORK_SNO = '{2}'
									     		   ) B
									     	 WHERE FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  >= FORMAT(B.GRD_START_DDTT,'yyyyMMddHHmmss') 
									     	   AND FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  < FORMAT(B.GRD_END_DDTT,'yyyyMMddHHmmss') 
                                                          ", heat_no, heat_seq, rework_sno);
        }
        private void Set_Sql_GRD_OPER_INFO2(flexgrid datagrid)
        {
            string heat_no = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue1;
            string heat_seq = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue2;
            string rework_sno = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue3;


            datagrid.Sql = string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY WORK_DDTT ASC)) AS L_NUM    
                                               ,ISNULL(CONVERT(VARCHAR(19),WORK_DDTT, 120), 0) AS WORK_DDTT                       
                                               ,ISNULL(CONVERT(INT, WORK_Track_NO)        ,0) AS  WORK_Track_NO  
                                               ,ISNULL(GRD_PRES_ENDP_SETV   ,0) AS  GRD_PRES_ENDP_SETV           --   그라인딩 압력 끝단 설정값                                        
                                               ,ISNULL(GRD_PRES_CORN_SETV   ,0) AS  GRD_PRES_CORN_SETV           --   그라인딩 압력 코너 설정값
											   ,ISNULL(SET_GRD_Force        ,0) AS  SET_GRD_Force                --   그라인딩 압력 Force                                       
                                               ,ISNULL(GRD_PRES_MV          ,0) AS  GRD_PRES_MV                  --   그라인딩 모터 전력값,                                        
                                               ,ISNULL(TRUCKSPEED_SETV      ,0) AS  TRUCKSPEED_SETV              --   대차속도 설정값                                                
                                               ,ISNULL(TRUCKSPEED_MV        ,0) AS  TRUCKSPEED_MV                --   대차속도 측정값                                                      
                                               ,ISNULL(CONVERT(INT, GRD_LOC)              ,0) AS  GRD_LOC        --   그라인딩 위치    
                                               ,ISNULL(COOL_DOWN_HR_SETV    ,0) AS  COOL_DOWN_HR_SETV            --   쿨다운 시간 설정값                                    
                                               ,ISNULL(COOL_DOWN_SPEED_SETV ,0) AS  COOL_DOWN_SPEED_SETV         --   쿨다운 속도 설정값 ?                                
                                               ,ISNULL(BELT_SLIP_WARN_SETV  ,0) AS  BELT_SLIP_WARN_SETV          --   벨트 슬립 경보 설정값 ?                                  
                                               ,ISNULL(CORN_GRD_3TRK1bs     ,0) AS  CORN_GRD_3TRK1bs             --   코너 그라인드 3트랙1bs                                     
                                               ,ISNULL(CORN_GRD_3TRK2bs     ,0) AS  CORN_GRD_3TRK2bs             --   코너 그라인드 3트랙2bs                                        
                                               ,ISNULL(CORN_GRD_3TRK3fs     ,0) AS  CORN_GRD_3TRK3fs             --   코너 그라인드 3트랙3fs                                       
                                               ,ISNULL(CORN_GRD_5TRK1bs     ,0) AS  CORN_GRD_5TRK1bs             --   코너 그라인드 5트랙1bs                                       
                                               ,ISNULL(CORN_GRD_5TRK2bs     ,0) AS  CORN_GRD_5TRK2bs             --   코너 그라인드 5트랙2bs                                       
                                               ,ISNULL(CORN_GRD_5TRK3bs     ,0) AS  CORN_GRD_5TRK3bs             --   코너 그라인드 5트랙3bs                                       
                                               ,ISNULL(CORN_GRD_5TRK4fs     ,0) AS  CORN_GRD_5TRK4fs             --   코너 그라인드 5트랙4fs                                       
                                               ,ISNULL(CORN_GRD_5TRK5fs     ,0) AS  CORN_GRD_5TRK5fs             --   코너 그라인드 5트랙5fs                                       
                                               ,ISNULL(OutsIDe_back         ,0) AS  OutsIDe_back                 --   Outside back                                                 
                                               ,ISNULL(OutsIDe_front        ,0) AS  OutsIDe_front                --   Outside front                                                
                                               ,ISNULL(CONVERT(INT, WORK_SETV_Top   )     ,0) AS  WORK_SETV_Top                --   작업 설정값 Top                                    
                                               ,ISNULL(CONVERT(INT, WORK_SETV_Right )     ,0) AS  WORK_SETV_Right              --   작업 설정값 Right                                   
                                               ,ISNULL(CONVERT(INT, WORK_SETV_Bottom)     ,0) AS  WORK_SETV_Bottom             --   작업 설정값 Bottom                                    
                                               ,ISNULL(CONVERT(INT, WORK_SETV_Left)     ,0) AS  WORK_SETV_Left             --   작업 설정값 Bottom                                    
                                               --,ISNULL( CASE WHEN DATALENGTH(WORK_SETV_Left) = 0 THEN 0 ELSE CONVERT(INT, WORK_SETV_Left) END ,0)  AS  WORK_SETV_Left               --   작업 설정값 Left           
									          FROM  TB_GRD_OPER_INFO A
									     	    ,  (
									     				SELECT  TOP 1 
									     			            GRD_START_DDTT
									     			          , GRD_END_DDTT                                                                                     
									     			       FROM TB_GRD_WR                                                                                        
									     			      WHERE HEAT_NO  = '{0}'
									     			    	AND HEAT_SEQ = '{1}'
									     			    	AND REWORK_SNO = '{2}'
									     		   ) B
									     	 WHERE FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  >= FORMAT(B.GRD_START_DDTT,'yyyyMMddHHmmss') 
									     	   AND FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  < FORMAT(B.GRD_END_DDTT,'yyyyMMddHHmmss') 
                                                          ", heat_no, heat_seq, rework_sno);
        }
        private void Set_Sql_GRD_DUST_INFO1(flexgrid datagrid)
        {
            string heat_no = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue1;
            string heat_seq = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue2;
            string rework_sno = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue3;

            datagrid.Sql = string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY WORK_DDTT ASC)) AS L_NUM  
                                             ,CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                      
                                             ,ISNULL(Bag_BEF_STATICP              , 0) AS Bag_BEF_STATICP              
                                             ,ISNULL(INH_TEMP                     , 0) AS INH_TEMP                                                         
                                             ,ISNULL(Rotary_Valve_NO1_STAT        , 0) AS Rotary_Valve_NO1_STAT        
                                             ,ISNULL(Rotary_Valve_NO1_ABNORM_STAT , 0) AS Rotary_Valve_NO1_ABNORM_STAT 
                                             ,ISNULL(Rotary_Valve_NO2_STAT        , 0) AS Rotary_Valve_NO2_STAT        
                                             ,ISNULL(Rotary_Valve_NO2_ABNORM_STAT , 0) AS Rotary_Valve_NO2_ABNORM_STAT 
                                             ,ISNULL(Rotary_Valve_NO3_STAT        , 0) AS Rotary_Valve_NO3_STAT        
                                             ,ISNULL(Rotary_Valve_NO3_ABNORM_STAT , 0) AS Rotary_Valve_NO3_ABNORM_STAT 
                                             ,ISNULL(Rotary_Valve_NO4_STAT        , 0) AS Rotary_Valve_NO4_STAT        
                                             ,ISNULL(Rotary_Valve_NO4_ABNORM_STAT , 0) AS Rotary_Valve_NO4_ABNORM_STAT 
                                             ,ISNULL(Vibrator_NO1_STAT            , 0) AS Vibrator_NO1_STAT            
                                             ,ISNULL(Vibrator_NO1_ABNORM_STAT     , 0) AS Vibrator_NO1_ABNORM_STAT     
                                             ,ISNULL(Vibrator_NO2_STAT            , 0) AS Vibrator_NO2_STAT            
                                             ,ISNULL(Vibrator_NO2_ABNORM_STAT     , 0) AS Vibrator_NO2_ABNORM_STAT     
                                             ,ISNULL(Vibrator_NO3_STAT            , 0) AS Vibrator_NO3_STAT            
                                             ,ISNULL(Vibrator_NO3_ABNORM_STAT     , 0) AS Vibrator_NO3_ABNORM_STAT     
                                             ,ISNULL(Pulse_Controller_STAT1       , 0) AS Pulse_Controller_STAT1       
                                             ,ISNULL(Pulse_Controller_STAT2       , 0) AS Pulse_Controller_STAT2       
                                             ,ISNULL(Pulse_Controller_STAT3       , 0) AS Pulse_Controller_STAT3       
                                             ,ISNULL(Pulse_Controller_STAT4       , 0) AS Pulse_Controller_STAT4       
                                             ,ISNULL(Pulse_Controller_STAT5       , 0) AS Pulse_Controller_STAT5       
                                             ,ISNULL(Pulse_Controller_STAT6       , 0) AS Pulse_Controller_STAT6       
                                             ,ISNULL(Pulse_Controller_STAT7       , 0) AS Pulse_Controller_STAT7       
                                             ,ISNULL(Pulse_Controller_STAT8       , 0) AS Pulse_Controller_STAT8       
                                             ,ISNULL(Pulse_Controller_STAT9       , 0) AS Pulse_Controller_STAT9                     
									          FROM  TB_GRD_DUST_INFO A
									     	    ,  (
									     				SELECT  TOP 1 
									     			            GRD_START_DDTT
									     			          , GRD_END_DDTT                                                                                     
									     			       FROM TB_GRD_WR                                                                                        
									     			      WHERE HEAT_NO  = '{0}'
									     			    	AND HEAT_SEQ = '{1}'
									     			    	AND REWORK_SNO = '{2}'
									     		   ) B
									     	 WHERE FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  >= FORMAT(B.GRD_START_DDTT,'yyyyMMddHHmmss') 
									     	   AND FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  < FORMAT(B.GRD_END_DDTT,'yyyyMMddHHmmss') 
                                                          ", heat_no, heat_seq, rework_sno);
        }

        private void Set_Sql_GRD_DUST_INFO2(flexgrid datagrid)
        {
            string heat_no = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue1;
            string heat_seq = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue2;
            string rework_sno = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue3;

            datagrid.Sql = string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY WORK_DDTT ASC)) AS L_NUM  
                                             ,CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                      
                                             ,ISNULL(Pulse_Controller_STAT10      , 0) AS Pulse_Controller_STAT10      
                                             ,ISNULL(Pulse_Controller_STAT11      , 0) AS Pulse_Controller_STAT11      
                                             ,ISNULL(Pulse_Controller_STAT12      , 0) AS Pulse_Controller_STAT12      
                                             ,ISNULL(Pulse_Controller_STAT13      , 0) AS Pulse_Controller_STAT13      
                                             ,ISNULL(Pulse_Controller_STAT14      , 0) AS Pulse_Controller_STAT14      
                                             ,ISNULL(Pulse_Controller_STAT15      , 0) AS Pulse_Controller_STAT15      
                                             ,ISNULL(Pulse_Controller_STAT16      , 0) AS Pulse_Controller_STAT16      
                                             ,ISNULL(Pulse_Controller_STAT17      , 0) AS Pulse_Controller_STAT17      
                                             ,ISNULL(Pulse_Controller_STAT18      , 0) AS Pulse_Controller_STAT18      
                                             ,ISNULL(Pulse_Controller_STAT19      , 0) AS Pulse_Controller_STAT19      
                                             ,ISNULL(Popet_Damper_NO1_STAT        , 0) AS Popet_Damper_NO1_STAT        
                                             ,ISNULL(Popet_Damper_NO2_STAT        , 0) AS Popet_Damper_NO2_STAT        
                                             ,ISNULL(Popet_Damper_NO3_STAT        , 0) AS Popet_Damper_NO3_STAT        
                                             ,ISNULL(DEF_PRES_NO1                 , 0) AS DEF_PRES_NO1                 
                                             ,ISNULL(DEF_PRES_NO2                 , 0) AS DEF_PRES_NO2                 
                                             ,ISNULL(DEF_PRES_NO3                 , 0) AS DEF_PRES_NO3                 
                                             ,ISNULL(OPRN_RT                      , 0) AS OPRN_RT                      
                                             ,ISNULL(SETV                         , 0) AS SETV                         
                                             ,ISNULL(LOAD_BRG_TEMP                , 0) AS LOAD_BRG_TEMP                
                                             ,ISNULL(H_LOAD_BRG_TEMP              , 0) AS H_LOAD_BRG_TEMP              
                                             ,ISNULL(IVT_SPEED_SETV               , 0) AS IVT_SPEED_SETV               
                                             ,ISNULL(LOAD_VIBR                    , 0) AS LOAD_VIBR                    
                                             ,ISNULL(H_LOAD_VIBR                  , 0) AS H_LOAD_VIBR                  
									          FROM  TB_GRD_DUST_INFO A
									     	    ,  (
									     				SELECT  TOP 1 
									     			            GRD_START_DDTT
									     			          , GRD_END_DDTT                                                                                     
									     			       FROM TB_GRD_WR                                                                                        
									     			      WHERE HEAT_NO  = '{0}'
									     			    	AND HEAT_SEQ = '{1}'
									     			    	AND REWORK_SNO = '{2}'
									     		   ) B
									     	 WHERE FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  >= FORMAT(B.GRD_START_DDTT,'yyyyMMddHHmmss') 
									     	   AND FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  < FORMAT(B.GRD_END_DDTT,'yyyyMMddHHmmss') 
                                                          ", heat_no, heat_seq, rework_sno);
        }




        private void Set_Sql_MPI_OPER_INFO(flexgrid datagrid)
        {
            string heat_no = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue1;
            string heat_seq = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue2;
            string rework_sno = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue3;

            datagrid.Sql = string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY WORK_DDTT ASC)) AS L_NUM   
                                               ,CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                       
                                               ,ISNULL(CLWTR_PMP1_STAT       ,0) AS CLWTR_PMP1_STAT                                                        
                                               ,ISNULL(CLWTR_PMP2_STAT       ,0) AS CLWTR_PMP2_STAT                                                        
                                               ,ISNULL(FIL_MTR_STAT          ,0) AS FIL_MTR_STAT                                                           
                                               ,ISNULL(CLWTR_PMP_HDR_STAT    ,0) AS CLWTR_PMP_HDR_STAT                                                     
                                               ,ISNULL(MAGNET_PMP1_STAT      ,0) AS MAGNET_PMP1_STAT                                                       
                                               ,ISNULL(MAGNET_PMP2_STAT      ,0) AS MAGNET_PMP2_STAT                                                       
                                               ,ISNULL(STIR_PMP_HDR_STAT     ,0) AS STIR_PMP_HDR_STAT                                                      
                                               ,ISNULL(Entry_Motor_FWD       ,0) AS Entry_Motor_FWD                                                        
                                               ,ISNULL(Entry_Motor_REV       ,0) AS Entry_Motor_REV                                                        
                                               ,ISNULL(Inspection_Motor_FWD  ,0) AS Inspection_Motor_FWD                                                   
                                               ,ISNULL(Inspection_Motor_REV  ,0) AS Inspection_Motor_REV                                                   
                                               ,ISNULL(OPRS_Motor1_STAT      ,0) AS OPRS_Motor1_STAT                                                       
                                               ,ISNULL(OPRS_Motor2_STAT      ,0) AS OPRS_Motor2_STAT                                                       
                                               ,ISNULL(OPRS_Fan_STAT         ,0) AS OPRS_Fan_STAT                                                          
                                               ,ISNULL(Dark_Room_Hood_Fan    ,0) AS Dark_Room_Hood_Fan                                                     
                                               ,ISNULL(Yoke_TRANS_Motor1_FWD ,0) AS Yoke_TRANS_Motor1_FWD                                                  
                                               ,ISNULL(Yoke_TRANS_Motor1_BWD ,0) AS Yoke_TRANS_Motor1_BWD                                                  
                                               ,ISNULL(Yoke_TRANS_Motor2_BWD ,0) AS Yoke_TRANS_Motor2_BWD                                                  
                                               ,ISNULL(Yoke_TRANS_Motor2_FWD ,0) AS Yoke_TRANS_Motor2_FWD                                                  
                                               ,ISNULL(OPRS_MTR_HDR_STAT     ,0) AS OPRS_MTR_HDR_STAT                                                      
                                               ,ISNULL(OPRS_SUP_HDR_STAT     ,0) AS OPRS_SUP_HDR_STAT                                                      
                                               ,ISNULL(Yoke_MTR_NO1_CURRENT  ,0) AS Yoke_MTR_NO1_CURRENT                                                 
                                               ,ISNULL(Yoke_MTR_NO2_CURRENT  ,0) AS Yoke_MTR_NO2_CURRENT                                                 
                                               ,ISNULL(Yoke_MTR_NO3_CURRENT  ,0) AS Yoke_MTR_NO3_CURRENT                                                 
                                               ,ISNULL(Yoke_MTR_NO4_CURRENT  ,0) AS Yoke_MTR_NO4_CURRENT                                                 
                                               ,ISNULL(COIL_NO1_2_CURRENT    ,0) AS COIL_NO1_2_CURRENT 
									          FROM  TB_MPI_OPER_INFO A
									     	    ,  (
									     				SELECT  TOP 1 
									     			            MPI_START_DDTT
									     			          , MPI_END_DDTT                                                                                     
									     			       FROM TB_MPI_WR                                                                                        
									     			      WHERE HEAT_NO  = '{0}'
									     			    	AND HEAT_SEQ = '{1}'
									     			    	AND REWORK_SNO = '{2}'
									     		   ) B
									     	 WHERE FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  >= FORMAT(B.MPI_START_DDTT,'yyyyMMddHHmmss') 
									     	   AND FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  < FORMAT(B.MPI_END_DDTT,'yyyyMMddHHmmss') 
                                                          ", heat_no, heat_seq, rework_sno);
        }

        private void Set_Sql_SHT_DUST_INFO(flexgrid datagrid)
        {
            string heat_no = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue1;
            string heat_seq = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue2;
            string rework_sno = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue3;

            datagrid.Sql = string.Format(@" SELECT  CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY WORK_DDTT ASC)) AS L_NUM  
                                                  , CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                     
                                                  , ISNULL( Bag_BEF_STATICP           , 0) AS Bag_BEF_STATICP
                                                  , ISNULL( INH_TEMP                  , 0) AS INH_TEMP
                                                  , ISNULL( Screw_Conveyor_STAT       , 0) AS Screw_Conveyor_STAT
                                                  , ISNULL( Screw_Conveyor_ABNORM_STAT, 0) AS Screw_Conveyor_ABNORM_STAT
                                                  , ISNULL( Rotary_Valve_STAT         , 0) AS Rotary_Valve_STAT
                                                  , ISNULL( Rotary_Valve_ABNORM_STAT  , 0) AS Rotary_Valve_ABNORM_STAT
                                                  , ISNULL( Vibrator_STAT             , 0) AS Vibrator_STAT
                                                  , ISNULL( Vibrator_ABNORM_STAT      , 0) AS Vibrator_ABNORM_STAT
                                                  , ISNULL( Pulse_Controller_STAT1    , 0) AS Pulse_Controller_STAT1
                                                  , ISNULL( Pulse_Controller_STAT2    , 0) AS Pulse_Controller_STAT2
                                                  , ISNULL( Pulse_Controller_STAT3    , 0) AS Pulse_Controller_STAT3
                                                  , ISNULL( Pulse_Controller_STAT4    , 0) AS Pulse_Controller_STAT4
                                                  , ISNULL( Pulse_Controller_STAT5    , 0) AS Pulse_Controller_STAT5
                                                  , ISNULL( Pulse_Controller_STAT6    , 0) AS Pulse_Controller_STAT6
                                                  , ISNULL( Pulse_Controller_STAT7    , 0) AS Pulse_Controller_STAT7
                                                  , ISNULL( Pulse_Controller_STAT8    , 0) AS Pulse_Controller_STAT8
                                                  , ISNULL( Pulse_Controller_STAT9    , 0) AS Pulse_Controller_STAT9
                                                  , ISNULL( Pulse_Controller_STAT10   , 0) AS Pulse_Controller_STAT10
                                                  , ISNULL( Pulse_Controller_STAT11   , 0) AS Pulse_Controller_STAT11
                                                  , ISNULL( Pulse_Controller_STAT12   , 0) AS Pulse_Controller_STAT12
                                                  , ISNULL( Pulse_Controller_STAT13   , 0) AS Pulse_Controller_STAT13
                                                  , ISNULL( DEF_PRES                  , 0) AS DEF_PRES
                                                  , ISNULL( OPRN_RT                   , 0) AS OPRN_RT
                                                  , ISNULL( SETV                      , 0) AS SETV
                                                  , ISNULL( LOAD_BRG_TEMP             , 0) AS LOAD_BRG_TEMP
                                                  , ISNULL( H_LOAD_BRG_TEMP           , 0) AS H_LOAD_BRG_TEMP
                                                  , ISNULL( IVT_SPEED_SETV            , 0) AS IVT_SPEED_SETV
                                                  , ISNULL( LOAD_VIBR                 , 0) AS LOAD_VIBR
                                                  , ISNULL( H_LOAD_VIBR               , 0) AS H_LOAD_VIBR 
									          FROM  TB_SHT_DUST_INFO A
									     	    ,  (
									     				SELECT  TOP 1 
									     			            SHT_START_DDTT
									     			          , SHT_END_DDTT                                                                                     
									     			       FROM TB_SHT_WR                                                                                        
									     			      WHERE HEAT_NO  = '{0}'
									     			    	AND HEAT_SEQ = '{1}'
									     			    	AND REWORK_SNO = '{2}'
									     		   ) B
									     	 WHERE FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  >= FORMAT(B.SHT_START_DDTT,'yyyyMMddHHmmss') 
									     	   AND FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  < FORMAT(B.SHT_END_DDTT,'yyyyMMddHHmmss') 
                                                          ", heat_no, heat_seq, rework_sno);
        }

        private void Set_Sql_SHT_OPER_INFO(flexgrid datagrid)
        {

            string heat_no = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue1;
            string heat_seq = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue2;
            string rework_sno = ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnValue3;

            datagrid.Sql = string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY WORK_DDTT )) AS L_NUM  
                                                 , CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                      
                                                 , ISNULL(ROLLER_CONVR_SPEED     , 0)  AS ROLLER_CONVR_SPEED                                                  
                                                 , ISNULL(IMPELLER_NO1_SPEED     , 0)  AS IMPELLER_NO1_SPEED                                                 
                                                 , ISNULL(IMPELLER_NO2_SPEED     , 0)  AS IMPELLER_NO2_SPEED                                                 
                                                 , ISNULL(IMPELLER_NO3_SPEED     , 0)  AS IMPELLER_NO3_SPEED                                                 
                                                 , ISNULL(IMPELLER_NO4_SPEED     , 0)  AS IMPELLER_NO4_SPEED                                                 
                                                 , ISNULL(ROLLER_CONVR_CURRENT   , 0)  AS ROLLER_CONVR_CURRENT                                               
                                                 , ISNULL(IMPELLER_NO1_CURRENT   , 0)  AS IMPELLER_NO1_CURRENT                                               
                                                 , ISNULL(IMPELLER_NO2_CURRENT   , 0)  AS IMPELLER_NO2_CURRENT                                               
                                                 , ISNULL(IMPELLER_NO3_CURRENT   , 0)  AS IMPELLER_NO3_CURRENT                                               
                                                 , ISNULL(IMPELLER_NO4_CURRENT   , 0)  AS IMPELLER_NO4_CURRENT                                               
                                                 , ISNULL(ROTY_SCR_CURRENT       , 0)  AS ROTY_SCR_CURRENT                                                   
                                                 , ISNULL(BCKT_LVTR_CURRENT      , 0)  AS BCKT_LVTR_CURRENT                                                  
                                                 , ISNULL(SCREW_CONVR1_CURRENT   , 0)  AS SCREW_CONVR1_CURRENT                                               
                                                 , ISNULL(SCREW_CONVR2_CURRENT   , 0)  AS SCREW_CONVR2_CURRENT                                               
                                                 , ISNULL(FAN_CURRENT            , 0)  AS FAN_CURRENT                                                        
                                                 , ISNULL(ROLLER_CONVR_SPEED_SETV, 0)  AS ROLLER_CONVR_SPEED_SETV                                            
                                                 , ISNULL(IMPELLER_NO1_SPEED_SETV, 0)  AS IMPELLER_NO1_SPEED_SETV                                            
                                                 , ISNULL(IMPELLER_NO2_SPEED_SETV, 0)  AS IMPELLER_NO2_SPEED_SETV                                            
                                                 , ISNULL(IMPELLER_NO3_SPEED_SETV, 0)  AS IMPELLER_NO3_SPEED_SETV                                            
                                                 , ISNULL(IMPELLER_NO4_SPEED_SETV, 0)  AS IMPELLER_NO4_SPEED_SETV
									          FROM  TB_SHT_OPER_INFO A
									     	    ,  (
									     				SELECT  TOP 1 
									     			            SHT_START_DDTT
									     			          , SHT_END_DDTT                                                                                     
									     			       FROM TB_SHT_WR                                                                                        
									     			      WHERE HEAT_NO  = '{0}'
									     			    	AND HEAT_SEQ = '{1}'
									     			    	AND REWORK_SNO = '{2}'
									     		   ) B
									     	 WHERE FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  >= FORMAT(B.SHT_START_DDTT,'yyyyMMddHHmmss') 
									     	   AND FORMAT(A.WORK_DDTT,'yyyyMMddHHmmss')  < FORMAT(B.SHT_END_DDTT,'yyyyMMddHHmmss') 
                                                          ", heat_no, heat_seq, rework_sno);


        }



        private void Set_Cols_GRD_OPER_INFO1(C1FlexGrid grdItem)
        {
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "높이 설정값";
            grdItem[1, 3] = "폭 설정값";
            grdItem[1, 4] = "높이 측정값";
            grdItem[1, 5] = "폭 측정값";
            grdItem[1, 6] = "길이 측정값";
            grdItem[1, 7] = "면 Pass수";
            grdItem[1, 8] = "코더 Pass수";
            grdItem[1, 9] =  "작업순서";
            grdItem[1, 10] = "연마지석 속도 설정값";
            grdItem[1, 11] = "연마지석 전진 설정값";
            grdItem[1, 12] = "연마지석 직경 측정값";
            grdItem[1, 13] = "연마지석 에너지 측정값";
            grdItem[1, 14] = "연마지석 진동 측정값";
            grdItem[1, 15] = "회전각도 설정값";
            grdItem[1, 16] = "회전각도 측정값";
            grdItem[1, 17] = "그라인딩압력표면 설정값";


            grdItem.AllowEditing = false;
            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            //clsFlexGrid.DataGridFormat(grdItem, 3, 42, "N0");
            clsFlexGrid.DataGridFormat(grdItem, 2, grdItem.Cols.Count - 1, "N0");
            grdItem.AutoSizeCols();
        }
        private void Set_Cols_GRD_OPER_INFO2(C1FlexGrid grdItem)
        {
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";                  
            grdItem[1, 2] = "작업TrackNo";
            grdItem[1, 3] = "그라인딩압력끝단설정값";
            grdItem[1, 4] = "그라인딩압력코너설정값";
            grdItem[1, 5] = "그라인딩압력Force";
            grdItem[1, 6] = "그라인딩모터전력값,";
            grdItem[1, 7] = "대차속도설정값";
            grdItem[1, 8] = "대차속도측정값";
            grdItem[1, 9] = "그라인딩위치";
            grdItem[1, 10] = "쿨다운시간설정값";
            grdItem[1, 11] = "쿨다운속도설정값?";
            grdItem[1, 12] = "벨트슬립경보설정값?";
            grdItem[1, 13] = "코너그라인드3트랙1bs";
            grdItem[1, 14] = "코너그라인드3트랙2bs";
            grdItem[1, 15] = "코너그라인드3트랙3fs";
            grdItem[1, 16] = "코너그라인드5트랙1bs";
            grdItem[1, 17] = "코너그라인드5트랙2bs";
            grdItem[1, 18] = "코너그라인드5트랙3bs";
            grdItem[1, 19] = "코너그라인드5트랙4fs";
            grdItem[1, 20] = "코너그라인드5트랙5fs";
            grdItem[1, 21] = "Outsideback";
            grdItem[1, 22] = "Outsidefront";
            grdItem[1, 23] = "작업설정값Top";
            grdItem[1, 24] = "작업설정값Right";
            grdItem[1, 25] = "작업설정값Bottom";
            grdItem[1, 26] = "작업설정값Left";



            grdItem.AllowEditing = false;
            //grdItem.Cols[2].Visible = false;
            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            //clsFlexGrid.DataGridFormat(grdItem, 3, 42, "N0");
            //clsFlexGrid.DataGridFormat(grdItem, 3, grdItem.Cols.Count - 1, "N0");
            grdItem.AutoSizeCols();
        }
        private void Set_Cols_GRD_DUST_INFO1(C1FlexGrid grdItem)
        {
            #region //컬럼 명 세팅
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "Bag전단정압계";
            grdItem[1, 3] = "흡입 온도";
            grdItem[1, 4] = "Rotary Valve#1 상태";
            grdItem[1, 5] = "Rotary Valve#1 이상상태";
            grdItem[1, 6] = "Rotary Valve#2 상태";
            grdItem[1, 7] = "Rotary Valve#2 이상상태";
            grdItem[1, 8] =  "Rotary Valve#3 상태";
            grdItem[1, 9] =  "Rotary Valve#3 이상상태";
            grdItem[1, 10] = "Rotary Valve#4 상태";
            grdItem[1, 11] = "Rotary Valve#4 이상상태";
            grdItem[1, 12] = "Vibrator#1 상태";
            grdItem[1, 13] = "Vibrator#1 이상상태";
            grdItem[1, 14] = "Vibrator#2 상태";
            grdItem[1, 15] = "Vibrator#2 이상상태";
            grdItem[1, 16] = "Vibrator#3 상태";
            grdItem[1, 17] = "Vibrator#3 이상상태";
            grdItem[1, 18] = "Pulse Controller1 상태";
            grdItem[1, 19] = "Pulse Controller2 상태";
            grdItem[1, 20] = "Pulse Controller3 상태";
            grdItem[1, 21] = "Pulse Controller4 상태";
            grdItem[1, 22] = "Pulse Controller5 상태";
            grdItem[1, 23] = "Pulse Controller6 상태";
            grdItem[1, 24] = "Pulse Controller7 상태";
            grdItem[1, 25] = "Pulse Controller8 상태";
            grdItem[1, 26] = "Pulse Controller9 상태";
            #endregion

            grdItem.AllowEditing = false;



            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            clsFlexGrid.DataGridFormat(grdItem, 4, 4, "N2");
            clsFlexGrid.DataGridFormat(grdItem, 5, 5, "N1");
            //clsFlexGrid.DataGridFormat(grdItem, 6, 9, "N0");
            //clsFlexGrid.DataGridFormat(grdItem, 42, 51, "N1");// 차압 반부하진동

            grdItem.AutoSizeCols();

        }
        private void Set_Cols_GRD_DUST_INFO2(C1FlexGrid grdItem)
        {
            #region //컬럼 명 세팅
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "Pulse Controller10 상태";
            grdItem[1, 3] = "Pulse Controller11 상태";  
            grdItem[1, 4] = "Pulse Controller12 상태";
            grdItem[1, 5] = "Pulse Controller13 상태";
            grdItem[1, 6] = "Pulse Controller14 상태";
            grdItem[1, 7] = "Pulse Controller15 상태";
            grdItem[1, 8] = "Pulse Controller16 상태";
            grdItem[1, 9] = "Pulse Controller17 상태";
            grdItem[1, 10] ="Pulse Controller18 상태";
            grdItem[1, 11] ="Pulse Controller19 상태";
            grdItem[1, 12] ="Popet Damper#1 상태";
            grdItem[1, 13] ="Popet Damper#2 상태";
            grdItem[1, 14] ="Popet Damper#3 상태";
            grdItem[1, 15] ="차압#1";
            grdItem[1, 16] ="차압#2";
            grdItem[1, 17] ="차압#3";
            grdItem[1, 18] ="가동율";
            grdItem[1, 19] ="세팅값";
            grdItem[1, 20] ="부하측베어링온도";
            grdItem[1, 21] ="반부하측베어링온도";
            grdItem[1, 22] ="인버터속도 설정값(Hz)";
            grdItem[1, 23] ="부하진동";
            grdItem[1, 24] ="반부하진동";

           
            #endregion

            grdItem.AllowEditing = false;

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            //clsFlexGrid.DataGridFormat(grdItem, 4, 4, "N2");
            //clsFlexGrid.DataGridFormat(grdItem, 5, 5, "N1");
            //clsFlexGrid.DataGridFormat(grdItem, 6, 9, "N0");
            clsFlexGrid.DataGridFormat(grdItem, 15, grdItem.Cols.Count - 1, "N1");// 차압 반부하진동

            grdItem.AutoSizeCols();
        }



        private void Set_Cols_MPI_OPER_INFO(C1FlexGrid grdItem)
        {


            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "세척용Water Pump1상태";
            grdItem[1, 3] = "세척용Water Pump2상태";
            grdItem[1, 4] = "Filter용 Motor상태";
            grdItem[1, 5] = "세척용 Pump Heater상태";
            grdItem[1, 6] = "자화 Pump1상태";
            grdItem[1, 7] = "자화 Pump2상태";
            grdItem[1, 8] = "교반Pump Heater상태";
            grdItem[1, 9] = "Entry Motor FWD";
            grdItem[1, 10] = "Entry Motor REV";
            grdItem[1, 11] = "Inspection Motor FWD";
            grdItem[1, 12] = "Inspection Motor REV";
            grdItem[1, 13] = "유압 Motor1상태";
            grdItem[1, 14] = "유압 Motor2상태";
            grdItem[1, 15] = "유압 Fan상태";
            grdItem[1, 16] = "Dark Room Hood Fan";
            grdItem[1, 17] = "Yoke이송Motor1 FWD";
            grdItem[1, 18] = "Yoke이송Motor1 BWD";
            grdItem[1, 19] = "Yoke이송Motor2 FWD";
            grdItem[1, 20] = "Yoke이송Motor2 BWD";
            grdItem[1, 21] = "유압모터Heater상태";
            grdItem[1, 22] = "유압공급용Motor상태";
            grdItem[1, 23] = "Yoke 모터 #1 전류값";
            grdItem[1, 24] = "Yoke 모터 #2 전류값";
            grdItem[1, 25] = "Yoke 모터 #3 전류값";
            grdItem[1, 26] = "Yoke 모터 #4 전류값";
            grdItem[1, 27] = "코일 #1,2 전류값";
            grdItem.AllowEditing = false;


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            clsFlexGrid.DataGridFormat(grdItem, 23, grdItem.Cols.Count - 1, "N0");
            grdItem.AutoSizeCols();
        }

        private void Set_Cols_SHT_DUST_INFO(C1FlexGrid grdItem)
        {

            #region //컬럼 명 세팅
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "Bag전단정압계";
            grdItem[1, 3] = "흡입 온도";      
            grdItem[1, 4] = "Screw Conveyor 상태";
            grdItem[1, 5] = "Rotary Valve 상태";
            grdItem[1, 6] = "Vibrator 상태";
            grdItem[1, 7] = "Pulse Controller1 상태";
            grdItem[1, 8] =  "Pulse Controller2 상태";
            grdItem[1, 9] =  "Pulse Controller3 상태";
            grdItem[1, 10] = "Pulse Controller4 상태";
            grdItem[1, 11] = "Pulse Controller5 상태";
            grdItem[1, 12] = "Pulse Controller6 상태";
            grdItem[1, 13] = "Pulse Controller7 상태";
            grdItem[1, 14] = "Pulse Controller8 상태";
            grdItem[1, 15] = "Pulse Controller9 상태";
            grdItem[1, 16] = "Pulse Controller10 상태";
            grdItem[1, 17] = "Pulse Controller11 상태";
            grdItem[1, 18] = "Pulse Controller12 상태";
            grdItem[1, 19] = "Pulse Controller13 상태";
            grdItem[1, 20] = "Pulse Controller14 상태";
            grdItem[1, 21] = "Pulse Controller15 상태";
            grdItem[1, 22] = "Pulse Controller16 상태";
            grdItem[1, 23] = "차압";
            grdItem[1, 24] = "가동율";
            grdItem[1, 25] = "세팅값";
            grdItem[1, 26] = "부하측베어링온도";
            grdItem[1, 27] = "반부하측베어링온도";
            grdItem[1, 28] = "인버터속도 설정값(Hz)";
            grdItem[1, 29] = "부하진동";
            grdItem[1, 30] = "반부하진동";
            #endregion

            grdItem.AllowEditing = false;



            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            clsFlexGrid.DataGridFormat(grdItem, 2, 2, "N2");
            clsFlexGrid.DataGridFormat(grdItem, 3, 3, "N1");
            clsFlexGrid.DataGridFormat(grdItem, 23, grdItem.Cols.Count - 1, "N1"); //차압, 반부하진동

            grdItem.AutoSizeCols();
        }

        private void Set_Cols_SHT_OPER_INFO(C1FlexGrid grdItem)
        {
            

            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "롤러컨베어 속도";
            grdItem[1, 3] = "임펠라#1속도";
            grdItem[1, 4] = "임펠라#2속도";
            grdItem[1, 5] = "임펠라#3속도";
            grdItem[1, 6] = "임펠라#4속도";
            grdItem[1, 7] = "롤러컨베어 전류";
            grdItem[1, 8] = "임펠라#1전류";
            grdItem[1, 9] = "임펠라#2전류";
            grdItem[1, 10] = "임펠라#3전류";
            grdItem[1, 11] = "임펠라#4전류";
            grdItem[1, 12] = "로타리스크린전류";
            grdItem[1, 13] = "베켓엘리베이터전류";
            grdItem[1, 14] = "스크류컨베어1전류";
            grdItem[1, 15] = "스크류컨베어2전류";
            grdItem[1, 16] = "송풍기 전류";
            grdItem[1, 17] = "롤러컨베어 속도설정값";
            grdItem[1, 18] = "임펠라#1속도 설정값";
            grdItem[1, 19] = "임펠라#2속도 설정값";
            grdItem[1, 20] = "임펠라#3속도 설정값";
            grdItem[1, 21] = "임펠라#4속도 설정값";

            grdItem.AllowEditing = false;


            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            clsFlexGrid.DataGridFormat(grdItem, 2, 21, "N0");

            grdItem.AutoSizeCols();
        }



        private SearchOption GetselSearchOption()
        {
            var selSearchOption = SearchOption.SHT_OPER_INFO;
            if (cbRsltGp != null && (ComLib.DictionaryList)this.cbRsltGp.SelectedItem != null)
            {
                string strSelSearchOption = (cbGongJung.Items.Count > 0) ? ((ComLib.DictionaryList)this.cbRsltGp.SelectedItem).fnValue2 : string.Empty;
                switch (strSelSearchOption)
                {
                    case "0":
                        selSearchOption = SearchOption.SHT_OPER_INFO;
                        break;
                    case "1":
                        selSearchOption = SearchOption.SHT_DUST_INFO;
                        break;
                    case "2":
                        selSearchOption = SearchOption.MPI_OPER_INFO;
                        break;
                    case "3":
                        selSearchOption = SearchOption.GRD_OPER_INFO1;
                        break;
                    case "4":
                        selSearchOption = SearchOption.GRD_OPER_INFO2;
                        break;
                    case "5":
                        selSearchOption = SearchOption.GRD_DUST_INFO1;
                        break;
                    case "6":
                        selSearchOption = SearchOption.GRD_DUST_INFO2;
                        break;

                }

            }
            return selSearchOption;
        }

        private void DrawSelGrid()
        {
            C1FlexGrid grdItem = grdCheck as C1FlexGrid;
            

            grdItem.BeginInit();
            try
            {

                clsFlexGrid.FlexGridMainSystem(grdItem, SEL.GridRowsCount, SEL.GridColsCount, SEL.RowsFixed, SEL.RowsFrozen, SEL.ColsFixed, SEL.ColsFrozen);

                //컬럼 스타일 세팅
                FlexSelGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, SEL.TopRowsHeight, SEL.DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;

                MakeAllSelect();
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

            _al.Add(new HostedControl(grdCheck, lbSel, 1, 1));
        }

        private void AllRowSelectedEvent(object sender, EventArgs e)
        {
            if (allChecked)
            {
                for (int rowCnt = SEL.GridRowsCount; rowCnt < grdCheck.Rows.Count; rowCnt++)
                {
                    grdCheck.SetData(rowCnt, "CHECKER", "False");
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = SEL.GridRowsCount; rowCnt < grdCheck.Rows.Count; rowCnt++)
                {
                    grdCheck.SetData(rowCnt, "CHECKER", "True");
                }
                allChecked = true;

            }

            UpdateChart(checker_dt, selectedFlexgridData.chart);

        }

        private void InitControl()
        {
            //조회조건 판넬 색상 세팅
            clsStyle.Style.InitPanel(panel1);
            start_dt.Value = DateTime.Now.Date;
            end_dt.Value = DateTime.Now.Date;

            //초기 조회시에 초기 시간 설정값에 의해 HEAT NO리스트가 설정되고 선택된 HEAT NO 관련한  BLOOM ID 리스트가
            chkApplyDateYN.CheckState = CheckState.Checked;

            cd.SetCombo(cbGongJung, "ROUTING_CD", "", false);
            //cd.SetCombo(cbGongJung, GetGongJung(), "", false, 0, 1);
            //SetCombo(cbGongJung, GetGongJungSql(), "", false);

            SetResltsCb();

            SetHeatCb();

            SetBloomListCb();

        }

        private void SetHeatCb()
        {
            if (cbRsltGp != null && cbRsltGp != null && (ComLib.DictionaryList)this.cbRsltGp.SelectedItem != null && (ComLib.DictionaryList)this.cbRsltGp.SelectedItem != null)
            {
                string selRsltsTBGp = ((ComLib.DictionaryList)this.cbRsltGp.SelectedItem).fnValue;
                string selRsltsWorkDateGp = ((ComLib.DictionaryList)this.cbRsltGp.SelectedItem).fnValue1;

                if (!string.IsNullOrEmpty(selRsltsTBGp) && !string.IsNullOrEmpty(selRsltsWorkDateGp))
                {
                    cd.SetCombo(cbHeatNo, GetHeatNoList(selRsltsTBGp, selRsltsWorkDateGp, start_dt.Value, end_dt.Value), "", false, 1, 0);
                }
            }

            
        }

        private void SetBloomListCb()
        {
            if (chkApplyDateYN.CheckState == CheckState.Checked)
            {
                if (cbHeatNo != null && cbRsltGp != null && (ComLib.DictionaryList)this.cbHeatNo.SelectedItem != null && (ComLib.DictionaryList)this.cbRsltGp.SelectedItem != null)
                {

                    string selHeatGp = (cbHeatNo.Items.Count > 0) ? ((ComLib.DictionaryList)this.cbHeatNo.SelectedItem).fnValue : string.Empty;
                    string selRsltsTBGp = (cbRsltGp.Items.Count > 0) ? ((ComLib.DictionaryList)this.cbRsltGp.SelectedItem).fnValue : string.Empty;

                    if (!string.IsNullOrEmpty(selHeatGp) && !string.IsNullOrEmpty(selRsltsTBGp))
                    {
                        cd.SetCombo(cbBloomId, GetBloomId(selHeatGp, selRsltsTBGp), "", false, 1, 0, 2, 3, 4);
                    }
                }

            }
            else
            {
                if (txt_Heat != null && cbRsltGp != null && txt_Heat.Text.Length > 0 && (ComLib.DictionaryList)this.cbRsltGp.SelectedItem != null)
                {

                    string selHeatGp = txt_Heat.Text;
                    string selRsltsTBGp = (cbRsltGp.Items.Count > 0) ? ((ComLib.DictionaryList)this.cbRsltGp.SelectedItem).fnValue : string.Empty;

                    if (!string.IsNullOrEmpty(selHeatGp) && !string.IsNullOrEmpty(selRsltsTBGp))
                    {
                        cd.SetCombo(cbBloomId, GetBloomId(selHeatGp, selRsltsTBGp), "", false, 1, 0, 2, 3, 4);
                    }
                }
            }
        }



        private string GetHeatNoList(string _selRsltsGp, string _selRsltsWorkDateGp, DateTime _startdt, DateTime _enddt)
        {
            string sql = string.Empty;

            DateTime startDt = _startdt.AddHours(6);//2018-08-28 06:00:00
            DateTime endDt = _enddt.AddDays(1).AddHours(6);         //2018-08-29 06:00:00

            string _start_dt = vf.Format(startDt, "yyyyMMddHHmmss");
            string _end_dt = vf.Format(endDt, "yyyyMMddHHmmss");


            sql = string.Format(@"  SELECT DISTINCT HEAT_NO AS CD_ID
                                                  , HEAT_NO AS CD_NM
                                               FROM {0}
                                              WHERE WORK_DATE  >= '{1}'
                                                AND WORK_DATE  < '{2}' 
                                           ORDER BY HEAT_NO", _selRsltsGp, _start_dt, _end_dt);

            return sql;
        }

        private string GetBloomId(string _selHeatGp, string _selRsltsGp )
        {
            string sql = string.Empty;

            sql = string.Format(@"  SELECT DISTINCT HEAT_NO+'  '+CONVERT(VARCHAR(10),HEAT_SEQ)+'  '+ CONVERT(VARCHAR(10),REWORK_SNO+1) AS CD_ID
                                                  , HEAT_NO+'  '+CONVERT(VARCHAR(10),HEAT_SEQ)+'  '+ CONVERT(VARCHAR(10),REWORK_SNO+1) AS CD_NM
                                                  , HEAT_NO
                                                  , HEAT_SEQ
                                                  , REWORK_SNO
                                               FROM {0}
                                              WHERE HEAT_NO LIKE '{1}%'
                                            ORDER BY  HEAT_NO,HEAT_SEQ,REWORK_SNO", _selRsltsGp, _selHeatGp);

            return sql;
        }

        private void SetResltsCb()
        {
            if (cbGongJung != null && (ComLib.DictionaryList)this.cbGongJung.SelectedItem != null)
            {
                string selGongJung = (cbGongJung.Items.Count > 0) ? ((ComLib.DictionaryList)this.cbGongJung.SelectedItem).fnValue : string.Empty;

                if (!string.IsNullOrEmpty(selGongJung))
                {
                    cd.SetCombo(cbRsltGp, GetcbRsltGp(selGongJung), "", false, 1, 0, 2, 3);
                }
            }
        }

        private string GetcbRsltGp(string _gongJungGp)
        {
            string sql = string.Empty;

            sql = string.Format(@" SELECT CD_NM
                                        , COLUMNC
                                        , COLUMNB
                                        , COLUMND
                                    FROM TB_CM_COM_CD
                                   WHERE CATEGORY = 'RESULTS_CD'
                                     AND CD_ID LIKE '{0}%'
                                     AND USE_YN = 'Y'
                                  ORDER BY SORT_SEQ", _gongJungGp);

            return sql;
        }

        

        private void FlexSelGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 120;
            grdItem.Cols[2].Width = 120;

            //컬럼 명 세팅
            grdItem[1, 0] =  "선택";
            grdItem[1, 1] =  "항목";
            grdItem[1, 2] =  "항목구분";

            //grdItem.AllowEditing = false;
            grdItem.Cols[0].AllowEditing = true;
            grdItem.Cols[1].AllowEditing = true;
            grdItem.Cols[2].Visible = false;

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우

            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);


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

                    if (!IsSelectedBloomId())
                    {
                        MessageBox.Show("조회할 BLOOM ID 를 선택해주세요.");
                        return;
                    }

                    //InitGridData(true);

                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

                    //DrawDataGrid();

                    FlexDataGridSql(selectedFlexgridData);

                    SetDataBinding();  // 조회 버튼을 통한 데이터입력

                    SetChartData();

                    //break;
                    SetCheckGrd();

                    UpdateChart(checker_dt, selectedFlexgridData.chart);

                    lastSelectedFlexgrid = selectedFlexgridData;

                    ShowActivateGrid();

                    ShowActivateChart();

                    break;


                case "btnExcel":
                    SaveExcel();
                    break;

            }
        }

        private void ShowActivateChart()
        {
            // 모든 그리드를 숨기고 해당 그리드만 docking 시킴
            selectedFlexgridData.chart.Dock = DockStyle.Fill;
            selectedFlexgridData.chart.BringToFront();
        }

        private void SetCheckGrd()
        {
            InitCheckerGrid(chart_dt, selectedFlexgridData.chart, selectedFlexgridData.grd);

            SetDataBinding_CheckGrd();


            #region 임시 스타일을 저장
            var tempStyle = new CellStyle[50];
            string[] tempStyle_name = new string[50];

            for (int itemCnt = 0; itemCnt < 50; itemCnt++)
            {
                tempStyle_name[itemCnt] = itemCnt.ToString();
                tempStyle[itemCnt] = grdCheck.Styles.Add(tempStyle_name[itemCnt]);
            }
            #endregion

            // series color 를 읽어서 grid forcolor를 정의함
            var tempStyle11 = grdCheck.Styles.Add("TempStyle");
            var crCellRange = new CellRange();
            //bool visiable;
            for (int row = 0; row < checker_dt.Rows.Count; row++)
            {

                //visiable = (checker_dt.Rows[row]["CHECKER"].ToString() == "True");

                foreach (var series in selectedFlexgridData.chart.Series)
                {
                    //마커 크기
                    ((PointAttributes)series).MarkerShape = MarkerShape.None;
                    //선 굵기
                    ((PointAttributes)series).Line.Width = 2;
                    ((PointAttributes)series).Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;

                    if (((PointAttributes)series).Text == grdCheck.GetData(row + SEL.GridRowsCount, "ITEM").ToString())
                    {
                        tempStyle[row].ForeColor = ((PointAttributes)series).Color;

                        crCellRange = grdCheck.GetCellRange(row + SEL.GridRowsCount, grdCheck.Cols["ITEM"].Index);
                        crCellRange.Style = grdCheck.Styles[row.ToString()];
                    }
                }

            }

            //chart1.RecalculateScale();// .RecalculateAxesScale();

            

            //RealizeMaxMin();
        }

        private bool IsSelectedBloomId()
        {
            bool isSelectedBloomId = false;

            if ((ComLib.DictionaryList)this.cbBloomId.SelectedItem != null && this.cbBloomId.Items.Count > 0 && ((ComLib.DictionaryList)this.cbBloomId.SelectedItem).fnText.Length > 0)
            {
                isSelectedBloomId = true;
            }


            return isSelectedBloomId;
        }

        private void ShowActivateGrid()
        {
            // 모든 그리드를 숨기고 해당 그리드만 docking 시킴
            selectedFlexgridData.grd.Dock = DockStyle.Fill;
            selectedFlexgridData.grd.BringToFront();
        }

        private void SetupDataGrid()
        {
            
        }

        private void SaveExcel()
        {
            grdCheck.Tag = "작업일보1";
            //grdData.Tag = "작업일보2";
            vf.SaveExcel(titleNM, selectedFlexgridData.grd);
            //SaveExcel(string fileName, string sheetName, FileFlags flags, PrinterSettings ps);
            //grdMain.SaveExcel("", FileFlags.IncludeMergedRanges);

        }

        private void SetDataBinding()
        {
            SetDataBinding_Grd_byinitData();

            SetDataBinding_DataGrd();

            //SetDataBinding_SelGrd();
        }

        private void SetDataBinding_DataGrd()
        {
            try
            {
                olddtData = null;
                moddtData = null;

                olddtData = cd.FindDataTable(selectedFlexgridData.Sql);
                moddtData = olddtData.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(selectedFlexgridData.grd, moddtData);

                selectedFlexgridData.grd.AutoSizeCols();

                //SetChartData();

                this.Cursor = Cursors.Default;

                //SetGridRowSelect(grdMain, -1);

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} DATA: {moddtData.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void SetChartData()
        {
            chart_dt = ReomveCols(moddtData);

            SetChart(chart_dt, selectedFlexgridData.grd, selectedFlexgridData.chart);
        }

        private DataTable ReomveCols(DataTable _moddtData)
        {
            DataTable dt = _moddtData.Copy();

            foreach (var removeColNM in selectedFlexgridData.RemoveColList)
            {
                dt.Columns.Remove(removeColNM);
            }

            return dt;
        }

        private void SetChart(DataTable Chartdt, C1FlexGrid grd, Chart chart)
        {
            //차트 초기화
            InitChart(chart);
            //차트 데이터 바인딩
            //chart.Data.Series = Chartdt.Columns.Count;
            //chart.DataSource = Chartdt;
            chart.Data.Series = Chartdt.Columns.Count - 1;// L_NUM 제외
            chart.Data.Points = Chartdt.Rows.Count;

            for (int series = 0; series < chart.Series.Count; series++)
            {
                chart.Series[series].Text = grd.GetData(1, series + 2).ToString();  //Chartdt.Columns[series +1].ColumnName;
                for (int row = 0; row < Chartdt.Rows.Count; row++)
                {
                    chart.Data[series, row] = Convert.ToDouble(Chartdt.Rows[row][series+1].ToString()); ;

                }
            }

        }

        private void SetDataBinding_CheckGrd()
        {
            try
            {

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawCheckGrid(grdCheck, checker_dt);

                grdCheck.AutoSizeCols();

                this.Cursor = Cursors.Default;

                //SetGridRowSelect(grdMain, -1);

                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} DATA: {moddtData.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void UpdateChart(DataTable _checker_dt, Chart _chart1)
        {

            for (int row = 0; row < _checker_dt.Rows.Count; row++)
            {
                //var line = from series in chart1.Series.Contains()

                foreach (var series in _chart1.Series)
                {
                    if (((PointAttributes)series).Text == _checker_dt.Rows[row]["ITEM"].ToString())// .Columns["ITEM"].ToString())
                    {
                        ((SeriesAttributes)series).Visible = (_checker_dt.Rows[row]["CHECKER"].ToString() == "True");
                        //continue;
                    }

                }
            }
            SetDataTableMinMax(checker_dt, selectedFlexgridData.grd, selectedFlexgridData.chart);
        }

        private void SetDataTableMinMax(DataTable _checker_dt, C1FlexGrid _selectedGrd , Chart chart)
        {
            //선택된 colsName 리스트를 가지고
            //선택된 그리드에서 해당 colsName의 min, max를 계산해서 
            //최종 min,max를 적용한다.
            List<string> selecteditem = new List<string>();
            foreach (DataRow row in _checker_dt.Rows)
            {
                if (row["CHECKER"].ToString() == "True")
                {
                    selecteditem.Add(row["ITEM_GUBUN"].ToString());
                }

            }

            double minValue = vf.CDbl(int.MaxValue);
            double maxValue = vf.CDbl(int.MinValue);

            DataTable dt = (DataTable)_selectedGrd.DataSource;
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            if (selecteditem.Count == 0)
            {
                chart.AxisY.Min = -1.0;
                chart.AxisY.Max = 1.0;
                return;
            }

            foreach (var item in selecteditem)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    double value = vf.CDbl(dr.Field<object>(item));
                    minValue = Math.Min(minValue, value);
                    maxValue = Math.Max(maxValue, value);

                }
            }

            double meanValue = Math.Abs((maxValue - minValue)) / 2;
            double marginValue = meanValue / 5;

            if (minValue.Equals(maxValue))
            {

                if (minValue.Equals(0.0))
                {
                    chart.AxisY.Min = -1.0;
                    chart.AxisY.Max = 1.0;
                    return;
                }

                chart.AxisY.Min = minValue * 0.8;
                chart.AxisY.Max = maxValue * 1.2;
                return;
            }


            chart.AxisY.Min = minValue - marginValue;
            chart.AxisY.Max = maxValue + marginValue;

        }

        private void SetUpChartSeriesText(DataTable _chartdt, Chart _chart, C1FlexGrid _grd)
        {
            foreach (DataColumn col in _chartdt.Columns)
            {

                if (_grd.Cols[col.ColumnName].Width > 0) // 화면에 보이는 항목
                {
                    // 작업시각은 제외
                    // dt의 column 21
                    // chart.series.count 20 + 1(L_NO)
                    if (col.ColumnName == "L_NO" || col.ColumnName == "WORK_DDTT") // L_NO + WORK_DDTT
                    {
                        continue;
                    }

                    // grd.cols[].index 1 이면 chart.series  순번은 0   여기서 2는 L_NO와 WORK_DDTT둘을 뺀것.
                    _chart.Series[_grd.Cols[col.ColumnName].Index - 2].Text = _grd.Cols[col.ColumnName].Caption;

                }
            }
        }

        private void InitCheckerGrid(DataTable _chartdt, Chart _chart, C1FlexGrid _grd)
        {
            checker_dt = new DataTable();
            checker_dt.Columns.Add("CHECKER", typeof(string));
            checker_dt.Columns.Add("ITEM", typeof(string));
            checker_dt.Columns.Add("ITEM_GUBUN", typeof(string));
            string colsNM = string.Empty;
            string chartdtColNM = "";
            // L_NUM 은 제외
            for (int col = 1; col < _chartdt.Columns.Count; col++)
            {
                chartdtColNM = _chartdt.Columns[col].ColumnName;
                colsNM = _grd.GetData(1, _grd.Cols[chartdtColNM].Index).ToString();
                checker_dt.Rows.Add("True", colsNM, chartdtColNM);

            }

        }

        private void ChartBindData(DataTable Chartdt, Chart chart)
        {
            
        }

        private void InitChart(Chart _chart)
        {
            _chart.Reset();
            _chart.LegendBox.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            _chart.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));


            _chart.Dock = DockStyle.Fill;
            _chart.ToolBar.Visible = false;
            _chart.LegendBox.Visible = false;
            _chart.ContextMenus = false; //팝업메뉴 죽임
            _chart.AxisY.AutoScale = true;
            _chart.AxisY.ForceZero = false; // 0 을 최소값으로 두지 않는 옵션.
            //chart1.AxisY.DataFormat.Decimals = 1;
        }

        private void ReomveCols()
        {
            chart_dt = moddtData.Copy();
            foreach (var removeColNM in selectedFlexgridData.RemoveColList)
            {
                chart_dt.Columns.Remove(removeColNM);
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
                clsFlexGrid.FlexGridRow(grdItem, selectedFlexgridData.TopRowsHeight, selectedFlexgridData.DataRowsHeight);

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

        private void DrawCheckGrid(C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
                clsFlexGridColumns FlexGridColumns = new clsFlexGridColumns();
                FlexGridColumns.Add("CHECKER", FlexGridCellTypeEnum.CheckBox, "true");

                //그리드 스크롤바 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;

                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, FlexGridColumns, true);

                //그리드 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, SEL.TopRowsHeight, SEL.DataRowsHeight);

                //마지막행 사이즈조절, 로우공백흰색
                grdItem.ExtendLastCol = true;
                grdItem.Styles.EmptyArea.BackColor = Color.White;

                grdItem.SelectionMode = SelectionModeEnum.Row;
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

        private void SetGridRowSelect(C1FlexGrid grdMain, int v)
        {
            
        }

        private void SetDataBinding_Grd_byinitData()
        {
            InitGridData();
        }

        private void InitGridData()
        {
            clsFlexGrid.grdDataClear(grdCheck, SEL.GridRowsCount);
            clsFlexGrid.grdDataClear(selectedFlexgridData.grd, selectedFlexgridData.GridRowsCount);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pbx_Search_Click(object sender, EventArgs e)
        {
            ck.StrKey1 = txt_Heat.Text;
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

        private void cbApplyDateYN_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkApplyDateYN.CheckState == CheckState.Checked)
            {
                SetPeriodHeatSearchMode();
            }
            else
            {
                SetHeatSearchMode();
            }
        }

        private void SetPeriodHeatSearchMode()
        {
            cbHeatNo.Visible = true;
            cbBloomId.Enabled = true;

            txt_Heat.Visible = false;
            pbx_Search.Visible = false;

            start_dt.Enabled = true;
            end_dt.Enabled = true;
        }

        private void SetHeatSearchMode()
        {
            cbHeatNo.Visible = false;
            cbBloomId.Enabled = false;

            txt_Heat.Visible = true;
            pbx_Search.Visible = true;

            start_dt.Enabled = false;
            end_dt.Enabled = false;
        }
        DateTime last_startdt;
        DateTime last_enddt;

        private void DateChanged(object sender, EventArgs e)
        {

            DateTimePicker picker = sender as DateTimePicker;

            if (DateTime.Compare(start_dt.Value, end_dt.Value) > 0)
            {
                if (picker.Name == "start_dt")
                {
                    start_dt.Value = end_dt.Value;
                }else
                {
                    end_dt.Value = start_dt.Value;
                }
                
            }

            ////조회시간 시간이하로 제한
            //int limtedDays = 3;
            //if (IsOverLimtedDay(start_dt.Value, end_dt.Value, limtedDays))
            //{
            //    MessageBox.Show($"{limtedDays.ToString()}일 이하로 조회하시기 바랍니다.");
            //    if(picker.Name == "start_dt")
            //    {
            //        start_dt.Value = last_startdt;
            //    }else
            //    {
            //        end_dt.Value = last_enddt;
            //    }
            //    return;
            //}

            ReloadHeat();

            //last_startdt = start_dt.Value;
            //last_enddt = end_dt.Value;

        }

        private bool IsOverLimtedDay(DateTime start, DateTime end, int limtedDays)
        {
            bool isOver = false;
            if (GetDurationDay(start, end) > limtedDays)
            {
                isOver = true;
            }
            return isOver;
        }

        private int GetDurationDay(DateTime start, DateTime end)
        {
            int timespan = 0;
            TimeSpan ts = end -start;
            timespan = Convert.ToInt32(Math.Ceiling(ts.TotalDays));

            return timespan;
        }

        private void ReloadHeat()
        {
            SetHeatCb();

            SetBloomListCb();
        }

        private void cbGongJung_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetResltsCb();
        }

        private void cbHeatNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangedHeatNo();

        }

        private void ChangedHeatNo()
        {
            SetBloomListCb();
        }

        private void cbRsltGp_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ReSetFlexgridDataSelect();
        }

        private void ReSetFlexgridDataSelect()
        {
            selectedFlexgridData = flexgridList.Find(x => x.SearchOption == GetselSearchOption());
        }

        private void grdCheck_CellChecked(object sender, RowColEventArgs e)
        {
            UpdateChart(checker_dt, selectedFlexgridData.chart);

        }

        private void chart1_MouseClick(object sender, HitTestEventArgs e)
        {
            Int32 nXPos;
            //Int32 nYPos;

            if (selectedFlexgridData.chart.Series.Count == 0)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                //chart_line_point = this.Size.Width - ChartPanel.Size.Width + 1;
                nXPos = decimal.ToInt32(decimal.Round(decimal.Parse(selectedFlexgridData.chart.Series[0].AxisX.PixelToValue(e.X).ToString()), 0));
                Console.WriteLine(nXPos);
                //MessageBox.Show(nXPos.ToString());
                if (nXPos > 0 && nXPos + selectedFlexgridData.GridRowsCount <= selectedFlexgridData.grd.Rows.Count )
                {
                    selectedFlexgridData.grd.Row = nXPos + selectedFlexgridData.GridRowsCount -1 ;
                }

            }
        }

        private void start_dt_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void start_dt_Validated(object sender, EventArgs e)
        {
            
        }

        private void txt_Heat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (!string.IsNullOrEmpty(txt_Heat.Text))
                {
                    cbBloomId.Enabled = true;
                    ChangedHeatNo();
                }
            }
        }
    }
}
