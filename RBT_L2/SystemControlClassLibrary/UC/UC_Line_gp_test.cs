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

namespace SystemControlClassLibrary.UC
{
    public partial class UC_Line_gp_test : UserControl
    {

        VbFunc vf = new VbFunc();
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        //clsCom ck = new clsCom();

        string _Line_GP = "";

        public UC_Line_gp_test()
        {
            InitializeComponent();
        }

        private void UC_Line_gp_test_Load(object sender, EventArgs e)
        {
            //cd.SetCombo(cbLine_gp, "LINE_GP", "", false);
            SetupCombo();
        }

        protected virtual void SetupCombo()
        {
            cs.InitCombo(cbLine_gp, StringAlignment.Center);

            //cd.SetCombo(cbLine_gp, "LINE_GP", false, "");
            cd.SetCombo(cbLine_gp, "LINE_GP", "", false);
        }

        [Description("Line GP"), Category("Contents")]
        public string Line_GP
        {
            get
            {
                return _Line_GP;
            }
            set
            {
                
                
                foreach (var item in cbLine_gp.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbLine_gp.SelectedIndex = cbLine_gp.Items.IndexOf(item);
                        _Line_GP = value;
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
                return cbLine_gp.Enabled;
            }
            set
            {

                cbLine_gp.Enabled = value;
                this.Invalidate();
            }
        }

        private void cbLine_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Line_GP = ((DictionaryList)cbLine_gp.SelectedItem).fnValue;
            //ck.Line_gp = Line_GP;
        }
    }
}
