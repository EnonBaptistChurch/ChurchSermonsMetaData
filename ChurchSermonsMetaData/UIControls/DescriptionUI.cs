
using ChurchSermonsMetaData.Models;

namespace ChurchSermonsMetaData.UIControls;

public class DescriptionUI
{
    private readonly GroupBox gbDescription;
    private readonly RichTextBox rtbDescription;

    public DescriptionUI(Form form)
    {
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
        gbDescription.Controls.Add(rtbDescription);

        form.Controls.Add(gbDescription);
    }

    public void LoadDescription(ref int groupBoxAddition)
    {
        gbDescription.Location = new Point(gbDescription.Left, gbDescription.Top + groupBoxAddition);
    }

    public string GetDescription() => rtbDescription.Text;
    public void SetDescription(string description) => rtbDescription.Text = description;
}
