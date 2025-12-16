
namespace SermonData.Models;

class ParsedScriptureReference(int startIndex, bool noVerses = false)
{
    private bool NoVerses { get; } = noVerses;
    public int StartIndex { get; } = startIndex;
    public required string Book { get; set; }
    public int? StartChapter { get; set; }
    public string StartVerse { get; set; } = "";
    public List<int> StartVerses { get; set; } = [];
    public int? EndChapter { get; set; }
    public string EndVerse { get; set; } = "";

    public bool IsValid { get
        {
            if (!StartChapter.HasValue) return false;
            if(StartVerses.Any()) return true;
            if(NoVerses && EndChapter.HasValue && StartChapter.Value > EndChapter.Value) return false;
            if (NoVerses && StartChapter.Value < 0) return false;
            if(NoVerses) return true;

            if (EndChapter.HasValue && StartChapter.Value > EndChapter.Value) return false;
            if (string.IsNullOrEmpty(StartVerse) && string.IsNullOrEmpty(EndVerse) && !StartVerses.Any() && !NoVerses) return false;
            
            if (string.IsNullOrEmpty(StartVerse) && !StartVerses.Any() && !NoVerses) return false;
            var startVerse = Convert.ToInt32(StartVerse);
            if(!string.IsNullOrEmpty(EndVerse))
            {
                var endVerse = Convert.ToInt32(EndVerse);
                if (EndChapter.HasValue && StartChapter.Value == EndChapter.Value && startVerse > endVerse) return false;
                if (startVerse < 0 || endVerse < 0) return false;
            }
            
            
            return true;
        } 
    }

    public override string ToString()
    {
        if(NoVerses)
        {
            var hasEndChapter = EndChapter !=null;
            return hasEndChapter
                    ? $"{Book} {StartChapter}-{EndChapter}"
                    : $"{Book} {StartChapter}";
        }
        if(StartVerses.Count > 0)
            return $"{Book} {StartChapter}:{SummariseVerses(StartVerses)}";
        else
        {
            var sameChapter = StartChapter == EndChapter;
            var hasEndVerse = !string.IsNullOrEmpty(EndVerse);
            

            if (sameChapter || EndChapter == null)
            {
                return hasEndVerse
                    ? $"{Book} {StartChapter}:{StartVerse}-{EndVerse}"
                    : $"{Book} {StartChapter}:{StartVerse}";
            }

            return hasEndVerse
                ? $"{Book} {StartChapter}:{StartVerse}-{EndChapter}:{EndVerse}"
                : $"{Book} {StartChapter}:{StartVerse}-{EndChapter}";
        }
    }

    string SummariseVerses(IEnumerable<int> verses)
    {
        var ordered = verses.Distinct().OrderBy(v => v).ToList();
        var parts = new List<string>();

        int start = ordered[0];
        int prev = start;

        for (int i = 1; i <= ordered.Count; i++)
        {
            if (i < ordered.Count && ordered[i] == prev + 1)
            {
                prev = ordered[i];
                continue;
            }

            parts.Add(start == prev ? start.ToString() : $"{start}-{prev}");

            if (i < ordered.Count)
            {
                start = prev = ordered[i];
            }
        }

        return string.Join(",", parts);
    }
}
