using BGsystemLibrary.MatMgmt;
using ComLib;
using ComLib.clsMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGsystemLibrary.Common
{
    public partial class EqmCheckResultReg : Form
    {
        //공통
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();

        DataTable olddt;
        string txtdeptNm = "";

        public ScheduleMgmt frm;

        public delegate void FormSendDataHandler(object obj);
        public event FormSendDataHandler FormSendEvent;



        private string titleNM = "";
        private string menuNM = "";
        private string categoryNM = "";

        private string itemSizeHead = "";
        private string itemSizeIDTail = "";

        private string newSeq = "";
        private string selected_EQP_CD = "";
        private string selected_ITEM_CD = "";


        public EqmCheckResultReg(string _titleNM, string _menuNM, string _categoryNM)
        {
            titleNM = _titleNM;
            menuNM = _menuNM;
            categoryNM = _categoryNM;



            InitializeComponent();

            Text = titleNM;
            categoryNM = _categoryNM;
        }
        clsParameterMember param = new clsParameterMember();
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (olddt.Rows.Count <= 0)
            {
                MessageBox.Show("해당정보가 없습니다.");
                return;
            }

            string date = vf.Format(start_dt.Value, "yyyyMMdd");

            string sql = "";


            SqlConnection conn = cd.OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                this.Cursor = Cursors.AppStarting;

                sql = string.Format(" INSERT INTO TB_EQP_CHECK_WR ");
                sql += string.Format("             (                              ");
                sql += string.Format("                EQP_CD                      ");
                sql += string.Format("               ,ITEM_CD                     ");
                sql += string.Format("               ,CHECK_SEQ                   ");
                sql += string.Format("               ,EQP_NM                      ");
                sql += string.Format("               ,ROUTING_NM                  ");
                sql += string.Format("               ,CHECK_ITEM                  ");
                sql += string.Format("               ,CHECK_PLN_DATE              ");
                sql += string.Format("               ,CHECK_WR_DATE               ");
                sql += string.Format("               ,CHECK_GAP                   ");
                sql += string.Format("               ,CHECK_CYCLE                 ");
                sql += string.Format("               ,CHECK_YN                    ");
                sql += string.Format("               ,CHECK_CNTS                  ");
                sql += string.Format("               ,USE_CNT                     ");
                sql += string.Format("               ,PROG_STAT                   ");
                sql += string.Format("               ,REGISTER                    ");
                sql += string.Format("               ,REG_DDTT                    ");
                sql += string.Format("             )                              ");
                sql += string.Format(" VALUES      (                              ");
                sql += string.Format("               '{0}' ", olddt.Rows[0]["EQP_CD"].ToString());
                sql += string.Format("              ,'{0}' ", olddt.Rows[0]["ITEM_CD"].ToString());
                sql += string.Format("              ,'{0}' ", olddt.Rows[0]["NEW_CHECK_SEQ"].ToString());
                sql += string.Format("              ,'{0}' ", olddt.Rows[0]["EQP_NM"].ToString());
                sql += string.Format("              ,'{0}' ", olddt.Rows[0]["ROUTING_NM"].ToString());
                sql += string.Format("              ,'{0}' ", olddt.Rows[0]["CHECK_ITEM"].ToString());
                sql += string.Format("              ,'{0}' ", vf.Format(start_dt.Value, "yyyyMMdd"));
                sql += string.Format("              ,'{0}' ", vf.Format(start_dt.Value, "yyyyMMdd"));
                sql += string.Format("              ,'{0}' ", olddt.Rows[0]["CHECK_GAP"].ToString());
                sql += string.Format("              ,'{0}' ", olddt.Rows[0]["CHECK_CYCLE"].ToString());
                sql += string.Format("              ,'{0}' ", "N");
                sql += string.Format("              ,'{0}' ", tb_checkCnts.Text);
                sql += string.Format("              ,'{0}' ", tb_UseCnt.Text);
                sql += string.Format("              ,'{0}' ", "END");
                sql += string.Format("              ,'{0}' ", ck.UserID);
                sql += string.Format("              ,(select convert(varchar, getdate(), 120)) ");
                sql += string.Format("             ) ");


                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                //InsCnt++;

                this.Cursor = Cursors.Default;

                //lastSearchHeatNo = _heat_no;

                //실행후 성공
                transaction.Commit();

                //SetDataBinding();

                // 성공시에 추가 수정 삭제 상황을 초기화시킴

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);

                Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {

            }

        }


        private void EqmCheckResultReg_Load(object sender, EventArgs e)
        {
            InitControl();
        }

        private void InitControl()
        {
            //SetCombo(cbx_Heat_No, false);
            cd.SetCombo(cbx_Eq_Gp, GetEqData(), "", false, 0, 1);

            SetCheckItemCB();

            SetData();
        }

        private void SetCheckItemCB()
        {
            cd.SetCombo(cbx_CheckItem_Gp, GetEqItemData(), "", false, 0, 1);
        }

        private string GetEqItemData()
        {
            string sql = string.Empty;

            sql = string.Format(@"SELECT DISTINCT ITEM_CD
                                       , CHECK_ITEM
                                    FROM TB_EQP_CHECK_ITEM A
                                   WHERE EQP_CD LIKE '{0}'
                                     AND USE_YN = 'Y'", ((ComLib.DictionaryList)this.cbx_Eq_Gp.SelectedItem).fnValue);

            return sql;
        }

        private string GetEqData()
        {
            string sql = string.Empty;

            sql = string.Format(@"SELECT A.EQP_CD
                                       , (SELECT EQP_NM FROM TB_EQP_INFO WHERE EQP_CD = A.EQP_CD) AS EQP_NM
                                    FROM (
                                    		SELECT DISTINCT EQP_CD 
                                    		  FROM TB_EQP_CHECK_ITEM
                                    		 WHERE USE_YN = 'Y'
                                         ) A ");

            return sql;
        }

        private void SetData()
        {
            string heatNo = cbx_Eq_Gp.Text;

            try
            {
                string sql = "";
                sql += string.Format(@"  SELECT TOP 1 
                                               EQP_CD
                                        	  ,ITEM_CD
                                        	  ,EQP_NM
                                        	  ,ROUTING_NM
                                        	  ,CHECK_ITEM
                                        	  ,CHECK_SEQ +1 AS NEW_CHECK_SEQ
                                              ,CHECK_PLN_DATE
                                              ,CONVERT(CHAR(8), GETDATE(), 112) AS TODAY 
                                              ,CONVERT(CHAR(19), GETDATE(), 120) AS DATE_CHECK_WR_DATE
                                              ,CHECK_GAP
                                              ,CHECK_CYCLE
                                              ,PROG_STAT
                                              ,CONVERT(varchar(10), CONVERT(date, CONVERT(varchar(8), CHECK_WR_DATE))) AS CHECK_WR_DATE
                                              ,CONVERT(VARCHAR,CHECK_GAP) +' '+ (SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'CHECK_CYCLE' AND USE_YN = 'Y' AND CD_ID = CHECK_CYCLE) AS CHECK_CYCLE_NM
                                              ,CHECK_CNTS
                                              ,USE_CNT
                                          FROM TB_EQP_CHECK_WR
                                         WHERE EQP_CD = '{0}'
                                           AND ITEM_CD = '{1}'
                                      ORDER BY  CHECK_SEQ  DESC "
                                           , ((ComLib.DictionaryList)this.cbx_Eq_Gp.SelectedItem).fnValue
                                           , ((ComLib.DictionaryList)this.cbx_CheckItem_Gp.SelectedItem).fnValue
                                           );


                olddt = cd.FindDataTable(sql);

                if (olddt.Rows.Count > 0)
                {
                    tb_checkCycle.ReadOnly = false;

                    tb_checkCycle.Text = olddt.Rows[0]["CHECK_CYCLE_NM"].ToString();
                    tb_checkCnts.Text = olddt.Rows[0]["CHECK_CNTS"].ToString();
                    tb_UseCnt.Text = olddt.Rows[0]["USE_CNT"].ToString();

                    tb_checkCycle.ReadOnly = true;
                }

                //this.Cursor = Cursors.AppStarting;
                //조회된 데이터 그리드에 세팅
                //DrawGrid(grdMain3, moddtMain3);
                //this.Cursor = Cursors.Default;

                //SetGridRowSelect(grdMain3, -1);
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void SetCombo(ComboBox cb, bool isTotalAdd)
        {
            string sql1 = string.Empty;
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.DataSource = null;
                cb.Items.Clear();

                sql1 = string.Format(@"SELECT HEAT_NO
                                         FROM TB_HEAT_PROG_MGMT
                                       WHERE HEAT_END_YN = 'N'");
                //sql1 = string.Format("select distinct ZONE_CD from {0} where ZONE_CD = '{1}' order by MOVE_ZONE_CD ASC", tableNM, _FromZoneId);
                DataTable dt = cd.FindDataTable(sql1);

                ArrayList arrType1 = new ArrayList();

                if (isTotalAdd)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        arrType1.Add(new DictionaryList(row["HEAT_NO"].ToString(), row["HEAT_NO"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }

                    cb.DataSource = arrType1;
                    cb.DisplayMember = "fnText";
                    cb.ValueMember = "fnValue";

                    //첫번째 아이템 선택
                    //cb.SelectedIndex = 0;
                    //cb.Selecteditem = ck.StrKey2;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            return;
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetData();
        }

        private void cbx_Eq_Gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCheckItemCB();

            SetData();
        }

        private void tb_UseCnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
