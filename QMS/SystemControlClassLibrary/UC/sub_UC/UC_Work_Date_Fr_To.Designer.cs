namespace SystemControlClassLibrary.UC.sub_UC
{
    partial class UC_Work_Date_Fr_To
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
            this.dtpWorkFr = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpWorkTo = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            this.SuspendLayout();
            // 
            // c1Label1
            // 
            this.c1Label1.Size = new System.Drawing.Size(72, 19);
            this.c1Label1.Text = "작업시각";
            // 
            // dtpWorkFr
            // 
            this.dtpWorkFr.CustomFormat = "yyyy-MM-dd  HH";
            this.dtpWorkFr.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpWorkFr.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpWorkFr.Location = new System.Drawing.Point(96, 2);
            this.dtpWorkFr.Name = "dtpWorkFr";
            this.dtpWorkFr.Size = new System.Drawing.Size(157, 24);
            this.dtpWorkFr.TabIndex = 90;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(257, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 15);
            this.label2.TabIndex = 92;
            this.label2.Text = "~";
            // 
            // dtpWorkTo
            // 
            this.dtpWorkTo.CustomFormat = "yyyy-MM-dd  HH";
            this.dtpWorkTo.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpWorkTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpWorkTo.Location = new System.Drawing.Point(279, 2);
            this.dtpWorkTo.Name = "dtpWorkTo";
            this.dtpWorkTo.Size = new System.Drawing.Size(157, 24);
            this.dtpWorkTo.TabIndex = 90;
            // 
            // UC_Work_Date_Fr_To
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpWorkTo);
            this.Controls.Add(this.dtpWorkFr);
            this.Name = "UC_Work_Date_Fr_To";
            this.Size = new System.Drawing.Size(450, 27);
            this.Load += new System.EventHandler(this.UC_Work_Date_Fr_To_Load);
            this.Controls.SetChildIndex(this.dtpWorkFr, 0);
            this.Controls.SetChildIndex(this.dtpWorkTo, 0);
            this.Controls.SetChildIndex(this.c1Label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpWorkFr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpWorkTo;
    }
}
