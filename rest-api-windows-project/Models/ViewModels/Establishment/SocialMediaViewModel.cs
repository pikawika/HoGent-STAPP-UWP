using System.ComponentModel.DataAnnotations;

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
