using BGsystemLibrary.SystemMgmt;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Data;
using System.Windows.Forms;

namespace BGsystemLibrary.Common
{
    public partial class PopUp : Form
    {
        //공통
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        DataTable olddt_sub;
        DataTable moddt_sub;
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();


        string txtdeptNm = "";

        public UserMgmt frm;

        public delegate void FormSendDataHandler(object obj);
        public event FormSendDataHandler FormSendEvent;

        //그리드 변수 
        private int GridRowsCount = 2;
        private int GridColsCount = 3;
        private int RowsFixed = 2;
        private int RowsFrozen = 0;
        private int ColsFixed = 0;
        private int ColsFrozen = 0;

        private int TopRowsHeight = 2;
        private int DataRowsHeight = 35;

        private string titleNM = "";
        private string menuNM = "";
        private string categoryNM = "";

        public PopUp(string _titleNM, string _menuNM, string _categoryNM)
        {

            titleNM = _titleNM;
            menuNM = _menuNM;
            categoryNM = _categoryNM;

            InitializeComponent();

            Text = titleNM;
            lbl_searchItem.Text = menuNM;
            categoryNM = _categoryNM;

        }


        private void DeptPopUp_Load(object sender, EventArgs e)
        {

            //InitGrd_Main();
            DrawGrid(grdMain);
            Seach();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            Seach();
        }

        private void Seach()
        {
            string Sql = "";
            Sql += string.Format("SELECT CONVERT(VARCHAR, ROW_NUMBER() OVER( ORDER BY CD_ID ASC)) AS L_NUM  ");
            Sql += string.Format("      ,CD_ID                                                              ");
            Sql += string.Format("      ,CD_NM                                                              ");
            Sql += string.Format("  FROM TB_CM_COM_CD                                                       ");
            Sql += string.Format(" WHERE CATEGORY = 'DEPT_CD'                                               ");
            Sql += string.Format("   AND USE_YN = 'Y'                                                       ");
            Sql += string.Format("   AND USE_YN = 'Y'                                                       ");
            Sql += string.Format("   AND CD_NM LIKE '%" + txtdeptNm + "%'                                   ");
            olddt_sub = cd.FindDataTable(Sql);
            moddt_sub = olddt_sub.Copy();

            //MessageBox.Show(olddt_sub.Rows.Count + "");
            DrawGrid(grdMain, moddt_sub);
            //grdMain.SetDataBinding(moddt_sub, null, true);
        }
        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                if (grdItem.Rows.Count == 1)
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                }
                else
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, grdItem.Rows.Count, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                }

                FlexGridCol(grdItem);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndInit();
                grdItem.Invalidate();
            }
        }

        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dataTable)
        {
            int RowsCount = 0;
            grdItem.BeginUpdate();

            try
            {
                //grdItem.ScrollBars = ScrollBars.Both;
                if (dataTable.Rows.Count == 0)
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                }
                else
                {
                    clsFlexGrid.FlexGridMainSystem(grdItem, dataTable.Rows.Count + 2, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);
                }

                grdItem.AllowEditing = true;
                grdItem.Cols[1].AllowEditing = true;
                grdItem.Cols[2].AllowEditing = true;

                RowsCount = clsFlexGrid.FlexGridBinding(grdItem, dataTable);
                clsFlexGrid.FlexGridRow(grdItem, TopRowsHeight, DataRowsHeight);

                grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;

                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), RowsCount, "조회하였습니다.");
            }
            catch (Exception ex)
            {
                clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), ex.Message);
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            finally
            {
                grdItem.EndUpdate();
                grdItem.Invalidate();
            }
        }

        private void FlexGridCol(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {

            grdItem.Cols[0].Width = 60;
            grdItem.Cols[1].Width = 100;
            grdItem.Cols[2].Width = 100;

            grdItem[1, 0] = "NO";
            grdItem[1, 1] = "부서코드";
            grdItem[1, 2] = "부서명";

            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            clsFlexGrid.DataGridLeftStyle(grdItem, 2);

        }
        private void txtDeptNm_TextChanged(object sender, EventArgs e)
        {
            txtdeptNm = searchItemNm.Text;

        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            frm.txtdept = grdMain.GetData(grdMain.RowSel, "CD_NM").ToString();
            frm.txtdeptCd = grdMain.GetData(grdMain.RowSel, "CD_ID").ToString();
            this.FormSendEvent(null);

            this.Dispose();
        }
    }
}
