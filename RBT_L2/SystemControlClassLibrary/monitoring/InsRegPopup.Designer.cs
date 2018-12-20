namespace SystemControlClassLibrary.monitoring
{
    partial class InsRegPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsRegPopup));
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.c1Label1 = new C1.Win.C1Input.C1Label();
            this.c1Label2 = new C1.Win.C1Input.C1Label();
            this.cbZone = new System.Windows.Forms.ComboBox();
            this.lbToZone = new C1.Win.C1Input.C1Label();
            this.cbToZone = new System.Windows.Forms.ComboBox();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.txtPoc = new C1.Win.C1Input.C1TextBox();
            this.lblPoc = new System.Windows.Forms.Label();
            this.cboMLFT_YN = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtHeat = new C1.Win.C1Input.C1TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbToZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeat)).BeginInit();
            this.SuspendLayout();
            // 
            // cbLine
            // 
            this.cbLine.Enabled = false;
            this.cbLine.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Location = new System.Drawing.Point(82, 15);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(78, 23);
            this.cbLine.TabIndex = 26;
            this.cbLine.SelectedIndexChanged += new System.EventHandler(this.cbLine_SelectedIndexChanged);
            // 
            // c1Label1
            // 
            this.c1Label1.AutoSize = true;
            this.c1Label1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label1.Location = new System.Drawing.Point(34, 17);
            this.c1Label1.Name = "c1Label1";
            this.c1Label1.Size = new System.Drawing.Size(42, 19);
            this.c1Label1.TabIndex = 25;
            this.c1Label1.Tag = null;
            this.c1Label1.Text = "라인";
            this.c1Label1.TextDetached = true;
            // 
            // c1Label2
            // 
            this.c1Label2.AutoSize = true;
            this.c1Label2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1Label2.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.c1Label2.Location = new System.Drawing.Point(198, 18);
            this.c1Label2.Name = "c1Label2";
            this.c1Label2.Size = new System.Drawing.Size(53, 17);
            this.c1Label2.TabIndex = 25;
            this.c1Label2.Tag = null;
            this.c1Label2.Text = "ZONE";
            this.c1Label2.TextDetached = true;
            // 
            // cbZone
            // 
            this.cbZone.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbZone.FormattingEnabled = true;
            this.cbZone.Location = new System.Drawing.Point(252, 15);
            this.cbZone.Name = "cbZone";
            this.cbZone.Size = new System.Drawing.Size(78, 23);
            this.cbZone.TabIndex = 26;
            this.cbZone.SelectedIndexChanged += new System.EventHandler(this.cbZone_SelectedIndexChanged);
            // 
            // lbToZone
            // 
            this.lbToZone.AutoSize = true;
            this.lbToZone.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbToZone.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbToZone.Location = new System.Drawing.Point(774, 50);
            this.lbToZone.Name = "lbToZone";
            this.lbToZone.Size = new System.Drawing.Size(103, 19);
            this.lbToZone.TabIndex = 25;
            this.lbToZone.Tag = null;
            this.lbToZone.Text = "이동할 ZONE";
            this.lbToZone.TextDetached = true;
            // 
            // cbToZone
            // 
            this.cbToZone.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbToZone.FormattingEnabled = true;
            this.cbToZone.Location = new System.Drawing.Point(895, 47);
            this.cbToZone.Name = "cbToZone";
            this.cbToZone.Size = new System.Drawing.Size(78, 23);
            this.cbToZone.TabIndex = 26;
            this.cbToZone.SelectedIndexChanged += new System.EventHandler(this.cbToZone_SelectedIndexChanged);
            this.cbToZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbToZone_KeyDown);
            // 
            // grdMain
            // 
            this.grdMain.ColumnInfo = resources.GetString("grdMain.ColumnInfo");
            this.grdMain.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.grdMain.Location = new System.Drawing.Point(13, 76);
            this.grdMain.Name = "grdMain";
            this.grdMain.Rows.Count = 1;
            this.grdMain.Rows.DefaultSize = 23;
            this.grdMain.Size = new System.Drawing.Size(1079, 467);
            this.grdMain.StyleInfo = resources.GetString("grdMain.StyleInfo");
            this.grdMain.TabIndex = 28;
            this.grdMain.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdMain_BeforeEdit);
            this.grdMain.CellChecked += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdMain_CellChecked);
            this.grdMain.Click += new System.EventHandler(this.c1FlexGrid1_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(1012, 44);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 29);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "저장";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(1012, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 29);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDisplay
            // 
            this.btnDisplay.AutoSize = true;
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDisplay.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplay.Image")));
            this.btnDisplay.Location = new System.Drawing.Point(932, 7);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(74, 29);
            this.btnDisplay.TabIndex = 30;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtPoc
            // 
            this.txtPoc.AutoSize = false;
            this.txtPoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPoc.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.txtPoc.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPoc.Location = new System.Drawing.Point(418, 15);
            this.txtPoc.MaxLength = 20;
            this.txtPoc.Name = "txtPoc";
            this.txtPoc.Size = new System.Drawing.Size(79, 24);
            this.txtPoc.TabIndex = 84;
            this.txtPoc.Tag = null;
            this.txtPoc.TextDetached = true;
            // 
            // lblPoc
            // 
            this.lblPoc.AutoSize = true;
            this.lblPoc.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblPoc.Location = new System.Drawing.Point(368, 19);
            this.lblPoc.Name = "lblPoc";
            this.lblPoc.Size = new System.Drawing.Size(44, 16);
            this.lblPoc.TabIndex = 85;
            this.lblPoc.Text = "POC";
            // 
            // cboMLFT_YN
            // 
            this.cboMLFT_YN.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cboMLFT_YN.FormattingEnabled = true;
            this.cboMLFT_YN.Location = new System.Drawing.Point(779, 16);
            this.cboMLFT_YN.Name = "cboMLFT_YN";
            this.cboMLFT_YN.Size = new System.Drawing.Size(94, 23);
            this.cboMLFT_YN.TabIndex = 87;
            this.cboMLFT_YN.SelectedIndexChanged += new System.EventHandler(this.cboMLFT_YN_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(691, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 15);
            this.label16.TabIndex = 86;
            this.label16.Text = "MLFT합부";
            // 
            // txtHeat
            // 
            this.txtHeat.AutoSize = false;
            this.txtHeat.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHeat.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.txtHeat.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtHeat.Location = new System.Drawing.Point(574, 16);
            this.txtHeat.MaxLength = 20;
            this.txtHeat.Name = "txtHeat";
            this.txtHeat.Size = new System.Drawing.Size(79, 24);
            this.txtHeat.TabIndex = 88;
            this.txtHeat.Tag = null;
            this.txtHeat.TextDetached = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(524, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 16);
            this.label1.TabIndex = 89;
            this.label1.Text = "HEAT";
            // 
            // InsRegPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1104, 555);
            this.Controls.Add(this.txtHeat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboMLFT_YN);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtPoc);
            this.Controls.Add(this.lblPoc);
            this.Controls.Add(this.btnDisplay);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdMain);
            this.Controls.Add(this.cbToZone);
            this.Controls.Add(this.cbZone);
            this.Controls.Add(this.lbToZone);
            this.Controls.Add(this.c1Label2);
            this.Controls.Add(this.cbLine);
            this.Controls.Add(this.c1Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InsRegPopup";
            this.Text = "ZONE이동관리";
            this.Load += new System.EventHandler(this.InsRegPopup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbToZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbLine;
        private C1.Win.C1Input.C1Label c1Label1;
        private C1.Win.C1Input.C1Label c1Label2;
        private System.Windows.Forms.ComboBox cbZone;
        private C1.Win.C1Input.C1Label lbToZone;
        private System.Windows.Forms.ComboBox cbToZone;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1Button btnClose;
        private C1.Win.C1Input.C1Button btnDisplay;
        private C1.Win.C1Input.C1TextBox txtPoc;
        private System.Windows.Forms.Label lblPoc;
        private System.Windows.Forms.ComboBox cboMLFT_YN;
        private System.Windows.Forms.Label label16;
        private C1.Win.C1Input.C1TextBox txtHeat;
        private System.Windows.Forms.Label label1;
    }
}