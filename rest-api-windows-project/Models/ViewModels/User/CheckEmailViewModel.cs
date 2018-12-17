using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.User
{
    public class CheckEmailViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [EmailAddress(ErrorMessage = "Geen geldig {0} ingevoerd")]
        public string Email { get; set; }
    }
}
