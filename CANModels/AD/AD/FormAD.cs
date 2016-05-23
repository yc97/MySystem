using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;

namespace AD
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

    public partial class FormAD : Form
    {
        //连接动态库
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375ReadData(UInt32 iIndex, ref Byte oBuffer, ref UInt32 ioLength);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375WriteData(UInt32 iIndex, ref Byte iBuffer, ref UInt32 ioLength);

        //连接数据库所用对象和变量
        OleDbConnection mConn = new OleDbConnection();
        OleDbDataAdapter mAdapter;
        OleDbCommandBuilder mBuilder;
        DataTable mDatatable = new DataTable();

        //全局变量定义
        int station_no =-1; //模块站址初始值
        static int SendTimes = 0;//时间标识
        UInt32 mlength;      //can帧数据场长度
        int curChanNo = 0;   //图表中当前通道号
        DateTime StartTime = new DateTime(); //开始时间标识
        double mPreTime = 0;

        public FormAD()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            
            //初始化
            EDIT_CH1.Text = "0";
            EDIT_CH2.Text = "0";
            EDIT_CH3.Text = "0";
            EDIT_CH4.Text = "0";
            EDIT_CH5.Enabled = false;
            EDIT_CH6.Enabled = false;
            EDIT_CH7.Enabled = false;
            EDIT_CH8.Enabled = false;

       
            CMB_DEV_INDEX.SelectedIndex = 0;
            CMB_FRM_FORMAT.SelectedIndex = 0;
            CMB_FRM_FORMAT.Enabled = false;
            CMB_FRM_TYPE.SelectedIndex = 0;
            CMB_FRM_TYPE.Enabled = false;


            EDIT_DLC.Text = "01";
            EDIT_FUN.Text = "0101";
            EDIT_ADD.Text ="01";
            EDIT_DATA.Text = "00";

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

            //初始化Teechart
            axTChart1.Panel.Gradient.StartColor = 0x00ffffff;
            axTChart1.Panel.Gradient.EndColor = 0x008f8f8f;
            axTChart1.Panel.Gradient.Visible = true;

            axTChart1.Header.Visible = false;

            axTChart1.Legend.LegendStyle = TeeChart.ELegendStyle.lsLastValues;
            axTChart1.Legend.Visible = true;

            axTChart1.Axis.Bottom.Automatic = false;
            axTChart1.Axis.Bottom.AutomaticMinimum = false;
            axTChart1.Axis.Bottom.AutomaticMaximum = false;
            axTChart1.Axis.Bottom.Maximum = 10000;
            axTChart1.Axis.Bottom.Minimum = 0;
            axTChart1.Axis.Bottom.Title.Caption = "时间/ms";
            axTChart1.Axis.Bottom.Title.Visible = true;

            axTChart1.Axis.Left.Automatic = false;
            axTChart1.Axis.Left.Maximum = 5;
            axTChart1.Axis.Left.Minimum = 0;
            axTChart1.Axis.Left.Title.Caption = "电压/V";
            axTChart1.Axis.Left.Title.Visible = true;


            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);            
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);             
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            
            axTChart1.Series(0).Title = "通道1";
            axTChart1.Series(1).Title = "通道2";
            axTChart1.Series(2).Title = "通道3";
            axTChart1.Series(3).Title = "通道4";
            /*axTChart1.Series(4).Title = "通道5";
            axTChart1.Series(5).Title = "通道6";
            axTChart1.Series(6).Title = "通道7";
           axTChart1.Series(7).Title = "通道8";*/
            
        }

        //回读模块配置信息
        private void BT_RD_CFG_Click(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32];
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);

            UInt32 mLength;
           
           							
            int func = 0x3;										//读模块配置参数指令为十六进制0X03
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);				//转换指令和站址的格式


            SendBuffer[0] = 0;                                              //字节0表示数据场长度，此处为0
            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //字节1，2包含指令和站址
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);
            mLength = 3;									                //指定下发帧长度为3个字节

            if (!CH375WriteData(DevId, ref SendBuffer[0], ref mLength))//发送回读配置请求
                EDIT_STATE.Text = "K7120发送请求失败,请检查连接!";
            else 
            {
                mLength = 11;
                if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mLength))//接收配置信息
                    EDIT_STATE.Text = "K7120接收配置信息失败!请检查连接!";
                else 
                {   //显示回读字符串
                    if (mLength !=0)
		            {
                        EDIT_STATE.Text = "回读字符串：  " + System.Convert.ToString(RecvBuffer[3], 16) + "  "
                                                +System.Convert.ToString(RecvBuffer[4], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[5], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[6], 16) + "  "
                                                + System.Convert.ToString(RecvBuffer[7], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[8], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[9], 16) + "  "
                                                + System.Convert.ToString(RecvBuffer[10], 16);
                     } 
                 }
                   
            }
  
        }

        //手动发送数据，数据全部自定义
        private void BT_SEDN_MANUAL_Click(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32]; 
       								
            int func = System.Convert.ToInt32(EDIT_FUN.Text,2);				    //设置指令
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);	//转换指令和站址的格式

            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //字节1，2包含指令和站址
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);

            if (EDIT_DLC.Text == "00")
            {
                EDIT_STATE.Text = "请求全通道数据!";
                SendBuffer[0] = 0;									           //字节0表示数据场长度，此处为1
                mlength = 3;									//指定下发帧长度为4个字节
            }
            else if (EDIT_DLC.Text == "01")
            {
                EDIT_STATE.Text = "请求单通道数据!";
                byte chan = System.Convert.ToByte (EDIT_DATA.Text); //通道号,通道号为01－08
                SendBuffer[0] = 1;									           //字节0表示数据场长度，此处为1
                SendBuffer[3] = chan;							//字节3为通道号
                mlength = 4;
            }
            else 
            {
                EDIT_STATE.Text = "错误的数据场长度,请检查DLC字符是否正确!"; return; 
            }
            OnSendData(SendBuffer,RecvBuffer);    		
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
        //将数据添加到teechart中显示
        private void TeeChartCurveShow(CanData mCanData)
        {
            double X_Val = System.Convert.ToDouble(mCanData.mTime.TimeOfDay.TotalMilliseconds) -
                           System.Convert.ToDouble(StartTime.TimeOfDay.TotalMilliseconds);

            axTChart1.Series(mCanData.ChNo - 1).AddXY(X_Val, System.Convert.ToDouble(string.Format("{0:0.000}", mCanData.Value)), null, 0);

            if (SendTimes == 1)
            {
                mPreTime = 0;
            }
            if (X_Val > axTChart1.Axis.Bottom.Maximum)
            {
                axTChart1.Axis.Bottom.Scroll(X_Val - mPreTime, false);
            }

            mPreTime = X_Val;
        }
        //清除界面上的数据
        private void BT_CLEAR_Click(object sender, EventArgs e)
        {
            LISTVIEW_DATA.Items.Clear();
            SendTimes = 0;
            EDIT_CH1.Text = "0";
            EDIT_CH2.Text = "0";
            EDIT_CH3.Text = "0";
            EDIT_CH4.Text = "0";
            EDIT_STATE.Text = "";

            axTChart1.RemoveAllSeries();
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);

            axTChart1.Series(0).Title = "通道1";
            axTChart1.Series(1).Title = "通道2";
            axTChart1.Series(2).Title = "通道3";
            axTChart1.Series(3).Title = "通道4";

            RADIO_CHART_SIGNLE_CH_CheckedChanged(sender, e);
        }
                  
        private void BT_SEND_CYCLE_Click(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32];
            string strtmp = BT_SEND_CYCLE.Text;
            if (strtmp == "自动发送")
            {
                //判断是否为周期发送，如果是直接调用定时器发送数据，否则发送一次数据
                if (CHK_CYCLE.Checked)
                {
                    BT_SEND_CYCLE.Text = "停止发送";
                    CHK_CYCLE.Enabled = false;
                    EDIT_SEND_INTVAL.Enabled = false;

                    timer1.Interval = System.Convert.ToInt32(EDIT_SEND_INTVAL.Text);
                    timer1.Start();
                }
                else
                {
                    //判断单通道数据 or 全通道数据，并设置发送数据帧格式					
                    int func;

                    if (RADIO_SINGLE.Checked)
                    {
                        func = 0x5;										//回读单通道模拟量输入指令为十六进制0X05
                        mlength = 4;
                        int chan = CMB_CHNO.SelectedIndex;				//指定通道号为1,通道号为1－8
                        SendBuffer[0] = 1;                              //字节0表示数据场长度，此处为1
                        SendBuffer[3] = System.Convert.ToByte(chan);    //字节3为通道号，减1是因为模块内通道号表示为0－7
                    }
                    else
                    {
                        func = 0x6;										//回读多通道模拟量输入指令为十六进制0X06
                        mlength = 3;
                        SendBuffer[0] = 0;                              //字节0表示数据场长度，此处为0
                    }

                    int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);			//转换指令和站址的格式

                    SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //字节1，2包含指令和站址
                    SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);

                    OnSendData(SendBuffer, RecvBuffer);//调用方法发送数据
                }
            }
            else
            {   //停止发送
                BT_SEND_CYCLE.Text = "自动发送";
                CHK_CYCLE.Enabled = true;
                EDIT_SEND_INTVAL.Enabled = true;

                timer1.Stop();
            }
        }

        //发送数据，供调用
        private void OnSendData(Byte[] SendBuffer,Byte[] RecvBuffer)
        {
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);
            double dac;

            if (!CH375WriteData(DevId, ref SendBuffer[0], ref mlength))		//发出CAN帧
                EDIT_STATE.Text = "K7120传送指令失败,请检查连接!";
            else 
            {
                EDIT_STATE.Text = "发送成功,等待接收!";
                SendTimes++;
                if (SendTimes == 1)
                    StartTime = DateTime.Now;

                CanData mCanData = new CanData();
                mCanData.Sn = SendTimes;
                mCanData.SendRecv = "发送";
                mCanData.mTime = DateTime.Now;
                mCanData.FrmFormat = CMB_FRM_FORMAT.SelectedItem.ToString();
                mCanData.FrmType = CMB_FRM_TYPE.SelectedItem.ToString();
                mCanData.ID = System.Convert.ToString (SendBuffer[1],16) + System.Convert.ToString(SendBuffer[2],16);
                
                if ((SendBuffer[1] & 0xf0) == 0x50)
                {
                    mCanData.Data = System.Convert.ToString(SendBuffer[3], 16);
                    mlength = 6;//指定回读单通道字节长度
                }
                else
                {
                    mCanData.Data = "";
                    mlength = 11;//指定回读全通道字节长度
                }
                AddDataToListView(mCanData);//添加数据到listview，以显示
                
               										
                if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mlength))   //接收can帧
                    EDIT_STATE.Text = "K7120接收指令失败,请检查连接!";
                else
                {
                    
                    if ((RecvBuffer[1] & 0xf0) == 0x50)          //判断返回帧是否是单通道数据内容
                    {
                        dac = (RecvBuffer[5] * 256 + RecvBuffer[4]) / 4095.0 * 5;	//转换电压值
                        EDIT_STATE.Text = "通道" + RecvBuffer[3] + "的电压是: " + System.Convert.ToString(dac) + "V";
                        switch (RecvBuffer[3])
                        {
                            case (00): { EDIT_CH1.Text = System.Convert.ToString(dac); break; }
                            case (01): { EDIT_CH2.Text = System.Convert.ToString(dac); break; }
                            case (02): { EDIT_CH3.Text = System.Convert.ToString(dac); break; }
                            case (03): { EDIT_CH4.Text = System.Convert.ToString(dac); break; }
                            case (04): { EDIT_CH5.Text = System.Convert.ToString(dac); break; }
                            case (05): { EDIT_CH6.Text = System.Convert.ToString(dac); break; }
                            case (06): { EDIT_CH7.Text = System.Convert.ToString(dac); break; }
                            case (07): { EDIT_CH8.Text = System.Convert.ToString(dac); break; }
                        }

                        SendTimes++;
                        mCanData.Sn = SendTimes;
                        mCanData.SendRecv = "接收";
                        mCanData.mTime = DateTime.Now;
                        mCanData.ID = System.Convert.ToString(RecvBuffer[1], 16) + System.Convert.ToString(RecvBuffer[2], 16);
                        mCanData.Data = System.Convert.ToString(RecvBuffer[3], 16) + "   "
                                            + System.Convert.ToString(RecvBuffer[4], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[5], 16);
                        AddDataToListView(mCanData);//添加数据到listview，以显示
                        mCanData.ChNo = RecvBuffer[3]+1;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                    }
                    else if ((RecvBuffer[1] & 0xf0) == 0x60)
                    {
                        EDIT_STATE.Text = "第一帧数据接收成功！";
                        
                        SendTimes++;
                        mCanData.Sn = SendTimes;
                        mCanData.SendRecv = "接收";
                        mCanData.mTime = DateTime.Now;
                        mCanData.ID = System.Convert.ToString(RecvBuffer[1], 16) + System.Convert.ToString(RecvBuffer[2], 16);
                        mCanData.Data = System.Convert.ToString(RecvBuffer[3], 16) + " "
                                            + System.Convert.ToString(RecvBuffer[4], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[5], 16) + " "
                                                    + System.Convert.ToString(RecvBuffer[6], 16) + " "
                                                        + System.Convert.ToString(RecvBuffer[7], 16) + " "
                                                            + System.Convert.ToString(RecvBuffer[8], 16) + " "
                                                                + System.Convert.ToString(RecvBuffer[9], 16) + " "
                                                                    + System.Convert.ToString(RecvBuffer[10], 16);
                        AddDataToListView(mCanData); //添加数据到listview，以显示

                        dac = (RecvBuffer[4] * 256 + RecvBuffer[3]) / 4095.0 * 5;	//转换电压值
                        EDIT_CH1.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 1;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                        dac = (RecvBuffer[6] * 256 + RecvBuffer[5]) / 4095.0 * 5;	//转换电压值
                        EDIT_CH2.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 2;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                        dac = (RecvBuffer[8] * 256 + RecvBuffer[7]) / 4095.0 * 5;	//转换电压值
                        EDIT_CH3.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 3;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                        dac = (RecvBuffer[10] * 256 + RecvBuffer[9]) / 4095.0 * 5;	//转换电压值
                        EDIT_CH4.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 4;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);


                        if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mlength))   //接收can帧
                            EDIT_STATE.Text = "K7120接收指令失败,请检查连接!";
                        else if ((RecvBuffer[1] & 0xf0) == 0x60)
                        {
                            EDIT_STATE.Text = "全部数据接收成功！";

                            SendTimes++;
                            mCanData.Sn = SendTimes;
                            mCanData.SendRecv = "接收";
                            mCanData.mTime = DateTime.Now;
                            mCanData.ID = System.Convert.ToString(RecvBuffer[1], 16) + System.Convert.ToString(RecvBuffer[2], 16);
                            mCanData.Data = System.Convert.ToString(RecvBuffer[3], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[4], 16) + " "
                                                    + System.Convert.ToString(RecvBuffer[5], 16) + " "
                                                        + System.Convert.ToString(RecvBuffer[6], 16) + " "
                                                            + System.Convert.ToString(RecvBuffer[7], 16) + " "
                                                                + System.Convert.ToString(RecvBuffer[8], 16) + " "
                                                                    + System.Convert.ToString(RecvBuffer[9], 16) + " "
                                                                        + System.Convert.ToString(RecvBuffer[10], 16);
                            AddDataToListView(mCanData); //添加数据到listview，以显示
                        }
                    }
                    else 
                    {
                        EDIT_STATE.Text = "接收到错误的字符串,请检查发送字符是否正确!"; return;
                    }
                }
            }
         		

        }
        
 
       
        //定时器发送数据
        private void timer1_Tick(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32];						
            int func;

            if (RADIO_SINGLE.Checked)
            {
                func = 0x5;										//回读单通道模拟量输入指令为十六进制0X05
                int chan = CMB_CHNO.SelectedIndex;				//指定通道号为1,通道号为1－8
                mlength = 4;
                SendBuffer[0] = 1;                              //字节0表示数据场长度，此处为1
                SendBuffer[3] = System.Convert.ToByte(chan);    //字节3为通道号，减1是因为模块内通道号表示为0－7
            }
            else
            {
                func = 0x6;										//回读多通道模拟量输入指令为十六进制0X06
                mlength = 3;
                SendBuffer[0] = 0;                              //字节0表示数据场长度，此处为0
            }

           
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);			//转换指令和站址的格式

            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //字节1，2包含指令和站址
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);


            OnSendData(SendBuffer, RecvBuffer);
        }

        private void RADIO_CHART_SIGNLE_CH_CheckedChanged(object sender, EventArgs e)
        {

            if (RADIO_CHART_ALL_CH.Checked)
            {
                BT_CHART_FRONT_CH.Enabled = false;
                BT_CHART_NEXT_CH.Enabled = false;

                axTChart1.Series(0).Active = true;
                axTChart1.Series(1).Active = true;
                axTChart1.Series(2).Active = true;
                axTChart1.Series(3).Active = true;

            }
            else
            {
                BT_CHART_FRONT_CH.Enabled = true;
                BT_CHART_NEXT_CH.Enabled = true;

                axTChart1.Series(0).Active = false;
                axTChart1.Series(1).Active = false;
                axTChart1.Series(2).Active = false;
                axTChart1.Series(3).Active = false;
                

                switch (curChanNo)
                {
                    case 0:
                        LABEL_CHART_CH.Text = "CH1";
                        axTChart1.Series(0).Active = true;
                        break;
                    case 1:
                        LABEL_CHART_CH.Text = "CH2";
                        axTChart1.Series(1).Active = true;
                        break;
                    case 2:
                        LABEL_CHART_CH.Text = "CH3";
                        axTChart1.Series(2).Active = true;
                        break;
                    case 3:
                        LABEL_CHART_CH.Text = "CH4";
                        axTChart1.Series(3).Active = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void RADIO_MULTIPLE_CheckedChanged(object sender, EventArgs e)
        {
            CMB_CHNO.Enabled = false;
        }

        private void RADIO_SINGLE_CheckedChanged(object sender, EventArgs e)
        {
            CMB_CHNO.Enabled = true;
        }

        private void BT_CHART_FRONT_CH_Click(object sender, EventArgs e)
        {
            if (curChanNo == 0)
                //curChanNo = 7;
                curChanNo = 3;
            else
                curChanNo--;

            axTChart1.Series(0).Active = false;
            axTChart1.Series(1).Active = false;
            axTChart1.Series(2).Active = false;
            axTChart1.Series(3).Active = false;

            switch (curChanNo)
            {
                case 0:
                    LABEL_CHART_CH.Text = "CH1";
                    axTChart1.Series(0).Active = true;
                    break;
                case 1:
                    LABEL_CHART_CH.Text = "CH2";
                    axTChart1.Series(1).Active = true;
                    break;
                case 2:
                    LABEL_CHART_CH.Text = "CH3";
                    axTChart1.Series(2).Active = true;
                    break;
                case 3:
                    LABEL_CHART_CH.Text = "CH4";
                    axTChart1.Series(3).Active = true;
                    break;
                default:
                    break;
            }
        }

        private void BT_CHART_NEXT_CH_Click(object sender, EventArgs e)
        {
            //if (curChanNo == 7)
            if (curChanNo == 3)
                curChanNo = 0;
            else
                curChanNo++;

            axTChart1.Series(0).Active = false;
            axTChart1.Series(1).Active = false;
            axTChart1.Series(2).Active = false;
            axTChart1.Series(3).Active = false;

            switch (curChanNo)
            {
                case 0:
                    LABEL_CHART_CH.Text = "CH1";
                    axTChart1.Series(0).Active = true;
                    break;
                case 1:
                    LABEL_CHART_CH.Text = "CH2";
                    axTChart1.Series(1).Active = true;
                    break;
                case 2:
                    LABEL_CHART_CH.Text = "CH3";
                    axTChart1.Series(2).Active = true;
                    break;
                case 3:
                    LABEL_CHART_CH.Text = "CH4";
                    axTChart1.Series(3).Active = true;
                    break;
                default:
                    break;
            }
        }

        //模块加载，连接数据库，获得模块地址
        private void FormAD_Load(object sender, EventArgs e)
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