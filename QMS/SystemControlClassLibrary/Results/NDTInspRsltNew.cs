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
    public partial class NDTInspRsltNew : Form
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
        string spName;
        private string strBefValues = "";

        private string ownerNM = "";
        private string titleNM = "";
        string gubun_values = string.Empty;



        private UC.sub_UC.UC_Line_gp_s uC_Line_gp_s1;
        private UC.sub_UC.UC_Work_Day uC_Work_Day1;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public NDTInspRsltNew(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void NDTInspRsltNew_Load(object sender, EventArgs e)
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
            uC_Line_gp_s1.Location = new System.Drawing.Point(433 + 50 + location_x, 7 + location_y); //location_x
            uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            uC_Line_gp_s1.TabIndex = 1;
            // 
            // uC_Work_Day1
            // 
            uC_Work_Day1 = new UC_Work_Day();
            uC_Work_Day1.BackColor = System.Drawing.Color.Transparent;
            uC_Work_Day1.Location = new System.Drawing.Point(26 + 24 + location_x, 7 + location_y); //(26 + 24 , 7 + location_y); //location_x
            uC_Work_Day1.Name = "uC_Work_Day1";
            uC_Work_Day1.Size = new System.Drawing.Size(270, 27);
            uC_Work_Day1.TabIndex = 2;

            
            panel1.Controls.Add(this.uC_Work_Day1);
            panel1.Controls.Add(this.uC_Line_gp_s1);

            InitGrd_Main();

            // controll init data
            uC_Line_gp_s1.Line_GP = ck.Line_gp;
            uC_Work_Day1.Work_Day = DateTime.Now.Date;
            #endregion
        }

        

        private void InitGrd_Main()
        {
            cs.InitGrid_search(grdMain, false, 2, 1);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["OK_PCS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["MAT_PCS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["MLFT_PCS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["MLFT2_PCS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["UT_PCS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];




            grdMain.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            grdMain.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            //grdMain.Dock = DockStyle.Fill;
            //grdMain.AllowEditing = false;

            int level1 = 50; // 2자리
            int level2 = 50; // 4자리
            int level2_5 = 80;
            int level3 = 110; // 6자리
            int level4 = 120; // 8자리이상
            int level5 = 140; // 8자리이상

            #region 1. grdMain head 및 row  align 설정
            grdMain[1, "L_NO"] = grdMain.Cols["L_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "WORK_TYPE_NM"] = grdMain.Cols["WORK_TYPE_NM"].Caption;
            //grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "ITEM_SIZE"] = grdMain.Cols["ITEM_SIZE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STEEL_NM"] = grdMain.Cols["STEEL_NM"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "HEAT"] = grdMain.Cols["HEAT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "POC_NO"] = grdMain.Cols["POC_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "WORK_DATE"] = grdMain.Cols["WORK_DATE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "LENGTH"] = grdMain.Cols["LENGTH"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "WORK_PCS"] = grdMain.Cols["WORK_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "OK_PCS"] = grdMain.Cols["OK_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "FAULT_PCS"] = grdMain.Cols["FAULT_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MAT_PCS"] = grdMain.Cols["MAT_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MLFT_PCS"] = grdMain.Cols["MLFT_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MLFT2_PCS"] = grdMain.Cols["MLFT2_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "UT_PCS"] = grdMain.Cols["UT_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "B11_PCS"] = "종크랙";
            //grdMain[1, "B14_PCS"] = "긁힘";
            //grdMain[1, "B17_PCS"] = "패임";
            //grdMain[1, "B13_PCS"] = "딱지";
            //grdMain[1, "B16_PCS"] = "주름";
            //grdMain[1, "ETC_PCS"] = "기타";
            //grdMain[1, "FAULT_CD"] = "결함코드";
            //grdMain[1, "ETC_PCS"] = "본수";

            grdMain.Cols["L_NO"].Width = 0;
            //grdMain.Cols["WORK_TYPE_NM"].Width = level2;
            grdMain.Cols["ITEM_SIZE"].Width = level3;
            grdMain.Cols["STEEL_NM"].Width = level4;
            grdMain.Cols["HEAT"].Width = level4;
            grdMain.Cols["POC_NO"].Width = level4;
            grdMain.Cols["WORK_DATE"].Width = level4;
            grdMain.Cols["LENGTH"].Width = level3;
            grdMain.Cols["WORK_PCS"].Width = level3;
            grdMain.Cols["OK_PCS"].Width = level3;
            grdMain.Cols["FAULT_PCS"].Width = level3;
            grdMain.Cols["MAT_PCS"].Width = level3;
            grdMain.Cols["MLFT_PCS"].Width = level3;
            grdMain.Cols["MLFT2_PCS"].Width = level3;
            grdMain.Cols["UT_PCS"].Width = level3;
            grdMain.Cols["GUBUN"].Width = 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            //grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["WORK_DATE"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["WORK_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["OK_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["FAULT_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MAT_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MLFT_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MLFT2_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["UT_PCS"].TextAlign = cs.PCS_TextAlign;
            
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
            grdMain.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain.Cols["STEEL_NM"].AllowEditing = false;
            grdMain.Cols["HEAT"].AllowEditing = false;
            grdMain.Cols["POC_NO"].AllowEditing = false;
            grdMain.Cols["WORK_DATE"].AllowEditing = false;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["WORK_PCS"].AllowEditing = false;
            grdMain.Cols["OK_PCS"].AllowEditing = true;
            grdMain.Cols["FAULT_PCS"].AllowEditing = false;

            grdMain.Cols["MAT_PCS"].AllowEditing = true;
            grdMain.Cols["MLFT_PCS"].AllowEditing = true;
            grdMain.Cols["MLFT2_PCS"].AllowEditing = true;
            grdMain.Cols["UT_PCS"].AllowEditing = true;
            grdMain.Cols["GUBUN"].AllowEditing = false;

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
            //start_date = vf.CDate(vf.Format(start_dt.Value, "yyyy-MM-dd HH:mm:ss").ToString());
            //end_date = vf.CDate(vf.Format(end_dt.Value, "yyyy-MM-dd HH:mm:ss").ToString()); ;

            string mfg_date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();

            if (uC_Line_gp_s1.Line_GP == "#3")
            {
                sql1 = string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,POC_NO");
                sql1 += string.Format("       ,FN_GET_INFO(POC_NO, 'K') WORK_DATE");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,NVL(INSP_PCS, 0) AS WORK_PCS ");
                sql1 += string.Format("       ,NVL(OK_PCS, 0) AS OK_PCS ");
                sql1 += string.Format("       ,NVL(ALL_NG_PCS, 0) AS FAULT_PCS ");
                sql1 += string.Format("       ,NVL(MAT_NG_PCS, 0) AS MAT_PCS ");
                sql1 += string.Format("       ,NVL(MLFT_NG_PCS, 0) AS MLFT_PCS ");
                sql1 += string.Format("       ,FN_GET_INFO(POC_NO, 'M') MLFT2_PCS ");
                sql1 += string.Format("       ,NVL(UT_NG_PCS, 0) AS UT_PCS ");
                sql1 += string.Format("       ,''  AS GUBUN ");
                sql1 += string.Format("FROM   (      ");
                sql1 += string.Format("         SELECT A.POC_NO         AS POC_NO ");
                sql1 += string.Format("              ,MAX(A.HEAT     ) AS HEAT ");
                sql1 += string.Format("              ,MAX(A.STEEL    ) AS STEEL ");
                sql1 += string.Format("              ,MAX(A.ITEM     ) AS ITEM ");
                sql1 += string.Format("              ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
                sql1 += string.Format("              ,MAX(A.LENGTH   ) AS LENGTH ");
                sql1 += string.Format("              ,COUNT(*)         AS INSP_PCS ");
                sql1 += string.Format("              ,SUM(DECODE(GOOD_YN, 'OK', 1, 0) ) AS OK_PCS ");
                sql1 += string.Format("              ,SUM(DECODE(A.MAT_GOOD_NG,'NG',1,0) ) AS MAT_NG_PCS "); //--작업본수
                sql1 += string.Format("              ,SUM(DECODE(A.MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',0,DECODE(A.MLFT_GOOD_NG,'NG',1,0))) ) AS MLFT_NG_PCS "); //--합격본수
                sql1 += string.Format("              ,SUM(DECODE(A.MAT_GOOD_NG,'NG',0,DECODE(A.UT_GOOD_NG,'NG',1,0)) ) AS UT_NG_PCS "); //--결함본수
                sql1 += string.Format("              ,SUM(CASE WHEN A.MAT_GOOD_NG = 'NG' OR A.MLFT_GOOD_NG = 'NG' OR A.UT_GOOD_NG   = 'NG' THEN 1 ELSE 0 END) AS ALL_NG_PCS  "); //--MAT
                sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ,TB_CR_ORD B ");
                sql1 += string.Format("        WHERE A.POC_NO        = B.POC_NO ");
                sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
                sql1 += string.Format("        AND   A.ROUTING_CD = 'F2'  "); //--MPI
                
                if (txtPoc.Text != "")
                {
                    sql1 += string.Format("        AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR    ");
                    sql1 += string.Format("                                                        WHERE  MILL_NO    = A.MILL_NO   ");
                    sql1 += string.Format("                                                        AND    PIECE_NO   = A.PIECE_NO   ");
                    sql1 += string.Format("                                                        AND    LINE_GP    = A.LINE_GP   ");
                    sql1 += string.Format("                                                        AND    ROUTING_CD = A.ROUTING_CD ) ");
                    sql1 += string.Format("        AND A.POC_NO IN (SELECT POC_NO FROM TB_CR_ORD WHERE HEAT = '{0}') ", txtPoc.Text);
                    
                }
                else
                {
                    sql1 += string.Format("        AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR    ");
                    sql1 += string.Format("                                                        WHERE  MILL_NO    = A.MILL_NO   ");
                    sql1 += string.Format("                                                        AND    PIECE_NO   = A.PIECE_NO   ");
                    sql1 += string.Format("                                                        AND    LINE_GP    = A.LINE_GP   ");
                    sql1 += string.Format("                                                        AND    ROUTING_CD = A.ROUTING_CD  ");
                    sql1 += string.Format("                                                        AND    POC_NO IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
                    sql1 += string.Format("                                                                           WHERE  ROUTING_CD    = 'F2' ");
                    sql1 += string.Format("                                                                           AND   LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
                    sql1 += string.Format("                                                                           AND   MFG_DATE   = '{0}' )) ", mfg_date); //P_MFG_DATE
                    sql1 += string.Format("               AND A.POC_NO       IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
                    sql1 += string.Format("                                         WHERE  ROUTING_CD    = 'F2' ");
                    sql1 += string.Format("                                         AND   LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
                    sql1 += string.Format("                                         AND   MFG_DATE   = '{0}' ) ", mfg_date); //P_MFG_DATE
                }
                sql1 += string.Format("       GROUP BY A.POC_NO, A.STEEL, A.HEAT, A.ITEM_SIZE, A.LENGTH ");
                sql1 += string.Format("ORDER BY POC_NO) X ");



                //sql1 = string.Format("SELECT ROWNUM AS L_NO ");
                //sql1 += string.Format("       ,POC_NO");
                //sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                //sql1 += string.Format("       ,HEAT ");
                //sql1 += string.Format("       ,ITEM_SIZE ");
                //sql1 += string.Format("       ,LENGTH ");
                //sql1 += string.Format("       ,NVL(WORK_PCS, 0) AS WORK_PCS ");
                //sql1 += string.Format("       ,NVL(OK_PCS, 0) AS OK_PCS ");
                //sql1 += string.Format("       ,NVL(FAULT_PCS, 0) AS FAULT_PCS ");
                //sql1 += string.Format("       ,NVL(MAT_PCS, 0) AS MAT_PCS ");
                //sql1 += string.Format("       ,NVL(MLFT_PCS, 0) AS MLFT_PCS ");
                //sql1 += string.Format("       ,NVL(UT_PCS, 0) AS UT_PCS ");
                //sql1 += string.Format("       ,''  AS GUBUN ");
                //sql1 += string.Format("FROM   (      ");
                //sql1 += string.Format("         SELECT POC_NO ");
                //sql1 += string.Format("              ,ITEM_SIZE ");
                //sql1 += string.Format("              ,STEEL ");
                //sql1 += string.Format("              ,HEAT ");
                //sql1 += string.Format("              ,LENGTH ");
                //sql1 += string.Format("              ,SUM(DECODE(GOOD_YN, 'OK', 1, 0)) + SUM(DECODE(GOOD_YN, 'NG', 1, 0) ) AS WORK_PCS "); //--작업본수
                //sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'OK',1,0)      ) AS OK_PCS "); //--합격본수
                //sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',1,0)      ) AS FAULT_PCS "); //--결함본수
                //sql1 += string.Format("              ,SUM(DECODE(MAT_GOOD_NG, 'NG', 1, 0) ) AS MAT_PCS  "); //--MAT
                //sql1 += string.Format("              ,SUM(DECODE(A.MLFT_GOOD_NG, 'NG', DECODE(A.MAT_GOOD_NG, 'NG', 0, DECODE(A.UT_GOOD_NG, 'NG', 0, 1)), 0)) AS MLFT_PCS  "); //--MLFT
                //sql1 += string.Format("              ,SUM(DECODE(A.UT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,1),0))   AS UT_PCS  "); //--UT
                //sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ");
                
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
                //sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
                //sql1 += string.Format("        AND   A.ROUTING_CD = 'F2'  "); //--MPI
                //sql1 += string.Format("        AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR    ");
                //sql1 += string.Format("                                                        WHERE  MILL_NO    = A.MILL_NO   ");
                //sql1 += string.Format("                                                        AND    PIECE_NO   = A.PIECE_NO   ");
                //sql1 += string.Format("                                                        AND    LINE_GP    = A.LINE_GP   ");
                //sql1 += string.Format("                                                        AND    ROUTING_CD = A.ROUTING_CD  ");
                //sql1 += string.Format("                                                        AND    POC_NO IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
                //sql1 += string.Format("                                                                           WHERE  ROUTING_CD    = 'F2' ");
                //sql1 += string.Format("                                                                           AND   LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
                //sql1 += string.Format("                                                                           AND   MFG_DATE   = '{0}' )) ", mfg_date); //P_MFG_DATE
                //sql1 += string.Format("        GROUP BY POC_NO, STEEL, HEAT, ITEM_SIZE, LENGTH ");
                //sql1 += string.Format(") X ");
            }

            else
            {
                sql1 = string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,POC_NO");
                sql1 += string.Format("       ,FN_GET_INFO(POC_NO, 'K') WORK_DATE");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,NVL(WORK_PCS, 0) AS WORK_PCS ");
                sql1 += string.Format("       ,NVL(OK_PCS, 0) AS OK_PCS ");
                sql1 += string.Format("       ,NVL(FAULT_PCS, 0) AS FAULT_PCS ");
                sql1 += string.Format("       ,NVL(MAT_PCS, 0) AS MAT_PCS ");
                sql1 += string.Format("       ,NVL(MLFT_PCS, 0) AS MLFT_PCS ");
                sql1 += string.Format("       ,FN_GET_INFO(POC_NO, 'M') MLFT2_PCS ");
                sql1 += string.Format("       ,NVL(UT_PCS, 0) AS UT_PCS ");
                sql1 += string.Format("       ,''  AS GUBUN ");
                sql1 += string.Format("FROM   (      ");
                sql1 += string.Format("         SELECT POC_NO ");
                sql1 += string.Format("              ,ITEM_SIZE ");
                sql1 += string.Format("              ,STEEL ");
                sql1 += string.Format("              ,HEAT ");
                sql1 += string.Format("              ,LENGTH ");
                sql1 += string.Format("              ,SUM(DECODE(GOOD_YN, 'OK', 1, 0)) + SUM(DECODE(GOOD_YN, 'NG', 1, 0) ) AS WORK_PCS "); //--작업본수
                sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'OK',1,0)      ) AS OK_PCS "); //--합격본수
                sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',1,0)      ) AS FAULT_PCS "); //--결함본수
                sql1 += string.Format("              ,SUM(DECODE(MAT_GOOD_NG, 'NG', 1, 0) ) AS MAT_PCS  "); //--MAT
                sql1 += string.Format("              ,SUM(DECODE(A.MLFT_GOOD_NG, 'NG', DECODE(A.MAT_GOOD_NG, 'NG', 0, DECODE(A.UT_GOOD_NG, 'NG', 0, 1)), 0)) AS MLFT_PCS  "); //--MLFT
                sql1 += string.Format("              ,SUM(DECODE(A.UT_GOOD_NG,'NG',DECODE(A.MAT_GOOD_NG,'NG',0,1),0))   AS UT_PCS  "); //--UT
                sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ");
                if (txtPoc.Text != "")
                {
                    //sql1 += string.Format("               WHERE POC_NO   = '{0}' ", txtPoc.Text);
                    sql1 += string.Format("               WHERE HEAT   = '{0}' ", txtPoc.Text);
                }
                else
                {
                    //sql1 += string.Format("               WHERE POC_NO       IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
                    sql1 += string.Format("               WHERE POC_NO       IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
                    sql1 += string.Format("                                         WHERE  ROUTING_CD    = 'F2' ");
                    sql1 += string.Format("                                         AND   LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
                    sql1 += string.Format("                                         AND   MFG_DATE   = '{0}' ) ", mfg_date); //P_MFG_DATE
                }
                sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
                sql1 += string.Format("        AND   A.ROUTING_CD = 'F2'  "); //--MPI
                sql1 += string.Format("        GROUP BY POC_NO, STEEL, HEAT, ITEM_SIZE, LENGTH ");
                sql1 += string.Format(") X ");





            }

            olddt = cd.FindDataTable(sql1);

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(olddt, null, true);

            this.Cursor = System.Windows.Forms.Cursors.Default;
            //소계, 계 BackColor
            for (int iRow = 1; iRow < grdMain.Rows.Count; iRow++)
            {
                if (grdMain[iRow, 2].ToString().Trim().Replace(" ", "") == "소계")
                    grdMain.Rows[iRow].StyleNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));

                if (grdMain[iRow, 1].ToString().Trim().Replace(" ", "") == "계")
                    grdMain.Rows[iRow].StyleNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            }

            if (olddt.Rows.Count > 0)
            {
                UpdateTotals();
                int gubun_index = grdMain.Cols["POC_NO"].Index;
                grdMain.Rows[2].StyleNew.BackColor = Color.Blue;
                //grdMain.Rows[2].StyleNew.BackColor = Color.FromArgb(255, 81, 181, 255);
                //grdMain.Rows[2].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);
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

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["WORK_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["OK_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["FAULT_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["MAT_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["MLFT_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["MLFT2_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["UT_PCS"].Index, "합계");
            

            AddSubtotalNo();
            grdMain.Rows.Frozen = GetAvailMinRow(grdMain) - 1;
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

        private bool SetComboinGrd()
        {
            try
            {
                fault_cd = new ListDictionary();
                DataTable dt2 = cd.Find_CD("FAULT_CD");

                foreach (DataRow row in dt2.Rows)
                {
                    fault_cd.Add(row["CD_ID"].ToString(), row["CD_ID"].ToString() + "  " + row["CD_NM"].ToString());
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

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
            if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            //디비선언
            string strQry = string.Empty;
            string strMsg = string.Empty;
            string wTime = "";
            DateTime time = new DateTime();

            int row = 0;
            int UpCnt = 0;

            string strLog = string.Empty;
            var itemList = new List<DictionaryList>();
            var logList = new List<LogDataList>();



            #region grdMain1
            for (row = 3; row < grdMain.Rows.Count; row++)
            {
                gubun_values = grdMain.GetData(row, "GUBUN").ToString();
                
                //gubun_values = grdMain.GetData(row, 15).ToString();
                string mfg_date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();
                string to_date = vf.Format(DateTime.Now.Date, "yyyyMMdd").ToString();
                string yest_date = vf.Format(DateTime.Now.Date.AddDays(-1), "yyyyMMdd").ToString();
                int mfg = Convert.ToInt32(mfg_date);

                olddt = new DataTable();
                string sql1 = "";
                sql1 = string.Format("SELECT POC_NO ");
                sql1 += string.Format("FROM L2USER.TB_PROG_POC_MGMT ");
                sql1 += string.Format("WHERE  LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP);

                olddt = cd.FindDataTable(sql1);
                string POC_MGMT = olddt.Rows[0]["POC_NO"].ToString();



                //if (mfg_date != to_date) 
                //{
                //    if (mfg_date != yest_date)
                //    {
                //        MessageBox.Show(" 해당일자실적은 수정불가합니다. 관리자에게 문의하세요");
                //        return;
                //    }
                //}

                // Update 처리
                if (gubun_values == "수정")
                {
                    string POC_ROW = grdMain.GetData(row, "POC_NO").ToString();

                    olddt = new DataTable();
                    string sql2 = "";
                    sql2 = string.Format("SELECT FN_GET_CLOSING('{0}') AS GET_DATE ", POC_ROW);
                    sql2 += string.Format("FROM DUAL ");

                    olddt = cd.FindDataTable(sql2);
                    string POC_MGMT1 = olddt.Rows[0]["GET_DATE"].ToString();
                    int get = Convert.ToInt32(POC_MGMT1);

                    //if (mfg < get)
                    //{

                    //    MessageBox.Show(" 해당생산일자는 마감이 완료되어 수정불가합니다.");
                    //    setDataBinding();
                    //    return;

                    //}

                    //if (POC_MGMT == POC_ROW)
                    //{

                    //    MessageBox.Show(" 진행중인 POC는 수정불가합니다.");
                    //    setDataBinding();
                    //    return;

                    //}
                    if (uC_Line_gp_s1.Line_GP == "#3")
                    {
                        spName = "SP_NDT_WR_PIECE_UPD_M3_NEW";  // 조회용 저장프로시저명
                    }
                    else
                    {
                        spName = "SP_NDT_WR_PIECE_UPD_M_NEW";  // 조회용 저장프로시저명
                    }

                    OracleConnection oConn = cd.OConnect();
                    OracleCommand cmd = new OracleCommand();


                    try
                    {
                        cmd.Connection = oConn;
                        cmd.CommandText = spName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter op;
                        string sDelGp = "";
                        string sLineGp = "";
                        string sBundleNo = "";
                        string sWorkType = "";
                        string sWorkTeam = "";

                            
                        op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                        op.Direction = ParameterDirection.Input;
                        op.Value = uC_Line_gp_s1.Line_GP;
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "POC_NO").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_MFG_DATE", OracleType.VarChar);
                        op.Direction = ParameterDirection.Input;
                        op.Value = mfg_date;
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_OK_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "OK_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_MAT_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "MAT_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_MLFT_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "MLFT_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_MLFT2_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "MLFT2_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_UT_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "UT_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                        op.Direction = ParameterDirection.Output;
                        op.Size = 4000;
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                        op.Direction = ParameterDirection.Output;
                        op.Size = 4000;
                        cmd.Parameters.Add(op);

                        oConn.Open();
                        transaction = cmd.Connection.BeginTransaction();
                        cmd.Transaction = transaction;

                        cmd.ExecuteOracleScalar();

                        string result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                        string result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                        transaction.Commit();

                            
                       MessageBox.Show(result_msg);

                    }
                    catch (Exception ex)
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
                        if (cmd.Connection != null)
                            cmd.Connection.Close();       //데이터베이스연결해제
                        if (transaction != null)
                            transaction.Dispose();

                        //this.Close();
                    }

                    UpCnt++;
                }
            }
            setDataBinding();
            #endregion grdMain1 


        }
           
        private void grdMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
