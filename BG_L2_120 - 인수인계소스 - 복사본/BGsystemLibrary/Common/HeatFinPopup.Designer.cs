namespace BGsystemLibrary.Common
{
    partial class HeatFinPopup
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeatFinPopup));
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Steel = new System.Windows.Forms.Label();
            this.lb_ItemSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_WorkCnt = new C1.Win.C1Input.C1TextBox();
            this.tbl_3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.tb_Steel = new C1.Win.C1Input.C1TextBox();
            this.tb_ItemSize = new C1.Win.C1Input.C1TextBox();
            this.ttp_ItemSize_Cd = new System.Windows.Forms.ToolTip(this.components);
            this.cbx_Heat_No = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.tb_WorkCnt)).BeginInit();
            this.tbl_3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Steel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ItemSize)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(19, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 116;
            this.label1.Text = "HEAT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_Steel
            // 
            this.lb_Steel.AutoSize = true;
            this.lb_Steel.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Steel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lb_Steel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_Steel.Location = new System.Drawing.Point(16, 56);
            this.lb_Steel.Margin = new System.Windows.Forms.Padding(0);
            this.lb_Steel.Name = "lb_Steel";
            this.lb_Steel.Size = new System.Drawing.Size(31, 12);
            this.lb_Steel.TabIndex = 119;
            this.lb_Steel.Text = "강종";
            this.lb_Steel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_ItemSize
            // 
            this.lb_ItemSize.AutoSize = true;
            this.lb_ItemSize.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_ItemSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lb_ItemSize.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_ItemSize.Location = new System.Drawing.Point(16, 97);
            this.lb_ItemSize.Margin = new System.Windows.Forms.Padding(0);
            this.lb_ItemSize.Name = "lb_ItemSize";
            this.lb_ItemSize.Size = new System.Drawing.Size(31, 12);
            this.lb_ItemSize.TabIndex = 119;
            this.lb_ItemSize.Text = "규격";
            this.lb_ItemSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(16, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 116;
            this.label3.Text = "작업본수";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_WorkCnt
            // 
            this.tb_WorkCnt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_WorkCnt.Location = new System.Drawing.Point(112, 135);
            this.tb_WorkCnt.Name = "tb_WorkCnt";
            this.tb_WorkCnt.ReadOnly = true;
            this.tb_WorkCnt.Size = new System.Drawing.Size(155, 21);
            this.tb_WorkCnt.TabIndex = 4;
            this.tb_WorkCnt.Tag = null;
            this.tb_WorkCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbl_3
            // 
            this.tbl_3.ColumnCount = 2;
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.Controls.Add(this.btnClose, 0, 0);
            this.tbl_3.Controls.Add(this.btnSave, 0, 0);
            this.tbl_3.Location = new System.Drawing.Point(18, 181);
            this.tbl_3.Margin = new System.Windows.Forms.Padding(0);
            this.tbl_3.Name = "tbl_3";
            this.tbl_3.RowCount = 1;
            this.tbl_3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tbl_3.Size = new System.Drawing.Size(249, 35);
            this.tbl_3.TabIndex = 123;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(127, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(119, 29);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::BGsystemLibrary.Properties.Resources.filesave;
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(118, 29);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "저장";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tb_Steel
            // 
            this.tb_Steel.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Steel.Location = new System.Drawing.Point(112, 53);
            this.tb_Steel.Name = "tb_Steel";
            this.tb_Steel.NumericInput = false;
            this.tb_Steel.ReadOnly = true;
            this.tb_Steel.Size = new System.Drawing.Size(155, 21);
            this.tb_Steel.TabIndex = 2;
            this.tb_Steel.Tag = null;
            this.tb_Steel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_ItemSize
            // 
            this.tb_ItemSize.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_ItemSize.Location = new System.Drawing.Point(112, 94);
            this.tb_ItemSize.Name = "tb_ItemSize";
            this.tb_ItemSize.NumericInput = false;
            this.tb_ItemSize.ReadOnly = true;
            this.tb_ItemSize.Size = new System.Drawing.Size(155, 21);
            this.tb_ItemSize.TabIndex = 3;
            this.tb_ItemSize.Tag = null;
            this.tb_ItemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbx_Heat_No
            // 
            this.cbx_Heat_No.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Heat_No.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbx_Heat_No.FormattingEnabled = true;
            this.cbx_Heat_No.Location = new System.Drawing.Point(112, 12);
            this.cbx_Heat_No.Name = "cbx_Heat_No";
            this.cbx_Heat_No.Size = new System.Drawing.Size(152, 20);
            this.cbx_Heat_No.TabIndex = 124;
            this.cbx_Heat_No.SelectedIndexChanged += new System.EventHandler(this.cbx_Heat_No_SelectedIndexChanged);
            // 
            // HeatFinPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 234);
            this.Controls.Add(this.cbx_Heat_No);
            this.Controls.Add(this.tbl_3);
            this.Controls.Add(this.lb_ItemSize);
            this.Controls.Add(this.lb_Steel);
            this.Controls.Add(this.tb_WorkCnt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_ItemSize);
            this.Controls.Add(this.tb_Steel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "HeatFinPopup";
            this.Text = "HEAT 종료 처리";
            this.Load += new System.EventHandler(this.HeatFinPopup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tb_WorkCnt)).EndInit();
            this.tbl_3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tb_Steel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ItemSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Steel;
        private System.Windows.Forms.Label lb_ItemSize;
        private System.Windows.Forms.Label label3;
        private C1.Win.C1Input.C1TextBox tb_WorkCnt;
        private System.Windows.Forms.TableLayoutPanel tbl_3;
        private C1.Win.C1Input.C1Button btnClose;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1TextBox tb_Steel;
        private C1.Win.C1Input.C1TextBox tb_ItemSize;
        private System.Windows.Forms.ToolTip ttp_ItemSize_Cd;
        public System.Windows.Forms.ComboBox cbx_Heat_No;
    }
}