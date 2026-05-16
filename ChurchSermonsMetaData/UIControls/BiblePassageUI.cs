using ChurchSermonsMetaData.Data;

namespace ChurchSermonsMetaData.UIControls;

public class BiblePassageUI
{
    private readonly GroupBox gbBiblePassages;
    private readonly TextBox txtBiblePassages;
    private readonly Label lblBiblePassagesWarning;

    private readonly SermonStore store;

    public BiblePassageUI(Form form, SermonStore store)
    {
        this.store = store;

        gbBiblePassages = new GroupBox
        {
            Location = new Point(137, 425),
            Name = "gbBiblePassages",
            Size = new Size(439, 115),
            TabIndex = 8,
            TabStop = false,
            Text = "Bible Passages"
        };

        txtBiblePassages = new TextBox
        {
            Location = new Point(15, 20),
            Width = 400,
            Name = "txtBiblePassages",
        };

        lblBiblePassagesWarning = new Label
        {
            Location = new Point(15, 50),
            Size = new Size(400, 50),
            Name = "lblBiblePassagesWarning",
            Text =
                "Please separate multiple passages with a semicolon (;)" +
                Environment.NewLine +
                "Transcription may be inaccurate, please verify manually.",
        };

        //
        // UI → Store (RAW input only)
        //
        txtBiblePassages.TextChanged += (s, e) =>
        {
            store.Update(state =>
            {
                state.BiblePassageText = txtBiblePassages.Text;
            });
        };


        gbBiblePassages.Controls.Add(txtBiblePassages);
        gbBiblePassages.Controls.Add(lblBiblePassagesWarning);
        form.Controls.Add(gbBiblePassages);
    }

    public void LoadBiblePassages(ref int groupBoxAddition)
    {
        gbBiblePassages.Location = new Point(
            gbBiblePassages.Left,
            gbBiblePassages.Top + groupBoxAddition
        );
    }

    //
    // Parse ONCE per change (not during rendering)
    //
    private static List<string> ParsePassages(string input)
    {
        return input
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
    }
}