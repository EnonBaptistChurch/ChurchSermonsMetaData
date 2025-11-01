namespace ChurchSermonsMetaData
{
    partial class FrmChurchSermonsMetaData
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
            btnFormSermonInfo = new Button();
            btnSermonAudioUpload = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // btnFormSermonInfo
            // 
            btnFormSermonInfo.Location = new Point(332, 472);
            btnFormSermonInfo.Name = "btnFormSermonInfo";
            btnFormSermonInfo.Size = new Size(129, 51);
            btnFormSermonInfo.TabIndex = 5;
            btnFormSermonInfo.Text = "Formulate Sermon Info";
            btnFormSermonInfo.UseVisualStyleBackColor = true;
            btnFormSermonInfo.Click += BtnFormSermonInfo_Click;
            // 
            // btnSermonAudioUpload
            // 
            btnSermonAudioUpload.Location = new Point(13, 32);
            btnSermonAudioUpload.Name = "btnSermonAudioUpload";
            btnSermonAudioUpload.Size = new Size(75, 42);
            btnSermonAudioUpload.TabIndex = 6;
            btnSermonAudioUpload.Text = "Load Sermon Audio";
            btnSermonAudioUpload.UseVisualStyleBackColor = true;
            btnSermonAudioUpload.Click += BtnSermonAudioUpload_Click;
            // 
            // button1
            // 
            button1.Location = new Point(13, 505);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 9;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // FrmChurchSermonsMetaData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(794, 535);
            Controls.Add(button1);
            Controls.Add(btnSermonAudioUpload);
            Controls.Add(btnFormSermonInfo);
            Name = "FrmChurchSermonsMetaData";
            Text = "Church Sermons Meta Data";
            ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        
        
        private Button btnFormSermonInfo;
        private Button btnSermonAudioUpload;
        private Button button1;
        //private GroupBox gbSeries;
    }
}
