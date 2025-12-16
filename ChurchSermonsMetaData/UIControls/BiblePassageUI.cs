using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchSermonsMetaData.UIControls
{
    public class BiblePassageUI
    {
        private readonly GroupBox gbBiblePassages;
        private readonly TextBox txtBiblePassages;
        private readonly Label lblBiblePassagesWarning;

        public BiblePassageUI(Form form)
        {
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
                Width = 400,
                Name = "txtBiblePassages",
                Text = "Please separate multiple passages with a semicolon (;)" + Environment.NewLine+
                "Texts that have been done via transcription maybe incorrect, please listen or check the transcript.",
            };
            gbBiblePassages.Controls.Add(txtBiblePassages);
            gbBiblePassages.Controls.Add(lblBiblePassagesWarning);
            form.Controls.Add(gbBiblePassages);
        }

        public void LoadBiblePassages(ref int groupBoxAddition)
        {
            gbBiblePassages.Location = new Point(gbBiblePassages.Left, gbBiblePassages.Top + groupBoxAddition);
        }

        public string GetBiblePassages() => txtBiblePassages.Text;
        public void SetBiblePassages(string description) => txtBiblePassages.Text = description;

    }
}
