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
    public partial class UC_TextBox : UC_temp1
    {
        public UC_TextBox()
        {
            InitializeComponent();
        }

        [Description("tbText"), Category("Contents")]
        public string TbText
        {
            get
            {
                return tbText.Text;
            }
            set
            {

                //_Line_GP = ((ComLib.DictionaryList)cbLine_gp.SelectedItem).fnValue;
                tbText.Text = value;
                this.Invalidate();
            }
        }

        [Description("LableText"), Category("Contents")]
        public string LableText
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


        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tbText.ReadOnly;
            }
            set
            {

                tbText.ReadOnly = value;
                this.Invalidate();
            }
        }


    }
}
