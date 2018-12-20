namespace SystemControlClassLibrary.monitoring
{
    partial class Line1WholeTrk
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Line1WholeTrk));
            this.btnPOCfin = new C1.Win.C1Input.C1Button();
            this.btnZoneMove = new C1.Win.C1Input.C1Button();
            this.btnInsertReg = new C1.Win.C1Input.C1Button();
            this.grdMain7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain10 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain9 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain6 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain3 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.grdMain1_1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdMain1_2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRsltCancel = new C1.Win.C1Input.C1Button();
            this.btnReWork = new C1.Win.C1Input.C1Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDisplay = new C1.Win.C1Input.C1Button();
            this.orientedTextLabel5 = new ComLib.OrientedTextLabel();
            this.orientedTextLabel4 = new ComLib.OrientedTextLabel();
            this.orientedTextLabel3 = new ComLib.OrientedTextLabel();
            this.orientedTextLabel2 = new ComLib.OrientedTextLabel();
            this.uC_Zone13 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone14 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone7 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone5 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone4 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone3 = new WindowsFormsApplication15.UC_Zone();
            this.uC_Zone1 = new WindowsFormsApplication15.UC_Zone();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnZ03Fin = new C1.Win.C1Input.C1Button();
            this.uC_Zone2 = new WindowsFormsApplication15.UC_Zone();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.uC_Zone6 = new WindowsFormsApplication15.UC_Zone();
            this.grdMain12 = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain12)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPOCfin
            // 
            this.btnPOCfin.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnPOCfin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPOCfin.Location = new System.Drawing.Point(253, 741);
            this.btnPOCfin.Name = "btnPOCfin";
            this.btnPOCfin.Size = new System.Drawing.Size(90, 30);
            this.btnPOCfin.TabIndex = 181;
            this.btnPOCfin.Tag = "";
            this.btnPOCfin.Text = "POC 종료";
            this.btnPOCfin.UseVisualStyleBackColor = true;
            this.btnPOCfin.Click += new System.EventHandler(this.btnPOCFin_Click);
            // 
            // btnZoneMove
            // 
            this.btnZoneMove.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnZoneMove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZoneMove.Location = new System.Drawing.Point(253, 699);
            this.btnZoneMove.Name = "btnZoneMove";
            this.btnZoneMove.Size = new System.Drawing.Size(90, 30);
            this.btnZoneMove.TabIndex = 180;
            this.btnZoneMove.Tag = "";
            this.btnZoneMove.Text = "ZONE이동";
            this.btnZoneMove.UseVisualStyleBackColor = true;
            this.btnZoneMove.Click += new System.EventHandler(this.btnZoneMove_Click);
            // 
            // btnInsertReg
            // 
            this.btnInsertReg.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnInsertReg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInsertReg.Location = new System.Drawing.Point(253, 657);
            this.btnInsertReg.Name = "btnInsertReg";
            this.btnInsertReg.Size = new System.Drawing.Size(90, 30);
            this.btnInsertReg.TabIndex = 179;
            this.btnInsertReg.Tag = "";
            this.btnInsertReg.Text = "투입등록";
            this.btnInsertReg.UseVisualStyleBackColor = true;
            this.btnInsertReg.Click += new System.EventHandler(this.btnInsertReg_Click);
            // 
            // grdMain7
            // 
            this.grdMain7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain7.AutoClipboard = true;
            this.grdMain7.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain7.ColumnInfo = "3,0,0,0,0,100,Columns:0{Width:88;Name:\"MILL_NO\";Style:\"TextAlign:RightCenter;\";}\t" +
    "1{Width:61;Name:\"MILL_PCS\";Style:\"TextAlign:RightCenter;\";}\t2{Width:67;Name:\"STR" +
    "_PCS\";Style:\"TextAlign:RightCenter;\";}\t";
            this.grdMain7.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain7.Location = new System.Drawing.Point(12, 658);
            this.grdMain7.Name = "grdMain7";
            this.grdMain7.Rows.Count = 1;
            this.grdMain7.Rows.DefaultSize = 20;
            this.grdMain7.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain7.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.AlwaysVisible;
            this.grdMain7.Size = new System.Drawing.Size(235, 199);
            this.grdMain7.StyleInfo = resources.GetString("grdMain7.StyleInfo");
            this.grdMain7.TabIndex = 177;
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
            this.grdMain8.Location = new System.Drawing.Point(348, 657);
            this.grdMain8.Name = "grdMain8";
            this.grdMain8.Rows.Count = 1;
            this.grdMain8.Rows.DefaultSize = 20;
            this.grdMain8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain8.Size = new System.Drawing.Size(904, 52);
            this.grdMain8.StyleInfo = resources.GetString("grdMain8.StyleInfo");
            this.grdMain8.TabIndex = 176;
            this.grdMain8.DoubleClick += new System.EventHandler(this.grdMain8_DoubleClick);
            // 
            // grdMain10
            // 
            this.grdMain10.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain10.AutoClipboard = true;
            this.grdMain10.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain10.ColumnInfo = resources.GetString("grdMain10.ColumnInfo");
            this.grdMain10.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain10.Location = new System.Drawing.Point(956, 727);
            this.grdMain10.Name = "grdMain10";
            this.grdMain10.Rows.Count = 1;
            this.grdMain10.Rows.DefaultSize = 20;
            this.grdMain10.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain10.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.AlwaysVisible;
            this.grdMain10.Size = new System.Drawing.Size(296, 130);
            this.grdMain10.StyleInfo = resources.GetString("grdMain10.StyleInfo");
            this.grdMain10.TabIndex = 183;
            this.grdMain10.Visible = false;
            // 
            // grdMain9
            // 
            this.grdMain9.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain9.AutoClipboard = true;
            this.grdMain9.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain9.ColumnInfo = resources.GetString("grdMain9.ColumnInfo");
            this.grdMain9.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain9.Location = new System.Drawing.Point(348, 727);
            this.grdMain9.Name = "grdMain9";
            this.grdMain9.Rows.Count = 2;
            this.grdMain9.Rows.DefaultSize = 20;
            this.grdMain9.Rows.Fixed = 2;
            this.grdMain9.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain9.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.AlwaysVisible;
            this.grdMain9.Size = new System.Drawing.Size(527, 130);
            this.grdMain9.StyleInfo = resources.GetString("grdMain9.StyleInfo");
            this.grdMain9.TabIndex = 182;
            // 
            // grdMain6
            // 
            this.grdMain6.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain6.AutoClipboard = true;
            this.grdMain6.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain6.ColumnInfo = "2,0,0,0,0,100,Columns:0{Width:113;Name:\"BUNDLE_NO\";Caption:\"제품번들번호\";}\t1{Width:52;" +
    "Name:\"PCS\";Caption:\"본수\";}\t";
            this.grdMain6.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain6.Location = new System.Drawing.Point(1064, 5);
            this.grdMain6.Name = "grdMain6";
            this.grdMain6.Rows.Count = 2;
            this.grdMain6.Rows.DefaultSize = 20;
            this.grdMain6.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain6.Size = new System.Drawing.Size(168, 54);
            this.grdMain6.StyleInfo = resources.GetString("grdMain6.StyleInfo");
            this.grdMain6.TabIndex = 184;
            this.grdMain6.Visible = false;
            // 
            // grdMain3
            // 
            this.grdMain3.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain3.AutoClipboard = true;
            this.grdMain3.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain3.ColumnInfo = resources.GetString("grdMain3.ColumnInfo");
            this.grdMain3.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain3.Location = new System.Drawing.Point(795, 5);
            this.grdMain3.Name = "grdMain3";
            this.grdMain3.Rows.Count = 4;
            this.grdMain3.Rows.DefaultSize = 20;
            this.grdMain3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain3.Size = new System.Drawing.Size(203, 103);
            this.grdMain3.StyleInfo = resources.GetString("grdMain3.StyleInfo");
            this.grdMain3.TabIndex = 185;
            // 
            // grdMain2
            // 
            this.grdMain2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain2.AutoClipboard = true;
            this.grdMain2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain2.ColumnInfo = resources.GetString("grdMain2.ColumnInfo");
            this.grdMain2.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain2.Location = new System.Drawing.Point(491, 5);
            this.grdMain2.Name = "grdMain2";
            this.grdMain2.Rows.Count = 5;
            this.grdMain2.Rows.DefaultSize = 20;
            this.grdMain2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain2.Size = new System.Drawing.Size(232, 130);
            this.grdMain2.StyleInfo = resources.GetString("grdMain2.StyleInfo");
            this.grdMain2.TabIndex = 186;
            // 
            // grdMain1
            // 
            this.grdMain1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain1.AutoClipboard = true;
            this.grdMain1.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain1.ColumnInfo = resources.GetString("grdMain1.ColumnInfo");
            this.grdMain1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain1.Location = new System.Drawing.Point(6, 5);
            this.grdMain1.Name = "grdMain1";
            this.grdMain1.Rows.Count = 5;
            this.grdMain1.Rows.DefaultSize = 20;
            this.grdMain1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain1.Size = new System.Drawing.Size(423, 130);
            this.grdMain1.StyleInfo = resources.GetString("grdMain1.StyleInfo");
            this.grdMain1.TabIndex = 187;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // grdMain1_1
            // 
            this.grdMain1_1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain1_1.AutoClipboard = true;
            this.grdMain1_1.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain1_1.ColumnInfo = "2,1,0,0,0,100,Columns:0{Width:120;Style:\"TextAlign:CenterCenter;\";StyleFixed:\"Tex" +
    "tAlign:LeftCenter;\";}\t1{Width:50;Style:\"TextAlign:RightCenter;\";StyleFixed:\"Text" +
    "Align:RightCenter;\";}\t";
            this.grdMain1_1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain1_1.Location = new System.Drawing.Point(6, 142);
            this.grdMain1_1.Name = "grdMain1_1";
            this.grdMain1_1.Rows.Count = 1;
            this.grdMain1_1.Rows.DefaultSize = 20;
            this.grdMain1_1.Rows.Fixed = 0;
            this.grdMain1_1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain1_1.Size = new System.Drawing.Size(170, 23);
            this.grdMain1_1.StyleInfo = resources.GetString("grdMain1_1.StyleInfo");
            this.grdMain1_1.TabIndex = 185;
            // 
            // grdMain1_2
            // 
            this.grdMain1_2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain1_2.AutoClipboard = true;
            this.grdMain1_2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain1_2.ColumnInfo = "2,1,0,0,0,100,Columns:0{Width:120;Style:\"TextAlign:LeftCenter;\";StyleFixed:\"TextA" +
    "lign:LeftCenter;\";}\t1{Width:50;Style:\"TextAlign:RightCenter;\";StyleFixed:\"TextAl" +
    "ign:RightCenter;\";}\t";
            this.grdMain1_2.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain1_2.Location = new System.Drawing.Point(6, 163);
            this.grdMain1_2.Name = "grdMain1_2";
            this.grdMain1_2.Rows.Count = 1;
            this.grdMain1_2.Rows.DefaultSize = 20;
            this.grdMain1_2.Rows.Fixed = 0;
            this.grdMain1_2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain1_2.Size = new System.Drawing.Size(170, 22);
            this.grdMain1_2.StyleInfo = resources.GetString("grdMain1_2.StyleInfo");
            this.grdMain1_2.TabIndex = 185;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 153);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1240, 499);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 171;
            this.pictureBox1.TabStop = false;
            // 
            // btnRsltCancel
            // 
            this.btnRsltCancel.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnRsltCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRsltCancel.Location = new System.Drawing.Point(253, 827);
            this.btnRsltCancel.Name = "btnRsltCancel";
            this.btnRsltCancel.Size = new System.Drawing.Size(90, 30);
            this.btnRsltCancel.TabIndex = 216;
            this.btnRsltCancel.Tag = "";
            this.btnRsltCancel.Text = "작업취소";
            this.btnRsltCancel.UseVisualStyleBackColor = true;
            this.btnRsltCancel.Click += new System.EventHandler(this.btnRsltCancel_Click);
            // 
            // btnReWork
            // 
            this.btnReWork.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnReWork.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReWork.Location = new System.Drawing.Point(253, 783);
            this.btnReWork.Name = "btnReWork";
            this.btnReWork.Size = new System.Drawing.Size(90, 30);
            this.btnReWork.TabIndex = 217;
            this.btnReWork.Tag = "";
            this.btnReWork.Text = "재작업관리";
            this.btnReWork.UseVisualStyleBackColor = true;
            this.btnReWork.Click += new System.EventHandler(this.btnReWork_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.grdMain1);
            this.panel1.Controls.Add(this.grdMain1_2);
            this.panel1.Controls.Add(this.grdMain1_1);
            this.panel1.Controls.Add(this.grdMain2);
            this.panel1.Controls.Add(this.grdMain3);
            this.panel1.Controls.Add(this.grdMain6);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1240, 194);
            this.panel1.TabIndex = 218;
            // 
            // btnDisplay
            // 
            this.btnDisplay.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDisplay.Location = new System.Drawing.Point(1142, 104);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(90, 30);
            this.btnDisplay.TabIndex = 188;
            this.btnDisplay.Text = "조회";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // orientedTextLabel5
            // 
            this.orientedTextLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.orientedTextLabel5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.orientedTextLabel5.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.orientedTextLabel5.Location = new System.Drawing.Point(179, 289);
            this.orientedTextLabel5.Name = "orientedTextLabel5";
            this.orientedTextLabel5.RotationAngle = 90D;
            this.orientedTextLabel5.Size = new System.Drawing.Size(18, 57);
            this.orientedTextLabel5.TabIndex = 215;
            this.orientedTextLabel5.Text = "9 Roll";
            this.orientedTextLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.orientedTextLabel5.TextDirection = ComLib.Direction.Clockwise;
            this.orientedTextLabel5.TextOrientation = ComLib.Orientation.Rotate;
            this.orientedTextLabel5.Click += new System.EventHandler(this.orientedTextLabel5_Click);
            // 
            // orientedTextLabel4
            // 
            this.orientedTextLabel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.orientedTextLabel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.orientedTextLabel4.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.orientedTextLabel4.Location = new System.Drawing.Point(772, 494);
            this.orientedTextLabel4.Name = "orientedTextLabel4";
            this.orientedTextLabel4.RotationAngle = 90D;
            this.orientedTextLabel4.Size = new System.Drawing.Size(18, 57);
            this.orientedTextLabel4.TabIndex = 193;
            this.orientedTextLabel4.Text = "UT";
            this.orientedTextLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.orientedTextLabel4.TextDirection = ComLib.Direction.Clockwise;
            this.orientedTextLabel4.TextOrientation = ComLib.Orientation.Rotate;
            // 
            // orientedTextLabel3
            // 
            this.orientedTextLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.orientedTextLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.orientedTextLabel3.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.orientedTextLabel3.Location = new System.Drawing.Point(752, 494);
            this.orientedTextLabel3.Name = "orientedTextLabel3";
            this.orientedTextLabel3.RotationAngle = 90D;
            this.orientedTextLabel3.Size = new System.Drawing.Size(18, 57);
            this.orientedTextLabel3.TabIndex = 192;
            this.orientedTextLabel3.Text = "MLFT";
            this.orientedTextLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.orientedTextLabel3.TextDirection = ComLib.Direction.Clockwise;
            this.orientedTextLabel3.TextOrientation = ComLib.Orientation.Rotate;
            // 
            // orientedTextLabel2
            // 
            this.orientedTextLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.orientedTextLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.orientedTextLabel2.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.orientedTextLabel2.Location = new System.Drawing.Point(732, 494);
            this.orientedTextLabel2.Name = "orientedTextLabel2";
            this.orientedTextLabel2.RotationAngle = 90D;
            this.orientedTextLabel2.Size = new System.Drawing.Size(18, 57);
            this.orientedTextLabel2.TabIndex = 191;
            this.orientedTextLabel2.Text = "MAT";
            this.orientedTextLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.orientedTextLabel2.TextDirection = ComLib.Direction.Clockwise;
            this.orientedTextLabel2.TextOrientation = ComLib.Orientation.Rotate;
            // 
            // uC_Zone13
            // 
            this.uC_Zone13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone13.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone13.Location = new System.Drawing.Point(1135, 523);
            this.uC_Zone13.MillNo = "";
            this.uC_Zone13.Name = "uC_Zone13";
            this.uC_Zone13.PCS = "0";
            this.uC_Zone13.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone13.TabIndex = 189;
            this.uC_Zone13.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone13.ZoneCD = "1Z41";
            this.uC_Zone13.ZoneForeColor = System.Drawing.Color.White;
            // 
            // uC_Zone14
            // 
            this.uC_Zone14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone14.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone14.Location = new System.Drawing.Point(891, 505);
            this.uC_Zone14.MillNo = "";
            this.uC_Zone14.Name = "uC_Zone14";
            this.uC_Zone14.PCS = "0";
            this.uC_Zone14.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone14.TabIndex = 189;
            this.uC_Zone14.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone14.ZoneCD = "1Z14";
            this.uC_Zone14.ZoneForeColor = System.Drawing.Color.White;
            // 
            // uC_Zone7
            // 
            this.uC_Zone7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone7.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone7.Location = new System.Drawing.Point(539, 457);
            this.uC_Zone7.MillNo = "";
            this.uC_Zone7.Name = "uC_Zone7";
            this.uC_Zone7.PCS = "0";
            this.uC_Zone7.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone7.TabIndex = 189;
            this.uC_Zone7.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone7.ZoneCD = "1Z11";
            this.uC_Zone7.ZoneForeColor = System.Drawing.Color.White;
            // 
            // uC_Zone5
            // 
            this.uC_Zone5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone5.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone5.Location = new System.Drawing.Point(251, 457);
            this.uC_Zone5.MillNo = "";
            this.uC_Zone5.Name = "uC_Zone5";
            this.uC_Zone5.PCS = "0";
            this.uC_Zone5.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone5.TabIndex = 189;
            this.uC_Zone5.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone5.ZoneCD = "1Z05";
            this.uC_Zone5.ZoneForeColor = System.Drawing.Color.White;
            // 
            // uC_Zone4
            // 
            this.uC_Zone4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone4.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone4.Location = new System.Drawing.Point(251, 381);
            this.uC_Zone4.MillNo = "";
            this.uC_Zone4.Name = "uC_Zone4";
            this.uC_Zone4.PCS = "0";
            this.uC_Zone4.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone4.TabIndex = 189;
            this.uC_Zone4.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone4.ZoneCD = "1Z04";
            this.uC_Zone4.ZoneForeColor = System.Drawing.Color.White;
            // 
            // uC_Zone3
            // 
            this.uC_Zone3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone3.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone3.Location = new System.Drawing.Point(251, 294);
            this.uC_Zone3.MillNo = "";
            this.uC_Zone3.Name = "uC_Zone3";
            this.uC_Zone3.PCS = "0";
            this.uC_Zone3.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone3.TabIndex = 189;
            this.uC_Zone3.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone3.ZoneCD = "1Z03";
            this.uC_Zone3.ZoneForeColor = System.Drawing.Color.White;
            // 
            // uC_Zone1
            // 
            this.uC_Zone1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone1.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone1.Location = new System.Drawing.Point(35, 375);
            this.uC_Zone1.MillNo = "";
            this.uC_Zone1.Name = "uC_Zone1";
            this.uC_Zone1.PCS = "0";
            this.uC_Zone1.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone1.TabIndex = 189;
            this.uC_Zone1.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone1.ZoneCD = "1Z01";
            this.uC_Zone1.ZoneForeColor = System.Drawing.Color.White;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(251, 426);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 18);
            this.label2.TabIndex = 220;
            this.label2.Text = "면취";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(426, 513);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 18);
            this.label1.TabIndex = 221;
            this.label1.Text = "쇼트";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(1133, 505);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 18);
            this.label3.TabIndex = 222;
            this.label3.Text = "재작업존";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnZ03Fin
            // 
            this.btnZ03Fin.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnZ03Fin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZ03Fin.Location = new System.Drawing.Point(1130, 592);
            this.btnZ03Fin.Name = "btnZ03Fin";
            this.btnZ03Fin.Size = new System.Drawing.Size(90, 30);
            this.btnZ03Fin.TabIndex = 223;
            this.btnZ03Fin.Tag = "";
            this.btnZ03Fin.Text = "Z03이후종료";
            this.btnZ03Fin.UseVisualStyleBackColor = true;
            this.btnZ03Fin.Click += new System.EventHandler(this.btnZ03Fin_Click);
            // 
            // uC_Zone2
            // 
            this.uC_Zone2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone2.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone2.Location = new System.Drawing.Point(1135, 426);
            this.uC_Zone2.MillNo = "";
            this.uC_Zone2.Name = "uC_Zone2";
            this.uC_Zone2.PCS = "0";
            this.uC_Zone2.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone2.TabIndex = 224;
            this.uC_Zone2.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone2.ZoneCD = "1Z42";
            this.uC_Zone2.ZoneForeColor = System.Drawing.Color.White;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(1133, 405);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 18);
            this.label4.TabIndex = 225;
            this.label4.Text = "결속격외";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Crimson;
            this.label5.Location = new System.Drawing.Point(353, 212);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(553, 26);
            this.label5.TabIndex = 226;
            this.label5.Text = "POC ND(SAE51B35) 강종은 난단척 주의 강종입니다";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uC_Zone6
            // 
            this.uC_Zone6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.uC_Zone6.Cursor = System.Windows.Forms.Cursors.Default;
            this.uC_Zone6.Location = new System.Drawing.Point(891, 402);
            this.uC_Zone6.MillNo = "";
            this.uC_Zone6.Name = "uC_Zone6";
            this.uC_Zone6.PCS = "0";
            this.uC_Zone6.Size = new System.Drawing.Size(85, 42);
            this.uC_Zone6.TabIndex = 227;
            this.uC_Zone6.ZoneBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.uC_Zone6.ZoneCD = "1Z15";
            this.uC_Zone6.ZoneForeColor = System.Drawing.Color.White;
            // 
            // grdMain12
            // 
            this.grdMain12.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain12.AutoClipboard = true;
            this.grdMain12.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain12.ColumnInfo = "3,0,0,0,0,100,Columns:0{Width:79;Name:\"POC\";Style:\"TextAlign:RightCenter;\";}\t1{Wi" +
    "dth:67;Name:\"QTY\";Style:\"TextAlign:RightCenter;\";}\t2{Width:60;Name:\"CO\";Style:\"T" +
    "extAlign:RightCenter;\";}\t";
            this.grdMain12.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain12.Location = new System.Drawing.Point(12, 602);
            this.grdMain12.Name = "grdMain12";
            this.grdMain12.Rows.Count = 1;
            this.grdMain12.Rows.DefaultSize = 20;
            this.grdMain12.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain12.Size = new System.Drawing.Size(211, 50);
            this.grdMain12.StyleInfo = resources.GetString("grdMain12.StyleInfo");
            this.grdMain12.TabIndex = 287;
            // 
            // Line1WholeTrk
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1257, 863);
            this.Controls.Add(this.grdMain12);
            this.Controls.Add(this.uC_Zone6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.uC_Zone2);
            this.Controls.Add(this.btnZ03Fin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRsltCancel);
            this.Controls.Add(this.btnReWork);
            this.Controls.Add(this.orientedTextLabel5);
            this.Controls.Add(this.orientedTextLabel4);
            this.Controls.Add(this.orientedTextLabel3);
            this.Controls.Add(this.orientedTextLabel2);
            this.Controls.Add(this.uC_Zone13);
            this.Controls.Add(this.uC_Zone14);
            this.Controls.Add(this.uC_Zone7);
            this.Controls.Add(this.uC_Zone5);
            this.Controls.Add(this.uC_Zone4);
            this.Controls.Add(this.uC_Zone3);
            this.Controls.Add(this.uC_Zone1);
            this.Controls.Add(this.grdMain10);
            this.Controls.Add(this.grdMain9);
            this.Controls.Add(this.btnPOCfin);
            this.Controls.Add(this.btnZoneMove);
            this.Controls.Add(this.btnInsertReg);
            this.Controls.Add(this.grdMain7);
            this.Controls.Add(this.grdMain8);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Line1WholeTrk";
            this.Text = "#1실시간트래킹";
            this.Load += new System.EventHandler(this.Line1WholeTrk_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain1_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain12)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected C1.Win.C1Input.C1Button btnPOCfin;
        protected C1.Win.C1Input.C1Button btnZoneMove;
        protected C1.Win.C1Input.C1Button btnInsertReg;
        protected C1.Win.C1FlexGrid.C1FlexGrid grdMain7;
        protected C1.Win.C1FlexGrid.C1FlexGrid grdMain8;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain10;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain9;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain6;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain3;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain2;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private WindowsFormsApplication15.UC_Zone uC_Zone1;
        private WindowsFormsApplication15.UC_Zone uC_Zone3;
        private WindowsFormsApplication15.UC_Zone uC_Zone4;
        private WindowsFormsApplication15.UC_Zone uC_Zone5;
        private WindowsFormsApplication15.UC_Zone uC_Zone7;
        private ComLib.OrientedTextLabel orientedTextLabel4;
        private ComLib.OrientedTextLabel orientedTextLabel3;
        private ComLib.OrientedTextLabel orientedTextLabel2;
        private ComLib.OrientedTextLabel orientedTextLabel5;
        private System.Windows.Forms.Timer timer1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain1_1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdMain1_2;
        private WindowsFormsApplication15.UC_Zone uC_Zone14;
        private C1.Win.C1Input.C1Button btnRsltCancel;
        private C1.Win.C1Input.C1Button btnReWork;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1Input.C1Button btnDisplay;
        private WindowsFormsApplication15.UC_Zone uC_Zone13;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label label3;
        protected C1.Win.C1Input.C1Button btnZ03Fin;
        private WindowsFormsApplication15.UC_Zone uC_Zone2;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.Label label5;
        private WindowsFormsApplication15.UC_Zone uC_Zone6;
        protected C1.Win.C1FlexGrid.C1FlexGrid grdMain12;
    }
}
