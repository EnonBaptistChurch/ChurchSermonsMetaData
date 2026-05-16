using SpeakerRecognition.Interfaces;

namespace SpeakerRecognition.Services;

public class EmbeddingEnrolmentService(
    ISpeakerRepository repo,
    IEmbeddingExtractor extractor) : IEmbeddingEnrollmentService
{
    private readonly ISpeakerRepository _repo = repo;
    private readonly IEmbeddingExtractor _extractor = extractor;

    public async Task AddEmbeddingAsync(string speakerName, string wavPath)
    {
        var registry = await _repo.LoadAsync();
        var speaker = registry.Speakers
            .FirstOrDefault(s => s.Name == speakerName);

        if (speaker == null)
            throw new InvalidOperationException("Speaker not found");

        var embedding = await _extractor.ExtractEmbeddingAsync(wavPath);
        speaker.Embeddings.Add(embedding);

        await _repo.SaveAsync(registry);
    }
}
