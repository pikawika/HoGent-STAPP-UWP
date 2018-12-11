using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
