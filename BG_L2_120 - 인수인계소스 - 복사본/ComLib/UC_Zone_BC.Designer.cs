namespace ComLib
{
    partial class UC_Zone_BC
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
            this.lbBloomCnt = new System.Windows.Forms.Label();
            this.lbZone = new System.Windows.Forms.Label();
            this.lbBloomNo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbBloomCnt
            // 
            this.lbBloomCnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.lbBloomCnt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBloomCnt.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold);
            this.lbBloomCnt.ForeColor = System.Drawing.Color.Black;
            this.lbBloomCnt.Location = new System.Drawing.Point(45, 2);
            this.lbBloomCnt.Margin = new System.Windows.Forms.Padding(3);
            this.lbBloomCnt.Name = "lbBloomCnt";
            this.lbBloomCnt.Size = new System.Drawing.Size(137, 20);
            this.lbBloomCnt.TabIndex = 21;
            this.lbBloomCnt.Text = "99";
            this.lbBloomCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBloomCnt.DoubleClick += new System.EventHandler(this.lbPieceNo_DoubleClick);
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
            this.lbZone.Text = "Z60";
            this.lbZone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbZone.DoubleClick += new System.EventHandler(this.lbZone_DoubleClick);
            // 
            // lbBloomNo
            // 
            this.lbBloomNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.lbBloomNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBloomNo.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold);
            this.lbBloomNo.ForeColor = System.Drawing.Color.Black;
            this.lbBloomNo.Location = new System.Drawing.Point(2, 24);
            this.lbBloomNo.Margin = new System.Windows.Forms.Padding(0);
            this.lbBloomNo.Name = "lbBloomNo";
            this.lbBloomNo.Size = new System.Drawing.Size(180, 20);
            this.lbBloomNo.TabIndex = 20;
            this.lbBloomNo.Text = "MQF0700007";
            this.lbBloomNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBloomNo.UseMnemonic = false;
            this.lbBloomNo.Click += new System.EventHandler(this.lbBundleNo_Click);
            this.lbBloomNo.DoubleClick += new System.EventHandler(this.lbBundleNo_DoubleClick);
            // 
            // UC_Zone_BC
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lbBloomNo);
            this.Controls.Add(this.lbBloomCnt);
            this.Controls.Add(this.lbZone);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "UC_Zone_BC";
            this.Size = new System.Drawing.Size(184, 46);
            this.Click += new System.EventHandler(this.UC_Zone_Click);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Label lbBloomCnt;
        protected System.Windows.Forms.Label lbZone;
        protected System.Windows.Forms.Label lbBloomNo;
    }
}
