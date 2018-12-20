using C1.Win.C1FlexGrid;
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
using WindowsFormsApplication15;
//using WindowsFormsApplication15;

namespace SystemControlClassLibrary.monitoring
{
    public partial class Line3STRTrk : Form
    {

        private static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();

        DataTable moddt = null;

        string line_gp = "#3";

        public Line3STRTrk(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            InitializeComponent();
        }

        private void Line3STRTrk_Load(object sender, EventArgs e)
        {
            ck.StrKey1 = "";

            //uc_zone setting
            UC_Zone_Setup();

            InitGrd();

            timer1.Interval = 5000;
            timer1.Start();
            timer1_Tick(null, EventArgs.Empty); // Simulate a timer tick event
        }

        #region grd 서식 설정
        private void InitGrd()
        {
            //grdMain1
            InitgrdMain1();

            //grdMain7
            InitgrdMain7();

            //grdMain8
            InitgrdMain8();
        }

        private void InitgrdMain1()
        {
            cs.InitGrid_search(grdMain1, true);


            grdMain1[0, 0] = grdMain1[0, 1] = "교정";
            grdMain1[1, 0] = "교정속도";
            grdMain1[2, 0] = "ROLL GAP";
            grdMain1[3, 0] = "Top Roll Angel";
            grdMain1[4, 0] = "Bottom Roll Angel";

            //grdMain1.Rows[0].StyleFixed = c1FlexGrid1.Styles["Fixed1"];

            CellStyle cs_fix = grdMain1.Styles.Add("Fixed");
            cs_fix.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

            // row head 설정
            for (int row = 1; row < grdMain1.Rows.Count; row++)
            {
                grdMain1.SetCellStyle(row, 0, cs_fix);

            }

            grdMain1.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            grdMain1.Rows[0].AllowMerging = true;

            //for (int i = 0; i < grdMain1.Cols.Count; i++)
            //{
            //    grdMain1.Cols[i].AllowMerging = true;

            //}
            grdMain1.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain1.Cols[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;

            grdMain1.Row = -1;
        }

        private void InitgrdMain8()
        {
            cs.InitGrid_search(grdMain8, true);

            grdMain8.Cols["STEEL_NM"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["HEAT"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["ITEM_SIZE"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["LENGTH"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain8.Cols["SURFACE_LEVEL"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain8.Cols["POC_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
        }

        private void InitgrdMain7()
        {
            cs.InitGrid_search(grdMain7, true);

            grdMain7[0, 0] = "압연번들번호"; grdMain7[0, 1] = "압연본수"; grdMain7[0, 2] = "교정본수";

            grdMain7.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain7.Cols["MILL_NO"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
            grdMain7.Cols["MILL_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
            grdMain7.Cols["STR_PCS"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
        }
        #endregion



        private void UC_Zone_Setup()
        {
            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "WindowsFormsApplication15.UC_Zone")
                {
                    var uc_zone = item as UC_Zone;
                    uc_zone.PopupEvent += PopupEvent;

                }
            }
        }

        private void PopupEvent(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            if (!string.IsNullOrEmpty(((Control)sender).Parent.Name))
            {
                //ck.StrKey2 = ((Control)sender).Parent.Name;

                var uc_zone = (Control)((Control)sender).Parent as UC_Zone;
                ck.StrKey2 = uc_zone.ZoneCD;
            }

            OpenPopup();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            SetDataBinding();

            SetDataBinding_Zone();
        }



        private void SetDataBinding()
        {
            SetDataBinding_grdMain7();

            SetDataBinding_grdMain8();

        }

        private void SetDataBinding_grdMain8()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL),STEEL) AS STEEL_NM ");
                sql1 += string.Format("      ,A.HEAT ");
                sql1 += string.Format("      ,ITEM_SIZE ");
                sql1 += string.Format("      ,LENGTH ");
                sql1 += string.Format("      ,SURFACE_LEVEL ");
                sql1 += string.Format("      ,POC_NO ");
                sql1 += string.Format("FROM   TB_CR_ORD A ");
                sql1 += string.Format("WHERE  POC_NO = ( ");
                sql1 += string.Format("        SELECT DISTINCT POC_NO AS POC_NO ");
                sql1 += string.Format("        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("        WHERE  PROG_STAT   IN ('RUN','WAT') ");
                sql1 += string.Format("        AND    LINE_GP     =  :P_LINE_GP ");
                sql1 += string.Format("        AND    ROWNUM      = 1 ) ");

                //moddt = new DataTable();

                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);


                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain8.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {

                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }


            grdMain8.Row = -1;
            return;
        }

        private void SetDataBinding_grdMain7()
        {
            try
            {
                string sql1 = string.Empty;
                //sql1 += string.Format(" SELECT ");
                //sql1 += string.Format("        MILL_NO ");
                //sql1 += string.Format("       ,PIECE_NO ");
                //sql1 += string.Format("       ,POC_NO ");
                //sql1 += string.Format("       ,ZONE_CD ");
                //sql1 += string.Format("FROM   TB_RL_TM_TRACKING ");
                //sql1 += string.Format("WHERE ");
                //sql1 += string.Format("    PROG_STAT = 'WAT' ");

                //sql1 += string.Format("--교정대기번들정보 ");
                sql1 += string.Format("SELECT B.MILL_NO AS MILL_NO ");
                sql1 += string.Format("      ,A.PCS    AS MILL_PCS ");
                sql1 += string.Format("      ,C.WR_PCS AS STR_PCS ");
                sql1 += string.Format("FROM   TB_CR_ORD_BUNDLEINFO A ");
                sql1 += string.Format("      ,(SELECT DISTINCT MILL_NO AS MILL_NO ");
                sql1 += string.Format("        FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("        WHERE  PROG_STAT   IN ('RUN','WAT') ");
                sql1 += string.Format("        AND    LINE_GP     =  :P_LINE_GP ");
                sql1 += string.Format("        AND    ZONE_CD     LIKE '_Z01') B ");
                sql1 += string.Format("      ,(SELECT MILL_NO, COUNT(*) AS WR_PCS ");
                sql1 += string.Format("        FROM   TB_CR_PIECE_WR ");
                sql1 += string.Format("        WHERE  LINE_GP     = :P_LINE_GP ");
                sql1 += string.Format("        AND    ROUTING_CD  = 'A1' ");
                sql1 += string.Format("        AND    REWORK_SEQ  = 0 ");
                sql1 += string.Format("        GROUP BY MILL_NO ) C ");
                sql1 += string.Format("WHERE  B.MILL_NO  = A.MILL_NO ");
                sql1 += string.Format("AND    B.MILL_NO  = C.MILL_NO(+) ");

                //moddt = new DataTable();
                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + line_gp;

                moddt = cd.FindDataTable(sql1, parm);

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain7.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {

                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }


            grdMain7.Row = -1;
            return;
        }

        private bool SetDataBinding_Zone()
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ZONE_CD ");
                sql1 += string.Format("      ,MIN(MILL_NO) AS MILL_NO ");
                sql1 += string.Format("      ,COUNT(*)     AS PCS ");
                sql1 += string.Format("FROM   TB_RL_TM_TRACKING ");
                sql1 += string.Format("WHERE  PROG_STAT   IN ('RUN','WAT') ");
                sql1 += string.Format("AND    LINE_GP     =  '{0}' ",line_gp);
                sql1 += string.Format("GROUP BY ZONE_CD ");

                //moddt = new DataTable();

                moddt = cd.FindDataTable(sql1);

                foreach (var item in vf.GetAllChildrens(this))
                {
                    if (item.GetType().ToString() == "WindowsFormsApplication15.UC_Zone")
                    {
                        var uc_zone = item as UC_Zone;
                        //zone 초기화
                        uc_zone.PCS = "0";
                        uc_zone.MillNo = "";

                        foreach (DataRow row in moddt.Rows)
                        {
                            if (uc_zone.ZoneCD == row["ZONE_CD"].ToString())
                            {
                                uc_zone.PCS = row["PCS"].ToString();
                                uc_zone.MillNo = row["MILL_NO"].ToString();
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }
            return true;
        }




        private void btnZoneMove_Click(object sender, EventArgs e)
        {
            ck.StrKey1 = line_gp;

            ck.StrKey2 = "";

            //if (((Control)sender).Parent =="")

            OpenPopup();

        }

        private void OpenPopup()
        {
            InsRegPopup popup = new InsRegPopup();
            //popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            //popup.Show();
            popup.ShowDialog();
        }
    }
}
