using SpeakerRecognition.Models;
using System.Text.Json;

namespace SpeakerRecognition.Data;

internal class SpeakerStorage
{
    //private static readonly JsonSerializerOptions CachedJsonOptions = new() { WriteIndented = true };

    //private SpeakerRegistry Speakers { get; set; } = new SpeakerRegistry();
    //public static string AppFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChurchSermonsMetaData");

    //public static string DataFile => Path.Combine(AppFolder, "speakers.json");

    //public void AddElder(string elder) => Speakers.Add(elder);
    //public void AddSpeaker(string speaker) => Speakers.Add(new SpeakerModel() {  Name = speaker, Role = SpeakerRole.PreviousSpeaker});

    //public SpeakerRegistry LoadOptions()
    //{
    //    if (!File.Exists(DataFile))
    //    {
    //        Directory.CreateDirectory(AppFolder);
    //        File.WriteAllText(DataFile, JsonSerializer.Serialize(Speakers, CachedJsonOptions));
    //        //Speakers = options;
    //        return Speakers;
    //    }

    //    string json = File.ReadAllText(DataFile);
    //    Speakers = JsonSerializer.Deserialize<SpeakerRegistry>(json, CachedJsonOptions)!;
    //    return Speakers;
    //}

    //public void SaveOptions()
    //{
    //    File.WriteAllText(DataFile, JsonSerializer.Serialize(Speakers, CachedJsonOptions));
    //}
}
