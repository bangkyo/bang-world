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
    public partial class CHFRslt : Form
    {
        #region 변수선언
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        DataTable olddt = new DataTable();
        private string sql = "";

        private static string cd_id = "";
        private static string cd_id2 = "";
        private static string cd_id3 = "";
        private string current_Tab = "TabP1";
        private string gangjung_id = "";
        private static string ownerNM = "";
        private static string titleNM = "";
        bool _first = true;
        private int subtotalNo;
        private C1FlexGrid selectedGrid;
        #endregion

        #region 화면
        public CHFRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void FrmCHFRControl_Load(object sender, System.EventArgs e)
        {
            InitControl();

            SetComboBox();

            TabOpt.SelectedIndex = 0;

            // grdMain_setting();

            EventCreate();      //사용자정의 이벤트  

            btnDisplay_Click(null, null);
        }

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(lblHeat);
            cs.InitLabel(lblSteel);
            cs.InitLabel(lblLine);
            cs.InitLabel(lblPoc);
            cs.InitLabel(lblWorkType);
            cs.InitLabel(lblMfgDate);
            cs.InitLabel(lblItemSize);
            cs.InitLabel(lblTEAM);

            cs.InitCombo(cboTEAM, StringAlignment.Center);
            cs.InitCombo(cboLine_GP, StringAlignment.Center);
            cs.InitCombo(cbo_Work_Type, StringAlignment.Center);

            cs.InitTextBox(txtPoc);
            cs.InitTextBox(txtHeat);
            cs.InitTextBox(txtItemSize);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(gangjong_Nm_tb);


            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            cs.InitTab(TabOpt);

            InitGrd_Main1();
            InitGrd_Main2();
            InitGrd_Main3();
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


        #endregion

        #region Init Grid Main 설정
        private void InitGrd_Main1()
        {
            clsStyle.Style.InitGrid_search(grdMain1);

            grdMain1.AllowEditing = false;
            grdMain1.AutoResize = true;

            //생산실적-----------------------------------------------------------------------

            grdMain1.Cols["L_NO"].Caption = "NO";
            grdMain1.Cols["MFG_DATE"].Caption = "작업일자";
            grdMain1.Cols["WORK_TYPE"].Caption = "근";
            grdMain1.Cols["WORK_TYPE_NM"].Caption = "근";
            grdMain1.Cols["WORK_TEAM"].Caption = "조";
            grdMain1.Cols["WORK_TEAM_NM"].Caption = "조";
            grdMain1.Cols["POC_NO"].Caption = "POC ";
            grdMain1.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain1.Cols["HEAT"].Caption = "HEAT";
            grdMain1.Cols["STEEL"].Caption = "강종";
            grdMain1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain1.Cols["ITEM"].Caption = "품목";
            grdMain1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain1.Cols["LENGTH"].Caption = "길이(m)";
            grdMain1.Cols["MILL_PCS"].Caption = "압연본수";
            grdMain1.Cols["MILL_WGT"].Caption = "압연중량(kg)";

            grdMain1.Cols["STR_PCS"].Caption = "교정본수";
            grdMain1.Cols["STR_WGT"].Caption = "교정중량(kg)";
            grdMain1.Cols["SHF_PCS"].Caption = "면취본수";
            grdMain1.Cols["SHF_WGT"].Caption = "면취중량(kg)";

            grdMain1.Cols[2].Visible = false;

            grdMain1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain1.Cols["MFG_DATE"].Width = cs.Date_8_Width + 20;
            grdMain1.Cols["WORK_TYPE"].Width = 0;
            grdMain1.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width + 0;
            grdMain1.Cols["WORK_TEAM"].Width = 0;
            grdMain1.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width + 0;
            grdMain1.Cols["POC_NO"].Width = cs.POC_NO_Width -30;                    //변경전 90
            grdMain1.Cols["MILL_NO"].Width = cs.Mill_No_Width ;              //변경전 130;
            grdMain1.Cols["HEAT"].Width = cs.HEAT_Width -10;                         //변경전 90;
            grdMain1.Cols["STEEL"].Width = cs.STEEL_Width;                  //변경전 50;
            grdMain1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width +10;            //변경전 100;
            grdMain1.Cols["ITEM"].Width = cs.ITEM_Width + 15;                    //변경전 60;
            grdMain1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;          //변경전 70;
            grdMain1.Cols["LENGTH"].Width = cs.LENGTH_Width -15;                //변경전 70;
            grdMain1.Cols["MILL_PCS"].Width = cs.STR_PCS_Width - 15;
            grdMain1.Cols["MILL_WGT"].Width = cs.STR_PCS_Width - 5;
            grdMain1.Cols["STR_PCS"].Width = cs.STR_PCS_Width - 15;                   //변경전 100;
            grdMain1.Cols["STR_WGT"].Width = 0;
            grdMain1.Cols["SHF_PCS"].Width = cs.SHF_PCS_Width - 15;                   //변경전 110;
            grdMain1.Cols["SHF_WGT"].Width = 0;

            grdMain1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain1.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain1.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain1.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain1.Cols["WORK_TEAM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grdMain1.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TEAM_NM_TextAlign;
            grdMain1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain1.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain1.Cols["STR_PCS"].TextAlign = cs.STR_PCS_TextAlign;
            grdMain1.Cols["STR_WGT"].TextAlign = cs.STR_WGT_TextAlign;
            grdMain1.Cols["SHF_PCS"].TextAlign = cs.SHF_PCS_TextAlign;
            grdMain1.Cols["SHF_WGT"].TextAlign = cs.SHF_WGT_TextAlign;

            grdMain1.Rows[0].TextAlign = TextAlignEnum.CenterCenter; // 캡션 정렬
            grdMain1.ExtendLastCol = true;

            grdMain1.Tree.Column = 1;
        }
        private void InitGrd_Main2()
        {
            clsStyle.Style.InitGrid_search(grdMain2_1);

            grdMain2_1.AllowEditing = false;
            grdMain2_1.AutoResize = true;
            //조업정보(라인별로 그리드 캡션 변경)-------------------------------------------------

            grdMain2_1.Cols["L_NO"].Caption = "NO";
            grdMain2_1.Cols["WORK_DATE"].Caption = "작업일자";
            grdMain2_1.Cols["WORK_TIME"].Caption = "작업시각";
            grdMain2_1.Cols["POC_NO"].Caption = "POC";
            grdMain2_1.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain2_1.Cols["HEAT"].Caption = "HEAT";
            grdMain2_1.Cols["STEEL"].Caption = "강종";
            grdMain2_1.Cols["STEEL_NM"].Caption = "강종명";
            grdMain2_1.Cols["ITEM"].Caption = "품목";
            grdMain2_1.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain2_1.Cols["LENGTH"].Caption = "길이(m)";
            grdMain2_1.Cols["COL_01"].Caption = "SERVO현재값1(mm)";
            grdMain2_1.Cols["COL_02"].Caption = "SERVO설정값1(mm)";
            grdMain2_1.Cols["COL_03"].Caption = "SERVO설정값2(mm)";
            grdMain2_1.Cols["COL_04"].Caption = "SERVO현재값2(mm)";
            grdMain2_1.Cols["COL_05"].Caption = "롤드라이브현재값1(Hz)";
            grdMain2_1.Cols["COL_06"].Caption = "롤드라이브설정값1(Hz)";
            grdMain2_1.Cols["COL_07"].Caption = "롤드라이브현재값2(Hz)";
            grdMain2_1.Cols["COL_08"].Caption = "롤드라이브설정값2(Hz)";
            grdMain2_1.Cols["COL_09"].Caption = "롤드라이브현재값1(Hz)";
            grdMain2_1.Cols["COL_10"].Caption = "롤드라이브설정값1(Hz)";
            grdMain2_1.Cols["COL_11"].Caption = "롤드라이브현재값2(Hz)";
            grdMain2_1.Cols["COL_12"].Caption = "롤드라이브설정값2(Hz)";
            grdMain2_1.Cols["COL_13"].Caption = "롤드라이브현재값Exit(Hz)";
            grdMain2_1.Cols["COL_14"].Caption = "롤드라이브설정값Exit(Hz)";
            grdMain2_1.Cols["COL_15"].Caption = "STOPPER1";
            grdMain2_1.Cols["COL_16"].Caption = "받침대1";
            grdMain2_1.Cols["COL_17"].Caption = "소재간격1";
            grdMain2_1.Cols["COL_18"].Caption = "STOPPER2";
            grdMain2_1.Cols["COL_19"].Caption = "받침대2";
            grdMain2_1.Cols["COL_20"].Caption = "소재간격2";

            grdMain2_1.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain2_1.Cols["WORK_DATE"].Width = cs.Date_8_Width;           //변경전 130;
            grdMain2_1.Cols["WORK_TIME"].Width = cs.Date_8_Width;           //변경전 100;
            grdMain2_1.Cols["POC_NO"].Width = cs.POC_NO_Width;              //변경전 80;
            grdMain2_1.Cols["MILL_NO"].Width = cs.Mill_No_Width + 20;       //변경전 130;
            grdMain2_1.Cols["HEAT"].Width = cs.HEAT_Width - 20;             //변경전 80;
            grdMain2_1.Cols["STEEL"].Width = cs.STEEL_Width;                //변경전 80;
            grdMain2_1.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;          //변경전 80;
            grdMain2_1.Cols["ITEM"].Width = cs.ITEM_Width;                  //변경전 80;
            grdMain2_1.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35;   //변경전 80;
            grdMain2_1.Cols["LENGTH"].Width = cs.LENGTH_Width;              //변경전 80;

            grdMain2_1.Cols["COL_01"].Width = cs.Longer_Value_Width + 10;
            grdMain2_1.Cols["COL_02"].Width = cs.Longer_Value_Width + 10;
            grdMain2_1.Cols["COL_03"].Width = cs.Longer_Value_Width + 10;
            grdMain2_1.Cols["COL_04"].Width = cs.Longer_Value_Width + 10;
            grdMain2_1.Cols["COL_05"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_06"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_07"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_08"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_09"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_10"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_11"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_12"].Width = cs.Longer_Value_Width + 30;//변경전 180;
            grdMain2_1.Cols["COL_13"].Width = cs.Longer_Value_Width + 40;//변경전 190;
            grdMain2_1.Cols["COL_14"].Width = cs.Longer_Value_Width + 40;//변경전 190;
            grdMain2_1.Cols["COL_15"].Width = cs.Short_Value_Width;      //변경전 90;
            grdMain2_1.Cols["COL_16"].Width = cs.Short_Value_Width;      //변경전 90;
            grdMain2_1.Cols["COL_17"].Width = cs.Short_Value_Width;      //변경전 90;
            grdMain2_1.Cols["COL_18"].Width = cs.Short_Value_Width;      //변경전 90;
            grdMain2_1.Cols["COL_19"].Width = cs.Short_Value_Width;      //변경전 90;
            grdMain2_1.Cols["COL_20"].Width = cs.Short_Value_Width;      //변경전 90;

            grdMain2_1.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain2_1.Cols["WORK_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain2_1.Cols["WORK_TIME"].TextAlign = cs.WORK_TIME_TextAlign;
            grdMain2_1.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain2_1.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain2_1.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain2_1.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain2_1.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain2_1.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain2_1.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain2_1.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain2_1.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_16"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_17"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_18"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_19"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_1.Cols["COL_20"].TextAlign = cs.VALUE_RIGHT_TextAlign;

            grdMain2_1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter; // 캡션 정렬
            grdMain2_1.ExtendLastCol = true;
        }
        private void InitGrd_Main3()
        {
            clsStyle.Style.InitGrid_search(grdMain2_2);

            grdMain2_2.AllowEditing = false;
            grdMain2_2.AutoResize = true;
            //조업정보(라인별로 그리드 캡션 변경)-------------------------------------------------

            grdMain2_2.Cols["L_NO"].Caption = "NO";
            grdMain2_2.Cols["WORK_DATE"].Caption = "작업일자";
            grdMain2_2.Cols["WORK_TIME"].Caption = "작업시각";
            grdMain2_2.Cols["POC_NO"].Caption = "POC";
            grdMain2_2.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain2_2.Cols["HEAT"].Caption = "HEAT";
            grdMain2_2.Cols["STEEL"].Caption = "강종";
            grdMain2_2.Cols["STEEL_NM"].Caption = "강종명";
            grdMain2_2.Cols["ITEM"].Caption = "품목";
            grdMain2_2.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain2_2.Cols["LENGTH"].Caption = "길이(m)";
            grdMain2_2.Cols["COL_01"].Caption = "KICKER SIZE조정현재값1";
            grdMain2_2.Cols["COL_02"].Caption = "KICKER SIZE조정설정값1";
            grdMain2_2.Cols["COL_03"].Caption = "ROLLER C/V드라이브HZ현재값1";
            grdMain2_2.Cols["COL_04"].Caption = "ROLLER C/V드라이브모터전류현재값1";
            grdMain2_2.Cols["COL_05"].Caption = "ROLLER C/V드라이브HZ HIGH설정값1";
            grdMain2_2.Cols["COL_06"].Caption = "ROLLER C/V드라이브HZ SLOW TIME설정값1";
            grdMain2_2.Cols["COL_07"].Caption = "ROLLER C/V드라이브HZ SLOW설정값1";
            grdMain2_2.Cols["COL_08"].Caption = "SCREW피딩드라이브HZ현재값1";
            grdMain2_2.Cols["COL_09"].Caption = "SCREW피딩모터전류현재값1";
            grdMain2_2.Cols["COL_10"].Caption = "SCREW피딩드라이브HZ설정값1";
            grdMain2_2.Cols["COL_11"].Caption = "GRIND드라이브HZ현재값1";
            grdMain2_2.Cols["COL_12"].Caption = "GRIND모터전류현재값1";
            grdMain2_2.Cols["COL_13"].Caption = "GRIND드라이브HZ설정값1";
            grdMain2_2.Cols["COL_14"].Caption = "STOPPER현재값1";
            grdMain2_2.Cols["COL_15"].Caption = "STOPPER설정값1";
            grdMain2_2.Cols["COL_16"].Caption = "GUIDE현재값1";
            grdMain2_2.Cols["COL_17"].Caption = "GUIDE설정값1";
            grdMain2_2.Cols["COL_18"].Caption = "SERVO현재값1";
            grdMain2_2.Cols["COL_19"].Caption = "SERVO JOG UP속도설정값1";
            grdMain2_2.Cols["COL_20"].Caption = "SERVO JOG DOWN속도설정값1";
            grdMain2_2.Cols["COL_21"].Caption = "SERVO UP속도설정값1";
            grdMain2_2.Cols["COL_22"].Caption = "SERVO UP위치설정값1";
            grdMain2_2.Cols["COL_23"].Caption = "SERVO툴변경위치설정값1";
            grdMain2_2.Cols["COL_24"].Caption = "SERVO툴속도설정값1";
            grdMain2_2.Cols["COL_25"].Caption = "SERVO툴위치설정값1";
            grdMain2_2.Cols["COL_26"].Caption = "SERVO툴위치인칭속도설정값1";
            grdMain2_2.Cols["COL_27"].Caption = "SERVO툴위치인칭위치설정값1";
            grdMain2_2.Cols["COL_28"].Caption = "KICKER SIZE조정현재값2";
            grdMain2_2.Cols["COL_29"].Caption = "KICKER SIZE조정설정값2";
            grdMain2_2.Cols["COL_30"].Caption = "ROLLER C/V드라이브HZ현재값2";
            grdMain2_2.Cols["COL_31"].Caption = "ROLLER C/V드라이브모터전류현재값2";
            grdMain2_2.Cols["COL_32"].Caption = "ROLLER C/V드라이브HZ HIGH설정값2";
            grdMain2_2.Cols["COL_33"].Caption = "ROLLER C/V드라이브HZ SLOW TIME설정값2";
            grdMain2_2.Cols["COL_34"].Caption = "ROLLER C/V드라이브HZ SLOW설정값2";
            grdMain2_2.Cols["COL_35"].Caption = "SCREW피딩드라이브HZ현재값2";
            grdMain2_2.Cols["COL_36"].Caption = "SCREW피딩모터전류현재값2";
            grdMain2_2.Cols["COL_37"].Caption = "SCREW피딩드라이브HZ설정값2";
            grdMain2_2.Cols["COL_38"].Caption = "GRIND드라이브HZ현재값2";
            grdMain2_2.Cols["COL_39"].Caption = "GRIND모터전류현재값2";
            grdMain2_2.Cols["COL_40"].Caption = "GRIND드라이브HZ설정값2";
            grdMain2_2.Cols["COL_41"].Caption = "STOPPER현재값2";
            grdMain2_2.Cols["COL_42"].Caption = "STOPPER설정값2";
            grdMain2_2.Cols["COL_43"].Caption = "GUIDE현재값2";
            grdMain2_2.Cols["COL_44"].Caption = "GUIDE설정값2";
            grdMain2_2.Cols["COL_45"].Caption = "SERVO현재값2";
            grdMain2_2.Cols["COL_46"].Caption = "SERVO JOG UP속도설정값2";
            grdMain2_2.Cols["COL_47"].Caption = "SERVO JOG DOWN속도설정값2";
            grdMain2_2.Cols["COL_48"].Caption = "SERVO UP속도설정값2";
            grdMain2_2.Cols["COL_49"].Caption = "SERVO UP위치설정값2";
            grdMain2_2.Cols["COL_50"].Caption = "SERVO툴변경위치설정값2";
            grdMain2_2.Cols["COL_51"].Caption = "SERVO툴속도설정값2";
            grdMain2_2.Cols["COL_52"].Caption = "SERVO툴위치설정값2";
            grdMain2_2.Cols["COL_53"].Caption = "SERVO툴위치인칭속도설정값2";
            grdMain2_2.Cols["COL_54"].Caption = "SERVO툴위치인칭위치설정값2";
            grdMain2_2.Cols["COL_55"].Caption = "SERVO 툴위치 인칭 상승카운터2";
            grdMain2_2.Cols["COL_56"].Caption = "SERVO 툴위치 인칭 하강카운터2";
            grdMain2_2.Cols["COL_57"].Caption = "SERVO 툴위치 인칭 상승카운터1";
            grdMain2_2.Cols["COL_58"].Caption = "SERVO 툴위치 인칭 하강카운터1";


            grdMain2_2.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain2_2.Cols["WORK_DATE"].Width = cs.Date_8_Width;           //변경전 130;
            grdMain2_2.Cols["WORK_TIME"].Width = cs.Date_8_Width;           //변경전 100;
            grdMain2_2.Cols["POC_NO"].Width = cs.POC_NO_Width;              //변경전 80;
            grdMain2_2.Cols["MILL_NO"].Width = cs.Mill_No_Width + 20;       //변경전 130;
            grdMain2_2.Cols["HEAT"].Width = cs.HEAT_Width - 20;             //변경전 80;
            grdMain2_2.Cols["STEEL"].Width = cs.STEEL_Width;                //변경전 80;
            grdMain2_2.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;          //변경전 80;
            grdMain2_2.Cols["ITEM"].Width = cs.ITEM_Width;                  //변경전 80;
            grdMain2_2.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 35;   //변경전 80;
            grdMain2_2.Cols["LENGTH"].Width = cs.LENGTH_Width;              //변경전 80;
            grdMain2_2.Cols["COL_01"].Width = cs.Longest_Value_Width - 20;  //변경전200;
            grdMain2_2.Cols["COL_02"].Width = cs.Longest_Value_Width - 20;  //변경전200;
            grdMain2_2.Cols["COL_03"].Width = cs.Longest_Value_Width + 60;  //변경전280;
            grdMain2_2.Cols["COL_04"].Width = cs.Longest_Value_Width + 80;  //변경전300;
            grdMain2_2.Cols["COL_05"].Width = cs.Longest_Value_Width + 80;  //변경전300;
            grdMain2_2.Cols["COL_06"].Width = cs.Longest_Value_Width + 110; //변경전330;
            grdMain2_2.Cols["COL_07"].Width = cs.Longest_Value_Width + 110; //변경전330;
            grdMain2_2.Cols["COL_08"].Width = cs.Longest_Value_Width + 10;  //변경전230;
            grdMain2_2.Cols["COL_09"].Width = cs.Longest_Value_Width + 10;  //변경전230;
            grdMain2_2.Cols["COL_10"].Width = cs.Longest_Value_Width + 10;  //변경전230;
            grdMain2_2.Cols["COL_11"].Width = cs.Longest_Value_Width + 10;  //변경전230;
            grdMain2_2.Cols["COL_12"].Width = cs.Longest_Value_Width + 10;  //변경전230;
            grdMain2_2.Cols["COL_13"].Width = cs.Longest_Value_Width + 10;  //변경전230;
            grdMain2_2.Cols["COL_14"].Width = cs.Middle_Value_Width + 20;   //변경전140;
            grdMain2_2.Cols["COL_15"].Width = cs.Middle_Value_Width + 20;   //변경전140;
            grdMain2_2.Cols["COL_16"].Width = cs.Middle_Value_Width + 20;   //변경전140;
            grdMain2_2.Cols["COL_17"].Width = cs.Middle_Value_Width + 20;   //변경전140;
            grdMain2_2.Cols["COL_18"].Width = cs.Middle_Value_Width + 20;   //변경전140;
            grdMain2_2.Cols["COL_19"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_20"].Width = cs.Longest_Value_Width + 30;  //변경전250;
            grdMain2_2.Cols["COL_21"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_22"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_23"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_24"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_25"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_26"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_27"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_28"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_29"].Width = cs.Longest_Value_Width;       //변경전220;
            grdMain2_2.Cols["COL_30"].Width = cs.Longest_Value_Width + 30;  //변경전250;
            grdMain2_2.Cols["COL_31"].Width = cs.Longest_Value_Width + 110; //변경전330;
            grdMain2_2.Cols["COL_32"].Width = cs.Longest_Value_Width + 110; //변경전330;
            grdMain2_2.Cols["COL_33"].Width = cs.Longest_Value_Width + 110; //변경전330;
            grdMain2_2.Cols["COL_34"].Width = cs.Longest_Value_Width + 110; //변경전330;
            grdMain2_2.Cols["COL_35"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_36"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_37"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_38"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_39"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_40"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_41"].Width = cs.Longer_Value_Width;        //변경전150;
            grdMain2_2.Cols["COL_42"].Width = cs.Longer_Value_Width;        //변경전150;
            grdMain2_2.Cols["COL_43"].Width = cs.Longer_Value_Width;        //변경전150;
            grdMain2_2.Cols["COL_44"].Width = cs.Longer_Value_Width;        //변경전150;
            grdMain2_2.Cols["COL_45"].Width = cs.Longer_Value_Width;        //변경전150;
            grdMain2_2.Cols["COL_46"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_47"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_48"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_49"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_50"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_51"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_52"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_53"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_54"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_55"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_56"].Width = cs.Longest_Value_Width + 20;  //변경전240;
            grdMain2_2.Cols["COL_57"].Width = cs.Longest_Value_Width + 20;
            grdMain2_2.Cols["COL_58"].Width = cs.Longest_Value_Width + 20;

            grdMain2_2.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain2_2.Cols["WORK_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain2_2.Cols["WORK_TIME"].TextAlign = cs.WORK_TIME_TextAlign;
            grdMain2_2.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain2_2.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain2_2.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain2_2.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain2_2.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain2_2.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain2_2.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain2_2.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain2_2.Cols["COL_01"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_02"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_03"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_04"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_05"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_06"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_07"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_08"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_09"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_10"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_11"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_12"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_13"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_14"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_15"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_16"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_17"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_18"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_19"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_20"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_21"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_22"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_23"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_24"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_25"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_26"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_27"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_28"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_29"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_30"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_31"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_32"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_33"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_34"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_35"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_36"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_37"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_38"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_39"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_40"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_41"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_42"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_43"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_44"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_45"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_46"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_47"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_48"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_49"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_50"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_51"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_52"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_53"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_54"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_55"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_56"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_57"].TextAlign = cs.VALUE_RIGHT_TextAlign;
            grdMain2_2.Cols["COL_58"].TextAlign = cs.VALUE_RIGHT_TextAlign;

            grdMain2_2.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter; // 캡션 정렬
            grdMain2_2.ExtendLastCol = true;
        }

        #endregion Init Grid Main 설정

        #region "콤보박스 설정"
        private void SetComboBox()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);

            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);

            cd.SetCombo(cboTEAM, "WORK_TEAM", "", true);
        }
        #endregion

        #region 조회
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            SetDataBinding();
        }

        private void SetDataBinding()
        {

            try
            {
                //쿼리 가져욤
                sql = QuerySetup();

                olddt = cd.FindDataTable(sql);
                Cursor.Current = Cursors.AppStarting;
                selectedGrid.SetDataBinding(olddt, null, true);
                Cursor.Current = Cursors.Default;
                if (current_Tab == "TabP2" )
                {
                    selectedGrid.AutoSizeCols();
                }
                else if (current_Tab == "TabP1")
                {
                    UpdateTotals(selectedGrid);
                }


                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), olddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }

            
        }
        #endregion

        private void UpdateTotals(C1FlexGrid grd)
        {

            subtotalNo = 0;

            // clear existing totals
            grd.Subtotal(AggregateEnum.Clear);

            grd.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grd.Cols["MILL_PCS"].Index, "합계");
            grd.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grd.Cols["MILL_WGT"].Index, "합계");
            grd.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grd.Cols["STR_PCS"].Index, "합계");
            //grd.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grd.Cols["STR_WGT"].Index, "합계");
            grd.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grd.Cols["SHF_PCS"].Index, "합계");
            //grd.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grd.Cols["SHF_WGT"].Index, "합계");
            

            //sql1 += string.Format("       ,MILL_PCS ");
            //sql1 += string.Format("       ,MILL_WGT ");
            //sql1 += string.Format("       ,STR_PCS    "); //--교정 본수
            //sql1 += string.Format("       ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,STR_PCS),0)  AS STR_WGT "); //--교정중량
            //sql1 += string.Format("       ,SHF_PCS "); //--면취 본수
            //sql1 += string.Format("       ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,SHF_PCS),0)  AS SHF_WGT "); //--면취중량
            AddSubtotalNo();
            grd.Rows.Frozen = GetAvailMinRow(grd) -1;
            //grdMain.Subtotal(AggregateEnum.Average, 1, -1, grdMain.Cols["THEORY_WGT"].Index, "평균");

            //grdMain.AutoSizeCols();

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
        #region 닫기
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "엑셀파일 생성"
        private void btnExcel_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = "xlsx";

            C1.Win.C1FlexGrid.C1FlexGrid grid = new C1.Win.C1FlexGrid.C1FlexGrid();

            if (current_Tab == "TabP1")
            {
                grid = grdMain1;
                dlg.FileName = "면취_실적_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP2" && (cboLine_GP.Text == "#2라인" || cboLine_GP.Text == "#3라인"))
            {
                grid = grdMain2_2;
                dlg.FileName = "면취_조업_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }

            if (current_Tab == "TabP2" && cboLine_GP.Text == "#1라인")
            {
                grid = grdMain2_1;
                dlg.FileName = "면취_조업_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                grid.SaveGrid(dlg.FileName, C1.Win.C1FlexGrid.FileFormatEnum.Excel, C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells);

                if (MessageBox.Show("저장한 Excel File을 여시겠습니까?", "Excel File Open", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("excel.exe", "\"" + dlg.FileName + "\"");
                }
            }
        }
        #endregion

        #region 이벤트
        private void TabOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabOpt.SelectedIndex == 0) current_Tab = "TabP1";
            if (TabOpt.SelectedIndex == 1) current_Tab = "TabP2";

            SetupGridSelect();

            btnDisplay_Click(null, null);
            //SetDataBinding();
        }

        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {


            cd_id = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;

            ck.Line_gp = cd_id;

            //sql = "";

            //if (olddt != null) olddt.Clear();

            SetupGridSelect();
        }

        private void SetupGridSelect()
        {
            if (current_Tab == "TabP1") selectedGrid = grdMain1;
            if (current_Tab == "TabP2" && (cboLine_GP.Text == "#2라인" || cboLine_GP.Text == "#3라인"))
            {
                selectedGrid = grdMain2_2;
                selectedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
                selectedGrid.BringToFront();
            }
            if (current_Tab == "TabP2" && cboLine_GP.Text == "#1라인")
            {
                selectedGrid = grdMain2_1;
                selectedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
                selectedGrid.BringToFront();
            }
        }

        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id2 = ((ComLib.DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }

        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id3 = ((ComLib.DictionaryList)cboTEAM.SelectedItem).fnValue;
        }



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

        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

        #endregion

        #region "사용자정의 이벤트"
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
        }

        //강종ID(LostFocus)
        private void gangjong_id_tb_LostFocus(object sender, EventArgs e)
        {
            if (gangjong_id_tb.Text == "") gangjong_Nm_tb.Text = "";
            else
            {
                gangjong_Nm_tb.Text = cd.Find_Steel_NM_By_ID(gangjong_id_tb.Text);

                if (gangjong_Nm_tb.Text.Length == 0)
                {
                    if (MessageBox.Show(" 해당 강종에 따른 강종명을 찾을 수 없습니다.", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        gangjong_Nm_tb.Text = "";
                        gangjong_id_tb.Focus();
                        return;
                    }
                }
            }
        }
        #endregion

        #region "팝업화면-강종 찾기"
        private void btnSteel_Click(object sender, EventArgs e)
        {
            SearchSteelNm popup = new SearchSteelNm();
            popup.Owner = this;      //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }
        #endregion



        #region 쿼리 설정
        private string QuerySetup()
        {
            string sql1 = "";

            string start_date = start_dt.Value.ToString("yyyyMMdd");
            string end_date = end_dt.Value.ToString("yyyyMMdd");

            //생산실적---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP1")
            {
                sql1 = string.Format("SELECT   ROWNUM AS L_NO ");
                sql1 += string.Format("       ,MFG_DATE ");
                sql1 += string.Format("       ,WORK_TYPE ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                sql1 += string.Format("       ,WORK_TEAM ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                sql1 += string.Format("       ,POC_NO ");
                sql1 += string.Format("       ,MILL_NO ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,STEEL");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,MILL_PCS ");
                sql1 += string.Format("       ,MILL_WGT ");
                sql1 += string.Format("       ,STR_PCS    "); //--교정 본수
                sql1 += string.Format("       ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,STR_PCS),0)  AS STR_WGT "); //--교정중량
                sql1 += string.Format("       ,SHF_PCS "); //--면취 본수
                sql1 += string.Format("       ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,SHF_PCS),0)  AS SHF_WGT "); //--면취중량
                sql1 += string.Format(" FROM   ( ");
                sql1 += string.Format("        SELECT  TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD')        AS MFG_DATE");
                sql1 += string.Format("               ,A.WORK_TYPE      AS WORK_TYPE ");
                sql1 += string.Format("               ,NVL(A.WORK_TEAM,'O')      AS WORK_TEAM ");
                sql1 += string.Format("               ,A.POC_NO         AS POC_NO ");
                sql1 += string.Format("               ,A.MILL_NO        AS MILL_NO ");
                sql1 += string.Format("               ,A.HEAT           AS HEAT ");
                sql1 += string.Format("               ,MAX(A.ITEM)      AS ITEM ");
                sql1 += string.Format("               ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
                sql1 += string.Format("               ,MAX(A.STEEL)     AS STEEL ");
                sql1 += string.Format("               ,TO_CHAR(MAX(A.LENGTH),'90.00')    AS LENGTH "); //TO_CHAR(MAX(A.LENGTH),'9,990.99') 
                sql1 += string.Format("               ,MAX(B.PCS)       AS MILL_PCS ");
                sql1 += string.Format("               ,TO_CHAR(MAX(B.NET_WGT),'9,999,000')   AS MILL_WGT ");
                sql1 += string.Format("               ,SUM(DECODE(A.ROUTING_CD,'A1',1,0)) AS STR_PCS ");
                sql1 += string.Format("               ,SUM(DECODE(A.ROUTING_CD,'B1',1,0)) AS SHF_PCS ");
                sql1 += string.Format("        FROM   TB_CR_PIECE_WR A ");
                sql1 += string.Format("               ,TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("        WHERE  A.MILL_NO  = B.MILL_NO ");
                sql1 += string.Format("        AND    A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date); // BETWEEN '{0}' AND '{1}' ", start_date, end_date); 
                sql1 += string.Format("        AND    A.LINE_GP    =  '{0}' ", cd_id); //:P_LINE_GP
                sql1 += string.Format("        AND    A.ROUTING_CD IN ('A1','B1') ");  //--면취

                if (cd_id2 != "%")
                    sql1 += string.Format("      AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);

                if (txtHeat.Text != "")
                    sql1 += string.Format("      AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);

                if (gangjong_id_tb.Text != "")
                    sql1 += string.Format("      AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);

                if (txtItemSize.Text != "")
                    sql1 += string.Format("      AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);

                if (txtPoc.Text != "")
                    sql1 += string.Format("      AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);

                if (cd_id3 != "%")
                    sql1 += string.Format("          AND NVL(A.WORK_TEAM,'O')  LIKE '%{0}%' ", cd_id3);
                
                sql1 += string.Format("          AND A.REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("          WHERE  MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date); // BETWEEN '{0}' AND '{1}' ", start_date, end_date); 
                sql1 += string.Format("          AND  MILL_NO = A.MILL_NO ");
                sql1 += string.Format("          AND PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("          AND LINE_GP = A.LINE_GP ");
                sql1 += string.Format("          AND ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("          GROUP BY A.MFG_DATE ");
                sql1 += string.Format("                ,A.WORK_TYPE ");
                sql1 += string.Format("                ,A.WORK_TEAM ");
                sql1 += string.Format("                ,A.POC_NO ");
                sql1 += string.Format("                ,A.MILL_NO ");
                sql1 += string.Format("                ,A.HEAT ");
                sql1 += string.Format("          ORDER BY  MFG_DATE DESC, WORK_TYPE, MILL_NO  ");
                sql1 += string.Format("        ) X ");

                //sql1 = string.Format("SELECT   ROWNUM AS L_NO ");
                //sql1 += string.Format("       ,MFG_DATE ");
                //sql1 += string.Format("       ,WORK_TYPE ");
                //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                //sql1 += string.Format("       ,WORK_TEAM ");
                //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                //sql1 += string.Format("       ,POC_NO ");
                //sql1 += string.Format("       ,MILL_NO ");
                //sql1 += string.Format("       ,HEAT ");
                //sql1 += string.Format("       ,STEEL");
                //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                //sql1 += string.Format("       ,ITEM ");
                //sql1 += string.Format("       ,ITEM_SIZE ");
                //sql1 += string.Format("       ,LENGTH ");
                //sql1 += string.Format("       ,MILL_PCS ");
                //sql1 += string.Format("       ,MILL_WGT ");
                //sql1 += string.Format("       ,STR_PCS    "); //--교정 본수
                //sql1 += string.Format("       ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,STR_PCS),0)  AS STR_WGT "); //--교정중량
                //sql1 += string.Format("       ,SHF_PCS "); //--면취 본수
                //sql1 += string.Format("       ,ROUND(FN_GET_WGT(ITEM,ITEM_SIZE,LENGTH,SHF_PCS),0)  AS SHF_WGT "); //--면취중량
                //sql1 += string.Format(" FROM   ( ");
                //sql1 += string.Format("        SELECT  TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD')        AS MFG_DATE");
                //sql1 += string.Format("               ,A.WORK_TYPE      AS WORK_TYPE ");
                //sql1 += string.Format("               ,NVL(A.WORK_TEAM,'A')      AS WORK_TEAM ");
                //sql1 += string.Format("               ,A.POC_NO         AS POC_NO ");
                //sql1 += string.Format("               ,A.MILL_NO        AS MILL_NO ");
                //sql1 += string.Format("               ,A.HEAT           AS HEAT ");
                //sql1 += string.Format("               ,MAX(A.ITEM)      AS ITEM ");
                //sql1 += string.Format("               ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
                //sql1 += string.Format("               ,MAX(A.STEEL)     AS STEEL ");
                //sql1 += string.Format("               ,TO_CHAR(MAX(A.LENGTH),'90.00')    AS LENGTH "); //TO_CHAR(MAX(A.LENGTH),'9,990.99') 
                //sql1 += string.Format("               ,MAX(B.PCS)       AS MILL_PCS ");
                //sql1 += string.Format("               ,TO_CHAR(MAX(B.NET_WGT),'9,999,000')   AS MILL_WGT ");
                //sql1 += string.Format("               ,SUM(DECODE(A.ROUTING_CD,'A1',1,0)) AS STR_PCS ");
                //sql1 += string.Format("               ,SUM(DECODE(A.ROUTING_CD,'B1',1,0)) AS SHF_PCS ");
                //sql1 += string.Format("        FROM   TB_CR_PIECE_WR A ");
                //sql1 += string.Format("               ,TB_CR_ORD_BUNDLEINFO B ");
                //sql1 += string.Format("        WHERE  A.MILL_NO  = B.MILL_NO ");
                //sql1 += string.Format("        AND    A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date); // BETWEEN '{0}' AND '{1}' ", start_date, end_date); 
                //sql1 += string.Format("        AND    A.LINE_GP    =  '{0}' ", cd_id); //:P_LINE_GP
                //sql1 += string.Format("        AND    A.ROUTING_CD IN ('A1','B1') ");  //--면취

                //if (cd_id2 != "")
                //    sql1 += string.Format("      AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);

                //if (txtHeat.Text != "")
                //    sql1 += string.Format("      AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);

                //if (gangjong_id_tb.Text != "")
                //    sql1 += string.Format("      AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);

                //if (txtItemSize.Text != "")
                //    sql1 += string.Format("      AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);

                //if (txtPoc.Text != "")
                //    sql1 += string.Format("      AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
                //sql1 += string.Format("          AND NVL(A.WORK_TEAM,'A')  LIKE '%{0}%' ", cd_id3);
                //sql1 += string.Format("          AND A.REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("          WHERE  MILL_NO = A.MILL_NO ");
                //sql1 += string.Format("          AND PIECE_NO = A.PIECE_NO ");
                //sql1 += string.Format("          AND LINE_GP = A.LINE_GP ");
                //sql1 += string.Format("          AND ROUTING_CD = A.ROUTING_CD ) ");
                //sql1 += string.Format("          GROUP BY A.MFG_DATE ");
                //sql1 += string.Format("                ,A.WORK_TYPE ");
                //sql1 += string.Format("                ,A.WORK_TEAM ");
                //sql1 += string.Format("                ,A.POC_NO ");
                //sql1 += string.Format("                ,A.MILL_NO ");
                //sql1 += string.Format("                ,A.HEAT ");
                //sql1 += string.Format("          ORDER BY  MFG_DATE DESC, WORK_TYPE, MILL_NO  ");
                //sql1 += string.Format("        ) X ");
            }

            //조업정보---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP2")
            {
                sql1 = string.Format("SELECT  ROWNUM AS L_NO ");
                sql1 += string.Format("     , X.* ");
                sql1 += string.Format(" FROM( ");
                sql1 += string.Format("SELECT ");
                sql1 += string.Format("         TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD')        AS WORK_DATE");
                sql1 += string.Format("       , TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 9, 6), 'HH24MISS'), 'HH24:MI:SS') AS WORK_TIME  "); //작업시각 
                sql1 += string.Format("       , B.POC_NO "); //POC
                sql1 += string.Format("       , A.MILL_NO "); //압연번들번호
                sql1 += string.Format("       , B.HEAT "); //HEAT
                sql1 += string.Format("       , B.STEEL "); //강종
                sql1 += string.Format("       , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM "); //강종명
                sql1 += string.Format("       , B.ITEM "); //품목
                sql1 += string.Format("       , B.ITEM_SIZE "); //규격
                sql1 += string.Format("        ,TO_CHAR(B.LENGTH,'90.00') AS LENGTH "); //길이
                sql1 += string.Format("       , TO_CHAR(KICKER_SIZE_ACTV_1,'9,999,990.9') AS COL_01 "); //KICKER SIZE조정 현재값 1 KICKER_SIZE_ACTV_1
                sql1 += string.Format("       , TO_CHAR(KICKER_SIZE_SETV_1,'9,999,990.9') AS COL_02 "); //KICKER SIZE조정 설정값 1 KICKER_SIZE_SETV_1
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_ACTV_1,'9,999,990.9') AS COL_03 "); //ROLLER C/ V드라이브 HZ 현재값 1 ROLLER_CV_HZ_ACTV_1 //윤영
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_CURRENT_ACTV_1,'9,999,990.9') AS COL_04 "); //ROLLER C/ V드라이브모터 전류 현재값 1 ROLLER_CV_CURRENT_ACTV_1
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_H_SETV_1,'9,999,990.9') AS COL_05 "); //ROLLER C/ V드라이브 HZ HIGH 설정값 1 ROLLER_CV_HZ_H_SETV_1
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_S_T_SETV_1,'9,999,990.9') AS COL_06 "); //ROLLER C/ V드라이브 HZ SLOW TIME 설정값 1 ROLLER_CV_HZ_S_T_SETV_1
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_S_SETV_1,'9,999,990.9') AS COL_07 "); //ROLLER C/ V드라이브 HZ SLOW 설정값 1 ROLLER_CV_HZ_S_SETV_1
                sql1 += string.Format("       , TO_CHAR(SCREW_FEED_HZ_ACTV_1,'9,999,990.9') AS COL_08 "); //SCREW 피딩드라이브 HZ 현재값 1 SCREW_FEED_HZ_ACTV_1
                sql1 += string.Format("       , TO_CHAR(SCREW_FEED_CURRENT_ACTV_1,'9,999,990.9') AS COL_09 "); //SCREW 피딩모터 전류 현재값 1 SCREW_FEED_CURRENT_ACTV_1
                sql1 += string.Format("       , TO_CHAR(SCREW_FEED_HZ_SETV_1,'9,999,990.9') AS COL_10 "); //SCREW 피딩드라이브 HZ 설정값 1 SCREW_FEED_HZ_SETV_1
                sql1 += string.Format("       , TO_CHAR(GRIND_HZ_ACTV_1,'9,999,990.9') AS COL_11 "); //GRIND드라이브 HZ 현재값 1 GRIND_HZ_ACTV_1
                sql1 += string.Format("       , TO_CHAR(GRIND_CURRENT_ACTV_1,'9,999,990.9') AS COL_12 "); //GRIND 모터전류 현재값 1 GRIND_CURRENT_ACTV_1
                sql1 += string.Format("       , TO_CHAR(GRIND_HZ_SETV_1,'9,999,990.9') AS COL_13 "); //GRIND 드라이브HZ 설정값 1 GRIND_HZ_SETV_1
                sql1 += string.Format("       , TO_CHAR(STOPPER_ACTV_1,'9,999,990.9') AS COL_14 "); //STOPPER 현재값 1 STOPPER_ACTV_1
                sql1 += string.Format("       , TO_CHAR(STOPPER_SETV_1,'9,999,990.9') AS COL_15 "); //STOPPER 설정값 1 STOPPER_SETV_1
                sql1 += string.Format("       , TO_CHAR(GUIDE_ACTV_1,'9,999,990.9') AS COL_16 "); //GUIDE 현재값 1 GUIDE_ACTV_1
                sql1 += string.Format("       , TO_CHAR(GUIDE_SETV_1,'9,999,990.9') AS COL_17 "); //GUIDE 설정값 1 GUIDE_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_ACTV_1,'9,999,990.9') AS COL_18 "); //SERVO 현재값 1 SERVO_ACTV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_JOG_UP_SP_SETV_1,'9,999,990.9') AS COL_19 "); //SERVO JOG UP 속도 설정값 1 SERVO_JOG_UP_SP_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_JOG_DOWN_SP_SETV_1,'9,999,990.9') AS COL_20 "); //SERVO JOG DOWN 속도 설정값 1 SERVO_JOG_DOWN_SP_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_UP_SP_SETV_1,'9,999,990.9') AS COL_21 "); //SERVO UP 속도 설정값 1 SERVO_UP_SP_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_UP_POS_SETV_1,'9,999,990.9') AS COL_22 "); //SERVO UP 위치 설정값 1 SERVO_UP_POS_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_CH_POS_SETV_1,'9,999,990.9') AS COL_23 "); //SERVO 툴변경위치 설정값 1 SERVO_CH_POS_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_SP_SETV_1,'9,999,990.9') AS COL_24 "); //SERVO 툴속도 설정값 1 SERVO_SP_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_SETV_1,'9,999,990.9') AS COL_25 "); //SERVO 툴위치 설정값 1 SERVO_POS_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_SP_SETV_1,'9,999,990.9')  AS COL_26 "); //SERVO 툴위치 인칭속도 설정값 1 SERVO_POS_IN_SP_SETV_1
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_SETV_1,'9,999,990.9') AS COL_27 "); //SERVO 툴위치 인칭위치 설정값 1 SERVO_POS_IN_POS_SETV_1
                sql1 += string.Format("       , TO_CHAR(KICKER_SIZE_ACTV_2,'9,999,990.9') AS COL_28 "); //KICKER SIZE조정 현재값 2 KICKER_SIZE_ACTV_2
                sql1 += string.Format("       , TO_CHAR(KICKER_SIZE_SETV_2,'9,999,990.9') AS COL_29 "); //KICKER SIZE조정 설정값 2 KICKER_SIZE_SETV_2
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_ACTV_2,'9,999,990.9') AS COL_30 "); //ROLLER C/ V드라이브 HZ 현재값 2 ROLLER_CV_HZ_ACTV_2
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_CURRENT_ACTV_2,'9,999,990.9') AS COL_31 "); //ROLLER C/ V드라이브모터 전류 현재값 2 ROLLER_CV_CURRENT_ACTV_2
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_H_SETV_2,'9,999,990.9') AS COL_32 "); //ROLLER C/ V드라이브 HZ HIGH 설정값 2 ROLLER_CV_HZ_H_SETV_2
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_S_T_SETV_2,'9,999,990.9') AS COL_33 "); //ROLLER C/ V드라이브 HZ SLOW TIME 설정값 2 ROLLER_CV_HZ_S_T_SETV_2
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_S_SETV_2,'9,999,990.9') AS COL_34 "); //ROLLER C/ V드라이브 HZ SLOW 설정값 2 ROLLER_CV_HZ_S_SETV_2
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_ACTV_1,'9,999,990.9') AS COL_35 "); //SCREW 피딩드라이브 HZ 현재값 2 SCREW_FEED_HZ_ACTV_2
                sql1 += string.Format("       , TO_CHAR(SCREW_FEED_HZ_ACTV_2,'9,999,990.9') AS COL_36 "); //SCREW 피딩모터 전류 현재값 2 SCREW_FEED_CURRENT_ACTV_2
                sql1 += string.Format("       , TO_CHAR(SCREW_FEED_HZ_SETV_2,'9,999,990.9') AS COL_37 "); //SCREW 피딩드라이브 HZ 설정값 2 SCREW_FEED_HZ_SETV_2
                sql1 += string.Format("       , TO_CHAR(GRIND_HZ_ACTV_2,'9,999,990.9') AS COL_38 "); //GRIND드라이브 HZ 현재값 2 GRIND_HZ_ACTV_2
                sql1 += string.Format("       , TO_CHAR(GRIND_CURRENT_ACTV_2,'9,999,990.9') AS COL_39 "); //GRIND 모터전류 현재값 2 GRIND_CURRENT_ACTV_2
                sql1 += string.Format("       , TO_CHAR(GRIND_HZ_SETV_2,'9,999,990.9') AS COL_40 "); //GRIND 드라이브HZ 설정값 2 GRIND_HZ_SETV_2
                sql1 += string.Format("       , TO_CHAR(STOPPER_ACTV_2,'9,999,990.9') AS COL_41 "); //STOPPER 현재값 2 STOPPER_ACTV_2
                sql1 += string.Format("       , TO_CHAR(STOPPER_SETV_2,'9,999,990.9') AS COL_42 "); //STOPPER 설정값 2 STOPPER_SETV_2
                sql1 += string.Format("       , TO_CHAR(GUIDE_ACTV_2,'9,999,990.9') AS COL_43 "); //GUIDE 현재값 2 GUIDE_ACTV_2
                sql1 += string.Format("       , TO_CHAR(GUIDE_SETV_2,'9,999,990.9') AS COL_44 "); //GUIDE 설정값 2 GUIDE_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_ACTV_2,'9,999,990.9') AS COL_45 "); //SERVO 현재값 2 SERVO_ACTV_2
                sql1 += string.Format("       , TO_CHAR(ROLLER_CV_HZ_ACTV_1,'9,999,990.9') AS COL_46 "); //SERVO JOG UP 속도 설정값 2 SERVO_JOG_UP_SP_SETV_2
                sql1 += string.Format("       ,TO_CHAR(SERVO_JOG_UP_SP_SETV_2,'9,999,990.9') AS COL_47 "); //SERVO JOG DOWN 속도 설정값 2 SERVO_JOG_DOWN_SP_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_UP_SP_SETV_2,'9,999,990.9') AS COL_48 "); //SERVO UP 속도 설정값 2 SERVO_UP_SP_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_UP_POS_SETV_2,'9,999,990.9') AS COL_49 "); //SERVO UP 위치 설정값 2 SERVO_UP_POS_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_CH_POS_SETV_2,'9,999,990.9') AS COL_50 "); //SERVO 툴변경위치 설정값 2 SERVO_CH_POS_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_SP_SETV_2,'9,999,990.9') AS COL_51 "); //SERVO 툴속도 설정값 2 SERVO_SP_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_SETV_2,'9,999,990.9') AS COL_52 "); //SERVO 툴위치 설정값 2 SERVO_POS_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_SP_SETV_2,'9,999,990.9') AS COL_53 "); //SERVO 툴위치 인칭속도 설정값 2 SERVO_POS_IN_SP_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_SETV_2,'9,999,990.9') AS COL_54 "); //SERVO 툴위치 인칭위치 설정값 2 SERVO_POS_IN_POS_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_UP_CNT_2,'9,999,990.9') AS COL_55 "); //SERVO 툴위치 인칭 상승카운터2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_DW_CNT_2,'9,999,990.9')     AS COL_56 "); //SERVO 툴위치 인칭 하강카운터2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_UP_CNT_1,'9,999,990.9') AS COL_57 "); //SERVO 툴위치 인칭 상승카운터1
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_DW_CNT_1,'9,999,990.9')     AS COL_58 "); //SERVO 툴위치 인칭 하강카운터1
                sql1 += string.Format("  FROM   TB_CHF_OPERINFO_NO3 A ");
                sql1 += string.Format("       , TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("  WHERE A.MILL_NO = B.MILL_NO ");
                if (cd_id != "")
                    sql1 += string.Format("     AND    '{0}' = '#3' ", cd_id);

                sql1 += string.Format("  AND A.WORK_DDTT BETWEEN '{0}' AND '{1}' ", start_date + "000000", end_date + "235959");

                if (cd_id2 != "")
                    sql1 += string.Format("      AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);

                if (txtHeat.Text != "")
                    sql1 += string.Format("      AND B.HEAT      LIKE '%{0}%' ", txtHeat.Text);

                if (gangjong_id_tb.Text != "")
                    sql1 += string.Format("      AND B.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);

                if (txtItemSize.Text != "")
                    sql1 += string.Format("      AND B.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);

                if (txtPoc.Text != "")
                    sql1 += string.Format("      AND B.POC_NO      LIKE '%{0}%' ", txtPoc.Text);

                sql1 += string.Format("  UNION ");
                sql1 += string.Format("  SELECT ");
                sql1 += string.Format("          TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD')        AS WORK_DATE");
                sql1 += string.Format("       , TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 9, 6), 'HH24MISS'), 'HH24:MI:SS') AS WORK_TIME  "); //작업시각 
                sql1 += string.Format("        , B.POC_NO "); //POC
                sql1 += string.Format("        , A.MILL_NO "); //압연번들번호
                sql1 += string.Format("        , B.HEAT "); //HEAT
                sql1 += string.Format("        , B.STEEL "); //강종
                sql1 += string.Format("        , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM "); //강종명
                sql1 += string.Format("        , B.ITEM "); //품목
                sql1 += string.Format("        , B.ITEM_SIZE "); //규격
                sql1 += string.Format("        , TO_CHAR(B.LENGTH,'90.00') AS LENGTH "); //길이  
                sql1 += string.Format("        , TO_CHAR(KICKER_SIZE_ACTV_1,'9,999,990.9')  AS COL_01 "); //KICKER SIZE조정 현재값 1 KICKER_SIZE_ACTV_1
                sql1 += string.Format("        , TO_CHAR(KICKER_SIZE_SETV_1,'9,999,990.9')  AS COL_02 "); //KICKER SIZE조정 설정값 1 KICKER_SIZE_SETV_1
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_ACTV_1,'9,999,990.9')  AS COL_03 "); //ROLLER C/ V드라이브 HZ 현재값 1 ROLLER_CV_HZ_ACTV_1
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_CURRENT_ACTV_1,'9,999,990.9')  AS COL_04 "); //ROLLER C/ V드라이브모터 전류 현재값 1 ROLLER_CV_CURRENT_ACTV_1
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_H_SETV_1,'9,999,990.9')  AS COL_05 "); //ROLLER C/ V드라이브 HZ HIGH 설정값 1 ROLLER_CV_HZ_H_SETV_1
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_S_T_SETV_1,'9,999,990.9')  AS COL_06 "); //ROLLER C/ V드라이브 HZ SLOW TIME 설정값 1 ROLLER_CV_HZ_S_T_SETV_1
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_S_SETV_1,'9,999,990.9')  AS COL_07 "); //ROLLER C/ V드라이브 HZ SLOW 설정값 1 ROLLER_CV_HZ_S_SETV_1
                sql1 += string.Format("        , TO_CHAR(SCREW_FEED_HZ_ACTV_1,'9,999,990.9')  AS COL_08 "); //SCREW 피딩드라이브 HZ 현재값 1 SCREW_FEED_HZ_ACTV_1
                sql1 += string.Format("        , TO_CHAR(SCREW_FEED_CURRENT_ACTV_1,'9,999,990.9')  AS COL_09 "); //SCREW 피딩모터 전류 현재값 1 SCREW_FEED_CURRENT_ACTV_1
                sql1 += string.Format("        , TO_CHAR(SCREW_FEED_HZ_SETV_1,'9,999,990.9')  AS COL_10 "); //SCREW 피딩드라이브 HZ 설정값 1 SCREW_FEED_HZ_SETV_1
                sql1 += string.Format("        , TO_CHAR(GRIND_HZ_ACTV_1,'9,999,990.9')  AS COL_11 "); //GRIND드라이브 HZ 현재값 1 GRIND_HZ_ACTV_1
                sql1 += string.Format("        , TO_CHAR(GRIND_CURRENT_ACTV_1,'9,999,990.9')  AS COL_12 "); //GRIND 모터전류 현재값 1 GRIND_CURRENT_ACTV_1
                sql1 += string.Format("        , TO_CHAR(GRIND_HZ_SETV_1,'9,999,990.9')  AS COL_13 "); //GRIND 드라이브HZ 설정값 1 GRIND_HZ_SETV_1
                sql1 += string.Format("        , TO_CHAR(STOPPER_ACTV_1,'9,999,990.9')  AS COL_14 "); //STOPPER 현재값 1 STOPPER_ACTV_1
                sql1 += string.Format("        , TO_CHAR(STOPPER_SETV_1,'9,999,990.9')  AS COL_15 "); //STOPPER 설정값 1 STOPPER_SETV_1
                sql1 += string.Format("        , TO_CHAR(GUIDE_ACTV_1,'9,999,990.9')  AS COL_16 "); //GUIDE 현재값 1 GUIDE_ACTV_1
                sql1 += string.Format("        , TO_CHAR(GUIDE_SETV_1,'9,999,990.9')  AS COL_17 "); //GUIDE 설정값 1 GUIDE_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_ACTV_1,'9,999,990.9')  AS COL_18 "); //SERVO 현재값 1 SERVO_ACTV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_JOG_UP_SP_SETV_1,'9,999,990.9')  AS COL_19 "); //SERVO JOG UP 속도 설정값 1 SERVO_JOG_UP_SP_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_JOG_DOWN_SP_SETV_1,'9,999,990.9')  AS COL_20 "); //SERVO JOG DOWN 속도 설정값 1 SERVO_JOG_DOWN_SP_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_UP_SP_SETV_1,'9,999,990.9')  AS COL_21 "); //SERVO UP 속도 설정값 1 SERVO_UP_SP_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_UP_POS_SETV_1,'9,999,990.9')  AS COL_22 "); //SERVO UP 위치 설정값 1 SERVO_UP_POS_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_CH_POS_SETV_1,'9,999,990.9')  AS COL_23 "); //SERVO 툴변경위치 설정값 1 SERVO_CH_POS_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_SP_SETV_1,'9,999,990.9')  AS COL_24 "); //SERVO 툴속도 설정값 1 SERVO_SP_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_POS_SETV_1,'9,999,990.9')  AS COL_25 "); //SERVO 툴위치 설정값 1 SERVO_POS_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_POS_IN_SP_SETV_1,'9,999,990.9')  AS COL_26 "); //SERVO 툴위치 인칭속도 설정값 1 SERVO_POS_IN_SP_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_POS_IN_POS_SETV_1,'9,999,990.9')  AS COL_27 "); //SERVO 툴위치 인칭위치 설정값 1 SERVO_POS_IN_POS_SETV_1
                sql1 += string.Format("        , TO_CHAR(KICKER_SIZE_ACTV_2,'9,999,990.9')  AS COL_28 "); //KICKER SIZE조정 현재값 2 KICKER_SIZE_ACTV_2
                sql1 += string.Format("        , TO_CHAR(KICKER_SIZE_SETV_2,'9,999,990.9')  AS COL_29 "); //KICKER SIZE조정 설정값 2 KICKER_SIZE_SETV_2
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_ACTV_2,'9,999,990.9')  AS COL_30 "); //ROLLER C/ V드라이브 HZ 현재값 2 ROLLER_CV_HZ_ACTV_2
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_CURRENT_ACTV_2,'9,999,990.9')  AS COL_31 "); //ROLLER C/ V드라이브모터 전류 현재값 2 ROLLER_CV_CURRENT_ACTV_2
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_H_SETV_2,'9,999,990.9')  AS COL_32 "); //ROLLER C/ V드라이브 HZ HIGH 설정값 2 ROLLER_CV_HZ_H_SETV_2
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_S_T_SETV_2,'9,999,990.9')  AS COL_33 "); //ROLLER C/ V드라이브 HZ SLOW TIME 설정값 2 ROLLER_CV_HZ_S_T_SETV_2
                sql1 += string.Format("        , TO_CHAR(ROLLER_CV_HZ_S_SETV_2,'9,999,990.9')  AS COL_34 "); //ROLLER C/ V드라이브 HZ SLOW 설정값 2 ROLLER_CV_HZ_S_SETV_2
                sql1 += string.Format("        , TO_CHAR(SCREW_FEED_HZ_ACTV_2,'9,999,990.9')  AS COL_35 "); //SCREW 피딩드라이브 HZ 현재값 2 SCREW_FEED_HZ_ACTV_2
                sql1 += string.Format("        , TO_CHAR(SCREW_FEED_CURRENT_ACTV_2,'9,999,990.9')  AS COL_36 "); //SCREW 피딩모터 전류 현재값 2 SCREW_FEED_CURRENT_ACTV_2
                sql1 += string.Format("        , TO_CHAR(SCREW_FEED_HZ_SETV_2,'9,999,990.9')  AS COL_37 "); //SCREW 피딩드라이브 HZ 설정값 2 SCREW_FEED_HZ_SETV_2
                sql1 += string.Format("        , TO_CHAR(GRIND_HZ_ACTV_2,'9,999,990.9')  AS COL_38 "); //GRIND드라이브 HZ 현재값 2 GRIND_HZ_ACTV_2
                sql1 += string.Format("        , TO_CHAR(GRIND_CURRENT_ACTV_2,'9,999,990.9')  AS COL_39 "); //GRIND 모터전류 현재값 2 GRIND_CURRENT_ACTV_2
                sql1 += string.Format("        , TO_CHAR(GRIND_HZ_SETV_2,'9,999,990.9')  AS COL_40 "); //GRIND 드라이브HZ 설정값 2 GRIND_HZ_SETV_2
                sql1 += string.Format("        , TO_CHAR(STOPPER_ACTV_2,'9,999,990.9')  AS COL_41 "); //STOPPER 현재값 2 STOPPER_ACTV_2
                sql1 += string.Format("        , TO_CHAR(STOPPER_SETV_2,'9,999,990.9')  AS COL_42 "); //STOPPER 설정값 2 STOPPER_SETV_2
                sql1 += string.Format("        , TO_CHAR(GUIDE_ACTV_2,'9,999,990.9')  AS COL_43 "); //GUIDE 현재값 2 GUIDE_ACTV_2
                sql1 += string.Format("        , TO_CHAR(GUIDE_SETV_2,'9,999,990.9')  AS COL_44 "); //GUIDE 설정값 2 GUIDE_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_ACTV_2,'9,999,990.9')  AS COL_45 "); //SERVO 현재값 2 SERVO_ACTV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_JOG_UP_SP_SETV_2,'9,999,990.9')  AS COL_46 "); //SERVO JOG UP 속도 설정값 2 SERVO_JOG_UP_SP_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_JOG_DOWN_SP_SETV_2,'9,999,990.9')  AS COL_47 "); //SERVO JOG DOWN 속도 설정값 2 SERVO_JOG_DOWN_SP_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_UP_SP_SETV_2,'9,999,990.9')  AS COL_48 "); //SERVO UP 속도 설정값 2 SERVO_UP_SP_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_UP_POS_SETV_2,'9,999,990.9')  AS COL_49 "); //SERVO UP 위치 설정값 2 SERVO_UP_POS_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_CH_POS_SETV_2,'9,999,990.9')  AS COL_50 "); //SERVO 툴변경위치 설정값 2 SERVO_CH_POS_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_SP_SETV_2,'9,999,990.9')  AS COL_51 "); //SERVO 툴속도 설정값 2 SERVO_SP_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_POS_SETV_2,'9,999,990.9')  AS COL_52 "); //SERVO 툴위치 설정값 2 SERVO_POS_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_POS_IN_SP_SETV_2,'9,999,990.9')  AS COL_53 "); //SERVO 툴위치 인칭속도 설정값 2 SERVO_POS_IN_SP_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_POS_IN_POS_SETV_2,'9,999,990.9')  AS COL_54 "); //SERVO 툴위치 인칭위치 설정값 2 SERVO_POS_IN_POS_SETV_2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_UP_CNT_2,'9,999,990.9') AS COL_55 "); //SERVO 툴위치 인칭 상승카운터2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_DW_CNT_2,'9,999,990.9')     AS COL_56 "); //SERVO 툴위치 인칭 하강카운터2
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_UP_CNT_1,'9,999,990.9') AS COL_57 "); //SERVO 툴위치 인칭 상승카운터1
                sql1 += string.Format("       , TO_CHAR(SERVO_POS_IN_POS_DW_CNT_1,'9,999,990.9')     AS COL_58 "); //SERVO 툴위치 인칭 하강카운터1
                sql1 += string.Format("   FROM   TB_CHF_OPERINFO_NO2 A ");
                sql1 += string.Format("        , TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("  WHERE A.MILL_NO = B.MILL_NO ");
                if (cd_id != "")
                    sql1 += string.Format("     AND    '{0}' = '#2' ", cd_id);

                sql1 += string.Format("  AND A.WORK_DDTT BETWEEN '{0}' AND '{1}' ", start_date + "000000", end_date + "235959");

                if (cd_id2 != "")
                    sql1 += string.Format("      AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);

                if (txtHeat.Text != "")
                    sql1 += string.Format("      AND B.HEAT      LIKE '%{0}%' ", txtHeat.Text);

                if (gangjong_id_tb.Text != "")
                    sql1 += string.Format("      AND B.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);

                if (txtItemSize.Text != "")
                    sql1 += string.Format("      AND B.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);

                if (txtPoc.Text != "")
                    sql1 += string.Format("      AND B.POC_NO      LIKE '%{0}%' ", txtPoc.Text);

                sql1 += string.Format("  UNION ");
                sql1 += string.Format("  SELECT ");
                sql1 += string.Format("          TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 1, 8),'YYYYMMDD'),'YYYY-MM-DD')        AS WORK_DATE");
                sql1 += string.Format("       , TO_CHAR(TO_DATE(SUBSTR(WORK_DDTT, 9, 6), 'HH24MISS'), 'HH24:MI:SS') AS WORK_TIME  "); //작업시각 
                sql1 += string.Format("        , B.POC_NO "); //POC
                sql1 += string.Format("        , A.MILL_NO "); //압연번들번호
                sql1 += string.Format("        , B.HEAT "); //HEAT
                sql1 += string.Format("        , B.STEEL "); //강종
                sql1 += string.Format("        , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM "); //강종명
                sql1 += string.Format("        , B.ITEM "); //품목
                sql1 += string.Format("        , B.ITEM_SIZE "); //규격
                sql1 += string.Format("        , TO_CHAR(B.LENGTH,'90.00') AS LENGTH "); //길이
                sql1 += string.Format("        , TO_CHAR(SERVO_ACTV_1,'9,999,990.9')  AS COL_01 "); //SERVO 현재값 1(mm) SERVO_ACTV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_SETV_1,'9,999,990.9')  AS COL_02 "); //SERVO 설정값 1(mm) SERVO_SETV_1
                sql1 += string.Format("        , TO_CHAR(SERVO_SETV_2,'9,999,990.9')  AS COL_03 "); //SERVO 설정값 2(mm) SERVO_SETV_2
                sql1 += string.Format("        , TO_CHAR(SERVO_ACTV_2,'9,999,990.9')  AS COL_04 "); //SERVO 현재값 2(mm) SERVO_ACTV_2
                sql1 += string.Format("        , TO_CHAR(SCREW_ACTV_1,'9,999,990.9')  AS COL_05 "); //SCREW 롤드라이브 현재값 1(Hz) SCREW_ACTV_1
                sql1 += string.Format("        , TO_CHAR(SCREW_SETV_1,'9,999,990.9')  AS COL_06 "); //SCREW 롤드라이브 설정값 1(Hz) SCREW_SETV_1
                sql1 += string.Format("        , TO_CHAR(SCREW_ACTV_2,'9,999,990.9')  AS COL_07 "); //SCREW 롤드라이브 현재값 2(Hz) SCREW_ACTV_2
                sql1 += string.Format("        , TO_CHAR(SCREW_SETV_2,'9,999,990.9')  AS COL_08 "); //SCREW 롤드라이브 설정값 2(Hz) SCREW_SETV_2
                sql1 += string.Format("        , TO_CHAR(RT_ACTV_1,'9,999,990.9')  AS COL_09 "); //롤드라이브 현재값 1(Hz) RT_ACTV_1
                sql1 += string.Format("        , TO_CHAR(RT_SETV_1,'9,999,990.9')  AS COL_10 "); //롤드라이브 설정값 1(Hz) RT_SETV_1
                sql1 += string.Format("        , TO_CHAR(RT_ACTV_2,'9,999,990.9')  AS COL_11 "); //롤드라이브 현재값 2(Hz) RT_ACTV_2
                sql1 += string.Format("        , TO_CHAR(RT_SETV_2,'9,999,990.9')  AS COL_12 "); //롤드라이브 설정값 1(Hz) RT_SETV_2
                sql1 += string.Format("        , TO_CHAR(RT_ACTV_EXIT,'9,999,990.9')  AS COL_13 "); //롤드라이브 현재값 Exit(Hz) RT_ACTV_EXIT
                sql1 += string.Format("        , TO_CHAR(RT_SETV_EXIT,'9,999,990.9')  AS COL_14 "); //롤드라이브 설정값 Exit(Hz) RT_SETV_EXIT
                sql1 += string.Format("        , TO_CHAR(STOPPER_1,'9,999,990.9')  AS COL_15 "); //STOPPER1 STOPPER_1
                sql1 += string.Format("        , TO_CHAR(PROP_1,'9,999,990.9')  AS COL_16 "); //받침대1 PROP_1
                sql1 += string.Format("        , TO_CHAR(MATERIAL_GAP_1,'9,999,990.9')  AS COL_17 "); //소재 간격1 MATERIAL_GAP_1
                sql1 += string.Format("        , TO_CHAR(STOPPER_2,'9,999,990.9')       AS COL_18 "); //STOPPER1 STOPPER_2
                sql1 += string.Format("        , TO_CHAR(PROP_2,'9,999,990.9')  AS COL_19 "); //받침대2 PROP_2
                sql1 += string.Format("        , TO_CHAR(MATERIAL_GAP_2,'9,999,990.9')  AS COL_20 "); //소재 간격2 MATERIAL_GAP_2
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_21 "); //NULL
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_22 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_23 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_24 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_25 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_26 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_27 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_28 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_29 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_30 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_31 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_32 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_33 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_34 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_35 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_36 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_37 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_38 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_39 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_40 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_41 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_42 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_43 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_44 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_45 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_46 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_47 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_48 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_49 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_50 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_51 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_52 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_53 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_54 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_55 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_56 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_57 ");
                sql1 += string.Format("        , TO_CHAR(NULL,'9,999,990.9') AS COL_58 ");
                sql1 += string.Format("   FROM   TB_CHF_OPERINFO_NO1  A ");
                sql1 += string.Format("        , TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("   WHERE A.MILL_NO = B.MILL_NO ");
                if (cd_id != "")
                    sql1 += string.Format("     AND    '{0}' = '#1' ", cd_id);

                sql1 += string.Format("   AND A.WORK_DDTT BETWEEN '{0}' AND '{1}' ", start_date + "000000", end_date + "235959");

                if (cd_id2 != "")
                    sql1 += string.Format("      AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);

                if (txtHeat.Text != "")
                    sql1 += string.Format("      AND B.HEAT      LIKE '%{0}%' ", txtHeat.Text);

                if (gangjong_id_tb.Text != "")
                    sql1 += string.Format("      AND B.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);

                if (txtItemSize.Text != "")
                    sql1 += string.Format("      AND B.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);

                if (txtPoc.Text != "")
                    sql1 += string.Format("      AND B.POC_NO      LIKE '%{0}%' ", txtPoc.Text);

                sql1 += string.Format("   ORDER BY 1 DESC ,2 DESC");
                sql1 += string.Format("   ) X ");
            }

            return sql1;
        }
        #endregion
    }
}
