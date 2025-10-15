using ChurchSermonsMetaData.Models;
using System.Text.Json;

namespace ChurchSermonsMetaData;

internal class SpeakerStorage
{
    private static readonly JsonSerializerOptions CachedJsonOptions = new() { WriteIndented = true };

    private  Speakers Speakers { get; set; } = new Speakers();
    public static string AppFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChurchSermonsMetaData");

    public static string DataFile => Path.Combine(AppFolder, "speakers.json");

    public void AddElder(string elder) => Speakers.ElderList.Add(elder);
    public void AddSpeaker(string speaker) => Speakers.PreviousSpeakers.Add(speaker);

    public Speakers LoadOptions()
    {
        if (!File.Exists(DataFile))
        {
            var options = new Speakers();
            Directory.CreateDirectory(AppFolder);
            File.WriteAllText(DataFile, JsonSerializer.Serialize(options, CachedJsonOptions));
            Speakers = options;
            return Speakers;
        }
        string json = File.ReadAllText(DataFile);
        Speakers = JsonSerializer.Deserialize<Speakers>(json, CachedJsonOptions)!;
        return Speakers;
    }

    public void SaveOptions()
    {
        File.WriteAllText(DataFile, JsonSerializer.Serialize(Speakers, CachedJsonOptions));
    }
}
