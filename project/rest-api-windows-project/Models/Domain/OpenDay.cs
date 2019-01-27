using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class OpenDay
    {
        [Key]
        public int OpenDayId { get; set; }
        public int DayOfTheWeek { get; set; }
        public List<OpenHour> OpenHours { get; set; } = new List<OpenHour>();
    }
}
