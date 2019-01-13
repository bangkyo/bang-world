using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using SystemControlClassLibrary.Popup;


namespace SystemControlClassLibrary
{
    public partial class GrRsltInq : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        private int selectedrow = 0;
        private static string cd_id = "";
        private static string cd_id2 = "";
        private static string cd_id3 = "";

        private string poc = "";
        private string heat = "";
        private string item_size = "";
        private static string gangjung_id = "";
        clsStyle cs = new clsStyle();

        // 셀별 수정 정보(이전/이후)
        private static string[][] strModi = new string[0][];

        private static string ownerNM = "";
        private static string titleNM = "";
        private string sBundleNo = "";
        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public GrRsltInq(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();

            grdMain.RowColChange += GrdMain_RowColChange;
        }

        ~GrRsltInq()
        {

        }
        private void GrRsltInq_Load(object sender, System.EventArgs e)
        {
            this.BackColor = Color.White;

            InitControl();

            SetComboBox1();
            SetComboBox2();
            SetComboBox3();
            EventCreate();      //사용자정의 이벤트

            btnDisplay_Click(null, null);
        }
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }
        #endregion 로딩, 생성자 설정

        #region Init Control 설정
        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);

            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitPanel(panel1);
            
            //initLable
            clsStyle.Style.InitLabel(lblHeat);
            clsStyle.Style.InitLabel(lblSteel);
            clsStyle.Style.InitLabel(lblLine);
            clsStyle.Style.InitLabel(lblPoc);
            clsStyle.Style.InitLabel(lblWorkType);
            clsStyle.Style.InitLabel(lblMfgDate);
            clsStyle.Style.InitLabel(lblItemSize);
            clsStyle.Style.InitLabel(lblTEAM);

            //initCombo
            clsStyle.Style.InitCombo(cbo_Work_Type, StringAlignment.Near);
            clsStyle.Style.InitCombo(cboLine_GP, StringAlignment.Near);
            clsStyle.Style.InitCombo(cboTEAM, StringAlignment.Near);

            //initTextBox
            clsStyle.Style.InitTextBox(txtPoc);
            clsStyle.Style.InitTextBox(gangjong_id_tb);
            clsStyle.Style.InitTextBox(txtHeat);
            clsStyle.Style.InitTextBox(gangjong_Nm_tb);
            
            // Button Color Set
            clsStyle.Style.InitButton(btnExcel);
            clsStyle.Style.InitButton(btnDisplay);
            clsStyle.Style.InitButton(btnClose);

            //시간 데이터 default 값 적용 
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;

            //dateEdit Init()
            clsStyle.Style.InitDateEdit(start_dt);
            clsStyle.Style.InitDateEdit(end_dt);
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            InitGrd();
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
        #endregion Init Control 설정

        #region Init Grid 설정
        private void InitGrd()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowEditing = false;

            #region  caption 설정
            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["MILL_NO"].Caption = "압연번들번호";
            grdMain.Cols["PIECE_NO"].Caption = "P.NO";
            grdMain.Cols["LINE_GP"].Caption = "라인 ";
            grdMain.Cols["MFG_DATE"].Caption = "작업일자";
            grdMain.Cols["WORK_TIME"].Caption = "작업시각";
            grdMain.Cols["WORK_TYPE"].Caption = "WORK_TYPE";
            grdMain.Cols["WORK_TYPE_NM"].Caption = "근";
            grdMain.Cols["WORK_TEAM"].Caption = "WORK_TEAM";
            grdMain.Cols["WORK_TEAM_NM"].Caption = "조";
            grdMain.Cols["POC_NO"].Caption = "POC";
            grdMain.Cols["HEAT"].Caption = "HEAT";
            grdMain.Cols["STEEL"].Caption = "강종";
            grdMain.Cols["STEEL_NM"].Caption = "강종명";
            grdMain.Cols["ITEM"].Caption = "품목";
            grdMain.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain.Cols["LENGTH"].Caption = "길이(m)";
            grdMain.Cols["GOOD_YN"].Caption = "합부";
            grdMain.Cols["MPI_FAULT_CD"].Caption = "MPI_FAULT_CD";
            grdMain.Cols["FAULT_CD_NM"].Caption = "결함코드";
            grdMain.Cols["GUBUN"].Caption = "구분";
            #endregion  caption 설정

            #region TextAlign 설정
            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["PIECE_NO"].TextAlign = cs.PIECE_NO_TextAlign;
            grdMain.Cols["LINE_GP"].TextAlign = cs.LINE_GP_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["WORK_TEAM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["GOOD_YN"].TextAlign = cs.GOOD_YN_TextAlign;
            grdMain.Cols["MPI_FAULT_CD"].TextAlign = cs.MPI_FAULT_CD_TextAlign;
            grdMain.Cols["FAULT_CD_NM"].TextAlign = cs.MPI_FAULT_NM_TextAlign;
            #endregion TextAlign 설정

            #region width 설정
            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;
            grdMain.Cols["LINE_GP"].Width = 0;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TIME"].Width = cs.TIME_8_Width;
            grdMain.Cols["WORK_TYPE"].Width = 0;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width;
            grdMain.Cols["WORK_TEAM"].Width = 0;
            grdMain.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width ;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width + 10;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width + 20;
            grdMain.Cols["ITEM"].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain.Cols["GOOD_YN"].Width = 70;
            grdMain.Cols["MPI_FAULT_CD"].Width = 0;
            grdMain.Cols["FAULT_CD_NM"].Width = cs.MPI_FAULT_NM_Width;
            grdMain.Cols["GUBUN"].Width = 0;
            #endregion width 설정
        }
        #endregion Init Grid 설정

        #region ComboBox 설정
        private void SetComboBox1()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
        }

        private void SetComboBox2()
        {
            cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);
        }

        private void SetComboBox3()
        {
            cd.SetCombo(cboTEAM, "WORK_TEAM", "", true);
        }
        #endregion ComboBox 설정

        #region 조회 설정
        private void btnDisplay_Click(object sender, EventArgs e)
        {

            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            try
            {
                string start_date = start_dt.Value.ToString();
                start_date = (start_date.Substring(0, 4) + start_date.Substring(5, 2) + start_date.Substring(8, 2));
                string end_date = end_dt.Value.ToString();
                end_date = (end_date.Substring(0, 4) + end_date.Substring(5, 2) + end_date.Substring(8, 2));

                txtPoc.Value = txtPoc.Text;
                txtHeat.Value = txtHeat.Text;

                string sql1 = string.Format("  SELECT TO_CHAR(rownum) AS L_NO,");
                sql1 += string.Format("      X.*  ");
                sql1 += string.Format("FROM   (    ");
                sql1 += string.Format("       SELECT  MILL_NO ");
                sql1 += string.Format("              ,PIECE_NO ");
                sql1 += string.Format("              ,LINE_GP  ");
                sql1 += string.Format("              ,TO_CHAR(TO_DATE(A.MFG_DATE,'YYYYMMDD'),'YYYY-MM-DD') AS MFG_DATE ");
                sql1 += string.Format("              ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME ");
                sql1 += string.Format("              ,WORK_TYPE ");
                sql1 += string.Format("              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = A.WORK_TYPE) AS WORK_TYPE_NM ");
                sql1 += string.Format("              ,WORK_TEAM ");
                sql1 += string.Format("              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = A.WORK_TEAM) AS WORK_TEAM_NM ");
                sql1 += string.Format("              ,POC_NO  ");
                sql1 += string.Format("              ,HEAT  ");
                sql1 += string.Format("              ,STEEL  ");
                sql1 += string.Format("              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM  ");
                sql1 += string.Format("              ,ITEM  ");
                sql1 += string.Format("              ,ITEM_SIZE ");
                sql1 += string.Format("              ,TO_CHAR(LENGTH,'90.00') AS LENGTH  ");
                sql1 += string.Format("              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = A.GOOD_YN) AS GOOD_YN  ");
                sql1 += string.Format("              ,MPI_FAULT_CD  ");
                sql1 += string.Format("              ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'FAULT_CD' AND CD_ID = A.MPI_FAULT_CD) AS FAULT_CD_NM   ");
                sql1 += string.Format("              ,'' AS GUBUN   ");
                sql1 += string.Format("        FROM  TB_CR_PIECE_WR A   ");
                sql1 += string.Format("        WHERE  A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);
                sql1 += string.Format("        AND    A.LINE_GP    = '{0}' ", cd_id);
                sql1 += string.Format("        AND   A.ROUTING_CD =  'K2'            "); //--GR
                sql1 += string.Format("        AND    A.POC_NO    LIKE '%' || '{0}' || '%'", poc);
                sql1 += string.Format("        AND    A.HEAT       LIKE '%' ||'{0}' || '%'", heat);
                sql1 += string.Format("        AND    A.STEEL      LIKE '%' || '{0}' || '%'", gangjung_id);
                sql1 += string.Format("        AND    A.WORK_TYPE  LIKE '{0}' || '%' ", cd_id2);
                sql1 += string.Format("        AND    A.WORK_TEAM  LIKE '{0}' || '%' ", cd_id3);
                sql1 += string.Format("        AND    A.ITEM_SIZE  LIKE  '%'||'{0}' || '%' ", item_size);
                sql1 += string.Format("        AND   A.REWORK_SEQ   = ( SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR  ");
                sql1 += string.Format("                                 WHERE  MILL_NO    = A.MILL_NO   ");
                sql1 += string.Format("                                 AND    PIECE_NO   = A.PIECE_NO  ");
                sql1 += string.Format("                                 AND    LINE_GP    = A.LINE_GP     ");
                sql1 += string.Format("                                 AND    ROUTING_CD = A.ROUTING_CD )   ");
                sql1 += string.Format("        ORDER BY MFG_DATE DESC, WORK_TIME DESC , MILL_NO, PIECE_NO  ");
                sql1 += string.Format("        ) X         ");

                olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }
        #endregion 조회 설정

        #region 이벤트 설정

        #region Click 이벤트 설정
        private void btnExcel_Click(object sender, EventArgs e)
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        private void btnSteel_Click(object sender, EventArgs e)
        {
            SearchSteelNm popup = new SearchSteelNm();
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Click 이벤트 설정

        #region SelectedIndexChanged 이벤트 설정 
        private void cboLine_GP_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cd_id = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = cd_id;
        }

        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id2 = ((DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }

        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id3 = ((DictionaryList)cboTEAM.SelectedItem).fnValue;
        }
        #endregion SelectedIndexChanged 이벤트 설정 

        #region TextChanged 이벤트 설정
        private void txtPoc_TextChanged(object sender, EventArgs e)
        {
            poc = txtPoc.Text;
        }

        private void txtHeat_TextChanged(object sender, EventArgs e)
        {
            heat = txtHeat.Text;
        }

        private void txtItemSize_TextChanged(object sender, EventArgs e)
        {
            item_size = txtItemSize.Text;
        }
        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjong_Nm_tb.Text = "";
            gangjung_id = "";
        }
        #endregion TextChanged 이벤트 설정

        #region 기타이벤트 설정
        private void GrdMain_RowColChange(object sender, EventArgs e)
        {
            int maxrow = 0;
            int oldSel = 0;
            string str = string.Empty;
            string temp = string.Empty;

            selectedrow = grdMain.RowSel;
        }

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
                {
                    gangjung_id = gangjong_id_tb.Text;
                }
            }
        }

        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }
        #endregion 기타이벤트 설정

        #endregion 이벤트 설정
    }
}
