using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace UserInterface.MyUserControl
{
    public class RangeEditorUI : System.Windows.Forms.UserControl
    {
        private double oldMinimum;
        private double oldMaximum;
        private double minimum;
        private double maximum;
        private NationalInstruments.UI.WindowsForms.NumericEdit maximumNumericEdit;
        private NationalInstruments.UI.WindowsForms.NumericEdit minimumNumericEdit;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label separatorLabel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        public RangeEditorUI(double minimumValue, double maximumValue)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            oldMinimum = minimumValue;
            minimum = minimumValue;
            minimumNumericEdit.Value = minimumValue;
            oldMaximum = maximumValue;
            maximum = maximumValue;
            maximumNumericEdit.Value = maximumValue;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if( components != null )
                    components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.minimumLabel = new System.Windows.Forms.Label();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.separatorLabel = new System.Windows.Forms.Label();
            this.maximumNumericEdit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.minimumNumericEdit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.SuspendLayout();
            // 
            // labelMinimum
            // 
            this.minimumLabel.Location = new System.Drawing.Point(16, 8);
            this.minimumLabel.Name = "labelMinimum";
            this.minimumLabel.Size = new System.Drawing.Size(88, 16);
            this.minimumLabel.TabIndex = 0;
            this.minimumLabel.Text = "Minimum";
            // 
            // labelMaximum
            // 
            this.maximumLabel.Location = new System.Drawing.Point(112, 8);
            this.maximumLabel.Name = "labelMaximum";
            this.maximumLabel.Size = new System.Drawing.Size(80, 16);
            this.maximumLabel.TabIndex = 1;
            this.maximumLabel.Text = "Maximum";
            // 
            // labelSeparator
            // 
            this.separatorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.separatorLabel.Location = new System.Drawing.Point(96, 24);
            this.separatorLabel.Name = "labelSeparator";
            this.separatorLabel.Size = new System.Drawing.Size(8, 16);
            this.separatorLabel.TabIndex = 4;
            this.separatorLabel.Text = "-";
            // 
            // maximumNumeric
            // 
            this.maximumNumericEdit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.maximumNumericEdit.Location = new System.Drawing.Point(112, 24);
            this.maximumNumericEdit.Name = "maximumNumeric";
            this.maximumNumericEdit.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.maximumNumericEdit.Size = new System.Drawing.Size(72, 20);
            this.maximumNumericEdit.TabIndex = 6;
            this.maximumNumericEdit.BeforeChangeValue += new NationalInstruments.UI.BeforeChangeNumericValueEventHandler(this.maximumNumeric_BeforeChangeValue);
            this.maximumNumericEdit.AfterChangeValue += new NationalInstruments.UI.AfterChangeNumericValueEventHandler(this.maximumNumeric_AfterChangeValue);
            // 
            // minimumNumeric
            // 
            this.minimumNumericEdit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0);
            this.minimumNumericEdit.Location = new System.Drawing.Point(16, 24);
            this.minimumNumericEdit.Name = "minimumNumeric";
            this.minimumNumericEdit.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.minimumNumericEdit.Size = new System.Drawing.Size(72, 20);
            this.minimumNumericEdit.TabIndex = 5;
            this.minimumNumericEdit.BeforeChangeValue += new NationalInstruments.UI.BeforeChangeNumericValueEventHandler(this.minimumNumeric_BeforeChangeValue);
            this.minimumNumericEdit.AfterChangeValue += new NationalInstruments.UI.AfterChangeNumericValueEventHandler(this.minimumNumeric_AfterChangeValue);
            // 
            // RangeEditorUI
            // 
            this.Controls.Add(this.minimumNumericEdit);
            this.Controls.Add(this.maximumNumericEdit);
            this.Controls.Add(this.separatorLabel);
            this.Controls.Add(this.maximumLabel);
            this.Controls.Add(this.minimumLabel);
            this.Name = "RangeEditorUI";
            this.Size = new System.Drawing.Size(192, 56);
            this.ResumeLayout(false);

        }
        #endregion

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if ((keyData & Keys.Escape) == Keys.Escape)
            {
                minimum = oldMinimum;
                maximum = oldMaximum;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void maximumNumeric_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {
            maximum = maximumNumericEdit.Value;
        }

        private void maximumNumeric_BeforeChangeValue(object sender, NationalInstruments.UI.BeforeChangeNumericValueEventArgs e)
        {
            if(e.NewValue <= minimum)
            {
                e.Cancel = true;
            }
        }

        private void minimumNumeric_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {
            minimum = minimumNumericEdit.Value;
        }

        private void minimumNumeric_BeforeChangeValue(object sender, NationalInstruments.UI.BeforeChangeNumericValueEventArgs e)
        {
            if(e.NewValue >= maximum)
            {
                e.Cancel = true;
            }
        }

        public double Minimum
        {
            get
            {
                return minimum;
            }
        }

        public double Maximum
        {
            get
            {
                return maximum;
            }
        }
    }
}
