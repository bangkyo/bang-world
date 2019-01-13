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
    public partial class UC_HEAT : UC_temp1
    {
        public UC_HEAT()
        {
            InitializeComponent();

            tbHEAT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;

            tbHEAT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        }

        [Description("HEAT"), Category("Contents")]
        public string HEAT
        {
            get
            {
                return tbHEAT.Text;
            }
            set
            {

                tbHEAT.Text = value;
                this.Invalidate();
            }
        }

        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tbHEAT.ReadOnly;
            }
            set
            {

                tbHEAT.ReadOnly = value;
                this.Invalidate();
            }
        }
    }
}
