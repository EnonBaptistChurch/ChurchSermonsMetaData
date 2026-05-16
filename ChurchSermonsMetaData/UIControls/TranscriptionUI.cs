

namespace ChurchSermonsMetaData.UIControls;

public class TranscriptionUI
{
    private readonly GroupBox gbTranscription;
    private readonly RichTextBox rtbTranscription;

    public TranscriptionUI(Form form)
    {
        gbTranscription = new GroupBox
        {
            Location = new Point(587, 59),
            Name = "gbDescription",
            Size = new Size(479, 365),
            TabIndex = 8,
            TabStop = false,
            Text = "Transcription"
        };
        Button btnTranscribe = new Button
        {
            Location = new Point(15, 20),
            Name = "btnTranscribe",
            Size = new Size(450, 25),
            Text = "Transcribe",
        };
        gbTranscription.Controls.Add(btnTranscribe);

        rtbTranscription = new RichTextBox
        {
            Location = new Point(15, 50),
            Width = 450,
            Name = "rtbTranscription",
            Size = new Size(450, 300),
        };
        gbTranscription.Controls.Add(rtbTranscription);

        form.Controls.Add(gbTranscription);
    }

    public void LoadTranscription(ref int groupBoxAddition)
    {
        gbTranscription.Location = new Point(gbTranscription.Left, gbTranscription.Top + groupBoxAddition);
    }

    public string GetTranscription() => rtbTranscription.Text;
    public void SetTranscription(string description) => rtbTranscription.Text = description;
}
