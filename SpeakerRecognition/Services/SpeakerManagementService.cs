using SpeakerRecognition.Interfaces;
using SpeakerRecognition.Models;

namespace SpeakerRecognition.Services;

public class SpeakerManagementService(ISpeakerRepository repo) : ISpeakerManagementService
{
    private readonly ISpeakerRepository _repo = repo;

    public async Task<SpeakerModel> AddSpeakerAsync(string name, SpeakerRole role)
    {
        var registry = await _repo.LoadAsync();

        var speaker = new SpeakerModel { Name = name, Role = role };

        registry.Speakers.Add(speaker);
        await _repo.SaveAsync(registry);

        return speaker;
    }

    public async Task<bool> RemoveSpeakerAsync(string speakerName)
    {
        var registry = await _repo.LoadAsync();
        var speaker = registry.Speakers
            .FirstOrDefault(s => s.Name == speakerName);
        if (speaker == null)
            return false;
        registry.Speakers.Remove(speaker);
        await _repo.SaveAsync(registry);
        return true;
    }

    public async Task<bool> UpdateSpeakerRoleAsync(string speakerName, SpeakerRole newRole)
    {
        var registry = await _repo.LoadAsync();
        var speaker = registry.Speakers
            .FirstOrDefault(s => s.Name == speakerName);
        if (speaker == null)
            return false;
        speaker.Role = newRole;
        await _repo.SaveAsync(registry);
        return true;
    }

    

    public async Task<SpeakerModel?> GetSpeakerAsync(string speakerName)
    {
        var registry = await _repo.LoadAsync();
        return registry.Speakers
            .FirstOrDefault(s => s.Name == speakerName);
    }

    public async Task<IReadOnlyList<SpeakerModel>> GetAllAsync()
    {
        var registry = await _repo.LoadAsync();
        return registry.Speakers;
    }

    public async Task<SpeakerModel> AddElderAsync(string name) => await AddSpeakerAsync(name, SpeakerRole.Elder);
    
}
