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

namespace SystemControlClassLibrary.UC.ReWork_UC
{
    public partial class UC_Textbox : UC_temp1
    {
        public UC_Textbox()
        {
            InitializeComponent();
        }

        private void UC_Textbox_Load(object sender, EventArgs e)
        {
            tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
        }

        [Description("tbText"), Category("Contents")]
        public string tbText
        {
            get
            {
                return tb.Text;
            }
            set
            {

                tb.Text = value;
                this.Invalidate();
            }
        }

        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tb.ReadOnly;
            }
            set
            {

                tb.ReadOnly = value;
                this.Invalidate();
            }
        }

        [Description("lbText"), Category("Contents")]
        public string lbText
        {
            get
            {
                return c1Label1.Text;
            }
            set
            {

                c1Label1.Text = value;
                this.Invalidate();
            }
        }
    }
}
