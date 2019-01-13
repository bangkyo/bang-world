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
    public partial class UC_Time : UC_temp1
    {
        public UC_Time()
        {
            InitializeComponent();
        }

        [Description("Work_Time"), Category("Contents")]
        public DateTime Work_Time
        {
            get
            {
                return dtpWorkTime.Value;
            }
            set
            {
                dtpWorkTime.Value = value;
                this.Invalidate();
            }
        }

        [Description("Work_Time_Text"), Category("Contents")]
        public string Work_Time_Text
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
        private void UC_Time_Load(object sender, EventArgs e)
        {
            cs.InitTimeEdit(dtpWorkTime);
        }
    }
}
