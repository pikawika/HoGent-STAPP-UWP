using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.ViewModels.Establishment
{
    public class SocialMediaViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Url { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }
    }
}
