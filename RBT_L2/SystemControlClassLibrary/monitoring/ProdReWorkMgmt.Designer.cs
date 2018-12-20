namespace SystemControlClassLibrary.monitoring
{
    partial class ProdReWorkMgmt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProdReWorkMgmt));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRegCancel = new C1.Win.C1Input.C1Button();
            this.btnReg = new C1.Win.C1Input.C1Button();
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdSub = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.uC_GuBun1 = new SystemControlClassLibrary.UC.sub_UC.UC_GuBun();
            this.uC_POC_sc1 = new SystemControlClassLibrary.UC.sub_UC.UC_POC_sc();
            this.uC_Work_Date_Fr_To_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_Work_Date_Fr_To_s();
            this.uC_HEAT_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_HEAT_s();
            this.uC_STEEL_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_STEEL_s();
            this.uC_Item_size_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_Item_size_s();
            this.uC_Line_gp_s1 = new SystemControlClassLibrary.UC.sub_UC.UC_Line_gp_s();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.Controls.Add(this.uC_GuBun1);
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
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.Controls.Add(this.btnRegCancel, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnReg, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 120);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1064, 35);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // btnRegCancel
            // 
            this.btnRegCancel.AutoSize = true;
            this.btnRegCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnRegCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRegCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRegCancel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnRegCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnRegCancel.Image")));
            this.btnRegCancel.Location = new System.Drawing.Point(917, 3);
            this.btnRegCancel.Name = "btnRegCancel";
            this.btnRegCancel.Size = new System.Drawing.Size(144, 29);
            this.btnRegCancel.TabIndex = 32;
            this.btnRegCancel.Text = "재작업등록취소";
            this.btnRegCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRegCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRegCancel.UseVisualStyleBackColor = true;
            this.btnRegCancel.Click += new System.EventHandler(this.btnRegCancel_Click);
            // 
            // btnReg
            // 
            this.btnReg.AutoSize = true;
            this.btnReg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnReg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReg.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnReg.Image = ((System.Drawing.Image)(resources.GetObject("btnReg.Image")));
            this.btnReg.Location = new System.Drawing.Point(797, 3);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(114, 29);
            this.btnReg.TabIndex = 31;
            this.btnReg.Text = "재작업등록";
            this.btnReg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
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
            this.tableLayoutPanel1.TabIndex = 16;
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
            this.grdSub.Click += new System.EventHandler(this.grdSub_Click);
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
            // uC_GuBun1
            // 
            this.uC_GuBun1.BackColor = System.Drawing.Color.Transparent;
            this.uC_GuBun1.cb_Enable = true;
            this.uC_GuBun1.GUBUN = "A";
            this.uC_GuBun1.Location = new System.Drawing.Point(9, 42);
            this.uC_GuBun1.Name = "uC_GuBun1";
            this.uC_GuBun1.Size = new System.Drawing.Size(224, 27);
            this.uC_GuBun1.TabIndex = 7;
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
            // ProdReWorkMgmt
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1064, 862);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProdReWorkMgmt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "제품재작업관리";
            this.Load += new System.EventHandler(this.ProdReWorkMgmt_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolTip toolTipPageID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private C1.Win.C1Input.C1Button btnClose;
        private UC.sub_UC.UC_HEAT_s uC_HEAT_s1;
        private UC.sub_UC.UC_STEEL_s uC_STEEL_s1;
        private UC.sub_UC.UC_Item_size_s uC_Item_size_s1;
        private UC.sub_UC.UC_Line_gp_s uC_Line_gp_s1;
        private UC.sub_UC.UC_Work_Date_Fr_To_s uC_Work_Date_Fr_To_s1;
        private UC.sub_UC.UC_POC_sc uC_POC_sc1;
        private UC.sub_UC.UC_GuBun uC_GuBun1;
        private C1.Win.C1Input.C1Button btnDisplay;
        private C1.Win.C1Input.C1Button btnRegCancel;
        private C1.Win.C1Input.C1Button btnReg;
        private C1.Win.C1FlexGrid.C1FlexGrid grdSub;
    }
}