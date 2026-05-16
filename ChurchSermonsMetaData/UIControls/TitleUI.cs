using ChurchSermonsMetaData.Data;

namespace ChurchSermonsMetaData.UIControls;

public class TitleUI
{
    private readonly GroupBox gbTitle;
    private readonly TextBox txtTitle;

    private readonly SermonStore store;

    public TitleUI(Form form, SermonStore store)
    {
        this.store = store;

        gbTitle = new GroupBox();
        txtTitle = new TextBox();

        //
        // gbTitle
        //
        gbTitle.Location = new Point(137, 149);
        gbTitle.Name = "gbTitle";
        gbTitle.Size = new Size(439, 57);
        gbTitle.TabIndex = 4;
        gbTitle.TabStop = false;
        gbTitle.Text = "Title";

        //
        // txtTitle
        //
        txtTitle.Location = new Point(20, 25);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(390, 23);
        txtTitle.TabIndex = 1;

        //
        // Update store when title changes
        //
        txtTitle.TextChanged += (s, e) =>
        {
            store.Update(state =>
            {
                state.Title = txtTitle.Text; // NO TRIM HERE
            });
        };

        //
        // React to store updates
        //
        store.StateChanged += (s, state) =>
        {
            if (txtTitle.Text != state.Title)
            {
                txtTitle.Text = state.Title ?? string.Empty;
            }
        };

        gbTitle.Controls.Add(txtTitle);

        form.Controls.Add(gbTitle);
    }

    public GroupBox LoadTitle(ref int groupBoxAddition)
    {
        //
        // Adjust position based on stacking offset
        //
        gbTitle.Location = new Point(
            gbTitle.Left,
            gbTitle.Top + groupBoxAddition
        );

        return gbTitle;
    }
}