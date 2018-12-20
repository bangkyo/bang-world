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

namespace SystemControlClassLibrary
{
    //PRIIRslt
    public partial class CHMRslt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        DataTable grdMain_dt;

        private int selectedrow = 0;
        private string line_gp = "";
        private string work_type = "";
        private string work_team = "";

        private string txtheat = "";
        private string txtpoc = "";
        private string txtitem_size = "";
        private string txtsteel = "";
        private string txtsteel_nm = "";
        private string gangjong_id = "";

        private DateTime start_date;
        private DateTime end_date;

        clsStyle cs = new clsStyle();


        private string ownerNM = "";
        private string titleNM = "";

        private List<CHM_item> ChmList;
        #endregion 변수 설정

        #region CHM_item 설정
        public class CHM_item
        {
            private string _min_Cols_NM;
            private string _value_Cols_NM;
            private string _max_Cols_NM;
            private string _value_NG_NM;

            private decimal _min_value =0;
            private decimal _value =0;
            private decimal _max_value =0;
            
            public CHM_item(string name)//, decimal value, decimal min_value, decimal max_value)
            {
                _min_Cols_NM = "CHM_MIN_" + name;
                _value_Cols_NM = "CHM_VAL_" + name;
                _max_Cols_NM = "CHM_MAX_" + name;
                _value_NG_NM = "CHM_GOOD_NG_" + name;
            } 
            public string Value_Cols_NM
            {
                set { this._value_Cols_NM = value; }
                get { return this._value_Cols_NM; }
            }

            public string Value_NG_NM
            {
                set { this._value_NG_NM = value; }
                get { return this._value_NG_NM; }
            }

            public string Min_Cols_NM
            {
                set { this._min_Cols_NM = value; }
                get { return this._min_Cols_NM; }
            }

            public string Max_Cols_NM
            {
                set { this._max_Cols_NM = value; }
                get { return this._max_Cols_NM; }
            }

            public decimal Value
            {
                set { this._value = value; } 
                get { return this._value; }
            }

            public decimal Min_Value
            {
                set { this._min_value = value; }
                get { return this._min_value; }
            }

            public decimal Max_Value
            {
                set { this._max_value = value; }
                get { return this._max_value; }
            }
        }
        #endregion CHM_item 설정

        #region 로딩, 생성자 설정
        public CHMRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;
            InitializeComponent();

            Load += CHMRslt_Load;

            btnDisplay.Click += Button_Click;
            btnExcel.Click += Button_Click;

            grdMain.RowColChange += GrdMain_RowColChange;

            cboLine_GP.SelectedIndexChanged += CboLine_GP_SelectedIndexChanged;
            cbo_Work_Type.SelectedIndexChanged += cbo_Work_Type_SelectedIndexChanged;

            btnSteel.Click += btnSteel_Click;
        }

        private void CHMRslt_Load(object sender, System.EventArgs e)
        {
            InitControl();

            SetComboBox1();
            SetComboBox2();
            SetComboBox3();

            Button_Click(btnDisplay, null);

            EventCreate();      //사용자정의 이벤트
        }
        #endregion 로딩, 생성자 설정

        #region ComboBox 설정
        private void SetComboBox1()
        {
            //cd.SetCombo_3(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
            cd.SetComboS(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
        }
        private void SetComboBox2()
        {
            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);
        }
        private void SetComboBox3()
        {
            cd.SetCombo(cboTEAM, "WORK_TEAM", "", true);
        }
        #endregion ComboBox 설정

        #region Init Control 설정
        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(label1);
            cs.InitLabel(label2);
            cs.InitLabel(label3);
            cs.InitLabel(label4);
            cs.InitLabel(label5);
            cs.InitLabel(label6);
            cs.InitLabel(label7);
            cs.InitLabel(label8);
            cs.InitLabel(lblTEAM);

            cs.InitCombo(cboLine_GP, StringAlignment.Center);
            cs.InitCombo(cbo_Work_Type, StringAlignment.Center);
            cs.InitCombo(cboTEAM, StringAlignment.Center);

            cs.InitTextBox(txtPOC);
            cs.InitTextBox(txtHEAT);
            cs.InitTextBox(txtItemSize);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(gangjong_Nm_tb);
            clsStyle.Style.InitTextBox(txtRework_SEQ);

            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);


            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            //시간 데이터 default 값 적용 
            start_date = start_dt.Value = DateTime.Now;
            end_date = end_dt.Value = DateTime.Now;

            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);

            InitGrd_Main();

            // grd 초기화 데이터 생성
            MakeInitGrdData();
        }
        #endregion Init Control 설정

        #region Init Grid 설정
        private void MakeInitGrdData()
        {
            grdMain_dt = vf.CreateDataTable(grdMain);
        }

        private void InitGrd_Main()
        {
            cs.InitGrid_search(grdMain, false, 2, 1);
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            #region caption 설정
            grdMain[1, "L_NO"] = grdMain.Cols["L_NO"].Caption;
            //grdMain[1, "MILL_NO"] = grdMain.Cols["MILL_NO"].Caption;
            grdMain[1, "MILL_NO"] = grdMain.Cols["BUNDLE_NO"].Caption;
            grdMain[1, "PIECE_NO"] = grdMain.Cols["PIECE_NO"].Caption;

            //grdMain[1, "CHM_SNO"] = grdMain.Cols["CHM_SNO"].Caption;
            grdMain[0, "CHM_SNO"] = grdMain[1, "CHM_SNO"] = "  분석\n차수";//grdMain.Cols["LENGTH"].Caption;
            grdMain[1, "LOC"] = grdMain.Cols["LOC"].Caption;

            //grdMain[1, "BUNDLE_NO"] = grdMain.Cols["BUNDLE_NO"].Caption;
            grdMain[1, "MFG_DATE"] = grdMain.Cols["MFG_DATE"].Caption;
            grdMain[1, "EXIT_DDTT"] = grdMain.Cols["EXIT_DDTT"].Caption;
            grdMain[1, "WORK_TYPE_NM"] = grdMain.Cols["WORK_TYPE_NM"].Caption;
            grdMain[1, "WORK_TEAM_NM"] = grdMain.Cols["WORK_TEAM_NM"].Caption;
            grdMain[1, "POC_NO"] = grdMain.Cols["POC_NO"].Caption;
            grdMain[1, "HEAT"] = grdMain.Cols["HEAT"].Caption;
            grdMain[1, "STEEL"] = grdMain.Cols["STEEL"].Caption;
            grdMain[1, "STEEL_NM"] = grdMain.Cols["STEEL_NM"].Caption;
            grdMain[1, "ITEM"] = grdMain.Cols["ITEM"].Caption;
            grdMain[1, "ITEM_SIZE"] = grdMain.Cols["ITEM_SIZE"].Caption;
            grdMain[0, "LENGTH"] = grdMain[1, "LENGTH"] = " 길이\n(m)";//grdMain.Cols["LENGTH"].Caption;

            grdMain[1, "INSP_MODE_NM"] = grdMain.Cols["INSP_MODE_NM"].Caption;
            grdMain[1, "INSP_GP_NM"] = grdMain.Cols["INSP_GP_NM"].Caption;


            grdMain[1, "CHM_GOOD_NG"] = grdMain.Cols["CHM_GOOD_NG"].Caption;

            grdMain[1, "CHM_MIN_C"] = "Min";
            grdMain[1, "CHM_VAL_C"] = "값";
            grdMain[1, "CHM_MAX_C"] = "Max";
            grdMain[1, "CHM_MIN_SI"] = "Min";
            grdMain[1, "CHM_VAL_SI"] = "값";
            grdMain[1, "CHM_MAX_SI"] = "Max";
            grdMain[1, "CHM_MIN_MN"] = "Min";
            grdMain[1, "CHM_VAL_MN"] = "값";
            grdMain[1, "CHM_MAX_MN"] = "Max";
            grdMain[1, "CHM_MIN_NI"] = "Min";
            grdMain[1, "CHM_VAL_NI"] = "값";
            grdMain[1, "CHM_MAX_NI"] = "Max";
            grdMain[1, "CHM_MIN_CR"] = "Min";
            grdMain[1, "CHM_VAL_CR"] = "값";
            grdMain[1, "CHM_MAX_CR"] = "Max";
            grdMain[1, "CHM_MIN_MO"] = "Min";
            grdMain[1, "CHM_VAL_MO"] = "값";
            grdMain[1, "CHM_MAX_MO"] = "Max";
            grdMain[1, "CHM_MIN_V"] = "Min";
            grdMain[1, "CHM_VAL_V"] = "값";
            grdMain[1, "CHM_MAX_V"] = "Max";
            grdMain[1, "CHM_MIN_TI"] = "Min";
            grdMain[1, "CHM_VAL_TI"] = "값";
            grdMain[1, "CHM_MAX_TI"] = "Max";
            grdMain[1, "CHM_MIN_NB"] = "Min";
            grdMain[1, "CHM_VAL_NB"] = "값";
            grdMain[1, "CHM_MAX_NB"] = "Max";
            grdMain[1, "CHM_MIN_CU"] = "Min";
            grdMain[1, "CHM_VAL_CU"] = "값";
            grdMain[1, "CHM_MAX_CU"] = "Max";
            grdMain[1, "CHM_MIN_ZR"] = "Min";
            grdMain[1, "CHM_VAL_ZR"] = "값";
            grdMain[1, "CHM_MAX_ZR"] = "Max";
            grdMain[1, "CHM_MIN_P"] = "Min";
            grdMain[1, "CHM_VAL_P"] = "값";
            grdMain[1, "CHM_MAX_P"] = "Max";
            grdMain[1, "CHM_MIN_S"] = "Min";
            grdMain[1, "CHM_VAL_S"] = "값";
            grdMain[1, "CHM_MAX_S"] = "Max";
            #endregion caption 설정

            #region ChmList 설정

            ChmList = new List<CHM_item>();

            ChmList.Add(new CHM_item("C"));
            ChmList.Add(new CHM_item("SI"));
            ChmList.Add(new CHM_item("MN"));
            ChmList.Add(new CHM_item("NI"));
            ChmList.Add(new CHM_item("CR"));
            ChmList.Add(new CHM_item("MO"));
            ChmList.Add(new CHM_item("V"));
            ChmList.Add(new CHM_item("TI"));
            ChmList.Add(new CHM_item("NB"));
            ChmList.Add(new CHM_item("CU"));
            ChmList.Add(new CHM_item("ZR"));
            ChmList.Add(new CHM_item("P"));
            ChmList.Add(new CHM_item("S"));
            #endregion ChmList 설정

            #region TextAlign 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["PIECE_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["CHM_SNO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["LOC"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            //grdMain.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_NO_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["EXIT_DDTT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["POC_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["ITEM_SIZE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["LENGTH"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["INSP_MODE_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["INSP_GP_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["CHM_GOOD_NG"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["CHM_MIN_C"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_C"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_C"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_SI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_SI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_SI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_MN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_MN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_MN"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_NI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_NI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_NI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_CR"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_CR"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_CR"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_MO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_MO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_MO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_V"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_V"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_V"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_TI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_TI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_TI"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_NB"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_NB"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_NB"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_CU"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_CU"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_CU"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_ZR"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_ZR"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_ZR"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_P"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_P"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_P"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MIN_S"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_VAL_S"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain.Cols["CHM_MAX_S"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            #endregion TextAlign 설정

            #region width 설정
            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;

            grdMain.Cols["CHM_SNO"].Width = cs.CHM_SNO_Width-50;
            grdMain.Cols["LOC"].Width = cs.LOC_Width;

            grdMain.Cols["BUNDLE_NO"].Width = cs.BUNDLE_NO_Width;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width + 20;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width;
            grdMain.Cols["WORK_TEAM_NM"].Width = cs.WORK_TYPE_NM_Width;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 10;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;

            grdMain.Cols["INSP_MODE_NM"].Width = cs.INSP_MODE_NM_Width;
            grdMain.Cols["INSP_GP_NM"].Width = cs.INSP_GP_NM_Width;

            grdMain.Cols["CHM_MIN_C"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_C"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_C"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_SI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_SI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_SI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_MN"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_MN"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_MN"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_NI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_NI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_NI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_CR"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_CR"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_CR"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_MO"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_MO"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_MO"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_V"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_V"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_V"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_TI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_TI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_TI"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_NB"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_NB"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_NB"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_CU"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_CU"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_CU"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_ZR"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_ZR"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_ZR"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_P"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_P"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_P"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MIN_S"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_VAL_S"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_S"].Width = cs.Chm_value_Width;

            grdMain.Cols["CHM_MAX_S"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_S"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_S"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_S"].Width = cs.Chm_value_Width;
            grdMain.Cols["CHM_MAX_S"].Width = cs.Chm_value_Width;

            grdMain.Cols["CHM_GOOD_NG"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_C"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_SI"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_MN"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_NI"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_CR"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_MO"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_V"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_TI"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_NB"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_CU"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_ZR"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_P"].Width = 0;
            grdMain.Cols["CHM_GOOD_NG_S"].Width = 0;
            #endregion width 설정

            grdMain.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                grdMain.Cols[i].AllowMerging = true;

            }

            grdMain.Rows[0].AllowMerging = true;
        }
        #endregion Init Grid 설정

        #region SetDataBinding 설정
        private bool SetDataBinding()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT  ROWNUM AS L_NO ");
                sql1 += string.Format("       ,MILL_NO ");
                sql1 += string.Format("       ,PIECE_NO ");
                sql1 += string.Format("       ,CHM_SNO ");
                sql1 += string.Format("       ,LOC ");
                //sql1 += string.Format("       ,BUNDLE_NO ");
                sql1 += string.Format("       ,X.EXIT_DDTT  AS EXIT_DDTT");
                sql1 += string.Format("       ,TO_DATE(MFG_DATE, 'YYYY-MM-DD') AS MFG_DATE ");
                sql1 += string.Format("       ,WORK_TYPE ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                sql1 += string.Format("       ,WORK_TEAM ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                sql1 += string.Format("       ,POC_NO ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'INSP_MODE' AND CD_ID = X.INSP_MODE) AS INSP_MODE_NM ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'INSP_GP' AND CD_ID = X.INSP_GP) AS INSP_GP_NM");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.CHM_GOOD_NG) AS CHM_GOOD_NG  ");
                sql1 += string.Format("       ,CHM_MIN_C ");
                sql1 += string.Format("       ,CHM_VAL_C ");
                sql1 += string.Format("       ,CHM_MAX_C ");
                sql1 += string.Format("       ,CHM_MIN_SI ");
                sql1 += string.Format("       ,CHM_VAL_SI ");
                sql1 += string.Format("       ,CHM_MAX_SI ");
                sql1 += string.Format("       ,CHM_MIN_MN ");
                sql1 += string.Format("       ,CHM_VAL_MN ");
                sql1 += string.Format("       ,CHM_MAX_MN ");
                sql1 += string.Format("       ,CHM_MIN_NI ");
                sql1 += string.Format("       ,CHM_VAL_NI ");
                sql1 += string.Format("       ,CHM_MAX_NI ");
                sql1 += string.Format("       ,CHM_MIN_CR ");
                sql1 += string.Format("       ,CHM_VAL_CR ");
                sql1 += string.Format("       ,CHM_MAX_CR ");
                sql1 += string.Format("       ,CHM_MIN_MO ");
                sql1 += string.Format("       ,CHM_VAL_MO ");
                sql1 += string.Format("       ,CHM_MAX_MO ");
                sql1 += string.Format("       ,CHM_MIN_V ");
                sql1 += string.Format("       ,CHM_VAL_V ");
                sql1 += string.Format("       ,CHM_MAX_V ");
                sql1 += string.Format("       ,CHM_MIN_TI ");
                sql1 += string.Format("       ,CHM_VAL_TI ");
                sql1 += string.Format("       ,CHM_MAX_TI ");
                sql1 += string.Format("       ,CHM_MIN_NB ");
                sql1 += string.Format("       ,CHM_VAL_NB ");
                sql1 += string.Format("       ,CHM_MAX_NB ");
                sql1 += string.Format("       ,CHM_MIN_CU ");
                sql1 += string.Format("       ,CHM_VAL_CU ");
                sql1 += string.Format("       ,CHM_MAX_CU ");
                sql1 += string.Format("       ,CHM_MIN_ZR ");
                sql1 += string.Format("       ,CHM_VAL_ZR ");
                sql1 += string.Format("       ,CHM_MAX_ZR ");
                sql1 += string.Format("       ,CHM_MIN_P ");
                sql1 += string.Format("       ,CHM_VAL_P ");
                sql1 += string.Format("       ,CHM_MAX_P ");
                sql1 += string.Format("       ,CHM_MIN_S ");
                sql1 += string.Format("       ,CHM_VAL_S ");
                sql1 += string.Format("       ,CHM_MAX_S ");
                sql1 += string.Format("       ,CHM_GOOD_NG_C ");
                sql1 += string.Format("       ,CHM_GOOD_NG_SI ");
                sql1 += string.Format("       ,CHM_GOOD_NG_MN ");
                sql1 += string.Format("       ,CHM_GOOD_NG_NI ");
                sql1 += string.Format("       ,CHM_GOOD_NG_CR ");
                sql1 += string.Format("       ,CHM_GOOD_NG_MO ");
                sql1 += string.Format("       ,CHM_GOOD_NG_V ");
                sql1 += string.Format("       ,CHM_GOOD_NG_TI ");
                sql1 += string.Format("       ,CHM_GOOD_NG_NB ");
                sql1 += string.Format("       ,CHM_GOOD_NG_CU ");
                sql1 += string.Format("       ,CHM_GOOD_NG_ZR ");
                sql1 += string.Format("       ,CHM_GOOD_NG_P ");
                sql1 += string.Format("       ,CHM_GOOD_NG_S ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT  /*+ rule */  A.MILL_NO        AS MILL_NO ");
                sql1 += string.Format("               ,A.PIECE_NO       AS PIECE_NO ");
                sql1 += string.Format("               ,A.CHM_SNO        AS CHM_SNO ");
                sql1 += string.Format("               ,A.LOC            AS LOC ");
                //sql1 += string.Format("               ,(   ");
                //sql1 += string.Format("                 SELECT NVL(BUNDLE_NO, '') AS BUNDLE_NO   ");
                //sql1 += string.Format("                 FROM TB_CR_PIECE_WR   ");
                //sql1 += string.Format("                 WHERE MILL_NO   = A.MILL_NO  ");
                //sql1 += string.Format("                 AND   PIECE_NO  = A.PIECE_NO  ");
                //sql1 += string.Format("                 AND   LINE_GP  = A.LINE_GP  ");
                //sql1 += string.Format("                 AND   REWORK_SEQ  = A.REWORK_SEQ  ");
                //sql1 += string.Format("                 AND   ROUTING_CD  = 'P3'  ");
                //sql1 += string.Format("                ) AS BUNDLE_NO   ");
                sql1 += string.Format("               ,A.MFG_DATE       AS MFG_DATE ");
                sql1 += string.Format("               ,A.WORK_TYPE      AS WORK_TYPE ");
                sql1 += string.Format("               ,NVL(A.WORK_TEAM,'A')  AS WORK_TEAM ");
                sql1 += string.Format("               ,A.EXIT_DDTT  ");
                sql1 += string.Format("               ,B.POC_NO         AS POC_NO ");
                sql1 += string.Format("               ,B.HEAT           AS HEAT ");
                sql1 += string.Format("               ,B.ITEM           AS ITEM ");
                sql1 += string.Format("               ,B.ITEM_SIZE      AS ITEM_SIZE ");
                sql1 += string.Format("               ,B.STEEL          AS STEEL ");
                sql1 += string.Format("               ,B.LENGTH         AS LENGTH ");
                sql1 += string.Format("               ,A.INSP_MODE      AS INSP_MODE ");
                sql1 += string.Format("               ,A.INSP_GP        AS INSP_GP ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG,0)    AS CHM_GOOD_NG ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_C,0)      AS CHM_MIN_C ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_C,0)      AS CHM_VAL_C ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_C,0)      AS CHM_MAX_C ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_SI,0)     AS CHM_MIN_SI ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_SI,0)     AS CHM_VAL_SI ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_SI,0)     AS CHM_MAX_SI ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_MN,0)     AS CHM_MIN_MN ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_MN,0)     AS CHM_VAL_MN ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_MN,0)     AS CHM_MAX_MN ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_NI,0)     AS CHM_MIN_NI ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_NI,0)     AS CHM_VAL_NI ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_NI,0)     AS CHM_MAX_NI ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_CR,0)     AS CHM_MIN_CR ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_CR,0)     AS CHM_VAL_CR ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_CR,0)     AS CHM_MAX_CR ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_MO,0)     AS CHM_MIN_MO ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_MO,0)     AS CHM_VAL_MO ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_MO,0)     AS CHM_MAX_MO ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_V,0)      AS CHM_MIN_V ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_V,0)      AS CHM_VAL_V ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_V,0)      AS CHM_MAX_V ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_TI,0)     AS CHM_MIN_TI ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_TI,0)     AS CHM_VAL_TI ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_TI,0)     AS CHM_MAX_TI ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_NB,0)     AS CHM_MIN_NB ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_NB,0)     AS CHM_VAL_NB ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_NB,0)     AS CHM_MAX_NB ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_CU,0)     AS CHM_MIN_CU ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_CU,0)     AS CHM_VAL_CU ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_CU,0)     AS CHM_MAX_CU ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_ZR,0)     AS CHM_MIN_ZR ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_ZR,0)     AS CHM_VAL_ZR ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_ZR,0)     AS CHM_MAX_ZR ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_P,0)      AS CHM_MIN_P ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_P,0)      AS CHM_VAL_P ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_P,0)      AS CHM_MAX_P ");
                sql1 += string.Format("               ,NVL(A.CHM_MIN_S,0)      AS CHM_MIN_S ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_S,0)      AS CHM_VAL_S ");
                sql1 += string.Format("               ,NVL(A.CHM_MAX_S,0)      AS CHM_MAX_S ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_C,0)  AS CHM_GOOD_NG_C ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_SI,0) AS CHM_GOOD_NG_SI ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_MN,0) AS CHM_GOOD_NG_MN ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_NI,0) AS CHM_GOOD_NG_NI ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_CR,0) AS CHM_GOOD_NG_CR ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_MO,0) AS CHM_GOOD_NG_MO");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_V,0)  AS CHM_GOOD_NG_V");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_TI,0) AS CHM_GOOD_NG_TI");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_NB,0) AS CHM_GOOD_NG_NB");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_CU,0) AS CHM_GOOD_NG_CU");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_ZR,0) AS CHM_GOOD_NG_ZR");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_P,0)  AS CHM_GOOD_NG_P");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_S,0)  AS CHM_GOOD_NG_S");
                sql1 += string.Format("        FROM   TB_CHM_WR A ");
                //sql1 += string.Format("               ,TB_CR_ORD_BUNDLEINFO B ");
                //sql1 += string.Format("               ,TB_CHM_PLC_ORD  B ");
                sql1 += string.Format("               ,TB_CR_ORD  B ");
                sql1 += string.Format("        WHERE  SUBSTR(A.MILL_NO,1,7)  = B.POC_NO ");
                //sql1 += string.Format("        AND    A.MILL_NO  = C.MILL_NO(+) ");
                sql1 += string.Format("        AND    A.MFG_DATE   BETWEEN :P_FR_DATE AND :P_TO_DATE ");
                sql1 += string.Format("        AND    A.LINE_GP    = :P_LINE_GP ");
                sql1 += string.Format("        AND    B.POC_NO     LIKE '%' || :P_POC_NO || '%' ");
                sql1 += string.Format("        AND    B.HEAT       LIKE '%' || :P_HEAT || '%' ");
                sql1 += string.Format("        AND    NVL(A.WORK_TYPE, 'O')  LIKE :P_WORK_TYPE || '%' ");
                sql1 += string.Format("        AND    A.CHM_SNO  LIKE '%{0}%' ", txtRework_SEQ.Text);
                sql1 += string.Format("        AND    A.CHM_SNO  < 3 ");
                if (txtLOC.Text != "")
                    sql1 += string.Format("      AND A.LOC      LIKE '%{0}%' ", txtLOC.Text);
                sql1 += string.Format("        AND    B.STEEL      LIKE :P_STEEL || '%' ");
                sql1 += string.Format("        AND    B.ITEM_SIZE  LIKE :P_ITEM_SIZE || '%' ");
                sql1 += string.Format("        AND    NVL(A.WORK_TEAM, 'O')  LIKE :P_WORK_TEAM || '%' ");
                sql1 += string.Format("        ORDER BY  MFG_DATE DESC, EXIT_DDTT DESC, MILL_NO, PIECE_NO DESC ");
                //sql1 += string.Format("        ORDER BY  1,2,3,4,5 ");
                sql1 += string.Format("        ) X ");

                string[] parm = new string[9];
                parm[0] = ":P_LINE_GP|" + line_gp;
                parm[1] = ":P_FR_DATE|" + vf.Format(start_date, "yyyyMMdd");
                parm[2] = ":P_TO_DATE|" + vf.Format(end_date, "yyyyMMdd");
                parm[3] = ":P_POC_NO|" + txtpoc;
                parm[4] = ":P_HEAT|" + txtheat;
                parm[5] = ":P_WORK_TYPE|" + work_type;
                parm[6] = ":P_STEEL|" + gangjong_id;
                parm[7] = ":P_ITEM_SIZE|" + txtitem_size;
                parm[8] = ":P_WORK_TEAM|" + work_team;


                olddt = cd.FindDataTable(sql1, parm);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                

                if (moddt.Rows.Count > 0)
                {
                    CheckMinMax();
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
                grdMain.AutoResize = true;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            return true;
        }
        #endregion SetDataBinding 설정

        #region SetDataBinding_ 설정
        private bool SetDataBinding_()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT /*+ rule */   ROWNUM AS L_NO ");
                sql1 += string.Format("       ,MILL_NO ");
                sql1 += string.Format("       ,PIECE_NO ");
                sql1 += string.Format("       ,CHM_SNO ");
                sql1 += string.Format("       ,LOC ");
                //sql1 += string.Format("       ,BUNDLE_NO ");
                sql1 += string.Format("       ,X.EXIT_DDTT  AS EXIT_DDTT");
                sql1 += string.Format("       ,TO_DATE(MFG_DATE, 'YYYY-MM-DD') AS MFG_DATE ");
                sql1 += string.Format("       ,WORK_TYPE ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                sql1 += string.Format("       ,WORK_TEAM ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                sql1 += string.Format("       ,POC_NO ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'INSP_MODE' AND CD_ID = X.INSP_MODE) AS INSP_MODE_NM ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'INSP_GP' AND CD_ID = X.INSP_GP) AS INSP_GP_NM");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = X.CHM_GOOD_NG) AS CHM_GOOD_NG  ");
                sql1 += string.Format("       ,CHM_MIN_C ");
                sql1 += string.Format("       ,CHM_VAL_C ");
                sql1 += string.Format("       ,CHM_MAX_C ");
                sql1 += string.Format("       ,CHM_MIN_SI ");
                sql1 += string.Format("       ,CHM_VAL_SI ");
                sql1 += string.Format("       ,CHM_MAX_SI ");
                sql1 += string.Format("       ,CHM_MIN_MN ");
                sql1 += string.Format("       ,CHM_VAL_MN ");
                sql1 += string.Format("       ,CHM_MAX_MN ");
                sql1 += string.Format("       ,CHM_MIN_NI ");
                sql1 += string.Format("       ,CHM_VAL_NI ");
                sql1 += string.Format("       ,CHM_MAX_NI ");
                sql1 += string.Format("       ,CHM_MIN_CR ");
                sql1 += string.Format("       ,CHM_VAL_CR ");
                sql1 += string.Format("       ,CHM_MAX_CR ");
                sql1 += string.Format("       ,CHM_MIN_MO ");
                sql1 += string.Format("       ,CHM_VAL_MO ");
                sql1 += string.Format("       ,CHM_MAX_MO ");
                sql1 += string.Format("       ,CHM_MIN_V ");
                sql1 += string.Format("       ,CHM_VAL_V ");
                sql1 += string.Format("       ,CHM_MAX_V ");
                sql1 += string.Format("       ,CHM_MIN_TI ");
                sql1 += string.Format("       ,CHM_VAL_TI ");
                sql1 += string.Format("       ,CHM_MAX_TI ");
                sql1 += string.Format("       ,CHM_MIN_NB ");
                sql1 += string.Format("       ,CHM_VAL_NB ");
                sql1 += string.Format("       ,CHM_MAX_NB ");
                sql1 += string.Format("       ,CHM_MIN_CU ");
                sql1 += string.Format("       ,CHM_VAL_CU ");
                sql1 += string.Format("       ,CHM_MAX_CU ");
                sql1 += string.Format("       ,CHM_MIN_ZR ");
                sql1 += string.Format("       ,CHM_VAL_ZR ");
                sql1 += string.Format("       ,CHM_MAX_ZR ");
                sql1 += string.Format("       ,CHM_MIN_P ");
                sql1 += string.Format("       ,CHM_VAL_P ");
                sql1 += string.Format("       ,CHM_MAX_P ");
                sql1 += string.Format("       ,CHM_MIN_S ");
                sql1 += string.Format("       ,CHM_VAL_S ");
                sql1 += string.Format("       ,CHM_MAX_S ");
                sql1 += string.Format("       ,CHM_GOOD_NG_C ");
                sql1 += string.Format("       ,CHM_GOOD_NG_SI ");
                sql1 += string.Format("       ,CHM_GOOD_NG_MN ");
                sql1 += string.Format("       ,CHM_GOOD_NG_NI ");
                sql1 += string.Format("       ,CHM_GOOD_NG_CR ");
                sql1 += string.Format("       ,CHM_GOOD_NG_MO ");
                sql1 += string.Format("       ,CHM_GOOD_NG_V ");
                sql1 += string.Format("       ,CHM_GOOD_NG_TI ");
                sql1 += string.Format("       ,CHM_GOOD_NG_NB ");
                sql1 += string.Format("       ,CHM_GOOD_NG_CU ");
                sql1 += string.Format("       ,CHM_GOOD_NG_ZR ");
                sql1 += string.Format("       ,CHM_GOOD_NG_P ");
                sql1 += string.Format("       ,CHM_GOOD_NG_S ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT  /*+ rule */  A.MILL_NO        AS MILL_NO ");
                sql1 += string.Format("               ,A.PIECE_NO       AS PIECE_NO ");
                sql1 += string.Format("               ,A.CHM_SNO        AS CHM_SNO ");
                sql1 += string.Format("               ,A.LOC            AS LOC ");
                sql1 += string.Format("               ,(   ");
                sql1 += string.Format("                 SELECT NVL(BUNDLE_NO, '') AS BUNDLE_NO   ");
                sql1 += string.Format("                 FROM TB_CR_PIECE_WR   ");
                sql1 += string.Format("                 WHERE MILL_NO   = A.MILL_NO  ");
                sql1 += string.Format("                 AND   PIECE_NO  = A.PIECE_NO  ");
                sql1 += string.Format("                 AND   LINE_GP  = A.LINE_GP  ");
                sql1 += string.Format("                 AND   REWORK_SEQ  = A.REWORK_SEQ  ");
                sql1 += string.Format("                 AND   ROUTING_CD  = 'P3'  ");
                sql1 += string.Format("                ) AS BUNDLE_NO   ");
                sql1 += string.Format("               ,A.MFG_DATE       AS MFG_DATE ");
                sql1 += string.Format("               ,A.WORK_TYPE      AS WORK_TYPE ");
                sql1 += string.Format("               ,NVL(A.WORK_TEAM,'A')  AS WORK_TEAM ");
                sql1 += string.Format("               ,A.EXIT_DDTT  ");
                sql1 += string.Format("               ,B.POC_NO         AS POC_NO ");
                sql1 += string.Format("               ,B.HEAT           AS HEAT ");
                sql1 += string.Format("               ,B.ITEM           AS ITEM ");
                sql1 += string.Format("               ,B.ITEM_SIZE      AS ITEM_SIZE ");
                sql1 += string.Format("               ,B.STEEL          AS STEEL ");
                sql1 += string.Format("               ,B.LENGTH         AS LENGTH ");
                sql1 += string.Format("               ,A.INSP_MODE      AS INSP_MODE ");
                sql1 += string.Format("               ,A.INSP_GP        AS INSP_GP ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG,0)    AS CHM_GOOD_NG ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_C,0)      AS CHM_MIN_C ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_C,0)      AS CHM_VAL_C ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_C,0)      AS CHM_MAX_C ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_SI,0)     AS CHM_MIN_SI ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_SI,0)     AS CHM_VAL_SI ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_SI,0)     AS CHM_MAX_SI ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_MN,0)     AS CHM_MIN_MN ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_MN,0)     AS CHM_VAL_MN ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_MN,0)     AS CHM_MAX_MN ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_NI,0)     AS CHM_MIN_NI ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_NI,0)     AS CHM_VAL_NI ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_NI,0)     AS CHM_MAX_NI ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_CR,0)     AS CHM_MIN_CR ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_CR,0)     AS CHM_VAL_CR ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_CR,0)     AS CHM_MAX_CR ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_MO,0)     AS CHM_MIN_MO ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_MO,0)     AS CHM_VAL_MO ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_MO,0)     AS CHM_MAX_MO ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_V,0)      AS CHM_MIN_V ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_V,0)      AS CHM_VAL_V ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_V,0)      AS CHM_MAX_V ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_TI,0)     AS CHM_MIN_TI ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_TI,0)     AS CHM_VAL_TI ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_TI,0)     AS CHM_MAX_TI ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_NB,0)     AS CHM_MIN_NB ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_NB,0)     AS CHM_VAL_NB ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_NB,0)     AS CHM_MAX_NB ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_CU,0)     AS CHM_MIN_CU ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_CU,0)     AS CHM_VAL_CU ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_CU,0)     AS CHM_MAX_CU ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_ZR,0)     AS CHM_MIN_ZR ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_ZR,0)     AS CHM_VAL_ZR ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_ZR,0)     AS CHM_MAX_ZR ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_P,0)      AS CHM_MIN_P ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_P,0)      AS CHM_VAL_P ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_P,0)      AS CHM_MAX_P ");
                sql1 += string.Format("               ,NVL(C.CHM_MIN_S,0)      AS CHM_MIN_S ");
                sql1 += string.Format("               ,NVL(A.CHM_VAL_S,0)      AS CHM_VAL_S ");
                sql1 += string.Format("               ,NVL(C.CHM_MAX_S,0)      AS CHM_MAX_S ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_C,0)  AS CHM_GOOD_NG_C ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_SI,0) AS CHM_GOOD_NG_SI ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_MN,0) AS CHM_GOOD_NG_MN ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_NI,0) AS CHM_GOOD_NG_NI ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_CR,0) AS CHM_GOOD_NG_CR ");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_MO,0) AS CHM_GOOD_NG_MO");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_V,0)  AS CHM_GOOD_NG_V");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_TI,0) AS CHM_GOOD_NG_TI");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_NB,0) AS CHM_GOOD_NG_NB");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_CU,0) AS CHM_GOOD_NG_CU");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_ZR,0) AS CHM_GOOD_NG_ZR");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_P,0)  AS CHM_GOOD_NG_P");
                sql1 += string.Format("               ,NVL(A.CHM_GOOD_NG_S,0)  AS CHM_GOOD_NG_S");
                sql1 += string.Format("        FROM   TB_CHM_WR A ");
                sql1 += string.Format("               ,TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("               ,TB_CHM_PLC_ORD  C ");
                sql1 += string.Format("        WHERE  A.MILL_NO  = B.MILL_NO ");
                sql1 += string.Format("        AND    A.MILL_NO  = C.MILL_NO(+) ");
                sql1 += string.Format("        AND    A.MFG_DATE   BETWEEN :P_FR_DATE AND :P_TO_DATE ");
                sql1 += string.Format("        AND    A.LINE_GP    = :P_LINE_GP ");
                sql1 += string.Format("        AND    B.POC_NO     LIKE '%' || :P_POC_NO || '%' ");
                sql1 += string.Format("        AND    B.HEAT       LIKE '%' || :P_HEAT || '%' ");
                sql1 += string.Format("        AND    NVL(A.WORK_TYPE, 'A')  LIKE :P_WORK_TYPE || '%' ");
                sql1 += string.Format("        AND    A.CHM_SNO  LIKE '%{0}%' ", txtRework_SEQ.Text);
                sql1 += string.Format("        AND    A.CHM_SNO  < 3 ");
                sql1 += string.Format("        AND    B.STEEL      LIKE :P_STEEL || '%' ");
                sql1 += string.Format("        AND    B.ITEM_SIZE  LIKE :P_ITEM_SIZE || '%' ");
                sql1 += string.Format("        AND    NVL(A.WORK_TEAM, 'A')  LIKE :P_WORK_TEAM || '%' ");
                sql1 += string.Format("        ORDER BY  MFG_DATE DESC, EXIT_DDTT DESC, MILL_NO, PIECE_NO ");
                sql1 += string.Format("        ) X ");

                string[] parm = new string[9];
                parm[0] = ":P_LINE_GP|" + line_gp;
                parm[1] = ":P_FR_DATE|" + vf.Format(start_date, "yyyyMMdd");
                parm[2] = ":P_TO_DATE|" + vf.Format(end_date, "yyyyMMdd");
                parm[3] = ":P_POC_NO|" + txtpoc;
                parm[4] = ":P_HEAT|" + txtheat;
                parm[5] = ":P_WORK_TYPE|" + work_type;
                parm[6] = ":P_STEEL|" + gangjong_id;
                parm[7] = ":P_ITEM_SIZE|" + txtitem_size;
                parm[8] = ":P_WORK_TEAM|" + work_team;


                olddt = cd.FindDataTable(sql1, parm);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);


                if (moddt.Rows.Count > 0)
                {
                    CheckMinMax();
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
                grdMain.AutoResize = true;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            return true;
        }
        #endregion SetDataBinding 설정
        #region 이벤트 설정
        private void btnSteel_Click(object sender, EventArgs e)
        {
            SearchSteelNm popup = new SearchSteelNm();
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }

        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = line_gp;
        }

        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_type = ((ComLib.DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }

        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_team = ((ComLib.DictionaryList)cboTEAM.SelectedItem).fnValue;
        }

        private void End_dt_ValueChanged(object sender, EventArgs e)
        {
            end_date = end_dt.Value;

            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void Start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_date = start_dt.Value;

            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void GrdMain_RowColChange(object sender, EventArgs e)
        {
            string str = string.Empty;
            string temp = string.Empty;

            selectedrow = grdMain.RowSel;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    int start_dates = vf.CLng(start_dt.Value.ToString("yyyyMMdd"));
                    setTextValue();
                    //SetDataBinding();
                    if (start_dates > 20161211)
                    {
                        SetDataBinding();
                    }
                    else if (start_dates <= 20161211)
                    {
                        SetDataBinding_();
                    }
                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
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
            txtheat = txtHEAT.Text;
            txtpoc = txtPOC.Text;
            txtitem_size = txtItemSize.Text;
            txtsteel = gangjong_id_tb.Text;
            txtsteel_nm = gangjong_Nm_tb.Text;
        }

        private void CheckMinMax()
        {
            CellRange rg;

            for (int row = 2; row < grdMain.Rows.Count; row++)
            {
                foreach (Column col in grdMain.Cols)
                {
                    foreach (CHM_item item in ChmList)
                    {
                        if (col.Name == item.Value_Cols_NM)
                        {
                            //NG COLUMN이 숨겨져있슴. 디자인모드에서
                            rg = grdMain.GetCellRange(row, grdMain.Cols[item.Value_Cols_NM].Index);
                            if (grdMain.GetData(row, item.Value_NG_NM).ToString() != "OK")
                            {
                                rg.StyleNew.ForeColor = Color.Red;
                            }

                            rg = grdMain.GetCellRange(row, grdMain.Cols[item.Min_Cols_NM].Index);
                            rg.StyleNew.BackColor = Color.LightGray;

                            rg = grdMain.GetCellRange(row, grdMain.Cols[item.Max_Cols_NM].Index);
                            rg.StyleNew.BackColor = Color.LightGray;

                            rg = grdMain.GetCellRange(row, grdMain.Cols[item.Value_Cols_NM].Index);
                            rg.StyleNew.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjong_id = gangjong_id_tb.Text;
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
                gangjong_id = "";
            }
            else
            {
                gangjong_Nm_tb.Text = cd.Find_Steel_NM_By_ID(gangjong_id_tb.Text);

                if (gangjong_Nm_tb.Text.Length == 0)
                {
                    if (MessageBox.Show(" 해당 강종에 따른 강종명을 찾을 수 없습니다.", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        gangjong_Nm_tb.Text = "";
                        return;
                    }
                }
                else
                    gangjong_id = gangjong_id_tb.Text;
            }
        }
        private void txtRework_SEQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }
        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }
        #endregion 이벤트 설정

        private void btnDisplay_Click(object sender, EventArgs e)
        {

        }
    }
}
