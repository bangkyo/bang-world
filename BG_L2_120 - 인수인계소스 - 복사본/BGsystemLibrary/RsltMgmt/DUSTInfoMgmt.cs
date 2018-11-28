using C1.Win.C1Command;
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
    public partial class DUSTInfoMgmt : Form
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
        private int main1_GridColsCount = 33;
        private int main1_RowsFixed = 2;
        private int main1_RowsFrozen = 0;
        private int main1_ColsFixed = 0;
        private int main1_ColsFrozen = 2;

        private int main2_GridRowsCount = 2;
        private int main2_GridColsCount = 52;
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

                    //조회시간 시간이하로 제한
                    int limtedHours = 48;
                    if (IsOverLimtedTime(start_dt.Value, end_dt.Value, limtedHours))
                    {
                        MessageBox.Show($"{limtedHours.ToString()}시간 이하로 조회하시기 바랍니다.");
                        return;
                    }

                    cd.InsertLogForSearch(ck.UserID, btnDisplay, "");

                    Search();

                    break;



                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

        private bool IsOverLimtedTime(DateTime start, DateTime end, int limitedHours)
        {
            bool isOver = false;
            if (GetDurationHour(start,end) > limitedHours)
            {
                isOver = true;
            }
            return isOver;
        }

        private int GetDurationHour(DateTime starttime, DateTime endtime)
        {
            int timespan = 0;
            TimeSpan ts = endtime - starttime;
            timespan = Convert.ToInt32(Math.Ceiling(ts.TotalHours));
            
            return timespan;
        }

        private void Search()
        {
            //활성화된 텝의 그리드를 조회한다..

            C1FlexGrid selectedGrd = SelectedGrd_InTab();

            if (selectedGrd.Name == "grdMain1")
            {
                SetDataBinding();
            }
            else if (selectedGrd.Name == "grdMain2")
            {
                SetDataBindingMain2();
            }
        }

        private C1FlexGrid SelectedGrd_InTab()
        {
            C1FlexGrid selectedGrd = null;

            C1.Win.C1Command.C1DockingTabPage selectedTab = TabOpt.SelectedTab;

            string grd_Nm = "";
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

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM +"_"+ TabOpt.SelectedTab.Text, SelectedGrd_InTab());
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

            start_dt.Value = DateTime.Now.Date.AddHours(6);
            end_dt.Value = DateTime.Now;
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
                clsFlexGrid.grdDataClearForBind(grdMain2);

                SetDataBindingMain2();
            }


        }






        #endregion

        public DUSTInfoMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
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

        private void DUSTInfoMgmt_Load(object sender, EventArgs e)
        {
            //그리드 초기화
            DrawMain1Grid(grdMain1);
            DrawMain2Grid(grdMain2);
            //초기화
            InitControl();
            //조회버튼 클릭
            Button_Click(btnDisplay, null);
        }



        private void SetDataBinding()
        {
            //clsFlexGrid.grdDataClearForBind(grdMain1);

            try
            {
                string _start_dt = vf.Format(start_dt.Value, "yyyy-MM-dd HH:mm:ss");
                string _end_dt = vf.Format(end_dt.Value, "yyyy-MM-dd HH:mm:ss");

                string sql = "";
                sql += string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY WORK_DDTT DESC)) AS L_NUM  
                                          ,CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                     
                                          ,CONVERT(VARCHAR(19),DUST_On_TR, 120)  AS DUST_On_TR                  
                                          ,CONVERT(VARCHAR(19),DUST_Off_TR, 120)  AS DUST_Off_TR                
                                          ,Bag_BEF_STATICP
                                          ,INH_TEMP
                                          ,Screw_Conveyor_STAT
                                          ,Screw_Conveyor_ABNORM_STAT
                                          ,Rotary_Valve_STAT
                                          ,Rotary_Valve_ABNORM_STAT
                                          ,Vibrator_STAT
                                          ,Vibrator_ABNORM_STAT
                                          ,Pulse_Controller_STAT1
                                          ,Pulse_Controller_STAT2
                                          ,Pulse_Controller_STAT3
                                          ,Pulse_Controller_STAT4
                                          ,Pulse_Controller_STAT5
                                          ,Pulse_Controller_STAT6
                                          ,Pulse_Controller_STAT7
                                          ,Pulse_Controller_STAT8
                                          ,Pulse_Controller_STAT9
                                          ,Pulse_Controller_STAT10
                                          ,Pulse_Controller_STAT11
                                          ,Pulse_Controller_STAT12
                                          ,Pulse_Controller_STAT13
                                          ,DEF_PRES
                                          ,OPRN_RT
                                          ,SETV
                                          ,LOAD_BRG_TEMP
                                          ,H_LOAD_BRG_TEMP
                                          ,IVT_SPEED_SETV
                                          ,LOAD_VIBR
                                          ,H_LOAD_VIBR                                                         
                                      FROM TB_SHT_DUST_INFO                                                     
                                     WHERE WORK_DDTT BETWEEN '{0}' AND '{1}'                
                                 ORDER BY WORK_DDTT DESC              
                                     ", _start_dt, _end_dt);

                olddtMain1 = cd.FindDataTable(sql);
                moddtMain1 = olddtMain1.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain1, moddtMain1);

                if (moddtMain1.Rows.Count > 1 && clsFlexGrid.IsSubTotal)
                {
                    UpdateGrdMain1Totals(grdMain1);
                }
                else
                {
                    main1_GridRowsCount = 2;
                }


                this.Cursor = Cursors.Default;

                SelectRow();

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} ({TabP1.Text}): {moddtMain1.Rows.Count} 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void UpdateGrdMain1Totals(C1FlexGrid grd)
        {
            // Show OutlineBar on column 0.
            grd.Tree.Column = 1;
            grd.Tree.Style = TreeStyleFlags.Simple;
            //grd.Rows[3].Style = grd.Styles[CellStyleEnum.GrandTotal];// = grd.Styles[CellStyleEnum.GrandTotal];

            //CellStyle cs;
            //cs = grd.Styles[CellStyleEnum.GrandTotal];
            //cs.BackColor = Color.Black;
            //cs.ForeColor = Color.White;
            //cs.Font = new Font("돋움체", clsFlexGrid.CommonFontSize, FontStyle.Bold);

            // clear existing totals
            grd.Subtotal(AggregateEnum.Clear);

            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["Bag_BEF_STATICP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["INH_TEMP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["DEF_PRES"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["OPRN_RT"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["SETV"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["LOAD_BRG_TEMP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["H_LOAD_BRG_TEMP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["IVT_SPEED_SETV"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["LOAD_VIBR"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["H_LOAD_VIBR"].Index, "평균");


            //grd.Rows.Fixed = 1;
            grd.Rows.Frozen = 1;
            main1_GridRowsCount = 3;

            //grd.Cols[grd.Cols["ROLLER_CONVR_SPEED"].Index].Format = "N0";
            //grd.Cols[grd.Cols["IMPELLER_NO1_SPEED"].Index].Format = "N0";

            grd.Rows[2].Height = GrandTotalHeight;

            //grd.Rows[3].Style.BackColor = Color.Blue;
            //grd.Rows[3].Style.ForeColor = Color.White;

            clsFlexGrid.GridCellRangeStyleColor(grd, 2, 0, 2, grd.Cols.Count - 1, Color.Blue, Color.White);

            //grd.Invalidate();
        }

        private void UpdateGrdMain2Totals()
        {

            
        }

        private void SetDataBindingMain2()
        {
            //clsFlexGrid.grdDataClearForBind(grdMain2);

            try
            {

                string _start_dt = vf.Format(start_dt.Value, "yyyy-MM-dd HH:mm:ss");
                string _end_dt = vf.Format(end_dt.Value, "yyyy-MM-dd HH:mm:ss");

                string sql = "";
                sql += string.Format(@" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY WORK_DDTT DESC)) AS L_NUM  
                                             ,CONVERT(VARCHAR(19),WORK_DDTT, 120) AS WORK_DDTT                      
                                             ,CONVERT(VARCHAR(19),DUST_On_TR, 120) AS DUST_On_TR                                                            
                                             ,CONVERT(VARCHAR(19),DUST_Off_TR, 120) AS DUST_Off_TR  
                                             ,Bag_BEF_STATICP
                                             ,INH_TEMP                                                         
                                             ,Rotary_Valve_NO1_STAT
                                             ,Rotary_Valve_NO1_ABNORM_STAT
                                             ,Rotary_Valve_NO2_STAT
                                             ,Rotary_Valve_NO2_ABNORM_STAT
                                             ,Rotary_Valve_NO3_STAT
                                             ,Rotary_Valve_NO3_ABNORM_STAT
                                             ,Rotary_Valve_NO4_STAT
                                             ,Rotary_Valve_NO4_ABNORM_STAT
                                             ,Vibrator_NO1_STAT
                                             ,Vibrator_NO1_ABNORM_STAT
                                             ,Vibrator_NO2_STAT
                                             ,Vibrator_NO2_ABNORM_STAT
                                             ,Vibrator_NO3_STAT
                                             ,Vibrator_NO3_ABNORM_STAT
                                             ,Pulse_Controller_STAT1
                                             ,Pulse_Controller_STAT2
                                             ,Pulse_Controller_STAT3
                                             ,Pulse_Controller_STAT4
                                             ,Pulse_Controller_STAT5
                                             ,Pulse_Controller_STAT6
                                             ,Pulse_Controller_STAT7
                                             ,Pulse_Controller_STAT8
                                             ,Pulse_Controller_STAT9
                                             ,Pulse_Controller_STAT10
                                             ,Pulse_Controller_STAT11
                                             ,Pulse_Controller_STAT12
                                             ,Pulse_Controller_STAT13
                                             ,Pulse_Controller_STAT14
                                             ,Pulse_Controller_STAT15
                                             ,Pulse_Controller_STAT16
                                             ,Pulse_Controller_STAT17
                                             ,Pulse_Controller_STAT18
                                             ,Pulse_Controller_STAT19
                                             ,Popet_Damper_NO1_STAT
                                             ,Popet_Damper_NO2_STAT
                                             ,Popet_Damper_NO3_STAT
                                             ,DEF_PRES_NO1
                                             ,DEF_PRES_NO2
                                             ,DEF_PRES_NO3
                                             ,OPRN_RT
                                             ,SETV
                                             ,LOAD_BRG_TEMP
                                             ,H_LOAD_BRG_TEMP
                                             ,IVT_SPEED_SETV
                                             ,LOAD_VIBR
                                             ,H_LOAD_VIBR                                                         
                                         FROM TB_GRD_DUST_INFO                                                      
                                        WHERE WORK_DDTT BETWEEN '{0}' AND '{1}'   
                                     ORDER BY WORK_DDTT DESC              
                                            ", _start_dt, _end_dt);



                olddtMain2 = cd.FindDataTable(sql);
                moddtMain2 = olddtMain2.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain2, moddtMain2);

                if (moddtMain2.Rows.Count > 1 && clsFlexGrid.IsSubTotal)
                {
                    UpdateGrdMain2Totals(grdMain2);
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

        private void UpdateGrdMain2Totals(C1FlexGrid grd)
        {
            // Show OutlineBar on column 0.
            grd.Tree.Column = 1;
            grd.Tree.Style = TreeStyleFlags.Simple;
            //grd.Rows[3].Style = grd.Styles[CellStyleEnum.GrandTotal];// = grd.Styles[CellStyleEnum.GrandTotal];

            //CellStyle cs;
            //cs = grd.Styles[CellStyleEnum.GrandTotal];
            //cs.BackColor = Color.Black;
            //cs.ForeColor = Color.White;
            //cs.Font = new Font("돋움체", clsFlexGrid.CommonFontSize, FontStyle.Bold);

            // clear existing totals
            grd.Subtotal(AggregateEnum.Clear);

            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["Bag_BEF_STATICP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["INH_TEMP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["DEF_PRES_NO1"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["DEF_PRES_NO2"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["DEF_PRES_NO3"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["OPRN_RT"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["SETV"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["LOAD_BRG_TEMP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["H_LOAD_BRG_TEMP"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["IVT_SPEED_SETV"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["LOAD_VIBR"].Index, "평균");
            grd.Subtotal(AggregateEnum.Average, -1, -1, grd.Cols["H_LOAD_VIBR"].Index, "평균");



            //grd.Rows.Fixed = 1;
            grd.Rows.Frozen = 1;
            main2_GridRowsCount = 3;

            //grd.Cols[grd.Cols["ROLLER_CONVR_SPEED"].Index].Format = "N0";
            //grd.Cols[grd.Cols["IMPELLER_NO1_SPEED"].Index].Format = "N0";

            grd.Rows[2].Height = GrandTotalHeight;

            //grd.Rows[3].Style.BackColor = Color.Blue;
            //grd.Rows[3].Style.ForeColor = Color.White;

            clsFlexGrid.GridCellRangeStyleColor(grd, 2, 0, 2, grd.Cols.Count - 1, Color.Blue, Color.White);

            //grd.Invalidate();
        }

        private void FlexMain1GridCol(C1FlexGrid grdItem)
        {

            #region //컬럼 Width 사이즈
            //grdItem.Cols[0].Width = 60;
            //grdItem.Cols[1].Width = 200;
            //grdItem.Cols[2].Width = 200;
            //grdItem.Cols[3].Width = 200;
            //grdItem.Cols[4].Width = 200;
            //grdItem.Cols[5].Width = 150;
            //grdItem.Cols[6].Width = 200;
            //grdItem.Cols[7].Width = 200;
            //grdItem.Cols[8].Width = 200;
            //grdItem.Cols[9].Width = 200;
            //grdItem.Cols[10].Width = 150;
            //grdItem.Cols[11].Width = 150;
            //grdItem.Cols[12].Width = 150;
            //grdItem.Cols[13].Width = 150;
            //grdItem.Cols[14].Width = 150;
            //grdItem.Cols[15].Width = 150;
            //grdItem.Cols[16].Width = 150;
            //grdItem.Cols[17].Width = 150;
            //grdItem.Cols[17].Width = 150;
            //grdItem.Cols[18].Width = 150;
            //grdItem.Cols[19].Width = 150;
            //grdItem.Cols[20].Width = 150;
            //grdItem.Cols[21].Width = 150;
            //grdItem.Cols[22].Width = 150;
            //grdItem.Cols[23].Width = 150;
            //grdItem.Cols[24].Width = 150;
            //grdItem.Cols[25].Width = 150;
            //grdItem.Cols[26].Width = 150;
            //grdItem.Cols[27].Width = 150;
            //grdItem.Cols[28].Width = 150;
            //grdItem.Cols[29].Width = 150;
            //grdItem.Cols[30].Width = 150;
            //grdItem.Cols[31].Width = 150;
            //grdItem.Cols[32].Width = 150;
            #endregion

            #region //컬럼 명 세팅
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "집진 시작(On) 시각";
            grdItem[1, 3] = "집진 종료(Off) 시각";
            grdItem[1, 4] = "Bag전단정압계";
            grdItem[1, 5] = "흡입 온도";
            grdItem[1, 6] = "Screw Conveyor 상태";
            grdItem[1, 7] = "Rotary Valve 상태";
            grdItem[1, 8] = "Vibrator 상태";
            grdItem[1, 9] = "Pulse Controller1 상태";
            grdItem[1, 10] = "Pulse Controller2 상태";
            grdItem[1, 11] = "Pulse Controller3 상태";
            grdItem[1, 12] = "Pulse Controller4 상태";
            grdItem[1, 13] = "Pulse Controller5 상태";
            grdItem[1, 14] = "Pulse Controller6 상태";
            grdItem[1, 15] = "Pulse Controller7 상태";
            grdItem[1, 16] = "Pulse Controller8 상태";
            grdItem[1, 17] = "Pulse Controller9 상태";
            grdItem[1, 18] = "Pulse Controller10 상태";
            grdItem[1, 19] = "Pulse Controller11 상태";
            grdItem[1, 20] = "Pulse Controller12 상태";
            grdItem[1, 21] = "Pulse Controller13 상태";
            grdItem[1, 22] = "Pulse Controller14 상태";
            grdItem[1, 23] = "Pulse Controller15 상태";
            grdItem[1, 24] = "Pulse Controller16 상태";
            grdItem[1, 25] = "차압";
            grdItem[1, 26] = "가동율";
            grdItem[1, 27] = "세팅값";
            grdItem[1, 28] = "부하측베어링온도";
            grdItem[1, 29] = "반부하측베어링온도";
            grdItem[1, 30] = "인버터속도 설정값(Hz)";
            grdItem[1, 31] = "부하진동";
            grdItem[1, 32] = "반부하진동";
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
            clsFlexGrid.DataGridFormat(grdItem, 25, 32, "N1"); //차압, 반부하진동

            grdItem.AutoSizeCols();

        }
        private void FlexMain2GridCol(C1FlexGrid grdItem)
        {
            #region 컬럼 Width 사이즈
            //컬럼 Width 사이즈
            //grdItem.Cols[0].Width = 60;
            //grdItem.Cols[1].Width = 150;
            //grdItem.Cols[2].Width = 150;
            //grdItem.Cols[3].Width = 150;
            //grdItem.Cols[4].Width = 150;
            //grdItem.Cols[5].Width = 150;
            //grdItem.Cols[6].Width = 150;
            //grdItem.Cols[7].Width = 150;
            //grdItem.Cols[8].Width = 150;
            //grdItem.Cols[9].Width = 200;
            //grdItem.Cols[10].Width = 200;
            //grdItem.Cols[11].Width = 200;
            //grdItem.Cols[12].Width = 200;
            //grdItem.Cols[13].Width = 200;
            //grdItem.Cols[14].Width = 200;
            //grdItem.Cols[15].Width = 200;
            //grdItem.Cols[16].Width = 200;
            //grdItem.Cols[17].Width = 200;
            //grdItem.Cols[18].Width = 200;
            //grdItem.Cols[19].Width = 200;
            //grdItem.Cols[20].Width = 200;
            //grdItem.Cols[21].Width = 200;
            //grdItem.Cols[22].Width = 200;
            //grdItem.Cols[23].Width = 200;
            //grdItem.Cols[24].Width = 200;
            //grdItem.Cols[25].Width = 200;
            //grdItem.Cols[26].Width = 200;
            //grdItem.Cols[27].Width = 200;
            //grdItem.Cols[28].Width = 200;
            //grdItem.Cols[29].Width = 200;
            //grdItem.Cols[30].Width = 200;
            //grdItem.Cols[31].Width = 200;
            //grdItem.Cols[32].Width = 200;
            //grdItem.Cols[33].Width = 200;
            //grdItem.Cols[34].Width = 200;
            //grdItem.Cols[35].Width = 200;
            //grdItem.Cols[36].Width = 200;
            //grdItem.Cols[37].Width = 200;
            //grdItem.Cols[38].Width = 200;
            //grdItem.Cols[39].Width = 200;
            //grdItem.Cols[40].Width = 200;
            //grdItem.Cols[41].Width = 200;
            //grdItem.Cols[42].Width = 200;
            //grdItem.Cols[43].Width = 200;
            //grdItem.Cols[44].Width = 200;
            //grdItem.Cols[45].Width = 200;
            //grdItem.Cols[46].Width = 200;
            //grdItem.Cols[47].Width = 200;
            //grdItem.Cols[48].Width = 200;
            //grdItem.Cols[49].Width = 200;
            //grdItem.Cols[50].Width = 200;
            //grdItem.Cols[51].Width = 200;
            #endregion

            #region //컬럼 명 세팅
            //컬럼 명 세팅
            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "자료수집시각";
            grdItem[1, 2] = "집진 시작(On) 시각";
            grdItem[1, 3] = "집진 종료(Off) 시각";
            grdItem[1, 4] = "Bag전단정압계";
            grdItem[1, 5] = "흡입 온도";
            grdItem[1, 6] = "Rotary Valve#1 상태";
            grdItem[1, 7] = "Rotary Valve#1 이상상태";
            grdItem[1, 8] = "Rotary Valve#2 상태";
            grdItem[1, 9] = "Rotary Valve#2 이상상태";
            grdItem[1, 10] = "Rotary Valve#3 상태";
            grdItem[1, 11] = "Rotary Valve#3 이상상태";
            grdItem[1, 12] = "Rotary Valve#4 상태";
            grdItem[1, 13] = "Rotary Valve#4 이상상태";
            grdItem[1, 14] = "Vibrator#1 상태";
            grdItem[1, 15] = "Vibrator#1 이상상태";
            grdItem[1, 16] = "Vibrator#2 상태";
            grdItem[1, 17] = "Vibrator#2 이상상태";
            grdItem[1, 18] = "Vibrator#3 상태";
            grdItem[1, 19] = "Vibrator#3 이상상태";
            grdItem[1, 20] = "Pulse Controller1 상태";
            grdItem[1, 21] = "Pulse Controller2 상태";
            grdItem[1, 22] = "Pulse Controller3 상태";
            grdItem[1, 23] = "Pulse Controller4 상태";
            grdItem[1, 24] = "Pulse Controller5 상태";
            grdItem[1, 25] = "Pulse Controller6 상태";
            grdItem[1, 26] = "Pulse Controller7 상태";
            grdItem[1, 27] = "Pulse Controller8 상태";
            grdItem[1, 28] = "Pulse Controller9 상태";
            grdItem[1, 29] = "Pulse Controller10 상태";
            grdItem[1, 30] = "Pulse Controller11 상태";
            grdItem[1, 31] = "Pulse Controller12 상태";
            grdItem[1, 32] = "Pulse Controller13 상태";
            grdItem[1, 33] = "Pulse Controller14 상태";
            grdItem[1, 34] = "Pulse Controller15 상태";
            grdItem[1, 35] = "Pulse Controller16 상태";
            grdItem[1, 36] = "Pulse Controller17 상태";
            grdItem[1, 37] = "Pulse Controller18 상태";
            grdItem[1, 38] = "Pulse Controller19 상태";
            grdItem[1, 39] = "Popet Damper#1 상태";
            grdItem[1, 40] = "Popet Damper#2 상태";
            grdItem[1, 41] = "Popet Damper#3 상태";
            grdItem[1, 42] = "차압#1";
            grdItem[1, 43] = "차압#2";
            grdItem[1, 44] = "차압#3";
            grdItem[1, 45] = "가동율";
            grdItem[1, 46] = "세팅값";
            grdItem[1, 47] = "부하측베어링온도";
            grdItem[1, 48] = "반부하측베어링온도";
            grdItem[1, 49] = "인버터속도 설정값(Hz)";
            grdItem[1, 50] = "부하진동";
            grdItem[1, 51] = "반부하진동";
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
            clsFlexGrid.DataGridFormat(grdItem, 42, 51, "N1");// 차압 반부하진동

            grdItem.AutoSizeCols();

        }

        private void TabOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            Button_Click(btnDisplay, null);
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
