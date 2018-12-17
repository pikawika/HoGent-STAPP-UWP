using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.Customer
{
    public class ModifySubscriptionViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int EstablishmentId { get; set; }
    }
}
