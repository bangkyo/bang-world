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
using SystemControlClassLibrary.UC.sub_UC;
using ComLib.clsMgr;
using C1.Win.C1FlexGrid;
using static ComLib.clsUtil;
using System.Collections.Specialized;
using System.Data.OracleClient;

namespace SystemControlClassLibrary.Results
{
    public partial class StopCrBnews : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();

        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();

        clsStyle cs = new clsStyle();

        ListDictionary stop_RSN_CD_Value = null; 
        ListDictionary stop_GP_Value = null;
        ListDictionary stop_cd_Value = null;
        ListDictionary stop_cd_Value1 = null;

        private string start_dt_str = "";
        private string end_dt_str = "";

        private int start_dt_str1 = 0;
        private int end_dt_str1 = 0;

        private string STOP_GP_id = "";
        private string STOP_RSN_CD_id = "";

        private int subtotalNo;
        DataTable olddt;
        DataTable moddt;

        DataTable olddt_Insert;
        DataTable moddt_Insert;

        private string line_gp_id = "";

        // 셀의 수정전 값
        private static Object strBefValue = "";

        private string ownerNM = "";
        private string titleNM = "";
        private int rowValue = 0;

        //private UC.sub_UC.UC_Line_gp_s uC_Line_gp_s1;
        //private UC.sub_UC.UC_Work_Day uC_Work_Day1;
        //private UC.sub_UC.UC_Work_Type_s uC_Work_Type1;
        private TextBox tbNo;
        private DateTimePicker dtpTime;
        #endregion 변수 설정

        #region 생성자, 로딩 설정
        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        public StopCrBnews(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }

        private void StopCrBnews_Load(object sender, EventArgs e)
        {
            InitControl();

            btnDisplay_Click(null, null);
        }
        #endregion 생성자, 로딩 설정

        #region CONTROL 설정
        private void InitControl()
        {
            cs.InitPicture(pictureBox1);

            cs.InitTitle(title_lb, ownerNM, titleNM);

            cs.InitPanel(panel1);

            cs.InitButton(btnSave);

            cs.InitButton(btnDisplay);

            cs.InitButton(btnClose);

            cs.InitButton(btnRowAdd);

            cs.InitButton(btnDelRow);

            cs.InitButton(btnExcel);

            cs.InitCombo(cboLine_GP, StringAlignment.Near);
            cs.InitCombo(cboSTOP_GP, StringAlignment.Near);
            cs.InitCombo(cboSTOP_RSN_CD, StringAlignment.Near);


            SetComboBox1();
            SetComboBox2();
            SetComboBox3();

            #region 유저컨트롤 설정


            int location_x = 0;
            int location_y = 0;

            start_dt.Value = DateTime.Now;
            end_dt.Value = DateTime.Now;
            start_dt.ValueChanged += Start_dt_ValueChanged;
            end_dt.ValueChanged += End_dt_ValueChanged;
            //
            // uC_Line_gp_s1
            // 
            //uC_Line_gp_s1 = new UC_Line_gp_s();
            //uC_Line_gp_s1.BackColor = System.Drawing.Color.Transparent;
            //uC_Line_gp_s1.cb_Enable = true;
            ////uC_Line_gp_s1.Line_GP = ck.Line_gp;
            //uC_Line_gp_s1.Location = new System.Drawing.Point(333 + 50 + location_x, 7 + location_y); //location_x
            //uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            //uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            //uC_Line_gp_s1.TabIndex = 1;
            //// 
            //// uC_Work_Day1
            //// 
            //uC_Work_Day1 = new UC_Work_Day();
            //uC_Work_Day1.BackColor = System.Drawing.Color.Transparent;
            //uC_Work_Day1.Location = new System.Drawing.Point(26 + 24 + location_x, 7 + location_y); //(26 + 24 , 7 + location_y); //location_x
            //uC_Work_Day1.Name = "uC_Work_Day1";
            //uC_Work_Day1.Size = new System.Drawing.Size(270, 27);
            //uC_Work_Day1.TabIndex = 2;
            //uC_Work_Day1.Work_Day = DateTime.Now.Date;

            //// 
            //// uC_Work_Type1
            //// 
            //uC_Work_Type1 = new UC_Work_Type_s();
            //uC_Work_Type1.BackColor = System.Drawing.Color.Transparent;
            //uC_Work_Type1.cb_Enable = true;
            //uC_Work_Type1.Work_Type = "1";
            //uC_Work_Type1.Location = new System.Drawing.Point(570 , 2);
            //uC_Work_Type1.Name = "uC_Work_Type1";
            //uC_Work_Type1.Size = new System.Drawing.Size(203, 27);
            //uC_Work_Type1.TabIndex = 3;

            //panel1.Controls.Add(this.uC_Work_Day1);
            //panel1.Controls.Add(this.uC_Line_gp_s1);
            //panel1.Controls.Add(this.uC_Work_Type1);

            //uC_Line_gp_s1.Line_GP = ck.Line_gp;

            InitGrd_Main();

            #endregion
        }

        #endregion CONTROL 설정

        #region 그리드 설정
        private void InitGrd_Main()
        {
            #region 업데이트 가능 컬럼 캡션 색 표현 설정

            cs.InitGrid_search(grdMain, false, 2, 1);

            var crCellRange = grdMain.GetCellRange(1, grdMain.Cols["START_DDTT"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["WORK_DATE"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(1, grdMain.Cols["END_DDTT"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["STOP_RSN_CD"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["STOP_GP"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["WORK_CNTS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(0, grdMain.Cols["WORK_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(0, grdMain.Cols["NG_PCS"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            //crCellRange = grdMain.GetCellRange(0, grdMain.Cols["PACKING_RE_QTY"].Index);
            //crCellRange.Style = grdMain.Styles["ModifyStyle"];

            crCellRange = grdMain.GetCellRange(0, grdMain.Cols["REMARKS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            #endregion 업데이트 가능 컬럼 캡션 색 표현 설정

            grdMain.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            grdMain.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;

            grdMain.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;

            int level1 = 50; // 2자리
            int level2 = 50; // 4자리
            int level2_5 = 80;
            int level3 = 100; // 6자리
            int level4 = 140; // 8자리이상

            #region 1. grdMain head 및 row  align 설정
            grdMain[1, "L_NO"] = grdMain.Cols["L_NO"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "OCC_SEQ"] = grdMain.Cols["OCC_SEQ"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "WORK_DATE"] = grdMain.Cols["WORK_DATE"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STOP_RSN_CD"] = grdMain.Cols["STOP_RSN_CD"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "STOP_GP"] = grdMain.Cols["STOP_GP"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain.Cols["WORK_CNTS"].Caption = "작업내용" + "\r\n" + "(정지내역)";
            //grdMain[1, "WORK_CNTS"] = "작업내용" + "\r\n" + "(정지내역)" ;
            grdMain.Cols["WORK_CNTS"].Caption = "정지 사유";
            grdMain[1, "WORK_CNTS"] = "정지 사유";
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "WORK_PCS"] = grdMain.Cols["WORK_PCS"].Caption;
            //grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "NG_PCS"] = grdMain.Cols["NG_PCS"].Caption;
            //grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            //grdMain[1, "PACKING_RE_QTY"] = grdMain.Cols["PACKING_RE_QTY"].Caption;
            //grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "REMARKS"] = grdMain.Cols["REMARKS"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);

            grdMain[1, "GUBUN"] = grdMain.Cols["GUBUN"].Caption;
            grdMain.Rows[1].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);



            grdMain[1, "START_DDTT"] = "정지시작";
            grdMain[1, "END_DDTT"] = "정지종료";
            grdMain[1, "TOTAL"] = "정지시간";
            grdMain[1, "TOTAL_A"] = "비조업";
            grdMain[1, "PER"] = "가동율";

            grdMain.Cols["L_NO"].Width = 0;
            grdMain.Cols["OCC_SEQ"].Width = 0;
            grdMain.Cols["WORK_DATE"].Width = 110;
            //grdMain.Cols["WORK_TYPE_NM"].Width = level2;
            grdMain.Cols["WORK_CNTS"].Width = 220;
            //grdMain.Cols["WORK_PCS"].Width = 70;
            //grdMain.Cols["NG_PCS"].Width = 70;
            //grdMain.Cols["PACKING_RE_QTY"].Width = 70;
            grdMain.Cols["REMARKS"].Width = 70;
            grdMain.Cols["GUBUN"].Width = 0;

            grdMain.Cols["STOP_RSN_CD"].Width = 100;
            grdMain.Cols["STOP_GP"].Width = 90;

            grdMain.Cols["START_DDTT"].Width = level3;
            grdMain.Cols["END_DDTT"].Width = level3;
            grdMain.Cols["TOTAL"].Width = level3;


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            grdMain.Rows[1].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            //grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["WORK_CNTS"].TextAlign = cs.WORK_CNTS_TextAlign;
           // grdMain.Cols["WORK_PCS"].TextAlign = cs.PCS_TextAlign;
            //grdMain.Cols["NG_PCS"].TextAlign = cs.PCS_TextAlign;
            //grdMain.Cols["PACKING_RE_QTY"].TextAlign = cs.PACKING_RE_QTY_TextAlign;
            grdMain.Cols["REMARKS"].TextAlign = cs.REMARKS_TextAlign;

            grdMain.Cols["START_DDTT"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["END_DDTT"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["TOTAL"].TextAlign = cs.TOTAL_TextAlign;


            #endregion

            grdMain.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly;
            for (int i = 0; i < grdMain.Cols.Count; i++)
            {
                grdMain.Cols[i].AllowMerging = true;

            }

            grdMain.Rows[0].AllowMerging = true;

            grdMain.AllowEditing = true;
            grdMain.Rows[0].AllowEditing = false;
            grdMain.Rows[1].AllowEditing = false;
            

            grdMain.Cols["L_NO"].AllowEditing = false;
            grdMain.Cols["TOTAL"].AllowEditing = false;
            grdMain.Cols["WORK_DATE"].AllowEditing = true;

            grdMain.Cols["WORK_CNTS"].AllowEditing = true;
            //grdMain.Cols["WORK_PCS"].AllowEditing = true;
            //grdMain.Cols["NG_PCS"].AllowEditing = true;
            //grdMain.Cols["PACKING_RE_QTY"].AllowEditing = true;
            grdMain.Cols["REMARKS"].AllowEditing = true;

            grdMain.Cols["START_DDTT"].AllowEditing = true;
            grdMain.Cols["END_DDTT"].AllowEditing = true;

            grdMain.Cols["STOP_RSN_CD"].AllowEditing = true;
            grdMain.Cols["STOP_GP"].AllowEditing = true; 

            SetComboinGrd();

            grdMain.Cols["STOP_RSN_CD"].DataMap = stop_RSN_CD_Value;
            grdMain.Cols["STOP_RSN_CD"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["STOP_GP"].DataMap = stop_GP_Value;
            grdMain.Cols["STOP_GP"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["WORK_CNTS"].DataMap = stop_cd_Value;
            grdMain.Cols["WORK_CNTS"].TextAlign = TextAlignEnum.LeftCenter;
            //grdMain.Cols["WORK_CNTS"].TextAlign = TextAlignEnum.CenterCenter;


            // 5자리수의 정수 입력
            // oracle number(5)
            //tbNo = new TextBox();
            //cs.InitTextBox(tbNo);
            //tbNo.MaxLength = 5;
            //tbNo.KeyPress += vf.KeyPressEvent_number;
            //tbNo.TextAlign = HorizontalAlignment.Right;

            //grdMain.Cols["WORK_PCS"].Editor = tbNo;
            //grdMain.Cols["NG_PCS"].Editor = tbNo;
            //grdMain.Cols["PACKING_RE_QTY"].Editor = tbNo;

            dtpTime = new DateTimePicker();
            cs.InitDateTimePicker(dtpTime);


            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "  HH:mm";
            dtpTime.Value = DateTime.Now;
            dtpTime.ShowUpDown = true;

            grdMain.Cols["START_DDTT"].Editor = dtpTime;
            grdMain.Cols["END_DDTT"].Editor = dtpTime;

        }
        #endregion 그리드 설정
        private void SetComboBox1()
        {
            cd.SetCombo(cboLine_GP, "LINE_GP", "", false, ck.Line_gp);
        }

        private void SetComboBox2()
        {
            cd.SetCombo(cboSTOP_GP, "STOP_GP_NEW", "", true);
        }

        private void SetComboBox3()
        {
            cd.SetCombo(cboSTOP_RSN_CD, "STOP_RSN_CD_NEW", "", true);
        }
        #region 콤보박스 설정
        private bool SetComboinGrd()
        {

           
            try
            {
                stop_RSN_CD_Value = new ListDictionary();
                DataTable dt1 = cd.Find_CD("STOP_RSN_CD_NEW");
                foreach (DataRow row in dt1.Rows)
                {
                    stop_RSN_CD_Value.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                stop_GP_Value = new ListDictionary();
                DataTable dt2 = cd.Find_CD("STOP_GP_NEW");

                foreach (DataRow row in dt2.Rows)
                {
                    stop_GP_Value.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                stop_cd_Value = new ListDictionary();
                DataTable dt3 = cd.Find_CD("STOP_CD");

                foreach (DataRow row in dt3.Rows)
                {
                    //stop_cd_Value.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    stop_cd_Value.Add(row["CD_ID"].ToString(), row["CD_ID"].ToString() + "  " + row["CD_NM"].ToString());
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion 콤보박스 설정

        private void End_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        private void cboSTOP_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            STOP_GP_id = ((DictionaryList)cboSTOP_GP.SelectedItem).fnValue;
        }

        private void cboSTOP_RSN_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            STOP_RSN_CD_id = ((DictionaryList)cboSTOP_RSN_CD.SelectedItem).fnValue;
        }

        private void Start_dt_ValueChanged(object sender, EventArgs e)
        {
            var modifiedDateEditor = sender as DateTimePicker;

            cs.ReArrageDateEdit(modifiedDateEditor, start_dt, end_dt);
        }

        #region Value Changed 이벤트 설정
        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_dt_str = clsUtil.Utl.GetDateFormat(3, start_dt.Value);
            start_dt_str1 = Convert.ToInt32(start_dt_str);
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            end_dt_str = clsUtil.Utl.GetDateFormat(3, end_dt.Value);
            end_dt_str1 = Convert.ToInt32(end_dt_str);
        }

        private void cboLine_GP_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp_id = ((DictionaryList)cboLine_GP.SelectedItem).fnValue;
            ck.Line_gp = line_gp_id;
        }
        #endregion Value Changed 이벤트 설정

        #region 가동속보 조회 설정(SQL)
        private void setDataBinding()
        {
            string sql1 = string.Empty;

            //start_dt_str = clsUtil.Utl.GetDateFormat(3, start_dt.Value);
            //start_dt_str1 = Convert.ToInt32(start_dt_str);
            //end_dt_str = clsUtil.Utl.GetDateFormat(3, end_dt.Value);
            //end_dt_str1 = Convert.ToInt32(start_dt_str);
            int minus = end_dt_str1 - start_dt_str1 + 1;

            sql1  = string.Format("SELECT ROWNUM AS L_NO ");
            sql1 += string.Format("       ,LINE_GP ");
            sql1 += string.Format("       ,WORK_DATE ");
            sql1 += string.Format("       ,OCC_SEQ ");
            sql1 += string.Format("       ,START_DDTT ");
            sql1 += string.Format("       ,END_DDTT  ");
            sql1 += string.Format("       ,TOTAL  ");
            sql1 += string.Format("       ,TOTAL_A  ");
            sql1 += string.Format("       , STOP_RSN_CD ");
            sql1 += string.Format("       , STOP_GP ");
            sql1 += string.Format("       , WORK_CNTS ");
            sql1 += string.Format("       ,REMARKS  ");
            sql1 += string.Format("       , '' AS GUBUN  ");
            sql1 += string.Format("FROM   (       ");
            sql1 += string.Format("        SELECT LINE_GP AS LINE_GP  ");
            sql1 += string.Format("              ,TO_DATE(WORK_DATE, 'YYYYMMDD') AS WORK_DATE  ");
            sql1 += string.Format("              ,TO_CHAR(START_DDTT, 'HH24:MI') AS START_DDTT  ");
            sql1 += string.Format("              ,TO_CHAR(END_DDTT, 'HH24:MI') AS END_DDTT  ");
            //sql1 += string.Format("              , CASE WHEN STOP_RSN_CD IN ('1', '2', '3') THEN TO_CHAR(TRUNC((END_DDTT - START_DDTT) * 24*60,1),990) END AS TOTAL  ");
            //sql1 += string.Format("              , CASE WHEN STOP_RSN_CD IN ('4') THEN TO_CHAR(TRUNC((END_DDTT - START_DDTT) * 24*60,1),990) END AS TOTAL_A  ");
            sql1 += string.Format("              , CASE WHEN STOP_RSN_CD IN ('1', '2', '3') THEN ROUND((END_DDTT - START_DDTT) * 24*60,0) END AS TOTAL  ");
            sql1 += string.Format("              , CASE WHEN STOP_RSN_CD IN ('4') THEN ROUND((END_DDTT - START_DDTT) * 24*60,0) END AS TOTAL_A  ");
            sql1 += string.Format("              , WORK_CNTS AS WORK_CNTS  ");
            sql1 += string.Format("              , REMARKS AS REMARKS  ");
            sql1 += string.Format("              , OCC_SEQ AS OCC_SEQ ");
            sql1 += string.Format("              , STOP_RSN_CD AS STOP_RSN_CD ");
            sql1 += string.Format("              , STOP_GP AS STOP_GP ");
            sql1 += string.Format("        FROM   TB_CR_STOP_BNEWS  ");
            sql1 += string.Format("        WHERE  LINE_GP   = '{0}' ", line_gp_id);
            sql1 += string.Format("        AND    FN_GET_MFG_DATE(TO_CHAR(START_DDTT, 'YYYYMMDDHH24MISS'))  BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("        AND    STOP_GP LIKE '{0}' || '%' ", STOP_GP_id);//P_WORK_TYPE
            sql1 += string.Format("        AND    STOP_RSN_CD LIKE '{0}' || '%' ", STOP_RSN_CD_id);//P_WORK_TYPE
            sql1 += string.Format("        order by WORK_DATE, START_DDTT  ");
            sql1 += string.Format("        ) X  ");

            olddt = cd.FindDataTable(sql1);

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(olddt, null, true);

            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (olddt.Rows.Count > 0)
            {
                UpdateTotals();
                int gubun_index = grdMain.Cols["WORK_DATE"].Index;
                grdMain.Rows[2].StyleNew.BackColor = Color.LightYellow;
                grdMain.Rows[2].StyleNew.ForeColor = Color.Black;
                
                //grdMain.Rows[2].StyleNew.BackColor = Color.FromArgb(255, 81, 181, 255);
                //grdMain.Rows[2].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);
                //grdMain.SetData(2, gubun_index, "합계");
                grdMain.SetData(2, 5, "합계");
                grdMain.SetData(2, grdMain.Cols.Count - 1, "구분");
                string total = grdMain.GetData(2, 8).ToString();
                int totals = Convert.ToInt32(total);
                string totala = grdMain.GetData(2, 9).ToString();
                int totalsa = Convert.ToInt32(totala);
                int e = (minus * 1440 - totalsa);
                double es = 100-((totals * 1.0 / e * 1.0 )*100);
                //int e = 100 - ((totals / (minus * 1440 - totalsa)) * 100);
                grdMain.SetData(2, 10, es);
            }

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;
            
        }


        private void setDataBinding1()
        {
            string sql1 = string.Empty;

            //start_dt_str = clsUtil.Utl.GetDateFormat(3, start_dt.Value);
            //start_dt_str1 = Convert.ToInt32(start_dt_str);
            //end_dt_str = clsUtil.Utl.GetDateFormat(3, end_dt.Value);
            //end_dt_str1 = Convert.ToInt32(start_dt_str);
            int minus = end_dt_str1 - start_dt_str1 + 1;

            sql1 = string.Format("SELECT ROWNUM AS L_NO ");
            sql1 += string.Format("       ,LINE_GP ");
            sql1 += string.Format("       ,WORK_DATE ");
            sql1 += string.Format("       ,OCC_SEQ ");
            //sql1 += string.Format("       ,NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = X.WORK_TYPE),WORK_TYPE) AS WORK_TYPE_NM  ");
            sql1 += string.Format("       ,START_DDTT ");
            sql1 += string.Format("       ,END_DDTT  ");
            sql1 += string.Format("       ,TOTAL  ");
            sql1 += string.Format("       ,TOTAL_A  ");
            sql1 += string.Format("       , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STOP_RSN_CD_NEW' AND CD_ID = X.STOP_RSN_CD ) AS STOP_RSN_CD  ");
            sql1 += string.Format("       , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STOP_GP_NEW' AND CD_ID = X.STOP_GP ) AS STOP_GP  ");
            sql1 += string.Format("       , WORK_CNTS||'_'||(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STOP_CD' AND CD_ID = X.WORK_CNTS ) AS WORK_CNTS  ");
            //sql1 += string.Format("       , STOP_RSN_CD ");
            //sql1 += string.Format("       , STOP_GP ");
            //sql1 += string.Format("       , WORK_CNTS ");
            sql1 += string.Format("       ,REMARKS  ");
            sql1 += string.Format("       , '' AS GUBUN  ");
            sql1 += string.Format("FROM   (       ");
            sql1 += string.Format("        SELECT LINE_GP AS LINE_GP  ");
            sql1 += string.Format("              ,TO_DATE(WORK_DATE, 'YYYYMMDD') AS WORK_DATE  ");
            sql1 += string.Format("              ,TO_CHAR(START_DDTT, 'HH24:MI') AS START_DDTT  ");
            sql1 += string.Format("              ,TO_CHAR(END_DDTT, 'HH24:MI') AS END_DDTT  ");
            sql1 += string.Format("              , CASE WHEN STOP_RSN_CD IN ('1', '2', '3') THEN TO_CHAR(TRUNC((END_DDTT - START_DDTT) * 24*60,0),990) END AS TOTAL  ");
            sql1 += string.Format("              , CASE WHEN STOP_RSN_CD IN ('4') THEN TO_CHAR(TRUNC((END_DDTT - START_DDTT) * 24*60,0),990) END AS TOTAL_A  ");
            sql1 += string.Format("              , WORK_CNTS AS WORK_CNTS  ");
            sql1 += string.Format("              , REMARKS AS REMARKS  ");
            sql1 += string.Format("              , OCC_SEQ AS OCC_SEQ ");
            sql1 += string.Format("              , STOP_RSN_CD AS STOP_RSN_CD ");
            sql1 += string.Format("              , STOP_GP AS STOP_GP ");
            sql1 += string.Format("        FROM   TB_CR_STOP_BNEWS  ");
            sql1 += string.Format("        WHERE  LINE_GP   = '{0}' ", line_gp_id);
            //sql1 += string.Format("        AND    WORK_DATE  BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("        AND    AND FN_GET_MFG_DATE(TO_CHAR(END_DDTT, 'YYYYMMDDHH24MISS'))  BETWEEN '{0}' AND '{1}' ", start_dt_str, end_dt_str);
            sql1 += string.Format("        AND    STOP_GP LIKE '{0}' || '%' ", STOP_GP_id);//P_WORK_TYPE
            sql1 += string.Format("        AND    STOP_RSN_CD LIKE '{0}' || '%' ", STOP_RSN_CD_id);//P_WORK_TYPE
            //sql1 += string.Format("        GROUP BY ROLLUP (START_DDTT)  ");
            sql1 += string.Format("        order by WORK_DATE, START_DDTT  ");
            sql1 += string.Format("        ) X  ");

            olddt = cd.FindDataTable(sql1);

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            grdMain.SetDataBinding(olddt, null, true);

            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (olddt.Rows.Count > 0)
            {
                UpdateTotals();
                int gubun_index = grdMain.Cols["WORK_DATE"].Index;
                grdMain.Rows[2].StyleNew.BackColor = Color.LightYellow;
                grdMain.Rows[2].StyleNew.ForeColor = Color.Black;

                //grdMain.Rows[2].StyleNew.BackColor = Color.FromArgb(255, 81, 181, 255);
                //grdMain.Rows[2].StyleNew.Font = new Font("돋움", 11.0f, FontStyle.Bold);
                //grdMain.SetData(2, gubun_index, "합계");
                grdMain.SetData(2, 5, "합계");
                grdMain.SetData(2, grdMain.Cols.Count - 1, "구분");
                string total = grdMain.GetData(2, 8).ToString();
                int totals = Convert.ToInt32(total);
                string totala = grdMain.GetData(2, 9).ToString();
                int totalsa = Convert.ToInt32(totala);
                int e = (minus * 1440 - totalsa);
                double es = 100 - ((totals * 1.0 / e * 1.0) * 100);
                //int e = 100 - ((totals / (minus * 1440 - totalsa)) * 100);
                grdMain.SetData(2, 10, es);
            }

            clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + olddt.Rows.Count.ToString() + " 건 조회 되었습니다.");

            grdMain.Row = -1;

        }
        #endregion 가동속보 조회 설정(SQL)
        private void UpdateTotals()
        {

            subtotalNo = -1;

            // clear existing totals
            grdMain.Subtotal(AggregateEnum.Clear);

            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["TOTAL"].Index, "합계");
            grdMain.Subtotal(AggregateEnum.Sum, subtotalNo, -1, grdMain.Cols["TOTAL_A"].Index, "합계");
            

            AddSubtotalNo();
            grdMain.Rows.Frozen = GetAvailMinRow(grdMain) - 1;
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

        #region 저장 설정
        private void btnSave_Click(object sender, EventArgs e)
        {
            
            string strMsg = string.Empty;

            #region 삭제항목체크
            string check_value1 = string.Empty;
            string check_Cols_NM1 = string.Empty;
            string check_field_NM1 = string.Empty;
            string check_table_NM1 = string.Empty;

            string check_value2 = string.Empty;
            string check_Cols_NM2 = string.Empty;
            string check_field_NM2 = string.Empty;
            string check_table_NM2 = string.Empty;

            string check_NM = string.Empty;
            string gubun_value = string.Empty;
            string show_msg = string.Empty;
            int checkrow = 0;

            bool isChange = false;
            //삭제할 항목이 있는지 파악하고 물어보고 진행
            for (checkrow = 1; checkrow < grdMain.Rows.Count; checkrow++)
            {
                gubun_value = grdMain.GetData(checkrow, grdMain.Cols.Count-1).ToString();

                if (gubun_value == "삭제" || gubun_value == "수정")
                {
                    isChange = true;
                }
                if (gubun_value == "추가")
                {
                    // string work_Date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();
                    #region FROM 시간 체크
                    check_field_NM1 = "START_DDTT";
                    check_table_NM1 = "TB_CR_STOP_BNEWS";
                    check_value1 = grdMain.GetData(checkrow, check_field_NM1).ToString();
                    check_Cols_NM1 = grdMain.Cols[check_field_NM1].Caption;

                    if (string.IsNullOrEmpty(check_value1))
                    {
                        show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM1);
                        MessageBox.Show(show_msg);
                        return;
                    }

                    #endregion FROM 시간 체크 체크

                    //#region WORK_TYPE 체크
                    //check_field_NM2 = "WORK_TYPE_NM";
                    //check_table_NM2 = "TB_CR_OPRN_BNEWS";
                    //check_value2 = grdMain.GetData(checkrow, check_field_NM2).ToString();
                    //check_Cols_NM2 = grdMain.Cols[check_field_NM2].Caption;

                    //if (string.IsNullOrEmpty(check_value2))
                    //{
                    //    show_msg = string.Format("{0}를 입력하세요.", check_Cols_NM2);
                    //    MessageBox.Show(show_msg);
                    //    return;
                    //}
                    //#endregion WORK_TYPE 체크

                    //string work_Date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();

                    //if (vf.Has_Item("TB_CR_OPRN_BNEWS", "START_DDTT", "LINE_GP", "WORK_DATE", "WORK_TYPE", check_value1, uC_Line_gp_s1.Line_GP, work_Date, grdMain.GetData(checkrow, "Work_Type_NM").ToString()))
                    //{
                    //    show_msg = string.Format(" 시간(FROM)이 중복되었습니다. 다시 입력해주세요.");
                    //    MessageBox.Show(show_msg);
                    //    return;
                    //}

                    isChange = true;
                }

            }
            if (isChange)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

            }
            #endregion 삭제항목체크

            int row = 0;
            int InsCnt = 0;
            int UpCnt = 0;
            int DelCnt = 0;
            //List<string> delSublist = null;

            DataTable dt = null;

            //디비선언
            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            try
            {


                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                //string work_Date = vf.Format(uC_Work_Day1.Work_Day, "yyyyMMdd").ToString();

                #region grdMain 저장
                for (row = 1; row < grdMain.Rows.Count; row++)
                {

                    // Update 처리
                    if (grdMain.GetData(row, grdMain.Cols.Count-1).ToString() == "추가")
                    {
                        //sql1 = string.Format("INSERT INTO TB_BUNDLE_PIECESTD (ITEM, ITEM_SIZE, LENGTH, BND_PCS, REMARKS) VALUES('{0}','{1}','{2}','{3}','{4}')",
                        string to_date = vf.Format(DateTime.Now.Date, "yyyyMMdd").ToString();
                        string work_Date = vf.Format(grdMain.GetData(row, "WORK_DATE"), "yyyyMMdd").ToString();
                        string START_DDTT = grdMain.GetData(row, "START_DDTT").ToString();
                        string END_DDTT = grdMain.GetData(row, "END_DDTT").ToString();
                        int add_date = Convert.ToInt32(work_Date);
                        int add_date2 = add_date + 1;
                        string work_Date1 = Convert.ToString(add_date2);
                        int sss = Convert.ToInt32(END_DDTT.Substring(1, 3)) - Convert.ToInt32(START_DDTT.Substring(1, 3));
                        

                        string sql1 = string.Format(" INSERT ");
                            sql1 += string.Format(" INTO TB_CR_STOP_BNEWS( ");
                            sql1 += string.Format("    LINE_GP, ");
                            sql1 += string.Format("    WORK_DATE, ");
                            sql1 += string.Format("    START_DDTT, ");
                            sql1 += string.Format("    END_DDTT, ");
                            sql1 += string.Format("    OCC_SEQ, ");
                            sql1 += string.Format("    STOP_RSN_CD, ");
                            sql1 += string.Format("    STOP_GP, ");
                            sql1 += string.Format("    WORK_CNTS, ");
                            sql1 += string.Format("    REMARKS,    ");
                            sql1 += string.Format("    DATA_GP,   ");
                            sql1 += string.Format("    REGISTER,  ");
                            sql1 += string.Format("    REG_DDTT  ");
                            sql1 += string.Format("    ) ");
                            sql1 += string.Format("VALUES('{0}', ", line_gp_id);
                            sql1 += string.Format("      '{0}', ", work_Date);
                            sql1 += string.Format("      TO_DATE( '{0}' ||'{1}'||'00', 'YYYY-MM-DD HH24:MI:SS'), ", work_Date, grdMain.GetData(row, "START_DDTT"));
                            if (START_DDTT == END_DDTT) 
                            {
                                sql1 += string.Format("      TO_DATE( '{0}' ||'{1}'||'00', 'YYYY-MM-DD HH24:MI:SS'), ", work_Date1, grdMain.GetData(row, "END_DDTT"));
                            }
                            else
                            {
                                if (sss < 0)
                                {
                                sql1 += string.Format("      TO_DATE( '{0}' ||'{1}'||'00', 'YYYY-MM-DD HH24:MI:SS'), ", work_Date1, grdMain.GetData(row, "END_DDTT"));
                                }
                                else
                                {
                                sql1 += string.Format("      TO_DATE( '{0}' ||'{1}'||'00', 'YYYY-MM-DD HH24:MI:SS'), ", work_Date, grdMain.GetData(row, "END_DDTT"));
                            }
                            }
                            sql1 += string.Format("      (SELECT NVL(MAX(OCC_SEQ),0) + 1 ");
                            sql1 += string.Format("        FROM TB_CR_STOP_BNEWS ");
                            sql1 += string.Format("        WHERE  LINE_GP   = '{0}' ", line_gp_id);
                            sql1 += string.Format("            AND    WORK_DATE = '{0}'), ", work_Date);
                            sql1 += string.Format("      '{0}', ", grdMain.GetData(row, "STOP_RSN_CD"));
                            sql1 += string.Format("      '{0}', ", grdMain.GetData(row, "STOP_GP"));
                            sql1 += string.Format("      '{0}', ", grdMain.GetData(row, "WORK_CNTS"));
                            sql1 += string.Format("      '{0}', ", grdMain.GetData(row, "REMARKS"));
                            sql1 += string.Format("      'MAN', ");
                            sql1 += string.Format("      '{0}', ", ck.UserID);
                            sql1 += string.Format("       SYSDATE ");
                            sql1 += string.Format("      ) ");

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();
                            InsCnt++;
                        
 
                    }
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "수정")
                    {

                        string to_date = vf.Format(DateTime.Now.Date, "yyyyMMdd").ToString();
                        string work_Date = vf.Format(grdMain.GetData(row, "WORK_DATE"), "yyyyMMdd").ToString();
                        string START_DDTT = grdMain.GetData(row, "START_DDTT").ToString();
                        string END_DDTT = grdMain.GetData(row, "END_DDTT").ToString();
                        int add_date = Convert.ToInt32(work_Date);
                        int add_date2 = add_date + 1;
                        string work_Date1 = Convert.ToString(add_date2);
                        //int ssss = Convert.ToInt32(END_DDTT.Substring(0, 4)) - Convert.ToInt32(START_DDTT.Substring(0, 2));
                        //int ssss = Convert.ToInt32(END_DDTT.Substring(0, 4));
                        //int sssss = Convert.ToInt32(START_DDTT.Substring(0, 2));

                        string sql1 = string.Format(" UPDATE TB_CR_STOP_BNEWS ");
                            sql1 += string.Format("SET START_DDTT = TO_DATE( WORK_DATE ||'{0}'||'00', 'YYYY-MM-DD HH24:MI:SS') ", grdMain.GetData(row, "START_DDTT"));
                            //if (START_DDTT == END_DDTT)
                            //{
                            //    sql1 += string.Format(", END_DDTT = TO_DATE( '{0}' ||'{1}'||'00', 'YYYY-MM-DD HH24:MI:SS') ", work_Date1, grdMain.GetData(row, "END_DDTT"));
                            //}
                            //else
                            //{
                            //    if (ssss < 0)
                            //    {
                            //        sql1 += string.Format(", END_DDTT = TO_DATE( '{0}' ||'{1}'||'00', 'YYYY-MM-DD HH24:MI:SS') ", work_Date1, grdMain.GetData(row, "END_DDTT"));
                            //    }
                            //    else
                            //    {
                            //        sql1 += string.Format(", END_DDTT = TO_DATE( '{0}' ||'{1}'||'00', 'YYYY-MM-DD HH24:MI:SS') ", work_Date, grdMain.GetData(row, "END_DDTT"));
                            //    }
                            //}
                            sql1 += string.Format(", END_DDTT = TO_DATE( WORK_DATE ||'{0}'|| '00', 'YYYY-MM-DD HH24:MI:SS')    ", grdMain.GetData(row, "END_DDTT"));
                            sql1 += string.Format(",STOP_RSN_CD = '{0}' ", grdMain.GetData(row, "STOP_RSN_CD"));
                            sql1 += string.Format(",STOP_GP = '{0}' ", grdMain.GetData(row, "STOP_GP"));
                            sql1 += string.Format(",WORK_DATE = '{0}' ", work_Date);
                            sql1 += string.Format(",WORK_CNTS = '{0}'  ", grdMain.GetData(row, "WORK_CNTS"));
                            sql1 += string.Format(",REMARKS = '{0}' ", grdMain.GetData(row, "REMARKS"));
                            sql1 += string.Format(",DATA_GP = 'MAN'  ");
                            sql1 += string.Format("WHERE LINE_GP = '{0}'  ", line_gp_id);
                            sql1 += string.Format("  AND WORK_DATE = '{0}' ", work_Date);
                            sql1 += string.Format("  AND OCC_SEQ = '{0}'    ", grdMain.GetData(row, "OCC_SEQ"));
                            //sql1 += string.Format("WHERE LINE_GP = '{0}'  ", olddt.Rows[row - 1]["LINE_GP"].ToString());
                            //sql1 += string.Format("  AND WORK_DATE = '{0}' ", olddt.Rows[row - 1]["WORK_DATE"].ToString());
                            //sql1 += string.Format("  AND OCC_SEQ = '{0}'    ", grdMain.GetData(row, "OCC_SEQ"));

                            cmd.CommandText = sql1;
                            cmd.ExecuteNonQuery();
                            UpCnt++;
                        
                        
                    }
                    else if (grdMain.GetData(row, grdMain.Cols.Count - 1).ToString() == "삭제")
                    {

                        string work_Date = vf.Format(grdMain.GetData(row, "WORK_DATE"), "yyyyMMdd").ToString();

                        string sql1 = string.Format("  DELETE  ");
                        sql1 += string.Format("FROM TB_CR_STOP_BNEWS ");
                        sql1 += string.Format("WHERE LINE_GP = '{0}'  ", line_gp_id);
                        sql1 += string.Format("  AND WORK_DATE = '{0}' ", work_Date);
                        sql1 += string.Format("  AND OCC_SEQ = '{0}'    ", grdMain.GetData(row, "OCC_SEQ"));


                        // sql1 += string.Format("  AND MAGNET_CURRENT = '{0}' ", grdMain.GetData(row, 4));

                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        DelCnt++;
                    }
                }
                #endregion grdMain 저장
                //실행후 성공
                transaction.Commit();
                btnDisplay_Click(null, null);
                //setDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴
                InitGrd_Main();

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다.");
                return;
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

        }
        #endregion 저장 설정

        #region 이벤트 설정

        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            

            // 수정가능 하도록 열추가
            grdMain.AllowEditing = true;

            grdMain.Cols["L_NO"].AllowEditing = false;
            grdMain.Cols["TOTAL"].AllowEditing = false;
            //grdMain.Cols["WORK_TYPE_NM"].AllowEditing = true; 
            grdMain.Cols["STOP_RSN_CD"].AllowEditing = true;
            grdMain.Cols["STOP_GP"].AllowEditing = true;
            grdMain.Cols["START_DDTT"].AllowEditing = true;
            grdMain.Cols["END_DDTT"].AllowEditing = true;
            grdMain.Cols["WORK_CNTS"].AllowEditing = true;
            //grdMain.Cols["WORK_PCS"].AllowEditing = true;
            //grdMain.Cols["NG_PCS"].AllowEditing = true;
            //grdMain.Cols["PACKING_RE_QTY"].AllowEditing = true;
            grdMain.Cols["REMARKS"].AllowEditing = true;

            //추가 행 데이터 디폴트 값넣기
            grdMain.Rows.Add();

            // 저장시 Insert로 처리하기 위해 flag set
            grdMain.SetData(grdMain.Rows.Count - 1, grdMain.Cols.Count-1, "추가");
            //grdMain.SetData(grdMain.Rows.Count - 1, 0, "추가");
            // Insert 배경색 지정
            grdMain.Rows[grdMain.Rows.Count - 1].Style = grdMain.Styles["InsColor"];

            //// 커서위치 결정
            grdMain.Row = 0;
            grdMain.Col = 0;

        }


        private void btnDelRow_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count < 2 || grdMain.Row < 1 )
            {
                return;
            }
            // 소계와 계는 삭제 불가
            //if (grdMain.Rows[grdMain.Row]["START_DDTT"].ToString() == "소계" || grdMain.Rows[grdMain.Row]["WORK_TYPE_NM"].ToString() == "계")
            //{
            //    return;
            //}
            //mj 추가되었지만 행삭제로 지울때
            

                if (grdMain.Rows[grdMain.Row][grdMain.Cols.Count - 1].ToString() == "추가")
                {
                
                    grdMain.RemoveItem(grdMain.Row);
                    return;
                }
                // 저장시 Delete로 처리하기 위해 flag set
                grdMain.Rows[grdMain.Row]["GUBUN"] = "삭제";
                //grdMain.Rows[grdMain.Row][0] = "삭제";
                

                // Delete 배경색 지정
                grdMain.Rows[grdMain.Row].Style = grdMain.Styles["DelColor"];

                // 커서위치 결정
                grdMain.Row = 0;
                grdMain.Col = 0;

        }
        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            //if (grdMain.Rows[e.Row]["START_DDTT"].ToString() == "소계" || grdMain.GetData(e.Row, "WORK_TYPE_NM").ToString() == "계")
            //{
            //    grdMain.Rows[e.Row].AllowEditing = false;
            //    return;
            //}
            //C1FlexGrid grdMain = sender as C1FlexGrid;
            if (grdMain.Row < 1 || grdMain.GetData(grdMain.Row, grdMain.Col) == null)
            {
                return;
            }

            //min max check

            // 수정여부 확인을 위해 저장
            strBefValue = grdMain.GetData(grdMain.Row, grdMain.Col).ToString();
        }

        private void grdMain_AfterEdit(object sender, RowColEventArgs e)
        {

            // C1FlexGrid selectedGrd = sender as C1FlexGrid;


            int editedRow = e.Row;
            int editedCol = e.Col;

            string set_value = "";
            string stop_gps = grdMain.GetData(editedRow, "STOP_GP").ToString();

            //string stop_gp = grdMain.GetData(editedRow, "STOP_GP").ToString();

            // No,구분은 수정 불가
            if (grdMain.Col == 0 || grdMain.Col == grdMain.Cols.Count - 1)
            {
                grdMain.SetData(grdMain.Row, grdMain.Col, strBefValue);
                return;
            }

            // 수정된 내용이 없으면 Update 처리하지 않는다.
            if (strBefValue == grdMain.GetData(grdMain.Row, grdMain.Col))
                return;


            //if (editedCol == grdMain.Cols["STOP_GP"].Index)
            //{
            //    //if (grdMain.GetData(editedRow, "STOP_GP").ToString() == "1")
            //    //{
            //        stop_cd_Value1 = new ListDictionary();
            //        DataTable dt4 = cd.Find_CD_ST("STOP_CD", stop_gps);

            //        //grdMain.Clear();

            //        foreach (DataRow row in dt4.Rows)
            //        {
            //            //stop_cd_Value.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
            //            //stop_cd_Value.Clear();
            //            stop_cd_Value1.Add(row["CD_ID"].ToString(), row["CD_ID"].ToString() + "  " + row["CD_NM"].ToString());
            //        }
            //        //grdMain.SetData(editedRow, "WORK_CNTS", null);
            //        grdMain.SetData(editedRow, "WORK_CNTS", stop_cd_Value1);

            //        //grdMain.Cols["WORK_CNTS"].DataMap = stop_cd_Value;
            //    //}
            //}

            //   
            // 추가된 열에 대한 수정은 INSERT 처리
            if (grdMain.GetData(grdMain.Row, grdMain.Cols.Count - 1).ToString() != "추가")
            {


                // 저장시 UPDATE로 처리하기 위해 flag set
                grdMain.SetData(grdMain.Row, grdMain.Cols.Count - 1, "수정");
                //grdMain.SetData(grdMain.Row, 0, "수정");

                // Update 배경색 지정
                grdMain.Rows[grdMain.Row].Style = grdMain.Styles["UpColor"];
            }


        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (grdMain.Rows.Count > 2)
            {
                setDataBinding1();
                vf.SaveExcel(titleNM, grdMain);
                setDataBinding();
                //vf.MakeExcelByTempleteRun(uC_Line_gp_s1.Line_GP, titleNM, uC_Work_Day1.Work_Day, grdMain);
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {

            cd.InsertLogForSearch(ck.UserID, btnDisplay);

            setDataBinding();
        }
        #endregion 이벤트 설정

        private void grdMain_StartEdit(object sender, RowColEventArgs e)
        {

        }
    }
}
