using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.ViewModels.Customer
{
    public class AddSubscriptionViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int establishmentId { get; set; }
    }
}
