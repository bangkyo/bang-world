using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemControlClassLibrary.UC;
using System.Windows.Forms;

namespace SystemControlClassLibrary.Order
{
    public partial class CrtInOrdCre : Form
    {
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        DataTable olddt;
        DataTable moddt;
        //DataTable logdt;

        DataTable grdMain_dt;
        DataTable grdSub_dt;

        DataTable olddt_sub;
        DataTable moddt_sub;
        //DataTable logdt_sub;

        DataTable temp_dt;

        //List<string> modifList;

        private UC.UC_ITEM_SIZE uC_ITEM_SIZE1;
        private UC.UC_LENGTH uC_LENGTH1;
        private UC.UC_STEEL uC_STEEL1;
        private UC.UC_WGT uC_WGT1;
        private UC.UC_LINE_GP uC_LINE_GP1;
        private UC.UC_DATE uC_DATE1;
        //private UC.UC_HEAT uC_HEAT1;


        //private int selectedrow = 0;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        //private string result_stat = "";
        //private string result_msg = "";

        private string ownerNM = "";
        private string titleNM = "";

        C1FlexGrid selectedGrd = null;

        //// search item
        //string item_size = "";
        //string steel = "";
        //string length_str = "";

        //string line_gp = "";
        //string crtin_dt_str = "";
        //string work_rank = "";
        //string wgt = "";
        


        public CrtInOrdCre(string titleNm, string scrAuth, string factCode, string ownerNm)
        {

            

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(temp));
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            //Icon = new Icon(Icon, Icon.Size);


            selectedGrd = grdMain;


            
        }

        private void CrtInOrdCre_Load(object sender, EventArgs e)
        {
            InitControl();

            MakeInitgrdData();

            ////초기값 설정
            //item_size = uC_ITEM_SIZE1.TEXT;
            //steel = uC_STEEL1.Steel;
            //length_str = uC_LENGTH1.TEXT;

            //line_gp = uC_LINE_GP1.Line_GP;
            //crtin_dt_str = vf.Format(uC_DATE1.Date, "yyyyMMdd");
            //work_rank = uC_WORK_RANK1.TEXT;
            //wgt = uC_WGT1.TEXT;


            // 초기값 설정
            //uC_LINE_GP1.Line_GP = "#1";
            //uC_DATE1.Date = DateTime.Now.Date;
            //uC_WGT1.WGT = "0";

            btnDisplay_Click(null, null);
        }

        private void MakeInitgrdData()
        {
            grdMain_dt = vf.CreateDataTable(grdMain);

            grdSub_dt = vf.CreateDataTable(grdSub);
        }

        private void InitGridData(bool isTotalInital)
        {
            if (isTotalInital)
            {
                grdMain.SetDataBinding(grdMain_dt, null, true);

                grdSub.SetDataBinding(grdSub_dt, null, true);
            }
            else
            {
                grdSub.SetDataBinding(grdSub_dt, null, true);
            }

            //라인 //투입지시일자 //순위 // 중량 초기화 필요

            uC_WGT1.WGT = "0";
            //uC_WORK_RANK1.WORK_RANK = "";
            
        }

        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);

            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitPanel(panel1);
            clsStyle.Style.InitPanel(panel2);

            clsStyle.Style.InitButton(btnExcel);

            clsStyle.Style.InitButton(btnDisplay);

            clsStyle.Style.InitButton(btnSave);

            clsStyle.Style.InitButton(btnClose);

            #region 유저 컨트롤 설정

            int location_x = 0;
            int location_y = 0;

            // 
            // uC_LENGTH1
            // 
            uC_LENGTH1 = new SystemControlClassLibrary.UC.UC_LENGTH();
            uC_LENGTH1.BackColor = System.Drawing.Color.Transparent;
            //uC_LENGTH1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            uC_LENGTH1.ITEM_SIZE = "";
            uC_LENGTH1.Length = "";
            uC_LENGTH1.Location = new System.Drawing.Point(710 + location_x, 6 + location_y);
            uC_LENGTH1.Margin = new System.Windows.Forms.Padding(0);
            uC_LENGTH1.Name = "uC_LENGTH1";
            uC_LENGTH1.Size = new System.Drawing.Size(223, 30);
            uC_LENGTH1.TabIndex = 2;
            // 
            // uC_HEAT1
            // 
            //uC_HEAT1 = new SystemControlClassLibrary.UC.UC_HEAT();
            //uC_HEAT1.BackColor = System.Drawing.Color.Transparent;
            ////uC_LENGTH1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ////uC_HEAT1.ITEM_SIZE = "";
            ////uC_HEAT1.Length = "";
            //uC_HEAT1.Location = new System.Drawing.Point(850 + location_x, 6 + location_y);
            //uC_HEAT1.Margin = new System.Windows.Forms.Padding(0);
            //uC_HEAT1.Name = "uC_HEAT1";
            //uC_HEAT1.Size = new System.Drawing.Size(223, 30);
            //uC_HEAT1.TabIndex = 3;
            // 
            // uC_STEEL1
            // 
            uC_STEEL1 = new SystemControlClassLibrary.UC.UC_STEEL();
            uC_STEEL1.BackColor = System.Drawing.Color.Transparent;
            //uC_STEEL1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            uC_STEEL1.Location = new System.Drawing.Point(360 + location_x, 6 + location_y);
            uC_STEEL1.Margin = new System.Windows.Forms.Padding(4);
            uC_STEEL1.Name = "uC_STEEL1";
            uC_STEEL1.Size = new System.Drawing.Size(234, 30);
            uC_STEEL1.Steel = "";
            uC_STEEL1.Steel_NM = "";
            uC_STEEL1.TabIndex = 1;
            // 
            // uC_ITEM_SIZE1
            // 
            uC_ITEM_SIZE1 = new SystemControlClassLibrary.UC.UC_ITEM_SIZE();
            uC_ITEM_SIZE1.BackColor = System.Drawing.Color.Transparent;
            //uC_ITEM_SIZE1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            uC_ITEM_SIZE1.ITEM_SIZE = "";
            uC_ITEM_SIZE1.Location = new System.Drawing.Point(51 + location_x, 6 + location_y);
            uC_ITEM_SIZE1.Margin = new System.Windows.Forms.Padding(0);
            uC_ITEM_SIZE1.Name = "uC_ITEM_SIZE1";
            uC_ITEM_SIZE1.Size = new System.Drawing.Size(174, 30);
            uC_ITEM_SIZE1.TabIndex = 0;

            // 
            // uC_DATE1
            // 
            uC_DATE1 = new SystemControlClassLibrary.UC.UC_DATE();
            uC_DATE1.BackColor = System.Drawing.Color.Transparent;
            uC_DATE1.Date = DateTime.Now.Date;
            uC_DATE1.Location = new System.Drawing.Point(360 + location_x, 6 + location_y);
            uC_DATE1.Name = "uC_DATE1";

            uC_DATE1.Size = new System.Drawing.Size(322, 30);
            uC_DATE1.TabIndex = 4;
            // 
            // uC_WGT1
            // 
            uC_WGT1 = new SystemControlClassLibrary.UC.UC_WGT();
            uC_WGT1.BackColor = System.Drawing.Color.Transparent;
            //uC_WGT1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            uC_WGT1.ITEM_SIZE = "";
            uC_WGT1.Location = new System.Drawing.Point(710 + location_x, 6 + location_y);
            uC_WGT1.Margin = new System.Windows.Forms.Padding(0);
            uC_WGT1.Name = "uC_WGT1";
            uC_WGT1.Size = new System.Drawing.Size(330, 30);
            uC_WGT1.TabIndex = 3;
            uC_WGT1.WGT = "0";
            // 
            // uC_LINE_GP1
            // 
            uC_LINE_GP1 = new SystemControlClassLibrary.UC.UC_LINE_GP();
            uC_LINE_GP1.BackColor = System.Drawing.Color.Transparent;
            uC_LINE_GP1.cb_Enable = true;
            //uC_LINE_GP1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            uC_LINE_GP1.Line_GP = ck.Line_gp;
            uC_LINE_GP1.Location = new System.Drawing.Point(51 + location_x, 6 + location_y);
            uC_LINE_GP1.Margin = new System.Windows.Forms.Padding(4);
            uC_LINE_GP1.Name = "uC_LINE_GP1";
            uC_LINE_GP1.Size = new System.Drawing.Size(161, 30);
            uC_LINE_GP1.TabIndex = 0;

            panel2.Controls.Add(this.uC_DATE1);
            panel2.Controls.Add(this.uC_WGT1);
            panel2.Controls.Add(this.uC_LINE_GP1);
            panel1.Controls.Add(this.uC_LENGTH1);
            //panel1.Controls.Add(this.uC_HEAT1);
            panel1.Controls.Add(this.uC_STEEL1);
            panel1.Controls.Add(this.uC_ITEM_SIZE1);

            #endregion

            InitGrd();
        }

        private void InitGrd()
        {
            InitGrd_grdMain();

            InitGrd_grdSub();
        }

        private void InitGrd_grdMain()
        {
            cs.InitGrid_search(grdMain);

            grdMain.AllowEditing = false;

            grdMain.AllowSorting = AllowSortingEnum.SingleColumn;

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            int nwidth = (1000 - cs.L_No_Width)/5;

            grdMain.Cols["ITEM_SIZE"].Width = nwidth;
            grdMain.Cols["STEEL"].Width = nwidth;
            grdMain.Cols["STEEL_NM"].Width = nwidth;
            grdMain.Cols["LENGTH"].Width = nwidth;
            grdMain.Cols["ORD_WGT"].Width = nwidth;
            grdMain.Cols["SURFACE_LEVEL_NM"].Width = nwidth;

            //grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 100;
            //grdMain.Cols["STEEL"].Width = 0;
            //grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_L_Width + 100;
            //grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width + 100;
            //grdMain.Cols["ORD_WGT"].Width = cs.Wgt_Width + 60;
            //grdMain.Cols["SURFACE_LEVEL_NM"].Width = cs.Surface_Level_NM_Width + 100;


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["ORD_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["SURFACE_LEVEL_NM"].TextAlign = cs.SURFACE_LEVEL_TextAlign;
        }

        private void InitGrd_grdSub()
        {
            C1FlexGrid grd = grdSub as C1FlexGrid;

            cs.InitGrid_search(grd);

            var crCellRange = grd.GetCellRange(0, grd.Cols["SEL"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grd.Styles["ModifyStyle"];

            var crCellRange1 = grd.GetCellRange(0, grd.Cols["INPUT_QTY"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange1.Style = grd.Styles["ModifyStyle"];

            //grd.AllowEditing = true;
            grd.Cols["SEL"].AllowEditing = true;

            grd.Cols["L_NO"].Width = cs.L_No_Width;
            grd.Cols["SEL"].Width = cs.Sel_Width+20;
            grd.Cols["POC_NO"].Width = cs.POC_NO_Width;
            grd.Cols["HEAT"].Width = cs.HEAT_Width;
            grd.Cols["STEEL"].Width = cs.STEEL_Width;
            grd.Cols["STEEL_NM"].Width = cs.STEEL_NM_L_Width;
            grd.Cols["ITEM"].Width = cs.ITEM_Width;
            grd.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grd.Cols["LENGTH"].Width = cs.LENGTH_Width;

            grd.Cols["BUNDLE_QTY"].Width = cs.BUNDLE_QTY_Width;
            grd.Cols["ORD_WGT"].Width = cs.Wgt_Width;

            grd.Cols["INPUT_QTY"].Width = cs.BUNDLE_QTY_Width +20;
            grd.Cols["INPUT_WGT"].Width = cs.Wgt_Width;

            grd.Cols["SURFACE_LEVEL_NM"].Width = cs.Surface_Level_NM_Width;
            grd.Cols["MILL_DATE"].Width = cs.Date_8_Width;
            grd.Cols["COMPANY_NM"].Width = cs.Company_NM_Width;
            grd.Cols["USAGE_CD_NM"].Width = cs.Usage_CD_NM_Width;

            grd.Cols["UNIT_WGT"].Width = 0;
            //grd.Cols["END_WGT"].Width = cs.Wgt_Width;

            //grd.Cols["POC_PROG_STAT_NM"].Width = cs.POC_PROG_STAT_NM_Width;

            //grd.Cols["ITEM"].Width = 0;
            //grd.Cols["POC_PROG_STAT"].Width = 0;
            //grd.Cols["LINE_GP"].Width = 0;
            //grd.Cols["WORK_ORD_DATE"].Width = 0;
            //grd.Cols["WORK_RANK"].Width = 0;
            //grd.Cols["WORK_UOM"].Width = 0;

            grd.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grd.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grd.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grd.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grd.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grd.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grd.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grd.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;

            grd.Cols["BUNDLE_QTY"].TextAlign = cs.BUNDLE_QTY_TextAlign;
            grd.Cols["ORD_WGT"].TextAlign = cs.WGT_TextAlign;

            grd.Cols["INPUT_QTY"].TextAlign = cs.BUNDLE_QTY_TextAlign;
            grd.Cols["INPUT_WGT"].TextAlign = cs.WGT_TextAlign;

            grd.Cols["SURFACE_LEVEL_NM"].TextAlign = cs.SURFACE_LEVEL_TextAlign;
            grd.Cols["MILL_DATE"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["COMPANY_NM"].TextAlign = cs.COMPANY_NM_TextAlign;
            grd.Cols["USAGE_CD_NM"].TextAlign = cs.USAGE_CD_NM_TextAlign;

            grd.Cols["UNIT_WGT"].TextAlign = cs.USAGE_CD_NM_TextAlign;
            //grd.Cols["END_WGT"].TextAlign = cs.WGT_TextAlign;
            //grd.Cols["POC_PROG_STAT_NM"].TextAlign = cs.POC_PROG_STAT_NM_TextAlign;

            //grd.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            //grd.Cols["POC_PROG_STAT"].TextAlign = cs.POC_PROG_STAT_TextAlign;
            //grd.Cols["LINE_GP"].TextAlign = cs.LINE_GP_TextAlign;
            //grd.Cols["WORK_ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            //grd.Cols["WORK_RANK"].TextAlign = cs.WORK_RANK_TextAlign;

            //Button btn1 = new Button();

            //btn1.BackColor = Color.Transparent;
            //btn1.Text = "선택";
            //btn1.Tag = "선택";
            //btn1.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //btn1.ForeColor = Color.Blue;

            //btn1.Click += Button_Click;

            //_al.Add(new HostedControl(grdSub, btn1, 0, grd.Cols["SEL"].Index));

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSel.Click += Button_Click;

            _al.Add(new HostedControl(grdSub, lbSel, 0, grd.Cols["SEL"].Index));
        }

        bool allChecked = false;
        /// <summary>
        /// 선택 column 선택시 모두 선택, 해제 시키게 하는 함수.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, EventArgs e)
        {
            //Button bt = (Button)sender;
            //MessageBox.Show("Clicked on row: " + bt.Tag);

            if (allChecked)
            {
                for (int rowCnt = 1; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, "SEL", false);
                    SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = false;
                
            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, "SEL", true);
                    SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = true;
                
            }

            // 모두 체크 혹은 체크해제 되지만 
            // 체크된 행의 지시번들이 계산되어지지않음.
            // 체크된 행의 지시번들이 계산되어지거나 해제되거나 해주어야함.
            //grdSub_CellChecked(grdSub, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, selectedGrd);
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {

            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            InitGridData(true);

            SetDataBinding();

            SelectedGrdMain(grdMain, grdMain.RowSel);
        }

        private void SetDataBinding()
        {

            string _item_size = (string.IsNullOrEmpty(uC_ITEM_SIZE1.ITEM_SIZE)) ? "" : uC_ITEM_SIZE1.ITEM_SIZE;
            string _steel = (string.IsNullOrEmpty(uC_STEEL1.Steel)) ? "" : uC_STEEL1.Steel;
            string _length = (string.IsNullOrEmpty(uC_LENGTH1.Length)) ? "" : uC_LENGTH1.Length;


            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,ORD_WGT ");
                sql1 += string.Format("       ,SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = X.SURFACE_LEVEL) AS SURFACE_LEVEL_NM ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("         SELECT  ITEM_SIZE ");
                sql1 += string.Format("                ,STEEL ");
                sql1 += string.Format("                ,LENGTH ");
                sql1 += string.Format("                ,SUM(ORD_WGT - NVL(INPUT_ORD_WGT,0)) AS ORD_WGT ");
                sql1 += string.Format("                ,MAX(SURFACE_LEVEL) AS SURFACE_LEVEL ");
                sql1 += string.Format("         FROM   TB_CR_ORD ");
                sql1 += string.Format("         WHERE  ITEM_SIZE LIKE :P_ITEM_SIZE || '%' ");
                sql1 += string.Format("         AND    STEEL     LIKE :P_STEEL || '%' ");
                sql1 += string.Format("         AND    (LENGTH   = :P_LENGTH OR :P_LENGTH IS NULL ) ");
                sql1 += string.Format("         AND    HEAT      LIKE '%' || :P_HEAT || '%' ");
                sql1 += string.Format("         AND    (BUNDLE_QTY - NVL(INPUT_ORD_BUNDLE_QTY,0)) > 0 ");
                sql1 += string.Format("         AND    POC_PROG_STAT <> 'F'   ");// --종결이 아닌것.
                sql1 += string.Format("         GROUP BY ITEM_SIZE, STEEL, LENGTH ");
                sql1 += string.Format("         ORDER BY 1,2,3 ");
                sql1 += string.Format("         ) X ");

                string[] parm = new string[4];
                parm[0] = ":P_ITEM_SIZE|" + _item_size;
                parm[1] = ":P_STEEL|" + _steel;
                parm[2] = ":P_LENGTH|" + _length;
                parm[3] = ":P_HEAT|" + uC_HEAT_s1.HEAT;

                olddt = cd.FindDataTable(sql1, parm);

                moddt = olddt.Copy();
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);

                //SelectedGrdMain(grdMain, grdMain.RowSel);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("[" + ex.ToString() + "]");
                return ;
            }
            
            return ;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 필수입력사항 체크

            bool isChecked = false;
            string show_msg = string.Empty;
            string check_NM = string.Empty;

            if (string.IsNullOrEmpty(uC_LINE_GP1.Line_GP))
            {
                check_NM = "라인";
                show_msg = string.Format("{0}(을)를 입력하세요.", check_NM);
                MessageBox.Show(show_msg);
                return;
            }

            if (string.IsNullOrEmpty(uC_DATE1.Date.ToString()))
            {
                check_NM = "투입지시일자";
                show_msg = string.Format("{0}(을)를 입력하세요.", check_NM);
                MessageBox.Show(show_msg);
                return;
            }

            //if (string.IsNullOrEmpty(uC_WORK_RANK1.WORK_RANK))
            //{
            //    check_NM = "순위";
            //    show_msg = string.Format("{0}(을)를 입력하세요.", check_NM);
            //    MessageBox.Show(show_msg);
            //    return;
            //}

            for (int row = 1; row < grdSub.Rows.Count; row++)
            {

                if (grdSub.GetData(row, "SEL").ToString() == "False")
                {
                    continue;
                }
                isChecked = true;

            }

                if (isChecked)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }
            else
            {
                return;
            }
            #endregion 필수입력사항 체크

            #region SP 구동
            string ling_gp = "";
            string poc_No = "";
            string work_rank = "";
            string work_ord_date = "";
            //string work_uom = "";
            string ord_bundle_No = "";
            string user_id = ck.UserID;
            int proc_stat = 4000;
            string result_stat = "";
            string result_msg = "";

            string strLog = string.Empty;
            var itemList = new List<DictionaryList>();
            var logList = new List<LogDataList>();


            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;
            //string spName = "SP_CR_INPUT_ORD_UPD";  // 프로시저명
            string spName = "SP_CR_INPUT_ORD_CRE";  // 프로시저명
            try
            {
                oConn.Open();
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                transaction = oConn.BeginTransaction();
                //cmd.Transaction = transaction;

                OracleParameter op;

                for (int row = 1; row < grdSub.Rows.Count; row++)
                {

                    if (grdSub.GetData(row, "SEL").ToString() == "False")
                    {
                        continue;
                    }

                    strLog = string.Empty;
                    itemList = new List<DictionaryList>();
                    //op = null;
                    cmd.Parameters.Clear();

                    ling_gp = uC_LINE_GP1.Line_GP;//grdSub.GetData(row, "LINE_GP").ToString();
                    poc_No = grdSub.GetData(row, "POC_NO").ToString();
                    //work_rank = uC_WORK_RANK1.WORK_RANK;//grdSub.GetData(row, "WORK_RANK").ToString();

                    // 순위 자동부여하도록 데이터를 가져옴
                    work_rank = GetWorkRank(ling_gp, uC_DATE1.Date);

                    work_ord_date = vf.Format(uC_DATE1.Date, "yyyyMMdd");//grdSub.GetData(row, "WORK_ORD_DATE").ToString();
                    //work_uom = " "; //grdSub.GetData(row, "WORK_UOM").ToString();
                    ord_bundle_No = grdSub.GetData(row, "INPUT_QTY").ToString();
                    user_id = ck.UserID;
                    proc_stat = 4000;

                    cmd.Transaction = transaction;

                    itemList.Add(new DictionaryList("SP_NAME", spName));

                    op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = poc_No;
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_POC_NO", poc_No));

                    op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ling_gp;
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_LINE_GP", ling_gp));


                    op = new OracleParameter("P_WORK_RANK", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = work_rank;
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_WORK_RANK", work_rank));

                    op = new OracleParameter("P_WORK_ORD_DATE", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = work_ord_date;// vf.Format(work_ord_date, "yyyyMMdd");//  work_ord_date.ToString("yyyyMMdd");
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_WORK_ORD_DATE", work_ord_date));

                    op = new OracleParameter("P_ORD_BD_QTY", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ord_bundle_No;
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_ORD_BD_QTY", ord_bundle_No));

                    op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ck.UserID;    // 사용자 id 가져와서 입력
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_USER_ID", ck.UserID));

                    op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = proc_stat;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = 4000;
                    cmd.Parameters.Add(op);

                    cmd.ExecuteNonQuery();

                    result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                    result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                    LogStrCreate(itemList, ref strLog);

                    if (result_stat == "ERR")
                    {
                        MessageBox.Show(result_msg);
                        logList.Add(new LogDataList(Alarms.Error, Text, strLog));
                        break;
                    }
                    else if (result_stat == "OK")
                    {
                        logList.Add(new LogDataList(Alarms.InSerted, Text, strLog));
                        
                    }

                }

                transaction.Commit();


                if (result_stat == "ERR")
                {
                    foreach (var item in logList)
                    {
                        if (item.Action == Alarms.Error)
                        {
                            clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), item.Contents);
                        }
                    }
                }
                else if (result_stat == "OK")
                {
                    //log make
                    btnDisplay_Click(null, null);

                    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);

                    foreach (var log in logList)
                    {
                        clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                    }
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
                //MessageBox.Show("저장에 실패하였습니다.");
                //return;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();

            }

            return;
            #endregion
        }

        public static void LogStrCreate(List<DictionaryList> _itemList, ref string _strLog)
        {

            foreach (var item in _itemList)
            {
                _strLog += string.Format(" {0}: {1},",item.fnText, item.fnValue);
            }
        }

        private string GetWorkRank(string _line_gp, DateTime _work_ord_date)
        {
            string _work_rank = string.Empty;

            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ");
                sql1 += string.Format("      NVL(X.WORK_RANK , 1) AS WORK_RANK ");
                sql1 += string.Format("FROM ");
                sql1 += string.Format("( ");
                sql1 += string.Format("SELECT ");
                sql1 += string.Format("      NVL(MAX(WORK_RANK), 0) + 1 AS WORK_RANK ");
                //sql1 += string.Format("FROM TB_CR_ORD ");
                sql1 += string.Format("FROM TB_CR_INPUT_ORD ");
                sql1 += string.Format("WHERE LINE_GP = '{0}' ",_line_gp);
                sql1 += string.Format("AND   WORK_ORD_DATE = '{0}' ", vf.Format(_work_ord_date, "yyyyMMdd"));
                sql1 += string.Format(") X ");

                temp_dt = cd.FindDataTable(sql1);

                if (temp_dt.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                    _work_rank = temp_dt.Rows[0]["WORK_RANK"].ToString();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                else
                {
                    _work_rank = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return _work_rank;
            }
            return _work_rank;
                
        }
        private void grdSub_Click(object sender, EventArgs e)
        {
            
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            C1FlexGrid _selectedGrd = sender as C1FlexGrid;

            selectedGrd = _selectedGrd;

            SelectedGrdMain(selectedGrd, selectedGrd.RowSel);

            //if (grdMain.RowSel == 0)
            //{

            //}
        }

        private void SelectedGrdMain(C1FlexGrid grd, int selected_Row)
        {
            if (grd.RowSel <= 0 || grd.Rows.Count <= 1)
            {
                return;
            }


            // subgrd data search
            //string _line_gp = grd.GetData(selected_Row, "LINE_GP").ToString();
            //string _work_date = vf.Format(grd.GetData(selected_Row, "WORK_ORD_DATE"), "yyyyMMdd");
            //string _work_rank = grd.GetData(selected_Row, "WORK_RANK").ToString();
            string _item_size = grd.GetData(selected_Row, "ITEM_SIZE").ToString();
            string _steel = grd.GetData(selected_Row, "STEEL").ToString();
            string _length = grd.GetData(selected_Row, "LENGTH").ToString();
            //string 

            InitGridData(false);

            SetDataBindingGrdSub( _item_size, _steel, _length);
        }

        private void SetDataBindingGrdSub(string _item_size, string _steel, string _length)
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,'False' AS SEL ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("SELECT  POC_NO ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,(BUNDLE_QTY - NVL(INPUT_ORD_BUNDLE_QTY, 0)) AS BUNDLE_QTY ");
                sql1 += string.Format("       ,(ORD_WGT - NVL(INPUT_ORD_WGT,0)) AS ORD_WGT ");
                sql1 += string.Format("       ,0  AS INPUT_QTY ");
                sql1 += string.Format("       ,0  AS INPUT_WGT ");
                sql1 += string.Format("       ,SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = A.SURFACE_LEVEL) AS SURFACE_LEVEL_NM ");
                sql1 += string.Format("       ,TO_DATE(MILL_DATE) AS MILL_DATE ");
                sql1 += string.Format("       ,COMPANY_NM ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'USAGE_CD' AND CD_ID = A.USAGE_CD) AS USAGE_CD_NM ");
                sql1 += string.Format("       ,ROUND(ORD_WGT / BUNDLE_QTY, 1) AS UNIT_WGT ");
                sql1 += string.Format("FROM   TB_CR_ORD A ");
                sql1 += string.Format("WHERE  ITEM_SIZE = :P_ITEM_SIZE ");
                sql1 += string.Format("AND    STEEL     = :P_STEEL ");
                sql1 += string.Format("AND    LENGTH    = :P_LENGTH ");
                sql1 += string.Format("AND    POC_PROG_STAT <> 'F'   ");// --종결이 아닌것.
                sql1 += string.Format("AND    (BUNDLE_QTY - NVL(INPUT_ORD_BUNDLE_QTY,0)) > 0 ");
                sql1 += string.Format("ORDER BY 1,2,3,4 ");
                sql1 += string.Format(") X ");

                string[] parm = new string[3];
                parm[0] = ":P_ITEM_SIZE|" + _item_size;
                parm[1] = ":P_STEEL|" + _steel;
                parm[2] = ":P_LENGTH|" + _length;

                olddt_sub = cd.FindDataTable(sql1, parm);

                moddt_sub = olddt_sub.Copy();
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdSub.SetDataBinding(moddt_sub, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt_sub.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        private void grdSub_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (e.Col != grd.Cols["SEL"].Index && e.Col != grd.Cols["INPUT_QTY"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
            }


            if (e.Col == grd.Cols["INPUT_QTY"].Index)
            {
                if (grd.GetData(e.Row, "SEL").ToString() != "True")
                {
                    e.Cancel = true;
                }
                
            }
            



        }

        private void grdSub_CellChecked(object sender, RowColEventArgs e)
        {
  
            SetupOrdBundleNo(sender, e.Row);

        }

        private void SetupOrdBundleNo(object sender, int checkedRow)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            #region 선택된 행의 지시번들 데이터 초기화
            if (grd.GetData(checkedRow, "SEL").ToString() == "True")
                grd.SetData(checkedRow, "INPUT_QTY", grd.GetData(checkedRow, "BUNDLE_QTY").ToString());
            else
                grd.SetData(checkedRow, "INPUT_QTY", "0");
            #endregion
        }

        private void CalOrdWgt()
        {
            C1FlexGrid grd = grdSub as C1FlexGrid;

            #region 지시중량합계 계산
            double sumOfWGT = 0;

            double wgt = 0;

            //선택된 rows의 중량 합을 표시.
            for (int row = 1; row < grd.Rows.Count; row++)
            {
                wgt = 0;
                if (grd.GetData(row, "SEL").ToString() == "True")
                {
                    //wgt = vf.CInt2(grd.GetData(row, "ORD_WGT").ToString());
                    wgt = vf.CDbl(grd.GetData(row, "INPUT_WGT").ToString());
                    sumOfWGT = sumOfWGT + wgt;
                }
            }

            uC_WGT1.WGT = vf.Format(sumOfWGT, "#,###,##0");
            #endregion
        }


        ArrayList _al = new ArrayList();

        /// <summary>
        /// HostedControl
        /// helper class that contains a control hosted within a C1FlexGrid
        /// </summary>
        internal class HostedControl
        {
            internal C1FlexGrid _flex;
            internal Control _ctl;
            internal Row _row;
            internal Column _col;

            internal HostedControl(C1FlexGrid flex, Control hosted, int row, int col)
            {
                // save info
                _flex = flex;
                _ctl = hosted;
                _row = flex.Rows[row];
                _col = flex.Cols[col];


                // insert hosted control into grid
                _flex.Controls.Add(_ctl);
            }

            internal bool UpdatePosition()
            {
                // get row/col indices
                int r = _row.Index;
                int c = _col.Index;
                if (r < 0 || c < 0) return false;

                // get cell rect
                Rectangle rc = _flex.GetCellRect(r, c, false);

                // hide control if out of range
                if (rc.Width <= 0 || rc.Height <= 0 || !rc.IntersectsWith(_flex.ClientRectangle))
                {
                    _ctl.Visible = false;
                    return true;
                }

                // move the control and show it
                _ctl.Bounds = rc;
                _ctl.Visible = true;

                // done
                return true;
            }
        }

        private void grdSub_Paint(object sender, PaintEventArgs e)
        {
            foreach (HostedControl hosted in _al)
                hosted.UpdatePosition();
        }

        private void grdSub_KeyDownEdit(object sender, KeyEditEventArgs e)
        {
            if (e.Col == grdSub.Cols["INPUT_QTY"].Index)
            {
                int pKey = e.KeyValue;

                //엔터 눌렀을 시, //  Tab 눌렀을때.
                if (pKey == 13 || pKey == 9)
                {
                    SendKeys.Send("{TAB}");
                    //item_size_tb.Text = vf.Format(item_size_tb.Text, "0");
                    //SetDataBinding();  // 조회 버튼을 통한 데이터입력
                }
            }
        }

        private void grdSub_KeyPressEdit(object sender, KeyPressEditEventArgs e)
        {
            if (e.Col == grdSub.Cols["INPUT_QTY"].Index)
            {
                vf.KeyPressEvent(sender, e);
            }
        }

        private void grdSub_RowColChange(object sender, EventArgs e)
        {
            
        }

        private void grdSub_CellChanged(object sender, RowColEventArgs e)
        {
            double WgtCaled = 0;

            if (e.Col == grdSub.Cols["INPUT_QTY"].Index)
            {
                if (vf.CDbl(grdSub.GetData(e.Row, "INPUT_QTY").ToString()) > vf.CDbl(grdSub.GetData(e.Row, "BUNDLE_QTY").ToString()))
                {
                    grdSub.SetData(e.Row, "INPUT_QTY", grdSub.GetData(e.Row, "BUNDLE_QTY").ToString());
                }

                WgtCaled = vf.CDbl(grdSub.GetData(e.Row, "INPUT_QTY").ToString()) * vf.CDbl(grdSub.GetData(e.Row, "UNIT_WGT").ToString());
                // 입력된 지시 번들수 에 따른 지시중량값을 계산해 입력한다.
                grdSub.SetData(e.Row, "INPUT_WGT", WgtCaled);
            }

            if (e.Col == grdSub.Cols["INPUT_WGT"].Index)
            {
                CalOrdWgt();
            }
        }

        private void btnPOCFin_Click(object sender, EventArgs e)
        {
            
            if (grdSub.Row < 1)
            {
                return;
            }

            string _spName = "SP_CR_POC_CLOSE";
            string _poc_No = grdSub.GetData(grdSub.Row, "POC_NO").ToString();
            string _RegOrCancel = "REG";
            string _user_id = ck.UserID;

            string msg = string.Format(" POC : {0} \n POC종결 하시겠습니까?", _poc_No);

            if (MessageBox.Show(msg, Text, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            else
            {
                ValidResp rsp = PocFinOrCancel(_spName, _poc_No, _RegOrCancel, _user_id);

                if (rsp.Successful)
                {
                    btnDisplay_Click(null, null);

                    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), rsp.Information);
                }
                
            }
            

        }

        private ValidResp PocFinOrCancel(string _spName, string _poc_No, string RegOrCancel, string _user_id )
        {
            ValidResp rsp = new ValidResp();
            rsp.Successful = false;
            rsp.Information = string.Empty;

            OracleConnection conn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            try
            {

                cmd.Connection = conn;
                cmd.CommandText = _spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;

                op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = _poc_No;    // poc
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = RegOrCancel;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = _user_id;    // 사용자 id 가져와서 입력
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);


                conn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.ExecuteOracleScalar();

                string result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                string result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                if (result_stat == "ERR")
                {
                    rsp.Successful = false;
                    rsp.Information = result_msg;

                    string error_msg = "POC_NO:" + _poc_No
                                 + " " + "User_id:" + _user_id
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Error, "POC종결", clsMsg.Log.__Line(), error_msg);
                    MessageBox.Show(result_msg);
                    
                }
                else
                {
                    rsp.Successful = true;
                    rsp.Information = result_msg;
                    string success_msg = "POC_NO:" + _poc_No
                                 + " " + "User_id:" + _user_id
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Modified, "POC종결", clsMsg.Log.__Line(), success_msg);
                    //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
                }


                transaction.Commit();

                //MessageBox.Show(result_msg);
                rsp.Successful= true;
                rsp.Information = result_msg;
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
                //rsp.Information = ex.Message;
                //return rsp;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
                //Close();
            }
            return rsp;
        }

        private void grdSub_DoubleClick(object sender, EventArgs e)
        {
            if (grdSub.Row < 1)
            {
                return;
            }

            //선택된 행의 POC  데이터를 전달해서 해당정보를 조회하는 창 OPEN
            string poc_No = grdSub.GetData(grdSub.Row, "POC_NO").ToString().Trim();
            //string poc_seq_No = grdSub.GetData(grdSub.Row, "POC_SEQ").ToString().Trim();

            CrtInBundleInfo popup = new CrtInBundleInfo(poc_No, "");
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.MinimizeBox = false;
            popup.MaximizeBox = false;
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
        }
    }
}
