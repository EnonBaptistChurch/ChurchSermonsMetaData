using System;
using System.Collections.Generic;
using System.Text;

namespace SermonData.Models
{
    public class SermonMetaData
    {
        public string Title { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public List<string> BiblePassages { get; set; } = new List<string>();
    }
}
