//==================================================================================================
//
//  Title       : AboutDlg.cs
//  Purpose     :
//
//==================================================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace UserInterface
{
    public class AboutDlg : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ListView keysListView;
        private System.Windows.Forms.ColumnHeader keys;
        private System.Windows.Forms.ColumnHeader action;
        private System.ComponentModel.Container components = null;

        public AboutDlg()
        {
            InitializeComponent();
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Zoom"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+LeftMouseDrag",
            "Zoom into Selection."}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+Alt+LeftMouseDrag",
            "Zoom into proportional selection."}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+LeftClick",
            "Zoom in around point."}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+UpArrow",
            "Zoom in around middle of plot area."}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+DownArrow",
            "Zoom out around middle of plot area."}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+MouseWheel",
            "Zoom in/Zoom out."}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+RightClick",
            "Undo."}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Shift+Backspace",
            "Reset."}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "Pan"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ctrl+LeftMouseDrag",
            "Pan."}, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ctrl+LeftArrow",
            "Pan left."}, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ctrl+RightArrow",
            "Pan right."}, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ctrl+UpArrow",
            "Pan up."}, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ctrl+DownArrow",
            "Pan down."}, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ctrl+RightClick",
            "Undo."}, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ctrl+Backspace",
            "Reset."}, -1);
            this.keysListView = new System.Windows.Forms.ListView();
            this.keys = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.action = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // keysListView
            // 
            this.keysListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.keys,
            this.action});
            this.keysListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keysListView.FullRowSelect = true;
            this.keysListView.GridLines = true;
            this.keysListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.keysListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17});
            this.keysListView.Location = new System.Drawing.Point(0, 0);
            this.keysListView.Name = "keysListView";
            this.keysListView.Scrollable = false;
            this.keysListView.Size = new System.Drawing.Size(402, 315);
            this.keysListView.TabIndex = 1;
            this.keysListView.UseCompatibleStateImageBehavior = false;
            this.keysListView.View = System.Windows.Forms.View.Details;
            // 
            // keys
            // 
            this.keys.Text = "Key Combination";
            this.keys.Width = 141;
            // 
            // action
            // 
            this.action.Text = "Action";
            this.action.Width = 251;
            // 
            // AboutDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(402, 315);
            this.Controls.Add(this.keysListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graph Keyboard Help";
            this.ResumeLayout(false);

        }
        #endregion

        //==========================================================================================
        /// <summary>
        /// Releases the resources used by the component.
        /// </summary>
        /// <param name="disposing">
        /// If <see langword="true"/>, this method releases managed and unmanaged resources.  If <see langword="false"/>, this method releases only
        /// unmanaged resources.
        /// </param>
        //==========================================================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

