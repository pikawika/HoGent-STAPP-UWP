using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models.ViewModels.Attachments
{
    public class FileViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string FullFileName { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Base64File { get; set; }
    }
}
