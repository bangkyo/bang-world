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
using System.Collections;

namespace SystemControlClassLibrary.phqms
{
    public partial class AnalDataReg : Form
    {
        #region 변수선언
        OracleTransaction transaction = null;
        DataTable olddt;
        DataTable moddt;

        clsCom ck = new clsCom();
        clsCom cS = new clsCom();
        ConnectDB cd = new ConnectDB();
        VbFunc vf = new VbFunc();
        clsStyle cs = new clsStyle();
        DateTime dateTime = new DateTime();

        private int cboLine_GP_index = 0;
        private int work_type = 0;
        private int work_team = 0;

        string biz_gp, use_yn;

        #endregion

        public AnalDataReg()
        {
            InitializeComponent();
        }

        private void AnalDataReg_Load(object sender, EventArgs e)
        {
            InitControl();

            toolTip1.AutoPopDelay = 15000;
            toolTip1.SetToolTip(txtWhere, "MV집계조건 : MV테이블의 집계조건으로 " 
                +"SELECT로 시작하면 등록된 문장 사용하고 아니면 where 조건부터 입력한다.\n"
                + "                    그리고 X로 등록하면 카운트 제외");
        }

        private void InitControl()
        {
            cs.InitCombo(cbOpBiz_gp, StringAlignment.Near);
            cs.InitCombo(cbUse_yn, StringAlignment.Near);

            SetComboEdit(cbOpBiz_gp, "OP_BIZ_GP", true, ck.Biz_gp);
            SetComboEdit(cbUse_yn, "USE_YN", false, ck.Use_yn);

            ClearForm();


        }

        private void ClearForm()
        {
            txtSpId.Text = "";
            txtSpNm.Text = "";
            txtBiTableId.Text = "";
            txtTableId.Text = "";
            txtWorkSeq.Text = "";
            txtWhere.Text = "";

            cbOpBiz_gp.SelectedIndex = 0;
            cbUse_yn.SelectedIndex = 0;

            last_exec_dt.Value = DateTime.Today;
        }

        public bool SetComboEdit(ComboBox cb, string categoryNm, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                DataTable dt = cd.Find_CD(categoryNm);


                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("선택", ""));
                }


                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public DataTable Find_CD_COLUMN(string cd_id)
        {
            OracleConnection conn = cd.OConnect();
            DataTable dt = null;
            OracleCommand ocmd = null;
            OracleDataAdapter olda = null;
            DataSet dset = new DataSet();

            string sql = string.Empty;

            try
            {
                conn.Open();
                sql = string.Format("SELECT CD_ID, CD_NM, COLUMNA, COLUMNB  FROM TB_CM_COM_CD WHERE CATEGORY = 'OP_BIZ_GP' AND CD_ID = '{0}' AND USE_YN = 'Y'  ORDER BY SORT_SEQ, CD_ID", cd_id);

                ocmd = new OracleCommand(sql, conn);
                olda = new OracleDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■sql■\n" + sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        private void cbOpBiz_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            biz_gp = string.Empty;
            string mv_biz_gp = "";
            string bi_pre = "";

            if (cbOpBiz_gp.SelectedIndex > 0)
            {
                biz_gp = cbOpBiz_gp.SelectedValue as string;
                
                txtBiTableId.Text = biz_gp + ".BI_";

                DataTable dt = Find_CD_COLUMN(biz_gp);

                foreach(DataRow row in dt.Rows)
                {
                    mv_biz_gp = row["COLUMNA"].ToString();
                    bi_pre = row["COLUMNB"].ToString();
                }

                if(mv_biz_gp != null) txtTableId.Text = mv_biz_gp + ".MV_";

            }

        }

        private void cbUse_yn_SelectedIndexChanged(object sender, EventArgs e)
        {
            use_yn = string.Empty;

            if (cbUse_yn.SelectedIndex >= 0)
            {
                use_yn = cbUse_yn.SelectedValue as string;
                //biz_ID = cd.Find_CD_ID("BIZ_GP", biz_gp);
                //MessageBox.Show(use_yn);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnReg":
                    SaveReg();
                    break;

                case "btnClose":
                    Close();
                    break;
            }
        }

        private void txtWorkSeq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txtBiTableId_TextChanged(object sender, EventArgs e)
        {
            string sp_txt = txtBiTableId.Text;
            txtSpId.Text = sp_txt.Replace(biz_gp+".BI_", "SP_");
        }

        #region 저장
        private bool SaveReg()
        {


            #region 입력 체크--------------------------------------------------------------------------------------------
            biz_gp = cbOpBiz_gp.SelectedValue.ToString();
            use_yn = cbUse_yn.SelectedValue.ToString();

            //업무구분
            if (biz_gp == "")   //|| (biz_gp == "")
            {
                MessageBox.Show("업무구분을 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbOpBiz_gp.Focus();
                return false;
            }
            
            //MV테이블ID
            if (txtTableId.Text == "")
            {
                MessageBox.Show("MV테이블ID를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTableId.Focus();
                return false;
            }

            //SP한글명
            if (txtSpNm.Text == "")
            {
                MessageBox.Show("SP한글명을 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSpNm.Focus();
                return false;
            }

            //BI테이블ID
            if (txtBiTableId.Text == "")
            {
                MessageBox.Show("BI테이블ID를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBiTableId.Focus();
                return false;
            }

            //SP처리순서
            //if (txtWorkSeq.Text == "")
            //{
            //    MessageBox.Show("DB Link ID를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtWorkSeq.Focus();
            //    return false;
            //}

            //사용여부
            if (use_yn =="")
            {
                MessageBox.Show("사용여부를 확인하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbUse_yn.Focus();
                return false;
            }
            #endregion

            string sql1 = string.Empty;
            string sql2 = string.Empty;

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

                sql2 += string.Format(@" SELECT BIZ_GP, SP_ID FROM TB_SP_TABLE 
                                         WHERE BIZ_GP = '{0}' AND SP_ID = '{1}'", biz_gp, txtSpId.Text);

                DataTable dt = cd.FindDataTable(sql2);

                if(dt.Rows.Count > 0)
                {
                    MessageBox.Show("중복된 데이터가 존재합니다.\n 저장 실패!");
                    return false;
                }

                sql1 += string.Format(@" INSERT INTO TB_SP_TABLE (
                                BIZ_GP
                              , SP_ID
                              , SP_NM
                              , USE_YN
                              , MAIN_BI_TABLE
                              , MAIN_MV_TABLE
                              , LAST_EXEC_DDTT
                              , WORK_SEQ
                              , REG_DDTT)
                          VALUES (
                                '{0}'
                              , '{1}'
                              , '{2}'
                              , '{3}'
                              , '{4}'
                              , '{5}'
                              , '{6}'
                              , '{7}'
                              , SYSDATE)", biz_gp, txtSpId.Text, txtSpNm.Text, use_yn, txtTableId.Text, txtBiTableId.Text, last_exec_dt.Text, txtWorkSeq.Text);

                int cnt = 0;
                cmd.CommandText = sql1;
                cnt = cmd.ExecuteNonQuery();
                transaction.Commit();

                if (cnt > 0)
                {
                    string message = "정상적으로 저장되었습니다.";
                    clsMsg.Log.Alarm(Alarms.Modified, Text, clsMsg.Log.__Line(), message);
                    MessageBox.Show("저장 완료.");
                    ClearForm();
                }

            }
            catch(Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                MessageBox.Show("저장에 실패하였습니다.");
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

            return true;
        }
        #endregion
    }
}
