using System;
using System.Collections.Generic;
using System.Text;

namespace SermonData.Models
{
    public class SermonMetaData
    {
        public string Title { get; set; }
        public string Series { get; set; }
        public List<string> BiblePassages { get; set; }
    }
}
