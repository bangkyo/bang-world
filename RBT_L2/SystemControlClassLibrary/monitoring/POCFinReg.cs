using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SystemControlClassLibrary.monitoring
{
    public partial class POCFinReg : Form
    {

        clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();


        DataTable olddt;
        DataTable moddt;
        DataTable logdt;


        private string cd_id = "";
        private string cd_nm = "";


        public static string __File()
        {
            var st = new StackTrace(new StackFrame(true));
            var sf = st.GetFrame(0);
            return sf.GetFileName();
        }

        private static int cboLine_GP_index = 0;

        //private MyInterface Myface = null;
        private static int Data = 0;

        private static string p_proc_gp = "";


        private string line_gp = "";
        private string poc_NM = "";

        public POCFinReg(string _line_gp, string _poc_NM)
        {
            line_gp = _line_gp;
            poc_NM = _poc_NM;
            InitializeComponent();
        }


        private void POCFinReg_Load(object sender, EventArgs e)
        {
            MinimizeBox = false;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            InitControl();

            SetDataBinding();
            
        }

        private void InitControl()
        {
            cs.InitCombo(cbOutLine, StringAlignment.Center);
            cs.InitCombo(cbFinSelect, StringAlignment.Center);
            cs.InitCombo(cbPOC, StringAlignment.Center);

            cd.SetComboPOC(cbPOC, line_gp);
            cd.SetCombo(cbOutLine, "LINE_GP", false, line_gp);
            SetCombo_YN(cbFinSelect);
        }

        private void TbPOC_LostFocus(object sender, EventArgs e)
        {
            SetDataBinding();
        }

        private void SetCombo_YN(ComboBox cb)
        {
            var arrType1 = new ArrayList();

            arrType1.Add("Y");
            arrType1.Add("N");

            cb.DataSource = arrType1;

        }


        private bool SetDataBinding()
        {
            try
            {

                string sql1 = string.Empty;
                sql1 += string.Format("SELECT B.LINE_GP ");
                sql1 += string.Format("      ,A.POC_NO ");
                sql1 += string.Format("      ,A.HEAT ");
                sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("      ,A.ITEM_SIZE ");
                sql1 += string.Format("      ,A.LENGTH ");
                sql1 += string.Format("      ,(SELECT SUM(PCS) FROM TB_CR_ORD_BUNDLEINFO WHERE POC_NO = A.POC_NO) AS MILL_PCS   ");
                sql1 += string.Format("      ,STR_PCS ");
                sql1 += string.Format("      ,BND_PCS ");
                sql1 += string.Format("      ,'N' as FINISH_YN ");
                sql1 += string.Format("FROM   TB_CR_ORD  A ");
                sql1 += string.Format("      ,(SELECT  LINE_GP ");
                sql1 += string.Format("               ,POC_NO ");
                sql1 += string.Format("               ,COUNT(*) AS STR_PCS ");
                sql1 += string.Format("        FROM   TB_CR_PIECE_WR A ");
                sql1 += string.Format("        WHERE  LINE_GP  = :P_LINE_GP ");
                sql1 += string.Format("        AND    POC_NO   = :P_POC_NO ");
                sql1 += string.Format("        AND    ROUTING_CD = 'A1' ");
                sql1 += string.Format("        AND    REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
                sql1 += string.Format("                             WHERE  MILL_NO    = A.MILL_NO ");
                sql1 += string.Format("                             AND    PIECE_NO   = A.PIECE_NO ");
                sql1 += string.Format("                             AND    LINE_GP    = A.LINE_GP ");
                sql1 += string.Format("                             AND    ROUTING_CD = A.ROUTING_CD) ");
                sql1 += string.Format("        GROUP BY LINE_GP, POC_NO) B ");
                sql1 += string.Format("      ,(SELECT  LINE_GP ");
                sql1 += string.Format("               ,POC_NO ");
                sql1 += string.Format("               ,SUM(PCS)  AS BND_PCS ");
                sql1 += string.Format("        FROM    TB_BND_WR ");
                sql1 += string.Format("        WHERE  LINE_GP  = :P_LINE_GP ");
                sql1 += string.Format("        AND    POC_NO   = :P_POC_NO ");
                sql1 += string.Format("        AND    NVL(DEL_YN,'N') <> 'Y' ");
                sql1 += string.Format("        GROUP BY LINE_GP, POC_NO) C ");
                sql1 += string.Format("WHERE  A.POC_NO  = B.POC_NO(+) ");
                sql1 += string.Format("AND    A.POC_NO  = C.POC_NO(+) ");
                sql1 += string.Format("AND    A.POC_NO  = :P_POC_NO ");
                //sql1 += string.Format("AND    A.LINE_GP = :P_LINE_GP ");

                //moddt = new DataTable();
                string[] parm = new string[2];
                parm[0] = ":P_LINE_GP|" + line_gp;
                parm[1] = ":P_POC_NO|" + cbPOC.Text;


                olddt = cd.FindDataTable(sql1, parm);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                if (moddt.Rows.Count > 0)
                {
                    tbHEAT.Text = moddt.Rows[0]["HEAT"].ToString();
                    tbSteel.Text = moddt.Rows[0]["STEEL_NM"].ToString();
                    tbSize.Text = moddt.Rows[0]["ITEM_SIZE"].ToString();
                    tbLength.Text = vf.Format(moddt.Rows[0]["LENGTH"], "#.00");
                    tbMillPcs.Text = vf.Format(moddt.Rows[0]["MILL_PCS"], "#,###");
                    tbSTRPcs.Text = vf.Format(moddt.Rows[0]["STR_PCS"], "#,###");
                    tbBNDPcs.Text = vf.Format(moddt.Rows[0]["BND_PCS"], "#,###");
                    cbFinSelect.Text = moddt.Rows[0]["FINISH_YN"].ToString(); 
                }
                else
                {
                    tbHEAT.Text = "";
                    tbSteel.Text = "";
                    tbSize.Text = "";
                    tbLength.Text = "";
                    tbMillPcs.Text = "";
                    tbSTRPcs.Text = "";
                    tbBNDPcs.Text = "";
                    cbFinSelect.Text = "";
                }
   

            }
            catch (Exception ex)
            {
                //MessageBox.Show("[" + ex.ToString() + "]");
                return false;
            }

            return true;
        }



        private void cbFinSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbFinSelect.Equals(null))
            {
                if (cbFinSelect.Text == "Y")
                {
                    btnCancel.Enabled = true;
                }
                else
                {
                    btnCancel.Enabled = false;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ProcessPocFin("REG");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ProcessPocFin("CAN");
        }

        /// <summary>
        /// poc 종료 등록 및 취소
        /// </summary>
        /// <param name="RegOrCancel">" REG: 등록, CAN: 취소"</param>
        private void ProcessPocFin(string RegOrCancel)
        {

            string _poc_No = cbPOC.Text;
            string _MINUSS = "";

            string sql1 = string.Empty;
            sql1 += string.Format(" SELECT POC, QTY, COUNT(*) CO, QTY - COUNT(*) AS MINUSS ");
            sql1 += string.Format(" FROM( ");
            sql1 += string.Format("         select A.POC_NO POC, F.BUNDLE_QTY QTY, A.MILL_NO, COUNT(*) STR_BON, C.ORD_PCS, C.ORD_PCS - (COUNT(*)) SETT ");
            sql1 += string.Format("               FROM TB_CR_PIECE_WR A, TB_CR_INPUT_WR B, TB_CR_ORD_BUNDLEINFO C, TB_CR_ORD F ");
            sql1 += string.Format("               WHERE A.POC_NO = B.POC_NO ");
            sql1 += string.Format("               AND B.POC_NO = C.POC_NO ");
            sql1 += string.Format("               AND A.POC_NO = F.POC_NO ");
            sql1 += string.Format("               AND A.MILL_NO = B.MILL_NO ");
            sql1 += string.Format("               AND B.MILL_NO = C.MILL_NO ");
            sql1 += string.Format("               AND A.POC_NO = :P_POC_NO ");
            sql1 += string.Format("               AND A.ROUTING_CD = 'A1' ");
            sql1 += string.Format("               GROUP BY A.POC_NO, A.MILL_NO, C.ORD_PCS, F.BUNDLE_QTY ");
            sql1 += string.Format("  ) D ");
            sql1 += string.Format(" WHERE SETT <= 0 ");
            sql1 += string.Format(" GROUP BY POC, QTY ");

            string[] parm = new string[1];
            parm[0] = ":P_POC_NO|" + _poc_No;
            moddt = cd.FindDataTable(sql1, parm);

            if (moddt != null)
            {
                try
                {
                    _MINUSS = moddt.Rows[0]["MINUSS"].ToString();
                    string msg = string.Format(" 교정미투입번들 {0} 번들 \n POC : {1} \n POC 종료 하시겠습니까?", _MINUSS, _poc_No);

                    if (MessageBox.Show(msg, Text, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                catch (Exception)
                {

                    //return;
                }
                
            }







            OracleConnection conn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string spName = "SP_CR_POC_END_REGCAN";
            try
            {

                cmd.Connection = conn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;

                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = line_gp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_POC_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = cbPOC.Text;    // poc
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = RegOrCancel;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = ck.UserID;    // 사용자 id 가져와서 입력
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);



                conn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.ExecuteOracleScalar();

                string result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                string result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                transaction.Commit();

                if (result_stat == "ERR")
                {
                    string error_msg = "POC_NO:" + _poc_No
                                 + " " + "User_id:" + ck.UserID
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), error_msg);
                    MessageBox.Show(result_msg);
                    //return false;
                }
                else
                {
                    string success_msg = "POC_NO:" + _poc_No
                                + " " + "User_id:" + ck.UserID
                                + " " + "Result_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Deleted, Text, clsMsg.Log.__Line(), success_msg);
                    //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
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

                return;
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
            return;
        }


        private void cbOutLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            line_gp =  ((ComLib.DictionaryList)cbOutLine.SelectedItem).fnValue;

            //라인변경되었을경우 POC COMBOBOX 변경
            cd.SetComboPOC(cbPOC, line_gp);
        }

        private void cbPOC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((DictionaryList)cbPOC.SelectedItem == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(((DictionaryList)cbPOC.SelectedItem).fnValue))
            {
                SetDataBinding();
            }
            
        }
    }
}
