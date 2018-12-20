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
using System.Windows.Forms;


namespace SystemControlClassLibrary.monitoring
{
    public partial class WorkRsltCancel : Form
    {
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();


        DataTable olddt;
        DataTable moddt;
        DataTable logdt;



        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }


        OracleTransaction transaction = null;

        C1FlexGrid selectedGrd = null;

        private string line_gp = string.Empty;
        private string poc_no = string.Empty;
        private DataTable grdMain_dt;
        private DataTable grdSub_dt;
        private DataTable olddt_sub;
        private DataTable moddt_sub;

        bool allChecked = false;
        ArrayList _al = new ArrayList();

        public WorkRsltCancel(string _line_gp, string _poc_NO)
        {
            line_gp = _line_gp;
            poc_no = _poc_NO;

            InitializeComponent();

            uC_RsltCancel1.GubunChangeEvent += uC_RsltCancel1_ChangeEvent;


            selectedGrd = grdMain;
        }

        private void uC_RsltCancel1_ChangeEvent(object sender, EventArgs e)
        {

            //arrType1.Add(new DictionaryList("최소대상", "A"));
            //arrType1.Add(new DictionaryList("최소결과", "B"));

            if (uC_RsltCancel1.GUBUN == "A")
            {
                btnCancel.Enabled = true;// 최소대상

            }
            else
            {
                btnCancel.Enabled = false;// 최소결과
            }

            if (grdMain_dt != null)
            {
                btnDisplay_Click(null, null);
            }
        }

        private void WorkRsltCancel_Load(object sender, EventArgs e)
        {
            InitControl();
            
            MakeInitgrdData();

            
            //초기 설정
            uC_Line_gp_s1.Line_GP = line_gp;
            uC_POC_sc1.POC = poc_no;

            uC_Work_Date_Fr_To_s1.Work_From_Date = DateTime.Now.Date;
            uC_Work_Date_Fr_To_s1.Work_To_Date = DateTime.Now.Date;

            uC_RsltCancel1_ChangeEvent(null, null);

            //초기 조회
            btnDisplay_Click(null, null);
        }

        private void MakeInitgrdData()
        {
            grdMain_dt = vf.CreateDataTable(grdMain);

            grdSub_dt = vf.CreateDataTable(grdSub);
        }

        private void InitControl()
        {
            clsStyle.Style.InitButton(btnDisplay);

            clsStyle.Style.InitButton(btnClose);


            clsStyle.Style.InitButton(btnCancel);

            

            InitGrd_Main();
            InitGrd_Sub();
        }

        private void InitGrd_Main()
        {
            cs.InitGrid_search(grdMain);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["SEL"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.Cols["L_NO"       ].Width = cs.L_No_Width;
            grdMain.Cols["SEL"        ].Width = cs.Sel_Width;
            grdMain.Cols["MILL_NO"    ].Width = cs.Mill_No_Width;
            grdMain.Cols["MFG_DATE"   ].Width = cs.Date_8_Width;
            grdMain.Cols["POC_NO"     ].Width = cs.POC_NO_Width;
            grdMain.Cols["POC_SEQ"    ].Width = cs.POC_SEQ_Width;
            grdMain.Cols["HEAT"       ].Width = cs.HEAT_Width;
            grdMain.Cols["STEEL"      ].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"   ].Width = cs.STEEL_NM_Width;
            grdMain.Cols["ITEM"       ].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"  ].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["LENGTH"     ].Width = cs.LENGTH_Width;
            grdMain.Cols["MILL_PCS"   ].Width = cs.PCS_L_Width;
            grdMain.Cols["MILL_WGT"   ].Width = cs.Wgt_Width;
            grdMain.Cols["STR_PCS"    ].Width = cs.PCS_L_Width;
            grdMain.Cols["NDT_PCS"    ].Width = cs.PCS_L_Width;
            grdMain.Cols["BND_PCS"    ].Width = cs.PCS_L_Width;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"       ].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["SEL"        ].TextAlign = cs.SEL_TextAlign;
            grdMain.Cols["MILL_NO"    ].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["MFG_DATE"   ].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["POC_NO"     ].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["POC_SEQ"    ].TextAlign = cs.POC_SEQ_TextAlign;
            grdMain.Cols["HEAT"       ].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"      ].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"   ].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"       ].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"  ].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"     ].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["MILL_PCS"   ].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MILL_WGT"   ].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["STR_PCS"    ].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["NDT_PCS"    ].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["BND_PCS"    ].TextAlign = cs.PCS_TextAlign;
            //grdMain.Cols["MEAS_NO"].TextAlign = 0;
            //grdMain.Cols["LINE_GP"].TextAlign = 0;

            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"       ].AllowEditing = false;
            //grdMain.Cols["SEL"        ].AllowEditing = cs.SEL_TextAlign;
            grdMain.Cols["MILL_NO"    ].AllowEditing = false;
            grdMain.Cols["MFG_DATE"   ].AllowEditing = false;
            grdMain.Cols["POC_NO"     ].AllowEditing = false;
            grdMain.Cols["POC_SEQ"    ].AllowEditing = false;
            grdMain.Cols["HEAT"       ].AllowEditing = false;
            grdMain.Cols["STEEL"      ].AllowEditing = false;
            grdMain.Cols["STEEL_NM"   ].AllowEditing = false;
            grdMain.Cols["ITEM"       ].AllowEditing = false;
            grdMain.Cols["ITEM_SIZE"  ].AllowEditing = false;
            grdMain.Cols["LENGTH"     ].AllowEditing = false;
            grdMain.Cols["MILL_PCS"   ].AllowEditing = false;
            grdMain.Cols["MILL_WGT"   ].AllowEditing = false;
            grdMain.Cols["STR_PCS"    ].AllowEditing = false;
            grdMain.Cols["NDT_PCS"    ].AllowEditing = false;
            grdMain.Cols["BND_PCS"    ].AllowEditing = false;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain, lbSel, 0, grdMain.Cols["SEL"].Index));
        }

        ///// <summary>
        ///// HostedControl
        ///// helper class that contains a control hosted within a C1FlexGrid
        ///// </summary>
        //internal class HostedControl
        //{
        //    internal C1FlexGrid _flex;
        //    internal Control _ctl;
        //    internal Row _row;
        //    internal Column _col;

        //    internal HostedControl(C1FlexGrid flex, Control hosted, int row, int col)
        //    {
        //        // save info
        //        _flex = flex;
        //        _ctl = hosted;
        //        _row = flex.Rows[row];
        //        _col = flex.Cols[col];


        //        // insert hosted control into grid
        //        _flex.Controls.Add(_ctl);
        //    }

        //    internal bool UpdatePosition()
        //    {
        //        // get row/col indices
        //        int r = _row.Index;
        //        int c = _col.Index;
        //        if (r < 0 || c < 0) return false;

        //        // get cell rect
        //        Rectangle rc = _flex.GetCellRect(r, c, false);

        //        // hide control if out of range
        //        if (rc.Width <= 0 || rc.Height <= 0 || !rc.IntersectsWith(_flex.ClientRectangle))
        //        {
        //            _ctl.Visible = false;
        //            return true;
        //        }

        //        // move the control and show it
        //        _ctl.Bounds = rc;
        //        _ctl.Visible = true;

        //        // done
        //        return true;
        //    }
        //}

        private void SEL_Click(object sender, EventArgs e)
        {
            if (allChecked)
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "SEL", false);
                    //SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "SEL", true);
                    //SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = true;

            }
        }

        private void InitGrd_Sub()
        {
            cs.InitGrid_search(grdSub);

            grdSub.AllowEditing = false;


            grdSub.Cols["L_NO"       ].Width = cs.L_No_Width;
            grdSub.Cols["MILL_NO"    ].Width = cs.Mill_No_Width;
            grdSub.Cols["PIECE_NO"   ].Width = cs.PIECE_NO_Width;
            grdSub.Cols["POC_NO"     ].Width = cs.POC_NO_Width;
            //grdSub.Cols["POC_SEQ"    ].Width = cs.POC_SEQ_Width ;
            grdSub.Cols["HEAT"       ].Width = cs.HEAT_Width;
            grdSub.Cols["STEEL"      ].Width = cs.STEEL_Width;
            grdSub.Cols["STEEL_NM"   ].Width = cs.STEEL_NM_Width;
            grdSub.Cols["ITEM"       ].Width = cs.ITEM_Width;
            grdSub.Cols["ITEM_SIZE"  ].Width = cs.ITEM_SIZE_Width;
            grdSub.Cols["LENGTH"     ].Width = cs.LENGTH_Width;
            grdSub.Cols["MFG_DATE"   ].Width = cs.Date_8_Width;
            grdSub.Cols["BUNDLE_NO"  ].Width = cs.BUNDLE_NO_Width;

            grdSub.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdSub.Cols["L_NO"       ].TextAlign = cs.L_NO_TextAlign;
            grdSub.Cols["MILL_NO"    ].TextAlign = cs.MILL_NO_TextAlign;
            grdSub.Cols["PIECE_NO"   ].TextAlign = cs.PIECE_NO_TextAlign;
            grdSub.Cols["POC_NO"     ].TextAlign = cs.POC_NO_TextAlign;
            //grdSub.Cols["POC_SEQ"    ].TextAlign = cs.POC_SEQ_TextAlign;
            grdSub.Cols["HEAT"       ].TextAlign = cs.HEAT_TextAlign;
            grdSub.Cols["STEEL"      ].TextAlign = cs.STEEL_TextAlign;
            grdSub.Cols["STEEL_NM"   ].TextAlign = cs.STEEL_NM_TextAlign;
            grdSub.Cols["ITEM"       ].TextAlign = cs.ITEM_TextAlign;
            grdSub.Cols["ITEM_SIZE"  ].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdSub.Cols["LENGTH"     ].TextAlign = cs.LENGTH_TextAlign;
            grdSub.Cols["MFG_DATE"   ].TextAlign = cs.DATE_TextAlign;
            grdSub.Cols["BUNDLE_NO"  ].TextAlign = cs.BUNDLE_NO_TextAlign;
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            InitGridData(true);

            SetDataBinding();

            SelectedGrdMain(grdMain, grdMain.RowSel);
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
        }

        private void SetDataBinding()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT  ROWNUM AS L_NO ");
                sql1 += string.Format("       ,'False' AS SEL ");
                sql1 += string.Format("       ,MILL_NO ");
                sql1 += string.Format("       ,TO_DATE(MFG_DATE,'YYYYMMDD') AS MFG_DATE ");
                sql1 += string.Format("       ,POC_NO ");
                sql1 += string.Format("       ,POC_SEQ ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,MILL_PCS ");
                sql1 += string.Format("       ,MILL_WGT ");
                sql1 += string.Format("       ,STR_PCS ");
                sql1 += string.Format("       ,NDT_PCS ");
                sql1 += string.Format("       ,BND_PCS ");
                sql1 += string.Format("FROM ( ");
                sql1 += string.Format("    SELECT  A.MILL_NO ");
                sql1 += string.Format("           ,A.MFG_DATE ");
                sql1 += string.Format("           ,A.POC_NO ");
                sql1 += string.Format("           ,A.POC_SEQ ");
                sql1 += string.Format("           ,A.HEAT ");
                sql1 += string.Format("           ,A.ITEM ");
                sql1 += string.Format("           ,A.ITEM_SIZE ");
                sql1 += string.Format("           ,A.STEEL ");
                sql1 += string.Format("           ,A.LENGTH ");
                sql1 += string.Format("           ,B.PCS     AS MILL_PCS ");
                sql1 += string.Format("           ,B.NET_WGT AS MILL_WGT ");
                sql1 += string.Format("           ,A.STR_PCS ");
                sql1 += string.Format("           ,A.NDT_PCS ");
                sql1 += string.Format("           ,A.BND_PCS ");
                sql1 += string.Format("    FROM   ( ");
                sql1 += string.Format("            SELECT  A.MILL_NO        AS MILL_NO ");
                sql1 += string.Format("                   ,MAX(A.MFG_DATE)  AS MFG_DATE ");
                sql1 += string.Format("                   ,MAX(A.POC_NO)    AS POC_NO ");
                sql1 += string.Format("                   ,MAX(A.POC_SEQ)   AS POC_SEQ ");
                sql1 += string.Format("                   ,MAX(A.HEAT)      AS HEAT ");
                sql1 += string.Format("                   ,MAX(A.ITEM)      AS ITEM ");
                sql1 += string.Format("                   ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
                sql1 += string.Format("                   ,MAX(A.STEEL)     AS STEEL ");
                sql1 += string.Format("                   ,MAX(A.LENGTH)    AS LENGTH ");
                sql1 += string.Format("                   ,SUM(DECODE(A.ROUTING_CD,'A1',1,0))  AS STR_PCS ");
                sql1 += string.Format("                   ,SUM(DECODE(A.ROUTING_CD,'F2',1,0))  AS NDT_PCS ");
                sql1 += string.Format("                   ,SUM(DECODE(A.ROUTING_CD,'P3',1,0))  AS BND_PCS ");
                sql1 += string.Format("            FROM   TB_CR_PIECE_WR A ");
                sql1 += string.Format("            WHERE  A.MFG_DATE   BETWEEN :P_FR_DATE AND :P_TO_DATE ");
                sql1 += string.Format("            AND    A.LINE_GP    = :P_LINE_GP ");
                sql1 += string.Format("            AND    A.ROUTING_CD IN ('A1','F2','P3')   "); //--교정,NDT,결속
                sql1 += string.Format("            AND    A.POC_NO     LIKE :P_POC_NO || '%' ");
                sql1 += string.Format("            AND    A.ITEM_SIZE  LIKE :P_ITEM_SIZE || '%'");
                sql1 += string.Format("            AND    A.HEAT       LIKE '%' || :P_HEAT || '%' ");
                sql1 += string.Format("            AND    :P_GP        = 'A' ");
                sql1 += string.Format("            AND    REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                                  WHERE  MILL_NO  = A.MILL_NO ");
                sql1 += string.Format("                                  AND    PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                                  AND    LINE_GP  = A.LINE_GP ");
                sql1 += string.Format("                                  AND    ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("            GROUP BY A.MILL_NO ");
                sql1 += string.Format("           ) A ");
                sql1 += string.Format("           ,TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("    WHERE  A.MILL_NO  = B.MILL_NO ");
                sql1 += string.Format("    UNION ");
                sql1 += string.Format("    SELECT  A.MILL_NO ");
                sql1 += string.Format("           ,A.MFG_DATE ");
                sql1 += string.Format("           ,A.POC_NO ");
                sql1 += string.Format("           ,A.POC_SEQ ");
                sql1 += string.Format("           ,A.HEAT ");
                sql1 += string.Format("           ,A.ITEM ");
                sql1 += string.Format("           ,A.ITEM_SIZE ");
                sql1 += string.Format("           ,A.STEEL ");
                sql1 += string.Format("           ,A.LENGTH ");
                sql1 += string.Format("           ,B.PCS     AS MILL_PCS ");
                sql1 += string.Format("           ,B.NET_WGT AS MILL_WGT ");
                sql1 += string.Format("           ,A.STR_PCS ");
                sql1 += string.Format("           ,A.NDT_PCS ");
                sql1 += string.Format("           ,A.BND_PCS ");
                sql1 += string.Format("    FROM   ( ");
                sql1 += string.Format("            SELECT  A.MILL_NO        AS MILL_NO ");
                sql1 += string.Format("                   ,MAX(A.MFG_DATE)  AS MFG_DATE ");
                sql1 += string.Format("                   ,MAX(A.POC_NO)    AS POC_NO ");
                sql1 += string.Format("                   ,MAX(A.POC_SEQ)   AS POC_SEQ ");
                sql1 += string.Format("                   ,MAX(A.HEAT)      AS HEAT ");
                sql1 += string.Format("                   ,MAX(A.ITEM)      AS ITEM ");
                sql1 += string.Format("                   ,MAX(A.ITEM_SIZE) AS ITEM_SIZE ");
                sql1 += string.Format("                   ,MAX(A.STEEL)     AS STEEL ");
                sql1 += string.Format("                   ,MAX(A.LENGTH)    AS LENGTH ");
                sql1 += string.Format("                   ,SUM(DECODE(A.ROUTING_CD,'A1',1,0))  AS STR_PCS ");
                sql1 += string.Format("                   ,SUM(DECODE(A.ROUTING_CD,'F2',1,0))  AS NDT_PCS ");
                sql1 += string.Format("                   ,SUM(DECODE(A.ROUTING_CD,'P3',1,0))  AS BND_PCS ");
                sql1 += string.Format("            FROM   TB_CR_PIECE_WR_CNCL A ");
                sql1 += string.Format("            WHERE  A.MFG_DATE   BETWEEN :P_FR_DATE AND :P_TO_DATE ");
                sql1 += string.Format("            AND    A.LINE_GP    = :P_LINE_GP ");
                sql1 += string.Format("            AND    A.ROUTING_CD IN ('A1','F2','P3')   "); //--교정,NDT,결속
                sql1 += string.Format("            AND    A.POC_NO     LIKE :P_POC_NO || '%' ");
                sql1 += string.Format("            AND    A.ITEM_SIZE  LIKE :P_ITEM_SIZE || '%' ");
                sql1 += string.Format("            AND    A.HEAT       LIKE '%' || :P_HEAT || '%' ");
                sql1 += string.Format("            AND    :P_GP        = 'B' ");
                sql1 += string.Format("            AND    REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR_CNCL ");
                sql1 += string.Format("                                  WHERE  MILL_NO  = A.MILL_NO ");
                sql1 += string.Format("                                  AND    PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                                  AND    LINE_GP  = A.LINE_GP ");
                sql1 += string.Format("                                  AND    ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("            GROUP BY A.MILL_NO ");
                sql1 += string.Format("           ) A ");
                sql1 += string.Format("           ,TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("    WHERE  A.MILL_NO  = B.MILL_NO ");
                sql1 += string.Format(") X ");
                sql1 += string.Format("ORDER BY  1,2,3,4,5 ");

                string[] parm = new string[7];
                parm[0] = ":P_LINE_GP|" + uC_Line_gp_s1.Line_GP;
                parm[1] = ":P_FR_DATE|" + vf.Format(uC_Work_Date_Fr_To_s1.Work_From_Date, "yyyyMMdd");
                parm[2] = ":P_TO_DATE|" + vf.Format(uC_Work_Date_Fr_To_s1.Work_To_Date, "yyyyMMdd");
                parm[3] = ":P_POC_NO|" + uC_POC_sc1.POC;
                parm[4] = ":P_HEAT|" + uC_HEAT_s1.HEAT;
                parm[5] = ":P_GP|" + uC_RsltCancel1.GUBUN;
                parm[6] = ":P_ITEM_SIZE|" + uC_Item_size_s1.ITEM_SIZE;



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
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        private void SelectedGrdMain(C1FlexGrid grd, int rowSel)
        {
            if (grd.RowSel <= 0 || grd.Rows.Count <= 1)
            {
                return;
            }


            // subgrd data search
            string _line_gp = uC_Line_gp_s1.Line_GP;
            string _mill_NO = grd.GetData(rowSel, "MILL_NO").ToString();
            string _rsltCancel = uC_RsltCancel1.GUBUN;

            InitGridData(false);

            // 압연번들번호, 라인넘버, 실적취소구분
            SetDataBindingGrdSub(_line_gp, _mill_NO, _rsltCancel);
        }

        private void SetDataBindingGrdSub(string _line_gp, string _mill_NO, string _rsltCancel)
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,MILL_NO ");
                sql1 += string.Format("       ,PIECE_NO ");
                sql1 += string.Format("       ,POC_NO ");

                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,TO_DATE(MFG_DATE,'YYYYMMDD') AS MFG_DATE ");
                sql1 += string.Format("       ,BUNDLE_NO ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT  MILL_NO ");
                sql1 += string.Format("               ,PIECE_NO ");
                sql1 += string.Format("               ,POC_NO ");

                sql1 += string.Format("               ,HEAT ");
                sql1 += string.Format("               ,STEEL ");
                sql1 += string.Format("               ,ITEM ");
                sql1 += string.Format("               ,ITEM_SIZE ");
                sql1 += string.Format("               ,LENGTH ");
                sql1 += string.Format("               ,MFG_DATE ");

                sql1 += string.Format("               ,(SELECT MAX(BUNDLE_NO) FROM TB_CR_PIECE_WR  ");
                sql1 += string.Format("                  WHERE  MILL_NO  = A.MILL_NO  ");
                sql1 += string.Format("                  AND    PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                  AND    LINE_GP  = A.LINE_GP ");
                sql1 += string.Format("                  AND    ROUTING_CD = 'P3' ) AS BUNDLE_NO ");

                sql1 += string.Format("        FROM   TB_CR_PIECE_WR A ");
                sql1 += string.Format("        WHERE  MILL_NO    = :P_MILL_NO ");
                sql1 += string.Format("        AND    LINE_GP    = :P_LINE_GP ");
                sql1 += string.Format("        AND    ROUTING_CD = 'A1' ");
                sql1 += string.Format("        AND    REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                              WHERE  MILL_NO  = A.MILL_NO ");
                sql1 += string.Format("                              AND    PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                              AND    LINE_GP  = A.LINE_GP ");
                sql1 += string.Format("                              AND    ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("        AND    :P_GP      = 'A' ");
                sql1 += string.Format("        UNION ");
                sql1 += string.Format("        SELECT  MILL_NO ");
                sql1 += string.Format("               ,PIECE_NO ");
                sql1 += string.Format("               ,POC_NO ");

                sql1 += string.Format("               ,HEAT ");
                sql1 += string.Format("               ,STEEL ");
                sql1 += string.Format("               ,ITEM ");
                sql1 += string.Format("               ,ITEM_SIZE ");
                sql1 += string.Format("               ,LENGTH ");
                sql1 += string.Format("               ,MFG_DATE ");

                sql1 += string.Format("               ,(SELECT MAX(BUNDLE_NO) FROM TB_CR_PIECE_WR_CNCL  ");
                sql1 += string.Format("                 WHERE  MILL_NO  = A.MILL_NO ");
                sql1 += string.Format("                 AND    PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                 AND    LINE_GP  = A.LINE_GP ");
                sql1 += string.Format("                 AND    ROUTING_CD = 'P3' ) AS BUNDLE_NO ");

                sql1 += string.Format("        FROM   TB_CR_PIECE_WR_CNCL A ");
                sql1 += string.Format("        WHERE  MILL_NO    = :P_MILL_NO ");
                sql1 += string.Format("        AND    LINE_GP    = :P_LINE_GP ");
                sql1 += string.Format("        AND    ROUTING_CD = 'A1' ");
                sql1 += string.Format("        AND    REWORK_SEQ = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR_CNCL ");
                sql1 += string.Format("                              WHERE  MILL_NO  = A.MILL_NO ");
                sql1 += string.Format("                              AND    PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                              AND    LINE_GP  = A.LINE_GP ");
                sql1 += string.Format("                              AND    ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("        AND    :P_GP      = 'B' ");
                sql1 += string.Format("        ORDER BY MILL_NO, PIECE_NO ");
                sql1 += string.Format("    ) X ");


                string[] parm = new string[3];
                parm[0] = ":P_LINE_GP|" + _line_gp;
                parm[1] = ":P_MILL_NO|" + _mill_NO;
                parm[2] = ":P_GP|" + _rsltCancel;

                olddt_sub = cd.FindDataTable(sql1, parm);

                moddt_sub = olddt_sub.Copy();
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdSub.SetDataBinding(moddt_sub, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt_sub.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            C1FlexGrid _selectedGrd = sender as C1FlexGrid;

            if (selectedGrd.RowSel < 1)
            {
                return;
            }

            selectedGrd = _selectedGrd;



            SelectedGrdMain(selectedGrd, selectedGrd.RowSel);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {


            #region 항목체크
            bool isChecked = false;
            for (int row = 1; row < grdMain.Rows.Count; row++)
            {
                isChecked = (grdMain.GetData(row, "SEL").ToString() == "True") ? true : isChecked;

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
            #endregion

            #region TB_WORK_WR_CNCL_TABLE 에 선택된 항목 입력 SP_WORK_WR_CNCL을 실행함 실행이 정상이면 메시지만 뿌리고 창 닫음, 실패하면 팝업으로 알리고 
            var logList = new List<LogDataList>();

            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string sql1 = string.Empty;
            string selected = string.Empty;
            string result_stat = string.Empty;
            string result_msg = string.Empty;

            string spName = "SP_WORK_WR_CNCL";
            OracleParameter op;
            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                //수정된 데이터 인서트 
                #region 수정된 테이블 인서트

                for (int row = 1; row < grdMain.Rows.Count; row++)
                {

                    selected = grdMain.GetData(row, "SEL").ToString();

                    if (selected == "True")
                    {
                       
                        sql1 = string.Empty;
                        sql1 += string.Format("INSERT INTO TB_WORK_WR_CNCL ");
                        sql1 += string.Format("( ");
                        sql1 += string.Format("     REGISTER ");
                        sql1 += string.Format("    ,LINE_GP ");
                        sql1 += string.Format("    ,MILL_NO ");
                        sql1 += string.Format("    ,MFG_DATE ");
                        sql1 += string.Format("    ,POC_NO ");
                        sql1 += string.Format("    ,POC_SEQ ");
                        sql1 += string.Format("    ,PROC_GP ");
                        sql1 += string.Format("    ,REG_DDTT ");
                        sql1 += string.Format(") ");
                        sql1 += string.Format("VALUES ");
                        sql1 += string.Format("( ");
                        sql1 += string.Format("     '{0}' ",  ck.UserID);                                   //REGISTER ");
                        sql1 += string.Format("    ,'{0}' ",  uC_Line_gp_s1.Line_GP);                       //LINE_GP ");
                        sql1 += string.Format("    ,'{0}' ",  grdMain.GetData(row, "MILL_NO").ToString());  //MILL_NO ");
                        sql1 += string.Format("    ,'{0}' ",  vf.Format(grdMain.GetData(row, "MFG_DATE"), "yyyyMMdd")); //MFG_DATE ");
                        sql1 += string.Format("    ,'{0}' ",  grdMain.GetData(row, "POC_NO").ToString());   //POC_NO ");
                        sql1 += string.Format("    ,'{0}' ",  grdMain.GetData(row, "POC_SEQ").ToString());  //POC_SEQ ");
                        sql1 += string.Format("    ,'{0}' ",  "");  //PROC_GP ");
                        sql1 += string.Format("    , SYSDATE ");      //REG_DDTT ");
                        sql1 += string.Format(") ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        logList.Add(new LogDataList(Alarms.InSerted, Text, sql1));
                    }
                }

                #endregion

                #region "SP_WORK_WR_CNCL" sp를 실행시켜 변경된 실적취소 실행하게함.
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = uC_Line_gp_s1.Line_GP;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = ck.UserID;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                cmd.ExecuteNonQuery();

                result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);
                #endregion

                //실행후 성공
                transaction.Commit();
                //SetDataBinding();

                if (result_stat == "ERR")
                {
                    MessageBox.Show(result_msg);
                }
                else if(result_stat == "OK")
                {
                    #region Log Create
                    foreach (var log in logList)
                    {
                        clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                    }

                    string sp_msg = string.Format("SP_NAME: {0}, LINE_GP: {1}, USER_ID: {2}", spName, line_gp, ck.UserID);

                    clsMsg.Log.Alarm(Alarms.InSerted, Text, clsMsg.Log.__Line(), sp_msg);
                    #endregion

                    btnDisplay_Click(null, null);
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), result_msg);
                }

                // 성공시에 추가 수정 삭제 상황을 초기화시킴
                //InitGrd_Main();

                

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
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
            #endregion

        }

        private void grdMain_Paint(object sender, PaintEventArgs e)
        {
            foreach (Order.CrtInOrdCre.HostedControl hosted in _al)
                hosted.UpdatePosition();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
