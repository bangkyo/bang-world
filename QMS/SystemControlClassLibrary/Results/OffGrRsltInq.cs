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
    public partial class OffGrRsltInq : Form
    {
        clsCom ck = new clsCom();

        OracleTransaction transaction = null;


        private static string cd_id = "";

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        clsStyle cs = new clsStyle();

        ListDictionary ngTest_Value = null;
        ListDictionary fault_cd = null;

        DataTable olddt;
        DataTable moddt;

        string GOOD_YN = "";
        string WORK_PCS = "";

        private int subtotalNo;

        private string strBefValues = "";

        private string ownerNM = "";
        private string titleNM = "";
        string gubun_values = string.Empty;



        //private UC.sub_UC.UC_Line_gp_s uC_Line_gp_s1;
        private UC.sub_UC.UC_Work_Day uC_Work_Day1;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public OffGrRsltInq(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void OffGrRsltInq_Load(object sender, EventArgs e)
        {
            InitControl();

            SetComboBox1();

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

            clsStyle.Style.InitCombo(cboLine_GP, StringAlignment.Near);

            #region 유저컨트롤 설정


            int location_x = 0;
            int location_y = 0;
            //
            // uC_Line_gp_s1
            // 
            //uC_Line_gp_s1 = new UC_Line_gp_s();
            //uC_Line_gp_s1.BackColor = System.Drawing.Color.Transparent;
            //uC_Line_gp_s1.cb_Enable = true;
            //uC_Line_gp_s1.Line_GP = ck.Line_gp;
            //uC_Line_gp_s1.Location = new System.Drawing.Point(333 + 50 + location_x, 7 + location_y); //location_x
            //uC_Line_gp_s1.Location = new System.Drawing.Point(333 + 50 + location_x, 7 + location_y); //location_x
            //uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            //uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            //uC_Line_gp_s1.TabIndex = 1;
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

            grdMain.BackColor = Color.White;

            var crCellRange = grdMain.GetCellRange(1, grdMain.Cols["NG_PCS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["A12_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["MFG_DATE"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(0, grdMain.Cols["OK_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["A13_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["A14_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["A15_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["A16_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["A17_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["B11_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["B12_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["B13_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["B14_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["B15_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["B16_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["B17_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["ETC_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(1, grdMain.Cols["FAULT_CD"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            grdMain.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            //grdMain.Dock = DockStyle.Fill;
            //grdMain.AllowEditing = false;

            int level1 = 50; // 2자리
            int level2 = 50; // 4자리
            int level2_5 = 60;
            int level3 = 90; // 6자리
            int level4 = 140; // 8자리이상

            #region 1. grdMain head 및 row  align 설정

            grdMain.Rows[1].Height = 40;

            grdMain[1, "L_NO"] = grdMain.Cols["L_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "WORK_TYPE_NM"] = grdMain.Cols["WORK_TYPE_NM"].Caption;
            //grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "ITEM_SIZE"] = grdMain.Cols["ITEM_SIZE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STEEL_NM"] = grdMain.Cols["STEEL_NM"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "LENGTH"] = grdMain.Cols["LENGTH"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "HEAT"] = grdMain.Cols["HEAT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MFG_DATE"] = grdMain.Cols["MFG_DATE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MPI_PCS"] = grdMain.Cols["MPI_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "OK_PCS"] = grdMain.Cols["OK_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "NG_PCS"] = grdMain.Cols["NG_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "WORK_PCS"] = grdMain.Cols["WORK_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "GOOD_YN"] = grdMain.Cols["GOOD_YN"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MPI_PCS"] = "  MPI 본수";
            grdMain[1, "OK_PCS"] = "   합격";
            grdMain[1, "NG_PCS"] = "   격외";
            grdMain[1, "WORK_PCS"] = " 계";
     

            grdMain.Cols["L_NO"].Width = 0;
            //grdMain.Cols["WORK_TYPE_NM"].Width = level2;
            grdMain.Cols["ITEM_SIZE"].Width = level4;
            grdMain.Cols["STEEL_NM"].Width = level4;
            grdMain.Cols["MFG_DATE"].Width = level4;
            grdMain.Cols["LENGTH"].Width = level4;
            grdMain.Cols["HEAT"].Width = level4;
            grdMain.Cols["MPI_PCS"].Width = 90;
            grdMain.Cols["OK_PCS"].Width = 90;
            grdMain.Cols["NG_PCS"].Width = 90;
            grdMain.Cols["WORK_PCS"].Width = 90;
            grdMain.Cols["GOOD_YN"].Width = 90;
            grdMain.Cols["GUBUN"].Width = 0;


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            //grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            
            grdMain.Cols["LENGTH"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["MPI_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["WORK_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["OK_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["GOOD_YN"].TextAlign = cs.HEAT_TextAlign;
                     

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
            grdMain.Cols["GOOD_YN"].AllowEditing = false;
            grdMain.Cols["MFG_DATE"].AllowEditing = false;
            grdMain.Cols["MPI_PCS"].AllowEditing = false;
            grdMain.Cols["WORK_PCS"].AllowEditing = false;
            grdMain.Cols["OK_PCS"].AllowEditing = true;
            grdMain.Cols["GUBUN"].AllowEditing = false;

            #region 콤보박스 설정


            #endregion 콤보박스 설정

            grdMain.ExtendLastCol = true;

        }

        private void SetComboBox1()
        {
            //cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
            //cd.SetCombo(cboLine_GP, "LINE_GP", "", false, "#3");
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
            sql1 += string.Format("       ,MFG_DATE");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
            sql1 += string.Format("       ,HEAT ");
            sql1 += string.Format("       ,ITEM_SIZE ");
            sql1 += string.Format("       ,LENGTH ");
            sql1 += string.Format("       ,FN_GET_INFO_GR(HEAT, '{0}') MPI_PCS ", mfg_date); //P_MFG_DATE
            sql1 += string.Format("       ,NVL(OK_PCS, 0) AS OK_PCS ");
            sql1 += string.Format("       ,NVL(NG_PCS, 0) AS NG_PCS ");
            sql1 += string.Format("       ,NVL(INSP_PCS, 0) AS WORK_PCS ");
            sql1 += string.Format("       ,CASE WHEN FN_GET_INFO_GR(HEAT, '{0}') - NVL(INSP_PCS, 0) = 0 THEN 'OK' ", mfg_date); //P_MFG_DATE
            sql1 += string.Format("             WHEN FN_GET_INFO_GR(HEAT, '{0}') - NVL(INSP_PCS, 0) <> 0 THEN 'NG' ", mfg_date); //P_MFG_DATE
            sql1 += string.Format("       END  AS GOOD_YN ");
            sql1 += string.Format("       ,''  AS GUBUN ");
            sql1 += string.Format("FROM   (      ");
            sql1 += string.Format("         SELECT MAX(A.MFG_DATE) AS MFG_DATE ");
            sql1 += string.Format("              ,MAX(A.HEAT ) AS HEAT ");
            sql1 += string.Format("              ,MAX(A.STEEL ) AS STEEL ");
            sql1 += string.Format("              ,MAX(A.ITEM ) AS ITEM ");
            sql1 += string.Format("              ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
            sql1 += string.Format("              ,MAX(A.LENGTH ) AS LENGTH ");
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN, 'OK', 1, 0) ) AS OK_PCS "); //--작업본수
            sql1 += string.Format("              ,SUM(DECODE(GOOD_YN, 'NG', 1, 0) ) AS NG_PCS "); //--합격본수
            sql1 += string.Format("              ,COUNT(*) AS INSP_PCS "); //--결함본수
            sql1 += string.Format("FROM TB_CR_PIECE_WR A  "); //--종크랙
            sql1 += string.Format("WHERE A.LINE_GP = '#3'  "); //--입계크랙
            sql1 += string.Format("AND A.ROUTING_CD = 'K2'  "); //--횡크랙
            sql1 += string.Format("AND A.POC_NO       IN ( SELECT DISTINCT POC_NO FROM  TB_CR_PIECE_WR ");
            sql1 += string.Format("                         WHERE  ROUTING_CD    = 'F2' ");
            sql1 += string.Format("                          AND   LINE_GP    = '#3' ");
            sql1 += string.Format("                          AND   MFG_DATE   = '{0}' ) ", mfg_date); //P_MFG_DATE
            //sql1 += string.Format("AND A.MFG_DATE   = '{0}'  ", mfg_date); //P_MFG_DATE
            sql1 += string.Format("GROUP BY A.MFG_DATE, A.STEEL, A.HEAT, A.ITEM_SIZE, A.LENGTH ");
            sql1 += string.Format("ORDER BY MFG_DATE, STEEL, HEAT, ITEM_SIZE, LENGTH ");
            sql1 += string.Format(") X ");


            olddt = cd.FindDataTable(sql1);

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(olddt, null, true);

           

            var grdCount = olddt.Rows.Count;


            for (int rowCnt = 0; rowCnt < olddt.Rows.Count; rowCnt++)
            {
                GOOD_YN = olddt.Rows[rowCnt]["GOOD_YN"].ToString();
                WORK_PCS = olddt.Rows[rowCnt]["WORK_PCS"].ToString();
                //grdMain.SetData(rowCnt, "CHECKER", false);
                if (GOOD_YN == "NG")
                {
                    grdMain.Rows[rowCnt+2].Style = grdMain.Styles["DelColor"];
                    //txtMemo1.Text = "수출품이 포함된 POC 입니다.";

                }
                else
                {
                    grdMain.Rows[rowCnt+2].Style = grdMain.Styles["BACColor"];
                    //txtMemo1.Text = "";
                }

            }

           

            this.Cursor = System.Windows.Forms.Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;

        }
        #endregion 조업일보 조회 설정(SQL)

        private void UpdateTotals()
        {

            subtotalNo = -1;

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["MLFT_NG_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["WORK_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["OK_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["FAULT_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["A11_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["A12_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["A13_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["A14_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["A15_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["A16_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["A17_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B11_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B12_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B13_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B14_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B15_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B16_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["B17_PCS"].Index, "합계");
            
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["ETC_PCS"].Index, "합계");

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
                //string mfg_date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();
                string mfg_date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();
                string to_date = vf.Format(DateTime.Now.Date, "yyyyMMdd").ToString();
                string to_date1 = vf.Format(DateTime.Now.AddHours(-6), "yyyyMMdd").ToString();
                int mfg = Convert.ToInt32(mfg_date);

                olddt = new DataTable();
                string sql1 = "";
                sql1 = string.Format("SELECT POC_NO ");
                sql1 += string.Format("FROM L2USER.TB_PROG_POC_MGMT ");
                sql1 += string.Format("WHERE  LINE_GP    = '{0}' ", cd_id);

                olddt = cd.FindDataTable(sql1);
                string POC_MGMT = olddt.Rows[0]["POC_NO"].ToString();

                //if (mfg_date != to_date1)
                //{
                //    MessageBox.Show(" 당일실적만 수정가능합니다. 관리자에게 문의하세요");
                //    setDataBinding();
                //    return;
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
                    string spName = "SP_MPI_WR_PIECE_UPD_M_NEW";  // 조회용 저장프로시저명

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
                        op.Value = cd_id;
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "POC_NO").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_MFG_DATE", OracleType.VarChar);
                        op.Direction = ParameterDirection.Input;
                        //op.Value = mfg_date;
                        op.Value = grdMain.GetData(row, "MFG_DATE").ToString();
                        cmd.Parameters.Add(op);

                        //op = new OracleParameter("P_WORK_TYPE", OracleType.VarChar);
                        //op.Direction = ParameterDirection.Input;
                        //op.Value = grdMain.GetData(row, "WORK_TYPE_NM").ToString();
                        //cmd.Parameters.Add(op);

                        op = new OracleParameter("P_A11_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "A11_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_A12_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "A12_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_A13_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "A13_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_A14_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "A14_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_A15_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "A15_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_A16_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "A16_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_A17_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "A17_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_B11_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "B11_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_B12_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "B12_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_B13_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "B13_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_B14_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "B14_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_B15_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "B15_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_B16_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "B16_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_B17_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "B17_PCS").ToString();
                        cmd.Parameters.Add(op);


                        op = new OracleParameter("P_FAULT_CD", OracleType.VarChar);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "FAULT_CD").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_ETC_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "ETC_PCS").ToString();
                        cmd.Parameters.Add(op);

                        op = new OracleParameter("P_OK_PCS", OracleType.Number);
                        op.Direction = ParameterDirection.Input;
                        op.Value = grdMain.GetData(row, "OK_PCS").ToString();
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

                        if (result_stat == "ERR")
                        {
                            MessageBox.Show(result_msg);
                        }
                        

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
            //AddProcedure();
            #endregion grdMain1 



        }

        private void cboLine_GP_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cd_id = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = cd_id;
        }

        private void AddProcedure()
        {
            string spName = "SP_MPIGR_WR_SUM";  // 조회용 저장프로시저명
            string mfg_date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();
            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();


            try
            {
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;



                op = new OracleParameter("P_FR_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = mfg_date;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_TO_DATE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = mfg_date;
                cmd.Parameters.Add(op);

                oConn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.ExecuteOracleScalar();


                transaction.Commit();


                //MessageBox.Show(result_msg);

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

            
        }
        private void grdMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
