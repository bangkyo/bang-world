﻿using C1.Win.C1FlexGrid;
using ComLib;
using System;
using System.Data;
using System.Windows.Forms;

namespace SystemControlClassLibrary.Results
{
    public partial class WGTRsltSecPopUP : Form
    {
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        DataTable Send_dt;

        DataTable grdMain_dt;

        private int check_row_value = 0;
        private int popup_dt_rownumber = 0 ;


        string line_No = string.Empty;

        public WGTRsltSecPopUP(string _line_No)
        {
            line_No = _line_No;

            InitializeComponent();
        }


        private void InitControl()
        {
            InitGrd_Main();
        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            //var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["CHECKER"].Index);//, 0, grdMain.Cols["REMARKS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
           // grdMain.Cols["CHECKER"].Width = cs.Sel_Width;
            grdMain.Cols["BUNDLE_NO"].Width = cs.BUNDLE_QTY_Width + 40;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_L_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 10;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width - 10;
            grdMain.Cols["PCS"].Width = 70;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width + 10;
            grdMain.Cols["END_DDTT"].Width = cs.Date_8_Width;
            grdMain.Cols["THEORY_WGT"].Width = 120;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            //grdMain.Cols["CHECKER"].TextAlign = cs.SEL_TextAlign;
            grdMain.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_QTY_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["PCS"].TextAlign = TextAlignEnum.RightCenter;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.MFG_METHOD_TextAlign;
            grdMain.Cols["END_DDTT"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["THEORY_WGT"].TextAlign = TextAlignEnum.RightCenter;





        }

        private void BNDRsltInqPopup_Load(object sender, EventArgs e)
        {
            InitControl();
            SetDataBinding();
            MakeGrdData();
            InitGridData();
        }

        private void InitGridData_Main()
        {
            grdMain.SetDataBinding(grdMain_dt, null, true);
        }

        private void MakeGrdData()
        {
            grdMain_dt = vf.CreateDataTable(grdMain);
        }

        private void InitGridData()
        {
            InitGridData_Main();
        }

        private void SetDataBinding()
        {

            string show_msg = string.Empty;

            InitGridData();

            try
            {
                string sql1 = string.Format("SELECT ROWNUM AS L_NO ");
                //sql1 += string.Format("      ,'False' AS CHECKER ");
                sql1 += string.Format("      ,FN_GET_WGT(X.ITEM,X.ITEM_SIZE,X.LENGTH,X.PCS) AS THEORY_WGT ");
                sql1 += string.Format("      ,X.*   ");
                sql1 += string.Format("FROM   (  ");
                sql1 += string.Format("        SELECT   ");
                sql1 += string.Format("              A.BUNDLE_NO  AS BUNDLE_NO");
                sql1 += string.Format("              ,A.POC_NO AS POC_NO ");
                sql1 += string.Format("              ,A.HEAT   AS HEAT");
                sql1 += string.Format("              ,A.STEEL AS STEEL  ");
                sql1 += string.Format("              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("              ,A.ITEM AS ITEM");
                sql1 += string.Format("              ,A.ITEM_SIZE AS ITEM_SIZE ");
                sql1 += string.Format("              ,A.LENGTH AS LENGTH ");
                sql1 += string.Format("              ,A.PCS AS PCS ");
                sql1 += string.Format("              ,TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD') AS MFG_DATE ");
                sql1 += string.Format("              ,TO_CHAR(TO_DATE(SUBSTR(WORK_END_DDTT, 9, 6),'HH24MISS'),'HH24:MI:SS') AS END_DDTT  ");
                sql1 += string.Format("        FROM   TB_BND_WR A  ");
                sql1 += string.Format("        WHERE  A.LINE_GP = '{0}' ", line_No);
                sql1 += string.Format("        AND    NOT EXISTS ( SELECT BUNDLE_NO FROM TB_WGT_WR ");
                sql1 += string.Format("                            WHERE  BUNDLE_NO = A.BUNDLE_NO  ");
                sql1 += string.Format("                            AND    NVL(DEL_YN,'N') <> 'Y' ) ");
                sql1 += string.Format("        AND    NVL(A.DEL_YN,'N') <> 'Y'  ");
                sql1 += string.Format("       ) X    ");

                olddt = cd.FindDataTable(sql1);
                moddt = olddt.Copy();
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                //if(moddt.Rows.Count.ToString() > "0")

                //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {

                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;
           
            popup_dt_rownumber = e.Row;

            //if (editedRow <= 0 || editedGrd.GetData(editedRow, editedCol) == null || editedCol != editedGrd.Cols["CHECKER"].Index)
            //{
            //    e.Cancel = true;
            //}

            check_row_value = e.Row;
        }
        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {
           

          
        }

        private void sel_btn_Click(object sender, EventArgs e)
        {

            ck.Temptable = (DataTable)grdMain.DataSource;


            // 전달할 데이터 테이블을 생성
            int num = moddt.Rows.Count-1 ;

            while (num > -1)
            {
                if ((check_row_value - 1) != num)
                {
                    ck.Temptable.Rows[num].Delete();
                }
                else
                {
                    ck.Temptable.Rows.Add(grdMain.Rows[num]);
                    //ck.Temptable.Rows[num]["CHECKER"] = false;
                }
                num--;
            }

            ck.Temptable.AcceptChanges();

            this.Close();
            //체커로 로우 구분할 때
            //int num = moddt.Rows.Count-1;

            //while (num > -1 )
            //{
            //    if (ck.Temptable.Rows[num]["CHECKER"].ToString() == "False")
            //    {
            //        ck.Temptable.Rows[num].Delete();
            //    }
            //    else
            //    {
            //        ck.Temptable.Rows.Add(grdMain.Rows[num]);
            //        //ck.Temptable.Rows[num]["CHECKER"] = false;
            //    }
            //    num--;
            //}

            //ck.Temptable.AcceptChanges();

            //this.Close();


        }

        private void grdMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ck.Temptable = (DataTable)grdMain.DataSource;


            // 전달할 데이터 테이블을 생성
            int num = moddt.Rows.Count - 1;

            while (num > -1)
            {
                if ( (check_row_value - 1) != num)
                {
                    ck.Temptable.Rows[num].Delete();
                }
                else
                {
                    ck.Temptable.Rows.Add(grdMain.Rows[num]);
                    //ck.Temptable.Rows[num]["CHECKER"] = false;
                }
                num--;
            }

            ck.Temptable.AcceptChanges();

            this.Close();
        }

        
    }
}
