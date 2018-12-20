﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Windows.Forms;
using SystemControlClassLibrary.UC.sub_UC;
using static SystemControlClassLibrary.Order.CrtInOrdCre;

namespace SystemControlClassLibrary.Results
{
    public partial class BNDRsltMgmt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable moddt;

        DataTable par_dt;

        DataTable temp_dt;

        DataTable grdMain_dt;

        private string steel_id = "";

        private string item = "";
        clsStyle cs = new clsStyle();

        // 유저컨트롤 생성
        private UC_BundleNo uC_BundleNo1;
        private UC_POC uC_POC1;
        private UC_Item_size uC_Item_size1;
        private UC_Line_gp uC_Line_gp1;
        private UC_DelYN uC_DelYN1;
        private UC_Length uC_Length1;
        private UC_Work_Day uC_Work_Day1;
        private UC_BND_PCS uC_BND_PCS1;
        private UC_CalWgt uC_CalWgt1;
        private UC_Work_Team uC_Work_Team1;
        private UC_Work_Type uC_Work_Type1;
        private UC_Time uC_Time2;
        private UC_Time uC_Time1;
        private UC_SteelNM uC_SteelNM1;
        private UC_HEAT uC_HEAT1;

        private UC_BND_PCS uC_POC_SEQ;

        DateTime start_dt;
        DateTime end_dt;

        private string line_gp = "";
        private string bundle_no = "";
        private string scrAuth;
        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public BNDRsltMgmt(string _line_gp, string _bundle_no, string _scrAuth)
        {
            line_gp = _line_gp;
            bundle_no = _bundle_no;
            scrAuth = _scrAuth;

            InitializeComponent();
        }

        private void BNDRsltMgmt_Load(object sender, EventArgs e)
        {
            MinimizeBox = false;
            MaximizeBox = false;
            InitControl();

            MakeGrdData();

            moddt = grdMain_dt;

            SetDataBinding();
        }
        #endregion 로딩, 생성자 설정

        #region Init 설정
        private void InitGridData_Main()
        {
            moddt = grdMain_dt;
            grdMain.SetDataBinding(moddt, null, true);
        }

        private void MakeGrdData()
        {
            grdMain_dt = vf.CreateDataTable(grdMain);
        }

        private void InitControl()
        {
            clsStyle.Style.InitPanel(panel1);

            clsStyle.Style.InitButton(btnClose);
            clsStyle.Style.InitButton(btnSave);
            clsStyle.Style.InitButton(btnNew);
            clsStyle.Style.InitButton(btnDelRow);
            clsStyle.Style.InitButton(btnRowAdd);

            //scrAuth button 권한 적용
            ApplyAuth();


            #region 유저컨트롤 설정
            int location_x = 10;
            int location_y = 8;

            //컨트롤 생성 및 배치
            uC_BundleNo1 = new UC_BundleNo();
            uC_BundleNo1.BackColor = System.Drawing.Color.Transparent;
            uC_BundleNo1.BundleNo = bundle_no;
            uC_BundleNo1.Location = new System.Drawing.Point(722 + location_x, 12 + location_y);
            uC_BundleNo1.Name = "uC_BundleNo1";
            //uC_BundleNo1.ReadOnly = true;
            uC_BundleNo1.ReadOnly = false;
            uC_BundleNo1.LabelForeColor = System.Drawing.Color.Blue;
            uC_BundleNo1.Size = new System.Drawing.Size(270, 27);
            uC_BundleNo1.TabIndex = 5;

            uC_POC1 = new UC_POC();
            uC_POC1.BackColor = System.Drawing.Color.Transparent;
            uC_POC1.Location = new System.Drawing.Point(385 + location_x, 12 + location_y);
            uC_POC1.Name = "uC_POC1";
            uC_POC1.POC = "";
            uC_POC1.ReadOnly = false;
            uC_POC1.Size = new System.Drawing.Size(270, 27);
            uC_POC1.TabIndex = 4;
            uC_POC1.ReadOnly = true;

            uC_Item_size1 = new UC_Item_size();
            uC_Item_size1.BackColor = System.Drawing.Color.Transparent;
            uC_Item_size1.ITEM_SIZE = "";
            uC_Item_size1.Location = new System.Drawing.Point(26 + location_x, 45 + location_y);
            uC_Item_size1.Name = "uC_Item_size1";
            uC_Item_size1.ReadOnly = true;
            uC_Item_size1.Size = new System.Drawing.Size(270, 27);
            uC_Item_size1.TabIndex = 2;

            uC_Line_gp1 = new UC_Line_gp();
            uC_Line_gp1.BackColor = System.Drawing.Color.Transparent;
            uC_Line_gp1.Location = new System.Drawing.Point(26 + location_x, 12 + location_y);
            uC_Line_gp1.Name = "uC_Line_gp1";
            uC_Line_gp1.Size = new System.Drawing.Size(270, 27);
            uC_Line_gp1.TabIndex = 1;
            uC_Line_gp1.cb_Enable = false;

            uC_DelYN1 = new UC_DelYN();
            uC_DelYN1.BackColor = System.Drawing.Color.Transparent;
            uC_DelYN1.Location = new System.Drawing.Point(26 + location_x, 144 + location_y);
            uC_DelYN1.Name = "uC_DelYN1";
            uC_DelYN1.LabelForeColor = System.Drawing.Color.Blue;
            uC_DelYN1.Size = new System.Drawing.Size(270, 27);
            uC_DelYN1.TabIndex = 15;

            uC_Length1 = new UC_Length();
            uC_Length1.BackColor = System.Drawing.Color.Transparent;
            uC_Length1.Length = "";
            uC_Length1.Location = new System.Drawing.Point(26 + location_x, 78 + location_y);
            uC_Length1.Name = "uC_Length2";
            uC_Length1.LabelForeColor = System.Drawing.Color.Blue;
            //uC_Length1.ReadOnly = true;
            uC_Length1.Size = new System.Drawing.Size(270, 27);
            uC_Length1.TabIndex = 14;

            uC_Work_Day1 = new UC_Work_Day();
            uC_Work_Day1.BackColor = System.Drawing.Color.Transparent;
            uC_Work_Day1.Location = new System.Drawing.Point(26 + location_x, 111 + location_y);
            uC_Work_Day1.Name = "uC_Work_Day1";
            uC_Work_Day1.LabelForeColor = System.Drawing.Color.Blue;
            uC_Work_Day1.Size = new System.Drawing.Size(270, 27);
            uC_Work_Day1.TabIndex = 13;

            uC_BND_PCS1 = new UC_BND_PCS();
            uC_BND_PCS1.BackColor = System.Drawing.Color.Transparent;
            uC_BND_PCS1.BND_PCS = "";
            uC_BND_PCS1.Location = new System.Drawing.Point(722 + location_x, 144 + location_y);
            uC_BND_PCS1.Name = "uC_BND_PCS1";
            uC_BND_PCS1.ReadOnly = true;
            uC_BND_PCS1.Size = new System.Drawing.Size(270, 27);
            uC_BND_PCS1.TabIndex = 12;

            uC_POC_SEQ = new UC_BND_PCS();
            uC_POC_SEQ.BackColor = System.Drawing.Color.Transparent;
            uC_POC_SEQ.BND_PCS = "";
            uC_POC_SEQ.Location = new System.Drawing.Point(722 + location_x, 144 + location_y);
            uC_POC_SEQ.Name = "uC_PCS_SEQ1";
            uC_POC_SEQ.ReadOnly = true;
            uC_POC_SEQ.Visible = false;

            uC_CalWgt1 = new UC_CalWgt();
            uC_CalWgt1.BackColor = System.Drawing.Color.Transparent;
            uC_CalWgt1.CalWgt = "";
            uC_CalWgt1.Location = new System.Drawing.Point(722 + location_x, 111 + location_y);
            uC_CalWgt1.Name = "uC_CalWgt1";
            uC_CalWgt1.ReadOnly = true;
            uC_CalWgt1.Size = new System.Drawing.Size(270, 27);
            uC_CalWgt1.TabIndex = 11;

            uC_Work_Team1 = new UC_Work_Team();
            uC_Work_Team1.BackColor = System.Drawing.Color.Transparent;
            uC_Work_Team1.Location = new System.Drawing.Point(722 + location_x, 78 + location_y);
            uC_Work_Team1.Name = "uC_Work_Team1";
            uC_Work_Team1.LabelForeColor = System.Drawing.Color.Blue;
            uC_Work_Team1.Size = new System.Drawing.Size(270, 27);
            uC_Work_Team1.TabIndex = 10;
            uC_Work_Team1.Work_Team = "A";

            uC_Work_Type1 = new UC_Work_Type();
            uC_Work_Type1.BackColor = System.Drawing.Color.Transparent;
            uC_Work_Type1.Location = new System.Drawing.Point(722 + location_x, 45 + location_y);
            uC_Work_Type1.Name = "uC_Work_Type1";
            uC_Work_Type1.LabelForeColor = System.Drawing.Color.Blue;
            uC_Work_Type1.Size = new System.Drawing.Size(270, 27);
            uC_Work_Type1.TabIndex = 9;
            uC_Work_Type1.Work_Type = "1";

            uC_Time2 = new UC_Time();
            uC_Time2.BackColor = System.Drawing.Color.Transparent;
            uC_Time2.Location = new System.Drawing.Point(385 + location_x, 144 + location_y);
            uC_Time2.Name = "uC_Time2";
            uC_Time2.LabelForeColor = System.Drawing.Color.Blue;
            uC_Time2.Size = new System.Drawing.Size(270, 27);
            uC_Time2.TabIndex = 8;
            uC_Time2.Work_Time_Text = "종료시각";

            uC_Time1 = new UC_Time();
            uC_Time1.BackColor = System.Drawing.Color.Transparent;
            uC_Time1.Location = new System.Drawing.Point(385 + location_x, 111 + location_y);
            uC_Time1.Name = "uC_Time1";
            uC_Time1.LabelForeColor = System.Drawing.Color.Blue;
            uC_Time1.Size = new System.Drawing.Size(270, 27);
            uC_Time1.TabIndex = 8;
            uC_Time1.Work_Time_Text = "시작시각";

            uC_SteelNM1 = new UC_SteelNM();
            uC_SteelNM1.BackColor = System.Drawing.Color.Transparent;
            uC_SteelNM1.Location = new System.Drawing.Point(385 + location_x, 78 + location_y);
            uC_SteelNM1.Name = "uC_SteelNM1";
            uC_SteelNM1.ReadOnly = true;
            uC_SteelNM1.Size = new System.Drawing.Size(270, 27);
            uC_SteelNM1.SteelNM = "";

            uC_SteelNM1.TabIndex = 7;

            uC_HEAT1 = new UC_HEAT();
            uC_HEAT1.BackColor = System.Drawing.Color.Transparent;
            uC_HEAT1.HEAT = "";
            uC_HEAT1.Location = new System.Drawing.Point(385 + location_x, 45 + location_y);
            uC_HEAT1.Name = "uC_HEAT1";
            uC_HEAT1.ReadOnly = true;
            uC_HEAT1.Size = new System.Drawing.Size(270, 27);
            uC_HEAT1.TabIndex = 6;

            panel1.Controls.Add(uC_DelYN1);
            panel1.Controls.Add(uC_Length1);
            panel1.Controls.Add(uC_Work_Day1);
            panel1.Controls.Add(uC_BND_PCS1);
            panel1.Controls.Add(uC_POC_SEQ); //POC_SEQ

            panel1.Controls.Add(uC_CalWgt1);
            panel1.Controls.Add(uC_Work_Team1);
            panel1.Controls.Add(uC_Work_Type1);
            panel1.Controls.Add(uC_Time2);
            panel1.Controls.Add(uC_Time1);
            panel1.Controls.Add(uC_SteelNM1);
            panel1.Controls.Add(uC_HEAT1);
            panel1.Controls.Add(uC_BundleNo1);
            panel1.Controls.Add(uC_POC1);
            panel1.Controls.Add(uC_Item_size1);
            panel1.Controls.Add(uC_Line_gp1);

            // 초기값 지정
            uC_Line_gp1.Line_GP = line_gp;
            uC_BundleNo1.BundleNo = bundle_no;

            #endregion

            InitGrd_Main();
        }

        private void ApplyAuth()
        {
            string[] arrText = System.Text.RegularExpressions.Regex.Split(scrAuth, ",");
            string sProgramId = arrText[0];
            string InqAcl = arrText[1];
            string RegAcl = arrText[2];
            string ModAcl = arrText[3];
            string DelAcl = arrText[4];

            if (RegAcl == "Y")
            {
                btnNew.Enabled = true;
            }
            else
            {
                btnNew.Enabled = false;
            }

            if (ModAcl == "Y")
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void InitGrd_Main()
        {
            cs.InitGrid_search(grdMain);

            grdMain.AllowEditing = false;
           
            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TIME"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TYPE"].Width = 0;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width -20-10;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width -20;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain.Cols["ZONE_CD"].Width = cs.ZONE_CD_Width-20;
            grdMain.Cols["CHM_GOOD_NG"].Width = cs.Good_NG_Width;
            
            grdMain.Cols["LINE_GP"].Width = 0;
            grdMain.Cols["ROUTING_CD"].Width = 0;
            grdMain.Cols["REWORK_SEQ"].Width = 0;
            grdMain.Cols["BUNDLE_NO"].Width = 0;
            grdMain.Cols["GOOD_YN"].Width = 0;
            grdMain.Cols["GUBUN"].Width = 0;
            grdMain.Cols["POC_SEQ"].Width = 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["PIECE_NO"].TextAlign = cs.PIECE_NO_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["ZONE_CD"].TextAlign = cs.ZONE_CD_TextAlign;
            grdMain.Cols["CHM_GOOD_NG"].TextAlign = cs.GOOD_YN_TextAlign;
            grdMain.Cols["LINE_GP"].TextAlign = cs.LINE_GP_TextAlign;
            grdMain.Cols["ROUTING_CD"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["REWORK_SEQ"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["BUNDLE_NO"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["GOOD_YN"].TextAlign = TextAlignEnum.CenterCenter;

        }
        #endregion Init 설정

        #region SetDataBinding 설정
        private void InitalParameter()
        {
            uC_POC1.POC = "";
            uC_BundleNo1.BundleNo = "";
            uC_Item_size1.ITEM_SIZE = "";
            uC_HEAT1.HEAT = "";
            uC_Length1.Length = "";
            uC_SteelNM1.SteelNM = "";

            steel_id = "";
            item = "";

            uC_Work_Day1.Work_Day = DateTime.Now.Date;
            uC_Time1.Work_Time = DateTime.Now;
            uC_Time2.Work_Time = DateTime.Now;

            uC_CalWgt1.CalWgt = 0.ToString();
            uC_BND_PCS1.BND_PCS = 0.ToString();
            uC_POC_SEQ.BND_PCS = 0.ToString();

            // grd
            InitGridData_Main();
        }

        private void SetupParameter()
        {
            C1FlexGrid grd = grdMain as C1FlexGrid;

            int row_cnt = 0;
            int TheoryWgt = 0;

            if (grd.Rows.Count > 1)
            {
                uC_POC1.POC                 = grd.GetData(1, "POC_NO"   ).ToString();
                uC_Item_size1.ITEM_SIZE     = grd.GetData(1, "ITEM_SIZE").ToString();
                uC_HEAT1.HEAT               = grd.GetData(1, "HEAT"     ).ToString();
                //현재의 근, 조, 일, 시를 가져와서 설정한다.
                uC_Length1.Length           = vf.Format(grd.GetData(1, "LENGTH").ToString(),"#.00");
                uC_SteelNM1.SteelNM         = grd.GetData(1, "STEEL_NM").ToString();

                steel_id                    = grd.GetData(1, "STEEL").ToString();
                item                        = grd.GetData(1, "ITEM").ToString();

                row_cnt = grd.Rows.Count - 1;
                uC_BND_PCS1.BND_PCS = row_cnt.ToString();
                uC_POC_SEQ.BND_PCS = grd.GetData(1, "POC_SEQ").ToString();

                //현재의 근, 조, 일, 시를 가져와서 설정한다. 그리고 가져온 ROW 숫자에 따른 이론중량을 설정한다.
                SetupInitialData(row_cnt);
            }
            else
            {
                uC_POC1.POC = "";
                uC_Item_size1.ITEM_SIZE = "";
                uC_HEAT1.HEAT = "";
                uC_Length1.Length = "";
                uC_SteelNM1.SteelNM = "";

                steel_id = "";
                item = "";

                uC_Work_Day1.Work_Day = DateTime.Now.Date;
                uC_Time1.Work_Time = DateTime.Now;
                uC_Time2.Work_Time = DateTime.Now;

                uC_CalWgt1.CalWgt = 0.ToString();
                uC_BND_PCS1.BND_PCS = 0.ToString();
                uC_POC_SEQ.BND_PCS = 0.ToString();
            }
        }

        private void SetDataBinding()
        {
            InitGridData_Main();

            Setup_Parameter();

            SetDataBinding_grdMain();
        }

        private void SetDataBinding_grdMain()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT  TO_CHAR(ROWNUM) AS L_NO ");
                sql1 += string.Format("       ,'' AS GUBUN ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT ");
                sql1 += string.Format("                MILL_NO ");
                sql1 += string.Format("               ,PIECE_NO ");
                sql1 += string.Format("               ,TO_DATE(MFG_DATE, 'YYYY-MM-DD') AS MFG_DATE ");
                sql1 += string.Format("               ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME ");
                sql1 += string.Format("               ,WORK_TYPE ");
                sql1 += string.Format("               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = A.WORK_TYPE) AS WORK_TYPE_NM ");
                sql1 += string.Format("               ,POC_NO ");
                sql1 += string.Format("               ,HEAT ");
                sql1 += string.Format("               ,STEEL ");
                sql1 += string.Format("               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("               ,ITEM ");
                sql1 += string.Format("               ,ITEM_SIZE ");
                sql1 += string.Format("               ,LENGTH ");
                sql1 += string.Format("               ,ZONE_CD ");
                sql1 += string.Format("               ,LINE_GP ");
                sql1 += string.Format("               ,ROUTING_CD ");
                sql1 += string.Format("               ,REWORK_SEQ ");
                sql1 += string.Format("               ,BUNDLE_NO ");
                sql1 += string.Format("               , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = A.GOOD_YN) AS GOOD_YN ");
                sql1 += string.Format("               ,(SELECT CHM_GOOD_NG FROM TB_CHM_WR  WHERE MILL_NO = A.MILL_NO AND PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                 AND    REWORK_SEQ = A.REWORK_SEQ AND  ROWNUM = 1) AS CHM_GOOD_NG ");//--성분합부
                sql1 += string.Format("               ,POC_SEQ ");
                sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ");
                sql1 += string.Format("        WHERE ROUTING_CD  = 'P3' ");
                sql1 += string.Format("        AND   BUNDLE_NO   = :P_BUNDLE_NO ");
                sql1 += string.Format("        AND   REWORK_SEQ  = ( SELECT MAX(REWORK_SEQ)  FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                              WHERE  MILL_NO    = A.MILL_NO ");
                sql1 += string.Format("                              AND    PIECE_NO   = A.PIECE_NO ");
                sql1 += string.Format("                              AND    LINE_GP    = A.LINE_GP ");
                sql1 += string.Format("                              AND    ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("        ORDER BY 1,2,3 ");
                sql1 += string.Format(") X ");

                string[] parm = new string[1];
                parm[0] = ":P_BUNDLE_NO|" + bundle_no;

                moddt = cd.FindDataTable(sql1, parm);

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        private void Setup_Parameter()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("  SELECT A.LINE_GP ");
                sql1 += string.Format("      ,A.BUNDLE_NO ");
                sql1 += string.Format("      ,A.POC_NO ");
                sql1 += string.Format("      ,A.ITEM ");
                sql1 += string.Format("      ,A.ITEM_SIZE ");
                sql1 += string.Format("      ,A.LENGTH ");
                sql1 += string.Format("      ,A.HEAT ");
                sql1 += string.Format("      ,A.STEEL ");
                sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("      ,A.MFG_DATE ");
                sql1 += string.Format("      ,WORK_START_DDTT     ");
                sql1 += string.Format("      ,WORK_END_DDTT     ");
                sql1 += string.Format("      ,A.WORK_TYPE ");
                sql1 += string.Format("      ,A.WORK_TEAM ");
                sql1 += string.Format("      ,A.PCS ");
                sql1 += string.Format("      ,A.POC_SEQ ");
                sql1 += string.Format("      ,NVL(A.DEL_YN,'N') AS DEL_YN ");
                sql1 += string.Format("      ,TO_CHAR(( FN_GET_WGT(A.ITEM,A.ITEM_SIZE,A.LENGTH,A.PCS) ),'999,999') AS THEORY_WGT ");
                sql1 += string.Format("FROM   TB_BND_WR A ");
                sql1 += string.Format("WHERE  A.BUNDLE_NO = :P_BUNDLE_NO ");
                sql1 += string.Format("AND    NVL(A.DEL_YN,'N') <> 'Y'");

                string[] parm = new string[1];
                parm[0] = ":P_BUNDLE_NO|" + bundle_no;

                par_dt = cd.FindDataTable(sql1, parm);

                if (par_dt.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                    uC_Line_gp1.Line_GP = par_dt.Rows[0]["LINE_GP"].ToString();
                    uC_POC1.POC = par_dt.Rows[0]["POC_NO"].ToString();
                    uC_BundleNo1.BundleNo = par_dt.Rows[0]["BUNDLE_NO"].ToString();
                    uC_Item_size1.ITEM_SIZE = par_dt.Rows[0]["ITEM_SIZE"].ToString();
                    uC_HEAT1.HEAT = par_dt.Rows[0]["HEAT"].ToString();
                    uC_Work_Type1.Work_Type = par_dt.Rows[0]["WORK_TYPE"].ToString();
                    uC_Work_Team1.Work_Team = par_dt.Rows[0]["WORK_TEAM"].ToString();
                    uC_Length1.Length = vf.Format(par_dt.Rows[0]["LENGTH"].ToString(),"#.00");
                    
                    uC_SteelNM1.SteelNM = par_dt.Rows[0]["STEEL_NM"].ToString();
                    uC_Work_Day1.Work_Day = vf.CDate(par_dt.Rows[0]["WORK_START_DDTT"].ToString(), "yyyyMMddHHmmss");
                    uC_Time1.Work_Time = vf.CDate(par_dt.Rows[0]["WORK_START_DDTT"].ToString(), "yyyyMMddHHmmss");
                    uC_Time2.Work_Time = vf.CDate(par_dt.Rows[0]["WORK_END_DDTT"].ToString(), "yyyyMMddHHmmss");
                    uC_BND_PCS1.BND_PCS = par_dt.Rows[0]["PCS"].ToString();
                    uC_POC_SEQ.BND_PCS = par_dt.Rows[0]["POC_SEQ"].ToString();
                    uC_CalWgt1.CalWgt = par_dt.Rows[0]["THEORY_WGT"].ToString();
                    uC_DelYN1.DelYN = par_dt.Rows[0]["DEL_YN"].ToString();

                    steel_id = par_dt.Rows[0]["STEEL"].ToString();
                    item = par_dt.Rows[0]["ITEM"].ToString();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }
        #endregion SetDataBinding 설정

        #region 이벤트 설정
        private void UC_POC1_SearchEvent(object sender, EventArgs e)
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT X.* ");
                sql1 += string.Format("       ,FN_GET_WGT(X.ITEM,X.ITEM_SIZE,X.LENGTH,X.PCS) AS THEORY_WGT ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("    SELECT HEAT ");
                sql1 += string.Format("          ,ITEM ");
                sql1 += string.Format("          ,ITEM_SIZE ");
                sql1 += string.Format("          ,LENGTH ");
                sql1 += string.Format("    FROM   TB_CR_ORD A ");
                sql1 += string.Format("          ,TB_LINE_WORK_TEAM B ");
                sql1 += string.Format("    WHERE  POC_NO    = :P_POC_NO ");
                sql1 += string.Format("    AND    B.LINE_GP = :P_LINE_GP ");
                sql1 += string.Format(") X ");

                string[] parm = new string[2];
                parm[0] = ":P_POC_NO|" + uC_POC1.POC;
                parm[1] = ":P_LINE_GP|" + uC_Line_gp1.Line_GP;

                moddt = cd.FindDataTable(sql1, parm);

                if (moddt.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                    uC_Item_size1.ITEM_SIZE = moddt.Rows[0]["ITEM_SIZE"].ToString();
                    uC_BundleNo1.BundleNo = moddt.Rows[0]["ITEM_SIZE"].ToString();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }
            return;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // 신규로 생성하기위해 기존의 등록데이터를 삭제시킴
            //제외) 라인, 제품번들번호

            InitalParameter();

            ItemAdd(string.Empty);
        }

        private void ItemAdd(string _poc_No)
        {
            BNDRsltInqPopup popup = new BNDRsltInqPopup(uC_Line_gp1.Line_GP, _poc_No, uC_Item_size1.ITEM_SIZE);
            popup.Owner = this; //A폼을 지정하게 된다.
            //popup.StartPosition = FormStartPosition.CenterScreen;
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.ShowDialog();

            if (ck.Temptable == null || ck.Temptable.Rows.Count < 1 )
            {
                return;
            }
            string sql1 = string.Empty;

            if (ck.Temptable != null)
            {
                foreach (DataRow datarow in ck.Temptable.Rows)
                {
                    sql1 = string.Format("PIECE_NO = '{0}' AND MILL_NO = '{1}'", datarow["PIECE_NO"], datarow["MILL_NO"]);
                    DataRow[] result = moddt.Select(sql1);
                    if (result.Count() == 0)
                    {
                        datarow["L_NO"] = "추가";
                        datarow["GUBUN"] = "추가";
                        moddt.ImportRow(datarow);
                        grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];
                    }
                }
                SetupParameter();

                ck.Temptable = null;
            }
        }

        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            ItemAdd(uC_POC1.POC);
        }

        private void grdMain_DataSourceChanged(object sender, EventArgs e)
        {
            C1FlexGrid grd = grdMain as C1FlexGrid;

            if (grd.Rows.Count > 1)
            {
                uC_POC1.POC = grd.GetData(1, "POC_NO").ToString();
            }
            else
            {
                uC_POC1.POC = "";
            }
        }

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedRow <= 0 || editedGrd.GetData(editedRow, editedCol) == null )
            {
                e.Cancel = true;
            }
        }

        private void SetupInitialData(int row_cnt)
        {
            int theoryWgt = 0;

            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT  X.*");
                sql1 += string.Format("       ,FN_GET_WGT(X.ITEM,X.ITEM_SIZE,X.LENGTH,{0}) AS THEORY_WGT ", row_cnt);
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("    SELECT HEAT ");
                sql1 += string.Format("          ,ITEM ");
                sql1 += string.Format("          ,ITEM_SIZE ");
                sql1 += string.Format("          ,LENGTH ");
                sql1 += string.Format("          ,WORK_TYPE ");
                sql1 += string.Format("          ,WORK_TEAM ");
                sql1 += string.Format("    FROM   TB_CR_ORD A ");
                sql1 += string.Format("          ,TB_LINE_WORK_TEAM B ");
                sql1 += string.Format("    WHERE  POC_NO    = :P_POC_NO ");
                sql1 += string.Format("    AND    B.LINE_GP = :P_LINE_GP ");
                sql1 += string.Format(") X ");

                string[] parm = new string[2];
                parm[0] = ":P_POC_NO|" + uC_POC1.POC;
                parm[1] = ":P_LINE_GP|" + uC_Line_gp1.Line_GP;

                temp_dt = cd.FindDataTable(sql1, parm);

                if (temp_dt.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                    uC_Work_Day1.Work_Day = DateTime.Now.Date;
                    uC_Time1.Work_Time = DateTime.Now;
                    uC_Time2.Work_Time = DateTime.Now;
                    uC_Work_Type1.Work_Type = temp_dt.Rows[0]["WORK_TYPE"].ToString();
                    uC_Work_Team1.Work_Team = temp_dt.Rows[0]["WORK_TEAM"].ToString();
                    uC_CalWgt1.CalWgt = vf.Format(vf.CInt2(temp_dt.Rows[0]["THEORY_WGT"].ToString()).ToString(), "#,###,##0");
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return ;
            }

            return ;
        }
        private string GetBundleNo(string _poc_No)
        {
            string _bundle_No = string.Empty;

            OracleConnection conn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();

            System.Data.OracleClient.OracleParameter prm = new System.Data.OracleClient.OracleParameter();

            try
            {
                conn.Open(); // open connection
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                prm.Direction = ParameterDirection.ReturnValue;
                prm.DbType = DbType.AnsiString;
                prm.Size = 16;

                cmd.Parameters.Add(prm);

                cmd.CommandText = string.Format("FN_BUNDLE_NO_WR('{0}')", _poc_No);

                cmd.ExecuteNonQuery();

                _bundle_No = cmd.Parameters[0].Value.ToString();
            }
            catch (Exception ex)
            {
                //실행후 실패 : 

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show(ex.Message);
                return _bundle_No;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
            }

            return _bundle_No;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 입력항목체크
            if (string.IsNullOrEmpty(uC_CalWgt1.CalWgt) || string.IsNullOrEmpty(uC_BND_PCS1.BND_PCS) || uC_CalWgt1.CalWgt == "0" || uC_BND_PCS1.BND_PCS == "0" || steel_id == "" || item == "")
            {
                MessageBox.Show("바인딩할 항목을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //bundle 넘버가 존재할경우와 bundle 넘버가 따로 생성해야하는경우
            if (string.IsNullOrEmpty(uC_BundleNo1.BundleNo))
            {
                // 생성하여 textbox에 입력
                uC_BundleNo1.BundleNo = GetBundleNo(uC_POC1.POC);
            }

            //PCS 를 미리 계산해서 즉 grdMain 의 실제의 본수를 계산해서 데이터를 가지고 바인드 실적 수정에 적용
            //추가된 행과 삭제되지 않은 행을 계산한다.
            int pcs_cnt = 0;
            for (int row = 1; row < grdMain.Rows.Count; row++)
            {
                if (grdMain.GetData(row, "GUBUN").ToString() == "삭제")
                {
                    continue;
                }
                pcs_cnt++;
            }
            #endregion


            string strLog = string.Empty;
            var itemList = new List<DictionaryList>();
            var logList = new List<LogDataList>();


            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            try
            {
                string sql1 = string.Empty;
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                #region TB_BND_WR
                // 현재 그리드에 수정 및 추가 될 리스트가 없어서 
                // 해당 번들 넘버의 데이터를 실적에서 지운다.
                // 모두 삭제를 눌렀거나 번들넘버만 존재하는경우.
                if (pcs_cnt ==0)
                {
                    sql1 = string.Empty;
                    strLog = string.Empty;
                    sql1  = string.Format(" DELETE TB_BND_WR ");
                    sql1 += string.Format(" WHERE BUNDLE_NO = '{0}' ", uC_BundleNo1.BundleNo);
                    cmd.CommandText = sql1;
                    cmd.ExecuteNonQuery();

                    itemList.Add(new DictionaryList("BUNDLE_NO", uC_BundleNo1.BundleNo));
                    itemList.Add(new DictionaryList("REGISTER", ck.UserID));

                    LogStrCreate(itemList, ref strLog);                   

                    logList.Add(new LogDataList(Alarms.Deleted, Text, strLog));
                }
                else
                {
                    sql1 = string.Empty;
                    strLog = string.Empty;
                    //해당번들번호가 존재하면 업데이트 없으면 인서트
                    sql1  = string.Format(" MERGE INTO TB_BND_WR  A ");
                    sql1 += string.Format(" USING ( SELECT '{0}'  AS BUNDLE_NO ", uC_BundleNo1.BundleNo);
                    sql1 += string.Format("         FROM  DUAL ) B ");
                    sql1 += string.Format(" ON    ( A.BUNDLE_NO    = B.BUNDLE_NO ) ");
                                            
                    sql1 += string.Format(" WHEN  MATCHED THEN ");
                    sql1 += string.Format("       UPDATE  SET ");
                    sql1 += string.Format("                MFG_DATE       = '{0}' ", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd"));
                    sql1 += string.Format("               ,WORK_TYPE      = '{0}' ", uC_Work_Type1.Work_Type);
                    sql1 += string.Format("               ,WORK_TEAM      = '{0}' ", uC_Work_Team1.Work_Team);
                    sql1 += string.Format("               ,WORK_START_DDTT=  {0} ", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time1.Work_Time, "HHmmss"));
                    sql1 += string.Format("               ,WORK_END_DDTT  =  {0} ", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time2.Work_Time, "HHmmss"));
                    sql1 += string.Format("               ,PCS            =  {0} ", pcs_cnt);
                    sql1 += string.Format("               ,THEORY_WGT     =  {0} ", vf.CInt2(uC_CalWgt1.CalWgt));
                    sql1 += string.Format("               ,DEL_YN         =  '{0}' ", uC_DelYN1.DelYN);
                    sql1 += string.Format("               ,DEL_DDTT       =  DECODE('{0}','Y',SYSDATE,DEL_DDTT) ", uC_DelYN1.DelYN);
                    sql1 += string.Format("               ,MODIFIER       =  '{0}' ", ck.UserID);
                    sql1 += string.Format("               ,LENGTH       =  '{0}' ", uC_Length1.Length);
                    sql1 += string.Format("               ,MOD_DDTT       =   SYSDATE ");
                                            
                    sql1 += string.Format(" WHEN NOT MATCHED THEN  ");
                    sql1 += string.Format("       INSERT (BUNDLE_NO ");
                    sql1 += string.Format("              ,LINE_GP ");
                    sql1 += string.Format("              ,MFG_DATE ");
                    sql1 += string.Format("              ,WORK_TYPE ");
                    sql1 += string.Format("              ,WORK_TEAM ");
                    sql1 += string.Format("              ,POC_NO ");
                    sql1 += string.Format("              ,HEAT ");
                    sql1 += string.Format("              ,STEEL ");
                    sql1 += string.Format("              ,ITEM ");
                    sql1 += string.Format("              ,ITEM_SIZE ");
                    sql1 += string.Format("              ,LENGTH ");
                    sql1 += string.Format("              ,WORK_START_DDTT ");
                    sql1 += string.Format("              ,WORK_END_DDTT ");
                    sql1 += string.Format("              ,PCS ");
                    sql1 += string.Format("              ,THEORY_WGT ");
                    sql1 += string.Format("              ,POC_SEQ ");
                    sql1 += string.Format("              ,REGISTER ");
                    sql1 += string.Format("              ,REG_DDTT ");
                    sql1 += string.Format("              ) VALUES ( ");
                    sql1 += string.Format("               '{0}' ", uC_BundleNo1.BundleNo);//W_BUNDLE_NO
                    sql1 += string.Format("              ,'{0}' ", uC_Line_gp1.Line_GP);//LINE_GP
                    sql1 += string.Format("              ,'{0}' ", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd"));//P_MFG_DATE
                    sql1 += string.Format("              ,'{0}' ", uC_Work_Type1.Work_Type);//P_WORK_TYPE
                    sql1 += string.Format("              ,'{0}' ", uC_Work_Team1.Work_Team);//P_WORK_TEAM
                    sql1 += string.Format("              ,'{0}' ", uC_POC1.POC);//P_POC_NO
                    sql1 += string.Format("              ,'{0}' ", uC_HEAT1.HEAT);//W_HEAT
                    sql1 += string.Format("              ,'{0}' ", steel_id);//W_STEEL
                    sql1 += string.Format("              ,'{0}' ", item);//W_ITEM
                    sql1 += string.Format("              ,'{0}' ", uC_Item_size1.ITEM_SIZE);//W_ITEM_SIZE
                    sql1 += string.Format("              ,'{0}'  ", uC_Length1.Length);//W_LENGTH
                    sql1 += string.Format("              ,'{0}' ", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time1.Work_Time, "HHmmss"));//SUBSTR(P_MFG_DATE || P_START_TIME,1,14)
                    sql1 += string.Format("              ,'{0}' ", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time2.Work_Time, "HHmmss"));//SUBSTR(P_MFG_DATE || P_END_TIME,1,14)
                    sql1 += string.Format("              , {0}  ", vf.CInt2(uC_BND_PCS1.BND_PCS));//P_PCS
                    sql1 += string.Format("              , {0}  ", vf.CInt2(uC_CalWgt1.CalWgt));//THEORY_WGT
                    sql1 += string.Format("              ,'{0}' ", uC_POC_SEQ.BND_PCS);//POC_SEQ
                    sql1 += string.Format("              ,'{0}' ", ck.UserID);
                    sql1 += string.Format("              ,SYSDATE ");
                    sql1 += string.Format("              ) ");

                    cmd.CommandText = sql1;
                    cmd.ExecuteNonQuery();

                    itemList.Add(new DictionaryList("BUNDLE_NO", uC_BundleNo1.BundleNo));

                    itemList.Add(new DictionaryList("MFG_DATE", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd")));
                    itemList.Add(new DictionaryList("WORK_TYPE", uC_Work_Type1.Work_Type));
                    itemList.Add(new DictionaryList("WORK_TEAM", uC_Work_Team1.Work_Team));
                    itemList.Add(new DictionaryList("WORK_START_DDTT", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time1.Work_Time, "HHmmss")));
                    itemList.Add(new DictionaryList("WORK_END_DDTT", vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time2.Work_Time, "HHmmss")));
                    itemList.Add(new DictionaryList("PCS", pcs_cnt.ToString()));
                    itemList.Add(new DictionaryList("THEORY_WGT", vf.CInt2(uC_CalWgt1.CalWgt).ToString()));
                    itemList.Add(new DictionaryList("DEL_YN", uC_DelYN1.DelYN));
                    if (uC_DelYN1.DelYN=="Y")
                    {
                        itemList.Add(new DictionaryList("DEL_DDTT", DateTime.Now.ToString()));
                    }
                    
                    itemList.Add(new DictionaryList("MODIFIER", ck.UserID));

                    LogStrCreate(itemList, ref strLog);

                    logList.Add(new LogDataList(Alarms.Modified, Text, strLog));
                    //if (uC_DelYN1.DelYN == "N")
                    //{
                    //    //계중실적 Y/N업데이트 20161205 신귀현
                    //    sql1 = string.Empty;
                    //    //해당번들번호가 존재하면 업데이트
                    //    sql1 = string.Format(" MERGE INTO TB_WGT_WR  A ");
                    //    sql1 += string.Format(" USING ( SELECT '{0}'  AS BUNDLE_NO ", uC_BundleNo1.BundleNo);
                    //    sql1 += string.Format("         FROM  DUAL ) B ");
                    //    sql1 += string.Format(" ON    ( A.BUNDLE_NO    = B.BUNDLE_NO ) ");

                    //    sql1 += string.Format(" WHEN  MATCHED THEN ");
                    //    sql1 += string.Format("       UPDATE  SET ");
                    //    sql1 += string.Format("               IF_YN         =  'U'");
                    //    sql1 += string.Format("               ,MODIFIER       =  '{0}' ", ck.UserID);
                    //    sql1 += string.Format("               ,MOD_DDTT       =   SYSDATE ");

                    //    cmd.CommandText = sql1;
                    //    cmd.ExecuteNonQuery();
                    //}
                }
                start_dt = uC_Work_Day1.Work_Day;
                end_dt = uC_Work_Day1.Work_Day;

                #endregion

                #region TB_CR_PIECE_WR
                for (int row = 1; row < grdMain.Rows.Count; row++)
                {
                    if (grdMain.GetData(row, "GUBUN").ToString() == "추가")
                    {
                        strLog = string.Empty;
                        sql1 = string.Empty;
                        sql1 += string.Format(" INSERT INTO TB_CR_PIECE_WR ");
                        sql1 += string.Format("        (MILL_NO ");
                        sql1 += string.Format("        ,PIECE_NO        ");
                        sql1 += string.Format("        ,LINE_GP        ");
                        sql1 += string.Format("        ,ROUTING_CD       ");
                        sql1 += string.Format("        ,REWORK_SEQ      ");
                        sql1 += string.Format("        ,POC_NO             ");
                        sql1 += string.Format("        ,HEAT               ");
                        sql1 += string.Format("        ,STEEL           ");
                        sql1 += string.Format("        ,ITEM                ");
                        sql1 += string.Format("        ,ITEM_SIZE          ");
                        sql1 += string.Format("        ,LENGTH         ");
                        sql1 += string.Format("        ,ENTRY_DDTT ");
                        sql1 += string.Format("        ,EXIT_DDTT  ");
                        sql1 += string.Format("        ,MFG_DATE            ");
                        sql1 += string.Format("        ,WORK_TYPE      ");
                        sql1 += string.Format("        ,WORK_TEAM       ");
                        sql1 += string.Format("        ,BUNDLE_NO       ");
                        sql1 += string.Format("        ,POC_SEQ       ");
                        sql1 += string.Format("        ,REGISTER       ");
                        sql1 += string.Format("        ,REG_DDTT       ");
                        sql1 += string.Format("        ) VALUES (      ");
                        sql1 += string.Format("         '{0}' ", grdMain.GetData(row, "MILL_NO").ToString());//MILL_NO
                        sql1 += string.Format("        , {0} ", vf.CInt2(grdMain.GetData(row, "PIECE_NO").ToString()));//PIECE_NO
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "LINE_GP").ToString());//LINE_GP
                        sql1 += string.Format("        ,'{0}' ", "P3");// grdMain.GetData(row, "ROUTING_CD").ToString());//ROUTING_CD
                        sql1 += string.Format("        , {0} ", vf.CInt2(grdMain.GetData(row, "REWORK_SEQ").ToString()));  // vf.CInt2(grdMain.GetData(row, "REWORK_SEQ").ToString()));//REWORK_SEQ
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "POC_NO").ToString());//POC_NO
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "HEAT").ToString());//HEAT
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "STEEL").ToString());//STEEL
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "ITEM").ToString());//ITEM
                        sql1 += string.Format("        ,'{0}' ", grdMain.GetData(row, "ITEM_SIZE").ToString());//ITEM_SIZE
                        sql1 += string.Format("        , {0}  ", vf.VAL(grdMain.GetData(row, "LENGTH").ToString()));//LENGTH
                        sql1 += string.Format("        ,TO_DATE('{0}','YYYY-MM-DD HH24:MI:SS' )", clsUtil.Utl.GetDateFormat(1, start_dt.Add(uC_Time1.Work_Time.TimeOfDay)));// vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time1.Work_Time, "HHmmss"));//ENTRY_DDTT
                        sql1 += string.Format("        ,TO_DATE('{0}','YYYY-MM-DD HH24:MI:SS' )", clsUtil.Utl.GetDateFormat(1, end_dt.Add(uC_Time2.Work_Time.TimeOfDay)));// vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd") + vf.Format(uC_Time2.Work_Time, "HHmmss"));//EXIT_DDTT
                        sql1 += string.Format("        ,'{0}'  ", vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd"));//MFG_DATE
                        sql1 += string.Format("        ,'{0}'  ", uC_Work_Type1.Work_Type);//WORK_TYPE
                        sql1 += string.Format("        ,'{0}'  ", uC_Work_Team1.Work_Team);//WORK_TEAM
                        sql1 += string.Format("        ,'{0}'  ", uC_BundleNo1.BundleNo);//BUNDLE_NO
                        sql1 += string.Format("        ,'{0}'  ", grdMain.GetData(row, "POC_SEQ").ToString());//POC_SEQ
                        sql1 += string.Format("        ,'{0}' ", ck.UserID);
                        sql1 += string.Format("        ,SYSDATE ");
                        sql1 += string.Format("        ) ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        itemList.Add(new DictionaryList("MILL_NO", grdMain.GetData(row, "MILL_NO").ToString()));
                        itemList.Add(new DictionaryList("PIECE_NO", grdMain.GetData(row, "PIECE_NO").ToString()));
                        itemList.Add(new DictionaryList("LINE_GP", grdMain.GetData(row, "LINE_GP").ToString()));
                        itemList.Add(new DictionaryList("ROUTING_CD", "P3"));
                        itemList.Add(new DictionaryList("REWORK_SEQ", grdMain.GetData(row, "REWORK_SEQ").ToString()));
                        itemList.Add(new DictionaryList("POC_NO", grdMain.GetData(row, "POC_NO").ToString()));
                        itemList.Add(new DictionaryList("HEAT", grdMain.GetData(row, "HEAT").ToString()));
                        itemList.Add(new DictionaryList("STEEL", grdMain.GetData(row, "STEEL").ToString()));
                        itemList.Add(new DictionaryList("ITEM", grdMain.GetData(row, "ITEM").ToString()));
                        itemList.Add(new DictionaryList("ITEM_SIZE", grdMain.GetData(row, "ITEM_SIZE").ToString()));
                        itemList.Add(new DictionaryList("LENGTH", grdMain.GetData(row, "LENGTH").ToString()));
                        itemList.Add(new DictionaryList("ENTRY_DDTT", clsUtil.Utl.GetDateFormat(1, start_dt.Add(uC_Time1.Work_Time.TimeOfDay))));
                        itemList.Add(new DictionaryList("EXIT_DDTT", clsUtil.Utl.GetDateFormat(1, end_dt.Add(uC_Time2.Work_Time.TimeOfDay))));
                        itemList.Add(new DictionaryList("MFG_DATE", grdMain.GetData(row, "MFG_DATE").ToString()));
                        itemList.Add(new DictionaryList("WORK_TYPE", uC_Work_Type1.Work_Type));
                        itemList.Add(new DictionaryList("WORK_TEAM", uC_Work_Team1.Work_Team));
                        itemList.Add(new DictionaryList("BUNDLE_NO", uC_BundleNo1.BundleNo));
                        itemList.Add(new DictionaryList("POC_SEQ", grdMain.GetData(row, "POC_SEQ").ToString()));
                        //itemList.Add(new DictionaryList("REG_DDTT", ck.UserID));

                        LogStrCreate(itemList, ref strLog);
                        logList.Add(new LogDataList(Alarms.InSerted, Text, strLog));
                    }
                    else if (grdMain.GetData(row, "GUBUN").ToString() == "삭제")
                    {
                        strLog = string.Empty;
                        sql1 = string.Empty;
                        sql1 += string.Format(" DELETE FROM TB_CR_PIECE_WR ");
                        sql1 += string.Format(" WHERE MILL_NO =      '{0}' ", grdMain.GetData(row, "MILL_NO").ToString());
                        sql1 += string.Format(" AND   PIECE_NO =     '{0}' ", grdMain.GetData(row, "PIECE_NO").ToString());
                        sql1 += string.Format(" AND   LINE_GP =      '{0}' ", grdMain.GetData(row, "LINE_GP").ToString());
                        sql1 += string.Format(" AND   ROUTING_CD =   '{0}' ", grdMain.GetData(row, "ROUTING_CD").ToString());
                        sql1 += string.Format(" AND   REWORK_SEQ =    {0}  ", grdMain.GetData(row, "REWORK_SEQ"));
                        sql1 += string.Format(" AND   POC_NO =       '{0}' ", grdMain.GetData(row, "POC_NO").ToString());

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        itemList.Add(new DictionaryList("MILL_NO", grdMain.GetData(row, "MILL_NO").ToString()));
                        itemList.Add(new DictionaryList("PIECE_NO", grdMain.GetData(row, "PIECE_NO").ToString()));
                        itemList.Add(new DictionaryList("LINE_GP", grdMain.GetData(row, "LINE_GP").ToString()));
                        itemList.Add(new DictionaryList("ROUTING_CD", grdMain.GetData(row, "ROUTING_CD").ToString()));
                        itemList.Add(new DictionaryList("REWORK_SEQ", grdMain.GetData(row, "REWORK_SEQ").ToString()));
                        itemList.Add(new DictionaryList("POC_NO", grdMain.GetData(row, "POC_NO").ToString()));
                        itemList.Add(new DictionaryList("RESISTER", ck.UserID));


                        LogStrCreate(itemList, ref strLog);

                        logList.Add(new LogDataList(Alarms.Deleted, Text, strLog));
                    }
                    
                }
                #endregion

                //실행후 성공
                transaction.Commit();
               
                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), message);

                foreach (var log in logList)
                {
                    clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                }


                ck.StrKey1 = message;
                Close();
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                foreach (var log in logList)
                {
                    clsMsg.Log.Alarm(Alarms.Error, log.PageName, clsMsg.Log.__Line(), log.Contents);
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show(ex.Message);
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

        private void btnDelRow_Click(object sender, EventArgs e)
        {
            C1FlexGrid grd = grdMain as C1FlexGrid;

            if (grdMain.Rows.Count < 2 || grd.RowSel < 1)
            {
                return;
            }
            //grd caption row 는 row == 0
            int selected_row = grd.RowSel;

            //grd.SetData(selected_row,"L_NO") = "추가".ToString();
            grd.SetData(selected_row, "L_NO", "삭제");
            grd.SetData(selected_row, "GUBUN", "삭제");
            grdMain.Rows[selected_row].Style = grdMain.Styles["DelColor"];

            // 커서위치 선택해제
            grdMain.Row = 0;
            grdMain.Col = 0;
        }
        #endregion 이벤트 설정
    }
}
