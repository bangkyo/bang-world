namespace SystemControlClassLibrary.UC
{
    partial class UC_Line_gp_test
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Line_gp_test));
            this.cbLine_gp = new System.Windows.Forms.ComboBox();
            this.lbLine = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // cbLine_gp
            // 
            this.cbLine_gp.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbLine_gp.FormattingEnabled = true;
            this.cbLine_gp.Location = new System.Drawing.Point(69, 4);
            this.cbLine_gp.Margin = new System.Windows.Forms.Padding(4);
            this.cbLine_gp.Name = "cbLine_gp";
            this.cbLine_gp.Size = new System.Drawing.Size(84, 23);
            this.cbLine_gp.TabIndex = 49;
            this.cbLine_gp.Text = "#1라인";
            this.cbLine_gp.SelectedIndexChanged += new System.EventHandler(this.cbLine_gp_SelectedIndexChanged);
            // 
            // lbLine
            // 
            this.lbLine.AutoSize = true;
            this.lbLine.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbLine.Location = new System.Drawing.Point(20, 8);
            this.lbLine.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new System.Drawing.Size(39, 15);
            this.lbLine.TabIndex = 48;
            this.lbLine.Text = "라인";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(7, 13);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(5, 5);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 50;
            this.pictureBox2.TabStop = false;
            // 
            // UC_Line_gp_test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.cbLine_gp);
            this.Controls.Add(this.lbLine);
            this.Name = "UC_Line_gp_test";
            this.Size = new System.Drawing.Size(161, 30);
            this.Load += new System.EventHandler(this.UC_Line_gp_test_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox cbLine_gp;
        private System.Windows.Forms.Label lbLine;
    }
}
