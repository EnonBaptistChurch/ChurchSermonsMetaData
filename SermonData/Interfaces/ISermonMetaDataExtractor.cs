
using SermonData.Models;

namespace SermonData.Interfaces;

public interface ISermonMetaDataExtractor
{
    Task<SermonMetaData> ExtractMetaAsync(string transcript);
}
