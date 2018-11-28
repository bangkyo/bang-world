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

namespace BGsystemLibrary.SystemMgmt
{
    public partial class UseLogMgmt : Form
    {
        //공통변수
        private int selectedrow = 0;
        clsStyle cs = new clsStyle();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsCom ck = new clsCom();

        // 셀의 수정전 값
        private static string strBefValue = "";

        // 셀별 수정 정보(이전/이후)
        private static string ownerNM = "";
        private static string titleNM = "";

        //데이터테이블 변수
        DataTable grdMain1_dt = null;
        DataTable grdSub1_dt = null;
        DataTable grdMain2_dt = null;
        DataTable grdSub2_dt = null;
        DataTable olddt_grdMain1 = null;
        DataTable moddt_grdMain1 = null;
        DataTable olddt_grdMain2 = null;
        DataTable moddt_grdMain2 = null;
        DataTable olddt_grdMain3 = null;
        DataTable moddt_grdMain3 = null;
        DataTable olddt_grdSub1 = null;
        DataTable moddt_grdSub1 = null;
        DataTable olddt_grdSub2 = null;
        DataTable moddt_grdSub2 = null;
        DataTable olddt_grdSub3 = null;
        DataTable moddt_grdSub3 = null;
        bool CanDateChange = true;

        //콤보박스 변수
        Hashtable Sys_cd = null;
        private static string cbxSys_id = "";

        //그리드 변수
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();
        private int GridRowsCount = 2;
        private int GridColsCount1 = 5; //grdMain1
        private int GridColsCount2 = 3; //grdSub1
        private int GridColsCount3 = 4; //grdMain2
        private int GridColsCount4 = 5; //grdSub2
        private int GridColsCount5 = 7; //grdMain3
        private int GridColsCount6 = 5; //grdSub3
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;
        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        //변수
        private bool _first = true;

        //권한관련 add [[
        private string scrAuthInq = ""; //조회 권한
        private string scrAuthReg = ""; //등록(추가)권한
        private string scrAuthMod = ""; //수정 권한
        private string scrAuthDel = ""; //삭제 권한
        //권한관련 add ]]

        public UseLogMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {

            ownerNM = ownerNm;
            titleNM = titleNm;

            //권한관련 add [[
            string[] scrAuthParams = scrAuth.Split(',');

            this.scrAuthInq = scrAuthParams[1];   //조회 권한 저장
            this.scrAuthReg = scrAuthParams[2];   //등록(추가) 권한 저장
            this.scrAuthMod = scrAuthParams[3];   //수정 권한 저장
            this.scrAuthDel = scrAuthParams[4];   //삭제 권한 저장
            //권한관련 add ]]

            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;//윈도우테두리제거방법


            //btnDisplay.Click += Button_Click;
            //btnExcel.Click += Button_Click;

            grdMain1.RowColChange += GrdMain1_RowColChange;
            grdMain2.RowColChange += GrdMain2_RowColChange;

        }

        /// <summary>
        /// 화면 초기화
        /// </summary>
        private void UseLogMgmt_Load(object sender, System.EventArgs e)
        {
            this.BackColor = Color.White;
            //초기화
            InitControl();
            //조회
            //Button_Click(btnDisplay, null);
        }

        /// <summary>
        /// 프로그램 초기화
        /// </summary>
        private void InitControl()
        {
            clsStyle.Style.InitPanel(panel1);
            TabOpt.Indent = 0;


            // 그리드 초기화
            DrawGrid(grdMain1);
            DrawGrid(grdSub1);
            DrawGrid(grdMain2);
            DrawGrid(grdSub2);

            //시간 데이터 default 값 적용 
            start_dt.Value = DateTime.Now.Date;
            end_dt.Value = DateTime.Now.Date;

            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            //그리드 초기 데이터 테이블 생성
            //MakeGridDT();

            TabOpt.SelectedTab = TabOpt1;


            ////리스트 다른 스레드로 처리
            //System.Threading.Thread worker = new System.Threading.Thread(Run);
            //worker.Start();
        }

        private void Run()
        {
            System.Threading.Thread.Sleep(500);

            this.Invoke(new MethodInvoker(
                                             delegate ()
                                             {
                                                 ////시간 데이터 default 값 적용 
                                                 //start_dt.Value = DateTime.Now;
                                                 //end_dt.Value = DateTime.Now;

                                                 //start_dt.ValueChanged += Start_dt_ValueChanged;
                                                 //end_dt.ValueChanged += End_dt_ValueChanged;

                                                 ////그리드 초기 데이터 테이블 생성
                                                 ////MakeGridDT();

                                                 //TabOpt.SelectedTab = TabOpt1;

                                                 ////시스템 구분 가져오기 
                                                 //setCbxSysGP();
                                             }
                                         )
                        );
        }

        private void End_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void Start_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        #region 그리드 초기 데이터 생성 및 그리드 초기화 함수

        private void MakeGridDT()
        {
            MakegrdMain1DT();

            MakegrdSub1DT();

            MakegrdMain2DT();

            MakegrdSub2DT();
        }

        private void MakegrdSub2DT()
        {
            grdSub2_dt = new DataTable();

            grdSub2_dt.Columns.Add(new DataColumn("L_NUM", typeof(string)));
            grdSub2_dt.Columns.Add(new DataColumn("USER_ID", typeof(string)));
            grdSub2_dt.Columns.Add(new DataColumn("USER_NM", typeof(string)));
            grdSub2_dt.Columns.Add(new DataColumn("CNT", typeof(string)));
        }

        private void MakegrdMain2DT()
        {
            grdMain2_dt = new DataTable();

            grdMain2_dt.Columns.Add(new DataColumn("L_NUM", typeof(string)));
            grdMain2_dt.Columns.Add(new DataColumn("SCR_ID", typeof(string)));
            grdMain2_dt.Columns.Add(new DataColumn("SCR_NM", typeof(string)));
            grdMain2_dt.Columns.Add(new DataColumn("CNT", typeof(string)));
        }


        private void MakegrdSub1DT()
        {
            grdSub1_dt = new DataTable();

            grdSub1_dt.Columns.Add(new DataColumn("L_NUM", typeof(string)));
            grdSub1_dt.Columns.Add(new DataColumn("SCR_NM", typeof(string)));
            grdSub1_dt.Columns.Add(new DataColumn("CNT", typeof(string)));
        }

        private void MakegrdMain1DT()
        {
            grdMain1_dt = new DataTable();

            grdMain1_dt.Columns.Add(new DataColumn("L_NUM", typeof(string)));
            grdMain1_dt.Columns.Add(new DataColumn("USER_ID", typeof(string)));
            grdMain1_dt.Columns.Add(new DataColumn("USER_NM", typeof(string)));
            grdMain1_dt.Columns.Add(new DataColumn("CNT", typeof(string)));
        }


        #endregion 그리드 초기 데이터 생성 및 그리드 초기화 함수

        /// <summary>
        /// grdMain1 RowColChange 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdMain1_RowColChange(object sender, EventArgs e)
        {
            if (grdMain1.Rows.Count < GridRowsCount || grdMain1.RowSel < 2)
                return;

            Selected_grdMain1(grdMain1.RowSel);
        }

        /// <summary>
        /// grdMain2 RowColChange 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdMain2_RowColChange(object sender, EventArgs e)
        {

            if (grdMain2.Rows.Count < GridRowsCount || grdMain2.RowSel < 2)
                return;

            Selected_grdMain2(grdMain2.RowSel);
        }


        /// <summary>
        /// 버튼클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    SetDataBinding();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }



        List<C1FlexGrid> gridList1;
        List<C1FlexGrid> gridList2;
        private void SaveExcel()
        {
            gridList1 = new List<C1FlexGrid>();
            gridList1.Add(grdMain1);
            gridList1.Add(grdSub1);
            grdMain1.Tag = "사용자별이력개요";
            grdSub1.Tag = "사용자별이력상세";

            gridList2 = new List<C1FlexGrid>();
            gridList2.Add(grdMain2);
            gridList2.Add(grdSub2);
            grdMain2.Tag = "화면별이력개요";
            grdSub2.Tag = "화면별이력상세";

            C1.Win.C1Command.C1DockingTabPage selectedTab = TabOpt.SelectedTab;

            if (selectedTab.Name == "TabOpt1")
            {
                vf.SaveExcel(titleNM, gridList1);
            }
            else if (selectedTab.Name == "TabOpt2")
            {
                vf.SaveExcel(titleNM, gridList2);
            }

            
        }

        /// <summary>
        /// 그리드 초기화
        /// </summary>
        /// <param name="grdItem"></param>
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            // grdItem.BeginInit();
            try
            {

                if (grdItem.Rows.Count == 1)
                {
                    if (grdItem.Name.ToString() == "grdMain1")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount1, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdSub1")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdMain2")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount3, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdSub2")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount4, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdMain3")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount5, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdSub3")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount6, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                }
                else
                {

                    if (grdItem.Name.ToString() == "grdMain1")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount1, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdSub1")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount2, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdMain2")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount3, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdSub2")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount4, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdMain3")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount5, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                    else if (grdItem.Name.ToString() == "grdSub3")
                    {
                        clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount6, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                    }
                }
                //컬럼 스타일 세팅
                FlexGridCol(grdItem);
                //컬럼 높이 세팅
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdMain1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdSub1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdMain2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdSub2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

                grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                //   grdItem.EndInit();
                //  grdItem.Invalidate();
            }
        }

        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            //grdItem.BeginUpdate();
            try
            {
                //스크롤 세팅
                grdItem.ScrollOptions = ScrollFlags.ScrollByRowColumn;
                grdItem.ScrollBars = ScrollBars.Both;
                //그리드 데이터테이블 바인딩
                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable, true);
                //로우 사이즈
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdMain1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdSub1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdMain2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
                grdSub2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                //     grdItem.EndUpdate();
                //     grdItem.Invalidate();
            }
        }

        /// <summary>
        /// grdMain그리드 컬럼 스타일 적용
        /// </summary>
        /// <param name="grdItem"></param>
        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {

            if (grdItem.Name.ToString() == "grdMain1")
            {
                grdItem.Cols[0].Width = 60;  //no
                grdItem.Cols[1].Width = 180; //사용자ID
                grdItem.Cols[2].Width = 200; //부서
                grdItem.Cols[3].Width = 180; //성명
                grdItem.Cols[4].Width = 120; //로그인 횟수

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "사용자ID";
                grdItem[1, 2] = "부서";
                grdItem[1, 3] = "성명";
                grdItem[1, 4] = "로그인횟수";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 4);
                clsFlexGrid.DataGridLeftStyle(grdItem, 1, 3);

                grdItem.AllowEditing = false;
            }
            else if (grdItem.Name.ToString() == "grdSub1")
            {
                grdItem.Cols[0].Width = 60;
                grdItem.Cols[1].Width = 350;
                grdItem.Cols[2].Width = 120;

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "화면명칭";
                grdItem[1, 2] = "사용횟수";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 2);
                clsFlexGrid.DataGridLeftStyle(grdItem, 1);

                grdItem.AllowEditing = false;
            }
            else if (grdItem.Name.ToString() == "grdMain2")
            {
                grdItem.Cols[0].Width = 60;
                grdItem.Cols[1].Width = 200;
                grdItem.Cols[2].Width = 250;
                grdItem.Cols[3].Width = 120;

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "화면ID";
                grdItem[1, 2] = "화면명칭";
                grdItem[1, 3] = "사용횟수";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 3);
                clsFlexGrid.DataGridLeftStyle(grdItem, 2);

                grdItem.AllowEditing = false;
            }
            else if (grdItem.Name.ToString() == "grdSub2")
            {
                grdItem.Cols[0].Width = 60;  //no
                grdItem.Cols[1].Width = 180; //사용자id
                grdItem.Cols[2].Width = 200; //부서
                grdItem.Cols[3].Width = 180; //성명
                grdItem.Cols[4].Width = 120; //사용횟수

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "사용자ID";
                grdItem[1, 2] = "부서";
                grdItem[1, 3] = "성명";
                grdItem[1, 4] = "사용횟수";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 4);
                clsFlexGrid.DataGridLeftStyle(grdItem, 1, 3);

                grdItem.AllowEditing = false;
            }
            else if (grdItem.Name.ToString() == "grdMain3")
            {
                grdItem.Cols[0].Width = 60;
                grdItem.Cols[1].Width = 80;
                grdItem.Cols[2].Width = 60;
                grdItem.Cols[3].Width = 80;
                grdItem.Cols[4].Width = 150;
                grdItem.Cols[5].Width = 60;
                grdItem.Cols[6].Width = 150;

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "공장";
                grdItem[1, 2] = "공정";
                grdItem[1, 3] = "설비";
                grdItem[1, 4] = "항목명";
                grdItem[1, 5] = "사용횟수";
                grdItem[1, 6] = "TAG명";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
                clsFlexGrid.DataGridLeftStyle(grdItem, 2, 4);
                clsFlexGrid.DataGridRightStyle(grdItem, 5);
                clsFlexGrid.DataGridLeftStyle(grdItem, 6);

                grdItem.AllowEditing = false;
            }
            else if (grdItem.Name.ToString() == "grdSub3")
            {
                grdItem.Cols[0].Width = 60;  //no
                grdItem.Cols[1].Width = 180; //화면명
                grdItem.Cols[2].Width = 200; //부서
                grdItem.Cols[3].Width = 180; //성명
                grdItem.Cols[4].Width = 120; //사용횟수

                grdItem[1, 0] = "NO";
                grdItem[1, 1] = "화면명";
                grdItem[1, 2] = "부서";
                grdItem[1, 3] = "사용자";
                grdItem[1, 4] = "사용횟수";

                clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
                clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
                clsFlexGrid.DataGridCenterStyle(grdItem, 0, 4);
                clsFlexGrid.DataGridLeftStyle(grdItem, 1, 3);

                grdItem.AllowEditing = false;
            }

        }

        /// <summary>
        /// 조회시에 사용
        /// </summary>
        /// <returns></returns>
        private void SetDataBinding()
        {
            //활성화된 텝의 그리드를 조회한다..

            C1FlexGrid selectedGrd = SelectedGrd_InTab();

            if (selectedGrd.Name == "grdMain1")
            {
                SetDataBinding_grdMain1();

            }
            else if (selectedGrd.Name == "grdMain2")
            {
                SetDataBinding_grdMain2();

            }
            return;

        }

        /// <summary>
        /// 탭 선택
        /// </summary>
        private C1FlexGrid SelectedGrd_InTab()
        {
            C1FlexGrid selectedGrd = null;

            C1.Win.C1Command.C1DockingTabPage selectedTab = TabOpt.SelectedTab;

            string grd_Nm = "";
            if (selectedTab.Name == "TabOpt1")
            {
                grd_Nm = "grdMain1";
            }
            else if (selectedTab.Name == "TabOpt2")
            {
                grd_Nm = "grdMain2";
            }
            else if (selectedTab.Name == "TabOpt3")
            {
                grd_Nm = "grdMain3";
            }
            else
            {
                grd_Nm = "grdMain1";
            }

            return selectedGrd = (C1FlexGrid)selectedTab.Controls.Find(grd_Nm, true)[0];
        }
        /// <summary>
        /// grdMain1 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDataBinding_grdMain1()
        {
            // main1 main2 데이터를 시간에 의해 검색해서 데이터 입력
            // sub1 sub2 데이터가 있을때 데이터 입력
            try
            {
                string sql = "";
                sql += string.Format(" SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY A.USER_ID ASC))      AS L_NUM                                                                ");
                sql += string.Format("       ,A.USER_ID                                                                                                                                 ");
                sql += string.Format("       ,(SELECT (SELECT CD_NM FROM TB_CM_COM_CD WHERE CD_ID = DEPT_CD) AS DEPT_NM FROM TB_CM_USER WHERE USER_ID = A.USER_ID)        AS DEPT_NM    ");
                sql += string.Format("       ,(SELECT NM from TB_CM_USER WHERE USER_ID = A.USER_ID)        AS USER_NM                                                                   ");
                sql += string.Format("       ,COUNT(*) AS CNT                                                                                                                           ");
                sql += string.Format("   FROM TB_CM_LOGINHIS A                                                                                                                          ");
                sql += string.Format(" WHERE CONVERT(varchar(10),A.LOGIN_DDTT, 120) BETWEEN '" + vf.Format(start_dt.Value, "yyyy-MM-dd") + "' AND '" + vf.Format(end_dt.Value, "yyyy-MM-dd") + "'");
                sql += string.Format("   GROUP BY A.USER_ID                                                                                                                             ");

                // datatable 입력후에 grid 설정해주어야함...
                olddt_grdMain1 = cd.FindDataTable(sql);
                //if (_first == false)
                //{
                //    //if (olddt_grdMain1 == null || olddt_grdMain1.Rows.Count == 0)
                //    //{
                //    //    MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    //    start_dt.Focus();
                //    //}
                //}

                _first = false;

                moddt_grdMain1 = olddt_grdMain1.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //grdMain1.SetDataBinding(moddt_grdMain1, null, true);
                DrawGrid(grdMain1, moddt_grdMain1);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                grdMain1.ExtendLastCol = true;
                grdMain1.Styles.EmptyArea.BackColor = Color.White;

                if (grdMain1.Rows.Count > GridRowsCount)
                {
                    Selected_grdMain1(GridRowsCount);
                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        /// <summary>
        /// grdMain2 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDataBinding_grdMain2()
        {
            try
            {
                string Sql1 = string.Empty;

                Sql1 += string.Format("/*2018.07.06 화면별 사용횟수Main 조회 */                                                                                                          ");
                Sql1 += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY A.SCR_ID ASC)) AS L_NUM                                                                                ");
                Sql1 += string.Format("      ,A.SCR_ID                                                                                                                                            ");
                Sql1 += string.Format("	     ,(SELECT SCR_NM FROM TB_CM_SCR WHERE SCR_ID = A.SCR_ID) AS SCR_NM                                                                                    ");
                Sql1 += string.Format("      ,COUNT(*) AS CNT                                                                                                                                     ");
                Sql1 += string.Format("  FROM TB_CM_SCR_USEHIS A                                                                                                                                  ");
                Sql1 += string.Format(" WHERE CONVERT(varchar(10),A.USE_DDTT, 120) BETWEEN '" + vf.Format(start_dt.Value, "yyyy-MM-dd") + "' AND '" + vf.Format(end_dt.Value, "yyyy-MM-dd") + "'  ");
                Sql1 += string.Format(" GROUP BY A.SCR_ID                                                                                                                                         ");
                // datatable 입력후에 grid 설정해주어야함...
                olddt_grdMain2 = cd.FindDataTable(Sql1);
                //if (_first == false)
                //{
                //    if (olddt_grdMain2 == null || olddt_grdMain2.Rows.Count == 0)
                //    {
                //        MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        start_dt.Focus();
                //    }
                //}

                //_first = false;

                moddt_grdMain2 = olddt_grdMain2.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //grdMain2.SetDataBinding(moddt_grdMain2, null, true);
                DrawGrid(grdMain2, moddt_grdMain2);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                grdMain2.ExtendLastCol = true;
                grdMain2.Styles.EmptyArea.BackColor = Color.White;
                if (grdMain2.Rows.Count > GridRowsCount)
                {
                    Selected_grdMain2(GridRowsCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;

            }

            return;
        }

        /// <summary>
        /// grdMain3 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDataBinding_grdMain3()
        {
            try
            {
                string Sql1 = string.Empty;

                Sql1 += string.Format("                                                                     ");
                Sql1 += string.Format("SELECT ROW_NUMBER() OVER( ORDER BY TAG_USE_CNT DESC, FACTORY_NM, ROUTING_NM, EQP_NM, ITEM_NM ) AS L_NUM                   ");
                Sql1 += string.Format("      ,FACTORY_NM                                                                                                         ");
                Sql1 += string.Format("      ,ROUTING_NM                                                                                                         ");
                Sql1 += string.Format("      ,EQP_NM                                                                                                             ");
                Sql1 += string.Format("      ,ITEM_NM                                                                                                            ");
                Sql1 += string.Format("      ,TAG_USE_CNT                                                                                                        ");
                Sql1 += string.Format("	  ,TAGNAME                                                                                                               ");
                Sql1 += string.Format("FROM  (                                                                                                                   ");
                Sql1 += string.Format("SELECT (SELECT CD_NM  FROM TB_CM_COM_CD WHERE CATEGORY = 'FACTORY' AND CD_ID = A.FACTORY) AS FACTORY_NM                   ");
                Sql1 += string.Format("      ,(SELECT ROUTING_NM FROM TB_ROUTING_CD WHERE FACTORY = A.FACTORY AND ROUTING_CD = A.ROUTING_CD ) ROUTING_NM         ");
                Sql1 += string.Format("      ,A.EQP_NM                                                                                                           ");
                Sql1 += string.Format("      ,A.ITEM_NM                                                                                                          ");
                Sql1 += string.Format("      ,A.TAGNAME                                                                                                          ");
                Sql1 += string.Format("      ,B.TAG_USE_CNT                                                                                                      ");
                Sql1 += string.Format("FROM   TB_TAG_INFO A                                                                                                      ");
                Sql1 += string.Format("      ,(SELECT TAGNAME                                                                                                    ");
                Sql1 += string.Format("              ,COUNT(*) AS TAG_USE_CNT                                                                                    ");
                Sql1 += string.Format("        FROM   dbo.TB_CM_TAG_USEHIS                                                                                       ");
                Sql1 += string.Format("        WHERE  CONVERT(VARCHAR(10), USE_DDTT,21) >= '" + vf.Format(start_dt.Value, "yyyy-MM-dd") + "'                      ");
                Sql1 += string.Format("        AND    CONVERT(VARCHAR(10), USE_DDTT,21) <= '" + vf.Format(end_dt.Value, "yyyy-MM-dd") + "'                        ");
                Sql1 += string.Format("        GROUP BY TAGNAME                                                                                                  ");
                Sql1 += string.Format("        ) B                                                                                                               ");
                Sql1 += string.Format("WHERE  A.TAGNAME  = B.TAGNAME                                                                                             ");
                Sql1 += string.Format(") X                                                                                                                       ");
                Sql1 += string.Format("ORDER BY TAG_USE_CNT DESC, FACTORY_NM, ROUTING_NM, EQP_NM, ITEM_NM                                                        ");

                olddt_grdMain3 = cd.FindDataTable(Sql1);
                if (_first == false)
                {
                    if (olddt_grdMain3 == null || olddt_grdMain3.Rows.Count == 0)
                    {
                        MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        start_dt.Focus();
                    }
                }

                _first = false;

                moddt_grdMain3 = olddt_grdMain3.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                //grdMain2.SetDataBinding(moddt_grdMain2, null, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;

            }

            return;
        }

        /// <summary>
        /// grdSub1조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selected_grdMain1(int SelectedRow)
        {
            string selected_user_id = grdMain1.Rows[SelectedRow]["USER_ID"].ToString();

            string Sql1 = string.Empty;

            Sql1 += string.Format("/*2018.07.06 사용자별 화면이용횟수 Detail 조회 */                                                                                                 ");
            Sql1 += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY A.SCR_ID ASC)) AS L_NUM                                                                                ");
            Sql1 += string.Format("      ,(SELECT SCR_NM FROM TB_CM_SCR WHERE SCR_ID = A.SCR_ID) AS SCR_NM                                                                                    ");
            Sql1 += string.Format("      ,COUNT(*) AS CNT                                                                                                                                     ");
            Sql1 += string.Format("  FROM TB_CM_SCR_USEHIS A                                                                                                                                  ");
            Sql1 += string.Format(" WHERE CONVERT(varchar(10),A.USE_DDTT, 120) BETWEEN '" + vf.Format(start_dt.Value, "yyyy-MM-dd") + "' AND '" + vf.Format(end_dt.Value, "yyyy-MM-dd") + "'   ");
            Sql1 += string.Format("   AND USER_ID = '" + selected_user_id + "'                                                                                                                 ");
            Sql1 += string.Format(" GROUP BY A.SCR_ID                                                                                                                                         ");
            // datatable 입력후에 grid 설정해주어야함...
            olddt_grdSub1 = cd.FindDataTable(Sql1);

            moddt_grdSub1 = olddt_grdSub1.Copy();

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            //grdSub1.SetDataBinding(moddt_grdSub1, null, true);
            DrawGrid(grdSub1, moddt_grdSub1);
            //마지막행 사이즈조절, 로우공백흰색
            grdSub1.ExtendLastCol = true;
            grdSub1.Styles.EmptyArea.BackColor = Color.White;
            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"{Text} ({TabOpt1.Text}): {moddt_grdSub1.Rows.Count} 건 조회 되었습니다.");
        }


        /// <summary>
        /// grdSub2조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selected_grdMain2(int SelectedRow)
        {
            string scr_id = grdMain2.Rows[grdMain2.RowSel]["SCR_ID"].ToString();

            string Sql1 = string.Empty;


            Sql1 += string.Format("/*2018.07.06 화면별 사용횟수 Detail 사용횟수 조회 */                                                                                                 ");
            Sql1 += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY A.USER_ID ASC)) AS L_NUM                                                                               ");
            Sql1 += string.Format("      ,A.USER_ID                                                                                                                                           ");
            Sql1 += string.Format("      ,(SELECT (SELECT CD_NM FROM TB_CM_COM_CD WHERE CD_ID = DEPT_CD) AS DEPT_NM FROM TB_CM_USER WHERE USER_ID = A.USER_ID)        AS DEPT_NM                                                               ");
            Sql1 += string.Format("	     ,(SELECT NM FROM TB_CM_USER WHERE USER_ID = A.USER_ID) AS USER_NM                                                                               ");
            Sql1 += string.Format("	     ,COUNT(*) AS CNT                                                                                                                                     ");
            Sql1 += string.Format("  FROM TB_CM_SCR_USEHIS A                                                                                                                                  ");
            Sql1 += string.Format(" WHERE CONVERT(varchar(10),A.USE_DDTT, 120) BETWEEN '" + vf.Format(start_dt.Value, "yyyy-MM-dd") + "' AND '" + vf.Format(end_dt.Value, "yyyy-MM-dd") + "'  ");
            Sql1 += string.Format("   AND A.SCR_ID = '" + scr_id + "'                                                                                                                          ");
            Sql1 += string.Format(" GROUP BY A.USER_ID, A.SCR_ID                                                                                                                              ");

            // datatable 입력후에 grid 설정해주어야함...
            olddt_grdSub2 = cd.FindDataTable(Sql1);
            moddt_grdSub2 = olddt_grdSub2.Copy();

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            //grdSub2.SetDataBinding(moddt_grdSub2, null, true);
            DrawGrid(grdSub2, moddt_grdSub2);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            //마지막행 사이즈조절, 로우공백흰색
            grdSub2.ExtendLastCol = true;
            grdSub2.Styles.EmptyArea.BackColor = Color.White;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), $"  {Text} ({TabOpt2.Text}): {moddt_grdSub2.Rows.Count} 건 조회 되었습니다.");


        }


        /// <summary>
        /// 탭 체인지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            Button_Click(btnDisplay, null);
            //SetDataBinding();
        }

        /// <summary>
        /// FROM 조회 날짜 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_dt_ValueChanged(object sender, EventArgs e)
        {

            if (DateTime.Compare(start_dt.Value, end_dt.Value) > 0)
            {
                start_dt.Value = end_dt.Value;
            }

            if (CanDateChange)
            {
                Button_Click(btnDisplay, null);
            }
        }

        /// <summary>
        /// TO 조회 날짜 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void end_dt_ValueChanged(object sender, EventArgs e)
        {

            if (DateTime.Compare(start_dt.Value, end_dt.Value) > 0)
            {
                end_dt.Value = start_dt.Value;
            }

            if (CanDateChange)
            {
                Button_Click(btnDisplay, null);
            }
        }


    }
}
