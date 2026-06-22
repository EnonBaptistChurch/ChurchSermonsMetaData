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
            btnPlay = new Button();
            btnPause = new Button();
            btnStop = new Button();
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
            // btnPlay
            // 
            btnPlay.Location = new Point(13, 81);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(75, 23);
            btnPlay.TabIndex = 10;
            btnPlay.Text = "Play";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(13, 110);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(75, 23);
            btnPause.TabIndex = 11;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(13, 139);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 12;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // FrmChurchSermonsMetaData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1077, 748);
            Controls.Add(btnStop);
            Controls.Add(btnPause);
            Controls.Add(btnPlay);
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
        private Button btnPlay;
        private Button btnPause;
        private Button btnStop;
        //private GroupBox gbSeries;
    }
}
