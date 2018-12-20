﻿using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SystemControlClassLibrary.Order
{
    [System.Runtime.InteropServices.Guid("49C2116D-3FFA-47CE-B80E-644BFFF7337A")]
    public partial class CrtInOrdInq : Form
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

        //List<string> modifList;


        //private int selectedrow = 0;



        private  string result_stat = "";
        private  string result_msg = "";

        private  string ownerNM = "";
        private  string titleNM = "";

        C1FlexGrid selectedGrd = null;

        string ling_gp;
        DateTime start_date;
        DateTime end_date;
        string itemsize;
        string gangjong_id;

        public CrtInOrdInq(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            ck.StrKey1 = "";
            ck.StrKey2 = "";

            InitializeComponent();

            //this.Icon = Properties.Resources.H_child_form_icon;


            selectedGrd = grdMain;

            

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
            string sql1 = string.Empty;
            sql1 += string.Format("SELECT TO_CHAR(ROWNUM) AS L_NO ");
            sql1 += string.Format("       ,'False' AS SEL ");
            sql1 += string.Format("       ,POC_PROG_STAT ");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'POC_PROG_STAT' AND CD_ID = X.POC_PROG_STAT) AS POC_PROG_STAT_NM ");
            sql1 += string.Format("       ,LINE_GP ");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'LINE_GP' AND CD_ID = X.LINE_GP) AS LINE_GP_NM ");
            //sql1 += string.Format("       ,WORK_ORD_DATE ");
            sql1 += string.Format("       ,TO_DATE(WORK_ORD_DATE, 'YYYY-MM-DD') AS WORK_ORD_DATE ");
            sql1 += string.Format("       ,WORK_RANK ");
            sql1 += string.Format("       ,WORK_RANK AS ORG_WORK_RANK ");
            sql1 += string.Format("       ,ITEM_SIZE ");
            sql1 += string.Format("       ,STEEL ");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
            sql1 += string.Format("       ,LENGTH ");
            sql1 += string.Format("       ,SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = X.SURFACE_LEVEL) AS SURFACE_LEVEL_NM ");
            sql1 += string.Format("       ,ORD_WGT ");
            sql1 += string.Format("       ,INPUT_WGT ");
            sql1 += string.Format("       ,END_WGT ");
            sql1 += string.Format("FROM   ( ");
            sql1 += string.Format("         SELECT  LINE_GP ");
            sql1 += string.Format("                ,WORK_ORD_DATE ");
            sql1 += string.Format("                ,WORK_RANK ");
            sql1 += string.Format("                ,ITEM_SIZE ");
            sql1 += string.Format("                ,STEEL ");
            sql1 += string.Format("                ,LENGTH ");
            sql1 += string.Format("                ,MAX(SURFACE_LEVEL) AS SURFACE_LEVEL ");
            sql1 += string.Format("                ,SUM(ORD_WGT) AS ORD_WGT ");
            sql1 += string.Format("                ,SUM((SELECT SUM(NET_WGT)  FROM  TB_CR_INPUT_WR WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ)) AS INPUT_WGT ");
            sql1 += string.Format("                ,SUM((SELECT SUM(NET_WGT) FROM TB_BND_WR WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ AND NVL(DEL_YN,'N') <> 'Y' )) AS END_WGT ");
            sql1 += string.Format("                ,MAX(POC_PROG_STAT) AS POC_PROG_STAT ");
            sql1 += string.Format("         FROM   TB_CR_INPUT_ORD  A ");
            sql1 += string.Format("         WHERE  LINE_GP   =    :P_LINE_GP ");
            sql1 += string.Format("         AND    WORK_ORD_DATE BETWEEN :P_FR_DATE AND :P_TO_DATE ");
            if (txtPoc.Text != "")
                sql1 += string.Format("         AND    ITEM_SIZE = (select DISTINCT ITEM_SIZE from TB_CR_INPUT_ORD WHERE POC_NO = '{0}') ", txtPoc.Text);
            else
            sql1 += string.Format("         AND    ITEM_SIZE LIKE :P_ITEM_SIZE || '%' ");
            if (txtPoc.Text != "")
                sql1 += string.Format("         AND    STEEL = (select DISTINCT STEEL from TB_CR_INPUT_ORD WHERE POC_NO = '{0}') ", txtPoc.Text);
            else
                sql1 += string.Format("         AND    STEEL     LIKE :P_STEEL || '%' ");
            sql1 += string.Format("         AND    POC_PROG_STAT > 'A'   "); // -- 확정대기부터 조회
            sql1 += string.Format("         GROUP BY LINE_GP, WORK_ORD_DATE, WORK_RANK, ITEM_SIZE, STEEL, LENGTH ");
            sql1 += string.Format("         ORDER BY 1,2,3,4,5,6 ");
            sql1 += string.Format("        ) X ");

            if (txtPoc.Text == "")
            {
                string[] parm = new string[5];
                parm[0] = ":P_LINE_GP|" + ling_gp;
                parm[1] = ":P_FR_DATE|" + vf.Format(start_date, "yyyyMMdd");
                parm[2] = ":P_TO_DATE|" + vf.Format(end_date, "yyyyMMdd");
                parm[3] = ":P_ITEM_SIZE|" + itemsize;
                parm[4] = ":P_STEEL|" + gangjong_id;
                olddt = cd.FindDataTable(sql1, parm);
            }
            else
            {
                string[] parm = new string[3];
                parm[0] = ":P_LINE_GP|" + ling_gp;
                parm[1] = ":P_FR_DATE|" + vf.Format(start_date, "yyyyMMdd");
                parm[2] = ":P_TO_DATE|" + vf.Format(end_date, "yyyyMMdd");
                olddt = cd.FindDataTable(sql1, parm);
            }
            //olddt = cd.FindDataTable(sql1, parm);

            moddt = olddt.Copy();
          
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(moddt, null, true);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            //grdMain.RowSel = 1;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            
        }

        private void CrtInOrdInq_Load(object sender, EventArgs e)
        {
            InitControl();

            // 초기 값 설정
            //cbLine_gp.SelectedIndex = 0;

            //ling_gp = ((ComLib.DictionaryList)cbLine_gp.SelectedItem).fnValue;
            start_date = start_dt.Value = DateTime.Now;
            end_date = end_dt.Value = DateTime.Now;
            itemsize = tbItemSize.Text;
            gangjong_id = gangjong_id_tb.Text;

            MakeInitgrdData();

            EventCreate();      //사용자정의 이벤트

            btnDisplay_Click(null, null);
        }

        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(tbSteel_LostFocus);            //강종ID
        }

        private void tbSteel_LostFocus(object sender, EventArgs e)
        {
            if (gangjong_id_tb.Text == "")
            {
                gangjong_Nm_tb.Text = "";
                gangjong_id = "";
            }
            else
            {
                gangjong_Nm_tb.Text = cd.Find_Steel_NM_By_ID(gangjong_id_tb.Text);

                if (gangjong_Nm_tb.Text.Length == 0)
                {
                    if (MessageBox.Show(" 해당 강종에 따른 강종명을 찾을 수 없습니다.", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        gangjong_Nm_tb.Text = "";
                        gangjong_id_tb.Focus();
                        return;
                    }
                }
                else
                    gangjong_id = gangjong_id_tb.Text;
            }
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
        }


        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(lbLine);
            cs.InitLabel(lbIn);
            cs.InitLabel(lbSteel);
            cs.InitLabel(label4);

            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            cs.InitTextBox(tbItemSize);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(gangjong_Nm_tb);

            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            cs.InitButton(btnSave); //btnReg
            cs.InitButton(btnReg);

            cs.InitButton(btnApply);
            cs.InitButton(btnApplyCancel);
            cs.InitButton(btnHeat);

            cs.InitCombo(cbLine_gp, StringAlignment.Near);

            //cd.SetCombo(cbLine_gp, "LINE_GP", "", false);
            cd.SetCombo(cbLine_gp, "LINE_GP", false, ck.Line_gp);

            InitGrd_Main();
        }

        private void End_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void Start_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void InitGrd_Main()
        {
            InitGrd_grdMain();

            InitGrd_grdSub();
            
        }

        private void InitGrd_grdSub()
        {
            C1FlexGrid grd = grdSub as C1FlexGrid;

            cs.InitGrid_search(grd);

            var crCellRange = grd.GetCellRange(0, grd.Cols["SEL"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grd.Styles["ModifyStyle"];

            grd.AllowEditing = true;
            //grd.Cols["SEL"].AllowEditing = true;

            grd.Cols["L_NO"].Width = cs.L_No_Width;
            grd.Cols["POC_NO"].Width = cs.POC_NO_Width;
            grd.Cols["HEAT"].Width = cs.HEAT_Width;
            grd.Cols["SEL"].Width = cs.Sel_Width;
            grd.Cols["STEEL"].Width = cs.STEEL_Width;
            grd.Cols["STEEL_NM"].Width = cs.STEEL_NM_L_Width;
            grd.Cols["ITEM"].Width = cs.ITEM_Width;
            grd.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grd.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grd.Cols["BUNDLE_QTY"].Width = cs.BUNDLE_QTY_Width;
            grd.Cols["ORD_WGT"].Width = cs.Wgt_Width;
            grd.Cols["INPUT_BUNDLE_QTY"].Width = cs.BUNDLE_QTY_Width;
            grd.Cols["INPUT_WGT"].Width = cs.Wgt_Width;

            grd.Cols["SURFACE_LEVEL_NM"].Width = cs.Surface_Level_NM_Width;
            grd.Cols["FINISH_YN"].Width = cs.FINISH_YN_Width;
            grd.Cols["MILL_DATE"].Width = cs.Date_8_Width;
            grd.Cols["COMPANY_NM"].Width = cs.Company_NM_Width;
            grd.Cols["USAGE_CD_NM"].Width = cs.Usage_CD_NM_Width;
            grd.Cols["END_WGT"].Width = cs.Wgt_Width;

            grd.Cols["POC_PROG_STAT"].Width = 0;
            grd.Cols["POC_PROG_STAT_NM"].Width = 0;

            grd.Cols["POC_PROG_STAT"].Width = 0;
            grd.Cols["LINE_GP"].Width = 0;
            grd.Cols["WORK_ORD_DATE"].Width = 0;
            grd.Cols["WORK_UOM"].Width = 0;
            grd.Cols["WORK_RANK"].Width = 0;
            grd.Cols["POC_SEQ"].Width = 0;

            //grd.Cols["WORK_UOM"].Width = 0;

            grd.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grd.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grd.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grd.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grd.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grd.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grd.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grd.Cols["BUNDLE_QTY"].TextAlign = cs.BUNDLE_QTY_TextAlign;
            grd.Cols["ORD_WGT"].TextAlign = cs.WGT_TextAlign;

            grd.Cols["INPUT_BUNDLE_QTY"].TextAlign = cs.BUNDLE_QTY_TextAlign;
            grd.Cols["INPUT_WGT"].TextAlign = cs.WGT_TextAlign;

            grd.Cols["SURFACE_LEVEL_NM"].TextAlign = cs.SURFACE_LEVEL_TextAlign;
            grd.Cols["FINISH_YN"].TextAlign = cs.FINISH_YN_TextAlign;
            grd.Cols["MILL_DATE"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["COMPANY_NM"].TextAlign = cs.COMPANY_NM_TextAlign;
            grd.Cols["USAGE_CD_NM"].TextAlign = cs.USAGE_CD_NM_TextAlign;
            grd.Cols["END_WGT"].TextAlign = cs.WGT_TextAlign;
            grd.Cols["POC_PROG_STAT_NM"].TextAlign = cs.POC_PROG_STAT_NM_TextAlign;

            grd.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grd.Cols["POC_PROG_STAT"].TextAlign = cs.POC_PROG_STAT_TextAlign;
            grd.Cols["LINE_GP"].TextAlign = cs.LINE_GP_TextAlign;
            grd.Cols["WORK_ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grd.Cols["WORK_RANK"].TextAlign = cs.WORK_RANK_TextAlign;
            grd.Cols["WORK_UOM"].TextAlign = 0;
            grd.Cols["POC_SEQ"].TextAlign = cs.POC_NO_TextAlign;
            //Button btn1 = new Button();
            //btn1.BackColor = SystemColors.Control;
            //btn1.Text = "선택";
            //btn1.Tag = "선택";
            //btn1.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //btn1.ForeColor = Color.Blue;

            //btn1.Click += Button_Click;

            //_al.Add(new HostedControl(grd, btn1, 0, grd.Cols["SEL"].Index));

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSel.Click += Button_Click;

            _al.Add(new CrtInOrdCre.HostedControl(grdSub, lbSel, 0, grd.Cols["SEL"].Index));

        }


        bool allChecked = false;
        private void Button_Click(object sender, EventArgs e)
        {
            //Button bt = (Button)sender;
            //MessageBox.Show("Clicked on row: " + bt.Tag);

            if (allChecked)
            {
                for (int rowCnt = 1; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, grdSub.Cols["SEL"].Index, false);
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, grdSub.Cols["SEL"].Index, true);
                }
                allChecked = true;

            }

            //grdSub_CellChecked(grdSub, null);
        }

        ArrayList _al = new ArrayList();

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


        private void InitGrd_grdMain()
        {
            cs.InitGrid_search(grdMain);

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["SEL"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //grdMain.DropMode = DropModeEnum.Automatic;

            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["SEL"].Width = cs.Sel_Width;
            grdMain.Cols["POC_PROG_STAT"].Width = 0;
            grdMain.Cols["POC_PROG_STAT_NM"].Width = cs.POC_PROG_STAT_NM_Width;

            grdMain.Cols["LINE_GP"].Width = 0;
            grdMain.Cols["LINE_GP_NM"].Width = 80;
            grdMain.Cols["WORK_ORD_DATE"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_RANK"].Width = cs.Work_Rank_Width;
            grdMain.Cols["ORG_WORK_RANK"].Width = 0;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width ;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width ;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_L_Width;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain.Cols["SURFACE_LEVEL_NM"].Width = cs.Surface_Level_NM_Width;
            
            grdMain.Cols["ORD_WGT"].Width = cs.Wgt_Width + 0;
            grdMain.Cols["INPUT_WGT"].Width = cs.Wgt_Width + 0;
            grdMain.Cols["END_WGT"].Width = cs.Wgt_Width + 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;

            grdMain.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain.Cols["POC_PROG_STAT_NM"].TextAlign = cs.POC_PROG_STAT_NM_TextAlign;

            grdMain.Cols["LINE_GP_NM"].TextAlign = cs.LINE_GP_TextAlign;
            grdMain.Cols["WORK_ORD_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_RANK"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["SURFACE_LEVEL_NM"].TextAlign = cs.SURFACE_LEVEL_TextAlign;
            
            grdMain.Cols["ORD_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["INPUT_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["END_WGT"].TextAlign = cs.WGT_TextAlign;


            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;
            //lbSel.Text = "선택";
            //lbSel.Tag = "선택";
            //lbSel.Font = new Font(cs.OHeadfontName, cs.OHeadfontSize, FontStyle.Bold);
            //lbSel.ForeColor = Color.Blue;

            lbSel.Click += LbSel_Click; 

            _al.Add(new CrtInOrdCre.HostedControl(grdMain, lbSel, 0, grdMain.Cols["SEL"].Index));
        }

        enum SelToggle
        {
            AllUnchecked
           ,WaitingChecked
           ,ConfirmChecked
        }
        SelToggle CheckState = SelToggle.AllUnchecked;
        SelToggle LastState = SelToggle.AllUnchecked;
        private void LbSel_Click(object sender, EventArgs e)
        {
            //전체 행을 unchecked 상태로 만들고
            // B: 확정대기, C: 확정
            // AllUnchecked, WaitingChecked, ConfirmChecked 상태를 루프를 돈다.
            // 단 해당 상태의 항목이 없는경우 건너뛴다.

            //OptionSel1();
            if (CheckState == SelToggle.AllUnchecked)
            {

                if (IsStatueRow(grdMain, "B", "POC_PROG_STAT"))
                {
                    //확정대기가 있으면..
                    CheckState = SelToggle.WaitingChecked;

                }
                else
                {

                    if (IsStatueRow(grdMain, "C", "POC_PROG_STAT"))
                    {
                        //확정이 있으면 확정을 체크
                        CheckState = SelToggle.ConfirmChecked;
                    }
                    else
                    {
                        //확정도 없으면 return
                        return;
                    }

                }

            }
            else if (CheckState == SelToggle.WaitingChecked)
            {
                if (IsStatueRow(grdMain, "C", "POC_PROG_STAT"))
                {
                    //확정이 있으면 확정을 체크
                    CheckState = SelToggle.ConfirmChecked;
                }
                else
                {
                    CheckState = SelToggle.AllUnchecked;
                }
            }
            else //if (CheckState == SelToggle.ConfirmChecked) // 확정상태인경우.
            {
                CheckState = SelToggle.AllUnchecked;

            }

            //if ((LastState == SelToggle.WaitingChecked ) || (LastState == SelToggle.ConfirmChecked ))
            //{
            //    CheckState = SelToggle.AllUnchecked;
            //}

            // 3가스 state를 이동하는 비트이동하는 방법을 찾아야함..

            switch (CheckState)
            {
                case SelToggle.AllUnchecked:
                    AllUnCheck();
                    LastState = SelToggle.AllUnchecked;
                    break;
                case SelToggle.WaitingChecked:
                    WaitingChecked();
                    LastState = SelToggle.WaitingChecked;
                    break;
                case SelToggle.ConfirmChecked:
                    ConfirmChecked();
                    LastState = SelToggle.ConfirmChecked;
                    break;
                default:
                    AllUnCheck();
                    break;
            }

        }
        
        private void OptionSel()
        {
            if (LastState == SelToggle.WaitingChecked)
            {
                CheckState = SelToggle.ConfirmChecked;
            }
            else if (LastState == SelToggle.ConfirmChecked)
            {
                CheckState = SelToggle.WaitingChecked;
            }
            else
            {
                CheckState = SelToggle.ConfirmChecked;
            }
        }

        private void OptionSel1()
        {

            if (LastState == SelToggle.AllUnchecked)
            {
                CheckState = SelToggle.WaitingChecked;
            }
            else if (LastState == SelToggle.WaitingChecked)
            {
                CheckState = SelToggle.ConfirmChecked;
            }
            else if (LastState == SelToggle.ConfirmChecked)
            {
                CheckState = SelToggle.AllUnchecked;
            }
        }


        private void ConfirmChecked()
        {
            AllUnCheck();
            CheckSel(grdMain, true, "C", "POC_PROG_STAT", "SEL");
        }

        private void WaitingChecked()
        {
            AllUnCheck();
            CheckSel(grdMain, true, "B", "POC_PROG_STAT", "SEL");
        }

        private void AllUnCheck()
        {
            CheckSel(grdMain, false, "SEL");
        }

        private bool IsStatueRow(C1FlexGrid grd, string _status, string _columnName)
        {
            bool _IsStatusRow = false;

            for (int row = 1; row < grd.Rows.Count; row++)
            {
                if (grd.GetData(row, _columnName).ToString() == _status)
                {
                    _IsStatusRow = true;
                    break;
                }
            }

            return _IsStatusRow;
        }


        private void CheckSel(C1FlexGrid grd, bool checkStat, string _status, string _columnName, string _checkColumnName)
        {
            for (int row = 1; row < grd.Rows.Count; row++)
            {
                if (grd.GetData(row, _columnName).ToString() == _status)
                {
                    grd.SetData(row, _checkColumnName, checkStat);
                }
            }
        }
        private void CheckSel(C1FlexGrid grd, bool checkStat, string _checkColumnName)
        {
            for (int row = 1; row < grd.Rows.Count; row++)
            {
                grd.SetData(row, _checkColumnName, checkStat);
            }
        }


        private void grdMain_Click(object sender, EventArgs e)
        {
            //if (grdMain.ColSel == grdMain.Cols["SEL"].Index)
            //{
            //    return;
            //}
            //else
            //{
            //    C1FlexGrid _selectedGrd = sender as C1FlexGrid;

            //    selectedGrd = _selectedGrd;

            //    SelectedGrdMain(selectedGrd, selectedGrd.RowSel);
            //}

            C1FlexGrid _selectedGrd = sender as C1FlexGrid;

            selectedGrd = _selectedGrd;

            SelectedGrdMain(selectedGrd, selectedGrd.RowSel);


        }

        private void SelectedGrdMain(C1FlexGrid grd, int selected_Row)
        {
            if (grd.RowSel <= 0 || grd.Rows.Count <= 1)
            {
                return;
            }


            // subgrd data search
            string _line_gp = grd.GetData(selected_Row, "LINE_GP").ToString();
            string _work_date = vf.Format(grd.GetData(selected_Row, "WORK_ORD_DATE"), "yyyyMMdd");
            //string _work_rank = grd.GetData(selected_Row, "WORK_RANK").ToString();
            string _work_rank = grd.GetData(selected_Row, "ORG_WORK_RANK").ToString();
            string _item_size = grd.GetData(selected_Row, "ITEM_SIZE").ToString();
            string _steel = grd.GetData(selected_Row, "STEEL").ToString();
            string _length = grd.GetData(selected_Row, "LENGTH").ToString();
            //string 

            InitGridData(false);
            SetDataBindingGrdSub(_line_gp, _work_date, _work_rank, _item_size, _steel, _length);
        }

        private void SetDataBindingGrdSub(string _line_gp, string _work_date,  string _work_rank, string _item_size, string _steel, string _length)
        {
            
            string sql1 = string.Empty;
            sql1 += string.Format("SELECT ROWNUM AS L_NO ");
            sql1 += string.Format("       ,X.* ");
            sql1 += string.Format("FROM   ( ");
            sql1 += string.Format("         SELECT  POC_NO ");
            sql1 += string.Format("                ,HEAT ");
            sql1 += string.Format("                ,'False' AS SEL ");
            sql1 += string.Format("                ,STEEL ");
            sql1 += string.Format("                ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
            sql1 += string.Format("                ,ITEM ");
            sql1 += string.Format("                ,ITEM_SIZE ");
            sql1 += string.Format("                ,LENGTH ");
            sql1 += string.Format("                ,BUNDLE_QTY ");
            sql1 += string.Format("                ,ORD_WGT ");
            sql1 += string.Format("                ,(SELECT COUNT(*) FROM TB_CR_INPUT_WR WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ) AS INPUT_BUNDLE_QTY"); // --투입번들수
            sql1 += string.Format("                ,(SELECT SUM(NET_WGT) FROM TB_CR_INPUT_WR WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ) AS INPUT_WGT "); //   --투입중량
            sql1 += string.Format("                ,SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = A.SURFACE_LEVEL) AS SURFACE_LEVEL_NM ");
            sql1 += string.Format("                ,TO_DATE(MILL_DATE, 'yyyy-MM-dd') AS  MILL_DATE");
            sql1 += string.Format("                ,NVL(A.FINISH_YN, 'N') AS  FINISH_YN");
            sql1 += string.Format("                ,COMPANY_NM ");
            sql1 += string.Format("                ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'USAGE_CD' AND CD_ID = A.USAGE_CD) AS USAGE_CD_NM ");
            sql1 += string.Format("                ,(SELECT SUM(NET_WGT) FROM TB_BND_WR WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ AND NVL(DEL_YN,'N') <> 'Y') AS END_WGT ");
            sql1 += string.Format("                ,POC_PROG_STAT ");
            sql1 += string.Format("                ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'POC_PROG_STAT' AND CD_ID = A.POC_PROG_STAT) AS POC_PROG_STAT_NM ");
            sql1 += string.Format("                ,LINE_GP ");
            sql1 += string.Format("                ,WORK_ORD_DATE ");
            sql1 += string.Format("                ,WORK_RANK ");
            sql1 += string.Format("                ,WORK_UOM ");
            sql1 += string.Format("                ,POC_SEQ ");
            sql1 += string.Format("         FROM   TB_CR_INPUT_ORD A ");
            sql1 += string.Format("         WHERE  LINE_GP       = :P_LINE_GP ");
            sql1 += string.Format("         AND    WORK_ORD_DATE = :P_WORK_ORD_DATE ");
            sql1 += string.Format("         AND    WORK_RANK     = :P_WORK_RANK ");
            sql1 += string.Format("         AND    ITEM_SIZE     = :P_ITEM_SIZE ");
            sql1 += string.Format("         AND    STEEL         = :P_STEEL ");
            sql1 += string.Format("         AND    LENGTH        = :P_LENGTH ");
            sql1 += string.Format("         AND    POC_PROG_STAT > 'A'   ");//--확정대기부터 조회
            sql1 += string.Format("         ORDER BY 1,2,3,4,5,6,7 ");
            sql1 += string.Format("        ) X ");

            string[] parm = new string[6];
            parm[0] = ":P_LINE_GP|" + ling_gp;
            parm[1] = ":P_WORK_ORD_DATE|" + _work_date; //+ vf.Format(start_date, "yyyyMMdd");
            parm[2] = ":P_WORK_RANK|" + _work_rank;// + vf.Format(end_date, "yyyyMMdd");
            parm[3] = ":P_ITEM_SIZE|" + _item_size;// + itemsize;
            parm[4] = ":P_STEEL|" + _steel;// + itemsize;
            parm[5] = ":P_LENGTH|" + _length;//+ itemsize;


            olddt_sub = cd.FindDataTable(sql1, parm);

            moddt_sub = olddt_sub.Copy();

            //if (moddt_sub.Rows.Count > 1)
            //{
            //    Cursor = Cursors.AppStarting;
            //    grdSub.SetDataBinding(moddt_sub, null, true);
            //    Cursor = Cursors.Default;
            //    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt_sub.Rows.Count.ToString() + " 건 조회 되었습니다.");
            //}
            Cursor = Cursors.AppStarting;
            grdSub.SetDataBinding(moddt_sub, null, true);
            Cursor = Cursors.Default;
            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt_sub.Rows.Count.ToString() + " 건 조회 되었습니다.");
        }

        private void grdSub_Click(object sender, EventArgs e)
        {
            C1FlexGrid _selectedGrd = sender as C1FlexGrid;

            selectedGrd = _selectedGrd;
        }

        private void cbLine_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ling_gp = ((ComLib.DictionaryList)cbLine_gp.SelectedItem).fnValue;
            ck.Line_gp = ling_gp;
        }

        private void tbItemSize_TextChanged(object sender, EventArgs e)
        {
            itemsize = tbItemSize.Text;
        }



        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_date = start_dt.Value;
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            end_date = end_dt.Value;
        }

        private void grdSub_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            if ( e.Col != grd.Cols["SEL"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
            }

        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            bool isChecked = false;
            for (int row = 1; row < grdSub.Rows.Count; row++)
            {
                isChecked = (grdSub.GetData(row, "SEL").ToString() == "True") ? true : isChecked;

            }

            if (isChecked)
            {
                if (MessageBox.Show("선택된 항목을 지시취소하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }
            else
            {
                return;
            }

            string ling_gp = "";
            string poc_No = "";
            int poc_seq = 0;
            string work_rank = "";
            string work_ord_date = "";
            string work_uom = "";
            string user_id = ck.UserID;
            int proc_stat = 4000;
            string result_stat = "";
            string result_msg = "";

            //isChecked = false;

            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;
            string spName = "SP_CR_INPUT_ORD_CAN";  // 프로시저명
            try
            {
                oConn.Open();
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                transaction = oConn.BeginTransaction();
                cmd.Transaction = transaction;
                OracleParameter op;
                for (int row = 1; row < grdSub.Rows.Count; row++)
                {
                   
                    if (grdSub.GetData(row, "SEL").ToString() == "False")
                    {
                        continue;
                    }

                    //op = null;
                    cmd.Parameters.Clear();

                    ling_gp = grdSub.GetData(row, "LINE_GP").ToString() ;
                    poc_No = grdSub.GetData(row, "POC_NO").ToString();
                    poc_seq = vf.CInt2(grdSub.GetData(row, "POC_SEQ").ToString());
                    work_rank = grdSub.GetData(row, "WORK_RANK").ToString();
                    work_ord_date = grdSub.GetData(row, "WORK_ORD_DATE").ToString();
                    work_uom = grdSub.GetData(row, "WORK_UOM").ToString();
                    user_id = ck.UserID;
                    proc_stat = 4000;

                    op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ling_gp;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = poc_No;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_POC_SEQ", OracleType.Int16);
                    op.Direction = ParameterDirection.Input;
                    op.Value = poc_seq;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_WORK_RANK", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = work_rank;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_WORK_ORD_DATE", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = work_ord_date;// vf.Format(work_ord_date, "yyyyMMdd");//  work_ord_date.ToString("yyyyMMdd");
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_WORK_UOM", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = work_uom;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ck.UserID;    // 사용자 id 가져와서 입력
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = proc_stat;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = 4000;
                    cmd.Parameters.Add(op);

                    //oConn.Open();
                    //transaction = cmd.Connection.BeginTransaction();
                    //cmd.Transaction = transaction;

                    //cmd.ExecuteOracleScalar();
                    cmd.ExecuteNonQuery();

                    result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                    result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                    if (result_stat == "ERR")
                    {
                        MessageBox.Show(result_msg);
                        break;
                    }
                }
                

                transaction.Commit();

                //MessageBox.Show(result_msg);
                btnDisplay_Click(null,null);
                
                //string message = "정상적으로 저장되었습니다.";
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
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
                return ;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();

                //this.Close();
            }

            return ;
        }

        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjong_Nm_tb.Text = "";
            gangjong_id = gangjong_id_tb.Text;
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        private void btnSteel_Click(object sender, EventArgs e)
        {
            Popup.SearchSteelNm popup = new Popup.SearchSteelNm();
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            //선택된 행이 없거나 0 이하이거나 하면 리턴시킨다.
            if (grdMain.RowSel < 1)
            {
                return;
            }


            //선택된 그리드 row와 일치하는 datatable에서의 row
            int dt_Row = grdMain.Row - 1;

            //움직이기전 선택된 그리드 row
            int pre_grd_Row = grdMain.Row;
            int after_grd_Row = pre_grd_Row - 1;


            if (pre_grd_Row <= 1)
            {
                return;
            }

            #region 날짜 체크
            // 이동하기 위해서는 투입지시일자가 일치하는지 확인해서 일치하면 이동 다르면 이동하지않음
            string frm_date = vf.Format(grdMain.GetData(pre_grd_Row, "WORK_ORD_DATE"), "yyyyMMdd");
            string to_date = vf.Format(grdMain.GetData(after_grd_Row, "WORK_ORD_DATE"), "yyyyMMdd");

            if (frm_date != to_date)
            {
                result_msg = "날짜간 이동은 불가합니다.";

                //MessageBox.Show(result_msg);
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
                return;
            }
            #endregion

            //#region POC 상태 체크
            ////string fr_confirm_state = grdMain.GetData(pre_grd_Row, "POC_PROG_STAT").ToString();
            ////string to_confirm_state = grdMain.GetData(after_grd_Row, "POC_PROG_STAT").ToString();

            ////A: 편성대기, B: 확정대기, C:확정, D: 정정시작, E:정정종료
            ////A,B,C,D 의 poc 상태를 숫자로 비교해서 B보다 높은 상태와 이동불가하게 하기위해 비교
            //int fr_confirm_state = (int)grdMain.GetData(pre_grd_Row, "POC_PROG_STAT").ToString()[0];
            //int to_confirm_state = (int)grdMain.GetData(after_grd_Row, "POC_PROG_STAT").ToString()[0];

            ////B: 확정대기
            //int levelB = (int)'B';



            //if (to_confirm_state > levelB || fr_confirm_state > levelB)
            //{
            //    result_msg = " POC상태가 확정이상의 상태간 이동은 불가합니다.";
            //    //MessageBox.Show(result_msg);
            //    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
            //    return;
            //}
            //#endregion


            MoveRow(moddt, dt_Row, dt_Row - 1);

            //수정마킹하기
            grdMain.SetData(pre_grd_Row, "L_NO", "수정");
            grdMain.SetData(after_grd_Row, "L_NO", "수정");

            //grdMain.SetData(pre_grd_Row, "GUBUN", "수정");
            //grdMain.SetData(after_grd_Row, "GUBUN", "수정");

            // Update 배경색 지정
            grdMain.Rows[pre_grd_Row].Style = grdMain.Styles["UpColor"];
            grdMain.Rows[after_grd_Row].Style = grdMain.Styles["UpColor"];

            grdMain.Row = after_grd_Row;

            // 수정된 row L_NO 수정으로 표시


        }



        private void btnDown_Click(object sender, EventArgs e)
        {
            //선택된 행이 없거나 0 이하이거나 하면 리턴시킨다.
            if (grdMain.RowSel < 0)
            {
                return;
            }

            //선택된 그리드 row와 일치하는 datatable에서의 row
            int dt_Row = grdMain.Row - 1;
            //움직이기전 선택된 그리드 row
            int pre_grd_Row = grdMain.Row;
            int after_grd_Row = pre_grd_Row + 1;


            if (dt_Row >= moddt.Rows.Count - 1)
            {
                return;
            }

            #region 날짜 체크
            // 이동하기 위해서는 투입지시일자가 일치하는지 확인해서 일치하면 이동 다르면 이동하지않음
            string frm_date = vf.Format(grdMain.GetData(pre_grd_Row, "WORK_ORD_DATE"), "yyyyMMdd");
            string to_date = vf.Format(grdMain.GetData(pre_grd_Row + 1, "WORK_ORD_DATE"), "yyyyMMdd");

            if (frm_date != to_date)
            {
                result_msg = "날짜간 이동은 불가합니다.";
                //MessageBox.Show(result_msg);
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
                return;
            }
            #endregion

            //#region POC 상태 체크
            ////string fr_confirm_state = grdMain.GetData(pre_grd_Row, "POC_PROG_STAT_NM").ToString();
            ////string to_confirm_state = grdMain.GetData(after_grd_Row, "POC_PROG_STAT_NM").ToString();

            ////A: 편성대기, B: 확정대기, C:확정, D: 정정시작, E:정정종료
            ////A,B,C,D 의 poc 상태를 숫자로 비교해서 B보다 높은 상태와 이동불가하게 하기위해 비교
            //int fr_confirm_state = (int)grdMain.GetData(pre_grd_Row, "POC_PROG_STAT").ToString()[0];
            //int to_confirm_state = (int)grdMain.GetData(after_grd_Row, "POC_PROG_STAT").ToString()[0];

            ////B: 확정대기
            //int levelB = (int)'B';

            //if (to_confirm_state > levelB || fr_confirm_state > levelB)
            //{
            //    result_msg = " POC상태가 확정이상의 상태간 이동은 불가합니다.";
            //    //MessageBox.Show(result_msg);
            //    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
            //    return;
            //}
            //#endregion


            //if (to_confirm_state == "확정" || fr_confirm_state == "확정")
            //{
            //    result_msg = "확정된 항목간 이동은 불가합니다.";
            //    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
            //    return;
            //}

            MoveRow(moddt, dt_Row, dt_Row + 1);

            //수정마킹하기
            grdMain.SetData(pre_grd_Row, "L_NO", "수정");
            grdMain.SetData(after_grd_Row, "L_NO", "수정");

            //grdMain.SetData(pre_grd_Row, "GUBUN", "수정");
            //grdMain.SetData(after_grd_Row, "GUBUN", "수정");

            // Update 배경색 지정
            grdMain.Rows[pre_grd_Row].Style = grdMain.Styles["UpColor"];
            grdMain.Rows[after_grd_Row].Style = grdMain.Styles["UpColor"];

            grdMain.Row = after_grd_Row;
        }

        private void MoveRow(DataTable dt, int rowSel, int moveIndex)
        {
            int grd_sourcRow = rowSel + 1;
            int grd_destRow = moveIndex + 1;

            //DataRow newRow = moddt.NewRow();
            DataRow sourceRow = dt.Rows[rowSel];
            DataRow destRow = dt.Rows[moveIndex];

            List<string> list = new List<string>();
            foreach (var item in dt.Rows[moveIndex].ItemArray)
            {
                list.Add(item.ToString());
            }

            //foreach (DataColumn col in dt.Rows)
            //{

            //}

            //이동시에 NO에 수정을 표시
            //         수정된 작업순위는 저장시에 순서대로 마킹해서 테이블에 저장하고 SP 호출하고 마무리.



            //foreach (var item in sourceRow.ItemArray)
            //{
            //    grdMain.SetData(grd_destRow, item, col);
            //}

            for (int itemindex = 0; itemindex < sourceRow.ItemArray.Count(); itemindex++)
            {
                if (itemindex == grdMain.Cols["L_NO"].Index || itemindex == grdMain.Cols["LINE_GP"].Index || itemindex == grdMain.Cols["WORK_ORD_DATE"].Index || itemindex == grdMain.Cols["WORK_RANK"].Index)
                {
                    continue;
                }

                grdMain.SetData(grd_destRow, itemindex, sourceRow[itemindex]);
            }

            for (int itemindex = 0; itemindex < list.Count(); itemindex++)
            {
                if (itemindex == grdMain.Cols["L_NO"].Index || itemindex == grdMain.Cols["LINE_GP"].Index || itemindex == grdMain.Cols["WORK_ORD_DATE"].Index || itemindex == grdMain.Cols["WORK_RANK"].Index)
                {
                    continue;
                }

                grdMain.SetData(grd_sourcRow, itemindex, list[itemindex]);
            }

            //foreach (var item in sourceRow.ItemArray)
            //{


            //    //if (col.ColumnName == "L_NO" || col.ColumnName == "LINE_GP" || col.ColumnName == "WORK_ORD_DATE" || col.ColumnName == "WORK_RANK")
            //    //{
            //    //    continue;
            //    //}
            //    //grdMain.SetData(grd_destRow, col.ColumnName, col);
            //}

            //foreach (DataColumn col in destRow.ItemArray)
            //{
            //    if (col.ColumnName == "L_NO" || col.ColumnName == "LINE_GP" || col.ColumnName == "WORK_ORD_DATE" || col.ColumnName == "WORK_RANK")
            //    {
            //        continue;
            //    }
            //    grdMain.SetData(grd_destRow, col.ColumnName, col);
            //}


            //newRow.ItemArray = oldRow.ItemArray;



            //moddt.Rows.InsertAt(newRow, moveIndex);
            //moddt.Rows.Remove(oldRow);


        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 수정사항 체크
            bool IsChange = false;
            string modified = "";
            int checkrow = 0;
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                modified = grdMain.GetData(checkrow, "L_NO").ToString();

                if (modified == "수정")
                {
                    IsChange = true;
                    break;
                }
            }

            if (IsChange)
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

            #region 수정된 해당 투입지시일자에 관련된 사항들을 테이블에 insert 시키고 procedure call 
            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string check_date = "";
            string last_date = "";
            int work_rank = 0;
            string sql1 = string.Empty;
            string strLog = string.Empty;
            var logList = new List<LogDataList>();
            var itemList = new List<DictionaryList>();
            string spName = "SP_CR_ORD_RANK_MOD";
            OracleParameter op;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                
                //수정된 데이터 인서트 
                #region 수정된 테이블 인서트

                for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
                {

                    modified = grdMain.GetData(checkrow, "L_NO").ToString();

                    if (modified == "수정")
                    {
                        //check_date = vf.Format(grdMain.GetData(checkrow, "WORK_ORD_DATE"), "yyyyMMdd");
                        // 날짜가 달라지면 work rank 초기화시켜줌
                        //if (last_date != check_date)
                        //{
                        //    work_rank = 1;
                        //}

                        strLog = string.Empty;

                        sql1 = string.Empty;
                        sql1 += string.Format(" INSERT INTO TB_CR_ORD_RANK_MOD ");
                        sql1 += string.Format("            ( ");
                        sql1 += string.Format("                 REGISTER ");
                        sql1 += string.Format("                ,LINE_GP ");
                        sql1 += string.Format("                ,WORK_ORD_DATE ");
                        sql1 += string.Format("                ,WORK_RANK ");
                        sql1 += string.Format("                ,ORG_WORK_RANK ");
                        sql1 += string.Format("                ,ITEM_SIZE ");
                        sql1 += string.Format("                ,STEEL ");
                        sql1 += string.Format("                ,LENGTH ");
                        sql1 += string.Format("                ,REG_DDTT ");
                        sql1 += string.Format("                ,PROC_YN ");
                        sql1 += string.Format("            ) ");
                        sql1 += string.Format(" VALUES ");
                        sql1 += string.Format("        ( ");
                        sql1 += string.Format("          '{0}' ", ck.UserID);       //REGISTER ");
                        sql1 += string.Format("         ,'{0}' ", grdMain.GetData(checkrow, "LINE_GP").ToString());        //LINE_GP ");
                        sql1 += string.Format("         ,'{0}' ", vf.Format(grdMain.GetData(checkrow, "WORK_ORD_DATE"), "yyyyMMdd"));  //WORK_ORD_DATE ");
                        sql1 += string.Format("         ,'{0}' ", grdMain.GetData(checkrow, "WORK_RANK").ToString());      //WORK_RANK ");
                        sql1 += string.Format("         ,'{0}' ", grdMain.GetData(checkrow, "ORG_WORK_RANK").ToString());  //ORG_WORK_RANK ");
                        sql1 += string.Format("         ,'{0}' ", grdMain.GetData(checkrow, "ITEM_SIZE").ToString());      //ITEM_SIZE ");
                        sql1 += string.Format("         ,'{0}' ", grdMain.GetData(checkrow, "STEEL").ToString());         //STEEL ");
                        sql1 += string.Format("         , {0} ", vf.VAL(grdMain.GetData(checkrow, "LENGTH").ToString()));      //LENGTH ");
                        sql1 += string.Format("         ,SYSDATE ");       //REG_DDTT ");
                        sql1 += string.Format("         ,'{0}' ","N");        //PROC_YN ");
                        sql1 += string.Format("        ) ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        itemList.Add(new DictionaryList("REGISTER", ck.UserID));
                        itemList.Add(new DictionaryList("LINE_GP", grdMain.GetData(checkrow, "LINE_GP").ToString()));
                        itemList.Add(new DictionaryList("WORK_ORD_DATE", vf.Format(grdMain.GetData(checkrow, "WORK_ORD_DATE"), "yyyyMMdd")));
                        itemList.Add(new DictionaryList("WORK_RANK", grdMain.GetData(checkrow, "WORK_RANK").ToString()));
                        itemList.Add(new DictionaryList("ORG_WORK_RANK", grdMain.GetData(checkrow, "ORG_WORK_RANK").ToString()));
                        itemList.Add(new DictionaryList("ITEM_SIZE", grdMain.GetData(checkrow, "ITEM_SIZE").ToString()));
                        itemList.Add(new DictionaryList("STEEL", grdMain.GetData(checkrow, "STEEL").ToString()));
                        itemList.Add(new DictionaryList("LENGTH", vf.VAL(grdMain.GetData(checkrow, "LENGTH").ToString()).ToString()));

                        CrtInOrdCre.LogStrCreate(itemList, ref strLog);

                        logList.Add(new LogDataList(Alarms.InSerted, Text, strLog));

                        last_date = check_date;
                        work_rank++;
                    }
                }

                #endregion

                #region "SP_CR_ORD_RANK_MOD" sp를 실행시켜 변경된 work_rank가 적용되게 함.
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
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

                if (result_stat == "ERR")
                {
                    MessageBox.Show(result_msg);

                    string _msg = string.Format(" SPNAME: {0}, USER_ID: {1}, ERROR_MSG: {2}", spName, ck.UserID, result_msg);

                    clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), _msg);
                }
                else if (result_stat == "OK")
                {
                    btnDisplay_Click(null, null);

                    // 성공시에 추가 수정 삭제 상황을 초기화시킴
                    //InitGrd_Main();

                    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);

                    foreach (var log in logList)
                    {
                        clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                    }

                    string __msg = string.Format(" SPNAME: {0}, USER_ID: {1} ",spName,ck.UserID);

                    clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), __msg);
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
                //MessageBox.Show(ex.Message);
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

        private void grdSub_Paint(object sender, PaintEventArgs e)
        {
            foreach (CrtInOrdCre.HostedControl hosted in _al)
                hosted.UpdatePosition();
        }

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;
            if (e.Col != grd.Cols["SEL"].Index )  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
            }
            //else
            //{
            //    e.Cancel = true;
            //}
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            #region 수정사항 체크
            bool IsChange = false;
            string modified = "";
            int checkrow = 0;
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                modified = grdMain.GetData(checkrow, "SEL").ToString();


                if (modified == "True")
                {
                    if (grdMain.GetData(checkrow, "POC_PROG_STAT_NM").ToString() == "확정대기")
                    {
                        IsChange = true;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("상태가 확정대기이어야 확정가능합니다.");
                        return;
                    }

                    
                }
            }

            if (IsChange)
            {
                if (MessageBox.Show("선택된 항목을 확정하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            #endregion

            #region 수정된 해당 투입지시일자에 관련된 사항들을 테이블에 insert 시키고 procedure call 
            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string check_date = "";
            string last_date = "";
            int work_rank = 0;
            string sql1 = string.Empty;
            string proc_gp = "REG";
            var logList = new List<LogDataList>();

            string spName = "SP_CR_INPUT_ORD_CMF";
            OracleParameter op;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                //수정된 데이터 인서트 
                #region 수정된 테이블 인서트

                for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
                {

                    modified = grdMain.GetData(checkrow, "SEL").ToString();

                    if (modified == "True")
                    {
                        check_date = vf.Format(grdMain.GetData(checkrow, "WORK_ORD_DATE"), "yyyyMMdd");
                        // 날짜가 달라지면 work rank 초기화시켜줌
                        if (last_date != check_date)
                        {
                            work_rank = 1;
                        }
                        sql1 = string.Empty;
                        sql1 += string.Format("INSERT INTO TB_CR_ORD_RANK_MOD ");
                        sql1 += string.Format("( ");
                        sql1 += string.Format("    REGISTER ");
                        sql1 += string.Format("    ,LINE_GP ");
                        sql1 += string.Format("    ,WORK_ORD_DATE ");
                        sql1 += string.Format("    ,WORK_RANK ");
                        sql1 += string.Format("    ,ORG_WORK_RANK ");
                        sql1 += string.Format("    ,ITEM_SIZE ");
                        sql1 += string.Format("    ,STEEL ");
                        sql1 += string.Format("    ,LENGTH ");
                        sql1 += string.Format("    ,REG_DDTT ");
                        sql1 += string.Format("    ,PROC_YN ");
                        sql1 += string.Format(") ");
                        sql1 += string.Format("VALUES ");
                        sql1 += string.Format("( ");
                        sql1 += string.Format("     '{0}' ", ck.UserID);       //REGISTER ");
                        sql1 += string.Format("    ,'{0}' ", grdMain.GetData(checkrow, "LINE_GP").ToString());        //LINE_GP ");
                        sql1 += string.Format("    ,'{0}' ", vf.Format(grdMain.GetData(checkrow, "WORK_ORD_DATE"), "yyyyMMdd"));  //WORK_ORD_DATE ");
                        sql1 += string.Format("    , {0} ", work_rank);      //WORK_RANK ");
                        sql1 += string.Format("    , {0} ", grdMain.GetData(checkrow, "ORG_WORK_RANK").ToString());  //ORG_WORK_RANK ");
                        sql1 += string.Format("    ,'{0}' ", grdMain.GetData(checkrow, "ITEM_SIZE").ToString());      //ITEM_SIZE ");
                        sql1 += string.Format("    ,'{0}' ", grdMain.GetData(checkrow, "STEEL").ToString());         //STEEL ");
                        sql1 += string.Format("    , {0} ", vf.VAL(grdMain.GetData(checkrow, "LENGTH").ToString()));      //LENGTH ");
                        sql1 += string.Format("    ,SYSDATE ");       //REG_DDTT ");
                        sql1 += string.Format("    ,'{0}' ", "N");        //PROC_YN ");
                        sql1 += string.Format(") ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        logList.Add(new LogDataList(Alarms.InSerted, Text, sql1));

                        last_date = check_date;
                        work_rank++;
                    }
                }

                #endregion

                #region "SP_CR_INPUT_ORD_CMF" sp를 실행시켜 변경된 work_rank가 확정되게 함.
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();

                op = new OracleParameter("P_PROC_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = proc_gp;
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

                if (result_stat == "ERR")
                {
                    MessageBox.Show(result_msg);

                    string _msg = string.Format(" SPNAME: {0}, PROC_GP: {3} USER_ID: {1}, ERROR_MSG: {2}", spName, ck.UserID, result_msg, proc_gp);

                    clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), _msg);
                }
                else if (result_stat == "OK")
                {
                    btnDisplay_Click(null, null);

                    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);

                    foreach (var log in logList)
                    {
                        clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                    }

                    string __msg = string.Format(" SPNAME: {0}, PROC_GP: {2}, USER_ID: {1} ", spName,ck.UserID, proc_gp);

                    clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), __msg);
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

        private void btnApplyCancel_Click(object sender, EventArgs e)
        {
            #region 수정사항 체크
            bool IsChange = false;
            string modified = "";
            int checkrow = 0;
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                modified = grdMain.GetData(checkrow, "SEL").ToString();

                if (modified == "True")
                {

                    if (grdMain.GetData(checkrow, "POC_PROG_STAT_NM").ToString() == "확정")
                    {
                        IsChange = true;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("상태가 확정이어야 확정취소가능합니다.");
                        return;
                    }

                }
            }

            if (IsChange)
            {
                if (MessageBox.Show("선택된 항목을 확정취소하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            #endregion

            #region 수정된 해당 투입지시일자에 관련된 사항들을 테이블에 insert 시키고 procedure call 
            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string check_date = "";
            string last_date = "";
            int work_rank = 0;
            string sql1 = string.Empty;
            string proc_gp = "CAN";
            var logList = new List<LogDataList>();

            string spName = "SP_CR_INPUT_ORD_CMF";
            OracleParameter op;
            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                //수정된 데이터 인서트 
                #region 수정된 테이블 인서트

                for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
                {

                    modified = grdMain.GetData(checkrow, "SEL").ToString();

                    if (modified == "True")
                    {
                        check_date = vf.Format(grdMain.GetData(checkrow, "WORK_ORD_DATE"), "yyyyMMdd");
                        // 날짜가 달라지면 work rank 초기화시켜줌
                        if (last_date != check_date)
                        {
                            work_rank = 1;
                        }
                        sql1 = string.Empty;
                        sql1 += string.Format("INSERT INTO TB_CR_ORD_RANK_MOD ");
                        sql1 += string.Format("( ");
                        sql1 += string.Format("    REGISTER ");
                        sql1 += string.Format("    ,LINE_GP ");
                        sql1 += string.Format("    ,WORK_ORD_DATE ");
                        sql1 += string.Format("    ,WORK_RANK ");
                        sql1 += string.Format("    ,ORG_WORK_RANK ");
                        sql1 += string.Format("    ,ITEM_SIZE ");
                        sql1 += string.Format("    ,STEEL ");
                        sql1 += string.Format("    ,LENGTH ");
                        sql1 += string.Format("    ,REG_DDTT ");
                        sql1 += string.Format("    ,PROC_YN ");
                        sql1 += string.Format(") ");
                        sql1 += string.Format("VALUES ");
                        sql1 += string.Format("( ");
                        sql1 += string.Format("     '{0}' ", ck.UserID);       //REGISTER ");
                        sql1 += string.Format("    ,'{0}' ", grdMain.GetData(checkrow, "LINE_GP").ToString());        //LINE_GP ");
                        sql1 += string.Format("    ,'{0}' ", vf.Format(grdMain.GetData(checkrow, "WORK_ORD_DATE"), "yyyyMMdd"));  //WORK_ORD_DATE ");
                        sql1 += string.Format("    , {0} ", work_rank);      //WORK_RANK ");
                        sql1 += string.Format("    , {0} ", grdMain.GetData(checkrow, "ORG_WORK_RANK").ToString());  //ORG_WORK_RANK ");
                        sql1 += string.Format("    ,'{0}' ", grdMain.GetData(checkrow, "ITEM_SIZE").ToString());      //ITEM_SIZE ");
                        sql1 += string.Format("    ,'{0}' ", grdMain.GetData(checkrow, "STEEL").ToString());         //STEEL ");
                        sql1 += string.Format("    , {0} ", vf.VAL(grdMain.GetData(checkrow, "LENGTH").ToString()));      //LENGTH ");
                        sql1 += string.Format("    ,SYSDATE ");       //REG_DDTT ");
                        sql1 += string.Format("    ,'{0}' ", "N");        //PROC_YN ");
                        sql1 += string.Format(") ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        logList.Add(new LogDataList(Alarms.InSerted, Text, sql1));

                        last_date = check_date;
                        work_rank++;
                    }
                }

                #endregion

                #region "SP_CR_INPUT_ORD_CMF" sp를 실행시켜 변경된 work_rank가 확정되게 함.
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();

                op = new OracleParameter("P_PROC_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = proc_gp;
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

                if (result_stat == "ERR")
                {
                    MessageBox.Show(result_msg);

                    string _msg = string.Format(" SPNAME: {0}, PROC_GP: {3} USER_ID: {1}, ERROR_MSG: {2}", spName, ck.UserID, result_msg, proc_gp);

                    clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), _msg);
                }
                else if (result_stat == "OK")
                {
                    btnDisplay_Click(null, null);

                    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);

                    foreach (var log in logList)
                    {
                        clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                    }

                    string __msg = string.Format(" SPNAME: {0}, PROC_GP: {2}, USER_ID: {1} ", spName,ck.UserID, proc_gp);

                    clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), __msg);
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

        private void grdMain_CellChecked(object sender, RowColEventArgs e)
        {

        }

        private void grdSub_DoubleClick(object sender, EventArgs e)
        {
            if (grdSub.Row < 1)
            {
                return;
            }

            //선택된 행의 POC 와 POC_SEQ 데이터를 전달해서 해당정보를 조회하는 창 OPEN
            string poc_No = grdSub.GetData(grdSub.Row, "POC_NO").ToString().Trim();
            string poc_seq_No = grdSub.GetData(grdSub.Row, "POC_SEQ").ToString().Trim();



            CrtInBundleInfo popup = new CrtInBundleInfo(poc_No, poc_seq_No);
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.MinimizeBox = false;
            popup.MaximizeBox = false;
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
        }

        private void btnHeat_Click(object sender, EventArgs e)
        {
            string sql1 = string.Empty;
            sql1 += string.Format("SELECT TO_CHAR(ROWNUM) AS L_NO ");
            sql1 += string.Format("       ,'False' AS SEL ");
            sql1 += string.Format("       ,POC_PROG_STAT ");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'POC_PROG_STAT' AND CD_ID = X.POC_PROG_STAT) AS POC_PROG_STAT_NM ");
            sql1 += string.Format("       ,LINE_GP ");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'LINE_GP' AND CD_ID = X.LINE_GP) AS LINE_GP_NM ");
            //sql1 += string.Format("       ,WORK_ORD_DATE ");
            sql1 += string.Format("       ,TO_DATE(WORK_ORD_DATE, 'YYYY-MM-DD') AS WORK_ORD_DATE ");
            sql1 += string.Format("       ,WORK_RANK ");
            sql1 += string.Format("       ,WORK_RANK AS ORG_WORK_RANK ");
            sql1 += string.Format("       ,ITEM_SIZE ");
            sql1 += string.Format("       ,STEEL ");
            sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
            sql1 += string.Format("       ,LENGTH ");
            sql1 += string.Format("       ,SURFACE_LEVEL || ' ' || (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'SURFACE_LEVEL' AND CD_ID = X.SURFACE_LEVEL) AS SURFACE_LEVEL_NM ");
            sql1 += string.Format("       ,ORD_WGT ");
            sql1 += string.Format("       ,INPUT_WGT ");
            sql1 += string.Format("       ,END_WGT ");
            sql1 += string.Format("FROM   ( ");
            sql1 += string.Format("         SELECT  LINE_GP ");
            sql1 += string.Format("                ,WORK_ORD_DATE ");
            sql1 += string.Format("                ,WORK_RANK ");
            sql1 += string.Format("                ,ITEM_SIZE ");
            sql1 += string.Format("                ,STEEL ");
            sql1 += string.Format("                ,LENGTH ");
            sql1 += string.Format("                ,MAX(SURFACE_LEVEL) AS SURFACE_LEVEL ");
            sql1 += string.Format("                ,SUM(ORD_WGT) AS ORD_WGT ");
            sql1 += string.Format("                ,SUM((SELECT SUM(NET_WGT)  FROM  TB_CR_INPUT_WR WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ)) AS INPUT_WGT ");
            sql1 += string.Format("                ,SUM((SELECT SUM(NET_WGT) FROM TB_BND_WR WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ AND NVL(DEL_YN,'N') <> 'Y' )) AS END_WGT ");
            sql1 += string.Format("                ,MAX(POC_PROG_STAT) AS POC_PROG_STAT ");
            sql1 += string.Format("         FROM   TB_CR_INPUT_ORD  A ");
            sql1 += string.Format("         WHERE  LINE_GP   =    :P_LINE_GP ");
            //sql1 += string.Format("         AND    WORK_ORD_DATE = (select DISTINCT WORK_ORD_DATE from TB_CR_INPUT_ORD WHERE POC_SEQ = 1 AND POC_NO =  (SELECT DISTINCT POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP)) ");
            sql1 += string.Format("         AND    WORK_ORD_DATE = (SELECT WORK_ORD_DATE FROM (SELECT WORK_ORD_DATE FROM TB_CR_INPUT_ORD  WHERE POC_NO = (SELECT DISTINCT POC_NO  FROM TB_PROG_POC_MGMT  WHERE LINE_GP = :P_LINE_GP) order by WORK_ORD_DATE DESC) WHERE ROWNUM = 1) ");
            sql1 += string.Format("         AND    ITEM_SIZE = (select DISTINCT ITEM_SIZE from TB_CR_INPUT_ORD WHERE POC_NO =  (SELECT DISTINCT POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP)) ");
            sql1 += string.Format("         AND    STEEL = (select DISTINCT STEEL from TB_CR_INPUT_ORD WHERE POC_NO = (SELECT DISTINCT POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP)) ");
            sql1 += string.Format("         AND    LENGTH = (select DISTINCT LENGTH from TB_CR_INPUT_ORD WHERE POC_NO = (SELECT DISTINCT POC_NO FROM TB_PROG_POC_MGMT WHERE LINE_GP = :P_LINE_GP)) ");
            sql1 += string.Format("         AND    POC_PROG_STAT > 'A'   "); // -- 확정대기부터 조회
            sql1 += string.Format("         GROUP BY LINE_GP, WORK_ORD_DATE, WORK_RANK, ITEM_SIZE, STEEL, LENGTH ");
            sql1 += string.Format("         ORDER BY 1,2,3,4,5,6 ");
            sql1 += string.Format("        ) X ");

            
                string[] parm = new string[1];
                parm[0] = ":P_LINE_GP|" + ling_gp;
                //parm[1] = ":P_FR_DATE|" + vf.Format(start_date, "yyyyMMdd");
                //parm[2] = ":P_TO_DATE|" + vf.Format(end_date, "yyyyMMdd");
                olddt = cd.FindDataTable(sql1, parm);
            
            //olddt = cd.FindDataTable(sql1, parm);

            moddt = olddt.Copy();
            if (moddt.Rows.Count > 0)
            {
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                //grdMain.RowSel = 1;
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
        }
    }
}