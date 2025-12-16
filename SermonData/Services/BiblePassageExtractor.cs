using SermonData.Interfaces;
using SermonData.Models;

using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace SermonData.Services;

public class BiblePassageExtractor : IBiblePassageExtractor
{

    static readonly HashSet<string> BibleBooks = new(StringComparer.OrdinalIgnoreCase)
    {
        "Genesis", "Exodus", "Leviticus", "Numbers", "Deuteronomy",
        "Joshua", "Judges", "Ruth", "1 Samuel", "2 Samuel",
        "1 Kings", "2 Kings", "1 Chronicles", "2 Chronicles", "Ezra",
        "Nehemiah", "Esther", "Job", "Psalms", "Proverbs",
        "Ecclesiastes", "Song of Solomon", "Isaiah", "Jeremiah", "Lamentations",
        "Ezekiel", "Daniel", "Hosea", "Joel", "Amos", "Obadiah", "Jonah",
        "Micah", "Nahum", "Habakkuk", "Zephaniah", "Haggai", "Zechariah", "Malachi",
        "Matthew", "Mark", "Luke", "John", "Acts", "Romans", "1 Corinthians", "2 Corinthians",
        "Galatians", "Ephesians", "Philippians", "Colossians", "1 Thessalonians", "2 Thessalonians",
        "1 Timothy", "2 Timothy", "Titus", "Philemon", "Hebrews", "James", "1 Peter", "2 Peter",
        "1 John", "2 John", "3 John", "Jude", "Revelation"
    };

    public List<string> ExtractPassages(string text)
    {
        return ExtractPassages5(text);
    }


    public List<string> ExtractPassages5(string text)
    {
        var results = new List<ParsedScriptureReference>();

        if (string.IsNullOrWhiteSpace(text))
            return [];

        // Match patterns:
        // 1. Book chapter:verse-range
        // 2. Book chapter verse-range
        // 3. Book chapter only
        // 4. Book:verse
        
        results = AssignParsedScriptureReferenceNonLetters(text);

        // Handles: "Book chapter X verse Y and Z", "Book X:Y-Z", "Book chapter X verse Y to Z"
        

        // Remove duplicates while preserving order
        return results.Select(x=> x.ToString()).Distinct().ToList();
    }

    private List<ParsedScriptureReference> AssignParsedScriptureReferenceNonLetters(string text)
    {
        var results = new List<ParsedScriptureReference>();
        var bookPattern = string.Join("|", BibleBookVariants.Keys
            .OrderByDescending(k => k.Length)
            .Select(Regex.Escape));

        var numberPattern = string.Join("|", numberWords.Keys
            .OrderByDescending(k => k.Length)
            .Select(Regex.Escape));

        var revivedText = ReplaceWordsWithNumbers(text);

        var patterns = new ScriptureParserModel[]
        {
            new ($@"\b({bookPattern})\s+(?:chapters?\s+)?(\d+)?[,\ :](?:\s+verses?\s+)?(\d+(?:\-\d+?|[\d\,]+)?(?:\s(and|to)\s(?!)\d+?)?)",false),
            new ($@"\b({bookPattern})\ (?:chapters?\ \,?)(\d+)\,?(?:\ verses?\ )(\d+)\,?(?:\s(?:and\s)|(?:to\s))?(\d+)?",false),
            new ($@"\b({bookPattern})\s+(?:chapters?\s+)?(\d+)?[,\ :](?:\s+verses?\s+)?(\d+(?:\-\d+?|[\d\,]+))?(?:\s(?:and|to)[\s\ \,]{{0,5}})?(\d+):(\d+)", true),
            new ($@"\b({bookPattern})\s+(\d+)?[,\ :](\d+)?(?:\-[\s\ \,]{{0,5}})?(\d+):(\d+)", true),
            new ($@"\b({bookPattern})\s+(?:chapters?\s+)?(\d+)?[,\ :]?(?:(?:\s+verses?\s+)|\:)?(\d+)?(?:\s(?:and|to)[\s\ \,]{{0,5}})?(?:chapters?\s+)?(\d+)(?:(?:\s+verses?\s+)|\:)(\d+)", true),
            new ($@"\b({bookPattern})\s+(?:chapter\s)?(\d+)(?:\s?(?:and|to|\-|\,)+\s?)?(\d+)?", false, true),
        };//$@"\b({bookPattern})\s*(?:chapter\s+(\d+))?\s*(?:[:]?(\d+)(?:\s*(?:-|,?\s*and\s*|to\s*)(\d+))?)?";
        for (int i = 0; i < patterns.Length; i++)
        {
            var patternModel = patterns[i];
            var matches = Regex.Matches(revivedText, patternModel.Pattern, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                //var sr = ExtractReference(text, match, bookPattern);
                string book = match.Groups[1].Value;
                if (!BibleBookVariants.Keys.Contains(book))
                    continue;
                var cleanedBook = BibleBookVariants[book];

                if (patternModel.NoVerses)
                {
                    string firstChapterStr = match.Groups[2].Success ? match.Groups[2].Value : "";

                    string secondChapterStr = match.Groups[3].Success ? match.Groups[3].Value : "";
                    results.Add(new ParsedScriptureReference(match.Index, patternModel.NoVerses)
                    {
                        Book = cleanedBook,
                        StartChapter = Convert.ToInt32(firstChapterStr),
                        EndChapter = string.IsNullOrEmpty(secondChapterStr) ? null : Convert.ToInt32(secondChapterStr)
                    });
                    continue;
                }

                string chapterStr = match.Groups[2].Success ? match.Groups[2].Value : "";

                string verseStart = match.Groups[3].Success ? match.Groups[3].Value : "";
                string verseEnd = "";
                int? endChapter = -1;
                if (patternModel.OverChapterRangeScripture)
                {
                    endChapter = match.Groups[4].Success ? Convert.ToInt32(match.Groups[4].Value) : -1;
                    verseEnd = match.Groups[5].Success ? match.Groups[5].Value : "";
                }
                
                if (new string[] { verseStart, verseEnd }.All(x => string.IsNullOrEmpty(x)))
                {
                    var index = match.Groups[2].Index + chapterStr.Length;
                    string potentialVerseString = revivedText.Substring(index, revivedText.Length - index).Trim();
                    verseStart = potentialVerseString.Replace("verses", "").Replace("verse", "").Trim();

                }
                
                if (verseStart.Contains(","))
                {
                    var listofStartVerses = verseStart.Split(",", StringSplitOptions.RemoveEmptyEntries).Where(x=> int.TryParse(x, out var _)).Select(x => Convert.ToInt32(x)).ToList();
                    results.Add(new ParsedScriptureReference(match.Index, patternModel.NoVerses)
                    {
                        Book = cleanedBook,
                        StartChapter = Convert.ToInt32(chapterStr),
                        StartVerses = listofStartVerses
                    });
                    continue;
                }
                else if (verseStart.Contains("-"))
                {
                    var verseRngStart = verseStart.Substring(0,verseStart.IndexOf("-"));
                    var verseRngEnd = verseStart.Substring(verseStart.IndexOf("-")+1);
                    results.Add(new ParsedScriptureReference(match.Index, patternModel.NoVerses)
                    {
                        Book = cleanedBook,
                        StartChapter = Convert.ToInt32(chapterStr),
                        StartVerse = verseRngStart,
                        EndVerse = verseRngEnd
                    });
                    continue;
                }

                if (!string.IsNullOrEmpty(chapterStr) && !int.TryParse(chapterStr, out int chapter))
                    continue;

                if (!string.IsNullOrEmpty(verseStart) && !int.TryParse(verseStart, out _))
                    continue;

                if (!string.IsNullOrEmpty(verseEnd) && !int.TryParse(verseEnd, out _))
                    continue;

                
                var parsedRef = new ParsedScriptureReference(match.Index, patternModel.NoVerses)
                {
                    Book = cleanedBook,
                    StartChapter = string.IsNullOrEmpty(chapterStr) ? null : int.Parse(chapterStr),
                    StartVerse = verseStart,
                    EndChapter = endChapter == -1 ? null : endChapter,
                    EndVerse = verseEnd
                };

                results.Add(parsedRef);
            }
        }

        
        return results.Where(x=> x.IsValid)
        .GroupBy(r => r.StartIndex)
        .Select(g =>
            g.OrderByDescending(r => r.ToString().Length)
             .First()
        )
        .ToList(); 
    }

    private string ReplaceWordsWithNumbers(string text)
    {
        var numberWordPattern =
    @"\b(?:(?:one|two|three|four|five|six|seven|eight|nine|" +
    @"ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|" +
    @"seventeen|eighteen|nineteen|twenty|thirty|forty|" +
    @"fifty|sixty|seventy|eighty|ninety|hundred)\s*)+\b";

        return Regex.Replace(
            text,
            numberWordPattern,
            match =>
            {
                var phrase = match.Value.ToLower();
                var number = ParseNumberWords(phrase);
                return number.ToString();
            },
            RegexOptions.IgnoreCase
        );

    }

    private static int ParseNumberWords(string text)
    {
        int total = 0;
        int current = 0;

        foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var value = numberWords[word];

            if (value == 100)
            {
                current = current == 0 ? 100 : current * 100;
            }
            else
            {
                current += value;
            }
        }

        return total + current;
    }

    private readonly static Dictionary<string, int> numberWords = new()
    {
        {"one",1},{"two",2},{"three",3},{"four",4},{"five",5},{"six",6},{"seven",7},
        {"eight",8},{"nine",9},{"ten",10},{"eleven",11},{"twelve",12},{"thirteen",13},
        {"fourteen",14},{"fifteen",15},{"sixteen",16},{"seventeen",17},{"eighteen",18},
        {"nineteen",19},{"twenty",20},{"thirty",30},{"forty",40},{"fifty",50},{"sixty",60},
        {"seventy",70},{"eighty",80},{"ninety",90},{"hundred",100}
    };

    private void ExtractReference(string text, Match match, string bookPattern)
    {
        var patterns = new string[] {
            $@"\b({bookPattern})\s+(?:chapters?\s+)?(\d+)?[,\ :](?:\s+verses?\s+)?(\d+(?:\-\d+?|[\d\,]+)(?:\s(and|to)\s\d+)?)"
        };


        string book = match.Groups[1].Value;
        if (!BibleBookVariants.Keys.Contains(book)) return;

        string chapterStr = match.Groups[2].Success ? match.Groups[2].Value : "";
        if(string.IsNullOrEmpty(chapterStr)) return;

        string verseStart = match.Groups[3].Success ? match.Groups[3].Value : "";
        string verseEnd = match.Groups[4].Success ? match.Groups[4].Value : "";
        if (new string[] { verseStart, verseEnd }.All(x => string.IsNullOrEmpty(x)))
        {
            var index = match.Groups[2].Index + chapterStr.Length;
            string potentialVerseString = text.Substring(index, text.Length - index).Trim();
            verseStart = potentialVerseString.Replace("verses", "").Replace("verse", "").Trim();

        }
        ExtractReference(text, Regex.Match(text, patterns[0]), bookPattern);
    }
    
    static int WordToNumber(string word)
    {
        word = word.ToLower();
        return numberWords.ContainsKey(word) ? numberWords[word] : 0;
    }

    static List<string> ExtractBibleReferences(string text)
    {
        var results = new List<string>();

        // Match book names, optional "chapter X" (word or digit), optional "verse X" (ranges)
        var bookPattern = string.Join("|", BibleBooks);
        var pattern = $@"\b({bookPattern})\s*(?:chapter\s+(\d+|\w+))?,?\s*(?:verse\s+(\d+)(?:\s*(?:-|,?\s*and\s*)\s*(\d+))?)?";

        var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);

        foreach (Match match in matches)
        {
            string book = match.Groups[1].Value.Trim();
            string chapterStr = match.Groups[2].Success ? match.Groups[2].Value.Trim() : "";
            int chapter = 0;

            if (!string.IsNullOrEmpty(chapterStr))
            {
                if (int.TryParse(chapterStr, out int c))
                    chapter = c;
                else
                    chapter = WordToNumber(chapterStr);
            }

            string verseStart = match.Groups[3].Success ? match.Groups[3].Value : "";
            string verseEnd = match.Groups[4].Success ? match.Groups[4].Value : "";

            string reference = book;
            if (chapter > 0)
                reference += " " + chapter;

            if (!string.IsNullOrEmpty(verseStart))
            {
                reference += ":" + verseStart;
                if (!string.IsNullOrEmpty(verseEnd))
                    reference += "-" + verseEnd;
            }

            results.Add(reference);
        }

        return results;
    }

    private List<string> ExtractPassages4(string text)
    {
        var passages = new List<string>();
        var bookPattern = string.Join("|", BibleBookVariants.Keys
            .OrderByDescending(k => k.Length)
            .Select(Regex.Escape));

        var bookMatches = Regex.Matches(text, $@"\b({bookPattern})\b", RegexOptions.IgnoreCase);

        foreach (Match bookMatch in bookMatches)
        {
            string bookRaw = bookMatch.Value;
            string book = BibleBookVariants[bookRaw];
            // Preview next 50 characters for numbers/ranges
            int startIndex = bookMatch.Index + bookMatch.Length;
            var hopefulRef = GetRef(text, startIndex);
            if(!string.IsNullOrEmpty(hopefulRef))
            {
                passages.Add(book + " " + hopefulRef);
                continue;
            }


                
            int i = 0;

            
            //foreach (Match val in scriptureRefMatches)
            //{
            //    string chapter = val.Groups["chapter"].Success ? val.Groups["chapter"].Value :
            //                     val.Groups["chapter2"].Success ? val.Groups["chapter2"].Value :
            //                     val.Groups["chapter3"].Success ? val.Groups["chapter3"].Value : null;

            //    string verses = val.Groups["verses"].Success ? val.Groups["verses"].Value :
            //                    val.Groups["verses2"].Success ? val.Groups["verses2"].Value : null;

            //    string endChapter = val.Groups["chapter4"].Success ? val.Groups["chapter4"].Value : null;

            //    string scripture = chapter;

            //    if (!string.IsNullOrEmpty(verses))
            //    {
            //        // Normalize commas to dash if consecutive verses
            //        var verseList = verses.Split(',', StringSplitOptions.RemoveEmptyEntries)
            //                              .Select(x => x.Trim())
            //                              .ToList();
            //        if (verseList.Count > 1)
            //        {
            //            scripture += ":" + SummariseVerses(verseList.Select(int.Parse).ToList());
            //        }
            //        else
            //        {
            //            scripture += ":" + verses;
            //        }
            //    }
            //    else if (!string.IsNullOrEmpty(endChapter))
            //    {
            //        // chapter1 and chapter2 -> range
            //        scripture += "-" + endChapter;
            //    }

            //    passages.Add(book + " " + scripture);
            //}

            //    var scriptureRefMatches= Regex.Matches(preview, @"\b(\d{1,3})(?::(\d{1,3}(?:-\d{1,3})?(?:,\d{1,3}(?:-\d{1,3})?)*))?|\s+verses?\s+(\d{1,3}(?:\s*(?:and|,)\s*\d{1,3})*)?\b", RegexOptions.IgnoreCase);
            //    if (scriptureRefMatches.Any())
            //    {

            //        var val = scriptureRefMatches.Where(refs => refs.Success).First();
            //        var chapter = val.Groups[1].Value;

            //        var verses = val.Groups[2].Value;
            //        string verseSummary = "";
            //        string scripture = chapter;
            //        if (verses.Contains(","))
            //        {
            //            var vlist = verses.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
            //            verseSummary = SummariseVerses(vlist);
            //            scripture = chapter + ":" + verseSummary;
            //        }
            //        else
            //        {
            //            if(string.IsNullOrEmpty(verses))
            //            {
            //                if (scriptureRefMatches.Skip(1).Any(x => BibleFormat(x.Value).Success))
            //                {
            //                    List<string> additionalVerses = new List<string>();
            //                    foreach (var match in scriptureRefMatches.Skip(1).Where(x => BibleFormat(x.Value).Success))
            //                    {
            //                        var text1 = BibleFormat(match.Value)!;
            //                        additionalVerses.Add(text1.Value);

            //                    }
            //                    scripture = chapter+  ":" + SummariseVerses(additionalVerses.Select(x => Convert.ToInt32(x)).ToList());
            //                }
            //            }
            //            else
            //            {
            //                scripture = chapter + ":" + verses;
            //            }
            //        }

            //        //if (scriptureRefMatches.Skip(1).Any(x => BibleFormat(x.Value).Success))
            //        //{
            //        //    List<string> additionalVerses = new List<string>();
            //        //    foreach(var match in scriptureRefMatches.Skip(1).Where(x => BibleFormat(x.Value).Success))
            //        //    {
            //        //        var text1 = BibleFormat(match.Value)!;
            //        //        additionalVerses.Add(text1.Value);

            //        //    }
            //        //    scripture += ":" + scriptureRefMatches.Skip(1).Where(refs => BibleFormat(refs.Value).Success).First().Groups[3].Value;
            //        //}
            //        passages.Add(book + " " + scripture);
            //    }
            //}
        }
        

        return passages;

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

    private Match ContainsBibleChapterAndVerseFormat(string reference)
    {
        var result = Regex.Match(reference, @"\d{1,3}:\d{1,3}");
        return result;
    }

    private Match ContainsBibleChapterAndVerseInLetterFormat(string reference)
    {
        var result = Regex.Match(reference.Trim(), @"chapters?\s+\d{1,3}\s*,?\s*verses?\s+\d{1,3}", RegexOptions.Singleline);
        return result;
    }

    string GetRef(string text, int startIndex)
    {
        int previewLength = Math.Min(50, text.Length - startIndex);
        string preview = text.Substring(startIndex, previewLength);
        var bookPattern = string.Join("|", BibleBookVariants.Keys
            .OrderByDescending(k => k.Length)
            .Select(Regex.Escape));
        var matches = Regex.Matches(preview, $@"\b({bookPattern})\b", RegexOptions.IgnoreCase);
        if (matches.Any())
        {
            var matchIndex = matches.First().Index;
            var newText = preview.Substring(0, matchIndex);
            if(newText.Contains(":") || newText.Contains("chapter") || newText.Contains("verse"))
                return GetRef(newText, 0);
            return "";
        }
        else
        {
            var acrossBibleChaptersCheck = AcrossChapterBibleFormat(preview);
            if(acrossBibleChaptersCheck.Success)
            {
                var acrossRefText = acrossBibleChaptersCheck.Value.Replace(" to ", "-");
                return acrossRefText;
            }
            else
            {
                var bf = BibleFormat(preview);
                if (bf.Success)
                {
                    var refText = bf.Value;
                    return ContainsMultipleVerses(ref refText);
                    //return bf.Value;
                }
                else
                {
                    preview = ContainsBibleChapterAndVerseInLetterFormat(preview).Value;
                }

                    ConvertBibleFormat(ref preview);
                
                return preview;
            }
                
        }
    }

    private Match BibleFormat(string reference)
    {
        //var result = Regex.Match(reference, @"(?<=verse\s+)\d{1,3}|\d{1,3}:\d{1,3}|\d{1,3}\s+and\s+\d{1,3}|\d{1,3}\s+to\s+\d{1,3}");
        var result = Regex.Match(reference, @"\d{1,3}:\d{1,3}(?:-\d{1,3}:\d{1,3}|\-\d{1,3})?(?:,\d{1,3}(?:-\d{1,3})?)*");
        return result;
    }

    private Match AcrossChapterBibleFormat(string reference)
    {
        var result = Regex.Match(reference, @"\d{1,3}:\d{1,3}\s+to\s+\d{1,3}:\d{1,3}");
        return result;
    }

    private string ContainsMultipleVerses(ref string reference)
    {
        if (Regex.IsMatch(reference, @"\d{1,3}:\d{1,3}-\d{1,3}:\d{1,3}"))
            return reference;
        //var result = Regex.Match(reference, @"(?<=verse\s+)\d{1,3}|\d{1,3}:\d{1,3}|\d{1,3}\s+and\s+\d{1,3}|\d{1,3}\s+to\s+\d{1,3}");
        var result = Regex.Match(reference, @"\d{1,3}:\d{1,3}(?:-\d{1,3})?(?:,\d{1,3}(?:-\d{1,3})?)*");
        if (result.Success)
        {
            var colonIndex = reference.IndexOf(":");
            var chapter = reference.Substring(0, colonIndex + 1);
            var verses = reference.Substring(colonIndex + 1);
            if(verses.Contains(","))
                return chapter + SummariseVerses(verses.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList());

            return chapter + verses;
        }
        else
        {
            return "";
        }

        
    }

    private void ConvertBibleFormat(ref string reference)
    {
        //var chapterAndVerseResult =Regex.Matches(reference, @"(?<=chapter )\d{1,3} verses? \d{1,3}|\d{1,3}\s+and\s+\d{1,3}|\d{1,3}\s+to\s+\d{1,3}");

        //if(chapterAndVerseResult.Any())
        //{
        //    reference = chapterAndVerseResult.First().Value;
        //}

        //var verseResult = Regex.Matches(reference, @"verses? \d{1,3}|\d{1,3}\s+and\s+\d{1,3}|\d{1,3}\s+to\s+\d{1,3}");
        //foreach(var match in verseResult.Where(x => x.Success))
        //{
        //    var text = match.Value;
        //    var newText = text.Replace("verse", "").Replace("and", ",").Replace("to", "-");
        //    reference = reference.Replace(text, newText);
        //}
        //var result = Regex.Match(reference, @"\d{1,3}:\d{1,3}");
        var match = Regex.Match(reference, @"chapter (\d{1,3}) verses? ((?:\d{1,3}(?: (?:to|and) \d{1,3})?))");

        if (match.Success)
        {
            var chapter = match.Groups[1].Value;
            var verses = match.Groups[2].Value;

            // Normalize the verses
            verses = verses.Replace("and", ",").Replace("to", "-");

            // Combine into standard format
            reference = $"{chapter}:{verses}";
        }
        else
        {
            var match2 = Regex.Match(reference, @"chapter\s+(\d{1,3}),?\s*verse\s+(\d{1,3})", RegexOptions.IgnoreCase);
            var newRef = match2.Value;
            var chapter = int.Parse(match2.Groups[1].Value);
            var verse = int.Parse(match2.Groups[2].Value);
            reference = $"{chapter:D2}:{verse:D2}";

        }
    }

    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "Lord","God","prayer","freedom","thanksgiving","Amen","He","They","But","Well"
    };

    public List<string> ExtractPassages3(string text)
    {
        var passages = new List<string>();

        // Normalize text
        string normalizedText = text.Replace(" and ", ",", StringComparison.OrdinalIgnoreCase);
        normalizedText = Regex.Replace(normalizedText, @"verse[s]*\s+", "", RegexOptions.IgnoreCase);
        normalizedText = Regex.Replace(normalizedText, @"chapter\s+", "", RegexOptions.IgnoreCase);

        // Build book regex
        var bookPattern = string.Join("|", BibleBookVariants.Keys
            .OrderByDescending(k => k.Length)
            .Select(Regex.Escape));

        var bookMatches = Regex.Matches(normalizedText, $@"\b({bookPattern})\b", RegexOptions.IgnoreCase);

        foreach (Match bookMatch in bookMatches)
        {
            string bookRaw = bookMatch.Value;
            string book = BibleBookVariants[bookRaw];

            // Preview next 50 characters for numbers/ranges
            int startIndex = bookMatch.Index + bookMatch.Length;
            int previewLength = Math.Min(50, normalizedText.Length - startIndex);
            string preview = normalizedText.Substring(startIndex, previewLength);

            var tokens = Regex.Split(preview, @"[\s,]+").Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            int? chapter = null;
            List<int> verses = new();
            int? endChapter = null;
            List<int> endVerses = new();
            bool inRange = false;

            foreach (var token in tokens)
            {
                if (StopWords.Contains(token)) break;

                if (token.Equals("to", StringComparison.OrdinalIgnoreCase) || token == "-")
                {
                    inRange = true;
                    continue;
                }

                if (token.Contains(":"))
                {
                    var parts = token.Split(':');
                    if (int.TryParse(parts[0], out int c)) chapter ??= c;
                    if (int.TryParse(parts[1], out int v))
                    {
                        if (!inRange)
                            verses.Add(v);
                        else
                            endVerses.Add(v);
                    }
                    continue;
                }

                if (int.TryParse(token, out int number))
                {
                    if (!inRange)
                    {
                        if (chapter == null) chapter = number;
                        else verses.Add(number);
                    }
                    else
                    {
                        if (endChapter == null && chapter != null && verses.Count == 0)
                        {
                            endChapter = number;
                        }
                        else
                        {
                            endVerses.Add(number);
                        }
                    }
                }
            }

            // Collapse verses to ranges
            string versePart = CollapseToRanges(verses);
            string endVersePart = CollapseToRanges(endVerses);

            string passage = book;
            if (chapter != null)
            {
                passage += " " + chapter;
                if (!string.IsNullOrEmpty(versePart))
                    passage += ":" + versePart;
            }
            if (endChapter != null)
            {
                passage += "-" + endChapter;
                if (!string.IsNullOrEmpty(endVersePart))
                    passage += ":" + endVersePart;
            }
            else if (!string.IsNullOrEmpty(endVersePart))
            {
                passage += "-" + endVersePart;
            }

            passages.Add(passage);
        }

        return passages;
    }

    private static string CollapseToRanges(List<int> numbers)
    {
        if (numbers.Count == 0) return "";

        numbers.Sort();
        var ranges = new List<string>();
        for (int i = 0; i < numbers.Count; i++)
        {
            int start = numbers[i];
            int end = start;
            while (i + 1 < numbers.Count && numbers[i + 1] == end + 1)
            {
                end = numbers[i + 1];
                i++;
            }
            ranges.Add(start == end ? start.ToString() : $"{start}-{end}");
        }
        return string.Join(",", ranges);
    }


    static List<string> ExtractScriptureReferences(string text)
    {
        var results = new List<string>();

        // Normalize text: unify "and", remove "chapter"/"verse(s)"
        text = text.Replace(" and ", ",");
        text = Regex.Replace(text, @"verse[s]*\s+", ":", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"chapter\s+", "", RegexOptions.IgnoreCase);

        // Build a regex for all books
        string bookPattern = string.Join("|", BibleBooks.OrderByDescending(b => b.Length).Select(Regex.Escape));

        string pattern = $@"
            (?<book>{bookPattern})
            \s*
            (?<startChapter>\d+)
            (?:
                :(?<startVerses>[\d,]+)
            )?
            (?:\s*(?:to|-)
                (?:(?<endChapter>\d+):)?
                (?<endVerses>[\d,]+)?
            )?
        ";

        var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        foreach (Match m in matches)
        {
            string book = m.Groups["book"].Value;
            int startChapter = int.Parse(m.Groups["startChapter"].Value);
            string startVerses = m.Groups["startVerses"].Success ? m.Groups["startVerses"].Value : "";
            int? endChapter = m.Groups["endChapter"].Success ? int.Parse(m.Groups["endChapter"].Value) : (int?)null;
            string endVerses = m.Groups["endVerses"].Success ? m.Groups["endVerses"].Value : "";

            // Summarize consecutive verses into ranges
            string summarizedStartVerses = SummarizeVerses(startVerses);
            string summarizedEndVerses = SummarizeVerses(endVerses);

            string passage;
            if (!string.IsNullOrEmpty(summarizedEndVerses))
            {
                if (endChapter.HasValue)
                    passage = $"{book} {startChapter}:{summarizedStartVerses}-{endChapter}:{summarizedEndVerses}";
                else
                    passage = $"{book} {startChapter}:{summarizedStartVerses}-{summarizedEndVerses}";
            }
            else if (!string.IsNullOrEmpty(summarizedStartVerses))
            {
                passage = $"{book} {startChapter}:{summarizedStartVerses}";
            }
            else
            {
                passage = $"{book} {startChapter}";
            }

            results.Add(passage);
        }

        return results;
    }

    static string SummarizeVerses(string verses)
    {
        if (string.IsNullOrWhiteSpace(verses)) return "";

        var numbers = new List<int>();

        foreach (var part in verses.Split(','))
        {
            if (int.TryParse(part, out int n))
                numbers.Add(n);
        }

        numbers.Sort();

        var ranges = new List<string>();
        for (int i = 0; i < numbers.Count; i++)
        {
            int start = numbers[i];
            int end = start;
            while (i + 1 < numbers.Count && numbers[i + 1] == end + 1)
            {
                end = numbers[i + 1];
                i++;
            }

            ranges.Add(start == end ? start.ToString() : $"{start}-{end}");
        }

        return string.Join(",", ranges);
    }

    public List<string> ExtractPassages2(string text)
    {
        var references = new List<ParsedScriptureReference>();
        string pattern = @"(?<book>[1-3]?\s?[A-Za-z]+)\s*(?:chapter\s*)?(?<chapter>\d+)?(?:[, ]*verse[s]*\s*(?<verses>[\d,\s\-and]+))?(?::(?<versesColon>[\d,\s\-]+))?";

        var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
        foreach (Match match in matches)
        {
            
            // Only accept exact matches in BibleBookVariants
            string book = match.Groups["book"].Value;
            if (!BibleBooks.Any(bb => bb.Equals(book))) continue;
            string chapter = match.Groups["chapter"].Success ? match.Groups["chapter"].Value : null;

            List<string> allVerses = new List<string>();

            // Handle "verse 10 and 11" or "verse 10,11"
            if (match.Groups["verses"].Success)
            {
                string versesRaw = match.Groups["verses"].Value.Replace("and", ",");
                foreach (var v in versesRaw.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    allVerses.Add(v);
                }
            }

            // Handle "Chapter:Verse" format like Romans 3:23
            if (match.Groups["versesColon"].Success)
            {
                string versesRaw = match.Groups["versesColon"].Value;
                foreach (var v in versesRaw.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    allVerses.Add(v);
                }
            }


        }

        return references.Select(r => r.ToString()).ToList();
    }

    static readonly Dictionary<string, string> BibleBookVariants = new(StringComparer.OrdinalIgnoreCase)
    {
        // Old Testament
        {"Genesis", "Genesis"},
        {"Exodus", "Exodus"},
        {"Leviticus", "Leviticus"},
        {"Numbers", "Numbers"},
        {"Deuteronomy", "Deuteronomy"},
        {"Joshua", "Joshua"},
        {"Judges", "Judges"},
        {"Ruth", "Ruth"},
        {"1 Samuel", "1 Samuel"}, {"1st Samuel", "1 Samuel"}, {"First Samuel", "1 Samuel"},
        {"2 Samuel", "2 Samuel"}, {"2nd Samuel", "2 Samuel"}, {"Second Samuel", "2 Samuel"},
        {"1 Kings", "1 Kings"}, {"1st Kings", "1 Kings"}, {"First Kings", "1 Kings"},
        {"2 Kings", "2 Kings"}, {"2nd Kings", "2 Kings"}, {"Second Kings", "2 Kings"},
        {"1 Chronicles", "1 Chronicles"}, {"1st Chronicles", "1 Chronicles"}, {"First Chronicles", "1 Chronicles"},
        {"2 Chronicles", "2 Chronicles"}, {"2nd Chronicles", "2 Chronicles"}, {"Second Chronicles", "2 Chronicles"},
        {"Ezra", "Ezra"},
        {"Nehemiah", "Nehemiah"},
        {"Esther", "Esther"},
        {"Job", "Job"},
        {"Psalms", "Psalms"}, {"Psalm", "Psalms"},
        {"Proverbs", "Proverbs"},
        {"Ecclesiastes", "Ecclesiastes"},
        {"Song of Solomon", "Song of Solomon"}, {"Song of Songs", "Song of Solomon"},
        {"Isaiah", "Isaiah"},
        {"Jeremiah", "Jeremiah"},
        {"Lamentations", "Lamentations"},
        {"Ezekiel", "Ezekiel"},
        {"Daniel", "Daniel"},
        {"Hosea", "Hosea"},
        {"Joel", "Joel"},
        {"Amos", "Amos"},
        {"Obadiah", "Obadiah"},
        {"Jonah", "Jonah"},
        {"Micah", "Micah"},
        {"Nahum", "Nahum"},
        {"Habakkuk", "Habakkuk"},
        {"Zephaniah", "Zephaniah"},
        {"Haggai", "Haggai"},
        {"Zechariah", "Zechariah"},
        {"Malachi", "Malachi"},

        // New Testament
        {"Matthew", "Matthew"},
        {"Mark", "Mark"},
        {"Luke", "Luke"},
        {"John", "John"},
        {"Acts", "Acts"},
        {"Romans", "Romans"},
        {"1 Corinthians", "1 Corinthians"}, {"1st Corinthians", "1 Corinthians"}, {"First Corinthians", "1 Corinthians"},
        {"2 Corinthians", "2 Corinthians"}, {"2nd Corinthians", "2 Corinthians"}, {"Second Corinthians", "2 Corinthians"},
        {"Galatians", "Galatians"},
        {"Ephesians", "Ephesians"},
        {"Philippians", "Philippians"},
        {"Colossians", "Colossians"},
        {"1 Thessalonians", "1 Thessalonians"}, {"1st Thessalonians", "1 Thessalonians"}, {"First Thessalonians", "1 Thessalonians"},
        {"2 Thessalonians", "2 Thessalonians"}, {"2nd Thessalonians", "2 Thessalonians"}, {"Second Thessalonians", "2 Thessalonians"},
        {"1 Timothy", "1 Timothy"}, {"1st Timothy", "1 Timothy"}, {"First Timothy", "1 Timothy"},
        {"2 Timothy", "2 Timothy"}, {"2nd Timothy", "2 Timothy"}, {"Second Timothy", "2 Timothy"},
        {"Titus", "Titus"},
        {"Philemon", "Philemon"},
        {"Hebrews", "Hebrews"},
        {"James", "James"},
        {"1 Peter", "1 Peter"}, {"1st Peter", "1 Peter"}, {"First Peter", "1 Peter"},
        {"2 Peter", "2 Peter"}, {"2nd Peter", "2 Peter"}, {"Second Peter", "2 Peter"},
        {"1 John", "1 John"}, {"1st John", "1 John"}, {"First John", "1 John"},
        {"2 John", "2 John"}, {"2nd John", "2 John"}, {"Second John", "2 John"},
        {"3 John", "3 John"}, {"3rd John", "3 John"}, {"Third John", "3 John"},
        {"Jude", "Jude"},
        {"Revelation", "Revelation"}, {"Revelations", "Revelation"}
    };


    static string NormalizeBook(string rawBook, bool hasChapterOrVerse)
    {
        if (BibleBookVariants.TryGetValue(rawBook, out string canonical))
            return canonical;

        if (hasChapterOrVerse)
            return FindClosestBook(rawBook);

        return null;
    }

    static string FindClosestBook(string input)
    {
        return BibleBookVariants.Values.Distinct()
            .FirstOrDefault(b => b.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    static string CombineVerses(string versesText)
    {
        if (string.IsNullOrWhiteSpace(versesText)) return "";
        versesText = versesText.Replace("and", ",").Replace(" ", "");
        return CollapseToRanges(versesText);
    }

    // Parse colon range like "20-51:5"
    static void ParseColonRange(string range, out string startVerses, out int? endChapter, out string endVerses)
    {
        startVerses = "";
        endChapter = null;
        endVerses = "";

        if (string.IsNullOrWhiteSpace(range)) return;

        var parts = range.Split('-');
        if (parts.Length == 1)
        {
            startVerses = parts[0];
        }
        else if (parts.Length == 2)
        {
            startVerses = parts[0];

            // check if second part contains colon for cross-chapter
            if (parts[1].Contains(":"))
            {
                var sub = parts[1].Split(':');
                if (int.TryParse(sub[0], out int c)) endChapter = c;
                endVerses = sub[1];
            }
            else
            {
                endVerses = parts[1];
            }
        }
    }

    static string CollapseToRanges(string verses)
    {
        var numbers = new List<int>();
        foreach (var part in verses.Split(','))
        {
            if (int.TryParse(part, out int n))
                numbers.Add(n);
            else if (part.Contains('-'))
                numbers.AddRange(ParseRange(part));
        }

        numbers.Sort();
        var result = new List<string>();
        for (int i = 0; i < numbers.Count; i++)
        {
            int start = numbers[i];
            int end = start;
            while (i + 1 < numbers.Count && numbers[i + 1] == end + 1)
            {
                end = numbers[i + 1];
                i++;
            }
            result.Add(start == end ? start.ToString() : $"{start}-{end}");
        }

        return string.Join(",", result);
    }

    static List<int> ParseRange(string range)
    {
        var parts = range.Split('-');
        if (parts.Length != 2) return new List<int>();
        if (int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end))
        {
            var list = new List<int>();
            for (int i = start; i <= end; i++)
                list.Add(i);
            return list;
        }
        return new List<int>();
    }
}
