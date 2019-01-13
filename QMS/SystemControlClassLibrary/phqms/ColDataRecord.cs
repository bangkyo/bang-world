using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComLib;
using ComLib.clsMgr;
using C1.Win.C1FlexGrid;

namespace SystemControlClassLibrary.phqms
{
    public partial class ColDataRecord : Form
    {
        VbFunc vf = new VbFunc();
        ConnectDB cd = new ConnectDB();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        string biz_gp, mv_table_id;
        private static string titleNM = "";

        public ColDataRecord()
        {
            InitializeComponent();
        }

        public ColDataRecord(string _biz_gp, string _mv_table_id)
        {
            InitializeComponent();

            biz_gp = _biz_gp;
            mv_table_id = _mv_table_id;
        }

        private void ColDataRecord_Load(object sender, EventArgs e)
        {
            InitGrid();

            txtBiz.Text = biz_gp;
            txtMView.Text = mv_table_id;
            start_dt.Value = DateTime.Today.AddDays(-2);

            end_dt.Value = DateTime.Now;

            SetDataBinding();
        }

        private void InitGrid()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["REFRESH_DDTT"].Caption = "REFRESH 일시";
            grdMain.Cols["OPER_DATA_CNT"].Caption = "운영 데이터 건수";
            grdMain.Cols["MLOG_DATA_CNT"].Caption = "MLOG 데이터 건수";
            grdMain.Cols["GATH_DATA_CNT"].Caption = "수집(MV) 데이터 건수";
            grdMain.Cols["ERROR_YN"].Caption = "에러 여부";
            grdMain.Cols["ERROR_MSG"].Caption = "에러 메세지";


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["REFRESH_DDTT"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["OPER_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["MLOG_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["GATH_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["ERROR_YN"].TextAlign = TextAlignEnum.LeftCenter;
            grdMain.Cols["ERROR_MSG"].TextAlign = TextAlignEnum.LeftCenter;


            grdMain.Cols["L_NO"].Width = 50;
            grdMain.Cols["REFRESH_DDTT"].Width = 200;
            grdMain.Cols["OPER_DATA_CNT"].Width = 180;
            grdMain.Cols["MLOG_DATA_CNT"].Width = 180;
            grdMain.Cols["GATH_DATA_CNT"].Width = 180;
            grdMain.Cols["ERROR_YN"].Width = 100;
            grdMain.Cols["ERROR_MSG"].Width = 300;

        }

        private void Button_Click(object sender, EventArgs e)
        {

            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    //cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    SetDataBinding();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;

                case "btnClose":
                    this.Close();
                    break;
            }
        }

        private bool SetDataBinding()
        {
            try
            {
                txtMView.Value = txtMView.Text;

                string sql1 = "";
                sql1 += string.Format(@" SELECT ROWNUM AS L_NO, X.*
                                         FROM (
                                            SELECT REFRESH_DDTT
                                                 , OPER_DATA_CNT
                                                 , MLOG_DATA_CNT
                                                 , GATH_DATA_CNT
                                                 , ERROR_YN
                                                 , ERROR_MSG
                                            FROM TB_MV_REFRESH_LOG
                                            WHERE BIZ_GP LIKE '%{0}%' 
                                                AND TABLE_ID LIKE '%{1}%' 
                                                AND (REFRESH_DDTT >= '{2}'
                                                    AND REFRESH_DDTT <= TO_DATE('{3}','YYYYMMDDHH24MISS'))
                                            ORDER BY REFRESH_DDTT DESC 
                                         )X   ", txtBiz.Text, txtMView.Text, start_dt.Text, end_dt.Value.ToString("yyyyMMddHHmmss"));

                olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString() + "건이 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }
            return true;
        }
        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }
    }
}
