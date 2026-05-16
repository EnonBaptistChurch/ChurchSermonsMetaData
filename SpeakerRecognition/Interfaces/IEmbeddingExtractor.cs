namespace SpeakerRecognition.Interfaces;
public interface IEmbeddingExtractor
{
    Task<float[]> ExtractEmbeddingAsync(string wavPath);
}
