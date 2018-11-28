using BGsystemLibrary.MatMgmt;
using BGsystemLibrary.SystemMgmt;
using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Data;
using System.Windows.Forms;

namespace BGsystemLibrary.Common
{
    public partial class ItemSizePopup : Form
    {
        //공통
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        DataTable olddt_sub;
        DataTable moddt_sub;
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();


        string txtdeptNm = "";



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
        public ItemSizePopup()
        {

            InitializeComponent();

        }

        private void ItemSizePopup_Load(object sender, EventArgs e)
        {
            DrawGrid(grdMain);
            Seach();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            clsFlexGrid.grdDataClearForNotBind(grdMain);
            Seach();
        }



        clsParameterMember param = new clsParameterMember();
        private void Seach()
        {
            string take_Over_gp = "N";
            string category = "ITEM_SIZE";
            string cd_id = txt_CD_ID.Text;
            try
            {

                take_Over_gp = ck.StrKey1;

                param.Clear();
                param.Add(SqlDbType.VarChar, "@P_CATEGORY", category, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_CD_ID", cd_id, ParameterDirection.Input);


                //SQL 쿼리 조회 후 데이터셋 return
                olddt_sub = cd.ExecuteStoreProcedureDataTable("SP_ComcdList", param);
                moddt_sub = olddt_sub.Copy();

                this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                DrawGrid(grdMain, moddt_sub);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //grdMain_Click(null, null);

            }

        }

        private void DrawGrid(C1.Win.C1FlexGrid.C1FlexGrid grdItem)
        {
            grdItem.BeginInit();
            try
            {
                clsFlexGrid.FlexGridMainSystem(grdItem, GridRowsCount, GridColsCount, RowsFixed, RowsFrozen, ColsFixed, ColsFrozen);

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
            grdItem[1, 1] = "규격";
            grdItem[1, 2] = "규격명";

            //grdItem.Cols[2].Visible = false;


            clsFlexGrid.TopBorderStyle(grdItem, 0, 0, 0, grdItem.Cols.Count - 1);
            clsFlexGrid.CaptionCellRangeColumnStyle(grdItem, 1, 0, 1, grdItem.Cols.Count - 1);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 0);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 1);
            clsFlexGrid.DataGridCenterStyle(grdItem, 0, 2);

        }
        private void txt_Nm_TextChanged(object sender, EventArgs e)
        {
            txtdeptNm = txt_CD_ID.Text;

        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            ck.StrKey1 = grdMain.GetData(grdMain.RowSel, "CD_ID").ToString();
            ck.StrKey2 = grdMain.GetData(grdMain.RowSel, "CD_NM").ToString();
            this.Close();
        }

        private void grdMain_Click(object sender, EventArgs e)
        {

        }
    }
}
