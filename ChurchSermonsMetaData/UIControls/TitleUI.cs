using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSermonsMetaData.UIControls
{
    public class TitleUI
    {
        private readonly GroupBox gbTitle;
        private readonly TextBox txtTitle;

        public TitleUI(Form form)
        {
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

            gbTitle.Controls.Add(txtTitle);
            form.Controls.Add(gbTitle);
        }

        public GroupBox LoadTitle(ref int groupBoxAddition)
        {
            // Adjust position based on stacking offset
            gbTitle.Location = new Point(gbTitle.Left, gbTitle.Top + groupBoxAddition);

            // Update stacking offset
            //groupBoxAddition += gbTitle.Height + 10;

            return gbTitle;
        }

        public string GetTitle() => txtTitle.Text.Trim();
        public void SetTitle(string title) => txtTitle.Text = title;
    }
}
