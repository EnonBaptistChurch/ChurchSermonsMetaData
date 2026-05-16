using ChurchSermonsMetaData.Data;

namespace ChurchSermonsMetaData.UIControls;

public class DescriptionUI
{
    private readonly GroupBox gbDescription;
    private readonly RichTextBox rtbDescription;

    private readonly SermonStore store;

    public DescriptionUI(Form form, SermonStore store)
    {
        this.store = store;

        gbDescription = new GroupBox
        {
            Location = new Point(137, 304),
            Name = "gbDescription",
            Size = new Size(439, 115),
            TabIndex = 8,
            TabStop = false,
            Text = "Description"
        };

        rtbDescription = new RichTextBox
        {
            Location = new Point(15, 20),
            Width = 400,
            Name = "txtDescription",
            Size = new Size(400, 75),
        };

        //
        // Update store when description changes
        //
        rtbDescription.TextChanged += (s, e) =>
        {
            store.Update(state =>
            {
                state.Description = rtbDescription.Text;
            });
        };

        //
        // React to store updates
        //
        store.StateChanged += (s, state) =>
        {
            if (rtbDescription.Text != state.Description)
            {
                rtbDescription.Text = state.Description ?? string.Empty;
            }
        };

        gbDescription.Controls.Add(rtbDescription);

        form.Controls.Add(gbDescription);
    }

    public void LoadDescription(ref int groupBoxAddition)
    {
        gbDescription.Location = new Point(
            gbDescription.Left,
            gbDescription.Top + groupBoxAddition
        );
    }
}