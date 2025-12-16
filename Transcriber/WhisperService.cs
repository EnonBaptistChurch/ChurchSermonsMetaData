//using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Transcriber.Models;
using Whisper.net;

public class WhisperService
{
    private readonly string _modelPath;
    private readonly WhisperFactory _factory;

    // Prevent multiple simultaneous downloads for the same model
    private static readonly ConcurrentDictionary<string, Task<string>> _modelTasks = new();

    private WhisperService(string modelPath)
    {
        _modelPath = modelPath;
        _factory = WhisperFactory.FromPath(_modelPath);
    }

    /// <summary>
    /// Async factory method to create a WhisperService instance with the specified model.
    /// Downloads the model if missing.
    /// </summary>
    public static async Task<WhisperService> CreateAsync(string modelName)
    {
        string modelPath = await _modelTasks.GetOrAdd(modelName, _ => EnsureModelExistsAsync(modelName));
        return new WhisperService(modelPath);
    }

    /// <summary>
    /// Ensures the requested model exists locally, downloading if necessary.
    /// Explicit local filenames are used for safety with URLs that have query parameters.
    /// </summary>
    private static async Task<string> EnsureModelExistsAsync(string modelName)
    {
        string modelFolder = Path.Combine(Directory.GetCurrentDirectory(), "WhisperModels");
        Directory.CreateDirectory(modelFolder);

        // Map model names to (URL, local filename)
        var modelInfos = new Dictionary<string, (string url, string fileName)>(StringComparer.OrdinalIgnoreCase)
        {
            { "tiny",   ("https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-tiny.en.bin?download=true", "ggml-tiny.en.bin") },
            { "base",   ("https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-tiny.en.bin?download=true", "ggml-base.en.bin") },
            { "small",  ("https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-small.en.bin?download=true", "ggml-small.en.bin") },
            { "medium", ("https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-medium.en.bin?download=true", "ggml-medium.en.bin") }
        };

        if (!modelInfos.ContainsKey(modelName))
            throw new ArgumentException($"Unknown model name: {modelName}");

        var (url, localFileName) = modelInfos[modelName];
        string modelFile = Path.Combine(modelFolder, localFileName);

        if (!File.Exists(modelFile))
        {
            using var client = new HttpClient();
            Console.WriteLine($"Downloading {modelName} model...");
            var bytes = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(modelFile, bytes);
            Console.WriteLine($"Downloaded {modelName} model to {modelFile}");
        }
        else
        {
            Console.WriteLine($"{modelName} model already exists at {modelFile}");
        }

        return modelFile;
    }

    /// <summary>
    /// Transcribes an audio file. Optional: limit transcription to first N minutes.
    /// </summary>
    public async Task<string> TranscribeAudioAsync(string audioFilePath)
    {
        string transcription;

        try
        {
            var transcriptionResult = await TranscribeAsync(audioFilePath);
            transcription = string.Join(" ", transcriptionResult.ConvertAll(s => s.Text));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during transcription: {ex.Message}");
            transcription = string.Empty;
        }

        return transcription;
    }

    private async Task<List<WhisperTranscribeSection>> TranscribeAsync(string wavFilePath)
    {
        var resultGroupings = new List<WhisperTranscribeSection>();

        using var processor = _factory.CreateBuilder()
            .WithLanguage("en") // Change to "auto" for auto-detection
            .WithProbabilities() // Optional: include word probabilities
            .Build();

        using var fileStream = File.OpenRead(wavFilePath);
        await foreach (var segment in processor.ProcessAsync(fileStream))
        {
            resultGroupings.Add(new WhisperTranscribeSection
            {
                Start = segment.Start,
                End = segment.End,
                Text = segment.Text
            });
        }

        return resultGroupings;
    }
}
