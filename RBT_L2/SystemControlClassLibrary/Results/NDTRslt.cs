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
    public partial class NDTRslt : Form
    {
        #region 변수선언
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        private static string cd_id = "";
        private static string cd_id2 = "";
        private static string cd_id3 = ""; //조 ID
        private string current_Tab = "";

        private static string ownerNM = "";
        private static string titleNM = "";

        DataTable olddt_Main1;
        DataTable olddt_Main2;
        DataTable moddt_Main1;
        DataTable moddt_Main2;
        bool _CanSaveSearchLog = false;
        private int subtotalNo;


        #endregion

        #region 화면
        public NDTRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;
            InitializeComponent();
        }

        private void NDTRslt_Load(object sender, System.EventArgs e)
        {
            InitControl();

            SetComboBox();

            TabOpt.SelectedIndex = 0;

            EventCreate();      //사용자정의 이벤트
            _CanSaveSearchLog = true;
            btnDisplay_Click(null, null);
        }

        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);

            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitPanel(panel1);

            // InitLabel
            clsStyle.Style.InitLabel(label1);
            clsStyle.Style.InitLabel(label2);
            clsStyle.Style.InitLabel(label3);
            clsStyle.Style.InitLabel(label4);
            clsStyle.Style.InitLabel(label5);
            clsStyle.Style.InitLabel(label6);
            clsStyle.Style.InitLabel(label7);
            clsStyle.Style.InitLabel(label8);
            clsStyle.Style.InitLabel(lblTEAM);

            //InitCombo
            clsStyle.Style.InitCombo(cboLine_GP, StringAlignment.Near);
            clsStyle.Style.InitCombo(cbo_Work_Type, StringAlignment.Near);
            clsStyle.Style.InitCombo(cboTEAM, StringAlignment.Near);

            //InitTextBox
            clsStyle.Style.InitTextBox(txtPOC);
            clsStyle.Style.InitTextBox(txtHEAT);
            clsStyle.Style.InitTextBox(txtItemSize);
            clsStyle.Style.InitTextBox(gangjong_id_tb);
            clsStyle.Style.InitTextBox(gangjong_Nm_tb);

            //InitDateEdit
            clsStyle.Style.InitDateEdit(start_dt);
            clsStyle.Style.InitDateEdit(end_dt);
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            // Button Color Set
            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnClose);

            //InitTab
            clsStyle.Style.InitTab(TabOpt);

            InitGrid();
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

        private void InitGrid()
        {
            InitgrdMain2();
            InitgrdMain1();
        }

        private void InitgrdMain2()
        {
            C1FlexGrid grid = grdMain2 as C1FlexGrid;

            clsStyle.Style.InitGrid_search(grid);

            grid.AllowEditing = false;

            #region Caption
            grid.Cols[0].Caption = "NO";
            grid.Cols[1].Caption = "작업일자";
            grid.Cols[2].Caption = "작업시각";
            grid.Cols[3].Caption = "POC";
            grid.Cols[4].Caption = "압연번들번호";
            grid.Cols[5].Caption = "HEAT";
            grid.Cols[6].Caption = "강종";
            grid.Cols[7].Caption = "강종명";
            grid.Cols[8].Caption = "품목";
            grid.Cols[9].Caption = "규격";
            grid.Cols[10].Caption = "길이(m)";
            grid.Cols[11].Caption = "검사본수";
            grid.Cols[12].Caption = "OK본수";
            grid.Cols[13].Caption = "NG본수";
            grid.Cols[14].Caption = "MAT NG본수";
            grid.Cols[15].Caption = "MLFT NG본수";
            grid.Cols[16].Caption = "UT NG본수";
            grid.Cols[17].Caption = "MAT주파수";
            grid.Cols[18].Caption = "MAT CurrentMax";
            grid.Cols[19].Caption = "MAT Attenuation";
            grid.Cols[20].Caption = "MAT Sensitivity";
            grid.Cols[21].Caption = "MAT Harmonic";
            grid.Cols[22].Caption = "MLFT회전수";
            grid.Cols[23].Caption = "MLFT전단불감대";
            grid.Cols[24].Caption = "MLFT후단불감대";
            grid.Cols[25].Caption = "MLFT NG레벨F1";
            grid.Cols[26].Caption = "MLFT NG레벨F2";
            grid.Cols[27].Caption = "MLFT NG레벨F3";
            grid.Cols[28].Caption = "MLFT NG판정기준";
            grid.Cols[29].Caption = "MLFT감도보정";
            grid.Cols[30].Caption = "MLFT YOKE주파수";
            grid.Cols[31].Caption = "MLFT탐상감도(Gain)";
            grid.Cols[32].Caption = "MLFT위상(Phase)";
            grid.Cols[33].Caption = "MLFT Y GAIN";
            grid.Cols[34].Caption = "MLFT FILTER MODE";
            grid.Cols[35].Caption = "MLFT LP FILTER";
            grid.Cols[36].Caption = "MLFT HP FILTER";
            grid.Cols[37].Caption = "MLFT FILTER CORR";
            grid.Cols[38].Caption = "MLFT SFS Length F1";
            grid.Cols[39].Caption = "MLFT SFS Length F2";
            grid.Cols[40].Caption = "MLFT SFS Length F3";
            grid.Cols[41].Caption = "MLFT Gain Correction1";
            grid.Cols[42].Caption = "MLFT Gain Correction2";
            grid.Cols[43].Caption = "MLFT Gain Correction3";
            grid.Cols[44].Caption = "MLFT Gain Correction4";
            grid.Cols[45].Caption = "MLFT Gain Correction5";
            grid.Cols[46].Caption = "MLFT Gain Correction6";
            grid.Cols[47].Caption = "MLFT Gain Correction7";
            grid.Cols[48].Caption = "MLFT Gain Correction8";
            grid.Cols[49].Caption = "MLFT Gain Correction9";
            grid.Cols[50].Caption = "MLFT Gain Correction10";
            grid.Cols[51].Caption = "MLFT Gain Correction11";
            grid.Cols[52].Caption = "MLFT Gain Correction12";
            grid.Cols[53].Caption = "UT Velocity L";
            grid.Cols[54].Caption = "UT Velocity D";
            grid.Cols[55].Caption = "UT PRF";
            grid.Cols[56].Caption = "UT Probe주파수";
            grid.Cols[57].Caption = "UT Probe Radius";
            grid.Cols[58].Caption = "UT Probe Pitch";
            grid.Cols[59].Caption = "UT수직Angle";
            grid.Cols[60].Caption = "UT사각Angle";
            grid.Cols[61].Caption = "UT탐상감도Gain(LCW1)";
            grid.Cols[62].Caption = "UT탐상감도Gain(LCW2)";
            grid.Cols[63].Caption = "UT탐상감도Gain(LCW3)";
            grid.Cols[64].Caption = "UT탐상감도Gain(LCW4)";
            grid.Cols[65].Caption = "UT탐상감도Gain(LCW5)";
            grid.Cols[66].Caption = "UT탐상감도Gain(LCW6)";
            grid.Cols[67].Caption = "UT탐상감도Gain(LCW7)";
            grid.Cols[68].Caption = "UT탐상감도Gain(LCW8)";
            grid.Cols[69].Caption = "UT탐상감도Gain(LCW9)";
            grid.Cols[70].Caption = "UT탐상감도Gain(LCW10)";
            grid.Cols[71].Caption = "UT탐상감도Gain(LCCW1)";
            grid.Cols[72].Caption = "UT탐상감도Gain(LCCW2)";
            grid.Cols[73].Caption = "UT탐상감도Gain(LCCW3)";
            grid.Cols[74].Caption = "UT탐상감도Gain(LCCW4)";
            grid.Cols[75].Caption = "UT탐상감도Gain(LCCW5)";
            grid.Cols[76].Caption = "UT탐상감도Gain(LCCW6)";
            grid.Cols[77].Caption = "UT탐상감도Gain(LCCW7)";
            grid.Cols[78].Caption = "UT탐상감도Gain(LCCW8)";
            grid.Cols[79].Caption = "UT탐상감도Gain(LCCW9)";
            grid.Cols[80].Caption = "UT탐상감도Gain(LCCW10)";
            grid.Cols[81].Caption = "UT탐상감도Gain(D1)";
            grid.Cols[82].Caption = "UT탐상감도Gain(D2)";
            grid.Cols[83].Caption = "UT탐상감도Gain(D3)";
            grid.Cols[84].Caption = "UT탐상감도Gain(D4)";
            grid.Cols[85].Caption = "UT탐상감도Gain(D5)";
            grid.Cols[86].Caption = "UT탐상감도Gain(D6)";
            grid.Cols[87].Caption = "UT탐상감도Gain(D7)";
            grid.Cols[88].Caption = "UT탐상감도Gain(D8)";
            grid.Cols[89].Caption = "UT탐상감도Gain(D9)";
            grid.Cols[90].Caption = "UT탐상감도Gain(D10)";
            grid.Cols[91].Caption = "UT NG레벨";
            grid.Cols[92].Caption = "UT감도보정";
            grid.Cols[93].Caption = "UT전단불감대";
            grid.Cols[94].Caption = "UT후단불감대";
            grid.Cols[95].Caption = "UT주사방법";
            grid.Cols[96].Caption = "UT탐촉자초점거리";
            grid.Cols[97].Caption = "시험편NO";
            grid.Cols[98].Caption = "인공결함";
            grid.Cols[99].Caption = "MLFT결함크기";
            grid.Cols[100].Caption = "MLFT결함깊이";
            grid.Cols[101].Caption = "MLFT결함길이";
            grid.Cols[102].Caption = "UT수직결함크기";
            grid.Cols[103].Caption = "UT수직결함깊이";
            grid.Cols[104].Caption = "UT수직결함길이";
            grid.Cols[105].Caption = "UT사각결함크기";
            grid.Cols[106].Caption = "UT사각결함깊이";
            grid.Cols[107].Caption = "UT사각결함길이";
            grid.Cols[108].Caption = "검사속도";
            #endregion

            #region Name
            grid.Cols[0].Name = "L_NO";
            grid.Cols[1].Name = "WORK_DATE";
            grid.Cols[2].Name = "WORK_TIME";
            grid.Cols[3].Name = "POC_NO";
            grid.Cols[4].Name = "MILL_NO";
            grid.Cols[5].Name = "HEAT";
            grid.Cols[6].Name = "STEEL";
            grid.Cols[7].Name = "STEEL_NM";
            grid.Cols[8].Name = "ITEM";
            grid.Cols[9].Name = "ITEM_SIZE";
            grid.Cols[10].Name = "LENGTH";
            grid.Cols[11].Name = "INSP_PCS";
            grid.Cols[12].Name = "GOOD_PCS";
            grid.Cols[13].Name = "ALL_NG_PCS";
            grid.Cols[14].Name = "MAT_NG_PCS";
            grid.Cols[15].Name = "MLFT_NG_PCS";
            grid.Cols[16].Name = "UT_NG_PCS";
            grid.Cols[17].Name = "MAT_FREQUENCY";
            grid.Cols[18].Name = "MAT_CURRENTMAX";
            grid.Cols[19].Name = "MAT_ATTENUATION";
            grid.Cols[20].Name = "MAT_SENSITIVITY";
            grid.Cols[21].Name = "MAT_HARMONIC";
            grid.Cols[22].Name = "MLFT_RPM";
            grid.Cols[23].Name = "MLFT_BEF_DEAD_ZONE";
            grid.Cols[24].Name = "MLFT_AFT_DEAD_ZONE";
            grid.Cols[25].Name = "MLFT_FAULT_LEVEL_F1";
            grid.Cols[26].Name = "MLFT_FAULT_LEVEL_F2";
            grid.Cols[27].Name = "MLFT_FAULT_LEVEL_F3";
            grid.Cols[28].Name = "MLFT_FAULT_ADJDT_STD";
            grid.Cols[29].Name = "MLFT_SENS_CORR";
            grid.Cols[30].Name = "MLFT_YOKE_FREQUENCY";
            grid.Cols[31].Name = "MLFT_SCAN_SENS";
            grid.Cols[32].Name = "MLFT_PHASE";
            grid.Cols[33].Name = "MLFT_Y_GAIN";
            grid.Cols[34].Name = "MLFT_FILTER_MODE";
            grid.Cols[35].Name = "MLFT_LP_FILTER";
            grid.Cols[36].Name = "MLFT_HP_FILTER";
            grid.Cols[37].Name = "MLFT_FILTER_CORR";
            grid.Cols[38].Name = "MLFT_P_SFS_LENGTH_F1";
            grid.Cols[39].Name = "MLFT_P_SFS_LENGTH_F2";
            grid.Cols[40].Name = "MLFT_P_SFS_LENGTH_F3";
            grid.Cols[41].Name = "MLFT_GAIN_CORRECTION_1";
            grid.Cols[42].Name = "MLFT_GAIN_CORRECTION_2";
            grid.Cols[43].Name = "MLFT_GAIN_CORRECTION_3";
            grid.Cols[44].Name = "MLFT_GAIN_CORRECTION_4";
            grid.Cols[45].Name = "MLFT_GAIN_CORRECTION_5";
            grid.Cols[46].Name = "MLFT_GAIN_CORRECTION_6";
            grid.Cols[47].Name = "MLFT_GAIN_CORRECTION_7";
            grid.Cols[48].Name = "MLFT_GAIN_CORRECTION_8";
            grid.Cols[49].Name = "MLFT_GAIN_CORRECTION_9";
            grid.Cols[50].Name = "MLFT_GAIN_CORRECTION_10";
            grid.Cols[51].Name = "MLFT_GAIN_CORRECTION_11";
            grid.Cols[52].Name = "MLFT_GAIN_CORRECTION_12";
            grid.Cols[53].Name = "UT_VELOCITY_L";
            grid.Cols[54].Name = "UT_VELOCITY_D";
            grid.Cols[55].Name = "UT_PRF";
            grid.Cols[56].Name = "UT_PROBE_FREQUENCY";
            grid.Cols[57].Name = "UT_PROBE_RADIUS";
            grid.Cols[58].Name = "UT_PROBE_PITCH";
            grid.Cols[59].Name = "UT_VER_ANGLE";
            grid.Cols[60].Name = "UT_DUT_ANGLE";
            grid.Cols[61].Name = "UT_GAIN_LCW1";
            grid.Cols[62].Name = "UT_GAIN_LCW2";
            grid.Cols[63].Name = "UT_GAIN_LCW3";
            grid.Cols[64].Name = "UT_GAIN_LCW4";
            grid.Cols[65].Name = "UT_GAIN_LCW5";
            grid.Cols[66].Name = "UT_GAIN_LCW6";
            grid.Cols[67].Name = "UT_GAIN_LCW7";
            grid.Cols[68].Name = "UT_GAIN_LCW8";
            grid.Cols[69].Name = "UT_GAIN_LCW9";
            grid.Cols[70].Name = "UT_GAIN_LCW10";
            grid.Cols[71].Name = "UT_GAIN_LCCW1";
            grid.Cols[72].Name = "UT_GAIN_LCCW2";
            grid.Cols[73].Name = "UT_GAIN_LCCW3";
            grid.Cols[74].Name = "UT_GAIN_LCCW4";
            grid.Cols[75].Name = "UT_GAIN_LCCW5";
            grid.Cols[76].Name = "UT_GAIN_LCCW6";
            grid.Cols[77].Name = "UT_GAIN_LCCW7";
            grid.Cols[78].Name = "UT_GAIN_LCCW8";
            grid.Cols[79].Name = "UT_GAIN_LCCW9";
            grid.Cols[80].Name = "UT_GAIN_LCWC10";
            grid.Cols[81].Name = "UT_GAIN_D1";
            grid.Cols[82].Name = "UT_GAIN_D2";
            grid.Cols[83].Name = "UT_GAIN_D3";
            grid.Cols[84].Name = "UT_GAIN_D4";
            grid.Cols[85].Name = "UT_GAIN_D5";
            grid.Cols[86].Name = "UT_GAIN_D6";
            grid.Cols[87].Name = "UT_GAIN_D7";
            grid.Cols[88].Name = "UT_GAIN_D8";
            grid.Cols[89].Name = "UT_GAIN_D9";
            grid.Cols[90].Name = "UT_GAIN_D10";
            grid.Cols[91].Name = "UT_FAULT_LEVEL";
            grid.Cols[92].Name = "UT_SENS_CORR";
            grid.Cols[93].Name = "UT_BEF_DEAD_ZONE";
            grid.Cols[94].Name = "UT_AFT_DEAD_ZONE";
            grid.Cols[95].Name = "UT_INJ_METHOD";
            grid.Cols[96].Name = "UT_PROBE_FOCAL";
            grid.Cols[97].Name = "SAMPLE_NO";
            grid.Cols[98].Name = "ARTI_FAULT";
            grid.Cols[99].Name = "MLFT_FAULT_SIZE";
            grid.Cols[100].Name = "MLFT_FAULT_DEPTH";
            grid.Cols[101].Name = "MLFT_FAULT_LENGTH";
            grid.Cols[102].Name = "UT_VER_FAULT_SIZE";
            grid.Cols[103].Name = "UT_VER_FAULT_DEPTH";
            grid.Cols[104].Name = "UT_VER_FAULT_LENGTH";
            grid.Cols[105].Name = "UT_DUT_FAULT_SIZE";
            grid.Cols[106].Name = "UT_DUT_FAULT_DEPTH";
            grid.Cols[107].Name = "UT_DUT_FAULT_LENGTH";
            grid.Cols[108].Name = "INSP_SPEED";
            #endregion

            #region Width
            grid.Cols["L_NO"                   ].Width = cs.L_No_Width;
            grid.Cols["WORK_DATE"              ].Width = cs.Date_8_Width;
            grid.Cols["WORK_TIME"              ].Width = cs.Date_8_Width;
            grid.Cols["POC_NO"                 ].Width = cs.POC_NO_Width;
            grid.Cols["MILL_NO"                ].Width = cs.Mill_No_Width;
            grid.Cols["HEAT"                   ].Width = cs.HEAT_Width;
            grid.Cols["STEEL"                  ].Width = cs.STEEL_Width;
            grid.Cols["STEEL_NM"               ].Width = cs.STEEL_NM_Width;
            grid.Cols["ITEM"                   ].Width = cs.ITEM_Width;
            grid.Cols["ITEM_SIZE"              ].Width = cs.ITEM_SIZE_Width;
            grid.Cols["LENGTH"                 ].Width = cs.LENGTH_Width;
            grid.Cols["INSP_PCS"               ].Width = cs.PCS_L_Width;
            grid.Cols["GOOD_PCS"               ].Width = cs.PCS_L_Width;
            grid.Cols["ALL_NG_PCS"             ].Width = cs.PCS_L_Width;
            grid.Cols["MAT_NG_PCS"             ].Width = cs.PCS_L_Width;
            grid.Cols["MLFT_NG_PCS"            ].Width = cs.PCS_L_Width;
            grid.Cols["UT_NG_PCS"              ].Width = cs.PCS_L_Width;

            grid.Cols["MAT_FREQUENCY"          ].Width = cs.MAT_FREQUENCY_Width ;           
            grid.Cols["MAT_CURRENTMAX"         ].Width = cs.MAT_CURRENTMAX_Width ;          
            grid.Cols["MAT_ATTENUATION"        ].Width = cs.MAT_ATTENUATION_Width ;         
            grid.Cols["MAT_SENSITIVITY"        ].Width = cs.MAT_SENSITIVITY_Width ;         
            grid.Cols["MAT_HARMONIC"           ].Width = cs.MAT_HARMONIC_Width ;            
            grid.Cols["MLFT_RPM"               ].Width = cs.MLFT_RPM_Width ;                
            grid.Cols["MLFT_BEF_DEAD_ZONE"     ].Width = cs.MLFT_BEF_DEAD_ZONE_Width ;      
            grid.Cols["MLFT_AFT_DEAD_ZONE"     ].Width = cs.MLFT_AFT_DEAD_ZONE_Width ;      
            grid.Cols["MLFT_FAULT_LEVEL_F1"    ].Width = cs.MLFT_FAULT_LEVEL_F1_Width ;     
            grid.Cols["MLFT_FAULT_LEVEL_F2"    ].Width = cs.MLFT_FAULT_LEVEL_F2_Width ;     
            grid.Cols["MLFT_FAULT_LEVEL_F3"    ].Width = cs.MLFT_FAULT_LEVEL_F3_Width;      
            grid.Cols["MLFT_FAULT_ADJDT_STD"   ].Width = cs.MLFT_FAULT_ADJDT_STD_Width;     
            grid.Cols["MLFT_SENS_CORR"         ].Width = cs.MLFT_SENS_CORR_Width;           
            grid.Cols["MLFT_YOKE_FREQUENCY"    ].Width = cs.MLFT_YOKE_FREQUENCY_Width;      
            grid.Cols["MLFT_SCAN_SENS"         ].Width = cs.MLFT_SCAN_SENS_Width;           
            grid.Cols["MLFT_PHASE"             ].Width = cs.MLFT_PHASE_Width;               
            grid.Cols["MLFT_Y_GAIN"            ].Width = cs.MLFT_Y_GAIN_Width;              
            grid.Cols["MLFT_FILTER_MODE"       ].Width = cs.MLFT_FILTER_MODE_Width;         
            grid.Cols["MLFT_LP_FILTER"         ].Width = cs.MLFT_LP_FILTER_Width;           
            grid.Cols["MLFT_HP_FILTER"         ].Width = cs.MLFT_HP_FILTER_Width;           
            grid.Cols["MLFT_FILTER_CORR"       ].Width = cs.MLFT_FILTER_CORR_Width;         
            grid.Cols["MLFT_P_SFS_LENGTH_F1"   ].Width = cs.MLFT_P_SFS_LENGTH_F1_Width;     
            grid.Cols["MLFT_P_SFS_LENGTH_F2"   ].Width = cs.MLFT_P_SFS_LENGTH_F2_Width;     
            grid.Cols["MLFT_P_SFS_LENGTH_F3"   ].Width = cs.MLFT_P_SFS_LENGTH_F3_Width;     
            grid.Cols["MLFT_GAIN_CORRECTION_1" ].Width = cs.MLFT_GAIN_CORRECTION_1_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_2" ].Width = cs.MLFT_GAIN_CORRECTION_2_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_3" ].Width = cs.MLFT_GAIN_CORRECTION_3_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_4" ].Width = cs.MLFT_GAIN_CORRECTION_4_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_5" ].Width = cs.MLFT_GAIN_CORRECTION_5_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_6" ].Width = cs.MLFT_GAIN_CORRECTION_6_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_7" ].Width = cs.MLFT_GAIN_CORRECTION_7_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_8" ].Width = cs.MLFT_GAIN_CORRECTION_8_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_9" ].Width = cs.MLFT_GAIN_CORRECTION_9_Width;   
            grid.Cols["MLFT_GAIN_CORRECTION_10"].Width = cs.MLFT_GAIN_CORRECTION_10_Width;  
            grid.Cols["MLFT_GAIN_CORRECTION_11"].Width = cs.MLFT_GAIN_CORRECTION_11_Width;  
            grid.Cols["MLFT_GAIN_CORRECTION_12"].Width = cs.MLFT_GAIN_CORRECTION_12_Width;  
            grid.Cols["UT_VELOCITY_L"          ].Width = cs.UT_VELOCITY_L_Width;            
            grid.Cols["UT_VELOCITY_D"          ].Width = cs.UT_VELOCITY_D_Width;            
            grid.Cols["UT_PRF"                 ].Width = cs.UT_PRF_Width;                   
            grid.Cols["UT_PROBE_FREQUENCY"     ].Width = cs.UT_PROBE_FREQUENCY_Width;       
            grid.Cols["UT_PROBE_RADIUS"        ].Width = cs.UT_PROBE_RADIUS_Width;          
            grid.Cols["UT_PROBE_PITCH"         ].Width = cs.UT_PROBE_PITCH_Width;           
            grid.Cols["UT_VER_ANGLE"           ].Width = cs.UT_VER_ANGLE_Width;             
            grid.Cols["UT_DUT_ANGLE"           ].Width = cs.UT_DUT_ANGLE_Width;             
            grid.Cols["UT_GAIN_LCW1"           ].Width = cs.UT_GAIN_LCW1_Width;             
            grid.Cols["UT_GAIN_LCW2"           ].Width = cs.UT_GAIN_LCW2_Width;             
            grid.Cols["UT_GAIN_LCW3"           ].Width = cs.UT_GAIN_LCW3_Width;             
            grid.Cols["UT_GAIN_LCW4"           ].Width = cs.UT_GAIN_LCW4_Width;             
            grid.Cols["UT_GAIN_LCW5"           ].Width = cs.UT_GAIN_LCW5_Width;             
            grid.Cols["UT_GAIN_LCW6"           ].Width = cs.UT_GAIN_LCW6_Width;             
            grid.Cols["UT_GAIN_LCW7"           ].Width = cs.UT_GAIN_LCW7_Width;             
            grid.Cols["UT_GAIN_LCW8"           ].Width = cs.UT_GAIN_LCW8_Width;             
            grid.Cols["UT_GAIN_LCW9"           ].Width = cs.UT_GAIN_LCW9_Width;             
            grid.Cols["UT_GAIN_LCW10"          ].Width = cs.UT_GAIN_LCW10_Width;            
            grid.Cols["UT_GAIN_LCCW1"          ].Width = cs.UT_GAIN_LCCW1_Width;            
            grid.Cols["UT_GAIN_LCCW2"          ].Width = cs.UT_GAIN_LCCW2_Width;            
            grid.Cols["UT_GAIN_LCCW3"          ].Width = cs.UT_GAIN_LCCW3_Width;            
            grid.Cols["UT_GAIN_LCCW4"          ].Width = cs.UT_GAIN_LCCW4_Width;            
            grid.Cols["UT_GAIN_LCCW5"          ].Width = cs.UT_GAIN_LCCW5_Width;            
            grid.Cols["UT_GAIN_LCCW6"          ].Width = cs.UT_GAIN_LCCW6_Width;            
            grid.Cols["UT_GAIN_LCCW7"          ].Width = cs.UT_GAIN_LCCW7_Width;            
            grid.Cols["UT_GAIN_LCCW8"          ].Width = cs.UT_GAIN_LCCW8_Width;            
            grid.Cols["UT_GAIN_LCCW9"          ].Width = cs.UT_GAIN_LCCW9_Width;            
            grid.Cols["UT_GAIN_LCWC10"         ].Width = cs.UT_GAIN_LCWC10_Width;           
            grid.Cols["UT_GAIN_D1"             ].Width = cs.UT_GAIN_D1_Width;               
            grid.Cols["UT_GAIN_D2"             ].Width = cs.UT_GAIN_D2_Width;               
            grid.Cols["UT_GAIN_D3"             ].Width = cs.UT_GAIN_D3_Width;               
            grid.Cols["UT_GAIN_D4"             ].Width = cs.UT_GAIN_D4_Width;               
            grid.Cols["UT_GAIN_D5"             ].Width = cs.UT_GAIN_D5_Width;               
            grid.Cols["UT_GAIN_D6"             ].Width = cs.UT_GAIN_D6_Width;               
            grid.Cols["UT_GAIN_D7"             ].Width = cs.UT_GAIN_D7_Width;               
            grid.Cols["UT_GAIN_D8"             ].Width = cs.UT_GAIN_D8_Width;               
            grid.Cols["UT_GAIN_D9"             ].Width = cs.UT_GAIN_D9_Width;               
            grid.Cols["UT_GAIN_D10"            ].Width = cs.UT_GAIN_D10_Width;              
            grid.Cols["UT_FAULT_LEVEL"         ].Width = cs.UT_FAULT_LEVEL_Width;           
            grid.Cols["UT_SENS_CORR"           ].Width = cs.UT_SENS_CORR_Width;             
            grid.Cols["UT_BEF_DEAD_ZONE"       ].Width = cs.UT_BEF_DEAD_ZONE_Width;         
            grid.Cols["UT_AFT_DEAD_ZONE"       ].Width = cs.UT_AFT_DEAD_ZONE_Width;         
            grid.Cols["UT_INJ_METHOD"          ].Width = cs.UT_INJ_METHOD_Width;            
            grid.Cols["UT_PROBE_FOCAL"         ].Width = cs.UT_PROBE_FOCAL_Width;           
            grid.Cols["SAMPLE_NO"              ].Width = cs.SAMPLE_NO_Width;                
            grid.Cols["ARTI_FAULT"             ].Width = cs.ARTI_FAULT_Width;               
            grid.Cols["MLFT_FAULT_SIZE"        ].Width = cs.MLFT_FAULT_SIZE_Width;          
            grid.Cols["MLFT_FAULT_DEPTH"       ].Width = cs.MLFT_FAULT_DEPTH_Width;         
            grid.Cols["MLFT_FAULT_LENGTH"      ].Width = cs.MLFT_FAULT_LENGTH_Width;        
            grid.Cols["UT_VER_FAULT_SIZE"      ].Width = cs.UT_VER_FAULT_SIZE_Width;        
            grid.Cols["UT_VER_FAULT_DEPTH"     ].Width = cs.UT_VER_FAULT_DEPTH_Width;       
            grid.Cols["UT_VER_FAULT_LENGTH"    ].Width = cs.UT_VER_FAULT_LENGTH_Width;      
            grid.Cols["UT_DUT_FAULT_SIZE"      ].Width = cs.UT_DUT_FAULT_SIZE_Width;        
            grid.Cols["UT_DUT_FAULT_DEPTH"     ].Width = cs.UT_DUT_FAULT_DEPTH_Width;       
            grid.Cols["UT_DUT_FAULT_LENGTH"    ].Width = cs.UT_DUT_FAULT_LENGTH_Width;      
            grid.Cols["INSP_SPEED"             ].Width = cs.INSP_SPEED_Width;
            #endregion

            #region TextAlign
            grid.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grid.Cols["L_NO"                   ].TextAlign = cs.L_NO_TextAlign;
            grid.Cols["WORK_DATE"              ].TextAlign = cs.DATE_TextAlign;
            grid.Cols["WORK_TIME"              ].TextAlign = cs.DATE_TextAlign;
            grid.Cols["POC_NO"                 ].TextAlign = cs.POC_NO_TextAlign;
            grid.Cols["MILL_NO"                ].TextAlign = cs.MILL_NO_TextAlign;
            grid.Cols["HEAT"                   ].TextAlign = cs.HEAT_TextAlign;
            grid.Cols["STEEL"                  ].TextAlign = cs.STEEL_TextAlign;
            grid.Cols["STEEL_NM"               ].TextAlign = cs.STEEL_NM_TextAlign;
            grid.Cols["ITEM"                   ].TextAlign = cs.ITEM_TextAlign;
            grid.Cols["ITEM_SIZE"              ].TextAlign = cs.ITEM_SIZE_TextAlign;
            grid.Cols["LENGTH"                 ].TextAlign = cs.LENGTH_TextAlign;
            grid.Cols["INSP_PCS"               ].TextAlign = cs.PCS_TextAlign;
            grid.Cols["GOOD_PCS"               ].TextAlign = cs.PCS_TextAlign;
            grid.Cols["ALL_NG_PCS"             ].TextAlign = cs.PCS_TextAlign;
            grid.Cols["MAT_NG_PCS"             ].TextAlign = cs.PCS_TextAlign;
            grid.Cols["MLFT_NG_PCS"            ].TextAlign = cs.PCS_TextAlign;
            grid.Cols["UT_NG_PCS"              ].TextAlign = cs.PCS_TextAlign;
            grid.Cols["MAT_FREQUENCY"          ].TextAlign = cs.MAT_FREQUENCY_TextAlign;
            grid.Cols["MAT_CURRENTMAX"         ].TextAlign = cs.MAT_CURRENTMAX_TextAlign;
            grid.Cols["MAT_ATTENUATION"        ].TextAlign = cs.MAT_ATTENUATION_TextAlign;
            grid.Cols["MAT_SENSITIVITY"        ].TextAlign = cs.MAT_SENSITIVITY_TextAlign;
            grid.Cols["MAT_HARMONIC"           ].TextAlign = cs.MAT_HARMONIC_TextAlign;
            grid.Cols["MLFT_RPM"               ].TextAlign = cs.MLFT_RPM_TextAlign;
            grid.Cols["MLFT_BEF_DEAD_ZONE"     ].TextAlign = cs.MLFT_BEF_DEAD_ZONE_TextAlign;
            grid.Cols["MLFT_AFT_DEAD_ZONE"     ].TextAlign = cs.MLFT_AFT_DEAD_ZONE_TextAlign;
            grid.Cols["MLFT_FAULT_LEVEL_F1"    ].TextAlign = cs.MLFT_FAULT_LEVEL_F1_TextAlign;
            grid.Cols["MLFT_FAULT_LEVEL_F2"    ].TextAlign = cs.MLFT_FAULT_LEVEL_F2_TextAlign;
            grid.Cols["MLFT_FAULT_LEVEL_F3"    ].TextAlign = cs.MLFT_FAULT_LEVEL_F3_TextAlign;
            grid.Cols["MLFT_FAULT_ADJDT_STD"   ].TextAlign = cs.MLFT_FAULT_ADJDT_STD_TextAlign;
            grid.Cols["MLFT_SENS_CORR"         ].TextAlign = cs.MLFT_SENS_CORR_TextAlign;
            grid.Cols["MLFT_YOKE_FREQUENCY"    ].TextAlign = cs.MLFT_YOKE_FREQUENCY_TextAlign;
            grid.Cols["MLFT_SCAN_SENS"         ].TextAlign = cs.MLFT_SCAN_SENS_TextAlign;
            grid.Cols["MLFT_PHASE"             ].TextAlign = cs.MLFT_PHASE_TextAlign;
            grid.Cols["MLFT_Y_GAIN"            ].TextAlign = cs.MLFT_Y_GAIN_TextAlign;
            grid.Cols["MLFT_FILTER_MODE"       ].TextAlign = cs.MLFT_FILTER_MODE_TextAlign;
            grid.Cols["MLFT_LP_FILTER"         ].TextAlign = cs.MLFT_LP_FILTER_TextAlign;
            grid.Cols["MLFT_HP_FILTER"         ].TextAlign = cs.MLFT_HP_FILTER_TextAlign;
            grid.Cols["MLFT_FILTER_CORR"       ].TextAlign = cs.MLFT_FILTER_CORR_TextAlign;
            grid.Cols["MLFT_P_SFS_LENGTH_F1"   ].TextAlign = cs.MLFT_P_SFS_LENGTH_F1_TextAlign;
            grid.Cols["MLFT_P_SFS_LENGTH_F2"   ].TextAlign = cs.MLFT_P_SFS_LENGTH_F2_TextAlign;
            grid.Cols["MLFT_P_SFS_LENGTH_F3"   ].TextAlign = cs.MLFT_P_SFS_LENGTH_F3_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_1" ].TextAlign = cs.MLFT_GAIN_CORRECTION_1_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_2" ].TextAlign = cs.MLFT_GAIN_CORRECTION_2_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_3" ].TextAlign = cs.MLFT_GAIN_CORRECTION_3_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_4" ].TextAlign = cs.MLFT_GAIN_CORRECTION_4_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_5" ].TextAlign = cs.MLFT_GAIN_CORRECTION_5_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_6" ].TextAlign = cs.MLFT_GAIN_CORRECTION_6_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_7" ].TextAlign = cs.MLFT_GAIN_CORRECTION_7_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_8" ].TextAlign = cs.MLFT_GAIN_CORRECTION_8_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_9" ].TextAlign = cs.MLFT_GAIN_CORRECTION_9_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_10"].TextAlign = cs.MLFT_GAIN_CORRECTION_10_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_11"].TextAlign = cs.MLFT_GAIN_CORRECTION_11_TextAlign;
            grid.Cols["MLFT_GAIN_CORRECTION_12"].TextAlign = cs.MLFT_GAIN_CORRECTION_12_TextAlign;
            grid.Cols["UT_VELOCITY_L"          ].TextAlign = cs.UT_VELOCITY_L_TextAlign;
            grid.Cols["UT_VELOCITY_D"          ].TextAlign = cs.UT_VELOCITY_D_TextAlign;
            grid.Cols["UT_PRF"                 ].TextAlign = cs.UT_PRF_TextAlign;
            grid.Cols["UT_PROBE_FREQUENCY"     ].TextAlign = cs.UT_PROBE_FREQUENCY_TextAlign;
            grid.Cols["UT_PROBE_RADIUS"        ].TextAlign = cs.UT_PROBE_RADIUS_TextAlign;
            grid.Cols["UT_PROBE_PITCH"         ].TextAlign = cs.UT_PROBE_PITCH_TextAlign;
            grid.Cols["UT_VER_ANGLE"           ].TextAlign = cs.UT_VER_ANGLE_TextAlign;
            grid.Cols["UT_DUT_ANGLE"           ].TextAlign = cs.UT_DUT_ANGLE_TextAlign;
            grid.Cols["UT_GAIN_LCW1"           ].TextAlign = cs.UT_GAIN_LCW1_TextAlign;
            grid.Cols["UT_GAIN_LCW2"           ].TextAlign = cs.UT_GAIN_LCW2_TextAlign;
            grid.Cols["UT_GAIN_LCW3"           ].TextAlign = cs.UT_GAIN_LCW3_TextAlign;
            grid.Cols["UT_GAIN_LCW4"           ].TextAlign = cs.UT_GAIN_LCW4_TextAlign;
            grid.Cols["UT_GAIN_LCW5"           ].TextAlign = cs.UT_GAIN_LCW5_TextAlign;
            grid.Cols["UT_GAIN_LCW6"           ].TextAlign = cs.UT_GAIN_LCW6_TextAlign;
            grid.Cols["UT_GAIN_LCW7"           ].TextAlign = cs.UT_GAIN_LCW7_TextAlign;
            grid.Cols["UT_GAIN_LCW8"           ].TextAlign = cs.UT_GAIN_LCW8_TextAlign;
            grid.Cols["UT_GAIN_LCW9"           ].TextAlign = cs.UT_GAIN_LCW9_TextAlign;
            grid.Cols["UT_GAIN_LCW10"          ].TextAlign = cs.UT_GAIN_LCW10_TextAlign;
            grid.Cols["UT_GAIN_LCCW1"          ].TextAlign = cs.UT_GAIN_LCCW1_TextAlign;
            grid.Cols["UT_GAIN_LCCW2"          ].TextAlign = cs.UT_GAIN_LCCW2_TextAlign;
            grid.Cols["UT_GAIN_LCCW3"          ].TextAlign = cs.UT_GAIN_LCCW3_TextAlign;
            grid.Cols["UT_GAIN_LCCW4"          ].TextAlign = cs.UT_GAIN_LCCW4_TextAlign;
            grid.Cols["UT_GAIN_LCCW5"          ].TextAlign = cs.UT_GAIN_LCCW5_TextAlign;
            grid.Cols["UT_GAIN_LCCW6"          ].TextAlign = cs.UT_GAIN_LCCW6_TextAlign;
            grid.Cols["UT_GAIN_LCCW7"          ].TextAlign = cs.UT_GAIN_LCCW7_TextAlign;
            grid.Cols["UT_GAIN_LCCW8"          ].TextAlign = cs.UT_GAIN_LCCW8_TextAlign;
            grid.Cols["UT_GAIN_LCCW9"          ].TextAlign = cs.UT_GAIN_LCCW9_TextAlign;
            grid.Cols["UT_GAIN_LCWC10"         ].TextAlign = cs.UT_GAIN_LCWC10_TextAlign;// 
            grid.Cols["UT_GAIN_D1"             ].TextAlign = cs.UT_GAIN_D1_TextAlign;
            grid.Cols["UT_GAIN_D2"             ].TextAlign = cs.UT_GAIN_D2_TextAlign;
            grid.Cols["UT_GAIN_D3"             ].TextAlign = cs.UT_GAIN_D3_TextAlign;
            grid.Cols["UT_GAIN_D4"             ].TextAlign = cs.UT_GAIN_D4_TextAlign;
            grid.Cols["UT_GAIN_D5"             ].TextAlign = cs.UT_GAIN_D5_TextAlign;
            grid.Cols["UT_GAIN_D6"             ].TextAlign = cs.UT_GAIN_D6_TextAlign;
            grid.Cols["UT_GAIN_D7"             ].TextAlign = cs.UT_GAIN_D7_TextAlign;
            grid.Cols["UT_GAIN_D8"             ].TextAlign = cs.UT_GAIN_D8_TextAlign;
            grid.Cols["UT_GAIN_D9"             ].TextAlign = cs.UT_GAIN_D9_TextAlign;
            grid.Cols["UT_GAIN_D10"            ].TextAlign = cs.UT_GAIN_D10_TextAlign;
            grid.Cols["UT_FAULT_LEVEL"         ].TextAlign = cs.UT_FAULT_LEVEL_TextAlign;
            grid.Cols["UT_SENS_CORR"           ].TextAlign = cs.UT_SENS_CORR_TextAlign;
            grid.Cols["UT_BEF_DEAD_ZONE"       ].TextAlign = cs.UT_BEF_DEAD_ZONE_TextAlign;
            grid.Cols["UT_AFT_DEAD_ZONE"       ].TextAlign = cs.UT_AFT_DEAD_ZONE_TextAlign;
            grid.Cols["UT_INJ_METHOD"          ].TextAlign = cs.UT_INJ_METHOD_TextAlign;
            grid.Cols["UT_PROBE_FOCAL"         ].TextAlign = cs.UT_PROBE_FOCAL_TextAlign;
            grid.Cols["SAMPLE_NO"              ].TextAlign = cs.SAMPLE_NO_TextAlign;
            grid.Cols["ARTI_FAULT"             ].TextAlign = cs.ARTI_FAULT_TextAlign;
            grid.Cols["MLFT_FAULT_SIZE"        ].TextAlign = cs.MLFT_FAULT_SIZE_TextAlign;
            grid.Cols["MLFT_FAULT_DEPTH"       ].TextAlign = cs.MLFT_FAULT_DEPTH_TextAlign;
            grid.Cols["MLFT_FAULT_LENGTH"      ].TextAlign = cs.MLFT_FAULT_LENGTH_TextAlign;
            grid.Cols["UT_VER_FAULT_SIZE"      ].TextAlign = cs.UT_VER_FAULT_SIZE_TextAlign;
            grid.Cols["UT_VER_FAULT_DEPTH"     ].TextAlign = cs.UT_VER_FAULT_DEPTH_TextAlign;
            grid.Cols["UT_VER_FAULT_LENGTH"    ].TextAlign = cs.UT_VER_FAULT_LENGTH_TextAlign;
            grid.Cols["UT_DUT_FAULT_SIZE"      ].TextAlign = cs.UT_DUT_FAULT_SIZE_TextAlign;
            grid.Cols["UT_DUT_FAULT_DEPTH"     ].TextAlign = cs.UT_DUT_FAULT_DEPTH_TextAlign;
            grid.Cols["UT_DUT_FAULT_LENGTH"    ].TextAlign = cs.UT_DUT_FAULT_LENGTH_TextAlign;
            grid.Cols["INSP_SPEED"             ].TextAlign = cs.INSP_SPEED_TextAlign;
            #endregion

            #region Format

            for (int col = 0; col < grid.Cols.Count; col++)
            {
                if (col == grid.Cols["LENGTH"].Index)
                {
                    grid.Cols[col].Format = "N0";
                }
                else if (  col > grid.Cols["LENGTH"].Index 
                       &&  col != grid.Cols["MAT_HARMONIC"].Index
                       &&  col != grid.Cols["MLFT_FAULT_ADJDT_STD"].Index
                       &&  col != grid.Cols["MLFT_FILTER_MODE"].Index
                       &&  col != grid.Cols["UT_INJ_METHOD"].Index
                       &&  col != grid.Cols["SAMPLE_NO"].Index
                       &&  col != grid.Cols["ARTI_FAULT"].Index
                         )
                {
                    grid.Cols[col].Format = "N0";
                }

            }
            #endregion
        }

        private void InitgrdMain1()
        {
            C1FlexGrid grid = grdMain1 as C1FlexGrid;

            clsStyle.Style.InitGrid_search(grid);

            grid.Rows[0].Height = cs.Head_Height;
            
            #region Width 설정
            grid.AllowEditing = false;

            grid.Cols["INSP_PCS"].Caption = "  검사\n본수";
            grid.Cols["GOOD_PCS"].Caption = "  합격\n본수";
            grid.Cols["MAT_NG_PCS"].Caption = "  MAT\nNG본수";
            grid.Cols["MLFT_NG_PCS"].Caption = "  MLFT\nNG본수";
            grid.Cols["UT_NG_PCS"].Caption = "  UT\nNG본수";
            grid.Cols["TOT_NG_PCS"].Caption = "  총\nNG본수";

            grid.Cols["L_NO"].Width = cs.L_No_Width;
            grid.Cols["MFG_DATE"].Width = cs.Date_8_Width + 20;
            grid.Cols["WORK_TYPE"].Width = 0;
            grid.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width;
            grid.Cols["WORK_TEAM"].Width = 0;
            grid.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width;
            grid.Cols["POC_NO"].Width = cs.POC_NO_Width - 30;
            grid.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grid.Cols["HEAT"].Width = cs.HEAT_Width -20;
            grid.Cols["STEEL"].Width = cs.STEEL_Width;
            grid.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width+10;
            grid.Cols["ITEM"].Width = cs.ITEM_Width +15;
            grid.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grid.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grid.Cols["INSP_PCS"].Width = cs.PCS_Width +10;
            grid.Cols["GOOD_PCS"].Width = cs.PCS_Width + 10;
            grid.Cols["MAT_NG_PCS"].Width = cs.PCS_Width + 10;
            grid.Cols["MLFT_NG_PCS"].Width = cs.PCS_Width + 10;
            grid.Cols["UT_NG_PCS"].Width = cs.PCS_Width + 10;
            grid.Cols["TOT_NG_PCS"].Width = cs.PCS_Width + 10;
            #endregion Width 설정

            #region  TextAlign 설정
            grid.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grid.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grid.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grid.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grid.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grid.Cols["WORK_TEAM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grid.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grid.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grid.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grid.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grid.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grid.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grid.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grid.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grid.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grid.Cols["INSP_PCS"].TextAlign = cs.PCS_TextAlign;
            grid.Cols["GOOD_PCS"].TextAlign = cs.PCS_TextAlign;
            grid.Cols["MAT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            grid.Cols["MLFT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            grid.Cols["UT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            grid.Cols["TOT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            #endregion  TextAlign 설정

            grid.Tree.Column = 1;
        }


        #endregion

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
            if (_CanSaveSearchLog)
            {
                cd.InsertLogForSearch(ck.UserID, btnDisplay);
            }


            C1FlexGrid grid = new C1FlexGrid();
            DataTable dt = new DataTable();
            DataTable olddt = new DataTable();

            if (current_Tab == "TabP1")
            {
                grid = grdMain1;
                dt = moddt_Main1;
                olddt = olddt_Main1;
            }
            else if (current_Tab == "TabP2")
            {
                grid = grdMain2;
                dt = moddt_Main2;
                olddt = olddt_Main2;
            }
            SetBindingData(grid, dt, olddt);
        }

        private void SetBindingData(C1FlexGrid grid, DataTable dt, DataTable olddt)
        {
            try
            {
                string sql = "";

                //쿼리 가져욤
                sql = QuerySetup();

                dt = cd.FindDataTable(sql);
                olddt = dt.Copy();

                Cursor.Current = Cursors.WaitCursor;
                grid.SetDataBinding(dt, null, true);
                Cursor.Current = System.Windows.Forms.Cursors.Default;

                if (current_Tab == "TabP1" && grid.Name == "grdMain1")
                {
                    UpdateTotals(grid);
                }


                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), olddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }

        private void UpdateTotals(C1FlexGrid grid)
        {
            subtotalNo = 0;

            // clear existing totals
            grid.Subtotal(AggregateEnum.Clear);

            grid.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grid.Cols["INSP_PCS"].Index, "합계");
            grid.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grid.Cols["GOOD_PCS"].Index, "합계");

            grid.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grid.Cols["MAT_NG_PCS"].Index, "합계");
            grid.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grid.Cols["MLFT_NG_PCS"].Index, "합계");

            grid.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grid.Cols["UT_NG_PCS"].Index, "합계");
            grid.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grid.Cols["TOT_NG_PCS"].Index, "합계");




            //sql1 += string.Format("       ,INSP_PCS "); //--검사본수
            //sql1 += string.Format("       ,INSP_PCS - TOT_NG_PCS AS GOOD_PCS "); //--합격본수
            //sql1 += string.Format("       ,MAT_NG_PCS "); //--MAT NG본수
            //sql1 += string.Format("       ,MLFT_NG_PCS "); //--MLFT NG본수
            //sql1 += string.Format("       ,UT_NG_PCS   "); //--UT NG본수
            //sql1 += string.Format("       ,TOT_NG_PCS  "); //--총 NG본수

            AddSubtotalNo();
            grid.Rows.Frozen = GetAvailMinRow(grid)-1;
        }

        private void AddSubtotalNo()
        {
            ++subtotalNo;
        }

        private int GetAvailMinRow(C1FlexGrid grid)
        {
            return (grid.Rows.Fixed + subtotalNo);
        }
        #endregion

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
                dlg.FileName = "NDT_실적_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            }
            if (current_Tab == "TabP2")
            {
                grid = grdMain2;
                dlg.FileName = "NDT_조업_조회" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
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

            btnDisplay_Click(null, null);
        }

        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = cd_id;
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
                sql1 += string.Format("       ,MFG_DATE "); //INPUT_DDTT 에서 MFG_DATE로 변경 
                sql1 += string.Format("       ,WORK_TYPE ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                sql1 += string.Format("       ,WORK_TEAM ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                sql1 += string.Format("       ,POC_NO ");
                sql1 += string.Format("       ,MILL_NO ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,INSP_PCS "); //--검사본수
                sql1 += string.Format("       ,INSP_PCS - TOT_NG_PCS AS GOOD_PCS "); //--합격본수
                sql1 += string.Format("       ,MAT_NG_PCS "); //--MAT NG본수
                sql1 += string.Format("       ,MLFT_NG_PCS "); //--MLFT NG본수
                sql1 += string.Format("       ,UT_NG_PCS   "); //--UT NG본수
                sql1 += string.Format("       ,TOT_NG_PCS  "); //--총 NG본수
                sql1 += string.Format("       ,ENTRY_DDTT  "); //--총 NG본수
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT  TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD')        AS MFG_DATE");
                sql1 += string.Format("               ,A.WORK_TYPE      AS WORK_TYPE ");
                sql1 += string.Format("               ,NVL(A.WORK_TEAM,'O')      AS WORK_TEAM ");
                sql1 += string.Format("               ,A.POC_NO         AS POC_NO ");
                sql1 += string.Format("               ,A.MILL_NO        AS MILL_NO ");
                sql1 += string.Format("               ,A.HEAT           AS HEAT ");
                sql1 += string.Format("               ,MAX(A.ITEM)      AS ITEM ");
                sql1 += string.Format("               ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
                sql1 += string.Format("               ,MAX(A.STEEL)     AS STEEL ");
                sql1 += string.Format("               ,TO_CHAR(MAX(A.LENGTH),'90.00')    AS LENGTH ");
                sql1 += string.Format("               ,COUNT(*)         AS INSP_PCS ");
                sql1 += string.Format("               ,SUM(DECODE(A.MAT_GOOD_NG,'NG',1,0))  AS MAT_NG_PCS ");
                sql1 += string.Format("               ,SUM(DECODE(A.MLFT_GOOD_NG, 'NG', DECODE(A.MAT_GOOD_NG, 'NG', 0, DECODE(A.UT_GOOD_NG, 'NG', 0, 1)), 0)) AS MLFT_NG_PCS ");
                sql1 += string.Format("               ,SUM(DECODE(A.UT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,1),0))   AS UT_NG_PCS ");
                sql1 += string.Format("               ,SUM(DECODE(A.MAT_GOOD_NG,'NG',1, ");
                sql1 += string.Format("                    DECODE(A.MLFT_GOOD_NG,'NG',1, ");
                sql1 += string.Format("                    DECODE(A.UT_GOOD_NG,'NG',1,0)))) AS TOT_NG_PCS, ");
                sql1 += string.Format("                    MIN(ENTRY_DDTT) ENTRY_DDTT ");
                sql1 += string.Format("        FROM   TB_CR_PIECE_WR A ");
                sql1 += string.Format("        WHERE  A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                sql1 += string.Format("        AND    A.LINE_GP    = '{0}' ", cd_id); //:P_LINE_GP
                sql1 += string.Format("        AND    A.ROUTING_CD = 'F2' ");
                if (cd_id2 != "%")
                {
                    sql1 += string.Format("          AND A.WORK_TYPE  LIKE '%{0}%' ", cd_id2);
                }
                if (cd_id3 != "%")
                {
                    sql1 += string.Format("          AND NVL(A.WORK_TEAM,'O')  LIKE '%{0}%' ", cd_id3);
                }
                if (txtHEAT.Text != "")
                {
                    sql1 += string.Format("          AND A.HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                }
                if (gangjong_id_tb.Text != "")
                {
                    sql1 += string.Format("          AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                }
                if (txtItemSize.Text != "")
                {
                    sql1 += string.Format("          AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                }
                if (txtPOC.Text != "")
                {
                    sql1 += string.Format("          AND A.POC_NO      LIKE '%{0}%' ", txtPOC.Text);
                }
                //sql1 += string.Format("        AND A.WORK_TYPE  LIKE '{0}%' ", cd_id2);
                //sql1 += string.Format("        AND A.HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                //sql1 += string.Format("        AND A.STEEL     LIKE '{0}%' ", gangjong_id_tb.Text);
                //sql1 += string.Format("        AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                //sql1 += string.Format("        AND A.POC_NO      LIKE '%{0}%' ", txtPOC.Text);
                //sql1 += string.Format("        AND NVL(A.WORK_TEAM,'A')  LIKE '%{0}%' ", cd_id3);
                sql1 += string.Format("        AND    A.REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM  TB_CR_PIECE_WR ");
                sql1 += string.Format("                               WHERE MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("                                AND  MILL_NO     = A.MILL_NO ");
                sql1 += string.Format("                                AND    PIECE_NO    = A.PIECE_NO ");
                sql1 += string.Format("                                AND    LINE_GP     = A.LINE_GP ");
                sql1 += string.Format("                                AND    ROUTING_CD  = A.ROUTING_CD ) ");
                sql1 += string.Format("        GROUP BY A.MFG_DATE ");
                sql1 += string.Format("               ,A.WORK_TYPE ");
                sql1 += string.Format("               ,A.WORK_TEAM ");
                sql1 += string.Format("               ,A.POC_NO    ");
                sql1 += string.Format("               ,A.MILL_NO   ");
                sql1 += string.Format("               ,A.HEAT ");
                sql1 += string.Format("        ORDER BY  MFG_DATE DESC, WORK_TYPE, MILL_NO  ");
                sql1 += string.Format("        ) X ");
                sql1 += string.Format("ORDER BY ENTRY_DDTT ");


                //sql1 = string.Format("SELECT   ROWNUM AS L_NO ");
                //sql1 += string.Format("       ,MFG_DATE "); //INPUT_DDTT 에서 MFG_DATE로 변경 
                //sql1 += string.Format("       ,WORK_TYPE ");
                //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                //sql1 += string.Format("       ,WORK_TEAM ");
                //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                //sql1 += string.Format("       ,POC_NO ");
                //sql1 += string.Format("       ,MILL_NO ");
                //sql1 += string.Format("       ,HEAT ");
                //sql1 += string.Format("       ,STEEL ");
                //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                //sql1 += string.Format("       ,ITEM ");
                //sql1 += string.Format("       ,ITEM_SIZE ");
                //sql1 += string.Format("       ,LENGTH ");
                //sql1 += string.Format("       ,INSP_PCS "); //--검사본수
                //sql1 += string.Format("       ,INSP_PCS - TOT_NG_PCS AS GOOD_PCS "); //--합격본수
                //sql1 += string.Format("       ,MAT_NG_PCS "); //--MAT NG본수
                //sql1 += string.Format("       ,MLFT_NG_PCS "); //--MLFT NG본수
                //sql1 += string.Format("       ,UT_NG_PCS   "); //--UT NG본수
                //sql1 += string.Format("       ,TOT_NG_PCS  "); //--총 NG본수
                //sql1 += string.Format("FROM   ( ");
                //sql1 += string.Format("        SELECT  TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD')        AS MFG_DATE");
                //sql1 += string.Format("               ,A.WORK_TYPE      AS WORK_TYPE ");
                //sql1 += string.Format("               ,NVL(A.WORK_TEAM,'A')      AS WORK_TEAM ");
                //sql1 += string.Format("               ,A.POC_NO         AS POC_NO ");
                //sql1 += string.Format("               ,A.MILL_NO        AS MILL_NO ");
                //sql1 += string.Format("               ,A.HEAT           AS HEAT ");
                //sql1 += string.Format("               ,MAX(A.ITEM)      AS ITEM ");
                //sql1 += string.Format("               ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
                //sql1 += string.Format("               ,MAX(A.STEEL)     AS STEEL ");
                //sql1 += string.Format("               ,TO_CHAR(MAX(A.LENGTH),'90.00')    AS LENGTH ");
                //sql1 += string.Format("               ,COUNT(*)         AS INSP_PCS ");
                //sql1 += string.Format("               ,SUM(DECODE(A.MAT_GOOD_NG,'NG',1,0))  AS MAT_NG_PCS ");
                //sql1 += string.Format("               ,SUM(DECODE(A.MLFT_GOOD_NG, 'NG', DECODE(A.MAT_GOOD_NG, 'NG', 0, DECODE(A.UT_GOOD_NG, 'NG', 0, 1)), 0)) AS MLFT_NG_PCS ");
                //sql1 += string.Format("               ,SUM(DECODE(A.UT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,1),0))   AS UT_NG_PCS ");
                //sql1 += string.Format("               ,SUM(DECODE(A.MAT_GOOD_NG,'NG',1, ");
                //sql1 += string.Format("                    DECODE(A.MLFT_GOOD_NG,'NG',1, ");
                //sql1 += string.Format("                    DECODE(A.UT_GOOD_NG,'NG',1,0)))) AS TOT_NG_PCS ");
                //sql1 += string.Format("        FROM   TB_CR_PIECE_WR A ");
                //sql1 += string.Format("        WHERE  A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                //sql1 += string.Format("        AND    A.LINE_GP    = '{0}' ", cd_id); //:P_LINE_GP
                //sql1 += string.Format("        AND    A.ROUTING_CD = 'F2' ");
                //sql1 += string.Format("        AND A.WORK_TYPE  LIKE '{0}%' ", cd_id2);
                //sql1 += string.Format("        AND A.HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                //sql1 += string.Format("        AND A.STEEL     LIKE '{0}%' ", gangjong_id_tb.Text);
                //sql1 += string.Format("        AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                //sql1 += string.Format("        AND A.POC_NO      LIKE '%{0}%' ", txtPOC.Text);
                //sql1 += string.Format("        AND NVL(A.WORK_TEAM,'A')  LIKE '%{0}%' ", cd_id3);
                //sql1 += string.Format("        AND    A.REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM  TB_CR_PIECE_WR ");
                //sql1 += string.Format("                                WHERE  MILL_NO     = A.MILL_NO ");
                //sql1 += string.Format("                                AND    PIECE_NO    = A.PIECE_NO ");
                //sql1 += string.Format("                                AND    LINE_GP     = A.LINE_GP ");
                //sql1 += string.Format("                                AND    ROUTING_CD  = A.ROUTING_CD ) ");
                //sql1 += string.Format("        GROUP BY A.MFG_DATE ");
                //sql1 += string.Format("               ,A.WORK_TYPE ");
                //sql1 += string.Format("               ,A.WORK_TEAM ");
                //sql1 += string.Format("               ,A.POC_NO    ");
                //sql1 += string.Format("               ,A.MILL_NO   ");
                //sql1 += string.Format("               ,A.HEAT ");
                //sql1 += string.Format("        ORDER BY  MFG_DATE DESC, WORK_TYPE, MILL_NO  ");
                //sql1 += string.Format("        ) X ");
            }

            //조업정보---------------------------------------------------------------------------------------------------------
            if (current_Tab == "TabP2")
            {
                sql1 = string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("     , X.* ");
                sql1 += string.Format("FROM( ");
                sql1 += string.Format("SELECT ");
                sql1 += string.Format("         TO_CHAR(WORK_END_DDTT,'YYYY-MM-DD')        AS WORK_DATE"); // 작업일자
                sql1 += string.Format("       , TO_CHAR(WORK_END_DDTT,'HH24:MI:SS')        AS WORK_TIME "); //작업시각
                sql1 += string.Format("       , B.POC_NO "); //POC
                sql1 += string.Format("       , A.MILL_NO "); //압연번들번호
                sql1 += string.Format("       , B.HEAT "); //HEAT
                sql1 += string.Format("       , B.STEEL "); //강종
                sql1 += string.Format("       , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM "); //강종명
                sql1 += string.Format("       , B.ITEM "); //품목
                sql1 += string.Format("       , B.ITEM_SIZE "); //규격
                sql1 += string.Format("       , TO_CHAR(B.LENGTH,'90.00') AS LENGTH "); //길이
                sql1 += string.Format("       , INSP_PCS "); //검사본수
                sql1 += string.Format("       , GOOD_PCS "); //합격본수
                sql1 += string.Format("       , ALL_NG_PCS "); //불량본수
                sql1 += string.Format("       , MAT_NG_PCS "); //MAT불량본수
                sql1 += string.Format("       , MLFT_NG_PCS "); //MLFT불량본수
                sql1 += string.Format("       , UT_NG_PCS "); //UT불량본수
                sql1 += string.Format("       , MAT_FREQUENCY "); //MAT주파수
                sql1 += string.Format("       , MAT_CURRENTMAX "); //MAT CurrentMax
                sql1 += string.Format("       , MAT_ATTENUATION "); //MAT Attenuation
                sql1 += string.Format("       , MAT_SENSITIVITY "); //MAT Sensitivity
                sql1 += string.Format("       , MAT_HARMONIC "); //MAT Harmonic
                sql1 += string.Format("       , MLFT_RPM "); //MLFT회전수
                sql1 += string.Format("       , MLFT_BEF_DEAD_ZONE "); //MLFT전단불감대
                sql1 += string.Format("       , MLFT_AFT_DEAD_ZONE "); //MLFT후단불감대
                sql1 += string.Format("       , MLFT_FAULT_LEVEL_F1 "); //MLFT불량레벨F1
                sql1 += string.Format("       , MLFT_FAULT_LEVEL_F2 "); //MLFT불량레벨F2
                sql1 += string.Format("       , MLFT_FAULT_LEVEL_F3 "); //MLFT불량레벨F3
                sql1 += string.Format("       , MLFT_FAULT_ADJDT_STD "); //MLFT불량판정기준
                sql1 += string.Format("       , MLFT_SENS_CORR "); //MLFT감도보정
                sql1 += string.Format("       , MLFT_YOKE_FREQUENCY "); //MLFT YOKE주파수
                sql1 += string.Format("       , MLFT_SCAN_SENS "); //MLFT탐상감도(Gain)
                sql1 += string.Format("       , MLFT_PHASE "); //MLFT위상(Phase)
                sql1 += string.Format("       , MLFT_Y_GAIN "); //MLFT Y GAIN
                sql1 += string.Format("       , MLFT_FILTER_MODE "); //MLFT FILTER MODE
                sql1 += string.Format("       , MLFT_LP_FILTER "); //MLFT LP FILTER
                sql1 += string.Format("       , MLFT_HP_FILTER "); //MLFT HP FILTER
                sql1 += string.Format("       , MLFT_FILTER_CORR "); //MLFT FILTER CORR
                sql1 += string.Format("       , MLFT_P_SFS_LENGTH_F1 "); //MLFT SFS Length F1
                sql1 += string.Format("       , MLFT_P_SFS_LENGTH_F2 "); //MLFT SFS Length F2
                sql1 += string.Format("       , MLFT_P_SFS_LENGTH_F3 "); //MLFT SFS Length F3
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_1 "); //MLFT Gain Correction1
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_2 "); //MLFT Gain Correction2
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_3 "); //MLFT Gain Correction3
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_4 "); //MLFT Gain Correction4
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_5 "); //MLFT Gain Correction5
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_6 "); //MLFT Gain Correction6
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_7 "); //MLFT Gain Correction7
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_8 "); //MLFT Gain Correction8
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_9 "); //MLFT Gain Correction9
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_10 "); //MLFT Gain Correction10
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_11 "); //MLFT Gain Correction11
                sql1 += string.Format("       , MLFT_GAIN_CORRECTION_12 "); //MLFT Gain Correction12
                sql1 += string.Format("       , UT_VELOCITY_L "); //UT Velocity L
                sql1 += string.Format("       , UT_VELOCITY_D "); //UT Velocity D
                sql1 += string.Format("       , UT_PRF "); //UT PRF
                sql1 += string.Format("       , UT_PROBE_FREQUENCY "); //UT Probe주파수
                sql1 += string.Format("       , UT_PROBE_RADIUS "); //UT Probe Radius
                sql1 += string.Format("       , UT_PROBE_PITCH "); //UT Probe Pitch
                sql1 += string.Format("       , UT_VER_ANGLE "); //UT수직Angle
                sql1 += string.Format("       , UT_DUT_ANGLE "); //UT사각Angle
                sql1 += string.Format("       , UT_GAIN_LCW1 "); //UT탐상감도Gain(LCW1)
                sql1 += string.Format("       , UT_GAIN_LCW2 "); //UT탐상감도Gain(LCW2)
                sql1 += string.Format("       , UT_GAIN_LCW3 "); //UT탐상감도Gain(LCW3)
                sql1 += string.Format("       , UT_GAIN_LCW4 "); //UT탐상감도Gain(LCW4)
                sql1 += string.Format("       , UT_GAIN_LCW5 "); //UT탐상감도Gain(LCW5)
                sql1 += string.Format("       , UT_GAIN_LCW6 "); //UT탐상감도Gain(LCW6)
                sql1 += string.Format("       , UT_GAIN_LCW7 "); //UT탐상감도Gain(LCW7)
                sql1 += string.Format("       , UT_GAIN_LCW8 "); //UT탐상감도Gain(LCW8)
                sql1 += string.Format("       , UT_GAIN_LCW9 "); //UT탐상감도Gain(LCW9)
                sql1 += string.Format("       , UT_GAIN_LCW10 "); //UT탐상감도Gain(LCW10)
                sql1 += string.Format("       , UT_GAIN_LCCW1 "); //UT탐상감도Gain(LCCW1)
                sql1 += string.Format("       , UT_GAIN_LCCW2 "); //UT탐상감도Gain(LCCW2)
                sql1 += string.Format("       , UT_GAIN_LCCW3 "); //UT탐상감도Gain(LCCW3)
                sql1 += string.Format("       , UT_GAIN_LCCW4 "); //UT탐상감도Gain(LCCW4)
                sql1 += string.Format("       , UT_GAIN_LCCW5 "); //UT탐상감도Gain(LCCW5)
                sql1 += string.Format("       , UT_GAIN_LCCW6 "); //UT탐상감도Gain(LCCW6)
                sql1 += string.Format("       , UT_GAIN_LCCW7 "); //UT탐상감도Gain(LCCW7)
                sql1 += string.Format("       , UT_GAIN_LCCW8 "); //UT탐상감도Gain(LCCW8)
                sql1 += string.Format("       , UT_GAIN_LCCW9 "); //UT탐상감도Gain(LCCW9)
                sql1 += string.Format("       , UT_GAIN_LCWC10 "); //UT탐상감도Gain(LCWC10)
                sql1 += string.Format("       , UT_GAIN_D1 "); //UT탐상감도Gain(D1)
                sql1 += string.Format("       , UT_GAIN_D2 "); //UT탐상감도Gain(D2)
                sql1 += string.Format("       , UT_GAIN_D3 "); //UT탐상감도Gain(D3)
                sql1 += string.Format("       , UT_GAIN_D4 "); //UT탐상감도Gain(D4)
                sql1 += string.Format("       , UT_GAIN_D5 "); //UT탐상감도Gain(D5)
                sql1 += string.Format("       , UT_GAIN_D6 "); //UT탐상감도Gain(D6)
                sql1 += string.Format("       , UT_GAIN_D7 "); //UT탐상감도Gain(D7)
                sql1 += string.Format("       , UT_GAIN_D8 "); //UT탐상감도Gain(D8)
                sql1 += string.Format("       , UT_GAIN_D9 "); //UT탐상감도Gain(D9)
                sql1 += string.Format("       , UT_GAIN_D10 "); //UT탐상감도Gain(D10)
                sql1 += string.Format("       , UT_FAULT_LEVEL "); //UT불량레벨
                sql1 += string.Format("       , UT_SENS_CORR "); //UT감도보정
                sql1 += string.Format("       , UT_BEF_DEAD_ZONE "); //UT전단불감대
                sql1 += string.Format("       , UT_AFT_DEAD_ZONE "); //UT후단불감대
                sql1 += string.Format("       , UT_INJ_METHOD "); //UT주사방법
                sql1 += string.Format("       , UT_PROBE_FOCAL "); //UT탐촉자초점거리
                sql1 += string.Format("       , SAMPLE_NO "); //시험편NO
                sql1 += string.Format("       , ARTI_FAULT "); //인공결함
                sql1 += string.Format("       , MLFT_FAULT_SIZE "); //MLFT결함크기
                sql1 += string.Format("       , MLFT_FAULT_DEPTH "); //MLFT결함깊이
                sql1 += string.Format("       , MLFT_FAULT_LENGTH "); //MLFT결함길이
                sql1 += string.Format("       , UT_VER_FAULT_SIZE "); //UT수직결함크기
                sql1 += string.Format("       , UT_VER_FAULT_DEPTH "); //UT수직결함깊이
                sql1 += string.Format("       , UT_VER_FAULT_LENGTH "); //UT수직결함길이
                sql1 += string.Format("       , UT_DUT_FAULT_SIZE "); //UT사각결함크기
                sql1 += string.Format("       , UT_DUT_FAULT_DEPTH "); //UT사각결함깊이
                sql1 += string.Format("       , UT_DUT_FAULT_LENGTH "); //UT사각결함길이
                sql1 += string.Format("       , INSP_SPEED "); //검사속도
                sql1 += string.Format("FROM   TB_NDT_OPERINFO A ");
                sql1 += string.Format("     , TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("WHERE A.MILL_NO = B.MILL_NO ");
                sql1 += string.Format("AND   A.LINE_GP    = '{0}' ", cd_id);
                sql1 += string.Format("AND   A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("      AND A.WORK_TYPE  LIKE '{0}%' ", cd_id2);
                sql1 += string.Format("      AND B.HEAT      LIKE '%{0}%' ", txtHEAT.Text);
                sql1 += string.Format("      AND B.STEEL     LIKE '{0}%' ", gangjong_id_tb.Text);
                sql1 += string.Format("      AND B.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);
                sql1 += string.Format("      AND B.POC_NO      LIKE '%{0}%' ", txtPOC.Text);
                sql1 += string.Format("ORDER BY 1, 2 ");
                sql1 += string.Format(") X ");
            }
            return sql1;
        }
        #endregion
    }
}
