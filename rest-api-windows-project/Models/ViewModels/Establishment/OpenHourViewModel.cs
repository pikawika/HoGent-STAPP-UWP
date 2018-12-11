using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.ViewModels.Establishment
{
    public class OpenHourViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int StartHour { get; set; }
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int Startminute { get; set; }
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int EndHour { get; set; }
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int EndMinute { get; set; }
    }
}
