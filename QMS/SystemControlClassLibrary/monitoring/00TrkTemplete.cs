using ComLib;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace SystemControlClassLibrary.monitoring
{
    public partial class _00TrkTemplete : Form
    {
        public _00TrkTemplete(string titleNm, string scrAuth, string factCode, string ownerNm)
        {
            ownerNM = ownerNm;
            titleNM = titleNm;

            InitializeComponent();
        }


        protected ConnectDB cd = new ConnectDB();
        protected VbFunc vf = new VbFunc();
        protected clsStyle cs = new clsStyle();
        protected clsCom ck = new clsCom();

        protected string ownerNM = "";
        protected string titleNM = "";

        protected DataTable moddt = null;

        protected string line_gp = "";

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnInsertReg_Click(object sender, EventArgs e)
        {

        }
    }
}
