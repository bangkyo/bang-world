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
    public partial class UC_OKNG : UC_temp1
    {
        string _okng = string.Empty;

        public UC_OKNG()
        {
            InitializeComponent();
        }

        [Description("OKNG"), Category("Contents")]
        public string OKNG
        {
            get
            {
                //if (cbOKNG == null)
                //{
                //    cbOKNG.SelectedIndex = -1;
                //    return "";
                //}
                //else
                //{
                //    return ((DictionaryList)cbOKNG.SelectedItem).fnValue;
                //}
                return _okng;

            }
            set
            {
                //if (cbOKNG == null)
                //{
                //    cbOKNG.SelectedIndex = -1;
                //}
                //else
                //{
                //    foreach (var item in cbOKNG.Items)
                //    {
                //        if (((DictionaryList)item).fnValue == value)
                //        {
                //            cbOKNG.SelectedIndex = cbOKNG.Items.IndexOf(item);
                //        }
                //    }
                //}
                foreach (var item in cbOKNG.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbOKNG.SelectedIndex = cbOKNG.Items.IndexOf(item);
                        _okng = value;
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
                return cbOKNG.Enabled;
            }
            set
            {

                cbOKNG.Enabled = value;
                this.Invalidate();
            }
        }

        private void SetupCombo()
        {
            cs.InitCombo(cbOKNG, StringAlignment.Center);

            cd.SetCombo(cbOKNG, "GOOD_NG", "", false);
        }

        private void UC_OKNG_Load(object sender, EventArgs e)
        {
            SetupCombo();
        }

        private void cbOKNG_SelectedIndexChanged(object sender, EventArgs e)
        {
            OKNG = ((DictionaryList)cbOKNG.SelectedItem).fnValue;
        }
    }
}
