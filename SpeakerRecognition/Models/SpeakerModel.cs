

namespace SpeakerRecognition.Models;

public class SpeakerModel
{
    public string Name { get; set; } = "";

    public SpeakerRole Role { get; set; }

    // One or more embeddings per speaker
    public List<float[]> Embeddings { get; set; } = [];
}
