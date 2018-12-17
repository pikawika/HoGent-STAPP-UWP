using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using stappBackend.Models.ViewModels.Attachments;

namespace stappBackend.Models.ViewModels.Promotion
{
    public class AddPromotionViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int? EstablishmentId { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public List<FileViewModel> Images { get; set; }

        public List<FileViewModel> Attachments { get; set; }
    }
}
