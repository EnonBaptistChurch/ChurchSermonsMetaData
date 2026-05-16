using SpeakerRecognition.Interfaces;
using SpeakerRecognition.Models;
using System.Text.Json;

namespace SpeakerRecognition.Services;

public class JsonSpeakerRepository : ISpeakerRepository
{
    private readonly string _path;
    private readonly object _lock = new();

    public JsonSpeakerRepository(string path)
    {
        _path = path;
    }

    public Task<SpeakerRegistry> LoadAsync()
    {
        lock (_lock)
        {
            if (!File.Exists(_path))
                return Task.FromResult(new SpeakerRegistry());

            var json = File.ReadAllText(_path);
            var registry = JsonSerializer.Deserialize<SpeakerRegistry>(json);

            return Task.FromResult(registry ?? new SpeakerRegistry());
        }
    }

    public Task SaveAsync(SpeakerRegistry registry)
    {
        lock (_lock)
        {
            var json = JsonSerializer.Serialize(
                registry,
                new JsonSerializerOptions { WriteIndented = true }
            );

            File.WriteAllText(_path, json);
            return Task.CompletedTask;
        }
    }
}
