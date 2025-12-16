using OllamaSharp;
using SermonData.Interfaces;
using SermonData.Models;
using System.Text;
using System.Text.Json;

namespace SermonData.Services;

public class SermonMetaDataExtractor : ISermonMetaDataExtractor
{
    private readonly OllamaApiClient ollama;
    private readonly IBiblePassageExtractor _biblePassageExtractor;
    public SermonMetaDataExtractor(IBiblePassageExtractor biblePassageExtractor)
    {
        _biblePassageExtractor = biblePassageExtractor;
        var apiUri = new Uri("http://localhost:11434");  // Ollama API endpoint
        ollama = new OllamaApiClient(apiUri)
        {
            SelectedModel = "llama3:latest"
        };
    }

    public async Task<SermonMetaData> ExtractMetaAsync(string transcript)
    {
        var passages = _biblePassageExtractor.ExtractPassages(transcript);
        return new SermonMetaData
        {
            Title = "Sample Sermon Title",
            Series = "Sample Series",
            BiblePassages = passages
        };
    }

    private string BuildPrompt(string transcript)
    {
        return $@"
                I have transcribed a sermon (not potentially without errors) and need to extract key information from it.
                Extract the sermon title and all Scripture references from the following text.
                Return the output in strict JSON format matching C# class properties:
                {{
                    ""Title"": string,
                    ""Series"": string (if available, otherwise empty string),
                    ""BiblePassage"": [ list of strings ]
                }}
                Text:
                ""{transcript}""
                ";
    }

    private SermonMetaData DeserializeOutput(string output)
    {
        try
        {
            var meta = JsonSerializer.Deserialize<SermonMetaData>(output);
            return meta ?? new SermonMetaData();
        }
        catch (JsonException)
        {
            // Fallback if JSON is malformed
            Console.WriteLine("Model output was not valid JSON: " + output);
            return new SermonMetaData();
        }
    }

    private async Task<string> GenerateModelOutputAsync(string prompt)
    {
        var sb = new StringBuilder();
        await foreach (var response in ollama.GenerateAsync(prompt))
        {
            sb.Append(response.Response); // Stream token by token
        }
        return sb.ToString();
    }

    public async Task<SermonMetaData> ExtractMetaUsingAIAsync(string transcript)
    {
        string prompt = BuildPrompt(transcript);
        string output = await GenerateModelOutputAsync(prompt);
        return DeserializeOutput(output);
    }
}
