using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComLib;
using ComLib.clsMgr;
using System.Data.OracleClient;

namespace SystemControlClassLibrary.phqms
{
    public partial class ColDataRefresh : Form
    {
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();

        string biz_gp, mv_table_id;
        string ref_mode;
        public ColDataRefresh()
        {
            InitializeComponent();
        }

        public ColDataRefresh(string _biz_gp, string _mv_table_id)
        {
            InitializeComponent();

            biz_gp = _biz_gp;
            mv_table_id = _mv_table_id;

        }

        private void ColDataRefresh_Load(object sender, EventArgs e)
        {
            cs.InitCombo(cbMode, StringAlignment.Near);
            cd.SetCombo(cbMode, "REF_MODE", false, ck.Ref_mode);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnReg":
                    Exec();
                    break;

                case "btnClose":
                    Close();
                    break;
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ref_mode = cbMode.SelectedValue.ToString();
        }

        private bool Exec()
        {
            OracleConnection conn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;

            string spName = "SP_MV_REFRESH_MAN";
            OracleParameter op;

            ref_mode = cbMode.SelectedValue.ToString();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                op = new OracleParameter("P_BIZ_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = biz_gp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_TABLE_ID", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = mv_table_id;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_REFRESH_MODE", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = biz_gp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("W_STAT", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                cmd.ExecuteNonQuery();

                transaction.Commit();

                string w_stat = cmd.Parameters["W_STAT"].Value.ToString();
                //MessageBox.Show("w_stat : "+ w_stat);

                if (w_stat == "OK")
                {
                    MessageBox.Show(spName + " : " + biz_gp + " : " + mv_table_id + " Refresh 완료 ");
                }
                string result_msg = spName + ":" +biz_gp+":"+mv_table_id+ " Refresh 실행!";
                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), result_msg);
            }
            catch (Exception ex)
            {
                if(transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
                if (transaction != null)
                    transaction.Dispose();
            }


            return true;
        }
    }
}
