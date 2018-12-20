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
    public partial class UC_LENGTH : UC_ITEM_SIZE
    {
        public UC_LENGTH()
        {
            InitializeComponent();
        }


        [Description("Length"), Category("Contents")]
        public string Length
        {
            get
            {
                return tbItemSize.Text;
            }
            set
            {

                tbItemSize.Text = value;
                this.Invalidate();
            }
        }

        protected override void Key(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_decimal(sender, e);
        }

        protected override void TextChange()
        {
            Length = tbItemSize.Text;
        }

        private void UC_LENGTH_Load(object sender, EventArgs e)
        {
            tbItemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            tbItemSize.MaxLength = 10;
        }

        private void tbItemSize_TextChanged(object sender, EventArgs e)
        {

        }

        protected override void tbKeyDown(object sender, KeyEventArgs e)
        {
            //base.tbKeyDown(sender, e);
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                SendKeys.Send("{TAB}");
                //tbItemSize.Text = vf.Format(tbItemSize.Text, "0000");
            }
        }
    }
}
