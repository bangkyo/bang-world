namespace SystemControlClassLibrary.monitoring
{
    partial class _00TrkTemplete
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_00TrkTemplete));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.grdMain8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.msg_lb = new System.Windows.Forms.Label();
            this.grdMain7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnPOCFin = new C1.Win.C1Input.C1Button();
            this.btnZoneMove = new C1.Win.C1Input.C1Button();
            this.btnInsertReg = new C1.Win.C1Input.C1Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain7)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
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
            this.grdMain8.Location = new System.Drawing.Point(350, 730);
            this.grdMain8.Name = "grdMain8";
            this.grdMain8.Rows.Count = 1;
            this.grdMain8.Rows.DefaultSize = 20;
            this.grdMain8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdMain8.Size = new System.Drawing.Size(904, 58);
            this.grdMain8.StyleInfo = resources.GetString("grdMain8.StyleInfo");
            this.grdMain8.TabIndex = 166;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(15, 222);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1240, 499);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 170;
            this.pictureBox1.TabStop = false;
            // 
            // msg_lb
            // 
            this.msg_lb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(93)))), ((int)(((byte)(162)))));
            this.msg_lb.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.msg_lb.ForeColor = System.Drawing.Color.White;
            this.msg_lb.Location = new System.Drawing.Point(16, 732);
            this.msg_lb.Margin = new System.Windows.Forms.Padding(3);
            this.msg_lb.Name = "msg_lb";
            this.msg_lb.Size = new System.Drawing.Size(236, 21);
            this.msg_lb.TabIndex = 172;
            this.msg_lb.Text = "교정대기번들정보";
            this.msg_lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grdMain7
            // 
            this.grdMain7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdMain7.AutoClipboard = true;
            this.grdMain7.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.grdMain7.ColumnInfo = resources.GetString("grdMain7.ColumnInfo");
            this.grdMain7.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.grdMain7.Location = new System.Drawing.Point(15, 756);
            this.grdMain7.Name = "grdMain7";
            this.grdMain7.Rows.Count = 1;
            this.grdMain7.Rows.DefaultSize = 20;
            this.grdMain7.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdMain7.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.AlwaysVisible;
            this.grdMain7.Size = new System.Drawing.Size(236, 174);
            this.grdMain7.StyleInfo = resources.GetString("grdMain7.StyleInfo");
            this.grdMain7.TabIndex = 171;
            // 
            // btnPOCFin
            // 
            this.btnPOCFin.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnPOCFin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPOCFin.Location = new System.Drawing.Point(255, 825);
            this.btnPOCFin.Name = "btnPOCFin";
            this.btnPOCFin.Size = new System.Drawing.Size(90, 29);
            this.btnPOCFin.TabIndex = 175;
            this.btnPOCFin.Text = "POC 종료";
            this.btnPOCFin.UseVisualStyleBackColor = true;
            // 
            // btnZoneMove
            // 
            this.btnZoneMove.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnZoneMove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZoneMove.Location = new System.Drawing.Point(255, 790);
            this.btnZoneMove.Name = "btnZoneMove";
            this.btnZoneMove.Size = new System.Drawing.Size(90, 29);
            this.btnZoneMove.TabIndex = 174;
            this.btnZoneMove.Text = "Zone이동";
            this.btnZoneMove.UseVisualStyleBackColor = true;
            // 
            // btnInsertReg
            // 
            this.btnInsertReg.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.btnInsertReg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInsertReg.Location = new System.Drawing.Point(255, 755);
            this.btnInsertReg.Name = "btnInsertReg";
            this.btnInsertReg.Size = new System.Drawing.Size(90, 29);
            this.btnInsertReg.TabIndex = 173;
            this.btnInsertReg.Text = "투입등록";
            this.btnInsertReg.UseVisualStyleBackColor = true;
            // 
            // _00TrkTemplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1257, 863);
            this.Controls.Add(this.btnPOCFin);
            this.Controls.Add(this.btnZoneMove);
            this.Controls.Add(this.btnInsertReg);
            this.Controls.Add(this.msg_lb);
            this.Controls.Add(this.grdMain7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.grdMain8);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "_00TrkTemplete";
            this.Text = "_00TrkTemplete";
            ((System.ComponentModel.ISupportInitialize)(this.grdMain8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        protected System.Windows.Forms.PictureBox pictureBox1;
        protected C1.Win.C1FlexGrid.C1FlexGrid grdMain8;
        protected System.Windows.Forms.Label msg_lb;
        protected C1.Win.C1FlexGrid.C1FlexGrid grdMain7;
        protected C1.Win.C1Input.C1Button btnPOCFin;
        protected C1.Win.C1Input.C1Button btnZoneMove;
        protected C1.Win.C1Input.C1Button btnInsertReg;
    }
}