using System;
using System.Windows.Forms;

namespace BG_L2_120
{
    partial class Password
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Password));
            this.txtPwdCfm = new C1.Win.C1Input.C1TextBox();
            this.lblCaps = new System.Windows.Forms.Label();
            this.txtPwdNew = new C1.Win.C1Input.C1TextBox();
            this.txtPwdOld = new C1.Win.C1Input.C1TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdCfm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdOld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPwdCfm
            // 
            this.txtPwdCfm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwdCfm.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPwdCfm.Location = new System.Drawing.Point(182, 258);
            this.txtPwdCfm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPwdCfm.MaxLength = 20;
            this.txtPwdCfm.Name = "txtPwdCfm";
            this.txtPwdCfm.PasswordChar = '*';
            this.txtPwdCfm.Size = new System.Drawing.Size(171, 27);
            this.txtPwdCfm.TabIndex = 16;
            this.txtPwdCfm.Tag = null;
            this.txtPwdCfm.TextDetached = true;
            this.txtPwdCfm.Value = "";
            this.txtPwdCfm.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPwdCfm.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPwdCfm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwdCfm_KeyDown);
            // 
            // lblCaps
            // 
            this.lblCaps.AutoSize = true;
            this.lblCaps.BackColor = System.Drawing.Color.Transparent;
            this.lblCaps.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCaps.ForeColor = System.Drawing.Color.Red;
            this.lblCaps.Location = new System.Drawing.Point(144, 334);
            this.lblCaps.Name = "lblCaps";
            this.lblCaps.Size = new System.Drawing.Size(209, 19);
            this.lblCaps.TabIndex = 19;
            this.lblCaps.Text = "<Caps Lock>이 켜져 있습니다.";
            this.lblCaps.Visible = false;
            // 
            // txtPwdNew
            // 
            this.txtPwdNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwdNew.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPwdNew.Location = new System.Drawing.Point(182, 223);
            this.txtPwdNew.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPwdNew.MaxLength = 20;
            this.txtPwdNew.Name = "txtPwdNew";
            this.txtPwdNew.PasswordChar = '*';
            this.txtPwdNew.Size = new System.Drawing.Size(171, 27);
            this.txtPwdNew.TabIndex = 15;
            this.txtPwdNew.Tag = null;
            this.txtPwdNew.TextDetached = true;
            this.txtPwdNew.Value = "";
            this.txtPwdNew.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPwdNew.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPwdNew.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwdNew_KeyDown);
            // 
            // txtPwdOld
            // 
            this.txtPwdOld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwdOld.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPwdOld.Location = new System.Drawing.Point(182, 188);
            this.txtPwdOld.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPwdOld.MaxLength = 20;
            this.txtPwdOld.Name = "txtPwdOld";
            this.txtPwdOld.PasswordChar = '*';
            this.txtPwdOld.Size = new System.Drawing.Size(171, 27);
            this.txtPwdOld.TabIndex = 14;
            this.txtPwdOld.Tag = null;
            this.txtPwdOld.TextDetached = true;
            this.txtPwdOld.Value = "";
            this.txtPwdOld.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPwdOld.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPwdOld.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwdOld_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(268, 293);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 30);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(180, 293);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 30);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "변경";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(431, 181);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움체", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(30, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 18);
            this.label1.TabIndex = 23;
            this.label1.Text = "기존비밀번호";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(30, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 18);
            this.label2.TabIndex = 23;
            this.label2.Text = "신규비밀번호";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움체", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(30, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "비밀번호확인";
            // 
            // Password
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 401);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtPwdCfm);
            this.Controls.Add(this.lblCaps);
            this.Controls.Add(this.txtPwdNew);
            this.Controls.Add(this.txtPwdOld);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Password";
            this.Text = "비밀번호변경";
            this.Load += new System.EventHandler(this.Password_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Password_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdCfm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwdOld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private C1.Win.C1Input.C1TextBox txtPwdCfm;
        private System.Windows.Forms.Label lblCaps;
        private C1.Win.C1Input.C1TextBox txtPwdNew;
        private C1.Win.C1Input.C1TextBox txtPwdOld;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}