using C1.Win.C1FlexGrid;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControlClassLibrary.UC.sub_UC;

namespace SystemControlClassLibrary.Results
{
    public partial class BNDRsltInqPopup : Form
    {
        #region 변수 설정
        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        DataTable olddt;
        DataTable moddt;

        DataTable grdMain_dt;

        UC_OKNG uC_OKNG1;
        UC_STEEL_s uC_STEEL_s1;
        UC_HEAT_s uC_HEAT_s1;
        UC_Item_size_s uC_Item_size_s1;
        UC_POC_s uC_POC_s1;
        UC_Line_gp_s uC_Line_gp_s1;

        string poc_No = string.Empty;
        string line_gp = string.Empty;
        string item_size = string.Empty;

        ArrayList _al = new ArrayList();

        bool allChecked;


        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public BNDRsltInqPopup(string _line_gp, string _poc_No, string _item_size)
        {
            poc_No = _poc_No;
            line_gp = _line_gp;
            item_size = _item_size;

            InitializeComponent();
        }

        private void BNDRsltInqPopup_Load(object sender, EventArgs e)
        {

            MinimizeBox = false;
            MaximizeBox = false;
            InitControl();

            MakeGrdData();

            InitGridData();

            uC_Line_gp_s1.Line_GP = line_gp;
            uC_POC_s1.POC = poc_No;
            uC_Item_size_s1.ITEM_SIZE = item_size;

            if (poc_No.Length >0)
            {
                btnDisplay_Click(null, null);
            }
        }
        #endregion 로딩, 생성자 설정

        #region InitControl 설정
        private void InitControl()
        {
            clsStyle.Style.InitPanel(panel1);
            clsStyle.Style.InitButton(btnDisplay);

            #region 유저컨트롤 설정
            uC_OKNG1 = new UC_OKNG();
            uC_OKNG1.BackColor = System.Drawing.Color.Transparent;
            uC_OKNG1.Location = new System.Drawing.Point(729, 38);
            uC_OKNG1.Name = "uC_OKNG1";
            uC_OKNG1.OKNG = "Y";
            uC_OKNG1.Size = new System.Drawing.Size(202, 27);
            uC_OKNG1.TabIndex = 5;

            uC_STEEL_s1 = new UC_STEEL_s();
            uC_STEEL_s1.BackColor = System.Drawing.Color.Transparent;
            uC_STEEL_s1.Location = new System.Drawing.Point(377, 38);
            uC_STEEL_s1.Name = "uC_STEEL_s1";
            uC_STEEL_s1.Size = new System.Drawing.Size(240, 27);
            uC_STEEL_s1.Steel = "";
            uC_STEEL_s1.Steel_NM = "";
            uC_STEEL_s1.TabIndex = 4;

            uC_HEAT_s1 = new UC_HEAT_s();
            uC_HEAT_s1.BackColor = System.Drawing.Color.Transparent;
            uC_HEAT_s1.HEAT = "";
            uC_HEAT_s1.Location = new System.Drawing.Point(729, 10);
            uC_HEAT_s1.Name = "uC_HEAT_s1";
            uC_HEAT_s1.ReadOnly = false;
            uC_HEAT_s1.Size = new System.Drawing.Size(199, 27);
            uC_HEAT_s1.TabIndex = 3;

            uC_Item_size_s1 = new UC_Item_size_s();
            uC_Item_size_s1.BackColor = System.Drawing.Color.Transparent;
            uC_Item_size_s1.ITEM_SIZE = "";
            uC_Item_size_s1.Location = new System.Drawing.Point(377, 10);
            uC_Item_size_s1.Name = "uC_Item_size_s1";
            uC_Item_size_s1.ReadOnly = false;
            uC_Item_size_s1.Size = new System.Drawing.Size(202, 27);
            uC_Item_size_s1.TabIndex = 2;

            uC_POC_s1 = new UC_POC_s();
            uC_POC_s1.BackColor = System.Drawing.Color.Transparent;
            uC_POC_s1.Location = new System.Drawing.Point(24, 38);
            uC_POC_s1.Name = "uC_POC_s1";
            uC_POC_s1.ReadOnly = false;
            uC_POC_s1.Size = new System.Drawing.Size(181, 27);
            uC_POC_s1.TabIndex = 1;

            uC_Line_gp_s1 = new UC_Line_gp_s();
            uC_Line_gp_s1.BackColor = System.Drawing.Color.Transparent;
            uC_Line_gp_s1.Location = new System.Drawing.Point(24, 8);
            uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            uC_Line_gp_s1.TabIndex = 0;
            uC_Line_gp_s1.cb_Enable = false;

            panel1.Controls.Add(uC_OKNG1);
            panel1.Controls.Add(uC_STEEL_s1);
            panel1.Controls.Add(uC_HEAT_s1);
            panel1.Controls.Add(uC_Item_size_s1);
            panel1.Controls.Add(uC_POC_s1);
            panel1.Controls.Add(uC_Line_gp_s1);

            #endregion

            InitGrd_Main();
        }
        #endregion InitControl 설정

        #region Init Grid Main 설정
        private void InitGrd_Main()
        {
            clsStyle.Style.InitGrid_search(grdMain);

            for (int col = 0; col < grdMain.Cols.Count; col++)
            {
                grdMain.Cols[col].AllowEditing = false;
            }

            grdMain.Cols["CHECKER"].AllowEditing = true;

            var crCellRange = grdMain.GetCellRange(0, grdMain.Cols["CHECKER"].Index);//, 0, grdMain.Cols["REMARKS"].Index);
            crCellRange.Style = grdMain.Styles["ModifyStyle"];

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            grdMain.Cols["CHECKER"].Width = cs.Sel_Width;
            grdMain.Cols["MILL_NO"].Width = cs.Mill_No_Width;
            grdMain.Cols["PIECE_NO"].Width = cs.PIECE_NO_Width;
            grdMain.Cols["MFG_DATE"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TIME"].Width = cs.Date_8_Width;
            grdMain.Cols["WORK_TYPE"].Width = 0;
            grdMain.Cols["WORK_TYPE_NM"].Width = cs.WORK_TYPE_NM_Width;
            grdMain.Cols["POC_NO"].Width = cs.POC_NO_Width;
            grdMain.Cols["HEAT"].Width = cs.HEAT_Width;
            grdMain.Cols["STEEL"].Width = cs.STEEL_Width;
            grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_Width;
            grdMain.Cols["ITEM"].Width = cs.ITEM_Width;
            grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width;
            grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width;
            grdMain.Cols["ZONE_CD"].Width = cs.ZONE_CD_Width;
            grdMain.Cols["CHM_GOOD_NG"].Width = cs.Good_NG_Width;

            grdMain.Cols["LINE_GP"].Width = 0;
            grdMain.Cols["ROUTING_CD"].Width = 0;
            grdMain.Cols["REWORK_SEQ"].Width = 0;
            grdMain.Cols["BUNDLE_NO"].Width = 0;
            grdMain.Cols["GOOD_YN"].Width = cs.Good_NG_L_Width;
            grdMain.Cols["GUBUN"].Width = 0;
            grdMain.Cols["POC_SEQ"].Width = 0;

            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["CHECKER"].TextAlign = cs.SEL_TextAlign;
            grdMain.Cols["MILL_NO"].TextAlign = cs.MILL_NO_TextAlign;
            grdMain.Cols["PIECE_NO"].TextAlign = cs.PIECE_NO_TextAlign;
            grdMain.Cols["MFG_DATE"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TIME"].TextAlign = cs.DATE_TextAlign;
            grdMain.Cols["WORK_TYPE"].TextAlign = cs.WORK_TYPE_TextAlign;
            grdMain.Cols["WORK_TYPE_NM"].TextAlign = cs.WORK_TYPE_NM_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.POC_NO_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.HEAT_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NM"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["ZONE_CD"].TextAlign = cs.ZONE_CD_TextAlign;
            grdMain.Cols["CHM_GOOD_NG"].TextAlign = cs.GOOD_YN_TextAlign;
            grdMain.Cols["LINE_GP"].TextAlign = cs.LINE_GP_TextAlign;
            grdMain.Cols["ROUTING_CD"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["REWORK_SEQ"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["BUNDLE_NO"].TextAlign = TextAlignEnum.CenterCenter;
            grdMain.Cols["GOOD_YN"].TextAlign = TextAlignEnum.CenterCenter;

            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;


            lbSel.Click += SEL_Click;

            _al.Add(new Order.CrtInOrdCre.HostedControl(grdMain, lbSel, 0, grdMain.Cols["CHECKER"].Index));
        }

        private void SEL_Click(object sender, EventArgs e)
        {
            
            if (allChecked)
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "CHECKER", false);
                    
                }
                allChecked = false;

            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdMain.Rows.Count; rowCnt++)
                {
                    grdMain.SetData(rowCnt, "CHECKER", true);
                   
                }
                allChecked = true;

            }
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
        #endregion Init Grid 설정

        #region 조회 설정
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            SetDataBinding();
        }
        #endregion 조회 설정

        #region SetDataBinding 설정
        private void SetDataBinding()
        {
            string show_msg = string.Empty;

            #region 조회 필수입력 확인

            if (string.IsNullOrEmpty(uC_Item_size_s1.ITEM_SIZE))
            {
                show_msg = string.Format("규격(을)를 입력하세요.");
                MessageBox.Show(show_msg);
                return;
            }

            #endregion

            InitGridData();

            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT  TO_CHAR(ROWNUM) AS L_NO ");
                sql1 += string.Format("       ,'False' AS CHECKER ");
                sql1 += string.Format("       ,'' AS GUBUN ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("        SELECT ");
                sql1 += string.Format("                MILL_NO ");
                sql1 += string.Format("               ,PIECE_NO ");
                sql1 += string.Format("               ,TO_DATE(MFG_DATE, 'YYYY-MM-DD') AS MFG_DATE");
                sql1 += string.Format("               ,TO_CHAR(EXIT_DDTT,'HH24:MI:SS') AS WORK_TIME ");
                sql1 += string.Format("               ,WORK_TYPE ");
                sql1 += string.Format("               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'WORK_TYPE' AND CD_ID = A.WORK_TYPE) AS WORK_TYPE_NM ");
                sql1 += string.Format("               ,POC_NO ");
                sql1 += string.Format("               ,HEAT ");
                sql1 += string.Format("               ,STEEL ");
                sql1 += string.Format("               ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("               ,ITEM ");
                sql1 += string.Format("               ,ITEM_SIZE ");
                sql1 += string.Format("               ,LENGTH ");
                sql1 += string.Format("               ,ZONE_CD ");
                sql1 += string.Format("               ,LINE_GP ");
                sql1 += string.Format("               ,ROUTING_CD ");
                sql1 += string.Format("               ,REWORK_SEQ ");
                sql1 += string.Format("               ,BUNDLE_NO ");
                sql1 += string.Format("               , (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'GOOD_NG' AND CD_ID = A.GOOD_YN) AS GOOD_YN ");
                sql1 += string.Format("               ,(SELECT CHM_GOOD_NG FROM TB_CHM_WR  WHERE MILL_NO = A.MILL_NO AND PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                 AND    REWORK_SEQ = A.REWORK_SEQ AND  ROWNUM = 1) AS CHM_GOOD_NG ");//--성분합부
                sql1 += string.Format("               ,NVL(A.POC_SEQ, '') AS POC_SEQ ");   //-- POC_SEQ 추가 
                sql1 += string.Format("        FROM  TB_CR_PIECE_WR A ");
                sql1 += string.Format("        WHERE LINE_GP     = :P_LINE_GP ");
                sql1 += string.Format("        AND   ROUTING_CD  = 'F2' ");
                sql1 += string.Format("        AND   ITEM_SIZE   = :P_ITEM_SIZE ");
                sql1 += string.Format("        AND   HEAT        LIKE '%' || :P_HEAT || '%' ");
                sql1 += string.Format("        AND   POC_NO      LIKE :P_POC_NO || '%' ");
                sql1 += string.Format("        AND   STEEL       LIKE :P_STEEL  || '%' ");
                sql1 += string.Format("        AND   GOOD_YN     = :P_GOOD_NG ");
                sql1 += string.Format("        AND   REWORK_SEQ  = ( SELECT MAX(REWORK_SEQ)  FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                              WHERE  MILL_NO    = A.MILL_NO ");
                sql1 += string.Format("                              AND    PIECE_NO   = A.PIECE_NO ");
                sql1 += string.Format("                              AND    LINE_GP    = A.LINE_GP ");
                sql1 += string.Format("                              AND    ROUTING_CD = A.ROUTING_CD ) ");
                sql1 += string.Format("        AND   NOT EXISTS ( SELECT ROUTING_CD FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                                  WHERE  MILL_NO  = A.MILL_NO ");
                sql1 += string.Format("                                  AND    PIECE_NO = A.PIECE_NO ");
                sql1 += string.Format("                                  AND    LINE_GP  = A.LINE_GP ");
                sql1 += string.Format("                                  AND    ROUTING_CD = 'P3' ");
                sql1 += string.Format("                                  AND    REWORK_SEQ = A.REWORK_SEQ ) ");
                sql1 += string.Format("        ORDER BY 1,2,3 ");
                sql1 += string.Format(") X ");

                string[] parm = new string[6];
                parm[0] = ":P_LINE_GP|" + uC_Line_gp_s1.Line_GP;
                parm[1] = ":P_ITEM_SIZE|" + uC_Item_size_s1.ITEM_SIZE;
                parm[2] = ":P_HEAT|" + uC_HEAT_s1.HEAT;
                parm[3] = ":P_POC_NO|" + uC_POC_s1.POC;
                parm[4] = ":P_STEEL|" + uC_STEEL_s1.Steel;
                parm[5] = ":P_GOOD_NG|" + uC_OKNG1.OKNG;


                olddt = cd.FindDataTable(sql1, parm);
                moddt = olddt.Copy();
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }
        #endregion SetDataBinding 설정

        #region 이벤트 설정
        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdMain_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid editedGrd = sender as C1FlexGrid;

            int editedRow = e.Row;
            int editedCol = e.Col;

            if (editedRow <= 0 || editedGrd.GetData(editedRow, editedCol) == null || editedCol != editedGrd.Cols["CHECKER"].Index)
            {
                e.Cancel = true;
            }
        }

        private void sel_btn_Click(object sender, EventArgs e)
        {

            ck.Temptable = null;
            ck.Temptable = (DataTable)grdMain.DataSource;

            // 전달할 데이터 테이블을 생성
            foreach (DataRow dataRow in ck.Temptable.Rows)
            {
                if (dataRow["CHECKER"].ToString() == "False")
                {
                    dataRow.Delete();
                }
                else
                {
                    dataRow["CHECKER"] = false;
                }
            }

            //전달할 테이블에서 
            //parent form의 grid 의 컬럼과 ck.Temptable 의 컬럼과 일치 시켜야한다.
            ck.Temptable.Columns.Remove("CHECKER");

            ck.Temptable.AcceptChanges();

            this.Close();
        }
        #endregion 이벤트 설정
    }
}
