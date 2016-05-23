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
    public struct CanData             //���ݽṹ ���can֡��Ϣ
    {
        public int Sn;                 //���
        public string SendRecv;        //����/����
        public DateTime mTime;         //ʱ��
        public string FrmFormat;       //֡��ʽ
        public string FrmType;         //֡����
        public string ID;              //ID
        public int ChNo;               //ͨ����
        public string Data;            //������
        public double Value;           //��Ӧ��ת��ֵ
        
    }

    public partial class FormAD : Form
    {
        //���Ӷ�̬��
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375ReadData(UInt32 iIndex, ref Byte oBuffer, ref UInt32 ioLength);
        [DllImport("CH375DLL.DLL")]
        static extern bool CH375WriteData(UInt32 iIndex, ref Byte iBuffer, ref UInt32 ioLength);

        //�������ݿ����ö���ͱ���
        OleDbConnection mConn = new OleDbConnection();
        OleDbDataAdapter mAdapter;
        OleDbCommandBuilder mBuilder;
        DataTable mDatatable = new DataTable();

        //ȫ�ֱ�������
        int station_no =-1; //ģ��վַ��ʼֵ
        static int SendTimes = 0;//ʱ���ʶ
        UInt32 mlength;      //can֡���ݳ�����
        int curChanNo = 0;   //ͼ���е�ǰͨ����
        DateTime StartTime = new DateTime(); //��ʼʱ���ʶ
        double mPreTime = 0;

        public FormAD()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            
            //��ʼ��
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

            LISTVIEW_DATA.Columns.Add("����");
            LISTVIEW_DATA.Columns[0].Width = 60;
            LISTVIEW_DATA.Columns.Add("����/����");
            LISTVIEW_DATA.Columns[1].Width = 70;
            LISTVIEW_DATA.Columns.Add("ʱ��(hh:mm:ss:uu)");
            LISTVIEW_DATA.Columns[2].Width = 115;
            LISTVIEW_DATA.Columns.Add("֡��ʽ");
            LISTVIEW_DATA.Columns[3].Width = 60;
            LISTVIEW_DATA.Columns.Add("֡����");
            LISTVIEW_DATA.Columns[4].Width = 60;
            LISTVIEW_DATA.Columns.Add("ID��Ϣ");
            LISTVIEW_DATA.Columns[5].Width = 80;
            LISTVIEW_DATA.Columns.Add("������");
            LISTVIEW_DATA.Columns[6].Width = 240;

            //��ʼ��Teechart
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
            axTChart1.Axis.Bottom.Title.Caption = "ʱ��/ms";
            axTChart1.Axis.Bottom.Title.Visible = true;

            axTChart1.Axis.Left.Automatic = false;
            axTChart1.Axis.Left.Maximum = 5;
            axTChart1.Axis.Left.Minimum = 0;
            axTChart1.Axis.Left.Title.Caption = "��ѹ/V";
            axTChart1.Axis.Left.Title.Visible = true;


            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);            
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);             
            //axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
            
            axTChart1.Series(0).Title = "ͨ��1";
            axTChart1.Series(1).Title = "ͨ��2";
            axTChart1.Series(2).Title = "ͨ��3";
            axTChart1.Series(3).Title = "ͨ��4";
            /*axTChart1.Series(4).Title = "ͨ��5";
            axTChart1.Series(5).Title = "ͨ��6";
            axTChart1.Series(6).Title = "ͨ��7";
           axTChart1.Series(7).Title = "ͨ��8";*/
            
        }

        //�ض�ģ��������Ϣ
        private void BT_RD_CFG_Click(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32];
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);

            UInt32 mLength;
           
           							
            int func = 0x3;										//��ģ�����ò���ָ��Ϊʮ������0X03
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);				//ת��ָ���վַ�ĸ�ʽ


            SendBuffer[0] = 0;                                              //�ֽ�0��ʾ���ݳ����ȣ��˴�Ϊ0
            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //�ֽ�1��2����ָ���վַ
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);
            mLength = 3;									                //ָ���·�֡����Ϊ3���ֽ�

            if (!CH375WriteData(DevId, ref SendBuffer[0], ref mLength))//���ͻض���������
                EDIT_STATE.Text = "K7120��������ʧ��,��������!";
            else 
            {
                mLength = 11;
                if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mLength))//����������Ϣ
                    EDIT_STATE.Text = "K7120����������Ϣʧ��!��������!";
                else 
                {   //��ʾ�ض��ַ���
                    if (mLength !=0)
		            {
                        EDIT_STATE.Text = "�ض��ַ�����  " + System.Convert.ToString(RecvBuffer[3], 16) + "  "
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

        //�ֶ��������ݣ�����ȫ���Զ���
        private void BT_SEDN_MANUAL_Click(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32]; 
       								
            int func = System.Convert.ToInt32(EDIT_FUN.Text,2);				    //����ָ��
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);	//ת��ָ���վַ�ĸ�ʽ

            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //�ֽ�1��2����ָ���վַ
            SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);

            if (EDIT_DLC.Text == "00")
            {
                EDIT_STATE.Text = "����ȫͨ������!";
                SendBuffer[0] = 0;									           //�ֽ�0��ʾ���ݳ����ȣ��˴�Ϊ1
                mlength = 3;									//ָ���·�֡����Ϊ4���ֽ�
            }
            else if (EDIT_DLC.Text == "01")
            {
                EDIT_STATE.Text = "����ͨ������!";
                byte chan = System.Convert.ToByte (EDIT_DATA.Text); //ͨ����,ͨ����Ϊ01��08
                SendBuffer[0] = 1;									           //�ֽ�0��ʾ���ݳ����ȣ��˴�Ϊ1
                SendBuffer[3] = chan;							//�ֽ�3Ϊͨ����
                mlength = 4;
            }
            else 
            {
                EDIT_STATE.Text = "��������ݳ�����,����DLC�ַ��Ƿ���ȷ!"; return; 
            }
            OnSendData(SendBuffer,RecvBuffer);    		
        }

        //��������ӵ�listview�н�����ʾ
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
        //��������ӵ�teechart����ʾ
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
        //��������ϵ�����
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

            axTChart1.Series(0).Title = "ͨ��1";
            axTChart1.Series(1).Title = "ͨ��2";
            axTChart1.Series(2).Title = "ͨ��3";
            axTChart1.Series(3).Title = "ͨ��4";

            RADIO_CHART_SIGNLE_CH_CheckedChanged(sender, e);
        }
                  
        private void BT_SEND_CYCLE_Click(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32];
            string strtmp = BT_SEND_CYCLE.Text;
            if (strtmp == "�Զ�����")
            {
                //�ж��Ƿ�Ϊ���ڷ��ͣ������ֱ�ӵ��ö�ʱ���������ݣ�������һ������
                if (CHK_CYCLE.Checked)
                {
                    BT_SEND_CYCLE.Text = "ֹͣ����";
                    CHK_CYCLE.Enabled = false;
                    EDIT_SEND_INTVAL.Enabled = false;

                    timer1.Interval = System.Convert.ToInt32(EDIT_SEND_INTVAL.Text);
                    timer1.Start();
                }
                else
                {
                    //�жϵ�ͨ������ or ȫͨ�����ݣ������÷�������֡��ʽ					
                    int func;

                    if (RADIO_SINGLE.Checked)
                    {
                        func = 0x5;										//�ض���ͨ��ģ��������ָ��Ϊʮ������0X05
                        mlength = 4;
                        int chan = CMB_CHNO.SelectedIndex;				//ָ��ͨ����Ϊ1,ͨ����Ϊ1��8
                        SendBuffer[0] = 1;                              //�ֽ�0��ʾ���ݳ����ȣ��˴�Ϊ1
                        SendBuffer[3] = System.Convert.ToByte(chan);    //�ֽ�3Ϊͨ���ţ���1����Ϊģ����ͨ���ű�ʾΪ0��7
                    }
                    else
                    {
                        func = 0x6;										//�ض���ͨ��ģ��������ָ��Ϊʮ������0X06
                        mlength = 3;
                        SendBuffer[0] = 0;                              //�ֽ�0��ʾ���ݳ����ȣ��˴�Ϊ0
                    }

                    int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);			//ת��ָ���վַ�ĸ�ʽ

                    SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //�ֽ�1��2����ָ���վַ
                    SendBuffer[2] = System.Convert.ToByte(data & 0x00FF);

                    OnSendData(SendBuffer, RecvBuffer);//���÷�����������
                }
            }
            else
            {   //ֹͣ����
                BT_SEND_CYCLE.Text = "�Զ�����";
                CHK_CYCLE.Enabled = true;
                EDIT_SEND_INTVAL.Enabled = true;

                timer1.Stop();
            }
        }

        //�������ݣ�������
        private void OnSendData(Byte[] SendBuffer,Byte[] RecvBuffer)
        {
            UInt32 DevId = System.Convert.ToUInt32(CMB_DEV_INDEX.SelectedIndex);
            double dac;

            if (!CH375WriteData(DevId, ref SendBuffer[0], ref mlength))		//����CAN֡
                EDIT_STATE.Text = "K7120����ָ��ʧ��,��������!";
            else 
            {
                EDIT_STATE.Text = "���ͳɹ�,�ȴ�����!";
                SendTimes++;
                if (SendTimes == 1)
                    StartTime = DateTime.Now;

                CanData mCanData = new CanData();
                mCanData.Sn = SendTimes;
                mCanData.SendRecv = "����";
                mCanData.mTime = DateTime.Now;
                mCanData.FrmFormat = CMB_FRM_FORMAT.SelectedItem.ToString();
                mCanData.FrmType = CMB_FRM_TYPE.SelectedItem.ToString();
                mCanData.ID = System.Convert.ToString (SendBuffer[1],16) + System.Convert.ToString(SendBuffer[2],16);
                
                if ((SendBuffer[1] & 0xf0) == 0x50)
                {
                    mCanData.Data = System.Convert.ToString(SendBuffer[3], 16);
                    mlength = 6;//ָ���ض���ͨ���ֽڳ���
                }
                else
                {
                    mCanData.Data = "";
                    mlength = 11;//ָ���ض�ȫͨ���ֽڳ���
                }
                AddDataToListView(mCanData);//������ݵ�listview������ʾ
                
               										
                if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mlength))   //����can֡
                    EDIT_STATE.Text = "K7120����ָ��ʧ��,��������!";
                else
                {
                    
                    if ((RecvBuffer[1] & 0xf0) == 0x50)          //�жϷ���֡�Ƿ��ǵ�ͨ����������
                    {
                        dac = (RecvBuffer[5] * 256 + RecvBuffer[4]) / 4095.0 * 5;	//ת����ѹֵ
                        EDIT_STATE.Text = "ͨ��" + RecvBuffer[3] + "�ĵ�ѹ��: " + System.Convert.ToString(dac) + "V";
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
                        mCanData.SendRecv = "����";
                        mCanData.mTime = DateTime.Now;
                        mCanData.ID = System.Convert.ToString(RecvBuffer[1], 16) + System.Convert.ToString(RecvBuffer[2], 16);
                        mCanData.Data = System.Convert.ToString(RecvBuffer[3], 16) + "   "
                                            + System.Convert.ToString(RecvBuffer[4], 16) + " "
                                                + System.Convert.ToString(RecvBuffer[5], 16);
                        AddDataToListView(mCanData);//������ݵ�listview������ʾ
                        mCanData.ChNo = RecvBuffer[3]+1;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                    }
                    else if ((RecvBuffer[1] & 0xf0) == 0x60)
                    {
                        EDIT_STATE.Text = "��һ֡���ݽ��ճɹ���";
                        
                        SendTimes++;
                        mCanData.Sn = SendTimes;
                        mCanData.SendRecv = "����";
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
                        AddDataToListView(mCanData); //������ݵ�listview������ʾ

                        dac = (RecvBuffer[4] * 256 + RecvBuffer[3]) / 4095.0 * 5;	//ת����ѹֵ
                        EDIT_CH1.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 1;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                        dac = (RecvBuffer[6] * 256 + RecvBuffer[5]) / 4095.0 * 5;	//ת����ѹֵ
                        EDIT_CH2.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 2;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                        dac = (RecvBuffer[8] * 256 + RecvBuffer[7]) / 4095.0 * 5;	//ת����ѹֵ
                        EDIT_CH3.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 3;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);
                        dac = (RecvBuffer[10] * 256 + RecvBuffer[9]) / 4095.0 * 5;	//ת����ѹֵ
                        EDIT_CH4.Text = System.Convert.ToString(dac);
                        mCanData.ChNo = 4;
                        mCanData.Value = dac;
                        TeeChartCurveShow(mCanData);


                        if (!CH375ReadData(DevId, ref RecvBuffer[0], ref mlength))   //����can֡
                            EDIT_STATE.Text = "K7120����ָ��ʧ��,��������!";
                        else if ((RecvBuffer[1] & 0xf0) == 0x60)
                        {
                            EDIT_STATE.Text = "ȫ�����ݽ��ճɹ���";

                            SendTimes++;
                            mCanData.Sn = SendTimes;
                            mCanData.SendRecv = "����";
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
                            AddDataToListView(mCanData); //������ݵ�listview������ʾ
                        }
                    }
                    else 
                    {
                        EDIT_STATE.Text = "���յ�������ַ���,���鷢���ַ��Ƿ���ȷ!"; return;
                    }
                }
            }
         		

        }
        
 
       
        //��ʱ����������
        private void timer1_Tick(object sender, EventArgs e)
        {
            Byte[] SendBuffer = new Byte[32];
            Byte[] RecvBuffer = new Byte[32];						
            int func;

            if (RADIO_SINGLE.Checked)
            {
                func = 0x5;										//�ض���ͨ��ģ��������ָ��Ϊʮ������0X05
                int chan = CMB_CHNO.SelectedIndex;				//ָ��ͨ����Ϊ1,ͨ����Ϊ1��8
                mlength = 4;
                SendBuffer[0] = 1;                              //�ֽ�0��ʾ���ݳ����ȣ��˴�Ϊ1
                SendBuffer[3] = System.Convert.ToByte(chan);    //�ֽ�3Ϊͨ���ţ���1����Ϊģ����ͨ���ű�ʾΪ0��7
            }
            else
            {
                func = 0x6;										//�ض���ͨ��ģ��������ָ��Ϊʮ������0X06
                mlength = 3;
                SendBuffer[0] = 0;                              //�ֽ�0��ʾ���ݳ����ȣ��˴�Ϊ0
            }

           
            int data = ((station_no << 5) & 0x0FF0) | ((func << 12) & 0xF000);			//ת��ָ���վַ�ĸ�ʽ

            SendBuffer[1] = System.Convert.ToByte((data & 0xFF00) >> 8);    //�ֽ�1��2����ָ���վַ
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

        //ģ����أ��������ݿ⣬���ģ���ַ
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
                MessageBox.Show("δ��ȡ��ģ���ַ��"); this.Close(); 
            }
        }

    }
}