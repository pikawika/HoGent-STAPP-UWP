using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.User
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Password, ErrorMessage = "Geen geldig {0} ingevoerd")]
        [StringLength(30, ErrorMessage = "Het wachtwoord moet tussen {2} en {1} karakters lang zijn.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
