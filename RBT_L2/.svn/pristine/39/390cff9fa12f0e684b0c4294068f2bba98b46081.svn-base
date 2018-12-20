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
    public partial class UC_Work_Type_s : UC_temp1
    {
        string _work_type = string.Empty;

        public UC_Work_Type_s()
        {
            InitializeComponent();
        }

        [Description("Work_Type"), Category("Contents")]
        public string Work_Type
        {
            get
            {
                //return ((DictionaryList)cbWork_Type.SelectedItem).fnValue;
                return _work_type;
            }
            set
            {

                foreach (var item in cbWork_Type.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbWork_Type.SelectedIndex = cbWork_Type.Items.IndexOf(item);
                        _work_type = value;
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

        private void UC_Work_Type_Load(object sender, EventArgs e)
        {
            SetupCombo();
        }

        private void SetupCombo()
        {
            //cs.InitCombo(cbWork_Type, StringAlignment.Center);

            cs.InitCombo(cbWork_Type, StringAlignment.Center);

            cd.SetCombo(cbWork_Type, "WORK_TYPE", "", true);
        }

        private void cbWork_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Work_Type = ((DictionaryList)cbWork_Type.SelectedItem).fnValue;
        }
    }
}
