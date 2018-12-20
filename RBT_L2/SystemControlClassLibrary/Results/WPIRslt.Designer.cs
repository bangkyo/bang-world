
namespace SystemControlClassLibrary
{
    partial class WPIRslt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WPIRslt));
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.heattxt = new C1.Win.C1Input.C1TextBox();
            this.poctxt = new C1.Win.C1Input.C1TextBox();
            this.lblHeat = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.lblItemSize = new System.Windows.Forms.Label();
            this.txtItemSize = new C1.Win.C1Input.C1TextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.lblPoc = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.end_dt = new System.Windows.Forms.DateTimePicker();
            this.cboLine_GP = new System.Windows.Forms.ComboBox();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.lblMfgDate = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.lblLine = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.btnReg = new C1.Win.C1Input.C1Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLabel = new C1.Win.C1Input.C1Button();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.title_lb = new System.Windows.Forms.Label();
            this.btnExcel = new C1.Win.C1Input.C1Button();
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            this.cbo_ROUTING_Type = new System.Windows.Forms.ComboBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heattxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.poctxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDisplay
            // 
            this.btnDisplay.AutoSize = true;
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisplay.Font = new System.Drawing.Font("돋움", 11F);
            this.btnDisplay.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplay.Image")));
            this.btnDisplay.Location = new System.Drawing.Point(1020, 3);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(74, 29);
            this.btnDisplay.TabIndex = 0;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panel1.Controls.Add(this.cbo_ROUTING_Type);
            this.panel1.Controls.Add(this.pictureBox6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.heattxt);
            this.panel1.Controls.Add(this.poctxt);
            this.panel1.Controls.Add(this.lblHeat);
            this.panel1.Controls.Add(this.pictureBox5);
            this.panel1.Controls.Add(this.lblItemSize);
            this.panel1.Controls.Add(this.txtItemSize);
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Controls.Add(this.lblPoc);
            this.panel1.Controls.Add(this.pictureBox7);
            this.panel1.Controls.Add(this.end_dt);
            this.panel1.Controls.Add(this.cboLine_GP);
            this.panel1.Controls.Add(this.start_dt);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lblMfgDate);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.lblLine);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(3, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1251, 74);
            this.panel1.TabIndex = 5;
            // 
            // heattxt
            // 
            this.heattxt.AutoSize = false;
            this.heattxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.heattxt.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.heattxt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.heattxt.Location = new System.Drawing.Point(395, 41);
            this.heattxt.MaxLength = 6;
            this.heattxt.Name = "heattxt";
            this.heattxt.Size = new System.Drawing.Size(73, 24);
            this.heattxt.TabIndex = 47;
            this.heattxt.Tag = null;
            this.heattxt.TextDetached = true;
            // 
            // poctxt
            // 
            this.poctxt.AutoSize = false;
            this.poctxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.poctxt.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.poctxt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.poctxt.Location = new System.Drawing.Point(111, 40);
            this.poctxt.MaxLength = 20;
            this.poctxt.Name = "poctxt";
            this.poctxt.Size = new System.Drawing.Size(87, 24);
            this.poctxt.TabIndex = 46;
            this.poctxt.Tag = null;
            this.poctxt.TextDetached = true;
            // 
            // lblHeat
            // 
            this.lblHeat.AutoSize = true;
            this.lblHeat.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeat.Location = new System.Drawing.Point(307, 44);
            this.lblHeat.Name = "lblHeat";
            this.lblHeat.Size = new System.Drawing.Size(51, 16);
            this.lblHeat.TabIndex = 45;
            this.lblHeat.Text = "HEAT";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(570, 50);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(5, 5);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 39;
            this.pictureBox5.TabStop = false;
            // 
            // lblItemSize
            // 
            this.lblItemSize.AutoSize = true;
            this.lblItemSize.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblItemSize.Location = new System.Drawing.Point(581, 44);
            this.lblItemSize.Name = "lblItemSize";
            this.lblItemSize.Size = new System.Drawing.Size(42, 16);
            this.lblItemSize.TabIndex = 38;
            this.lblItemSize.Text = "규격";
            // 
            // txtItemSize
            // 
            this.txtItemSize.AutoSize = false;
            this.txtItemSize.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.txtItemSize.Location = new System.Drawing.Point(629, 41);
            this.txtItemSize.MaxLength = 4;
            this.txtItemSize.Name = "txtItemSize";
            this.txtItemSize.Size = new System.Drawing.Size(80, 24);
            this.txtItemSize.TabIndex = 36;
            this.txtItemSize.Tag = null;
            this.txtItemSize.TextDetached = true;
            this.txtItemSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtItemSize_KeyPress);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(296, 50);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(5, 5);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 37;
            this.pictureBox4.TabStop = false;
            // 
            // lblPoc
            // 
            this.lblPoc.AutoSize = true;
            this.lblPoc.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblPoc.Location = new System.Drawing.Point(61, 44);
            this.lblPoc.Name = "lblPoc";
            this.lblPoc.Size = new System.Drawing.Size(44, 16);
            this.lblPoc.TabIndex = 35;
            this.lblPoc.Text = "POC";
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(50, 50);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(5, 5);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 34;
            this.pictureBox7.TabStop = false;
            // 
            // end_dt
            // 
            this.end_dt.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.end_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.end_dt.Location = new System.Drawing.Point(570, 9);
            this.end_dt.Name = "end_dt";
            this.end_dt.Size = new System.Drawing.Size(144, 24);
            this.end_dt.TabIndex = 2;
            // 
            // cboLine_GP
            // 
            this.cboLine_GP.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cboLine_GP.FormattingEnabled = true;
            this.cboLine_GP.Location = new System.Drawing.Point(111, 10);
            this.cboLine_GP.Name = "cboLine_GP";
            this.cboLine_GP.Size = new System.Drawing.Size(87, 23);
            this.cboLine_GP.TabIndex = 0;
            // 
            // start_dt
            // 
            this.start_dt.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.start_dt.Location = new System.Drawing.Point(395, 9);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(142, 24);
            this.start_dt.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(543, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "~";
            // 
            // lblMfgDate
            // 
            this.lblMfgDate.AutoSize = true;
            this.lblMfgDate.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblMfgDate.Location = new System.Drawing.Point(307, 12);
            this.lblMfgDate.Name = "lblMfgDate";
            this.lblMfgDate.Size = new System.Drawing.Size(76, 16);
            this.lblMfgDate.TabIndex = 10;
            this.lblMfgDate.Text = "작업일자";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(296, 20);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(5, 5);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lblLine.Location = new System.Drawing.Point(61, 13);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(42, 16);
            this.lblLine.TabIndex = 7;
            this.lblLine.Text = "라인";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(50, 20);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(5, 5);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel3.Controls.Add(this.chkAll, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnReg, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 115);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1257, 35);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAll.Location = new System.Drawing.Point(3, 3);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(90, 19);
            this.chkAll.TabIndex = 10;
            this.chkAll.Text = "전체선택";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // btnReg
            // 
            this.btnReg.AutoSize = true;
            this.btnReg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnReg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReg.Font = new System.Drawing.Font("돋움", 11F);
            this.btnReg.Image = ((System.Drawing.Image)(resources.GetObject("btnReg.Image")));
            this.btnReg.Location = new System.Drawing.Point(1162, 3);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(92, 29);
            this.btnReg.TabIndex = 9;
            this.btnReg.Text = "실적등록";
            this.btnReg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdMain, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1257, 863);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // grdMain
            // 
            this.grdMain.AutoClipboard = true;
            this.grdMain.ColumnInfo = resources.GetString("grdMain.ColumnInfo");
            this.grdMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdMain.Location = new System.Drawing.Point(3, 153);
            this.grdMain.Name = "grdMain";
            this.grdMain.Rows.Count = 2;
            this.grdMain.Rows.DefaultSize = 19;
            this.grdMain.Rows.Fixed = 2;
            this.grdMain.Size = new System.Drawing.Size(1251, 707);
            this.grdMain.TabIndex = 9;
            this.grdMain.DoubleClick += new System.EventHandler(this.grdMain_DoubleClick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 311F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.Controls.Add(this.btnLabel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.title_lb, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnExcel, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDisplay, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1257, 35);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnLabel
            // 
            this.btnLabel.AutoSize = true;
            this.btnLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLabel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnLabel.Image = ((System.Drawing.Image)(resources.GetObject("btnLabel.Image")));
            this.btnLabel.Location = new System.Drawing.Point(940, 3);
            this.btnLabel.Name = "btnLabel";
            this.btnLabel.Size = new System.Drawing.Size(74, 29);
            this.btnLabel.TabIndex = 33;
            this.btnLabel.Text = "라벨";
            this.btnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLabel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLabel.UseVisualStyleBackColor = true;
            this.btnLabel.Click += new System.EventHandler(this.btnLabel_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(1180, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 29);
            this.btnClose.TabIndex = 32;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 14);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // title_lb
            // 
            this.title_lb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.title_lb.AutoSize = true;
            this.title_lb.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.title_lb.Location = new System.Drawing.Point(26, 19);
            this.title_lb.Name = "title_lb";
            this.title_lb.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.title_lb.Size = new System.Drawing.Size(305, 16);
            this.title_lb.TabIndex = 0;
            this.title_lb.Text = "*";
            this.title_lb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExcel
            // 
            this.btnExcel.AutoSize = true;
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExcel.Font = new System.Drawing.Font("돋움", 11F);
            this.btnExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.Image")));
            this.btnExcel.Location = new System.Drawing.Point(1100, 3);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(74, 29);
            this.btnExcel.TabIndex = 2;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // cbo_ROUTING_Type
            // 
            this.cbo_ROUTING_Type.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cbo_ROUTING_Type.FormattingEnabled = true;
            this.cbo_ROUTING_Type.Location = new System.Drawing.Point(865, 10);
            this.cbo_ROUTING_Type.Name = "cbo_ROUTING_Type";
            this.cbo_ROUTING_Type.Size = new System.Drawing.Size(64, 23);
            this.cbo_ROUTING_Type.TabIndex = 48;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(809, 21);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(5, 5);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 50;
            this.pictureBox6.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(817, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 49;
            this.label4.Text = "공정";
            // 
            // WPIRslt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 863);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WPIRslt";
            this.Text = "재공실적관리";
            this.Load += new System.EventHandler(this.WPIRslt_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heattxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.poctxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1Input.C1Button btnDisplay;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker end_dt;
        private System.Windows.Forms.ComboBox cboLine_GP;
        private System.Windows.Forms.DateTimePicker start_dt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblMfgDate;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private C1.Win.C1Input.C1Button btnClose;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label title_lb;
        private C1.Win.C1Input.C1Button btnExcel;
        private System.Windows.Forms.ToolTip toolTipPageID;
        private System.Windows.Forms.Label lblPoc;
        private System.Windows.Forms.PictureBox pictureBox7;
        private C1.Win.C1Input.C1TextBox heattxt;
        private C1.Win.C1Input.C1TextBox poctxt;
        private System.Windows.Forms.Label lblHeat;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label lblItemSize;
        private C1.Win.C1Input.C1TextBox txtItemSize;
        private System.Windows.Forms.PictureBox pictureBox4;
        private C1.Win.C1Input.C1Button btnReg;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain;
        private System.Windows.Forms.CheckBox chkAll;
        private C1.Win.C1Input.C1Button btnLabel;
        private System.Windows.Forms.ComboBox cbo_ROUTING_Type;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label4;
    }
}