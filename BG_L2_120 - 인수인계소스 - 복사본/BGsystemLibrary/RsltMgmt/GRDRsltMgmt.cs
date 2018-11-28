using C1.Win.C1Command;
using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1Input;
using BGsystemLibrary.Common;

namespace BGsystemLibrary.MatMgmt
{
    public partial class GRDRsltMgmt : Form
    {
        #region 공통 생성자
        //공통변수
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        //데이터테이블
        DataTable olddtMain1;
        DataTable moddtMain1;
        DataTable olddtMain2;
        DataTable moddtMain2;

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private int main1_GridRowsCount = 2;
        private int main1_GridColsCount = 38;
        private int main1_RowsFixed = 2;
        private int main1_RowsFrozen = 0;
        private int main1_ColsFixed = 0;
        private int main1_ColsFrozen = 5;

        private int main2_GridRowsCount = 2;
        private int main2_GridColsCount = 44;
        private int main2_RowsFixed = 2;
        private int main2_RowsFrozen = 0;
        private int main2_ColsFixed = 0;
        private int main2_ColsFrozen = 2;



        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;
        private int GrandTotalHeight = 35;

        // 프로그램 명 변수
        private static string ownerNM = "";
        private static string titleNM = "";

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한

        #endregion 공통 생성자

        #region 공통메소드


        
        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":

                    if (this.scrAuthInq != "Y")
                    {
                        MessageBox.Show("조회 권한이 없습니다");
                        return;
                    }

                    InitHeatNo();

                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

                    SetDataBinding();

                    break;



                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

        private void InitHeatNo()
        {
            tb_BloomID.ReadOnly = false;
            tb_StartDT.ReadOnly = false;
            tb_EndDT.ReadOnly = false;

            tb_BloomID.Text = string.Empty;
            tb_StartDT.Text = string.Empty;
            tb_EndDT.Text = string.Empty;

            tb_BloomID.ReadOnly = true;
            tb_StartDT.ReadOnly = true;
            tb_EndDT.ReadOnly = true;
        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM + "_" + TabOpt.SelectedTab.Text, SelectedGrd_InTab());
        }

        private C1FlexGrid SelectedGrd_InTab()
        {
            C1FlexGrid selectedGrd = null;

            C1DockingTabPage selectedTab = TabOpt.SelectedTab;

            string grd_Nm = "grdMain1";
            if (selectedTab.Name == "TabP1")
            {
                grd_Nm = "grdMain1";
            }
            else if (selectedTab.Name == "TabP2")
            {
                grd_Nm = "grdMain2";
            }

            return selectedGrd = (C1FlexGrid)selectedTab.Controls.Find(grd_Nm, true)[0];
        }

        private void SelectRow()
        {
            if (clsFlexGrid.CanRowSelectGrid(grdMain1, main1_GridRowsCount))
            {
                //grdMain1.RowSel = main1_GridRowsCount + 1;
                //grdMain1.Rows[main1_GridRowsCount].Selected = true;

                SetupSelectedRow();
            }

        }

        private void DrawGrid(C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();
            try
            {
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

        private void InitControl()
        {
            TabOpt.SelectedIndex = 0;
            start_dt.Value = DateTime.Now.Date;
            end_dt.Value = DateTime.Now.Date;
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

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);

                //컬럼 스타일 세팅
                FlexMain1GridCol(grdItem);
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

                clsFlexGrid.FlexGridMainSystem(grdItem, _GridRowsCount, _GridColsCount, _RowsFixed, _RowsFrozen, _ColsFixed, _ColsFrozen);
                //컬럼 스타일 세팅
                FlexMain2GridCol(grdItem);
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

        private void SetupSelectedRow()
        {
            if (grdMain1.RowSel >= main1_GridRowsCount)
            {
                tb_BloomID.ReadOnly = false;
                tb_BloomID.Text = grdMain1.GetData(grdMain1.RowSel, "BLOOM_ID").ToString();
                tb_BloomID.ReadOnly = true;

                tb_StartDT.ReadOnly = false;
                tb_StartDT.Text = grdMain1.GetData(grdMain1.RowSel, "START_DDTT").ToString();
                tb_StartDT.ReadOnly = true;

                tb_EndDT.ReadOnly = false;
                tb_EndDT.Text = grdMain1.GetData(grdMain1.RowSel, "END_DDTT").ToString();
                tb_EndDT.ReadOnly = true;
            }
        }

        private void grdMain1_SelChange(object sender, EventArgs e)
        {
            SetupSelectedRow();
        }

        private void TabOpt_SelectedTabChanged(object sender, EventArgs e)
        {
            C1DockingTab Tap = sender as C1DockingTab;

            if (Tap.SelectedTab.Name == TabP2.Name)
            {
                SetSearchMode(Search.ProdInfo);

                clsFlexGrid.grdDataClearForBind(grdMain2);

                SetDataBindingMain2();
            }
            else if (Tap.SelectedTab.Name == TabP1.Name)
            {
                SetSearchMode(Search.Reault);

                Button_Click(btnDisplay, null);
            }


        }

        private void SetSearchMode(Search prodInfo)
        {
            switch (prodInfo)
            {
                case Search.Reault:
                    start_dt.Enabled = true;
                    end_dt.Enabled = true;
                    txt_Heat.ReadOnly = false;
                    btnDisplay.Enabled = true;
                    break;
                case Search.ProdInfo:
                    start_dt.Enabled = false;
                    end_dt.Enabled = false;
                    SetReadOnlyTB(txt_Heat, txt_Heat.Text);
                    txt_Heat.ReadOnly = true;
                    btnDisplay.Enabled = false;
                    break;
            }
        }

        private void SetReadOnlyTB(C1TextBox txt_Heat, string text)
        {
            if (!txt_Heat.ReadOnly)
            {
                string temp = text;
                txt_Heat.Text = temp;
            }
        }

        public enum Search
        {
            Reault,   // 실적조회
            ProdInfo  // 조업정보조회
        }

        #endregion

        #region 화면별 설정
        private void GRDRsltMgmt_Load(object sender, EventArgs e)
        {
            //그리드 초기화
            DrawMain1Grid(grdMain1);
            DrawMain2Grid(grdMain2);
            //초기화
            InitControl();
            //조회버튼 클릭
            Button_Click(btnDisplay, null);
        }

        public GRDRsltMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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
        private void SetDataBinding()
        {
            clsFlexGrid.grdDataClearForBind(grdMain1);

            try
            {
                DateTime startDt = start_dt.Value.AddHours(6);//2018-08-28 06:00:00
                DateTime endDt = end_dt.Value.AddDays(1).AddHours(6);         //2018-08-29 06:00:00
                string _start_dt = vf.Format(startDt, "yyyyMMddHHmmss");
                string _end_dt = vf.Format(endDt, "yyyyMMddHHmmss");

                string sql = "";
                sql += string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY GRD_START_DDTT DESC)) AS L_NUM                            
                                    	     ,CONVERT(VARCHAR,HEAT_NO) +' '+ CONVERT(VARCHAR,HEAT_SEQ)  AS BLOOM_ID  
                                          ,REWORK_SNO +1                                                                                     
                                          ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = STEEL) AS STEEL_NM
                                          ,ITEM                                                                                             
                                          ,(SELECT CD_NM         FROM TB_CM_COM_CD           WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = ITEM + ITEM_SIZE) AS ITEM_SIZE_NM
                                          ,STEEL_TYPE                                                                                       
                                          ,CONVERT(DATE, MFG_DATE)   AS MFG_DATE                                                            
                                          ,CONVERT(DATE, WORK_DATE)   AS WORK_DATE                                                          
                                          ,CONVERT(CHAR(19), GRD_START_DDTT, 120)  START_DDTT                                               
                                          ,CONVERT(CHAR(19), GRD_END_DDTT, 120)    END_DDTT                                                 
                                          ,ISNULL(SURFACE_STAT         , 0 ) AS  SURFACE_STAT                                                                                   
                                          ,ISNULL(QR_SCAN_INFO         , 0 ) AS  QR_SCAN_INFO                                                                                   
                                          ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GRD_DRV_MODE' AND CD_ID = ISNULL(GRD_DRV_MODE, '0' )) AS  GRD_DRV_MODE          --   그라인딩 운전 모드                                                                                   
                                          ,HEIGHT_MV                                                                                      
                                          ,WIDTH_MV                                                                                       
                                          ,LENGTH_MV                                                                                      
                                          ,FACE_PASS_CNT                                                                                  
                                          ,CORN_PASS_CNT                                                                                  
                                          ,RBGW_SPEED_SETV                                                                                
                                          ,RBGW_ADV_SETV                                                                                  
                                          ,RBGW_DIA_MV                                                                                    
                                          ,RBGW_ENGY_MV                                                                                   
                                          ,RBGW_VIBR_MV                                                                                   
                                          ,ROTANGLE_SETV                                                                                  
                                          ,ROTANGLE_MV                                                                                    
                                          ,GRD_PRES_SURFACE_SETV                                                                          
                                          ,GRD_PRES_ENDP_SETV                                                                             
                                          ,GRD_PRES_CORN_SETV                                                                             
                                          ,GRD_PRES_MV                                                                                    
                                          ,TRUCKSPEED_SETV                                                                                
                                          ,TRUCKSPEED_MV                                                                                  
                                          ,WORK_SETV_Top                                                                                  
                                          ,WORK_SETV_Right                                                                                
                                          ,WORK_SETV_Bottom                                                                               
                                          ,WORK_SETV_Left                                                                                 
                                          ,REWORK_YN                                                                                        
                                          ,SEND_STAT                                                                                        
                                      FROM TB_GRD_WR                                                                                         
                                     WHERE FORMAT(GRD_START_DDTT,'yyyyMMddHHmmss')  >= '{0}'
                                       AND FORMAT(GRD_START_DDTT,'yyyyMMddHHmmss')  < '{1}'
                                       AND HEAT_NO LIKE '{2}%'
                                  ORDER BY GRD_START_DDTT DESC
                                                  ", _start_dt, _end_dt, txt_Heat.Text);

                olddtMain1 = cd.FindDataTable(sql);
                moddtMain1 = olddtMain1.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain1, moddtMain1);

                if (moddtMain1.Rows.Count > 1 && clsFlexGrid.IsSubTotal)
                {
                    UpdateTotals();
                }
                else
                {
                    main1_GridRowsCount = 2;
                }
                grdMain1.AutoSizeCols();

                this.Cursor = Cursors.Default;

                SelectRow();

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} ({TabP1.Text}): {moddtMain1.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void UpdateTotals()
        {

            // Show OutlineBar on column 0.
            grdMain1.Tree.Column = 1;
            grdMain1.Tree.Style = TreeStyleFlags.Simple;
            //grdMain1.Rows[3].Style = grdMain1.Styles[CellStyleEnum.GrandTotal];// = grdMain1.Styles[CellStyleEnum.GrandTotal];

            //CellStyle cs;
            //cs = grdMain1.Styles[CellStyleEnum.GrandTotal];
            //cs.BackColor = Color.Black;
            //cs.ForeColor = Color.White;
            //cs.Font = new Font("돋움체", clsFlexGrid.CommonFontSize, FontStyle.Bold);

            // clear existing totals
            grdMain1.Subtotal(AggregateEnum.Clear);

            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["HEIGHT_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["WIDTH_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["LENGTH_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["FACE_PASS_CNT"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["CORN_PASS_CNT"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["RBGW_SPEED_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["RBGW_ADV_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["RBGW_DIA_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["RBGW_ENGY_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["RBGW_VIBR_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["ROTANGLE_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["ROTANGLE_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["GRD_PRES_SURFACE_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["GRD_PRES_ENDP_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["GRD_PRES_CORN_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["GRD_PRES_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["TRUCKSPEED_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["TRUCKSPEED_MV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["WORK_SETV_Top"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["WORK_SETV_Right"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["WORK_SETV_Bottom"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["WORK_SETV_Left"].Index, "평균");


            //grdMain1.Rows.Fixed = 1;
            grdMain1.Rows.Frozen = 1;
            main1_GridRowsCount = 3;

            //grdMain1.Cols[grdMain1.Cols["ROLLER_CONVR_SPEED"].Index].Format = "N0";
            //grdMain1.Cols[grdMain1.Cols["IMPELLER_NO1_SPEED"].Index].Format = "N0";

            grdMain1.Rows[2].Height = GrandTotalHeight;

            //grdMain1.Rows[3].Style.BackColor = Color.Blue;
            //grdMain1.Rows[3].Style.ForeColor = Color.White;

            clsFlexGrid.GridCellRangeStyleColor(grdMain1, 2, 0, 2, grdMain1.Cols.Count - 1, Color.Blue, Color.White);



            //grdMain1.Invalidate();
        }

        private void SetDataBindingMain2()
        {
            clsFlexGrid.grdDataClearForBind(grdMain2);

            try
            {
                string sql = "";
                sql += string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER(ORDER BY WORK_DDTT DESC)) AS L_NUM    
                                               ,CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                       
                                               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GRD_DRV_MODE' AND CD_ID = GRD_DRV_MODE) AS  GRD_DRV_MODE_NM          --   그라인딩 운전 모드                                           
                                               ,HEIGHT_SETV                   --   높이 설정값                                        
                                               ,WIDTH_SETV                    --   폭 설정값                                        
                                               ,HEIGHT_MV                     --   높이 측정값                                        
                                               ,WIDTH_MV                      --   폭 측정값                                        
                                               ,LENGTH_MV                     --   길이 측정값                                        
                                               ,FACE_PASS_CNT                 --   면 Pass수                                        
                                               ,CORN_PASS_CNT                 --   코너 Pass수                                        
                                               ,WORK_SEQ                      --   작업 순서                                        
                                               ,WORK_Track_NO                 --   작업 Track NO                                        
                                               ,RBGW_SPEED_SETV               --   연마지석 속도 설정값                                        
                                               ,RBGW_ADV_SETV                 --   연마지석 전진 설정값                                        
                                               ,RBGW_DIA_MV                   --   연마지석 직경 측정값                                        
                                               ,RBGW_ENGY_MV                  --   연마지석 에너지 측정값                                        
                                               ,RBGW_VIBR_MV                  --   연마지석 진동 측정값                                        
                                               ,ROTANGLE_SETV                 --   회전각도 설정값                                        
                                               ,ROTANGLE_MV                   --   회전각도 측정값                                        
                                               ,GRD_PRES_SURFACE_SETV         --   그라인딩 압력 표면 설정값                                        
                                               ,GRD_PRES_ENDP_SETV            --   그라인딩 압력 끝단 설정값                                        
                                               ,GRD_PRES_CORN_SETV            --   그라인딩 압력 코너 설정값
											   ,SET_GRD_Force                 --   그라인딩 압력 Force                                       
                                               ,GRD_PRES_MV                   --   그라인딩 모터 전력값,                                        
                                               ,TRUCKSPEED_SETV               --   대차속도 설정값                                                
                                               ,TRUCKSPEED_MV                 --   대차속도 측정값                                            
                                               ,GRD_LOC                       --   그라인딩 위치                                         
                                               ,COOL_DOWN_HR_SETV             --   쿨다운 시간 설정값                                    
                                               ,COOL_DOWN_SPEED_SETV          --   쿨다운 속도 설정값 ?                                
                                               ,BELT_SLIP_WARN_SETV           --   벨트 슬립 경보 설정값 ?                                  
                                               ,CORN_GRD_3TRK1bs              --   코너 그라인드 3트랙1bs                                     
                                               ,CORN_GRD_3TRK2bs              --   코너 그라인드 3트랙2bs                                        
                                               ,CORN_GRD_3TRK3fs              --   코너 그라인드 3트랙3fs                                       
                                               ,CORN_GRD_5TRK1bs              --   코너 그라인드 5트랙1bs                                       
                                               ,CORN_GRD_5TRK2bs              --   코너 그라인드 5트랙2bs                                       
                                               ,CORN_GRD_5TRK3bs              --   코너 그라인드 5트랙3bs                                       
                                               ,CORN_GRD_5TRK4fs              --   코너 그라인드 5트랙4fs                                       
                                               ,CORN_GRD_5TRK5fs              --   코너 그라인드 5트랙5fs                                       
                                               ,OutsIDe_back                  --   Outside back                                                 
                                               ,OutsIDe_front                 --   Outside front                                                
                                               ,WORK_SETV_Top                 --   작업 설정값 Top                                    
                                               ,WORK_SETV_Right               --   작업 설정값 Right                                   
                                               ,WORK_SETV_Bottom              --   작업 설정값 Bottom                                    
                                               ,WORK_SETV_Left                --   작업 설정값 Left                      
                                           FROM TB_GRD_OPER_INFO                                              
                                          WHERE CONVERT(VARCHAR(19), WORK_DDTT, 120) BETWEEN '{0}' AND '{1}'  
                                         ORDER BY WORK_DDTT DESC     
                                               ", tb_StartDT.Text, tb_EndDT.Text);


                olddtMain2 = cd.FindDataTable(sql);
                moddtMain2 = olddtMain2.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain2, moddtMain2);

                if (moddtMain2.Rows.Count > 1 && clsFlexGrid.IsSubTotal)
                {
                    UpdateGrdMain2Totals();
                }
                else
                {
                    main2_GridRowsCount = 2;
                }
                grdMain2.AutoSizeCols();

                SetupColumShow();

                //DataTable transpose = GenerateTransposedTable(moddtMain2);



                this.Cursor = Cursors.Default;
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} ({TabP2.Text}): {moddtMain2.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {


            }

        }

        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

        private void UpdateGrdMain2Totals()
        {
            // Show OutlineBar on column 0.
            grdMain2.Tree.Column = 1;
            grdMain2.Tree.Style = TreeStyleFlags.Simple;
            //grdMain2.Rows[3].Style = grdMain2.Styles[CellStyleEnum.GrandTotal];// = grdMain2.Styles[CellStyleEnum.GrandTotal];

            //CellStyle cs;
            //cs = grdMain2.Styles[CellStyleEnum.GrandTotal];
            //cs.BackColor = Color.Black;
            //cs.ForeColor = Color.White;
            //cs.Font = new Font("돋움체", clsFlexGrid.CommonFontSize, FontStyle.Bold);

            // clear existing totals
            grdMain2.Subtotal(AggregateEnum.Clear);

            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["HEIGHT_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WIDTH_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["HEIGHT_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WIDTH_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["LENGTH_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["FACE_PASS_CNT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_PASS_CNT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WORK_SEQ"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WORK_Track_NO"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["RBGW_SPEED_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["RBGW_ADV_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["RBGW_DIA_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["RBGW_ENGY_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["RBGW_VIBR_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["ROTANGLE_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["ROTANGLE_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["GRD_PRES_SURFACE_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["GRD_PRES_ENDP_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["GRD_PRES_CORN_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["SET_GRD_Force"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["GRD_PRES_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["TRUCKSPEED_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["TRUCKSPEED_MV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["GRD_LOC"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["COOL_DOWN_HR_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["COOL_DOWN_SPEED_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["BELT_SLIP_WARN_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_3TRK1bs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_3TRK2bs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_3TRK3fs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_5TRK1bs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_5TRK2bs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_5TRK3bs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_5TRK4fs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["CORN_GRD_5TRK5fs"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["OutsIDe_back"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["OutsIDe_front"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WORK_SETV_Top"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WORK_SETV_Right"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WORK_SETV_Bottom"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["WORK_SETV_Left"].Index, "평균");




            //grdMain2.Rows.Fixed = 1;
            grdMain2.Rows.Frozen = 1;
            main2_GridRowsCount = 3;

            //grdMain2.Cols[grdMain2.Cols["ROLLER_CONVR_SPEED"].Index].Format = "N0";
            //grdMain2.Cols[grdMain2.Cols["IMPELLER_NO1_SPEED"].Index].Format = "N0";

            grdMain2.Rows[2].Height = GrandTotalHeight;

            //grdMain2.Rows[3].Style.BackColor = Color.Blue;
            //grdMain2.Rows[3].Style.ForeColor = Color.White;

            clsFlexGrid.GridCellRangeStyleColor(grdMain2, 2, 0, 2, grdMain2.Cols.Count - 1, Color.Blue, Color.White);
        }

        private void SetupColumShow()
        {
            //드라이브 모드(전체 1(표면+코너),표면2,코너3,부분4)에 따라 실적값 변경 등록.(공통코드참조) , 화면쪽 수정 필요.
            //전체 1인경우 표면 코너 둘다 표현(표면+코너)
            //표면 2인경우 표면       만  표현(면)
            //코너 3인경우      코너  만  표현
            //전체 4인경우 표면 코너 둘다 표현.
            string showIndex = grdMain2.GetData(main2_GridRowsCount, "GRD_DRV_MODE_NM").ToString();
            switch (showIndex)
            {
                case "표면+코너":
                    grdMain2.Cols["FACE_PASS_CNT"].Visible = true;
                    grdMain2.Cols["CORN_PASS_CNT"].Visible = true;
                    break;
                case "면":
                    grdMain2.Cols["FACE_PASS_CNT"].Visible = true;
                    grdMain2.Cols["CORN_PASS_CNT"].Visible = false;
                    break;
                case "코너":
                    grdMain2.Cols["FACE_PASS_CNT"].Visible = false;
                    grdMain2.Cols["CORN_PASS_CNT"].Visible = true;
                    break;
                case "부분":
                    grdMain2.Cols["FACE_PASS_CNT"].Visible = true;
                    grdMain2.Cols["CORN_PASS_CNT"].Visible = true;
                    break;
                default:
                    break;
            }

        }

        private void FlexMain1GridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 150;
            grdItem.Cols[2].Width = 150;
            grdItem.Cols[3].Width = 150;
            grdItem.Cols[4].Width = 150;
            grdItem.Cols[5].Width = 150;
            grdItem.Cols[6].Width = 150;
            grdItem.Cols[7].Width = 150;
            grdItem.Cols[8].Width = 150;
            grdItem.Cols[9].Width = 200;
            grdItem.Cols[10].Width = 200;
            grdItem.Cols[11].Width = 150;
            grdItem.Cols[12].Width = 150;
            grdItem.Cols[13].Width = 150;
            grdItem.Cols[14].Width = 150;
            grdItem.Cols[15].Width = 150;
            grdItem.Cols[16].Width = 150;
            grdItem.Cols[17].Width = 150;
            grdItem.Cols[18].Width = 150;
            grdItem.Cols[19].Width = 200;
            grdItem.Cols[20].Width = 200;
            grdItem.Cols[21].Width = 200;
            grdItem.Cols[22].Width = 200;
            grdItem.Cols[23].Width = 200;
            grdItem.Cols[24].Width = 200;
            grdItem.Cols[25].Width = 200;
            grdItem.Cols[26].Width = 200;
            grdItem.Cols[27].Width = 200;
            grdItem.Cols[28].Width = 200;
            grdItem.Cols[29].Width = 200;
            grdItem.Cols[30].Width = 200;
            grdItem.Cols[31].Width = 200;
            grdItem.Cols[32].Width = 150;
            grdItem.Cols[33].Width = 150;
            grdItem.Cols[34].Width = 150;
            grdItem.Cols[35].Width = 150;
            grdItem.Cols[36].Width = 150;
            grdItem.Cols[37].Width = 150;
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "BLOOM ID";
            grdItem[1, 2] = "재작업순번";
            grdItem[1, 3] = "강종";
            grdItem[1, 4] = "품목";
            grdItem[1, 5] = "규격";
            grdItem[1, 6] = "STEEL_TYPE";
            grdItem[1, 7] = "생산일자";
            grdItem[1, 8] = "작업일자";
            grdItem[1, 9] = "시작시각";
            grdItem[1, 10] = "종료시각";
            grdItem[1, 11] = "표면상태";
            grdItem[1, 12] = "QR스캔정보";
            grdItem[1, 13] = "그라인딩 운전모드";
            grdItem[1, 14] = "높이 측정값";
            grdItem[1, 15] = "폭 측정값";
            grdItem[1, 16] = "길이 측정값";
            grdItem[1, 17] = "면 Pass수";
            grdItem[1, 18] = "코더 Pass수";
            grdItem[1, 19] = "연마지석 속도 설정값";
            grdItem[1, 20] = "연마지석 전진 설정값";
            grdItem[1, 21] = "연마지석 직경 측정값";
            grdItem[1, 22] = "연마지석 에너지 측정값";
            grdItem[1, 23] = "연마지석 진동 측정값";
            grdItem[1, 24] = "회전각도 설정값";
            grdItem[1, 25] = "회전각도 측정값";
            grdItem[1, 26] = "그라인딩압력표면 설정값";
            grdItem[1, 27] = "그라인딩압력끝단 설정값";
            grdItem[1, 28] = "그라인딩압력코너 설정값";
            grdItem[1, 29] = "그라인딩압력 측정값";
            grdItem[1, 30] = "대차속도 설정값";
            grdItem[1, 31] = "대차속도 측정값";
            grdItem[1, 32] = "작업 설정값 T";
            grdItem[1, 33] = "작업 설정값 R";
            grdItem[1, 34] = "작업 설정값 B";
            grdItem[1, 35] = "작업 설정값 L";
            grdItem[1, 36] = "재작업유무";
            grdItem[1, 37] = "전송상태";

            grdItem.Cols[6].Visible = false;
            grdItem.Cols[36].Visible = false;
            grdItem.Cols[37].Visible = false;

            grdItem.AllowEditing = false;
            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 0);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 1, 2);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3, grdItem.Cols.Count - 1);

            clsFlexGrid.DataGridFormat(grdItem, 14, 35, "N0");

        }
        private void FlexMain2GridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 200;
            grdItem.Cols[2].Width = 150;
            grdItem.Cols[3].Width = 150;
            grdItem.Cols[4].Width = 150;
            grdItem.Cols[5].Width = 150;
            grdItem.Cols[6].Width = 150;
            grdItem.Cols[7].Width = 150;
            grdItem.Cols[8].Width = 150;
            grdItem.Cols[9].Width = 150;
            grdItem.Cols[10].Width = 150;
            grdItem.Cols[11].Width = 150;
            grdItem.Cols[12].Width = 200;
            grdItem.Cols[13].Width = 200;
            grdItem.Cols[14].Width = 200;
            grdItem.Cols[15].Width = 200;
            grdItem.Cols[16].Width = 200;
            grdItem.Cols[17].Width = 200;
            grdItem.Cols[18].Width = 200;
            grdItem.Cols[19].Width = 200;
            grdItem.Cols[20].Width = 200;
            grdItem.Cols[21].Width = 200;
            grdItem.Cols[22].Width = 200;
            grdItem.Cols[23].Width = 200;
            grdItem.Cols[24].Width = 200;
            grdItem.Cols[25].Width = 170;
            grdItem.Cols[26].Width = 350;
            grdItem.Cols[27].Width = 350;
            grdItem.Cols[28].Width = 350;
            grdItem.Cols[29].Width = 350;
            grdItem.Cols[30].Width = 350;
            grdItem.Cols[31].Width = 350;
            grdItem.Cols[32].Width = 350;
            grdItem.Cols[33].Width = 350;
            grdItem.Cols[34].Width = 350;
            grdItem.Cols[35].Width = 350;
            grdItem.Cols[36].Width = 350;
            grdItem.Cols[37].Width = 350;
            grdItem.Cols[38].Width = 350;
            grdItem.Cols[39].Width = 150;
            grdItem.Cols[40].Width = 150;
            grdItem.Cols[41].Width = 150;
            grdItem.Cols[42].Width = 150;

            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "그라인딩 운전모드";
            grdItem[1, 3] = "높이 설정값";
            grdItem[1, 4] = "폭 설정값";
            grdItem[1, 5] = "높이 측정값";
            grdItem[1, 6] = "폭 측정값";
            grdItem[1, 7] = "길이 측정값";
            grdItem[1, 8] = "면 Pass수";
            grdItem[1, 9] = "코더 Pass수";
            grdItem[1, 10] = "작업순서";
            grdItem[1, 11] = "작업TrackNo";
            grdItem[1, 12] = "연마지석 속도 설정값";
            grdItem[1, 13] = "연마지석 전진 설정값";
            grdItem[1, 14] = "연마지석 직경 측정값";
            grdItem[1, 15] = "연마지석 에너지 측정값";
            grdItem[1, 16] = "연마지석 진동 측정값";
            grdItem[1, 17] = "회전각도 설정값";
            grdItem[1, 18] = "회전각도 측정값";
            grdItem[1, 19] = "그라인딩압력표면 설정값";
            grdItem[1, 20] = "그라인딩압력끝단 설정값";
            grdItem[1, 21] = "그라인딩압력코너 설정값";
            grdItem[1, 22] = "그라인딩압력 측정값";
            grdItem[1, 23] = "대차속도 설정값";
            grdItem[1, 24] = "대차속도 측정값";
            grdItem[1, 25] = "그라인딩위치";
            grdItem[1, 26] = "그라인딩설비 쿨다운시간 설정값";
            grdItem[1, 27] = "그라인딩설비 쿨다운속도 설정값";
            grdItem[1, 28] = "그라인딩설비 벨트슬립경보 설정값";
            grdItem[1, 29] = "코더그라인드 파라미터 3트랙1(backside)";
            grdItem[1, 30] = "코더그라인드 파라미터 3트랙2(backside)";
            grdItem[1, 31] = "코더그라인드 파라미터 3트랙3(frontside)";
            grdItem[1, 32] = "코더그라인드 파라미터 5트랙1(backside)";
            grdItem[1, 33] = "코더그라인드 파라미터 5트랙2(backside)";
            grdItem[1, 34] = "코더그라인드 파라미터 5트랙3(backside)";
            grdItem[1, 35] = "코더그라인드 파라미터 5트랙4(frontside)";
            grdItem[1, 36] = "코더그라인드 파라미터 5트랙5(frontside)";
            grdItem[1, 37] = "Start distancd from Outside back";
            grdItem[1, 38] = "Start distancd from Outside front";
            grdItem[1, 39] = "작업 설정값 T";
            grdItem[1, 40] = "작업 설정값 R";
            grdItem[1, 41] = "작업 설정값 B";
            grdItem[1, 42] = "작업 설정값 L";

            grdItem.AllowEditing = false;

            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count - 1);

            clsFlexGrid.DataGridFormat(grdItem, 3, 42, "N0");
        }

        #endregion

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

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
