namespace ComLib
{
    partial class UC_ZoneMoveBtn
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
            this.btnArrow = new C1.Win.C1Input.C1Button();
            this.SuspendLayout();
            // 
            // btnArrow
            // 
            this.btnArrow.BackgroundImage = global::ComLib.Properties.Resources.if_34_61498_right;
            this.btnArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnArrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnArrow.Font = new System.Drawing.Font("돋움체", 11F, System.Drawing.FontStyle.Bold);
            this.btnArrow.Location = new System.Drawing.Point(0, 0);
            this.btnArrow.Name = "btnArrow";
            this.btnArrow.Size = new System.Drawing.Size(35, 30);
            this.btnArrow.TabIndex = 0;
            this.btnArrow.UseVisualStyleBackColor = true;
            this.btnArrow.Click += new System.EventHandler(this.btnArrow_Click);
            // 
            // UC_ZoneMoveBtn
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnArrow);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "UC_ZoneMoveBtn";
            this.Size = new System.Drawing.Size(35, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1Input.C1Button btnArrow;
    }
}
