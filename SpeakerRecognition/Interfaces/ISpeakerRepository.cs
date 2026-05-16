using SpeakerRecognition.Models;

namespace SpeakerRecognition.Interfaces;

public interface ISpeakerRepository
{

    Task<SpeakerRegistry> LoadAsync();
    Task SaveAsync(SpeakerRegistry registry);
}
