using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

using ComLib;
using ComLib.clsMgr;
using C1.Win.C1FlexGrid;
using System.Data.OracleClient;
using SystemControlClassLibrary.Popup;

namespace SystemControlClassLibrary
{
    public partial class frmRB_Monitoring2 : Form
    {
        Timer t = new Timer();

        public frmRB_Monitoring2()
        {
            InitializeComponent();
        }


        #region "변수 선언"

        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        DataTable olddt;
        DataTable moddt;

        DataTable grdMain_dt;
        DataTable grdDetail_dt;

        private int subtotalNo;
        private int subtotalNo2;

        string poc_no_nm = "";

        // 셀의 수정전 값
        private static string ownerNM = "";
        private static string titleNM = "";

        C1FlexGrid selectedGrd = null;

        //전역변수 설정
        string FindHeat = string.Empty;

        int c_scrollPosition = 0;
        int c_scrollPosition1 = 0;

        #endregion

        #region "생성자.. 중요함"

        //생성자!!! 중요함!!! 이걸로 메뉴 매핑해서 화면을 띄워줌
        public frmRB_Monitoring2(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }
        #endregion

        private void frmRB_Monitoring2_Load(object sender, EventArgs e)
        {
            InitControl();
            //this.WindowState = FormWindowState.Maximized;
        }

        #region "InitControl 설정"
        private void InitControl()
        {
            //clsStyle.Style.InitGrid_search_fix(grdMain1);
            //clsStyle.Style.InitGrid_search_fix(grdMain2);
            //clsStyle.Style.InitGrid_search_fix(grdSub1);
            //clsStyle.Style.InitGrid_search_fix(grdSub2);

            //clsStyle.Style.InitGrid_search_Mor(grdMain1);

            InitGrd_STR1(grdMain1);

            InitGrd_STR5(grdMain2);

            InitGrd_STR2(grdSub1);

            //clsStyle.Style.InitGrid_search(grdSub1);
            
        }
        #endregion

        private void InitGrd_STR1(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search_Mor(grdMain1); 

            grd.AllowEditing = false;

            grd.Rows[0].Height = 40;

            #region 테스트..

            grd.Cols["NO"].Caption = "No.";
            grd.Cols["HEAT"].Caption = "HEAT";
            grd.Cols["PO_NO"].Caption = "POC";
            grd.Cols["STEEL"].Caption = "강종";
            grd.Cols["STEEL_NAME"].Caption = "강종명";
            grd.Cols["SIZE_NAME"].Caption = "규격";
            grd.Cols["LENGTH"].Caption = "길이";
            grd.Cols["MARKET_TYPE"].Caption = "구분";
            grd.Cols["TOTAL_CNT"].Caption = "총 번들수";
            grd.Cols["PRODUCT_CNT"].Caption = "입고 번들수";
            grd.Cols["WEIGHT"].Caption = "입고 중량";



            grd.Cols["NO"].Width = 0;
            grd.Cols["HEAT"].Width = 180;
            grd.Cols["PO_NO"].Width = 180;
            grd.Cols["STEEL"].Width = 80;
            grd.Cols["STEEL_NAME"].Width = 200;
            grd.Cols["SIZE_NAME"].Width = 70;
            grd.Cols["LENGTH"].Width = 120;
            grd.Cols["MARKET_TYPE"].Width = 90;
            grd.Cols["TOTAL_CNT"].Width = 140;
            grd.Cols["PRODUCT_CNT"].Width = 160;
            grd.Cols["WEIGHT"].Width = 90;


            grd.Rows.DefaultSize = 36;


            //grd.Cols["POINT"].TextAlign = cs.Angle_Value_TextAlign;


            #endregion


        }

        private void InitGrd_STR5(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search_Mor(grdMain2);

            grd.AllowEditing = false;

            grd.Rows[0].Height = 40;

            #region 테스트..

            grd.Cols["NO"].Caption = "No.";
            grd.Cols["COMPANY_NAME"].Caption = "유통사(상사)";
            grd.Cols["COMPANY_NAME2"].Caption = "End User";
            grd.Cols["T_BND_CNT"].Caption = "예상번들수";
            grd.Cols["T_BND_WGT"].Caption = "예상중량";
            grd.Cols["COLOR"].Caption = "색상코드";


            grd.Cols["NO"].Width = 0;
            grd.Cols["COMPANY_NAME"].Width = 370;
            grd.Cols["COMPANY_NAME2"].Width = 360;
            grd.Cols["T_BND_CNT"].Width = 220;
            grd.Cols["T_BND_WGT"].Width = 220;
            grd.Cols["COLOR"].Width = 70;


            grd.Rows.DefaultSize = 36;


            //grd.Cols["POINT"].TextAlign = cs.Angle_Value_TextAlign;


            #endregion


        }


        private void InitGrd_STR3(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search_Mor_New(grdMain2);

            //grd.AllowEditing = false;

            grd.Rows[0].Height = 40;

           
            //grd.Cols["NO"].Caption = "No.";
            //grd.Cols["BUNDLE_NO"].Caption = "제품번호";
            //grd.Cols["MARKET_TYPE"].Caption = "구분";
            //grd.Cols["COMPANY_SEQ"].Caption = "유통사(상사)";
            //grd.Cols["COMPANY_SEQ2"].Caption = "End User";
            //grd.Cols["WEIGHT"].Caption = "중량";
            //grd.Cols["RB_USED"].Caption = "용도";
            //grd.Cols["COMPLETE_YON"].Caption = "진행";



            //grd.Cols["NO"].Width = 50;
            //grd.Cols["BUNDLE_NO"].Width = 160;
            //grd.Cols["MARKET_TYPE"].Width = 70;
            //grd.Cols["COMPANY_SEQ"].Width = 350;
            //grd.Cols["COMPANY_SEQ2"].Width = 330;
            //grd.Cols["WEIGHT"].Width = 70;
            //grd.Cols["RB_USED"].Width = 120;
            //grd.Cols["COMPLETE_YON"].Width = 80;


            //grd.Rows.DefaultSize = 42;

            //grd.Cols["POINT"].TextAlign = cs.Angle_Value_TextAlign;



        }

        private void InitGrd_STR2(C1FlexGrid grd)
        {
            clsStyle.Style.InitGrid_search_Mor(grdSub1);

            grd.AllowEditing = false;

            grd.Rows[0].Height = 40;

            #region 테스트..

            grd.Cols["NO"].Caption = "No.";
            grd.Cols["BUNDLE_NO"].Caption = "제품번호";
            grd.Cols["MARKET_TYPE"].Caption = "구분";
            grd.Cols["COMPANY_SEQ"].Caption = "유통사(상사)";
            grd.Cols["COMPANY_SEQ2"].Caption = "End User";
            grd.Cols["WEIGHT"].Caption = "중량";
            grd.Cols["RB_USED"].Caption = "용도";
            grd.Cols["COMPLETE_YON"].Caption = "진행";


            grd.Cols["NO2"].Width = 0;
            grd.Cols["NO"].Width =50;
            grd.Cols["BUNDLE_NO"].Width = 160;
            grd.Cols["MARKET_TYPE"].Width = 70;
            grd.Cols["COMPANY_SEQ"].Width = 350;
            grd.Cols["COMPANY_SEQ2"].Width = 330;
            grd.Cols["WEIGHT"].Width = 145;
            grd.Cols["RB_USED"].Width = 145;
            grd.Cols["COMPLETE_YON"].Width = 80;

            grd.Rows.DefaultSize = 38;


            //grd.Cols["POINT"].TextAlign = cs.Angle_Value_TextAlign;


            #endregion


        }

        //조회버튼 클릭
        public void btnDisplay_Click(object sender, EventArgs e)
        {
            if(cboLine.Text == "")
            {
                MessageBox.Show("조회 버튼 옆 라인을 선택 후 조회하세요.");
                return;
            }

            if (cboPoc.Text == "")
            {
                MessageBox.Show("Poc 상태를 선택 후 조회하세요.");
                return;
            }

            //c_scrollPosition = 0;

            //MessageBox.Show(c_scrollPosition.ToString());

            this.Cursor = Cursors.WaitCursor;

            SetDataBinding();

            //SetDataBinding2();

            this.Cursor = Cursors.Default;

            chkTimerOnOff();
        }

        #region "쿼리데이터 그리드 바인딩"
        private void SetDataBinding()
        {
            try
            {
                string pLine = string.Empty;

                if (cboLine.SelectedIndex.ToString() == "0")
                {
                    pLine = "#1";
                }
                else if (cboLine.SelectedIndex.ToString() == "1")
                {
                    pLine = "#2";
                }
                else if (cboLine.SelectedIndex.ToString() == "2")
                {
                    pLine = "#3";
                }
                else
                {
                    pLine = "";
                }

                string sql = string.Empty;
                if (cboPoc.SelectedIndex.ToString() == "0")
                {
                    sql = string.Format("  SELECT  HEAT, PO_NO, STEEL, STEEL_NAME, ITEM_SIZE, SIZE_NAME, LENGTH, MARKET_TYPE, TOTAL_CNT, COUNT(BUNDLE_NO) PRODUCT_CNT, TO_CHAR(NVL(SUM(WEIGHT),0), '999,999') WEIGHT  ");
                    sql += string.Format(" FROM    ( ");
                    sql += string.Format("          WITH WORK_ORDER AS ");
                    sql += string.Format("          ( ");
                    sql += string.Format("          SELECT  A.HEAT, A.PO_NO, A.STEEL, A.ITEM, A.ITEM_SIZE, A.LENGTH, B.MARKET_TYPE, COUNT(B.WORK_ORDER_ROUTING_LINE)*2 AS BUNDLE_CNT ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER A, BARCODE_WORK_ORDER_ROUTING B, TB_PROG_POC_MGMT C ");
                    sql += string.Format("          WHERE   A.PO_NO = B.PO_NO  ");
                    sql += string.Format("          AND     A.PO_NO = C.POC_NO  ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          GROUP BY A.HEAT, A.PO_NO, A.STEEL, A.ITEM, A.ITEM_SIZE, A.LENGTH, B.MARKET_TYPE ");
                    sql += string.Format("          ) ");
                    sql += string.Format("          SELECT  X.HEAT, X.PO_NO, X.STEEL, S.STEEL_NAME, X.ITEM_SIZE, V.SIZE_NAME, X.LENGTH, DECODE(X.MARKET_TYPE,'1511','내수','1522','수출',X.MARKET_TYPE) MARKET_TYPE ");
                    sql += string.Format("                  , X.BUNDLE_CNT AS TOTAL_CNT, Z.BUNDLE_NO, Z.WEIGHT ");
                    sql += string.Format("          FROM    WORK_ORDER X, BARCODE_WORK_ORDER_ROUTING Y, BARCODE_INSPECT Z, TB_ITEM_SIZE_V@ISPDB V, TB_STEEL_TYPE_V@ISPDB S ");
                    sql += string.Format("          WHERE   X.PO_NO = Y.PO_NO ");
                    sql += string.Format("          AND     X.ITEM = V.ITEM ");
                    sql += string.Format("          AND     X.ITEM = S.ITEM ");
                    sql += string.Format("          AND     X.ITEM_SIZE = V.ITEM_SIZE ");
                    sql += string.Format("          AND     X.STEEL = S.STEEL  ");
                    sql += string.Format("          AND     X.PO_NO = Z.PO_NO(+) ");
                    sql += string.Format("          AND     X.MARKET_TYPE = Z.MARKET_TYPE(+) ");
                    sql += string.Format("          GROUP BY X.HEAT, X.PO_NO, X.STEEL, S.STEEL_NAME, X.ITEM_SIZE, V.SIZE_NAME, X.LENGTH, X.MARKET_TYPE, X.BUNDLE_CNT , Z.BUNDLE_NO, Z.WEIGHT ");
                    sql += string.Format("         ) ");
                    sql += string.Format(" GROUP BY HEAT, PO_NO, STEEL, STEEL_NAME, ITEM_SIZE, SIZE_NAME, LENGTH, MARKET_TYPE, TOTAL_CNT ");
                }
                else if (cboPoc.SelectedIndex.ToString() == "1")
                {
                    sql = string.Format("  SELECT  HEAT, PO_NO, STEEL, STEEL_NAME, ITEM_SIZE, SIZE_NAME, LENGTH, MARKET_TYPE, TOTAL_CNT, COUNT(BUNDLE_NO) PRODUCT_CNT, TO_CHAR(NVL(SUM(WEIGHT),0), '999,999') WEIGHT  ");
                    sql += string.Format(" FROM    ( ");
                    sql += string.Format("          WITH WORK_ORDER AS ");
                    sql += string.Format("          ( ");
                    sql += string.Format("          SELECT  A.HEAT, A.PO_NO, A.STEEL, A.ITEM, A.ITEM_SIZE, A.LENGTH, B.MARKET_TYPE, COUNT(B.WORK_ORDER_ROUTING_LINE)*2 AS BUNDLE_CNT ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER A, BARCODE_WORK_ORDER_ROUTING B, TB_PROG_POC_MGMT C ");
                    sql += string.Format("          WHERE   A.PO_NO = B.PO_NO  ");
                    sql += string.Format("          AND     A.PO_NO = C.BEF_POC_NO  ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          GROUP BY A.HEAT, A.PO_NO, A.STEEL, A.ITEM, A.ITEM_SIZE, A.LENGTH, B.MARKET_TYPE ");
                    sql += string.Format("          ) ");
                    sql += string.Format("          SELECT  X.HEAT, X.PO_NO, X.STEEL, S.STEEL_NAME, X.ITEM_SIZE, V.SIZE_NAME, X.LENGTH, DECODE(X.MARKET_TYPE,'1511','내수','1522','수출',X.MARKET_TYPE) MARKET_TYPE ");
                    sql += string.Format("                  , X.BUNDLE_CNT AS TOTAL_CNT, Z.BUNDLE_NO, Z.WEIGHT ");
                    sql += string.Format("          FROM    WORK_ORDER X, BARCODE_WORK_ORDER_ROUTING Y, BARCODE_INSPECT Z, TB_ITEM_SIZE_V@ISPDB V, TB_STEEL_TYPE_V@ISPDB S ");
                    sql += string.Format("          WHERE   X.PO_NO = Y.PO_NO ");
                    sql += string.Format("          AND     X.ITEM = V.ITEM ");
                    sql += string.Format("          AND     X.ITEM = S.ITEM ");
                    sql += string.Format("          AND     X.ITEM_SIZE = V.ITEM_SIZE ");
                    sql += string.Format("          AND     X.STEEL = S.STEEL  ");
                    sql += string.Format("          AND     X.PO_NO = Z.PO_NO(+) ");
                    sql += string.Format("          AND     X.MARKET_TYPE = Z.MARKET_TYPE(+) ");
                    sql += string.Format("          GROUP BY X.HEAT, X.PO_NO, X.STEEL, S.STEEL_NAME, X.ITEM_SIZE, V.SIZE_NAME, X.LENGTH, X.MARKET_TYPE, X.BUNDLE_CNT , Z.BUNDLE_NO, Z.WEIGHT ");
                    sql += string.Format("         ) ");
                    sql += string.Format(" GROUP BY HEAT, PO_NO, STEEL, STEEL_NAME, ITEM_SIZE, SIZE_NAME, LENGTH, MARKET_TYPE, TOTAL_CNT ");
                }
                

                string[] parm = new string[1];
                parm[0] = "";

                //olddt = cd.FindDataTable("MAIN", sql, parm);
                olddt = cd.FindDataTable(sql, parm);

                moddt = olddt.Copy();

                grdMain1.SetDataBinding(moddt, null, true);

                if (olddt.Rows.Count > 0)
                {
                    poc_no_nm = moddt.Rows[0]["MARKET_TYPE"].ToString();

                    if (poc_no_nm == "수출")
                    {
                        InitGrd_STR3(grdMain2);
                        //txtMemo1.Text = "수출품이 포함된 POC 입니다.";

                    }
                    else
                    {
                        InitGrd_STR5(grdMain2);
                        //txtMemo1.Text = "";
                    }

                    SetDataBinding2();
                    SetDataBindingSub();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }
            return;
        }
        #endregion

        #region "쿼리데이터 그리드 바인딩"
        private void SetDataBinding2()
        {
            try
            {
                string pLine = string.Empty;

                if (cboLine.SelectedIndex.ToString() == "0")
                {
                    pLine = "#1";
                }
                else if (cboLine.SelectedIndex.ToString() == "1")
                {
                    pLine = "#2";
                }
                else if (cboLine.SelectedIndex.ToString() == "2")
                {
                    pLine = "#3";
                }
                else
                {
                    pLine = "";
                }
                string sql = string.Empty;
                if (cboPoc.SelectedIndex.ToString() == "0")
                {
                    sql = string.Format("  SELECT  COMPANY_NAME, COMPANY_NAME2,  ");
                    sql += string.Format(" CASE WHEN REQ_WGT <= ORD_WGT THEN CASE WHEN (BND_WGT * T_BND_CNT) < REQ_WGT THEN T_BND_CNT + 1 ELSE T_BND_CNT END ELSE T_BND_CNT END T_BND_CNT,");
                    sql += string.Format(" BND_WGT * (CASE WHEN REQ_WGT < ORD_WGT THEN CASE WHEN (BND_WGT * T_BND_CNT) < REQ_WGT THEN T_BND_CNT + 1 ELSE T_BND_CNT END ELSE T_BND_CNT END) T_BND_WGT, ");
                    sql += string.Format(" COLOR ");
                    sql += string.Format(" FROM    ( ");
                    sql += string.Format("          WITH T_WEIGHT AS ( ");
                    sql += string.Format("          SELECT  A.PO_NO, A.ITEM, A.ITEM_SIZE, A.STEEL, A.LENGTH, (ROUND(FN_GETWEIGHT_ROM@ISPDB(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.UOM, B.BND_PCS,'BON','BWB'),0) * B.BND_PCS)/1000 AS BND_WGT  ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER A, TB_BUNDLE_PIECESTD B, TB_PROG_POC_MGMT C  ");
                    sql += string.Format("          WHERE   A.PO_NO = C.POC_NO  ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          AND     A.ITEM = B.ITEM  ");
                    sql += string.Format("          AND     A.ITEM_SIZE = B.ITEM_SIZE ");
                    sql += string.Format("          AND     A.LENGTH = B.LENGTH * 100 ) ,  ");
                    sql += string.Format("          ROUTING AS ( ");
                    sql += string.Format("          SELECT  A.PO_NO, MAX(A.WORK_ORDER_ROUTING_LINE) * 2 AS BND_CNT ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_ROUTING A, TB_PROG_POC_MGMT B ");
                    sql += string.Format("          WHERE   A.PO_NO = B.POC_NO ");
                    sql += string.Format("          AND     B.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          GROUP BY A.PO_NO ) ,  ");
                    sql += string.Format("          MAPPING_INFO AS ( ");
                    sql += string.Format("          SELECT  A.POC_NO, A.REQ_WGT, A.ORD_WGT, A.ORD_NO, A.ORD_SEQ, A.LOT_NO, A.LOT_SEQ, ");
                    sql += string.Format("                  B.C_CUST_CD AS COMPANY_SEQ, FN_BARCODE_GETCOMPANY(B.C_CUST_CD) AS COMPANY_NAME, ");
                    sql += string.Format("                  B.DEMANDER_CD AS COMPANY_SEQ2, FN_BARCODE_GETCOMPANY(B.DEMANDER_CD) AS COMPANY_NAME2, B.SPST_SECT_CLR ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_MAPPING A, TB_WORK_ORDER_EUSR_INFO@ISPDB B, TB_PROG_POC_MGMT C  ");
                    sql += string.Format("          WHERE   A.POC_NO = C.POC_NO  ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          AND     A.ORD_NO = B.ORD_NO ");
                    sql += string.Format("          AND     A.ORD_SEQ = B.ORD_DTL ) , ");
                    sql += string.Format("          MAPPING_REQ AS ( ");
                    sql += string.Format("          SELECT  POC_NO, REQ_WGT, ORD_NO, ORD_SEQ, SUM(ORD_WGT) AS ORD_WGT, SUM(SUM_WGT) AS SUM_WGT ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_MAPPING ");
                    sql += string.Format("          WHERE   ORD_NO||ORD_SEQ IN ( ");
                    sql += string.Format("                                      SELECT  A.ORD_NO||A.ORD_SEQ  ");
                    sql += string.Format("                                      FROM    BARCODE_WORK_ORDER_MAPPING A, TB_PROG_POC_MGMT B  ");
                    sql += string.Format("                                      WHERE   A.POC_NO = B.POC_NO  ");
                    sql += string.Format("                                      AND     B.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("                                     )  ");
                    sql += string.Format("          GROUP BY POC_NO, REQ_WGT, ORD_NO, ORD_SEQ ");
                    sql += string.Format("                          )  ");
                    sql += string.Format("          SELECT  A.PO_NO, A.ITEM, A.ITEM_SIZE, A.STEEL, A.LENGTH, A.BND_WGT, B.BND_CNT, BND_WGT * BND_CNT AS TOTAL_BND_WGT, ");
                    sql += string.Format("                  C.REQ_WGT, C.ORD_WGT, C.ORD_NO, C.ORD_SEQ, C.LOT_NO, C.LOT_SEQ, C.COMPANY_NAME, C.COMPANY_NAME2,  ");
                    sql += string.Format("                  CASE WHEN BND_CNT < ROUND(CASE WHEN C.REQ_WGT < C.ORD_WGT THEN C.REQ_WGT ELSE C.ORD_WGT END / BND_WGT) THEN BND_CNT ELSE ROUND(CASE WHEN C.REQ_WGT < C.ORD_WGT THEN C.REQ_WGT ELSE C.ORD_WGT END / BND_WGT) END T_BND_CNT, ");
                    sql += string.Format("                  REPLACE(C.SPST_SECT_CLR,'3K','') COLOR");
                    sql += string.Format("          FROM    T_WEIGHT A, ROUTING B, MAPPING_INFO C ");
                    sql += string.Format("          WHERE   A.PO_NO = B.PO_NO  ");
                    sql += string.Format("          AND     A.PO_NO = C.POC_NO ");
                    sql += string.Format("          )  ");
                    sql += string.Format("          ORDER BY LOT_NO, LOT_SEQ ");
                }
                else if (cboPoc.SelectedIndex.ToString() == "1")
                {
                    sql = string.Format("  SELECT  COMPANY_NAME, COMPANY_NAME2,  ");
                    sql += string.Format(" CASE WHEN REQ_WGT <= ORD_WGT THEN CASE WHEN (BND_WGT * T_BND_CNT) < REQ_WGT THEN T_BND_CNT + 1 ELSE T_BND_CNT END ELSE T_BND_CNT END T_BND_CNT,");
                    sql += string.Format(" BND_WGT * (CASE WHEN REQ_WGT < ORD_WGT THEN CASE WHEN (BND_WGT * T_BND_CNT) < REQ_WGT THEN T_BND_CNT + 1 ELSE T_BND_CNT END ELSE T_BND_CNT END) T_BND_WGT, ");
                    sql += string.Format(" COLOR ");
                    sql += string.Format(" FROM    ( ");
                    sql += string.Format("          WITH T_WEIGHT AS ( ");
                    sql += string.Format("          SELECT  A.PO_NO, A.ITEM, A.ITEM_SIZE, A.STEEL, A.LENGTH, (ROUND(FN_GETWEIGHT_ROM@ISPDB(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.UOM, B.BND_PCS,'BON','BWB'),0) * B.BND_PCS)/1000 AS BND_WGT  ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER A, TB_BUNDLE_PIECESTD B, TB_PROG_POC_MGMT C  ");
                    sql += string.Format("          WHERE   A.PO_NO = C.BEF_POC_NO  ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          AND     A.ITEM = B.ITEM  ");
                    sql += string.Format("          AND     A.ITEM_SIZE = B.ITEM_SIZE ");
                    sql += string.Format("          AND     A.LENGTH = B.LENGTH * 100 ) ,  ");
                    sql += string.Format("          ROUTING AS ( ");
                    sql += string.Format("          SELECT  A.PO_NO, MAX(A.WORK_ORDER_ROUTING_LINE) * 2 AS BND_CNT ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_ROUTING A, TB_PROG_POC_MGMT B ");
                    sql += string.Format("          WHERE   A.PO_NO = B.BEF_POC_NO ");
                    sql += string.Format("          AND     B.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          GROUP BY A.PO_NO ) ,  ");
                    sql += string.Format("          MAPPING_INFO AS ( ");
                    sql += string.Format("          SELECT  A.POC_NO, A.REQ_WGT, A.ORD_WGT, A.ORD_NO, A.ORD_SEQ, A.LOT_NO, A.LOT_SEQ, ");
                    sql += string.Format("                  B.C_CUST_CD AS COMPANY_SEQ, FN_BARCODE_GETCOMPANY(B.C_CUST_CD) AS COMPANY_NAME, ");
                    sql += string.Format("                  B.DEMANDER_CD AS COMPANY_SEQ2, FN_BARCODE_GETCOMPANY(B.DEMANDER_CD) AS COMPANY_NAME2, B.SPST_SECT_CLR ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_MAPPING A, TB_WORK_ORDER_EUSR_INFO@ISPDB B, TB_PROG_POC_MGMT C  ");
                    sql += string.Format("          WHERE   A.POC_NO = C.BEF_POC_NO  ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("          AND     A.ORD_NO = B.ORD_NO ");
                    sql += string.Format("          AND     A.ORD_SEQ = B.ORD_DTL ) , ");
                    sql += string.Format("          MAPPING_REQ AS ( ");
                    sql += string.Format("          SELECT  POC_NO, REQ_WGT, ORD_NO, ORD_SEQ, SUM(ORD_WGT) AS ORD_WGT, SUM(SUM_WGT) AS SUM_WGT ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_MAPPING ");
                    sql += string.Format("          WHERE   ORD_NO||ORD_SEQ IN ( ");
                    sql += string.Format("                                      SELECT  A.ORD_NO||A.ORD_SEQ  ");
                    sql += string.Format("                                      FROM    BARCODE_WORK_ORDER_MAPPING A, TB_PROG_POC_MGMT B  ");
                    sql += string.Format("                                      WHERE   A.POC_NO = B.BEF_POC_NO  ");
                    sql += string.Format("                                      AND     B.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format("                                     )  ");
                    sql += string.Format("          GROUP BY POC_NO, REQ_WGT, ORD_NO, ORD_SEQ ");
                    sql += string.Format("                          )  ");
                    sql += string.Format("          SELECT  A.PO_NO, A.ITEM, A.ITEM_SIZE, A.STEEL, A.LENGTH, A.BND_WGT, B.BND_CNT, BND_WGT * BND_CNT AS TOTAL_BND_WGT, ");
                    sql += string.Format("                  C.REQ_WGT, C.ORD_WGT, C.ORD_NO, C.ORD_SEQ, C.LOT_NO, C.LOT_SEQ, C.COMPANY_NAME, C.COMPANY_NAME2,  ");
                    sql += string.Format("                  CASE WHEN BND_CNT < ROUND(CASE WHEN C.REQ_WGT < C.ORD_WGT THEN C.REQ_WGT ELSE C.ORD_WGT END / BND_WGT) THEN BND_CNT ELSE ROUND(CASE WHEN C.REQ_WGT < C.ORD_WGT THEN C.REQ_WGT ELSE C.ORD_WGT END / BND_WGT) END T_BND_CNT, ");
                    sql += string.Format("                  REPLACE(C.SPST_SECT_CLR,'3K','') COLOR");
                    sql += string.Format("          FROM    T_WEIGHT A, ROUTING B, MAPPING_INFO C ");
                    sql += string.Format("          WHERE   A.PO_NO = B.PO_NO  ");
                    sql += string.Format("          AND     A.PO_NO = C.POC_NO ");
                    sql += string.Format("          )  ");
                    sql += string.Format("          ORDER BY LOT_NO, LOT_SEQ ");
                }


               
                

                string[] parm = new string[1];
                parm[0] = "";

                //olddt = cd.FindDataTable("MAIN", sql, parm);
                olddt = cd.FindDataTable(sql, parm);

                moddt = olddt.Copy();

                grdMain2.SetDataBinding(moddt, null, true);

                if (olddt.Rows.Count > 0)
                {
                    //InitGrd_STR3(grdMain2);
                    //txtMemo1.Text = "수출품이 포함된 POC 입니다.";
                }
                else
                {
                    //InitGrd_STR5(grdMain2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }
            return;
        }
        #endregion

        #region "쿼리데이터 그리드 바인딩"
        private void SetDataBinding3()
        {
            try
            {
                string pLine = string.Empty;

                if (cboLine.SelectedIndex.ToString() == "0")
                {
                    pLine = "#1";
                }
                else if (cboLine.SelectedIndex.ToString() == "1")
                {
                    pLine = "#2";
                }
                else if (cboLine.SelectedIndex.ToString() == "2")
                {
                    pLine = "#3";
                }
                else
                {
                    pLine = "";
                }

                string sql = string.Empty;
                sql = string.Format("  SELECT  FN_BARCODE_GETCOMPANY(B.C_CUST_CD) AS COMPANY_NAME,  ");
                sql += string.Format(" FN_BARCODE_GETCOMPANY(B.DEMANDER_CD) AS COMPANY_NAME2,  20 AS T_BND_CNT,  2450 AS T_BND_WGT,  'YE' AS COLOR ");
                sql += string.Format(" FROM    BARCODE_WORK_ORDER_MAPPING A, TB_WORK_ORDER_EUSR_INFO@ISPDB B, TB_PROG_POC_MGMT C ");
                sql += string.Format(" WHERE    A.ORD_NO = B.ORD_NO ");
                sql += string.Format(" AND     A.ORD_SEQ = B.ORD_DTL ");
                sql += string.Format(" AND    A.LOT_NO = 'P180400388' ");
               
                string[] parm = new string[1];
                parm[0] = "";

                //olddt = cd.FindDataTable("MAIN", sql, parm);
                olddt = cd.FindDataTable(sql, parm);

                moddt = olddt.Copy();

                grdMain2.SetDataBinding(moddt, null, true);

                if (olddt.Rows.Count > 0)
                {
                    InitGrd_STR3(grdMain2);
                    //txtMemo1.Text = "수출품이 포함된 POC 입니다.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }
            return;
        }
        #endregion

        #region "GRD DETAIL 쿼리데이터 그리드 바인딩"
        private void SetDataBindingSub()
        {
            try
            {
                string pLine = string.Empty;

                if (cboLine.SelectedIndex.ToString() == "0")
                {
                    pLine = "#1";
                }
                else if (cboLine.SelectedIndex.ToString() == "1")
                {
                    pLine = "#2";
                }
                else if (cboLine.SelectedIndex.ToString() == "2")
                {
                    pLine = "#3";
                }
                else
                {
                    pLine = "";
                }

                string sql = string.Empty;

                if (cboPoc.SelectedIndex.ToString() == "0")
                {
                    sql = string.Format(" WITH INSPECT AS ");
                    sql += string.Format(" ( ");
                    sql += string.Format(" SELECT  B.BUNDLE_NO, B.MARKET_TYPE, B.COMPANY_SEQ, B.COMPANY_SEQ2, B.WEIGHT, B.RB_USED ");
                    sql += string.Format(" FROM    BARCODE_WORK_ORDER_ROUTING A, BARCODE_INSPECT B, TB_PROG_POC_MGMT C ");
                    sql += string.Format(" WHERE   A.PO_NO = C.POC_NO ");
                    sql += string.Format(" AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format(" AND     A.PO_NO = B.PO_NO ");
                    sql += string.Format(" AND     A.MARKET_TYPE = B.MARKET_TYPE ");
                    sql += string.Format(" AND     SUBSTR(SUBSTR(B.BUNDLE_NO, -3, 3),1,2) = CASE WHEN LENGTH(A.WORK_ORDER_ROUTING_LINE) = 1 THEN '0'||A.WORK_ORDER_ROUTING_LINE ELSE TO_CHAR(A.WORK_ORDER_ROUTING_LINE) END ");
                    sql += string.Format(" AND     B.ITEM IN ('OB','RB','SB') ");
                    sql += string.Format(" AND     B.INSPECT_TYPE = 'D3PR' ");
                    sql += string.Format(" AND     B.INSPECT_STATUS = 'Y' ");
                    sql += string.Format(" AND     B.SIRF_CODE = '24A' ");
                    sql += string.Format(" ) ");
                    sql += string.Format(" SELECT  (RANK() OVER(ORDER BY X.BUNDLE_NO)) AS NO, X.BUNDLE_NO, DECODE(X.MARKET_TYPE,'1511','내수','1522','수출',X.MARKET_TYPE) MARKET_TYPE ");
                    sql += string.Format("          , CASE WHEN Y.COMPANY_SEQ IS NULL THEN FN_BARCODE_GETCOMPANY(X.COMPANY_SEQ) ELSE FN_BARCODE_GETCOMPANY(Y.COMPANY_SEQ) END COMPANY_SEQ, FN_BARCODE_GETCOMPANY(Y.COMPANY_SEQ2) COMPANY_SEQ2, TO_CHAR(Y.WEIGHT, '999,999') WEIGHT ");
                    sql += string.Format("          , FN_BARCODE_GETCOMNAME(Y.RB_USED) RB_USED, CASE WHEN Y.WEIGHT > 0 THEN '완료' ELSE '미완료' END COMPLETE_YON ");
                    sql += string.Format(" FROM    ( ");
                    sql += string.Format("          SELECT  A.PO_NO||CASE WHEN LENGTH(A.WORK_ORDER_ROUTING_LINE) = 2 THEN TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'A' ELSE '0'||TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'A' END BUNDLE_NO  ");
                    sql += string.Format("                  , A.MARKET_TYPE, A.COMPANY_SEQ, '' COMPANY_SEQ2, 0 WEIGHT, '' RB_USED ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_ROUTING A, TB_PROG_POC_MGMT C ");
                    sql += string.Format("          WHERE   A.PO_NO = C.POC_NO ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}' ", pLine);
                    sql += string.Format("          UNION ALL ");
                    sql += string.Format("          SELECT  A.PO_NO||CASE WHEN LENGTH(A.WORK_ORDER_ROUTING_LINE) = 2 THEN TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'B' ELSE '0'||TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'B' END BUNDLE_NO ");
                    sql += string.Format("                  , A.MARKET_TYPE, A.COMPANY_SEQ, '' COMPANY_SEQ2, 0 WEIGHT, '' RB_USED  ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_ROUTING A, TB_PROG_POC_MGMT C ");
                    sql += string.Format("          WHERE   A.PO_NO = C.POC_NO ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}' ", pLine);
                    sql += string.Format("          ) X, INSPECT Y ");
                    sql += string.Format("  WHERE   X.BUNDLE_NO = Y.BUNDLE_NO(+) ");
                    sql += string.Format("  ORDER BY X.BUNDLE_NO ");
                }
                else if (cboPoc.SelectedIndex.ToString() == "1")
                {
                    sql = string.Format(" WITH INSPECT AS ");
                    sql += string.Format(" ( ");
                    sql += string.Format(" SELECT  B.BUNDLE_NO, B.MARKET_TYPE, B.COMPANY_SEQ, B.COMPANY_SEQ2, B.WEIGHT, B.RB_USED ");
                    sql += string.Format(" FROM    BARCODE_WORK_ORDER_ROUTING A, BARCODE_INSPECT B, TB_PROG_POC_MGMT C ");
                    sql += string.Format(" WHERE   A.PO_NO = C.BEF_POC_NO ");
                    sql += string.Format(" AND     C.LINE_GP = '{0}'  ", pLine);
                    sql += string.Format(" AND     A.PO_NO = B.PO_NO ");
                    sql += string.Format(" AND     A.MARKET_TYPE = B.MARKET_TYPE ");
                    sql += string.Format(" AND     SUBSTR(SUBSTR(B.BUNDLE_NO, -3, 3),1,2) = CASE WHEN LENGTH(A.WORK_ORDER_ROUTING_LINE) = 1 THEN '0'||A.WORK_ORDER_ROUTING_LINE ELSE TO_CHAR(A.WORK_ORDER_ROUTING_LINE) END ");
                    sql += string.Format(" AND     B.ITEM IN ('OB','RB','SB') ");
                    sql += string.Format(" AND     B.INSPECT_TYPE = 'D3PR' ");
                    sql += string.Format(" AND     B.INSPECT_STATUS = 'Y' ");
                    sql += string.Format(" AND     B.SIRF_CODE = '24A' ");
                    sql += string.Format(" ) ");
                    sql += string.Format(" SELECT  (RANK() OVER(ORDER BY X.BUNDLE_NO)) AS NO, X.BUNDLE_NO, DECODE(X.MARKET_TYPE,'1511','내수','1522','수출',X.MARKET_TYPE) MARKET_TYPE ");
                    sql += string.Format("          , CASE WHEN Y.COMPANY_SEQ IS NULL THEN FN_BARCODE_GETCOMPANY(X.COMPANY_SEQ) ELSE FN_BARCODE_GETCOMPANY(Y.COMPANY_SEQ) END COMPANY_SEQ, FN_BARCODE_GETCOMPANY(Y.COMPANY_SEQ2) COMPANY_SEQ2, TO_CHAR(Y.WEIGHT, '999,999') WEIGHT ");
                    sql += string.Format("          , FN_BARCODE_GETCOMNAME(Y.RB_USED) RB_USED, CASE WHEN Y.WEIGHT > 0 THEN '완료' ELSE '미완료' END COMPLETE_YON ");
                    sql += string.Format(" FROM    ( ");
                    sql += string.Format("          SELECT  A.PO_NO||CASE WHEN LENGTH(A.WORK_ORDER_ROUTING_LINE) = 2 THEN TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'A' ELSE '0'||TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'A' END BUNDLE_NO  ");
                    sql += string.Format("                  , A.MARKET_TYPE, A.COMPANY_SEQ, '' COMPANY_SEQ2, 0 WEIGHT, '' RB_USED ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_ROUTING A, TB_PROG_POC_MGMT C ");
                    sql += string.Format("          WHERE   A.PO_NO = C.BEF_POC_NO ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}' ", pLine);
                    sql += string.Format("          UNION ALL ");
                    sql += string.Format("          SELECT  A.PO_NO||CASE WHEN LENGTH(A.WORK_ORDER_ROUTING_LINE) = 2 THEN TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'B' ELSE '0'||TO_CHAR(A.WORK_ORDER_ROUTING_LINE)||'B' END BUNDLE_NO ");
                    sql += string.Format("                  , A.MARKET_TYPE, A.COMPANY_SEQ, '' COMPANY_SEQ2, 0 WEIGHT, '' RB_USED  ");
                    sql += string.Format("          FROM    BARCODE_WORK_ORDER_ROUTING A, TB_PROG_POC_MGMT C ");
                    sql += string.Format("          WHERE   A.PO_NO = C.BEF_POC_NO ");
                    sql += string.Format("          AND     C.LINE_GP = '{0}' ", pLine);
                    sql += string.Format("          ) X, INSPECT Y ");
                    sql += string.Format("  WHERE   X.BUNDLE_NO = Y.BUNDLE_NO(+) ");
                    sql += string.Format("  ORDER BY X.BUNDLE_NO ");
                }
                

                string[] parm = new string[3];
                //parm[0] = ":P_LINE|" + pLine;
                //parm[1] = ":P_LINE|" + pLine;
                //parm[2] = ":P_LINE|" + pLine;
                parm[0] = "";
                parm[1] = "";
                parm[2] = "";

                //olddt = cd.FindDataTable("MAIN", sql, parm);
                olddt = cd.FindDataTable(sql, parm);

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdSub1.SetDataBinding(moddt, null, true);

                if (olddt.Rows.Count > 0)
                {

                }

                ////this.Cursor = System.Windows.Forms.Cursors.Default;
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

                setScrollPosition3();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }
            return;
        }
        #endregion

        #region "쿼리데이터 그리드 바인딩"
        
        #endregion

        #region "GRD DETAIL 쿼리데이터 그리드 바인딩"
       
        #endregion


        private void chkTimerOnOff()
        {
            t.Tick -= new EventHandler(Timer_Tick);
            t.Interval = 15000;
            t.Tick += new EventHandler(Timer_Tick);
            t.Start();
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            btnDisplay_Click(this, new EventArgs());
            //t.Stop();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            t.Stop();
            this.Close();
        }

        private void setScrollPosition()
        {
            var grdCount = grdSub1.Rows.Count;


            if (grdCount <= 22)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if(grdCount >= 23 && grdCount <= 41)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(23-1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -492);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 42 && grdCount <= 61)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(42 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -492);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(42-1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -982);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                //MessageBox.Show(grdSub1.GetData(grdCount-1, "BUNDLE_NO").ToString());
            }
            else if (grdCount >= 62)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(42 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -492);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(42 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(62 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -982);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(62-1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1357);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
        }

        private void setScrollPosition3()
        {
            var grdCount = grdSub1.Rows.Count;


            if (grdCount <= 8)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 9 && grdCount <= 16)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(9 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -302);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 17 && grdCount <= 24)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(9 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -302);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -608);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                //MessageBox.Show(grdSub1.GetData(grdCount-1, "BUNDLE_NO").ToString());
            }
            else if (grdCount >= 25 && grdCount <= 32)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(9 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -302);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -608);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -912);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 33 && grdCount <= 40)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(9 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -302);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -608);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(33 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -912);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(33 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1216);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 41 && grdCount <= 48)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(9 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -302);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -608);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(33 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -912);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(33 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(41 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1216);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(41 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1522);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 49)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(9 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -302);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(17 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -608);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(25 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(33 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -912);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(33 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(41 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1216);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(41 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(49 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1522);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(49 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1830);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
        }

        private void c1Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }


        public static Form GetForm(string formName)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == formName)
                {
                    return frm;
                }
            }
            return null;

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
