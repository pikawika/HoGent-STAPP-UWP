using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.Establishment
{
    public class ExceptionalDayViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public DateTime Day { get; set; }
        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Message { get; set; }
    }
}
