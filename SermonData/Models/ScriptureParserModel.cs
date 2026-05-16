using System;
using System.Collections.Generic;
using System.Text;

namespace SermonData.Models
{
    public record ScriptureParserModel(string Pattern, bool OverChapterRangeScripture, bool NoVerses = false);
}
