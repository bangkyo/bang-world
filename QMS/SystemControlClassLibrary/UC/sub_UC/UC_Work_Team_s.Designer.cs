﻿namespace SystemControlClassLibrary.UC.sub_UC
{
    partial class UC_Work_Team_s
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
            this.cbWork_team = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).BeginInit();
            this.SuspendLayout();
            // 
            // c1Label1
            // 
            this.c1Label1.Size = new System.Drawing.Size(27, 19);
            this.c1Label1.Text = "조";
            // 
            // cbWork_team
            // 
            this.cbWork_team.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.cbWork_team.FormattingEnabled = true;
            this.cbWork_team.Location = new System.Drawing.Point(53, 2);
            this.cbWork_team.Name = "cbWork_team";
            this.cbWork_team.Size = new System.Drawing.Size(66, 23);
            this.cbWork_team.TabIndex = 91;
            this.cbWork_team.SelectedIndexChanged += new System.EventHandler(this.cbWork_team_SelectedIndexChanged);
            // 
            // UC_Work_Team_s
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbWork_team);
            this.Name = "UC_Work_Team_s";
            this.Size = new System.Drawing.Size(126, 27);
            this.Load += new System.EventHandler(this.UC_Work_Team_s_Load);
            this.Controls.SetChildIndex(this.c1Label1, 0);
            this.Controls.SetChildIndex(this.cbWork_team, 0);
            ((System.ComponentModel.ISupportInitialize)(this.c1Label1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbWork_team;
    }
}
