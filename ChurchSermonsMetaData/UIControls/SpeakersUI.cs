using ChurchSermonsMetaData.Data;
using ChurchSermonsMetaData.Models;
using SpeakerRecognition.Models;

namespace ChurchSermonsMetaData.UIControls;

public class SpeakersUI
{
    private readonly GroupBox gbSpeakers;
    private readonly Button btnElder;

    public event EventHandler? ReloadRequested;

    private SpeakerStorage SpeakerStorage { get; set; }

    private readonly SermonStore store;

    public SpeakersUI(Form form, SermonStore store)
    {
        this.store = store;

        SpeakerStorage = new SpeakerStorage();

        gbSpeakers = new GroupBox();
        btnElder = new Button();

        //
        // gbSpeakers
        //
        gbSpeakers.Location = new Point(137, 28);
        gbSpeakers.Name = "gbSpeakers";
        gbSpeakers.Size = new Size(439, 115);
        gbSpeakers.TabIndex = 1;
        gbSpeakers.TabStop = false;
        gbSpeakers.Text = "Speakers";

        //
        // btnElder
        //
        btnElder.Location = new Point(600, 28);
        btnElder.Name = "btnElder";
        btnElder.Size = new Size(103, 23);
        btnElder.TabIndex = 2;
        btnElder.Text = "Add Elder";
        btnElder.UseVisualStyleBackColor = true;
        btnElder.Click += BtnElders_Click;

        form.Controls.Add(btnElder);
        form.Controls.Add(gbSpeakers);
    }

    private void BtnElders_Click(object? sender, EventArgs e)
    {
        using var inputBox = new InputBoxForm("Add New Elder", "New Elder Name:");

        if (inputBox.ShowDialog() == DialogResult.OK)
        {
            string name = inputBox.InputText;

            SpeakerStorage.AddElder(name);
            SpeakerStorage.SaveOptions();

            ReloadRequested?.Invoke(this, EventArgs.Empty);
        }
    }

    public GroupBox LoadSpeakers(ref int groupBoxAddition)
    {
        var originalHeight = gbSpeakers.Height;

        // Load speakers
        Speakers speakers = SpeakerStorage.LoadOptions();

        gbSpeakers.Top += groupBoxAddition;

        // Clear existing controls
        gbSpeakers.Controls.Clear();

        // Layout settings
        int topPosition = 20;
        int horizontalPosition = 20;

        const int horizontalSpacing = 5;
        const int verticalSpacing = 5;

        int rowHeight = 0;

        // Helper method for wrapping controls
        void AddWrappedControl(Control control)
        {
            control.Location = new Point(horizontalPosition, topPosition);

            rowHeight = Math.Max(rowHeight, control.Height);

            // Wrap if needed
            if (horizontalPosition + control.Width > gbSpeakers.ClientSize.Width)
            {
                horizontalPosition = 20;
                topPosition += rowHeight + verticalSpacing;
                rowHeight = control.Height;

                control.Location = new Point(horizontalPosition, topPosition);
            }

            gbSpeakers.Controls.Add(control);

            horizontalPosition += control.Width + horizontalSpacing;
        }

        //
        // Elder radio buttons
        //
        foreach (var speaker in speakers.ElderList)
        {
            var rb = new RadioButton
            {
                Name = $"rb_Elder_{speaker.Replace(" ", "_")}",
                Text = speaker,
                AutoSize = true,
                Tag = speaker,
                Checked = store.State.Speaker == speaker
            };

            rb.CheckedChanged += (s, e) =>
            {
                if (rb.Checked)
                {
                    store.Update(state =>
                    {
                        state.Speaker = speaker;
                    });
                }
            };

            AddWrappedControl(rb);
        }

        //
        // Reset position for additional controls
        //
        horizontalPosition = 20;
        topPosition += rowHeight + verticalSpacing;

        //
        // Previous Speaker ComboBox
        //
        var cmbPreviousSpeakers = new ComboBox
        {
            Name = "cmb_PreviousSpeakers",
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = speakers.PreviousSpeakers.ToArray(),
            Enabled = false
        };

        cmbPreviousSpeakers.SelectedIndexChanged += (s, e) =>
        {
            if (cmbPreviousSpeakers.SelectedItem is string selectedSpeaker)
            {
                store.Update(state =>
                {
                    state.Speaker = selectedSpeaker;
                });
            }
        };

        GeneralUI.AddOtherOption(
            gbSpeakers,
            "Previous Speaker",
            cmbPreviousSpeakers,
            ref horizontalPosition,
            ref topPosition,
            ref groupBoxAddition,
            cmbPreviousSpeakers.Height
        );

        //
        // Other TextBox
        //
        var txtOther = new TextBox
        {
            Name = "txt_Other",
            Width = 200,
            Enabled = false
        };

        GeneralUI.AddOtherOption(
            gbSpeakers,
            "Other",
            txtOther,
            ref horizontalPosition,
            ref topPosition,
            ref groupBoxAddition,
            txtOther.Height
        );

        //
        // Find "Other" radio button
        //
        var rbOther = gbSpeakers.Controls
            .OfType<RadioButton>()
            .First(c => c.Name.Contains("Other"));

        //
        // Update store when typing in Other textbox
        //
        txtOther.TextChanged += (s, e) =>
        {
            if (rbOther.Checked)
            {
                store.Update(state =>
                {
                    state.Speaker = txtOther.Text;
                });
            }
        };

        //
        // Add Previous Speaker button
        //
        var btnAddPrevious = new Button
        {
            Text = "Add Previous Speaker",
            AutoSize = true,
            Location = new Point(txtOther.Right + 20, txtOther.Top),
            Visible = false,
            Enabled = false
        };

        rbOther.CheckedChanged += (s, e) =>
        {
            btnAddPrevious.Visible = rbOther.Checked;
            btnAddPrevious.Enabled = rbOther.Checked;

            if (rbOther.Checked)
            {
                store.Update(state =>
                {
                    state.Speaker = txtOther.Text;
                });
            }
        };

        btnAddPrevious.Click += (s, e) =>
        {
            string name = txtOther.Text.Trim();

            if (!string.IsNullOrWhiteSpace(name) &&
                !speakers.PreviousSpeakers.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                SpeakerStorage.AddSpeaker(name);
                SpeakerStorage.SaveOptions();

                ReloadRequested?.Invoke(this, EventArgs.Empty);
            }
        };

        gbSpeakers.Controls.Add(btnAddPrevious);

        //
        // Resize group box if needed
        //
        gbSpeakers.Refresh();

        groupBoxAddition =
            gbSpeakers.Height - originalHeight + groupBoxAddition;

        return gbSpeakers;
    }
}