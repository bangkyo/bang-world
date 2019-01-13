using C1.Win.C1FlexGrid;
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
using SystemControlClassLibrary.UC;
using System.Windows.Forms;

namespace SystemControlClassLibrary.Order
{
    public partial class CrtInOrdYard2 : Form
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

        DateTime start_date;
        DateTime end_date;


        //DataTable logdt_sub;

        DataTable temp_dt;

        private string loc = "";

        private string yard = "";

        private string factory = "";

        private string loc1 = "";

        private string yard1 = "";

        //List<string> modifList;

        private UC.UC_ITEM_SIZE uC_ITEM_SIZE1;
        private UC.UC_LENGTH uC_LENGTH1;
        private UC.UC_STEEL uC_STEEL1;

        //private UC.UC_HEAT uC_HEAT1;


        //private int selectedrow = 0;

        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        //private string result_stat = "";
        //private string result_msg = "";

        private string ownerNM = "";
        private string titleNM = "";

        C1FlexGrid selectedGrd = null;

        //// search item
        //string item_size = "";
        //string steel = "";
        //string length_str = "";

        //string line_gp = "";
        //string crtin_dt_str = "";
        //string work_rank = "";
        //string wgt = "";
        


        public CrtInOrdYard2(string titleNm, string scrAuth, string factCode, string ownerNm)
        {

            

            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(temp));
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            //Icon = new Icon(Icon, Icon.Size);


            selectedGrd = grdMain;


            
        }

        private void CrtInOrdYard2_Load(object sender, EventArgs e)
        {
            InitControl();

            SetComboBox1();

            SetComboBox3();

            SetComboBox4();

            MakeInitgrdData();


            start_date = start_dt.Value = DateTime.Now;
            end_date = end_dt.Value = DateTime.Now;

            ////초기값 설정
            //item_size = uC_ITEM_SIZE1.TEXT;
            //steel = uC_STEEL1.Steel;
            //length_str = uC_LENGTH1.TEXT;

            //line_gp = uC_LINE_GP1.Line_GP;
            //crtin_dt_str = vf.Format(uC_DATE1.Date, "yyyyMMdd");
            //work_rank = uC_WORK_RANK1.TEXT;
            //wgt = uC_WGT1.TEXT;


            // 초기값 설정
            //uC_LINE_GP1.Line_GP = "#1";
            //uC_DATE1.Date = DateTime.Now.Date;
            //uC_WGT1.WGT = "0";

            btnDisplay_Click(null, null);
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

            //라인 //투입지시일자 //순위 // 중량 초기화 필요

            //uC_WGT1.WGT = "0";
            //uC_WORK_RANK1.WORK_RANK = "";
            
        }

        private void SetComboBox1()
        {
            cd.SetCombo(cbYard, "FACTORY", "", true, ck.Line_gp);
        }




        private void SetComboBox3()
        {
            cd.SetCombo(cbYard1, "LG", "", false, ck.Line_gp);
        }


        private void SetComboBox4()
        {
            cd.SetComboYard(cbLoc1, "LY", yard1, false, ck.Line_gp);
        }

        private void cbYard_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            factory = ((ComLib.DictionaryList)cbYard.SelectedItem).fnValue;
            
        }

  
        private void cbYard1_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            yard1 = ((ComLib.DictionaryList)cbYard1.SelectedItem).fnValue;
            SetComboBox4();
        }

        private void cbLoc1_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            loc1 = ((ComLib.DictionaryList)cbLoc1.SelectedItem).fnValue;
        }

        private void start_dt_ValueChanged(object sender, EventArgs e)
        {
            start_date = start_dt.Value;
        }

        private void end_dt_ValueChanged(object sender, EventArgs e)
        {
            end_date = end_dt.Value;
        }






        private void InitControl()
        {
            clsStyle.Style.InitPicture(pictureBox1);

            clsStyle.Style.InitTitle(title_lb, ownerNM, titleNM);

            clsStyle.Style.InitPanel(panel1);
            clsStyle.Style.InitPanel(panel2);

            clsStyle.Style.InitButton(btnExcel);

            clsStyle.Style.InitButton(btnDisplay);

            clsStyle.Style.InitButton(btnSave);

            clsStyle.Style.InitButton(btnClose);

            cs.InitTextBox(poctxt);

            #region 유저 컨트롤 설정

            int location_x = 0;
            int location_y = 0;

            // 
            // uC_LENGTH1
            // 
            uC_LENGTH1 = new SystemControlClassLibrary.UC.UC_LENGTH();
            uC_LENGTH1.BackColor = System.Drawing.Color.Transparent;
            //uC_LENGTH1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            uC_LENGTH1.ITEM_SIZE = "";
            uC_LENGTH1.Length = "";
            uC_LENGTH1.Location = new System.Drawing.Point(710 + location_x, 35 + location_y);
            uC_LENGTH1.Margin = new System.Windows.Forms.Padding(0);
            uC_LENGTH1.Name = "uC_LENGTH1";
            uC_LENGTH1.Size = new System.Drawing.Size(223, 50);
            uC_LENGTH1.TabIndex = 2;
            // 
            // uC_HEAT1
            // 
            //uC_HEAT1 = new SystemControlClassLibrary.UC.UC_HEAT();
            //uC_HEAT1.BackColor = System.Drawing.Color.Transparent;
            ////uC_LENGTH1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ////uC_HEAT1.ITEM_SIZE = "";
            ////uC_HEAT1.Length = "";
            //uC_HEAT1.Location = new System.Drawing.Point(850 + location_x, 6 + location_y);
            //uC_HEAT1.Margin = new System.Windows.Forms.Padding(0);
            //uC_HEAT1.Name = "uC_HEAT1";
            //uC_HEAT1.Size = new System.Drawing.Size(223, 30);
            //uC_HEAT1.TabIndex = 3;
            // 
            // uC_STEEL1
            // 
            uC_STEEL1 = new SystemControlClassLibrary.UC.UC_STEEL();
            uC_STEEL1.BackColor = System.Drawing.Color.Transparent;
            //uC_STEEL1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            uC_STEEL1.Location = new System.Drawing.Point(360 + location_x, 35 + location_y);
            uC_STEEL1.Margin = new System.Windows.Forms.Padding(4);
            uC_STEEL1.Name = "uC_STEEL1";
            uC_STEEL1.Size = new System.Drawing.Size(234, 50);
            uC_STEEL1.Steel = "";
            uC_STEEL1.Steel_NM = "";
            uC_STEEL1.TabIndex = 1;
            // 
            // uC_ITEM_SIZE1
            // 
            uC_ITEM_SIZE1 = new SystemControlClassLibrary.UC.UC_ITEM_SIZE();
            uC_ITEM_SIZE1.BackColor = System.Drawing.Color.Transparent;
            //uC_ITEM_SIZE1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            uC_ITEM_SIZE1.ITEM_SIZE = "";
            uC_ITEM_SIZE1.Location = new System.Drawing.Point(51 + location_x, 35 + location_y);
            uC_ITEM_SIZE1.Margin = new System.Windows.Forms.Padding(0);
            uC_ITEM_SIZE1.Name = "uC_ITEM_SIZE1";
            uC_ITEM_SIZE1.Size = new System.Drawing.Size(174, 50);
            uC_ITEM_SIZE1.TabIndex = 0;

           
            panel1.Controls.Add(this.uC_LENGTH1);
            //panel1.Controls.Add(this.uC_HEAT1);
            panel1.Controls.Add(this.uC_STEEL1);
            panel1.Controls.Add(this.uC_ITEM_SIZE1);

            #endregion

            InitGrd();
        }

        private void InitGrd()
        {
            InitGrd_grdMain();

            InitGrd_grdSub();
        }

        private void InitGrd_grdMain()
        {
            cs.InitGrid_search(grdMain);

            grdMain.AllowEditing = false;

            grdMain.AllowSorting = AllowSortingEnum.SingleColumn;

            grdMain.Cols["L_NO"].Width = cs.L_No_Width;
            int nwidth = (1000 - cs.L_No_Width)/5;
            grdMain.Cols["FACTORY"].Width = 0;
            grdMain.Cols["FAC_NAME"].Width = 80;
            grdMain.Cols["LOCA"].Width = 0;
            grdMain.Cols["HEAT"].Width = 80;
            grdMain.Cols["ITEM"].Width = 50;
            grdMain.Cols["LENGTH"].Width = 60;
            grdMain.Cols["BONSU"].Width = 50;
            grdMain.Cols["ITEM_SIZE"].Width = 60;
            grdMain.Cols["STEEL"].Width = 50;
            grdMain.Cols["STEEL_NAME"].Width = 120;
            grdMain.Cols["POC_NO"].Width = 100;
            grdMain.Cols["NET_WEIGHT"].Width = 100;
            grdMain.Cols["ROM_DATE_T"].Width = 130;

            //grdMain.Cols["ITEM_SIZE"].Width = cs.ITEM_SIZE_Width + 100;
            //grdMain.Cols["STEEL"].Width = 0;
            //grdMain.Cols["STEEL_NM"].Width = cs.STEEL_NM_L_Width + 100;
            //grdMain.Cols["LENGTH"].Width = cs.LENGTH_Width + 100;
            //grdMain.Cols["ORD_WGT"].Width = cs.Wgt_Width + 60;
            //grdMain.Cols["SURFACE_LEVEL_NM"].Width = cs.Surface_Level_NM_Width + 100;


            grdMain.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.
            
            grdMain.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grdMain.Cols["ITEM_SIZE"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["FACTORY"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["FAC_NAME"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["HEAT"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["ITEM"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["BONSU"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["POC_NO"].TextAlign = cs.ITEM_SIZE_TextAlign;
            grdMain.Cols["STEEL"].TextAlign = cs.STEEL_TextAlign;
            grdMain.Cols["STEEL_NAME"].TextAlign = cs.STEEL_NM_TextAlign;
            grdMain.Cols["LENGTH"].TextAlign = cs.LENGTH_TextAlign;
            grdMain.Cols["NET_WEIGHT"].TextAlign = cs.WGT_TextAlign;
            grdMain.Cols["ROM_DATE_T"].TextAlign = cs.SURFACE_LEVEL_TextAlign;
            
        }

        private void InitGrd_grdSub()
        {
            C1FlexGrid grd = grdSub as C1FlexGrid;

            cs.InitGrid_search(grd);


            //grd.AllowEditing = true;
            grd.Cols["SEL"].AllowEditing = true;

            grd.Cols["L_NO"].Width = cs.L_No_Width-10;
            grd.Cols["SEL"].Width = cs.Sel_Width;
            grd.Cols["BUNDLE_NO"].Width = cs.POC_NO_Width-10;
            grd.Cols["NET_WEIGHT"].Width = cs.Wgt_Width;

            grd.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;   // header 부분은 가운데 정렬로 한다.

            grd.Cols["L_NO"].TextAlign = cs.L_NO_TextAlign;
            grd.Cols["SEL"].TextAlign = cs.SEL_TextAlign;
            grd.Cols["BUNDLE_NO"].TextAlign = cs.POC_NO_TextAlign;
            grd.Cols["NET_WEIGHT"].TextAlign = cs.BUNDLE_QTY_TextAlign;
           
            Label lbSel = new Label();

            lbSel.BackColor = Color.Transparent;
            lbSel.Cursor = Cursors.Hand;
      
            lbSel.Click += Button_Click;

            _al.Add(new HostedControl(grdSub, lbSel, 0, grd.Cols["SEL"].Index));
        }

        bool allChecked = false;
        /// <summary>
        /// 선택 column 선택시 모두 선택, 해제 시키게 하는 함수.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, EventArgs e)
        {
            //Button bt = (Button)sender;
            //MessageBox.Show("Clicked on row: " + bt.Tag);

            if (allChecked)
            {
                for (int rowCnt = 1; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, "SEL", false);
                    
                }
                allChecked = false;
                
            }
            else
            {
                for (int rowCnt = 1; rowCnt < grdSub.Rows.Count; rowCnt++)
                {
                    grdSub.SetData(rowCnt, "SEL", true);
                    
                }
                allChecked = true;
                
            }

            // 모두 체크 혹은 체크해제 되지만 
            // 체크된 행의 지시번들이 계산되어지지않음.
            // 체크된 행의 지시번들이 계산되어지거나 해제되거나 해주어야함.
            //grdSub_CellChecked(grdSub, null);
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

            string _item_size = (string.IsNullOrEmpty(uC_ITEM_SIZE1.ITEM_SIZE)) ? "" : uC_ITEM_SIZE1.ITEM_SIZE;
            string _steel = (string.IsNullOrEmpty(uC_STEEL1.Steel)) ? "" : uC_STEEL1.Steel;
            string _length = (string.IsNullOrEmpty(uC_LENGTH1.Length)) ? "" : uC_LENGTH1.Length;


            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ROWNUM AS L_NO, FACTORY,FAC_NAME,  HEAT, ITEM, ITEM_SIZE, STEEL, ");
                sql1 += string.Format("       STEEL_NAME, LENGTH, BONSU, NET_WEIGHT, ");
                sql1 += string.Format("       ROM_DATE_T,POC_NO ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("SELECT A.FROM_FACTORY AS FACTORY, A.HEAT, A.ITEM, A.ITEM_SIZE AS SIZE_NAME, A.STEEL, ");
                sql1 += string.Format("       (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NAME, ");
                sql1 += string.Format("       (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'FACTORY' AND CD_ID = A.FROM_FACTORY) AS FAC_NAME, ");
                sql1 += string.Format("       A.LENGTH, COUNT(*) BONSU, SUM(NET_WEIGHT) NET_WEIGHT,  ");
                sql1 += string.Format("       A.ITEM_SIZE, STEEL_USE, MAX(ROM_DATE) ROM_DATE_T, ");
                sql1 += string.Format("       MAX(B.POC_NO) POC_NO ");
                //sql1 += string.Format("       FROM TB_INVENTORY_WIP@ISPDB A, TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("       FROM TB_INVENTORY_WIP A, TB_CR_ORD_BUNDLEINFO B ");
                sql1 += string.Format("       WHERE STATUS = 'WP00' ");
                sql1 += string.Format("       AND A.ITEM IN ('OB','SB','RB') ");
                sql1 += string.Format("       AND A.ROM_DATE BETWEEN :P_FR_DATE AND :P_TO_DATE ");
                sql1 += string.Format("       AND A.FROM_FACTORY LIKE '%' || :P_FACTORY || '%' ");
                sql1 += string.Format("       AND    A.HEAT      LIKE '%' || :P_HEAT || '%' ");
                sql1 += string.Format("         AND  A.ITEM_SIZE LIKE :P_ITEM_SIZE || '%' ");
                sql1 += string.Format("         AND    A.STEEL     LIKE :P_STEEL || '%' ");
                sql1 += string.Format("         AND    (A.LENGTH   = :P_LENGTH OR :P_LENGTH IS NULL ) ");
                //sql1 += string.Format("         AND A.YARD <> 'LG00' AND A.LOCATION NOT IN ('LYA00','LYZZ') ");
                if (poctxt.Text != "")
                {
                    sql1 += string.Format("          AND    B.POC_NO     LIKE '%{0}%' || '%'", poctxt.Text);    //:P_POC_NO
                }
                sql1 += string.Format("         AND A.BUNDLE_NO = B.MILL_NO(+)   ");// --종결이 아닌것.
                sql1 += string.Format("         GROUP BY A.FROM_FACTORY, A.HEAT, A.ITEM, A.ITEM_SIZE, A.STEEL, A.LENGTH, STEEL_USE ");
                sql1 += string.Format("         ORDER BY A.FROM_FACTORY, A.HEAT, A.ITEM, A.ITEM_SIZE, A.STEEL, A.LENGTH, STEEL_USE ");
                sql1 += string.Format("         ) X ");

                string[] parm = new string[7];
                parm[0] = ":P_ITEM_SIZE|" + _item_size;
                parm[1] = ":P_STEEL|" + _steel;
                parm[2] = ":P_LENGTH|" + _length;
                parm[3] = ":P_FACTORY|" + factory;
                parm[4] = ":P_HEAT|" + uC_HEAT_s1.HEAT;
                parm[5] = ":P_FR_DATE|" + vf.Format(start_date, "yyyyMMdd");
                parm[6] = ":P_TO_DATE|" + vf.Format(end_date, "yyyyMMdd");


                olddt = cd.FindDataTable(sql1, parm);

                moddt = olddt.Copy();
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdMain.SetDataBinding(moddt, null, true);

                //SelectedGrdMain(grdMain, grdMain.RowSel);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("[" + ex.ToString() + "]");
                return ;
            }
            
            return ;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 필수입력사항 체크

            bool isChecked = false;
            string show_msg = string.Empty;
            string check_NM = string.Empty;

           

            //if (string.IsNullOrEmpty(uC_WORK_RANK1.WORK_RANK))
            //{
            //    check_NM = "순위";
            //    show_msg = string.Format("{0}(을)를 입력하세요.", check_NM);
            //    MessageBox.Show(show_msg);
            //    return;
            //}

            for (int row = 1; row < grdSub.Rows.Count; row++)
            {

                if (grdSub.GetData(row, "SEL").ToString() == "False")
                {
                    continue;
                }
                isChecked = true;

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
            #endregion 필수입력사항 체크

            #region SP 구동
            string ling_gp = "";
            string Bundle_No = "";
            string yards = "";
            string locs = "";
            //string work_uom = "";
            string ord_bundle_No = "";
            string user_id = ck.UserID;
            int proc_stat = 4000;
            string result_stat = "";
            string result_msg = "";

            string strLog = string.Empty;
            var itemList = new List<DictionaryList>();
            var logList = new List<LogDataList>();


            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;
            //string spName = "SP_CR_INPUT_ORD_UPD";  // 프로시저명
            string spName = "YARD_UPDATE_FACTORY";  // 프로시저명
            try
            {
                oConn.Open();
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                transaction = oConn.BeginTransaction();
                //cmd.Transaction = transaction;

                OracleParameter op;

                for (int row = 1; row < grdSub.Rows.Count; row++)
                {

                    if (grdSub.GetData(row, "SEL").ToString() == "False")
                    {
                        continue;
                    }

                    strLog = string.Empty;
                    itemList = new List<DictionaryList>();
                    //op = null;
                    cmd.Parameters.Clear();


                    Bundle_No = grdSub.GetData(row, "BUNDLE_NO").ToString();
                    //work_rank = uC_WORK_RANK1.WORK_RANK;//grdSub.GetData(row, "WORK_RANK").ToString();

                    
                    //work_uom = " "; //grdSub.GetData(row, "WORK_UOM").ToString();

                    user_id = ck.UserID;
                    proc_stat = 4000;

                    cmd.Transaction = transaction;

                    itemList.Add(new DictionaryList("SP_NAME", spName));

                    op = new OracleParameter("P_BUNDLE_NO", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = Bundle_No;
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_BUNDLE_NO", Bundle_No));

                    op = new OracleParameter("P_YARD", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = yard1;
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_YARD", yard1));


                    op = new OracleParameter("P_LOC", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = loc1;
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_LOC", loc1));

   
                    op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                    op.Direction = ParameterDirection.Input;
                    op.Value = ck.UserID;    // 사용자 id 가져와서 입력
                    cmd.Parameters.Add(op);
                    itemList.Add(new DictionaryList("P_USER_ID", ck.UserID));

                    op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = proc_stat;
                    cmd.Parameters.Add(op);

                    op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                    op.Direction = ParameterDirection.Output;
                    op.Size = 4000;
                    cmd.Parameters.Add(op);

                    cmd.ExecuteNonQuery();

                    result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                    result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                    LogStrCreate(itemList, ref strLog);

                    if (result_stat == "ERR")
                    {
                        MessageBox.Show(result_msg);
                        logList.Add(new LogDataList(Alarms.Error, Text, strLog));
                        break;
                    }
                    else if (result_stat == "OK")
                    {
                        logList.Add(new LogDataList(Alarms.InSerted, Text, strLog));
                        
                    }

                }

                transaction.Commit();


                if (result_stat == "ERR")
                {
                    foreach (var item in logList)
                    {
                        if (item.Action == Alarms.Error)
                        {
                            clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), item.Contents);
                        }
                    }
                }
                else if (result_stat == "OK")
                {
                    //log make
                    btnDisplay_Click(null, null);

                    clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);

                    foreach (var log in logList)
                    {
                        clsMsg.Log.Alarm(log.Action, log.PageName, clsMsg.Log.__Line(), log.Contents);
                    }
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
                //return;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cmd.Connection != null)
                    cmd.Connection.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();

            }
            btnDisplay_Click(null, null);

            return;
            #endregion
        }

        public static void LogStrCreate(List<DictionaryList> _itemList, ref string _strLog)
        {

            foreach (var item in _itemList)
            {
                _strLog += string.Format(" {0}: {1},",item.fnText, item.fnValue);
            }
        }

        
        private void grdSub_Click(object sender, EventArgs e)
        {
            
        }

        private void grdMain_Click(object sender, EventArgs e)
        {
            C1FlexGrid _selectedGrd = sender as C1FlexGrid;

            selectedGrd = _selectedGrd;

            SelectedGrdMain(selectedGrd, selectedGrd.RowSel);

            //if (grdMain.RowSel == 0)
            //{

            //}
        }

        private void SelectedGrdMain(C1FlexGrid grd, int selected_Row)
        {
            if (grd.RowSel <= 0 || grd.Rows.Count <= 1)
            {
                return;
            }


            // subgrd data search
            string _FACTORY = grd.GetData(selected_Row, "FACTORY").ToString();
            //string _HEAT = vf.Format(grd.GetData(selected_Row, "WORK_ORD_DATE"), "yyyyMMdd");
            string _HEAT = grd.GetData(selected_Row, "HEAT").ToString();
            string _item_size = grd.GetData(selected_Row, "ITEM_SIZE").ToString();
            //string _steel = grd.GetData(selected_Row, "STEEL").ToString();
            string _length = grd.GetData(selected_Row, "LENGTH").ToString();
            //string 

            InitGridData(false);

            SetDataBindingGrdSub( _item_size, _FACTORY, _HEAT, _length);
        }

        private void SetDataBindingGrdSub(string _item_size, string _FACTORY, string _HEAT, string _length)
        {
            try
            {
                string sql1 = string.Empty;
                sql1 += string.Format("SELECT ROWNUM AS L_NO ");
                sql1 += string.Format("       ,'False' AS SEL ");
                sql1 += string.Format("       ,X.* ");
                sql1 += string.Format("FROM   ( ");
                sql1 += string.Format("SELECT  BUNDLE_NO, NET_WEIGHT ");
                sql1 += string.Format("FROM   TB_INVENTORY_WIP ");
                sql1 += string.Format("WHERE  ITEM_SIZE = :P_ITEM_SIZE ");
                sql1 += string.Format("AND    STATUS = 'WP00' ");
                //sql1 += string.Format("AND    YARD LIKE '%' || :P_YARD || '%' ");
                sql1 += string.Format("AND    FROM_FACTORY LIKE '%' || :P_FACTORY || '%' ");
                sql1 += string.Format("AND    HEAT    = :P_HEAT ");
                sql1 += string.Format("AND    LENGTH = :P_LENGTH ");
                //sql1 += string.Format("AND    YARD <> 'LG00' AND LOCATION NOT IN ('LYA00','LYZZ')   ");// --종결이 아닌것.
                sql1 += string.Format("AND    STATUS <> 'WP90'   ");// --종결이 아닌것.
                sql1 += string.Format("ORDER BY 1,2");
                sql1 += string.Format(") X ");

                string[] parm = new string[4];
                parm[0] = ":P_ITEM_SIZE|" + _item_size;
                //parm[1] = ":P_YARD|" + yard;
                parm[1] = ":P_HEAT|" + _HEAT;
                parm[2] = ":P_FACTORY|" + _FACTORY;
                parm[3] = ":P_LENGTH|" + _length;

                olddt_sub = cd.FindDataTable(sql1, parm);

                moddt_sub = olddt_sub.Copy();
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;
                grdSub.SetDataBinding(moddt_sub, null, true);
                this.Cursor = System.Windows.Forms.Cursors.Default;

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), "  " + moddt_sub.Rows.Count.ToString() + " 건 조회 되었습니다.");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("[" + ex.ToString() + "]");
                return;
            }

            return;
        }

        private void grdSub_BeforeEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (e.Col != grd.Cols["SEL"].Index)  //특정 Row 와 Cell 지정하여 사용하세요
            {
                e.Cancel = true;
            }


          



        }

       ArrayList _al = new ArrayList();

        /// <summary>
        /// HostedControl
        /// helper class that contains a control hosted within a C1FlexGrid
        /// </summary>
        internal class HostedControl
        {
            internal C1FlexGrid _flex;
            internal Control _ctl;
            internal Row _row;
            internal Column _col;

            internal HostedControl(C1FlexGrid flex, Control hosted, int row, int col)
            {
                // save info
                _flex = flex;
                _ctl = hosted;
                _row = flex.Rows[row];
                _col = flex.Cols[col];


                // insert hosted control into grid
                _flex.Controls.Add(_ctl);
            }

            internal bool UpdatePosition()
            {
                // get row/col indices
                int r = _row.Index;
                int c = _col.Index;
                if (r < 0 || c < 0) return false;

                // get cell rect
                Rectangle rc = _flex.GetCellRect(r, c, false);

                // hide control if out of range
                if (rc.Width <= 0 || rc.Height <= 0 || !rc.IntersectsWith(_flex.ClientRectangle))
                {
                    _ctl.Visible = false;
                    return true;
                }

                // move the control and show it
                _ctl.Bounds = rc;
                _ctl.Visible = true;

                // done
                return true;
            }
        }

        private void grdSub_Paint(object sender, PaintEventArgs e)
        {
            foreach (HostedControl hosted in _al)
                hosted.UpdatePosition();
        }



        private void grdSub_KeyPressEdit(object sender, KeyPressEditEventArgs e)
        {
            if (e.Col == grdSub.Cols["INPUT_QTY"].Index)
            {
                vf.KeyPressEvent(sender, e);
            }
        }

        private void grdSub_RowColChange(object sender, EventArgs e)
        {
            
        }


    }
}
