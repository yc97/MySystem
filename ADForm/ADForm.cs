using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CANModels.ADForm
{
    public struct CanData             //数据结构 存放can帧信息
    {
        public int Sn;                 //序号
        public string SendRecv;        //发送/接收
        public DateTime mTime;         //时间
        public string FrmFormat;       //帧格式
        public string FrmType;         //帧类型
        public string ID;              //ID
        public int ChNo;               //通道号
        public string Data;            //数据区
        public double Value;           //对应的转换值

    }

    public partial class ADForm : Form
    {
        //连接动态库
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375ReadData(UInt32 iIndex, ref Byte oBuffer, ref UInt32 ioLength);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375WriteData(UInt32 iIndex, ref Byte iBuffer, ref UInt32 ioLength);

        int curChanNo;//通道号 
        static int SendTimes = 0;//次数
        DateTime StartTime = new DateTime(); //开始时间标识
        int station_no = 0x10;

        //连接数据库所用对象和变量
        OleDbConnection mConn = new OleDbConnection();
        OleDbDataAdapter mAdapter;
        OleDbCommandBuilder mBuilder;
        DataTable mDatatable = new DataTable();

        public ADForm(int chNO)
        {
            this.curChanNo = chNO;

            InitializeComponent();

            LISTVIEW_DATA.Columns.Add("次数");
            LISTVIEW_DATA.Columns[0].Width = 60;
            LISTVIEW_DATA.Columns.Add("发送/接收");
            LISTVIEW_DATA.Columns[1].Width = 70;
            LISTVIEW_DATA.Columns.Add("时间(hh:mm:ss:uu)");
            LISTVIEW_DATA.Columns[2].Width = 115;
            LISTVIEW_DATA.Columns.Add("帧格式");
            LISTVIEW_DATA.Columns[3].Width = 60;
            LISTVIEW_DATA.Columns.Add("帧类型");
            LISTVIEW_DATA.Columns[4].Width = 60;
            LISTVIEW_DATA.Columns.Add("ID信息");
            LISTVIEW_DATA.Columns[5].Width = 80;
            LISTVIEW_DATA.Columns.Add("数据区");
            LISTVIEW_DATA.Columns[6].Width = 240;

            // waveformPlot1
            // 
            waveformPlot1.XAxis.Mode = NationalInstruments.UI.AxisMode.StripChart;
            waveformPlot1.YAxis.Mode = NationalInstruments.UI.AxisMode.AutoScaleExact;
        }

        //将数据添加到listview中进行显示
        private void AddDataToListView(CanData mCanData)
        {
            int pos;
            pos = mCanData.Sn - 1;
            LISTVIEW_DATA.Items.Add(System.Convert.ToString(mCanData.Sn));
            LISTVIEW_DATA.EnsureVisible(pos);
            LISTVIEW_DATA.Items[pos].SubItems.Add(mCanData.SendRecv);
            LISTVIEW_DATA.Items[pos].SubItems.Add(mCanData.mTime.ToLongTimeString().ToString() + ":" + mCanData.mTime.Millisecond.ToString());
            LISTVIEW_DATA.Items[pos].SubItems.Add(mCanData.FrmFormat);
            LISTVIEW_DATA.Items[pos].SubItems.Add(mCanData.FrmType);
            LISTVIEW_DATA.Items[pos].SubItems.Add(mCanData.ID);
            LISTVIEW_DATA.Items[pos].SubItems.Add(mCanData.Data);
        }

        //发送数据，供调用
        private void OnSendData(Byte[] SendBuffer, Byte[] RecvBuffer, UInt32 mlength)
        {
            UInt32 DevId = 0;
            double dac;

            if (!CH375WriteData(DevId, ref SendBuffer[0], ref mlength))		//发出CAN帧
                lbl_Status.Text = "K7120传送指令失败,请检查连接!";
            else
            {
                lbl_Status.Text = "发送成功,等待接收!";
                SendTimes++;
                if (SendTimes == 1)
                    StartTime = DateTime.Now;

                CanData mCanData = new CanData();
                mCanData.Sn = SendTimes;
                mCanData.SendRecv = "发送";
                mCanData.mTime = DateTime.Now;
                mCanData.FrmFormat = "标准帧";
                mCanData.FrmType = "数据帧";
                mCanData.ID = System.Convert.ToString(SendBuffer[1], 16) + System.Convert.ToString(SendBuffer[2], 16);

                if ((SendBuffer[1] & 0xf0) == 0x50)
                {
                    mCanData.Data = System.Convert.ToString(SendBuffer[3], 16);
                    mlength = 6;//指定回读单通道字节长度
                }

                AddDataToListView(mCanData);//添加数据到listview，以显示


                if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mlength))   //接收can帧
                    lbl_Status.Text = "K7120接收指令失败,请检查连接!";
                else
                {

                    if ((RecvBuffer[1] & 0xf0) == 0x50)          //判断返回帧是否是单通道数据内容
                    {
                        dac = (RecvBuffer[5] * 256 + RecvBuffer[4]) / 4095.0 * 5;	//转换电压值
                        lbl_Status.Text = "通道" + RecvBuffer[3] + "的电压是: " + System.Convert.ToString(dac) + "V";
                        //cmb_ChNO.Text = RecvBuffer[3].ToString();
                        txt_Valtage.Text = System.Convert.ToString(dac);

                        SendTimes++;
                        mCanData.Sn = SendTimes;
                        mCanData.SendRecv = "接收";
                        mCanData.mTime = DateTime.Now;
                        mCanData.ID = System.Convert.ToString(RecvBuffer[1], 16) + System.Convert.ToString(RecvBuffer[2], 16);
                        mCanData.Data = System.Convert.ToString(RecvBuffer[3], 16) + "   "
                                            + System.Convert.ToString(RecvBuffer[4], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[5], 16);
                        AddDataToListView(mCanData);//添加数据到listview，以显示
                        mCanData.ChNo = RecvBuffer[3] + 1;
                        mCanData.Value = dac;
                        waveformGraph1.PlotY(dac);
                    }
                    
                    else
                    {
                        lbl_Status.Text = "接收到错误的字符串,请检查发送字符是否正确!"; return;
                    }
                }
            }


        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            string strtmp = btn_Start.Text;
            if (strtmp == "开始")
            {
                btn_Start.Text = "停止";

                timer1.Interval = System.Convert.ToInt32(txt_Interval.Text);
                    timer1.Start();
            }
            else
            {   //停止发送
                btn_Start.Text = "开始";
                timer1.Stop();
            }
        }

        //定时器发送数据
        private void timer1_Tick(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32];

            int func = 0x5;						//回读单通道模拟量输入指令为十六进制0X05
            int chan = cmb_ChNO.SelectedIndex;				//指定通道号为1,通道号为1－8
            UInt32 mlength = 4;
            SendBuffer[0] = 1;                              //字节0表示数据场长度，此处为1
            SendBuffer[3] = System.Convert.ToByte(chan);    //字节3为通道号，减1是因为模块内通道号表示为0－7

            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);			//转换指令和站址的格式

            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //字节1，2包含指令和站址
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);

            OnSendData(SendBuffer, RecvBuffer, mlength);
        }

        private void ADForm_Load(object sender, EventArgs e)
        {
            mConn.ConnectionString = @"Provider= Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + @"\IdList.mdb ";
            mConn.Open();
            mAdapter = new OleDbDataAdapter("Select*From [IdList] where [mdlname]='K8512'", mConn);
            mBuilder = new OleDbCommandBuilder(mAdapter);
            mAdapter.Fill(mDatatable);
            DataRow mrow;
            mrow = mDatatable.Rows[0];
            string tmp = Convert.ToString(mrow["mdlid"]);
            int id = System.Convert.ToInt32(tmp, 16);
            station_no = id;
            mConn.Close();
            mConn.Dispose();
            if (-1 == station_no)
            {
                MessageBox.Show("未获取到模块地址！"); this.Close();
            }
        }

    }
}
