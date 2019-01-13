namespace SystemControlClassLibrary
{
    partial class frmRB_RELABEL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRB_RELABEL));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.btnLabel = new C1.Win.C1Input.C1Button();
            this.btnClose = new C1.Win.C1Input.C1Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoNonPrint = new System.Windows.Forms.RadioButton();
            this.rdoPrint = new System.Windows.Forms.RadioButton();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.cboCircle = new System.Windows.Forms.ComboBox();
            this.cboSquare = new System.Windows.Forms.ComboBox();
            this.rdoCircle = new System.Windows.Forms.RadioButton();
            this.rdoSquare = new System.Windows.Forms.RadioButton();
            this.chkNo5 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoD3RT = new System.Windows.Forms.RadioButton();
            this.rdoD3PR = new System.Windows.Forms.RadioButton();
            this.chkNo3 = new System.Windows.Forms.CheckBox();
            this.lbl_pono = new System.Windows.Forms.Label();
            this.txtBundle_no = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboWorkTeam = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.end_dt = new System.Windows.Forms.DateTimePicker();
            this.start_dt = new System.Windows.Forms.DateTimePicker();
            this.cboFactory = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.grdMain = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tmTime = new System.Windows.Forms.Timer(this.components);
            this.toolTipPageID = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1177, 753);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnDisplay, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnLabel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1171, 36);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // btnDisplay
            // 
            this.btnDisplay.AutoSize = true;
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDisplay.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplay.Image")));
            this.btnDisplay.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDisplay.Location = new System.Drawing.Point(934, 3);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(74, 30);
            this.btnDisplay.TabIndex = 14;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // btnLabel
            // 
            this.btnLabel.AutoSize = true;
            this.btnLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLabel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.btnLabel.Image = ((System.Drawing.Image)(resources.GetObject("btnLabel.Image")));
            this.btnLabel.Location = new System.Drawing.Point(1014, 3);
            this.btnLabel.Name = "btnLabel";
            this.btnLabel.Size = new System.Drawing.Size(74, 30);
            this.btnLabel.TabIndex = 16;
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
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(1094, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 30);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.cboCircle);
            this.panel2.Controls.Add(this.cboSquare);
            this.panel2.Controls.Add(this.rdoCircle);
            this.panel2.Controls.Add(this.rdoSquare);
            this.panel2.Controls.Add(this.chkNo5);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.chkNo3);
            this.panel2.Controls.Add(this.lbl_pono);
            this.panel2.Controls.Add(this.txtBundle_no);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cboWorkTeam);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.end_dt);
            this.panel2.Controls.Add(this.start_dt);
            this.panel2.Controls.Add(this.cboFactory);
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1171, 95);
            this.panel2.TabIndex = 9;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.rdoNonPrint);
            this.groupBox3.Controls.Add(this.rdoPrint);
            this.groupBox3.Controls.Add(this.rdoAll);
            this.groupBox3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point(812, 45);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 46);
            this.groupBox3.TabIndex = 134;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "조회조건2";
            // 
            // rdoNonPrint
            // 
            this.rdoNonPrint.AutoSize = true;
            this.rdoNonPrint.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoNonPrint.Location = new System.Drawing.Point(141, 17);
            this.rdoNonPrint.Name = "rdoNonPrint";
            this.rdoNonPrint.Size = new System.Drawing.Size(77, 20);
            this.rdoNonPrint.TabIndex = 134;
            this.rdoNonPrint.Text = "미발행";
            this.rdoNonPrint.UseVisualStyleBackColor = true;
            // 
            // rdoPrint
            // 
            this.rdoPrint.AutoSize = true;
            this.rdoPrint.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoPrint.Location = new System.Drawing.Point(75, 17);
            this.rdoPrint.Name = "rdoPrint";
            this.rdoPrint.Size = new System.Drawing.Size(60, 20);
            this.rdoPrint.TabIndex = 133;
            this.rdoPrint.Text = "발행";
            this.rdoPrint.UseVisualStyleBackColor = true;
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Checked = true;
            this.rdoAll.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoAll.Location = new System.Drawing.Point(9, 16);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(60, 20);
            this.rdoAll.TabIndex = 132;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "전체";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // cboCircle
            // 
            this.cboCircle.Enabled = false;
            this.cboCircle.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cboCircle.FormattingEnabled = true;
            this.cboCircle.Items.AddRange(new object[] {
            "LPT1",
            "COM1",
            "COM2"});
            this.cboCircle.Location = new System.Drawing.Point(335, 57);
            this.cboCircle.Name = "cboCircle";
            this.cboCircle.Size = new System.Drawing.Size(106, 23);
            this.cboCircle.TabIndex = 131;
            // 
            // cboSquare
            // 
            this.cboSquare.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cboSquare.FormattingEnabled = true;
            this.cboSquare.Items.AddRange(new object[] {
            "LPT1",
            "COM1",
            "COM2"});
            this.cboSquare.Location = new System.Drawing.Point(116, 57);
            this.cboSquare.Name = "cboSquare";
            this.cboSquare.Size = new System.Drawing.Size(106, 23);
            this.cboSquare.TabIndex = 130;
            // 
            // rdoCircle
            // 
            this.rdoCircle.AutoSize = true;
            this.rdoCircle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoCircle.Location = new System.Drawing.Point(235, 59);
            this.rdoCircle.Name = "rdoCircle";
            this.rdoCircle.Size = new System.Drawing.Size(94, 20);
            this.rdoCircle.TabIndex = 129;
            this.rdoCircle.Text = "원형라벨";
            this.rdoCircle.UseVisualStyleBackColor = true;
            this.rdoCircle.CheckedChanged += new System.EventHandler(this.rdoCircle_CheckedChanged);
            // 
            // rdoSquare
            // 
            this.rdoSquare.AutoSize = true;
            this.rdoSquare.Checked = true;
            this.rdoSquare.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoSquare.Location = new System.Drawing.Point(16, 59);
            this.rdoSquare.Name = "rdoSquare";
            this.rdoSquare.Size = new System.Drawing.Size(94, 20);
            this.rdoSquare.TabIndex = 128;
            this.rdoSquare.TabStop = true;
            this.rdoSquare.Text = "사각라벨";
            this.rdoSquare.UseVisualStyleBackColor = true;
            this.rdoSquare.CheckedChanged += new System.EventHandler(this.rdoSquare_CheckedChanged);
            // 
            // chkNo5
            // 
            this.chkNo5.AutoSize = true;
            this.chkNo5.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkNo5.Location = new System.Drawing.Point(536, 59);
            this.chkNo5.Name = "chkNo5";
            this.chkNo5.Size = new System.Drawing.Size(79, 19);
            this.chkNo5.TabIndex = 127;
            this.chkNo5.Text = "3제품장";
            this.chkNo5.UseVisualStyleBackColor = true;
            this.chkNo5.CheckedChanged += new System.EventHandler(this.chkNo5_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.rdoD3RT);
            this.groupBox2.Controls.Add(this.rdoD3PR);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox2.Location = new System.Drawing.Point(642, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(141, 46);
            this.groupBox2.TabIndex = 126;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "조회조건";
            // 
            // rdoD3RT
            // 
            this.rdoD3RT.AutoSize = true;
            this.rdoD3RT.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoD3RT.Location = new System.Drawing.Point(75, 17);
            this.rdoD3RT.Name = "rdoD3RT";
            this.rdoD3RT.Size = new System.Drawing.Size(60, 20);
            this.rdoD3RT.TabIndex = 133;
            this.rdoD3RT.Text = "반납";
            this.rdoD3RT.UseVisualStyleBackColor = true;
            // 
            // rdoD3PR
            // 
            this.rdoD3PR.AutoSize = true;
            this.rdoD3PR.Checked = true;
            this.rdoD3PR.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoD3PR.Location = new System.Drawing.Point(9, 16);
            this.rdoD3PR.Name = "rdoD3PR";
            this.rdoD3PR.Size = new System.Drawing.Size(60, 20);
            this.rdoD3PR.TabIndex = 132;
            this.rdoD3PR.TabStop = true;
            this.rdoD3PR.Text = "검사";
            this.rdoD3PR.UseVisualStyleBackColor = true;
            // 
            // chkNo3
            // 
            this.chkNo3.AutoSize = true;
            this.chkNo3.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkNo3.Location = new System.Drawing.Point(474, 59);
            this.chkNo3.Name = "chkNo3";
            this.chkNo3.Size = new System.Drawing.Size(52, 19);
            this.chkNo3.TabIndex = 125;
            this.chkNo3.Text = "No3";
            this.chkNo3.UseVisualStyleBackColor = true;
            this.chkNo3.CheckedChanged += new System.EventHandler(this.chkNo3_CheckedChanged);
            // 
            // lbl_pono
            // 
            this.lbl_pono.AutoSize = true;
            this.lbl_pono.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_pono.Location = new System.Drawing.Point(384, 11);
            this.lbl_pono.Name = "lbl_pono";
            this.lbl_pono.Size = new System.Drawing.Size(30, 16);
            this.lbl_pono.TabIndex = 121;
            this.lbl_pono.Text = "BD";
            // 
            // txtBundle_no
            // 
            this.txtBundle_no.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBundle_no.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBundle_no.Location = new System.Drawing.Point(442, 7);
            this.txtBundle_no.MaxLength = 10;
            this.txtBundle_no.Name = "txtBundle_no";
            this.txtBundle_no.Size = new System.Drawing.Size(100, 25);
            this.txtBundle_no.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(789, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 16);
            this.label5.TabIndex = 119;
            this.label5.Text = "근무조";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboWorkTeam
            // 
            this.cboWorkTeam.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cboWorkTeam.FormattingEnabled = true;
            this.cboWorkTeam.Items.AddRange(new object[] {
            "A조",
            "B조",
            "C조",
            "D조",
            "E조",
            "F조"});
            this.cboWorkTeam.Location = new System.Drawing.Point(872, 7);
            this.cboWorkTeam.Name = "cboWorkTeam";
            this.cboWorkTeam.Size = new System.Drawing.Size(106, 23);
            this.cboWorkTeam.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 107;
            this.label1.Text = "검사일자";
            // 
            // end_dt
            // 
            this.end_dt.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.end_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.end_dt.Location = new System.Drawing.Point(235, 7);
            this.end_dt.Name = "end_dt";
            this.end_dt.Size = new System.Drawing.Size(120, 25);
            this.end_dt.TabIndex = 8;
            this.end_dt.ValueChanged += new System.EventHandler(this.End_dt_ValueChanged);
            // 
            // start_dt
            // 
            this.start_dt.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.start_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.start_dt.Location = new System.Drawing.Point(95, 7);
            this.start_dt.Name = "start_dt";
            this.start_dt.Size = new System.Drawing.Size(116, 25);
            this.start_dt.TabIndex = 7;
            this.start_dt.ValueChanged += new System.EventHandler(this.Start_dt_ValueChanged);
            // 
            // cboFactory
            // 
            this.cboFactory.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboFactory.FormattingEnabled = true;
            this.cboFactory.Location = new System.Drawing.Point(642, 7);
            this.cboFactory.Name = "cboFactory";
            this.cboFactory.Size = new System.Drawing.Size(116, 23);
            this.cboFactory.TabIndex = 0;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.Location = new System.Drawing.Point(577, 11);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(42, 16);
            this.label25.TabIndex = 59;
            this.label25.Text = "공장";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(212, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 16);
            this.label6.TabIndex = 22;
            this.label6.Text = "~";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAll);
            this.groupBox1.Controls.Add(this.grdMain);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1171, 604);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAll.Location = new System.Drawing.Point(35, -2);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(90, 19);
            this.chkAll.TabIndex = 3;
            this.chkAll.Text = "전체선택";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // grdMain
            // 
            this.grdMain.ColumnInfo = resources.GetString("grdMain.ColumnInfo");
            this.grdMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain.Font = new System.Drawing.Font("굴림", 9F);
            this.grdMain.Location = new System.Drawing.Point(3, 17);
            this.grdMain.Name = "grdMain";
            this.grdMain.Rows.Count = 1;
            this.grdMain.Rows.DefaultSize = 20;
            this.grdMain.Size = new System.Drawing.Size(1165, 584);
            this.grdMain.StyleInfo = resources.GetString("grdMain.StyleInfo");
            this.grdMain.TabIndex = 2;
            // 
            // frmRB_RELABEL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1177, 753);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmRB_RELABEL";
            this.Text = "frmRB_RELABEL";
            this.Activated += new System.EventHandler(this.frmRB_RELABEL_Activated);
            this.Load += new System.EventHandler(this.frmRB_RELABEL_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Timer tmTime;
        private System.Windows.Forms.ToolTip toolTipPageID;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip3;
        private C1.Win.C1Input.C1Button btnDisplay;
        private C1.Win.C1Input.C1Button btnLabel;
        private C1.Win.C1Input.C1Button btnClose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboCircle;
        private System.Windows.Forms.ComboBox cboSquare;
        private System.Windows.Forms.RadioButton rdoCircle;
        private System.Windows.Forms.RadioButton rdoSquare;
        private System.Windows.Forms.CheckBox chkNo5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoD3RT;
        private System.Windows.Forms.RadioButton rdoD3PR;
        private System.Windows.Forms.CheckBox chkNo3;
        private System.Windows.Forms.Label lbl_pono;
        private System.Windows.Forms.TextBox txtBundle_no;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboWorkTeam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker end_dt;
        private System.Windows.Forms.DateTimePicker start_dt;
        private System.Windows.Forms.ComboBox cboFactory;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoNonPrint;
        private System.Windows.Forms.RadioButton rdoPrint;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.CheckBox chkAll;
    }
}