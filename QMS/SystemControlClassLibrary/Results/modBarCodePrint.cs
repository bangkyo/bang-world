using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using ComLib;

//바코드프린트 출력 관련 사용
//using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;

//메시지박스 때문에 사용
using System.Windows.Forms;

namespace SystemControlClassLibrary
{
    public class modBarCodePrint
    {
        VbFunc vf = new VbFunc();

        #region "바코드 프린터 관련 정의 - DLL 및 CREATEFILE 함수 정의"

        [DllImport("kernel32.dll", SetLastError = true)]
        //static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        #endregion

        #region "바코드변수"

        public string BARCODE_COUNTRY_BASIS;
        public string BARCODE_SIZE_CODE;
        public string BARCODE_LABEL_SIZE_NAME;
        public string BARCODE_APPORVAL_NO;
        public string BARCODE_INSPECT_DATE;
        public string BARCODE_ITEM;
        public string BARCODE_ITEM_SIZE;
        public string BARCODE_SIZE_NAME;
        public string BARCODE_STEEL;
        public string BARCODE_PRINTED_BASIS;
        public string BARCODE_INSPECTER_NAME;
        public string BARCODE_HEAT;
        public string BARCODE_MARKET_TYPE;
        public string BARCODE_UOM;
        public string BARCODE_UOM2;
        public string BARCODE_SIRF_CODE;
        public string BARCODE_Length;
        public string BARCODE_BONSU;
        public string BARCODE_WEIGHT;
        public string BARCODE_BUNDLE_NO;
        public string BARCODE_COMPANY;
        public string BARCODE_AFTER_ROUTING;
        public string BARCODE_SURFACE_LEVEL;
        public string BARCODE_ITEM_USE;
        public string BARCODE_MEMO1;
        public string BARCODE_MEMO2;
        public string BARCODE_MEMO3;
        public string BARCODE_MEMO4;
        public string BARCODE_MEMO5;
        public string BARCODE_MEMO6;
        public string BARCODE_MEMO7;
        public string BARCODE_MEMO8;
        public string BARCODE_MEMO9;
        public string BARCODE_MEMO10;
        public string BARCODE_MEMO11;
        public string BARCODE_MEMO12;
        public string BARCODE_MEMO13;
        public string BARCODE_MEMO14;
        public string BARCODE_MEMO15;
        public string BARCODE_PO_NO;
        public string BARCODE_SPEC;
        public string BARCODE_Length2;
        public string BARCODE_RES_SEQ;
        public Boolean BARCODE_CANCLE;
        public string BARCODE_PROGRAM;
        public int BARCODE_PRINT_COUNT;
        public string BARCODE_PRINT_OPTION1;
        public int BARCODE_CUTTING_COUNT;
        public int BARCODE_CUTTING_LOCAL_COUNT;
        public string BARCODE_SHIP_NO;
        public string BARCODE_PRINT_STRING;
        public string BARCODE_SP_CNT;
        public string BARCODE_FACTORY;
        public string BARCODE_LABEL_GUBUN;
        public string BARCODE_YARD;
        public string BARCODE_TAG_KIND;
        public string BARCODE_LOCATION;
        public string BARCODE_NUM;
        public string BARCODE_KIND_TAG;
        public string BARCODE_SPEC2;
        public string BARCODE_WEIGHT_DSP;
        public string BARCODE_PER_BONSU;
        public string BARCODE_UK_DSP;
        public string BARCODE_HEAT_DSP;
        public string BARCODE_BD_NO;
        public int BARCODE_PIECE_CNT;
        public int BARCODE_PCS;
        public string BARCODE_OLD_NEW_LABEL;
        public string BARCODE_BUNDLE_NO_DSP;
        public string BARCODE_RB_USE_GUBUN;
        public string BARCODE_PRT_BUNDLE_NO;
        public string BARCODE_HEAT_ALL_PRT;
        public string BARCODE_STAND_NUM;
        public string BARCODE_LICENSE;
        public string BARCODE_INSPECTOR;
        public string BARCODE_MARK;
        public string BARCODE_MEMO_CODE1;
        public string BARCODE_MEMO_CODE2;
        public string BARCODE_ARABIA;
        public string BARCODE_HONJAE;
        public string BARCODE_BNPP;
        public string BARCODE_MATERIAL;
        public string BARCODE_chkNEW_LABEL;
        public string BARCODE_chgSteel;
        public string BARCODE_STEEL1;
        public string BARCODE_PRODUCT_DATE;
        public string BARCODE_KS;
        public string BARCODE_MODE;
        public string BARCODE_OLD_ITEM;

        public string BARCODE_MFG_DATE;

        public int BARCODE_TOTAL_CNT;
        #endregion

        #region "스크립트 처리용 함수"

        /* 스크립트 처리용 함수*/
        public string Space(int Length)
        {
            var rtnVal = "";
            for (var i = 0; i < Length; i++)
            {
                rtnVal += " ";
            }
            return rtnVal;
        }

        public string Mid(string str, int start, int len)
        {
            if (start < 0 || len < 0) return "";

            /*
            string iEnd = vf.Len(str).ToString();
            string iLen = vf.Len(str).ToString();

            if (start + len > int.Parse(iLen))
            {
                //iEnd = iLen;
                iEnd = (Convert.ToInt32(iLen) - start).ToString();
            }
            else
            {
                //iEnd = (start + len).ToString();
                iEnd = (start + len).ToString();
            }

            return str.Substring(start, int.Parse(iEnd));
            */

            //시작위치가 데이터의 범위를 넘긴지 확인
            if (start <= str.Length)
            {
                if(start + len <= str.Length)
                {
                    return str.Substring(start, len);
                }
                else
                {
                    return str.Substring(start);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public int InStr(string strSearch, string charSearchFor)
        {
            for (var i = 0; i < vf.Len(strSearch); i++)
            {
                if (charSearchFor == Mid(strSearch, i, 1))
                {
                    return i;
                }
            }
            return -1;
        }

        /* Left*/
        public string Left(string Str, int Num)
        {
            if (Num <= 0)
            {
                return "";
            }
            else if (Num > int.Parse(Str))
            {
                return Str;
            }
            else
            {
                return Str.Substring(0, Num);
            }
        }       

        #endregion

        #region "바코드발행"

        public void PrintStart(string tmpPrintType, String tmpPortName, int tmpCNT)
        {
            String prtData_Image;
            String prtData;

            //'이미지 저장
            prtData_Image = "";
            prtData = "";

            #region "바코드 스크립트 MAKE 분기"

            switch (tmpPrintType)
            {
                case "PB5_A_NEW":
                    if (BARCODE_PRINT_OPTION1 == "NO")
                    {
                        BARCODE_COUNTRY_BASIS = "NO";
                    }
                    else
                    {
                        prtData += Print_Image(BARCODE_COUNTRY_BASIS);
                        prtData += Print_Image_KSA(BARCODE_COUNTRY_BASIS);
                    }
                    prtData += PB5_A_NEW();
                    break;
                case "FREE_STEEL_EXPORT_10":                   
                    prtData += FREE_STEEL_EXPORT_10(); ;
                    break;
                case "FREE_STEEL_EXPORT_14":
                    prtData += FREE_STEEL_EXPORT_14(); ;
                    break;
                case "NUCLEAR_TAG_NEW":
                    prtData += NUCLEAR_TAG_NEW();
                    break;
                case "DB_EXPORT_HISTORY_SHIPPING_NEW":
                    prtData += DB_EXPORT_HISTORY_SHIPPING_NEW();
                    break;
                case "RB_BINDING_TAG":
                    //원형강 Tracking 구축관련 - 원형강 이력라벨 발행(바인딩) : SHARP(2013.04.05)
                    prtData += RB_BINDING_TAG();
                    break;
                case "PB5_A": //  '중형강 라벨
                    if (BARCODE_PRINT_OPTION1 == "NO")
                    {
                        BARCODE_COUNTRY_BASIS = "NO";
                    }
                    else
                    {
                        //prtData = Print_Image1(BARCODE_COUNTRY_BASIS);
                        prtData = Print_Image1(BARCODE_COUNTRY_BASIS);
                    }
                    prtData += PB5_A(tmpCNT);
                    break;
                case "PB5_C": // '중형강 라벨
                    if (BARCODE_PRINT_OPTION1 == "NO")
                    {
                        BARCODE_COUNTRY_BASIS = "NO";
                    }
                    else
                    {
                        //prtData = Print_Image(BARCODE_COUNTRY_BASIS);
                        prtData += Print_Image(BARCODE_COUNTRY_BASIS);
                    }
                    prtData += PB5_C();
                    break;              
                case "PB5_C_NEW":
                    if (BARCODE_PRINT_OPTION1 == "NO")
                    {
                        BARCODE_COUNTRY_BASIS = "NO";
                    }
                    else
                    {
                        //prtData = Print_Image(BARCODE_COUNTRY_BASIS);
                        //prtData += Print_Image_KSA(BARCODE_COUNTRY_BASIS);
                        prtData += Print_Image(BARCODE_COUNTRY_BASIS);
                        prtData += Print_Image_KSA(BARCODE_COUNTRY_BASIS);
                    }
                    prtData += PB5_C_NEW(tmpCNT);
                    break;
                case "RB_SQUARE":
                    prtData += RB_SQUARE();
                    break;
                case "RB_ROUND_SML":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += RB_ROUND_SML();
                    break;
                case "RB_ROUND_MID":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += RB_ROUND_MID();
                    break;
                //2014.09.24 구성욱 추가
                case "RB_SQUARE_JIS":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += RB_SQUARE_JIS();
                    break;
                case "RB_ROUND_LAG":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += RB_ROUND_LAG();
                    break;

                /*2015.05.04 라운드바 통합으로 처리*/
                case "RB_SQUARET":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += RB_SQUARET();
                    break;
                case "RB_SQUARET2":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += RB_SQUARET2();
                    break;
                case "RB_SQUARET3":
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += RB_SQUARET3();
                    break;
                case "RB_SQUAREHOLE":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    //prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    //prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);

                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += RB_SQUAREHOLE();
                    break;

                case "RB_ROUND_SMLT":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    //prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    //prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += RB_ROUND_SMLT();
                    break;
                case "RB_ROUND_MIDT":
                    //prtData = Print_Image_SP(BARCODE_PRINTED_BASIS);
                    //prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    //prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);

                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += RB_ROUND_MIDT();
                    break;
                case "RB_ROUND_LAGT":
                    //prtData = Print_Image_SP("JIS");
                    //prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    //prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP("JIS");
                    prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += RB_ROUND_LAGT();
                    break;
                case "RB_ROUND_MID1":
                    //			prtData = Print_Image_SP("JIS") ;
                    prtData += Print_Image_SP40("JIS");
                    prtData += RB_ROUND_MID1();
                    break;
                case "RB_ROUND_MID2":
                    //			prtData = Print_Image_SP("JIS") ;
                    prtData += Print_Image_SP40("JIS");
                    prtData += RB_ROUND_MID2();
                    break;
                case "RB_ROUND_MID3":
                    //			prtData = Print_Image_SP("JIS") ;
                    prtData += Print_Image_SP40("JIS");
                    prtData += RB_ROUND_MID3();
                    break;
                case "RB_ROUND_MID4":
                    //			prtData = Print_Image_SP("JIS") ;
                    prtData += Print_Image_SP40("JIS");
                    prtData += RB_ROUND_MID4();
                    break;
                case "RB_ROUND_MID5":
                    //			prtData = Print_Image_SP("JIS") ;
                    prtData += Print_Image_SP40("JIS");
                    prtData += RB_ROUND_MID5(tmpCNT);
                    break;
                case "RB_ROUND_BIGLAGT":
                    prtData += Print_Image_SP(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP40(BARCODE_PRINTED_BASIS);
                    prtData += Print_Image_SP25(BARCODE_PRINTED_BASIS);
                    prtData += RB_ROUND_BIGLAGT();
                    break;
                case "RB_HISTORY_TAG_NEW":
                    //원형강 Tracking 구축관련 - 원형강 이력라벨 발행 : SHARP(2011.06.01)
                    prtData += RB_HISTORY_TAG_NEW(tmpCNT);
                    break;
                //2015.05.08 특수강 라벨 통합으로 인한 추가
                case "RB_BINDING_TAGT":
                    //원형강 Tracking 구축관련 - 원형강 이력라벨 발행(바인딩) : SHARP(2013.04.05)
                    prtData += RB_BINDING_TAGT();
                    break;
                //2013.11.29 12386분할 1116103
                case "RB_BINDING_TAG_PB4":
                    prtData += RB_BINDING_TAG_PB4();
                    break;
                case "IA_TAG_00":
                    prtData += Print_Image_Hyundai();
                    prtData += IA_TAG_00(tmpCNT);
                    break;
                case "IA_TAG_01":
                    prtData += Print_Image_Hyundai();
                    prtData += IA_TAG_01(tmpCNT);
                    break;
                case "SP_U_TAG":
                    prtData += SP_U_TAG(tmpCNT, "0");
                    break;
                case "HB_CE_TAG":
                    prtData += HB_CE_TAG(tmpCNT, "0");
                    break;
                case "SP_DOMESTIC":
                    prtData += Print_Image_NEW(BARCODE_MARK.Trim().ToUpper());
                    prtData += Print_Image_KSA_PB4(BARCODE_MARK.Trim().ToUpper());
                    prtData += SP_DOMESTIC(tmpCNT);
                    break;
                case "HB_TAG":
                    prtData += Print_Image_NEW(BARCODE_MARK.Trim().ToUpper());
                    prtData += Print_Image_KSA_PB4(BARCODE_MARK.Trim().ToUpper());
                    prtData += Print_Image_NAEJIN(BARCODE_COUNTRY_BASIS.Trim().ToUpper());
                    prtData += HB_TAG(tmpCNT);
                    break;
                case "PB5_HB_LABELA":
                    if (BARCODE_PRINT_OPTION1.Trim().ToUpper() == "NO")
                    {
                        BARCODE_COUNTRY_BASIS = "NO";
                    }
                    else
                    {
                        prtData += Print_Image(((BARCODE_COUNTRY_BASIS))); //
                        prtData += Print_Image_KSA(((BARCODE_COUNTRY_BASIS)));
                        prtData += Print_Image_NAEJIN(((BARCODE_COUNTRY_BASIS)));
                        prtData += Print_Image_ACRS(((BARCODE_COUNTRY_BASIS)));
                    }
                    prtData += PB5_HB_LABELA(tmpCNT);
                    break;
                case "PB5_HB_LABELC":
                    if (BARCODE_PRINT_OPTION1.Trim().ToUpper() == "NO")
                    {
                        BARCODE_COUNTRY_BASIS = "NO";
                    }
                    else
                    {
                        prtData += Print_Image(((BARCODE_COUNTRY_BASIS))); //
                        prtData += Print_Image_KSA(((BARCODE_COUNTRY_BASIS)));
                    }
                    prtData += PB5_HB_LABELC(tmpCNT);
                    break;
                case "BLANK_TAG":
                    prtData += BLANK_TAG(tmpCNT);
                    break;
                case "PB5_LABEL_TAG_00":
                    prtData += Print_Image_Hyundai();
                    prtData += PB5_LABEL_TAG_00(tmpCNT);
                    break;
                case "PB5_LABEL_TAG_01":
                    prtData += Print_Image_Hyundai();
                    prtData += PB5_LABEL_TAG_01(tmpCNT);
                    break;
                case "DB_ATOMIC_TAG":
                    prtData += DB_ATOMIC_TAG(tmpCNT);
                    break;
                case "RAIL_SIDE_TAG":
                    prtData += Print_Image_Hyundai();
                    prtData += RAIL_SIDE_TAG(tmpCNT);
                    break;
                case "DB_EXPORT_NC_SHIPPING":
                    prtData += Print_Image_Hyundai();
                    prtData += DB_EXPORT_NC_SHIPPING(tmpCNT);
                    break;
                case "DB_EXPORT_STEEL_SHIPPING":
                    prtData += Print_Image_Hyundai();
                    prtData += DB_EXPORT_STEEL_SHIPPING(tmpCNT);
                    break;
                case "DB_EXPORT_HISTORY_SHIPPING":
                    prtData += DB_EXPORT_HISTORY_SHIPPING(tmpCNT);
                    break;
                case "DB_EXPORT_PDF417_NC_SHIPPING":  //2013.04.18, SHARP
                    prtData += Print_Image_Hyundai();
                    prtData += DB_EXPORT_PDF417_NC_SHIPPING(tmpCNT);
                    break;
                case "DB_EXPORT_PDF417_STEEL_SHIPPING": //2013.04.18, SHARP
                    prtData += Print_Image_Hyundai();
                    prtData += DB_EXPORT_PDF417_STEEL_SHIPPING(tmpCNT);
                    break;
                case "DB_EXPORT_PDF417_HISTORY_SHIPPING": //2013.04.18, SHARP
                    prtData += DB_EXPORT_PDF417_HISTORY_SHIPPING(tmpCNT);
                    break;
                case "NUCLEAR_TAG":
                    prtData += NUCLEAR_TAG(tmpCNT);
                    break;
                case "RB_DOMESTIC":
                    prtData += RB_DOMESTIC(tmpCNT);
                    break;
                case "STEEL_DOMESTIC_NEW":
                    prtData += Print_KS_Image(BARCODE_PRINTED_BASIS);
                    prtData += STEEL_DOMESTIC_NEW(tmpCNT);
                    break;
                case "STEEL_JIS_DOMESTIC_NEW":
                    prtData += Print_Image_Steel(BARCODE_PRINTED_BASIS);
                    prtData += STEEL_JIS_DOMESTIC_NEW(tmpCNT);
                    break;
                case "RUST_TAG":
                    prtData += RUST_TAG(tmpCNT);
                    break;
                case "PB3_SOJAE":
                    prtData += PB3_SOJAE(tmpCNT);
                    break;
                case "PB3_SOJAE_HONJAE":
                    prtData += PB3_SOJAE_HONJAE(tmpCNT);
                    break;       
                case "PB5_BUNDLE_HISTORY_TAG":
                    prtData += PB5_BUNDLE_HISTORY_TAG(tmpCNT);
                    break;
                case "SP_TAG":
                    prtData += Print_Image_SP(BARCODE_COUNTRY_BASIS);
                    prtData += SP_TAG(tmpCNT);
                    break;
                case "NEW_SIDE_TAG":
                    prtData += Print_Image_Hyundai();
                    prtData += NEW_SIDE_TAG(tmpCNT);
                    break;
                case "DB_UK_NC_SHIPPING":
                    prtData += Print_Image_H();
                    prtData += Print_Image_CARES();
                    prtData += Print_Image_UKAS();
                    prtData += Print_Image_Hyundai();
                    prtData += DB_UK_NC_SHIPPING(tmpCNT);
                    break;
                case "UK_TAG":
                    prtData += Print_Image_H();
                    prtData += Print_Image_CARES();
                    prtData += Print_Image_UKAS();
                    prtData += Print_Image_Hyundai();
                    prtData += UK_TAG(tmpCNT);
                    break;
                case "STEEL_SIRIM_TAG":
                    prtData += Print_Image_SIRIM();
                    prtData += Print_Image_CARES();
                    prtData += Print_Image_UKAS();
                    prtData += Print_Image_Hyundai();
                    prtData += STEEL_SIRIM_TAG(tmpCNT);
                    break;
                case "NC_SIRIM_TAG":
                    prtData += Print_Image_SIRIM();
                    prtData += Print_Image_CARES();
                    prtData += Print_Image_UKAS();
                    prtData += Print_Image_Hyundai();
                    prtData += NC_SIRIM_TAG(tmpCNT);
                    break;
                case "RB_SOM":
                    prtData += RB_SOM(tmpCNT);
                    break;
                case "SP_SIDE_TAG":
                    prtData += SP_SIDE_TAG(tmpCNT);
                    break;
                case "LOCATION_TAG":
                    prtData += LOCATION_TAG(tmpCNT);
                    break;
                case "IA_BUNDLE_TAG":
                    //prtData = Print_Image_Hyundai();
                    prtData += Print_Image_Hyundai();
                    prtData += IA_BUNDLE_TAG(tmpCNT);
                    break;
                case "CS_TAG1":
                    //			prtData = Print_Image_Hyundai();
                    prtData += CS_TAG1(tmpCNT);
                    break;
                case "BARCODE_TAG":
                    prtData += BARCODE_TAG();
                    break;
                case "PB4_EXPORT_LABEL_TP":        //대형(TP) 수동라벨 발행_20170425추가
                    prtData += PB4_EXPORT_LABEL_TP(tmpCNT);
                    break;
                case "PB4_MANUAL_LABEL":        //정정 대형(내수,수출) 수동라벨 발행_20170425추가
                    if(BARCODE_MARK == "1") //KS
                    {
                        prtData += Print_Image_NEW("1");               //KS
                        prtData += Print_Image_KSA_PB4("1");          //한국표준협회
                        prtData += Print_Image_NAEJIN("");            //내진용
                    } else if (BARCODE_MARK == "2") //JIS
                    {
                        prtData += Print_Image_NEW("2");               //JIS
                    }

                    if(BARCODE_ITEM == "RL")
                    {
                        prtData += Print_Image_Hyundai();
                    }
                        prtData += PB4_MANUAL_LABEL(tmpCNT);
                    break;
                case "PB4_MANUAL_LABEL2":        //정정 대형 (수출 : CE Mark용) 수동라벨 발행_20170425추가
                    if (BARCODE_MARK == "1") //KS
                    {
                        prtData += Print_Image_NEW("1");               //KS
                        prtData += Print_Image_KSA_PB4("1");          //한국표준협회
                        prtData += Print_Image_NAEJIN("");            //내진용
                    }
                    else if (BARCODE_MARK == "2") //JIS
                    {
                        prtData += Print_Image_NEW("2");               //JIS
                    }

                    if (BARCODE_ITEM == "RL")
                    {
                        prtData += Print_Image_Hyundai();
                    }
                    prtData = PB4_MANUAL_LABEL2(tmpCNT);
                    
                    break;
                default:
                    prtData = "";
                    break;
            }
            #endregion

            #region "바코드 프린터로 ZPL 명령어 보내기"

            // Create a buffer with the command
            Byte[] buffer = new byte[prtData.Length];
            buffer = System.Text.Encoding.ASCII.GetBytes(prtData);

            if (tmpPortName == "")
            {
                MessageBox.Show("프린터 드라이버가 선택되지 않았습니다.");
                return;
            }

            // Use the CreateFile external func to connect to the LPT1 port
            //SafeFileHandle printer = CreateFile("LPT1:", FileAccess.ReadWrite, 0, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            SafeFileHandle printer = CreateFile(tmpPortName + ":", FileAccess.ReadWrite, 0 ,IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            // Aqui verifico se a impressora é válida
            if (printer.IsInvalid == true)
            {
                MessageBox.Show("프린터가 적합하지 않습니다!!!(ERROR PRINT INVALID)");
                return;
            }

            // Open the filestream to the lpt1 port and send the command
            FileStream lpt1 = new FileStream(printer, FileAccess.ReadWrite);
            lpt1.Write(buffer, 0, buffer.Length);
            // Close the FileStream connection
            lpt1.Close();

            #endregion
        }

        #endregion

        #region "바코드 스크립트"
        public string PB5_A_NEW(){
            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);           

            Zpl += "^XA";
            Zpl += ("^LH" + "017" + "," + "017" + "^FS");
            Zpl += "^MMT";

            if (BARCODE_ITEM == "IP")
            {
                BARCODE_ITEM = "IPE";
            }

            Zpl += "^A0N,35^FO130,005^FD" + BARCODE_ITEM + "^FS"; //            '품명
            Zpl += "^A0N,35^FO130,054^FD" + BARCODE_STEEL + "^FS"; //          '강종
            Zpl += "^A0N,19^FO200,030^FD" + BARCODE_SIZE_CODE + "^FS"; //      '규격번호

            if (BARCODE_Length.Length < 4)
            {
                Zpl += "^A0N,45^FO267,142^FD" + BARCODE_Length + "^FS"; //         '길이
                Zpl += "^A0N,45^FO327,142^FD" + BARCODE_UOM + "^FS";    //            '길이단위
                Zpl += "^A0N,25^FO260,313^FD" + BARCODE_INSPECTER_NAME + "^FS";// '검사원
            }

            else if (BARCODE_Length.Length == 4)
            {
                Zpl += "^A0N,45^FO248,142^FD" + BARCODE_Length + "^FS";//        '길이
                Zpl += "^A0N,45^FO327,142^FD" + BARCODE_UOM + "^FS";//             '길이단위
                Zpl += "^A0N,25^FO260,313^FD" + BARCODE_INSPECTER_NAME + "^FS";//   '검사원
            }

            else if (BARCODE_Length.Length >= 4)
            {
                Zpl += "^A0N,45^FO233,142^FD" + BARCODE_Length + "^FS";//        '길이
                Zpl += "^A0N,45^FO325,142^FD" + BARCODE_UOM + "^FS";//             '길이단위
                Zpl += "^A0N,25^FO260,313^FD" + BARCODE_INSPECTER_NAME + "^FS";//   '검사원
            }

            Zpl += "^A0N,30^FO130,105^FD" + BARCODE_SIZE_NAME + "^FS"; //      '사이즈
            Zpl += "^A0N,35^FO130,147^FD" + BARCODE_HEAT + "^FS";//           'HEAT_NO

            if (BARCODE_PROGRAM == "10752")
            {
                Zpl += "^FO040,240^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";// '바코드라인
                Zpl += "^A0N,18^FO105,313^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS"; //  '바코드 텍스트
            }
            else
            {
                Zpl += "^A0N,30^FO150,250^FD" + BARCODE_INSPECT_DATE + "^FS"; //      '검사일자
            }

            if (BARCODE_COUNTRY_BASIS == "KS")
            {
                Zpl += "^A0N,23^FO270,061^FD" + BARCODE_APPORVAL_NO + "^FS"; // ; //  '허가번호
            }
            else if (BARCODE_COUNTRY_BASIS == "JIS")
            {
                Zpl += "^A0N,23^FO270,080^FD" + BARCODE_APPORVAL_NO + "^FS";// ; //   '허가번호
            }


            if (BARCODE_PROGRAM == "10752")
            {
                Zpl += "^A0N,25^FO310,313^FD" + BARCODE_BONSU + "^FS"; //; //          '본수
            }
            else
            {
                Zpl += "^A0N,25^FO330,313^FD" + "1" + "^FS"; // ;          '낱본발행본수
            }

            switch (BARCODE_COUNTRY_BASIS)
            {
                case "NO":
                    break;
                case "KS":
                    //'KS 신버전 이미지 : 2009.12.18
                    Zpl += "^FO282,000^FR^XGR:KS_PB51,1,1^FS"; //;
                                                                // '"한국표준협회" 글자 이미지 출력 : 2009.12.18 SHARP
                    Zpl += "^FO266,081^FD^XGR:KSA,1,1^FS"; // ;
                    break;
                case "JIS":
                    Zpl += "^FO278,012^FR^XGJIS_7_1,1,1^FS";
                    break;
            }

            /*
            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }
            else
            {
                BARCODE_CUTTING_LOCAL_COUNT = BARCODE_CUTTING_LOCAL_COUNT + 1;
                if (BARCODE_CUTTING_LOCAL_COUNT == BARCODE_CUTTING_COUNT)
                {
                    Zpl += "^MMC"; // ; '절단
                    BARCODE_CUTTING_LOCAL_COUNT = 0;
                }
            }
            */
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string FREE_STEEL_EXPORT_14()
        {
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            if (BARCODE_BUNDLE_NO != "")
            {
                Zpl += "^A0N,35,35^FO025,022^FDBUNDLE NO : " + BARCODE_BUNDLE_NO + "^FS";
            }

            Zpl += "^A0N,33,33^FO025,058^FD" + BARCODE_MEMO1 + "^FS";
            Zpl += "^A0N,33,33^FO025,094^FD" + BARCODE_MEMO2 + "^FS";
            Zpl += "^A0N,33,33^FO025,130^FD" + BARCODE_MEMO3 + "^FS";
            Zpl += "^A0N,33,33^FO025,166^FD" + BARCODE_MEMO4 + "^FS";
            Zpl += "^A0N,33,33^FO025,202^FD" + BARCODE_MEMO5 + "^FS";
            Zpl += "^A0N,33,33^FO025,238^FD" + BARCODE_MEMO6 + "^FS";
            Zpl += "^A0N,33,33^FO025,274^FD" + BARCODE_MEMO7 + "^FS";
            Zpl += "^A0N,33,33^FO025,310^FD" + BARCODE_MEMO8 + "^FS";
            Zpl += "^A0N,33,33^FO025,346^FD" + BARCODE_MEMO9 + "^FS";
            Zpl += "^A0N,33,33^FO025,382^FD" + BARCODE_MEMO10 + "^FS";
            Zpl += "^A0N,33,33^FO025,418^FD" + BARCODE_MEMO11 + "^FS";
            Zpl += "^A0N,33,33^FO025,454^FD" + BARCODE_MEMO12 + "^FS";
            Zpl += "^A0N,33,33^FO025,490^FD" + BARCODE_MEMO13 + "^FS";
            Zpl += "^A0N,33,33^FO025,526^FD" + BARCODE_MEMO14 + "^FS";
            Zpl += "^XZ";

            return Zpl;
        }
        
        public string FREE_STEEL_EXPORT_10()
        {
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            if (BARCODE_BUNDLE_NO != "")
            {
                Zpl += "^A0N,40,40^FO025,010^FDBUNDLE NO : " + BARCODE_BUNDLE_NO + "^FS";
            }

            Zpl += "^A0N,40,40^FO025,60^FD" + BARCODE_MEMO1 + "^FS";
            Zpl += "^A0N,40,40^FO025,110^FD" + BARCODE_MEMO2 + "^FS";
            Zpl += "^A0N,40,40^FO025,160^FD" + BARCODE_MEMO3 + "^FS";
            Zpl += "^A0N,40,40^FO025,210^FD" + BARCODE_MEMO4 + "^FS";
            Zpl += "^A0N,40,40^FO025,260^FD" + BARCODE_MEMO5 + "^FS";

            Zpl += "^A0N,40,40^FO025,310^FD" + BARCODE_MEMO6 + "^FS";
            Zpl += "^A0N,40,40^FO025,360^FD" + BARCODE_MEMO7 + "^FS";
            Zpl += "^A0N,40,40^FO025,410^FD" + BARCODE_MEMO8 + "^FS";
            Zpl += "^A0N,40,40^FO025,460^FD" + BARCODE_MEMO9 + "^FS";
            Zpl += "^A0N,40,40^FO025,510^FD" + BARCODE_MEMO10 + "^FS";
            //  //2016.03.29 김록석 기사 요청_2줄 추가
            Zpl += "^XZ";

            return Zpl;
        }

        public string NUCLEAR_TAG_NEW()
        {
            var Zpl = "";

            //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
            Zpl += "^XA";
            Zpl += "^PR 2";
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";   //                         '기본좌표

            Zpl += "^A0N,60,60^FO130,45^FD" + BARCODE_BNPP + "^FS";  //'발전기 번호
            Zpl += "^A0N,30,30^FO130,130^FD" + BARCODE_MATERIAL + "^FS";  //'SPEC
            Zpl += "^A0N,40,40^FO130,180^FD" + BARCODE_SIZE_NAME + "^FS";  //'BAR NO(ITEM &ITEM_SIZE)
            Zpl += "^A0N,40,40^FO130,250^FD" + BARCODE_Length + " " + BARCODE_UOM + "^FS";   //       '길이/단위(Feet)
            Zpl += "^A0N,40,40^FO220,250^FD" + BARCODE_Length2 + " " + BARCODE_UOM2 + "^FS"; //       '길이/단위(Inch)
            Zpl += "^A0N,40,40^FO130,305^FD" + BARCODE_HEAT + "^FS";  //         'HEAT_NO
            Zpl += "^A0N,40,40^FO355,305^FD" + BARCODE_BONSU + "^FS"; //         'PCS(번들당본수)
            Zpl += "^A0N,40,40^FO130,370^FD" + BARCODE_WEIGHT + "^FS";        // '중량
            Zpl += "^A0N,40,40^FO355,370^FD" + BARCODE_PRT_BUNDLE_NO + "^FS";     // '번들 NO
            Zpl += "^FO020,425^BY2,3.0^BCN,050,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS"; //'바코드라인
            Zpl += "^A0N,20^FO317,450^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS"; //'바코드 텍스트
            Zpl += "^XZ";

            return Zpl;
        }

        public string DB_EXPORT_HISTORY_SHIPPING_NEW()
        {
            var Zpl = "";
            var MM = "";

            Zpl = "";

            //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
            Zpl += "^XA";
            Zpl += "^PR 2";
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";
            
            // item DB , # 구분 로직적립후에 가능함.
            if (BARCODE_ITEM == "DB")
            {
                MM = " MM";
            }
            else if (BARCODE_ITEM == "#")
            {
                MM = "";
            }
            else
            {
                MM = "";
            }

            Zpl += "^FO001,002^GB445,0,8^FS";
            Zpl += "^FO001,060^GB445,0,2^FS";
            Zpl += "^FO001,112^GB445,0,2^FS";
            Zpl += "^FO001,168^GB452,0,8^FS";

            Zpl += "^FO001,002^GB0,168,8^FS";
            Zpl += "^FO132,002^GB0,168,3^FS";
            Zpl += "^FO445,002^GB0,168,8^FS";

            Zpl += "^A0N,30,32^FO037,022^FD" + "SPEC" + "^FS";
            Zpl += "^A0N,26,28^FO138,024^FD" + BARCODE_SPEC + "^FS";
            Zpl += "^A0N,30,32^FO041,075^FD" + "SIZE" + "^FS";
            if (BARCODE_Length2 == "")
            {
                Zpl += "^A0N,30,32^FO138,075^FD" + BARCODE_ITEM_SIZE + MM + "   " + BARCODE_Length + BARCODE_UOM + "^FS";
            }
            else
            {
                Zpl += "^A0N,30,32^FO138,075^FD" + BARCODE_ITEM_SIZE + MM + "   " + BARCODE_Length + "." + BARCODE_Length2 + BARCODE_UOM + "^FS";
            }
            Zpl += "^A0N,30,32^FO011,128^FD" + "HEAT NO" + "^FS";
            Zpl += "^A0N,34,36^FO138,127^FD" + BARCODE_HEAT + "           " + BARCODE_PRT_BUNDLE_NO.PadLeft(4, ' ') + "^FS";
            Zpl += "^FO020,181^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,20^FO333,200^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        /*
         * 원형강 이력라벨 발행하는 루틴(바인딩)
         * 출력형태- RB 이력라벨 발행 형태(원형강 Tracking 구축관련)
         */
        public string RB_BINDING_TAG()
        {
            var barCode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;

            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH003,003^FS";

            Zpl += "^FO020,030^A0N,36,36^CI13^FR^FD" + "SIZE :  " + "^FS" + "^FS" + "^A0N,36,36^FO150,030^FD" + BARCODE_ITEM + "  " + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^FO020,070^A0N,36,36^CI13^FR^FD" + "GRADE :  " + "^FS" + "^FS" + "^A0N,41,41^FO170,070^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^FO020,110^A0N,36,36^CI13^FR^FD" + "Length :  " + "^FS" + "^FS" + "^A0N,36,36^FO205,110^FD" + BARCODE_Length + " " + BARCODE_UOM + "^FS";
            Zpl += "^FO020,150^A0N,36,36^CI13^FR^FD" + "HEAT.NO :  " + "^FS" + "^FS" + "^A0N,36,36^FO220,150^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^FO020,190^A0N,36,36^CI13^FR^FD" + "WORK_TEAM :  " + "^FS" + "^FS" + "^A0N,36,36^FO270,190^FD" + BARCODE_INSPECTER_NAME + "^FS";
            Zpl += "^FO020,230^A0N,36,36^CI13^FR^FD" + "HYUNDAI STEEL" + "^FS";

            Zpl += "^FO350,030^A0N,43,43^CI13^FR^FD" + BARCODE_FACTORY + "^FS";//공장
            Zpl += "^FO350,110^A0N,43,43^CI13^FR^FD" + BARCODE_RB_USE_GUBUN + "^FS";//RB용도구분
            Zpl += "^FO380,220^A0N,43,43^CI13^FR^FD" + BARCODE_RES_SEQ + "^FS";//RB용도구분

            //12386 프로그램에서 사용 추가 : 2013.05.29 이강현
            //2015.05.08 넘기는 파라메터가 없으므로 일단 무시...
            /*
            if (BARCODE_STEEL != "")
            {
                if (BARCODE_STEEL.Length == 2)
                {
                    Zpl += "^FO360,100^A0N,80,80^CI13^FR^FD" + BARCODE_STEEL + "^FS";
                }
                else
                {
                    Zpl += "^FO370,100^A0N,80,80^CI13^FR^FD" + BARCODE_STEEL + "^FS";
                }
            }
            */

            Zpl += "^BY3,3^FO030,280^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
            Zpl += "^FO40,353^A0N,30,30^CI13^FR^FD" + barCode.Substring(0, 6) + "-" + barCode.Substring(5, 4) + "^FS";//PO+BD
            Zpl += "^FO310,353^A0N,30,30^CI13^FR^FD" + BARCODE_MFG_DATE + "^FS";//2014.07.02 MFG_DATE 날짜

            Zpl += "^BY2,2.0^FO477,030^BCB,080,N,Y,N^FR^FD>:" + barCode + "^FS";//세로 바코드
            Zpl += "^FO565,100^A0B,23,36^CI13^FR^FD" + barCode + "^FS";//세로 바코드 텍스트-1

            
            if (BARCODE_TOTAL_CNT == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";//절단
            }
            
            Zpl += "^XZ";

            //print Mode를 CUTER->TEAR OFF로 전환
            Zpl += "^XA";
            Zpl += "^MMT";
            //Zpl += "^MMC";//절단
            Zpl += "^XZ";
            return Zpl;
        }

        public string BARCODE_TAG()
        {

            var barCode = BARCODE_BUNDLE_NO;
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "003" + "," + "003" + "^FS";


            Zpl += "^BY3,3^FO030,020^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
            Zpl += "^FO040,130^A0N,30,30^CI13^FR^FD" + barCode.Substring(0, 6) + "-" + barCode.Substring(7, 4) + "^FS";//PO+BD

            Zpl += "^MMC";//절단
            Zpl += "^XZ";

            //print Mode를 CUTER->TEAR OFF로 전환
            //    Zpl += "^XA";
            //    Zpl += "^MMT";
            //    Zpl += "^XZ";
            return Zpl;
        }

        /*
         * 원형강 이력라벨 발행하는 루틴(바인딩)
         * 출력형태- RB 이력라벨 발행 형태(원형강 Tracking 구축관련)
         */
        public string RB_BINDING_TAGT()
        {
            var barCode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH003,003^FS";

            Zpl += "^FO020,030^A0N,50,50^CI13^FR^FDSize^FS";
            Zpl += "^FO230,030^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,030^A0N,50,50^CI13^FR^FD" + BARCODE_SIZE_NAME + Space(2) + "mm" + Space(2) + BARCODE_Length + Space(2) + BARCODE_UOM + "^FS";

            Zpl += "^FO020,090^A0N,50,50^CI13^FR^FDSpec^FS";
            Zpl += "^FO230,090^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,090^A0N,50,50^CI13^FR^FD" + BARCODE_STEEL + "^FS";

            Zpl += "^FO020,150^A0N,50,50^CI13^FR^FDHeat No^FS";
            Zpl += "^FO230,150^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,150^A0N,50,50^CI13^FR^FD" + BARCODE_HEAT + "^FS";

            Zpl += "^FO020,210^A0N,50,50^CI13^FR^FDBundle No^FS";
            Zpl += "^FO230,210^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,210^A0N,50,50^CI13^FR^FD" + barCode.Substring(0, 6) + "-" + barCode.Substring(7, 3) + "^FS";

            Zpl += "^FO020,270^A0N,50,50^CI13^FR^FDPCS^FS";
            Zpl += "^FO230,270^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,270^A0N,50,50^CI13^FR^FD" + BARCODE_BONSU + Space(5) + "EA" + "^FS";

            Zpl += "^FO020,330^A0N,50,50^CI13^FR^FDDATE^FS";
            Zpl += "^FO230,330^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,330^A0N,50,50^CI13^FR^FD" + BARCODE_MFG_DATE.Substring(0, 4) + "-" + BARCODE_MFG_DATE.Substring(5, 2) + "-" + BARCODE_MFG_DATE.Substring(8, 2) + "^FS";

            Zpl += "^FO020,400^A0N,50,50^CI13^FR^FDHYUNDAI STEEL^FS";

            Zpl += "^BY4.3,4^FO020,460^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
            Zpl += "^FO20,540^A0N,30,30^CI13^FR^FD" + barCode.Substring(0, 6) + "^FS";
            Zpl += "^FO525,540^A0N,30,30^CI13^FR^FD" + int.Parse(barCode.Substring(7, 3)) + "^FS";

            Zpl += "^FO660,030^BY3,3.0^BCB,070,N,Y,N^FR^FD>:" + barCode + "^FS";//세로 바코드

            //	Zpl += "^FO020,330^A0N,50,50^CI13^FR^FDWEIGHT^FS" ;
            //	Zpl += "^FO230,330^A0N,50,50^CI13^FR^FD:^FS" ;
            //	Zpl += "^FO270,330^A0N,50,50^CI13^FR^FD" + BARCODE_WEIGHT + Space(4) + "KG" + "^FS" ;

            //	Zpl += "^FO020,390^A0N,50,50^CI13^FR^FDDATE^FS" ;
            //	Zpl += "^FO230,390^A0N,50,50^CI13^FR^FD:^FS" ;
            //	Zpl += "^FO270,390^A0N,50,50^CI13^FR^FD" + BARCODE_MFG_DATE + "^FS" ;
            //
            //	Zpl += "^FO020,460^A0N,50,50^CI13^FR^FDHYUNDAI STEEL^FS";
            //
            //    Zpl += "^BY4.3,4^FO020,530^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
            //    Zpl += "^FO20,610^A0N,30,30^CI13^FR^FD" + barCode.substring(0,6) + "^FS";
            //    Zpl += "^FO510,610^A0N,30,30^CI13^FR^FD" + barCode.substring(6,9) + "^FS";
            //
            //    Zpl += "^FO660,030^BY3,3.0^BCB,070,N,Y,N^FR^FD>:" + barCode + "^FS";//세로 바코드

            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC";//절단
            //}

            Zpl += "^XZ";

            //print Mode를 CUTER->TEAR OFF로 전환
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        // 2013.11.29 12385,12387 -> 12386 프로그램 분할
        public string RB_BINDING_TAG_PB4()
        {
            var barCode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "003" + "," + "003" + "^FS";

            Zpl += "^FO020,030^A0N,36,36^CI13^FR^FD" + "SIZE :  " + "^FS" + "^FS" + "^A0N,36,36^FO150,030^FD" + BARCODE_ITEM + "  " + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^FO020,070^A0N,36,36^CI13^FR^FD" + "GRADE :  " + "^FS" + "^FS" + "^A0N,41,41^FO170,070^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^FO020,110^A0N,36,36^CI13^FR^FD" + "Length :  " + "^FS" + "^FS" + "^A0N,36,36^FO205,110^FD" + BARCODE_Length + " " + BARCODE_UOM + "^FS";
            Zpl += "^FO020,150^A0N,36,36^CI13^FR^FD" + "HEAT.NO :  " + "^FS" + "^FS" + "^A0N,36,36^FO220,150^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^FO020,190^A0N,36,36^CI13^FR^FD" + "WORK_TEAM :  " + "^FS" + "^FS" + "^A0N,36,36^FO270,190^FD" + BARCODE_INSPECTER_NAME + "^FS";
            Zpl += "^FO020,230^A0N,36,36^CI13^FR^FD" + "HYUNDAI STEEL" + "^FS";

            Zpl += "^FO350,030^A0N,43,43^CI13^FR^FD" + BARCODE_FACTORY + "^FS";//공장
            Zpl += "^FO350,110^A0N,43,43^CI13^FR^FD" + BARCODE_RB_USE_GUBUN + "^FS";//RB용도구분
            Zpl += "^FO380,220^A0N,43,43^CI13^FR^FD" + BARCODE_RES_SEQ + "^FS";//RB용도구분


            //12386 프로그램에서 사용 추가 : 2013.05.29 이강현
            //12386 프로그램 RE_BINDING_TAG_PB4 변경 2013.11.29 이강현
            if (BARCODE_chgSteel != "")
            {
                if (BARCODE_chgSteel.Length == 2)
                {
                    Zpl += "^FO360,100^A0N,80,80^CI13^FR^FD" + BARCODE_chgSteel + "^FS";
                }
                else
                {
                    Zpl += "^FO370,100^A0N,80,80^CI13^FR^FD" + BARCODE_chgSteel + "^FS";
                }
            }

            Zpl += "^BY3,3^FO030,280^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
                                                                              //	  2013.11.28 이강현 수정
            Zpl += "^FO160,353^A0N,30,30^CI13^FR^FD" + barCode.Substring(0, 6) + "-" + barCode.Substring(7, 3) + "^FS";//PO+BD

            Zpl += "^BY2,2.0^FO477,030^BCB,080,N,Y,N^FR^FD>:" + barCode + "^FS";//세로 바코드
            Zpl += "^FO565,100^A0B,23,36^CI13^FR^FD" + barCode + "^FS";//세로 바코드 텍스트-1

            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC";//절단
            //}

            Zpl += "^XZ";

            //print Mode를 CUTER->TEAR OFF로 전환
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;

        }

        //2013.11.29 12385,12387 -> 12386 프로그램 분할
        public string RB_BINDING_TAG_PB4T()
        {

            var barCode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH003,003^FS";

            Zpl += "^FO020,030^A0N,50,50^CI13^FR^FDSize^FS";
            Zpl += "^FO230,030^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,030^A0N,50,50^CI13^FR^FD" + BARCODE_SIZE_NAME + Space(2) + "mm" + Space(2) + BARCODE_Length + Space(2) + BARCODE_UOM + "^FS";

            Zpl += "^FO020,090^A0N,50,50^CI13^FR^FDSpec^FS";
            Zpl += "^FO230,090^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,090^A0N,50,50^CI13^FR^FD" + BARCODE_STEEL + "^FS";

            Zpl += "^FO020,150^A0N,50,50^CI13^FR^FDHeat No^FS";
            Zpl += "^FO230,150^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,150^A0N,50,50^CI13^FR^FD" + BARCODE_HEAT + "^FS";

            Zpl += "^FO020,210^A0N,50,50^CI13^FR^FDBundle No^FS";
            Zpl += "^FO230,210^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,210^A0N,50,50^CI13^FR^FD" + barCode.Substring(0, 6) + "-" + barCode.Substring(7, 3) + "^FS";

            Zpl += "^FO020,270^A0N,50,50^CI13^FR^FDPCS^FS";
            Zpl += "^FO230,270^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,270^A0N,50,50^CI13^FR^FD" + BARCODE_BONSU + Space(5) + "EA" + "^FS";

            Zpl += "^FO020,330^A0N,50,50^CI13^FR^FDDATE^FS";
            Zpl += "^FO230,330^A0N,50,50^CI13^FR^FD:^FS";
            Zpl += "^FO270,330^A0N,50,50^CI13^FR^FD" + BARCODE_MFG_DATE.Substring(0, 4) + "-" + BARCODE_MFG_DATE.Substring(5, 2) + "-" + BARCODE_MFG_DATE.Substring(8, 2) + "^FS";

            Zpl += "^FO020,400^A0N,50,50^CI13^FR^FDHYUNDAI STEEL^FS";

            Zpl += "^BY4.3,4^FO020,460^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
            Zpl += "^FO20,540^A0N,30,30^CI13^FR^FD" + barCode.Substring(0, 6) + "^FS";
            Zpl += "^FO525,540^A0N,30,30^CI13^FR^FD" + int.Parse(barCode.Substring(7, 3)) + "^FS";

            Zpl += "^FO660,030^BY3,3.0^BCB,070,N,Y,N^FR^FD>:" + barCode + "^FS";//세로 바코드

            //	Zpl += "^FO020,330^A0N,50,50^CI13^FR^FDWEIGHT^FS" ;
            //	Zpl += "^FO230,330^A0N,50,50^CI13^FR^FD:^FS" ;
            //	Zpl += "^FO270,330^A0N,50,50^CI13^FR^FD" + BARCODE_WEIGHT + Space(4) + "KG" + "^FS" ;

            //	Zpl += "^FO020,390^A0N,50,50^CI13^FR^FDDATE^FS" ;
            //	Zpl += "^FO230,390^A0N,50,50^CI13^FR^FD:^FS" ;
            //	Zpl += "^FO270,390^A0N,50,50^CI13^FR^FD" + BARCODE_MFG_DATE + "^FS" ;
            //
            //	Zpl += "^FO020,460^A0N,50,50^CI13^FR^FDHYUNDAI STEEL^FS";
            //
            //    Zpl += "^BY4.3,4^FO020,530^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
            //    Zpl += "^FO20,610^A0N,30,30^CI13^FR^FD" + barCode.substring(0,6) + "^FS";
            //    Zpl += "^FO510,610^A0N,30,30^CI13^FR^FD" + barCode.substring(6,9) + "^FS";
            //
            //    Zpl += "^FO660,030^BY3,3.0^BCB,070,N,Y,N^FR^FD>:" + barCode + "^FS";//세로 바코드

            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC";//절단
            //}

            Zpl += "^XZ";
            return Zpl;
        }

        public string RB_ROUND_LAG()
        {
            var Zpl = "";

            Zpl += "^XA";
            Zpl += "^MMT";

            if (BARCODE_PROGRAM == "21030")
            {
                Zpl += "^FO180,150^FD^XGKI006,1,1^FS";
                Zpl += "^A0N,25,25^FO335,180^FDKSKR07034^FS";
            }

            Zpl += "^A0N120,70^FO275,250^FD" + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length >= 11)
            {
                Zpl += "^A0N170,080^FO080,345^FD" + BARCODE_STEEL + "^FS";   //--2013.07.12, sharp : 44MNSIVS6-H
            }
            else if (BARCODE_STEEL.Length >= 9 || BARCODE_STEEL.Length <= 10)
            {
                Zpl += "^A0N175,090^FO090,345^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^A0N210,110^FO110,345^FD" + BARCODE_STEEL + "^FS";
            }
            Zpl += "^A0N120,70^FO220,460^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N120,70^FO180,545^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^XZ";

            return Zpl;
        }

        public string RB_ROUND_LAGT()
        {
            var Zpl = "";
            //	var Xposition ;
            //	if(BARCODE_STEEL.Length
            Zpl += "^XA";
            Zpl += "^MMT";

            //	if(COUNTRY_BASIS == "JIS"){
            //		Zpl += "^FO180,150^FD^XGKI006,1,1^FS" ;
            //	} else {
            //		Zpl += "^FO180,150^FD^XGKI007,1,1^FS" ;
            //	}

            //KS일 경우
            //	Zpl += "^FO150,010^FD^XGKI007,1,1^FS" ;
            //	Zpl += "^A0N80,40^FO270,040^FDG3101^FS" ;

            //	if(BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != ""){
            //		if(BARCODE_COUNTRY_BASIS == "JIS"){
            //			//JIS일 경우
            //			Zpl += "^FO100,040^FD^XGKI006,1,1^FS" ;
            //			Zpl += "^A0N40,20^FO260,065^FD" + BARCODE_LABEL_SIZE_NAME + "^FS" ;
            //			Zpl += "^A0N40,20^FO260,085^FD" + BARCODE_APPORVAL_NO + "^FS" ;
            //		}
            //	}
            //
            //	Zpl += "^A0N110,60^FO210,120^FD" + BARCODE_SIZE_NAME + "^FS";
            //
            //	if( BARCODE_STEEL.Length >= 11){
            //	 	Zpl += "^A0N120,070^FO040,185^FD" + BARCODE_STEEL + "^FS";   //--2013.07.12, sharp : 44MNSIVS6-H
            //	}else if(BARCODE_STEEL.Length == 10){
            //		Zpl += "^A0N120,070^FO060,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 9){
            // 		Zpl += "^A0N120,070^FO070,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 8){
            // 		Zpl += "^A0N120,070^FO100,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 7){
            // 		Zpl += "^A0N120,070^FO110,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 6){
            // 		Zpl += "^A0N120,070^FO130,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 5){
            // 		Zpl += "^A0N120,070^FO150,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 3 || BARCODE_STEEL.Length == 4){
            // 		Zpl += "^A0N120,070^FO180,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 2){
            // 		Zpl += "^A0N120,070^FO200,185^FD" + BARCODE_STEEL + "^FS";
            // 	}else{
            //	 	Zpl += "^A0N120,070^FO200,185^FD" + BARCODE_STEEL + "^FS";
            // 	}
            //
            //	Zpl += "^A0N120,70^FO080,255^FD" + BARCODE_BUNDLE_NO + "^FS";
            //	Zpl += "^A0N120,70^FO150,325^FD" + BARCODE_HEAT + "^FS";
            //
            // 	Zpl += "^XZ";

            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    //JIS일 경우
                    Zpl += "^FO120,370^FD^XGKI006,1,1^FS";
                    Zpl += "^A0N40,25^FO270,388^FD" + BARCODE_LABEL_SIZE_NAME + "^FS";
                    Zpl += "^A0N40,25^FO270,413^FD" + BARCODE_APPORVAL_NO + "^FS";
                }
            }

            Zpl += "^FO180,225^FD^XGPI060,1,1^FS";
            Zpl += "^A0N110,60^FO220,220^FD" + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 11)
            {
                Zpl += "^A0N120,070^FO20,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                Zpl += "^A0N120,073^FO55,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                Zpl += "^A0N120,073^FO65,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                Zpl += "^A0N120,073^FO85,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                Zpl += "^A0N120,073^FO105,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                Zpl += "^A0N120,073^FO115,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                Zpl += "^A0N120,073^FO145,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                Zpl += "^A0N120,073^FO165,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                Zpl += "^A0N120,073^FO185,150^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 2)
            {
                Zpl += "^A0N120,073^FO205,150^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^A0N110,060^FO25,160^FD" + BARCODE_STEEL + "^FS";
            }

            Zpl += "^A0N120,45^FO150,280^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N120,45^FO190,330^FD" + BARCODE_HEAT + "^FS";

            Zpl += "^XZ";

            return Zpl;
        }

        //태그 사이즈 80파이
        public string RB_ROUND_BIGLAGT()
        {
            var Zpl = "";

            Zpl += "^XA";
            Zpl += "^MMT";

            //	if(COUNTRY_BASIS == "JIS"){
            //		Zpl += "^FO180,150^FD^XGKI006,1,1^FS" ;
            //	} else {
            //		Zpl += "^FO180,150^FD^XGKI007,1,1^FS" ;
            //	}

            //KS일 경우
            //	Zpl += "^FO300,80^FD^XGKI007,1,1^FS" ;
            //	Zpl += "^A0N80,40^FO400,110^FDA1234^FS" ;

            //	//JIS일 경우
            //	Zpl += "^FO250,080^FD^XGKI006,1,1^FS" ;
            //	Zpl += "^A0N80,40^FO400,110^FDG3101^FS" ;

            //	if(BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != ""){
            //		if(BARCODE_COUNTRY_BASIS == "JIS"){
            //			//JIS일 경우
            //			Zpl += "^FO230,080^FD^XGKI006,1,1^FS" ;
            //			Zpl += "^A0N80,40^FO400,090^FD" + BARCODE_LABEL_SIZE_NAME + "^FS" ;
            //			Zpl += "^A0N80,40^FO400,130^FD" + BARCODE_APPORVAL_NO + "^FS" ;
            //		}
            //	}
            //
            //	Zpl += "^A0N180,110^FO300,200^FD" + BARCODE_SIZE_NAME + "^FS";
            //
            //	if( BARCODE_STEEL.Length == 11){
            //	 	Zpl += "^A0N190,120^FO030,300^FD" + BARCODE_STEEL + "^FS";
            //	}else if(BARCODE_STEEL.Length == 10){
            //		Zpl += "^A0N190,120^FO070,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 9){
            // 		Zpl += "^A0N190,120^FO090,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 8){
            // 		Zpl += "^A0N190,120^FO110,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 7){
            // 		Zpl += "^A0N190,120^FO160,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 6){
            // 		Zpl += "^A0N190,120^FO190,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 5){
            // 		Zpl += "^A0N190,120^FO220,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 4){
            // 		Zpl += "^A0N190,120^FO250,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 3){
            // 		Zpl += "^A0N190,120^FO280,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 2){
            // 		Zpl += "^A0N190,120^FO320,300^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length > 11){
            // 		Zpl += "^A0N170,100^FO30,310^FD" + BARCODE_STEEL + "^FS";
            // 	}else{
            // 		Zpl += "^A0N190,120^FO320,300^FD" + BARCODE_STEEL + "^FS";
            // 	}
            //
            //	Zpl += "^A0N180,110^FO150,430^FD" + BARCODE_BUNDLE_NO + "^FS";
            //	Zpl += "^A0N180,110^FO250,550^FD" + BARCODE_HEAT + "^FS";
            //
            // 	Zpl += "^XZ";

            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    //JIS일 경우
                    Zpl += "^FO260,650^FD^XGKI006,1,1^FS";
                    Zpl += "^A0N80,32^FO410,665^FD" + BARCODE_LABEL_SIZE_NAME + "^FS";
                    Zpl += "^A0N80,32^FO410,695^FD" + BARCODE_APPORVAL_NO + "^FS";
                }
            }

            Zpl += "^FO270,405^FD^XGPI100,1,1^FS";      //파이
            Zpl += "^A0N180,125^FO340,390^FD" + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 11)
            {
                Zpl += "^A0N190,110^FO040,280^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                Zpl += "^A0N190,120^FO075,280^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                Zpl += "^A0N190,120^FO105,280^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                Zpl += "^A0N190,120^FO125,280^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                Zpl += "^A0N190,170^FO050,250^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                Zpl += "^A0N190,170^FO110,250^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                Zpl += "^A0N190,170^FO150,250^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                Zpl += "^A0N190,170^FO200,250^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                Zpl += "^A0N190,170^FO250,250^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 2)
            {
                Zpl += "^A0N190,170^FO090,250^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length > 11)
            {
                Zpl += "^A0N190,90^FO060,280^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^A0N190,120^FO320,200^FD" + BARCODE_STEEL + "^FS";
            }

            Zpl += "^A0N200,80^FO230,500^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N200,80^FO300,580^FD" + BARCODE_HEAT + "^FS";

            Zpl += "^XZ";

            return Zpl;
        }

        public string RB_ROUND_MID()
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            if (BARCODE_PROGRAM == "21030")
            {
                Zpl += "^FO085,015^FD^XGKI006,1,1^FS";
                Zpl += "^A0N,15,15^FO235,050^FDKSKR07034^FS";

                Zpl += "^FO440,015^FD^XGKI006,1,1^FS";
                Zpl += "^A0N,15,15^FO590,050^FDKSKR07034^FS";

                Zpl += "^A0N080,40^FO140,090^FD" + BARCODE_SIZE_NAME + Space(5) + "MM" + "^FS";
                Zpl += "^A0N080,40^FO490,090^FD" + BARCODE_SIZE_NAME + Space(5) + "MM" + "^FS";

                if (BARCODE_STEEL.Length > 7)
                {
                    if (BARCODE_STEEL.Length >= 9)
                    {
                        Zpl += "^A0N130,50^FO040,130^FD" + BARCODE_STEEL + "^FS";
                        Zpl += "^A0N130,50^FO395,130^FD" + BARCODE_STEEL + "^FS";
                    }
                    else
                    {
                        Zpl += "^A0N150,60^FO060,130^FD" + BARCODE_STEEL + "^FS";
                        Zpl += "^A0N150,60^FO415,130^FD" + BARCODE_STEEL + "^FS";
                    }
                }
                else
                {
                    Zpl += "^A0N150,70^FO060,130^FD" + BARCODE_STEEL + "^FS";
                    Zpl += "^A0N150,70^FO415,130^FD" + BARCODE_STEEL + "^FS";
                }

                Zpl += "^A0N060,35^FO120,185^FD" + BARCODE_HEAT + Space(26) + BARCODE_HEAT + "^FS";
                Zpl += "^A0N060,35^FO095,220^FD" + BARCODE_BUNDLE_NO + Space(19) + BARCODE_BUNDLE_NO + "^FS";
            }
            else
            {
                Zpl += "^A0N080,40^FO090,050^FD" + BARCODE_SIZE_NAME + Space(5) + "MM" + Space(18) + BARCODE_SIZE_NAME + Space(5) + "MM" + "^FS" + BARCODE_UOM + "^FS";

                if (BARCODE_STEEL.Length > 7)
                {
                    if (BARCODE_STEEL.Length >= 9)
                    {
                        Zpl += "^A0N130,50^FO040,110^FD" + BARCODE_STEEL + "^FS";
                        Zpl += "^A0N130,50^FO395,110^FD" + BARCODE_STEEL + "^FS";
                    }
                    else
                    {
                        Zpl += "^A0N150,60^FO060,110^FD" + BARCODE_STEEL + "^FS";
                        Zpl += "^A0N150,60^FO415,110^FD" + BARCODE_STEEL + "^FS";
                    }
                }
                else
                {
                    Zpl += "^A0N150,70^FO060,110^FD" + BARCODE_STEEL + "^FS";
                    Zpl += "^A0N150,70^FO415,110^FD" + BARCODE_STEEL + "^FS";
                }

                Zpl += "^A0N060,35^FO120,185^FD" + BARCODE_HEAT + Space(26) + BARCODE_HEAT + "^FS";
                Zpl += "^A0N060,35^FO095,220^FD" + BARCODE_BUNDLE_NO + Space(19) + BARCODE_BUNDLE_NO + "^FS";
            }

            Zpl += "^XZ";
            return Zpl;
        }

        //2015.08.05 40파이 태그 _ 파이 인쇄된 용지 기준
        public string RB_ROUND_MID1()
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            //	if(BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != ""){
            //		if(BARCODE_COUNTRY_BASIS == "JIS"){
            //			//JIS일 경우
            //			Zpl += "^FO230,080^FD^XGKI006,1,1^FS" ;
            //			Zpl += "^A0N80,40^FO400,090^FD" + BARCODE_LABEL_SIZE_NAME + "^FS" ;
            //			Zpl += "^A0N80,40^FO400,130^FD" + BARCODE_APPORVAL_NO + "^FS" ;
            //		}
            //	}
            //
            //	Zpl += "^A0N080,040^FO130,075^FD" + BARCODE_SIZE_NAME + "^FS";
            //	Zpl += "^A0N080,040^FO490,075^FD" + BARCODE_SIZE_NAME + "^FS";
            //
            //	if( BARCODE_STEEL.Length == 11){
            //	 	Zpl += "^A0N125,45^FO020,130^FD" + BARCODE_STEEL + "^FS";
            //	 	Zpl += "^A0N125,45^FO375,130^FD" + BARCODE_STEEL + "^FS";
            //	}else if(BARCODE_STEEL.Length == 10){
            //		Zpl += "^A0N130,50^FO040,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO395,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 9){
            // 		Zpl += "^A0N130,50^FO050,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO405,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 8){
            // 		Zpl += "^A0N130,50^FO060,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO415,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 7){
            // 		Zpl += "^A0N130,50^FO070,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO425,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 6){
            // 		Zpl += "^A0N130,50^FO080,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO435,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 5){
            // 		Zpl += "^A0N130,50^FO090,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO445,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 4){
            // 		Zpl += "^A0N130,50^FO110,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO465,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 3){
            // 		Zpl += "^A0N130,50^FO130,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO485,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length == 2){
            // 		Zpl += "^A0N130,50^FO150,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO505,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else if(BARCODE_STEEL.Length > 11){
            // 		Zpl += "^A0N120,40^FO040,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N120,40^FO395,130^FD" + BARCODE_STEEL + "^FS";
            // 	}else{
            // 		Zpl += "^A0N130,50^FO170,130^FD" + BARCODE_STEEL + "^FS";
            //		Zpl += "^A0N130,50^FO525,130^FD" + BARCODE_STEEL + "^FS";
            // 	}
            //
            //	Zpl += "^A0N060,035^FO115,185^FD" + BARCODE_HEAT + "^FS";
            //	Zpl += "^A0N060,035^FO475,185^FD" + BARCODE_HEAT + "^FS";
            //
            //	Zpl += "^A0N060,035^FO080,220^FD" + BARCODE_BUNDLE_NO + "^FS";
            //	Zpl += "^A0N060,035^FO440,220^FD" + BARCODE_BUNDLE_NO + "^FS";
            //
            // 	Zpl += "^XZ";

            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    //JIS일 경우
                    Zpl += "^A0N,22,18^FO095,273^FD" + "KSA" + "^FS";   //KSA
                    Zpl += "^A0N,22,18^FO455,273^FD" + "KSA" + "^FS";
                    Zpl += "^FO127,263^FD^XGKI040,1,1^FS";                  //JIS
                    Zpl += "^FO487,263^FD^XGKI040,1,1^FS";
                    Zpl += "^A0N22,14^FO165,268^FD" + BARCODE_LABEL_SIZE_NAME + "^FS";
                    Zpl += "^A0N22,14^FO525,268^FD" + BARCODE_LABEL_SIZE_NAME + "^FS";
                    Zpl += "^A0N22,14^FO165,283^FD" + BARCODE_APPORVAL_NO + "^FS";
                    Zpl += "^A0N22,14^FO525,283^FD" + BARCODE_APPORVAL_NO + "^FS";
                }
            }

            Zpl += "^FO120,162^FD^XGPI040,1,1^FS";      //파이
            Zpl += "^FO480,162^FD^XGPI040,1,1^FS";

            Zpl += "^A0N080,045^FO155,160^FD" + " " + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^A0N080,045^FO515,160^FD" + " " + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 11)
            {
                Zpl += "^A0N125,45^FO030,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO385,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                Zpl += "^A0N125,45^FO040,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO395,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                Zpl += "^A0N130,45^FO050,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO405,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                Zpl += "^A0N130,45^FO060,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO415,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                Zpl += "^A0N130,45^FO070,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO425,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                Zpl += "^A0N130,60^FO070,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO415,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                Zpl += "^A0N130,60^FO090,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO435,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                Zpl += "^A0N130,60^FO110,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO455,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                Zpl += "^A0N130,60^FO120,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO465,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 2)
            {
                Zpl += "^A0N130,60^FO150,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO485,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length > 11)
            {
                Zpl += "^A0N120,40^FO025,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N120,40^FO375,115^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^A0N130,50^FO170,075^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO525,075^FD" + BARCODE_STEEL + "^FS";
            }

            Zpl += "^A0N060,030^FO090,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N060,030^FO450,205^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^A0N060,030^FO120,235^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N060,030^FO480,235^FD" + BARCODE_HEAT + "^FS";

            Zpl += "^XZ";

            return Zpl;
        }

        public string RB_ROUND_MID2()
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //


            Zpl += "^FO020,150^BY2,2^BCN,050,N,Y,N^FR^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^FO480,162^FD^XGPI040,1,1^FS";

            Zpl += "^A0N130,36^FO80,250^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N080,045^FO505,160^FD" + " " + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 11)
            {
                //Zpl += "^A0N125,45^FO030,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO385,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                //Zpl += "^A0N125,45^FO040,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO395,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                //Zpl += "^A0N130,45^FO050,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO405,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                //Zpl += "^A0N130,45^FO060,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO415,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                //Zpl += "^A0N130,45^FO070,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO425,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                //Zpl += "^A0N130,60^FO070,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO415,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                //Zpl += "^A0N130,60^FO090,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO435,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                //Zpl += "^A0N130,60^FO110,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO455,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                //Zpl += "^A0N130,60^FO120,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO465,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 2)
            {
                //Zpl += "^A0N130,60^FO150,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO485,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length > 11)
            {
                //Zpl += "^A0N120,40^FO025,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N120,40^FO375,115^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                //Zpl += "^A0N130,50^FO170,075^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO525,075^FD" + BARCODE_STEEL + "^FS";
            }

            //Zpl += "^A0N060,030^FO090,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^A0N060,030^FO450,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^A0N060,030^FO480,205^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N060,040^FO460,200^FD" + BARCODE_HEAT + "^FS";

            //Zpl += "^A0N060,030^FO120,235^FD" + BARCODE_HEAT + "^FS";
            //Zpl += "^A0N060,030^FO480,235^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N060,030^FO450,235^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^A0N060,040^FO460,265^FD" + BARCODE_BONSU + " PCS^FS";

            Zpl += "^XZ";

            return Zpl;
        }

        public string RB_ROUND_MID3()
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //


            Zpl += "^FO008,150^BY2,2^BCN,050,N,Y,N^FR^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^FO480,162^FD^XGPI040,1,1^FS";
            Zpl += "^FO480,162^FD^XGPI040,1,1^FS";

            Zpl += "^A0N130,36^FO80,250^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N080,045^FO505,160^FD" + " " + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 11)
            {
                //Zpl += "^A0N125,45^FO030,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO385,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                //Zpl += "^A0N125,45^FO040,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO395,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                //Zpl += "^A0N130,45^FO050,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO405,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                //Zpl += "^A0N130,45^FO060,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO415,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                //Zpl += "^A0N130,45^FO070,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO425,115^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                //Zpl += "^A0N130,60^FO070,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO415,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                //Zpl += "^A0N130,60^FO090,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO435,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                //Zpl += "^A0N130,60^FO110,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO455,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                //Zpl += "^A0N130,60^FO120,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO465,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 2)
            {
                //Zpl += "^A0N130,60^FO150,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO485,105^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length > 11)
            {
                //Zpl += "^A0N120,40^FO025,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N120,40^FO375,115^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                //Zpl += "^A0N130,50^FO170,075^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO525,075^FD" + BARCODE_STEEL + "^FS";
            }

            //Zpl += "^A0N060,030^FO090,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^A0N060,030^FO450,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N060,040^FO460,200^FD" + BARCODE_HEAT + "^FS";

            //Zpl += "^A0N060,030^FO120,235^FD" + BARCODE_HEAT + "^FS";
            //Zpl += "^A0N060,030^FO480,235^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N060,030^FO450,235^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^A0N060,040^FO460,265^FD" + BARCODE_BONSU + " PCS^FS";

            Zpl += "^XZ";

            return Zpl;
        }


        public string RB_ROUND_MID4()
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //


            Zpl += "^FO020,150^BY2,2^BCN,050,N,Y,N^FR^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^FO480,162^FD^XGPI040,1,1^FS";
            Zpl += "^FO480,112^FD^XGPI040,1,1^FS";

            Zpl += "^A0N130,36^FO80,250^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N080,045^FO505,110^FD" + " " + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 11)
            {
                //Zpl += "^A0N125,45^FO030,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO385,180^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                //Zpl += "^A0N125,45^FO040,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,45^FO395,180^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                //Zpl += "^A0N130,45^FO050,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO405,180^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                //Zpl += "^A0N130,45^FO060,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO415,180^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                //Zpl += "^A0N130,45^FO070,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,45^FO425,180^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                //Zpl += "^A0N130,60^FO070,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,55^FO415,175^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                //Zpl += "^A0N130,60^FO090,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO435,175^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                //Zpl += "^A0N130,60^FO110,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO455,175^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                //Zpl += "^A0N130,60^FO120,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO465,175^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 2)
            {
                //Zpl += "^A0N130,60^FO150,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO485,175^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length > 11)
            {
                //Zpl += "^A0N120,40^FO025,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N120,40^FO375,180^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                //Zpl += "^A0N130,50^FO170,075^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO525,075^FD" + BARCODE_STEEL + "^FS";
            }

            //Zpl += "^A0N060,030^FO090,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^A0N060,030^FO450,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N060,040^FO460,225^FD" + BARCODE_HEAT + "^FS";

            //Zpl += "^A0N060,030^FO120,235^FD" + BARCODE_HEAT + "^FS";
            //Zpl += "^A0N060,030^FO480,235^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N060,030^FO450,265^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^A0N060,030^FO480,145^FD" + BARCODE_BONSU + " PCS^FS";

            Zpl += "^XZ";

            return Zpl;
        }

        public string RB_ROUND_MID5(int Count)
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //


            Zpl += "^FO020,150^BY2,2^BCN,050,N,Y,N^FR^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^FO480,162^FD^XGPI040,1,1^FS";
            Zpl += "^FO470,72^FD^XGPI040,1,1^FS";
            //Zpl += "^FO440,62^FD^XGPI040,2,2^FS";

            Zpl += "^A0N130,38^FO80,240^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N080,055^FO495,70^FD" + " " + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 11)
            {
                //Zpl += "^A0N125,45^FO030,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,50^FO385,170^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                //Zpl += "^A0N125,45^FO040,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N125,50^FO395,170^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                //Zpl += "^A0N130,45^FO050,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO405,170^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                //Zpl += "^A0N130,45^FO060,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO415,170^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                //Zpl += "^A0N130,45^FO070,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO425,170^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                //Zpl += "^A0N130,60^FO070,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,55^FO415,165^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                //Zpl += "^A0N130,60^FO090,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO435,165^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                //Zpl += "^A0N130,60^FO110,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO455,165^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                //Zpl += "^A0N130,60^FO120,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO465,165^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 2)
            {
                //Zpl += "^A0N130,60^FO150,105^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,60^FO485,165^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length > 11)
            {
                //Zpl += "^A0N120,40^FO025,115^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N120,40^FO375,185^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                //Zpl += "^A0N130,50^FO170,075^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N130,50^FO525,075^FD" + BARCODE_STEEL + "^FS";
            }

            //Zpl += "^A0N060,030^FO090,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            //Zpl += "^A0N060,030^FO450,205^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N060,040^FO460,220^FD" + BARCODE_HEAT + "^FS";

            //Zpl += "^A0N060,030^FO120,235^FD" + BARCODE_HEAT + "^FS";
            //Zpl += "^A0N060,030^FO480,235^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N060,030^FO450,265^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^A0N060,040^FO410,125^FD" + BARCODE_BONSU + " PCS  " + BARCODE_Length + "M^FS";
            //Zpl += "^A0N060,040^FO460,125^FD" + BARCODE_BONSU + " PCS^FS";


            if (Count-1 == BARCODE_CUTTING_LOCAL_COUNT)
            {
                Zpl += "^MMC";
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }
            else
            {
                BARCODE_CUTTING_LOCAL_COUNT = BARCODE_CUTTING_LOCAL_COUNT + 1;

                if (BARCODE_CUTTING_LOCAL_COUNT == BARCODE_CUTTING_COUNT)
                {
                    Zpl += "^MMC";
                    BARCODE_CUTTING_LOCAL_COUNT = 0;
                }
            }

            Zpl += "^XZ";


            //'print Mode를 CUTER->TEAR OFF로 전환
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }
        //태그 사이즈 60파이
        public string RB_ROUND_MIDT()
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            //	Zpl += "^A0N50,30^FO090,60^FD" + BARCODE_SIZE_NAME + "^FS" ;
            //	Zpl += "^A0N50,30^FO315,60^FD" + BARCODE_SIZE_NAME + "^FS" ;
            //	Zpl += "^A0N50,30^FO540,60^FD" + BARCODE_SIZE_NAME + "^FS" ;
            //
            //	if(BARCODE_STEEL.Length == 2){
            //		Zpl += "^A0N50,30^FO090,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO315,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO540,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 3){
            //		Zpl += "^A0N50,30^FO080,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO305,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO530,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 4){
            //		Zpl += "^A0N50,30^FO075,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO300,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO525,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 5){
            //		Zpl += "^A0N50,30^FO065,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO290,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO515,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 6){
            //		Zpl += "^A0N50,30^FO055,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO280,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO505,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 7){
            //		Zpl += "^A0N50,30^FO045,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO270,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO495,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 8 || BARCODE_STEEL.Length == 9){
            //		Zpl += "^A0N50,30^FO035,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO260,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO485,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 10){
            //		Zpl += "^A0N50,30^FO030,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO255,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO480,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}else if(BARCODE_STEEL.Length == 11){
            //		Zpl += "^A0N50,30^FO015,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO235,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,30^FO460,90^FD" + BARCODE_STEEL + "^FS" ;
            //	} else {
            //		//12자리까지 측정해났음.. 추후 강종길이가 길어지면 라벨에 전부 찍히지 않을 경우가 발생~~~
            //		Zpl += "^A0N50,25^FO020,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,25^FO240,90^FD" + BARCODE_STEEL + "^FS" ;
            //		Zpl += "^A0N50,25^FO465,90^FD" + BARCODE_STEEL + "^FS" ;
            //	}
            //
            //	Zpl += "^A0N50,30^FO030,120^FD" + BARCODE_BUNDLE_NO + "^FS" ;
            //	Zpl += "^A0N50,30^FO255,120^FD" + BARCODE_BUNDLE_NO + "^FS" ;
            //	Zpl += "^A0N50,30^FO480,120^FD" + BARCODE_BUNDLE_NO + "^FS" ;
            //
            //	Zpl += "^A0N50,30^FO070,150^FD" + BARCODE_HEAT + "^FS" ;
            //	Zpl += "^A0N50,30^FO295,150^FD" + BARCODE_HEAT + "^FS" ;
            //	Zpl += "^A0N50,30^FO520,150^FD" + BARCODE_HEAT + "^FS" ;
            //
            //	Zpl += "^XZ" ;

            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    //JIS일 경우
                    Zpl += "^A0N,22,18^FO076,163^FD" + "KSA" + "^FS";       //KSA
                    Zpl += "^A0N,22,18^FO301,163^FD" + "KSA" + "^FS";
                    Zpl += "^A0N,22,18^FO526,163^FD" + "KSA" + "^FS";
                    Zpl += "^FO105,153^FD^XGKI025,1,1^FS";                      //JIS
                    Zpl += "^FO330,153^FD^XGKI025,1,1^FS";
                    Zpl += "^FO555,153^FD^XGKI025,1,1^FS";
                }
            }

            Zpl += "^FO080,090^FD^XGPI025,1,1^FS";                              //파이
            Zpl += "^FO305,090^FD^XGPI025,1,1^FS";
            Zpl += "^FO530,090^FD^XGPI025,1,1^FS";

            Zpl += "^A0N50,33^FO105,088^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^A0N50,33^FO330,088^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^A0N50,33^FO555,088^FD" + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_STEEL.Length == 2)
            {
                Zpl += "^A0N50,40^FO090,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO315,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO540,53^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                Zpl += "^A0N50,40^FO080,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO305,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO530,53^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                Zpl += "^A0N50,40^FO070,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO295,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO520,53^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                Zpl += "^A0N50,40^FO055,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO280,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO500,53^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                Zpl += "^A0N50,40^FO042,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO267,53^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,40^FO492,53^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                Zpl += "^A0N50,30^FO055,60^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,30^FO280,60^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,30^FO505,60^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 8 || BARCODE_STEEL.Length == 9)
            {
                Zpl += "^A0N50,30^FO045,60^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,30^FO270,60^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,30^FO495,60^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                Zpl += "^A0N50,30^FO035,60^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,30^FO260,60^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,30^FO485,60^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length == 11)
            {
                Zpl += "^A0N50,25^FO035,65^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,25^FO260,65^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,25^FO485,65^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                //12자리까지 측정해났음.. 추후 강종길이가 길어지면 라벨에 전부 찍히지 않을 경우가 발생~~~
                Zpl += "^A0N50,25^FO015,50^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,25^FO240,50^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N50,25^FO465,50^FD" + BARCODE_STEEL + "^FS";
            }

            Zpl += "^A0N50,24^FO050,118^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N50,24^FO275,118^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N50,24^FO495,118^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^A0N50,20^FO080,138^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N50,20^FO300,138^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N50,20^FO525,138^FD" + BARCODE_HEAT + "^FS";

            Zpl += "^XZ";

            return Zpl;
        }

        public string RB_ROUND_SML()
        {

            var Zpl = ""; //
            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            //     if(BARCODE_PROGRAM == "21030"){
            //		  Zpl += "^A0N015,15^FO005,005^FD^XGKI006,1,1^FS" ;
            //		  Zpl += "^A0N015,15^FO155,020^FDKSKR07034^FS" ;
            //
            //		  Zpl += "^A0N015,15^FO190,005^FD^XGKI006,1,1^FS" ;
            //		  Zpl += "^A0N015,15^FO340,020^FDKSKR07034^FS" ;
            //
            //		  Zpl += "^A0N015,15^FO375,005^FD^XGKI006,1,1^FS" ;
            //		  Zpl += "^A0N015,15^FO525,020^FDKSKR07034^FS" ;
            //
            //		  Zpl += "^A0N015,15^FO560,005^FD^XGKI006,1,1^FS" ;
            //		  Zpl += "^A0N015,15^FO710,020^FDKSKR07034^FS" ;
            //     }

            Zpl += "^A0N030,20^FO055,050^FD" + BARCODE_SIZE_NAME + Space(2) + "MM" + "^FS"; //     '사이즈
            Zpl += "^A0N030,20^FO240,050^FD" + BARCODE_SIZE_NAME + Space(2) + "MM" + "^FS"; //     '사이즈
            Zpl += "^A0N030,20^FO425,050^FD" + BARCODE_SIZE_NAME + Space(2) + "MM" + "^FS"; //     '사이즈
            Zpl += "^A0N030,20^FO610,050^FD" + BARCODE_SIZE_NAME + Space(2) + "MM" + "^FS"; //     '사이즈

            if (BARCODE_STEEL.Length > 6)
            {
                if (BARCODE_STEEL.Length >= 9)
                {
                    Zpl += "^A0N040,28^FO030,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                    Zpl += "^A0N040,28^FO215,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                    Zpl += "^A0N040,28^FO400,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                    Zpl += "^A0N040,28^FO580,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                }
                else
                {
                    Zpl += "^A0N045,30^FO030,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                    Zpl += "^A0N045,30^FO215,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                    Zpl += "^A0N045,30^FO400,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                    Zpl += "^A0N045,30^FO580,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                }
            }
            else
            {
                Zpl += "^A0N045,35^FO030,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N045,35^FO215,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N045,35^FO400,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N045,35^FO580,070^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }

            Zpl += "^A0N030,20^FO070,105^FD" + BARCODE_HEAT + "^FS"; //    'HEAT
            Zpl += "^A0N030,20^FO255,105^FD" + BARCODE_HEAT + "^FS"; //    'HEAT
            Zpl += "^A0N030,20^FO440,105^FD" + BARCODE_HEAT + "^FS"; //    'HEAT
            Zpl += "^A0N030,20^FO625,105^FD" + BARCODE_HEAT + "^FS"; //    'HEAT
            Zpl += "^A0N030,20^FO052,125^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO
            Zpl += "^A0N030,20^FO237,125^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO
            Zpl += "^A0N030,20^FO422,125^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO
            Zpl += "^A0N030,20^FO607,125^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO
            Zpl += "^XZ"; //

            return Zpl;
        }

        //태그 사이즈 15파이
        public string RB_ROUND_SMLT()
        {

            var Zpl = ""; //
            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            //    ^A0N30,15^FO060,30^FD15.0^FS
            //    ^A0N30,15^FO205,30^FD15.0^FS
            //    ^A0N30,15^FO350,30^FD15.0^FS
            //    ^A0N30,15^FO495,30^FD15.0^FS
            //
            //    ^A0N25,17^FO015,48^FD44MNSIVS6-H^FS
            //    ^A0N25,17^FO160,48^FD44MNSIVS6-H^FS
            //    ^A0N25,17^FO305,48^FD44MNSIVS6-H^FS
            //    ^A0N25,17^FO450,48^FD44MNSIVS6-H^FS
            //
            //    ^A0N30,15^FO025,66^FDADB133301A^FS
            //    ^A0N30,15^FO170,66^FDADB133301A^FS
            //    ^A0N30,15^FO315,66^FDADB133301A^FS
            //    ^A0N30,15^FO460,66^FDADB133301A^FS
            //
            //    ^A0N30,15^FO048,84^FD1E0004^FS
            //    ^A0N30,15^FO191,84^FD1E0004^FS
            //    ^A0N30,15^FO336,84^FD1E0004^FS
            //    ^A0N30,15^FO479,84^FD1E0004^FS

            //    Zpl += "^A0N40,16^FO060,30^FD" + BARCODE_SIZE_NAME + "^FS" ; //     '사이즈
            //    Zpl += "^A0N40,16^FO205,30^FD" + BARCODE_SIZE_NAME + "^FS" ; //     '사이즈
            //    Zpl += "^A0N40,16^FO350,30^FD" + BARCODE_SIZE_NAME + "^FS" ; //     '사이즈
            //    Zpl += "^A0N40,16^FO495,30^FD" + BARCODE_SIZE_NAME + "^FS" ; //     '사이즈
            //
            //    if( BARCODE_STEEL.Length == 2 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(9) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(9) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(9) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(9) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 3 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(8) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(8) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(8) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(8) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 4 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(7) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(7) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(7) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(7) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 5 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(6) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(6) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(6) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(6) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 6 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(5) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(5) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(5) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(5) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 7 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(4) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(4) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(4) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(4) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 8 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(3) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(3) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(3) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(3) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 9 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(2) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(2) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(2) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(2) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 10 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + Space(1) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + Space(1) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + Space(1) + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + Space(1) + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else if( BARCODE_STEEL.Length == 11 ){
            //    	Zpl += "^A0N35,16^FO020,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //    } else {
            //        Zpl += "^A0N35,16^FO020,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO165,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO310,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //        Zpl += "^A0N35,16^FO455,48^FD" + BARCODE_STEEL + "^FS" ; //    '강종
            //   }
            //
            //    Zpl += "^A0N40,16^FO030,66^FD" + BARCODE_BUNDLE_NO + "^FS" ; //    '번들 NO
            //    Zpl += "^A0N40,16^FO175,66^FD" + BARCODE_BUNDLE_NO + "^FS" ; //    '번들 NO
            //    Zpl += "^A0N40,16^FO320,66^FD" + BARCODE_BUNDLE_NO + "^FS" ; //    '번들 NO
            //    Zpl += "^A0N40,16^FO465,66^FD" + BARCODE_BUNDLE_NO + "^FS" ; //    '번들 NO
            //
            //    Zpl += "^A0N40,16^FO048,84^FD" + BARCODE_HEAT + "^FS" ; //    'HEAT
            //    Zpl += "^A0N40,16^FO193,84^FD" + BARCODE_HEAT + "^FS" ; //    'HEAT
            //    Zpl += "^A0N40,16^FO338,84^FD" + BARCODE_HEAT + "^FS" ; //    'HEAT
            //    Zpl += "^A0N40,16^FO483,84^FD" + BARCODE_HEAT + "^FS" ; //    'HEAT
            //
            //    Zpl += "^XZ" ; //

            Zpl += "^FO052,50^FD^XGPI015,1,1^FS";   //파이
            Zpl += "^FO197,50^FD^XGPI015,1,1^FS";
            Zpl += "^FO342,50^FD^XGPI015,1,1^FS";
            Zpl += "^FO487,50^FD^XGPI015,1,1^FS";

            //    ^A0N30,15^FO060,30^FD15.0^FS
            //    ^A0N30,15^FO205,30^FD15.0^FS
            //    ^A0N30,15^FO350,30^FD15.0^FS
            //    ^A0N30,15^FO495,30^FD15.0^FS
            //
            //    ^A0N25,17^FO015,48^FD44MNSIVS6-H^FS
            //    ^A0N25,17^FO160,48^FD44MNSIVS6-H^FS
            //    ^A0N25,17^FO305,48^FD44MNSIVS6-H^FS
            //    ^A0N25,17^FO450,48^FD44MNSIVS6-H^FS
            //
            //    ^A0N30,15^FO025,66^FDADB133301A^FS
            //    ^A0N30,15^FO170,66^FDADB133301A^FS
            //    ^A0N30,15^FO315,66^FDADB133301A^FS
            //    ^A0N30,15^FO460,66^FDADB133301A^FS
            //
            //    ^A0N30,15^FO048,84^FD1E0004^FS
            //    ^A0N30,15^FO191,84^FD1E0004^FS
            //    ^A0N30,15^FO336,84^FD1E0004^FS
            //    ^A0N30,15^FO479,84^FD1E0004^FS

            Zpl += "^A0N40,20^FO066,50^FD" + BARCODE_SIZE_NAME + "^FS"; //     '사이즈
            Zpl += "^A0N40,20^FO211,50^FD" + BARCODE_SIZE_NAME + "^FS"; //     '사이즈
            Zpl += "^A0N40,20^FO356,50^FD" + BARCODE_SIZE_NAME + "^FS"; //     '사이즈
            Zpl += "^A0N40,20^FO501,50^FD" + BARCODE_SIZE_NAME + "^FS"; //     '사이즈

            if (BARCODE_STEEL.Length == 2)
            {
                Zpl += "^A0N35,25^FO055,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO200,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO345,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO490,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 3)
            {
                Zpl += "^A0N35,25^FO045,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO190,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO335,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO480,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 4)
            {
                Zpl += "^A0N35,25^FO040,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO185,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO330,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO475,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 5)
            {
                Zpl += "^A0N35,25^FO032,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO177,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO322,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO467,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 6)
            {
                Zpl += "^A0N35,25^FO027,28^FD" + BARCODE_STEEL + "^FS"; //    '강종		//완료
                Zpl += "^A0N35,25^FO172,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO317,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,25^FO462,28^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 7)
            {
                Zpl += "^A0N35,16^FO040,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO180,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO325,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO475,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 8)
            {
                Zpl += "^A0N35,16^FO035,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO175,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO320,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO455,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 9)
            {
                Zpl += "^A0N35,16^FO035,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO180,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO325,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO470,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 10)
            {
                Zpl += "^A0N35,16^FO030,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO175,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO320,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO465,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else if (BARCODE_STEEL.Length == 11)
            {
                Zpl += "^A0N35,16^FO020,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO165,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO310,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N35,16^FO455,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }
            else
            {
                Zpl += "^A0N20,13^FO025,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N20,13^FO170,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N20,13^FO315,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
                Zpl += "^A0N20,13^FO460,35^FD" + BARCODE_STEEL + "^FS"; //    '강종
            }

            Zpl += "^A0N45,20^FO015,70^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO
            Zpl += "^A0N45,20^FO160,70^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO
            Zpl += "^A0N45,20^FO305,70^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO
            Zpl += "^A0N45,20^FO450,70^FD" + BARCODE_BUNDLE_NO + "^FS"; //    '번들 NO

            Zpl += "^A0N40,16^FO045,90^FD" + BARCODE_HEAT + "^FS"; //    'HEAT
            Zpl += "^A0N40,16^FO190,90^FD" + BARCODE_HEAT + "^FS"; //    'HEAT
            Zpl += "^A0N40,16^FO335,90^FD" + BARCODE_HEAT + "^FS"; //    'HEAT
            Zpl += "^A0N40,16^FO480,90^FD" + BARCODE_HEAT + "^FS"; //    'HEAT

            Zpl += "^XZ"; //

            return Zpl;
        }

        public string RB_SQUARE()
        {

            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);
            Zpl += "^XA"; //
            Zpl += "^MMT"; //
            Zpl += ("^A0N200,65^FO050,030^FD" + BARCODE_COMPANY + "^FS"); //  '회사
            Zpl += ("^A0N105,38^FO365,105^FD" + BARCODE_SIZE_NAME + Space(14) + BARCODE_Length + "^FS"); //    '사이즈,길이
            Zpl += ("^A0N105,38^FO370,148^FD" + BARCODE_STEEL + "^FS"); //    '강종
            Zpl += ("^A0N105,38^FO370,192^FD" + BARCODE_HEAT + "^FS"); //    'HEAT
            Zpl += ("^A0N105,38^FO430,235^FD" + BARCODE_BONSU + "^FS"); //          '본수
            Zpl += ("^A0N105,38^FO370,278^FD" + BARCODE_WEIGHT + "^FS"); //         '중량
            Zpl += ("^A0N105,38^FO370,322^FD" + BARCODE_INSPECT_DATE + "^FS"); // '일자
            Zpl += ("^FO106,455^BY3^BCN,090 ^FD" + BARCODE_BUNDLE_NO + "^FS"); // '바코드라인
            Zpl += ("^A0N090,32^FO130,660^FD" + BARCODE_SIZE_NAME + Space(10) + BARCODE_Length + Space(28) + BARCODE_WEIGHT + "^FS"); //   '사이즈,길이,중량
            Zpl += ("^A0N090,32^FO130,705^FD" + BARCODE_STEEL + Space(10) + BARCODE_ITEM + Space(15) + BARCODE_BONSU + "^FS"); //    '강종,ITEM,본수
            Zpl += ("^A0N090,32^FO050,750^FD" + BARCODE_AFTER_ROUTING + "^FS"); // '공정
            Zpl += ("^A0N090,32^FO680,750^FD" + BARCODE_SURFACE_LEVEL + "^FS"); // '표면등급
            Zpl += ("^A0N090,32^FO050,795^FD" + BARCODE_ITEM_USE + "^FS"); // '제품용도
            Zpl += ("^FO160,750^BY3^BCN,060 ^FD" + BARCODE_BUNDLE_NO + "^FS"); // '바코드라인
            Zpl += ("^A0N090,32^FO680,795^FD" + BARCODE_SIRF_CODE + "^FS"); // '합부코드

            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC"; // '절단
            //}
            Zpl += "^XZ"; //
            Zpl += "^XA"; //
            Zpl += "^MMT"; //
            Zpl += "^XZ"; //
            return Zpl;

        }

        public string RB_SQUARET()
        { //기존 라벨과 동일한 크기

            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            //KS일 경우
            //	Zpl += "^FO565,010^FD^XGKI007,1,1^FS" ;
            //	Zpl += "^A0N70,30^FO570,105^FDA1234^FS" ;

            //JIS일 경우
            //	Zpl += "^FO550,010^FD^XGKI006,1,1^FS" ;
            //	Zpl += "^A0N70,30^FO570,90^FDG3101^FS" ;
            Zpl += "^FO550,005^FD^XGKI006,1,1^FS";
            Zpl += "^A0N70,30^FO570,80^FDG3101^FS";

            //	Zpl += "^BY2,2.0^FO580,140^BCB,60,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS" ; 	// '세로바코드라인
            Zpl += "^BY3,3.0^FO580,110^BCB,60,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";    // '세로바코드라인
                                                                                                    //	Zpl += "^FO580,420^A0B,23,36^CI13^FR^FD" + BARCODE_BUNDLE_NO + "^FS" ;  			//'세로 바코드 텍스트-1

            Zpl += "^A0N30,30^FO030,030^FDCustomer^FS";

            if (BARCODE_COMPANY.Length <= 14)
            {
                Zpl += "^A0N200,65^FO030,060^FD" + BARCODE_COMPANY + "^FS";                      // '회사
            }
            else
            {
                Zpl += "^A0N150,50^FO030,060^FD" + BARCODE_COMPANY + "^FS";                      // '회사
            }
            Zpl += "^A0N105,38^FO230,125^FD" + BARCODE_STEEL + "^FS";                        // '강종
            Zpl += "^A0N105,38^FO230,170^FD" + BARCODE_SIZE_NAME + "^FS";                    // '사이즈
            Zpl += "^A0N105,38^FO370,170^FD" + BARCODE_Length + "^FS";                       // '길이
            Zpl += "^A0N105,38^FO230,215^FD" + BARCODE_HEAT + "^FS";                         // 'HEAT
            Zpl += "^A0N105,38^FO230,260^FD" + BARCODE_BUNDLE_NO + "^FS";                    // '번들번호
            Zpl += "^A0N105,38^FO230,305^FD" + BARCODE_BONSU + "^FS";                        // '본수
            Zpl += "^A0N105,38^FO230,350^FD" + BARCODE_WEIGHT + "^FS";                       // '중량

            Zpl += "^A0N105,38^FO230,395^FD" + BARCODE_INSPECT_DATE + "^FS";                 // '일자

            Zpl += "^FO030,510^BY3^BCN,055,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";       // '바코드라인

            //	Zpl += "^FO555,450^A0N,75,75^FDRB^FS"; 													// 'RB
            //	Zpl += "^FO554,520^A0N,20,20^FDRound Bar^FS"; 											// 'Round Bar

            Zpl += "^FO565,560^A0N,75,75^FDRB^FS";                                                  // 'RB
            Zpl += "^FO564,620^A0N,20,20^FDRound Bar^FS";                                           // 'Round Bar

            Zpl += "^A0N090,32^FO130,680^FD" + BARCODE_SIZE_NAME + "^FS";                    // '사이즈
            Zpl += "^A0N090,32^FO240,680^FD" + BARCODE_Length + "^FS";                       // '길이
            Zpl += "^A0N090,32^FO520,680^FD" + BARCODE_WEIGHT + "^FS";                       // '중량

            Zpl += "^A0N090,32^FO130,727^FD" + BARCODE_STEEL + "^FS";                        // '강종
            Zpl += "^A0N090,32^FO520,727^FD" + BARCODE_BONSU + "^FS";                        // '본수

            Zpl += "^FO030,775^BY3^BCN,050,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";       // '바코드라인
            Zpl += "^A0N090,32^FO030,830^FD" + BARCODE_HEAT + "^FS";                         // '아래부분 HEAT
            Zpl += "^A0N090,32^FO295,830^FD" + BARCODE_BUNDLE_NO + "^FS";                    // '아래부분 BUNDLE_NO

            Zpl += "^A0N090,35^FO480,775^FD" + BARCODE_AFTER_ROUTING + "^FS";                // '공정
            Zpl += "^A0N090,35^FO480,815^FD" + BARCODE_SURFACE_LEVEL + "^FS";                // '표면등급
            Zpl += "^A0N090,35^FO560,815^FD" + BARCODE_SIRF_CODE + "^FS";                    // '합부코드

            Zpl += "^A0N090,35^FO600,810^FD" + BARCODE_INSPECTOR + "^FS";                    // '사번

            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC"; // '절단
            //}

            Zpl += "^XZ"; //
            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            Zpl += "^XZ"; //
            return Zpl;

        }

        public string RB_SQUARET3()
        { //기존 라벨과 동일한 크기_20150611 태그폼 변경

            var Zpl = "";

            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA"; //
            Zpl += "^MMC"; //

            //	//JIS 마크만 출력하도록 함..
            //	if(BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != ""){
            //		if(BARCODE_COUNTRY_BASIS == "JIS"){
            //			Zpl += "^FO290,435^FD^XGKI006,1,1^FS" ;
            //			Zpl += "^A0N20,30^FO440,450^FD" + BARCODE_LABEL_SIZE_NAME + "^FS" ;
            //			Zpl += "^A0N20,30^FO440,480^FD" + BARCODE_APPORVAL_NO + "^FS" ;
            //		}
            //	}
            //
            //	//KS일 경우
            ////	Zpl += "^FO565,010^FD^XGKI007,1,1^FS" ;
            ////	Zpl += "^A0N70,30^FO570,105^FDA1234^FS" ;
            //
            //	//JIS일 경우
            ////	Zpl += "^FO550,010^FD^XGKI006,1,1^FS" ;
            ////	Zpl += "^A0N70,30^FO570,90^FDG3101^FS" ;
            //
            //
            //	Zpl += "^BY3,2.0^FO635,30^BCB,60,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS" ; 	// '세로바코드라인
            //
            //	Zpl += "^A0N30,30^FO070,030^FDCustomer^FS";
            //
            //	if(BARCODE_COMPANY.Length <= 14){
            //		Zpl += "^A0N200,65^FO070,065^FD" + BARCODE_COMPANY + "^FS"; 					// '회사
            //	} else {
            //		Zpl += "^A0N150,50^FO070,065^FD" + BARCODE_COMPANY + "^FS"; 					// '회사
            //	}
            //	Zpl += "^A0N105,38^FO250,130^FD" + BARCODE_STEEL + "^FS"; 						// '강종
            //	Zpl += "^A0N105,38^FO250,175^FD" + BARCODE_SIZE_NAME + "^FS";					// '사이즈
            //	Zpl += "^A0N105,38^FO440,175^FD" + BARCODE_Length + "^FS"; 						// '길이
            //	Zpl += "^A0N105,38^FO250,220^FD" + BARCODE_HEAT + "^FS"; 						// 'HEAT
            //	Zpl += "^A0N105,38^FO250,265^FD" + BARCODE_BUNDLE_NO + "^FS"; 					// '번들번호
            //	Zpl += "^A0N105,38^FO250,310^FD" + BARCODE_BONSU + "^FS"; 						// '본수
            //	Zpl += "^A0N105,38^FO250,355^FD" + BARCODE_WEIGHT + "^FS"; 						// '중량
            //
            //	Zpl += "^A0N105,38^FO250,400^FD" + BARCODE_INSPECT_DATE + "^FS"; 				// '일자
            //
            //	Zpl += "^FO030,515^BY3,3^BCN,055,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS"; 	// '바코드라인
            //
            //	Zpl += "^FO620,515^A0N,75,75^FDRB^FS"; 													// 'RB
            //	Zpl += "^FO619,575^A0N,20,20^FDRound Bar^FS"; 											// 'Round Bar
            //
            //	//아랫부분
            //	Zpl += "^A0N090,32^FO160,683^FD" + BARCODE_SIZE_NAME + "^FS"; 					// '사이즈
            //	Zpl += "^A0N090,32^FO290,683^FD" + BARCODE_Length + "^FS"; 						// '길이
            //	Zpl += "^A0N090,32^FO520,683^FD" + BARCODE_WEIGHT + "^FS"; 						// '중량
            //
            //	//2015.12.22 UP-SET 단조용으로 스크립트 개선
            ////	Zpl += "^A0N090,32^FO160,735^FD" + BARCODE_STEEL + "^FS"; 						// '강종
            //	Zpl += "^A0N090,32^FO160,735^FD" + BARCODE_STEEL1 + "^FS"; 						// '강종
            //	Zpl += "^A0N090,32^FO520,735^FD" + BARCODE_BONSU + "^FS"; 						// '본수
            //
            //	Zpl += "^FO025,777^BY3,3^BCN,050,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS" ; 	// '바코드라인
            //	Zpl += "^A0N090,32^FO023,830^FD" + BARCODE_HEAT+ "^FS" ; 						// '아래부분 HEAT
            //	Zpl += "^A0N090,32^FO330,830^FD" + BARCODE_BUNDLE_NO + "^FS" ; 					// '아래부분 BUNDLE_NO
            //
            //	Zpl += "^A0N090,35^FO500,777^FD" + BARCODE_AFTER_ROUTING + "^FS" ; 				// '공정
            //	Zpl += "^A0N090,35^FO500,817^FD" + BARCODE_SURFACE_LEVEL + "^FS"; 				// '표면등급
            //	Zpl += "^A0N090,35^FO560,797^FD" + BARCODE_SIRF_CODE + "^FS" ; 					// '합부코드
            //
            //	Zpl += "^A0N090,35^FO600,797^FD" + BARCODE_INSPECTOR + "^FS" ; 					// '사번

            //한글폰트 찍기 위한 추가
            Zpl += "^SEE:UHANGUL.DAT^FS";
            Zpl += "^PON^FS";
            Zpl += "^CW1,E:KFONT3.FNT^FS";

            //2015.12.29 라벨 양식 변경으로 인한 스크립트 적용.
            //JIS 마크만 출력하도록 함..
            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    Zpl += "^A0N,20,22^FO547,15^FD" + BARCODE_APPORVAL_NO + "^FS";                       //KSKR07034
                    Zpl += "^A0N,40,42^FO565,40^FDKSA^FS";                                                                              //KSA
                    Zpl += "^FO565,073^FD^XGR:JIS100,1,1^FS";                                                                           //JIS 마크
                }
            }

            Zpl += "^BY3,2.0^FO660,20^BCB,60,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";         // '세로바코드라인

            Zpl += "^A0N30,20^FO042,018^FDCustomer^FS";

            if (BARCODE_COMPANY.Length <= 14)
            {
                Zpl += "^A0N200,45^FO040,040^FD" + BARCODE_COMPANY + "^FS";                                  // '회사
            }
            else
            {
                Zpl += "^A0N200,45^FO040,040^FD" + BARCODE_COMPANY + "^FS";                                  // '회사
            }
            Zpl += "^A0N105,38^FO240,090^FD" + BARCODE_STEEL + "^FS";                                        // '강종
            Zpl += "^A0N105,38^FO240,140^FD" + BARCODE_SIZE_NAME + "^FS";                                // '사이즈
                                                                                                                //	Zpl += "^A0N105,38^FO375,140^FD" + BARCODE_Length + "^FS"; 									// '길이
            Zpl += "^A0N105,38^FO365,140^FD" + BARCODE_Length + "^FS";                                   // '길이
            Zpl += "^A0N105,38^FO240,190^FD" + BARCODE_HEAT + "^FS";                                         // 'HEAT
            Zpl += "^A0N105,38^FO240,235^FD" + BARCODE_BUNDLE_NO + "^FS";                                // '번들번호
            Zpl += "^A0N105,38^FO240,285^FD" + BARCODE_BONSU + "^FS";                                        // '본수
            Zpl += "^A0N105,38^FO240,330^FD" + BARCODE_WEIGHT + "^FS";                                       // '중량

            Zpl += "^A0N105,38^FO240,380^FD" + BARCODE_INSPECT_DATE + "^FS";                             // '일자

            //2015.12.29 라벨 양식 변경으로 인한 스크립트 적용.
            //JIS 마크만 출력하도록 함..
            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    Zpl += "^A0N20,30^FO240,425^FD" + BARCODE_LABEL_SIZE_NAME + "^FS";                       //JIS G 4051
                }
            }

            //Zpl += "^A1N20,25^FO240,455^CI26^FD" + BARCODE_LABEL_SIZE_NAME + "^FS" ;			//기계 구조용 탄소 강재

            Zpl += "^FO030,505^BY3,3^BCN,055,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";         // '바코드라인

            Zpl += "^FO555,465^A0N,75,75^FDRB^FS";                                                                                  // 'RB
            Zpl += "^FO555,525^A0N,20,20^FDRound Bar^FS";                                                                       // 'Round Bar
            Zpl += "^FO537,545^A0N,20,20^FDMade In Korea^FS";                                                                   // 'Made In Korea

            //아랫부분
            Zpl += "^A0N090,32^FO150,663^FD" + BARCODE_SIZE_NAME + "^FS";                                // '사이즈
                                                                                                                //	Zpl += "^A0N090,32^FO255,663^FD" + BARCODE_Length + "^FS"; 									// '길이
            Zpl += "^A0N090,32^FO245,663^FD" + BARCODE_Length + "^FS";                                   // '길이
            Zpl += "^A0N090,32^FO520,663^FD" + BARCODE_WEIGHT + "^FS";                                       // '중량

            //2015.12.22 UP-SET 단조용으로 스크립트 개선
            //	Zpl += "^A0N090,32^FO160,735^FD" + BARCODE_STEEL + "^FS"; 										// '강종
            Zpl += "^A0N090,32^FO150,720^FD" + BARCODE_STEEL1 + "^FS";                                       // '강종
            Zpl += "^A0N090,32^FO520,720^FD" + BARCODE_BONSU + "^FS";                                        // '본수

            Zpl += "^FO040,777^BY3,3^BCN,050,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";         // '바코드라인

            Zpl += "^A0N090,32^FO038,830^FD" + BARCODE_HEAT + "^FS";                                         // '아래부분 HEAT
                                                                                                                    //	Zpl += "^A0N090,32^FO305,830^FD" + BARCODE_BUNDLE_NO + "^FS" ; 								// '아래부분 BUNDLE_NO
            Zpl += "^A0N090,32^FO320,830^FD" + BARCODE_BUNDLE_NO + "^FS";                                // '아래부분 BUNDLE_NO

            Zpl += "^A0N090,35^FO500,777^FD" + BARCODE_AFTER_ROUTING + "^FS";                        // '공정
            Zpl += "^A0N090,35^FO500,817^FD" + BARCODE_SURFACE_LEVEL + "^FS";                        // '표면등급
            Zpl += "^A0N090,35^FO560,817^FD" + BARCODE_SIRF_CODE + "^FS";                                // '합부코드

            Zpl += "^A0N090,35^FO600,797^FD" + BARCODE_INSPECTOR + "^FS";                                // '이니셜


            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC"; // '절단
            //}

            Zpl += "^XZ"; //
            Zpl += "^XA"; //
            Zpl += "^MMC"; //

            Zpl += "^XZ"; //
            return Zpl;

        }

        public string RB_SQUARET2()
        { //신규 라벨

            var Zpl = "";

            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            //KS일 경우
            //	Zpl += "^FO300,80^FD^XGKI007,1,1^FS" ;
            //	Zpl += "^A0N80,40^FO400,110^FDA1234^FS" ;

            //JIS일 경우
            Zpl += "^FO610,010^FD^XGKI006,1,1^FS";
            Zpl += "^A0N70,30^FO650,90^FDG3101^FS";

            Zpl += "^BY3,3.0^FO650,140^BCB,70,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";    // '세로바코드라인
                                                                                                    //	Zpl += "^FO580,420^A0B,23,36^CI13^FR^FD" + BARCODE_BUNDLE_NO + "^FS" ;  			//'세로 바코드 텍스트-1

            Zpl += "^A0N30,30^FO030,040^FDCustomer^FS";

            if (BARCODE_COMPANY.Length <= 14)
            {
                Zpl += "^A0N200,65^FO030,080^FD" + BARCODE_COMPANY + "^FS";                      // '회사
            }
            else
            {
                Zpl += "^A0N150,50^FO030,080^FD" + BARCODE_COMPANY + "^FS";                      // '회사
            }
            Zpl += "^A0N105,38^FO280,165^FD" + BARCODE_STEEL + "^FS";                        // '강종
            Zpl += "^A0N105,38^FO280,225^FD" + BARCODE_SIZE_NAME + "^FS";                    // '사이즈
            Zpl += "^A0N105,38^FO430,225^FD" + BARCODE_Length + "^FS";                       // '길이
            Zpl += "^A0N105,38^FO280,285^FD" + BARCODE_HEAT + "^FS";                         // 'HEAT
            Zpl += "^A0N105,38^FO280,345^FD" + BARCODE_BUNDLE_NO + "^FS";                    // '번들번호
            Zpl += "^A0N105,38^FO280,405^FD" + BARCODE_BONSU + "^FS";                        // '본수
            Zpl += "^A0N105,38^FO280,465^FD" + BARCODE_WEIGHT + "^FS";                       // '중량

            Zpl += "^A0N105,38^FO280,525^FD" + BARCODE_INSPECT_DATE + "^FS";                 // '일자

            Zpl += "^FO030,680^BY4,3.0^BCN,070,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";       // '바코드라인

            Zpl += "^^FO630,600^A0N,75,75^FDRB^FS";                                                     // 'RB
            Zpl += "^FO628,680^A0N,20,20^FDRound Bar^FS";                                           // 'Round Bar

            Zpl += "^A0N090,32^FO130,880^FD" + BARCODE_SIZE_NAME + "^FS";                    // '사이즈
            Zpl += "^A0N090,32^FO260,880^FD" + BARCODE_Length + "^FS";                       // '길이
            Zpl += "^A0N090,32^FO520,880^FD" + BARCODE_WEIGHT + "^FS";                       // '중량

            Zpl += "^A0N090,32^FO130,940^FD" + BARCODE_STEEL + "^FS";                        // '강종
            Zpl += "^A0N090,32^FO520,940^FD" + BARCODE_BONSU + "^FS";                        // '본수

            Zpl += "^FO020,990^BY3^BCN,050,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";       // '바코드라인
            Zpl += "^A0N090,32^FO020,1050^FD" + BARCODE_HEAT + "^FS";                        // '아래부분 HEAT
            Zpl += "^A0N090,32^FO275,1050^FD" + BARCODE_BUNDLE_NO + "^FS";                   // '아래부분 BUNDLE_NO

            Zpl += "^A0N090,40^FO480,990^FD" + BARCODE_AFTER_ROUTING + "^FS";                // '공정
            Zpl += "^A0N090,40^FO480,1040^FD" + BARCODE_SURFACE_LEVEL + "^FS";               // '표면등급
            Zpl += "^A0N090,40^FO560,1040^FD" + BARCODE_SIRF_CODE + "^FS";                   // '합부코드

            Zpl += "^A0N090,30^FO600,1020^FD" + BARCODE_INSPECTOR + "^FS";                   // '사번

            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //   Zpl += "^MMC"; // '절단
            //}

            Zpl += "^XZ"; //
            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            Zpl += "^XZ"; //
            return Zpl;

        }

        public string RB_SQUAREHOLE()
        { //현대자동차 직납 신규 라벨

            var Zpl = "";

            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA"; //
            //Zpl += "^MMT"; //

            //	Zpl += "^BY3,2.0^FO615,110^BCB,60,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS" ; 	// '세로바코드라인
            ////	Zpl += "^FO580,420^A0B,23,36^CI13^FR^FD" + BARCODE_BUNDLE_NO + "^FS" ;  				//'세로 바코드 텍스트-1
            //
            //	Zpl += "^A0N30,30^FO020,055^FDCustomer^FS";
            //
            //	if(BARCODE_COMPANY.Length <= 14){
            //		Zpl += "^A0N200,65^FO020,090^FD" + BARCODE_COMPANY + "^FS"; 						// '회사
            //	} else {
            //		Zpl += "^A0N150,50^FO020,090^FD" + BARCODE_COMPANY + "^FS"; 						// '회사
            //	}
            //	Zpl += "^A0N105,38^FO200,150^FD" + BARCODE_STEEL + "^FS"; 							// '강종
            //	Zpl += "^A0N105,38^FO200,195^FD" + BARCODE_SIZE_NAME + "^FS";					// '사이즈
            //	Zpl += "^A0N105,38^FO390,195^FD" + BARCODE_Length + "^FS"; 						// '길이
            //	Zpl += "^A0N105,38^FO200,240^FD" + BARCODE_HEAT + "^FS"; 							// 'HEAT
            //	Zpl += "^A0N105,38^FO200,285^FD" + BARCODE_BUNDLE_NO + "^FS"; 					// '번들번호
            //	Zpl += "^A0N105,38^FO200,330^FD" + BARCODE_BONSU + "^FS"; 							// '본수
            //	Zpl += "^A0N105,38^FO200,375^FD" + BARCODE_WEIGHT + "^FS"; 							// '중량
            //
            //	Zpl += "^A0N105,38^FO200,420^FD" + BARCODE_INSPECT_DATE + "^FS"; 				// '일자
            //
            //	Zpl += "^FO005,520^BY4,3^BCN,055,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS"; 		// '바코드라인
            //
            //	Zpl += "^FO605,560^A0N,75,75^FDRB^FS"; 																		// 'RB
            //	Zpl += "^FO604,620^A0N,20,20^FDRound Bar^FS"; 															// 'Round Bar
            //
            //	//아랫부분
            //	Zpl += "^A0N090,32^FO120,700^FD" + BARCODE_SIZE_NAME + "^FS"; 					// '사이즈
            //	Zpl += "^A0N090,32^FO250,700^FD" + BARCODE_Length + "^FS"; 						// '길이
            //	Zpl += "^A0N090,32^FO510,700^FD" + BARCODE_WEIGHT + "^FS"; 							// '중량
            //	//2015.12.22 UP-SET 단조용으로 스크립트 개선
            ////	Zpl += "^A0N090,32^FO120,747^FD" + BARCODE_STEEL + "^FS"; 							// '강종
            //	Zpl += "^A0N090,32^FO120,747^FD" + BARCODE_STEEL1 + "^FS"; 							// '강종
            //	Zpl += "^A0N090,32^FO510,747^FD" + BARCODE_BONSU + "^FS"; 							// '본수
            //
            //	Zpl += "^FO005,787^BY3,3^BCN,050,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS" ; 		// '바코드라인
            //	Zpl += "^A0N090,32^FO005,840^FD" + BARCODE_HEAT+ "^FS" ; 								// '아래부분 HEAT
            //	Zpl += "^A0N090,32^FO280,840^FD" + BARCODE_BUNDLE_NO + "^FS" ; 						// '아래부분 BUNDLE_NO
            //
            //	Zpl += "^A0N090,35^FO450,788^FD" + BARCODE_AFTER_ROUTING + "^FS" ; 				// '공정
            //	Zpl += "^A0N090,35^FO450,828^FD" + BARCODE_SURFACE_LEVEL + "^FS"; 				// '표면등급
            //	Zpl += "^A0N090,35^FO530,808^FD" + BARCODE_SIRF_CODE + "^FS" ; 						// '합부코드
            //
            //	Zpl += "^A0N090,35^FO570,808^FD" + BARCODE_INSPECTOR + "^FS" ; 						// '사번


            //한글폰트 찍기 위한 추가
            Zpl += "^SEE:UHANGUL.DAT^FS";
            Zpl += "^PON^FS";
            Zpl += "^CW1,E:KFONT3.FNT^FS";

            //2015.12.29 라벨 양식 변경으로 인한 스크립트 적용.
            //JIS 마크만 출력하도록 함..
            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    Zpl += "^A0N,20,22^FO547,15^FD" + BARCODE_APPORVAL_NO + "^FS";                       //KSKR07034
                    Zpl += "^A0N,40,42^FO565,40^FDKSA^FS";                                                                              //KSA
                    Zpl += "^FO565,073^FD^XGR:JIS100,1,1^FS";                                                                           //JIS 마크
                }
            }

            Zpl += "^BY3,2.0^FO640,40^BCB,60,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";         // '세로바코드라인
            Zpl += "^A0N30,20^FO012,049^FDCustomer^FS";

            if (BARCODE_COMPANY.Length <= 14)
            {
                Zpl += "^A0N200,45^FO010,071^FD" + BARCODE_COMPANY + "^FS";                                  // '회사
            }
            else
            {
                Zpl += "^A0N200,45^FO010,071^FD" + BARCODE_COMPANY + "^FS";                                  // '회사
            }
            Zpl += "^A0N105,38^FO220,125^FD" + BARCODE_STEEL + "^FS";                                        // '강종
            Zpl += "^A0N105,38^FO220,167^FD" + BARCODE_SIZE_NAME + "^FS";                                // '사이즈
                                                                                                                //	Zpl += "^A0N105,38^FO375,140^FD" + BARCODE_Length + "^FS"; 									// '길이
            Zpl += "^A0N105,38^FO350,167^FD" + BARCODE_Length + "^FS";                                   // '길이
            Zpl += "^A0N105,38^FO220,217^FD" + BARCODE_HEAT + "^FS";                                         // 'HEAT
            Zpl += "^A0N105,38^FO220,265^FD" + BARCODE_BUNDLE_NO + "^FS";                                // '번들번호
            Zpl += "^A0N105,38^FO220,312^FD" + BARCODE_BONSU + "^FS";                                        // '본수
            Zpl += "^A0N105,38^FO220,357^FD" + BARCODE_WEIGHT + "^FS";                                       // '중량

            Zpl += "^A0N105,38^FO220,407^FD" + BARCODE_INSPECT_DATE + "^FS";                             // '일자

            //2015.12.29 라벨 양식 변경으로 인한 스크립트 적용.
            //JIS 마크만 출력하도록 함..
            if (BARCODE_COUNTRY_BASIS != "" && BARCODE_LABEL_SIZE_NAME != "" && BARCODE_APPORVAL_NO != "")
            {
                if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    Zpl += "^A0N20,30^FO220,455^FD" + BARCODE_LABEL_SIZE_NAME + "^FS";                       //JIS G 4051
                }
            }

            //Zpl += "^A1N20,25^FO220,485^CI26^FD" + BARCODE_LABEL_SIZE_NAME + "^FS" ;			//기계 구조용 탄소 강재

            Zpl += "^FO000,523^BY3,3^BCN,055,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";         // '바코드라인

            Zpl += "^FO530,482^A0N,75,75^FDRB^FS";                                                                                  // 'RB
            Zpl += "^FO530,542^A0N,20,20^FDRound Bar^FS";                                                                       // 'Round Bar
            Zpl += "^FO512,562^A0N,20,20^FDMade In Korea^FS";                                                                   // 'Made In Korea

            //아랫부분
            Zpl += "^A0N090,32^FO130,687^FD" + BARCODE_SIZE_NAME + "^FS";                                // '사이즈
                                                                                                                //	Zpl += "^A0N090,32^FO225,687^FD" + BARCODE_Length + "^FS"; 									// '길이
            Zpl += "^A0N090,32^FO225,687^FD" + BARCODE_Length + "^FS";                                   // '길이
            Zpl += "^A0N090,32^FO500,687^FD" + BARCODE_WEIGHT + "^FS";                                       // '중량

            //2015.12.22 UP-SET 단조용으로 스크립트 개선
            //	Zpl += "^A0N090,32^FO160,735^FD" + BARCODE_STEEL + "^FS"; 										// '강종
            Zpl += "^A0N090,32^FO130,735^FD" + BARCODE_STEEL1 + "^FS";                                       // '강종
            Zpl += "^A0N090,32^FO500,735^FD" + BARCODE_BONSU + "^FS";                                        // '본수

            Zpl += "^FO015,790^BY3,3^BCN,050,N,Y,N^FR^FD>:" + BARCODE_BUNDLE_NO + "^FS";         // '바코드라인

            Zpl += "^A0N090,32^FO015,845^FD" + BARCODE_HEAT + "^FS";                                         // '아래부분 HEAT
            Zpl += "^A0N090,32^FO285,845^FD" + BARCODE_BUNDLE_NO + "^FS";                                // '아래부분 BUNDLE_NO
                                                                                                                //	Zpl += "^A0N090,32^FO320,830^FD" + BARCODE_BUNDLE_NO + "^FS" ; 								// '아래부분 BUNDLE_NO

            Zpl += "^A0N090,35^FO470,792^FD" + BARCODE_AFTER_ROUTING + "^FS";                        // '공정
            Zpl += "^A0N090,35^FO470,832^FD" + BARCODE_SURFACE_LEVEL + "^FS";                        // '표면등급
            Zpl += "^A0N090,35^FO530,832^FD" + BARCODE_SIRF_CODE + "^FS";                                // '합부코드
            Zpl += "^A0N090,35^FO570,812^FD" + BARCODE_INSPECTOR + "^FS";                                // '이니셜

            Zpl += "^MMC";
            Zpl += "^XZ";

            return Zpl;
        }

        //'==================================================================================================
        //' R/B 후처리 JIS Mark용 네모라벨 발행하는 루틴(임의 값을 입력한 후 출력됨)
        //'--------------------------------------------------------------------------------------------------
        //' FrmName : 적용되는 Form Name  ( frm21030 ) - 2006.11.16, SHARP
        //'==================================================================================================
        public string RB_SQUARE_JIS()
        {

            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^MMT";

            Zpl += "^A0N,15,15^FO637,040^FDKSKR07034^FS"; //'--//2010.12.30, SHARP : 신규 JIS 심사 대비 허가번호
                                                          //	'Zpl &= "^A0N,20,20^FO641,030^FDKSKR^FS" & vbCrLf '--//2010.12.30, SHARP : 신규 JIS 심사 대비 허가번호
                                                          //	'Zpl &= "^A0N,20,20^FO637,040^FD07034^FS" & vbCrLf '--//2010.12.30, SHARP : 신규 JIS 심사 대비 허가번호
                                                          //	'--//Zpl &= "^A0N,20,20^FO590,050^FDAPPROVED No^FS" & vbCrLf '--//2010.12.30, SHARP : 주석처리함.(신규 JIS 심사에는 필요없음)
                                                          //	'--//Zpl &= "^A0N,20,20^FO590,070^FDKR 9434^FS" & vbCrLf     '--//2010.12.30, SHARP : 주석처리함.(신규 JIS 심사에는 필요없음)

            if (BARCODE_COMPANY.Length > 13)
            {
                Zpl += ("^A0N100,45^FO050,030^FD" + BARCODE_COMPANY + "^FS"); //  '회사
            }
            else
            {
                Zpl += ("^A0N200,65^FO050,030^FD" + BARCODE_COMPANY + "^FS"); //  '회사
            }

            Zpl += ("^A0N105,38^FO365,105^FD" + BARCODE_SIZE_NAME + Space(14) + BARCODE_Length + "^FS"); //    '사이즈,길이
            Zpl += ("^A0N105,38^FO370,148^FD" + BARCODE_STEEL + "^FS"); //    '강종
            Zpl += ("^A0N105,38^FO370,192^FD" + BARCODE_HEAT + "^FS"); //    'HEAT
            Zpl += ("^A0N105,38^FO430,235^FD" + BARCODE_BONSU + "^FS"); //          '본수
            Zpl += ("^A0N105,38^FO370,278^FD" + BARCODE_WEIGHT + "^FS"); //         '중량
            Zpl += ("^A0N105,38^FO370,322^FD" + BARCODE_INSPECT_DATE + "^FS"); // '일자
            Zpl += ("^FO106,455^BY3^BCN,090 ^FD" + BARCODE_BUNDLE_NO + "^FS"); // '바코드라인
            Zpl += ("^A0N090,32^FO130,660^FD" + BARCODE_SIZE_NAME + Space(10) + BARCODE_Length + Space(22) + BARCODE_WEIGHT + "^FS"); //   '사이즈,길이,중량
            Zpl += ("^A0N090,32^FO130,705^FD" + BARCODE_STEEL + Space(10) + BARCODE_ITEM + Space(24) + BARCODE_BONSU + "^FS"); //    '강종,ITEM,본수
            Zpl += ("^A0N090,32^FO050,750^FD" + BARCODE_AFTER_ROUTING + "^FS"); // '공정
            Zpl += ("^A0N090,32^FO680,750^FD" + BARCODE_SURFACE_LEVEL + "^FS"); // '표면등급
            Zpl += ("^A0N090,32^FO050,795^FD" + BARCODE_ITEM_USE + "^FS"); // '제품용도
            Zpl += ("^FO160,750^BY3^BCN,060 ^FD" + BARCODE_BUNDLE_NO + "^FS"); // '바코드라인
            Zpl += ("^A0N090,32^FO680,795^FD" + BARCODE_SIRF_CODE + "^FS"); // '합부코드
            Zpl += "^FO485,008^FD^XGKI006,1,1^FS";  //<-- 이곳이 JIS 마크의 ASCII CODE값을 뿌려 주는 곳이다.

            //if (Count == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC"; // '절단
            //}

            Zpl += "^XZ";

            //'print Mode를 CUTER->TEAR OFF로 전환
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }


        public string PB5_C_NEW(int Count)
        {
            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA";
            Zpl += ("^LH" + "017" + "," + "017" + "^FS");
            Zpl += "^MMT";

            if (BARCODE_ITEM == "IP")
            {
                BARCODE_ITEM = "IPE";
            }

            Zpl += "^A0N,35^FO020,025^FD" + BARCODE_ITEM + "^FS"; //            '품명
            if (int.Parse(BARCODE_STEEL) >= 10)
            {
                Zpl += "^A0N,35^FO152,070^FD" + BARCODE_STEEL + "^FS"; //          '강종
            }
            else
            {
                Zpl += "^A0N,35^FO185,070^FD" + BARCODE_STEEL + "^FS"; //          '강종
            }
            Zpl += "^A0N,22^FO020,065^FD" + BARCODE_SIZE_CODE.Substring(1, 10) + "^FS"; //    '규격번호
            Zpl += "^A0N,22^FO020,085^FD" + BARCODE_SIZE_CODE.Substring(11, 7) + "^FS"; //    '규격번호

            if (BARCODE_Length.Length < 4)
            {
                Zpl += "^A0N,45^FO270,155^FD" + BARCODE_Length + "^FS"; //         '길이
                Zpl += "^A0N,45^FO327,155^FD" + BARCODE_UOM + "^FS"; //            '길이단위
                Zpl += "^A0N,25^FO285,240^FD" + BARCODE_INSPECTER_NAME + "^FS"; // '검사원
            }
            else if (BARCODE_Length.Length == 4)
            {
                Zpl += "^A0N,45^FO246,155^FD" + BARCODE_Length + "^FS"; //         '길이
                Zpl += "^A0N,45^FO328,155^FD" + BARCODE_UOM + "^FS"; //            '길이단위
                Zpl += "^A0N,25^FO285,240^FD" + BARCODE_INSPECTER_NAME + "^FS"; // '검사원
            }
            else if (BARCODE_Length.Length >= 5)
            {
                Zpl += "^A0N,45^FO248,155^FD" + BARCODE_Length + "^FS"; //         '길이
                Zpl += "^A0N,45^FO327,155^FD" + BARCODE_UOM + "^FS"; //            '길이단위
                Zpl += "^A0N,25^FO285,240^FD" + BARCODE_INSPECTER_NAME + "^FS"; // '검사원
            }

            Zpl += "^A0N,35^FO020,115^FD" + BARCODE_SIZE_NAME + "^FS"; //      '사이즈
            Zpl += "^A0N,30^FO020,162^FD" + BARCODE_HEAT + "^FS"; //           'HEAT_NO

            if (BARCODE_PROGRAM == "10752")
            {
                Zpl += "^FO040,270^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS"; // '바코드라인
                Zpl += "^A0N,18^FO105,343^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS"; // '바코드 텍스트
            }
            else
            {
                Zpl += "^A0N,30^FO150,270^FD" + BARCODE_INSPECT_DATE + "^FS"; //      '검사일자
            }

            if (BARCODE_COUNTRY_BASIS == "KS")
            {
                Zpl += "^A0N,23^FO290,080^FD" + BARCODE_APPORVAL_NO + "^FS"; //  '허가번호
            }

            if (BARCODE_PROGRAM == "10752")
            {
                Zpl += "^A0N,25^FO350,240^FD" + BARCODE_BONSU + "^FS"; //          '본수
            }
            else
            {
                Zpl += "^A0N,25^FO330,240^FD" + "1" + "^FS"; //          '낱본발행본수
            }

            switch (BARCODE_COUNTRY_BASIS)
            {
                case "NO":
                    Zpl += ""; //
                    break;
                case "KS":
                    Zpl += "^FO282,000^FR^XGR:KS_PB51,1,1^FS"; //
                    Zpl += "^FO266,081^FD^XGR:KSA,1,1^FS"; //
                    break;
                case "JIS":
                    Zpl += "^FO298,012^FR^XGJIS_7_1,1,1^FS"; //
                    break;
            }

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC"; // '절단
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }
            else
            {
                BARCODE_CUTTING_LOCAL_COUNT = BARCODE_CUTTING_LOCAL_COUNT + 1;
                if (BARCODE_CUTTING_LOCAL_COUNT == BARCODE_CUTTING_COUNT)
                {
                    Zpl += "^MMC"; // '절단
                    BARCODE_CUTTING_LOCAL_COUNT = 0;
                }
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }       

        public string PB5_C()
        {
            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA";
            Zpl += ("^LH" + "017" + "," + "017" + "^FS");
            Zpl += "^MMT";

            if (BARCODE_ITEM == "IP")
            {
                BARCODE_ITEM = "IPE";
            }

            Zpl += "^A0N,35^FO020,025^FD" + BARCODE_ITEM + "^FS";

            if (BARCODE_STEEL.Length >= 10)
            {
                Zpl += "^A0N,35^FO152,070^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^A0N,35^FO185,070^FD" + BARCODE_STEEL + "^FS";
            }


            Zpl += "^A0N,22^FO020,065^FD" + BARCODE_SIZE_CODE.Substring(0, 10) + "^FS";
            Zpl += "^A0N,22^FO020,085^FD" + BARCODE_SIZE_CODE.Substring(11, 7) + "^FS";

            if (BARCODE_PROGRAM == "10750")
            {
                Zpl += "^FO040,270^BY1,3.0^B3N,N,070,N,N^FD" + BARCODE_INSPECT_DATE + BARCODE_BUNDLE_NO + "^FS"; //바코드라인
                Zpl += "^A0N,18^FO105,343^CI13^FR^FD" + BARCODE_INSPECT_DATE + " " + BARCODE_BUNDLE_NO + "^FS"; //바코드 텍스트
            }
            else
            {
                Zpl += "^A0N,30^FO150,270^FD" + BARCODE_INSPECT_DATE + "^FS"; //      '검사일자
            }

            if (BARCODE_COUNTRY_BASIS == "KS")
            {
                Zpl += "^A0N,23^FO290,080^FD" + "KSA" + BARCODE_APPORVAL_NO + "^FS"; //허가번호
            }

            if (BARCODE_PROGRAM == "10750")
            {
                Zpl += "^A0N,25^FO310,270^FD" + BARCODE_BONSU + "^FS";   // 본수
            }
            else
            {
                Zpl += "^A0N,25^FO310,270^FD" + "1" + "^FS";// 낱본발행본수
            }

            switch (BARCODE_COUNTRY_BASIS)
            {
                case "NO":
                    break;
                case "KS":
                    Zpl += "^FO305,015^FR^XGJNTYBKS_,1,1^FS";
                    break;
                case "JIS":
                    Zpl += "^FO298,012^FR^XGJIS_7_1,1,1^FS";
                    break;
            }

            //if (COUNT == BARCODE_PRINT_COUNT)
            //{
            //    Zpl += "^MMC"; // '절단
            //    BARCODE_CUTTING_LOCAL_COUNT = 0;
            //    BARCODE_CUTTING_COUNT = 0;
            //}
            //else
            //{
                BARCODE_CUTTING_LOCAL_COUNT = BARCODE_CUTTING_LOCAL_COUNT + 1;

                if (BARCODE_CUTTING_LOCAL_COUNT == BARCODE_CUTTING_COUNT)
                {
                    Zpl += "^MMC"; //  '절단
                    BARCODE_CUTTING_LOCAL_COUNT = 0;
                }
            //}

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

        }

        public string PB5_A(int Count)
        {
            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA";
            Zpl += ("^LH" + "017" + "," + "017" + "^FS");
            Zpl += "^MMT";

            if (BARCODE_ITEM == "IP")
            {
                BARCODE_ITEM = "IPE";
            }

            Zpl += ("^A0N,35^FO150,005^FD" + BARCODE_ITEM + "^FS");
            Zpl += ("^A0N,35^FO150,054^FD" + BARCODE_STEEL + "^FS");
            Zpl += ("^A0N,19^FO220,030^FD" + BARCODE_SIZE_CODE + "^FS");

            if (BARCODE_Length.Length < 4)
            {
                Zpl += ("^A0N,45^FO287,142^FD" + BARCODE_Length + "^FS");
                Zpl += ("^A0N,45^FO347,142^FD" + BARCODE_UOM + "^FS");
                Zpl += ("^A0N,25^FO295,270^FD" + BARCODE_INSPECTER_NAME + "^FS");
            }
            else if (BARCODE_Length.Length == 4)
            {
                Zpl += ("^A0N,45^FO268,142^FD" + BARCODE_Length + "^FS");
                Zpl += ("^A0N,45^FO347,142^FD" + BARCODE_UOM + "^FS");
                Zpl += ("^A0N,25^FO295,270^FD" + BARCODE_INSPECTER_NAME + "^FS");
            }
            else
            {
                Zpl += ("^A0N,45^FO253,142^FD" + BARCODE_Length + "^FS");
                Zpl += ("^A0N,45^FO345,142^FD" + BARCODE_UOM + "^FS");
                Zpl += ("^A0N,25^FO295,270^FD" + BARCODE_INSPECTER_NAME + "^FS");
            }


            if (BARCODE_PROGRAM == "10750")
            {
                Zpl += ("^FO040,240^BY1,3.0^B3N,N,070,N,N^FD" + BARCODE_INSPECT_DATE + BARCODE_BUNDLE_NO + "^FS");
                Zpl += ("^A0N,18^FO105,313^CI13^FR^FD" + BARCODE_INSPECT_DATE + BARCODE_BUNDLE_NO + "^FS");
            }
            else
            {
                Zpl += ("^A0N,30^FO150,250^FD" + BARCODE_INSPECT_DATE + BARCODE_BUNDLE_NO + "^FS");
            }

            if (BARCODE_COUNTRY_BASIS == "KS")
            {
                Zpl += "^A0N,23^FO290,080^FD" + BARCODE_APPORVAL_NO + "^FS";
            }
            else if (BARCODE_COUNTRY_BASIS == "JIS")
            {
                Zpl += "^A0N,23^FO290,080^FD" + BARCODE_APPORVAL_NO + "^FS";
            }


            if (BARCODE_PROGRAM == "10750")
            {
                Zpl += "^A0N,25^FO310,240^FD" + BARCODE_BONSU + "^FS";
            }
            else
            {
                Zpl += "^A0N,25^FO310,240^FD" + "1" + "^FS";
            }

            switch (BARCODE_COUNTRY_BASIS)
            {
                case "NO":
                    break;
                case "KS":
                    Zpl += "^FO305,015^FR^XGJNTYBKS_,1,1^FS";
                    break;
                case "JIS":
                    Zpl += "^FO298,012^FR^XGJIS_7_1,1,1^FS";
                    break;
            }

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }
            else
            {
                BARCODE_CUTTING_LOCAL_COUNT = BARCODE_CUTTING_LOCAL_COUNT + 1;

                if (BARCODE_CUTTING_LOCAL_COUNT == BARCODE_CUTTING_COUNT)
                {
                    Zpl += "^MMC";
                    BARCODE_CUTTING_LOCAL_COUNT = 0;
                }
            }

            Zpl += "^XZ";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string IA_TAG_00(int Count)
        {
            var Barcode = "";
            var Zpl = "";

            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^POI^FS";
            Zpl += "^PW830";

            Zpl += "^FO001,417^FD^XGR:HD,1,1^FS";
            Zpl += "^A0N150,30^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,37^FO390,417^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N130,25^FO090,448^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(7, 4) + "^FS";
            Zpl += "^FO500,417^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS"; // '바코드라인

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  IA_TAG_01(int Count)
        {

            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl += "^XA";
            Zpl += "^POI^FS";
            Zpl += "^PW720";
            Zpl += "^LH" + "00" + "," + "00" + "^MMT";

            Zpl += "^BY4,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^FO246,604^A0N,23,41^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^FO246,303^A0N,23,41^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^FO643,220^A0N,53,43^CI13^FR^FD" + "1" + "^FS";

            switch (BARCODE_PRINTED_BASIS)
            {
                case "NO": break;
                case "KS": Zpl += "^FO599,26^FR^XGJNTYBKS_,1,1^FS"; break;
                case "JIS": Zpl += "^FO611,28^FR^XGJIS_7_1,1,1^FS"; break;
            }


            switch (BARCODE_ITEM)
            {
                case "HB":
                    Zpl += "^FO300,22^A0I,50,100^CI13^FP^FD" + "H" + "^FS";
                    break;
                case "AI":
                    Zpl += "^FO599,26^FR^XGJNTYBKS_,1,1^FS";
                    break;
                case "AU":
                    Zpl += "^FO599,26^FR^XGJNTYBKS_,1,1^FS";
                    break;
                case "JIS":
                    Zpl += "^FO300,22^A0I,50,100^CI13^FP^FD" + "L" + "^FS";
                    break;

            }

            if (int.Parse(BARCODE_STEEL) >= 1 && int.Parse(BARCODE_STEEL) <= 11)
            {
                Zpl += "^FO430,80^A0N,57,41^CI13^FR^FD" + BARCODE_STEEL + "^FS";
            }
            else if (int.Parse(BARCODE_STEEL) >= 12 && int.Parse(BARCODE_STEEL) <= 15)
            {
                Zpl += "^FO430,80^A0N,57,31^CI13^FR^FD" + BARCODE_STEEL;
            }
            else if (int.Parse(BARCODE_STEEL) >= 16 && int.Parse(BARCODE_STEEL) <= 21)
            {
                Zpl += "^FO410,80^A0N,57,25^CI13^FR^FD" + BARCODE_STEEL;
            }

            Zpl += "^FO212,135^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^FO212,183^A0N,43,43^CI13^FR^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^FO490,145^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl += "^FO571,303^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTER_NAME + "^FS";
            Zpl += "^FO102,645^A0N,48,42^CI13^FR^FD" + BARCODE_BUNDLE_NO.Substring(0, 6) + "-" + BARCODE_BUNDLE_NO.Substring(7, 4) + "^FS";

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
            }
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  SP_U_TAG(int Count, string HeatTreatment)
        {
            var Zpl = "";
            var Barcode = "";
            //   var Barcode1 = "";
            var PoomMyoung = "";
            var AR_N = "";

            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;

            Zpl = "";
            Zpl = Zpl + "^XA";
            Zpl = Zpl + "^POI^FS";
            Zpl = Zpl + "^PW720";
            Zpl = Zpl + "^LH0,0^FS";
            Zpl = Zpl + "^BY2.0,1.5^FO280,360^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";
            Zpl = Zpl + "^BY2.0,1.5^FO600,364^BCB,080,N,Y,N^FR^FD>:" + Barcode + "^FS";
            Zpl = Zpl + "^FO300,620^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";
            Zpl = Zpl + "^FO300,320^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";
            Zpl = Zpl + "^FO520,314^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";
            Zpl = Zpl + "^FO063,314^A0N,45,40^CI13^FR^FD" + BARCODE_LICENSE + "^FS";
            Zpl = Zpl + "^FO220,670^A0N,38,38^CI13^FR^FD" + Barcode.Substring(0, 6) + "-" + Barcode.Substring(7, 4) + "^FS";
            Zpl = Zpl + "^FO506,670^A0N,38,38^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";

            Zpl = Zpl + "^FO49,87^A0N,35,35^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS";

            if (BARCODE_STEEL.Substring(BARCODE_STEEL.Length - 5, 2) == "04")
            {
                BARCODE_STEEL = BARCODE_STEEL.Substring(0, BARCODE_STEEL.Length - 2);
            }


            if (int.Parse(BARCODE_STEEL) >= 1 && int.Parse(BARCODE_STEEL) <= 11)
            {
                Zpl = Zpl + "^FO370,85^A0N,52,52^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";
            }
            else if (int.Parse(BARCODE_STEEL) >= 12 && int.Parse(BARCODE_STEEL) <= 15)
            {
                Zpl = Zpl + "^FO395,85^A0N,50,35^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";
            }
            else if (int.Parse(BARCODE_STEEL) >= 16 && int.Parse(BARCODE_STEEL) <= 21)
            {
                Zpl = Zpl + "^FO385,85^A0N,50,29^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";
            }


            switch (BARCODE_ITEM.Trim())
            {
                case "HB":
                    PoomMyoung = "H-BEAM";
                    break;
                case "UB":
                    PoomMyoung = "H-BEAM";
                    break;
                case "WB":
                    PoomMyoung = "H-BEAM";
                    break;
                case "WX":
                    PoomMyoung = "H-BEAM";
                    break;
                case "TP":
                    PoomMyoung = "SHEET PILE";
                    Zpl += "^BY4,3^FO65,712^BCN,65,N,Y,N^FR^FD>:" + Barcode + "^FS";
                    Zpl += "^A0N90,30^FO062,782^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N100,30^FO367,782^FD" + BARCODE_STEEL.Substring(0, BARCODE_STEEL.Length - 3) + "^FS";
                    Zpl += "^FO480,782^A0N,30,34^CI13^FR^FD" + Barcode.Substring(1, 6) + "-" + Barcode.Substring(7, 4) + "^FS";
                    break;
                case "AI": PoomMyoung = "INVERED ANGLE"; break;
                case "AE": PoomMyoung = "EAQUAL ANGLE"; break;
                case "IB": PoomMyoung = "I-BEAM"; break;
                default:
                    PoomMyoung = "";
                    break;
            }

            Zpl = Zpl + "^FO49,120^A0N,55,50^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl = Zpl + "^FO49,188^A0N,49,45^CI13^FR^FD" + BARCODE_HEAT + "^FS";
            //Zpl = Zpl + "^FO490,153^A0N,75,65^CI13^FR^FD" + bar_format(BARCODE_Length, 1) + BARCODE_UOM + "^FS";
            Zpl = Zpl + "^FO490,153^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl = Zpl + "^FO49,45^A0N,40,40^CI13^FR^FD" + PoomMyoung + "^FS";

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }
        
        public string  HB_CE_TAG(int Count, string HeatTreatment)
        {
            var Zpl = "";

            var Barcode = "";
            var Barcode1 = "";
            var PoomMyoung = "";
            // var ItemSize  = "";
            var AR_N = "";
            var T_STEEL = "";


            if (Mid(BARCODE_STEEL, BARCODE_STEEL.Length - 4, 2) == "04")
            {
                T_STEEL = BARCODE_STEEL.Substring(0, BARCODE_STEEL.Length - 3);
            }
            else
            {
                T_STEEL = BARCODE_STEEL;
            }

            BARCODE_STEEL = BARCODE_STEEL.Replace("04", "");

            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + T_STEEL;
            HeatTreatment = "0";

            Zpl = "";
            Zpl = Zpl + "^XA";
            Zpl = Zpl + "^POI^FS";
            Zpl = Zpl + "^PW720";
            Zpl = Zpl + "^LH0,0^FS";

            Zpl = Zpl + "^BY2.0,1.5^FO285,360^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";
            Zpl = Zpl + "^BY2.0,1.5^FO600,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS";
            Zpl = Zpl + "^FO260,620^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";
            Zpl = Zpl + "^FO260,320^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";

            Zpl = Zpl + "^FO520,314^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";
            Zpl = Zpl + "^FO063,314^A0N,45,40^CI13^FR^FD" + BARCODE_LICENSE + "^FS";
            Zpl = Zpl + "^FO220,670^A0N,38,38^CI13^FR^FD" + Mid(Barcode, 0, 6) + "-" + Mid(Barcode, 6, 4) + "^FS";
            Zpl = Zpl + "^FO506,670^A0N,38,38^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";
            Zpl = Zpl + "^FO49,87^A0N,35,35^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS";
            Zpl = Zpl + "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";
            Zpl = Zpl + "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";
            Zpl = Zpl + "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS";
            Zpl = Zpl + "^FO520,752^A0N,28,28^CI13^FR^FD" + Mid(BARCODE_INSPECT_DATE, 0, 4) + "-" + Mid(BARCODE_INSPECT_DATE, 4, 2) + "-" + Mid(BARCODE_INSPECT_DATE, 6, 2) + "^FS";


            if (int.Parse(BARCODE_STEEL) >= 1 && int.Parse(BARCODE_STEEL) <= 11)
            {
                Zpl = Zpl + "^FO370,85^A0N,52,52^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";
            }
            else if (int.Parse(BARCODE_STEEL) >= 12 && int.Parse(BARCODE_STEEL) <= 15)
            {
                Zpl = Zpl + "^FO395,85^A0N,50,35^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";
            }
            else if (int.Parse(BARCODE_STEEL) >= 16 && int.Parse(BARCODE_STEEL) <= 21)
            {
                Zpl = Zpl + "^FO385,85^A0N,50,29^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";
            }

            switch (BARCODE_ITEM.Trim())
            {
                case "HB":
                    PoomMyoung = "H-BEAM";
                    break;
                case "UB":
                    PoomMyoung = "H-BEAM";
                    break;
                case "WB":
                    PoomMyoung = "H-BEAM";
                    break;
                case "WX":
                    PoomMyoung = "H-BEAM";
                    break;
                case "TP":
                    PoomMyoung = "SHEET PILE";
                    Zpl += "^BY4,3^FO65,712^BCN,65,N,Y,N^FR^FD>:" + Barcode + "^FS";
                    Zpl += "^A0N90,30^FO062,782^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N100,30^FO367,782^FD" + BARCODE_STEEL.Substring(0, BARCODE_STEEL.Length - 3) + "^FS";
                    Zpl += "^FO480,782^A0N,30,34^CI13^FR^FD" + Barcode.Substring(1, 6) + "-" + Barcode.Substring(7, 4) + "^FS";
                    break;
                case "AI": PoomMyoung = "INVERED ANGLE"; break;
                case "AE": PoomMyoung = "EAQUAL ANGLE"; break;
                case "IB": PoomMyoung = "I-BEAM"; break;
                default:
                    PoomMyoung = "";
                    break;
            }                

            Zpl = Zpl + "^FO49,135^A0N,55,50^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl = Zpl + "^FO49,188^A0N,49,45^CI13^FR^FD" + BARCODE_HEAT + "^FS";
            //Zpl = Zpl + "^FO490,153^A0N,75,65^CI13^FR^FD" + bar_format(BARCODE_Length, 1) + BARCODE_UOM + "^FS";
            Zpl = Zpl + "^FO490,153^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl = Zpl + "^FO49,35^A0N,40,40^CI13^FR^FD" + PoomMyoung + "^FS";

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
            }
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
                      
        }

        public string  SP_DOMESTIC(int Count)
        {
            var Zpl = "";
            var Barcode = "";
            var Barcode1 = "";
            var PoomMyoung = "";
            //    var ItemSize = "";
            //   var i, cnt ;

            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + BARCODE_STEEL;

            if (BARCODE_LABEL_GUBUN == "내수")
            {

                Zpl = "";
                Zpl += "^XA";
                Zpl += "^POI^FS";
                Zpl += "^PW720";
                Zpl += "^LH0,0^FS";
                Zpl += "^BY3,3^FO090,358^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";       // 가로 바코드
                Zpl += "^FO230,320^A0N,19,36^CI13^FR^FD" + Barcode + "^FS";                  // 가로 바코드 텍스트-1
                Zpl += "^FO230,615^A0N,19,36^CI13^FR^FD" + Barcode + "^FS"; //                                       '가로 바코드 텍스트-2
                Zpl += "^FO40,650^A0N,40,37^CI13^FR^FD" + Mid(Barcode, 1, 6) + "-" + Mid(Barcode, 7, 4) + "^FS"; //     'PO+BD
                Zpl += "^FO340,650^A0N,40,37^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS"; //                     '발행일자
                Zpl += "^BY2,2.0^FO600,350^BCB,90,N,Y,N^FR^FD>:" + Barcode + "^FS"; //                                  '세로 바코드
                Zpl += "^FO580,420^A0B,23,36^CI13^FR^FD" + Barcode + "^FS"; //                                          '세로 바코드 텍스트-1
                Zpl = Zpl + "^BY4,3^FO65,712^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";                //  '가로 바코드
                Zpl = Zpl + "^FO65,782^A0N,25,30^CI13^FR^FD" + Barcode1 + "^FS"; //                     '가로 바코드 텍스트-1


                switch (BARCODE_MARK)
                {
                    case "0":
                        break;
                    case "1":
                        Zpl += "^FO588,35^FR^XGKI007,1,1^FS";
                        Zpl += "^FO030,323^FR^XGR:KSA_1,1,1^FS";
                        break;
                    case "2":
                        Zpl += "^FO538,20^FR^XGKI006,1,1^FS";
                        break;
                }


                Zpl += "^FO040,298^A0N,35,35^CI13^FR^FD" + BARCODE_LICENSE + "^FS";
                Zpl += "^FO440,303^A0N,38,36^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS"; //                    '작업자


                switch (BARCODE_ITEM.Trim())
                {
                    case "HB": PoomMyoung = "H"; break;
                    case "UB": PoomMyoung = "H"; break;
                    case "WB": PoomMyoung = "H"; break;
                    case "WX": PoomMyoung = "H"; break;
                    case "SP": PoomMyoung = "SP"; break;
                    case "IA": PoomMyoung = "L"; break;
                    case "EA": PoomMyoung = "L"; break;
                    case "IB": PoomMyoung = "I"; break;
                    default: PoomMyoung = ""; break;
                }

                switch (PoomMyoung)
                {
                    case "H":
                        Zpl += "^FO280,30^A0N,55,100^CI13^FP^FD" + PoomMyoung + "^FS";// 'H 형강
                        break;
                    case "UB":
                        Zpl += "^FO280,30^A0N,55,100^CI13^FP^FD" + PoomMyoung + "^FS";// 'H 형강
                        break;
                    case "WB":
                        Zpl += "^FO280,30^A0N,55,100^CI13^FP^FD" + PoomMyoung + "^FS";// 'H 형강
                        break;
                    case "WX":
        		        Zpl += "^FO280,30^A0N,55,100^CI13^FP^FD" + PoomMyoung + "^FS";// 'H 형강
                        break;
                    case "I":
                        Zpl += "^FO280,30^ADN,55,70^CI13^FP^FD" + PoomMyoung + "^FS"; //'I 형강
                        break;
                    case "SP":
                        Zpl += "^FO270,30^A0N,55,70^CI13^FP^FD" + PoomMyoung + "^FS"; //'SP 형강
                        break;
                    case "L":
                        Zpl += "^FO300,22^A0I,50,100^CI13^FP^FD" + PoomMyoung + "^FS";//  'ㄱ 형강
                        break;
                    default:
                        break;
                }

                Zpl += "^FO212,85^A0N,45,40^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS";//'규격 Num

                var v_steelLength = BARCODE_STEEL.Length;
                if (v_steelLength >= 1 && v_steelLength <= 11)
                {
                    Zpl += "^FO430,80^A0N,57,41^CI13^FR^FD" + BARCODE_STEEL + "^FS";//'강종명
                }
                else if (v_steelLength >= 12 && v_steelLength <= 15)
                {
                    Zpl += "^FO430,80^A0N,57,31^CI13^FR^FD" + BARCODE_STEEL + "^FS";//'강종명
                }
                else if (v_steelLength >= 16 && v_steelLength <= 21)
                {
                    Zpl += "^FO410,80^A0N,57,25^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //'강종명
                }

                Zpl += "^FO212,135^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS"; //                                 '제품규격
                Zpl += "^FO212,183^A0N,43,43^CI13^FR^FD" + BARCODE_HEAT + "^FS"; //                                      'Heat No
                //Zpl += "^FO490,145^A0N,75,65^CI13^FR^FD" + bar_format(BARCODE_Length, 1) + BARCODE_UOM + "^FS"; //      '제품길이
                Zpl += "^FO490,145^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS"; //      '제품길이

                if (Count == BARCODE_PRINT_COUNT)
                {
                    Zpl += "^MMC";
                }
                Zpl += "^XZ";
                Zpl += "^XA";
                Zpl += "^MMT";
                Zpl += "^XZ";
                return Zpl;

                // 해당코드가 실행되지않는거 같은데 확인필요 - 이현성
                Zpl += "^PQ1,0,0,N";
                Zpl += "^XZ";

            }
            else if (BARCODE_LABEL_GUBUN == "수출(기타수출)")
            {

            }
            return Zpl;
        }

        public string  HB_TAG(int Count)
        {

            var Zpl = "";
            var Barcode = "";
            var Barcode1 = "";
            var PoomMyoung = "";
            var ItemSize = "";
            //   var i, cnt ;
            var xPosition = 0;
            var sIDE_TAG_STEEL = "";
            
 
            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;

            var v_steelLength = BARCODE_STEEL.Length;

            if (v_steelLength >= 22 && v_steelLength <= 30)
            {
                Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + Mid(BARCODE_STEEL, 0, 5).Trim() + "/" + Mid(BARCODE_STEEL, 11, 5).Trim();
            }
            else
            {
                Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + BARCODE_STEEL;
            }



            if (BARCODE_STEEL.Length < 20)
            {
                xPosition = InStr(BARCODE_STEEL.Trim(), "-");

                if (xPosition == 0)
                {
                    xPosition = InStr(BARCODE_STEEL.Trim(), " ");
                }

                if (xPosition == 0)
                {
                    xPosition = InStr(BARCODE_STEEL.Trim(), "/");
                }

                if (xPosition < 5)
                {
                    sIDE_TAG_STEEL = Mid(BARCODE_STEEL.Trim(), xPosition, 10).Trim();
                    Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + sIDE_TAG_STEEL;
                }
            }


            if (BARCODE_LABEL_GUBUN == "내수")
            {
                //if(true){
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^POI^FS";
                Zpl += "^PW720";
                Zpl += "^LH0,0^FS";
                Zpl += "^BY3.0,3.0^FO63,358^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";  //   '바코드

                switch (Mid(BARCODE_STEEL, 0, 3))
                {
                    case "SHN":
                        Zpl = Zpl + "^FO560,404^FR^XGR:NAEJIN,1,1^FS"; // 'KS
                        break;
                    default:
                        Zpl += "^BY2.0,1.5^FO590,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS"; //     '세로바코드
                        Zpl += "^FO570,420^A0B,23,41^CI13^FR^FD" + Barcode + "^FS"; //  '세로바코드 텍스트
                        break;
                }


                Zpl += "^FO170,607^A0N,23,41^CI13^FR^FD" + Barcode + "^FS"; //            '바코드 텍스트
                Zpl += "^FO306,318^A0N,23,41^CI13^FR^FD" + Barcode + "^FS"; //            '바코드 텍스트


                Zpl = Zpl + "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS"; //         '가로 바코드
                Zpl = Zpl + "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS"; //         '가로 바코드 텍스트-1
                Zpl = Zpl + "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS"; // 'Heat No
                Zpl = Zpl + "^FO520,752^A0N,28,28^CI13^FR^FD" + Mid(BARCODE_INSPECT_DATE, 0, 4) + "-" + Mid(BARCODE_INSPECT_DATE, 4, 2) + "-" + Mid(BARCODE_INSPECT_DATE, 6, 2) + "^FS"; //'발행일자
                Zpl = Zpl + "^FO004,718^FD^XGR:HD,1,1^FS";  //                   'Hyundai Image (H)


                switch (BARCODE_MARK)
                {
                    case "0": break;
                    case "1":
                        Zpl += "^FO588,35^FR^XGKI007,1,1^FS";// 'KS
                        Zpl += "^FO030,323^FR^XGR:KSA_1,1,1^FS";// '"한국표준협회" 글자 이미지 출력 : 2009.12.18, SHARP
                        break;
                    case "2":
                        Zpl += "^FO538,20^FR^XGKI006,1,1^FS"; //'JIS  '1 column up 55->30 081014 kde
                        break;
                }

                Zpl += "^FO571,303^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";//     '작업자
                Zpl += "^FO040,298^A0N,35,35^CI13^FR^FD" + BARCODE_LICENSE + "^FS";//  '허가번호
                Zpl += "^FO080,645^A0N,48,42^CI13^FR^FD" + Mid(Barcode, 0, 6) + "-" + Mid(Barcode, 6, 4) + "^FS";//     'PO+BD
                Zpl += "^FO376,645^A0N,48,42^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";//      '발행일자


                switch (BARCODE_ITEM.Trim())
                {
                    case "HB":
                    case "UB":
                    case "WB":
                    case "WX":
                    case "IP":
                        PoomMyoung = "H-BEAM";
                        break;
                    case "TP": PoomMyoung = "SHEET-PILE"; break;
                    case "AI": PoomMyoung = "L"; break; //ㄱ
                    case "AE": PoomMyoung = "L"; break; //ㄱ
                    case "IB": PoomMyoung = "I"; break; //영문자 i
                    case "TS": PoomMyoung = "TS"; break; //영문자 i
                    case "RR": PoomMyoung = "RL"; break; //영문자 i
                    default: PoomMyoung = ""; break; //내용없음
                }


                switch (PoomMyoung)
                {
                    case "H-BEAM": Zpl += "^FO043,30^A0N,45,40^CI13^FP^FD" + PoomMyoung + "^FS"; break; //H 형강
                    case "I": Zpl += "^FO280,30^ADN,55,70^CI13^FP^FD" + PoomMyoung + "^FS"; break; //I 형강
                    case "SHEET-PILE": Zpl += "^FO043,30^A0N,55,70^CI13^FP^FD" + PoomMyoung + "^FS"; break; //SP 형강
                    case "L": Zpl += "^FO300,22^A0I,50,100^CI13^FP^FD" + PoomMyoung + "^FS"; break; //ㄱ 형강
                    case "RL": Zpl = Zpl + "^FO270,30^A0N,55,70^CI13^FP^FD" + PoomMyoung + "^FS"; break; //SP 형강 : 2011.04.18, SHARP
                    case "TS": Zpl = Zpl + "^FO280,32^A0N,55,100^CI13^FP^FD" + PoomMyoung + "^FS"; break; //TS 형강
                    default: break;
                }

                v_steelLength = BARCODE_STEEL.Length;

                //alert(v_steelLength);

                if (v_steelLength >= 1 && v_steelLength <= 11)
                {
                    if (BARCODE_STAND_NUM.Length > 10)
                    {
                        Zpl += "^FO043,85^A0N,37,30^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS"; //       '규격 Num
                    }
                    else
                    {
                        Zpl += "^FO043,85^A0N,45,40^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS"; //    '규격 Num
                    }
                    Zpl = Zpl + "^FO261,80^A0N,57,41^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //'강종명
                }
                else if (v_steelLength >= 12 && v_steelLength <= 15)
                {
                    if (BARCODE_STAND_NUM.Length > 10)
                    {
                        Zpl += "^FO043,85^A0N,37,30^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS"; //     '규격 Num
                    }
                    else
                    {
                        Zpl += "^FO043,85^A0N,45,40^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS"; //    '규격 Num
                    }
                    Zpl = Zpl + "^FO261,80^A0N,57,31^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //'강종명
                }
                else if (v_steelLength >= 16 && v_steelLength <= 21)
                {
                    if (BARCODE_STAND_NUM.Length > 10)
                    {
                        Zpl += "^FO043,85^A0N,37,30^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS"; //    '규격 Num
                    }
                    else
                    {
                        Zpl += "^FO043,85^A0N,45,40^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS";//  '규격 Num
                    }
                    Zpl = Zpl + "^FO241,80^A0N,57,25^CI13^FR^FD" + BARCODE_STEEL + "^FS"; // '강종명
                }
                else if (v_steelLength >= 22 && v_steelLength <= 30)
                {
                    Zpl = Zpl + "^FO043,95^A0N,32,28^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS"; //    '규격 Num
                    Zpl = Zpl + "^FO163,83^A0N,54,38^CI13^FR^FD" + Mid(BARCODE_STEEL, 0, 6) + "^FS"; //  '강종명(SS400/) '--//2010.08.04, SHARP 추가 : 하경태대리 요청사항
                    Zpl = Zpl + "^FO281,95^A0N,32,28^CI13^FR^FD" + Mid(BARCODE_STEEL, 6, 4) + "^FS"; //  '강종명(ASTM) '--//2010.08.04, SHARP 추가 : 하경태대리 요청사항
                    Zpl = Zpl + "^FO341,83^A0N,54,38^CI13^FR^FD" + Mid(BARCODE_STEEL, 10, 4) + "^FS"; //  '강종명( A36) '--//2010.08.04, SHARP 추가 : 하경태대리 요청사항
                }

                if (BARCODE_ITEM.Trim() == "WX")
                {
                    Zpl += "^FO043,135^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "lb" + "^FS";//      '제품규격
                }
                else
                {
                    Zpl += "^FO043,135^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";//     '제품규격
                }

                Zpl += "^FO043,183^A0N,43,43^CI13^FR^FD" + BARCODE_HEAT + "^FS"; // 'Heat No
                //Zpl += "^FO490,145^A0N,75,65^CI13^FR^FD" + bar_format(BARCODE_Length, 1) + BARCODE_UOM + "^FS"; //    '제품길이
                Zpl += "^FO490,145^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS"; //    '제품길이


                if (Count == BARCODE_PRINT_COUNT)
                {
                    Zpl += "^MMC";
                }

                Zpl += "^XZ";
                Zpl += "^XA";
                Zpl += "^MMT";
                Zpl += "^XZ";


                return Zpl;
            }
            else if (BARCODE_LABEL_GUBUN == "수출(기타수출)")
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^POI^FS";
                Zpl += "^PW712";//  '742
                Zpl += "^LH0,0^FS";//          '기본좌표
                Zpl += "^BY3.0,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";//  '바코드 090206 kde
                Zpl += "^BY2.0,1.5^FO580,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS"; //  '세로바코드090206 kde
                Zpl += "^FO170,604^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";//      '바코드 텍스트
                Zpl += "^FO306,318^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";//      '바코드 텍스트
                Zpl = Zpl + "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";//          '가로 바코드
                Zpl = Zpl + "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";//          '가로 바코드 텍스트-1
                Zpl = Zpl + "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS";//   'Heat No
                Zpl = Zpl + "^FO520,752^A0N,28,28^CI13^FR^FD" + Mid(BARCODE_INSPECT_DATE, 0, 4) + "-" + Mid(BARCODE_INSPECT_DATE, 4, 2) + "-" + Mid(BARCODE_INSPECT_DATE, 6, 2) + "^FS";// '발행일자

                switch (BARCODE_MARK)
                {
                    case "0": break;
                    case "1": Zpl += "^FO588,35^FR^XGKI007,1,1^FS"; break;
                    case "2": Zpl += "^FO538,20^FR^XGKI006,1,1^FS"; break; //'1 column up 55->30 081014 kde
                }

                Zpl += "^FO573,314^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";//     '작업자
                Zpl += "^FO063,314^A0N,45,40^CI13^FR^FD" + BARCODE_LICENSE + "^FS";//  '허가번호
                Zpl += "^FO080,645^A0N,48,42^CI13^FR^FD" + Mid(Barcode, 0, 6) + "-" + Mid(Barcode, 7, 4) + "^FS";//     'PO+BD
                Zpl += "^FO376,645^A0N,48,42^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";//  '발행일자

                int v_standNumLength = BARCODE_STAND_NUM.Length;

                if (v_standNumLength >= 1 && v_standNumLength <= 15)
                {
                    Zpl += "^FO036,87^A0N,45,40^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS";//'규격 Num
                }
                else if (v_standNumLength >= 16 && v_standNumLength <= 21)
                {
                    Zpl += "^FO036,87^A0N,42,25^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS";// '규격 Num
                }

                v_steelLength = BARCODE_STEEL.Length;
                if (v_steelLength >= 1 && v_steelLength <= 11)
                {
                    Zpl += "^FO276,85^A0N,50,41^CI13^FR^FD" + BARCODE_STEEL + "^FS"; // '강종명
                }
                else if (v_steelLength >= 12 && v_steelLength <= 15)
                {
                    Zpl += "^FO276,85^A0N,50,31^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //'강종명
                }
                else if (v_steelLength >= 16 && v_steelLength <= 21)
                {
                    Zpl += "^FO266,85^A0N,47,25^CI13^FR^FD" + BARCODE_STEEL + "^FS";//'강종명
                }

                switch (BARCODE_ITEM.Trim())
                {
                    case "HB":
                    case "UB":
                    case "WB":
                    case "WX":
                    case "IP":
                        PoomMyoung = "H-BEAM";
                        switch (BARCODE_STEEL)
                        {
                            case "SS400 B":
                            case "A572G50B":
                                PoomMyoung = "";
                                break;
                            default:
                                PoomMyoung = "H-BEAM";
                                break;
                        }
                        break;
                    case "TP": PoomMyoung = "SHEET PILE"; break;
                    case "AI": PoomMyoung = "INVERED ANGLE"; break;
                    case "AE": PoomMyoung = "EAQUAL ANGLE"; break;
                    case "IB": PoomMyoung = "I-BEAM"; break;
                    default: PoomMyoung = ""; break;
                }

                Zpl += "^FO036,135^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS"; //    '제품규격
                Zpl += "^FO036,188^A0N,53,43^CI13^FR^FD" + BARCODE_HEAT + "^FS";//     'Heat No
                //Zpl += "^FO490,153^A0N,75,65^CI13^FR^FD" + bar_format(BARCODE_Length, 1) + BARCODE_UOM + "^FS";//    '제품길이
                Zpl += "^FO490,153^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";//    '제품길이
                Zpl += "^FO036,35^A0N,45,40^CI13^FR^FD" + PoomMyoung + "^FS";//  '제품명

                if (Count == BARCODE_PRINT_COUNT)
                {
                    Zpl += "^MMC";
                }

                Zpl += "^XZ";
                Zpl += "^XA";
                Zpl += "^MMT";
                Zpl += "^XZ";

                return Zpl;
            }

            // 수출용 라벨
            else
            {
                switch (BARCODE_ITEM.Trim())
                {
                    case "HB": ItemSize = "H-BEAM"; break;
                    default: ItemSize = ""; break;
                }

                Zpl = "";
                Zpl += "^XA";
                Zpl += "^POI^FS";
                Zpl += "^PW714";
                Zpl += "^LH0,0^FS";//        '기본좌표
                Zpl += "^BY3.0,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";//       '바코드 090206 kde
                Zpl += "^BY2.0,1.5^FO580,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS";//        '세로바코드090206 kde
                Zpl += "^FO170,604^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";//            '바코드텍스트
                Zpl += "^FO246,318^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";//           '바코드텍스트
                Zpl = Zpl + "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";//              '가로 바코드
                Zpl = Zpl + "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";//              '가로 바코드 텍스트-1
                Zpl = Zpl + "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS";//  'Heat No
                Zpl = Zpl + "^FO520,752^A0N,28,28^CI13^FR^FD" + Mid(BARCODE_INSPECT_DATE, 0, 4) + "-" + Mid(BARCODE_INSPECT_DATE, 5, 2) + "-" + Mid(BARCODE_INSPECT_DATE, 6, 2) + "^FS";// '발행일자
                Zpl += "^FO573,314^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";//            '작업자
                Zpl += "^FO063,314^A0N,45,40^CI13^FR^FD" + BARCODE_LICENSE + "^FS";//         '허가번호
                Zpl += "^FO080,645^A0N,48,42^CI13^FR^FD" + Mid(Barcode, 0, 6) + "-" + Mid(Barcode, 6, 4) + "^FS";//  'PO+BD
                Zpl += "^FO376,645^A0N,48,42^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";//    '발행일자

                v_steelLength = BARCODE_STEEL.Length;

                if (v_steelLength >= 1 && v_steelLength <= 11)
                {
                    Zpl += "^FO170,107^A0N,51,57^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //'강종명
                }
                else if (v_steelLength >= 12 && v_steelLength <= 15)
                {
                    Zpl += "^FO170,107^A0N,51,35^CI13^FR^FD" + BARCODE_STEEL + "^FS";//'강종명
                }
                else if (v_steelLength >= 16 && v_steelLength <= 21)
                {
                    Zpl += "^FO145,104^A0N,65,25^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //'강종명

                }

                //alert(ItemSize);

                Zpl += "^FO034,37^A0N,51,40^CI13^FR^FD" + ItemSize + "^FS"; //   '제품


                if (BARCODE_ITEM.Trim() == "WX")
                {
                    Zpl += "^FO202,37^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "lb" + "^FS";//         '제품규격
                }
                else
                {
                    Zpl += "^FO202,37^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS"; //             '제품규격
                }

                Zpl += "^FO034,185^A0N,53,43^CI13^FR^FD" + BARCODE_HEAT + "^FS"; //      'Heat No
                //Zpl += "^FO480,125^A0N,75,65^CI13^FR^FD" + bar_format(BARCODE_Length, 1) + BARCODE_UOM + "^FS"; //          '제품길이
                Zpl += "^FO480,125^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS"; //          '제품길이
                Zpl += "^FO034,133^A0N,43,36^CI13^FR^FD" + BARCODE_STAND_NUM + "^FS"; //       '규격 Num

                if (Count == BARCODE_PRINT_COUNT)
                {
                    Zpl += "^MMC";
                }

                Zpl += "^XZ";
                Zpl += "^XA";
                Zpl += "^MMT";
                Zpl += "^XZ";


                return Zpl;
            }
            
        }

        public string  PB5_HB_LABELA(int Count)
        {

            var Zpl = "";
            var barcodesteel = "";
            var v_BARCODE_STEELLength = 0;
            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "017" + "," + "017" + "^FS";
            Zpl += "^MMT";


            if (BARCODE_ITEM == "IP")
            {
                BARCODE_ITEM = "IPE";
            }
            else if (BARCODE_ITEM == "HB" || BARCODE_ITEM == "WX" || BARCODE_ITEM == "UB")
            {
                BARCODE_ITEM = "H-BEAM";
            }

            Zpl += "^A0N,35^FO010,008^FD" + BARCODE_ITEM + "^FS";

            barcodesteel = BARCODE_STEEL;

            v_BARCODE_STEELLength = barcodesteel.Length;
            //alert("barcodesteel  -- barcodesteel.Length ==> " + barcodesteel + " -- " + barcodesteel.Length );

            //    alert(BARCODE_SIZE_NAME);

            //--트리플/듀얼강종 추가 관련 보완작업 : FX(SHN400/SS400/ASTM A36), FZ(SHN490/SM490A), 2014.03.11, SHARP 추가
            if (BARCODE_STEEL == "SHN400/SS400/ASTM A36")
            {
                //--강종출력① : 정상위치
                Zpl += "^A0N,19^FO010,048^FD" + "KS D 3866" + "^FS";
                Zpl += "^A0N,28^FO093,043^FD" + Mid(BARCODE_STEEL, 0, 7) + "^FS";
                Zpl += "^A0N,19^FO010,082^FD" + "KS D 3503" + "^FS";
                Zpl += "^A0N,28^FO093,077^FD" + Mid(BARCODE_STEEL, 7, 15) + "^FS";
                //--강종출력② : 사이드라벨
                Zpl += "^A0N,20^FO340,409^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL == "SHN490/SM490A")
            {
                //--강종출력① : 정상위치
                Zpl += "^A0N,19^FO010,048^FD" + "KS D 3866" + "^FS";
                Zpl += "^A0N,28^FO093,043^FD" + Mid(BARCODE_STEEL, 0, 7) + "^FS";
                Zpl += "^A0N,19^FO010,082^FD" + "KS D 3515" + "^FS";
                Zpl += "^A0N,28^FO093,077^FD" + Mid(BARCODE_STEEL, 7, 10) + "^FS";
                //--강종출력② : 사이드라벨
                Zpl += "^A0N,20^FO340,409^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                if (v_BARCODE_STEELLength >= 14 && v_BARCODE_STEELLength <= 15)
                {               //--"SS400/ASTM A36" 용으로 사용될 경우
                                //--강종출력① : 정상위치
                             
                    Zpl += "^A0N,32^FO010,050^FD" + Mid(BARCODE_STEEL, 0, 5) + "^FS";
                    Zpl += "^A0N,32^FO010,077^FD" + Mid(BARCODE_STEEL, 5, 1) + "^FS";
                    Zpl += "^A0N,20^FO019,081^FD" + Mid(BARCODE_STEEL, 6, 4) + "^FS";
                    Zpl += "^A0N,32^FO063,077^FD" + Mid(BARCODE_STEEL, 10, 5) + "^FS";

                    /*
                    Zpl += "^A0N,32^FO010,050^FD" + BARCODE_STEEL.Substring(0, 5) + "^FS";
                    Zpl += "^A0N,32^FO010,077^FD" + BARCODE_STEEL.Substring(5, 1) + "^FS";
                    Zpl += "^A0N,20^FO019,081^FD" + BARCODE_STEEL.Substring(6, 4) + "^FS";
                    Zpl += "^A0N,32^FO063,077^FD" + BARCODE_STEEL.Substring(10, 5) + "^FS";
                    */
                    //--강종출력② : 사이드라벨
                    Zpl += "^A0N,20^FO340,409^FD" + Mid(BARCODE_STEEL,0, 5) + "^FS";
                    Zpl += "^A0N,20^FO395,409^FD" + Mid(BARCODE_STEEL,5, 1) + "^FS";
                    Zpl += "^A0N,15^FO405,411^FD" + Mid(BARCODE_STEEL,6, 4) + "^FS";
                    Zpl += "^A0N,20^FO442,409^FD" + Mid(BARCODE_STEEL,10, 5) + "^FS";
                    
                    /*
                    Zpl += "^A0N,20^FO340,409^FD" + BARCODE_STEEL.Substring(0, 5) + "^FS";
                    Zpl += "^A0N,20^FO395,409^FD" + BARCODE_STEEL.Substring(5, 1) + "^FS";
                    Zpl += "^A0N,15^FO405,411^FD" + BARCODE_STEEL.Substring(6, 4) + "^FS";
                    Zpl += "^A0N,20^FO442,409^FD" + BARCODE_STEEL.Substring(10, 5) + "^FS";
                    */
                }
                else if (v_BARCODE_STEELLength >= 20 && v_BARCODE_STEELLength <= 22)
                {
                    Zpl += "^A0N,35,25^FO010,054^FD" + BARCODE_STEEL + "^FS";         //--강종출력① : 정상위치
                    Zpl += "^A0N,20^FO340,409^FD" + BARCODE_STEEL + "^FS";             //--강종출력② : 사이드라벨
                }
                else
                {
                    Zpl += "^A0N,35^FO010,054^FD" + BARCODE_STEEL + "^FS";            //--강종출력① : 정상위치
                    Zpl += "^A0N,20^FO340,409^FD" + BARCODE_STEEL + "^FS";             //--강종출력② : 사이드라벨
                }
                //2015.10.01 ACRS코드 관련 SIZE_CODE 출력위치 변경.
                if (BARCODE_STEEL == "300")
                {
                    Zpl += "^A0N,19^FO240,005^FD" + BARCODE_SIZE_CODE + "^FS";
                }
                else
                {
                    Zpl += "^A0N,19^FO213,030^FD" + BARCODE_SIZE_CODE + "^FS";
                }
            }

            int v_BARCODE_Length_Length = BARCODE_Length.Length;

            if (v_BARCODE_Length_Length < 4)
            {
                Zpl += "^A0N,45^FO267,142^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,45^FO327,142^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N,23^FO495,429^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,23^FO525,429^FD" + BARCODE_UOM + "^FS";
            }
            else if (v_BARCODE_Length_Length == 4)
            {
                Zpl += "^A0N,45^FO248,142^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,45^FO327,142^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N,23^FO495,429^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,23^FO540,429^FD" + BARCODE_UOM + "^FS";
            }
            else if (v_BARCODE_Length_Length >= 5)
            {
                Zpl += "^A0N,45^FO233,142^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,45^FO325,142^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N,23^FO495,429^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,23^FO555,429^FD" + BARCODE_UOM + "^FS";
            }


            if (BARCODE_OLD_NEW_LABEL == "OLD")
            {
                Zpl += "^FO030,265^BY2,3.0^BCN,048,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                Zpl += "^A0N,20^FO105,316^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS";
                Zpl += "^A0N,20^FO260,315^FD" + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^A0N,22^FO330,315^FD" + "1" + "^FS";
                Zpl += "^A0N,20^FO150,248^FD" + BARCODE_INSPECT_DATE + "^FS";
                Zpl += "^A0N,30^FO010,115^FD" + BARCODE_SIZE_NAME + "^FS";
                Zpl += "^A0N,35^FO010,155^FD" + BARCODE_HEAT + "^FS";
            }
            else
            {
                switch (BARCODE_ITEM_SIZE.Trim())
                {
                    //case "001":
                    //case	"002":
                    //case	"003":
                    //case	"004":
                    //case	"005":
                    case "10B":
                    case "12A":
                    case "14A":
                    case "15A":
                    case "165":
                        Zpl += "^FO030,265^BY2,3.0^BCN,048,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                        Zpl += "^A0N,20^FO105,316^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS";
                        Zpl += "^A0N,20^FO260,315^FD" + BARCODE_INSPECTER_NAME + "^FS";
                        Zpl += "^A0N,22^FO330,315^FD" + "1" + "^FS";
                        Zpl += "^A0N,20^FO150,248^FD" + BARCODE_INSPECT_DATE + "^FS";
                        Zpl += "^A0N,30^FO010,115^FD" + BARCODE_SIZE_NAME + "^FS";
                        Zpl += "^A0N,35^FO010,155^FD" + BARCODE_HEAT + "^FS";
                        break;
                    default:
                        Zpl += "^FO030,265^BY2,3.0^BCN,070,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                        Zpl += "^A0N,20^FO105,345^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS";
                        //                 Zpl += "^A0N,25^FO260,343^FD" + BARCODE_INSPECTER_NAME + "^FS" ;
                        Zpl += "^A0N,25^FO245,343^FD" + BARCODE_INSPECTER_NAME + "^FS";
                        Zpl += "^A0N,25^FO330,343^FD" + "1" + "^FS";
                        Zpl += "^A0N,22^FO150,242^FD" + BARCODE_INSPECT_DATE + "^FS";
                        Zpl += "^A0N,30^FO010,112^FD" + BARCODE_SIZE_NAME + "^FS";
                        Zpl += "^A0N,35^FO010,147^FD" + BARCODE_HEAT + "^FS";
                        break;
                }
            }

            switch (Mid(BARCODE_STEEL, 0, 3))
            {
                case "SHN":
                    if (BARCODE_STEEL == "SHN490/SM490A" || BARCODE_STEEL == "SHN400/SS400/ASTM A36")
                    {
                        Zpl += "^FO430,030^BY2,3.0^BCB,090,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                        Zpl += "^FO540,090^A0B,23,30^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS";
                    }
                    else
                    {
                        Zpl += "^FO420,070^FR^XGR:NAEJIN,1,1^FS";
                        Zpl += "^FO520,030^BY2,3.0^BCB,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                    }
                    break;
                //20150911 이강현 추가
                case "300":
                    //	    	 alert(BARCODE_ITEM_SIZE.Trim());
                    if (BARCODE_ITEM_SIZE.Trim() == "010")
                    {
                        Zpl += "^A0N,19^FO240,005^FD" + "AS/NZS3679.1" + "^FS";         //호주향 강종일 시, ACRS 마크 상단AS/NZS3679.1 추가 (2016.09.09 원정호)
                        Zpl += "^FO260,020^FR^XGR:ACRS,1,1^FS";
                        Zpl += "^A0N,19^FO220,126^FD" + "Certi no : 150502" + "^FS";
                    }
                    else
                    {
                        Zpl += "^A0N,19^FO240,005^FD" + "AS/NZS3679.1" + "^FS";         //호주향 강종일 시, ACRS 마크 상단AS/NZS3679.1 추가 (2016.09.09 원정호)
                                                                                        //	    		 Zpl += "^FO325,003^FR^XGR:ACRS,1,1^FS" ;
                                                                                        //	    		 Zpl += "^A0N,19^FO285,110^FD" + "Certi no : 150502"+ "^FS" ;
                        Zpl += "^FO260,020^FR^XGR:ACRS,1,1^FS";
                        Zpl += "^A0N,19^FO220,126^FD" + "Certi no : 150502" + "^FS";
                    }
                    break;
                default:
                    Zpl += "^FO430,030^BY2,3.0^BCB,090,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                    Zpl += "^FO540,090^A0B,23,30^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS";
                    break;
            }

            Zpl += "^FO006,388^BY2,2.0^BCN,050,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,20^FO340,388^CI13^FR^FD" + BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,22^FO493,388^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N,20^FO340,429^FD" + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_COUNTRY_BASIS.Trim() == "KS")
            {
                Zpl += "^A0N,21^FO293,066^FD" + BARCODE_APPORVAL_NO + "^FS";

                //KS인증으로 인하여 강종별 KS인증 문구추가 (2016.09.27 원정호 추가)
                if (BARCODE_STEEL == "SHN400" || BARCODE_STEEL == "SHN490")
                {
                    Zpl += "^A0N,18^FO240,105^FD" + "Hot rolled H-beam" + "^FS";
                    Zpl += "^A0N,18^FO240,120^FD" + "For building structure" + "^FS";
                }
                else if (BARCODE_STEEL == "SM400A" || BARCODE_STEEL == "SM400B" || BARCODE_STEEL == "SM490A" || BARCODE_STEEL == "SM490B" || BARCODE_STEEL == "SM490YA" || BARCODE_STEEL == "SM490YB")
                {
                    Zpl += "^A0N,18^FO240,105^FD" + "Rolled steels" + "^FS";
                    Zpl += "^A0N,18^FO240,120^FD" + "For welded structures" + "^FS";
                }
                else if (Mid(BARCODE_STEEL, 0, 5) == "SS400" || Mid(BARCODE_STEEL, 0, 5) == "SS490")
                {
                    Zpl += "^A0N,18^FO240,105^FD" + "Rolled steels" + "^FS";
                    Zpl += "^A0N,18^FO240,120^FD" + "For general structure" + "^FS";
                }
                else
                {

                }

            }
            else if (BARCODE_COUNTRY_BASIS.Trim() == "JIS")
            {
                Zpl += "^A0N,23^FO280,080^FD" + BARCODE_APPORVAL_NO + "^FS";
            }
            else if (BARCODE_COUNTRY_BASIS.Trim() == "BV(선급)")
            {
                if (BARCODE_STEEL == "AH36")
                {
                    Zpl += "^A0N,23^FO282,080^FD" + BARCODE_APPORVAL_NO + "^FS";
                }
            }
            else if (BARCODE_COUNTRY_BASIS.Trim() == "ASME")
            { //--2013.02.18, SHARP, 김기원요청으로 추가
                Zpl += "^A0N,19^FO203,080^FD" + BARCODE_APPORVAL_NO + "^FS";
            }
            if (BARCODE_OLD_NEW_LABEL == "OLD")
            {
                switch (BARCODE_COUNTRY_BASIS.Trim().ToUpper())
                {
                    case "NO": break;
                    case "KS":
                        Zpl += "^FO295,005^FR^XGR:KS_PB51,1,1^FS";
                        Zpl += "^FO270,086^FD^XGR:KSA,1,1^FS";
                        break;
                    case "JIS":
                        Zpl += "^FO293,012^FR^XGJIS_7_1,1,1^FS";
                        Zpl += "^A0N,30^FO362,220^FD" + "2" + "^FS";
                        break;

                }
            }
            else
            {
                switch (BARCODE_ITEM_SIZE.Trim())
                {
                    //   	 case "001":
                    //   	 case "002":
                    //   	 case "003":
                    //   	 case "004":
                    //   	 case "005":
                    case "10B":
                    case "12A":
                    case "14A":
                    case "15A":
                    case "165":
                        switch (BARCODE_COUNTRY_BASIS.Trim().ToUpper())
                        {
                            case "NO": break;
                            case "KS":
                                Zpl += "^FO295,005^FR^XGR:KS_PB51,1,1^FS";
                                Zpl += "^FO285,086^FD^XGR:KSA,1,1^FS";
                                break;
                            case "JIS":
                                Zpl += "^FO293,012^FR^XGJIS_7_1,1,1^FS";
                                Zpl += "^A0N,30^FO362,220^FD" + "2" + "^FS";
                                break;
                        }
                        break;
                    default:
                        switch (BARCODE_COUNTRY_BASIS.Trim().ToUpper())
                        {
                            case "NO": break;
                            case "KS":
                                Zpl += "^FO305,003^FR^XGR:KS_PB51,1,1^FS";
                                Zpl += "^FO290,084^FD^XGR:KSA,1,1^FS";
                                break;
                            case "JIS":
                                Zpl += "^FO293,012^FR^XGJIS_7_1,1,1^FS";
                                Zpl += "^A0N,30^FO362,220^FD" + "2" + "^FS";
                                break;
                        }
                        break;
                }
            }

            if (BARCODE_PIECE_CNT == int.Parse(BARCODE_SP_CNT))
            {
                Zpl += "^MMC";
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }
            else
            {
                BARCODE_CUTTING_LOCAL_COUNT = BARCODE_CUTTING_LOCAL_COUNT + 1;

                if (BARCODE_CUTTING_LOCAL_COUNT == BARCODE_CUTTING_COUNT)
                {
                    Zpl += "^MMC";
                    BARCODE_CUTTING_LOCAL_COUNT = 0;
                }
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  PB5_HB_LABELC(int Count)
        {

            var Zpl = "";
            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "017" + "," + "017" + "^FS";
            Zpl += "^MMT";


            if (BARCODE_ITEM == "IP")
            {
                BARCODE_ITEM = "IPE";
            }
            else if (BARCODE_ITEM == "HB" || BARCODE_ITEM == "WX" || BARCODE_ITEM == "UB")
            {
                BARCODE_ITEM = "H-BEAM";
            }

            Zpl += "^A0N,35^FO020,025^FD" + BARCODE_ITEM + "^FS";

            //     alert(BARCODE_STEEL.Length);

            if (BARCODE_STEEL.Length >= 10 && BARCODE_STEEL.Length <= 13)
            {
                Zpl += "^A0N,35^FO152,070^FD" + BARCODE_STEEL + "^FS";
            }
            else if (BARCODE_STEEL.Length >= 14)
            {
                Zpl += "^A0N,35^FO107,070^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^A0N,35^FO185,070^FD" + BARCODE_STEEL + "^FS";
            }

            Zpl += "^A0N,20^FO340,409^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N,22^FO020,065^FD" + Mid(BARCODE_SIZE_CODE, 0, 10) + "^FS";
            Zpl += "^A0N,22^FO020,085^FD" + Mid(BARCODE_SIZE_CODE, 10, 7) + "^FS";

            if (int.Parse(BARCODE_Length) < 4)
            {
                Zpl += "^A0N,45^FO270,155^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,45^FO327,155^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N,25^FO285,343^FD" + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^A0N,23^FO495,429^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,23^FO520,429^FD" + BARCODE_UOM + "^FS";
            }
            else if (int.Parse(BARCODE_Length) == 4)
            {
                Zpl += "^A0N,45^FO246,155^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,45^FO328,155^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N,25^FO285,343^FD" + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^A0N,23^FO495,429^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,23^FO540,429^FD" + BARCODE_UOM + "^FS";
            }
            else if (int.Parse(BARCODE_Length) >= 5)
            {
                Zpl += "^A0N,45^FO248,155^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,45^FO327,155^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N,25^FO285,343^FD" + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^A0N,23^FO495,429^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N,23^FO555,429^FD" + BARCODE_UOM + "^FS";
            }

            Zpl += "^A0N,35^FO020,115^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^A0N,30^FO020,172^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^FO040,280^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,18^FO105,348^CI13^FR^FD" + BARCODE_PO_NO + " " + BARCODE_BUNDLE_NO + "^FS";

            if (BARCODE_COUNTRY_BASIS.Trim() == "KS")
            {
                Zpl += "^A0N,23^FO290,080^FD" + BARCODE_APPORVAL_NO + "^FS";
            }

            Zpl += "^A0N,25^FO330,343^FD" + "1" + "^FS";
            Zpl += "^A0N,25^FO150,260^FD" + BARCODE_INSPECT_DATE + "^FS";
            Zpl += "^FO006,392^BY2,2.0^BCN,050,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,20^FO340,392^CI13^FR^FD" + BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,22^FO493,392^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N,20^FO340,433^FD" + BARCODE_SIZE_NAME + "^FS";



            switch (BARCODE_COUNTRY_BASIS.Trim().ToUpper())
            {
                case "NO": break;
                case "KS":
                    Zpl += "^FO282,000^FR^XGR:KS_PB51,1,1^FS";
                    Zpl += "^FO266,081^FD^XGR:KSA,1,1^FS";
                    break;
                case "JIS":
                    Zpl += "^FO298,012^FR^XGJIS_7_1,1,1^FS";
                    break;
            }

            if (BARCODE_PIECE_CNT == int.Parse(BARCODE_SP_CNT))
            {
                Zpl += "^MMC";
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }
            else
            {
                BARCODE_CUTTING_LOCAL_COUNT = BARCODE_CUTTING_LOCAL_COUNT + 1;

                if (BARCODE_CUTTING_LOCAL_COUNT == BARCODE_CUTTING_COUNT)
                {
                    Zpl += "^MMC";
                    BARCODE_CUTTING_LOCAL_COUNT = 0;
                }
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  BLANK_TAG(int Count)
        {
            var Zpl = "";
            Zpl = "";
            Zpl += "^XA";
            Zpl += "^A0N140,35^FO010,022^FD" + "Blank" + "^FS";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }
        
        public string  PB5_LABEL_TAG_00(int Count)
        {
            var Zpl = "";

            //var Barcode = "";
            //var PoomMyoung = "";
            //var ItemSize = "";
            //var i, cnt, Print_CNT ;
            //var STEEL_Length;



            Zpl = "";
            Zpl += "^XA";
            Zpl += "^PW830";
            Zpl += "^LH" + "003" + "," + "000" + "^FS";
            Zpl += "^FO001,008^FD^XGR:HD,1,1^FS";

            if (BARCODE_ITEM == "AU" || BARCODE_ITEM == "AE")
            {
                if (BARCODE_STEEL.Length > 4)
                {
                    Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,31^FO267,013^FD" + BARCODE_STEEL + "^FS";
                }
                else
                {
                    Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,34^FO277,020^FD" + BARCODE_STEEL + "^FS";
                }

                if (BARCODE_HEAT_ALL_PRT == "Y" && BARCODE_ITEM == "AU")
                {
                    Zpl += "^A0N130,22^FO084,038^FD" + Mid(BARCODE_BD_NO, 0, 6) + " - " + Mid(BARCODE_BD_NO, 6, 5) + "   " + BARCODE_HEAT + "^FS";
                }
                else
                {
                    Zpl += "^A0N130,22^FO084,038^FD" + Mid(BARCODE_BD_NO, 0, 6) + " - " + Mid(BARCODE_BD_NO, 6, 5) + "^FS";
                }

                Zpl += "^FO388,015^BY2,3.0^BCN,050,N,Y,N^FR^FD" + BARCODE_BD_NO + "^FS";


            }
            else
            {
                if (BARCODE_STEEL.Length > 4)
                {
                    Zpl += "^A0N130,24^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO270,020^FD" + BARCODE_STEEL + "^FS";
                }
                else
                {
                    Zpl += "^A0N130,24^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO280,020^FD" + BARCODE_STEEL + "^FS";
                }
                Zpl += "^A0N130,22^FO085,038^FD" + Mid(BARCODE_BD_NO, 0, 6) + " - " + Mid(BARCODE_BD_NO, 6, 5) + "^FS";
                Zpl += "^FO393,015^BY2,3.0^BCN,050,N,Y,N^FR^FD" + BARCODE_BD_NO + "^FS";
            }

            //if(BARCODE_PIECE_CNT == Print_CNT ) Print_CNT 넣어주는곳이 없는데 비교하여서 체크
            if (BARCODE_PIECE_CNT == Count)
            {
                Zpl += "^MMC";
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        public string  PB5_LABEL_TAG_01(int Count)
        {
            var Zpl = "";
            var Print_CNT = 0;
                        
            switch (BARCODE_PROGRAM)
            {
                case "10792":
                    Print_CNT = 1;
                    break;
                default:
                    Print_CNT = Convert.ToInt32(BARCODE_SP_CNT) - 1;
                    break;
            }

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^PW830" + "\n";
            Zpl += "^LH" + "003" + "," + "000" + "^FS" + "\n";
            Zpl += "^FO001,008^FD^XGR:HD,1,1^FS" + "\n";

            //	alert(BARCODE_HEAT_ALL_PRT);
            //	alert(BARCODE_STEEL.Length);

            if (BARCODE_ITEM == "AU" || BARCODE_ITEM == "AE")
            {
                //		 if (BARCODE_STEEL.Length > 4){
                //
                //			 if(BARCODE_SHIP_NO =="Y"){
                //				 Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,31^FO258,013^FD" + "LR " + BARCODE_STEEL + "^FS" +"\n";
                //			 }else{
                //				 Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,31^FO262,013^FD" + BARCODE_STEEL + "^FS"+"\n" ;
                //			 }
                //		 }else{
                //			 if(BARCODE_SHIP_NO =="Y"){
                //				 Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N117,32^FO273,013^FD" + "LR " + BARCODE_STEEL + "^FS"+"\n" ;
                //			 }else{
                //				 Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,34^FO273,013^FD" + BARCODE_STEEL + "^FS"+"\n" ;
                //			 }
                //		 }
                if (BARCODE_STEEL.Length > 8)
                {
                    if (BARCODE_SHIP_NO == "Y")
                    {
                        Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,18^FO250,019^FD" + "LR " + BARCODE_STEEL + "^FS" + "\n";
                    }
                    else
                    {
                        Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,18^FO252,017^FD" + BARCODE_STEEL + "^FS" + "\n";
                    }
                }
                else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                {
                    if (BARCODE_SHIP_NO == "Y")
                    {
                        Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,31^FO258,013^FD" + "LR " + BARCODE_STEEL + "^FS" + "\n";
                    }
                    else
                    {
                        Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,31^FO262,013^FD" + BARCODE_STEEL + "^FS" + "\n";
                    }
                }
                else
                {
                    if (BARCODE_SHIP_NO == "Y")
                    {
                        Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N117,32^FO273,013^FD" + "LR " + BARCODE_STEEL + "^FS" + "\n";
                    }
                    else
                    {
                        Zpl += "^A0N130,22^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,34^FO273,013^FD" + BARCODE_STEEL + "^FS" + "\n";
                    }
                }

                //		//--강종명 자리수 증가(9자리이상)에 따른 로직반영 - 2014.02.25, SHARP, 김기원요청(GQ - LT-FH32MC  등)
                //		if(BARCODE_STEEL.Length>8){
                //			if(BARCODE_SHIP_NO =="Y"){
                //				Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO330,417^FD" + "LR " + BARCODE_STEEL + "^FS";
                //			}else{
                //				Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO330,417^FD" + BARCODE_STEEL + "^FS" ;
                //			}
                //		}else if(BARCODE_STEEL.Length >= 5 &&  BARCODE_STEEL.Length <= 8){
                //			if(BARCODE_SHIP_NO =="Y"){
                //				Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,417^FD" + "LR " + BARCODE_STEEL + "^FS";
                //			}else{
                //				Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,417^FD" + BARCODE_STEEL + "^FS" ;
                //			}
                //		}
                if (BARCODE_HEAT_ALL_PRT == "Y" && (BARCODE_ITEM == "AU" || BARCODE_ITEM == "AE"))
                {
                    if (BARCODE_SHIP_NO == "Y")
                    {
                        Zpl += "^A0N128,22^FO060,038^FD" + BARCODE_BD_NO.Substring(0, 6) + "-" + BARCODE_BD_NO.Substring(6, 5) + " " + BARCODE_HEAT + " Q526" + "^FS" + "\n";
                    }
                    else
                    {
                        Zpl += "^A0N128,22^FO060,038^FD" + BARCODE_BD_NO.Substring(0, 6) + "-" + BARCODE_BD_NO.Substring(6, 5) + " " + BARCODE_HEAT + "^FS" + "\n";
                    }
                }
                else
                {
                    Zpl += "^A0N128,22^FO060,038^FD" + BARCODE_BD_NO.Substring(0, 6) + "-" + BARCODE_BD_NO.Substring(6, 5) + "^FS" + "\n";
                }
                Zpl += "^FO388,015^BY2,3.0^BCN,050,N,Y,N^FR^FD" + BARCODE_BD_NO + "^FS" + "\n";
            }
            else
            {
                if (BARCODE_STEEL.Length > 4)
                {
                    Zpl += "^A0N130,24^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO270,020^FD" + BARCODE_STEEL + "^FS" + "\n";
                }
                else
                {
                    Zpl += "^A0N130,24^FO060,015^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO280,020^FD" + BARCODE_STEEL + "^FS" + "\n";
                }

                Zpl += "^A0N130,22^FO085,038^FD" + BARCODE_BD_NO.Substring(0, 6) + " - " + BARCODE_BD_NO.Substring(6, 5) + "^FS" + "\n";
                Zpl += "^FO393,015^BY2,3.0^BCN,050,N,Y,N^FR^FD" + BARCODE_BD_NO + "^FS" + "\n";

                //MessageBox.Show(BARCODE_BD_NO);
            }
            //MessageBox.Show(BARCODE_PIECE_CNT + " " + Print_CNT);
            if (BARCODE_PIECE_CNT == Print_CNT)
            {
                Zpl += "^MMC" + "\n";
            }

            Zpl += "^XZ" + "\n";
            Zpl += "^XA" + "\n";
            Zpl += "^MMT" + "\n";
            Zpl += "^XZ" + "\n";

            return Zpl;
        }

        public string  DB_ATOMIC_TAG(int Count)
        {
            var Zpl = "";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^A0N050,40^FO063,030^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N050,40^FO247,030^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N050,40^FO432,030^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N050,40^FO617,030^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N050,40^FO047,070^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N050,40^FO232,070^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N050,40^FO417,070^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N050,40^FO597,070^FD" + BARCODE_HEAT + "^FS";

            if (BARCODE_Length.Length > 3)
            {
                Zpl += "^A0N035,30^FO056,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N035,30^FO241,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N035,30^FO426,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N035,30^FO612,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N035,30^FO122,110^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N035,30^FO307,110^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N035,30^FO491,110^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N035,30^FO678,110^FD" + BARCODE_UOM + "^FS";
            }
            else
            {
                Zpl += "^A0N050,40^FO067,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N050,40^FO252,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N050,40^FO437,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N050,40^FO623,110^FD" + BARCODE_Length + "^FS";
                Zpl += "^A0N050,40^FO104,110^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N050,40^FO289,110^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N050,40^FO473,110^FD" + BARCODE_UOM + "^FS";
                Zpl += "^A0N050,40^FO660,110^FD" + BARCODE_UOM + "^FS";

            }

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC"; // '절단
            }
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        public string  RAIL_SIDE_TAG(int Count)
        {
            var Zpl = "";

            Zpl += "^XA";
            Zpl += "^LH" + "003" + "," + "000" + "^FS";
            Zpl += "^MMT";
            Zpl += "^FO003,020^FD^XGR:HD,1,1^FS";
            Zpl += "^A0N100,36^FO070,006^FD" + "RL MLT 162kg/m   " + "^FS";
            Zpl += "^A0N100,36^FO350,006^FD" + BARCODE_STEEL + "^FS";

            switch (int.Parse(BARCODE_Length))
            {
                case 5:
                    Zpl += "^A0N067,067^FO496,005^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
                    break;
                case 4:
                    Zpl += "^A0N070,70^FO520,005^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
                    break;
                case 2:
                    BARCODE_Length = BARCODE_Length + ".0";
                    Zpl += "^A0N100,70^FO520,005^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
                    break;
            }


            Zpl += "^A0N046,46^FO070,050^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^FO230,058^A0N,30,30^CI13^FR^FD" + BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N028,28^FO440,068^FD" + Mid(BARCODE_INSPECT_DATE, 0, 4) + "-" + Mid(BARCODE_INSPECT_DATE, 4, 2) + "-" + Mid(BARCODE_INSPECT_DATE, 6, 2) + "^FS";
            Zpl += "^A0N030,30^FO640,068^FD" + BARCODE_INSPECTER_NAME + "^FS";

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC"; // '절단
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }
        
        public string  DB_EXPORT_NC_SHIPPING(int Count)
        {
            var Zpl = "";
            var MM = "";

            if (BARCODE_SPEC2 == "")
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";


                Zpl += "^A0N,50,50^FO040,035^FD" + "DEFORMED BAR " + "^FS";
                Zpl += "^A0N,31,31^FO001,120^FD" + BARCODE_MEMO1 + "^FS";
                Zpl += "^A0N,31,31^FO001,155^FD" + BARCODE_MEMO2 + "^FS";
                Zpl += "^A0N,31,31^FO001,190^FD" + BARCODE_MEMO3 + "^FS";
                Zpl += "^A0N,31,31^FO001,225^FD" + BARCODE_MEMO4 + "^FS";

                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }

                Zpl += "^A0N,31,31^FO001,281^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";
                Zpl += "^A0N,31,31^FO001,327^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";
                Zpl += "^A0N,31,31^FO300,327^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";
                Zpl += "^A0N,31,31^FO001,373^FD" + "SPEC      : " + "^FS" + "^A0N,29,29^FO140,373^FD" + BARCODE_SPEC + "^FS";

                if (BARCODE_HEAT_DSP == "0")
                {
                    Zpl += "^A0N,31,31^FO001,419^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {
                    Zpl += "^A0N,31,31^FO300,419^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }

                Zpl += "^FO000,481^FD^XGR:HD,1,1^FS";
                Zpl += "^A0N,33,33^FO060,491^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
                Zpl += "^A0N,31,31^FO400,529^FD" + BARCODE_BUNDLE_NO + "^FS";
            }

            else
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";

                Zpl += "^A0N,50,50^FO040,035^FD" + "DEFORMED BAR " + "^FS";
                Zpl += "^A0N,31,31^FO001,120^FD" + BARCODE_MEMO1 + "^FS";
                Zpl += "^A0N,31,31^FO001,155^FD" + BARCODE_MEMO2 + "^FS";
                Zpl += "^A0N,31,31^FO001,190^FD" + BARCODE_MEMO3 + "^FS";
                Zpl += "^A0N,31,31^FO001,225^FD" + BARCODE_MEMO4 + "^FS";

                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }

                Zpl += "^A0N,31,31^FO001,281^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";
                Zpl += "^A0N,31,31^FO001,317^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";
                Zpl += "^A0N,31,31^FO300,317^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";
                Zpl += "^A0N,31,31^FO001,353^FD" + "SPEC      : " + "^FS" + "^A0N,29,29^FO140,353^FD" + BARCODE_SPEC + "^FS";
                Zpl += "^A0N,31,31^FO001,389^FD" + "                 " + BARCODE_SPEC2 + "^FS";

                if (BARCODE_HEAT_DSP == "0")
                {
                    Zpl += "^A0N,31,31^FO001,419^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {
                    Zpl += "^A0N,31,31^FO300,425^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }

                Zpl += "^FO000,481^FD^XGR:HD,1,1^FS";
                Zpl += "^A0N,33,33^FO060,491^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
                Zpl += "^A0N,31,31^FO400,529^FD" + BARCODE_BUNDLE_NO + "^FS";
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }
        
        public string  DB_EXPORT_STEEL_SHIPPING(int Count)
        {
            var Zpl = "";
            var MM = "";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            if (BARCODE_UK_DSP == "0")
            {
                Zpl += "^A0N,50,50^FO080,050^FD" + "DEFORMED BAR " + "^FS";
            }

            Zpl += "^A0N,35,35^FO010,140^FD" + BARCODE_MEMO1 + "^FS";
            Zpl += "^A0N,35,35^FO010,175^FD" + BARCODE_MEMO2 + "^FS";
            Zpl += "^A0N,35,35^FO010,210^FD" + BARCODE_MEMO3 + "^FS";
            Zpl += "^A0N,35,35^FO010,245^FD" + BARCODE_MEMO4 + "^FS";

            if (BARCODE_ITEM == "DB")
            {
                MM = " MM";
            }
            else if (BARCODE_ITEM == "#")
            {
                MM = "";
            }

            Zpl += "^A0N,35,35^FO010,316^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";
            Zpl += "^A0N,35,35^FO010,362^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";
            Zpl += "^A0N,31,31^FO350,362^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";
            Zpl += "^A0N,35,35^FO010,408^FD" + "SPEC      : " + BARCODE_SPEC + "^FS";

            if (BARCODE_HEAT_DSP == "0")
            {
                Zpl += "^A0N,35,35^FO010,454^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
            }


            if (BARCODE_WEIGHT_DSP == "0")
            {
                Zpl += "^A0N,35,35^FO370,454^FD" + BARCODE_WEIGHT + "kg" + "^FS";
            }

            Zpl += "^FO015,516^FD^XGR:HD,1,1^FS";
            Zpl += "^A0N,35,35^FO079,526^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
            Zpl += "^A0N,35,35^FO460,552^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  DB_EXPORT_HISTORY_SHIPPING(int Count)
        {
            var Zpl = "";
            var MM = "";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            if (BARCODE_ITEM == "DB")
            {
                MM = " MM";
            }
            else if (BARCODE_ITEM == "#")
            {
                MM = "";
            }


            Zpl += "^FO001,002^GB430,0,8^FS";
            Zpl += "^FO001,080^GB430,0,2^FS";
            Zpl += "^FO001,156^GB430,0,2^FS";
            Zpl += "^FO001,232^GB430,0,8^FS";
            Zpl += "^FO001,002^GB0,232,8^FS";
            Zpl += "^FO132,002^GB0,232,3^FS";
            Zpl += "^FO430,002^GB0,237,8^FS";

            Zpl += "^A0N,30,32^FO037,034^FD" + "SPEC" + "^FS";
            Zpl += "^A0N,26,30^FO143,034^FD" + BARCODE_SPEC + "^FS";
            Zpl += "^A0N,30,32^FO041,105^FD" + "SIZE" + "^FS";
            Zpl += "^A0N,30,32^FO143,105^FD" + BARCODE_ITEM_SIZE + MM + "   " + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl += "^A0N,30,32^FO011,184^FD" + "HEAT NO" + "^FS";
            Zpl += "^A0N,34,36^FO143,184^FD" + BARCODE_HEAT + "            " + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        //2013.04.18, SHARP
        public string  DB_EXPORT_PDF417_NC_SHIPPING(int Count)
        {
            var Zpl = "";
            var MM = "";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            Zpl += "^A0N,40,40^FO040,035^FD" + "DEFORMED BAR " + "^FS";
            Zpl += "^A0N,29,29^FO001,080^FD" + BARCODE_MEMO1 + "^FS";
            Zpl += "^A0N,29,29^FO001,110^FD" + BARCODE_MEMO2 + "^FS";
            Zpl += "^A0N,29,29^FO001,140^FD" + BARCODE_MEMO3 + "^FS";
            Zpl += "^A0N,29,29^FO001,170^FD" + BARCODE_MEMO4 + "^FS";

            Zpl += "^A0N,29,29^FO001,230^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";
            Zpl += "^A0N,29,29^FO300,230^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";
            Zpl += "^A0N,29,29^FO001,260^FD" + "SPEC      : " + "^FS";
            Zpl += "^A0N,29,29^FO130,260^FD" + BARCODE_SPEC + "^FS";

            //SPEC2가 없는경우
            if (BARCODE_SPEC2 == "")
            {
                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }
                Zpl += "^A0N,29,29^FO001,200^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";

                if (BARCODE_HEAT_DSP == "0")
                {
                    Zpl += "^A0N,29,29^FO001,290^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {
                    Zpl += "^A0N,29,29^FO300,290^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }
            }
            //SPEC2가 있는경우
            else
            {
                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }
                Zpl += "^A0N,29,29^FO001,200^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";
                Zpl += "^A0N,29,29^FO130,290^FD" + BARCODE_SPEC2 + "^FS";
                if (BARCODE_HEAT_DSP == "0")
                {
                    Zpl += "^A0N,29,29^FO001,320^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {
                    Zpl += "^A0N,29,29^FO300,320^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }
            }
            var PDF417 = BARCODE_MEMO1 + BARCODE_MEMO2 + BARCODE_MEMO3 + BARCODE_MEMO4 + BARCODE_ITEM_SIZE + MM + BARCODE_Length + BARCODE_UOM + BARCODE_PER_BONSU + "pcs" + BARCODE_SPEC + BARCODE_SPEC2
             + BARCODE_HEAT + BARCODE_WEIGHT;

            Zpl += " ^BY2.5,2.5^FO005,355^B7N,5,4,,100,N  ^FD" + PDF417 + "^FS";
            Zpl += "^FO000,481^FD^XGR:HD,1,1^FS";
            Zpl += "^A0N,33,33^FO060,491^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
            Zpl += "^A0N,29,29^FO400,529^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }
        
        //2013.04.18, SHARP
        public string  DB_EXPORT_PDF417_STEEL_SHIPPING(int Count)
        {
            var Zpl = "";
            var MM = "";
            var PDF417 = "5095192208PB5KOREAIIIDS16400WCSA-G30.18-M92400W1A22301872";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            if (BARCODE_UK_DSP == "0")
            {
                Zpl += "^A0N,40,40^FO040,035^FD" + "DEFORMED BAR " + "^FS";
            }

            Zpl += "^A0N,33,33^FO010,80^FD" + BARCODE_MEMO1 + "^FS";
            Zpl += "^A0N,33,33^FO010,120^FD" + BARCODE_MEMO2 + "^FS";
            Zpl += "^A0N,33,33^FO010,160^FD" + BARCODE_MEMO3 + "^FS";
            Zpl += "^A0N,33,33^FO010,200^FD" + BARCODE_MEMO4 + "^FS";

            if (BARCODE_ITEM == "DB")
            {
                MM = " MM";
            }
            else if (BARCODE_ITEM == "#")
            {
                MM = "";
            }

            Zpl += "^A0N,33,33^FO010,240^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";
            Zpl += "^A0N,33,33^FO010,280^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";
            Zpl += "^A0N,33,33^FO350,280^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";
            Zpl += "^A0N,33,33^FO010,320^FD" + "SPEC      : " + BARCODE_SPEC + "^FS";

            if (BARCODE_HEAT_DSP == "0")
            {
                Zpl += "^A0N,33,33^FO010,360^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
            }


            if (BARCODE_WEIGHT_DSP == "0")
            {
                Zpl += "^A0N,33,33^FO370,360^FD" + BARCODE_WEIGHT + "kg" + "^FS";
            }

            Zpl += " ^BY2.5,2.5^FO005,395^B7N,5,4,,100,N  ^FD" + PDF417 + "^FS";
            Zpl += "^FO015,516^FD^XGR:HD,1,1^FS";
            Zpl += "^A0N,35,35^FO079,526^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
            Zpl += "^A0N,33,33^FO460,552^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        public string  DB_EXPORT_PDF417_HISTORY_SHIPPING(int Count)
        {
            var Zpl = "";
            var MM = "";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            if (BARCODE_ITEM == "DB")
            {
                MM = " MM";
            }
            else if (BARCODE_ITEM == "#")
            {
                MM = "";
            }


            Zpl += "^FO001,002^GB430,0,8^FS";
            Zpl += "^FO001,080^GB430,0,2^FS";
            Zpl += "^FO001,156^GB430,0,2^FS";
            Zpl += "^FO001,232^GB430,0,8^FS";
            Zpl += "^FO001,002^GB0,232,8^FS";
            Zpl += "^FO132,002^GB0,232,3^FS";
            Zpl += "^FO430,002^GB0,237,8^FS";

            Zpl += "^A0N,30,32^FO037,034^FD" + "SPEC" + "^FS";
            Zpl += "^A0N,26,30^FO143,034^FD" + BARCODE_SPEC + "^FS";
            Zpl += "^A0N,30,32^FO041,105^FD" + "SIZE" + "^FS";
            Zpl += "^A0N,30,32^FO143,105^FD" + BARCODE_ITEM_SIZE + MM + "   " + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl += "^A0N,30,32^FO011,184^FD" + "HEAT NO" + "^FS";
            Zpl += "^A0N,34,36^FO143,184^FD" + BARCODE_HEAT + "            " + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        public string  NUCLEAR_TAG(int Count)
        {
            var Zpl = "";
            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";
            Zpl += "^A0N,40,40^FO130,180^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^A0N,40,40^FO130,250^FD" + BARCODE_Length + " " + BARCODE_UOM + "^FS";
            Zpl += "^A0N,40,40^FO220,250^FD" + BARCODE_Length2 + " " + BARCODE_UOM2 + "^FS";
            Zpl += "^A0N,40,40^FO130,305^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N,40,40^FO355,305^FD" + BARCODE_BONSU + "^FS";
            Zpl += "^A0N,40,40^FO130,370^FD" + BARCODE_WEIGHT + "^FS";
            Zpl += "^A0N,40,40^FO355,370^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";
            return Zpl;
        }

        public string  RB_DOMESTIC(int Count)
        {
            var Zpl = "";

            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "00" + "," + "00" + "^MMT";
            Zpl += "^A0N,40,40^FO035,140^FD" + "ITEM :" + "^FS";
            Zpl += "^A0N,40,40^FO035,190^FD" + BARCODE_ITEM + "  " + BARCODE_ITEM_SIZE + "^FS";
            Zpl += "^A0N,40,40^FO285,140^FD" + "STEEL :" + "^FS";
            Zpl += "^A0N,40,40^FO285,190^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N,40,40^FO035,245^FD" + "HEAT :" + "^FS";
            Zpl += "^A0N,40,40^FO035,285^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N,40,40^FO285,245^FD" + "Length :" + "^FS";
            Zpl += "^A0N,40,40^FO285,285^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl += "^A0N,40,40^FO035,350^FD" + "B/D NO :" + "^FS";
            Zpl += "^A0N,40,40^FO035,390^FD" + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,40,40^FO285,350^FD" + "BONSU :" + "^FS";
            Zpl += "^A0N,40,40^FO285,390^FD" + BARCODE_BONSU + "^FS";
            Zpl += "^A0N,40,40^FO035,455^FD" + "WEIGHT :" + "^FS";
            Zpl += "^A0N,40,40^FO035,495^FD" + BARCODE_WEIGHT + "^FS";
            Zpl += "^A0N,40,40^FO285,455^FD" + "DATE :" + "^FS";
            Zpl += "^A0N,40,40^FO285,495^FD" + BARCODE_INSPECT_DATE + "^FS";



            if (BARCODE_PRINT_COUNT == Count)
            {
                Zpl += "^MMC";
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        public string  STEEL_DOMESTIC_NEW(int Count)
        {

            var Zpl = "";
            // 2012.11.23 신규라벨발행로직
            if (BARCODE_chkNEW_LABEL == "Y")
            {
                Zpl = "";
                //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
                Zpl += "^XA";
                Zpl += "^PR 2";
                Zpl += "^XZ";

                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";
                Zpl += "^A0N,40,40^FO160,200^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^A0N,40,40^FO160,260^FD" + BARCODE_ITEM_SIZE + " X " + BARCODE_Length + BARCODE_UOM + "^FS";
                Zpl += "^A0N,40,40^FO160,316^FD" + BARCODE_HEAT + "^FS";
                Zpl += "^A0N,40,40^FO160,372^FD" + BARCODE_BONSU + "^FS";
                Zpl += "^A0N,40,40^FO335,430^FD" + BARCODE_WEIGHT + "^FS";
                if (BARCODE_BUNDLE_NO_DSP == "1")
                {
                    Zpl += "^A0N,40,40^FO370,372^FD" + BARCODE_PRT_BUNDLE_NO + "^FS";
                }

                Zpl += "^A0N,32,32^FO155,557^FD" + BARCODE_INSPECT_DATE + "       " + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^FO037,590^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                Zpl += "^A0N,24^FO355,613^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                Zpl += "^XZ";

                return Zpl;

            }
            else
            {
                Zpl = "";
                //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
                Zpl += "^XA";
                Zpl += "^PR 2";
                Zpl += "^XZ";

                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";

                // 2016.08.31 철근 ks 개정으로 인한 추가_구태그로 발행할 경우만 실행
                Zpl += "^A0N,25,25^FO340,85^FD2016^FS ";

                Zpl += "^A0N,40,40^FO160,200^FD" + BARCODE_STEEL + "^FS";

                Zpl += "^A0N,40,40^FO160,260^FD" + BARCODE_ITEM_SIZE + " X " + BARCODE_Length + BARCODE_UOM + "^FS";
                Zpl += "^A0N,40,40^FO160,316^FD" + BARCODE_HEAT + "^FS";
                Zpl += "^A0N,40,40^FO160,372^FD" + BARCODE_BONSU + "^FS";
                Zpl += "^A0N,40,40^FO335,430^FD" + BARCODE_WEIGHT + "^FS";
                if (BARCODE_BUNDLE_NO_DSP == "1")
                {
                    Zpl += "^A0N,40,40^FO370,372^FD" + BARCODE_PRT_BUNDLE_NO + "^FS";
                }

                Zpl += "^A0N,32,32^FO155,557^FD" + BARCODE_INSPECT_DATE + "       " + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^FO037,590^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                Zpl += "^A0N,24^FO355,613^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
                Zpl += "^XZ";

                return Zpl;

                /* 20160831 구성욱 주석 처리
                //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
                Zpl += "^XA" ;
                Zpl += "^PR 2" ;
                Zpl += "^XZ" ;

                Zpl += "^XA" ;
                Zpl += "^LH" +"10" +"," +"10" +"^MMT" ;
                switch(BARCODE_PRINTED_BASIS.Trim().ToUpper()){
                case "NO":
                    break;
                case "KS":
                    Zpl += "^A0N,22,22^FO001,100^FD" +"- KSA871 -" +"^FS" ;
                    break;
                case "JIS":
                    break;
                }

                Zpl += "^A0N,40,40^FO120,188^FD" +BARCODE_STEEL +"^FS" ;

                switch(BARCODE_PRINTED_BASIS.Trim().ToUpper()){
                case "NO":
                    break;
                case "KS":
                    Zpl += "^A0N,40,40^FO260,188^FD" +"KS D3504" +"^FS" ;
                    break;
                case "비KS":
                     Zpl += "^A0N,40,40^FO260,188^FD" +"KS D3504" +"^FS" ;
                    break;
                case "JIS":
                    break;
                }

                Zpl += "^A0N,40,40^FO150,2400^FD" +BARCODE_ITEM_SIZE +" X " +BARCODE_Length +BARCODE_UOM +"^FS" ;
                Zpl += "^A0N,40,40^FO115,295^FD" +BARCODE_HEAT +"^FS" ; //           'HEAT_NO
                Zpl += "^A0N,40,40^FO337,295^FD" +BARCODE_BONSU +"^FS" ; //          '본수
                Zpl += "^A0N,40,40^FO115,347^FD" +BARCODE_WEIGHT +"^FS" ; //         '중량

                if(BARCODE_BUNDLE_NO_DSP=="1"){
                    Zpl += "^A0N,40,40^FO337,347^FD" +BARCODE_PRT_BUNDLE_NO +"^FS" ;
                }

                Zpl += "^A0N,40,40^FO075,394^FD" +BARCODE_INSPECT_DATE +"  " +BARCODE_INSPECTER_NAME +"^FS" ;
                Zpl += "^FO020,495^BY2,3.0^BCN,060,N,Y,N^FR^FD" +BARCODE_PO_NO +BARCODE_BUNDLE_NO +"^FS" ;
                Zpl += "^A0N,20^FO337,518^CI13^FR^FD" +BARCODE_PO_NO +BARCODE_BUNDLE_NO +"^FS" ;

                switch(BARCODE_PRINTED_BASIS.Trim().ToUpper()){
                    case "NO":
                        break;
                    case "KS":
                        Zpl += "^FO030,030^FD^XGR:KS11,1,1^FS" ;
                        Zpl += "^FO340,100^FD^XGR:KSA,1,1^FS" ; //
                        break;
                    case "JIS":
                        break;
                }

                Zpl += "^FO355,045^GB065,0,50^FS"   ; //
                Zpl += "^XZ" ; //

                return Zpl;
                */
            }
        }

        public string  STEEL_JIS_DOMESTIC_NEW(int Count)
        {
            var Zpl = "";

            //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
            Zpl += "^XA";
            Zpl += "^PR 2";
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            Zpl += "^FO004,045^GB435,525,7^FS";
            Zpl += "^FO008,148^GB429,0,2^FS";
            Zpl += "^FO011,190^GB429,0,2^FS";
            Zpl += "^FO008,238^GB429,0,2^FS";
            Zpl += "^FO008,289^GB429,0,2^FS";
            Zpl += "^FO004,336^GB433,0,2^FS";
            Zpl += "^FO004,383^GB430,0,2^FS";

            Zpl += "^FO118,148^GB0,237,2^FS";
            Zpl += "^FO246,289^GB0,095,2^FS";
            Zpl += "^FO329,289^GB0,095,2^FS";

            Zpl += "^A0N,20,20^FO275,110^FDAPPROVED No^FS";
            Zpl += "^A0N,20,20^FO275,130^FDKR 8749^FS";
            Zpl += "^A0N,32,25^FO14,158^FDARTICLE^FS";
            Zpl += "^A0N,36,27^FO125,158^FDDEFORMED STEEL BAR^FS";
            Zpl += "^A0N,32,25^FO17,204^FDGRADE^FS";
            Zpl += "^A0N,32,25^FO20,253^FDSIZE^FS";
            Zpl += "^A0N,32,25^FO14,302^FDHEAT No^FS";
            Zpl += "^A0N,32,25^FO262,302^FDPCS^FS";
            Zpl += "^A0N,32,25^FO15,349^FDWEIGHT^FS";
            Zpl += "^A0N,32,25^FO258,349^FDBD No^FS";

            Zpl += "^A0N,37,35^FO125,202^FD" + BARCODE_STEEL + "^FS";

            if (BARCODE_PRINTED_BASIS.Trim().ToUpper() == "JIS")
            {
                Zpl += "^A0N,37,30^FO272,202^FD" + "JIS G3112" + "^FS";
            }

            Zpl += "^A0N,37,35^FO125,251^FD" + BARCODE_ITEM_SIZE + " X " + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl += "^A0N,37,35^FO121,299^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N,37,35^FO349,299^FD" + BARCODE_BONSU + "^FS";
            Zpl += "^A0N,37,35^FO132,346^FD" + BARCODE_WEIGHT + "^FS";
            Zpl += "^A0N,37,35^FO352,346^FD" + BARCODE_PRT_BUNDLE_NO.PadLeft(4, '0') + "^FS";
            Zpl += "^A0N,32,32^FO122,390^FD" + BARCODE_INSPECT_DATE + "  " + BARCODE_INSPECTER_NAME + "^FS";

            if (BARCODE_PRINTED_BASIS.Trim().ToUpper() == "JIS")
            {
                Zpl += "^FO170,50^FD^XGKI006,1,1^FS";
            }

            Zpl += "^A0N,32,30^FO35,424^FDHYUNDAI STEEL CO.[POHANG]^FS";
            Zpl += "^A0N,32,30^FO128,458^FDMADE IN KOREA^FS";
            Zpl += "^FO025,492^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,20^FO330,510^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  DB_EXPORT_HISTORY_SHIPPING_NEW(int Count)
        {


            var Zpl = "";
            var MM = "";

            Zpl = "";

            //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
            Zpl += "^XA";
            Zpl += "^PR 2";
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";


            // item DB , # 구분 로직적립후에 가능함.
            if (BARCODE_ITEM == "DB")
            {
                MM = " MM";
            }
            else if (BARCODE_ITEM == "#")
            {
                MM = "";
            }
            else
            {
                MM = "";
            }

            Zpl += "^FO001,002^GB445,0,8^FS";
            Zpl += "^FO001,060^GB445,0,2^FS";
            Zpl += "^FO001,112^GB445,0,2^FS";
            Zpl += "^FO001,168^GB452,0,8^FS";

            Zpl += "^FO001,002^GB0,168,8^FS";
            Zpl += "^FO132,002^GB0,168,3^FS";
            Zpl += "^FO445,002^GB0,168,8^FS";

            Zpl += "^A0N,30,32^FO037,022^FD" + "SPEC" + "^FS";
            Zpl += "^A0N,26,28^FO138,024^FD" + BARCODE_SPEC + "^FS";
            Zpl += "^A0N,30,32^FO041,075^FD" + "SIZE" + "^FS";
            if (BARCODE_Length2 == "")
            {
                Zpl += "^A0N,30,32^FO138,075^FD" + BARCODE_ITEM_SIZE + MM + "   " + BARCODE_Length + BARCODE_UOM + "^FS";
            }
            else
            {
                Zpl += "^A0N,30,32^FO138,075^FD" + BARCODE_ITEM_SIZE + MM + "   " + BARCODE_Length + "." + BARCODE_Length2 + BARCODE_UOM + "^FS";
            }
            Zpl += "^A0N,30,32^FO011,128^FD" + "HEAT NO" + "^FS";
            Zpl += "^A0N,34,36^FO138,127^FD" + BARCODE_HEAT + "           " + BARCODE_PRT_BUNDLE_NO.PadLeft(4, ' ') + "^FS";
            Zpl += "^FO020,181^BY2,3.0^BCN,060,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^A0N,20^FO333,200^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";
            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;

        }

        public string  RUST_TAG(int Count)
        {

            var zpl = "";
            var tITEM = "";
            zpl = "";
            zpl += "^XA";

            if (BARCODE_chkNEW_LABEL == "Y")
            {
                if (BARCODE_ITEM == "WX")
                {
                    tITEM = Mid(BARCODE_ITEM, 0, 1);
                }
                else
                {
                    tITEM = BARCODE_ITEM;
                }

                if (BARCODE_ITEM == "HB" || BARCODE_ITEM == "WX" ||
                    BARCODE_ITEM == "UB" || BARCODE_ITEM == "IP")
                {
                    // '--//왼쪽
                    zpl += "^FO015,015^A0N,38,47^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + "^FS";
                    zpl += "^FO110,058^A0N,38,47^CI13^FR^FD" + "# " + BARCODE_BUNDLE_NO + "^FS";
                    // '--//오른쪽
                    zpl += "^FO380,015^A0N,38,47^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + "^FS";
                    zpl += "^FO473,058^A0N,38,47^CI13^FR^FD" + "# " + BARCODE_BUNDLE_NO + "^FS";
                }
                else
                {
                    if (Mid(BARCODE_SIZE_NAME, 0, 3).Trim() == "Con")
                    {
                        //  '--//왼쪽
                        zpl += "^FO030,015^A0N,38,47^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + "^FS";
                        zpl += "^FO055,058^A0N,38,47^CI13^FR^FD" + BARCODE_BUNDLE_NO + "^FS";
                        // '--//오른쪽
                        zpl += "^FO395,015^A0N,38,47^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + "^FS";
                        zpl += "^FO421,058^A0N,38,47^CI13^FR^FD" + BARCODE_BUNDLE_NO + "^FS";
                    }
                    else if (Mid(BARCODE_SIZE_NAME, 0, 3).Trim() == "Non")
                    {
                        //  '--//왼쪽
                        zpl += "^FO030,035^A0N,40,50^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + " " + BARCODE_BUNDLE_NO + "^FS";
                        // '--//오른쪽
                        zpl += "^FO413,035^A0N,40,50^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + " " + BARCODE_BUNDLE_NO + "^FS";
                    }
                    else
                    {
                        // '--//왼쪽
                        zpl += "^FO030,038^A0N,38,42^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + "^FS";
                        //'--//오른쪽
                        zpl += "^FO395,038^A0N,38,42^CI13^FR^FD" + tITEM + BARCODE_SIZE_NAME + "^FS";
                    }
                }

            }
            else
            {
                if (BARCODE_SPEC == "RUST(CLASS D)" || BARCODE_SPEC == "RUST(CLASS E)")
                {
                    zpl += "^FO030,014^A0N,40,40^CI13^FR^FD" + BARCODE_SPEC + "^FS";
                    zpl += "^FO400,014^A0N,40,40^CI13^FR^FD" + BARCODE_SPEC + "^FS";
                }
                else
                {
                    //    	  zpl += "^FO150,014^A0N,48,58^CI13^FR^FD" + Mid(BARCODE_SPEC, 0, 11) + "^FS" ;
                    //          zpl += "^FO465,009^A0N,62,95^CI13^FR^FD" + Mid(BARCODE_SPEC, 11, 1) + "^FS" ;
                    //          zpl += "^FO517,014^A0N,48,58^CI13^FR^FD" + Mid(BARCODE_SPEC, 12, 1) + "^FS" ;

                    //    	  zpl += "^FO30,014^A0N,40,40^CI13^FR^FD" + Mid(BARCODE_SPEC, 0, 11) + "^FS" ;
                    //    	  //zpl += "^F330,014^A0N,40,40^CI13^FR^FD" + Mid(BARCODE_SPEC, 0, 11) + "^FS" ;
                    //          zpl += "^FO250,009^A0N,62,95^CI13^FR^FD" + Mid(BARCODE_SPEC, 11, 1) + "^FS" ;
                    //          zpl += "^FO300,014^A0N,48,58^CI13^FR^FD" + Mid(BARCODE_SPEC, 12, 1) + "^FS" ;

                    zpl += "^FO30,014^A0N,40,40^CI13^FR^FD" + Mid(BARCODE_SPEC, 0, 11) + "^FS";
                    zpl += "^FO300,014^A0N,40,40^CI13^FR^FD" + Mid(BARCODE_SPEC, 0, 11) + "^FS";
                }
            }


            if (Count == BARCODE_PRINT_COUNT)
            {
                zpl += "^MMC";
            }

            zpl += "^XZ";

            zpl += "^XA";
            zpl += "^MMT";
            zpl += "^XZ";

            return zpl;
        }
        
        public string  PB3_SOJAE(int Count)
        {

            var zpl = "";

            zpl = "";
            zpl += "^XA";
            zpl += "^^LH" + "010" + "," + "000" + "^FS";
            zpl += "^MMT";
            zpl += "^A0N,45^FO,034^FD" + "STEEL : " + BARCODE_STEEL + "^FS";
            zpl += "^A0N,45^FO,104^FD" + "HEAT  : " + BARCODE_HEAT + "^FS";
            zpl += "^A0N,45^FO,174^FD" + "Length  : " + BARCODE_Length + "^FS";
            zpl += "^A0N,40^FO,234^FD" + "MEMO1  : " + BARCODE_MEMO_CODE1.Replace("X6", "") + "^FS";
            zpl += "^A0N,40^FO,284^FD" + "MEMO2  : " + BARCODE_MEMO_CODE2.Replace("X6", "") + "^FS";

            if (Count == BARCODE_PRINT_COUNT && BARCODE_ARABIA == "")
            {
                zpl += "^MMC";
            }

            zpl += "^XZ";

            zpl += "^XA";
            zpl += "^MMC";
            zpl += "^XZ";

            return zpl;
        }

        public string  PB3_SOJAE_HONJAE(int Count)
        {

            var zpl = "";

            zpl = "";
            zpl += "^XA";
            zpl += "^^LH" + "000" + "," + "010" + "^FS";
            zpl += "^MMT";
            //설명1 : ^AON,가로크기, 세로크기^FO,X좌표,Y좌표^FD 출력할문자 ^FS
            //설명2 : ^AON,세로크기^FO,Y좌표^FD 출력할문자 ^FS  (가로크기랑 X좌표를 생략할 수 도 있음)
            //설명3 : ^A 다음에 오는 O는 폰트 종류이다.

            if (int.Parse(BARCODE_HONJAE) == 1)
            {
                zpl += "^A0N,470,850^FO,000,000^FD" + BARCODE_ARABIA + "^FS";
                zpl += "^A0N,355,850^FO,000,000^FD" + "_" + "^FS";
            }
            else if (int.Parse(BARCODE_HONJAE) == 2)
            {
                zpl += "^A0N,460,425^FO,000,000^FD" + BARCODE_ARABIA + "^FS";
                zpl += "^A0N,355,850^FO,000,850^FD" + "_" + "^FS";
            }

            if (Count == BARCODE_PRINT_COUNT)
            {
                zpl += "^MMC";
            }

            zpl += "^XZ";
            zpl += "^XA";
            zpl += "^MMC";
            zpl += "^XZ";

            return zpl;
        }
        
        public string  NUCLEAR_TAG_NEW(int Count)
        {
            var Zpl = "";

            //2015.06.03 구성욱 추가... 프린트 속도 2로 FIX
            Zpl += "^XA";
            Zpl += "^PR 2";
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";   //                         '기본좌표

            Zpl += "^A0N,60,60^FO130,45^FD" + BARCODE_BNPP + "^FS";  //'발전기 번호
            Zpl += "^A0N,30,30^FO130,130^FD" + BARCODE_MATERIAL + "^FS";  //'SPEC

            Zpl += "^A0N,40,40^FO130,180^FD" + BARCODE_SIZE_NAME + "^FS";  //'BAR NO(ITEM &ITEM_SIZE)
            Zpl += "^A0N,40,40^FO130,250^FD" + BARCODE_Length + " " + BARCODE_UOM + "^FS";   //       '길이/단위(Feet)
            Zpl += "^A0N,40,40^FO220,250^FD" + BARCODE_Length2 + " " + BARCODE_UOM2 + "^FS"; //       '길이/단위(Inch)
            Zpl += "^A0N,40,40^FO130,305^FD" + BARCODE_HEAT + "^FS";  //         'HEAT_NO
            Zpl += "^A0N,40,40^FO355,305^FD" + BARCODE_BONSU + "^FS"; //         'PCS(번들당본수)
            Zpl += "^A0N,40,40^FO130,370^FD" + BARCODE_WEIGHT + "^FS";        // '중량
            Zpl += "^A0N,40,40^FO355,370^FD" + BARCODE_PRT_BUNDLE_NO + "^FS";     // '번들 NO
            Zpl += "^FO020,425^BY2,3.0^BCN,050,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS"; //'바코드라인
            Zpl += "^A0N,20^FO317,450^CI13^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS"; //'바코드 텍스트
            Zpl += "^XZ";

            return Zpl;
        }

        public string  PB5_BUNDLE_HISTORY_TAG(int Count)
        {

            var Zpl = "";
            var Print_CNT = 0;
            /*
            var Barcode = "";
            var PoomMyoung = "";
            var ItemSize = "";
            var ItemSize = "";
            var i,ctn ;
            var STEEL_Length ;
            var PosX = "";
            var PosY = "";
            */

            if (BARCODE_PROGRAM == "10791" || BARCODE_PROGRAM == "10793")
            {
                Print_CNT = BARCODE_PRINT_COUNT;
            }

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^PW720";
            Zpl += "^LH" + "003" + "," + "003" + "^FS";


            if (BARCODE_ITEM == "AU" || BARCODE_ITEM == "AE")
            {
                Zpl += "^FO020,030^A0N,41,41^CI13^FR^FD" + "SIZE :  " + "^FS" + "^FS" + "^A0N,41,41^FO150,030^FD" + BARCODE_ITEM + "  " + BARCODE_SIZE_NAME + "^FS";

                if (BARCODE_STEEL == "A." || BARCODE_STEEL == "AH32." || BARCODE_STEEL == "AH36.")
                {
                    Zpl += "^FO020,083^A0N,41,41^CI13^FR^FD" + "GRADE :  " + "^FS" + "^FS" + "^A0N,41,41^FO190,083^FD" + "LR " + BARCODE_STEEL + "^FS";
                }
                else
                {
                    Zpl += "^FO020,083^A0N,41,41^CI13^FR^FD" + "GRADE :  " + "^FS" + "^FS" + "^A0N,41,41^FO190,083^FD" + BARCODE_STEEL + "^FS";
                }

                if (BARCODE_SHIP_NO == "Y")
                {
                    Zpl += "^FO380,275^A0N,45,45^CI13^FR^FD" + "Q526" + "^FS";
                }

                Zpl += "^FO020,136^A0N,41,41^CI13^FR^FD" + "Length :  " + "^FS" + "^FS" + "^A0N,41,41^FO205,136^FD" + BARCODE_Length + " " + BARCODE_UOM + "^FS";
                Zpl += "^FO020,189^A0N,41,41^CI13^FR^FD" + "INSPECTOR :  " + "^FS" + "^FS" + "^A0N,41,41^FO270,189^FD" + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^FO020,242^A0N,41,41^CI13^FR^FD" + "HYUNDAI STEEL" + "^FS";
                //2015.04.30 백상진 과장 요청으로 주석처리
                //          Zpl += "^FO020,320^A0N,41,41^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS" ;

                if (BARCODE_HEAT_ALL_PRT == "Y" && (BARCODE_ITEM == "AU" || BARCODE_ITEM == "AE"))
                {
                    Zpl += "^FO380,320^A0N,48,48^CI13^FR^FD" + BARCODE_HEAT + "^FS";
                }
                else
                {
                    Zpl += "^FO430,320^A0N,53,53^CI13^FR^FD" + BARCODE_HEAT.Substring(3, 3) + "^FS";
                }

            }
            else if (BARCODE_ITEM == "TS")
            {
                Zpl += "^FO020,030^A0N,41,41^CI13^FR^FD" + "SIZE :  " + "^FS" + "^FS" + "^A0N,41,41^FO150,030^FD" + BARCODE_ITEM + "  " + BARCODE_SIZE_NAME + "^FS";
                Zpl += "^FO020,078^A0N,41,41^CI13^FR^FD" + "GRADE :  " + "^FS" + "^FS" + "^A0N,41,41^FO170,078^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^FO020,126^A0N,41,41^CI13^FR^FD" + "Length :  " + "^FS" + "^FS" + "^A0N,41,41^FO205,126^FD" + BARCODE_Length + " " + BARCODE_UOM + " X " + BARCODE_PCS + " PCS" + "^FS";
                Zpl += "^FO020,174^A0N,41,41^CI13^FR^FD" + "INSPECTOR :  " + "^FS" + "^FS" + "^A0N,41,41^FO270,174^FD" + BARCODE_INSPECTER_NAME + "^FS";
                Zpl += "^FO020,270^A0N,41,41^CI13^FR^FD" + "HEAT.NO :  " + "^FS" + "^FS" + "^A0N,41,41^FO220,270^FD" + BARCODE_HEAT + "^FS";
                Zpl += "^FO020,222^A0N,41,41^CI13^FR^FD" + "HYUNDAI STEEL" + "^FS";
                //2015.04.30 백상진 과장 요청으로 주석처리
                //		 Zpl += "^FO020,340^A0N,41,41^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS" ;
            }


            if (BARCODE_PROGRAM == "10792")
            {
                Zpl += "^MMC";
            }
            else if (BARCODE_PROGRAM == "10791")
            {
                if (Count == Print_CNT)
                {
                    Zpl += "^MMC";
                }
            }
            else
            {
                if (Count == Print_CNT)
                {
                    Zpl += "^MMC";
                }
            }

            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

        }

        public string  UK_TAG(int Count)
        {

            var Zpl = "";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            Zpl += "^FO060,27^FR^XGR:UK_H11,1,1^FS";
            Zpl += "^FO220,12^FR^XGR:UK_C12,1,1^FS";
            Zpl += "^FO360,30^FR^XGR:UK_C22,1,1^FS";

            Zpl += "^FO215,008^GB0,163,3^FS";
            Zpl += "^FO453,008^GB0,163,3^FS";
            Zpl += "^FO215,008^GB241,0,3^FS";
            Zpl += "^FO215,168^GB241,0,3^FS";

            Zpl += "^FO298,012^FR^XGJIS_7_1,1,1^FS";

            Zpl += "^A0N,33,33^FO027,190^FD" + BARCODE_MEMO1 + "^FS";
            Zpl += "^A0N,33,33^FO027,225^FD" + BARCODE_MEMO2 + "^FS";
            Zpl += "^A0N,33,33^FO027,260^FD" + BARCODE_MEMO3 + "^FS";
            Zpl += "^A0N,33,33^FO027,295^FD" + BARCODE_MEMO4 + "^FS";

            Zpl += "^A0N,35,35^FO027,346^FD" + "SIZE        : " + BARCODE_ITEM_SIZE + " MM" + "^FS";
            Zpl += "^A0N,35,35^FO027,392^FD" + "Length   : " + BARCODE_Length + " MM" + "^FS";

            Zpl += "^A0N,35,35^FO350,392^FD" + BARCODE_PER_BONSU + "  pcs" + "^FS";
            Zpl += "^A0N,35,35^FO027,438^FD" + "SPEC       : " + BARCODE_SPEC + "^FS";

            if (BARCODE_HEAT_DSP == "0")
            {// 0 : HEAT표기, 1 : HEAT 미표기
                Zpl += "^A0N,35,35^FO027,484^FD" + "HEAT NO  : " + BARCODE_HEAT + "^FS";
            }

            if (BARCODE_WEIGHT_DSP == "0")
            {// 0 : HEAT표기, 1 : HEAT 미표기
                Zpl += "^A0N,35,35^FO350,484^FD" + BARCODE_WEIGHT + "kg" + "^FS";
            }

            Zpl += "^FO032,546^FD^XGR:HD,1,1^FS";
            Zpl += "^A0N,35,35^FO096,556^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
            Zpl += "^A0N,35,35^FO500,582^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

        }
        
        public string  STEEL_SIRIM_TAG(int Count)
        {

            var Zpl = "";

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "10" + "," + "10" + "^MMT";

            Zpl += "^FO060,12^FR^XGR:R6_SIRIM,1,1^FS";
            Zpl += "^FO220,12^FR^XGR:UK_C12,1,1^FS";
            Zpl += "^FO360,30^FR^XGR:UK_C22,1,1^FS";

            Zpl += "^FO215,008^GB0,163,3^FS";
            Zpl += "^FO453,008^GB0,163,3^FS";
            Zpl += "^FO215,008^GB241,0,3^FS";
            Zpl += "^FO215,168^GB241,0,3^FS";

            Zpl += "^FO298,012^FR^XGJIS_7_1,1,1^FS";

            //--SIRIM 관련 출력내용
            Zpl += "^A0N,20,20^FO027,171^FD" + "Certified MS 146:2006" + "^FS";
            Zpl += "^A0N,20,20^FO027,190^FD" + "Certification No : PC000739" + "^FS";

            Zpl += "^A0N,31,31^FO027,220^FD" + BARCODE_MEMO1 + "^FS";
            Zpl += "^A0N,31,31^FO027,248^FD" + BARCODE_MEMO2 + "^FS";
            Zpl += "^A0N,31,31^FO027,276^FD" + BARCODE_MEMO3 + "^FS";
            Zpl += "^A0N,31,31^FO027,304^FD" + BARCODE_MEMO4 + "^FS";

            if (BARCODE_SPEC2 == "")
            {
                Zpl += "^A0N,35,35^FO027,346^FD" + "SIZE        : " + BARCODE_ITEM_SIZE + " MM" + "^FS";
                Zpl += "^A0N,35,35^FO027,392^FD" + "Length   : " + BARCODE_Length + " M" + "^FS";
                Zpl += "^A0N,35,35^FO350,392^FD" + BARCODE_PER_BONSU + "  pcs" + "^FS";
                Zpl += "^A0N,35,35^FO027,438^FD" + "SPEC       : " + BARCODE_SPEC + "^FS";
                if (BARCODE_HEAT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,35,35^FO027,484^FD" + "HEAT NO  : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,35,35^FO350,484^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }
            }
            else
            {
                Zpl += "^A0N,33,33^FO027,346^FD" + "SIZE        : " + BARCODE_ITEM_SIZE + " MM" + "^FS";
                Zpl += "^A0N,33,33^FO027,385^FD" + "Length   : " + BARCODE_Length + " M" + "^FS";
                Zpl += "^A0N,33,33^FO350,385^FD" + BARCODE_PER_BONSU + "  pcs" + "^FS";
                Zpl += "^A0N,33,33^FO027,424^FD" + "SPEC       : " + BARCODE_SPEC + "^FS";
                Zpl += "^A0N,33,33^FO190,460^FD" + BARCODE_SPEC2 + "^FS";
                if (BARCODE_HEAT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,33,33^FO027,494^FD" + "HEAT NO  : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,33,33^FO350,494^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }
            }


            Zpl += "^FO032,546^FD^XGR:HD,1,1^FS";
            Zpl += "^A0N,35,35^FO096,556^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
            Zpl += "^A0N,35,35^FO500,582^FD" + BARCODE_BUNDLE_NO + "^FS";

            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

        }

        public string  NC_SIRIM_TAG(int Count)
        {

            var Zpl = "";
            var MM = "";

            if (BARCODE_SPEC2 == "")
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";

                Zpl += "^FO020,08^FR^XGR:R6_SIRIM,1,1^FS"; //MS 로그
                Zpl += "^FO170,12^FR^XGR:UK_C12,1,1^FS";    //CARES 로그
                Zpl += "^FO310,30^FR^XGR:UK_C22,1,1^FS";    //UKAS 로그

                Zpl += "^FO165,008^GB0,163,3^FS";  //좌
                Zpl += "^FO403,008^GB0,163,3^FS";  //우
                Zpl += "^FO165,008^GB241,0,3^FS";  //상
                Zpl += "^FO165,168^GB241,0,3^FS";  //하

                Zpl += "^A0N,18,18^FO001,171^FD" + "Certified MS 146:2006" + "^FS";
                Zpl += "^A0N,18,18^FO001,190^FD" + "Certification No : PC000739" + "^FS";

                Zpl += "^A0N,25,30^FO001,220^FD" + BARCODE_MEMO1 + "^FS";
                Zpl += "^A0N,25,30^FO001,250^FD" + BARCODE_MEMO2 + "^FS";
                Zpl += "^A0N,25,30^FO001,280^FD" + BARCODE_MEMO3 + "^FS";
                Zpl += "^A0N,25,30^FO001,310^FD" + BARCODE_MEMO4 + "^FS";

                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }

                Zpl += "^A0N,25,30^FO001,350^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";

                Zpl += "^A0N,25,30^FO001,390^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";
                Zpl += "^A0N,25,30^FO300,390^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";

                Zpl += "^A0N,25,30^FO001,430^FD" + "SPEC      : " + BARCODE_SPEC + "^FS";


                if (BARCODE_HEAT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,25,30^FO001,470^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,25,30^FO300,470^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }

                Zpl += "^FO000,500^FD^XGR:HD,1,1^FS";
                Zpl += "^A0N,33,33^FO060,510^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
                Zpl += "^A0N,25,25^FO400,540^FD" + BARCODE_BUNDLE_NO + "^FS";
            }
            else
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";

                Zpl += "^FO020,08^FR^XGR:R6_SIRIM,1,1^FS"; //MS 로그
                Zpl += "^FO170,12^FR^XGR:UK_C12,1,1^FS";    //CARES 로그
                Zpl += "^FO310,30^FR^XGR:UK_C22,1,1^FS";    //UKAS 로그

                Zpl += "^FO165,008^GB0,163,3^FS";  //좌
                Zpl += "^FO403,008^GB0,163,3^FS";  //우
                Zpl += "^FO165,008^GB241,0,3^FS";  //상
                Zpl += "^FO165,168^GB241,0,3^FS";  //하

                Zpl += "^A0N,18,18^FO001,171^FD" + "Certified MS 146:2006" + "^FS";
                Zpl += "^A0N,18,18^FO001,190^FD" + "Certification No : PC000739" + "^FS";

                Zpl += "^A0N,25,30^FO001,220^FD" + BARCODE_MEMO1 + "^FS";
                Zpl += "^A0N,25,30^FO001,250^FD" + BARCODE_MEMO2 + "^FS";
                Zpl += "^A0N,25,30^FO001,280^FD" + BARCODE_MEMO3 + "^FS";
                Zpl += "^A0N,25,30^FO001,310^FD" + BARCODE_MEMO4 + "^FS";

                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }

                Zpl += "^A0N,25,30^FO001,350^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";

                Zpl += "^A0N,25,30^FO001,380^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";
                Zpl += "^A0N,25,30^FO300,380^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";

                Zpl += "^A0N,25,30^FO001,410^FD" + "SPEC      : " + BARCODE_SPEC + "^FS";
                Zpl += "^A0N,25,30^FO001,440^FD" + "               " + BARCODE_SPEC2 + "^FS";

                if (BARCODE_HEAT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,25,30^FO001,470^FD" + "HEAT NO : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,25,30^FO300,470^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }

                Zpl += "^FO000,500^FD^XGR:HD,1,1^FS";
                Zpl += "^A0N,33,33^FO060,510^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
                Zpl += "^A0N,25,25^FO400,540^FD" + BARCODE_BUNDLE_NO + "^FS";
            }


            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  DB_UK_NC_SHIPPING(int Count)
        {

            var Zpl = "";
            var MM = "";

            if (BARCODE_SPEC2 == "")
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";

                Zpl += "^FO020,27^FR^XGR:UK_H11,1,1^FS";
                Zpl += "^FO180,12^FR^XGR:UK_C12,1,1^FS";
                Zpl += "^FO320,30^FR^XGR:UK_C22,1,1^FS";

                Zpl += "^FO175,008^GB0,163,3^FS";
                Zpl += "^FO413,008^GB0,163,3^FS";
                Zpl += "^FO175,008^GB241,0,3^FS";
                Zpl += "^FO175,168^GB241,0,3^FS";

                Zpl += "^A0N,31,31^FO001,175^FD" + BARCODE_MEMO1 + "^FS";
                Zpl += "^A0N,31,31^FO001,210^FD" + BARCODE_MEMO2 + "^FS";
                Zpl += "^A0N,31,31^FO001,245^FD" + BARCODE_MEMO3 + "^FS";
                Zpl += "^A0N,31,31^FO001,280^FD" + BARCODE_MEMO4 + "^FS";

                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }

                Zpl += "^A0N,31,31^FO001,330^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";
                Zpl += "^A0N,31,31^FO001,370^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";

                Zpl += "^A0N,31,31^FO300,370^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";
                Zpl += "^A0N,31,31^FO001,410^FD" + "SPEC      : " + BARCODE_SPEC + "^FS";

                if (BARCODE_HEAT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,31,31^FO001,450^FD" + "HEAT NO  : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,31,31^FO300,450^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }

                Zpl += "^FO000,495^FD^XGR:HD,1,1^FS";
                Zpl += "^A0N,33,33^FO060,505^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
                Zpl += "^A0N,31,31^FO400,535^FD" + BARCODE_BUNDLE_NO + "^FS";

            }
            else
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^LH" + "10" + "," + "10" + "^MMT";

                Zpl += "^A0N,50,50^FO040,035^FD" + "DEFORMED BAR " + "^FS"; ;

                Zpl += "^A0N,31,31^FO001,120^FD" + BARCODE_MEMO1 + "^FS";
                Zpl += "^A0N,31,31^FO001,155^FD" + BARCODE_MEMO2 + "^FS";
                Zpl += "^A0N,31,31^FO001,190^FD" + BARCODE_MEMO3 + "^FS";
                Zpl += "^A0N,31,31^FO001,225^FD" + BARCODE_MEMO4 + "^FS";

                if (BARCODE_ITEM == "DB")
                {
                    MM = " MM";
                }
                else if (BARCODE_ITEM == "#")
                {
                    MM = "";
                }

                Zpl += "^A0N,31,31^FO001,281^FD" + "SIZE       : " + BARCODE_ITEM_SIZE + MM + "^FS";
                Zpl += "^A0N,31,31^FO001,317^FD" + "Length  : " + BARCODE_Length + " " + BARCODE_UOM + "^FS";

                Zpl += "^A0N,31,31^FO300,317^FD" + BARCODE_PER_BONSU + " pcs" + "^FS";
                Zpl += "^A0N,31,31^FO001,353^FD" + "SPEC      : " + BARCODE_SPEC + "^FS";
                Zpl += "^A0N,31,31^FO001,389^FD" + "                 " + BARCODE_SPEC2 + "^FS";

                if (BARCODE_HEAT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,31,31^FO001,425^FD" + "HEAT NO  : " + BARCODE_HEAT + "^FS";
                }

                if (BARCODE_WEIGHT_DSP == "0")
                {// 0 : HEAT표기, 1 : HEAT 미표기
                    Zpl += "^A0N,31,31^FO300,425^FD" + BARCODE_WEIGHT + "kg" + "^FS";
                }

                Zpl += "^FO000,481^FD^XGR:HD,1,1^FS";
                Zpl += "^A0N,33,33^FO060,491^FD" + "HYUNDAI STEEL COMPANY" + "^FS";
                Zpl += "^A0N,31,31^FO400,529^FD" + BARCODE_BUNDLE_NO + "^FS";
            }


            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

        }
        
        public string  SP_TAG(int Count)
        {

            var Zpl = "";
            var Barcode = "";
            var PoomMyoung = "";

            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;

            if (BARCODE_MARKET_TYPE == "11")
            {
                Zpl = "";
                Zpl += "^XA";
                Zpl += "^POI^FS";
                Zpl += "^PW720";
                Zpl += "^LH0,0^FS";
                Zpl += "^BY3,3^FO090,348^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";
                Zpl += "^FO210,320^A0N,19,36^CI13^FR^FD" + Barcode + "^FS";
                Zpl += "^FO195,600^A0N,19,36^CI13^FR^FD" + Barcode + "^FS";
                Zpl += "^FO40,630^A0N,40,37^CI13^FR^FD" + Barcode.Substring(1, 6) + "-" + Barcode.Substring(7, 4) + "^FS";
                Zpl += "^FO340,630^A0N,40,37^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";
                Zpl += "^BY2,2.0^FO600,350^BCB,90,N,Y,N^FR^FD>:" + Barcode + "^FS";
                Zpl += "^FO580,420^A0B,23,36^CI13^FR^FD" + Barcode + "^FS";

                Zpl += "^FO630,200^A0N,53,43^CI13^FR^FD" + BARCODE_SP_CNT + "^FS";

                if (BARCODE_COUNTRY_BASIS == "NO" || BARCODE_COUNTRY_BASIS == "KS")
                {
                    Zpl += "^FO588,35^FD^XGKI007,1,1^FS";
                }
                else if (BARCODE_COUNTRY_BASIS == "JIS")
                {
                    Zpl += "^FO548,45^FD^XGKI006,1,1^FS";
                }

                Zpl += "^FO040,303^A0N,35,35^CI13^FR^FD" + BARCODE_APPORVAL_NO + "^FS";
                Zpl += "^FO420,303^A0N,38,36^CI13^FR^FD" + BARCODE_INSPECTER_NAME + "^FS";

                if (BARCODE_ITEM == "HB")
                {
                    PoomMyoung = "H";
                }
                else if (BARCODE_ITEM == "TP")
                {
                    PoomMyoung = "TP";
                }
                else if (BARCODE_ITEM == "AI")
                {
                    PoomMyoung = "L";
                }
                else if (BARCODE_ITEM == "AE")
                {
                    PoomMyoung = "L";
                }
                else if (BARCODE_ITEM == "IB")
                {
                    PoomMyoung = "I";
                }
                else
                {
                    PoomMyoung = "";
                }



                if (PoomMyoung == "H")
                {
                    Zpl += "^FO280,30^A0N,55,100^CI13^FP^FD" + PoomMyoung + "^FS";
                }
                else if (PoomMyoung == "I")
                {
                    Zpl += "^FO280,30^ADN,55,70^CI13^FP^FD" + PoomMyoung + "^FS";
                }
                else if (PoomMyoung == "TP")
                {
                    Zpl += "^FO270,30^A0N,55,70^CI13^FP^FD" + PoomMyoung + "^FS";
                }
                else if (PoomMyoung == "L")
                {
                    Zpl += "^FO300,22^A0I,50,100^CI13^FP^FD" + PoomMyoung + "^FS";
                }

                Zpl += "^FO212,85^A0N,45,40^CI13^FR^FD" + BARCODE_SIZE_CODE + "^FS";

                if (BARCODE_STEEL.Length >= 1 || BARCODE_STEEL.Length <= 11)
                {
                    Zpl += "^FO430,80^A0N,57,41^CI13^FR^FD" + BARCODE_STEEL + "^FS";
                }
                else if (BARCODE_STEEL.Length >= 12 || BARCODE_STEEL.Length <= 15)
                {
                    Zpl += "^FO430,80^A0N,57,31^CI13^FR^FD" + BARCODE_STEEL + "^FS";
                }
                else if (BARCODE_STEEL.Length >= 16 || BARCODE_STEEL.Length <= 21)
                {
                    Zpl += "^FO410,80^A0N,57,25^CI13^FR^FD" + BARCODE_STEEL + "^FS";
                }

                Zpl += "^FO212,135^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";
                Zpl += "^FO212,183^A0N,43,43^CI13^FR^FD" + BARCODE_HEAT + "^FS";
                //Zpl += "^FO490,145^A0N,75,65^CI13^FR^FD" + bar_format(BARCODE_Length, 1) + BARCODE_UOM + "^FS"; //          '제품길이
                Zpl += "^FO490,145^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS"; //          '제품길이

                if (Count == BARCODE_PRINT_COUNT)
                {
                    Zpl += "^MMC";
                }


            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

            Zpl += "^PQ1,0,0,N";
            Zpl += "^XZ";
        }

        public string  NEW_SIDE_TAG(int Count)
        {

            var Zpl = "";
            var v_factory = "";
            var output_Length = 0;

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "003" + "," + "000" + "^FS";
            Zpl += "^MMT";
            Zpl += "^FO001,009^FD^XGR:HD,1,1^FS";
            v_factory = BARCODE_FACTORY;
            output_Length = BARCODE_SIZE_NAME.Length;
            if (BARCODE_STEEL.Length > 4)
            {
                Zpl += "^A0N150,27^FO060,012^FD" + BARCODE_SIZE_NAME + "X" + BARCODE_Length + "M-" + "^FS" + "^A0N130,32^FO365,013^FD" + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^A0N150,30^FO060,012^FD" + BARCODE_SIZE_NAME + "X" + BARCODE_Length + "M-" + "^FS" + "^A0N130,37^FO390,013^FD" + BARCODE_STEEL + "^FS";
            }

            if (BARCODE_HEAT_ALL_PRT == "Y")
            {
                Zpl += "^A0N130,25^FO090,043^FD" + BARCODE_PO_NO + " - " + BARCODE_BUNDLE_NO + "   " + BARCODE_HEAT + "^FS";
            }
            else
            {
                Zpl += "^A0N130,25^FO090,043^FD" + BARCODE_PO_NO + " - " + BARCODE_BUNDLE_NO + "^FS";
            }

            Zpl += "^FO500,006^BY2,3.0^BCN,050,N,Y,N^FR^FD" + BARCODE_PO_NO + BARCODE_BUNDLE_NO + "^FS";

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }


            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";


            return Zpl;

        }

        public string  RB_SOM(int Count)
        {
            var Zpl = "";

            //BARCODE_Length = comUtil_roundXL(BARCODE_Length, 2);
            Zpl = "";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^A0N080,40^FO110,050^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^A0N080,40^FO470,050^FD" + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^A0N130,60^FO055,100^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N130,60^FO415,100^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^A0N100,55^FO105,165^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N100,55^FO465,165^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^A0N080,40^FO130,215^FD" + BARCODE_Length + "^FS";
            Zpl += "^A0N080,40^FO490,215^FD" + BARCODE_Length + "^FS";

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }

        public string  SP_SIDE_TAG(int Count)
        {
            var Zpl = "";
            var Barcode = "";

            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;

            Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "003" + "," + "000" + "^FS";
            Zpl += "^MMT";
            Zpl += "^BY4,3^FO65,003^BCN,65,N,Y,N^FR^FD>:" + Barcode + "^FS";
            Zpl += "^A0N90,30^FO062,074^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N100,30^FO367,074^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^FO480,074^A0N,30,34^CI13^FR^FD" + Barcode + "^FS";

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
                BARCODE_CUTTING_LOCAL_COUNT = 0;
                BARCODE_CUTTING_COUNT = 0;
            }

            Zpl += "^XZ";
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;
        }
        
        public string  LOCATION_TAG(int Count)
        {
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^POI^FS";
            Zpl += "^PW720";
            Zpl += "^LH0,0^FS";

            if (BARCODE_TAG_KIND == "L")
            {
                Zpl += "^BY11.0,11.0^FO005,120^BCB,500,N,Y,N^FR^FD>:" + BARCODE_YARD + BARCODE_LOCATION + BARCODE_NUM + "^FS";
                Zpl += "^FO550,385^A0B,200,300^CI13^FR^FD" + BARCODE_LOCATION + BARCODE_NUM + "^FS";
            }
            else if (BARCODE_TAG_KIND == "S")
            {
                Zpl += "^BY6.5,6.5^FO050,040^BCN,250,N,Y,N^FR^FD>:" + BARCODE_YARD + BARCODE_LOCATION + BARCODE_NUM + "^FS";
                Zpl += "^FO255,320^A0N,095,150^CI13^FR^FD" + BARCODE_LOCATION + BARCODE_NUM + "^FS";

            }
            else
            {
                Zpl += "^BY6.5,6.5^FO030,040^BCN,250,N,Y,N^FR^FD>:" + BARCODE_YARD + BARCODE_LOCATION + BARCODE_NUM + "^FS";
                Zpl += "^FO255,320^A0N,095,150^CI13^FR^FD" + BARCODE_LOCATION + BARCODE_NUM + "^FS";
            }

            /*
                //--공사작업자 시스템 개선 관련 테스트 구문 :  2013.11.25, SHARP--//
                var BarCode_Sample = "P131125001";
                var Worker_name = "기봉춘"; //--"Chloe Moretz";
                alert("문자길이(기봉춘) --> " + Worker_name.Length );
                Zpl += "^SEE:UHANGUL.DAT^FS";
                Zpl += "^CW1,E:H2GPRM.FNT^FS";
                Zpl += "^BY2.0,2.0^FO065,005^BCN,080,N,Y,N^FR^FD>:" + BarCode_Sample + "^FS" ; //--+ BARCODE_YARD + BARCODE_LOCATION + BARCODE_NUM + "^FS" ;
                Zpl += "^FO065,093^A1N,023,023^CI26^FD" + BarCode_Sample + "^FS" ; //--BARCODE_LOCATION + BARCODE_NUM + "^FS"  ;
                if(Worker_name.Length == 3){
                    Zpl += "^FO275,090^A1N,028,028^CI26^FD" +  Worker_name + "^FS" ; //--BARCODE_LOCATION + BARCODE_NUM + "^FS"  ;
                }else if(Worker_name.Length == 4){
                    Zpl += "^FO248,090^A1N,028,028^CI26^FD" +  Worker_name + "^FS" ; //--BARCODE_LOCATION + BARCODE_NUM + "^FS"  ;
                }else if(Worker_name.Length > 4){
                    Zpl += "^FO238,093^A1N,022,022^CI26^FD" +  Worker_name + "^FS" ; //--BARCODE_LOCATION + BARCODE_NUM + "^FS"  ;
                }
                //--Zpl += "^FO440,780^A0N,025,025^CI13^FR^FD" + "2013.08.31 - 2013.09.30" + "^FS" ; //--BARCODE_LOCATION + BARCODE_NUM + "^FS"
            */

            if (Count == BARCODE_PRINT_COUNT)
            {
                Zpl += "^MMC";
            }
            Zpl += "^XZ";

            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

        }

        public string  IA_BUNDLE_TAG(int Count)
        {

            var Barcode = "";
            var i = 0;
            var Print_CNT = 0;
            var Zpl = "";

            Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            Print_CNT = int.Parse(BARCODE_SP_CNT);

            Zpl += "^XA";
            //Zpl += "^MMC";
            Zpl += "^POI^FS";
            Zpl += "^PW830";
            Zpl += "^LH0,0^FS";
            Zpl += "^LH" + "003" + "," + "003" + "^FS";
            Zpl += "^FO020,020^A0N,37,37^CI13^FR^FD" + "SIZE  " + "^FS" + "^AON,37,37^FO220,20^FD" + ": " + "^FS" + "^A0N,37,37^FO270,020^FD" + BARCODE_ITEM + "  " + BARCODE_SIZE_NAME + "^FS";

            if (BARCODE_SHIP_NO == "Y")
            {
                Zpl += "^FO020,058^A0N,37,37^CI13^FR^FD" + "GRADE  " + "^FS" + "^AON,37,37^FO220,58^FD" + ": " + "^FS" + "^A0N,37,37^FO270,058^FD" + "LR " + BARCODE_STEEL + "^FS";
            }
            else
            {
                Zpl += "^FO020,058^A0N,37,37^CI13^FR^FD" + "GRADE  " + "^FS" + "^AON,37,37^FO220,58^FD" + ": " + "^FS" + "^A0N,37,37^FO270,058^FD" + BARCODE_STEEL + "^FS";
            }


            Zpl += "^FO020,096^A0N,37,37^CI13^FR^FD" + "Length  " + "^FS" + "^AON,37,37^FO220,96^FD" + ": " + "^FS" + "^A0N,37,37^FO270,096^FD" + BARCODE_Length + BARCODE_UOM + "^FS";
            Zpl += "^FO020,134^A0N,37,37^CI13^FR^FD" + "INSPECTOR  " + "^FS" + "^AON,37,37^FO220,134^FD" + ": " + "^FS" + "^A0N,37,37^FO270,134^FD" + BARCODE_INSPECTER_NAME + "^FS";
            Zpl += "^FO020,172^A0N,37,37^CI13^FR^FD" + "HYUNDAI STEEL" + "^FS";
            //2015.04.23 품보 백상진 과장 요청_검사일자 및 생산일자는 태그에 출력하지 않는다. 영업 요청 사항
            //	Zpl += "^FO020,210^A0N,37,37^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS" ;
            Zpl += "^BY4,4^FO020,270^BCN,130,N,Y,N^FR^FD>:" + Barcode + "^FS";
            //Zpl += "^FO270,230^A0N,35,35^CI13^FR^FD" + Mid(Barcode, 0, 6) + "-" + Mid(Barcode, 6, 4) + "^FS";
            Zpl += "^FO270,230^A0N,35,35^CI13^FR^FD" + Barcode.Substring(0, 6) + "-" + Barcode.Substring(6, 4) + "^FS";
            Zpl += "^BY2,2.0^FO750,050^BCB,70,N,Y,N^FR^FD>:" + Barcode + "^FS";
            Zpl += "^FO720,100^A0B,23,36^CI13^FR^FD" + Barcode + "^FS";

            // AI 태그 KS 인증 문구 추가 (원정호. 20161031)
            if (BARCODE_STEEL == "SS400" || BARCODE_STEEL == "SS490")
            {
                Zpl += "^FO520,200^A0N,20,20^CI13^FR^FD" + "KS D 3503" + "^FS";
                Zpl += "^FO520,220^A0N,20,20^CI13^FR^FD" + "Rolled steels for" + "^FS";
                Zpl += "^FO520,240^A0N,20,20^CI13^FR^FD" + "General structure" + "^FS";
            }
            else if (BARCODE_STEEL == "SM400A" || BARCODE_STEEL == "SM400B" || BARCODE_STEEL == "SM490A" || BARCODE_STEEL == "SM490B" || BARCODE_STEEL == "SM490YA" || BARCODE_STEEL == "SM490YB")
            {
                Zpl += "^FO520,200^A0N,20,20^CI13^FR^FD" + "KS D 3515" + "^FS";
                Zpl += "^FO520,220^A0N,20,20^CI13^FR^FD" + "Rolled steels for" + "^FS";
                Zpl += "^FO520,240^A0N,20,20^CI13^FR^FD" + "Welded structures" + "^FS";
            }

            // 생산일자 출력 추가 (원정호. 20161031)
            Zpl += "^FO020,230^A0N,35,35^CI13^FR^FD" + BARCODE_PRODUCT_DATE + "^FS";

            if (BARCODE_SHIP_NO == "Y")
            {//호선번호 출력부분(Q526) : 2012.07.05, SHARP
                Zpl += "^FO615,290^A0N,36,36^CI13^FR^FD" + "Q526" + "^FS";
            }

            if (BARCODE_HEAT_ALL_PRT == "Y")
            {
                Zpl += "^FO615,330^A0N,40,40^CI13^FR^FD" + BARCODE_HEAT + "^FS";
            }
            else
            {
                //Zpl += "^FO630,300^A0N,53,53^CI13^FR^FD" + Mid(BARCODE_HEAT, 3, 3) + "^FS";
                Zpl += "^FO630,300^A0N,53,53^CI13^FR^FD" + BARCODE_HEAT.Substring(3, 3) + "^FS";
            }


            for (i = 0; i < Print_CNT; i++)
            {
                Barcode = Barcode.Substring(0, 9) + i;

                if (i == 0)
                {
                    Zpl += "^FO001,417^FD^XGR:HD,1,1^FS";
                    //--강종명 자리수 증가(9자리이상)에 따른 로직반영 - 2014.02.25, SHARP, 김기원요청(GQ - LT-FH32MC  등)
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO330,417^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO330,417^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,417^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,417^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,417^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,417^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,417^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            //Zpl += "^A0N130,25^FO090,448^FD" + Mid(Barcode, 0, 6) + " - " + Mid(Barcode, 6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                            Zpl += "^A0N130,25^FO090,448^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            //Zpl += "^A0N130,25^FO090,448^FD" + Mid(Barcode, 0, 6) + " - " + Mid(Barcode, 6, 4) + "   " + BARCODE_HEAT + "^FS";
                            Zpl += "^A0N130,25^FO090,448^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,448^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,417^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 1)
                {
                    Zpl += "^FO001,480^FD^XGR:HD,1,1^FS";
                    //IA 신강종(SM490YB) 발생에 따라 사이드라벨에 출력되는 "규격명+강종"의 글자크기를 변경해야 한다.(하경태요구) 2009.05.15,SHARP
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,480^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,480^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,480^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,480^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,480^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,480^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,480^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,480^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,480^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,480^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,480^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,480^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            //Zpl += "^A0N130,25^FO090,511^FD" + Mid(Barcode, 0, 6) + " - " + Mid(Barcode, 6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                            Zpl += "^A0N130,25^FO090,511^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,511^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        //Zpl += "^A0N130,25^FO090,511^FD" + Mid(Barcode, 0, 6) + " - " + Mid(Barcode, 6, 4) + "^FS";
                        Zpl += "^A0N130,25^FO090,511^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,480^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 2)
                {
                    Zpl += "^FO001,545^FD^XGR:HD,1,1^FS";
                    //IA 신강종(SM490YB) 발생에 따라 사이드라벨에 출력되는 "규격명+강종"의 글자크기를 변경해야 한다.(하경태요구) 2009.05.15,SHARP
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,545^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,545^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,545^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,545^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,545^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,545^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,545^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,545^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,545^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,545^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,545^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,545^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,576^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,576^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,576^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,545^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 3)
                {
                    Zpl += "^FO001,610^FD^XGR:HD,1,1^FS";
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,610^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,610^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,610^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,610^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,610^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,610^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,610^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,610^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,610^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,610^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,610^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,610^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,645^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,645^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,645^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,610^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 4)
                {
                    Zpl += "^FO001,675^FD^XGR:HD,1,1^FS";
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,675^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,675^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,675^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,675^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,675^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,675^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,675^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,675^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,675^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,675^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,675^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,37^FO390,675^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,706^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,706^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,706^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,675^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 5)
                {
                    Zpl += "^FO001,740^FD^XGR:HD,1,1^FS";
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,740^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,740^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,740^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,740^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,740^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,33^FO355,740^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,740^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,740^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,740^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,740^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,740^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,740^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,771^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,771^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,771^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,740^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 6)
                {
                    Zpl += "^FO001,805^FD^XGR:HD,1,1^FS";
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,805^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,805^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,805^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,805^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,805^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,805^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,805^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,805^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,805^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,805^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,805^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,805^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,836^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,836^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,836^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,805^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 7)
                {
                    Zpl += "^FO001,870^FD^XGR:HD,1,1^FS";
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,870^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,870^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,870^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,870^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,870^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,870^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,870^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,870^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,870^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,870^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,870^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,870^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,901^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,901^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,901^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,870^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 8)
                {
                    Zpl += "^FO001,935^FD^XGR:HD,1,1^FS";
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,935^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,935^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,935^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,935^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,935^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,935^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,935^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,935^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,935^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,935^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,935^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,935^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,966^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,966^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,966^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,935^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }
                else if (i == 9)
                {
                    Zpl += "^FO001,995^FD^XGR:HD,1,1^FS";
                    if (BARCODE_STEEL.Length > 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,995^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO335,995^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,995^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,25^FO335,995^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else if (BARCODE_STEEL.Length >= 5 && BARCODE_STEEL.Length <= 8)
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,27^FO060,995^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N110,30^FO355,995^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,27^FO060,995^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N120,30^FO355,995^FD" + BARCODE_STEEL + "^FS";
                        }
                    }
                    else
                    {
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N150,30^FO060,995^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,33^FO390,995^FD" + "LR " + BARCODE_STEEL + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N150,30^FO060,995^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N130,35^FO390,995^FD" + BARCODE_STEEL + "^FS";
                        }
                    }

                    if (BARCODE_HEAT_ALL_PRT == "Y")
                    {//S355J2, S355J204 강종일때 HEAT 6자리 출력 : 2
                        if (BARCODE_SHIP_NO == "Y")
                        {
                            Zpl += "^A0N130,25^FO090,1026^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "  Q526" + "^FS";
                        }
                        else
                        {
                            Zpl += "^A0N130,25^FO090,1026^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "   " + BARCODE_HEAT + "^FS";
                        }
                    }
                    else
                    {
                        Zpl += "^A0N130,25^FO090,1026^FD" + Barcode.Substring(0, 6) + " - " + Barcode.Substring(6, 4) + "^FS";
                    }

                    Zpl += "^FO500,995^BY2,3.0^BCN,050,N,Y,N^FR^FD" + Barcode + "^FS";
                }


            }

            //if (Count == BARCODE_PRINT_COUNT - 1)
            //{
            if (Count == BARCODE_PRINT_COUNT)
                {
                    Zpl += "^MMC";
            }


            Zpl += "^XZ";

            Zpl += "^XA";
            //Zpl += "^MMC";
            Zpl += "^MMT";
            Zpl += "^XZ";

            return Zpl;

        }

        public string  CS_TAG1(int Count)
        {
            var Zpl = ""; //

            Zpl += "^XA"; //
            Zpl += "^MMT"; //

            //	Zpl += "^AON090,40^FO020,230^FD" + BARCODE_SPEC + Space(5) + "^FS";
            //	Zpl += "^AON090,40^FO410,230^FD" + BARCODE_SPEC + Space(5) + "^FS";

            Zpl += "^AF 40,20^FO060,230^FD" + BARCODE_SPEC + Space(5) + "^FS";
            Zpl += "^AF 40,20^FO450,230^FD" + BARCODE_SPEC + Space(5) + "^FS";

            if (Count == BARCODE_PRINT_COUNT - 1)
            {
                Zpl += "^MMC";
            }

            Zpl += "^XZ";
            return Zpl;
        }
          
        public string RB_HISTORY_TAG_NEW(int Count)
        {
            /*
           * 원형강 이력라벨 발행하는 루틴
           * 출력형태- RB 이력라벨 발행 형태(원형강 Tracking 구축관련)
           */
            //--var printCnt = BARCODE_PRINT_COUNT;
            var printCnt = 0;

            //번들당 4매 발행요청, 위로직 주석(나영식기사) : 2013.02.13, SHARP
            if (BARCODE_PROGRAM == "28161" || BARCODE_PROGRAM == "28159")
            {
                printCnt = BARCODE_PRINT_COUNT;
            }
            else
            {
                printCnt = BARCODE_CUTTING_COUNT;
            }
            //var printCnt = BARCODE_CUTTING_COUNT;

            var barCode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^LH" + "003" + "," + "003" + "^FS";

            Zpl += "^FO020,030^A0N,36,36^CI13^FR^FD" + "SIZE :  " + "^FS" + "^FS" + "^A0N,36,36^FO150,030^FD" + BARCODE_ITEM + "  " + BARCODE_SIZE_NAME + "^FS";
            Zpl += "^FO020,070^A0N,36,36^CI13^FR^FD" + "GRADE :  " + "^FS" + "^FS" + "^A0N,41,41^FO170,070^FD" + BARCODE_STEEL + "^FS";
            Zpl += "^FO020,110^A0N,36,36^CI13^FR^FD" + "LENGTH :  " + "^FS" + "^FS" + "^A0N,36,36^FO205,110^FD" + BARCODE_Length + " " + BARCODE_UOM + "^FS";
            Zpl += "^FO020,150^A0N,36,36^CI13^FR^FD" + "HEAT.NO :  " + "^FS" + "^FS" + "^A0N,36,36^FO220,150^FD" + BARCODE_HEAT + "^FS";
            Zpl += "^FO020,190^A0N,36,36^CI13^FR^FD" + "INSPECTOR :  " + "^FS" + "^FS" + "^A0N,36,36^FO270,190^FD" + BARCODE_INSPECTER_NAME + "^FS";
            Zpl += "^FO020,230^A0N,36,36^CI13^FR^FD" + "HYUNDAI STEEL" + "^FS";

            Zpl += "^FO020,270^A0N,36,36^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";
            // Zpl += "^FO353,270^A0N,38,38^CI13^FR^FD" + BARCODE_BUNDLE_NO + "^FS";//BUNDLE_NO
            Zpl += "^FO353,270^A0N,38,38^CI13^FR^FD" + BARCODE_PRT_BUNDLE_NO + "^FS";//BUNDLE_NO

            Zpl += "^FO350,030^A0N,43,43^CI13^FR^FD" + BARCODE_FACTORY + "^FS";//공장
            Zpl += "^FO350,110^A0N,43,43^CI13^FR^FD" + BARCODE_RB_USE_GUBUN + "^FS";//RB용도구분

            Zpl += "^BY3,3^FO030,305^BCN,070,N,Y,N^FR^FD>:" + barCode + "^FS";//가로 바코드
            Zpl += "^FO160,378^A0N,30,30^CI13^FR^FD" + barCode.Substring(0, 6) + "-" + barCode.Substring(7, 4) + "^FS";//PO+BD

            Zpl += "^BY2,2.0^FO477,030^BCB,080,N,Y,N^FR^FD>:" + barCode + "^FS";//세로 바코드
            Zpl += "^FO565,100^A0B,23,36^CI13^FR^FD" + barCode + "^FS";//세로 바코드 텍스트-1

            //번들당 4매 발행요청, 위로직 주석(나영식기사) : 2013.02.13, SHARP
            if (BARCODE_PROGRAM == "28161" || BARCODE_PROGRAM == "28159")
            {
                if (Count == printCnt) Zpl += "^MMC";//절단
            }
            else
            {
                if (printCnt == 1) Zpl += "^MMC";//절단("1" = 커팅의미임.)
            }

            Zpl += "^XZ";

            //print Mode를 CUTER->TEAR OFF로 전환
            Zpl += "^XA";
            Zpl += "^MMT";
            Zpl += "^XZ";
            return Zpl;
        }

        public string PB4_EXPORT_LABEL_TP(int Count)
        {
            //대형 수출용 TAG 수동발행

            var printCnt = 0;
            var Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            var AR_N = "";
            var PoomMyoung = "";

            printCnt = BARCODE_PRINT_COUNT;

            //HeatTreatment = "0" : 미열처리 - 강종 + 'AR'
            var Zpl = "";
            Zpl += "^XA";
            Zpl += "^POI^FS";
            Zpl += "^PW712";             //742
            Zpl += "^LH0,0^FS";          //기본좌표
            //2008.08.05, SHARP 수정
            // 1. CE Mark 용 태그 도입에 따른 바코드 출력크기 조정(작게) 및 전체적으로 출력위치 조정
            // 2. 대형제품 재고관리 시스템 도입관련 : 세로바코드 추가출력
            // 원로직 주석처리 → Zpl = Zpl & "^BY4,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" & Barcode & "^FS"     '바코드
            Zpl += "^BY2.0,1.5^FO280,360^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";
            // 세로 바코드 출력(BCB)
            Zpl += "^BY2.0,1.5^FO600,364^BCB,080,N,Y,N^FR^FD>:" + Barcode + "^FS";

            Zpl += "^FO300,620^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";
            Zpl += "^FO300,320^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";

            //QA 요청으로 출력하지 않기로 함. : 임낙광부장요청 : 2009.09.29
            //Zpl = Zpl & "^FO652,212^A0N,53,43^CI13^FR^FD1^FS"             //piece(1고정)

            //Select Case BARCODE_MARK     'mark종류에 따른 마크 출력
            //    Case "0"
            //    Case "1" : Zpl = Zpl & "^FO588,35^FR^XGKI007,1,1^FS"
            //    Case "2" : Zpl = Zpl & "^FO538,55^FR^XGKI006,1,1^FS"
            //End Select

            Zpl += "^FO590,314^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";
            Zpl += "^FO063,314^A0N,45,40^CI13^FR^FD" + BARCODE_APPORVAL_NO + "^FS";       //허가번호
            Zpl += "^FO220,670^A0N,38,38^CI13^FR^FD" + Barcode.Substring(0, 6) + "-" + Barcode.Substring(6, 4) + "^FS";     //PO+BD
            Zpl += "^FO506,670^A0N,38,38^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";               //발행일자
            //2008.08.05 SHARP 수정 -----------------------------------------------------------------
            //Zpl = Zpl & "^FO38,37^A0N,48,37^CI13^FR^FDARTICLE^FS"              '고정텍스트(ARTICLE)
            //태그에 규격번호가 인쇄되어 있기 때문에 출력하지 않아도 됨. 2009.01.06, SHARP
            //Zpl = Zpl & "^FO49,87^A0N,35,35^CI13^FR^FD" & BARCODE_SPEC & "^FS"         '규격 Num
            //-------------------------------------------------------------------------------------------

            //강종명 자리수에 따른 발행위치 계산
            if(BARCODE_HEAT_DSP == "0")
            {
                AR_N = "+AR";
            }
            else if (BARCODE_HEAT_DSP == "1")
            {
                AR_N = "+N";
            }

            if (BARCODE_STEEL.Substring(BARCODE_STEEL.Length - 2, 2) == "04")
            {
                BARCODE_STEEL = BARCODE_STEEL.Substring(0, BARCODE_STEEL.Length - 2);
            }

            if(BARCODE_STEEL.Length > 0 && BARCODE_STEEL.Length < 12)
            {
                Zpl += "^FO370,85^A0N,52,52^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";     //강종명
            }
            else if (BARCODE_STEEL.Length > 11 && BARCODE_STEEL.Length < 16)
            {
                Zpl += "^FO395,85^A0N,50,35^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";     //강종명
            }
            else if (BARCODE_STEEL.Length > 15 && BARCODE_STEEL.Length < 22)
            {
                Zpl += "^FO385,85^A0N,50,29^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";     //강종명
            }

            /*
            '철강일원화 관련 수정 (20121128김영조) : 품목 코드 변경
            'HB UB WB WX → HB , SP → TP , IA → AI , EA → AE , IB → IB
            '            Select Case Me.BARCODE_ITEM.Trim
            '                Case "HB", "UB", "WB", "WX" : PoomMyoung = "H-BEAM"
            '                Case "SP" : PoomMyoung = "SHEET PILE"
            '            Zpl &= "^BY4,3^FO65,707^BCN,65,N,Y,N^FR^FD>:" & Barcode & "^FS"                '가로 바코드, 20090316 chh
            '            Zpl &= "^A0N90,30^FO062,778^FD" & Me.BARCODE_SIZE_NAME & "X " & Me.BARCODE_Length & " M-" & "^FS" & "^A0N100,30^FO367,778^FD" & Me.BARCODE_STEEL & "^FS" & vbCrLf
            '            Zpl &= "^FO480,778^A0N,30,34^CI13^FR^FD" & Barcode & "^FS"                     '가로 바코드 텍스트 - 1
            '                Case "IA" : PoomMyoung = "INVERED ANGLE"
            '                Case "EA" : PoomMyoung = "EAQUAL ANGLE"
            '                Case "IB" : PoomMyoung = "I-BEAM" '영문자 i
            '                Case Else : PoomMyoung = "" '내용없음
            '            End Select
            */

            if(BARCODE_ITEM == "HB")
            {
                PoomMyoung = "H-BEAM";
            }
            else if(BARCODE_ITEM == "TP")
            {
                PoomMyoung = "SHEET PILE";
                Zpl += "^BY4,3^FO65,707^BCN,65,N,Y,N^FR^FD>:" + Barcode + "^FS";                //가로 바코드, 20090316 chh
                Zpl += "^A0N90,30^FO062,778^FD" + BARCODE_SIZE_NAME + "X " + BARCODE_Length + " M-" + "^FS" + "^A0N100,30^FO367,778^FD" + BARCODE_STEEL + "^FS";
                Zpl += "^FO480,778^A0N,30,34^CI13^FR^FD" + Barcode + "^FS";                     //가로 바코드 텍스트-1
            }
            else if(BARCODE_ITEM == "AI")
            {
                PoomMyoung = "INVERED ANGLE";
            }
            else if (BARCODE_ITEM == "AE")
            {
                PoomMyoung = "EAQUAL ANGLE";
            }
            else if (BARCODE_ITEM == "IB")
            {
                PoomMyoung = "I-BEAM";
            }
            else
            {
                PoomMyoung = "";
            }

            Zpl += "^FO49,120^A0N,55,50^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";                                            //제품사양
            Zpl += "^FO49,188^A0N,49,45^CI13^FR^FD" + BARCODE_HEAT + "^FS";                                                 //Heat No
            Zpl += "^FO490,153^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";      //제품길이
            Zpl += "^FO49,45^A0N,40,40^CI13^FR^FD" + PoomMyoung + "^FS";                                                    //제품명
            /*
            '--//2008.08.05 SHARP 수정 ------------------------------------------------------------------
            '''Zpl = Zpl & "^FO49,86^A0N,48,37^CI13^FR^FDGRADE^FS"                '고정텍스트(GRADE)
            '''Zpl = Zpl & "^FO36,135^A0N,51,29^CI13^FR^FDDIMENSION^FS"           '고정텍스트(DIMENSION)
            '''Zpl = Zpl & "^FO40,188^A0N,49,29^CI13^FR^FDHEAT-NO^FS"             '고정텍스트(HEAT - NO)
            '''Zpl = Zpl & "^FO181,22^GB0,213,3^FS"          '       세로 라인
            '''Zpl = Zpl & "^FO22,079^GB657,0,3^FS"          '첫번째 가로 라인
            '''Zpl = Zpl & "^FO22,127^GB657,0,3^FS"          '두번째 가로 라인
            '''Zpl = Zpl & "^FO22,175^GB657,0,3^FS"          '세번째 가로 라인
            '--------------------------------------------------------------------------------------------
            */
            Zpl += BARCODE_MODE;              //Print Mode ("Tear Off" or "Cutter")
            Zpl += "^PQ1,0,0,N";
            Zpl += "^XZ";

            return Zpl;
        }

        public string PB4_MANUAL_LABEL(int Count)
        {

            //정정검사 바코드 만들기 (내수, 수출)
            var printCnt = 0;
            printCnt = BARCODE_PRINT_COUNT;

            var Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            var Barcode1 = "";
            var PoomMyoung = "";
            var ItemSize = "";
            var P_Name = "";
            var Zpl = "";

            if (BARCODE_STEEL.Length > 19 && BARCODE_STEEL.Length < 23)      //B9 강종 추가로 수정 (20130103김영조)
            {
                //Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + Mid(BARCODE_STEEL, 1, 6) + "/" + Mid(BARCODE_STEEL, 13, 5);
                Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + BARCODE_STEEL.Substring(0, 6) + "/" + BARCODE_STEEL.Substring(12, 5);
            }
            else
            {
                Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + BARCODE_STEEL;
            }

            if (BARCODE_MARKET_TYPE == "내수" || BARCODE_MARKET_TYPE == "1511")
            {
                
                //내수용 라벨
                Zpl += "^XA";
                Zpl += "^POI^FS";
                Zpl += "^PW720";
                Zpl += "^LH0,0^FS";       //기본좌표
                Zpl += "^BY3.0,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";     //바코드 090206 kde

                //2011.04.18, SHARP : 내수의 경우 내진용 특정 강종에 대해서는 세로바코드 영역에 "내진용"이라는 글자를 출력하도록 요청함(QA:하경태과장)
                //세로바코드
                if (BARCODE_STEEL.Substring(0, 3) == "SHN") //"SHN400", "SHN490", "SHN490.", "SHN520", "SHN520.", "SHN570"
                {
                    if (BARCODE_STEEL.Length > 7)    //SHN400 - TRIPLE(SHN400/SS400/A36), SHN490 - DUAL(SHN490/SM490A), 2014.03.07
                    {
                        Zpl += "^BY2.0,1.5^FO580,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS";   //세로바코드
                    }
                    else
                    {
                        Zpl += "^FO560,404^FR^XGR:NAEJIN,1,1^FS";   //KS
                    }
                }
                else
                {
                    Zpl += "^BY2.0,1.5^FO580,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS";     //세로바코드090206 kde
                }

                Zpl += "^FO170,607^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";             //바코드 텍스트

                //SIDE 라벨 출력부분 통합요청(자기부상레일과 동일한 형태로 출력), 위 로직주석처리 - 2011.10.26, SHARP , 하경태과장
                Zpl += "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";                //가로 바코드
                Zpl += "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";                     //가로 바코드 텍스트-1
                Zpl += "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS";                //Heat No
                //Zpl += "^FO520,752^A0N,28,28^CI13^FR^FD" + Mid(BARCODE_INSPECT_DATE, 1, 4) + "-" + Mid(BARCODE_INSPECT_DATE, 5, 2) + "-" + Mid(BARCODE_INSPECT_DATE, 7, 2) + "^FS"; //발행일자
                Zpl += "^FO520,752^A0N,28,28^CI13^FR^FD" + BARCODE_INSPECT_DATE.Substring(0, 4) + "-" + BARCODE_INSPECT_DATE.Substring(4, 2) + "-" + BARCODE_INSPECT_DATE.Substring(6, 2) + "^FS"; //발행일자
                Zpl += "^FO004,718^FD^XGR:HD,1,1^FS";                                            //Hyundai Image (H)

                if (BARCODE_MARK == "1")    //mark종류에 따른 마크 출력
                {
                    Zpl += "^FO588,35^FR^XGKI007,1,1^FS";   //KS
                                                            //20110810 구성욱 보완(A36.이면 한국표준협회 글자 안 보이기 : 2011 08 10 하경태 요청
                    if (BARCODE_STEEL != "A36.")
                    {
                        Zpl += "^FO030,323^FR^XGR:KSA_1,1,1^FS";    //"한국표준협회" 글자 이미지 출력 : 2009.12.18, SHARP
                    }
                }
                else if (BARCODE_MARK == "2")
                {
                    Zpl += "^FO538,20^FR^XGKI006,1,1^FS";               //JIS  '1 column up 55->30 081014 kde
                }

                Zpl += "^FO615,303^A0N,42,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";                                   //작업자
                Zpl += "^FO040,298^A0N,35,35^CI13^FR^FD" + BARCODE_APPORVAL_NO + "^FS";                                 //허가번호
                Zpl += "^FO080,645^A0N,48,42^CI13^FR^FD" + Barcode.Substring(0, 6) + "-" + Barcode.Substring(6, 4) + "^FS";       //PO+BD
                Zpl += "^FO376,645^A0N,48,42^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";                                //발행일자


                switch (BARCODE_ITEM)
                {
                    case "NO":
                        Zpl += ""; //
                        break;
                    case "HB":
                        PoomMyoung = "H-BEAM";           //2011.04.18, SHARP 수정
                        break;
                    case "TP":
                        PoomMyoung = "SHEET-PILE";       //2011.04.18, SHARP 수정
                        break;
                    case "AI":
                        PoomMyoung = "L";                //ㄱ
                        break;
                    case "AE":
                        PoomMyoung = "L";                //ㄱ
                        break;
                    case "IB":
                        PoomMyoung = "I";                //영문자 i
                        break;
                    case "TS":
                        PoomMyoung = "TS";               //영문자 i
                        break;
                    case "RR":
                        PoomMyoung = "RL";               //영문자 i
                        break;
                    default:
                        PoomMyoung = "";                 //내용없음
                        break;
                }

                switch (PoomMyoung)
                {
                    case "H-BEAM":
                        Zpl += "^FO043,30^A0N,45,40^CI13^FP^FD" + PoomMyoung + "^FS";               //H 형강 : 2011.04.18, SHARP
                        break;
                    case "I":
                        Zpl += "^FO280,30^ADN,55,70^CI13^FP^FD" + PoomMyoung + "^FS";                    //I 형강
                        break;
                    case "SHEET-PILE":
                        Zpl += "^FO043,30^A0N,46,41^CI13^FP^FD" + PoomMyoung + "^FS";           //SP 형강 : 2011.04.18, SHARP(SP, RL 분리)
                        Zpl += "^FO0275,30^A0N,40,35^CI13^FP^FD(U-SHAPE)^FS";                  //2015.06.17 KDE
                        break;
                    case "RL":
                        Zpl += "^FO270,30^A0N,55,70^CI13^FP^FD" + PoomMyoung + "^FS";                   //SP 형강 : 2011.04.18, SHARP
                        break;
                    case "L":
                        Zpl += "^FO300,22^A0I,50,100^CI13^FP^FD" + PoomMyoung + "^FS";                   //ㄱ 형강
                        break;
                    case "TS":
                        Zpl += "^FO280,32^A0N,55,100^CI13^FP^FD" + PoomMyoung + "^FS";                  //TS 형강
                        break;
                }

                //KS인증명2015.8.28
                if(BARCODE_KS.Length > 0) { 
                Zpl += "^FO240,300^A0N,30,24^CI13^FR^FD" + BARCODE_KS.Substring(0, 17) + "^FS";
                Zpl += "^FO240,328^A0N,30,24^CI13^FR^FD" + BARCODE_KS.Substring(17, BARCODE_KS.Length - 17) + "^FS";
                }

                //Zpl = Zpl & "^FO212,85^A0N,45,40^CI13^FR^FD" & BARCODE_SPEC & "^FS"      '규격 Num

                //해당 강종(FX,FZ)의 강종명이 바뀌어 넘어오면 여기 조건에 걸려
                //규격+강종명 2줄 출력하는데 규격과 2라인을 소스에 직접 코딩함 (20140307SHARP)
                if (BARCODE_STEEL == "SHN400/SS400/A36")
                {
                    Zpl += "^FO043,147^A0N,29,29^CI13^FR^FD" + "KS D 3866" + "^FS";              //규격 Num1
                    Zpl += "^FO170,143^A0N,38,38^CI13^FR^FD" + BARCODE_STEEL.Substring(0, 7) + "^FS";  //강종명1
                    Zpl += "^FO043,187^A0N,29,29^CI13^FR^FD" + "KS D 3503" + "^FS";              //규격 Num2
                    Zpl += "^FO170,183^A0N,38,38^CI13^FR^FD" + "SS400/ASTM A36" + "^FS";         //강종명2
                }
                else if (BARCODE_STEEL == "SHN490/SM490A")
                {
                    Zpl += "^FO043,147^A0N,29,29^CI13^FR^FD" + "KS D 3866" + "^FS";                 //규격 Num1
                    Zpl += "^FO170,143^A0N,38,38^CI13^FR^FD" + BARCODE_STEEL.Substring(0, 7) + "^FS";    //강종명1
                    Zpl += "^FO043,187^A0N,29,29^CI13^FR^FD" + "KS D 3515" + "^FS";                 //규격 Num2
                    Zpl += "^FO170,183^A0N,38,38^CI13^FR^FD" + BARCODE_STEEL.Substring(7, 6) + "^FS";    //강종명2
                }
                else
                {
                    //강종명 자리수에 따른 발행위치 계산
                    if (BARCODE_STEEL.Length > 0 && BARCODE_STEEL.Length < 12)
                    {
                        Zpl += "^FO043,135^A0N,45,40^CI13^FR^FD" + BARCODE_SPEC + "^FS";      //규격 Num
                        if (BARCODE_STEEL == "A")
                        {
                            Zpl += "^FO043,169^A0N,57,41^CI13^FR^FD" + "GRADE " + BARCODE_STEEL + "^FS"; //강종명
                        }
                        else
                        {
                            Zpl += "^FO043,183^A0N,57,41^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                        }
                    }
                    else if (BARCODE_STEEL.Length > 11 && BARCODE_STEEL.Length < 16)
                    {
                        Zpl += "^FO043,135^A0N,45,40^CI13^FR^FD" + BARCODE_SPEC + "^FS";      //규격 Num
                        if (BARCODE_STEEL == "A")
                        {
                            Zpl += "^FO043,169^A0N,57,41^CI13^FR^FD" + "GRADE " + BARCODE_STEEL + "^FS";     //강종명
                        }
                        else
                        {
                            Zpl += "^FO043,183^A0N,57,41^CI13^FR^FD" + BARCODE_STEEL + "^FS";  //강종명
                        }
                    }
                    else if (BARCODE_STEEL.Length > 15 && BARCODE_STEEL.Length < 22)
                    {
                        Zpl += "^FO043,135^A0N,45,40^CI13^FR^FD" + BARCODE_SPEC + "^FS";      //규격 Num
                        if (BARCODE_STEEL == "A")
                        {
                            Zpl += "^FO043,169^A0N,57,25^CI13^FR^FD" + "GRADE " + BARCODE_STEEL + "^FS"; //강종명
                        }
                        else
                        {
                            Zpl += "^FO043,183^A0N,57,41^CI13^FR^FD" + BARCODE_STEEL + "^FS";            //강종명
                        }
                    }
                    else if (BARCODE_STEEL.Length > 23 && BARCODE_STEEL.Length < 31)
                    {
                        Zpl += "^FO043,155^A0N,32,28^CI13^FR^FD" + BARCODE_SPEC + "^FS";                 //규격 Num
                        Zpl += "^FO163,143^A0N,54,38^CI13^FR^FD" + BARCODE_STEEL.Substring(0, 7) + "^FS";     //강종명(SS400/) '--//2010.08.04, SHARP 추가 : 하경태대리 요청사항
                        Zpl += "^FO281,155^A0N,32,28^CI13^FR^FD" + BARCODE_STEEL.Substring(7, 4) + "^FS";     //강종명(ASTM) '--//2010.08.04, SHARP 추가 : 하경태대리 요청사항
                        Zpl += "^FO341,143^A0N,54,38^CI13^FR^FD" + BARCODE_STEEL.Substring(11, 4) + "^FS";    //강종명( A36) '--//2010.08.04, SHARP 추가 : 하경태대리 요청사항
                    }
                }

                if (BARCODE_ITEM == "WX")
                {
                    Zpl += "^FO043,85^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "lb" + "^FS";                         //제품사양
                }
                else
                {
                    Zpl += "^FO043,85^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";                                //제품사양
                }

                Zpl += "^FO500,192^A0N,43,43^CI13^FR^FD" + BARCODE_HEAT + "^FS";                                        //Heat No
                Zpl += "^FO490,133^A0N,65,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";   //제품길이
                Zpl += BARCODE_MODE;                                                                        //Print Mode ("Tear Off" or "Cutter")
                Zpl += "^PQ1,0,0,N";
                Zpl += "^XZ";
            }

            else if (BARCODE_MARKET_TYPE == "수출" || BARCODE_MARKET_TYPE == "1522")
            {
                //수출용 라벨
                Zpl = Zpl + "^XA";
                Zpl = Zpl + "^POI^FS";
                Zpl = Zpl + "^PW712";    //742
                Zpl = Zpl + "^LH0,0^FS"; //기본좌표

                Zpl = Zpl + "^BY3.0,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";      //바코드 090206 kde
                //세로바코드
                Zpl = Zpl + "^BY2.0,1.5^FO580,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS";     //세로바코드090206 kde
                Zpl = Zpl + "^FO170,604^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";                //바코드 텍스트

                //내수용 바코드 아래 미니 바코드 추가 - 기봉춘 K, 구성 욱 S 요청 081217
                Zpl = Zpl + "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";    //가로 바코드

                //강종 'B9' 추가로 길이가 23 이라서 폰트 사이즈 줄임 (20130103김영조) F070 → F060
                if (BARCODE_STEEL.Length > 22)
                {
                    Zpl = Zpl + "^FO60,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";                     //가로 바코드 텍스트-1
                }
                else
                {
                    //그외에는 그대로 사용
                    Zpl = Zpl + "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";                     //가로 바코드 텍스트-1
                }

                Zpl = Zpl + "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS";                    //Heat No
                Zpl = Zpl + "^FO520,752^A0N,28,28^CI13^FR^FD" + BARCODE_INSPECT_DATE.Substring(0, 4) + "-" + BARCODE_INSPECT_DATE.Substring(4, 2) + "-" + BARCODE_INSPECT_DATE.Substring(6, 2) + "^FS"; //발행일자

                switch (BARCODE_MARK)
                {    //mark종류에 따른 마크 출력 
                    case "1":
                        Zpl = Zpl + "^FO588,35^FR^XGKI007,1,1^FS";
                        break;
                    case "2":
                        Zpl = Zpl + "^FO538,20^FR^XGKI006,1,1^FS";       //1 column up 55->30 081014 kde
                        break;
                }

                Zpl = Zpl + "^FO583,314^A0N,42,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";                                  //작업자
                Zpl = Zpl + "^FO063,314^A0N,45,40^CI13^FR^FD" + BARCODE_APPORVAL_NO + "^FS";                                //허가번호
                Zpl = Zpl + "^FO080,645^A0N,48,42^CI13^FR^FD" + Barcode.Substring(0, 6) + "-" + Barcode.Substring(6, 4) + "^FS";      //PO+BD
                Zpl = Zpl + "^FO376,645^A0N,48,42^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";                              //발행일자

                if (BARCODE_SPEC.Length > 0 && BARCODE_SPEC.Length < 16)
                {
                    Zpl = Zpl + "^FO036,140^A0N,45,40^CI13^FR^FD" + BARCODE_SPEC + "^FS"; //규격 Num
                }
                else if (BARCODE_SPEC.Length > 15 && BARCODE_SPEC.Length < 22)
                {
                    Zpl = Zpl + "^FO036,140^A0N,42,25^CI13^FR^FD" + BARCODE_SPEC + "^FS"; //규격 Num
                }

                //강종명 자리수에 따른 발행위치 계산
                if (BARCODE_SPEC == "")  //2012.02.27, SHARP 보완 : 규격번호가 없는 경우에 대한 로직 추가
                {
                    if (BARCODE_STEEL.Length > 0 && BARCODE_STEEL.Length < 12)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,50,41^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                    }
                    else if (BARCODE_STEEL.Length > 11 && BARCODE_STEEL.Length < 16)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,50,31^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                    }
                    else if (BARCODE_STEEL.Length > 15 && BARCODE_STEEL.Length < 22)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,47,25^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                    }
                    else if (BARCODE_STEEL.Length > 21 && BARCODE_STEEL.Length < 25)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,44,26^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명 (20130103김영조) 강종 추가 'B9'
                    }
                }
                else
                {
                    if (BARCODE_STEEL.Length > 0 && BARCODE_STEEL.Length < 12)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,50,41^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                    }
                    else if (BARCODE_STEEL.Length > 11 && BARCODE_STEEL.Length < 16)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,50,31^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                    }
                    else if (BARCODE_STEEL.Length > 15 && BARCODE_STEEL.Length < 22)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,47,25^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                    }
                    else if (BARCODE_STEEL.Length > 21 && BARCODE_STEEL.Length < 25)
                    {
                        Zpl = Zpl + "^FO036,188^A0N,44,26^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명 (20130103김영조) 강종 추가 'B9'
                    }
                }

                switch (BARCODE_ITEM)
                {
                    case "HB":
                        PoomMyoung = "H-BEAM";

                        if (BARCODE_STEEL == "SS400 B" || BARCODE_STEEL == "A572G50 B")  //A572G50 B 강종일때 무시 (201207202김영조)
                        {
                            PoomMyoung = "";
                        }
                        else
                        {
                            PoomMyoung = "H-BEAM";
                        }
                        break;
                    case "TP":
                        PoomMyoung = "SHEET PILE";
                        break;
                    case "AI":
                        PoomMyoung = "INVERED ANGLE";
                        break;
                    case "AE":
                        PoomMyoung = "EAQUAL ANGLE";
                        break;
                    case "IB":
                        PoomMyoung = "I-BEAM";
                        break;
                    default:
                        PoomMyoung = "";
                        break;
                }

                //KS인증명2015.8.28  에러나서 일단 수출은 한줄로 인쇄되도록 해놓음(원정호)
                Zpl = Zpl + "^FO340,300^A0N,30,24^CI13^FR^FD" + BARCODE_KS.Substring(0, BARCODE_KS.Length) + "^FS";
                //Zpl = Zpl + "^FO340,328^A0N,30,24^CI13^FR^FD" + BARCODE_KS.Substring(17, BARCODE_KS.Length - 17) + "^FS";

                if (BARCODE_ITEM == "WX")
                {
                    Zpl = Zpl + "^FO036,87^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "lb" + "^FS";       //제품사양
                }
                else
                {
                    Zpl = Zpl + "^FO036,87^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";       //제품사양
                }

                Zpl = Zpl + "^FO490,188^A0N,53,43^CI13^FR^FD" + BARCODE_HEAT + "^FS";     //Heat No
                Zpl = Zpl + "^FO490,120^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";     //제품길이

                if (PoomMyoung == "SHEET PILE")
                {
                    Zpl = Zpl + "^FO036,35^A0N,45,40^CI13^FR^FD" + PoomMyoung + "^FS";  //제품명
                    Zpl = Zpl + "^FO0255,35^A0N,40,35^CI13^FR^FD(U-SHAPE)^FS";  //제품명
                }
                else
                {
                    Zpl = Zpl + "^FO036,35^A0N,45,40^CI13^FR^FD" + PoomMyoung + "^FS";  //제품명
                }

                Zpl = Zpl + BARCODE_MODE;              //Print Mode ("Tear Off" or "Cutter")
                Zpl = Zpl + "^PQ1,0,0,N";
                Zpl = Zpl + "^XZ";
            }
            else
            {
                //수출용 라벨 (INCH-BEAM)

                //철강일원화 관련 수정 (20121206김영조) : 품목 코드 변경
                switch (BARCODE_ITEM)
                {
                    case "HB":
                        ItemSize = "H-BEAM";
                        break;
                    default:
                        ItemSize = "";
                        break;
                }

                Zpl = Zpl + "^XA";
                Zpl = Zpl + "^POI^FS";
                Zpl = Zpl + "^PW714";
                Zpl = Zpl + "^LH0,0^FS";
                Zpl = Zpl + "^BY3.0,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";     //바코드 090206 kde
                //세로바코드
                Zpl = Zpl + "^BY2.0,1.5^FO580,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS";     //세로바코드090206 kde
                Zpl = Zpl + "^FO170,604^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";           //바코드텍스트
                Zpl = Zpl + "^FO306,318^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";            //'바코드 텍스트 //==>20090828 구성욱 보완 하경태 대리 요청(ksa 09-0330과 제품번호가 겹침)

                //SIDE 라벨 출력부분 통합요청(자기부상레일과 동일한 형태로 출력), 위 로직주석처리 - 2011.10.26, SHARP , 하경태과장
                Zpl = Zpl + "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";                //가로 바코드
                Zpl = Zpl + "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";                     //가로 바코드 텍스트-1
                Zpl = Zpl + "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS";                //Heat No
                Zpl = Zpl + "^FO520,752^A0N,28,28^CI13^FR^FD" + BARCODE_INSPECT_DATE.Substring(0, 4) + "-" + BARCODE_INSPECT_DATE.Substring(4, 2) + "-" + BARCODE_INSPECT_DATE.Substring(6, 2) + "^FS";     //발행일자

                Zpl = Zpl + "^FO573,314^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";             //작업자
                Zpl = Zpl + "^FO063,314^A0N,45,40^CI13^FR^FD" + BARCODE_APPORVAL_NO + "^FS";           //허가번호
                Zpl = Zpl + "^FO080,645^A0N,48,42^CI13^FR^FD" + Barcode.Substring(0, 6) + "-" + Barcode.Substring(6, 4) + "^FS";    //PO+BD

                // sDate 로 날짜출력 통일, 2011.10.31, SHARP
                Zpl = Zpl + "^FO376,645^A0N,48,42^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";        //발행일자

                //강종명 자리수에 따른 발행위치 계산
                if (BARCODE_STEEL.Length > 0 && BARCODE_STEEL.Length < 12)
                {
                    Zpl = Zpl + "^FO034,185^A0N,51,57^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                }
                else if (BARCODE_STEEL.Length > 11 && BARCODE_STEEL.Length < 16)
                {
                    Zpl = Zpl + "^FO034,185^A0N,51,40^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                }
                else if (BARCODE_STEEL.Length > 15 && BARCODE_STEEL.Length < 22)
                {
                    Zpl = Zpl + "^FO034,185^A0N,42,32^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명
                }
                else if (BARCODE_STEEL.Length > 21 && BARCODE_STEEL.Length < 15)
                {
                    Zpl = Zpl + "^FO036,188^A0N,44,26^CI13^FR^FD" + BARCODE_STEEL + "^FS"; //강종명 (20130103김영조) 강종 추가 'B9'
                }

                Zpl = Zpl + "^FO034,39^A0N,45,40^CI13^FR^FD" + ItemSize + "^FS";           //제품


                //철강일원화 관련 기능 추가 (20121205김영조) : 올드 규격을 조회
                if (BARCODE_OLD_ITEM == "WX")
                {
                    Zpl = Zpl + "^FO036,087^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "lb" + "^FS";       //제품사양
                }
                else
                {
                    Zpl = Zpl + "^FO036,087^A0N,51,40^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";              //제품사양
                }

                Zpl = Zpl + "^FO480,185^A0N,53,43^CI13^FR^FD" + BARCODE_HEAT + "^FS";                                        //Heat No
                Zpl = Zpl + "^FO480,100^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";    //제품길이

                if (BARCODE_SPEC.Length > 0 && BARCODE_SPEC.Length < 16)
                {
                    Zpl = Zpl + "^FO34,135^A0N,43,36^CI13^FR^FD" + BARCODE_SPEC + "^FS"; //규격 Num
                }
                else if (BARCODE_SPEC.Length > 15 && BARCODE_SPEC.Length < 22)
                {
                    Zpl = Zpl + "^FO34,135^A0N,43,26^CI13^FR^FD" + BARCODE_SPEC + "^FS"; //규격 Num
                }

                Zpl = Zpl + BARCODE_MODE;           //Print Mode ("Tear Off" or "Cutter")
                Zpl = Zpl + "^PQ1,0,0,N";
                Zpl = Zpl + "^XZ";
            }

            return Zpl;
        }               //대형 내수,수출 라벨 발행 끝


        public string PB4_MANUAL_LABEL2(int Count)
        {
        
            //정정검사 (수출 : CE Mark용) - 2008.08.04, SHARP
    
            var printCnt = 0;
            printCnt = BARCODE_PRINT_COUNT;

            var Barcode = BARCODE_PO_NO + BARCODE_BUNDLE_NO;
            var Barcode1 = "";
            var PoomMyoung = "";
            var ItemSize = "";
            var AR_N = "";
            var Zpl = "";

            //가로 바코드(미니?) 강종 체크 추가
            if (BARCODE_STEEL == "S355G11")
            {
                Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + BARCODE_STEEL + "+M";
            }
            else
            {
                Barcode1 = BARCODE_PO_NO + "-" + BARCODE_BUNDLE_NO + " " + BARCODE_SIZE_NAME + " " + BARCODE_Length + " " + BARCODE_UOM + " " + "-" + " " + BARCODE_STEEL;
            }

            //HeatTreatment = "0" : 미열처리 - 강종 + 'AR'
            Zpl = Zpl + "^XA";
            Zpl = Zpl + "^POI^FS";
            Zpl = Zpl + "^PW712";               //742
            Zpl = Zpl + "^LH0,0^FS";            //기본좌표
            //2008.08.05, SHARP 수정
            // 1. CE Mark 용 태그 도입에 따른 바코드 출력크기 조정(작게) 및 전체적으로 출력위치 조정
            // 2. 대형제품 재고관리 시스템 도입관련 : 세로바코드 추가출력
            // 원로직 주석처리 → Zpl = Zpl & "^BY4,3.0^FO63,354^BCN,243,N,Y,N^FR^FD>:" & Barcode & "^FS"     '바코드
            Zpl = Zpl + "^BY2.0,1.5^FO285,360^BCN,243,N,Y,N^FR^FD>:" + Barcode + "^FS";     //바코드
            // 세로 바코드 출력(BCB)
            Zpl = Zpl + "^BY2.0,1.5^FO600,364^BCB,100,N,Y,N^FR^FD>:" + Barcode + "^FS";     //바코드
            Zpl = Zpl + "^FO260,620^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";             //바코드 텍스트
            Zpl = Zpl + "^FO260,320^A0N,23,41^CI13^FR^FD" + Barcode + "^FS";             //바코드 텍스트

            Zpl = Zpl + "^FO520,314^A0N,45,40^CI13^FR^FD" + BARCODE_INSPECTOR + "^FS";        //작업자
            Zpl = Zpl + "^FO063,314^A0N,35,30^CI13^FR^FD" + BARCODE_APPORVAL_NO + "^FS";      //허가번호
            Zpl = Zpl + "^FO220,670^A0N,38,38^CI13^FR^FD" + Barcode.Substring(0, 6) + "-" + Barcode.Substring(6, 4) + "^FS";     //PO+BD
            // sDate 로 날짜출력 통일, 2011.10.31, SHARP
            Zpl = Zpl + "^FO506,670^A0N,38,38^CI13^FR^FD" + BARCODE_INSPECT_DATE + "^FS";   //발행일자

            //2008.08.05 SHARP 수정 -----------------------------------------------------------------
            Zpl = Zpl + "^FO49,140^A0N,35,35^CI13^FR^FD" + BARCODE_SPEC + "^FS";         //규격 Num

            //SIDE 라벨 출력부분 통합요청(자기부상레일과 동일한 형태로 출력), 위 로직주석처리 - 2011.10.26, SHARP , 하경태과장
            Zpl = Zpl + "^BY3,3^FO70,710^BCN,60,N,Y,N^FR^FD>:" + Barcode + "^FS";                //가로 바코드
            Zpl = Zpl + "^FO70,780^A0N,22,22^CI13^FR^FD" + Barcode1 + "^FS";                     //가로 바코드 텍스트-1
            Zpl = Zpl + "^FO533,715^A0N,37,37^CI13^FR^FD" + BARCODE_HEAT + "^FS";                         //Heat No
            Zpl = Zpl + "^FO520,752^A0N,28,28^CI13^FR^FD" + BARCODE_INSPECT_DATE.Substring(0, 4) + "-" + BARCODE_INSPECT_DATE.Substring(4, 2) + "-" + BARCODE_INSPECT_DATE.Substring(6, 2) + "^FS";    //발행일자
                
            //조건문 무시 강종 추가로(S355J0+M) 수정함 (20141121김영조)
            if(BARCODE_STEEL == "S355J2+M" || BARCODE_STEEL == "S355J0+M")
            {
                AR_N = "";
            }
            else
            {
                switch (BARCODE_HEAT_DSP)
                {
                    case "0":
                        if (BARCODE_STEEL == "S355G11")
                        {
                            AR_N = "+M";
                        }
                        else
                        {
                            AR_N = "+AR";
                        }
                        break;
                    case "1":
                        AR_N = "+N";
                        break;
                }
            }   

            if(BARCODE_STEEL.Substring(BARCODE_STEEL.Length - 2, 2) == "04")
            {
                BARCODE_STEEL = BARCODE_STEEL.Substring(0, BARCODE_STEEL.Length - 2);
            }

            //강종명 변경 추가 (20120925김영조)
            if(BARCODE_STEEL == "S275JR+B")
            {
                BARCODE_STEEL = "S275JRB";
            }
            else if(BARCODE_STEEL == "S355JR+B")
            {
                BARCODE_STEEL = "S355JRB";
            }

            if(BARCODE_STEEL.Length > 0 && BARCODE_STEEL.Length < 12)
            {
                Zpl = Zpl + "^FO049,188^A0N,52,52^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";    //강종명
            }
            else if (BARCODE_STEEL.Length > 11 && BARCODE_STEEL.Length < 16)
            {
                Zpl = Zpl + "^FO049,188^A0N,50,35^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";    //강종명
            }
            else if (BARCODE_STEEL.Length > 15 && BARCODE_STEEL.Length < 22)
            {
                Zpl = Zpl + "^FO049,188^A0N,50,29^CI13^FR^FD" + BARCODE_STEEL + AR_N + "^FS";    //강종명
            }

            //철강일원화 관련 수정 (20121128김영조) : 품목 코드 변경
            
            switch(BARCODE_ITEM)
            {
                case "HB" : PoomMyoung = "H-BEAM";
                    break;
                case "TP" : PoomMyoung = "SHEET PILE";
                    break;
                case "AI" : PoomMyoung = "INVERED ANGLE";
                    break;
                case "AE" : PoomMyoung = "EAQUAL ANGLE";
                    break;
                case "IB" : PoomMyoung = "I-BEAM";      //영문자 i
                    break;
                default : PoomMyoung = "";              //내용없음
                    break;
            }   

            //철강일원화 관련 기능 추가 (20121205김영조) : 올드 규격을 조회
            if(BARCODE_OLD_ITEM == "WX")
            {
                Zpl = Zpl + "^FO49,087^A0N,55,50^CI13^FR^FD" + BARCODE_SIZE_NAME + "lb" + "^FS";        //제품사양
            }   
            else
            {
                Zpl = Zpl + "^FO49,087^A0N,55,50^CI13^FR^FD" + BARCODE_SIZE_NAME + "^FS";               //제품사양
            }

            Zpl = Zpl + "^FO490,188^A0N,49,45^CI13^FR^FD" + BARCODE_HEAT + "^FS";                       //Heat No
            Zpl = Zpl + "^FO490,115^A0N,75,65^CI13^FR^FD" + BARCODE_Length + BARCODE_UOM + "^FS";     //제품길이
            Zpl = Zpl + "^FO49,35^A0N,40,40^CI13^FR^FD" + PoomMyoung + "^FS";                            //제품명

            Zpl = Zpl + BARCODE_MODE;              //Print Mode ("Tear Off" or "Cutter")
            Zpl = Zpl + "^PQ1,0,0,N";
            Zpl = Zpl + "^XZ";

            return Zpl;
        }

        #endregion

            #region "바코드 이미지"
        public string Print_Image(string tmpMARKET)
        {
            var Zpl = "";

            switch (tmpMARKET)
            {
                case "KS":
                    //KS 신버전 이미지 : 2009.12.18, SHARP(프로그램 내 이미지 명칭 - "GR:KS_PB51" 임.)
                    Zpl += "~DGR:KS_PB51,00488,008,";
                    Zpl += "0000000000000000";
                    Zpl += "0000003FF0000000";
                    Zpl += "000003FFFF800000";
                    Zpl += "00001FFFFFE00000";
                    Zpl += "00007FFFFFF00000";
                    Zpl += "0000FFFFFFFC0000";
                    Zpl += "0003FFFFFFFF0000";
                    Zpl += "0007FFFFFFFF8000";
                    Zpl += "001FFFF83FFFC000";
                    Zpl += "003FFE0001FFE000";
                    Zpl += "007FF800007FF000";
                    Zpl += "007FE000001FF800";
                    Zpl += "00FFC000000FFC00";
                    Zpl += "01FF80000003FE00";
                    Zpl += "01FF00000001FF00";
                    Zpl += "03FE00000000FF00";
                    Zpl += "07FC000000007F80";
                    Zpl += "0FF8000000003FC0";
                    Zpl += "0FF0000000003FC0";
                    Zpl += "1FF03FC07FE01FE0";
                    Zpl += "1FE03FC0FFC00000";
                    Zpl += "1FC03FC1FF800000";
                    Zpl += "3FC03FC3FF000000";
                    Zpl += "3F803FC7FE000000";
                    Zpl += "3F803FCFFC000000";
                    Zpl += "7F003FDFF8000000";
                    Zpl += "7F003FFFF0000000";
                    Zpl += "7FFF3FFFF007FFF8";
                    Zpl += "7FFF3FFFF007FFF8";
                    Zpl += "7FFF3FFFF007FFF8";
                    Zpl += "7FFF3FFFF807FFF8";
                    Zpl += "7FFF3FFFFC07FFF8";
                    Zpl += "7FFF3FFBFC07FFF8";
                    Zpl += "7FFF3FF1FE07FFF8";
                    Zpl += "00003FE0FF0007F8";
                    Zpl += "00003FC07F8007F8";
                    Zpl += "00003FC03FC007F0";
                    Zpl += "00003FC01FE00FF0";
                    Zpl += "00003FC01FF00FF0";
                    Zpl += "00003FC00FF01FE0";
                    Zpl += "0FF0000000001FE0";
                    Zpl += "0FF8000000003FE0";
                    Zpl += "07F8000000003FC0";
                    Zpl += "03FC000000007F80";
                    Zpl += "03FF00000001FF80";
                    Zpl += "01FF80000003FF00";
                    Zpl += "00FFC0000007FE00";
                    Zpl += "00FFC000000FFC00";
                    Zpl += "007FE000001FFC00";
                    Zpl += "003FF800007FF800";
                    Zpl += "001FFE0001FFF000";
                    Zpl += "000FFFF03FFFE000";
                    Zpl += "0007FFFFFFFF8000";
                    Zpl += "0001FFFFFFFF0000";
                    Zpl += "00007FFFFFFC0000";
                    Zpl += "00003FFFFFF80000";
                    Zpl += "00000FFFFFE00000";
                    Zpl += "000003FFFF800000";
                    Zpl += "0000003FF8000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    Zpl += "0:";
                    break;
                case "JIS":
                    Zpl += "~DGR:JIS_7_1,00528,008,";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000003FF8000000";
                    Zpl += "000001FFFF800000";
                    Zpl += "00000FFFFFF00000";
                    Zpl += "00003FFFFFFC0000";
                    Zpl += "00007FFFFFFE0000";
                    Zpl += "0001FFFFFFFF8000";
                    Zpl += "0003FF0007FFC000";
                    Zpl += "000FFC0000FFE000";
                    Zpl += "001FF800003FF800";
                    Zpl += "003FF000000FFC00";
                    Zpl += "007FE0000007FC00";
                    Zpl += "00FF80000001FE00";
                    Zpl += "00FF00000000FF00";
                    Zpl += "01FE000000007F80";
                    Zpl += "03FC000000003F80";
                    Zpl += "03F8000000001FC0";
                    Zpl += "07F8000000000FC0";
                    Zpl += "0FF03F07E01FCFE0";
                    Zpl += "0FE03F07E03FC7F0";
                    Zpl += "0FE03F07E07FC7F0";
                    Zpl += "1FC03F07E07FC3F0";
                    Zpl += "1FC03F07E0FFC3F0";
                    Zpl += "1F803F07E0FFC1F8";
                    Zpl += "3F803F07E0F801F8";
                    Zpl += "3F803F07E0F801F8";
                    Zpl += "3F003F07E0F801F8";
                    Zpl += "3F003F07E0F800FC";
                    Zpl += "3F003F07E0F800FC";
                    Zpl += "3F003F07E0FE00FC";
                    Zpl += "3F003F07E07F00FC";
                    Zpl += "3F003F07E07F80FC";
                    Zpl += "3F003F07E03FC0FC";
                    Zpl += "3F003F07E007C0FC";
                    Zpl += "3F003F07E003E0FC";
                    Zpl += "3F003F07E003E1FC";
                    Zpl += "3F003F07E003E1F8";
                    Zpl += "1F803F07E007E1F8";
                    Zpl += "1F807F07E007E3F8";
                    Zpl += "1FC07E07E0FFC3F8";
                    Zpl += "0FE0FE07E0FFC3F0";
                    Zpl += "0FFFFC07E0FFC7F0";
                    Zpl += "07FFFC07E0FF8FF0";
                    Zpl += "03FFF807E0FE0FF0";
                    Zpl += "00FFE00000001FE0";
                    Zpl += "003F800000001FC0";
                    Zpl += "0000000000003FC0";
                    Zpl += "0000000000007F80";
                    Zpl += "000000000000FF80";
                    Zpl += "00FF00000001FF00";
                    Zpl += "007F80000003FE00";
                    Zpl += "003FE0000007FC00";
                    Zpl += "003FF000001FFC00";
                    Zpl += "001FFC00003FF800";
                    Zpl += "000FFF0000FFE000";
                    Zpl += "0003FFF03FFFC000";
                    Zpl += "0001FFFFFFFF8000";
                    Zpl += "00007FFFFFFE0000";
                    Zpl += "00003FFFFFFC0000";
                    Zpl += "00000FFFFFF00000";
                    Zpl += "000001FFFF800000";
                    Zpl += "0000001FF8000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    break;
            }
            return Zpl;
        }

        public string Print_Image1(string tmpMARKET)
        {
            var Zpl = "";

            switch (tmpMARKET)
            {
                case "KS":
                    //KS 구버전 이미지
                    Zpl += "~DGJNTYBKS_,378,7,";
                    Zpl += ",";
                    Zpl += ":";
                    Zpl += "K01FE,";
                    Zpl += "K0IFE,";
                    Zpl += "J03JF8,";
                    Zpl += "I01KFE,";
                    Zpl += "I07FEH0HF8,";
                    Zpl += "I0FEI01FC,";
                    Zpl += "H01F8J07E,";
                    Zpl += "H03FK01F,";
                    Zpl += "H07CL0F8,";
                    Zpl += "H0F8L07C,";
                    Zpl += "01FM03E,";
                    Zpl += "03EM01E,";
                    Zpl += "03EM01F,";
                    Zpl += "07CH0EH0FH0F80";
                    Zpl += "078H0E01EH0780";
                    Zpl += "0FI0E03CH03C0";
                    Zpl += "0FI0E078H03C0";
                    Zpl += "0EI0E0F,";
                    Zpl += "1EI0E0E,";
                    Zpl += "1EI0E1E,";
                    Zpl += "1EI0E3C,";
                    Zpl += "1CI0E78,";
                    Zpl += "1CI0EF,";
                    Zpl += "1HFE0FEH0JF0";
                    Zpl += "1HFE0FCH0JF0";
                    Zpl += ":";
                    Zpl += "1HFE0FEK0F0";
                    Zpl += "K0HFK0E0";
                    Zpl += "K0E78J0E0";
                    Zpl += "K0E3CI01E0";
                    Zpl += "K0E1EI01E0";
                    Zpl += "K0E0FI03C0";
                    Zpl += "0FI0E078H03C0";
                    Zpl += "0FI0E03EH0780";
                    Zpl += "078H0E01FH0780";
                    Zpl += "078H0EH0F80F,";
                    Zpl += "03CM01F,";
                    Zpl += "01EM03E,";
                    Zpl += "01FM07C,";
                    Zpl += "H0F8L0FC,";
                    Zpl += "H07EK01F8,";
                    Zpl += "H03FK07F,";
                    Zpl += "H01FCJ0FC,";
                    Zpl += "I0HFI07F8,";
                    Zpl += "I03FE03FE,";
                    Zpl += "J0KFC,";
                    Zpl += "J03JF,";
                    Zpl += "K07HF8,";
                    Zpl += ",";
                    Zpl += ":";
                    Zpl += ":";
                    Zpl += ":";
                    Zpl += "0:";
                    break;
                case "JIS":
                    Zpl += "~DGR:JIS_7_1,00528,008,";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000003FF8000000";
                    Zpl += "000001FFFF800000";
                    Zpl += "00000FFFFFF00000";
                    Zpl += "00003FFFFFFC0000";
                    Zpl += "00007FFFFFFE0000";
                    Zpl += "0001FFFFFFFF8000";
                    Zpl += "0003FF0007FFC000";
                    Zpl += "000FFC0000FFE000";
                    Zpl += "001FF800003FF800";
                    Zpl += "003FF000000FFC00";
                    Zpl += "007FE0000007FC00";
                    Zpl += "00FF80000001FE00";
                    Zpl += "00FF00000000FF00";
                    Zpl += "01FE000000007F80";
                    Zpl += "03FC000000003F80";
                    Zpl += "03F8000000001FC0";
                    Zpl += "07F8000000000FC0";
                    Zpl += "0FF03F07E01FCFE0";
                    Zpl += "0FE03F07E03FC7F0";
                    Zpl += "0FE03F07E07FC7F0";
                    Zpl += "1FC03F07E07FC3F0";
                    Zpl += "1FC03F07E0FFC3F0";
                    Zpl += "1F803F07E0FFC1F8";
                    Zpl += "3F803F07E0F801F8";
                    Zpl += "3F803F07E0F801F8";
                    Zpl += "3F003F07E0F801F8";
                    Zpl += "3F003F07E0F800FC";
                    Zpl += "3F003F07E0F800FC";
                    Zpl += "3F003F07E0FE00FC";
                    Zpl += "3F003F07E07F00FC";
                    Zpl += "3F003F07E07F80FC";
                    Zpl += "3F003F07E03FC0FC";
                    Zpl += "3F003F07E007C0FC";
                    Zpl += "3F003F07E003E0FC";
                    Zpl += "3F003F07E003E1FC";
                    Zpl += "3F003F07E003E1F8";
                    Zpl += "1F803F07E007E1F8";
                    Zpl += "1F807F07E007E3F8";
                    Zpl += "1FC07E07E0FFC3F8";
                    Zpl += "0FE0FE07E0FFC3F0";
                    Zpl += "0FFFFC07E0FFC7F0";
                    Zpl += "07FFFC07E0FF8FF0";
                    Zpl += "03FFF807E0FE0FF0";
                    Zpl += "00FFE00000001FE0";
                    Zpl += "003F800000001FC0";
                    Zpl += "0000000000003FC0";
                    Zpl += "0000000000007F80";
                    Zpl += "000000000000FF80";
                    Zpl += "00FF00000001FF00";
                    Zpl += "007F80000003FE00";
                    Zpl += "003FE0000007FC00";
                    Zpl += "003FF000001FFC00";
                    Zpl += "001FFC00003FF800";
                    Zpl += "000FFF0000FFE000";
                    Zpl += "0003FFF03FFFC000";
                    Zpl += "0001FFFFFFFF8000";
                    Zpl += "00007FFFFFFE0000";
                    Zpl += "00003FFFFFFC0000";
                    Zpl += "00000FFFFFF00000";
                    Zpl += "000001FFFF800000";
                    Zpl += "0000001FF8000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    break;
            }
            return Zpl;
        }

        public string Print_Image_KSA(string tmpMARKET)
        {

            var Zpl = "";

            switch (tmpMARKET)
            {
                case "KS":
                    Zpl += "~DGR:KSA,00299,013,";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "01E38FFC3FF0FFE1E383C70000";
                    Zpl += "07F98FFC3FF0FFE7F98FF30000";
                    Zpl += "03F1C00C0CC00E03F787E30000";
                    Zpl += "0739C00C0CC03F873F8E730000";
                    Zpl += "0619800C0CC0F1E6198C330000";
                    Zpl += "0739800C0CC0C0673F8E730000";
                    Zpl += "03F19FFE3FF0FFE3F787E30000";
                    Zpl += "01E19FFE3FF0FFE1E183C30000";
                    Zpl += "000180C0000006000181830000";
                    Zpl += "00018FFC0CC0C6018181830000";
                    Zpl += "01800FFC0CC0C001FF8FFF0000";
                    Zpl += "0180000C0CC0C001818FFF0000";
                    Zpl += "01FF800C7FF8FFE1FF80030000";
                    Zpl += "00FF800C7FF87FE0FF80030000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";

                    Zpl += "0:";
                    break;
                case "26K":
                    Zpl += "~DGR:KSA,00299,013,";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "01E38FFC3FF0FFE1E383C70000";
                    Zpl += "07F98FFC3FF0FFE7F98FF30000";
                    Zpl += "03F1C00C0CC00E03F787E30000";
                    Zpl += "0739C00C0CC03F873F8E730000";
                    Zpl += "0619800C0CC0F1E6198C330000";
                    Zpl += "0739800C0CC0C0673F8E730000";
                    Zpl += "03F19FFE3FF0FFE3F787E30000";
                    Zpl += "01E19FFE3FF0FFE1E183C30000";
                    Zpl += "000180C0000006000181830000";
                    Zpl += "00018FFC0CC0C6018181830000";
                    Zpl += "01800FFC0CC0C001FF8FFF0000";
                    Zpl += "0180000C0CC0C001818FFF0000";
                    Zpl += "01FF800C7FF8FFE1FF80030000";
                    Zpl += "00FF800C7FF87FE0FF80030000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";
                    Zpl += "00000000000000000000000000";

                    Zpl += "0:";
                    break;
            }

            return Zpl;
        }

        public string Print_Image_SP(string tmpMARKET)
        {
            var Zpl = "";

            switch (tmpMARKET)
            {
                case "KS":
                    Zpl += "~DGKI007,01140,012,";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "0000000000FFFE0000000000";
                    Zpl += "000000003FFFFFF800000000";
                    Zpl += "00000001FFFFFFFF00000000";
                    Zpl += "00000003FFFFFFFF80000000";
                    Zpl += "0000000FFFFFFFFFE0000000";
                    Zpl += "0000003FFFFFFFFFF8000000";
                    Zpl += "000000FFFFFFFFFFFE000000";
                    Zpl += "000001FFFFFFFFFFFF000000";
                    Zpl += "000007FFFFFFFFFFFFC00000";
                    Zpl += "00000FFFFFFFFFFFFFE00000";
                    Zpl += "00001FFFFF800FFFFFF00000";
                    Zpl += "00003FFFF800007FFFF80000";
                    Zpl += "00007FFFC0000007FFFC0000";
                    Zpl += "0000FFFE00000000FFFE0000";
                    Zpl += "0001FFFC000000007FFF0000";
                    Zpl += "0003FFF8000000003FFF8000";
                    Zpl += "0007FFE0000000000FFFC000";
                    Zpl += "000FFFC00000000007FFE000";
                    Zpl += "001FFF800000000003FFF000";
                    Zpl += "003FFF000000000000FFF000";
                    Zpl += "003FFE0000000000007FF800";
                    Zpl += "007FFC0000000000007FFC00";
                    Zpl += "007FF80000000000003FFC00";
                    Zpl += "00FFF00000000000001FFE00";
                    Zpl += "01FFE00000000000000FFE00";
                    Zpl += "01FFC00000000000000FFF00";
                    Zpl += "01FFC000000000000007FF00";
                    Zpl += "03FF8000000000000003FF00";
                    Zpl += "03FF0000000000000001FF00";
                    Zpl += "07FF001FF8003FFFC0000000";
                    Zpl += "07FF001FF800FFFF80000000";
                    Zpl += "07FE001FF801FFFF00000000";
                    Zpl += "0FFE001FF803FFFC00000000";
                    Zpl += "0FFE001FF807FFF000000000";
                    Zpl += "0FFC001FF81FFFE000000000";
                    Zpl += "0FFC001FF83FFF8000000000";
                    Zpl += "0FFC001FF87FFF0000000000";
                    Zpl += "1FFC001FF9FFFE0000000000";
                    Zpl += "1FFC001FFFFFFC0000000000";
                    Zpl += "1FFC001FFFFFFC0000000000";
                    Zpl += "1FFFFF1FFFFFFC000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFE000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFE000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFF000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFF800FFFFFF0";
                    Zpl += "1FFFFF1FFFE3FF800FFFFFF0";
                    Zpl += "1FFFFF1FFF81FFC00FFFFFF0";
                    Zpl += "1FFFFF1FFF00FFC00FFFFFF0";
                    Zpl += "1FFFFF1FFC00FFE00FFFFFF0";
                    Zpl += "0000001FF8007FF000007FF0";
                    Zpl += "0000001FF8007FF000007FF0";
                    Zpl += "0000001FF8003FF800007FF0";
                    Zpl += "0000001FF8003FFC00007FE0";
                    Zpl += "0000001FF8001FFC00007FE0";
                    Zpl += "0000001FF8000FFE00007FE0";
                    Zpl += "0000001FF8000FFE0000FFE0";
                    Zpl += "0000001FF80007FF0000FFE0";
                    Zpl += "0000001FF80007FF0000FFE0";
                    Zpl += "0000001FF80003FF8001FFC0";
                    Zpl += "0000001FF80001FF8001FFC0";
                    Zpl += "01FF0000000000000001FFC0";
                    Zpl += "01FF8000000000000003FF80";
                    Zpl += "01FFC000000000000007FF00";
                    Zpl += "01FFE00000000000000FFF00";
                    Zpl += "00FFE00000000000000FFE00";
                    Zpl += "00FFF00000000000001FFE00";
                    Zpl += "007FF80000000000003FFC00";
                    Zpl += "007FFC0000000000007FFC00";
                    Zpl += "003FFE0000000000007FF800";
                    Zpl += "001FFF000000000000FFF000";
                    Zpl += "001FFF800000000003FFF000";
                    Zpl += "0007FFC00000000007FFC000";
                    Zpl += "0003FFE0000000000FFF8000";
                    Zpl += "0001FFF8000000003FFF0000";
                    Zpl += "0001FFFC000000007FFF0000";
                    Zpl += "0000FFFE00000000FFFE0000";
                    Zpl += "00007FFFC0000007FFFC0000";
                    Zpl += "00001FFFFC00003FFFF00000";
                    Zpl += "00000FFFFFE00FFFFFE00000";
                    Zpl += "000007FFFFFFFFFFFFC00000";
                    Zpl += "000007FFFFFFFFFFFF800000";
                    Zpl += "000001FFFFFFFFFFFF000000";
                    Zpl += "000000FFFFFFFFFFFE000000";
                    Zpl += "0000003FFFFFFFFFF8000000";
                    Zpl += "0000000FFFFFFFFFE0000000";
                    Zpl += "00000001FFFFFFFF00000000";
                    Zpl += "00000001FFFFFFFE00000000";
                    Zpl += "000000003FFFFFF800000000";
                    Zpl += "0000000000FFFE0000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "0:";
                    break;
                case "JIS":
                    Zpl += "~DGKI006   ,01501,019,  ";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "000000000000000000000000003FF800000000";
                    Zpl += "00000000000000000000000003FFFF80000000";
                    Zpl += "0000000000000000000000001FFFFFF8000000";
                    Zpl += "000000000000000000000000FFFFFFFF000000";
                    Zpl += "000000000000000000000001FFFFFFFF800000";
                    Zpl += "000000000000000000000007FFFFFFFFE00000";
                    Zpl += "00000000000000000000000FFFC007FFF00000";
                    Zpl += "00000000000000000000001FF800003FFC0000";
                    Zpl += "00000000000000000000007FE000000FFE0000";
                    Zpl += "0000000000000000000000FFC0000003FF0000";
                    Zpl += "0000000000000000000001FF00000000FF8000";
                    Zpl += "0000000000000000000003FE000000007FC000";
                    Zpl += "0000000000000000000007FC000000003FE000";
                    Zpl += "0000000000000000000007F0000000001FF000";
                    Zpl += "000000000000000000000FE0000000000FF800";
                    Zpl += "000000000000000000001FE00000000007F800";
                    Zpl += "000000000000000000003FC00000000003FC00";
                    Zpl += "000000000000000000003F800000000001FE00";
                    Zpl += "000000000000000000007F800000000000FE00";
                    Zpl += "000000000000000000007F00FE0FF007F87F00";
                    Zpl += "0000000000F800000000FE00FE0FF00FF87F00";
                    Zpl += "0000000003FE00030000FE00FE0FF01FF83F00";
                    Zpl += "007F83FE0F0780030001FC00FE0FF03FF83F80";
                    Zpl += "007F80F80F0780030001F800FE0FF03FF81F80";
                    Zpl += "003E00F01F0380038001F800FE0FF07FF81F80";
                    Zpl += "003E01C03E03C0078003F800FE0FF07FF81F80";
                    Zpl += "003E03803E01C007C003F000FE0FF07E001F80";
                    Zpl += "003E03803E01C007C003F000FE0FF07E001FC0";
                    Zpl += "003E0E003F00000FE007F000FE0FF07E001FC0";
                    Zpl += "003E0E003F00000DE007E000FE0FF07E001FC0";
                    Zpl += "003E1C001FC00019F007E000FE0FF07E000FC0";
                    Zpl += "003E38001FE00018F007E000FE0FF07F000FC0";
                    Zpl += "003E70000FF80018F007E000FE0FF03FC00FC0";
                    Zpl += "003FF00003FE0038F807E000FE0FF03FE00FC0";
                    Zpl += "003FF80003FF00307807E000FE0FF01FF00FC0";
                    Zpl += "003FF80001FF80307807E000FE0FF00FF80FC0";
                    Zpl += "003E7C00007FC0607C07E000FE0FF007F80FC0";
                    Zpl += "003E7E00003FC07FFC07E000FE0FF001F80FC0";
                    Zpl += "003E7E00001FE0FFFC07E000FE0FF000FC0FC0";
                    Zpl += "003E1F000007E0FFFC07E000FE0FF000FC1FC0";
                    Zpl += "003E1F800007E0E03E03F000FE0FF000FC1FC0";
                    Zpl += "003E0FC03003E1C03E03F000FE0FF000FC1FC0";
                    Zpl += "003E07C03003E1C03F03F000FE0FF000FC1F80";
                    Zpl += "003E07C03803C1C01F03F801FE0FF001FC3F80";
                    Zpl += "003E03F03C07C3801F01FE03FC0FF07FF83F80";
                    Zpl += "003E03F03E0783801F81FF07FC0FF07FF83F00";
                    Zpl += "003E01F03E0783801F80FFFFF80FF07FF87F00";
                    Zpl += "00FFE1FC0FFC0FE07FE07FFFF00FF07FF0FF00";
                    Zpl += "00FFE1FE03F81FF07FF03FFFE00FF07FE0FE00";
                    Zpl += "000000000000000000001FFFC00FF07F01FE00";
                    Zpl += "000000000000000000000FFF0000000003FC00";
                    Zpl += "0000000000000000000003FC0000000003FC00";
                    Zpl += "0000000000000000000000000000000007F800";
                    Zpl += "000000000000000000000000000000000FF800";
                    Zpl += "000000000000000000000000000000000FF000";
                    Zpl += "000000000000000000000000000000001FE000";
                    Zpl += "0000000000000000000007FC000000003FE000";
                    Zpl += "0000000000000000000007FE00000000FFC000";
                    Zpl += "0000000000000000000003FF00000001FF8000";
                    Zpl += "0000000000000000000001FF80000007FF0000";
                    Zpl += "0000000000000000000000FFE000000FFE0000";
                    Zpl += "00000000000000000000007FF800003FFC0000";
                    Zpl += "00000000000000000000003FFE0000FFF80000";
                    Zpl += "00000000000000000000000FFFE01FFFE00000";
                    Zpl += "000000000000000000000007FFFFFFFFC00000";
                    Zpl += "000000000000000000000001FFFFFFFF000000";
                    Zpl += "0000000000000000000000007FFFFFFC000000";
                    Zpl += "0000000000000000000000001FFFFFF0000000";
                    Zpl += "00000000000000000000000003FFFF80000000";
                    Zpl += "000000000000000000000000001FF000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "0:";

                    Zpl += "~DGR:JIS100,00730,010, ";
                    Zpl += "00000000000000000000";
                    Zpl += "00000000FF8000000000";
                    Zpl += "0000000FFFF800000000";
                    Zpl += "000000FFDDFF80000000";
                    Zpl += "000003BFFFFFE0000000";
                    Zpl += "00000FFFFFFFF8000000";
                    Zpl += "00003FFFFFFFFE000000";
                    Zpl += "0000FFFC003FFF000000";
                    Zpl += "0001FFE00003FF800000";
                    Zpl += "0003FF000000FFC00000";
                    Zpl += "0007FC0000003FE00000";
                    Zpl += "000FF00000001FF00000";
                    Zpl += "001FE000000007F80000";
                    Zpl += "003FC000000003FC0000";
                    Zpl += "007F8000000001FE0000";
                    Zpl += "00FF0000000000FF0000";
                    Zpl += "01FE00000000007F0000";
                    Zpl += "01FC00000000003F8000";
                    Zpl += "03F800000000001FC000";
                    Zpl += "03F800000000001FC000";
                    Zpl += "07F01FF0FF807FCFE000";
                    Zpl += "07E01FF0FF80FFC7E000";
                    Zpl += "0FE01FF0FF81FFC7F000";
                    Zpl += "0FC01FF0FF83FFC3F000";
                    Zpl += "0FC01FF0FF87FFC3F000";
                    Zpl += "1F801FF0FF8FFFC3F800";
                    Zpl += "1F801FF0FF8FFFC1F800";
                    Zpl += "1F801FF0FF8FF001F800";
                    Zpl += "1F001FF0FF8FE001FC00";
                    Zpl += "3F001FF0FF8FE000FC00";
                    Zpl += "3F001FF0FF8FE000FC00";
                    Zpl += "3E001FF0FF8FF000FC00";
                    Zpl += "7E001FF0FF8FF8007C00";
                    Zpl += "7E001FF0FF8FFC007E00";
                    Zpl += "7E001FF0FF87FE007E00";
                    Zpl += "7E001FF0FF83FF007E00";
                    Zpl += "7E001FF0FF81FF807E00";
                    Zpl += "7E001FF0FF80FFC07E00";
                    Zpl += "7E001FF0FF807FE07E00";
                    Zpl += "7E001FF0FF803FE07E00";
                    Zpl += "3F001FF0FF801FE07C00";
                    Zpl += "3F001FF0FF800FE0FC00";
                    Zpl += "3F001BF0FF8007E0FC00";
                    Zpl += "3F801FE0FF8007E0FC00";
                    Zpl += "1FC07FE0FF8007E0FC00";
                    Zpl += "1FE0FFC0FF800FE1FC00";
                    Zpl += "0FFFFF80FF8FFFE1F800";
                    Zpl += "0FFFFF80FF8FFFE1F800";
                    Zpl += "07FFFF00FF8FFFC3F800";
                    Zpl += "03FFFE00FF8FFF83F000";
                    Zpl += "01FFFC00FF8FFF07F000";
                    Zpl += "007FF000FF8FFE0FE000";
                    Zpl += "001FE000FF8FFC0FE000";
                    Zpl += "000000000000001FC000";
                    Zpl += "000000000000003FC000";
                    Zpl += "000000000000007F8000";
                    Zpl += "00000000000000FF0000";
                    Zpl += "00038000000001FF0000";
                    Zpl += "0007C000000003FE0000";
                    Zpl += "000FE000000007FC0000";
                    Zpl += "000FF00000000FF80000";
                    Zpl += "000FFC0000001FF00000";
                    Zpl += "0007FE0000003FE00000";
                    Zpl += "0003FF000000FFC00000";
                    Zpl += "0001FFE00003FF800000";
                    Zpl += "00007FFC003FFF000000";
                    Zpl += "00003FFFFFFFFC000000";
                    Zpl += "00000FFFFFFFF8000000";
                    Zpl += "000003FFFFFFE0000000";
                    Zpl += "0000007FFFFF00000000";
                    Zpl += "0000000FFFF000000000";
                    Zpl += "00000000FF8000000000";
                    Zpl += "00000000000000000000";
                    Zpl += "0:";


                    break;
            }

            Zpl += "~DGPI015,00028,002,";
            Zpl += "0010";
            Zpl += "0FB8";
            Zpl += "1FF0";
            Zpl += "3860";
            Zpl += "70F0";
            Zpl += "61B0";
            Zpl += "6330";
            Zpl += "6630";
            Zpl += "6C30";
            Zpl += "7870";
            Zpl += "30E0";
            Zpl += "7FC0";
            Zpl += "EF80";
            Zpl += "4000";
            Zpl += "0:";

            Zpl += "~DGPI060,00185,005,";
            Zpl += "0000000000";
            Zpl += "0000000100";
            Zpl += "0000000380";
            Zpl += "0007FE07C0";
            Zpl += "003FFFCF80";
            Zpl += "007FFFFF00";
            Zpl += "00FFFFFE00";
            Zpl += "01FE07FC00";
            Zpl += "03F0017C00";
            Zpl += "07E0007C00";
            Zpl += "0FC001FE00";
            Zpl += "0F8003DE00";
            Zpl += "1F8007DF00";
            Zpl += "1F000F9F00";
            Zpl += "1F001F1F00";
            Zpl += "1F003E1F80";
            Zpl += "3F007C1F80";
            Zpl += "3E00F80F80";
            Zpl += "3E01F00F80";
            Zpl += "3E03E00F80";
            Zpl += "3E07C00F80";
            Zpl += "3F0F801F80";
            Zpl += "1F1F001F00";
            Zpl += "1F3E001F00";
            Zpl += "1F7C001F00";
            Zpl += "1FF8003E00";
            Zpl += "0FF0003E00";
            Zpl += "0FE0007C00";
            Zpl += "07F000FC00";
            Zpl += "07FE07F800";
            Zpl += "0FFFFFF000";
            Zpl += "1FFFFFE000";
            Zpl += "3E7FFF8000";
            Zpl += "7C07FE0000";
            Zpl += "3800000000";
            Zpl += "1000000000";
            Zpl += "0000000000";
            Zpl += "0:";

            Zpl += "~DGPI100,00512,008,";
            Zpl += "0000000000000000";
            Zpl += "0000000000000000";
            Zpl += "0000000000000300";
            Zpl += "0000000000000780";
            Zpl += "0000001F80000FC0";
            Zpl += "000000FFF0001FE0";
            Zpl += "000007FFFE003FE0";
            Zpl += "00001BFFFFE07FC0";
            Zpl += "00007FFFFFF8FF80";
            Zpl += "0001FFFFFFF9FF00";
            Zpl += "0003FFFFFFFFFE00";
            Zpl += "0007FFFFFFFFFC00";
            Zpl += "000FFFE07FFFF800";
            Zpl += "001FFE0007FFF000";
            Zpl += "003FF80001FFE000";
            Zpl += "007FF00000FFF000";
            Zpl += "00FFE000007FF000";
            Zpl += "00FFC00000FFF000";
            Zpl += "01FF800001FFF800";
            Zpl += "01FF000003FFF800";
            Zpl += "03FF000007FFFC00";
            Zpl += "03FE00000FF7FC00";
            Zpl += "03FE00001FE7FC00";
            Zpl += "07FC00003FC7FE00";
            Zpl += "07FC00007F83FE00";
            Zpl += "07FC0000FF03FE00";
            Zpl += "07F80001FE03FE00";
            Zpl += "0FF80003FC03FF00";
            Zpl += "0FF00007F801FF00";
            Zpl += "0FF0000FF001FF00";
            Zpl += "0FF0000FE001FF00";
            Zpl += "0FF0001FC001FF00";
            Zpl += "0FF0003F8001FF00";
            Zpl += "0FF0007F0001FF00";
            Zpl += "0FF000FF0001FF00";
            Zpl += "0FF001FE0001FF00";
            Zpl += "0FF803FC0003FF00";
            Zpl += "07F807F80003FE00";
            Zpl += "07FC0FF00003FE00";
            Zpl += "07FC1FE00003FE00";
            Zpl += "07FC3FC00003FE00";
            Zpl += "07FE7F800007FC00";
            Zpl += "03FEFF000007FC00";
            Zpl += "03FFFE000007FC00";
            Zpl += "03FFFC00000FF800";
            Zpl += "01FFF800000FF800";
            Zpl += "01FFF000001FF000";
            Zpl += "00FFE000003FF000";
            Zpl += "003FF000007FE000";
            Zpl += "003FF80000FFE000";
            Zpl += "007FFC0003FFC000";
            Zpl += "00FFFF801FFF8000";
            Zpl += "01FFFFE07FFF0000";
            Zpl += "03FFFFFFFFFE0000";
            Zpl += "07FFFFFFFFFC0000";
            Zpl += "0FF8FFFFFFF00000";
            Zpl += "1FF07FFFFFE00000";
            Zpl += "3FE00FFFFF000000";
            Zpl += "3FC001FFF8000000";
            Zpl += "1F80001F80000000";
            Zpl += "0F00000000000000";
            Zpl += "0600000000000000";
            Zpl += "0000000000000000";
            Zpl += "0000000000000000";
            Zpl += "0:";

            return Zpl;
        }

        public string Print_Image_SP25(string tmpMARKET)
        {
            var Zpl = "";

            //	switch(tmpMARKET){
            //		case "JIS":
            //
            //			break;
            //	}

            Zpl += "~DGKI025,00190,005,";
            Zpl += "0000000000";
            Zpl += "0000000000";
            Zpl += "0003FF0000";
            Zpl += "001FFF8000";
            Zpl += "007E07E000";
            Zpl += "00F800F800";
            Zpl += "01E0003C00";
            Zpl += "0380001E00";
            Zpl += "0380000E00";
            Zpl += "0F00000700";
            Zpl += "1E79E0F780";
            Zpl += "1E79E1F380";
            Zpl += "1C79E3F3C0";
            Zpl += "3C79E783C0";
            Zpl += "3879E703C0";
            Zpl += "3879E701C0";
            Zpl += "3879E781C0";
            Zpl += "3879E7C1C0";
            Zpl += "3879E3E1C0";
            Zpl += "3879E1F1C0";
            Zpl += "3879E0F1C0";
            Zpl += "3879E071C0";
            Zpl += "3C79E073C0";
            Zpl += "3E79E0F3C0";
            Zpl += "0FF1E7F380";
            Zpl += "07C1E7E780";
            Zpl += "0181E7C700";
            Zpl += "0000000F00";
            Zpl += "0300001E00";
            Zpl += "0380001C00";
            Zpl += "01C0007800";
            Zpl += "00F803F000";
            Zpl += "007FFFE000";
            Zpl += "003FFFC000";
            Zpl += "0007FC0000";
            Zpl += "0003F80000";
            Zpl += "0000000000";
            Zpl += "0000000000";
            Zpl += "0:";

            Zpl += "~DGPI025,00060,003,";
            Zpl += "000000";
            Zpl += "01F0C0";
            Zpl += "07FDC0";
            Zpl += "0FFF80";
            Zpl += "1E1F00";
            Zpl += "3C0F80";
            Zpl += "381F80";
            Zpl += "783FC0";
            Zpl += "7079C0";
            Zpl += "70F1C0";
            Zpl += "71E1C0";
            Zpl += "73C1C0";
            Zpl += "7F83C0";
            Zpl += "3F0380";
            Zpl += "3E0780";
            Zpl += "1E0F00";
            Zpl += "3FFE00";
            Zpl += "77FC00";
            Zpl += "61F000";
            Zpl += "000000";
            Zpl += "0:";

            return Zpl;
        }

        public string  Print_Image_SP40(string tmpMARKET)
        {
            var Zpl = "";

            //	switch(tmpMARKET){
            //		case "JIS":
            //
            //			break;
            //	}

            Zpl += "~DGKI040,00190,005,0000000000";
            Zpl += "0000000000";
            Zpl += "0003FF0000";
            Zpl += "001FFF8000";
            Zpl += "007E07E000";
            Zpl += "00F800F800";
            Zpl += "01E0003C00";
            Zpl += "0380001E00";
            Zpl += "0380000E00";
            Zpl += "0F00000700";
            Zpl += "1E79E0F780";
            Zpl += "1E79E1F380";
            Zpl += "1C79E3F3C0";
            Zpl += "3C79E783C0";
            Zpl += "3879E703C0";
            Zpl += "3879E701C0";
            Zpl += "3879E781C0";
            Zpl += "3879E7C1C0";
            Zpl += "3879E3E1C0";
            Zpl += "3879E1F1C0";
            Zpl += "3879E0F1C0";
            Zpl += "3879E071C0";
            Zpl += "3C79E073C0";
            Zpl += "3E79E0F3C0";
            Zpl += "0FF1E7F380";
            Zpl += "07C1E7E780";
            Zpl += "0181E7C700";
            Zpl += "0000000F00";
            Zpl += "0300001E00";
            Zpl += "0380001C00";
            Zpl += "01C0007800";
            Zpl += "00F803F000";
            Zpl += "007FFFE000";
            Zpl += "003FFFC000";
            Zpl += "0007FC0000";
            Zpl += "0003F80000";
            Zpl += "0000000000";
            Zpl += "0000000000";
            Zpl += "0:";

            Zpl += "~DGPI040,00116,004,00000000";
            Zpl += "00000000";
            Zpl += "00000000";
            Zpl += "007F8300";
            Zpl += "01FFE780";
            Zpl += "03FFFF80";
            Zpl += "07F1FF00";
            Zpl += "0FC03E00";
            Zpl += "1F007F00";
            Zpl += "1E00FF00";
            Zpl += "3E01FF80";
            Zpl += "3E03EF80";
            Zpl += "3C07C780";
            Zpl += "3C0F8780";
            Zpl += "3C1F0780";
            Zpl += "3C3E0780";
            Zpl += "3C7C0780";
            Zpl += "3EF80780";
            Zpl += "3FF00F80";
            Zpl += "1FE00F00";
            Zpl += "1FC01F00";
            Zpl += "0F801F00";
            Zpl += "0FC07E00";
            Zpl += "1FF1FC00";
            Zpl += "3FFFF800";
            Zpl += "3DFFF000";
            Zpl += "187FC000";
            Zpl += "00000000";
            Zpl += "00000000";
            Zpl += "0:";

            return Zpl;
        }

        //1. H MARK 이미지 출력
        public string  Print_Image_H()
        {

            var Zpl = "~DGR:UK_H11,01554,014,";
            Zpl += "0000000000000000000000000000";
            Zpl += "0000000000000000000000000000";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE000000000000FFFFFFFC";
            Zpl += "3FFFFFFE00007FFC0000FFFFFFFC";
            Zpl += "3FFFFFFE0007FFFFC000FFFFFFFC";
            Zpl += "3FFFFFFE003FFFFFF800FFFFFFFC";
            Zpl += "3FFFFFFE00FFFFFFFE00FFFFFFFC";
            Zpl += "3FFFFFFE03FFFFFFFF80FFFFFFFC";
            Zpl += "3FFFFFFE0FFFFFFFFFE0FFFFFFFC";
            Zpl += "3FFFFFFE3FFFFFFFFFF0FFFFFFFC";
            Zpl += "3FFFFFFE7FFFFFFFFFF8FFFFFFFC";
            Zpl += "3FFFFFFEFFFFFFFFFFFEFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFF81FFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFE00007FFFFFFFFFFC";
            Zpl += "3FFFFFFFFFF000000FFFFFFFFFFC";
            Zpl += "3FFFFFFFFFC0000003FFFFFFFFFC";
            Zpl += "3FFFFFFFFF00000000FFFFFFFFFC";
            Zpl += "3FFFFFFFFE000000007FFFFFFFFC";
            Zpl += "3FFFFFFFFC000000003FFFFFFFFC";
            Zpl += "3FFFFFFFF8000000001FFFFFFFFC";
            Zpl += "3FFFFFFFF0000000000FFFFFFFFC";
            Zpl += "3FFFFFFFE00000000007FFFFFFFC";
            Zpl += "3FFFFFFFC00000000003FFFFFFFC";
            Zpl += "3FFFFFFF800000000001FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "3FFFFFFF000000000000FFFFFFFC";
            Zpl += "0000000000000000000000000000";
            Zpl += "0000000000000000000000000000";
            Zpl += "0000000000000000000000000000";
            Zpl += "0:";

            return Zpl;
        }

        public string  Print_Image_CARES()
        {

            var Zpl = "~DGR:UK_C12,02618,017,";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "00000000000000007C0000000000000000";
            Zpl += "0000000000000001FF8000000000000000";
            Zpl += "0000000000000007FFC000000000000000";
            Zpl += "000000000000000F81F000000000000000";
            Zpl += "000000000000001E1C7800000000000000";
            Zpl += "000000000000003CFF3C00000000000000";
            Zpl += "0000000000000079FF9E00000000000000";
            Zpl += "0000000000000073FFCF00000000000000";
            Zpl += "00000000000000E7FFE780000000000000";
            Zpl += "00000000000000EFFFF380000000000000";
            Zpl += "00000000000003CFFFFBC0000000000000";
            Zpl += "000000000000039FFFFCE0000000000000";
            Zpl += "000000000000073FFFFE60000000000000";
            Zpl += "0000000000000F7FFFFF30000000000000";
            Zpl += "0000000000001EFFFFFFB8000000000000";
            Zpl += "0000000000007CFFFFFF98000000000000";
            Zpl += "00000003F000F9FFFFFFCC000000000000";
            Zpl += "0000003FFFFFF3FFFFFFEF80FFF8000000";
            Zpl += "000000FFFFFFC7FFFFFFE7FFFFFE000000";
            Zpl += "000001F801FF1FFFFFFFF3FFC03F000000";
            Zpl += "000003F000003FFFFFFFF9FE0F0FC00000";
            Zpl += "000007C7F800FFFE007FFC00FFC3C00000";
            Zpl += "00000F9FFFFFFF800001FE01FFF3E00000";
            Zpl += "00000F3FFFFFF801FF803FFFFFF8E00000";
            Zpl += "00001E7FFFFFC07FFFFE03FFFFFC700000";
            Zpl += "00001CFFFFFF83FFFFFFC1FFFFFE700000";
            Zpl += "00003CFFFFFE1FFFFFFFF87FFFFF700000";
            Zpl += "00003DFFFFF87FFFFFFFFE1FFFFF300000";
            Zpl += "000039FFFFE1FFFFFFFFFF87FFFF380000";
            Zpl += "000039FFFFC7FFFFFFFFFFE3FFFF380000";
            Zpl += "000039FFFF1FFFFFFFFFFFF8FFFF380000";
            Zpl += "00003DFFFE3FFFFFFFFFFFFC7FFF380000";
            Zpl += "00001DFFFC7FFFFFFFFFFFFE3FFF380000";
            Zpl += "00001DFFF9FFFFFFFFFFFFFF9FFF380000";
            Zpl += "00001CFFF3FFFFFFFFFFFFFFCFFF380000";
            Zpl += "00000CFFE7FFFFFFFFFFFFFFE7FF300000";
            Zpl += "00000EFFCFFFFFFFFFFFFFFFF3FF300000";
            Zpl += "00000EFF9FFFFFFFFFFFFFFFF9FE700000";
            Zpl += "00000E7F3FFFFFFFFFFFFFFFFCFE700000";
            Zpl += "00000E7E3FFFFFFFFFFFFFFFFC7E700000";
            Zpl += "00000E7C7FFFFFFFFFFFFFFFFE3E700000";
            Zpl += "00000E7CFFFFFFFFFFFFFFFFFF3E700000";
            Zpl += "00000E79FFFFFFFFFFFFFFFFFF9E700000";
            Zpl += "00000E79FFFFFFFFFFFFFFFFFF9E700000";
            Zpl += "00000E73FFFFFFFFFFFFFFFFFFCE700000";
            Zpl += "00001C63FFFFFFFFFFFFFFFFFFC6700000";
            Zpl += "00003CE7FFFFFFFFFFFFFFFFFFE7700000";
            Zpl += "000078CFFFFFFFFFFFFFFFFFFFF3300000";
            Zpl += "0001F9C000000000000000000003B80000";
            Zpl += "0003F180000000000000000000019C0000";
            Zpl += "0007E781FF00FFC1FFE0FFF07FC1CF0000";
            Zpl += "000F8F83FF80FFC1FFF0FFF0FFE1EF8000";
            Zpl += "001F0F07FFC0FFC1FFF8FFF1FFF0F38000";
            Zpl += "003C7F0FFFE1FFE1FFFCFFF3FFF8F9E000";
            Zpl += "0078FF1FFFE1FFE1FFFCFFF3FFF8FCF000";
            Zpl += "00F3FF1FFFE1FFE1FFFCFFF3FFF8FE7C00";
            Zpl += "01E7FE1FC7E1FFE1F8FCFFF3F9F87F3C00";
            Zpl += "03CFFE1FC7E1FFE1F8FCFFF3F9F87F9F00";
            Zpl += "038FFE1FC7E1FFE1F8FCFF03F9F87FCF00";
            Zpl += "039FFE1FC7E1FFE1F8FCFF03F9F87FE780";
            Zpl += "079FFE1FC7E3FFE1F8FCFF03F9F87FF3C0";
            Zpl += "071FFE1FC7E3F7E1F8FCFF03FC007FF9C0";
            Zpl += "0F3FFC1FC7E3F7E1F8FCFFF3FE003FFDC0";
            Zpl += "0E3FFC1FC7E3F7E1FDFCFFF3FF003FFDC0";
            Zpl += "0E7FFC1FC7E3F7E1FFF8FFF1FF003FFEC0";
            Zpl += "0E7FFC1FC7E3F7E1FFF0FFF0FFC03FFEE0";
            Zpl += "0E7FFC1FC003F3E1FFF0FFF07FE03FFEE0";
            Zpl += "0E7FFC1FC003F3F1FFF8FFF03FF03FFEE0";
            Zpl += "073FFC1FC003F3F1FDFCFFF01FF83FFDC0";
            Zpl += "073FFC1FC003F3F1F8FCFFF00FF83FFDC0";
            Zpl += "079FFC1FC7E3F3F1F8FCFF0007F83FFDC0";
            Zpl += "039FFE1FC7E3F3F1F8FCFF0003F87FF980";
            Zpl += "03DFFE1FC7E3FFF1F8FCFF03F3F87FF380";
            Zpl += "01CFFE1FC7E7FFF1F8FCFF03F3F87FE700";
            Zpl += "01E7FE1FC7E7FFF9F8FCFF03F3F87FDF00";
            Zpl += "00F3FE1FC7E7FFF9F8FCFF03F3F87FBC00";
            Zpl += "0079FE1FC7E7FFF9F8FCFFF3F3F87F7C00";
            Zpl += "003CFF1FC7E7F7F9F8FCFFF3F3F8FE7000";
            Zpl += "001E3F1FEFE7F3F9F8FCFFF3F3F8FCE000";
            Zpl += "000F9F1FFFE7F3F9F8FCFFF3FFF8F9E000";
            Zpl += "0003CF0FFFEFF3FDF8FCFFF1FFF8F38000";
            Zpl += "0001E78FFFCFF3FDF8FCFFF1FFF1E78000";
            Zpl += "0000F387FF8FF3FDF8FCFFF0FFE1CE0000";
            Zpl += "00007981FF0FF3FDF8FCFFF07FC1DE0000";
            Zpl += "000039C0000000000000000000039C0000";
            Zpl += "000039C000000000000000000003B80000";
            Zpl += "00003DE7FFFFFFFFFFFFFFFFFFE7380000";
            Zpl += "00001CE3FFFFFFFFFFFFFFFFFFC7700000";
            Zpl += "00000E73FFFFFFFFFFFFFFFFFFCF700000";
            Zpl += "00000E79FFFFFFFFFFFFFFFFFF9E700000";
            Zpl += "00000E79FFFFFFFFFFFFFFFFFF9E700000";
            Zpl += "00000E7CFFFFFFFFFFFFFFFFFF3E700000";
            Zpl += "00000E7C7FFFFFFFFFFFFFFFFE3E700000";
            Zpl += "00000E7E3FFFFFFFFFFFFFFFFC7E700000";
            Zpl += "00000E7F3FFFFFFFFFFFFFFFFCFE700000";
            Zpl += "00000E7F9FFFFFFFFFFFFFFFF9FE700000";
            Zpl += "00000E7FCFFFFFFFFFFFFFFFF3FE700000";
            Zpl += "00000E7FE7FFFFFFFFFFFFFFE7FE700000";
            Zpl += "00000E7FF3FFFFFFFFFFFFFFCFFE700000";
            Zpl += "00000E7FF9FFFFFFFFFFFFFF9FFE700000";
            Zpl += "00000E7FFC7FFFFFFFFFFFFE3FFE700000";
            Zpl += "00000E7FFE3FFFFFFFFFFFFC7FFE700000";
            Zpl += "00000E7FFF1FFFFFFFFFFFF8FFFE700000";
            Zpl += "00000E7FFFC7FFFFFFFFFFE3FFFE700000";
            Zpl += "00000E7FFFE1FFFFFFFFFF87FFFE700000";
            Zpl += "00000E7FFFF87FFFFFFFFE1FFFFE700000";
            Zpl += "00000E7FFFFE1FFFFFFFF87FFFFE700000";
            Zpl += "00000F7FFFFF83FFFFFFC1FFFFFCE00000";
            Zpl += "0000073FFFFFC07FFFFE03FFFFF9E00000";
            Zpl += "0000079FFFFFFC01FF803FFFFFF1E00000";
            Zpl += "000003C00000FF800001FFFFFFE3C00000";
            Zpl += "000001F000003FFE007FFE000007C00000";
            Zpl += "000000FFFFFF9FFFFFFFF800001F800000";
            Zpl += "0000003FFFFFC7FFFFFFF3FFFFFE000000";
            Zpl += "0000001FFFFFF3FFFFFFE7FFFFFE000000";
            Zpl += "00000000000079FFFFFFCF801FE0000000";
            Zpl += "0000000000003DFFFFFF9E000000000000";
            Zpl += "0000000000001CFFFFFFBC000000000000";
            Zpl += "0000000000001E7FFFFF38000000000000";
            Zpl += "0000000000000F3FFFFE70000000000000";
            Zpl += "00000000000007BFFFFCE0000000000000";
            Zpl += "000000000000079FFFF9C0000000000000";
            Zpl += "00000000000001CFFFF380000000000000";
            Zpl += "00000000000001E7FFE700000000000000";
            Zpl += "00000000000000F3FFCE00000000000000";
            Zpl += "0000000000000079FF1C00000000000000";
            Zpl += "000000000000007C007800000000000000";
            Zpl += "000000000000001E00F000000000000000";
            Zpl += "000000000000000FFFE000000000000000";
            Zpl += "0000000000000001FF0000000000000000";
            Zpl += "0000000000000000FE0000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000001F8FF8FC7FC3E00F000000000";
            Zpl += "0000000003FCFF9FE7FC7F01F000000000";
            Zpl += "0000000007FEFFBFF7FCFF83F000000000";
            Zpl += "00000000070E07B8703DE3C7F000000000";
            Zpl += "00000000070E07B8703DC1CFF000000000";
            Zpl += "00000000070E0FB8707DC1DEF000000000";
            Zpl += "00000000070E0F387079C1FC7000000000";
            Zpl += "00000000070E1F3870F9C1FEF800000000";
            Zpl += "00000000070E1E3870F1C1FFFC00000000";
            Zpl += "0000000007FE3C3FF1E0FFBFFC00000000";
            Zpl += "0000000003FC7C1FE3E0FF00F000000000";
            Zpl += "0000000001F8780FC3C03E00F000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += "0000000000000000000000000000000000";
            Zpl += ":";
            Zpl += "0:";

            return Zpl;
        }

        public string  Print_Image_UKAS()
        {
            var Zpl = "";

            Zpl += "~DGR:UK_C22,01276,011,";
            Zpl += "0000000000000000000000";
            Zpl += "0000000000000000000000";
            Zpl += "0000000000000000000000";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC";
            Zpl += "380000000000000000001C";
            Zpl += "380000000000000000001C";
            Zpl += "380000000000000000001C";
            Zpl += "380000000020000000001C";
            Zpl += "380000000070000000001C";
            Zpl += "3800000001FC000000001C";
            Zpl += "3800000001FC000000001C";
            Zpl += "3800000000F8000000001C";
            Zpl += "380000000070000000001C";
            Zpl += "3800000000F8000000001C";
            Zpl += "38000001F8F8FC0000001C";
            Zpl += "38000003FFFFFE0000001C";
            Zpl += "3800000FFFFFFF8000001C";
            Zpl += "3800001FCFFFCFC000001C";
            Zpl += "3800003F83FF07E000001C";
            Zpl += "3800003F00FC03E000001C";
            Zpl += "3800003F00FC03E000001C";
            Zpl += "3800003F00FC03E000001C";
            Zpl += "3800003E00FC01E040001C";
            Zpl += "3800003E30FC71E0C0001C";
            Zpl += "3800001E39FCF1C1C0001C";
            Zpl += "3800000F3FFFF3C3C0001C";
            Zpl += "38000007FFFFFF83C0001C";
            Zpl += "38000007FFFFFF07C0001C";
            Zpl += "38000007EDFEDF0FC0001C";
            Zpl += "380000038CFCC70FC0001C";
            Zpl += "38000003BFFFF71FC0001C";
            Zpl += "38000001F3DE7C1FC0001C";
            Zpl += "38000001FFDFFC3FC0001C";
            Zpl += "38000000FFFFFC7FC0001C";
            Zpl += "38000000FFFFFC7FC0001C";
            Zpl += "38000000E00039FFC0001C";
            Zpl += "38000000000001FFC0001C";
            Zpl += "38000000000003FFC0001C";
            Zpl += "38000000000003FFC0001C";
            Zpl += "38000000000007FF80001C";
            Zpl += "3800000400000FFF00001C";
            Zpl += "3800000600000FFE00001C";
            Zpl += "3800000700001FFE00001C";
            Zpl += "3800000780001FFC00001C";
            Zpl += "38000007C0007FFC00001C";
            Zpl += "38000007E0007FF800001C";
            Zpl += "38000007F000FFF800001C";
            Zpl += "38000007F801FFF000001C";
            Zpl += "38000007FC01FFE000001C";
            Zpl += "38000007FE03FFE000001C";
            Zpl += "38000007FF03FFC000001C";
            Zpl += "38000007FF8FFF8000001C";
            Zpl += "38000007FFCFFF0000001C";
            Zpl += "38000007FFFFFF0000001C";
            Zpl += "38000007FFFFFE0000001C";
            Zpl += "38000007FFFFFC0000001C";
            Zpl += "38000007FFFFFC0000001C";
            Zpl += "38000007FFFFF80000001C";
            Zpl += "38000007FFFFF00000001C";
            Zpl += "38000007FFFFF00000001C";
            Zpl += "38000003FFFFE00000001C";
            Zpl += "38000001FFFFC00000001C";
            Zpl += "38000000FFFF800000001C";
            Zpl += "38000000FFFF800000001C";
            Zpl += "380000007FFF000000001C";
            Zpl += "380000003FFE000000001C";
            Zpl += "380000001FFE000000001C";
            Zpl += "380000000FFC000000001C";
            Zpl += "3800000007FC000000001C";
            Zpl += "3800000003F8000000001C";
            Zpl += "3800000001F0000000001C";
            Zpl += "3800000000F0000000001C";
            Zpl += "380000000060000000001C";
            Zpl += "380000000000000000001C";
            Zpl += "380000000000000000001C";
            Zpl += "38001C1C38E01003F0001C";
            Zpl += "38001C1C39E03807F8001C";
            Zpl += "38001C1C3BE0380FFC001C";
            Zpl += "38001C1C3FC07C0F1C001C";
            Zpl += "38001C1C3F807C0F00001C";
            Zpl += "38001C1C3F00FE0FC0001C";
            Zpl += "38001C1C3E00EE07E0001C";
            Zpl += "38001C1C3F01EF01F8001C";
            Zpl += "38001C1C3F81FF007C001C";
            Zpl += "38001E3C3FC3FF9C3C001C";
            Zpl += "38001FFC3BE3879F1C001C";
            Zpl += "38000FF839E787DFFC001C";
            Zpl += "380007F038E783CFF8001C";
            Zpl += "3800000000000003E0001C";
            Zpl += "380000000000000000001C";
            Zpl += "380000000000000000001C";
            Zpl += "380003CE3CF3339E00001C";
            Zpl += "380003DF7EFB37DE00001C";
            Zpl += "3800025B66DB340C00001C";
            Zpl += "380003DF66DB340C00001C";
            Zpl += "3800021E66DB340C00001C";
            Zpl += "3800021B7EFBF7CC00001C";
            Zpl += "3800021B3CF1E38C00001C";
            Zpl += "380000000000000000001C";
            Zpl += "380E7BDEDECE11EC73201C";
            Zpl += "381F7BDEDEDF11ECFB201C";
            Zpl += "381B624CD8DB38CDDBA01C";
            Zpl += "381073CCDCD028CD9BA01C";
            Zpl += "381073CCDCD07CCD9BE01C";
            Zpl += "381B624CD8DB6ECDDB601C";
            Zpl += "381F7A4CD8DFC6CCFB601C";
            Zpl += "380E7A4CD8CEC6CC73201C";
            Zpl += "380000000000000000001C";
            Zpl += "380000000000000000001C";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC";
            Zpl += "0000000000000000000000";
            Zpl += "0000000000000000000000";

            Zpl += "0:";

            return Zpl;
        }

        //--말레시아(SD400S 관련) SIRIM 마크 : 2013.09.17, SHARP(김정호, 조응용)  --//
        public string  Print_Image_SIRIM()
        {
            var Zpl = "";

            Zpl += "~DGR:R6_SIRIM,02496,016,";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000040000000000000000";
            Zpl += "000000000000001F0000000000000000";
            Zpl += "000000000000003F8000000000000000";
            Zpl += "00000000000000FFE000000000000000";
            Zpl += "00000000000003FFF800000000000000";
            Zpl += "00000000000007FFFC00000000000000";
            Zpl += "0000000000001FFFFF00000000000000";
            Zpl += "0000000000003FFFFF80000000000000";
            Zpl += "000000000000FFFFFFE0000000000000";
            Zpl += "000000000003FFFFFFF0000000000000";
            Zpl += "000000000007E1FFF1FC000000000000";
            Zpl += "00000000001FFFFFFFFE000000000000";
            Zpl += "00000000003FFFFFFFFF800000000000";
            Zpl += "0000000000FC0FFFFC07E00000000000";
            Zpl += "0000000003FFFFFFFFFFF00000000000";
            Zpl += "0000000007FFFFFFFFFFFC0000000000";
            Zpl += "000000001F00FFFFFFE00E0000000000";
            Zpl += "000000003FFFFFFFFFFFFF8000000000";
            Zpl += "00000000FFFFFFFFFFFFFFC000000000";
            Zpl += "00000003E00FFFFFFFFC00F000000000";
            Zpl += "00000007FFFFFFFFFFFFFFF800000000";
            Zpl += "0000001FFFFFFFFFFFFFFFFE00000000";
            Zpl += "0000003800FFFFFFFFFFE00780000000";
            Zpl += "000000FFFFFFFFFFFFFFFFFFC0000000";
            Zpl += "000003FFFFFFFFFFFFFFFFFFF0000000";
            Zpl += "000007FFFFFFFFFFFFFFFFFFF8000000";
            Zpl += "00001F0000000000000000003E000000";
            Zpl += "00003C0000000000000000001F000000";
            Zpl += "0000700000000000000000000F000000";
            Zpl += "0000E007FE0001FFC07FE00003800000";
            Zpl += "0001C007FF0003FFC3FFFC0000E00000";
            Zpl += "00038007FF0003FFC7FFFE0000700000";
            Zpl += "00070007FF0003FFCFFFFF8000380000";
            Zpl += "000E0007FF0003FFDFFFFFC0001C0000";
            Zpl += "001C0007FF8007FFFFFFFFE0000E0000";
            Zpl += "00380007FF8007FFFFFFFFE0000F0000";
            Zpl += "00300007FF8007FFFFFFFFF000070000";
            Zpl += "00600007FF8007FFFFE01FF800038000";
            Zpl += "00E00007FFC00FFFFF800FF80001C000";
            Zpl += "00C00007FFC00FFFFF000FF80000C000";
            Zpl += "01C00007FFC00FFFFF000FF80000C000";
            Zpl += "01800007FFF01FFFFF000FF800006000";
            Zpl += "03800007FFF01FFFFF80000000006000";
            Zpl += "03000007FFF01FFFFFE0000000007000";
            Zpl += "07000007FFF03FFFFFFF000000003000";
            Zpl += "07000007FFF83FFFFFFFF00000003800";
            Zpl += "06000007FFF83FFFFFFFFE0000001800";
            Zpl += "06000007FFF83FFFFFFFFF8000001800";
            Zpl += "06000007FFFC7FFFDFFFFFE000001800";
            Zpl += "06000007FFFC7FFFCFFFFFF000003800";
            Zpl += "07000007FFFC7FFFC3FFFFF800003800";
            Zpl += "07000007FBFEFFBFC0FFFFF800003000";
            Zpl += "03000007FBFEFFBFC07FFFFC00007000";
            Zpl += "03800007F9FFFF3FC007FFFC00007000";
            Zpl += "01800007F9FFFF3FC00007FC00006000";
            Zpl += "01C00007F8FFFF3FFF8007FC00006000";
            Zpl += "00E00007F8FFFE3FFF8003FC0000E000";
            Zpl += "00E00007F8FFFE3FFFC003FC0000C000";
            Zpl += "00F00007F87FFC3FFFE007FC0001C000";
            Zpl += "00700007F87FFC3FFFF01FFC00018000";
            Zpl += "00380007F83FF83FFFFFFFF800038000";
            Zpl += "00380007F83FF83FFFFFFFF800070000";
            Zpl += "001C0007F81FF03FFFFFFFF000070000";
            Zpl += "001C0007F81FF03FDFFFFFE0000E0000";
            Zpl += "000E0007F81FE03FCFFFFFC0001C0000";
            Zpl += "00070007F80FE03FC7FFFF8000180000";
            Zpl += "00078007F80FE03FC0FFFE0000380000";
            Zpl += "0003800000000000003FF00000300000";
            Zpl += "0001C000000000000000000000600000";
            Zpl += "0001E000000000000000000000E00000";
            Zpl += "0000F000000000000000000001C00000";
            Zpl += "00007800000000000000000003800000";
            Zpl += "00003FFFFFFFFFFFFFFFFFFFFF000000";
            Zpl += "00001FFFFFFFFFFFFFFFFFFFFE000000";
            Zpl += "00001FFFFFFFFFFFFFFFFFFFFC000000";
            Zpl += "00000E001FFFFFFFFFFFFF0018000000";
            Zpl += "000007FFFFFFFFFFFFFFFFFFF8000000";
            Zpl += "000003FFFFFFFFFFFFFFFFFFF0000000";
            Zpl += "000003C007FFFFFFFFFFFE01E0000000";
            Zpl += "000001FFFFFFFFFFFFFFFFFFC0000000";
            Zpl += "000000FFFFFFFFFFFFFFFFFFC0000000";
            Zpl += "0000007FFFFFFFFFFFFFFFFF80000000";
            Zpl += "0000003803FFFFFFFFFFF80F00000000";
            Zpl += "0000001FFFFFFFFFFFFFFFFE00000000";
            Zpl += "0000001FFFFFFFFFFFFFFFFC00000000";
            Zpl += "0000000FC07FFFFFFFFFE07C00000000";
            Zpl += "00000007FFFFFFFFFFFFFFF800000000";
            Zpl += "00000003FFFFFFFFFFFFFFF000000000";
            Zpl += "00000001FFFFFFFFFFFFFFE000000000";
            Zpl += "00000000F01FFFFFFFFF83C000000000";
            Zpl += "000000007FFFFFFFFFFFFFC000000000";
            Zpl += "000000003FFFFFFFFFFFFF8000000000";
            Zpl += "000000001C0FFFFFFFFE0F0000000000";
            Zpl += "000000000FFFFFFFFFFFFE0000000000";
            Zpl += "000000000FFFFFFFFFFFFE0000000000";
            Zpl += "000000000703FFFFFFF83C0000000000";
            Zpl += "0000000003FFFFFFFFFFF80000000000";
            Zpl += "0000000001FFFFFFFFFFF00000000000";
            Zpl += "0000000000F0FFFFFFF1F00000000000";
            Zpl += "00000000007FFFFFFFFFE00000000000";
            Zpl += "00000000003FFFFFFFFFC00000000000";
            Zpl += "00000000001FFFFFFFFF800000000000";
            Zpl += "00000000000E3FFFFF87000000000000";
            Zpl += "000000000007FFFFFFFE000000000000";
            Zpl += "000000000007FFFFFFFC000000000000";
            Zpl += "000000000003CFFFFE38000000000000";
            Zpl += "000000000001FFFFFFF0000000000000";
            Zpl += "000000000000FFFFFFE0000000000000";
            Zpl += "0000000000007FFFFFC0000000000000";
            Zpl += "0000000000003FFFFF80000000000000";
            Zpl += "0000000000001FFFFF00000000000000";
            Zpl += "0000000000000FFFFE00000000000000";
            Zpl += "00000000000007FFFC00000000000000";
            Zpl += "00000000000003FFF800000000000000";
            Zpl += "00000000000001FFF000000000000000";
            Zpl += "00000000000000FFE000000000000000";
            Zpl += "000000000000007FC000000000000000";
            Zpl += "000000000000003F8000000000000000";
            Zpl += "000000000000001F0000000000000000";
            Zpl += "000000000000000E0000000000000000";
            Zpl += "00000000000000040000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000001F0E7E1CF1E00000000000";
            Zpl += "00000000003F8E7F1CF1E00000000000";
            Zpl += "000000000071CE639CF1E00000000000";
            Zpl += "000000000070CE619CF1E00000000000";
            Zpl += "0000000000780E619CD2E00000000000";
            Zpl += "00000000003E0E631CDB600000000000";
            Zpl += "00000000000F8E7F1CD2E00000000000";
            Zpl += "000000000007CE7E1CCAE00000000000";
            Zpl += "000000000001CE7C1CDE600000000000";
            Zpl += "000000000060CE6E1CCEC00000000000";
            Zpl += "000000000071CE671CCCE00000000000";
            Zpl += "00000000003F8E631CC4600000000000";
            Zpl += "00000000001F0E639CCCE00000000000";
            Zpl += "00000000000E0E619C84400000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";
            Zpl += "00000000000000000000000000000000";

            Zpl += "0:";
            return Zpl;
        }

        public string  Print_Image_Hyundai()
        {

            var Zpl = "~DGR:HD,00322,007,";
            Zpl += "0:";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC1FF83FFE00";
            Zpl += "7FF8FFFF1FFE00";
            Zpl += "7FFDFFFFBFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFE00";
            Zpl += "7FFFF00FFFFE00";
            Zpl += "7FFFC003FFFE00";
            Zpl += "7FFF0000FFFE00";
            Zpl += "7FFE00007FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "7FFC00003FFE00";
            Zpl += "0:";

            return Zpl;
        }

        public string  Print_Image_NEW(string tmpMARKET)
        {
            var Zpl = "";
            switch (tmpMARKET.Trim().ToUpper())
            {
                case "1":
                    Zpl += "~DGKI007,01140,012,";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "0000000000FFFE0000000000";
                    Zpl += "000000003FFFFFF800000000";
                    Zpl += "00000001FFFFFFFF00000000";
                    Zpl += "00000003FFFFFFFF80000000";
                    Zpl += "0000000FFFFFFFFFE0000000";
                    Zpl += "0000003FFFFFFFFFF8000000";
                    Zpl += "000000FFFFFFFFFFFE000000";
                    Zpl += "000001FFFFFFFFFFFF000000";
                    Zpl += "000007FFFFFFFFFFFFC00000";
                    Zpl += "00000FFFFFFFFFFFFFE00000";
                    Zpl += "00001FFFFF800FFFFFF00000";
                    Zpl += "00003FFFF800007FFFF80000";
                    Zpl += "00007FFFC0000007FFFC0000";
                    Zpl += "0000FFFE00000000FFFE0000";
                    Zpl += "0001FFFC000000007FFF0000";
                    Zpl += "0003FFF8000000003FFF8000";
                    Zpl += "0007FFE0000000000FFFC000";
                    Zpl += "000FFFC00000000007FFE000";
                    Zpl += "001FFF800000000003FFF000";
                    Zpl += "003FFF000000000000FFF000";
                    Zpl += "003FFE0000000000007FF800";
                    Zpl += "007FFC0000000000007FFC00";
                    Zpl += "007FF80000000000003FFC00";
                    Zpl += "00FFF00000000000001FFE00";
                    Zpl += "01FFE00000000000000FFE00";
                    Zpl += "01FFC00000000000000FFF00";
                    Zpl += "01FFC000000000000007FF00";
                    Zpl += "03FF8000000000000003FF00";
                    Zpl += "03FF0000000000000001FF00";
                    Zpl += "07FF001FF8003FFFC0000000";
                    Zpl += "07FF001FF800FFFF80000000";
                    Zpl += "07FE001FF801FFFF00000000";
                    Zpl += "0FFE001FF803FFFC00000000";
                    Zpl += "0FFE001FF807FFF000000000";
                    Zpl += "0FFC001FF81FFFE000000000";
                    Zpl += "0FFC001FF83FFF8000000000";
                    Zpl += "0FFC001FF87FFF0000000000";
                    Zpl += "1FFC001FF9FFFE0000000000";
                    Zpl += "1FFC001FFFFFFC0000000000";
                    Zpl += "1FFC001FFFFFFC0000000000";
                    Zpl += "1FFFFF1FFFFFFC000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFE000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFE000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFF000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFF800FFFFFF0";
                    Zpl += "1FFFFF1FFFE3FF800FFFFFF0";
                    Zpl += "1FFFFF1FFF81FFC00FFFFFF0";
                    Zpl += "1FFFFF1FFF00FFC00FFFFFF0";
                    Zpl += "1FFFFF1FFC00FFE00FFFFFF0";
                    Zpl += "0000001FF8007FF000007FF0";
                    Zpl += "0000001FF8007FF000007FF0";
                    Zpl += "0000001FF8003FF800007FF0";
                    Zpl += "0000001FF8003FFC00007FE0";
                    Zpl += "0000001FF8001FFC00007FE0";
                    Zpl += "0000001FF8000FFE00007FE0";
                    Zpl += "0000001FF8000FFE0000FFE0";
                    Zpl += "0000001FF80007FF0000FFE0";
                    Zpl += "0000001FF80007FF0000FFE0";
                    Zpl += "0000001FF80003FF8001FFC0";
                    Zpl += "0000001FF80001FF8001FFC0";
                    Zpl += "01FF0000000000000001FFC0";
                    Zpl += "01FF8000000000000003FF80";
                    Zpl += "01FFC000000000000007FF00";
                    Zpl += "01FFE00000000000000FFF00";
                    Zpl += "00FFE00000000000000FFE00";
                    Zpl += "00FFF00000000000001FFE00";
                    Zpl += "007FF80000000000003FFC00";
                    Zpl += "007FFC0000000000007FFC00";
                    Zpl += "003FFE0000000000007FF800";
                    Zpl += "001FFF000000000000FFF000";
                    Zpl += "001FFF800000000003FFF000";
                    Zpl += "0007FFC00000000007FFC000";
                    Zpl += "0003FFE0000000000FFF8000";
                    Zpl += "0001FFF8000000003FFF0000";
                    Zpl += "0001FFFC000000007FFF0000";
                    Zpl += "0000FFFE00000000FFFE0000";
                    Zpl += "00007FFFC0000007FFFC0000";
                    Zpl += "00001FFFFC00003FFFF00000";
                    Zpl += "00000FFFFFE00FFFFFE00000";
                    Zpl += "000007FFFFFFFFFFFFC00000";
                    Zpl += "000007FFFFFFFFFFFF800000";
                    Zpl += "000001FFFFFFFFFFFF000000";
                    Zpl += "000000FFFFFFFFFFFE000000";
                    Zpl += "0000003FFFFFFFFFF8000000";
                    Zpl += "0000000FFFFFFFFFE0000000";
                    Zpl += "00000001FFFFFFFF00000000";
                    Zpl += "00000001FFFFFFFE00000000";
                    Zpl += "000000003FFFFFF800000000";
                    Zpl += "0000000000FFFE0000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "0:";
                    break;
                case "2":
                    Zpl += "~DGKI006   ,01501,019,  ";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "000000000000000000000000003FF800000000";
                    Zpl += "00000000000000000000000003FFFF80000000";
                    Zpl += "0000000000000000000000001FFFFFF8000000";
                    Zpl += "000000000000000000000000FFFFFFFF000000";
                    Zpl += "000000000000000000000001FFFFFFFF800000";
                    Zpl += "000000000000000000000007FFFFFFFFE00000";
                    Zpl += "00000000000000000000000FFFC007FFF00000";
                    Zpl += "00000000000000000000001FF800003FFC0000";
                    Zpl += "00000000000000000000007FE000000FFE0000";
                    Zpl += "0000000000000000000000FFC0000003FF0000";
                    Zpl += "0000000000000000000001FF00000000FF8000";
                    Zpl += "0000000000000000000003FE000000007FC000";
                    Zpl += "0000000000000000000007FC000000003FE000";
                    Zpl += "0000000000000000000007F0000000001FF000";
                    Zpl += "000000000000000000000FE0000000000FF800";
                    Zpl += "000000000000000000001FE00000000007F800";
                    Zpl += "000000000000000000003FC00000000003FC00";
                    Zpl += "000000000000000000003F800000000001FE00";
                    Zpl += "000000000000000000007F800000000000FE00";
                    Zpl += "000000000000000000007F00FE0FF007F87F00";
                    Zpl += "0000000000F800000000FE00FE0FF00FF87F00";
                    Zpl += "0000000003FE00030000FE00FE0FF01FF83F00";
                    Zpl += "007F83FE0F0780030001FC00FE0FF03FF83F80";
                    Zpl += "007F80F80F0780030001F800FE0FF03FF81F80";
                    Zpl += "003E00F01F0380038001F800FE0FF07FF81F80";
                    Zpl += "003E01C03E03C0078003F800FE0FF07FF81F80";
                    Zpl += "003E03803E01C007C003F000FE0FF07E001F80";
                    Zpl += "003E03803E01C007C003F000FE0FF07E001FC0";
                    Zpl += "003E0E003F00000FE007F000FE0FF07E001FC0";
                    Zpl += "003E0E003F00000DE007E000FE0FF07E001FC0";
                    Zpl += "003E1C001FC00019F007E000FE0FF07E000FC0";
                    Zpl += "003E38001FE00018F007E000FE0FF07F000FC0";
                    Zpl += "003E70000FF80018F007E000FE0FF03FC00FC0";
                    Zpl += "003FF00003FE0038F807E000FE0FF03FE00FC0";
                    Zpl += "003FF80003FF00307807E000FE0FF01FF00FC0";
                    Zpl += "003FF80001FF80307807E000FE0FF00FF80FC0";
                    Zpl += "003E7C00007FC0607C07E000FE0FF007F80FC0";
                    Zpl += "003E7E00003FC07FFC07E000FE0FF001F80FC0";
                    Zpl += "003E7E00001FE0FFFC07E000FE0FF000FC0FC0";
                    Zpl += "003E1F000007E0FFFC07E000FE0FF000FC1FC0";
                    Zpl += "003E1F800007E0E03E03F000FE0FF000FC1FC0";
                    Zpl += "003E0FC03003E1C03E03F000FE0FF000FC1FC0";
                    Zpl += "003E07C03003E1C03F03F000FE0FF000FC1F80";
                    Zpl += "003E07C03803C1C01F03F801FE0FF001FC3F80";
                    Zpl += "003E03F03C07C3801F01FE03FC0FF07FF83F80";
                    Zpl += "003E03F03E0783801F81FF07FC0FF07FF83F00";
                    Zpl += "003E01F03E0783801F80FFFFF80FF07FF87F00";
                    Zpl += "00FFE1FC0FFC0FE07FE07FFFF00FF07FF0FF00";
                    Zpl += "00FFE1FE03F81FF07FF03FFFE00FF07FE0FE00";
                    Zpl += "000000000000000000001FFFC00FF07F01FE00";
                    Zpl += "000000000000000000000FFF0000000003FC00";
                    Zpl += "0000000000000000000003FC0000000003FC00";
                    Zpl += "0000000000000000000000000000000007F800";
                    Zpl += "000000000000000000000000000000000FF800";
                    Zpl += "000000000000000000000000000000000FF000";
                    Zpl += "000000000000000000000000000000001FE000";
                    Zpl += "0000000000000000000007FC000000003FE000";
                    Zpl += "0000000000000000000007FE00000000FFC000";
                    Zpl += "0000000000000000000003FF00000001FF8000";
                    Zpl += "0000000000000000000001FF80000007FF0000";
                    Zpl += "0000000000000000000000FFE000000FFE0000";
                    Zpl += "00000000000000000000007FF800003FFC0000";
                    Zpl += "00000000000000000000003FFE0000FFF80000";
                    Zpl += "00000000000000000000000FFFE01FFFE00000";
                    Zpl += "000000000000000000000007FFFFFFFFC00000";
                    Zpl += "000000000000000000000001FFFFFFFF000000";
                    Zpl += "0000000000000000000000007FFFFFFC000000";
                    Zpl += "0000000000000000000000001FFFFFF0000000";
                    Zpl += "00000000000000000000000003FFFF80000000";
                    Zpl += "000000000000000000000000001FF000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "00000000000000000000000000000000000000";
                    Zpl += "0:";
                    break;


            }

            return Zpl;
        }
        
        public string  Print_Image_KSA_PB4(string tmpMARKET)
        {
            var Zpl = "";
            switch (tmpMARKET.Trim().ToUpper())
            {
                case "1":
                    Zpl = Zpl + "~DGR:KSA_1,01025,025,";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "0003FC0FC0FFFFF00FFFFF00FFFFFC03FC0FC00FF03F000000";
                    Zpl = Zpl + "0003FC0FC0FFFFF00FFFFF00FFFFFC03FC0FC00FF03F000000";
                    Zpl = Zpl + "003FFFC7C0FFFFF00FFFFF00FFFFFC3FFFC3C0FFFF1F000000";
                    Zpl = Zpl + "003FFFC3C0FFFFF00FFFFF00FFFFFC3FFFC3C0FFFF0F000000";
                    Zpl = Zpl + "003FFFC3F00000F000F0F00000FC003FFFFFC0FFFF0F000000";
                    Zpl = Zpl + "000FFF03F00000F000F0F00003FF000FFF3FC03FFC0F000000";
                    Zpl = Zpl + "001F0F83F00000F000F0F0000FFFC01F0FBFC07C3E0F000000";
                    Zpl = Zpl + "003E07C3F00000F000F0F0003FCFF03E07FFC0F81F0F000000";
                    Zpl = Zpl + "003C03C3C00000F000F0F000FF03FC3C03C3C0F00F0F000000";
                    Zpl = Zpl + "003C03C3C00000F000F0F000FC00FC3C03C3C0F00F0F000000";
                    Zpl = Zpl + "003E07C3C00000F00FFFFF00F0003C3E07FFC0F81F0F000000";
                    Zpl = Zpl + "003F0FC3C3FFFFFC0FFFFF00FFFFFC3F0FFFC0FC3F0F000000";
                    Zpl = Zpl + "001FFF83C3FFFFFC0FFFFF00FFFFFC1FFFBFC07FFE0F000000";
                    Zpl = Zpl + "000FFF03C3FFFFFC0FFFFF00FFFFFC0FFF3FC03FFC0F000000";
                    Zpl = Zpl + "0003FC03C000F00000000000003C0003FC03C00FF00F000000";
                    Zpl = Zpl + "00000003C000F00000000000003C00000003C003C00F000000";
                    Zpl = Zpl + "00000003C0FFFFF000F0F000F03C0007C003C003C00F000000";
                    Zpl = Zpl + "0007C003C0FFFFF000F0F000F03C0007C003C003C00F000000";
                    Zpl = Zpl + "0007C00000FFFFF000F0F000F0000007FFFFC0FFFFFF000000";
                    Zpl = Zpl + "0007C00000FFFFF000F0F000F0000007FFFFC0FFFFFF000000";
                    Zpl = Zpl + "0007C000000000F000F0F000F0000007FFFFC0FFFFFF000000";
                    Zpl = Zpl + "0007FFFFC00000F03FFFFFC0FFFFFC07C003C0FFFFFF000000";
                    Zpl = Zpl + "0007FFFFC00000F03FFFFFC0FFFFFC07FFFFC000000F000000";
                    Zpl = Zpl + "0003FFFFC00000F03FFFFFC07FFFFC03FFFFC000000F000000";
                    Zpl = Zpl + "0001FFFFC00000F03FFFFFC03FFFFC01FFFFC000000F000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";
                    Zpl = Zpl + "00000000000000000000000000000000000000000000000000";

                    Zpl += "0:";
                    break;
            }

            return Zpl;
        }

        public string  Print_Image_NAEJIN(string tmpMARKET)
        {
            var Zpl = "";
            Zpl += "~DGR:NAEJIN,02844,012,";
            Zpl += "000000000000000000000000";
            Zpl += "00003FFFFFFFFFFFFFFF8000";
            Zpl += "0000FFFFFFFFFFFFFFFFC000";
            Zpl += "0001FFFFFFFFFFFFFFFFE000";
            Zpl += "0003FFFFFFFFFFFFFFFFE000";
            Zpl += "0007FFFFFFFFFFFFFFFFE000";
            Zpl += "0007FFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFE000";
            Zpl += "000FFFFFFFFFFFFFFFFFC000";
            Zpl += "000FFFFFFFFFFFFFFFFF8000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "000FFF000000000000000000";
            Zpl += "0FFFFFFFFFFFFFFFFFFFE000";
            Zpl += "1FFFFFFFFFFFFFFFFFFFF800";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC00";
            Zpl += "1FFFFFFFFFFFFFFFFFFFF800";
            Zpl += "0FFFFFFFFFFFFFFFFFFFE000";
            Zpl += "00000000007FF80000000000";
            Zpl += "00000000007FF80000000000";
            Zpl += "00000000007FF80000000000";
            Zpl += "00000000007FF80000000000";
            Zpl += "00000000007FF80000000000";
            Zpl += "00000000007FF80000000000";
            Zpl += "00000000007FF80000000000";
            Zpl += "00000000007FF80000000000";
            Zpl += "0FFFFFFFFFFFFFFFFFFFE000";
            Zpl += "1FFFFFFFFFFFFFFFFFFFF800";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFC00";
            Zpl += "1FFFFFFFFFFFFFFFFFFFF800";
            Zpl += "0FFFFFFFFFFFFFFFFFFFE000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000080000000000000";
            Zpl += "0000000007C0000000000000";
            Zpl += "000000000FE00000007FC000";
            Zpl += "000000001FF0000000FFE000";
            Zpl += "000000003FF8000000FFE000";
            Zpl += "000000007FFC000000FFE000";
            Zpl += "00000000FFFE000000FFE000";
            Zpl += "01FFFFF8FFFF000000FFE000";
            Zpl += "03FFFFFCFFFF800000FFE000";
            Zpl += "07FFFFFCFFFFC00000FFE000";
            Zpl += "0FFFFFFC7FFFE00000FFE000";
            Zpl += "1FFFFFFC3FFFF00000FFE000";
            Zpl += "1FFFFFFC1FFFF80000FFE000";
            Zpl += "1FFFFFFC0FFFFC0000FFE000";
            Zpl += "1FFFFFFC07FFFE0000FFE000";
            Zpl += "1FFFFFFC03FFFF0000FFE000";
            Zpl += "1FFFFFFC01FFFF8000FFE000";
            Zpl += "1FFFFFFC00FFFFC000FFE000";
            Zpl += "1FFFFFFC007FFFE000FFE000";
            Zpl += "1FFFFFFC003FFFFFFFFFE000";
            Zpl += "1FFFFFF8001FFFFFFFFFE000";
            Zpl += "1FFE0000000FFFFFFFFFE000";
            Zpl += "1FFE00000007FFFFFFFFE000";
            Zpl += "1FFE00000003FFFFFFFFE000";
            Zpl += "1FFE00000003FFFFFFFFE000";
            Zpl += "1FFE00000003FFFFFFFFE000";
            Zpl += "1FFE00000003FFFFFFFFE000";
            Zpl += "1FFE00000003FFFFFFFFE000";
            Zpl += "1FFE00000003FFFFFFFFE000";
            Zpl += "1FFE00000003FFFFFFFFE000";
            Zpl += "1FFE00000007FFFFFFFFE000";
            Zpl += "1FFE0000000FFFFFFFFFE000";
            Zpl += "1FFE0000001FFFFFFFFFE000";
            Zpl += "1FFE0000003FFFE000FFE000";
            Zpl += "1FFE0000007FFFC000FFE000";
            Zpl += "1FFE000000FFFF8000FFE000";
            Zpl += "1FFE000001FFFF0000FFE000";
            Zpl += "1FFE000003FFFE0000FFE000";
            Zpl += "1FFE000007FFFC0000FFE000";
            Zpl += "1FFE00000FFFF80000FFE000";
            Zpl += "1FFE00001FFFF00000FFE000";
            Zpl += "1FFE00003FFFE00000FFE000";
            Zpl += "1FFE00007FFFC00000FFE000";
            Zpl += "1FFE0000FFFF800000FFE000";
            Zpl += "1FFE0000FFFF000000FFE000";
            Zpl += "1FFE0000FFFE000000FFE000";
            Zpl += "1FFE0000FFFC000000FFE000";
            Zpl += "1FFE00007FF8000000FFE000";
            Zpl += "1FFE00003FF0000000FFE000";
            Zpl += "1FFE00001FE00000007FC000";
            Zpl += "1FFE00000FC0000000000000";
            Zpl += "1FFE00000780000000000000";
            Zpl += "1FFE00000300000000000000";
            Zpl += "1FFE00000000000000000000";
            Zpl += "1FFE00000000000000000000";
            Zpl += "1FFE00000000000000000000";
            Zpl += "1FFE01FFFFFFFFFFFFFFE000";
            Zpl += "1FFE03FFFFFFFFFFFFFFF800";
            Zpl += "1FFE07FFFFFFFFFFFFFFFC00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFE00";
            Zpl += "1FFE07FFFFFFFFFFFFFFFC00";
            Zpl += "0FFE03FFFFFFFFFFFFFFF800";
            Zpl += "07FE01FFFFFFFFFFFFFFE000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "0000000007FC000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "0007F0000FFE00003F000000";
            Zpl += "0018FC000FFE0000FFC00000";
            Zpl += "003FFF000FFE0003FFF00000";
            Zpl += "007FFF800FFE0007FFF80000";
            Zpl += "00FFFFC00FFE000FFFFC0000";
            Zpl += "01FFFFE00FFE001FFFFE0000";
            Zpl += "03FFFFE00FFE001FFFFF0000";
            Zpl += "03FFFFF00FFE003FFFFF8000";
            Zpl += "07FFFFF80FFFFFFFFFFF8000";
            Zpl += "0FFFFFF80FFFFFFFFFFFC000";
            Zpl += "0FFFFFFC0FFFFFFFFFFFC000";
            Zpl += "1FFFFFFC0FFFFFFFFFFFE000";
            Zpl += "1FFFFFFE0FFFFFFFFFFFE000";
            Zpl += "1FFFFFFE0FFFFFFFFFFFF000";
            Zpl += "3FFFFFFF0FFFFFFFFFFFF000";
            Zpl += "3FFE3FFF0FFFFFFFF0FFF800";
            Zpl += "3FFE1FFE0FFFFFFFE1FFF800";
            Zpl += "7FFC0FFE0FFFFFFFC0FFF800";
            Zpl += "7FFC0FFF8FFFFFFF807FF800";
            Zpl += "7FF807FF8FFFFFFF807FFC00";
            Zpl += "7FF807FF8FFFFFFF003FFC00";
            Zpl += "7FF003FF8FFFFFFF003FFC00";
            Zpl += "FFF003FFCFFE07FE001FFC00";
            Zpl += "FFF003FFCFFE07FE001FFC00";
            Zpl += "FFF003FFCFFE07FE001FFE00";
            Zpl += "FFE001FFCFFE0FFC000FFE00";
            Zpl += "FFE001FFCFFE0FFC000FFE00";
            Zpl += "FFE001FFCFFE0FFC000FFE00";
            Zpl += "FFE001FFCFFE0FFC000FFE00";
            Zpl += "FFC000FFCFFE0FF80007FE00";
            Zpl += "FFC000FFCFFE0FF80007FE00";
            Zpl += "FFC000FFCFFE0FF80007FE00";
            Zpl += "FFC000FFCFFE0FF80007FE00";
            Zpl += "FFC000FFCFFE0FF80007FE00";
            Zpl += "FFC000FFCFFE0FFC000FFE00";
            Zpl += "FFC000FFCFFE0FFC000FFE00";
            Zpl += "FFE001FFCFFE0FFC000FFE00";
            Zpl += "FFE001FFCFFE0FFC000FFE00";
            Zpl += "FFE001FFCFFE07FE001FFE00";
            Zpl += "FFF003FFCFFE07FE001FFC00";
            Zpl += "FFF003FFCFFE07FE001FFC00";
            Zpl += "FFF003FFCFFFFFFF003FFC00";
            Zpl += "7FF003FF8FFFFFFF003FFC00";
            Zpl += "7FF807FF8FFFFFFF807FFC00";
            Zpl += "7FF807FF8FFFFFFF807FF800";
            Zpl += "7FFC0FFF0FFFFFFFC0FFF800";
            Zpl += "7FFC0FFF0FFFFFFFE1FFF800";
            Zpl += "3FFE1FFF0FFFFFFFF3FFF800";
            Zpl += "3FFE3FFF0FFFFFFFFFFFF000";
            Zpl += "3FFCFFFF0FFFFFFFFFFFF000";
            Zpl += "1FFFFFFE0FFFFFFFFFFFE000";
            Zpl += "1FFFFFFE0FFFFFFFFFFFE000";
            Zpl += "1FFFFFFC0FFFFFFFFFFFC000";
            Zpl += "0FFFFFFC0FFFFFFFFFFFC000";
            Zpl += "0FFFFFF80FFFFFFFFFFF8000";
            Zpl += "07FFFFF80FFFFFFFFFFF8000";
            Zpl += "03FFFFF00FFE001FFFFF0000";
            Zpl += "03FFFFF00FFE001FFFFE0000";
            Zpl += "01FFFFE00FFE000FFFFC0000";
            Zpl += "01FFFFC00FFE0007FFF80000";
            Zpl += "00FFFFC00FFE0003FFF00000";
            Zpl += "007FFF800FFE0000FFE00000";
            Zpl += "003FFF000FFE00003F800000";
            Zpl += "001FFC000FFE000000000000";
            Zpl += "0007F0000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "000000000FFE000000000000";
            Zpl += "0000000007FC000000000000";
            Zpl += "0:";

            return Zpl;
        }

        public string  Print_Image_ACRS(string tmpMARKET)
        {
            var Zpl = "";

            Zpl += "~DGR:ACRS,01248,012,";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";
            Zpl += "0FFFFFFFFFFFFFFFFFFFFC00";
            Zpl += "1FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFF00";
            Zpl += "7FFFFFFFFFFFFFFFFFFFFF80";
            Zpl += "FE0000E0000E0000E0001FC0";
            Zpl += "FC0000E0000E0000E0000FC0";
            Zpl += "F80000E0000E0000E00007C0";
            Zpl += "F00000E0000E0000E00003C0";
            Zpl += "F00000E0000E0000E00003C0";
            Zpl += "F00000E0000E0000E00003C0";
            Zpl += "F00000E0007FC000E00003C0";
            Zpl += "F00000E007FFFC00E00003C0";
            Zpl += "F00000E01FFFFF00E00003C0";
            Zpl += "F00000E07FFFFFC0E00003C0";
            Zpl += "F00000E0FFFFFFF0E00003C0";
            Zpl += "F00000E2FFFFFFF8E00003C0";
            Zpl += "F00000EFFEFFF7FEE00003C0";
            Zpl += "F00000FFF0FEF1FFE00003C0";
            Zpl += "F00000FFE0FF707FE00003C0";
            Zpl += "F00000DFC1FFF83FE00003C0";
            Zpl += "FFFFFFFF03FFFC1FFFFFFFC0";
            Zpl += "FFFFFFFC03FFF40FFFFFFFC0";
            Zpl += "FFFFFFF807FFF607FFFFFFC0";
            Zpl += "F00007F7FFFFFFFBFC0003C0";
            Zpl += "F00003EFFFFFFFFDFC0003C0";
            Zpl += "F0000FDFFFFFFFFEFE0003C0";
            Zpl += "F0000FFFFFFFFFFFFF0003C0";
            Zpl += "F0001FFFFFEFFFFFFF0003C0";
            Zpl += "F0001FFFFFFFFFFFFF0003C0";
            Zpl += "F0003FFFFDBFF7FFFF8003C0";
            Zpl += "F0003FFFFEFFEFFFFF8003C0";
            Zpl += "F0007FFFFE3F8FFFFFC003C0";
            Zpl += "F0007FFFFE1F0FFFFFC003C0";
            Zpl += "F0007DFFFE7FCFFFFFC003C0";
            Zpl += "F000FDFFFDFFF7FFE7E003C0";
            Zpl += "F000F8FFFF7FFFFFE3E003C0";
            Zpl += "F000F8FFFFFFFFFFC3E003C0";
            Zpl += "F001F07FFBFFFEFFC2F003C0";
            Zpl += "F001F03FF7FFFFFF00F003C0";
            Zpl += "F001F80FFFFFFFBE03F003C0";
            Zpl += "FFFFFFFFFF7FFFFFFFFFFFC0";
            Zpl += "FFFFFFFFFF7FFFFFFFFFFFC0";
            Zpl += "FFFFFFFFFFFFFFFFFFFFFFC0";
            Zpl += "F001F87FFEFFFFFE03F003C0";
            Zpl += "F001F07FFEFFFFFF01F003C0";
            Zpl += "F001F1FFFFFFFFFFC3F003C0";
            Zpl += "F000F9FFFCFFE7FFC3E003C0";
            Zpl += "F000F3FFFE7FCFFFE3E003C0";
            Zpl += "F000F3FFFE1F0FFFE7E003C0";
            Zpl += "F00077FFFE1F0FFFFFC003C0";
            Zpl += "F0007FFFFE7FCFFFFFC003C0";
            Zpl += "F0007FFFFEFF6FFFFFC003C0";
            Zpl += "F0003FFFFFFFFFFFFF8003C0";
            Zpl += "F0003FFFFFFFFFFFFF8003C0";
            Zpl += "F0001FFFFFFFFFFFFF8003C0";
            Zpl += "F0001FFFFFFFFFFFFF8003C0";
            Zpl += "F0003FFFFFFFFFFFFFC003C0";
            Zpl += "F0007FFFF7FFFDFFFFE003C0";
            Zpl += "F000FFFBE7FFFCFBFFF003C0";
            Zpl += "FFFFFFF807FFFC03FFFFFFC0";
            Zpl += "FFFFFFFC07FFFC07FFFFFFC0";
            Zpl += "FFFFFFFF03FFF81FFFFFFFC0";
            Zpl += "F00FE0FFC1FFF07FF0FE03C0";
            Zpl += "F01FC0FFF0FFE0FFF07F03C0";
            Zpl += "F03F80FFF87FC3FFF03F83C0";
            Zpl += "F07F00EFFFFFFEFF701FC3C0";
            Zpl += "F0FE00E7FEFFFFFC700FE3C0";
            Zpl += "F0FC00E1FFFFFFF87007E3C0";
            Zpl += "F0F800E0FFFFFFE07003E3C0";
            Zpl += "F07000E01FFFFF807001C3C0";
            Zpl += "F00000E007FFFE00700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "FFFFFFFFFFFFFFFFFFFFFFC0";
            Zpl += "FFFFFFFFFFFFFFFFFFFFFFC0";
            Zpl += "FFFFFFFFFFFFFFFFFFFFFFC0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00000E0000E0000700003C0";
            Zpl += "F00180E07F8E3FF871FF83C0";
            Zpl += "F00180E0FF8E3FFC73FFC3C0";
            Zpl += "F003C0E1C00E301C72C1C3C0";
            Zpl += "F003C0E3800E201C73C003C0";
            Zpl += "F007E0E7000E301C71E003C0";
            Zpl += "F007E0E7000E3FFC70F003C0";
            Zpl += "F00E70E7000E3FF8707803C0";
            Zpl += "F00E70E7000E3380701C03C0";
            Zpl += "F01C38E7000E31C0700E03C0";
            Zpl += "F01FF8E7000E30E0700703C0";
            Zpl += "F03FECE3000E3070700383C0";
            Zpl += "F03C3CE1800E30387303C3C0";
            Zpl += "F8281CE0FF8E301C73FFC7C0";
            Zpl += "FC300CE07F8E300C71FF8FC0";
            Zpl += "FE0000E0000E000070001FC0";
            Zpl += "7F0000E0000E000070003F80";
            Zpl += "3FFFFFFFFFFFFFFFFFFFFF00";
            Zpl += "1FFFFFFFFFFFFFFFFFFFFE00";
            Zpl += "0FFFFFFFFFFFFFFFFFFFFC00";
            Zpl += "07FFFFFFFFFFFFFFFFFFF800";
            Zpl += "000000000000000000000000";
            Zpl += "000000000000000000000000";

            Zpl += "0:";

            return Zpl;
        }

        public string  Print_KS_Image(string tmpMARKET)
        {
            var Zpl = "";

            switch (tmpMARKET)
            {
                case "KS":
                    Zpl += "~DGR:KS11,00464,008,";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    Zpl += "000001FFE0000000";
                    Zpl += "00001FFFFC000000";
                    Zpl += "00007FFFFF800000";
                    Zpl += "0001FFFFFFE00000";
                    Zpl += "0003FFFFFFF00000";
                    Zpl += "000FFFFFFFFC0000";
                    Zpl += "001FFF001FFE0000";
                    Zpl += "003FF80003FF0000";
                    Zpl += "007FF00001FF8000";
                    Zpl += "00FFC000007FC000";
                    Zpl += "01FF0000001FE000";
                    Zpl += "01FE0000000FE000";
                    Zpl += "03FC00000007F000";
                    Zpl += "07F800000003F800";
                    Zpl += "07F000000001FC00";
                    Zpl += "0FF000000001FC00";
                    Zpl += "0FE000000000FE00";
                    Zpl += "1FC07E007FC07E00";
                    Zpl += "1FC07E01FF800000";
                    Zpl += "1F807E03FF000000";
                    Zpl += "1F807E0FFC000000";
                    Zpl += "3F807E1FF8000000";
                    Zpl += "3F807E7FE0000000";
                    Zpl += "3F807EFFC0000000";
                    Zpl += "3FFE7FFF80FFFF80";
                    Zpl += "3FFE7FFFC0FFFF80";
                    Zpl += "3FFE7FFFC0FFFF80";
                    Zpl += "3FFE7FFFE0FFFF80";
                    Zpl += "3FFE7FEFF0FFFF80";
                    Zpl += "3FFE7F87F0FFFF80";
                    Zpl += "00007F03F8003F80";
                    Zpl += "00007E03FC003F80";
                    Zpl += "00007E01FE003F00";
                    Zpl += "00007E00FE007F00";
                    Zpl += "00007E00FF007F00";
                    Zpl += "00007E007F807E00";
                    Zpl += "0FE07E003FC0FE00";
                    Zpl += "0FF000000001FE00";
                    Zpl += "0FF000000001FC00";
                    Zpl += "07F800000003F800";
                    Zpl += "07FC00000007F800";
                    Zpl += "03FE0000000FF000";
                    Zpl += "01FF0000001FE000";
                    Zpl += "01FF8000003FE000";
                    Zpl += "00FFE00001FFC000";
                    Zpl += "007FF00003FF8000";
                    Zpl += "003FFC000FFF0000";
                    Zpl += "001FFFE1FFFE0000";
                    Zpl += "000FFFFFFFFC0000";
                    Zpl += "0003FFFFFFF00000";
                    Zpl += "0001FFFFFFE00000";
                    Zpl += "00007FFFFF800000";
                    Zpl += "00000FFFFC000000";
                    Zpl += "000001FFE0000000";
                    Zpl += "0000000000000000";
                    Zpl += "0000000000000000";
                    Zpl += "0:";
                    break;
                case "JIS":
                    Zpl = Zpl + "~DGKI006,00528,008,";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000003FF8000000";
                    Zpl = Zpl + "000001FFFF800000";
                    Zpl = Zpl + "00000FFFFFF00000";
                    Zpl = Zpl + "00003FFFFFFC0000";
                    Zpl = Zpl + "00007FFFFFFE0000";
                    Zpl = Zpl + "0001FFFFFFFF8000";
                    Zpl = Zpl + "0003FF0007FFC000";
                    Zpl = Zpl + "000FFC0000FFE000";
                    Zpl = Zpl + "001FF800003FF800";
                    Zpl = Zpl + "003FF000000FFC00";
                    Zpl = Zpl + "007FE0000007FC00";
                    Zpl = Zpl + "00FF80000001FE00";
                    Zpl = Zpl + "00FF00000000FF00";
                    Zpl = Zpl + "01FE000000007F80";
                    Zpl = Zpl + "03FC000000003F80";
                    Zpl = Zpl + "03F8000000001FC0";
                    Zpl = Zpl + "07F8000000000FC0";
                    Zpl = Zpl + "0FF03F07E01FCFE0";
                    Zpl = Zpl + "0FE03F07E03FC7F0";
                    Zpl = Zpl + "0FE03F07E07FC7F0";
                    Zpl = Zpl + "1FC03F07E07FC3F0";
                    Zpl = Zpl + "1FC03F07E0FFC3F0";
                    Zpl = Zpl + "1F803F07E0FFC1F8";
                    Zpl = Zpl + "3F803F07E0F801F8";
                    Zpl = Zpl + "3F803F07E0F801F8";
                    Zpl = Zpl + "3F003F07E0F801F8";
                    Zpl = Zpl + "3F003F07E0F800FC";
                    Zpl = Zpl + "3F003F07E0F800FC";
                    Zpl = Zpl + "3F003F07E0FE00FC";
                    Zpl = Zpl + "3F003F07E07F00FC";
                    Zpl = Zpl + "3F003F07E07F80FC";
                    Zpl = Zpl + "3F003F07E03FC0FC";
                    Zpl = Zpl + "3F003F07E007C0FC";
                    Zpl = Zpl + "3F003F07E003E0FC";
                    Zpl = Zpl + "3F003F07E003E1FC";
                    Zpl = Zpl + "3F003F07E003E1F8";
                    Zpl = Zpl + "1F803F07E007E1F8";
                    Zpl = Zpl + "1F807F07E007E3F8";
                    Zpl = Zpl + "1FC07E07E0FFC3F8";
                    Zpl = Zpl + "0FE0FE07E0FFC3F0";
                    Zpl = Zpl + "0FFFFC07E0FFC7F0";
                    Zpl = Zpl + "07FFFC07E0FF8FF0";
                    Zpl = Zpl + "03FFF807E0FE0FF0";
                    Zpl = Zpl + "00FFE00000001FE0";
                    Zpl = Zpl + "003F800000001FC0";
                    Zpl = Zpl + "0000000000003FC0";
                    Zpl = Zpl + "0000000000007F80";
                    Zpl = Zpl + "000000000000FF80";
                    Zpl = Zpl + "00FF00000001FF00";
                    Zpl = Zpl + "007F80000003FE00";
                    Zpl = Zpl + "003FE0000007FC00";
                    Zpl = Zpl + "003FF000001FFC00";
                    Zpl = Zpl + "001FFC00003FF800";
                    Zpl = Zpl + "000FFF0000FFE000";
                    Zpl = Zpl + "0003FFF03FFFC000";
                    Zpl = Zpl + "0001FFFFFFFF8000";
                    Zpl = Zpl + "00007FFFFFFE0000";
                    Zpl = Zpl + "00003FFFFFFC0000";
                    Zpl = Zpl + "00000FFFFFF00000";
                    Zpl = Zpl + "000001FFFF800000";
                    Zpl = Zpl + "0000001FF8000000";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000000000000000";
                    break;
            }
            return Zpl;
        }

        public string  Print_Image_Steel(string tmpMARKET)
        {
            var Zpl = "";
            switch (tmpMARKET)
            {
                case "KS":
                    Zpl += "~DGKI007,01140,012,";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "0000000000FFFE0000000000";
                    Zpl += "000000003FFFFFF800000000";
                    Zpl += "00000001FFFFFFFF00000000";
                    Zpl += "00000003FFFFFFFF80000000";
                    Zpl += "0000000FFFFFFFFFE0000000";
                    Zpl += "0000003FFFFFFFFFF8000000";
                    Zpl += "000000FFFFFFFFFFFE000000";
                    Zpl += "000001FFFFFFFFFFFF000000";
                    Zpl += "000007FFFFFFFFFFFFC00000";
                    Zpl += "00000FFFFFFFFFFFFFE00000";
                    Zpl += "00001FFFFF800FFFFFF00000";
                    Zpl += "00003FFFF800007FFFF80000";
                    Zpl += "00007FFFC0000007FFFC0000";
                    Zpl += "0000FFFE00000000FFFE0000";
                    Zpl += "0001FFFC000000007FFF0000";
                    Zpl += "0003FFF8000000003FFF8000";
                    Zpl += "0007FFE0000000000FFFC000";
                    Zpl += "000FFFC00000000007FFE000";
                    Zpl += "001FFF800000000003FFF000";
                    Zpl += "003FFF000000000000FFF000";
                    Zpl += "003FFE0000000000007FF800";
                    Zpl += "007FFC0000000000007FFC00";
                    Zpl += "007FF80000000000003FFC00";
                    Zpl += "00FFF00000000000001FFE00";
                    Zpl += "01FFE00000000000000FFE00";
                    Zpl += "01FFC00000000000000FFF00";
                    Zpl += "01FFC000000000000007FF00";
                    Zpl += "03FF8000000000000003FF00";
                    Zpl += "03FF0000000000000001FF00";
                    Zpl += "07FF001FF8003FFFC0000000";
                    Zpl += "07FF001FF800FFFF80000000";
                    Zpl += "07FE001FF801FFFF00000000";
                    Zpl += "0FFE001FF803FFFC00000000";
                    Zpl += "0FFE001FF807FFF000000000";
                    Zpl += "0FFC001FF81FFFE000000000";
                    Zpl += "0FFC001FF83FFF8000000000";
                    Zpl += "0FFC001FF87FFF0000000000";
                    Zpl += "1FFC001FF9FFFE0000000000";
                    Zpl += "1FFC001FFFFFFC0000000000";
                    Zpl += "1FFC001FFFFFFC0000000000";
                    Zpl += "1FFFFF1FFFFFFC000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFE000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFE000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFF000FFFFFF0";
                    Zpl += "1FFFFF1FFFFFFF800FFFFFF0";
                    Zpl += "1FFFFF1FFFE3FF800FFFFFF0";
                    Zpl += "1FFFFF1FFF81FFC00FFFFFF0";
                    Zpl += "1FFFFF1FFF00FFC00FFFFFF0";
                    Zpl += "1FFFFF1FFC00FFE00FFFFFF0";
                    Zpl += "0000001FF8007FF000007FF0";
                    Zpl += "0000001FF8007FF000007FF0";
                    Zpl += "0000001FF8003FF800007FF0";
                    Zpl += "0000001FF8003FFC00007FE0";
                    Zpl += "0000001FF8001FFC00007FE0";
                    Zpl += "0000001FF8000FFE00007FE0";
                    Zpl += "0000001FF8000FFE0000FFE0";
                    Zpl += "0000001FF80007FF0000FFE0";
                    Zpl += "0000001FF80007FF0000FFE0";
                    Zpl += "0000001FF80003FF8001FFC0";
                    Zpl += "0000001FF80001FF8001FFC0";
                    Zpl += "01FF0000000000000001FFC0";
                    Zpl += "01FF8000000000000003FF80";
                    Zpl += "01FFC000000000000007FF00";
                    Zpl += "01FFE00000000000000FFF00";
                    Zpl += "00FFE00000000000000FFE00";
                    Zpl += "00FFF00000000000001FFE00";
                    Zpl += "007FF80000000000003FFC00";
                    Zpl += "007FFC0000000000007FFC00";
                    Zpl += "003FFE0000000000007FF800";
                    Zpl += "001FFF000000000000FFF000";
                    Zpl += "001FFF800000000003FFF000";
                    Zpl += "0007FFC00000000007FFC000";
                    Zpl += "0003FFE0000000000FFF8000";
                    Zpl += "0001FFF8000000003FFF0000";
                    Zpl += "0001FFFC000000007FFF0000";
                    Zpl += "0000FFFE00000000FFFE0000";
                    Zpl += "00007FFFC0000007FFFC0000";
                    Zpl += "00001FFFFC00003FFFF00000";
                    Zpl += "00000FFFFFE00FFFFFE00000";
                    Zpl += "000007FFFFFFFFFFFFC00000";
                    Zpl += "000007FFFFFFFFFFFF800000";
                    Zpl += "000001FFFFFFFFFFFF000000";
                    Zpl += "000000FFFFFFFFFFFE000000";
                    Zpl += "0000003FFFFFFFFFF8000000";
                    Zpl += "0000000FFFFFFFFFE0000000";
                    Zpl += "00000001FFFFFFFF00000000";
                    Zpl += "00000001FFFFFFFE00000000";
                    Zpl += "000000003FFFFFF800000000";
                    Zpl += "0000000000FFFE0000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "000000000000000000000000";
                    Zpl += "0:";

                    break;
                case "JIS":
                    Zpl = Zpl + "~DGKI006,00528,008,";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000003FF8000000";
                    Zpl = Zpl + "000001FFFF800000";
                    Zpl = Zpl + "00000FFFFFF00000";
                    Zpl = Zpl + "00003FFFFFFC0000";
                    Zpl = Zpl + "00007FFFFFFE0000";
                    Zpl = Zpl + "0001FFFFFFFF8000";
                    Zpl = Zpl + "0003FF0007FFC000";
                    Zpl = Zpl + "000FFC0000FFE000";
                    Zpl = Zpl + "001FF800003FF800";
                    Zpl = Zpl + "003FF000000FFC00";
                    Zpl = Zpl + "007FE0000007FC00";
                    Zpl = Zpl + "00FF80000001FE00";
                    Zpl = Zpl + "00FF00000000FF00";
                    Zpl = Zpl + "01FE000000007F80";
                    Zpl = Zpl + "03FC000000003F80";
                    Zpl = Zpl + "03F8000000001FC0";
                    Zpl = Zpl + "07F8000000000FC0";
                    Zpl = Zpl + "0FF03F07E01FCFE0";
                    Zpl = Zpl + "0FE03F07E03FC7F0";
                    Zpl = Zpl + "0FE03F07E07FC7F0";
                    Zpl = Zpl + "1FC03F07E07FC3F0";
                    Zpl = Zpl + "1FC03F07E0FFC3F0";
                    Zpl = Zpl + "1F803F07E0FFC1F8";
                    Zpl = Zpl + "3F803F07E0F801F8";
                    Zpl = Zpl + "3F803F07E0F801F8";
                    Zpl = Zpl + "3F003F07E0F801F8";
                    Zpl = Zpl + "3F003F07E0F800FC";
                    Zpl = Zpl + "3F003F07E0F800FC";
                    Zpl = Zpl + "3F003F07E0FE00FC";
                    Zpl = Zpl + "3F003F07E07F00FC";
                    Zpl = Zpl + "3F003F07E07F80FC";
                    Zpl = Zpl + "3F003F07E03FC0FC";
                    Zpl = Zpl + "3F003F07E007C0FC";
                    Zpl = Zpl + "3F003F07E003E0FC";
                    Zpl = Zpl + "3F003F07E003E1FC";
                    Zpl = Zpl + "3F003F07E003E1F8";
                    Zpl = Zpl + "1F803F07E007E1F8";
                    Zpl = Zpl + "1F807F07E007E3F8";
                    Zpl = Zpl + "1FC07E07E0FFC3F8";
                    Zpl = Zpl + "0FE0FE07E0FFC3F0";
                    Zpl = Zpl + "0FFFFC07E0FFC7F0";
                    Zpl = Zpl + "07FFFC07E0FF8FF0";
                    Zpl = Zpl + "03FFF807E0FE0FF0";
                    Zpl = Zpl + "00FFE00000001FE0";
                    Zpl = Zpl + "003F800000001FC0";
                    Zpl = Zpl + "0000000000003FC0";
                    Zpl = Zpl + "0000000000007F80";
                    Zpl = Zpl + "000000000000FF80";
                    Zpl = Zpl + "00FF00000001FF00";
                    Zpl = Zpl + "007F80000003FE00";
                    Zpl = Zpl + "003FE0000007FC00";
                    Zpl = Zpl + "003FF000001FFC00";
                    Zpl = Zpl + "001FFC00003FF800";
                    Zpl = Zpl + "000FFF0000FFE000";
                    Zpl = Zpl + "0003FFF03FFFC000";
                    Zpl = Zpl + "0001FFFFFFFF8000";
                    Zpl = Zpl + "00007FFFFFFE0000";
                    Zpl = Zpl + "00003FFFFFFC0000";
                    Zpl = Zpl + "00000FFFFFF00000";
                    Zpl = Zpl + "000001FFFF800000";
                    Zpl = Zpl + "0000001FF8000000";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000000000000000";
                    Zpl = Zpl + "0000000000000000";
                    break;
            }
            return Zpl;
        }

        #endregion

    }
}
