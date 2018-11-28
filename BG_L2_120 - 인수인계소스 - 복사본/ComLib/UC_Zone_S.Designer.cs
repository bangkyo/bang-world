namespace ComLib
{
    partial class UC_Zone_S
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbZone = new System.Windows.Forms.Label();
            this.lbPieceNo = new System.Windows.Forms.Label();
            this.lbBundleNo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lbBundleNo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbPieceNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbZone, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(84, 34);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbZone
            // 
            this.lbZone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(93)))), ((int)(((byte)(162)))));
            this.lbZone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbZone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbZone.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lbZone.ForeColor = System.Drawing.Color.White;
            this.lbZone.Location = new System.Drawing.Point(2, 2);
            this.lbZone.Margin = new System.Windows.Forms.Padding(0);
            this.lbZone.Name = "lbZone";
            this.lbZone.Size = new System.Drawing.Size(39, 14);
            this.lbZone.TabIndex = 229;
            this.lbZone.Text = "3Z13";
            this.lbZone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPieceNo
            // 
            this.lbPieceNo.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lbPieceNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPieceNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPieceNo.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lbPieceNo.ForeColor = System.Drawing.Color.Black;
            this.lbPieceNo.Location = new System.Drawing.Point(42, 2);
            this.lbPieceNo.Margin = new System.Windows.Forms.Padding(0);
            this.lbPieceNo.Name = "lbPieceNo";
            this.lbPieceNo.Size = new System.Drawing.Size(40, 14);
            this.lbPieceNo.TabIndex = 230;
            this.lbPieceNo.Text = "99";
            this.lbPieceNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBundleNo
            // 
            this.lbBundleNo.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.lbBundleNo, 2);
            this.lbBundleNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBundleNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBundleNo.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lbBundleNo.ForeColor = System.Drawing.Color.White;
            this.lbBundleNo.Location = new System.Drawing.Point(2, 17);
            this.lbBundleNo.Margin = new System.Windows.Forms.Padding(0);
            this.lbBundleNo.Name = "lbBundleNo";
            this.lbBundleNo.Size = new System.Drawing.Size(80, 15);
            this.lbBundleNo.TabIndex = 231;
            this.lbBundleNo.Text = "MQF0700007";
            this.lbBundleNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UC_Zone_S
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "UC_Zone_S";
            this.Size = new System.Drawing.Size(84, 34);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbZone;
        private System.Windows.Forms.Label lbPieceNo;
        private System.Windows.Forms.Label lbBundleNo;
    }
}
