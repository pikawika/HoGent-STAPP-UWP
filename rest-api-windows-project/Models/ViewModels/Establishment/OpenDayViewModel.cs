using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.Establishment
{
    public class OpenDayViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int DayOfTheWeek { get; set; }
        [Required(ErrorMessage = "{0} is verplicht.")]
        public List<OpenHourViewModel> OpenHours { get; set; }
    }
}
