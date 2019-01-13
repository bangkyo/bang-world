﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using static ComLib.clsUtil;
using System.Collections.Specialized;
using SystemControlClassLibrary.Popup;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

using excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;

namespace SystemControlClassLibrary
{

    public partial class WGTRslt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;


        private int selectedrow = 0;
        private string cd_id = "";
        private string cd_nm = "";
        private string cd_id2 = "";
        private string cd_nm2 = "";
        private string cd_id3 = "";
        private string auto_type = "";

        private string txtheat = "";
        private string txtpoc = "";
        private string txtitem_size = "";
        private string txtsteel = "";
        private string txtsteel_nm = "";
        private string gangjung_id = "";
        int subtotalNo ;

        clsStyle cs = new clsStyle();

        // 셀의 수정전 값
        private string strBefValue = "";


        private string selected_grp_id = "";


        private string ownerNM = "";
        private string titleNM = "";

        private string scrAuth;

        string sMeasNo = "";
        #endregion

        #region 생성자, 로딩 설정
        public WGTRslt(string titleNm, string _scrAuth, string factCode, string ownerNm)
        {

            ownerNM = ownerNm;
            titleNM = titleNm;
            scrAuth = _scrAuth;
            InitializeComponent();

            //Load += WGTRslt_Load;



            btnDisplay.Click += Button_Click;
            btnExcel.Click += Button_Click;

            grdMain.RowColChange += GrdMain_RowColChange;

            cboLine_GP.SelectedIndexChanged += CboLine_GP_SelectedIndexChanged;
            cbo_Work_Type.SelectedIndexChanged += cbo_Work_Type_SelectedIndexChanged;

            //btnSteel.Click += btnSteel_Click;

        }

        ~WGTRslt()
        {
            
        }
        private void WGTRslt_Load(object sender, System.EventArgs e)
        {


            this.BackColor = Color.White;

            InitControl();


            SetComboBox1();
            SetComboBox2();
            SetComboBox3();
            SetComboBox4();
            //SetDataBinding(); // 그리드 초기화면 제어 
            EventCreate();      //사용자정의 이벤트
            Button_Click(btnDisplay, null);
            //SetDataBinding();
        }

        #endregion

        #region Init 컨트롤, 그리드 설정

        private void InitControl()
        {

            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);


            cs.InitLabel(lblLine);
            cs.InitLabel(lblPoc);
            cs.InitLabel(lblHeat);
            cs.InitLabel(lblItemSize);
            cs.InitLabel(lblSteel);
            cs.InitLabel(lblWorkType);
            cs.InitLabel(lblMfgDate);
            cs.InitLabel(lblTEAM);

            //cs.InitCount(lblCount);

            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            cs.InitCombo(cbo_Work_Type, StringAlignment.Near);
            cs.InitCombo(cboTEAM, StringAlignment.Near);
            cs.InitCombo(cbo_OnOff, StringAlignment.Near);

            cs.InitTextBox(poctxt);
            cs.InitTextBox(heattxt);
            cs.InitTextBox(txtItemSize);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(gangjong_Nm_tb);


            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);

            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            cs.InitButton(btnReg);



            //시간 데이터 default 값 적용 
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            //dateEdit Init()
            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);

            InitGrd_Main();

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

        private void cbo_OnOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            auto_type = ((ComLib.DictionaryList)cbo_OnOff.SelectedItem).fnValue;
        }

        private void InitGrd_Main()
        {
            grdMain.AutoClipboard = true;
            clsStyle.Style.InitGrid_search(grdMain);
           
            grdMain.Rows[0].Height = cs.Head_Height;
            grdMain.AutoResize = true;

            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["MEAS_NO"].Caption = "  계중\r\n번호";
            grdMain.Cols["MEAS_DDTT"].Caption = "작업일시";
            grdMain.Cols["WORK_TYPE"].Caption = "WORK_TYPE";
            grdMain.Cols["WORK_TYPE_NM"].Caption = "근";
            grdMain.Cols["WORK_TEAM"].Caption = "WORK_TEAM";
            grdMain.Cols["WORK_TEAM_NM"].Caption = "조";
            grdMain.Cols["POC_NO"].Caption = "POC";
            grdMain.Cols["BUNDLE_NO"].Caption = "번들번호";
            grdMain.Cols["HEAT"].Caption = "HEAT";
            grdMain.Cols["STEEL"].Caption = "강종";
            grdMain.Cols["STEEL_NM"].Caption = "강종명";
            grdMain.Cols["ITEM"].Caption = "품목";
            grdMain.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain.Cols["LENGTH"].Caption = "  길이\r\n(m)";
            grdMain.Cols["PCS"].Caption = "본수";
            grdMain.Cols["THEORY_WGT"].Caption = "  이론중량\r\n(kg)";
            grdMain.Cols["NET_WGT"].Caption = "  실중량\r\n(kg)";
            grdMain.Cols["WGT_CHA"].Caption = "편차";
            grdMain.Cols["WORK_METHOD"].Caption = "  계량\r\n구분";
            grdMain.Cols["WORK_METHOD_NM"].Caption = "  계량\r\n구분";

            grdMain.Cols["L_NO"].AllowSorting = false;
            grdMain.Cols["MEAS_NO"].AllowSorting = false;
            grdMain.Cols["MEAS_DDTT"].AllowSorting = false;
            grdMain.Cols["WORK_TYPE"].AllowSorting = false;
            grdMain.Cols["WORK_TYPE_NM"].AllowSorting = false;
            grdMain.Cols["WORK_TEAM"].AllowSorting = false;
            grdMain.Cols["WORK_TEAM_NM"].AllowSorting = false;
            grdMain.Cols["POC_NO"].AllowSorting = false;
            grdMain.Cols["BUNDLE_NO"].AllowSorting = false;
            grdMain.Cols["HEAT"].AllowSorting = false;
            grdMain.Cols["STEEL"].AllowSorting = false;
            grdMain.Cols["STEEL_NM"].AllowSorting = false;
            grdMain.Cols["ITEM"].AllowSorting = false;
            grdMain.Cols["ITEM_SIZE"].AllowSorting = false;
            grdMain.Cols["LENGTH"].AllowSorting = false;
            grdMain.Cols["PCS"].AllowSorting = false;
            grdMain.Cols["THEORY_WGT"].AllowSorting = false;
            grdMain.Cols["NET_WGT"].AllowSorting = false;
            grdMain.Cols["WGT_CHA"].AllowSorting = false;
            grdMain.Cols["WORK_METHOD"].AllowSorting = false;
            grdMain.Cols["WORK_METHOD_NM"].AllowSorting = false;

            grdMain.Cols["L_NO"].AllowEditing = false;
            grdMain.Cols["MEAS_NO"].AllowEditing = false;
            grdMain.Cols["MEAS_DDTT"].AllowEditing = false;
            grdMain.Cols["WORK_TYPE"].AllowEditing = false;
            grdMain.Cols["WORK_TYPE_NM"].AllowEditing = false;
            grdMain.Cols["WORK_TEAM"].AllowEditing = false;
            grdMain.Cols["WORK_TEAM_NM"].AllowEditing = false;
            grdMain.Cols["POC_NO"].AllowEditing = false;
            grdMain.Cols["BUNDLE_NO"].AllowEditing = false;
            grdMain.Cols["HEAT"].AllowEditing = false;
            grdMain.Cols["STEEL"].AllowEditing = false;
            grdMain.Cols["STEEL_NM"].AllowEditing = false;
            grdMain.Cols["ITEM"].AllowEditing = false;
            grdMain.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["PCS"].AllowEditing = false;
            grdMain.Cols["THEORY_WGT"].AllowEditing = false;
            grdMain.Cols["NET_WGT"].AllowEditing = false;
            grdMain.Cols["WGT_CHA"].AllowEditing = false;
            grdMain.Cols["WORK_METHOD"].AllowEditing = false;
            grdMain.Cols["WORK_METHOD_NM"].AllowEditing = false;


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["MEAS_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["MEAS_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TYPE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TEAM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["POC_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["BUNDLE_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["HEAT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["STEEL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["STEEL_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ITEM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ITEM_SIZE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["LENGTH"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["THEORY_WGT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["NET_WGT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["WGT_CHA"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["WORK_METHOD"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["WORK_METHOD_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;


            grdMain.Cols["L_NO"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["MEAS_NO"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["MEAS_DDTT"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TYPE"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TYPE_NM"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TEAM"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TEAM_NM"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["POC_NO"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["BUNDLE_NO"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["HEAT"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["STEEL"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["STEEL_NM"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["ITEM"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["ITEM_SIZE"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["LENGTH"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["PCS"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["THEORY_WGT"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["NET_WGT"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["WGT_CHA"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_METHOD"].TextAlignFixed = TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_METHOD_NM"].TextAlignFixed = TextAlignEnum.CenterCenter;


            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["MEAS_NO"].Width = 80;
            grdMain.Cols["MEAS_DDTT"].Width = 190;
            grdMain.Cols["WORK_TYPE"].Width = 0;
            grdMain.Cols["WORK_TYPE_NM"].Width = 50;
            grdMain.Cols["WORK_TEAM"].Width = 0;
            grdMain.Cols["WORK_TEAM_NM"].Width = 50;
            grdMain.Cols["POC_NO"].Width = 120;
            grdMain.Cols["BUNDLE_NO"].Width = 110;
            grdMain.Cols["HEAT"].Width = 100;
            grdMain.Cols["STEEL"].Width = 80;
            grdMain.Cols["STEEL_NM"].Width = 120;
            grdMain.Cols["ITEM"].Width = 80;
            grdMain.Cols["ITEM_SIZE"].Width = 70;
            grdMain.Cols["LENGTH"].Width = 80;
            grdMain.Cols["PCS"].Width = 70;
            grdMain.Cols["THEORY_WGT"].Width = 100;
            grdMain.Cols["NET_WGT"].Width = 80;
            grdMain.Cols["WGT_CHA"].Width = 70;
            grdMain.Cols["WORK_METHOD"].Width = 0;
            grdMain.Cols["WORK_METHOD_NM"].Width = 100;

            grdMain.Cols["WORK_TYPE"].Visible = false;
            grdMain.Cols["WORK_TEAM"].Visible = false;


            
            // more setup
            grdMain.AllowDragging = AllowDraggingEnum.None;
            //grdMain.AllowEditing = false;
            //grdMain.Cols[0].WidthDisplay /= 3;
            grdMain.Tree.Column = 1;

        }

        #endregion

        #region 조회 설정

        private bool SetDataBinding()
        {
            try
            {

                string start_date = start_dt.Value.ToString();
                start_date = (start_date.Substring(0, 4) + start_date.Substring(5, 2) + start_date.Substring(8, 2));

                string end_date = end_dt.Value.ToString();
                end_date = (end_date.Substring(0, 4) + end_date.Substring(5, 2) + end_date.Substring(8, 2));

                poctxt.Value = poctxt.Text;
                heattxt.Value = heattxt.Text;
                string sql1 = string.Empty;

                if (cd_id == "#3")
                {
                    sql1 = string.Format(" SELECT  ROWNUM AS L_NO ");
                    sql1 += string.Format("       ,MEAS_NO ");
                    sql1 += string.Format("       ,MEAS_DDTT ");
                    sql1 += string.Format("       ,WORK_TYPE");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                    sql1 += string.Format("       ,WORK_TEAM");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                    sql1 += string.Format("       ,POC_NO ");
                    sql1 += string.Format("       ,BUNDLE_NO ");
                    sql1 += string.Format("       ,HEAT ");
                    sql1 += string.Format("       ,STEEL ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("       ,ITEM ");
                    sql1 += string.Format("       ,ITEM_SIZE ");
                    sql1 += string.Format("       ,LENGTH ");
                    sql1 += string.Format("       ,PCS ");
                    sql1 += string.Format("       ,THEORY_WGT ");
                    sql1 += string.Format("       ,NET_WGT ");
                    sql1 += string.Format("       ,WGT_CHA ");
                    sql1 += string.Format("       ,WORK_METHOD ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_METHOD' AND CD_ID = X.WORK_METHOD) AS WORK_METHOD_NM ");
                    sql1 += string.Format("FROM   ( ");
                    sql1 += string.Format("        SELECT  A.MEAS_NO "); //--계중번호
                    sql1 += string.Format("               ,TO_CHAR(TO_DATE(A.MEAS_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS MEAS_DDTT  "); //--계중일시
                    sql1 += string.Format("               ,A.WORK_TYPE ");
                    sql1 += string.Format("               ,NVL(A.WORK_TEAM, 'A') AS WORK_TEAM ");
                    sql1 += string.Format("               ,B.POC_NO ");
                    sql1 += string.Format("               ,B.BUNDLE_NO ");
                    sql1 += string.Format("               ,B.HEAT ");
                    sql1 += string.Format("               ,B.STEEL ");
                    sql1 += string.Format("               ,B.ITEM ");
                    sql1 += string.Format("               ,B.ITEM_SIZE ");
                    sql1 += string.Format("               ,B.LENGTH ");
                    sql1 += string.Format("               ,B.PCS ");
                    sql1 += string.Format("               ,B.THEORY_WGT ");
                    sql1 += string.Format("               ,A.NET_WGT ");
                    sql1 += string.Format("               ,A.NET_WGT - B.THEORY_WGT  AS WGT_CHA "); //--편차
                    sql1 += string.Format("               ,A.WORK_METHOD "); //--작업방법
                    sql1 += string.Format("        FROM   TB_WGT_WR  A ");
                    sql1 += string.Format("              ,TB_BND_WR  B ");
                    sql1 += string.Format("        WHERE  A.BUNDLE_NO  = B.BUNDLE_NO ");
                    sql1 += string.Format("        AND    A.MEAS_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                    sql1 += string.Format("        AND    A.LINE_GP    = '{0}' ", cd_id); //:P_LINE_GP
                    sql1 += string.Format("        AND    B.POC_NO     LIKE '%{0}%' || '%'", poctxt.Text);    //:P_POC_NO
                    sql1 += string.Format("        AND    B.HEAT       LIKE '%{0}%' || '%'", heattxt.Text);  //:P_HEAT
                    sql1 += string.Format("        AND    A.WORK_TYPE  LIKE '{0}' || '%' ", cd_id2);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                    sql1 += string.Format("        AND    A.WORK_TEAM  LIKE '{0}' || '%' ", cd_id3);
                    sql1 += string.Format("        AND    B.ITEM_SIZE       LIKE '%{0}%' || '%'", txtitem_size);  //:ITEM_SIZE
                    sql1 += string.Format("        AND    B.STEEL       LIKE '%{0}%' || '%'", gangjung_id);  //:STEEL
                    sql1 += string.Format("        AND    NVL(A.DEL_YN,'N')   <> 'Y'    ");
                    sql1 += string.Format("        AND    NVL(B.DEL_YN,'N')   <> 'Y' ");
                    if (auto_type == "A")
                    {
                        //sql1 += string.Format("     AND   (A.BUNDLE_NO LIKE '%A' OR A.BUNDLE_NO LIKE '%B')   ");
                        sql1 += string.Format("     AND   A.BUNDLE_NO_L2 IS NULL  ");
                    }
                    if (auto_type == "B")
                    {
                        sql1 += string.Format("     AND   A.BUNDLE_NO_L2 IS NOT NULL ");
                    }
                    sql1 += string.Format("        ORDER BY  1 desc ,2 desc,3 desc,4 desc,5 desc ");
                    sql1 += string.Format("        ) X ");
                }
                else
                {
                    sql1 = string.Format(" SELECT  ROWNUM AS L_NO ");
                    sql1 += string.Format("       ,MEAS_NO ");
                    sql1 += string.Format("       ,MEAS_DDTT ");
                    sql1 += string.Format("       ,WORK_TYPE");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                    sql1 += string.Format("       ,WORK_TEAM");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                    sql1 += string.Format("       ,POC_NO ");
                    sql1 += string.Format("       ,BUNDLE_NO ");
                    sql1 += string.Format("       ,HEAT ");
                    sql1 += string.Format("       ,STEEL ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("       ,ITEM ");
                    sql1 += string.Format("       ,ITEM_SIZE ");
                    sql1 += string.Format("       ,LENGTH ");
                    sql1 += string.Format("       ,PCS ");
                    sql1 += string.Format("       ,THEORY_WGT ");
                    sql1 += string.Format("       ,NET_WGT ");
                    sql1 += string.Format("       ,WGT_CHA ");
                    sql1 += string.Format("       ,WORK_METHOD ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_METHOD' AND CD_ID = X.WORK_METHOD) AS WORK_METHOD_NM ");
                    sql1 += string.Format("FROM   ( ");
                    sql1 += string.Format("        SELECT  A.MEAS_NO "); //--계중번호
                    sql1 += string.Format("               ,TO_CHAR(TO_DATE(A.MEAS_DDTT,'YYYYMMDDHH24MISS'),'YYYY-MM-DD HH24:MI:SS') AS MEAS_DDTT  "); //--계중일시
                    sql1 += string.Format("               ,A.WORK_TYPE ");
                    sql1 += string.Format("               ,NVL(A.WORK_TEAM, 'A') AS WORK_TEAM ");
                    sql1 += string.Format("               ,B.POC_NO ");
                    sql1 += string.Format("               ,B.BUNDLE_NO ");
                    sql1 += string.Format("               ,B.HEAT ");
                    sql1 += string.Format("               ,B.STEEL ");
                    sql1 += string.Format("               ,B.ITEM ");
                    sql1 += string.Format("               ,B.ITEM_SIZE ");
                    sql1 += string.Format("               ,B.LENGTH ");
                    sql1 += string.Format("               ,B.PCS ");
                    sql1 += string.Format("               ,B.THEORY_WGT ");
                    sql1 += string.Format("               ,A.NET_WGT ");
                    sql1 += string.Format("               ,A.NET_WGT - B.THEORY_WGT  AS WGT_CHA "); //--편차
                    sql1 += string.Format("               ,A.WORK_METHOD "); //--작업방법
                    sql1 += string.Format("        FROM   TB_WGT_WR  A ");
                    sql1 += string.Format("              ,TB_BND_WR  B ");
                    sql1 += string.Format("        WHERE  A.BUNDLE_NO  = B.BUNDLE_NO ");
                    sql1 += string.Format("        AND    A.MEAS_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                    sql1 += string.Format("        AND    A.LINE_GP    = '{0}' ", cd_id); //:P_LINE_GP
                    sql1 += string.Format("        AND    B.POC_NO     LIKE '%{0}%' || '%'", poctxt.Text);    //:P_POC_NO
                    sql1 += string.Format("        AND    B.HEAT       LIKE '%{0}%' || '%'", heattxt.Text);  //:P_HEAT
                    sql1 += string.Format("        AND    A.WORK_TYPE  LIKE '{0}' || '%' ", cd_id2);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                    sql1 += string.Format("        AND    A.WORK_TEAM  LIKE '{0}' || '%' ", cd_id3);
                    sql1 += string.Format("        AND    B.ITEM_SIZE       LIKE '%{0}%' || '%'", txtitem_size);  //:ITEM_SIZE
                    sql1 += string.Format("        AND    B.STEEL       LIKE '%{0}%' || '%'", gangjung_id);  //:STEEL
                    sql1 += string.Format("        AND    NVL(A.DEL_YN,'N')   <> 'Y'    ");
                    sql1 += string.Format("        AND    NVL(B.DEL_YN,'N')   <> 'Y' ");
                    sql1 += string.Format("        ORDER BY  1 desc ,2 desc,3 desc,4 desc,5 desc ");
                    sql1 += string.Format("        ) X ");
                }

                    olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                Cursor = Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                Cursor = Cursors.Default;
                grdMain.AutoResize = true;
                grdMain.AutoSizeCols();

                if (moddt.Rows.Count > 1 )
                {
                    UpdateTotals();
                }
               

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.Message + "]");
                return false;
            }
            return true;
        }

        private void UpdateTotals()
        {

            subtotalNo = 0;

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["THEORY_WGT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["NET_WGT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["PCS"].Index, "합계");
            

            AddSubtotalNo();
            grdMain.Rows.Frozen = GetAvailMinRow(grdMain) -1;
            //grdMain.Subtotal(AggregateEnum.Average, 1, -1, grdMain.Cols["THEORY_WGT"].Index, "평균");

            grdMain.AutoSizeCols();

            //grdMain.Rows.Fixed = GetAvailMinRow();
        }

        private void AddSubtotalNo()
        {
            ++subtotalNo;
        }
        private int GetAvailMinRow(C1FlexGrid grid)
        {
            return (grid.Rows.Fixed + subtotalNo);
        }


        #endregion 조회 설정

        #region 콤보박스 설정
        private void SetComboBox1()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
        }
        private void SetComboBox2()
        {
            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);
        }
        private void SetComboBox3()
        {
            cd.SetCombo(cboTEAM, "WORK_TEAM", "", true);
        }

        private void SetComboBox4()
        {
            cd.SetCombo(cbo_OnOff, "AUTO_MODE", "", true);
        }
        #endregion 콤보박스 설정

        #region 이벤트 설정

        private void Button_Click(object sender, EventArgs e)
        {

            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    setTextValue();
                    SetDataBinding();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

            #region 그리드 이벤트 설정
        private void GrdMain_RowColChange(object sender, EventArgs e)
        {
            int maxrow = 0;
            int oldSel = 0;
            string str = string.Empty;
            string temp = string.Empty;

            selectedrow = grdMain.RowSel;
        }



        private void grdMain_DoubleClick(object sender, EventArgs e)
        {


            if (grdMain.RowSel < GetAvailMinRow(grdMain))
            {
                return;
            }
            sMeasNo = grdMain[grdMain.RowSel, 1].ToString();
            WGTRsltPopUP popup = new WGTRsltPopUP(cboLine_GP.SelectedIndex, sMeasNo, scrAuth);
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            //SetDataBinding();
            Button_Click(btnDisplay, null);

        }

        #endregion 그리드 이벤트 설정

        #region 버튼 설정
        private void btnReg_Click(object sender, EventArgs e)
        {
            WGTRsltPopUP popup = new WGTRsltPopUP(cboLine_GP.SelectedIndex);
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            //SetDataBinding();
            Button_Click(btnDisplay, null);

        }
        private void btnSteel_Click(object sender, EventArgs e)
        {
            SearchSteelNm popup = new SearchSteelNm();
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            

        }
        #endregion 버튼 설정
       
        #region 컨트롤 설정
        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjong_Nm_tb.Text = "";
            gangjung_id = gangjong_id_tb.Text;
            
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        //사용자이벤트 생성
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
        }

        //강종ID(LostFocus)
        private void gangjong_id_tb_LostFocus(object sender, EventArgs e)
        {
            if (gangjong_id_tb.Text == "")
            {
                gangjong_Nm_tb.Text = "";
                gangjung_id = "";
            }
            else
            {
                gangjong_Nm_tb.Text = cd.Find_Steel_NM_By_ID(gangjong_id_tb.Text);

                if (gangjong_Nm_tb.Text.Length == 0)
                {
                    if (MessageBox.Show(" 해당 강종에 따른 강종명을 찾을 수 없습니다.", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        gangjong_Nm_tb.Text = "";
                        //gangjong_id_tb.Focus();
                        return;
                    }
                }
                else
                    gangjung_id = gangjong_id_tb.Text;
            }
        }

        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = cd_id;
        }
        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id2 = ((DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }

        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id3 = ((DictionaryList)cboTEAM.SelectedItem).fnValue;
        }

        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        private void setTextValue()
        {

            txtheat = heattxt.Text;
            txtpoc = poctxt.Text;
            txtitem_size = txtItemSize.Text;
            txtsteel = gangjong_id_tb.Text;
            txtsteel_nm = gangjong_Nm_tb.Text;

        }

        #endregion 컨트롤 설정

        #endregion

        private void btnDisplay_Click(object sender, EventArgs e)
        {

        }
    }
}