using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using stappBackend.Models.ViewModels.Attachments;

namespace stappBackend.Models.ViewModels.Establishment
{
    public class AddEstablishmentViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int? CompanyId { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public List<CategoryViewModel> Categories { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public List<OpenDayViewModel> OpenDays { get; set; }

        public List<SocialMediaViewModel> SocialMedias { get; set; }

        public List<ExceptionalDayViewModel> ExceptionalDays { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public List<FileViewModel> Images { get; set; }
    }
}
