namespace ChurchSermonsMetaData
{
    partial class ChurchSermonsMetaData
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openFileDialog1 = new OpenFileDialog();
            rdbPastor = new RadioButton();
            gbSpeakers = new GroupBox();
            gbSpeakers.SuspendLayout();
            SuspendLayout();
            // 
            // rdbPastor
            // 
            rdbPastor.Location = new Point(6, 22);
            rdbPastor.Name = "rdbPastor";
            rdbPastor.Size = new Size(104, 24);
            rdbPastor.TabIndex = 0;
            rdbPastor.Text = "Paul Relf";
            // 
            // gbSpeakers
            // 
            gbSpeakers.Controls.Add(rdbPastor);
            gbSpeakers.Location = new Point(142, 133);
            gbSpeakers.Name = "gbSpeakers";
            gbSpeakers.Size = new Size(429, 57);
            gbSpeakers.TabIndex = 1;
            gbSpeakers.TabStop = false;
            gbSpeakers.Text = "Speakers";
            // 
            // ChurchSermonsMetaData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(gbSpeakers);
            Name = "ChurchSermonsMetaData";
            Text = "Form1";
            gbSpeakers.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        private RadioButton rdbPastor;
        private GroupBox gbSpeakers;
    }
}
