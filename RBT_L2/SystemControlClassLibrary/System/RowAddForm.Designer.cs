namespace SystemControlClassLibrary
{
    partial class RowAddForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RowAddForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSCRID = new C1.Win.C1Input.C1TextBox();
            this.txtPageID = new C1.Win.C1Input.C1TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBizGp = new C1.Win.C1Input.C1TextBox();
            this.txtSCRNM = new C1.Win.C1Input.C1TextBox();
            this.cboUse = new C1.Win.C1List.C1Combo();
            this.btnOK = new C1.Win.C1Input.C1Button();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSCRID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPageID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBizGp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSCRNM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUse)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.txtSCRID, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPageID, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtBizGp, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSCRNM, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cboUse, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(413, 194);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtSCRID
            // 
            this.txtSCRID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSCRID.Location = new System.Drawing.Point(93, 66);
            this.txtSCRID.Name = "txtSCRID";
            this.txtSCRID.Size = new System.Drawing.Size(100, 21);
            this.txtSCRID.TabIndex = 6;
            this.txtSCRID.Tag = null;
            // 
            // txtPageID
            // 
            this.txtPageID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPageID.Location = new System.Drawing.Point(300, 16);
            this.txtPageID.Name = "txtPageID";
            this.txtPageID.Size = new System.Drawing.Size(100, 21);
            this.txtPageID.TabIndex = 5;
            this.txtPageID.Tag = null;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("현대하모니 L", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(9, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "화면명칭";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("현대하모니 L", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "업무구분";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("현대하모니 L", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(13, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "화면 ID";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("현대하모니 L", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(211, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "PAGE ID";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("현대하모니 L", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(214, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "사용유무";
            // 
            // txtBizGp
            // 
            this.txtBizGp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtBizGp.Location = new System.Drawing.Point(93, 16);
            this.txtBizGp.Name = "txtBizGp";
            this.txtBizGp.Size = new System.Drawing.Size(100, 21);
            this.txtBizGp.TabIndex = 4;
            this.txtBizGp.Tag = null;
            // 
            // txtSCRNM
            // 
            this.txtSCRNM.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSCRNM.Location = new System.Drawing.Point(93, 116);
            this.txtSCRNM.Name = "txtSCRNM";
            this.txtSCRNM.Size = new System.Drawing.Size(100, 21);
            this.txtSCRNM.TabIndex = 7;
            this.txtSCRNM.Tag = null;
            // 
            // cboUse
            // 
            this.cboUse.AddItemSeparator = ';';
            this.cboUse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboUse.Caption = "";
            this.cboUse.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboUse.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.cboUse.EditorBackColor = System.Drawing.SystemColors.Window;
            this.cboUse.EditorFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboUse.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.cboUse.Images.Add(((System.Drawing.Image)(resources.GetObject("cboUse.Images"))));
            this.cboUse.Location = new System.Drawing.Point(297, 64);
            this.cboUse.MatchEntryTimeout = ((long)(2000));
            this.cboUse.MaxDropDownItems = ((short)(5));
            this.cboUse.MaxLength = 32767;
            this.cboUse.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.cboUse.Name = "cboUse";
            this.cboUse.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.cboUse.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.cboUse.Size = new System.Drawing.Size(105, 22);
            this.cboUse.TabIndex = 8;
            this.cboUse.Text = "c1Combo1";
            this.cboUse.PropBag = resources.GetString("cboUse.PropBag");
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.btnOK, 2);
            this.btnOK.Location = new System.Drawing.Point(127, 163);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.btnCancel, 2);
            this.btnCancel.Location = new System.Drawing.Point(208, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // RowAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 198);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RowAddForm";
            this.Text = "RowAddForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSCRID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPageID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBizGp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSCRNM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUse)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private C1.Win.C1Input.C1TextBox txtBizGp;
        private C1.Win.C1Input.C1TextBox txtSCRID;
        private C1.Win.C1Input.C1TextBox txtPageID;
        private C1.Win.C1Input.C1TextBox txtSCRNM;
        private C1.Win.C1List.C1Combo cboUse;
        private C1.Win.C1Input.C1Button btnOK;
        private C1.Win.C1Input.C1Button btnCancel;
    }
}