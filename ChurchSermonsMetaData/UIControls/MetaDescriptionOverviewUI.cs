using ChurchSermonsMetaData.Data;
using ChurchSermonsMetaData.Models;
using System.Text;

namespace ChurchSermonsMetaData.UIControls;

public class MetaDescriptionOverviewUI
{
    private readonly GroupBox gbMetaDescription;
    private readonly RichTextBox rtbMetaDescription;

    private readonly SermonStore store;

    public MetaDescriptionOverviewUI(Form form, SermonStore store)
    {
        this.store = store;

        gbMetaDescription = new GroupBox
        {
            Location = new Point(587, 484),
            Name = "gbMetaDescription",
            Size = new Size(479, 215),
            TabIndex = 8,
            TabStop = false,
            Text = "Meta Description"
        };

        Button btnMetaDescription = new Button
        {
            Location = new Point(15, 20),
            Name = "btnMetaDescription",
            Size = new Size(450, 25),
            Text = "Meta Description",
        };

        btnMetaDescription.Click += (s, e) => Render();

        gbMetaDescription.Controls.Add(btnMetaDescription);

        rtbMetaDescription = new RichTextBox
        {
            Location = new Point(15, 50),
            Width = 450,
            Name = "rtbMetaDescription",
            Size = new Size(450, 150),
            ReadOnly = true
        };

        gbMetaDescription.Controls.Add(rtbMetaDescription);

        form.Controls.Add(gbMetaDescription);

        // 👇 IMPORTANT: react to ALL state changes automatically
        store.StateChanged += (s, state) => Render();
    }

    public void LoadMetaDescription(ref int groupBoxAddition)
    {
        gbMetaDescription.Location = new Point(
            gbMetaDescription.Left,
            gbMetaDescription.Top + groupBoxAddition
        );
    }

    // ❌ REMOVE THIS ENTIRELY
    // private SermonInfo _sermonInfo;

    // ❌ REMOVE THIS TOO
    // public SermonInfo GetSermonInfo()

    // ❌ REMOVE SetMetaDescription

    private void Render()
    {
        var state = store.State;

        rtbMetaDescription.Text = FormulateMetaDescription(state);
        Clipboard.SetText(rtbMetaDescription.Text);
    }

    private static string FormatDate(DateTime? date)
    {
        if(date == null || !date.HasValue) return "";
        return date.Value.ToString("dd-MM-yyyy");
    }

    private string FormulateMetaDescription(SermonInfo state)
    {
        StringBuilder sb = new();

        sb.AppendLine($"<p>Title: {state.Title}</p>");
        sb.AppendLine($"<p>Preacher: {state.Speaker}</p>");
        sb.AppendLine($"<p>Date: {FormatDate(state.Date)}</p>");
        sb.AppendLine($"<p>Service: {state.Service}</p>");
        
        sb.AppendLine(state.Series == null || state.Series == "N/A"
            ? "<p></p>"
            : $"<p>Series: {state.Series}</p>");
        var passages = state.BiblePassage;
        sb.AppendLine(passages == null || !passages.Any()
            ? "<p></p>"
            : $"<p>Bible Passages: {string.Join(", ", passages)}</p>");

        return sb.ToString();
    }
}