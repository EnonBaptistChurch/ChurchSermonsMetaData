using SpeakerRecognition.Models;

namespace SpeakerRecognition.Interfaces;

public interface ISpeakerRecognitionService
{
    Task<SpeakerMatch?> IdentifyAsync(string wavPath);
}
