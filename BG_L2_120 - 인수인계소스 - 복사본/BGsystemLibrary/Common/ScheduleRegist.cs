using BGsystemLibrary.MatMgmt;
using ComLib;
using ComLib.clsMgr;
using System;
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
    public partial class TakeOverRegist : Form
    {
        //공통
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();
        VbFunc vf = new VbFunc();
        private clsFlexGrid clsFlexGrid = new clsFlexGrid();


        string txtdeptNm = "";

        public ScheduleMgmt frm;

        public delegate void FormSendDataHandler(object obj);
        public event FormSendDataHandler FormSendEvent;



        private string titleNM = "";
        private string menuNM = "";
        private string categoryNM = "";

        private string itemSizeHead = "";
        private string itemSizeIDTail = "";

        public TakeOverRegist(string _titleNM, string _menuNM, string _categoryNM)
        {
            titleNM = _titleNM;
            menuNM = _menuNM;
            categoryNM = _categoryNM;



            InitializeComponent();

            Text = titleNM;
            categoryNM = _categoryNM;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            // SP CALL 방식으로 인수 강제 등록
            clsParameterMember param = new clsParameterMember();

            SqlCommand cmd = new SqlCommand();

            string spName = "SP_MatTakeOverMgmt_CRE";

            string _heatNo = tb_Heat.Text;
            string _steel = tb_Steel.Text;
            string _itemCode = tb_ItemSize.Tag.ToString();

            
            int    _mfgPcs = int.Parse(tb_MFGpcs.Text);
            string _mfgDate = vf.Format(start_dt.Value,"yyyyMMdd");
            string _userId = ck.UserID;
            string _proc_stat = "";
            string _proc_msg = "";

            string show_msg = string.Empty;
            if (string.IsNullOrEmpty(_heatNo))
            {
                show_msg = "HEAT_NO 를 입력해주세요.";
                MessageBox.Show(show_msg);
                return;
            }

            if (string.IsNullOrEmpty(_steel))
            {
                show_msg = "강종 을 입력해주세요.";
                MessageBox.Show(show_msg);
                return;
            }

            if (string.IsNullOrEmpty(_itemCode))
            {
                show_msg = "규격 를 입력해주세요.";
                MessageBox.Show(show_msg);
                return;
            }

            if (string.IsNullOrEmpty(_mfgPcs.ToString()) || _mfgPcs == 0)
            {
                show_msg = "생산본수 를 1개이상 입력해주세요.";
                MessageBox.Show(show_msg);
                return;
            }

            try
            {

                param.Clear();
                param.Add(SqlDbType.VarChar, "@P_HEAT_NO", _heatNo, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_STEEL", _steel, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_ITEM_CODE", _itemCode, ParameterDirection.Input);
                param.Add(SqlDbType.Int, "@P_MFG_PCS", _mfgPcs, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "P_MFG_DATE", _mfgDate, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_USER_ID", _userId, ParameterDirection.Input);
                param.Add(SqlDbType.VarChar, "@P_PROC_STAT", _proc_stat, ParameterDirection.Output, 4000);
                param.Add(SqlDbType.VarChar, "@P_PROC_MSG", _proc_msg, ParameterDirection.Output, 4000);


                this.Cursor = Cursors.AppStarting;
                cmd = cd.ExecuteStoreProcedureRecCmd(spName, param);
                this.Cursor = Cursors.Default;


                //Console.WriteLine("@P_PROC_STAT : {0}", cmd.Parameters["@P_PROC_STAT"].Value);
                //Console.WriteLine("@P_PROC_MSG : {0}", cmd.Parameters["@P_PROC_MSG"].Value);

                if (cmd.Parameters["@P_PROC_STAT"].Value.ToString().Equals("ERR"))
                {
                    MessageBox.Show("@P_PROC_MSG:{0}"+ cmd.Parameters["@P_PROC_MSG"].Value.ToString());
                    string _warning_msg = "HEAT_NO: " + _heatNo + ", STEEL:" + _steel +", ITEM_CODE:"+_itemCode +", 생산본수:"+_mfgPcs +"생산일자:"+ _mfgDate + "가 인수강제등록 실패되었습니다.";
                    clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _warning_msg);
                    return;
                }

                string _info_msg = "HEAT_NO: " + _heatNo + ", STEEL:" + _steel + ", ITEM_CODE:" + _itemCode + ", 생산본수:" + _mfgPcs + "생산일자:" + _mfgDate + "가 인수강제등록되었습니다.";
                clsMsg.Log.Alarm(Alarms.Info, Text, clsMsg.Log.__Line(), _info_msg);

                Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ex.message : {0}", ex.Message);
            }
            finally
            {


                //SelectFirstRow();
            }

            
        }

        private int GetMaxSeq(string strDate)
        {
            int newSeq = 1;
            DataTable dt = new DataTable();
            try
            {
                string sql = "";
                sql += string.Format(" SELECT MAX(WORK_SEQ) +1    NEWSEQ                            ");
                sql += string.Format("  FROM TB_SCHEDULE                                            ");
                sql += string.Format(" WHERE CONVERT(varchar(10), WORK_PLN_DATE, 120) = '{0}'      ", strDate);

                dt = cd.FindDataTable(sql);

                if (dt == null)
                {
                    newSeq = 999;
                }
                newSeq = int.Parse(dt.Rows[0]["NEWSEQ"].ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show("조회에 실패하였습니다.: " + ex.Message);
            }



            return newSeq;
        }



        private void ScheduleRegist_Load(object sender, EventArgs e)
        {
            tb_ItemSize.Tag = string.Empty;
            tb_MFGpcs.Text = "1";
        }

        private void pbx_Search_Click(object sender, EventArgs e)
        {
            //ck.StrKey1 = "Y";
            //ck.StrKey2 = tb_Heat.Text;
            //HEAT 선택 팝업
            HeatPopup();
        }
        public string heatNo = "";     //HEAT NO
        private void HeatPopup()
        {
            ck.StrKey1 = tb_Heat.Text;

            HeatPopup dia = new HeatPopup();
            dia.Owner = this; //A폼을 지정하게 된다.
            dia.StartPosition = FormStartPosition.CenterScreen;
            dia.ShowDialog();
            if (ck.StrKey1 != "" || ck.StrKey2 !="")
            {
                tb_Heat.Text = ck.StrKey1;

                TempVarInitialize();
            }
        }

        private void TempVarInitialize()
        {
            ck.StrKey1 = "";
            ck.StrKey2 = "";
        }

        private void callback(object sender)
        {
            tb_Heat.Text = heatNo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbx_Search_Steel_Click(object sender, EventArgs e)
        {

            ck.StrKey1 = tb_Steel.Text;

            SteelPopup dia = new SteelPopup();
            dia.Owner = this; //A폼을 지정하게 된다.
            dia.StartPosition = FormStartPosition.CenterScreen;
            dia.ShowDialog();
            if (ck.StrKey1 != "" || ck.StrKey2 != "")
            {
                tb_Steel.Text = ck.StrKey2;

                TempVarInitialize();
            }
        }

        private void pbx_Search_ItemSize_Click(object sender, EventArgs e)
        {
            itemSizeHead = "";
            itemSizeIDTail = "";
            //ck.StrKey1 = tb_ItemSize.Text;
            ttp_ItemSize_Cd.SetToolTip(tb_ItemSize, "");
            TempVarInitialize();

            ItemSizePopup dia = new ItemSizePopup();
            dia.Owner = this; //A폼을 지정하게 된다.
            dia.StartPosition = FormStartPosition.CenterScreen;
            dia.ShowDialog();
            if (ck.StrKey1 != "" || ck.StrKey2 != "")
            {
                itemSizeHead = ck.StrKey1.Substring(0, 2);
                itemSizeIDTail = ck.StrKey1.Substring(2);

                tb_ItemSize.ReadOnly = false;
                tb_ItemSize.Text = ck.StrKey2;
                tb_ItemSize.Tag = ck.StrKey1;
                tb_ItemSize.ReadOnly = true;
                ttp_ItemSize_Cd.SetToolTip(tb_ItemSize, ck.StrKey1);
                TempVarInitialize();
            }
        }

        private void c1TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tb_Seq_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
