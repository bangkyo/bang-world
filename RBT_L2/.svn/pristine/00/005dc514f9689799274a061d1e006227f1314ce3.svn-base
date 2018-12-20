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
    public partial class UC_Work_Team : UC_temp1
    {
        public UC_Work_Team()
        {
            InitializeComponent();
        }

        [Description("Work_Team"), Category("Contents")]
        public string Work_Team
        {
            get
            {
                return ((ComLib.DictionaryList)cbWork_team.SelectedItem).fnValue;
            }
            set
            {
                foreach (var item in cbWork_team.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbWork_team.SelectedIndex = cbWork_team.Items.IndexOf(item);
                    }
                }
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

        public Color LabelForeColor
        {
            get
            {
                return c1Label1.ForeColor;
            }
            set
            {

                c1Label1.ForeColor = value;
                this.Invalidate();
            }
        }

        private void UC_Work_Team_Load(object sender, EventArgs e)
        {
            SetupCombo();
            
        }

        private void SetupCombo()
        {
            cs.InitCombo(cbWork_team, StringAlignment.Center);

            cd.SetCombo(cbWork_team, "WORK_TEAM", "", false);
        }


    }
}
