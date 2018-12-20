namespace SystemControlClassLibrary.UC.sub_UC
{
    partial class UC_Length
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
            this.tbLength = new C1.Win.C1Input.C1TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLength)).BeginInit();
            this.SuspendLayout();
            // 
            // c1Label1
            // 
            this.c1Label1.Size = new System.Drawing.Size(67, 19);
            this.c1Label1.Text = "길이(m)";
            // 
            // tbLength
            // 
            this.tbLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbLength.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbLength.Location = new System.Drawing.Point(120, 2);
            this.tbLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbLength.Name = "tbLength";
            this.tbLength.Size = new System.Drawing.Size(142, 24);
            this.tbLength.TabIndex = 90;
            this.tbLength.TabStop = false;
            this.tbLength.Tag = null;
            this.tbLength.TextDetached = true;
            this.tbLength.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            this.tbLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLength_KeyPress);
            // 
            // UC_Length
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tbLength);
            this.Name = "UC_Length";
            this.Controls.SetChildIndex(this.c1Label1, 0);
            this.Controls.SetChildIndex(this.tbLength, 0);
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1Input.C1TextBox tbLength;
    }
}
