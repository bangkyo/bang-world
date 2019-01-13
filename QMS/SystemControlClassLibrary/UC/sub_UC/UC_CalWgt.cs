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
    public partial class UC_CalWgt : UC_temp1
    {
        public UC_CalWgt()
        {
            InitializeComponent();

            tbCalWgt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        }

        [Description("CalWgt"), Category("Contents")]
        public string CalWgt
        {
            get
            {
                return tbCalWgt.Text;
            }
            set
            {

                tbCalWgt.Text = value;
                this.Invalidate();
            }
        }

        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tbCalWgt.ReadOnly;
            }
            set
            {

                tbCalWgt.ReadOnly = value;
                this.Invalidate();
            }
        }
    }
}
