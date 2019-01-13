namespace SystemControlClassLibrary.monitoring
{
    partial class WorkRsltCancel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkRsltCancel));
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.grdSub = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uC_RsltCancel1 = new SystemControlClassLibrary.UC.sub_UC.UC_RsltCancel();
            this.uC_POC_sc1 = new SystemControlClassLibrary.UC.sub_UC.UC_POC_sc();
            this.uC_Work_Date_Fr_To_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_Work_Date_Fr_To_s();
            this.uC_HEAT_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_HEAT_s();
            this.uC_STEEL_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_STEEL_s();
            this.uC_Item_size_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_Item_size_s();
            this.uC_Line_gp_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_Line_gp_s();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(987, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 29);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdSub
            // 
            this.grdSub.ColumnInfo = resources.GetString("grdSub.ColumnInfo");
            this.grdSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSub.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.grdSub.Location = new System.Drawing.Point(3, 511);
            this.grdSub.Name = "grdSub";
            this.grdSub.Rows.Count = 1;
            this.grdSub.Rows.DefaultSize = 23;
            this.grdSub.Size = new System.Drawing.Size(1058, 348);
            this.grdSub.StyleInfo = resources.GetString("grdSub.StyleInfo");
            this.grdSub.TabIndex = 10;
            // 
            // grdMain
            // 
            this.grdMain.ColumnInfo = resources.GetString("grdMain.ColumnInfo");
            this.grdMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.grdMain.Location = new System.Drawing.Point(3, 158);
            this.grdMain.Name = "grdMain";
            this.grdMain.Rows.Count = 1;
            this.grdMain.Rows.DefaultSize = 23;
            this.grdMain.Size = new System.Drawing.Size(1058, 347);
            this.grdMain.StyleInfo = resources.GetString("grdMain.StyleInfo");
            this.grdMain.TabIndex = 9;
            this.grdMain.Click += new System.EventHandler(this.grdMain_Click);
            this.grdMain.Paint += new System.Windows.Forms.PaintEventHandler(this.grdMain_Paint);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 311F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.Controls.Add(this.btnDisplay, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 7, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1064, 35);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnDisplay
            // 
            this.btnDisplay.AutoSize = true;
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisplay.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplay.Image")));
            this.btnDisplay.Location = new System.Drawing.Point(907, 3);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(74, 29);
            this.btnDisplay.TabIndex = 29;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdSub, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.grdMain, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1064, 862);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.Controls.Add(this.uC_RsltCancel1);
            this.panel1.Controls.Add(this.uC_POC_sc1);
            this.panel1.Controls.Add(this.uC_Work_Date_Fr_To_s1);
            this.panel1.Controls.Add(this.uC_HEAT_s1);
            this.panel1.Controls.Add(this.uC_STEEL_s1);
            this.panel1.Controls.Add(this.uC_Item_size_s1);
            this.panel1.Controls.Add(this.uC_Line_gp_s1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(3, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1058, 79);
            this.panel1.TabIndex = 5;
            // 
            // uC_RsltCancel1
            // 
            this.uC_RsltCancel1.BackColor = System.Drawing.Color.Transparent;
            this.uC_RsltCancel1.cb_Enable = true;
            this.uC_RsltCancel1.GUBUN = "A";
            this.uC_RsltCancel1.Location = new System.Drawing.Point(9, 42);
            this.uC_RsltCancel1.Name = "uC_RsltCancel1";
            this.uC_RsltCancel1.Size = new System.Drawing.Size(226, 27);
            this.uC_RsltCancel1.TabIndex = 7;
            // 
            // uC_POC_sc1
            // 
            this.uC_POC_sc1.BackColor = System.Drawing.Color.Transparent;
            this.uC_POC_sc1.Location = new System.Drawing.Point(307, 42);
            this.uC_POC_sc1.Name = "uC_POC_sc1";
            this.uC_POC_sc1.POC = "";
            this.uC_POC_sc1.ReadOnly = false;
            this.uC_POC_sc1.Size = new System.Drawing.Size(207, 27);
            this.uC_POC_sc1.TabIndex = 6;
            // 
            // uC_Work_Date_Fr_To_s1
            // 
            this.uC_Work_Date_Fr_To_s1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Work_Date_Fr_To_s1.Location = new System.Drawing.Point(307, 9);
            this.uC_Work_Date_Fr_To_s1.Name = "uC_Work_Date_Fr_To_s1";
            this.uC_Work_Date_Fr_To_s1.Size = new System.Drawing.Size(407, 27);
            this.uC_Work_Date_Fr_To_s1.TabIndex = 5;
            this.uC_Work_Date_Fr_To_s1.Work_From_Date = new System.DateTime(2016, 8, 23, 10, 7, 18, 102);
            this.uC_Work_Date_Fr_To_s1.Work_To_Date = new System.DateTime(2016, 8, 23, 10, 7, 18, 100);
            // 
            // uC_HEAT_s1
            // 
            this.uC_HEAT_s1.BackColor = System.Drawing.Color.Transparent;
            this.uC_HEAT_s1.HEAT = "";
            this.uC_HEAT_s1.Location = new System.Drawing.Point(520, 42);
            this.uC_HEAT_s1.Name = "uC_HEAT_s1";
            this.uC_HEAT_s1.ReadOnly = false;
            this.uC_HEAT_s1.Size = new System.Drawing.Size(199, 27);
            this.uC_HEAT_s1.TabIndex = 4;
            // 
            // uC_STEEL_s1
            // 
            this.uC_STEEL_s1.BackColor = System.Drawing.Color.Transparent;
            this.uC_STEEL_s1.Location = new System.Drawing.Point(809, 42);
            this.uC_STEEL_s1.Name = "uC_STEEL_s1";
            this.uC_STEEL_s1.Size = new System.Drawing.Size(240, 27);
            this.uC_STEEL_s1.Steel = "";
            this.uC_STEEL_s1.Steel_NM = "";
            this.uC_STEEL_s1.TabIndex = 2;
            // 
            // uC_Item_size_s1
            // 
            this.uC_Item_size_s1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Item_size_s1.ITEM_SIZE = "";
            this.uC_Item_size_s1.Location = new System.Drawing.Point(809, 9);
            this.uC_Item_size_s1.Name = "uC_Item_size_s1";
            this.uC_Item_size_s1.ReadOnly = false;
            this.uC_Item_size_s1.Size = new System.Drawing.Size(202, 27);
            this.uC_Item_size_s1.TabIndex = 1;
            // 
            // uC_Line_gp_s1
            // 
            this.uC_Line_gp_s1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Line_gp_s1.cb_Enable = false;
            this.uC_Line_gp_s1.Line_GP = "#1";
            this.uC_Line_gp_s1.Location = new System.Drawing.Point(9, 9);
            this.uC_Line_gp_s1.Name = "uC_Line_gp_s1";
            this.uC_Line_gp_s1.Size = new System.Drawing.Size(203, 27);
            this.uC_Line_gp_s1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 120);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1064, 35);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(957, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 29);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "작업취소";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // WorkRsltCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 862);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WorkRsltCancel";
            this.Text = "작업취소(실적삭제)";
            this.Load += new System.EventHandler(this.WorkRsltCancel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1Input.C1Button btnClose;
        private C1.Win.C1FlexGrid.C1FlexGrid grdSub;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private C1.Win.C1Input.C1Button btnDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private UC.sub_UC.UC_POC_sc uC_POC_sc1;
        private UC.sub_UC.UC_Work_Date_Fr_To_s uC_Work_Date_Fr_To_s1;
        private UC.sub_UC.UC_HEAT_s uC_HEAT_s1;
        private UC.sub_UC.UC_STEEL_s uC_STEEL_s1;
        private UC.sub_UC.UC_Item_size_s uC_Item_size_s1;
        private UC.sub_UC.UC_Line_gp_s uC_Line_gp_s1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private C1.Win.C1Input.C1Button btnCancel;
        private System.Windows.Forms.ToolTip toolTipPageID;
        private UC.sub_UC.UC_RsltCancel uC_RsltCancel1;
    }
}