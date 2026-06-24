using ChurchSermonsMetaData.Data;
using ChurchSermonsMetaData.Models;

namespace ChurchSermonsMetaData.UIControls;

public class GeneralUI
{
    private readonly SpeakersUI speakersUI;
    private readonly TitleUI titleUI;
    private readonly DescriptionUI descriptionUI;
    private readonly TranscriptionUI transcriptionUI;
    private readonly BiblePassageUI biblePassagesUI;
    private readonly SeriesUI seriesUI;
    private readonly ServicesUI servicesUI;
    private readonly MetaDescriptionOverviewUI metaDescriptionOverviewUI;
    private readonly SermonStore sermonStore;

    private SermonInfo _sermonInfo = new();

    public GeneralUI(Form form, SermonStore store)
    {
        speakersUI = new SpeakersUI(form, store);
        titleUI = new TitleUI(form, store);
        descriptionUI = new DescriptionUI(form, store);
        transcriptionUI = new TranscriptionUI(form);
        biblePassagesUI = new BiblePassageUI(form, store);
        seriesUI = new SeriesUI(form, store);
        servicesUI = new ServicesUI(form, store);
        metaDescriptionOverviewUI = new MetaDescriptionOverviewUI(form, store);
        sermonStore = store;
    }



    public void LoadScreens(SermonInfo? sermonInfo = null)
    {


        int groupBoxAddition = 50;
        speakersUI.LoadSpeakers(ref groupBoxAddition);
        groupBoxAddition += 30;
        servicesUI.LoadServices(ref groupBoxAddition);
        titleUI.LoadTitle(ref groupBoxAddition);
        seriesUI.LoadSeries(ref groupBoxAddition);
        descriptionUI.LoadDescription(ref groupBoxAddition);
        
        biblePassagesUI.LoadBiblePassages(ref groupBoxAddition);
        groupBoxAddition = 0;
        transcriptionUI.LoadTranscription(ref groupBoxAddition);
        metaDescriptionOverviewUI.LoadMetaDescription(ref groupBoxAddition);

        speakersUI.ReloadRequested += (s,e) => LoadScreens();
        
    }

    public void SetService(SermonServiceAndDate service)
    {
        servicesUI.SelectService(service.Service);
        sermonStore.Update(state =>
        {
            if(state != null && service.Date.HasValue) {
                state.Date = service.Date.Value;
            }
        });
    }

    public void HandleChange(object? sender, SermonInfo partial)
    {
        // merge changes instead of replacing
        _sermonInfo.Title = partial.Title ?? _sermonInfo.Title;
        _sermonInfo.Speaker = partial.Speaker ?? _sermonInfo.Speaker;
        // etc...

        
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

    public SpeakersUI GetSpeakersUI() => speakersUI;
    public TitleUI GetTitleUI() => titleUI;
    public DescriptionUI GetDescriptionUI() => descriptionUI;
    public TranscriptionUI GetTranscriptionUI() => transcriptionUI;
    public BiblePassageUI GetBiblePassagesUI() => biblePassagesUI;

}
