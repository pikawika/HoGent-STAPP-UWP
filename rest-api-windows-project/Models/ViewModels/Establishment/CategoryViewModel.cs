using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.Establishment
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }
    }
}
