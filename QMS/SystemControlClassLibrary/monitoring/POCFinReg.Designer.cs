namespace SystemControlClassLibrary.monitoring
{
    partial class POCFinReg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POCFinReg));
            this.cbFinSelect = new System.Windows.Forms.ComboBox();
            this.tbHEAT = new C1.Win.C1Input.C1TextBox();
            this.cbOutLine = new System.Windows.Forms.ComboBox();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.tbBNDPcs = new C1.Win.C1Input.C1TextBox();
            this.tbSTRPcs = new C1.Win.C1Input.C1TextBox();
            this.tbMillPcs = new C1.Win.C1Input.C1TextBox();
            this.tbLength = new C1.Win.C1Input.C1TextBox();
            this.tbSize = new C1.Win.C1Input.C1TextBox();
            this.tbSteel = new C1.Win.C1Input.C1TextBox();
            this.c1Label10 = new C1.Win.C1Input.C1Label();
            this.c1Label9 = new C1.Win.C1Input.C1Label();
            this.c1Label8 = new C1.Win.C1Input.C1Label();
            this.c1Label7 = new C1.Win.C1Input.C1Label();
            this.c1Label6 = new C1.Win.C1Input.C1Label();
            this.c1Label5 = new C1.Win.C1Input.C1Label();
            this.c1Label4 = new C1.Win.C1Input.C1Label();
            this.c1Label3 = new C1.Win.C1Input.C1Label();
            this.c1Label2 = new C1.Win.C1Input.C1Label();
            this.c1Label1 = new C1.Win.C1Input.C1Label();
            this.cbPOC = new System.Windows.Forms.ComboBox();
            this.tbl_3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbHEAT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBNDPcs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSTRPcs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMillPcs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSteel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            this.tbl_3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbFinSelect
            // 
            this.cbFinSelect.FormattingEnabled = true;
            this.cbFinSelect.Location = new System.Drawing.Point(124, 277);
            this.cbFinSelect.Name = "cbFinSelect";
            this.cbFinSelect.Size = new System.Drawing.Size(115, 23);
            this.cbFinSelect.TabIndex = 10;
            this.cbFinSelect.Visible = false;
            this.cbFinSelect.SelectedIndexChanged += new System.EventHandler(this.cbFinSelect_SelectedIndexChanged);
            // 
            // tbHEAT
            // 
            this.tbHEAT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbHEAT.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbHEAT.Location = new System.Drawing.Point(123, 65);
            this.tbHEAT.MaxLength = 15;
            this.tbHEAT.Name = "tbHEAT";
            this.tbHEAT.ReadOnly = true;
            this.tbHEAT.Size = new System.Drawing.Size(115, 24);
            this.tbHEAT.TabIndex = 3;
            this.tbHEAT.Tag = null;
            this.tbHEAT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbHEAT.TextDetached = true;
            this.tbHEAT.TrimEnd = false;
            // 
            // cbOutLine
            // 
            this.cbOutLine.Enabled = false;
            this.cbOutLine.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbOutLine.FormattingEnabled = true;
            this.cbOutLine.Location = new System.Drawing.Point(124, 7);
            this.cbOutLine.Name = "cbOutLine";
            this.cbOutLine.Size = new System.Drawing.Size(114, 23);
            this.cbOutLine.TabIndex = 0;
            this.cbOutLine.SelectedIndexChanged += new System.EventHandler(this.cbOutLine_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(49, 277);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 29);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "종료취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbBNDPcs
            // 
            this.tbBNDPcs.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbBNDPcs.Location = new System.Drawing.Point(123, 245);
            this.tbBNDPcs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbBNDPcs.Name = "tbBNDPcs";
            this.tbBNDPcs.ReadOnly = true;
            this.tbBNDPcs.Size = new System.Drawing.Size(115, 24);
            this.tbBNDPcs.TabIndex = 9;
            this.tbBNDPcs.Tag = null;
            this.tbBNDPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBNDPcs.TextDetached = true;
            // 
            // tbSTRPcs
            // 
            this.tbSTRPcs.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbSTRPcs.Location = new System.Drawing.Point(123, 215);
            this.tbSTRPcs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSTRPcs.Name = "tbSTRPcs";
            this.tbSTRPcs.ReadOnly = true;
            this.tbSTRPcs.Size = new System.Drawing.Size(115, 24);
            this.tbSTRPcs.TabIndex = 8;
            this.tbSTRPcs.Tag = null;
            this.tbSTRPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbSTRPcs.TextDetached = true;
            // 
            // tbMillPcs
            // 
            this.tbMillPcs.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbMillPcs.Location = new System.Drawing.Point(123, 185);
            this.tbMillPcs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbMillPcs.Name = "tbMillPcs";
            this.tbMillPcs.ReadOnly = true;
            this.tbMillPcs.Size = new System.Drawing.Size(115, 24);
            this.tbMillPcs.TabIndex = 7;
            this.tbMillPcs.Tag = null;
            this.tbMillPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbMillPcs.TextDetached = true;
            // 
            // tbLength
            // 
            this.tbLength.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbLength.Location = new System.Drawing.Point(123, 155);
            this.tbLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbLength.Name = "tbLength";
            this.tbLength.ReadOnly = true;
            this.tbLength.Size = new System.Drawing.Size(115, 24);
            this.tbLength.TabIndex = 6;
            this.tbLength.Tag = null;
            this.tbLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbLength.TextDetached = true;
            // 
            // tbSize
            // 
            this.tbSize.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbSize.Location = new System.Drawing.Point(123, 125);
            this.tbSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSize.Name = "tbSize";
            this.tbSize.ReadOnly = true;
            this.tbSize.Size = new System.Drawing.Size(115, 24);
            this.tbSize.TabIndex = 5;
            this.tbSize.Tag = null;
            this.tbSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSize.TextDetached = true;
            // 
            // tbSteel
            // 
            this.tbSteel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbSteel.Location = new System.Drawing.Point(123, 95);
            this.tbSteel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSteel.Name = "tbSteel";
            this.tbSteel.ReadOnly = true;
            this.tbSteel.Size = new System.Drawing.Size(115, 24);
            this.tbSteel.TabIndex = 4;
            this.tbSteel.Tag = null;
            this.tbSteel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSteel.TextDetached = true;
            // 
            // c1Label10
            // 
            this.c1Label10.AutoSize = true;
            this.c1Label10.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label10.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label10.Location = new System.Drawing.Point(12, 280);
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
            this.c1Label9.Location = new System.Drawing.Point(12, 248);
            this.c1Label9.Name = "c1Label9";
            this.c1Label9.Size = new System.Drawing.Size(89, 20);
            this.c1Label9.TabIndex = 58;
            this.c1Label9.Tag = null;
            this.c1Label9.Text = "바인딩본수";
            this.c1Label9.TextDetached = true;
            // 
            // c1Label8
            // 
            this.c1Label8.AutoSize = true;
            this.c1Label8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label8.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label8.Location = new System.Drawing.Point(12, 218);
            this.c1Label8.Name = "c1Label8";
            this.c1Label8.Size = new System.Drawing.Size(74, 20);
            this.c1Label8.TabIndex = 57;
            this.c1Label8.Tag = null;
            this.c1Label8.Text = "교정본수";
            this.c1Label8.TextDetached = true;
            // 
            // c1Label7
            // 
            this.c1Label7.AutoSize = true;
            this.c1Label7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label7.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label7.Location = new System.Drawing.Point(12, 188);
            this.c1Label7.Name = "c1Label7";
            this.c1Label7.Size = new System.Drawing.Size(74, 20);
            this.c1Label7.TabIndex = 56;
            this.c1Label7.Tag = null;
            this.c1Label7.Text = "압연본수";
            this.c1Label7.TextDetached = true;
            // 
            // c1Label6
            // 
            this.c1Label6.AutoSize = true;
            this.c1Label6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label6.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label6.Location = new System.Drawing.Point(11, 158);
            this.c1Label6.Name = "c1Label6";
            this.c1Label6.Size = new System.Drawing.Size(69, 20);
            this.c1Label6.TabIndex = 55;
            this.c1Label6.Tag = null;
            this.c1Label6.Text = "길이(m)";
            this.c1Label6.TextDetached = true;
            // 
            // c1Label5
            // 
            this.c1Label5.AutoSize = true;
            this.c1Label5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label5.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label5.Location = new System.Drawing.Point(13, 128);
            this.c1Label5.Name = "c1Label5";
            this.c1Label5.Size = new System.Drawing.Size(43, 20);
            this.c1Label5.TabIndex = 54;
            this.c1Label5.Tag = null;
            this.c1Label5.Text = "규격";
            this.c1Label5.TextDetached = true;
            // 
            // c1Label4
            // 
            this.c1Label4.AutoSize = true;
            this.c1Label4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label4.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label4.Location = new System.Drawing.Point(13, 98);
            this.c1Label4.Name = "c1Label4";
            this.c1Label4.Size = new System.Drawing.Size(57, 19);
            this.c1Label4.TabIndex = 53;
            this.c1Label4.Tag = null;
            this.c1Label4.Text = "강종명";
            this.c1Label4.TextDetached = true;
            // 
            // c1Label3
            // 
            this.c1Label3.AutoSize = true;
            this.c1Label3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label3.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label3.Location = new System.Drawing.Point(15, 68);
            this.c1Label3.Name = "c1Label3";
            this.c1Label3.Size = new System.Drawing.Size(52, 17);
            this.c1Label3.TabIndex = 52;
            this.c1Label3.Tag = null;
            this.c1Label3.Text = "HEAT";
            this.c1Label3.TextDetached = true;
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
            // cbPOC
            // 
            this.cbPOC.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbPOC.FormattingEnabled = true;
            this.cbPOC.Location = new System.Drawing.Point(124, 35);
            this.cbPOC.Name = "cbPOC";
            this.cbPOC.Size = new System.Drawing.Size(114, 23);
            this.cbPOC.TabIndex = 0;
            this.cbPOC.SelectedIndexChanged += new System.EventHandler(this.cbPOC_SelectedIndexChanged);
            // 
            // tbl_3
            // 
            this.tbl_3.ColumnCount = 2;
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbl_3.Controls.Add(this.btnClose, 0, 0);
            this.tbl_3.Controls.Add(this.btnSave, 0, 0);
            this.tbl_3.Location = new System.Drawing.Point(15, 277);
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
            this.btnSave.Text = "선택";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // POCFinReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(255, 316);
            this.Controls.Add(this.tbl_3);
            this.Controls.Add(this.cbFinSelect);
            this.Controls.Add(this.tbHEAT);
            this.Controls.Add(this.cbPOC);
            this.Controls.Add(this.cbOutLine);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbBNDPcs);
            this.Controls.Add(this.tbSTRPcs);
            this.Controls.Add(this.tbMillPcs);
            this.Controls.Add(this.tbLength);
            this.Controls.Add(this.tbSize);
            this.Controls.Add(this.tbSteel);
            this.Controls.Add(this.c1Label10);
            this.Controls.Add(this.c1Label9);
            this.Controls.Add(this.c1Label8);
            this.Controls.Add(this.c1Label7);
            this.Controls.Add(this.c1Label6);
            this.Controls.Add(this.c1Label5);
            this.Controls.Add(this.c1Label4);
            this.Controls.Add(this.c1Label3);
            this.Controls.Add(this.c1Label2);
            this.Controls.Add(this.c1Label1);
            this.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "POCFinReg";
            this.Text = "POC종료등록";
            this.Load += new System.EventHandler(this.POCFinReg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbHEAT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBNDPcs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSTRPcs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMillPcs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSteel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            this.tbl_3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFinSelect;
        private C1.Win.C1Input.C1TextBox tbHEAT;
        private System.Windows.Forms.ComboBox cbOutLine;
        private C1.Win.C1Input.C1Button btnCancel;
        private C1.Win.C1Input.C1TextBox tbBNDPcs;
        private C1.Win.C1Input.C1TextBox tbSTRPcs;
        private C1.Win.C1Input.C1TextBox tbMillPcs;
        private C1.Win.C1Input.C1TextBox tbLength;
        private C1.Win.C1Input.C1TextBox tbSize;
        private C1.Win.C1Input.C1TextBox tbSteel;
        private C1.Win.C1Input.C1Label c1Label10;
        private C1.Win.C1Input.C1Label c1Label9;
        private C1.Win.C1Input.C1Label c1Label8;
        private C1.Win.C1Input.C1Label c1Label7;
        private C1.Win.C1Input.C1Label c1Label6;
        private C1.Win.C1Input.C1Label c1Label5;
        private C1.Win.C1Input.C1Label c1Label4;
        private C1.Win.C1Input.C1Label c1Label3;
        private C1.Win.C1Input.C1Label c1Label2;
        private C1.Win.C1Input.C1Label c1Label1;
        private System.Windows.Forms.ComboBox cbPOC;
        private System.Windows.Forms.TableLayoutPanel tbl_3;
        private C1.Win.C1Input.C1Button btnClose;
        private C1.Win.C1Input.C1Button btnSave;
    }
}