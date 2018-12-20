﻿using C1.Win.C1FlexGrid;
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
    public partial class InsRegPopup : Form
    {
        //public static InsRegPopup Instance;


        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsCom ck = new clsCom();
        clsStyle cs = new clsStyle();
        //ColsStyleList cw = new ColsStyleList();


        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        List<string> msg;
        List<string> modifList;

        string line_gp = "";
        string zone_cd = "";

        private string mlft_YN = "";

        string isRework = "Y";

        string toZone_cd = "";

        bool IsFirst = false;

        ArrayList _al = new ArrayList();

        bool CanZoneMoveAcl;



        public InsRegPopup()
        {

            //Instance = this;

            IsFirst = true;

            line_gp = ck.StrKey1;

            zone_cd = ck.StrKey2;

            InitializeComponent();
        }
        public InsRegPopup(bool _CanZoneMoveAcl):this()
        {
            CanZoneMoveAcl = _CanZoneMoveAcl;
            ////Instance = this;

            //IsFirst = true;

            //line_gp = ck.StrKey1;

            //zone_cd = ck.StrKey2;

            //InitializeComponent();
        }


        private void c1FlexGrid1_Click(object sender, EventArgs e)
        {

        }

        private void InsRegPopup_Load(object sender, EventArgs e)
        {
            MinimizeBox = false;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            InitControl();

            //SetComboBox6();

            IsFirst = false;

            btnSave.Enabled = CanZoneMoveAcl;

            //btnSearch_Click(null, null);
        }

        private void InitControl()
        {
            cs.InitCombo(cbLine, StringAlignment.Center);

            cs.InitCombo(cbZone, StringAlignment.Near);

            cs.InitCombo(cbToZone, StringAlignment.Near);

            SetComboBox(ck.StrKey1, ck.StrKey2);

            clsStyle.Style.InitCombo(cboMLFT_YN, StringAlignment.Near);

            cs.InitTextBox(txtPoc);

            InitGrd_Main();

            ck.StrKey1 = "";
            ck.StrKey2 = "";

        }

        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["SEL"].Index);//, 0, grdMain.Cols["MFG_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.Cols["L_NO"].Width =  cs.L_No_Width;
            grdMain.Cols["SEL"].Width = cs.Sel_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;
            grdMain.Cols["ENTRY_DDTT"].Width = cs.ENTRY_DDTT_Width;
            grdMain.Cols["ZONE_CD"].Width = cs.ZONE_CD_Width;
            grdMain.Cols["BEF_ZONE_CD"].Width = cs.ZONE_CD_Width;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width -30;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width -20;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width ;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width ;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width ;
            grdMain.Cols["GOOD_YN"].Width = cs.Good_NG_Width;
            grdMain.Cols["MAT_GOOD_NG"].Width = cs.Good_NG_Width;
            grdMain.Cols["MLFT_GOOD_NG"].Width = cs.Good_NG_Width;
            grdMain.Cols["UT_GOOD_NG"].Width = cs.Good_NG_Width;
            grdMain.Cols["MPI_GOOD_NG"].Width = cs.Good_NG_Width;
            grdMain.Cols["GR_GOOD_NG"].Width = cs.Good_NG_Width;

            grdMain.Cols["MPI_FAULT_CD"].Width = cs.MPI_FAULT_CD_Width;
            grdMain.Cols["MEASURE_LENGTH"].Width = cs.LENGTH_L_Width;
            grdMain.Cols["BAD_LENGTH"].Width = cs.LENGTH_L_Width;

            grdMain.Cols["BUNDLE_NO"].Width = cs.BUNDLE_NO_Width;
            grdMain.Cols["PROG_STAT"].Width = cs.PROG_STAT_Width;
            grdMain.Cols["BEF_PROG_STAT"].Width = cs.PROG_STAT_Width;

            grdMain.Cols["EXIT_DDTT"].Width = cs.ENTRY_DDTT_Width;
            grdMain.Cols["TO_ZONE"].Width = 0 ;
            grdMain.Cols["REWORK_YN"].Width = cs.REWORK_YN_Width;


            #region 1. grdMain1 head 및 row  align 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["PIECE_NO"].TextAlign = cs.PIECE_NO_TextAlign;
            grdMain.Cols["ENTRY_DDTT"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["ZONE_CD"].TextAlign = cs.ZONE_CD_TextAlign;
            grdMain.Cols["BEF_ZONE_CD"].TextAlign = cs.ZONE_CD_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["GOOD_YN"].TextAlign = cs.GOOD_YN_TextAlign;
            grdMain.Cols["MAT_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["MLFT_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["UT_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["MPI_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;
            grdMain.Cols["GR_GOOD_NG"].TextAlign = cs.GOOD_NG_TextAlign;

            grdMain.Cols["MPI_FAULT_CD"].TextAlign = cs.MPI_FAULT_CD_TextAlign;
            grdMain.Cols["MEASURE_LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["BAD_LENGTH"].TextAlign = cs.LENGTH_TextAlign;

            grdMain.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_NO_TextAlign;
            grdMain.Cols["PROG_STAT"].TextAlign = cs.PROG_STAT_TextAlign;
            grdMain.Cols["BEF_PROG_STAT"].TextAlign = cs.PROG_STAT_TextAlign;
            grdMain.Cols["EXIT_DDTT"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["TO_ZONE"].TextAlign = cs.ZONE_CD_TextAlign;
            grdMain.Cols["REWORK_YN"].TextAlign = cs.REWORK_YN_TextAlign;
            #endregion

            //grdMain.Cols["STEEL_GRP"].DataMap = steel_Nm_ld;
            //grdMain.Cols["STEEL_GRP"].TextAlign = TextAlignEnum.LeftCenter;


            grdMain.AutoResize = true;


            grdMain.AllowEditing = true;


            grdMain.Cols["L_NO"].AllowEditing = false;
            //grdMain.Cols["SEL"].AllowEditing = cs.Sel_AllowEditing;
            grdMain.Cols["MILL_NO"].AllowEditing = false;
            grdMain.Cols["PIECE_NO"].AllowEditing = false;
            grdMain.Cols["ENTRY_DDTT"].AllowEditing = false;
            grdMain.Cols["ZONE_CD"].AllowEditing = false;
            grdMain.Cols["BEF_ZONE_CD"].AllowEditing = false;
            grdMain.Cols["POC_NO"].AllowEditing = false;
            grdMain.Cols["HEAT"].AllowEditing = false;
            grdMain.Cols["STEEL"].AllowEditing = false;
            grdMain.Cols["STEEL_NM"].AllowEditing = false;
            grdMain.Cols["ITEM"].AllowEditing = false;
            grdMain.Cols["ITEM_SIZE"].AllowEditing = false;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["GOOD_YN"].AllowEditing = false;
            grdMain.Cols["MAT_GOOD_NG"].AllowEditing = false;
            grdMain.Cols["MLFT_GOOD_NG"].AllowEditing = false;
            grdMain.Cols["UT_GOOD_NG"].AllowEditing = false;
            grdMain.Cols["MPI_GOOD_NG"].AllowEditing = false;
            grdMain.Cols["GR_GOOD_NG"].AllowEditing = false;

            grdMain.Cols["MPI_FAULT_CD"].AllowEditing = false;
            grdMain.Cols["MEASURE_LENGTH"].AllowEditing = false;
            grdMain.Cols["BAD_LENGTH"].AllowEditing = false;

            grdMain.Cols["BUNDLE_NO"].AllowEditing = false;
            grdMain.Cols["PROG_STAT"].AllowEditing = false;
            grdMain.Cols["BEF_PROG_STAT"].AllowEditing = false;

            grdMain.Cols["EXIT_DDTT"].AllowEditing = false;
            grdMain.Cols["TO_ZONE"].AllowEditing = false;
            grdMain.Cols["REWORK_YN"].AllowEditing = false;


            grdMain.Cols.Frozen = grdMain.Cols["ENTRY_DDTT"].Index;


            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;

            lbSel.Click += SEL_Click;

            //if (line_gp == "#3")
            //{
            //    lbSel.Click += SEL_Click;
            //}

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain, lbSel, 0, grdMain.Cols["SEL"].Index));
        }

        private void SEL_Click(object sender, EventArgs e)
        {

            //첫행의 poc No를 기준으로 동일한 poc 만 체크함

            string indexPOC = grdMain.GetData(1, "POC_NO").ToString().Trim();

            string rowPOC = string.Empty;

            if (allChecked) 
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    rowPOC = grdMain.GetData(rowCnt, "POC_NO").ToString().Trim();
                    if (indexPOC == rowPOC)
                    {

                            grdMain.SetData(rowCnt, "SEL", false);
                            lastCheckedPOCNO = string.Empty;
                        
                    }
                    if (line_gp != "#3")
                    {
                        grdMain.SetData(rowCnt, "SEL", false);
                        lastCheckedPOCNO = string.Empty;
                    }
                    //SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    rowPOC = grdMain.GetData(rowCnt, "POC_NO").ToString().Trim();
                    if (indexPOC == rowPOC)
                    {
                        if (line_gp == "#3")
                        {
                            grdMain.SetData(rowCnt, "SEL", true);
                            lastCheckedPOCNO = rowPOC;
                        }
                        
                    }
                    if (line_gp != "#3")
                    {
                        grdMain.SetData(rowCnt, "SEL", true);
                        lastCheckedPOCNO = string.Empty;
                    }
                    //grdMain.SetData(rowCnt, "SEL", true);
                    //SetupOrdBundleNo(grdSub, rowCnt);
                }
                allChecked = true;

            }
        }

        private void SetComboBox(string _line_gp, string _initZone)
        {
            SetLIne(_line_gp);

            SetZone(_line_gp, _initZone);
            
            SetToZone(_line_gp, _initZone);

            SetComboBox6();

            //zone_cd 가 있을때 해당 zone을 선택하게한다.
        }

        private void ChangedLIne()
        {
            //SetZone(line_gp, "");

            //SetToZone(line_gp, zone_cd);
        }

        private void ChangedZone()
        {
            SetToZone(line_gp, zone_cd);
        }

        private void SetZone(string _ling_gp, string _InitZone_id)
        {
            SetCombo(cbZone, "TB_RL_TM_TRACKING", _ling_gp, _InitZone_id, true);
        }

        private void SetToZone(string _line_gp, string _InitZone_id)
        {
            SetCombo(cbToZone, "TB_ZONE_MOVE_ROUTE", _line_gp, _InitZone_id, "", false);
            //SetCombo(cbToZone, "TB_CR_ZONEINFO", _line_gp, _InitZone_id, "", false);
        }



        private void SetLIne(string _line_gp)
        {
            // Line_Gp
            //cd.SetCombo(cbLine, "LINE_GP");

            
            cd.SetComboIDNM(cbLine, "LINE_GP");

            cbLine.SelectedValue = _line_gp;


        }

        private void SetCombo(ComboBox cb, string tableNM, string _ling_gp, bool isTotalAdd)
        {
            string sql1 = string.Empty;
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                sql1 = string.Format("select distinct ZONE_CD from {0} where LINE_GP = '{1}' order by ZONE_CD ASC", tableNM, _ling_gp);
                DataTable dt = cd.FindDataTable(sql1);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }
                

                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                //첫번째 아이템 선택
                //cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ;
                
            }
            return ;
        }

        private void SetCombo(ComboBox cb, string tableNM, string _ling_gp, string _Init_zone_id, bool isTotalAdd)
        {
            string sql1 = string.Empty;
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                sql1 = string.Format("select distinct ZONE_CD from {0} where LINE_GP = '{1}' order by ZONE_CD ASC", tableNM, _ling_gp);
                //sql1 = string.Format("select distinct ZONE_CD from {0} where LINE_GP = '{1}' and ZONE_CD = '{2}' order by ZONE_CD ASC", tableNM, _ling_gp, _Init_zone_id);
                DataTable dt = cd.FindDataTable(sql1);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    //arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                //첫번째 아이템 선택
                //cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnText == _Init_zone_id)
                    {
                       cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }

        private void SetCombo(ComboBox cb, string tableNM, string _ling_gp, string _FromZoneId, string _Init_zone_id, bool isTotalAdd)
        {
            string sql1 = string.Empty;
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                sql1 = string.Format("select distinct MOVE_ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = cd.FindDataTable(sql1);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row["MOVE_ZONE_CD"].ToString(), row["MOVE_ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    foreach (var item in cb.Items)
                    {
                        if (((DictionaryList)item).fnText == _Init_zone_id)
                        {
                            cb.SelectedIndex = cb.Items.IndexOf(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,'False' AS SEL ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT A.MILL_NO ");
                sql1 += string.Format("              ,A.PIECE_NO ");
                sql1 += string.Format("              ,TO_CHAR(A.ENTRY_DDTT,'YYYY-MM-DD HH24:MI:SS') AS ENTRY_DDTT ");
                sql1 += string.Format("              ,A.ZONE_CD ");
                sql1 += string.Format("              ,A.BEF_ZONE_CD ");
                sql1 += string.Format("              ,A.POC_NO ");
                sql1 += string.Format("              ,A.HEAT ");
                sql1 += string.Format("              ,B.STEEL ");
                sql1 += string.Format("              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = B.STEEL) AS STEEL_NM ");
                sql1 += string.Format("              ,B.ITEM ");
                sql1 += string.Format("              ,B.ITEM_SIZE ");
                sql1 += string.Format("              ,B.LENGTH ");
                sql1 += string.Format("              ,A.GOOD_YN ");
                sql1 += string.Format("              ,A.MAT_GOOD_NG ");
                sql1 += string.Format("              ,A.MLFT_GOOD_NG ");
                sql1 += string.Format("              ,A.UT_GOOD_NG ");
                sql1 += string.Format("              ,A.MPI_GOOD_NG ");
                sql1 += string.Format("              ,A.GR_GOOD_NG ");
                sql1 += string.Format("              ,A.MPI_FAULT_CD ");
                sql1 += string.Format("              ,A.MEASURE_LENGTH ");
                sql1 += string.Format("              ,A.BAD_LENGTH ");
                sql1 += string.Format("              ,A.BUNDLE_NO ");
                sql1 += string.Format("              ,A.REWORK_YN ");
                sql1 += string.Format("              ,A.PROG_STAT ");
                sql1 += string.Format("              ,A.BEF_PROG_STAT ");
                sql1 += string.Format("              ,TO_CHAR(A.EXIT_DDTT,'YYYY-MM-DD HH24:MI:SS') AS EXIT_DDTT ");
                sql1 += string.Format("              ,''  AS TO_ZONE ");
                sql1 += string.Format("        FROM   TB_RL_TM_TRACKING  A ");
                sql1 += string.Format("              ,TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("        WHERE  A.MILL_NO     =  B.MILL_NO ");
                sql1 += string.Format("        AND    A.PROG_STAT   IN ('RUN','WAT', 'FIN') ");
                sql1 += string.Format("        AND    A.LINE_GP     =  :P_LINE_GP ");
                sql1 += string.Format("        AND    A.ZONE_CD     LIKE  :P_ZONE_CD ");
                if (txtPoc.Text != "")
                    sql1 += string.Format("      AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);
                if (txtHeat.Text != "")
                    sql1 += string.Format("      AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);
                sql1 += string.Format("             AND    NVL(A.MLFT_GOOD_NG, '%')   LIKE '%{0}%' ", mlft_YN);
                sql1 += string.Format("        ORDER BY A.ENTRY_DDTT,A.MILL_NO,A.PIECE_NO,A.ZONE_CD DESC");
                sql1 += string.Format("        ) X ");

                //moddt = new DataTable();
                string[] parm = new string[2];
                parm[0] = ":P_LINE_GP|" + line_gp;
                parm[1] = ":P_ZONE_CD|" + zone_cd;

                moddt = cd.FindDataTable(sql1, parm);

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {

                MessageBox.Show("[" + ex.ToString() + "]");
                return ;
            }


            // 데이터가 그리드에 뿌려지고 첫번째 행이 선택 된상황을 만듬;
            //grdMain_Row_Selected(grdMain.Row);
            return ;
        }

        private void cbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (IsFirst)
            //{
            //    return;
            //}

            line_gp = ((ComLib.DictionaryList)cbLine.SelectedItem).fnValue;

            ChangedLIne();

            ////line 을 변경하면 zone cb 도 변경
            //SetCombo(cbZone, "TB_RL_TM_TRACKING", line_gp, true);

            ////이동가능한 zone 도 변경
            //SetCombo(cbToZone, "TB_CR_ZONEINFO", line_gp, false);

        }

        private void SetComboBox6()
        {
            cd.SetCombo(cboMLFT_YN, "GOOD_NG", "", true);
        }

        private void cbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbZone.DataSource != null)
            {
                zone_cd = ((DictionaryList)cbZone.SelectedItem).fnValue;

                // 전체이면      이동할존과 저장버튼 비활성화
                // 개별 존명이면 이동가능할 존 설정 및 저장버튼 활성화

                ChangedZone();

                if (((DictionaryList)cbZone.SelectedItem).fnText == "전체" || string.IsNullOrEmpty(((DictionaryList)cbZone.SelectedItem).fnText) || cbToZone.Items.Count == 0)
                {
                    SetSaveEnable(false);
                }
                else
                {
                    SetSaveEnable(true);
                }

                btnSearch_Click(null, null);
            }   
        }



        private void SetSaveEnable(bool IsEnable)
        {
            cbToZone.Enabled = IsEnable;
            btnSave.Enabled = (IsEnable && CanZoneMoveAcl);
        }

        private void cbToZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (IsFirst)
            //{
            //    return;
            //}

            if (cbToZone.DataSource != null)
            {
                toZone_cd = ((DictionaryList)cbToZone.SelectedItem).fnValue;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            #region 변경사항 적용여부 확인
            bool isChecked = false;
            for (int checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                isChecked = (grdMain.GetData(checkrow, "SEL").ToString() == "True") ? true : isChecked;

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

            #region TB_ZONE_MOVE_ROUTE 에 선택된 항목 입력 SP_ZONE_MOVE_PROC을 실행함 실행이 정상이면 메시지만 뿌리고 창 닫음, 실패하면 팝업으로 알리고 

            var logList = new List<LogDataList>();
            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            

            string sql1 = string.Empty;
            string selected = string.Empty;
            string result_stat = string.Empty;
            string result_msg = string.Empty;

            string spName = "SP_ZONE_MOVE_PROC";
            OracleParameter op;
            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                //수정된 데이터 인서트 
                #region 수정된 테이블 Insert

                for (int row = 1; row < grdMain.Rows.Count; row++)
                {

                    selected = grdMain.GetData(row, "SEL").ToString();

                    if (selected == "True")
                    {
                        sql1  = string.Format(" INSERT INTO TB_ZONE_MOVE_PROC ");
                        sql1 += string.Format("             ( ");
                        sql1 += string.Format("                  REGISTER ");
                        sql1 += string.Format("                 ,LINE_GP ");
                        sql1 += string.Format("                 ,MILL_NO ");
                        sql1 += string.Format("                 ,PIECE_NO ");
                        sql1 += string.Format("                 ,ZONE_CD ");
                        sql1 += string.Format("                 ,MOVE_ZONE_CD ");
                        sql1 += string.Format("                 ,PROC_GP ");
                        sql1 += string.Format("                 ,REG_DDTT ");
                        sql1 += string.Format("             ) ");
                        sql1 += string.Format(" VALUES ");
                        sql1 += string.Format("             ( ");
                        sql1 += string.Format("                  '{0}' ", ck.UserID);                                   //REGISTER ");
                        sql1 += string.Format("                 ,'{0}' ", line_gp);                       //LINE_GP ");
                        sql1 += string.Format("                 ,'{0}' ", grdMain.GetData(row, "MILL_NO").ToString());  //MILL_NO ");
                        sql1 += string.Format("                 ,'{0}' ", grdMain.GetData(row, "PIECE_NO")); //PIECE_NO ");
                        sql1 += string.Format("                 ,'{0}' ", grdMain.GetData(row, "ZONE_CD").ToString());   //ZONE_CD ");
                        sql1 += string.Format("                 ,'{0}' ", toZone_cd);  //MOVE_ZONE_CD ");
                        sql1 += string.Format("                 ,'{0}' ", "N");  //PROC_GP ");
                        sql1 += string.Format("                 , SYSDATE ");      //REG_DDTT ");
                        sql1 += string.Format("             ) ");

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();

                        logList.Add(new LogDataList(Alarms.InSerted, Text, sql1));
                    }
                }

                #endregion

                #region "SP_ZONE_MOVE_PROC" sp를 실행시켜 변경된 존이동 실행하게함.
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = line_gp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_ZONE_CD", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = zone_cd;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MOVE_ZONE_CD", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = toZone_cd;
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

                }
                else if (result_stat == "OK")
                {
                    #region Log Create
                    foreach (var log in logList)
                    {
                        clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                    }

                    string sp_msg = string.Format("SP_NAME: {0}, LINE_GP: {1}, ZONE_CD: {2}, MOVE_ZONE_CD: {3}, USER_ID: {4}", spName, line_gp, zone_cd, toZone_cd, ck.UserID);

                    clsMsg.Log.Alarm(Alarms.InSerted, Text, clsMsg.Log.__Line(), sp_msg);
                    #endregion

                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), result_msg);
                    btnClose_Click(null, null);

                    
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

        private void grdMain_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbToZone_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        string lastCheckedPOCNO = string.Empty;
        private bool allChecked;

        private void grdMain_CellChecked(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            // 1.처음 checked 한 행의 poc no를 저장한다.
            // 2.checked 된 행의 poc 가 처음 poc 와 다를때 체크 안됨을 메시지로 보여준다.
            var grd = sender as C1FlexGrid;
            var checkedRow = e.Row;
            var checkedPOCNO = grd.GetData(checkedRow, "POC_NO").ToString().Trim();
            var error_msg = "동일한 POC 만을 선택할수있습니다.";
            string checkedState = grd.GetData(checkedRow, "SEL").ToString();

            if (checkedState == "True")
            {
                if (lastCheckedPOCNO == "")
                {
                    lastCheckedPOCNO = checkedPOCNO;
                }
                else
                {
                    // 기존 poc 와 다른 poc 를 선택했을경우
                    if (lastCheckedPOCNO != checkedPOCNO) 
                    {
                        if (line_gp == "#3")
                        {
                            MessageBox.Show(error_msg);
                            grd.SetData(checkedRow, "SEL", false);
                        }
                    }
                }

                if (IsAllCheckedOrUnChecked(grd, "SEL", true)) allChecked = true;

            }
            else
            {
                // 전체가 unchecked 되었을때 선택된 poc 초기화  및 ALLchecked state = false

                if (IsAllCheckedOrUnChecked(grd, "SEL", false))
                {
                    
                        lastCheckedPOCNO = string.Empty;
                        allChecked = false;
                    
                }
                
            }

            // allchecked check
            

        }

        private bool IsCheckedGrid(C1FlexGrid grd, string selColName)
        {
            // checkedRow = false;
            for (int row = 1; row < grd.Rows.Count; row++)
            {
                if (grd.GetData(row, selColName).ToString() == "True")
                {
                    //checkedRow = true;
                    return true;
                }
                
            }
            return false;
        }

        private bool IsAllCheckedOrUnChecked(C1FlexGrid grd, string selColName, bool IsCheckedOrUnChecked)
        {
            for (int row = 1; row < grd.Rows.Count; row++)
            {
                if (grd.GetData(row, selColName).ToString() != IsCheckedOrUnChecked.ToString())
                {
                    return false;
                }

            }
            return true;
        }

        private void cboMLFT_YN_SelectedIndexChanged(object sender, EventArgs e)
        {
            mlft_YN = ((ComLib.DictionaryList)cboMLFT_YN.SelectedItem).fnValue;
        }

        private void UnCheckExistedCheckRow(C1FlexGrid grd , string selColName)
        {
            for (int row = 1; row < grd.Rows.Count; row++)
            {
                grd.SetData(row, selColName, false);
                allChecked = false;
            }
        }
    }
}