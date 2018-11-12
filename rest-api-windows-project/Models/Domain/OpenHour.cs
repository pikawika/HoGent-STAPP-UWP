using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class OpenHour
    {
        [Key]
        public int OpenHourId { get; set; }

        public int StartHour { get; set; }
        public int Startminute { get; set; }

        public int EndHour { get; set; }
        public int EndMinute { get; set; }
    }
}
