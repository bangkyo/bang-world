namespace SystemControlClassLibrary.UC.sub_UC
{
    partial class UC_Ling_gp_sc
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
            this.cbLine = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            this.SuspendLayout();
            // 
            // c1Label1
            // 
            this.c1Label1.Size = new System.Drawing.Size(42, 19);
            this.c1Label1.Text = "라인";
            // 
            // cbLine
            // 
            this.cbLine.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Location = new System.Drawing.Point(53, 2);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(66, 23);
            this.cbLine.TabIndex = 91;
            // 
            // UC_Ling_gp_sc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbLine);
            this.Name = "UC_Ling_gp_sc";
            this.Size = new System.Drawing.Size(140, 27);
            this.Controls.SetChildIndex(this.c1Label1, 0);
            this.Controls.SetChildIndex(this.cbLine, 0);
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.ComboBox cbLine;
    }
}
