using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    public partial class NetConfig : Form
    {
        public NetConfig()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.txt_OnlineIP.Text = "121.42.15.139";
            this.txt_SCMIP.Text = "10.10.100.254";
        }

        private void submit_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.OnlineIP = "http://" + txt_OnlineIP.Text + ":" + txt_OnlinePort.Text;
            Properties.Settings.Default.SCMIP = txt_SCMIP.Text;
            Properties.Settings.Default.SCMPort = txt_SCMPort.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
