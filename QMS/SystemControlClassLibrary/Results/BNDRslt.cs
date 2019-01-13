﻿using C1.Win.C1FlexGrid;
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
using SystemControlClassLibrary.Popup;
using SystemControlClassLibrary.Results;

namespace SystemControlClassLibrary
{
    public partial class BNDRslt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        private  int oldValue = 0;
        private int selectedrow = 0;
        private  string line_gp = "";
        private  string work_type = "";
        private string auto_type = "";
        private string work_team = "";
        private  string cd_nm2 = "";

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

        private string scrAuth;
        private int subtotalNo;
        #endregion 변수 설정

        #region 생성자, 로딩 설정

        public BNDRslt(string _titleNm, string _scrAuth, string factCode, string _ownerNm)
        {
            ownerNM = _ownerNm;
            titleNM = _titleNm;
            scrAuth = _scrAuth;
            InitializeComponent();

            grdMain.RowColChange += GrdMain_RowColChange;
        }

        ~BNDRslt()
        {

        }
        private void FrmBNDRControl_Load(object sender, System.EventArgs e)
        {
            this.BackColor = Color.White;

            InitControl();

            SetComboBox1();
            SetComboBox2();
            SetComboBox3();
            SetComboBox4();

            EventCreate();      //사용자정의 이벤트

            btnDisplay_Click(null, null);
        }

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        #endregion 생성자, 로딩 설정

        #region init 컨트롤, 그리드 설정

        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitLabel(lblHeat);
            cs.InitLabel(lblSteel);
            cs.InitLabel(lblLine);
            cs.InitLabel(lblPoc);
            cs.InitLabel(lblWorkType);
            cs.InitLabel(lblMfgDate); 
            cs.InitLabel(lblItemSize);
            cs.InitLabel(lblTEAM);

            cs.InitCombo(cbo_Work_Type, StringAlignment.Near);
            cs.InitCombo(cbo_OnOff, StringAlignment.Near);
            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            cs.InitCombo(cboTEAM, StringAlignment.Near);

            cs.InitTextBox(txtPoc);
            cs.InitTextBox(gangjong_id_tb);
            cs.InitTextBox(txtHeat);
            cs.InitTextBox(gangjong_Nm_tb);

            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);

            // 그리드 스타일(색) 추가
            C1.Win.C1FlexGrid.CellStyle DelRs = grdMain.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            C1.Win.C1FlexGrid.CellStyle UpRs = grdMain.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            C1.Win.C1FlexGrid.CellStyle InsRs = grdMain.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;

            // 그리드 초기화

            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;
            //시간 데이터 default 값 적용 
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;


           
            //dateEdit Init()
            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);

            InitGrid();
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



        private void InitGrid()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            grdMain.AllowEditing = false;

            grdMain.Cols["L_NO"].Caption = "NO";
            grdMain.Cols["POC_NO"].Caption = "POC";
            grdMain.Cols["BUNDLE_NO"].Caption = "제품번들번호";
            grdMain.Cols["MFG_DATE"].Caption = "작업일자";
            grdMain.Cols["WORK_END_DDTT"].Caption = "작업시각";
            grdMain.Cols["WORK_TYPE"].Caption = "근";
            grdMain.Cols["WORK_TYPE_NM"].Caption = "근";
            grdMain.Cols["WORK_TEAM"].Caption = "조";
            grdMain.Cols["WORK_TEAM_NM"].Caption = "조";
            grdMain.Cols["HEAT"].Caption = "HEAT";
            grdMain.Cols["STEEL"].Caption = "강종";
            grdMain.Cols["STEEL_NM"].Caption = "강종명";
            grdMain.Cols["ITEM"].Caption = "품목";
            grdMain.Cols["ITEM_SIZE"].Caption = "규격";
            grdMain.Cols["LENGTH"].Caption = "길이(m)";
            grdMain.Cols["PCS"].Caption = "본수";
            grdMain.Cols["THEORY_WGT"].Caption = "이론중량(kg)";
            grdMain.Cols["NET_WGT"].Caption = "실중량(kg)";
            grdMain.Cols["BND_POINT1"].Caption = "결속Point1";
            grdMain.Cols["BND_POINT2"].Caption = "결속Point2";

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["BUNDLE_NO"].TextAlign = cs.BUNDLE_NO_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_END_DDTT"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["WORK_TEAM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grdMain.Cols["WORK_TEAM_NM"].TextAlign = cs.WORK_TEAM_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["PCS"].TextAlign = cs.FINISH_PCS_TextAlign;
            grdMain.Cols["THEORY_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["NET_WGT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["BND_POINT1"].TextAlign = cs.BND_POINT_TextAlign;
            grdMain.Cols["BND_POINT2"].TextAlign = cs.BND_POINT_TextAlign;

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width-20;
            grdMain.Cols["BUNDLE_NO"].Width = cs.BUNDLE_NO_Width;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_END_DDTT"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TYPE"].Width = 0;
            //grdMain.Cols["WORK_TYPE"].Visible = false;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width;
            grdMain.Cols["WORK_TEAM"].Width = 0;
            grdMain.Cols["WORK_TEAM_NM"].Width = cs.WORK_TEAM_NM_Width;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain.Cols["PCS"].Width = cs.PCS_Width;
            grdMain.Cols["THEORY_WGT"].Width = cs.Wgt_Width;
            grdMain.Cols["NET_WGT"].Width = cs.Wgt_Width;
            grdMain.Cols["BND_POINT1"].Width = 0;
            grdMain.Cols["BND_POINT2"].Width = 0;

            // more setup
            //grdMain.AllowDragging = AllowDraggingEnum.None;
            //grdMain.AllowEditing = false;
            //grdMain.Cols[0].WidthDisplay /= 3;
            grdMain.Tree.Column = 1;
        }

        #endregion init 컨트롤, 그리드 설정

        #region 콤보박스 설정

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

        private void SetComboBox4()
        {
            cd.SetCombo(cbo_OnOff, "AUTO_MODE", "", true);
        }

        #endregion 콤보박스 설정

        #region 이벤트 설정

        #region click event 설정
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

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            if (grdMain.Row < GetAvailMinRow(grdMain))
            {
                return;
            }
            sBundleNo = grdMain[grdMain.RowSel, 2].ToString();
            btnReg_Click(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

            #endregion click event 설정

            #region RowColChange 설정

        private void GrdMain_RowColChange(object sender, EventArgs e)
        {
            int maxrow = 0;
            int oldSel = 0;
            string str = string.Empty;
            string temp = string.Empty;

            selectedrow = grdMain.RowSel;
        }

            #endregion RowColChange 설정

            #region SelectedIndexChanged 설정

        private void cboLine_GP_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            line_gp =  ((ComLib.DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = line_gp;
        }
        private void cbo_Work_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_type = ((ComLib.DictionaryList)cbo_Work_Type.SelectedItem).fnValue;
        }

        private void cbo_OnOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            auto_type = ((ComLib.DictionaryList)cbo_OnOff.SelectedItem).fnValue;
        }

        private void cboTEAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            work_team = ((ComLib.DictionaryList)cboTEAM.SelectedItem).fnValue;
        }

        #endregion SelectedIndexChanged 설정

            #region TextChanged 설정

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

        #endregion TextChanged 설정

            #region 강종 이벤트 설정

        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjung_id = gangjong_id_tb.Text;
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
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

        #endregion 강종 이벤트 설정

            #region 사용자 이벤트 설정

        //사용자이벤트 생성
        private void EventCreate()
        {
            this.gangjong_id_tb.LostFocus += new System.EventHandler(gangjong_id_tb_LostFocus);            //강종ID
        }

        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

            #endregion 사용자 이벤트 설정

        #endregion 이벤트 설정

        #region 실적등록 설정

        private void btnReg_Click(object sender, EventArgs e)
        {
            //권한도 같이 가져가서 적용
            BNDRsltMgmt popup = new BNDRsltMgmt(line_gp, sBundleNo, scrAuth);
            popup.Owner = this; //A폼을 지정하게 된다.
            //popup.MaximizeBox = false;
            //popup.MinimizeBox = false;
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.ShowDialog();

            sBundleNo = "";
            btnDisplay_Click(null, null);

            if (ck.StrKey1 != "")
            {
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), ck.StrKey1);
            }
            ck.StrKey1 = "";
        }

        #endregion 실적등록 설정

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
                string sql1 = string.Empty;

                if (line_gp == "#3")
                {
                    sql1 = string.Format(" SELECT  ROWNUM AS L_NO ");
                    sql1 += string.Format("       ,POC_NO ");
                    sql1 += string.Format("       ,BUNDLE_NO ");
                    sql1 += string.Format("       ,MFG_DATE ");
                    sql1 += string.Format("       ,WORK_END_DDTT ");
                    sql1 += string.Format("       ,WORK_TYPE ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                    sql1 += string.Format("       ,WORK_TEAM ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                    sql1 += string.Format("       ,HEAT ");
                    sql1 += string.Format("       ,STEEL ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("       ,ITEM ");
                    sql1 += string.Format("       ,ITEM_SIZE ");
                    sql1 += string.Format("       ,LENGTH ");
                    sql1 += string.Format("       ,PCS ");
                    sql1 += string.Format("       ,THEORY_WGT ");
                    sql1 += string.Format("       ,NET_WGT ");
                    sql1 += string.Format("       ,BND_POINT1 ");
                    sql1 += string.Format("       ,BND_POINT2 ");
                    sql1 += string.Format("FROM   ( ");
                    sql1 += string.Format("        SELECT  A.POC_NO ");
                    sql1 += string.Format("               ,A.BUNDLE_NO ");
                    sql1 += string.Format("               ,TO_DATE(A.MFG_DATE) AS MFG_DATE ");
                    sql1 += string.Format("               ,TO_CHAR(TO_DATE(A.WORK_END_DDTT,'YYYYMMDDHH24MISS'),'HH24:MI:SS') AS WORK_END_DDTT ");
                    sql1 += string.Format("               ,A.WORK_TYPE ");
                    sql1 += string.Format("               ,NVL(A.WORK_TEAM, 'A') AS WORK_TEAM ");
                    sql1 += string.Format("               ,A.HEAT ");
                    sql1 += string.Format("               ,A.STEEL ");
                    sql1 += string.Format("               ,A.ITEM ");
                    sql1 += string.Format("               ,A.ITEM_SIZE ");
                    sql1 += string.Format("               ,TO_CHAR(A.LENGTH,'99.00') AS LENGTH ");
                    sql1 += string.Format("               ,A.PCS ");
                    sql1 += string.Format("               ,TO_CHAR(( FN_GET_WGT(A.ITEM,A.ITEM_SIZE,A.LENGTH,A.PCS) ),'999,999') AS THEORY_WGT ");  //to_
                    sql1 += string.Format("               ,TO_CHAR(( SELECT NET_WGT FROM TB_WGT_WR WHERE BUNDLE_NO = A.BUNDLE_NO AND NVL(DEL_YN,'N') <> 'Y' AND ROWNUM = 1),'999,999') AS NET_WGT");
                    sql1 += string.Format("               ,A.BND_POINT1 ");
                    sql1 += string.Format("               ,A.BND_POINT2 ");
                    sql1 += string.Format("        FROM   TB_BND_WR  A ");
                    sql1 += string.Format("        WHERE  A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                    sql1 += string.Format("        AND    A.LINE_GP    = '{0}' ", line_gp); //:P_LINE_GP
                    sql1 += string.Format("        AND    A.POC_NO     LIKE '%' ||'{0}' || '%'", poc);    //:P_POC_NO
                    sql1 += string.Format("        AND    A.HEAT       LIKE '%' ||'{0}' || '%'", heat);  //:P_HEAT
                    sql1 += string.Format("        AND    A.WORK_TYPE  LIKE '{0}' || '%' ", work_type);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                    sql1 += string.Format("        AND    A.WORK_TEAM  LIKE '{0}' || '%' ", work_team);
                    sql1 += string.Format("        AND    A.STEEL      LIKE '{0}' || '%'", gangjung_id);  //:P_STEEL
                    sql1 += string.Format("        AND    A.ITEM_SIZE  LIKE  '%'||'{0}' || '%' ", item_size);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                    if (auto_type == "A")
                    {
                        sql1 += string.Format("     AND   A.BUNDLE_NO_L2 IS NULL  ");
                    }
                    if (auto_type == "B")
                    {
                        sql1 += string.Format("     AND   A.BUNDLE_NO_L2 IS NOT NULL ");
                    }
                    sql1 += string.Format("        AND    NVL(A.DEL_YN,'N') <> 'Y'   ");
                    sql1 += string.Format("        ORDER BY MFG_DATE DESC, WORK_END_DDTT DESC, BUNDLE_NO ");
                    sql1 += string.Format("        ) X ");
                }
                else
                {
                    sql1 = string.Format(" SELECT  ROWNUM AS L_NO ");
                    sql1 += string.Format("       ,POC_NO ");
                    sql1 += string.Format("       ,BUNDLE_NO ");
                    sql1 += string.Format("       ,MFG_DATE ");
                    sql1 += string.Format("       ,WORK_END_DDTT ");
                    sql1 += string.Format("       ,WORK_TYPE ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE) AS WORK_TYPE_NM ");
                    sql1 += string.Format("       ,WORK_TEAM ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TEAM' AND CD_ID = X.WORK_TEAM) AS WORK_TEAM_NM ");
                    sql1 += string.Format("       ,HEAT ");
                    sql1 += string.Format("       ,STEEL ");
                    sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                    sql1 += string.Format("       ,ITEM ");
                    sql1 += string.Format("       ,ITEM_SIZE ");
                    sql1 += string.Format("       ,LENGTH ");
                    sql1 += string.Format("       ,PCS ");
                    sql1 += string.Format("       ,THEORY_WGT ");
                    sql1 += string.Format("       ,NET_WGT ");
                    sql1 += string.Format("       ,BND_POINT1 ");
                    sql1 += string.Format("       ,BND_POINT2 ");
                    sql1 += string.Format("FROM   ( ");
                    sql1 += string.Format("        SELECT  A.POC_NO ");
                    sql1 += string.Format("               ,A.BUNDLE_NO ");
                    sql1 += string.Format("               ,TO_DATE(A.MFG_DATE) AS MFG_DATE ");
                    sql1 += string.Format("               ,TO_CHAR(TO_DATE(A.WORK_END_DDTT,'YYYYMMDDHH24MISS'),'HH24:MI:SS') AS WORK_END_DDTT ");
                    sql1 += string.Format("               ,A.WORK_TYPE ");
                    sql1 += string.Format("               ,NVL(A.WORK_TEAM, 'A') AS WORK_TEAM ");
                    sql1 += string.Format("               ,A.HEAT ");
                    sql1 += string.Format("               ,A.STEEL ");
                    sql1 += string.Format("               ,A.ITEM ");
                    sql1 += string.Format("               ,A.ITEM_SIZE ");
                    sql1 += string.Format("               ,TO_CHAR(A.LENGTH,'99.00') AS LENGTH ");
                    sql1 += string.Format("               ,A.PCS ");
                    sql1 += string.Format("               ,TO_CHAR(( FN_GET_WGT(A.ITEM,A.ITEM_SIZE,A.LENGTH,A.PCS) ),'999,999') AS THEORY_WGT ");  //to_
                    sql1 += string.Format("               ,TO_CHAR(( SELECT NET_WGT FROM TB_WGT_WR WHERE BUNDLE_NO = A.BUNDLE_NO AND NVL(DEL_YN,'N') <> 'Y' AND ROWNUM = 1),'999,999') AS NET_WGT");
                    sql1 += string.Format("               ,A.BND_POINT1 ");
                    sql1 += string.Format("               ,A.BND_POINT2 ");
                    sql1 += string.Format("        FROM   TB_BND_WR  A ");
                    sql1 += string.Format("        WHERE  A.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                    sql1 += string.Format("        AND    A.LINE_GP    = '{0}' ", line_gp); //:P_LINE_GP
                    sql1 += string.Format("        AND    A.POC_NO     LIKE '%' ||'{0}' || '%'", poc);    //:P_POC_NO
                    sql1 += string.Format("        AND    A.HEAT       LIKE '%' ||'{0}' || '%'", heat);  //:P_HEAT
                    sql1 += string.Format("        AND    A.WORK_TYPE  LIKE '{0}' || '%' ", work_type);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                    sql1 += string.Format("        AND    A.WORK_TEAM  LIKE '{0}' || '%' ", work_team);
                    sql1 += string.Format("        AND    A.STEEL      LIKE '{0}' || '%'", gangjung_id);  //:P_STEEL
                    sql1 += string.Format("        AND    A.ITEM_SIZE  LIKE  '%'||'{0}' || '%' ", item_size);//, cbo_Work_Type.Text);    //:P_WORK_TYPE
                    sql1 += string.Format("        AND    NVL(A.DEL_YN,'N') <> 'Y'   ");
                    sql1 += string.Format("        ORDER BY MFG_DATE DESC, WORK_END_DDTT DESC, BUNDLE_NO ");
                    sql1 += string.Format("        ) X ");

                }


                    olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                if (moddt.Rows.Count > 1)
                {
                    UpdateTotals();
                }

                //grdMain.AutoSizeCols();
                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
            }
        }
        #endregion 조회 설정

        private void UpdateTotals()
        {

            subtotalNo = 0;

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["THEORY_WGT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["NET_WGT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["PCS"].Index, "합계");


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
        private int GetAvailMinRow()
        {
            return (grdMain.Rows.Fixed + subtotalNo);
        }
        private int GetAvailMinRow(C1FlexGrid grid)
        {
            return (grid.Rows.Fixed + subtotalNo);
        }


    }
}
