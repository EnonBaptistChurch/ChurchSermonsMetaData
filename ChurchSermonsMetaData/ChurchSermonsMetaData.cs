using AudioService.Interfaces;
using ChurchSermonsMetaData.Data;
using ChurchSermonsMetaData.Interfaces;
using ChurchSermonsMetaData.UIControls;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using SermonData.Interfaces;


namespace ChurchSermonsMetaData
{
    public partial class FrmChurchSermonsMetaData : Form
    {
        private readonly GeneralUI generalUI;
        private readonly IAudioService _audioService;
        private readonly ISermonMetaDataExtractor _sermonMetaDataExtractor;
        private readonly ISermonDateTimeCalculator _sermonDateTimeCalculator;
        private readonly SermonStore _sermonStore;
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private string _currentFile;


        public FrmChurchSermonsMetaData(IAudioService audioService, ISermonMetaDataExtractor sermonMetaDataExtractor, ISermonDateTimeCalculator sermonDateTimeCalculator)
        {
            InitializeComponent();
            _sermonStore = new SermonStore();
            generalUI = new GeneralUI(this, _sermonStore);
            generalUI.LoadScreens();
            _audioService = audioService;
            _sermonMetaDataExtractor = sermonMetaDataExtractor;
            _sermonDateTimeCalculator = sermonDateTimeCalculator;

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

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

                _mediaPlayer.Stop();
                _currentFile = filePath;


                var sermonServiceAndDate = _sermonDateTimeCalculator.GetSermonServiceInfoAsync(new FileInfo(filePath));
                generalUI.SetService(sermonServiceAndDate);
                //var whisperTask = Task.Run(async () => await WhisperService.CreateAsync("tiny"));
                var audioProcessingTask = Task.Run(async () => await _audioService.ProcessAsync(filePath, new AudioService.Models.AudioProcessingOptions()
                {
                    MaxMinutes = 5
                }));

                //var whisperService = await whisperTask;
                var audioFilePath = await audioProcessingTask;

                if (audioFilePath == null)
                {
                    MessageBox.Show("Audio processing failed.");
                    return;
                }



                /////////
                //var transcriptionTask = Task.Run(async () => await whisperService.TranscribeAudioAsync(audioFilePath));


                //var transcript = await transcriptionTask;
                //generalUI.GetTranscriptionUI().SetTranscription(string.Join("." +Environment.NewLine,transcript.Split(".", StringSplitOptions.RemoveEmptyEntries)));

                //var obj = await _sermonMetaDataExtractor.ExtractMetaAsync(transcript);
                //var biblePassages = string.Join("; ",obj.BiblePassages);
                //generalUI.GetBiblePassagesUI().SetBiblePassages(biblePassages);


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generalUI.LoadScreens();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFile))
                return;

            if (_mediaPlayer.Media == null)
            {
                _mediaPlayer.Media = new Media(_libVLC, _currentFile, FromType.FromPath);
            }

            _mediaPlayer.Play();
            btnPlay.Enabled = false;
            btnPause.Enabled = true;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _mediaPlayer.Pause();
            btnPlay.Enabled = true;
            btnPause.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _mediaPlayer.Stop();
            btnPlay.Enabled = true;
            btnPause.Enabled = false;
        }
    }
}
