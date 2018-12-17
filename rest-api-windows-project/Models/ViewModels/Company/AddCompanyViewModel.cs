using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.Company
{
    public class AddCompanyViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }
    }
}
