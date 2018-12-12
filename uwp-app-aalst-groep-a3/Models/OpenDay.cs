using System.Collections.Generic;

namespace uwp_app_aalst_groep_a3.Models
{
    public class OpenDay
    {
        public int OpenDayId { get; set; }
        public int DayOfTheWeek { get; set; }
        public List<OpenHour> OpenHours { get; set; } = new List<OpenHour>();
    }
}
