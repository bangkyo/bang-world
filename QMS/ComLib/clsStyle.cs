﻿using C1.Win.C1Command;
using C1.Win.C1FlexGrid;
using C1.Win.C1Input;
using System.Data.OracleClient;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace ComLib
{
    public class clsStyle
    {
        public static clsStyle Style = new clsStyle();

        ConnectDB cd = new ConnectDB();
        clsCom cS = new clsCom();

        //static System.Drawing.Color gridBackColor = System.Drawing.Color.WhiteSmoke;
        static System.Drawing.Color gridBackColor = System.Drawing.Color.Black;

        static float fontSizeBiggerst = 18.0f;
        static float fontSizeBigger = 14.0f;
        static float fontSizeBig = 13.0f;
        static float fontSizeMiddle = 12.0f;
        static float fontSizeSmall = 11.0f;
        static float oHeadfontSize = 11.0f;
        //static int oHeadfontStyle = 1;
        static string oHeadfontName = "돋움";
        static string oHeadfontName1 = "현대하모니";


        public System.Drawing.Color GridBackColor { get { return gridBackColor; } }

        public static clsStyle GetInstance()
        {


            return Style;
        }


        #region 컬럼 사이즈
        // L_NO 3자
        int _L_No_Width = 50;
        int _Head_Height = 40;
        //CHECKER
        int _Checker_Width = 50;
        // 선택 checkbox
        int _Sel_Width = 40;
        // 압연번틀번호 10자
        int _Mill_No_Width = 110;
        // PCS 수  3자
        int _PIECE_NO_Width = 50;

        int _CHM_SNO_Width = 90;
        int _LOC_Width = 50;

        int _INSP_MODE_NM_Width = 90;
        int _INSP_GP_NM_Width = 90;

        // ENTRY_DDTT 20자 이상
        int _ENTRY_DDTT_Width = 170;
        //zone_cd 4자
        int _ZONE_CD_Width = 70;
        // poc No  7자
        int _POC_NO_Width = 120;
        int _POC_END_YN_Width = 100;
        int _POC_SEQ_Width = 80;

        int _BND_PCS_Width = 100;
        int _BND_POINT_Width = 100;
        // heat 6자
        int _HEAT_Width = 100;

        int _HEAT_YN_Width = 80;

        int _PITCH_Width = 80;
    
        // 강종 2자
        int _STEEL_Width = 40;
        //강종명 10자 이상
        int _STEEL_NM_Width = 80;
        int _STEEL_NM_L_Width = 120;
        // 품목
        int _ITEM_Width = 45;
        // 규격 4자
        int _ITEM_SIZE_Width = 45;

        int _Item_L_Width = 100;

        int _ROUTING_NM_Width = 60;

        int _POST_PROC_ROUTING_NM_Width = 200;

        // ReWork
        int _REWORK_YN_Width = 90;

        int _REWORK_SEQ_Width = 90;

        int _CATEGORY_Width = 200;

        int _L3_Grp_cd_Width = 90;

        int _LENGTH_Width = 80;

        int _LENGTH_L_Width = 110;

        int _TRANS_ROLLER_RPM_Width = 150;
        int _IMPELLER_FREQUENCY_Width = 150;

        int _LENGTH_MIN_Width = 100;
        int _LENGTH_MAX_Width = 100;
        int _UOM_NM_Width = 80;

        int _MPI_FAULT_CD_Width = 100;
        int _MPI_FAULT_NM_Width = 100;

        int _WORK_TYPE_NM_Width = 50;
        int _WORK_TEAM_NM_Width = 50;

        int _MFG_Date_Width = 140;

        int _Date_8_Width = 100;
        int _TIME_8_Width = 100;

        int _Date_14_width = 200;

        int _Angle_Value_Width = 90;

        int _Stopper_Value_Width = 100;
        int _Servo_Value_Width = 100;

        int _Guide_Value_Width = 100;

        int _Speed_Value_Width = 120;

        int _Current_Value_Width = 150;

        // 성분 
        int _Chm_value_Width = 40;

        int _Good_NG_Width = 80;
        int _Good_NG_L_Width = 90;
        int _Good_NG_S_Width = 50;

        int _Work_Rank_Width = 80;

        int _Surface_Level_NM_Width = 180;

        int _Wgt_Width = 100;

        int _NET_WGT_Width = 100;

        int _Company_NM_Width = 200;

        int _Usage_CD_NM_Width = 100;

        int _POC_PROG_STAT_NM_Width = 80;

        int _BUNDLE_QTY_Width = 80;
        int _BUNDLE_NO_Width = 130;
        int _PCS_Width = 50;
        int _PCS_L_Width = 100;

        int _FINISH_PCS_Width = 50;
        int _FINISH_WGT_Width = 50;
        int _FINISH_YN_Width = 100;
        int _CRTIN_YN_Width = 80;

        int _PROG_STAT_Width = 80;

        int _SEND_YN_Width = 100;


        //교정실적조회 관련 컬럼명, 변수들------------------- 

        int _Shortest_Value_Width = 20;
        int _Short_Value_Width = 90;
        int _Middle_Value_Width = 120;
        int _Longer_Value_Width = 150;
        int _Longest_Value_Width = 220;

        int _MILL_PCS_Width = 100;
        int _MILL_WGT_Width = 100;
        int _STR_PCS_Width = 100;
        //면취실적조회 관련 변수-------------------------------------------------
        int _SHF_PCS_Width = 110;
        int _SHF_WGT_Width = 110;
        //-----------------------------------------------------------------------
        public int Shortest_Value_Width { get { return _Short_Value_Width; } }
        public int Short_Value_Width { get { return _Short_Value_Width; } }
        public int Middle_Value_Width { get { return _Middle_Value_Width; } }
        public int Longer_Value_Width { get { return _Longer_Value_Width; } }
        public int Longest_Value_Width { get { return _Longest_Value_Width; } }

        int _FACTORY_Width = 100;

        int _SPEC_NO_Width = 100;

        int _MARKET_GP_NM_Width = 100;

        int _DEVIATION_Width = 100;

        int _PACKING_METHOD_NM_Width = 100;

        int _ALLOW_FAULT_DEPTH_Width = 100;

        int _MEMO_Width = 100;

        int _MFG_METHOD_NM_Width = 100;

        int _LINE_GP_Width = 100;

        int _WORK_UOM_Width = 100;

        int _INSP_STAT_NM_Width = 100;

        int _REWORK_RSN_NM_Width = 100;
        
        //NDT 실적관리
        int _MAT_GOOD_NG_Width = 90;

        int _MLFT_GOOD_NG_Width = 100;

        int _UT_GOOD_NG_Width = 80;


        #region NDT 실적조회
        int _MAT_FREQUENCY_Width = 80;
        int _MAT_CURRENTMAX_Width = 130;
        int _MAT_ATTENUATION_Width = 130;
        int _MAT_SENSITIVITY_Width = 130;
        int _MAT_HARMONIC_Width = 110;
        int _MLFT_RPM_Width = 90;
        int _MLFT_BEF_DEAD_ZONE_Width = 130;
        int _MLFT_AFT_DEAD_ZONE_Width = 130;
        int _MLFT_FAULT_LEVEL_F1_Width = 130;
        int _MLFT_FAULT_LEVEL_F2_Width = 130;
        int _MLFT_FAULT_LEVEL_F3_Width = 130;
        int _MLFT_FAULT_ADJDT_STD_Width = 140;
        int _MLFT_SENS_CORR_Width = 110;
        int _MLFT_YOKE_FREQUENCY_Width = 140;
        int _MLFT_SCAN_SENS_Width = 150;
        int _MLFT_PHASE_Width = 140;
        int _MLFT_Y_GAIN_Width = 130;
        int _MLFT_FILTER_MODE_Width = 150;
        int _MLFT_LP_FILTER_Width = 140;
        int _MLFT_HP_FILTER_Width = 140;
        int _MLFT_FILTER_CORR_Width = 150;
        int _MLFT_P_SFS_LENGTH_F1_Width = 160;
        int _MLFT_P_SFS_LENGTH_F2_Width = 160;
        int _MLFT_P_SFS_LENGTH_F3_Width = 160;
        int _MLFT_GAIN_CORRECTION_1_Width = 180;
        int _MLFT_GAIN_CORRECTION_2_Width = 180;
        int _MLFT_GAIN_CORRECTION_3_Width = 180;
        int _MLFT_GAIN_CORRECTION_4_Width = 180;
        int _MLFT_GAIN_CORRECTION_5_Width = 180;
        int _MLFT_GAIN_CORRECTION_6_Width = 180;
        int _MLFT_GAIN_CORRECTION_7_Width = 180;
        int _MLFT_GAIN_CORRECTION_8_Width = 180;
        int _MLFT_GAIN_CORRECTION_9_Width = 180;
        int _MLFT_GAIN_CORRECTION_10_Width = 200;
        int _MLFT_GAIN_CORRECTION_11_Width = 200;
        int _MLFT_GAIN_CORRECTION_12_Width = 200;
        int _UT_VELOCITY_L_Width = 130;
        int _UT_VELOCITY_D_Width = 130;
        int _UT_PRF_Width = 70;
        int _UT_PROBE_FREQUENCY_Width = 140;
        int _UT_PROBE_RADIUS_Width = 150;
        int _UT_PROBE_PITCH_Width = 140;
        int _UT_VER_ANGLE_Width = 120;
        int _UT_DUT_ANGLE_Width = 120;
        int _UT_GAIN_LCW1_Width = 170;
        int _UT_GAIN_LCW2_Width = 170;
        int _UT_GAIN_LCW3_Width = 170;
        int _UT_GAIN_LCW4_Width = 170;
        int _UT_GAIN_LCW5_Width = 170;
        int _UT_GAIN_LCW6_Width = 170;
        int _UT_GAIN_LCW7_Width = 170;
        int _UT_GAIN_LCW8_Width = 170;
        int _UT_GAIN_LCW9_Width = 170;
        int _UT_GAIN_LCW10_Width = 180;
        int _UT_GAIN_LCCW1_Width = 180;
        int _UT_GAIN_LCCW2_Width = 180;
        int _UT_GAIN_LCCW3_Width = 180;
        int _UT_GAIN_LCCW4_Width = 180;
        int _UT_GAIN_LCCW5_Width = 180;
        int _UT_GAIN_LCCW6_Width = 180;
        int _UT_GAIN_LCCW7_Width = 180;
        int _UT_GAIN_LCCW8_Width = 180;
        int _UT_GAIN_LCCW9_Width = 180;
        int _UT_GAIN_LCWC10_Width = 190;
        int _UT_GAIN_D1_Width = 160;
        int _UT_GAIN_D2_Width = 160;
        int _UT_GAIN_D3_Width = 160;
        int _UT_GAIN_D4_Width = 160;
        int _UT_GAIN_D5_Width = 160;
        int _UT_GAIN_D6_Width = 160;
        int _UT_GAIN_D7_Width = 160;
        int _UT_GAIN_D8_Width = 160;
        int _UT_GAIN_D9_Width = 160;
        int _UT_GAIN_D10_Width = 170;
        int _UT_FAULT_LEVEL_Width = 110;
        int _UT_SENS_CORR_Width = 110;
        int _UT_BEF_DEAD_ZONE_Width = 120;
        int _UT_AFT_DEAD_ZONE_Width = 120;
        int _UT_INJ_METHOD_Width = 120;
        int _UT_PROBE_FOCAL_Width = 150;
        int _SAMPLE_NO_Width = 100;
        int _ARTI_FAULT_Width = 100;
        int _MLFT_FAULT_SIZE_Width = 120;
        int _MLFT_FAULT_DEPTH_Width = 120;
        int _MLFT_FAULT_LENGTH_Width = 120;
        int _UT_VER_FAULT_SIZE_Width = 130;
        int _UT_VER_FAULT_DEPTH_Width = 130;
        int _UT_VER_FAULT_LENGTH_Width = 130;
        int _UT_DUT_FAULT_SIZE_Width = 130;
        int _UT_DUT_FAULT_DEPTH_Width = 130;
        int _UT_DUT_FAULT_LENGTH_Width = 130;
        int _INSP_SPEED_Width = 100;



        public int MAT_FREQUENCY_Width { get { return _MAT_FREQUENCY_Width; } }
        public int MAT_CURRENTMAX_Width { get { return _MAT_CURRENTMAX_Width; } }
        public int MAT_ATTENUATION_Width { get { return _MAT_ATTENUATION_Width; } }
        public int MAT_SENSITIVITY_Width { get { return _MAT_SENSITIVITY_Width; } }
        public int MAT_HARMONIC_Width { get { return _MAT_HARMONIC_Width; } }
        public int MLFT_RPM_Width { get { return _MLFT_RPM_Width; } }
        public int MLFT_BEF_DEAD_ZONE_Width { get { return _MLFT_BEF_DEAD_ZONE_Width; } }
        public int MLFT_AFT_DEAD_ZONE_Width { get { return _MLFT_AFT_DEAD_ZONE_Width; } }
        public int MLFT_FAULT_LEVEL_F1_Width { get { return _MLFT_FAULT_LEVEL_F1_Width; } }
        public int MLFT_FAULT_LEVEL_F2_Width { get { return _MLFT_FAULT_LEVEL_F2_Width; } }
        public int MLFT_FAULT_LEVEL_F3_Width { get { return _MLFT_FAULT_LEVEL_F3_Width; } }
        public int MLFT_FAULT_ADJDT_STD_Width { get { return _MLFT_FAULT_ADJDT_STD_Width; } }
        public int MLFT_SENS_CORR_Width { get { return _MLFT_SENS_CORR_Width; } }
        public int MLFT_YOKE_FREQUENCY_Width { get { return _MLFT_YOKE_FREQUENCY_Width; } }
        public int MLFT_SCAN_SENS_Width { get { return _MLFT_SCAN_SENS_Width; } }
        public int MLFT_PHASE_Width { get { return _MLFT_PHASE_Width; } }
        public int MLFT_Y_GAIN_Width { get { return _MLFT_Y_GAIN_Width; } }
        public int MLFT_FILTER_MODE_Width { get { return _MLFT_FILTER_MODE_Width; } }
        public int MLFT_LP_FILTER_Width { get { return _MLFT_LP_FILTER_Width; } }
        public int MLFT_HP_FILTER_Width { get { return _MLFT_HP_FILTER_Width; } }
        public int MLFT_FILTER_CORR_Width { get { return _MLFT_FILTER_CORR_Width; } }
        public int MLFT_P_SFS_LENGTH_F1_Width { get { return _MLFT_P_SFS_LENGTH_F1_Width; } }
        public int MLFT_P_SFS_LENGTH_F2_Width { get { return _MLFT_P_SFS_LENGTH_F2_Width; } }
        public int MLFT_P_SFS_LENGTH_F3_Width { get { return _MLFT_P_SFS_LENGTH_F3_Width; } }
        public int MLFT_GAIN_CORRECTION_1_Width { get { return _MLFT_GAIN_CORRECTION_1_Width; } }
        public int MLFT_GAIN_CORRECTION_2_Width { get { return _MLFT_GAIN_CORRECTION_2_Width; } }
        public int MLFT_GAIN_CORRECTION_3_Width { get { return _MLFT_GAIN_CORRECTION_3_Width; } }
        public int MLFT_GAIN_CORRECTION_4_Width { get { return _MLFT_GAIN_CORRECTION_4_Width; } }
        public int MLFT_GAIN_CORRECTION_5_Width { get { return _MLFT_GAIN_CORRECTION_5_Width; } }
        public int MLFT_GAIN_CORRECTION_6_Width { get { return _MLFT_GAIN_CORRECTION_6_Width; } }
        public int MLFT_GAIN_CORRECTION_7_Width { get { return _MLFT_GAIN_CORRECTION_7_Width; } }
        public int MLFT_GAIN_CORRECTION_8_Width { get { return _MLFT_GAIN_CORRECTION_8_Width; } }
        public int MLFT_GAIN_CORRECTION_9_Width { get { return _MLFT_GAIN_CORRECTION_9_Width; } }
        public int MLFT_GAIN_CORRECTION_10_Width { get { return _MLFT_GAIN_CORRECTION_10_Width; } }
        public int MLFT_GAIN_CORRECTION_11_Width { get { return _MLFT_GAIN_CORRECTION_11_Width; } }
        public int MLFT_GAIN_CORRECTION_12_Width { get { return _MLFT_GAIN_CORRECTION_12_Width; } }
        public int UT_VELOCITY_L_Width { get { return _UT_VELOCITY_L_Width; } }
        public int UT_VELOCITY_D_Width { get { return _UT_VELOCITY_D_Width; } }
        public int UT_PRF_Width { get { return _UT_PRF_Width; } }
        public int UT_PROBE_FREQUENCY_Width { get { return _UT_PROBE_FREQUENCY_Width; } }
        public int UT_PROBE_RADIUS_Width { get { return _UT_PROBE_RADIUS_Width; } }
        public int UT_PROBE_PITCH_Width { get { return _UT_PROBE_PITCH_Width; } }
        public int UT_VER_ANGLE_Width { get { return _UT_VER_ANGLE_Width; } }
        public int UT_DUT_ANGLE_Width { get { return _UT_DUT_ANGLE_Width; } }
        public int UT_GAIN_LCW1_Width { get { return _UT_GAIN_LCW1_Width; } }
        public int UT_GAIN_LCW2_Width { get { return _UT_GAIN_LCW2_Width; } }
        public int UT_GAIN_LCW3_Width { get { return _UT_GAIN_LCW3_Width; } }
        public int UT_GAIN_LCW4_Width { get { return _UT_GAIN_LCW4_Width; } }
        public int UT_GAIN_LCW5_Width { get { return _UT_GAIN_LCW5_Width; } }
        public int UT_GAIN_LCW6_Width { get { return _UT_GAIN_LCW6_Width; } }
        public int UT_GAIN_LCW7_Width { get { return _UT_GAIN_LCW7_Width; } }
        public int UT_GAIN_LCW8_Width { get { return _UT_GAIN_LCW8_Width; } }
        public int UT_GAIN_LCW9_Width { get { return _UT_GAIN_LCW9_Width; } }
        public int UT_GAIN_LCW10_Width { get { return _UT_GAIN_LCW10_Width; } }
        public int UT_GAIN_LCCW1_Width { get { return _UT_GAIN_LCCW1_Width; } }
        public int UT_GAIN_LCCW2_Width { get { return _UT_GAIN_LCCW2_Width; } }
        public int UT_GAIN_LCCW3_Width { get { return _UT_GAIN_LCCW3_Width; } }
        public int UT_GAIN_LCCW4_Width { get { return _UT_GAIN_LCCW4_Width; } }
        public int UT_GAIN_LCCW5_Width { get { return _UT_GAIN_LCCW5_Width; } }
        public int UT_GAIN_LCCW6_Width { get { return _UT_GAIN_LCCW6_Width; } }
        public int UT_GAIN_LCCW7_Width { get { return _UT_GAIN_LCCW7_Width; } }
        public int UT_GAIN_LCCW8_Width { get { return _UT_GAIN_LCCW8_Width; } }
        public int UT_GAIN_LCCW9_Width { get { return _UT_GAIN_LCCW9_Width; } }
        public int UT_GAIN_LCWC10_Width { get { return _UT_GAIN_LCWC10_Width; } }
        public int UT_GAIN_D1_Width { get { return _UT_GAIN_D1_Width; } }
        public int UT_GAIN_D2_Width { get { return _UT_GAIN_D2_Width; } }
        public int UT_GAIN_D3_Width { get { return _UT_GAIN_D3_Width; } }
        public int UT_GAIN_D4_Width { get { return _UT_GAIN_D4_Width; } }
        public int UT_GAIN_D5_Width { get { return _UT_GAIN_D5_Width; } }
        public int UT_GAIN_D6_Width { get { return _UT_GAIN_D6_Width; } }
        public int UT_GAIN_D7_Width { get { return _UT_GAIN_D7_Width; } }
        public int UT_GAIN_D8_Width { get { return _UT_GAIN_D8_Width; } }
        public int UT_GAIN_D9_Width { get { return _UT_GAIN_D9_Width; } }
        public int UT_GAIN_D10_Width { get { return _UT_GAIN_D10_Width; } }
        public int UT_FAULT_LEVEL_Width { get { return _UT_FAULT_LEVEL_Width; } }
        public int UT_SENS_CORR_Width { get { return _UT_SENS_CORR_Width; } }
        public int UT_BEF_DEAD_ZONE_Width { get { return _UT_BEF_DEAD_ZONE_Width; } }
        public int UT_AFT_DEAD_ZONE_Width { get { return _UT_AFT_DEAD_ZONE_Width; } }
        public int UT_INJ_METHOD_Width { get { return _UT_INJ_METHOD_Width; } }
        public int UT_PROBE_FOCAL_Width { get { return _UT_PROBE_FOCAL_Width; } }
        public int SAMPLE_NO_Width { get { return _SAMPLE_NO_Width; } }
        public int ARTI_FAULT_Width { get { return _ARTI_FAULT_Width; } }
        public int MLFT_FAULT_SIZE_Width { get { return _MLFT_FAULT_SIZE_Width; } }
        public int MLFT_FAULT_DEPTH_Width { get { return _MLFT_FAULT_DEPTH_Width; } }
        public int MLFT_FAULT_LENGTH_Width { get { return _MLFT_FAULT_LENGTH_Width; } }
        public int UT_VER_FAULT_SIZE_Width { get { return _UT_VER_FAULT_SIZE_Width; } }
        public int UT_VER_FAULT_DEPTH_Width { get { return _UT_VER_FAULT_DEPTH_Width; } }
        public int UT_VER_FAULT_LENGTH_Width { get { return _UT_VER_FAULT_LENGTH_Width; } }
        public int UT_DUT_FAULT_SIZE_Width { get { return _UT_DUT_FAULT_SIZE_Width; } }
        public int UT_DUT_FAULT_DEPTH_Width { get { return _UT_DUT_FAULT_DEPTH_Width; } }
        public int UT_DUT_FAULT_LENGTH_Width { get { return _UT_DUT_FAULT_LENGTH_Width; } }
        public int INSP_SPEED_Width { get { return _INSP_SPEED_Width; } }


        #endregion
        public int MAT_GOOD_NG_Width { get { return _MAT_GOOD_NG_Width; } }
        public int MLFT_GOOD_NG_Width { get { return _MLFT_GOOD_NG_Width; } }
        public int UT_GOOD_NG_Width { get { return _UT_GOOD_NG_Width; } }


        public int REWORK_RSN_NM_Width { get { return _REWORK_RSN_NM_Width; } }
        public int INSP_STAT_NM_Width { get { return _INSP_STAT_NM_Width; } }
        public int WORK_UOM_Width { get { return _WORK_UOM_Width; } }
        public int LINE_GP_Width { get { return _LINE_GP_Width; } }
        public int MFG_METHOD_NM_Width { get { return _MFG_METHOD_NM_Width; } }

        public int SEND_YN_Width { get { return _SEND_YN_Width; } }
        

        public int MEMO_Width { get { return _MEMO_Width; } }
        public int ALLOW_FAULT_DEPTH_Width { get { return _ALLOW_FAULT_DEPTH_Width; } }
        public int PACKING_METHOD_NM_Width { get { return _PACKING_METHOD_NM_Width; } }
        public int DEVIATION_Width { get { return _DEVIATION_Width; } }
        public int SPEC_NO_Width { get { return _SPEC_NO_Width; } }

        public int MARKET_GP_NM_Width { get { return _MARKET_GP_NM_Width; } }

        public int FACTORY_Width { get { return _FACTORY_Width; } }

        public int BND_POINT_Width { get { return _BND_POINT_Width; } }
        public int BND_PCS_Width { get { return _BND_PCS_Width; } }
        public int PCS_Width { get { return _PCS_Width; } }
        public int PCS_L_Width { get { return _PCS_L_Width; } }
        
        public int FINISH_PCS_Width { get { return _FINISH_PCS_Width; } }

        public int FINISH_WGT_Width { get { return _FINISH_WGT_Width; } }
        public int FINISH_YN_Width { get { return _FINISH_YN_Width; } }
        public int CRTIN_YN_Width { get { return _CRTIN_YN_Width; } }
        public int L_No_Width { get { return _L_No_Width; } }
        public int Head_Height { get { return _Head_Height; } }

        public int Checker_Width { get { return _Checker_Width; } }
        public int Sel_Width { get { return _Sel_Width; } }
        public int Mill_No_Width { get { return _Mill_No_Width; } }
        public int PIECE_NO_Width { get { return _PIECE_NO_Width; } }
        public int CHM_SNO_Width { get { return _CHM_SNO_Width; } }
        public int LOC_Width { get { return _LOC_Width; } }
        public int INSP_MODE_NM_Width { get { return _INSP_MODE_NM_Width; } }
        public int INSP_GP_NM_Width { get { return _INSP_GP_NM_Width; } }

        public int ZONE_CD_Width { get { return _ZONE_CD_Width; } }
        public int POC_NO_Width { get { return _POC_NO_Width; } }
        public int POC_END_YN_Width { get { return _POC_END_YN_Width; } }
        public int POC_SEQ_Width { get { return _POC_SEQ_Width; } }
        
        public int HEAT_Width { get { return _HEAT_Width; } }
        public int HEAT_YN_Width { get { return _HEAT_YN_Width; } }

        public int PITCH_Width { get { return _PITCH_Width; } }
        

        public int STEEL_Width { get { return _STEEL_Width; } }
        public int STEEL_NM_Width { get { return _STEEL_NM_Width; } }
        public int STEEL_NM_L_Width { get { return _STEEL_NM_L_Width; } }
        

        public int ITEM_Width { get { return _ITEM_Width; } }
        public int ITEM_SIZE_Width { get { return _ITEM_SIZE_Width; } }
        public int Item_L_Width { get { return _Item_L_Width; } }
        
        public int ENTRY_DDTT_Width { get { return _ENTRY_DDTT_Width; } }
        public int REWORK_YN_Width { get { return _REWORK_YN_Width; } }

        public int REWORK_SEQ_Width { get { return _REWORK_SEQ_Width; } }
        
        public int CATEGORY_Width { get { return _CATEGORY_Width; } }
        public int L3_Grp_cd_Width { get { return _L3_Grp_cd_Width; } }
        public int LENGTH_Width { get { return _LENGTH_Width; } }

        public int LENGTH_L_Width { get { return _LENGTH_L_Width; } }
        
        public int TRANS_ROLLER_RPM_Width { get { return _TRANS_ROLLER_RPM_Width; } }
        public int IMPELLER_FREQUENCY_Width { get { return _IMPELLER_FREQUENCY_Width; } }
        

        public int LENGTH_MIN_Width { get { return _LENGTH_MIN_Width; } }
        public int LENGTH_MAX_Width { get { return _LENGTH_MAX_Width; } }
        public int UOM_NM_Width { get { return _UOM_NM_Width; } }
        
        public int WORK_TYPE_NM_Width { get { return _WORK_TYPE_NM_Width; } }

        public int WORK_TEAM_NM_Width { get { return _WORK_TEAM_NM_Width; } }

        
        public int MFG_Date_Width { get { return _MFG_Date_Width; } }
        public int Date_8_Width { get { return _Date_8_Width; } }
        public int TIME_8_Width { get { return _TIME_8_Width; } }
        

        public int Date_14_width { get { return _Date_14_width; } }
        public int ROUTING_NM_With { get { return _ROUTING_NM_Width; } }
        public int POST_PROC_ROUTING_NM_Width { get { return _POST_PROC_ROUTING_NM_Width; } }
        

        public int MPI_FAULT_CD_Width { get { return _MPI_FAULT_CD_Width; } }
        public int MPI_FAULT_NM_Width { get { return _MPI_FAULT_NM_Width; } }
        
        public int Chm_value_Width { get { return _Chm_value_Width; } }
        public int Good_NG_Width { get { return _Good_NG_Width; } }
        public int Good_NG_L_Width { get { return _Good_NG_L_Width; } }
        public int Good_NG_S_Width { get { return _Good_NG_S_Width; } }
        
        public int Angle_Value_Width { get { return _Angle_Value_Width; } }
        public int Stopper_Value_Width { get { return _Stopper_Value_Width; } }
        public int Guide_Value_Width { get { return _Guide_Value_Width; } }
        
        public int Servo_Value_Width { get { return _Servo_Value_Width; } }
        
        public int Speed_Value_Width { get { return _Speed_Value_Width; } }
        public int Current_Value_Width { get { return _Current_Value_Width; } }
        
        public int Work_Rank_Width { get { return _Work_Rank_Width; } }
        public int Surface_Level_NM_Width { get { return _Surface_Level_NM_Width; } }
        public int Wgt_Width { get { return _Wgt_Width; } }
        public int NET_WGT_Width { get { return _NET_WGT_Width; } }
        
        public int Company_NM_Width { get { return _Company_NM_Width; } }
        public int Usage_CD_NM_Width { get { return _Usage_CD_NM_Width; } }
        public int POC_PROG_STAT_NM_Width { get { return _POC_PROG_STAT_NM_Width; } }

        public int BUNDLE_QTY_Width { get { return _BUNDLE_QTY_Width; } }
        public int BUNDLE_NO_Width { get { return _BUNDLE_NO_Width; } }
        public int PROG_STAT_Width { get { return _PROG_STAT_Width; } }
        
        //압연본수
        public int MILL_PCS_Width { get { return _MILL_PCS_Width; } }
        //압연중량
        public int MILL_WGT_Width { get { return _MILL_WGT_Width; } }
        //교정본수
        public int STR_PCS_Width { get { return _STR_PCS_Width; } }
        //면취본수
        public int SHF_PCS_Width { get { return _SHF_PCS_Width; } }
        //면취중량
        public int SHF_WGT_Width { get { return _SHF_WGT_Width; } }
        #endregion

        #region 컬럼 텍스트 align
        TextAlignEnum _L_NO_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _PIECE_NO_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MILL_NO_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _BUNDLE_NO_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _POC_NO_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _POC_SEQ_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _PROG_STAT_TextAlign = TextAlignEnum.CenterCenter;
        

        TextAlignEnum _HEAT_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _FACTORY_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _STEEL_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _ITEM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _ITEM_SIZE_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _LENGTH_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _UOM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _BUNDLE_QTY_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _WGT_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _BND_POINT_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _REMARKS_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _PACKING_RE_QTY_TextAlign = TextAlignEnum.RightCenter;
        


        TextAlignEnum _DATE_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _CRTIN_YN_TextAlign = TextAlignEnum.CenterCenter;
        
        TextAlignEnum _HEAT_YN_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _PITCH_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _COMPANY_CD_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _COMPANY_NM_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _SURFACE_LEVEL_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _SURFACE_LEVEL_NM_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _POST_PROC_ROUTING_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _POST_PROC_ROUTING_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _ROUTING_NM_TextAlign = TextAlignEnum.CenterCenter;
        
        TextAlignEnum _SPEC_NO_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MARKET_GP_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _SIZE_MIN_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _SIZE_MAX_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _LENGTH_MIN_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _LENGTH_MAX_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _DEVIATION_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _PACKING_METHOD_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _PACKING_METHOD_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _ALLOW_FAULT_DEPTH_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MEMO_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _USAGE_CD_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MFG_METHOD_TextAlign = TextAlignEnum.CenterCenter;
        //TextAlignEnum _CR_ORD_DATE_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _LINE_GP_TextAlign = TextAlignEnum.CenterCenter;
        //TextAlignEnum _WORK_ORD_DATE_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _WORK_RANK_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _WORK_TYPE_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _WORK_TEAM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _WORK_TEAM_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _WORK_TYPE_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _WORK_CNTS_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _TOTAL_TextAlign = TextAlignEnum.RightCenter;
        


        TextAlignEnum _WORK_UOM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _FINISH_PCS_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _PCS_TextAlign = TextAlignEnum.RightCenter;

        TextAlignEnum _FINISH_YN_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _FINISH_WGT_TextAlign = TextAlignEnum.CenterCenter;
        //TextAlignEnum _FINISH_DATE_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _POC_PROG_STAT_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _POC_PROG_STAT_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _REGISTER_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _REG_DDTT_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MODIFIER_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MOD_DDTT_TextAlign = TextAlignEnum.CenterCenter;

        TextAlignEnum _SEL_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _STEEL_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _ZONE_CD_TextAlign = TextAlignEnum.CenterCenter;


        TextAlignEnum _Angle_Value_TextAlign = TextAlignEnum.RightCenter;

        TextAlignEnum _Speed_Value_TextAlign = TextAlignEnum.RightCenter;

        TextAlignEnum _Current_Value_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _Item_List_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _GOOD_YN_TextAlign = TextAlignEnum.CenterCenter;

        TextAlignEnum _IF_YN_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _GOOD_NG_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _REWORK_YN_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _REWORK_SEQ_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MPI_FAULT_CD_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _MPI_FAULT_NM_TextAlign = TextAlignEnum.LeftCenter;

        //교정중량
        TextAlignEnum _STR_WGT_TextAlign = TextAlignEnum.RightCenter;
        //면취본수
        TextAlignEnum _SHF_PCS_TextAlign = TextAlignEnum.RightCenter;
        //면취중량
        TextAlignEnum _SHF_WGT_TextAlign = TextAlignEnum.RightCenter;
        //압연본수
        TextAlignEnum _MILL_PCS_TextAlign = TextAlignEnum.RightCenter;
        //압연중량
        TextAlignEnum _MILL_WGT_TextAlign = TextAlignEnum.RightCenter;
        //교정본수
        TextAlignEnum _STR_PCS_TextAlign = TextAlignEnum.RightCenter;
        //Work_Time
        TextAlignEnum _WORK_TIME_TextAlign = TextAlignEnum.CenterCenter;
        //교정 오른쪽 정렬 값들 정렬
        TextAlignEnum _VALUE_RIGHT_TextAlign = TextAlignEnum.RightCenter;
        //교정 오른쪽 정렬 값들 정렬
        TextAlignEnum _VALUE_LEFT_TextAlign = TextAlignEnum.LeftCenter;

        TextAlignEnum _SEND_YN_TextAlign = TextAlignEnum.CenterCenter;

        #region NDT 실적조회 TextAlign
        TextAlignEnum _MAT_FREQUENCY_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MAT_CURRENTMAX_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MAT_ATTENUATION_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MAT_SENSITIVITY_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MAT_HARMONIC_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _MLFT_RPM_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_BEF_DEAD_ZONE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_AFT_DEAD_ZONE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FAULT_LEVEL_F1_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FAULT_LEVEL_F2_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FAULT_LEVEL_F3_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FAULT_ADJDT_STD_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _MLFT_SENS_CORR_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_YOKE_FREQUENCY_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_SCAN_SENS_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_PHASE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_Y_GAIN_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FILTER_MODE_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _MLFT_LP_FILTER_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_HP_FILTER_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FILTER_CORR_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_P_SFS_LENGTH_F1_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_P_SFS_LENGTH_F2_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_P_SFS_LENGTH_F3_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_1_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_2_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_3_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_4_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_5_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_6_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_7_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_8_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_9_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_10_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_11_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_GAIN_CORRECTION_12_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_VELOCITY_L_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_VELOCITY_D_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_PRF_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_PROBE_FREQUENCY_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_PROBE_RADIUS_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_PROBE_PITCH_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_VER_ANGLE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_DUT_ANGLE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW1_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW2_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW3_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW4_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW5_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW6_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW7_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW8_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW9_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCW10_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW1_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW2_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW3_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW4_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW5_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW6_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW7_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW8_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCCW9_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_LCWC10_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D1_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D2_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D3_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D4_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D5_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D6_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D7_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D8_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D9_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_GAIN_D10_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_FAULT_LEVEL_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_SENS_CORR_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_BEF_DEAD_ZONE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_AFT_DEAD_ZONE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_INJ_METHOD_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _UT_PROBE_FOCAL_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _SAMPLE_NO_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _ARTI_FAULT_TextAlign = TextAlignEnum.LeftCenter;
        TextAlignEnum _MLFT_FAULT_SIZE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FAULT_DEPTH_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _MLFT_FAULT_LENGTH_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_VER_FAULT_SIZE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_VER_FAULT_DEPTH_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_VER_FAULT_LENGTH_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_DUT_FAULT_SIZE_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_DUT_FAULT_DEPTH_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _UT_DUT_FAULT_LENGTH_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _INSP_SPEED_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _POC_END_YN_TextAlign = TextAlignEnum.RightCenter;
        TextAlignEnum _INSP_STAT_NM_TextAlign = TextAlignEnum.CenterCenter;
        TextAlignEnum _REWORK_RSN_NM_TextAlign = TextAlignEnum.CenterCenter;

        public TextAlignEnum REWORK_RSN_NM_TextAlign { get { return _REWORK_RSN_NM_TextAlign; } }
        public TextAlignEnum INSP_STAT_NM_TextAlign { get { return _INSP_STAT_NM_TextAlign; } }
        public TextAlignEnum POC_END_YN_TextAlign { get { return _POC_END_YN_TextAlign; } }
        //int _REWORK_YN_Width = 80;
        public TextAlignEnum MAT_FREQUENCY_TextAlign { get { return _MAT_FREQUENCY_TextAlign; } }
        public TextAlignEnum MAT_CURRENTMAX_TextAlign { get { return _MAT_CURRENTMAX_TextAlign; } }
        public TextAlignEnum MAT_ATTENUATION_TextAlign { get { return _MAT_ATTENUATION_TextAlign; } }
        public TextAlignEnum MAT_SENSITIVITY_TextAlign { get { return _MAT_SENSITIVITY_TextAlign; } }
        public TextAlignEnum MAT_HARMONIC_TextAlign { get { return _MAT_HARMONIC_TextAlign; } }
        public TextAlignEnum MLFT_RPM_TextAlign { get { return _MLFT_RPM_TextAlign; } }
        public TextAlignEnum MLFT_BEF_DEAD_ZONE_TextAlign { get { return _MLFT_BEF_DEAD_ZONE_TextAlign; } }
        public TextAlignEnum MLFT_AFT_DEAD_ZONE_TextAlign { get { return _MLFT_AFT_DEAD_ZONE_TextAlign; } }
        public TextAlignEnum MLFT_FAULT_LEVEL_F1_TextAlign { get { return _MLFT_FAULT_LEVEL_F1_TextAlign; } }
        public TextAlignEnum MLFT_FAULT_LEVEL_F2_TextAlign { get { return _MLFT_FAULT_LEVEL_F2_TextAlign; } }
        public TextAlignEnum MLFT_FAULT_LEVEL_F3_TextAlign { get { return _MLFT_FAULT_LEVEL_F3_TextAlign; } }
        public TextAlignEnum MLFT_FAULT_ADJDT_STD_TextAlign { get { return _MLFT_FAULT_ADJDT_STD_TextAlign; } }
        public TextAlignEnum MLFT_SENS_CORR_TextAlign { get { return _MLFT_SENS_CORR_TextAlign; } }
        public TextAlignEnum MLFT_YOKE_FREQUENCY_TextAlign { get { return _MLFT_YOKE_FREQUENCY_TextAlign; } }
        public TextAlignEnum MLFT_SCAN_SENS_TextAlign { get { return _MLFT_SCAN_SENS_TextAlign; } }
        public TextAlignEnum MLFT_PHASE_TextAlign { get { return _MLFT_PHASE_TextAlign; } }
        public TextAlignEnum MLFT_Y_GAIN_TextAlign { get { return _MLFT_Y_GAIN_TextAlign; } }
        public TextAlignEnum MLFT_FILTER_MODE_TextAlign { get { return _MLFT_FILTER_MODE_TextAlign; } }
        public TextAlignEnum MLFT_LP_FILTER_TextAlign { get { return _MLFT_LP_FILTER_TextAlign; } }
        public TextAlignEnum MLFT_HP_FILTER_TextAlign { get { return _MLFT_HP_FILTER_TextAlign; } }
        public TextAlignEnum MLFT_FILTER_CORR_TextAlign { get { return _MLFT_FILTER_CORR_TextAlign; } }
        public TextAlignEnum MLFT_P_SFS_LENGTH_F1_TextAlign { get { return _MLFT_P_SFS_LENGTH_F1_TextAlign; } }
        public TextAlignEnum MLFT_P_SFS_LENGTH_F2_TextAlign { get { return _MLFT_P_SFS_LENGTH_F2_TextAlign; } }
        public TextAlignEnum MLFT_P_SFS_LENGTH_F3_TextAlign { get { return _MLFT_P_SFS_LENGTH_F3_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_1_TextAlign { get { return _MLFT_GAIN_CORRECTION_1_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_2_TextAlign { get { return _MLFT_GAIN_CORRECTION_2_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_3_TextAlign { get { return _MLFT_GAIN_CORRECTION_3_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_4_TextAlign { get { return _MLFT_GAIN_CORRECTION_4_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_5_TextAlign { get { return _MLFT_GAIN_CORRECTION_5_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_6_TextAlign { get { return _MLFT_GAIN_CORRECTION_6_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_7_TextAlign { get { return _MLFT_GAIN_CORRECTION_7_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_8_TextAlign { get { return _MLFT_GAIN_CORRECTION_8_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_9_TextAlign { get { return _MLFT_GAIN_CORRECTION_9_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_10_TextAlign { get { return _MLFT_GAIN_CORRECTION_10_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_11_TextAlign { get { return _MLFT_GAIN_CORRECTION_11_TextAlign; } }
        public TextAlignEnum MLFT_GAIN_CORRECTION_12_TextAlign { get { return _MLFT_GAIN_CORRECTION_12_TextAlign; } }
        public TextAlignEnum UT_VELOCITY_L_TextAlign { get { return _UT_VELOCITY_L_TextAlign; } }
        public TextAlignEnum UT_VELOCITY_D_TextAlign { get { return _UT_VELOCITY_D_TextAlign; } }
        public TextAlignEnum UT_PRF_TextAlign { get { return _UT_PRF_TextAlign; } }
        public TextAlignEnum UT_PROBE_FREQUENCY_TextAlign { get { return _UT_PROBE_FREQUENCY_TextAlign; } }
        public TextAlignEnum UT_PROBE_RADIUS_TextAlign { get { return _UT_PROBE_RADIUS_TextAlign; } }
        public TextAlignEnum UT_PROBE_PITCH_TextAlign { get { return _UT_PROBE_PITCH_TextAlign; } }
        public TextAlignEnum UT_VER_ANGLE_TextAlign { get { return _UT_VER_ANGLE_TextAlign; } }
        public TextAlignEnum UT_DUT_ANGLE_TextAlign { get { return _UT_DUT_ANGLE_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW1_TextAlign { get { return _UT_GAIN_LCW1_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW2_TextAlign { get { return _UT_GAIN_LCW2_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW3_TextAlign { get { return _UT_GAIN_LCW3_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW4_TextAlign { get { return _UT_GAIN_LCW4_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW5_TextAlign { get { return _UT_GAIN_LCW5_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW6_TextAlign { get { return _UT_GAIN_LCW6_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW7_TextAlign { get { return _UT_GAIN_LCW7_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW8_TextAlign { get { return _UT_GAIN_LCW8_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW9_TextAlign { get { return _UT_GAIN_LCW9_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCW10_TextAlign { get { return _UT_GAIN_LCW10_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW1_TextAlign { get { return _UT_GAIN_LCCW1_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW2_TextAlign { get { return _UT_GAIN_LCCW2_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW3_TextAlign { get { return _UT_GAIN_LCCW3_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW4_TextAlign { get { return _UT_GAIN_LCCW4_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW5_TextAlign { get { return _UT_GAIN_LCCW5_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW6_TextAlign { get { return _UT_GAIN_LCCW6_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW7_TextAlign { get { return _UT_GAIN_LCCW7_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW8_TextAlign { get { return _UT_GAIN_LCCW8_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCCW9_TextAlign { get { return _UT_GAIN_LCCW9_TextAlign; } }
        public TextAlignEnum UT_GAIN_LCWC10_TextAlign { get { return _UT_GAIN_LCWC10_TextAlign; } }
        public TextAlignEnum UT_GAIN_D1_TextAlign { get { return _UT_GAIN_D1_TextAlign; } }
        public TextAlignEnum UT_GAIN_D2_TextAlign { get { return _UT_GAIN_D2_TextAlign; } }
        public TextAlignEnum UT_GAIN_D3_TextAlign { get { return _UT_GAIN_D3_TextAlign; } }
        public TextAlignEnum UT_GAIN_D4_TextAlign { get { return _UT_GAIN_D4_TextAlign; } }
        public TextAlignEnum UT_GAIN_D5_TextAlign { get { return _UT_GAIN_D5_TextAlign; } }
        public TextAlignEnum UT_GAIN_D6_TextAlign { get { return _UT_GAIN_D6_TextAlign; } }
        public TextAlignEnum UT_GAIN_D7_TextAlign { get { return _UT_GAIN_D7_TextAlign; } }
        public TextAlignEnum UT_GAIN_D8_TextAlign { get { return _UT_GAIN_D8_TextAlign; } }
        public TextAlignEnum UT_GAIN_D9_TextAlign { get { return _UT_GAIN_D9_TextAlign; } }
        public TextAlignEnum UT_GAIN_D10_TextAlign { get { return _UT_GAIN_D10_TextAlign; } }
        public TextAlignEnum UT_FAULT_LEVEL_TextAlign { get { return _UT_FAULT_LEVEL_TextAlign; } }
        public TextAlignEnum UT_SENS_CORR_TextAlign { get { return _UT_SENS_CORR_TextAlign; } }
        public TextAlignEnum UT_BEF_DEAD_ZONE_TextAlign { get { return _UT_BEF_DEAD_ZONE_TextAlign; } }
        public TextAlignEnum UT_AFT_DEAD_ZONE_TextAlign { get { return _UT_AFT_DEAD_ZONE_TextAlign; } }
        public TextAlignEnum UT_INJ_METHOD_TextAlign { get { return _UT_INJ_METHOD_TextAlign; } }
        public TextAlignEnum UT_PROBE_FOCAL_TextAlign { get { return _UT_PROBE_FOCAL_TextAlign; } }
        public TextAlignEnum SAMPLE_NO_TextAlign { get { return _SAMPLE_NO_TextAlign; } }
        public TextAlignEnum ARTI_FAULT_TextAlign { get { return _ARTI_FAULT_TextAlign; } }
        public TextAlignEnum MLFT_FAULT_SIZE_TextAlign { get { return _MLFT_FAULT_SIZE_TextAlign; } }
        public TextAlignEnum MLFT_FAULT_DEPTH_TextAlign { get { return _MLFT_FAULT_DEPTH_TextAlign; } }
        public TextAlignEnum MLFT_FAULT_LENGTH_TextAlign { get { return _MLFT_FAULT_LENGTH_TextAlign; } }
        public TextAlignEnum UT_VER_FAULT_SIZE_TextAlign { get { return _UT_VER_FAULT_SIZE_TextAlign; } }
        public TextAlignEnum UT_VER_FAULT_DEPTH_TextAlign { get { return _UT_VER_FAULT_DEPTH_TextAlign; } }
        public TextAlignEnum UT_VER_FAULT_LENGTH_TextAlign { get { return _UT_VER_FAULT_LENGTH_TextAlign; } }
        public TextAlignEnum UT_DUT_FAULT_SIZE_TextAlign { get { return _UT_DUT_FAULT_SIZE_TextAlign; } }
        public TextAlignEnum UT_DUT_FAULT_DEPTH_TextAlign { get { return _UT_DUT_FAULT_DEPTH_TextAlign; } }
        public TextAlignEnum UT_DUT_FAULT_LENGTH_TextAlign { get { return _UT_DUT_FAULT_LENGTH_TextAlign; } }
        public TextAlignEnum INSP_SPEED_TextAlign { get { return _INSP_SPEED_TextAlign; } }
        #endregion









        public TextAlignEnum MPI_FAULT_NM_TextAlign { get { return _MPI_FAULT_NM_TextAlign; } }
        public TextAlignEnum MPI_FAULT_CD_TextAlign { get { return _MPI_FAULT_CD_TextAlign; } }
        public TextAlignEnum ROUTING_NM_TextAlign { get { return _ROUTING_NM_TextAlign; } }
        public TextAlignEnum REWORK_SEQ_TextAlign { get { return _REWORK_SEQ_TextAlign; } }
        public TextAlignEnum REWORK_YN_TextAlign { get { return _REWORK_YN_TextAlign; } }
        public TextAlignEnum BUNDLE_NO_TextAlign { get { return _BUNDLE_NO_TextAlign; } }
        public TextAlignEnum Item_List_TextAlign { get { return _Item_List_TextAlign; } }
        
        public TextAlignEnum Angle_Value_TextAlign { get { return _Angle_Value_TextAlign; } }
        public TextAlignEnum Speed_Value_TextAlign { get { return _Speed_Value_TextAlign; } }
        public TextAlignEnum Current_Value_TextAlign { get { return _Current_Value_TextAlign; } }
        public TextAlignEnum L_NO_TextAlign { get { return _L_NO_TextAlign; } }
        public TextAlignEnum POC_NO_TextAlign { get { return _POC_NO_TextAlign; } }
        public TextAlignEnum POC_SEQ_TextAlign { get { return _POC_SEQ_TextAlign; } }
        public TextAlignEnum PROG_STAT_TextAlign { get { return _PROG_STAT_TextAlign; } }

        public TextAlignEnum HEAT_TextAlign { get { return _HEAT_TextAlign; } }
        public TextAlignEnum FACTORY_TextAlign { get { return _FACTORY_TextAlign; } }
        public TextAlignEnum STEEL_TextAlign { get { return _STEEL_TextAlign; } }
        public TextAlignEnum STEEL_NM_TextAlign { get { return _STEEL_NM_TextAlign; } }
        public TextAlignEnum ITEM_TextAlign { get { return _ITEM_TextAlign; } }
        public TextAlignEnum ITEM_SIZE_TextAlign { get { return _ITEM_SIZE_TextAlign; } }
        public TextAlignEnum LENGTH_TextAlign { get { return _LENGTH_TextAlign; } }
        public TextAlignEnum UOM_TextAlign { get { return _UOM_TextAlign; } }
        public TextAlignEnum BUNDLE_QTY_TextAlign { get { return _BUNDLE_QTY_TextAlign; } }
        public TextAlignEnum WGT_TextAlign { get { return _WGT_TextAlign; } }
        public TextAlignEnum BND_POINT_TextAlign { get { return _BND_POINT_TextAlign; } }
        public TextAlignEnum REMARKS_TextAlign { get { return _REMARKS_TextAlign; } }
        public TextAlignEnum PACKING_RE_QTY_TextAlign { get { return _PACKING_RE_QTY_TextAlign; } }
        
        public TextAlignEnum DATE_TextAlign { get { return _DATE_TextAlign; } }
        public TextAlignEnum CRTIN_YN_TextAlign { get { return _CRTIN_YN_TextAlign; } }
        
        public TextAlignEnum HEAT_YN_TextAlign { get { return _HEAT_YN_TextAlign; } }
        public TextAlignEnum PITCH_TextAlign { get { return _PITCH_TextAlign; } }
        public TextAlignEnum COMPANY_CD_TextAlign { get { return _COMPANY_CD_TextAlign; } }
        public TextAlignEnum COMPANY_NM_TextAlign { get { return _COMPANY_NM_TextAlign; } }
        public TextAlignEnum SURFACE_LEVEL_TextAlign { get { return _SURFACE_LEVEL_TextAlign; } }
        public TextAlignEnum SURFACE_LEVEL_NM_TextAlign { get { return _SURFACE_LEVEL_NM_TextAlign; } }
        
        public TextAlignEnum POST_PROC_ROUTING_TextAlign { get { return _POST_PROC_ROUTING_TextAlign; } }
        public TextAlignEnum POST_PROC_ROUTING_NM_TextAlign { get { return _POST_PROC_ROUTING_NM_TextAlign; } }
        
        public TextAlignEnum SPEC_NO_TextAlign { get { return _SPEC_NO_TextAlign; } }

        public TextAlignEnum MARKET_GP_TextAlign { get { return _MARKET_GP_TextAlign; } }
        public TextAlignEnum SIZE_MIN_TextAlign { get { return _SIZE_MIN_TextAlign; } }
        public TextAlignEnum SIZE_MAX_TextAlign { get { return _SIZE_MAX_TextAlign; } }
        public TextAlignEnum LENGTH_MIN_TextAlign { get { return _LENGTH_MIN_TextAlign; } }
        public TextAlignEnum LENGTH_MAX_TextAlign { get { return _LENGTH_MAX_TextAlign; } }
        public TextAlignEnum DEVIATION_TextAlign { get { return _DEVIATION_TextAlign; } }
        public TextAlignEnum PACKING_METHOD_TextAlign { get { return _PACKING_METHOD_TextAlign; } }
        public TextAlignEnum PACKING_METHOD_NM_TextAlign { get { return _PACKING_METHOD_NM_TextAlign; } }
        
        public TextAlignEnum ALLOW_FAULT_DEPTH_TextAlign { get { return _ALLOW_FAULT_DEPTH_TextAlign; } }
        public TextAlignEnum MEMO_TextAlign { get { return _MEMO_TextAlign; } }
        public TextAlignEnum USAGE_CD_NM_TextAlign { get { return _USAGE_CD_NM_TextAlign; } }
        public TextAlignEnum MFG_METHOD_TextAlign { get { return _MFG_METHOD_TextAlign; } }
        public TextAlignEnum LINE_GP_TextAlign { get { return _LINE_GP_TextAlign; } }
        public TextAlignEnum WORK_RANK_TextAlign { get { return _WORK_RANK_TextAlign; } }
        public TextAlignEnum WORK_UOM_TextAlign { get { return _WORK_UOM_TextAlign; } }
        public TextAlignEnum FINISH_PCS_TextAlign { get { return _FINISH_PCS_TextAlign; } }
        public TextAlignEnum PCS_TextAlign { get { return _PCS_TextAlign; } }
        
        public TextAlignEnum FINISH_YN_TextAlign { get { return _FINISH_YN_TextAlign; } }
        public TextAlignEnum FINISH_WGT_TextAlign { get { return _FINISH_WGT_TextAlign; } }
        
        public TextAlignEnum POC_PROG_STAT_TextAlign { get { return _POC_PROG_STAT_TextAlign; } }
        public TextAlignEnum POC_PROG_STAT_NM_TextAlign { get { return _POC_PROG_STAT_NM_TextAlign; } }
        
        public TextAlignEnum REGISTER_TextAlign { get { return _REGISTER_TextAlign; } }
        public TextAlignEnum REG_DDTT_TextAlign { get { return _REG_DDTT_TextAlign; } }
        public TextAlignEnum MODIFIER_TextAlign { get { return _MODIFIER_TextAlign; } }
        public TextAlignEnum MOD_DDTT_TextAlign { get { return _MOD_DDTT_TextAlign; } }

        public TextAlignEnum SEL_TextAlign { get { return _SEL_TextAlign; } }
        public TextAlignEnum MILL_NO_TextAlign { get { return _MILL_NO_TextAlign; } }
        public TextAlignEnum PIECE_NO_TextAlign { get { return _PIECE_NO_TextAlign; } }
        public TextAlignEnum WORK_TYPE_NM_TextAlign { get { return _WORK_TYPE_NM_TextAlign; } }
        public TextAlignEnum TOTAL_TextAlign { get { return _TOTAL_TextAlign; } }
        public TextAlignEnum WORK_CNTS_TextAlign { get { return _WORK_CNTS_TextAlign; } }
        public TextAlignEnum WORK_TEAM_TextAlign { get { return _WORK_TEAM_TextAlign; } }
        public TextAlignEnum WORK_TEAM_NM_TextAlign { get { return _WORK_TEAM_NM_TextAlign; } }
        public TextAlignEnum WORK_TYPE_TextAlign { get { return _WORK_TYPE_TextAlign; } }

        
        public TextAlignEnum ZONE_CD_TextAlign { get { return _ZONE_CD_TextAlign; } }
        public TextAlignEnum GOOD_YN_TextAlign { get { return _GOOD_YN_TextAlign; } }
        public TextAlignEnum GOOD_NG_TextAlign { get { return _GOOD_NG_TextAlign; } }
        
        public TextAlignEnum IF_YN_TextAlign { get { return _IF_YN_TextAlign; } }
        //교정중량
        public TextAlignEnum STR_WGT_TextAlign { get { return _STR_WGT_TextAlign; } }
        //면취본수
        public TextAlignEnum SHF_PCS_TextAlign { get { return _SHF_PCS_TextAlign; } }
        //면취중량
        public TextAlignEnum SHF_WGT_TextAlign { get { return _SHF_WGT_TextAlign; } }
        //압연본수
        public TextAlignEnum MILL_PCS_TextAlign { get { return _MILL_PCS_TextAlign; } }
        //압연중량
        public TextAlignEnum MILL_WGT_TextAlign { get { return _MILL_WGT_TextAlign; } }
        //교정본수
        public TextAlignEnum STR_PCS_TextAlign { get { return _STR_PCS_TextAlign; } }
        #endregion
        //WORK_TIME
        public TextAlignEnum WORK_TIME_TextAlign { get { return _WORK_TIME_TextAlign; } }
        //#3라인 값들 오른쪽정렬
        public TextAlignEnum VALUE_RIGHT_TextAlign { get { return _VALUE_RIGHT_TextAlign; } }
        // 값들 왼쪽정렬
        public TextAlignEnum VALUE_LEFT_TextAlign { get { return _VALUE_LEFT_TextAlign; } }
        public TextAlignEnum SEND_YN_TextAlign { get { return _SEND_YN_TextAlign; } }
        
        //public int GetWidth(string colsName)
        //{
        //    int width;



        //    return width;
        //}


        //그리드 폰트 확대 Size
        public float FontSizeBig { get { return fontSizeBig; } }

        public float FontSizeBigger { get { return fontSizeBigger; } }

        public float FontSizeMiddle { get { return fontSizeMiddle; } }

        //그리드 폰트 축소 Size
        public float FontSizeSmall { get { return fontSizeSmall; } }

        //그리드 복원 헤더 폰트 Size
        public float OHeadfontSize { get { return oHeadfontSize; } }

        //그리드 복원 헤더 폰트 Style
        //public int OHeadfontStyle { get { return oHeadfontStyle; } }

        //그리드 복원 헤더 폰트 명
        public string OHeadfontName { get { return oHeadfontName; } }

        public string OHeadfontName1 { get { return oHeadfontName1; } }


        public void InitButton(C1Button btn)
        {
            btn.AllowDrop = false;
            btn.AutoEllipsis = false;
            btn.DialogResult = 0;
            btn.Enabled = true;
            btn.TabStop = true;
            btn.UseCompatibleTextRendering = false;
            btn.Visible = true;

            //mj
            btn.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //btn.BackColor = Color.Red;
            //btn.Margin = new System.Windows.Forms.Padding(1);
            btn.Margin = new System.Windows.Forms.Padding(1,2,1,1);   //아래쪽 마진을 0으로...

            //btn.MouseUp += Btn_MouseUp;

            //btn.FlatStyle = FlatStyle.Flat;
            btn.FlatStyle = FlatStyle.Standard;
            btn.FlatAppearance.BorderColor = Color.FromArgb(255,223, 223, 223);

            return;
        }

        public void InitButton_1(C1Button btn)
        {
            btn.AllowDrop = false;
            btn.AutoEllipsis = false;
            btn.DialogResult = 0;
            btn.Enabled = true;
            btn.TabStop = true;
            btn.UseCompatibleTextRendering = false;
            btn.Visible = true;

            //mj
            btn.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //btn.BackColor = Color.Red;
            //btn.Margin = new System.Windows.Forms.Padding(1);
            btn.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);   //아래쪽 마진을 0으로...

            //btn.MouseUp += Btn_MouseUp;

            //btn.FlatStyle = FlatStyle.Flat;
            btn.FlatStyle = FlatStyle.Standard;
            btn.FlatAppearance.BorderColor = Color.FromArgb(255, 223, 223, 223);

            return;
        }

        private void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            Button btnDisplay = sender as Button;
            string scr_id = string.Empty;

            if (btnDisplay.Name == "btnDisplay")
            {
                //조회를 기록한다.
                //scr_id, 로그인 user_id

                //string page_id = btnDisplay.Parent.Parent.Parent.Name;
                //scr_NM = '화면관리'
                

                if (string.IsNullOrEmpty(cd.Find_Scr_ID_By_PAGE_ID(btnDisplay.Parent.Parent.Parent.Name)))
                {
                    return;
                }
                else
                {
                    scr_id = cd.Find_Scr_ID_By_PAGE_ID(btnDisplay.Parent.Parent.Parent.Name);
                }
                
                string user_id = cS.UserID;
                string btn_gp = "1";

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

                    string sql1 = string.Empty;
                    sql1 += string.Format("INSERT INTO TB_CM_SCR_USEHIS ");
                    sql1 += string.Format("( ");
                    sql1 += string.Format("    USER_ID ");
                    sql1 += string.Format("  , SCR_ID ");
                    sql1 += string.Format("  , USE_DATE_SEQ ");
                    sql1 += string.Format("  ,  USE_DDTT ");
                    sql1 += string.Format("  ,  BTN_GP ");
                    sql1 += string.Format("  , REGISTER ");
                    sql1 += string.Format("  , REG_DDTT ");
                    sql1 += string.Format("  ) ");
                    sql1 += string.Format("VALUES ( ");
                    sql1 += string.Format("    '{0}' ",user_id);
                    sql1 += string.Format("  , '{0}' ", scr_id);
                    sql1 += string.Format(", ( ");
                    sql1 += string.Format("SELECT NVL(MAX(USE_DATE_SEQ),0) + 1 FROM  TB_CM_SCR_USEHIS ");
                    sql1 += string.Format("WHERE  USER_ID = '{0}' ", user_id);
                    sql1 += string.Format("AND    SCR_ID  = '{0}' ", scr_id);
                    sql1 += string.Format("AND    TO_CHAR(USE_DDTT,'YYYYMMDD') = TO_CHAR(SYSDATE,'YYYYMMDD') ");
                    sql1 += string.Format(") ");
                    sql1 += string.Format("  , sysdate ");
                    sql1 += string.Format("  , '{0}' ", btn_gp);
                    sql1 += string.Format("  , '{0}' ", user_id);
                    sql1 += string.Format("  , sysdate ");
                    sql1 += string.Format("  ) ");

                    cmd.CommandText = sql1;
                    cmd.ExecuteNonQuery();

                    //실행후 성공
                    transaction.Commit();

                }
                catch (System.Exception ex)
                {
                    //실행후 실패 : 
                    if (transaction != null)
                    {
                        transaction.Rollback();
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
            return;
        }

        public void InitMsgLabel(Label lb)
        {
            lb.Font = new Font(OHeadfontName, FontSizeBigger, FontStyle.Bold);
        }

        public void InitMsgListBox(ListBox lb)
        {
            lb.Font = new Font(OHeadfontName, FontSizeBigger, FontStyle.Bold);
            //lb.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void InitLabel(Label lb)
        {
            lb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            
        }

        public void InitLabel(Label lb, bool boldOption)
        {
            lb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
        }

        public void InitText(TextBox tb)
        {
            tb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
        }



        public void InitCombo(ComboBox cb)
        {
            cb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

            //cb.DrawItem += Cb_DrawItem_Line;
            ////텍스트를 센터로 보내기위해 제정의 가능하게 함.. drawitem event 추가되어야함.
            //cb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        public void InitCombo_Center(ComboBox cb)
        {
            cb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //cb.DropDownStyle = ComboBoxStyle.DropDownList;

            cb.DrawItem += Cb_DrawItem_Line_Center;
            //텍스트를 센터로 보내기위해 제정의 가능하게 함.. drawitem event 추가되어야함.
            cb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        public void InitCombo(ComboBox cb, StringAlignment option)
        {
            cb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //cb.DropDownStyle = ComboBoxStyle.DropDownList;

            if (option == StringAlignment.Far)
            {
                cb.DrawItem += Cb_DrawItem_Line_Far;
            }
            else if (option == StringAlignment.Near)
            {
                cb.DrawItem += Cb_DrawItem_Line_Near;
            }
            else
            {
                cb.DrawItem += Cb_DrawItem_Line_Center;
            }

            
            //텍스트를 센터로 보내기위해 제정의 가능하게 함.. drawitem event 추가되어야함.
            cb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        public void Cb_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    if (cbx.Items[0].GetType().Name == "DictionaryList")
                    {
                        // Draw the string
                        e.Graphics.DrawString(((ComLib.DictionaryList)cbx.Items[e.Index]).fnText, cbx.Font, brush, e.Bounds, sf);
                    }
                    else
                    {
                        // Draw the string
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }
        public void Cb_DrawItem_Line_Center(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    if (cbx.Items[0].GetType().Name == "DictionaryList")
                    {
                        // Draw the string
                        e.Graphics.DrawString(((ComLib.DictionaryList)cbx.Items[e.Index]).fnText, cbx.Font, brush, e.Bounds, sf);
                    }
                    else
                    {
                        // Draw the string
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }

        public void Cb_DrawItem_Line_Near(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Alignment = StringAlignment.Near;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    if (cbx.Items[0].GetType().Name == "DictionaryList")
                    {
                        // Draw the string
                        e.Graphics.DrawString(((ComLib.DictionaryList)cbx.Items[e.Index]).fnText, cbx.Font, brush, e.Bounds, sf);
                    }
                    else
                    {
                        // Draw the string
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }

        public void Cb_DrawItem_Line_Far(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Far;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    if (cbx.Items[0].GetType().Name == "DictionaryList")
                    {
                        // Draw the string
                        e.Graphics.DrawString(((ComLib.DictionaryList)cbx.Items[e.Index]).fnText, cbx.Font, brush, e.Bounds, sf);
                    }
                    else
                    {
                        // Draw the string
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }

        public void InitGrid(C1FlexGrid sender)
        {
            var fg = (C1FlexGrid)sender;

            fg.Redraw = false;

            //fg.Rows.Fixed = 1;
            
            //fg.Rows.Fixed = 0;

            fg.AllowEditing = false;

            fg.Font = new Font(oHeadfontName, 11.0f);
            //fg.Font = new Font("굴림", 20);

            fg.Cols[0].Visible = false;

            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            fg.Rows[0].Height = 40;

            // Grid Alternate Color
            fg.BackColor = Color.White;
            fg.Styles.Alternate.BackColor = Color.FromArgb(255, 245, 245, 245);
            fg.Rows[0].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            

            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);
            csCellStyle.Border.Color = Color.FromArgb(255, 81,181,255);
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);


            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            //var crCellRange = fg.GetCellRange(1, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            // column 을 임의로 리사이징 못하게.
            fg.AllowResizing = AllowResizingEnum.None;

            fg.AllowSorting = AllowSortingEnum.None;

            //fg.Rows.Frozen = 1;

            // 수정되지않게 고정
            fg.Rows.Frozen = 0;

            fg.ExtendLastCol = true;
            //fg.ExtendLastCol = false;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        public void InitGrid_search(C1FlexGrid sender)
        {
            var fg = (C1FlexGrid)sender;

            fg.BeginUpdate();

            fg.Redraw = false;

            //flexgrid 에서는 모든게 다 cell로 구성되어있슴... cols or rows 중 라인을 fixed 시키면.. 됨...
            //fg.Rows.Fixed = 1;

            //fg.KeyDownEdit += Fg_KeyDownEdit;

            fg.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyDown += Fg_KeyDown;

            // row[0] (header 고정)
            fg.Rows.Fixed = 1;

            fg.AutoClipboard = true;

            // cell들중 첫번째 row를 고정으로 해서 header로 사용
            //fg.Rows.Fixed = 1;

            // row 선택시 행별로 선택됨..
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange;
            fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;

            //fg.Selection.Style.BackColor = Color.Black; 

            // cell들중 모든 column을 고정 해제시킴..
            fg.Cols.Fixed = 0;

            fg.AllowEditing = true;

            fg.AllowSorting = AllowSortingEnum.None;

            fg.AllowResizing = AllowResizingEnum.Columns;

            //fg.Font = new Font("돋움", 11.0f, FontStyle.Bold);
            fg.Rows[0].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);


            for (int col = 0; col < fg.Cols.Count; col++)
            {
                fg.Cols[col].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            }

            //fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["EmptyArea"].BackColor = Color.FromArgb(247, 247, 247);
            

            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            //fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            //fg.Rows[0].Height = 40;

            // Grid White Color
            fg.BackColor = Color.WhiteSmoke;

            // cell 라인별 색 설정
            fg.Styles.Alternate.BackColor = Color.White;//Color.FromArgb(255, 245, 245, 245);
            
            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            var MDCellStyle = fg.Styles.Add("ModifyStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            MDCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            MDCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            MDCellStyle.ForeColor = Color.Blue;

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            //crCellRange.Style = fg.Styles["CellStyle"];

            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = fg.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            DelRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle UpRs = fg.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            UpRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle InsRs = fg.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;
            InsRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            //subtotal
            CellStyle s = fg.Styles[CellStyleEnum.Subtotal0];

            s.Border.Color = fg.Styles["CellStyle"].Border.Color;
            s.ForeColor = fg.Styles["CellStyle"].ForeColor;
            s.BackColor = fg.Styles["CellStyle"].BackColor;

            // 1 첫번째 row 고정 (header 제외)
            fg.Rows.Frozen = 0;

            // 스크롤시에  0번의 row를 고정시킴
            //fg.Rows.Frozen = 0;

            // column 추가 생성 가능
            fg.ExtendLastCol = true;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        public void InitGrid_search_Mor(C1FlexGrid sender)
        {
            var fg = (C1FlexGrid)sender;

            fg.BeginUpdate();

            fg.Redraw = false;

            //flexgrid 에서는 모든게 다 cell로 구성되어있슴... cols or rows 중 라인을 fixed 시키면.. 됨...
            //fg.Rows.Fixed = 1;

            //fg.KeyDownEdit += Fg_KeyDownEdit;

            fg.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyDown += Fg_KeyDown;

            // row[0] (header 고정)
            fg.Rows.Fixed = 1;

            fg.AutoClipboard = true;

            // cell들중 첫번째 row를 고정으로 해서 header로 사용
            //fg.Rows.Fixed = 1;

            // row 선택시 행별로 선택됨..
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange;
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell;
            //fg.Selection.Style.BackColor = Color.Black; 

            // cell들중 모든 column을 고정 해제시킴..
            fg.Cols.Fixed = 0;

            fg.AllowEditing = true;

            fg.AllowSorting = AllowSortingEnum.None;

            fg.AllowResizing = AllowResizingEnum.Columns;

            //fg.Font = new Font("돋움", 11.0f, FontStyle.Bold);
            fg.Rows[0].StyleNew.Font = new Font(OHeadfontName, fontSizeBiggerst, FontStyle.Bold);


            for (int col = 0; col < fg.Cols.Count; col++)
            {
                fg.Cols[col].StyleNew.Font = new Font(OHeadfontName, fontSizeBiggerst, FontStyle.Bold);
            }

            //fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["EmptyArea"].BackColor = Color.FromArgb(247, 247, 247);


            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            //fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            //fg.Rows[0].Height = 40;

            // Grid White Color
            //fg.BackColor = Color.Red;
            fg.BackColor = Color.WhiteSmoke;

            // cell 라인별 색 설정
            fg.Styles.Alternate.BackColor = Color.White;//Color.FromArgb(255, 245, 245, 245);

            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            var MDCellStyle = fg.Styles.Add("ModifyStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            MDCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            MDCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            MDCellStyle.ForeColor = Color.Blue;

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            //crCellRange.Style = fg.Styles["CellStyle"];

            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = fg.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            DelRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle UpRs = fg.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            UpRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle InsRs = fg.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;
            InsRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            //subtotal
            CellStyle s = fg.Styles[CellStyleEnum.Subtotal0];

            s.Border.Color = fg.Styles["CellStyle"].Border.Color;
            s.ForeColor = fg.Styles["CellStyle"].ForeColor;
            s.BackColor = fg.Styles["CellStyle"].BackColor;

            // 1 첫번째 row 고정 (header 제외)
            fg.Rows.Frozen = 0;

            // 스크롤시에  0번의 row를 고정시킴
            //fg.Rows.Frozen = 0;

            // column 추가 생성 가능
            fg.ExtendLastCol = true;

            fg.Redraw = true;

            fg.EndUpdate();
        }


        public void InitGrid_search_Mor_New(C1FlexGrid sender)
        {
            var fg = (C1FlexGrid)sender;

            fg.BeginUpdate();

            fg.Redraw = false;

            //flexgrid 에서는 모든게 다 cell로 구성되어있슴... cols or rows 중 라인을 fixed 시키면.. 됨...
            //fg.Rows.Fixed = 1;

            //fg.KeyDownEdit += Fg_KeyDownEdit;

            fg.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyDown += Fg_KeyDown;

            // row[0] (header 고정)
            fg.Rows.Fixed = 1;

            fg.AutoClipboard = true;

            // cell들중 첫번째 row를 고정으로 해서 header로 사용
            //fg.Rows.Fixed = 1;

            // row 선택시 행별로 선택됨..
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange;
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell;

            //fg.Selection.Style.BackColor = Color.Black; 

            // cell들중 모든 column을 고정 해제시킴..
            fg.Cols.Fixed = 0;

            fg.AllowEditing = true;

            fg.AllowSorting = AllowSortingEnum.None;

            fg.AllowResizing = AllowResizingEnum.Columns;

            //fg.Font = new Font("돋움", 11.0f, FontStyle.Bold);
            fg.Rows[0].StyleNew.Font = new Font(OHeadfontName, fontSizeBiggerst, FontStyle.Bold);


            for (int col = 0; col < fg.Cols.Count; col++)
            {
                fg.Cols[col].StyleNew.Font = new Font(OHeadfontName, fontSizeBiggerst, FontStyle.Bold);
            }

            //fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["EmptyArea"].BackColor = Color.FromArgb(247, 247, 247);


            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            //fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            //fg.Rows[0].Height = 40;

            // Grid White Color
            fg.BackColor = Color.White;
            //fg.BackColor = Color.Tomato;
            //fg.BackColor = Color.Red;

            // cell 라인별 색 설정
            fg.Styles.Alternate.BackColor = Color.White;//Color.FromArgb(255, 245, 245, 245);

            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            //csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);
            //csCellStyle.BackColor = Color.Aqua;
            csCellStyle.BackColor = Color.FromArgb(255, 255, 72, 72);


            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            var MDCellStyle = fg.Styles.Add("ModifyStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            MDCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            MDCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            MDCellStyle.ForeColor = Color.Blue;

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            //crCellRange.Style = fg.Styles["CellStyle"];

            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = fg.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            DelRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle UpRs = fg.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            UpRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle InsRs = fg.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;
            InsRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            //subtotal
            CellStyle s = fg.Styles[CellStyleEnum.Subtotal0];

            s.Border.Color = fg.Styles["CellStyle"].Border.Color;
            s.ForeColor = fg.Styles["CellStyle"].ForeColor;
            s.BackColor = fg.Styles["CellStyle"].BackColor;

            // 1 첫번째 row 고정 (header 제외)
            fg.Rows.Frozen = 0;

            // 스크롤시에  0번의 row를 고정시킴
            //fg.Rows.Frozen = 0;

            // column 추가 생성 가능
            fg.ExtendLastCol = true;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        public void InitGrid_search(C1FlexGrid sender, int addTotal)
        {
            var fg = (C1FlexGrid)sender;

            fg.BeginUpdate();

            fg.Redraw = false;

            //flexgrid 에서는 모든게 다 cell로 구성되어있슴... cols or rows 중 라인을 fixed 시키면.. 됨...
            //fg.Rows.Fixed = 1;

            //fg.KeyDownEdit += Fg_KeyDownEdit;

            fg.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            fg.KeyDown += Fg_KeyDown;

            // row[0] (header 고정)
            fg.Rows.Fixed = 1 + addTotal;

            fg.AutoClipboard = true;

            // cell들중 첫번째 row를 고정으로 해서 header로 사용
            //fg.Rows.Fixed = 1;

            // row 선택시 행별로 선택됨..
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange;
            fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;

            //fg.Selection.Style.BackColor = Color.Black; 

            // cell들중 모든 column을 고정 해제시킴..
            fg.Cols.Fixed = 0;

            fg.AllowEditing = true;

            fg.AllowSorting = AllowSortingEnum.None;

            fg.AllowResizing = AllowResizingEnum.Columns;

            //fg.Font = new Font("돋움", 11.0f, FontStyle.Bold);
            fg.Rows[0].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);


            for (int col = 0; col < fg.Cols.Count; col++)
            {
                fg.Cols[col].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            }

            //fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["EmptyArea"].BackColor = Color.FromArgb(247, 247, 247);


            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            //fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            //fg.Rows[0].Height = 40;

            // Grid White Color
            fg.BackColor = Color.WhiteSmoke;

            // cell 라인별 색 설정
            fg.Styles.Alternate.BackColor = Color.White;//Color.FromArgb(255, 245, 245, 245);

            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            var MDCellStyle = fg.Styles.Add("ModifyStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            MDCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            MDCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            MDCellStyle.ForeColor = Color.Blue;

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            //crCellRange.Style = fg.Styles["CellStyle"];

            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = fg.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            DelRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle UpRs = fg.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            UpRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle InsRs = fg.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;
            InsRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            // 1 첫번째 row 고정 (header 제외)
            fg.Rows.Frozen = 0;

            // 스크롤시에  0번의 row를 고정시킴
            //fg.Rows.Frozen = 0;

            // column 추가 생성 가능
            fg.ExtendLastCol = true;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        private void Fg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                
            }
        }

        private void Fg_KeyDownEdit(object sender, KeyEditEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            int pKey = e.KeyValue;
            int pCol = grd.Col;

            ////스페이스 눌렀을 시, 이벤트를 빠져나간다.
            //if (pKey == 32)
            //{
            //    //pKey = "0";
            //    return;
            //}
            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                if (e.Col < grd.Cols.Count - 1)
                {
                    grd.Select(e.Row, e.Col + 1);
                    grd.AllowEditing = true;
                }
            }
        }


        /// <summary>
        /// 그리드 초기화시 폰트사이즈를 기본으로하려면 false, 임의로 fontsize로 하려면 true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="IsModifyFontsize"></param>
        public void InitGrid_search(C1FlexGrid sender, bool IsModifyFontsize)
        {
            var fg = (C1FlexGrid)sender;

            fg.Redraw = false;

            //flexgrid 에서는 모든게 다 cell로 구성되어있슴... cols or rows 중 라인을 fixed 시키면.. 됨...
            //fg.Rows.Fixed = 1;

            // row[0] (header 고정)
            fg.Rows.Fixed = 1;

            fg.AutoClipboard = true;

            // cell들중 첫번째 row를 고정으로 해서 header로 사용
            //fg.Rows.Fixed = 1;

            // row 선택시 행별로 선택됨..
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange;
            fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            //fg.Selection.Style.BackColor = Color.Black; 

            // cell들중 모든 column을 고정 해제시킴..
            fg.Cols.Fixed = 0;

            fg.AllowEditing = false;

            fg.AllowSorting = AllowSortingEnum.None;

            fg.AllowResizing = AllowResizingEnum.Columns;

            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            if (!IsModifyFontsize)
            {
                //fg.Font = new Font("돋움", 11.0f, FontStyle.Bold);
                fg.Rows[0].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);


                for (int col = 0; col < fg.Cols.Count; col++)
                {
                    fg.Cols[col].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
                }
            }
            

            //fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            fg.Styles["Highlight"].BackColor = SystemColors.Highlight;

            fg.Styles["EmptyArea"].BackColor = Color.FromArgb(247, 247, 247);

            //fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            //fg.Rows[0].Height = 40;

            // Grid White Color
            fg.BackColor = Color.WhiteSmoke;

            // cell 라인별 색 설정
            fg.Styles.Alternate.BackColor = Color.White;//Color.FromArgb(255, 245, 245, 245);


            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = fg.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            DelRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle UpRs = fg.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            UpRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle InsRs = fg.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;
            InsRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            // 1 첫번째 row 고정 (header 제외)
            fg.Rows.Frozen = 0;

            // 스크롤시에  0번의 row를 고정시킴
            //fg.Rows.Frozen = 0;

            // column 추가 생성 가능
            fg.ExtendLastCol = true;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        private void Fg_MouseClick(object sender, MouseEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (Control.ModifierKeys == Keys.Control)
            {
                int row = grd.HitTest().Row;
                int col = grd.HitTest().Column;

                string contents = string.Empty;
                try
                {
                    contents = grd[row, col].ToString();
                }
                catch (Exception)
                {

                    return;
                }

                if (row <= 0 || string.IsNullOrEmpty(contents))
                {
                    return;
                }
                

                try
                {
                    //String str = Clipboard.GetText();
                    //add delimiters as per your requirement.
                    //add [backslash]t to paste cell data in single row in ms-excel
                    //Clipboard.SetText(str + grd[row, col].ToString() + "[backslash]t");
                    Clipboard.Clear();

                    Clipboard.SetText(grd[row, col].ToString());
                }
                catch (Exception)
                {

                    return;

                }
            }

        }

        /// <summary>
        /// 고정된 행의 숫자: FixRowCount, 헤드의 스타일이 적용될 끝 Row 값: styleEndRow
        /// 을 이용해서 그리드 스타일 초기화
        /// </summary>
        /// <param name="sender">그리드 </param>
        /// <param name="IsModifyFontsize">기본폰트를 적용할것인가</param>
        /// <param name="FixRowCount">고정행의 갯수 1부터 시작함</param>
        /// <param name="styleEndRow">스타일이 들어가는 해당 그리드 Row index 0부터시작함</param>
        public void InitGrid_search(C1FlexGrid sender, bool IsModifyFontsize, int FixRowCount, int styleEndRow)
        {
            var fg = (C1FlexGrid)sender;
            //그리드 병합이 되도록 디폴트로 설정
            fg.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Default;

            fg.Redraw = false;

            //fg.KeyDownEdit += Fg_KeyDownEdit;

            //flexgrid 에서는 모든게 다 cell로 구성되어있슴... cols or rows 중 라인을 fixed 시키면.. 됨...
            //fg.Rows.Fixed = 1;

            // row[0] (header 고정)
            fg.Rows.Fixed = FixRowCount;

            fg.AutoClipboard = true;

            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            // cell들중 첫번째 row를 고정으로 해서 header로 사용
            //fg.Rows.Fixed = 1;

            // row 선택시 행별로 선택됨..
            //fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange;
            fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            //fg.Selection.Style.BackColor = Color.Black; 

            // cell들중 모든 column을 고정 해제시킴..
            fg.Cols.Fixed = 0;

            fg.AllowEditing = false;

            fg.AllowSorting = AllowSortingEnum.None;

            fg.AllowResizing = AllowResizingEnum.Columns;

            if (!IsModifyFontsize)
            {
                //fg.Font = new Font("돋움", 11.0f, FontStyle.Bold);
                fg.Rows[0].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);


                for (int col = 0; col < fg.Cols.Count; col++)
                {
                    fg.Cols[col].StyleNew.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
                }
            }


            fg.Styles["Highlight"].BackColor = SystemColors.Highlight;
            //fg.Styles["Highlight"].BackColor = Color.Transparent;

            fg.Styles["EmptyArea"].BackColor = Color.FromArgb(247, 247, 247);

            //fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            //fg.Rows[0].Height = 40;

            // Grid White Color
            fg.BackColor = Color.WhiteSmoke;

            // cell 라인별 색 설정
            fg.Styles.Alternate.BackColor = Color.White;//Color.FromArgb(255, 245, 245, 245);


            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);

            var crCellRange = fg.GetCellRange(0, 0, styleEndRow, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            var MDCellStyle = fg.Styles.Add("ModifyStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            MDCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            MDCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            MDCellStyle.ForeColor = Color.Blue;

            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = fg.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            DelRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle UpRs = fg.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            UpRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle InsRs = fg.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;
            InsRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            C1.Win.C1FlexGrid.CellStyle BacRs = fg.Styles.Add("BACColor");
            BacRs.BackColor = Color.White;
            BacRs.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

            // 1 첫번째 row 고정 (header 제외)
            fg.Rows.Frozen = 0;

            // 스크롤시에  0번의 row를 고정시킴
            //fg.Rows.Frozen = 0;

            // column 추가 생성 가능
            fg.ExtendLastCol = true;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        public void InitGrid_search_end_add(C1FlexGrid sender)
        {
            var fg = (C1FlexGrid)sender;

            fg.Redraw = false;

            //fg.Rows.Fixed = 1;

            //fg.Rows.Fixed = 0;

            fg.AllowEditing = false;

            fg.Font = new Font(oHeadfontName, 11.0f);
            //fg.Font = new Font("굴림", 20);

            fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            fg.Rows[0].Height = 40;

            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            // Grid Alternate Color
            fg.BackColor = Color.White;
            fg.Styles.Alternate.BackColor = Color.FromArgb(255, 245, 245, 245);
            fg.Rows[0].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);


            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];

            // column 을 임의로 리사이징 못하게.
            fg.AllowResizing = AllowResizingEnum.None;

            fg.AllowSorting = AllowSortingEnum.None;

            //fg.Rows.Frozen = 1;

            // 수정되지않게 고정
            fg.Rows.Frozen = 0;

            //fg.ExtendLastCol = true;
            fg.ExtendLastCol = false;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        public void InitGrid_temp(C1FlexGrid sender)
        {
            var fg = (C1FlexGrid)sender;

            fg.Redraw = false;

            //flexgrid 에서는 모든게 다 cell로 구성되어있슴... cols or rows 중 라인을 fixed 시키면.. 됨...
            //fg.Rows.Fixed = 1;

            // row[0] (header 고정)
            fg.Rows.Fixed = 0;

            // cell들중 첫번째 row를 고정으로 해서 header로 사용
            //fg.Rows.Fixed = 1;

            // row 선택시 행별로 선택됨..
            fg.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            //fg.Selection.Style.BackColor = Color.Black; 

            // cell들중 모든 column을 고정 해제시킴..
            fg.Cols.Fixed = 0;

            fg.AllowEditing = false;

            fg.AllowResizing = AllowResizingEnum.None;

            // Ctrl + 마우스 클릭시에 CELL TEXT COPY
            fg.MouseClick += Fg_MouseClick;

            //fg.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            for (int col = 0; col < fg.Cols.Count; col++)
            {
                fg.Cols[col].StyleNew.Font = new Font("돋움", 11.0f);
            }
            fg.Rows[0].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            fg.Styles["Highlight"].BackColor = SystemColors.Highlight;

            //fg.Cols[0].Visible = false;

            // Grid Height
            fg.Rows.MinSize = 26;
            fg.Rows[0].Height = 26;
            //fg.Rows[0].Height = 100;
            //fg.Rows[0].Height = 40;

            // Grid White Color
            fg.BackColor = Color.WhiteSmoke;

            // cell 라인별 색 설정
            fg.Styles.Alternate.BackColor = Color.White;//Color.FromArgb(255, 245, 245, 245);

            var csCellStyle = fg.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            csCellStyle.BackColor = Color.FromArgb(255, 218, 233, 245);

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            crCellRange.Style = fg.Styles["CellStyle"];
            fg.Rows[0].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);
            // 1 첫번째 row 고정 (header 제외)
            //fg.Rows.Frozen = 1;

            // 스크롤시에  0번의 row를 고정시킴
            fg.Rows.Frozen = 0;

            // column 추가 생성 가능
            fg.ExtendLastCol = false;

            fg.Redraw = true;

            fg.EndUpdate();
        }

        public void InitPicture(PictureBox picture)
        {
            picture.Dock = System.Windows.Forms.DockStyle.Fill;
            picture.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            picture.BorderStyle = BorderStyle.None;
        }

        public void InitTitle(Label title_lb, string ownertext, string titletext)
        {
            title_lb.Dock = System.Windows.Forms.DockStyle.Fill;
            title_lb.Text = ownertext + " > "+ titletext;
            title_lb.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            title_lb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            title_lb.BorderStyle = BorderStyle.None;
        }

        public void InitPanel(Panel panel)
        {
            panel.BorderStyle = BorderStyle.None;
            panel.Margin = new System.Windows.Forms.Padding(1, 3, 1, 1);

        }

        public void InitCount(Label count_lb)
        {
            count_lb.Font = new Font(OHeadfontName, FontSizeMiddle);
            //count_lb.Text = "건수";
        }

        public void InitTab(C1DockingTab tap)
        {
            tap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tap.TabAreaBorder = false;
            tap.Indent = 0;
            tap.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //tap.BackColor = Color.White;
            tap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            tap.TabStyle = C1.Win.C1Command.TabStyleEnum.Classic;
            //tap.TabStyle = C1.Win.C1Command.TabStyleEnum.Sloping;
            tap.VisualStyle = C1.Win.C1Command.VisualStyle.Custom;
            tap.VisualStyleBase = C1.Win.C1Command.VisualStyle.Classic;
        }

        public void InitDateEdit(C1DateEdit dateEdit)
        {
            dateEdit.Font = new Font(OHeadfontName, FontSizeSmall);
            dateEdit.FormatType = FormatTypeEnum.ShortDate;
        }

        public void InitDateEdit(DateTimePicker dateEdit)
        {
            dateEdit.Format = DateTimePickerFormat.Short;
            dateEdit.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            dateEdit.Size = new Size(139, 26);
        }

        public void InitDateEdit(DateTimePicker dateEdit, DateTimePickerFormat format)
        {
            dateEdit.Format = format;
            //dateEdit.CustomFormat = hh:mm: ss dddd MMMM dd, yyyy
            dateEdit.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateEdit.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //dateEdit.Size = new Size(139, 26);
        }
        public void InitStartDateEdit(DateTimePicker dateEdit, DateTimePickerFormat format)
        {
            dateEdit.Format = format;
            //dateEdit.CustomFormat = hh:mm: ss dddd MMMM dd, yyyy
            dateEdit.CustomFormat = "yyyy-MM-dd HH:mm:00";
            dateEdit.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //dateEdit.Size = new Size(139, 26);
        }
        public void InitEndDateEdit(DateTimePicker dateEdit, DateTimePickerFormat format)
        {
            dateEdit.Format = format;
            //dateEdit.CustomFormat = hh:mm: ss dddd MMMM dd, yyyy
            dateEdit.CustomFormat = "yyyy-MM-dd HH:mm:59";
            dateEdit.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //dateEdit.Size = new Size(139, 26);
        }
        public void InitTimeEdit(DateTimePicker dateEdit)
        {
            //dateEdit.Format = DateTimePickerFormat.Time;
            dateEdit.Format = DateTimePickerFormat.Custom;
            dateEdit.CustomFormat = "HH:mm:ss";
            dateEdit.ShowUpDown = true;
            
            dateEdit.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
            //dateEdit.Size = new Size(139, 26);
        }

        public void InitTextBox(TextBox tb)
        {
            tb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
        }

        public void InitTextBox(C1TextBox tb)
        {
            tb.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);
        }

        public void InitDateTimePicker(DateTimePicker dtp)
        {
            dtp.Font = new Font(OHeadfontName, FontSizeSmall, FontStyle.Bold);

        }

        public void KeyDownEdit(object sender, KeyEditEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            int pKey = e.KeyValue;
            int pCol = grd.Col;

            ////스페이스 눌렀을 시, 이벤트를 빠져나간다.
            //if (pKey == 32)
            //{
            //    //pKey = "0";
            //    return;
            //}
            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                if (e.Col < grd.Cols.Count - 1)
                {
                    grd.Select(e.Row, e.Col + 1);
                    grd.AllowEditing = true;
                }
            }
        }


        public void ReArrageDateEdit(DateTimePicker modifiedDateEditor, DateTimePicker start_dt, DateTimePicker end_dt)
        {
            var dtpRearrange = (modifiedDateEditor.Name == start_dt.Name) ? end_dt : start_dt;

            if (start_dt.Value > end_dt.Value)
            {
                dtpRearrange.Value = modifiedDateEditor.Value;
            }
        }
    }
}