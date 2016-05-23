using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.MyUserControl
{
    /// <summary>
    /// 自定义的TreeView;
    /// 继承自原版TreeView
    /// 添加功能：
    /// 1、可以绑定DataTable
    /// 2、可以批量更改选择状态
    /// 3、可以获取当前被选择的TreeNode列表
    /// 4、勾选的时候，会自动选择上级与下级
    /// </summary>
    ///[ToolboxBitmap("AcTreeView.ico")]
    public partial class AcTreeView : TreeView
    {
        public AcTreeView()
        {
            //构造函数
        }

        #region 数据读写
        /// <summary>
        /// 设置和获取绑定的DataTable
        /// </summary>
        [Browsable(true)]
        [Description("绑定DataTable")]
        [Category("自定义")]
        public DataTable bindingDataTable
        {
            get
            {
                return _bindingDataTable;
            }
            set
            {
                this.Nodes.Clear();//绑定数据前清空所有Nodes
                _bindingDataTable = value;
                if (_bindingDataTable != null)
                {
                    this.Dt2Tv();
                }

            }
        }
        private DataTable _bindingDataTable;
        /// <summary>
        /// 设置要绑定的DataTable的列名
        /// (ID列名，Name列名，UpID上级ID列名)
        /// </summary>
        /// <param name="ID">要绑定的ID列的列名</param>
        /// <param name="Name">要绑定的Name列的列名</param>
        /// <param name="UpID">要绑定的UpID(上级ID)列的列名</param>
        /// <param name="Remark">备注列的列名，可以不填，备注会显示在鼠标指示的项上弹出提示(设置后要开启ToolTips才会显示)</param>
        public void SetbindingWord(string ID, string Name, string UpID, string Remark = "")
        {
            if (string.IsNullOrWhiteSpace(ID) || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(UpID)) //为空检测
            {
                throw new Exception("写入的ID，Name,UpID不能为空或者null!");
            }
            bindingDataTableWord = new List<string>();
            bindingDataTableWord.Add(ID);
            bindingDataTableWord.Add(Name);
            bindingDataTableWord.Add(UpID);
            bindingDataTableWord.Add(Remark);
        }
        private List<string> bindingDataTableWord;

        #endregion DataTab读写器！

        #region 把DataTable的数据显示出来
        /// <summary>
        /// 显示DataTable数据方法
        /// </summary>
        private void Dt2Tv()
        {
            if (bindingDataTableWord == null)
            {
                throw new Exception("请写设置bindingDataTableWord，才能绑定DataTable");
            }
            DataView dv = new DataView(_bindingDataTable);
            dv.RowFilter = bindingDataTableWord[2] + "=" +"0";
            TreeNode tn = new TreeNode("ROOT");
            Btv(dv, tn);
            foreach (TreeNode x in tn.Nodes)
            {
                this.Nodes.Add(x);
            }
        }
        /// <summary>
        /// 显示DataTable的遍历方法
        /// </summary>
        /// <param name="dv">DataView</param>
        /// <param name="tn">TreeNode</param>
        private void Btv(DataView dv, TreeNode tn)
        {
            TreeNode tmpTN;
            foreach (DataRowView x in dv)
            {
                tmpTN = tn.Nodes.Add(x[bindingDataTableWord[0]].ToString());
                tmpTN.Name = x[bindingDataTableWord[0]].ToString();
                tmpTN.Text = x[bindingDataTableWord[1]].ToString();
                if (bindingDataTableWord[3] != "")
                {
                    tmpTN.ToolTipText = x[bindingDataTableWord[3]].ToString();
                }
                dv.RowFilter = bindingDataTableWord[2] + "=" + x[bindingDataTableWord[0]].ToString();
                Btv(dv, tmpTN);
            }
        }
        #endregion 把DataTable的数据显示出来

        #region 选择状态同步更改
        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node != null && !Convert.IsDBNull(e.Node))
                {
                    CheckParentNode(e.Node);
                    if (e.Node.Nodes.Count > 0)
                    {
                        CheckAllChildNodes(e.Node, e.Node.Checked);
                    }
                }
            }
            base.OnAfterCheck(e);
        }
        /// <summary>
        /// 改变父节点的选中状态，此处为所有子节点不选中时才取消父节点选中，可以根据需要修改
        /// </summary>
        /// <param name="curNode"></param>
        private void CheckParentNode(TreeNode curNode)
        {
            bool bChecked = false;

            if (curNode.Parent != null)
            {
                foreach (TreeNode node in curNode.Parent.Nodes)
                {
                    if (node.Checked)
                    {
                        bChecked = true;
                        break;
                    }
                }

                if (bChecked)
                {
                    curNode.Parent.Checked = true;
                    CheckParentNode(curNode.Parent);
                }
                else
                {
                    curNode.Parent.Checked = false;
                    CheckParentNode(curNode.Parent);
                }
            }
        }
        /// <summary>
        /// //改变所有子节点的状态
        /// </summary>
        private void CheckAllChildNodes(TreeNode pn, bool IsChecked)
        {
            foreach (TreeNode tn in pn.Nodes)
            {
                tn.Checked = IsChecked;

                if (tn.Nodes.Count > 0)
                {
                    CheckAllChildNodes(tn, IsChecked);
                }
            }
        }
        #endregion 选择状态同步更改

        #region 获取和设置当前为选择状态的TreeNode
        #region 获取List:GetCheckedNodes(NodeType nodeType)
        /// <summary>
        /// 获取状态为选择的TreeNodes的列表
        /// </summary>
        /// <param name="nodeType">要取的数据类型</param>
        /// <returns>返回字符串列表</returns>
        public List<string> GetCheckedNodes(NodeType nodeType)
        {
            List<string> tmp = new List<string>();
            foreach (TreeNode x in this.Nodes)
            {
                switch (nodeType)
                {
                    case NodeType.Name:
                        if (x.Checked)
                        {
                            tmp.Add(x.Name);
                        }
                        GetCheckedNodesO(tmp, x, nodeType);
                        break;
                    case NodeType.ToolTips:
                        if (x.Checked)
                        {
                            tmp.Add(x.ToolTipText);
                        }
                        GetCheckedNodesO(tmp, x, nodeType);
                        break;
                    case NodeType.Text:
                        if (x.Checked)
                        {
                            tmp.Add(x.Text);
                        }
                        GetCheckedNodesO(tmp, x, nodeType);
                        break;
                    case NodeType.FullPath:
                        if (x.Checked)
                        {
                            tmp.Add(x.FullPath);
                        }
                        GetCheckedNodesO(tmp, x, nodeType);
                        break;
                    default:
                        break;
                }
            }

            return tmp;
        }
        /// <summary>
        /// 取Nodes的列表的递归函数
        /// </summary>
        /// <param name="lStr">数据列表</param>
        /// <param name="tn">TreeNode</param>
        /// <param name="nodeType">要取消的数据类型</param>
        private void GetCheckedNodesO(List<string> lStr, TreeNode tn, NodeType nodeType)
        {
            foreach (TreeNode x in tn.Nodes)
            {
                switch (nodeType)
                {
                    case NodeType.Name:
                        if (x.Checked)
                        {
                            lStr.Add(x.Name);
                        }
                        if (x.Nodes.Count > 0)
                        {
                            GetCheckedNodesO(lStr, x, nodeType);
                        }
                        break;
                    case NodeType.ToolTips:
                        if (x.Checked)
                        {
                            lStr.Add(x.ToolTipText);
                        }
                        if (x.Nodes.Count > 0)
                        {
                            GetCheckedNodesO(lStr, x, nodeType);
                        }
                        break;
                    case NodeType.Text:
                        if (x.Checked)
                        {
                            lStr.Add(x.Text);
                        }
                        if (x.Nodes.Count > 0)
                        {
                            GetCheckedNodesO(lStr, x, nodeType);
                        }
                        break;
                    case NodeType.FullPath:
                        if (x.Checked)
                        {
                            lStr.Add(x.FullPath);
                        }
                        if (x.Nodes.Count > 0)
                        {
                            GetCheckedNodesO(lStr, x, nodeType);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        /// <summary>
        /// TreeNode的数据类型表示
        /// 名字列表，提示列表，显示文字列表，路径列表
        /// </summary>
        public enum NodeType
        {
            /// <summary>
            /// Node的Name属性资料
            /// </summary>
            Name,
            /// <summary>
            /// Node的ToolTips属性资料
            /// </summary>
            ToolTips,
            /// <summary>
            /// Node的Text属性资料
            /// </summary>
            Text,
            /// <summary>
            /// Node的FullPath属性资料
            /// </summary>
            FullPath
        }
        #endregion 获取List

        #region 设置Checked:SetNodesCheck(List<string> listString)
        /// <summary>
        /// 设置当前TreeView的项的选择状态
        /// (只能为Nodes的Name属性的列表;
        /// 本控件设置的Node的Name属性，对应的是数据表的ID列，一般是主键，或者是设置了唯一键，因为才不会发生设置重复的情况
        /// </summary>
        /// <param name="listString">状态为选择的项的ID的列表</param>
        public void SetNodesCheck(List<string> listString)
        {
            foreach (TreeNode x in this.Nodes)
            {
                if (listString.Contains(x.Name))
                {
                    x.Checked = true;
                }
                else
                {
                    x.Checked = false;
                }
                SetNodesCheckO(listString, x);
            }
        }
        /// <summary>
        /// 设置Check的递归方法
        /// </summary>
        private void SetNodesCheckO(List<string> lStr, TreeNode tn)
        {
            foreach (TreeNode x in tn.Nodes)
            {
                if (lStr.Contains(x.Name))
                {
                    x.Checked = true;
                }
                else
                {
                    x.Checked = false;
                }
                SetNodesCheckO(lStr, x);
            }
        }
        #endregion 设置Checked
        #endregion 获取和设置当前为选择状态的TreeNode

    }
}