namespace ComLib
{
    partial class UC_KeyValue_Status0
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
            this.lbUom = new System.Windows.Forms.Label();
            this.lbLimit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbValue
            // 
            this.lbValue.BackColor = System.Drawing.Color.DimGray;
            this.lbValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbValue.Font = new System.Drawing.Font("돋움체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbValue.ForeColor = System.Drawing.Color.Black;
            this.lbValue.Location = new System.Drawing.Point(3, 3);
            this.lbValue.Margin = new System.Windows.Forms.Padding(3);
            this.lbValue.Name = "lbValue";
            this.lbValue.Size = new System.Drawing.Size(26, 26);
            this.lbValue.TabIndex = 21;
            this.lbValue.Text = "P";
            this.lbValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbTitle.Font = new System.Drawing.Font("돋움체", 10F, System.Drawing.FontStyle.Bold);
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(47, 3);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(3);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(170, 26);
            this.lbTitle.TabIndex = 22;
            this.lbTitle.Text = "트랙1: (backside)";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbUom
            // 
            this.lbUom.BackColor = System.Drawing.Color.Transparent;
            this.lbUom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbUom.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold);
            this.lbUom.ForeColor = System.Drawing.Color.White;
            this.lbUom.Location = new System.Drawing.Point(153, 1);
            this.lbUom.Margin = new System.Windows.Forms.Padding(0);
            this.lbUom.Name = "lbUom";
            this.lbUom.Size = new System.Drawing.Size(10, 20);
            this.lbUom.TabIndex = 20;
            this.lbUom.Text = "km/s";
            this.lbUom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbUom.UseMnemonic = false;
            this.lbUom.Visible = false;
            // 
            // lbLimit
            // 
            this.lbLimit.BackColor = System.Drawing.Color.Transparent;
            this.lbLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbLimit.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold);
            this.lbLimit.ForeColor = System.Drawing.Color.Black;
            this.lbLimit.Location = new System.Drawing.Point(163, 1);
            this.lbLimit.Margin = new System.Windows.Forms.Padding(0);
            this.lbLimit.Name = "lbLimit";
            this.lbLimit.Size = new System.Drawing.Size(10, 20);
            this.lbLimit.TabIndex = 20;
            this.lbLimit.Text = "100~10000";
            this.lbLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLimit.UseMnemonic = false;
            this.lbLimit.Visible = false;
            // 
            // UC_KeyValue_Status0
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.lbLimit);
            this.Controls.Add(this.lbUom);
            this.Controls.Add(this.lbValue);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "UC_KeyValue_Status0";
            this.Size = new System.Drawing.Size(224, 32);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Label lbValue;
        protected System.Windows.Forms.Label lbTitle;
        protected System.Windows.Forms.Label lbUom;
        protected System.Windows.Forms.Label lbLimit;
    }
}
