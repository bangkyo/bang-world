namespace SystemControlClassLibrary.UC
{
    partial class UC_LENGTH
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
            ((System.ComponentModel.ISupportInitialize)(this.tbItemSize)).BeginInit();
            this.SuspendLayout();
            // 
            // lbLine
            // 
            this.lbLine.Size = new System.Drawing.Size(65, 15);
            this.lbLine.Text = "길이(m)";
            // 
            // tbItemSize
            // 
            this.tbItemSize.Location = new System.Drawing.Point(96, 3);
            this.tbItemSize.TextChanged += new System.EventHandler(this.tbItemSize_TextChanged);
            // 
            // UC_LENGTH
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ITEM_SIZE = "tbItemSize";
            this.Name = "UC_LENGTH";
            this.Size = new System.Drawing.Size(181, 30);
            this.Load += new System.EventHandler(this.UC_LENGTH_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbItemSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
