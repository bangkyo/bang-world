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
    public partial class AnalDataRecord : Form
    {
        VbFunc vf = new VbFunc();
        ConnectDB cd = new ConnectDB();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        string biz_gp, sp_id;
        private static string titleNM = "";

        public AnalDataRecord()
        {
            InitializeComponent();
        }

        public AnalDataRecord(string _biz_gp, string _sp_id)
        {
            InitializeComponent();

            biz_gp = _biz_gp;
            sp_id = _sp_id;
        }

        private void AnalDataRecord_Load(object sender, EventArgs e)
        {
            InitGrid();

            txtBiz.Text = biz_gp;
            txtSpId.Text = sp_id;
            start_dt.Value = DateTime.Today.AddDays(-2);

            end_dt.Value = DateTime.Now;


            SetDataBinding();
        }

        private void InitGrid()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["MOD_DDTT"].Caption = "SP처리일시";
            grdMain.Cols["BI_DATA_CNT"].Caption = "BI데이터 건수";
            grdMain.Cols["MV_DATA_CNT"].Caption = "MV데이터 건수";
            grdMain.Cols["DIFF_CNT"].Caption = "차이";
            grdMain.Cols["INSERT_CNT"].Caption = "INSERT 건수";
            grdMain.Cols["UPDATE_CNT"].Caption = "UPDATE 건수";
            grdMain.Cols["DELETE_CNT"].Caption = "DELETE 건수";
            grdMain.Cols["ERROR_YN"].Caption = "에러여부";
            grdMain.Cols["ERROR_MSG"].Caption = "에러 메세지";


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["MOD_DDTT"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["BI_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["MV_DATA_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["DIFF_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["INSERT_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["UPDATE_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["DELETE_CNT"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["ERROR_YN"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["ERROR_MSG"].TextAlign = TextAlignEnum.LeftCenter;

            grdMain.Cols["L_NO"].Width = 50;
            grdMain.Cols["MOD_DDTT"].Width = 200;
            grdMain.Cols["BI_DATA_CNT"].Width = 180;
            grdMain.Cols["MV_DATA_CNT"].Width = 180;
            grdMain.Cols["DIFF_CNT"].Width = 100;
            grdMain.Cols["INSERT_CNT"].Width = 180;
            grdMain.Cols["UPDATE_CNT"].Width = 180;
            grdMain.Cols["DELETE_CNT"].Width = 180;
            grdMain.Cols["ERROR_YN"].Width = 100;
            grdMain.Cols["ERROR_MSG"].Width = 500;

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
                txtSpId.Value = txtSpId.Text;

                string sql1 = "";
                sql1 += string.Format(@" SELECT ROWNUM AS L_NO, X.*
                                         FROM (
                                            SELECT 
                                                MOD_DDTT
                                                ,BI_DATA_CNT
                                                ,MV_DATA_CNT
                                                ,(MV_DATA_CNT - BI_DATA_CNT) AS DIFF_CNT
                                                ,INSERT_CNT
                                                ,UPDATE_CNT
                                                ,DELETE_CNT
                                                ,ERROR_YN
                                                ,ERROR_MSG
                                            FROM TB_SP_TABLE
                                            WHERE BIZ_GP LIKE '%' || '{0}' || '%'
                                              AND SP_ID LIKE '%' || '{1}' || '%' 
                                              AND (MOD_DDTT >= '{2}'
                                                    AND MOD_DDTT <= TO_DATE('{3}','YYYYMMDDHH24MISS'))
                                            ORDER BY MOD_DDTT DESC
                                         )X   ", txtBiz.Text, txtSpId.Text, start_dt.Text, end_dt.Value.ToString("yyyyMMddHHmmss"));

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
