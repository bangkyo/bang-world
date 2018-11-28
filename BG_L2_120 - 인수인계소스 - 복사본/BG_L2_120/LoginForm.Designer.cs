namespace BG_L2_120
{
    partial class LoginForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.txtLoginID = new C1.Win.C1Input.C1TextBox();
            this.txtPasswd = new C1.Win.C1Input.C1TextBox();
            this.idchk = new System.Windows.Forms.CheckBox();
            this.lblChangePassword = new System.Windows.Forms.Label();
            this.btnLogin = new C1.Win.C1Input.C1Button();
            this.lblCapsLock = new C1.Win.C1Input.C1Label();
            this.c1Button1 = new C1.Win.C1Input.C1Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCapsLock)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLoginID
            // 
            this.txtLoginID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginID.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLoginID.Location = new System.Drawing.Point(111, 309);
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(180, 19);
            this.txtLoginID.TabIndex = 2;
            this.txtLoginID.Tag = null;
            this.txtLoginID.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtLoginID.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtLoginID.TextChanged += new System.EventHandler(this.txtLoginID_TextChanged);
            this.txtLoginID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtLoginID_KeyDown);
            // 
            // txtPasswd
            // 
            this.txtPasswd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPasswd.Font = new System.Drawing.Font("돋움체", 9F);
            this.txtPasswd.Location = new System.Drawing.Point(111, 341);
            this.txtPasswd.Name = "txtPasswd";
            this.txtPasswd.PasswordChar = '*';
            this.txtPasswd.Size = new System.Drawing.Size(180, 19);
            this.txtPasswd.TabIndex = 3;
            this.txtPasswd.Tag = "admin";
            this.txtPasswd.Value = "";
            this.txtPasswd.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPasswd.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Blue;
            this.txtPasswd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPasswd_KeyDown);
            // 
            // idchk
            // 
            this.idchk.AutoSize = true;
            this.idchk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.idchk.Font = new System.Drawing.Font("돋움체", 9F);
            this.idchk.Location = new System.Drawing.Point(110, 373);
            this.idchk.Name = "idchk";
            this.idchk.Size = new System.Drawing.Size(90, 16);
            this.idchk.TabIndex = 7;
            this.idchk.Text = "아이디 저장";
            this.idchk.UseVisualStyleBackColor = true;
            // 
            // lblChangePassword
            // 
            this.lblChangePassword.AutoSize = true;
            this.lblChangePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblChangePassword.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblChangePassword.Image = ((System.Drawing.Image)(resources.GetObject("lblChangePassword.Image")));
            this.lblChangePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblChangePassword.Location = new System.Drawing.Point(200, 374);
            this.lblChangePassword.Name = "lblChangePassword";
            this.lblChangePassword.Size = new System.Drawing.Size(107, 12);
            this.lblChangePassword.TabIndex = 8;
            this.lblChangePassword.Text = "    비밀번호 변경";
            this.lblChangePassword.Click += new System.EventHandler(this.lblChangePassword_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLogin.Location = new System.Drawing.Point(309, 306);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(89, 57);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnLogin_KeyDown);
            // 
            // lblCapsLock
            // 
            this.lblCapsLock.AutoSize = true;
            this.lblCapsLock.BackColor = System.Drawing.Color.Transparent;
            this.lblCapsLock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblCapsLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCapsLock.ForeColor = System.Drawing.Color.Red;
            this.lblCapsLock.Location = new System.Drawing.Point(90, 405);
            this.lblCapsLock.Name = "lblCapsLock";
            this.lblCapsLock.Size = new System.Drawing.Size(213, 17);
            this.lblCapsLock.TabIndex = 3;
            this.lblCapsLock.Tag = null;
            this.lblCapsLock.Text = "<Caps Lock>이 켜져 있습니다.";
            this.lblCapsLock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCapsLock.TextDetached = true;
            this.lblCapsLock.Visible = false;
            // 
            // c1Button1
            // 
            this.c1Button1.Location = new System.Drawing.Point(309, 256);
            this.c1Button1.Name = "c1Button1";
            this.c1Button1.Size = new System.Drawing.Size(75, 23);
            this.c1Button1.TabIndex = 10;
            this.c1Button1.Text = "로그인";
            this.c1Button1.UseVisualStyleBackColor = true;
            this.c1Button1.Visible = false;
            this.c1Button1.Click += new System.EventHandler(this.c1Button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(88, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(30, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "아이디:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(30, 346);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "비밀번호:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.BlueViolet;
            this.button1.Location = new System.Drawing.Point(12, 314);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(12, 11);
            this.button1.TabIndex = 14;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.BlueViolet;
            this.button2.Location = new System.Drawing.Point(12, 347);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(12, 11);
            this.button2.TabIndex = 14;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(431, 434);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCapsLock);
            this.Controls.Add(this.c1Button1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblChangePassword);
            this.Controls.Add(this.idchk);
            this.Controls.Add(this.txtPasswd);
            this.Controls.Add(this.txtLoginID);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "LoginForm";
            this.Text = "로그인";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCapsLock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1Input.C1TextBox txtLoginID;
        private C1.Win.C1Input.C1TextBox txtPasswd;
        private System.Windows.Forms.CheckBox idchk;
        private System.Windows.Forms.Label lblChangePassword;
        private C1.Win.C1Input.C1Button btnLogin;
        private C1.Win.C1Input.C1Label lblCapsLock;
        private C1.Win.C1Input.C1Button c1Button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

