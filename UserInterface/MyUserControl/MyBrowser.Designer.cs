namespace UserInterface.MyUserControl
{
    partial class MyBrowser
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_Back = new System.Windows.Forms.ToolStripButton();
            this.btn_Forward = new System.Windows.Forms.ToolStripButton();
            this.cmb_Url = new System.Windows.Forms.ToolStripComboBox();
            this.btn_Refresh = new System.Windows.Forms.ToolStripButton();
            this.btn_Home = new System.Windows.Forms.ToolStripButton();
            this.btn_Browser = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.webBrowser);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 384);
            this.panel1.TabIndex = 1;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 25);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(550, 359);
            this.webBrowser.TabIndex = 1;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_Back,
            this.btn_Forward,
            this.cmb_Url,
            this.btn_Refresh,
            this.btn_Home,
            this.btn_Browser});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(550, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btn_Back
            // 
            this.btn_Back.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Back.Image = global::UserInterface.Properties.Resources.back;
            this.btn_Back.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(23, 22);
            this.btn_Back.Text = "后退";
            this.btn_Back.Click += new System.EventHandler(this.btn_Back_Click);
            // 
            // btn_Forward
            // 
            this.btn_Forward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Forward.Image = global::UserInterface.Properties.Resources.forward;
            this.btn_Forward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Forward.Name = "btn_Forward";
            this.btn_Forward.Size = new System.Drawing.Size(23, 22);
            this.btn_Forward.Text = "前进";
            this.btn_Forward.Click += new System.EventHandler(this.btn_Forward_Click);
            // 
            // cmb_Url
            // 
            this.cmb_Url.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmb_Url.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_Url.Name = "cmb_Url";
            this.cmb_Url.Size = new System.Drawing.Size(300, 25);
            this.cmb_Url.Sorted = true;
            this.cmb_Url.SelectedIndexChanged += new System.EventHandler(this.cmb_Url_SelectedIndexChanged);
            this.cmb_Url.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_Url_KeyDown);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Refresh.Image = global::UserInterface.Properties.Resources.refresh;
            this.btn_Refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(23, 22);
            this.btn_Refresh.Text = "刷新";
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // btn_Home
            // 
            this.btn_Home.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Home.Image = global::UserInterface.Properties.Resources.home;
            this.btn_Home.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Home.Name = "btn_Home";
            this.btn_Home.Size = new System.Drawing.Size(23, 22);
            this.btn_Home.Text = "主页";
            this.btn_Home.Click += new System.EventHandler(this.btn_Home_Click);
            // 
            // btn_Browser
            // 
            this.btn_Browser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Browser.Image = global::UserInterface.Properties.Resources.browser;
            this.btn_Browser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Browser.Name = "btn_Browser";
            this.btn_Browser.Size = new System.Drawing.Size(23, 22);
            this.btn_Browser.Text = "浏览器中打开";
            this.btn_Browser.Click += new System.EventHandler(this.btn_Browser_Click);
            // 
            // MyBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "MyBrowser";
            this.Size = new System.Drawing.Size(550, 384);
            this.Load += new System.EventHandler(this.MyBrowser_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_Back;
        private System.Windows.Forms.ToolStripButton btn_Forward;
        private System.Windows.Forms.ToolStripComboBox cmb_Url;
        private System.Windows.Forms.ToolStripButton btn_Refresh;
        private System.Windows.Forms.ToolStripButton btn_Home;
        private System.Windows.Forms.ToolStripButton btn_Browser;
    }
}
