using SpeakerRecognition.Interfaces;
using System.Net.Http.Json;

namespace SpeakerRecognition.Services;

internal class PythonEmbeddingExtractor : IEmbeddingExtractor
{
    private readonly HttpClient _http;

    public PythonEmbeddingExtractor(HttpClient http)
    {
        _http = http;
    }

    public async Task<float[]> ExtractEmbeddingAsync(string wavPath)
    {
        using var form = new MultipartFormDataContent();
        form.Add(
            new StreamContent(File.OpenRead(wavPath)),
            "file",
            Path.GetFileName(wavPath)
        );

        var response = await _http.PostAsync("/embed", form);
        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<EmbeddingResponse>();

        return result!.Embedding;
    }

    private class EmbeddingResponse
    {
        public float[] Embedding { get; set; } = [];
    }
}
