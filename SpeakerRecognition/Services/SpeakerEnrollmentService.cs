using SpeakerRecognition.Interfaces;
using SpeakerRecognition.Models;

namespace SpeakerRecognition.Services;

internal class SpeakerEnrollmentService : ISpeakerEnrollmentService
{
    private readonly ISpeakerRepository _repo;
    private readonly IEmbeddingExtractor _extractor;

    public SpeakerEnrollmentService(ISpeakerRepository repo, IEmbeddingExtractor extractor)
    {
        _repo = repo;
        _extractor = extractor;
    }

    public async Task EnrollAsync(string speakerName, SpeakerRole role, string wavPath)
    {
        var registry = await _repo.LoadAsync();
        var embedding = await _extractor.ExtractEmbeddingAsync(wavPath);

        var speaker = registry.Speakers
            .FirstOrDefault(s => s.Name == speakerName);

        if (speaker == null)
        {
            speaker = new SpeakerModel
            {
                Name = speakerName,
                Role = role
            };
            registry.Speakers.Add(speaker);
        }

        speaker.Embeddings.Add(embedding);
        await _repo.SaveAsync(registry);
    }
}
