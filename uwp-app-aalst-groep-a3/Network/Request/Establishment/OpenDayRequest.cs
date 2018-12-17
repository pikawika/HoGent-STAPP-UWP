using System.Collections.Generic;

namespace uwp_app_aalst_groep_a3.Network.Request.Establishment
{
    public class OpenDayRequest
    {
        public int DayOfTheWeek { get; set; }
        public List<OpenHourRequest> OpenHours { get; set; }
    }
}
