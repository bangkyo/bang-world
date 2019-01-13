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
    public partial class UC_Item_size_s : UC_temp1
    {
        public UC_Item_size_s()
        {
            InitializeComponent();

            tbItemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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

                //tbItemSize.Text = value;
                tbItemSize.Text = vf.Format(value, "0000");
                this.Invalidate();
            }
        }
        [Description("ReadOnly"), Category("Contents")]
        public bool ReadOnly
        {
            get
            {
                return tbItemSize.ReadOnly;
            }
            set
            {

                tbItemSize.ReadOnly = value;
                this.Invalidate();
            }
        }

        private void tbItemSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            Key(sender, e);
        }

        private void Key(object sender, KeyPressEventArgs e)
        {
            
            vf.KeyPressEvent_number(sender, e);

        }

        private void UC_Item_size_s_Load(object sender, EventArgs e)
        {
            tbItemSize.MaxLength = 4;
        }

        private void tbItemSize_KeyDown(object sender, KeyEventArgs e)
        {
            int pKey = e.KeyValue;

            //엔터 눌렀을 시, //  Tab 눌렀을때.
            if (pKey == 13 || pKey == 9)
            {
                SendKeys.Send("{TAB}");
                tbItemSize.Text = vf.Format(tbItemSize.Text, "0000");
                //SetDataBinding();  // 조회 버튼을 통한 데이터입력
            }
        }
    }
}
