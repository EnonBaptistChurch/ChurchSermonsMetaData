

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
            Size = new Size(479, 465),
            TabIndex = 8,
            TabStop = false,
            Text = "Transcription"
        };

        rtbTranscription = new RichTextBox
        {
            Location = new Point(15, 20),
            Width = 450,
            Name = "rtbTranscription",
            Size = new Size(450, 425),
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
