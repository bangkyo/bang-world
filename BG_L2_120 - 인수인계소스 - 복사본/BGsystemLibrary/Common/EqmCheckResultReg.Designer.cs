namespace BGsystemLibrary.Common
{
    partial class EqmCheckResultReg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EqmCheckResultReg));
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Steel = new System.Windows.Forms.Label();
            this.lb_ItemSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbl_3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.tb_checkCycle = new C1.Win.C1Input.C1TextBox();
            this.ttp_ItemSize_Cd = new System.Windows.Forms.ToolTip(this.components);
            this.cbx_Eq_Gp = new System.Windows.Forms.ComboBox();
            this.cbx_CheckItem_Gp = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_checkCnts = new C1.Win.C1Input.C1TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_UseCnt = new C1.Win.C1Input.C1TextBox();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.tbl_3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_checkCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_checkCnts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_UseCnt)).BeginInit();
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
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 116;
            this.label1.Text = "설비";
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
            this.lb_Steel.Size = new System.Drawing.Size(57, 12);
            this.lb_Steel.TabIndex = 119;
            this.lb_Steel.Text = "점검항목";
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
            this.lb_ItemSize.Size = new System.Drawing.Size(57, 12);
            this.lb_ItemSize.TabIndex = 119;
            this.lb_ItemSize.Text = "점검주기";
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
            this.label3.Text = "점검일자";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbl_3
            // 
            this.tbl_3.ColumnCount = 2;
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.Controls.Add(this.btnClose, 0, 0);
            this.tbl_3.Controls.Add(this.btnSave, 0, 0);
            this.tbl_3.Location = new System.Drawing.Point(180, 244);
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
            this.btnClose.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
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
            this.btnSave.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
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
            // tb_checkCycle
            // 
            this.tb_checkCycle.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_checkCycle.Location = new System.Drawing.Point(112, 94);
            this.tb_checkCycle.Name = "tb_checkCycle";
            this.tb_checkCycle.NumericInput = false;
            this.tb_checkCycle.ReadOnly = true;
            this.tb_checkCycle.Size = new System.Drawing.Size(155, 21);
            this.tb_checkCycle.TabIndex = 3;
            this.tb_checkCycle.Tag = null;
            // 
            // cbx_Eq_Gp
            // 
            this.cbx_Eq_Gp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Eq_Gp.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbx_Eq_Gp.FormattingEnabled = true;
            this.cbx_Eq_Gp.Location = new System.Drawing.Point(112, 12);
            this.cbx_Eq_Gp.Name = "cbx_Eq_Gp";
            this.cbx_Eq_Gp.Size = new System.Drawing.Size(152, 20);
            this.cbx_Eq_Gp.TabIndex = 124;
            this.cbx_Eq_Gp.SelectedIndexChanged += new System.EventHandler(this.cbx_Eq_Gp_SelectedIndexChanged);
            // 
            // cbx_CheckItem_Gp
            // 
            this.cbx_CheckItem_Gp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_CheckItem_Gp.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbx_CheckItem_Gp.FormattingEnabled = true;
            this.cbx_CheckItem_Gp.Location = new System.Drawing.Point(112, 53);
            this.cbx_CheckItem_Gp.Name = "cbx_CheckItem_Gp";
            this.cbx_CheckItem_Gp.Size = new System.Drawing.Size(152, 20);
            this.cbx_CheckItem_Gp.TabIndex = 124;
            this.cbx_CheckItem_Gp.SelectedIndexChanged += new System.EventHandler(this.cbx_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(16, 176);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 116;
            this.label2.Text = "점검내용";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_checkCnts
            // 
            this.tb_checkCnts.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_checkCnts.Location = new System.Drawing.Point(112, 173);
            this.tb_checkCnts.MaxLength = 50;
            this.tb_checkCnts.Name = "tb_checkCnts";
            this.tb_checkCnts.Size = new System.Drawing.Size(317, 21);
            this.tb_checkCnts.TabIndex = 4;
            this.tb_checkCnts.Tag = null;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(16, 212);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 116;
            this.label4.Text = "사용횟수";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_UseCnt
            // 
            this.tb_UseCnt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_UseCnt.Location = new System.Drawing.Point(112, 209);
            this.tb_UseCnt.MaxLength = 100;
            this.tb_UseCnt.Name = "tb_UseCnt";
            this.tb_UseCnt.Size = new System.Drawing.Size(155, 21);
            this.tb_UseCnt.TabIndex = 4;
            this.tb_UseCnt.Tag = null;
            this.tb_UseCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tb_UseCnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_UseCnt_KeyPress);
            // 
            // start_dt
            // 
            this.start_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.start_dt.Location = new System.Drawing.Point(112, 132);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(155, 21);
            this.start_dt.TabIndex = 125;
            // 
            // EqmCheckResultReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 286);
            this.Controls.Add(this.start_dt);
            this.Controls.Add(this.cbx_CheckItem_Gp);
            this.Controls.Add(this.cbx_Eq_Gp);
            this.Controls.Add(this.tbl_3);
            this.Controls.Add(this.lb_ItemSize);
            this.Controls.Add(this.lb_Steel);
            this.Controls.Add(this.tb_UseCnt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_checkCnts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_checkCycle);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EqmCheckResultReg";
            this.Text = "설비점검실적등록";
            this.Load += new System.EventHandler(this.EqmCheckResultReg_Load);
            this.tbl_3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tb_checkCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_checkCnts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_UseCnt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Steel;
        private System.Windows.Forms.Label lb_ItemSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tbl_3;
        private C1.Win.C1Input.C1Button btnClose;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1TextBox tb_checkCycle;
        private System.Windows.Forms.ToolTip ttp_ItemSize_Cd;
        public System.Windows.Forms.ComboBox cbx_Eq_Gp;
        public System.Windows.Forms.ComboBox cbx_CheckItem_Gp;
        private System.Windows.Forms.Label label2;
        private C1.Win.C1Input.C1TextBox tb_checkCnts;
        private System.Windows.Forms.Label label4;
        private C1.Win.C1Input.C1TextBox tb_UseCnt;
        private System.Windows.Forms.DateTimePicker start_dt;
    }
}