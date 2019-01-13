using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RBT_L2
{
    public partial class ProgressBar : Form
    {
        ApplicationDeployment ad;

        public ProgressBar(ApplicationDeployment _ad)
        {
            InitializeComponent();

            ad = _ad;
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {

            ad.UpdateCompleted += Ad_UpdateCompleted; 

            ad.UpdateProgressChanged += Ad_UpdateProgressChanged; 
            ad.UpdateAsync();

        }

        private void Ad_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Ad_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Close();
        }

    }
}
