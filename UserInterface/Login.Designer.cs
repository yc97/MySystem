namespace UserInterface
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_PassWord = new System.Windows.Forms.TextBox();
            this.chk_RememberMe = new System.Windows.Forms.CheckBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.btn_Signin = new System.Windows.Forms.Button();
            this.lnklbl_Forget = new System.Windows.Forms.LinkLabel();
            this.status = new System.Windows.Forms.Label();
            this.cmb_UserName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码";
            // 
            // txt_PassWord
            // 
            this.txt_PassWord.Location = new System.Drawing.Point(142, 106);
            this.txt_PassWord.Name = "txt_PassWord";
            this.txt_PassWord.PasswordChar = '*';
            this.txt_PassWord.Size = new System.Drawing.Size(137, 21);
            this.txt_PassWord.TabIndex = 3;
            // 
            // chk_RememberMe
            // 
            this.chk_RememberMe.AutoSize = true;
            this.chk_RememberMe.Location = new System.Drawing.Point(62, 160);
            this.chk_RememberMe.Name = "chk_RememberMe";
            this.chk_RememberMe.Size = new System.Drawing.Size(72, 16);
            this.chk_RememberMe.TabIndex = 4;
            this.chk_RememberMe.Text = "记住密码";
            this.chk_RememberMe.UseVisualStyleBackColor = true;
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(62, 196);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(82, 23);
            this.btn_Login.TabIndex = 6;
            this.btn_Login.Text = "登录";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // btn_Signin
            // 
            this.btn_Signin.Location = new System.Drawing.Point(186, 196);
            this.btn_Signin.Name = "btn_Signin";
            this.btn_Signin.Size = new System.Drawing.Size(82, 23);
            this.btn_Signin.TabIndex = 7;
            this.btn_Signin.Text = "注册";
            this.btn_Signin.UseVisualStyleBackColor = true;
            // 
            // lnklbl_Forget
            // 
            this.lnklbl_Forget.AutoSize = true;
            this.lnklbl_Forget.Location = new System.Drawing.Point(184, 161);
            this.lnklbl_Forget.Name = "lnklbl_Forget";
            this.lnklbl_Forget.Size = new System.Drawing.Size(53, 12);
            this.lnklbl_Forget.TabIndex = 8;
            this.lnklbl_Forget.TabStop = true;
            this.lnklbl_Forget.Text = "忘记密码";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(60, 246);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 12);
            this.status.TabIndex = 9;
            // 
            // cmb_UserName
            // 
            this.cmb_UserName.FormattingEnabled = true;
            this.cmb_UserName.Location = new System.Drawing.Point(142, 52);
            this.cmb_UserName.Name = "cmb_UserName";
            this.cmb_UserName.Size = new System.Drawing.Size(137, 20);
            this.cmb_UserName.TabIndex = 10;
            this.cmb_UserName.SelectedIndexChanged += new System.EventHandler(this.cmb_UserName_SelectedIndexChanged);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 296);
            this.Controls.Add(this.cmb_UserName);
            this.Controls.Add(this.status);
            this.Controls.Add(this.lnklbl_Forget);
            this.Controls.Add(this.btn_Signin);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.chk_RememberMe);
            this.Controls.Add(this.txt_PassWord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_PassWord;
        private System.Windows.Forms.CheckBox chk_RememberMe;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Button btn_Signin;
        private System.Windows.Forms.LinkLabel lnklbl_Forget;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.ComboBox cmb_UserName;
    }
}