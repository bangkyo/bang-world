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
    public partial class UC_Work_Date_Fr_To : UC_temp1
    {
        public UC_Work_Date_Fr_To()
        {
            InitializeComponent();
        }

        [Description("Work_From_Date"), Category("Contents")]
        public DateTime Work_From_Date
        {
            get
            {
                return dtpWorkFr.Value;
            }
            set
            {

                dtpWorkFr.Value = value;
                this.Invalidate();
            }
        }

        [Description("Work_To_Date"), Category("Contents")]
        public DateTime Work_To_Date
        {
            get
            {
                return dtpWorkTo.Value;
            }
            set
            {

                dtpWorkTo.Value = value;
                this.Invalidate();
            }
        }

        private void UC_Work_Date_Fr_To_Load(object sender, EventArgs e)
        {
            cs.InitDateEdit(dtpWorkFr, DateTimePickerFormat.Custom);
            dtpWorkFr.CustomFormat = "yyyy-MM-dd  HH";
            //dtpWorkFr.Size = new Size(215, 24);

            cs.InitDateEdit(dtpWorkTo, DateTimePickerFormat.Custom);
            dtpWorkTo.CustomFormat = "yyyy-MM-dd  HH";
            //dtpWorkTo.Size = new Size(215, 24);
        }
    }
}
