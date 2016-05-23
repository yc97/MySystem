using BusinessLogicLayer;
using NationalInstruments.UI.WindowsForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace UserInterface
{
    /// <summary>
    /// 主窗体类
    /// </summary>
    public partial class MainForm : Form
    {

        Dictionary<int, TabPage> tabPageList = new Dictionary<int, TabPage>();

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            //初始化listView
            listView_Properties_Init();

            //初始化setting
            if (Properties.Settings.Default.OnlineIP == "") 
            {
                Properties.Settings.Default.OnlineIP = "http://127.0.0.1:8000";
            }

            if (Properties.Settings.Default.SCMIP == "")
            {
                Properties.Settings.Default.SCMIP = "10.10.100.254";
            }

            if (Properties.Settings.Default.SCMPort == "")
            {
                Properties.Settings.Default.SCMPort = "8899";
            }
            Properties.Settings.Default.Save();

        }

        /// <summary>
        /// 窗体加载，启动登录页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            
            Login logFrm = new Login();
            if (logFrm.ShowDialog() == DialogResult.OK)
            {
                ModelLayer.Users user = ModelLayer.CurrentUser.currentUser;    // 此处获取“登陆窗体”传递过来的数据
                //System.Console.WriteLine("OK");
                //以下代码初始化主窗体
                this.Text = this.Text + "-(" + user.userName + ")";
                this.lbl_Time.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                this.timer_time.Start();
                this.lbl_Time.Alignment = ToolStripItemAlignment.Right;
                //初始化cmb_SystemSelect 
                BusinessLogicLayer.MySystemsBLL sysBLL= new BusinessLogicLayer.MySystemsBLL();
                DataTable allSystems = sysBLL.getSystems(user.userName);
                this.cmb_SystemSelect.DataSource = allSystems;
                this.cmb_SystemSelect.DisplayMember = "sysName";
                this.cmb_SystemSelect.ValueMember = "mySystemsID";
            }
            else
            {
                //System.Console.WriteLine("Cancel");
                this.Close();    // 关闭主窗体
            }
        }

        /// <summary>
        /// 系统选择下拉单 对应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_SystemSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectID = 0;
            if (int.TryParse(this.cmb_SystemSelect.SelectedValue.ToString(), out selectID))
            {
                BusinessLogicLayer.MySystemsBLL sysBLL = new BusinessLogicLayer.MySystemsBLL();
                DataTable nodes = sysBLL.getAllNodes(selectID);
                this.sysTree.SetbindingWord("nodeID", "nodeName", "parentID");
                this.sysTree.bindingDataTable = nodes;
                this.sysTree.Refresh();
                this.sysTree.ExpandAll();
            }
            
        }
        /// <summary>
        /// 双击系统树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.LastNode == null)
            {
                DataBLL dataBLL = new DataBLL();
                ModelLayer.Nodes nodeSel = null;
                int nodeID = System.Convert.ToInt32(e.Node.Name);
                nodeSel = dataBLL.getNodeInfo(nodeID);//获取node的详细信息 device,signalType,chNO
                nodeSel.nodeName = e.Node.Text;
                nodeSel.nodeID = nodeID;
                listView_AddItems(listView_Properties, nodeSel.getNodeDic());
                if (nodeSel.device == null || nodeSel.device == "")
                {
                    //没有nodeinfo 使用自定义layoutpanel布局
                    MyUserControl.MyTableLayoutPanel tlp = new MyUserControl.MyTableLayoutPanel();
                    tlp.SetNodeConfigPanel(nodeID, nodeSel.nodeName);
                    tlp.save.Click += new System.EventHandler(this.tpl_save_Click);
                    Ctab_MainTab.PageAdd(tlp, nodeID, nodeSel.nodeName, tabPageList);
                }
                else 
                {
                    if (nodeSel.device.ToUpper() == "WIFI") 
                    {
                        //有nodeinfo  添加曲线图
                        MyUserControl.TCPWave tcpWave = new  MyUserControl.TCPWave();
                        tcpWave.Dock = DockStyle.Fill;
                        Ctab_MainTab.PageAdd(tcpWave, nodeID, nodeSel.nodeName, tabPageList);
                    }
                    else
                    {
                        //有nodeinfo  添加曲线图
                        CANModels.ADForm.ADForm fm = new CANModels.ADForm.ADForm(nodeSel.chNO);
                        fm.FormBorderStyle = FormBorderStyle.None; // 取消窗体边框
                        fm.TopLevel = false; // 取消最顶层窗体
                        fm.Dock = DockStyle.Fill;
                        Ctab_MainTab.PageAdd(fm, nodeID, nodeSel.nodeName, tabPageList);
                        fm.Show();
                    }
                }
            }
        }

        /// <summary>
        /// MyTableLayoutPanel的 保存 按钮事件
        /// 保存device,signalType,chNO到数据库并刷新tabpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tpl_save_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //int nodeID, string device, string signalType, int chNO
            int nodeID;
            string nodeName;
            string device;
            string signalType;
            int chNO;
            Button save = (Button)sender;
            MyUserControl.MyTableLayoutPanel tlp = (MyUserControl.MyTableLayoutPanel)save.Parent;
            TabPage tp = (TabPage)tlp.Parent;
            MyUserControl.ClosableTabControl ctc = (MyUserControl.ClosableTabControl)tp.Parent;
            nodeID = System.Convert.ToInt32(tlp.lbl_nodeID.Text);
            nodeName = tlp.lbl_nodeName.Text;
            device = tlp.cmb_device.Text;
            signalType = tlp.cmb_signalType.Text;
            chNO = System.Convert.ToInt32(tlp.cmb_chNO.Text);
            new BusinessLogicLayer.MySystemsBLL().saveNodeInfo(nodeID, device, signalType, chNO);
            MessageBox.Show("保存成功！");
            WaveformGraph wave = new WaveformGraph();
            wave.Dock = DockStyle.Fill;
            WaveGraph_Init(wave, nodeName);
            ctc.PageUpdate(wave, nodeID, tabPageList);
            tp.Refresh();
        }


        /// <summary>
        /// WaveGraph_Init
        /// </summary>
        /// <param name="wave"></param>
        /// <param name="waveName"></param>
        private void WaveGraph_Init(WaveformGraph wave, string waveName)
        {
            // 
            // waveformGraph1
            // 
            NationalInstruments.UI.XAxis xAxis1 = new NationalInstruments.UI.XAxis();
            NationalInstruments.UI.YAxis yAxis1 = new NationalInstruments.UI.YAxis();
            NationalInstruments.UI.WaveformPlot waveformPlot1 = new NationalInstruments.UI.WaveformPlot();

            //wave.Location = new System.Drawing.Point(26, 107);
            wave.Name = waveName;
            wave.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {waveformPlot1});
            //wave.Size = new System.Drawing.Size(68, 72);
            wave.TabIndex = 0;
            wave.UseColorGenerator = true;
            wave.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {xAxis1});
            wave.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {yAxis1});
            // 
            // waveformPlot1
            // 
            waveformPlot1.XAxis = xAxis1;
            waveformPlot1.XAxis.Mode = NationalInstruments.UI.AxisMode.StripChart;
            waveformPlot1.YAxis = yAxis1;
            waveformPlot1.YAxis.Mode = NationalInstruments.UI.AxisMode.AutoScaleExact;
            // 
        }


        /// <summary>
        /// listView_Properties_Init
        /// </summary>
        private void listView_Properties_Init()
        {
            //初始化listview
            listView_Properties.Items.Clear();
            listView_Properties.Columns.Add("属性", 80);
            listView_Properties.Columns.Add("值", 100);
        }

        /// <summary>
        /// 使用Dictionary参数，往listview中添加数据
        /// 通用所有listview
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="dic"></param>
        private void listView_AddItems(ListView listView, Dictionary<string, string[]> dic)
        {
            //添加数据
            listView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            listView.Items.Clear();  //移除所有的项。  
            foreach (KeyValuePair<string, string[]> kvp in dic)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = kvp.Key;
                foreach (string v in kvp.Value)
                {
                    lvi.SubItems.Add(v);
                }
                listView.Items.Add(lvi);
            }
            listView.Items[listView.Items.Count - 1].EnsureVisible(); //显示最新添加的item
            listView.EndUpdate();  //结束数据处理，UI界面一次性绘制。
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (btn_Start.Text == "启动")
            { 
                btn_Start.Text = "停止"; 
            }
            else if (btn_Start.Text == "停止")
            { 
                btn_Start.Text = "启动";
            }
        }

        /// <summary>
        ///系统时间tick 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_time_Tick(object sender, EventArgs e)
        {
            this.lbl_Time.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        /// <summary>
        /// 选项卡切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctab_MainTab_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage != null)
            {
                Refresh_listView_Properties(e.TabPage.Tag.ToString(), e.TabPage.Text);
            }
            else 
            {
                this.listView_Properties_Init();
            }
        }

        /// <summary>
        /// 使用nodeID   nodeName参数刷新 listView_Properties
        /// </summary>
        /// <param name="nodeID_str"></param>
        /// <param name="nodeName"></param>
        private void Refresh_listView_Properties(string nodeID_str, string nodeName) 
        {
            DataBLL dataBLL = new DataBLL();
            ModelLayer.Nodes nodeSel = null;
            int nodeID = System.Convert.ToInt32(nodeID_str);
            nodeSel = dataBLL.getNodeInfo(nodeID);//获取node的详细信息
            nodeSel.nodeName = nodeName;
            nodeSel.nodeID = nodeID;
            listView_AddItems(listView_Properties, nodeSel.getNodeDic());
        }
        /// <summary>
        /// online_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void online_Click(object sender, EventArgs e)
        {
            string ipAddr;
            if (Properties.Settings.Default.OnlineIP == "")
            { 
                ipAddr = "http://127.0.0.1"; 
            }
            else 
            { 
                ipAddr = Properties.Settings.Default.OnlineIP; 
            }
            MyUserControl.MyBrowser browser = new MyUserControl.MyBrowser(ipAddr);
            Ctab_MainTab.PageAdd(browser, -10, "在线系统", tabPageList);
        }

        private void cAN通信配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CANModels.ComConfig.CANComConfigForm comConfigForm = new CANModels.ComConfig.CANComConfigForm();
            comConfigForm.ShowDialog();
        }

        private void 模块地址配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CANModels.DevConfig.FormDevCfg fm = new CANModels.DevConfig.FormDevCfg();
            fm.ShowDialog();
        }

        private void 网络配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetConfig fm = new NetConfig();
            fm.ShowDialog();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void btn_ReadCfg_Click(object sender, EventArgs e)
        {

        }

    }
}
