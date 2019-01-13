using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControlClassLibrary.UC
{
    public partial class UC_DATE : UserControl
    {
        public UC_DATE()
        {
            InitializeComponent();

            Date = DateTime.Now;
        }

        public DateTime Date { set; get; }

        private void dt_dt_ValueChanged(object sender, EventArgs e)
        {
            Date = dt_dt.Value;
        }
    }
}
