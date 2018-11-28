using C1.Win.C1FlexGrid;
using C1.Win.C1Input;
using System;

namespace BGsystemLibrary.RsltMgmt
{
    partial class DUSTInfoMgmt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DUSTInfoMgmt));
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
            this.lbl_Date = new System.Windows.Forms.Label();
            this.end_dt = new System.Windows.Forms.DateTimePicker();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabOpt)).BeginInit();
            this.TabOpt.SuspendLayout();
            this.TabP1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).BeginInit();
            this.TabP2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1257, 863);
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
            this.TabOpt.Size = new System.Drawing.Size(1251, 796);
            this.TabOpt.SplitterWidth = 1;
            this.TabOpt.TabIndex = 9;
            this.TabOpt.TabsSpacing = 0;
            this.TabOpt.TabStyle = C1.Win.C1Command.TabStyleEnum.Classic;
            this.TabOpt.VisualStyle = C1.Win.C1Command.VisualStyle.Custom;
            this.TabOpt.VisualStyleBase = C1.Win.C1Command.VisualStyle.Classic;
            this.TabOpt.SelectedIndexChanged += new System.EventHandler(this.TabOpt_SelectedIndexChanged);
            // 
            // TabP1
            // 
            this.TabP1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TabP1.Controls.Add(this.grdMain1);
            this.TabP1.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabP1.Location = new System.Drawing.Point(1, 29);
            this.TabP1.Name = "TabP1";
            this.TabP1.Size = new System.Drawing.Size(1249, 766);
            this.TabP1.TabBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TabP1.TabBackColorSelected = System.Drawing.SystemColors.ControlLightLight;
            this.TabP1.TabIndex = 0;
            this.TabP1.Text = "쇼트집진정보";
            // 
            // grdMain1
            // 
            this.grdMain1.AutoClipboard = true;
            this.grdMain1.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.grdMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdMain1.Location = new System.Drawing.Point(0, 0);
            this.grdMain1.Name = "grdMain1";
            this.grdMain1.Rows.Count = 1;
            this.grdMain1.Rows.DefaultSize = 20;
            this.grdMain1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.grdMain1.Size = new System.Drawing.Size(1249, 766);
            this.grdMain1.TabIndex = 10;
            // 
            // TabP2
            // 
            this.TabP2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TabP2.Controls.Add(this.grdMain2);
            this.TabP2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TabP2.Location = new System.Drawing.Point(1, 29);
            this.TabP2.Name = "TabP2";
            this.TabP2.Size = new System.Drawing.Size(1249, 766);
            this.TabP2.TabBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TabP2.TabBackColorSelected = System.Drawing.SystemColors.ControlLightLight;
            this.TabP2.TabIndex = 1;
            this.TabP2.Text = "그라인딩집진정보";
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
            this.grdMain2.Size = new System.Drawing.Size(1249, 766);
            this.grdMain2.TabIndex = 10;
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
            this.btnExcel.Image = global::BGsystemLibrary.Properties.Resources.excel;
            this.btnExcel.Location = new System.Drawing.Point(1162, 9);
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
            this.btnDisplay.Location = new System.Drawing.Point(1076, 9);
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
            this.lbl_Date.TabIndex = 108;
            this.lbl_Date.Text = "자료수집시각";
            this.lbl_Date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // end_dt
            // 
            this.end_dt.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.end_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.end_dt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end_dt.Location = new System.Drawing.Point(388, 17);
            this.end_dt.Name = "end_dt";
            this.end_dt.Size = new System.Drawing.Size(190, 24);
            this.end_dt.TabIndex = 3;
            this.end_dt.ValueChanged += new System.EventHandler(this.end_dt_ValueChanged);
            // 
            // start_dt
            // 
            this.start_dt.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.start_dt.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.start_dt.Location = new System.Drawing.Point(159, 17);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(190, 24);
            this.start_dt.TabIndex = 2;
            this.start_dt.ValueChanged += new System.EventHandler(this.start_dt_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(362, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "~";
            // 
            // DUSTInfoMgmt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1257, 863);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("현대하모니 M", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DUSTInfoMgmt";
            this.Text = "집진정보관리";
            this.Load += new System.EventHandler(this.DUSTInfoMgmt_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabOpt)).EndInit();
            this.TabOpt.ResumeLayout(false);
            this.TabP1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).EndInit();
            this.TabP2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private C1.Win.C1Command.C1DockingTab TabOpt;
        private C1.Win.C1Command.C1DockingTabPage TabP1;
        private C1.Win.C1Command.C1DockingTabPage TabP2;
        private C1FlexGrid grdMain1;
        private C1FlexGrid grdMain2;
        private C1Button btnExcel;
        private C1Button btnDisplay;
    }
}