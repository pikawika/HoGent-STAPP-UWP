using System.ComponentModel.DataAnnotations;

namespace uwp_app_aalst_groep_a3.Models
{
    public class OpenHour
    {
        public int OpenHourId { get; set; }

        public int StartHour { get; set; }
        public int Startminute { get; set; }

        public int EndHour { get; set; }
        public int EndMinute { get; set; }
    }
}
