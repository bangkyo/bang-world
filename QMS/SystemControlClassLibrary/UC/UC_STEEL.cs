using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLib;
using SystemControlClassLibrary.Popup;

namespace SystemControlClassLibrary.UC
{
    public partial class UC_STEEL : UserControl
    {

        ConnectDB cd = new ConnectDB();
        clsCom ck = new clsCom();

        public UC_STEEL()
        {
            InitializeComponent();
        }

        public string Steel
        {
            get
            {
                return gangjong_id_tb.Text;
            }

            set
            {
                gangjong_id_tb.Text = value;
                this.Invalidate();
            }
        }

        public string Steel_NM
        {
            get
            {
                return gangjong_Nm_tb.Text;
            }

            set
            {
                gangjong_Nm_tb.Text = value;
                this.Invalidate();
            }
        }

        private void UC_STEEL_Load(object sender, EventArgs e)
        {
            gangjong_id_tb.LostFocus += Gangjong_id_tb_LostFocus;

            gangjong_Nm_tb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;

            gangjong_id_tb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
        }

        private void Gangjong_id_tb_LostFocus(object sender, EventArgs e)
        {
            if (gangjong_id_tb.Text == "")
            {
                gangjong_Nm_tb.Text = "";
                Steel = "";
            }
            else
            {
                gangjong_Nm_tb.Text = cd.Find_Steel_NM_By_ID(gangjong_id_tb.Text);

                if (gangjong_Nm_tb.Text.Length == 0)
                {
                    if (MessageBox.Show(" 해당 강종에 따른 강종명을 찾을 수 없습니다.", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        gangjong_Nm_tb.Text = "";
                        gangjong_id_tb.Focus();
                        return;
                    }
                }
                else
                    Steel = gangjong_id_tb.Text;
            }
        }

        private void gangjong_id_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] Key는 다음 컨트롤로 이동
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        private void gangjong_id_tb_TextChanged(object sender, EventArgs e)
        {
            gangjong_Nm_tb.Text = "";
            Steel = gangjong_id_tb.Text;
        }

        private void gangjong_Nm_tb_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSteel_Click(object sender, EventArgs e)
        {
            SearchSteelNm popup = new SearchSteelNm();
            //popup.Owner = this; //A폼을 지정하게 된다.
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.ShowDialog();
            if (ck.StrKey1 != "")
            {
                gangjong_id_tb.Text = ck.StrKey1;
                gangjong_Nm_tb.Text = ck.StrKey2;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
