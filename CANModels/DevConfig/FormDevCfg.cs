using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;

namespace CANModels.DevConfig
{
    public partial class FormDevCfg : Form
    {
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375ReadData(UInt32 iIndex, ref Byte oBuffer, ref UInt32 ioLength);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375WriteData(UInt32 iIndex, ref Byte iBuffer, ref UInt32 ioLength);

        OleDbConnection mConn = new OleDbConnection();
        DataTable mDatatable = new DataTable();

        static FileStream fs;
        static StreamWriter sw;
        string log;


        Byte[] SendBuffer = new Byte[32];
    
        public FormDevCfg()
        {
            InitializeComponent();

            this.MaximizeBox = false;
        }

        private void FormDevCfg_Load(object sender, EventArgs e)
        {
            CMB_DEV_INDEX.SelectedIndex = 0;

            mConn.ConnectionString = @"Provider= Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + @"\IdList.mdb ";
            mConn.Open();

            fs = new FileStream(Application.StartupPath + @"\cache\log.txt", FileMode.Append);
            sw = new StreamWriter(fs, Encoding.Default);
            log = "模块配置" + "  " + DateTime.Now.ToString() + "  " + "打开数据库： IdList.mdb" + "\r\n";
            sw.WriteLine(log);
        }

        private void SaveId(int mdlindex,string id)
        {            
            string str = "UPDATE [IdList] set [mdlid]= '" + id + "' where [ID]=" + mdlindex + "";
            OleDbCommand inst = new OleDbCommand(str, mConn);

            if (1 != inst.ExecuteNonQuery())
                MessageBox.Show("录入数据失败！");
            else
            {
                log = "模块配置" + "  " + DateTime.Now.ToString() + "  " + "向数据库发送命令：" + str + "\r\n";
                sw.WriteLine(log);
            }
        }

        private void BT_SCAN_Click(object sender, EventArgs e)
        {
            int i;
            int mdlNum=0;
            for (i = 0x00; i <= 0x4f; i++) 
            {
                if (1 == ReadCof(i))
                {
                    mdlNum += 1;
                    log = "模块配置" + "  " + DateTime.Now.ToString() + "  " + "发送回读模块信息指令成功！" + "\r\n";
                    sw.WriteLine(log);

                    log = "模块配置" + "  " + DateTime.Now.ToString() + "  " + "收到模块配置信息，模块ID：" + System.Convert.ToString(i, 16) + "\r\n";
                    sw.WriteLine(log);
                }
                else if (-2 == ReadCof(i))
                    return;
                progressBar1.Value = i;
            }
            EDIT_MDL_NUM.Text = System.Convert.ToString(mdlNum);

            if (EDIT_ID1.Text!="")
                SaveId(1, EDIT_ID1.Text);
            if (EDIT_ID2.Text != "")
                SaveId(2, EDIT_ID2.Text);
            if (EDIT_ID3.Text != "")
                SaveId(3, EDIT_ID3.Text);
            if (EDIT_ID4.Text != "")
                SaveId(4, EDIT_ID4.Text);
        }

        private int ReadCof(int id)//根据地址，发送回读配置指令，获取模块型号
        {
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);
            UInt32 mLength;
            Byte[] RecvBuffer = new Byte[32];
            byte DevType;
            
            int station_no = id;									//
            int func = 0x3;										//读模块配置参数指令为十六进制0X03
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);				//转换指令和站址的格式

            SendBuffer[0] = 0;                                              //字节0表示数据场长度，此处为0
            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //字节1，2包含指令和站址
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);
            mLength = 3;									                //指定下发帧长度为3个字节

            if (!CH375WriteData(DevId, ref SendBuffer[0], ref mLength))
            {
                EDIT_STATE.Text = "K7120发送回读请求失败,请检查连接! ID = " + System.Convert.ToString(station_no);
                return -2;
             }
             else
            {
                mLength = 11;
                if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mLength))
                    return 0;
                else
                {
                    DevType = RecvBuffer[10];
                    switch (DevType)
                    {
                        case 0x0C:
                            {
                                EDIT_ID1.Text = System.Convert.ToString(station_no, 16);
                                EDIT_STATE.Text = "获取地址成功";
                            } return 1;
                        case 0x0E:
                            {
                                EDIT_ID2.Text = System.Convert.ToString(station_no, 16);
                                EDIT_STATE.Text = "获取地址成功";
                            } return 1;
                         case 0x10:
                            {
                                EDIT_ID3.Text = System.Convert.ToString(station_no, 16);
                                EDIT_STATE.Text = "获取地址成功";
                            } return 1; 
                         case 0x16:
                            {
                                EDIT_ID4.Text = System.Convert.ToString(station_no, 16);
                                EDIT_STATE.Text = "获取地址成功";
                            } return 1;
                         default:  RecvBuffer=null; return 0;
                        }
                    }
                }
            
        }

        private void BT_RECOF1_Click(object sender, EventArgs e)//重新配置模块
        {
            {
                MessageBox.Show("模块地址已经配置完成，不允许修改！");
                return;
            }
            int oldId;
            String newId;
            oldId = System.Convert.ToInt32(EDIT_ID1.Text, 16);
            newId = CMB_ID1.SelectedItem.ToString();
            OnSendData(oldId, newId);
            SaveId(1, newId);
        }

        private void BT_RECOF2_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("模块地址已经配置完成，不允许修改！");
                return;
            }
            int oldId;
            String newId;
            oldId = System.Convert.ToInt32(EDIT_ID2.Text, 16);
            newId = CMB_ID2.SelectedItem.ToString();
            OnSendData(oldId, newId);
            SaveId(2, newId);
        }
        private void BT_RECOF3_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("模块地址已经配置完成，不允许修改！");
                return;
            }
            int oldId;
            String newId;
            oldId = System.Convert.ToInt32(EDIT_ID3.Text, 16);
            newId = CMB_ID3.SelectedItem.ToString();
            OnSendData(oldId, newId);
            SaveId(3, newId);
        }

        private void BT_RECOF4_Click(object sender, EventArgs e)
        {
            {
                MessageBox.Show("模块地址已经配置完成，不允许修改！");
                return;
            }
            int oldId;
            String newId;
            oldId = System.Convert.ToInt32(EDIT_ID4.Text, 16);
            newId = CMB_ID4.SelectedItem.ToString();
            OnSendData(oldId, newId);
            SaveId(4, newId);
        }

        private void OnSendData(  int oldId,String newId)
        {
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);
            UInt32 mLength;

            SendBuffer[0] = 0x07;
            SendBuffer[4] = 0x92;
            SendBuffer[5] = 0x00;
            SendBuffer[6] = 0xC7;
            SendBuffer[7] = 0x00;
            SendBuffer[8] = 0x00;
            SendBuffer[9] = 0x00;

            int station_no = oldId;				              //站址
            int func = 0x2;										//模块配置参数指令为十六进制0X02
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);				//转换指令和站址的格式          
            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //字节1，2包含指令和站址
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);
            SendBuffer[3] = System.Convert.ToByte(newId, 16);   //字节3为新配站址

            mLength = 10;

            if (!CH375WriteData(DevId, ref SendBuffer[0], ref mLength))
            { EDIT_STATE.Text = "K7120发送广播请求失败,请检查连接!"; return; }
            else
            {
                EDIT_STATE.Text = "配置成功，请重新上电，然后刷新查看新站址";
                log = "模块配置" + "  " + DateTime.Now.ToString() + "  " + "重新配置模块成功！ 原模块ID：" + oldId + " 新模块ID：" + newId + "\r\n";
                sw.WriteLine(log);
            }
        }

        private void FormDevCfg_FormClosing(object sender, FormClosingEventArgs e)
        {
            mConn.Close();
            mConn.Dispose();

            log = "模块配置" + "  " + DateTime.Now.ToString() + "  " + "关闭数据库： IdList.mdb" + "\r\n";
            sw.WriteLine(log);

            sw.Close();
            fs.Close();
        }



    }
}
