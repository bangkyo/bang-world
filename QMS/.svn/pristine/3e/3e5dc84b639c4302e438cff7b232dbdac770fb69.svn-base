using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControlClassLibrary.UC.UC_temp;
using ComLib;

namespace SystemControlClassLibrary.UC.sub_UC
{
    public partial class UC_Work_Team_s : UC_temp1
    {

        string _work_team = string.Empty;

        public UC_Work_Team_s()
        {
            InitializeComponent();
        }

        [Description("Work_Team"), Category("Contents")]
        public string Work_Team
        {
            get
            {
                //return ((ComLib.DictionaryList)cbWork_team.SelectedItem).fnValue;
                return _work_team;
            }
            set
            {
                foreach (var item in cbWork_team.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbWork_team.SelectedIndex = cbWork_team.Items.IndexOf(item);
                        _work_team = value;
                    }
                }
                //cbWork_team.ValueMember = value;
                this.Invalidate();
            }
        }

        [Description("cb_Enable"), Category("Contents")]
        public bool cb_Enable
        {
            get
            {
                return cbWork_team.Enabled;
            }
            set
            {

                cbWork_team.Enabled = value;
                this.Invalidate();
            }
        }

        private void UC_Work_Team_s_Load(object sender, EventArgs e)
        {
            SetupCombo();

        }

        private void SetupCombo()
        {
            cs.InitCombo(cbWork_team, StringAlignment.Center);

            cd.SetCombo(cbWork_team, "WORK_TEAM", "", false);
        }

        private void cbWork_team_SelectedIndexChanged(object sender, EventArgs e)
        {
            Work_Team = ((DictionaryList)cbWork_team.SelectedItem).fnValue;
        }
    }
}
