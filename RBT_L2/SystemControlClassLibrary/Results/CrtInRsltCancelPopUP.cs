﻿using C1.Win.C1FlexGrid;
using ComLib;
//using DataControlClassLibrary;
using ComLib.clsMgr;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SystemControlClassLibrary
{
    public partial class CrtInRsltCancelPopUP : Form
    {
        #region 변수 설정
        private clsCom ck = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();

        DataTable olddt;
        DataTable moddt;
        DataTable logdt;

        private string millNo = "";
        private bool isChange = false;

        private string line_gp = "";


        OracleTransaction transaction = null;
        #endregion 변수 설정

        #region 로딩, 생성자 설정
        public CrtInRsltCancelPopUP(string _line_gp, string _mill_no) // 파라미터 한개 더 추가
        {
            line_gp = _line_gp;
            millNo = _mill_no;

            InitializeComponent();

            Load += CrtInRsltInqPopUP_Load;

            btnCancel.Click += Button_Click;
            btnClose.Click += Button_Click;

            cbo_outputLine.SelectedIndexChanged += cbo_outputLine_SelectedIndexChanged;
        }

        ~CrtInRsltCancelPopUP()
        {

        }

        private void CrtInRsltInqPopUP_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimizeBox = false;
            MaximizeBox = false;

            //StartPosition = FormStartPosition.CenterParent;

            InitControl();
            SetComboBox1();
        }
        #endregion 로딩, 생성자 설정

        #region Init Control 설정
        private void InitControl()
        {
            txtMillNo.MaxLength = 10;
            txtMillNo.Text = millNo;

            cs.InitCombo(cbo_outputLine, StringAlignment.Center);

            SetDataBinding();
        }
        #endregion Init Control 설정

        #region  ComboBox 설정
        private void SetComboBox1()
        {
            cd.SetCombo(cbo_outputLine, "LINE_GP", "", false, line_gp);

            //cbo_outputLine.Enabled = false;
        }
        #endregion  ComboBox 설정

        #region SetDataBinding 설정
        private bool SetDataBinding()
        {
            try
            {
                string sql1 = string.Format("SELECT A.MILL_NO    AS MILL_NO   ");
                sql1 += string.Format("      ,A.POC_NO     AS POC_NO  ");
                sql1 += string.Format("      ,A.HEAT       AS HEAT ");
                sql1 += string.Format("      ,(SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL) AS STEEL_NM ");
                sql1 += string.Format("      ,A.ITEM_SIZE  AS ITEM_SIZE ");
                sql1 += string.Format("      ,A.LENGTH     AS ITEM_LENGTH    ");
                sql1 += string.Format("      ,A.PCS        AS MILL_PCS  ");
                sql1 += string.Format("      ,A.NET_WGT    AS MILL_WGT ");
                sql1 += string.Format("      ,B.INPUT_DDTT AS INPUT_DDTT ");
                sql1 += string.Format("      ,B.LINE_GP    AS LINE_GP ");
                sql1 += string.Format("FROM   TB_CR_ORD_BUNDLEINFO A ");
                sql1 += string.Format("      ,TB_CR_INPUT_WR       B ");
                sql1 += string.Format("WHERE  A.MILL_NO  LIKE '{0}' || '%' ", txtMillNo.Text);    //:P_MILL_NO
                sql1 += string.Format("AND    B.LINE_GP = '{0}' ", line_gp); //:P_LINE_GP
                sql1 += string.Format("AND    A.MILL_NO  = B.MILL_NO(+) ");

                olddt = cd.FindDataTable(sql1);

                logdt = olddt.Copy();

                moddt = olddt.Copy();

                txtheat.Text = moddt.Rows[0]["HEAT"].ToString();
                txtPoc.Text = moddt.Rows[0]["POC_NO"].ToString();
                txtSteel.Text = moddt.Rows[0]["STEEL_NM"].ToString();
                txtSize.Text = moddt.Rows[0]["ITEM_SIZE"].ToString();
                txtLength.Text = vf.Format(moddt.Rows[0]["ITEM_LENGTH"].ToString(), "0.00");
                txtMillPcs.Text = vf.Format(moddt.Rows[0]["MILL_PCS"].ToString(), "0,0");
                txtMillWgt.Text = vf.Format(moddt.Rows[0]["MILL_WGT"].ToString(), "0,000");
                tbCrtInDate.Text = vf.Format(moddt.Rows[0]["INPUT_DDTT"], "yyyy-MM-dd");
                tbCrtInTime.Text = vf.Format(moddt.Rows[0]["INPUT_DDTT"], "HH:mm:ss");

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion SetDataBinding 설정

        #region 이벤트 설정
        private void cbo_outputLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string cd_Nm = string.Empty;
            //if (cbo_outputLine.SelectedIndex >= 0)
            //{
            //    cd_Nm = cbo_outputLine.SelectedItem as string;
            //    line_gp = cd.Find_CD_ID("LINE_GP", cd_Nm);
            //}

            line_gp = ((DictionaryList)cbo_outputLine.SelectedItem).fnValue;
        }

        private void txtMillNo_LostFocus(object sender, EventArgs e)
        {
            SetDataBinding();
        }

        private void txtMillNo_TextChanged(object sender, EventArgs e)
        {
            ClearContentsWhenMillNoBlank();
        }

        private void ClearContentsWhenMillNoBlank()
        {
            if (txtMillNo.Text == "")
            {
                txtheat.Text = "";
                txtPoc.Text = "";
                txtSteel.Text = "";
                txtSize.Text = "";
                txtLength.Text = "";
                txtMillPcs.Text = "";
                txtMillWgt.Text = "";
                tbCrtInDate.Text = "";
                tbCrtInTime.Text = "";
            }
        }

        private void txtMill_No_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtMillNo.Text != "")
            {
                SetDataBinding();
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnCancel":
                    SaveCancel();
                    break;

                case "btnClose":
                    Close();
                    break;
            }
            return;
        }


        private void SaveCancel()
        {
            #region 항목체크
            if (isChange)
            {
                if (MessageBox.Show("저장하시겠습니까?", Text, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            #endregion 항목체크     

            string spName = "SP_CR_IN_REGCAN"; // 조회용 저장프로시저명

            OracleConnection oConn = cd.OConnect();
            OracleCommand cmd = new OracleCommand();

            try
            {
                cmd.Connection = oConn;
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter op;

                op = new OracleParameter("P_PROC_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = "CAN";
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_USER", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = ck.UserID;    // 사용자 id 가져와서 입력
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_LINE_GP", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = line_gp;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_MILL_NO", OracleType.VarChar);
                op.Direction = ParameterDirection.Input;
                op.Value = txtMillNo.Text;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_STAT", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                op = new OracleParameter("P_PROC_MSG", OracleType.VarChar);
                op.Direction = ParameterDirection.Output;
                op.Size = 4000;
                cmd.Parameters.Add(op);

                oConn.Open();
                transaction = cmd.Connection.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.ExecuteOracleScalar();

                string result_stat = Convert.ToString(cmd.Parameters["P_PROC_STAT"].Value);
                string result_msg = Convert.ToString(cmd.Parameters["P_PROC_MSG"].Value);

                transaction.Commit();

                if (result_stat == "ERR")
                {
                    //string error_msg = "MILL_NO:" + txtMillNo.Text
                    //             + " " + "User_id:" + ck.UserID
                    //             + " " + "Error_msg:" + result_msg;
                    //clsMsg.Log.Alarm(Alarms.Error, Text, clsMsg.Log.__Line(), error_msg);
                    MessageBox.Show(result_msg);
                }
                else
                {
                    //string success_msg = "POC_NO:" + txtMillNo.Text
                    //           + " " + "User_id:" + ck.UserID
                    //           + " " + "Result_msg:" + result_msg;
                    //clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), success_msg);
                    //InitContents();
                    //clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), result_msg);
                    Close();
                }
            }
            catch (Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }
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
        }

        private void InitContents()
        {
            txtMillNo.Text = "";
            txtheat.Text = "";
            txtPoc.Text = "";
            txtSteel.Text = "";
            txtSize.Text = "";
            txtLength.Text = "";
            txtMillPcs.Text = "";
            txtMillWgt.Text = "";
            tbCrtInDate.Text = "";
            tbCrtInTime.Text = "";

            txtMillNo.Focus();
        }
        #endregion 이벤트 설정

        private void txtMillNo_Leave(object sender, EventArgs e)
        {
            if (txtMillNo.Text == "")
            {
                return;
            }
            SetDataBinding();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
