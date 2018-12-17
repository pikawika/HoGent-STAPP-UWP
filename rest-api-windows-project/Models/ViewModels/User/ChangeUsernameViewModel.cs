using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.User
{
    public class ChangeUsernameViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Text, ErrorMessage = "Geen geldig {0} ingevoerd")]
        public string Username { get; set; }
    }
}
