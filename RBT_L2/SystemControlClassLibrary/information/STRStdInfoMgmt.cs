﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SystemControlClassLibrary.information
{
    public partial class STRStdInfoMgmt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        

        DataTable olddt_1;
        DataTable moddt_1;

        DataTable olddt_2;
        DataTable moddt_2;

        DataTable olddt_3;
        DataTable moddt_3;

        ListDictionary steel_Nm_ld = null;

        List<string> msg_1;
        List<string> modifList_1;

        string set_value = string.Empty;
        string grd_Nm = "grdMain1";

        clsStyle cs = new clsStyle();

        private static string steel_grp_Nm = "";

        private static string item_size_Nm = "";

        // 셀의 수정전 값
        private string strBefValue1 = "";
        private string strBefValue2 = "";
        private string strBefValue3 = "";

        private bool _CanSaveSearchLog = false;


        private static string ownerNM = "";
        private static string titleNM = "";


        #endregion 변수 설정

        #region 생성자 및 로딩 설정
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }
        public STRStdInfoMgmt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            this.BackColor = Color.White;

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void STRStdinfoMgmt_Load(object sender, EventArgs e)
        {
            InitControl();
            _CanSaveSearchLog = true;
            btnDisplay_Click(null, null);
        }
        #endregion 생성자 및 로딩 설정

        #region 컨트롤 설정
        private void InitControl()
        {
            cs.InitPicture(pictureBox1);
            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(steel_grp_lb);
            cs.InitLabel(item_size_lb);

            cs.InitCombo(steel_grp_cb, StringAlignment.Near);

            cs.InitTextBox(item_size_tb);

            item_size_tb.MaxLength = 4;

            cs.InitButton(btnExcel);
            cs.InitButton(btnSave);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            cs.InitButton(btnRowAdd);
            cs.InitButton(btnDelRow);

            cs.InitTab(TabOpt);

            SetComboBox();

            SetListDictionary();

            TabOpt.Indent = 0;

            InitGrd_Main1();

            InitGrd_Main2();

            InitGrd_Main3();

            TabOpt.SelectedTab = line_1_tp;
        }
        #endregion 컨트롤 설정

        #region 그리드 설정

        #region 3번 그리드 설정
        private void InitGrd_Main3()
        {
            clsStyle.Style.InitGrid_search(grdMain3);

            var crCellRange = grdMain3.GetCellRange(0, grdMain3.Cols["TOP_ROLL_POS"].Index, 0, grdMain3.Cols["BOT_ROLL_MTR_HZ"].Index);
            crCellRange.Style = grdMain3.Styles["ModifyStyle"];

            grdMain3.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            grdMain3.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain3.Cols["STEEL_GRP"].Width = 90;
            grdMain3.Cols["ITEM_SIZE_MIN"].Width = 120;
            grdMain3.Cols["ITEM_SIZE_MAX"].Width = 120;
            grdMain3.Cols["TOP_ROLL_POS"].Width = 100;
            grdMain3.Cols["TOP_ROLL_ANGLE"].Width = 100;
            grdMain3.Cols["BOT_ROLL_ANGLE"].Width = 100;
            grdMain3.Cols["UP_DOWN_LINEAR_POS"].Width = 150;
            grdMain3.Cols["FRONT_LINEAR_GUIDE_POS"].Width = 160;
            grdMain3.Cols["REAL_LINEAR_GUIDE_POS"].Width = 160;
            grdMain3.Cols["TOP_ROLL_MTR_HZ"].Width = 150;
            grdMain3.Cols["BOT_ROLL_MTR_HZ"].Width = 160;
            grdMain3.Cols["GUBUN"].Width = 0;

            #region 1. grdMain3 head 및 row  align 설정
            grdMain3.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain3.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain3.Cols["STEEL_GRP"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain3.Cols["ITEM_SIZE_MIN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain3.Cols["ITEM_SIZE_MAX"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain3.Cols["TOP_ROLL_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["TOP_ROLL_ANGLE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["BOT_ROLL_ANGLE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["UP_DOWN_LINEAR_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["FRONT_LINEAR_GUIDE_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["REAL_LINEAR_GUIDE_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["TOP_ROLL_MTR_HZ"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["BOT_ROLL_MTR_HZ"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            #endregion

            grdMain3.Cols["STEEL_GRP"].DataMap = steel_Nm_ld;
            grdMain3.Cols["STEEL_GRP"].TextAlign = TextAlignEnum.LeftCenter;

            grdMain3.AutoResize = true;
        }
        #endregion 3번 그리드 설정

        #region 2번 그리드 설정
        private void InitGrd_Main2()
        {
            clsStyle.Style.InitGrid_search(grdMain2);

            var crCellRange = grdMain2.GetCellRange(0, grdMain2.Cols["TOP_ROLL_POS"].Index, 0, grdMain2.Cols["BOT_ROLL_MTR_HZ"].Index);
            crCellRange.Style = grdMain2.Styles["ModifyStyle"];

            grdMain2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            grdMain2.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain2.Cols["STEEL_GRP"].Width = 90;
            grdMain2.Cols["ITEM_SIZE_MIN"].Width = 120;
            grdMain2.Cols["ITEM_SIZE_MAX"].Width = 120;
            grdMain2.Cols["TOP_ROLL_POS"].Width = 100;
            grdMain2.Cols["TOP_ROLL_ANGLE"].Width = 100;
            grdMain2.Cols["BOT_ROLL_ANGLE"].Width = 100;
            grdMain2.Cols["UP_DOWN_LINEAR_POS"].Width = 150;
            grdMain2.Cols["FRONT_LINEAR_GUIDE_POS"].Width = 160;
            grdMain2.Cols["REAL_LINEAR_GUIDE_POS"].Width = 160;
            grdMain2.Cols["TOP_ROLL_MTR_HZ"].Width = 150;
            grdMain2.Cols["BOT_ROLL_MTR_HZ"].Width = 160;
            grdMain2.Cols["GUBUN"].Width = 0;

            #region 1. grdMain2 head 및 row  align 설정
            grdMain2.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain2.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain2.Cols["STEEL_GRP"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain2.Cols["ITEM_SIZE_MIN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain2.Cols["ITEM_SIZE_MAX"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain2.Cols["TOP_ROLL_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["TOP_ROLL_ANGLE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["BOT_ROLL_ANGLE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["UP_DOWN_LINEAR_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["FRONT_LINEAR_GUIDE_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["REAL_LINEAR_GUIDE_POS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["TOP_ROLL_MTR_HZ"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["BOT_ROLL_MTR_HZ"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            #endregion

            grdMain2.Cols["STEEL_GRP"].DataMap = steel_Nm_ld;
            grdMain2.Cols["STEEL_GRP"].TextAlign = TextAlignEnum.LeftCenter;

            grdMain2.AutoResize = true;
        }
        #endregion 2번 그리드 설정

        #region 1번 그리드 설정
        private void InitGrd_Main1()
        {
            clsStyle.Style.InitGrid_search(grdMain1);

            var crCellRange = grdMain1.GetCellRange(0, grdMain1.Cols["TOP_ROLL_GAP_1"].Index, 0, grdMain1.Cols["OUTLET_ROLL_CURRENT"].Index);
            crCellRange.Style = grdMain1.Styles["ModifyStyle"];

            grdMain1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            grdMain1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain1.Cols["STEEL_GRP"].Width = 90;
            grdMain1.Cols["ITEM_SIZE_MIN"].Width = 120;
            grdMain1.Cols["ITEM_SIZE_MAX"].Width = 120;
            grdMain1.Cols["TOP_ROLL_GAP_1"].Width = 100;
            grdMain1.Cols["TOP_ROLL_GAP_2"].Width = 100;
            grdMain1.Cols["TOP_ROLL_GAP_3"].Width = 100;
            grdMain1.Cols["TOP_ROLL_GAP_4"].Width = 100;
            grdMain1.Cols["TOP_ROLL_GAP_5"].Width = 100;
            grdMain1.Cols["TOP_ROLL_GAP_6"].Width = 100;
            grdMain1.Cols["TOP_ROLL_ANGLE_1"].Width = 100;
            grdMain1.Cols["TOP_ROLL_ANGLE_2"].Width = 100;
            grdMain1.Cols["TOP_ROLL_ANGLE_3"].Width = 100;
            grdMain1.Cols["TOP_ROLL_ANGLE_4"].Width = 100;
            grdMain1.Cols["TOP_ROLL_ANGLE_5"].Width = 100;
            grdMain1.Cols["TOP_ROLL_ANGLE_6"].Width = 100;
            grdMain1.Cols["BOT_ROLL_ANGLE_1"].Width = 100;
            grdMain1.Cols["BOT_ROLL_ANGLE_2"].Width = 100;
            grdMain1.Cols["BOT_ROLL_ANGLE_3"].Width = 100;
            grdMain1.Cols["THREAD_SPEED"].Width = 150;
            grdMain1.Cols["MACHINE_SPEED"].Width = 150;
            grdMain1.Cols["INLET_ROLL_CURRENT"].Width = 140;
            grdMain1.Cols["MID_ROLL_CURRENT"].Width = 130;
            grdMain1.Cols["OUTLET_ROLL_CURRENT"].Width = 150;
            grdMain1.Cols["GUBUN"].Width = 0;

            #region 1. grdMain11 head 및 row  align 설정
            grdMain1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain1.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain1.Cols["STEEL_GRP"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain1.Cols["ITEM_SIZE_MIN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain1.Cols["ITEM_SIZE_MAX"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain1.Cols["TOP_ROLL_GAP_1"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_GAP_2"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_GAP_3"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_GAP_4"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_GAP_5"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_GAP_6"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_ANGLE_1"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_ANGLE_2"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_ANGLE_3"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_ANGLE_4"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_ANGLE_5"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["TOP_ROLL_ANGLE_6"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["BOT_ROLL_ANGLE_1"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["BOT_ROLL_ANGLE_2"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["BOT_ROLL_ANGLE_3"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["THREAD_SPEED"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["MACHINE_SPEED"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["INLET_ROLL_CURRENT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["MID_ROLL_CURRENT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["OUTLET_ROLL_CURRENT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["GUBUN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            #endregion

            grdMain1.Cols["STEEL_GRP"].DataMap = steel_Nm_ld;
            grdMain1.Cols["STEEL_GRP"].TextAlign = TextAlignEnum.LeftCenter;

            grdMain1.AutoResize = true;

        }
        #endregion 1번 그리드 설정

        #endregion 그리드 설정

        #region 조회 설정

        #region SetDataBinding 분기 설정
        private bool SetDataBinding()
        {
            //활성화된 텝의 그리드를 조회한다..

            C1FlexGrid selectedGrd = SelectedGrd_InTab();

            if (selectedGrd.Name == "grdMain1")
            {
                SetDataBinding_1();
            }
            else if (selectedGrd.Name == "grdMain2")
            {
                SetDataBinding_2();
            }
            else if (selectedGrd.Name == "grdMain3")
            {
                SetDataBinding_3();
            }

            return true;
        }
        #endregion SetDataBinding 분기 설정

        #region SetDataBinding(3번 그리드 조회) 설정
        private bool SetDataBinding_3()
        {
            string strQry = string.Empty;

            try
            {
                string sql1 = string.Format(" SELECT TO_CHAR(ROWNUM) AS L_NO ");
                sql1 += string.Format(" ,STEEL_GRP ");
                sql1 += string.Format(" ,ITEM_SIZE_MIN ");
                sql1 += string.Format(" ,ITEM_SIZE_MAX ");
                sql1 += string.Format(" ,TOP_ROLL_POS ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE    ");
                sql1 += string.Format(" ,BOT_ROLL_ANGLE    ");
                sql1 += string.Format(" ,UP_DOWN_LINEAR_POS    ");
                sql1 += string.Format(" ,FRONT_LINEAR_GUIDE_POS    ");
                sql1 += string.Format(" ,REAL_LINEAR_GUIDE_POS    ");
                sql1 += string.Format(" ,TOP_ROLL_MTR_HZ  ");
                sql1 += string.Format(" ,BOT_ROLL_MTR_HZ  ");
                sql1 += string.Format("    ,'' AS GUBUN ");
                sql1 += string.Format(" from TB_STR_STDINFO_NO3 ");
                sql1 += string.Format(" WHERE STEEL_GRP LIKE '{0}' || '%'  ", steel_grp_Nm);
                sql1 += string.Format(" AND TO_NUMBER(ITEM_SIZE_MIN) <= TO_NUMBER(NVL('{0}','9999')) ", item_size_Nm);
                sql1 += string.Format(" AND TO_NUMBER(ITEM_SIZE_MAX) >= TO_NUMBER(NVL('{0}','0000')) ", item_size_Nm);
                sql1 += string.Format(" ORDER BY STEEL_GRP,ITEM_SIZE_MIN,ITEM_SIZE_MAX ");
                olddt_3 = cd.FindDataTable(sql1);
               
                moddt_3 = olddt_3.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain3.SetDataBinding(moddt_3, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt_3.Rows.Count.ToString() + "건이 조회 되었습니다.");

                grdMain3.Row = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            return true;
        }
        #endregion SetDataBinding(3번 그리드 조회) 설정

        #region SetDataBinding(2번 그리드 조회) 설정
        private bool SetDataBinding_2()
        {
            string strQry = string.Empty;

            try
            {
                string sql1 = string.Format(" SELECT TO_CHAR(ROWNUM) AS L_NO ");
                sql1 += string.Format(" ,STEEL_GRP ");
                sql1 += string.Format(" ,ITEM_SIZE_MIN ");
                sql1 += string.Format(" ,ITEM_SIZE_MAX ");
                sql1 += string.Format(" ,TOP_ROLL_POS ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE    ");
                sql1 += string.Format(" ,BOT_ROLL_ANGLE    ");
                sql1 += string.Format(" ,UP_DOWN_LINEAR_POS    ");
                sql1 += string.Format(" ,FRONT_LINEAR_GUIDE_POS    ");
                sql1 += string.Format(" ,REAL_LINEAR_GUIDE_POS    ");
                sql1 += string.Format(" ,TOP_ROLL_MTR_HZ  ");
                sql1 += string.Format(" ,BOT_ROLL_MTR_HZ  ");
                sql1 += string.Format("    ,'' AS GUBUN ");
                sql1 += string.Format(" from TB_STR_STDINFO_NO2 ");
                sql1 += string.Format(" WHERE STEEL_GRP LIKE '{0}' || '%'  ", steel_grp_Nm);
                sql1 += string.Format(" AND TO_NUMBER(ITEM_SIZE_MIN) <= TO_NUMBER(NVL('{0}','9999')) ", item_size_Nm);
                sql1 += string.Format(" AND TO_NUMBER(ITEM_SIZE_MAX) >= TO_NUMBER(NVL('{0}','0000')) ", item_size_Nm);
                sql1 += string.Format(" ORDER BY STEEL_GRP,ITEM_SIZE_MIN,ITEM_SIZE_MAX ");

                olddt_2 = cd.FindDataTable(sql1);

                moddt_2 = olddt_2.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain2.SetDataBinding(moddt_2, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt_2.Rows.Count.ToString() + "건이 조회 되었습니다.");

                grdMain2.Row = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            return true;
        }
        #endregion SetDataBinding(2번 그리드 조회) 설정

        #region SetDataBinding(1번 그리드 조회) 설정
        private bool SetDataBinding_1()
        {
            string strQry = string.Empty;

            bool isAnd = false;
            
            try
            {
                string sql1 = string.Format(" SELECT TO_CHAR(ROWNUM) AS L_NO ");
                sql1 += string.Format(" ,STEEL_GRP ");
                sql1 += string.Format(" ,ITEM_SIZE_MIN ");
                sql1 += string.Format(" ,ITEM_SIZE_MAX ");
                sql1 += string.Format(" ,TOP_ROLL_GAP_1 ");
                sql1 += string.Format(" ,TOP_ROLL_GAP_2    ");
                sql1 += string.Format(" ,TOP_ROLL_GAP_3    ");
                sql1 += string.Format(" ,TOP_ROLL_GAP_4    ");
                sql1 += string.Format(" ,TOP_ROLL_GAP_5    ");
                sql1 += string.Format(" ,TOP_ROLL_GAP_6    ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE_1  ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE_2  ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE_3  ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE_4  ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE_5   ");
                sql1 += string.Format(" ,TOP_ROLL_ANGLE_6  ");
                sql1 += string.Format(" ,BOT_ROLL_ANGLE_1  ");
                sql1 += string.Format(" ,BOT_ROLL_ANGLE_2  ");
                sql1 += string.Format(" ,BOT_ROLL_ANGLE_3  ");
                sql1 += string.Format(" ,THREAD_SPEED      ");
                sql1 += string.Format(" ,MACHINE_SPEED     ");
                sql1 += string.Format(" ,INLET_ROLL_CURRENT     ");
                sql1 += string.Format(" ,MID_ROLL_CURRENT     ");
                sql1 += string.Format(" ,OUTLET_ROLL_CURRENT     ");
                sql1 += string.Format("    ,'' AS GUBUN ");
                sql1 += string.Format(" from TB_STR_STDINFO_NO1 ");
                sql1 += string.Format(" WHERE STEEL_GRP LIKE '{0}' || '%'  ", steel_grp_Nm);
                sql1 += string.Format(" AND TO_NUMBER(ITEM_SIZE_MIN) <= TO_NUMBER(NVL('{0}','9999')) ", item_size_Nm);
                sql1 += string.Format(" AND TO_NUMBER(ITEM_SIZE_MAX) >= TO_NUMBER(NVL('{0}','0000')) ", item_size_Nm);
                sql1 += string.Format(" ORDER BY STEEL_GRP,ITEM_SIZE_MIN,ITEM_SIZE_MAX ");

                olddt_1 = cd.FindDataTable(sql1);

                moddt_1 = olddt_1.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain1.SetDataBinding(moddt_1, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt_1.Rows.Count.ToString() + "건이 조회 되었습니다.");

                grdMain1.Row = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }
            
            return true;
        }
        #endregion SetDataBinding(1번 그리드 조회) 설정

        #endregion 조회 설정

        #region 저장 설정
        private void save_btn_Click(object sender, EventArgs e)
        {
            #region 삭제항목체크
            string check_value1 = string.Empty;
            string check_value2 = string.Empty;
            string check_value3 = string.Empty;
            string check_Cols_NM1 = string.Empty;
            string check_Cols_NM2 = string.Empty;
            string check_Cols_NM3 = string.Empty;

            string check_field_NM1 = string.Empty;
            string check_field_NM2 = string.Empty;
            string check_field_NM3 = string.Empty;
            string check_table_NM = string.Empty;

            string check_keyColNM = string.Empty;
            string check_keyValue = string.Empty;

            string check_NM = string.Empty;

            string gubun_value = string.Empty;
            string show_msg = string.Empty;
            int checkrow = 0;

            bool isChange = false;
            
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            if (grd_Nm == "grdMain1")
            {
                #region grdMain1 체크

            for (checkrow = 1; checkrow < grdMain1.Rows.Count; checkrow++)
            {
                gubun_value = grdMain1.GetData(checkrow, "GUBUN").ToString();

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }

                if (gubun_value == "추가")
                {
                    #region "STEEL_GRP" 체크
                    check_field_NM1 = "STEEL_GRP";
                    check_table_NM = "TB_STR_STDINFO_NO1";
                    check_value1 = grdMain1.GetData(checkrow, check_field_NM1).ToString();
                    check_Cols_NM1 = grdMain1.Cols[check_field_NM1].Caption;

                    if (string.IsNullOrEmpty(check_value1))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                        MessageBox.Show(show_msg);
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    if (vf.isContainHangul(check_value1))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    if (vf.getStrLen(check_value1) > 30)
                    {
                        MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    // 명이입력되지 않은경우 체크
                    check_field_NM1 = "STEEL_GRP";
                    check_NM = grdMain1.GetData(checkrow, check_field_NM1).ToString();
                    check_Cols_NM1 = grdMain1.Cols[check_field_NM1].Caption;

                    if (string.IsNullOrEmpty(check_NM))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                        MessageBox.Show(show_msg);
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }
                    isChange = true;

                    #region "ITEM_SIZE_MIN" 체크
                    check_field_NM2 = "ITEM_SIZE_MIN";
                    check_table_NM = "TB_STR_STDINFO_NO1";
                    check_value2 = grdMain1.GetData(checkrow, check_field_NM2).ToString();
                    check_Cols_NM2 = grdMain1.Cols[check_field_NM2].Caption;

                    if (string.IsNullOrEmpty(check_value2))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                        MessageBox.Show(show_msg);
                        grdMain1.Select(checkrow, 1, true);
                        return;
                    }

                    if (vf.isContainHangul(check_value2))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    if (vf.getStrLen(check_value2) > 4)
                    {
                        MessageBox.Show("숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    #endregion "ITEM_SIZE_MIN" 체크

                    #region "ITEM_SIZE_MAX" 체크
                    check_field_NM3 = "ITEM_SIZE_MAX";
                    check_table_NM = "TB_STR_STDINFO_NO1";
                    check_value3 = grdMain1.GetData(checkrow, check_field_NM3).ToString();
                    check_Cols_NM3 = grdMain1.Cols[check_field_NM3].Caption;

                    if (string.IsNullOrEmpty(check_value3))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM3);
                            grdMain1.Select(checkrow, 1, true);
                            MessageBox.Show(show_msg);
                        return;
                    }

                    if (vf.isContainHangul(check_value3))
                    {
                        MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    if (vf.getStrLen(check_value3) > 4)
                    {
                        MessageBox.Show(" 숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    if (vf.Has_Item(check_table_NM, check_field_NM1, check_field_NM2, check_field_NM3, check_value1, check_value2, check_value3))
                    {
                        MessageBox.Show("필수요소가 중복되었습니다.");
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }
                        #endregion "ITEM_SIZE_MAX" 체크

                    // 추가로 연속 입력시에 추가 값
                    if (vf.IsSameRangeInGrd(grdMain1, check_NM, check_value2, check_value3, checkrow))
                    {
                            return;
                    }

                    // 크기비교
                    if (vf.CInt(check_value2) > vf.CInt(check_value3))
                    {
                        string message = string.Format("{0}의 값이 {1}값보다 큽니다.", check_Cols_NM2, check_Cols_NM3);
                        MessageBox.Show(message);
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }

                    if (vf.IsSameRange(check_table_NM, check_NM, check_value2, check_value3))
                    {
                        string message = string.Format("중복하는 범위가 이미 존재합니다.");
                        MessageBox.Show(message);
                            grdMain1.Select(checkrow, 1, true);
                            return;
                    }
                 }
                 #endregion "STEEL_GRP" 체크
                    
                }
                #endregion grdMain1 체크
            }
            if (grd_Nm == "grdMain2")
            {
                #region grdMain2 체크

                for (checkrow = 1; checkrow < grdMain2.Rows.Count; checkrow++)
                {
                    gubun_value = grdMain2.GetData(checkrow, "GUBUN").ToString();

                    if (gubun_value == "삭제" || gubun_value == "수정")
                    {
                        isChange = true;
                    }

                    if (gubun_value == "추가")
                    {
                        #region "STEEL_GRP" 체크
                        check_field_NM1 = "STEEL_GRP";
                        check_table_NM = "TB_STR_STDINFO_NO2";
                        check_value1 = grdMain2.GetData(checkrow, check_field_NM1).ToString();
                        check_Cols_NM1 = grdMain2.Cols[check_field_NM1].Caption;

                        if (string.IsNullOrEmpty(check_value1))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                            MessageBox.Show(show_msg);
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.isContainHangul(check_value1))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value1) > 30)
                        {
                            MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                            return;
                        }

                        // 명이입력되지 않은경우 체크
                        check_field_NM1 = "STEEL_GRP";
                        check_NM = grdMain2.GetData(checkrow, check_field_NM1).ToString();
                        check_Cols_NM1 = grdMain2.Cols[check_field_NM1].Caption;

                        if (string.IsNullOrEmpty(check_NM))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                            MessageBox.Show(show_msg);
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }
                        isChange = true;
                        #endregion "STEEL_GRP" 체크

                        #region "ITEM_SIZE_MIN" 체크
                        check_field_NM2 = "ITEM_SIZE_MIN";
                        check_table_NM = "TB_STR_STDINFO_NO2";
                        check_value2 = grdMain2.GetData(checkrow, check_field_NM2).ToString();
                        check_Cols_NM2 = grdMain2.Cols[check_field_NM2].Caption;

                        if (string.IsNullOrEmpty(check_value2))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                            MessageBox.Show(show_msg);
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.isContainHangul(check_value2))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value2) > 4)
                        {
                            MessageBox.Show("숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        #endregion "ITEM_SIZE_MIN" 체크

                        #region "ITEM_SIZE_MAX" 체크
                        check_field_NM3 = "ITEM_SIZE_MAX";
                        check_table_NM = "TB_STR_STDINFO_NO2";
                        check_value3 = grdMain2.GetData(checkrow, check_field_NM3).ToString();
                        check_Cols_NM3 = grdMain2.Cols[check_field_NM3].Caption;

                        if (string.IsNullOrEmpty(check_value3))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM3);
                            MessageBox.Show(show_msg);
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.isContainHangul(check_value3))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value3) > 4)
                        {
                            MessageBox.Show("숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.Has_Item(check_table_NM, check_field_NM1, check_field_NM2, check_field_NM3, check_value1, check_value2, check_value3))
                        {
                            MessageBox.Show("필수요소가 중복되었습니다.");
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }
                        #endregion "ITEM_SIZE_MAX" 체크

                        // 추가로 연속 입력시에 추가 값
                        if (vf.IsSameRangeInGrd(grdMain2, check_NM, check_value2, check_value3, checkrow))
                        {
                            return;
                        }

                        // 크기비교
                        if (vf.CInt(check_value2) > vf.CInt(check_value3))
                        {
                            string message = string.Format("{0}의 값이 {1}값보다 큽니다.", check_Cols_NM2, check_Cols_NM3);
                            MessageBox.Show(message);
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.IsSameRange(check_table_NM, check_NM, check_value2, check_value3))
                        {
                            string message = string.Format("중복하는 범위가 이미 존재합니다.");
                            MessageBox.Show(message);
                            grdMain2.Select(checkrow, 1, true);
                            return;
                        }

                    }
                }
                #endregion grdMain2 체크
            }
            if (grd_Nm == "grdMain3")
            {
                #region grdMain3 체크

                for (checkrow = 1; checkrow < grdMain3.Rows.Count; checkrow++)
                {
                    gubun_value = grdMain3.GetData(checkrow, "GUBUN").ToString();

                    if (gubun_value == "삭제" || gubun_value == "수정")
                    {
                        isChange = true;
                    }

                    if (gubun_value == "추가")
                    {
                        #region "STEEL_GRP" 체크
                        check_field_NM1 = "STEEL_GRP";
                        check_table_NM = "TB_STR_STDINFO_NO3";
                        check_value1 = grdMain3.GetData(checkrow, check_field_NM1).ToString();
                        check_Cols_NM1 = grdMain3.Cols[check_field_NM1].Caption;

                        if (string.IsNullOrEmpty(check_value1))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                            MessageBox.Show(show_msg);
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.isContainHangul(check_value1))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value1) > 30)
                        {
                            MessageBox.Show("영문 및 숫자 30자 이하로 입력하시기 바랍니다..");
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        // 명이입력되지 않은경우 체크
                        check_field_NM1 = "STEEL_GRP";
                        check_NM = grdMain3.GetData(checkrow, check_field_NM1).ToString();
                        check_Cols_NM1 = grdMain3.Cols[check_field_NM1].Caption;

                        if (string.IsNullOrEmpty(check_NM))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                            MessageBox.Show(show_msg);
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }
                        isChange = true;
                        #endregion "STEEL_GRP" 체크

                        #region "ITEM_SIZE_MIN" 체크
                        check_field_NM2 = "ITEM_SIZE_MIN";
                        check_table_NM = "TB_STR_STDINFO_NO3";
                        check_value2 = grdMain3.GetData(checkrow, check_field_NM2).ToString();
                        check_Cols_NM2 = grdMain3.Cols[check_field_NM2].Caption;

                        if (string.IsNullOrEmpty(check_value2))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                            MessageBox.Show(show_msg);
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.isContainHangul(check_value2))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value2) > 4)
                        {
                            MessageBox.Show("숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        #endregion "ITEM_SIZE_MIN" 체크

                        #region "ITEM_SIZE_MAX" 체크
                        check_field_NM3 = "ITEM_SIZE_MAX";
                        check_table_NM = "TB_STR_STDINFO_NO3";
                        check_value3 = grdMain3.GetData(checkrow, check_field_NM3).ToString();
                        check_Cols_NM3 = grdMain3.Cols[check_field_NM3].Caption;

                        if (string.IsNullOrEmpty(check_value3))
                        {
                            show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM3);
                            MessageBox.Show(show_msg);
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.isContainHangul(check_value3))
                        {
                            MessageBox.Show("한글이 포함되어서는 안됩니다.");
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.getStrLen(check_value3) > 4)
                        {
                            MessageBox.Show("숫자 4자 이하로 입력하시기 바랍니다..");
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.Has_Item(check_table_NM, check_field_NM1, check_field_NM2, check_field_NM3, check_value1, check_value2, check_value3))
                        {
                            MessageBox.Show("필수요소가 중복되었습니다.");
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }
                        #endregion "ITEM_SIZE_MAX" 체크

                        // 추가로 연속 입력시에 추가 값
                        if (vf.IsSameRangeInGrd(grdMain3, check_NM, check_value2, check_value3, checkrow))
                        {
                            return;
                        }

                        // 크기비교
                        if (vf.CInt(check_value2) > vf.CInt(check_value3))
                        {
                            string message = string.Format("{0}의 값이 {1}값보다 큽니다.", check_Cols_NM2, check_Cols_NM3);
                            MessageBox.Show(message);
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }

                        if (vf.IsSameRange(check_table_NM, check_NM, check_value2, check_value3))
                        {
                            string message = string.Format("중복하는 범위가 이미 존재합니다.");
                            MessageBox.Show(message);
                            grdMain3.Select(checkrow, 1, true);
                            return;
                        }
                    }
                }
                #endregion grdMain3 체크
            }

            if (isChange)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }
            #endregion 삭제항목체크


            string strQry = string.Empty;
            string strMsg = string.Empty;

            int row = 0;
            int InsCnt = 0;
            int UpCnt = 0;
            int DelCnt = 0;
            
            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                if (grd_Nm == "grdMain1")
                {
                    #region grdMain1
                    for (row = 1; row < grdMain1.Rows.Count; row++)
                    {
                        // Insert 처리
                        if (grdMain1.GetData(row, grdMain1.Cols.Count - 1).ToString() == "추가")
                        {
                            string sql1 = string.Format(" INSERT INTO TB_STR_STDINFO_NO1 ");
                            sql1 += string.Format(" ( ");
                            sql1 += string.Format("      STEEL_GRP ");
                            sql1 += string.Format("     ,ITEM_SIZE_MIN ");
                            sql1 += string.Format("     ,ITEM_SIZE_MAX ");
                            sql1 += string.Format("     ,TOP_ROLL_GAP_1 ");
                            sql1 += string.Format("     ,TOP_ROLL_GAP_2 ");
                            sql1 += string.Format("     ,TOP_ROLL_GAP_3 ");
                            sql1 += string.Format("     ,TOP_ROLL_GAP_4 ");
                            sql1 += string.Format("     ,TOP_ROLL_GAP_5 ");
                            sql1 += string.Format("     ,TOP_ROLL_GAP_6 ");
                            sql1 += string.Format("     ,TOP_ROLL_ANGLE_1 ");
                            sql1 += string.Format("     ,TOP_ROLL_ANGLE_2 ");
                            sql1 += string.Format("     ,TOP_ROLL_ANGLE_3 ");
                            sql1 += string.Format("     ,TOP_ROLL_ANGLE_4 ");
                            sql1 += string.Format("     ,TOP_ROLL_ANGLE_5 ");
                            sql1 += string.Format("     ,TOP_ROLL_ANGLE_6 ");
                            sql1 += string.Format("     ,BOT_ROLL_ANGLE_1 ");
                            sql1 += string.Format("     ,BOT_ROLL_ANGLE_2 ");
                            sql1 += string.Format(",     BOT_ROLL_ANGLE_3 ");
                            sql1 += string.Format("     ,THREAD_SPEED ");
                            sql1 += string.Format("     ,MACHINE_SPEED ");
                            sql1 += string.Format("     ,INLET_ROLL_CURRENT ");
                            sql1 += string.Format("     ,MID_ROLL_CURRENT ");
                            sql1 += string.Format("     ,OUTLET_ROLL_CURRENT ");
                            sql1 += string.Format("     ,REGISTER  ");
                            sql1 += string.Format("     ,REG_DDTT  ");
                            sql1 += string.Format(" ) ");
                            sql1 += string.Format("  VALUES ");
                            sql1 += string.Format("  ( ");
                            sql1 += string.Format("     '{0}' ", grdMain1.GetData(row, "STEEL_GRP"));
                            sql1 += string.Format("     ,nvl('{0}','0000') ", grdMain1.GetData(row, "ITEM_SIZE_MIN").ToString());
                            sql1 += string.Format("     ,nvl('{0}','9999') ", grdMain1.GetData(row, "ITEM_SIZE_MAX").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_1").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_2").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_3").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_4").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_5").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_6").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_1").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_2").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_3").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_4").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_5").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_6").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "BOT_ROLL_ANGLE_1").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "BOT_ROLL_ANGLE_2").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "BOT_ROLL_ANGLE_3").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "THREAD_SPEED").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "MACHINE_SPEED").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "INLET_ROLL_CURRENT").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "MID_ROLL_CURRENT").ToString());
                            sql1 += string.Format("     ,nvl('{0}',0) ", grdMain1.GetData(row, "OUTLET_ROLL_CURRENT").ToString());
                            sql1 += string.Format("     ,'{0}' ", ck.UserID);
                            sql1 += string.Format("     ,SYSDATE ");
                            sql1 += string.Format("   ) ");

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            InsCnt++;
                        }
                        // Update 처리
                        else if (grdMain1.GetData(row, grdMain1.Cols.Count - 1).ToString() == "수정")
                        {
                            string sql1 = string.Format(" UPDATE TB_STR_STDINFO_NO1   ");
                            sql1 += string.Format(" SET TOP_ROLL_GAP_1 = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_1").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE_1   = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_1").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_GAP_2 = NVL('{0}', 0)  ", grdMain1.GetData(row, "TOP_ROLL_GAP_2").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE_2   = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_2").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_GAP_3 = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_3").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE_3   = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_3").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_GAP_4 = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_4").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE_4   = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_4").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_GAP_5 = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_5").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE_5   = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_5").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_GAP_6 = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_GAP_6").ToString());
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE_6   = NVL('{0}', 0) ", grdMain1.GetData(row, "TOP_ROLL_ANGLE_6").ToString());
                            sql1 += string.Format("    ,BOT_ROLL_ANGLE_1   = NVL('{0}', 0) ", grdMain1.GetData(row, "BOT_ROLL_ANGLE_1").ToString());
                            sql1 += string.Format("    ,BOT_ROLL_ANGLE_2   = NVL('{0}', 0) ", grdMain1.GetData(row, "BOT_ROLL_ANGLE_2").ToString());
                            sql1 += string.Format("    ,BOT_ROLL_ANGLE_3   = NVL('{0}', 0) ", grdMain1.GetData(row, "BOT_ROLL_ANGLE_3").ToString());
                            sql1 += string.Format("    ,THREAD_SPEED   = NVL('{0}', 0)  ", grdMain1.GetData(row, "THREAD_SPEED").ToString());
                            sql1 += string.Format("    ,MACHINE_SPEED  = NVL('{0}', 0)  ", grdMain1.GetData(row, "MACHINE_SPEED").ToString());
                            sql1 += string.Format("    ,INLET_ROLL_CURRENT  = NVL('{0}', 0)  ", grdMain1.GetData(row, "INLET_ROLL_CURRENT").ToString());
                            sql1 += string.Format("    ,MID_ROLL_CURRENT   = NVL('{0}', 0)  ", grdMain1.GetData(row, "MID_ROLL_CURRENT").ToString());
                            sql1 += string.Format("    ,OUTLET_ROLL_CURRENT   = NVL('{0}', 0)  ", grdMain1.GetData(row, "OUTLET_ROLL_CURRENT").ToString());
                            sql1 += string.Format("    ,MODIFIER       = '{0}'   ", ck.UserID);
                            sql1 += string.Format("    ,MOD_DDTT       = SYSDATE ");
                            sql1 += string.Format(" WHERE STEEL_GRP   = '{0}' ", grdMain1.GetData(row, "STEEL_GRP").ToString());
                            sql1 += string.Format(" AND ITEM_SIZE_MIN  = '{0}' ", grdMain1.GetData(row, "ITEM_SIZE_MIN").ToString());
                            sql1 += string.Format(" AND ITEM_SIZE_MAX   = '{0}' ", grdMain1.GetData(row, "ITEM_SIZE_MAX").ToString());

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            UpCnt++;
                        }
                        else if (grdMain1.GetData(row, grdMain1.Cols.Count - 1).ToString() == "삭제")
                        {

                            string sql1 = string.Format(" DELETE FROM TB_STR_STDINFO_NO1   ");
                            sql1 += string.Format(" WHERE STEEL_GRP   = '{0}' ", grdMain1.GetData(row, "STEEL_GRP"));
                            sql1 += string.Format(" AND ITEM_SIZE_MIN  >= NVL('{0}','0000')   ", grdMain1.GetData(row, "ITEM_SIZE_MIN"));
                            sql1 += string.Format(" AND ITEM_SIZE_MAX  <= NVL('{0}','9999')   ", grdMain1.GetData(row, "ITEM_SIZE_MAX"));

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            DelCnt++;
                        }
                    }
                    #endregion grdMain1 
                }

                if (grd_Nm == "grdMain2")
                {
                    #region grdMain2
                    for (row = 1; row < grdMain2.Rows.Count; row++)
                    {
                        // Insert 처리
                        if (grdMain2.GetData(row, grdMain2.Cols.Count - 1).ToString() == "추가")
                        {
                            string sql1 = string.Format("INSERT INTO TB_STR_STDINFO_NO2 ");
                            sql1 += string.Format(" ( ");
                            sql1 += string.Format("   STEEL_GRP  ");
                            sql1 += string.Format("  ,ITEM_SIZE_MIN ");
                            sql1 += string.Format("  ,ITEM_SIZE_MAX ");
                            sql1 += string.Format("  ,TOP_ROLL_POS  ");
                            sql1 += string.Format("  ,TOP_ROLL_ANGLE ");
                            sql1 += string.Format("  ,BOT_ROLL_ANGLE ");
                            sql1 += string.Format("  ,UP_DOWN_LINEAR_POS ");
                            sql1 += string.Format("  ,FRONT_LINEAR_GUIDE_POS ");
                            sql1 += string.Format("  ,REAL_LINEAR_GUIDE_POS ");
                            sql1 += string.Format("  ,TOP_ROLL_MTR_HZ  ");
                            sql1 += string.Format("  ,BOT_ROLL_MTR_HZ ");
                            sql1 += string.Format("     ,REGISTER  ");
                            sql1 += string.Format("     ,REG_DDTT  ");
                            sql1 += string.Format(" ) ");
                            sql1 += string.Format("  VALUES (");
                            sql1 += string.Format("          '{0}' ", grdMain2.GetData(row, "STEEL_GRP").ToString());
                            sql1 += string.Format("      , nvl('{0}','0000') ", grdMain2.GetData(row, "ITEM_SIZE_MIN").ToString());
                            sql1 += string.Format("      , nvl('{0}','9999') ", grdMain2.GetData(row, "ITEM_SIZE_MAX").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "TOP_ROLL_POS").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "TOP_ROLL_ANGLE").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "BOT_ROLL_ANGLE").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "UP_DOWN_LINEAR_POS").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "FRONT_LINEAR_GUIDE_POS").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "REAL_LINEAR_GUIDE_POS").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "TOP_ROLL_MTR_HZ").ToString());
                            sql1 += string.Format("      , nvl('{0}',0) ", grdMain2.GetData(row, "BOT_ROLL_MTR_HZ").ToString());
                            sql1 += string.Format("      ,'{0}' ", ck.UserID);
                            sql1 += string.Format("      ,SYSDATE ");
                            sql1 += string.Format(") ");

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            InsCnt++;
                        }
                        // Update 처리
                        else if (grdMain2.GetData(row, grdMain2.Cols.Count - 1).ToString() == "수정")
                        {
                            string sql1 = string.Format(" UPDATE TB_STR_STDINFO_NO2  ");
                            sql1 += string.Format(" SET  ");
                            sql1 += string.Format("     TOP_ROLL_POS        = NVL('{0}', 0) ", grdMain2.GetData(row, "TOP_ROLL_POS"));
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE     = NVL('{0}', 0) ", grdMain2.GetData(row, "TOP_ROLL_ANGLE"));
                            sql1 += string.Format("    ,BOT_ROLL_ANGLE         = NVL('{0}', 0) ", grdMain2.GetData(row, "BOT_ROLL_ANGLE"));
                            sql1 += string.Format("    ,UP_DOWN_LINEAR_POS     = NVL('{0}', 0) ", grdMain2.GetData(row, "UP_DOWN_LINEAR_POS"));
                            sql1 += string.Format("    ,FRONT_LINEAR_GUIDE_POS = NVL('{0}', 0) ", grdMain2.GetData(row, "FRONT_LINEAR_GUIDE_POS"));
                            sql1 += string.Format("    ,REAL_LINEAR_GUIDE_POS  = NVL('{0}', 0) ", grdMain2.GetData(row, "REAL_LINEAR_GUIDE_POS"));
                            sql1 += string.Format("    ,TOP_ROLL_MTR_HZ        = NVL('{0}', 0) ", grdMain2.GetData(row, "TOP_ROLL_MTR_HZ"));
                            sql1 += string.Format("    ,BOT_ROLL_MTR_HZ       = NVL('{0}', 0) ", grdMain2.GetData(row, "BOT_ROLL_MTR_HZ"));
                            sql1 += string.Format("    ,MODIFIER       = '{0}'   ", ck.UserID);
                            sql1 += string.Format("    ,MOD_DDTT       = SYSDATE ");
                            sql1 += string.Format(" WHERE STEEL_GRP   = '{0}' ", grdMain2.GetData(row, "STEEL_GRP"));
                            sql1 += string.Format(" AND ITEM_SIZE_MIN  = '{0}' ", grdMain2.GetData(row, "ITEM_SIZE_MIN"));
                            sql1 += string.Format(" AND ITEM_SIZE_MAX   = '{0}' ", grdMain2.GetData(row, "ITEM_SIZE_MAX"));

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            UpCnt++;
                        }
                        else if (grdMain2.GetData(row, grdMain2.Cols.Count - 1).ToString() == "삭제")
                        {
                            strQry = string.Format("DELETE FROM TB_STR_STDINFO_NO2 ");
                            strQry += string.Format("WHERE STEEL_GRP = '{0}' ", grdMain2.GetData(row, "STEEL_GRP"));
                            strQry += string.Format("AND   ITEM_SIZE_MIN = '{0}' ", grdMain2.GetData(row, "ITEM_SIZE_MIN"));
                            strQry += string.Format("AND   ITEM_SIZE_MAX = '{0}' ", grdMain2.GetData(row, "ITEM_SIZE_MAX"));

                            cmd.CommandText = strQry;
                            cmd.ExecuteNonQuery();

                            DelCnt++;
                        }
                    }
                    #endregion grdMain2 
                }

                if (grd_Nm == "grdMain3")
                {
                    #region grdMain3
                    for (row = 1; row < grdMain3.Rows.Count; row++)
                    {
                        // Insert 처리
                        if (grdMain3.GetData(row, grdMain3.Cols.Count - 1).ToString() == "추가")
                        {
                            string sql1 = string.Format("INSERT INTO TB_STR_STDINFO_NO3 ");
                            sql1 += string.Format("        ( ");
                            sql1 += string.Format("             STEEL_GRP  ");
                            sql1 += string.Format("            ,ITEM_SIZE_MIN ");
                            sql1 += string.Format("            ,ITEM_SIZE_MAX ");
                            sql1 += string.Format("            ,TOP_ROLL_POS  ");
                            sql1 += string.Format("            ,TOP_ROLL_ANGLE ");
                            sql1 += string.Format("            ,BOT_ROLL_ANGLE ");
                            sql1 += string.Format("            ,UP_DOWN_LINEAR_POS ");
                            sql1 += string.Format("            ,FRONT_LINEAR_GUIDE_POS ");
                            sql1 += string.Format("            ,REAL_LINEAR_GUIDE_POS ");
                            sql1 += string.Format("            ,TOP_ROLL_MTR_HZ  ");
                            sql1 += string.Format("            ,BOT_ROLL_MTR_HZ ");
                            sql1 += string.Format("            ,REGISTER  ");
                            sql1 += string.Format("            ,REG_DDTT  ");
                            sql1 += string.Format("         )  ");
                            sql1 += string.Format("  VALUES ");
                            sql1 += string.Format("        ( ");
                            sql1 += string.Format("           '{0}' ", grdMain3.GetData(row, "STEEL_GRP").ToString());
                            sql1 += string.Format("           ,nvl('{0}','0000') ", grdMain3.GetData(row, "ITEM_SIZE_MIN").ToString());
                            sql1 += string.Format("           ,nvl('{0}','9999') ", grdMain3.GetData(row, "ITEM_SIZE_MAX").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "TOP_ROLL_POS").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "TOP_ROLL_ANGLE").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "BOT_ROLL_ANGLE").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "UP_DOWN_LINEAR_POS").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "FRONT_LINEAR_GUIDE_POS").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "REAL_LINEAR_GUIDE_POS").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "TOP_ROLL_MTR_HZ").ToString());
                            sql1 += string.Format("           ,nvl('{0}',0) ", grdMain3.GetData(row, "BOT_ROLL_MTR_HZ").ToString());
                            sql1 += string.Format("           ,'{0}' ", ck.UserID);
                            sql1 += string.Format("           ,SYSDATE ");
                            sql1 += string.Format("        ) ");

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            InsCnt++;
                        }
                        // Update 처리
                        else if (grdMain3.GetData(row, grdMain3.Cols.Count - 1).ToString() == "수정")
                        {
                            string sql1 = string.Format(" UPDATE TB_STR_STDINFO_NO3  ");
                            sql1 += string.Format(" SET  ");
                            sql1 += string.Format("     TOP_ROLL_POS        = NVL('{0}', 0) ", grdMain3.GetData(row, "TOP_ROLL_POS"));
                            sql1 += string.Format("    ,TOP_ROLL_ANGLE     = NVL('{0}', 0) ", grdMain3.GetData(row, "TOP_ROLL_ANGLE"));
                            sql1 += string.Format("    ,BOT_ROLL_ANGLE         = NVL('{0}', 0) ", grdMain3.GetData(row, "BOT_ROLL_ANGLE"));
                            sql1 += string.Format("    ,UP_DOWN_LINEAR_POS     = NVL('{0}', 0) ", grdMain3.GetData(row, "UP_DOWN_LINEAR_POS"));
                            sql1 += string.Format("    ,FRONT_LINEAR_GUIDE_POS = NVL('{0}', 0) ", grdMain3.GetData(row, "FRONT_LINEAR_GUIDE_POS"));
                            sql1 += string.Format("    ,REAL_LINEAR_GUIDE_POS  = NVL('{0}', 0) ", grdMain3.GetData(row, "REAL_LINEAR_GUIDE_POS"));
                            sql1 += string.Format("    ,TOP_ROLL_MTR_HZ        = NVL('{0}', 0) ", grdMain3.GetData(row, "TOP_ROLL_MTR_HZ"));
                            sql1 += string.Format("    ,BOT_ROLL_MTR_HZ       = NVL('{0}', 0) ", grdMain3.GetData(row, "BOT_ROLL_MTR_HZ"));
                            sql1 += string.Format("    ,MODIFIER       = '{0}'   ", ck.UserID);
                            sql1 += string.Format("    ,MOD_DDTT       = SYSDATE ");
                            sql1 += string.Format(" WHERE STEEL_GRP   = '{0}' ", grdMain3.GetData(row, "STEEL_GRP"));
                            sql1 += string.Format(" AND ITEM_SIZE_MIN  = '{0}' ", grdMain3.GetData(row, "ITEM_SIZE_MIN"));
                            sql1 += string.Format(" AND ITEM_SIZE_MAX   = '{0}' ", grdMain3.GetData(row, "ITEM_SIZE_MAX"));

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            UpCnt++;
                        }
                        else if (grdMain3.GetData(row, grdMain3.Cols.Count - 1).ToString() == "삭제")
                        {
                            string sql1 = string.Format("DELETE FROM TB_STR_STDINFO_NO3 ");
                            sql1 += string.Format("WHERE STEEL_GRP = '{0}' ", grdMain3.GetData(row, "STEEL_GRP"));
                            sql1 += string.Format("AND   ITEM_SIZE_MIN = '{0}' ", grdMain3.GetData(row, "ITEM_SIZE_MIN"));
                            sql1 += string.Format("AND   ITEM_SIZE_MAX = '{0}' ", grdMain3.GetData(row, "ITEM_SIZE_MAX"));

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();

                            DelCnt++;
                        }
                    }
                    #endregion grdMain3 
                }
                    //실행후 성공
                transaction.Commit();

                btnDisplay_Click(null, null);
                
                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다.");
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
            return;
        }
        #endregion 저장 설정

        #region 이벤트 설정

        #region 그리드 더블 클릭 이벤트 설정

        private void grdMain1_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain1.Row <= 0)
            {
                return;
            }
            grdMain1.AllowEditing = true;
        }

        private void grdMain2_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain2.Row <= 0)
            {
                return;
            }
            grdMain2.AllowEditing = true;
        }

        private void grdMain3_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain3.Row <= 0)
            {
                return;
            }
            grdMain3.AllowEditing = true;
        }

        #endregion 그리드 더블 클릭 이벤트 설정

        #region 추가 삭제 버튼 이벤트 설정
        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            C1FlexGrid selectedGrd = SelectedGrd_InTab();

            // 수정가능 하도록 열추가
            selectedGrd.AllowEditing = true;

            //추가 행 데이터 디폴트 값넣기
            selectedGrd.Rows.Add();

            for (int column = 4; column < selectedGrd.Cols.Count - 1; column++)
            {
                selectedGrd.SetData(selectedGrd.Rows.Count - 1, column, 0);
            }

            selectedGrd.SetData(selectedGrd.Rows.Count - 1, 0, (selectedGrd.Rows.Count - 1).ToString());

            // 저장시 Insert로 처리하기 위해 flag set
            selectedGrd.SetData(selectedGrd.Rows.Count - 1, selectedGrd.Cols.Count - 1, "추가");
            selectedGrd.SetData(selectedGrd.Rows.Count - 1, 0, "추가");

            // Insert 배경색 지정
            selectedGrd.Rows[selectedGrd.Rows.Count - 1].Style = selectedGrd.Styles["InsColor"];

            //// 커서위치 결정
            selectedGrd.Row = 0;
            selectedGrd.Col = 0;
        }

        private void btnDelRow_Click(object sender, EventArgs e)
        {
            C1FlexGrid selectedGrd = SelectedGrd_InTab();

            if (selectedGrd.Rows.Count < 2 || selectedGrd.Row < 1)
            {
                return;
            }

            //mj 추가되었지만 행삭제로 지울때
            if (selectedGrd.Rows[selectedGrd.Row][selectedGrd.Cols.Count - 1].ToString() == "추가")
            {
                selectedGrd.RemoveItem(selectedGrd.Row);
                return;
            }

            // 저장시 Delete로 처리하기 위해 flag set
            selectedGrd.Rows[selectedGrd.Row][selectedGrd.Cols.Count - 1] = "삭제";
            selectedGrd.Rows[selectedGrd.Row][0] = "삭제";

            // Delete 배경색 지정
            selectedGrd.Rows[selectedGrd.Row].Style = selectedGrd.Styles["DelColor"];

            // 커서위치 결정
            selectedGrd.Row = 0;
            selectedGrd.Col = 0;
        }
        #endregion 추가 삭제 버튼 이벤트 설정

        #region 탭 이벤트 설정
        private C1FlexGrid SelectedGrd_InTab()
        {
            C1FlexGrid selectedGrd = null;

            C1.Win.C1Command.C1DockingTabPage selectedTab = TabOpt.SelectedTab;
            
            if (selectedTab.Name == "line_1_tp")
            {
                grd_Nm = "grdMain1";
            }
            else if (selectedTab.Name == "line_2_tp")
            {
                grd_Nm = "grdMain2";
            }
            else if (selectedTab.Name == "line_3_tp")
            {
                grd_Nm = "grdMain3";
            }
            return selectedGrd = (C1FlexGrid)selectedTab.Controls.Find(grd_Nm, true)[0];
        }
        private void TabOpt_SelectedIndexChanged(object sender, EventArgs e)
        {

            btnDisplay_Click(null, null);
            //SetDataBinding();
        }
        #endregion 탭 이벤트 설정

        #region Before & After Edit 설정

        #region grdMain1 설정
        private void grdMain1_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            if (e.Row < 1 || grd.GetData(e.Row, e.Col) == null)
            {
                return;
            }

            // 수정여부 확인을 위해 저장
            strBefValue1 = grd.GetData(e.Row, e.Col).ToString();
        }

        private void grdMain1_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            // No,구분은 수정 불가
            if (e.Col == 0 || e.Col == grd.Cols.Count - 1)
            {
                grd.SetData(e.Row, e.Col, strBefValue1);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue1 == grd.GetData(e.Row, e.Col).ToString().Trim())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grd.GetData(e.Row, grd.Cols.Count - 1).ToString() != "추가")
            {
                // 강종그룹, 규격min, max 수정 불가
                if (e.Col == grdMain1.Cols["STEEL_GRP"].Index || e.Col == grdMain1.Cols["ITEM_SIZE_MIN"].Index || e.Col == grdMain1.Cols["ITEM_SIZE_MAX"].Index)
                {
                    grd.SetData(e.Row, e.Col, strBefValue1);
                    return;
                }

                // 저장시 UPDATE로 처리하기 위해 flag set
                grd.SetData(e.Row, grd.Cols.Count - 1, "수정");
                grd.SetData(e.Row, 0, "수정");
                // Update 배경색 지정
                grd.Rows[e.Row].Style = grd.Styles["UpColor"];
            }
            else
            {
                if (e.Col == grd.Cols["ITEM_SIZE_MIN"].Index)
                {
                    set_value = vf.Format(vf.CInt(grd.GetData(e.Row, "ITEM_SIZE_MIN").ToString()).ToString().Trim(), "0000");
                    grd.SetData(e.Row, "ITEM_SIZE_MIN", set_value);
                }

                if (e.Col == grd.Cols["ITEM_SIZE_MAX"].Index)
                {
                    set_value = vf.Format(vf.CInt(grd.GetData(e.Row, "ITEM_SIZE_MAX").ToString()).ToString().Trim(), "0000");
                    grd.SetData(e.Row, "ITEM_SIZE_MAX", set_value);
                }
            }

            if (e.Col > grd.Cols["ITEM_SIZE_MAX"].Index && e.Col < grd.Cols["GUBUN"].Index)
            {
                if (string.IsNullOrEmpty(grd.GetData(e.Row, e.Col).ToString().Trim()))
                {
                    set_value = "0.0";
                    grd.SetData(e.Row, e.Col, set_value);
                }
            }
        }
        #endregion grdMain1 설정

        #region grdMain2 설정
        private void grdMain2_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            if (e.Row < 1 || grd.GetData(e.Row, e.Col) == null)
            {
                return;
            }
            // 수정여부 확인을 위해 저장
            strBefValue2 = grd.GetData(e.Row, e.Col).ToString();
        }

        private void grdMain2_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            // No,구분은 수정 불가
            if (e.Col == 0 || e.Col == grd.Cols.Count - 1)
            {
                grd.SetData(e.Row, e.Col, strBefValue2);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue2 == grd.GetData(e.Row, e.Col).ToString().Trim())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grd.GetData(e.Row, grd.Cols.Count - 1).ToString() != "추가")
            {
                // 강종그룹, 규격min, max 수정 불가
                if (e.Col == grdMain2.Cols["STEEL_GRP"].Index || e.Col == grdMain2.Cols["ITEM_SIZE_MIN"].Index || e.Col == grdMain2.Cols["ITEM_SIZE_MAX"].Index)
                {
                    grd.SetData(e.Row, e.Col, strBefValue2);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grd.SetData(e.Row, grd.Cols.Count - 1, "수정");
                grd.SetData(e.Row, 0, "수정");
                // Update 배경색 지정
                grd.Rows[e.Row].Style = grd.Styles["UpColor"];
            }
            else
            {
                if (e.Col == grd.Cols["ITEM_SIZE_MIN"].Index)
                {
                    set_value = vf.Format(vf.CInt(grd.GetData(e.Row, "ITEM_SIZE_MIN").ToString()).ToString().Trim(), "0000");
                    grd.SetData(e.Row, "ITEM_SIZE_MIN", set_value);
                }

                if (e.Col == grd.Cols["ITEM_SIZE_MAX"].Index)
                {
                    set_value = vf.Format(vf.CInt(grd.GetData(e.Row, "ITEM_SIZE_MAX").ToString()).ToString().Trim(), "0000");
                    grd.SetData(e.Row, "ITEM_SIZE_MAX", set_value);
                }
            }

            if (e.Col > grd.Cols["ITEM_SIZE_MAX"].Index && e.Col < grd.Cols["GUBUN"].Index)
            {
                if (string.IsNullOrEmpty(grd.GetData(e.Row, e.Col).ToString().Trim()))
                {
                    set_value = "0.0";
                    grd.SetData(e.Row, e.Col, set_value);
                }
            }
        }
        #endregion grdMain2 설정

        #region grdMain3 설정
        private void grdMain3_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            if (e.Row < 1 || grd.GetData(e.Row, e.Col) == null)
            {
                return;
            }
            // 수정여부 확인을 위해 저장
            strBefValue3 = grd.GetData(e.Row, e.Col).ToString();
        }

        private void grdMain3_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            // No,구분은 수정 불가
            if (e.Col == 0 || e.Col == grd.Cols.Count - 1)
            {
                grd.SetData(e.Row, e.Col, strBefValue3);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue3 == grd.GetData(e.Row, e.Col).ToString().Trim())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
            if (grd.GetData(e.Row, grd.Cols.Count - 1).ToString() != "추가")
            {
                // 강종그룹, 규격min, max 수정 불가
                if (e.Col == grdMain3.Cols["STEEL_GRP"].Index || e.Col == grdMain3.Cols["ITEM_SIZE_MIN"].Index || e.Col == grdMain3.Cols["ITEM_SIZE_MAX"].Index)
                {
                    grd.SetData(e.Row, e.Col, strBefValue3);
                    return;
                }
                // 저장시 UPDATE로 처리하기 위해 flag set
                grd.SetData(e.Row, grd.Cols.Count - 1, "수정");
                grd.SetData(e.Row, 0, "수정");
                // Update 배경색 지정
                grd.Rows[e.Row].Style = grd.Styles["UpColor"];
            }
            else
            {
                if (e.Col == grd.Cols["ITEM_SIZE_MIN"].Index)
                {
                    set_value = vf.Format(vf.CInt(grd.GetData(e.Row, "ITEM_SIZE_MIN").ToString()).ToString().Trim(), "0000");
                    grd.SetData(e.Row, "ITEM_SIZE_MIN", set_value);
                }

                if (e.Col == grd.Cols["ITEM_SIZE_MAX"].Index)
                {
                    set_value = vf.Format(vf.CInt(grd.GetData(e.Row, "ITEM_SIZE_MAX").ToString()).ToString().Trim(), "0000");
                    grd.SetData(e.Row, "ITEM_SIZE_MAX", set_value);
                }

            }

            if (e.Col > grd.Cols["ITEM_SIZE_MAX"].Index && e.Col < grd.Cols["GUBUN"].Index)
            {
                if (string.IsNullOrEmpty(grd.GetData(e.Row, e.Col).ToString().Trim()))
                {
                    set_value = "0.0";
                    grd.SetData(e.Row, e.Col, set_value);
                }
            }
        }
        #endregion grdMain3 설정

        #endregion Before & After Edit 설정

        #region 나머지 버튼, 콤보, 텍스트박스 등 이벤트 설정
        private void SetListDictionary()
        {
            var dt2 = cd.Find_STEEL_GRP("STEEL");
            
            //컬럼1 - 공장
            steel_Nm_ld = new ListDictionary();

            foreach (DataRow row in dt2.Rows)
            {
                steel_Nm_ld.Add(row["COLUMNA"].ToString(), row["COLUMNA"].ToString());
            }
        }

        private void SetComboBox()
        {
            // steel group
            DataTable dt = cd.Find_STEEL_GRP("STEEL");

            steel_grp_cb.Items.Add("전체");
            foreach (DataRow datarow in dt.Rows)
            {
                steel_grp_cb.Items.Add(datarow["COLUMNA"]);
            }

            // 아이템이 한개 이상인 경우 첫번째 아이템 선택
            if (steel_grp_cb.Items.Count > 1)
            {
                steel_grp_cb.SelectedIndex = 0;
            }
        }
        private void steel_grp_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string grp_Nm = string.Empty;
            if (steel_grp_cb.SelectedIndex >= 0)
            {
                grp_Nm = steel_grp_cb.SelectedItem as string;
                steel_grp_Nm = cd.Find_STEEL_GRP("STEEL", grp_Nm);
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (_CanSaveSearchLog)
            {
                cd.InsertLogForSearch(ck.UserID, btnDisplay);
            }
            

            SetDataBinding();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            C1FlexGrid selectedGrd = SelectedGrd_InTab();
            vf.SaveExcel(titleNM, selectedGrd);
        }

        private void item_size_tb_TextChanged(object sender, EventArgs e)
        {
            item_size_Nm = vf.Format(item_size_tb.Text, "0000");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void item_size_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

        #endregion 나머지 버튼, 콤보, 텍스트박스 등 이벤트 설정

        private void grdMain1_StartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid selectedGrd = sender as C1FlexGrid;

            if (selectedGrd.GetData(selectedGrd.Row, "GUBUN").ToString() != "추가")
            {
                if (selectedGrd.ColSel == selectedGrd.Cols["L_NO"].Index || selectedGrd.ColSel == selectedGrd.Cols["STEEL_GRP"].Index || selectedGrd.ColSel == selectedGrd.Cols["ITEM_SIZE_MIN"].Index || selectedGrd.ColSel == selectedGrd.Cols["ITEM_SIZE_MAX"].Index)
                {
                    e.Cancel = true;
                }
            }
        }

        private void grdMain2_StartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid selectedGrd = sender as C1FlexGrid;

            if (selectedGrd.GetData(selectedGrd.Row, "GUBUN").ToString() != "추가")
            {
                if (selectedGrd.ColSel == selectedGrd.Cols["L_NO"].Index || selectedGrd.ColSel == selectedGrd.Cols["STEEL_GRP"].Index || selectedGrd.ColSel == selectedGrd.Cols["ITEM_SIZE_MIN"].Index || selectedGrd.ColSel == selectedGrd.Cols["ITEM_SIZE_MAX"].Index)
                {
                    e.Cancel = true;
                }
            }
        }

        private void grdMain3_StartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid selectedGrd = sender as C1FlexGrid;

            if (selectedGrd.GetData(selectedGrd.Row, "GUBUN").ToString() != "추가")
            {
                if (selectedGrd.ColSel == selectedGrd.Cols["L_NO"].Index || selectedGrd.ColSel == selectedGrd.Cols["STEEL_GRP"].Index || selectedGrd.ColSel == selectedGrd.Cols["ITEM_SIZE_MIN"].Index || selectedGrd.ColSel == selectedGrd.Cols["ITEM_SIZE_MAX"].Index)
                {
                    e.Cancel = true;
                }
            }
        }

        private void item_size_tb_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                SendKeys.Send("{TAB}");
                item_size_tb.Text = vf.Format(item_size_tb.Text, "0000");
            }
        }
        #endregion 이벤트 설정
    }
}