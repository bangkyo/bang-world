using System;
using System.Windows.Forms;

namespace BG_L2_120
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tmTime = new System.Windows.Forms.Timer(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.msgshow_lb = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.delete_ts = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsTop = new System.Windows.Forms.ToolStrip();
            this.tsBtnAllDelete = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.pbx_StartUpPageActive = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.delete_ts.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_StartUpPageActive)).BeginInit();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 60);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 922);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 310F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.msgshow_lb, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDateTime, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 955);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1661, 27);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // msgshow_lb
            // 
            this.msgshow_lb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.msgshow_lb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.msgshow_lb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgshow_lb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.msgshow_lb.ForeColor = System.Drawing.Color.Black;
            this.msgshow_lb.Location = new System.Drawing.Point(3, 3);
            this.msgshow_lb.Margin = new System.Windows.Forms.Padding(3);
            this.msgshow_lb.Name = "msgshow_lb";
            this.msgshow_lb.Size = new System.Drawing.Size(1345, 21);
            this.msgshow_lb.TabIndex = 13;
            this.msgshow_lb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateTime
            // 
            this.lblDateTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.lblDateTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDateTime.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDateTime.Location = new System.Drawing.Point(1354, 3);
            this.lblDateTime.Margin = new System.Windows.Forms.Padding(3);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(304, 21);
            this.lblDateTime.TabIndex = 12;
            this.lblDateTime.Text = "9999-99-99 99:99:99";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.miniToolStrip.Location = new System.Drawing.Point(-11, 23);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.miniToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.miniToolStrip.Size = new System.Drawing.Size(20, 23);
            this.miniToolStrip.TabIndex = 4;
            // 
            // delete_ts
            // 
            this.delete_ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.delete_ts.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.delete_ts.Location = new System.Drawing.Point(1241, 0);
            this.delete_ts.Name = "delete_ts";
            this.delete_ts.Padding = new System.Windows.Forms.Padding(0);
            this.delete_ts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.delete_ts.Size = new System.Drawing.Size(20, 23);
            this.delete_ts.TabIndex = 4;
            this.delete_ts.Text = "toolStrip1";
            this.delete_ts.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.delete_ts_ItemClicked);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::BG_L2_120.Properties.Resources.smallBlackX;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton1.Text = "All Close";
            this.toolStripButton1.Click += new System.EventHandler(this.menuStripMouseHover);
            // 
            // tsTop
            // 
            this.tsTop.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsTop.Location = new System.Drawing.Point(0, 0);
            this.tsTop.Name = "tsTop";
            this.tsTop.Padding = new System.Windows.Forms.Padding(0);
            this.tsTop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tsTop.Size = new System.Drawing.Size(1661, 25);
            this.tsTop.TabIndex = 3;
            this.tsTop.Text = "toolStrip1";
            // 
            // tsBtnAllDelete
            // 
            this.tsBtnAllDelete.Name = "tsBtnAllDelete";
            this.tsBtnAllDelete.Size = new System.Drawing.Size(23, 23);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.tsTop, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 60);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1661, 28);
            this.tableLayoutPanel2.TabIndex = 17;
            // 
            // mnuMain
            // 
            this.mnuMain.AutoSize = false;
            this.mnuMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(140)))), ((int)(((byte)(204)))));
            this.mnuMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mnuMain.Font = new System.Drawing.Font("돋움체", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1664, 60);
            this.mnuMain.TabIndex = 1;
            this.mnuMain.Text = "menuStrip1";
            this.mnuMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuMain_ItemClicked);
            this.mnuMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mnuMain_MouseMove);
            // 
            // pbx_StartUpPageActive
            // 
            this.pbx_StartUpPageActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(140)))), ((int)(((byte)(204)))));
            this.pbx_StartUpPageActive.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_StartUpPageActive.BackgroundImage")));
            this.pbx_StartUpPageActive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_StartUpPageActive.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbx_StartUpPageActive.Location = new System.Drawing.Point(22, 15);
            this.pbx_StartUpPageActive.Margin = new System.Windows.Forms.Padding(0);
            this.pbx_StartUpPageActive.Name = "pbx_StartUpPageActive";
            this.pbx_StartUpPageActive.Size = new System.Drawing.Size(102, 31);
            this.pbx_StartUpPageActive.TabIndex = 22;
            this.pbx_StartUpPageActive.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1664, 982);
            this.Controls.Add(this.pbx_StartUpPageActive);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.mnuMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMain;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "중형그라인더시스템 ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.delete_ts.ResumeLayout(false);
            this.delete_ts.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_StartUpPageActive)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.Timer tmTime;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label msgshow_lb;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStrip delete_ts;
        private System.Windows.Forms.ToolStrip tsTop;
        private System.Windows.Forms.ToolStripButton tsBtnAllDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private PictureBox pbx_StartUpPageActive;
    }
}