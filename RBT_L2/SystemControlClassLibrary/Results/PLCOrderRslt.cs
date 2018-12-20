﻿using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using SystemControlClassLibrary.Popup;

namespace SystemControlClassLibrary.information
{
    public partial class PLCOrderRslt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;

        clsStyle cs = new clsStyle();

        // 셀의 수정전 값
        private Object strBefValue = "";

        private string ownerNM = "";
        private string titleNM = "";

        private string heat_nm = "";
        private string gangjong_id = "";

        private string cd_id = "";
        private string cd_nm = "";
        private string cd_id2 = "";
        private string cd_nm2 = "";

        private DateTime start_date;
        private DateTime end_date;
        #endregion 변수 설정

        #region 초기 로딩 설정
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public PLCOrderRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void PLCOrderRslt_Load(object sender, EventArgs e)
        {
            InitControl();

            grdMain1.Dock = DockStyle.Fill;
            grdMain2.Dock = DockStyle.Fill;
            grdMain3.Dock = DockStyle.Fill;
            grdMain4.Dock = DockStyle.Fill;
            grdMain5.Dock = DockStyle.Fill;
            grdMain6.Dock = DockStyle.Fill;
            grdMain7.Dock = DockStyle.Fill;

            btnDisplay_Click(null, null);
        }
        #endregion 초기 로딩 설정

        #region Init, Combo 설정
        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);
            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitPanel(panel1);

            //Label
            clsStyle.Style.InitLabel(lblLine);
            clsStyle.Style.InitLabel(lblMfgDate);
            clsStyle.Style.InitLabel(lblRouting);

            //Combo
            clsStyle.Style.InitCombo(cboRouting_GP, StringAlignment.Near);
            clsStyle.Style.InitCombo(cbo_Line_GP, StringAlignment.Near);

            //Button
            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnClose);

            //InitDateEdit
            cs.InitStartDateEdit(start_dt, DateTimePickerFormat.Custom);
            cs.InitEndDateEdit(end_dt, DateTimePickerFormat.Custom);
            
            SetComboBox1();
            SetComboBox2();

            InitGrd_Main1();
            InitGrd_Main2();
            InitGrd_Main3();
            InitGrd_Main4();
            InitGrd_Main5();
            InitGrd_Main6();
            InitGrd_Main7();

            grdMain1.BringToFront();
        }
        private void SetComboBox1()
        {
            cd.SetCombo(cbo_Line_GP, "LINE_GP", "", false, ck.Line_gp);
        }
        private void SetComboBox2()
        {
            List<string> list = new List<string>();
            list.Add("교정");
            list.Add("면취");
            list.Add("성분분석");

            cd.SetCombo(cboRouting_GP, "ROUTING_CD", "", false, list);
        }
        #endregion Init, Combo 설정

        #region 그리드 설정
        #region grdMain1 설정
        private void InitGrd_Main1()
        {
            cs.InitGrid_search(grdMain1, false, 2, 1);
            
            grdMain1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            #region 1. grdMain1 head 및 row  align 설정
            grdMain1[1, "L_NO"] = grdMain1.Cols["L_NO"].Caption;
            grdMain1.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain1[1, "WORK_DDTT"] = grdMain1.Cols["WORK_DDTT"].Caption;

            grdMain1[1, "HEAT"] = grdMain1.Cols["HEAT"].Caption;
            grdMain1[1, "STEEL"] = grdMain1.Cols["STEEL"].Caption;
            grdMain1[1, "STEEL_NM"] = grdMain1.Cols["STEEL_NM"].Caption;
            grdMain1[1, "POC_NO"] = grdMain1.Cols["POC_NO"].Caption;

            grdMain1[1, "COL_01O"] = "지시";
            grdMain1[1, "COL_01W"] = "실적";
            grdMain1[1, "COL_02O"] = "지시";
            grdMain1[1, "COL_02W"] = "실적";
            grdMain1[1, "COL_03O"] = "지시";
            grdMain1[1, "COL_03W"] = "실적";
            grdMain1[1, "COL_04O"] = "지시";
            grdMain1[1, "COL_04W"] = "실적";
            grdMain1[1, "COL_05O"] = "지시";
            grdMain1[1, "COL_05W"] = "실적";
            grdMain1[1, "COL_06O"] = "지시";
            grdMain1[1, "COL_06W"] = "실적";
            grdMain1[1, "COL_07O"] = "지시";
            grdMain1[1, "COL_07W"] = "실적";
            grdMain1[1, "COL_08O"] = "지시";
            grdMain1[1, "COL_08W"] = "실적";
            grdMain1[1, "COL_09O"] = "지시";
            grdMain1[1, "COL_09W"] = "실적";
            grdMain1[1, "COL_10O"] = "지시";
            grdMain1[1, "COL_10W"] = "실적";
            grdMain1[1, "COL_11O"] = "지시";
            grdMain1[1, "COL_11W"] = "실적";
            grdMain1[1, "COL_12O"] = "지시";
            grdMain1[1, "COL_12W"] = "실적";
            grdMain1[1, "COL_13O"] = "지시";
            grdMain1[1, "COL_13W"] = "실적";
            grdMain1[1, "COL_14O"] = "지시";
            grdMain1[1, "COL_14W"] = "실적";
            grdMain1[1, "COL_15O"] = "지시";
            grdMain1[1, "COL_15W"] = "실적";
            grdMain1[1, "COL_16O"] = "지시";
            grdMain1[1, "COL_16W"] = "실적";
            grdMain1[1, "COL_17O"] = "지시";
            grdMain1[1, "COL_17W"] = "실적";
            grdMain1[1, "COL_18O"] = "지시";
            grdMain1[1, "COL_18W"] = "실적";
            grdMain1[1, "COL_19O"] = "지시";
            grdMain1[1, "COL_19W"] = "실적";
            grdMain1[1, "COL_20O"] = "지시";
            grdMain1[1, "COL_20W"] = "실적";

            grdMain1.Cols["L_NO"].Width = 70;
            grdMain1.Cols["WORK_DDTT"].Width = 180;

            grdMain1.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain1.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain1.Cols["POC_NO"].Width = cs.POC_NO_Width;

            grdMain1.Cols["COL_01O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_01W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_02O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_02W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_03O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_03W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_04O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_04W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_05O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_05W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_06O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_06W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_07O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_07W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_08O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_08W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_09O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_09W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_10O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_10W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_11O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_11W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_12O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_12W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_13O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_13W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_14O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_14W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_15O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_15W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_16O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_16W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_17O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_17W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_18O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_18W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_19O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_19W"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_20O"].Width = cs.Short_Value_Width -10;  // 80
            grdMain1.Cols["COL_20W"].Width = cs.Short_Value_Width -10;  // 80

            grdMain1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain1.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain1.Cols["WORK_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain1.Cols["STEEL_NM"].TextAlign = cs.STEEL_TextAlign;
            grdMain1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

            grdMain1.Cols["COL_01O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_01W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_02O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_02W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_03O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_03W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_04O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_04W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_05O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_05W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_06O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_06W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_07O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_07W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_08O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_08W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_09O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_09W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_10O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_10W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_11O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_11W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_12O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_12W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_13O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_13W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_14O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_14W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_15O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_15W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_16O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_16W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_17O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_17W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_18O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_18W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_19O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_19W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_20O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain1.Cols["COL_20W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            #endregion

            grdMain1.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain1.Cols.Count; i++)
            {
                grdMain1.Cols[i].AllowMerging = true;
            }

            grdMain1.Rows[0].AllowMerging = true;
        }
        #endregion grdMain1 설정

        #region grdMain2 설정
        private void InitGrd_Main2()
        {
            cs.InitGrid_search(grdMain2, false, 2, 1);

            grdMain2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            #region 1. grdMain2 head 및 row  align 설정
            grdMain2[1, "L_NO"] = grdMain2.Cols["L_NO"].Caption;
            grdMain2.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain2[1, "WORK_DDTT"] = grdMain2.Cols["WORK_DDTT"].Caption;

            grdMain2[1, "HEAT"] = grdMain2.Cols["HEAT"].Caption;
            grdMain2[1, "STEEL"] = grdMain2.Cols["STEEL"].Caption;
            grdMain2[1, "STEEL_NM"] = grdMain2.Cols["STEEL_NM"].Caption;
            grdMain2[1, "POC_NO"] = grdMain2.Cols["POC_NO"].Caption;

            grdMain2[1, "COL_01O"] = "지시";
            grdMain2[1, "COL_01W"] = "실적";
            grdMain2[1, "COL_02O"] = "지시";
            grdMain2[1, "COL_02W"] = "실적";
            grdMain2[1, "COL_03O"] = "지시";
            grdMain2[1, "COL_03W"] = "실적";
            grdMain2[1, "COL_04O"] = "지시";
            grdMain2[1, "COL_04W"] = "실적";
            grdMain2[1, "COL_05O"] = "지시";
            grdMain2[1, "COL_05W"] = "실적";
            grdMain2[1, "COL_06O"] = "지시";
            grdMain2[1, "COL_06W"] = "실적";
            grdMain2[1, "COL_07O"] = "지시";
            grdMain2[1, "COL_07W"] = "실적";
            grdMain2[1, "COL_08O"] = "지시";
            grdMain2[1, "COL_08W"] = "실적";
            grdMain2[1, "COL_09O"] = "지시";
            grdMain2[1, "COL_09W"] = "실적";
            grdMain2[1, "COL_10O"] = "지시";
            grdMain2[1, "COL_10W"] = "실적";
            grdMain2[1, "COL_11O"] = "지시";
            grdMain2[1, "COL_11W"] = "실적";
            grdMain2[1, "COL_12O"] = "지시";
            grdMain2[1, "COL_12W"] = "실적";
            grdMain2[1, "COL_13O"] = "지시";
            grdMain2[1, "COL_13W"] = "실적";
            grdMain2[1, "COL_14O"] = "지시";
            grdMain2[1, "COL_14W"] = "실적";
            grdMain2[1, "COL_15O"] = "지시";
            grdMain2[1, "COL_15W"] = "실적";
            grdMain2[1, "COL_16O"] = "지시";
            grdMain2[1, "COL_16W"] = "실적";
            grdMain2[1, "COL_17O"] = "지시";
            grdMain2[1, "COL_17W"] = "실적";
            grdMain2[1, "COL_18O"] = "지시";
            grdMain2[1, "COL_18W"] = "실적";
            grdMain2[1, "COL_19O"] = "지시";
            grdMain2[1, "COL_19W"] = "실적";
            grdMain2[1, "COL_20O"] = "지시";
            grdMain2[1, "COL_20W"] = "실적";

            grdMain2.Cols["L_NO"].Width = 70;
            grdMain2.Cols["WORK_DDTT"].Width = 180;

            grdMain2.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain2.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain2.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain2.Cols["POC_NO"].Width = cs.POC_NO_Width;

            grdMain2.Cols["COL_01O"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_01W"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_02O"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_02W"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_03O"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_03W"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_04O"].Width = cs.Short_Value_Width + 10;
            grdMain2.Cols["COL_04W"].Width = cs.Short_Value_Width + 10;
            grdMain2.Cols["COL_05O"].Width = cs.Short_Value_Width + 10;
            grdMain2.Cols["COL_05W"].Width = cs.Short_Value_Width + 10;
            grdMain2.Cols["COL_06O"].Width = cs.Short_Value_Width + 10;
            grdMain2.Cols["COL_06W"].Width = cs.Short_Value_Width + 10;
            grdMain2.Cols["COL_07O"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_07W"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_08O"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_08W"].Width = cs.Short_Value_Width - 10;
            grdMain2.Cols["COL_09O"].Width = 0;
            grdMain2.Cols["COL_09W"].Width = 0;
            grdMain2.Cols["COL_10O"].Width = 0;
            grdMain2.Cols["COL_10W"].Width = 0;
            grdMain2.Cols["COL_11O"].Width = 0;
            grdMain2.Cols["COL_11W"].Width = 0;
            grdMain2.Cols["COL_12O"].Width = 0;
            grdMain2.Cols["COL_12W"].Width = 0;
            grdMain2.Cols["COL_13O"].Width = 0;
            grdMain2.Cols["COL_13W"].Width = 0;
            grdMain2.Cols["COL_14O"].Width = 0;
            grdMain2.Cols["COL_14W"].Width = 0;
            grdMain2.Cols["COL_15O"].Width = 0;
            grdMain2.Cols["COL_15W"].Width = 0;
            grdMain2.Cols["COL_16O"].Width = 0;
            grdMain2.Cols["COL_16W"].Width = 0;
            grdMain2.Cols["COL_17O"].Width = 0;
            grdMain2.Cols["COL_17W"].Width = 0;
            grdMain2.Cols["COL_18O"].Width = 0;
            grdMain2.Cols["COL_18W"].Width = 0;
            grdMain2.Cols["COL_19O"].Width = 0;
            grdMain2.Cols["COL_19W"].Width = 0;
            grdMain2.Cols["COL_20O"].Width = 0;
            grdMain2.Cols["COL_20W"].Width = 0;

            grdMain2.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain2.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain2.Cols["WORK_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain2.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain2.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain2.Cols["STEEL_NM"].TextAlign = cs.STEEL_TextAlign;
            grdMain2.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

            grdMain2.Cols["COL_01O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_01W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_02O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_02W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_03O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_03W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_04O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_04W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_05O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_05W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_06O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_06W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_07O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_07W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_08O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_08W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_09O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_09W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_10O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_10W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_11O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_11W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_12O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_12W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_13O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_13W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_14O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_14W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_15O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_15W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_16O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_16W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_17O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_17W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_18O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_18W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_19O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_19W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_20O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain2.Cols["COL_20W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            #endregion

            grdMain2.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain2.Cols.Count; i++)
            {
                grdMain2.Cols[i].AllowMerging = true;
            }
            grdMain2.Rows[0].AllowMerging = true;
        }
        #endregion grdMain2 설정

        #region grdMain3 설정
        private void InitGrd_Main3()
        {
            cs.InitGrid_search(grdMain3, false, 2, 1);

            grdMain3.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            #region 1. grdMain3 head 및 row  align 설정
            grdMain3[1, "L_NO"] = grdMain3.Cols["L_NO"].Caption;
            grdMain3.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain3[1, "WORK_DDTT"] = grdMain3.Cols["WORK_DDTT"].Caption;

            grdMain3[1, "HEAT"] = grdMain3.Cols["HEAT"].Caption;
            grdMain3[1, "STEEL"] = grdMain3.Cols["STEEL"].Caption;
            grdMain3[1, "STEEL_NM"] = grdMain3.Cols["STEEL_NM"].Caption;
            grdMain3[1, "POC_NO"] = grdMain3.Cols["POC_NO"].Caption;

            grdMain3[1, "COL_01O"] = "지시";
            grdMain3[1, "COL_01W"] = "실적";
            grdMain3[1, "COL_02O"] = "지시";
            grdMain3[1, "COL_02W"] = "실적";
            grdMain3[1, "COL_03O"] = "지시";
            grdMain3[1, "COL_03W"] = "실적";
            grdMain3[1, "COL_04O"] = "지시";
            grdMain3[1, "COL_04W"] = "실적";
            grdMain3[1, "COL_05O"] = "지시";
            grdMain3[1, "COL_05W"] = "실적";
            grdMain3[1, "COL_06O"] = "지시";
            grdMain3[1, "COL_06W"] = "실적";
            grdMain3[1, "COL_07O"] = "지시";
            grdMain3[1, "COL_07W"] = "실적";
            grdMain3[1, "COL_08O"] = "지시";
            grdMain3[1, "COL_08W"] = "실적";

            grdMain3.Cols["L_NO"].Width = 70;
            grdMain3.Cols["WORK_DDTT"].Width = 180;

            grdMain3.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain3.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain3.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain3.Cols["POC_NO"].Width = cs.POC_NO_Width;

            grdMain3.Cols["COL_01O"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_01W"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_02O"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_02W"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_03O"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_03W"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_04O"].Width = cs.Short_Value_Width + 10;
            grdMain3.Cols["COL_04W"].Width = cs.Short_Value_Width + 10;
            grdMain3.Cols["COL_05O"].Width = cs.Short_Value_Width + 10;
            grdMain3.Cols["COL_05W"].Width = cs.Short_Value_Width + 10;
            grdMain3.Cols["COL_06O"].Width = cs.Short_Value_Width + 10;
            grdMain3.Cols["COL_06W"].Width = cs.Short_Value_Width + 10;
            grdMain3.Cols["COL_07O"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_07W"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_08O"].Width = cs.Short_Value_Width - 10;
            grdMain3.Cols["COL_08W"].Width = cs.Short_Value_Width - 10;

            grdMain3.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain3.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain3.Cols["WORK_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain3.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain3.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain3.Cols["STEEL_NM"].TextAlign = cs.STEEL_TextAlign;
            grdMain3.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

            grdMain3.Cols["COL_01O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_01W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_02O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_02W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_03O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_03W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_04O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_04W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_05O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_05W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_06O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_06W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_07O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_07W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_08O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain3.Cols["COL_08W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            
            #endregion

            grdMain3.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain3.Cols.Count; i++)
            {
                grdMain3.Cols[i].AllowMerging = true;
            }
            grdMain3.Rows[0].AllowMerging = true;
        }
        #endregion grdMain3 설정

        #region grdMain4 설정
        private void InitGrd_Main4()
        {
            cs.InitGrid_search(grdMain4, false, 2, 1);

            grdMain4.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            #region 1. grdMain4 head 및 row  align 설정
            grdMain4[1, "L_NO"] = grdMain4.Cols["L_NO"].Caption;
            grdMain4.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain4[1, "WORK_DDTT"] = grdMain4.Cols["WORK_DDTT"].Caption;

            grdMain4[1, "HEAT"] = grdMain4.Cols["HEAT"].Caption;
            grdMain4[1, "STEEL"] = grdMain4.Cols["STEEL"].Caption;
            grdMain4[1, "STEEL_NM"] = grdMain4.Cols["STEEL_NM"].Caption;
            grdMain4[1, "POC_NO"] = grdMain4.Cols["POC_NO"].Caption;

            grdMain4[1, "COL_01O"] = "지시";
            grdMain4[1, "COL_01W"] = "실적";
            grdMain4[1, "COL_02O"] = "지시";
            grdMain4[1, "COL_02W"] = "실적";
            grdMain4[1, "COL_03O"] = "지시";
            grdMain4[1, "COL_03W"] = "실적";
            grdMain4[1, "COL_04O"] = "지시";
            grdMain4[1, "COL_04W"] = "실적";
            grdMain4[1, "COL_05O"] = "지시";
            grdMain4[1, "COL_05W"] = "실적";
            grdMain4[1, "COL_06O"] = "지시";
            grdMain4[1, "COL_06W"] = "실적";
            grdMain4[1, "COL_07O"] = "지시";
            grdMain4[1, "COL_07W"] = "실적";
            grdMain4[1, "COL_08O"] = "지시";
            grdMain4[1, "COL_08W"] = "실적";
            grdMain4[1, "COL_09O"] = "지시";
            grdMain4[1, "COL_09W"] = "실적";
            grdMain4[1, "COL_10O"] = "지시";
            grdMain4[1, "COL_10W"] = "실적";
            grdMain4[1, "COL_11O"] = "지시";
            grdMain4[1, "COL_11W"] = "실적";
            grdMain4[1, "COL_12O"] = "지시";
            grdMain4[1, "COL_12W"] = "실적";
            grdMain4[1, "COL_13O"] = "지시";
            grdMain4[1, "COL_13W"] = "실적";

            grdMain4.Cols["L_NO"].Width = 70;
            grdMain4.Cols["WORK_DDTT"].Width = 180;

            grdMain4.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain4.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain4.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain4.Cols["POC_NO"].Width = cs.POC_NO_Width;

            grdMain4.Cols["COL_01O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_01W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_02O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_02W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_03O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_03W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_04O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_04W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_05O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_05W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_06O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_06W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_07O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_07W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_08O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_08W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_09O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_09W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_10O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_10W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_11O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_11W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_12O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_12W"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_13O"].Width = cs.Short_Value_Width - 10;
            grdMain4.Cols["COL_13W"].Width = cs.Short_Value_Width - 10;

            grdMain4.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain4.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain4.Cols["WORK_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain4.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain4.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain4.Cols["STEEL_NM"].TextAlign = cs.STEEL_TextAlign;
            grdMain4.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

            grdMain4.Cols["COL_01O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_01W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_02O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_02W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_03O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_03W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_04O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_04W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_05O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_05W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_06O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_06W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_07O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_07W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_08O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_08W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_09O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_09W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_10O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_10W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_11O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_11W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_12O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_12W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_13O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain4.Cols["COL_13W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            
            #endregion

            grdMain4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain4.Cols.Count; i++)
            {
                grdMain4.Cols[i].AllowMerging = true;
            }
            grdMain4.Rows[0].AllowMerging = true;
        }
        #endregion grdMain4 설정

        #region grdMain5 설정
        private void InitGrd_Main5()
        {
            cs.InitGrid_search(grdMain5, false, 2, 1);

            grdMain5.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            #region 1. grdMain5 head 및 row  align 설정
            grdMain5[1, "L_NO"] = grdMain5.Cols["L_NO"].Caption;
            grdMain5.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain5[1, "WORK_DDTT"] = grdMain5.Cols["WORK_DDTT"].Caption;

            grdMain5[1, "HEAT"] = grdMain5.Cols["HEAT"].Caption;
            grdMain5[1, "STEEL"] = grdMain5.Cols["STEEL"].Caption;
            grdMain5[1, "STEEL_NM"] = grdMain5.Cols["STEEL_NM"].Caption;
            grdMain5[1, "POC_NO"] = grdMain5.Cols["POC_NO"].Caption;

            grdMain5[1, "COL_01O"] = "지시";
            grdMain5[1, "COL_01W"] = "실적";
            grdMain5[1, "COL_02O"] = "지시";
            grdMain5[1, "COL_02W"] = "실적";
            grdMain5[1, "COL_03O"] = "지시";
            grdMain5[1, "COL_03W"] = "실적";
            grdMain5[1, "COL_04O"] = "지시";
            grdMain5[1, "COL_04W"] = "실적";
            grdMain5[1, "COL_05O"] = "지시";
            grdMain5[1, "COL_05W"] = "실적";
            grdMain5[1, "COL_06O"] = "지시";
            grdMain5[1, "COL_06W"] = "실적";
            grdMain5[1, "COL_07O"] = "지시";
            grdMain5[1, "COL_07W"] = "실적";
            grdMain5[1, "COL_08O"] = "지시";
            grdMain5[1, "COL_08W"] = "실적";
            grdMain5[1, "COL_09O"] = "지시";
            grdMain5[1, "COL_09W"] = "실적";
            grdMain5[1, "COL_10O"] = "지시";
            grdMain5[1, "COL_10W"] = "실적";
            grdMain5[1, "COL_11O"] = "지시";
            grdMain5[1, "COL_11W"] = "실적";
            grdMain5[1, "COL_12O"] = "지시";
            grdMain5[1, "COL_12W"] = "실적";
            grdMain5[1, "COL_13O"] = "지시";
            grdMain5[1, "COL_13W"] = "실적";
            grdMain5[1, "COL_14O"] = "지시";
            grdMain5[1, "COL_14W"] = "실적";
            grdMain5[1, "COL_15O"] = "지시";
            grdMain5[1, "COL_15W"] = "실적";
            grdMain5[1, "COL_16O"] = "지시";
            grdMain5[1, "COL_16W"] = "실적";

            grdMain5.Cols["L_NO"].Width = 70;
            grdMain5.Cols["WORK_DDTT"].Width = 180;

            grdMain5.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain5.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain5.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain5.Cols["POC_NO"].Width = cs.POC_NO_Width;

            grdMain5.Cols["COL_01O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_01W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_02O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_02W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_03O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_03W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_04O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_04W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_05O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_05W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_06O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_06W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_07O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_07W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_08O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_08W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_09O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_09W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_10O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_10W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_11O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_11W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_12O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_12W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_13O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_13W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_14O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_14W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_15O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_15W"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_16O"].Width = cs.Short_Value_Width - 10;
            grdMain5.Cols["COL_16W"].Width = cs.Short_Value_Width - 10;

            grdMain5.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain5.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain5.Cols["WORK_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain5.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain5.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain5.Cols["STEEL_NM"].TextAlign = cs.STEEL_TextAlign;
            grdMain5.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

            grdMain5.Cols["COL_01O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_01W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_02O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_02W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_03O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_03W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_04O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_04W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_05O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_05W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_06O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_06W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_07O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_07W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_08O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_08W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_09O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_09W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_10O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_10W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_11O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_11W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_12O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_12W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_13O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_13W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_14O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_14W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_15O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_15W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_16O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain5.Cols["COL_16W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            
            #endregion

            grdMain5.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain5.Cols.Count; i++)
            {
                grdMain5.Cols[i].AllowMerging = true;
            }
            grdMain5.Rows[0].AllowMerging = true;
        }
        #endregion grdMain5 설정
        
        #region grdMain6 설정
        private void InitGrd_Main6()
        {
            cs.InitGrid_search(grdMain6, false, 2, 1);

            grdMain6.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
     
            #region 1. grdMain6 head 및 row  align 설정
            grdMain6[1, "L_NO"] = grdMain6.Cols["L_NO"].Caption;
            grdMain6.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain6[1, "WORK_DDTT"] = grdMain6.Cols["WORK_DDTT"].Caption;

            grdMain6[1, "HEAT"] = grdMain6.Cols["HEAT"].Caption;
            grdMain6[1, "STEEL"] = grdMain6.Cols["STEEL"].Caption;
            grdMain6[1, "STEEL_NM"] = grdMain6.Cols["STEEL_NM"].Caption;
            grdMain6[1, "POC_NO"] = grdMain6.Cols["POC_NO"].Caption;

            grdMain6[1, "COL_01O"] = "지시";
            grdMain6[1, "COL_01W"] = "실적";
            grdMain6[1, "COL_02O"] = "지시";
            grdMain6[1, "COL_02W"] = "실적";
            grdMain6[1, "COL_03O"] = "지시";
            grdMain6[1, "COL_03W"] = "실적";
            grdMain6[1, "COL_04O"] = "지시";
            grdMain6[1, "COL_04W"] = "실적";
            grdMain6[1, "COL_05O"] = "지시";
            grdMain6[1, "COL_05W"] = "실적";
            grdMain6[1, "COL_06O"] = "지시";
            grdMain6[1, "COL_06W"] = "실적";
            grdMain6[1, "COL_07O"] = "지시";
            grdMain6[1, "COL_07W"] = "실적";
            grdMain6[1, "COL_08O"] = "지시";
            grdMain6[1, "COL_08W"] = "실적";
            grdMain6[1, "COL_09O"] = "지시";
            grdMain6[1, "COL_09W"] = "실적";
            grdMain6[1, "COL_10O"] = "지시";
            grdMain6[1, "COL_10W"] = "실적";
            grdMain6[1, "COL_11O"] = "지시";
            grdMain6[1, "COL_11W"] = "실적";
            grdMain6[1, "COL_12O"] = "지시";
            grdMain6[1, "COL_12W"] = "실적";
            grdMain6[1, "COL_13O"] = "지시";
            grdMain6[1, "COL_13W"] = "실적";
            grdMain6[1, "COL_14O"] = "지시";
            grdMain6[1, "COL_14W"] = "실적";
            grdMain6[1, "COL_15O"] = "지시";
            grdMain6[1, "COL_15W"] = "실적";
            grdMain6[1, "COL_16O"] = "지시";
            grdMain6[1, "COL_16W"] = "실적";

            grdMain6.Cols["L_NO"].Width = 70;
            grdMain6.Cols["WORK_DDTT"].Width = 180;

            grdMain6.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain6.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain6.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain6.Cols["POC_NO"].Width = cs.POC_NO_Width;

            grdMain6.Cols["COL_01O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_01W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_02O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_02W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_03O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_03W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_04O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_04W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_05O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_05W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_06O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_06W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_07O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_07W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_08O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_08W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_09O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_09W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_10O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_10W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_11O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_11W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_12O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_12W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_13O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_13W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_14O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_14W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_15O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_15W"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_16O"].Width = cs.Short_Value_Width - 10;
            grdMain6.Cols["COL_16W"].Width = cs.Short_Value_Width - 10;

            grdMain6.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain6.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain6.Cols["WORK_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain6.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain6.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain6.Cols["STEEL_NM"].TextAlign = cs.STEEL_TextAlign;
            grdMain6.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

            grdMain6.Cols["COL_01O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_01W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_02O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_02W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_03O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_03W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_04O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_04W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_05O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_05W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_06O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_06W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_07O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_07W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_08O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_08W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_09O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_09W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_10O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_10W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_11O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_11W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_12O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_12W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_13O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_13W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_14O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_14W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_15O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_15W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_16O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain6.Cols["COL_16W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            #endregion

            grdMain6.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain6.Cols.Count; i++)
            {
                grdMain6.Cols[i].AllowMerging = true;
            }
            grdMain6.Rows[0].AllowMerging = true;
        }
        #endregion grdMain6 설정

        #region grdMain7 설정
        private void InitGrd_Main7()
        {
            cs.InitGrid_search(grdMain7, false, 2, 1);

            grdMain7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            
            #region 1. grdMain7 head 및 row  align 설정
            grdMain7[1, "L_NO"] = grdMain7.Cols["L_NO"].Caption;
            grdMain7.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain7[1, "WORK_DDTT"] = grdMain7.Cols["WORK_DDTT"].Caption;

            grdMain7[1, "HEAT"] = grdMain7.Cols["HEAT"].Caption;
            grdMain7[1, "STEEL"] = grdMain7.Cols["STEEL"].Caption;
            grdMain7[1, "STEEL_NM"] = grdMain7.Cols["STEEL_NM"].Caption;
            grdMain7[1, "POC_NO"] = grdMain7.Cols["POC_NO"].Caption;

            grdMain7[1, "COL_01O"] = "지시";
            grdMain7[1, "COL_01W"] = "실적";
            grdMain7[1, "COL_02O"] = "지시";
            grdMain7[1, "COL_02W"] = "실적";
            grdMain7[1, "COL_03O"] = "지시";
            grdMain7[1, "COL_03W"] = "실적";
            grdMain7[1, "COL_04O"] = "지시";
            grdMain7[1, "COL_04W"] = "실적";
            grdMain7[1, "COL_05O"] = "지시";
            grdMain7[1, "COL_05W"] = "실적";
            grdMain7[1, "COL_06O"] = "지시";
            grdMain7[1, "COL_06W"] = "실적";
            grdMain7[1, "COL_07O"] = "지시";
            grdMain7[1, "COL_07W"] = "실적";
            grdMain7[1, "COL_08O"] = "지시";
            grdMain7[1, "COL_08W"] = "실적";
            grdMain7[1, "COL_09O"] = "지시";
            grdMain7[1, "COL_09W"] = "실적";
            grdMain7[1, "COL_10O"] = "지시";
            grdMain7[1, "COL_10W"] = "실적";
            grdMain7[1, "COL_11O"] = "지시";
            grdMain7[1, "COL_11W"] = "실적";
            grdMain7[1, "COL_12O"] = "지시";
            grdMain7[1, "COL_12W"] = "실적";
            grdMain7[1, "COL_13O"] = "지시";
            grdMain7[1, "COL_13W"] = "실적";

            grdMain7.Cols["L_NO"].Width = 70;
            grdMain7.Cols["WORK_DDTT"].Width = 180;

            grdMain7.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain7.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain7.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain7.Cols["POC_NO"].Width = cs.POC_NO_Width;

            grdMain7.Cols["COL_01O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_01W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_02O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_02W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_03O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_03W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_04O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_04W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_05O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_05W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_06O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_06W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_07O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_07W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_08O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_08W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_09O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_09W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_10O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_10W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_11O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_11W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_12O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_12W"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_13O"].Width = cs.Short_Value_Width - 10;
            grdMain7.Cols["COL_13W"].Width = cs.Short_Value_Width - 10;

            grdMain7.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain7.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain7.Cols["WORK_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain7.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain7.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain7.Cols["STEEL_NM"].TextAlign = cs.STEEL_TextAlign;
            grdMain7.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;

            grdMain7.Cols["COL_01O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_01W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_02O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_02W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_03O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_03W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_04O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_04W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_05O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_05W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_06O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_06W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_07O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_07W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_08O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_08W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_09O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_09W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_10O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_10W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_11O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_11W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_12O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_12W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_13O"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["COL_13W"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            #endregion

            grdMain7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain7.Cols.Count; i++)
            {
                grdMain7.Cols[i].AllowMerging = true;
            }
            grdMain7.Rows[0].AllowMerging = true;
        }
        #endregion grdMain7 설정

        #endregion 그리드 설정

        #region 조회 SQL 설정
        private void setDataBinding()
        {
            string sql1 = string.Empty;
            string start_date = vf.Format(start_dt.Value, "yyyyMMddHHmmss").ToString();
            string end_date = vf.Format(end_dt.Value, "yyyyMMddHHmmss").ToString();

            if (cboRouting_GP.SelectedIndex == 0)
            {
                #region 1. 교정 조회 설정(SQL)
                //--교정
                sql1 = string.Format(" SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   (   ");
                sql1 += string.Format("             SELECT  TO_CHAR(TO_DATE(A.WORK_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS WORK_DDTT"); //--작업시각
                sql1 += string.Format("                    ,B.HEAT                AS HEAT "); //--HEAT
                sql1 += string.Format("                    ,B.STEEL               AS STEEL "); //--STEEL
                sql1 += string.Format("                    ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");//--STEEL_NM
                sql1 += string.Format("                    ,B.POC_NO                 AS POC_NO "); //--POC
                sql1 += string.Format("                    ,B.TOP_ROLL_GAP_1      AS COL_01O "); //--상롤 GAP 1(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_GAP_1      AS COL_01W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_GAP_2      AS COL_02O  "); //--상롤 GAP 2(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_GAP_2      AS COL_02W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_GAP_3      AS COL_03O  "); //--상롤 GAP 3(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_GAP_3      AS COL_03W  "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_GAP_4      AS COL_04O  ");// --상롤 GAP 4(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_GAP_4      AS COL_04W  "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_GAP_5      AS COL_05O  "); //--상롤 GAP 5(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_GAP_5      AS COL_05W  "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_GAP_6      AS COL_06O  "); //--상롤 GAP 6(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_GAP_6      AS COL_06W  "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE_1    AS COL_07O "); //--상롤 각도 1(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_1    AS COL_07W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE_2    AS COL_08O  "); //--상롤 각도 2(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_2    AS COL_08W  "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE_3    AS COL_09O  "); //--상롤 각도 3(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_3    AS COL_09W  "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE_4    AS COL_10O "); //--상롤 각도 4(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_4    AS COL_10W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE_5    AS COL_11O "); //--상롤 각도 5(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_5    AS COL_11W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE_6    AS COL_12O "); //--상롤 각도 6(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_6    AS COL_12W  "); //--(실적)
                sql1 += string.Format("                    ,B.BOT_ROLL_ANGLE_1    AS COL_13O "); //--하롤 각도 1(지시)
                sql1 += string.Format("                    ,A.BOT_ROLL_ANGLE_1    AS COL_13W "); //--(실적)
                sql1 += string.Format("                    ,B.BOT_ROLL_ANGLE_2    AS COL_14O "); //--하롤 각도 2(지시)
                sql1 += string.Format("                    ,A.BOT_ROLL_ANGLE_2    AS COL_14W "); //--(실적)
                sql1 += string.Format("                    ,B.BOT_ROLL_ANGLE_3    AS COL_15O "); //--하롤 각도 3(지시)
                sql1 += string.Format("                    ,A.BOT_ROLL_ANGLE_3    AS COL_15W  "); //--(실적)
                sql1 += string.Format("                    ,0                     AS COL_16O  "); //--INLET ROLL 전류 (지시)
                sql1 += string.Format("                    ,A.INLET_ROLL_CURRENT  AS COL_16W  "); //--(실적)
                sql1 += string.Format("                    ,0                     AS COL_17O  ");// --MID ROLL 전류   (지시)
                sql1 += string.Format("                    ,A.MID_ROLL_CURRENT    AS COL_17W  ");// --(실적)
                sql1 += string.Format("                    ,0                     AS COL_18O  ");// --OUTLET ROLL 전류(지시)
                sql1 += string.Format("                    ,A.OUTLET_ROLL_CURRENT AS COL_18W  "); //--(실적)
                sql1 += string.Format("                    ,B.THREAD_SPEED        AS COL_19O "); //--THREAD SPEED(지시)
                sql1 += string.Format("                    ,A.THREAD_SPEED        AS COL_19W "); //--(실적)
                sql1 += string.Format("                    ,B.MACHINE_SPEED       AS COL_20O "); //--MACHINE SPEED(지시)
                sql1 += string.Format("                    ,A.MACHINE_SPEED       AS COL_20W "); //--(실적)
                sql1 += string.Format("             FROM    TB_STR_OPERINFO_NO1 A ");
                sql1 += string.Format("                    ,TB_CR_INPUT_WR  C ");
                sql1 += string.Format("                    ,TB_STR_PLC_ORD_NO1  B ");
                sql1 += string.Format("             WHERE  A.MILL_NO     = C.MILL_NO ");
                sql1 += string.Format("             AND    C.POC_NO      = B.POC_NO" );  
                sql1 += string.Format("             AND    '{0}'  = '#1' ",cd_id);  //:P_LINE_GP 
                sql1 += string.Format("             AND    '{0}'  = 'A1' ", cd_id2); //:P_ROUTING_CD 
                sql1 += string.Format("             AND    A.WORK_DDTT  BETWEEN {0} AND {1} ", start_date,end_date);//:P_FR_DDTT AND :P_TO_DDTT
                sql1 += string.Format("        UNION ");
                sql1 += string.Format("             SELECT  TO_CHAR(TO_DATE(A.WORK_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS WORK_DDTT "); //--작업시각
                sql1 += string.Format("                    ,B.HEAT                AS HEAT "); //--HEAT
                sql1 += string.Format("                    ,B.STEEL               AS STEEL "); //--STEEL
                sql1 += string.Format("                    ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");//--STEEL_NM
                sql1 += string.Format("                    ,B.POC_NO                 AS POC_NO "); //--POC
                sql1 += string.Format("                    ,B.TOP_ROLL_POS            AS COL_01O "); //--상롤 위치(지시)
                sql1 += string.Format("                    ,a.TOP_ROLL_POS_ACTV       AS COL_01W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE          AS COL_02O "); //--상롤 각도(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_ACTV     AS COL_02W "); //--(실적)
                sql1 += string.Format("                    ,B.BOT_ROLL_ANGLE          AS COL_03O "); //--하롤 각도(지시)
                sql1 += string.Format("                    ,A.BOT_ROLL_ANGLE_ACTV     AS COL_03W "); //--(실적)
                sql1 += string.Format("                    ,B.UP_DOWN_LINEAR_POS      AS COL_04O "); //--상하가이드의위치(지시)
                sql1 += string.Format("                    ,A.UP_DOWN_LINEAR_POS_ACTV AS COL_04W "); //--(실적)
                sql1 += string.Format("                    ,B.FRONT_LINEAR_GUIDE_POS  AS COL_05O "); //--전후면의가이드위치(지시)
                sql1 += string.Format("                    ,A.FRONT_LINEAR_POS_ACTV   AS COL_05W "); //--(실적)
                sql1 += string.Format("                    ,B.REAL_LINEAR_GUIDE_POS   AS COL_06O "); //--진입롤모터주파수(Hz)(지시)
                sql1 += string.Format("                    ,A.REAL_LINEAR_POS_ACTV    AS COL_06W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_MTR_HZ         AS COL_07O "); //--상롤모터주파수(Hz)(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_MTR_RPM_ACTV   AS COL_07W "); //--(실적)
                sql1 += string.Format("                    ,B.BOT_ROLL_MTR_HZ         AS COL_08O "); //--출구롤모터주파수(Hz)(지시)
                sql1 += string.Format("                    ,A.BOT_ROLL_MTR_RPM_ACTV   AS COL_08W "); //--(실적)
                sql1 += string.Format("                    ,NULL                      AS COL_09O ");
                sql1 += string.Format("                    ,NULL                      AS COL_09W ");
                sql1 += string.Format("                    ,NULL                      AS COL_10O ");
                sql1 += string.Format("                    ,NULL                      AS COL_10W ");
                sql1 += string.Format("                    ,NULL                      AS COL_11O ");
                sql1 += string.Format("                    ,NULL                      AS COL_11W ");
                sql1 += string.Format("                    ,NULL                      AS COL_12O ");
                sql1 += string.Format("                    ,NULL                      AS COL_12W ");
                sql1 += string.Format("                    ,NULL                      AS COL_13O ");
                sql1 += string.Format("                    ,NULL                      AS COL_13W ");
                sql1 += string.Format("                    ,NULL                      AS COL_14O ");
                sql1 += string.Format("                    ,NULL                      AS COL_14W ");
                sql1 += string.Format("                    ,NULL                      AS COL_15O ");
                sql1 += string.Format("                    ,NULL                      AS COL_15W ");
                sql1 += string.Format("                    ,NULL                      AS COL_16O ");
                sql1 += string.Format("                    ,NULL                      AS COL_16W ");
                sql1 += string.Format("                    ,NULL                      AS COL_17O ");
                sql1 += string.Format("                    ,NULL                      AS COL_17W ");
                sql1 += string.Format("                    ,NULL                      AS COL_18O ");
                sql1 += string.Format("                    ,NULL                      AS COL_18W ");
                sql1 += string.Format("                    ,NULL                      AS COL_19O ");
                sql1 += string.Format("                    ,NULL                      AS COL_19W ");
                sql1 += string.Format("                    ,NULL                      AS COL_20O ");
                sql1 += string.Format("                    ,NULL                      AS COL_20W ");
                sql1 += string.Format("             FROM    TB_STR_OPERINFO_NO2 A ");
                sql1 += string.Format("                    ,TB_STR_PLC_ORD_NO2  B ");
                sql1 += string.Format("             WHERE  A.MILL_NO     = B.MILL_NO ");
                sql1 += string.Format("             AND     '{0}' = '#2' ",cd_id); //:P_LINE_GP
                sql1 += string.Format("             AND     '{0}' = 'A1' ",cd_id2); //:P_ROUTING_CD
                sql1 += string.Format("             AND    A.WORK_DDTT  BETWEEN {0} AND {1} ", start_date, end_date);//:P_FR_DDTT AND :P_TO_DDTT
                sql1 += string.Format("        UNION ");
                sql1 += string.Format("             SELECT  TO_CHAR(TO_DATE(A.WORK_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS WORK_DDTT "); //--작업시각
                sql1 += string.Format("                    ,B.HEAT                AS HEAT "); //--HEAT
                sql1 += string.Format("                    ,B.STEEL               AS STEEL "); //--STEEL
                sql1 += string.Format("                    ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");//--STEEL_NM
                sql1 += string.Format("                    ,B.POC_NO                 AS POC_NO "); //--POC
                sql1 += string.Format("                    ,B.TOP_ROLL_POS            AS COL_01O "); //--상롤 위치(지시)
                sql1 += string.Format("                    ,a.TOP_ROLL_POS_ACTV       AS COL_01W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_ANGLE          AS COL_02O "); //--상롤 각도(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_ANGLE_ACTV     AS COL_02W "); //--(실적)
                sql1 += string.Format("                    ,B.BOT_ROLL_ANGLE          AS COL_03O "); //--하롤 각도(지시)
                sql1 += string.Format("                    ,A.BOT_ROLL_ANGLE_ACTV     AS COL_03W "); //--(실적)
                sql1 += string.Format("                    ,B.UP_DOWN_LINEAR_POS      AS COL_04O "); //--상하가이드의위치(지시)
                sql1 += string.Format("                    ,A.UP_DOWN_LINEAR_POS_ACTV AS COL_04W "); //--(실적)
                sql1 += string.Format("                    ,B.FRONT_LINEAR_GUIDE_POS  AS COL_05O "); //--전후면의가이드위치(지시)
                sql1 += string.Format("                    ,A.FRONT_LINEAR_POS_ACTV   AS COL_05W "); //--(실적)
                sql1 += string.Format("                    ,B.REAL_LINEAR_GUIDE_POS   AS COL_06O "); //--진입롤모터주파수(Hz)(지시)
                sql1 += string.Format("                    ,A.REAL_LINEAR_POS_ACTV    AS COL_06W "); //--(실적)
                sql1 += string.Format("                    ,B.TOP_ROLL_MTR_HZ         AS COL_07O "); //--상롤모터주파수(Hz)(지시)
                sql1 += string.Format("                    ,A.TOP_ROLL_MTR_RPM_ACTV   AS COL_07W "); //--(실적)
                sql1 += string.Format("                    ,B.BOT_ROLL_MTR_HZ         AS COL_08O "); //--출구롤모터주파수(Hz)(지시)
                sql1 += string.Format("                    ,A.BOT_ROLL_MTR_RPM_ACTV   AS COL_08W "); //--(실적)
                sql1 += string.Format("                    ,NULL                      AS COL_09O ");
                sql1 += string.Format("                    ,NULL                      AS COL_09W ");
                sql1 += string.Format("                    ,NULL                      AS COL_10O ");
                sql1 += string.Format("                    ,NULL                      AS COL_10W ");
                sql1 += string.Format("                    ,NULL                      AS COL_11O ");
                sql1 += string.Format("                    ,NULL                      AS COL_11W ");
                sql1 += string.Format("                    ,NULL                      AS COL_12O ");
                sql1 += string.Format("                    ,NULL                      AS COL_12W ");
                sql1 += string.Format("                    ,NULL                      AS COL_13O ");
                sql1 += string.Format("                    ,NULL                      AS COL_13W ");
                sql1 += string.Format("                    ,NULL                      AS COL_14O ");
                sql1 += string.Format("                    ,NULL                      AS COL_14W ");
                sql1 += string.Format("                    ,NULL                      AS COL_15O ");
                sql1 += string.Format("                    ,NULL                      AS COL_15W ");
                sql1 += string.Format("                    ,NULL                      AS COL_16O ");
                sql1 += string.Format("                    ,NULL                      AS COL_16W ");
                sql1 += string.Format("                    ,NULL                      AS COL_17O ");
                sql1 += string.Format("                    ,NULL                      AS COL_17W ");
                sql1 += string.Format("                    ,NULL                      AS COL_18O ");
                sql1 += string.Format("                    ,NULL                      AS COL_18W ");
                sql1 += string.Format("                    ,NULL                      AS COL_19O ");
                sql1 += string.Format("                    ,NULL                      AS COL_19W ");
                sql1 += string.Format("                    ,NULL                      AS COL_20O ");
                sql1 += string.Format("                    ,NULL                      AS COL_20W ");
                sql1 += string.Format("             FROM    TB_STR_OPERINFO_NO3 A ");
                sql1 += string.Format("                    ,TB_STR_PLC_ORD_NO3  B ");
                sql1 += string.Format("             WHERE  A.MILL_NO     = B.MILL_NO ");
                sql1 += string.Format("             AND    '{0}' = '#3' ",cd_id); //:P_LINE_GP
                sql1 += string.Format("             AND    '{0}' = 'A1' ", cd_id2); //:P_ROUTING_CD
                sql1 += string.Format("             AND    A.WORK_DDTT  BETWEEN {0} AND {1} ", start_date, end_date);//:P_FR_DDTT AND :P_TO_DDTT
                sql1 += string.Format("             ORDER BY 1 ");
                sql1 += string.Format("        ) X ");
                #endregion 1. 교정 조회 설정(SQL)
            }

            if (cboRouting_GP.SelectedIndex == 1)
            {
                #region 2. 면취 조회 설정(SQL)
                //--면취
                sql1 += string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   (        ");
                sql1 += string.Format("        SELECT  TO_CHAR(TO_DATE(A.WORK_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS WORK_DDTT "); //--작업시각
                sql1 += string.Format("               ,B.HEAT                AS HEAT "); //--HEAT
                sql1 += string.Format("               ,B.STEEL               AS STEEL "); //--STEEL
                sql1 += string.Format("               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");//--STEEL_NM
                sql1 += string.Format("               ,B.POC_NO                 AS POC_NO "); //--POC
                sql1 += string.Format("               ,0                 AS COL_01O   "); //--SERVO 1(지시)
                sql1 += string.Format("               ,A.SERVO_ACTV_1    AS COL_01W   "); //--(실적)
                sql1 += string.Format("               ,0                 AS COL_02O   "); //--SERVO 2(지시)
                sql1 += string.Format("               ,A.SERVO_ACTV_2    AS COL_02W   "); //--(실적)
                sql1 += string.Format("               ,B.SCREW_1         AS COL_03O   "); //--SCREW 1(지시)
                sql1 += string.Format("               ,A.SCREW_ACTV_1    AS COL_03W   "); //--(실적)
                sql1 += string.Format("               ,B.SCREW_2         AS COL_04O   "); //--SCREW 2(지시)
                sql1 += string.Format("               ,A.SCREW_ACTV_2    AS COL_04W   "); //--(실적)
                sql1 += string.Format("               ,B.RT_1            AS COL_05O   "); //--RT 1(지시)
                sql1 += string.Format("               ,A.RT_ACTV_1       AS COL_05W   "); //--(실적)
                sql1 += string.Format("               ,B.RT_2            AS COL_06O   "); //--RT 2(지시)
                sql1 += string.Format("               ,A.RT_ACTV_2       AS COL_06W   "); //--(실적)
                sql1 += string.Format("               ,B.RT_EXIT         AS COL_07O   ");// --RT EXIT(지시)
                sql1 += string.Format("               ,A.RT_ACTV_EXIT    AS COL_07W   "); //--(실적)
                sql1 += string.Format("               ,0                 AS COL_08O   ");// --STOPPER 1(지시)
                sql1 += string.Format("               ,A.STOPPER_1       AS COL_08W   "); //--(실적)
                sql1 += string.Format("               ,0                 AS COL_09O   ");// --받침대 1(지시)
                sql1 += string.Format("               ,A.PROP_1          AS COL_09W   "); //--(실적)
                sql1 += string.Format("               ,0                 AS COL_10O   "); //--소재 간격 1(지시)
                sql1 += string.Format("               ,A.MATERIAL_GAP_1  AS COL_10W   "); //--(실적)
                sql1 += string.Format("               ,0                 AS COL_11O   "); //--STOPPER 2(지시)
                sql1 += string.Format("               ,A.STOPPER_2       AS COL_11W   "); //--(실적)
                sql1 += string.Format("               ,0                 AS COL_12O   "); //--받침대 2(지시)
                sql1 += string.Format("               ,A.PROP_2          AS COL_12W   "); //--(실적)
                sql1 += string.Format("               ,0                 AS COL_13O   ");// --소재 간격 2(지시)
                sql1 += string.Format("               ,A.MATERIAL_GAP_2  AS COL_13W   "); //--(실적)
                sql1 += string.Format("               ,NULL              AS COL_14O   ");
                sql1 += string.Format("               ,NULL              AS COL_14W   ");
                sql1 += string.Format("               ,NULL              AS COL_15O   ");
                sql1 += string.Format("               ,NULL              AS COL_15W   ");
                sql1 += string.Format("               ,NULL              AS COL_16O   ");
                sql1 += string.Format("               ,NULL              AS COL_16W   ");
                sql1 += string.Format("             FROM    TB_CHF_OPERINFO_NO1 A ");
                sql1 += string.Format("                    ,TB_CR_INPUT_WR  C ");
                sql1 += string.Format("                    ,TB_CHF_PLC_ORD_NO1  B ");
                sql1 += string.Format("             WHERE  A.MILL_NO     = C.MILL_NO ");
                sql1 += string.Format("             AND    C.POC_NO      = B.POC_NO");
                sql1 += string.Format("        AND     '{0}' = '#1'  ",cd_id);//:P_LINE_GP
                sql1 += string.Format("        AND     '{0}' = 'B1'  ",cd_id2); //:P_ROUTING_CD
                sql1 += string.Format("        AND    A.WORK_DDTT  BETWEEN {0} AND {1} ", start_date, end_date);//:P_FR_DDTT AND :P_TO_DDTT
                sql1 += string.Format("        UNION  ");
                sql1 += string.Format("        SELECT  TO_CHAR(TO_DATE(A.WORK_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS WORK_DDTT "); //--작업시각
                sql1 += string.Format("               ,B.HEAT                AS HEAT "); //--HEAT
                sql1 += string.Format("               ,B.STEEL               AS STEEL "); //--STEEL
                sql1 += string.Format("               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");//--STEEL_NM
                sql1 += string.Format("               ,B.POC_NO                 AS POC_NO "); //--POC
                sql1 += string.Format("               ,B.KICKER_SIZE_1        AS COL_01O  "); //--키커 사이즈 조정1   (지시)
                sql1 += string.Format("               ,A.KICKER_SIZE_ACTV_1   AS COL_01W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_H_1     AS COL_02O  "); //--롤러 컨베어 HZ 고속1(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_1  AS COL_02W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_S_1     AS COL_03O  "); //--롤러 컨베어 HZ 저속1(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_1  AS COL_03W  "); //--(실적)
                sql1 += string.Format("               ,B.SCREW_FEED_HZ_1      AS COL_04O  "); //--스크류 구동 HZ1     (지시)
                sql1 += string.Format("               ,A.SCREW_FEED_HZ_ACTV_1 AS COL_04W  "); //--(실적)
                sql1 += string.Format("               ,B.GRIND_HZ_1           AS COL_05O  "); //--그라인딩 구동 Hz1   (지시)
                sql1 += string.Format("               ,A.GRIND_HZ_ACTV_1      AS COL_05W  "); //--(실적)
                sql1 += string.Format("               ,B.STOPPER_1            AS COL_06O  "); //--스토퍼1(지시)
                sql1 += string.Format("               ,A.STOPPER_ACTV_1       AS COL_06W  "); //--(실적)
                sql1 += string.Format("               ,B.GUIDE_1              AS COL_07O  "); //--가이드1(지시)
                sql1 += string.Format("               ,A.GUIDE_ACTV_1         AS COL_07W  "); //--(실적)
                sql1 += string.Format("               ,B.SERVO_POS_1          AS COL_08O  "); //--서보 위치1(지시)
                sql1 += string.Format("               ,A.SERVO_ACTV_1         AS COL_08W  "); //--(실적)
                sql1 += string.Format("               ,B.KICKER_SIZE_2        AS COL_09O  "); //--키커 사이즈 조정2   (지시)
                sql1 += string.Format("               ,A.KICKER_SIZE_ACTV_2   AS COL_09W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_H_2     AS COL_10O  "); //--롤러 컨베어 HZ 고속2(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_2  AS COL_10W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_S_2     AS COL_11O  "); //--롤러 컨베어 HZ 저속2(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_2  AS COL_11W  "); //--(실적)
                sql1 += string.Format("               ,B.SCREW_FEED_HZ_2      AS COL_12O  "); //--스크류 구동 HZ2     (지시)
                sql1 += string.Format("               ,A.SCREW_FEED_HZ_ACTV_2 AS COL_12W  "); //--(실적)
                sql1 += string.Format("               ,B.GRIND_HZ_2           AS COL_13O  "); //--그라인딩 구동 Hz2   (지시)
                sql1 += string.Format("               ,A.GRIND_HZ_ACTV_2      AS COL_13W  "); //--(실적)
                sql1 += string.Format("               ,B.STOPPER_2            AS COL_14O  "); //--스토퍼2(지시)
                sql1 += string.Format("               ,A.STOPPER_ACTV_2       AS COL_14W  "); //--(실적)
                sql1 += string.Format("               ,B.GUIDE_2              AS COL_15O  "); //--가이드 2(지시)
                sql1 += string.Format("               ,A.GUIDE_ACTV_2         AS COL_15W  "); //--(실적)
                sql1 += string.Format("               ,B.SERVO_POS_2          AS COL_16O  "); //--서보 위치2(지시)
                sql1 += string.Format("               ,A.SERVO_ACTV_2         AS COL_16W  "); //--(실적)
                sql1 += string.Format("        FROM    TB_CHF_OPERINFO_NO2 A              ");
                sql1 += string.Format("               ,TB_CHF_PLC_ORD_NO2 B               ");
                sql1 += string.Format("        WHERE  A.MILL_NO     = B.MILL_NO           ");
                sql1 += string.Format("        AND      '{0}'  = '#2'                ",cd_id);//:P_LINE_GP
                sql1 += string.Format("        AND      '{0}'  = 'B1'                ",cd_id2);//:P_ROUTING_CD
                sql1 += string.Format("        AND    A.WORK_DDTT  BETWEEN {0} AND {1} ", start_date, end_date);//:P_FR_DDTT AND :P_TO_DDTT
                sql1 += string.Format("        UNION   ");
                sql1 += string.Format("        SELECT  TO_CHAR(TO_DATE(A.WORK_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS WORK_DDTT  "); //--작업시각
                sql1 += string.Format("               ,B.HEAT                AS HEAT "); //--HEAT
                sql1 += string.Format("               ,B.STEEL               AS STEEL "); //--STEEL
                sql1 += string.Format("               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");//--STEEL_NM
                sql1 += string.Format("               ,B.POC_NO                 AS POC_NO "); //--POC
                sql1 += string.Format("               ,B.KICKER_SIZE_1        AS COL_01O  "); //--키커 사이즈 조정1   (지시)
                sql1 += string.Format("               ,A.KICKER_SIZE_ACTV_1   AS COL_01W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_H_1     AS COL_02O  "); // --롤러 컨베어 HZ 고속1(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_1  AS COL_02W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_S_1     AS COL_03O  "); //--롤러 컨베어 HZ 저속1(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_1  AS COL_03W  "); //--(실적)
                sql1 += string.Format("               ,B.SCREW_FEED_HZ_1      AS COL_04O  "); //--스크류 구동 HZ1     (지시)
                sql1 += string.Format("               ,A.SCREW_FEED_HZ_ACTV_1 AS COL_04W  "); //--(실적)
                sql1 += string.Format("               ,B.GRIND_HZ_1           AS COL_05O  "); //--그라인딩 구동 Hz1   (지시)
                sql1 += string.Format("               ,A.GRIND_HZ_ACTV_1      AS COL_05W  "); //--(실적)
                sql1 += string.Format("               ,B.STOPPER_1            AS COL_06O  "); //--스토퍼1(지시)
                sql1 += string.Format("               ,A.STOPPER_ACTV_1       AS COL_06W  "); //--(실적)
                sql1 += string.Format("               ,B.GUIDE_1              AS COL_07O  "); //--가이드1(지시)
                sql1 += string.Format("               ,A.GUIDE_ACTV_1         AS COL_07W  "); //--(실적)
                sql1 += string.Format("               ,B.SERVO_POS_1          AS COL_08O  "); //--서보 위치1(지시)
                sql1 += string.Format("               ,A.SERVO_ACTV_1         AS COL_08W  "); //--(실적)
                sql1 += string.Format("               ,B.KICKER_SIZE_2        AS COL_09O  "); //--키커 사이즈 조정2   (지시)
                sql1 += string.Format("               ,A.KICKER_SIZE_ACTV_2   AS COL_09W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_H_2     AS COL_10O  "); //--롤러 컨베어 HZ 고속2(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_2  AS COL_10W  "); //--(실적)
                sql1 += string.Format("               ,B.ROLLER_CV_HZ_S_2     AS COL_11O  "); //--롤러 컨베어 HZ 저속2(지시)
                sql1 += string.Format("               ,A.ROLLER_CV_HZ_ACTV_2  AS COL_11W  "); //--(실적)
                sql1 += string.Format("               ,B.SCREW_FEED_HZ_2      AS COL_12O  "); //--스크류 구동 HZ2     (지시)
                sql1 += string.Format("               ,A.SCREW_FEED_HZ_ACTV_2 AS COL_12W  "); //--(실적)
                sql1 += string.Format("               ,B.GRIND_HZ_2           AS COL_13O  "); //--그라인딩 구동 Hz2   (지시)
                sql1 += string.Format("               ,A.GRIND_HZ_ACTV_2      AS COL_13W  "); //--(실적)
                sql1 += string.Format("               ,B.STOPPER_2            AS COL_14O  "); //--스토퍼2(지시)
                sql1 += string.Format("               ,A.STOPPER_ACTV_2       AS COL_14W  "); //--(실적)
                sql1 += string.Format("               ,B.GUIDE_2              AS COL_15O  "); //--가이드 2(지시)
                sql1 += string.Format("               ,A.GUIDE_ACTV_2         AS COL_15W  "); //--(실적)
                sql1 += string.Format("               ,B.SERVO_POS_2          AS COL_16O  "); //--서보 위치2(지시)
                sql1 += string.Format("               ,A.SERVO_ACTV_2         AS COL_16W  "); //--(실적)
                sql1 += string.Format("        FROM    TB_CHF_OPERINFO_NO3 A              ");
                sql1 += string.Format("               ,TB_CHF_PLC_ORD_NO3 B ");
                sql1 += string.Format("        WHERE  A.MILL_NO     = B.MILL_NO ");
                sql1 += string.Format("        AND     '{0}'   = '#3' ",cd_id); //:P_LINE_GP
                sql1 += string.Format("        AND     '{0}'   = 'B1' ",cd_id2); //:P_ROUTING_CD
                sql1 += string.Format("        AND    A.WORK_DDTT  BETWEEN {0} AND {1} ", start_date, end_date);//:P_FR_DDTT AND :P_TO_DDTT
                sql1 += string.Format("        ORDER BY 1 ");
                sql1 += string.Format("        ) X ");
                #endregion 2. 면취 조회 설정(SQL)
            }
            if (cboRouting_GP.SelectedIndex == 2)
            {
                #region 3. 성분분석 조회 설정(SQL)
                //--성분분석(L3)
                sql1 += string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   (        ");
                sql1 += string.Format("SELECT  TO_CHAR(EXIT_DDTT,'YYYY-MM-DD HH24:MI:SS') AS WORK_DDTT   "); //--작업시각
                sql1 += string.Format("       ,B.HEAT                AS HEAT "); //--HEAT
                sql1 += string.Format("       ,B.STEEL               AS STEEL "); //--STEEL
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");//--STEEL_NM
                sql1 += string.Format("       ,B.POC_NO                 AS POC_NO "); //--POC
                sql1 += string.Format("       ,B.CHM_VAL_C   AS COL_01O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_C   AS COL_01W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_SI  AS COL_02O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_SI  AS COL_02W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_MN  AS COL_03O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_MN  AS COL_03W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_NI  AS COL_04O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_NI  AS COL_04W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_CR  AS COL_05O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_CR  AS COL_05W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_MO  AS COL_06O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_MO  AS COL_06W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_V   AS COL_07O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_V   AS COL_07W  ");// --실적
                sql1 += string.Format("       ,B.CHM_VAL_TI  AS COL_08O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_TI  AS COL_08W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_NB  AS COL_09O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_NB  AS COL_09W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_CU  AS COL_10O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_CU  AS COL_10W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_ZR  AS COL_11O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_ZR  AS COL_11W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_P   AS COL_12O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_P   AS COL_12W  "); //--실적
                sql1 += string.Format("       ,B.CHM_VAL_S   AS COL_13O  "); //--지시
                sql1 += string.Format("       ,A.CHM_VAL_S   AS COL_13W  "); //--실적
                sql1 += string.Format("FROM    TB_CHM_WR      A          ");
                sql1 += string.Format("       ,TB_CHM_PLC_ORD B          ");
                sql1 += string.Format("WHERE  A.MILL_NO   = B.MILL_NO    ");
                sql1 += string.Format("AND    A.LINE_GP   = '{0}' ",cd_id);//:P_LINE_GP
                sql1 += string.Format("AND    '{0}' = 'L3' ",cd_id2);       //:P_ROUTING_CD
                sql1 += string.Format("AND    TO_CHAR(EXIT_DDTT,'YYYYMMDDHH24MISS') BETWEEN {0} AND {1} ", start_date, end_date);//:P_FR_DDTT AND :P_TO_DDTT
                sql1 += string.Format("ORDER BY 1 ");
                sql1 += string.Format(") X ");
                #endregion 3. 성분분석 조회 설정(SQL)
            }
            olddt = cd.FindDataTable(sql1);

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain1.SetDataBinding(olddt, null, true);
            grdMain2.SetDataBinding(olddt, null, true);
            grdMain3.SetDataBinding(olddt, null, true);
            grdMain4.SetDataBinding(olddt, null, true);
            grdMain5.SetDataBinding(olddt, null, true);
            grdMain6.SetDataBinding(olddt, null, true);
            grdMain7.SetDataBinding(olddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain1.Row = -1;
            grdMain2.Row = -1;
            grdMain3.Row = -1;
            grdMain4.Row = -1;
            grdMain5.Row = -1;
            grdMain6.Row = -1;
            grdMain7.Row = -1;
        }
        #endregion 조회 SQL 설정

        #region 이벤트 설정
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            //cbo_Line_GP.SelectedIndex = 0(1번라인),cbo_Line_GP.SelectedIndex = 1(2번라인),cbo_Line_GP.SelectedIndex = 2(3번라인)
            //cboRouting_GP.SelectedIndex = 0(A1,교정),cboRouting_GP.SelectedIndex = 0(B1,면취),cboRouting_GP.SelectedIndex = 0(L3,성분분석)

            if (cboRouting_GP.SelectedIndex == 0 && cbo_Line_GP.SelectedIndex == 0) grdMain1.BringToFront();

            if (cboRouting_GP.SelectedIndex == 0 && cbo_Line_GP.SelectedIndex == 1) grdMain2.BringToFront();

            if (cboRouting_GP.SelectedIndex == 0 && cbo_Line_GP.SelectedIndex == 2) grdMain3.BringToFront();

            if (cboRouting_GP.SelectedIndex == 1 && cbo_Line_GP.SelectedIndex == 0) grdMain4.BringToFront();

            if (cboRouting_GP.SelectedIndex == 1 && cbo_Line_GP.SelectedIndex == 1) grdMain5.BringToFront();

            if (cboRouting_GP.SelectedIndex == 1 && cbo_Line_GP.SelectedIndex == 2) grdMain6.BringToFront();

            if (cboRouting_GP.SelectedIndex == 2 ) grdMain7.BringToFront();

            setDataBinding();

        }


        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, grdMain1);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_date = start_dt.Value;
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            end_date = end_dt.Value;
        }

        private void cbo_Line_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id = ((DictionaryList)cbo_Line_GP.SelectedItem).fnValue;
            ck.Line_gp = cd_id;
        }

        private void cboRouting_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id2 = ((DictionaryList)cboRouting_GP.SelectedItem).fnValue;
        }

        #endregion 이벤트 설정
    }
}
