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
    public partial class UC_Work_Type : UC_temp1
    {
        public UC_Work_Type()
        {
            InitializeComponent();
        }

        [Description("Work_Type"), Category("Contents")]
        public string Work_Type
        {
            get
            {
                return ((ComLib.DictionaryList)cbWork_Type.SelectedItem).fnValue;
            }
            set
            {

                foreach (var item in cbWork_Type.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbWork_Type.SelectedIndex = cbWork_Type.Items.IndexOf(item);
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
                return cbWork_Type.Enabled;
            }
            set
            {

                cbWork_Type.Enabled = value;
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

        private void UC_Work_Type_Load(object sender, EventArgs e)
        {
            SetupCombo();
        }

        private void SetupCombo()
        {
            cs.InitCombo(cbWork_Type, StringAlignment.Center);

            cd.SetCombo(cbWork_Type, "WORK_TYPE", "", false);
        }

    }
}
