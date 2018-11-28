namespace ComLib
{
    partial class UC_KeyValue_S0
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
            this.lbValue = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbLimit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbValue
            // 
            this.lbValue.BackColor = System.Drawing.Color.Black;
            this.lbValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbValue.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbValue.ForeColor = System.Drawing.Color.White;
            this.lbValue.Location = new System.Drawing.Point(2, 32);
            this.lbValue.Margin = new System.Windows.Forms.Padding(3);
            this.lbValue.Name = "lbValue";
            this.lbValue.Size = new System.Drawing.Size(123, 30);
            this.lbValue.TabIndex = 21;
            this.lbValue.Text = "99";
            this.lbValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lbTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbTitle.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold);
            this.lbTitle.ForeColor = System.Drawing.Color.White;
            this.lbTitle.Location = new System.Drawing.Point(2, 1);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(3);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(123, 30);
            this.lbTitle.TabIndex = 22;
            this.lbTitle.Text = "연마지석 사용";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLimit
            // 
            this.lbLimit.BackColor = System.Drawing.Color.Silver;
            this.lbLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbLimit.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold);
            this.lbLimit.ForeColor = System.Drawing.Color.Black;
            this.lbLimit.Location = new System.Drawing.Point(123, 1);
            this.lbLimit.Margin = new System.Windows.Forms.Padding(0);
            this.lbLimit.Name = "lbLimit";
            this.lbLimit.Size = new System.Drawing.Size(0, 30);
            this.lbLimit.TabIndex = 20;
            this.lbLimit.Text = "100~10000";
            this.lbLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLimit.UseMnemonic = false;
            // 
            // UC_KeyValue_S0
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lbLimit);
            this.Controls.Add(this.lbValue);
            this.Controls.Add(this.lbTitle);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "UC_KeyValue_S0";
            this.Size = new System.Drawing.Size(127, 64);
            this.Resize += new System.EventHandler(this.UC_KeyValue_S0_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Label lbValue;
        protected System.Windows.Forms.Label lbTitle;
        protected System.Windows.Forms.Label lbLimit;
    }
}
