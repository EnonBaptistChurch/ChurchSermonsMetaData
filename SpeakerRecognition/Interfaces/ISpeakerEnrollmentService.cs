using SpeakerRecognition.Models;

namespace SpeakerRecognition.Interfaces;

public interface ISpeakerEnrollmentService
{
    Task EnrollAsync(string speakerName, SpeakerRole role, string wavPath);
}
