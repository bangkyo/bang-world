namespace SystemControlClassLibrary.Results
{
    partial class BNDOrdUpd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BNDOrdUpd));
            this.cbFinSelect = new System.Windows.Forms.ComboBox();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.c1Label10 = new C1.Win.C1Input.C1Label();
            this.c1Label9 = new C1.Win.C1Input.C1Label();
            this.c1Label2 = new C1.Win.C1Input.C1Label();
            this.c1Label1 = new C1.Win.C1Input.C1Label();
            this.tbl_3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.tbOutLine = new C1.Win.C1Input.C1TextBox();
            this.txtPcs = new C1.Win.C1Input.C1TextBox();
            this.txtPOC = new C1.Win.C1Input.C1TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            this.tbl_3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbOutLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPcs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPOC)).BeginInit();
            this.SuspendLayout();
            // 
            // cbFinSelect
            // 
            this.cbFinSelect.FormattingEnabled = true;
            this.cbFinSelect.Location = new System.Drawing.Point(124, 106);
            this.cbFinSelect.Name = "cbFinSelect";
            this.cbFinSelect.Size = new System.Drawing.Size(115, 23);
            this.cbFinSelect.TabIndex = 10;
            this.cbFinSelect.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(49, 106);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 29);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "종료취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // c1Label10
            // 
            this.c1Label10.AutoSize = true;
            this.c1Label10.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label10.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label10.Location = new System.Drawing.Point(12, 109);
            this.c1Label10.Name = "c1Label10";
            this.c1Label10.Size = new System.Drawing.Size(74, 20);
            this.c1Label10.TabIndex = 59;
            this.c1Label10.Tag = null;
            this.c1Label10.Text = "종료여부";
            this.c1Label10.TextDetached = true;
            this.c1Label10.Visible = false;
            // 
            // c1Label9
            // 
            this.c1Label9.AutoSize = true;
            this.c1Label9.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label9.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label9.Location = new System.Drawing.Point(12, 69);
            this.c1Label9.Name = "c1Label9";
            this.c1Label9.Size = new System.Drawing.Size(89, 20);
            this.c1Label9.TabIndex = 58;
            this.c1Label9.Tag = null;
            this.c1Label9.Text = "바인딩본수";
            this.c1Label9.TextDetached = true;
            // 
            // c1Label2
            // 
            this.c1Label2.AutoSize = true;
            this.c1Label2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label2.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label2.Location = new System.Drawing.Point(15, 38);
            this.c1Label2.Name = "c1Label2";
            this.c1Label2.Size = new System.Drawing.Size(44, 17);
            this.c1Label2.TabIndex = 51;
            this.c1Label2.Tag = null;
            this.c1Label2.Text = "POC";
            this.c1Label2.TextDetached = true;
            // 
            // c1Label1
            // 
            this.c1Label1.AutoSize = true;
            this.c1Label1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label1.Location = new System.Drawing.Point(15, 10);
            this.c1Label1.Name = "c1Label1";
            this.c1Label1.Size = new System.Drawing.Size(42, 19);
            this.c1Label1.TabIndex = 50;
            this.c1Label1.Tag = null;
            this.c1Label1.Text = "라인";
            this.c1Label1.TextDetached = true;
            // 
            // tbl_3
            // 
            this.tbl_3.ColumnCount = 2;
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.Controls.Add(this.btnClose, 0, 0);
            this.tbl_3.Controls.Add(this.btnSave, 0, 0);
            this.tbl_3.Location = new System.Drawing.Point(15, 100);
            this.tbl_3.Margin = new System.Windows.Forms.Padding(0);
            this.tbl_3.Name = "tbl_3";
            this.tbl_3.RowCount = 1;
            this.tbl_3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.Size = new System.Drawing.Size(224, 35);
            this.tbl_3.TabIndex = 60;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(115, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 29);
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
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(106, 29);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "저장";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbOutLine
            // 
            this.tbOutLine.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbOutLine.Location = new System.Drawing.Point(123, 7);
            this.tbOutLine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbOutLine.Name = "tbOutLine";
            this.tbOutLine.ReadOnly = true;
            this.tbOutLine.Size = new System.Drawing.Size(115, 24);
            this.tbOutLine.TabIndex = 62;
            this.tbOutLine.Tag = null;
            this.tbOutLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbOutLine.TextDetached = true;
            // 
            // txtPcs
            // 
            this.txtPcs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPcs.EditFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.GeneralNumber;
            this.txtPcs.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((((C1.Win.C1Input.FormatInfoInheritFlags.CustomFormat | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.EmptyAsNull) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd)));
            this.txtPcs.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPcs.Location = new System.Drawing.Point(123, 66);
            this.txtPcs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPcs.Name = "txtPcs";
            this.txtPcs.Size = new System.Drawing.Size(114, 24);
            this.txtPcs.TabIndex = 64;
            this.txtPcs.Tag = null;
            this.txtPcs.TextDetached = true;
            this.txtPcs.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            // 
            // txtPOC
            // 
            this.txtPOC.AutoSize = false;
            this.txtPOC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPOC.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.txtPOC.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPOC.Location = new System.Drawing.Point(123, 35);
            this.txtPOC.MaxLength = 20;
            this.txtPOC.Name = "txtPOC";
            this.txtPOC.Size = new System.Drawing.Size(114, 24);
            this.txtPOC.TabIndex = 65;
            this.txtPOC.Tag = null;
            this.txtPOC.TextDetached = true;
            // 
            // BNDOrdUpd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(251, 149);
            this.Controls.Add(this.txtPOC);
            this.Controls.Add(this.txtPcs);
            this.Controls.Add(this.tbOutLine);
            this.Controls.Add(this.tbl_3);
            this.Controls.Add(this.cbFinSelect);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.c1Label10);
            this.Controls.Add(this.c1Label9);
            this.Controls.Add(this.c1Label2);
            this.Controls.Add(this.c1Label1);
            this.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "BNDOrdUpd";
            this.Text = "바인딩추가지시";
            this.Load += new System.EventHandler(this.BNDOrdUpd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.c1Label10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            this.tbl_3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbOutLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPcs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPOC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFinSelect;
        private C1.Win.C1Input.C1Button btnCancel;
        private C1.Win.C1Input.C1Label c1Label10;
        private C1.Win.C1Input.C1Label c1Label9;
        private C1.Win.C1Input.C1Label c1Label2;
        private C1.Win.C1Input.C1Label c1Label1;
        private System.Windows.Forms.TableLayoutPanel tbl_3;
        private C1.Win.C1Input.C1Button btnClose;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1TextBox tbOutLine;
        private C1.Win.C1Input.C1TextBox txtPcs;
        private C1.Win.C1Input.C1TextBox txtPOC;
    }
}