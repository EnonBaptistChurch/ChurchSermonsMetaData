using ChurchSermonsMetaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSermonsMetaData.UIControls
{
    public class ServicesUI
    {
        private readonly GroupBox gbServices;

        public ServicesUI(Form form)
        {
            
            gbServices = new()
            {
                // 
                // gbServices
                // 
                Location = new Point(137, 149),
                Name = "gbServices",
                Size = new Size(439, 57),
                TabIndex = 4,
                TabStop = false,
                Text = "Services"
            };
            form.Controls.Add(gbServices);
        }
        public void LoadServices(ref int groupBoxAddition)
        {
            gbServices.Location = new Point(gbServices.Left, gbServices.Top + groupBoxAddition);
            GeneralUI.AddRadioButtons(gbServices, [.. Models.Services.ServiceTimes], "service", RadioButton_CheckedChanged);
        }

        private void RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                // Output something when this radio button is selected

                // Or do whatever logic you need
            }
        }
    }
}
