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
using SystemControlClassLibrary.Popup;

namespace SystemControlClassLibrary
{
    public partial class WholeWrkState : Form
    {
        #region 변수선언
        clsStyle cs = new clsStyle();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsCom ck = new clsCom();

        DataTable olddt;

        private string WORK_TYPE = "";
        private string line_gp = "";
        //private string GUBUN_ID = "";
        private string ownerNM = "";
        private string titleNM = "";
        private string item_size = "";
        private static string gangjung_id = "";
        bool _CanSaveSearchLog = false;
        #endregion

        #region 화면
        public WholeWrkState(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

        }

        private void WholeWrkState_Load(object sender, System.EventArgs e)
        {
            InitControl();

            grdMain_setting();


            SetComboBox();
            _CanSaveSearchLog = true;
            btnDisplay_Click(null, null);

            EventCreate();      //사용자정의 이벤트
        }



        private void InitControl()
        {
            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitButton(btnClose);

            clsStyle.Style.InitLabel(lblItemSize);


            clsStyle.Style.InitTextBox(gangjong_id_tb);
            clsStyle.Style.InitTextBox(gangjong_Nm_tb);


            cs.InitCombo(cbo_Work_Type, StringAlignment.Center);
            cs.InitCombo(cboLine_GP, StringAlignment.Center);
            //일자 데이터 default 값 적용 
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;
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

        #region "콤보박스 설정"
        private void SetComboBox()
        {

            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);
            cd.SetCombo(cboLine_GP, "LINE_GP", "", true);


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


                //string sql1 = "";

                string start_date = start_dt.Value.ToString("yyyyMMdd");
                string end_date = end_dt.Value.ToString("yyyyMMdd");
                string sql1 = "";
                sql1 += string.Format("SELECT DECODE(GROUPING(POC_NO), 1,DECODE(GROUPING(LINE_GP),1, '계','소계'),LINE_GP) LINE_GP ");
                sql1 += string.Format("     , POC_NO ");
                sql1 += string.Format("     , DECODE(GROUPING(POC_NO), 1, '', MAX(HEAT)) HEAT ");
                sql1 += string.Format("     , DECODE(GROUPING(POC_NO), 1, '', FN_GET_HEAT_DATE(MAX(HEAT),'S')) SDATE ");
                sql1 += string.Format("     , DECODE(GROUPING(POC_NO), 1, '', FN_GET_HEAT_DATE(MAX(HEAT),'R')) RDATE ");
                sql1 += string.Format("     , DECODE(GROUPING(POC_NO), 1, '', FN_GET_WGT_DATE(MAX(POC_NO))) WGT_DATE ");
                sql1 += string.Format("     , DECODE(GROUPING(POC_NO), 1, '', MAX(C.CD_NM)) STEEL ");
                sql1 += string.Format("     , DECODE(GROUPING(POC_NO), 1, '', MAX(ITEM_SIZE)) ITEM_SIZE ");
                sql1 += string.Format("     , SUM(INPUT_PCS)       INPUT_PCS ");
                sql1 += string.Format("     , SUM(STR_OK_PCS)      STR_OK_PCS ");
                sql1 += string.Format("     , SUM(STR_NG_PCS)      STR_NG_PCS ");
                sql1 += string.Format("     , SUM(SHF_PCS)         SHF_PCS ");
                sql1 += string.Format("     , DECODE(GROUPING(POC_NO), 1, '', FN_GET_SURFACE_LEVEL(MAX(POC_NO))) SURFACE_LEVEL ");
                sql1 += string.Format("     , SUM(NDT_INSP_PCS)    NDT_INSP_PCS ");
                sql1 += string.Format("     , SUM(NDT_OK_PCS)      NDT_OK_PCS ");
                sql1 += string.Format("     , SUM(MAT_NG_PCS)      MAT_NG_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(MAT_NG_PCS)) MAT_NG_WGT ");
                sql1 += string.Format("     , CASE WHEN SUM(NDT_INSP_PCS)>0 THEN ROUND(SUM(MAT_NG_PCS)/SUM(NDT_INSP_PCS)*100,1) END MAT_NG_PER ");
                sql1 += string.Format("     , SUM(MLFT_NG_PCS)     MLFT_NG_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(MLFT_NG_PCS)) MLFT_NG_WGT ");
                sql1 += string.Format("     , CASE WHEN SUM(NDT_INSP_PCS)>0 THEN ROUND(SUM(MLFT_NG_PCS)/SUM(NDT_INSP_PCS)*100,1) END MLFT_NG_PER ");
                sql1 += string.Format("     , SUM(UT_NG_PCS)       UT_NG_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(UT_NG_PCS)) UT_NG_WGT ");
                sql1 += string.Format("     , CASE WHEN SUM(NDT_INSP_PCS)>0 THEN ROUND(SUM(UT_NG_PCS)/SUM(NDT_INSP_PCS)*100,1) END UT_NG_PER ");
                sql1 += string.Format("     , SUM(NDT_NG_PCS)      NDT_NG_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(NDT_NG_PCS)) NDT_NG_WGT ");
                sql1 += string.Format("     , SUM(MPI_OK_PCS)      MPI_OK_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(MPI_OK_PCS)) MPI_OK_WGT ");
                sql1 += string.Format("     , SUM(MPI_NG_PCS)      MPI_NG_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(MPI_NG_PCS)) MPI_NG_WGT ");
                sql1 += string.Format("     , SUM(GR_OK_PCS)       GR_OK_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(GR_OK_PCS)) GR_OK_WGT ");
                sql1 += string.Format("     , SUM(GR_NG_PCS)       GR_NG_PCS ");
                sql1 += string.Format("     , ROUND(SUM(THEORY_WGT) / SUM(PCS_CNT) * SUM(GR_NG_PCS)) GR_NG_WGT ");
                sql1 += string.Format("     , SUM(BUNDLE_CNT)      BUNDLE_CNT ");
                sql1 += string.Format("     , SUM(PCS_CNT)         PCS_CNT ");
                sql1 += string.Format("     , SUM(THEORY_WGT)      THEORY_WGT ");
                sql1 += string.Format("     , SUM(NET_WGT)         NET_WGT ");
                sql1 += string.Format("FROM ");
                sql1 += string.Format(" (SELECT X.LINE_GP, POC_NO, HEAT, ITEM_SIZE ");
                //sql1 += string.Format("     ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) STEEL ");
                sql1 += string.Format("     , STEEL ");
                sql1 += string.Format("     , MAX(INPUT_PCS)   INPUT_PCS ");
                sql1 += string.Format("     , MAX(STR_OK_PCS)      STR_OK_PCS ");
                sql1 += string.Format("     , MAX(STR_NG_PCS)      STR_NG_PCS ");
                sql1 += string.Format("     , MAX(SHF_PCS)         SHF_PCS ");
                sql1 += string.Format("     , MAX(NDT_INSP_PCS)    NDT_INSP_PCS ");
                sql1 += string.Format("     , MAX(NDT_OK_PCS)      NDT_OK_PCS ");
                sql1 += string.Format("     , MAX(MAT_NG_PCS)      MAT_NG_PCS ");
                sql1 += string.Format("     , MAX(MLFT_NG_PCS)     MLFT_NG_PCS ");
                sql1 += string.Format("     , MAX(UT_NG_PCS)       UT_NG_PCS ");
                sql1 += string.Format("     , MAX(NDT_NG_PCS)      NDT_NG_PCS ");
                sql1 += string.Format("     , MAX(MPI_OK_PCS)      MPI_OK_PCS ");
                sql1 += string.Format("     , MAX(MPI_NG_PCS)      MPI_NG_PCS ");
                sql1 += string.Format("     , MAX(GR_OK_PCS)       GR_OK_PCS ");
                sql1 += string.Format("     , MAX(GR_NG_PCS)       GR_NG_PCS ");
                sql1 += string.Format("     , MAX(BUNDLE_CNT)      BUNDLE_CNT ");
                sql1 += string.Format("     , MAX(PCS_CNT)         PCS_CNT ");
                sql1 += string.Format("     , MAX(THEORY_WGT)      THEORY_WGT ");
                sql1 += string.Format("     , MAX(NET_WGT)         NET_WGT ");
                sql1 += string.Format(" FROM ");
                sql1 += string.Format("     (SELECT LINE_GP, POC_NO, HEAT, STEEL, ITEM_SIZE ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'A1', 1, 0)) AS INPUT_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'A1', DECODE(GOOD_YN, 'NG', 0, 1))) AS STR_OK_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'A1', DECODE(GOOD_YN, 'NG', 1, 0))) AS STR_NG_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'B1', 1, 0)) AS SHF_PCS ");
                sql1 += string.Format("           , NVL(SUM(DECODE(ROUTING_CD,'F2',1,0)),0)  AS  NDT_INSP_PCS ");
                sql1 += string.Format("           , NVL(SUM(DECODE(ROUTING_CD,'F2',DECODE(GOOD_YN,'OK',1,0),0)),0)  AS  NDT_OK_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'F2', DECODE(MAT_GOOD_NG, 'NG', 1, 0))) AS MAT_NG_PCS ");
                sql1 += string.Format("           , SUM(DECODE(A.MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',0,DECODE(A.MLFT_GOOD_NG,'NG',1,0))) ) AS MLFT_NG_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD,'F2', DECODE(MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',1,0)) )) AS UT_NG_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'F2', DECODE(MAT_GOOD_NG, 'NG', 1, DECODE(MLFT_GOOD_NG, 'NG', 1, DECODE(UT_GOOD_NG, 'NG', 1, 0))))) AS NDT_NG_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'H2', DECODE(MPI_INSP_GOOD_NG, 'OK', 1, 0))) AS MPI_OK_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'H2', DECODE(MPI_INSP_GOOD_NG, 'NG', 1, 0))) AS MPI_NG_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'K2', DECODE(GOOD_YN, 'OK', 1, 0))) AS GR_OK_PCS ");
                sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'K2', DECODE(GOOD_YN, 'NG', 1, 0))) AS GR_NG_PCS ");
                sql1 += string.Format("           , NULL BUNDLE_CNT ");
                sql1 += string.Format("           , NULL PCS_CNT ");
                sql1 += string.Format("           , NULL THEORY_WGT ");
                sql1 += string.Format("           , NULL NET_WGT ");
                sql1 += string.Format("      FROM   (   SELECT MILL_NO, PIECE_NO, LINE_GP, ROUTING_CD, REWORK_SEQ ");
                sql1 += string.Format("                ,POC_NO, HEAT, GOOD_YN, MAT_GOOD_NG, MLFT_GOOD_NG, UT_GOOD_NG, MPI_INSP_GOOD_NG, STEEL, ITEM_SIZE ");
                sql1 += string.Format("                FROM  TB_CR_PIECE_WR V ");
                sql1 += string.Format("                WHERE MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("                AND WORK_TYPE  LIKE '{0}%' ", WORK_TYPE);
                //sql1 += string.Format("                AND AUTO_YN IS NULL ");
                sql1 += string.Format("                AND ROUTING_CD  IN ('A1','B1','D2','F2','K2', 'H2') ");
                sql1 += string.Format("                 ) A ");
                //sql1 += string.Format("                WHERE   REWORK_SEQ = CASE WHEN A.REWORK_SEQ = 3 THEN 3 WHEN A.REWORK_SEQ = 2 THEN 2 ELSE 1 END ");
                //sql1 += string.Format("                WHERE   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                //sql1 += string.Format("                                    WHERE  MILL_NO = A.MILL_NO ");
                //sql1 += string.Format("                                    AND    PIECE_NO = A.PIECE_NO ");
                //sql1 += string.Format("                                    AND    LINE_GP = A.LINE_GP ");
                //sql1 += string.Format("                                    AND    ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("                GROUP BY LINE_GP, POC_NO, HEAT, STEEL, ITEM_SIZE ");
                sql1 += string.Format("                UNION ALL ");
                sql1 += string.Format("                SELECT A.LINE_GP, A.POC_NO, A.HEAT, A.STEEL, A.ITEM_SIZE ");
                sql1 += string.Format("                       , NULL INPUT_PCS ");
                sql1 += string.Format("                       , NULL STR_OK_PCS ");
                sql1 += string.Format("                       , NULL STR_NG_PCS ");
                sql1 += string.Format("                       , NULL SHF_PCS ");
                sql1 += string.Format("                       , NULL NDT_INSP_PCS ");
                sql1 += string.Format("                       , NULL NDT_OK_PCS ");
                sql1 += string.Format("                       , NULL MAT_NG_PCS ");
                sql1 += string.Format("                       , NULL MLFT_NG_PCS ");
                sql1 += string.Format("                       , NULL UT_NG_PCS ");
                sql1 += string.Format("                       , NULL NDT_NG_PCS ");
                sql1 += string.Format("                       , NULL MPI_OK_PCS ");
                sql1 += string.Format("                       , NULL MPI_NG_PCS ");
                sql1 += string.Format("                       , NULL GR_OK_PCS ");
                sql1 += string.Format("                       , NULL GR_NG_PCS ");
                sql1 += string.Format("                       , MAX(DECODE(A.LINE_GP,'#3', B.BUNDLE_CNT, C.BUNDLE_CNT)) BUNDLE_CNT ");
                sql1 += string.Format("                       , MAX(DECODE(A.LINE_GP,'#3', B.PCS_CNT,    C.PCS_CNT   )) PCS_CNT ");
                sql1 += string.Format("                       , MAX(DECODE(A.LINE_GP,'#3', B.THEORY_WGT, C.THEORY_WGT)) THEORY_WGT ");
                sql1 += string.Format("                       , MAX(DECODE(A.LINE_GP,'#3', B.NET_WGT,    C.NET_WGT   )) NET_WGT ");
                sql1 += string.Format("                FROM  TB_CR_PIECE_WR A ");
                sql1 += string.Format("                     , (SELECT A.LINE_GP, A.POC_NO, A.HEAT ");
                sql1 += string.Format("                     , COUNT(A.BUNDLE_NO) AS BUNDLE_CNT ");
                sql1 += string.Format("                     , SUM(A.PCS) AS PCS_CNT ");
                sql1 += string.Format("                     , SUM(FN_GET_WGT(A.ITEM, A.ITEM_SIZE, A.LENGTH, A.PCS)) AS THEORY_WGT ");
                sql1 += string.Format("                     , SUM(B.NET_WGT) NET_WGT ");
                sql1 += string.Format("                     FROM  TB_BND_WR  A ");
                sql1 += string.Format("                      , TB_WGT_WR  B ");
                sql1 += string.Format("                     WHERE A.BUNDLE_NO = B.BUNDLE_NO(+) ");
                sql1 += string.Format("                     AND   A.WORK_TYPE  LIKE '{0}%' ", WORK_TYPE);
                sql1 += string.Format("                     AND   A.MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("                     AND   NVL(A.DEL_YN, 'N') <> 'Y' ");
                sql1 += string.Format("                     AND   NVL(B.DEL_YN,'N') <> 'Y' ");
                sql1 += string.Format("                     GROUP BY A.LINE_GP, A.POC_NO, A.HEAT ");
                sql1 += string.Format("                     ) B, ");
                sql1 += string.Format("                     (SELECT PO_NO POC_NO, HEAT, STEEL, ITEM_SIZE ");
                sql1 += string.Format("                     ,COUNT(BUNDLE_NO) BUNDLE_CNT ");
                sql1 += string.Format("                     ,SUM(BONSU) PCS_CNT ");
                sql1 += string.Format("                     ,SUM(THEORY_WEIGHT) THEORY_WGT ");
                sql1 += string.Format("                     ,SUM(WEIGHT) NET_WGT ");
                sql1 += string.Format("                     FROM TB_INSPECT@ISPDB ");
                sql1 += string.Format("                     WHERE INSPECT_TYPE = 'D3PR' ");
                sql1 += string.Format("                     AND INSPECT_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("                     AND SIRF_CODE = '24A' ");
                sql1 += string.Format("                     AND AFTER_ROUTING <> 'A700'");
                sql1 += string.Format("                     AND PO_NO IN (SELECT DISTINCT POC_NO FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                                  WHERE MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("                                  AND   WORK_TYPE  LIKE '{0}%' ", WORK_TYPE);
                sql1 += string.Format("                                  AND LINE_GP IN ('#1','#2')) ");
                sql1 += string.Format("                     GROUP BY PO_NO, HEAT, STEEL, ITEM_SIZE ");
                sql1 += string.Format("                     ) C ");
                sql1 += string.Format(" WHERE A.MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format(" AND   A.WORK_TYPE  LIKE '{0}%' ", WORK_TYPE);
                sql1 += string.Format(" AND A.POC_NO = B.POC_NO(+) ");
                sql1 += string.Format(" AND A.POC_NO = C.POC_NO(+) ");
                //sql1 += string.Format(" AND A.AUTO_YN IS NULL ");
                sql1 += string.Format(" GROUP BY A.LINE_GP, A.POC_NO, A.HEAT, A.STEEL, A.ITEM_SIZE ");
                sql1 += string.Format(" ) X ");
                sql1 += string.Format(" GROUP BY X.LINE_GP, X.POC_NO, X.HEAT, X.STEEL, X.ITEM_SIZE, X.STEEL ");
                sql1 += string.Format(" ) Y, (SELECT * FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL') C");
                sql1 += string.Format(" WHERE Y.STEEL = C.CD_ID(+)");
                if (gangjung_id != "")
                    sql1 += string.Format(" AND STEEL  LIKE '{0}' || '%'", gangjung_id);
                if (line_gp != "")
                    sql1 += string.Format(" AND LINE_GP   LIKE '{0}' || '%'", line_gp);
                if (item_size != "")
                    sql1 += string.Format(" AND ITEM_SIZE  LIKE  '%'||'{0}' || '%' ", item_size);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                sql1 += string.Format(" GROUP BY ROLLUP(Y.LINE_GP, Y.POC_NO) ");

                olddt = cd.FindDataTable(sql1);
                Cursor.Current = Cursors.WaitCursor;
                grdMain.SetDataBinding(olddt, null, false);
                grdMain_setting();
                Cursor.Current = Cursors.Default;
                //Format
                for (int i = 7; i < grdMain.Cols.Count; i++)
                {
                    grdMain.Cols[i].Format = "#,###";
                }
                
                grdMain.Cols[16].Format = "0.#";
                grdMain.Cols[19].Format = "0.#";
                grdMain.Cols[22].Format = "0.#";
                grdMain.AutoSizeCols(4);
                //grdMain.AutoSizeCols(4, 5, 0);

                //소계, 계 BackColor
                for (int iRow = 2; iRow < grdMain.Rows.Count; iRow++)
                {
                    if (grdMain[iRow, 0].ToString().Trim().Replace(" ", "") == "소계")
                        grdMain.Rows[iRow].StyleNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));

                    if (grdMain[iRow, 0].ToString().Trim().Replace(" ", "") == "계")
                        grdMain.Rows[iRow].StyleNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
                }


                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
            finally
            {

                
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

        #endregion

        #region 그리드 설정
        private void grdMain_setting()
        {
            //셀 병합------------------------------------------
            grdMain.AllowMerging = AllowMergingEnum.Custom;
            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                //라인, POC, 투입본수, 면취, 비파괴불량총본수
                //if (i == 0 || i == 1 || i == 2 || i == 5 || i == 11)
                if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7 || i == 8 || i == 11 || i == 12)
                {
                    CellRange crCellRange = grdMain.GetCellRange(0, i, 1, i);
                    grdMain.MergedRanges.Add(crCellRange);
                }
                //else if (i == 3 || i == 6 || i == 12 || i == 14 || i == 16 || i == 18 || i == 20)
                else if (i == 9 || i == 13 || i == 24  || i == 34  || i == 36)
                {
                    CellRange crCellRange = grdMain.GetCellRange(0, i, 0, i + 1);
                    grdMain.MergedRanges.Add(crCellRange);
                }
                else if (i == 26 || i == 30)
                {
                    CellRange crCellRange = grdMain.GetCellRange(0, i, 0, i + 3);
                    grdMain.MergedRanges.Add(crCellRange);
                }
                else if (i == 15 || i == 18 || i == 21)
                {
                    CellRange crCellRange = grdMain.GetCellRange(0, i, 0, i + 2);
                    grdMain.MergedRanges.Add(crCellRange);
                }

            }
            //--------------------------------------------------------

            clsStyle.Style.InitGrid_search(grdMain);
            grdMain.AutoResize = true;
            grdMain.AllowEditing = false;

            //Column Header가 2행이므로 추가됨------------------------
            grdMain.Rows.Fixed = 2;
            grdMain.Rows[1].StyleNew.Font = new Font(clsStyle.Style.OHeadfontName, clsStyle.Style.FontSizeSmall, FontStyle.Bold);
            grdMain.Rows[0].Height = 22;
            grdMain.Rows[1].Height = 30;

            CellRange crCellRange3 = grdMain.GetCellRange(1, 0, 1, 37);
            crCellRange3.Style = grdMain.Styles["CellStyle"];
            
            grdMain.EndUpdate();
            //-------------------------------------------------------

            //Header텍스트-------------------------------------------
            grdMain.Cols[0].Caption = "라인";
            grdMain.Cols[1].Caption = "POC";
            grdMain.Cols[2].Caption = "HEAT";
            grdMain.Cols[3].Caption = "제강일자";
            grdMain.Cols[4].Caption = "압연일자";
            grdMain.Cols[5].Caption = "정정일자";
            grdMain.Cols[6].Caption = "강종명";
            grdMain.Cols[7].Caption = "규격";
            grdMain.Cols[8].Caption = "  투입\n\r본수";
            grdMain.Cols[9].Caption = grdMain.Cols[10].Caption = "교정";
            grdMain.Cols[11].Caption = "면취";
            grdMain.Cols[12].Caption = " 검사\n\r기준";
            grdMain.Cols[13].Caption = grdMain.Cols[14].Caption = "NDT";
            grdMain.Cols[15].Caption = grdMain.Cols[16].Caption = grdMain.Cols[17].Caption = "MAT";
            grdMain.Cols[18].Caption = grdMain.Cols[19].Caption = grdMain.Cols[20].Caption = "MLFT";
            grdMain.Cols[21].Caption = grdMain.Cols[22].Caption = grdMain.Cols[23].Caption = "UT";
            //grdMain.Cols[14].Caption = grdMain.Cols[15].Caption = "비파괴" + "\r\n" + "NG" + "\r\n" + "총본수";
            grdMain.Cols[24].Caption = grdMain.Cols[25].Caption = "비파괴";
            grdMain.Cols[26].Caption = grdMain.Cols[27].Caption = grdMain.Cols[28].Caption = grdMain.Cols[29].Caption = "MPI";
            grdMain.Cols[30].Caption = grdMain.Cols[31].Caption = grdMain.Cols[32].Caption = grdMain.Cols[33].Caption = "G/R";
            //grdMain.Cols[25].Caption = grdMain.Cols[26].Caption = "성분분석";
            grdMain.Cols[34].Caption = grdMain.Cols[35].Caption = "결속";
            grdMain.Cols[36].Caption = grdMain.Cols[37].Caption = "중량(kg)";



            


            // 정상, 불량, 번들수, 본수, 이론, 실중량
            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                if (i == 9 || i == 14 || i == 26 || i == 30)
                    grdMain[1, i] = "OK";
                else if (i == 10 || i == 15 || i == 18 || i == 21  || i == 28 || i == 32)
                    grdMain[1, i] = "NG";
                else if (i == 24)
                    grdMain[1, i] = "총NG";
                else if (i == 17 || i == 20 || i == 23)
                    grdMain[1, i] = "%";
                else if (i == 16 || i == 19 || i == 22 || i == 25 || i == 27 || i == 29 || i == 31 || i == 33)
                    grdMain[1, i] = "중량";
                else if (i == 13)
                    grdMain[1, i] = "검사";
                else if (i == 34)
                    grdMain[1, i] = "번들수";
                else if (i == 35)
                    grdMain[1, i] = "본수";
                else if (i == 36)
                    grdMain[1, i] = "이론";
                else if (i == 37)
                    grdMain[1, i] = "실중량";
            }
            //-------------------------------------------------------

            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                if (i > 12)
                    grdMain.Cols[i].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
                else if (i == 8 || i == 9 || i == 10 || i == 11)
                {
                    grdMain.Cols[i].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
                }
                else
                    grdMain.Cols[i].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

                if (i == 0)
                {
                    grdMain.Cols[i].Width = 63;
                }
                else if (i == 1 || i == 34 || i == 35)
                {
                    grdMain.Cols[i].Width = 100;
                }
                else if (i == 12)
                {
                    grdMain.Cols[i].Width = 40;
                }
                else
                {
                    grdMain.Cols[i].Width = 60;
                }
            }
        }
        #endregion

        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            WORK_TYPE = ((DictionaryList)cbo_Work_Type.SelectedItem).fnValue;

            btnDisplay_Click(null, null);
        }
        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            btnDisplay_Click(null, null);
        }
        private void txtItemSize_TextChanged(object sender, EventArgs e)
        {
            item_size = txtItemSize.Text;
        }
        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
        }

        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjung_id = gangjong_id_tb.Text;
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

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
                        gangjong_id_tb.Focus();
                        return;
                    }
                }
                else
                {
                    gangjung_id = gangjong_id_tb.Text;
                }
            }
        }
    }
}
