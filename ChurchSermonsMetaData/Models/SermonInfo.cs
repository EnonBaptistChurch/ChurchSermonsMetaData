
namespace ChurchSermonsMetaData.Models;

internal record SermonInfo
{
    public string Title { get; set; } = string.Empty;
    public string Speaker { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public string Service { get; set; } = string.Empty;
    public List<string> BiblePassage { get; set; } = [];
}
