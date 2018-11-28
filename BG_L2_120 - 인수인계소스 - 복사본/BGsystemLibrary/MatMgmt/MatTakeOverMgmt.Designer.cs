using System;
using C1.Win.C1FlexGrid;

namespace BGsystemLibrary.MatMgmt
{
    partial class MatTakeOverMgmt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatTakeOverMgmt));
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnForceInsert = new C1.Win.C1Input.C1Button();
            this.btnExcel = new C1.Win.C1Input.C1Button();
            this.btnTakOverCancel = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.btnTakeOver = new C1.Win.C1Input.C1Button();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.pbx_Search = new System.Windows.Forms.PictureBox();
            this.txt_Heat = new C1.Win.C1Input.C1TextBox();
            this.end_dt = new System.Windows.Forms.DateTimePicker();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cbx_TakeOver_Gp = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_TakeOverDay = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdSub = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_Search)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Heat)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1584, 863);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnForceInsert);
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.btnTakOverCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnTakeOver);
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.pbx_Search);
            this.panel1.Controls.Add(this.txt_Heat);
            this.panel1.Controls.Add(this.end_dt);
            this.panel1.Controls.Add(this.start_dt);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbx_TakeOver_Gp);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lb_TakeOverDay);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1578, 55);
            this.panel1.TabIndex = 5;
            // 
            // btnForceInsert
            // 
            this.btnForceInsert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForceInsert.AutoSize = true;
            this.btnForceInsert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnForceInsert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnForceInsert.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnForceInsert.Image = ((System.Drawing.Image)(resources.GetObject("btnForceInsert.Image")));
            this.btnForceInsert.Location = new System.Drawing.Point(1471, 9);
            this.btnForceInsert.Name = "btnForceInsert";
            this.btnForceInsert.Size = new System.Drawing.Size(97, 35);
            this.btnForceInsert.TabIndex = 122;
            this.btnForceInsert.Text = "강제등록";
            this.btnForceInsert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnForceInsert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnForceInsert.UseVisualStyleBackColor = true;
            this.btnForceInsert.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExcel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.Image")));
            this.btnExcel.Location = new System.Drawing.Point(1385, 9);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 35);
            this.btnExcel.TabIndex = 120;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnTakOverCancel
            // 
            this.btnTakOverCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTakOverCancel.AutoSize = true;
            this.btnTakOverCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnTakOverCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTakOverCancel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnTakOverCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnTakOverCancel.Image")));
            this.btnTakOverCancel.Location = new System.Drawing.Point(1282, 9);
            this.btnTakOverCancel.Name = "btnTakOverCancel";
            this.btnTakOverCancel.Size = new System.Drawing.Size(97, 35);
            this.btnTakOverCancel.TabIndex = 119;
            this.btnTakOverCancel.Text = "인수취소";
            this.btnTakOverCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTakOverCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTakOverCancel.UseVisualStyleBackColor = true;
            this.btnTakOverCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(1110, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 35);
            this.btnSave.TabIndex = 119;
            this.btnSave.Text = "저장";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnTakeOver
            // 
            this.btnTakeOver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTakeOver.AutoSize = true;
            this.btnTakeOver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnTakeOver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTakeOver.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnTakeOver.Image = ((System.Drawing.Image)(resources.GetObject("btnTakeOver.Image")));
            this.btnTakeOver.Location = new System.Drawing.Point(1195, 9);
            this.btnTakeOver.Name = "btnTakeOver";
            this.btnTakeOver.Size = new System.Drawing.Size(81, 35);
            this.btnTakeOver.TabIndex = 119;
            this.btnTakeOver.Text = "인수";
            this.btnTakeOver.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTakeOver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTakeOver.UseVisualStyleBackColor = true;
            this.btnTakeOver.Click += new System.EventHandler(this.Button_Click);
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
            this.btnDisplay.Location = new System.Drawing.Point(1024, 9);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(80, 35);
            this.btnDisplay.TabIndex = 118;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.Button_Click);
            // 
            // pbx_Search
            // 
            this.pbx_Search.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbx_Search.BackColor = System.Drawing.Color.Transparent;
            this.pbx_Search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbx_Search.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbx_Search.Image = global::BGsystemLibrary.Properties.Resources.Search16;
            this.pbx_Search.InitialImage = global::BGsystemLibrary.Properties.Resources.Search16;
            this.pbx_Search.Location = new System.Drawing.Point(809, 17);
            this.pbx_Search.Name = "pbx_Search";
            this.pbx_Search.Size = new System.Drawing.Size(22, 19);
            this.pbx_Search.TabIndex = 115;
            this.pbx_Search.TabStop = false;
            this.pbx_Search.Click += new System.EventHandler(this.pbx_Search_Click);
            // 
            // txt_Heat
            // 
            this.txt_Heat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_Heat.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_Heat.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Heat.Location = new System.Drawing.Point(697, 15);
            this.txt_Heat.Name = "txt_Heat";
            this.txt_Heat.Size = new System.Drawing.Size(103, 24);
            this.txt_Heat.TabIndex = 114;
            this.txt_Heat.Tag = null;
            // 
            // end_dt
            // 
            this.end_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.end_dt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end_dt.Location = new System.Drawing.Point(459, 15);
            this.end_dt.Name = "end_dt";
            this.end_dt.Size = new System.Drawing.Size(103, 24);
            this.end_dt.TabIndex = 112;
            this.end_dt.ValueChanged += new System.EventHandler(this.end_dt_ValueChanged);
            // 
            // start_dt
            // 
            this.start_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.start_dt.Location = new System.Drawing.Point(328, 15);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(103, 24);
            this.start_dt.TabIndex = 111;
            this.start_dt.ValueChanged += new System.EventHandler(this.start_dt_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(437, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 15);
            this.label2.TabIndex = 113;
            this.label2.Text = "~";
            // 
            // cbx_TakeOver_Gp
            // 
            this.cbx_TakeOver_Gp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_TakeOver_Gp.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbx_TakeOver_Gp.FormattingEnabled = true;
            this.cbx_TakeOver_Gp.Location = new System.Drawing.Point(76, 16);
            this.cbx_TakeOver_Gp.Name = "cbx_TakeOver_Gp";
            this.cbx_TakeOver_Gp.Size = new System.Drawing.Size(89, 23);
            this.cbx_TakeOver_Gp.TabIndex = 110;
            this.cbx_TakeOver_Gp.DropDown += new System.EventHandler(this.cbx_TakeOver_Gp_DropDown);
            this.cbx_TakeOver_Gp.SelectedIndexChanged += new System.EventHandler(this.cbx_TakeOver_Gp_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(651, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 109;
            this.label1.Text = "HEAT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_TakeOverDay
            // 
            this.lb_TakeOverDay.AutoSize = true;
            this.lb_TakeOverDay.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_TakeOverDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lb_TakeOverDay.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_TakeOverDay.Location = new System.Drawing.Point(250, 20);
            this.lb_TakeOverDay.Margin = new System.Windows.Forms.Padding(0);
            this.lb_TakeOverDay.Name = "lb_TakeOverDay";
            this.lb_TakeOverDay.Size = new System.Drawing.Size(75, 15);
            this.lb_TakeOverDay.TabIndex = 3;
            this.lb_TakeOverDay.Text = "인수일자";
            this.lb_TakeOverDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(32, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "구분";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.grdMain, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.grdSub, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 64);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1578, 796);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // grdMain
            // 
            this.grdMain.AutoClipboard = true;
            this.grdMain.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.grdMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdMain.Location = new System.Drawing.Point(3, 3);
            this.grdMain.Name = "grdMain";
            this.grdMain.Rows.Count = 1;
            this.grdMain.Rows.DefaultSize = 19;
            this.grdMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain.Size = new System.Drawing.Size(783, 790);
            this.grdMain.TabIndex = 13;
            this.grdMain.Tag = "Heat";
            this.grdMain.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdMain_BeforeEdit);
            this.grdMain.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdMain_AfterEdit);
            this.grdMain.Click += new System.EventHandler(this.grdMain_Click);
            // 
            // grdSub
            // 
            this.grdSub.AutoClipboard = true;
            this.grdSub.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.grdSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSub.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdSub.Location = new System.Drawing.Point(792, 3);
            this.grdSub.Name = "grdSub";
            this.grdSub.Rows.Count = 1;
            this.grdSub.Rows.DefaultSize = 19;
            this.grdSub.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdSub.Size = new System.Drawing.Size(783, 790);
            this.grdSub.TabIndex = 11;
            this.grdSub.Tag = "Bloom";
            this.grdSub.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdSub_BeforeEdit);
            this.grdSub.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdSub_AfterEdit);
            this.grdSub.CellChecked += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdSub_CellChecked);
            this.grdSub.Click += new System.EventHandler(this.grdSub_Click);
            this.grdSub.Paint += new System.Windows.Forms.PaintEventHandler(this.grdSub_Paint);
            // 
            // MatTakeOverMgmt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1584, 863);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("현대하모니 M", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "MatTakeOverMgmt";
            this.Text = "소재인수관리";
            this.Load += new System.EventHandler(this.MatTakeOverMgmt_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_Search)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Heat)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSub)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.ToolTip toolTipPageID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lb_TakeOverDay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker end_dt;
        private System.Windows.Forms.DateTimePicker start_dt;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cbx_TakeOver_Gp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbx_Search;
        private C1.Win.C1Input.C1TextBox txt_Heat;
        private C1.Win.C1Input.C1Button btnDisplay;
        private C1.Win.C1Input.C1Button btnTakeOver;
        private C1.Win.C1Input.C1Button btnExcel;
        private C1.Win.C1Input.C1Button btnTakOverCancel;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1Button btnForceInsert;
        private C1FlexGrid grdSub;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private C1FlexGrid grdMain;
    }
}