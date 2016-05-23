using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace UserInterface.MyUserControl
{
    public partial class MyBrowser : UserControl
    {
        private string defaultUrl = "";
        BusinessLogicLayer.UrlBLL urlBLL = new BusinessLogicLayer.UrlBLL();

        public MyBrowser()
        {
            InitializeComponent();
            defaultUrl = "127.0.0.1";
            this.Dock = DockStyle.Fill;
            //设置为IE9，以支持html5
            var appName = Process.GetCurrentProcess().MainModule.ModuleName;
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", appName, 9999, RegistryValueKind.DWord);
        }

        public MyBrowser(string _defaultUrl)
        {
            InitializeComponent();
            defaultUrl = _defaultUrl;
            this.Dock = DockStyle.Fill;
            //设置为IE9，以支持html5
            var appName = Process.GetCurrentProcess().MainModule.ModuleName;
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", appName, 9999, RegistryValueKind.DWord);
        }

        public string DefaultUrl 
        {
            get { return defaultUrl; }
            set { defaultUrl = value; }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            if (btn_Back.Enabled == true)
            { 
                this.webBrowser.GoBack(); 
            }
        }

        private void btn_Forward_Click(object sender, EventArgs e)
        {
            if (btn_Forward.Enabled == true)
            {
                this.webBrowser.GoForward();
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            this.webBrowser.Refresh();
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            this.webBrowser.Navigate(defaultUrl);
        }

        private void btn_Browser_Click(object sender, EventArgs e)
        {
            //调用系统默认的浏览器   
            System.Diagnostics.Process.Start(cmb_Url.Text);  
        }

        private void MyBrowser_Load(object sender, EventArgs e)
        {
            this.webBrowser.Navigate(defaultUrl);
            Refresh_cmb_Url();
            cmb_Url.Text = DefaultUrl;
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            cmb_Url.Text = webBrowser.Url.ToString();
        }

        private void cmb_Url_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmb_Url.Text == "清除记录")
            {
                if (MessageBox.Show("是否要清除所有输入记录？", "警告！", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    urlBLL.clearUrlHistory();
                    Refresh_cmb_Url();
                }
            }
            else
            {
                this.webBrowser.Navigate(cmb_Url.Text);
            }
        }

        private void cmb_Url_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.webBrowser.Navigate(cmb_Url.Text);
                //保存输入记录并更新combobox
                urlBLL.addUrlHistory(cmb_Url.Text);
                Refresh_cmb_Url();
            }
            
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //将所有的链接的目标，指向本窗体   
            foreach (HtmlElement archor in this.webBrowser.Document.Links)
            {
                archor.SetAttribute("target", "_self");
            }
            //将所有的FORM的提交目标，指向本窗体   
            foreach (HtmlElement form in this.webBrowser.Document.Forms)
            {
                form.SetAttribute("target", "_self");
            }   
        }

        private void Refresh_cmb_Url() 
        {
            DataTable urlAll = urlBLL.getUrlHistory();
            DataRow dr = urlAll.NewRow();
            dr["url"] = "清除记录";
            urlAll.Rows.Add(dr);
            cmb_Url.ComboBox.DataSource = urlAll;
            cmb_Url.ComboBox.DisplayMember = "url";
        }


    }
}
