namespace SystemControlClassLibrary.monitoring
{
    partial class Line3STRTrk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Line3STRTrk));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.msg_lb = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPOCFin = new C1.Win.C1Input.C1Button();
            this.btnOutReg = new C1.Win.C1Input.C1Button();
            this.btnZoneMove = new C1.Win.C1Input.C1Button();
            this.btnMPIReg = new C1.Win.C1Input.C1Button();
            this.btnInsertReg = new C1.Win.C1Input.C1Button();
            this.grdMain7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.orientedTextLabel4 = new ComLib.OrientedTextLabel();
            this.label85 = new System.Windows.Forms.Label();
            this.uC_Zone7 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone11 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone6 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone8 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone3 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone5 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone4 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone10 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone1 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone2 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone9 = new WindowsFormsApplication15.UC_Zone();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 222);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1240, 499);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 122;
            this.pictureBox1.TabStop = false;
            // 
            // msg_lb
            // 
            this.msg_lb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(93)))), ((int)(((byte)(162)))));
            this.msg_lb.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.msg_lb.ForeColor = System.Drawing.Color.White;
            this.msg_lb.Location = new System.Drawing.Point(12, 732);
            this.msg_lb.Margin = new System.Windows.Forms.Padding(3);
            this.msg_lb.Name = "msg_lb";
            this.msg_lb.Size = new System.Drawing.Size(236, 21);
            this.msg_lb.TabIndex = 125;
            this.msg_lb.Text = "교정대기번들정보";
            this.msg_lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel2.Controls.Add(this.btnPOCFin, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnOutReg, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnZoneMove, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnMPIReg, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnInsertReg, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(249, 756);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(97, 174);
            this.tableLayoutPanel2.TabIndex = 124;
            // 
            // btnPOCFin
            // 
            this.btnPOCFin.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnPOCFin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPOCFin.Location = new System.Drawing.Point(3, 38);
            this.btnPOCFin.Name = "btnPOCFin";
            this.btnPOCFin.Size = new System.Drawing.Size(90, 29);
            this.btnPOCFin.TabIndex = 35;
            this.btnPOCFin.Text = "POC 종료";
            this.btnPOCFin.UseVisualStyleBackColor = true;
            // 
            // btnOutReg
            // 
            this.btnOutReg.AutoSize = true;
            this.btnOutReg.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnOutReg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOutReg.Location = new System.Drawing.Point(3, 143);
            this.btnOutReg.Name = "btnOutReg";
            this.btnOutReg.Size = new System.Drawing.Size(90, 29);
            this.btnOutReg.TabIndex = 34;
            this.btnOutReg.Text = "격외등록";
            this.btnOutReg.UseVisualStyleBackColor = true;
            // 
            // btnZoneMove
            // 
            this.btnZoneMove.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnZoneMove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZoneMove.Location = new System.Drawing.Point(3, 108);
            this.btnZoneMove.Name = "btnZoneMove";
            this.btnZoneMove.Size = new System.Drawing.Size(90, 29);
            this.btnZoneMove.TabIndex = 33;
            this.btnZoneMove.Text = "Zone이동";
            this.btnZoneMove.UseVisualStyleBackColor = true;
            this.btnZoneMove.Click += new System.EventHandler(this.btnZoneMove_Click);
            // 
            // btnMPIReg
            // 
            this.btnMPIReg.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnMPIReg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMPIReg.Location = new System.Drawing.Point(3, 73);
            this.btnMPIReg.Name = "btnMPIReg";
            this.btnMPIReg.Size = new System.Drawing.Size(90, 29);
            this.btnMPIReg.TabIndex = 32;
            this.btnMPIReg.Text = "MPI검사등록";
            this.btnMPIReg.UseVisualStyleBackColor = true;
            // 
            // btnInsertReg
            // 
            this.btnInsertReg.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnInsertReg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInsertReg.Location = new System.Drawing.Point(3, 3);
            this.btnInsertReg.Name = "btnInsertReg";
            this.btnInsertReg.Size = new System.Drawing.Size(90, 29);
            this.btnInsertReg.TabIndex = 30;
            this.btnInsertReg.Text = "투입등록";
            this.btnInsertReg.UseVisualStyleBackColor = true;
            // 
            // grdMain7
            // 
            this.grdMain7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain7.AutoClipboard = true;
            this.grdMain7.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain7.ColumnInfo = resources.GetString("grdMain7.ColumnInfo");
            this.grdMain7.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain7.Location = new System.Drawing.Point(11, 756);
            this.grdMain7.Name = "grdMain7";
            this.grdMain7.Rows.Count = 1;
            this.grdMain7.Rows.DefaultSize = 20;
            this.grdMain7.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain7.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.AlwaysVisible;
            this.grdMain7.Size = new System.Drawing.Size(236, 174);
            this.grdMain7.StyleInfo = resources.GetString("grdMain7.StyleInfo");
            this.grdMain7.TabIndex = 123;
            // 
            // grdMain8
            // 
            this.grdMain8.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Both;
            this.grdMain8.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain8.AutoClipboard = true;
            this.grdMain8.AutoResize = true;
            this.grdMain8.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain8.ColumnInfo = resources.GetString("grdMain8.ColumnInfo");
            this.grdMain8.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain8.Location = new System.Drawing.Point(348, 732);
            this.grdMain8.Name = "grdMain8";
            this.grdMain8.Rows.Count = 1;
            this.grdMain8.Rows.DefaultSize = 20;
            this.grdMain8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain8.Size = new System.Drawing.Size(904, 58);
            this.grdMain8.StyleInfo = resources.GetString("grdMain8.StyleInfo");
            this.grdMain8.TabIndex = 126;
            // 
            // grdMain2
            // 
            this.grdMain2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain2.AutoClipboard = true;
            this.grdMain2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain2.ColumnInfo = "2,0,0,0,0,100,Columns:0{Width:93;}\t1{Width:67;}\t";
            this.grdMain2.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain2.Location = new System.Drawing.Point(264, 12);
            this.grdMain2.Name = "grdMain2";
            this.grdMain2.Rows.Count = 5;
            this.grdMain2.Rows.DefaultSize = 20;
            this.grdMain2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain2.Size = new System.Drawing.Size(163, 143);
            this.grdMain2.StyleInfo = resources.GetString("grdMain2.StyleInfo");
            this.grdMain2.TabIndex = 163;
            // 
            // grdMain1
            // 
            this.grdMain1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain1.AutoClipboard = true;
            this.grdMain1.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain1.ColumnInfo = "2,0,0,0,0,100,Columns:0{Width:123;}\t1{Width:67;}\t";
            this.grdMain1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain1.Location = new System.Drawing.Point(11, 12);
            this.grdMain1.Name = "grdMain1";
            this.grdMain1.Rows.Count = 5;
            this.grdMain1.Rows.DefaultSize = 20;
            this.grdMain1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain1.Size = new System.Drawing.Size(193, 143);
            this.grdMain1.StyleInfo = resources.GetString("grdMain1.StyleInfo");
            this.grdMain1.TabIndex = 164;
            // 
            // orientedTextLabel4
            // 
            this.orientedTextLabel4.BackColor = System.Drawing.Color.Orange;
            this.orientedTextLabel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.orientedTextLabel4.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.orientedTextLabel4.Location = new System.Drawing.Point(510, 424);
            this.orientedTextLabel4.Name = "orientedTextLabel4";
            this.orientedTextLabel4.RotationAngle = 90D;
            this.orientedTextLabel4.Size = new System.Drawing.Size(18, 57);
            this.orientedTextLabel4.TabIndex = 174;
            this.orientedTextLabel4.Text = "2 Roll";
            this.orientedTextLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.orientedTextLabel4.TextDirection = ComLib.Direction.Clockwise;
            this.orientedTextLabel4.TextOrientation = ComLib.Orientation.Rotate;
            // 
            // label85
            // 
            this.label85.BackColor = System.Drawing.Color.Orange;
            this.label85.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label85.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.label85.ForeColor = System.Drawing.Color.Black;
            this.label85.Location = new System.Drawing.Point(900, 549);
            this.label85.Margin = new System.Windows.Forms.Padding(3);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(57, 18);
            this.label85.TabIndex = 175;
            this.label85.Text = "쇼트";
            this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uC_Zone7
            // 
            this.uC_Zone7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone7.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone7.Location = new System.Drawing.Point(1043, 647);
            this.uC_Zone7.MillNo = "";
            this.uC_Zone7.Name = "uC_Zone7";
            this.uC_Zone7.PCS = "0";
            this.uC_Zone7.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone7.TabIndex = 171;
            this.uC_Zone7.ZoneCD = "3Z11";
            // 
            // uC_Zone11
            // 
            this.uC_Zone11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone11.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone11.Location = new System.Drawing.Point(1043, 590);
            this.uC_Zone11.MillNo = "";
            this.uC_Zone11.Name = "uC_Zone11";
            this.uC_Zone11.PCS = "0";
            this.uC_Zone11.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone11.TabIndex = 170;
            this.uC_Zone11.ZoneCD = "3Z08";
            // 
            // uC_Zone6
            // 
            this.uC_Zone6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone6.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone6.Location = new System.Drawing.Point(1043, 265);
            this.uC_Zone6.MillNo = "";
            this.uC_Zone6.Name = "uC_Zone6";
            this.uC_Zone6.PCS = "0";
            this.uC_Zone6.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone6.TabIndex = 172;
            this.uC_Zone6.ZoneCD = "3Z13";
            // 
            // uC_Zone8
            // 
            this.uC_Zone8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone8.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone8.Location = new System.Drawing.Point(1043, 439);
            this.uC_Zone8.MillNo = "";
            this.uC_Zone8.Name = "uC_Zone8";
            this.uC_Zone8.PCS = "0";
            this.uC_Zone8.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone8.TabIndex = 172;
            this.uC_Zone8.ZoneCD = "3Z12";
            // 
            // uC_Zone3
            // 
            this.uC_Zone3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone3.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone3.Location = new System.Drawing.Point(663, 590);
            this.uC_Zone3.MillNo = "";
            this.uC_Zone3.Name = "uC_Zone3";
            this.uC_Zone3.PCS = "0";
            this.uC_Zone3.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone3.TabIndex = 169;
            this.uC_Zone3.ZoneCD = "3Z07";
            // 
            // uC_Zone5
            // 
            this.uC_Zone5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone5.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone5.Location = new System.Drawing.Point(663, 525);
            this.uC_Zone5.MillNo = "";
            this.uC_Zone5.Name = "uC_Zone5";
            this.uC_Zone5.PCS = "0";
            this.uC_Zone5.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone5.TabIndex = 169;
            this.uC_Zone5.ZoneCD = "3Z05";
            // 
            // uC_Zone4
            // 
            this.uC_Zone4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone4.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone4.Location = new System.Drawing.Point(663, 465);
            this.uC_Zone4.MillNo = "";
            this.uC_Zone4.Name = "uC_Zone4";
            this.uC_Zone4.PCS = "0";
            this.uC_Zone4.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone4.TabIndex = 168;
            this.uC_Zone4.ZoneCD = "3Z04";
            // 
            // uC_Zone10
            // 
            this.uC_Zone10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone10.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone10.Location = new System.Drawing.Point(663, 407);
            this.uC_Zone10.MillNo = "";
            this.uC_Zone10.Name = "uC_Zone10";
            this.uC_Zone10.PCS = "0";
            this.uC_Zone10.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone10.TabIndex = 167;
            this.uC_Zone10.ZoneCD = "3Z03";
            // 
            // uC_Zone1
            // 
            this.uC_Zone1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone1.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone1.Location = new System.Drawing.Point(231, 525);
            this.uC_Zone1.MillNo = "";
            this.uC_Zone1.Name = "uC_Zone1";
            this.uC_Zone1.PCS = "0";
            this.uC_Zone1.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone1.TabIndex = 165;
            this.uC_Zone1.ZoneCD = "3Z01";
            // 
            // uC_Zone2
            // 
            this.uC_Zone2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone2.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone2.Location = new System.Drawing.Point(231, 407);
            this.uC_Zone2.MillNo = "";
            this.uC_Zone2.Name = "uC_Zone2";
            this.uC_Zone2.PCS = "0";
            this.uC_Zone2.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone2.TabIndex = 166;
            this.uC_Zone2.ZoneCD = "3Z02";
            // 
            // uC_Zone9
            // 
            this.uC_Zone9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone9.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone9.Location = new System.Drawing.Point(82, 407);
            this.uC_Zone9.MillNo = "";
            this.uC_Zone9.Name = "uC_Zone9";
            this.uC_Zone9.PCS = "0";
            this.uC_Zone9.Size = new System.Drawing.Size(84, 42);
            this.uC_Zone9.TabIndex = 167;
            this.uC_Zone9.ZoneCD = "3Z40";
            // 
            // Line3STRTrk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1257, 863);
            this.Controls.Add(this.label85);
            this.Controls.Add(this.orientedTextLabel4);
            this.Controls.Add(this.uC_Zone6);
            this.Controls.Add(this.uC_Zone8);
            this.Controls.Add(this.uC_Zone7);
            this.Controls.Add(this.uC_Zone11);
            this.Controls.Add(this.uC_Zone3);
            this.Controls.Add(this.uC_Zone5);
            this.Controls.Add(this.uC_Zone4);
            this.Controls.Add(this.uC_Zone9);
            this.Controls.Add(this.uC_Zone10);
            this.Controls.Add(this.uC_Zone2);
            this.Controls.Add(this.uC_Zone1);
            this.Controls.Add(this.grdMain2);
            this.Controls.Add(this.grdMain1);
            this.Controls.Add(this.grdMain8);
            this.Controls.Add(this.msg_lb);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.grdMain7);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Line3STRTrk";
            this.Text = "Line3STRTrk";
            this.Load += new System.EventHandler(this.Line3STRTrk_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label msg_lb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private C1.Win.C1Input.C1Button btnPOCFin;
        private C1.Win.C1Input.C1Button btnOutReg;
        private C1.Win.C1Input.C1Button btnZoneMove;
        private C1.Win.C1Input.C1Button btnMPIReg;
        private C1.Win.C1Input.C1Button btnInsertReg;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain7;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain8;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain2;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain1;
        private ComLib.OrientedTextLabel orientedTextLabel4;
        private System.Windows.Forms.Label label85;
        private WindowsFormsApplication15.UC_Zone uC_Zone7;
        private WindowsFormsApplication15.UC_Zone uC_Zone11;
        private WindowsFormsApplication15.UC_Zone uC_Zone6;
        private WindowsFormsApplication15.UC_Zone uC_Zone8;
        private WindowsFormsApplication15.UC_Zone uC_Zone3;
        private WindowsFormsApplication15.UC_Zone uC_Zone5;
        private WindowsFormsApplication15.UC_Zone uC_Zone4;
        private WindowsFormsApplication15.UC_Zone uC_Zone10;
        private WindowsFormsApplication15.UC_Zone uC_Zone1;
        private WindowsFormsApplication15.UC_Zone uC_Zone2;
        private WindowsFormsApplication15.UC_Zone uC_Zone9;
    }
}