namespace CANModels.ComConfig
{
    partial class CANComConfigForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.EDIT_STATE = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.BT_CONVERT = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.BT_OPEN = new System.Windows.Forms.Button();
            this.BT_CFG_READ = new System.Windows.Forms.Button();
            this.BT_CFG = new System.Windows.Forms.Button();
            this.BT_RESET = new System.Windows.Forms.Button();
            this.CMB_DEV_INDEX = new System.Windows.Forms.ComboBox();
            this.EDIT_AMR0 = new System.Windows.Forms.TextBox();
            this.EDIT_ACR0 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.EDIT_BTR1 = new System.Windows.Forms.TextBox();
            this.EDIT_BTR0 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // EDIT_STATE
            // 
            this.EDIT_STATE.Enabled = false;
            this.EDIT_STATE.Location = new System.Drawing.Point(112, 156);
            this.EDIT_STATE.Name = "EDIT_STATE";
            this.EDIT_STATE.Size = new System.Drawing.Size(269, 21);
            this.EDIT_STATE.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "程序运行状态";
            // 
            // BT_CONVERT
            // 
            this.BT_CONVERT.Location = new System.Drawing.Point(286, 16);
            this.BT_CONVERT.Name = "BT_CONVERT";
            this.BT_CONVERT.Size = new System.Drawing.Size(61, 24);
            this.BT_CONVERT.TabIndex = 0;
            this.BT_CONVERT.Text = "启动转换";
            this.BT_CONVERT.UseVisualStyleBackColor = true;
            this.BT_CONVERT.Click += new System.EventHandler(this.BT_CONVERT_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.BT_CONVERT);
            this.groupBox4.Controls.Add(this.BT_OPEN);
            this.groupBox4.Controls.Add(this.BT_CFG_READ);
            this.groupBox4.Controls.Add(this.BT_CFG);
            this.groupBox4.Controls.Add(this.BT_RESET);
            this.groupBox4.Location = new System.Drawing.Point(26, 21);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(355, 54);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "设备操作";
            // 
            // BT_OPEN
            // 
            this.BT_OPEN.Location = new System.Drawing.Point(6, 16);
            this.BT_OPEN.Name = "BT_OPEN";
            this.BT_OPEN.Size = new System.Drawing.Size(64, 24);
            this.BT_OPEN.TabIndex = 0;
            this.BT_OPEN.Text = "打开设备";
            this.BT_OPEN.UseVisualStyleBackColor = true;
            this.BT_OPEN.Click += new System.EventHandler(this.BT_OPEN_Click);
            // 
            // BT_CFG_READ
            // 
            this.BT_CFG_READ.Location = new System.Drawing.Point(217, 16);
            this.BT_CFG_READ.Name = "BT_CFG_READ";
            this.BT_CFG_READ.Size = new System.Drawing.Size(63, 24);
            this.BT_CFG_READ.TabIndex = 5;
            this.BT_CFG_READ.Text = "回读配置";
            this.BT_CFG_READ.UseVisualStyleBackColor = true;
            this.BT_CFG_READ.Click += new System.EventHandler(this.BT_CFG_READ_Click);
            // 
            // BT_CFG
            // 
            this.BT_CFG.Location = new System.Drawing.Point(152, 16);
            this.BT_CFG.Name = "BT_CFG";
            this.BT_CFG.Size = new System.Drawing.Size(59, 24);
            this.BT_CFG.TabIndex = 4;
            this.BT_CFG.Text = "配置";
            this.BT_CFG.UseVisualStyleBackColor = true;
            this.BT_CFG.Click += new System.EventHandler(this.BT_CFG_Click);
            // 
            // BT_RESET
            // 
            this.BT_RESET.Location = new System.Drawing.Point(76, 16);
            this.BT_RESET.Name = "BT_RESET";
            this.BT_RESET.Size = new System.Drawing.Size(65, 24);
            this.BT_RESET.TabIndex = 2;
            this.BT_RESET.Text = "复位USB";
            this.BT_RESET.UseVisualStyleBackColor = true;
            this.BT_RESET.Click += new System.EventHandler(this.BT_RESET_Click);
            // 
            // CMB_DEV_INDEX
            // 
            this.CMB_DEV_INDEX.FormattingEnabled = true;
            this.CMB_DEV_INDEX.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.CMB_DEV_INDEX.Location = new System.Drawing.Point(282, 34);
            this.CMB_DEV_INDEX.Name = "CMB_DEV_INDEX";
            this.CMB_DEV_INDEX.Size = new System.Drawing.Size(46, 20);
            this.CMB_DEV_INDEX.TabIndex = 8;
            // 
            // EDIT_AMR0
            // 
            this.EDIT_AMR0.Location = new System.Drawing.Point(198, 33);
            this.EDIT_AMR0.Name = "EDIT_AMR0";
            this.EDIT_AMR0.Size = new System.Drawing.Size(62, 21);
            this.EDIT_AMR0.TabIndex = 7;
            // 
            // EDIT_ACR0
            // 
            this.EDIT_ACR0.Location = new System.Drawing.Point(113, 33);
            this.EDIT_ACR0.Name = "EDIT_ACR0";
            this.EDIT_ACR0.Size = new System.Drawing.Size(62, 21);
            this.EDIT_ACR0.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CMB_DEV_INDEX);
            this.groupBox2.Controls.Add(this.EDIT_AMR0);
            this.groupBox2.Controls.Add(this.EDIT_ACR0);
            this.groupBox2.Controls.Add(this.EDIT_BTR1);
            this.groupBox2.Controls.Add(this.EDIT_BTR0);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(26, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 59);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模块配置信息";
            // 
            // EDIT_BTR1
            // 
            this.EDIT_BTR1.Location = new System.Drawing.Point(57, 33);
            this.EDIT_BTR1.Name = "EDIT_BTR1";
            this.EDIT_BTR1.Size = new System.Drawing.Size(27, 21);
            this.EDIT_BTR1.TabIndex = 5;
            // 
            // EDIT_BTR0
            // 
            this.EDIT_BTR0.Location = new System.Drawing.Point(26, 33);
            this.EDIT_BTR0.Name = "EDIT_BTR0";
            this.EDIT_BTR0.Size = new System.Drawing.Size(27, 21);
            this.EDIT_BTR0.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(287, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "设备号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(209, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "AMR0-3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(123, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "ACR0-3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "BTR0/BTR1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 203);
            this.Controls.Add(this.EDIT_STATE);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "通信配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EDIT_STATE;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button BT_CONVERT;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button BT_OPEN;
        private System.Windows.Forms.Button BT_CFG_READ;
        private System.Windows.Forms.Button BT_CFG;
        private System.Windows.Forms.Button BT_RESET;
        private System.Windows.Forms.ComboBox CMB_DEV_INDEX;
        private System.Windows.Forms.TextBox EDIT_AMR0;
        private System.Windows.Forms.TextBox EDIT_ACR0;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox EDIT_BTR1;
        private System.Windows.Forms.TextBox EDIT_BTR0;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;

    }
}