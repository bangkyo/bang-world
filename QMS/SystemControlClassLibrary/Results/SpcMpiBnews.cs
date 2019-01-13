using ComLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControlClassLibrary.UC.sub_UC;
using ComLib.clsMgr;

namespace SystemControlClassLibrary.Results
{
    public partial class SpcMpiBnews : Form
    {
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        private string line_gp = "";

        clsStyle cs = new clsStyle();

        DataTable olddt;
        DataTable moddt;

        private string ownerNM = "";
        private string titleNM = "";


        //private UC.sub_UC.UC_Line_gp_s uC_Line_gp_s1;
        private UC.sub_UC.UC_Work_Day uC_Work_Day1;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public SpcMpiBnews(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void cboLine_GP_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            line_gp = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = line_gp;
        }

        private void SpcMpiBnews_Load(object sender, EventArgs e)
        {
            InitControl();

            SetComboBox1();

            btnDisplay_Click(null, null);
        }

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitButton(btnExcel);

            cs.InitButton(btnDisplay);

            cs.InitButton(btnClose);

            cs.InitCombo(cboLine_GP, StringAlignment.Near);

            #region 유저컨트롤 설정


            int location_x = 0;
            int location_y = 0;
            //
            // uC_Line_gp_s1
            // 
            //uC_Line_gp_s1 = new UC_Line_gp_s();
            //uC_Line_gp_s1.BackColor = System.Drawing.Color.Transparent;
            //uC_Line_gp_s1.cb_Enable = true;
            //uC_Line_gp_s1.Location = new System.Drawing.Point(333 + 50 + location_x, 7 + location_y); //location_x
            //uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            //uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            //uC_Line_gp_s1.TabIndex = 1;
            //// 
            // uC_Work_Day1
            // 
            uC_Work_Day1 = new UC_Work_Day();
            uC_Work_Day1.BackColor = System.Drawing.Color.Transparent;
            uC_Work_Day1.Location = new System.Drawing.Point(26 + 24 + location_x, 7 + location_y); //(26 + 24 , 7 + location_y); //location_x
            uC_Work_Day1.Name = "uC_Work_Day1";
            uC_Work_Day1.Size = new System.Drawing.Size(270, 27);
            uC_Work_Day1.TabIndex = 2;
            

            panel1.Controls.Add(this.uC_Work_Day1);
            //panel1.Controls.Add(this.uC_Line_gp_s1);

            InitGrd_Main();

            // controll init data
            //uC_Line_gp_s1.Line_GP = ck.Line_gp;
            uC_Work_Day1.Work_Day = DateTime.Now.Date;
            #endregion
        }
        private void InitGrd_Main()
        {
            cs.InitGrid_search(grdMain, false, 2, 1);

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            //grdMain.Dock = DockStyle.Fill;
            //grdMain.AllowEditing = false;

            int level1 = 50; // 2자리
            int level2 = 50; // 4자리
            int level2_5 = 80;
            int level3 = 100; // 6자리
            int level4 = 140; // 8자리이상

            #region 1. grdMain head 및 row  align 설정
            grdMain[1, "L_NO"] = grdMain.Cols["L_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "WORK_TYPE_NM"] = grdMain.Cols["WORK_TYPE_NM"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "ITEM_SIZE"] = grdMain.Cols["ITEM_SIZE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STEEL_NM"] = grdMain.Cols["STEEL_NM"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "HEAT"] = grdMain.Cols["HEAT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "POC_NO"] = grdMain.Cols["POC_NO"].Caption;
            //grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "WORK_PCS"] = grdMain.Cols["WORK_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "FAULT_PCS"] = grdMain.Cols["FAULT_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "B11_PCS"] = "크랙";
            grdMain[1, "B14_PCS"] = "긁힘";
            grdMain[1, "B17_PCS"] = "접힘";
            grdMain[1, "B13_PCS"] = "딱지";
            grdMain[1, "B16_PCS"] = "주름";
            grdMain[1, "ETC_PCS"] = "기타";
            grdMain[1, "REMARKS"] = "비고";

            grdMain.Cols["L_NO"].Width = 0;
            grdMain.Cols["WORK_TYPE_NM"].Width = level2;
            grdMain.Cols["ITEM_SIZE"].Width = level2_5;
            grdMain.Cols["STEEL_NM"].Width = level3;
            grdMain.Cols["HEAT"].Width = level3;
            //grdMain.Cols["POC_NO"].Width = level3;
            grdMain.Cols["WORK_PCS"].Width = 70;
            grdMain.Cols["FAULT_PCS"].Width = 70;


            grdMain.Cols["B11_PCS"].Width = level2_5;
            grdMain.Cols["B14_PCS"].Width = level2_5;
            grdMain.Cols["B17_PCS"].Width = level2_5;
            grdMain.Cols["B13_PCS"].Width = level2_5;
            grdMain.Cols["B16_PCS"].Width = level2_5;
            grdMain.Cols["ETC_PCS"].Width = level2_5;
            grdMain.Cols["REMARKS"].Width = level2_5;



            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            //grdMain.Cols["POC_NO"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["WORK_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["FAULT_PCS"].TextAlign = cs.PCS_TextAlign;

            grdMain.Cols["B11_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["B14_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["B17_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["B13_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["B16_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["ETC_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["REMARKS"].TextAlign = cs.REMARKS_TextAlign;

            #endregion

            grdMain.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                grdMain.Cols[i].AllowMerging = true;
            }

            grdMain.Rows[0].AllowMerging = true;
        }

        private void SetComboBox1()
        {
            cd.SetComboS(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
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
            


            sql1 = string.Format("SELECT ROWNUM AS L_NO ");
            sql1 += string.Format("       ,NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE),WORK_TYPE) AS WORK_TYPE_NM ");
            sql1 += string.Format("       ,ITEM_SIZE");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
            sql1 += string.Format("       ,HEAT ");
            //sql1 += string.Format("       ,POC_NO ");
            sql1 += string.Format("       ,WORK_PCS ");
            sql1 += string.Format("       ,FAULT_PCS ");
            sql1 += string.Format("       ,B11_PCS ");
            sql1 += string.Format("       ,B14_PCS ");
            sql1 += string.Format("       ,B17_PCS ");
            sql1 += string.Format("       ,B13_PCS ");
            sql1 += string.Format("       ,B16_PCS ");
            sql1 += string.Format("       ,ETC_PCS ");
            sql1 += string.Format("       ,''  AS REMARKS ");
            sql1 += string.Format("FROM   (      ");
            sql1 += string.Format("       SELECT  DECODE(GROUPING(WORK_TYPE),1,'계',WORK_TYPE) AS WORK_TYPE ");
            sql1 += string.Format("              ,DECODE(GROUPING(ITEM_SIZE),1,(DECODE(GROUPING(WORK_TYPE),1,'','소계')),ITEM_SIZE) AS ITEM_SIZE ");
            sql1 += string.Format("              ,STEEL ");
            sql1 += string.Format("              ,HEAT ");
            //sql1 += string.Format("              ,POC_NO ");
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN, 'OK', 1, 0)) + SUM(DECODE(GOOD_YN, 'NG', 1, 0) ) AS WORK_PCS "); //--작업본수
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',1,0)      ) AS FAULT_PCS "); //--결함본수
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',DECODE(MPI_FAULT_CD,'AR0',1,0))) AS B11_PCS  "); //--크랙
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',DECODE(MPI_FAULT_CD,'B12',1,0))) AS B14_PCS  "); //--긁힘
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',DECODE(MPI_FAULT_CD,'BR2',1,0))) AS B17_PCS  "); //--접힘
            //sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',DECODE(MPI_FAULT_CD,'B02',1,0))) AS B13_PCS  "); //--딱지
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN, 'NG', CASE WHEN MPI_FAULT_CD IN ( 'BR1', 'B02') THEN 1 ELSE 0  END)) AS B13_PCS  "); //--딱지
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',DECODE(MPI_FAULT_CD,'BR3',1,0))) AS B16_PCS  "); //--주름
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN,'NG',CASE WHEN MPI_FAULT_CD IN ('AR0','B12','BR1','B02','BR2', 'BR3') THEN 0 ");
            sql1 += string.Format("                    ELSE 1  END))  AS ETC_PCS "); //--기타
            sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ");
            sql1 += string.Format("        WHERE A.MFG_DATE   = '{0}' ", mfg_date); //P_MFG_DATE
            //sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", uC_Line_gp_s1.Line_GP); //P_LINE_GP
            sql1 += string.Format("        AND   A.LINE_GP    = '{0}' ", line_gp); //P_LINE_GP
            sql1 += string.Format("        AND   A.ROUTING_CD = 'H2'  "); //--MPI
            sql1 += string.Format("        AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            sql1 += string.Format("                                 WHERE  MILL_NO    = A.MILL_NO ");
            sql1 += string.Format("                                 AND    PIECE_NO   = A.PIECE_NO ");
            sql1 += string.Format("                                 AND    LINE_GP    = A.LINE_GP ");
            sql1 += string.Format("                                 AND    ROUTING_CD = A.ROUTING_CD ) ");
            sql1 += string.Format("        GROUP BY ROLLUP(WORK_TYPE, (ITEM_SIZE, STEEL, HEAT)) ");
            //sql1 += string.Format("        GROUP BY ROLLUP(WORK_TYPE, (POC_NO, ITEM_SIZE, STEEL)) ");
            sql1 += string.Format(") X ");
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


            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;

        }
        #endregion 조업일보 조회 설정(SQL)


        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count > 2)
            {
                vf.MakeExcelByTemplete1MPI(line_gp, titleNM, uC_Work_Day1.Work_Day, grdMain);
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
