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
    public partial class TmProdRslt : Form
    {
        #region 변수선언
        clsStyle cs = new clsStyle();
        ConnectDB cd = new ConnectDB();
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();

        DataTable olddt;

        private string ling_gp = "";
        private string work_type = "";
        private string ownerNM = "";
        private string titleNM = "";
        bool _CanSaveSearchLog = false;

        #endregion

        #region 화면
        public TmProdRslt(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

        }

        private void LnProdRsltInq_Load(object sender, System.EventArgs e)
        {
            InitControl();

            


            SetComboBox();
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
            clsStyle.Style.InitPicture(pictureBox1);
            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);


            cs.InitCombo(cboLine_GP, StringAlignment.Center);

            cs.InitCombo(cbo_Work_Type, StringAlignment.Center);

            clsStyle.Style.InitPanel(panel1);
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
        #endregion

        #region "콤보박스 설정"
        private void SetComboBox()
        {

            cd.SetCombo(cboLine_GP, "LINE_GP", "", true, ck.Line_gp);

            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);
           
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
                

                string sql1 = string.Empty;

                string start_date = start_dt.Value.ToString("yyyyMMdd");
                string end_date = end_dt.Value.ToString("yyyyMMdd");

                sql1 += string.Format(" SELECT      NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'LINE_GP' AND CD_ID = X.LINE_GP),X.LINE_GP) AS LINE_GP ");
                sql1 += string.Format("            ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE)  AS WORK_TYPE_NM ");
                sql1 += string.Format("            ,TIME_1 ");
                sql1 += string.Format("            ,TIME_2 ");
                sql1 += string.Format("            ,TIME_3 ");
                sql1 += string.Format("            ,TIME_4 ");
                sql1 += string.Format("            ,TIME_5 ");
                sql1 += string.Format("            ,TIME_6 ");
                sql1 += string.Format("            ,TIME_7 ");
                sql1 += string.Format("            ,TIME_8 ");
                sql1 += string.Format("            ,TIME_T ");
                sql1 += string.Format(" FROM( " );
                sql1 += string.Format("            SELECT DECODE(GROUPING(A.WORK_TYPE), 1, DECODE(GROUPING(A.LINE_GP), 1, '계', '소계'), A.LINE_GP)  AS LINE_GP ");
                sql1 += string.Format("                 , A.WORK_TYPE ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '1', A.PCS))  AS TIME_1 ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '2', A.PCS))  AS TIME_2 ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '3', A.PCS))  AS TIME_3 ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '4', A.PCS))  AS TIME_4 ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '5', A.PCS))  AS TIME_5 ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '6', A.PCS))  AS TIME_6 ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '7', A.PCS))  AS TIME_7 ");
                sql1 += string.Format("                 , SUM(DECODE(T_GP, '8', A.PCS))  AS TIME_8 ");
                sql1 += string.Format("                 , SUM(A.PCS)  AS TIME_T ");
                sql1 += string.Format("            FROM   TB_BND_WR A ");
                sql1 += string.Format("                 , (   ");
                sql1 += string.Format("                        SELECT CD_ID AS T_GP ");
                sql1 += string.Format("                             , SUBSTR(COLUMNA, 1, 2) || '00' AS ST_T1 ");
                sql1 += string.Format("                             , SUBSTR(COLUMNA, 4, 2) || '00' AS ED_T1 ");
                sql1 += string.Format("                        FROM   TB_CM_COM_CD ");
                sql1 += string.Format("                        WHERE  CATEGORY = 'WORK_TYPE_TIME' ");
                sql1 += string.Format("                     UNION ");
                sql1 += string.Format("                        SELECT CD_ID AS T_GP ");
                sql1 += string.Format("                             , SUBSTR(COLUMNB, 1, 2) || '00' AS ST_T1 ");
                sql1 += string.Format("                             , SUBSTR(COLUMNB, 4, 2) || '00' AS ED_T1 ");
                sql1 += string.Format("                        FROM   TB_CM_COM_CD ");
                sql1 += string.Format("                        WHERE  CATEGORY = 'WORK_TYPE_TIME' ");
                sql1 += string.Format("                     UNION ");
                sql1 += string.Format("                        SELECT CD_ID AS T_GP ");
                sql1 += string.Format("                             , SUBSTR(COLUMNC, 1, 2) || '00' AS ST_T1 ");
                sql1 += string.Format("                             , SUBSTR(COLUMNC, 4, 2) || '00' AS ED_T1 ");
                sql1 += string.Format("                        FROM   TB_CM_COM_CD ");
                sql1 += string.Format("                        WHERE  CATEGORY = 'WORK_TYPE_TIME' ");
                sql1 += string.Format("                    ) B ");
                sql1 += string.Format("             WHERE  A.MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("             AND    A.LINE_GP   LIKE '{0}%' ", ling_gp);
                sql1 += string.Format("             AND    A.WORK_TYPE LIKE '{0}%' ", work_type);
                sql1 += string.Format("             AND    TO_CHAR(A.REG_DDTT, 'HH24MI') > ST_T1 ");
                sql1 += string.Format("             AND    TO_CHAR(A.REG_DDTT, 'HH24MI') <= ED_T1 ");
                sql1 += string.Format("             AND    NVL(A.DEL_YN,'N') <> 'Y' ");
                sql1 += string.Format("             GROUP BY ROLLUP(A.LINE_GP, A.WORK_TYPE) ");
                sql1 += string.Format("      ) X ");

                olddt = cd.FindDataTable(sql1);
                Cursor.Current = Cursors.WaitCursor;
                grdMain.SetDataBinding(olddt, null, true);
                Cursor.Current = Cursors.Default;



                //소계, 계 BackColor
                for (int iRow = 2; iRow < grdMain.Rows.Count; iRow++)
                {
                    if (grdMain[iRow, 0].ToString().Trim().Replace(" ", "") == "소계")
                    {
                        grdMain.Rows[iRow].StyleNew.BackColor = Color.FromArgb(153, 204, 255);
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


        #region 그리드 설정
        private void InitGrid()
        {
            cs.InitGrid_search(grdMain, false, 2, 1);

            //grdMain.AllowMerging = AllowMergingEnum.FixedOnly;
            grdMain.BackColor = Color.White;
            grdMain[1, "LINE_GP"] = grdMain[0, "LINE_GP"] = "라인";
            grdMain[1, "WORK_TYPE_NM"] = grdMain[0, "WORK_TYPE_NM"] = "근";
            grdMain[0, "TIME_1"] = grdMain[0, "TIME_2"] = grdMain[0, "TIME_3"] = grdMain[0, "TIME_4"] = grdMain[0, "TIME_5"] = grdMain[0, "TIME_6"] = grdMain[0, "TIME_7"] = grdMain[0, "TIME_8"] = "시간대별 본수";
            grdMain[1, "TIME_1"] = "1";
            grdMain[1, "TIME_2"] = "2";
            grdMain[1, "TIME_3"] = "3";
            grdMain[1, "TIME_4"] = "4";
            grdMain[1, "TIME_5"] = "5";
            grdMain[1, "TIME_6"] = "6";
            grdMain[1, "TIME_7"] = "7";
            grdMain[1, "TIME_8"] = "8";
            grdMain[1, "TIME_T"] = "계";
            grdMain.AllowMerging = AllowMergingEnum.FixedOnly;

            grdMain.Rows[1].StyleNew.Font = new Font(cs.OHeadfontName, cs.FontSizeSmall, FontStyle.Bold);

            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                grdMain.Cols[i].AllowMerging = true;

            }

            grdMain.Rows[0].AllowMerging = true;


            grdMain.Cols["LINE_GP"     ].Width =110;
            grdMain.Cols["WORK_TYPE_NM"].Width =110;
            grdMain.Cols["TIME_1"      ].Width =110;
            grdMain.Cols["TIME_2"      ].Width =110;
            grdMain.Cols["TIME_3"      ].Width =110;
            grdMain.Cols["TIME_4"      ].Width =110;
            grdMain.Cols["TIME_5"      ].Width =110;
            grdMain.Cols["TIME_6"      ].Width =110;
            grdMain.Cols["TIME_7"      ].Width =110;
            grdMain.Cols["TIME_8"      ].Width =110;
            grdMain.Cols["TIME_T"      ].Width =110;

            grdMain.Cols["LINE_GP"     ].TextAlign =cs.LINE_GP_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign =cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["TIME_1"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_2"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_3"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_4"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_5"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_6"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_7"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_8"      ].TextAlign =cs.PCS_TextAlign;
            grdMain.Cols["TIME_T"      ].TextAlign =cs.PCS_TextAlign;

           
        }

        #endregion

        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            ling_gp = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = ling_gp;

            btnDisplay_Click(null, null);
        }

        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_type = ((DictionaryList)cbo_Work_Type.SelectedItem).fnValue;

            btnDisplay_Click(null, null);
        }
    }
}
