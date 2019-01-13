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
using ComLib;
using System.Collections;

namespace SystemControlClassLibrary.UC.ReWork_UC
{
    public partial class UC_GuBun_RE : UC_temp1
    {

        //public event EventHandler GubunChangeEvent;

        string _gubun = string.Empty;

        public UC_GuBun_RE()
        {
            InitializeComponent();
        }

        [Description("Gubun"), Category("Contents")]
        public string GUBUN
        {
            get
            {

                return _gubun;

            }
            set
            {

                foreach (var item in cbGubun.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cbGubun.SelectedIndex = cbGubun.Items.IndexOf(item);
                        _gubun = value;
                    }
                }
                this.Invalidate();
            }
        }

        [Description("cb_Enable"), Category("Contents")]
        public bool cb_Enable
        {
            get
            {
                return cbGubun.Enabled;
            }
            set
            {

                cbGubun.Enabled = value;
                this.Invalidate();
            }
        }

        private void SetupCombo()
        {
            cs.InitCombo(cbGubun, StringAlignment.Center);

            //cd.SetCombo(cbGubun, "LINE_GP", "", false);

            cbGubun.DataSource = null;
            cbGubun.Items.Clear();

            ArrayList arrType1 = new ArrayList();

            arrType1.Add(new DictionaryList("재작업등록", "REG"));
            arrType1.Add(new DictionaryList("재작업등록취소", "CAN"));


            cbGubun.DataSource = arrType1;
            cbGubun.DisplayMember = "fnText";
            cbGubun.ValueMember = "fnValue";
            cbGubun.DropDownStyle = ComboBoxStyle.DropDownList;
            cbGubun.SelectedIndex = 0;
        }

        private void UC_GuBun_RE_Load(object sender, EventArgs e)
        {
            SetupCombo();
        }

        private void cbGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            GUBUN = ((DictionaryList)cbGubun.SelectedItem).fnValue;
            
            //GubunChangeEvent(sender, e);
        }
    }
}
