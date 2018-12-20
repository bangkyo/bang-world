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

namespace SystemControlClassLibrary.UC.UC_temp
{
    public partial class UC_temp1 : UserControl
    {
        public VbFunc vf = new VbFunc();
        public ConnectDB cd = new ConnectDB();
        public clsStyle cs = new clsStyle();
        public UC_temp1()
        {
            InitializeComponent();
        }
    }
}
