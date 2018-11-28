namespace BGsystemLibrary.SystemMgmt
{
    partial class UseLogMgmt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UseLogMgmt));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExcel = new C1.Win.C1Input.C1Button();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.end_dt = new System.Windows.Forms.DateTimePicker();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.TabOpt = new C1.Win.C1Command.C1DockingTab();
            this.TabOpt1 = new C1.Win.C1Command.C1DockingTabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grdMain1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdSub1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.TabOpt2 = new C1.Win.C1Command.C1DockingTabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.grdMain2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdSub2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabOpt)).BeginInit();
            this.TabOpt.SuspendLayout();
            this.TabOpt1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub1)).BeginInit();
            this.TabOpt2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub2)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TabOpt, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 756F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1257, 863);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.lbl_Date);
            this.panel1.Controls.Add(this.end_dt);
            this.panel1.Controls.Add(this.start_dt);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("현대하모니 M", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1251, 55);
            this.panel1.TabIndex = 5;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExcel.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
            this.btnExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.Image")));
            this.btnExcel.Location = new System.Drawing.Point(1162, 10);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 35);
            this.btnExcel.TabIndex = 123;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplay.AutoSize = true;
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDisplay.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplay.Image")));
            this.btnDisplay.Location = new System.Drawing.Point(1076, 10);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(80, 35);
            this.btnDisplay.TabIndex = 122;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.Button_Click);
            // 
            // lbl_Date
            // 
            this.lbl_Date.AutoSize = true;
            this.lbl_Date.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Date.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lbl_Date.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_Date.Location = new System.Drawing.Point(32, 22);
            this.lbl_Date.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(75, 15);
            this.lbl_Date.TabIndex = 108;
            this.lbl_Date.Text = "사용기간";
            this.lbl_Date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // end_dt
            // 
            this.end_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.end_dt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end_dt.Location = new System.Drawing.Point(287, 17);
            this.end_dt.Name = "end_dt";
            this.end_dt.Size = new System.Drawing.Size(135, 24);
            this.end_dt.TabIndex = 3;
            this.end_dt.ValueChanged += new System.EventHandler(this.end_dt_ValueChanged);
            // 
            // start_dt
            // 
            this.start_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.start_dt.Location = new System.Drawing.Point(121, 17);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(135, 24);
            this.start_dt.TabIndex = 2;
            this.start_dt.ValueChanged += new System.EventHandler(this.start_dt_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(262, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "~";
            // 
            // TabOpt
            // 
            this.TabOpt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TabOpt.Controls.Add(this.TabOpt1);
            this.TabOpt.Controls.Add(this.TabOpt2);
            this.TabOpt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabOpt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabOpt.Location = new System.Drawing.Point(3, 64);
            this.TabOpt.Name = "TabOpt";
            this.TabOpt.SelectedIndex = 1;
            this.TabOpt.SelectedTabBold = true;
            this.TabOpt.Size = new System.Drawing.Size(1251, 796);
            this.TabOpt.SplitterWidth = 1;
            this.TabOpt.TabIndex = 9;
            this.TabOpt.TabsSpacing = 0;
            this.TabOpt.TabStyle = C1.Win.C1Command.TabStyleEnum.Classic;
            this.TabOpt.VisualStyle = C1.Win.C1Command.VisualStyle.Custom;
            this.TabOpt.VisualStyleBase = C1.Win.C1Command.VisualStyle.Classic;
            this.TabOpt.SelectedIndexChanged += new System.EventHandler(this.TabOpt_SelectedIndexChanged);
            // 
            // TabOpt1
            // 
            this.TabOpt1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TabOpt1.Controls.Add(this.splitContainer1);
            this.TabOpt1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabOpt1.Location = new System.Drawing.Point(1, 29);
            this.TabOpt1.Name = "TabOpt1";
            this.TabOpt1.Size = new System.Drawing.Size(1249, 766);
            this.TabOpt1.TabBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TabOpt1.TabBackColorSelected = System.Drawing.Color.White;
            this.TabOpt1.TabIndex = 0;
            this.TabOpt1.Text = "사용자별이력";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grdMain1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdSub1);
            this.splitContainer1.Size = new System.Drawing.Size(1249, 766);
            this.splitContainer1.SplitterDistance = 602;
            this.splitContainer1.TabIndex = 0;
            // 
            // grdMain1
            // 
            this.grdMain1.AllowEditing = false;
            this.grdMain1.ColumnInfo = "4,1,0,0,0,125,Columns:";
            this.grdMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain1.ExtendLastCol = true;
            this.grdMain1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdMain1.Location = new System.Drawing.Point(0, 0);
            this.grdMain1.Margin = new System.Windows.Forms.Padding(0);
            this.grdMain1.Name = "grdMain1";
            this.grdMain1.Rows.Count = 1;
            this.grdMain1.Rows.DefaultSize = 28;
            this.grdMain1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain1.Size = new System.Drawing.Size(602, 766);
            this.grdMain1.TabIndex = 1;
            // 
            // grdSub1
            // 
            this.grdSub1.AllowEditing = false;
            this.grdSub1.ColumnInfo = "3,1,0,0,0,125,Columns:";
            this.grdSub1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSub1.ExtendLastCol = true;
            this.grdSub1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdSub1.Location = new System.Drawing.Point(0, 0);
            this.grdSub1.Margin = new System.Windows.Forms.Padding(0);
            this.grdSub1.Name = "grdSub1";
            this.grdSub1.Rows.Count = 1;
            this.grdSub1.Rows.DefaultSize = 28;
            this.grdSub1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdSub1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdSub1.Size = new System.Drawing.Size(643, 766);
            this.grdSub1.TabIndex = 0;
            // 
            // TabOpt2
            // 
            this.TabOpt2.BackColor = System.Drawing.Color.Black;
            this.TabOpt2.Controls.Add(this.splitContainer2);
            this.TabOpt2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabOpt2.Location = new System.Drawing.Point(1, 29);
            this.TabOpt2.Name = "TabOpt2";
            this.TabOpt2.Size = new System.Drawing.Size(1249, 766);
            this.TabOpt2.TabBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TabOpt2.TabBackColorSelected = System.Drawing.Color.White;
            this.TabOpt2.TabIndex = 1;
            this.TabOpt2.Text = "화면별이력";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grdMain2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grdSub2);
            this.splitContainer2.Size = new System.Drawing.Size(1249, 766);
            this.splitContainer2.SplitterDistance = 602;
            this.splitContainer2.TabIndex = 0;
            // 
            // grdMain2
            // 
            this.grdMain2.AllowEditing = false;
            this.grdMain2.ColumnInfo = "4,1,0,0,0,110,Columns:0{Style:\"Font:돋움체, 9pt;\";}\t1{Style:\"Font:돋움체, 9pt;\";}\t2{Sty" +
    "le:\"Font:돋움체, 9pt;\";}\t3{Style:\"Font:돋움체, 9pt;\";}\t";
            this.grdMain2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdMain2.Location = new System.Drawing.Point(0, 0);
            this.grdMain2.Margin = new System.Windows.Forms.Padding(0);
            this.grdMain2.Name = "grdMain2";
            this.grdMain2.Rows.Count = 1;
            this.grdMain2.Rows.DefaultSize = 22;
            this.grdMain2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain2.Size = new System.Drawing.Size(602, 766);
            this.grdMain2.TabIndex = 0;
            // 
            // grdSub2
            // 
            this.grdSub2.AllowEditing = false;
            this.grdSub2.ColumnInfo = "4,1,0,0,0,110,Columns:0{Style:\"Font:돋움체, 9pt;\";}\t1{Style:\"Font:돋움체, 9pt;\";}\t2{Sty" +
    "le:\"Font:돋움체, 9pt;\";}\t3{Style:\"Font:돋움체, 9pt;\";}\t";
            this.grdSub2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSub2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdSub2.Location = new System.Drawing.Point(0, 0);
            this.grdSub2.Margin = new System.Windows.Forms.Padding(0);
            this.grdSub2.Name = "grdSub2";
            this.grdSub2.Rows.Count = 1;
            this.grdSub2.Rows.DefaultSize = 22;
            this.grdSub2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdSub2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdSub2.Size = new System.Drawing.Size(643, 766);
            this.grdSub2.TabIndex = 0;
            // 
            // UseLogMgmt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1257, 863);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("현대하모니 M", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UseLogMgmt";
            this.Text = "사용이력조회";
            this.Load += new System.EventHandler(this.UseLogMgmt_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabOpt)).EndInit();
            this.TabOpt.ResumeLayout(false);
            this.TabOpt1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub1)).EndInit();
            this.TabOpt2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolTip toolTipPageID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker start_dt;
        private System.Windows.Forms.DateTimePicker end_dt;
        private C1.Win.C1Command.C1DockingTab TabOpt;
        private C1.Win.C1Command.C1DockingTabPage TabOpt1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdSub1;
        private C1.Win.C1Command.C1DockingTabPage TabOpt2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lbl_Date;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain2;
        private C1.Win.C1FlexGrid.C1FlexGrid grdSub2;
        private C1.Win.C1Input.C1Button btnExcel;
        private C1.Win.C1Input.C1Button btnDisplay;
    }
}