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
    public partial class UC_LINE_GP : UserControl
    {

        VbFunc vf = new VbFunc();
        ConnectDB cd = new ConnectDB();
        clsStyle cs = new clsStyle();
        clsCom ck = new clsCom();

        //string _Line_GP = "#1";
        string _Line_GP = "";

        public UC_LINE_GP()
        {
            InitializeComponent();
        }

        private void cbLine_gp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Line_GP = ((DictionaryList)cbLine_gp.SelectedItem).fnValue;
            ck.Line_gp = Line_GP;
        }

        [Description("Line GP"), Category("Contents")]
        public string Line_GP
        {
            get
            {
                //return ((DictionaryList)cbLine_gp.SelectedItem).fnValue;
                return _Line_GP;
            }
            set
            {
                //ck.Line_gp = value;
                //foreach (var item in cbLine_gp.Items)
                //{
                //    if (((DictionaryList)item).fnValue == value)
                //    {
                //        cbLine_gp.SelectedIndex = cbLine_gp.Items.IndexOf(item);
                //    }
                //}
                //this.Invalidate();
                
                foreach (var item in cbLine_gp.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbLine_gp.SelectedIndex = cbLine_gp.Items.IndexOf(item);
                        ck.Line_gp = value;
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

        protected virtual void SetupCombo()
        {
            cs.InitCombo(cbLine_gp, StringAlignment.Center);

            cd.SetCombo(cbLine_gp, "LINE_GP",  false, ck.Line_gp);
        }

        private void UC_LINE_GP_Load(object sender, EventArgs e)
        {
            //cd.SetCombo(cbLine_gp, "LINE_GP", "", false);
            SetupCombo();
        }
    }
}
