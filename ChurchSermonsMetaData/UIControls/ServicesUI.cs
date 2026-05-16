using ChurchSermonsMetaData.Data;

namespace ChurchSermonsMetaData.UIControls;

public class ServicesUI
{
    private readonly GroupBox gbServices;
    private readonly Form _form;
    private readonly SermonStore store;

    public ServicesUI(Form form, SermonStore store)
    {
        _form = form;
        this.store = store;

        gbServices = new GroupBox
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

        _form.Controls.Add(gbServices);
    }

    public void LoadServices(ref int groupBoxAddition)
    {
        gbServices.Location = new Point(
            gbServices.Left,
            gbServices.Top + groupBoxAddition
        );

        gbServices.Controls.Clear();

        GeneralUI.AddRadioButtons(
            gbServices,
            [.. Models.Services.ServiceTimes],
            "service",
            RadioButton_CheckedChanged
        );
        groupBoxAddition += gbServices.Height + 10;

    }

    public void SelectService(string service)
    {
        foreach (Control control in gbServices.Controls)
        {
            if (control is RadioButton rb &&
                rb.Text == service)
            {
                rb.Checked = true;
                break;
            }
        }
    }

    private void RadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is RadioButton rb && rb.Checked)
        {
            string selectedService = rb.Text;

            store.Update(state =>
            {
                state.Service = selectedService;
            });
        }
    }




}