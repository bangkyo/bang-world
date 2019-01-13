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
    public partial class UC_Line_gp : UC_temp1
    {

        string _line_gp = string.Empty;

        public UC_Line_gp()
        {
            InitializeComponent();
        }

        [Description("Line GP"), Category("Contents")]
        public string Line_GP
        {
            get
            {
                //if (cbLine == null)
                //{
                //    cbLine.SelectedIndex = -1;
                //    return "";
                //}
                //else
                //{
                //    return ((ComLib.DictionaryList)cbLine.SelectedItem).fnValue;
                //}
                //return ((ComLib.DictionaryList)cbLine.SelectedItem).fnValue;
                return _line_gp;

            }
            set
            {
                //if (cbLine == null)
                //{
                //    cbLine.SelectedIndex = -1;
                //}
                //else
                //{
                //    foreach (var item in cbLine.Items)
                //    {
                //        if (((DictionaryList)item).fnValue == value)
                //        {
                //            cbLine.SelectedIndex = cbLine.Items.IndexOf(item);
                //        }
                //    }
                //}

                foreach (var item in cbLine.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbLine.SelectedIndex = cbLine.Items.IndexOf(item);
                        _line_gp = value;
                    }
                }
                //cbLine.SelectedValue = value;
                this.Invalidate();
            }
        }

        [Description("cb_Enable"), Category("Contents")]
        public bool cb_Enable
        {
            get
            {
                return cbLine.Enabled;
            }
            set
            {

                cbLine.Enabled = value;
                this.Invalidate();
            }
        }

        private void SetupCombo()
        {
            cs.InitCombo(cbLine, StringAlignment.Center);

            cd.SetCombo(cbLine, "LINE_GP", "", false);
        }

        private void UC_Line_gp_Load(object sender, EventArgs e)
        {
            SetupCombo();

            //cbLine.KeyPress += (sndr, eva) => eva.Handled = true;
        }

        private void cbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            Line_GP = ((DictionaryList)cbLine.SelectedItem).fnValue;
        }
    }
}
