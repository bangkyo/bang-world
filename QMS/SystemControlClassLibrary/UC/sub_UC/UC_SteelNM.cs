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
    public partial class UC_SteelNM : UC_temp1
    {
        public UC_SteelNM()
        {
            InitializeComponent();

            tbSteelNM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        }


        [Description("SteelNM"), Category("Contents")]
        public string SteelNM
        {
            get
            {
                return tbSteelNM.Text;
            }
            set
            {

                tbSteelNM.Text = value;
                this.Invalidate();
            }
        }
        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tbSteelNM.ReadOnly;
            }
            set
            {

                tbSteelNM.ReadOnly = value;
                this.Invalidate();
            }
        }
    }
}
