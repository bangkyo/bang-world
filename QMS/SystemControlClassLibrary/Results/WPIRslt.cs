using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using static ComLib.clsUtil;
using System.Collections.Specialized;
using SystemControlClassLibrary.Popup;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

using excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;

namespace SystemControlClassLibrary
{

    public partial class WPIRslt : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;


        private int selectedrow = 0;
        private string cd_id = "";
        private string cd_nm = "";
        private string cd_id2 = "";
        private string cd_nm2 = "";
        private string cd_id3 = "";

        private string txtheat = "";
        private string txtpoc = "";
        private string txtitem_size = "";
        private string txtsteel = "";
        private string txtsteel_nm = "";
        private string gangjung_id = "";
        int subtotalNo ;

        clsStyle cs = new clsStyle();

        // 셀의 수정전 값
        private string strBefValue = "";


        private string selected_grp_id = "";


        private string ownerNM = "";
        private string titleNM = "";

        private string scrAuth;

        string sMeasNo = "";
        #endregion

        #region 생성자, 로딩 설정
        public WPIRslt(string titleNm, string _scrAuth, string factCode, string ownerNm)
        {

            ownerNM = ownerNm;
            titleNM = titleNm;
            scrAuth = _scrAuth;
            InitializeComponent();

            //Load += WPIRslt_Load;



            btnDisplay.Click += Button_Click;
            btnExcel.Click += Button_Click;

            grdMain.RowColChange += GrdMain_RowColChange;

            cboLine_GP.SelectedIndexChanged += CboLine_GP_SelectedIndexChanged;
            cbo_ROUTING_Type.SelectedIndexChanged += cbo_ROUTING_Type_SelectedIndexChanged;
            //cbo_Work_Type.SelectedIndexChanged += cbo_Work_Type_SelectedIndexChanged;

            //btnSteel.Click += btnSteel_Click;

        }

        ~WPIRslt()
        {
            
        }
        private void WPIRslt_Load(object sender, System.EventArgs e)
        {


            this.BackColor = Color.White;

            InitControl();


            SetComboBox1();
            SetComboBox2();
            //SetComboBox2();
            //SetComboBox3();
            //SetDataBinding(); // 그리드 초기화면 제어 
            //EventCreate();      //사용자정의 이벤트
            Button_Click(btnDisplay, null);
            //SetDataBinding();
        }

        #endregion

        #region Init 컨트롤, 그리드 설정

        private void InitControl()
        {

            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);


            cs.InitLabel(lblLine);
            cs.InitLabel(lblPoc);
            cs.InitLabel(lblHeat);
            cs.InitLabel(lblItemSize);
            //cs.InitLabel(lblSteel);
            //cs.InitLabel(lblWorkType);
            cs.InitLabel(lblMfgDate);
            //cs.InitLabel(lblTEAM);

            //cs.InitCount(lblCount);

            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            clsStyle.Style.InitCombo(cbo_ROUTING_Type, StringAlignment.Near);
            //cs.InitCombo(cbo_Work_Type, StringAlignment.Near);
            //cs.InitCombo(cboTEAM, StringAlignment.Near);

            cs.InitTextBox(poctxt);
            cs.InitTextBox(heattxt);
            cs.InitTextBox(txtItemSize);
            //cs.InitTextBox(gangjong_id_tb);
            //cs.InitTextBox(gangjong_Nm_tb);


            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);

            // Button Color Set
            cs.InitButton(btnExcel);
            cs.InitButton(btnDisplay);
            cs.InitButton(btnClose);
            cs.InitButton(btnReg);
            cs.InitButton(btnLabel);



            //시간 데이터 default 값 적용 
            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;

            //dateEdit Init()
            cs.InitDateEdit(start_dt);
            cs.InitDateEdit(end_dt);

            InitGrd_Main();

        }

        private void SetComboBox2()
        {
            List<string> Extractlist = new List<string>();
            Extractlist.Add("교정");
            Extractlist.Add("면취");
            Extractlist.Add("쇼트");
            Extractlist.Add("PRII");
            Extractlist.Add("성분분석");
            Extractlist.Add("라벨");
            Extractlist.Add("결속");
            Extractlist.Add("계중");
            Extractlist.Add("격외");

            cd.SetCombo(cbo_ROUTING_Type, "ROUTING_CD", Extractlist, false);
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

        private void cbo_ROUTING_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id2 = ((ComLib.DictionaryList)cbo_ROUTING_Type.SelectedItem).fnValue;
        }
        private void InitGrd_Main()
        {
            cs.InitGrid_search(grdMain, false, 2, 1);


            grdMain.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            grdMain.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            //grdMain.Dock = DockStyle.Fill;
            //grdMain.AllowEditing = false;

            int level1 = 50; // 2자리
            int level2 = 50; // 4자리
            int level2_5 = 60;
            int level3 = 90; // 6자리
            int level4 = 170; // 8자리이상

            #region 1. grdMain head 및 row  align 설정

            grdMain.Rows[1].Height = 40;

            grdMain[1, "L_NO"] = grdMain.Cols["L_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "P_YN"] = grdMain.Cols["P_YN"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "P_CNT"] = grdMain.Cols["P_CNT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MFG_DATE"] = grdMain.Cols["MFG_DATE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "POC_NO"] = grdMain.Cols["POC_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STEEL"] = grdMain.Cols["STEEL"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STEEL_NM"] = grdMain.Cols["STEEL_NM"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "BUNDLE_NO"] = grdMain.Cols["BUNDLE_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "HEAT"] = grdMain.Cols["HEAT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "ITEM_SIZE"] = grdMain.Cols["ITEM_SIZE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "LENGTH"] = grdMain.Cols["LENGTH"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "PCS"] = grdMain.Cols["PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "THEORY_WGT"] = grdMain.Cols["THEORY_WGT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MPI_PCS"] = grdMain.Cols["MPI_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MPI_WGT"] = grdMain.Cols["MPI_WGT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "MPI_DDTT"] = grdMain.Cols["MPI_DDTT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "GR_PCS"] = grdMain.Cols["GR_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "GR_WGT"] = grdMain.Cols["GR_WGT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "GR_DDTT"] = grdMain.Cols["GR_DDTT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "END_PCS"] = grdMain.Cols["END_PCS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "END_WGT"] = grdMain.Cols["END_WGT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "END_DDTT"] = grdMain.Cols["END_DDTT"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "LOCATION"] = grdMain.Cols["LOCATION"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);


            grdMain[1, "MPI_PCS"] = "본수";
            grdMain[1, "MPI_WGT"] = "중량";
            grdMain[1, "MPI_DDTT"] = "처리일자";
            grdMain[1, "GR_PCS"] = "본수";
            grdMain[1, "GR_WGT"] = "중량";
            grdMain[1, "GR_DDTT"] = "처리일자";
            grdMain[1, "END_PCS"] = "본수";
            grdMain[1, "END_WGT"] = "중량";
            grdMain[1, "END_DDTT"] = "처리일자";


            grdMain.Cols["L_NO"].Width = 0;
            grdMain.Cols["P_YN"].Width = 73;
            grdMain.Cols["P_CNT"].Width = 73;
            grdMain.Cols["HEAT"].Width = level3;
            grdMain.Cols["POC_NO"].Width = level3;
            grdMain.Cols["STEEL"].Width = 0;
            grdMain.Cols["STEEL_NM"].Width = 0;
            grdMain.Cols["MFG_DATE"].Width = level4;
            grdMain.Cols["BUNDLE_NO"].Width = 60;
            grdMain.Cols["ITEM_SIZE"].Width = level2_5;
            grdMain.Cols["LENGTH"].Width = level2_5;
            grdMain.Cols["PCS"].Width = 70;
            grdMain.Cols["THEORY_WGT"].Width = 70;
            grdMain.Cols["MPI_PCS"].Width = 70;
            grdMain.Cols["MPI_WGT"].Width = 70;
            grdMain.Cols["MPI_DDTT"].Width = level4;
            grdMain.Cols["GR_PCS"].Width = 70;
            grdMain.Cols["GR_WGT"].Width = 70;
            grdMain.Cols["GR_DDTT"].Width = level4;
            grdMain.Cols["END_PCS"].Width = 70;
            grdMain.Cols["END_WGT"].Width = 70;
            grdMain.Cols["END_DDTT"].Width = level4;
            grdMain.Cols["LOCATION"].Width = level4;
            //grdMain.Cols["GUBUN"].Width = 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["P_YN"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["P_CNT"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["BUNDLE_NO"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["THEORY_WGT"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MPI_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MPI_WGT"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["MPI_DDTT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["GR_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["GR_WGT"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["GR_DDTT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["END_PCS"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["END_WGT"].TextAlign = cs.PCS_TextAlign;
            grdMain.Cols["END_DDTT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["LOCATION"].TextAlign = cs.HEAT_TextAlign;

            #endregion

            grdMain.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                grdMain.Cols[i].AllowMerging = true;
            }

            grdMain.Rows[0].AllowMerging = true;

            grdMain.Rows[0].AllowMerging = true;

            grdMain.AllowEditing = true;
            grdMain.Rows[0].AllowEditing = false;
            grdMain.Rows[1].AllowEditing = false;

            grdMain.Cols["L_NO"].AllowEditing = false;
            grdMain.Cols["P_YN"].AllowEditing = true;
            grdMain.Cols["P_CNT"].AllowEditing = true;
            grdMain.Cols["HEAT"].AllowEditing = false;
            grdMain.Cols["POC_NO"].AllowEditing = false;
            grdMain.Cols["STEEL"].AllowEditing = false;
            grdMain.Cols["STEEL_NM"].AllowEditing = false;
            grdMain.Cols["MFG_DATE"].AllowEditing = false;
            grdMain.Cols["BUNDLE_NO"].AllowEditing = true;
            grdMain.Cols["LENGTH"].AllowEditing = false;
            grdMain.Cols["PCS"].AllowEditing = false;
            grdMain.Cols["THEORY_WGT"].AllowEditing = false;
            grdMain.Cols["MPI_PCS"].AllowEditing = false;
            grdMain.Cols["MPI_WGT"].AllowEditing = false;
            grdMain.Cols["MPI_DDTT"].AllowEditing = false;
            grdMain.Cols["GR_PCS"].AllowEditing = false;
            grdMain.Cols["GR_WGT"].AllowEditing = false;
            grdMain.Cols["GR_DDTT"].AllowEditing = false;
            grdMain.Cols["END_PCS"].AllowEditing = false;
            grdMain.Cols["END_WGT"].AllowEditing = false;
            grdMain.Cols["END_DDTT"].AllowEditing = false;
            grdMain.Cols["LOCATION"].AllowEditing = false;

            //grdMain.Cols[5].Format = "#,##";
            //for (int i = 5; i < grdMain.Cols.Count; i++)
            //{
            //    grdMain.Cols[i].Format = "#,###";
            //}

            #region 콤보박스 설정
            //SetComboinGrd();
            //grdMain.Cols["FAULT_CD"].DataMap = fault_cd;
            //grdMain.Cols["FAULT_CD"].TextAlign = TextAlignEnum.LeftCenter;

            #endregion 콤보박스 설정

            grdMain.ExtendLastCol = true;

        }

        #endregion

        #region 조회 설정

        private bool SetDataBinding()
        {
            try
            {

                string start_date = start_dt.Value.ToString();
                start_date = (start_date.Substring(0, 4) + start_date.Substring(5, 2) + start_date.Substring(8, 2));

                string end_date = end_dt.Value.ToString();
                end_date = (end_date.Substring(0, 4) + end_date.Substring(5, 2) + end_date.Substring(8, 2));

                poctxt.Value = poctxt.Text;
                heattxt.Value = heattxt.Text;
                

                string sql1 = string.Format(" SELECT  ROWNUM AS L_NO ");
                sql1 += string.Format("       ,'False' AS P_YN ");
                sql1 += string.Format("       ,3 AS P_CNT ");
                sql1 += string.Format("       ,MFG_DATE ");
                sql1 += string.Format("       ,POC_NO ");
                sql1 += string.Format("       ,BUNDLE_NO ");
                sql1 += string.Format("       ,HEAT ");
                sql1 += string.Format("       ,STEEL ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = X.STEEL) AS STEEL_NM ");
                //sql1 += string.Format("       ,ITEM ");
                sql1 += string.Format("       ,ITEM_SIZE ");
                sql1 += string.Format("       ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'RB' AND CD_ID = X.ITEM_SIZE) AS ITEM_NM ");
                sql1 += string.Format("       ,LENGTH ");
                sql1 += string.Format("       ,PCS ");
                sql1 += string.Format("       ,THEORY_WGT ");
                sql1 += string.Format("       ,MPI_PCS ");
                sql1 += string.Format("       ,MPI_WGT ");
                sql1 += string.Format("       ,MPI_DDTT ");
                sql1 += string.Format("       ,GR_PCS ");
                sql1 += string.Format("       ,GR_WGT ");
                sql1 += string.Format("       ,GR_DDTT ");
                sql1 += string.Format("       ,END_PCS ");
                sql1 += string.Format("       ,END_WGT ");
                sql1 += string.Format("       ,END_DDTT ");
                sql1 += string.Format("       ,LOCATION ");
                sql1 += string.Format("       ,PROCESS ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT  "); //--계중번호
                sql1 += string.Format("                TO_CHAR(TO_DATE(B.MFG_DATE, 'YYYYMMDD'), 'YYYY-MM-DD') AS MFG_DATE  "); //--계중일시
                sql1 += string.Format("               ,B.POC_NO ");
                sql1 += string.Format("               ,B.BUNDLE_NO ");
                sql1 += string.Format("               ,B.HEAT ");
                sql1 += string.Format("               ,B.STEEL ");
                sql1 += string.Format("               ,B.ITEM ");
                sql1 += string.Format("               ,B.ITEM_SIZE ");
                sql1 += string.Format("               ,B.LENGTH ");
                sql1 += string.Format("               ,B.PCS ");
                sql1 += string.Format("               ,B.THEORY_WGT ");
                sql1 += string.Format("               ,B.MPI_PCS ");
                sql1 += string.Format("               ,B.MPI_WGT ");
                sql1 += string.Format("               ,TO_CHAR(TO_DATE(B.MPI_DDTT, 'YYYYMMDDHH24MISS'), 'YYYY-MM-DD HH24:MI') AS MPI_DDTT ");
                sql1 += string.Format("               ,B.GR_PCS ");
                sql1 += string.Format("               ,B.GR_WGT ");
                sql1 += string.Format("               ,TO_CHAR(TO_DATE(B.GR_DDTT, 'YYYYMMDDHH24MISS'), 'YYYY-MM-DD HH24:MI') AS GR_DDTT ");
                sql1 += string.Format("               ,B.END_PCS ");
                sql1 += string.Format("               ,B.END_WGT ");
                sql1 += string.Format("               ,TO_CHAR(TO_DATE(B.END_DDTT, 'YYYYMMDDHH24MISS'), 'YYYY-MM-DD HH24:MI') AS END_DDTT ");
                sql1 += string.Format("               ,B.LOCATION ");
                sql1 += string.Format("               ,B.PROCESS ");
                sql1 += string.Format("        FROM   TB_WIP_WR  B ");
                if (cd_id2 == "F2")
                {
                    sql1 += string.Format("        WHERE    B.MFG_DATE   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                }
                if (cd_id2 == "H2")
                {
                    sql1 += string.Format("        WHERE    SUBSTR(B.GR_DDTT,1,8)   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                }
                if (cd_id2 == "K2")
                {
                    sql1 += string.Format("        WHERE    SUBSTR(B.END_DDTT,1,8)   BETWEEN '{0}' AND '{1}' ", start_date, end_date);    //:P_FR_DATE AND :P_TO_DATE
                }
                sql1 += string.Format("        AND    B.LINE_GP    = '{0}' ", cd_id); //:P_LINE_GP
                sql1 += string.Format("        AND    B.POC_NO     LIKE '%{0}%' || '%'", poctxt.Text);    //:P_POC_NO
                sql1 += string.Format("        AND    B.HEAT       LIKE '%{0}%' || '%'", heattxt.Text);  //:P_HEAT
                sql1 += string.Format("        AND    B.ITEM_SIZE       LIKE '%{0}%' || '%'", txtitem_size);  //:ITEM_SIZE
                sql1 += string.Format("        AND    B.STEEL       LIKE '%{0}%' || '%'", gangjung_id);  //:STEEL
                sql1 += string.Format("        ORDER BY  1 asc ,2 asc,3 asc,4 desc,5 desc ");
                sql1 += string.Format("        ) X ");

                olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                Cursor = Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                Cursor = Cursors.Default;
                grdMain.AutoResize = true;
                grdMain.AutoSizeCols();

                if (moddt.Rows.Count > 0)
                {
                    UpdateTotals();
                    int gubun_index = grdMain.Cols["POC_NO"].Index;
                    grdMain.Rows[2].StyleNew.BackColor = Color.LightYellow;
                    grdMain.Rows[2].StyleNew.ForeColor = Color.Black;
                    grdMain.SetData(2, gubun_index, "합계");
                }


                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), moddt.Rows.Count.ToString(), "건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.Message + "]");
                return false;
            }
            return true;
        }

        private void UpdateTotals()
        {

            subtotalNo = 0;

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["THEORY_WGT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["MPI_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["MPI_WGT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["GR_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["GR_WGT"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["END_PCS"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["END_WGT"].Index, "합계");
            


            //AddSubtotalNo();
            //grdMain.Rows.Frozen = GetAvailMinRow(grdMain) -1;
            //grdMain.Subtotal(AggregateEnum.Average, 1, -1, grdMain.Cols["THEORY_WGT"].Index, "평균");

            grdMain.AutoSizeCols();

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


        #endregion 조회 설정
        private void dolabel()
        {
            var Reprint_CNT = 0;
            var Tcnt = 0;
            string selected = string.Empty;

            for (int i = 0; i < grdMain.Rows.Count; i++)
            {
                selected = string.Empty;

                if (grdMain.GetData(i, "P_YN") == null)
                {
                    selected = "False";
                }
                else
                {
                    selected = grdMain.GetData(i, "P_YN").ToString();
                }

                if (selected == "True")
                {
                    Reprint_CNT += 1;
                }
            }

            modBarCodePrint a = new modBarCodePrint();

            //바코드 출력 확인
            DialogResult result = MessageBox.Show("바코드 출력 하시겠습니까?", "출력 확인", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            selected = string.Empty;

            for (var i = 1; i < grdMain.Rows.Count; i++)
            {
                if (grdMain.GetData(i, "P_YN") == null)
                {
                    selected = "False";
                }
                else
                {
                    selected = grdMain.GetData(i, "P_YN").ToString();
                }

                if (selected == "True")
                {
                    //if (grdMain.GetData(i, "P_YN") != null)
                    //{
                    //string prtINSPECT_DATE = grdMain.GetData(i, "INSPECT_DATE").ToString();
                    // prtCOMPANY = g_initVal.COMPANY;
                    //string prtCOMPANY = grdMain.GetData(i, "PO_CUST_ENG_NAME").ToString();

                    //string prtITEM = grdMain.GetData(i, "ITEM").ToString();
                    string prtSIZE = grdMain.GetData(i, "ITEM_NM").ToString();
                    string prtSTEEL = grdMain.GetData(i, "STEEL_NM").ToString();
                    string prtHEAT = grdMain.GetData(i, "HEAT").ToString();
                    //string prtEMPLOYEE = grdMain.GetData(i, "INITIAL1").ToString();
                    //string prtAFTER_ROUTING = grdMain.GetData(i, "AFTER_ROUTING").ToString();
                    //string prtSURFACE_LEVEL = grdMain.GetData(i, "SURFACE_LEVEL").ToString();
                    //string prtITEM_USE = grdMain.GetData(i, "ITEM_USE_NAME").ToString();
                    //string prtSIRF_CODE = grdMain.GetData(i, "SIRF_CODE").ToString();
                    //string prtUOM = grdMain.GetData(i, "LENGTHUOM").ToString();
                    string prtLENGTH = grdMain.GetData(i, "LENGTH").ToString();
                    string prtWEIGHT = grdMain.GetData(i, "THEORY_WGT").ToString();
                    string prtBONSU = grdMain.GetData(i, "PCS").ToString();

                    //2015.06.30 구성욱 추가
                    string prtBUNDLE_NO = grdMain.GetData(i, "BUNDLE_NO").ToString();
                    string prtITEM_SIZE = grdMain.GetData(i, "ITEM_SIZE").ToString();
                    //string prtCOUNTRY_BASIS = grdMain.GetData(i, "COUNTRY_BASIS").ToString();
                    //string prtSPEC_NUM = grdMain.GetData(i, "SPEC_NUM").ToString();
                    //string prtCERTIFY_NUM = grdMain.GetData(i, "CERTIFY_NUM").ToString();

                    

                    if (!(prtLENGTH == null || prtLENGTH.Trim() == ""))
                    {
                        //prtLENGTH = (double.Parse(prtLENGTH) / 100).ToString();
                        //prtLENGTH = (double.Parse(prtLENGTH)).ToString("N2");
                        prtLENGTH = (double.Parse(prtLENGTH)).ToString();
                    }

                    //if ((prtLENGTH.Trim() == "6"))
                    //{
                    //    //prtLENGTH = (double.Parse(prtLENGTH) / 100).ToString();
                    //    prtLENGTH = "6.00";
                    //}

                    //if ((prtLENGTH.Trim() == "5"))
                    //{
                    //    //prtLENGTH = (double.Parse(prtLENGTH) / 100).ToString();
                    //    prtLENGTH = "5.00";
                    //}

                    //if ((prtLENGTH.Trim() == "7"))
                    //{
                    //    //prtLENGTH = (double.Parse(prtLENGTH) / 100).ToString();
                    //    prtLENGTH = "7.00";
                    //}
                    /*
                    if (!(prtWEIGHT == null || prtWEIGHT.Trim() == ""))
                    {
                        prtWEIGHT = prtWEIGHT;
                    }

                    if (!(prtBONSU == null || prtBONSU.Trim() == ""))
                    {
                        prtBONSU = prtBONSU;
                    }
                    */

                    a.BARCODE_PRINT_COUNT = 1;

                    var j = 1;
                    var sROUND = "";
                    sROUND = "RB_ROUND_MID5";                   //40파이

                    // 		 BARCODE_COUNTRY_BASIS[j] = "";
                   // a.BARCODE_COUNTRY_BASIS = prtCOUNTRY_BASIS;
                    a.BARCODE_SIZE_CODE = "";
                    //a.BARCODE_LABEL_SIZE_NAME = prtSPEC_NUM;
                    //a.BARCODE_APPORVAL_NO = prtCERTIFY_NUM;
                    //a.BARCODE_INSPECT_DATE = prtINSPECT_DATE;
                    //a.BARCODE_ITEM = prtITEM;
                    a.BARCODE_ITEM_SIZE = prtITEM_SIZE;
                    a.BARCODE_SIZE_NAME = prtSIZE;
                    a.BARCODE_STEEL = prtSTEEL;
                    a.BARCODE_PRINTED_BASIS = "";
                    a.BARCODE_INSPECTER_NAME = "";
                    a.BARCODE_HEAT = prtHEAT;
                    a.BARCODE_MARKET_TYPE = "";
                    a.BARCODE_UOM = "";
                    //a.BARCODE_SIRF_CODE = prtSIRF_CODE;
                    a.BARCODE_Length = prtLENGTH;
                    a.BARCODE_BONSU = prtBONSU;
                    a.BARCODE_WEIGHT = prtWEIGHT;
                    a.BARCODE_BUNDLE_NO = prtBUNDLE_NO;
                    //a.BARCODE_COMPANY = prtCOMPANY;
                    //a.BARCODE_AFTER_ROUTING = prtAFTER_ROUTING;
                    //a.BARCODE_SURFACE_LEVEL = prtSURFACE_LEVEL;
                    //a.BARCODE_ITEM_USE = prtITEM_USE;

                    // 매수 만큼 돌면서 출력한다.
                    var nMaesu = grdMain.GetData(i, "P_CNT").ToString();
                    int Maesu = int.Parse(nMaesu);

                    for (var k = 0; k < int.Parse(nMaesu); k++)
                    {
                        a.PrintStart(sROUND, "LPT1", Maesu);
                        System.Threading.Thread.Sleep(200);
                    }
                     // End if chk2   
                    //}
                }
            }


            btnDisplay_Click(this, new EventArgs());

            //window.setTimeout('fn_close()', 2000);
        }
        #region 콤보박스 설정
        private void SetComboBox1()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
        }
        //private void SetComboBox2()
        //{
        //    cd.SetCombo(cbo_Work_Type, "WORK_TYPE", "", true);
        //}
        //private void SetComboBox3()
        //{
        //    cd.SetCombo(cboTEAM, "WORK_TEAM", "", true);
        //}
        #endregion 콤보박스 설정

        #region 이벤트 설정

        private void Button_Click(object sender, EventArgs e)
        {

            switch (((Button)sender).Name)
            {
                case "btnDisplay":
                    cd.InsertLogForSearch(ck.UserID, btnDisplay);
                    setTextValue();
                    SetDataBinding();

                    break;

                case "btnExcel":
                    SaveExcel();
                    break;
            }
        }

            #region 그리드 이벤트 설정
        private void GrdMain_RowColChange(object sender, EventArgs e)
        {
            int maxrow = 0;
            int oldSel = 0;
            string str = string.Empty;
            string temp = string.Empty;

            selectedrow = grdMain.RowSel;
        }



        private void grdMain_DoubleClick(object sender, EventArgs e)
        {


            if (grdMain.RowSel < GetAvailMinRow(grdMain))
            {
                return;
            }
            sMeasNo = grdMain[grdMain.RowSel, 10].ToString();
            WPIRsltPopUP popup = new WPIRsltPopUP(cboLine_GP.SelectedIndex, sMeasNo, scrAuth);
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            //SetDataBinding();
            Button_Click(btnDisplay, null);

        }

        #endregion 그리드 이벤트 설정

        #region 버튼 설정
        private void btnReg_Click(object sender, EventArgs e)
        {
            WPIRsltPopUP popup = new WPIRsltPopUP(cboLine_GP.SelectedIndex);
            popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();

            //SetDataBinding();
            Button_Click(btnDisplay, null);

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            

        }

        private void btnLabel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //발행 함수 호출
            dolabel();

            this.Cursor = Cursors.Default;

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + "발행이 완료되었습니다.");
        }
        #endregion 버튼 설정

        #region 컨트롤 설정


        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        //사용자이벤트 생성

        //강종ID(LostFocus)


        private void CboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd_id = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = cd_id;
        }


        private void txtItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

        private void SaveExcel()
        {
            vf.SaveExcel(titleNM, grdMain);
        }

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        private void setTextValue()
        {

            txtheat = heattxt.Text;
            txtpoc = poctxt.Text;
            txtitem_size = txtItemSize.Text;
   

        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = 3; i < grdMain.Rows.Count; i++)
            {
                if (chkAll.Checked == true)
                {
                    grdMain.SetData(i, "P_YN", true);
                }
                else
                {
                    grdMain.SetData(i, "P_YN", false);
                }
            }
        }
        #endregion 컨트롤 설정

        #endregion

        private void btnDisplay_Click(object sender, EventArgs e)
        {

        }
    }
}
