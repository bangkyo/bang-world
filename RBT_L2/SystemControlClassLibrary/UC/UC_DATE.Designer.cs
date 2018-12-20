namespace SystemControlClassLibrary.UC
{
    partial class UC_DATE
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_DATE));
            this.lbLine = new System.Windows.Forms.Label();
            this.dt_dt = new System.Windows.Forms.DateTimePicker();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lbLine
            // 
            this.lbLine.AutoSize = true;
            this.lbLine.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbLine.Location = new System.Drawing.Point(22, 8);
            this.lbLine.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new System.Drawing.Size(103, 15);
            this.lbLine.TabIndex = 51;
            this.lbLine.Text = "투입계획일자";
            // 
            // dt_dt
            // 
            this.dt_dt.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dt_dt.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dt_dt.Location = new System.Drawing.Point(134, 3);
            this.dt_dt.Name = "dt_dt";
            this.dt_dt.Size = new System.Drawing.Size(142, 24);
            this.dt_dt.TabIndex = 54;
            this.dt_dt.ValueChanged += new System.EventHandler(this.dt_dt_ValueChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(11, 13);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(5, 5);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 53;
            this.pictureBox2.TabStop = false;
            // 
            // UC_DATE
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dt_dt);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lbLine);
            this.Name = "UC_DATE";
            this.Size = new System.Drawing.Size(285, 30);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        protected System.Windows.Forms.Label lbLine;
        private System.Windows.Forms.DateTimePicker dt_dt;
    }
}
