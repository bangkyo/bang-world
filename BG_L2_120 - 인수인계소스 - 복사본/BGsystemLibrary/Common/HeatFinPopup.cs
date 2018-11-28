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
    public partial class HeatFinPopup : Form
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

        public HeatFinPopup(string _titleNM, string _menuNM, string _categoryNM)
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
            SqlCommand cmd = new SqlCommand();

            string spName = "SP_HEAT_END_PROC";


            string _heat_no = cbx_Heat_No.Text;

            if (string.IsNullOrEmpty(_heat_no))
            {
                MessageBox.Show("HEAT NO 를 선택해주세요.");
                return;
            }

            string proc_stat = "";
            string proc_msg = "";
            try
            {

                param.Clear();
                param.Add(SqlDbType.VarChar, "@P_HEAT_NO", _heat_no, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_USER_ID", ck.UserID, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_PROC_STAT", proc_stat, ParameterDirection.Output, 4000);
                param.Add(SqlDbType.VarChar, "@P_PROC_MSG", proc_msg, ParameterDirection.Output, 4000);


                this.Cursor = Cursors.AppStarting;
                cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                this.Cursor = Cursors.Default;

                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);

                if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                {
                    string _warning_msg = "HEAT_NO: " + _heat_no + "가 종료 실패하였습니다.:"+ cmd.Parameters["@P_PROC_MSG"].Value.ToString();
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                    return;
                }
                string _info_msg = "HEAT_NO: "+ _heat_no + "가 종료되었습니다.";
                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _info_msg);

                Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {
                
                //Button_Click(btnDisplay, null);
                //SelectFirstRow();
            }

        }


        private void HeatFinPopup_Load(object sender, EventArgs e)
        {
            InitControl();
        }

        private void InitControl()
        {
            SetCombo(cbx_Heat_No, false);

            SetData();
        }

        private void SetData()
        {
            string heatNo = cbx_Heat_No.Text;

            try
            {
                string sql = "";
                sql += string.Format(@"   SELECT HEAT_NO
                                               , (SELECT ISNULL(CD_NM, '') FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = STEEL)         AS STEEL_NM
                                               , (SELECT CD_NM         FROM TB_CM_COM_CD           WHERE CATEGORY = 'ITEM_SIZE' AND CD_ID = ITEM + ITEM_SIZE) AS ITEM_SIZE_NM
  	                                           , ISNULL(WORK_PCS, 0)  AS WORK_PCS
                                            FROM TB_SCHEDULE 
                                           WHERE HEAT_NO = '{0}'   ", heatNo);


                olddt = cd.FindDataTable(sql);

                tb_Steel.ReadOnly = false;
                tb_ItemSize.ReadOnly = false;
                tb_WorkCnt.ReadOnly = false;
                tb_Steel.Text = olddt.Rows[0]["STEEL_NM"].ToString();
                tb_ItemSize.Text = olddt.Rows[0]["ITEM_SIZE_NM"].ToString();
                tb_WorkCnt.Text = olddt.Rows[0]["WORK_PCS"].ToString();
                tb_Steel.ReadOnly = true;
                tb_ItemSize.ReadOnly = true;
                tb_WorkCnt.ReadOnly = true;



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
                                         FROM TB_SCHEDULE
                                        WHERE WORK_STAT <> 'END'");
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




        private void cbx_Heat_No_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetData();
        }
    }
}
