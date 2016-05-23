﻿using System;
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
    /// <summary>
    /// TabControl带添加和关闭选项卡按钮的自定义拓展控件
    /// </summary>
    public partial class ClosableTabControl : TabControl
    {
        #region Tab控件全局变量
        //绘制选项卡的尺寸
        int pgSize1 = 15;
        int pgSize2 = 5;
        //新建标签页标题
        String newPageCaptain = "NewTab";
        //标签索引
        private int pgIndex = 1;

        //动态新建Tab选项卡标题
        //private string newPageCaptain = "新标签页";
        #endregion

        #region Tab控件成员变量

        /// <summary>
        /// 标签页索引
        /// </summary>
        public int PgIndex
        {
            get { return pgIndex; }
            set { pgIndex = value; }
        }

        /// <summary>
        /// 动态新建Tab选项卡标题
        /// </summary>
        public string NewPageCaptain
        {
            get { return newPageCaptain; }
            set { newPageCaptain = value; }
        }

        #endregion

        #region Tab控件初始化构造函数

        /// <summary>
        /// 构造函数 初始化Tab控件
        /// </summary>
        public ClosableTabControl()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.Padding = new System.Drawing.Point(pgSize1, pgSize2);
            this.DrawItem += new DrawItemEventHandler(ClosableTab_DrawItem);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(ClosableTab_MouseDown);

        }

        #endregion

        #region Tab控件成员方法
        //初始化便签页添加按钮
        public void SetPageAddBtn()
        {
            //初始化便签页添加按钮
            if (this.TabPages["tabFirst"] == null)
            {
                this.TabPages.Add("tabFirst", "");
                this.PgIndex++;
            }
        }
        /// <summary>
        /// 添加新标签页
        /// </summary>
        public void PageAdd(String PageCaptain)
        {
            NewPageCaptain = PageCaptain;
            //新建Tab页
            TabPage pg = new TabPage(NewPageCaptain);
            //设置Tab页样式
            pg.BorderStyle = BorderStyle.None;
            //设置Tab页ID
            pg.Name = string.Format("pg{0}", this.PgIndex - 1);

            //将新建的Tab页绑定到Tab中
            //tabMain.TabPages.Add(pg);

            TabPage pgFirst = (this.TabPages["tabFirst"]);
            this.TabPages.Remove(this.TabPages["tabFirst"]);
            //this.TabPages.Add(pg);
            this.TabPages.Add(pgFirst);

            //设置Tab当前标签页为最新添加的页面
            this.SelectedIndex = this.TabPages.Count - 2;

            //为新建的Tab页添加标签
            //BrowserAttach();
            //Page索引加1
            this.PgIndex++;
        }
        #endregion

        public void PageAdd(System.Windows.Forms.Control ctl, int nodeID, string nodeName, Dictionary<int, TabPage> tabPageList) 
        {
            if (!tabPageList.ContainsKey(nodeID))
            {
                TabPage tabPage = new TabPage(nodeName);
                tabPage.Tag = nodeID;
                tabPageList.Add(nodeID, tabPage);
                tabPageList[nodeID].Controls.Add(ctl);
                tabPageList[nodeID].Tag = nodeID;
                this.Controls.Add(tabPageList[nodeID]);
            }
            else
            {
                if (!this.Controls.Contains(tabPageList[nodeID]))
                {
                    this.Controls.Add(tabPageList[nodeID]);
                }
            }
            this.SelectedTab = tabPageList[nodeID];
            this.Refresh();
        }

        public void PageUpdate(System.Windows.Forms.Control newCtl, int nodeID, Dictionary<int, TabPage> tabPageList)
        {
            foreach (Control c in tabPageList[nodeID].Controls)
            {
                tabPageList[nodeID].Controls.Remove(c);
            }
            tabPageList[nodeID].Controls.Add(newCtl);
            if (!this.Controls.Contains(tabPageList[nodeID]))
            {
                this.Controls.Add(tabPageList[nodeID]);
            }
            this.SelectedTab = tabPageList[nodeID];
            this.Refresh();
        }

        #region Tab控件事件

        /// <summary>
        /// Tab选项卡重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosableTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                //获取当前Tab选项卡的绘图区域
                Rectangle myTabRect = this.GetTabRect(e.Index);

                //判断该Tab页是否为添加按钮
                if (!String.IsNullOrEmpty(this.TabPages[e.Index].Text))
                {

                    //先添加TabPage属性      
                    e.Graphics.DrawString(this.TabPages[e.Index].Text
                    , this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);

                    //再画一个矩形框   
                    using (Pen p = new Pen(Color.Transparent))
                    {
                        myTabRect.Offset(myTabRect.Width - (pgSize1 + 3), 2);
                        myTabRect.Width = pgSize1;
                        myTabRect.Height = pgSize1;
                        e.Graphics.DrawRectangle(p, myTabRect);

                    }

                    //画Tab选项卡右上方关闭按钮   
                    using (Pen objpen = new Pen(Color.Black))
                    {
                        //获取绘图区域的开始坐标位置
                        Point p1 = new Point(myTabRect.X, myTabRect.Y);

                        //画关闭关闭按钮   
                        Bitmap bt = new Bitmap(Properties.Resources.close);
                        e.Graphics.DrawImage(bt, p1);
                    }
                }
                else
                {

                    //先添加TabPage属性      
                    e.Graphics.DrawString(this.TabPages[e.Index].Text
                    , this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);

                    //再画一个矩形框   
                    using (Pen p = new Pen(Color.Transparent))
                    {
                        myTabRect.Offset(myTabRect.Width - (pgSize1 + 3), 2);
                        myTabRect.Width = pgSize1;
                        myTabRect.Height = pgSize1;
                        e.Graphics.DrawRectangle(p, myTabRect);

                    }

                    //画Tab选项卡新增按钮   
                    using (Pen objpen = new Pen(Color.Black))
                    {
                        //获取绘图区域的开始坐标位置
                        Point p1 = new Point(myTabRect.X - 25, myTabRect.Y);

                        //画Tab选项卡新增按钮  
                        Bitmap bt = new Bitmap(Properties.Resources.add);
                        e.Graphics.DrawImage(bt, p1);
                    }
                }
                //释放绘图资源
                e.Graphics.Dispose();
            }
            //控件出现异常时进行捕获
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ClosableTab控件已崩溃", MessageBoxButtons.OK);
            }

        }

        private void ClosableTab_MouseDown(object sender, MouseEventArgs e)
        {
            //判断该Tab页是否为添加按钮
            if (!String.IsNullOrEmpty(this.SelectedTab.Text))
            {
                if (e.Button == MouseButtons.Left)
                {
                    int x = e.X, y = e.Y;

                    //计算关闭区域      
                    Rectangle myTabRect = this.GetTabRect(this.SelectedIndex);

                    myTabRect.Offset(myTabRect.Width - (pgSize1 + 3), 2);
                    myTabRect.Width = pgSize1;
                    myTabRect.Height = pgSize1;

                    //如果鼠标在区域内就关闭选项卡      
                    bool isClose = x > myTabRect.X && x < myTabRect.Right
                        && y > myTabRect.Y && y < myTabRect.Bottom;

                    if (isClose == true)
                    {
                        //this.TabPages.Remove(this.SelectedTab);//移除选项卡
                        this.Controls.Remove(this.SelectedTab);
                    }
                }
            }
            //else this.PageAdd(); //当Tab页为添加按钮添加新标签页


        }
        #endregion
    }


}