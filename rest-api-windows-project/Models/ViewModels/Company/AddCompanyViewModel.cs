using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.ViewModels.Company
{
    public class AddCompanyViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }
    }
}
