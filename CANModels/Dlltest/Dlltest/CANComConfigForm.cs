using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace CANModels.ComConfig
{
    public delegate void CallBackCH375IntRoutine(ref Byte RecvBuff);  //定义委托函数类型

    public partial class CANComConfigForm : Form
    {
        [DllImport("CH375DLL.DLL")]
        static extern IntPtr CH375OpenDevice(UInt32 iIndex);
        [DllImport("CH375DLL.DLL")]
        static extern int CH375CloseDevice(UInt32 iIndex);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375ResetDevice(UInt32 iIndex);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375ReadData(UInt32 iIndex, ref Byte oBuffer, ref UInt32 ioLength);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375WriteData(UInt32 iIndex, ref Byte iBuffer, ref UInt32 ioLength);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375SetTimeout(UInt32 iIndex, UInt32 iWriteTimeout, UInt32 iReadTimeout);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375SetExclusive(UInt32 iIndex, UInt32 iExclusive);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375SetIntRoutine(UInt32 iIndex, CallBackCH375IntRoutine iIntRoutine);

        static FileStream fs;
        static StreamWriter sw;
        string log;

        public CANComConfigForm()
        {
            InitializeComponent();

            this.MaximizeBox = false;

            this.StartPosition = FormStartPosition.CenterScreen;

            //初始化显示界面
            EDIT_BTR0.Text = "18";
            EDIT_BTR1.Text = "1C";
            EDIT_ACR0.Text = "00000000";
            EDIT_AMR0.Text = "FFFFFFFF";

            CMB_DEV_INDEX.SelectedIndex = 0;
                        
            EDIT_STATE.Text = "";

            //禁止按钮动作
          
            BT_RESET.Enabled = false;
            BT_CONVERT.Enabled = false;

            BT_CFG.Enabled = false;
            BT_CFG_READ.Enabled = false;

        }

        private void BT_OPEN_Click(object sender, EventArgs e)
        {
            if (BT_OPEN.Text == "打开设备")
            {
                IntPtr DevHandle = new IntPtr();
                UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);
                DevHandle = CH375OpenDevice(DevId);

                if ((0 == (int)DevHandle) || (-1 == (int)DevHandle))
                {
                    EDIT_STATE.Text = "无法打开设备！";

                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 无法打开设备！" + "\r\n";
                    sw.WriteLine(log);

                    MessageBox.Show("无法打开设备", "信息提示：");
                    return;
                }
                else
                {
                    CH375SetTimeout(DevId, 0x20, 0x20);	//发送/接收超时设为20ms

                    CH375SetExclusive(DevId, 0);	    //共享使用该设备

                    //设置中断方式；
                    CH375SetIntRoutine(DevId, null);
                   // CH375SetIntRoutine(DevId, iIntRoutine);

                    EDIT_STATE.Text = "打开设备成功！";
                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 打开设备成功！" + "\r\n";
                    sw.WriteLine(log);

                    //禁止按钮动作
                 
                    BT_RESET.Enabled = true;
                    BT_CFG.Enabled = true;
                    BT_CFG_READ.Enabled = true;
                    BT_CONVERT.Enabled = true;
                }
                BT_OPEN.Text = "关闭设备";
            }
            else
            {
                UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);
                CH375CloseDevice(DevId);
                EDIT_STATE.Text = "设备已关闭!";
                
                log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 关闭设备成功！"+"\r\n";
                sw.WriteLine(log);

                //禁止按钮动作
               
                BT_OPEN.Enabled = true;
                
                BT_RESET.Enabled = false;
                BT_CONVERT.Enabled = false;

                BT_CFG.Enabled = false;
                BT_CFG_READ.Enabled = false;

                BT_OPEN.Text = "打开设备";
            }
        }

       

        private void BT_RESET_Click(object sender, EventArgs e)
        {
            Byte[] CanConfig = new Byte[2];
            CanConfig[0] = 0x30;
            CanConfig[1] = 0xBB;
            UInt32 mLength = 2;
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);

            if (CH375WriteData(DevId, ref CanConfig[0], ref mLength))
            {
                EDIT_STATE.Text = "设备已复位!";

                log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 已复位！" + "\r\n";
                sw.WriteLine(log);
                
                //禁止按钮动作
                BT_OPEN.Enabled = true;
             
                BT_RESET.Enabled = false;
                BT_CONVERT.Enabled = false;

                BT_CFG.Enabled = false;
                BT_CFG_READ.Enabled = false;

            }
            else
            {
                EDIT_STATE.Text = "设备复位失败!";

                log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 复位失败！" + "\r\n";
                sw.WriteLine(log);

                MessageBox.Show("设备复位失败", "信息提示：");
            }
        }

        private void BT_EXIT_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BT_CFG_Click(object sender, EventArgs e)
        {
            string SubString;

            UInt32 mLength;
            bool bOpen;
            Byte[] CanConfig = new Byte[13];
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);
            CanConfig[0] = 0x30;
            CanConfig[1] = 0xAA;
            CanConfig[2] = System.Convert.ToByte("0x" + EDIT_BTR0.Text, 16);
            CanConfig[3] = System.Convert.ToByte("0x" + EDIT_BTR1.Text, 16);

            SubString = EDIT_ACR0.Text.Substring(0, 2);
            CanConfig[4] = System.Convert.ToByte("0x" + SubString, 16);
            SubString = EDIT_ACR0.Text.Substring(2, 2);
            CanConfig[5] = System.Convert.ToByte("0x" + SubString, 16);
            SubString = EDIT_ACR0.Text.Substring(4, 2);
            CanConfig[6] = System.Convert.ToByte("0x" + SubString, 16);
            SubString = EDIT_ACR0.Text.Substring(6, 2);
            CanConfig[7] = System.Convert.ToByte("0x" + SubString, 16);

            SubString = EDIT_AMR0.Text.Substring(0, 2);
            CanConfig[8] = System.Convert.ToByte("0x" + SubString, 16);
            SubString = EDIT_AMR0.Text.Substring(2, 2);
            CanConfig[9] = System.Convert.ToByte("0x" + SubString, 16);
            SubString = EDIT_AMR0.Text.Substring(4, 2);
            CanConfig[10] = System.Convert.ToByte("0x" + SubString, 16);
            SubString = EDIT_AMR0.Text.Substring(6, 2);
            CanConfig[11] = System.Convert.ToByte("0x" + SubString, 16);

            CanConfig[12] = 0x01;                   //最后一个为1，表示用中断；

            mLength = 13;
            bOpen = CH375WriteData(DevId, ref CanConfig[0], ref mLength);
            if (bOpen && (mLength == 13))
            {
                EDIT_STATE.Text = "配置成功";

                string info = "BTR0: " + EDIT_BTR0.Text + " BTR1: " + EDIT_BTR1.Text + " ACR0: " + EDIT_ACR0.Text + " AMR0: " + EDIT_AMR0.Text+" ";

                log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 配置成功！配置信息："+info + "\r\n";
                sw.WriteLine(log);

            }
            else
            {
                EDIT_STATE.Text = "配置失败";
                log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 配置失败！" + "\r\n";
                sw.WriteLine(log);
            }
        }

        private void BT_CFG_READ_Click(object sender, EventArgs e)
        {
            Byte[] CanConfig = new Byte[13];
            UInt32 mLength;
            string strtmp;
            int i;
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);

            CanConfig[0] = 0x30;
            CanConfig[1] = 0x55;

            mLength = 2;
            if (CH375WriteData(DevId, ref CanConfig[0], ref mLength))
                mLength = 13;
            if (CH375ReadData(DevId, ref CanConfig[0], ref mLength))
            {
                if (mLength != 0)
                {
                    EDIT_BTR0.Text = System.Convert.ToString(CanConfig[2], 16);
                    EDIT_BTR1.Text = System.Convert.ToString(CanConfig[3], 16);

                    strtmp = "";
                    for (i = 4; i < 8; i++)
                    {
                        if (CanConfig[i] > 0x0F)
                            strtmp += System.Convert.ToString(CanConfig[i], 16);
                        else
                            strtmp += "0" + System.Convert.ToString(CanConfig[i], 16);
                    }
                    EDIT_ACR0.Text = strtmp;

                    strtmp = "";
                    for (i = 8; i < 12; i++)
                    {
                        if (CanConfig[i] > 0x0F)
                            strtmp += System.Convert.ToString(CanConfig[i], 16);
                        else
                            strtmp += "0" + System.Convert.ToString(CanConfig[i], 16);
                    }
                    EDIT_AMR0.Text = strtmp;

                    EDIT_STATE.Text = "回读状态正确!";
                    
                    string info = "BTR0: " + EDIT_BTR0.Text + " BTR1: " + EDIT_BTR1.Text + " ACR0: " + EDIT_ACR0.Text + " AMR0: " + EDIT_AMR0.Text + " ";
                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 回读配置成功！配置信息：" +info+ "\r\n";
                    sw.WriteLine(log);
                }
                else
                {
                    EDIT_STATE.Text = "回读状态有误!请检查连接或者重新配置!";
                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 回读配置失败！" + "\r\n";
                    sw.WriteLine(log);
                }
            }
            else
            {
                EDIT_STATE.Text = "回读状态有误!请检查连接或者重新配置!";
                log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 回读配置失败！" + "\r\n";
                sw.WriteLine(log);
            }
        }



        private void BT_CONVERT_Click(object sender, EventArgs e)
        {
            Byte[] CanConfig = new Byte[2];
            UInt32 mLength = 2;
            string strtmp = BT_CONVERT.Text;
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);

            if (strtmp == "启动转换")
            {
                CanConfig[0] = 0x30;
                CanConfig[1] = 0x00;
                if (CH375WriteData(DevId, ref CanConfig[0], ref mLength))
                {
                    EDIT_STATE.Text = "启动成功!";
                    BT_CONVERT.Text = "停止转换";
                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 启动转换！" + "\r\n";
                    sw.WriteLine(log);
                    //禁止按钮动作
                    BT_OPEN.Enabled = false ;
                 
                    BT_RESET.Enabled = true;
                }
                else
                {
                    EDIT_STATE.Text = "启动失败!";
                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 启动转换失败！" + "\r\n";
                    sw.WriteLine(log);
                    MessageBox.Show("启动转换失败!", "信息提示:");
                }
            }
            else
            {
                CanConfig[0] = 0x30;
                CanConfig[1] = 0xFF;
                mLength = 2;
                if (CH375WriteData(DevId, ref CanConfig[0], ref mLength))
                {
                    EDIT_STATE.Text = "停止转换成功!";
                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 停止转换！" + "\r\n";
                    sw.WriteLine(log);
                    BT_CONVERT.Text = "启动转换";
                    
                    BT_OPEN.Enabled = true;
                }
                else
                {
                    EDIT_STATE.Text = "停止转换失败!";
                    log = "通信配置" + "  " + DateTime.Now.ToString() + "  " + "K7120 停止转换失败！" + "\r\n";
                    sw.WriteLine(log);
                    MessageBox.Show("停止转换失败!", "信息提示:");
                }
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            Byte[] CanConfig = new Byte[13];
            UInt32 mLength;
           
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);

            CanConfig[0] = 0x30;
            CanConfig[1] = 0x55;

            mLength = 2;
            if (CH375WriteData(DevId, ref CanConfig[0], ref mLength))
                mLength = 13;
            if (CH375ReadData(DevId, ref CanConfig[0], ref mLength))
            {
                if (mLength != 0)
                {                    
                    BT_RESET.Enabled = true ;
                    BT_CONVERT.Enabled =true;
                    BT_OPEN.Enabled = false;
                    BT_CFG.Enabled = true ;
                    BT_CFG_READ.Enabled = true;
                    BT_CONVERT.Text = "停止转换";
                    BT_OPEN.Text = "关闭设备";
                }
            }

            fs = new FileStream(Application.StartupPath + @"\cache\log.txt", FileMode.Append);
            sw = new StreamWriter(fs, Encoding.Default);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sw.Close();
            fs.Close();
        }




    }
}