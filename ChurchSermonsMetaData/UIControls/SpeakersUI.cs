using ChurchSermonsMetaData.Data;
using ChurchSermonsMetaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChurchSermonsMetaData.UIControls
{
    public class SpeakersUI
    {
        private readonly GroupBox gbSpeakers;
        private readonly Button btnElder;
        public event EventHandler ReloadRequested;
        private SpeakerStorage SpeakerStorage { get; set; } 

        public SpeakersUI(Form form)
        {
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

        private void BtnElders_Click(object sender, EventArgs e)
        {
            using var inputBox = new InputBoxForm("Add New Elder", "New Elder Name:");
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                string name = inputBox.InputText;
                SpeakerStorage.AddElder(name);
                SpeakerStorage.SaveOptions();
                //GeneralUI.LoadScreens(gbSpeakers, gbServices);
                ReloadRequested?.Invoke(this, EventArgs.Empty);
            }
        }
        public GroupBox LoadSpeakers(ref int groupBoxAddition)
        {
            var originalHeight = gbSpeakers.Height;
            // Load speakers
            Speakers speakers = SpeakerStorage.LoadOptions();

            // Clear existing controls
            gbSpeakers.Controls.Clear();

            // Layout settings
            int topPosition = 20;
            int horizontalPosition = 20;
            const int horizontalSpacing = 5;
            const int verticalSpacing = 5;
            int rowHeight = 0;

            // Helper to add a control with wrapping
            void AddWrappedControl(Control control)
            {
                // Set initial location
                control.Location = new Point(horizontalPosition, topPosition);

                // Measure row height
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

            // Add Elder radio buttons
            foreach (var speaker in speakers.ElderList)
            {
                var rb = new RadioButton
                {
                    Name = $"rb_Elder_{speaker.Replace(" ", "_")}",
                    Text = speaker,
                    AutoSize = true,
                    Tag = speaker,
                    //Checked = speaker == selectedSpeaker
                };
                AddWrappedControl(rb);
            }

            // Reset horizontal position for combo boxes / text boxes
            horizontalPosition = 20;
            topPosition += verticalSpacing;

            // Previous Speaker combo box
            var cmbPreviousSpeakers = new ComboBox
            {
                Name = "cmb_PreviousSpeakers",
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = speakers.PreviousSpeakers.ToArray(),
                Enabled = false
            };

            GeneralUI.AddOtherOption(gbSpeakers, "Previous Speaker", cmbPreviousSpeakers, ref horizontalPosition, ref topPosition, ref groupBoxAddition, cmbPreviousSpeakers.Height);

            // "Other" text box
            var txtOther = new TextBox
            {
                Name = "txt_Other",
                Width = 200,
                Enabled = false
            };
            GeneralUI.AddOtherOption(gbSpeakers, "Other", txtOther, ref horizontalPosition, ref topPosition, ref groupBoxAddition, txtOther.Height);

            // "Add Previous Speaker" button
            var rbOther = gbSpeakers.Controls
                .OfType<RadioButton>()
                .First(c => c.Name.Contains("Other"));

            var btnAddPrevious = new Button
            {
                Text = "Add Previous Speaker",
                AutoSize = true,
                Location = new Point(txtOther.Right + 20, txtOther.Top),
                Visible = false
            };

            rbOther.CheckedChanged += (s, e) =>
            {
                btnAddPrevious.Enabled = rbOther.Checked;
                btnAddPrevious.Visible = rbOther.Checked;
            };

            btnAddPrevious.Click += (s, e) =>
            {
                string name = txtOther.Text.Trim();
                if (!string.IsNullOrWhiteSpace(name) && !speakers.PreviousSpeakers.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    SpeakerStorage.AddSpeaker(name);
                    SpeakerStorage.SaveOptions();
                    ReloadRequested?.Invoke(this, EventArgs.Empty);

                }
            };

            gbSpeakers.Controls.Add(btnAddPrevious);
            gbSpeakers.Refresh();
            groupBoxAddition = gbSpeakers.Height - originalHeight;
            return gbSpeakers;
        }

        

    }
   
}
