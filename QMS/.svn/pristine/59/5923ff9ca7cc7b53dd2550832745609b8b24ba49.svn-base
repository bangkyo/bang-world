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
    public partial class UC_Work_Day : UC_temp1
    {
        public UC_Work_Day()
        {
            InitializeComponent();
        }

        [Description("Work_Day"), Category("Contents")]
        public DateTime Work_Day
        {
            get
            {
                return dtpWorkDay.Value;
            }
            set
            {

                dtpWorkDay.Value = value;
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
        private void UC_Work_Day_Load(object sender, EventArgs e)
        {
            cs.InitDateEdit(dtpWorkDay);
            dtpWorkDay.Size = new Size(142, 23);
        }
    }
}
