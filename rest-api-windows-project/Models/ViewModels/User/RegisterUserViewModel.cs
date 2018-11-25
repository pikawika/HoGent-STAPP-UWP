using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.ViewModels.User
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [EmailAddress(ErrorMessage = "Geen geldig {0} ingevoerd")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Text, ErrorMessage = "Geen geldig {0} ingevoerd")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Text, ErrorMessage = "Geen geldig {0} ingevoerd")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Logingegevens zijn verplicht.")]
        public RegisterLoginViewModel Login { get; set; }
    }
}
