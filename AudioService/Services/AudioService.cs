using AudioService.Interfaces;
using AudioService.Models;
using NAudio.Wave;

namespace AudioService.Services;

public sealed class AudioService : IAudioService
{
    public async Task<string> ProcessAsync(
        string inputPath,
        AudioProcessingOptions options,
        CancellationToken ct = default)
    {
        if (!File.Exists(inputPath))
            throw new FileNotFoundException("Input audio file not found", inputPath);

        string workingPath = inputPath;

        if (options.ForceWav && !workingPath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
        {
            string wavPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_audio.wav");
            ConvertToWav(workingPath, wavPath);
            workingPath = wavPath;
        }
        
        if (options.MaxMinutes.HasValue)
        {
            string trimmedPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_trimmed.wav");
            TrimAudioToFirstNMinutes(workingPath, trimmedPath, options.MaxMinutes.Value);
            workingPath = trimmedPath;
        }

        return workingPath;
    }

    private void ConvertToWav(string inputPath, string outputPath)
    {
        using var reader = new AudioFileReader(inputPath);
        var outFormat = new WaveFormat(16000, 16, 1);

        using var resampler = new MediaFoundationResampler(reader, outFormat);
        WaveFileWriter.CreateWaveFile(outputPath, resampler);
    }

    
    private void TrimAudioToFirstNMinutes(string inputPath, string outputPath, int maxMinutes)
    {
        using var reader = new AudioFileReader(inputPath);
        var outFormat = new WaveFormat(16000, 16, 1);
        using var resampler = new MediaFoundationResampler(reader, outFormat);

        int bytesPerSecond = resampler.WaveFormat.AverageBytesPerSecond;
        int bytesToCopy = bytesPerSecond * 60 * maxMinutes;

        using var writer = new WaveFileWriter(outputPath, resampler.WaveFormat);
        byte[] buffer = new byte[4096];
        int bytesRead;
        int totalBytesCopied = 0;

        while ((bytesRead = resampler.Read(buffer, 0, buffer.Length)) > 0 && totalBytesCopied < bytesToCopy)
        {
            int bytesRemaining = bytesToCopy - totalBytesCopied;
            int bytesToWrite = Math.Min(bytesRead, bytesRemaining);
            writer.Write(buffer, 0, bytesToWrite);
            totalBytesCopied += bytesToWrite;
        }
    }
}
