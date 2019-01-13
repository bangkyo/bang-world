using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SystemControlClassLibrary
{
    public partial class LnProdRslt : Form
    {
        #region 변수선언
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        DataTable olddt;

        private string line_gp = "";
        private string ownerNM = "";
        private string titleNM = "";

        private string LOCK_CD_id = "";
        bool _CanSaveSearchLog = false;
        #endregion

        #region 화면
        public LnProdRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void LnProdRsltInq_Load(object sender, System.EventArgs e)
        {
            InitControl();

            SetComboBox();
            SetComboBox2();
            _CanSaveSearchLog = true;
            btnDisplay_Click(null, null);
        }

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        private void InitControl()
        {
            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitButton(btnClose);

            cs.InitCombo(cboLine_GP, StringAlignment.Center);
            cs.InitCombo(cboLockCd, StringAlignment.Near);

            //일자 데이터 default 값 적용 
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

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

        private void cboLockCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            LOCK_CD_id = ((DictionaryList)cboLockCd.SelectedItem).fnValue;
            btnDisplay_Click(null, null);
        }
        #endregion

        #region "콤보박스 설정"
        private void SetComboBox()
        {
            //cd.SetCombo(cboLine_GP, "LINE_GP", "", true, ck.Line_gp);
            cd.SetCombo(cboLine_GP, "LINE_GP", "", true);
        }

        private void SetComboBox2()
        {
            cd.SetCombo(cboLockCd, "LOCK_CD", "", true);
        }
        #endregion

        #region 조회
        private void btnDisplay_Click(object sender, EventArgs e)
        {

            if (_CanSaveSearchLog)
            {
                cd.InsertLogForSearch(ck.UserID, btnDisplay);
            }

            try
            {
                string sql1 = "";

                string start_date = start_dt.Value.ToString("yyyyMMdd");
                string end_date = end_dt.Value.ToString("yyyyMMdd");

                sql1 = string.Format(" SELECT NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'LINE_GP' AND CD_ID = X.LINE_GP),X.LINE_GP) AS LINE_GP ");
                sql1 += string.Format("     , POC_NO ");
                sql1 += string.Format("     , WGT_DATE ");
                sql1 += string.Format("     , HEAT ");
                sql1 += string.Format("     , STEEL_NM ");
                sql1 += string.Format("     , ITEM ");
                sql1 += string.Format("     , ITEM_SIZE ");
                sql1 += string.Format("     , LENGTH ");
                sql1 += string.Format("     , BUNDLE_QTY ");
                sql1 += string.Format("     , PCS ");
                sql1 += string.Format("     , NET_WGT ");
                sql1 += string.Format(" FROM  ( ");
                sql1 += string.Format("     SELECT DECODE(GROUPING(A.POC_NO), 1, DECODE(GROUPING(B.LINE_GP), 1, '계', '소계'), B.LINE_GP)  AS LINE_GP ");
                sql1 += string.Format("          , A.POC_NO AS POC_NO ");
                sql1 += string.Format("          , DECODE(GROUPING(A.POC_NO), 1, '', MAX(A.HEAT))  AS HEAT ");
                sql1 += string.Format("          , DECODE(GROUPING(A.POC_NO), 1, '', MAX((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL))) AS STEEL_NM ");
                sql1 += string.Format("          , DECODE(GROUPING(A.POC_NO), 1, '', MAX(A.ITEM))    AS ITEM ");
                sql1 += string.Format("          , DECODE(GROUPING(A.POC_NO), 1, '', FN_GET_WGT_DATE(MAX(A.POC_NO))) WGT_DATE ");
                sql1 += string.Format("          , DECODE(GROUPING(A.POC_NO), 1, '', MAX(A.ITEM_SIZE)) AS ITEM_SIZE ");
                sql1 += string.Format("          , TO_CHAR(DECODE(GROUPING(A.POC_NO), 1, '', MAX(A.LENGTH)),'990.99')    AS LENGTH ");
                sql1 += string.Format("          , COUNT(*)         AS BUNDLE_QTY ");  //번들수
                sql1 += string.Format("          , SUM(A.PCS)       AS PCS ");         //본수
                sql1 += string.Format("          , SUM(B.NET_WGT)   AS NET_WGT ");     //중량
                sql1 += string.Format("     FROM   TB_BND_WR A ");
                sql1 += string.Format("          , TB_WGT_WR B ");
                sql1 += string.Format("     WHERE A.BUNDLE_NO = B.BUNDLE_NO ");
                sql1 += string.Format("     AND   B.MEAS_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("     AND   NVL(A.DEL_YN,'N') <> 'Y' ");
                sql1 += string.Format("     AND   NVL(B.DEL_YN,'N') <> 'Y' ");
                sql1 += string.Format("      AND   B.LINE_GP   LIKE '{0}%' ", line_gp);
                if (LOCK_CD_id == "1")
                {
                    sql1 += string.Format("          AND SUBSTR(A.BUNDLE_NO,-1) IN ('A','B') ");
                }
                if (LOCK_CD_id == "2")
                {
                    sql1 += string.Format("          AND SUBSTR(A.BUNDLE_NO,-1) = 'M' ");
                }
                sql1 += string.Format("     GROUP BY ROLLUP(B.LINE_GP, A.POC_NO) ");
                sql1 += string.Format(" ) X ");

                //sql1 = string.Format(" SELECT DECODE(GROUPING(A.POC_NO), 1,DECODE(GROUPING(A.LINE_GP),1, '계','소계'),A.LINE_GP) LINE_GP ");
                //sql1 += string.Format("      , DECODE(GROUPING(A.POC_NO), 1,'', MAX(A.POC_NO)) POC_NO ");
                //sql1 += string.Format("      , DECODE(GROUPING(A.POC_NO), 1,'', MAX(A.HEAT)) HEAT ");
                //sql1 += string.Format("      , DECODE(GROUPING(A.POC_NO), 1,'', MAX(A.STEEL_NM)) STEEL_NM ");
                //sql1 += string.Format("      , DECODE(GROUPING(A.POC_NO), 1,'', MAX(A.ITEM)) ITEM ");
                //sql1 += string.Format("      , DECODE(GROUPING(A.POC_NO), 1,'', MAX(A.ITEM_SIZE)) ITEM_SIZE ");
                //sql1 += string.Format("      , DECODE(GROUPING(A.POC_NO), 1,'', MAX(A.LENGTH)) LENGTH ");
                //sql1 += string.Format("      , SUM(BUNDLE_QTY) BUNDLE_QTY ");
                //sql1 += string.Format("      , SUM(PCS) PCS ");
                //sql1 += string.Format("      , SUM(NET_WGT) NET_WGT ");
                //sql1 += string.Format(" FROM   ( ");
                //sql1 += string.Format("          SELECT A.LINE_GP, A.POC_NO, A.HEAT ");
                //sql1 += string.Format("               , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) STEEL_NM ");
                //sql1 += string.Format("               , ITEM, ITEM_SIZE, LENGTH ");
                //sql1 += string.Format("               , COUNT(*)  BUNDLE_QTY ");
                //sql1 += string.Format("               , SUM(A.PCS) PCS ");
                //sql1 += string.Format("               , SUM(B.NET_WGT) NET_WGT ");
                //sql1 += string.Format("               FROM TB_BND_WR A, TB_WGT_WR B ");
                //sql1 += string.Format("               WHERE A.MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                //sql1 += string.Format("               AND A.BUNDLE_NO = B.BUNDLE_NO(+) ");
                //sql1 += string.Format("               AND   A.LINE_GP   LIKE '%{0}%' ", line_gp);
                //sql1 += string.Format("               AND   NVL(A.DEL_YN,'N') <> 'Y' ");
                //sql1 += string.Format("               AND   NVL(B.DEL_YN,'N') <> 'Y' ");
                //sql1 += string.Format("          GROUP BY A.LINE_GP, A.POC_NO, A.HEAT, A.STEEL,ITEM, ITEM_SIZE, LENGTH ");
                //sql1 += string.Format("          UNION ALL ");
                //sql1 += string.Format("        SELECT P.LINE_GP, I.PO_NO POC_NO, I.HEAT ");
                //sql1 += string.Format("        ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = I.STEEL) STEEL_NM ");
                //sql1 += string.Format("        ,I.ITEM, I.ITEM_SIZE, (I.LENGTH/100) LENGTH ");
                //sql1 += string.Format("        ,COUNT(*)  BUNDLE_QTY ");
                //sql1 += string.Format("        ,SUM(I.BONSU) PCS ");
                //sql1 += string.Format("        ,SUM(I.WEIGHT) NET_WGT ");
                //sql1 += string.Format("        FROM TB_INSPECT@ISPDB I ");
                //sql1 += string.Format("        ,(SELECT DISTINCT LINE_GP, POC_NO FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("        WHERE MFG_DATE BETWEEN '{0}' AND '{1}') P ", start_date, end_date);
                //sql1 += string.Format("        WHERE I.INSPECT_TYPE = 'D3PR' ");
                //sql1 += string.Format("        AND I.INSPECT_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                //sql1 += string.Format("        AND I.SIRF_CODE = '24A' ");
                //sql1 += string.Format("        AND AFTER_ROUTING <> 'A700' ");
                //sql1 += string.Format("        AND I.PO_NO IN (SELECT DISTINCT POC_NO FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("        WHERE MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                //sql1 += string.Format("        AND LINE_GP IN ('#1','#2') ");
                //sql1 += string.Format("        AND LINE_GP   LIKE '%{0}%' )", line_gp);
                //sql1 += string.Format("        AND I.PO_NO = P.POC_NO(+) ");
                //sql1 += string.Format("        GROUP BY P.LINE_GP, I.PO_NO, I.HEAT, I.STEEL, I.ITEM, I.ITEM_SIZE, I.LENGTH ");
                //sql1 += string.Format(" ) A ");
                //sql1 += string.Format(" GROUP BY ROLLUP( A.LINE_GP, A.POC_NO ) ");

                olddt = cd.FindDataTable(sql1);
                Cursor.Current = Cursors.WaitCursor;
                grdMain.SetDataBinding(olddt, null, true);
                Cursor.Current = Cursors.Default;
               
                //소계, 계 BackColor
                for (int iRow = 1; iRow < grdMain.Rows.Count; iRow++)
                {
                    if (grdMain[iRow, 0].ToString().Trim().Replace(" ", "") == "소계")
                    {
                        grdMain.Rows[iRow].StyleNew.BackColor = Color.FromArgb(153,204,255);

                    }

                    if (grdMain[iRow, 0].ToString().Trim().Replace(" ", "") == "계")
                    {
                        grdMain.Rows[iRow].StyleNew.BackColor = Color.FromArgb(112, 184, 255);
                    }
                }

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
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
            vf.SaveExcel(titleNM, grdMain);
        }
        #endregion

        #region 이벤트
        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            btnDisplay_Click(null, null);
        }
        #endregion

        #region 그리드 설정
        private void InitGrid()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.BackColor = Color.White;
            grdMain.AutoResize = true;

            grdMain.Cols["LINE_GP"   ].Caption = "라인";                                        
            grdMain.Cols["POC_NO"    ].Caption = "POC";
            grdMain.Cols["WGT_DATE"  ].Caption = "정정일자";
            grdMain.Cols["HEAT"      ].Caption = "HEAT";                                        
            grdMain.Cols["STEEL_NM"  ].Caption = "강종명";                                      
            grdMain.Cols["ITEM"      ].Caption = "품목";                                        
            grdMain.Cols["ITEM_SIZE" ].Caption = "규격";                                        
            grdMain.Cols["LENGTH"    ].Caption = "길이(m)";                                     
            grdMain.Cols["BUNDLE_QTY"].Caption = "번들수";                                      
            grdMain.Cols["PCS"       ].Caption = "본수";                                        
            grdMain.Cols["NET_WGT"   ].Caption = "실중량(kg)";   

            grdMain.Cols["LINE_GP"   ].Width = 70;
            grdMain.Cols["POC_NO"    ].Width = 120;
            grdMain.Cols["WGT_DATE"  ].Width = 120;
            grdMain.Cols["HEAT"      ].Width = 160;
            grdMain.Cols["STEEL_NM"  ].Width = 140;
            grdMain.Cols["ITEM"      ].Width = 120;
            grdMain.Cols["ITEM_SIZE" ].Width = 100;
            grdMain.Cols["LENGTH"    ].Width = 100;
            grdMain.Cols["BUNDLE_QTY"].Width = 100;
            grdMain.Cols["PCS"       ].Width = 100;
            grdMain.Cols["NET_WGT"   ].Width = 100;

            grdMain.Cols["LINE_GP"   ].TextAlign = cs.LINE_GP_TextAlign;
            grdMain.Cols["POC_NO"    ].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["WGT_DATE"  ].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"      ].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL_NM"  ].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"      ].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE" ].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"    ].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["BUNDLE_QTY"].TextAlign = cs.BUNDLE_QTY_TextAlign;
            grdMain.Cols["PCS"       ].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["NET_WGT"   ].TextAlign = cs.WGT_TextAlign;
        }                         
        #endregion
    }
}
