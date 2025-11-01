using ChurchSermonsMetaData.Data;

namespace ChurchSermonsMetaData.UIControls;

public class SeriesUI
{
    private readonly GroupBox gbSeries;
    private readonly ComboBox cmbSeries;
    private readonly Button btnAddSeries;
    private SeriesStorage SeriesStorage { get; set; } 

    public SeriesUI(Form form)
    {
        SeriesStorage = new SeriesStorage();
        gbSeries = new GroupBox
        {
            Location = new Point(137, 220),
            Name = "gbSeries",
            Size = new Size(439, 69),
            TabIndex = 9,
            TabStop = false,
            Text = "Series"
        };

        cmbSeries = new ComboBox
        {
            Location = new Point(20, 28),
            Width = 300,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        gbSeries.Controls.Add(cmbSeries);

        btnAddSeries = new Button
        {
            Location = new Point(340, 28),
            Name = "btnAddSeries",
            Size = new Size(83, 23),
            TabIndex = 10,
            Text = "Add Series",
            UseVisualStyleBackColor = true
        };

        btnAddSeries.Click += BtnAddSeries_Click;

        gbSeries.Controls.Add(btnAddSeries);
        form.Controls.Add(gbSeries);
    }

    private void BtnAddSeries_Click(object sender, EventArgs e)
    {
        using var inputBox = new InputBoxForm("Add New Series", "New Series Name:");
        if (inputBox.ShowDialog() == DialogResult.OK)
        {
            string name = inputBox.InputText.Trim();
            if (!string.IsNullOrWhiteSpace(name))
            {
                SeriesStorage.AddSeries(name);
                SeriesStorage.SaveOptions();
                LoadSeriesData();
            }
        }
    }

    public GroupBox LoadSeries(ref int groupBoxAddition)
    {
        gbSeries.Location = new Point(gbSeries.Left, gbSeries.Top + groupBoxAddition);
        LoadSeriesData();
        return gbSeries;
    }

    private void LoadSeriesData()
    {
        var seriesList = SeriesStorage.LoadOptions();
        if(seriesList.SeriesNames.Count != 0)
            cmbSeries.DataSource = seriesList.SeriesNames;
    }

    public string GetSelectedSeries() => cmbSeries.SelectedItem?.ToString() ?? string.Empty;
}
