using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLib;

namespace SystemControlClassLibrary.UC
{
    public partial class UC_ITEM_SIZE : UserControl
    {

        public VbFunc vf = new VbFunc();
        public ConnectDB cd = new ConnectDB();

        public UC_ITEM_SIZE()
        {
            InitializeComponent();

            tbItemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            tbItemSize.MaxLength = 4;
        }

        private void tbItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            Key(sender, e);
        }

        private void tbItemSize_TextChanged(object sender, EventArgs e)
        {
            TextChange();
        }

        protected virtual void TextChange()
        {
            ITEM_SIZE = tbItemSize.Text;
        }

        [Description("ITEM_SIZE"), Category("Contents")]
        public string ITEM_SIZE
        {
            get
            {
                return tbItemSize.Text;
            }
            set
            {

                tbItemSize.Text = value;
                //tbItemSize.Text = vf.Format(value, "0000");
                this.Invalidate();
            }
        }

        protected virtual void Key(object sender, KeyPressEventArgs e)
        {
            vf.KeyPressEvent_number(sender, e);
        }

        private void tbItemSize_KeyDown(object sender, KeyEventArgs e)
        {
            tbKeyDown(sender, e);
            
        }

        protected virtual void tbKeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                SendKeys.Send("{TAB}");
                tbItemSize.Text = vf.Format(tbItemSize.Text, "0000");
            }
        }
    }
}
