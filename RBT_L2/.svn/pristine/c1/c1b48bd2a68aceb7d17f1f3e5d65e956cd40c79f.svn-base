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

namespace SystemControlClassLibrary.UC.ReWork_UC
{
    public partial class UC_Input_Zone : UC_temp1
    {

        string _zone = string.Empty;
        string _line_gp = "#1";

        public UC_Input_Zone()
        {
            InitializeComponent();
        }

        [Description("Zone"), Category("Contents")]
        public string Zone
        {
            get
            {
                return _zone;
            }
            set
            {

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == value)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                        _zone = value;
                    }
                }
                this.Invalidate();
            }
        }


        [Description("Line_gp"), Category("Contents")]
        public string Line_gp
        {
            get
            {
                return _line_gp;
            }
            set
            {
                if (_line_gp != value)
                {
                    //SetupCombo(value);
                    _line_gp = value;
                }
                
            }
        }

        public void SetupCombo(string __line_gp)
        {
            // 라인 정보로 검색해서 가능 zone 정보를 가져옴..
            //SetComboZoneInfo
            cs.InitCombo(cb, StringAlignment.Center);

            cd.SetComboZoneInfo(cb, __line_gp);
        }

        [Description("cb_Enable"), Category("Contents")]
        public bool cb_Enable
        {
            get
            {
                return cb.Enabled;
            }
            set
            {

                cb.Enabled = value;
                this.Invalidate();
            }
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb.SelectedItem != null)
            {
                Zone = ((DictionaryList)cb.SelectedItem).fnValue;
            }
            
        }

        private void UC_Input_Zone_Load(object sender, EventArgs e)
        {

            //Line_gp = "#1";

            SetupCombo(Line_gp);
        }
    }
}
