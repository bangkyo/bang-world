using C1.Win.C1List;
using ComLib;
using System.Windows.Forms;

namespace SystemControlClassLibrary
{
    public partial class RowAddForm : Form
    {
        private string strUse;
        public RowAddForm()
        {
            InitializeComponent();

            Load += RowAddForm_Load;
            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;
            cboUse.SelectionChangeCommitted += CboUse_SelectionChangeCommitted;
        }

        private void CboUse_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            strUse = cboUse.SelectedText;
        }

        private void RowAddForm_Load(object sender, System.EventArgs e)
        {
            SetComboBox();
        }

        private void SetComboBox()
        {
            cboUse.ClearItems();
            cboUse.DataMode = DataModeEnum.AddItem;
            cboUse.AddItem("N");
            cboUse.AddItem("Y");
            cboUse.SelectedIndex = 0;
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, System.EventArgs e)
        {
            //if(DialogResult.Yes == MessageBox.Show("정보를 추가하시겠습니까?","행추가",MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //{
            //    var MaxRow = string.Empty;
            //    strUse = (strUse == null) ? "N" : strUse;
            //    MaxRow = clsUtil.Utl.GetiniValue("SCR_CONTROL", "SCRCNTL_MAXROW");
            //    string temp = "|" + MaxRow + "|" + txtBizGp.Text + "|" + txtSCRID.Text + "|" + txtSCRNM.Text + "|" + txtPageID.Text + "|" + strUse;
            //    //clsUtil.Utl.SetiniValue("ROW_ADD", "SCRCNTL", temp);
            //    this.DialogResult = DialogResult.OK;
            //}
            //else
            //{
            //    //this.DialogResult = DialogResult.Cancel;
            //}
        }
    }
}
