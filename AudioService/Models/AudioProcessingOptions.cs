
namespace AudioService.Models;

public sealed class AudioProcessingOptions
{
    /// <summary>
    /// Limit audio to first N minutes. Null = full file.
    /// </summary>
    public int? MaxMinutes { get; set; }

    /// <summary>
    /// Force conversion to WAV format.
    /// </summary>
    public bool ForceWav { get; set; } = true;
}
