using System.ComponentModel.DataAnnotations;

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
