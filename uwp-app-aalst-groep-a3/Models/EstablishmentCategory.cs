using System.ComponentModel.DataAnnotations;

namespace uwp_app_aalst_groep_a3.Models
{
    public class EstablishmentCategory
    {
        public int EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
