using ChurchSermonsMetaData.Interfaces;
using ChurchSermonsMetaData.Models;
using System.Globalization;

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
            DateTime? date = ExtractDateFromFileName(name);

            if (date.HasValue)
            {
                if (!IsRecordedOnSunday(date.Value))
                    return new SermonServiceAndDate { Service = "Midweek Service", Date = date };
                else
                {
                    if (date.Value.TimeOfDay < new TimeSpan(14, 0, 0))
                        return new SermonServiceAndDate { Service = "Morning Service", Date = date };
                    else
                        return new SermonServiceAndDate { Service = "Evening Service", Date = date };
                }
            } 
            else 
                return new SermonServiceAndDate { Service = "N/A", Date = date };
        }

        private DateTime? ExtractDateFromFileName(string name)
        {
            string dateTimePart = name.Split('_')[0];

            // Extract parts
            string datePart = dateTimePart.Substring(0, 8);   // 26042026
            string timePart = dateTimePart.Substring(8, 6);    // 111517

            // Parse to DateTime
            return DateTime.ParseExact(
                datePart + timePart,
                "ddMMyyyyHHmmss",
                CultureInfo.InvariantCulture
            );
        }
        private bool IsRecordedOnSunday(DateTime date)
           => date.DayOfWeek == DayOfWeek.Sunday;
    }
}
