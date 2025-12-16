using System;
using System.Collections.Generic;
using System.Text;

namespace SermonData.Interfaces
{
    public interface IBiblePassageExtractor
    {
        List<string> ExtractPassages(string text);
    }
}
