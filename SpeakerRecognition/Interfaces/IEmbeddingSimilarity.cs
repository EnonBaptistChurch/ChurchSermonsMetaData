namespace SpeakerRecognition.Interfaces;

public interface IEmbeddingSimilarity
{
    double Compare(float[] a, float[] b);
}
