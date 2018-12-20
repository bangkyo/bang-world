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
    public partial class frmRB_Monitoring4 : Form
    {
        Timer t = new Timer();

        public frmRB_Monitoring4()
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
        public frmRB_Monitoring4(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }
        #endregion

        private void frmRB_Monitoring4_Load(object sender, EventArgs e)
        {
            InitControl();
            this.WindowState = FormWindowState.Maximized;
        }

        #region "InitControl 설정"
        private void InitControl()
        {
            //clsStyle.Style.InitGrid_search_fix(grdMain1);
            //clsStyle.Style.InitGrid_search_fix(grdMain2);
            //clsStyle.Style.InitGrid_search_fix(grdSub1);
            //clsStyle.Style.InitGrid_search_fix(grdSub2);

            clsStyle.Style.InitGrid_search(grdMain1);
            clsStyle.Style.InitGrid_search(grdMain2);
            clsStyle.Style.InitGrid_search(grdSub1);
            clsStyle.Style.InitGrid_search(grdSub2);
        }
        #endregion

        //조회버튼 클릭
        public void btnDisplay_Click(object sender, EventArgs e)
        {
            if(cboLine.Text == "")
            {
                MessageBox.Show("조회 버튼 옆 라인을 선택 후 조회하세요.");
                return;
            }

            //c_scrollPosition = 0;

            //MessageBox.Show(c_scrollPosition.ToString());

            this.Cursor = Cursors.WaitCursor;

            SetDataBinding();

            SetDataBinding2();

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

                string[] parm = new string[1];
                parm[0] = "";

                //olddt = cd.FindDataTable("MAIN", sql, parm);
                olddt = cd.FindDataTable(sql, parm);

                moddt = olddt.Copy();

                grdMain1.SetDataBinding(moddt, null, true);

                if (olddt.Rows.Count > 0)
                {
                    if(grdMain1.Rows.Count > 2)
                    {
                        txtMemo1.Text = "내수, 수출이 같이 포함된 POC 입니다.";
                    }
                    else
                    {
                        txtMemo1.Text = "";
                    }

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

                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

                setScrollPositionnow();
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
                /*
                string sql = string.Empty;

                sql = string.Format("   WITH WORK_ORDER AS ");
                sql += string.Format("  ( ");
                sql += string.Format("  SELECT  A.HEAT, A.PO_NO, A.STEEL, A.ITEM, A.ITEM_SIZE, A.LENGTH, B.MARKET_TYPE, COUNT(B.WORK_ORDER_ROUTING_LINE)*2 AS BUNDLE_CNT ");
                sql += string.Format("  FROM    BARCODE_WORK_ORDER A, BARCODE_WORK_ORDER_ROUTING B, TB_PROG_POC_MGMT C ");
                sql += string.Format("  WHERE   A.PO_NO = B.PO_NO ");
                sql += string.Format("  AND     A.PO_NO = C.BEF_POC_NO  ");
                sql += string.Format("  AND     C.LINE_GP = '{0}' ", pLine);
                sql += string.Format("  GROUP BY A.HEAT, A.PO_NO, A.STEEL, A.ITEM, A.ITEM_SIZE, A.LENGTH, B.MARKET_TYPE ");
                sql += string.Format("  ), ");
                sql += string.Format("      INSPECT_CNT AS");
                sql += string.Format("  ( ");
                sql += string.Format("  SELECT  CASE WHEN COUNT(D.BUNDLE_NO) > 0 THEN 1 ELSE 0 END CNT ");
                sql += string.Format("  FROM    BARCODE_WORK_ORDER A, BARCODE_WORK_ORDER_ROUTING B, TB_PROG_POC_MGMT C, BARCODE_INSPECT D ");
                sql += string.Format("  WHERE   A.PO_NO = B.PO_NO ");
                sql += string.Format("  AND     A.PO_NO = C.BEF_POC_NO ");
                sql += string.Format("  AND     C.LINE_GP = '{0}' ", pLine);
                sql += string.Format("  AND     A.PO_NO = D.PO_NO ");
                sql += string.Format("  AND     D.INSPECT_DATE = TO_CHAR(SYSDATE, 'YYYYMMDD') ");
                sql += string.Format("  AND     D.ITEM IN ('OB','RB','SB') ");
                sql += string.Format("  AND     D.INSPECT_TYPE = 'D3PR' ");
                sql += string.Format("  AND     D.INSPECT_STATUS = 'Y' ");
                sql += string.Format("  AND     D.SIRF_CODE = '24A' ");
                sql += string.Format("  ) ");
                sql += string.Format("  SELECT  X.HEAT, X.PO_NO, X.STEEL, S.STEEL_NAME, X.ITEM_SIZE, V.SIZE_NAME, X.LENGTH, DECODE(X.MARKET_TYPE,'1511','내수','1522','수출',X.MARKET_TYPE) MARKET_TYPE ");
                sql += string.Format("          , X.BUNDLE_CNT AS TOTAL_CNT, COUNT(BUNDLE_NO) PRODUCT_CNT, TO_CHAR(NVL(SUM(Z.WEIGHT),0), '999,999') WEIGHT ");
                sql += string.Format("  FROM    WORK_ORDER X, BARCODE_WORK_ORDER_ROUTING Y, BARCODE_INSPECT Z, INSPECT_CNT C, TB_ITEM_SIZE_V@ISPDB V, TB_STEEL_TYPE_V@ISPDB S ");
                sql += string.Format("  WHERE   X.PO_NO = Y.PO_NO ");
                sql += string.Format("  AND     X.ITEM = V.ITEM ");
                sql += string.Format("  AND     X.ITEM = S.ITEM ");
                sql += string.Format("  AND     X.ITEM_SIZE = V.ITEM_SIZE ");
                sql += string.Format("  AND     X.STEEL = S.STEEL ");
                sql += string.Format("  AND     X.PO_NO = Z.PO_NO(+) ");
                sql += string.Format("  AND     X.MARKET_TYPE = Z.MARKET_TYPE(+) ");
                sql += string.Format("  AND     ((C.CNT = 1 AND Z.ITEM IN ('OB','RB','SB') AND Z.INSPECT_TYPE = 'D3PR' AND Z.INSPECT_STATUS = 'Y'  AND Z.SIRF_CODE = '24A' AND SUBSTR(SUBSTR(Z.BUNDLE_NO, -3, 3),1,2) = Y.WORK_ORDER_ROUTING_LINE) ");
                sql += string.Format("          OR (C.CNT = 0)) ");
                sql += string.Format("  GROUP BY X.HEAT, X.PO_NO, X.STEEL, S.STEEL_NAME, X.ITEM_SIZE, V.SIZE_NAME, X.LENGTH, X.MARKET_TYPE, X.BUNDLE_CNT ");
                
                string[] parm = new string[2];
                //parm[0] = ":P_LINE|" + pLine;
                //parm[1] = ":P_LINE|" + pLine;
                parm[0] = "";
                parm[1] = "";
                */

                string sql = string.Empty;
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

                string[] parm = new string[1];
                parm[0] = "";
                //olddt = cd.FindDataTable("MAIN", sql, parm);
                olddt = cd.FindDataTable(sql, parm);

                moddt = olddt.Copy();

                grdMain2.SetDataBinding(moddt, null, true);

                if (olddt.Rows.Count > 0)
                {
                    if (grdMain2.Rows.Count > 2)
                    {
                        txtMemo2.Text = "내수, 수출이 같이 포함된 POC 입니다.";
                    }
                    else
                    {
                        txtMemo2.Text = "";
                    }

                    SetDataBindingSub2();
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
        private void SetDataBindingSub2()
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
                
                grdSub2.SetDataBinding(moddt, null, true);

                if (olddt.Rows.Count > 0)
                {

                }

                //setScrollPosition2();
                setScrollPositionbefore();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }
            return;
        }
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
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition);
                }
            }
            else if(grdCount >= 23 && grdCount <= 41)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if(grdSub1.GetData(23-1, "COMPLETE_YON").ToString() == "완료")
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

        private void setScrollPositionnow()
        {
            var grdCount = grdSub1.Rows.Count;


            if (grdCount <= 11)
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
            else if (grdCount >= 12 && grdCount <= 22)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 23 && grdCount <= 33)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -922);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                //MessageBox.Show(grdSub1.GetData(grdCount-1, "BUNDLE_NO").ToString());
            }
            else if (grdCount >= 34 && grdCount <= 44)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(35 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -922);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(35 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1387);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    //grdSub1.ScrollPosition = new Point(0, -922);
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 45)
            {
                if (grdSub1.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub1.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(35 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -922);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(35 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub1.GetData(45 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1387);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub1.GetData(45 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub1.ScrollPosition = new Point(0, -1717);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    //grdSub1.ScrollPosition = new Point(0, -922);
                    grdSub1.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
        }

        private void setScrollPositionbefore()
        {
            var grdCount = grdSub2.Rows.Count;


            if (grdCount <= 11)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 12 && grdCount <= 22)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub2.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 23 && grdCount <= 33)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub2.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -922);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                //MessageBox.Show(grdSub2.GetData(grdCount-1, "BUNDLE_NO").ToString());
            }
            else if (grdCount >= 34 && grdCount <= 44)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub2.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(35 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -922);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(35 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -1387);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else
                {
                    //grdSub2.ScrollPosition = new Point(0, -922);
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 45)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub2.GetData(12 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -462);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(35 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -922);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(35 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(45 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -1387);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(45 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -1717);
                    c_scrollPosition = Convert.ToInt32(grdSub2.ScrollPosition.Y.ToString());
                }
                else
                {
                    //grdSub2.ScrollPosition = new Point(0, -922);
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
        }

        private void setScrollPosition2()
        {
            var grdCount = grdSub2.Rows.Count;


            if (grdCount <= 22)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 23 && grdCount <= 41)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -492);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
            else if (grdCount >= 42 && grdCount <= 61)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(42 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -492);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(42 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -982);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                //MessageBox.Show(grdSub1.GetData(grdCount-1, "BUNDLE_NO").ToString());
            }
            else if (grdCount >= 62)
            {
                if (grdSub2.GetData(1, "COMPLETE_YON").ToString() == "미완료")
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
                else if (grdSub2.GetData(23 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(42 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -492);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(42 - 1, "COMPLETE_YON").ToString() == "완료" && grdSub2.GetData(62 - 1, "COMPLETE_YON").ToString() != "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -982);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else if (grdSub2.GetData(62 - 1, "COMPLETE_YON").ToString() == "완료")
                {
                    grdSub2.ScrollPosition = new Point(0, -1357);
                    c_scrollPosition = Convert.ToInt32(grdSub1.ScrollPosition.Y.ToString());
                }
                else
                {
                    grdSub2.ScrollPosition = new Point(0, c_scrollPosition1);
                }
            }
        }

        private void btnDisplay2_Click(object sender, EventArgs e)
        {

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

        private void btnDisplay2_Click_1(object sender, EventArgs e)
        {

        }

        private void c1Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
