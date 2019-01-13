using C1.Win.C1FlexGrid;
using ComLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControlClassLibrary.monitoring;

namespace SystemControlClassLibrary.Order
{
    public partial class CrtInBundleInfo : Form
    {
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();


        string poc_No = string.Empty;
        string poc_seq_No = string.Empty;

        private DataTable olddt;
        private DataTable moddt;

        private DataTable moddt1;
        private DataTable Acltable;
        private bool CanMODAcl;

        public CrtInBundleInfo(string _poc_No, string _poc_seq_No)
        {
            poc_No = _poc_No;
            poc_seq_No = _poc_seq_No; 

            InitializeComponent();

        }


        private void CrtInBundleInfo_Load(object sender, EventArgs e)
        {
            initControl();

            if (!string.IsNullOrWhiteSpace(poc_seq_No))
            {
                SetDataBindingWithPOCseq();
            }
            else
            {
                SetDataBindingWithoutPOCseq();
            }
            
        }

        private void initControl()
        {

            //cs.InitButton(btnClose);

            #region 유저컨트롤 설정
            //POC NO
            uC_POC1.TbText = poc_No;

            cs.InitButton_1(btnClose);
            cs.InitButton_1(btnExcel);

            
            //POC SEQ NO
            uC_POC_SEQ1.TbText = poc_seq_No;

            #endregion

            InitGrd();

            if (string.IsNullOrEmpty(poc_seq_No))
            {
                uC_POC_SEQ1.Visible = false;
                grdMain.Cols["CRTIN_YN"].Visible = false;
            }
            else
            {
                Size = new Size(515, 515);
            }
        }

        private void InitGrd()
        {
            cs.InitGrid_search(grdMain);

            grdMain.AllowEditing = false;

            int nwidth = (grdMain.Size.Width - cs.L_No_Width - cs.Mill_No_Width) / 3 - 20;

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["CRTIN_YN"].Width = cs.CRTIN_YN_Width;
            grdMain.Cols["PCS"].Width = nwidth;
            grdMain.Cols["NET_WGT"].Width = nwidth;
            grdMain.Cols["MFG_DATE"].Width = nwidth;


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["PCS"].TextAlign = cs.FINISH_PCS_TextAlign;
            grdMain.Cols["NET_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["CRTIN_YN"].TextAlign = cs.CRTIN_YN_TextAlign;

            grdMain.ExtendLastCol = true;

            CellStyle yCellstyle = grdMain.Styles.Add("YCellStyle");
            yCellstyle.ForeColor = Color.LimeGreen;
            //yCellstyle.BackColor = Color.Transparent;
            //yCellstyle.Font = new Font(grdMain.Font, grdMain., FontStyle.Bold);
            grdMain.DrawMode = DrawModeEnum.OwnerDraw;
            grdMain.OwnerDrawCell += GrdMain_OwnerDrawCell;

        }

        private void GrdMain_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            var _flex = sender as C1FlexGrid;
            // ignore fixed cells
            if (e.Row < _flex.Rows.Fixed || e.Col < _flex.Cols.Fixed)
                return;

            // apply custom style if reorder level is critical
            if (_flex.Cols[e.Col].Name == "CRTIN_YN")
            {
                if (_flex[e.Row, "CRTIN_YN"].ToString() == "Y")
                    e.Style = _flex.Styles["YCellStyle"];
            }
        }

        private void SetDataBindingWithPOCseq()
        {
            string sql1 = string.Empty;

            sql1 += string.Format(" SELECT ROWNUM AS L_NO ");
            sql1 += string.Format("       ,X.* ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("     ( ");
            sql1 += string.Format("         SELECT  ");
            sql1 += string.Format("                MILL_NO ");
            sql1 += string.Format("               ,NVL((SELECT 'Y' FROM TB_CR_INPUT_WR B WHERE MILL_NO = A.MILL_NO),'N') AS CRTIN_YN ");
            sql1 += string.Format("               ,PCS ");
            sql1 += string.Format("               ,NET_WGT ");
            sql1 += string.Format("               ,TO_DATE(MFG_DATE) AS MFG_DATE ");
            sql1 += string.Format("         FROM  TB_CR_ORD_BUNDLEINFO A");
            sql1 += string.Format("         WHERE POC_NO = NVL(:P_POC_NO, ' ') ");
            sql1 += string.Format("         AND   POC_SEQ = NVL(:P_POC_SEQ, ' ') ");
            sql1 += string.Format("         ORDER BY MFG_DATE, MILL_NO ");
            sql1 += string.Format("     ) X ");

            string[] parm = new string[2];
            parm[0] = ":P_POC_NO|" + poc_No;
            parm[1] = ":P_POC_SEQ|" + poc_seq_No;
            

            olddt = cd.FindDataTable(sql1, parm);

            moddt = olddt.Copy();
            this.Cursor = Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = Cursors.Default;
            //SetGridCRTIN();
            grdMain.AutoSizeCols();
        }

        private void SetDataBindingWithoutPOCseq()
        {
            string sql1 = string.Empty;

            sql1 += string.Format(" SELECT ROWNUM AS L_NO ");
            sql1 += string.Format("       ,X.* ");
            sql1 += string.Format(" FROM ");
            sql1 += string.Format("     ( ");
            sql1 += string.Format("         SELECT  ");
            sql1 += string.Format("                MILL_NO ");
            sql1 += string.Format("               ,NULL AS CRTIN_YN ");
            sql1 += string.Format("               ,PCS ");
            sql1 += string.Format("               ,NET_WGT ");
            sql1 += string.Format("               ,TO_DATE(MFG_DATE) AS MFG_DATE ");
            sql1 += string.Format("         FROM  TB_CR_ORD_BUNDLEINFO ");
            sql1 += string.Format("         WHERE POC_NO = NVL(:P_POC_NO, ' ') ");
            sql1 += string.Format("         ORDER BY MFG_DATE, MILL_NO ");
            sql1 += string.Format("     ) X ");

            string[] parm = new string[1];
            parm[0] = ":P_POC_NO|" + poc_No;
            //parm[1] = ":P_POC_SEQ|" + poc_seq_No;


            moddt1 = cd.FindDataTable(sql1, parm);

            this.Cursor = Cursors.AppStarting;
            grdMain.SetDataBinding(moddt1, null, true);
            this.Cursor = Cursors.Default;

            
            grdMain.AutoSizeCols();
        }

        private void SetGridCRTIN()
        {
            for (int row = grdMain.Rows.Fixed; row < grdMain.Rows.Count; row++)
            {
                if (grdMain.GetData(row,"CRTIN_YN").ToString() == "Y")
                {
                    //grdMain.Rows[row]["CRTIN_YN"].ForeColor = Color.Green;
                    //grdMain.Rows[row, "CRTIN_YN"]
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(Text, grdMain);
        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {

            // 라인별 실시간 트래킹화면에서의 정보를 가져와서 수정가능한지 확인해서 
            // 권한이 있으면 진행
            // 권한이 없으면 리턴시킨다.

            // 권한에 따른 테이블 가져옴
            Acltable = cd.GetScreeAcl(ck.UserGrp);

            // 라인별 트래킹 화면의 투입화면의 NAME을 가져와서 수정가능한지 확인
            string CrtInScreenName = GetLineCrtInScreenName(ck.Line_gp);


            CanMODAcl = Line3WholeTrk.SetModifyACL(CrtInScreenName, Acltable, "MOD_ACL");

            //// 권한 없을 경우 리턴
            //if (!CanMODAcl)
            //{
            //    return;
            //}

            //sanity
            if (grdMain.Rows.Count < 2)
            {
                return;
            }

            string strY = grdMain.GetData(grdMain.Row, "CRTIN_YN").ToString();

            if ( strY == "Y")
            {
                return;
            }
            string _mill_no = grdMain.GetData(grdMain.Row, "MILL_NO").ToString();
            Line3WholeTrk.InSertCRT(ck.Line_gp, _mill_no, CanMODAcl);
        }

        public static string GetLineCrtInScreenName(string line_gp)
        {
            string rtn = "Line3CrtInRsltPopUP";

            switch (line_gp)
            {
                case "#1":
                    rtn = "Line1CrtInRsltPopUP";
                    break;
                case "#2":
                    rtn = "Line2CrtInRsltPopUP";
                    break;
                case "#3":
                    rtn = "Line3CrtInRsltPopUP";
                    break;

            }

            return rtn;
        }

        private void CrtInBundleInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            var temp_form = Line3WholeTrk.GetForm("CrtInRsltPopUP");

            if (temp_form != null)
            {
                temp_form.Close();
            }
        }
    }
}
