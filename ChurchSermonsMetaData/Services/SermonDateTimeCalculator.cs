using ChurchSermonsMetaData.Interfaces;
using ChurchSermonsMetaData.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChurchSermonsMetaData.Services
{
    internal class SermonDateTimeCalculator : ISermonDateTimeCalculator
    {
        public SermonDateTimeCalculator() { }

        public SermonServiceAndDate GetSermonServiceInfoAsync(FileInfo file)
        {
            if (file == null || string.IsNullOrWhiteSpace(file.Name))
                return new SermonServiceAndDate { Service = "Other" };

            string name = Path.GetFileNameWithoutExtension(file.Name);

            // Try to find a date in the filename
            // Examples: "2025-10-12", "12-10-2025", "12 Oct 2025"
            var dateMatch = Regex.Match(name, @"(\d{4}-\d{2}-\d{2}|\d{2}-\d{2}-\d{4}|\d{1,2}\s+\w+\s+\d{4})");
            DateTime? date = null;

            if (dateMatch.Success)
            {
                if (DateTime.TryParse(dateMatch.Value, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out DateTime parsed))
                {
                    date = parsed;
                }
            }

            // Determine service type
            string service = "Other";
            if (name.Contains("Morning", StringComparison.OrdinalIgnoreCase))
                service = "Sunday Morning Service";
            else if (name.Contains("Evening", StringComparison.OrdinalIgnoreCase))
                service = "Sunday Evening Service";
            else if (name.Contains("Midweek", StringComparison.OrdinalIgnoreCase))
                service = "Midweek Service";

            return new SermonServiceAndDate
            {
                Date = date,
                Service = service
            };


        }
    }
}
