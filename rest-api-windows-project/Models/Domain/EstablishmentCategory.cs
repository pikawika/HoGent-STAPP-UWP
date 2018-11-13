using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class EstablishmentCategory
    {
        [Key]
        public int EstablishmentCategoryId { get; private set; }
        public Category Category { get; set; }
    }
}
