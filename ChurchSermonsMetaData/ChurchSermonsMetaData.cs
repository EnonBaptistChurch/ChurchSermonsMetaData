using AudioService.Interfaces;
using ChurchSermonsMetaData.UIControls;
using SermonData.Interfaces;


namespace ChurchSermonsMetaData
{
    public partial class FrmChurchSermonsMetaData : Form
    {
        private readonly GeneralUI generalUI;
        private readonly IAudioService _audioService;
        private readonly ISermonMetaDataExtractor _sermonMetaDataExtractor;

        public FrmChurchSermonsMetaData(IAudioService audioService, ISermonMetaDataExtractor sermonMetaDataExtractor)
        {
            InitializeComponent();
            generalUI = new GeneralUI(this);
            generalUI.LoadScreens();
            _audioService = audioService;
            _sermonMetaDataExtractor = sermonMetaDataExtractor;
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

                
                var whisperTask = Task.Run(async () => await WhisperService.CreateAsync("tiny"));
                var audioProcessingTask = Task.Run(async () => await _audioService.ProcessAsync(filePath, new AudioService.Models.AudioProcessingOptions()
                {
                    MaxMinutes = 5
                }));

                var whisperService = await whisperTask;
                var audioFilePath = await audioProcessingTask;


                if(audioFilePath == null)
                {
                    MessageBox.Show("Audio processing failed.");
                    return;
                }

                var transcriptionTask = Task.Run(async () => await whisperService.TranscribeAudioAsync(audioFilePath));
                

                var transcript = await transcriptionTask;
                generalUI.GetTranscriptionUI().SetTranscription(string.Join("." +Environment.NewLine,transcript.Split(".", StringSplitOptions.RemoveEmptyEntries)));

                var obj = await _sermonMetaDataExtractor.ExtractMetaAsync(transcript);
                var biblePassages = string.Join("; ",obj.BiblePassages);
                generalUI.GetBiblePassagesUI().SetBiblePassages(biblePassages);
                FileInfo info = new (filePath);
                var lastWriteTime = info.LastWriteTimeUtc;

                


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generalUI.LoadScreens();
        }
    }
}
