using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
//using DataControlClassLibrary;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SystemControlClassLibrary
{
    public partial class CrtInRsltstr : Form
    {
        #region 변수 설정
        clsCom cS = new clsCom();
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        private static string ownerNM = "";
        private static string titleNM = "";

        private int selectedrow = 0;
        private  string line_gp = "";
        private  string cd_id2 = "";
        private string work_TEAM = "";
        private  string gangjung_id = "";
        private string itemSize;
        private static string mill_no = "";
        clsStyle cs = new clsStyle();
        
        // 셀별 수정 정보(이전/이후)
        private static string[][] strModi = new string[0][];

        internal int sendData = 0;
        private int subtotalNo;
        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public CrtInRsltstr(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            InitializeComponent();

            Load += FrmSTRRControl_Load;

            ownerNM = ownerNm;
            titleNM = titleNm;

            btnDisplay.Click += Button_Click;
            btnExcel.Click += Button_Click;
            //btnReg.Click += Button_Click;
            //btnCancel.Click += Button_Click;
            grdMain.RowColChange += GrdMain_RowColChange;

            cboLine_GP.SelectedIndexChanged += CboLine_GP_SelectedIndexChanged;
            //cbo_Work_Type.SelectedIndexChanged += cbo_Work_Type_SelectedIndexChanged;
        }


        private void FrmSTRRControl_Load(object sender, System.EventArgs e)
        {
            InitControl();

            SetComboBox1();

            Button_Click(btnDisplay, null);

            EventCreate();      //사용자정의 이벤트
        }
        #endregion 로딩, 생성자 설정

        #region InitControl 설정
        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(label1);
            cs.InitLabel(label2);
            cs.InitLabel(label3);
            cs.InitLabel(label5);
            cs.InitLabel(label6);
            cs.InitLabel(label7);
            cs.InitLabel(label8);
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            //cs.InitButton(btnReg);
            //cs.InitButton(btnCancel);

            
            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            

            cs.InitTextBox(txtPoc);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(txtHeat);
            cs.InitTextBox(txtItemSize);
            cs.InitTextBox(gangjong_Nm_tb);

            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;


            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = grdMain.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            C1.Win.C1FlexGrid.CellStyle UpRs = grdMain.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            C1.Win.C1FlexGrid.CellStyle InsRs = grdMain.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;

            // 그리드 초기화
            InitGrid();
        
            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);
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

        #endregion InitControl 설정

        #region Init Grid 설정
        private void InitGrid()
        {
            clsStyle.Style.InitGrid_search(grdMain);
            grdMain.AllowEditing = false;
            #region caption 설정
            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["INPUT_DDTT"].Caption = "투입일시";
            grdMain.Cols["END_DDTT"].Caption = "완료일시";
            grdMain.Cols["WORK_TYPE"].Caption = "근";
            grdMain.Cols["WORK_TYPE_NM"].Caption = "근";
            grdMain.Cols["WORK_TEAM"].Caption = "조";
            grdMain.Cols["WORK_TEAM_NM"].Caption = "조";
            grdMain.Cols["POC"].Caption = "POC";
            grdMain.Cols["HEAT"].Caption = "HEAT";
            grdMain.Cols["STEEL"].Caption = "강종";
            grdMain.Cols["STEEL_NM"].Caption = "강종명";
            grdMain.Cols["ITEM"].Caption = "품목";
            grdMain.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain.Cols["LENGTH"].Caption = "길이(m)";
            grdMain.Cols["QTY"].Caption = "압연지시번들수";
            grdMain.Cols["CO"].Caption = "교정완료번들수";
            grdMain.Cols["MINUSS"].Caption = "미투입번들수";

            #endregion caption 설정

            #region TextAlign 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["END_DDTT"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["INPUT_DDTT"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["WORK_TEAM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TEAM_NM_TextAlign;
            grdMain.Cols["POC"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["QTY"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["CO"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MINUSS"].TextAlign = cs.PCS_TextAlign;
            
            #endregion TextAlign 설정

            #region width 설정
            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["END_DDTT"].Width = cs.Date_14_width;
            grdMain.Cols["INPUT_DDTT"].Width = cs.Date_14_width;
            grdMain.Cols["WORK_TYPE"].Width = 0;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width;
            grdMain.Cols["WORK_TEAM"].Width = 0;
            grdMain.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width;
            grdMain.Cols["POC"].Width = cs.POC_NO_Width;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain.Cols["QTY"].Width = cs.PCS_L_Width - 20;
            grdMain.Cols["CO"].Width = cs.Wgt_Width ;
            grdMain.Cols["MINUSS"].Width = cs.PCS_L_Width -20;
            
            #endregion width 설정

            grdMain.Tree.Column = 1;
        }
        #endregion Init Grid 설정

        #region SetDataBinding 설정
        private bool SetDataBinding()
        {
            try
            {
                string start_date = start_dt.Value.ToString();
                start_date = (start_date.Substring(0, 4) + start_date.Substring(5, 2) + start_date.Substring(8, 2));
                string end_date = end_dt.Value.ToString();
                end_date = (end_date.Substring(0, 4) + end_date.Substring(5, 2) + end_date.Substring(8, 2));

                txtPoc.Value = txtPoc.Text;

                string test = "";
                string sql1 = string.Format("SELECT  ROWNUM AS L_NO ");
                sql1 += string.Format("             ,INPUT_DDTT ");
                sql1 += string.Format("             ,END_DDTT ");
                sql1 += string.Format("             ,WORK_TYPE ");
                sql1 += string.Format("             ,WORK_TYPE_NM ");
                sql1 += string.Format("             ,WORK_TEAM ");
                sql1 += string.Format("             ,WORK_TEAM_NM ");
                sql1 += string.Format("             ,POC ");
                sql1 += string.Format("             ,HEAT ");
                sql1 += string.Format("             ,STEEL ");
                sql1 += string.Format("             ,STEEL_NM ");
                sql1 += string.Format("             ,ITEM ");
                sql1 += string.Format("             ,ITEM_SIZE ");
                sql1 += string.Format("             ,LENGTH "); // LENGTH
                sql1 += string.Format("             ,QTY  "); //--본수
                sql1 += string.Format("             ,CO  "); // INPUT_WGT --실중량
                sql1 += string.Format("             ,MINUSS  "); //--계산 본수
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("SELECT  ");
                sql1 += string.Format("             INPUT_DDTT ");
                sql1 += string.Format("             ,END_DDTT ");
                sql1 += string.Format("             ,WORK_TYPE ");
                sql1 += string.Format("             ,WORK_TYPE_NM ");
                sql1 += string.Format("             ,WORK_TEAM ");
                sql1 += string.Format("             ,WORK_TEAM_NM ");
                sql1 += string.Format("             ,POC ");
                sql1 += string.Format("             ,HEAT ");
                sql1 += string.Format("             ,STEEL ");
                sql1 += string.Format("             ,STEEL_NM ");
                sql1 += string.Format("             ,ITEM ");
                sql1 += string.Format("             ,ITEM_SIZE ");
                sql1 += string.Format("             ,LENGTH "); // LENGTH
                sql1 += string.Format("             ,QTY  "); //--본수
                sql1 += string.Format("             ,CO  "); // INPUT_WGT --실중량
                sql1 += string.Format("             ,MINUSS  "); //--계산 본수
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT  FN_GET_INFO(POC, 'I') INPUT_DDTT ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'J') END_DDTT ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'F') WORK_TYPE ");
                sql1 += string.Format("             ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = FN_GET_INFO(POC, 'F')) AS WORK_TYPE_NM ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'G') WORK_TEAM ");
                sql1 += string.Format("             ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID =FN_GET_INFO(POC, 'G')) AS WORK_TEAM_NM ");
                sql1 += string.Format("             ,POC ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'H') HEAT ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'A') STEEL ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'B') STEEL_NM ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'C') ITEM ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'D') ITEM_SIZE ");
                sql1 += string.Format("             ,FN_GET_INFO(POC, 'E') LENGTH "); // LENGTH
                sql1 += string.Format("             ,QTY  "); //--지시번들수
                sql1 += string.Format("             ,COUNT(*) CO  "); // 교정번들수
                sql1 += string.Format("             ,QTY - COUNT(*) MINUSS  "); //--미투입번들수
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT  A.POC_NO POC, F.BUNDLE_QTY QTY, A.MILL_NO, COUNT(*) STR_BON, C.ORD_PCS, C.ORD_PCS - (COUNT(*)) SETT ");
                sql1 += string.Format("               FROM TB_CR_PIECE_WR A, TB_CR_INPUT_WR B, TB_CR_ORD_BUNDLEINFO C, TB_CR_ORD F ");
                sql1 += string.Format("               WHERE A.POC_NO = B.POC_NO ");
                sql1 += string.Format("               AND   B.POC_NO = C.POC_NO ");
                sql1 += string.Format("               AND   A.POC_NO = F.POC_NO  ");
                sql1 += string.Format("               AND A.MILL_NO = B.MILL_NO  ");
                sql1 += string.Format("               AND B.MILL_NO = C.MILL_NO  ");
                sql1 += string.Format("               AND A.POC_NO IN ( SELECT DISTINCT POC_NO FROM  TB_CR_INPUT_WR  WHERE INPUT_DATE  BETWEEN '{0}' AND '{1}' ) ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                sql1 += string.Format("               AND A.ROUTING_CD = 'A1'  "); //교정
                sql1 += string.Format("               AND    A.LINE_GP    = '{0}' ", line_gp); //:P_LINE_GP


                if (txtHeat.Text != "")
                    sql1 += string.Format("      AND A.HEAT      LIKE '%{0}%' ", txtHeat.Text);

                if (gangjong_id_tb.Text != "")
                    sql1 += string.Format("      AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);

                if (txtItemSize.Text != "")
                    sql1 += string.Format("      AND A.ITEM_SIZE LIKE '%{0}%' ", txtItemSize.Text);

                if (txtPoc.Text != "")
                    sql1 += string.Format("      AND A.POC_NO      LIKE '%{0}%' ", txtPoc.Text);


                //sql1 += string.Format("               AND    A.POC_NO     LIKE '%{0}%'", txtPoc.Text);    //:P_POC_NO
                //sql1 += string.Format("               AND    A.HEAT       LIKE '%{0}%'", txtHeat.Text);  //:P_HEAT
                //sql1 += string.Format("               AND    A.ITEM_SIZE     LIKE '%{0}%'", itemSize);  //:P_ITEM_SIZE
                //sql1 += string.Format("               AND A.STEEL     LIKE '%{0}%' ", gangjong_id_tb.Text);
                //sql1 += string.Format("               AND    A.WORK_TYPE  LIKE '%{0}%'  ", cd_id2);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                //sql1 += string.Format("               AND    NVL(A.WORK_TEAM,'A')  LIKE '%{0}%'  ", work_TEAM);
                sql1 += string.Format("               GROUP BY A.POC_NO, A.MILL_NO, C.ORD_PCS, F.BUNDLE_QTY  ");
                sql1 += string.Format("        ) D ");
                sql1 += string.Format("WHERE STR_BON > 0 ");
                sql1 += string.Format("GROUP BY POC, QTY ");
                sql1 += string.Format(") X ");
                sql1 += string.Format("ORDER BY INPUT_DDTT ) Y");

                //sql1 += string.Format(" ORDER BY INPUT_DDTT DESC ");

                olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                grdMain.AutoSizeCols(grdMain.Cols["POC"].Index, grdMain.Cols["STEEL_NM"].Index, 5);
                grdMain.AutoSizeCols(grdMain.Cols["QTY"].Index, grdMain.Cols["MINUSS"].Index, 5);

                UpdateTotals();


                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            return true;
        }

        private void UpdateTotals()
        {

            subtotalNo = 0;

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["QTY"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["CO"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["MINUSS"].Index, "합계");

            //sql1 += string.Format("             ,INPUT_PCS  "); //--본수
            //sql1 += string.Format("             ,INPUT_WGT  "); // INPUT_WGT --실중량
            //sql1 += string.Format("             ,THEORY_PCS  "); //--계산 본수
            //sql1 += string.Format("             ,FINISH_PCS  "); //--완료 본수
            //sql1 += string.Format("             ,FINISH_WGT  "); // FINISH_WGT --완료 중량

            //sql1 += string.Format("        ,MILL_PCS");
            //sql1 += string.Format("        ,MILL_WGT");
            //sql1 += string.Format("        ,STR_PCS");

            
            AddSubtotalNo();
            grdMain.Rows.Frozen = GetAvailMinRow(grdMain) -1;

            //grdMain.Subtotal(AggregateEnum.Average, 1, -1, grdMain.Cols["THEORY_WGT"].Index, "평균");

            //grdMain.AutoSizeCols();

            //grdMain.Rows.Fixed = GetAvailMinRow();
        }

        private void AddSubtotalNo()
        {
            ++subtotalNo;
        }
        private int GetAvailMinRow(C1FlexGrid grid)
        {
            return (grid.Rows.Fixed + subtotalNo);
        }

        #endregion SetDataBinding 설정

        #region ComboBox 설정
        private void SetComboBox1()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
        }


        #endregion ComboBox 설정

        #region click 이벤트 설정
        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    SetDataBinding();
                    break;

                case "btnExcel":
                    SaveExcel();
                    break;


            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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


        #endregion click 이벤트 설정

        #region SelectedIndexChanged 이벤트 설정
        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp = ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = line_gp;
        }


        #endregion SelectedIndexChanged 이벤트 설정

        #region TextChanged 이벤트 설정
        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjong_Nm_tb.Text = "";
            gangjung_id = "";
        }

        private void txtItemSize_TextChanged(object sender, EventArgs e)
        {
            itemSize = txtItemSize.Text;
        }
        #endregion TextChanged 이벤트 설정

        #region 기타 이벤트 설정
        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        //사용자이벤트 생성
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
        }

        //강종ID(LostFocus)
        private void gangjong_id_tb_LostFocus(object sender, EventArgs e)
        {
            if (gangjong_id_tb.Text == "")
            {
                gangjong_Nm_tb.Text = "";
                gangjung_id = "";
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
                    gangjung_id = gangjong_id_tb.Text;
            }
        }

        private void GrdMain_RowColChange(object sender, EventArgs e)
        {

            string str = string.Empty;
            string temp = string.Empty;

            selectedrow = grdMain.RowSel;
        }






        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }
       

      

        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }
        #endregion 기타 이벤트 설정

        private void btnReg_Click(object sender, EventArgs e)
        {

        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            
        }
    }
}