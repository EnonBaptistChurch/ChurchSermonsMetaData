using ChurchSermonsMetaData.Models;

namespace ChurchSermonsMetaData.UIControls;

public class GeneralUI(Form form)
{
    private readonly SpeakersUI speakersUI = new (form);
    private readonly TitleUI titleUI = new (form);
    private readonly DescriptionUI descriptionUI = new (form);
    private readonly SeriesUI seriesUI = new (form);
    private readonly ServicesUI servicesUI = new (form);
    

    public void LoadScreens(SermonInfo? sermonInfo = null)
    {
        int groupBoxAddition = 0;
        speakersUI.LoadSpeakers(ref groupBoxAddition);
        servicesUI.LoadServices(ref groupBoxAddition);
        titleUI.LoadTitle(ref groupBoxAddition);
        seriesUI.LoadSeries(ref groupBoxAddition);
        descriptionUI.LoadDescription(ref groupBoxAddition);

        speakersUI.ReloadRequested += (s,e) => LoadScreens();
    }
    public static void AddOtherOption(GroupBox groupBox, string radioText, Control control, ref int horizontalPosition, ref int topPosition, ref int groupBoxHeightAddition, int rbHeight = 20)
    {
        horizontalPosition = 20;
        topPosition += rbHeight + 5;

        // Create RadioButton
        var rbOther = new RadioButton
        {
            Name = "rb" + radioText,
            Text = radioText,
            AutoSize = true,
            Location = new Point(horizontalPosition, topPosition),
        };

        groupBox.Controls.Add(rbOther);

        // Create TextBox
        horizontalPosition += rbOther.Width + 5;
        control.Location = new Point(horizontalPosition, topPosition - 3);

        groupBox.Controls.Add(control);


        // Link the RadioButton to the TextBox
        rbOther.CheckedChanged += (s, e) =>
        {
            control.Enabled = rbOther.Checked;
        };

        if (control.Top + control.Height > groupBox.ClientSize.Height)
        {
            groupBoxHeightAddition = (control.Top + control.Height + 10) - groupBox.Height;
            groupBox.Height = control.Top + control.Height + 10;
        }
    }

    public static void AddRadioButtons(GroupBox container, IEnumerable<string> speakers, string rbPrefix, EventHandler? eventHandler = null)
    {
        int topPosition = 20; // Starting Y position
        int horizontalPosition = 20; // Starting X position

        foreach (var speaker in speakers)
        {
            var rb = new RadioButton
            {
                Name = $"rb_{rbPrefix}_{speaker.Replace(" ", "_")}",
                Text = speaker,
                AutoSize = true,
                Location = new Point(horizontalPosition, topPosition),
                Tag = speaker
            };
            if (eventHandler != null)
            {
                rb.CheckedChanged += eventHandler;
            }

            container.Controls.Add(rb);

            // Wrap to next line if the radio button exceeds the container width
            if (rb.Left + rb.Width > container.ClientSize.Width)
            {
                horizontalPosition = 20;
                topPosition += rb.Height + 5;
                rb.Location = new Point(horizontalPosition, topPosition);
            }

            horizontalPosition += rb.Width + 5;
        }
    }

}
