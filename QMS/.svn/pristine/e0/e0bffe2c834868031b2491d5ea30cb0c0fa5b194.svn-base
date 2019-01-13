using ComLib;
using ComLib.clsMgr;
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
using SystemControlClassLibrary.information;

namespace SystemControlClassLibrary.Popup
{
    public partial class SearchSteelNm : Form
    {
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        DataTable olddt;

        private  string gangjung_id = "";
        private  string gangjung_nm = "";



        private int Data = 0;

        private string search_Item = "";

        public SearchSteelNm()
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";

            InitializeComponent();
        }

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            // search_item 에 해당하는 테이블을 검색해서 grdMain에 뿌려줌.
            string strQry = string.Empty;

            strQry += string.Format("select  ");
            strQry += string.Format("    ROWNUM AS L_NO");
            strQry += string.Format("   ,CD_ID ");
            strQry += string.Format("   ,CD_NM ");
            strQry += string.Format("FROM  ");
            strQry += string.Format("    TB_CM_COM_CD  ");
            strQry += string.Format("WHERE  ");
            strQry += string.Format("    category = 'STEEL' ");
            strQry += string.Format("AND ( ");
            strQry += string.Format("        CD_ID LIKE '%{0}%'  ", search_Item);
            strQry += string.Format("    OR  CD_NM LIKE '%{0}%' ", search_Item);
            strQry += string.Format("    )  ");

            olddt = cd.FindDataTable(strQry);

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(olddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;


            grdMain.Row = -1;

        }

        private void steelID_tb_TextChanged(object sender, EventArgs e)
        {
            search_Item = steelID_tb.Text;
        }

        private void SearchSteelNm_Load(object sender, EventArgs e)
        {
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            InitControl();

            // 폰트 초기화

            search_btn_Click(null, null);
        }

        private void InitControl()
        {
            clsStyle.Style.InitLabel(search_lb);

            clsStyle.Style.InitButton(search_btn);
            clsStyle.Style.InitButton(sel_btn);
            clsStyle.Style.InitButton(cancel_btn);

            InitGrd_Main();

        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);


            //InitGrid_search(grdMain);

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            //grdMain.AllowEditing = false;


            //grdMain.Cols[0].Width = 50;
            //grdMain.Cols[1].Width = 250;
            //grdMain.Cols[2].Width = 150;
            //grdMain.Cols[3].Width = 150;
            //grdMain.Cols[4].Width = 0;
            ////grdMain.Cols[5].Width = 0;

            //grdMain.AllowEditing = true;

            //for (int col = 0; col < grdMain.Cols.Count; col++)
            //{
            //    grdMain.Cols[col].AllowEditing = false;
            //}

            //grdMain.Cols[5].AllowEditing = true;

            grdMain.Cols["L_NO"].Width = 50;
            grdMain.Cols["CD_ID"].Width = 70;
            grdMain.Cols["CD_NM"].Width = cs.Usage_CD_NM_Width;
           

            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["CD_ID"].Caption = "강종";
            grdMain.Cols["CD_NM"].Caption = "강종명";


            #region 1. grdMain head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["CD_ID"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain.Cols["CD_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

            #endregion
        }

        private void sel_btn_Click(object sender, EventArgs e)
        {
            //CHMInfo fa = (CHMInfo)this.Owner; //Owner를 부모폼으로 형변환한다.

            ////A폼의 TextBox1이  public으로 선언되어 있기 때문에 접근이 가능하다.

            //fa.gangjong_id_tb.Text = gangjung_id;
            //fa.gangjong_Nm_tb.Text = gangjung_nm;

            ck.StrKey1 = gangjung_id;
            ck.StrKey2 = gangjung_nm;
           
            this.Close();

        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            // grd head -1
            // grd row 1 번부터 start
            if (grdMain.RowSel > 0)
            {
                gangjung_id = grdMain.GetData(grdMain.RowSel, "CD_ID").ToString();
                gangjung_nm = grdMain.GetData(grdMain.RowSel, "CD_NM").ToString();
            }

            //cd_id =
        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain.RowSel < 1)
            {
                return;
            }
            //if (grdMain.RowSel > 0)
            //{
            //    gangjung_id = grdMain.GetData(grdMain.RowSel, "CD_ID").ToString();
            //    gangjung_nm = grdMain.GetData(grdMain.RowSel, "CD_NM").ToString();
            //}

            sel_btn_Click(null, null);
        }
    }
}
