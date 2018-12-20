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
    public partial class UC_WORK_RANK : UC_ITEM_SIZE
    {
        public UC_WORK_RANK()
        {
            InitializeComponent();

            tbItemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        }


        [Description("WORK_RANK"), Category("Contents")]
        public string WORK_RANK
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

        protected override void TextChange()
        {
            WORK_RANK = tbItemSize.Text;
        }

        protected override void Key(object sender, KeyPressEventArgs e)
        {
            base.Key(sender, e);
        }

        protected override void tbKeyDown(object sender, KeyEventArgs e)
        {
            //base.tbKeyDown(sender, e);
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                SendKeys.Send("{TAB}");
                
            }
        }
    }
}
