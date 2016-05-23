//==================================================================================================
//
//  Title       : RangeEditor.cs
//  Purpose     :
//
//==================================================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface
{
    public class RangeEditorDlg : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.ComponentModel.Container components = null;
        private MyUserControl.RangeEditorUI mainRangeEditorUI;

        public RangeEditorDlg(double minimum, double maximum)
        {
            InitializeComponent();
            okButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
            StartPosition = FormStartPosition.CenterParent;
            mainRangeEditorUI = new MyUserControl.RangeEditorUI(minimum, maximum);
            Controls.Add(mainRangeEditorUI);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(12, 60);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(108, 60);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            // 
            // RangeEditorDlg
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(194, 95);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RangeEditorDlg";
            this.ShowInTaskbar = false;
            this.Text = "Range Editor";
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

        public double Maximum
        {
            get
            {
                return mainRangeEditorUI.Maximum;
            }
        }

        public double Minimum
        {
            get
            {
                return mainRangeEditorUI.Minimum;
            }
        }
    }
}