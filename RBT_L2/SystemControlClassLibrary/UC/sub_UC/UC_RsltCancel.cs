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
using System.Collections;
using ComLib;

namespace SystemControlClassLibrary.UC.sub_UC
{
    public partial class UC_RsltCancel : UC_temp1
    {
        public UC_RsltCancel()
        {
            InitializeComponent();
        }

        [Description("SelectedIndexChanged"), Category("SelectedIndexChanged")]
        public event EventHandler GubunChangeEvent;

        string _gubun = string.Empty;

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

        private void cbGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGubun.SelectedItem != null)
            {
                GUBUN = ((DictionaryList)cbGubun.SelectedItem).fnValue;
                if (GubunChangeEvent != null)
                {
                    Invoke(GubunChangeEvent, null);

                }
                //GubunChangeEvent(sender, e);
            }

        }

        private void SetupCombo()
        {
            cs.InitCombo(cbGubun, StringAlignment.Center);

            cd.SetCombo(cbGubun, "WORKWR_CNCL_GP", "", false);

            ////cbGubun.DataSource = null;
            ////cbGubun.Items.Clear();

            //ArrayList arrType1 = new ArrayList();

            //arrType1.Add(new DictionaryList("최소대상", "A"));
            //arrType1.Add(new DictionaryList("최소결과", "B"));


            //cbGubun.DataSource = arrType1;
            //cbGubun.DisplayMember = "fnText";
            //cbGubun.ValueMember = "fnValue";
            //cbGubun.DropDownStyle = ComboBoxStyle.DropDownList;
            //cbGubun.SelectedIndex = 0;
        }

        private void UC_RsltCancel_Load(object sender, EventArgs e)
        {
            SetupCombo();

            
        }
    }
}
