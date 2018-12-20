using ComLib;
using ComLib.clsMgr;
using System;
using System.Data.OracleClient;
using System.Windows.Forms;

namespace SystemControlClassLibrary.monitoring
{
    public partial class Change_Work_popup : Form
    {
        ConnectDB cd = new ConnectDB();

        private UC.sub_UC.UC_Work_Team uC_Work_Team1;
        private UC.sub_UC.UC_Work_Type uC_Work_Type1;
        private UC.sub_UC.UC_Line_gp uC_Line_gp1;

        private string line_gp = string.Empty;
        private string work_type = string.Empty;
        private string work_team = string.Empty;

        public Change_Work_popup(string _line_gp, string _work_type, string _work_team)
        {
            line_gp = _line_gp;
            work_type = _work_type.Substring(0, 1);
            work_team = _work_team.Substring(0, 1);

            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Change_Work_popup_Load(object sender, EventArgs e)
        {
            MinimizeBox = false;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
         
            initControl();
        }

        private void initControl()
        {
            #region 유저컨트롤 설정
            this.uC_Work_Team1 = new SystemControlClassLibrary.UC.sub_UC.UC_Work_Team();
            this.uC_Work_Type1 = new SystemControlClassLibrary.UC.sub_UC.UC_Work_Type();
            this.uC_Line_gp1 = new SystemControlClassLibrary.UC.sub_UC.UC_Line_gp();

            // 
            // uC_Work_Team1
            // 
            this.uC_Work_Team1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Work_Team1.cb_Enable = true;
            this.uC_Work_Team1.Location = new System.Drawing.Point(13, 77);
            this.uC_Work_Team1.Name = "uC_Work_Team1";
            this.uC_Work_Team1.Size = new System.Drawing.Size(270, 27);
            this.uC_Work_Team1.TabIndex = 11;
            
            // 
            // uC_Work_Type1
            // 
            this.uC_Work_Type1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Work_Type1.cb_Enable = true;
            this.uC_Work_Type1.Location = new System.Drawing.Point(13, 44);
            this.uC_Work_Type1.Name = "uC_Work_Type1";
            this.uC_Work_Type1.Size = new System.Drawing.Size(270, 27);
            this.uC_Work_Type1.TabIndex = 12;
            
            // 
            // uC_Line_gp1
            // 
            this.uC_Line_gp1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Line_gp1.cb_Enable = true;
            
            this.uC_Line_gp1.Location = new System.Drawing.Point(13, 11);
            this.uC_Line_gp1.Name = "uC_Line_gp1";
            this.uC_Line_gp1.Size = new System.Drawing.Size(270, 27);
            this.uC_Line_gp1.TabIndex = 13;
            this.uC_Line_gp1.cb_Enable = false;


            this.Controls.Add(this.uC_Line_gp1);
            this.Controls.Add(this.uC_Work_Type1);
            this.Controls.Add(this.uC_Work_Team1);

            this.uC_Work_Team1.Work_Team = work_team;
            this.uC_Work_Type1.Work_Type = work_type;
            this.uC_Line_gp1.Line_GP = line_gp;
            #endregion
        }

        private void sel_btn_Click(object sender, EventArgs e)
        {
            //수정된 라인, 근, 조 를 해당 라인을 키로 수정한다.

            OracleConnection conn = cd.OConnect();

            OracleCommand cmd = new OracleCommand();
            OracleTransaction transaction = null;
            string sql1 = string.Empty;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                sql1 += string.Format("UPDATE TB_LINE_WORK_TEAM ");
                sql1 += string.Format("SET ");
                sql1 += string.Format("    WORK_TYPE = '{0}' ", uC_Work_Type1.Work_Type);
                sql1 += string.Format("   ,WORK_TEAM = '{0}' ", uC_Work_Team1.Work_Team);
                sql1 += string.Format("WHERE ");
                sql1 += string.Format("    LINE_GP = '{0}' ", uC_Line_gp1.Line_GP);

                cmd.CommandText = sql1;
                cmd.ExecuteNonQuery();

                //실행후 성공
                transaction.Commit();

                //this.Close();

                string message = "정상적으로 저장되었습니다.";

                clsMsg.Log.Alarm(Alarms.Info, clsMsg.Log.__Function(), clsMsg.Log.__Line(), message);

                btnClose_Click(null, null);
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

            //this.Close();
        }
    }
}
