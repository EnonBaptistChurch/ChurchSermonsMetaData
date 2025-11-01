using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSermonsMetaData.Models
{
    internal record SermonServiceAndDate
    {
        public DateTime? Date { get; set; }
        public string Service {  get; set; } = string.Empty;
    }
}
