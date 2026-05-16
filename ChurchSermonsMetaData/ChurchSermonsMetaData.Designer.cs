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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChurchSermonsMetaData));
            openFileDialog1 = new OpenFileDialog();
            btnFormSermonInfo = new Button();
            btnSermonAudioUpload = new Button();
            button1 = new Button();
            audPlain = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)audPlain).BeginInit();
            SuspendLayout();
            // 
            // btnFormSermonInfo
            // 
            btnFormSermonInfo.Location = new Point(330, 685);
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
            button1.Location = new Point(12, 699);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 9;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // audPlain
            // 
            audPlain.Enabled = true;
            audPlain.Location = new Point(142, 16);
            audPlain.Name = "audPlain";
            audPlain.OcxState = (AxHost.State)resources.GetObject("audPlain.OcxState");
            audPlain.Size = new Size(317, 46);
            audPlain.TabIndex = 10;
            // 
            // FrmChurchSermonsMetaData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1077, 748);
            Controls.Add(audPlain);
            Controls.Add(button1);
            Controls.Add(btnSermonAudioUpload);
            Controls.Add(btnFormSermonInfo);
            Name = "FrmChurchSermonsMetaData";
            Text = "Church Sermons Meta Data";
            ((System.ComponentModel.ISupportInitialize)audPlain).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        
        
        private Button btnFormSermonInfo;
        private Button btnSermonAudioUpload;
        private Button button1;
        private AxWMPLib.AxWindowsMediaPlayer audPlain;
        //private GroupBox gbSeries;
    }
}
