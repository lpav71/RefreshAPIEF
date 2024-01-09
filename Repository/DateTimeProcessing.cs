using RefreshAPIEF.Data;
using System.Globalization;

namespace RefreshAPIEF.Repository
{
    public class DateTimeProcessing
    {
        public DateTimeProcessing()
        {

        }
        public DateTime ParseDateTime(string time)
        {
            return DateTime.ParseExact(time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public DateTime GetUtcTime(int UTC) 
        {
            var today = DateTime.Now;
            return today.AddMinutes(UTC);
        }
    }
}
