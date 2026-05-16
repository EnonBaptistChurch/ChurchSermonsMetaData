
namespace SpeakerRecognition.Interfaces;

public interface IEmbeddingEnrollmentService
{
    Task AddEmbeddingAsync(string speakerId, string wavPath);
}
