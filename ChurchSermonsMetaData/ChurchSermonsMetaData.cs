using ChurchSermonsMetaData.Models;
using ChurchSermonsMetaData.UIControls;
using SermonData;
using System.Windows.Forms;


namespace ChurchSermonsMetaData
{
    public partial class FrmChurchSermonsMetaData : Form
    {
        private readonly GeneralUI generalUI;
        public FrmChurchSermonsMetaData()
        {
            InitializeComponent();
            generalUI = new GeneralUI(this);
            generalUI.LoadScreens();
        }

        private void BtnFormSermonInfo_Click(object sender, EventArgs e)
        {

        }

        private async void BtnSermonAudioUpload_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                string filePath = openFileDialog1.FileName;
                WhisperService whisperService = await WhisperService.CreateAsync("tiny");
                var transcript = await whisperService.TranscribeAudioAsync(filePath, 2);
                SermonExtractor sermonExtractor = new();
                var obj = await sermonExtractor.ExtractMetaAsync(transcript);
                FileInfo info = new (filePath);
                

                var lastWriteTime = info.LastWriteTimeUtc;

                // Example: Display it in a textbox
                var file = System.IO.File.Open(filePath, FileMode.Open);

                // You can now open/read the file
                string fileContent = System.IO.File.ReadAllText(filePath);
                MessageBox.Show($"File Content:\n{fileContent}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generalUI.LoadScreens();
        }
    }
}
