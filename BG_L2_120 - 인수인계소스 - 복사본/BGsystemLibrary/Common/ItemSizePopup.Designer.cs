namespace BGsystemLibrary.Common
{
    partial class ItemSizePopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSizePopup));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.lbl_searchItem = new System.Windows.Forms.Label();
            this.txt_CD_ID = new C1.Win.C1Input.C1TextBox();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CD_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.lbl_searchItem);
            this.panel1.Controls.Add(this.txt_CD_ID);
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(356, 48);
            this.panel1.TabIndex = 1;
            // 
            // btnDisplay
            // 
            this.btnDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplay.AutoSize = true;
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDisplay.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplay.Image")));
            this.btnDisplay.Location = new System.Drawing.Point(267, 5);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(80, 35);
            this.btnDisplay.TabIndex = 123;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // lbl_searchItem
            // 
            this.lbl_searchItem.AutoSize = true;
            this.lbl_searchItem.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_searchItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lbl_searchItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_searchItem.Location = new System.Drawing.Point(17, 18);
            this.lbl_searchItem.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_searchItem.Name = "lbl_searchItem";
            this.lbl_searchItem.Size = new System.Drawing.Size(41, 15);
            this.lbl_searchItem.TabIndex = 106;
            this.lbl_searchItem.Text = "규격";
            this.lbl_searchItem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_CD_ID
            // 
            this.txt_CD_ID.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_CD_ID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txt_CD_ID.Location = new System.Drawing.Point(64, 14);
            this.txt_CD_ID.Name = "txt_CD_ID";
            this.txt_CD_ID.Size = new System.Drawing.Size(139, 24);
            this.txt_CD_ID.TabIndex = 105;
            this.txt_CD_ID.Tag = null;
            this.txt_CD_ID.TextChanged += new System.EventHandler(this.txt_Nm_TextChanged);
            // 
            // grdMain
            // 
            this.grdMain.AllowEditing = false;
            this.grdMain.ColumnInfo = "3,1,0,0,0,100,Columns:0{Style:\"Font:돋움체, 9pt;\";}\t1{Style:\"Font:돋움체, 9pt;\";}\t2{Sty" +
    "le:\"Font:돋움체, 9pt;\";}\t";
            this.grdMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.grdMain.ExtendLastCol = true;
            this.grdMain.Location = new System.Drawing.Point(3, 54);
            this.grdMain.Name = "grdMain";
            this.grdMain.Rows.Count = 1;
            this.grdMain.Rows.DefaultSize = 20;
            this.grdMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain.Size = new System.Drawing.Size(356, 514);
            this.grdMain.TabIndex = 2;
            this.grdMain.Click += new System.EventHandler(this.grdMain_Click);
            this.grdMain.DoubleClick += new System.EventHandler(this.grdMain_DoubleClick);
            // 
            // ItemSizePopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 567);
            this.Controls.Add(this.grdMain);
            this.Controls.Add(this.panel1);
            this.Name = "ItemSizePopup";
            this.Text = "규격 조회";
            this.Load += new System.EventHandler(this.ItemSizePopup_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_CD_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_searchItem;
        private C1.Win.C1Input.C1TextBox txt_CD_ID;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain;
        private C1.Win.C1Input.C1Button btnDisplay;
    }
}