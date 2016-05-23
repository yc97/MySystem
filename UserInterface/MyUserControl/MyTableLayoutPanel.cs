using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.MyUserControl
{
    public partial class MyTableLayoutPanel : TableLayoutPanel
    {
        public Label lbl_nodeID;
        public Label lbl_nodeName;
        public ComboBox cmb_chNO;
        public ComboBox cmb_device;
        public ComboBox cmb_signalType;
        public Button save = new Button();

        public MyTableLayoutPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        public MyTableLayoutPanel(int row, int col)
        {
            InitializeComponent();
            DynamicLayout(this, row, col);
            this.Dock = DockStyle.Fill;
        }

        public void AddToTableLayout(Control ctl, int row, int col)
        {
            //ctl.Dock = DockStyle.Fill;
            this.Controls.Add(ctl);
            this.SetRow(ctl, row);
            this.SetColumn(ctl, col);
        }

        public void SetNodeConfigPanel(int nodeID, string nodeName)
        {
            DynamicLayout(this, 7, 3);

            Label label1 = new Label();
            label1.Text = "节点信息不完整，请配置";
            label1.TextAlign = ContentAlignment.MiddleLeft; 
            label1.Dock = DockStyle.Fill;
            AddToTableLayout(label1, 0, 1);

            FlowLayoutPanel flp1 = new FlowLayoutPanel();
            Label label4 = new Label();
            label4.Text = "节点ID：";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            lbl_nodeID = new Label();
            lbl_nodeID.Text = nodeID.ToString();
            lbl_nodeID.TextAlign = ContentAlignment.MiddleLeft;
            flp1.Controls.Add(label4);
            flp1.Controls.Add(lbl_nodeID);
            flp1.Dock = DockStyle.Fill;
            AddToTableLayout(flp1, 1, 1);

            FlowLayoutPanel flp2 = new FlowLayoutPanel();
            Label label5 = new Label();
            label5.Text = "节点名称：";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            lbl_nodeName = new Label();
            lbl_nodeName.Text = nodeName;
            lbl_nodeName.TextAlign = ContentAlignment.MiddleLeft;
            flp2.Controls.Add(label5);
            flp2.Controls.Add(lbl_nodeName);
            flp2.Dock = DockStyle.Fill;
            AddToTableLayout(flp2, 2, 1);

            FlowLayoutPanel flp4 = new FlowLayoutPanel();
            Label label7 = new Label();
            label7.Text = "设备：";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            cmb_device = new ComboBox();
            cmb_device.DataSource = new BusinessLogicLayer.MySystemsBLL().getAllDevice();
            cmb_device.DisplayMember = "deviceName";
            flp4.Controls.Add(label7);
            flp4.Controls.Add(cmb_device);
            flp4.Dock = DockStyle.Fill;
            AddToTableLayout(flp4, 3, 1);

            FlowLayoutPanel flp5 = new FlowLayoutPanel();
            Label label8 = new Label();
            label8.Text = "信号类型：";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            cmb_signalType = new ComboBox();
            cmb_signalType.DataSource = new object[] { "AD", "DA", "DI", "DO" };
            flp5.Controls.Add(label8);
            flp5.Controls.Add(cmb_signalType);
            flp5.Dock = DockStyle.Fill;
            AddToTableLayout(flp5, 4, 1);

            FlowLayoutPanel flp3 = new FlowLayoutPanel();
            Label label6 = new Label();
            label6.Text = "通道号：";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            cmb_chNO = new ComboBox();
            cmb_chNO.DataSource = new object[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            flp3.Controls.Add(label6);
            flp3.Controls.Add(cmb_chNO);
            flp3.Dock = DockStyle.Fill;
            AddToTableLayout(flp3, 5, 1);

            save.Text = "保存";
            AddToTableLayout(save, 6, 1);
            //save.Click += new System.EventHandler(this.save_Click);
        }



        /// <summary>  
        /// 动态布局  
        /// </summary>  
        /// <param name="layoutPanel">布局面板</param>  
        /// <param name="row">行</param>  
        /// <param name="col">列</param>  
        private void DynamicLayout(TableLayoutPanel layoutPanel, int row, int col)
        {
            layoutPanel.RowCount = row;    //设置分成几行  
            for (int i = 0; i < row; i++)
            {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }
            layoutPanel.ColumnCount = col;    //设置分成几列  
            for (int i = 0; i < col; i++)
            {
                layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
        }

        public void InitLayoutDemo()
        {
            TableLayoutPanel demoLayoutPanel = new TableLayoutPanel();
            demoLayoutPanel.Dock = DockStyle.Fill;
            this.Controls.Add(demoLayoutPanel);
            int row = 3, col = 3;
            DynamicLayout(demoLayoutPanel, row, col);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Button btn = new Button();
                    btn.Text = string.Format("({0},{1})", i, j);
                    btn.Dock = DockStyle.Fill;
                    demoLayoutPanel.Controls.Add(btn);
                    demoLayoutPanel.SetRow(btn, i);
                    demoLayoutPanel.SetColumn(btn, j);
                }
            }
        }  
    }
}
