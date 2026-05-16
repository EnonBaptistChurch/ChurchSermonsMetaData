
namespace SpeakerRecognition.Models;

public record SpeakerMatch(
    SpeakerModel? Speaker,
    double Score,
    bool IsMatch
);
