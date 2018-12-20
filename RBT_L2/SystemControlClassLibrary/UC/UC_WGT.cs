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
    public partial class UC_WGT : UC_ITEM_SIZE
    {
        public UC_WGT()
        {
            InitializeComponent();

            tbItemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        }


        [Description("WGT"), Category("Contents")]
        public string WGT
        {
            get
            {
                return tbItemSize.Text;
            }
            set
            {

                tbItemSize.Text = value;
                //tbItemSize

                this.Invalidate();
            }
        }


        protected override void TextChange()
        {
            WGT = tbItemSize.Text;
        }

        protected override void Key(object sender, KeyPressEventArgs e)
        {
           // base.Key(sender, e);
            vf.KeyPressEvent_None(sender, e);
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
