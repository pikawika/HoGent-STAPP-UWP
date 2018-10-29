using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class OpenDay
    {
        [Key]
        public int OpenDayId { get; set; }
        public int DayOfTheWeek { get; set; }
        public List<OpenHour> OpenHours { get; set; }
    }
}
