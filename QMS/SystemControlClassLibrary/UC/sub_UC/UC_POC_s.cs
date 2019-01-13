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

namespace SystemControlClassLibrary.UC.sub_UC
{
    public partial class UC_POC_s : UC_temp1
    {
        public event EventHandler SearchEvent;
        public UC_POC_s()
        {
            InitializeComponent();

            tbPOC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;

            tbPOC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        }

        [Description("POC"), Category("Contents")]
        public string POC
        {
            get
            {
                return tbPOC.Text;
            }
            set
            {

                tbPOC.Text = value;
                this.Invalidate();
            }
        }
        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tbPOC.ReadOnly;
            }
            set
            {

                tbPOC.ReadOnly = value;
                this.Invalidate();
            }
        }

        private void tbPOC_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                //SetDataBinding();
                //SearchEvent(sender, e);
            }
        }
    }
}
