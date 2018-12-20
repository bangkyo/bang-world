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

namespace SystemControlClassLibrary.UC.ReWork_UC
{
    public partial class UC_ReWork_RSN : UC_temp1
    {
        public UC_ReWork_RSN()
        {
            InitializeComponent();
        }

        private void UC_ReWork_RSN_Load(object sender, EventArgs e)
        {
            cs.InitCombo(cb, StringAlignment.Center);

            cd.SetCombo(cb, "REWORK_RSN", false, "");
        }

        private string _ReWork;
        [Description("ReWork"), Category("Contents")]
        public string ReWork
        {
            get
            {
                return _ReWork;

            }
            set
            {
                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                        _ReWork = value;
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
                return cb.Enabled;
            }
            set
            {
                cb.Enabled = value;
                this.Invalidate();
            }
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReWork = ((DictionaryList)cb.SelectedItem).fnValue;
        }
    }
}
