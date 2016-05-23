namespace UserInterface
{
    partial class NetConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetConfig));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tab_main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_OnlinePort = new System.Windows.Forms.TextBox();
            this.cancel = new System.Windows.Forms.Button();
            this.submit = new System.Windows.Forms.Button();
            this.txt_SCMPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_OnlineIP = new UserInterface.MyUserControl.UserControl_IPv4TextBox();
            this.txt_SCMIP = new UserInterface.MyUserControl.UserControl_IPv4TextBox();
            this.panel1.SuspendLayout();
            this.tab_main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tab_main);
            this.panel1.Location = new System.Drawing.Point(37, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(374, 301);
            this.panel1.TabIndex = 0;
            // 
            // tab_main
            // 
            this.tab_main.Controls.Add(this.tabPage1);
            this.tab_main.Controls.Add(this.tabPage2);
            this.tab_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_main.Location = new System.Drawing.Point(0, 0);
            this.tab_main.Name = "tab_main";
            this.tab_main.SelectedIndex = 0;
            this.tab_main.Size = new System.Drawing.Size(374, 301);
            this.tab_main.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txt_OnlineIP);
            this.tabPage1.Controls.Add(this.txt_OnlinePort);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(366, 275);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "在线系统";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txt_SCMIP);
            this.tabPage2.Controls.Add(this.txt_SCMPort);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(366, 275);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "单片机";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口号";
            // 
            // txt_OnlinePort
            // 
            this.txt_OnlinePort.Location = new System.Drawing.Point(97, 87);
            this.txt_OnlinePort.Name = "txt_OnlinePort";
            this.txt_OnlinePort.Size = new System.Drawing.Size(60, 21);
            this.txt_OnlinePort.TabIndex = 3;
            this.txt_OnlinePort.Text = "1000";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(336, 359);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "取消";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // submit
            // 
            this.submit.Location = new System.Drawing.Point(255, 359);
            this.submit.Name = "submit";
            this.submit.Size = new System.Drawing.Size(75, 23);
            this.submit.TabIndex = 2;
            this.submit.Text = "确认";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Click += new System.EventHandler(this.submit_Click);
            // 
            // txt_SCMPort
            // 
            this.txt_SCMPort.Location = new System.Drawing.Point(104, 87);
            this.txt_SCMPort.Name = "txt_SCMPort";
            this.txt_SCMPort.Size = new System.Drawing.Size(48, 21);
            this.txt_SCMPort.TabIndex = 7;
            this.txt_SCMPort.Text = "8899";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "端口号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "IP地址";
            // 
            // txt_OnlineIP
            // 
            this.txt_OnlineIP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txt_OnlineIP.Location = new System.Drawing.Point(97, 45);
            this.txt_OnlineIP.Name = "txt_OnlineIP";
            this.txt_OnlineIP.Size = new System.Drawing.Size(212, 27);
            this.txt_OnlineIP.TabIndex = 4;
            this.txt_OnlineIP.Value = ((System.Net.IPAddress)(resources.GetObject("txt_OnlineIP.Value")));
            // 
            // txt_SCMIP
            // 
            this.txt_SCMIP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txt_SCMIP.Location = new System.Drawing.Point(105, 46);
            this.txt_SCMIP.Name = "txt_SCMIP";
            this.txt_SCMIP.Size = new System.Drawing.Size(212, 27);
            this.txt_SCMIP.TabIndex = 8;
            this.txt_SCMIP.Value = ((System.Net.IPAddress)(resources.GetObject("txt_SCMIP.Value")));
            // 
            // NetConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 412);
            this.Controls.Add(this.submit);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.panel1);
            this.Name = "NetConfig";
            this.Text = "网络配置";
            this.panel1.ResumeLayout(false);
            this.tab_main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tab_main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txt_OnlinePort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button submit;
        private System.Windows.Forms.TextBox txt_SCMPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private MyUserControl.UserControl_IPv4TextBox txt_OnlineIP;
        private MyUserControl.UserControl_IPv4TextBox txt_SCMIP;


    }
}