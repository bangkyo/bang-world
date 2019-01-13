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
    public partial class UC_BND_PCS : UC_temp1
    {
        public UC_BND_PCS()
        {
            InitializeComponent();

            tbBNDPCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

        }

        [Description("BND_PCS"), Category("Contents")]
        public string BND_PCS
        {
            get
            {
                return tbBNDPCS.Text;
            }
            set
            {

                tbBNDPCS.Text = value;
                this.Invalidate();
            }
        }

        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tbBNDPCS.ReadOnly;
            }
            set
            {

                tbBNDPCS.ReadOnly = value;
                this.Invalidate();
            }
        }
    }
}
