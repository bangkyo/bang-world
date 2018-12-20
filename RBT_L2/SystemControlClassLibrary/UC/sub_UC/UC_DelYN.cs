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
    public partial class UC_DelYN : UC_temp1
    {

        string _delYN = string.Empty;

        public UC_DelYN()
        {
            InitializeComponent();
        }

        [Description("DelYN"), Category("Contents")]
        public string DelYN
        {
            get
            {
                //return ((ComLib.DictionaryList)cbDelYN.SelectedItem).fnValue;
                return _delYN;
            }
            set
            {
                foreach (var item in cbDelYN.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbDelYN.SelectedIndex = cbDelYN.Items.IndexOf(item);
                        _delYN = value;
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
                return cbDelYN.Enabled;
            }
            set
            {

                cbDelYN.Enabled = value;
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

        private void SetupCombo()
        {
            cs.InitCombo(cbDelYN, StringAlignment.Center);

            cd.SetComboYN(cbDelYN);
        }

        private void UC_DelYN_Load(object sender, EventArgs e)
        {
            SetupCombo();
        }

        private void cbDelYN_SelectedIndexChanged(object sender, EventArgs e)
        {
            DelYN = ((DictionaryList)cbDelYN.SelectedItem).fnValue;
        }
    }
}
