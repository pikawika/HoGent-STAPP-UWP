using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
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
