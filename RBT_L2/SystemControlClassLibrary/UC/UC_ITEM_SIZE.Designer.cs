using System;
using System.Windows.Forms;

namespace SystemControlClassLibrary.UC
{
    partial class UC_ITEM_SIZE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_ITEM_SIZE));
            this.lbLine = new System.Windows.Forms.Label();
            this.tbItemSize = new C1.Win.C1Input.C1TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbItemSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lbLine
            // 
            this.lbLine.AutoSize = true;
            this.lbLine.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbLine.Location = new System.Drawing.Point(24, 8);
            this.lbLine.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new System.Drawing.Size(39, 15);
            this.lbLine.TabIndex = 47;
            this.lbLine.Text = "규격";
            // 
            // tbItemSize
            // 
            this.tbItemSize.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbItemSize.Location = new System.Drawing.Point(72, 3);
            this.tbItemSize.Name = "tbItemSize";
            this.tbItemSize.Size = new System.Drawing.Size(80, 24);
            this.tbItemSize.TabIndex = 49;
            this.tbItemSize.Tag = null;
            this.tbItemSize.TextChanged += new System.EventHandler(this.tbItemSize_TextChanged);
            this.tbItemSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbItemSize_KeyDown);
            this.tbItemSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbItemSize_KeyPress);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(11, 13);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(5, 5);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 50;
            this.pictureBox2.TabStop = false;
            // 
            // UC_ITEM_SIZE
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.tbItemSize);
            this.Controls.Add(this.lbLine);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UC_ITEM_SIZE";
            this.Size = new System.Drawing.Size(158, 30);
            ((System.ComponentModel.ISupportInitialize)(this.tbItemSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        protected System.Windows.Forms.Label lbLine;
        protected C1.Win.C1Input.C1TextBox tbItemSize;
        private PictureBox pictureBox2;
    }
}
