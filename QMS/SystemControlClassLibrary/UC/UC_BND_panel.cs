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
    public partial class UC_BND_panel : UserControl
    {
        private sub_UC.UC_BundleNo uC_BundleNo1;
        private sub_UC.UC_BundleNo uC_BundleNo2;
        private sub_UC.UC_BundleNo uC_BundleNo3;
        private sub_UC.UC_BundleNo uC_BundleNo4;
        private sub_UC.UC_BundleNo uC_BundleNo5;
        private sub_UC.UC_BundleNo uC_BundleNo6;
        private sub_UC.UC_BundleNo uC_BundleNo7;
        private sub_UC.UC_BundleNo uC_BundleNo8;
        private sub_UC.UC_BundleNo uC_BundleNo9;
        public UC_BND_panel()
        {
            InitializeComponent();
        }

        private void UC_BND_panel_Load(object sender, EventArgs e)
        {
            this.uC_BundleNo1 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo2 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo3 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo4 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo5 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo6 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo7 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo8 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            this.uC_BundleNo9 = new SystemControlClassLibrary.UC.sub_UC.UC_BundleNo();
            // 
            // uC_BundleNo1
            // 
            this.uC_BundleNo1.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo1.BundleNo = "";
            this.uC_BundleNo1.Location = new System.Drawing.Point(20, 13);
            this.uC_BundleNo1.Name = "uC_BundleNo1";
            this.uC_BundleNo1.ReadOnly = false;
            this.uC_BundleNo1.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo1.TabIndex = 0;
            // 
            // uC_BundleNo2
            // 
            this.uC_BundleNo2.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo2.BundleNo = "";
            this.uC_BundleNo2.Location = new System.Drawing.Point(20, 46);
            this.uC_BundleNo2.Name = "uC_BundleNo2";
            this.uC_BundleNo2.ReadOnly = false;
            this.uC_BundleNo2.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo2.TabIndex = 1;
            // 
            // uC_BundleNo3
            // 
            this.uC_BundleNo3.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo3.BundleNo = "";
            this.uC_BundleNo3.Location = new System.Drawing.Point(20, 79);
            this.uC_BundleNo3.Name = "uC_BundleNo3";
            this.uC_BundleNo3.ReadOnly = false;
            this.uC_BundleNo3.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo3.TabIndex = 1;
            // 
            // uC_BundleNo4
            // 
            this.uC_BundleNo4.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo4.BundleNo = "";
            this.uC_BundleNo4.Location = new System.Drawing.Point(361, 13);
            this.uC_BundleNo4.Name = "uC_BundleNo4";
            this.uC_BundleNo4.ReadOnly = false;
            this.uC_BundleNo4.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo4.TabIndex = 1;
            // 
            // uC_BundleNo5
            // 
            this.uC_BundleNo5.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo5.BundleNo = "";
            this.uC_BundleNo5.Location = new System.Drawing.Point(710, 13);
            this.uC_BundleNo5.Name = "uC_BundleNo5";
            this.uC_BundleNo5.ReadOnly = false;
            this.uC_BundleNo5.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo5.TabIndex = 1;
            // 
            // uC_BundleNo6
            // 
            this.uC_BundleNo6.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo6.BundleNo = "";
            this.uC_BundleNo6.Location = new System.Drawing.Point(361, 46);
            this.uC_BundleNo6.Name = "uC_BundleNo6";
            this.uC_BundleNo6.ReadOnly = false;
            this.uC_BundleNo6.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo6.TabIndex = 1;
            // 
            // uC_BundleNo7
            // 
            this.uC_BundleNo7.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo7.BundleNo = "";
            this.uC_BundleNo7.Location = new System.Drawing.Point(361, 79);
            this.uC_BundleNo7.Name = "uC_BundleNo7";
            this.uC_BundleNo7.ReadOnly = false;
            this.uC_BundleNo7.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo7.TabIndex = 1;
            // 
            // uC_BundleNo8
            // 
            this.uC_BundleNo8.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo8.BundleNo = "";
            this.uC_BundleNo8.Location = new System.Drawing.Point(710, 46);
            this.uC_BundleNo8.Name = "uC_BundleNo8";
            this.uC_BundleNo8.ReadOnly = false;
            this.uC_BundleNo8.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo8.TabIndex = 1;
            // 
            // uC_BundleNo9
            // 
            this.uC_BundleNo9.BackColor = System.Drawing.Color.Transparent;
            this.uC_BundleNo9.BundleNo = "";
            this.uC_BundleNo9.Location = new System.Drawing.Point(710, 79);
            this.uC_BundleNo9.Name = "uC_BundleNo9";
            this.uC_BundleNo9.ReadOnly = false;
            this.uC_BundleNo9.Size = new System.Drawing.Size(270, 27);
            this.uC_BundleNo9.TabIndex = 1;


            this.Controls.Add(this.uC_BundleNo5);
            this.Controls.Add(this.uC_BundleNo9);
            this.Controls.Add(this.uC_BundleNo8);
            this.Controls.Add(this.uC_BundleNo7);
            this.Controls.Add(this.uC_BundleNo6);
            this.Controls.Add(this.uC_BundleNo4);
            this.Controls.Add(this.uC_BundleNo3);
            this.Controls.Add(this.uC_BundleNo2);
            this.Controls.Add(this.uC_BundleNo1);
        }
    }
}
