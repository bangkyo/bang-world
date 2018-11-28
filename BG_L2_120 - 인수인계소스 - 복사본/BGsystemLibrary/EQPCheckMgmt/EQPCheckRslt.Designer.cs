namespace BGsystemLibrary.EQPCheckMgmt
{
    partial class EQPCheckRslt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EQPCheckRslt));
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_DelayYN_Gp = new System.Windows.Forms.CheckBox();
            this.cbx_CheckItem_Gp = new System.Windows.Forms.ComboBox();
            this.lb_TestItem = new System.Windows.Forms.Label();
            this.cbx_Eq_Gp = new System.Windows.Forms.ComboBox();
            this.lb_EQ = new System.Windows.Forms.Label();
            this.btnExcel = new C1.Win.C1Input.C1Button();
            this.btnAddRes = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.end_dt = new System.Windows.Forms.DateTimePicker();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdMain, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1664, 863);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // grdMain
            // 
            this.grdMain.AutoClipboard = true;
            this.grdMain.AutoResize = true;
            this.grdMain.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.grdMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain.ExtendLastCol = true;
            this.grdMain.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdMain.Location = new System.Drawing.Point(3, 64);
            this.grdMain.Name = "grdMain";
            this.grdMain.Rows.Count = 1;
            this.grdMain.Rows.DefaultSize = 19;
            this.grdMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain.Size = new System.Drawing.Size(1658, 796);
            this.grdMain.TabIndex = 11;
            this.grdMain.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdMain_BeforeEdit);
            this.grdMain.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdMain_AfterEdit);
            this.grdMain.CellChecked += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdMain_CellChecked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cb_DelayYN_Gp);
            this.panel1.Controls.Add(this.cbx_CheckItem_Gp);
            this.panel1.Controls.Add(this.lb_TestItem);
            this.panel1.Controls.Add(this.cbx_Eq_Gp);
            this.panel1.Controls.Add(this.lb_EQ);
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.btnAddRes);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.lbl_Date);
            this.panel1.Controls.Add(this.end_dt);
            this.panel1.Controls.Add(this.start_dt);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1658, 55);
            this.panel1.TabIndex = 5;
            // 
            // cb_DelayYN_Gp
            // 
            this.cb_DelayYN_Gp.AutoSize = true;
            this.cb_DelayYN_Gp.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
            this.cb_DelayYN_Gp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.cb_DelayYN_Gp.Location = new System.Drawing.Point(1043, 20);
            this.cb_DelayYN_Gp.Name = "cb_DelayYN_Gp";
            this.cb_DelayYN_Gp.Size = new System.Drawing.Size(60, 19);
            this.cb_DelayYN_Gp.TabIndex = 130;
            this.cb_DelayYN_Gp.Text = "지연";
            this.cb_DelayYN_Gp.UseVisualStyleBackColor = true;
            // 
            // cbx_CheckItem_Gp
            // 
            this.cbx_CheckItem_Gp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_CheckItem_Gp.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbx_CheckItem_Gp.FormattingEnabled = true;
            this.cbx_CheckItem_Gp.Location = new System.Drawing.Point(789, 18);
            this.cbx_CheckItem_Gp.Name = "cbx_CheckItem_Gp";
            this.cbx_CheckItem_Gp.Size = new System.Drawing.Size(211, 23);
            this.cbx_CheckItem_Gp.TabIndex = 129;
            // 
            // lb_TestItem
            // 
            this.lb_TestItem.AutoSize = true;
            this.lb_TestItem.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_TestItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lb_TestItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_TestItem.Location = new System.Drawing.Point(715, 22);
            this.lb_TestItem.Margin = new System.Windows.Forms.Padding(0);
            this.lb_TestItem.Name = "lb_TestItem";
            this.lb_TestItem.Size = new System.Drawing.Size(75, 15);
            this.lb_TestItem.TabIndex = 128;
            this.lb_TestItem.Text = "점검항목";
            this.lb_TestItem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbx_Eq_Gp
            // 
            this.cbx_Eq_Gp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Eq_Gp.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbx_Eq_Gp.FormattingEnabled = true;
            this.cbx_Eq_Gp.Location = new System.Drawing.Point(538, 18);
            this.cbx_Eq_Gp.Name = "cbx_Eq_Gp";
            this.cbx_Eq_Gp.Size = new System.Drawing.Size(146, 23);
            this.cbx_Eq_Gp.TabIndex = 129;
            this.cbx_Eq_Gp.SelectedIndexChanged += new System.EventHandler(this.cbx_Eq_Gp_SelectedIndexChanged);
            // 
            // lb_EQ
            // 
            this.lb_EQ.AutoSize = true;
            this.lb_EQ.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_EQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lb_EQ.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_EQ.Location = new System.Drawing.Point(494, 22);
            this.lb_EQ.Margin = new System.Windows.Forms.Padding(0);
            this.lb_EQ.Name = "lb_EQ";
            this.lb_EQ.Size = new System.Drawing.Size(41, 15);
            this.lb_EQ.TabIndex = 128;
            this.lb_EQ.Text = "설비";
            this.lb_EQ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExcel.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
            this.btnExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.Image")));
            this.btnExcel.Location = new System.Drawing.Point(1461, 11);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 35);
            this.btnExcel.TabIndex = 127;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnAddRes
            // 
            this.btnAddRes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRes.AutoSize = true;
            this.btnAddRes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnAddRes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAddRes.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddRes.Image = global::BGsystemLibrary.Properties.Resources.if_application_form_add;
            this.btnAddRes.Location = new System.Drawing.Point(1547, 11);
            this.btnAddRes.Name = "btnAddRes";
            this.btnAddRes.Size = new System.Drawing.Size(101, 35);
            this.btnAddRes.TabIndex = 126;
            this.btnAddRes.Text = "신규등록";
            this.btnAddRes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddRes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddRes.UseVisualStyleBackColor = true;
            this.btnAddRes.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(1375, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 35);
            this.btnSave.TabIndex = 126;
            this.btnSave.Text = "저장";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Button_Click);
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
            this.btnDisplay.Location = new System.Drawing.Point(1289, 11);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(80, 35);
            this.btnDisplay.TabIndex = 125;
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
            this.lbl_Date.Size = new System.Drawing.Size(109, 15);
            this.lbl_Date.TabIndex = 115;
            this.lbl_Date.Text = "점검계획일자";
            this.lbl_Date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // end_dt
            // 
            this.end_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.end_dt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end_dt.Location = new System.Drawing.Point(321, 17);
            this.end_dt.Name = "end_dt";
            this.end_dt.Size = new System.Drawing.Size(135, 24);
            this.end_dt.TabIndex = 113;
            this.end_dt.ValueChanged += new System.EventHandler(this.end_dt_ValueChanged);
            // 
            // start_dt
            // 
            this.start_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.start_dt.Location = new System.Drawing.Point(153, 17);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(135, 24);
            this.start_dt.TabIndex = 112;
            this.start_dt.ValueChanged += new System.EventHandler(this.start_dt_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(296, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 15);
            this.label2.TabIndex = 114;
            this.label2.Text = "~";
            // 
            // EQPCheckRslt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1664, 863);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "EQPCheckRslt";
            this.Text = "설비점검실적관리";
            this.Load += new System.EventHandler(this.EQPCheckRslt_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipPageID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.DateTimePicker end_dt;
        private System.Windows.Forms.DateTimePicker start_dt;
        private System.Windows.Forms.Label label2;
        private C1.Win.C1Input.C1Button btnExcel;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1Button btnDisplay;
        private System.Windows.Forms.CheckBox cb_DelayYN_Gp;
        public System.Windows.Forms.ComboBox cbx_CheckItem_Gp;
        private System.Windows.Forms.Label lb_TestItem;
        public System.Windows.Forms.ComboBox cbx_Eq_Gp;
        private System.Windows.Forms.Label lb_EQ;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain;
        private C1.Win.C1Input.C1Button btnAddRes;
    }
}