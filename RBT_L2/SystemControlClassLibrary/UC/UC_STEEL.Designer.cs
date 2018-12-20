using System;
using System.Windows.Forms;

namespace SystemControlClassLibrary.UC
{
    partial class UC_STEEL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_STEEL));
            this.gangjong_Nm_tb = new C1.Win.C1Input.C1TextBox();
            this.gangjong_id_tb = new C1.Win.C1Input.C1TextBox();
            this.lbSteel = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnSteel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gangjong_Nm_tb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gangjong_id_tb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // gangjong_Nm_tb
            // 
            this.gangjong_Nm_tb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.gangjong_Nm_tb.EmptyAsNull = true;
            this.gangjong_Nm_tb.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.gangjong_Nm_tb.Location = new System.Drawing.Point(142, 2);
            this.gangjong_Nm_tb.Margin = new System.Windows.Forms.Padding(4);
            this.gangjong_Nm_tb.Name = "gangjong_Nm_tb";
            this.gangjong_Nm_tb.Size = new System.Drawing.Size(80, 24);
            this.gangjong_Nm_tb.TabIndex = 47;
            this.gangjong_Nm_tb.Tag = null;
            this.gangjong_Nm_tb.TextDetached = true;
            this.gangjong_Nm_tb.TextChanged += new System.EventHandler(this.gangjong_Nm_tb_TextChanged);
            // 
            // gangjong_id_tb
            // 
            this.gangjong_id_tb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.gangjong_id_tb.EmptyAsNull = true;
            this.gangjong_id_tb.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.gangjong_id_tb.Location = new System.Drawing.Point(68, 2);
            this.gangjong_id_tb.Margin = new System.Windows.Forms.Padding(4);
            this.gangjong_id_tb.Name = "gangjong_id_tb";
            this.gangjong_id_tb.Size = new System.Drawing.Size(41, 24);
            this.gangjong_id_tb.TabIndex = 46;
            this.gangjong_id_tb.Tag = null;
            this.gangjong_id_tb.TextDetached = true;
            this.gangjong_id_tb.TextChanged += new System.EventHandler(this.gangjong_id_tb_TextChanged);
            this.gangjong_id_tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gangjong_id_tb_KeyDown);
            // 
            // lbSteel
            // 
            this.lbSteel.AutoSize = true;
            this.lbSteel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSteel.Location = new System.Drawing.Point(20, 8);
            this.lbSteel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSteel.Name = "lbSteel";
            this.lbSteel.Size = new System.Drawing.Size(39, 15);
            this.lbSteel.TabIndex = 43;
            this.lbSteel.Text = "강종";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(11, 13);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(5, 5);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 48;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // btnSteel
            // 
            this.btnSteel.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSteel.Image = ((System.Drawing.Image)(resources.GetObject("btnSteel.Image")));
            this.btnSteel.Location = new System.Drawing.Point(108, 1);
            this.btnSteel.Margin = new System.Windows.Forms.Padding(1);
            this.btnSteel.Name = "btnSteel";
            this.btnSteel.Size = new System.Drawing.Size(36, 27);
            this.btnSteel.TabIndex = 45;
            this.btnSteel.UseVisualStyleBackColor = true;
            this.btnSteel.Click += new System.EventHandler(this.btnSteel_Click);
            // 
            // UC_STEEL
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.gangjong_Nm_tb);
            this.Controls.Add(this.gangjong_id_tb);
            this.Controls.Add(this.btnSteel);
            this.Controls.Add(this.lbSteel);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UC_STEEL";
            this.Size = new System.Drawing.Size(226, 30);
            this.Load += new System.EventHandler(this.UC_STEEL_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gangjong_Nm_tb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gangjong_id_tb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public C1.Win.C1Input.C1TextBox gangjong_Nm_tb;
        public C1.Win.C1Input.C1TextBox gangjong_id_tb;
        private System.Windows.Forms.Button btnSteel;
        private System.Windows.Forms.Label lbSteel;
        private PictureBox pictureBox2;
    }
}
