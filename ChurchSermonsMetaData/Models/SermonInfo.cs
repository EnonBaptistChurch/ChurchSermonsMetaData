
namespace ChurchSermonsMetaData.Models;

public record SermonInfo
{
    public string Title { get; set; } = string.Empty;
    public string Speaker { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public string Service { get; set; } = string.Empty;
    public string BiblePassageText { get; set; } = "";
    public List<string> BiblePassage => BiblePassageText
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Trim())
    .ToList();

    public string Description { get; set; } = string.Empty;
}
