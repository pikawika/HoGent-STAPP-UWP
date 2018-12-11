using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using stappBackend.Models.ViewModels.Establishment;

namespace stappBackend.Models.ViewModels.Event
{
    public class AddEventViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public int? establishmentId { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Upload)]
        public IFormCollection Images { set; get; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Upload)]
        public IFormCollection Attachments { set; get; }
    }
}
