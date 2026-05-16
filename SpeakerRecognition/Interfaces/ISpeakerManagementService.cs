using SpeakerRecognition.Models;

namespace SpeakerRecognition.Interfaces;

public interface ISpeakerManagementService
{

    Task<SpeakerModel> AddSpeakerAsync(string name, SpeakerRole role);
    Task<bool> RemoveSpeakerAsync(string speakerId);

    Task<SpeakerModel> AddElderAsync(string name);
    Task<SpeakerModel?> GetSpeakerAsync(string speakerId);
    Task<IReadOnlyList<SpeakerModel>> GetAllAsync();
}
