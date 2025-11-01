
using global::ChurchSermonsMetaData.Models;
using System.Text.Json;

namespace ChurchSermonsMetaData.Data;

internal class SeriesStorage
{
    private static readonly JsonSerializerOptions CachedJsonOptions = new() { WriteIndented = true };

    private Series Series { get; set; } = new Series() { SeriesNames= ["One-Off"] };
    public static string AppFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChurchSermonsMetaData");

    public static string DataFile => Path.Combine(AppFolder, "series.json");

    public void AddSeries(string elder) => Series.SeriesNames.Add(elder);

    public Series LoadOptions()
    {
        if (!File.Exists(DataFile))
        {
            Directory.CreateDirectory(AppFolder);
            File.WriteAllText(DataFile, JsonSerializer.Serialize(Series, CachedJsonOptions));
            return Series;
        }

        string json = File.ReadAllText(DataFile);
        Series = JsonSerializer.Deserialize<Series>(json, CachedJsonOptions)!;
        return Series;
    }

    public void SaveOptions()
    {
        File.WriteAllText(DataFile, JsonSerializer.Serialize(Series, CachedJsonOptions));
        OnOptionsSaved();
    }

    public static event EventHandler? OptionsSaved;

    protected static void OnOptionsSaved()
    {
        OptionsSaved?.Invoke(null, EventArgs.Empty);
    }
}
