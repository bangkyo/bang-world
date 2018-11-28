namespace ComLib
{
    partial class UC_Zone_B
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
            this.lbZone = new System.Windows.Forms.Label();
            this.lb_BloomNo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbZone
            // 
            this.lbZone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lbZone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbZone.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold);
            this.lbZone.ForeColor = System.Drawing.Color.White;
            this.lbZone.Location = new System.Drawing.Point(2, 2);
            this.lbZone.Margin = new System.Windows.Forms.Padding(3);
            this.lbZone.Name = "lbZone";
            this.lbZone.Size = new System.Drawing.Size(41, 20);
            this.lbZone.TabIndex = 22;
            this.lbZone.Text = "Z01";
            this.lbZone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbZone.DoubleClick += new System.EventHandler(this.lbZone_DoubleClick);
            // 
            // lb_BloomNo
            // 
            this.lb_BloomNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.lb_BloomNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lb_BloomNo.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_BloomNo.ForeColor = System.Drawing.Color.Black;
            this.lb_BloomNo.Location = new System.Drawing.Point(45, 2);
            this.lb_BloomNo.Margin = new System.Windows.Forms.Padding(0);
            this.lb_BloomNo.Name = "lb_BloomNo";
            this.lb_BloomNo.Size = new System.Drawing.Size(137, 20);
            this.lb_BloomNo.TabIndex = 20;
            this.lb_BloomNo.Text = "4D4568-2";
            this.lb_BloomNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_BloomNo.UseMnemonic = false;
            this.lb_BloomNo.Click += new System.EventHandler(this.lbBundleNo_Click);
            this.lb_BloomNo.DoubleClick += new System.EventHandler(this.lbBundleNo_DoubleClick);
            // 
            // UC_Zone_B
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lb_BloomNo);
            this.Controls.Add(this.lbZone);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "UC_Zone_B";
            this.Size = new System.Drawing.Size(184, 24);
            this.Click += new System.EventHandler(this.UC_Zone_Click);
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.Label lbZone;
        protected System.Windows.Forms.Label lb_BloomNo;
    }
}
