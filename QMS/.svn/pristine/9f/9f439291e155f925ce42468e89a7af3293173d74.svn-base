using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControlClassLibrary.monitoring
{
    public partial class ReWorkReg : Form
    {

        ConnectDB cd = new ConnectDB();
        clsCom ck = new clsCom();

        string line_gp = string.Empty;
        //string gubun = string.Empty;
        string bundleNo = string.Empty;
        string zoneNo = string.Empty;
        string RegCan = string.Empty;
        string rework_RSN = string.Empty;
        /// <summary>
        /// 제작업등록 및 취소
        /// </summary>
        /// <param name="_RegCan">재작업등록: REG, 재작업취소: CAN</param>
        /// <param name="_line_gp">선택된 라인 </param>
        /// <param name="_bundleNo">선택된 번들번호</param>
        /// <param name="_zoneNo">선택된 ZONE</param>
        public ReWorkReg(string _RegCan, string _line_gp, string _bundleNo, string _zoneNo)//, string _rework_RSN)
        {
            line_gp = _line_gp;
            //gubun = _gubun;
            bundleNo = _bundleNo;
            zoneNo = _zoneNo;
            RegCan = _RegCan;
            //rework_RSN = _rework_RSN;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sel_btn_Click(object sender, EventArgs e)
        {

            string result_stat = string.Empty;
            string result_msg = string.Empty;


            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;
            string spName = "SP_GDS_REWORK_REGCAN";  // 프로시저명
            try
            {
                oConn.Open();
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                transaction = oConn.BeginTransaction();
                cmd.Transaction = transaction;

                //conn.Open();
                //cmd.Connection = conn;
                //transaction = conn.BeginTransaction();
                //cmd.Transaction = transaction;

                OracleParameter op;

                cmd.Parameters.Clear();


                op = new OracleParameter("V_PROC_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = uC_GuBun_RE1.GUBUN;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_BUNDLE_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = uC_BundleNo1.BundleNo;// vf.Format(work_ord_date, "yyyyMMdd");//  work_ord_date.ToString("yyyyMMdd");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_REWORK_RSN", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = uC_ReWork_RSN1.ReWork;// vf.Format(work_ord_date, "yyyyMMdd");//  work_ord_date.ToString("yyyyMMdd");
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_ZONE_CD", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = uC_Input_Zone1.Zone;
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

                //oConn.Open();
                //transaction = cmd.Connection.BeginTransaction();
                //cmd.Transaction = transaction;

                //cmd.ExecuteOracleScalar();
                cmd.ExecuteNonQuery();

                result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                transaction.Commit();

                if (result_stat == "ERR")
                {
                    string error_msg = "PROC_GP:" + uC_GuBun_RE1.GUBUN
                                 + " " + "BUNDLE_NO:" + uC_BundleNo1.BundleNo
                                 + " " + "REWORK_RSN:" + uC_ReWork_RSN1.ReWork
                                 + " " + "ZONE_CD:" + uC_Input_Zone1.Zone
                                 + " " + "BUNDLE_NO:" + uC_BundleNo1.BundleNo
                                 + " " + "USER_ID:" + ck.UserID
                                 + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), error_msg);
                    MessageBox.Show(result_msg);
                }
                else
                {
                    string success_msg = "PROC_GP:" + uC_GuBun_RE1.GUBUN
                                    + " " + "BUNDLE_NO:" + uC_BundleNo1.BundleNo
                                    + " " + "REWORK_RSN:" + uC_ReWork_RSN1.ReWork
                                    + " " + "ZONE_CD:" + uC_Input_Zone1.Zone
                                    + " " + "BUNDLE_NO:" + uC_BundleNo1.BundleNo
                                    + " " + "USER_ID:" + ck.UserID
                                    + " " + "Error_msg:" + result_msg;
                    clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), success_msg);
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
                MessageBox.Show(ex.Message);
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

                //this.Close();
            }

            return;
        }

        private void ReWorkReg_Load(object sender, EventArgs e)
        {
            MinimizeBox = false;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            InitControl();

            // 초기설정


        }

        private void InitControl()
        {
            uC_GuBun_RE1.GUBUN = RegCan;
            //uC_GuBun_RE1.GUBUN = gubun;
            uC_BundleNo1.BundleNo = bundleNo;
            uC_Input_Zone1.SetupCombo(line_gp);
            uC_Input_Zone1.Zone = zoneNo;
            uC_ReWork_RSN1.ReWork = rework_RSN;

            if (RegCan == "CAN")
            {
                uC_Input_Zone1.cb_Enable = false;
            }

        }
    }
}
