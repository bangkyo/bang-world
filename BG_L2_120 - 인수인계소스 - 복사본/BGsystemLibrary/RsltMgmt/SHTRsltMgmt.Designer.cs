using System;
using C1.Win.C1FlexGrid;
using C1.Win.C1Input;

namespace BGsystemLibrary.MatMgmt
{
    partial class SHTRsltMgmt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SHTRsltMgmt));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TabOpt = new C1.Win.C1Command.C1DockingTab();
            this.TabP1 = new C1.Win.C1Command.C1DockingTabPage();
            this.grdMain1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.TabP2 = new C1.Win.C1Command.C1DockingTabPage();
            this.grdMain2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExcel = new C1.Win.C1Input.C1Button();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.tb_BloomID = new C1.Win.C1Input.C1TextBox();
            this.tb_EndDT = new C1.Win.C1Input.C1TextBox();
            this.tb_StartDT = new C1.Win.C1Input.C1TextBox();
            this.txt_Heat = new C1.Win.C1Input.C1TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_BloomID = new System.Windows.Forms.Label();
            this.lb_selected_date = new System.Windows.Forms.Label();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.end_dt = new System.Windows.Forms.DateTimePicker();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pbx_Search = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabOpt)).BeginInit();
            this.TabOpt.SuspendLayout();
            this.TabP1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).BeginInit();
            this.TabP2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_BloomID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_EndDT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_StartDT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Heat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_Search)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.TabOpt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 756F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1664, 863);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // TabOpt
            // 
            this.TabOpt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TabOpt.Controls.Add(this.TabP1);
            this.TabOpt.Controls.Add(this.TabP2);
            this.TabOpt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabOpt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabOpt.Location = new System.Drawing.Point(3, 64);
            this.TabOpt.Name = "TabOpt";
            this.TabOpt.SelectedIndex = 1;
            this.TabOpt.SelectedTabBold = true;
            this.TabOpt.Size = new System.Drawing.Size(1658, 796);
            this.TabOpt.SplitterWidth = 1;
            this.TabOpt.TabIndex = 9;
            this.TabOpt.TabsSpacing = 0;
            this.TabOpt.TabStyle = C1.Win.C1Command.TabStyleEnum.Classic;
            this.TabOpt.VisualStyle = C1.Win.C1Command.VisualStyle.Custom;
            this.TabOpt.VisualStyleBase = C1.Win.C1Command.VisualStyle.Classic;
            this.TabOpt.SelectedTabChanged += new System.EventHandler(this.TabOpt_SelectedTabChanged);
            // 
            // TabP1
            // 
            this.TabP1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TabP1.Controls.Add(this.grdMain1);
            this.TabP1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabP1.Location = new System.Drawing.Point(1, 29);
            this.TabP1.Name = "TabP1";
            this.TabP1.Size = new System.Drawing.Size(1656, 766);
            this.TabP1.TabBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TabP1.TabBackColorSelected = System.Drawing.SystemColors.ControlLightLight;
            this.TabP1.TabIndex = 0;
            this.TabP1.Text = "쇼트실적";
            // 
            // grdMain1
            // 
            this.grdMain1.AutoClipboard = true;
            this.grdMain1.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.grdMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain1.Font = new System.Drawing.Font("돋움체", 9F);
            this.grdMain1.Location = new System.Drawing.Point(0, 0);
            this.grdMain1.Name = "grdMain1";
            this.grdMain1.Rows.Count = 1;
            this.grdMain1.Rows.DefaultSize = 20;
            this.grdMain1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain1.Size = new System.Drawing.Size(1656, 766);
            this.grdMain1.StyleInfo = resources.GetString("grdMain1.StyleInfo");
            this.grdMain1.TabIndex = 10;
            this.grdMain1.SelChange += new System.EventHandler(this.grdMain1_SelChange);
            // 
            // TabP2
            // 
            this.TabP2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TabP2.Controls.Add(this.grdMain2);
            this.TabP2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabP2.Location = new System.Drawing.Point(1, 29);
            this.TabP2.Name = "TabP2";
            this.TabP2.Size = new System.Drawing.Size(1656, 766);
            this.TabP2.TabBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TabP2.TabBackColorSelected = System.Drawing.SystemColors.ControlLightLight;
            this.TabP2.TabIndex = 1;
            this.TabP2.Text = "쇼트조업정보";
            // 
            // grdMain2
            // 
            this.grdMain2.AutoClipboard = true;
            this.grdMain2.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.grdMain2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdMain2.Location = new System.Drawing.Point(0, 0);
            this.grdMain2.Name = "grdMain2";
            this.grdMain2.Rows.Count = 1;
            this.grdMain2.Rows.DefaultSize = 19;
            this.grdMain2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain2.Size = new System.Drawing.Size(1656, 766);
            this.grdMain2.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.Controls.Add(this.pbx_Search);
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.tb_BloomID);
            this.panel1.Controls.Add(this.tb_EndDT);
            this.panel1.Controls.Add(this.tb_StartDT);
            this.panel1.Controls.Add(this.txt_Heat);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lb_BloomID);
            this.panel1.Controls.Add(this.lb_selected_date);
            this.panel1.Controls.Add(this.lbl_Date);
            this.panel1.Controls.Add(this.end_dt);
            this.panel1.Controls.Add(this.start_dt);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("현대하모니 M", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1658, 55);
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
            this.btnExcel.Location = new System.Drawing.Point(1569, 10);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 35);
            this.btnExcel.TabIndex = 127;
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
            this.btnDisplay.Location = new System.Drawing.Point(1483, 10);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(80, 35);
            this.btnDisplay.TabIndex = 125;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.Button_Click);
            // 
            // tb_BloomID
            // 
            this.tb_BloomID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tb_BloomID.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_BloomID.Location = new System.Drawing.Point(808, 17);
            this.tb_BloomID.Name = "tb_BloomID";
            this.tb_BloomID.ReadOnly = true;
            this.tb_BloomID.Size = new System.Drawing.Size(127, 24);
            this.tb_BloomID.TabIndex = 116;
            this.tb_BloomID.Tag = null;
            // 
            // tb_EndDT
            // 
            this.tb_EndDT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tb_EndDT.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_EndDT.Location = new System.Drawing.Point(1297, 17);
            this.tb_EndDT.Name = "tb_EndDT";
            this.tb_EndDT.ReadOnly = true;
            this.tb_EndDT.Size = new System.Drawing.Size(160, 24);
            this.tb_EndDT.TabIndex = 116;
            this.tb_EndDT.Tag = null;
            // 
            // tb_StartDT
            // 
            this.tb_StartDT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tb_StartDT.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_StartDT.Location = new System.Drawing.Point(1109, 17);
            this.tb_StartDT.Name = "tb_StartDT";
            this.tb_StartDT.ReadOnly = true;
            this.tb_StartDT.Size = new System.Drawing.Size(160, 24);
            this.tb_StartDT.TabIndex = 116;
            this.tb_StartDT.Tag = null;
            // 
            // txt_Heat
            // 
            this.txt_Heat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_Heat.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_Heat.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Heat.Location = new System.Drawing.Point(489, 17);
            this.txt_Heat.Name = "txt_Heat";
            this.txt_Heat.Size = new System.Drawing.Size(127, 24);
            this.txt_Heat.TabIndex = 116;
            this.txt_Heat.Tag = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(443, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 115;
            this.label1.Text = "HEAT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_BloomID
            // 
            this.lb_BloomID.AutoSize = true;
            this.lb_BloomID.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_BloomID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lb_BloomID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_BloomID.Location = new System.Drawing.Point(666, 22);
            this.lb_BloomID.Margin = new System.Windows.Forms.Padding(0);
            this.lb_BloomID.Name = "lb_BloomID";
            this.lb_BloomID.Size = new System.Drawing.Size(139, 15);
            this.lb_BloomID.TabIndex = 108;
            this.lb_BloomID.Text = "선택된 Bloom ID";
            this.lb_BloomID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_selected_date
            // 
            this.lb_selected_date.AutoSize = true;
            this.lb_selected_date.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_selected_date.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(65)))), ((int)(((byte)(96)))));
            this.lb_selected_date.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_selected_date.Location = new System.Drawing.Point(971, 22);
            this.lb_selected_date.Margin = new System.Windows.Forms.Padding(0);
            this.lb_selected_date.Name = "lb_selected_date";
            this.lb_selected_date.Size = new System.Drawing.Size(135, 15);
            this.lb_selected_date.TabIndex = 108;
            this.lb_selected_date.Text = "선택된 작업일시";
            this.lb_selected_date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.lbl_Date.Text = "작업일자";
            this.lbl_Date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // end_dt
            // 
            this.end_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.end_dt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end_dt.Location = new System.Drawing.Point(277, 17);
            this.end_dt.Name = "end_dt";
            this.end_dt.Size = new System.Drawing.Size(135, 24);
            this.end_dt.TabIndex = 3;
            this.end_dt.ValueChanged += new System.EventHandler(this.end_dt_ValueChanged);
            // 
            // start_dt
            // 
            this.start_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.start_dt.Location = new System.Drawing.Point(110, 17);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(135, 24);
            this.start_dt.TabIndex = 2;
            this.start_dt.ValueChanged += new System.EventHandler(this.start_dt_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(1274, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 15);
            this.label3.TabIndex = 33;
            this.label3.Text = "~";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(251, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "~";
            // 
            // pbx_Search
            // 
            this.pbx_Search.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbx_Search.BackColor = System.Drawing.Color.Transparent;
            this.pbx_Search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbx_Search.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbx_Search.Image = global::BGsystemLibrary.Properties.Resources.Search16;
            this.pbx_Search.InitialImage = global::BGsystemLibrary.Properties.Resources.Search16;
            this.pbx_Search.Location = new System.Drawing.Point(622, 19);
            this.pbx_Search.Name = "pbx_Search";
            this.pbx_Search.Size = new System.Drawing.Size(22, 19);
            this.pbx_Search.TabIndex = 128;
            this.pbx_Search.TabStop = false;
            this.pbx_Search.Click += new System.EventHandler(this.pbx_Search_Click);
            // 
            // SHTRsltMgmt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1664, 863);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("현대하모니 M", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SHTRsltMgmt";
            this.Text = "쇼트실적관리";
            this.Load += new System.EventHandler(this.SHTRsltMgmt_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabOpt)).EndInit();
            this.TabOpt.ResumeLayout(false);
            this.TabP1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).EndInit();
            this.TabP2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_BloomID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_EndDT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_StartDT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Heat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_Search)).EndInit();
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
        private System.Windows.Forms.Label lbl_Date;
        private C1TextBox txt_Heat;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1Command.C1DockingTab TabOpt;
        private C1.Win.C1Command.C1DockingTabPage TabP1;
        private C1.Win.C1Command.C1DockingTabPage TabP2;
        private C1FlexGrid grdMain1;
        private C1FlexGrid grdMain2;
        private C1Button btnExcel;
        private C1Button btnDisplay;
        private C1TextBox tb_StartDT;
        private System.Windows.Forms.Label lb_selected_date;
        private C1TextBox tb_BloomID;
        private System.Windows.Forms.Label lb_BloomID;
        private C1TextBox tb_EndDT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbx_Search;
    }
}