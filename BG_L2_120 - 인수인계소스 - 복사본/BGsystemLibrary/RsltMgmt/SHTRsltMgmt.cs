using BGsystemLibrary.Common;
using C1.Win.C1Command;
using C1.Win.C1FlexGrid;
using C1.Win.C1Input;
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

namespace BGsystemLibrary.MatMgmt
{
    public partial class SHTRsltMgmt : Form
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
        private int main1_GridColsCount = 23;
        private int main1_RowsFixed = 2;
        private int main1_RowsFrozen = 0;
        private int main1_ColsFixed = 0;
        private int main1_ColsFrozen = 5;

        private int main2_GridRowsCount = 2;
        private int main2_GridColsCount = 22;
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
        private int subtotalNo;

        #endregion 공통 생성자

        public SHTRsltMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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
                //grdMain1.RowSel = 3;
                SetupSelectedRow();
            }

        }
        private void DrawMainGrid(C1FlexGrid grdItem, DataTable dataTable)
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
                //clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
                //clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");


                //grdItem.AutoResize = true;
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
        private void DrawSubGrid(C1FlexGrid grdItem, DataTable dataTable)
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

                //grdItem.Cols["ROLLER_CONVR_SPEED"].DataType = Type.d;
                //grdItem.Cols["ROLLER_CONVR_SPEED"].Format = "N0";
                //grdItem.AutoResize = true;
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

        private void SHTRsltMgmt_Load(object sender, EventArgs e)
        {
            //그리드 초기화
            DrawMain1Grid(grdMain1);
            DrawMain2Grid(grdMain2);
            //초기화
            InitControl();
            //조회버튼 클릭
            Button_Click(btnDisplay, null);
        }

        private void FlexMain1GridCol(C1FlexGrid grdItem)
        {
            //컬럼 Width 사이즈
            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 100;
            grdItem.Cols[3].Width = 100;
            grdItem.Cols[4].Width = 100;
            grdItem.Cols[5].Width = 100;
            grdItem.Cols[6].Width = 100;
            grdItem.Cols[7].Width = 100;
            grdItem.Cols[8].Width = 100;
            grdItem.Cols[9].Width = 200;
            grdItem.Cols[10].Width = 200;
            grdItem.Cols[11].Width = 150;
            grdItem.Cols[12].Width = 150;
            grdItem.Cols[13].Width = 150;
            grdItem.Cols[14].Width = 150;
            grdItem.Cols[15].Width = 150;
            grdItem.Cols[16].Width = 150;
            grdItem.Cols[17].Width = 200;
            grdItem.Cols[18].Width = 200;
            grdItem.Cols[19].Width = 200;
            grdItem.Cols[20].Width = 200;
            grdItem.Cols[21].Width = 200;
            grdItem.Cols[22].Width = 150;

            grdItem.Cols[4].Visible = false;
            grdItem.Cols[6].Visible = false;
            grdItem.Cols[22].Visible = false;

            grdItem.AllowEditing = false;

            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "BLOOM ID";
            grdItem[1, 2] = "재작업순번";
            grdItem[1, 3] = "강종";
            grdItem[1, 4] = "ITEM";
            grdItem[1, 5] = "규격";
            grdItem[1, 6] = "STEEL_TYPE";
            grdItem[1, 7] = "생산일자";
            grdItem[1, 8] = "작업일자";
            grdItem[1, 9] = "시작시각";
            grdItem[1, 10] = "종료시각";
            grdItem[1, 11] = "QR스캔정보";
            grdItem[1, 12] = "롤러컨베어 속도";
            grdItem[1, 13] = "임펠라#1속도";
            grdItem[1, 14] = "임펠라#2속도";
            grdItem[1, 15] = "임펠라#3속도";
            grdItem[1, 16] = "임펠라#4속도";
            grdItem[1, 17] = "롤러컨베어속도설정값";
            grdItem[1, 18] = "임펠라#1속도 설정값";
            grdItem[1, 19] = "임펠라#2속도 설정값";
            grdItem[1, 20] = "임펠라#3속도 설정값";
            grdItem[1, 21] = "임펠라#4속도 설정값";
            grdItem[1, 22] = "재작업유무";

            grdItem.AllowEditing = false;
            //그리드 스타일 적용
            //헤더
            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            //데이터로우
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 0);
            clsFlexGrid.DataGridCenterBoldStyle(grdItem, 1, 2);
            clsFlexGrid.DataGridCenterStyle(grdItem, 3, grdItem.Cols.Count - 1);

            //format 설정
            clsFlexGrid.DataGridFormat(grdItem, 12, 21, "N0");
            //grdItem.AutoResize = true;
            
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
            grdItem.Cols[10].Width = 200;
            grdItem.Cols[11].Width = 200;
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

            grdItem.AllowEditing = false;

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
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, grdItem.Cols.Count-1);

            clsFlexGrid.DataGridFormat(grdItem, 2, 21, "N0");


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
                sql += string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY SHT_START_DDTT DESC)) AS L_NUM                            
                                              ,CONVERT(VARCHAR,HEAT_NO) +' '+ CONVERT(VARCHAR,HEAT_SEQ)  AS BLOOM_ID  
                                        	  ,REWORK_SNO +1                                                                                       
                                              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = STEEL) AS STEEL_NM          
                                              ,ITEM                                                                                             
                                              ,(SELECT CD_NM         FROM TB_CM_COM_CD           WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = ITEM + ITEM_SIZE) AS ITEM_SIZE_NM  
                                              ,STEEL_TYPE                                                                                       
                                              ,CONVERT(DATE, MFG_DATE)   AS MFG_DATE                                                            
                                              ,CONVERT(DATE, WORK_DATE)   AS WORK_DATE                                                          
                                              ,CONVERT(CHAR(19), SHT_START_DDTT, 120)  START_DDTT                                               
                                              ,CONVERT(CHAR(19), SHT_END_DDTT, 120)    END_DDTT  
                                              ,QR_SCAN_INFO                                               
                                        	  ,ROLLER_CONVR_SPEED                                                                               
                                              ,IMPELLER_NO1_SPEED                                                                             
                                              ,IMPELLER_NO2_SPEED                                                                               
                                              ,IMPELLER_NO3_SPEED                                                                               
                                              ,IMPELLER_NO4_SPEED                                                                               
                                              ,ROLLER_CONVR_SPEED_SETV                                                                          
                                              ,IMPELLER_NO1_SPEED_SETV                                                                          
                                              ,IMPELLER_NO2_SPEED_SETV                                                                          
                                              ,IMPELLER_NO3_SPEED_SETV                                                                          
                                        	  ,IMPELLER_NO4_SPEED_SETV                                                                         
                                              ,REWORK_YN                                                                                        
                                          FROM TB_SHT_WR                                                                                        
                                         WHERE FORMAT(SHT_START_DDTT,'yyyyMMddHHmmss')  >= '{0}'
                                           AND FORMAT(SHT_START_DDTT,'yyyyMMddHHmmss')  < '{1}'
                                           AND HEAT_NO LIKE '{2}%'
                                       ORDER BY SHT_START_DDTT DESC
                                                  ", _start_dt, _end_dt, txt_Heat.Text);

                olddtMain1 = cd.FindDataTable(sql);
                moddtMain1 = olddtMain1.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawMainGrid(grdMain1, moddtMain1);

                if (moddtMain1.Rows.Count > 1 && clsFlexGrid.IsSubTotal)
                {
                    UpdateGrdMain1Totals();
                }else
                {
                    main1_GridRowsCount = 2;
                }

                this.Cursor = Cursors.Default;
                grdMain1.AutoSizeCols();
                SelectRow();
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} ({TabP1.Text}): {moddtMain1.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                //throw;
            }



        }

        private void UpdateGrdMain1Totals()
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

            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["ROLLER_CONVR_SPEED"     ].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO1_SPEED"     ].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO2_SPEED"     ].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO3_SPEED"     ].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO4_SPEED"     ].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["ROLLER_CONVR_SPEED_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO1_SPEED_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO2_SPEED_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO3_SPEED_SETV"].Index, "평균");
            grdMain1.Subtotal(AggregateEnum.Average, -1, -1, grdMain1.Cols["IMPELLER_NO4_SPEED_SETV"].Index, "평균");

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
                                               ,ROLLER_CONVR_SPEED                                                    
                                               ,IMPELLER_NO1_SPEED                                                   
                                               ,IMPELLER_NO2_SPEED                                                   
                                               ,IMPELLER_NO3_SPEED                                                   
                                               ,IMPELLER_NO4_SPEED                                                   
                                               ,ROLLER_CONVR_CURRENT                                                 
                                               ,IMPELLER_NO1_CURRENT                                                 
                                               ,IMPELLER_NO2_CURRENT                                                 
                                               ,IMPELLER_NO3_CURRENT                                                 
                                               ,IMPELLER_NO4_CURRENT                                                 
                                               ,ROTY_SCR_CURRENT                                                     
                                               ,BCKT_LVTR_CURRENT                                                    
                                               ,SCREW_CONVR1_CURRENT                                                 
                                               ,SCREW_CONVR2_CURRENT                                                 
                                               ,FAN_CURRENT                                                          
                                               ,ROLLER_CONVR_SPEED_SETV                                              
                                               ,IMPELLER_NO1_SPEED_SETV                                              
                                               ,IMPELLER_NO2_SPEED_SETV                                              
                                               ,IMPELLER_NO3_SPEED_SETV                                              
                                               ,IMPELLER_NO4_SPEED_SETV                                              
                                           FROM TB_SHT_OPER_INFO                                                        
                                          WHERE CONVERT(VARCHAR(19), WORK_DDTT, 120) BETWEEN '{0}' AND '{1}'   
                                       ORDER BY WORK_DDTT DESC     
                                               ", tb_StartDT.Text, tb_EndDT.Text);



                olddtMain2 = cd.FindDataTable(sql);
                moddtMain2 = olddtMain2.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawSubGrid(grdMain2, moddtMain2);

                if (moddtMain2.Rows.Count > 1 && clsFlexGrid.IsSubTotal)
                {
                    UpdateGrdMain2Totals();
                }
                else
                {
                    main2_GridRowsCount = 2;
                }
                grdMain2.AutoSizeCols();

                this.Cursor = Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} ({TabP2.Text}): {moddtMain2.Rows.Count} 건 조회 되었습니다.");

            }
            catch (Exception ex)
            {


            }


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

            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["ROLLER_CONVR_SPEED"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO1_SPEED"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO2_SPEED"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO3_SPEED"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO4_SPEED"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["ROLLER_CONVR_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO1_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO2_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO3_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO4_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["ROTY_SCR_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["BCKT_LVTR_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["SCREW_CONVR1_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["SCREW_CONVR2_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["FAN_CURRENT"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["ROLLER_CONVR_SPEED_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO1_SPEED_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO2_SPEED_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO3_SPEED_SETV"].Index, "평균");
            grdMain2.Subtotal(AggregateEnum.Average, -1, -1, grdMain2.Cols["IMPELLER_NO4_SPEED_SETV"].Index, "평균");


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
    }
}
