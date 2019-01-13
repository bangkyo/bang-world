using ComLib;
using System;
using C1.Win.C1FlexGrid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using SystemControlClassLibrary.UC.sub_UC;
using ComLib.clsMgr;
using static SystemControlClassLibrary.Order.CrtInOrdCre;


namespace SystemControlClassLibrary.Results
{
    public partial class WholeState : Form
    {
        clsCom ck = new clsCom();

        OracleTransaction transaction = null;

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        clsStyle cs = new clsStyle();

        ListDictionary ngTest_Value = null;
        ListDictionary fault_cd = null;

        DataTable olddt;
        DataTable moddt;

        private int subtotalNo;

        private string item_size = "";

        private string strBefValues = "";

        private string ownerNM = "";
        private string titleNM = "";
        string gubun_values = string.Empty;



        private UC.sub_UC.UC_Line_gp_s uC_Line_gp_s1;
        //private UC.sub_UC.UC_Work_Day uC_Work_Day1;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public WholeState(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void WholeState_Load(object sender, EventArgs e)
        {
            InitControl();

            btnDisplay_Click(null, null);

            //다중스레드무시
            CheckForIllegalCrossThreadCalls = false;
        }

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitButton(btnExcel);

            cs.InitButton(btnSave);

            cs.InitButton(btnDisplay);

            cs.InitButton(btnClose);

            #region 유저컨트롤 설정


            int location_x = 0;
            int location_y = 0;
            //
            // uC_Line_gp_s1
            // 
            uC_Line_gp_s1 = new UC_Line_gp_s();
            uC_Line_gp_s1.BackColor = System.Drawing.Color.Transparent;
            uC_Line_gp_s1.cb_Enable = true;
            //uC_Line_gp_s1.Line_GP = ck.Line_gp;
            //uC_Line_gp_s1.Location = new System.Drawing.Point(333 + 50 + location_x, 7 + location_y); //location_x
            uC_Line_gp_s1.Location = new System.Drawing.Point(3 + 50 + location_x, 7 + location_y); //location_x
            uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            uC_Line_gp_s1.TabIndex = 1;
            // 

            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;



            panel1.Controls.Add(this.uC_Line_gp_s1);

            InitGrd_Main();

            // controll init data
            uC_Line_gp_s1.Line_GP = ck.Line_gp;
            
            #endregion
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

        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

        private void InitGrd_Main()
        {
            cs.InitGrid_search(grdMain, false, 2, 1);
            

            grdMain.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            grdMain.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            //grdMain.Dock = DockStyle.Fill;
            //grdMain.AllowEditing = false;

            int level1 = 50; // 2자리
            int level2 = 50; // 4자리
            int level2_5 = 60;
            int level3 = 90; // 6자리
            int level4 = 120; // 8자리이상

            #region 1. grdMain head 및 row  align 설정

            grdMain.Rows[1].Height = 40;

            grdMain[1, "L_NO"] = grdMain.Cols["L_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "WORK_TYPE_NM"] = grdMain.Cols["WORK_TYPE_NM"].Caption;
            //grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);
            grdMain[1, "HEAT"] = grdMain.Cols["HEAT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STEEL"] = grdMain.Cols["STEEL"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "FACTORY"] = grdMain.Cols["FACTORY"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "ITEM_SIZE"] = grdMain.Cols["ITEM_SIZE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "LENGTH"] = grdMain.Cols["LENGTH"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "PRODUCT_PCS"] = grdMain.Cols["PRODUCT_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "TRANS_PCS"] = grdMain.Cols["TRANS_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "INPUT_PCS"] = grdMain.Cols["INPUT_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "NDT_INSP_PCS"] = grdMain.Cols["NDT_INSP_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "PCS_CNT"] = grdMain.Cols["PCS_CNT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "NDT_OK_PCS"] = grdMain.Cols["NDT_OK_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "NDT_NG_PCS"] = grdMain.Cols["NDT_NG_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "CG_PCS"] = grdMain.Cols["CG_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MPI_PCS"] = grdMain.Cols["MPI_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "GR_PCS"] = grdMain.Cols["GR_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MINUS"] = grdMain.Cols["MINUS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "RDATE"] = grdMain.Cols["RDATE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "TRANS_DATE"] = grdMain.Cols["TRANS_DATE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "INPUT_DATE"] = grdMain.Cols["INPUT_DATE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "INPUT_PCS"] = "교정";
            grdMain[1, "NDT_INSP_PCS"] = "비파괴";
            grdMain[1, "PCS_CNT"] = "결속";
            grdMain[1, "NDT_OK_PCS"] = "합격";
            grdMain[1, "NDT_NG_PCS"] = "불합격";
            grdMain[1, "CG_PCS"] = "입고";
            grdMain[1, "MPI_PCS"] = "MPI";
            grdMain[1, "GR_PCS"] = "G/R";
            



            grdMain.Cols["L_NO"].Width = 0;
            grdMain.Cols["HEAT"].Width = level3;
            grdMain.Cols["STEEL"].Width = level3;
            grdMain.Cols["FACTORY"].Width = 60;
            grdMain.Cols["ITEM_SIZE"].Width = level2_5;
            grdMain.Cols["LENGTH"].Width = level2_5;
            grdMain.Cols["PRODUCT_PCS"].Width = 70;
            grdMain.Cols["TRANS_PCS"].Width = 70;
            grdMain.Cols["INPUT_PCS"].Width = 70;
            grdMain.Cols["NDT_INSP_PCS"].Width = 70;
            grdMain.Cols["PCS_CNT"].Width = 70;
            grdMain.Cols["NDT_OK_PCS"].Width = 70;
            grdMain.Cols["NDT_NG_PCS"].Width = 70;
            grdMain.Cols["CG_PCS"].Width = 70;
            grdMain.Cols["MPI_PCS"].Width = 70;
            grdMain.Cols["GR_PCS"].Width = 70;
            grdMain.Cols["MINUS"].Width = 70;
            grdMain.Cols["RDATE"].Width = level4;
            grdMain.Cols["TRANS_DATE"].Width = level4;
            grdMain.Cols["INPUT_DATE"].Width = level4;
            grdMain.Cols["GUBUN"].Width = 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["FACTORY"].TextAlign = cs.HEAT_TextAlign;

            grdMain.Cols["PRODUCT_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["TRANS_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["INPUT_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["NDT_INSP_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["PCS_CNT"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["NDT_OK_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["NDT_NG_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["CG_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MPI_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["GR_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MINUS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["RDATE"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["TRANS_DATE"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["INPUT_DATE"].TextAlign = cs.HEAT_TextAlign;
            //grdMain.Cols["REMARKS"].TextAlign = cs.REMARKS_TextAlign;


            #endregion

            grdMain.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                grdMain.Cols[i].AllowMerging = true;
            }

            grdMain.Rows[0].AllowMerging = true;

            grdMain.Rows[0].AllowMerging = true;

            grdMain.AllowEditing = true;
            grdMain.Rows[0].AllowEditing = false;
            grdMain.Rows[1].AllowEditing = false;

            grdMain.Cols["L_NO"].AllowEditing = false;
            //grdMain.Cols["WORK_TYPE_NM"].AllowEditing = false;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain.Cols["STEEL"].AllowEditing = false;
            grdMain.Cols["HEAT"].AllowEditing = false;
            grdMain.Cols["FACTORY"].AllowEditing = false;
            grdMain.Cols["PRODUCT_PCS"].AllowEditing = false;
            grdMain.Cols["TRANS_PCS"].AllowEditing = true;
            grdMain.Cols["INPUT_PCS"].AllowEditing = false;
            grdMain.Cols["NDT_INSP_PCS"].AllowEditing = false;
            grdMain.Cols["PCS_CNT"].AllowEditing = false;
            grdMain.Cols["NDT_OK_PCS"].AllowEditing = false;
            grdMain.Cols["NDT_NG_PCS"].AllowEditing = false;
            grdMain.Cols["CG_PCS"].AllowEditing = false;
            grdMain.Cols["MPI_PCS"].AllowEditing = false;
            grdMain.Cols["GR_PCS"].AllowEditing = false;
            grdMain.Cols["MINUS"].AllowEditing = false;
            grdMain.Cols["RDATE"].AllowEditing = false;
            grdMain.Cols["TRANS_DATE"].AllowEditing = false;
            grdMain.Cols["INPUT_DATE"].AllowEditing = false;
            grdMain.Cols["GUBUN"].AllowEditing = false;

            //grdMain.Cols[5].Format = "#,##";
            //for (int i = 5; i < grdMain.Cols.Count; i++)
            //{
            //    grdMain.Cols[i].Format = "#,###";
            //}

            #region 콤보박스 설정
            //SetComboinGrd();
            //grdMain.Cols["FAULT_CD"].DataMap = fault_cd;
            //grdMain.Cols["FAULT_CD"].TextAlign = TextAlignEnum.LeftCenter;

            #endregion 콤보박스 설정

            grdMain.ExtendLastCol = true;

        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {

            cd.InsertLogForSearch(ck.UserID, btnDisplay);
            setDataBinding();
            
        }

        #region 조업일보 조회 설정(SQL)
        private void setDataBinding()
        {
            string sql1 = string.Empty;
            string start_date = start_dt.Value.ToString("yyyyMMdd");
            string end_date = end_dt.Value.ToString("yyyyMMdd");

            sql1 += string.Format("SELECT HEAT ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', FN_GET_HEAT_DATE(MAX(HEAT), 'R')) RDATE ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', FN_GET_FACTORY(MAX(HEAT))) FACTORY ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', MAX(C.CD_NM)) STEEL ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', MAX(ITEM_SIZE)) ITEM_SIZE ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', MAX(LENGTH)) LENGTH ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', FN_GET_PRODUCT_PCS(MAX(HEAT))) PRODUCT_PCS ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', FN_GET_INPUT_DATE(MAX(HEAT))) INPUT_DATE ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', FN_GET_PCS(MAX(HEAT), 'S')) MPI_PCS ");
            sql1 += string.Format("     , DECODE(GROUPING(HEAT), 1, '', FN_GET_PCS(MAX(HEAT), 'R')) GR_PCS ");
            sql1 += string.Format("     , SUM(INPUT_PCS) INPUT_PCS ");
            sql1 += string.Format("     , SUM(NDT_INSP_PCS) NDT_INSP_PCS ");
            sql1 += string.Format("     , SUM(NDT_OK_PCS) NDT_OK_PCS ");
            sql1 += string.Format("     , SUM(NDT_NG_PCS) NDT_NG_PCS ");
            sql1 += string.Format("     , SUM(PCS_CNT) PCS_CNT ");
            sql1 += string.Format("     , SUM(PCS_CNT) CG_PCS ");
            sql1 += string.Format("FROM ");
            sql1 += string.Format(" (SELECT HEAT, STEEL ");
            sql1 += string.Format("     , MAX(ITEM_SIZE)   ITEM_SIZE ");
            sql1 += string.Format("     , TO_CHAR(MAX(LENGTH),'FM9.90') LENGTH ");
            sql1 += string.Format("     , MAX(INPUT_PCS)   INPUT_PCS ");
            sql1 += string.Format("     , MAX(NDT_INSP_PCS) NDT_INSP_PCS ");
            sql1 += string.Format("     , MAX(NDT_OK_PCS) NDT_OK_PCS ");
            sql1 += string.Format("     , MAX(NDT_NG_PCS) NDT_NG_PCS ");
            sql1 += string.Format("     , MAX(PCS_CNT) PCS_CNT ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("     (SELECT HEAT, STEEL, MAX(ITEM_SIZE) ITEM_SIZE, MAX(LENGTH) LENGTH ");
            sql1 += string.Format("           , NVL(SUM(DECODE(ROUTING_CD,'F2',1,0)),0)  AS  NDT_INSP_PCS ");
            sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'A1', 1, 0)) AS INPUT_PCS ");
            sql1 += string.Format("           , NVL(SUM(DECODE(ROUTING_CD,'F2',DECODE(GOOD_YN,'OK',1,0),0)),0)  AS  NDT_OK_PCS ");
            sql1 += string.Format("           , SUM(DECODE(ROUTING_CD, 'F2', DECODE(MAT_GOOD_NG, 'NG', 1, DECODE(MLFT_GOOD_NG, 'NG', 1, DECODE(UT_GOOD_NG, 'NG', 1, 0))))) AS NDT_NG_PCS ");
            sql1 += string.Format("           , NULL PCS_CNT ");
            sql1 += string.Format("      FROM   (   SELECT MILL_NO, PIECE_NO, LINE_GP, ROUTING_CD, REWORK_SEQ ");
            sql1 += string.Format("                ,POC_NO, HEAT, GOOD_YN, MAT_GOOD_NG, MLFT_GOOD_NG, UT_GOOD_NG, MPI_INSP_GOOD_NG, STEEL, ITEM_SIZE, LENGTH ");
            sql1 += string.Format("                FROM  TB_CR_PIECE_WR V ");
            sql1 += string.Format("                WHERE LINE_GP  = '{0}' ", uC_Line_gp_s1.Line_GP);
            sql1 += string.Format("                AND ROUTING_CD  IN ('A1','F2') ");
            sql1 += string.Format("                 ) A ");
            sql1 += string.Format("                WHERE POC_NO IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
            sql1 += string.Format("                                  WHERE MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
            sql1 += string.Format("                                  AND  ROUTING_CD    = 'F2' ");
            sql1 += string.Format("                                  AND LINE_GP  = '{0}') ", uC_Line_gp_s1.Line_GP);
            //sql1 += string.Format("                AND REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            //sql1 += string.Format("                                    WHERE  MILL_NO = A.MILL_NO ");
            //sql1 += string.Format("                                    AND    PIECE_NO = A.PIECE_NO ");
            //sql1 += string.Format("                                    AND    LINE_GP = A.LINE_GP ");
            //sql1 += string.Format("                                    AND    ROUTING_CD = A.ROUTING_CD ) ");
            sql1 += string.Format("                GROUP BY HEAT, STEEL ");
            sql1 += string.Format("                UNION ALL ");
            sql1 += string.Format("                SELECT A.HEAT, A.STEEL ");
            sql1 += string.Format("                       , NULL ITEM_SIZE ");
            sql1 += string.Format("                       , NULL LENGTH ");
            sql1 += string.Format("                       , NULL INPUT_PCS ");
            sql1 += string.Format("                       , NULL NDT_INSP_PCS ");
            sql1 += string.Format("                       , NULL NDT_OK_PCS ");
            sql1 += string.Format("                       , NULL NDT_NG_PCS ");
            sql1 += string.Format("                       , MAX(PCS_CNT) PCS_CNT ");
            sql1 += string.Format("                FROM  TB_CR_PIECE_WR A ");
            sql1 += string.Format("                     , (SELECT A.HEAT ");
            sql1 += string.Format("                     , SUM(A.PCS) AS PCS_CNT ");
            sql1 += string.Format("                     FROM  TB_BND_WR  A ");
            sql1 += string.Format("                      , TB_WGT_WR  B ");
            sql1 += string.Format("                     WHERE A.BUNDLE_NO = B.BUNDLE_NO(+) ");
            sql1 += string.Format("                     AND POC_NO IN ( SELECT DISTINCT POC_NO FROM  TB_BND_WR ");
            sql1 += string.Format("                                  WHERE MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
            sql1 += string.Format("                                  AND LINE_GP  = '{0}') ", uC_Line_gp_s1.Line_GP);
            sql1 += string.Format("                     AND   A.LINE_GP  = '{0}' ", uC_Line_gp_s1.Line_GP);
            sql1 += string.Format("                     AND   NVL(A.DEL_YN, 'N') <> 'Y' ");
            sql1 += string.Format("                     AND   NVL(B.DEL_YN,'N') <> 'Y' ");
            sql1 += string.Format("                     AND SUBSTR(B.BUNDLE_NO, 10,1) IN ('A', 'B') ");
            sql1 += string.Format("                     GROUP BY A.HEAT ");
            sql1 += string.Format("                     ) B ");
            sql1 += string.Format(" WHERE A.MFG_DATE BETWEEN '{0}' AND '{1}' ", start_date, end_date);
            sql1 += string.Format(" AND A.HEAT = B.HEAT(+) ");
            sql1 += string.Format(" AND A.LINE_GP  = '{0}' ", uC_Line_gp_s1.Line_GP);
            sql1 += string.Format(" GROUP BY A.HEAT, A.STEEL ");
            sql1 += string.Format(" ) X ");
            sql1 += string.Format(" GROUP BY X.HEAT, X.STEEL ");
            sql1 += string.Format(" ) Y, (SELECT * FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL') C");
            sql1 += string.Format(" WHERE Y.STEEL = C.CD_ID(+)");
            //sql1 += string.Format(" AND STEEL  LIKE '{0}' || '%'", gangjung_id);
            sql1 += string.Format(" AND ITEM_SIZE  LIKE  '%'||'{0}' || '%' ", item_size);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
            sql1 += string.Format(" AND HEAT  LIKE  '%'||'{0}' || '%' ", txtPoc.Text);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
            sql1 += string.Format(" GROUP BY Y.HEAT ");
            sql1 += string.Format(" ORDER BY INPUT_DATE, HEAT ");


            //sql1 = string.Format("SELECT ROWNUM AS L_NO ");
            //sql1 += string.Format("       ,POC_NO");
            //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
            //sql1 += string.Format("       ,HEAT ");
            //sql1 += string.Format("       ,ITEM_SIZE ");
            //sql1 += string.Format("       ,NVL(MLFT_NG_PCS, 0) AS MLFT_NG_PCS ");
            //sql1 += string.Format("       ,NVL(WORK_PCS, 0) AS WORK_PCS ");
            //sql1 += string.Format("       ,NVL(OK_PCS, 0) AS OK_PCS ");
            //sql1 += string.Format("       ,NVL(FAULT_PCS, 0) AS FAULT_PCS ");
            //sql1 += string.Format("       ,NVL(B11_PCS, 0) AS B11_PCS ");
            //sql1 += string.Format("       ,NVL(B14_PCS, 0) AS B14_PCS ");
            //sql1 += string.Format("       ,NVL(B17_PCS, 0) AS B17_PCS ");
            //sql1 += string.Format("       ,NVL(B13_PCS, 0) AS B13_PCS ");
            //sql1 += string.Format("       ,NVL(B16_PCS, 0) AS B16_PCS ");
            //sql1 += string.Format("       ,NVL(B12_PCS, 0) AS B12_PCS ");
            //sql1 += string.Format("       ,NVL(B15_PCS, 0) AS B15_PCS ");
            //sql1 += string.Format("       ,NVL(B18_PCS, 0) AS B18_PCS ");
            //sql1 += string.Format("       ,''  AS  FAULT_CD");
            //sql1 += string.Format("       ,NVL(ETC_PCS, 0) AS ETC_PCS ");
            //sql1 += string.Format("       ,''  AS GUBUN ");
            //sql1 += string.Format("FROM   (      ");
            //sql1 += string.Format("         SELECT POC_NO ");
            //sql1 += string.Format("              ,ITEM_SIZE ");
            //sql1 += string.Format("              ,STEEL ");
            //sql1 += string.Format("              ,HEAT ");
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'F2',DECODE(A.MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',0,DECODE(A.MLFT_GOOD_NG,'NG',1,0)))) ) MLFT_NG_PCS ");
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2',DECODE(GOOD_YN,'OK',1,0),0)) + SUM(DECODE(ROUTING_CD,'H2',DECODE(GOOD_YN,'NG',1,0),0)) AS WORK_PCS "); //--작업본수
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'OK',1,0))) OK_PCS "); //--합격본수
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',1,0))) FAULT_PCS "); //--결함본수
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'AR0',1,0)) )) B11_PCS  "); //--크랙
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'B12',1,0)) )) B14_PCS  "); //--긁힘
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'BR1',1,0)) )) B17_PCS  "); //--패임
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'B02',1,0)) )) B13_PCS  "); //--딱지
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'BR2',1,0)) )) B16_PCS  "); //--주름
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'BR3',1,0)) )) B12_PCS  "); //--WRINKLE
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'A2',1,0)) )) B15_PCS  "); //--입계크랙
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2', DECODE(GOOD_YN,'NG',DECODE(A.MPI_FAULT_CD,'BR7',1,0)) )) B18_PCS  "); //--눌림
            //sql1 += string.Format("              ,SUM(DECODE(ROUTING_CD,'H2',DECODE(GOOD_YN, 'NG',CASE WHEN A.MPI_FAULT_CD IN ('AR0','B12','BR1','B02','BR2', 'BR3', 'A2', 'BR7') THEN 0 ");
            //sql1 += string.Format("                    ELSE 1  END)))  AS ETC_PCS "); //--기타
            //sql1 += string.Format("        FROM (SELECT MILL_NO, PIECE_NO, LINE_GP, ROUTING_CD, REWORK_SEQ, POC_NO, HEAT, GOOD_YN, ");
            //sql1 += string.Format("                     MAT_GOOD_NG, MLFT_GOOD_NG, UT_GOOD_NG, MPI_INSP_GOOD_NG, STEEL, ITEM_SIZE, MPI_FAULT_CD ");
            //sql1 += string.Format("               FROM TB_CR_PIECE_WR V ");
            //if (txtPoc.Text != "")
            //{
            //    sql1 += string.Format("               WHERE HEAT   = '{0}' ", txtPoc.Text);
            //}
            //else
            //{
            //    sql1 += string.Format("               WHERE POC_NO       IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
            //    sql1 += string.Format("                                         WHERE  ROUTING_CD    = 'F2' ");
            //    sql1 += string.Format("                                         AND   LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
            //    sql1 += string.Format("                                         AND   MFG_DATE   = '{0}' ) ", mfg_date); //P_MFG_DATE
            //}
            //sql1 += string.Format("                  AND   LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
            //sql1 += string.Format("                  AND ROUTING_CD IN( 'H2', 'F2') ");
            //sql1 += string.Format("             ) A ");
            //sql1 += string.Format("        GROUP BY POC_NO, STEEL, HEAT, ITEM_SIZE ");
            //sql1 += string.Format(") X ");


            olddt = cd.FindDataTable(sql1);

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(olddt, null, true);

            this.Cursor = System.Windows.Forms.Cursors.Default;


            for (int i = 5; i < grdMain.Cols.Count; i++)
            {
                grdMain.Cols[i].Format = "#,###";
            }

            
           grdMain.Cols[5].Format = "0.##";


            //소계, 계 BackColor
            //for (int iRow = 1; iRow < grdMain.Rows.Count; iRow++)
            //{
            //    if (grdMain[iRow, 2].ToString().Trim().Replace(" ", "") == "소계")
            //        grdMain.Rows[iRow].StyleNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));

            //    if (grdMain[iRow, 1].ToString().Trim().Replace(" ", "") == "계")
            //        grdMain.Rows[iRow].StyleNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            //}

            if (olddt.Rows.Count > 0)
            {
                UpdateTotals();
                int gubun_index = grdMain.Cols["HEAT"].Index;
                grdMain.Rows[2].StyleNew.BackColor = Color.Blue;
                grdMain.SetData(2, gubun_index, "합계");
            }


            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;

        }
        #endregion 조업일보 조회 설정(SQL)

        private void UpdateTotals()
        {

            subtotalNo = -1;

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["PRODUCT_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["INPUT_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["NDT_INSP_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["NDT_OK_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["NDT_NG_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["PCS_CNT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["CG_PCS"].Index, "합계");
            //grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B13_PCS"].Index, "합계");
            //grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B16_PCS"].Index, "합계");
            //grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B12_PCS"].Index, "합계");
            //grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B15_PCS"].Index, "합계");
            //grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B18_PCS"].Index, "합계");
            //grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["ETC_PCS"].Index, "합계");

            AddSubtotalNo();
            grdMain.Rows.Frozen = GetAvailMinRow(grdMain) -1;
            //grdMain.Subtotal(AggregateEnum.Average, 1, -1, grdMain.Cols["THEORY_WGT"].Index, "평균");

            //grdMain.AutoSizeCols();

            //grdMain.Rows.Fixed = GetAvailMinRow();
        }

        private void AddSubtotalNo()
        {
            ++subtotalNo;
        }

        private int GetAvailMinRow()
        {
            return (grdMain.Rows.Fixed + subtotalNo);
        }
        private int GetAvailMinRow(C1FlexGrid grid)
        {
            return (grid.Rows.Fixed + subtotalNo);
        }

        #region Grid Main Before Edit & After Edit 설정
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrds = sender as C1FlexGrid;

            int editedRows = e.Row;
            int editedCols = e.Col;

            if (editedRows <= 2)
            {
                return;
            }


            // 수정여부 확인을 위해 저장
            strBefValues = editedGrds.GetData(editedRows, editedCols).ToString();

        }

        private void txtItemSize_TextChanged(object sender, EventArgs e)
        {
            item_size = txtItemSize.Text;
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrds = sender as C1FlexGrid;

            int editedRows = e.Row;
            int editedCols = e.Col;
            int gubun_index = editedGrds.Cols["GUBUN"].Index;

            if (editedRows <= 2)
            {
                return;
            }

 

            // No,구분은 수정 불가
            if (editedCols == gubun_index)
            {
                editedGrds.SetData(editedRows, editedCols, strBefValues);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValues.ToString() == editedGrds.GetData(editedRows, editedCols).ToString().Trim())
                return;

            // 추가된 열에 대한 수정은 INSERT 처리
 
            if (editedCols == 0)
            {
                editedGrds.SetData(editedRows, editedCols, strBefValues);
                return;
            }
                

                
            // 저장시 UPDATE로 처리하기 위해 flag set
            editedGrds.SetData(editedRows, gubun_index, "수정");
            //editedGrds.SetData(editedRows, 0, "수정");

            // Update 배경색 지정
            editedGrds.Rows[editedRows].Style = editedGrds.Styles["UpColor"];
            
        }
        #endregion Grid Main Before Edit & After Edit 설정

        

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count > 1)
            {
                vf.SaveExcel(titleNM, grdMain);
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            //AddProcedure();

        }
        
   
    }
}
